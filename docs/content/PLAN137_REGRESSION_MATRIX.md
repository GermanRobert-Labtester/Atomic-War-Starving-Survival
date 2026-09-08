# Plan 137 — Regression Verification Matrix

## 1. Overview
This document records the verification suite and results for Plan 137 (Survivor Enrichment Activation: Beliefs, Professions, Keepsakes & Phantom Backgrounds).

---

## 2. Verification Gate Matrix

| Check / Gate | Command / Target | Status | Result / Detail |
|---|---|---|---|
| **Survivor Enrichment Unit Tests** | `dotnet test Ashfall.Core.Tests --filter "FullyQualifiedName~SurvivorEnrichment"` | **PASS** | 12/12 passed (0 failed). Covers overlays, merge precedence, formatting, null safety, keepsake isolation, and non-mutation of stats. |
| **Core Test Suite** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** | Full suite execution clean. |
| **Content Utilization Selftest** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** | `expansion_survivor_fields.json` and `expansion_item_tags.json` verified with real production consumers. |
| **Data Integrity Selftest** | `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 0 errors across all 411 catalogs and narrative files. |
| **Scene Binding Selftest** | `godot --headless --path . -- --scene-binding-selftest` | **PASS** | 22/22 scenes bound cleanly. |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS** | 30 production scenes checked; 0 errors, 0 warnings. |

---

## 3. Specific Test Scenarios Gated
1. `Catalog_Loads_All_72_Baseline_Survivors`: Validates full coverage and 0 missing baseline entries.
2. `Deep_Lore_Overlays_Loaded_Without_Duplicates`: Validates all 4 deep lore additions and duplicate rejection.
3. `Specialist_Overlay_Antigravity_Merges_Stance_And_Manifesto`: Verifies Tier 3 additive enrichment.
4. `Field_Level_Precedence_Preserves_Baseline_And_Adds_Specialist_Fields`: Confirms baseline immutability against conflicting specialist fields.
5. `SurvivorEnrichmentService_Formats_Clean_Strings_Without_Raw_Tokens`: Tests Title-Casing and underscore stripping.
6. `SurvivorEnrichmentService_Handles_Unenriched_Survivor_Gracefully`: Confirms zero null references for fallback cases.
7. `SurvivorEnrichmentService_Does_Not_Mutate_SurvivorDefinition`: Confirms pure projection and zero side-effects.
8. `ItemInspectionModel_Enriches_Keepsake_Candidate_And_Narrative_Tags`: Tests inventory inspection metadata decoration.
9. `Keepsake_Association_Does_Not_Imply_Inventory_Possession`: Verifies keepsakes are not automatically injected into player inventory.
10. `Belief_And_Profession_Do_Not_Mutate_Gameplay_Stats`: Confirms no combat, morale, or resource multipliers are applied.
