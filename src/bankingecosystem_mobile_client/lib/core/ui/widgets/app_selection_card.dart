import 'package:flutter/material.dart';
import '../theme/app_colors.dart';
import '../theme/app_text_styles.dart';

class AppSelectionCard extends StatelessWidget {
  final String label;
  final String value;
  final Widget? trailingIcon;
  final VoidCallback? onTap;
  final bool highlightValue;

  const AppSelectionCard({
    super.key,
    required this.label,
    required this.value,
    this.trailingIcon,
    this.onTap,
    this.highlightValue = false,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          color: AppColors.white,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(color: AppColors.lightGrey, width: 1),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  label,
                  style: AppTextStyles.small.copyWith(color: AppColors.grey),
                ),
                const SizedBox(height: 4),
                Text(
                  value,
                  style: AppTextStyles.medium.copyWith(
                    fontWeight: FontWeight.bold,
                    color: highlightValue
                        ? AppColors.primary
                        : AppColors.textPrimary,
                  ),
                ),
              ],
            ),
            trailingIcon ?? const SizedBox.shrink(),
          ],
        ),
      ),
    );
  }
}
