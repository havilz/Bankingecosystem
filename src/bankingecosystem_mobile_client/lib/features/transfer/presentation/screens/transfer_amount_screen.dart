import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../../core/storage/token_storage.dart';
import '../../../../core/ui/ui.dart';
import '../widgets/transfer_confirmation_sheet.dart';
import '../../../dashboard/presentation/providers/dashboard_provider.dart';

class TransferAmountScreen extends ConsumerStatefulWidget {
  final String targetAccountNumber;
  final String bankName;
  final String recipientName;

  const TransferAmountScreen({
    super.key,
    required this.targetAccountNumber,
    required this.bankName,
    required this.recipientName,
  });

  @override
  ConsumerState<TransferAmountScreen> createState() =>
      _TransferAmountScreenState();
}

class _TransferAmountScreenState extends ConsumerState<TransferAmountScreen> {
  final TextEditingController _amountController = TextEditingController();
  bool _isButtonActive = false;
  String _sourceAccountNumber = '';
  String? _selectedPurpose;

  final List<String> _purposes = [
    'Investasi',
    'Transfer Kekayaan',
    'Pembelian Barang',
    'Pembayaran Jasa',
    'Lainnya',
  ];

  @override
  void initState() {
    super.initState();
    _amountController.addListener(_validateInput);
    _loadSourceAccount();
  }

  Future<void> _loadSourceAccount() async {
    final tokenStorage = ref.read(tokenStorageProvider);
    final accountNum = await tokenStorage.getAccountNumber();
    if (mounted) {
      setState(() {
        _sourceAccountNumber = accountNum ?? '-';
      });
    }
  }

  @override
  void dispose() {
    _amountController.dispose();
    super.dispose();
  }

  void _validateInput() {
    setState(() {
      _isButtonActive =
          _amountController.text.isNotEmpty &&
          _amountController.text != '0' &&
          _selectedPurpose != null; // Require purpose selection
    });
  }

  @override
  Widget build(BuildContext context) {
    final balanceState = ref.watch(dashboardProvider).balance;
    final balanceStr = balanceState != null
        ? _formatCurrency(balanceState.balance)
        : 'Rp -';

    return Scaffold(
      backgroundColor: AppColors.white, // No rounded corners, full white bg
      resizeToAvoidBottomInset:
          false, // Prevent button from rising with keyboard
      appBar: AppBar(
        backgroundColor: AppColors.white,
        scrolledUnderElevation: 0,
        surfaceTintColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: AppColors.textPrimary),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: Stack(
        fit: StackFit.expand,
        children: [
          SingleChildScrollView(
            padding: const EdgeInsets.fromLTRB(
              24,
              16,
              24,
              100,
            ), // padding for bottom button
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                AppRecipientCard(
                  recipientName: widget.recipientName,
                  bankName: widget.bankName,
                  accountNumber: widget.targetAccountNumber,
                ),
                const SizedBox(height: 32),
                AppNominalInput(
                  controller: _amountController,
                  onClear: () {
                    _amountController.clear();
                  },
                ),
                const SizedBox(height: 24),
                AppSourceAccountCard(
                  bankName: 'Banking Ecosystem',
                  accountNumber: _sourceAccountNumber,
                  balanceStr: balanceStr,
                ),
                const SizedBox(height: 24),
                AppSelectionCard(
                  label: 'Metode transfer',
                  value: 'BI Fast',
                  trailingIcon: const Icon(
                    Icons.edit,
                    size: 20,
                    color: AppColors.primary,
                  ),
                ),
                const SizedBox(height: 24),
                AppSelectionCard(
                  label: 'Tujuan transaksi',
                  value: _selectedPurpose ?? 'Pilih tujuan',
                  highlightValue: _selectedPurpose == null,
                  trailingIcon: const Icon(
                    Icons.keyboard_arrow_down,
                    color: AppColors.grey,
                  ),
                  onTap: _showPurposeSelectionSheet,
                ),
              ],
            ),
          ),
          // Footer Button
          Positioned(
            left: 16,
            right: 16,
            bottom: 32,
            child: SafeArea(
              child: AppButton(
                label: 'Lanjut',
                isFullWidth: true,
                onPressed: _isButtonActive
                    ? () {
                        _showConfirmationBottomSheet();
                      }
                    : null,
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _showConfirmationBottomSheet() {
    final amountText = _amountController.text.replaceAll(RegExp(r'\D'), '');
    final amount = int.tryParse(amountText) ?? 0;
    final fee =
        widget.bankName.contains('Internal') ||
            widget.bankName.contains('Ecosystem')
        ? 0
        : 2500; // Mock fee for BI Fast external
    final total = amount + fee;

    TransferConfirmationSheet.show(
      context: context,
      amount: amount,
      fee: fee,
      total: total,
      bankName: widget.bankName,
      targetAccountNumber: widget.targetAccountNumber,
      recipientName: widget.recipientName,
      selectedPurpose: _selectedPurpose,
      formatCurrency: _formatCurrency,
    );
  }

  void _showPurposeSelectionSheet() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      builder: (ctx) => SizedBox(
        height: MediaQuery.of(ctx).size.height * 0.5,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.all(16.0),
              child: Text(
                'Pilih Tujuan Transaksi',
                style: AppTextStyles.large.copyWith(
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            const Divider(height: 1),
            Expanded(
              child: ListView.builder(
                itemCount: _purposes.length,
                itemBuilder: (context, index) {
                  final purpose = _purposes[index];
                  return ListTile(
                    title: Text(purpose),
                    trailing: _selectedPurpose == purpose
                        ? const Icon(Icons.check, color: AppColors.primary)
                        : null,
                    onTap: () {
                      setState(() {
                        _selectedPurpose = purpose;
                        _validateInput();
                      });
                      Navigator.pop(ctx);
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

  String _formatCurrency(num amount) {
    final str = amount.toInt().toString();
    var result = '';
    int count = 0;
    for (int i = str.length - 1; i >= 0; i--) {
      if (count != 0 && count % 3 == 0) {
        result = '.$result';
      }
      result = '${str[i]}$result';
      count++;
    }
    return 'Rp $result';
  }
}
