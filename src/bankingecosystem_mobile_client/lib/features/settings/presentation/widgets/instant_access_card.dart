import 'package:flutter/material.dart';

import 'settings_group_card.dart';

class InstantAccessSettingsCard extends StatelessWidget {
  const InstantAccessSettingsCard({super.key});

  @override
  Widget build(BuildContext context) {
    return SettingsGroupCard(
      title: 'Feature Tanpa Login',
      children: [
        SettingsItem(
          icon: Icons.flash_on_outlined,
          title: 'Instant Access',
          subtitle: 'Atur akses cepat di halaman login',
          onTap: () {},
        ),
      ],
    );
  }
}
