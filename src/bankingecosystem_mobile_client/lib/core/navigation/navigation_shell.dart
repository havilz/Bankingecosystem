import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../ui/theme/app_colors.dart';
import '../ui/theme/app_text_styles.dart';

/// Builds the "shell" for the app with a styled BottomNavigationBar.
/// QRIS icon is elevated above the navbar (floating center button).
class NavigationShell extends StatelessWidget {
  final StatefulNavigationShell navigationShell;

  const NavigationShell({required this.navigationShell, Key? key})
    : super(key: key ?? const ValueKey<String>('NavigationShell'));

  @override
  Widget build(BuildContext context) {
    final currentIndex = navigationShell.currentIndex;

    return Scaffold(
      body: navigationShell,
      extendBody: true,
      bottomNavigationBar: Container(
        margin: const EdgeInsets.fromLTRB(12, 0, 12, 12),
        height: 72,
        decoration: BoxDecoration(
          color: AppColors.white,
          borderRadius: BorderRadius.circular(24),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withValues(alpha: 0.1),
              blurRadius: 16,
              offset: const Offset(0, -2),
            ),
          ],
          border: Border.all(color: AppColors.grey.withValues(alpha: 0.15)),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            _buildNavItem(
              0,
              Icons.home_outlined,
              Icons.home,
              'Home',
              currentIndex,
            ),
            _buildNavItem(
              1,
              Icons.credit_card_outlined,
              Icons.credit_card,
              'Product',
              currentIndex,
            ),
            _buildQrisItem(currentIndex),
            _buildNavItem(
              3,
              Icons.favorite_outline,
              Icons.favorite,
              'Sukha',
              currentIndex,
            ),
            _buildNavItem(
              4,
              Icons.settings_outlined,
              Icons.settings,
              'Settings',
              currentIndex,
            ),
          ],
        ),
      ),
    );
  }

  // ===== QRIS center button (elevated) =====
  Widget _buildQrisItem(int currentIndex) {
    final isActive = currentIndex == 2;
    return Expanded(
      child: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onTap: () =>
            navigationShell.goBranch(2, initialLocation: 2 == currentIndex),
        child: Transform.translate(
          offset: const Offset(0, -16),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Container(
                width: 50,
                height: 50,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  gradient: const LinearGradient(
                    colors: [AppColors.primary, AppColors.primaryDark],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  boxShadow: [
                    BoxShadow(
                      color: AppColors.primary.withValues(alpha: 0.35),
                      blurRadius: 10,
                      offset: const Offset(0, 4),
                    ),
                  ],
                  border: Border.all(color: AppColors.white, width: 3),
                ),
                child: const Icon(
                  Icons.qr_code_scanner,
                  color: AppColors.white,
                  size: 24,
                ),
              ),
              const SizedBox(height: 2),
              Text(
                'QRIS',
                style: AppTextStyles.small.copyWith(
                  fontSize: 11,
                  fontWeight: isActive ? FontWeight.w600 : FontWeight.normal,
                  color: isActive ? AppColors.primary : AppColors.grey,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  // ===== Regular nav item builder =====
  Widget _buildNavItem(
    int index,
    IconData outlinedIcon,
    IconData filledIcon,
    String label,
    int currentIndex,
  ) {
    final isActive = index == currentIndex;
    return Expanded(
      child: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onTap: () => navigationShell.goBranch(
          index,
          initialLocation: index == currentIndex,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              isActive ? filledIcon : outlinedIcon,
              color: isActive ? AppColors.primary : AppColors.grey,
              size: 24,
            ),
            const SizedBox(height: 2),
            Text(
              label,
              style: AppTextStyles.small.copyWith(
                fontSize: 11,
                fontWeight: isActive ? FontWeight.w600 : FontWeight.normal,
                color: isActive ? AppColors.primary : AppColors.grey,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
