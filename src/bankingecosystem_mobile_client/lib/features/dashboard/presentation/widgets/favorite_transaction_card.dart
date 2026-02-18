import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class FavoriteTransactionCard extends StatelessWidget {
  const FavoriteTransactionCard({super.key});

  // Favorite transaction items
  static final List<Map<String, dynamic>> _favorites = [
    {'icon': Icons.send_outlined, 'label': 'Transfer'},
    {'icon': Icons.qr_code_scanner_outlined, 'label': 'QRIS'},
    {'icon': Icons.phone_android_outlined, 'label': 'Pulsa'},
    {'icon': Icons.bolt_outlined, 'label': 'Listrik'},
    {'icon': Icons.water_drop_outlined, 'label': 'PDAM'},
    {'icon': Icons.wifi_outlined, 'label': 'Internet'},
    {'icon': Icons.credit_card_outlined, 'label': 'E-Wallet'},
    {'icon': Icons.more_horiz, 'label': 'Lainnya'},
  ];

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.symmetric(horizontal: 16),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.white,
        borderRadius: BorderRadius.circular(16),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.1),
            blurRadius: 16,
            spreadRadius: 2,
            offset: const Offset(0, 6),
          ),
          BoxShadow(
            color: AppColors.primary.withValues(alpha: 0.06),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
        border: Border.all(color: AppColors.grey.withValues(alpha: 0.12)),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          // ===== HEADER ROW =====
          _buildHeaderRow(),
          const SizedBox(height: 16),

          // ===== FAVORITE TRANSACTION GRID =====
          _buildFavoriteGrid(),
        ],
      ),
    );
  }

  /// Header: "Transaksi Favorit" (bold, left) | "Atur" + filter (right)
  Widget _buildHeaderRow() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        // Left: "Transaksi Favorit"
        Text(
          'Transaksi Favorit',
          style: AppTextStyles.medium.copyWith(
            fontWeight: FontWeight.bold,
            fontSize: 16,
            color: AppColors.textPrimary,
          ),
        ),

        const Spacer(),

        // Right: "Atur" + filter icon
        GestureDetector(
          onTap: () {
            // TODO: Handle atur action
          },
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(
                'Atur',
                style: AppTextStyles.small.copyWith(
                  fontSize: 13,
                  color: AppColors.primary,
                ),
              ),
              const SizedBox(width: 4),
              const Icon(Icons.tune, size: 18, color: AppColors.primary),
            ],
          ),
        ),
      ],
    );
  }

  /// Grid of favorite transactions (4 columns x 2 rows)
  Widget _buildFavoriteGrid() {
    return GridView.builder(
      shrinkWrap: true,
      physics: const NeverScrollableScrollPhysics(),
      gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
        crossAxisCount: 4,
        mainAxisSpacing: 12,
        crossAxisSpacing: 8,
        childAspectRatio: 0.85,
      ),
      itemCount: _favorites.length,
      itemBuilder: (context, index) {
        final item = _favorites[index];
        return _buildFavoriteItem(
          context,
          item['icon'] as IconData,
          item['label'] as String,
        );
      },
    );
  }

  /// Single favorite item (icon circle + label)
  Widget _buildFavoriteItem(BuildContext context, IconData icon, String label) {
    return GestureDetector(
      onTap: () {
        if (label == 'Transfer') {
          context.push('/transfer');
        } else {
          // TODO: Handle other transaction tap
        }
      },
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 48,
            height: 48,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: AppColors.primaryLight.withValues(alpha: 0.4),
            ),
            child: Icon(icon, size: 24, color: AppColors.primary),
          ),
          const SizedBox(height: 6),
          Text(
            label,
            style: AppTextStyles.small.copyWith(
              fontSize: 11,
              color: AppColors.textPrimary,
            ),
            textAlign: TextAlign.center,
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
          ),
        ],
      ),
    );
  }
}
