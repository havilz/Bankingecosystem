import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_input_types.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

/// Glassmorphism login overlay with PIN input, login button, and forget password.
/// Called via [showLoginOverlay] static method.
class LoginOverlay {
  /// Shows the glassmorphism login overlay dialog.
  static void show(BuildContext context) {
    final pinController = TextEditingController();
    bool obscurePin = true;

    showGeneralDialog(
      context: context,
      barrierDismissible: true,
      barrierLabel: 'Login Overlay',
      barrierColor: Colors.black.withValues(alpha: 0.3),
      transitionDuration: const Duration(milliseconds: 300),
      pageBuilder: (context, animation, secondaryAnimation) {
        return const SizedBox.shrink();
      },
      transitionBuilder: (context, animation, secondaryAnimation, child) {
        return SlideTransition(
          position: Tween<Offset>(begin: const Offset(0, 1), end: Offset.zero)
              .animate(
                CurvedAnimation(parent: animation, curve: Curves.easeOutCubic),
              ),
          child: _buildContent(context, pinController, obscurePin),
        );
      },
    );
  }

  static Widget _buildContent(
    BuildContext dialogContext,
    TextEditingController pinController,
    bool obscurePin,
  ) {
    return StatefulBuilder(
      builder: (context, setModalState) {
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
                            'Enter Your Password',
                            style: AppTextStyles.large.copyWith(
                              fontWeight: FontWeight.bold,
                              color: AppColors.white,
                              fontSize: 24,
                            ),
                          ),
                          const SizedBox(height: 32),

                          // PIN Input
                          TextField(
                            controller: pinController,
                            obscureText: obscurePin,
                            keyboardType: AppInputTypes.password,
                            style: AppTextStyles.medium.copyWith(
                              color: AppColors.white,
                            ),
                            textAlign: TextAlign.center,
                            decoration: InputDecoration(
                              hintText: 'Enter your password',
                              hintStyle: AppTextStyles.medium.copyWith(
                                color: AppColors.white.withValues(alpha: 0.5),
                              ),
                              suffixIcon: IconButton(
                                icon: Icon(
                                  obscurePin
                                      ? Icons.visibility_off
                                      : Icons.visibility,
                                  color: AppColors.white.withValues(alpha: 0.7),
                                ),
                                onPressed: () {
                                  setModalState(() {
                                    obscurePin = !obscurePin;
                                  });
                                },
                              ),
                              filled: true,
                              fillColor: AppColors.white.withValues(
                                alpha: 0.15,
                              ),
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

                          // Login Button (compact width, with hover)
                          ElevatedButton(
                            onPressed: () {
                              // Skip auth for now (frontend only) — go to main layout
                              context.go('/home');
                            },
                            style: ButtonStyle(
                              backgroundColor:
                                  WidgetStateProperty.resolveWith<Color>((
                                    states,
                                  ) {
                                    if (states.contains(WidgetState.hovered) ||
                                        states.contains(WidgetState.pressed)) {
                                      return AppColors.primaryLight;
                                    }
                                    return AppColors.white;
                                  }),
                              foregroundColor: WidgetStateProperty.all(
                                AppColors.primary,
                              ),
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
                            child: Text(
                              'Login',
                              style: AppTextStyles.button.copyWith(
                                color: AppColors.primary,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ),
                          const SizedBox(height: 8),

                          // Forget Password
                          TextButton(
                            onPressed: () {
                              // TODO: Navigate to forgot password / change PIN
                            },
                            child: Text(
                              'Forget Password?',
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
      },
    );
  }
}
