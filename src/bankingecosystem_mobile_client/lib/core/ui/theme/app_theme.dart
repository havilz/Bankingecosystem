import 'package:flutter/material.dart';
import 'app_colors.dart';
import 'app_text_styles.dart';

class AppTheme {
  static ThemeData get light {
    return ThemeData(
      useMaterial3: true,

      // Basic Colors
      primaryColor: AppColors.primary,
      scaffoldBackgroundColor: AppColors.white,

      // Color Scheme (M3)
      colorScheme: const ColorScheme.light(
        primary: AppColors.primary,
        secondary: AppColors.primaryLight,
        error: AppColors.danger,
        surface: AppColors.white,
        onPrimary: AppColors.white, // Text on primary button
        onSurface: AppColors.textPrimary, // Text on background
      ),

      // Text Theme
      textTheme: const TextTheme(
        headlineLarge: AppTextStyles.large,
        bodyMedium: AppTextStyles.medium,
        bodySmall: AppTextStyles.small,
        labelLarge: AppTextStyles.button, // For buttons
      ),

      // Button Theme
      elevatedButtonTheme: ElevatedButtonThemeData(
        style: ElevatedButton.styleFrom(
          backgroundColor: AppColors.primary, // Blue
          foregroundColor: AppColors.white, // White Text
          textStyle: AppTextStyles.button,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
        ),
      ),

      // Bottom Navigation Theme
      bottomNavigationBarTheme: const BottomNavigationBarThemeData(
        backgroundColor: AppColors.white,
        selectedItemColor: AppColors.bottomNavIcon,
        unselectedItemColor: AppColors.grey,
        showSelectedLabels: true,
        showUnselectedLabels: true,
        type: BottomNavigationBarType.fixed,
      ),
    );
  }
}
