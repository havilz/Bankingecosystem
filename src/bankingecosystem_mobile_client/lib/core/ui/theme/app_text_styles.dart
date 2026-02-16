import 'package:flutter/material.dart';
import 'app_colors.dart';

class AppTextStyles {
  // Base Font Family (e.g., Roboto or Inter - using default for now)
  static const String fontFamily = 'Roboto'; // Default Android font

  // Large Text (Headlines)
  static const TextStyle large = TextStyle(
    fontSize: 24,
    fontWeight: FontWeight.bold,
    color: AppColors.textPrimary,
  );

  // Medium Text (Body, Subtitles)
  static const TextStyle medium = TextStyle(
    fontSize: 16,
    fontWeight: FontWeight.normal,
    color: AppColors.textPrimary,
  );

  // Small Text (Captions, Labels)
  static const TextStyle small = TextStyle(
    fontSize: 12,
    fontWeight: FontWeight.normal,
    color: AppColors.grey,
  );

  // Button Text (Usually Medium/Bold)
  static const TextStyle button = TextStyle(
    fontSize: 16,
    fontWeight: FontWeight.w600,
    color: AppColors.textInverse, // White by default for primary buttons
  );
}
