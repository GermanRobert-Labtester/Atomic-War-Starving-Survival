# ASHFALL — Export Report (2026-09-25, Alpha Packaging Pass)

Skill: `.agents/skills/ashfall-export-build/SKILL.md` · Presets: `export_presets.cfg`
(Linux/X11 → `builds/linux/ashfall.x86_64`; Windows Desktop → `builds/windows/ashfall.exe`).
Templates present: `4.7.1.stable.mono`. `embed_pck=false`, `include_filter="*.json, *.csv"`.

## 1. Preset audit

| Preset | Output | Templates | Status |
|---|---|---|---|
| Linux/X11 | `builds/linux/ashfall.x86_64` + `ashfall.pck` + `data_Ashfall_linuxbsd_x86_64/` | 4.7.1.stable.mono | **exported + PCK-only verified** |
| Windows Desktop | `builds/windows/ashfall.exe` + `ashfall.pck` + `data_Ashfall_windows_x86_64/` | 4.7.1.stable.mono (installed 2026-09-25 from the mono template bundle) | **exported + wine smoke PASS** |
| macOS | — | `macos.zip` template installed, **no export preset defined** | future work |

Each distribution is **three artifacts**: binary, PCK, and the `data_*` assembly folder. Shipping
the binary + PCK alone fails with `.NET: Assemblies not found`; all three are required.

## 2. Pre-export gate

| Check | Command | Result |
|---|---|---|
| Host build | `dotnet build Ashfall.csproj --nologo` | 0 errors |
| In-tree data authority | `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |

## 3. Export

Command: `godot --headless --path . --export-release "Linux/X11" builds/linux/ashfall.x86_64`
(full log: `/tmp/export.log`). The log shows `Storing File` for **425/425**
`Assets/StreamingAssets/Data/*.json`; exit 0.

## 4. Data-in-package verification (corrected 2026-09-25)

An earlier draft of this report claimed the PCK exposed only 313/425 catalogs. **That
finding is retracted.** Root cause: a stale loose `Assets/StreamingAssets/Data` copy from an
earlier debug session sat next to the binary; `CatalogPath` correctly prefers executable-relative
data, so the runtime read the stale 313-file copy while the export log was right all along.

Decisive tests (binary + PCK + `data_*` copied to `/tmp/ashfall-full`, outside the project
tree so no project-tree fallback is possible):

| Deployment | Command | Result |
|---|---|---|
| **PCK-only** (no loose data) | `./ashfall.x86_64 --headless -- --data-integrity-selftest` in `/tmp/ashfall-full` | **PASS — 425 catalogs, 0 errors, 5 warnings** |
| Loose data via `ASHFALL_DATA` | same with `ASHFALL_DATA=<data tree>` | **PASS — 425 catalogs, 0 errors** |
| Executable-relative loose data | `builds/linux/Assets/StreamingAssets/Data` (fresh copy) | **PASS — 425 catalogs, 0 errors** |

So all three resolution paths work; in-pack enumeration is sound. The stale-copy incident is
recorded as a deployment-recipe warning, not a code defect.

## 5. Deployment recipe (alpha)

Fully self-contained (recommended — what §4 verifies):

```
builds/linux/
  ashfall.x86_64
  ashfall.pck
  data_Ashfall_linuxbsd_x86_64/
```

Optional loose-data mode (patchable data, same bytes): copy
`Assets/StreamingAssets/Data/` to `builds/linux/Assets/StreamingAssets/Data/` — **delete the
directory first** (`rm -rf`) so a stale copy can never shadow the package, then launch via
`run-ashfall.sh` (sets `ASHFALL_DATA`). Verified PASS in §4.

## 6. Smoke results (two-platform)

| Metric | Linux | Windows |
|---|---|---|
| Export artifacts | x86_64 + PCK + `data_*` | exe + PCK + `data_*` |
| Data verification | PCK-only, isolated from the project tree: **425 catalogs, 0 errors** | wine smoke of the exported exe: **425 catalogs, 0 errors** |
| Binary size | 73,627,864 B (73.6 MB) | 109,359,104 B (109.4 MB) |
| PCK size | 294,172,544 B (294.2 MB) | 294,529,552 B (294.5 MB) |
| Boot wall (headless) | 3.51 s | (wine smoke, headless) |
| Distribution zip | `dist/ashfall-alpha-linux-x86_64-20260925.zip` (338.4 MB) + `.sha256` | `dist/ashfall-alpha-windows-x86_64-20260925.zip` (349.0 MB) + `.sha256` |

Both zips contain `SHA256SUMS.txt` for the binary and PCK, a `HOW-TO-RUN.txt`, and the
`RELEASE_STAMP.txt` (Linux). `scripts/tools/ashfall-release-zip.sh` regenerates them.

## 7. Follow-ups

1. Define a macOS export preset (the `macos.zip` template is installed) and smoke it on a Mac.
2. Human visual pass on the Linux binary (window, input, first-hour funnel) before alpha handoff.
3. Keep `docs/builds/BUILD_SIZES.md` updated by `ashfall-package-verify.sh` on every export.
