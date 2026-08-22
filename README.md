# ASHFALL: Atomic War — Starving Survival

Post-nuclear survival strategy RPG written in C#, currently migrating from a Unity-first implementation to a Godot 4.7.1 .NET host through an engine-agnostic shared Core.

The project is data-driven, deterministic by design, and built around plain-C# simulation systems with thin host/UI layers. No LLM is used at runtime.

## Source authority

The most important repository rule is to modify the correct layer.

| Concern | Authority |
| --- | --- |
| Engine-agnostic gameplay/simulation logic | `Assets/Ashfall.Core/` |
| Active Godot host, composition and UI | `src/`, `scenes/`, `project.godot` |
| Authored gameplay/content data | `Assets/StreamingAssets/Data/` |
| Unity-coupled legacy gameplay awaiting migration | `Assets/_Game/` |
| Unity compatibility project/build | `Assets/`, `Packages/`, `ProjectSettings/` |
| Historical/generated/quarantined material | reference only; not runtime authority |

See [`docs/ENGINE_SUPPORT_POLICY.md`](docs/ENGINE_SUPPORT_POLICY.md) for the canonical engine and migration policy and [`docs/ASHFALL_CODE_INDEX.md`](docs/ASHFALL_CODE_INDEX.md) for the detailed engineering map.

## Migration architecture

ASHFALL uses a strangler migration rather than a Godot rewrite.

- `Assets/_Game/` contains the large Unity-coupled implementation that is being migrated subsystem by subsystem.
- `Assets/Ashfall.Core/` is the migration destination. It contains engine-agnostic domain logic, ports, deterministic RNG/clock infrastructure, save/checksum logic, and migrated gameplay systems.
- `Ashfall.Core/Ashfall.Core.csproj` compiles the exact Core sources from `Assets/Ashfall.Core/` for xUnit tests.
- `Ashfall.csproj` is the Godot .NET aggregate project. It compiles `src/`, `scripts/`, `Assets/Ashfall.Core/`, and the legacy `_Game` surface through the compatibility bridge.
- `src/Bridge/` provides the Unity compatibility shim used to keep legacy code compilable while migration proceeds. Load-bearing semantic gaps fail loudly rather than silently returning plausible defaults.
- `scenes/Main.tscn` boots `src/Main.cs`.

A migrated gameplay rule should have one domain authority in Core. Do not create a second Godot-only implementation of logic that already exists in `_Game` or Core.

## Data authority

`Assets/StreamingAssets/Data/` is the authored JSON authority for shared gameplay/content data. Both hosts consume this data during the migration.

Important conventions:

- IDs are `snake_case`.
- Do not invent IDs in host code when a catalog owns them.
- Simulation state must be deterministic for the same seed and inputs.
- Save integrity must not depend on serializer formatting.
- Core code must not use Unity `JsonUtility`.
- Simulation calendar/randomness belongs behind `IClock` / `ISeededRng` rather than wall-clock or process-randomized APIs.

## Active host

The canonical development and verification host is Godot 4.7.1 .NET.

`project.godot` currently boots:

```text
scenes/Main.tscn -> src/Main.cs
```

The Godot host contains interactive UI plus a large headless diagnostic/self-test surface exposed by `src/Host/HostCli.cs`.

Examples:

```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --combat-selftest
godot --headless --path . -- --expansions-selftest
```

Use `godot --headless --path . -- --host-help` for the current command list.

## Build and test

Canonical verification is .NET + Godot:

```bash
# Engine-agnostic Core tests
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# Compile the Godot host + Core + compatibility surface
dotnet build Ashfall.csproj

# Canonical asset/data/bridge/gameplay gates
./scripts/ci/godot-asset-gate.sh
```

The primary GitHub Actions workflow, `.github/workflows/ci.yml`, performs JSON validation, Core tests, the Godot aggregate build/import, and canonical headless gates.

## Unity compatibility

The Unity project remains in the repository as a migration source and compatibility build surface. It is not the canonical gameplay verification host.

`.github/workflows/build.yml` can still produce Unity Windows/WebGL compatibility artifacts on `main` when Unity credentials are available. Those builds do not replace the Godot/Core quality gate.

Do not invoke Unity tooling for ordinary migration work unless the task explicitly requires Unity compatibility validation.

## Repository map

| Path | Responsibility |
| --- | --- |
| `Assets/Ashfall.Core/` | Shared engine-agnostic domain logic and migration target |
| `Ashfall.Core/` | .NET project wrapper that globs the shared Core sources |
| `Ashfall.Core.Tests/` | xUnit tests for shared Core behavior |
| `Assets/_Game/` | Unity-coupled legacy gameplay implementation |
| `Assets/StreamingAssets/Data/` | Authored JSON catalogs and shared data authority |
| `src/` | Godot host, sessions, UI, bridge, CLI/self-test harness |
| `scenes/` | Godot scenes; `Main.tscn` is the application entry scene |
| `scripts/ci/` | Canonical repository/Godot validation scripts |
| `.github/workflows/ci.yml` | Primary Godot/Core CI gate |
| `.github/workflows/build.yml` | Unity compatibility artifact workflow |
| `docs/` | Current engineering, migration and design documentation |
| `sources.md` | Comprehensive repository audit/risk report dated 2026-08-22 |

## Engineering principles

Keep changes reviewable and preserve these invariants:

1. One simulation/domain authority per system.
2. Same seed + same inputs must produce the same simulation result.
3. Validate all action preconditions before consuming resources.
4. Save capture/restore must be symmetric and must not alias live state.
5. Host wiring is part of correctness; a Core feature is incomplete until the active host supplies its required ports.
6. Persisted queues/histories must remain bounded or explicitly compacted.
7. Semantic compatibility-bridge gaps should fail loudly.

## Additional references

- `docs/ENGINE_SUPPORT_POLICY.md` — authoritative engine/support/migration policy.
- `docs/ASHFALL_CODE_INDEX.md` — detailed codebase map and subsystem reference.
- `sources.md` — current comprehensive architecture/codebase audit.
- `10LOOP_AUDIT_REPORT.md` — historical deep-audit ledger and regression evidence; treat reported test results as historical snapshots, not current CI status.
