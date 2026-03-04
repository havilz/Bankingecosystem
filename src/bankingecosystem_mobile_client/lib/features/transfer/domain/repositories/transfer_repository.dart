import '../entities/transfer_entity.dart';

abstract class TransferRepository {
  Future<({bool success, String message, TransferEntity? result})> doTransfer({
    required int accountId,
    required String targetAccountNumber,
    required double amount,
    String? description,
  });
}
