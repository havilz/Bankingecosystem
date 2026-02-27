import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';
import '../../../../core/ui/widgets/app_search_bar.dart';
import 'widgets/transfer_header.dart';
import 'widgets/favorite_transfer_section.dart';
import 'widgets/recent_transfer_section.dart';

import 'package:skeletonizer/skeletonizer.dart';

class TransferScreen extends StatefulWidget {
  const TransferScreen({super.key});

  @override
  State<TransferScreen> createState() => _TransferScreenState();
}

class _TransferScreenState extends State<TransferScreen> {
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
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
  Widget build(BuildContext context) {
    final bottomPadding = MediaQuery.of(context).viewInsets.bottom;

    return Scaffold(
      backgroundColor: AppColors.primaryLight, // Light Blue / Sky Blue
      resizeToAvoidBottomInset: false, // Prevent background from resizing
      body: Stack(
        fit: StackFit.expand,
        children: [
          // ===== Header Background & Content =====
          const TransferHeader(),

          // ===== Main Body Container =====
          Positioned.fill(
            top: 170, // Adjusted to be below header title
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
                  // Search Bar
                  Padding(
                    padding: const EdgeInsets.fromLTRB(16, 24, 16, 8),
                    child: AppSearchBar(
                      hintText: 'Cari daftar transfer',
                      margin: EdgeInsets.zero,
                    ),
                  ),

                  // Scrollable Content
                  Expanded(
                    child: SingleChildScrollView(
                      padding: EdgeInsets.only(bottom: 100 + bottomPadding),
                      child: Skeletonizer(
                        enabled: _isLoading,
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: const [
                            FavoriteTransferSection(),
                            RecentTransferSection(title: 'Daftar Transfer'),
                            RecentTransferSection(title: 'Rekening Lain'),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),

          // ===== Overlay Button (Floating - explicitly bottom) =====
          Positioned(
            left: 16,
            right: 16,
            bottom: 16, // Tetap di bawah (tidak ikut naik keyboard)
            child: SafeArea(
              child: ElevatedButton(
                onPressed: () => context.push('/transfer/new'),
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.primaryDark,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12),
                  ),
                  padding: const EdgeInsets.symmetric(vertical: 16),
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      'Transfer ke penerima baru',
                      style: AppTextStyles.medium.copyWith(
                        color: AppColors.white,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(width: 8),
                    const Icon(
                      Icons.queue_play_next,
                      color: AppColors.white,
                      size: 20,
                    ),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
