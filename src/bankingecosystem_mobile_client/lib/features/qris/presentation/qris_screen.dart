import 'package:flutter/material.dart';

import '../../../core/ui/ui.dart';

class QrisScreen extends StatelessWidget {
  const QrisScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white,
      appBar: AppBar(
        title: Text(
          'QRIS',
          style: AppTextStyles.large.copyWith(
            fontWeight: FontWeight.bold,
            color: AppColors.textPrimary,
          ),
        ),
        backgroundColor: AppColors.white,
        elevation: 0,
      ),
      body: const Center(child: Text('QRIS Scanner - Coming Soon')),
    );
  }
}
