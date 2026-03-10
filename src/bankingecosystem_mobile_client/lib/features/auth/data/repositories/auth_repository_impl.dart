import '../../../../core/storage/token_storage.dart';
import '../../domain/entities/session_entity.dart';
import '../../domain/repositories/auth_repository.dart';
import '../datasources/auth_remote_datasource.dart';

class AuthRepositoryImpl implements AuthRepository {
  final AuthRemoteDatasource _datasource;
  final TokenStorage _tokenStorage;

  AuthRepositoryImpl(this._datasource, this._tokenStorage);

  @override
  Future<({bool success, String message, int? cardId, String? customerName})>
  verifyCard(String cardNumber) async {
    final result = await _datasource.verifyCard(cardNumber);
    if (result == null) {
      return (
        success: false,
        message: 'Kartu tidak ditemukan.',
        cardId: null,
        customerName: null,
      );
    }
    if (result.isBlocked) {
      return (
        success: false,
        message: 'Kartu diblokir.',
        cardId: null,
        customerName: null,
      );
    }
    return (
      success: true,
      message: 'OK',
      cardId: result.cardId,
      customerName: result.customerName,
    );
  }

  @override
  Future<({bool success, String message, SessionEntity? session})>
  registerMbanking({
    required String cardNumber,
    required String email,
    required DateTime dateOfBirth,
    required String password,
  }) async {
    final result = await _datasource.registerMbanking(
      cardNumber: cardNumber,
      email: email,
      dateOfBirth: dateOfBirth,
      password: password,
    );
    if (!result.success || result.data == null) {
      return (success: false, message: result.message, session: null);
    }
    final session = result.data!.toEntity(email);
    await _tokenStorage.saveSession(
      token: session.token,
      accountId: session.accountId,
      customerName: session.customerName,
      email: email,
      accountNumber: session.accountNumber,
    );
    return (success: true, message: result.message, session: session);
  }

  @override
  Future<({bool success, String message, SessionEntity? session})>
  loginMbanking({required String email, required String password}) async {
    final result = await _datasource.loginMbanking(
      email: email,
      password: password,
    );
    if (!result.success || result.data == null) {
      return (success: false, message: result.message, session: null);
    }
    final session = result.data!.toEntity(email);
    await _tokenStorage.saveSession(
      token: session.token,
      accountId: session.accountId,
      customerName: session.customerName,
      email: email,
      accountNumber: session.accountNumber,
    );
    return (success: true, message: result.message, session: session);
  }

  @override
  Future<({bool success, String message})> verifyMbankingPin(
    int accountId,
    String pin,
  ) {
    return _datasource.verifyMbankingPin(accountId: accountId, pin: pin);
  }

  @override
  Future<void> logout() => _tokenStorage.clearSession();
}
