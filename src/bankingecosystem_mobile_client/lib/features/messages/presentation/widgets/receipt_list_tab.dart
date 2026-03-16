import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../../core/ui/ui.dart';
import '../../../../../features/dashboard/domain/entities/dashboard_entities.dart';
import '../providers/history_provider.dart';
import 'receipt_tab/transaction_date_header.dart';
import 'receipt_tab/transaction_list_item.dart';

class ReceiptListTab extends ConsumerWidget {
  const ReceiptListTab({super.key});

  // Group a sorted list of transactions by calendar date
  Map<String, List<TransactionEntity>> _groupByDate(
    List<TransactionEntity> transactions,
  ) {
    final Map<String, List<TransactionEntity>> grouped = {};
    for (final tx in transactions) {
      final date = tx.createdAt.toLocal();
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
      final key =
          '${date.day.toString().padLeft(2, '0')} ${monthNames[date.month]} ${date.year}';
      grouped.putIfAbsent(key, () => []).add(tx);
    }
    return grouped;
  }

  // Map transactionType → icon
  IconData _iconFor(String type) {
    switch (type) {
      case 'Transfer':
        return Icons.swap_horiz_rounded;
      case 'Withdrawal':
        return Icons.money_off_rounded;
      default:
        return Icons.receipt_long_outlined;
    }
  }

  // Map transactionType → label shown in title
  String _titleFor(TransactionEntity tx) {
    switch (tx.transactionType) {
      case 'Transfer':
        final target = tx.targetAccountNumber ?? '-';
        return 'Transfer ke $target';
      case 'Withdrawal':
        return 'Penarikan ATM';
      default:
        return tx.transactionType;
    }
  }

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final state = ref.watch(historyProvider);

    return Column(
      children: [
        // Top Bar: static header + filter icon
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

        // Content area
        Expanded(child: _buildBody(context, ref, state)),
      ],
    );
  }

  Widget _buildBody(BuildContext context, WidgetRef ref, HistoryState state) {
    // --- Loading ---
    if (state.isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    // --- Error ---
    if (state.error != null) {
      return Center(
        child: Padding(
          padding: const EdgeInsets.all(32.0),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(Icons.error_outline, size: 48, color: Colors.red.shade300),
              const SizedBox(height: 16),
              Text(
                state.error!,
                textAlign: TextAlign.center,
                style: AppTextStyles.medium.copyWith(color: AppColors.grey),
              ),
              const SizedBox(height: 24),
              ElevatedButton.icon(
                onPressed: () => ref.read(historyProvider.notifier).refresh(),
                icon: const Icon(Icons.refresh),
                label: const Text('Coba Lagi'),
              ),
            ],
          ),
        ),
      );
    }

    // --- Empty ---
    if (state.transactions.isEmpty) {
      return Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              Icons.receipt_long_outlined,
              size: 64,
              color: AppColors.grey.withValues(alpha: 0.4),
            ),
            const SizedBox(height: 16),
            Text(
              'Belum ada riwayat transaksi',
              style: AppTextStyles.medium.copyWith(color: AppColors.grey),
            ),
          ],
        ),
      );
    }

    // --- Success: group by date and render ---
    final grouped = _groupByDate(state.transactions);

    return RefreshIndicator(
      onRefresh: () => ref.read(historyProvider.notifier).refresh(),
      child: ListView(
        padding: const EdgeInsets.symmetric(horizontal: 20),
        children: [
          for (final entry in grouped.entries) ...[
            TransactionDateHeader(dateString: entry.key),
            for (final tx in entry.value)
              TransactionListItem(
                icon: _iconFor(tx.transactionType),
                title: _titleFor(tx),
                status: tx.status,
                amount: tx.amount.toInt(),
                isCredit: false, // Transfer & Withdrawal are always debits
              ),
          ],
          const SizedBox(height: 40),
        ],
      ),
    );
  }
}
