import 'package:flutter/material.dart';

import '../../../../core/ui/ui.dart';
import '../widgets/account_settings_card.dart';
import '../widgets/feature_settings_card.dart';
import '../widgets/info_settings_card.dart';
import '../widgets/instant_access_card.dart';
import '../widgets/security_settings_card.dart';

class SettingsScreen extends StatelessWidget {
  const SettingsScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.lightGrey,
      body: SafeArea(
        child: SingleChildScrollView(
          padding: const EdgeInsets.only(bottom: 100),
          child: Column(
            children: const [
              // ===== Search Bar (scrolls with content) =====
              AppSearchBar(hintText: 'Cari pengaturan yang ingin diubah'),

              // ===== Settings Cards =====
              InstantAccessSettingsCard(),
              AccountSettingsCard(),
              FeatureSettingsCard(),
              SecuritySettingsCard(),
              InfoSettingsCard(),
            ],
          ),
        ),
      ),
    );
  }
}
