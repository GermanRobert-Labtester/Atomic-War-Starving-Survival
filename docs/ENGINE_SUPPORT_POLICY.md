# ASHFALL Engine Support and Source-Authority Policy

This document defines which engine and source tree is authoritative during the Unity-to-Godot strangler migration.

## Current support model

| Area | Status | Authority |
| --- | --- | --- |
| Godot 4.7.1 .NET host | **Active development and canonical verification host** | `project.godot`, `Ashfall.csproj`, `src/`, `scenes/` |
| Engine-agnostic gameplay logic | **Migration target / shared authority** | `Assets/Ashfall.Core/` |
| Authored gameplay/content data | **Single data authority** | `Assets/StreamingAssets/Data/` |
| Unity gameplay implementation | **Legacy migration source / compatibility surface** | `Assets/_Game/` |
| Unity project/build | **Compatibility build only; not the canonical gameplay gate** | `Assets/`, `Packages/`, `ProjectSettings/`, `.github/workflows/build.yml` |
| Historical audits/generated/quarantined material | **Reference only** | archive/quarantine/report locations; never treat as runtime authority |

## Hard rules

1. New engine-independent gameplay behavior belongs in `Assets/Ashfall.Core/`, not in a Godot-only copy.
2. `src/` and `scenes/` are host/composition/presentation code. They may adapt Core APIs to Godot but should not fork simulation rules already represented in Core.
3. `Assets/_Game/` is the Unity-coupled migration source. Modify it only when a compatibility fix or migration step genuinely requires it.
4. JSON under `Assets/StreamingAssets/Data/` is the authored data authority. Do not invent replacement IDs in host code.
5. Canonical pull-request verification is .NET + Godot: core tests, Godot aggregate build, data validation, asset import, and headless self-tests.
6. Unity builds are compatibility artifacts. A green Unity build does not replace the canonical Godot/Core gates, and Unity tooling is not required for ordinary migration work.
7. Cross-host invariants remain mandatory while Unity compatibility exists: deterministic simulation, serializer-independent save integrity, and no duplicated domain authority.

## Canonical verification

Run the same high-level path used by CI:

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
./scripts/ci/godot-asset-gate.sh
```

The final script performs the Godot import/build checks and the canonical headless gameplay/data/bridge gates.

For additional targeted checks, use the flags implemented by `src/Host/HostCli.cs`, for example:

```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --combat-selftest
```

## Unity compatibility workflow

`.github/workflows/build.yml` remains intentionally separate from the primary CI workflow. It produces legacy/compatibility Windows and WebGL artifacts on `main` when Unity credentials are available.

That workflow:

- is not the source-of-truth gameplay test gate;
- must not be referenced as if it runs the current Godot/Core test battery;
- should skip cleanly when Unity credentials are unavailable;
- should be retired when Unity compatibility is formally removed.

## Migration completion criteria

A subsystem is considered migrated only when:

- its simulation/domain authority lives in `Assets/Ashfall.Core/`;
- the Godot host uses that Core implementation rather than a host-only fork;
- deterministic behavior is covered by Core tests where applicable;
- save capture/restore is covered and cross-host-safe where applicable;
- required host ports are integration-tested;
- legacy `_Game` ownership is either removed or explicitly retained only for compatibility.

## Related references

- `docs/ASHFALL_CODE_INDEX.md` — detailed engineering map.
- `sources.md` — 2026-08-22 comprehensive repository audit and risk register.
- `src/Host/HostCli.cs` — current headless command registry/parser.
- `.github/workflows/ci.yml` — canonical PR/push quality gate.
- `.github/workflows/build.yml` — Unity compatibility artifact build.
