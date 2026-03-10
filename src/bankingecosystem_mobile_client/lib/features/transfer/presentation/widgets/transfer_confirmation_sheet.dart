import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import '../../../../core/ui/ui.dart'; // import app_colors, app_text_styles, app_detail_row, app_button

class TransferConfirmationSheet {
  static void show({
    required BuildContext context,
    required int amount,
    required int fee,
    required int total,
    required String bankName,
    required String targetAccountNumber,
    required String recipientName,
    required String? selectedPurpose,
    required String Function(int) formatCurrency,
  }) {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => Container(
        padding: const EdgeInsets.only(
          top: 24,
          left: 24,
          right: 24,
          bottom: 32,
        ),
        decoration: const BoxDecoration(
          color: AppColors.white,
          borderRadius: BorderRadius.vertical(top: Radius.circular(24)),
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  'Konfirmasi Transfer',
                  style: AppTextStyles.h2.copyWith(fontWeight: FontWeight.bold),
                ),
                GestureDetector(
                  onTap: () => Navigator.pop(ctx),
                  child: const Icon(Icons.close, color: AppColors.grey),
                ),
              ],
            ),
            const SizedBox(height: 24),
            AppDetailRow(
              label: 'Nominal',
              value: formatCurrency(amount),
              isBold: true,
            ),
            const SizedBox(height: 12),
            AppDetailRow(label: 'Biaya Admin', value: formatCurrency(fee)),
            const Padding(
              padding: EdgeInsets.symmetric(vertical: 16),
              child: Divider(color: AppColors.lightGrey, height: 1),
            ),
            AppDetailRow(
              label: 'Total Transaksi',
              value: formatCurrency(total),
              isBold: true,
              valueColor: AppColors.primary,
            ),
            const SizedBox(height: 24),
            Text(
              'Detail Tujuan',
              style: AppTextStyles.medium.copyWith(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),
            AppDetailRow(label: 'Bank', value: bankName),
            const SizedBox(height: 12),
            AppDetailRow(label: 'Nomor Rekening', value: targetAccountNumber),
            const SizedBox(height: 12),
            AppDetailRow(label: 'Nama Penerima', value: recipientName),
            const SizedBox(height: 12),
            AppDetailRow(label: 'Catatan', value: selectedPurpose ?? '-'),
            const SizedBox(height: 32),
            SafeArea(
              child: AppButton(
                label: 'Konfirmasi & Lanjut',
                isFullWidth: true,
                onPressed: () {
                  Navigator.pop(ctx);
                  context.push(
                    '/transfer/pin',
                    extra: {
                      'amount': amount,
                      'fee': fee,
                      'total': total,
                      'bankName': bankName,
                      'targetAccountNumber': targetAccountNumber,
                      'recipientName': recipientName,
                    },
                  );
                },
              ),
            ),
          ],
        ),
      ),
    );
  }
}
