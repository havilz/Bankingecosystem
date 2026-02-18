import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class RecentTransferSection extends StatelessWidget {
  final String title;

  const RecentTransferSection({super.key, required this.title});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.fromLTRB(16, 24, 16, 8),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                title,
                style: AppTextStyles.medium.copyWith(
                  fontWeight: FontWeight.bold,
                ),
              ),
              if (title == 'Daftar Transfer')
                Text(
                  'Atur',
                  style: AppTextStyles.small.copyWith(
                    color: AppColors.primary,
                    fontWeight: FontWeight.bold,
                  ),
                ),
            ],
          ),
        ),
        _buildTransferItem('AH', 'Ahmad Hidayat', 'Bank BCA', '1234567890'),
        const Divider(height: 1, indent: 72),
        _buildTransferItem('RS', 'Rina Sari', 'Bank Mandiri', '0987654321'),
        const Divider(height: 1, indent: 72),
        _buildTransferItem('DK', 'Dede Kurniawan', 'Bank BRI', '1122334455'),
      ],
    );
  }

  Widget _buildTransferItem(
    String initials,
    String name,
    String bankName,
    String accountNumber,
  ) {
    return ListTile(
      contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
      leading: CircleAvatar(
        radius: 20,
        backgroundColor: AppColors.lightGrey,
        child: Text(
          initials,
          style: AppTextStyles.medium.copyWith(
            color: AppColors.primary,
            fontWeight: FontWeight.bold,
          ),
        ),
      ),
      title: Text(
        name,
        style: AppTextStyles.medium.copyWith(fontWeight: FontWeight.w600),
      ),
      subtitle: Text(
        '$bankName • $accountNumber',
        style: AppTextStyles.small.copyWith(color: AppColors.grey),
      ),
      onTap: () {},
    );
  }
}
