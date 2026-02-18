import 'package:flutter/material.dart';

import 'settings_group_card.dart';

class AccountSettingsCard extends StatelessWidget {
  const AccountSettingsCard({super.key});

  @override
  Widget build(BuildContext context) {
    return SettingsGroupCard(
      title: 'Akun',
      children: [
        SettingsItem(
          icon: Icons.person_outline,
          title: 'Nama Panggilan',
          value: 'User',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.email_outlined,
          title: 'Email',
          value: 'user@example.com',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.language,
          title: 'Bahasa',
          value: 'Indonesia',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.folder_shared_outlined,
          title: 'Data Anda',
          onTap: () {},
        ),
      ],
    );
  }
}
