# CLAUDE INSTRUCTIONS FOR ASHFALL PROJECT

## Engine Policy
- **Migration target / primary going forward**: Godot Engine 4.7+ (.NET / C# Edition).
  The project is being reworked from Unity to Godot. New host code lands in Godot first.
- **Unity 6 LTS is still supported and may be used.** It holds the art pipeline, the authoring
  tooling and most existing gameplay. You may build, run and keep developing in Unity while the
  migration proceeds. Unity is not banned and not frozen — it is being handed over subsystem by
  subsystem, not abandoned.
- Prefer Godot for verification because it is fast and headless-friendly; reach for Unity when the
  work genuinely touches Unity host code, the art pipeline or the editor tooling.
- See `docs/GODOT_MIGRATION_STATUS.md` for what has actually been ported. Read it before claiming
  any subsystem works in Godot.

## Build & Test Commands
- Build Godot C# assembly: `dotnet build Ashfall.csproj`
- Run Godot Project: `godot --path "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"`
- Headless Verification: `godot --headless --path "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War" --quit-after 2`
- Unity EditMode/PlayMode suites remain valid for Unity-side changes.

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
