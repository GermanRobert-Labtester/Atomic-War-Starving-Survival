# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Repository Enhancement Tasks 4-8 — Sentry Audit, LINQ Sweep, L10n, Clutter, Audio

STATUS: APPROVED BY USER

## Outcome (2026-09-27)

| Task | Result | Commit / record |
|---|---|---|
| 4 Sentry | Premise disproven; SDK unused. Dangling package removal blocked by the `Ashfall.csproj` claim (`DEBT-SENTRY-DANGLING-PACKAGE`) | `docs/telemetry/SENTRY_CRASH_REPORTING_AUDIT_2026-09-27.md` |
| 5 LINQ | 2 sites changed (Owners cached view, linear trend scan); 3 left unchanged on cadence evidence; runtime-scale selftest skipped (dirty host csproj) | `873870de4`, `docs/perf/LINQ_CLOSURE_SWEEP_2026-09-27.md` |
| 6 L10n | 184 German fills, 5 malformed rows repaired, full-catalog gate test | `0f8dccdd0`, `docs/i18n/L10N_TASK6_COVERAGE_RECORD_2026-09-27.md` |
| 7 Clutter | All findings re-verified; nothing left to delete; KNOWN_DEBT rows; 9 HOLDs keep POTENTIALCLUTTER.md in place | `docs/hygiene/CLUTTER_DISPOSITION_2026-09-27.md` |
| 8 Audio | Verdict gap has no Core event; rationing events have no producer (`DEBT-RATIONING-CRISIS-NO-PRODUCER`); volume persistence already correct. No code change | `docs/audio/AUDIO_TASK8_COVERAGE_RECORD_2026-09-27.md` |

Divergences from the approved scope: Task 5 narrowed from five sites to two;
Task 8 wired nothing; `docs/hygiene/CLUTTER_DISPOSITION_2026-09-27.md` is an
added path under the Task 7 claim.

Authorization: user assigned Tasks 4-8 of
`Repo_enhancement_plans/ashfall-repository-enhancement-plan-8-tasks.md` in this session
(2026-09-27): "Tasks 1-3 are being worked on, but 4-8 you can take." Tasks 1-3 belong
to the active `claim-performance-host-qol-2026-09-27` lane and are untouched by this
plan. Every premise below was re-verified at current HEAD by a read-only sweep agent
before implementation (Rule 7); premises that failed verification are recorded as
such, and the task was re-scoped to the verified reality.

## Bounded outcome

Execute enhancement-plan Tasks 4, 5, 6, 7, and 8 only. Zero gameplay behavior change
for Tasks 4, 5, 7; player-facing value only for Task 6 (locale fills) and Task 8
(audio gaps, per the plan's own scope). Each task lands as its own small commit
(user directive: "commit in smaller chunks").

## Non-goals (hard, from the active lane claim and the plan's negative scope)

- All `claim-performance-host-qol-2026-09-27` paths: `.ai/state.md` existing sections,
  `.ai/plans/performance-host-qol-2026-09-27.md`, `INTEGRATION_PLANS.md` existing rows,
  `src/UI/FeedbackPanel.cs`, `UiBackgroundCarousel.cs`, `DailyBriefingModal.cs`,
  `ExpeditionPanel.cs`, `SnapshotOrchestrator.cs`, `src/Host/FrameStartupProfiler.cs`,
  `scenes/Main.tscn`, `docs/perf/` existing measurement records, `Ashfall.csproj`,
  `src/Host/HostCli.cs`, `src/Main.Application.cs`, `src/Main.UiHandlers.cs`,
  `src/Assets/AssetRegistry.cs`, `scripts/ci/export-build.sh`, Main plan-partial
  renames, `src/Main.Campaign.cs`, `src/Main.CampaignOwners.cs`, `src/Main.Holdfast.cs`,
  `src/Host/CampaignDayHostSession.cs`, HostCli.PanelTests split destinations,
  `docs/architecture/MAIN_DECOMPOSITION_MAP.md`, `Assets/Ashfall.Core/Save/SaveSlotService.cs`,
  `src/Host/SaveLoadHostSession.cs`, `src/Main.SaveOrchestrator.cs`,
  `src/Main.GameFlow.cs`, recovery UI and their save tests. Edits to
  `WORKTREE_OWNERSHIP.md` are append-only below the existing claim sections.
- No new architecture (no new event bus, DI, service locator, manager classes).
- No full-suite test runs; focused runs via `bin/run-scoped-tests` only.
- No deletion based on `POTENTIALCLUTTER.md` alone; Task 7 follows its own protocol.
- The shared `Repo_enhancement_plans/ashfall-repository-enhancement-plan-8-tasks.md`
  file is renamed/moved to `Integrated_plans/` only when all eight tasks (including
  the Tasks 1-3 lane) are integrated, per `ENHACEMENT_AGENTS.md`; my task records
  carry their own completion markers.

## Task 4 — Sentry crash-reporting audit (PREMISE DISPROVEN → audit record only)

Verified reality (sweep-agent audit, re-confirmed by implementer):
- The Sentry.io SDK is never used: zero `SentrySdk.Init`/`CaptureException`/
  `AddBreadcrumb`/`using Sentry` anywhere. Every `Sentry` match in C# source is a
  local gameplay symbol (guard sentry: `AssignSentry`, `hasSentry`,
  `CampSentryDetectionBonus`, `GetBySentry`, display strings). No Core architecture
  bleed — Task 4.1.2 (route Core call sites through `ILog`) has ZERO candidates.
- No configuration exists to verify: no DSN, no Environment/Release/Debug/scrubbing,
  no crash hook at all.
- One real defect: dangling package reference — `Ashfall.csproj:50`
  (`<PackageReference Include="Sentry" />`), pinned `Directory.Packages.props:16`
  (6.9.0), restored into every build including exports, never activated.

Deliverable: `docs/telemetry/SENTRY_CRASH_REPORTING_AUDIT_2026-09-27.md` (written).
Disposition recorded there: removal is preferred (zero behavior change) but is
blocked by the active lane's `Ashfall.csproj` claim; wiring a real crash pipeline is
a feature requiring explicit user approval (DSN source, privacy, export gating) and
is NOT silently implemented. No code change from Task 4.

## Task 5 — LINQ/closure allocation sweep (premise verified; REGRESSION confirmed)

Verified reality (sweep-agent audit, all file:line evidence):
- The Plan 82 fixes partially REGRESSED: `CampaignDayCoordinator.Owners` rebuilds a
  `List` per invocation again (`Campaign/CampaignDayCoordinator.cs:77-84`); the
  `List.Find` predicate closures are back in `LocationEvolutionSystem` (`:51` +
  `LocationEvolutionSystem.Live.cs:53`), `WildlifeMigrationSystem` (`:50`, `:67`,
  `.Live.cs:158`), and `SurvivorCatalog.cs:89`/`:162` (non-static Sort lambda).
  Surviving fixes: `Array.Empty` day-owner events, static lambdas, indexed
  `FindDefinition`/`_byId`.
- Hot sites in CLEAN files, ranked by cadence/multiplier:
  `EmergencyAlertSystem.TickHour:185` (per-tick hourly — hottest);
  `CampaignDayCoordinator.Owners:77-84` (per-day + per-query);
  `MutationSystem.TryMutateSurvivor:169-174` (per survivor per day, 4 Where chains);
  `HealthHistorySystem.RecordDailyHealthTrend:306-309` (per survivor per metric per
  day, Where+OrderByDescending+FirstOrDefault); `RailwaySystem.TickDay:552-554`
  (Where+Select+OrderBy per day). Per-day chains also exist in Agriculture/Espionage/
  Quest/FluidLogistics/Echo/ShelterThermal/WildlifeEcosystem and
  `DailyBriefingReportBuilder:547-619` (8 method-group Sort delegate allocations).
- Measured budget reality: per-day tick allocation is ~2,640 B median — inside
  budget; the harness gates in `src/Host/PerformanceSelfTest.cs:55-100` are the
  loose originals (2000/12000/30000 ms; 500 ms save; 5 MB alloc; 20 MB retained).
  Tightening gates is NOT in this task's scope.

Scope (frequency-justified, clean files only, cold-path LINQ untouched, no
readability sacrifice):
1. `CampaignDayCoordinator.Owners` — restore the cached-view pattern (invalidated on
   Register/Unregister) after verifying no caller mutates the returned list.
2. `EmergencyAlertSystem.TickHour` — indexed loop over unresolved alerts.
3. `MutationSystem.TryMutateSurvivor` — indexed loops replacing the Where/OrderBy
   chains.
4. `HealthHistorySystem.RecordDailyHealthTrend` — reversed index scan replacing
   Where/OrderByDescending/FirstOrDefault.
5. `RailwaySystem.TickDay` — for-loop + ordinal ordering replacing the chain.
Verification: focused Core tests per touched system; `--runtime-scale-selftest`
before/after; deltas recorded in new `docs/perf/LINQ_CLOSURE_SWEEP_2026-09-27.md`.
Determinism guard: preserve `StringComparison.Ordinal` traversal order exactly;
`src/Host/HostCli.cs` (dispatch) is dirty/lane-owned — selftest run is read-only
use of the built host, no edit.

## Task 6 — Localisation completeness (premise verified, coverage measured)

Verified reality (sweep-agent audit):
- `assets/l10n/strings.csv`: header `key,en,de,source`; 359 data rows; 0 duplicate
  keys; `en` 100% filled; `de` 176/359 filled (49%), 183 empty `de` cells
  (`discovery.*` 116, `settings.*` 27, `tutorial.*` 15, `ui.*` 15, `warning.*` 6,
  `codex.*` 4). Empty cells already resolve to English at runtime through the
  fallback chain — the "raw IDs in the de locale" hypothesis is disproven; the real
  gap is missing German coverage.
- Loader: Core `LocalizationService` (authority) + `src/Localization/AshfallLocalization.cs`
  (lazy adapter); locales `en`/`de`/dev-only `pseudo`; locale persisted in
  `user://settings.json`. Godot CSV importer path is vestigial and generates an
  inert, unregistered bogus `strings.source.translation` (recorded as a finding, not
  fixed here — renaming the `source` column would require a coordinated loader +
  re-import change).
- Conventions: `docs/L10N_CONTRACT.md` (English fallback authority; placeholder
  parity; no duplicate keys). Fills follow it: proper German for translatable keys,
  deliberate English fallback for the rest — never a raw key.

Deliverables:
1. Fill all 183 empty `de` cells in `assets/l10n/strings.csv`.
2. New gate test `Ashfall.Core.Tests/Localization/StringsCsvLocaleGateTests.cs`:
   header contract, key uniqueness, non-empty `en` for every row, non-empty `de`
   for every row, `{N}` placeholder parity between `en` and `de`. Mirrors the
   `LocalizationPilotTests` RepoRoot() walk-up + ParseCsv pattern and the
   aggregate-message assert pattern; does NOT duplicate the pilot-prefix or drift
   gate checks.
3. Findings recorded (not fixed, owned elsewhere): the ratchet is 3 literals above
   its 603 baseline from the concurrent UI lane's dirty files (theirs);
   `artifacts/l10n-inventory.json` is stale against concurrent state (theirs);
   Core's in-code EN/DE micro-location duplicates are a sync hazard (recorded);
   the `source` column quirk (above).

Orphan-key gating (6.2.2) is skipped with reason: keys are constructed dynamically
(`wildlife.{category}.{id}.{field}`, `{tutorialId}.title`) so a cheap static
reference list cannot be derived reliably.

## Task 7 — Clutter disposition (audit in flight; scope fixed by the protocol)

Only after re-verified zero references at HEAD per entry: ARCHIVE to the existing
`docs/archive/` structure, DELETE pure clutter in one dedicated commit with the
ledger as justification, PROMOTE only with wiring evidence, HOLD anything dirty or
lane-owned. Every decision recorded as a RETIRED row in `KNOWN_DEBT.md` matching
its existing format. `POTENTIALCLUTTER.md` itself moves to the archive once
emptied. Section completed when the clutter audit report is applied.

## Task 8 — Audio subsystem (audit in flight)

Premise is already partially stale: `src/Audio/` contains 14 C# files (cue catalog,
event/state bridges, controllers, selftest, settings), not a "single 2-reference
surface". The audit's wired/unwired table decides the real gaps; only
`AudioManager.cs` (clean) and other clean files in that tree are editable. Volume
persistence verdict, event-wiring gaps, and `AudioSelfTest` extension land per the
audit's evidence, following existing patterns only.

## Verification summary

- Task 6: `bin/run-scoped-tests -ref HEAD` (localization targets) green; de fills
  verified against the gate test.
- Task 5: focused runtime-scale selftest before/after; allocation deltas recorded;
  budget gates stay green.
- Task 7: re-run zero-reference greps post-move; focused suite spot checks; ledger
  governance trail in git history.
- Task 8: focused audio/settings tests; `AudioSelfTest` extension green.
- Common: host build 0 errors for every code chunk; each task committed separately;
  docs records committed in one batch; `generate-docs-index.py` state handled by the
  pre-commit guard.

## Handoff

Progress and findings are recorded in this plan file plus a final `.ai/state.md`
append section (below the active lane's sections). Commit list is reported to the
user with exact IDs.
