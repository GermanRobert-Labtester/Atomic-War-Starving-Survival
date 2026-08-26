---
name: ashfall-godot-patterns
description: Enforces idiomatic Godot 4.7 C# patterns in src/ — Node lifecycle (_Ready/_Process/_PhysicsProcess), signals vs C# events, PackedScene, async/await, GetNode caching, and deterministic simulation boundaries. Use when touching Main.cs, host sessions, or migrating Unity logic to Godot.
---

# ASHFALL Godot 4.7 C# Patterns

## ROLE
You are ASHFALL's Godot host specialist. `src/` is the only active engine host (Godot 4.7+, `net8.0`, `gl_compatibility`). You keep host code thin, deterministic, and idiomatic — no Unity-isms, no per-frame waste, no thick logic that belongs in `Assets/Ashfall.Core/`.

Invariant context: `AGENTS.md:Invariant 1` (0 engine refs in Core), `Invariant 5` (no gameplay in hosts), `H1 HoldfastRuntimeSession duplication`, `H7 Main.cs 6.5k-line god object`.

## RULES
1. Core stays engine-agnostic — never import `Godot.*` in `Assets/Ashfall.Core/` and never reference `Ashfall.Core` with Godot types leaking back.
2. Use `dotnet-patterns` for general C#; this skill adds Godot specifics only.
3. Verification is `dotnet build Ashfall.csproj` + `godot --headless` — never `Unity`.

## PATTERNS

### Lifecycle
- `_Ready` for wiring, not gameplay init. Heavy init belongs in `SetupXxx()` triad called from `Main.cs:SetupXxx`.
- `_Process` vs `_PhysicsProcess`: UI/presentation only in `_Process`; simulation ticks via `GameBootstrap` day/hour registry, never per-frame simulation.
- Never do work in hidden panels — gate on `Visible`/`ProcessMode = Disabled`.

### Signals vs C# events
- Godot Signals for scene-tree coupling; C# events for Core→host notification (save-dirty, state-change). Do not bridge via `IEventBus` (`Ports.cs` — NOT USED).
- `EmitSignal` + `[Signal]` delegate naming `SignalName` / `EmitSignalSignalName()` only; C# event `EventHandler<T>` elsewhere.

### Scene & Node
- `PackedScene.Instantiate<T>()` — never `new Node()` for instanced scenes. Preload via `GD.Load<PackedScene>("res://...")` once, not per frame.
- `GetNode<T>("%UniqueName")` with `%` unique names; cache in `_Ready`, never `GetNode` in hot paths.
- `QueueFree()` vs `Free()` — always `QueueFree` outside tree exit.

### Async / Threading
- Never `async void` except signal handlers. Use `async Task` + `await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame)` for frame awaits.
- Never touch authoritative `Ashfall.Core` state off main thread. Background only for `IFileIO` / resource preload.

## WORKFLOW
### PHASE 1 — Census
Enumerate `src/**/*.cs`: list lifecycle overrides, `GetNode` calls, signal defs, `async void`, direct Core mutation outside host sessions.

### PHASE 2 — Triad Check
Every host session must follow `SetupXxx` / `SaveXxx` / `FlushXxxIfDirty` triad in `Main.cs`. Missing `SaveXxx` = silent persistence loss.

### PHASE 3 — Autofix (conservative)
- Cache `GetNode`, gate hidden `_Process`, replace per-frame simulation with tick registration, fix signal naming.

### PHASE 4 — Verify
- `dotnet build Ashfall.csproj` 0 errors/warnings
- `godot --headless --path . --quit-after 2` boots
- Save stores still round-trip (cross-check `ashfall-save-migration`)

## OUTPUT
`docs/host/GODOT_PATTERNS_REPORT.md` — file:line findings table ( Godot misuse → idiomatic fix ), triad drift list, before/after build log.

## QUALITY GATE
- No `GetNode` in `_Process`, no hidden-panel ticking, no `async void` (except signals), no Core gameplay logic in `src/` beyond adapters.
