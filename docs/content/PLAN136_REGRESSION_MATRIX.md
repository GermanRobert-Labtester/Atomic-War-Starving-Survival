# Plan 136 Regression Verification Matrix

## Mandatory Verification Gates

| # | Gate / Command | Threshold | Purpose |
|---|----------------|-----------|---------|
| 1 | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~ItemDescription` | 0 Failed | Validates loader, catalog, aliases, fallback, and inspection model |
| 2 | `dotnet test Ashfall.Core.Tests` | 0 Failed | Full Core test suite verification across all systems |
| 3 | `dotnet build Ashfall.csproj` | 0 Errors | Verifies Godot host compilation with updated UI panel |
| 4 | `godot --headless --path . -- --data-integrity-selftest` | 0 Errors | Verifies data integrity and schema compliance for all JSON catalogs |
| 5 | `godot --headless --path . -- --content-utilization-selftest` | PASS | Verifies content utilization scanner and baseline consistency |
| 6 | `godot --headless --path . -- --scene-binding-selftest` | 22/22 Passed | Verifies typed scene node bindings including InventoryDetailPanel |
| 7 | `python3 scripts/ci/scene-lint.py` | 0 Errors | Verifies scene and script linting |
