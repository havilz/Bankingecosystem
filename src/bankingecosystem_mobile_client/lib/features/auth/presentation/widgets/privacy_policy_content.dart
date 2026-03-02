import 'package:flutter/material.dart';

import '../../../../core/ui/ui.dart';

class PrivacyPolicyContent extends StatelessWidget {
  const PrivacyPolicyContent({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        _sectionTitle('1. Pendahuluan'),
        _body(
          'Selamat datang di layanan Mobile Banking Banking Ecosystem. '
          'Kebijakan Privasi ini menjelaskan bagaimana kami mengumpulkan, '
          'menggunakan, mengungkapkan, dan melindungi informasi pribadi Anda '
          'saat menggunakan aplikasi mobile banking kami. Dengan menggunakan '
          'layanan ini, Anda menyetujui praktik yang dijelaskan dalam kebijakan ini.',
        ),
        _sectionTitle('2. Informasi yang Kami Kumpulkan'),
        _body(
          'Kami mengumpulkan berbagai jenis informasi sehubungan dengan '
          'layanan yang kami berikan, meliputi:\n\n'
          '• Informasi Identitas: nama lengkap, tanggal lahir, nomor identitas (NIK), '
          'dan informasi demografis lainnya.\n\n'
          '• Informasi Rekening: nomor rekening, saldo, riwayat transaksi, '
          'dan informasi kartu yang terhubung.\n\n'
          '• Informasi Perangkat: model perangkat, sistem operasi, '
          'pengidentifikasi perangkat unik, dan informasi jaringan.\n\n'
          '• Informasi Penggunaan: cara Anda berinteraksi dengan aplikasi, '
          'fitur yang Anda gunakan, dan waktu akses.',
        ),
        _sectionTitle('3. Penggunaan Informasi'),
        _body(
          'Informasi yang kami kumpulkan digunakan untuk:\n\n'
          '• Menyediakan, memelihara, dan meningkatkan layanan mobile banking.\n\n'
          '• Memproses transaksi keuangan Anda secara aman.\n\n'
          '• Memverifikasi identitas Anda dan mencegah penipuan.\n\n'
          '• Mengirimkan pemberitahuan terkait akun dan transaksi.\n\n'
          '• Mematuhi kewajiban hukum dan regulasi perbankan yang berlaku.',
        ),
        _sectionTitle('4. Keamanan Data'),
        _body(
          'Keamanan informasi Anda adalah prioritas utama kami. Kami menerapkan '
          'langkah-langkah keamanan teknis dan organisasi yang sesuai, termasuk:\n\n'
          '• Enkripsi end-to-end untuk semua transmisi data sensitif.\n\n'
          '• Penyimpanan PIN dan kredensial menggunakan enkripsi tingkat tinggi.\n\n'
          '• Sistem deteksi penipuan dan pemantauan aktivitas mencurigakan.\n\n'
          '• Pembatasan akses data hanya kepada personel yang berwenang.\n\n'
          'Meskipun demikian, tidak ada metode transmisi atau penyimpanan '
          'elektronik yang 100% aman. Kami mendorong Anda untuk menjaga '
          'kerahasiaan PIN dan kredensial Anda.',
        ),
        _sectionTitle('5. Pengungkapan kepada Pihak Ketiga'),
        _body(
          'Kami tidak menjual, memperdagangkan, atau mengalihkan informasi '
          'pribadi Anda kepada pihak ketiga tanpa persetujuan Anda, kecuali:\n\n'
          '• Mitra tepercaya yang membantu dalam operasional layanan kami, '
          'dengan tunduk pada perjanjian kerahasiaan yang ketat.\n\n'
          '• Otoritas regulasi dan hukum apabila diwajibkan oleh undang-undang.\n\n'
          '• Proses pemindahan bisnis seperti merger atau akuisisi.',
        ),
        _sectionTitle('6. Hak Anda'),
        _body(
          'Anda memiliki hak untuk:\n\n'
          '• Mengakses informasi pribadi yang kami simpan tentang Anda.\n\n'
          '• Meminta koreksi data yang tidak akurat atau tidak lengkap.\n\n'
          '• Meminta penghapusan data dalam kondisi tertentu.\n\n'
          '• Menarik persetujuan Anda kapan saja, dengan memahami bahwa '
          'hal ini dapat memengaruhi kemampuan kami untuk menyediakan layanan.',
        ),
        _sectionTitle('7. Perubahan Kebijakan'),
        _body(
          'Kami dapat memperbarui Kebijakan Privasi ini dari waktu ke waktu. '
          'Setiap perubahan akan diberitahukan melalui aplikasi atau email '
          'yang terdaftar. Penggunaan layanan yang berkelanjutan setelah '
          'pemberitahuan dianggap sebagai persetujuan Anda terhadap perubahan tersebut.\n\n'
          'Terakhir diperbarui: Maret 2026.',
        ),
      ],
    );
  }

  Widget _sectionTitle(String text) => Padding(
    padding: const EdgeInsets.only(top: 20, bottom: 8),
    child: Text(
      text,
      style: AppTextStyles.medium.copyWith(
        fontWeight: FontWeight.bold,
        color: AppColors.textPrimary,
      ),
    ),
  );

  Widget _body(String text) => Text(
    text,
    style: AppTextStyles.small.copyWith(
      color: AppColors.textSecondary,
      height: 1.6,
    ),
  );
}
