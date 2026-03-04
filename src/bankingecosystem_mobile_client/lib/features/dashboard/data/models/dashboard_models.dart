import '../../domain/entities/dashboard_entities.dart';

class BalanceModel {
  final double amount;
  final double balanceAfter;

  const BalanceModel({required this.amount, required this.balanceAfter});

  factory BalanceModel.fromJson(Map<String, dynamic> json) {
    return BalanceModel(
      amount: (json['amount'] as num?)?.toDouble() ?? 0.0,
      balanceAfter: (json['balanceAfter'] as num?)?.toDouble() ?? 0.0,
    );
  }
}

class TransactionModel {
  final int transactionId;
  final String transactionType;
  final double amount;
  final double balanceBefore;
  final double balanceAfter;
  final String referenceNumber;
  final String? targetAccountNumber;
  final String status;
  final String? description;
  final DateTime createdAt;

  const TransactionModel({
    required this.transactionId,
    required this.transactionType,
    required this.amount,
    required this.balanceBefore,
    required this.balanceAfter,
    required this.referenceNumber,
    this.targetAccountNumber,
    required this.status,
    this.description,
    required this.createdAt,
  });

  factory TransactionModel.fromJson(Map<String, dynamic> json) {
    return TransactionModel(
      transactionId: json['transactionId'] as int,
      transactionType: json['transactionType'] as String,
      amount: (json['amount'] as num).toDouble(),
      balanceBefore: (json['balanceBefore'] as num).toDouble(),
      balanceAfter: (json['balanceAfter'] as num).toDouble(),
      referenceNumber: json['referenceNumber'] as String,
      targetAccountNumber: json['targetAccountNumber'] as String?,
      status: json['status'] as String,
      description: json['description'] as String?,
      createdAt: DateTime.parse(json['createdAt'] as String),
    );
  }

  TransactionEntity toEntity() => TransactionEntity(
    transactionId: transactionId,
    transactionType: transactionType,
    amount: amount,
    balanceBefore: balanceBefore,
    balanceAfter: balanceAfter,
    referenceNumber: referenceNumber,
    targetAccountNumber: targetAccountNumber,
    status: status,
    description: description,
    createdAt: createdAt,
  );
}
