import '../../domain/entities/transfer_entity.dart';

class TransferModel {
  final int transactionId;
  final String referenceNumber;
  final double amount;
  final String? targetAccountNumber;
  final String status;
  final String? description;
  final DateTime createdAt;

  const TransferModel({
    required this.transactionId,
    required this.referenceNumber,
    required this.amount,
    this.targetAccountNumber,
    required this.status,
    this.description,
    required this.createdAt,
  });

  factory TransferModel.fromJson(Map<String, dynamic> json) {
    return TransferModel(
      transactionId: json['transactionId'] as int,
      referenceNumber: json['referenceNumber'] as String,
      amount: (json['amount'] as num).toDouble(),
      targetAccountNumber: json['targetAccountNumber'] as String?,
      status: json['status'] as String,
      description: json['description'] as String?,
      createdAt: DateTime.parse(json['createdAt'] as String),
    );
  }

  TransferEntity toEntity() => TransferEntity(
    transactionId: transactionId,
    referenceNumber: referenceNumber,
    amount: amount,
    targetAccountNumber: targetAccountNumber ?? '',
    status: status,
    description: description,
    createdAt: createdAt,
  );
}
