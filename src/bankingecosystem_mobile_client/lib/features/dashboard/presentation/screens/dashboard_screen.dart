import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../core/ui/ui.dart';
import '../providers/dashboard_provider.dart';
import '../widgets/account_card.dart';
import '../widgets/dashboard_header.dart';
import '../widgets/ewallet_card.dart';
import '../widgets/favorite_transaction_card.dart';
import '../widgets/quick_pick_card.dart';

class DashboardScreen extends ConsumerWidget {
  const DashboardScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final dashState = ref.watch(dashboardProvider);
    final customerName = dashState.balance?.customerName ?? 'Nasabah';
    return Scaffold(
      backgroundColor: AppColors.lightGrey,
      body: SafeArea(
        child: Column(
          children: [
            // ===== HEADER =====
            DashboardHeader(username: customerName),

            // ===== BODY =====
            Expanded(
              child: ClipRRect(
                borderRadius: const BorderRadius.only(
                  topLeft: Radius.circular(24),
                  topRight: Radius.circular(24),
                ),
                child: Container(
                  color: AppColors.lightGrey,
                  child: SingleChildScrollView(
                    padding: const EdgeInsets.only(top: 16, bottom: 100),
                    child: Column(
                      children: [
                        // Account Card (Rekening)
                        const AccountCard(),
                        const SizedBox(height: 16),
                        // Favorite Transaction Card
                        const FavoriteTransactionCard(),
                        const SizedBox(height: 16),
                        // Quick Pick Card
                        const QuickPickCard(),
                        const SizedBox(height: 16),
                        // E-Wallet Card
                        const EWalletCard(),
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
