# Phase 4: Full ATM Features

## Step 1: Transfer Antar Rekening

**Goal**: Enable users to transfer funds to another account within the same bank.

### 1. AppLayer (`BankingEcosystem.Atm.AppLayer`)

- **Interface**: `ITransactionService`
  - Add `Task<TransactionDto> TransferAsync(decimal amount, string targetAccountNumber);`
- **Implementation**: `TransactionService`
  - Call `POST /api/transaction/transfer`
  - Handle errors (insufficient balance, target not found, etc.)

### 2. ViewModel (`TransferViewModel.cs`)

- **State Management**:
  - `Step 1`: Input Target Account Number
  - `Step 2`: Input Amount
  - `Step 3`: Confirmation (Show Target Name & Amount)
  - `Step 4`: Processing & Result
- **Commands**:
  - `SubmitAccountCommand`: Validate account number format (digits only).
  - `SubmitAmountCommand`: Validate amount > 0.
  - `ConfirmTransferCommand`: Execute transfer via `ITransactionService`.
  - `CancelCommand`: Return to Main Menu.
- **Keypad Support**: Handle numeric input for both Account Number and Amount.

### 3. View (`TransferView.xaml`)

- **Layout**:
  - Wizard-style UI (switching visibility of Grid/StackPanel based on `CurrentStep`).
  - **Step 1**: "Masukkan Nomor Rekening Tujuan" (TextBox)
  - **Step 2**: "Masukkan Jumlah Transfer" (TextBox + Currency Format)
  - **Step 3**: "Konfirmasi Transfer" (Text: Ke [Nama], Jumlah [Rp ...])
  - **Step 4**: Success/Error Message + "Cetak Struk?" option (simulated).

### 4. Navigation

- Update `MainMenuView.xaml` to add "Transfer" button.
- Update `NavigationService` or `MainWindow` to handle navigation to `TransferView`.

### 5. Verification

- Run Backend & ATM Client.
- Login with User A.
- Transfer to User B (e.g., from seed data).
- Verify User A balance decreases.
- Verify User B balance increases (via Admin App or DB).
