import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:skeletonizer/skeletonizer.dart';

import '../../../../core/ui/ui.dart';
import '../widgets/privacy_policy_content.dart';

class PrivacyPolicyScreen extends StatefulWidget {
  const PrivacyPolicyScreen({super.key});

  @override
  State<PrivacyPolicyScreen> createState() => _PrivacyPolicyScreenState();
}

class _PrivacyPolicyScreenState extends State<PrivacyPolicyScreen> {
  bool _isAgreed = false;
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    Future.delayed(const Duration(milliseconds: 1500), () {
      if (mounted) setState(() => _isLoading = false);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        scrolledUnderElevation: 0,
        surfaceTintColor: Colors.transparent,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: AppColors.textPrimary),
          onPressed: () => context.pop(),
        ),
        title: Text(
          'Kebijakan Privasi',
          style: AppTextStyles.medium.copyWith(
            fontWeight: FontWeight.bold,
            color: AppColors.textPrimary,
          ),
        ),
        centerTitle: false,
      ),
      body: Column(
        children: [
          // ===== Scrollable Policy Content =====
          Expanded(
            child: Skeletonizer(
              enabled: _isLoading,
              child: const SingleChildScrollView(
                padding: EdgeInsets.fromLTRB(20, 8, 20, 24),
                child: PrivacyPolicyContent(),
              ),
            ),
          ),

          // ===== Fixed Bottom: Checkbox + Button =====
          Container(
            padding: const EdgeInsets.fromLTRB(16, 12, 16, 28),
            decoration: BoxDecoration(
              color: AppColors.white,
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withOpacity(0.06),
                  blurRadius: 12,
                  offset: const Offset(0, -4),
                ),
              ],
            ),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                // Checkbox
                GestureDetector(
                  onTap: () => setState(() => _isAgreed = !_isAgreed),
                  child: Row(
                    children: [
                      Checkbox(
                        value: _isAgreed,
                        onChanged: (val) =>
                            setState(() => _isAgreed = val ?? false),
                        activeColor: AppColors.primary,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(4),
                        ),
                      ),
                      Expanded(
                        child: Text(
                          'Saya telah membaca dan menyetujui Kebijakan Privasi ini.',
                          style: AppTextStyles.small.copyWith(
                            color: AppColors.textPrimary,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 12),

                // Lanjut Button
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton(
                    onPressed: _isAgreed
                        ? () {
                            // TODO: Navigate to Register Form
                            context.push('/register');
                          }
                        : null,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: AppColors.primary,
                      disabledBackgroundColor: AppColors.grey.withOpacity(0.3),
                      padding: const EdgeInsets.symmetric(vertical: 14),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      elevation: 0,
                    ),
                    child: Text(
                      'Lanjut',
                      style: AppTextStyles.button.copyWith(
                        color: _isAgreed ? AppColors.white : AppColors.grey,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
