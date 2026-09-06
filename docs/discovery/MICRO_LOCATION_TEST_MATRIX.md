# ASHFALL — Micro-Location Regression & Verification Matrix (Task F25)

## 1. Overview & Quality Standard

This document certifies the complete regression test matrix for the Micro-Location feature (Plan 49, Tasks F21–F25).
The micro-location subsystem provides ambient, narrative-rich, non-combat points of interest encountered during wasteland expeditions, featuring rewards, moral decisions, one-time depletion, journal clues, and location discoveries.

All tests operate under the canonical ASHFALL invariants:
- **Invariant 1 (Engine-agnostic Core)**: All simulation, state capture, and choice resolution logic reside exclusively in `Ashfall.Core` with zero engine dependencies.
- **Invariant 2 (Ports & Adapters)**: File I/O and serialization use `IFileIO` and `IJsonSerializer`.
- **Invariant 3 (Save Compatibility)**: Persisted narrative encounter state round-trips through checksummed JSON envelopes (`SaveStore<NarrativeEncounterState>` / `SaveStoreHub`).
- **Invariant 4 (Determinism)**: Encounter selection uses `ISeededRng` / `SeededRng` (xorshift64*); 1000-draw simulations reproduce identically across runs.
- **Invariant 5 (No Gameplay Logic in Hosts)**: Host (`ExpeditionPanel`, `ExpeditionHostSession`) handles only presentation, input focus, badging, and action dispatch.
- **Invariant 6 (Data Authority is JSON)**: `Assets/StreamingAssets/Data/micro_locations.json` is the single source of truth.

---

## 2. Test Suites Summary

| Test Suite | Class / Runner | Test Count | Status | Key Coverage |
|---|---|---|---|---|
| **UI Presentation** | `Main.UiTests.Expeditions.cs` (`--expedition-panel-uitest`) | 4 exemplars | **PASS** | Auto-wrapping (WordSmart, 198 chars), badge rendering (`Gain`, `Cost`, `[CODEX]`, `[MAP]`, `[ONE-TIME]`), keyboard navigation focus, German translation |
| **Localization Readiness** | `MicroLocationLocalizationTests.cs` | 4 tests | **PASS** | English baseline, German translations, fallback behavior, pseudo-localization expansion, gameplay ID/value invariance |
| **Export & Packaging Parity** | `MicroLocationExportParityTests.cs` | 4 tests | **PASS** | `export_presets.cfg` packaging filter (`*.json, *.csv`), schema version 1, 100% item referential integrity, ID disjointness |
| **Lifecycle Smoke Tests** | `MicroLocationLifecycleSmokeTests.cs` | 3 tests | **PASS** | 8-tick organic trace determinism, crashed-truck select-resolve-grant-deplete-save-restore lifecycle, duplicate resolution prevention |
| **Full Regression Matrix** | `MicroLocationRegressionMatrixTests.cs` | 5 tests | **PASS** | 28-entry structural schema validation, item/codex/location referential integrity, 1000-encounter deterministic simulation oracle, parameterized choice resolution oracle, standard encounters non-regression |

---

## 3. Detailed Test Specifications

### 3.1 UI Presentation & Interaction (`Main.UiTests.Expeditions.cs`)
- **Exemplars Tested**:
  1. `micro_roadside_memorial`: Moral choice, offerings, journal codex clue unlock badge `[CODEX]`.
  2. `micro_crashed_truck`: Cargo recovery (`Gain: Canned Food ×2`), cab inspection (guilt), one-time depletion badge `[ONE-TIME]`.
  3. `micro_observation_post`: Radio triangulation, cartography map discovery badge `[MAP]`.
  4. `micro_frozen_bus`: Maximum text length description (198 characters), WordSmart autowrap verification, no text truncation or clipping.
- **Interaction Contracts**:
  - Modal min width 480px, auto-wrapped body text.
  - Context header: `DISCOVERY · MICRO-LOCATION` (no faction badge for anonymous discoveries).
  - Choice buttons render formatted color badges (`Theme.Lethe` for gains, `Theme.LetheAmber`/`Theme.Critical` for costs, `Theme.Cyan` for journal/map).
  - First actionable button receives UI keyboard focus on opening.

### 3.2 Localization & Invariance (`MicroLocationLocalizationTests.cs`)
- **`English_AllKeys_ResolveWithNonEmptyStrings`**: All 135 keys across 28 micro-locations resolve to valid English text.
- **`German_Exemplars_ResolveOrFallback`**: German translations resolve for the 4 exemplars; non-translated keys fall back cleanly to English without missing string tags.
- **`PseudoLocalization_ExpandsLength_WithoutBreakingFormat`**: Validates expanded strings (e.g. +30% length in pseudo-loc) without delimiter breakage.
- **`GameplayInvariance_AcrossLocales`**: Proves that encounter IDs, weights, item IDs, quantities, and flags remain strictly invariant regardless of active UI locale.

### 3.3 Export Parity (`MicroLocationExportParityTests.cs`)
- **`ExportPresets_IncludeFilter_ContainsJsonAndCsv`**: Verifies both Linux/X11 and Windows Desktop export presets include `*.json` and `*.csv`.
- **`CatalogStructure_MatchesSchemaContract`**: Verifies `schema_version == 1`, collection ID, and all 28 entries loaded.
- **`ItemReferences_ResolveInItemCatalog`**: Proves that every item referenced in grants or costs exists in `Assets/StreamingAssets/Data/items.json`.
- **`EncounterIds_AreUniqueAndDisjointFromEncountersCatalog`**: Guarantees zero ID collision with `narrative_encounters.json`.

### 3.4 Lifecycle Smoke (`MicroLocationLifecycleSmokeTests.cs`)
- **`F24_TestA_EightTick_SeededOrganicSelectionTrace_ExecutesDeterministically`**:
  - 8 simulation ticks executed across paired runs with seed 42.
  - Traces of tick number, surfaced encounter ID, depletion snapshot, RNG draws, and resolved counts match 100% identically.
- **`F24_TestB_CrashedTruck_FullLifecycle_SelectResolveGrantDepleteSaveRestore`**:
  - Starts expedition to `loc_denial_cut_substation`.
  - Surfaces `micro_crashed_truck`.
  - Resolves choice `search_truck_cargo`.
  - Grants `+2 canned_food` into active sortie pack via `ExpeditionSystem.TryGrantLoot`.
  - Verifies depletion recorded in `NarrativeEncounterState.depletedEncounterIds`.
  - Saves narrative state to `{ State, Checksum }` JSON envelope and restores into a brand new `NarrativeEncounterSystem`.
  - Verifies restored session retains depletion, 100 Monte Carlo seeds never re-select `micro_crashed_truck`, and candidate queries across all stances exclude it.
- **`F24_TestC_DuplicateResolution_CannotReGrantOrReResolve`**:
  - Re-resolving an already decided surfaced encounter is rejected by `ExpeditionEncounterBridge`.

### 3.5 Regression Matrix & Parameterized Oracle (`MicroLocationRegressionMatrixTests.cs`)
- **`F25_01_AllMicroLocations_PassStructuralSchemaValidation`**:
  - All 28 entries meet: `id` starting with `micro_`, non-empty title/desc, valid category, positive base weight, non-negative multipliers, and >= 2 valid choices.
- **`F25_02_ReferentialIntegrity_Items_Locations_Codex`**:
  - Every `grantItemId` and `costItems` entry resolves in `ItemCatalog`.
  - Proves signed delta support (`grantItemQuantity != 0`).
- **`F25_03_Deterministic1000EncounterReplay_SimulationOracle`**:
  - 1000 sequential encounter selections with rotating stances, danger levels, and locations.
  - Replay with identical seed produces identical sequence with 0 deviations.
- **`F25_04_ParameterizedResolutionOracle_EveryChoiceAcrossAllMicroLocations`**:
  - Resolves every choice of all 28 micro-locations.
  - Verifies morale/guilt deltas, grant items/quantities, journal unlock keys, location discovery IDs, world flag IDs, and depletion flags match authored definitions.
- **`F25_05_StandardEncounters_NonRegressionVerification`**:
  - Confirms base encounters in `narrative_encounters.json` continue to load and resolve without interference from micro-locations.

---

## 4. Canonical Verification Commands

```bash
# 1. Core Unit Tests & Regression Matrix (0 failures required)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~MicroLocation"

# 2. Godot Headless UI Presentation Test (exit code 0 required)
godot --headless --path . -- --expedition-panel-uitest

# 3. Canonical ASHFALL CI Gate Matrix (all exit code 0 required)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --scene-binding-selftest
godot --headless --path . -- --expedition-selftest
python3 scripts/ci/scene-lint.py
```
