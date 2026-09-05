# Plan 92 — Regression & Verification Matrix

> **Scope:** Verification of catalog loading, selector behavior, test gates, and engine self-tests.

---

## 1. Automated Test Coverage Matrix

| Test Suite / Command | Scope / Contract Guard | Expected Outcome | Actual Result |
|---|---|---|---|
| `FactionWarDialogueExpansionTests.Catalog_Loads_Exactly_40_Snippets` | Verifies total snippet count is exactly 40 | 40 entries | **PASS** |
| `FactionWarDialogueExpansionTests.All_Dialogue_IDs_Are_Unique` | Verifies zero ID collisions | 40 distinct IDs | **PASS** |
| `FactionWarDialogueExpansionTests.All_Dialogue_IDs_Have_Dlg_Prefix` | Enforces snake_case `dlg_` naming convention | All match `dlg_*` | **PASS** |
| `FactionWarDialogueExpansionTests.All_18_Baseline_Snippets_Preserved_With_Original_Keys_And_Bodies` | Protects baseline 18 snippets from accidental alteration | All 18 present & unchanged | **PASS** |
| `FactionWarDialogueExpansionTests.All_22_New_Snippets_Present` | Verifies all 22 requested additions load properly | All 22 present & valid | **PASS** |
| `FactionWarDialogueExpansionTests.All_Snippets_Have_NonEmpty_Fields_And_Valid_MinDay` | Enforces non-empty ID, locationId, tag, body, and minDay $\in [480, 605]$ | All non-empty & valid | **PASS** |
| `FactionWarDialogueExpansionTests.GetDialogueForLocation_Filters_Correctly_At_Day_Boundaries` | Verifies onset threshold (`minDay <= day`) boundary filtering | Excluded at day-1, present at day & day+50 | **PASS** |
| `FactionWarDialogueExpansionTests.GetDialogueForLocation_Wrong_Location_Returns_No_Snippets_For_That_Location` | Verifies strict spatial filtering (`locationId` matching) | Empty list for unknown location | **PASS** |
| `FactionWarDialogueExpansionTests.Faction_Context_Distribution_Satisfied` | Verifies exact context distribution across the 22 new additions | 5 Gar, 4 Exc, 4 Und, 3 Ind, 3 Fnd, 3 Civ | **PASS** |
| `FactionWarContentCatalogTests.GetDialogueForLocation_Filters_By_Location_And_Day` | Existing regression test | PASS | **PASS** |

---

## 2. CI Self-Test Suite Matrix

| CI Command | Purpose | Result |
|---|---|---|
| `godot --headless --path . -- --data-integrity-selftest` | Verifies all definition and reference IDs across 208 catalogs | **PASS (0 errors, 0 warnings)** |
| `godot --headless --path . -- --content-utilization-selftest` | Verifies catalog gameplay consumption and utilization | **PASS (CI gate PASS)** |
| `godot --headless --path . -- --scene-binding-selftest` | Verifies Godot UI scene node bindings (22/22) | **PASS (22/22 passed)** |
| `python3 scripts/ci/scene-lint.py` | Verifies Godot scene file linting (27 scenes) | **PASS (0 errors)** |
| `dotnet build Ashfall.csproj` | Verifies host project compilation | **PASS (0 errors, 0 warnings)** |
| `dotnet test Ashfall.Core.Tests` | Full test suite execution across all systems | **PASS (6,955 passed, 0 failed)** |
