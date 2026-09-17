# C1 — Flagship Integration Plan [8]: The Event Layer Speaks — Semantic Day Events, Navigable Briefings & Replayable Diagnostics

> **Output:** `C1_planintegration[8].md`
>
> **Source baseline:** Plan 31 — The Event Layer Speaks: Semantic Kinds, No Silent Drops
>
> **Wave:** Continuity Wave 4 — *The World Beyond the Gate*
>
> **Corrects:** Wave-1 Plan 17A's original premise. Re-verification shows the typed day-event transport is live and all campaign-day owners emit. The actual defect is vocabulary drift and silent semantic loss at the briefing consumer.
>
> **Mandatory execution order:** 31A → 31B → 31C.
>
> **Wave sequencing rule:** land 31A before other Wave-4 producers (Plans 30, 32, 33, 34) add new world events, otherwise those plans will author against an unstable vocabulary.
>
> **Primary architectural rule:** use the typed day-event channel that already ships. Do not add another event bus, another briefing framework, or player-facing heartbeat noise.
>
> **Completion standard:** zero emitted event kinds silently disappear, zero semantic kinds remain permanently producerless unless explicitly reserved, every player-facing line is attributable and either actionable or explicitly informational, and the same event stream can reproduce a reported day in diagnostics.

---

# 0. Mission

ASHFALL does not need a new event architecture.

The existing campaign-day pipeline already has the important pieces:

```text
IDayAdvanceOwner
      │
      ▼
TickDay(day, List<DayStateChangeEvent>)
      │
      ▼
CampaignDayCoordinator
      │
      ├── events
      ├── per-owner report
      └── failure information
      │
      ▼
DayAdvancedEventArgs
      │
      ▼
DailyBriefingReportBuilder
      │
      ▼
player-facing daily briefing
```

The source evidence says that transport works.

The failure is semantic:

- producers and consumer use unowned string literals;
- producers mostly emit heartbeat/tick kinds;
- the builder handles a richer semantic vocabulary that producers rarely use;
- 20 emitted kinds currently have no matching consumer branch;
- the switch reportedly has no default, so those events disappear silently;
- owner failures are captured but not surfaced;
- some handled semantic kinds are never produced;
- the player report cannot reliably connect a consequence to the panel where it can be acted upon;
- support/debugging does not preserve a machine-readable day record that matches what the player saw.

This plan converts the event layer from "a transport that technically works" into a semantic contract.

The target architecture is:

```text
             ┌─────────────────────────────┐
             │      DayEventKinds          │
             │ canonical Core vocabulary   │
             └──────────────┬──────────────┘
                            │
          ┌─────────────────┼──────────────────────┐
          ▼                 ▼                      ▼
      survivors          shelter                world
       owners             owners                 owners
          │                 │                      │
          └─────────────────┼──────────────────────┘
                            ▼
                  DayStateChangeEvent
             kind / source / ids / value
                 cause / actor where needed
                            │
                   ┌────────┴────────┐
                   ▼                 ▼
         Daily Briefing         Day Record
         semantic report        diagnostics JSONL
                   │                 │
                   ▼                 ▼
           route to live UI      replay by seed/day
```

The guiding principle is simple:

> A day with no meaningful transition emits nothing for the briefing. A day with a meaningful transition must not be allowed to disappear silently.

---

# 1. Source-Evidence Interpretation

## 1.1 The event transport is healthy

The source plan re-measures all campaign-day owners and reports that all 19 owner classes emit at least one `DayStateChangeEvent`.

Therefore:
- do not rebuild transport;
- do not replace coordinator;
- do not add parallel event bus;
- do not treat fallback briefing path as primary.

## 1.2 The consumer silently drops most emitted vocabulary

The current briefing builder reportedly handles only a subset of emitted kinds and has no `default`.

This is the primary correctness defect.

A silent unhandled semantic event is worse than a noisy failure because:
- simulation changed;
- player receives no explanation;
- tests may still pass;
- support has no clue what happened.

## 1.3 Heartbeats are not briefing content

Kinds like:
- `power_ticked`;
- `market_ticked`;
- `needs_ticked`;
- `world_ticked`;
are scheduler diagnostics, not player consequences.

They should:
- become semantic transitions;
- or disappear from the player-event channel.

## 1.4 The builder already knows richer semantic forms

The consumer reportedly already supports kinds such as:
- `survivor_condition`;
- `resource_delta`;
- `shelter_consequence`;
- `weather_condition`;
- `radio_transmission`;
- `crafting_production`.

The cheapest early win is to make producers emit these existing semantic forms.

## 1.5 Owner failure already exists in the day report

`DayOwnerReport.FailureMessage` and `HasFailures` exist.

A skipped owner that looks like a quiet day is a dangerous observability failure.

31A must surface it.

## 1.6 Volume control already exists

`maxEntriesPerSection` and `AddSectionIfNotEmpty(...)` already provide report volume constraints.

Do not replace them with a new pagination architecture unless current UI requires it.

---

# 2. Non-Negotiable Event Invariants

## INV-31.1 — One canonical kind vocabulary

All production `DayStateChangeEvent.Kind` values must originate from `DayEventKinds`.

No scattered string literals.

## INV-31.2 — Event kinds are semantic, not scheduler heartbeats

Player-facing day events represent:
- transition;
- decision;
- consequence;
- warning;
- meaningful delta.

Not:
- "system ticked".

## INV-31.3 — No silent consumer drop

Every emitted production kind must be:
- handled;
- explicitly diagnostic-only;
- or explicitly reserved.

An unknown player-event kind must never vanish at a switch end.

## INV-31.4 — No permanently producerless handled kinds

A handled semantic kind must be:
- emitted;
- reserved with reason;
- or removed from current vocabulary.

## INV-31.5 — Cause and ambient origin remain distinct

An event may be caused by:
- player decision;
- named actor;
- world/system transition;
- ambient condition.

Do not label ambient consequences as "your decision" merely because they happened on the same day.

## INV-31.6 — Event ordering is deterministic

Same seed and same day state must produce the same:
- event order;
- aggregation;
- briefing line order.

## INV-31.7 — Owner failures surface visibly

If a day owner fails:
- briefing/dev UI must expose a warning;
- diagnostic record must include it.

## INV-31.8 — Briefing routes only to live panels

No route may point to:
- shelved;
- prototype;
- retired;
- dead panel.

## INV-31.9 — Player line and diagnostic record share identity

The same `kind` and `SourceOwnerId` must allow support to map:
player report ↔ diagnostic day record.

## INV-31.10 — Debug day-record output is opt-in/dev-only

Release builds do not silently write campaign telemetry to disk by default.

---

# 3. Definition of Done

Plan 31 is complete only when all are true:

- `DayEventKinds` exists in Core;
- every production event kind resolves to it;
- every emitted kind is handled/reserved/diagnostic by contract;
- every handled semantic kind is emitted or explicitly reserved;
- 20 heartbeat/drop kinds are reconciled one by one;
- producers emit transitions rather than routine ticks;
- the six handled-but-producerless semantic kinds are connected where appropriate;
- player-caused events carry causal attribution;
- owner failures appear in briefing/diagnostics;
- unknown semantic kind cannot disappear silently;
- event aggregation is deterministic;
- report volume remains bounded at 1280×800;
- briefing text is localization-key driven;
- every briefing entry either routes to a live panel or is explicitly informational;
- routing reuses existing `OpenPlayerPanel` seam;
- daily history survives modal dismissal through journal/event-log parity;
- ordering prioritizes severe events predictably;
- briefing supports keyboard activation/accessibility;
- quiet/normal/crisis snapshots fit without overflow;
- debug builds can emit stable per-day JSONL records;
- records include seed, day, owner order/timing, events, failures, headline state;
- diagnostic records redact paths/user information;
- output rotates under a size bound;
- ring buffer retains last N days for crash dump;
- CLI replay can reproduce a recorded day from seed/state;
- release builds do not write day records by default;
- day record ↔ briefing identity matches;
- full verification and real-campaign journey are green.

---

# 4. Phase P0 — Re-verify the Real Event Pipeline

## P0.1 Freeze repository baseline

Capture:

```text
commit SHA
branch
dirty-file count
campaign-day owner count
DayStateChangeEvent construction sites
unique emitted Kind strings
DailyBriefingReportBuilder handled kinds
builder default branch present?
fallback briefing call sites
owner failure fields/consumers
briefing route/modal implementation
events_log route
journal route
snapshot dimensions
```

Do not execute this plan from the source plan's old line numbers without rechecking.

---

## P0.2 Produce event vocabulary matrix

Before editing, generate:

```text
kind
producers
producer_count
handled_by_briefing
briefing_section
meaningful_transition?
heartbeat?
player_cause_possible?
route
reserved?
verdict
```

Verdicts:
- KEEP_SEMANTIC
- CONVERT_HEARTBEAT
- MERGE
- REMOVE
- RESERVED
- DIAGNOSTIC_ONLY

This matrix is the migration ledger.

---

## P0.3 Recount owner emissions

For each of the 19 registered owners:

```text
owner_id
owner_type
events_emitted
semantic kinds
heartbeat kinds
failure reporting
```

Confirm the corrected premise:
- all owners emit;
- transport path is active.

---

## P0.4 Reproduce silent-drop defect

Add a test with a currently dropped kind.

Expected pre-fix:
- builder returns no corresponding entry.

Preserve as regression test.

---

## P0.5 Reproduce owner-failure invisibility

Build day args with:
- owner failure;
- no meaningful events.

Expected pre-fix:
- report looks quiet.

After repair:
- warning entry must exist.

---

# TASK 31A — Own the Vocabulary and Convert Heartbeats into Meaning

# 31A.0 Goal

Create one Core vocabulary and make every producer emit a transition, decision, warning, or delta with defined semantics.

---

## 31A.1 Create `DayEventKinds`

File:

`Assets/Ashfall.Core/Campaign/DayEventKinds.cs`

Preferred form:

```csharp
public static class DayEventKinds
{
    public const string SurvivorCondition = "survivor_condition";
    public const string ResourceDelta = "resource_delta";
    ...
}
```

Use current C# naming convention while preserving snake_case wire values.

---

## 31A.2 Add semantic metadata documentation

Every kind gets a doc comment defining:

```text
wire value
section/category
producer expectation
PrimaryId meaning
SecondaryId meaning
Numeric meaning
cause/actor semantics
aggregation key
severity default
```

Example:

```csharp
/// <summary>
/// Survivor condition changed materially.
/// PrimaryId: survivor id.
/// SecondaryId: condition/reason id.
/// Numeric: signed magnitude or severity when applicable.
/// </summary>
public const string SurvivorCondition = "survivor_condition";
```

---

## 31A.3 Seed vocabulary from current consumer

Start from the semantic kinds the builder already understands.

Do not start by copying all heartbeat strings into the canonical vocabulary.

The consumer's semantic forms are the initial target.

---

## 31A.4 Explicit kind registry helper

Expose a stable set for tests:

```csharp
public static IReadOnlySet<string> All { get; }
```

or reflection/test helper.

Purpose:
- source contract tests;
- reserved kind validation;
- localization coverage.

Avoid runtime reflection in hot path if unnecessary.

---

## 31A.5 Reserved kind policy

If a kind is defined for imminent Wave-4 producer but not emitted yet:

```text
kind
reserved=true
owner plan
expiry/target
```

Keep reservation explicit.

A kind must not remain indefinitely unused.

---

## 31A.6 Replace literal kinds in producer code

Migrate:
- `src/Main.CampaignOwners.cs`;
- performance campaign harness;
- fallback path;
- other production emitters.

Target:
- no `"kind_literal"` in `new DayStateChangeEvent` calls.

---

## 31A.7 Replace six handled-but-never-emitted forms first

Prioritize current source-plan set:

- `survivor_condition`;
- `resource_delta`;
- `shelter_consequence`;
- `weather_condition`;
- `radio_transmission`;
- `crafting_production`.

For each:
1. identify real owner;
2. identify meaningful transition;
3. define field meanings;
4. emit only when state changes.

---

## 31A.8 Survivor condition transitions

Potential triggers from current systems:
- severe hunger threshold crossed;
- severe fatigue;
- illness/quarantine transition;
- health/warmth critical state;
- recovery/discharge where day-owner path owns observation.

Do not emit every numeric needs tick.

Use threshold/delta semantics.

---

## 31A.9 Resource deltas

Use for meaningful inventory/economy changes:

```text
PrimaryId = resource/item id
SecondaryId = reason/source
Numeric = signed delta
```

Examples:
- ration consumption;
- production gain;
- spoilage loss;
- maintenance cost.

Avoid duplicate event if a more specific semantic kind already exists.

---

## 31A.10 Shelter consequence

Use for:
- brownout;
- breaker trip;
- room disabled;
- heating failure;
- refrigeration loss;
- infrastructure recovery.

Do not emit `power_ticked`.

---

## 31A.11 Weather condition

Use when:
- weather state transitions;
- severity threshold changes;
- hazard becomes relevant.

Do not emit routine `weather_ticked` if nothing changed.

---

## 31A.12 Radio transmission

Use when:
- actual transmission/intercept arrives;
- player-relevant radio state changes.

Do not emit routine radio tick.

---

## 31A.13 Crafting production

Use when:
- job completed;
- meaningful batch produced;
- failure/quality outcome occurs.

Merge or reconcile with existing `crafting_completed`.

Avoid two lines for same completion.

---

## 31A.14 Convert the 20 dropped kinds systematically

Create migration table:

```text
old_kind
old_owner
old_meaning
new_kind
transition condition
diagnostic replacement?
deleted?
test
```

No old kind disappears without an explicit row.

---

## 31A.15 Heartbeat conversion rule

For every `*_ticked` kind ask:

> What changed because this owner ticked?

If answer:
- nothing → emit nothing;
- state transition → emit semantic kind;
- diagnostic timing only → write diagnostic day record, not briefing event.

---

## 31A.16 Power owner examples

Instead of:
`power_ticked`

Emit when applicable:
- `grid_brownout`;
- `breaker_tripped`;
- `fuel_low`;
- `power_restored`;
or current semantic equivalents.

Only add kinds genuinely needed by current systems.

---

## 31A.17 Needs owner examples

Instead of:
`needs_ticked`

Emit:
- `survivor_condition`;
- `resource_delta`;
- `duty_consequence`;
as appropriate.

Do not flood report with every bar decay.

---

## 31A.18 Market owner examples

Instead of:
`market_ticked`

Emit:
- meaningful price shock;
- embargo;
- shortage;
- caravan change;
if those are actual modeled transitions.

Otherwise emit nothing.

---

## 31A.19 Memorial owner examples

Instead of:
`memorial_checked`

Emit:
- memorial created;
- mourning completed;
- legacy consequence;
if transition occurs.

No transition → no briefing event.

---

## 31A.20 World owner examples

Instead of:
`world_ticked`
or
`world_evolution_ticked`

Emit:
- location changed;
- hazard escalated;
- faction/world state transition;
- travel access change;
where modeled.

---

## 31A.21 Cause/actor extension decision

Re-read `DayStateChangeEvent`.

If existing fields can encode cause:
- reuse them.

Only add optional:
- `CauseId`;
- `ActorId`;
if current semantics cannot be expressed without overloading fields.

Do not expand DTO casually.

---

## 31A.22 Cause semantics

Define:

```text
CauseId:
  stable decision/action/event id

ActorId:
  survivor/player-owned actor/faction id where applicable
```

Ambient world events:
- cause empty or ambient source identifier.

---

## 31A.23 Causality examples

Player ration cut:

```text
kind = resource_delta / ration consequence
causeId = ration_policy:<id>
actorId = player/campaign decision marker
```

Storm:

```text
kind = weather_condition
causeId = ambient_weather
actorId = empty
```

---

## 31A.24 Owner failure event

Create a semantic/internal kind such as:

`owner_failure`

or report-specific failure entry if failures remain outside event stream.

Requirements:
- owner ID;
- safe failure summary;
- severity critical/warning;
- diagnostic record full detail in dev.

Do not expose stack traces to player.

---

## 31A.25 Briefing failure wording

Player-facing:

```text
Shelter systems report incomplete data for this day.
```

or localized equivalent.

Developer mode may include owner ID.

Do not make a failed owner look like a normal quiet day.

---

## 31A.26 Unknown kind default

Add default handling in builder.

Behavior:
- development/test: fail loudly or generate diagnostic unknown-kind entry;
- release: show safe informational fallback or route to generic log, depending policy.

Never silently discard.

---

## 31A.27 Contract gate: emitted kinds must exist

Source-scan production `DayStateChangeEvent` construction.

Fail if:
- literal kind not in `DayEventKinds`;
- unknown constant;
- dynamic uncontrolled string.

---

## 31A.28 Contract gate: kinds must have ownership

For each `DayEventKinds` member:
- emitted somewhere;
- reserved;
- diagnostic-only;
- or handled by synthetic compatibility path.

No unexplained orphan.

---

## 31A.29 Contract gate: consumer completeness

For every player-facing semantic kind:
- builder mapping exists;
- localization key exists;
- aggregation rule exists;
- route/info status defined.

---

## 31A.30 Test the contract gate itself

Inject:
- unknown emitted kind;
- unused non-reserved kind;
- handled string absent from registry.

Assert failures.

---

## 31A.31 Aggregation model

Define:

```text
aggregation key =
kind
+ primary grouping subject/resource
+ secondary reason where material
```

Examples:
- 5 survivors hungry → one section with affected IDs/count;
- 3 identical resource losses → aggregate signed total;
- severe death events remain distinct.

---

## 31A.32 Severity model

Canonical levels:

```text
critical
warning
decision
consequence
ambient
informational
```

Exact enum/string can differ.

Severity should derive from:
- kind metadata;
- event payload;
- domain rules.

---

## 31A.33 Stable aggregation order

Order by:
1. severity;
2. semantic section;
3. kind ordinal;
4. primary ID ordinal;
5. secondary ID ordinal.

Never rely on dictionary iteration.

---

## 31A.34 Volume budget

Reuse `maxEntriesPerSection`.

Test:
- 1 event;
- 10 events;
- 100 events;
- crisis mixture.

Report remains legible.

---

## 31A.35 Overflow strategy

If more events than display budget:
- aggregate;
- show `+N more`;
- full history remains in event log.

Do not silently truncate with no discoverability.

---

## 31A.36 Localization keys

Pattern:

```text
briefing.kind.<kind>
briefing.section.<section>
briefing.failure.owner
briefing.more
```

Placeholders:

```text
{primary}
{secondary}
{value}
{count}
{actor}
```

No new inline English in builder logic.

---

## 31A.37 Localization validation

Gate:
- every player-facing kind has localization key;
- placeholders match formatter;
- no missing fallback.

---

## 31A.38 Per-kind producer tests

For each semantic kind:
- set pre-state;
- tick owner;
- produce transition;
- assert exactly one event;
- assert fields.

---

## 31A.39 No-transition test

For every converted heartbeat owner where feasible:

```text
stable pre-state
→ TickDay
→ no semantic event
```

Diagnostics may still record owner timing separately.

---

## 31A.40 Snapshot at 1280×800

Render:
- quiet;
- normal;
- crisis.

Assertions:
- no clipping;
- no overflow;
- `+N more` works;
- severity text visible.

### 31A DoD

Every emitted semantic kind is owned and handled; heartbeat noise is removed; no unknown kind can silently disappear.

---

# TASK 31B — Make the Briefing Navigable and Historical

# 31B.0 Goal

Turn the daily briefing into a decision-support interface.

Every line must be:
- actionable via a live route;
- or explicitly informational.

The record must survive modal dismissal.

---

## 31B.1 Extend briefing entry model

Add fields where missing:

```text
kind
category
severity
primaryId
secondaryId
numeric
route
causeId
actorId
isActionable
```

Keep Core model engine-free.

---

## 31B.2 Kind-to-route mapping authority

Create one mapping:

```text
kind → preferred panel id
```

Do not scatter routes in UI click handlers.

Possible Core metadata table if panel IDs are stable shared identifiers.

---

## 31B.3 Avoid Core depending on Godot objects

Core may carry:
- route ID string.

Host validates route against `PanelRegistry`.

Do not make Core directly own Godot panel instances.

---

## 31B.4 Route contract

Every route target must:
- exist in registry;
- be player-navigable/live;
- pass Plan-16 liveness contract.

If target is shelved:
- route to parent live surface;
- or mark informational.

---

## 31B.5 Route examples

Potential mappings:
- survivor condition → survivor/medical surface;
- resource delta → inventory;
- shelter consequence → power/shelter infrastructure;
- weather condition → weather;
- radio transmission → radio;
- crafting production → crafting/workshop;
- duty vacancy → duty roster;
- memorial/death → memorial/survivor log.

Use actual current route IDs.

---

## 31B.6 Route fallback policy

If semantic event has no useful live panel:
- `route = null`;
- `isActionable = false`;
- display informational icon/text.

Do not invent a dead affordance.

---

## 31B.7 Reuse existing navigation seam

Use existing:

```text
close overlay
→ OpenPlayerPanel(route)
```

as proven by onboarding.

No second navigation controller.

---

## 31B.8 Click handling

On briefing entry activation:
1. validate route still live;
2. close/dismiss briefing;
3. open target panel;
4. optionally focus relevant subject/entity.

If route became unavailable:
- remain safe;
- show informational status;
- log dev warning.

---

## 31B.9 Subject focus

Where target panel supports it, pass:
- survivor ID;
- item ID;
- expedition ID;
- incident ID.

Use existing selection APIs.

Do not create route-specific ad hoc global state.

---

## 31B.10 Cause-route mapping

If `CauseId` represents a player choice:
- route to source decision surface where useful;
- otherwise consequence route.

Example:
- ration dispute may route to ration policy;
- duty injury may route to duty roster;
- quest consequence may route to quest/journal.

---

## 31B.11 Preserve events in event log

Write same semantic entries to `events_log`.

Required parity:
- kind;
- day;
- subject;
- cause;
- severity;
- summary key.

Do not render separate independently-derived text.

---

## 31B.12 Journal daily brief parity

Journal gets daily summary from same events.

Avoid:
- one vocabulary for modal;
- another for journal.

---

## 31B.13 Event history idempotence

Save/load must not duplicate same day events.

Use stable key such as:

```text
day + owner + kind + identity index
```

according to current persistence.

---

## 31B.14 Severity ordering

Recommended:

```text
1. death / critical failure
2. hazard warning
3. player decision
4. consequence
5. ambient state change
6. informational
```

Document tie-breakers.

---

## 31B.15 Group by subject

Where several events concern one survivor:

```text
Marta
- became severely fatigued
- missed duty
- admitted to ward
```

Grouping should not hide critical sequence.

---

## 31B.16 Grouping policy

Use grouping only when:
- same subject;
- same severity band;
- event semantics remain clear.

Do not merge unrelated resource/weather/world events.

---

## 31B.17 Delta-first presentation

Prefer:

```text
−2 canned meals — ration cut
```

over:

```text
canned_food: 4
```

Levels may appear as secondary context.

---

## 31B.18 Fallback branch migration

The headless/no-event-args path must construct semantic `DayEventKinds`.

Do not preserve a separate prose-only report model.

---

## 31B.19 Fallback semantics test

Same representative state fed through:
- primary event path;
- fallback path.

Expected:
- equivalent semantic kinds;
- equivalent important sections;
- deterministic wording keys.

---

## 31B.20 Quiet-day behavior

A genuinely quiet day should say:
- minimal explicit "No major changes" or current design,
not fabricate heartbeat lines.

---

## 31B.21 Crisis-day behavior

Crisis report must:
- prioritize critical;
- aggregate lower severity;
- retain route access;
- avoid scrolling trap if modal design limits height.

---

## 31B.22 Keyboard navigation

Requirements:
- Tab/arrow movement per current UI pattern;
- Enter/Space activates route;
- Escape closes;
- focus visible;
- no pointer-only line actions.

---

## 31B.23 Screen-reader/accessibility semantics

Each entry announces:
- severity;
- summary;
- actionability.

Do not rely on color.

---

## 31B.24 Severity visual semantics

Use:
- text label;
- icon;
- border/style;
- color as supplemental.

---

## 31B.25 Route contract test

Generate all player-facing kinds.

For each:
- mapped route exists/live;
- or explicitly informational.

No orphan click.

---

## 31B.26 Live-panel contract

Integrate with Plan-16 maturity metadata.

If panel becomes shelved later:
- route test fails;
- developer chooses parent/info behavior.

---

## 31B.27 Journal parity test

For day event set:
- briefing semantic entries;
- event log;
- journal daily brief.

Assert same core events represented.

---

## 31B.28 Ordering determinism test

Shuffle input event list if coordinator order is not itself contract.

Expected final report order remains stable by defined rules.

---

## 31B.29 Quiet snapshot

Content:
- 0–2 events;
- no failures.

Verify compact layout.

---

## 31B.30 Normal snapshot

Content:
- several decisions/consequences;
- one route per section.

Verify balanced density.

---

## 31B.31 Crisis snapshot

Content:
- death;
- hazard;
- owner failure;
- multiple survivors;
- resource loss;
- weather;
- `+N more`.

Verify no overflow at 1280×800.

### 31B DoD

Every briefing entry is either a working doorway into the live game or clearly informational, and the day's semantic record persists after the modal closes.

---

# TASK 31C — Diagnostics and Replay from the Same Event Stream

# 31C.0 Goal

Make every simulated day reproducible enough that a bug report containing seed/day can be investigated from a machine-readable record.

---

## 31C.1 Define day-record schema

Create:

`docs/telemetry/DAY_RECORD.md`

Recommended schema:

```json
{
  "schemaVersion": 1,
  "sessionId": "...",
  "seed": 12345,
  "day": 42,
  "ownerOrder": ["..."],
  "owners": [
    {
      "ownerId": "...",
      "durationMs": 0.42,
      "failed": false,
      "failureCode": null
    }
  ],
  "events": [
    {
      "kind": "resource_delta",
      "sourceOwnerId": "...",
      "primaryId": "...",
      "secondaryId": "...",
      "numeric": -2,
      "causeId": "...",
      "actorId": "..."
    }
  ],
  "headline": {
    "...": "..."
  }
}
```

Use actual project serialization conventions.

---

## 31C.2 Schema versioning

Add:
- integer schema version;
- backward-readable policy for tooling;
- tests on field set.

Do not silently change diagnostics format.

---

## 31C.3 Dev/debug-only writing

Enable via:
- debug build;
- developer setting;
- CLI flag.

Default release:
- off.

---

## 31C.4 Output path

Use project artifact/log convention.

Requirements:
- relative application/user data path;
- no source-repo absolute path leakage;
- `.gdignore`/gitignore where repo artifacts used.

---

## 31C.5 JSONL strategy

Preferred:
- one record per line;
- rolling `day-record.jsonl`;
or bounded chunk files.

Benefits:
- append-safe;
- easy grep;
- easy stream replay.

---

## 31C.6 Seed and session identity

Record:
- campaign seed;
- day;
- save/session generation ID if safe.

Do not include account/user identity.

---

## 31C.7 Owner order

Record actual execution order.

This is required because a failure may depend on:
- owner ordering;
- missing owner;
- timing.

---

## 31C.8 Per-owner timing

Instrument coordinator around each owner tick.

Use:
- monotonic timer;
- duration only.

Do not use wall-clock timestamp for determinism.

---

## 31C.9 Timing overhead check

Measure collector on/off.

Target:
- low overhead;
- no meaningful simulation behavior difference.

---

## 31C.10 Failure capture

Per owner:
- failed bool;
- safe failure code/message;
- diagnostic details in dev.

Avoid raw stack trace in player-facing briefing.

---

## 31C.11 Event capture

Record every semantic event after validation.

Fields match player-event identity.

---

## 31C.12 Headline state

Keep small.

Candidates:
- living survivors;
- inventory critical totals;
- power state;
- weather;
- active expeditions;
- major hazard count.

Do not dump whole save.

---

## 31C.13 Privacy/redaction policy

Never write:
- usernames;
- home paths;
- absolute project path;
- full save payload;
- authentication info;
- connected account info.

Normalize data-dir path to logical token if needed.

---

## 31C.14 Redaction tests

Inject:
- Unix home path;
- Windows user path;
- username-like string;
- save directory.

Assert record excludes them.

---

## 31C.15 Ring buffer

Maintain last N day records in memory.

Choose N by size budget, e.g. 7/14/30.

No unbounded growth.

---

## 31C.16 Crash dump integration

On fatal error:
- dump ring buffer;
- preserve event sequence;
- preserve owner failure.

Use existing error-routing conventions.

No bare catch.

---

## 31C.17 Rotation policy

Bound:
- max file size;
- max files;
- max days.

Delete oldest debug record safely.

---

## 31C.18 Rotation test

Generate enough records to exceed bound.

Assert:
- max size/count respected;
- newest retained;
- no active file corruption.

---

## 31C.19 Replay CLI verb

Add explicit command, e.g.:

```text
--replay-day-record <path>
```

or current host CLI convention.

---

## 31C.20 Replay contract

Replay should:
1. parse schema;
2. validate seed/day;
3. reconstruct required campaign state from a supported checkpoint/save;
4. advance/replay deterministic inputs;
5. compare semantic event digest.

A day record alone may not contain complete pre-day state.

Therefore record should reference:
- golden/save checkpoint ID;
- or replay tooling must use seed + previous save.

Do not pretend seed/day alone can reconstruct arbitrary mid-campaign player choices unless those decisions are also recorded.

---

## 31C.21 Decision-input recording

To support replay, record stable decision references needed for the day:
- ration choice;
- duty assignment;
- dispatch;
- quest decision;
where current replay tooling supports them.

Do not dump full player interaction log if unnecessary.

---

## 31C.22 Replay result

Output:

```text
expected event digest
actual event digest
matching events
missing events
unexpected events
owner failures
```

---

## 31C.23 Deterministic event digest

Normalize:
- order;
- floats;
- optional fields.

Digest input:
- kind;
- owner;
- ids;
- numeric;
- cause/actor.

Exclude timing.

---

## 31C.24 Day record ↔ briefing correlation

Developer can search by:
- day;
- kind;
- owner;
- primary ID.

The player line and diagnostic line must map.

---

## 31C.25 Support workflow

Document:

```text
1. ask for seed/day or day-record file
2. run replay verb
3. compare event digest
4. inspect owner timing/failure
5. reproduce with same build/commit
```

---

## 31C.26 Release-build guard

Automated test:
- normal release-like boot;
- no day record file appears.

---

## 31C.27 Debug-build guard

With flag:
- record appears;
- schema valid;
- one line per day.

---

## 31C.28 Schema stability test

Assert required fields exactly/compatibly.

Optional fields may expand under versioning.

---

## 31C.29 Replay round-trip test

Generate deterministic test day:
- write record;
- replay;
- digest equal.

---

## 31C.30 Long-campaign size test

Simulate record sizes for 360 days.

Ensure:
- rotation prevents uncontrolled disk growth.

---

## 31C.31 Export boot smoke

Run shipped/exported build.

Expected:
- day-record disabled;
- no debug files on first run.

### 31C DoD

A development bug report can be correlated from briefing line to day record and replayed through a deterministic CLI path without leaking private machine paths or generating unbounded logs.

---

# 5. Cross-Task Dependency Graph

```text
31A — DAY EVENT VOCABULARY
 │
 ├── canonical kinds
 ├── producer transitions
 ├── failure surfacing
 ├── aggregation
 └── localization
 │
 ▼
31B — NAVIGABLE BRIEFING
 │
 ├── live routes
 ├── cause navigation
 ├── history parity
 ├── accessibility
 └── snapshots
 │
 ▼
31C — DAY RECORD / REPLAY
   ├── same kinds
   ├── same owner ids
   ├── timings/failures
   ├── redaction
   └── deterministic replay
```

Wave dependencies:

```text
31A ─► Plan 30 world politics events
31A ─► Plan 32 travel/route events
31A ─► Plan 33 intelligence events
31A ─► Plan 34 milestone events
31A ─► Plan 25 localization extraction
31A kinds ─► Plan 17C audio trigger alignment
```

---

# 6. Event Semantics Matrix

Each production kind must define:

| Field | Meaning |
|---|---|
| `Kind` | canonical semantic event |
| `SourceOwnerId` | day owner producing transition |
| `PrimaryId` | principal subject/resource/entity |
| `SecondaryId` | cause/status/related entity |
| `Numeric` | signed delta / severity / amount |
| `CauseId` | optional player/system causal decision |
| `ActorId` | optional survivor/faction actor |
| `Severity` | briefing ordering |
| `Route` | preferred live player surface |
| `LocalizationKey` | rendered text |
| `AggregationKey` | collapse behavior |

Do not accept ambiguous multi-use field semantics per kind.

---

# 7. Old-to-New Vocabulary Migration Ledger

Required artifact:

```text
old_kind
status
new_kind
owner
transition predicate
diagnostic heartbeat?
removed?
```

Minimum old set from source:

- `duty_roster_ticked`
- `events_evaluated`
- `expeditions_ticked`
- `greenhouse_foundry_ticked`
- `holdfast_ticked`
- `journal_ticked`
- `maritime_ticked`
- `market_ticked`
- `medical_disease_ticked`
- `memorial_checked`
- `narrative_ticked`
- `needs_ticked`
- `phase0_ticked`
- `power_ticked`
- `shelter_decor_morale`
- `shelter_facilities_ticked`
- `survivor_social_ticked`
- `survivors_ticked`
- `world_evolution_ticked`
- `world_ticked`

Re-verify exact current list.

---

# 8. Unknown-Kind Failure Policy

Recommended:

## Development/test
- fail contract test;
- builder emits explicit developer warning if somehow reached.

## Release
- render generic localized informational line;
- record in diagnostic/error telemetry if enabled;
- never crash player because a new kind lacked prose.

This balances:
- no silent drop;
- resilient release behavior.

---

# 9. Event Deduplication Policy

Two owners may observe same underlying transition.

Rules:
- authoritative owner emits;
- observer does not re-emit same semantic event;
- if both viewpoints are meaningful, kinds differ.

Example:
- needs owner emits survivor critical hunger;
- ration system emits resource consumption;
Both may coexist because they represent different facts.

---

# 10. Attribution Policy

Classify cause:

```text
PLAYER_DECISION
SURVIVOR_ACTION
SYSTEM_CONSEQUENCE
AMBIENT_WORLD
OWNER_FAILURE
```

Briefing can style wording accordingly.

Do not anthropomorphize ambient changes as player-caused.

---

# 11. Event Severity Policy

Suggested:

## Critical
- death;
- catastrophic infrastructure;
- owner failure affecting simulation;
- major hazard.

## Warning
- severe survivor condition;
- low fuel;
- approaching failure.

## Decision
- direct player choice committed.

## Consequence
- downstream result of choice.

## Ambient
- world/weather change without direct player cause.

## Informational
- positive completion or low-impact update.

Severity is semantic, not arbitrary UI color.

---

# 12. Briefing Routing Policy

For each semantic kind specify:

```text
preferred_route
fallback_route
live_only
subject_focus_supported
informational_if_unavailable
```

Route validation happens against live registry.

---

# 13. History Persistence Policy

Persist enough semantic event data to reconstruct daily history.

Do not persist:
- rendered localized string only.

Persist:
- kind;
- IDs;
- numeric;
- cause;
- actor;
- day.

Render in current locale.

This preserves localization updates.

---

# 14. Diagnostics Persistence vs Save Persistence

Day-record diagnostics are not canonical save state.

Rules:
- save state remains authoritative;
- diagnostics are observational;
- deleting diagnostics must not change campaign;
- replay validates against save/checkpoint.

---

# 15. Performance Guardrails

Day event volume should be low.

Targets:
- transitions only;
- no per-frame events;
- no per-item heartbeat event spam;
- aggregation after owner ticks;
- diagnostics serializer bounded.

Measure:
- events/day median;
- p95;
- crisis maximum;
- briefing build time;
- day-record write size.

---

# 16. Determinism Contract

Same:

```text
seed
+ pre-day save state
+ player decisions
+ owner order
```

must yield same:

```text
semantic events
aggregation
briefing order
event digest
```

Owner timing is excluded.

---

# 17. Accessibility Acceptance

Briefing lines:
- focusable;
- keyboard actionable;
- severity text announced;
- route purpose discernible;
- no color-only information;
- focus returns safely after target panel closes if current navigation supports it.

---

# 18. Localization Acceptance

For every semantic kind:
- base language key;
- placeholders match schema;
- no inline English in Core builder;
- missing key gate fails.

Future Wave-4 producers add key with kind in same commit.

---

# 19. Failure Injection Matrix

## N31.1 Unknown event literal emitted
Expected: contract gate fails.

## N31.2 New DayEventKinds member never emitted
Expected: fails unless reserved.

## N31.3 Producer heartbeat emitted to briefing
Expected: semantic policy test flags old kind.

## N31.4 Builder receives unknown kind
Expected: explicit fallback/warning, not silence.

## N31.5 Owner fails with no events
Expected: briefing shows failure warning.

## N31.6 Event routes to shelved panel
Expected: route contract fails.

## N31.7 Cause route removed
Expected: fallback/info behavior, no dead click.

## N31.8 Crisis event volume > UI budget
Expected: deterministic aggregation / +N more.

## N31.9 Input event order shuffled
Expected: same final briefing order.

## N31.10 Save/load duplicates journal day event
Expected: parity/idempotence test fails.

## N31.11 Release build writes debug day record
Expected: guard test fails.

## N31.12 Day record leaks home path
Expected: redaction test fails.

## N31.13 Replay event digest differs
Expected: deterministic replay fails with diff.

---

# 20. Test Pyramid

## Tier 1 — Core contract
- DayEventKinds membership;
- builder mapping;
- aggregation;
- ordering;
- localization key mapping.

## Tier 2 — Owner tests
- transition produces event;
- no transition emits none;
- fields correct.

## Tier 3 — Host integration
- owner failure surface;
- route mapping;
- history parity;
- primary/fallback semantic equivalence.

## Tier 4 — UI
- quiet/normal/crisis snapshots;
- keyboard activation;
- accessibility.

## Tier 5 — Journey
- day1;
- real campaign multi-day stream;
- persistence.

## Tier 6 — Diagnostics
- record schema;
- replay;
- rotation;
- release guard.

---

# 21. CI Gates

Add/register:

```text
day_event_kind_contract
day_event_consumer_completeness
day_event_localization
briefing_route_contract
day_record_schema
```

Keep:
- fast: static/contract gates;
- Tier 2: real-campaign journey, replay.

---

# 22. Verification Commands

Run after each task and at close:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --day1-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/verify-fast.sh
```

Add current gate command for DayEventKinds contract.

Also run:
- quiet/normal/crisis snapshot suite;
- export boot smoke after 31C.

---

# 23. Recommended Commit Breakdown

```text
31A-1 baseline vocabulary matrix + failing silent-drop test
31A-2 DayEventKinds + kind metadata
31A-3 six producerless semantic kinds
31A-4 heartbeat migration part 1
31A-5 heartbeat migration part 2
31A-6 failure surface + unknown default
31A-7 aggregation/localization/contracts/snapshots

31B-1 briefing entry route model
31B-2 live kind→route mapping
31B-3 OpenPlayerPanel integration
31B-4 journal/events-log parity
31B-5 ordering/grouping/delta presentation
31B-6 accessibility + route contracts + snapshots

31C-1 day-record schema/writer
31C-2 owner timing/failure capture
31C-3 ring buffer/rotation/redaction
31C-4 replay CLI + digest
31C-5 schema/replay/release tests + export smoke
```

---

# 24. Risk Register

## R31.1 Briefing becomes noisy

Mitigation:
- transitions only;
- aggregation;
- max entries.

## R31.2 Semantic kind proliferation

Mitigation:
- canonical registry;
- prefer reusable semantic kinds;
- reserve only with owner.

## R31.3 Duplicate events

Mitigation:
- one authoritative producer per semantic fact;
- dedup tests.

## R31.4 Route recreates false-console problem

Mitigation:
- live-panel contract;
- informational fallback.

## R31.5 Localization work balloons

Mitigation:
- key pattern from day one;
- no inline strings;
- add keys with kinds.

## R31.6 Diagnostics leak privacy

Mitigation:
- dev-only;
- redaction;
- no save dump.

## R31.7 Replay overpromises seed/day reconstruction

Mitigation:
- require checkpoint/save + decisions when needed;
- document replay prerequisites honestly.

## R31.8 Timing instrumentation perturbs simulation

Mitigation:
- monotonic timing;
- observational capture;
- on/off determinism test.

---

# 25. Acceptance Checklist

## 31A — Vocabulary

- [ ] current event matrix generated
- [ ] current owner emission matrix generated
- [ ] silent-drop test captured
- [ ] owner-failure invisibility test captured
- [ ] DayEventKinds created
- [ ] semantic metadata documented
- [ ] initial vocabulary seeded from builder
- [ ] explicit kind registry available to tests
- [ ] reserved-kind policy added
- [ ] literal producer kinds removed
- [ ] survivor_condition producer wired
- [ ] resource_delta producer wired
- [ ] shelter_consequence producer wired
- [ ] weather_condition producer wired
- [ ] radio_transmission producer wired
- [ ] crafting_production producer reconciled
- [ ] all 20 dropped heartbeat kinds have migration rows
- [ ] heartbeat emissions removed from player stream
- [ ] meaningful power transitions emitted
- [ ] meaningful needs transitions emitted
- [ ] meaningful market transitions emitted where applicable
- [ ] memorial/world transitions emitted where applicable
- [ ] cause/actor fields reused or added deliberately
- [ ] cause semantics documented
- [ ] owner failures surface
- [ ] unknown-kind default prevents silence
- [ ] emitted-kind contract gate passes
- [ ] kind ownership gate passes
- [ ] consumer completeness gate passes
- [ ] contract gates proven able to fail
- [ ] aggregation model deterministic
- [ ] severity model documented
- [ ] volume budget enforced
- [ ] overflow visible
- [ ] localization keys added
- [ ] localization validation passes
- [ ] per-kind producer tests pass
- [ ] no-transition tests pass
- [ ] 1280×800 snapshot passes

## 31B — Briefing

- [ ] entry model includes route/actionability
- [ ] kind→route mapping centralized
- [ ] Core remains engine-free
- [ ] routes validate against live panels
- [ ] route fallbacks defined
- [ ] existing OpenPlayerPanel seam reused
- [ ] click/keyboard route works
- [ ] subject focus passed where supported
- [ ] cause route mapped where useful
- [ ] events_log parity
- [ ] journal daily brief parity
- [ ] event history idempotent
- [ ] severity ordering deterministic
- [ ] subject grouping implemented selectively
- [ ] delta-first presentation used
- [ ] fallback branch emits same semantic kinds
- [ ] fallback equivalence tests pass
- [ ] quiet day is explicit, not heartbeat spam
- [ ] crisis day remains legible
- [ ] keyboard navigation works
- [ ] accessibility semantics work
- [ ] no color-only severity
- [ ] route completeness contract passes
- [ ] no route to non-live panel
- [ ] journal parity test passes
- [ ] ordering determinism passes
- [ ] quiet snapshot accepted
- [ ] normal snapshot accepted
- [ ] crisis snapshot accepted

## 31C — Diagnostics

- [ ] DAY_RECORD schema documented
- [ ] schema version present
- [ ] writer dev/debug-only
- [ ] output path safe
- [ ] JSONL/chunk strategy bounded
- [ ] seed/session identity safe
- [ ] owner order recorded
- [ ] per-owner timing recorded
- [ ] instrumentation overhead checked
- [ ] owner failures recorded
- [ ] events recorded with same identity
- [ ] headline state minimal
- [ ] redaction policy implemented
- [ ] redaction tests pass
- [ ] ring buffer bounded
- [ ] crash dump integration uses existing error routing
- [ ] rotation bounded
- [ ] rotation tests pass
- [ ] replay CLI added
- [ ] replay prerequisites documented honestly
- [ ] needed player decisions represented
- [ ] replay diff output useful
- [ ] deterministic event digest implemented
- [ ] briefing↔record correlation works
- [ ] support workflow documented
- [ ] release-build guard passes
- [ ] debug-build writer test passes
- [ ] schema stability test passes
- [ ] replay round-trip passes
- [ ] 360-day size bound passes
- [ ] export boot emits no day record by default

---

# 26. Ship / No-Ship Gate

**SHIP** only if:

```text
canonical_day_event_vocabularies == 1
AND production_kind_literals_outside_registry == 0
AND emitted_unhandled_kinds == 0
AND handled_unproduced_unreserved_kinds == 0
AND player_heartbeat_kinds == 0
AND owner_failures_surfaced == true
AND unknown_kind_silent_drop == false
AND briefing_order_deterministic == true
AND briefing_overflow_failures == 0
AND briefing_localization_complete == true
AND routes_to_non_live_panels == 0
AND actionable_briefing_entries_work == true
AND journal_event_history_parity == true
AND keyboard_briefing_navigation == pass
AND day_record_dev_only == true
AND day_record_redaction == pass
AND day_record_rotation == pass
AND replay_roundtrip == pass
AND release_boot_writes_day_record == false
AND day1_selftest == pass
AND real_campaign_journey == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 27. Implementer Handoff

1. Do not build another event bus.
2. Reverify the exact kind list before migration.
3. Define `DayEventKinds` before Wave-4 producers add more strings.
4. Convert heartbeats into transitions; silence is valid when nothing changed.
5. Give every semantic event defined field meanings.
6. Add cause/actor only where attribution really exists.
7. Surface owner failures—never let failure look like a calm day.
8. Make unknown kinds loud in development and non-silent in release.
9. Aggregate deterministically and keep report volume bounded.
10. Localize from the kind registry, not inline prose.
11. Route only to live panels.
12. Reuse the existing panel-navigation seam.
13. Persist semantic history, not just rendered strings.
14. Build diagnostics from the same event identities.
15. Keep debug telemetry bounded and private.
16. Do not claim seed/day replay can reconstruct player choices unless those choices or a checkpoint are available.
17. Close with day-one and real-campaign journeys plus export smoke.

---

# 28. Final Outcome

When this plan is complete, the day-event layer stops behaving like a quiet data sink and becomes a semantic spine for ASHFALL.

The nineteen campaign-day owners continue using the typed channel they already share, but they stop reporting meaningless heartbeats. They emit transitions, deltas, warnings, decisions, and consequences through one canonical `DayEventKinds` vocabulary. Every emitted kind has a consumer, every handled kind has an owner or explicit reservation, and an unknown kind can never disappear silently again.

The daily briefing becomes useful. A death, ration cut, shelter failure, weather transition, radio transmission, production result, or survivor crisis is sorted by severity, attributed to its cause, grouped when appropriate, localized, and routed to the live screen where the player can respond. Closing the modal does not erase the day; the same semantic record remains in the journal and event history.

For developers, the same event identities feed a bounded, redacted day record with seed, owner order, timings, events, and failures. A support report can move from player-facing line to diagnostic record to deterministic replay without inventing a second telemetry language.

The result is not a new briefing system. It is the existing campaign event channel finally speaking the same language from simulation, to player, to diagnostics.
