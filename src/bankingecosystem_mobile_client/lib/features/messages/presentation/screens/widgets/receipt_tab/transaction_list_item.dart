import 'package:flutter/material.dart';
import '../../../../../../../core/ui/ui.dart';

class TransactionListItem extends StatelessWidget {
  final IconData icon;
  final String title;
  final String status;
  final int amount;
  final bool isCredit;

  const TransactionListItem({
    super.key,
    required this.icon,
    required this.title,
    required this.status,
    required this.amount,
    this.isCredit = false,
  });

  @override
  Widget build(BuildContext context) {
    // Format the amount (e.g., "- Rp 18.000^00")
    // Simple mock formatting for now
    String formattedAmount =
        '${isCredit ? '+' : '-'} Rp ${_formatCurrency(amount)}';

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 12.0),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // Left: Icon in a box
          Container(
            padding: const EdgeInsets.all(8),
            decoration: BoxDecoration(
              color: AppColors.white,
              borderRadius: BorderRadius.circular(8),
              boxShadow: [
                BoxShadow(
                  color: Colors.grey.withValues(alpha: 0.1),
                  spreadRadius: 1,
                  blurRadius: 2,
                  offset: const Offset(0, 1),
                ),
              ],
            ),
            child: Icon(icon, color: AppColors.primary, size: 24),
          ),
          const SizedBox(width: 16),

          // Middle: Title & Status
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: AppTextStyles.medium.copyWith(
                    fontWeight: FontWeight.w500,
                    color: AppColors.textPrimary,
                  ),
                ),
                const SizedBox(height: 4),
                Text(
                  status,
                  style: AppTextStyles.small.copyWith(
                    color: status.toLowerCase() == 'berhasil'
                        ? Colors.green
                        : AppColors.grey,
                  ),
                ),
              ],
            ),
          ),

          // Right: Amount
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                formattedAmount,
                style: AppTextStyles.medium.copyWith(
                  fontWeight: FontWeight.w500,
                  color: isCredit ? Colors.green : AppColors.textPrimary,
                ),
              ),
              const SizedBox(width: 2), // small gap for superscript
              Padding(
                padding: const EdgeInsets.only(top: 2.0), // push down slightly
                child: Text(
                  '00',
                  style: AppTextStyles.small.copyWith(
                    fontSize: 10,
                    fontWeight: FontWeight.w500,
                    color: isCredit ? Colors.green : AppColors.textPrimary,
                  ),
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  // Helper function to insert dots in thousands
  String _formatCurrency(int value) {
    String str = value.toString();
    String result = '';
    int count = 0;
    for (int i = str.length - 1; i >= 0; i--) {
      result = str[i] + result;
      count++;
      if (count == 3 && i > 0) {
        result = '.$result';
        count = 0;
      }
    }
    return result;
  }
}
