import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class TransferSuccessBottomSheet extends StatelessWidget {
  final String accountName;
  final String bankName;
  final String accountNumber;
  final VoidCallback onContinue;

  const TransferSuccessBottomSheet({
    super.key,
    required this.accountName,
    required this.bankName,
    required this.accountNumber,
    required this.onContinue,
  });

  @override
  Widget build(BuildContext context) {
    // Generate initials for avatar (e.g., "Budi Santoso" -> "BS")
    final initials = accountName
        .split(' ')
        .take(2)
        .map((e) => e.isNotEmpty ? e[0].toUpperCase() : '')
        .join('');

    return Container(
      decoration: const BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.vertical(top: Radius.circular(24)),
      ),
      padding: const EdgeInsets.all(24),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Handle indicator
          Center(
            child: Container(
              width: 40,
              height: 4,
              decoration: BoxDecoration(
                color: AppColors.grey.withValues(alpha: 0.3),
                borderRadius: BorderRadius.circular(2),
              ),
            ),
          ),
          const SizedBox(height: 24),
          Text(
            'Penerima Baru',
            style: AppTextStyles.h2.copyWith(fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 24),
          Container(
            padding: const EdgeInsets.all(16),
            decoration: BoxDecoration(
              color: AppColors.lightGrey,
              borderRadius: BorderRadius.circular(12),
            ),
            child: Row(
              children: [
                CircleAvatar(
                  backgroundColor: AppColors.primaryLight,
                  child: Text(
                    initials,
                    style: AppTextStyles.medium.copyWith(
                      color: AppColors.primaryDark,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                const SizedBox(width: 16),
                Expanded(
                  // Added Expanded to text columns to prevent overflow
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        accountName,
                        style: AppTextStyles.medium.copyWith(
                          fontWeight: FontWeight.bold,
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                      ),
                      const SizedBox(height: 4),
                      Text(
                        '$bankName - $accountNumber',
                        style: AppTextStyles.small.copyWith(
                          color: AppColors.grey,
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: 32),
          SizedBox(
            width: double.infinity,
            child: ElevatedButton(
              onPressed: onContinue,
              style: ElevatedButton.styleFrom(
                backgroundColor: AppColors.primary,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
                padding: const EdgeInsets.symmetric(vertical: 16),
              ),
              child: Text(
                'Lanjut',
                style: AppTextStyles.button.copyWith(color: AppColors.white),
              ),
            ),
          ),
          const SizedBox(height: 16), // Bottom padding for safety
        ],
      ),
    );
  }

  /// Helper method to show this bottom sheet easily
  static void show({
    required BuildContext context,
    required String accountName,
    required String bankName,
    required String accountNumber,
    required VoidCallback onContinue,
  }) {
    showModalBottomSheet(
      context: context,
      backgroundColor: Colors.transparent,
      isScrollControlled: true,
      builder: (context) {
        return TransferSuccessBottomSheet(
          accountName: accountName,
          bankName: bankName,
          accountNumber: accountNumber,
          onContinue: onContinue,
        );
      },
    );
  }
}
