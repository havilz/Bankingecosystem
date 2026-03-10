import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/ui.dart';
import '../../../dashboard/presentation/providers/dashboard_provider.dart';

class TransferReceiptScreen extends ConsumerStatefulWidget {
  final Map<String, dynamic> transactionDetails;

  const TransferReceiptScreen({super.key, required this.transactionDetails});

  @override
  ConsumerState<TransferReceiptScreen> createState() =>
      _TransferReceiptScreenState();
}

class _TransferReceiptScreenState extends ConsumerState<TransferReceiptScreen> {
  final DraggableScrollableController _sheetController =
      DraggableScrollableController();

  @override
  void dispose() {
    _sheetController.dispose();
    super.dispose();
  }

  void _onClose() {
    ref.invalidate(dashboardProvider);
    context.go('/home');
  }

  @override
  Widget build(BuildContext context) {
    final amount = widget.transactionDetails['amount'] as int? ?? 0;
    final fee = widget.transactionDetails['fee'] as int? ?? 0;
    final total = widget.transactionDetails['total'] as int? ?? 0;
    final bankName = widget.transactionDetails['bankName'] as String? ?? '-';
    final targetAccountNumber =
        widget.transactionDetails['targetAccountNumber'] as String? ?? '-';
    final recipientName =
        widget.transactionDetails['recipientName'] as String? ?? '-';

    final rawDate = widget.transactionDetails['transactionTime'] as String?;
    final now = rawDate != null
        ? DateTime.tryParse(rawDate) ?? DateTime.now()
        : DateTime.now();

    // Formatting Date: 08 Mar 2026 • 00:41:55 WIB
    final monthNames = [
      '',
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'Mei',
      'Jun',
      'Jul',
      'Agu',
      'Sep',
      'Okt',
      'Nov',
      'Des',
    ];
    final dateString =
        '${now.day.toString().padLeft(2, '0')} ${monthNames[now.month]} ${now.year} • ${now.hour.toString().padLeft(2, '0')}:${now.minute.toString().padLeft(2, '0')}:${now.second.toString().padLeft(2, '0')} WIB';

    final refString =
        widget.transactionDetails['referenceNumber'] as String? ??
        'TRX${now.millisecondsSinceEpoch}';

    final dashState = ref.read(dashboardProvider);
    final senderName = dashState.balance?.customerName ?? 'PENGIRIM';
    final senderAccount = dashState.balance?.accountNumber ?? '-';

    String formatCurrency(int value) {
      if (value == 0) return 'Rp 0';
      final valueStr = value.toString();
      final buffer = StringBuffer();
      int count = 0;
      for (int i = valueStr.length - 1; i >= 0; i--) {
        if (count != 0 && count % 3 == 0) buffer.write('.');
        buffer.write(valueStr[i]);
        count++;
      }
      return 'Rp ${buffer.toString().split('').reversed.join()}';
    }

    return Scaffold(
      backgroundColor: const Color(0xFFF3F5F8), // Light grayish blue background
      body: SafeArea(
        child: Stack(
          children: [
            // --- TOP LAYER: Background Info ---
            Positioned(
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              child: Column(
                mainAxisAlignment: MainAxisAlignment.start,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  // (Close button moved to top of stack)
                  const SizedBox(height: 16),
                  // Green Checkmark
                  Container(
                    width: 72,
                    height: 72,
                    decoration: const BoxDecoration(
                      color: Colors.green,
                      shape: BoxShape.circle,
                    ),
                    child: const Icon(
                      Icons.check,
                      color: Colors.white,
                      size: 48,
                    ),
                  ),
                  const SizedBox(height: 24),
                  Text(
                    'Transfer Berhasil!',
                    style: AppTextStyles.h2.copyWith(
                      fontWeight: FontWeight.bold,
                      fontSize: 26,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Text(
                    dateString,
                    style: AppTextStyles.medium.copyWith(color: AppColors.grey),
                  ),

                  // Empty space/Illustration area
                  Expanded(
                    child: Center(
                      child: Column(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Icon(
                            Icons.receipt_long_rounded,
                            size: 100,
                            color: AppColors.primary.withValues(alpha: 0.1),
                          ),
                          const SizedBox(height: 16),
                          GestureDetector(
                            onTap: () {
                              _sheetController.animateTo(
                                0.9,
                                duration: const Duration(milliseconds: 300),
                                curve: Curves.easeOut,
                              );
                            },
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Text(
                                  'Lihat Resi',
                                  style: AppTextStyles.medium.copyWith(
                                    fontWeight: FontWeight.bold,
                                    color: AppColors.primary,
                                  ),
                                ),
                                const SizedBox(width: 4),
                                const Icon(
                                  Icons.keyboard_arrow_up,
                                  color: AppColors.primary,
                                ),
                              ],
                            ),
                          ),
                          const SizedBox(
                            height: 40,
                          ), // Push standard sheet down
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ),

            // --- BOTTOM LAYER: The Draggable Receipt Card ---
            DraggableScrollableSheet(
              controller: _sheetController,
              initialChildSize: 0.42, // Shows basic info
              minChildSize: 0.42,
              maxChildSize: 1.0, // Completely covers the screen
              snap: true,
              builder: (context, scrollController) {
                return Container(
                  decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: const BorderRadius.vertical(
                      top: Radius.circular(24),
                    ),
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black.withValues(alpha: 0.1),
                        blurRadius: 15,
                        offset: const Offset(0, -2),
                      ),
                    ],
                  ),
                  child: SingleChildScrollView(
                    controller: scrollController,
                    physics: const ClampingScrollPhysics(),
                    child: Padding(
                      padding: const EdgeInsets.symmetric(
                        horizontal: 24.0,
                        vertical: 16.0,
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.stretch,
                        children: [
                          // Drag Handle
                          Center(
                            child: Container(
                              width: 40,
                              height: 5,
                              decoration: BoxDecoration(
                                color: Colors.grey[300],
                                borderRadius: BorderRadius.circular(10),
                              ),
                            ),
                          ),
                          const SizedBox(height: 24),

                          // --- RECIPIENT ---
                          Center(
                            child: Text(
                              'Penerima',
                              style: AppTextStyles.medium.copyWith(
                                color: AppColors.grey,
                              ),
                            ),
                          ),
                          const SizedBox(height: 8),
                          Center(
                            child: Text(
                              recipientName.toUpperCase(),
                              style: AppTextStyles.large.copyWith(
                                fontWeight: FontWeight.bold,
                                fontSize: 22,
                              ),
                              textAlign: TextAlign.center,
                            ),
                          ),
                          const SizedBox(height: 4),
                          Center(
                            child: Text(
                              bankName.toUpperCase(),
                              style: AppTextStyles.medium.copyWith(
                                color: AppColors.textSecondary,
                              ),
                            ),
                          ),
                          const SizedBox(height: 4),
                          Center(
                            child: Text(
                              'Rekening • $targetAccountNumber',
                              style: AppTextStyles.small.copyWith(
                                color: AppColors.grey,
                              ),
                            ),
                          ),

                          const SizedBox(height: 24),

                          // --- AMOUNT ---
                          Center(
                            child: Text(
                              'Nominal Transaksi',
                              style: AppTextStyles.medium.copyWith(
                                color: AppColors.grey,
                              ),
                            ),
                          ),
                          const SizedBox(height: 8),
                          Center(
                            child: Text(
                              formatCurrency(amount),
                              style: AppTextStyles.large.copyWith(
                                fontWeight: FontWeight.bold,
                                fontSize: 32,
                              ),
                            ),
                          ),

                          const SizedBox(height: 32),
                          const Divider(height: 1, color: AppColors.lightGrey),
                          const SizedBox(height: 24),

                          // --- EXPANDED DETAILS ---
                          // These are seen when scrolling down (expanding sheet)
                          Text(
                            'Detail Transaksi',
                            style: AppTextStyles.medium.copyWith(
                              color: AppColors.grey,
                            ),
                          ),
                          const SizedBox(height: 16),
                          _buildDetailRow(
                            'Total Transaksi',
                            formatCurrency(total),
                            isBold: true,
                          ),
                          const SizedBox(height: 8),
                          _buildDetailRow('Biaya Admin', formatCurrency(fee)),

                          const SizedBox(height: 24),
                          Text(
                            'Sumber Dana',
                            style: AppTextStyles.medium.copyWith(
                              color: AppColors.grey,
                            ),
                          ),
                          const SizedBox(height: 16),
                          Text(
                            senderName.toUpperCase(),
                            style: AppTextStyles.medium.copyWith(
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          const SizedBox(height: 4),
                          Text(
                            'Bank Ecosystem • $senderAccount',
                            style: AppTextStyles.small.copyWith(
                              color: AppColors.textSecondary,
                            ),
                          ),
                          const SizedBox(height: 32),

                          // Gray extra box
                          Container(
                            padding: const EdgeInsets.all(16),
                            decoration: BoxDecoration(
                              color: AppColors.lightGrey,
                              borderRadius: BorderRadius.circular(12),
                            ),
                            child: Column(
                              children: [
                                _buildDetailRow('No. Referensi', refString),
                                const SizedBox(height: 12),
                                _buildDetailRow(
                                  'Jenis Transaksi',
                                  'Transfer Antar Rekening',
                                ),
                              ],
                            ),
                          ),

                          const SizedBox(height: 32),

                          // Share Button
                          OutlinedButton.icon(
                            onPressed: () {
                              ScaffoldMessenger.of(context).showSnackBar(
                                const SnackBar(
                                  content: Text('Fitur bagikan segera hadir'),
                                ),
                              );
                            },
                            icon: const Icon(
                              Icons.share_outlined,
                              color: AppColors.primary,
                            ),
                            label: Text(
                              'Bagikan Resi',
                              style: AppTextStyles.button.copyWith(
                                color: AppColors.primary,
                              ),
                            ),
                            style: OutlinedButton.styleFrom(
                              side: const BorderSide(color: AppColors.primary),
                              padding: const EdgeInsets.symmetric(vertical: 16),
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(8),
                              ),
                            ),
                          ),
                          const SizedBox(height: 32), // padding at bottom
                        ],
                      ),
                    ),
                  ),
                );
              },
            ),

            // --- PERMANENT CLOSE BUTTON ---
            Positioned(
              top: 8.0,
              right: 16.0,
              child: IconButton(
                icon: const Icon(
                  Icons.close,
                  color: AppColors.textPrimary,
                  size: 28,
                ),
                onPressed: _onClose,
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildDetailRow(String label, String value, {bool isBold = false}) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: AppTextStyles.medium.copyWith(color: AppColors.textSecondary),
        ),
        const SizedBox(width: 16),
        Expanded(
          child: Text(
            value,
            textAlign: TextAlign.right,
            style: AppTextStyles.medium.copyWith(
              fontWeight: isBold ? FontWeight.bold : FontWeight.w500,
            ),
          ),
        ),
      ],
    );
  }
}
