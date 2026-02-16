import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class EWalletCard extends StatelessWidget {
  const EWalletCard({super.key});

  // Mock connected e-wallets
  static final List<Map<String, dynamic>> _wallets = [
    {
      'name': 'OVO',
      'balance': 'Rp 350.000',
      'color': const Color(0xFF4C3494),
      'icon': Icons.account_balance_wallet,
    },
    {
      'name': 'DANA',
      'balance': 'Rp 125.500',
      'color': const Color(0xFF108EE9),
      'icon': Icons.account_balance_wallet,
    },
    {
      'name': 'GoPay',
      'balance': 'Rp 78.200',
      'color': const Color(0xFF00AED6),
      'icon': Icons.account_balance_wallet,
    },
    {
      'name': 'ShopeePay',
      'balance': 'Rp 50.000',
      'color': const Color(0xFFEE4D2D),
      'icon': Icons.account_balance_wallet,
    },
  ];

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 16),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(16),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.1),
            blurRadius: 16,
            spreadRadius: 2,
            offset: const Offset(0, 6),
          ),
          BoxShadow(
            color: AppColors.primary.withValues(alpha: 0.06),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
        border: Border.all(color: AppColors.grey.withValues(alpha: 0.12)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // ===== HEADER ROW =====
          _buildHeaderRow(),
          const SizedBox(height: 14),

          // ===== 2x2 GRID OF E-WALLET CARDS =====
          GridView.builder(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
              crossAxisCount: 2,
              mainAxisSpacing: 10,
              crossAxisSpacing: 10,
              childAspectRatio: 1.6,
            ),
            itemCount: _wallets.length,
            itemBuilder: (context, index) {
              final wallet = _wallets[index];
              return _buildWalletItem(
                name: wallet['name'] as String,
                balance: wallet['balance'] as String,
                color: wallet['color'] as Color,
                icon: wallet['icon'] as IconData,
              );
            },
          ),
        ],
      ),
    );
  }

  /// Header: "E-Wallet" (bold, left) | "Hubungkan" + link icon (right)
  Widget _buildHeaderRow() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Text(
          'E-Wallet',
          style: AppTextStyles.medium.copyWith(
            fontWeight: FontWeight.bold,
            fontSize: 16,
            color: AppColors.textPrimary,
          ),
        ),
        const Spacer(),
        GestureDetector(
          onTap: () {
            // TODO: Handle connect new e-wallet
          },
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(
                'Hubungkan',
                style: AppTextStyles.small.copyWith(
                  fontSize: 13,
                  color: AppColors.primary,
                ),
              ),
              const SizedBox(width: 4),
              const Icon(
                Icons.add_circle_outline,
                size: 18,
                color: AppColors.primary,
              ),
            ],
          ),
        ),
      ],
    );
  }

  /// Single e-wallet card — branded color + name + balance
  Widget _buildWalletItem({
    required String name,
    required String balance,
    required Color color,
    required IconData icon,
  }) {
    return GestureDetector(
      onTap: () {
        // TODO: Handle e-wallet tap
      },
      child: Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [color, color.withValues(alpha: 0.75)],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
          borderRadius: BorderRadius.circular(12),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // E-wallet name + icon
            Row(
              children: [
                Icon(
                  icon,
                  size: 18,
                  color: Colors.white.withValues(alpha: 0.9),
                ),
                const SizedBox(width: 6),
                Text(
                  name,
                  style: AppTextStyles.small.copyWith(
                    fontSize: 13,
                    fontWeight: FontWeight.w600,
                    color: Colors.white,
                  ),
                ),
              ],
            ),
            const Spacer(),
            // Balance label
            Text(
              'Saldo',
              style: AppTextStyles.small.copyWith(
                fontSize: 10,
                color: Colors.white.withValues(alpha: 0.7),
              ),
            ),
            const SizedBox(height: 2),
            // Balance amount
            Text(
              balance,
              style: AppTextStyles.medium.copyWith(
                fontSize: 14,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
