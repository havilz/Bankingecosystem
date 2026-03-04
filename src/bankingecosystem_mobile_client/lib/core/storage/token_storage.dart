import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

final tokenStorageProvider = Provider<TokenStorage>((ref) => TokenStorage());

class TokenStorage {
  static const _storage = FlutterSecureStorage();

  static const _keyToken = 'mbanking_token';
  static const _keyAccountId = 'mbanking_account_id';
  static const _keyCustomerName = 'mbanking_customer_name';
  static const _keyEmail = 'mbanking_email';
  static const _keyAccountNumber = 'mbanking_account_number';

  // ─── Token ───
  Future<void> saveToken(String token) =>
      _storage.write(key: _keyToken, value: token);

  Future<String?> getToken() => _storage.read(key: _keyToken);

  // ─── Session Data ───
  Future<void> saveSession({
    required String token,
    required int accountId,
    required String customerName,
    required String email,
    required String accountNumber,
  }) async {
    await Future.wait([
      _storage.write(key: _keyToken, value: token),
      _storage.write(key: _keyAccountId, value: accountId.toString()),
      _storage.write(key: _keyCustomerName, value: customerName),
      _storage.write(key: _keyEmail, value: email),
      _storage.write(key: _keyAccountNumber, value: accountNumber),
    ]);
  }

  Future<int?> getAccountId() async {
    final val = await _storage.read(key: _keyAccountId);
    return val != null ? int.tryParse(val) : null;
  }

  Future<String?> getCustomerName() => _storage.read(key: _keyCustomerName);
  Future<String?> getEmail() => _storage.read(key: _keyEmail);
  Future<String?> getAccountNumber() => _storage.read(key: _keyAccountNumber);

  Future<bool> isLoggedIn() async {
    final token = await getToken();
    return token != null && token.isNotEmpty;
  }

  // ─── Clear ───
  Future<void> clearAll() => _storage.deleteAll();
}
