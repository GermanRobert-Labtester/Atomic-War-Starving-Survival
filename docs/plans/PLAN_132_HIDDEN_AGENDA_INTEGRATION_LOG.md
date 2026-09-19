# Plan 132 (C2[26]) — Survivor Hidden Agendas & Betrayal Arc Full Integration Log

**Date:** 2026-09-20
**Corpus Key:** `C2[26]`
**Status:** SEALED
**Owner:** Foreman Integration Agent

---

## 1. Executive Summary

Plan 132 (`C2[26]`) specifies the **Survivor Hidden Agendas & Betrayal Arc System**:
- Core domain model for hidden agendas (`ResourceTheft`, `Sabotage`, `InformationLeak`, `EscapePlan`, `FactionLoyalty`, `CultRecruitment`, `SecretProtection`).
- Discovery, investigation, and clue generation mechanics leading to exposure above the 60% threshold.
- Multi-branch confrontation resolution: `Reconciled`, `Expelled`, `Executed`, `Betrayed`, and `CoverUp`.
- Passive slip-ups accumulating progress over long horizons (> 10 days).
- Full host session orchestration, persistence via `HiddenAgendaSaveStore` with `SaveStoreHub` checksumming, Godot main application wiring, interactive UI panel (`HiddenAgendaPanel`), navigation button in `GameDashboardPanel`, headless selftest (`--hidden-agenda-selftest`), and exhaustive unit/integration tests.

---

## 2. Architecture & Seam Implementation

### 2.1 Core Domain (`Assets/Ashfall.Core/Survivors/`)
- [`Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs`](../../Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs):
  - Retained pure engine-free C# implementation.
  - Added read-only queries: `GetAllAgendas()`, `GetCluesForAgenda(string agendaId)`, and `GetAllClues()`.
  - Supports full lifecycle: `AssignAgenda()`, `InvestigateAgenda()`, `ConfrontSurvivor()`, `TickDay()`, `CaptureState()`, and `RestoreState()`.

### 2.2 Persistence (`src/Host/HiddenAgendaSaveStore.cs`)
- [`src/Host/HiddenAgendaSaveStore.cs`](../../src/Host/HiddenAgendaSaveStore.cs):
  - Checksummed persistence store using `SaveStoreHub.FromCodec`.
  - Saves to `user://hidden_agenda_save.json` and supports envelope capture for `campaign.json`.
- Registered in [`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`](../../Assets/Ashfall.Core/Save/SaveSectionRegistry.cs):
  - Key: `hidden_agenda`
  - Group: `ExpandedShelterLifecycleGroup`
  - Section file: `hidden_agenda_save.json`

### 2.3 Host Orchestration & Godot Integration (`src/Host/`, `src/Main.*.cs`)
- [`src/Host/HiddenAgendaHostSession.cs`](../../src/Host/HiddenAgendaHostSession.cs):
  - Wraps `HiddenAgendaSystem`, coordinating `StateChanged` notifications, `AssignAgenda`, `Investigate`, `Confront`, and `TickDay`.
- [`src/Main.HiddenAgenda.cs`](../../src/Main.HiddenAgenda.cs):
  - Coordinates `EnsureHiddenAgenda()`, `SetupHiddenAgenda()`, `SaveHiddenAgenda()` via `CaptureSection`, `TickHiddenAgenda(day)`, and `ShowHiddenAgendaPanel()`.
  - Conforms to the Triad Drift Gate (`private void SetupHiddenAgenda()`, `private void SaveHiddenAgenda()`).
- [`src/Main.ExpandedShelterSystems.cs`](../../src/Main.ExpandedShelterSystems.cs):
  - Integrated into expanded shelter setup, save, and daily tick routines.
- [`src/Main.PlayerSurfaces.cs`](../../src/Main.PlayerSurfaces.cs):
  - Added `"hidden_agenda"` to `expandedIds`.
- [`Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs`](../../Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs) & [`Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`](../../Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs):
  - Registered as `"Survivor Intrigue & Hidden Agendas"` in `PanelGroup.Expanded`.

### 2.4 Presentation & UI (`src/UI/`)
- [`src/UI/HiddenAgendaPanel.cs`](../../src/UI/HiddenAgendaPanel.cs):
  - Full `IBindablePanel` Godot UI.
  - Status rail: Active Agendas, Known Clues, Exposures, Betrayals.
  - Scrollable agenda roster with severity and suspicion indicators.
  - Detailed dossier inspection pane with clue list.
  - Action bar: Investigate, Reconcile, Exploit, Expel, Attack.
- [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs):
  - Wired `INTRIGUE` navigation button directing to `"hidden_agenda"`.

### 2.5 Verification & Diagnostic Gates
- [`Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs`](../../Ashfall.Core.Tests/Survivors/Plan132HiddenAgendaIntegrationTests.cs):
  - Unit and integration tests covering multi-day investigation, clue discovery, exposure thresholds, confrontation branches, passive slip-up timing, and full state serialization round-trip.
- [`src/Host/HiddenAgendaSelfTest.cs`](../../src/Host/HiddenAgendaSelfTest.cs):
  - Headless verification runner testing Core, Host, Persistence, and UI binding lifecycle.
  - Registered in `HostCliRegistry.cs`, `HostCli.cs`, `Main.Application.cs`, and `generate-architecture-map.py`.
