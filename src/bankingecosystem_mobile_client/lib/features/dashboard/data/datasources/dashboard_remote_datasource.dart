import 'package:dio/dio.dart';

import '../../../../core/network/api_endpoints.dart';
import '../../../../core/network/dio_client.dart';
import '../models/dashboard_models.dart';

class DashboardRemoteDatasource {
  final DioClient _client;

  DashboardRemoteDatasource(this._client);

  Future<({bool success, String message, TransactionModel? data})> getBalance(
    int accountId,
  ) async {
    try {
      final response = await _client.dio.get(
        '${ApiEndpoints.balance}/$accountId',
      );
      final data = response.data['data'];
      return (
        success: true,
        message: 'OK',
        data: TransactionModel.fromJson(data as Map<String, dynamic>),
      );
    } on DioException catch (e) {
      final msg =
          e.response?.data?['message'] as String? ?? 'Gagal mengambil saldo.';
      return (success: false, message: msg, data: null);
    }
  }

  Future<({bool success, String message, List<TransactionModel> data})>
  getHistory(int accountId, {int page = 1, int pageSize = 10}) async {
    try {
      final response = await _client.dio.get(
        '${ApiEndpoints.history}/$accountId',
        queryParameters: {'page': page, 'pageSize': pageSize},
      );
      final items = response.data['data'] as List<dynamic>;
      return (
        success: true,
        message: 'OK',
        data: items
            .map((e) => TransactionModel.fromJson(e as Map<String, dynamic>))
            .toList(),
      );
    } on DioException catch (e) {
      final msg =
          e.response?.data?['message'] as String? ?? 'Gagal mengambil riwayat.';
      return (success: false, message: msg, data: <TransactionModel>[]);
    }
  }
}
