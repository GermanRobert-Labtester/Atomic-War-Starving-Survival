# UNBLOCK C3 PLANS 174 & 175 — FULL HOST INTEGRATION PLAN

**Role:** Integrator (user-authorized 2026-09-23)
**Claim:** `claim-unblock-c3-plans-174-175-2026-09-23`
**Targets:**
- **Plan 174:** Procedural Survivor Backstories & Origin Mechanics (`BackstorySystem` in Core)
- **Plan 175:** Meta Profile & Cross-Run Profile Store (`CrossRunProfileStore` & `MetaProgressionSystem` in Core)

---

## 1. Context & Motivation
In previous roadmap sweeps, C3 Plan 174 and C3 Plan 175 were held under:
- C3 Plan 174 HOLD: "Mechanical-origin seam decision; BackstorySystem exists in Core with 0 host refs"
- C3 Plan 175 HOLD: "Cross-run profile-store / NG+ product owner; CrossRunProfileStore exists, no MetaProgressionSystem"

The user explicitly requested:
"Yes fully integrate and scaffold any blockades in the way and fully resolve and initiate full integration with verification and making sure it doesn't end up a partial from a partially integrated plan once previously and again! C3 Plan 174 HOLD ... C3 Plan 175 HOLD"

This package unblocks both plans permanently, promoting them from 0 host references to 100% full host integration with save persistence, campaign day owners, downstream consumers, CLI diagnostic self-tests, and regression verification.

---

## 2. Architecture & Seams

### Plan 174: Procedural Survivor Backstories
- **Data Catalog:** `Assets/StreamingAssets/Data/backstory_templates.json` (10 occupations, 6 life experiences, 4 backstory templates).
- **Core Domain:** `Assets/Ashfall.Core/Survivors/BackstorySystem.cs`
  - Add `BackstoryCensus` struct and `GetCensus()`.
  - Expose accessors `GetAllAssignedBackstories()`.
- **Save Section:** `backstory` (#231, `backstory_save.json`) in `SaveSectionRegistry.cs`.
- **Day Event Vocabulary:** `backstory_ticked` classified as InternalHeartbeat.
- **Host Session:** `src/Host/BackstoryHostSession.cs`
  - `BackstorySaveStore` (`SaveStoreHub.Checksummed<BackstoryState>`).
  - Integration with `SurvivorsHostSession` and `SurvivorDetailPanel.cs`.
- **Campaign Day Owner:** `BackstoryDayOwner` in `src/Main.CampaignOwners.cs` (Phase 5, `IPreDaySnapshotRestore`).
- **Host Lifecycle:** `src/Main.Backstory.cs`, `src/Main.SaveOrchestrator.cs`.
- **CLI Diagnostic Probe:** `--backstory-selftest` in `src/Host/HostCli.Backstory.cs` (12 checks).

### Plan 175: Meta Progression & Cross-Run Profile Store
- **Data Catalog:** `Assets/StreamingAssets/Data/meta_unlockables.json` (schema_version 1, unlockable profile crests, insignias, and NG+ starting options).
- **Core Domain:**
  - `Assets/Ashfall.Core/Endgame/CrossRunProfileStore.cs` (user-level run history aggregator).
  - `Assets/Ashfall.Core/Endgame/MetaProgressionSystem.cs` (meta progression authority, prestige score calculation, achievement integration, unlockable resolver, census).
- **Save Section:** `meta_progression` (#232, `meta_progression_save.json`) in `SaveSectionRegistry.cs`.
- **Day Event Vocabulary:** `meta_progression_ticked` classified as InternalHeartbeat.
- **Host Session:** `src/Host/MetaProgressionHostSession.cs`
  - `MetaProgressionSaveStore` (`SaveStoreHub.Checksummed<MetaProgressionSaveState>`).
  - Integration with `OnCampaignSealed` in `src/Main.Endgame.cs` and `AchievementSystem`.
- **Campaign Day Owner:** `MetaProgressionDayOwner` in `src/Main.CampaignOwners.cs` (Phase 5, `IPreDaySnapshotRestore`).
- **Host Lifecycle:** `src/Main.MetaProgression.cs`, `src/Main.SaveOrchestrator.cs`.
- **CLI Diagnostic Probe:** `--meta-progression-selftest` in `src/Host/HostCli.MetaProgression.cs` (12 checks).

---

## 3. Implementation Plan
1. Author `Assets/StreamingAssets/Data/meta_unlockables.json`.
2. Extend `Assets/Ashfall.Core/Survivors/BackstorySystem.cs` with `BackstoryCensus` and `GetCensus()`.
3. Create `Assets/Ashfall.Core/Endgame/MetaProgressionSystem.cs` with `MetaProgressionCensus`, `GetCensus()`, and catalog loading.
4. Register save sections `backstory` (#231) and `meta_progression` (#232) in `SaveSectionRegistry.cs`.
5. Update `DayEventVocabulary.cs` and `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`.
6. Register CLI actions in `HostCliRegistry.cs` and `HostCli.cs`.
7. Implement host sessions:
   - `src/Host/BackstoryHostSession.cs`
   - `src/Host/MetaProgressionHostSession.cs`
8. Implement host CLI probes:
   - `src/Host/HostCli.Backstory.cs`
   - `src/Host/HostCli.MetaProgression.cs`
9. Implement Main lifecycle hooks:
   - `src/Main.Backstory.cs`
   - `src/Main.MetaProgression.cs`
   - `src/Main.SaveOrchestrator.cs`
   - `src/Main.CampaignOwners.cs`
   - `src/Main.Application.cs`
   - Wire `OnCampaignSealed` in `src/Main.Endgame.cs` to ingest run facts into `MetaProgressionHostSession`.
   - Wire `SurvivorDetailPanel.cs` to display backstory when available.
10. Write focused integration tests:
   - `Ashfall.Core.Tests/Survivors/Plan174BackstoryHostIntegrationTests.cs`
   - `Ashfall.Core.Tests/Endgame/Plan175MetaProgressionHostIntegrationTests.cs`
11. Update `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` pin to 232.
12. Run generators:
   - `generate-architecture-map.py`
   - `generate-save-store-matrix.py`
   - `generate-selftest-manifest.py`
   - `generate-docs-index.py`
13. Ratify decisions in `DECISION_REGISTER.md` (lifting C3 HOLDs 174 & 175) and update `KNOWN_DEBT.md`, `WORKTREE_OWNERSHIP.md`, and `INTEGRATION_PLANS.md`.
