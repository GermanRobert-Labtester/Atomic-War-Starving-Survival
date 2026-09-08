# Plan 120 Regression Matrix

## 1. Automated Verification Gates

| Test Suite / Verification Target | Command | Expected State | Verified Result |
|---|---|---|---|
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | 0 errors, 0 warnings | Clean (30 scenes checked, 0 errors) |
| **Godot Host Build** | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings | Clean (0 warnings, 0 errors) |
| **Crossing Self-Test** | `godot --headless --path . -- --crossing-selftest` | 40/40 checks pass | PASS 40/40 |
| **Data Integrity Self-Test** | `godot --headless --path . -- --data-integrity-selftest` | 0 errors across all catalogs | PASS (0 findings across 298 catalogs) |
| **Content Utilization Self-Test** | `godot --headless --path . -- --content-utilization-selftest` | CI Content Utilization Gate PASS | PASS (CI gate: PASS) |
| **Scene Binding Self-Test** | `godot --headless --path . -- --scene-binding-selftest` | All scenes pass | PASS (25/25 passed) |
| **Targeted Unit Tests** | `dotnet test --filter CrossingFactionExpansionTests` | 16 passed, 0 failed | PASS (16/16 passed) |
| **Expansion Integration Tests** | `dotnet test --filter ExpansionsIntegrationTests` | 10 passed, 0 failed | PASS (10/10 passed) |
| **Full Unit Test Suite** | `dotnet test Ashfall.Core.Tests` | 0 failures | PASS (10019/10019 passed) |

---

## 2. Parity & Compatibility Checklist

- [x] Baseline factions (The Scale, The Underwrite, The Compact) unchanged in positions 0..2.
- [x] `crossing_factions.json` matches in both `Assets/StreamingAssets/Data/` and `builds/linux/Assets/StreamingAssets/Data/`.
- [x] `CrossingIds` constants added for all 8 factions without renaming or deleting existing constants.
- [x] `CrossingHeadlessDemo.cs` updated to check `>= 8` factions.
- [x] Mature save compatibility preserved: existing saves reference stable IDs (`faction_the_scale`, `faction_the_underwrite`, `faction_the_compact`) while new factions initialize deterministically.
