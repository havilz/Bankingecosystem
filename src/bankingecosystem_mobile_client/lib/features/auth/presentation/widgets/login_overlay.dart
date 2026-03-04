import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/storage/token_storage.dart';
import '../../../../core/ui/ui.dart';
import '../providers/auth_provider.dart';

/// Glassmorphism login overlay — password only.
/// Email is loaded from local storage (saved during registration).
class LoginOverlay {
  static void show(BuildContext context) {
    showGeneralDialog(
      context: context,
      barrierDismissible: true,
      barrierLabel: 'Login Overlay',
      barrierColor: Colors.black.withValues(alpha: 0.3),
      transitionDuration: const Duration(milliseconds: 300),
      pageBuilder: (_, p1, p2) => const SizedBox.shrink(),
      transitionBuilder: (context, animation, p1, p2) {
        return SlideTransition(
          position: Tween<Offset>(begin: const Offset(0, 1), end: Offset.zero)
              .animate(
                CurvedAnimation(parent: animation, curve: Curves.easeOutCubic),
              ),
          child: const _LoginOverlayContent(),
        );
      },
    );
  }
}

class _LoginOverlayContent extends ConsumerStatefulWidget {
  const _LoginOverlayContent();

  @override
  ConsumerState<_LoginOverlayContent> createState() =>
      _LoginOverlayContentState();
}

class _LoginOverlayContentState extends ConsumerState<_LoginOverlayContent> {
  final _passwordController = TextEditingController();
  bool _obscure = true;
  String? _email;

  @override
  void initState() {
    super.initState();
    _loadEmail();
  }

  @override
  void dispose() {
    _passwordController.dispose();
    super.dispose();
  }

  Future<void> _loadEmail() async {
    final email = await ref.read(tokenStorageProvider).getEmail();
    if (mounted) setState(() => _email = email);
  }

  Future<void> _login() async {
    if (_email == null || _email!.isEmpty) return;
    final password = _passwordController.text.trim();
    if (password.isEmpty) return;

    final router = GoRouter.of(context);
    await ref
        .read(authProvider.notifier)
        .loginMbanking(email: _email!, password: password, router: router);

    // Close overlay on success
    final authState = ref.read(authProvider);
    if (authState is AuthSuccess && mounted) {
      Navigator.of(context).pop();
    }
  }

  @override
  Widget build(BuildContext context) {
    final authState = ref.watch(authProvider);
    final isLoading = authState is AuthLoading;
    final errorMsg = authState is AuthError ? authState.message : null;

    return Material(
      color: Colors.transparent,
      child: SizedBox.expand(
        child: ClipRect(
          child: BackdropFilter(
            filter: ImageFilter.blur(sigmaX: 20, sigmaY: 20),
            child: Container(
              color: AppColors.white.withValues(alpha: 0.1),
              child: Center(
                child: Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 32),
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      // Title
                      Text(
                        'Masuk ke mBanking',
                        style: AppTextStyles.large.copyWith(
                          fontWeight: FontWeight.bold,
                          color: AppColors.white,
                          fontSize: 24,
                        ),
                      ),
                      if (_email != null) ...[
                        const SizedBox(height: 8),
                        Text(
                          _email!,
                          style: AppTextStyles.small.copyWith(
                            color: AppColors.white.withValues(alpha: 0.7),
                          ),
                        ),
                      ],
                      const SizedBox(height: 32),

                      // Error message
                      if (errorMsg != null) ...[
                        Container(
                          padding: const EdgeInsets.symmetric(
                            horizontal: 16,
                            vertical: 10,
                          ),
                          decoration: BoxDecoration(
                            color: Colors.red.withValues(alpha: 0.2),
                            borderRadius: BorderRadius.circular(8),
                            border: Border.all(
                              color: Colors.red.withValues(alpha: 0.4),
                            ),
                          ),
                          child: Text(
                            errorMsg,
                            style: AppTextStyles.small.copyWith(
                              color: Colors.white,
                            ),
                            textAlign: TextAlign.center,
                          ),
                        ),
                        const SizedBox(height: 16),
                      ],

                      // Password Input
                      TextField(
                        controller: _passwordController,
                        obscureText: _obscure,
                        keyboardType: AppInputTypes.password,
                        enabled: !isLoading,
                        style: AppTextStyles.medium.copyWith(
                          color: AppColors.white,
                        ),
                        textAlign: TextAlign.center,
                        decoration: InputDecoration(
                          hintText: 'Masukkan password',
                          hintStyle: AppTextStyles.medium.copyWith(
                            color: AppColors.white.withValues(alpha: 0.5),
                          ),
                          suffixIcon: IconButton(
                            icon: Icon(
                              _obscure
                                  ? Icons.visibility_off
                                  : Icons.visibility,
                              color: AppColors.white.withValues(alpha: 0.7),
                            ),
                            onPressed: () =>
                                setState(() => _obscure = !_obscure),
                          ),
                          filled: true,
                          fillColor: AppColors.white.withValues(alpha: 0.15),
                          contentPadding: const EdgeInsets.symmetric(
                            horizontal: 16,
                            vertical: 14,
                          ),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                            borderSide: BorderSide(
                              color: AppColors.white.withValues(alpha: 0.3),
                            ),
                          ),
                          enabledBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                            borderSide: BorderSide(
                              color: AppColors.white.withValues(alpha: 0.3),
                            ),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                            borderSide: const BorderSide(
                              color: AppColors.white,
                              width: 2,
                            ),
                          ),
                        ),
                      ),
                      const SizedBox(height: 24),

                      // Login Button
                      ElevatedButton(
                        onPressed: isLoading ? null : _login,
                        style: ButtonStyle(
                          backgroundColor:
                              WidgetStateProperty.resolveWith<Color>((states) {
                                if (states.contains(WidgetState.disabled)) {
                                  return AppColors.white.withValues(alpha: 0.5);
                                }
                                return AppColors.white;
                              }),
                          padding: WidgetStateProperty.all(
                            const EdgeInsets.symmetric(
                              horizontal: 36,
                              vertical: 12,
                            ),
                          ),
                          shape: WidgetStateProperty.all(
                            RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(24),
                            ),
                          ),
                          elevation: WidgetStateProperty.all(0),
                        ),
                        child: isLoading
                            ? const SizedBox(
                                width: 20,
                                height: 20,
                                child: CircularProgressIndicator(
                                  strokeWidth: 2,
                                  color: AppColors.primary,
                                ),
                              )
                            : Text(
                                'Masuk',
                                style: AppTextStyles.button.copyWith(
                                  color: AppColors.primary,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                      ),
                      const SizedBox(height: 8),

                      // Forgot password
                      TextButton(
                        onPressed: isLoading ? null : () {},
                        child: Text(
                          'Lupa password?',
                          style: AppTextStyles.small.copyWith(
                            color: AppColors.white.withValues(alpha: 0.8),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
