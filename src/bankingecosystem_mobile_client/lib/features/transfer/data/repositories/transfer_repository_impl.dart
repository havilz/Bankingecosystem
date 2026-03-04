import '../../domain/entities/transfer_entity.dart';
import '../../domain/repositories/transfer_repository.dart';
import '../datasources/transfer_remote_datasource.dart';

class TransferRepositoryImpl implements TransferRepository {
  final TransferRemoteDatasource _datasource;

  TransferRepositoryImpl(this._datasource);

  @override
  Future<({bool success, String message, TransferEntity? result})> doTransfer({
    required int accountId,
    required String targetAccountNumber,
    required double amount,
    String? description,
  }) async {
    final result = await _datasource.doTransfer(
      accountId: accountId,
      targetAccountNumber: targetAccountNumber,
      amount: amount,
      description: description,
    );
    if (!result.success || result.data == null) {
      return (success: false, message: result.message, result: null);
    }
    return (
      success: true,
      message: result.message,
      result: result.data!.toEntity(),
    );
  }
}
