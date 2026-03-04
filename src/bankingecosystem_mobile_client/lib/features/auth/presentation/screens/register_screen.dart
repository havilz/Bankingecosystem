import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:go_router/go_router.dart';
import 'package:skeletonizer/skeletonizer.dart';

import '../../../../core/ui/ui.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _cardNumberController = TextEditingController();
  final _emailController = TextEditingController();
  final _passwordController = TextEditingController();
  final _dobController = TextEditingController();

  bool _obscurePassword = true;
  DateTime? _selectedDate;
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    Future.delayed(const Duration(milliseconds: 1500), () {
      if (mounted) setState(() => _isLoading = false);
    });
  }

  @override
  void dispose() {
    _cardNumberController.dispose();
    _emailController.dispose();
    _passwordController.dispose();
    _dobController.dispose();
    super.dispose();
  }

  bool get _isFormValid =>
      _cardNumberController.text.trim().isNotEmpty &&
      _emailController.text.trim().isNotEmpty &&
      _dobController.text.trim().isNotEmpty &&
      _passwordController.text.trim().isNotEmpty;

  Future<void> _pickDate() async {
    final now = DateTime.now();
    final picked = await showDatePicker(
      context: context,
      initialDate: _selectedDate ?? DateTime(now.year - 25),
      firstDate: DateTime(1940),
      lastDate: DateTime(now.year - 17),
      helpText: 'Pilih Tanggal Lahir',
      builder: (context, child) => Theme(
        data: Theme.of(context).copyWith(
          colorScheme: const ColorScheme.light(primary: AppColors.primary),
        ),
        child: child!,
      ),
    );
    if (picked != null) {
      setState(() {
        _selectedDate = picked;
        _dobController.text =
            '${picked.day.toString().padLeft(2, '0')}/'
            '${picked.month.toString().padLeft(2, '0')}/'
            '${picked.year}';
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.white,
      resizeToAvoidBottomInset: false,
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        scrolledUnderElevation: 0,
        surfaceTintColor: Colors.transparent,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: AppColors.textPrimary),
          onPressed: () => context.pop(),
        ),
      ),
      body: SafeArea(
        child: Column(
          children: [
            // ===== Scrollable Form =====
            Expanded(
              child: Skeletonizer(
                enabled: _isLoading,
                child: SingleChildScrollView(
                  padding: const EdgeInsets.fromLTRB(24, 8, 24, 24),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      // Title
                      Text(
                        'Buat Akun',
                        style: AppTextStyles.large.copyWith(
                          fontWeight: FontWeight.bold,
                          color: AppColors.textPrimary,
                          fontSize: 26,
                        ),
                      ),
                      const SizedBox(height: 6),
                      Text(
                        'Hubungkan rekening bank Anda untuk mulai menggunakan mobile banking.',
                        style: AppTextStyles.small.copyWith(
                          color: AppColors.textSecondary,
                          height: 1.5,
                        ),
                      ),
                      const SizedBox(height: 32),

                      // --- Nomor Kartu ---
                      AppTextInput(
                        label: 'Nomor Kartu',
                        hint: 'Masukkan 16 digit nomor kartu',
                        controller: _cardNumberController,
                        keyboardType: TextInputType.number,
                        suffixIcon: const Icon(
                          Icons.credit_card,
                          color: AppColors.grey,
                        ),
                        onChanged: (_) => setState(() {}),
                      ),
                      const SizedBox(height: 20),

                      // --- Email ---
                      AppTextInput(
                        label: 'Email',
                        hint: 'contoh@email.com',
                        controller: _emailController,
                        keyboardType: TextInputType.emailAddress,
                        suffixIcon: const Icon(
                          Icons.email_outlined,
                          color: AppColors.grey,
                        ),
                        onChanged: (_) => setState(() {}),
                      ),
                      const SizedBox(height: 20),

                      // --- Tanggal Lahir ---
                      Text(
                        'Tanggal Lahir',
                        style: AppTextStyles.medium.copyWith(
                          fontWeight: FontWeight.w500,
                        ),
                      ),
                      const SizedBox(height: 8),
                      GestureDetector(
                        onTap: _pickDate,
                        child: AbsorbPointer(
                          child: AppTextInput(
                            label: '',
                            hint: 'DD/MM/YYYY',
                            controller: _dobController,
                            keyboardType: TextInputType.datetime,
                            suffixIcon: const Icon(
                              Icons.calendar_today_outlined,
                              color: AppColors.grey,
                            ),
                          ),
                        ),
                      ),
                      const SizedBox(height: 20),

                      // --- Password ---
                      AppTextInput(
                        label: 'Password',
                        hint: 'Buat password Anda',
                        controller: _passwordController,
                        obscureText: _obscurePassword,
                        suffixIcon: IconButton(
                          icon: Icon(
                            _obscurePassword
                                ? Icons.visibility_off_outlined
                                : Icons.visibility_outlined,
                            color: AppColors.grey,
                          ),
                          onPressed: () => setState(
                            () => _obscurePassword = !_obscurePassword,
                          ),
                        ),
                        onChanged: (_) => setState(() {}),
                      ),
                      const SizedBox(height: 8),
                      Text(
                        'Minimal 8 karakter.',
                        style: AppTextStyles.small.copyWith(
                          color: AppColors.textSecondary,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),

            // ===== Fixed Bottom: Register Button =====
            Container(
              padding: const EdgeInsets.fromLTRB(24, 12, 24, 28),
              decoration: BoxDecoration(
                color: AppColors.white,
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withValues(alpha: 0.05),
                    blurRadius: 10,
                    offset: const Offset(0, -3),
                  ),
                ],
              ),
              child: SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: _isFormValid
                      ? () {
                          // TODO: call register provider
                        }
                      : null,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: AppColors.primary,
                    disabledBackgroundColor: AppColors.grey.withValues(
                      alpha: 0.25,
                    ),
                    padding: const EdgeInsets.symmetric(vertical: 14),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                    elevation: 0,
                  ),
                  child: Text(
                    'Daftar',
                    style: AppTextStyles.button.copyWith(
                      color: _isFormValid ? AppColors.white : AppColors.grey,
                      fontWeight: FontWeight.bold,
                    ),
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
