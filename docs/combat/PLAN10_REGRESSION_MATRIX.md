# Plan 10 Regression & Verification Matrix

**Document:** `docs/combat/PLAN10_REGRESSION_MATRIX.md`
**Status:** All Gates Verified Green

---

## 1. Regression Test Suite

| Test Area | Verification Suite | Exit Code | Results Summary |
|---|---|---|---|
| **Combat Catalog Contracts** | `dotnet test Ashfall.Core.Tests --filter Plan10CatalogCoverageTests` | 0 | 100% PASS (Combatants, Doctrines, Vehicles, Dives, Recipes) |
| **Plan 10 Remediation & DTOs** | `dotnet test Ashfall.Core.Tests --filter Plan10RemediationTests` | 0 | 100% PASS (Ballistics, Jams, Dive Keeper Flags) |
| **Tactical Combat Simulation** | `dotnet test Ashfall.Core.Tests --filter TacticalCombatSystemTests` | 0 | 100% PASS (5-lane movement, stances, damage, persistence) |
| **Warlord Doctrine Execution** | `dotnet test Ashfall.Core.Tests --filter WarlordDoctrineSystemTests` | 0 | 100% PASS (8 doctrines, transition signals, action weights) |
| **Expedition Vehicle Logistics** | `dotnet test Ashfall.Core.Tests --filter ExpeditionVehicleTests` | 0 | 100% PASS (8 chassis, fuel math, breakdown, garage) |
| **Maritime Dive System** | `dotnet test Ashfall.Core.Tests --filter MaritimeDiveSystemTests` | 0 | 100% PASS (12 sites, oxygen depletion, noise, hazards) |
| **Full Core Unit Test Suite** | `dotnet test Ashfall.Core.Tests` | 0 | 5,317 passed, 0 failed |
| **Data Integrity Self-Test** | `godot --headless --path . -- --data-integrity-selftest` | 0 | 0 errors across 138 catalogs (5,563 IDs) |
| **Content Utilization Self-Test**| `godot --headless --path . -- --content-utilization-selftest` | 0 | CI Gate PASS (413 catalogs) |
| **Scene Binding Self-Test** | `godot --headless --path . -- --scene-binding-selftest` | 0 | 22/22 scenes bound |
| **Scene Tree Linter** | `python3 scripts/ci/scene-lint.py` | 0 | 26 scenes checked, 0 errors |
| **Audio Catalog Sync** | `python3 scripts/ci/generate-audio-catalog.py --check` | 0 | 74 cues in sync |
