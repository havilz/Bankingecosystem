import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../core/network/dio_client.dart';
import '../../../../core/storage/token_storage.dart';
import '../../data/datasources/dashboard_remote_datasource.dart';
import '../../data/repositories/dashboard_repository_impl.dart';
import '../../domain/entities/dashboard_entities.dart';
import '../../domain/repositories/dashboard_repository.dart';

// ─── Repository Provider ───

final dashboardRepositoryProvider = Provider<DashboardRepository>((ref) {
  final dioClient = ref.read(dioClientProvider);
  final tokenStorage = ref.read(tokenStorageProvider);
  final datasource = DashboardRemoteDatasource(dioClient);
  return DashboardRepositoryImpl(datasource, tokenStorage);
});

// ─── State ───

class DashboardState {
  final bool isLoading;
  final String? error;
  final BalanceEntity? balance;
  final List<TransactionEntity> recentTransactions;

  const DashboardState({
    this.isLoading = true,
    this.error,
    this.balance,
    this.recentTransactions = const [],
  });

  DashboardState copyWith({
    bool? isLoading,
    String? error,
    BalanceEntity? balance,
    List<TransactionEntity>? recentTransactions,
  }) => DashboardState(
    isLoading: isLoading ?? this.isLoading,
    error: error,
    balance: balance ?? this.balance,
    recentTransactions: recentTransactions ?? this.recentTransactions,
  );
}

// ─── Notifier ───

class DashboardNotifier extends Notifier<DashboardState> {
  @override
  DashboardState build() {
    _loadData();
    return const DashboardState(isLoading: true);
  }

  Future<void> _loadData() async {
    final tokenStorage = ref.read(tokenStorageProvider);
    final accountId = await tokenStorage.getAccountId();
    if (accountId == null) {
      state = state.copyWith(
        isLoading: false,
        error: 'Sesi tidak ditemukan. Silakan login kembali.',
      );
      return;
    }

    final repo = ref.read(dashboardRepositoryProvider);

    // Fetch balance and history in parallel
    final results = await Future.wait([
      repo.getBalance(accountId),
      repo.getHistory(accountId),
    ]);

    final balanceResult =
        results[0] as ({bool success, String message, BalanceEntity? balance});
    final historyResult =
        results[1]
            as ({
              bool success,
              String message,
              List<TransactionEntity> transactions,
            });

    state = state.copyWith(
      isLoading: false,
      balance: balanceResult.balance,
      recentTransactions: historyResult.transactions,
      error: balanceResult.success ? null : balanceResult.message,
    );
  }

  Future<void> refresh() async {
    state = state.copyWith(isLoading: true);
    await _loadData();
  }
}

final dashboardProvider = NotifierProvider<DashboardNotifier, DashboardState>(
  DashboardNotifier.new,
);
