import 'package:flutter/material.dart';
import '../../../../../../../core/ui/ui.dart';

class TransactionDateHeader extends StatelessWidget {
  final String dateString;

  const TransactionDateHeader({super.key, required this.dateString});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(top: 24.0, bottom: 8.0),
      child: Text(
        dateString,
        style: AppTextStyles.medium.copyWith(
          color: AppColors.grey,
          fontWeight: FontWeight.w500,
        ),
      ),
    );
  }
}
