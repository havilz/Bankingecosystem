import '../../domain/entities/session_entity.dart';

class AuthResponseModel {
  final String token;
  final String accountNumber;
  final String customerName;
  final double balance;
  final int accountId;

  const AuthResponseModel({
    required this.token,
    required this.accountNumber,
    required this.customerName,
    required this.balance,
    required this.accountId,
  });

  factory AuthResponseModel.fromJson(Map<String, dynamic> json) {
    return AuthResponseModel(
      token: json['token'] as String,
      accountNumber: json['accountNumber'] as String,
      customerName: json['customerName'] as String,
      balance: (json['balance'] as num).toDouble(),
      accountId: json['accountId'] as int,
    );
  }

  SessionEntity toEntity(String email) => SessionEntity(
    token: token,
    accountId: accountId,
    customerName: customerName,
    accountNumber: accountNumber,
    balance: balance,
    email: email,
  );
}

class VerifyCardModel {
  final int cardId;
  final int accountId;
  final String accountNumber;
  final String customerName;
  final bool isBlocked;

  const VerifyCardModel({
    required this.cardId,
    required this.accountId,
    required this.accountNumber,
    required this.customerName,
    required this.isBlocked,
  });

  factory VerifyCardModel.fromJson(Map<String, dynamic> json) {
    return VerifyCardModel(
      cardId: json['cardId'] as int,
      accountId: json['accountId'] as int,
      accountNumber: json['accountNumber'] as String,
      customerName: json['customerName'] as String,
      isBlocked: json['isBlocked'] as bool,
    );
  }
}
