# ASHFALL — Repository Enhancement Plan

Repository: `GermanRobert-Labtester/Atomic-War-Starving-Survival` ("ASHFALL" v1.1.0, Godot 4.7.1 .NET).
Scope: Performance optimisation, Game functionality, Life improvements, plus five follow-up tasks from a second evidence pass. All findings are grounded in inspected repository evidence; hypotheses are labelled.

---

## Part I — Repository Inventory (Verified)

| Area | Evidence | Notes |
|---|---|---|
| Engine | `project.godot`: Godot 4.7, C#, Compatibility renderer, 1920x1080, `max_fps=60` | Main scene `res://scenes/Main.tscn` |
| Host assembly | `Ashfall.csproj` compiles `src/**/*.cs` + `Assets/Ashfall.Core/**/*.cs`, `EnableDynamicLoading=true`, Sentry package referenced | Self-test sources (`Main.UiTests.*.cs`) ship in the runtime assembly |
| Core | `Assets/Ashfall.Core/` — ~90 engine-agnostic domain folders + ports (`IJsonSerializer`, `IFileIO`, `ISeededRng`, `ILog`) | Determinism contract: no `System.Random` in simulation (verified) |
| Host sprawl | ~150 files in `src/`; partial-class `Main` across ~90 files incl. ~40 plan-numbered leftovers; `HostCli.PanelTests.cs` 234,893 B | Largest files: `Main.UiPanels.cs` 83,872 B, `Main.CampaignOwners.cs` 79,252 B |
| UI | `src/UI/` 248 files; `ExpeditionPanel.cs` 66,949 B largest panel; 5 UI files override `_Process` | Runtime verification of per-frame cost outstanding |
| Perf infra | `Assets/Ashfall.Core/Performance/` (PerfSession, PerfStatistics, ScaleTier, WorkloadProfile); `docs/perf/BUDGETS.md` | Day-advance measured at 0.05–0.4% of budget — simulation is NOT the bottleneck |
| Tests | xUnit, `Ashfall.Core.Tests` (net9.0); suite reported 11,697/11,697; `TEST_POLICY.md` mandates focused runs, 180s limit via `scripts/run_test.sh` | |
| Debt ledger | `KNOWN_DEBT.md` — all listed entries RETIRED/RECONCILED | Governance functioning |
| L10n | `assets/l10n/`: `strings.csv` (58,116 B) with only `de` and `en` translations plus source template | |
| Clutter ledger | `POTENTIALCLUTTER.md` — read-only sweep of unreferenced forensic reports, unwired code, anomalies | Tag-only; authorises nothing by itself |

---

## Part II — Original Three Major Tasks

## Task 1 — Performance Optimisation (Frame Path and Assembly Weight)

### 1.1 Establish frame-path and startup baselines
- 1.1.1 Profile a five-minute idle session of `Main.tscn`; record frame time, `_Process` cost per node, draw calls.
- 1.1.2 Measure cold-start time to first interactive frame using existing `PerfSession` instrumentation.
- 1.1.3 Commit `docs/perf/FRAME_BASELINE.md` and `STARTUP_BASELINE.md`.

### 1.2 Audit UI `_Process` overrides
- 1.2.1 Classify per-frame work in `FeedbackPanel.cs`, `UiBackgroundCarousel.cs`, `DailyBriefingModal.cs`, `ExpeditionPanel.cs`, `SnapshotOrchestrator.cs`: animation, polling, or one-shot.
- 1.2.2 Convert polling to event-driven updates (day-advance / user-action signals).
- 1.2.3 Gate animations with `SetProcess(false)` when panels are hidden (check `Main.PanelLifecycle.cs`).
- 1.2.4 Re-profile; record before/after per file.

### 1.3 Remove self-test code from runtime assembly
- 1.3.1 Inventory `Main.UiTests.*.cs` and `HostCli.*Tests*.cs` compiled into the host; map required selftest entry points (`--runtime-scale-selftest` etc.).
- 1.3.2 Introduce `ASHFALL_SELFTEST` conditional-compilation constant; exclude test-only sources from export builds.
- 1.3.3 Verify documented selftest commands pass in dev configuration; confirm exported package lacks self-test symbols.
- 1.3.4 Measure exported assembly size and startup delta.

### 1.4 Investigate eager catalog loading
- 1.4.1 Instrument the ~50 `File.ReadAllText` catalog loaders with `PerfSession`.
- 1.4.2 Lazy-load the largest narrative catalogs only if measurement shows material startup cost.
- 1.4.3 Re-run `7day_smoke_selftest`.

**Verification:** profiler comparisons, export size/startup deltas, full xUnit suite green. **Risk:** Medium (build config change).

---

## Task 2 — Game Functionality Consolidation (Host Sprawl Reduction)

### 2.1 Map `Main` partial ownership
- 2.1.1 Table each `Main.*.cs` → Core domain folder.
- 2.1.2 Identify domain-duplicating files and dead plan leftovers.
- 2.1.3 Record map in `docs/architecture/`.

### 2.2 Rename and consolidate plan-numbered partials (no behavior change)
- 2.2.1 Rename each `Main.PlansNNN.cs` to its actual domain, one domain per commit.
- 2.2.2 Merge same-domain plan partials.
- 2.2.3 Full xUnit run after every rename.

### 2.3 Extract day-advance orchestration
- 2.3.1 Extract from `Main.CampaignOwners.cs` into a `*HostSession` following the existing `src/Host/` pattern.
- 2.3.2 Route events via `IEventBus` per README contract.
- 2.3.3 Reuse `SubsystemManifest` orchestration; no new architecture.

### 2.4 Split `HostCli.PanelTests.cs` (234 KB)
- 2.4.1 Split by selftest command under existing `HostCli.*` naming.
- 2.4.2 Verify every documented selftest command resolves and passes.

**Verification:** zero test changes; 11,697/11,697 green per step. **Risk:** Low (renames) / Medium (extraction; check `MainTriadDriftGateTests`).

---

## Task 3 — Life Improvements (Player and Developer QoL)

### 3.1 Verify and harden the autosave path
- 3.1.1 Inspect `Main.SaveOrchestrator.cs` / `SaveLoadHostSession.cs`: confirm autosave frequency and crash window (currently unverified).
- 3.1.2 Add rolling save-slot rotation (e.g. three slots); `SaveChecksum` already detects corruption.
- 3.1.3 Add recovery prompt when primary slot fails checksum but a backup validates.
- 3.1.4 Add save/load round-trip plus deliberately corrupted-slot tests.

### 3.2 Improve session resumption
- 3.2.1 Persist last-open panel and campaign day via existing `CaptureState`/`RestoreState`; restore on load.
- 3.2.2 Surface a "continue campaign" entry point (verify `Main.GameFlow.cs` first).

### 3.3 Reduce repository navigation cost
- 3.3.1 Consolidate the six near-identical agent-instruction files (`.clinerules`, `.cursorrules`, `CLAUDE.md`, `CODEX.md`, `ANTIGRAVITY.md`, `.windsurfrules`) into one canonical source.
- 3.3.2 Move completed coordination records (`A1_*`, `WAVE9_*`, `SESSION_HANDOFF.md`) into `docs/archive/`.
- 3.3.3 Update `README.md` folder map; run doc-index checks (`docs/INDEX.md` staleness was a past standing failure).

**Verification:** new xUnit save-rotation/corruption tests; crash-kill reproduction; doc link check. **Risk:** Medium (persistence is high-risk; additive envelope change with schema-version bump).

---

## Part III — Five Additional Tasks (Second Evidence Pass)

## Task 4 — Sentry Crash-Reporting Configuration Audit

**Finding (HIGH CONFIDENCE):** 51 files reference `Sentry` across host UI, host sessions, and even Core narrative catalogs, and the package is referenced in `Ashfall.csproj`. Whether it is correctly configured (DSN, release tagging, disabled-in-debug, PII scrubbing) is not documented in the inspected material.

**Why it matters:** unconfigured or over-broad crash reporting either silently loses crash data or leaks player data; references deep inside `Ashfall.Core` suggest possible architecture bleed (Core should depend on nothing outside its tree per README).

### Steps
- 4.1 Inventory every Sentry call site.
  - 4.1.1 List the 51 files; classify capture calls (exception, message, breadcrumb).
  - 4.1.2 Identify any call sites inside `Assets/Ashfall.Core/` and route them through the existing `ILog` port instead, preserving the engine-agnostic contract.
- 4.2 Verify configuration.
  - 4.2.1 Locate Sentry initialisation; confirm DSN source, environment, and release version match `config/version` from `project.godot`.
  - 4.2.2 Confirm reporting is disabled in offline/export builds if unintended.
- 4.3 Add a crash-report test.
  - 4.3.1 Add a selftest that deliberately throws inside a guarded subsystem and confirms the Sentry hook receives it (test SDK, no network).
  - 4.3.2 Document the intended behaviour in `docs/telemetry/` (existing folder).

**Verification:** static call-site inventory; offline throw test. **Risk:** Low (read-first; changes only if misconfiguration is proven).

## Task 5 — LINQ and Closure Allocation Sweep in Core

**Finding (HIGH CONFIDENCE):** `docs/perf/RUNTIME_SCALE_BASELINE.md` already documented closure-allocation bottlenecks in `CampaignDayCoordinator`, `LocationEvolutionSystem`, `WildlifeMigrationSystem`, and `SurvivorRosterSystem`. Code search shows 48 Core files still contain `.Select(` and similar LINQ calls; the same pattern plausibly persists in day-advance paths not yet swept.

**Why it matters:** per-day tick allocation (currently 2,640 B median) is inside budget, but allocation churn grows with roster/journal scale (858,720 B at 360-day stress) and GC pauses are frame-visible in the Godot host.

### Steps
- 5.1 Extend the hot-path analysis.
  - 5.1.1 Grep the 48 `.Select(` files and classify each call by execution frequency (per-frame, per-day, per-load).
  - 5.1.2 Shortlist per-day or per-call sites in day-advance owners and tick loops.
- 5.2 Replace hot closures only where frequency justifies it.
  - 5.2.1 Replace predicate-closure `Find/Exists/Sort` calls with cached comparers or indexed lookups, mirroring the fixes already recorded in `RUNTIME_SCALE_AFTER.md`.
  - 5.2.2 Keep cold-path LINQ untouched — change only measured or per-tick sites.
- 5.3 Re-measure.
  - 5.3.1 Run `--runtime-scale-selftest`; compare allocation medians against the recorded baseline artefacts (`artifacts/runtime-scale-results.json`).
  - 5.3.2 Record deltas in `docs/perf/RUNTIME_SCALE_AFTER.md`.

**Verification:** before/after allocation telemetry; budget gates stay green. **Risk:** Low (local, measurable changes). Constraint: do not trade readability for microscopic gains.

## Task 6 — Localisation Completeness and Integrity

**Finding (VERIFIED structure / HYPOTHESIS on coverage):** `assets/l10n/` holds a 58 KB `strings.csv` but only two translations exist (`de` 12,135 B and `en` 22,860 B, versus the 23,255 B source template). Coverage gaps are plausible but unverified.

**Why it matters:** missing keys render as raw IDs in the player-facing UI for the `de` locale; there is no documented gate preventing new English strings from shipping without German translations.

### Steps
- 6.1 Measure actual coverage.
  - 6.1.1 Parse `strings.csv`; compute per-locale missing/empty key counts.
  - 6.1.2 Sweep `src/UI/` for hard-coded UI strings that bypass the CSV entirely.
- 6.2 Add a l10n gate test.
  - 6.2.1 Add an xUnit test asserting every `strings.csv` key has non-empty `en` and `de` values, following the repo's existing loader-gate pattern (e.g. `AllMapNodes_ExistInLocationsCatalog`).
  - 6.2.2 Extend the test to fail on orphan keys no longer referenced in code, if a reference list can be derived cheaply.
- 6.3 Fill verified gaps.
  - 6.3.1 Translate or temporarily mark missing keys as English fallbacks, per `docs/i18n/` conventions (existing folder — inspect first).

**Verification:** l10n gate test in the suite; in-game spot check of the `de` locale. **Risk:** Low.

## Task 7 — Clutter Disposition per `POTENTIALCLUTTER.md` Protocol

**Finding (VERIFIED):** `POTENTIALCLUTTER.md` is a tag-only sweep ledger listing unreferenced forensic reports (`docs/forensics/BUG_HUNT_25..29`, `PLANS_*` reports), unwired code, and anomalies. The file explicitly states it authorises nothing and awaits owner disposition. `KNOWN_DEBT.md` contains no disposition entries for these items.

**Why it matters:** several hundred KB of unreferenced forensic and coordination material inflate the repository and mislead future agents; the repo's own governance requires an explicit recorded decision before deletion or archiving.

### Steps
- 7.1 Triage the sweep ledger.
  - 7.1.1 For each `POTENTIALCLUTTER.md` entry, re-run the zero-reference grep to confirm it is still unreferenced at current HEAD.
  - 7.1.2 Classify: archive (historical value), delete (pure clutter), promote (turns out to be wired).
- 7.2 Execute dispositions.
  - 7.2.1 Archive retained items to `docs/archive/` (the existing structure).
  - 7.2.2 Delete confirmed clutter in one dedicated commit with the ledger as justification.
  - 7.2.3 Record every decision as a RETIRED row in `KNOWN_DEBT.md`, matching the ledger's own expected contract.
- 7.3 Close the ledger.
  - 7.3.1 Move `POTENTIALCLUTTER.md` itself into the archive once emptied.
  - 7.3.2 Verify `docs/INDEX.md` links remain valid.

**Verification:** grep confirms no dangling references; suite green; ledger governance trail in git history. **Risk:** Low (governance-conformant; reversible via git).

## Task 8 — Audio Subsystem Audit and Coverage

**Finding (VERIFIED structure / HYPOTHESIS on coverage):** the entire audio surface consists of `src/Audio/AudioManager.cs` plus one composition-root reference; `assets/audio/` exists as the authored source. Bus layout, volume persistence, and whether UI/gameplay events are actually wired to sound are not documented in the inspected material (the `docs/audio/` folder exists but was not opened).

**Why it matters:** a single 2-reference audio surface in a 248-panel UI strongly suggests most gameplay events produce no audio feedback — a direct "life of the game" gap; misconfigured buses also cause volume controls that do not persist.

### Steps
- 8.1 Establish current audio reality.
  - 8.1.1 Read `AudioManager.cs` and `docs/audio/`; inventory bus layout, volume persistence, and which gameplay events emit sound.
  - 8.1.2 Cross-reference the `IEventBus` event list against registered sound hooks; produce a wired/unwired table.
- 8.2 Fill the highest-value gaps.
  - 8.2.1 Wire the top player-experience events first (day advance, expedition resolve, verdict, low-supply warning) using the existing host-session pattern — no new architecture.
  - 8.2.2 Persist master/music/SFX volumes in the existing settings save path; add a settings round-trip test.
- 8.3 Gate against regressions.
  - 8.3.1 Extend the existing `AudioSelfTest.cs` to assert the newly wired events resolve to valid streams from `assets/audio/`.
  - 8.3.2 Record the audio contract in `docs/audio/`.

**Verification:** AudioSelfTest extended; manual in-game audio pass; settings round-trip test. **Risk:** Low/Medium (new wiring follows existing patterns; asset availability must be confirmed first).

---

## Recommended Execution Order

Task 1 (baselines) → Task 5 (allocation sweep, uses Task 1 instrumentation) → Task 2 (consolidation, protected by baselines) → Task 4 (Sentry audit) → Task 7 (clutter disposition) → Task 3 (player QoL) → Task 6 (l10n) → Task 8 (audio).

## Constraints

- No behavior change except where a task explicitly adds player-facing value (Tasks 3, 6, 8).
- Persistence changes require schema-version bump and old-save compatibility tests.
- Full-suite runs require a documented reason per `TEST_POLICY.md`; default to focused runs via `scripts/run_test.sh`.
- Items marked HYPOTHESIS require measurement before any implementation.

---

## Part IV — What NOT to Do (Negative Scope)

These prohibitions protect the repository's verified strengths and contracts. Violating any of them is a defect even if a task appears to progress.

### Do not touch the working systems

- Do NOT optimise the day-advance simulation. It is measured at 0.05–0.4 percent of its budget (`day_advance_360d` ~12.7 ms against a 30,000 ms budget). There is nothing to gain and regression risk to add.
- Do NOT modify the determinism infrastructure: `ISeededRng`, the forbidden-API gate tests (`ForbiddenCoreApiGateTests`, `DeterminismGuardTests`), or the no-`System.Random` simulation contract. These are verified intact and load-bearing for the entire test suite.
- Do NOT alter the save checksum/envelope contracts (`SaveChecksum`, `SaveEnvelopeDetection`) in place. Any persistence change must be additive with a schema-version bump and old-save compatibility tests, per the repo's established pattern (see `DEBT-PLAN34` history).
- Do NOT re-open retired debt items. Every entry in `KNOWN_DEBT.md` carries a promotion condition ("do not re-open without a new contract change"). Re-opening without that trigger violates repo governance.

### Do not invent work or scope

- Do NOT exceed eight tasks or pad with cosmetic items ("add more comments", "improve error handling", "add logging", "make code modular"). If a task completes with fewer substeps than planned, that is acceptable; do not manufacture substeps.
- Do NOT turn refactors into features. Tasks 1, 2, 4, 5, and 7 must produce zero gameplay behavior change. Any behavior-altering discovery becomes a new finding for user approval, not a silent edit.
- Do NOT add architecture (event bus, DI framework, service locator, ECS, new manager/coordinator classes). The repo's existing `IEventBus`, `SubsystemManifest` orchestration, and `*HostSession` patterns are sufficient; all plans explicitly reuse them.
- Do NOT split one coherent fix into multiple tasks, and do NOT split files merely because they are long. File size alone is not a defect (per the ~150-file `Main` partial evidence, consolidation is justified only by domain mapping, not size).
- Do NOT delete anything based on `POTENTIALCLUTTER.md` alone. The ledger states explicitly that it authorises nothing. Deletion requires re-verified zero references at HEAD, an explicit disposition decision, and a `KNOWN_DEBT.md` record (Task 7 protocol).

### Do not break the process contracts

- Do NOT run the full test suite by default. `TEST_POLICY.md` mandates focused runs via `scripts/run_test.sh` (modified file plus affected regional tests, normally below 100 cases). Full-suite runs require an explicit documented reason and a dedicated window.
- Do NOT create tests to hit a count. Tests exist only for confirmed defects, new public contracts, save/load, determinism, lifecycle, mutation, state transitions, or cross-system consequences — never merely because a plan names a feature.
- Do NOT guess at runtime behavior. Per-frame cost of UI `_Process` overrides, startup catalog cost, and autosave crash windows are HYPOTHESES until measured. Do not claim a bottleneck or a fix without the profiler or `PerfSession` numbers first.
- Do NOT claim "fixed" or "done" from compilation alone. Compilation is necessary, not sufficient; every task's verification section defines what must actually pass.
- Do NOT bypass the approval workflow. Findings → plans → user selects → implement → verify. Substantial refactors (Task 2.3), save migrations (Task 3.1), and deletions (Task 7.2) wait for explicit approval.

### Do not regress the player's experience

- Do NOT change combat difficulty, loot frequency, AI decisions, timing, progression, or encounter rules as a side effect of any performance or consolidation work. If behavior might change, state it explicitly before proceeding.
- Do NOT trade correctness or readability for microscopic performance gains. Cold-path LINQ stays untouched in Task 5; only measured or per-tick sites change.
- Do NOT introduce destructive save-schema changes without migration analysis (Task 3 risk statement).
- Do NOT ship broken locale fallback. Task 6 must never leave a `de` key resolving to a raw ID in place of a deliberate English fallback.