import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/network/dio_client.dart';
import '../../../../core/storage/token_storage.dart';
import '../../data/datasources/auth_remote_datasource.dart';
import '../../data/repositories/auth_repository_impl.dart';
import '../../domain/repositories/auth_repository.dart';

// ─── Repository Provider ───

final authRepositoryProvider = Provider<AuthRepository>((ref) {
  final dioClient = ref.read(dioClientProvider);
  final tokenStorage = ref.read(tokenStorageProvider);
  final datasource = AuthRemoteDatasource(dioClient);
  return AuthRepositoryImpl(datasource, tokenStorage);
});

// ─── State ───

sealed class AuthState {
  const AuthState();
}

class AuthIdle extends AuthState {
  const AuthIdle();
}

class AuthLoading extends AuthState {
  const AuthLoading();
}

class AuthError extends AuthState {
  final String message;
  const AuthError(this.message);
}

class AuthSuccess extends AuthState {
  const AuthSuccess();
}

// ─── Notifier (Riverpod 3.x) ───

class AuthNotifier extends Notifier<AuthState> {
  @override
  AuthState build() => const AuthIdle();

  void reset() => state = const AuthIdle();

  Future<void> registerMbanking({
    required String cardNumber,
    required String email,
    required DateTime dateOfBirth,
    required String password,
    required GoRouter router,
  }) async {
    state = const AuthLoading();
    final repo = ref.read(authRepositoryProvider);
    final result = await repo.registerMbanking(
      cardNumber: cardNumber,
      email: email,
      dateOfBirth: dateOfBirth,
      password: password,
    );
    if (result.success) {
      state = const AuthSuccess();
      router.go('/login');
    } else {
      state = AuthError(result.message);
    }
  }

  Future<void> loginMbanking({
    required String email,
    required String password,
    required GoRouter router,
  }) async {
    state = const AuthLoading();
    final repo = ref.read(authRepositoryProvider);
    final result = await repo.loginMbanking(email: email, password: password);
    if (result.success) {
      state = const AuthSuccess();
      router.go('/home');
    } else {
      state = AuthError(result.message);
    }
  }

  Future<void> logout(GoRouter router) async {
    final repo = ref.read(authRepositoryProvider);
    await repo.logout();
    state = const AuthIdle();
    router.go('/login');
  }
}

// ─── Provider ───

final authProvider = NotifierProvider<AuthNotifier, AuthState>(
  AuthNotifier.new,
);
