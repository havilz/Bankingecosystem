import 'package:flutter/material.dart';

import '../theme/app_colors.dart';
import '../theme/app_text_styles.dart';

class AppPinPad extends StatelessWidget {
  final ValueChanged<int> onNumberPressed;
  final VoidCallback onDeletePressed;

  const AppPinPad({
    super.key,
    required this.onNumberPressed,
    required this.onDeletePressed,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        _buildRow(context, [1, 2, 3]),
        const SizedBox(height: 16),
        _buildRow(context, [4, 5, 6]),
        const SizedBox(height: 16),
        _buildRow(context, [7, 8, 9]),
        const SizedBox(height: 16),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            _buildEmptyButton(),
            _buildNumberButton(context, 0),
            _buildDeleteButton(context),
          ],
        ),
      ],
    );
  }

  Widget _buildRow(BuildContext context, List<int> numbers) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: numbers.map((n) => _buildNumberButton(context, n)).toList(),
    );
  }

  Widget _buildNumberButton(BuildContext context, int number) {
    return Container(
      width: 72,
      height: 72,
      decoration: const BoxDecoration(
        shape: BoxShape.circle,
        color: AppColors.white,
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          borderRadius: BorderRadius.circular(36),
          onTap: () => onNumberPressed(number),
          child: Center(
            child: Text(
              number.toString(),
              style: AppTextStyles.large.copyWith(
                fontSize: 32,
                fontWeight: FontWeight.bold,
                color: AppColors.textPrimary,
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildDeleteButton(BuildContext context) {
    return Container(
      width: 72,
      height: 72,
      decoration: const BoxDecoration(
        shape: BoxShape.circle,
        color: Colors.transparent,
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          borderRadius: BorderRadius.circular(36),
          onTap: onDeletePressed,
          child: const Center(
            child: Icon(
              Icons.backspace_outlined,
              size: 28,
              color: AppColors.textPrimary,
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildEmptyButton() {
    return const SizedBox(width: 72, height: 72);
  }
}
