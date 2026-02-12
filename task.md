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
