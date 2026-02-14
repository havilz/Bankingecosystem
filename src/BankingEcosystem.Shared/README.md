# BankingEcosystem.Shared — Documentation

> **Project Type:** .NET Class Library  
> **Target Framework:** .NET 10.0  
> **Role:** Shared Contracts & Definitions  
> **Dependencies:** None

---

## Overview

This library contains data type definitions shared between **Backend**, **Atm.Client**, and **Admin.UI**. The purpose is to ensure consistent Data Contracts across the entire ecosystem.

## Project Structure

```
BankingEcosystem.Shared/
├── DTOs/
│   └── DTOs.cs                  # All Request & Response Objects (Records)
├── Enums/
│   └── TransactionType.cs       # Withdrawal, Transfer, etc.
├── Constants/
│   └── AppConstants.cs          # Global constants (if any)
└── BankingEcosystem.Shared.csproj
```

---

## Data Transfer Objects (DTOs)

All DTOs are defined as C# `record` types for immutability and value-equality.

### Authentication

- `VerifyCardRequest` / `VerifyCardResponse`
- `VerifyPinRequest` / `AuthResponse`
- `VerifyEmployeeRequest` / `EmployeeLoginResponse`

### Account & Transaction

- `AccountDto`: Account Information (Balance, Limit).
- `TransactionDto`: Transaction History.
- `WithdrawRequest`: Payload for cash withdrawal.
- `TransferRequest`: Payload for fund transfer.
- `DepositRequest`: Payload for cash deposit.

### Administration

- `CustomerDto` / `CreateCustomerRequest`
- `AtmDto` / `CreateAtmRequest`

---

## Best Practices

1. **Shared Contract:** Do not add business logic here. Only data definitions (POCO/Record).
2. **Versioning:** Changes to DTOs here will affect **all** other projects (Backend & Clients). Perform changes with caution.
