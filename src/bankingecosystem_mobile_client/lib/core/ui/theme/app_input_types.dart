import 'package:flutter/material.dart';

/// Centralized input type configurations for consistent keyboard behavior
/// across the app. Use these presets for email, password, number, and PIN fields.
class AppInputTypes {
  AppInputTypes._();

  // ===== Email =====
  static const TextInputType email = TextInputType.emailAddress;

  // ===== Password =====
  static const TextInputType password = TextInputType.visiblePassword;

  // ===== Number (general numeric) =====
  static const TextInputType number = TextInputType.number;

  // ===== PIN (numeric, no suggestions) =====
  static const TextInputType pin = TextInputType.number;

  // ===== Phone =====
  static const TextInputType phone = TextInputType.phone;

  // ===== Text (default) =====
  static const TextInputType text = TextInputType.text;

  // ===== Multiline =====
  static const TextInputType multiline = TextInputType.multiline;

  // ===== Card Number (numeric, formatted) =====
  static const TextInputType cardNumber = TextInputType.number;
}
