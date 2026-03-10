import 'package:flutter/material.dart';

import '../theme/app_colors.dart';

class AppPinIndicator extends StatelessWidget {
  final int length;
  final int maxLength;

  const AppPinIndicator({super.key, required this.length, this.maxLength = 6});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: List.generate(maxLength, (index) {
        final isFilled = index < length;
        return Container(
          margin: const EdgeInsets.symmetric(horizontal: 12),
          width: 16,
          height: 16,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            color: isFilled ? AppColors.primary : AppColors.lightGrey,
            border: Border.all(
              color: isFilled
                  ? AppColors.primary
                  : AppColors.grey.withValues(alpha: 0.5),
              width: 1,
            ),
          ),
        );
      }),
    );
  }
}
