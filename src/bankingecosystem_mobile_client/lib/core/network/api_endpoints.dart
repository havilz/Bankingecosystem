class ApiEndpoints {
  // Base URL (Android Emulator: 10.0.2.2, iOS Simulator: localhost)
  static const String baseUrl = 'http://10.148.20.246:5046/api';

  // --- Auth Endpoints (AuthController) ---
  // POST /Auth/verify-card { "cardNumber": "..." }
  static const String verifyCard = '/Auth/verify-card';

  // POST /Auth/verify-pin { "cardId": 1, "pin": "123456" }
  // Returns: AuthResponse (Token/Session)
  static const String verifyPin = '/Auth/verify-pin';

  // POST /Auth/change-pin { "cardId": 1, "oldPin": "...", "newPin": "..." }
  static const String changePin = '/Auth/change-pin';

  // POST /Auth/verify-mbanking-pin { "accountId": 1, "pin": "123456" }
  static const String verifyMbankingPin = '/Auth/verify-mbanking-pin';

  // POST /Auth/register-mbanking { "cardNumber", "email", "dateOfBirth", "password" }
  static const String registerMbanking = '/Auth/register-mbanking';

  // POST /Auth/login-mbanking { "email", "password" }
  static const String loginMbanking = '/Auth/login-mbanking';

  // --- Transaction Endpoints (TransactionController) ---

  // POST /Transaction/transfer { "fromAccountId": 1, "toAccountId": 2, "amount": 100, "pin": "..." }
  static const String transfer = '/Transaction/transfer';

  // GET /Transaction/balance/{accountId}?atmId={atmId}
  // Use string interpolation: "$balance/$accountId"
  static const String balance = '/Transaction/balance';

  // GET /Transaction/history/{accountId}?page=1&pageSize=20
  // Use string interpolation: "$history/$accountId"
  static const String history = '/Transaction/history';

  // GET /Transaction/lookup/{accountNumber}
  // Returns: { accountNumber, customerName }
  static const String accountLookup = '/Transaction/lookup';

  // --- Bank Endpoints (BankController) ---
  // GET /Bank
  static const String banks = '/Bank';

  // --- Transfer Favorites (TransferController) ---
  // GET    /Transfer/favorites/{accountId}
  // POST   /Transfer/favorites { accountId, accountNumber, nickname, bankName }
  // DELETE /Transfer/favorites/{id}?accountId={accountId}
  static const String favorites = '/Transfer/favorites';
}
