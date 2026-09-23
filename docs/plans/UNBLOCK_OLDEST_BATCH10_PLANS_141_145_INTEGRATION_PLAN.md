# ASHFALL Plan 141 & Plan 145 Full Host Integration Plan
**Batch 10: Research → Downstream Unlocks Bridge (Plan 141) & Unified Ending Resolution & Epilogue Personalization (Plan 145)**
**Authority:** `DEC-309`, `DEC-310` | **Status:** `SEALED` | **Date:** 2026-09-23

---

## 1. Executive Summary

This package permanently unblocks and executes full Godot host integration for the two oldest unblocked plans in Batch 10:
1. **Plan 141: Research → Downstream Unlocks Bridge (`ResearchUnlockBridge`)**
   - **Problem:** Core had a functional unlock evaluator (`ResearchUnlockBridge.cs`), but had 0 Godot host references. Research node completions in `ResearchSystem` were not mechanically connected to downstream consumers (`Inventory` breakthrough item grants, `CraftingSystem` workshop recipe unlocks, and `shelter` / `expedition` / `combat` / `medical` capability flags).
   - **Solution:** Integrated `ResearchUnlockHostSession`, `ResearchUnlockSaveStore` (`SaveStoreHub.Checksummed<ResearchUnlockState>`, registered section `research_unlock` #227, filename `research_unlock_save.json`), wired into `Main.ExpandedShelterSystems.cs`, `Main.ResearchUnlock.cs`, and `Main.CampaignOwners.cs` (Phase 5 `ResearchUnlockDayOwner` with pre-day snapshot restore). Connected to `ResearchPanel.cs` UI and added diagnostic CLI probe `--research-unlock-selftest` / `--research-unlocks-selftest`.
2. **Plan 145: Unified Ending Resolution & Epilogue Personalization (`UnifiedEndingResolver`)**
   - **Problem:** Core had a comprehensive epilogue evaluator (`UnifiedEndingResolver.cs`), but had 0 Godot host references. Endgame completion in `Main.Endgame.cs` (`OnCampaignSealed`) used a generic epilogue summary without synthesizing the personalized narrative chronicle or passing `legacyTraitsAwarded` into `CampaignLegacyState` (Plan 140).
   - **Solution:** Integrated `UnifiedEndingHostSession`, `UnifiedEndingSaveStore` (`SaveStoreHub.Checksummed<UnifiedEndingSaveState>`, registered section `unified_ending` #228, filename `unified_ending_save.json`), wired into `Main.ExpandedShelterSystems.cs`, `Main.UnifiedEnding.cs`, `Main.Endgame.cs`, and `Main.CampaignOwners.cs` (Phase 5 `UnifiedEndingDayOwner` with pre-day snapshot restore). Connected to `EpiloguePanel.cs` UI and added diagnostic CLI probe `--unified-ending-selftest` / `--epilogue-selftest`.

---

## 2. Invariant & Rule Compliance

- **Rule 1 (Godot Authoritative; Unity Retired):** Pure C# .NET 8 / Godot 4 architecture; zero legacy engine dependencies.
- **Rule 2 (Core Engine-Free):** `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs` and `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs` reference standard BCL only (`System`, `System.Collections.Generic`, `System.Linq`). Zero references to Godot or UnityEngine.
- **Rule 3 (JSON Data Authoritative):** Autonomously reads `Assets/StreamingAssets/Data/research_unlocks.json` and `Assets/StreamingAssets/Data/epilogue_personalization.json`. No hardcoded fallback registries.
- **Rule 4 (Determinism & Persistence):**
  - Section pin count updated to 228 in `SaveSectionRegistry.cs` and `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`.
  - Checksummed save stores (`ResearchUnlockSaveStore` and `UnifiedEndingSaveStore`) support corrupted payload detection, schema migrations, and clean empty fallbacks.
  - Phase 5 campaign day owners implement `IPreDaySnapshotRestore` to guard against mid-tick rollback inconsistencies.
- **Rule 5 (One Authority Per Concern):**
  - `ResearchUnlockBridge` is the single authority for translating completed research nodes into downstream unlocks. Items flow directly into canonical `Inventory`; recipes flow into `CraftingSystem`.
  - `UnifiedEndingResolver` is the single authority for synthesizing the overarching campaign epilogue, merging political, social, moral, and personal survivor fates, and awarding legacy traits directly into `CampaignLegacyState`.

---

## 3. Verification & Evidence

| Verification Target | Command / Path | Result |
|---|---|---|
| Core Project Build | `dotnet build Ashfall.Core/Ashfall.Core.csproj` | **PASS (0 warnings, 0 errors)** |
| Godot Host Build | `dotnet build Ashfall.csproj` | **PASS (0 warnings, 0 errors)** |
| Core Tests Build | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS (0 warnings, 0 errors)** |
| Plan 141 Core Tests | `scripts/run_test.sh Ashfall.Core.Tests/Research/Plan141ResearchUnlockBridgeIntegrationTests.cs` | **PASS (5/5)** |
| Plan 141 Host Tests | `scripts/run_test.sh Ashfall.Core.Tests/Research/Plan141ResearchUnlockHostIntegrationTests.cs` | **PASS (5/5)** |
| Plan 145 Core Tests | `scripts/run_test.sh Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingIntegrationTests.cs` | **PASS (7/7)** |
| Plan 145 Host Tests | `scripts/run_test.sh Ashfall.Core.Tests/Endgame/Plan145UnifiedEndingHostIntegrationTests.cs` | **PASS (5/5)** |
| Save Corruption Gate | `scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | **PASS (1370/1370)** |
| Architecture Map Gen | `python3 scripts/ci/generate-architecture-map.py` | **PASS (228/228 subsystems mapped with 100% evidence)** |
| Save Store Matrix | `python3 scripts/ci/generate-save-store-matrix.py` | **PASS (230 save stores registered)** |
| Selftest Manifest | `python3 scripts/ci/generate-selftest-manifest.py` | **PASS (155 tests cataloged, 153 headless)** |

---

## 4. Next Steps
Batch 11 promotion head:
- Plan 147: `Contraband Stash Discovery & Black-Market Barter`
- Plan 148: `Expedition Vehicle Recovery & Field Salvage Operations`
