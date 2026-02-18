import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

class FavoriteTransferSection extends StatelessWidget {
  const FavoriteTransferSection({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.fromLTRB(16, 16, 16, 8),
          child: Text(
            'Favorit',
            style: AppTextStyles.medium.copyWith(fontWeight: FontWeight.bold),
          ),
        ),
        SizedBox(
          height: 90,
          child: ListView(
            scrollDirection: Axis.horizontal,
            padding: const EdgeInsets.symmetric(horizontal: 16),
            children: [
              // Add Favorite Button
              _buildFavoriteItem(
                icon: Icons.star_border,
                label: 'Tambah\nFavorit',
                isAddButton: true,
              ),
              const SizedBox(width: 16),
              // Mock Favorites
              _buildFavoriteItem(
                label: 'Budi Santoso',
                imageUrl: 'https://i.pravatar.cc/100?img=1',
              ),
              const SizedBox(width: 16),
              _buildFavoriteItem(
                label: 'Siti Aminah',
                imageUrl: 'https://i.pravatar.cc/100?img=5',
              ),
              const SizedBox(width: 16),
              _buildFavoriteItem(
                label: 'Agus P.',
                imageUrl: 'https://i.pravatar.cc/100?img=3',
              ),
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildFavoriteItem({
    IconData? icon,
    required String label,
    String? imageUrl,
    bool isAddButton = false,
  }) {
    return Column(
      children: [
        Container(
          width: 48,
          height: 48,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            color: isAddButton ? AppColors.white : AppColors.lightGrey,
            border: isAddButton
                ? Border.all(color: AppColors.primaryLight, width: 1.5)
                : null,
            image: imageUrl != null
                ? DecorationImage(
                    image: NetworkImage(imageUrl),
                    fit: BoxFit.cover,
                  )
                : null,
          ),
          child: isAddButton
              ? Icon(icon, color: AppColors.primary, size: 24)
              : null,
        ),
        const SizedBox(height: 8),
        Text(
          label,
          textAlign: TextAlign.center,
          style: AppTextStyles.small.copyWith(
            fontSize: 11,
            color: AppColors.textPrimary,
          ),
          maxLines: 2,
          overflow: TextOverflow.ellipsis,
        ),
      ],
    );
  }
}
