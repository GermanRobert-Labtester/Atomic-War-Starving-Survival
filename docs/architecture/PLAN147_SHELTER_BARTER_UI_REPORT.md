# Plan 147 Follow-Up — Shelter Barter Terminal UI Architecture & Implementation Report

**Author:** Antigravity
**Date:** 2026-09-09
**Seam:** Shelter Barter Route (`ShelterBarterSystem` ↔ `ShelterBarterPanel` ↔ `PanelRegistry`)
**Design Authority:** Google Stitch MCP (Project `5749075276134882439`, Screen `8510d12322714acf990baaead9dabddc`, "Shelter Airlock Barter Terminal")

---

## 1. Executive Summary

As requested in the Plan 147 follow-up directive, the shelter barter route trade UI panel has been designed and implemented to provide a dedicated, interactive trading terminal bound to `ShelterBarterSystem`.

The screen adheres strictly to:
1. **Google Stitch Design Authority:** Reconciled from the live Stitch mockup ("Shelter Airlock Barter Terminal"), adopting the 320px merchant caravan manifest selector, two-column barter exchange ledger, arbitrator balance scale, pre-condition diagnostics rail, and cost/consequence summary strip.
2. **`state → blocker → cost → consequence` Panel Standard:**
   - **State:** Displays caravan arrival status, arrival day, days remaining until departure, airlock thermal status, stock levels, and current player inventory counts.
   - **Blocker:** Pre-condition diagnostics dynamically flag airlock freezes, caravan absence/departure, empty selections, insufficient value, inventory limits, and counterfeit detection.
   - **Cost:** Displays explicit unit valuations in Valuation Units (VU), line subtotal calculations, and total offered value against total requested value.
   - **Consequence:** Visualizes net trade delta, merchant tolerance margin check, surplus/deficit indicator, and projected inventory changes.
3. **UI-07 & UI-09 Compliance:**
   - No stub registration: routes (`shelter_barter`) are registered in `PanelRegistry` and `PlayerSurfaceManifest` as `PanelMaturity.Live` and `SurfaceActionCoverage.InteractiveCommands` only with full, operational command handling.
   - Non-empty `RefreshView` bound to live domain model (`ShelterBarterSystem`).
   - Human-readable feedback: zero raw item IDs exposed to the player (formatted via `FormatItemName` and catalog lookup).
   - Atomic `ExecuteTrade` command execution with journal feedback and state refresh.

---

## 2. Architecture & Seams

### 2.1 Core Contracts
- **`PanelRegistryBootstrap`:** Registered `shelter_barter` as `PanelGroup.Expanded` with dependency on `inventory`.
- **`PlayerSurfaceManifest`:** Added `shelter_barter` to `InteractivePanelIds` with `SurfaceRouteKind.ExpandedShelter` and `SurfaceActionCoverage.InteractiveCommands`.
- **`ActionResult` Contract:** Handled `ExecuteTrade` outcome via `result.IsSuccess` and `result.FailureCode` with human-readable error resolution.

### 2.2 Host Wiring & Lifecycle
- **`Main.Plans147.cs`:**
  - `EnsureShelterBarterPanel()`: Instantiates and attaches `ShelterBarterPanel` node.
  - `OpenShelterBarterPanel()`: Binds `ShelterBarterSystem`, player `Inventory`, `JournalSystem`, and item catalog resolver.
- **`Main.ExpandedShelterSystems.cs`:**
  - Routes `"shelter_barter"` to `OpenShelterBarterPanel()`.
  - Cleans up panel instance during teardown/reset.
- **`Main.GameFlow.cs`:**
  - Added `"shelter_barter"` dispatch to `OpenExpandedPanel`.
- **`Main.PlayerSurfaces.cs`:**
  - Configured `shelter_barter` actions (`bindAction`, `openAction`, `closeAction`).

### 2.3 UI Components (`src/UI/ShelterBarterPanel.cs`)
- **Shell:** Follows `AshfallUiHelpers.BuildStandardPanelShell` with responsive margins, `DesignTheme` colors, and fixed 1920×1080 bounds.
- **Caravan Selector Rail (Left Column, 320px):** Lists all known caravans with arrival status, duration timers, airlock readiness badges, and description.
- **Exchange Ledger (Middle Column, 2-Column Split):**
  - Left Pane: Player Offerings (shows inventory count, unit value, and `[-] [qty] [+]` adjustment controls).
  - Right Pane: Merchant Goods (shows stock count, unit value, and `[-] [qty] [+]` adjustment controls).
- **Arbitrator & Summary Rail (Right Column, 360px):**
  - Arbitrator Balance Scale: Visual balance indicator with merchant tolerance bar.
  - Valuation Delta: Value offered vs Value requested, displaying current balance and fairness status.
  - Gate Diagnostics: Real-time checks for airlock state, stock limits, and trade viability.
  - Action Controls: `Execute Trade` button (gated by validation) and `Reset All` button.
  - Feedback Strip: Diegetic feedback line logging executed trade consequences and journal entries.

---

## 3. Verification Matrix

| Verification Step | Command | Exit Code | Result |
|---|---|---|---|
| **Dotnet Tests** | `dotnet test Ashfall.Core.Tests` | `0` | **10,366 passed, 0 failed** |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | `0` | **CI gate PASS** |
| **Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | `0` | **PASS (0 findings across 299 catalogs)** |
| **Scene Binding** | `godot --headless --path . -- --scene-binding-selftest` | `0` | **25/25 passed, 0 failed** |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | `0` | **30 scenes checked, 0 errors** |
| **Contraband/Barter Selftest** | `godot --headless --path . -- --contraband-stash-selftest` | `0` | **CONTRABAND_STASH_SELFTEST PASS** |
| **Barter Panel Route Gate** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~ShelterBarterPanelRouteTests` | `0` | **3 passed, 0 failed** |
| **UI Suite Gate** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~UI.` | `0` | **81 passed, 0 failed** |
