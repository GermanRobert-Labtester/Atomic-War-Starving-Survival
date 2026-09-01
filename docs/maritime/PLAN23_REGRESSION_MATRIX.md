# Plan 23 Regression Matrix

| # | Gate | Command | Status |
|---|---|---|---|
| 1 | Core/tests build | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 errors, 0 warnings |
| 2 | Full xUnit suite | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 5,819 tests |
| 3 | Godot host build | `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| 4 | Catalog integrity | `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 153 catalogs |
| 5 | Maritime/Flotilla selftest | `godot --headless --path . -- --maritime-selftest` | PASS |
| 6 | Deep-coast selftest | `godot --headless --path . -- --deep-coast-selftest` | PASS (72/72) |
| 7 | Plan 23 unit tests | `dotnet test --filter Plan23` | PASS — 44 tests (14 faction depth + 11 dive mechanics + 9 coastal + 6 cross-layer + 7 long-campaign, minus overlap) |
| 8 | Radio corpus/tone lint | `dotnet test --filter FactionRadioCorpus` | PASS (6) |
| 9 | Deterministic loot/safe/tide/surge tests | in Plan23* suites | PASS |
| 10 | Old-save compatibility | `OldSaves_*` / `Surge_OldSaves_*` / `FlotillaAdditionsRequireNoFabricatedState` | PASS |

Scopes excluded from "my green" at commit time: tests belonging to parallel in-flight
workstreams (Plan 27/29/60 batches) sharing this tree — each verified green at final
full-run; any residual failure there belongs to that workstream, not Plan 23.
