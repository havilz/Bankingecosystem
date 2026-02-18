import 'package:flutter/material.dart';

import 'settings_group_card.dart';

class InfoSettingsCard extends StatelessWidget {
  const InfoSettingsCard({super.key});

  @override
  Widget build(BuildContext context) {
    return SettingsGroupCard(
      title: 'Informasi',
      children: [
        SettingsItem(icon: Icons.help_outline, title: 'FAQ', onTap: () {}),
        SettingsItem(
          icon: Icons.description_outlined,
          title: 'Syarat dan Ketentuan',
          onTap: () {},
        ),
        SettingsItem(
          icon: Icons.info_outline,
          title: 'Tentang Aplikasi',
          value: 'v1.0.0',
          onTap: () {},
        ),
      ],
    );
  }
}
