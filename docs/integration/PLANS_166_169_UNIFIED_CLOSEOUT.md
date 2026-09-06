# Plans 166–169 Unified Closeout

This implementation establishes the shared Core-first pillar:

```text
cataloged tech salvage → workshop research points/blueprint progress
→ authored crafting gate → faction espionage and fluid-logistics seams
→ structured procedural quest generation and shared quest read model
```

The four catalogs are registered with the content-utilization scanner and runtime collector. The three new host sessions are composed by `Main`, registered with campaign-day phases, enrolled in lifecycle reset, and captured into the campaign envelope. The plan’s existing authorities remain intact: Research/Workshop, FactionWar, WaterTreatment, Greenhouse, Disease, and mature quest systems are not replaced.

Verification completed in this pass:

- `dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore --nologo` — pass, 0 warnings.
- `dotnet build Ashfall.csproj --no-restore --nologo` — pass, 13 existing warnings.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore --nologo` — pass, 8,422 tests.
- `godot --headless --path . -- --data-integrity-selftest` — pass, 0 errors and 0 warnings across 269 catalogs.
- `godot --headless --path . -- --content-utilization-selftest` — pass; 0 orphaned catalogs and all four new catalogs recognized.
- Focused Plans 166, 167, 168, and 169 tests — pass, 24 tests total.

The fast gate runner remains red on pre-existing worktree hygiene and registry drift: trailing whitespace in `src/UI/WorkshopPanel.cs`, plus five older composite `Save*` wrappers not declared in `SaveSectionRegistry.cs`. Its embedded test/Godot subprocesses also cannot open their required IPC/socket under the restricted sandbox; the same full xUnit suite and direct Godot data/content selftests pass when run with their required execution mode.

The complete 50-step campaign scenario in the master plan is not claimed complete. Deferred items are the player-facing espionage/fluid/quest panels, typed downstream consequence adapters, automatic incident-context collection, full WaterTreatment-to-Fluid-to-Greenhouse/Disease delivery, and a dedicated continuous-vs-reload integration replay. The worktree already contained unrelated Plans 158–161 edits; they were preserved.
