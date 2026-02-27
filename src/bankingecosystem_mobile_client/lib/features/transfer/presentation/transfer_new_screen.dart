import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';
import 'widgets/transfer_new_header.dart';
import 'widgets/transfer_success_bottom_sheet.dart';

import 'package:skeletonizer/skeletonizer.dart';

class TransferNewScreen extends StatefulWidget {
  const TransferNewScreen({super.key});

  @override
  State<TransferNewScreen> createState() => _TransferNewScreenState();
}

class _TransferNewScreenState extends State<TransferNewScreen> {
  final TextEditingController _accountController = TextEditingController();
  bool _isButtonActive = false;
  bool _isLoading = true;
  bool _isValidating = false; // Add validation loading state

  // Mock selected bank for now
  // ignore: prefer_final_fields
  String _selectedBank = "BCA";

  @override
  void initState() {
    super.initState();
    _accountController.addListener(_validateInput);
    // Simulate loading
    Future.delayed(const Duration(milliseconds: 1500), () {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    });
  }

  @override
  void dispose() {
    _accountController.dispose();
    super.dispose();
  }

  void _validateInput() {
    setState(() {
      _isButtonActive = _accountController.text.isNotEmpty;
    });
  }

  @override
  Widget build(BuildContext context) {
    final bottomPadding = MediaQuery.of(context).viewInsets.bottom;

    return Scaffold(
      backgroundColor: AppColors.primaryLight,
      resizeToAvoidBottomInset: false, // Prevent background from resizing
      body: Stack(
        fit: StackFit.expand,
        children: [
          // Header
          const TransferNewHeader(),

          // Body Container
          Positioned.fill(
            top: 170,
            child: Container(
              decoration: const BoxDecoration(
                color: AppColors.white,
                borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(24),
                  topRight: Radius.circular(24),
                ),
              ),
              child: Column(
                children: [
                  Expanded(
                    child: SingleChildScrollView(
                      padding: EdgeInsets.fromLTRB(
                        24,
                        24,
                        24,
                        100 + bottomPadding,
                      ),
                      child: Skeletonizer(
                        enabled: _isLoading,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            // 1. Bank Selector Button
                            _buildBankSelector(),

                            const SizedBox(height: 24),

                            // 2. Account Number Input
                            Text(
                              'Nomor Rekening',
                              style: AppTextStyles.medium.copyWith(
                                color: AppColors.grey,
                                fontSize: 14,
                              ),
                            ),
                            const SizedBox(height: 8),
                            // Using a standard TextField for now, or AppTextInput if adaptable
                            // User requested "form untuk input nomor rekening"
                            TextField(
                              controller: _accountController,
                              keyboardType: TextInputType.number,
                              style: AppTextStyles.medium.copyWith(
                                fontWeight: FontWeight.bold,
                              ),
                              decoration: InputDecoration(
                                hintText: 'Masukkan nomor rekening',
                                hintStyle: AppTextStyles.medium.copyWith(
                                  color: AppColors.grey,
                                ),
                                filled: true,
                                fillColor: AppColors
                                    .lightGrey, // Assuming variable exists or grey[50]
                                border: OutlineInputBorder(
                                  borderRadius: BorderRadius.circular(12),
                                  borderSide: BorderSide.none,
                                ),
                                contentPadding: const EdgeInsets.symmetric(
                                  horizontal: 16,
                                  vertical: 16,
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),

          // Footer Button "Lanjut"
          Positioned(
            left: 16,
            right: 16,
            bottom: 32,
            child: SafeArea(
              child: ElevatedButton(
                onPressed: _isButtonActive && !_isValidating
                    ? _handleValidation
                    : null,
                style: ElevatedButton.styleFrom(
                  backgroundColor: _isButtonActive
                      ? AppColors.primary
                      : AppColors.grey,
                  disabledBackgroundColor:
                      AppColors.grey, // Background when disabled
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                  padding: const EdgeInsets.symmetric(vertical: 16),
                ),
                child: _isValidating
                    ? const SizedBox(
                        height: 20,
                        width: 20,
                        child: CircularProgressIndicator(
                          color: AppColors.white,
                          strokeWidth: 2,
                        ),
                      )
                    : Text(
                        'Lanjut',
                        style: AppTextStyles.button.copyWith(
                          color: _isButtonActive
                              ? AppColors.white
                              : AppColors.grey, // Dark grey text when inactive
                        ),
                      ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  void _handleValidation() async {
    setState(() {
      _isValidating = true;
    });

    // Simulate network delay
    await Future.delayed(const Duration(milliseconds: 1500));

    if (!mounted) return;

    setState(() {
      _isValidating = false;
    });

    final accountNumber = _accountController.text.trim();

    // Mock validation logic
    if (accountNumber == '1234567890') {
      _showSuccessBottomSheet();
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
            'Nomor rekening tidak ditemukan',
            style: AppTextStyles.small.copyWith(color: AppColors.white),
          ),
          backgroundColor: Colors.red,
          behavior: SnackBarBehavior.floating,
        ),
      );
    }
  }

  void _showSuccessBottomSheet() {
    TransferSuccessBottomSheet.show(
      context: context,
      accountName: 'Budi Santoso',
      bankName: _selectedBank,
      accountNumber: _accountController.text,
      onContinue: () {
        Navigator.pop(context); // Close bottom sheet
        context.push('/transfer/amount');
      },
    );
  }

  Widget _buildBankSelector() {
    return GestureDetector(
      onTap: () {
        // TODO: Handle bank selection navigation
      },
      child: Container(
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          color: AppColors.lightGrey,
          borderRadius: BorderRadius.circular(12),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Nama Bank',
                  style: AppTextStyles.small.copyWith(
                    color: AppColors.grey,
                    fontSize: 12,
                  ),
                ),
                const SizedBox(height: 4),
                Text(
                  _selectedBank,
                  style: AppTextStyles.medium.copyWith(
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ],
            ),
            const Icon(Icons.keyboard_arrow_down, color: AppColors.textPrimary),
          ],
        ),
      ),
    );
  }
}
