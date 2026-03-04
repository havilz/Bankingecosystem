class TransferEntity {
  final int transactionId;
  final String referenceNumber;
  final double amount;
  final String targetAccountNumber;
  final String status;
  final String? description;
  final DateTime createdAt;

  const TransferEntity({
    required this.transactionId,
    required this.referenceNumber,
    required this.amount,
    required this.targetAccountNumber,
    required this.status,
    this.description,
    required this.createdAt,
  });
}
