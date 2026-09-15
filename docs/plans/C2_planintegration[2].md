# C2 — Flagship Integration Plan [2]: Legibility, Cause, Guidance, and Confirmation

> **Output:** `docs/plans/C2_planintegration[2].md`
> **Source scope:** Plan 17 — *Legibility: Cause, Effect, and Guidance*
> **Wave:** Continuity Wave 1
> **Integration theme:** make the simulation explain itself, make guidance reachable, and make ordinary actions audibly confirm exactly once.
> **Hard prerequisite:** Plan 16B must establish stable campaign-owned live panel/system bindings before this plan depends on `Weather`, `PowerGrid`, or other live authority state.
> **Upstream benefit:** Plan 15A/15B gives moral choices and their consequences a real path into the same cause/effect layer.
> **Double-fire dependency:** coordinate with Plan 16C so audio/event confirmation is not emitted twice.
> **Critical erratum:** the original Task 17A diagnosis is stale. Do **not** implement the obsolete “18 mute day owners” work. The re-audit states all 19 owners already emit; the actual defect is producer/consumer vocabulary drift. Execute/consume **Plan 31 — Event Layer Semantic Kinds / No Silent Drops** for that repair.
> **Effective execution order:** **Prerequisite validation → Plan 31 compatibility gate → 17C → 17B → integrated playthrough closure**
> **Scope discipline:** reuse the existing `DayStateChangeEvent`, `DailyBriefingReportBuilder`, `OnboardingJourney`, guidance panel, audio catalog, audio buses, and host seams. Do not create parallel systems.

---

# 0. Executive Intent

ASHFALL already has three player-legibility channels:

1. **Cause / attribution**
   - `DayStateChangeEvent`
   - day-owner reports
   - daily briefing
   - event history/log route

2. **Guidance**
   - `OnboardingJourney`
   - `OnboardingHintPanel`
   - “show me where” route behavior
   - onboarding persistence/status state

3. **Confirmation**
   - `AudioCueCatalog`
   - `AudioEventBridge`
   - `ShelterAudioController`
   - existing UI/hazard/music/ambience cues and buses

The problem is not absence of infrastructure. It is incomplete continuity.

The player can be affected by a large simulation but cannot reliably answer:

```text
What happened?
Why did it happen?
Was it because of me?
Where do I go to deal with it?
Did my action actually register?
```

This integration plan closes that gap without inventing another reporting, tutorial, or audio architecture.

The flagship outcome is:

> **A player performs an action, receives exactly one appropriate confirmation, advances the simulation, reads a cause-aware briefing that does not silently discard valid semantic events, can jump from a consequence to the relevant live surface, and can open/reopen contextual guidance at any time.**

---

# 1. Source Truth and Erratum Handling

## 1.1 Recorded source evidence

The source plan identifies:

- a typed `DayStateChangeEvent` with fields:
  - `Kind`
  - `SourceOwnerId`
  - `PrimaryId`
  - `SecondaryId`
  - `Numeric`
- `IDayAdvanceOwner.TickDay(int, List<DayStateChangeEvent>)`
- owner reports surfaced through `DayAdvancedEventArgs`
- a daily briefing path in `src/Main.Campaign.cs`
- an onboarding panel that exists but is not reachable
- a partially completed audio feedback layer
- unresolved alert stacking
- a missing radiation exposure-end signal blocking reliable geiger-loop stop
- snapshot coverage infrastructure.

## 1.2 Mandatory erratum

The plan's original 17A premise was later disproven.

Do **not** implement these obsolete assumptions:

```text
18 of 19 day owners are mute.
Only SurvivorFateSystem emits.
Normal sessions take the fallback briefing branch.
The repair is to add emission plumbing to the other owners.
```

The later audit states instead:

```text
All 19 owner classes emit.
There are 26 emission sites in src/Main.CampaignOwners.cs.
The primary briefing branch is already taken.
20 of 27 emitted kinds do not match any DailyBriefingReportBuilder case.
The builder has no default case, so those kinds silently disappear.
6 handled kinds are never emitted.
```

Therefore:

> **Plan 31 owns the producer/consumer semantic-vocabulary reconciliation.**

This C2 plan must consume the corrected event layer rather than recreate or compete with it.

## 1.3 What remains valid from Plan 17

The following goals remain active:

- briefings should communicate cause and effect,
- briefing entries should link to useful player surfaces,
- owner/tick failures must not be silently presented as “nothing happened,”
- guidance must be reachable and reopenable,
- guidance progression should reflect real player progress,
- ordinary UI actions need consistent confirmation,
- ambience/music/hazard cues need live-state wiring,
- item-acquisition confirmation should be centralized,
- geiger loop needs a real exposure-end lifecycle,
- alert stacking needs ducking/concurrency control,
- all audio buses used by runtime need test/settings parity.

---

## 1.4 Measured evidence appendix (2026-09-15 re-measurement)

Historical numbers in this plan were re-measured against the live tree before
any implementation. The measurement supersedes every historical count in this
document wherever the two disagree.

| Metric | Plan's re-audit claim | Measured 2026-09-15 | Delta |
|---|---|---|---|
| Owner classes emitting | 19 | 19+ (45 direct `new DayStateChangeEvent` sites in `Main.CampaignOwners.cs` alone) | confirms erratum |
| Distinct emitted kinds | 27 | **70** (across `src/` + Core producers) | broader drift |
| Builder-handled kinds | 7 of 27 | **31** | partial repair landed since |
| Emitted-but-unhandled (silent drop) | 20 | **50** | broader drift |
| Handled-but-never-emitted | 6 | **11** | broader drift |
| Builder default case | none | **none (at measurement)** | repaired by C2 |
| Briefing branch taken | primary | primary (`ShowBriefingForDay` → `BuildFromDayEvents(args.AllEvents())`) | confirms erratum |

Full per-kind disposition: `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`
(generated, gate-enforced by `DayEventParitySourceGateTests` — the matrix
cannot go stale silently).

### 1.4.1 Additional measured audio/onboarding facts

| Premise in this plan | Measured 2026-09-15 | Consequence |
|---|---|---|
| Guidance panel unreachable | `guidance` route registered (Dashboard group), dashboard nav row exists, bind/open/close wired, veteran gate present, show-me-where bridged, save store live | Phases A–C stale; execute only verification |
| Geiger loop "cannot stop" | Worse: `StartGeiger`/`StopGeiger` had **zero production callers** — the loop was never started either | Phase G scope widened to full lifecycle |
| Alert ducking missing | Correct — no ducking/concurrency exists in `AudioManager` | Phase I is a real gap; detailed spec in §16A |
| Ambience state machine missing | `SurfaceAmbienceController` + `ShelterAudioController` + `ReactiveAmbienceEvaluator` exist, subscribed to live power/weather | Phase C stale; execute only §10.5 matrix |
| F1 free for guidance | **Collision**: `ashfall_help` (tutorial) owns F1 | Phase D binding moved to F2, documented |
| Plan 31 executed | **Unregistered** — no docs, no vocabulary infrastructure | §6.12 contract spec binds what Plan 31 must deliver |

### 1.4.2 Evidence-freshness rule

Any future execution of this plan must re-run the §5 baseline and update this
appendix. A measured number older than the current working tree is evidence of
history, not a license to implement against it.

# 2. Program-Level Success Criteria

The program is complete only when all five legibility questions can be answered during a normal playthrough.

## 2.1 What happened?

The daily briefing shows semantic events that actually occurred and does not silently drop known emitted kinds.

## 2.2 Why did it happen?

Entries carry attribution sufficient to distinguish:

- player decision,
- simulation consequence,
- condition needing attention,
- non-actionable/unchanged state where intentionally retained.

## 2.3 Was it because of me?

Player-caused outcomes are visually/semantically distinguishable from ambient simulation changes.

## 2.4 Where do I go?

Actionable entries route to the relevant live panel through the existing player-surface routing seam.

## 2.5 Did my action register?

Ordinary actions emit exactly one appropriate confirmation cue or explicit invalid/warning cue when rejected.

---

# 3. Non-Negotiable Architectural Invariants

## 3.1 One semantic event channel

Use the existing typed day-event pipeline.

Do not introduce:

- `GameplayFeedbackEvent`,
- `BriefingEventV2`,
- a second daily-event list,
- a UI-only consequence feed that duplicates Core state.

Plan 31 may standardize constants/vocabulary, but C2 consumes that result.

## 3.2 One onboarding state machine

`OnboardingJourney` remains authoritative.

Do not:

- add a second tutorial progress enum,
- hardcode “Day 1 = step X” as the sole progress source,
- store tutorial completion only in the panel.

## 3.3 One audio catalog and existing buses

Reuse existing cues and buses.

Do not:

- add per-panel AudioStreamPlayers,
- hardcode asset paths in arbitrary panels,
- bypass `AudioCueCatalog`,
- introduce new buses merely to solve routing bugs,
- scatter button-click wiring across 164 UI files if a shared helper can solve it once.

## 3.4 Campaign-owned authority only

Guidance, ambience, hazards, and panel navigation must observe the real campaign-owned systems.

No throwaway `WeatherSystem`, `PowerGridSystem`, inventory, or radiation instances.

## 3.5 Exactly-once user feedback

An action may not produce two confirmation cues because both:

- a UI helper,
- and a host/event bridge

respond to the same semantic act.

Define ownership per cue category.

## 3.6 Audio cannot alter simulation

With audio enabled or disabled:

- deterministic simulation output is identical,
- save checksum is identical where the existing checksum represents simulation state,
- no gameplay RNG is consumed by audio.

## 3.7 Fail visible, not silent

If:

- a day-owner report fails,
- an event kind is unknown,
- a route cannot resolve,
- a guidance step cannot progress,
- an audio cue cannot resolve,

the system must produce a test-visible or player-visible failure path appropriate to severity.

---

# 4. Prerequisite Gate

Before implementing this C2 package, verify upstream readiness.

## 4.1 Plan 16B gate

Required facts:

- relevant panels use live campaign-owned systems,
- weather and power references used by audio are real,
- guidance “show me where” targets resolve to live player surfaces,
- no target route points at a throwaway system instance.

If 16B is incomplete:

- execute only isolated test scaffolding or documentation,
- do not finalize ambience/power-driven behavior,
- do not claim integrated DoD.

## 4.2 Plan 15A/15B compatibility

If moral-choice work is complete:

- verify its consequence records enter the canonical semantic event channel,
- verify Plan 31 recognizes their event kinds,
- verify briefing consumers can render them,
- verify guidance may listen to them only where pedagogically relevant.

If 15B is not complete, keep the consumer seam generic; do not special-case moral events in UI.

## 4.3 Plan 16C double-fire gate

Before centralizing UI audio:

- inspect whether 16C moves action notification to a shared seam,
- document cue ownership by layer,
- prevent duplicate host + UI confirmation.

## 4.4 Plan 31 gate

C2 does not reimplement Plan 31.

Before integrated closure, verify Plan 31 provides or proves:

- canonical semantic-kind vocabulary,
- producer/consumer parity,
- no silent unhandled known kind,
- stale handled-but-never-emitted cases removed, mapped, or intentionally retained,
- deterministic handling,
- tests for unknown/missing semantic cases.

If Plan 31 has not yet been executed, 17C may proceed, but 17B's event-driven progression must avoid binding to unstable kind strings until Plan 31 stabilizes them.

---

# 5. Baseline Capture

Record the exact working commit and current status before code changes.

## 5.1 Required baseline commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run:

```bash
python generate-audio-catalog.py --check
```

or the repository-canonical path to that script if located elsewhere.

## 5.2 Record baseline metrics

Capture:

- Git commit,
- build warnings/errors,
- test count,
- known event kinds emitted,
- known event kinds handled,
- unknown/unhandled emitted kinds,
- handled-but-unemitted kinds,
- audio selftest totals,
- cue count,
- bus count validated,
- UI `PlayCue` call-site count,
- snapshot target count,
- guidance route reachability status,
- onboarding journey save/load status,
- geiger loop start/stop behavior,
- alert concurrency behavior.

Historical source numbers are evidence, not permanent assertions. Re-measure.

## 5.3 Baseline stop conditions

Stop and classify before implementation if:

- core tests already fail,
- audio catalog validation already fails,
- Plan 31 changes are partially merged and producer/consumer semantics are in flux,
- route IDs changed,
- onboarding save store is broken before C2,
- audio buses differ substantially from the source plan,
- radiation exposure lifecycle was already replaced by another architecture.

---


### 5.4 Baseline already captured (2026-09-15, this plan's own execution)

For the successor executor: the §5.1 battery was run bounded (full 11k-case
suite intentionally skipped by user directive; focused runs only):

- `dotnet build Ashfall.Core` → 0 errors / 0 warnings,
- focused new tests 17/17, adjacent briefing/coordinator 58/58,
- data-integrity selftest PASS (327 catalogs, 0 errors),
- host build blocked *only* by concurrent unclaimed in-flight work in
  `TravelingCaravanHostSession.cs` (not this plan's path; reported, not
  touched — see AGENTS.md rule 6),
- event-kind metrics: §1.4; audio/onboarding facts: §1.4.1.

A later executor must re-run this battery; if the shared-tree blockage
persists, verify per-file error attribution (the plan's own files must
contribute zero errors) and record the external blocker in the closure
report rather than "fixing" another writer's file.
# 6. Workstream 17A-S — Superseded Attribution Task / Plan 31 Handoff

> **Status:** DO NOT IMPLEMENT THE ORIGINAL 17A SUBSTEPS.
>
> This workstream exists to integrate the corrected diagnosis into C2 and define the contract that 17B/17C consume.

## 6.1 Objective

Verify that the semantic event layer emerging from Plan 31 is usable by:

- daily briefing,
- event history/log,
- onboarding progression,
- confirmation/audio triggers where appropriate,
- moral-choice consequence attribution from Plan 15B.

## 6.2 Required Plan 31 outputs for C2

C2 requires a stable API/contract for:

```text
semantic kind
source owner
primary entity
secondary entity
numeric payload
display interpretation
route interpretation
```

The implementation may use constants, mappings, or another repository-consistent mechanism.

C2 must not depend on arbitrary raw strings scattered across panels.

## 6.3 No-silent-drop acceptance

For every emitted known semantic event:

- a consumer explicitly handles it,
- or a documented generic/default representation makes the event visible,
- or it is intentionally classified as non-player-facing and tested as such.

Forbidden outcome:

```text
producer emitted a valid known event
→ DailyBriefingReportBuilder has no matching case
→ event disappears
```

## 6.4 Producer/consumer parity matrix

Maintain or generate a matrix:

| Kind | Producer(s) | Briefing handler | Event log | Guidance consumer | Route | Status |
|---|---|---|---|---|---|---|
| semantic kind | owner | yes/no | yes/no | optional | target | LIVE/INTERNAL/DEFER |

Every row must have an explicit status.

## 6.5 Attribution categories

The daily briefing should support the existing category mechanism but make attribution legible.

Preferred conceptual groups:

```text
YOUR DECISIONS
CONSEQUENCES
NEEDS ATTENTION
UNCHANGED / STATUS
```

Do not force every event into `YOUR DECISIONS`.

A weather transition is not necessarily player-caused.

A failed craft caused by a player's recipe choice might be.

## 6.6 Player-caused classification

Where the event contract provides enough source identity, classify:

- direct player decision,
- downstream consequence of a recorded decision,
- autonomous world/system event,
- maintenance/status information.

Do not infer causality merely because an event occurs after an action.

## 6.7 Route contract

A briefing/event-log entry may provide a route only when:

- a relevant panel exists,
- it is LIVE under the liveness rules,
- `PrimaryId`/`SecondaryId` can resolve the intended entity,
- opening the target is useful.

Do not add clickable affordances that open irrelevant or dead panels.

## 6.8 Owner failure visibility

The source notes that day-owner reports already contain:

- `FailureMessage`,
- `HasFailures`.

Ensure normal daily briefing behavior surfaces a visible warning when an owner fails.

Acceptance:

```text
owner tick fails
→ briefing indicates incomplete simulation update
→ logs retain technical detail
→ UI does not claim "nothing happened"
```

## 6.9 Event-log parity

A player-important semantic event that appears in the briefing should also be queryable later through the existing `events_log` route when supported by current architecture.

Do not create an independent history database.

## 6.10 Determinism

Same simulation seed + same inputs:

- same semantic events,
- same aggregation order,
- same briefing output ordering,
- same guidance progression triggers.

Avoid dictionary-order dependence.

## 6.11 17A-S acceptance gate

- [ ] Original mute-owner repair is not implemented.
- [ ] Plan 31 semantic vocabulary is the authority.
- [ ] Emitted known kinds do not silently disappear.
- [ ] player-caused vs autonomous events are distinguishable.
- [ ] actionable entries have valid live routes.
- [ ] owner failures are visible.
- [ ] event-log parity is defined.
- [ ] deterministic ordering is tested.

---

## 6.12 Plan 31 contract specification (binding on the successor plan)

Plan 31 is unregistered. Until a registered Plan 31 package exists, this
section is the binding statement of what C2 requires from it, so the successor
plan can be written directly from this document.

### 6.12.1 Required deliverables

1. **Canonical semantic-kind vocabulary.** One authority type (constants or
   enum — builder-consumable, engine-free, Core-resident). Every emitted kind
   string in `src/` and Core must resolve to exactly one vocabulary entry.
   String literals scattered across producers are the defect class being
   retired.
2. **Per-kind disposition record.** Exactly one of:
   - `PLAYER_FACING` — briefing renders tailored text (builder case),
   - `GENERIC_VISIBLE` — rendered through the generic section,
   - `INTERNAL_HEARTBEAT` — intentionally non-player-facing, tested as such,
   - `RETIRED` — producer removed; the case is deleted in the same package.
3. **Producer-side cleanup.** The 11 handled-but-never-emitted cases are
   either wired to a real emitter, reclassified, or removed. A handler with
   no producer is dead vocabulary and must not survive Plan 31.
4. **Parity gate ownership.** `DayEventParitySourceGateTests` (or its
   successor) must fail CI when a new producer kind appears without a
   vocabulary entry, or a vocabulary entry loses its last producer without a
   `RETIRED` mark.
5. **Attribution metadata** (see §6.5/§6.6): per-kind default attribution
   category so the briefing can group `YOUR DECISIONS` / `CONSEQUENCES` /
   `NEEDS ATTENTION` / `UNCHANGED / STATUS` without per-panel inference.
6. **Determinism guarantee.** Vocabulary-driven rendering must remain a pure
   function of the event list — no RNG, no hash-iteration-order dependence,
   stable sort keys.
7. **Migration note.** `DayEventVocabulary` (the C2 consumer-side repair) is
   superseded, not deleted: Plan 31 absorbs its classification set and
   retires the suffix heuristic in favor of the explicit vocabulary.

### 6.12.2 Acceptance tests Plan 31 inherits

- every emitted kind resolves to one vocabulary entry (source-gate),
- every vocabulary entry has a producer or a `RETIRED` mark,
- unknown/unresolvable kind at the consumer → visible generic render (never
  silent), plus a test-visible diagnostic,
- deterministic ordering under shuffled input insertion order,
- the parity matrix document regenerates clean (`--check` mode).

### 6.12.3 Non-goals for Plan 31

- No new event transport, bus, or queue (§3.1).
- No UI work beyond rendering the classification.
- No change to `DayStateChangeEvent` field shape unless additive and
  migration-safe.

## 6.13 Attribution category mapping (default disposition per kind family)

The generic section renders facts; the briefing's *grouping* is what answers
"was it because of me?". Default mapping to be formalized by Plan 31; C2
records the intent here so the successor vocabulary carries it from birth.

| Kind family (pattern) | Default attribution | Rationale |
|---|---|---|
| `*_ticked`, `events_evaluated`, `world_ticked` | (non-player-facing) | heartbeat; suppressed by classification |
| `sanitation_spill`, `shelter_fire`, `subterranean_*_warning`, `market_shocks_active` | NEEDS ATTENTION | hazard/threshold crossing; rarely player-caused directly |
| `crafting_completed`, `workshop_job_completed`, `expedition_milestone`, `personal_quest_progressed` | YOUR DECISIONS | direct consequences of ordered player work |
| `survivor_perished`, `survivor_condition` | CONSEQUENCES (or NEEDS ATTENTION when ongoing) | simulation outcomes of prior state; ongoing conditions belong in attention |
| `radio_intercept*`, `radio_distress_*` | CONSEQUENCES | world intelligence; player reacts rather than causes |
| `weather_*` | UNCHANGED / STATUS (unless crisis transition) | autonomous world; only transitions matter |
| `consumed_rations`, `resource_delta` | UNCHANGED / STATUS when steady; CONSEQUENCES when crossing a warning threshold | noise-budget-driven split |
| `narrative_arc_selected` | YOUR DECISIONS | explicit player selection |
| `trapping_harvest`, `nuclear_generation_published`, `radio_program_production_*` | CONSEQUENCES | system outputs from standing orders |

Rules:

- attribution is a **default**, never an inference from timing (§6.6);
- crisis transitions may upgrade STATUS → NEEDS ATTENTION; downgrades are
  forbidden without a threshold reason;
- the mapping lives in the vocabulary, not in panel code.

## 6.14 C2 consumer-side repair (shipped 2026-09-15, record)

To keep the repository honest between C2 and Plan 31, the consumer side of the
no-silent-drop contract was already executed. Plan 31 builds **on** this, not
over it:

- `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` — classification
  authority (heartbeat suffix rule + curated internal set + generic renderer),
- `DailyBriefingReportBuilder.BuildFromDayEvents` — default case routing every
  unhandled non-heartbeat kind into a deduplicated, overflow-capped
  "System Activity" section,
- `ShowBriefingForDay` — visible Warnings entry when `args.HasFailures` (§6.8),
- tests: `DayEventVocabularyTests` (8 cases), `DayEventParitySourceGateTests`
  (structural default-case pin + per-kind coherence + matrix freshness),
- parity matrix document generated and gate-enforced.

Plan 31 must absorb (7) and retire the suffix heuristic; until then the
heuristic is the tested, documented behavior of record.

# 7. Workstream 17C — Confirmation and Audio Continuity

## 7.1 Objective

Finish the feedback layer so ordinary player actions feel responsive and environmental/hazard audio follows real campaign state.

The source identifies six partial silence categories:

- UI panels,
- ambience,
- music,
- shelter door,
- item pickup,
- danger,

plus the geiger stop Core gap and alert stacking.

The implementation goal is not “more sound.” It is **correct lifecycle, routing, ownership, and exactly-once confirmation**.

---

# 8. 17C Phase A — Audit as an Executable Gate

## 8.1 Re-run audio validation

Run:

```bash
godot --headless --path . -- --audio-selftest
python generate-audio-catalog.py --check
```

Record:

- cue count,
- resolved asset count,
- missing asset count,
- bus coverage,
- duplicate path aliases,
- invalid extensions,
- orphan cues,
- dead buses,
- loop lifecycle failures.

## 8.2 Convert prose findings into test assertions

Where practical, encode audit requirements so they fail automatically.

Examples:

- every cue resolves to a catalog asset,
- every runtime-used bus exists,
- every runtime-used bus is validated,
- looping cues have a defined stop path,
- no production literal bypasses the catalog,
- duplicate semantic cues sharing one asset are explicitly allowed or rejected.

## 8.3 Historical number handling

The source records `245/245` and `70 cues`.

Treat these as a historical checkpoint only.

Do not fail a future repository merely because the legitimate cue count changed.

Fail on:

- unresolved references,
- invalid routing,
- missing stop lifecycle,
- unsupported bus,
- catalog drift.

---

# 9. 17C Phase B — Shared UI Cue Coverage

## 9.1 Centralize at `AshfallUiHelpers`

The source identifies `AshfallUiHelpers` as the shared button factory across a large UI surface.

Preferred strategy:

```text
MakeActionButton
→ standard interaction cue policy
→ one centralized playback path
```

Map existing cue categories:

- ordinary click,
- confirm,
- invalid,
- warning,
- tab/modal transitions where appropriate.

## 9.2 Cue ownership table

Before changing helper behavior, write the ownership matrix.

| Interaction | UI helper owns cue? | Host/event owns cue? | Rule |
|---|---:|---:|---|
| navigation click | yes | no | UI-only feedback |
| invalid UI action | yes or host-result | no duplicate | one rejection cue |
| successful domain action | maybe | preferably semantic host result if already wired | exactly once |
| asynchronous completion | no | yes | event-driven |
| hazard | no | yes | simulation-driven |

The actual rows should match repository behavior.

## 9.3 Exactly-once guard

Add tests/probes ensuring:

```text
one user act
→ one confirmation
```

not:

```text
button click cue
+ host confirmation cue
+ event bridge confirmation cue
```

## 9.4 Avoid per-panel scattering

Do not modify dozens of panels solely to add `PlayCue`.

Only add panel-specific handling when:

- the interaction is semantically different,
- the shared helper cannot know success/failure,
- the cue belongs to a domain event.

---

# 10. 17C Phase C — Ambience State Machine

## 10.1 Inputs

Drive ambience from real campaign state named in the source:

- current screen/context,
- `WeatherSystem.Current`,
- `PowerGridSystem` load tier.

## 10.2 States

At minimum, distinguish the existing bunker/surface ambience families.

Do not create new ambience families unless a separate approved content plan does so.

## 10.3 Lifecycle rules

- start a loop once,
- do not restart the same loop every refresh,
- cross/stop when context changes,
- stop on session replacement,
- rebind on New Game,
- rebind on Load,
- no orphan player node remains after teardown.

## 10.4 Campaign authority

Ambience state must observe the real:

- weather system,
- power grid,
- current gameplay context.

No UI-owned mock state in production.

## 10.5 Tests

- shelter context starts bunker ambience,
- surface context transitions appropriately,
- weather affects expected ambience behavior where specified,
- power-load tier affects expected behavior where specified,
- session replacement stops old loop,
- load starts correct loop once,
- repeated refresh does not duplicate loop.

---

# 11. 17C Phase D — Music Transition Integrity

## 11.1 Supported transitions

Use existing music cues for:

```text
menu ↔ gameplay ↔ ending
```

## 11.2 Catalog-only resolution

All paths must resolve through the catalog.

Explicitly guard the historical bug class:

```text
.wav vs .ogg extension mismatch
```

No literal file path should be the source of truth when the catalog already owns the cue.

## 11.3 Transition behavior

Verify:

- no double music instance,
- previous state fades/stops according to existing controller behavior,
- session change does not leave stale music,
- ending music is not retriggered every UI refresh.

---

# 12. 17C Phase E — Item Pickup Parity

## 12.1 Problem

The source states `action_item_pickup` currently fires only from expedition completion.

That misses acquisition through:

- craft output,
- trade,
- gift,
- scavenging,
- autopsy,
- any other inventory-producing route.

## 12.2 Correct integration seam

Move acquisition confirmation to the canonical inventory mutation seam if that seam can distinguish:

```text
net item acquisition
```

from:

- restore/load,
- migration,
- internal reconciliation,
- quantity normalization.

## 12.3 Exactly-once rule

One acquisition transaction should produce one semantically appropriate pickup confirmation, not one cue per internal stack mutation unless design explicitly demands that.

## 12.4 Save/load exclusion

Loading inventory must never sound like newly acquired loot.

Add a regression test:

```text
save with item
→ load
→ no pickup cue
```

---


## 12A. Phase E audit matrix — canonical acquisition routes

The pickup seam may only confirm **net acquisition** (§12.2). Enumerate every
inventory-producing route and its completion authority before wiring; the
wiring lands once, at the canonical seam, and this matrix becomes its test
list. Statuses are from the 2026-09-15 re-measurement and must be re-verified
at execution.

| # | Route | Producing authority / seam | Emits completion signal? | Pickup cue owner | Status |
|---|---|---|---|---|---|
| 1 | Expedition loot | `ExpeditionSystem` completion → `OnLootAdded` | yes | host bridge (already wired) | LIVE |
| 2 | Craft output | Crafting completion authority | yes (completion event) | host bridge | VERIFY wiring |
| 3 | Trade / market purchase | Economy transaction authority | VERIFY | host bridge at transaction commit | AUDIT |
| 4 | Barter / gift | Economy or NPC gift authority | VERIFY | host bridge | AUDIT |
| 5 | Scavenging / loot site | Scavenging authority | VERIFY | host bridge | AUDIT |
| 6 | Autopsy / salvage yield | Medical/salvage authority | VERIFY | host bridge | AUDIT |
| 7 | Production (foundry/workshop/aeroponics/aquaponics/greenhouse) | each production authority's completion event | yes (various) | host bridge | VERIFY each |
| 8 | Restock / delivery (caravan, radio program material) | delivery authority | VERIFY | host bridge | AUDIT |
| 9 | Save restore / migration | restore path | MUST NOT CUE | — | REGRESSION TEST |
| 10 | Internal reconciliation / stack split / quantity normalize | inventory internals | MUST NOT CUE | — | REGRESSION TEST |

Wiring rules:

- one cue per **transaction**, not per stack mutation (§12.3): if a route
  delivers five stacks, that is one acquisition act;
- if the canonical seam cannot yet distinguish acquisition from
  restore/normalization, **fix the seam first** — never filter by call-site
  booleans scattered across producers (that recreates the vocabulary-drift
  defect class in audio form; §40.7);
- the restore-exclusion regression test (§12.4) runs against the real codec:
  save with items → restore → assert zero cue invocations on the captured
  playback log.

# 13. 17C Phase F — Danger Cue Wiring

## 13.1 Use existing hazard events

Wire existing danger cues to real events such as source-listed:

- foundry incident,
- sump flooding,
- hatch defense,
- vault breach.

Use existing hazard/domain events.

Do not synthesize fake “danger” events solely for audio.

## 13.2 Semantic mapping

Each hazard cue should have:

- source event,
- severity/priority,
- bus,
- concurrency class,
- whether it can interrupt/duck ambience.

## 13.3 No state polling for one-shot hazards

Prefer event-driven one-shots.

Do not play explosions because a panel observes `IsDamaged == true` on every refresh.

---

# 14. 17C Phase G — Radiation Exposure-End Signal

## 14.1 Objective

Close the documented Core lifecycle gap preventing `rad_geiger_loop` from stopping.

## 14.2 Core event

Add an explicit, type-safe end-of-exposure signal to `RadiationSystem` or the repository's canonical radiation authority.

Requirements:

- emitted exactly when exposure transitions from active to inactive,
- does not depend on UI lifecycle,
- no new RNG,
- no save-schema churn unless truly necessary,
- capture/restore does not spuriously replay an “end” event.

## 14.3 Audio bridge behavior

```text
exposure starts
→ geiger loop starts once

exposure remains active
→ loop remains active, no restart spam

exposure ends
→ geiger loop stops

session ends/replaces
→ loop stops defensively
```

## 14.4 Restore behavior

On load:

- derive correct current exposure state,
- establish correct loop state,
- do not replay historical begin/end one-shots unless design expects it.

## 14.5 Tests

- begin exposure starts once,
- sustained exposure does not duplicate,
- end exposure stops,
- repeated end is harmless,
- save/load while exposed restores correct loop,
- save/load while not exposed does not start loop,
- teardown stops loop.

---

# 15. 17C Phase H — Asset De-duplication

The source flags semantic collisions where distinct threats share the same asset.

Initial pairs to re-verify:

- `rad_contamination` vs `weather_black_rain`,
- `weather_alert` vs `danger_alarm_klaxon`,
- `shelter_pipe_clang` vs `day_transition`.

The source describes these as four pairs; re-run the audit and use the actual current list.

## 15.1 Decision rule

For each collision:

- if semantic distinction matters to player interpretation, assign distinct audio,
- if sharing is intentional, document it and test the alias explicitly.

Do not leave accidental aliasing ambiguous.

## 15.2 Content scope

No new audio family.

Only replace/assign the minimal distinct assets needed to preserve threat semantics.

---

# 16. 17C Phase I — Alert Ducking and Concurrency

## 16.1 Problem

A fallout storm can stack multiple Alerts-bus cues in a short window, creating unintelligible output.

## 16.2 Ducking

When a high-priority Alerts cue fires:

- duck Ambience/SFX by approximately the source-specified ~6 dB,
- restore after the alert lifecycle,
- ensure nested alerts do not prematurely restore volume.

Use the existing audio architecture.

## 16.3 Concurrency cap

Define a per-bus or per-alert-class concurrency policy.

Desired outcome:

```text
storm + dose + klaxon
→ prioritized intelligible alert behavior
```

not:

```text
three or four overlapping high-priority cues
```

## 16.4 Priority semantics

Determine priority deterministically using existing severity semantics where possible.

Do not base alert survival on race timing or node enumeration order.

## 16.5 Tests

- two simultaneous alerts resolve predictably,
- lower-priority alert is suppressed/deferred per policy,
- ducking engages once,
- nested alert does not over-duck,
- volume restores correctly,
- session teardown clears duck state.

---


## 16A. 17C Phase I — Detailed Design (the real gap; nothing exists yet)

Re-measurement confirmed alert ducking/concurrency is entirely absent from
`AudioManager`. This is therefore the largest remaining 17C engineering item
and is specified here at implementation depth.

### 16A.1 State model (host-side, `AudioManager` or a dedicated `AlertCoordinator`)

```text
AlertActivation
    CueId, Bus, Priority, ConcurrencyClass, StartedTick, DuckProfile

AlertCoordinator state:
    activeAlerts: ordered map (insertion = fire order, priority = sort key)
    duckedBuses: map bus → duck depth (dB, ref-counted)
```

Rules:

- state lives **beside** `AudioManager`, never inside Core; simulation
  neutrality (§3.6) is structural because Core never sees this type;
- the coordinator is deterministic: priority ties break by fire order, never
  by node enumeration or race timing (§16.4).

### 16A.2 Priority table (initial; derive severities from existing event payloads)

| Class | Example events | Priority | Concurrency cap | Duck |
|---|---|---:|---:|---|
| CRITICAL hazard | cave-in, vault breach, shelter fire | 0 (highest) | 2 | Ambience −6 dB, SFX −6 dB |
| STORM alert | fallout storm, black rain, blizzard onset | 1 | 2 | Ambience −6 dB |
| RAD alert | geiger intense, dose threshold | 2 | 1 | Ambience −3 dB |
| Routine alert | EBS broadcast, weather watch | 3 | 1 | none |

- caps are **per class**, so a storm can coexist with a critical hazard but
  cannot stack four same-class cues;
- exceeding a cap **defers** the lowest-priority pending activation; deferral
  is a visible, testable state (not a silent drop — §3.7).

### 16A.3 Ducking lifecycle

```text
alert fires
→ duck depth for its profile is applied once per bus (ref-counted)
→ nested alert of same/lower profile does NOT deepen the duck
→ alert lifecycle ends (cue finished or stop signal)
→ ref-count decrements; bus restores only at depth 0
→ session teardown / Dispose clears all duck state (no leak — §40.9)
```

Implementation constraints:

- use the existing bus-volume pathway (`SetBusVolume`) — no new buses (§3.3);
- volume changes must be idempotent per state snapshot; repeated application
  of the same duck state is a no-op;
- restore uses the player's **settings** volume as the base, not a cached
  literal — a mid-alert settings change must not be clobbered on restore.

### 16A.4 Tests (extends §16.5)

- `TwoSimultaneousAlerts_HighestPrioritySurvues_CapEnforced` — deterministic
  winner under both fire orders,
- `DuckDepth_RefCounted_NestedAlertDoesNotOverDuck`,
- `VolumeRestores_ToSettingsBase_NotCachedLiteral`,
- `TeardownDuringActiveAlert_ClearsDuckState_AndStopsCues`,
- `DeferredAlert_Emits_WhenSlotFrees` — deferral is observable, never silent,
- `AlertCoordinator_ZeroGameplayStateReads` — structural neutrality probe
  (the coordinator type must not reference Core systems beyond event args).

# 17. 17C Phase J — Bus Topology and Settings Parity

## 17.1 Runtime buses

The source notes additional buses such as:

- Generator,
- Ventilation,
- Medical,
- Surface,

and an expectation that all 12 buses be validated rather than only the original seven.

Re-measure actual bus topology.

## 17.2 Bus requirements

Every named runtime bus must satisfy:

- exists,
- has a runtime purpose,
- has at least one routed cue if considered active,
- validated by audio selftest,
- volume controlled through settings where player-facing,
- recoverable through settings-reset/recovery flow.

## 17.3 Settings recovery

Run the repository's:

```text
docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md
```

Validate:

- slider maps to intended bus,
- mute/unmute,
- restore defaults,
- session persistence,
- no missing newly active bus.

---


---

# 17C Phase K — Diagnostics, Telemetry, and Fail-Visible Paths (expansion)

> Added by the C2 expansion. §3.7 demands fail-visible behavior; this phase
> gives every legibility channel a **diagnosable** failure path so field
> reports can answer "what broke" without a debugger.

## K.1 Diagnostic surfaces

| Failure | Player-visible | Log/diagnostic | Test-visible |
|---|---|---|---|
| unknown event kind reaches consumer | generic System Activity row | kind + producer file in gate output | parity gate per-kind coherence |
| owner tick failure | briefing Warnings entry (§6.8) | `FailureMessage` retained verbatim in logs | coordinator test injecting a failing owner |
| route cannot resolve | existing `OpenPlayerPanel` diagnostics | `GD.PrintErr` + status label | PanelRegistry unknown-route test (exists) |
| guidance step cannot progress | status-bar notice; stage unchanged | trigger class + observed signal | 17B deep battery |
| audio cue unresolved | text path continues (never crashes) | one-time log per missing cue | audio selftest missing-cue assertion |
| duck state anomaly (restore below zero) | none | assert + reset to settings base | Phase I ref-count test |

## K.2 Diagnostic-count budget

- missing-cue logs are **once per cue id per session** (existing policy —
  keep it); repeating logs for a known-missing cue is log spam,
- owner-failure warnings aggregate per day: one briefing row listing all
  failed owners, not one row per failure,
- parity-gate failures name kind, producer file, and required action —
  a failing gate must be actionable without reading this plan.

## K.3 Determinism of diagnostics

Diagnostic emission order must be deterministic (sorted by owner id /
cue id). A diagnostic channel that itself depends on dictionary iteration
order is a determinism defect of the same class the parity gate exists to
prevent.

## K.4 Tests

- injected failing owner → one aggregated briefing warning, verbatim
  `FailureMessage` in logs, simulation continues,
- missing cue → single log line, text path completes, no exception,
- diagnostics emitted in sorted order under shuffled input,
- diagnostic emission consumes no gameplay RNG (structural: it must not
  touch the seeded streams).
# 18. 17C Regression Suite

Minimum required tests/probes:

- shared UI click fires once,
- successful action confirm fires once,
- invalid action fires one invalid cue,
- domain completion does not double-fire with UI helper,
- item acquisition from each canonical path fires once,
- inventory load fires zero pickup cues,
- ambience starts/stops correctly,
- music transition resolves catalog paths,
- geiger begins/ends correctly,
- danger events map to intended cue,
- alert concurrency is bounded,
- ducking recovers,
- all active buses validate,
- audio disabled yields identical simulation checksum.

## 18.1 Simulation neutrality

Run equivalent deterministic scenario:

```text
Run A: audio enabled
Run B: audio disabled
```

Assert simulation state/checksum parity.

Audio must be observational.

---

# 19. 17C Acceptance Gate

Do not begin final guidance integration until:

- [ ] audio selftest green,
- [ ] catalog check green,
- [ ] UI ordinary actions confirm exactly once,
- [ ] no broad per-panel cue scattering,
- [ ] ambience uses live campaign state,
- [ ] music transitions use catalog paths,
- [ ] item pickup is canonicalized,
- [ ] hazard cues use real events,
- [ ] geiger loop has a real stop signal,
- [ ] duplicate semantic asset collisions are resolved/classified,
- [ ] alert stacking controlled,
- [ ] all active buses validated,
- [ ] settings parity verified,
- [ ] audio is simulation-neutral.

---

# 20. Workstream 17B — Reachable, Reopenable Guidance

## 20.1 Objective

The existing onboarding state machine becomes a usable, optional teaching layer.

A new player should be able to:

- encounter contextual guidance,
- open guidance from the HUD,
- reopen it on demand,
- use F1,
- navigate to the relevant live screen,
- progress because of real actions,
- save/load progress,
- dismiss assistance without permanently losing help.

Veterans must be able to disable assistance without losing the explicit help route.

---

# 21. 17B Phase A — Read-First Onboarding Contract

Inspect:

- `src/Main.Onboarding.cs`
- `src/UI/OnboardingHintPanel.cs`
- `src/UI/GameDashboardPanel.cs`
- `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs`
- `project.godot` input map
- `Assets/Ashfall.Core/Onboarding/*`
- onboarding save store
- existing manual playthrough checklist.

Document exact existing APIs:

- `SetDay`,
- `OnJourneyChanged`,
- `SetOnboardingAssistance`,
- `OnShowMeWhereRequested`,
- any current stage enum/IDs,
- persisted fields,
- failure/idle tracking fields.

Do not redesign the state machine.

---

# 22. 17B Phase B — Reproduce the Visibility Defect

Create a runtime/UI test demonstrating current failure:

```text
session starts
→ onboarding state exists
→ OnboardingHintPanel exists
→ panel never becomes Visible through a player route
```

Commit the failing test before the behavior fix where workflow permits.

The test must prove reachability, not merely construction.

---

# 23. 17B Phase C — Guidance Route

## 23.1 Reuse `help`

The source indicates `help` already belongs to `PanelGroup.Dashboard`.

Prefer reusing it as the player-facing route.

Do not create a new panel solely for “GUIDANCE.”

## 23.2 Dashboard navigation

Add one `GUIDANCE` nav entry alongside the existing dashboard navigation rows.

Requirements:

- normal route ownership,
- keyboard focus,
- screen-reader/accessibility label under project conventions,
- no duplicate route IDs.

## 23.3 Liveness compatibility

The guidance route must pass the panel-liveness rules established earlier in the continuity wave.

Opening a dead/unbound guidance shell does not satisfy this task.

---

# 24. 17B Phase D — F1 Input Action

Add/verify:

```text
ui_guidance
```

with F1 as the default requested binding.

## 24.1 Input-map rules

- respect rebinding conventions,
- do not hardcode keyboard polling in the panel,
- run input map audit,
- check collisions against current actions,
- ensure route works with keyboard only.

## 24.2 Reopen semantics

F1 should:

- open guidance when closed,
- use existing close behavior when appropriate,
- never permanently disable guidance,
- function after load/new game/session replacement.

---

# 25. 17B Phase E — Host-Owned Visibility

The source defect is that `_Ready()` forces:

```text
Visible = false
```

and no route makes the panel reachable.

Correct ownership:

```text
host/player route
→ Open()
→ visible

close action
→ Close()
→ hidden
```

The constructor/_Ready lifecycle should not permanently own player-facing visibility.

## 25.1 Avoid visibility races

Test:

- construction,
- deferred UI initialization,
- route open,
- route close,
- session swap,
- load,
- reopen.

A late `_Ready()` callback must not undo an explicit route-open request.

---

# 26. 17B Phase F — Complete “Show Me Where”

The source already has:

```text
OnShowMeWhereRequested → OpenPlayerPanel
```

Complete the semantic lifecycle.

## 26.1 Required behavior

```text
guidance step visible
→ player chooses "show me where"
→ guidance overlay closes/minimizes as designed
→ relevant LIVE panel opens
→ journey step records that guidance action/progress where appropriate
```

## 26.2 Do not mark task completion too early

“Show me where” is not automatically equivalent to completing the gameplay action.

Differentiate:

- `hint acknowledged`,
- `target opened`,
- `action completed`.

Advance the actual onboarding objective only at the existing state machine's correct semantic checkpoint.

---

# 27. 17B Phase G — Progress from Real Events and Actions

## 27.1 Event source

After Plan 31 stabilizes event kinds, guidance may consume semantic events for real progress.

Examples from the source teaching order:

- rations,
- water/filter,
- craft,
- expedition,
- advance day,
- dose.

## 27.2 Direct action sources

Where a semantic day event is too delayed, consume existing authoritative player-action completion signals.

Examples:

- craft completed,
- expedition returned,
- relevant route/action completed.

## 27.3 Clock is not truth

Do not mark a tutorial stage complete merely because:

- a day number changed,
- elapsed seconds passed,
- a panel was opened.

Time may drive idle nudges, not false completion.

## 27.4 Idle/failure fields

Reuse existing:

- `_onboardingFailedActions`,
- `_onboardingLastInteractionSeconds`

for nudging behavior where already intended.

Do not add a second idle tracker without necessity.

---

# 28. 17B Phase H — Status Bar and Dismiss/Reopen Semantics

Verify `RefreshOnboardingStatusBar()` output is:

- legible,
- current,
- dismissible,
- not permanently destructive.

Required invariant:

```text
dismiss hint
≠
disable all future access to guidance
```

If assistance toggle is off:

- no forced interruption,
- explicit guidance route/F1 remains available unless existing product semantics explicitly say otherwise.

---

# 29. 17B Phase I — Day 1 to Day 3 Teaching Sequence

Validate the source-proposed teaching order:

```text
rations
→ water/filter
→ craft
→ expedition
→ advance day
→ dose
```

Do not force this exact order if current authored `OnboardingJourney` already defines a different canonical order. The source instruction is to cross-check, not silently replace authored state.

## 29.1 Teach-before-demand audit

For the first three days:

- identify every required player action,
- identify when it is first taught,
- ensure no mandatory progression demand precedes its teaching moment.

Create a matrix:

| Action | First demanded | First taught | Pass? | Fix |
|---|---|---|---|---|

## 29.2 Avoid over-tutorialization

Keep guidance contextual and optional.

Do not block normal gameplay behind modal tutorials unless the existing design already requires it.

---

# 30. 17B Phase J — Accessibility

Run the project's accessibility workflow on the overlay and route.

Verify:

- keyboard-only access,
- visible focus,
- no color-only meaning,
- readable contrast,
- text alternatives for icons/color cues,
- sensible focus return after “show me where,”
- guidance can be reopened without mouse.

If the project has standard font-size constraints, apply them.

---

# 31. 17B Phase K — Snapshot Coverage

Add/update a real snapshot fixture for the now-player-facing guidance overlay following:

```text
docs/ui/SNAPSHOT_FIXTURE_POLICY.md
```

Update:

```text
docs/ui/SNAPSHOT_COVERAGE.md
```

Classify it as covered only after:

- real route,
- real binding,
- stable representative state.

Do not snapshot a fake preview and call the live route covered.

---

# 32. 17B Phase L — Save/Load and Session Replacement

## 32.1 Save persistence

Prove:

```text
progress guidance
→ save
→ load
→ same onboarding stage/progress
→ F1 opens guidance
```

## 32.2 New Game

A New Game should:

- construct fresh guidance UI,
- bind it,
- initialize onboarding according to fresh campaign state.

## 32.3 Load

Load should:

- tear down old panel/session,
- construct/rebind fresh panel if architecture does so,
- restore onboarding journey,
- avoid stale event handlers from previous session.

## 32.4 Subscription leak test

Where feasible:

```text
new game
→ load
→ new game
```

Then trigger one onboarding event and assert exactly one progression callback.

---

# 33. 17B Regression Suite

Minimum tests:

- guidance route opens,
- guidance route closes,
- route reopens,
- F1 opens,
- F1 works after load,
- “show me where” opens correct live panel,
- journey step does not falsely complete on mere panel open unless authored that way,
- real craft/action advances intended stage,
- persisted journey survives save/load,
- New Game resets correctly,
- old subscriptions do not double-fire,
- assistance off prevents forced hints but explicit route remains usable,
- keyboard-only path reaches all controls,
- snapshot fixture matches approved state.

---

# 34. 17B Acceptance Gate

- [ ] guidance accessible from dashboard,
- [ ] guidance accessible from F1,
- [ ] guidance reopenable,
- [ ] `_Ready()` no longer permanently owns visibility,
- [ ] “show me where” routes to a live surface,
- [ ] journey progresses from real actions/events,
- [ ] idle nudges use existing state,
- [ ] status bar legible/dismissible,
- [ ] assistance-off behavior respected,
- [ ] Day 1–3 teach-before-demand audit passes,
- [ ] accessibility passes,
- [ ] snapshot coverage added,
- [ ] save/load passes,
- [ ] session replacement passes.

---

# 35. Integrated Cause → Guidance → Confirmation Flows

After individual workstreams are green, test complete player journeys.

## 35.1 Flow A — Ordinary action

```text
player clicks valid action
→ action reaches live authority
→ exactly one confirmation cue
→ simulation state changes
→ no duplicate event/cue
```

## 35.2 Flow B — Invalid action

```text
player attempts unavailable action
→ no simulation mutation
→ explicit visual reason
→ exactly one invalid/warning cue
```

## 35.3 Flow C — Day consequence

```text
player makes meaningful decision
→ day advances
→ semantic event exists
→ Plan 31 consumer recognizes kind
→ briefing displays consequence
→ entry routes to relevant live panel
→ event remains available in history where applicable
```

## 35.4 Flow D — Guidance-assisted action

```text
guidance recommends action
→ "show me where"
→ relevant live panel opens
→ player completes action
→ exactly one confirmation
→ onboarding journey advances
→ save
→ load
→ step remains advanced
```

## 35.5 Flow E — Radiation exposure

```text
exposure begins
→ geiger loop begins once
→ player acts / time advances
→ exposure ends
→ explicit Core end signal
→ geiger loop stops
→ no lingering loop after session replacement
```

## 35.6 Flow F — Alert storm

```text
multiple hazards/alerts occur close together
→ priority/concurrency policy selects intelligible output
→ ambience/SFX duck
→ alert completes
→ volumes restore
→ simulation state unaffected
```

---

# 36. Dependency Graph

```text
Plan 16B ───────────────────────────────┐
   │                                    │
   │ stable campaign authority          │
   ▼                                    │
Plan 31 — semantic kinds/no silent drop │
   │                                    │
   ├─────────────► briefing/event log   │
   │                   │                │
   │                   ▼                │
   │              guidance progress ◄───┘
   │                   ▲
   │                   │
Plan 15B ── moral consequence events ───┤
   │
   └────────► attribution

Plan 16C ── exactly-once action seam ─────► 17C audio confirmation
                                             │
                                             ▼
                                      17B guided actions
```

## 36.1 Effective order

```text
1. Validate 16B.
2. Validate/consume Plan 31 semantic contract.
3. Execute 17C audio continuity.
4. Execute 17B guidance.
5. Run integrated cause-guidance-confirmation journeys.
```

Why 17C before 17B:

- central confirmation becomes stable before guided actions depend on it,
- exactly-once semantics are tested before onboarding drives repeated interactions,
- 17B can then be validated against both semantic events and final confirmation behavior.

---


## 36.2 Execution status ledger (2026-09-15, from this plan's own bounded execution)

The effective order (§36.1) with current standing. Successor work starts here,
not at the top of the document.

| Step | Standing | Evidence / remaining work |
|---|---|---|
| 1. Validate 16B (live bindings) | SATISFIED (recon) | guidance/ambience/power audio bind live session systems; no throwaway instances found on the audited paths |
| 2. Plan 31 contract | **UNREGISTERED** | C2 shipped the consumer-side repair (§6.14); §6.12 binds Plan 31's required deliverables |
| 3. 17C audio | PARTIAL | Phase G geiger lifecycle DONE (Core + bridge + tests); Phases B/C/D/F audited stale (already built); **Phase I ducking/concurrency is the real gap — spec in §16A**; Phase E matrix in §12A; H/J sweeps open |
| 4. 17B guidance | PARTIAL | Phases A–C + E–L mechanics audited stale (already built); content authority + deep verification battery in §47B; F2 toggle DONE (F1 collision documented) |
| 5. Integrated journeys | OPEN | run after the remaining 17C/17B items; scenarios in §35 with the §38 matrix |

## 36.3 Suggested successor package split

Three disjoint packages (per AGENTS.md concurrency rules):

1. **`PLAN-31-SEMANTIC-VOCABULARY`** — §6.12 spec; owns the vocabulary type,
   producer cleanup, matrix regeneration; Core + tests only.
2. **`C2-ALERT-DUCKING`** — §16A spec; owns `AlertCoordinator` + `AudioManager`
   ducking integration + tests; host-only.
3. **`C2-GUIDANCE-DEEP-VERIFY`** — §47B; owns stage inventory, teach-before-
   demand fixes, deep battery, snapshot; host/UI + docs.

Sequencing: 1 → 3 (guidance binds stable kinds); 2 is independent and can run
in parallel with 1.

# 37. Commit Strategy

Use small commits with independent verification.

## Commit C2[2].1 — Baseline + erratum lock

- record current metrics,
- add plan note/tests proving actual event-layer state,
- explicitly prevent obsolete 17A implementation.

## Commit C2[2].2 — Plan 31 compatibility assertions

- producer/consumer parity contract,
- no-silent-drop integration tests,
- route/event-log compatibility.

## Commit C2[2].3 — Shared UI confirmation

- helper-level policy,
- exactly-once tests,
- coordinate with 16C.

## Commit C2[2].4 — Ambience/music lifecycle

- live state inputs,
- session teardown/rebind,
- catalog path tests.

## Commit C2[2].5 — Inventory + hazard audio seams

- pickup canonicalization,
- danger event mapping,
- no load-time pickup cues.

## Commit C2[2].6 — Radiation end signal

- Core lifecycle event,
- audio bridge stop,
- save/load/session tests.

## Commit C2[2].7 — Alert ducking + bus/settings parity

- concurrency policy,
- ducking,
- all active buses,
- settings recovery.

### Gate: 17C complete

## Commit C2[2].8 — Guidance route + F1

- dashboard entry,
- input map,
- visibility ownership,
- open/reopen tests.

## Commit C2[2].9 — Guidance progression

- real event/action progression,
- show-me-where lifecycle,
- idle/status behavior.

## Commit C2[2].10 — Accessibility + snapshots + persistence

- keyboard/a11y,
- snapshot,
- save/load/session replacement.

### Gate: 17B complete

## Commit C2[2].11 — Integrated playthrough closure

- Day 1→2 manual path,
- cause/effect route,
- confirmation,
- guidance,
- final audits/docs.

---

## Commit C2[2].12 — Successor: Plan 31 semantic vocabulary (new package)

- vocabulary type + per-kind disposition (§6.12),
- producer cleanup (11 handled-but-never-emitted),
- `DayEventVocabulary` absorbed and suffix heuristic retired (§6.12.1.7),
- gate tests migrated to the new authority (§40.16).

## Commit C2[2].13 — Successor: alert ducking/concurrency (new package)

- `AlertCoordinator` per §16A,
- priority table + per-class caps + ref-counted ducking,
- tests per §16A.4.

## Commit C2[2].14 — Successor: guidance deep verification (new package)

- stage inventory + gates (§47B.1),
- teach-before-demand fixes (§47B.3),
- deep battery + snapshot (§47B.4).

### Gate: full C2[2] flagship DoD

---
---

# 38. Test Matrix

| Area | Scenario | Expected |
|---|---|---|
| semantic events | emitted known kind | rendered or explicitly classified |
| semantic events | unknown kind | visible/test failure; not silent |
| briefing | owner failure | warning visible |
| briefing | actionable entry | opens live relevant panel |
| briefing | same seed | stable ordering/text semantics |
| UI audio | ordinary click | exactly one cue |
| UI audio | invalid action | exactly one invalid/warning cue |
| UI audio | domain completion | no double-fire |
| inventory | craft/trade/gift/etc. acquisition | pickup confirmation once |
| inventory | load existing items | no pickup confirmation |
| ambience | shelter/surface transition | correct loop lifecycle |
| music | menu/game/ending | one correct catalog-resolved transition |
| radiation | exposure begin | geiger starts once |
| radiation | exposure end | geiger stops |
| radiation | session replacement | no orphan loop |
| alerts | stacked alerts | bounded concurrency |
| alerts | alert ends | duck state restores |
| buses | all active buses | audio selftest validates |
| settings | bus slider/recovery | correct runtime mapping |
| guidance | dashboard route | opens |
| guidance | F1 | opens |
| guidance | close/reopen | works repeatedly |
| guidance | show-me-where | opens correct live surface |
| guidance | real action | intended step advances |
| guidance | save/load | step persists |
| guidance | assistance off | no forced nags; explicit help still available |
| accessibility | keyboard only | full usable path |
| integration | audio enabled vs disabled | same simulation checksum |
| parity gate | new emitted kind without vocabulary entry | CI failure naming kind + producer |
| parity matrix | regenerated | `--check` clean; zero stale rows |
| briefing | generic unknown kind | visible System Activity row; classified |
| briefing | heartbeat kind | suppressed by classification; tested |
| briefing | attribution grouping | kind → §6.13 default category |
| route | matrix route ↔ registry | every route resolves + player-navigable |
| route | entity binding | survivor/expedition/sanitation deep-link |
| route | generic entry | no route affordance |
| guidance | stage inventory | journey ↔ inventory gate parity |
| guidance | trigger classification | zero clock-based completions |
| telemetry | injected owner failure | one aggregated warning; verbatim log |
| telemetry | missing cue | single log; text path completes |
| ducking | deferred alert | observable emission when slot frees |
| ducking | restore base | settings volume, not cached literal |

---

# 39. Manual Playthrough Script

Run the repository's Day 1 → Day 2 checklist and add C2-specific observations.

## Phase 1 — New game

Verify:

- no stale audio from previous session,
- guidance state initializes,
- F1 opens,
- dashboard guidance opens,
- assistance behavior matches setting.

## Phase 2 — Rations / water

Perform early survival actions.

Verify:

- UI confirms exactly once,
- guidance recognizes real progress,
- no false stage completion from merely opening a panel.

## Phase 3 — Craft

Complete a craft.

Verify:

- crafting confirmation,
- resulting item acquisition does not double-fire,
- guidance advances if craft is the current taught objective.

## Phase 4 — Expedition

Dispatch/complete according to available flow.

Verify:

- relevant action cues,
- return/acquisition feedback,
- guidance progression.

## Phase 5 — Advance day

Verify:

- semantic events collected,
- known kinds rendered,
- player-caused consequences attributable,
- owner failure visible if injected test/failure path is available,
- relevant briefing route works.

## Phase 6 — Radiation/hazard

Trigger a safe test scenario through existing harness/fixture.

Verify:

- geiger start,
- geiger stop,
- danger cue,
- alert priority,
- ducking restore.

## Phase 7 — Save/load

Save and reload.

Verify:

- onboarding progress,
- no pickup-on-load cue,
- ambience/music correct,
- no duplicate subscriptions,
- guidance F1 still works.

---

# 40. Failure Modes and Corrective Actions

## 40.1 Event exists but briefing is silent

Likely:

- semantic vocabulary mismatch,
- missing handler,
- silent default behavior.

Fix:

- repair via Plan 31 authority,
- do not add one-off UI string checks.

## 40.2 Briefing route opens wrong panel/entity

Likely:

- route mapping uses kind instead of entity semantics,
- `PrimaryId` interpreted inconsistently.

Fix:

- define kind-specific route interpretation centrally.

## 40.3 Guidance advances too early

Likely:

- panel-open treated as action-complete,
- clock/day used as proxy.

Fix:

- bind to authoritative completion signal.

## 40.4 Guidance cannot reopen after dismissal

Likely:

- dismissal mutates persistent disable flag,
- route visibility coupled to journey status.

Fix:

- separate “current hint dismissed” from “help route available.”

## 40.5 F1 works before load but not after

Likely:

- input handler bound to old session/panel,
- teardown removes handler without rebind.

Fix:

- bind in canonical lifecycle path and test New Game/Load.

## 40.6 Every button now double-clicks audibly

Likely:

- shared helper added cue while panels/host already emit.

Fix:

- enforce ownership matrix and remove duplicate lower-value emitters.

## 40.7 Pickup sound plays during load

Likely:

- inventory mutation seam cannot distinguish restore from acquisition.

Fix:

- route playback only from semantic acquisition transaction, not raw quantity setter.

## 40.8 Geiger never stops

Likely:

- end signal not emitted on one transition path,
- bridge misses teardown.

Fix:

- state-transition test all radiation exits + defensive teardown stop.

## 40.9 Ducking remains after alert

Likely:

- nested alert reference count/lifecycle incorrect.

Fix:

- use deterministic active-alert count/state rather than one-shot decrement assumptions.

## 40.10 Audio changes checksum

Critical defect.

Likely:

- audio path consumes gameplay RNG,
- audio mutates gameplay state,
- timing callback affects simulation order.

Fix before merge.

---

## 40.11 Heartbeat classification misfires on a real player-facing kind

Likely: a genuinely player-relevant kind was named with the `_ticked` suffix
and is now suppressed as a heartbeat.

Fix: rename the kind at the producer (Plan 31 vocabulary) — never special-case
the string in the builder. Add a vocabulary test pinning the kind as
player-facing.

## 40.12 Generic section floods the briefing

Likely: a producer emits high-frequency non-heartbeat kinds (status noise
without the `_ticked` suffix).

Fix: classify the family as internal in the vocabulary (documented, tested) —
or fix the producer to emit transitions instead of steady state (§42.1).
Never raise the overflow cap to "fix" flooding.

## 40.13 Parity matrix rots

Likely: the matrix document was edited by hand or regenerated from stale
producers.

Fix: matrix is generated only; the gate test compares it against live
producer sources. Never hand-edit rows; regenerate.

## 40.14 Guidance deep-link honors route but not entity

Likely: panel bind action ignores the entity context carried by the entry.

Fix: extend the panel's bind signature additively; do not open panels to
default state while claiming deep-link semantics in the matrix.

## 40.15 Ducking applied to the wrong bus after settings change

Likely: duck depth applied as absolute volume, clobbering a live settings
change.

Fix: restore from the settings authority (§16A.3); duck depth is a delta
applied to the current settings base, reapplied on settings change.

## 40.16 Plan 31 lands and retires DayEventVocabulary while tests still pin it

Likely: successor plan replaced the vocabulary but kept the C2 gate tests
importing it.

Fix: Plan 31's package owns migrating the gate tests to the new authority in
the same package; the suffix heuristic retires with its tests, never after.

---

# 41. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| stale 17A implemented despite erratum | Medium | High | explicit superseded gate; Plan 31 authority |
| Plan 31 kind names change mid-work | Medium | Medium | delay event-driven 17B binding until contract stable |
| 16B not complete | Medium | High | prerequisite gate |
| UI audio double-fire | High | Medium | ownership matrix + exactly-once probe |
| item pickup sounds on restore | Medium | Medium | acquisition seam, not raw setter |
| alert duck state leak | Medium | Medium | lifecycle/ref-count tests |
| guidance route opens prototype panel | Medium | High | liveness gate compatibility |
| onboarding subscriptions leak | Medium | Medium | repeated session-swap test |
| F1 collision | Low–Med | Medium | input-map audit |
| snapshot churn | Medium | Low | stable fixture state |
| geiger state restore mismatch | Medium | Medium | load tests while exposed/not exposed |
| audio bus missing settings control | Medium | Medium | bus/settings parity sweep |
| semantic event route ambiguity | Medium | Medium | explicit kind/entity route mapping |
| new audio logic impacts sim | Low | Critical | checksum parity |
| heartbeat misclassification hides a real event | Medium | Medium | rename at producer; vocabulary pin tests |
| generic section flooding | Medium | Medium | classification review; producer transition discipline |
| parity matrix hand-edits | Medium | Low | generated-only policy + gate |
| deep-link honors route not entity | Medium | Medium | bind-signature additive rule + test |
| duck clobbers live settings change | Medium | Medium | settings-base restore (§16A.3) |
| guidance stage inventory rots | Medium | Low | inventory ↔ journey gate |

---

# 42. Performance and Noise Budgets

Legibility can fail through over-reporting just as badly as silence.

## 42.1 Briefing budget

Do not show every steady-state measurement as an event.

Prefer:

- transitions,
- decisions,
- threshold crossings,
- failures,
- meaningful consequences.

Avoid:

- every hunger percentage update,
- every unchanged resource count,
- repeated identical low-value events.

Plan 31 may own aggregation semantics; C2 consumes them.

## 42.2 Audio budget

Do not sound every internal mutation.

Prefer one cue per semantic player act.

For high-frequency actions, verify the shared helper does not produce fatiguing duplicate cues.

## 42.3 Guidance budget

Do not interrupt on every event.

Use:

- current journey stage,
- failed action count,
- idle state,
- explicit assistance preference.

---

# 43. Documentation Updates

At closure update:

- `docs/audio/SILENCE_AUDIT.md`
- `docs/audio/AUDIO_QA_REPORT.md` if its blocker/status changed
- `docs/ui/SNAPSHOT_COVERAGE.md`
- manual playthrough notes
- any event-semantic documentation owned by Plan 31
- onboarding/help documentation if present.

## 43.1 Audit freshness rule

Do not leave the source audit saying “PARTIAL” after the implementation is complete.

Each item must become:

- CLOSED,
- PARTIAL with exact remaining blocker,
- DEFERRED with owner,
- INVALIDATED by architecture change.

---

# 44. Verification Checklist

Run after each substantial workstream and once at final closure.

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --audio-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also:

```bash
python generate-audio-catalog.py --check
```

and manual:

```text
docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md
Day 1 → Day 2
```

Use the actual repository invocation if script locations differ.

---

# 45. Flagship Definition of Done

## Semantic cause/effect

- [ ] Obsolete 17A implementation is not performed.
- [ ] Plan 31 is the semantic-kind authority.
- [ ] Known emitted events cannot silently disappear.
- [ ] producer/consumer parity is testable.
- [ ] player-caused vs autonomous outcomes are distinguishable where supported.
- [ ] actionable briefing entries route to live panels.
- [ ] event history parity exists where intended.
- [ ] day-owner failure is visible.
- [ ] deterministic ordering holds.

## Confirmation/audio

- [ ] audio audit rerun.
- [ ] catalog validation green.
- [ ] shared UI action confirmation centralized where appropriate.
- [ ] exactly one cue per ordinary semantic act.
- [ ] ambience driven by real campaign context.
- [ ] music transitions catalog-resolved.
- [ ] item pickup covers all canonical acquisition paths.
- [ ] item pickup does not trigger on load.
- [ ] danger cues use real hazard events.
- [ ] radiation exposes explicit end-of-exposure signal.
- [ ] geiger loop starts/stops correctly.
- [ ] semantic asset collisions resolved/classified.
- [ ] alert concurrency bounded.
- [ ] ducking restores correctly.
- [ ] all active buses validated.
- [ ] all active player-facing buses have settings parity.
- [ ] audio-disabled simulation checksum matches.

## Confirmation/audio (expansion phases)

- [ ] Plan 31 vocabulary authority registered with per-kind disposition,
- [ ] 11 handled-but-never-emitted cases wired/reclassified/removed,
- [ ] parity matrix regenerated from the new authority (`--check` green),
- [ ] acquisition-route matrix (§12A) fully verified per route,
- [ ] acquisition seam distinguishes acquisition from restore/normalization,
- [ ] alert coordinator: priority table + per-class concurrency caps,
- [ ] ducking ref-counted, settings-base restore, teardown-safe (§16A),
- [ ] deferred alerts observable, never silently dropped,
- [ ] diagnostics: aggregated owner-failure warnings, once-per-cue logs,
- [ ] diagnostic order deterministic, consumes no gameplay RNG.

## Guidance (expansion phases)

- [ ] stage inventory published and gate-enforced (§47B.1),
- [ ] every stage trigger classified; zero clock-based completions (§47B.2),
- [ ] teach-before-demand matrix executed against live campaign start,
- [ ] visibility-race battery green (§47B.4.1),
- [ ] subscription-leak soak ×3 green (§47B.4.2),
- [ ] F2 (guidance) verified after load/new game/session replacement,
- [ ] assistance-level matrix verified across all levels,
- [ ] keyboard-only walk green including show-me-where return path,
- [ ] snapshot fixture live and coverage doc updated.

## Guidance

- [ ] guidance opens from dashboard.
- [ ] guidance opens via F1.
- [ ] guidance is reopenable.
- [ ] visibility owned by route/host lifecycle.
- [ ] “show me where” uses existing live routing seam.
- [ ] guidance progression uses real events/actions.
- [ ] panel open alone does not falsely complete gameplay objectives.
- [ ] idle/failure nudges reuse existing fields.
- [ ] status bar is legible and dismissible.
- [ ] assistance-off mode remains respected.
- [ ] Day 1–3 teach-before-demand audit passes.
- [ ] keyboard-only path works.
- [ ] color is not the sole indicator.
- [ ] snapshot coverage is real/live.
- [ ] save/load preserves journey.
- [ ] session replacement rebinds correctly.
- [ ] no duplicate onboarding subscriptions.

## Full integration

- [ ] valid action → one confirmation → real mutation.
- [ ] invalid action → visible reason + one invalid cue.
- [ ] meaningful decision → semantic consequence → briefing.
- [ ] briefing → relevant live surface.
- [ ] guided action → real completion → journey advancement.
- [ ] save/load preserves progression and audio lifecycle.
- [ ] full automated verification green.
- [ ] manual Day 1 → Day 2 playthrough green.
- [ ] audits/docs updated.

---


---

# 47A. Workstream 17D — Briefing Route Liveness and Event-Log Parity (expanded from §6.7/§6.9)

> Added by the C2 expansion. §6.7/§6.9 defined the *contract*; this workstream
> defines the *implementation and tests* for briefing entries that route
> somewhere useful, and for briefing/event-log parity.

## 47A.1 Route resolution pipeline

```text
briefing entry
  → DeepLinkRoute (already a field on DailyBriefingEntry)
  → PanelRegistry.Resolve(route)          — must resolve; else fail visible
  → descriptor.IsPlayerNavigable          — must be true; else no affordance
  → entity binding: PrimaryId/SecondaryId → the target panel's bind action
  → OpenPlayerPanel(route)                — existing seam, exactly one confirm cue
```

Rules:

- a route on an entry is a **promise**: the panel must bind the entry's
  entities (e.g., a survivor-condition entry routes to the survivors detail
  with that survivor preselected where the panel supports it);
- if entity preselection is not supported by the target panel, route to the
  panel and let it show its default state — the route must never fabricate a
  deep-link it cannot honor;
- unknown route / prototype route → the existing visible diagnostics in
  `OpenPlayerPanel` are the failure path (§3.7); no silent dead affordances.

## 47A.2 Route assignment matrix (builder-side)

Route assignment lives **next to the handler case** in the builder (one
place), populated only when all §6.7 conditions hold. Initial mapping:

| Emitted kind | Route | Entity binding |
|---|---|---|
| `survivor_perished` / `survivor_condition` | `survivors` (detail) | `PrimaryId` = survivor id |
| `radio_distress_active` / `radio_distress_expiring` | `radio` | signal frequency context |
| `radio_intercept*` | `events` (log) | — |
| `crafting_completed` / `workshop_*` | `crafting` / `foundry` | production context |
| `expedition_milestone` | `expeditions` | expedition id |
| `sanitation_spill` | `sanitation` | room id |
| `market_shocks_active` | `economy` | category id |
| `hazard_warning` / `shelter_fire` | `emergency_response` | hazard context |
| `subterranean_*` | `holdfast` | sector id |
| generic (System Activity) | none | — (no affordance on unclassified kinds) |

Adding a route to a kind without a live panel for it is forbidden — the route
follows the panel registry, never the reverse.

## 47A.3 Event-log parity

- the briefing and `events` log read the **same** `DayAdvancedEventArgs`
  payload; the log is the full-fidelity view, the briefing is the curated
  view (cap + dedup + classification);
- an event visible in the briefing must be findable in the events log by
  `PrimaryId` the same session; a test pins this for at least one kind per
  attribution category (§6.13);
- no second history store: parity is achieved by sharing the source payload,
  not by copying rows.

## 47A.4 Tests

- every route in the §47A.2 matrix resolves to a registered, player-navigable
  panel (source-gate over the matrix ↔ registry parity),
- route open honors the entity context for at least: survivor, expedition,
  sanitation room,
- briefing-visible event is events-log-findable same session,
- generic-section entries carry **no** route affordance,
- route open plays exactly one confirm cue (owned by `OpenPlayerPanel`;
  §9.2 ownership table).


---

# 47B. Workstream 17B expansion — Guidance Content Authority and Deep Verification

> Added by the C2 expansion. 2026-09-15 re-measurement found the guidance
> *mechanics* (route, visibility, save store, show-me-where) already built —
> the plan's premises there are stale. What remains unverified is the
> *content contract*: which stages exist, what completes them, and whether
> the first three days teach before they demand.

## 47B.1 Stage inventory (read-first; do not redesign)

Enumerate from the live `OnboardingJourney` (not from this document):

- every stage id, in authored order,
- each stage's completion trigger (event kind, action signal, or manual),
- each stage's hint text and show-me-where target,
- each stage's dismissal semantics.

Publish the inventory as `docs/onboarding/GUIDANCE_STAGE_INVENTORY.md` and
gate it the way the parity matrix is gated: a stage added to the journey
without an inventory row fails CI. The inventory is the contract between the
journey (§3.2 authority) and every consumer.

## 47B.2 Completion-trigger classification

For each stage, exactly one trigger class:

| Class | Source | Example |
|---|---|---|
| `EVENT` | semantic day event (post-Plan 31 binding; until then an explicit, stable kind from the current vocabulary) | first craft completed |
| `ACTION` | authoritative player-action completion signal (host command result) | expedition dispatched |
| `READ` | player opened the teaching surface and the panel's own state confirms the concept was exercised | water filter inspected AND filter actually cycled |
| `DERIVED` | purely derivable from live authority state (never the clock) | power grid online |

Forbidden: day-number completion, wall-clock completion, panel-open-only
completion (§27.3). `DERIVED` is legal only because it reads the authority,
not the clock.

## 47B.3 Teach-before-demand audit (execute §29.1 for real)

Produce the matrix from the live campaign start state:

| Action first demanded (day 1–3) | Demanding authority | First taught (stage id) | Taught before demand? | Fix |
|---|---|---|---|---|

A demand with no preceding teaching stage is a defect in the journey content,
fixed in the journey — never by weakening the demand or faking completion.

## 47B.4 Deep verification battery (executes §33 in full)

Beyond the unit-level tests, run:

1. **Visibility race battery** (§25.1): construct → deferred init → open →
   close → session swap → load → reopen; a late `_Ready` must not undo an
   explicit open.
2. **Subscription-leak soak** (§32.4): new game → load → new game ×3; then
   one completion event → exactly one progression callback.
3. **F-binding after load**: `ashfall_guidance` opens the reloaded session's
   panel (bind in the canonical lifecycle path).
4. **Assistance-off matrix**: for each `OnboardingAssistance` level — no
   forced interruptions; explicit route + F2 still function; status label
   reflects the level truthfully.
5. **Keyboard-only walk**: dashboard nav row → guidance → show-me-where →
   target panel → close → F2 reopen, no mouse.
6. **Snapshot fixture** (§31): the guidance overlay at a stable representative
   stage, real route, real binding; `SNAPSHOT_COVERAGE.md` updated.

## 47B.5 Acceptance gate (extends §34)

- [ ] stage inventory published and gate-enforced,
- [ ] every stage's trigger classified; zero clock-based completions,
- [ ] teach-before-demand matrix passes for days 1–3,
- [ ] deep verification battery green,
- [ ] guidance content authority documented for future stage authors.

# 46. Closure Report Template

```markdown
## C2[2] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Audio selftest:
- Audio catalog check:
- Event kinds emitted:
- Event kinds handled:
- Unknown/unhandled kinds:
- Active buses:

### Plan 31 Compatibility
- Semantic authority:
- No-silent-drop test:
- Producer/consumer parity:
- Briefing route parity:
- Event-log parity:
- Owner-failure visibility:
- Result:

### 17C — Confirmation
- Shared UI cue ownership:
- Exactly-once result:
- Ambience:
- Music:
- Item acquisition:
- Hazard cues:
- Radiation end signal:
- Geiger lifecycle:
- Asset de-duplication:
- Alert concurrency:
- Ducking:
- Bus validation:
- Settings parity:
- Audio/simulation checksum parity:
- Result:

### 17B — Guidance
- Dashboard route:
- F1:
- Open/reopen:
- Show me where:
- Event/action progression:
- Status bar:
- Assistance-off behavior:
- Teach-before-demand:
- Accessibility:
- Snapshot:
- Save/load:
- Session replacement:
- Result:

### Manual Playthrough
- New Game:
- Rations/water:
- Craft:
- Expedition:
- Day advance:
- Radiation/hazard:
- Save/load:

### Full Verification
- dotnet build Core.Tests:
- dotnet test:
- dotnet build Ashfall:
- data-integrity:
- bridge-selftest:
- audio-selftest:
- audio catalog:
- triad-drift-gate:
- verify-fast:

### Remaining Debt
- Plan 31:
- Plan 16B:
- Plan 16C:
- Audio:
- Guidance:
- UI:
```

---

---

# 48. Appendix — Full Implementation Checklist (successor execution order)

> Line-item ledger of every requirement in this plan, grouped by owning
> package (§36.3). Executed items are checked with their evidence pointer;
> unchecked items are the successor packages' scope. Maintained as the
> single pass/fail surface for the C2[2] flagship.

## 48.1 Package `C2-PLAN17-LEGIBILITY` (executed 2026-09-15, bounded)

- [x] Obsolete 17A substeps NOT implemented (erratum honored) — §1.2/§6
- [x] Measured evidence appendix recorded (70/31/50/11) — §1.4
- [x] Builder default case: no silent drops — §6.3 (shipped §6.14)
- [x] Heartbeat classification documented + tested — §6.3, §42.1
- [x] Generic renderer deterministic; overflow-capped — §6.10, §42.1
- [x] Owner-failure briefing warning (aggregated) — §6.8
- [x] Parity matrix generated + gate-enforced — §6.4
- [x] Plan 31 binding contract written — §6.12
- [x] Attribution default mapping recorded — §6.13
- [x] Radiation exposure begin/end events (Core, transient tracking) — §14.2
- [x] Restore never replays exposure transitions (test-pinned) — §14.4
- [x] Geiger loop lifecycle wired (start once / no restart spam / stop / teardown) — §14.3
- [x] Guidance F-binding added (F2; F1 collision documented) — §24
- [x] F-toggle reopen semantics (open/close, never permanent disable) — §24.2
- [x] Simulation neutrality structural (observational events only) — §3.6
- [x] Focused verification green (17/17 new + 58/58 adjacent) — §44 (full suite skipped by directive)
- [x] Baseline re-measured and recorded — §5, §5.4

## 48.2 Package `PLAN-31-SEMANTIC-VOCABULARY` (open)

- [ ] Canonical vocabulary type registered in Core — §6.12.1.1
- [ ] Per-kind disposition (PLAYER_FACING/GENERIC/HEARTBEAT/RETIRED) complete — §6.12.1.2
- [ ] All 70 emitted kinds resolve to one entry (source-gate) — §6.12.2
- [ ] 11 handled-but-never-emitted cases wired/reclassified/removed — §6.12.1.3
- [ ] Producer-side kind literals replaced by vocabulary constants — §6.12.1.1
- [ ] Attribution metadata carried per kind — §6.12.1.5
- [ ] `DayEventVocabulary` absorbed; suffix heuristic retired — §6.12.1.7
- [ ] Gate tests migrated to the new authority — §40.16
- [ ] Parity matrix regeneration `--check` mode — §6.12.2
- [ ] Determinism under shuffled insertion — §6.12.2

## 48.3 Package `C2-ALERT-DUCKING` (open; spec §16A)

- [ ] `AlertCoordinator` type beside `AudioManager` — §16A.1
- [ ] Priority table implemented from event payloads — §16A.2
- [ ] Per-class concurrency caps with observable deferral — §16A.2
- [ ] Ref-counted ducking; nested alerts never over-duck — §16A.3
- [ ] Restore from settings base (no cached literal) — §16A.3
- [ ] Teardown clears duck state + stops cues — §16A.3
- [ ] Tests: §16A.4 six cases — §16.5 extended
- [ ] Zero gameplay-state reads (structural probe) — §16A.4

## 48.4 Package `C2-GUIDANCE-DEEP-VERIFY` (open; spec §47B)

- [ ] Stage inventory published + gate-enforced — §47B.1
- [ ] Trigger classification per stage; zero clock completions — §47B.2
- [ ] Teach-before-demand matrix executed (days 1–3) — §47B.3
- [ ] Visibility-race battery — §47B.4.1
- [ ] Subscription-leak soak ×3 — §47B.4.2
- [ ] F-binding after load/new game — §47B.4.3
- [ ] Assistance-level matrix — §47B.4.4
- [ ] Keyboard-only walk — §47B.4.5
- [ ] Snapshot fixture + coverage doc — §47B.4.6, §31

## 48.5 Package `C2-ITEM-ACQUISITION-PARITY` (open; spec §12A)

- [ ] Route matrix verified against live authorities (rows 2–8) — §12A
- [ ] Acquisition seam distinguishes net acquisition from restore — §12.2
- [ ] One cue per transaction rule — §12.3
- [ ] Restore-exclusion regression test against the real codec — §12.4
- [ ] Craft/trade/gift/scavenge/autopsy confirmation coverage — §12.1

## 48.6 Package `C2-AUDIO-AUDITS` (open; Phases H/J + Phase A refresh)

- [ ] Audio selftest re-run; counts re-recorded (historical 245/70 not pinned) — §8.3
- [ ] Catalog check green — §8.1
- [ ] Semantic asset collisions re-audited; resolved or alias-documented — §15
- [ ] Bus topology re-measured; all active buses validated — §17.1/§17.2
- [ ] Settings recovery smoke executed — §17.3
- [ ] Prose audit findings converted to assertions — §8.2

## 48.7 Integrated closure (after 48.2–48.6)

- [ ] Flow A–F journeys green — §35
- [ ] Manual Day 1 → Day 2 playthrough with C2 observations — §39
- [ ] Test matrix full pass — §38
- [ ] Documentation updates (§43) with §43.1 freshness rule
- [ ] Closure report completed from the §46 template
- [ ] Governance ledgers updated (INTEGRATION_PLANS.md, WORKTREE_OWNERSHIP.md)

---

# 47. Final Execution Directive

Execute this plan as a continuity repair across three existing channels:

```text
semantic cause
→ understandable consequence
→ useful route
→ contextual guidance
→ exactly-once confirmation
```

The implementation is not complete merely because:

- the briefing contains text,
- guidance can be constructed,
- F1 opens once,
- a button makes a sound,
- the geiger can start,
- an audio selftest counts assets.

The system is complete when the player can understand a real causal chain and act on it:

```text
I did X
→ the game confirmed X
→ Y changed
→ the briefing explains Y
→ I can open the screen that matters
→ guidance can help me if needed
→ save/load preserves the understanding and state
```

Most importantly, honor the source erratum:

> **Do not spend implementation effort adding day-event producers that already exist. Fix/consume the semantic vocabulary through Plan 31 and spend this C2 package on the actual continuity failures: legibility, guidance reachability, audio lifecycle, and feedback correctness.**
