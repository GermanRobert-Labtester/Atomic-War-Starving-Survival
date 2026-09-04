# RELEASE_EXPORT.md — Linux shipping pipeline (Plan VIII · Task 23)

## Prerequisites

| Tool | Version | Notes |
|---|---|---|
| Godot | 4.7.1.stable.mono (`godot --version`) | on PATH; `gl_compatibility` renderer |
| Export templates | 4.7.1.stable.mono | `~/.local/share/godot/export_templates/` |
| .NET SDK | 8/9/10 via `global.json` roll-forward | builds host + Core |
| Git LFS | initialized (`./setup-repo.sh` once) | fonts/images must be materialized, not pointers |

## Presets (`export_presets.cfg`)

- **Linux/X11** (`preset.0`): `export_filter="all_resources"`, `include_filter="*.json"` —
  ships every imported resource (all of `assets/**`, fonts, sprites) plus all JSON.
  Export path `builds/linux/ashfall.x86_64` (release) — debug uses the same preset
  via `--export-debug`.
- **Windows Desktop** (`preset.1`) exists but Task 23 targets Linux only.

Dev-only trees (`docs/`, `scripts/`, `.cache/`, `Ashfall.Core.Tests/`, `_verify_*`,
`Builds/`) are not Godot resources and are excluded by the resource filter;
the runtime data authority (`Assets/StreamingAssets/Data/**`) is never excluded.

## Data deployment model (why there are two copies of Data)

`src/Host/CatalogPath.cs` resolves the data dir in a fixed precedence:
`ASHFALL_DATA` env → **executable-relative** `Assets/StreamingAssets/Data` →
project dir → CWD walk → PCK virtual FS (`res://assets/StreamingAssets/Data`).

`scripts/ci/godot-export-linux.sh` therefore:
1. stages `Assets/StreamingAssets/Data` → `assets/StreamingAssets/Data` so the
   PCK carries a packed copy (fallback path #5), then removes the staging copy;
2. exports the exe + `.pck`;
3. deploys the loose authoritative copy beside the executable (primary path #2)
   and verifies representative catalogs landed.

## One-command export

```bash
scripts/ci/export-build.sh [--skip-smoke]
```

Stages: preflight → `dotnet build Ashfall.csproj` → `godot --headless --import` →
`godot-export-linux.sh` (PCK staging + export + loose Data deployment) →
`RELEASE_STAMP.txt` (game/commit/configuration/Godot version/UTC timestamp) →
**packaged parity gate** → exported runtime smoke (headless boot, bridge /
data-integrity / research-catalog / export-parity selftests **from the packaged
artifact**, `ASHFALL_DATA` explicitly unset so only packaged data is used).

Artifact layout:

```text
builds/linux/
  ashfall.x86_64                     ELF executable
  ashfall.pck                        packed resources (+ staged Data copy)
  Assets/StreamingAssets/Data/       loose authoritative data (byte-identical)
  RELEASE_STAMP.txt                  version traceability
```

## Packaged parity gate — `--export-parity-selftest`

`src/Host/HostCli.ExportParity.cs`, verb `--export-parity-selftest
[--parity-target <dir>]` (default: executable dir when run inside an exported
build, else `builds/linux`). Verifies against the repository data authority:

1. every source catalog (recursive `*.json`) exists in the exported deployment;
2. exact Linux path casing (explicit case-folded index comparison);
3. SHA-256 byte-identical for every catalog (stronger than the plan's sample);
4. every packaged catalog parses as JSON;
5. no Git-LFS pointer text shipped in place of binaries (23.8);
6. executable is a real ELF binary; PCK present and > 1 MB (23.7/23.8);
7. `RELEASE_STAMP.txt` echoed for traceability (23.11).

Exit 0 = shippable layout. A stale export FAILs with the exact
missing/mismatched catalog list (proven against the pre-Task-23 artifact:
missing `cupola_foundry_catalog.json` etc. were caught).

Deterministic payload hashes (23.13): two exports of the same commit contain
byte-identical catalog sets (the loose deployment is a plain `cp -r`); the
executable/PCK bytes themselves are not required to be reproducible.

## Known issue (infrastructure) — editor-mode mono crash on this machine (2026-09-05)

Since ~2026-09-05 00:58, **every Godot editor-mode process on this machine
crashes** (`--import`, `--editor --quit`, `--export-release`, `--export-debug`)
with `alloc_static NULL` → `SIGILL` in `libcoreclr` immediately after
`first_scan_filesystem`. Runtime (game) mode is unaffected — all headless
selftests still pass. Unset `DOTNET_ROOT`, sanitized env, RAM headroom, import-cache
rebuild (933 MB cache verified intact), and LFS-pointer checks were all ruled
out; valid-XML checks on all new SVGs passed. Yesterday (2026-09-04 18:30) the
same tree exported successfully. Classification: `infrastructure`
(per `docs/ci/README.md` taxonomy) — root cause lives outside this repository
(concurrent-stream environment, mono/coreclr resolution, or machine state).

**Workaround until it clears:** run `scripts/ci/export-build.sh` from a normal
user shell outside the agent sandbox (the 2026-09-04 artifact proves the
pipeline), or retry once the mono/editor regression clears. The parity gate and
all exported-build selftests are already wired and will run as part of the
script the moment a fresh export succeeds.

## CI wiring (23.12)

Full-tier gate `python3 scripts/ci/run-gates.py --tier full` includes
`export-parity` when `builds/linux/ashfall.x86_64` exists (see
`docs/ci/README.md`); runners without export templates use the documented
manual fallback: execute `scripts/ci/export-build.sh` on a capable machine and
commit/upload the artifact for parity verification.

## Selftest verbs from the exported build (23.9)

```bash
cd builds/linux
ASHFALL_DATA= ./ashfall.x86_64 --headless --data-integrity-selftest
ASHFALL_DATA= ./ashfall.x86_64 --headless --research-catalog-selftest
ASHFALL_DATA= ./ashfall.x86_64 --headless --bridge-selftest
ASHFALL_DATA= ./ashfall.x86_64 --headless --export-parity-selftest
```

`ASHFALL_DATA=` (explicitly empty) forces `CatalogPath` to the
executable-relative deployment — the selftests run on packaged data, never the
repository checkout.
