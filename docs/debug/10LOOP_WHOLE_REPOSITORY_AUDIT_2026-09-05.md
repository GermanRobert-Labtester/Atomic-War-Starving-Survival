# ASHFALL whole-repository audit — 2026-09-05

**Mode:** read-only forensic audit; no production code, JSON authority, assets, or existing plan was changed.
**Anchor:** `e5adf24e`; the worktree was already heavily modified and was preserved.
**Companion plan:** `docs/remediation/plans/2026-09-05_whole_repository_200_task_audit_plan.md` (T001–T200).

## Current baseline

| Check | Result | Interpretation |
|---|---|---|
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-build --no-restore` | PASS, 7,613/7,613 | Strong Core regression baseline, but it does not prove full Godot composition. |
| `dotnet build Ashfall.csproj --no-restore` | PASS, 0 warnings / 0 errors | Compilation is clean despite broad nullable-warning suppression. |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 235 catalogs / 10,592 IDs | JSON shape/reference integrity passes; typed runtime deserialization is not fully covered. |
| `--7-day-smoke-selftest` | PASS, 10/10 | Deterministic happy-path save/resume baseline is green. |
| `--day1-selftest` | PASS | Scripted direct-session opening loop is green. |
| `--runtime-scale-selftest` | PASS | 360-day day-advance median was 8.3 ms; this does not cover full UI composition startup. |

## Confirmed release-impacting findings

1. **P0 — Expedition locations silently fail to load.** `ExpeditionCatalogLoader` deserializes `dangerLevel` as `int`, while all 151 `locations.json` and 38 `holdfast_locations.json` values are JSON numbers such as `8.0`. Its whole-file exception scope rejects both catalogs while `--content-utilization-selftest` still exits PASS. See plan T021–T029.
2. **P0 — Full UI composition is broken.** `--composition-root-selftest` reports disposed-label exceptions in `ExpeditionRadarPanel`, `FactionsNarrativePanel`, and `SkillMatrixPanel`, invalid `Margin` paths in instantiated stub panels, and unbounded recursion through the survivor exposure-location callback. See T031–T035 and T101–T108.
3. **P1 — Player-facing content is disconnected.** The current utilization artifact reports 61 `NO_LOADER` catalogs, including large quest, arc, radio, encounter, and ending families. See T026 and T079–T086.
4. **P1 — Asset coverage is incomplete.** The report resolves 512 of 1,330 IDs; `--asset-registry-selftest` fails for `item_hot_dust_drum`, `item_sludge_cake`, and `item_tailings_drum`. See T057–T069.
5. **P1 — Audio topology/resource tests fail.** `--audio-selftest` has 12 failures: absent declared buses, missing mess-hall loop WAV, non-distinct breaker cue, and out-of-tree playback. See T070–T078.
6. **P1 — Input authority conflicts with itself.** Both navigation-down and Holdfast status default to `S`; documents describe status as `U`; direct raw-key paths bypass the stated InputMap policy. See T141–T145.
7. **P1 — Generated contracts drift.** The save-store matrix is stale and the architecture graph omits registered `dynamic_quests`; triad/catch-policy checks otherwise pass. See T181–T185.

## Important quality and signal findings

- 290 Core/host files suppress `CS8618`, and project-wide warning suppression hides additional nullable flow warnings. A zero-warning build is therefore not a null-safety proof (T009–T015).
- The normal full shell can log Godot errors while narrow selftests report success. CI must treat engine errors, leaks, and timeouts as failures (T031, T107, T146–T150).
- The active narrative corpus contains repetitive formulaic prose, particularly in weather/ration records, and `epilogue_chronicle.json` contains five placeholder art IDs. Canonical survivor references for Elena and Marcus are current and should not be removed based on older reports (T087–T100).
- Accessibility smoke passes its selected checks but covers only 11 panels/61 controls and leaves CanvasItem/ObjectDB/font leaks at exit (T134–T140).
- The host campaign-fuzz verb is a pointer to Core tests, not a fuzz execution (T171).

## Environment observation

Some sandboxed Godot invocations crashed before running their test because Godot could not create `user://logs`. Re-running the same read-only narrative/accessibility checks with normal user-data access passed. This is an environment/CI-isolation concern, not evidence that the narrative or accessibility assertions failed; it is captured in T112 and T150.

## Recommended first execution packet

Start with **T021–T024**, **T031–T034**, and **T101–T103**. These repair the broken runtime authority, shell exceptions, recursion, and false-green reporting before expanding assets, content, or prose.
