# C2 — Flagship Integration Plan [8]: Ship Gate, Single Data Path Authority, Real Export Smoke, and Performance Budgets

> **Deliverable:** `C2_planintegration[8].md`
> **Source scope:** Plan 26 — *The Ship Gate: Exported Builds That Actually Find Their Data*
> **Wave:** Continuity Wave 3 — *Ship It Intact*
> **Primary objective:** ensure every catalog read resolves through one data-path authority, make CI build artifacts through the same staging/export scripts developers use, boot and load-test the exported binary itself, eliminate duplicate packed data authorities, and convert runtime/performance diagnostics from advisory measurements into enforceable product gates.
> **Execution order:** **26A → 26B → 26C**
> **Hard sequencing rule:** do not begin export smoke as the primary validation until 26A has eliminated bypass readers, or the smoke can go green while individual readers still resolve dev-only paths.
> **Cross-wave integration:** locale overlays from Plan 25 must be included by export packaging/count checks; known save fixtures from earlier journey tests feed the exported load-smoke.
> **Scope discipline:** no new build system, no new CI provider, no store integration, no DRM/encryption work, no second data resolver, and no export declared healthy without a real PCK/file-count assertion and exported-binary boot.

---

# 0. Executive Intent

ASHFALL already contains sophisticated source-tree verification.

It also already has:

- a well-designed `CatalogPath`,
- many CI gates,
- export scripts,
- version/save-schema reporting,
- runtime-scale measurements.

The source plan identifies the productization gap:

```text
source-tree CI is healthier than the artifact the player downloads
```

Key failures:

- several readers bypass `CatalogPath`,
- the CI Linux job does not use the mature staging script,
- Windows export has no real data deployment plan,
- data verification steps print but do not fail,
- exported artifacts are not actually booted,
- data may be packed twice under case-variant paths,
- performance measurements are advisory rather than contractual.

C2[8] converts the release artifact from an output file into a tested runtime product.

The final release chain should be:

```text
source authority
→ one CatalogPath resolution contract
→ one staging/export script per platform
→ exactly one packed/deployed data authority
→ exported binary boots
→ exported binary loads a known save
→ exported binary reports resolved data root
→ artifact emits diagnostics
→ performance budgets can fail CI
→ artifact upload includes everything needed to run
```

The strongest product outcome is:

> **The exact binary/pck/data bundle uploaded by CI is the thing that boots, resolves catalogs, loads a save, advances gameplay, and is judged against explicit health budgets.**

---

# 1. Source Evidence and Starting Diagnosis

The source plan identifies the following high-value facts:

- `CatalogPath` already has sensible precedence:
  - `ASHFALL_DATA`,
  - executable-relative,
  - globalized `res://`,
  - CWD walk,
  - PCK virtual FS.
- multiple host readers bypass it,
- a mature Linux export/staging script exists,
- CI uses raw `godot --export-release` instead,
- the staging script assumes a `.gdignore` condition that is not actually present,
- data can be packed twice under `Assets/...` and `assets/...`,
- current verification does not fail on missing/invalid data,
- Windows has no equivalent data packaging contract,
- no shipped binary is booted in CI,
- runtime-scale metrics exist but do not enforce budgets.

The correct interpretation is:

```text
the project does not need more source-only gates;
it needs gates that interrogate the artifact.
```

---

# 2. Program-Level Success Criteria

C2[8] is complete only when all of the following are true.

## 2.1 One file decides where data lives

All runtime catalog readers route through `CatalogPath` or its sanctioned helper API.

## 2.2 Every catalog read uses compatible IO

PCK-resident data must not be opened with plain `System.IO.File`.

## 2.3 Local export and CI export are the same code path

No separate “developer script” and “CI raw export” behavior.

## 2.4 Exactly one authority copy ships

No case-aliased duplicate data tree inside the PCK/artifact.

## 2.5 Export verification can fail

Missing:

- binary,
- PCK,
- data files,
- wrong counts,
- missing runtime,
- bad integrity

must stop CI.

## 2.6 The exported binary boots

A headless artifact smoke:

- starts,
- resolves catalogs,
- enters campaign,
- advances/ticks,
- writes temp save,
- exits 0.

## 2.7 The exported binary loads

A known save fixture loads in the shipped artifact and validates checksum.

## 2.8 Diagnostics answer support questions

A session diagnostics file identifies:

- version,
- resolved data root,
- catalog count,
- locale,
- save root,
- device selections,
- recent operations/warnings.

## 2.9 Performance budgets are enforceable

Critical runtime metrics have explicit reviewed thresholds.

## 2.10 Long-session growth is monitored

Nightly/slow tiers catch monotonic allocation/node/state growth.

---

# 3. Architectural Invariants

## 3.1 One data path authority

`CatalogPath` is the only implementation of data-root precedence.

## 3.2 One catalog-file helper

Callers resolve:

```text
folder
subfolder
catalog file
```

through sanctioned helpers.

## 3.3 One catalog IO construction point

The resolver determines the correct IO backend:

- normal filesystem,
- Godot/PCK virtual filesystem.

## 3.4 No data-path literals in production readers

Forbid:

- `Assets/StreamingAssets/Data`,
- hardcoded `res://Assets/...`,
- custom `GetCurrentDirectory()` data search,
- custom `AppContext.BaseDirectory` resolution.

outside the authority.

## 3.5 Export scripts are product authority

Workflows invoke versioned scripts.

Do not duplicate staging logic inline in YAML.

## 3.6 One packed data tree

Choose one canonical resource path.

Do not tolerate case variants.

## 3.7 Artifact smoke is separate from source-tree smoke

A source-tree run is not evidence an export works.

## 3.8 Diagnostics are local-only

No network telemetry, PII collection, or external reporting service.

## 3.9 Performance budget source is documented

Budget values cannot silently disappear from code.

---

# 4. Dependency Graph

```text
26A — one CatalogPath authority
 │
 ├──────────────► 26B export/PCK verification
 │                  │
 │                  ├─ Linux parity
 │                  ├─ Windows parity
 │                  ├─ boot smoke
 │                  └─ load smoke
 │
 └──────────────► 26C diagnostics use canonical resolved root
                    │
                    ├─ performance budgets
                    ├─ allocation budgets
                    ├─ leak/node budgets
                    └─ nightly soak

25A/25C locale overlays
 └──────────────► 26B packaging/count verification

17B/22A save journey fixtures
 └──────────────► exported load smoke
```

Required order:

```text
26A → 26B → 26C
```

---

# 5. Baseline Capture

Before edits, record:

- commit SHA,
- current CI workflows,
- current export script behavior,
- data file counts,
- current artifact sizes,
- current PCK contents where inspectable,
- current runtime-scale results,
- current version report.

## 5.1 Baseline commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Also capture current:

```bash
bash scripts/ci/godot-export-linux.sh
```

behavior without modifying output.

## 5.2 Data inventory

Record:

- source JSON count,
- locale overlay file count,
- loose deployed count,
- packed resource count,
- case-variant duplicate count,
- representative required files.

---

# 6. Workstream 26A — One Path Authority

## 6.1 Objective

Every runtime data reader resolves through `CatalogPath`.

---

# 7. 26A Phase A — Extend `CatalogPath`

Add sanctioned helpers such as:

```csharp
ResolveCatalog(string fileName)
ResolveSub(string dir, string fileName)
```

or equivalent.

## 7.1 Responsibilities

The helper:

- validates relative file/subpath,
- applies root precedence,
- returns compatible path/URI,
- pairs with correct file IO.

## 7.2 Security/safety

Reject:

- absolute path traversal via file name,
- `..`,
- malformed URI injections,
- unexpected separators if necessary.

This is local game content, but path discipline still matters.

---

# 8. 26A Phase B — Reproduce the Bypass Defect

Create an integration/contract test.

Scenario:

```text
copy data authority
set ASHFALL_DATA to copy
boot reader
```

Before fix:

- bypass reader still reads original/dev path.

After fix:

- every reader uses override copy.

Test at least one known bypass before refactoring all sites.

---

# 9. 26A Phase C — Migrate Known Bypass Sites

Re-verify and convert all source-identified readers.

Expected categories:

- event/incidents/narrative progression,
- faction branch,
- holdfast terminal,
- radio host,
- panel bind selftest.

Do not only patch the five named files.

Search repository for equivalent resolution code.

---

# 10. 26A Phase D — Eliminate CWD-Specific Data Logic

`Directory.GetCurrentDirectory()` may remain as a fallback inside `CatalogPath`.

It must not be reimplemented by a consumer.

Desktop launchers, Steam, shell launch, and tests may all have different CWD.

---

# 11. 26A Phase E — Eliminate `GlobalizePath` Private Resolution

If PCK virtual IO is valid, globalizing a `res://` path to a filesystem path can break the packaged case.

Keep PCK handling centralized.

---

# 12. 26A Phase F — One File IO Construction Point

Enforce:

```text
CatalogPath resolves data root
→ CreateFileIOForDataDir chooses IO
→ catalog loader uses returned IO
```

Avoid:

```text
resolved res:// path
→ System.IO.File
```

Add a test that this combination is impossible/safely rejected.

---

# 13. 26A Phase G — Forbidden Path Gate

Extend:

```text
scripts/ci/forbidden-api-gate.sh
```

or sibling.

Fail on production patterns outside the authority such as:

```text
Assets/StreamingAssets/Data
GlobalizePath("res://Assets/
Directory.GetCurrentDirectory()
```

with carefully scoped exceptions.

## 13.1 Exceptions

Allow:

- `CatalogPath.cs`,
- export/staging scripts,
- tests explicitly checking forbidden patterns,
- documentation if source scanner excludes docs.

No blanket directory exemption for host code.

---

# 14. 26A Phase H — Catalog Contract Enumeration

Create `CatalogPathContractTests`.

Enumerate every catalog expected by runtime.

Validate under:

1. dev tree,
2. `ASHFALL_DATA`,
3. PCK-like fixture/environment.

For each catalog:

- resolve non-empty,
- IO backend can open,
- representative parse/read succeeds.

---

# 15. 26A Phase I — Boot Resolution Logging

Extend version/boot report with:

- selected data root,
- resolution source:
  - env,
  - executable-relative,
  - res/PCK,
  - CWD fallback,
- IO backend.

Example conceptual line:

```text
data_root=res://assets/StreamingAssets/Data source=pck io=GodotFileIO
```

Avoid secrets/absolute personal paths in public-facing UI logs if support bundle may be shared; local file path in local diagnostics is acceptable but document privacy.

---

# 16. 26A Metrics

Publish:

```text
private resolvers before
private resolvers after
forbidden path hits
catalog contract entries
catalogs resolvable under each mode
```

Target:

```text
private resolvers after = 0
```

---

# 17. 26A Tests

- env override wins,
- executable-relative fallback,
- PCK path,
- CWD fallback only through authority,
- catalog-file helper,
- subdir helper,
- traversal rejection,
- compatible IO selection,
- every catalog resolves,
- boot/version logs chosen root.

---

# 18. 26A Definition of Done

- [ ] catalog-file helper exists,
- [ ] bypass defect captured by test,
- [ ] all private readers migrated,
- [ ] CWD logic centralized,
- [ ] GlobalizePath logic centralized,
- [ ] one IO construction point,
- [ ] forbidden path gate green,
- [ ] catalog contract test green,
- [ ] boot logs resolved root,
- [ ] exactly one source file owns resolution precedence.

---

# 19. Workstream 26B — Real Export and Artifact Smoke

## 19.1 Objective

CI and developers produce the same export bundle, it contains exactly one authority copy, checks can fail, and the shipped binary actually boots.

---

# 20. 26B Phase A — Resolve the `.gdignore` Contradiction

Choose one canonical data packaging strategy.

Option A:

```text
exclude Assets/StreamingAssets from PCK
stage lowercase assets/StreamingAssets as canonical packed path
```

Option B:

```text
keep Assets/StreamingAssets canonical
remove lowercase staging
```

Pick one.

Do not support both.

## 20.1 Decision criteria

Evaluate:

- current `CatalogPath`,
- Godot export filters,
- platform case sensitivity,
- developer checkout conventions,
- mod/loose-data deployment.

Document in export ADR/CI docs.

---

# 21. 26B Phase B — PCK Inspection

After export, verify actual package contents.

Assertions:

- one canonical data tree,
- no case-variant duplicate,
- JSON count matches intended packed set,
- locale overlay trees included,
- representative catalogs present.

Do not trust staging directory contents as proof of PCK contents.

---

# 22. 26B Phase C — Linux CI Uses Export Script

Modify Linux workflow to call:

```bash
scripts/ci/godot-export-linux.sh
```

instead of raw inline export.

The script becomes the canonical Linux packaging path.

---

# 23. 26B Phase D — Windows Export Parity

Create:

```text
scripts/ci/godot-export-windows.sh
```

Match Linux contract:

- staging,
- export,
- binary/PCK checks,
- data deploy,
- count checks,
- runtime check,
- report output.

Avoid platform-specific drift except where required.

---

# 24. 26B Phase E — Fail-Fast Assertions

Replace print-only verification with real assertions.

Minimum:

```bash
test -s binary
test -s pck
```

Then assert:

- expected source/deployed counts,
- representative files,
- no duplicate canonical roots,
- integrity selftest against deployed authority.

Any mismatch exits non-zero.

---

# 25. 26B Phase F — Deployed Data Integrity

Run exported/deployed data validation with:

```text
ASHFALL_DATA=<artifact/deployed/data>
```

The integrity test must read the same files that ship.

Do not validate source-tree data and infer deployment is correct.

---

# 26. 26B Phase G — Exported Boot Smoke

Create:

```text
scripts/ci/export-smoke-boot.sh
```

Run the exported binary headless.

Use an existing CLI host verb or add a small `--boot-smoke` verb to the existing CLI.

Required steps:

1. boot main scene,
2. initialize campaign,
3. resolve catalogs,
4. validate representative systems,
5. advance/tick one day,
6. save to temporary user directory,
7. exit 0.

## 26.1 Environment isolation

Use temporary:

- `user://` or override path,
- data root as artifact data,
- deterministic seed if possible.

No developer home-state dependency.

---

# 27. 26B Phase H — Exported Load Smoke

Use a pinned known save fixture.

Required:

1. start exported binary,
2. load fixture,
3. validate save checksum/schema,
4. render/build daily briefing state,
5. perform one safe read/action,
6. exit 0.

This catches:

- missing save migrations,
- missing data,
- broken route composition,
- locale/font initialization blockers.

---

# 28. 26B Phase I — Gate Registration

Register export artifact checks in:

```text
docs/ci/CI_GATE_MANIFEST.json
```

At minimum:

- export package verification,
- exported boot smoke,
- exported load smoke if separated.

Include:

- tier,
- command,
- expected summary,
- owner/docs link.

Ensure `verify-fast.sh --list` or equivalent surfaces them according to tier.

---

# 29. 26B Phase J — Artifact Hygiene

CI upload must contain:

- executable,
- `.pck`,
- required loose data if packaging strategy uses it,
- locale overlay files,
- report directory,
- version metadata.

Do not upload a binary-only artifact that depends on unshipped loose data.

---

# 30. 26B Phase K — Artifact Size Tracking

Record:

- binary size,
- PCK size,
- loose data size,
- total artifact size.

Add warning/failure policy for suspicious large jumps.

A duplicate 8 MB data authority should be obvious.

Avoid overly tight absolute size budgets; use reviewed baseline deltas.

---

# 31. 26B Phase L — Generated Tree Cleanup

Add generated staging/build directories to `.gitignore`.

Protect case sensitivity.

Do not allow:

```text
assets/StreamingAssets
```

generated staging tree to become tracked accidentally if canonical source is:

```text
Assets/StreamingAssets
```

---

# 32. 26B Phase M — Self-Describing Artifact Reports

Include `report/` inside CI artifact.

Suggested files:

- `version-report.txt`,
- `data-integrity.txt`,
- `content-utilization.txt`,
- `artifact-manifest.txt`,
- `data-root-report.txt`,
- `build-size.txt`.

Reports must come from the shipped/deployed data context.

---

# 33. 26B Linux/Windows Parity Matrix

| Contract | Linux | Windows |
|---|---:|---:|
| canonical script | yes | yes |
| binary exists | yes | yes |
| PCK exists | yes | yes |
| runtime exists | yes | yes |
| data staged | yes | yes |
| count checked | yes | yes |
| duplicate path checked | yes | yes |
| data integrity | yes | yes |
| boot smoke | yes | yes |
| load smoke | yes | yes |
| report bundle | yes | yes |

No “Windows later” gap at closure.

---

# 34. 26B Tests

- staging path decision test,
- PCK one-tree assertion,
- file-count comparison,
- missing binary failure test,
- missing PCK failure test,
- missing data failure,
- wrong count failure,
- boot smoke success,
- boot smoke intentional missing-data failure,
- load fixture smoke,
- Linux/Windows script dry/static parity.

---

# 35. 26B Definition of Done

- [ ] one packaging strategy documented,
- [ ] no duplicate packed authority,
- [ ] Linux CI uses script,
- [ ] Windows CI uses parity script,
- [ ] binary/PCK checks fail hard,
- [ ] deployed data counts checked,
- [ ] data integrity runs against artifact data,
- [ ] exported boot smoke passes,
- [ ] exported load smoke passes,
- [ ] gates registered,
- [ ] artifact contains all dependencies,
- [ ] artifact size reported,
- [ ] generated staging ignored,
- [ ] reports included in artifact.

---

# 36. Workstream 26C — Budgets and Diagnostics

## 36.1 Objective

Define what a healthy shipped build means and make regressions actionable.

---

# 37. 26C Phase A — Performance Budget ADR

Create:

```text
docs/perf/BUDGETS.md
```

Start from current measured data.

Source baseline:

```text
day_advance_30d median ≈ 0.609 s
p95 ≈ 1.145 s
```

Re-measure current branch.

Set reviewed thresholds with initial headroom.

Example approach:

```text
warning budget
hard failure budget
```

where appropriate.

Avoid immediately choosing a threshold so tight that normal CI variance dominates.

---

# 38. 26C Phase B — Runtime Scale Becomes Enforceable

Change:

```text
advisory
```

to:

```text
pass/fail against documented budget
```

for critical metrics.

Budget source should be machine-readable or parsed from a canonical config referenced by docs.

Do not duplicate numbers in five locations.

---

# 39. 26C Phase C — Allocation Budgets

Gate allocations as well as elapsed time.

Track:

- 30-day,
- 180-day,
- 360-day,
- large holdfast tier.

Focus on:

- per-day allocations,
- transient list churn,
- modifier stacks,
- cascade evaluation,
- gear buffers.

Use normalized metrics where useful:

```text
bytes/day
bytes/day/survivor
```

---

# 40. 26C Phase D — Late-Game Scale Tier

Create a max/large fixture:

- large roster,
- installed plant,
- full census/muster,
- mature content state,
- many active systems.

The normal early-game fixture is insufficient to expose scale regressions.

---

# 41. 26C Phase E — UI Frame Budget

Add a heavy-panel opening test.

Measure:

- dashboard open,
- inventory,
- expedition,
- journal,
- power,
- other heavy live panels.

Use a headless/UITest-compatible metric.

Do not overfit GPU frame time in unstable CI environments.

Prefer deterministic host/UI construction/update budget where possible.

---

# 42. 26C Phase F — Node/Leak Budget

After:

```text
open
close
load
new game
```

cycles:

- node count returns to baseline,
- handler count returns to baseline,
- memory/allocation does not grow monotonically.

This integrates Plan 16C subscription hygiene into product health.

---

# 43. 26C Phase G — Nightly 360-Day Soak

Run scheduled, not per-push.

Track:

- memory growth,
- node count,
- save size,
- day-advance time trend,
- warning count,
- exception count,
- major resource counts.

Fail on monotonic growth beyond budget.

---

# 44. 26C Phase H — Session Diagnostics File

Write:

```text
user://session-diagnostics.txt
```

or equivalent.

Include:

- game version,
- save schema versions,
- resolved data root,
- resolution mode,
- catalog counts,
- save root,
- locale,
- graphics/audio device summary,
- last boot stage,
- last N warnings/actions.

## 44.1 Privacy

No:

- network IDs,
- account data,
- personal usernames unless unavoidable path display and clearly documented,
- telemetry upload.

Prefer path redaction or normalized paths if diagnostic bundles may be shared.

---

# 45. 26C Phase I — Ring Buffer of Recent Operations

Maintain a bounded local ring buffer.

Candidate entries:

- day advance phases,
- save/load,
- session replacement,
- major incidents,
- catalog failures,
- warning summaries.

Bound:

```text
N entries
```

fixed memory.

No persistent unbounded log growth.

---

# 46. 26C Phase J — Export Warning Gate

Ensure Release/export build has zero unexpected warnings.

Do not only gate development build warnings.

Integrate existing warning-baseline tooling.

---

# 47. 26C Phase K — Runtime Dependency Gate

Linux/Windows exports must hard-fail if required .NET/Godot runtime components are absent.

A missing runtime is not a warning.

---

# 48. 26C Phase L — Budget Source Tests

Add tests:

- budget config exists,
- critical budget non-null,
- docs/config consistent,
- intentionally bad metric causes gate failure.

This prevents a deleted budget from converting the gate back into advisory behavior.

---

# 49. 26C Documentation

Create/update:

```text
docs/perf/README.md
docs/perf/BUDGETS.md
docs/CURRENT_AUTHORITY.md
docs/CI.md
```

Document:

- metric,
- threshold,
- reason,
- owner,
- re-baseline process,
- platform variance policy.

---

# 50. 26C Definition of Done

- [ ] explicit performance budgets,
- [ ] runtime-scale gate fails regressions,
- [ ] allocation budgets,
- [ ] late-game tier,
- [ ] heavy UI budget,
- [ ] node/leak budget,
- [ ] nightly soak,
- [ ] session diagnostics file,
- [ ] bounded operation ring buffer,
- [ ] release warning gate,
- [ ] missing runtime is hard failure,
- [ ] budget source tests,
- [ ] perf docs linked.

---

# 51. Integrated Ship Gate Pipeline

```text
Source data
   │
   ▼
CatalogPath authority
   │
   ├─ dev path
   ├─ env override
   ├─ executable-relative
   └─ PCK res://
   │
   ▼
Export staging script
   │
   ├─ canonical data tree
   ├─ locale overlays
   ├─ runtime dependencies
   └─ report metadata
   │
   ▼
Godot export
   │
   ▼
Artifact assertions
   ├─ binary
   ├─ PCK
   ├─ data count
   ├─ one authority tree
   └─ integrity
   │
   ▼
Exported boot smoke
   │
   ▼
Exported load smoke
   │
   ▼
Performance / leak budgets
   │
   ▼
CI artifact upload
```

---

# 52. Packaging Authority Decision Record

At implementation time, explicitly document:

```text
canonical resource data path:
loose deployed path:
PCK inclusion policy:
locale overlay path:
mod override precedence:
```

Do not leave these implied in scripts.

---

# 53. Data Count Contract

Define source groups:

- base gameplay JSON,
- localization overlays,
- generated-but-required reports if applicable.

Counts should compare like-for-like.

Do not fail because dev-only fixtures are intentionally excluded.

Create a manifest or glob contract.

Example:

```text
base_catalog_count = N
locale_overlay_count = M
required_export_count = N + M
```

---

# 54. Export Smoke CLI Contract

Preferred CLI shape:

```text
--boot-smoke
--load-smoke <fixture>
```

or existing command system.

Output should include machine-readable summary lines:

```text
BOOT_SMOKE_OK
DATA_ROOT=...
CATALOG_COUNT=...
DAY_TICK_OK
SAVE_OK
```

Load smoke:

```text
LOAD_SMOKE_OK
SAVE_SCHEMA=...
CHECKSUM_OK
BRIEFING_OK
```

Pin with contract tests similarly to version output.

---

# 55. CI Tier Strategy

Recommended:

## Per push

- Linux export,
- PCK/data assertion,
- boot smoke,
- key performance budget.

## PR/quality tier

- Windows export,
- load smoke,
- heavier UI/leak tests.

## Nightly

- 360-day soak,
- cross-platform deep smoke,
- larger performance matrix.

Use existing CI tier vocabulary.

---

# 56. Determinism and Locale Interaction

Since Plan 25 adds locale files, test exported builds under:

- English,
- pseudo locale.

Same seed/actions:

- same state,
- same checksum,
- same save data.

Only presentation changes.

This proves export packaging of localization does not affect simulation.

---

# 57. Failure Modes and Corrective Actions

## 57.1 CI export passes but player binary cannot find data

Cause:

- source-tree validation,
- bypass reader,
- artifact missing loose data.

Fix:

- 26A contract,
- exported boot smoke.

## 57.2 Two data trees in PCK

Cause:

- staging + source both exported.

Fix:

- one canonical packaging strategy,
- PCK tree assertion.

## 57.3 Windows build boots only in repo checkout

Cause:

- no data deployment or CWD assumptions.

Fix:

- parity script + artifact smoke.

## 57.4 Verification prints “PCK found” but file is empty/missing data

Cause:

- non-failing shell checks.

Fix:

- `test -s`,
- count and integrity assertions.

## 57.5 `ASHFALL_DATA` override works for some systems only

Cause:

- private reader bypass.

Fix:

- contract test + forbidden gate.

## 57.6 Performance test records regression but stays green

Cause:

- advisory result.

Fix:

- explicit budget.

## 57.7 CI becomes flaky due to tight time budget

Cause:

- no variance/headroom strategy.

Fix:

- generous initial hard budget,
- trend monitoring,
- ratchet later.

## 57.8 Session diagnostics leak private paths

Fix:

- redact/normalize,
- local-only policy.

---

# 58. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| packaging-path choice breaks mods | Low–Med | High | ADR + override tests |
| hidden data bypass remains | Medium | High | repo-wide forbidden gate |
| PCK inspection tooling brittle | Medium | Medium | simple manifest/count assertions |
| Windows parity lags | Medium | High | same contract matrix |
| boot smoke too shallow | Medium | Medium | day tick + save |
| load smoke fixture stale | Medium | Medium | pinned fixture/version policy |
| artifact size gate too strict | Medium | Low | delta-based review |
| perf budgets flaky | Medium | High | initial headroom |
| nightly soak too expensive | Low–Med | Low | scheduled only |
| diagnostics too verbose | Medium | Low | bounded ring buffer |
| runtime dependency check platform-specific | Medium | Medium | script-level parity tests |

---

# 59. Commit Strategy

## Commit C2[8].1 — Baseline + path inventory

- bypass list,
- source/export counts,
- current artifact behavior.

## Commit C2[8].2 — CatalogPath file helpers

- contract tests.

## Commit C2[8].3 — host reader migration

- known bypass sites.

## Commit C2[8].4 — forbidden path gate + boot root logging

### Gate: 26A complete

## Commit C2[8].5 — packaging strategy / `.gdignore` resolution

- canonical path decision.

## Commit C2[8].6 — Linux export script becomes CI authority

## Commit C2[8].7 — Windows export parity

## Commit C2[8].8 — hard artifact assertions + data integrity

## Commit C2[8].9 — exported boot smoke

## Commit C2[8].10 — exported load smoke

## Commit C2[8].11 — CI manifest + artifact reports + size tracking

### Gate: 26B complete

## Commit C2[8].12 — perf budget doc/config

## Commit C2[8].13 — allocation/late-game budgets

## Commit C2[8].14 — UI/leak budget

## Commit C2[8].15 — session diagnostics + ring buffer

## Commit C2[8].16 — release warning/runtime dependency gates

## Commit C2[8].17 — nightly soak + perf docs

### Gate: 26C complete

## Commit C2[8].18 — integrated ship closure

- Linux/Windows,
- EN/pseudo,
- load fixture,
- perf/leak reports.

---

# 60. Verification Checklist

Run per workstream and final closure:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/godot-export-linux.sh
bash scripts/ci/godot-export-windows.sh
bash scripts/ci/export-smoke-boot.sh
bash scripts/ci/perf-budget-gate.sh
bash scripts/ci/verify-fast.sh
```

Also execute exported load smoke.

---

# 61. Flagship Definition of Done

## 26A — Data path

- [ ] one resolver authority,
- [ ] catalog file/subdir helpers,
- [ ] env override test,
- [ ] all bypass readers migrated,
- [ ] forbidden path gate,
- [ ] one IO construction point,
- [ ] every catalog resolves under supported modes,
- [ ] boot logs data root.

## 26B — Export

- [ ] one canonical packed path,
- [ ] no duplicate data authority,
- [ ] Linux CI uses script,
- [ ] Windows parity script,
- [ ] binary/PCK fail-fast,
- [ ] deployed data count validated,
- [ ] artifact integrity selftest,
- [ ] exported boot smoke,
- [ ] exported load smoke,
- [ ] reports registered,
- [ ] artifact includes required data/runtime,
- [ ] size tracked,
- [ ] generated staging ignored.

## 26C — Budgets/diagnostics

- [ ] runtime budget documented,
- [ ] runtime gate can fail,
- [ ] allocation budgets,
- [ ] late-game scale tier,
- [ ] UI frame/construction budget,
- [ ] node/leak budget,
- [ ] nightly 360-day soak,
- [ ] session diagnostics,
- [ ] bounded recent-operation buffer,
- [ ] release warnings gated,
- [ ] runtime dependency hard failure,
- [ ] budget contract tests.

## Cross-system

- [ ] Plan 25 locale overlays packaged,
- [ ] exported EN and pseudo builds deterministic,
- [ ] known save fixture loads,
- [ ] artifact itself is tested,
- [ ] full verification green.

---

# 62. Closure Report Template

```markdown
## C2[8] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline
- Private data resolvers:
- Source JSON count:
- Packed JSON count:
- Duplicate authority count:
- Linux artifact size:
- Windows artifact size:
- Runtime-scale median/p95:

### 26A — CatalogPath
- File helpers:
- Readers migrated:
- Forbidden hits:
- IO backend:
- Dev resolution:
- ASHFALL_DATA resolution:
- PCK resolution:
- Boot data-root log:
- Result:

### 26B — Export
- Packaging strategy:
- Canonical PCK path:
- Duplicate-tree assertion:
- Linux script:
- Windows script:
- Binary/PCK checks:
- Data count:
- Artifact data integrity:
- Boot smoke:
- Load smoke:
- Artifact reports:
- Artifact size:
- Result:

### 26C — Budgets
- Budget source:
- Day-advance budget:
- Allocation budget:
- Late-game tier:
- UI budget:
- Node/leak budget:
- Nightly soak:
- Diagnostics:
- Ring buffer:
- Release warning gate:
- Runtime dependency gate:
- Result:

### Full Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Linux export:
- Windows export:
- Boot smoke:
- Load smoke:
- Perf budget:
- Verify fast:

### Final Ship Metrics
- Catalog readers bypassing authority:
- Packed authority copies:
- Export smoke failures:
- Load smoke failures:
- Day-advance median:
- Day-advance p95:
- Allocation/day:
- Node leak delta:
- Artifact sizes:

### Remaining Debt
- Packaging:
- Windows:
- Saves:
- Performance:
- Diagnostics:
```

---

# 63. Final Execution Directive

Execute Plan 26 as an artifact-truth repair.

The critical path is:

```text
one data resolver
→ every reader obeys it
→ one canonical packaged data tree
→ CI invokes real export scripts
→ checks fail hard
→ shipped binary boots
→ shipped binary loads
→ shipped build reports what it resolved
→ runtime budgets protect the artifact
```

Do not accept:

- source-tree green as exported-build evidence,
- an artifact that was never executed,
- an `if [ -f ]` log message as verification,
- Linux success while Windows has no data contract,
- duplicate data trees,
- advisory performance metrics with no threshold.

The strongest path rule is:

> **Exactly one implementation decides where the data authority lives, and every catalog reader uses it.**

The strongest export rule is:

> **CI must boot the exact exported artifact it uploads.**

The strongest packaging rule is:

> **A shipped PCK contains exactly one canonical copy of the data authority, verified by count and path assertions.**

The strongest performance rule is:

> **A measured regression is not a gate until the metric has an explicit reviewed failure budget.**
