import '../../domain/entities/dashboard_entities.dart';
import '../../domain/repositories/dashboard_repository.dart';
import '../datasources/dashboard_remote_datasource.dart';
import '../../../../core/storage/token_storage.dart';

class DashboardRepositoryImpl implements DashboardRepository {
  final DashboardRemoteDatasource _datasource;
  final TokenStorage _tokenStorage;

  DashboardRepositoryImpl(this._datasource, this._tokenStorage);

  @override
  Future<({bool success, String message, BalanceEntity? balance})> getBalance(
    int accountId,
  ) async {
    final result = await _datasource.getBalance(accountId);
    if (!result.success || result.data == null) {
      return (success: false, message: result.message, balance: null);
    }

    final customerName = await _tokenStorage.getCustomerName() ?? '';
    final accountNumber = await _tokenStorage.getAccountNumber() ?? '';

    return (
      success: true,
      message: 'OK',
      balance: BalanceEntity(
        accountId: accountId,
        accountNumber: accountNumber,
        customerName: customerName,
        balance: result.data!.balanceAfter,
      ),
    );
  }

  @override
  Future<({bool success, String message, List<TransactionEntity> transactions})>
  getHistory(int accountId, {int page = 1, int pageSize = 10}) async {
    final result = await _datasource.getHistory(
      accountId,
      page: page,
      pageSize: pageSize,
    );
    if (!result.success) {
      return (
        success: false,
        message: result.message,
        transactions: <TransactionEntity>[],
      );
    }
    return (
      success: true,
      message: 'OK',
      transactions: result.data.map((m) => m.toEntity()).toList(),
    );
  }
}
