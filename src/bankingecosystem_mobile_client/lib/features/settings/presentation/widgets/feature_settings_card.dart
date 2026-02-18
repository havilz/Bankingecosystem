import 'package:flutter/material.dart';

import 'settings_group_card.dart';

class FeatureSettingsCard extends StatelessWidget {
  const FeatureSettingsCard({super.key});

  @override
  Widget build(BuildContext context) {
    return SettingsGroupCard(
      title: 'Fitur',
      children: [
        SettingsItem(icon: Icons.sync_alt, title: 'BI-FAST', onTap: () {}),
        SettingsItem(
          icon: Icons.account_balance_wallet_outlined,
          title: 'Sumber Dana Utama',
          value: 'Tabungan NOW',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.receipt_long_outlined,
          title: 'Terima Tagihan',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.dashboard_customize_outlined,
          title: 'Produk di Beranda',
          onTap: () {},
        ),
      ],
    );
  }
}
