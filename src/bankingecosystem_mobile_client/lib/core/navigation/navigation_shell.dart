import 'dart:async';

import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:skeletonizer/skeletonizer.dart';

import '../ui/theme/app_colors.dart';
import '../ui/theme/app_text_styles.dart';

/// Builds the "shell" for the app with a styled BottomNavigationBar.
/// Uses Stack overlay so the navbar floats above the body without
/// any grey wrapper/footer effect from Scaffold's bottomNavigationBar.
///
/// Implements Skeleton Loading on initial mount and tab switching.
class NavigationShell extends StatefulWidget {
  final StatefulNavigationShell navigationShell;

  const NavigationShell({required this.navigationShell, Key? key})
    : super(key: key ?? const ValueKey<String>('NavigationShell'));

  @override
  State<NavigationShell> createState() => _NavigationShellState();
}

class _NavigationShellState extends State<NavigationShell> {
  bool _isLoading = true;
  Timer? _loadingTimer;

  @override
  void initState() {
    super.initState();
    // Force initial loading when shell is first mounted
    _isLoading = true;
    _startLoadingTimer(const Duration(milliseconds: 1500));
  }

  @override
  void dispose() {
    _loadingTimer?.cancel();
    super.dispose();
  }

  void _startLoadingTimer(Duration duration) {
    _loadingTimer?.cancel();
    _loadingTimer = Timer(duration, () {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    });
  }

  void _onTapNav(int index) {
    if (index == widget.navigationShell.currentIndex) return;

    // Switch tab immediately
    widget.navigationShell.goBranch(
      index,
      initialLocation: index == widget.navigationShell.currentIndex,
    );

    // Start loading effect on the NEW tab
    setState(() {
      _isLoading = true;
    });

    _startLoadingTimer(const Duration(milliseconds: 800));
  }

  @override
  Widget build(BuildContext context) {
    final currentIndex = widget.navigationShell.currentIndex;

    return Scaffold(
      backgroundColor: AppColors.lightGrey,
      body: Stack(
        children: [
          // ===== Main content (Actual Screens) with Skeletonizer =====
          Skeletonizer(enabled: _isLoading, child: widget.navigationShell),

          // ===== Floating bottom navbar =====
          Positioned(
            left: 12,
            right: 12,
            bottom: MediaQuery.of(context).padding.bottom + 8,
            child: Container(
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
                border: Border.all(
                  color: AppColors.grey.withValues(alpha: 0.15),
                ),
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
          ),
        ],
      ),
    );
  }

  // ===== QRIS center button (elevated) =====
  Widget _buildQrisItem(int currentIndex) {
    final isActive = currentIndex == 2;
    return Expanded(
      child: GestureDetector(
        behavior: HitTestBehavior.opaque,
        onTap: () => _onTapNav(2),
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
        onTap: () => _onTapNav(index),
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
