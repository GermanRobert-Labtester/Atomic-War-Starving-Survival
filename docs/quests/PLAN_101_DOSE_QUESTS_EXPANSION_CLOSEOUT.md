# Plan 101 — Dose Quests Expansion: Closeout Report

**Document ID:** `docs/quests/PLAN_101_DOSE_QUESTS_EXPANSION_CLOSEOUT.md`
**Execution Scope:** Expand ASHFALL's dose-ledger narrative layer from 4 baseline questlines to a 12-quest campaign-spanning arc.
**Catalog Authority:** `Assets/StreamingAssets/Data/dose_quests.json`
**Core Classes:** `Assets/Ashfall.Core/DoseContentCatalog.cs`, `Assets/Ashfall.Core/DoseQuestMigration.cs`
**Test Suite:** `Ashfall.Core.Tests/DoseQuestExpansionTests.cs` (9/9 PASS)
**Status:** **COMPLETE & FULLY VERIFIED**

---

## 1. Executive Summary

Plan 101 establishes a 12-questline narrative arc spanning the full 360-day Year of Ash campaign. The dose ledger operates as a morally challenging bureaucratic institution where measurement, triage, equipment scarcity, consent, and historical accountability generate difficult leadership decisions.

All 12 questlines are fully authored, schema-compliant, DAG-verified with zero cycles, and mapped directly to existing items, NPCs (`Dr. Irina Vel`, `Sister Wyn Omah`, `Piet Abar`, `Saria Voss`), and the `DoseQuestMigration` allowlist.

---

## 2. Complete 12-Questline Roster

| # | Questline ID | Title | Min Day | Max Day | Stages | Focus & Seam |
|---:|:---|:---|:---:|:---:|:---:|:---|
| 1 | `quest_the_dose_the_first_reading` | The First Reading | 40 | 360 | 3 | Opening the ledger vs refusing measurement paperwork. |
| 2 | `quest_the_falsified_reading` | The Falsified Reading | 60 | 360 | 3 | Survey vs ledger discrepancy: true red line vs pencil mercy. |
| 3 | `quest_the_stolen_dosimeter` | The Stolen Dosimeter | 80 | 360 | 3 | Missing calibrated meter before boiler repair. |
| 4 | `quest_the_sick_of_room_seven` | The Sick of Room Seven | 90 | 360 | 4 | Triage of two Red-band patients with one morphine tray. |
| 5 | `quest_child_over_the_limit` | Child Over the Limit | 110 | 360 | 3 | Adolescent worker crossing into Amber band before lathe shift. |
| 6 | `quest_the_register_audit` | The Register Audit | 130 | 360 | 3 | Bench fatigue causing 15% under-read across past month. |
| 7 | `quest_the_childs_number` | The Child's Number | 150 | 360 | 4 | Newborn baseline recorded on chalkboard. |
| 8 | `quest_black_market_clean_bill` | Black-Market Clean Bill | 160 | 360 | 3 | Counterfeit clearance chits bypassing screening stations. |
| 9 | `quest_the_broken_calibration_chain` | The Broken Calibration Chain | 180 | 360 | 3 | Micro-fractured cesium source crystal in Piet's lab. |
| 10 | `quest_the_signed_hour` | The Signed Hour | 200 | 360 | 3 | Volunteer signature for hazardous reactor repair window. |
| 11 | `quest_exposure_for_the_essential_worker` | Exposure for Essential Worker | 210 | 360 | 3 | Chief power engineer facing Red-band override during blackout threat. |
| 12 | `quest_the_missing_page` | The Missing Page | 230 | 360 | 3 | Torn Sheet 04 showing founding families' early fallout exposures. |

---

## 3. Structural & Graph Verification

- **Schema Compliance:** Strict JSON schema version 1, root container `{"schema_version": 1, "quests": [...]}`.
- **Topology:** All 12 questlines form strictly acyclic directed graphs (DAGs) with 0 cycles.
- **Stage Progression:** Non-terminal stages contain 2–3 choices; all terminal stages have `isTerminal: true` and empty choice arrays.
- **ID Scoping:** All 12 `questlineId`s, 38 `stageId`s, and 27 `choiceId`s are globally unique and prefixed.
- **Transitions:** Every `nextStageId` resolves locally within the same questline definition; zero cross-questline jumps exist.

---

## 4. Item & Faction Validation

- **Item Resolution:** Every `grantItemId` (`item_dose_ledger`, `item_palliative_morphine`, `item_cohort_first_board`, `item_forged_clean_bill_chit`, `item_calibrated_dosimeter`, `item_dosimeter_tag`, `item_shielded_badge_case`, `item_calibration_key`, `item_chelation_decorporation_course`) exists and is defined in `Assets/StreamingAssets/Data/dose_items.json`.
- **Item Quantities:** All granted item quantities are positive integers (`1` or `2`).
- **Faction Tags:** All 12 quests use `"factionTag": "none"` adhering to internal shelter administration scope.

---

## 5. Persistence & Save Compatibility

- **Single Ownership:** Dose quest progression is owned exclusively by `DoseLedgerSave.quests` (v2+).
- **Adoption from Legacy Saves:** `DoseQuestMigration.AdoptFromYearOfAsh` faithfully imports active, completed, and failed records for all 12 canonical questlines.
- **Idempotent Rewards:** Choice taking records choice history in `ActiveQuestlineRecord.choiceHistory`, preventing duplicate grants upon save/load.
- **Radiation Invariant:** Administrative classification changes (e.g. forged clean-bill chits) update administrative bands without modifying underlying physical radiation dose (`radState.RadiationDose` and `radState.LifetimeRadiationExposure` remain pristine).

---

## 6. Verification Matrix Results

```bash
# 1. Targeted Dose Quest Expansion Suite (9 tests)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter DoseQuestExpansionTests
# Result: Passed! 9 passed, 0 failed.

# 2. Dose Quest Ownership Suite (3 tests)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter DoseQuestOwnershipTests
# Result: Passed! 3 passed, 0 failed.

# 3. Master Questline Catalog Suite (4 tests)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter QuestlineMasterCatalogTests
# Result: Passed! 4 passed, 0 failed.

# 4. Plan 27 BodyMind Suite (6 tests)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Plan27BodyMindTests
# Result: Passed! 6 passed, 0 failed.

# 5. Whole Workspace Data Integrity
godot --headless --path . -- --data-integrity-selftest
# Result: DATA_INTEGRITY_SELFTEST PASS — 0 findings, 0 errors across 216 catalogs.

# 6. Host Compilation
dotnet build Ashfall.csproj
# Result: Build succeeded. 0 Warning(s), 0 Error(s).
```
