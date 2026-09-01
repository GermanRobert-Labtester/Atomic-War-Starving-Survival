# Plan 26 — The Ship Gate: Exported Builds That Actually Find Their Data

> **Wave:** Continuity Wave 3 — *Ship It Intact*
> **Depends on:** nothing to start. Feeds every other Wave-3 plan (a gate nobody boots can't
> protect anything).
>
> **Theme:** ASHFALL has 46 CI gates, an excellent path resolver, and a careful export script —
> and the CI export job **doesn't use that script**, the "verify data authority" step **cannot
> fail**, **nothing boots the exported binary**, the Windows build stages **no data at all**, and
> at least five places in the host resolve the data folder on their own. The artifact a player
> downloads is the least-tested thing in the repository.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The resolver is well designed | `src/Host/CatalogPath.cs:17–63` — precedence: `ASHFALL_DATA` env → executable-relative → globalized `res://` → CWD walk → PCK virtual FS (`res://Assets/…`, `res://assets/…`, `res://StreamingAssets/Data`), with `CreateFileIOForDataDir` switching to `GodotFileIO` for `res://` (plain `System.IO.File` cannot read inside a PCK) |
| 2 | **Five+ call sites bypass it** | `src/Host/EventsHostSession.cs:40,49,61` — literal `"res://Assets/StreamingAssets/Data/events.json"`/`incidents.json`/`narrative_progression.json`; `src/Main.FactionBranch.cs:18–20` — `ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data")` then `AppContext.BaseDirectory`; `src/Host/HoldfastTerminalPanel.cs:72` — relative `"Assets/StreamingAssets/Data"`; `src/Host/RadioHostSession.cs:67` — `Directory.GetCurrentDirectory()`; `src/Host/PanelBindLifecycleSelfTest.cs:366` |
| 3 | A good staging script exists and CI ignores it | `scripts/ci/godot-export-linux.sh` — stages lowercase `assets/StreamingAssets/Data`, exports, verifies `ashfall.x86_64` + `.pck`, deploys loose data beside the binary, checks representative catalogs and counts. `.github/workflows/build.yml:44–48` runs raw `godot --export-release` **instead** |
| 4 | **The staging script depends on a file that doesn't exist** | its own comment: `"Assets/.gdignore prevents packing Assets/StreamingAssets/Data"`. `find`/`git ls-files` show `.gdignore` under `Assets/Ashfall.Core`, `Assets/art`, `Assets/audio`, `Assets/sprites`, `Assets/ui` — but **no `Assets/.gdignore` and no `Assets/StreamingAssets/.gdignore`**, while `export_filter="all_resources"` + `include_filter="*.json"` pack every JSON they can see |
| 5 | Consequence: the authority is packed twice | 413 JSON files / **8.3 MB** staged at `res://Assets/…` *and* `res://assets/…` inside one PCK — 2× size and a resolver coin-flip, on the exact case-aliasing hazard `AGENTS.md` pins `core.ignorecase=false` to avoid |
| 6 | The data-verification step cannot fail | `build.yml:51–61` — `if [ -f "builds/linux/ashfall.pck" ]; then echo "PCK found…"; fi` + `ls … \| head -n 5`. No `test -s`, no count comparison, no exit path; both branches only print |
| 7 | The Windows job ships no data plan at all | `build.yml:96–105` — export `.exe`, `test -f`, upload `builds/windows/`. No staging, no verification, no `ASHFALL_DATA` handling |
| 8 | **Nothing boots a shipped artifact** | `grep -rn "ashfall.x86_64\|smoke" .github/workflows/*.yml scripts/ci/*` — only build-path existence checks. Every one of the 46 gates runs from the *source tree*, so an export-only failure is structurally invisible |
| 9 | Performance is measured, not budgeted | `src/Host/PerformanceSelfTest.cs:47` records `day_advance_30d` as `"advisory"`; `artifacts/runtime-scale-results.json`: median **0.609 s**, p95 **1.145 s**, max **1.265 s** over 5 iterations. `--runtime-scale-selftest` is gate #19, but the headline metric has no threshold |
| 10 | Version/report discipline is already good | `Ashfall.Core.Tests/VersionReportContractTests.cs` pins `--version` output incl. every save-store schema version — the right model to imitate for export/report gates |

---

## Task 26A — One path authority: everything reads the data folder through CatalogPath

**Goal:** delete every private data-path resolution, so "where is the JSON" has one answer that
already handles PCK, env override, loose deploy, and development checkouts.

**Files:** `src/Host/CatalogPath.cs`, `src/Host/EventsHostSession.cs:40,49,61`,
`src/Main.FactionBranch.cs:18–20`, `src/Host/HoldfastTerminalPanel.cs:72`,
`src/Host/RadioHostSession.cs:67`, `src/Host/PanelBindLifecycleSelfTest.cs:366`,
`Ashfall.Core.Tests/CatalogPathContractTests.cs` (new), `scripts/ci/forbidden-api-gate.sh`.

### Substeps

1. **Add a single catalog-file helper** to `CatalogPath` — `ResolveCatalog(string fileName)` (and a
   `ResolveSub(dir, fileName)`) — because the bypass sites mostly want *one file*, not the folder.
2. **Prove the failure mode first**: run the host with `ASHFALL_DATA` pointed at a copy and assert
   the bypassing sites still read the old location. That is the bug in one line, and the test that
   keeps it from returning.
3. **Convert `EventsHostSession`** (three literals): the res:// literals only work in dev/editor;
   under an exported PCK-only build with the lowercase staging path they resolve to a directory
   that may or may not exist. Route through `CatalogPath` and its `GodotFileIO` switch.
4. **Convert `Main.FactionBranch.cs:18–20`**: it duplicates precedence 2 with `AppContext`
   and skips precedence 5 (PCK) entirely — exactly the exported-build break. Delete the local
   resolver and use the shared one.
5. **Convert `HoldfastTerminalPanel.cs:72`** and any `"Assets/StreamingAssets/Data"` relative
   literals: CWD-relative resolution is silently wrong when launched from a desktop entry or
   Steam.
6. **Convert `RadioHostSession.cs:67`**: keep the CWD fallback only inside `CatalogPath`, not as a
   second implementation.
7. **Convert the selftest site** (`PanelBindLifecycleSelfTest.cs:366`) so the test exercises the
   shipped resolution order rather than a dev-only default.
8. **Gate the bypass**: extend `scripts/ci/forbidden-api-gate.sh` (or a sibling) to fail on the
   string `"Assets/StreamingAssets/Data"` and on `GlobalizePath("res://Assets/` /
   `GetCurrentDirectory()` outside `CatalogPath.cs`, mirroring the existing forbidden-API idiom so
   no new resolver appears.
9. **Make `CreateFileIOForDataDir` the only file-IO construction point** for catalog reads, so a
   PCK-resident authority can never be read with `System.IO.File` again.
10. **Add the contract test**: enumerate every catalog filename the game loads and assert each
    resolves non-empty under (a) dev tree, (b) `ASHFALL_DATA` copy, (c) simulated PCK-only
    resolution — the last via `GodotFileIO` over a staged fixture if a real export isn't available
    in that gate tier.
11. **Log the resolved root at boot** (`--version` report already prints a data-directory line —
    extend it) so a support ticket can answer "which data did that build read?" without debugging.
12. **Run the checklist** + `verify-fast.sh`.

**DoD:** exactly one file in the repository decides where the data authority lives, and the log
says which one it picked.

---

## Task 26B — Make the export real: CI uses the staging script and the checks can fail

**Goal:** the artifact is built by the same script developers run, contains exactly one copy of
the data authority, and boots.

**Files:** `.github/workflows/build.yml`, `scripts/ci/godot-export-linux.sh`,
new `scripts/ci/godot-export-windows.sh`, new `Assets/.gdignore` (or
`Assets/StreamingAssets/.gdignore`), `export_presets.cfg`, new
`scripts/ci/export-smoke-boot.sh`, `docs/ci/CI_GATE_MANIFEST.json`, `docs/CI.md`,
`.gitignore`.

### Substeps

1. **Resolve the `.gdignore` contradiction first**: either add `Assets/StreamingAssets/.gdignore`
   (matching the script's stated assumption and leaving `assets/` staging as the single packed
   copy) or drop the lowercase staging step and let `res://Assets/…` be packed once. Pick one and
   make the script's comment, `CatalogPath` precedence, and the preset agree — the current state
   quietly ships two copies.
2. **Verify the choice by inspecting the built PCK**, not by trusting the copy: `godot --headless
   --path . -- --version` against the exported build, plus a listing/diff of the two candidate
   trees; assert one canonical res:// path and a file count equal to the source (413 JSON today).
3. **Point `build.yml`'s Linux job at `scripts/ci/godot-export-linux.sh`** so CI and local builds
   are the same code path — the divergence is the root cause here, not any individual bug.
4. **Add a Windows export script** with parity (staging, loose deploy, representative-file checks,
   counts) and use it in the Windows job; today that job stages no data and verifies nothing.
5. **Write assertions that can fail**: replace the `if [ -f … ]; then echo` block with `test -s`
   on binary and PCK, a JSON-count comparison source vs deployed, and a data-integrity run against
   the *deployed* folder (`--data-integrity-selftest` with `ASHFALL_DATA` pointed at the artifact).
6. **Add the smoke boot**: `scripts/ci/export-smoke-boot.sh` runs the exported binary headless with
   a `--boot-smoke` style verb — boots the main scene, resolves catalogs, ticks one day, saves to a
   temp `user://`, exits 0. If no such verb exists, add one to the existing host CLI rather than
   inventing a new harness.
7. **Add a load smoke**: boot the exported build against a **known save fixture** and assert the
   campaign loads, the briefing renders, and the checksum validates — that is the one test a
   player's first five minutes actually depends on.
8. **Register both as gates** in `docs/ci/CI_GATE_MANIFEST.json` (Tier-2 quality at minimum, with
   `expected_summary` in the existing format) so `verify-fast.sh --list` shows them.
9. **Artifact hygiene**: upload binary + `.pck` + deployed `Assets/StreamingAssets/Data` together
   (an artifact missing its loose data reproduces the bug locally for every tester), and record
   size so a double-packed authority is visible as a size jump.
10. **Add `.gdignore`/staging cleanup to `.gitignore`** — `assets/StreamingAssets/` and `builds/`
    are generated; the repo must not start tracking the duplicate tree (which would also trip the
    case-sensitivity hazard the `setup-repo.sh` script guards).
11. **Export the data-integrity + content-utilization reports** into the artifact as `report/` text
    so a build is self-describing (mirrors what `VersionReportContractTests` already pins).
12. **Run the checklist**, then run the export scripts locally for both presets and paste the
    counts into the PR.

**DoD:** CI builds ship via the same script devs use, contain one copy of the authority, and a
headless boot of the *exported* binary passes on every push.

---

## Task 26C — Budgets and diagnostics: what "healthy build" means after the loop work lands

**Goal:** turn measurement into gates before Wave 2's new seams (wear sinks, needs stacks, cascade
evaluation, per-tick buffers) make performance a fire drill — and give support enough to triage a
crash.

**Files:** `src/Host/PerformanceSelfTest.cs`, `artifacts/runtime-scale-results.json`,
`Ashfall.Core/Performance/*`, `docs/perf/*` (new), `.github/workflows/ci.yml`,
`docs/CURRENT_AUTHORITY.md`, new `scripts/ci/perf-budget-gate.sh`, boot/lifecycle logging in
`src/Main.Lifecycle.cs`.

### Substeps

1. **Set the budget from data, not hope**: take the current
   `day_advance_30d` median/p95 (`0.609` / `1.145 s`) plus headroom for Waves 1–2's added seams,
   and encode it as an explicit, reviewed number in a `docs/perf/BUDGETS.md` — then have
   `--runtime-scale-selftest` fail against it instead of labelling the result `"advisory"`
   (`PerformanceSelfTest.cs:47`).
2. **Add allocation budgets**, not just time: the harness already samples
   `TotalAllocatedBytes`; gate day-advance allocation per tier (30/180/360-day) since the Wave-2
   seams (`CollectWornGear` lists, modifier stacks, cascade rules) are exactly the per-tick
   allocation class.
3. **Scale the tiers deliberately**: add a "big late-game holdfast" tier (max roster, max installed
   plant, max census/muster records) because that is where the day loop actually gets expensive —
   the existing tiers use normal roster/catalog sizes.
4. **Frame budget for the 2D UI**: a separate `--player-panels-uitest`/snapshot-adjacent check that
   opening the heaviest dashboard surfaces stays inside a frame budget (the panel count is 164
   classes / 135 routes; per `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`).
5. **Node/leak budget**: assert node counts return to baseline after open→close→load→new-game
   cycles, using the telemetry guide's own instrumentation — this is where 16C-style subscription
   leaks would otherwise show up as slow-motion memory growth.
6. **Long-session soak**: a headless 360-day run in a scheduled (nightly, not per-push) job with
   monotonic growth assertions; keep per-push tiers fast and let soak live nightly.
7. **Boot diagnostics**: log resolved data dir (26A step 11), catalog count, save slot root,
   graphics/audio device selection, locale, and version-report contents into a single
   `session-diagnostics.txt` in `user://` for support triage.
8. **Crash/first-chance surface**: a ring buffer of the last N day-advance operations and warnings
   so a reported crash carries a repro sequence; no new telemetry service, no network calls, no
   PII — and never a bare `catch { }` (H4's lesson, already enforced by
   `catch-policy-gate.sh`).
9. **Fail the build on warnings**: `warning-baseline-gate.sh` exists; assert the *export* config is
   also warning-clean (release builds currently only checked by the dev build).
10. **Dependency freshness**: `nuget-dependency-gate.sh` exists; confirm the exported binary's
    `.NET` runtime folder check (the export script already WARNs if
    `data_Ashfall_linuxbsd_x86_64` is missing) becomes a hard failure, since a missing mono
    runtime is an instant non-boot.
11. **Document the contract**: `docs/perf/README.md` naming each budget, its owner, and how to
    re-baseline — and cross-link from `docs/CURRENT_AUTHORITY.md`.
12. **Tests**: a selftest asserting budgets are non-null and sourced from the doc file, so a
    budget can't silently disappear; plus one intentional-regression test that the gate actually
    fails a bad number.
13. **Run the checklist** + both export scripts + `verify-fast.sh`.

**DoD:** `--runtime-scale-selftest` can fail a PR, a nightly soak catches growth, and every
exported build writes a boot diagnostics file.

---

## Cross-Task Dependencies

```
26A (one path authority) ──► 26B (PCK/single-copy verification is only meaningful once every
                               │      reader uses the resolver)
                               ├──► 26C (diagnostics reference the resolved root)
25A/25C (locale overlay files must be packed too — 26B's count checks cover the new trees)
17B/22A (save fixtures for the load smoke come from the wave-1/2 journey tests)
```

**Execution order:** 26A → 26B → 26C. Do **not** reorder: booting an exported build (26B step 6)
while five readers bypass the resolver (26A) produces a green smoke and a broken game — the exact
false confidence this wave is cleaning up.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/godot-export-linux.sh                         # local parity with CI
7. bash scripts/ci/godot-export-windows.sh                       # (26B)
8. bash scripts/ci/export-smoke-boot.sh                          # boots the artifact
9. bash scripts/ci/perf-budget-gate.sh                           # (26C)
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Files | New gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|
| 26A | ~7 call sites + 1 helper | 1 (forbidden path) | 6–9 | Low–Med | LOW (behaviour identical in dev) |
| 26B | 2 workflows, 2 scripts, presets | 2 (export + boot) | 3–5 + CI | Medium | LOW–MED (CI-only until proven locally) |
| 26C | 3 + docs | 1 (budget) | 5–8 | Medium | MEDIUM (budgets too tight block PRs — start generous, ratchet) |

**Guardrails:** no new build system, no new CI provider, no encryption/DRM work, no store
integration, and no shipping a "fixed" export without a PCK file-count assertion — a double-packed
8.3 MB authority is invisible in every gate that exists today.
