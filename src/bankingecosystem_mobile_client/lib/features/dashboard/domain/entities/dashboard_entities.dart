class BalanceEntity {
  final int accountId;
  final String accountNumber;
  final String customerName;
  final double balance;

  const BalanceEntity({
    required this.accountId,
    required this.accountNumber,
    required this.customerName,
    required this.balance,
  });
}

class TransactionEntity {
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

  const TransactionEntity({
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
}
