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

### Step 2.5: Simulated Card Entry (New Request)

- [x] UI — `OnboardingView.xaml`: Add "Insert Card" overlay (TextBox + Button)
- [x] Logic — `OnboardingView.xaml.cs`: Handle 'C' key to show overlay
- [x] Logic — `OnboardingView.xaml.cs`: Call `VerifyCardAsync` with user input
- [x] Verify: Test with new card created in Admin App
- [x] Fixed: "Gagal memuat saldo" (Added default ATM seed data ID 1)

## 🟡 Phase 3 — Bank Office System (Admin App)

### Step 1: WPF Admin UI — Login karyawan, Dashboard

- [x] Foundation & Setup
  - [x] Add NuGet packages (`CommunityToolkit.Mvvm`, `Microsoft.Extensions.DependencyInjection`, `Microsoft.Extensions.Http`)
  - [x] Fix broken `MainWindow.xaml.cs` (duplicate class)
  - [x] Create `AdminTheme.xaml` (corporate blue/gray styling)
  - [x] Trim `App.xaml.cs` DI to Step 1 services only
  - [x] Trim `MainWindow.xaml` DataTemplates to Step 1 only
- [x] Employee Login
  - [x] `AdminAuthService.cs` — call `POST /api/auth/employee-login`, store JWT
  - [x] `NavigationService.cs` — simple view switching via `CurrentView`
  - [x] `LoginViewModel.cs` — EmployeeCode, Password, validation, LoginCommand
  - [x] `LoginView.xaml` — login UI (TextBox, PasswordBox, Button)
  - [x] `MainViewModel.cs` — CurrentView, IsLoggedIn, navigation commands, Logout
- [x] Dashboard (Placeholder)
  - [x] `DashboardViewModel.cs` — placeholder stats
  - [x] `DashboardView.xaml` — welcome screen with placeholder cards
- [x] Build & Verify — `dotnet build` passes ✅

### Step 2: Customer & Account Management

- [x] Backend — New Endpoints
  - [x] `GET /api/admin/customers/{id}/accounts` — accounts by customer
  - [x] `POST /api/admin/accounts` — create account
  - [x] `GET /api/admin/customers/{id}/cards` — cards by customer
  - [x] `POST /api/admin/cards` — issue card
  - [x] New DTOs: `CustomerDetailDto`, `AccountTypeDto`
- [x] Admin UI — Service
  - [x] `AdminApiService.cs` — HTTP client for customer/account/card endpoints
- [x] Admin UI — ViewModels
  - [x] `CustomerListViewModel.cs` — list, search, navigate
  - [x] `CustomerDetailViewModel.cs` — info, accounts, cards, actions
  - [x] `AddCustomerViewModel.cs` — form + validation + save
- [x] Admin UI — Views
  - [x] `CustomerListView.xaml` — search bar + DataGrid + Add button
  - [x] `CustomerDetailView.xaml` — info panel + accounts/cards grids
  - [x] `AddCustomerView.xaml` — form with validation
- [x] Admin UI — Navigation & DI
  - [x] Add "Nasabah" nav button in `MainWindow.xaml`
  - [x] Add DataTemplates for new ViewModels
  - [x] Register services + VMs in `App.xaml.cs`
  - [x] `NavigateToCustomers` command in `MainViewModel.cs`
- [x] Build & Verify — Admin UI + Backend build passed ✅

### Step 3: ATM Management

- [x] 3. Manajemen ATM (refill, monitor status, on/offline)
  - [x] Backend: Update `AdminApiService.cs` (add ATM endpoints)
  - [x] ViewModel: `AtmListViewModel.cs` (List, Toggle Status, Nav to Create/Refill)
  - [x] ViewModel: `AddAtmViewModel.cs` (Create form)
  - [x] ViewModel: `RefillAtmViewModel.cs` (Refill dialog logic)
  - [x] View: `AtmListView.xaml` (DataGrid + Actions)
  - [x] View: `AddAtmView.xaml` (Form)
  - [x] View: `RefillAtmView.xaml` (Dialog/Popup)
  - [x] Navigation: Add "ATM" to sidebar & App.xaml DI
  - [x] Build & Verify — Admin UI build ✅

### Step 4: Reporting & Audit Log Viewer

- [x] 4. Reporting & audit log viewer (Real-time monitoring)
  - [x] Backend: `GET /transactions` & `GET /reports/dashboard`
  - [x] Frontend: Real-time Dashboard Stats
  - [x] Frontend: Transaction List (DataGrid + Filter)
  - [x] Frontend: Audit Log Viewer
  - [x] Build Passed (Backend & UI)
  - [x] Verification (Run & Test)

### Documentation

- [x] Create `src/BankingEcosystem.Admin.UI/doc/README.md` (Project Structure, Architecture, Setup)

## 🟠 Phase 4 — Full ATM Features

### Step 1: Transfer Antar Rekening

- [x] 1. Transfer antar rekening
  - [x] Backend: `POST /api/transaction/transfer` endpoint
  - [x] AppLayer: `ITransactionService.TransferAsync`
  - [x] ViewModel: `TransferViewModel.cs`
  - [x] View: `TransferView.xaml` (Input Account -> Input Amount -> Confirm)
  - [x] Navigation: Main Menu -> Transfer Feature
  - [x] Verification

### Step 2: Balance Inquiry & Mini Statement

- [x] 2. Balance inquiry + mini statement
  - [x] Backend: `GET /api/transaction/balance` & `/history`
  - [x] AppLayer: `ITransactionService.GetHistoryAsync`
  - [x] ViewModel: `HistoryViewModel.cs`
  - [x] View: `HistoryView.xaml` (DataGrid/ListView of transactions)
  - [x] Balance Inquiry: Enhance `MainMenuView` integration
  - [x] Verification

### Step 3: Receipt Generation

- [x] 3. Receipt generation
  - [x] Helper: `ReceiptBuilder` (Header, Body, Footer formatter)
  - [x] AppLayer: Update `TransactionService` to use Builder
  - [x] Hardware: Update `HardwareInteropService` to save receipt to file (`receipts/`)
  - [x] Verification (Check generated files)

### Step 4: ATM State Machine (C++ FSM)

- [/] 4. ATM state machine (C++ FSM)
  - [x] Interop: Add `AtmFsm_*` P/Invoke declarations
  - [x] AppLayer: Create `AtmStateService`
  - [x] Integration: `OnboardingViewModel` (Idle -> CardInserted)
  - [x] Integration: `PinEntryViewModel` (CardInserted -> PinEntry -> Authenticated)
  - [x] Integration: `TransactionService` (Authenticated -> Transaction -> Dispensing -> Completed)
  - [x] Verification (Test flow enforcement)

### Step 5: Remaining Features (Withdraw, Deposit, Change PIN)

- [/] 5. Remaining Features
  - [x] **Withdrawal** (Tarik Tunai)
    - [x] ViewModel: `WithdrawViewModel` (Amounts grid + Custom amount)
    - [x] View: `WithdrawView.xaml`
    - [x] Navigation: Update `MainMenuView`
  - [ ] **Deposit** (Setor Tunai)
    - [x] Backend: `POST /api/transaction/deposit` (Verified)
    - [x] AppLayer: `TransactionService.DepositAsync`
    - [x] ViewModel: `DepositViewModel` (Cash slot simulation)
    - [x] View: `DepositView.xaml`
    - [x] Navigation: Update `MainMenuView`
  - [ ] **Change PIN** (Ubah PIN)
    - [x] Backend: Add `POST /api/auth/change-pin`
    - [x] AppLayer: `AuthService.ChangePinAsync`
    - [x] ViewModel: `ChangePinViewModel` (Old PIN -> New PIN -> Confirm)
    - [x] View: `ChangePinView.xaml`
    - [x] Navigation: Update `MainMenuView`
  - [x] Verification (End-to-End for all features)

## 🔴 Phase 5 — Polish & Security

### Step 1: Secure PIN Communication (E2EE)

- [ ] 1. Secure PIN Communication
  - [x] NativeLogic: Add `Encryption_EncryptPin` (Simulated XOR)
  - [x] Backend: Add Decryption logic in `AuthService` or Middleware
  - [x] Client: Encrypt PIN in `AuthService` before sending `VerifyPinRequest`
  - [ ] Verification: Ensure PIN provided in API logs is encrypted

### Step 2: Fraud Detection & Limits

- [ ] 2. Fraud Detection
  - [ ] Backend: Enforce Daily Withdrawal Limit
  - [ ] Backend: Enforce Max PIN Attempts

### Step 3: Error Handling & Recovery

- [ ] 3. Error Handling
  - [ ] Global Exception Handler in Client
  - [ ] Retry Policy for Network Requests

### Step 4: UI Polish

- [ ] 4. UI Polish
  - [ ] Animations
  - [ ] Realistic "Processing" delays

### Step 5: Final Verification

- [ ] 5. Final Verification
  - [ ] End-to-End Test Run
