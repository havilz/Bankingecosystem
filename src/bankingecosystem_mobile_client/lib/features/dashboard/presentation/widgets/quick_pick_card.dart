import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class QuickPickCard extends StatelessWidget {
  const QuickPickCard({super.key});

  // Quick pick frequent transactions
  static final List<Map<String, dynamic>> _quickPicks = [
    {
      'icon': Icons.send_outlined,
      'type': 'Transfer',
      'bank': 'Bank Mandiri',
      'recipient': 'Ahmad Rizky',
    },
    {
      'icon': Icons.send_outlined,
      'type': 'Transfer',
      'bank': 'Bank BCA',
      'recipient': 'Siti Nurhaliza',
    },
    {
      'icon': Icons.account_balance_wallet_outlined,
      'type': 'Top Up',
      'bank': 'DANA',
      'recipient': '0812****3456',
    },
    {
      'icon': Icons.account_balance_wallet_outlined,
      'type': 'Top Up',
      'bank': 'OVO',
      'recipient': '0856****7890',
    },
    {
      'icon': Icons.phone_android_outlined,
      'type': 'Pulsa',
      'bank': 'Telkomsel',
      'recipient': '0812****1234',
    },
    {
      'icon': Icons.bolt_outlined,
      'type': 'Listrik',
      'bank': 'PLN',
      'recipient': '5423 1234 5678',
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

          // ===== HORIZONTAL SCROLL OF QUICK PICK CARDS =====
          SizedBox(
            height: 100,
            child: ListView.separated(
              scrollDirection: Axis.horizontal,
              itemCount: _quickPicks.length,
              separatorBuilder: (context, index) => const SizedBox(width: 10),
              itemBuilder: (context, index) {
                final pick = _quickPicks[index];
                return _buildQuickPickItem(
                  icon: pick['icon'] as IconData,
                  type: pick['type'] as String,
                  bank: pick['bank'] as String,
                  recipient: pick['recipient'] as String,
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  /// Header: "Quick Pick" (bold, left) | "Atur" + filter (right)
  Widget _buildHeaderRow() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Text(
          'Quick Pick',
          style: AppTextStyles.medium.copyWith(
            fontWeight: FontWeight.bold,
            fontSize: 16,
            color: AppColors.textPrimary,
          ),
        ),
        const Spacer(),
        GestureDetector(
          onTap: () {
            // TODO: Handle atur action
          },
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(
                'Atur',
                style: AppTextStyles.small.copyWith(
                  fontSize: 13,
                  color: AppColors.primary,
                ),
              ),
              const SizedBox(width: 4),
              const Icon(Icons.tune, size: 18, color: AppColors.primary),
            ],
          ),
        ),
      ],
    );
  }

  /// Single quick pick card — icon + type + bank + recipient
  Widget _buildQuickPickItem({
    required IconData icon,
    required String type,
    required String bank,
    required String recipient,
  }) {
    return GestureDetector(
      onTap: () {
        // TODO: Handle quick pick tap
      },
      child: Container(
        width: 120,
        padding: const EdgeInsets.all(10),
        decoration: BoxDecoration(
          color: AppColors.lightGrey,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(color: AppColors.grey.withValues(alpha: 0.15)),
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Icon + Type row
            Row(
              children: [
                Container(
                  width: 28,
                  height: 28,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    color: AppColors.primaryLight.withValues(alpha: 0.5),
                  ),
                  child: Icon(icon, size: 14, color: AppColors.primary),
                ),
                const SizedBox(width: 6),
                Expanded(
                  child: Text(
                    type,
                    style: AppTextStyles.small.copyWith(
                      fontSize: 12,
                      fontWeight: FontWeight.w600,
                      color: AppColors.textPrimary,
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ],
            ),
            const Spacer(),
            // Bank / service name
            Text(
              bank,
              style: AppTextStyles.small.copyWith(
                fontSize: 10,
                color: AppColors.grey,
              ),
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
            ),
            const SizedBox(height: 2),
            // Recipient name / number
            Text(
              recipient,
              style: AppTextStyles.small.copyWith(
                fontSize: 11,
                fontWeight: FontWeight.w500,
                color: AppColors.textPrimary,
              ),
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
            ),
          ],
        ),
      ),
    );
  }
}
