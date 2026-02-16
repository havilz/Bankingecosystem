import 'package:flutter/material.dart';

import '../../../core/ui/theme/app_colors.dart';
import '../../../core/ui/theme/app_text_styles.dart';
import 'widgets/login_overlay.dart';

class LoginScreen extends StatelessWidget {
  const LoginScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white,
      body: SafeArea(
        child: Column(
          children: [
            // ===== CENTER SECTION: Logo + Name =====
            Expanded(
              child: Center(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    // Logo
                    Container(
                      width: 100,
                      height: 100,
                      decoration: BoxDecoration(
                        shape: BoxShape.circle,
                        gradient: const LinearGradient(
                          colors: [AppColors.primary, AppColors.primaryDark],
                          begin: Alignment.topLeft,
                          end: Alignment.bottomRight,
                        ),
                        boxShadow: [
                          BoxShadow(
                            color: AppColors.primary.withValues(alpha: 0.3),
                            blurRadius: 16,
                            offset: const Offset(0, 6),
                          ),
                        ],
                      ),
                      child: const Center(
                        child: Icon(
                          Icons.account_balance,
                          size: 44,
                          color: AppColors.white,
                        ),
                      ),
                    ),
                    const SizedBox(height: 16),

                    // App Name
                    Text(
                      'Banking Ecosystem',
                      style: AppTextStyles.large.copyWith(
                        color: AppColors.primary,
                        fontWeight: FontWeight.bold,
                        fontSize: 22,
                      ),
                    ),
                  ],
                ),
              ),
            ),

            // ===== BOTTOM SECTION: Quick Access + Login in grey container =====
            Container(
              width: double.infinity,
              decoration: BoxDecoration(
                color: AppColors.lightGrey,
                borderRadius: const BorderRadius.only(
                  topLeft: Radius.circular(28),
                  topRight: Radius.circular(28),
                ),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withValues(alpha: 0.05),
                    blurRadius: 10,
                    offset: const Offset(0, -2),
                  ),
                ],
              ),
              padding: const EdgeInsets.fromLTRB(20, 24, 20, 32),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Quick Access Title
                  Text(
                    'Quick Access',
                    style: AppTextStyles.medium.copyWith(
                      fontWeight: FontWeight.w600,
                      color: AppColors.textPrimary,
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Quick Access Icons
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceAround,
                    children: [
                      _buildQuickAccessItem(Icons.qr_code_scanner, 'QRIS'),
                      _buildQuickAccessItem(Icons.send, 'Transfer'),
                      _buildQuickAccessItem(Icons.phone_android, 'Pulsa'),
                      _buildQuickAccessItem(Icons.bolt, 'PLN'),
                      _buildQuickAccessItem(Icons.water_drop, 'PDAM'),
                      _buildQuickAccessItem(Icons.more_horiz, 'More'),
                    ],
                  ),
                  const SizedBox(height: 24),

                  // Login Button (compact, centered below quick access)
                  Center(
                    child: ElevatedButton(
                      onPressed: () => LoginOverlay.show(context),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: AppColors.primary,
                        foregroundColor: AppColors.white,
                        padding: const EdgeInsets.symmetric(
                          horizontal: 36,
                          vertical: 12,
                        ),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(24),
                        ),
                        elevation: 0,
                      ),
                      child: Text(
                        'Login',
                        style: AppTextStyles.button.copyWith(
                          color: AppColors.white,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  // ===== Quick Access Item Builder =====
  Widget _buildQuickAccessItem(IconData icon, String label) {
    return Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          width: 50,
          height: 50,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            color: AppColors.primaryLight.withValues(alpha: 0.4),
            border: Border.all(
              color: AppColors.primary.withValues(alpha: 0.15),
            ),
          ),
          child: Center(child: Icon(icon, size: 24, color: AppColors.primary)),
        ),
        const SizedBox(height: 6),
        Text(
          label,
          style: AppTextStyles.small.copyWith(
            color: AppColors.textPrimary,
            fontSize: 11,
          ),
        ),
      ],
    );
  }
}
