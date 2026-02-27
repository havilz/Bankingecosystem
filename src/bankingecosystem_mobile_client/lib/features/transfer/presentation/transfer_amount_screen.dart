import 'package:flutter/material.dart';
import '../../../../core/ui/ui.dart';

class TransferAmountScreen extends StatefulWidget {
  const TransferAmountScreen({super.key});

  @override
  State<TransferAmountScreen> createState() => _TransferAmountScreenState();
}

class _TransferAmountScreenState extends State<TransferAmountScreen> {
  final TextEditingController _amountController = TextEditingController();
  bool _isButtonActive = false;

  @override
  void initState() {
    super.initState();
    _amountController.addListener(_validateInput);
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
          _amountController.text != '0'; // Basic validation
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white, // No rounded corners, full white bg
      resizeToAvoidBottomInset:
          false, // Prevent button from rising with keyboard
      appBar: AppBar(
        backgroundColor: AppColors.white,
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
                _buildRecipientDetails(),
                const SizedBox(height: 32),
                _buildNominalCard(),
                const SizedBox(height: 24),
                _buildSourceAccountSection(),
                const SizedBox(height: 24),
                _buildTransferMethodCard(),
                const SizedBox(height: 24),
                _buildTransactionPurposeCard(),
              ],
            ),
          ),
          // Footer Button
          Positioned(
            left: 16,
            right: 16,
            bottom: 32,
            child: SafeArea(
              child: ElevatedButton(
                onPressed: _isButtonActive
                    ? () {
                        // TODO: Proceed to confirmation
                      }
                    : null,
                style: ElevatedButton.styleFrom(
                  backgroundColor: _isButtonActive
                      ? AppColors.primary
                      : AppColors.grey,
                  disabledBackgroundColor: AppColors.grey,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                  padding: const EdgeInsets.symmetric(vertical: 16),
                ),
                child: Text(
                  'Lanjut',
                  style: AppTextStyles.button.copyWith(
                    color: _isButtonActive
                        ? AppColors.white
                        : AppColors.white.withValues(alpha: 0.8),
                  ),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildRecipientDetails() {
    return Column(
      children: [
        CircleAvatar(
          radius: 32,
          backgroundColor: AppColors.lightGrey,
          child: Text(
            'BS', // Mock initials
            style: AppTextStyles.h2.copyWith(
              color: AppColors.grey,
              fontWeight: FontWeight.bold,
            ),
          ),
        ),
        const SizedBox(height: 16),
        // Added back the Text widget wrapper that was accidentally removed
        Text(
          'Budi Santoso', // Mock name
          style: AppTextStyles.large.copyWith(fontWeight: FontWeight.bold),
          textAlign: TextAlign.center,
        ),
        const SizedBox(height: 4),
        Text(
          'BCA - 1234567890', // Mock bank details
          style: AppTextStyles.medium.copyWith(color: AppColors.grey),
          textAlign: TextAlign.center,
        ),
      ],
    );
  }

  Widget _buildNominalCard() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: AppColors.lightGrey, width: 1),
        boxShadow: [
          BoxShadow(
            color: AppColors.grey.withValues(alpha: 0.1),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Nominal',
            style: AppTextStyles.small.copyWith(color: AppColors.grey),
          ),
          const SizedBox(height: 8),
          Row(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Text(
                'Rp ',
                style: AppTextStyles.large.copyWith(
                  fontWeight: FontWeight.bold,
                  color: AppColors.textPrimary,
                ),
              ),
              Expanded(
                child: TextField(
                  controller: _amountController,
                  keyboardType: TextInputType.number,
                  style: AppTextStyles.large.copyWith(
                    fontWeight: FontWeight.bold,
                    color: AppColors.textPrimary,
                  ),
                  decoration: const InputDecoration(
                    hintText: '0',
                    hintStyle: TextStyle(color: AppColors.grey),
                    border: InputBorder.none,
                    isDense: true,
                    contentPadding: EdgeInsets.zero,
                  ),
                ),
              ),
              if (_amountController.text.isNotEmpty)
                GestureDetector(
                  onTap: () {
                    _amountController.clear();
                  },
                  child: Container(
                    padding: const EdgeInsets.all(4),
                    decoration: const BoxDecoration(
                      color: AppColors.lightGrey,
                      shape: BoxShape.circle,
                    ),
                    child: const Icon(
                      Icons.close,
                      size: 16,
                      color: AppColors.grey,
                    ),
                  ),
                ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildSourceAccountSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'Rekening Sumber',
          style: AppTextStyles.small.copyWith(color: AppColors.grey),
        ),
        const SizedBox(height: 8),
        Container(
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            color: AppColors.white,
            borderRadius: BorderRadius.circular(12),
            border: Border.all(color: AppColors.lightGrey, width: 1),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Mandiri - 0987654321', // Mock user account
                    style: AppTextStyles.medium.copyWith(
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Saldo: Rp 10.500.000', // Mock balance
                    style: AppTextStyles.small.copyWith(color: AppColors.grey),
                  ),
                ],
              ),
              const Icon(Icons.keyboard_arrow_down, color: AppColors.grey),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildTransferMethodCard() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: AppColors.lightGrey, width: 1),
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Metode transfer',
                style: AppTextStyles.small.copyWith(color: AppColors.grey),
              ),
              const SizedBox(height: 4),
              Text(
                'BI Fast', // Default
                style: AppTextStyles.medium.copyWith(
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
          const Icon(Icons.edit, size: 20, color: AppColors.primary),
        ],
      ),
    );
  }

  Widget _buildTransactionPurposeCard() {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: AppColors.lightGrey, width: 1),
      ),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'Tujuan transaksi',
                style: AppTextStyles.small.copyWith(color: AppColors.grey),
              ),
              const SizedBox(height: 4),
              Text(
                'Pilih tujuan',
                style: AppTextStyles.medium.copyWith(
                  fontWeight: FontWeight.bold,
                  color: AppColors.primary,
                ),
              ),
            ],
          ),
          const Icon(Icons.keyboard_arrow_down, color: AppColors.grey),
        ],
      ),
    );
  }
}
