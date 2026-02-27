import 'package:flutter/material.dart';
import '../../../../../../core/ui/ui.dart';

import 'receipt_tab/transaction_date_header.dart';
import 'receipt_tab/transaction_list_item.dart';

class ReceiptListTab extends StatelessWidget {
  const ReceiptListTab({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        // Top Bar: "Semua Transaksi" + Filter Icon (STATIC, DOES NOT SCROLL)
        Padding(
          padding: const EdgeInsets.fromLTRB(20, 16, 20, 8),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                'Semua Transaksi',
                style: AppTextStyles.h2.copyWith(fontWeight: FontWeight.bold),
              ),
              IconButton(
                icon: const Icon(
                  Icons.filter_alt_outlined,
                  color: AppColors.primary,
                ),
                onPressed: () {
                  // TODO: Show filter options
                },
              ),
            ],
          ),
        ),

        // --- SCROLLABLE TRANSACTIONS LIST ---
        Expanded(
          child: ListView(
            padding: const EdgeInsets.symmetric(horizontal: 20),
            children: [
              // --- Mock Data Section 1: 27 Feb 2026 ---
              const TransactionDateHeader(dateString: '27 Feb 2026'),
              const TransactionListItem(
                icon: Icons.qr_code_scanner_outlined,
                title: 'Bank BCA - TOKO FIRZAN 3',
                status: 'Berhasil',
                amount: 18000,
                isCredit: false,
              ),

              // --- Mock Data Section 2: 26 Feb 2026 ---
              const TransactionDateHeader(dateString: '26 Feb 2026'),
              const TransactionListItem(
                icon: Icons.qr_code_scanner_outlined,
                title: 'Bank BCA - TOKO FIRZAN 3',
                status: 'Berhasil',
                amount: 10000,
                isCredit: false,
              ),
              const TransactionListItem(
                icon: Icons.qr_code_scanner_outlined,
                title: 'Bank BCA - TOKO FIRZAN 3',
                status: 'Berhasil',
                amount: 10000,
                isCredit: false,
              ),

              // --- Mock Data Section 3: 25 Feb 2026 ---
              const TransactionDateHeader(dateString: '25 Feb 2026'),
              const TransactionListItem(
                icon: Icons.qr_code_scanner_outlined,
                title: 'Bank BCA - TOKO FIRZAN 3',
                status: 'Berhasil',
                amount: 7000,
                isCredit: false,
              ),
              const TransactionListItem(
                icon: Icons.qr_code_scanner_outlined,
                title: 'Bank BCA - TOKO FIRZAN 3',
                status: 'Berhasil',
                amount: 14000,
                isCredit: false,
              ),

              // Add a bit of padding at the bottom for scrolling comfort
              const SizedBox(height: 40),
            ],
          ),
        ),
      ],
    );
  }
}
