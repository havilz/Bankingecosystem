import 'package:flutter/material.dart';
import '../theme/app_colors.dart';
import '../theme/app_text_styles.dart';

enum AppButtonType { primary, danger, outline }

class AppButton extends StatelessWidget {
  final String label;
  final VoidCallback? onPressed;
  final AppButtonType type;
  final bool isFullWidth;
  final bool isLoading;

  const AppButton({
    super.key,
    required this.label,
    required this.onPressed,
    this.type = AppButtonType.primary,
    this.isFullWidth = false,
    this.isLoading = false,
  });

  @override
  Widget build(BuildContext context) {
    // Determine Base Colors
    Color backgroundColor;
    Color foregroundColor;
    Color? borderColor;

    switch (type) {
      case AppButtonType.primary:
        backgroundColor = AppColors.primary;
        foregroundColor = AppColors.white;
        break;
      case AppButtonType.danger:
        backgroundColor = AppColors.danger;
        foregroundColor = AppColors.white;
        break;
      case AppButtonType.outline:
        backgroundColor = Colors.transparent;
        foregroundColor = AppColors.textPrimary;
        borderColor = AppColors.primary;
        break;
    }

    // Button Style with Hover Logic (WidgetStateProperty)
    final ButtonStyle style = ButtonStyle(
      backgroundColor: WidgetStateProperty.resolveWith<Color>((
        Set<WidgetState> states,
      ) {
        if (states.contains(WidgetState.disabled)) {
          return AppColors.grey;
        }
        if (type == AppButtonType.outline) {
          // Special Hover Logic for Outline: White -> Blue
          if (states.contains(WidgetState.hovered) ||
              states.contains(WidgetState.pressed)) {
            return AppColors.primary;
          }
          return Colors.transparent; // Default
        }
        // Default for Primary/Danger (Standard interactions)
        if (states.contains(WidgetState.pressed)) {
          return type == AppButtonType.danger
              ? Colors.red[800]!
              : Colors.blue[800]!;
        }
        return backgroundColor;
      }),
      foregroundColor: WidgetStateProperty.resolveWith<Color>((
        Set<WidgetState> states,
      ) {
        if (states.contains(WidgetState.disabled)) {
          return Colors.white;
        }
        if (type == AppButtonType.outline) {
          // Special Hover Logic for Outline: Black -> White
          if (states.contains(WidgetState.hovered) ||
              states.contains(WidgetState.pressed)) {
            return AppColors.white;
          }
          return AppColors.textPrimary; // Default Black
        }
        return foregroundColor;
      }),
      side: type == AppButtonType.outline
          ? WidgetStateProperty.resolveWith<BorderSide?>((states) {
              if (states.contains(WidgetState.hovered) ||
                  states.contains(WidgetState.pressed)) {
                return const BorderSide(color: AppColors.primary);
              }
              // Use the calculated borderColor or default to primary/black
              return BorderSide(color: borderColor ?? AppColors.primary);
            })
          : null,
      shape: WidgetStateProperty.all(
        RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
      ),
      padding: WidgetStateProperty.all(
        const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
      ),
      elevation: WidgetStateProperty.all(
        0,
      ), // Flat style preferred for modern look
    );

    Widget content = Text(
      label,
      style: AppTextStyles.button.copyWith(
        // Dynamic color is handled by foregroundColor, but we can enforce it here if needed.
        // Leaving it to standard style inheriting from button theme.
      ),
    );

    if (isLoading) {
      content = SizedBox(
        height: 20,
        width: 20,
        child: CircularProgressIndicator(
          strokeWidth: 2,
          color: foregroundColor,
        ),
      );
    }

    Widget button = ElevatedButton(
      onPressed: isLoading ? null : onPressed,
      style: style,
      child: content,
    );

    if (isFullWidth) {
      return SizedBox(width: double.infinity, child: button);
    }

    return button;
  }
}
