# Plan 207 (C1[39]) — Shelter Reputation & External Perception Full Integration Log

**Date:** 2026-09-20
**Corpus Key:** `C1[39]`
**Status:** SEALED
**Owner:** Foreman Integration Agent

---

## 1. Executive Summary

Plan 207 (`C1[39]`) specifies the **Shelter Reputation & External Perception System**:
- Core domain model for shelter reputation dimensions (`Reliability`, `Strength`, `Generosity`, `Ruthlessness`, `Wealth`).
- Evidence provenance tracking across information media (`Witness`, `RadioBroadcast`, `TraderWord`, `RefugeeReport`, `Propaganda`) with anti-farming duplicate protection.
- Dynamic notoriety scaling based on evidence salience, delta, and media reach multipliers.
- Threshold-based public tag evaluation (`Sanctuary`, `TradingPost`, `Fortress`, `Dangerous`, `Treacherous`, `Honorable`, `Desperate`, `RaiderBane`).
- Daily decay toward neutrality for scores and notoriety.
- Full host session orchestration, persistence via `ShelterReputationSaveStore` with `SaveStoreHub` checksumming, Godot main application wiring, interactive UI panel (`ShelterReputationPanel`), navigation button in `GameDashboardPanel`, headless selftest (`--shelter-reputation-selftest`), and exhaustive unit/integration tests.

---

## 2. Architecture & Seam Implementation

### 2.1 Core Domain (`Assets/Ashfall.Core/Reputation/`)
- [`Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs`](../../Assets/Ashfall.Core/Reputation/ShelterReputationSystem.cs):
  - Pure engine-free C# implementation.
  - Dimension scores clamped between -100 and +100.
  - Media reach multipliers for notoriety (`RadioBroadcast` 2.0x, `Propaganda` 1.5x, `TraderWord` 1.2x, `RefugeeReport` 1.0x, `Witness` 0.8x).
  - Anti-farming duplicate suppression for identical source event, medium, and audience.
  - Supports full lifecycle: `RecordEvidence()`, `TickDay()`, `GetScore()`, `HasTag()`, `GetActiveTags()`, `CaptureState()`, and `RestoreState()`.

### 2.2 Persistence (`src/Host/ShelterReputationSaveStore.cs`)
- [`src/Host/ShelterReputationSaveStore.cs`](../../src/Host/ShelterReputationSaveStore.cs):
  - Checksummed persistence store using `SaveStoreHub.FromCodec`.
  - Saves to `user://shelter_reputation_save.json` and supports envelope capture for `campaign.json`.
- Registered in [`Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`](../../Assets/Ashfall.Core/Save/SaveSectionRegistry.cs):
  - Key: `shelter_reputation`
  - Group: `ExpandedShelterLifecycleGroup`
  - Section file: `shelter_reputation_save.json`

### 2.3 Host Orchestration & Godot Integration (`src/Host/`, `src/Main.*.cs`)
- [`src/Host/ShelterReputationHostSession.cs`](../../src/Host/ShelterReputationHostSession.cs):
  - Wraps `ShelterReputationSystem`, coordinating `StateChanged` notifications, `RecordEvidence`, `TickDay`, queries, and state persistence.
- [`src/Main.ShelterReputation.cs`](../../src/Main.ShelterReputation.cs):
  - Coordinates `EnsureShelterReputation()`, `SetupShelterReputation()`, `SaveShelterReputation()` via `CaptureSection`, `TickShelterReputation(day)`, and `ShowShelterReputationPanel()`.
  - Conforms strictly to the Triad Drift Gate (`private void SetupShelterReputation()`, `private void SaveShelterReputation()`).
- [`src/Main.ExpandedShelterSystems.cs`](../../src/Main.ExpandedShelterSystems.cs):
  - Integrated into expanded shelter setup, save, and daily tick routines.
- [`src/Main.PlayerSurfaces.cs`](../../src/Main.PlayerSurfaces.cs):
  - Added `"shelter_reputation"` to `expandedIds`.
- [`Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs`](../../Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs) & [`Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`](../../Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs):
  - Registered as `"Shelter Reputation & External Perception"` in `PanelGroup.Expanded`.

### 2.4 Presentation & UI (`src/UI/`)
- [`src/UI/ShelterReputationPanel.cs`](../../src/UI/ShelterReputationPanel.cs):
  - Full `IBindablePanel` Godot UI.
  - Status rail: Notoriety, Dominant Trait, Active Tags, Evidence Events.
  - Left column: 5 dimension gauges with colored progress bars and public tag badges.
  - Right column: Wasteland perception briefing and recent evidence log.
- [`src/UI/GameDashboardPanel.cs`](../../src/UI/GameDashboardPanel.cs):
  - Wired `REPUTATION` navigation button directing to `"shelter_reputation"`.

### 2.5 Verification & Diagnostic Gates
- [`Ashfall.Core.Tests/Reputation/Plan207ShelterReputationIntegrationTests.cs`](../../Ashfall.Core.Tests/Reputation/Plan207ShelterReputationIntegrationTests.cs):
  - Unit and integration tests covering evidence recording, medium reach multipliers, anti-farming protection, notoriety accumulation, tag threshold triggers, daily decay, and full state serialization round-trip.
- [`src/Host/ShelterReputationSelfTest.cs`](../../src/Host/ShelterReputationSelfTest.cs):
  - Headless verification runner testing Core, Host, Persistence, and UI binding lifecycle.
  - Registered in `HostCliRegistry.cs`, `HostCli.cs`, `Main.Application.cs`, and `generate-architecture-map.py`.
