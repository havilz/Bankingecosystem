# BankingEcosystem.Interop — Documentation

> **Project Type:** .NET Class Library  
> **Target Framework:** .NET 10.0  
> **Role:** Native P/Invoke Wrapper  
> **Dependencies:** None (Reference to C++ DLLs at runtime)

---

## Overview

This project contains `[DllImport]` (Platform Invocation Services / P/Invoke) declarations to bridge Managed C# code with Unmanaged C++ code from `BankingEcosystem.Hardware.dll` and `BankingEcosystem.NativeLogic.dll`.

## Project Structure

```
BankingEcosystem.Interop/
├── NativeMethods.cs             # Extern static function declarations
└── BankingEcosystem.Interop.csproj
```

---

## Native Signatures

### Hardware DLL (`BankingEcosystem.Hardware.dll`)

These functions control the ATM hardware simulation.

```csharp
// Card Reader
[DllImport("BankingEcosystem.Hardware.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern IntPtr CardReader_ReadCard();

[DllImport("BankingEcosystem.Hardware.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern bool CardReader_EjectCard();

// Cash Dispenser
[DllImport("BankingEcosystem.Hardware.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern bool CashDispenser_DispenseCash(int amount);

[DllImport("BankingEcosystem.Hardware.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern int CashDispenser_GetRemainingCash();

// Printer
[DllImport("BankingEcosystem.Hardware.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern bool ReceiptPrinter_Print(string content);

// Free Memory (Helper)
[DllImport("BankingEcosystem.Hardware.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern void FreeString(IntPtr ptr);
```

### Native Logic DLL (`BankingEcosystem.NativeLogic.dll`)

Low-level security and validation logic functions.

```csharp
// Encryption
[DllImport("BankingEcosystem.NativeLogic.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern IntPtr Encryption_EncryptPin(string pin);

// Validator
[DllImport("BankingEcosystem.NativeLogic.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern bool Validator_ValidateCardNumber(string cardNumber);
```

---

## Memory Management

Since C++ returns pointers (`char*`), manual marshaling is required:

1. The C++ function returns an `IntPtr`.
2. C# uses `Marshal.PtrToStringAnsi(ptr)` to read the string.
3. C# calls `FreeString(ptr)` to prevent memory leaks in the Native Heap.
