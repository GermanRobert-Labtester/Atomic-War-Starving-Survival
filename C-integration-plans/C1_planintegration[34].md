# C1 — Flagship Integration Plan [34]: Individual Survivor Daily Routines, Personal Rhythms & Schedule Coordination

> **Output:** `C1_planintegration[34].md`
>
> **Source baseline:** Plan 188 — Individual Survivor Daily Routines
>
> **Primary mission:** give each survivor a persistent, legible daily rhythm—sleep, work, meals, social/personal time, hygiene and leisure preferences—while preserving the existing authorities for shelter phases, duty assignment, needs, food consumption, sleep/fatigue, relationships, workspace capacity, survivor autonomy, mental health, caregiving, education, and campaign time.
>
> **Primary architectural rule:** personal routines own **preference, intended time windows, routine template identity, player overrides, and schedule-resolution intent**. They do not own actual duty completion, hunger/fatigue state, food consumption, relationship scores, health, workspace occupancy, shelter curfew/emergency phases, or final survivor availability when those facts already belong to canonical systems.
>
> **Primary behavioral rule:** a routine is a **soft plan**, not a deterministic animation script. The world may interrupt it through emergencies, illness, expeditions, duty changes, caregiving, combat, shortages, shelter restrictions, or survivor autonomy. The routine system resolves what the survivor intends to do next and reports deviations; domain systems decide what actually happens.
>
> **Primary correction to the source plan:** the source proposes a second time-block execution system, daily satisfaction state, conflict objects for situations that are not actually conflicts, and direct needs/morale/relationship consequences. The flagship instead introduces a bounded `SurvivorRoutineCoordinator` that derives schedule intents, feeds the canonical duty/needs/relations systems, and emits only meaningful deviations or compatibility issues.
>
> **Mandatory execution order:** 188A schedule/duty/needs/autonomy authority audit → 188B routine preference/template contract → 188C deterministic schedule-intent resolver → 188D sleep/meal/work/personal adapters → 188E compatibility/conflict semantics → 188F satisfaction/quality as derived read models → 188G UI, persistence, migration, performance, determinism and CI → 188H advanced adaptive routines/events only after the base layer proves low-micromanagement value.
>
> **Critical re-baseline rule:** before creating `SurvivorRoutineSystem.cs`, inspect `ShelterScheduleSystem`, `DutyRosterSystem`, `NeedsSystem`, survivor lifecycle/availability, Plan-137 performance, survivor autonomy/refusal, food/rationing, sleeping/bed assignment, `SurvivorRelationsSystem`, hygiene, exercise/health systems, Plan-154 education schedules, Plan-179 mental-health crisis integration, caregiving, expeditions, emergency/curfew phases, workspace capacity, and save orchestration.
>
> **Guardrails:** no second shelter clock; no second duty roster; no routine-owned hunger/fatigue/morale/relationship state; no routine-owned food consumption; no routine-owned workspace reservation if a canonical workspace/room system exists; no “workspace overlap = conflict” unless capacity is actually exceeded; no “different meal times = conflict” unless shared-resource constraints make it one; no “introvert + extrovert = social clash” by default; no automatic social relationship decay because a scheduled block was missed; no hard-coded 6–8h sleep medical truth without using the actual Needs/sleep model; no universal three-meal assumption unless the food system supports it; no strict mode that overrides emergencies or incapacitation; no daily satisfaction journal spam; no per-frame scanning of all time blocks; no unseeded RNG; no wall-clock time; no custom routine becoming a second gameplay scripting language.

---

# 0. Mission

ASHFALL already has shelter-level scheduling and work assignment, but survivors lack an individual daily rhythm.

The source baseline identifies:

- `ShelterScheduleSystem.cs` with Day/Night/Curfew/Emergency phases and bed assignments;
- `DutyRosterSystem` for work assignment;
- `NeedsSystem` for hunger, fatigue, hygiene and related survival pressures;
- no personal wake/sleep times;
- no meal windows;
- no personal/social blocks;
- no chronotype or work-time preference;
- no per-survivor routine intent.

Current shape:

```text
SHELTER PHASE
    │
    ├── Day
    ├── Night
    ├── Curfew
    └── Emergency
          │
          ▼
DutyRosterSystem
          │
          ▼
ON DUTY / OFF DUTY
```

Target shape:

```text
CAMPAIGN CLOCK
    │
    ▼
ShelterScheduleSystem
    │
    ├── allowed phase
    ├── curfew
    ├── emergency override
    └── sleep-space policy
    │
    ▼
SurvivorRoutineCoordinator
    │
    ├── chronotype
    ├── preferred wake/sleep window
    ├── preferred work windows
    ├── preferred meal windows
    ├── preferred social/personal windows
    ├── routine template
    └── player/survivor override
    │
    ▼
RESOLVED ACTIVITY INTENT
    │
    ├────────► DutyRosterSystem
    ├────────► Needs / sleep
    ├────────► food/rationing
    ├────────► hygiene
    ├────────► social/relations
    ├────────► education/training
    ├────────► caregiving
    ├────────► leisure/recovery
    └────────► workspace/room access
           │
           ▼
ACTUAL DOMAIN OUTCOME
```

The routine system should answer:

> Given this survivor's preferences, assignments, shelter rules, current condition and interruptions, what activity window should they try to follow?

It should not answer:

> Did work produce resources?
> Did the survivor become hungry?
> Did they actually eat?
> Did the relationship score increase?
> Is the bed occupied?
> Did a crisis occur?
> Is the survivor medically fit for duty?

Those remain domain authorities.

---

# 1. Source-Evidence Interpretation

## 1.1 Personal routines are genuinely absent

The source reports zero Core matches for:

```text
DailyRoutine
PersonalSchedule
IndividualSchedule
WakeTime
SleepTime
ActivitySchedule
TimeBlock
SurvivorRoutine
```

A personal-routine layer is therefore a legitimate new feature.

## 1.2 Shelter phases already own macro time constraints

`ShelterScheduleSystem` must remain authority for:

- Day/Night;
- Curfew;
- Emergency;
- bed assignments or shelter sleep constraints.

Personal routines fit inside those phases.

## 1.3 DutyRoster already owns work

A routine may say:

```text
preferred work window = 06:00–12:00
```

but `DutyRosterSystem` decides:
- actual assignment;
- location;
- eligibility;
- workload.

## 1.4 Needs already own sleep/hunger/hygiene pressure

Routine timing can affect opportunities to:
- sleep;
- eat;
- wash.

It must not directly set:
- fatigue;
- hunger;
- hygiene.

## 1.5 “Routine satisfaction” risks becoming a duplicate morale system

The source proposes:
- sleep satisfaction;
- meal satisfaction;
- work satisfaction;
- social satisfaction;
- overall satisfaction.

This can be useful as an **explainable daily quality/read model**.

It should not become:
- a second persistent emotional stat;
- a second needs system.

## 1.6 Several proposed “conflicts” are not actually conflicts

Examples from the source:

### Workspace overlap
“Two survivors assigned to same workspace at different times.”

That is not a conflict.

Actual conflict:
- overlapping same resource beyond capacity.

### Meal-time conflict
Different desired meal times are not inherently conflict.

It matters only if:
- kitchen/ration service is time-bounded;
- shared food preparation capacity exists.

### Introvert/extrovert social clash
Personality mismatch alone should not create automatic conflict.

Use:
- relationship/autonomy/preference compatibility if a real interaction happens.

## 1.7 Routine enforcement must respect autonomy

Strict/Flexible/None can be useful **management policy**.

But strict mode cannot force:
- sick;
- panicked;
- refusing;
- absent
survivors through invalid actions.

---

# 2. Non-Negotiable Routine Invariants

## INV-188.1 — One campaign clock

Routine uses canonical campaign time.

## INV-188.2 — ShelterSchedule owns shelter phases

Routine cannot redefine curfew/emergency.

## INV-188.3 — DutyRoster owns work assignment

## INV-188.4 — Needs owns hunger/fatigue/hygiene state

## INV-188.5 — Food/rationing owns actual meal consumption

## INV-188.6 — Relations owns social relationship state

## INV-188.7 — Workspace/room authority owns capacity

## INV-188.8 — Routine is intent, not guaranteed execution

## INV-188.9 — Routine preferences are not immutable personality facts unless authored

## INV-188.10 — Chronotype is a preference, not a medical diagnosis

## INV-188.11 — No universal three-meal model unless food system uses it

## INV-188.12 — No universal 6–8h required-sleep hardcode if NeedsSystem defines different mechanics

## INV-188.13 — Satisfaction is derived and explainable

## INV-188.14 — Satisfaction does not duplicate morale

## INV-188.15 — Deviations have provenance

Why was routine missed:
- emergency;
- duty;
- illness;
- no food;
- no bed;
- player override;
- survivor choice.

## INV-188.16 — Emergencies override routines through canonical shelter rules

## INV-188.17 — Survivor autonomy can override player schedule where current autonomy rules allow

## INV-188.18 — Schedule resolution is deterministic

## INV-188.19 — No per-frame N×blocks scanning

## INV-188.20 — Old saves preserve current behavior safely

---

# 3. Definition of Done

Plan 188 closes only when:

- `ShelterScheduleSystem` authority is documented;
- `DutyRosterSystem` authority is documented;
- needs/sleep/meal/hygiene ownership is documented;
- autonomy/refusal ownership is documented;
- workspace/room capacity ownership is documented;
- routine intent is separated from actual execution;
- one routine preference/template authority exists;
- time blocks are validated and non-overlapping within a routine;
- templates are versioned/data-driven;
- early-riser/night-owl/standard are defaults rather than forced personality;
- Custom uses the same schema;
- work blocks align with canonical duty assignments;
- sleep windows route to canonical sleep/fatigue handling;
- meal windows route to actual ration/food availability;
- social windows create interaction opportunities rather than direct relation gain;
- hygiene/exercise/leisure ship only if actual activity/needs systems can consume them;
- emergency/curfew overrides are deterministic;
- routine deviations are reason-coded;
- “conflicts” only exist when a real shared resource, sleep disturbance, or social interaction incompatibility occurs;
- no fake conflict from merely different preferences;
- routine quality/satisfaction is derived from actual outcomes;
- no persistent duplicate morale state exists;
- routine consequences reach needs/relations/mental-health only through typed adapters;
- routine execution is event/time-boundary driven rather than per frame;
- old saves default to current-behavior-compatible routine mode;
- old saves are not immediately punished for missing routines;
- save/load preserves intended routine and current schedule position;
- routine edits do not duplicate or cancel canonical duty work incorrectly;
- `--survivor-routine-selftest` exists or equivalent;
- 30/120/180-day simulations show personality value without excessive micromanagement;
- UI allows fast template assignment plus advanced editing;
- routine warnings are bounded and actionable;
- headless execution passes.

---

# 4. Phase P0 — Scheduling & Activity Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
campaign clock APIs
ShelterScheduleSystem phases
ShelterScheduleSystem bed assignment
curfew/emergency rules
DutyRosterSystem assignment APIs
duty schedule granularity
NeedsSystem hunger/fatigue/hygiene/morale
sleep/recovery APIs
food/ration consumption APIs
SurvivorLifecycle availability
survivor autonomy/refusal APIs
workspace/room capacity APIs
SurvivorRelationsSystem
education/training schedules
CaregivingSystem
expedition/away state
mental-health crisis APIs
save ordering
UI roster/schedule surfaces
```

## P0.2 Build routine authority matrix

Create:

`docs/survivors/ROUTINE_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
read API
write API
persisted?
routine role
status
```

Rows:
- campaign hour;
- shelter phase;
- curfew;
- emergency;
- bed assignment;
- duty assignment;
- work location;
- actual work;
- sleep opportunity;
- fatigue;
- hunger;
- meal consumed;
- hygiene;
- social interaction;
- relationship;
- education;
- caregiving;
- survivor away;
- incapacitation;
- autonomy/refusal;
- personal preference;
- routine intent;
- routine deviation;
- daily quality/read model.

## P0.3 Audit time resolution

Determine actual simulation resolution:

```text
minute
hour
phase
day
event-driven
```

Do not build hour-by-hour routine semantics if engine advances only by coarse phases without a stable hourly clock.

## P0.4 Schedule resolution ADR

Create:

`docs/architecture/ADR_PERSONAL_ROUTINES_VS_SHELTER_SCHEDULE.md`

Define:
- macro vs micro authority;
- override precedence;
- time resolution;
- emergency behavior.

## P0.5 Routine satisfaction ADR

Create:

`ADR_ROUTINE_QUALITY_VS_MORALE.md`

Define:
- derived quality;
- downstream morale adapter;
- no second emotional state.

## P0.6 Conflict semantics ADR

Create:

`ADR_ROUTINE_CONFLICT_SEMANTICS.md`

Explicitly reject:
- different desired meal times as conflict by default;
- differing chronotypes as conflict by default;
- non-overlapping workspace use as conflict.

## P0.7 Baseline proof

Demonstrate:
- survivor can be assigned duty;
- off-duty time has no personal structure;
- no individual wake/sleep/meal intent exists.

---

# TASK 188A — Routine Preference & Template Contract

# 188A.0 Goal

Represent personal rhythm as a small set of preferences and intended windows.

## 188A.1 Proposed file

If no existing owner:

`Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs`

Better naming if purely orchestration:

`SurvivorRoutineCoordinator.cs`

## 188A.2 Routine state

Recommended:

```text
schema_version
survivor_id
template_id
custom_windows[]
preference_profile
enforcement_policy
last_edit_day
```

Do not store:
- daily hunger;
- sleep satisfaction;
- morale;
- relationship score.

## 188A.3 Time representation

Use canonical campaign time primitive.

Avoid raw integer hour if the game uses:
- phase;
- ticks;
- minutes.

## 188A.4 Time window DTO

Suggested:

```text
window_id
activity_kind
start
end
flexibility
priority
location_constraint optional
```

## 188A.5 Activity kinds

Ship only with real consumers:

```text
sleep
work
meal
social
personal
hygiene
exercise
leisure
education
caregiving
```

## 188A.6 Personal umbrella

If hygiene/exercise/leisure lack distinct systems:
- represent as `personal`
instead of fake sub-mechanics.

## 188A.7 Routine template catalog

`Assets/StreamingAssets/Data/routine_templates.json`

Versioned.

## 188A.8 Template DTO

```text
id
display_key
description_key
chronotype_hint
windows[]
flexibility_profile
tags[]
```

## 188A.9 Source templates

Candidate defaults:

```text
early_riser
night_owl
standard
custom
```

## 188A.10 Custom is not a static catalog template

It is:
- player-edited resolved schedule
with optional base-template provenance.

## 188A.11 Early-riser timing

Source:
- wake 05–06;
- sleep 21–22.

Treat as tuning candidate.

## 188A.12 Night-owl timing

Source:
- wake 09–10;
- sleep 01–02.

Treat as tuning candidate.

## 188A.13 Standard timing

Source:
- wake 07–08;
- sleep 22–23.

Treat as tuning candidate.

## 188A.14 Chronotype

Suggested:

```text
early
intermediate
late
```

Neutral terminology.

## 188A.15 Meal preference

Audit actual meal mechanics.

If game has no meal-count concept:
- use preferred eating windows/frequency rather than:
  - breakfast;
  - lunch;
  - dinner.

## 188A.16 Work preference

Possible:
- morning;
- day;
- evening;
- night.

Duty availability remains canonical.

## 188A.17 Social preference

Avoid storing simplistic:
- introvert/extrovert
as schedule demand unless trait/personality system already does.

Prefer:
- preferred social duration/frequency;
- social-intensity comfort.

## 188A.18 Exercise preference

Only if exercise activity exists.

## 188A.19 Preference source

Could be:
- authored survivor trait;
- deterministic generation;
- player-set preference.

Document authority.

## 188A.20 Player override

May modify routine.

Does not necessarily modify intrinsic preference.

Separate:
- preference;
- assigned routine.

## 188A.21 Preference conflict

Assigned routine can mismatch preference.

That is a quality input.

## 188A.22 Validation

Check:
- no invalid overlap;
- start/end valid;
- enough potential sleep opportunity if template claims so;
- work block feasible;
- no duplicate IDs.

## 188A.23 Overnight windows

Explicit support:
- 22:00–06:00.

## 188A.24 Day wrap

Deterministic.

## 188A.25 Window priority

Emergency > critical care > mandatory duty > routine preferences,
subject to autonomy/fitness.

Do not hardcode without authority matrix.

## 188A.26 Generated docs

Create:

`ROUTINE_TEMPLATE_MATRIX.md`

### 188A DoD

Each survivor can have a data-driven preferred daily rhythm without duplicating actual needs, work, meals, relationships, or shelter-phase state.

---

# TASK 188B — Schedule Intent Resolution

# 188B.0 Goal

Convert routine windows + world state into one current intended activity.

## 188B.1 Resolver

Suggested:

```text
RoutineIntentResolver
```

Pure where possible.

## 188B.2 Inputs

```text
campaign time
shelter phase
emergency state
survivor availability
duty assignment
routine
needs urgency
caregiving obligations
education commitments
expedition/away state
autonomy/refusal state
resource/space availability
```

## 188B.3 Output

```text
RoutineIntent
  survivor_id
  activity_kind
  source_window_id
  start
  expected_end
  priority
  location_id optional
  duty_id optional
  override_reason optional
```

## 188B.4 Intent does not execute activity

## 188B.5 Resolution precedence

Example:

```text
dead/absent/incapacitated
→ emergency
→ critical medical/care obligation
→ mandatory duty
→ scheduled routine
→ needs-driven fallback
→ idle/personal
```

Finalize with existing systems.

## 188B.6 Need urgency

A severely hungry survivor may eat outside preferred meal window if:
- food available;
- autonomy/needs behavior supports it.

Routine should not cause starvation for schedule purity.

## 188B.7 Fatigue urgency

Same for sleep.

## 188B.8 Emergency

ShelterScheduleSystem can override:
- curfew;
- shelter-wide emergency assignment.

## 188B.9 Duty change

If DutyRoster changes assignment:
- current/future work intent updates.

## 188B.10 Expeditions

Away survivor:
- shelter routine suspended.

Expedition owns travel-day activities.

## 188B.11 Illness

Medical/fitness authority may replace work intent with:
- rest;
- treatment;
- care.

## 188B.12 Refusal

Autonomy can reject strict schedule where existing system supports.

## 188B.13 Flexible window

Can slide within:
- min/max tolerance.

## 188B.14 Rigid window

Means strong preference/requirement.

It still cannot violate higher-priority canonical constraints.

## 188B.15 Fallback

If activity cannot execute:
- choose compatible fallback;
- record deviation reason.

## 188B.16 No recursive resolution loop

Bound fallback chain.

## 188B.17 Stable tie-break

Canonical priority + stable IDs.

## 188B.18 Seeded RNG

Only if two equally valid personal choices intentionally need variation.

Deterministic default preferred.

## 188B.19 No per-frame full recompute

Recompute on:
- time-window boundary;
- phase change;
- duty change;
- urgent need threshold;
- emergency;
- survivor state event.

## 188B.20 Next transition index

Track:
- next relevant boundary per survivor.

## 188B.21 Headless

Pure.

## 188B.22 Diagnostics

Expose:
- chosen intent;
- rejected alternatives;
- reason.

### 188B DoD

At any meaningful simulation boundary, the game can deterministically explain what a survivor intends to do next and why, without the routine layer performing the action itself.

---

# TASK 188C — Sleep, Meal, Work & Personal Activity Adapters

# 188C.0 Goal

Make routine intents produce real gameplay only through canonical systems.

---

# 188C-SLEEP

## 188C.S1 Sleep authority audit

Find:
- sleep action;
- fatigue recovery;
- bed assignment;
- interruptions.

## 188C.S2 Routine supplies sleep window

## 188C.S3 Bed assignment remains ShelterSchedule/room authority

## 188C.S4 Sleep opportunity

If bed available:
- request sleep.

## 188C.S5 No bed

Deviation:
- no_bed;
- alternate sleep location if system supports.

## 188C.S6 Fatigue

NeedsSystem owns fatigue.

## 188C.S7 Sleep duration

Actual time slept, not scheduled time, feeds NeedsSystem.

## 188C.S8 Preferred duration

May inform routine quality.

Do not use hard-coded 6–8h if canonical sleep system defines needs.

## 188C.S9 Sleep disturbance

Requires real cause:
- roommate noise;
- alarm;
- emergency;
- environment;
- overcrowding.

## 188C.S10 Chronotype

May alter preferred timing, not raw sleep recovery unless sleep system supports circadian modifier.

## 188C.S11 Night shift

Works if:
- shelter phase and duty permit.

---

# 188C-MEAL

## 188C.M1 Food authority audit

## 188C.M2 Meal window

Represents preferred eating opportunity.

## 188C.M3 Actual consumption

Food/ration authority.

## 188C.M4 No food

Deviation:
- unavailable_food.

NeedsSystem hunger evolves normally.

## 188C.M5 Rations

Governance/ration policy applies.

Routine cannot bypass rationing.

## 188C.M6 Frequency

Support one/multiple meal windows according to actual food model.

## 188C.M7 No breakfast/lunch/dinner hard dependency

Labels can be presentation only.

## 188C.M8 Social meal

If co-presence interaction exists:
- relations may consume actual shared-meal event.

---

# 188C-WORK

## 188C.W1 DutyRoster owns task

## 188C.W2 Routine window is preference

## 188C.W3 Work intent only if assignment exists

## 188C.W4 No routine-generated job

## 188C.W5 Work capacity/fitness

Plan 24/137/143 remain authoritative.

## 188C.W6 Scheduled work but unfit

Deviation:
- medically_unfit;
- exhausted;
- unavailable.

## 188C.W7 Work output

Duty/work system computes.

## 188C.W8 Chronotype/work preference

May contribute to satisfaction/efficiency only through a bounded explicit adapter if balance proves useful.

Do not double Plan 137 automatically.

## 188C.W9 Shift handoff

If workspaces support shifts:
- use capacity reservation.

## 188C.W10 Overtime

Actual extra work can create:
- fatigue;
- routine-quality impact.

---

# 188C-SOCIAL

## 188C.SO1 Social block is opportunity

Not guaranteed relation gain.

## 188C.SO2 Interaction requires participants

## 188C.SO3 Co-presence

Use room/location availability if modeled.

## 188C.SO4 Compatibility

SurvivorRelations/personality decides outcome.

## 188C.SO5 No introvert/extrovert automatic clash

## 188C.SO6 Missed social block

Does not directly decay relationship.

May reduce routine-quality if survivor values social time.

---

# 188C-PERSONAL

## 188C.P1 Personal time is fallback umbrella

## 188C.P2 Hygiene

Use NeedsSystem/hygiene authority if actionable.

## 188C.P3 Exercise

Only if real action/system exists.

## 188C.P4 Leisure

Only if real recreation/morale action exists.

## 188C.P5 Reading/study

Can route to education/memory-review.

## 188C.P6 Rest

Can route to fatigue recovery if supported.

## 188C.P7 No fake stat bonus for personal block by itself

### 188C DoD

Routine time windows generate requests/opportunities into real sleep, food, duty, social and personal-activity systems; no survival or social outcome is calculated twice.

---

# TASK 188D — Routine Quality, Preference Fit & Satisfaction Projection

# 188D.0 Goal

Measure whether a survivor's day matched their needs/preferences without creating a second morale meter.

## 188D.1 Prefer “routine quality” read model

Instead of persistent `RoutineSatisfaction`.

Example:

```text
RoutineDayQuality
  survivor_id
  day
  sleep_fit
  meal_fit
  work_fit
  social_fit
  personal_time_fit
  disruption_load
  overall_band
  reason_codes[]
```

## 188D.2 Derived from actual outcomes

Not scheduled intention.

## 188D.3 Sleep fit

Inputs:
- actual sleep duration;
- timing;
- interruptions;
- bed/environment.

## 188D.4 Meal fit

Inputs:
- actual eating;
- timing relative to preference;
- food quality if canonical;
- shared meal if relevant.

## 188D.5 Work fit

Inputs:
- actual workload;
- preferred window;
- overtime;
- task preference only if real trait/job preference exists.

## 188D.6 Social fit

Inputs:
- actual interaction duration/quality;
- preference.

## 188D.7 Personal-time fit

Only if personal-time systems exist.

## 188D.8 Overall

Weighted by survivor preference profile.

## 188D.9 Do not persist every category forever by default

Keep:
- current/recent rolling read model;
- meaningful trend aggregate.

## 188D.10 Morale consequence

If morale system supports:
- one bounded routine-quality contribution.

## 188D.11 No double counting

Example:
- missed sleep already increases fatigue.
Do not also apply huge morale/work penalties from routine quality.

Routine-quality morale effect must be modest.

## 188D.12 High quality

Do not automatically grant productivity bonus.

If a bonus exists:
- route through Plan-137 modifier composition;
- prove it is not duplicate of adequate sleep/needs.

Recommended default:
- no direct productivity bonus.

## 188D.13 Low quality

Likewise:
- fatigue/hunger already have performance consequences.

Use:
- mood/stress/social complaint
only if needed.

## 188D.14 Daily score

UI may show:
- good;
- strained;
- disrupted.

Avoid exact 0–100 unless meaningful.

## 188D.15 Trend

7-day rolling trend.

## 188D.16 Satisfaction vs preference

A survivor can have:
- healthy day;
- poor preference fit.

Keep distinction.

## 188D.17 Severe deprivation

Comes from Needs/medical systems.

Routine quality can explain:
- schedule contributed.

## 188D.18 Mental-health integration

Plan 179 may consume:
- chronic routine disruption
as one stressor.

Not single-day low score.

## 188D.19 Threshold

Require sustained disruption.

## 188D.20 Diagnostics

Explain:
- “Night shift repeatedly conflicts with preferred sleep window.”

### 188D DoD

Routine quality is an explainable projection of actual daily experience, useful for UI and bounded stress/morale input without duplicating survival or emotional state.

---

# TASK 188E — Real Conflict & Compatibility Detection

# 188E.0 Goal

Detect only conflicts where two schedules compete for a real shared resource or create a real disturbance.

## 188E.1 Conflict is not preference mismatch alone

## 188E.2 Conflict sources

Valid categories may include:

```text
workspace_capacity
bedroom_sleep_disturbance
shared_facility_capacity
mandatory_schedule_collision
caregiving_collision
education_duty_collision
```

## 188E.3 Workspace conflict

Only if:

```text
same resource
+ overlapping time
+ occupancy > capacity
```

Source's “same workspace at different times” is explicitly not a conflict.

## 188E.4 Meal-time conflict

Only if:
- kitchen/mess capacity;
- ration service window;
- communal event
creates actual collision.

Different meal preferences alone are harmless.

## 188E.5 Sleep disturbance

Requires:
- shared room/bedroom;
- incompatible activity/noise;
- actual overlap.

## 188E.6 Social clash

Do not create from introvert/extrovert labels.

Use actual interaction + relation/personality systems.

## 188E.7 Mandatory schedule collision

Example:
- duty and medical treatment at same time.

This is a schedule conflict.

## 188E.8 Education collision

Class vs duty.

## 188E.9 Caregiving collision

Care duty vs work/sleep.

## 188E.10 Conflict DTO

Suggested:

```text
conflict_id
day
resource_or_commitment_id
survivor_ids[]
time_window
conflict_kind
severity
resolution_state
source_refs[]
```

## 188E.11 Stable ID

Deterministic from:
- resource;
- survivors;
- window;
- day.

## 188E.12 Conflict detection

Event/boundary-driven.

## 188E.13 Resolution options

Possible:
- move work window;
- reassign duty;
- change room;
- change meal window;
- replace caregiver;
- reschedule class.

Actions are delegated to owning systems.

## 188E.14 Routine system cannot directly reassign duty unless it uses DutyRoster API

## 188E.15 Survivor compromise

Can generate proposed routine edit.

Autonomy/relationship systems decide acceptance if applicable.

## 188E.16 “Unresolvable”

Use only when no valid alternative under current constraints.

## 188E.17 Conflict severity

Based on:
- missed critical need;
- duration;
- repeated disruption;
- resource criticality.

Not arbitrary label.

## 188E.18 Conflict closure

When constraints no longer overlap.

## 188E.19 No daily duplicate conflict

Stable identity.

## 188E.20 Conflict history

Keep only meaningful/recent unless archive event-worthy.

### 188E DoD

Routine conflicts correspond to real scheduling/resource collisions and can be resolved through canonical duty, room, care, education, or routine APIs rather than invented social friction.

---

# TASK 188F — Routine Enforcement, Autonomy & Governance

# 188F.0 Goal

Provide player control without turning schedules into coercive hard scripts.

## 188F.1 Source modes

Candidate:

```text
none
flexible
strict
```

## 188F.2 None

Preserves current broad behavior:
- no personal routine enforcement;
- needs/duty/shelter phases operate normally.

## 188F.3 Flexible

Routine is preferred.

Needs/autonomy/world events may deviate.

## 188F.4 Strict

Higher priority for assigned routine windows.

Still cannot override:
- medical incapacity;
- emergency;
- absence;
- impossible resource;
- hard fitness constraints.

## 188F.5 Strict consequence

Do not add generic “penalty for deviation.”

Deviation consequence already emerges from:
- unmet needs;
- missed work;
- dissatisfaction
where real.

## 188F.6 Survivor preference mismatch

Strict schedule can create:
- routine-quality stress
over time.

## 188F.7 Autonomy

Plan 144 can produce:
- refusal;
- self-adjustment.

## 188F.8 Player override

Can change routine.

## 188F.9 Survivor-requested adjustment

If autonomy/dialogue system supports:
- “I need later sleep.”
- “Move me off nights.”

## 188F.10 Governance

Plan 159 may configure:
- global work/sleep/curfew policies.

Governance does not own individual schedule.

## 188F.11 Emergency policy

ShelterSchedule overrides.

## 188F.12 No coercion stat

## 188F.13 Enforcement state

Per survivor or shelter?

Prefer:
- shelter policy default;
- per-survivor exception
if UI/control needs it.

## 188F.14 No global duplicate setting inside routine if governance already owns schedule strictness

Audit first.

## 188F.15 Anti-toggle exploit

Changing enforcement cannot:
- erase missed-work event;
- instantly reset routine quality;
- cancel ongoing consequence.

### 188F DoD

Routine enforcement expresses how strongly the shelter tries to follow personal schedules while respecting survivor autonomy, fitness, shelter emergencies, and real-world constraints.

---

# TASK 188G — Persistence, Migration, Determinism & Performance

# 188G.0 Goal

Make routine state compact, stable, and cheap.

## 188G.1 Persist intended routine

Potential:

```text
survivor_id
template_id
custom_windows
preference overrides
enforcement override
last_edit_day
```

## 188G.2 Persist intrinsic preferences only if not already in survivor/personality state

## 188G.3 Do not persist daily derived quality forever

## 188G.4 Persist recent quality trend only if needed

## 188G.5 Persist unresolved real conflicts only if their source commitments persist

Otherwise derive.

## 188G.6 Old save compatibility

Critical:

```text
default routine_mode = none
```

or:
- standard template with **no retroactive penalties**.

Recommended:
- `none` for old saves;
- optional one-click adopt Standard later.

This best preserves current behavior.

## 188G.7 Do not auto-assign strict routines on migration

## 188G.8 New campaign default

Can use:
- standard/flexible
if onboarding introduces routines.

## 188G.9 Existing survivor preference

If survivor traits imply chronotype:
- infer only from explicit trait data.

Do not randomize old-save personality unless deterministic generation contract allows it.

## 188G.10 Missing template

Preserve resolved custom windows or fallback safely.

## 188G.11 Template version updates

Existing campaign should not silently change personal schedules.

Persist resolved routine windows.

Template ID remains provenance.

## 188G.12 Determinism

No wall clock.

## 188G.13 RNG

Routine assignment uses seeded RNG only if preferences/templates are generated.

Manual/default assignment needs no RNG.

## 188G.14 Stable seed

Campaign seed + survivor ID.

## 188G.15 Processing

Use:
- schedule boundaries;
- phase events;
- condition changes.

## 188G.16 No per-frame scan

## 188G.17 Complexity

Target:
- O(active survivors) at relevant boundaries;
- conflict detection indexed by resource/window.

## 188G.18 No O(N²) all-survivor social clash scan

## 188G.19 Conflict indexes

By:
- room;
- workspace;
- commitment.

## 188G.20 Save/load

Restore does not replay:
- meal;
- sleep;
- work;
- relationship;
- conflict event.

## 188G.21 Transaction idempotence

Activity intent transition has stable source ID.

### 188G DoD

Routines add minimal persistent state, preserve old-save behavior, and process through time/event boundaries without quadratic survivor scans or replayed domain effects.

---

# TASK 188H — UI, Player Workflow & Low-Micromanagement Design

# 188H.0 Goal

Make routines fast to manage and easy to ignore when the player does not want fine-grained control.

## 188H.1 Reuse survivor detail

Show:
- routine template;
- wake/sleep preference;
- work preference;
- current intent;
- recent disruption.

## 188H.2 Routine editor

Timeline editor if:
- current UI system supports it.

## 188H.3 Fast actions

Essential:

```text
Use Standard
Use Early
Use Late
Match Duty
Reset
Copy Routine
```

## 188H.4 Batch assignment

Needed for larger shelters.

## 188H.5 Copy to selected survivors

## 188H.6 Auto-fit

“Fit routine around current duty.”

## 188H.7 Conflict panel

Show only actual actionable collisions.

## 188H.8 No giant “all preferences” dashboard by default

## 188H.9 Daily timeline

Useful as:
- optional overview.

## 188H.10 Shelter schedule overlay

Overlay:
- shelter phase;
- routine blocks;
- duty blocks.

## 188H.11 Color

Do not rely solely on color.

## 188H.12 Icons/text

Activity label + time.

## 188H.13 Satisfaction/quality display

Prefer:
- Good;
- Strained;
- Disrupted;
with reasons.

## 188H.14 Exact score

Advanced tooltip if useful.

## 188H.15 Conflict resolution shortcut

Navigate to:
- routine;
- duty;
- room;
- care assignment.

## 188H.16 Warning prioritization

Only:
- repeated sleep loss;
- missed meals;
- impossible duty overlap;
- severe care collision.

## 188H.17 No notification for minor timing preference miss

## 188H.18 Tutorial

First routine assignment or first meaningful conflict.

## 188H.19 Accessibility

- keyboard;
- controller;
- timeline screen-reader alternative;
- large text;
- no drag-only editing.

## 188H.20 Reduced motion

No animated time cursor requirement.

## 188H.21 Localization

Activity labels and template descriptions keyed.

## 188H.22 24h/12h time format

Use user locale/settings if framework supports.

## 188H.23 Custom times

Accessible numeric/spinner alternative to drag.

## 188H.24 Mobile/small panel

Avoid dense 24-hour grid as only representation.

### 188H DoD

The routine system can be managed through templates and batch operations in seconds, while advanced players can inspect and edit details without turning the shelter into schedule micromanagement.

---

# TASK 188I — Cross-System Consequences & Semantic Events

# 188I.0 Goal

Expose meaningful routine outcomes to existing systems exactly once.

## 188I.1 Semantic routine events

Candidate:

```text
routine_assigned
routine_changed
routine_deviation
routine_conflict_started
routine_conflict_resolved
chronic_routine_disruption
```

## 188I.2 Avoid source event spam

Do not automatically emit:
- The Routine;
- The Conflict;
- The Resolution;
- The Adjustment;
- The Deprivation;
- The Satisfaction;
- The Clash;
- The Compromise
for every mundane case.

These are narrative-content candidates, not required engine events.

## 188I.3 Sleep deprivation

NeedsSystem detects fatigue.

Routine can attach:
- schedule_cause metadata.

Do not duplicate deprivation detection.

## 188I.4 Missed meal

Food/Needs authority owns:
- hunger.

Routine records:
- missed preferred meal window.

## 188I.5 Social isolation

Relations/mental-health authority owns it if such mechanic exists.

Routine supplies:
- actual lack of social opportunity data.

## 188I.6 Stress

Plan 179 can consume:
- chronic disruption event.

## 188I.7 Morale

Use one bounded adapter if current morale model supports routine-quality input.

## 188I.8 Productivity

Recommended:
- no direct routine-quality productivity bonus.

Needs/fatigue and Plan 137 already represent performance consequences.

## 188I.9 Conflict stress

Only actual unresolved repeated conflict.

## 188I.10 Quest hooks

Plan 171 can consume semantic events.

Source quest ideas:
- establish routines;
- resolve conflicts;
- maintain balance.

Treat as content backlog, not system requirement.

## 188I.11 “5 early risers” quest

Avoid rewarding arbitrary template assignment unrelated to survivor preferences.

If used:
- require genuine matching preference or operational need.

## 188I.12 “80% satisfaction”

Avoid exact threshold quest unless score is stable/meaningful.

## 188I.13 Journal

Only meaningful:
- major schedule crisis;
- persistent night-shift conflict;
- survivor-requested adjustment
where narratively relevant.

## 188I.14 Archive

Routine history generally not archive-worthy.

## 188I.15 Analytics

Can track aggregate routine-quality.

### 188I DoD

Routine consequences are expressed through semantic, source-attributed events and canonical needs/social/mental-health systems rather than direct duplicate mutations.

---

# TASK 188J — Balance, Simulation & CI

# 188J.0 Goal

Prove that routines increase survivor individuality without reducing ASHFALL to schedule maintenance.

## 188J.1 30-day baseline

Compare:

```text
routine mode none
standard/flexible
mixed chronotypes
```

Track:
- sleep;
- meals;
- work attendance;
- routine deviations;
- player interventions.

## 188J.2 120-day normal shelter

Track:
- routine edits per survivor;
- conflicts;
- chronic disruption;
- fatigue;
- missed meals;
- work coverage.

## 188J.3 180-day mixed-shift shelter

Night work, medical care, education.

Assert:
- no schedule collapse;
- no conflict explosion.

## 188J.4 Emergency-heavy scenario

Routine must yield to shelter emergencies cleanly.

## 188J.5 Epidemic/medical scenario

Sick survivors:
- routines suspended/adjusted;
- no invalid work.

## 188J.6 Food shortage

Meal windows do not create phantom consumption.

## 188J.7 No-bed scenario

Sleep quality degrades through actual sleep/bed mechanics.

## 188J.8 Small shelter

2–4 survivors:
- routine system useful but low overhead.

## 188J.9 Large shelter

20+ survivors:
- batch templates;
- no N² scan.

## 188J.10 All-night-shift scenario

Shelter phase/curfew compatibility.

## 188J.11 Mixed chronotype roommate scenario

Only actual disturbance matters.

## 188J.12 No routine scenario

Current behavior preserved.

## 188J.13 Strict mode

Still respects emergency/health.

## 188J.14 Flexible mode

Preferences matter without starvation.

## 188J.15 Micromanagement metric

Track:

```text
routine edits per 30 days
alerts per survivor per 30 days
manual conflict resolutions
```

Set budget.

## 188J.16 Satisfaction double-count metric

Compare:
- fatigue effect;
- hunger effect;
- routine-quality effect.

Ensure no punitive stacking.

## 188J.17 Performance

Measure:
- intent resolution;
- conflict indexing;
- UI timeline construction.

---

# 188J-T — Testing & CI

## 188J.T1 Data integrity

Validate:
- template IDs;
- windows;
- activities;
- time format;
- overlap;
- consumer adapters;
- localization.

## 188J.T2 Selftest

Create:

```text
--survivor-routine-selftest
```

## 188J.T3 Selftest cases

At least:

1. no routine/current behavior;
2. standard flexible;
3. early preference;
4. late preference;
5. overnight sleep;
6. duty alignment;
7. emergency override;
8. sick survivor;
9. missed meal;
10. unavailable bed;
11. actual workspace-capacity conflict;
12. non-overlapping workspace no conflict;
13. different meal times no false conflict;
14. social preference no automatic clash;
15. strict mode;
16. flexible mode;
17. routine edit;
18. old save;
19. save/load;
20. headless.

## 188J.T4 Source-scan authority gate

Detect:
- routine-owned hunger;
- routine-owned fatigue;
- routine-owned morale;
- routine-owned relationship score;
- routine-owned duty assignment state;
- routine-owned bed assignment;
- per-frame full survivor scan;
- all-pairs social conflict scan.

## 188J.T5 Content acceptance

Every activity kind:
- has consumer;
or is presentation-only/deferred.

## 188J.T6 Reachability

Every shipped template:
- deterministic fixture.

Every conflict type:
- real fixture.

## 188J.T7 Golden schedule

Fixed fixture:
- resolved intents for 24h.

## 188J.T8 Determinism fingerprint

Same state:
- same intent timeline.

## 188J.T9 Old-save parity

`none` mode reproduces current behavior.

## 188J.T10 Generated docs

Create:
- `ROUTINE_ARCHITECTURE.md`;
- `ROUTINE_AUTHORITY_MATRIX.md`;
- `ROUTINE_TEMPLATE_MATRIX.md`;
- `ROUTINE_ACTIVITY_ADAPTER_MATRIX.md`;
- `ROUTINE_CONFLICT_MATRIX.md`;
- `ROUTINE_TEMPORAL_SEMANTICS.md`;
- `ROUTINE_MIGRATION_MATRIX.md`;
- `ROUTINE_BALANCE_REPORT.md`;
- `ADR_PERSONAL_ROUTINES_VS_SHELTER_SCHEDULE.md`;
- `ADR_ROUTINE_QUALITY_VS_MORALE.md`;
- `ADR_ROUTINE_CONFLICT_SEMANTICS.md`.

### 188J DoD

Personal routines remain deterministic, performant, low-maintenance, backward-compatible, and grounded in actual shelter activities and constraints.

---

# TASK 188K — Advanced Adaptive Routines, Dynamic Preferences & Narrative Follow-Ons

# 188K.0 Goal

Keep adaptive personality/routine evolution out of the MVP until static routines prove useful.

## 188K.1 Adaptive chronotype

Could shift after prolonged night work.

Follow-on only.

## 188K.2 Learned routine preference

Repeated successful schedule may become preferred.

Needs explicit design.

## 188K.3 Survivor-initiated routine edits

Potential autonomy feature.

## 188K.4 Habit formation

Could reduce disruption cost.

Do not create a second habit stat without a consumer.

## 188K.5 Routine legacy

Reject as a generic feature.

Archive only truly significant life patterns if narrative system supports.

## 188K.6 Routine trading

Source proposes “survivors swap routines.”

Reframe:
- shift swap;
- duty-schedule negotiation.

DutyRoster owns work swap.

## 188K.7 Routine-specific quests

Plan 171 only.

## 188K.8 Routine disruption events

Use:
- emergency;
- illness;
- weather;
- power outage;
- crowding
from real systems.

## 188K.9 Shift bargaining

Could integrate with autonomy/governance.

## 188K.10 Household routines

Family/caregiving follow-on.

## 188K.11 Child routines

Plan 183/childcare authority.

Do not generalize adult assumptions to children.

### 188K DoD

Advanced routine evolution remains a follow-on layer over the same canonical duty, autonomy, family, education, and event systems.

---

# 5. Routine State Model

```text
PREFERENCE PROFILE
      │
      ▼
ROUTINE TEMPLATE / CUSTOM WINDOWS
      │
      ▼
CURRENT WORLD CONSTRAINTS
      │
      ├── shelter phase
      ├── duty
      ├── health
      ├── needs urgency
      ├── emergency
      ├── care/education
      └── autonomy
      │
      ▼
RESOLVED ACTIVITY INTENT
      │
      ▼
CANONICAL DOMAIN ACTION
      │
      ▼
ACTUAL OUTCOME
      │
      ▼
ROUTINE QUALITY / DEVIATION
```

---

# 6. Routine vs Shelter Schedule Contract

Shelter schedule owns:

```text
macro phase
curfew
emergency
bed/shelter-wide constraints
```

Routine owns:

```text
personal preference
personal windows
```

---

# 7. Routine vs Duty Contract

DutyRoster owns:
- what job;
- who is assigned;
- fitness/eligibility.

Routine can express:
- preferred shift/window.

---

# 8. Routine vs Needs Contract

Routine creates opportunities.

NeedsSystem owns:
- actual hunger;
- thirst;
- fatigue;
- hygiene;
- morale if present there.

---

# 9. Routine vs Food Contract

Preferred eating time is not food consumption.

Only food authority:
- checks availability;
- consumes ration;
- updates hunger.

---

# 10. Routine vs Sleep Contract

Scheduled sleep time is not actual sleep.

Actual:
- bed;
- interruptions;
- duration;
- environment
determine sleep outcome.

---

# 11. Routine vs Relations Contract

Scheduled social time is an interaction opportunity.

Relations system decides:
- whether interaction happens;
- result.

---

# 12. Preference vs Assigned Routine Contract

A survivor can prefer:
- late schedule.

Player can assign:
- early schedule.

Routine quality can reflect mismatch.

Intrinsic preference does not silently change because player edited schedule.

---

# 13. Enforcement Contract

`none`:
- current broad behavior.

`flexible`:
- soft preference.

`strict`:
- higher schedule priority.

All yield to:
- hard physical/world constraints.

---

# 14. Deviation Contract

A deviation is:

```text
planned activity
≠ actual/available activity
```

with reason.

No generic punishment.

---

# 15. Conflict Contract

Conflict requires:
- actual incompatible simultaneous commitments/resource use.

Not:
- different preferences.

---

# 16. Workspace Contract

Conflict only if:

```text
occupancy > capacity
```

during overlap.

---

# 17. Meal Contract

Different meal windows are valid.

Conflict only if real:
- kitchen/service constraint.

---

# 18. Social Compatibility Contract

Introversion/extroversion alone does not create hostility.

Actual social outcomes belong to relations/personality.

---

# 19. Routine Quality Contract

Derived from actual day.

It is not:
- new need;
- new morale bar.

---

# 20. Mental Health Contract

Plan 179 may consume:
- chronic disruption;
- repeated sleep conflict;
- sustained lack of valued personal/social time.

Not:
- one low daily score.

---

# 21. Plan 137 Performance Contract

Avoid direct routine productivity modifiers by default.

Sleep/fatigue/needs already affect performance.

Any extra preference-fit performance effect requires explicit no-double-count proof.

---

# 22. Education Contract

Education blocks:
- time preference/commitment.

Plan 154 owns:
- class;
- learning;
- teacher/student.

---

# 23. Caregiving Contract

Caregiving commitment can override personal block.

CaregivingSystem owns:
- assignment;
- care outcome.

---

# 24. Expedition Contract

Away survivor:
- shelter routine suspended.

Do not simulate shelter meal/sleep blocks during expedition unless ExpeditionSystem explicitly consumes a travel routine profile.

---

# 25. Emergency Contract

Shelter emergency:
- temporarily overrides routine.

After emergency:
- resolver resumes from current time.

No replay of missed blocks.

---

# 26. Curfew Contract

Curfew constrains:
- allowed movement/activity.

Routine adapts.

---

# 27. Persistence Matrix

| Fact | Owner |
|---|---|
| campaign time | calendar |
| shelter phase | ShelterSchedule |
| bed assignment | ShelterSchedule/room |
| duty | DutyRoster |
| survivor health/availability | lifecycle/medical |
| needs | NeedsSystem |
| food consumed | food/rationing |
| relationship | SurvivorRelations |
| education | education |
| care assignment | CaregivingSystem |
| personal preference | routine/personality |
| assigned personal routine | routine |
| current intent | derived |
| daily routine quality | derived/recent aggregate |
| real conflict | source authority + routine coordinator |
| morale/stress | canonical morale/mental health |

---

# 28. Old-Save Migration Contract

Recommended:

```text
routine_mode = none
```

for existing saves.

This preserves current behavior.

Optional:
- prompt player to adopt Standard/Flexible later.

No migration penalty.

---

# 29. Template Versioning Contract

Persist resolved windows.

Future template changes:
- affect new assignments;
- not existing survivor schedules unless player resets.

---

# 30. Deterministic Preference Generation

If survivors lack authored preference:

```text
campaign_seed
+ survivor_id
→ stable preference
```

Only if design wants procedural preference.

Old saves should not change preference each load.

---

# 31. Exactly-Once Event Identity

Stable:

```text
routine_change:<survivor>:<sequence>
conflict:<resource>:<day>:<survivor-set>
chronic_disruption:<survivor>:<episode>
```

---

# 32. Failure Injection Matrix

## N188.1 Routine directly sets hunger
Expected: needs authority gate fails.

## N188.2 Routine directly increments relationship after social block
Expected: relations authority gate fails.

## N188.3 Two survivors use same workspace at non-overlapping times and conflict appears
Expected: conflict-semantics test fails.

## N188.4 Two survivors prefer different meal times and conflict appears without shared constraint
Expected: false-conflict gate fails.

## N188.5 Introvert/extrovert pair automatically loses relationship
Expected: social-authority gate fails.

## N188.6 Strict routine forces medically unfit survivor to work
Expected: fitness/precedence gate fails.

## N188.7 Emergency starts but routine keeps leisure block
Expected: shelter-schedule gate fails.

## N188.8 Old save gets Standard/Strict and immediate penalties
Expected: migration parity fails.

## N188.9 Night-owl preference changes every reload
Expected: deterministic preference fail.

## N188.10 Daily routine scan runs every frame
Expected: performance/source-scan fails.

## N188.11 Routine quality adds work penalty on top of fatigue penalty for same missed sleep
Expected: double-count gate fails.

## N188.12 Routine restore replays meal/work/social effects
Expected: idempotence fails.

---

# 33. Determinism Contract

Same:

```text
campaign time
+ shelter phase
+ survivor state
+ duty
+ routine
+ needs urgency
+ commitments
+ autonomy state
```

must yield same:
- intended activity;
- deviation reason;
- conflict;
- quality projection.

Seeded RNG only for deliberately equivalent discretionary choices.

---

# 34. Long-Horizon Metrics

Track:

```text
routine mode adoption
template distribution
routine edits
intent transitions
deviations
deviation reasons
sleep-window adherence
meal-window adherence
work-window adherence
actual conflicts
conflict resolutions
routine-quality bands
chronic disruption episodes
alerts
processing time
state bytes
```

---

# 35. Micromanagement Budget

Set explicit UX targets.

Example targets for normal play:

```text
routine edits per survivor per 30 days: low
routine alerts per survivor per 30 days: low
mandatory manual conflict resolution: uncommon
```

Tune from testing.

---

# 36. Balance Guardrails

Routines should create:
- personality;
- shift planning;
- emergent timing;
- scheduling tradeoffs.

They should not create:
- hourly babysitting;
- arbitrary morale loss;
- false interpersonal conflict;
- duplicate survival penalties.

---

# 37. Chronotype Guardrails

Chronotype affects:
- preference;
- routine quality;
possibly bounded shift comfort.

Do not hardcode:
- early = productive;
- late = lazy.

---

# 38. Sleep Guardrails

Actual fatigue system remains dominant.

Routine mismatch can explain:
- why sleep opportunity was poor.

---

# 39. Meal Guardrails

Food scarcity dominates.

Routine should not produce:
- extra hunger penalties beyond missed eating itself.

---

# 40. Social Guardrails

Social time:
- optional opportunity.

A survivor preferring solitude should not be punished for not socializing.

---

# 41. Conflict Guardrails

Aim for:
- few, meaningful, solvable conflicts.

Not:
- constant pairwise incompatibility notifications.

---

# 42. UI Acceptance

## Survivor detail
- template;
- preference;
- current intent;
- next transition.

## Editor
- fast templates;
- timeline;
- batch/copy.

## Conflict view
- only real collisions;
- direct resolution links.

## Quality
- readable band + reasons.

---

# 43. Accessibility

- timeline has list/text alternative;
- no drag-only editing;
- keyboard/controller support;
- no color-only activity/conflict states;
- time values readable;
- text scaling.

---

# 44. Localization

Templates/activity labels:
- localization keys.

Time:
- locale format.

No logic encoded in strings.

---

# 45. Content Acceptance

Routine template ladder:

```text
DISCOVERED
LOADED
REGISTERED
ASSIGNED
RESOLVED
DOMAIN_CONSUMED
OUTCOME_OBSERVED
```

Every activity kind must reach a real consumer or be explicitly presentation-only.

---

# 46. Reachability

Each shipped template:
- deterministic survivor fixture.

Each activity:
- real consumer fixture.

Each conflict:
- actual capacity/commitment fixture.

No fake content count.

---

# 47. Performance Guardrails

- next-boundary scheduling;
- event-driven recompute;
- resource-indexed conflicts;
- no N² social compatibility scan;
- no full-day reconstruction every frame.

---

# 48. CI / Gate Set

Recommended:

```text
routine_authority_matrix
routine_shelter_schedule_precedence
routine_duty_authority
routine_needs_authority
routine_relations_authority
routine_template_integrity
routine_window_integrity
routine_false_conflict_guard
routine_emergency_override
routine_fitness_override
routine_quality_no_double_count
routine_old_save_parity
routine_determinism
routine_performance
routine_long_horizon
routine_ui_access
```

---

# 49. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivor-routine-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 50. Recommended Commit Breakdown

```text
188A-1 shelter/duty/needs/autonomy authority audit
188A-2 personal-vs-shelter schedule ADR
188A-3 routine-quality-vs-morale ADR
188A-4 conflict-semantics ADR
188A-5 routine preference DTO
188A-6 template schema/loader
188A-7 time-window validation
188A-8 docs/tests

188B-1 intent resolver
188B-2 precedence policy
188B-3 emergency/curfew override
188B-4 needs-urgency fallback
188B-5 duty/medical/care commitment handling
188B-6 next-boundary scheduling
188B-7 diagnostics
188B-8 deterministic tests

188C-1 sleep adapter
188C-2 meal adapter
188C-3 work adapter
188C-4 social adapter
188C-5 personal/hygiene adapter
188C-6 education/care adapters
188C-7 expedition suspension
188C-8 no-double-count tests

188D-1 daily routine-quality read model
188D-2 sleep/meal/work/social fit
188D-3 chronic disruption projection
188D-4 morale/stress adapter
188D-5 no-direct-productivity decision
188D-6 quality diagnostics
188D-7 tests/docs

188E-1 resource/commitment conflict index
188E-2 workspace capacity conflicts
188E-3 sleep disturbance
188E-4 care/education/duty collision
188E-5 false meal/social conflict regression
188E-6 resolution adapters
188E-7 conflict idempotence
188E-8 docs

188F-1 none/flexible/strict policy
188F-2 autonomy integration
188F-3 governance/shelter default audit
188F-4 survivor-requested adjustment
188F-5 anti-toggle exploit tests
188F-6 docs

188G-1 persistence schema
188G-2 old-save `none` migration
188G-3 resolved template snapshot
188G-4 deterministic preference generation if needed
188G-5 performance indexes
188G-6 restore idempotence
188G-7 migration tests
188G-8 docs

188H-1 survivor-detail routine UI
188H-2 fast template/batch operations
188H-3 timeline editor/list alternative
188H-4 conflict navigation
188H-5 routine-quality presentation
188H-6 accessibility/localization
188H-7 snapshots
188H-8 tutorial/help

188I-1 semantic routine events
188I-2 needs/relations/mental-health adapters
188I-3 Plan-171 quest hook surface
188I-4 event budget/journal rules
188I-5 consequence tests

188J-1 30-day baseline sim
188J-2 120-day normal shelter
188J-3 180-day mixed-shift/emergency scenarios
188J-4 large-shelter performance
188J-5 micromanagement budget
188J-6 CI/failure fixtures/goldens
188J-7 generated reports
188J-8 final ship/no-ship report

188K-1 adaptive routine/shift-swap/family follow-on disposition
```

---

# 51. Risk Register

## R188.1 Creates a second duty scheduler

Mitigation:
- routine owns intent/preferences only;
- DutyRoster owns work.

## R188.2 Duplicates NeedsSystem

Mitigation:
- no routine-owned hunger/fatigue/hygiene.

## R188.3 Satisfaction double-punishes the player

Mitigation:
- derived quality;
- modest/no direct performance effect;
- no duplicate deprivation penalties.

## R188.4 False conflicts create noisy simulation

Mitigation:
- real resource/capacity semantics.

## R188.5 Schedule micromanagement overwhelms gameplay

Mitigation:
- templates;
- batch editing;
- flexible mode;
- low alert budget.

## R188.6 Strict schedules override survivor reality

Mitigation:
- hard precedence for health/emergency/availability.

## R188.7 Old saves become immediately worse

Mitigation:
- migrate to `none`.

## R188.8 Large shelters cause O(N²) scans

Mitigation:
- resource/window indexes;
- no pairwise personality comparisons.

## R188.9 Chronotype becomes stereotype

Mitigation:
- preference only;
- no productivity moralization.

## R188.10 UI timeline inaccessible

Mitigation:
- list alternative;
- keyboard numeric editing.

---

# 52. Acceptance Checklist

## P0

- [ ] campaign clock audited
- [ ] ShelterSchedule phases audited
- [ ] bed assignment audited
- [ ] curfew/emergency rules audited
- [ ] DutyRoster APIs audited
- [ ] duty time granularity audited
- [ ] NeedsSystem audited
- [ ] sleep/recovery audited
- [ ] food/rationing audited
- [ ] survivor availability audited
- [ ] autonomy/refusal audited
- [ ] workspace/room capacity audited
- [ ] SurvivorRelations audited
- [ ] education schedules audited
- [ ] caregiving audited
- [ ] expedition away-state audited
- [ ] mental-health integration audited
- [ ] save order audited
- [ ] UI schedule surfaces audited
- [ ] routine authority matrix
- [ ] personal-vs-shelter ADR
- [ ] routine-quality-vs-morale ADR
- [ ] conflict semantics ADR
- [ ] time resolution confirmed
- [ ] baseline no-personal-routine captured

## 188A — Preferences/Templates

- [ ] routine coordinator has narrow ownership
- [ ] no survival/social duplicate state
- [ ] canonical time primitive
- [ ] typed time-window DTO
- [ ] only real activity kinds ship
- [ ] `personal` fallback for unsupported subactivities
- [ ] versioned template catalog
- [ ] template DTO
- [ ] early/late/standard defaults
- [ ] Custom is resolved user schedule
- [ ] source times treated as tuning
- [ ] neutral chronotype terminology
- [ ] meal model audited
- [ ] no forced three-meal assumption
- [ ] work preference
- [ ] social preference not caricature
- [ ] exercise only if real
- [ ] preference source documented
- [ ] intrinsic preference separate from assigned routine
- [ ] mismatch feeds quality
- [ ] window validation
- [ ] overnight support
- [ ] deterministic day wrap
- [ ] precedence aligned with real authorities
- [ ] generated template matrix

## 188B — Resolver

- [ ] pure resolver where possible
- [ ] all canonical inputs
- [ ] typed RoutineIntent
- [ ] intent does not execute
- [ ] precedence defined
- [ ] urgent hunger can override preference
- [ ] urgent fatigue can override
- [ ] emergency override
- [ ] duty change update
- [ ] expedition suspension
- [ ] medical replacement
- [ ] refusal integration
- [ ] flexible windows
- [ ] rigid still respects hard constraints
- [ ] fallback reason
- [ ] bounded fallback chain
- [ ] stable tie-break
- [ ] RNG only if genuinely needed
- [ ] no per-frame full recompute
- [ ] next-boundary scheduling
- [ ] headless
- [ ] diagnostics

## 188C — Sleep

- [ ] sleep authority identified
- [ ] routine only supplies window
- [ ] bed authority unchanged
- [ ] sleep request through canonical path
- [ ] no-bed deviation
- [ ] Needs owns fatigue
- [ ] actual sleep duration used
- [ ] no hard-coded sleep medical truth
- [ ] actual disturbance source
- [ ] chronotype not direct recovery modifier by default
- [ ] night shift works if allowed

## 188C — Meals

- [ ] food authority identified
- [ ] meal window is preference
- [ ] actual consumption canonical
- [ ] no-food behavior canonical
- [ ] ration policy respected
- [ ] flexible meal frequency
- [ ] no breakfast/lunch/dinner logic dependency
- [ ] social meal only through actual shared event

## 188C — Work

- [ ] DutyRoster remains owner
- [ ] routine work window is preference
- [ ] no routine-generated task
- [ ] fitness/Plan 137 respected
- [ ] medically unfit deviation
- [ ] work output canonical
- [ ] preference productivity adapter only if justified
- [ ] workspace capacity real
- [ ] overtime real

## 188C — Social/Personal

- [ ] social block is opportunity
- [ ] participants required
- [ ] co-presence if modeled
- [ ] relations owns outcome
- [ ] no introvert/extrovert automatic clash
- [ ] missed social block does not decay relation
- [ ] personal umbrella
- [ ] hygiene canonical
- [ ] exercise only if real
- [ ] leisure only if real
- [ ] reading/study routes to education
- [ ] no fake block stat bonuses

## 188D — Quality

- [ ] derived day-quality model
- [ ] actual outcomes used
- [ ] sleep fit
- [ ] meal fit
- [ ] work fit
- [ ] social fit
- [ ] personal fit only if real
- [ ] weighted by preference
- [ ] no full persistent satisfaction history
- [ ] bounded morale adapter
- [ ] deprivation double-count prevented
- [ ] no direct productivity bonus by default
- [ ] no double work penalty
- [ ] semantic quality bands
- [ ] 7-day trend
- [ ] health vs preference fit separated
- [ ] Needs owns deprivation
- [ ] Plan 179 chronic disruption only
- [ ] diagnostics

## 188E — Conflicts

- [ ] no preference-only conflicts
- [ ] real conflict source types
- [ ] workspace conflict requires capacity overflow
- [ ] different meal times no false conflict
- [ ] sleep disturbance requires shared cause
- [ ] no social-label clash
- [ ] mandatory schedule collision
- [ ] education collision
- [ ] caregiving collision
- [ ] stable conflict DTO
- [ ] deterministic conflict ID
- [ ] event-driven detection
- [ ] resolution via owning systems
- [ ] no direct duty reassignment bypass
- [ ] compromise as proposed routine change
- [ ] unresolvable only if no alternative
- [ ] severity grounded
- [ ] closure grounded
- [ ] no daily duplicate
- [ ] bounded history

## 188F — Enforcement/Autonomy

- [ ] none/flexible/strict semantics
- [ ] none preserves current behavior
- [ ] flexible is preference
- [ ] strict respects hard constraints
- [ ] no generic deviation penalty
- [ ] preference mismatch can affect quality
- [ ] Plan 144 refusal
- [ ] player override
- [ ] survivor-requested adjustment if real
- [ ] governance only sets policy
- [ ] emergency override
- [ ] no coercion stat
- [ ] global/per-survivor policy authority decision
- [ ] no duplicate governance setting
- [ ] anti-toggle exploit

## 188G — Persistence/Performance

- [ ] intended routine persisted
- [ ] intrinsic preferences not duplicated
- [ ] no daily quality history bloat
- [ ] unresolved conflict persistence justified
- [ ] old save -> none
- [ ] no old-save strict assignment
- [ ] new-campaign default explicit
- [ ] no invented old-save personality
- [ ] missing-template fallback
- [ ] resolved windows persist through template updates
- [ ] no wall clock
- [ ] seeded preference only if needed
- [ ] stable seed
- [ ] boundary/event processing
- [ ] no per-frame scan
- [ ] O(active survivors) target
- [ ] resource-indexed conflicts
- [ ] no N² social scan
- [ ] restore no side-effect replay
- [ ] transition idempotence

## 188H — UI

- [ ] survivor detail summary
- [ ] timeline editor only if useful
- [ ] fast template actions
- [ ] batch assignment
- [ ] copy routines
- [ ] auto-fit duty
- [ ] actual-conflict panel
- [ ] no giant preference dashboard
- [ ] optional daily overview
- [ ] shelter phase overlay
- [ ] no color-only state
- [ ] text/icon labels
- [ ] quality band + reasons
- [ ] direct resolution navigation
- [ ] warning priority
- [ ] no minor timing alert spam
- [ ] tutorial
- [ ] keyboard/controller
- [ ] screen-reader list alternative
- [ ] no drag-only editor
- [ ] reduced motion
- [ ] localization
- [ ] locale time format
- [ ] numeric/spinner alternative
- [ ] small-screen safe

## 188I — Consequences

- [ ] semantic event family bounded
- [ ] source narrative events treated as content candidates
- [ ] fatigue deprivation remains Needs-owned
- [ ] missed meal remains food/Needs-owned
- [ ] social isolation remains social/mental-health-owned
- [ ] chronic disruption only
- [ ] bounded morale adapter
- [ ] no direct productivity bonus
- [ ] real repeated conflict stress only
- [ ] Plan 171 quest hook surface
- [ ] no arbitrary-template quests by default
- [ ] no unstable exact satisfaction quest
- [ ] journal only major cases
- [ ] no routine archive spam
- [ ] analytics optional

## 188J — Simulation

- [ ] 30-day none vs flexible
- [ ] 120-day normal shelter
- [ ] 180-day mixed shifts
- [ ] emergency-heavy
- [ ] epidemic/medical
- [ ] food shortage
- [ ] no-bed
- [ ] small shelter
- [ ] large shelter
- [ ] all-night-shift
- [ ] mixed chronotype roommates
- [ ] no-routine parity
- [ ] strict mode hard constraints
- [ ] flexible mode survival-safe
- [ ] micromanagement metric
- [ ] double-count metric
- [ ] performance measured

## 188J — CI

- [ ] template integrity
- [ ] time-window integrity
- [ ] activity-consumer integrity
- [ ] routine selftest
- [ ] no-routine parity
- [ ] duty authority gate
- [ ] needs authority gate
- [ ] relation authority gate
- [ ] bed authority gate
- [ ] false-conflict regression
- [ ] emergency override
- [ ] fitness override
- [ ] quality no-double-count
- [ ] old-save migration
- [ ] deterministic golden
- [ ] performance gate
- [ ] content acceptance
- [ ] reachability
- [ ] generated docs
- [ ] verify-fast

## 188K

- [ ] adaptive chronotype deferred
- [ ] learned routine preference deferred
- [ ] survivor-initiated edits follow autonomy
- [ ] no duplicate habit stat
- [ ] routine legacy rejected by default
- [ ] “routine trading” reframed as shift swap
- [ ] Plan 171 owns routine quest generation
- [ ] disruptions come from real events
- [ ] shift bargaining follow-on
- [ ] household routines follow family/caregiving
- [ ] child routines follow Plan 183

---

# 53. Ship / No-Ship Gate

**SHIP** only if:

```text
campaign_clock_authorities == 1
AND shelter_schedule_authorities == 1
AND duty_assignment_authorities == 1
AND routine_owned_hunger_state == false
AND routine_owned_fatigue_state == false
AND routine_owned_food_consumption == false
AND routine_owned_relationship_score == false
AND routine_owned_bed_assignment == false
AND routine_owned_final_work_output == false
AND false_workspace_conflicts == 0
AND false_meal_time_conflicts == 0
AND automatic_introvert_extrovert_conflicts == 0
AND strict_mode_bypasses_medical_or_emergency_constraints == false
AND routine_quality_double_counts_needs_penalties == false
AND old_save_immediate_routine_penalty == false
AND per_frame_full_routine_scans == 0
AND all_pairs_social_conflict_scans == 0
AND unseeded_routine_rng == 0
AND restore_replays_routine_domain_effects == false
AND routine_old_save_parity == pass
AND routine_save_roundtrip == pass
AND routine_shelter_schedule_precedence == pass
AND routine_duty_integration == pass
AND routine_needs_integration == pass
AND routine_conflict_semantics == pass
AND routine_quality_no_double_count == pass
AND routine_determinism == pass
AND routine_30_day_balance == pass
AND routine_120_day_balance == pass
AND routine_180_day_balance == pass
AND routine_large_shelter_performance == pass
AND routine_micromanagement_budget == pass
AND survivor_routine_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 54. Implementer Handoff

1. Audit the campaign clock, ShelterSchedule, DutyRoster, Needs, food, autonomy, workspace, relations, education and caregiving authorities before writing a routine system.
2. Treat the personal routine as an **intent/preference layer**, not an execution engine.
3. Preserve ShelterSchedule as owner of Day/Night/Curfew/Emergency.
4. Preserve DutyRoster as owner of actual work assignments.
5. Preserve NeedsSystem as owner of fatigue, hunger, hygiene and related survival state.
6. Preserve food/rationing as owner of actual eating.
7. Preserve SurvivorRelations as owner of relationship state.
8. Represent routine templates as data-driven preferred time windows.
9. Treat Early, Late and Standard times as tuning candidates, not universal biological truth.
10. Do not force breakfast/lunch/dinner logic if the food system does not model three meals.
11. Separate intrinsic preference from player-assigned routine.
12. Resolve current activity intent from routine + shelter rules + duty + health + needs urgency + commitments + autonomy.
13. Let emergencies, illness, expeditions and critical needs override routine through canonical rules.
14. Recompute only on time/window/state boundaries, not every frame.
15. Make sleep windows request actual sleep; only actual sleep affects fatigue.
16. Make meal windows request actual eating; only actual food consumption affects hunger.
17. Make social blocks create interaction opportunities; do not grant relationship points for being scheduled.
18. Make personal/hygiene/exercise/leisure subtypes ship only when real systems consume them.
19. Replace the source's broad persistent satisfaction model with a derived routine-quality read model.
20. Avoid direct productivity bonuses/penalties where Needs/Plan 137 already represent the same cause.
21. Define conflicts only around actual resource capacity or incompatible commitments.
22. Explicitly regression-test that non-overlapping workspace use, different meal preferences, and introvert/extrovert labels do **not** create fake conflicts.
23. Implement None/Flexible/Strict as schedule-priority policy, never as a hard override of incapacity or emergency.
24. Migrate old saves to `none` by default to preserve current behavior.
25. Persist resolved routine windows so future template tuning does not silently rewrite existing survivors.
26. Provide fast templates, batch assignment, copy, and auto-fit-to-duty tools.
27. Track routine edits and alerts as a micromanagement budget.
28. Run 30/120/180-day plus large-shelter simulations before adding adaptive habits or more routine categories.
29. Close only when survivors feel like they have their own daily rhythms without requiring the player to become a minute-by-minute scheduler.

---

# 55. Final Outcome

When this plan is complete, ASHFALL's survivors gain a daily rhythm without creating a second simulation on top of the shelter.

One survivor can prefer to wake early, work mornings, eat after the first shift, and keep evenings for quiet personal time. Another can prefer late hours and evening duty. A third can use the standard shelter rhythm. The player can accept those preferences, override them, or fit schedules around operational demands.

But those schedules remain plans rather than magic commands.

If a survivor is sick, the medical/fitness systems can keep them from working.
If an emergency begins, the shelter schedule can override personal plans.
If food is unavailable, a meal block does not create food.
If no bed is free, a sleep block does not pretend the survivor slept.
If an expedition has taken someone outside the shelter, their shelter routine pauses.
If a survivor refuses an assignment, the autonomy system remains authoritative.

Actual outcomes still come from the systems that already know how the world works.

That also keeps routine quality honest.

A bad night is bad because the survivor actually slept poorly.
A missed meal matters because hunger actually increased.
A difficult shift matters because work and fatigue really happened.
A socially isolated survivor is affected only when the real relationship or mental-health systems recognize that condition.

The routine layer simply makes those causes legible.

Conflicts become real scheduling problems rather than invented personality drama. Two survivors using the same workshop at different times are fine. Two people preferring different meal times are fine. An introvert and an extrovert are not automatically enemies.

A conflict exists when two people need the same one-person workstation at the same time, when a night worker repeatedly wakes a roommate, when medical treatment collides with duty, or when caregiving and work commitments cannot both be satisfied.

Those are problems the player can actually solve.

The feature also stays manageable. Templates, copy/paste, batch assignment, auto-fit-to-duty, flexible enforcement and bounded warnings let the player configure rhythms without editing every hour of every day.

The result is not a spreadsheet of twenty-four-hour calendars.

It is a shelter where people wake, work, eat, rest, socialize and sleep according to recognizable individual patterns—patterns that bend when survival demands it, but still make each survivor feel like a person with a life between their assigned duties.
