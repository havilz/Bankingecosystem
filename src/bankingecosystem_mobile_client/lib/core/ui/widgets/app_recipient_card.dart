import 'package:flutter/material.dart';
import '../theme/app_colors.dart';
import '../theme/app_text_styles.dart';

class AppRecipientCard extends StatelessWidget {
  final String recipientName;
  final String bankName;
  final String accountNumber;

  const AppRecipientCard({
    super.key,
    required this.recipientName,
    required this.bankName,
    required this.accountNumber,
  });

  @override
  Widget build(BuildContext context) {
    // Determine initials
    final names = recipientName.split(' ');
    String initials = '';
    if (names.isNotEmpty && names[0].isNotEmpty) {
      initials += names[0][0];
    }
    if (names.length > 1 && names[1].isNotEmpty) {
      initials += names[1][0];
    }

    return Column(
      children: [
        CircleAvatar(
          radius: 32,
          backgroundColor: AppColors.lightGrey,
          child: Text(
            initials.toUpperCase(),
            style: AppTextStyles.h2.copyWith(
              color: AppColors.grey,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
        const SizedBox(height: 16),
        Text(
          recipientName,
          style: AppTextStyles.large.copyWith(fontWeight: FontWeight.bold),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 4),
        Text(
          '$bankName - $accountNumber',
          style: AppTextStyles.medium.copyWith(color: AppColors.grey),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }
}
