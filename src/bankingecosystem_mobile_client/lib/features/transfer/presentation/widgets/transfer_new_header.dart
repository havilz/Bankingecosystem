import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class TransferNewHeader extends StatelessWidget {
  const TransferNewHeader({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      color: AppColors.primaryLight, // Light Blue
      padding: const EdgeInsets.fromLTRB(16, 60, 16, 16),
      height: 180, // Same height as TransferHeader
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              // Back Button (Same as TransferHeader)
              IconButton(
                icon: const Icon(Icons.arrow_back, color: AppColors.white),
                onPressed: () => context.pop(),
              ),
              // No Scheduled Transfer Button here
            ],
          ),
          const SizedBox(height: 24),
          Padding(
            padding: const EdgeInsets.only(left: 16),
            child: Text(
              'Transfer',
              style: AppTextStyles.h2.copyWith(
                color: AppColors.white,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
