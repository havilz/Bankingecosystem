import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/ui.dart';
import '../../../auth/presentation/providers/auth_provider.dart';

/// Dashboard header widget.
/// Left: Greeting with username.
/// Right: Notification (mail) icon + Logout icon.
class DashboardHeader extends ConsumerWidget {
  final String username;

  const DashboardHeader({super.key, required this.username});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 12),
      child: Row(
        children: [
          // ===== LEFT: Greeting =====
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  'Hi, $username',
                  style: AppTextStyles.large.copyWith(
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                    color: AppColors.textPrimary,
                  ),
                  overflow: TextOverflow.ellipsis,
                ),
              ],
            ),
          ),

          // ===== RIGHT: Icons =====
          // Notification (mail) icon
          IconButton(
            onPressed: () {
              context.push('/messages');
            },
            icon: const Icon(Icons.mail_outline),
            color: AppColors.textPrimary,
            iconSize: 24,
            tooltip: 'Notifications',
            splashRadius: 22,
          ),
          const SizedBox(width: 4),

          // Logout icon
          IconButton(
            onPressed: () => _handleLogout(context, ref),
            icon: const Icon(Icons.logout),
            color: AppColors.danger,
            iconSize: 24,
            tooltip: 'Logout',
            splashRadius: 22,
          ),
        ],
      ),
    );
  }

  void _handleLogout(BuildContext context, WidgetRef ref) {
    final router = GoRouter.of(context);
    ref.read(authProvider.notifier).logout(router);
  }
}
