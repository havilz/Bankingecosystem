# Implementation Plan - Phase 5, Step 1: Secure PIN Communication

## Goal

Implement simulated End-to-End Encryption (E2EE) for PIN transmission between the ATM Client and the Backend. Current PINs are sent as plain text in JSON (over HTTP). We will encrypt the PIN at the "Hardware" level (simulated via C++ NativeLogic) and decrypt it at the Backend before verification.

## Architecture

1.  **Client (ATM)**:
    - User enters PIN -> `PinEntryViewModel`.
    - `AuthService` calls `NativeLogic.Encryption_EncryptPin(rawPin)`.
    - `NativeLogic` performs XOR encryption with a shared secret key.
    - `AuthService` sends **Encrypted PIN** to Backend API.

2.  **Backend (API)**:
    - `AuthController` receives `VerifyPinRequest` with `Pin` field (now encrypted).
    - `AuthService` decrypts the PIN using the same shared secret.
    - `AuthService` verifies the decrypted PIN against the stored BCrypt hash.

## Shared Secret

For simulation purposes, we will use a hardcoded key: `"BANK_ECO_SECURE_2026"`.

## Changes

### 1. C++ NativeLogic (`BankingEcosystem.NativeLogic`)

- **[MODIFY]** `src/encryption_helper.cpp`: Add `Encryption_EncryptPin` function.
  - Algorithm: Simple XOR cipher + Hex encoding.
- **[MODIFY]** `include/encryption_helper.h`: Add function declaration.
- **[MODIFY]** `src/BankingEcosystem.NativeLogic.def` (if exists) or `__declspec(dllexport)`: Ensure function is exported.

### 2. C# Interop (`BankingEcosystem.Interop`)

- **[MODIFY]** `NativeMethods.cs`: Add P/Invoke for `Encryption_EncryptPin`.

### 3. C# AppLayer (`BankingEcosystem.Atm.AppLayer`)

- **[MODIFY]** `Services/AuthService.cs`:
  - Update `VerifyPinAsync` to encrypt PIN before API call.
  - Update `ChangePinAsync` to encrypt `oldPin` and `newPin` before API call.

### 4. C# Backend (`BankingEcosystem.Backend`)

- **[MODIFY]** `Services/AuthService.cs`:
  - Add private `DecryptPin(string encryptedPin)` method (Symmetric XOR logic).
  - Call `DecryptPin` at the start of `VerifyPinAsync` and `ChangePinAsync`.

## Verification

1.  **Manual Test**: Perform a Logical Transaction (Withdrawal).
2.  **Log Check**: Verify in Backend logs (if enabled) or debug output that the received PIN is NOT "123456" but an encrypted string.
3.  **Success**: Transaction succeeds implies Decryption -> Hash Verification worked.
