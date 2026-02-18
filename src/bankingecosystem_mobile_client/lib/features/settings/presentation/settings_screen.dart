import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/widgets/app_search_bar.dart';
import 'widgets/account_settings_card.dart';
import 'widgets/feature_settings_card.dart';
import 'widgets/info_settings_card.dart';
import 'widgets/instant_access_card.dart';
import 'widgets/security_settings_card.dart';

class SettingsScreen extends StatelessWidget {
  const SettingsScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.lightGrey,
      body: SafeArea(
        child: Column(
          children: [
            // ===== Search Bar =====
            const AppSearchBar(hintText: 'Cari pengaturan yang ingin diubah'),

            // ===== Settings List =====
            Expanded(
              child: SingleChildScrollView(
                padding: const EdgeInsets.only(bottom: 100), // Space for navbar
                child: Column(
                  children: const [
                    InstantAccessSettingsCard(),
                    AccountSettingsCard(),
                    FeatureSettingsCard(),
                    SecuritySettingsCard(),
                    InfoSettingsCard(),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
