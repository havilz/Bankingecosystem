import 'package:flutter/material.dart';
import '../../../../../../core/ui/ui.dart';

class NotificationListTab extends StatelessWidget {
  const NotificationListTab({super.key});

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Text(
        'Daftar Notifikasi Sistem Akan Muncul di Sini',
        style: AppTextStyles.medium,
      ),
    );
  }
}
