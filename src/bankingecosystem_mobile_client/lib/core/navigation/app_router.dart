import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../features/auth/presentation/login_screen.dart';
import '../../features/dashboard/presentation/dashboard_screen.dart';
import '../../features/product/presentation/product_screen.dart';
import '../../features/qris/presentation/qris_screen.dart';
import '../../features/settings/presentation/settings_screen.dart';
import '../../features/sukha/presentation/sukha_screen.dart';
import '../../features/transfer/presentation/transfer_screen.dart';
import '../../features/transfer/presentation/transfer_new_screen.dart';
import '../../features/transfer/presentation/transfer_amount_screen.dart';
import '../../features/messages/presentation/screens/message_screen.dart';
import 'navigation_shell.dart';

final _rootNavigatorKey = GlobalKey<NavigatorState>();
final _shellNavigatorHomeKey = GlobalKey<NavigatorState>(
  debugLabel: 'shellHome',
);
final _shellNavigatorProductKey = GlobalKey<NavigatorState>(
  debugLabel: 'shellProduct',
);
final _shellNavigatorQrisKey = GlobalKey<NavigatorState>(
  debugLabel: 'shellQris',
);
final _shellNavigatorSukhaKey = GlobalKey<NavigatorState>(
  debugLabel: 'shellSukha',
);
final _shellNavigatorSettingsKey = GlobalKey<NavigatorState>(
  debugLabel: 'shellSettings',
);

/// Riverpod provider for GoRouter.
final goRouterProvider = Provider<GoRouter>((ref) {
  return GoRouter(
    navigatorKey: _rootNavigatorKey,
    initialLocation: '/login',
    debugLogDiagnostics: true,
    routes: [
      GoRoute(path: '/login', builder: (context, state) => const LoginScreen()),
      GoRoute(
        path: '/transfer',
        builder: (context, state) => const TransferScreen(),
        routes: [
          GoRoute(
            path: 'new',
            builder: (context, state) => const TransferNewScreen(),
          ),
          GoRoute(
            path: 'amount',
            builder: (context, state) => const TransferAmountScreen(),
          ),
        ],
      ),
      GoRoute(
        path: '/messages',
        builder: (context, state) => const MessageScreen(),
      ),
      StatefulShellRoute.indexedStack(
        builder: (context, state, navigationShell) {
          return NavigationShell(navigationShell: navigationShell);
        },
        branches: [
          StatefulShellBranch(
            navigatorKey: _shellNavigatorHomeKey,
            routes: [
              GoRoute(
                path: '/home',
                builder: (context, state) => const DashboardScreen(),
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorProductKey,
            routes: [
              GoRoute(
                path: '/product',
                builder: (context, state) => const ProductScreen(),
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorQrisKey,
            routes: [
              GoRoute(
                path: '/qris',
                builder: (context, state) => const QrisScreen(),
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorSukhaKey,
            routes: [
              GoRoute(
                path: '/sukha',
                builder: (context, state) => const SukhaScreen(),
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorSettingsKey,
            routes: [
              GoRoute(
                path: '/settings',
                builder: (context, state) => const SettingsScreen(),
              ),
            ],
          ),
        ],
      ),
    ],
  );
});
