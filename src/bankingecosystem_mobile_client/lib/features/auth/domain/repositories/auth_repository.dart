import '../../domain/entities/session_entity.dart';

abstract class AuthRepository {
  Future<({bool success, String message, int? cardId, String? customerName})>
  verifyCard(String cardNumber);

  Future<({bool success, String message, SessionEntity? session})>
  registerMbanking({
    required String cardNumber,
    required String email,
    required DateTime dateOfBirth,
    required String password,
  });

  Future<({bool success, String message, SessionEntity? session})>
  loginMbanking({required String email, required String password});

  Future<({bool success, String message})> verifyMbankingPin(
    int accountId,
    String pin,
  );

  Future<void> logout();
}
