import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../core/network/dio_client.dart';
import '../../../../core/storage/token_storage.dart';
import '../../../dashboard/data/datasources/dashboard_remote_datasource.dart';
import '../../../dashboard/domain/entities/dashboard_entities.dart';

// ─── State ───

class HistoryState {
  final bool isLoading;
  final String? error;
  final List<TransactionEntity> transactions;

  const HistoryState({
    this.isLoading = true,
    this.error,
    this.transactions = const [],
  });

  HistoryState copyWith({
    bool? isLoading,
    String? error,
    List<TransactionEntity>? transactions,
  }) => HistoryState(
    isLoading: isLoading ?? this.isLoading,
    error: error,
    transactions: transactions ?? this.transactions,
  );
}

// ─── Allowed transaction types to show in Resi tab ───

const _allowedTypes = {'Transfer', 'Withdrawal'};

// ─── Notifier ───

class HistoryNotifier extends Notifier<HistoryState> {
  @override
  HistoryState build() {
    _loadHistory();
    return const HistoryState(isLoading: true);
  }

  Future<void> _loadHistory() async {
    final tokenStorage = ref.read(tokenStorageProvider);
    final accountId = await tokenStorage.getAccountId();

    if (accountId == null) {
      state = state.copyWith(
        isLoading: false,
        error: 'Sesi tidak ditemukan. Silakan login kembali.',
      );
      return;
    }

    final datasource = DashboardRemoteDatasource(ref.read(dioClientProvider));
    final result = await datasource.getHistory(accountId, pageSize: 50);

    if (!result.success) {
      state = state.copyWith(isLoading: false, error: result.message);
      return;
    }

    // Filter: only Transfer and Withdrawal
    final filtered = result.data
        .where((m) => _allowedTypes.contains(m.transactionType))
        .map((m) => m.toEntity())
        .toList();

    // Sort newest first
    filtered.sort((a, b) => b.createdAt.compareTo(a.createdAt));

    state = state.copyWith(isLoading: false, transactions: filtered);
  }

  Future<void> refresh() async {
    state = state.copyWith(isLoading: true, error: null);
    await _loadHistory();
  }
}

// ─── Provider ───

final historyProvider = NotifierProvider<HistoryNotifier, HistoryState>(
  HistoryNotifier.new,
);
