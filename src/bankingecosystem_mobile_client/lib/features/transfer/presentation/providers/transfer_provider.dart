import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../core/network/dio_client.dart';
import '../../../../core/storage/token_storage.dart';
import '../../data/datasources/transfer_remote_datasource.dart';
import '../../data/repositories/transfer_repository_impl.dart';
import '../../domain/entities/transfer_entity.dart';
import '../../domain/repositories/transfer_repository.dart';

// ─── Repository Provider ───

final transferRepositoryProvider = Provider<TransferRepository>((ref) {
  final dioClient = ref.read(dioClientProvider);
  final datasource = TransferRemoteDatasource(dioClient);
  return TransferRepositoryImpl(datasource);
});

// ─── State ───

sealed class TransferState {
  const TransferState();
}

class TransferIdle extends TransferState {
  const TransferIdle();
}

class TransferLoading extends TransferState {
  const TransferLoading();
}

class TransferSuccess extends TransferState {
  final TransferEntity result;
  const TransferSuccess(this.result);
}

class TransferError extends TransferState {
  final String message;
  const TransferError(this.message);
}

// ─── Notifier ───

class TransferNotifier extends Notifier<TransferState> {
  @override
  TransferState build() => const TransferIdle();

  void reset() => state = const TransferIdle();

  Future<void> doTransfer({
    required String targetAccountNumber,
    required double amount,
    String? description,
  }) async {
    state = const TransferLoading();

    final tokenStorage = ref.read(tokenStorageProvider);
    final accountId = await tokenStorage.getAccountId();
    if (accountId == null) {
      state = const TransferError('Sesi tidak valid. Silakan login kembali.');
      return;
    }

    final repo = ref.read(transferRepositoryProvider);
    final result = await repo.doTransfer(
      accountId: accountId,
      targetAccountNumber: targetAccountNumber,
      amount: amount,
      description: description,
    );

    if (result.success && result.result != null) {
      state = TransferSuccess(result.result!);
    } else {
      state = TransferError(result.message);
    }
  }
}

final transferProvider = NotifierProvider<TransferNotifier, TransferState>(
  TransferNotifier.new,
);
