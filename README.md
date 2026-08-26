# ASHFALL: Atomic War – Starving Survival

2D post-nuclear survival-management game. Godot 4.7 .NET (C#) is the only
runtime/editor target.

Engine-agnostic simulation lives in `Assets/Ashfall.Core/`; the Godot host and
presentation live in `src/`; authored data lives in
`Assets/StreamingAssets/Data/` (the sole data authority); Godot-native art,
audio, fonts, and UI resources live in `assets/`.

A legacy-engine (Unity) tree is still present as a migration artifact and is
being removed — see "Legacy migration surface" below. It is never the source
of truth for architecture decisions.

## Stack

- **Engine:** Godot 4.7.1 .NET (Mono). Compatibility renderer, `canvas_items`
  stretch, 1920×1080 default viewport.
- **Host:** `Ashfall.csproj` (net8.0) compiles `src/**/*.cs` +
  `Assets/Ashfall.Core/**/*.cs` into the Godot assembly (`AtomicWar`).
- **Core:** engine-agnostic `Ashfall.Core` (plumbed into host and tests from a
  single source tree; do not copy files).
- **Tests:** xUnit via `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.

## Namespace scheme

- Gameplay systems: `Ashfall.Core.<Domain>` (e.g. `Ashfall.Core.Inventory`,
  `Ashfall.Core.Economy`). No `Godot`, no `GodotSharp`, no `UnityEngine`.
- Godot host: `AtomicWar.GodotApp.*` — `UI` panels, `Host` sessions, `Journal`,
  `World`, `YearOfAsh` widgets.
- Ids: `snake_case` everywhere (`item_clean_water`, `recipe_iodine`).
- State changes: events via `IEventBus`; state captured and restored through
  `CaptureState`/`RestoreState`; no `System.Random`/`Guid.NewGuid()` in
  simulation — `ISeededRng` only.

## Folder map

### `Assets/Ashfall.Core/` — engine-agnostic core

Every system (Disease, Dose Ledger, Duty Roster, Economy/Market, Crafting,
Expeditions, Muster, Narrative catalogs, Radiation, Research, Survivors,
UtilityAI, Verdict, Year of Ash, Weather, …) plus the shared ports
(`IJsonSerializer`, `IFileIO`, `ISeededRng`, `ILog`), checksummed save
envelope contracts (`SaveChecksum`, `SaveEnvelopeDetection`), and the
`CatalogIntegrityValidator`. Depends on nothing outside this tree.

### `Assets/StreamingAssets/Data/` — authored JSON data

One JSON file per catalog (plus the large `narrative/` corpus). `snake_case`
ids, every file carries `schema_version`; loaded via `res://…` by host
sessions and validated by the data-integrity selftest (cross-reference,
range, uniqueness).

### `src/` — Godot host

Thin presentation/wiring only. `src/Host/` typed sessions per catalog/subsystem
(Capture/Restore + per-store save codecs); `src/UI/` panels; `src/Main.cs`
(`AtomicWar.GodotApp.Main`) builds all screens from the ASCII feel of Godot UI.
The root `Main.tscn` bootstraps the host.

### `assets/`, `scenes/`, `Ashfall.Core.Tests/`, `scripts/ci/`, `tools/`

- `assets/` — Godot-native art/audio/fonts/sprites/ui (images/fonts are Git
  LFS; runtime audio plain binary). Every importable file has a `.import`
  sidecar (pre-commit hook enforces this).
- `scenes/` — hand-written scenes (`Main.tscn` + world shells).
- `Ashfall.Core.Tests/` — xUnit tests targeting net9.0 (host is net8.0).
- `scripts/ci/` — gates: `godot-asset-gate.sh` (selftest battery),
  `asset-orphan-sweep.sh`, `git-hooks/pre-commit`.
- `tools/` — asset pipeline + dev utilities (`ui-preview`, manifest tools,
  generation scripts).

## Persistence

Checksummed save envelopes with explicit version migration; malformed current
envelopes are rejected; writes go through the shared atomic writer; per-store
SHA-256 records (`SaveLoadHostSession`, `SaveChecksum`).

`Assets/StreamingAssets/Data/` is the authored JSON authority for shared gameplay/content data. Both hosts consume this data during the migration.

The simulation systems are implemented against the Core and the host is a
working shell: navigation between all panels, the multi-day gameplay loop,
save/continue, and settings overlay pass `--playable-shell-selftest`, and the
catalog battery (`--data-integrity-selftest` etc.) stays green. Remaining
presentation polish is tracked in the panel-level snapshots the selftests
generate.

## Legacy migration surface (deprecated)

`Assets/_Game/`, `Assets/UI/…`, `Assets/Resources/`, `Assets/Samples/` and
the `.meta`/`.asmdef` sidecars alongside them are migration artifacts from
the prior engine host, kept under version control only until the dependency
map is exhausted. Prefer `Assets/Ashfall.Core/` and `src/` for new work.
Do not add new code or assets here, and follow the "Godot is authoritative"
rule. The shim under `src/Bridge/` (a `UnityEngine` compatibility namespace)
exists to hold the compiled Unity tree in compatibility only and is in
scope for removal with `Assets/_Game/`.

## Active host

The canonical development and verification host is Godot 4.7.1 .NET.

`project.godot` currently boots:

```text
scenes/Main.tscn -> src/Main.cs
```

The Godot host contains interactive UI plus a large headless diagnostic/self-test surface exposed by `src/Host/HostCli.cs`.

Examples:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --asset-registry-selftest
godot --headless --path . -- --playable-shell-selftest
godot --headless --path . -- --ui-layout-selftest
./scripts/ci/godot-asset-gate.sh
```
