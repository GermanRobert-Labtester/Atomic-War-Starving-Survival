# Batch 9 — Unblock Oldest Partial Plans: Plans 137 + 140

**Package:** `UNBLOCK-OLDEST-BATCH9-PLANS`
**Claim:** `claim-unblock-oldest-batch9-plans-2026-09-23`
**Date:** 2026-09-23
**Status:** COMPLETE (see ledger entry in `INTEGRATION_PLANS.md`)
**Predecessor:** `UNBLOCK-OLDEST-BATCH8-PLANS` (Plans 135 + 136), which completed Plan 136 full host integration and verified Plan 135.

---

## 1. Scope and Selection

The user requested audit, unblocking, and full integration of the next batch of partial plans:
1. **Plan 137:** `Needs → Performance Cascade` (Host refs: 0 -> full host integration as pure domain read model projection over live survivor needs, CLI probe `--needs-performance-selftest`, and day owner lifecycle).
2. **Plan 140:** `Generational Legacy & Campaign Inheritance` (Host refs: 0 -> full host integration with save section `campaign_legacy_save.json`, CLI probe `--campaign-legacy-selftest`, starting context preparation, and day owner lifecycle).

---

## 2. Premise Audit — Current Evidence

| System / Asset | Current Evidence | Verdict |
|---|---|---|
| `NeedsPerformanceBridge.cs` | Exists in `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs`. Pure functional query bridge projecting survivor needs (hunger, thirst, fatigue, warmth, morale) into domain performance multipliers for combat accuracy/damage, work speed, and expedition stamina drain. Added `NeedsPerformanceCensus` with `TotalSurvivorsEvaluated`. | Core authority live |
| `NeedsPerformanceHostSession.cs` | Created in `src/Host/NeedsPerformanceHostSession.cs`. Host session inheriting `HostSessionBase`, evaluating live survivor needs states via `_main.Survivors.Find()`, computing census, and emitting `needs_performance_ticked`. | Integrated |
| `HostCli.NeedsPerformance.cs` | Created in `src/Host/HostCli.NeedsPerformance.cs`. Provides `--needs-performance-selftest` / `--needs-perf-selftest` with 12 validation checks. | Integrated |
| `Main.NeedsPerformance.cs` & Lifecycle | Created in `src/Main.NeedsPerformance.cs`. Wired to `Main.ExpandedShelterSystems.cs` (`SetupNeedsPerformance()`, `ResetNeedsPerformance()`), `Main.CampaignOwners.cs` (`NeedsPerformanceDayOwner` in phase 5), and `Main.Application.cs`. Registered in `SetupWithoutSaveAllowlist` in `MainTriadDriftGateTests.cs`. | Integrated |
| `legacy_traits.json` | Exists in `Assets/StreamingAssets/Data/legacy_traits.json`. Authored generational legacy traits with sources (`survivor`, `shelter`, `faction`, `ending`), effects, and evolution targets. | Valid authored data authority |
| `CampaignLegacySystem.cs` | Exists in `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs`. Pure domain logic for cross-campaign generational traits, shelter improvement carry-forward, faction memory, and New Game+ lineage continuity. Added `CampaignLegacyCensus` and `GetCensus()`. | Core authority live |
| `CampaignLegacyHostSession.cs` & `CampaignLegacySaveStore` | Created in `src/Host/CampaignLegacyHostSession.cs`. Host session inheriting `HostSessionBase`, managing archive lifecycle, with static `CampaignLegacySaveStore` delegating to `SaveStoreHub.Checksummed<CampaignLegacyState>`. Section `campaign_legacy`, file `campaign_legacy_save.json`. | Integrated |
| `HostCli.CampaignLegacy.cs` | Created in `src/Host/HostCli.CampaignLegacy.cs`. Provides `--campaign-legacy-selftest` / `--legacy-selftest` with 12 validation checks. | Integrated |
| `Main.CampaignLegacy.cs` & Lifecycle | Created in `src/Main.CampaignLegacy.cs`. Wired to `Main.ExpandedShelterSystems.cs` (`SetupCampaignLegacy()`, `SaveCampaignLegacy()`, `ResetCampaignLegacy()`), `Main.SaveOrchestrator.cs` (`RestoreAllSubsystemsFromDisk`, `SaveAll`), `Main.CampaignOwners.cs` (`CampaignLegacyDayOwner` in phase 5 with pre-day snapshot restore), and `Main.Application.cs`. | Integrated |
| Event Vocabulary | Internal heartbeat events `needs_performance_ticked` and `campaign_legacy_ticked` added to `DayEventVocabulary` as `SemanticKind.Heartbeat` and documented in `EVENT_SEMANTIC_PARITY_MATRIX.md`. | Integrated |
| Section Pin Count | `SaveSectionRegistry.cs` updated to include `campaign_legacy` (section #226). Pin count updated to 226 across corruption tests. | Verified |

---

## 3. Architecture & Boundary Rules

1. **One Authority per Concern (Rule 5):** `NeedsSystem` is the sole mutable authority for dweller biological needs. `NeedsPerformanceBridge` provides derived query multipliers without shadow state or duplicate save sections. `CampaignLegacySystem` is the sole authority for generational traits and campaign lineage.
2. **Core Stays Engine-Free (Rule 2):** `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs` and `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs` contain zero engine references (`Godot`, `UnityEngine`).
3. **Deterministic Persistence (Rule 4):** `CampaignLegacyState` carries `schema_version = 1`. Inheritance rolls draw deterministically from `_campaignDay.Rng`. Day advance uses seeded RNG streams.
4. **Fail-Closed Rollback:** `CampaignLegacyDayOwner` implements `IPreDaySnapshotRestore`, capturing and restoring snapshots on failed campaign day advance.
5. **No Shadow Stores:** `CampaignLegacySaveStore` routes through `SaveStoreHub.Checksummed<CampaignLegacyState>` writing to `campaign_legacy_save.json`. `NeedsPerformanceBridge` has zero save store per the allowlisted read model architecture.

---

## 4. Verification Evidence

- `Plan137NeedsPerformanceHostIntegrationTests.cs`: **4/4 PASS**
- `NeedsPerformanceBridgeTests.cs`: **7/7 PASS**
- `Plan140CampaignLegacyHostIntegrationTests.cs`: **6/6 PASS**
- `Plan140GenerationalLegacyIntegrationTests.cs`: **5/5 PASS**
- `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: **1358/1358 PASS** (section pin: 226)
- `MainTriadDriftGateTests.cs`: **7/7 PASS** (allowlist validated)
- `DayEventVocabularyTests.cs`: **8/8 PASS**
- `agent-fast-verify.py`: **10/10 PASS**
- `generate-architecture-map.py`: **226 subsystems mapped with 100% mechanical evidence**.
- `generate-save-store-matrix.py`: **228 save store classes**.
- `generate-selftest-manifest.py`: **153 tests cataloged, 151 headless compatible**.
- `generate-docs-index.py`: **4200 documents indexed**.
