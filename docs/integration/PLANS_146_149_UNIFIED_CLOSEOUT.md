# Plans 146–149: Unified Advanced Industrial & Expedition Systems Closeout Report

**Master Plan ID:** AF-146-149-FLAGSHIP
**Scope:** Plans 146, 147, 148, 149
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Completion Date:** 2026-09-05
**Verification Pipeline:** `dotnet build` + `dotnet test` + `godot --headless`

---

## 1. Executive Summary

Plans 146–149 constitute a major flagship expansion to ASHFALL’s late-game technological progression:
1. **Plan 146 (EB-PVD Thermal Barrier Coatings):** Provides the shelter with precision physical vapor deposition capabilities, producing high-temperature columnar ceramic coatings for advanced turbine blades, combustors, and heavy diesel engines.
2. **Plan 147 (Mine-Clearing Flail):** Provides an armored expedition vehicle module to mechanically breach and clear wasteland minefields, permanently upgrading corridor safety and cutting travel casualty rates.
3. **Plan 148 (Microfluidic Diagnostics):** Implements point-of-care rapid pathogen diagnostics via soft-lithography molded PDMS cartridges and multi-channel fluorescence detection across 8 canonical diseases.
4. **Plan 149 (Rail Grinding Corridors):** Rehabilitates corrugated wasteland rail corridors, reprofiling deformed tracks, reducing derailment risks, and raising safe transit speed limits.
5. **Phase E (Route Infrastructure System):** Unifies route clearance and railway track condition under a single authoritative Core system (`RouteInfrastructureSystem`), eliminating fragmented state ownership.

---

## 2. Core Invariants & Architecture Compliance

| Invariant | Status | Evidence |
|---|---|---|
| **Invariant 1: Zero Engine Coupling in Core** | PASS | All 5 Core systems (`RouteInfrastructureSystem`, `EbPvdCoatingEngine`, `MineClearingFlailEngine`, `MicrofluidicDiagnosticEngine`, `RailGrindingEngine`) reside in `Assets/Ashfall.Core/` with zero Godot or Unity imports. |
| **Invariant 2: Ports & Adapters** | PASS | Power queries routed via `PowerSupplyContext`; file I/O and serialization routed via `SaveStore<T>` / `IFileIO` / `IJsonSerializer`. |
| **Invariant 3: Save Compatibility & Persistence** | PASS | 5 new sections registered in `SaveSectionRegistry.cs`, stored with checksummed envelopes in `SaveStoreHub`, tested for round-trip fidelity. |
| **Invariant 4: Determinism** | PASS | All PRNG generation powered by `CoreSeededRng` / `ISeededRng`. Zero `System.Random` or `Guid.NewGuid()`. |
| **Invariant 5: No Gameplay Logic in Hosts** | PASS | Godot panels (`src/UI/`) and host sessions (`src/Host/`) are strictly presentation and lifecycle facades. |
| **Invariant 6: Data Authority is JSON** | PASS | 4 authoritative catalogs in `Assets/StreamingAssets/Data/` with `schema_version: 1`, registered in `ContentUtilizationScanner.cs`, and 20 new items in `items.json`. |

---

## 3. Deliverable Matrix

| Domain | Catalog JSON | Core Engine | Host Session / Save Store | UI Panel | Unit / Integration Tests |
|---|---|---|---|---|---|
| **Phase E** | — | `RouteInfrastructureSystem.cs` | `RouteInfrastructureSaveStore.cs` | Bound to Expeditions/Rail | `RouteInfrastructureSystemTests.cs` |
| **Plan 146** | `ebpvd_coating_catalog.json` | `EbPvdCoatingEngine.cs` | `EbPvdCoatingHostSession.cs` / `EbPvdCoatingSaveStore.cs` | `EbPvdCoatingPanel.cs` | `EbPvdCoatingEngineTests.cs` |
| **Plan 147** | `mine_flail_catalog.json` | `MineClearingFlailEngine.cs` | `MineClearingFlailHostSession.cs` / `MineClearingFlailSaveStore.cs` | `MineFlailPanel.cs` | `MineClearingFlailEngineTests.cs` |
| **Plan 148** | `microfluidic_diagnostic_catalog.json` | `MicrofluidicDiagnosticEngine.cs` | `MicrofluidicDiagnosticHostSession.cs` / `MicrofluidicDiagnosticSaveStore.cs` | `MicrofluidicDiagnosticPanel.cs` | `MicrofluidicDiagnosticEngineTests.cs` |
| **Plan 149** | `rail_grinding_catalog.json` | `RailGrindingEngine.cs` | `RailGrindingHostSession.cs` / `RailGrindingSaveStore.cs` | `RailGrindingPanel.cs` | `RailGrindingEngineTests.cs` |
| **Cross-Plan** | `items.json` (20 items) | `AdvancedMachineContracts.cs` | `Main.Plans146_149.cs` | All 4 panels in `Main.UiPanels.cs` | `Plans146_149IntegrationTests.cs` |

---

## 3a. Cross-Tool Review & Repair Pass (2026-09-06)

A second-tool review against the master plan found the initial implementation
individually clean (determinism, save DTO hygiene, zero engine coupling,
correct one-authority mutations) but non-functional as shipped. All findings
below were repaired and pinned by new tests (52 filtered tests, up from 26):

| # | Defect | Repair |
|---|---|---|
| A1 | `TickPlans146To149` was an empty stub with zero call sites — no job ever progressed | Implemented (8h machine shift/day, power projection from `PowerGridSystem.NetWatts`, clinical truth predicate from `DiseaseSystem.IsInfected`) and wired into `TickAllExpandedShelterSystems` |
| A2 | The 4 JSON catalogs were never loaded; definitions hardcoded in C# and drifted from authored values | Added 4 catalog loaders (`EbPvdCoatingCatalogLoader`, `MicrofluidicDiagnosticCatalogLoader`, `MineFlailCatalogLoader`, `RailGrindingCatalogLoader`); host registers catalog defs over engine seeds; failure profiles now catalog-driven; parity tests pin the correspondence |
| A3 | Failed job starts consumed items (ceramic target lost when bond coat missing) | Start paths now take an all-or-nothing `IReadOnlyList<InventoryDemand>` bulk consume; failure consumes nothing (tested) |
| A4 | Microfluidic runs progressed at 30% speed while unpowered | Power gate mirrors EB-PVD: ≥0.8 brownout or <50% nominal pauses all processes; power return auto-resumes (tested) |
| A5 | Confidence bands were branch-specific — a Positive ≥0.85 provably true, ≤0.75 provably false | Single confidence distribution shared by all outcome branches, parameterized only by sample quality (overlap test) |
| A6 | Sparse minefields (density < 0.02) threw `ArgumentException` in the residual clamp | Floor clamped to `min(floor, baseDensity)`; residual floor authored per flail def (`ResidualRiskFloor`) (tested) |
| A7 | Rail speed upgrade ignored the route design limit | `RegisterRailSegment` records `DesignSpeedLimitKph`; speed ceiling = min(head cap, design limit) (tested) |
| A8 | `StartJob` silently destroyed a Paused job's progress/materials | Paused jobs reject new starts (`job_paused_resume_or_fail`) until resumed or failed (tested) |
| A9 | Sensitivity/specificity used raw (unclamped) in resolution math | Clamped at `RegisterAssay` (tested) |
| A10 | Outputs never reached live authorities | Completion now mints authored result items into inventory (EB-PVD substrate-class map, assay cartridge); route projections remain query APIs (`GetHazardModifier`/`GetTravelModifier` — consumer wiring into `ExpeditionSystem` travel estimate is still open) |
| A11 | Flail maintenance hardcoded chain capacity (40) and hydraulic nominal (180 bar) regardless of module | Maintenance reads the active module def (light scout: 24 links, 140 bar) (tested) |
| A12 | Null clinical-truth predicate crashed resolution | Null predicate yields an Indeterminate run with no evidence recorded (tested) |
| A13 | Rail registration on a pre-existing road segment left `Mode = "road"` | `RegisterRailSegment` upgrades the mode explicitly (tested) |

RNG streams: `advanced_mfg_ebpvd_coating`, `medical_microfluidic_diagnostics`,
`route_engineering_mine_flail`, `route_engineering_rail_grinding` (distinct
StableHash-derived strings, snake_case per `CampaignRngSourceGateTests`).

**Still open (out of scope here, tracked by the UI audit register):** the four
panels declare `OnActionRequested` but emit nothing beyond OPEN — no normal
player route to start jobs (UI-14 acceptance criteria apply).

---

## 4. Verification CLI Commands

- Unit & Integration Tests: `dotnet test Ashfall.Core.Tests --filter "EbPvdCoating|MicrofluidicDiagnostic|MineClearingFlail|RailGrinding|RouteInfrastructure|Plans146_149"`
- Godot Headless UI Tests:
  - `godot --headless --path . -- --ebpvd-coating-uitest`
  - `godot --headless --path . -- --microfluidic-diagnostic-uitest`
  - `godot --headless --path . -- --mine-flail-uitest`
  - `godot --headless --path . -- --rail-grinding-uitest`
- Full CI Matrix:
  - `godot --headless --path . -- --data-integrity-selftest`
  - `godot --headless --path . -- --content-utilization-selftest`
  - `godot --headless --path . -- --scene-binding-selftest`
  - `python3 scripts/ci/scene-lint.py`
  - `dotnet test Ashfall.Core.Tests`
