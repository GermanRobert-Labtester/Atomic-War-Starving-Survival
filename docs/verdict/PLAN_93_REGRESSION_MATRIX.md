# Plan 93 — Regression & Test Matrix

> **Verification Suites:** xUnit (`Ashfall.Core.Tests`), Headless Godot Self-Tests, CI Gates

---

## 1. Automated Unit Test Matrix

| Test Suite / Method | Contract / Invariant Guarded | Result |
|---|---|---|
| `VerdictNpcExpansionTests.Catalog_Loads_All_18_Npc_Entries` | Verifies full catalog load (count = 18, count >= 15) | **PASS** |
| `VerdictNpcExpansionTests.All_18_Npc_Ids_Are_Unique_And_Prefixed` | Enforces unique `npc_*` prefix and zero collisions | **PASS** |
| `VerdictNpcExpansionTests.Original_6_Baseline_Npcs_Preserved` | Guards baseline 6 NPCs against alteration | **PASS** |
| `VerdictNpcExpansionTests.Plan18_Tribunal_Npcs_Preserved` | Guards Plan 18 defense/tribunal clerks (`tomas_reid`, `elena_vane`, `kasper_holt`) | **PASS** |
| `VerdictNpcExpansionTests.All_9_Plan93_Investigation_Npcs_Present` | Verifies presence, fields, phase bounds (1-3), dialogue counts (2-4) of 9 additions | **PASS** |
| `VerdictNpcExpansionTests.All_Npc_Kinds_Are_Supported` | Validates kind is member of `{"paper_ghost", "tape_echo", "living", "readings"}` | **PASS** |
| `VerdictNpcExpansionTests.All_Plan93_LocationIds_Map_To_Distinct_Verdict_Sites` | Verifies 1-to-1 site assignment across all 9 new Plan 82 investigation sites | **PASS** |
| `VerdictNpcExpansionTests.GetAvailable_Filters_By_Phase_And_Flag_And_Location` | Truth table test of phase gate, flag gate, and location matching | **PASS** |
| `VerdictNpcExpansionTests.Speak_Is_OneShot_And_Persists_In_State` | Proves one-shot speech and `VerdictNpcState.spokenNpcIds` state round-trip | **PASS** |
| `VerdictNpcExpansionTests.Availability_Is_Deterministic_Across_Invocations` | Verifies deterministic ordering and availability without RNG | **PASS** |
| `Plan18ExpansionDeepeningTests.QuestAndNpcContent_Parity` | Regression test for Plan 18 NPC and quest invariants | **PASS** |
| `VerdictTests.*` (all 133 tests) | Regression suite across the entire Verdict expansion system | **PASS** |

---

## 2. CI Self-Test Suite Matrix

| CI Command | Purpose | Result |
|---|---|---|
| `godot --headless --path . -- --data-integrity-selftest` | Verifies all definition and reference IDs across 208 catalogs | **PASS (0 errors, 0 warnings)** |
| `godot --headless --path . -- --content-utilization-selftest` | Verifies catalog gameplay consumption and utilization | **PASS (CI gate PASS)** |
| `godot --headless --path . -- --scene-binding-selftest` | Verifies Godot UI scene node bindings (22/22) | **PASS (22/22 passed)** |
| `python3 scripts/ci/scene-lint.py` | Verifies Godot scene file linting (27 scenes) | **PASS (0 errors)** |
| `dotnet build Ashfall.csproj` | Verifies host project compilation | **PASS (0 errors, 0 warnings)** |
| `dotnet test Ashfall.Core.Tests` | Full test suite execution across all systems | **PASS (7,013 passed, 0 failed)** |
