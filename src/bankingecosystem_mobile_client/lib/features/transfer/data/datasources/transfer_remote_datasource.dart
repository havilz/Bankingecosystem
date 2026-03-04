import 'package:dio/dio.dart';

import '../../../../core/network/api_endpoints.dart';
import '../../../../core/network/dio_client.dart';
import '../models/transfer_model.dart';

class TransferRemoteDatasource {
  final DioClient _client;

  TransferRemoteDatasource(this._client);

  Future<({bool success, String message, TransferModel? data})> doTransfer({
    required int accountId,
    required String targetAccountNumber,
    required double amount,
    String? description,
  }) async {
    try {
      final response = await _client.dio.post(
        ApiEndpoints.transfer,
        data: {
          'accountId': accountId,
          'targetAccountNumber': targetAccountNumber,
          'amount': amount,
          if (description != null && description.isNotEmpty)
            'description': description,
        },
      );
      final data = response.data['data'];
      return (
        success: true,
        message: response.data['message'] as String? ?? 'Transfer berhasil.',
        data: TransferModel.fromJson(data as Map<String, dynamic>),
      );
    } on DioException catch (e) {
      final msg = e.response?.data?['message'] as String? ?? 'Transfer gagal.';
      return (success: false, message: msg, data: null);
    }
  }
}
