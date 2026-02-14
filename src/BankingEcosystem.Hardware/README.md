# BankingEcosystem.Hardware — Documentation

> **Project Type:** C++ Dynamic Link Library (DLL)  
> **Platform:** Windows (x64)  
> **Role:** Hardware Simulation Layer  
> **Dependencies:** Standard C++ Runtime

---

## Overview

This project is an ATM hardware simulation written in **C++**. Since we do not have a physical ATM machine, this module mimics the behavior of hardware components such as the Card Reader, Cash Dispenser, and Receipt Printer.

This module is exported as a **DLL** (`BankingEcosystem.Hardware.dll`) which is then called by the C# application (`Atm.Client`) via the **P/Invoke** mechanism.

## Project Structure

```
BankingEcosystem.Hardware/
├── include/
│   └── Hardware.h               # Header file (dllexport declarations)
├── src/
│   ├── CardReader.cpp           # Card read/eject simulation
│   ├── CashDispenser.cpp        # Cash dispense & inventory simulation
│   └── ReceiptPrinter.cpp       # Receipt printing simulation (to text file)
├── bin/                         # Output DLL
└── BankingEcosystem.Hardware.vcxproj
```

---

## Exposed Functions (API)

The following functions are exported with `extern "C"` so that function names are not subject to _name mangling_ and can be called from C#.

### 1. Card Reader

- `CardReader_ReadCard()`: Returns the card number (string) if a card is detected.
- `CardReader_EjectCard()`: Returns `true` if the card is successfully ejected.

### 2. Cash Dispenser

- `CashDispenser_DispenseCash(int amount)`:
  - Simulation logic: Decreases the count of virtual bills.
  - Returns `true` if successful.
- `CashDispenser_GetRemainingCash()`:
  - Returns the total cash remaining in the ATM "cassette".

### 3. Receipt Printer

- `ReceiptPrinter_Print(const char* content)`:
  - Accepts receipt content string.
  - Writes a `.txt` file to the `receipts/` folder with a unique timestamp.
  - Returns `true` if file writing is successful.

---

## Memory Management

Since C++ does not have a Garbage Collector:

- Strings returned by the `ReadCard` function are allocated using `malloc` or `new`.
- C# ("Caller") is responsible for freeing this memory.
- We provide a helper function: `FreeString(char* ptr)` to be called by C# after it has finished using the string.

## Build Instructions

This project uses **MSBuild** (Visual Studio Solution).

1. Open `BankingEcosystem.sln` in Visual Studio.
2. Select Configuration: **Debug** or **Release**.
3. Select Platform: **x64**.
4. Build Project.
5. The output DLL will be automatically copied to the C# project `bin` folder via Post-Build Event (if configured) or manual copy.
