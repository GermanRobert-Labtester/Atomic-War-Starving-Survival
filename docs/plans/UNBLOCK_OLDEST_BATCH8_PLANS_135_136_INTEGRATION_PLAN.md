# Batch 8 — Unblock Oldest Partial Plans: Plans 135 + 136

**Package:** `UNBLOCK-OLDEST-BATCH8-PLANS`
**Claim:** `claim-unblock-oldest-batch8-plans-2026-09-23`
**Date:** 2026-09-23
**Status:** COMPLETE (see ledger entry in `INTEGRATION_PLANS.md`)
**Predecessor:** `UNBLOCK-OLDEST-BATCH7-PLANS` (Plans 59 + 134), which completed Plan 134 full host integration and verified Plan 59.

---

## 1. Scope and Selection

The user requested audit, unblocking, and full integration of the next batch of partial plans:
1. **Plan 135:** `Weather → Deep Gameplay Cascade` (previously listed in Batch 6; verified full host integration, campaign day advance, fail-closed rollback, and save/load persistence).
2. **Plan 136:** `Wildlife Trapping → Food Pipeline & Cooking` (host refs: 0 -> full host integration with save section, CLI probe, lifecycle wiring, and catalog validation).

---

## 2. Premise Audit — Current Evidence

| System / Asset | Current Evidence | Verdict |
|---|---|---|
| `recipes_cooking.json` | Exists in `Assets/StreamingAssets/Data/recipes_cooking.json`. Authored recipes with inputs, outputs, cook times, and required stations. | Valid authored data authority |
| `CookingRecipeCatalogLoader.cs` | Created in `Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs`. Strict JSON validation, unique recipe IDs, positive quantities and cook times. | Pure domain catalog loader live |
| `CookingSystem.cs` | Exists in `Assets/Ashfall.Core/Cooking/CookingSystem.cs`. Pure domain logic for recipe queues, cooking station progress, ingredient consumption, and batch completion. | Core authority live |
| `CookingCensus` & `GetCensus()` | Implemented on `CookingSystem` as `public struct CookingCensus` matching CI architecture scanner requirements. | Census authority live |
| `CookingHostSession.cs` & `CookingSaveStore` | Created in `src/Host/CookingHostSession.cs`. Host session inherits `HostSessionBase`, exposes recipe binds and queue ticks, with `CookingSaveStore` using `SaveStoreHub.Checksummed<CookingState>`. Section `cooking`, file `cooking_save.json`. | Integrated |
| `HostCli.Cooking.cs` | Created in `src/Host/HostCli.Cooking.cs`. Provides `--cooking-selftest` / `--cooking-test` with 12 validation checks. | Integrated |
| `Main.Cooking.cs` & Lifecycle | Created in `src/Main.Cooking.cs`. Wired to `Main.ExpandedShelterSystems.cs` (`SetupCooking()`, `SaveCooking()`), `Main.SaveOrchestrator.cs` (`RestoreAllSubsystemsFromDisk`, `SaveAll`), `Main.CampaignOwners.cs` (`CookingDayOwner` in phase 5 with pre-day snapshot restore), and `Main.Application.cs`. | Integrated |
| Event Vocabulary | Internal heartbeat event `cooking_ticked` added to `DayEventVocabulary` as `SemanticKind.Heartbeat` and documented in `EVENT_SEMANTIC_PARITY_MATRIX.md`. | Integrated |
| Section Pin Count | `SaveSectionRegistry.cs` updated to include `cooking` (section #225). Pin count updated to 225 across corruption tests. | Verified |

---

## 3. Architecture & Boundary Rules

1. **One Authority per Concern (Rule 5):** `CookingSystem` and `WildlifeTrappingSystem` form the canonical pipeline for raw meat, forage, and harvested crops conversion into cooked meals and caloric rations. Needs and hunger systems consume completed meals without shadow nutrition stores or parallel inventory ledgers.
2. **Core Stays Engine-Free (Rule 2):** `Assets/Ashfall.Core/Cooking/CookingRecipeCatalogLoader.cs` and `CookingSystem.cs` contain zero engine references (`Godot`, `UnityEngine`).
3. **Deterministic Persistence (Rule 4):** `CookingState` carries `schema_version = 1`. Ingredients and stations update deterministically per tick. Day advance draws deterministically from `_campaignDay.Rng`.
4. **Fail-Closed Rollback:** `CookingDayOwner` implements `IPreDaySnapshotRestore`, capturing and restoring snapshots on failed campaign day advance.
5. **No Shadow Stores:** All save operations route through `SaveStoreHub.Checksummed<CookingState>` writing to `cooking_save.json`.

---

## 4. Verification Evidence

- `Plan135WeatherCascadeHostIntegrationTests.cs`: **16/16 PASS**
- `Plan135WeatherCascadeIntegrationTests.cs`: **5/5 PASS**
- `Plan136CookingHostIntegrationTests.cs`: **8/8 PASS**
- `Plan136WildlifeCookingIntegrationTests.cs`: **5/5 PASS**
- `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: **1352/1352 PASS**
- `DayEventVocabularyTests.cs`: **8/8 PASS**
- `generate-architecture-map.py`: **225 subsystems mapped with 100% mechanical evidence**.
- `generate-save-store-matrix.py`: **227 save store classes**.
- `generate-selftest-manifest.py`: **151 tests cataloged**.
