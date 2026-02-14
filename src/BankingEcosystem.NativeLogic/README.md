# BankingEcosystem.NativeLogic — Documentation

> **Project Type:** C++ Dynamic Link Library (DLL)  
> **Platform:** Windows (x64)  
> **Role:** Core Logic, Security & Validation  
> **Dependencies:** Standard C++ Runtime

---

## Overview

This project contains high-performance core logic or security components that are intentionally separated from the application layer (C#). In a real-world scenario, this module could be a _Hardware Security Module_ (HSM) or a proprietary library from an ATM vendor.

This module is exported as a **DLL** (`BankingEcosystem.NativeLogic.dll`) and accessed via **P/Invoke**.

## Project Structure

```
BankingEcosystem.NativeLogic/
├── include/
│   └── NativeLogic.h            # Header file (dllexport declarations)
├── src/
│   ├── Encryption.cpp           # PIN Encryption logic
│   └── Validator.cpp            # Card Validation logic (Luhn Algo)
├── bin/                         # Output DLL
└── BankingEcosystem.NativeLogic.vcxproj
```

---

## Exposed Functions (API)

### 1. Encryption

- `Encryption_EncryptPin(const char* pin)`:
  - Accepts PIN (plaintext).
  - Performs XOR operation with a static Key (Simulation).
  - Returns Hex String of the encryption result.
  - **Purpose:** To ensure the PIN is never sent as static Plain Text over the network.

### 2. Validation

- `Validator_ValidateCardNumber(const char* cardNumber)`:
  - Implements **Luhn Algorithm** (Modulus 10).
  - Standard international credit/debit card validation.
  - Returns `true` if checksum is valid.

---

## Security Context

Although this is a simulation, the patterns used mimic best practices:

1. **Pin Block:** In the real world, the PIN is encrypted directly on the keypad (hardware) into a PIN Block (ISO 9564). This C++ code simulates such "hardware encryption" before data touches the OS/Application Layer.
2. **Native Execution:** Sensitive logic running in unmanaged code is harder to reverse engineer or hook compared to managed code (.NET).

## Build Instructions

Same as the Hardware project:

1. Use Visual Studio 2022+.
2. Build as **x64** (Mandatory, since .NET Core runs in 64-bit).
3. Ensure the output DLL is in the same directory as the C# executable at runtime.
