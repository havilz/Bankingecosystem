import 'package:flutter/material.dart';

import 'settings_group_card.dart';

class SecuritySettingsCard extends StatelessWidget {
  const SecuritySettingsCard({super.key});

  @override
  Widget build(BuildContext context) {
    return SettingsGroupCard(
      title: 'Keamanan',
      children: [
        SettingsItem(icon: Icons.lock_outline, title: 'PIN', onTap: () {}),
        SettingsItem(
          icon: Icons.vpn_key_outlined,
          title: 'Password',
          subtitle: 'Untuk login akun',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.password,
          title: 'Transaksi Tanpa PIN',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.phone_android_outlined,
          title: 'Ubah Nomor Handphone',
          onTap: () {},
        ),
      ],
    );
  }
}
