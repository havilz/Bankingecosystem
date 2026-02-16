import 'package:flutter/material.dart';

import '../../../core/ui/theme/app_colors.dart';
import '../../../core/ui/theme/app_text_styles.dart';

class SukhaScreen extends StatelessWidget {
  const SukhaScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white,
      appBar: AppBar(
        title: Text(
          'Sukha',
          style: AppTextStyles.large.copyWith(
            fontWeight: FontWeight.bold,
            color: AppColors.textPrimary,
          ),
        ),
        backgroundColor: AppColors.white,
        elevation: 0,
      ),
      body: const Center(child: Text('Sukha - Coming Soon')),
    );
  }
}
