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

  // Bank internal — selalu ada di posisi pertama
  static const _internalBank = BankModel(
    id: 0,
    code: 'ECOSYS',
    name: 'Banking Ecosystem (Internal)',
  );

  Future<void> _fetchBanks() async {
    try {
      final dio = ref.read(dioClientProvider).dio;
      final response = await dio.get(ApiEndpoints.banks);
      final items = response.data['data'] as List<dynamic>;
      final fromApi = items
          .map((e) => BankModel.fromJson(e as Map<String, dynamic>))
          // hapus duplikat bank internal yang mungkin ada dari API
          .where((b) => b.code != 'ECOSYS')
          .toList();

      // Selalu tampilkan bank internal di paling atas
      state = state.copyWith(
        isLoading: false,
        banks: [_internalBank, ...fromApi],
      );
    } catch (e) {
      // Jika API gagal, minimal bank internal tetap tersedia
      state = state.copyWith(
        isLoading: false,
        banks: [_internalBank],
        error: e.toString(),
      );
    }
  }
}

final bankProvider = NotifierProvider<BankNotifier, BankListState>(
  BankNotifier.new,
);
