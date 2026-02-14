# BankingEcosystem.Tests — Documentation

> **Project Type:** xUnit Test Project  
> **Target Framework:** .NET 10.0-windows  
> **Role:** Quality Assurance (Unit, Integration, End-to-End)  
> **Dependencies:** xUnit, Moq, FluentAssertions, Microsoft.AspNetCore.Mvc.Testing

---

## Overview

This project contains a suite of automated tests to ensure the correctness of business logic, integration between components, and End-to-End user scenarios.

Testing Strategy:

1. **Unit Tests:** Isolate logic (e.g., `TransactionService`, `AuthService`) by mocking dependencies (`Moq`).
2. **Integration Tests:** Test interactions with an In-Memory Database (`EF Core InMemory`).
3. **End-to-End (E2E):** Simulate full flow from Login -> Transaction -> Logout.

## Project Structure

```
Testing/
├── UnitTests/
│   ├── TransactionServiceTests.cs   # Cash withdrawal & transfer logic tests
│   ├── DtoTests.cs                  # Model/DTO validation tests
│   └── SessionServiceTests.cs       # Session management & timeout tests
├── IntegrationTests/
│   ├── BackendServiceTests.cs       # Backend service tests with InMemory DB
│   ├── DbInitializer.cs             # Dummy data seeding for test environment
│   └── DataSeedingTests.cs          # Initial data verification
└── EndToEnd/
    └── EndToEndScenarioTests.cs     # Complete user scenarios (Budi, Siti, Andi)
```

---

## Test Scenarios

### 1. End-To-End Scenarios (`EndToEndScenarioTests.cs`)

This file contains "Real World" scenarios run against the fully integrated system.

- **Scenario_NormalWithdrawal_Flow (Budi):**
  - Initial Balance: Rp 5,000,000.
  - Login with encrypted PIN.
  - Withdraw Rp 100,000 (Valid).
  - Verify Final Balance: Rp 4,900,000.

- **Scenario_Transfer_Flow (Budi -> Siti):**
  - Transfer Rp 500,000.
  - Verify sender's balance decreases.
  - Verify receiver's balance increases.

- **Scenario_LowBalance_Rejection (Siti):**
  - Balance: Rp 150,000.
  - Attempt to withdraw Rp 150,000 (Fail, minimum remaining balance must be Rp 50,000).
  - Verify transaction rejected.

- **Scenario_FraudLimit (Andi):**
  - Attempt to withdraw Rp 50,000,000 (Daily Max Limit).
  - Success.
  - Attempt to withdraw another Rp 10,000 -> Fail (Limit Exceeded).

### 2. Backend Service Tests (`BackendServiceTests.cs`)

Tests backend service logic directly without going through controllers/HTTP.

- Verify `VerifyPin` logic with encryption.
- Verify `DailyLimit` calculation.
- Verify unique account number generation.

### 3. Transaction Logic (`TransactionServiceTests.cs`)

Tests client-side logic before calling API.

- Ensure `AppLayer` calls `HardwareInterop` correctly.
- Uses `Mock<IUiService>` and `Mock<IAtmStateService>` for isolation.

---

## Running Tests

### Via Visual Studio

- Open **Test Explorer**.
- Click **Run All Tests**.

### Via CLI (Command Line)

Run from the solution root folder:

```bash
# Run all tests
dotnet test src/Testing/BankingEcosystem.Tests.csproj

# Run only End-to-End scenarios
dotnet test src/Testing/BankingEcosystem.Tests.csproj --filter "FullyQualifiedName~EndToEndScenarioTests"
```

## Test Data (Seeding)

Dummy data provided in `DbInitializer.cs`:

| User             | PIN    | Initial Balance | Role                   |
| :--------------- | :----- | :-------------- | :--------------------- |
| **Budi Santoso** | 123456 | Rp 5.000.000    | Savings (Standard)     |
| **Siti Aminah**  | 654321 | Rp 150.000      | Checking (Low Balance) |
| **Andi Wijaya**  | 112233 | Rp 100.000.000  | Business (High Limit)  |

**Note:** PINs in the database are stored as Hashes (BCrypt). The test helper `EncryptPin` is used to mimic hardware encryption.
