import 'package:dio/dio.dart';

import '../../../../core/network/api_endpoints.dart';
import '../../../../core/network/dio_client.dart';
import '../models/auth_models.dart';

class AuthRemoteDatasource {
  final DioClient _client;

  AuthRemoteDatasource(this._client);

  Future<VerifyCardModel?> verifyCard(String cardNumber) async {
    try {
      final response = await _client.dio.post(
        ApiEndpoints.verifyCard,
        data: {'cardNumber': cardNumber},
      );
      final data = response.data['data'];
      return VerifyCardModel.fromJson(data as Map<String, dynamic>);
    } on DioException {
      return null;
    }
  }

  Future<({bool success, String message, AuthResponseModel? data})>
  registerMbanking({
    required String cardNumber,
    required String email,
    required DateTime dateOfBirth,
    required String password,
  }) async {
    try {
      final response = await _client.dio.post(
        ApiEndpoints.registerMbanking,
        data: {
          'cardNumber': cardNumber,
          'email': email,
          'dateOfBirth': dateOfBirth.toIso8601String(),
          'password': password,
        },
      );
      final body = response.data as Map<String, dynamic>;
      return (
        success: body['success'] as bool,
        message: body['message'] as String,
        data: AuthResponseModel.fromJson(body['data'] as Map<String, dynamic>),
      );
    } on DioException catch (e) {
      final msg = e.response?.data?['message'] as String? ?? 'Gagal mendaftar.';
      return (success: false, message: msg, data: null);
    }
  }

  Future<({bool success, String message, AuthResponseModel? data})>
  loginMbanking({required String email, required String password}) async {
    try {
      final response = await _client.dio.post(
        ApiEndpoints.loginMbanking,
        data: {'email': email, 'password': password},
      );
      final body = response.data as Map<String, dynamic>;
      return (
        success: body['success'] as bool,
        message: body['message'] as String,
        data: AuthResponseModel.fromJson(body['data'] as Map<String, dynamic>),
      );
    } on DioException catch (e) {
      final msg = e.response?.data?['message'] as String? ?? 'Login gagal.';
      return (success: false, message: msg, data: null);
    }
  }

  Future<({bool success, String message})> verifyMbankingPin({
    required int accountId,
    required String pin,
  }) async {
    try {
      final response = await _client.dio.post(
        ApiEndpoints.verifyMbankingPin,
        data: {'accountId': accountId, 'pin': pin},
      );
      final body = response.data as Map<String, dynamic>;
      return (
        success: body['success'] as bool? ?? true,
        message: body['message'] as String? ?? 'PIN Valid',
      );
    } on DioException catch (e) {
      final msg =
          e.response?.data?['message'] as String? ?? 'PIN Salah atau Error.';
      return (success: false, message: msg);
    }
  }
}
