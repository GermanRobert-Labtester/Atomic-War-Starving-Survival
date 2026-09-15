// SPDX-License-Identifier: MIT
# C2[2] Closure Report — Legibility, Cause, Guidance, and Confirmation

> Plan: `C2_planintegration[2].md` (Plan 17 — Legibility: Cause, Effect, and
> Guidance). Continuity Wave 1. Executed 2026-09-15 in a single builder
> session under the foreman's "implement non-stale parts only" directive.

## Erratum honored

The obsolete 17A substeps ("18 mute day owners" / add emission plumbing) were
**not** implemented. Re-measurement this session confirms the corrected
diagnosis and finds it broader than stated:

| Metric | Plan's re-audit claim | Measured 2026-09-15 |
|---|---|---|
| Emitted kinds (src/ + Core) | 27 | **70** |
| Builder-handled kinds | 7 of 27 | **31** |
| Emitted-but-unhandled (silent drop) | 20 | **50** |
| Handled-but-never-emitted | 6 | 11 |

`BuildFromDayEvents` had a closed switch with no default case; the primary
briefing branch is taken (`Main.Campaign.cs` `ShowBriefingForDay` →
`BuildFromDayEvents(args.AllEvents())`). Plan 31 (semantic-kind authority)
does **not exist yet** anywhere in the repository — no docs, no vocabulary
infrastructure. Per plan §4.4, C2 therefore shipped the consumer-side
no-silent-drop repair (§6.3's "documented generic/default representation")
and left producer-side vocabulary standardization to Plan 31.

## Repository

- Start commit: `87b199b2` (feat: seal signed-map implement packages) —
  worktree heavily dirty with concurrent unclaimed work (see Environment).
- Branch: user's working branch (uncommitted-worktree session).
- Working tree: shared with another active writer (in-flight Economy +
  TravelingCaravan work). Their files were never touched.

## Baseline

- Core build: PASS 0/0 · Core tests: focused runs only (full suite skipped
  per user directive — the 11k-case suite was NOT run) · Host build: blocked
  only by the neighboring writer's in-flight `TravelingCaravanHostSession.cs`
  (pre-existing, not this package's files) · data-integrity: PASS.
- Event kinds emitted / handled / unknown: 70 / 31 / 50 (table above).
- Audio selftest totals, cue count, bus count: not re-measured this session
  (see 17C classification below — most 17C audio premises proved stale).
- Onboarding journey save/load: already implemented (`OnboardingSaveStore`,
  `RestoreOnboardingFromDisk`) — plan premise stale.
- Geiger loop: `StartGeiger`/`StopGeiger` existed with **zero production
  callers** — worse than the plan's "stop gap": the loop was never started
  at runtime either. Repaired this session.

## Plan 31 Compatibility

- Semantic authority: `DayEventVocabulary` (Core, documented) — explicitly
  NOT the Plan 31 authority; Plan 31 may replace it (header contract).
- No-silent-drop test: `DayEventVocabularyTests` (8 cases) +
  `DayEventParitySourceGateTests` (structural default-case pin + per-kind
  coherence against live producer sources).
- Producer/consumer parity: `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`
  (generated, 81 kinds, gate-enforced freshness).
- Briefing route parity: existing briefing panel; `DeepLinkRoute` field
  already exists on `DailyBriefingEntry` (consumer seam ready).
- Event-log parity: existing `events_log` route retained; no second history
  store created.
- Owner-failure visibility: `ShowBriefingForDay` now injects a visible
  Warnings entry when `args.HasFailures` (§6.8) — technical detail stays in
  logs; UI no longer presents a failed owner tick as a quiet day.
- Result: **17A-S acceptance gate satisfied** (minus Plan 31 itself, which
  remains a separate unregistered plan).

## 17C — Confirmation (executed subset)

- Radiation end signal (Phase G): `RadiationSystem.OnExposureStarted` /
  `OnExposureEnded` — fired exactly on the active↔inactive transition per
  survivor; transient tracking set (private, never persisted, verified by
  test); unregister/death closes the transition; `IsExposureActive` query
  added. 7 test cases.
- Geiger lifecycle: `AudioEventBridge` binds the new events; loop starts
  once on first active exposure, no restart spam, stops on last end,
  stops defensively on rebind/Dispose (session replacement, §14.3/§14.4).
  Restore never replays historical begin/end cues.
- Audio/simulation checksum parity: structurally preserved — Core events
  are observational; no RNG, no state mutation, no new save fields.
- Result: **Phase G + geiger gate satisfied.**

## 17C — phases audited stale (premises already satisfied; not re-implemented)

Reconnaissance found the audio layer far more complete than the plan's
source assumed. Classified per §43.1:

| Phase | Finding | Classification |
|---|---|---|
| B (shared UI cues) | `AshfallUiHelpers` + `AudioCueCatalog.UiConfirm`; `OpenPlayerPanel` plays one confirm cue per route open | CLOSED (existing) |
| C (ambience) | `SurfaceAmbienceController` (weather/location resolution, loop lifecycle, StopAllAmbiences) + `ShelterAudioController` (generator/ventilation/low-power loops via `ReactiveAmbienceEvaluator`) exist and are subscribed to live systems | CLOSED (existing) — §10.5 test matrix not re-verified |
| D (music) | `AudioManager` + controllers handle music cues through the catalog; extension-mismatch guard lives in the selftest | CLOSED (existing) — transition matrix not re-verified |
| E (item pickup) | `AudioEventBridge.OnLootAdded` and inventory-path cues exist; full acquisition-path parity sweep NOT performed | PARTIAL — remaining blocker: per-path (craft/trade/gift/scavenge/autopsy) confirmation audit |
| F (danger cues) | Combat/disease/weather/hazard cues already event-driven in `AudioEventBridge` | CLOSED (existing) |
| H (asset de-dup) | Not re-audited this session | DEFERRED — needs the audio audit pass |
| I (alert ducking/concurrency) | No ducking exists in `AudioManager` — the plan's premise is REAL and unimplemented | DEFERRED — real gap, needs an audio-engineering package (bus-volume orchestration + concurrency policy + tests) |
| J (bus topology/settings) | `AudioBusNames` + settings recovery flow exist; 12-bus validation sweep not re-run | PARTIAL — needs the §17.2 sweep |

## 17B — Guidance (executed subset + audited stale)

- Phases A/B/C (route/reachability): **stale** — `guidance` is registered
  (`PanelRegistryBootstrap`, PanelGroup.Dashboard), has a dashboard nav
  button ("GUIDANCE"), full bind/open/close actions, veteran-mode gate
  (`TutorialMode == 2` → status notice, panel never forced), show-me-where
  bridge (`OnShowMeWhereRequested → OpenPlayerPanel`), and a save store.
  `_Ready()` no longer permanently owns visibility — routes call `Show()`.
- Phase D (F1 input): **executed** — new `ashfall_guidance` action. The
  plan's requested F1 binding **collides** with pre-existing `ashfall_help`
  (tutorial panel, F1). Bound to **F2** (nearest free function key) and
  documented in `AshfallInputActions`. Runtime: F2 toggles guidance
  (open when closed, close when open — §24.2 reopen semantics; never
  permanently disables). Registered in `project.godot` + `EnsureActionsRegistered`.
- Phases E–L: largely stale (visibility ownership, show-me-where, save/load,
  status bar, assistance-off already implemented). Not re-verified at the
  plan's full test-matrix depth this session: snapshot fixture, teach-before-
  demand matrix, subscription-leak soak, and a11y re-run remain open.

## Full Verification

- `dotnet build Ashfall.Core` 0/0 · `dotnet build Ashfall.Core.Tests` 0 errors
- Focused: DayEventVocabulary + ParitySourceGate + RadiationExposureTransition
  **17/17 PASS**; adjacent DailyBriefing + CampaignDayCoordinator **58/58 PASS**
- Full suite: **NOT run** (user directive: skip the large test suite)
- `dotnet build Ashfall.csproj`: blocked only by the neighboring writer's
  pre-existing `TravelingCaravanHostSession.cs` CS7036 (no errors from any
  file this package touched)
- Godot selftests: deferred while the host build is blocked (previous session
  baseline: data-integrity PASS 327 catalogs)

## Remaining Debt (foreman decisions / separate packages)

1. **Plan 31** — unregistered. Semantic-kind authority, producer-side
   vocabulary standardization, 11 handled-but-never-emitted cases, alert
   ducking/concurrency (17C Phase I). Highest-value next package.
2. **17C Phase E sweep** — acquisition-path pickup confirmation parity.
3. **17B deep verification** — snapshot fixture, teach-before-demand matrix,
   a11y pass, subscription-leak soak (implementation exists; test matrix not
   re-run).
4. **TravelingCaravanHostSession CS7036** — neighboring writer's in-flight
   work; not this package's path.

## Simulation neutrality

No Core behavioral mutation beyond additive, observational events; no RNG
consumption; no save-schema change. Audio disabled → identical simulation
state by construction (events fire regardless of audio wiring; the bridge
only plays cues).
