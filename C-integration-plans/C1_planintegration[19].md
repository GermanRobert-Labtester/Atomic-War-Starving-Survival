# C1 — Flagship Integration Plan [19]: Medicine Made Legible — Disease, Treatment, Dependency, Palliative Care & Medical Continuity

> **Output:** `C1_planintegration[19].md`
>
> **Source baseline:** Plan 60 — Medicine Made Legible: Plan 09 Integrated & Re-Baselined
>
> **Wave:** Continuity / Medical Integration — repository reality first
>
> **Origin:** flagship Plan 09, re-baselined against the current repository and regrouped into Tasks 60A–60F.
>
> **Primary mission:** make medicine a longitudinal management problem — diagnosed, monitored, treated, contained, sometimes endured — **without creating parallel medical systems**.
>
> **Current verified reality:** the disease catalog already holds 15 diseases across all four vectors; the remaining problem is not disease-count expansion but **legibility, causality, treatment binding, dependency care, palliative reachability, persistence, and proof**.
>
> **Mandatory implementation principle:** no authoring before the field or runtime seam that represents it exists; no completion claim without a command/test that proves it.
>
> **Guardrails:** no second disease runtime, no second affliction model, no new symptom authority, no second detox scheduler, no second palliative state machine, no second ward-triage system, no second grief model, no parallel pharma economy, no invented vector, no fantasy pathology, no medical UI recomputing Core truth, no wall-clock/GUID/unordered-iteration dependency in clinical progression.

---

# 0. Mission

ASHFALL already contains most of the mechanics required for a deep medical game:

- 15 diseases;
- water / air / blood / spore vectors;
- incubation and illness durations;
- infectivity and lethality;
- world outbreak sources;
- a disease runtime and persisted infection state;
- a sick list;
- medical ward categories;
- isolation flags;
- 25 pharma recipes;
- chemical dependency;
- stress/relapse APIs;
- palliative-plan state;
- a vigil state machine;
- memorial quality/outcomes;
- grief hooks;
- final wishes;
- medical UI surfaces;
- content-utilization and selftest infrastructure.

The historical Plan 09 assumed the project lacked content.

That assumption is now false.

The real failure modes are integration failures:

```text
authored clinical text
→ no reader

stress relapse API
→ no producer
→ no subscriber

vector prevention protocol
→ callable
→ no UI path
→ no expiry

vigil state machine
→ host-wired
→ no player surface / formerly frame-time tick

memorial grief sink
→ interface exists
→ not injected / not bound

sick list band
→ dose band
→ disease severity ambiguity

treatment taxonomy
→ absent from old schema
→ impossible to author honestly
```

The source also reports meaningful progress already landed:
- D1 derived clinical staging;
- D5 illness → sick-list bridge;
- D7 grief chain;
- treatment hook and disease schema v2;
- treatment arrays;
- diagnostic tell/timing fields;
- ward clinical note / GIVE action / KEEP VIGIL;
- vigil care recorded on consequence ledger.

Therefore this flagship plan is not a reset.

It is a **continuation plan that preserves completed work and attacks the remaining gaps in causal order**.

Target architecture:

```text
WORLD STATE
   │
   ▼
OUTBREAK SOURCE
   │
   ▼
EXPOSURE
   │
   ▼
DISEASE SYSTEM
   │
   ├── derived phase
   ├── severity / trend
   ├── diagnosis state
   ├── treatment window
   └── isolation state
   │
   ▼
SICK LIST / MEDICAL PANEL
   │
   ├── tell
   ├── timing clue
   ├── urgency
   ├── prognosis
   └── available action
   │
   ▼
WARD / PHARMA / CARE
   │
   ├── treatment role
   ├── dose
   ├── quarantine
   ├── dependency protocol
   └── palliative plan
   │
   ▼
OUTCOME
   │
   ├── recovery
   ├── chronic consequence
   ├── relapse
   ├── vigil
   └── death
   │
   ▼
GRIEF / MEMORIAL / JOURNAL / CODEX
```

Every edge must be real.

---

# 1. Re-Baseline Before Editing

## 1.1 Verify current status, do not trust historical assumptions

Re-read:
- `DiseaseCatalog.cs`;
- `DiseaseSystem.cs`;
- `DiseaseTriage.cs`;
- `SickListSystem.cs`;
- `MedicalWardSystem.cs`;
- `MedicalHostSession.cs`;
- `VigilStateMachine.cs`;
- `ChemicalDependencySystem.cs`;
- `StressRelapseRules.cs`;
- `MemorialSystem.cs`;
- `RelationsGriefSink.cs`;
- current disease JSON;
- pharma recipes;
- dependency items;
- final wishes;
- current UI surfaces.

## 1.2 Capture current counts

Record:

```text
disease_count
vector_counts
treatment_defs
pharma_recipe_count
dependency_item_count
outbreak_source_count
medical UI surfaces
medical save sections
medical selftest counts
disease selftest counts
medical xUnit count
```

## 1.3 Reconfirm already-landed decisions

Status table:

```text
D1 derived clinical staging      COMPLETE?
D2 tell delivery                 PARTIAL/COMPLETE?
D3 treatment taxonomy            COMPLETE?
D4 protocol expiry               OPEN?
D5 sick-list semantics bridge    COMPLETE?
D6 vigil surface                 COMPLETE?
D7 grief binding                 COMPLETE?
```

No duplicate implementation if status is already green.

## 1.4 Capture baseline commands

Run:
- `dotnet test`;
- `dotnet build Ashfall.csproj`;
- disease selftest;
- medical selftest;
- expansions selftest;
- day1 selftest;
- real campaign journey;
- 7-day smoke;
- triad drift;
- warning baseline.

Record current pass/fail and check counts.

---

# 2. Non-Negotiable Medical Invariants

## INV-60.1 — One disease runtime

`DiseaseSystem` remains infection authority.

## INV-60.2 — One clinical state projection

Phase, severity, trend and prognosis are derived from Core state.

Panels never recompute them independently.

## INV-60.3 — Prevention is not treatment

`countermeasure_item_id` or vector neutralisation state must not be repurposed as cure semantics.

## INV-60.4 — Treatment uses the existing disease state

Treatment modifies outcome through one narrow intervention hook.

No parallel treatment runtime.

## INV-60.5 — Diagnosis remains uncertain until earned

UI cannot name hidden pathogen simply because infection exists.

## INV-60.6 — Clinical text is authored but state-bound

Tell/guidance/source note appears only where state/knowledge allows.

## INV-60.7 — Outbreaks have named causes

Every outbreak traces to one `IDiseaseOutbreakSource` and one actual world authority.

## INV-60.8 — No second contamination authority

Existing filtration/contamination/hygiene state triggers disease.

Disease does not invent its own duplicate contamination state.

## INV-60.9 — Protocol state expires

Temporary prevention protocols cannot remain sticky forever unless explicitly authored.

## INV-60.10 — Ward routing is explicit

Severity/contagion maps to existing bed categories/isolation.

## INV-60.11 — Every pharma output has a purpose or disposition

No dead recipe output.

## INV-60.12 — No universal cure

Treatment roles remain distinct and bounded.

## INV-60.13 — One consume/effect path

Medical consumption uses the shared consume/inventory authority.

## INV-60.14 — Dependency uses existing stress/morale/trauma sources

No new stress accumulator.

## INV-60.15 — Detox protocol uses existing medical day tick

No second treatment scheduler.

## INV-60.16 — Dependency continues on expedition

Leaving shelter does not pause disease/dependency state.

## INV-60.17 — Vigil uses campaign time

No frame-time medical progression.

## INV-60.18 — Grief has one owner

Memorial death context feeds existing relations/morale authority exactly once.

## INV-60.19 — Good death is not a reward exploit

Comfort care can reduce grief burden; it cannot generate repeatable morale profit.

## INV-60.20 — Medical saves are idempotent

Reload never duplicates:
- dose;
- treatment;
- relapse;
- death;
- vigil;
- memorial;
- grief.

---

# 3. Definition of Done

Plan 60 is complete only when:

- the 15-disease catalog remains canonical;
- every disease has player-visible tell/timing/clinical guidance where appropriate;
- no authored clinical field remains unread without explicit disposition;
- sick list distinguishes dose-band state from disease severity;
- phase/trend/urgency/prognosis come from one Core read model;
- prevention actions are reachable;
- prevention protocols expire deterministically;
- outbreak sources are bounded, attributable and data-gated;
- radio warnings respect information availability;
- codex/knowledge unlocks occur from diagnosis/treatment/documentation, not globally;
- treatment roles are distinct;
- treatment windows matter;
- treatment changes disease outcome through one narrow hook;
- all 25 pharma recipe outputs have purpose or explicit deletion;
- ward categories are exercised by real disease content;
- isolation affects duty/contact through existing authority;
- dependency classes all have viable care paths;
- stress producers are bound;
- stress consumer subscription is exactly once;
- relapse evaluator is deterministic/data-backed;
- detox protocol persists;
- expedition withdrawal continues correctly;
- market demand is bounded and non-exploitative;
- vigil is reachable;
- palliative plan has an actual writer;
- final wishes are consumable by the real palliative/memorial path;
- grief binding is exactly once;
- memorial outcomes remain existing canonical states;
- ≥6 vigil vignettes render only state the engine actually holds;
- old saves load safely;
- all medical state round-trips;
- medical reachability scan is green;
- determinism fingerprints are stable;
- 120–180 day simulations avoid runaway epidemic / permanently saturated ward / impossible pharma demand;
- all medical surfaces pass accessibility and text-scale checks;
- regression matrix passes;
- all medical gates have owners and proof-of-failure fixtures.

---

# 4. Architecture Decisions & Status Lock

Create/update:

`docs/medical/ARCHITECTURE_DECISIONS.md`

## D1 — Clinical phase model

Preferred/current:
- derive from `incubation_days`;
- `illness_days`;
- `daysSick`.

No explicit parallel phase timeline unless impossible case proves need.

## D2 — Diagnostic tell

Use:
- `tell`;
- `tell_secondary`;
- `timing_clue`;
- guidance/source note.

No symptom subsystem.

## D3 — Treatment roles

Existing/new schema field:

```text
curative
suppressive
symptomatic
supportive
```

Each treatment entry binds disease ↔ item ↔ role.

## D4 — Protocol expiry

Still-open highest-priority prevention gap.

Define deterministic expiry semantics for:
- purified water;
- sealed vents;
- sterilized tools;
- air filtration;
and any other temporary protocol.

## D5 — Sick-list semantics

Dose band and disease severity are distinct named facts.

Never overload one `band` field with two meanings.

## D6 — Vigil surface

Reuse Medical / Caregiving / Sick List detail.

No new panel unless proven necessary.

## D7 — Grief binding

Use existing memorial + relations/morale authority.

No new grief system.

---

# 5. Reweighted Immediate Fixes

Before broad authoring, verify/finish these seams:

## 5.1 D4 protocol expiry

This is the largest still-open causal defect reported by the source.

## 5.2 Clinical note projection

The sick list must show disease clinical state, not only dose band.

## 5.3 Guidance/source-note delivery

No authored clinical text may remain dead.

## 5.4 Prevention action reachability

`PurifyWater`, `SealVents`, `SterilizeTools`, `SetAirFiltration` must be reachable from live gameplay where relevant.

## 5.5 Idempotent palliative/vigil/grief chain

Already partially/live according to source — verify and harden before adding vignettes.

---

# TASK 60A — Diagnostic Legibility

# 60A.0 Goal

A player should notice patterns, uncertainty, worsening and urgency before a hidden binary infection label becomes the only useful fact.

## 60A.1 Verify derived phase function

Use existing `DiseaseTriage` if already landed.

Do not create another.

## 60A.2 Phase boundary tests

For every disease:
- day 0;
- incubation end -1;
- incubation end;
- mid illness;
- late illness;
- recovery boundary.

## 60A.3 Trend model

Derive:
- emerging;
- worsening;
- stable/established;
- improving;
- terminal risk
from existing state/treatment projection.

No panel-local arrows.

## 60A.4 Prognosis read model

Expose:

```text
diagnosis_state
disease_display_if_known
phase
trend
urgency
isolation
days_sick
treatment_status
palliative_plan
tell_primary
tell_secondary
timing_clue
guidance
```

Only include facts current architecture can support.

## 60A.5 Disease identity gating

If undiagnosed:
- display syndrome/tell;
- not exact disease ID.

## 60A.6 Tell migration completeness

Every 15 disease definitions:
- tell;
- optional secondary;
- timing clue;
- localization key.

No half migration.

## 60A.7 Guidance/source-note audit

For every field:
- consumer;
- surface;
- or delete if invalid/duplicate.

## 60A.8 Sick-list bridge

Sick-list row shows:
- dose state separately;
- disease state separately.

## 60A.9 Disease severity source

Name source explicitly:

```text
DoseSeverity
ClinicalSeverity
```

or current equivalents.

Never ambiguous `band`.

## 60A.10 Survivor detail clinical block

Use same read model.

No duplicated prose formatting logic beyond presentation.

## 60A.11 Medical panel clinical note

Use same state.

## 60A.12 Feedback messages

Only emit semantic transitions:
- suspected infection;
- diagnosed;
- worsening;
- isolation;
- treatment response.

No daily spam.

## 60A.13 Concrete symptom language

Clinical:
- fever timing;
- sputum;
- wound odor;
- rash distribution;
- exposure timing.

Avoid exclusive "one disease = one magic symptom."

## 60A.14 Symptom overlap

At least some tells overlap plausibly.

Diagnosis must still require:
- test;
- knowledge;
- evidence.

## 60A.15 No hidden probability leak

Do not show exact lethality/infectivity percentages unless explicitly intended.

## 60A.16 Localization

All tells/guidance:
- keyed;
- text-scale safe.

## 60A.17 Generated matrices

Generate:
- `DISEASE_VECTOR_MATRIX.md`;
- `DIAGNOSTIC_TELL_MATRIX.md`;
- `MEDICAL_STATE_MODEL.md`;
- `SICK_LIST_CONTRACT.md`.

## 60A.18 No handwritten drift

Generator `--check`.

## 60A.19 Healthy snapshot

No clinical clutter.

## 60A.20 Early snapshot

Uncertain, symptom-led.

## 60A.21 Established snapshot

Known disease if diagnosed.

## 60A.22 Severe snapshot

Urgency and next action clear.

## 60A.23 Quarantine snapshot

Isolation explicit.

## 60A.24 Palliative snapshot

Prognosis/plan clear, restrained.

## 60A.25 UI authority test

Opening any clinical panel must not mutate state.

## 60A.26 Duplicate render path test

Same patient:
- one derived clinical state;
- all panels match.

## 60A.27 Acceptance evidence

All disease definitions:
- LOADED;
- REGISTERED;
- QUERIED;
- SELECTED.

Treatment-linked definitions later must reach effect.

### 60A DoD

Disease becomes readable without becoming omniscient.

---

# TASK 60B — Outbreak Causality, Prevention & Information

# 60B.0 Goal

Every outbreak must come from actual world state, remain bounded, and have reachable counterplay.

## 60B.1 Inventory current outbreak sources

At minimum verify:
- sump flooding;
- excavation.

## 60B.2 Source ID convention

Document:

```text
sump_flooding
excavation
water_treatment_failure
brine_contamination
caravan_arrival
crowding
...
```

Only real states.

## 60B.3 Source adapter contract

Each source provides:
- `SourceId`;
- exposed population/scope;
- reason key;
- vector/environment facts.

## 60B.4 Water treatment source

Only trigger if actual:
- dirty input;
- failed filtration;
- contamination.

## 60B.5 Brine source

Use real brine contamination state.

## 60B.6 Caravan source

Use actual arrival/travel state.

## 60B.7 Crowding source

Use real occupancy/bunk/schedule state.

## 60B.8 Blood/contact source

Only from real:
- open wound;
- contaminated tool;
- equipment reuse;
- hygiene state.

## 60B.9 No free-floating infection roll

Every environmental exposure has source evidence.

## 60B.10 Outbreak data

Create/extend data with:

```text
source_id
eligible_vectors
min_day
season/weather gates
cooldown_days
budget_weight
scope_rule
warning_key
mitigation_action
```

## 60B.11 Deterministic eligibility

Source eligibility pure from state.

## 60B.12 Cooldown

Persist or derive per source.

No spam.

## 60B.13 Simultaneous crisis budget

Bound:
- concurrent outbreaks;
- overlapping medical crises.

Do not make concurrency impossible.

## 60B.14 Event stages

Use semantic events:

```text
outbreak_detected
exposure_group_infected
outbreak_contained
outbreak_faded
```

## 60B.15 Reason attribution

Report:
- source;
- vector;
- affected group;
- mitigation.

## 60B.16 Respiratory coherence

Audit `RespiratoryDegenerationSystem`.

Define:
- shared exposure inputs;
- distinct consequences.

## 60B.17 No double respiratory penalty

Same poor-air cause cannot independently apply duplicate equivalent damage unless explicitly distinct.

## 60B.18 Spore differentiation

Use:
- excavation/damp provenance;
- timing;
- symptom pattern.

No fantasy mutation.

## 60B.19 Prevention actions reachability

Surface existing:
- PurifyWater;
- SealVents;
- SterilizeTools;
- SetAirFiltration.

## 60B.20 Prevention UI placement

Reuse:
- medical;
- shelter environmental;
- relevant control panel.

No new protocol console.

## 60B.21 Protocol state visibility

Show:
- active;
- expires;
- affected vector/source.

## 60B.22 D4 expiry contract

Define for each protocol:
- start day/time;
- expiry rule;
- reset trigger;
- renewal action.

## 60B.23 Water purification expiry

No permanent one-click purification.

Use actual treatment cycle/capacity.

## 60B.24 Vent sealing expiry/state

If seal is persistent physical state:
- it may remain until changed.

Do not force expiry if physics says persistent.

D4 is per protocol, not one blanket timer.

## 60B.25 Tool sterilization state

Scope:
- equipment batch / shift / procedure
according to actual system.

## 60B.26 Air filtration state

Depends on:
- power;
- filter;
- current running state.

## 60B.27 Reset callers

No zero-caller reset API after integration.

## 60B.28 Prevention event

Emit:
- protocol enabled;
- protocol lapsed;
- protocol failed
only on transition.

## 60B.29 Radio warnings

Warnings may state:
- exposure;
- symptoms;
- vector;
- region
only if source knows.

## 60B.30 No undiagnosed pathogen name

Radio cannot name exact disease without diagnosis/intelligence.

## 60B.31 Learned codex

Unlock from:
- diagnosis;
- treatment;
- document/archive;
- autopsy
where authored.

## 60B.32 Hidden probabilities stay hidden

Codex may contain qualitative caution.

## 60B.33 At least four outbreak templates

Use actual world systems.

## 60B.34 Mitigation must be real

Each template has an action the player can perform.

## 60B.35 Save active outbreak

Round-trip:
- source;
- cooldown;
- active state;
- exposed group.

## 60B.36 Flood + storm stress test

30-day run.

No avalanche.

## 60B.37 Source determinism test

Same seed/state:
- same outbreak trace.

### 60B DoD

Disease enters the shelter because something happened, and the player can identify the causal chain and available mitigation.

---

# TASK 60C — Pharma Purpose, Treatment & Ward Routing

# 60C.0 Goal

Make treatment a timed, scarce decision and make the 25 recipes clinically meaningful.

## 60C.1 Audit recipe outputs first

Generate table:

```text
recipe
output item
current consumer
disease role
dependency role
palliative role
unused
```

## 60C.2 Reuse before authoring

No new drug until existing outputs are audited.

## 60C.3 Verify treatment schema v2

Current source reports:
- `treatments[]`;
- tell fields;
- TryTreat live.

Confirm actual schema.

## 60C.4 Treatment entry contract

Fields may include:

```text
item_id
role
treatable_days
response
late_response
```

Use current actual structure.

## 60C.5 Prevention vs treatment

Do not conflate `countermeasure_item_id` with treatment.

## 60C.6 Treatment intervention hook

Use current `DiseaseSystem.TryTreat`.

No second outcome engine.

## 60C.7 Outcome projection

Treatment should alter:
- recovery timing;
- severity;
- mortality risk;
- symptoms/support
according to role.

## 60C.8 Deterministic treatment math

No hidden wall-clock.

Seed only where probabilistic outcome is intended.

## 60C.9 Curative role

Rare/bounded.

No universal cure.

## 60C.10 Suppressive role

Reduces progression/risk, not instant erase.

## 60C.11 Symptomatic role

Improves symptoms/needs, not infection itself.

## 60C.12 Supportive role

Supports hydration/pain/comfort/respiration through existing systems.

## 60C.13 Treatable window

Use `daysSick`.

No new scheduler.

## 60C.14 Early response

Meaningfully better when authored.

## 60C.15 Late response

Partial/supportive.

## 60C.16 Untreated path

Disease retains authored natural history.

## 60C.17 Chronic/terminal path

Where supported:
- palliative or long-term care;
- no private health flag.

## 60C.18 Role distribution gate

No single output dominates excessive share.

## 60C.19 No universal treatment item

Assert no one item solves most catalog unless explicitly intended.

## 60C.20 Ward triage matrix

Generate:

`WARD_TRIAGE_CONTRACT.md`

Map:
- clinical severity;
- contagion;
- isolation need;
- bed category.

## 60C.21 Existing bed classes only

Do not add a class to make matrix pretty.

## 60C.22 Exercise every retained class

If a class never used:
- fix content;
- or document/delete implication.

## 60C.23 Admission refusal

If no bed:
- typed refusal;
- visible consequence.

No silent drop.

## 60C.24 No hidden admission queue loss

If queue exists, the patient remains explicit.

## 60C.25 Isolation → duty

Quarantined survivor:
- not scheduled to shared shift if rules prohibit.

Use Plan 24 fitness/duty authority.

## 60C.26 Isolation → bunk/contact

Use existing schedule/contact rules.

## 60C.27 Medical consumption

Every dose uses shared consume path.

## 60C.28 Exactly-once dose

Repeated click/stale UI:
- one consumption;
- one treatment effect.

## 60C.29 Treatment event

Emit:
- item;
- role;
- patient;
- result;
- day.

## 60C.30 Resource audit

Track:
- antimicrobials;
- analgesics;
- sedatives;
- diagnostics;
- ward supplies;
- dependency support.

## 60C.31 120-day demand vs supply

Compare pharma throughput.

## 60C.32 Impossible-demand assertion

Fail if normal intended play cannot supply critical class.

## 60C.33 Overabundance assertion

Flag treatment trivialization.

## 60C.34 Trade exploit assertion

No:
- treat → trade → treat infinite-value loop.

## 60C.35 Mid-treatment save

Reload:
- treatment status same;
- no redose.

## 60C.36 Generated docs

Generate:
- `DISEASE_TREATMENT_MATRIX.md`;
- `WARD_TRIAGE_CONTRACT.md`;
- `MEDICAL_REWARD_RESOURCE_AUDIT.md`.

### 60C DoD

Pharma outputs have distinct medical jobs, treatment timing matters, and ward routing is real.

---

# TASK 60D — Dependency as Managed Care

# 60D.0 Goal

Make dependency a coherent care/logistics problem across shelter and expedition.

## 60D.1 Enumerate actual dependency kinds

Read the enum/type.

Do not assume four.

## 60D.2 Generate class matrix

Fields:

```text
kind
exposure source
tolerance
withdrawal
craving
relapse
care options
ward impact
expedition impact
save fields
```

## 60D.3 Audit 13 dependency support items

Map each item to:
- kind;
- stage;
- role.

## 60D.4 Coverage gate

Every kind has at least one viable care path.

## 60D.5 Bind real stress producers

Use actual:
- guilt;
- witnessed death;
- ration conflict;
- combat trauma;
- sustained low morale;
- leadership/social friction
where authoritative.

## 60D.6 Named source IDs

Examples:

```text
guilt
witnessed_death
ration_conflict
combat_trauma
low_morale
```

## 60D.7 No duplicate stress accumulator

Call `ReportStress`.

## 60D.8 Bind `OnStressReported`

One subscriber path for:
- care/report/journal needs.

## 60D.9 Subscription identity

Rebind/open panels:
- no double subscriber.

## 60D.10 Move relapse table to data

Preserve semantics.

## 60D.11 Pure evaluator

`ComputeDelta` remains deterministic/pure.

## 60D.12 Fallback behavior

C# fallback may exist for missing data, but data is authority.

## 60D.13 Relapse is probabilistic/manageable

No guaranteed relapse from one stress event.

## 60D.14 Maintenance/taper reduce risk

Use real protocol state.

## 60D.15 Cold-turkey remains one option

Never the only default.

## 60D.16 Detox protocol state

Use smallest record if not already landed:

```text
survivorId
kind
stage
scheduledDoses
monitoringInterval
symptomThreshold
escalationRule
startedDay
```

## 60D.17 One medical day tick

Protocol advances there.

## 60D.18 Start test

Valid protocol starts.

## 60D.19 Advance test

Stages progress.

## 60D.20 Pause test

Medical interruption preserves state.

## 60D.21 Failure test

Threshold breach escalates.

## 60D.22 Relapse test

Returns to appropriate state.

## 60D.23 Complete test

No recurring dose after completion.

## 60D.24 Save/load protocol

No restart/double dose.

## 60D.25 Expedition continuity

Dependency state advances while deployed.

## 60D.26 Expedition preparation

Maintenance/taper supplies consume normally.

## 60D.27 No expedition-only dependency model

Same state.

## 60D.28 Trade demand

Map care medicine to current market system.

## 60D.29 Regional scarcity bounded

Avoid runaway medicine prices.

## 60D.30 Anti-incentive guard

Player cannot profit by inducing dependency.

## 60D.31 Backstories

Only after mechanics.

4–6 state-respecting origins if current narrative scope still calls for it.

## 60D.32 No personality modifier

Narrative origin does not become a permanent trait penalty.

## 60D.33 Non-stigmatizing language

Tone gate.

## 60D.34 120-day dependency simulation

Track:
- incidence;
- withdrawal;
- protocol success;
- relapse;
- supply.

## 60D.35 Negative relapse test

No single ordinary stress event guarantees relapse.

### 60D DoD

Dependency is managed through real medical care, real supply, real stress sources and the same state on expedition.

---

# TASK 60E — Palliative Care, Vigil, Grief & Remembrance

# 60E.0 Goal

Make terminal care one reachable, authoritative path from prognosis to remembrance.

## 60E.1 Verify current vigil surface

Source reports `KEEP VIGIL` is now live.

Re-read actual route.

## 60E.2 No new panel

Reuse existing medical/caregiving/sick-list surface.

## 60E.3 Vigil state contract

Document existing fields/events.

## 60E.4 Day-based time only

Confirm no remaining frame-time progression.

If `Tick(deltaSeconds)` remains:
- adapt to campaign/day time;
- preserve deterministic semantics.

## 60E.5 Palliative entry

Chain:

```text
prognosis
→ AssignPalliative(plan)
→ ward palliative category
→ vigil available
```

## 60E.6 Palliative writer

Ensure at least one real gameplay call writes the plan.

## 60E.7 Comfort-care actions

Reuse:
- analgesia;
- sedative;
- anti-dyspnoea;
- nausea control;
- presence;
- familiar object
where existing.

## 60E.8 Familiar object is not medicine

It is relation/narrative state.

## 60E.9 No peace currency

No `peace_points`.

## 60E.10 Final-wish consumer

`final_wishes.json` gets one real consumer.

## 60E.11 Wish states

Minimum if supported:

```text
unknown
known
fulfilled
partial
impossible
ignored
```

Reuse existing state if already landed.

## 60E.12 No duplicate wish store

Use current memorial/choice/history owner.

## 60E.13 Grief sink verification

Source reports D7 bound.

Confirm:
- injected sink;
- relations grief call;
- exactly-once.

## 60E.14 Death quality

Use existing:
- Peaceful;
- Rushed;
- Unattended;
- Sudden
or current enum.

## 60E.15 Existing grief multipliers

Do not invent new coefficients unless current code changed.

## 60E.16 Anti-reward rule

Comfort care may reduce grief burden.

It cannot create a repeatable morale gain.

## 60E.17 Ceiling test

No palliative action creates net exploitable morale farm.

## 60E.18 Vigil vignettes

Only after surface and deterministic clock are real.

Author 6–8 if not already present.

## 60E.19 Vignette predicates

Use:
- relationship;
- consciousness;
- comfort;
- final-wish state;
- presence.

## 60E.20 State fidelity

No line may assert:
- person present when absent;
- final wish fulfilled when not;
- consciousness when unconscious.

## 60E.21 Deterministic selection

Stable context key.

## 60E.22 Repeat safety

No same vignette spam.

## 60E.23 Localization

Keyed text.

## 60E.24 Memorial outcomes

Reuse existing enum.

## 60E.25 Outcome eligibility

Use:
- location;
- resources;
- final wish;
- relationship;
- body/recovery state
where existing.

## 60E.26 No resource generation

Memorial choice consumes or records.

## 60E.27 Memorial display

Use existing remembrance surface.

If absent:
- publish integration contract;
- do not build decor framework inside medicine.

## 60E.28 Eulogy/epitaph/heirloom continuity

Reuse Plan 41A.

## 60E.29 Vigil → consequence ledger

Verify kept vigil is recorded once.

## 60E.30 Death-quality derivation

Read that ledger.

## 60E.31 Full palliative chain test

```text
prognosis
→ palliative
→ vigil
→ death
→ grief
→ memorial
```

## 60E.32 Reload active vigil

No duplicate events.

## 60E.33 Reload after death

No duplicate grief/memorial.

## 60E.34 Tone test

Restrained, specific, non-sensational.

### 60E DoD

Terminal care is visible, deterministic, non-exploitative and connected to the people who survive.

---

# TASK 60F — Persistence, Reachability, Determinism & Long-Horizon Hardening

# 60F.0 Goal

Prove the medical layer stays coherent under old saves, long campaigns and repeated reloads.

## 60F.1 Persisted field inventory

Generate table:

```text
fact
authority
save section
field
version
default
migration
```

Include:
- disease;
- isolation;
- sick-list clinical state;
- palliative plan;
- treatment;
- dependency;
- protocol;
- vigil;
- memorial;
- grief marker if persisted.

## 60F.2 Old-save defaults

Schema v1 disease catalogs/saves remain loadable where applicable.

## 60F.3 Disease schema migration

Version 1 → 2:
- deterministic defaults;
- no missing required field crash.

## 60F.4 Round-trip matrix

At least 12:

1. old/pre-feature save;
2. active diagnosed disease;
3. active new-schema disease;
4. incubation;
5. mid-treatment;
6. dependency;
7. active taper;
8. relapsed;
9. palliative prognosis;
10. active vigil;
11. completed memorial;
12. repeated save/load.

## 60F.5 Idempotence

Assert:
- no rerolled phase;
- no duplicate dose;
- no restarted protocol;
- no refired death;
- no duplicate vigil event;
- no duplicate memorial;
- no double grief.

## 60F.6 Continuity audit

Create:

`MEDICAL_CONTINUITY_AUDIT.md`

Rows:
- outbreak source;
- contamination;
- infection;
- severity;
- diagnosis;
- isolation;
- treatment;
- dependency;
- protocol;
- palliative wish;
- vigil;
- grief;
- memorial.

One owner each.

## 60F.7 File-line evidence

Generate if possible.

Avoid handwritten stale citations.

## 60F.8 Content reachability

Medical families:

```text
disease triggerable
tell reachable
diagnosis reachable
treatment reachable
protocol reachable
vignette reachable
memorial outcome reachable
```

## 60F.9 Rare-state allowlist

Intentionally rare states:
- explicit;
- owner;
- reason.

## 60F.10 No orphan medical content

Fail otherwise.

## 60F.11 Determinism fingerprint — disease

Replay:

```text
exposure
→ incubation
→ phase
→ treatment
→ outcome
```

## 60F.12 Determinism fingerprint — dependency

Replay:

```text
dependency
→ withdrawal
→ protocol
→ stress
→ relapse/stabilization
```

## 60F.13 Determinism fingerprint — vigil

Replay state transitions.

## 60F.14 Forbidden nondeterminism scan

Medical progression cannot use:
- wall clock;
- `Guid.NewGuid`;
- unordered iteration;
- UI frame timing;
- platform enumeration.

## 60F.15 UI-preview safety

Opening medical UI:
- zero state mutation.

## 60F.16 Long-horizon seeds

Run:
- 120 days;
- 180 days;
- several seeds;
- baseline vs expanded.

## 60F.17 Track incidence

Per vector/disease.

## 60F.18 Track severity

Severe share.

## 60F.19 Track deaths

Medical deaths.

## 60F.20 Track pharma demand

By treatment class.

## 60F.21 Track ward occupancy

Median/p95/full days.

## 60F.22 Track dependency load

Active, withdrawal, protocol.

## 60F.23 Track palliative load

Active plans/vigils.

## 60F.24 Tune incidence before health model

If too lethal:
- adjust exposure/outbreak frequency first.

## 60F.25 Runaway epidemic assertion

Fail if contagion exceeds intended bound in normal reference scenario.

## 60F.26 Saturated ward assertion

Fail if normal scenario remains permanently full.

## 60F.27 Impossible pharma assertion

Fail if required treatment demand cannot be met under intended baseline.

## 60F.28 Trivial disease assertion

Fail if treatment abundance makes severe disease irrelevant.

## 60F.29 Accessibility regression

Seven surfaces:
- sick list;
- disease detail;
- clues;
- dependency;
- protocol;
- palliative;
- vigil.

## 60F.30 No color-only status

Critical state always text/icon.

## 60F.31 Text-scale overflow

Run enlarged scale snapshot.

## 60F.32 Terminal-state tone

Clear but not sensational.

## 60F.33 10-scenario regression set

1. water → diagnose → treat;
2. excavation → spore → respiratory;
3. wound → infection → ward;
4. dependency → taper → stress relapse → stable;
5. expedition withdrawal;
6. prognosis → vigil → wish → memorial;
7. attended vs unattended grief;
8. old save with active disease;
9. save during taper;
10. save during vigil.

## 60F.34 No duplicate authority assertion

Each scenario asserts authority identity.

## 60F.35 Gate registration

Register:
- medical reachability;
- determinism fingerprint;
- cross-version save matrix;
- long-horizon medical.

## 60F.36 Gate owners

Every gate:
- owner;
- tier;
- command;
- last-green.

## 60F.37 Self-proof

Each gate has a failing fixture.

## 60F.38 Full gate run

Run:
- disease selftest;
- medical/dependency selftests;
- data integrity;
- save-load UI failure;
- expansions;
- real campaign journey;
- dotnet tests;
- host build;
- verify-fast.

### 60F DoD

Medical continuity is measured by deterministic, cross-version, reachability and long-horizon evidence.

---

# 6. Cross-Task Dependency Graph

```text
current landed seams
      │
      ▼
D4 expiry + status recheck
      │
      ▼
60A legibility
  │        │
  │        └────────► 60B outbreak causality
  │
  ▼
60C treatment / ward
  │
  ├────────► 60D dependency care
  │
  └────────► 60E palliative / grief
                    │
                    ▼
                 60F hardening
```

Hardening steps 60F.1–60F.5 land continuously with each task.

---

# 7. Medical Authority Matrix

| Fact | Authority |
|---|---|
| infection | DiseaseSystem |
| clinical phase | DiseaseTriage/derived |
| diagnosis | SickList/diagnostic path |
| contamination | existing contamination systems |
| outbreak source | IDiseaseOutbreakSource |
| prevention protocol | Disease/host protocol state |
| treatment | DiseaseSystem.TryTreat |
| ward admission | MedicalWardSystem |
| isolation | ward/disease state |
| dependency | ChemicalDependencySystem |
| detox protocol | medical/dependency state |
| palliative plan | SickList/Caregiving |
| vigil | VigilStateMachine |
| final wish | existing wish/memorial state |
| grief | Memorial + relations/morale |
| memorial | MemorialSystem |

No second owner allowed.

---

# 8. Disease Read Model Contract

The UI reads one projection.

Suggested:

```text
survivor_id
diagnosis_state
clinical_label
phase
trend
urgency
isolation
palliative
treatment_actions
tell_primary
tell_secondary
timing_clue
guidance
```

---

# 9. Prevention vs Treatment Contract

```text
PREVENTION
→ blocks/reduces exposure/vector

TREATMENT
→ changes course after infection
```

No shared field may ambiguously do both.

---

# 10. Protocol Expiry Contract

For each prevention action define:

```text
state
authority
activation
duration/persistence
expiry/reset
renewal
UI visibility
save state
```

This closes the sticky-protocol defect.

---

# 11. Outbreak Source Contract

Every source must provide:

```text
source_id
authority
vector
scope
eligibility
cooldown
warning
mitigation
event attribution
```

---

# 12. Treatment Contract

Every treatment has:

```text
item
role
eligible disease
window
effect
late effect
consumption path
event
```

---

# 13. Ward Contract

Triage maps:

```text
clinical severity
+ contagiousness
+ palliative status
→ existing bed category
+ isolation
```

---

# 14. Dependency Contract

Dependency is not morale.

It has:
- class;
- state;
- withdrawal;
- care;
- stress-linked relapse;
- protocol;
- expedition continuity.

---

# 15. Palliative Contract

```text
prognosis
→ palliative plan
→ comfort actions
→ vigil
→ death quality
→ grief
→ memorial
```

One chain.

---

# 16. Medical Event Vocabulary

Use canonical events such as:

```text
disease_suspected
disease_diagnosed
outbreak_detected
outbreak_contained
treatment_administered
treatment_failed
isolation_started
dependency_stress
detox_started
relapse
palliative_started
vigil_started
vigil_completed
survivor_died
memorial_created
```

Only if event registry supports.

---

# 17. Persistence Matrix

| State | Persist? |
|---|---:|
| infection | yes |
| derived phase | no |
| diagnosis | yes |
| treatment state | yes |
| outbreak cooldown | yes/derived deterministically |
| prevention protocol | yes if not derivable |
| ward/isolation | yes |
| dependency | yes |
| detox protocol | yes |
| palliative plan | yes |
| vigil | yes |
| final wish state | yes |
| grief | existing authority |
| memorial | yes |

---

# 18. Failure Injection Matrix

## N60.1 Disease tell missing
Expected: integrity gate fails.

## N60.2 UI names undiagnosed pathogen
Expected: knowledge gate fails.

## N60.3 Prevention protocol never expires
Expected: D4 protocol test fails.

## N60.4 Water protocol resets incorrectly on load
Expected: round-trip fail.

## N60.5 Outbreak source has no real authority
Expected: source contract fails.

## N60.6 Respiratory penalty double-applies
Expected: coherence test fails.

## N60.7 Treatment item consumed twice on repeated click
Expected: stale-action/idempotence fail.

## N60.8 Ward full silently drops patient
Expected: admission test fails.

## N60.9 One cure treats most diseases
Expected: role-distribution gate fails.

## N60.10 Stress event guarantees relapse
Expected: dependency negative test fails.

## N60.11 Expedition pauses withdrawal
Expected: expedition continuity fails.

## N60.12 Vigil advances on frame time
Expected: determinism scan/test fails.

## N60.13 Death reload re-applies grief
Expected: idempotence fail.

## N60.14 Good-death loop farms morale
Expected: anti-reward test fails.

## N60.15 Old save cannot load schema v2
Expected: migration gate fails.

---

# 19. Determinism Contract

Same:

```text
seed
+ disease state
+ exposure state
+ treatment choices
+ dependency state
+ stress sequence
+ palliative choices
```

must produce same:
- phase;
- outbreak trace;
- treatment response;
- relapse trace;
- vigil events;
- death quality;
- grief application count.

---

# 20. UI Acceptance

## Sick List
Shows:
- dose vs clinical severity separately;
- trend;
- urgency;
- isolation;
- palliative.

## Survivor detail
Shows:
- tell/timing;
- diagnosis;
- treatment history/current action.

## Medical panel
Shows:
- clinical note;
- GIVE treatment with role;
- isolation;
- vigil.

## Dependency
Shows:
- state;
- withdrawal;
- care plan;
- protocol.

## Vigil
Shows:
- who;
- state;
- available action;
- final wish where known.

---

# 21. Accessibility

No:
- color-only severity;
- hidden status in tooltip;
- clipped symptom strings.

Support:
- text scale;
- keyboard;
- clear terminal state.

---

# 22. Tone Guardrails

Medicine should be:
- clinical;
- restrained;
- materially grounded.

Dependency:
- non-stigmatizing;
- no moral scoring.

Palliative:
- no melodrama;
- no "good death bonus."

---

# 23. Balance Metrics

Track:
- incidence;
- transmission;
- severe share;
- deaths;
- treatment consumption;
- ward occupancy;
- dependency prevalence;
- relapse;
- palliative load.

Tune:
- exposure;
- outbreak timing;
- resource availability
before rewriting base health rules.

---

# 24. Gate Set

Recommended named gates:

```text
medical_schema_migration
medical_tell_coverage
medical_protocol_expiry
outbreak_source_integrity
medical_treatment_matrix
medical_ward_routing
dependency_care_coverage
medical_reachability
medical_determinism
medical_save_matrix
medical_long_horizon
medical_ui_access
```

Use existing gate infrastructure.

---

# 25. Verification Commands

At each task and final:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --disease-selftest
godot --headless --path . -- --medical-selftest
godot --headless --path . -- --expansions-selftest
godot --headless --path . -- --day1-selftest
godot --headless --path . -- --real-campaign-journey-selftest
godot --headless --path . -- --7-day-smoke-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/warning-baseline-gate.sh
bash scripts/ci/verify-fast.sh
```

Add current medical-specific gates as they land.

---

# 26. Recommended Commit Breakdown

```text
60-0 current-status rebaseline + ADR lock
60A-1 clinical read model / sick-list semantics
60A-2 tell/guidance delivery
60A-3 uncertainty/trend/prognosis
60A-4 localization/snapshots/generated matrices

60B-1 source inventory / trigger data
60B-2 prevention action reachability
60B-3 D4 protocol expiry/reset
60B-4 water/brine/caravan/crowding sources
60B-5 respiratory/blood/spore coherence
60B-6 radio/codex/event budget
60B-7 outbreak save/stress tests

60C-1 pharma output audit
60C-2 treatment role/window verification
60C-3 ward routing/isolation
60C-4 consume/idempotence
60C-5 120-day resource audit
60C-6 generated treatment docs

60D-1 dependency class matrix
60D-2 stress producer/subscriber binding
60D-3 data relapse rules
60D-4 detox protocol
60D-5 expedition continuity
60D-6 trade/backstory/balance

60E-1 vigil surface/day-clock verification
60E-2 palliative writer/comfort care
60E-3 final wishes
60E-4 grief exact-once
60E-5 vigil vignettes
60E-6 memorial/eulogy continuity

60F-1 save inventory/migration
60F-2 12-case roundtrip matrix
60F-3 reachability
60F-4 determinism fingerprints
60F-5 long-horizon simulation
60F-6 UI/accessibility
60F-7 10-scenario regression
60F-8 gate registration/failure fixtures
```

---

# 27. Risk Register

## R60.1 Clinical legibility leaks hidden diagnosis

Mitigation:
- explicit knowledge gating.

## R60.2 Prevention semantics are mistaken for treatment

Mitigation:
- separate contract/schema.

## R60.3 Protocol expiry makes existing saves inconsistent

Mitigation:
- migration/defaults;
- explicit persistent-state rules.

## R60.4 Treatment makes disease trivial

Mitigation:
- roles/windows;
- no universal cure;
- long-horizon balance.

## R60.5 Ward routing destabilizes duty simulation

Mitigation:
- existing fitness/duty authority;
- integration tests.

## R60.6 Dependency becomes punitive morale mechanic

Mitigation:
- logistics/care framing;
- real relief;
- relapse negative tests.

## R60.7 Palliative care becomes exploitable morale source

Mitigation:
- anti-reward invariant.

## R60.8 Medical save schema becomes fragile

Mitigation:
- versioned defaults;
- 12-case cross-version matrix.

---

# 28. Acceptance Checklist

## Re-baseline

- [ ] current disease count verified
- [ ] vector counts verified
- [ ] current schema version verified
- [ ] current treatment hook verified
- [ ] D1–D7 status rechecked
- [ ] current vigil path rechecked
- [ ] current grief binding rechecked
- [ ] protocol expiry status rechecked
- [ ] baseline tests captured

## 60A

- [ ] one clinical phase derivation
- [ ] phase boundary tests
- [ ] trend projection
- [ ] prognosis read model
- [ ] diagnosis uncertainty preserved
- [ ] every disease has tell
- [ ] guidance/source_note disposition
- [ ] sick list dose vs disease semantics separated
- [ ] survivor detail clinical block
- [ ] medical panel clinical note
- [ ] feedback events bounded
- [ ] concrete symptom language
- [ ] plausible symptom overlap
- [ ] no hidden probability leak
- [ ] localization keys
- [ ] generated matrices
- [ ] generator drift check
- [ ] healthy snapshot
- [ ] early snapshot
- [ ] established snapshot
- [ ] severe snapshot
- [ ] quarantine snapshot
- [ ] palliative snapshot
- [ ] UI-preview safety
- [ ] no duplicate render state

## 60B

- [ ] source IDs documented
- [ ] live sump source verified
- [ ] live excavation source verified
- [ ] water source only from real state
- [ ] brine source only from real state
- [ ] caravan source only from real state
- [ ] crowding source only from real state
- [ ] blood/contact hooks grounded
- [ ] no free-floating infection roll
- [ ] outbreak data eligibility
- [ ] cooldown
- [ ] crisis budget
- [ ] event attribution
- [ ] respiratory no-double rule
- [ ] spore differentiation
- [ ] prevention actions reachable
- [ ] prevention state visible
- [ ] D4 per-protocol expiry
- [ ] water reset real
- [ ] vent semantics real
- [ ] sterilization semantics real
- [ ] filtration power/state real
- [ ] reset APIs have callers
- [ ] radio respects knowledge
- [ ] codex unlock path
- [ ] 4+ real outbreak templates
- [ ] each template has real mitigation
- [ ] outbreak round-trip
- [ ] flood+storm stress test
- [ ] deterministic outbreak trace

## 60C

- [ ] all 25 recipe outputs audited
- [ ] reuse-first rule
- [ ] treatment schema verified
- [ ] prevention/treatment separated
- [ ] TryTreat is sole treatment hook
- [ ] curative role bounded
- [ ] suppressive role distinct
- [ ] symptomatic role distinct
- [ ] supportive role distinct
- [ ] treatable window real
- [ ] early vs late response
- [ ] untreated natural history
- [ ] chronic/palliative handoff
- [ ] no universal cure
- [ ] ward triage matrix
- [ ] existing bed classes only
- [ ] each retained class exercised
- [ ] bed refusal visible
- [ ] no hidden queue loss
- [ ] isolation affects duty
- [ ] isolation affects contact/bunk
- [ ] one consume path
- [ ] exactly-once dose
- [ ] treatment event
- [ ] 120-day demand audit
- [ ] impossible demand assertion
- [ ] overabundance assertion
- [ ] no trade exploit
- [ ] mid-treatment save
- [ ] generated treatment docs

## 60D

- [ ] dependency kinds enumerated
- [ ] class matrix
- [ ] 13 items audited
- [ ] all classes have care
- [ ] guilt stress producer
- [ ] witnessed-death producer
- [ ] ration-conflict producer
- [ ] combat-trauma producer
- [ ] low-morale producer where valid
- [ ] named source IDs
- [ ] no new stress accumulator
- [ ] OnStressReported subscriber
- [ ] exactly-once subscription
- [ ] relapse data authority
- [ ] pure evaluator
- [ ] relapse manageable
- [ ] maintenance/taper lowers risk
- [ ] cold-turkey not sole/default
- [ ] protocol state
- [ ] one medical tick
- [ ] start/advance/pause/fail/relapse/complete tests
- [ ] protocol save/load
- [ ] expedition continuity
- [ ] normal expedition prep consumption
- [ ] no expedition-only model
- [ ] market demand bounded
- [ ] no dependency-profit incentive
- [ ] backstories only if relevant
- [ ] non-stigmatizing tone
- [ ] 120-day dependency balance
- [ ] no guaranteed-relapse negative test

## 60E

- [ ] vigil surface live
- [ ] no new panel
- [ ] vigil state documented
- [ ] campaign-time vigil
- [ ] palliative plan writer
- [ ] ward palliative route
- [ ] comfort-care actions use existing items
- [ ] familiar object not medicine
- [ ] no peace currency
- [ ] final wishes have consumer
- [ ] no duplicate wish store
- [ ] grief sink injected/bound
- [ ] grief exactly-once
- [ ] death quality uses existing model
- [ ] existing multipliers reused
- [ ] no morale farm
- [ ] 6–8 vignettes if still needed
- [ ] vignettes state-gated
- [ ] deterministic vignette selection
- [ ] localization
- [ ] memorial outcomes reused
- [ ] outcome eligibility real
- [ ] no resource generation
- [ ] remembrance surface or deferral contract
- [ ] eulogy/epitaph/heirloom continuity
- [ ] consequence ledger exact-once
- [ ] full palliative chain
- [ ] active-vigil reload
- [ ] post-death reload
- [ ] tone gate

## 60F

- [ ] persisted medical field inventory
- [ ] old-save defaults
- [ ] disease schema migration
- [ ] 12-case roundtrip
- [ ] no phase reroll
- [ ] no duplicate dose
- [ ] no protocol restart
- [ ] no refired death
- [ ] no duplicate vigil
- [ ] no duplicate memorial
- [ ] no double grief
- [ ] continuity audit
- [ ] one owner per fact
- [ ] generated evidence where possible
- [ ] medical reachability scan
- [ ] rare-state allowlist
- [ ] no orphan medical content
- [ ] disease determinism fingerprint
- [ ] dependency determinism fingerprint
- [ ] vigil determinism fingerprint
- [ ] forbidden nondeterminism scan
- [ ] UI-preview safety
- [ ] 120-day run
- [ ] 180-day run
- [ ] multi-seed comparison
- [ ] incidence metrics
- [ ] severity metrics
- [ ] death metrics
- [ ] pharma metrics
- [ ] ward metrics
- [ ] dependency metrics
- [ ] palliative metrics
- [ ] runaway epidemic assertion
- [ ] saturated ward assertion
- [ ] impossible pharma assertion
- [ ] trivial disease assertion
- [ ] all medical surfaces accessible
- [ ] enlarged text-scale snapshots
- [ ] 10-scenario regression
- [ ] authority identity assertions
- [ ] medical gates registered
- [ ] owners assigned
- [ ] failure fixtures
- [ ] full medical gate run

---

# 29. Ship / No-Ship Gate

**SHIP** only if:

```text
disease_runtimes == 1
AND symptom_diagnosis_authorities == 1
AND disease_catalog_count >= 15
AND diseases_missing_rendered_tell == 0
AND authored_clinical_fields_without_reader == 0
AND sick_list_dose_clinical_ambiguity == 0
AND prevention_actions_unreachable == 0
AND sticky_protocols_without_expiry_contract == 0
AND outbreak_sources_without_real_authority == 0
AND outbreak_templates_with_no_real_mitigation == 0
AND radio_knowledge_leaks == 0
AND treatment_hooks == 1
AND pharma_outputs_without_purpose == 0
AND universal_cure_detected == false
AND ward_categories_retained_but_unreachable == 0
AND silent_ward_admission_loss == 0
AND dependency_classes_without_care == 0
AND stress_report_real_producers >= 1
AND stress_report_real_subscribers >= 1
AND duplicate_stress_subscriptions == 0
AND detox_protocol_uses_second_scheduler == false
AND expedition_dependency_pause == false
AND vigil_reachable == true
AND vigil_uses_frame_time == false
AND palliative_plan_has_writer == true
AND grief_authorities == 1
AND duplicate_grief_on_reload == 0
AND morale_farming_via_palliative == false
AND old_save_migration == pass
AND medical_roundtrip_matrix == pass
AND medical_determinism == pass
AND medical_reachability == pass
AND medical_long_horizon == pass
AND medical_ui_access == pass
AND regression_10_scenarios == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 30. Implementer Handoff

1. Re-read the actual medical code before changing anything.
2. Preserve already-landed D1, D5, D7 and treatment/vigil work.
3. Finish D4 protocol reachability/expiry before adding more medical content.
4. Make authored clinical text readable before writing replacement prose.
5. Keep diagnosis uncertain until the existing diagnostic path earns certainty.
6. Keep prevention and treatment structurally separate.
7. Add outbreak sources only where real world-state authorities already exist.
8. Use cooldown and event budgets to prevent medical avalanche.
9. Route every treatment through `DiseaseSystem.TryTreat` and the shared consume path.
10. Audit all pharma outputs before adding a single new drug.
11. Make ward categories prove their use with catalog-driven tests.
12. Bind dependency stress to existing guilt/death/conflict/trauma state; do not create a stress meter.
13. Keep detox protocol inside the medical day tick.
14. Continue dependency progression on expedition.
15. Use real market hooks for care demand and add explicit anti-incentive tests.
16. Keep palliative care in the existing sick-list/ward/vigil path.
17. Verify final wishes and grief have real consumers exactly once.
18. Author vigils only after the reachable surface and deterministic clock are proven.
19. Treat cross-version save behavior and idempotence as feature work, not cleanup.
20. Run medical reachability and deterministic fingerprints continuously.
21. Tune outbreak incidence/exposure before weakening base health mechanics.
22. Close only when long-horizon simulations, old-save migration, UI accessibility and the 10-scenario medical matrix are green.

---

# 31. Final Outcome

When this plan is complete, medicine in ASHFALL stops being a set of hidden systems and becomes a management problem the player can actually reason about.

The player does not simply discover a red "infected" label. They see timing, clinical clues, worsening or improvement, isolation state, and uncertain diagnosis through one Core-derived read model. Authored guidance and clinical notes finally reach the surfaces they were written for.

Outbreaks stop feeling arbitrary. Flooding, filtration failure, excavation, crowding, wounds, caravans and other real world states become named exposure sources. Prevention protocols become reachable and stop being permanently sticky. Radio reports and codex entries respect what the shelter actually knows.

Pharma stops being a list of recipes. Each output has a role. Treatment windows matter. Early and late treatment differ. No universal cure trivializes the catalog. Ward categories and isolation finally receive real disease content, and bed scarcity becomes a visible survival tradeoff rather than a silent dropped admission.

Dependency becomes care rather than punishment. Stress already modeled by guilt, death, conflict and trauma reaches relapse risk through the existing dependency system. Detox protocols persist. Withdrawal continues while deployed. Supply and trade matter without incentivizing the player to create sickness.

Terminal care becomes part of play. Prognosis can move into a real palliative plan. A vigil can be kept through a live surface and campaign-time clock. Final wishes can matter. How somebody dies reaches the existing grief and memorial authorities exactly once. A peaceful death can reduce the burden on the living without becoming a morale reward farm.

Finally, the entire layer is proven across old saves, repeated reloads, deterministic traces, long campaigns, reachability scans and medical UI snapshots.

The result is not another medical subsystem.

It is the medical simulation ASHFALL already had finally becoming legible, causal, treatable, persistent, and humane.
