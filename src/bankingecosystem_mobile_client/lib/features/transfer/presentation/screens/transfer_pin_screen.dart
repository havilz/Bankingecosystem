import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/storage/token_storage.dart';
import '../../../../core/ui/ui.dart';
import '../../../auth/presentation/providers/auth_provider.dart';
import '../providers/transfer_provider.dart';

class TransferPinScreen extends ConsumerStatefulWidget {
  final Map<String, dynamic> transactionDetails;

  const TransferPinScreen({super.key, this.transactionDetails = const {}});

  @override
  ConsumerState<TransferPinScreen> createState() => _TransferPinScreenState();
}

class _TransferPinScreenState extends ConsumerState<TransferPinScreen> {
  String _pin = '';
  final int _maxPinLength = 6;
  bool _isLoading = false;

  void _onNumberPressed(int number) {
    if (_pin.length < _maxPinLength) {
      setState(() {
        _pin += number.toString();
      });

      if (_pin.length == _maxPinLength) {
        _submitPin();
      }
    }
  }

  void _onDeletePressed() {
    if (_pin.isNotEmpty) {
      setState(() {
        _pin = _pin.substring(0, _pin.length - 1);
      });
    }
  }

  Future<void> _submitPin() async {
    setState(() {
      _isLoading = true;
    });

    final accountId = await ref.read(tokenStorageProvider).getAccountId();
    if (accountId == null) {
      if (mounted) {
        setState(() => _isLoading = false);
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Sesi Anda tidak valid. Silakan login kembali.'),
          ),
        );
      }
      return;
    }

    final authRepo = ref.read(authRepositoryProvider);
    final verifyResult = await authRepo.verifyMbankingPin(accountId, _pin);

    if (!verifyResult.success) {
      if (mounted) {
        setState(() {
          _pin = '';
          _isLoading = false;
        });
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(verifyResult.message),
            backgroundColor: Colors.red,
          ),
        );
      }
      return;
    }

    // Call actual transfer API
    await ref
        .read(transferProvider.notifier)
        .doTransfer(
          targetAccountNumber:
              widget.transactionDetails['targetAccountNumber'] as String,
          amount: (widget.transactionDetails['amount'] as num).toDouble(),
          description: "Transfer mBanking",
        );

    if (!mounted) return;

    final state = ref.read(transferProvider);
    if (state is TransferSuccess) {
      final updatedDetails = Map<String, dynamic>.from(
        widget.transactionDetails,
      );
      updatedDetails['referenceNumber'] = state.result.referenceNumber;
      updatedDetails['transactionTime'] = state.result.createdAt
          .toIso8601String();

      // Ensure loading state is turned off in case user presses back from receipt
      setState(() => _isLoading = false);

      context.go('/transfer/receipt', extra: updatedDetails);
    } else if (state is TransferError) {
      setState(() {
        _pin = '';
        _isLoading = false;
      });
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(state.message), backgroundColor: Colors.red),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.lightGrey,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: AppColors.textPrimary),
          onPressed: () => context.pop(),
        ),
      ),
      body: SafeArea(
        child: Column(
          children: [
            const Spacer(flex: 1),
            Text(
              'Masukkan PIN',
              style: AppTextStyles.large.copyWith(
                fontWeight: FontWeight.bold,
                fontSize: 24,
              ),
            ),
            const SizedBox(height: 12),
            Text(
              'Masukkan 6 digit PIN mBanking Anda',
              style: AppTextStyles.medium.copyWith(color: AppColors.grey),
            ),
            const SizedBox(height: 32),

            if (_isLoading)
              const SizedBox(
                height: 24,
                width: 24,
                child: CircularProgressIndicator(
                  strokeWidth: 2,
                  color: AppColors.primary,
                ),
              )
            else
              AppPinIndicator(length: _pin.length, maxLength: _maxPinLength),

            const Spacer(flex: 2),

            Container(
              padding: const EdgeInsets.symmetric(vertical: 32, horizontal: 16),
              decoration: const BoxDecoration(
                color: AppColors.white,
                borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(32),
                  topRight: Radius.circular(32),
                ),
              ),
              child: Opacity(
                opacity: _isLoading ? 0.5 : 1.0,
                child: IgnorePointer(
                  ignoring: _isLoading,
                  child: AppPinPad(
                    onNumberPressed: _onNumberPressed,
                    onDeletePressed: _onDeletePressed,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
