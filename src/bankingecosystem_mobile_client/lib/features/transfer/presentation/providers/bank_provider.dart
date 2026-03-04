import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../core/network/dio_client.dart';
import '../../../../core/network/api_endpoints.dart';

// ─── Bank Model ───

class BankModel {
  final int id;
  final String code;
  final String name;

  const BankModel({required this.id, required this.code, required this.name});

  factory BankModel.fromJson(Map<String, dynamic> json) => BankModel(
    id: json['bankId'] as int,
    code: json['bankCode'] as String,
    name: json['bankName'] as String,
  );
}

// ─── State ───

class BankListState {
  final bool isLoading;
  final List<BankModel> banks;
  final String? error;

  const BankListState({
    this.isLoading = true,
    this.banks = const [],
    this.error,
  });

  BankListState copyWith({
    bool? isLoading,
    List<BankModel>? banks,
    String? error,
  }) => BankListState(
    isLoading: isLoading ?? this.isLoading,
    banks: banks ?? this.banks,
    error: error,
  );
}

// ─── Notifier ───

class BankNotifier extends Notifier<BankListState> {
  @override
  BankListState build() {
    _fetchBanks();
    return const BankListState(isLoading: true);
  }

  Future<void> _fetchBanks() async {
    try {
      final dio = ref.read(dioClientProvider).dio;
      final response = await dio.get(ApiEndpoints.banks);
      final items = response.data['data'] as List<dynamic>;
      final banks = items
          .map((e) => BankModel.fromJson(e as Map<String, dynamic>))
          .toList();
      state = state.copyWith(isLoading: false, banks: banks);
    } catch (e) {
      state = state.copyWith(isLoading: false, error: e.toString());
    }
  }
}

final bankProvider = NotifierProvider<BankNotifier, BankListState>(
  BankNotifier.new,
);
