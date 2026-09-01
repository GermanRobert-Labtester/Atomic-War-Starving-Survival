# Expansion Regression Matrix & Test Gate Verification

## 1. Test Suite Verification Matrix

| Verification Tier | Command | Verification Scope | Result |
|---|---|---|---|
| **Dotnet Unit Suite** | `dotnet test Ashfall.Core.Tests` | 5,400+ xUnit tests incl. `Plan18ExpansionDeepeningTests` | **PASS (0 failed)** |
| **Expansion Depth CLI** | `godot --headless --path . -- --expansion-depth-selftest` | Holdfast (24), Standing Record (52/22), Crossing (20/14), Verdict (16/9), Master (437) | **PASS (0 failures)** |
| **Expansion Master Suite** | `godot --headless --path . -- --expansions-selftest` | Full 7-expansion verification suite | **PASS** |
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | 142 catalogs, 5,600+ IDs cross-referenced | **PASS (0 errors)** |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | 417 JSON catalogs, runtime consumption | **PASS (CI Gate)** |
| **Scene Binding Gate** | `godot --headless --path . -- --scene-binding-selftest` | 22 production scenes verified | **PASS (22/22)** |

## 2. Invariant Checklist

- [x] Zero engine coupling in `Assets/Ashfall.Core/` (`noEngineReferences: true`).
- [x] Single JSON data authority in `Assets/StreamingAssets/Data/`.
- [x] No duplicate expansion frameworks or duplicate host sessions created.
- [x] Deterministic simulation preserved via `ISeededRng` and standard clock ticks.
- [x] Save/load round-trips verified across all four expanded catalogs.
