# C1 — Flagship Integration Plan [21]: Needs → Performance Cascade — One Survival Pressure Projection Across Combat, Work & Expeditions

> **Output:** `C1_planintegration[21].md`
>
> **Source baseline:** Plan 137 — Needs → Performance Cascade
>
> **Primary mission:** connect canonical survivor needs to real combat, work, and expedition performance so hunger, thirst, fatigue, warmth and morale matter before they reach death thresholds.
>
> **Primary architectural rule:** do not create a second survivor-condition authority. The cascade must project from existing `NeedsSystem` and, where appropriate, compose with Plan-24 fitness/condition authority rather than compete with it.
>
> **Primary gameplay rule:** needs should create meaningful tradeoffs, not opaque punishment or unrecoverable death spirals.
>
> **Mandatory execution order:** 137A authority audit + pure projection contract → 137B combat/work/expedition consumers → 137C UI, telemetry, social/mental-health integration only where existing systems support it → 137D persistence, determinism, balance and CI hardening.
>
> **Critical scope rule:** the source plan contains several speculative mechanics (`Pushing Through`, `Desperate Strike`, `Cold Fury`, theft chance, needs mutations, special expedition roles, training resistance). None of those ship in the core integration unless the repository already has a matching event/tactic/social-effect authority and the mechanic can be expressed through it without creating a new subsystem.
>
> **Guardrails:** no new combat rule engine; no second duty-efficiency system; no second expedition stamina authority; no UI-side performance math; no saved modifier state if modifiers are derivable; no wall-clock or RNG in modifier calculation; no universal hardcoded death-spiral amplifier; no penalty hidden from the player; no stacking through duplicate condition paths; no fake work-quality/medical-error system if one does not already exist.

---

# 0. Mission

ASHFALL already tracks the things that should make a survivor perform differently from one day to the next:

- hunger;
- thirst;
- fatigue;
- warmth;
- morale;
- hygiene;
- trauma;
- somatic flashback penalties;
- trauma-bond effects;
- fitness/condition decisions;
- combat state;
- duty assignments;
- expedition state.

But the source baseline reports a continuity break:

```text
NeedsSystem
  hunger / thirst / fatigue / warmth / morale
        │
        └── mostly death-threshold pressure

TacticalCombatSystem
        │
        └── no NeedsSystem input

DutyRosterSystem
        │
        └── no NeedsSystem input

ExpeditionSystem
        │
        └── no NeedsSystem input
```

The result is a survival game in which a starving, exhausted, freezing survivor can still:
- shoot as accurately;
- work as fast;
- travel as effectively
as a well-fed, rested survivor until a threshold suddenly kills or disables them.

That creates the wrong pressure curve.

The target is:

```text
CANONICAL SURVIVOR STATE
      │
      ├── hunger
      ├── thirst
      ├── fatigue
      ├── warmth
      ├── morale
      ├── trauma/somatic modifiers
      └── fitness/condition
      │
      ▼
NeedsPerformanceProjection
      │
      ├── combat accuracy
      ├── combat damage / initiative / AP where supported
      ├── work throughput
      ├── expedition speed
      ├── expedition stamina
      └── carry / endurance where supported
      │
      ▼
EXISTING DOMAIN AUTHORITIES
      │
      ├────────► TacticalCombatSystem
      ├────────► DutyRoster / Production
      └────────► ExpeditionSystem
```

The projection should answer:

> Given this survivor's actual current needs, what performance multipliers should existing systems consume right now?

It should not answer:
- whether the survivor dies;
- whether they are medically fit;
- whether they refuse work;
- whether they steal;
- whether they trigger a narrative event.

Those remain other systems' jobs.

---

# 1. Source-Evidence Interpretation

## 1.1 The repository already has performance precedents

The source points to:
- `SomaticFlashbackSystem.workEfficiencyPenalty`;
- `TraumaBondSystem` work-efficiency language.

These must be audited before adding another modifier layer.

The flagship plan should produce one composition model where:
- needs;
- trauma;
- social bond;
- skill/role;
- fitness
can contribute without multiplying twice through hidden paths.

## 1.2 Plan 24 likely already introduced a fitness verdict

Recent integration work defines a canonical survivor condition / fitness path.

Therefore needs-performance should not bypass that authority if it already feeds duty/combat/expedition decisions.

The first task is to determine:
- whether `FitnessVerdict` already consumes needs;
- whether it blocks assignment only;
- whether it also provides throughput penalties.

## 1.3 The source thresholds may use inverted semantics

The baseline source says things like:
- “Hunger <30% no penalty”
- “>90% severe penalty.”

This suggests the field may mean **hunger severity** rather than fullness.

The implementation must re-read `NeedsSystem` units and names.

Never copy thresholds until semantics are confirmed.

## 1.4 Pure query is the right default

The proposed bridge needs no tick if:
- modifiers are derived only from current state.

That is desirable:
- no extra save state;
- no desynchronization;
- deterministic.

## 1.5 Combat/work/expedition are the real core

The source also proposes:
- special combat tactics;
- quest hooks;
- theft;
- morale events;
- role-based modifiers.

These are optional follow-ons unless matching authorities already exist.

The core flagship must first close the three documented dead seams.

---

# 2. Non-Negotiable Performance Invariants

## INV-137.1 — One canonical needs source

`NeedsSystem` owns hunger/thirst/fatigue/warmth/morale facts.

No cached duplicate need values in bridge consumers.

## INV-137.2 — Projection is pure

Same survivor state => same modifiers.

No RNG.

## INV-137.3 — Projection is derived

Do not persist multipliers.

Persist the underlying needs through existing save state.

## INV-137.4 — No double application

If Plan-24 fitness already contains a need effect:
- either reuse it;
- or separate assignment eligibility from throughput.

Never apply the same fatigue penalty twice.

## INV-137.5 — Multipliers are bounded

Every domain multiplier has:
- minimum;
- maximum;
- explicit neutral value.

## INV-137.6 — Recovery is automatic

Restore needs:
- multiplier returns toward 1.0 immediately/at next query.

No stale penalty state.

## INV-137.7 — No hidden last-effort state

A critical 0.3 multiplier at death threshold may be used only if:
- it matches existing incapacitation/death semantics.

If existing fitness already blocks the survivor:
- do not add contradictory “last effort.”

## INV-137.8 — Combat reads combat modifiers

Work modifiers never leak into combat by accident.

## INV-137.9 — Work reads work modifiers

Production output changes through the authoritative work/output path.

## INV-137.10 — Expedition reads expedition modifiers

Travel speed/stamina/carry changes through expedition authority.

## INV-137.11 — UI reads the same projection

No separate presentation formula.

## INV-137.12 — Every penalty is explainable

UI/diagnostics can show:
- need;
- tier;
- contribution;
- final multiplier.

## INV-137.13 — No death spiral by stealth

Balance tests must detect:
- hunger → less food production → more hunger runaway
under normal intended play.

## INV-137.14 — No speculative social consequence without owner

Hunger-irritability, theft, refusal, morale events, quest hooks and tactics require an existing system seam.

## INV-137.15 — Existing trauma/social modifiers compose once

Somatic flashback / trauma bond work effects must be unified or explicitly ordered.

## INV-137.16 — Skill/role offsets are not invented casually

If existing work/expedition role systems already expose skill modifiers:
- compose them.

Otherwise defer.

---

# 3. Definition of Done

Plan 137 closes only when:

- `NeedsSystem` field semantics/units are verified;
- current Plan-24 fitness integration is audited;
- current somatic-flashback and trauma-bond efficiency effects are audited;
- one composition order is documented;
- `NeedsPerformanceBridge` or equivalent pure projection exists only if no existing canonical projection already does;
- combat, work and expedition multipliers are explicit and bounded;
- threshold data is validated;
- modifiers recover to neutral when needs recover;
- combat consumes the projection;
- duty/production consumes the projection;
- expedition consumes the projection;
- no duplicate application occurs through fitness/trauma/social paths;
- UI surfaces show final multiplier and breakdown;
- warnings are semantic and not spammy;
- old saves load with no migration burden beyond existing needs data;
- no modifier save section exists unless a non-derivable future mechanic proves need;
- headless tests pass;
- data-integrity validation covers threshold ordering/ranges;
- `--needs-performance-selftest` exists or equivalent;
- 30/120/180-day balance sweeps prove no pathological survival death spiral under reference play;
- one-scenario “critical push” is tested through existing assignment rules, not invented special tactics;
- performance calculations are deterministic;
- performance projection cost is bounded;
- UI preview never mutates state;
- all three consumer seams have authority-identity tests;
- all speculative features from the source are dispositioned as REUSE / DEFER / DELETE FROM SCOPE.

---

# 4. Phase P0 — Authority & Semantics Audit

## P0.1 Capture repository baseline

Record:

```text
commit SHA
branch
dirty paths
NeedsSystem fields
need value ranges
need increase/decrease semantics
death/incapacitation thresholds
FitnessVerdict inputs
DutyRoster assignment rules
production throughput path
TacticalCombat calculation path
Expedition speed/stamina/carry path
SomaticFlashback work penalty consumer
TraumaBond work bonus consumer
current morale effect path
existing modifier interfaces
save sections
UI surfaces
```

## P0.2 Verify need semantics

Create table:

```text
need
field
range
0 means
max means
warning thresholds
death threshold
recovery action
```

Do not assume:
- “hunger 90” means hungry;
- “warmth 90” means cold.

## P0.3 Build performance authority matrix

Create:

`docs/survivors/NEEDS_PERFORMANCE_AUTHORITY_MATRIX.md`

Columns:

```text
performance fact
current authority
current modifier sources
needs consumer?
fitness consumer?
trauma consumer?
final owner
status
```

Rows:
- combat accuracy;
- combat damage;
- initiative/AP;
- work throughput;
- work duration;
- work quality if real;
- expedition speed;
- expedition stamina;
- carry capacity;
- assignment eligibility.

## P0.4 Audit Plan-24 overlap

Questions:

```text
Does FitnessVerdict already read needs?
Does DutyRoster already consume FitnessVerdict?
Does Combat consume FitnessVerdict?
Does Expedition consume FitnessVerdict?
Is FitnessVerdict binary or graded?
```

Use findings to avoid duplicate penalties.

## P0.5 Audit SomaticFlashback / TraumaBond

Determine:
- whether they are live;
- where applied;
- whether one is dead prose/comment only.

Do not preserve unwired modifiers as a second performance system.

## P0.6 Baseline performance tests

Before wiring:
- survivor at severe hunger;
- compare combat/work/expedition to healthy.

Capture current no-effect evidence.

---

# TASK 137A — Pure Needs Performance Projection

# 137A.0 Goal

Create one engine-free, deterministic projection of current needs into domain-specific performance multipliers.

## 137A.1 Prefer existing authority if present

If Plan-24 or another system already exposes graded throughput:
- extend that.

Only create `NeedsPerformanceBridge` when it fills a real missing seam.

## 137A.2 Proposed namespace

If new:

`Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs`

## 137A.3 Projection DTO

Prefer named nested structures:

```text
NeedsPerformanceModifiers
  Combat
    accuracy_multiplier
    damage_multiplier
    initiative_multiplier
  Work
    speed_multiplier
    output_multiplier
  Expedition
    speed_multiplier
    stamina_multiplier
    carry_multiplier
  overall_band
  contributions[]
```

Do not add fields consumers cannot use.

## 137A.4 Explainability contribution

Each contribution:

```text
source
need
normalized_value
tier
multiplier
reason_key
```

## 137A.5 Thresholds live in data/config

Create/extend:

`needs_performance.json`

Fields per need/domain.

Do not bury the source plan's thresholds directly in C#.

## 137A.6 Threshold semantic normalization

Use a canonical "deprivation severity" 0..1 internally if useful.

Example:
- hunger severity;
- thirst severity;
- fatigue severity;
- cold severity.

Projection adapter converts from actual NeedsSystem fields.

## 137A.7 Why normalize

Prevents confusion where:
- warmth high = good;
- hunger high = bad
or vice versa.

## 137A.8 Validation

Thresholds:
- monotonic;
- 0..1;
- multipliers 0..1.5 bounded;
- minimum <= maximum.

## 137A.9 Initial source thresholds

Treat source values as candidate balance defaults, not immutable truth.

Start with the source's intent:
- moderate penalties;
- severe needs can reduce output ~50%;
- extreme work penalty may be stronger.

But re-express after real field semantics.

## 137A.10 Multiplicative composition

Within needs:

```text
final = hunger * thirst * fatigue * warmth
```

only after testing.

## 137A.11 Floor

Set domain floor.

Reason:
- multiplicative stacking can collapse toward zero rapidly.

Use data.

## 137A.12 Alternative composition review

Test:
- product;
- worst-of;
- weighted product.

Choose based on death-spiral balance.

Document ADR.

## 137A.13 Morale interaction

Source proposes +10% penalty.

Before implementing:
- check Plan-24 morale already affects output.

If yes:
- do not double count.

If no:
- model as separate bounded contribution.

## 137A.14 Hygiene

Source does not propose performance impact.

Do not add unless existing design says so.

## 137A.15 Critical threshold

If needs already trigger:
- incapacitation;
- death;
- fitness-unfit,
then throughput floor follows that authority.

No contradictory 0.3 “last effort” branch.

## 137A.16 Healthy neutral

All normal needs:
- 1.0.

## 137A.17 Recovery

Same pure query returns higher multiplier immediately after needs improve.

## 137A.18 No tick

No scheduled update.

## 137A.19 No save

No modifier state.

## 137A.20 Interfaces

Only add interfaces if consumer seams benefit:

```text
ICombatPerformanceModifier
IWorkEfficiencyModifier
IExpeditionPerformanceModifier
```

Avoid interface proliferation if one `ISurvivorPerformanceProjection` is cleaner.

## 137A.21 Recommended interface

Potential:

```text
ISurvivorPerformanceProjection.Get(survivorId)
```

Consumers read their slice.

## 137A.22 Missing survivor

Typed result:
- neutral only if caller contract allows;
- otherwise fail with diagnostic.

Do not silently hide invalid IDs.

## 137A.23 Dead survivor

Should not be queried for active performance.

Diagnostic or blocked assignment.

## 137A.24 Incapacitated survivor

Fitness authority decides availability.

## 137A.25 Pure-function tests

For every need and tier.

## 137A.26 Combination tests

Two, three, four severe needs.

## 137A.27 Floor tests

No multiplier below configured floor.

## 137A.28 Monotonic tests

Worse deprivation:
- never improves multiplier.

## 137A.29 Recovery tests

Restore need:
- multiplier returns.

## 137A.30 Determinism test

Same state:
- bit/stable value.

## 137A.31 Data-integrity selftest

Thresholds and multipliers validate.

## 137A.32 Generated matrix

Create:

`docs/survivors/NEEDS_PERFORMANCE_MATRIX.md`

Generated from data.

### 137A DoD

There is one pure, explainable, bounded needs-to-performance projection and no duplicate survivor-condition authority.

---

# TASK 137B — Combat, Work & Expedition Consumption

# 137B.0 Goal

Make the three real systems consume the same projection at their authoritative calculation seams.

---

# 137B-C — Combat Integration

## 137B.C1 Find final combat calculation seam

Identify:
- hit/accuracy;
- damage;
- initiative/AP;
- stamina/action budget.

## 137B.C2 Apply only supported dimensions

If TacticalCombatSystem has:
- accuracy;
- damage;
but no initiative,
do not invent initiative system.

## 137B.C3 Accuracy

Apply combat accuracy multiplier at final pre-clamp point.

## 137B.C4 Damage

Apply damage multiplier only if design wants needs to affect raw damage.

Consider:
- accuracy only;
- action budget only;
to avoid over-punishing firearms.

Use telemetry.

## 137B.C5 Fatigue initiative

Only if initiative exists.

## 137B.C6 Cold action stamina

Only if combat action stamina exists.

## 137B.C7 Composition order

Document:

```text
base
× skill
× weapon
× condition/fitness
× needs
× trauma/social if applicable
→ clamp
```

Avoid double source.

## 137B.C8 UI combat breakdown

Show:
- final penalty;
- top contributing need.

No long wall of math unless expanded tooltip.

## 137B.C9 AI parity

If survivors controlled by AI use same combat code:
- same projection.

## 137B.C10 Combat integration tests

Healthy vs:
- hungry;
- thirsty;
- fatigued;
- cold;
- combined.

## 137B.C11 No binary cliff

Check threshold transitions.

## 137B.C12 Combat balance

Severe but not terminal needs:
- degraded;
- still potentially usable.

---

# 137B-W — Work / Duty / Production

## 137B.W1 Find work-output authority

DutyRoster may assign.

Production/job system may actually calculate output.

Apply modifier at output authority, not assignment UI.

## 137B.W2 Assignment eligibility

Fitness remains authority.

Needs throughput does not override:
- unfit;
- incapacitated.

## 137B.W3 Work output

Apply multiplier to:
- produced quantity;
- progress;
- labor hours
according to existing architecture.

Prefer one dimension.

## 137B.W4 No double time/output penalty

Do not both:
- double shift time;
- halve output
unless intended.

## 137B.W5 Somatic flashback integration

Existing work penalty:
- compose in same final multiplier pipeline.

## 137B.W6 Trauma bond integration

If live work bonus:
- compose once.

## 137B.W7 Skill composition

Use existing skill multiplier.

No new resilience skill.

## 137B.W8 Duty UI

Show:
- expected efficiency;
- top cause.

## 137B.W9 Work quality

Source suggests defects/medical errors.

Only implement if existing quality/error system exists.

Otherwise DEFER.

## 137B.W10 Light vs heavy duty

Only if current job definitions have workload/effort class.

If not:
- DEFER.

## 137B.W11 Medical work

Do not inject random treatment error from needs unless medical system already has worker-quality seam.

## 137B.W12 Work tests

Healthy vs deprived:
- progress/output.

## 137B.W13 Production consistency

Same state/query:
- same output formula.

## 137B.W14 No resource creation exploit

Rounding cannot produce more output from low multiplier.

---

# 137B-E — Expedition Integration

## 137B.E1 Find expedition authority

Separate:
- travel duration;
- stamina;
- carrying;
- encounter readiness.

## 137B.E2 Travel speed

If route time is based on team/survivor speed:
- apply projection there.

## 137B.E3 Team aggregation

Define:

```text
slowest
average
weighted role
```

Use existing expedition model.

Do not invent one arbitrarily.

## 137B.E4 Stamina

If expedition has explicit stamina:
- apply.

If not:
- don't create a new stamina system merely for Plan 137.

## 137B.E5 Carry capacity

Source proposes cold reduction.

Only if carry capacity is survivor-based and has modifier seam.

## 137B.E6 Fatigue

Avoid double counting if fatigue already:
- increases travel time;
- affects fitness.

## 137B.E7 Expedition briefing

Show:
- team performance warning;
- slowest member/major penalty;
- projected time change if real.

## 137B.E8 Dispatch refusal

Do not block automatically solely from moderate needs.

Fitness/expedition eligibility remains authority.

## 137B.E9 Severe needs

May cause:
- explicit warning;
- worse projection.

## 137B.E10 Expedition role modifiers

Source suggests Scout/Carrier/Navigator.

Only use if role system already exists.

Otherwise DEFER.

## 137B.E11 Expedition tests

- healthy team;
- one fatigued member;
- all hungry;
- cold carrier if carry seam exists;
- recovery before departure.

## 137B.E12 Route preview parity

Map/dispatch preview and actual expedition use same multiplier.

### 137B DoD

Combat, work and expeditions consume the same needs projection at their real calculation seams with no duplicate penalty paths.

---

# TASK 137C — Presentation, Warnings & Optional Existing-System Integrations

# 137C.0 Goal

Make the cascade visible and integrate only those secondary consequences that already have a canonical system.

## 137C.1 Survivor detail

Show:
- overall performance band;
- combat;
- work;
- expedition.

## 137C.2 Breakdown tooltip

Example:

```text
Work efficiency: 0.68×
Fatigue: 0.80×
Hunger: 0.85×
Thirst: 1.00×
Cold: 1.00×
```

## 137C.3 Do not show misleading percent

Prefer:
- `68% efficiency`
or:
- `0.68×`
consistent project style.

## 137C.4 Warning threshold

Source says >60% needs penalty.

Interpret as:
- final multiplier below configurable threshold.

Do not tie to raw need percentage.

## 137C.5 Warning semantic

Example:
- "Performance severely impaired."

## 137C.6 Warning icon accessibility

Text + icon, no color only.

## 137C.7 Duty roster

Show expected output penalty before assignment.

## 137C.8 Expedition briefing

Show impact before dispatch.

## 137C.9 Combat UI

Show one compact status.

## 137C.10 Journal

Do not auto-log every low multiplier.

Only semantic events:
- critical impairment;
- recovered from critical impairment;
if project event vocabulary supports.

## 137C.11 Tutorial

Only if existing tutorial framework.

Add:
- first meaningful need-performance warning.

No new tutorial engine.

## 137C.12 Social friction

Source proposes hunger irritability.

Before wiring:
- inspect SurvivorRelationsSystem input ports.

If existing stress/grievance modifier accepts needs:
- bind data-driven effect.

Otherwise DEFER.

## 137C.13 Mental health

Source proposes need stress increases crisis probability.

Only if current MentalHealthCrisisSystem has an explicit stress input.

Do not create hidden probability inside bridge.

## 137C.14 ShelterDecor interaction

Source proposes "need penalties affect morale from decor."

Likely redundant/undesirable.

Default disposition:
- DO NOT IMPLEMENT unless current decor system already scales by morale/needs in documented design.

## 137C.15 Refusal/breaking point

If Plan 43C refusal system is live:
- extreme impairment may be one authored trigger.

Otherwise DEFER.

## 137C.16 Pushing Through

Do not create random morale event.

If existing event framework supports:
- task completed below severe efficiency threshold
as a narrative event,
it may be added later.

## 137C.17 Rally

Do not create temporary need-penalty suppression unless existing morale system explicitly defines it.

## 137C.18 Special combat tactics

Default DEFER:
- Desperate Strike;
- Adrenaline Rush;
- Cold Fury.

They invert survival pressure and create new combat verbs.

## 137C.19 Needs-based quest hooks

Default DEFER to content wave unless:
- current quest runtime has declarative predicates;
- they can be authored without new mechanics.

## 137C.20 Needs performance events

If implemented:
- semantic transitions only.

## 137C.21 Localization

All labels/reasons keyed.

## 137C.22 Reduced motion

Warnings do not flash/pulse.

## 137C.23 Text scaling

Breakdown remains readable.

## 137C.24 Snapshot states

- healthy;
- moderate impairment;
- severe impairment;
- multi-need impairment;
- recovered.

## 137C.25 One read model

All UI surfaces read same projection.

### 137C DoD

The player can see why performance is degraded before committing a survivor to combat, work or expedition, without introducing speculative side systems.

---

# TASK 137D — Persistence, Determinism, Balance & CI

# 137D.0 Goal

Prove the cascade is derivable, save-safe, deterministic, performant and not catastrophically self-reinforcing.

## 137D.1 Save principle

Modifiers are not saved.

## 137D.2 Old save

Existing needs state restores.

Projection computes immediately.

## 137D.3 Save round-trip

Healthy/deprived survivor:
- same needs;
- same modifiers after load.

## 137D.4 No new save section

Assert.

## 137D.5 Threshold config versioning

If data schema needs version:
- version catalog;
- no save migration.

## 137D.6 Determinism fingerprint

Same needs:
- same modifiers.

## 137D.7 Combat replay

Same state/seed:
- same combat outcome distribution.

Needs modifier itself adds no RNG.

## 137D.8 Work replay

Same state:
- same progress.

## 137D.9 Expedition replay

Same route/team:
- same projected/actual performance.

## 137D.10 Headless

All calculations work without UI.

## 137D.11 Invalid survivor ID

Selftest.

## 137D.12 Edge case all neutral

All 1.0.

## 137D.13 Edge case all severe

Bounded floor.

## 137D.14 Death threshold

Align with existing fitness/death semantics.

## 137D.15 Recovery edge

Eat/drink/rest/warm:
- improvement.

## 137D.16 Combination balance

Test:
- two moderate needs;
- one severe + two moderate;
- all severe.

## 137D.17 Death-spiral simulation

Scenario:

```text
food shortage
→ hungry workers
→ food production drops
→ shortage worsens
```

Measure whether recovery remains possible.

## 137D.18 Recovery pathway assertion

At least one plausible:
- ration;
- rest;
- reassign;
- reserve workers;
- trade/import
must break spiral.

## 137D.19 Reference 30-day run

Normal campaign.

## 137D.20 120-day run

Track:
- avg work multiplier;
- expedition speed;
- combat impairment;
- deaths.

## 137D.21 180-day stress run

Harsh but playable.

## 137D.22 Balance metrics

Track:

```text
median combat modifier
median work modifier
median expedition modifier
days below 0.75
days below 0.5
critical impairment events
production loss
route delay
combat hit-rate delta
recovery time
```

## 137D.23 Tuning order

If too punishing:
1. threshold frequency;
2. curve severity;
3. multiplicative composition;
4. floors;
before changing core needs consumption.

## 137D.24 No "feed best fighters only" exploit dominance

Resource scarcity should create tradeoff, not obvious permanent caste.

Telemetry:
- performance distribution by survivor.

## 137D.25 Compute cost

Projection may be queried frequently.

Benchmark:
- 1000 survivors/queries.

## 137D.26 Allocation budget

Prefer:
- struct/read-only result;
- no per-query heap churn.

## 137D.27 Caching

Default:
- no cache.

Only cache if profiling proves needed.

## 137D.28 Data-integrity selftest

Validate thresholds.

## 137D.29 Needs-performance selftest

Create:

```text
--needs-performance-selftest
```

## 137D.30 Selftest scenarios

At least:
- neutral;
- hunger;
- thirst;
- fatigue;
- cold;
- combined;
- combat;
- work;
- expedition;
- recovery;
- save/load parity.

## 137D.31 Source-scan authority gate

Detect:
- UI performance formulas;
- duplicate needs thresholds;
- second saved modifier state.

## 137D.32 Consumer identity tests

Combat/work/expedition use same projection instance/config.

## 137D.33 Warnings bounded

Critical warning:
- transition-based;
- not every frame/day.

## 137D.34 Regression gate

No performance cascade when feature/config disabled if backward comparison is useful.

## 137D.35 Docs

Create:
- `NEEDS_PERFORMANCE_ARCHITECTURE.md`;
- `NEEDS_PERFORMANCE_MATRIX.md`;
- `NEEDS_PERFORMANCE_BALANCE_REPORT.md`.

### 137D DoD

The cascade remains deterministic, derivable, performant, explainable and recoverable over long survival runs.

---

# 5. Modifier Composition Model

The most important architectural question is **where** each modifier composes.

Recommended conceptual pipeline:

```text
BASE DOMAIN PERFORMANCE
        │
        ├── skill / equipment
        ├── fitness eligibility
        ├── trauma / social modifier
        └── needs projection
        │
        ▼
FINAL DOMAIN VALUE
```

Fitness should answer:
- can this survivor perform?

Needs should answer:
- how effectively?

Do not let both answer the same question.

---

# 6. Needs Normalization Contract

For each need, define one deprivation severity:

```text
0.0 = no deprivation
1.0 = worst survivable deprivation
```

Adapters convert actual fields.

This prevents inverted-value bugs.

---

# 7. Candidate Balance Curves

The source proposes tiered thresholds.

Do not hardcode until semantics confirmed.

Possible data form:

```json
{
  "need": "fatigue",
  "tiers": [
    { "min": 0.0, "max": 0.4, "work": 1.0, "combat": 1.0, "expedition": 1.0 },
    { "min": 0.4, "max": 0.7, "work": 0.8, "combat": 0.9, "expedition": 0.9 },
    { "min": 0.7, "max": 0.9, "work": 0.6, "combat": 0.75, "expedition": 0.75 },
    { "min": 0.9, "max": 1.0, "work": 0.4, "combat": 0.5, "expedition": 0.5 }
  ]
}
```

Exact values are tuning data, not architecture.

---

# 8. Domain Multiplier Contract

## Combat
Possible:
- accuracy;
- damage;
- initiative/AP.

Only real dimensions.

## Work
Prefer:
- progress/output.

Avoid simultaneous time + output penalty.

## Expedition
Possible:
- travel speed;
- stamina;
- carry.

Only real dimensions.

---

# 9. No-Double-Counting Matrix

Create test matrix:

| Source | Combat | Work | Expedition |
|---|---:|---:|---:|
| Needs | yes | yes | yes |
| Fitness | eligibility / maybe graded | eligibility | eligibility |
| Somatic flashback | maybe | yes | maybe |
| Trauma bond | no? | yes | no? |
| Skill | yes | yes | yes |
| Morale | audit | audit | audit |

Every cell has one path.

---

# 10. Recovery Contract

Player actions should visibly improve performance:

```text
eat
drink
rest
warm up
medical care if relevant
```

The UI should update after the authoritative state changes.

---

# 11. Warning Contract

Use bands:

```text
Normal
Impaired
Severely Impaired
Unfit/Incapacitated
```

Prefer project-wide terminology.

---

# 12. Performance Read Model

Presentation DTO:

```text
survivor_id
overall_band
combat
work
expedition
top_contributors[]
eligibility
```

Eligibility may come from fitness.

---

# 13. Work Output Contract

Example:

```text
base progress
× skill
× tool/facility
× trauma/social
× needs
= actual progress
```

Use actual current order after audit.

---

# 14. Expedition Team Aggregation Contract

Do not invent until current system read.

Potential models:
- slowest traveler;
- average;
- leader weighted;
- role weighted.

Whichever exists remains authority.

---

# 15. Combat Fairness Contract

Needs can degrade combat.

Avoid:
- zero accuracy at moderate deprivation;
- simultaneous massive accuracy + damage + action loss
without evidence.

Prefer one or two meaningful dimensions.

---

# 16. Social/Mental-Health Scope Contract

Secondary systems may consume needs only if they already expose a typed input.

Otherwise:
- follow-on plan.

The bridge is not allowed to become a universal "bad things happen" coordinator.

---

# 17. Speculative Source Feature Disposition

Create:

`docs/survivors/NEEDS_PERFORMANCE_SCOPE_DISPOSITION.md`

Rows:

```text
Pushing Through
Breaking Point
Rally
Hungry Guard quest
Exhausted Expedition quest
Frozen Watch quest
Desperate Strike
Adrenaline Rush
Cold Fury
Light/Heavy duty
Scout/Carrier/Navigator role sensitivity
Need recovery cross-effects
Hunger irritability
Fatigue withdrawal
Cold theft
```

Status:
- REUSE EXISTING;
- DEFER;
- REJECT;
- IMPLEMENT.

---

# 18. Save/Persistence Matrix

| State | Persist? |
|---|---:|
| hunger/thirst/fatigue/warmth | existing NeedsSystem |
| morale | existing |
| needs performance modifiers | no |
| threshold config | data |
| warning state | derived / event history |
| combat current calculations | domain |
| work progress | domain |
| expedition progress | domain |

---

# 19. Failure Injection Matrix

## N137.1 Needs field semantics inverted
Expected: normalization tests fail.

## N137.2 Fatigue penalty applied in fitness and bridge
Expected: no-double-count test fails.

## N137.3 UI computes its own multiplier
Expected: source-scan gate fails.

## N137.4 Modifier state saved
Expected: architecture gate fails.

## N137.5 Severe hunger reduces output below configured floor
Expected: bounds test fails.

## N137.6 Recovery leaves stale penalty
Expected: pure-query test fails.

## N137.7 Combat applies work multiplier
Expected: domain-slice test fails.

## N137.8 Expedition preview differs from actual
Expected: parity test fails.

## N137.9 Hunger causes theft without social system seam
Expected: scope gate fails.

## N137.10 One 30-day shortage causes unrecoverable production collapse
Expected: death-spiral balance gate fails.

## N137.11 Same state produces different modifier
Expected: determinism test fails.

## N137.12 Rebinding UI duplicates warning event
Expected: subscription/event test fails.

---

# 20. Determinism Contract

Same:

```text
needs state
+ configuration
+ existing fitness/skill/trauma state
```

must yield same:

```text
combat slice
work slice
expedition slice
breakdown
```

The projection itself consumes no seed.

---

# 21. Long-Horizon Metrics

Track:

```text
mean/median needs severity
mean/median performance modifier by domain
days severely impaired
output lost to needs
expedition delays
combat accuracy delta
recovery time
food production feedback
death spiral incidents
```

---

# 22. Performance Budget

Pure projection target:
- O(1);
- no catalog parsing;
- no LINQ allocations in hot path if avoidable;
- no dictionary scans beyond small indexed lookup.

Benchmark 1,000–100,000 calls.

---

# 23. UI Acceptance

## Survivor Detail
- overall band;
- combat/work/expedition slices;
- top causes.

## Duty Roster
- expected efficiency before assignment.

## Expedition
- team performance before dispatch.

## Combat
- compact current impairment.

---

# 24. Accessibility

- text + icon;
- no color-only penalty;
- breakdown keyboard accessible;
- text scaling;
- no flashing warning.

---

# 25. Balance Guardrails

The cascade should make needs:
- strategically important;
- recoverable.

It should not:
- turn every shortage into inevitable collapse;
- make weak survivors permanently worthless;
- create obvious min-max starvation caste.

---

# 26. CI / Gate Set

Recommended:

```text
needs_performance_threshold_integrity
needs_performance_no_double_count
needs_performance_combat
needs_performance_work
needs_performance_expedition
needs_performance_preview_parity
needs_performance_determinism
needs_performance_balance
needs_performance_ui_access
```

---

# 27. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --needs-performance-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 28. Recommended Commit Breakdown

```text
137A-1 authority/semantics audit
137A-2 normalized deprivation model
137A-3 needs performance data/config
137A-4 pure projection + explanation trace
137A-5 no-double-count composition
137A-6 unit/bounds/recovery tests
137A-7 generated matrix/docs
137A-8 selftest

137B-1 combat integration
137B-2 combat parity/balance tests
137B-3 work-output integration
137B-4 trauma/social work modifier unification
137B-5 expedition integration
137B-6 route-preview parity
137B-7 cross-domain identity tests
137B-8 headless integration tests

137C-1 survivor-detail read model
137C-2 duty/expedition/combat UI
137C-3 warnings/localization/accessibility
137C-4 social/mental-health seam audit
137C-5 optional existing-system integrations
137C-6 speculative feature disposition
137C-7 snapshots
137C-8 docs

137D-1 old-save/derived-state tests
137D-2 determinism fingerprints
137D-3 30-day balance
137D-4 120-day balance
137D-5 180-day stress/death-spiral
137D-6 performance benchmark
137D-7 CI gates/failure fixtures
137D-8 final ship/no-ship report
```

---

# 29. Risk Register

## R137.1 Double counting with fitness

Mitigation:
- authority matrix;
- composition tests.

## R137.2 Multiplicative stacking becomes too harsh

Mitigation:
- floors;
- telemetry;
- alternative composition review.

## R137.3 Combat becomes frustrating

Mitigation:
- compact penalty set;
- clear preview;
- avoid stacking accuracy + damage + AP too aggressively.

## R137.4 Work creates irreversible starvation spiral

Mitigation:
- 30/120/180-day balance gates;
- recovery-path assertion.

## R137.5 Expedition UI disagrees with actual travel

Mitigation:
- one projection;
- route-preview parity test.

## R137.6 Source plan balloons scope

Mitigation:
- speculative feature disposition;
- no new tactic/social/quest system.

## R137.7 Pure query becomes hot-path allocation source

Mitigation:
- benchmark;
- struct/read-only result;
- no unnecessary allocation.

---

# 30. Acceptance Checklist

## P0

- [ ] NeedsSystem semantics verified
- [ ] thresholds verified
- [ ] death/incapacitation semantics verified
- [ ] FitnessVerdict overlap audited
- [ ] DutyRoster work-output seam audited
- [ ] TacticalCombat seam audited
- [ ] Expedition seam audited
- [ ] SomaticFlashback penalty audited
- [ ] TraumaBond bonus audited
- [ ] morale path audited
- [ ] performance authority matrix created
- [ ] baseline no-effect tests captured

## 137A

- [ ] existing projection reused if present
- [ ] bridge only if needed
- [ ] domain-specific DTO
- [ ] explainability contributions
- [ ] thresholds in data
- [ ] deprivation normalization
- [ ] threshold validation
- [ ] source thresholds treated as tunable
- [ ] multiplicative model tested
- [ ] domain floors
- [ ] alternative composition reviewed
- [ ] morale double-count audit
- [ ] hygiene not added casually
- [ ] critical threshold aligned to fitness
- [ ] neutral = 1.0
- [ ] recovery automatic
- [ ] no tick
- [ ] no save
- [ ] interfaces minimal
- [ ] invalid survivor handling
- [ ] dead/incapacitated handling
- [ ] pure tests
- [ ] combination tests
- [ ] floor tests
- [ ] monotonic tests
- [ ] recovery tests
- [ ] determinism
- [ ] data-integrity selftest
- [ ] generated matrix

## 137B — Combat

- [ ] final calculation seam found
- [ ] only real dimensions used
- [ ] accuracy integration
- [ ] damage integration justified
- [ ] initiative only if real
- [ ] stamina/AP only if real
- [ ] composition order documented
- [ ] compact UI breakdown
- [ ] AI parity
- [ ] healthy/deprived tests
- [ ] no binary cliff
- [ ] combat balance

## 137B — Work

- [ ] output authority found
- [ ] fitness remains eligibility authority
- [ ] one output/progress dimension
- [ ] no time+output double penalty
- [ ] SomaticFlashback composes once
- [ ] TraumaBond composes once if live
- [ ] skill composes once
- [ ] duty UI expected efficiency
- [ ] work quality deferred unless real
- [ ] workload class deferred unless real
- [ ] no invented medical-error system
- [ ] work tests
- [ ] deterministic output
- [ ] rounding exploit checked

## 137B — Expedition

- [ ] expedition authority found
- [ ] travel speed integration
- [ ] team aggregation uses existing model
- [ ] stamina only if real
- [ ] carry only if real
- [ ] no fatigue double count
- [ ] briefing warning
- [ ] no moderate-needs auto block
- [ ] severe needs warning
- [ ] roles only if existing
- [ ] expedition tests
- [ ] route preview parity

## 137C

- [ ] survivor detail overall band
- [ ] domain slices
- [ ] top contributor breakdown
- [ ] warning threshold uses final multiplier
- [ ] accessible warning
- [ ] duty preview
- [ ] expedition preview
- [ ] combat status
- [ ] no journal spam
- [ ] tutorial only if framework exists
- [ ] relations seam audited
- [ ] mental-health seam audited
- [ ] decor interaction rejected unless real
- [ ] refusal only if Plan 43C live
- [ ] Pushing Through disposition
- [ ] Rally disposition
- [ ] combat tactic disposition
- [ ] quest hook disposition
- [ ] events transition-based
- [ ] localization
- [ ] reduced-motion safe
- [ ] text scaling
- [ ] snapshots
- [ ] one read model

## 137D

- [ ] modifiers not persisted
- [ ] old saves compute correctly
- [ ] round-trip parity
- [ ] no new save section
- [ ] config versioning if needed
- [ ] deterministic fingerprint
- [ ] combat replay
- [ ] work replay
- [ ] expedition replay
- [ ] headless
- [ ] invalid ID test
- [ ] all-neutral test
- [ ] all-severe test
- [ ] death threshold semantics
- [ ] recovery edge
- [ ] combination balance
- [ ] starvation death-spiral simulation
- [ ] recovery pathway
- [ ] 30-day run
- [ ] 120-day run
- [ ] 180-day run
- [ ] balance metrics
- [ ] tuning order documented
- [ ] caste/minmax dominance checked
- [ ] projection benchmark
- [ ] allocation budget
- [ ] no premature cache
- [ ] data-integrity selftest
- [ ] needs-performance selftest
- [ ] source-scan authority gate
- [ ] consumer identity tests
- [ ] bounded warnings
- [ ] docs complete

---

# 31. Ship / No-Ship Gate

**SHIP** only if:

```text
needs_authorities == 1
AND performance_projection_authorities == 1
AND duplicate_need_penalty_paths == 0
AND ui_side_performance_formulas == 0
AND performance_modifier_save_sections == 0
AND healthy_modifier == 1.0
AND worse_needs_never_improve_performance == true
AND recovery_restores_modifier == true
AND combat_consumes_projection == true
AND work_consumes_projection == true
AND expedition_consumes_projection == true
AND combat_work_cross_slice_leaks == 0
AND expedition_preview_actual_drift == 0
AND retained_somatic_or_trauma_modifiers_unwired == 0
AND speculative_new_subsystems_added == 0
AND severe_combined_modifier_below_floor == false
AND reference_30_day_death_spiral == false
AND reference_120_day_balance == pass
AND harsh_180_day_recovery_possible == true
AND needs_performance_determinism == pass
AND needs_performance_headless == pass
AND needs_performance_selftest == pass
AND data_integrity_selftest == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 32. Implementer Handoff

1. Re-read `NeedsSystem` value semantics before copying a single threshold.
2. Audit Plan-24 fitness before creating a bridge.
3. Audit somatic-flashback and trauma-bond efficiency paths.
4. Decide one modifier-composition order and document it.
5. Keep the needs projection pure, derived, and unsaved.
6. Normalize deprivation semantics if field directions differ.
7. Put thresholds/curves in data.
8. Apply needs only at the authoritative combat/work/expedition calculation seams.
9. Let fitness decide eligibility; let needs decide effectiveness.
10. Do not double penalize fatigue through both fitness and throughput.
11. Keep combat penalties moderate and legible.
12. Apply one work penalty dimension rather than both time and output unless explicitly intended.
13. Ensure route/dispatch preview and actual expedition use the same calculation.
14. Surface one read model across survivor, duty, expedition and combat UI.
15. Treat warnings as semantic transitions, not spam.
16. Do not implement theft, mutations, special tactics, quests or role-resistance unless a real existing system seam already supports them.
17. Run starvation/production feedback simulations before finalizing curves.
18. Tune the performance curve before changing core needs rates.
19. Benchmark the pure projection; avoid hot-path allocations.
20. Prove old-save compatibility simply by restoring needs and recomputing.
21. Close only when combat, work and expedition all use the same projection and the long-horizon balance remains recoverable.

---

# 33. Final Outcome

When this plan is complete, needs stop being passive meters that matter only at the edge of death.

A hungry survivor still exists in the same `NeedsSystem`, but now the rest of the game reacts to that fact. Combat accuracy or endurance degrades through the real combat calculation. Work slows through the real production path. Expedition travel and stamina worsen through the real expedition authority.

The player sees the consequence before committing the survivor. A duty slot can show reduced expected output. An expedition can warn that one exhausted member will slow the team. Combat can display a compact impairment reason. Resting, feeding, hydrating, or warming the survivor immediately improves the projection because nothing extra was saved or cached.

The implementation also preserves the rest of ASHFALL's survivor architecture. Fitness still decides whether someone can perform a job at all. Trauma and social modifiers compose once. Skill remains skill. The needs bridge does not become a universal "bad things happen" engine.

Most importantly, the pressure remains recoverable.

A food shortage can reduce labor and create real strategic pain, but the long-horizon tests prevent that feedback from becoming an automatic death spiral. The curve is bounded, explainable, deterministic, and tunable from data.

The result is exactly what a survival-management game needs:

the condition of the people living through the crisis finally changes what they can actually do.
