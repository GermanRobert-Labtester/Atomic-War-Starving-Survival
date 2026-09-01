# Plan 43 Regression Matrix

## 1. Automated Verification Gates

| Gate / Command | Expected Output | Status |
|---|---|---|
| `dotnet build Ashfall.csproj` | 0 errors | PASS |
| `dotnet test Ashfall.Core.Tests --filter "FullyQualifiedName~SettlementCatalog"` | 8/8 passed | PASS |
| `godot --headless --path . -- --data-integrity-selftest` | 0 errors | PASS |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS | PASS |
| `godot --headless --path . -- --scene-binding-selftest` | 22/22 passed | PASS |
| `python3 scripts/ci/scene-lint.py` | 0 errors | PASS |

## 2. Cross-System Compatibility Checks
- `locations.json`: 121 locations total, all 12 settlement locations resolve.
- `caravans.json`: 4 active caravan routes correctly contain settlement locations.
- `expeditions.json`: 3 settlement locations configured as friendly trading destinations.
- `items.json`: All exported and imported items resolve without broken references.
- `factions`: All settlement allegiances resolve to valid factions.
