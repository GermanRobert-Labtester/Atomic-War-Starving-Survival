# Standing Record Faction Regression Safety & Gate Enforcement

## 1. Regression Risk Analysis

| Subsystem / Area | Risk Description | Mitigation & Guardrail |
|---|---|---|
| **Location Layout Tests** | `LocationLayoutSystemTests.cs` contained hardcoded `Assert.Single(factions)`. | Refactored to `Assert.NotEmpty(factions)` and targeted lookup of `faction_the_overlay`. |
| **Catalog Integrity Gate** | Non-registered prefix collisions in `wants` or `offers` (e.g. `crop_` prefix). | Renamed `crop_rotation_almanac` to `soil_rotation_almanac`. All 216 catalogs validated clean. |
| **Faction ID Collisions** | Risk of clashing with global faction namespace. | Full repository survey performed. Faction IDs follow strict `faction_the_*` expansion conventions. |
| **Save File Stability** | Default trust overwriting persisted player state on game reload. | Save contract verified; `StandingRecordSaveStore` persists mutable state independently. |
| **Build Integrity** | C# syntax or compilation regressions in Godot host or test suites. | Verified with `dotnet build Ashfall.csproj` and `dotnet test Ashfall.Core.Tests`. |

---

## 2. CI Gate Command Matrix

All five gates in the canonical ASHFALL verification matrix passed without errors:

```bash
# 1. Core Unit Tests & Save Round-trip Gate
dotnet test Ashfall.Core.Tests

# 2. Host Compilation Gate
dotnet build Ashfall.csproj

# 3. Data Integrity & ID Resolution Gate
godot --headless --path . -- --data-integrity-selftest

# 4. Content Utilization Scanner Gate
godot --headless --path . -- --content-utilization-selftest

# 5. Scene Architecture & Contract Lint Gate
godot --headless --path . -- --scene-binding-selftest
python3 scripts/ci/scene-lint.py
```
