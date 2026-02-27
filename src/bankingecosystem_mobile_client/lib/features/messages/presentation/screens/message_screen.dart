import 'package:flutter/material.dart';
import '../../../../core/ui/ui.dart';

import 'widgets/receipt_list_tab.dart';
import 'widgets/notification_list_tab.dart';
import 'widgets/promo_list_tab.dart';

class MessageScreen extends StatelessWidget {
  const MessageScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 3,
      child: Scaffold(
        backgroundColor: AppColors.white,
        appBar: AppBar(
          backgroundColor: AppColors.white,
          elevation: 0,
          leading: IconButton(
            icon: const Icon(Icons.arrow_back, color: AppColors.textPrimary),
            onPressed: () => Navigator.pop(context),
          ),
          title: Text(
            'Pesan',
            style: AppTextStyles.h2.copyWith(fontWeight: FontWeight.bold),
          ),
          centerTitle: false,
          bottom: const TabBar(
            labelColor: AppColors.primary,
            unselectedLabelColor: AppColors.grey,
            indicatorSize:
                TabBarIndicatorSize.tab, // Makes all indicators the same width
            dividerColor: Colors
                .transparent, // Hide default Material 3 M3 edge-to-edge divider
            splashFactory:
                NoSplash.splashFactory, // Removes the splash effect on click
            overlayColor: WidgetStatePropertyAll(
              Colors.transparent,
            ), // Removes hover/highlight colors
            indicator: UnderlineTabIndicator(
              borderSide: BorderSide(width: 4.0, color: AppColors.primary),
              borderRadius: BorderRadius.only(
                topLeft: Radius.circular(4),
                topRight: Radius.circular(4),
              ),
            ),
            tabs: [
              Tab(text: 'Resi'),
              Tab(text: 'Notifikasi'),
              Tab(text: 'Promo'),
            ],
          ),
        ),
        body: Container(
          color: AppColors.white,
          child: Column(
            children: [
              // Subtle grey line below tabs, not touching edges
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 20),
                child: Divider(
                  height: 2,
                  thickness: 2,
                  color: Colors.grey.withValues(alpha: 0.3),
                ),
              ),
              const Expanded(
                child: TabBarView(
                  children: [
                    ReceiptListTab(),
                    NotificationListTab(),
                    PromoListTab(),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
