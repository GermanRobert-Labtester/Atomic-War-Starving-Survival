# ASHFALL dual-engine plan (Phase 0)

Unity 6 LTS is the **primary** authoring/shipping engine. Godot 4.7 .NET is the **secondary** runtime so the simulation is runnable and testable without Unity.

This file is the Phase 0 spike record. Simulation lives in `Ashfall.Core` (plain C#, netstandard2.1). Hosts are thin.

## Layout

| Path | Role |
|---|---|
| `Ashfall.Core/` | Engine-agnostic classlib. Zero `UnityEngine` / `Godot` usings. |
| `Ashfall.Core.Tests/` | xUnit suite (`dotnet test`). |
| `Ashfall.csproj` + `project.godot` | Existing Godot 4.7 .NET host at repo root (not under `Assets/`). |
| `src/` | Thin Godot Nodes / Control UI. |
| `Assets/StreamingAssets/Data/` | JSON authority. Not copied. Both engines read these files. |

`Assets/.gdignore` keeps Godot from importing the Unity tree. Catalogs load via filesystem (`CatalogLocator` / `IFileIO`).

Unity cache dirs are **not** in git; create skip files once so Godot does not scan `Library/`:

```bash
touch Library/.gdignore Packages/.gdignore Logs/.gdignore UserSettings/.gdignore
```

## Ports (in `Ashfall.Core`)

`IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng`.

Default adapters: `SystemTextJsonSerializer`, `FileSystemIO`, `SimClock`, `SeededRng`, `ConsoleLog`. Godot adds `GodotLog`. Unity `JsonUtility` is banned from core.

## Phase 0 slice (what actually runs)

`IceRoadSystem` — Holdfast seasonal gate. Daily tick advances ice thickness from weather, opens an 11–20 day freeze window after unlock + clerk, closes on length / dark beacon / fallout.

`HoldfastCatalogLoader` — `holdfast_locations.json` + `holdfast_quests.json`.

## How to run (Godot, not Unity)

Godot 4.7.1 .NET (`godot --version` → `4.7.1.stable.mono`).

```bash
# Core library + tests (no Godot required). Tests target net9 — this machine has no net8 runtime.
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Phase 0 result: Passed 12 / Failed 0

# Godot compile (Godot 4.7.1.stable.mono)
dotnet build Ashfall.csproj

# Headless ice-road + catalog smoke (exit 0 = PASS)
godot --headless --path . -- --ice-road-selftest
# Phase 0 result: IceRoadHeadlessDemo PASS 21/21 locations=35 quests=10

# Minimal Control UI (tick-day button)
godot --path .
```

Do **not** launch Unity Hub/editor/batchmode on this machine (memory-starved).

## Still Unity-only

GameBootstrap (~80 partials), expeditions, needs/shelter, Utility AI, UI Toolkit HUD, ScriptableObject importers, CensusClaim / Waystation / BrineWater / Holdfast quests as MonoBehaviour-wired systems. Unity `IceRoadSystem.cs` is temporarily still the Unity copy; Phase 1 should make it a wrapper over `Ashfall.Core`.

## Phase 1 (next)

Point Unity `IceRoadSystem` at `Ashfall.Core` (precompiled DLL or project reference). Extract `CensusClaimSystem` next (only `Mathf.Clamp` couples it). Do not port GameBootstrap.
