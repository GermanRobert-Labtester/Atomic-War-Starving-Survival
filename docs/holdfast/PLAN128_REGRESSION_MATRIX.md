# Plan 128 Regression Matrix

| Check / Gate | Target Area | Command | Expected Result | Actual Result | Status |
|---|---|---|---|---|---|
| C# Host Compilation | `Ashfall.csproj` | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings | 0 errors, 0 warnings | PASS |
| Core Unit Tests | `Ashfall.Core.Tests` | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 failed | 0 failed, 10,045 passed | PASS |
| Plan 128 Dedicated Tests | `HoldfastFlavorExpansionTests` | `dotnet test --filter FullyQualifiedName~HoldfastFlavorExpansionTests` | 7 passed, 0 failed | 7 passed, 0 failed | PASS |
| Data Integrity Gate | Data JSONs | `godot --headless --path . -- --data-integrity-selftest` | 0 findings, 298 catalogs | 0 findings, 298 catalogs | PASS |
| Holdfast Selftest | Holdfast Systems | `godot --headless --path . -- --holdfast-selftest` | 25/25 passed | 25/25 passed | PASS |
| Holdfast Runtime UI Test | Holdfast terminal | `godot --headless --path . -- --holdfast-runtime-uitest` | Browse 40 items and 9 factions; trade/save/reload pass | Passed after updating stale pre-expansion faction census | PASS |
| Content Utilization Gate | Manifest Verification | `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS | CI gate PASS | PASS |
| Scene Binding Gate | UI Scenes | `godot --headless --path . -- --scene-binding-selftest` | 25/25 passed | 25/25 passed | PASS |
| Scene Linter | TSCN files | `python3 scripts/ci/scene-lint.py` | 30 scenes, 0 errors | 30 scenes, 0 errors | PASS |
| Build Export Parity | Builds Linux Mirror | Diff between `Assets/.../holdfast_flavor.json` & `builds/linux/...` | 0 diff | Identical byte-for-byte | PASS |
