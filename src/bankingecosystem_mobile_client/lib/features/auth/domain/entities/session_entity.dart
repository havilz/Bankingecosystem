class SessionEntity {
  final String token;
  final int accountId;
  final String customerName;
  final String accountNumber;
  final double balance;
  final String email;

  const SessionEntity({
    required this.token,
    required this.accountId,
    required this.customerName,
    required this.accountNumber,
    required this.balance,
    required this.email,
  });
}
