# Mobile Banking Development Rules & Guidelines

> [!CAUTION]
> **ABSOLUTE SOURCE OF TRUTH**
>
> 1. `task.md` (The Roadmap: Phases & Steps)
> 2. `implementation_mobile.md` (The Blueprint: Architecture & Design)
> 3. `rules.md` (The Law: Constraints & Standards)
>
> All development MUST follow these three files strictly. **NO SHORTCUTS ALLOWED.**
> If a change is needed, update these documents _first_ before writing code.
> Any deviation without prior approval is unacceptable.

## 1. Architecture: Strict Clean Architecture

We adhere to the Architecture defined in `implementation_mobile.md`.

- **Feature-First Structure:** `lib/features/<feature_name>/`
- **Layers:** `presentation`, `domain`, `data`.
- **Forbidden Imports:**
  - `domain` layer MUST NOT import `data` or `presentation`.
  - `domain` layer MUST NOT import `flutter` (Pure Dart only).
  - UI Widgets MUST NOT call API directly (Use Providers/UseCases).

## 2. Tech Stack Mandates

- **State Management:** `flutter_riverpod` (Avoid `ChangeNotifier`). Use `podo` & `freezed` for state.
- **Networking:** `dio` with Interceptors. NO `http` package unless wrapped in adapter.
- **Navigation:** `go_router`. Use `context.go` or `context.push`.
- **Local Storage:** `flutter_secure_storage` for Credentials. `shared_preferences` for Flags only.

## 3. Widget Guidelines (Visual Consistency)

- **Atomic Design:** Use shared components in `lib/core/ui/widgets/`.
- **Styling:** Use `AppTheme` and `TextStyle` from `core`. NO hardcoded colors/fonts.
- **Responsiveness:** Ensure layouts work on different screen sizes (use `MediaQuery` or `LayoutBuilder` if needed).

## 4. Workflow & Process

- **Step-by-Step Execution:**
  - Follow `task.md` sequentially. Do NOT skip ahead.
  - Complete "Unit Testing" before moving to "Integration".
  - Mark completed tasks with `[x]` immediately.
- **Commit Standards:**
  - Atomic commits. Follow Conventional Commits (`feat:`, `fix:`, `chore:`, `test:`).
- **Quality Assurance:**
  - Code must compile with **zero warnings/errors**.
  - `flutter test` must pass before any merge.

## 5. Security Mandates

- **Data Privacy:** NEVER log sensitive info (PIN, Token, Password).
- **Secure Handling:** Use `SecureStorage` for tokens. Clear storage on Logout.
- **SSL:** Ensure API endpoints use `HTTPS`.

## 6. Debugging & Error Logging Protocol

- **Location:** `doc/debug/`.
- **Format:** Create a new markdown file for each significant error/issue (e.g., `YYYY-MM-DD_issue-name.md` or append to a log file).
- **Required Content:**
  1.  **Error Log:** The raw error message/stack trace.
  2.  **Root Cause:** Why it happened (Investigation).
  3.  **Solution:** The fix applied.
  4.  **Method:** _How_ it was fixed (Tools, commands, code changes).
  5.  **Reasoning:** _Why_ this method was chosen.
