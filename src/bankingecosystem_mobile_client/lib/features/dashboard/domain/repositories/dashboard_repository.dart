import '../entities/dashboard_entities.dart';

abstract class DashboardRepository {
  Future<({bool success, String message, BalanceEntity? balance})> getBalance(
    int accountId,
  );
  Future<({bool success, String message, List<TransactionEntity> transactions})>
  getHistory(int accountId, {int page = 1, int pageSize = 10});
}
