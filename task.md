# Banking Ecosystem Simulation — Task Tracker

## 🔵 Phase 1 — Foundation (Core)

- [x] Setup solution structure + semua project references
- [x] Database schema + EF Core migrations
- [x] Backend API: Auth + Account + Transaction
- [x] C++ Hardware DLL: CardReader + CashDispenser
- [x] C++ NativeLogic DLL: FSM + Validator + Encryption
- [x] C# Interop wrapper (P/Invoke declarations)
- [x] Architecture plan review & approval

## 🟢 Phase 2 — ATM Client

- [x] WPF ATM UI (`Atm.Client`)
  - [x] Views: Intro, Onboarding, PIN Entry, Main Menu
  - [x] Theme & Styling (`AtmTheme.xaml` — Navy & Gold)
  - [x] Responsive PIN Entry Layout
  - [x] Fixed: Launch Failure (Rebuilt project)
  - [x] Fixed: XAML Resource Crash (NavyBrush)
- [x] C# Application Layer (`Atm.AppLayer`)
  - [x] Service Interfaces (IAuthService, ITransactionService, IHardwareInteropService)
  - [x] `AtmSessionService` (State, Timeout 60s, CardId)
  - [x] `AuthService` (VerifyCard + VerifyPin via Backend API)
  - [x] `TransactionService` (GetBalance + Withdraw + Hardware check)
  - [x] `HardwareInteropService` (P/Invoke wrapper)
  - [x] DI Registration in `App.xaml.cs`
- [x] Connect UI → App Layer → Backend + Hardware
  - [x] `OnboardingView` → `IAuthService.VerifyCardAsync()` (card validation)
  - [x] `PinEntryView` → `IAuthService.VerifyPinAsync(cardId, pin)` (real auth)
  - [x] `MainMenuView` → `ITransactionService.GetBalanceAsync()` (cek saldo)
  - [x] Session timeout → auto navigate to Onboarding
  - [x] DI injection for all Views
  - [x] Fixed: Backend URL port (5046)
  - [x] Fixed: Missing ATM seed data (FK constraint)
- [x] Documentation
  - [x] Client README (`src/BankingEcosystem.Atm.Client/doc/README.md`)
  - [x] Connection Map (layer-by-layer file reference)
- [x] Integration & end-to-end testing (46/46 passed ✅)

## 🟡 Phase 3 — Bank Office System (Admin App)

- [ ] WPF Admin UI: Login karyawan, dashboard
- [ ] Manajemen nasabah (CRUD customer + account)
- [ ] Manajemen ATM (refill, monitor status, on/offline)
- [ ] Reporting & audit log viewer

## 🟠 Phase 4 — Full ATM Features

- [ ] Transfer antar rekening
- [ ] Balance inquiry + mini statement
- [ ] Receipt generation
- [ ] ATM state machine (C++ FSM)

## 🔴 Phase 5 — Polish & Security

- [ ] PIN encryption + secure communication
- [ ] Fraud detection (max attempts, daily limit)
- [ ] Error handling + recovery flows
- [ ] UI polish (animations, realistic ATM skin)
- [ ] Comprehensive testing + seeding data

## ⚪ Phase 6 — Mobile Banking (Future)

- [ ] TBD (MAUI / Flutter / React Native)
