import 'package:flutter/material.dart';

import '../../../../core/ui/theme/app_colors.dart';
import '../../../../core/ui/theme/app_text_styles.dart';

/// Account card widget — Livin' Mandiri inspired.
/// Contains a header row with "Rekening" on the left,
/// "Saldo" (eye icon) and "Atur" (filter icon) on the right.
/// Body content will be added incrementally below.
class AccountCard extends StatefulWidget {
  const AccountCard({super.key});

  @override
  State<AccountCard> createState() => _AccountCardState();
}

class _AccountCardState extends State<AccountCard> {
  bool _isBalanceVisible = true;
  int _activeCategory = 0;
  final PageController _pageController = PageController();

  // Category data
  static final List<Map<String, dynamic>> _categories = [
    {'icon': Icons.account_balance_wallet_outlined, 'label': 'Tabungan'},
    {'icon': Icons.savings_outlined, 'label': 'Deposito'},
    {'icon': Icons.credit_card_outlined, 'label': 'Kartu Kredit'},
    {'icon': Icons.request_quote_outlined, 'label': 'Pinjaman'},
    {'icon': Icons.trending_up_outlined, 'label': 'Investasi'},
  ];

  @override
  void dispose() {
    _pageController.dispose();
    super.dispose();
  }

  void _onCategoryTapped(int index) {
    setState(() {
      _activeCategory = index;
    });
    _pageController.animateToPage(
      index,
      duration: const Duration(milliseconds: 300),
      curve: Curves.easeInOut,
    );
  }

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
          const SizedBox(height: 20),

          // ===== CATEGORY TABS + DIVIDER =====
          _buildCategorySection(),
          const SizedBox(height: 16),

          // ===== CATEGORY CONTENT (PageView) =====
          SizedBox(
            height: 90,
            child: PageView(
              controller: _pageController,
              padEnds: false,
              clipBehavior: Clip.hardEdge,
              onPageChanged: (index) {
                setState(() {
                  _activeCategory = index;
                });
              },
              children: [
                _buildTabunganContent(),
                _buildDepositoContent(),
                _buildKartuKreditContent(),
                _buildPinjamanContent(),
                _buildInvestasiContent(),
              ],
            ),
          ),
        ],
      ),
    );
  }

  /// Header: "Rekening" (bold, left) | "Saldo" + eye, "Atur" + filter (right)
  Widget _buildHeaderRow() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        // Left: "Rekening"
        Text(
          'Rekening',
          style: AppTextStyles.medium.copyWith(
            fontWeight: FontWeight.bold,
            fontSize: 16,
            color: AppColors.textPrimary,
          ),
        ),

        const Spacer(),

        // Right: "Saldo" + eye icon
        GestureDetector(
          onTap: () {
            setState(() {
              _isBalanceVisible = !_isBalanceVisible;
            });
          },
          child: Row(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(
                'Saldo',
                style: AppTextStyles.small.copyWith(
                  fontSize: 13,
                  color: AppColors.primary,
                ),
              ),
              const SizedBox(width: 4),
              Icon(
                _isBalanceVisible
                    ? Icons.visibility_outlined
                    : Icons.visibility_off_outlined,
                size: 18,
                color: AppColors.primary,
              ),
            ],
          ),
        ),

        const SizedBox(width: 16),

        // Right: "Atur" + filter icon
        GestureDetector(
          onTap: () {
            // TODO: Handle filter/atur action
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

  /// Category section: icon tabs + divider with active yellow indicator
  Widget _buildCategorySection() {
    return Column(
      children: [
        // Category icon tabs
        Row(
          children: List.generate(_categories.length, (index) {
            return _buildCategoryTab(
              index,
              _categories[index]['icon'] as IconData,
              _categories[index]['label'] as String,
            );
          }),
        ),
        const SizedBox(height: 10),

        // ===== DIVIDER with yellow active indicator =====
        Stack(
          children: [
            // Grey divider (full width)
            Container(
              height: 2.5,
              decoration: BoxDecoration(
                color: AppColors.grey.withValues(alpha: 0.2),
                borderRadius: BorderRadius.circular(2),
              ),
            ),
            // Yellow indicator (positioned under active category)
            Row(
              children: List.generate(_categories.length, (index) {
                return Expanded(
                  child: index == _activeCategory
                      ? Center(
                          child: _buildActiveIndicator(
                            _categories[index]['label'] as String,
                          ),
                        )
                      : const SizedBox(height: 2.5),
                );
              }),
            ),
          ],
        ),
      ],
    );
  }

  /// Yellow indicator with width matching the category text
  Widget _buildActiveIndicator(String label) {
    final textPainter = TextPainter(
      text: TextSpan(
        text: label,
        style: AppTextStyles.small.copyWith(fontSize: 11),
      ),
      textDirection: TextDirection.ltr,
    )..layout();

    return Container(
      width: textPainter.width + 4,
      height: 2.5,
      decoration: BoxDecoration(
        color: AppColors.accent,
        borderRadius: BorderRadius.circular(2),
      ),
    );
  }

  /// Single category tab (icon + text, tappable)
  Widget _buildCategoryTab(int index, IconData icon, String label) {
    final isActive = index == _activeCategory;
    return Expanded(
      child: GestureDetector(
        onTap: () => _onCategoryTapped(index),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 44,
              height: 44,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: isActive
                    ? AppColors.primaryLight.withValues(alpha: 0.6)
                    : AppColors.primaryLight.withValues(alpha: 0.3),
              ),
              child: Icon(
                icon,
                size: 22,
                color: isActive ? AppColors.primary : AppColors.grey,
              ),
            ),
            const SizedBox(height: 6),
            Text(
              label,
              style: AppTextStyles.small.copyWith(
                fontSize: 11,
                color: isActive ? AppColors.textPrimary : AppColors.grey,
                fontWeight: isActive ? FontWeight.w600 : FontWeight.normal,
              ),
              textAlign: TextAlign.center,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
            ),
          ],
        ),
      ),
    );
  }

  // ================================================================
  // ===== CATEGORY CONTENT BUILDERS =====
  // ================================================================

  /// Tabungan: Account name + balance nominal
  Widget _buildTabunganContent() {
    return _buildContentCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Text(
            'Tabungan NOW IDR',
            style: AppTextStyles.medium.copyWith(
              fontSize: 14,
              fontWeight: FontWeight.w600,
              color: AppColors.textPrimary,
            ),
          ),
          const SizedBox(height: 8),
          Text(
            _isBalanceVisible ? 'Rp 1.250.000' : '••••••••',
            style: AppTextStyles.large.copyWith(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: AppColors.primary,
            ),
          ),
        ],
      ),
    );
  }

  /// Deposito: Title with arrow + short description
  Widget _buildDepositoContent() {
    return _buildContentCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Row(
            children: [
              Text(
                'Deposito',
                style: AppTextStyles.medium.copyWith(
                  fontSize: 14,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textPrimary,
                ),
              ),
              const SizedBox(width: 4),
              const Icon(
                Icons.arrow_forward,
                size: 16,
                color: AppColors.primary,
              ),
            ],
          ),
          const SizedBox(height: 6),
          Text(
            'Simpanan berjangka dengan bunga kompetitif',
            style: AppTextStyles.small.copyWith(
              fontSize: 12,
              color: AppColors.grey,
            ),
          ),
        ],
      ),
    );
  }

  /// Kartu Kredit: Bold message — not available
  Widget _buildKartuKreditContent() {
    return _buildContentCard(
      child: Center(
        child: Text(
          'Belum ada kartu kredit',
          style: AppTextStyles.medium.copyWith(
            fontSize: 14,
            fontWeight: FontWeight.bold,
            color: AppColors.textPrimary,
          ),
        ),
      ),
    );
  }

  /// Pinjaman: KPR with arrow + short description
  Widget _buildPinjamanContent() {
    return _buildContentCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Row(
            children: [
              Text(
                'KPR',
                style: AppTextStyles.medium.copyWith(
                  fontSize: 14,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textPrimary,
                ),
              ),
              const SizedBox(width: 4),
              const Icon(
                Icons.arrow_forward,
                size: 16,
                color: AppColors.primary,
              ),
            ],
          ),
          const SizedBox(height: 6),
          Text(
            'Wujudkan rumah impian dengan cicilan ringan',
            style: AppTextStyles.small.copyWith(
              fontSize: 12,
              color: AppColors.grey,
            ),
          ),
        ],
      ),
    );
  }

  /// Investasi: Reksa Dana with arrow + short description
  Widget _buildInvestasiContent() {
    return _buildContentCard(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Row(
            children: [
              Text(
                'Beli Reksa Dana',
                style: AppTextStyles.medium.copyWith(
                  fontSize: 14,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textPrimary,
                ),
              ),
              const SizedBox(width: 4),
              const Icon(
                Icons.arrow_forward,
                size: 16,
                color: AppColors.primary,
              ),
            ],
          ),
          const SizedBox(height: 6),
          Text(
            'Mulai investasi dari Rp 10.000',
            style: AppTextStyles.small.copyWith(
              fontSize: 12,
              color: AppColors.grey,
            ),
          ),
        ],
      ),
    );
  }

  /// Reusable inner content card wrapper
  Widget _buildContentCard({required Widget child}) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 4),
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
        decoration: BoxDecoration(
          color: AppColors.lightGrey,
          borderRadius: BorderRadius.circular(12),
        ),
        child: child,
      ),
    );
  }
}
