# CLAUDE INSTRUCTIONS FOR ASHFALL PROJECT

## Engine Policy
- **Primary Engine / Active Target**: Godot Engine 4.7+ (.NET / C# Edition).
  All new host code, development, testing, and execution must use Godot and dotnet.
- **STRICT BAN ON RUNNING UNITY**: Never run, launch, or invoke Unity (in batchmode, editor, headless, or playmode) unless the user EXPLICITLY asks or suggests for Unity to be run.
- Unity codebase in `Assets/_Game/` is legacy reference code undergoing migration to `Ashfall.Core` and Godot.
- See `docs/GODOT_MIGRATION_STATUS.md` for what has actually been ported.

## Build & Test Commands
- Build Godot C# assembly: `dotnet build Ashfall.csproj`
- Run Unit Tests: `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- Run Godot Project: `godot --path "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"`
- Headless Verification: `godot --headless --path "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War" --quit-after 2`
- NEVER run Unity commands (`Unity -batchmode`, etc.) unless explicitly instructed by the user.

## Project Architecture
- **Godot host**: C# under `src/` and `scripts/`, scenes in `scenes/`, config `project.godot`,
  project definition `Ashfall.csproj` (`Godot.NET.Sdk/4.7.1`).
- **Unity host**: C# under `Assets/_Game/` (~228k LOC across 24 subsystems).
- **Shared authority**: JSON catalogs in `Assets/StreamingAssets/Data/*.json` feed BOTH engines.
  The Godot host reads them via `res://Assets/StreamingAssets/Data`. Do not fork data per engine.
- Simulation logic belongs in engine-agnostic plain C# (`Ashfall.Core`) with zero `UnityEngine` or
  `Godot` references. Both engines are thin hosts. Moving logic into the agnostic core IS the
  migration — see `AGENTS.md` for the dual-engine rules.

## Migration Guardrails
- `Ashfall.csproj` is hand-written, NOT generated. `.gitignore` carries an explicit `!Ashfall.csproj`
  negation because the Unity-oriented `*.csproj` rule would otherwise drop it from the repo.
- Anything you add under `src/`, `scenes/` or `scripts/` must be committed. Those paths are not
  covered by any ignore rule, so untracked Godot work is one `git clean -fd` from deletion.
