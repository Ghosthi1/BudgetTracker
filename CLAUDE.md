# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## IDE

JetBrains Rider on Windows.

## Project Overview

BudgetTracker is a cross-platform budget tracking app built with **.NET MAUI** (v10.0) and **C#**, targeting Android, iOS, macOS Catalyst, and Windows. **Supabase** (v1.1.1) is used as the backend (database, auth, real-time). The MVVM pattern is supported via **CommunityToolkit.Mvvm** (v8.4.2).

The app is being built from scratch on top of the default MAUI template. It replaces a manual Excel bill-tracking spreadsheet.

**Core concept:** recurring bills (monthly or yearly) with a due day and a paid/unpaid status per cycle. Not a general transaction ledger — the primary model is `Bill`, not `Transaction`.

Core features to implement: listing bills, marking them as paid, seeing what's due/upcoming in the current cycle, categorisation, and a summary view — backed by Supabase for persistence and user authentication.

## Build & Run Commands

```bash
# Restore dependencies
dotnet restore

# Build (all platforms)
dotnet build -c Debug
dotnet build -c Release

# Platform-specific builds
dotnet build -f net10.0-windows10.0.19041.0 -c Debug
dotnet build -f net10.0-android -c Debug
dotnet build -f net10.0-ios -c Debug

# Clean
dotnet clean
```

There are currently no tests in this project.

## Page Structure

Pages live under `Pages/` organised by feature:
- `Pages/Login/MainPage.xaml` — login page (email + password `Entry` fields, Sign In button, temp nav button). Class: `BudgetTracker.MainPage`. Registered as the Shell root in `AppShell.xaml`.
- `Pages/ShowBudgets/Budget.xaml` — bills/budget list page (empty so far). Class: `BudgetTracker.Pages.ShowBudgets.Budget`. Registered as route `"bills"` in `AppShell.xaml.cs`.

Navigation from login → budget page: `await Shell.Current.GoToAsync("bills")` in `MainPage.xaml.cs`.

The login page uses code-behind style (no ViewModel yet). Auth and session persistence are not yet wired up — Supabase is installed but not configured.

## Architecture

**Navigation:** Shell-based navigation via `AppShell.xaml`. The root page is declared as `ShellContent` in `AppShell.xaml`. Additional pages are registered with `Routing.RegisterRoute()` in `AppShell.xaml.cs`.

**Dependency Injection:** Configured in `MauiProgram.cs`. Register services, pages, and ViewModels there.

**Styling:** Global colors are in `Resources/Styles/Colors.xaml` (supports light/dark via `AppThemeBinding`). Global control styles are in `Resources/Styles/Styles.xaml`. Pre-defined label styles: `"Headline"` (32pt bold) and `"SubHeadline"` (24pt).

**Platform-specific code:** Lives under `Platforms/{Android,iOS,MacCatalyst,Windows}/`. MAUI handles most cross-platform concerns automatically.

**XAML compilation:** Uses `<MauiXamlInflator>SourceGen</MauiXamlInflator>` — XAML is compiled to C# at build time via source generation.

**Windows:** Runs unpackaged (`<WindowsPackageType>None</WindowsPackageType>`), so no MSIX packaging is required.

**Android builds:** Java SDK path is set in `Directory.Build.props` (`C:\Program Files\Microsoft\jdk-21.0.10.7-hotspot`). If the JDK moves, update that file.

**Nullable & implicit usings** are both enabled project-wide.

## Working Style

### File Edits
- Always show the user the proposed change and explain what it does before editing any file.
- Do not create new files without explicit approval.
- When multiple files need changing, list them all upfront and get confirmation before proceeding.
- `CLAUDE.md` is an exception — update it freely as new decisions, patterns, or context are established during the session.

### Learning Support
- Explain *why* a pattern or approach is used, not just *what* the code does.
- When introducing a MAUI or Supabase concept for the first time, give a brief explanation before writing code.
- If there are multiple valid approaches, describe the trade-offs so the user can make an informed choice.
- Point out relevant official docs or concepts by name so the user can read further independently.

## Key Dependencies

- `CommunityToolkit.Mvvm` — use `ObservableObject`, `[ObservableProperty]`, and `[RelayCommand]` for ViewModels
- `Supabase` — backend client; package is installed but not yet wired up in `MauiProgram.cs`. Auth plan: email + password login with session persisted in `SecureStorage` so users stay logged in across app launches.

## Domain Model

**`Bill`** (`Models/Bill.cs`) — the core model:
- `Guid Id`
- `string Name` — e.g. "Netflix"
- `decimal Amount`
- `int DueDay` — day of month (monthly) or day of year (yearly)
- `Frequency Frequency` — `Monthly` or `Yearly` enum
- `bool IsPaid` — paid this cycle?
- `string Category` — e.g. "Subscriptions", "Utilities"
