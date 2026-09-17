# C1 — Flagship Integration Plan [36]: Unified Emergency Alerts, Warning Coordination & Crisis Readiness

> **Output:** `C1_planintegration[36].md`
>
> **Source baseline:** Plan 194 — Emergency Alert & Warning System
>
> **Primary mission:** add a single, deterministic emergency-warning and crisis-coordination layer that consolidates hazard signals from existing domain systems, turns them into legible player-facing alerts with severity, urgency, affected scope, lead time, recommended actions and provenance, and routes preparation/response intents into the systems that already own work, evacuation, medicine, shelter defense, maintenance, containment and survivor behavior.
>
> **Primary architectural rule:** `EmergencyAlertSystem` owns **alert records, alert lifecycle, severity/priority projection, acknowledgment metadata, presentation eligibility, deduplication, escalation policy, alert history, and recommended response descriptors**. It does **not** own the underlying hazard, raid, disease, radiation, fire, flood, component failure, medical condition, evacuation movement, duty assignment, survivor panic, repair success, combat resolution, quarantine, or resource consumption.
>
> **Primary correction to the source plan:** the source proposes `AlertResponse` and `EvacuationProtocol` DTOs with survivor assignments, supplies, timers, success chance and response execution. Those risk duplicating Plan 158 disaster response, duty assignment, shelter movement, medical response, repair, defense and evacuation authority. The flagship therefore makes alerts produce **typed response recommendations and canonical command links**. If a reusable disaster-response/protocol authority from Plan 158 exists, predefined protocols belong there and `EmergencyAlertSystem` only references/activates them through an adapter.
>
> **Primary detection rule:** the alert layer should not rediscover hazards by polling or inventing its own probabilistic detection. Existing systems detect/forecast their own domain events and emit typed warning observations. Alert creation from a deterministic observation requires no RNG. Seeded RNG belongs only inside the canonical forecasting/detection system if that system already models uncertainty.
>
> **Primary usability rule:** acknowledgment is a player-interface fact, not proof that the crisis stopped worsening. Clicking an alert must never freeze radiation, fire growth, disease spread, flooding or raid arrival. Severity/priority follows the hazard's real state and time-to-impact, not whether the player clicked the notification.
>
> **Mandatory execution order:** 194A hazard/notification/disaster-response authority audit → 194B warning observation contract and alert registry → 194C severity/urgency/priority and deduplication → 194D forecast vs active-emergency semantics → 194E notification/UI/audio/map accessibility → 194F acknowledgment/escalation/history → 194G canonical response/protocol adapters → 194H persistence/migration/determinism/performance → 194I long-horizon alert-fatigue and multi-crisis simulations → 194J advanced false alarms/inter-settlement warnings only after the base architecture proves itself.
>
> **Critical re-baseline rule:** before creating `EmergencyAlertSystem.cs`, inspect `WeatherSystem`, `ShelterFireHazardSystem`, `DiseaseSystem`, `SumpFloodingSystem`, `RadiationSystem`, raid/defense authority, breach/structural systems, power/air/water maintenance systems, Plan-158 disaster response, Plan-186 shelter maintenance, Plan-135 weather cascade/warnings, DutyRoster/availability, evacuation/movement, medical/quarantine, survivor mental-health/autonomy, map/zone topology, semantic event bus, notification UI, audio settings, accessibility settings, campaign clock and save orchestration.
>
> **Guardrails:** no second hazard state; no second raid scheduler; no second disease outbreak state; no second fire/flood/radiation model; no alert-owned survivor assignment; no alert-owned resource consumption; no alert-owned response success chance; no alert-owned evacuation movement; no acknowledgment stopping hazard escalation; no automatic panic mutation from “broadcast”; no forced audio-only warning; no color-only severity; no arbitrary last-50 history truncation if archive/log policy already exists; no per-frame polling of every system; no duplicate alert for the same incident; no stale alert that remains critical after the source incident resolves; no low-supply notification promoted to emergency unless a real failure threshold is crossed; no seeded RNG inside alert creation when source observation is already known; no UI modal that prevents action during critical time unless the game intentionally pauses; no narrative/journal event for every warning.

---

# 0. Mission

ASHFALL already knows how to detect several dangerous conditions, but those warnings are fragmented.

The source baseline identifies:
- `WeatherSystem` detecting radiation storms;
- `ShelterFireHazardSystem` detecting fire;
- `DiseaseSystem` detecting outbreaks;
- `SumpFloodingSystem` detecting flooding;
- `RadiationSystem` detecting high radiation;
- Plan 186 shelter-maintenance warnings;
- no unified alert hierarchy;
- no centralized broadcast;
- no unified prioritization;
- no shared alert history;
- no central acknowledgment/readiness surface.

The current shape is approximately:

```text
WeatherSystem ───────────► its own warning
FireSystem ──────────────► its own warning
DiseaseSystem ───────────► its own warning
FloodSystem ─────────────► its own warning
RadiationSystem ─────────► its own warning
Maintenance ─────────────► its own warning
Raid system ─────────────► its own warning
```

The target is:

```text
CANONICAL DOMAIN SYSTEMS
       │
       ├── weather forecast
       ├── fire incident
       ├── disease outbreak
       ├── flood state
       ├── radiation hazard
       ├── raid/threat
       ├── maintenance failure
       ├── structural breach
       └── medical emergency
       │
       ▼
EmergencyWarningObservation
       │
       ▼
EmergencyAlertSystem
       │
       ├── stable alert identity
       ├── source incident identity
       ├── severity
       ├── urgency / time-to-impact
       ├── affected scope
       ├── confidence / forecast state
       ├── priority
       ├── recommended actions
       ├── acknowledgment
       ├── presentation state
       └── history
       │
       ▼
PLAYER / SURVIVOR PRESENTATION
       │
       ├── alert stack
       ├── audio/haptic cue
       ├── map/zone highlight
       ├── shelter panel
       └── direct action links
       │
       ▼
CANONICAL RESPONSE SYSTEMS
       ├── Plan 158 disaster protocols
       ├── DutyRoster
       ├── evacuation/movement
       ├── medical/quarantine
       ├── fire suppression
       ├── maintenance/repair
       ├── shelter defense
       ├── shielding
       └── resource allocation
```

The alert system should answer:

> What known or forecast danger exists, how urgent is it, what does it affect, what should the player consider doing, has the warning been seen, and which real gameplay surface can respond?

It should not answer:

> Did the fire spread?
> Did the repair succeed?
> Which survivor is medically fit?
> Who actually evacuated?
> Did quarantine work?
> Did raiders breach the shelter?
> How much radiation was absorbed?

Those remain domain authorities.

---

# 1. Source-Evidence Interpretation

## 1.1 Unified alerting is genuinely absent

The source reports zero Core matches for:
- `EmergencyBroadcast`;
- `AlertSystem`;
- `EmergencyAlert`;
- `WarningSystem`;
- `ShelterAlert`;
- `EvacuationAlert`.

A central warning/read-model layer is justified.

## 1.2 Existing systems already detect danger

This is crucial.

The alert system must subscribe to canonical signals rather than reimplement:
- storm detection;
- fire spread;
- outbreak detection;
- flooding thresholds;
- radiation dose;
- component failure.

## 1.3 Plan 158 likely owns emergency response protocols

The source says Plan 158 adds disaster response protocols.

Therefore:
- response execution;
- evacuation plan execution;
- assignments;
- supplies;
- completion
should be audited there before duplicating them.

## 1.4 Plan 186 and Plan 135 already add warning producers

Plan 194 should unify those warnings.

It should not make their systems depend on a second state machine for basic correctness.

## 1.5 Acknowledgment cannot stop real escalation

The source says:
> acknowledgment stops alert escalation.

That is unsafe if “escalation” means hazard severity.

Correct split:

```text
hazard escalation = source authority
notification escalation = alert presentation
```

Acknowledgment may stop:
- repeated popup/siren cadence;
- “unseen” escalation.

It may not stop:
- fire;
- flood;
- storm;
- disease;
- raid.

## 1.6 `successChance` does not belong in generic alert response

Response success must come from:
- repair;
- combat;
- medical;
- disaster;
- evacuation
systems.

A generic alert-layer `successChance` would be a parallel simulation truth.

## 1.7 Survivor reactions are not broadcast side effects by default

Panic, preparation, evacuation and refusal belong to:
- mental health;
- autonomy;
- duty;
- disaster response.

Alert broadcast can produce:
- perceived emergency event;
- readiness cue.

The relevant survivor system decides reaction.

---

# 2. Non-Negotiable Alert Invariants

## INV-194.1 — One incident owner per hazard

The domain system owns the real event.

## INV-194.2 — Alert references incident identity

No duplicate incident truth.

## INV-194.3 — Alert creation is deterministic from a source observation

## INV-194.4 — Forecast uncertainty remains source-owned

## INV-194.5 — Severity is derived from domain facts and alert policy

## INV-194.6 — Priority is not hazard state

Priority controls presentation/order.

## INV-194.7 — Acknowledgment is not mitigation

## INV-194.8 — Acknowledgment cannot stop physical escalation

## INV-194.9 — Response recommendations do not execute domain work

## INV-194.10 — Plan 158 owns generic disaster protocol execution if present

## INV-194.11 — DutyRoster owns survivor assignment

## INV-194.12 — Movement/evacuation authority owns survivor relocation

## INV-194.13 — Inventory owns required supplies

## INV-194.14 — Repair/medical/combat systems own success/failure

## INV-194.15 — Survivor panic/reaction is canonical mental-health/autonomy behavior

## INV-194.16 — One source incident produces at most one active alert per alert channel/policy

## INV-194.17 — Alert updates are revisions, not duplicates

## INV-194.18 — Resolved source incident resolves/closes alert

## INV-194.19 — Old saves do not generate historical warning spam

## INV-194.20 — Headless alert generation is UI-independent

## INV-194.21 — Accessibility does not rely on color or audio alone

## INV-194.22 — Alert fatigue has explicit budgets

## INV-194.23 — Warnings distinguish forecast, detected hazard and active emergency

## INV-194.24 — No per-frame all-system polling

---

# 3. Definition of Done

Plan 194 closes only when:

- every alert-producing domain authority is documented;
- existing domain-specific warning/notification paths are inventoried;
- a typed warning-observation contract exists;
- source incidents have stable IDs;
- duplicate warnings for one incident collapse into one evolving alert;
- forecast alerts can later transition to active/cleared without creating duplicate history;
- severity levels are data-driven or policy-driven with explicit domain mapping;
- priority combines severity, urgency, affected scope and confidence without becoming second hazard truth;
- acknowledgment changes presentation/read state only;
- alert escalation after acknowledgment is defined separately from hazard escalation;
- emergency alerts continue to update if source severity increases;
- source resolution closes alerts exactly once;
- visual, textual and optional audio cues exist;
- critical information has non-color/non-audio alternatives;
- affected areas are mapped only when canonical zone/location IDs exist;
- recommended actions reference real actions/systems;
- response UI deep-links to canonical systems;
- Plan 158 protocol integration is used if present;
- no generic alert-layer survivor assignment or success chance exists unless no other canonical owner exists and an ADR explicitly justifies it;
- old saves load with no fabricated historical alerts;
- active source incidents on load can rebuild current alerts deterministically;
- history is bounded according to central event/archive policy;
- no low-value warning spam;
- warning thresholds have hysteresis where needed;
- 30/120/180-day simulations measure alert volume and critical-alert miss rate;
- multi-crisis prioritization is deterministic;
- `--emergency-alert-selftest` exists or equivalent;
- content acceptance proves every alert type has a real source producer and actionable consumer.

---

# 4. Phase P0 — Emergency & Notification Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
WeatherSystem warning APIs
ShelterFireHazardSystem incident IDs/state
DiseaseSystem outbreak state/events
SumpFloodingSystem state/events
RadiationSystem thresholds/events
raid/defense authority
breach/structural authority
power/air/water maintenance authority
Plan 158 disaster-response protocols
Plan 186 maintenance warnings
Plan 135 weather warnings
medical-emergency events
DutyRoster assignment APIs
evacuation/movement APIs
shelter zones/topology
semantic event bus
notification manager/UI
audio settings
accessibility settings
campaign clock
save order
journal/archive policy
```

## P0.2 Build emergency authority matrix

Create:

`docs/emergency/EMERGENCY_ALERT_AUTHORITY_MATRIX.md`

Columns:

```text
alert type
incident owner
source event/API
stable incident ID
forecast support
severity facts
resolution event
response owner
status
```

Rows:
- radiation storm;
- high local radiation;
- raid attack;
- shelter breach;
- disease outbreak;
- fire;
- flooding;
- power failure;
- ventilation/air failure;
- water contamination;
- structural collapse;
- gas leak if real;
- medical emergency;
- maintenance critical failure;
- any additional real hazards.

## P0.3 Inventory existing notifications

Find:
- popup;
- toast;
- log;
- journal;
- audio;
- map marker
for each hazard.

Disposition:
- route through central alert;
- preserve specialized detail;
- retire duplicate;
- leave non-emergency notification local.

## P0.4 Response authority ADR

Create:

`docs/architecture/ADR_ALERTS_VS_DISASTER_RESPONSE.md`

Answer:

```text
Does Plan 158 own protocols?
Who owns survivor assignments?
Who owns evacuation?
Who owns response items?
Who owns repair/containment success?
What may EmergencyAlertSystem command?
```

## P0.5 Alert lifecycle ADR

Create:

`ADR_EMERGENCY_ALERT_LIFECYCLE.md`

Define:
- forecast;
- active;
- escalated;
- acknowledged;
- mitigated;
- resolved;
- cancelled/false alarm.

## P0.6 Severity/priority ADR

Create:

`ADR_ALERT_SEVERITY_VS_PRIORITY.md`

Separate:
- hazard severity;
- player urgency;
- presentation priority.

## P0.7 Baseline proof

Demonstrate:
- same campaign can produce multiple unrelated notification patterns;
- no central active-alert list exists;
- no unified acknowledgment/history exists.

---

# TASK 194A — Warning Observation Contract

# 194A.0 Goal

Create a single typed input contract through which domain systems report warnings without ceding hazard ownership.

## 194A.1 DTO

Suggested:

```text
EmergencyWarningObservation
  observation_id
  incident_id
  source_system_id
  alert_type_id
  phase
  detected_day
  detected_time
  source_severity
  urgency
  time_to_impact optional
  confidence optional
  affected_scope_refs[]
  source_fact_refs[]
  recommended_response_tags[]
  revision
```

## 194A.2 Observation ID

Stable per emitted revision/event.

## 194A.3 Incident ID

Stable for whole hazard episode.

Examples:
- one fire;
- one flood episode;
- one incoming storm;
- one raid;
- one outbreak.

## 194A.4 Alert type

Data-backed stable ID.

## 194A.5 Phase

Suggested:

```text
forecast
watch
warning
active
recovering
resolved
cancelled
```

Use only necessary states.

## 194A.6 Source severity

Domain-specific.

Do not force every system to speak the same raw scale before adapter.

## 194A.7 Urgency

Source may provide:
- time to impact;
- growth rate;
- threshold proximity.

## 194A.8 Confidence

Only for forecasts/uncertain detection.

## 194A.9 Affected scope

Canonical IDs:
- shelter zone;
- location;
- survivor;
- system/component;
- world region.

## 194A.10 No free-form area identity

## 194A.11 Recommended response tags

Examples:
- shelter_in_place;
- evacuate;
- suppress_fire;
- repair;
- quarantine;
- shield;
- isolate_water;
- defend.

They are descriptors/deep-links.

## 194A.12 Source facts

Optional diagnostic refs.

## 194A.13 Revision

Higher revision updates same incident.

## 194A.14 Idempotence

Same observation cannot produce duplicate alert side effects.

## 194A.15 Event bus

Prefer semantic event/adapter subscription.

## 194A.16 No polling if producer event exists

## 194A.17 Poll adapters

Only for legacy systems lacking events.

Run:
- bounded day/hour/state boundary,
not frame.

## 194A.18 Producer adapter tests

Every alert producer has deterministic fixture.

### 194A DoD

Every emergency-producing system can emit a stable, typed observation that preserves domain ownership while giving the central alert layer enough information to present and prioritize danger.

---

# TASK 194B — Alert Definition & Registry

# 194B.0 Goal

Define player-facing alert behavior as data, without creating data-driven hazard logic.

## 194B.1 Data file

`Assets/StreamingAssets/Data/emergency_alert_definitions.json`

## 194B.2 Definition DTO

Suggested:

```text
id
display_key
description_key
icon_id
severity_mapping_id
priority_policy_id
presentation_policy_id
response_tags[]
map_scope_supported
history_policy
tutorial_tag optional
```

## 194B.3 Alert types

Ship only types with real producers.

Source candidate list:
- radiation_storm;
- raider_attack;
- shelter_breach;
- disease_outbreak;
- fire;
- flooding;
- power_failure;
- air_failure;
- water_contamination;
- structural_collapse;
- gas_leak;
- medical_emergency.

## 194B.4 No count-driven implementation

“12+ alert types” is not a goal if several have no real subsystem.

## 194B.5 Missing producer

Definition fails reachability.

## 194B.6 Severity definitions

Player-facing:

```text
info
warning
critical
emergency
```

Names are acceptable.

## 194B.7 Severity semantics

### Info
Operational awareness.

### Warning
Preparation recommended.

### Critical
Immediate action strongly advised.

### Emergency
Immediate life/shelter threat.

## 194B.8 Low supplies

Do not necessarily include in emergency alert system.

Could remain:
- resource notification.

Only include if it crosses a real critical failure threshold.

## 194B.9 Icon

Uses UI asset catalog.

## 194B.10 Audio cue

Presentation policy, not type logic.

## 194B.11 Map support

Only for spatial incidents.

## 194B.12 Required actions

Do not persist arbitrary strings.

Use typed response action descriptors.

## 194B.13 Localization

All text keyed.

## 194B.14 Data integrity

Validate:
- producer adapter;
- severity mapping;
- response tag consumer;
- icon;
- localization.

### 194B DoD

Alert definitions control how known incidents are presented and routed, while all hazard behavior remains in the source systems.

---

# TASK 194C — Severity, Urgency & Priority Projection

# 194C.0 Goal

Create a deterministic ordering that puts the most actionable danger first.

## 194C.1 Separate severity and priority

Severity:
- nature/magnitude of hazard.

Priority:
- current player attention order.

## 194C.2 Priority inputs

Possible:

```text
severity_band
time_to_impact
affected_survivor_count
critical_system_scope
spread/growth trend
confidence
current mitigation status
player acknowledgment/read state
```

## 194C.3 Acknowledgment weight

Can reduce:
- presentation nag priority.

Cannot reduce:
- hazard severity.

## 194C.4 Source base priority

Source proposes 1–10.

Could use normalized score internally.

Do not expose if semantic ordering is sufficient.

## 194C.5 Auto escalation

Correct definition:

```text
presentation escalation
```

Examples:
- toast → persistent banner;
- banner → siren repeat;
- reorder higher if time-to-impact shrinks.

## 194C.6 Hazard escalation

Only source observation can increase severity.

## 194C.7 Time-to-impact

Critical for forecast alerts.

## 194C.8 Affected scope

One-person medical event vs shelter-wide ventilation failure.

Both may be emergency, but attention order differs.

## 194C.9 Confidence

Low-confidence forecast may rank below confirmed active threat.

## 194C.10 Ties

Stable:
- severity;
- urgency;
- incident ID.

## 194C.11 No random priority

## 194C.12 Priority refresh

On:
- source revision;
- time boundary;
- acknowledgment;
- mitigation state.

## 194C.13 No frame recompute

## 194C.14 Priority reason trace

UI/debug can explain:
- “Emergency: 45 minutes to impact, shelter-wide.”

## 194C.15 Priority caps

Avoid huge arbitrary numbers.

## 194C.16 Multi-crisis stack

Highest current priority first.

## 194C.17 Critical-alert visibility

Never hidden by low-priority filter settings.

## 194C.18 Filter semantics

User may suppress:
- info;
- some warnings.

Cannot accidentally suppress required life-safety information without explicit advanced setting and warning.

### 194C DoD

Alert priority is stable, explainable and attention-focused, while severity remains grounded in the source hazard rather than UI acknowledgment state.

---

# TASK 194D — Forecast, Detection, Active Emergency & Resolution

# 194D.0 Goal

Give the player genuinely proactive warning where the source systems support prediction.

## 194D.1 Forecast capability audit

For each type classify:

```text
forecastable
threshold-detectable
instantaneous
```

## 194D.2 Radiation storm

WeatherSystem may support:
- forecast window.

Use that.

## 194D.3 Raid

Use:
- scouting/intelligence/faction warning
if source system has lead time.

Do not magically reveal unseen attack.

## 194D.4 Disease outbreak

May provide:
- suspected cluster;
- confirmed outbreak.

Use DiseaseSystem state.

## 194D.5 Fire

Usually detected after ignition.

Could have:
- smoke/temperature sensor warning
only if shelter sensor system exists.

## 194D.6 Flooding

Could have:
- rising sump warning.

## 194D.7 Radiation

Threshold warning.

## 194D.8 Maintenance

Plan 186 may provide:
- degraded;
- imminent failure;
- failed.

## 194D.9 Structural collapse

Only forecast if structural health exists.

## 194D.10 Medical emergency

Immediate.

## 194D.11 No fake lead time

If source cannot forecast:
- alert starts at detection.

## 194D.12 Lead time accuracy

Display:
- exact;
- estimate;
- unknown.

## 194D.13 Forecast confidence

Player-facing when relevant.

## 194D.14 Forecast update

Same incident alert evolves.

## 194D.15 Forecast cancelled

Source event:
- storm misses;
- threat dissipates.

Close as:
- cancelled/cleared.

## 194D.16 False alarm

Only if source forecasting model can produce false positives.

Do not manufacture inside alert system.

## 194D.17 Active transition

Alert remains same incident.

## 194D.18 Recovering

Optional:
- source hazard contained but cleanup ongoing.

## 194D.19 Resolved

Source says incident ended.

## 194D.20 Resolution reason

Capture:
- naturally passed;
- mitigated;
- repaired;
- defeated;
- contained.

Source-provided if available.

## 194D.21 No alert self-resolution

Except presentation-only dismissals for info alerts.

### 194D DoD

The alert lifecycle mirrors the real incident lifecycle, supports genuine early warning when available, and never invents prediction or resolution.

---

# TASK 194E — Deduplication, Correlation & Cascading Incidents

# 194E.0 Goal

Prevent ten notifications from representing one underlying crisis while preserving genuinely separate hazards.

## 194E.1 One incident → one alert

Stable incident key.

## 194E.2 Duplicate source event

Update existing alert.

## 194E.3 Multiple systems report same phenomenon

Example:
- WeatherSystem radiation storm;
- RadiationSystem high ambient radiation.

These may be:
- parent forecast + local effect;
or duplicate.

Define correlation policy.

## 194E.4 Parent/child incident relation

Suggested:

```text
parent_incident_id
```

Example:

```text
radiation storm
└── shelter-zone radiation breach
```

## 194E.5 Cascades

One power failure may cause:
- ventilation failure;
- water pump failure.

Do not collapse if actions differ.

Link:
- causal parent.

## 194E.6 Crisis grouping

UI can group related alerts.

## 194E.7 “The Crisis” narrative event

Only if multiple high-severity independent/linked incidents exceed threshold.

Not every pair.

## 194E.8 Hysteresis

Threshold-based warnings need:
- enter threshold;
- exit threshold.

Avoid on/off chatter.

## 194E.9 Cooldown

After resolution, same source can create new incident only when source assigns new ID/episode.

## 194E.10 No time-only dedupe guesses if stable incident IDs can exist

## 194E.11 Legacy producer without ID

Adapter derives stable episode key from:
- source;
- scope;
- threshold episode;
- sequence.

## 194E.12 Correlation diagnostics

Show:
- source;
- parent;
- child;
- dedupe reason.

### 194E DoD

Alerts represent real incident episodes rather than raw notification count, supporting causal grouping without hiding distinct actionable hazards.

---

# TASK 194F — Presentation, Audio, Map & Accessibility

# 194F.0 Goal

Make urgent warnings impossible to miss without creating alert fatigue or inaccessible cues.

## 194F.1 Presentation policy by severity

Example:

### Info
- event feed;
- optional toast.

### Warning
- toast/banner;
- optional audio cue.

### Critical
- persistent banner/panel;
- stronger audio/haptic;
- affected-area focus.

### Emergency
- highest-priority persistent presentation;
- distinct cue;
- direct actions.

## 194F.2 Color

Can reinforce severity.

Never sole signal.

## 194F.3 Icons

Distinct shape/icon.

## 194F.4 Text prefix

Example:
- WARNING;
- CRITICAL;
- EMERGENCY.

## 194F.5 Audio

Distinct categories, not necessarily one unique sound for every type.

Avoid 12+ confusing sirens.

Suggested:
- advisory;
- warning;
- critical;
- emergency.

Specific type can add subtle motif.

## 194F.6 Audio settings

Use global audio authority:
- volume;
- mute;
- category.

Do not store raw volume in Core alert state.

## 194F.7 Source “sound alerts bool/volume”

These belong in user settings, not campaign save, unless architecture says otherwise.

## 194F.8 Visual duration

Presentation/UI settings.

## 194F.9 Map alert

Use canonical zone/location refs.

## 194F.10 Map highlight

Does not reveal:
- unknown attacker position;
- hidden contamination source
unless source knowledge permits.

## 194F.11 Shelter zone highlight

For:
- fire;
- flood;
- breach;
- maintenance.

## 194F.12 World map

For:
- storms;
- raids;
- regional hazards
if knowledge exists.

## 194F.13 Survivor-specific alert

Medical emergency can deep-link to survivor.

## 194F.14 Detail panel

Show:

```text
what
where
severity
phase
lead time
confidence
affected scope
recommended actions
current source status
acknowledgment
```

## 194F.15 Direct actions

Examples:
- open shelter defense;
- open maintenance;
- open medical/quarantine;
- open duty roster;
- open evacuation protocol.

## 194F.16 No generic response panel duplicating those systems

## 194F.17 Keyboard/controller

Critical alert focus accessible.

## 194F.18 Screen reader

Read severity before details.

## 194F.19 Reduced motion

No mandatory flashing.

## 194F.20 Photosensitivity

No high-frequency flashing.

## 194F.21 Hearing accessibility

Every audio cue has:
- text/icon.

## 194F.22 Color vision

Severity shapes/text.

## 194F.23 Text scaling

Emergency panel remains readable.

## 194F.24 Pausing

If game supports pause on critical alert:
- setting/explicit design.

Do not assume.

## 194F.25 Modal restraint

Prefer nonblocking persistent UI.

## 194F.26 Alert stack cap

Visible:
- top critical alerts;
- expandable list.

## 194F.27 Alert fatigue budget

Count:
- alerts/day;
- repeat cues;
- false/low-value notices.

### 194F DoD

Emergency information is multimodal, accessible, nonblocking and prioritized, with direct paths into real response systems and explicit controls against notification fatigue.

---

# TASK 194G — Acknowledgment & Presentation Escalation

# 194G.0 Goal

Track whether the player has seen/accepted responsibility for a warning without confusing acknowledgment with mitigation.

## 194G.1 Acknowledgment state

Suggested:

```text
unseen
seen
acknowledged
```

## 194G.2 Info

Can auto-mark seen.

## 194G.3 Warning

May not require acknowledgment.

## 194G.4 Critical/Emergency

May remain persistent until acknowledged or resolved.

## 194G.5 Acknowledgment effect

Changes:
- UI repeat cadence;
- unseen badge;
- history metadata.

## 194G.6 No physical effect

## 194G.7 Alert can worsen after acknowledgment

If source severity increases:
- alert updates;
- may require re-acknowledgment on major escalation.

## 194G.8 Revision acknowledgment

Track:

```text
acknowledged_revision
```

If revision crosses severity threshold:
- new attention cue.

## 194G.9 Auto-acknowledge

Source proposes threshold.

Recommended:
- only for low-severity info;
- user preference.

## 194G.10 Auto-ack settings

UI/user-settings authority.

Not simulation exploit.

## 194G.11 Escalation cadence

For unseen urgent alerts:
- repeat after bounded interval.

## 194G.12 No spam loop

Max repeats.

## 194G.13 Input-unavailable case

Controller/modal/paused screen must still allow acknowledgment.

## 194G.14 Headless

Acknowledgment optional; alert lifecycle still works.

## 194G.15 Multiplayer

Not relevant unless future.

### 194G DoD

Acknowledgment cleanly represents player awareness and notification handling while hazards continue to evolve entirely according to their source systems.

---

# TASK 194H — Response Recommendation & Protocol Integration

# 194H.0 Goal

Turn alerts into actionable preparation without creating a second disaster-response engine.

## 194H.1 Response descriptor

Suggested:

```text
EmergencyResponseRecommendation
  response_tag
  display_key
  target_scope_ref optional
  required_system_id
  action_uri/command_id
  urgency
  readiness_status optional
```

## 194H.2 Examples

### Radiation storm
- shelter_in_place;
- increase_shielding;
- recall_expeditions if supported;
- secure ventilation.

### Fire
- open fire suppression;
- evacuate affected zone;
- isolate power if supported.

### Disease
- open quarantine;
- assign medical staff;
- review isolation capacity.

### Flood
- open pumps/maintenance;
- evacuate zone.

## 194H.3 Recommendation is not action

## 194H.4 Readiness status

Can query:
- available staff;
- required equipment;
- protocol preparedness.

But no state duplication.

## 194H.5 Plan 158 adapter

If Plan 158 has:

```text
DisasterProtocol
ActivateProtocol(...)
```

alert may offer:
- activate existing protocol.

## 194H.6 Protocol configuration

Belongs to disaster-response authority.

## 194H.7 Evacuation routes

Belong to:
- shelter topology;
- evacuation/disaster system.

Alert only displays.

## 194H.8 Assembly points

Same.

## 194H.9 Supplies

Inventory/protocol system.

## 194H.10 Survivor assignment

DutyRoster/protocol authority.

## 194H.11 Response time

Domain work/job authority.

## 194H.12 Success chance

Domain authority.

Delete generic alert-layer field.

## 194H.13 Response status

Alert can display a read model:

```text
not_started
responding
mitigated
failed
```

derived from protocol/domain state.

## 194H.14 Response outcome

Source system emits.

## 194H.15 Alert resolution

Not equivalent to response completion if hazard remains.

## 194H.16 Multiple responses

One alert may have:
- evacuation + repair + medical.

Display several action links.

## 194H.17 Automatic protocol activation

Only if disaster-response/game policy explicitly permits.

Default:
- player-confirmed for high-impact actions.

## 194H.18 Preparedness templates

Follow Plan 158.

## 194H.19 Response history

Alert history may reference:
- response/protocol IDs.

Do not duplicate full records.

### 194H DoD

Alerts provide meaningful preparation and deep links into canonical emergency-response systems without owning assignments, resources, evacuation, timers or success calculations.

---

# TASK 194I — Survivor Reaction & Communication Integration

# 194I.0 Goal

Make warnings matter to survivors through real social/mental-health/autonomy systems, not direct alert-layer mutations.

## 194I.1 Broadcast event

Emit:

```text
emergency_alert_broadcast
```

with:
- incident;
- severity;
- scope;
- confidence.

## 194I.2 Mental-health consumer

May react:
- anxiety;
- panic;
- preparedness
according to Plan 179/mental-health state.

## 194I.3 No direct `panic += severity`

## 194I.4 Autonomy

Survivors may:
- self-shelter;
- refuse unsafe task;
- seek dependent family member
if Plan 144/autonomy behavior supports it.

## 194I.5 DutyRoster

Emergency policy may reassign duties.

Alert does not.

## 194I.6 Routine system

Plan 188 routines yield to emergency via ShelterSchedule/response systems.

## 194I.7 Children/dependents

Childcare/caregiving authority.

## 194I.8 Alert reach

If shelter communications can fail:
- warning reach may depend on power/audio/network.

Only implement if communication infrastructure exists.

## 194I.9 Player vs survivor awareness

MVP may treat shelter-wide alert as shared knowledge.

Document.

## 194I.10 False alarm reaction

Only if false alarms exist canonically.

## 194I.11 Alert fatigue

Repeated warnings can affect reaction only if mental-health/social systems explicitly model it.

Default:
- UI fatigue only, not survivor stat.

### 194I DoD

Survivor behavior can consume warning broadcasts through existing autonomy, duty and mental-health systems while the alert layer remains purely coordinative.

---

# TASK 194J — Alert History, Audit & Archive Semantics

# 194J.0 Goal

Preserve operational history without duplicating the campaign archive or generating unbounded save growth.

## 194J.1 Active alerts

Persist current alert projection if needed.

## 194J.2 History

Store compact completed alert summaries or derive from semantic event log.

## 194J.3 Source last 50

Treat as UI retention candidate, not mandatory persistence truth.

## 194J.4 Better model

```text
recent operational history
+ significant campaign archive
```

## 194J.5 Recent history

Could retain:
- 50–200;
- or last N days
according to central retention policy.

## 194J.6 Significant incidents

Archive:
- deaths;
- shelter-wide emergencies;
- major raids;
- catastrophic failures;
- important successful evacuations.

Plan 162/archive if available.

## 194J.7 Search/filter

UI can filter:
- type;
- severity;
- date;
- outcome.

## 194J.8 Do not persist duplicated source descriptions

Reference:
- alert definition ID;
- incident metadata.

## 194J.9 Response references

Store IDs.

## 194J.10 Acknowledgment metadata

Can persist:
- acknowledged day/time;
- revision.

## 194J.11 Diagnostic audit

Keep:
- producer;
- observations;
- severity revisions;
- close reason
in debug/test output.

## 194J.12 Journal

Only significant event.

No automatic journal for every info/warning alert.

### 194J DoD

Operational alert history remains useful and bounded, while major emergencies flow into the canonical archive rather than creating a competing history system.

---

# TASK 194K — Persistence, Migration & Restore

# 194K.0 Goal

Make alert state save-safe without freezing stale projections or replaying notifications.

## 194K.1 Persist minimum

Suggested:

```text
schema_version
active_alert_records[]
acknowledgment_state
acknowledged_revision
presentation_repeat_state optional
recent_history_refs/summaries
```

## 194K.2 Source incident state remains source-owned

## 194K.3 Restore strategy

After all hazard systems restore:
1. restore alert metadata;
2. reconcile active source incidents;
3. rebuild/update projections;
4. suppress replay side effects.

## 194K.4 Save ordering

Critical.

Alert restore after:
- source hazard state
or in two-phase restore.

## 194K.5 Missing source incident

Alert closes/removes as stale.

## 194K.6 Source incident active but alert missing

Rebuild deterministically.

## 194K.7 Old save

No alert section.

Do not populate historical alerts.

After source systems restore:
- create alerts only for **currently active/forecast incidents**.

## 194K.8 No migration popup storm

Restored alerts are marked:
- restored/current
and presentation policy decides whether urgent alert must still surface.

## 194K.9 Critical active incident

Should surface after load even if newly reconstructed.

## 194K.10 Acknowledged critical incident

Preserve acknowledgment for same revision.

If source worsened while offline is impossible because campaign time did not advance, no new cue.

## 194K.11 Schema changes

Alert definitions can evolve.

Persist:
- stable type ID;
- source incident ID.

## 194K.12 Removed type

Fallback generic emergency presentation, not crash.

## 194K.13 History compaction

Versioned.

## 194K.14 Idempotence

Restore:
- no duplicate audio;
- no duplicate journal;
- no duplicate response activation.

### 194K DoD

Alert metadata survives save/load while the truth is re-reconciled from canonical hazard systems and old saves avoid fabricated historical warnings.

---

# TASK 194L — Determinism & Performance

# 194L.0 Goal

Keep the alert layer cheap, reproducible and event-driven.

## 194L.1 No RNG by default

Alert creation from known observation is deterministic.

## 194L.2 Forecast RNG

Belongs to:
- weather;
- intel;
- disease detection
source.

## 194L.3 Stable alert ID

Suggested:

```text
alert:<source_system>:<incident_id>:<alert_type>
```

## 194L.4 Alert revision

Monotonic.

## 194L.5 Priority projection

Pure function of:
- current observation;
- clock;
- acknowledgment;
- policy.

## 194L.6 No per-frame polling

## 194L.7 Event-driven refresh

On:
- source revision;
- time-to-impact boundary;
- acknowledgment;
- response-state change;
- source resolve.

## 194L.8 Scheduled urgency checkpoints

For forecasts:
- next warning threshold.

## 194L.9 Complexity

O(active alerts log active alerts) for sorting is fine at small N.

## 194L.10 Alert count

Should be naturally small.

## 194L.11 Event storm

Coalesce multiple source updates in one simulation transaction.

## 194L.12 UI debounce

Avoid redraw/audio cue for inconsequential revisions.

## 194L.13 State size

Bounded history.

## 194L.14 Headless

No UI/audio dependency.

### 194L DoD

The system reacts only to meaningful source changes and scheduled urgency boundaries, producing stable IDs/priorities without simulation polling or random behavior.

---

# TASK 194M — Alert Fatigue, Threshold Hysteresis & UX Budget

# 194M.0 Goal

Ensure players trust alerts because the system does not cry wolf constantly.

## 194M.1 Alert budget

Measure:

```text
info/day
warning/day
critical/day
emergency/day
repeat cues/day
dismissals without action
time-to-ack
```

## 194M.2 Minor maintenance

Keep outside emergency stack where possible.

## 194M.3 Hysteresis

Example:

```text
warning enters at 20%
clears below 15%
```

Source system/policy-specific.

## 194M.4 Threshold chatter

No repeated open/close around boundary.

## 194M.5 Bundling

Related low-level warnings can group:
- “3 maintenance warnings”.

Do not bundle distinct critical hazards.

## 194M.6 Repeat cue budget

Critical:
- bounded periodic reminder.

Emergency:
- stronger but still bounded.

## 194M.7 Acknowledged reminders

May reduce frequency.

## 194M.8 Resolved alerts

Disappear from active stack promptly.

## 194M.9 Long forecast

Do not siren every hour.

Escalate presentation at meaningful thresholds.

## 194M.10 Tutorial

First relevant warning only.

## 194M.11 Player filter

Info/warning adjustable.

Critical/emergency safety floor.

## 194M.12 False alarms

If canonical forecasts can be wrong, confidence and historical calibration matter.

## 194M.13 Trust metric

Track:
- warning precision;
- actionable warning ratio;
- repeated ignored alerts.

## 194M.14 No achievement encouraging spam

“Acknowledge 10 alerts” can incentivize alert farming.

Treat source quest hooks cautiously.

### 194M DoD

Alerts remain sparse, meaningful and trustworthy enough that critical warnings retain attention.

---

# TASK 194N — Cross-System Alert Adapters

# 194N.0 Goal

Implement explicit adapters for real source systems.

---

# 194N-WEATHER — Radiation Storm

## 194N.W1 Producer

WeatherSystem.

## 194N.W2 Forecast

Use existing forecast state.

## 194N.W3 Scope

World/shelter region.

## 194N.W4 Lead time

Source forecast.

## 194N.W5 Response tags

- shelter_in_place;
- shielding;
- expedition_recall if real.

## 194N.W6 Resolution

Storm passes.

---

# 194N-FIRE — Shelter Fire

## 194N.F1 Producer

ShelterFireHazardSystem.

## 194N.F2 Incident ID

One fire episode.

## 194N.F3 Scope

Canonical shelter zone/component.

## 194N.F4 Severity

Based on source:
- intensity;
- spread;
- occupied zone;
- critical infrastructure.

## 194N.F5 Response

Fire-suppression system/Plan 158.

## 194N.F6 Evacuation

Real topology/protocol.

---

# 194N-DISEASE — Outbreak

## 194N.D1 Producer

DiseaseSystem.

## 194N.D2 Phases

Possible:
- suspected;
- confirmed;
- uncontrolled;
- contained.

Use existing state.

## 194N.D3 Scope

Survivors/zone.

## 194N.D4 Response

Medical/quarantine.

## 194N.D5 No alert-owned quarantine.

---

# 194N-FLOOD — Sump Flooding

## 194N.FL1 Producer

SumpFloodingSystem.

## 194N.FL2 Lead time

Water level/trend if available.

## 194N.FL3 Scope

Flood-prone zones.

## 194N.FL4 Response

Pumping/maintenance/evacuation.

---

# 194N-RAD — Radiation Threshold

## 194N.R1 Producer

RadiationSystem.

## 194N.R2 Distinguish

- ambient/source hazard;
- individual high dose.

## 194N.R3 Individual medical warning

Could be medical/radiation subtype.

## 194N.R4 No duplicate storm alert

Use parent-child relation.

---

# 194N-RAID — Raider Attack

## 194N.RA1 Producer

Raid/faction/intelligence authority.

## 194N.RA2 Forecast

Only if scouting/intelligence knows.

## 194N.RA3 Scope

Shelter/world route.

## 194N.RA4 Response

Shelter defense/evacuation.

## 194N.RA5 No invented attacker visibility.

---

# 194N-MAINT — Power/Air/Water/Structural

## 194N.M1 Producer

Plan 186/current maintenance systems.

## 194N.M2 Warning ladder

Potential:
- degraded;
- imminent failure;
- failed.

## 194N.M3 Info vs emergency

A filter replacement should not equal:
- life-threatening ventilation loss.

## 194N.M4 Response

Maintenance/repair.

## 194N.M5 Water contamination

Could be contamination/water authority, not generic maintenance.

---

# 194N-MED — Medical Emergency

## 194N.MED1 Producer

Medical/lifecycle.

## 194N.MED2 Scope

Survivor.

## 194N.MED3 Examples

Only real:
- severe bleeding;
- collapse;
- critical infection;
- acute radiation syndrome threshold.

## 194N.MED4 Response

Medical assignment/treatment.

## 194N.MED5 Do not flood emergency system with routine illness.

### 194N DoD

Every shipped alert type has one explicit source adapter, correct scope/lead-time semantics and canonical response links.

---

# TASK 194O — UI Architecture

# 194O.0 Goal

Create one crisis-management surface that summarizes rather than duplicates all emergency subsystems.

## 194O.1 `EmergencyAlertPanel`

Appropriate as a UI aggregation surface.

## 194O.2 Active list

Fields:

```text
severity
type
phase
where
lead time
priority
acknowledgment
```

## 194O.3 Detail

Show:
- source status;
- recommended actions;
- response readiness;
- map.

## 194O.4 Response area

Buttons deep-link to canonical systems.

Avoid separate survivor/resource assignment widgets if those already exist.

## 194O.5 Protocol area

If Plan 158:
- list matching prepared protocols;
- activate via Plan-158 API.

## 194O.6 Evacuation view

Only if current evacuation system supports route/progress read model.

## 194O.7 History

Filter/search.

## 194O.8 Settings

Separate:
- presentation/user settings;
- campaign simulation settings.

Alert sound/display:
- user settings.

Do not store in emergency simulation state.

## 194O.9 Global indicator

Show count:
- critical/emergency;
- total active.

## 194O.10 Top alert

Click opens panel.

## 194O.11 Multiple simultaneous

Do not replace one with another.

## 194O.12 Critical modal

Avoid unless accessibility/playtesting proves needed.

## 194O.13 Tutorial

First warning:
- what severity means;
- how to acknowledge;
- where action links go.

## 194O.14 Tooltips

Not hover-only.

## 194O.15 Snapshot tests

- info;
- forecast warning;
- active critical;
- emergency;
- multiple alerts;
- acknowledged;
- resolved history.

### 194O DoD

The alert UI becomes a concise operational dashboard that shows urgency and provides navigation into real response systems without becoming a duplicate management game.

---

# TASK 194P — Semantic Events, Journal & Quest Hooks

# 194P.0 Goal

Expose meaningful crisis milestones without generating event spam or grind incentives.

## 194P.1 Engine events

Candidate:

```text
emergency_alert_created
emergency_alert_severity_changed
emergency_alert_acknowledged
emergency_alert_resolved
multi_crisis_entered
```

## 194P.2 Do not emit separate engine events merely for UI broadcast

`broadcast` may be presentation.

## 194P.3 Source narrative event names

Source:
- The Warning;
- The Alert;
- The Acknowledgment;
- The Response;
- The Evacuation;
- The Resolution;
- The Escalation;
- The Crisis.

Treat as authored narrative candidates.

## 194P.4 Journal

Record only:
- significant emergency;
- meaningful response;
- casualty;
- evacuation;
- major save.

## 194P.5 Archive

Plan 162 can retain major incidents.

## 194P.6 Quest hooks

Plan 171 owns dynamic quest generation.

Expose predicates:
- active_alert_type;
- active_critical_count;
- incident_resolved;
- evacuation_completed;
- preparedness_success
if source systems provide.

## 194P.7 Source quests

- acknowledge 10;
- respond to 20;
- evacuate 5;
- manage 3;
- prevent 10;
- no casualties in 50;
- maintain 100 days.

Treat as achievement/content backlog, not required mechanics.

## 194P.8 Avoid acknowledgment grind

Clicking alerts should not be rewarded as gameplay skill.

## 194P.9 Preparedness success

Use Plan 158/actual incident outcome.

## 194P.10 No casualty

Lifecycle/incident authority.

### 194P DoD

Emergency alerts expose clean semantic milestones to narrative/quest systems while operational clicks remain operational rather than grind objectives.

---

# TASK 194Q — Long-Horizon Simulation & CI

# 194Q.0 Goal

Prove the alert layer improves preparedness without becoming noisy, stale or inconsistent during complex campaigns.

## 194Q.1 30-day ordinary campaign

Track:
- warnings;
- criticals;
- emergencies;
- acknowledgments;
- response deep-links;
- stale alerts.

## 194Q.2 120-day normal campaign

Track:
- alert count by source;
- duplicate suppression;
- lead-time usefulness;
- average acknowledgment time;
- alert fatigue.

## 194Q.3 180-day stress campaign

Increase:
- storms;
- raids;
- maintenance degradation;
- outbreaks.

Verify:
- priority ordering;
- multi-crisis UI;
- no starvation of lower critical alert.

## 194Q.4 Peaceful scenario

Near-zero emergency alerts.

## 194Q.5 Many-alert scenario

20 simultaneous synthetic source incidents.

Panel remains stable.

## 194Q.6 One incident with 100 revisions

One alert, bounded history.

## 194Q.7 Forecast→active→resolved

No duplicate.

## 194Q.8 Forecast cancelled

Correct close state.

## 194Q.9 Acknowledged then severity worsens

New critical cue as policy requires.

## 194Q.10 Source resolves while UI closed

Alert closes.

## 194Q.11 Old save with active fire

Rebuild one active alert.

## 194Q.12 Old save with no active emergencies

No historical spam.

## 194Q.13 Plan 158 response

Deep-link/protocol state displays correctly.

## 194Q.14 Accessibility

Audio muted:
- still fully actionable.

Colorblind/reduced motion:
- severity remains clear.

## 194Q.15 Performance

Measure:
- observation processing;
- priority refresh;
- history compaction;
- UI update.

---

# 194Q-T — Testing & CI

## 194Q.T1 Data integrity

Validate:
- alert IDs;
- producer adapters;
- severity mappings;
- priority policies;
- response tags;
- icons;
- localization;
- map-scope types.

## 194Q.T2 Selftest

Create:

```text
--emergency-alert-selftest
```

## 194Q.T3 Selftest scenarios

At least:

1. no alerts;
2. radiation-storm forecast;
3. fire active;
4. disease outbreak;
5. flooding warning;
6. high radiation;
7. maintenance failure;
8. raid with known lead time;
9. medical emergency;
10. acknowledgment;
11. acknowledgment does not stop hazard escalation;
12. dedupe;
13. source revision;
14. severity escalation;
15. resolve;
16. forecast cancellation;
17. multiple simultaneous;
18. Plan-158 response deep-link/protocol;
19. old save;
20. save/load;
21. audio muted/accessibility;
22. headless.

## 194Q.T4 Source-scan authority gate

Detect:
- hazard state inside alert DTO beyond projection;
- response success chance;
- survivor assignment mutation;
- resource consumption;
- evacuation movement;
- direct panic mutation;
- per-frame system polling;
- seeded RNG in alert generation without forecast justification.

## 194Q.T5 Content acceptance

Alert type ladder:

```text
DISCOVERED
LOADED
REGISTERED
PRODUCER_FOUND
OBSERVATION_EMITTED
ALERT_CREATED
PRESENTED
ACTION_LINK_REACHABLE
RESOLVED
```

## 194Q.T6 Dead-alert-type gate

No definition with no producer.

## 194Q.T7 Dead-response-tag gate

No recommended action with no consumer.

## 194Q.T8 Golden alert fixtures

Fixed source observations:
- exact alert ID/severity/priority/presentation.

## 194Q.T9 Determinism fingerprint

Same incident set:
- same ordered active-alert list.

## 194Q.T10 History retention

Bounded.

## 194Q.T11 Performance

No frame polling.

## 194Q.T12 Generated docs

Create:
- `EMERGENCY_ALERT_ARCHITECTURE.md`;
- `EMERGENCY_ALERT_AUTHORITY_MATRIX.md`;
- `EMERGENCY_ALERT_TYPE_MATRIX.md`;
- `EMERGENCY_ALERT_SEVERITY_MATRIX.md`;
- `EMERGENCY_ALERT_RESPONSE_ADAPTER_MATRIX.md`;
- `EMERGENCY_ALERT_CORRELATION_MATRIX.md`;
- `EMERGENCY_ALERT_TEMPORAL_SEMANTICS.md`;
- `EMERGENCY_ALERT_MIGRATION_MATRIX.md`;
- `EMERGENCY_ALERT_FATIGUE_REPORT.md`;
- `ADR_ALERTS_VS_DISASTER_RESPONSE.md`;
- `ADR_EMERGENCY_ALERT_LIFECYCLE.md`;
- `ADR_ALERT_SEVERITY_VS_PRIORITY.md`.

### 194Q DoD

The unified alert layer remains deterministic, actionable, low-noise, backward-compatible and proven across both peaceful and cascading-crisis campaigns.

---

# TASK 194R — Advanced False Alarms, Inter-Settlement Warning & Emergency Communications: Follow-On

# 194R.0 Goal

Keep speculative information-network features out of the MVP.

## 194R.1 False alarms

Only if:
- forecast/intelligence source can be wrong;
- confidence/calibration is modeled.

## 194R.2 Sensor failures

Could create bad warnings only if:
- sensor/maintenance system supports detection quality.

## 194R.3 Sabotaged alarms

Requires:
- sabotage/faction infiltration.

## 194R.4 Inter-settlement warning

Plan 131 information/rumor network owns propagation.

Alert system can export:
- emergency bulletin.

## 194R.5 Radio emergency broadcast

Radio system owns transmission.

## 194R.6 Incoming allied warning

Plan 131/intel imports warning observation.

## 194R.7 Alert trading

Reframe as:
- emergency intelligence sharing.

Not a direct commerce feature.

## 194R.8 Emergency drills

Plan 158/training system.

## 194R.9 Trained responders

Skill/education authority.

## 194R.10 Automatic shelter AI

Future automation policy.

## 194R.11 External siren infrastructure

Only if shelter power/audio infrastructure models it.

## 194R.12 Communication failure

Future:
- power loss may reduce warning reach.

## 194R.13 Civil-defense doctrine

Governance/Plan 158.

### 194R DoD

Advanced warning reliability and information-sharing features compose through existing intelligence, radio, training, governance and infrastructure systems rather than expanding the central alert authority.

---

# 5. Core Alert Lifecycle

```text
SOURCE INCIDENT
      │
      ▼
WARNING OBSERVATION
      │
      ▼
ALERT CREATED
      │
      ├── forecast/watch
      ├── warning
      ├── active critical
      └── emergency
      │
      ├── acknowledged?  ──► presentation state only
      │
      ├── source revision ──► update same alert
      │
      ├── response state ───► display canonical status
      │
      └── source resolved ──► close/history
```

---

# 6. Incident Identity Contract

Every source hazard episode should expose:

```text
incident_id
```

Alert ID derives from it.

No random alert IDs.

---

# 7. Severity Contract

Severity describes:
- hazard urgency/impact.

It is not:
- how annoying the notification is.

---

# 8. Priority Contract

Priority describes:
- what the player should see first.

It can consider:
- acknowledgment;
- time-to-impact;
- scope.

---

# 9. Acknowledgment Contract

Acknowledgment means:
- player saw/accepted the warning.

It does not mean:
- hazard mitigated.

---

# 10. Forecast Contract

Forecast exists only when source has:
- prediction;
- lead time;
- confidence.

No alert-layer prediction.

---

# 11. Active Emergency Contract

Active means:
- source incident has crossed active-danger state.

---

# 12. Resolution Contract

Only source incident can resolve its alert.

UI dismissal is not resolution.

---

# 13. Response Contract

Alert gives:
- recommendation;
- navigation;
- readiness.

Canonical systems execute response.

---

# 14. Evacuation Contract

Evacuation route/assembly/assignment belongs to:
- Plan 158 / movement / shelter topology.

Alert only references.

---

# 15. Survivor Assignment Contract

DutyRoster/protocol authority.

No assignment arrays in alert truth.

---

# 16. Resource Contract

Inventory/protocol system owns required and consumed items.

---

# 17. Success Contract

No generic alert `successChance`.

Each response domain decides.

---

# 18. Panic Contract

Mental health/autonomy decides survivor reaction.

---

# 19. Map Contract

Only known affected scopes are highlighted.

No intelligence leaks.

---

# 20. Audio Contract

Audio is presentation.

Mute cannot suppress textual/visual critical information.

---

# 21. Settings Contract

User presentation settings:
- audio;
- volume;
- display duration;
- filter.

Campaign alert simulation state:
- none of those unless architecture requires.

---

# 22. History Contract

Operational history can be bounded.

Campaign historical truth remains archive/event authority.

---

# 23. Plan 158 Contract

If disaster protocols exist:

```text
alert
→ matching protocol recommendation
→ Plan 158 activation
→ Plan 158 status/outcome
→ alert displays status
```

---

# 24. Plan 186 Contract

Maintenance system owns:
- degradation;
- component failure;
- repair.

Alert consumes warning/failure observations.

---

# 25. Plan 135 Contract

Weather system owns:
- forecast;
- storm severity;
- cascade.

Alert unifies presentation.

---

# 26. Plan 131 Contract

Information network may distribute:
- external warning bulletins.

Alert system does not propagate rumors/intel itself.

---

# 27. Plan 171 Contract

Dynamic quests may consume:
- emergency events.

Alert system does not generate quests.

---

# 28. Plan 188 Contract

Routines yield to:
- emergency rules.

Alert does not directly edit routines.

---

# 29. Persistence Matrix

| Fact | Owner |
|---|---|
| fire state | Fire system |
| disease outbreak | DiseaseSystem |
| flood state | SumpFloodingSystem |
| radiation state | RadiationSystem |
| weather forecast | WeatherSystem |
| raid | raid authority |
| maintenance failure | maintenance |
| survivor medical state | medical |
| alert projection | EmergencyAlertSystem |
| acknowledgment | EmergencyAlertSystem |
| presentation repeat state | Alert/UI |
| disaster protocol | Plan 158 |
| survivor assignments | DutyRoster/protocol |
| evacuation movement | movement/protocol |
| resource consumption | Inventory/domain |
| response success | response domain |
| major historical record | Archive/event log |

---

# 30. Old-Save Migration

Old save:

```text
alert schema absent
```

Migration:
1. initialize empty alert metadata;
2. restore canonical hazard systems;
3. query active/forecast incidents;
4. reconstruct only current alerts;
5. do not invent past history;
6. suppress duplicate first-load journal/audio except currently critical/emergency alerts according to presentation policy.

---

# 31. Exactly-Once Identity

Stable:

```text
alert:<type>:<incident>
observation:<source>:<incident>:<revision>
ack:<alert>:<revision>
resolve:<alert>:<source-resolution-id>
```

---

# 32. Failure Injection Matrix

## N194.1 Acknowledging fire freezes severity
Expected: acknowledgment/hazard separation gate fails.

## N194.2 Alert response computes generic 70% repair success
Expected: response-authority gate fails.

## N194.3 Alert directly assigns survivor
Expected: DutyRoster authority gate fails.

## N194.4 Alert deducts fire extinguisher
Expected: inventory/domain authority gate fails.

## N194.5 Weather storm and radiation create duplicate identical top-level alert
Expected: correlation gate fails.

## N194.6 One fire emits 20 alerts after revisions
Expected: incident dedupe fails.

## N194.7 Resolved fire alert remains active
Expected: lifecycle reconciliation fails.

## N194.8 Low-supply filter warning appears as Emergency
Expected: severity mapping fails.

## N194.9 Muted audio makes emergency undiscoverable
Expected: accessibility gate fails.

## N194.10 Old save opens with 50 fabricated historical warnings
Expected: migration gate fails.

## N194.11 Alert generator uses seeded RNG to decide whether known fire exists
Expected: detection-authority gate fails.

## N194.12 UI map reveals unknown raid route
Expected: knowledge-scope gate fails.

---

# 33. Determinism Contract

Same:

```text
source incident states
+ warning observations
+ alert policy data
+ campaign time
+ acknowledgment metadata
```

must yield same:
- active alert IDs;
- severity;
- priority;
- phase;
- presentation eligibility;
- recommended action descriptors.

---

# 34. Long-Horizon Metrics

Track:

```text
alerts created
alerts updated
alerts resolved
duplicate observations suppressed
alerts by severity
alerts by producer
forecast lead time
critical lead time
acknowledgment time
repeat notification count
action-link usage
stale alerts
false/cleared forecasts
simultaneous critical count
history bytes
processing time
```

---

# 35. Alert Fatigue Guardrails

Success means:
- warnings are trusted;
- critical alerts stand out;
- info does not drown emergencies.

Failure means:
- every minor maintenance state becomes a banner;
- sirens fire constantly;
- players dismiss everything.

---

# 36. Severity Guardrails

A severity band should correspond to:
- concrete risk.

Do not make it purely type-based.

One fire can be:
- warning;
- critical;
- emergency
depending on real state.

---

# 37. Priority Guardrails

Priority must never hide:
- another critical emergency.

Top one first;
all critical visible.

---

# 38. Forecast Guardrails

Forecast is valuable only if:
- action can be taken before impact.

If lead time is near zero:
- active alert.

---

# 39. Response Guardrails

Recommended actions should be:
- short;
- actionable;
- linked.

Not 10 generic tasks for every event.

---

# 40. History Guardrails

History is for:
- learning;
- audit;
- narrative.

Not a duplicate incident database.

---

# 41. UI Acceptance

## Active alert
- what;
- where;
- severity;
- phase;
- time;
- response.

## Multiple crisis
- ordered;
- grouped by causal chain where useful.

## History
- searchable;
- outcome-aware.

## Settings
- presentation only.

---

# 42. Accessibility

- text + icon + color;
- audio optional;
- reduced motion;
- no flashing dependency;
- controller/keyboard;
- screen-reader order by severity;
- large text.

---

# 43. Localization

Alert definitions:
- keys.

Dynamic values:
- canonical entity renderers.

No free-form gameplay rules in localized strings.

---

# 44. Content Acceptance

Alert content ladder:

```text
DISCOVERED
LOADED
REGISTERED
PRODUCER_BOUND
OBSERVATION_EMITTED
ALERT_CREATED
PRESENTED
ACTION_REACHABLE
SOURCE_RESOLVED
```

No claimed type coverage without end-to-end proof.

---

# 45. Reachability

For every shipped alert type:
- deterministic source fixture;
- alert fixture;
- resolution fixture.

For every response tag:
- actual command/system target.

---

# 46. Performance Guardrails

- event-driven;
- scheduled urgency checkpoints;
- no frame polling;
- bounded history;
- small active list;
- coalesced revisions.

---

# 47. CI / Gate Set

Recommended:

```text
emergency_alert_authority_matrix
emergency_alert_source_reachability
emergency_alert_incident_dedupe
emergency_alert_severity_integrity
emergency_alert_priority_determinism
emergency_alert_ack_not_mitigation
emergency_alert_response_authority
emergency_alert_plan158_integration
emergency_alert_map_knowledge
emergency_alert_accessibility
emergency_alert_old_save
emergency_alert_restore_reconciliation
emergency_alert_history_bound
emergency_alert_fatigue
emergency_alert_headless
```

---

# 48. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --emergency-alert-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 49. Recommended Commit Breakdown

```text
194A-1 hazard/notification authority audit
194A-2 alerts-vs-response ADR
194A-3 alert lifecycle ADR
194A-4 severity-vs-priority ADR
194A-5 warning observation contract
194A-6 stable incident identity adapters
194A-7 producer fixture baseline
194A-8 docs/tests

194B-1 alert definition schema/loader
194B-2 severity mapping registry
194B-3 response-tag registry
194B-4 producer/consumer integrity
194B-5 localization/icon integrity
194B-6 type matrix/docs

194C-1 severity projection
194C-2 urgency/lead-time projection
194C-3 priority function
194C-4 stable ordering
194C-5 acknowledgment presentation weighting
194C-6 priority diagnostics
194C-7 multi-crisis tests
194C-8 docs

194D-1 forecast capability matrix
194D-2 forecast/watch/active transitions
194D-3 confidence/lead-time UI model
194D-4 cancellation
194D-5 source resolution
194D-6 no-fake-forecast tests
194D-7 lifecycle docs

194E-1 incident dedupe
194E-2 revision update semantics
194E-3 parent-child/cascade relation
194E-4 storm/radiation correlation
194E-5 hysteresis
194E-6 crisis grouping
194E-7 dedupe/cascade tests
194E-8 docs

194F-1 presentation policy
194F-2 alert stack
194F-3 severity icons/text
194F-4 audio integration
194F-5 map/zone highlight
194F-6 accessibility/reduced-motion
194F-7 action links
194F-8 UI snapshots

194G-1 unseen/seen/ack state
194G-2 revision acknowledgment
194G-3 repeat-cue policy
194G-4 low-severity auto-ack preference
194G-5 acknowledgment no-mitigation regression
194G-6 headless behavior
194G-7 docs

194H-1 response recommendation DTO
194H-2 Plan-158 adapter
194H-3 DutyRoster/protocol links
194H-4 evacuation read model
194H-5 inventory/readiness display
194H-6 response-state display
194H-7 no-generic-success tests
194H-8 docs

194I-1 broadcast semantic event
194I-2 mental-health consumer adapter
194I-3 autonomy/emergency behavior adapter
194I-4 routine/duty integration
194I-5 no-direct-panic tests
194I-6 docs

194J-1 operational history policy
194J-2 archive bridge
194J-3 filters/search
194J-4 response references
194J-5 history compaction
194J-6 journal restraint
194J-7 docs

194K-1 persistence schema
194K-2 two-phase restore reconciliation
194K-3 old-save active-incident rebuild
194K-4 stale-alert cleanup
194K-5 restore presentation policy
194K-6 idempotence tests
194K-7 migration docs

194L-1 event-driven refresh
194L-2 urgency checkpoints
194L-3 coalescing/debounce
194L-4 deterministic fingerprints
194L-5 perf/state benchmarks
194L-6 docs

194M-1 hysteresis policies
194M-2 alert budgets
194M-3 repeated-cue limits
194M-4 alert-fatigue metrics
194M-5 30-day sim
194M-6 120/180-day stress sims
194M-7 report

194N-1 Weather adapter
194N-2 Fire adapter
194N-3 Disease adapter
194N-4 Flood adapter
194N-5 Radiation adapter
194N-6 Raid adapter
194N-7 Maintenance adapter
194N-8 Medical adapter

194O-1 alert panel
194O-2 detail/action navigation
194O-3 protocol view
194O-4 history/settings
194O-5 accessibility/tutorial
194O-6 snapshots

194P-1 semantic events
194P-2 Plan-171 hook surface
194P-3 journal/archive event budget
194P-4 narrative-hook disposition

194Q-1 selftest
194Q-2 content acceptance/reachability
194Q-3 failure fixtures
194Q-4 golden alert corpus
194Q-5 final ship/no-ship report

194R-1 false-alarm/intel/radio/training follow-on disposition
```

---

# 50. Risk Register

## R194.1 Alert layer becomes second emergency simulator

Mitigation:
- observation/projection architecture;
- strict authority matrix.

## R194.2 Acknowledgment accidentally freezes danger

Mitigation:
- explicit acknowledgment-vs-hazard invariant.

## R194.3 Plan 158 gets duplicated

Mitigation:
- mandatory response authority ADR.

## R194.4 Alert fatigue

Mitigation:
- hysteresis;
- budgets;
- severity presentation policies.

## R194.5 Duplicate source notifications

Mitigation:
- stable incident IDs;
- correlation.

## R194.6 Alerts leak hidden intelligence

Mitigation:
- affected scope/forecast knowledge contract.

## R194.7 Old saves produce warning storm

Mitigation:
- reconstruct current incidents only.

## R194.8 Accessibility relies on sirens/colors

Mitigation:
- multimodal requirements.

## R194.9 History bloats saves

Mitigation:
- bounded operational history;
- canonical archive for major events.

## R194.10 Polling costs grow

Mitigation:
- event-driven producer adapters.

---

# 51. Acceptance Checklist

## P0

- [ ] WeatherSystem audited
- [ ] ShelterFireHazardSystem audited
- [ ] DiseaseSystem audited
- [ ] SumpFloodingSystem audited
- [ ] RadiationSystem audited
- [ ] raid/defense authority audited
- [ ] breach/structural systems audited
- [ ] power authority audited
- [ ] air/ventilation authority audited
- [ ] water contamination authority audited
- [ ] medical-emergency authority audited
- [ ] Plan 158 disaster-response audited
- [ ] Plan 186 maintenance warning audited
- [ ] Plan 135 weather warning audited
- [ ] DutyRoster audited
- [ ] evacuation/movement audited
- [ ] shelter zones/topology audited
- [ ] semantic event bus audited
- [ ] notification UI audited
- [ ] audio settings audited
- [ ] accessibility settings audited
- [ ] clock audited
- [ ] save ordering audited
- [ ] journal/archive policy audited
- [ ] emergency authority matrix
- [ ] alerts-vs-response ADR
- [ ] lifecycle ADR
- [ ] severity-vs-priority ADR
- [ ] fragmented-notification baseline captured

## 194A — Observation

- [ ] typed observation DTO
- [ ] stable observation ID
- [ ] stable incident ID
- [ ] stable alert type
- [ ] phase
- [ ] detected time
- [ ] domain-specific source severity
- [ ] urgency/time to impact
- [ ] forecast confidence
- [ ] canonical affected scope
- [ ] no free-form area truth
- [ ] response tags
- [ ] source fact refs optional
- [ ] monotonic revision
- [ ] idempotence
- [ ] event bus preferred
- [ ] no polling when event exists
- [ ] bounded legacy polling
- [ ] producer fixtures

## 194B — Definitions

- [ ] versioned alert definitions
- [ ] type DTO
- [ ] only real producer types ship
- [ ] 12+ not treated as quota
- [ ] missing producer fails reachability
- [ ] info severity
- [ ] warning severity
- [ ] critical severity
- [ ] emergency severity
- [ ] low-supply noise excluded unless true critical threshold
- [ ] icon catalog
- [ ] audio presentation separate
- [ ] map support only spatial
- [ ] typed response actions
- [ ] localization
- [ ] producer/consumer integrity

## 194C — Severity/Priority

- [ ] severity separate from priority
- [ ] priority inputs grounded
- [ ] acknowledgment affects nagging only
- [ ] no acknowledgment hazard effect
- [ ] presentation escalation explicit
- [ ] source-only hazard escalation
- [ ] time-to-impact
- [ ] affected-scope weight
- [ ] confidence weight
- [ ] stable tie-break
- [ ] no random priority
- [ ] refresh only on meaningful events
- [ ] no frame recompute
- [ ] reason trace
- [ ] multi-crisis stack
- [ ] critical alerts cannot be filtered away accidentally

## 194D — Lifecycle

- [ ] forecast capability matrix
- [ ] radiation forecast uses WeatherSystem
- [ ] raid forecast only from intel/scouting
- [ ] disease suspected/confirmed if real
- [ ] fire no fake early warning
- [ ] flood trend if real
- [ ] radiation threshold
- [ ] maintenance degradation/failure
- [ ] structural forecast only if real
- [ ] medical immediate
- [ ] no fake lead time
- [ ] lead-time certainty
- [ ] confidence
- [ ] same incident updates
- [ ] forecast cancel
- [ ] false alarm source-owned
- [ ] active transition
- [ ] recovering optional
- [ ] source resolution
- [ ] close reason
- [ ] no self-resolution

## 194E — Dedupe/Correlation

- [ ] one incident one alert
- [ ] duplicate revision update
- [ ] cross-system correlation policy
- [ ] parent-child incidents
- [ ] cascade relation
- [ ] distinct actionable hazards remain distinct
- [ ] crisis grouping
- [ ] narrative crisis threshold
- [ ] hysteresis
- [ ] new episode requires new source ID
- [ ] stable fallback ID for legacy sources
- [ ] diagnostics

## 194F — Presentation

- [ ] per-severity presentation policy
- [ ] color not sole signal
- [ ] icons
- [ ] severity text
- [ ] bounded audio categories
- [ ] global audio settings reused
- [ ] sound toggle not campaign simulation state
- [ ] display duration is UI setting
- [ ] canonical map refs
- [ ] no intel leak
- [ ] shelter-zone highlight
- [ ] world-map highlight only when known
- [ ] survivor-specific deep link
- [ ] complete detail panel
- [ ] direct action links
- [ ] no duplicate response-management UI
- [ ] keyboard/controller
- [ ] screen reader
- [ ] reduced motion
- [ ] no flashing
- [ ] hearing accessibility
- [ ] color-vision accessibility
- [ ] text scaling
- [ ] pause behavior explicit
- [ ] nonblocking default
- [ ] alert stack cap
- [ ] fatigue budget

## 194G — Acknowledgment

- [ ] unseen/seen/ack states
- [ ] info auto-seen policy
- [ ] warning acknowledgment optional
- [ ] critical persistent
- [ ] emergency persistent
- [ ] acknowledgment UI-only
- [ ] source can worsen after ack
- [ ] revision ack
- [ ] major severity escalation re-cues
- [ ] low-severity auto-ack preference
- [ ] presentation settings authority
- [ ] bounded repeat cadence
- [ ] no spam loop
- [ ] input access
- [ ] headless safe

## 194H — Response/Protocols

- [ ] typed response recommendation
- [ ] recommendation not action
- [ ] readiness queries only
- [ ] Plan 158 adapter
- [ ] protocol config stays Plan 158
- [ ] routes stay topology/protocol
- [ ] assembly stays protocol
- [ ] supplies stay inventory/protocol
- [ ] survivor assignment stays DutyRoster/protocol
- [ ] response time domain-owned
- [ ] generic success chance removed
- [ ] response status is read model
- [ ] outcome source-owned
- [ ] response complete != alert resolved necessarily
- [ ] multiple action links
- [ ] auto-activation policy explicit
- [ ] preparedness templates canonical
- [ ] response history stores refs

## 194I — Survivor reaction

- [ ] broadcast semantic event
- [ ] mental-health consumer
- [ ] no direct panic mutation
- [ ] autonomy consumer
- [ ] DutyRoster remains assignment owner
- [ ] Plan 188 emergency precedence
- [ ] dependents use caregiving
- [ ] communications reach only if real
- [ ] player/survivor awareness scope documented
- [ ] false-alarm reaction only if real
- [ ] UI alert fatigue not auto survivor stat

## 194J — History

- [ ] active alerts persisted/derived appropriately
- [ ] bounded recent history
- [ ] source last-50 treated as UI candidate
- [ ] significant archive bridge
- [ ] search/filter
- [ ] no duplicate full source descriptions
- [ ] response IDs only
- [ ] acknowledgment metadata
- [ ] diagnostic audit
- [ ] journal only significant

## 194K — Persistence/Migration

- [ ] schema version
- [ ] active alert metadata
- [ ] ack revision
- [ ] repeat state only if required
- [ ] recent history bounded
- [ ] source state not duplicated
- [ ] restore ordering
- [ ] reconcile active incidents
- [ ] stale alert cleanup
- [ ] missing alert rebuild
- [ ] old save no historical population
- [ ] current active incidents reconstructed
- [ ] no migration popup storm
- [ ] critical current incident still surfaces
- [ ] acknowledged revision preserved
- [ ] stable type IDs
- [ ] removed-type fallback
- [ ] history migration
- [ ] restore no duplicate side effects

## 194L — Determinism/Performance

- [ ] no alert RNG by default
- [ ] forecast RNG source-owned
- [ ] stable alert ID
- [ ] monotonic revision
- [ ] pure priority projection
- [ ] no per-frame polling
- [ ] event-driven refresh
- [ ] urgency checkpoints
- [ ] active-list performance
- [ ] source-update coalescing
- [ ] UI debounce
- [ ] bounded state
- [ ] headless

## 194M — Alert fatigue

- [ ] per-severity volume metrics
- [ ] minor maintenance excluded
- [ ] hysteresis
- [ ] no threshold chatter
- [ ] low-value bundling
- [ ] critical not bundled away
- [ ] repeat-cue budgets
- [ ] acknowledged reminder reduction
- [ ] prompt resolved removal
- [ ] long-forecast staged cues
- [ ] bounded tutorial
- [ ] info/warning filters
- [ ] critical/emergency safety floor
- [ ] false-alarm calibration if applicable
- [ ] trust metrics
- [ ] no acknowledgment grind reward

## 194N — Adapters

- [ ] Weather radiation-storm adapter
- [ ] Fire adapter
- [ ] Disease adapter
- [ ] Flood adapter
- [ ] Radiation threshold adapter
- [ ] Raid adapter
- [ ] Maintenance/power/air/water adapters
- [ ] Medical emergency adapter
- [ ] each incident ID stable
- [ ] each resolution source stable
- [ ] each affected scope canonical
- [ ] each response link real

## 194O — UI

- [ ] active list
- [ ] detail
- [ ] canonical response links
- [ ] Plan 158 protocol view
- [ ] evacuation read model only if real
- [ ] history
- [ ] presentation settings
- [ ] global critical count
- [ ] top-alert navigation
- [ ] simultaneous alerts
- [ ] modal restraint
- [ ] tutorial
- [ ] no hover-only details
- [ ] snapshots

## 194P — Events/Quest hooks

- [ ] bounded semantic engine events
- [ ] no redundant broadcast engine spam
- [ ] source narrative names treated as content
- [ ] significant journal rule
- [ ] Plan 162 archive bridge
- [ ] Plan 171 hook surface
- [ ] source quest ideas not required for ship
- [ ] acknowledgment grind avoided
- [ ] preparedness uses real response outcome
- [ ] casualty truth lifecycle-owned

## 194Q — Simulations/CI

- [ ] 30-day ordinary
- [ ] 120-day normal
- [ ] 180-day stress
- [ ] peaceful
- [ ] many simultaneous
- [ ] 100 revisions one incident
- [ ] forecast-active-resolved
- [ ] forecast cancelled
- [ ] acknowledged then worsened
- [ ] source resolves while panel closed
- [ ] old save active fire
- [ ] old save peaceful
- [ ] Plan 158 response integration
- [ ] audio-muted accessibility
- [ ] reduced-motion/color-safe
- [ ] data integrity
- [ ] emergency-alert selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] dead-alert-type gate
- [ ] dead-response-tag gate
- [ ] golden alert fixtures
- [ ] deterministic fingerprint
- [ ] bounded history
- [ ] performance
- [ ] generated docs
- [ ] verify-fast

## 194R — Follow-ons

- [ ] false alarms only if source uncertainty real
- [ ] sensor failures require sensor health
- [ ] sabotaged alarms require sabotage system
- [ ] inter-settlement warning uses Plan 131
- [ ] radio broadcast uses radio authority
- [ ] incoming warning uses intel network
- [ ] “alert trading” reframed as intel sharing
- [ ] drills use Plan 158/training
- [ ] responders use SkillProgression
- [ ] automation policy deferred
- [ ] siren infrastructure only if modeled
- [ ] communications failure only if modeled
- [ ] civil-defense doctrine uses governance

---

# 52. Ship / No-Ship Gate

**SHIP** only if:

```text
hazard_incident_authorities_documented == true
AND duplicate_hazard_state_in_alert_system == 0
AND duplicate_raid_state_in_alert_system == 0
AND duplicate_disease_state_in_alert_system == 0
AND duplicate_fire_state_in_alert_system == 0
AND alert_owned_survivor_assignment == false
AND alert_owned_resource_consumption == false
AND alert_owned_response_success_chance == false
AND alert_owned_evacuation_movement == false
AND acknowledgment_stops_physical_hazard_escalation == false
AND direct_alert_owned_survivor_panic_mutation == false
AND duplicate_alerts_per_incident == 0
AND stale_resolved_alerts == 0
AND per_frame_all_system_polling == 0
AND alert_generation_uses_unnecessary_rng == false
AND inaccessible_audio_only_critical_alerts == 0
AND color_only_severity_states == 0
AND hidden_intelligence_leaks == 0
AND dead_alert_type_definitions == 0
AND dead_response_tags == 0
AND old_save_historical_alert_spam == false
AND emergency_alert_old_save == pass
AND emergency_alert_save_roundtrip == pass
AND emergency_alert_incident_dedupe == pass
AND emergency_alert_ack_not_mitigation == pass
AND emergency_alert_priority_determinism == pass
AND emergency_alert_source_resolution == pass
AND emergency_alert_plan158_integration == pass
AND emergency_alert_accessibility == pass
AND emergency_alert_30_day_fatigue == pass
AND emergency_alert_120_day_fatigue == pass
AND emergency_alert_180_day_multicrisis == pass
AND emergency_alert_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 53. Implementer Handoff

1. Audit every current emergency producer and its existing notification path before writing the central system.
2. Give every real hazard episode a stable incident ID.
3. Make source systems emit typed warning observations; do not make the alert layer rediscover hazards.
4. Treat forecast, active hazard and resolution as revisions of the same alert where they refer to the same incident.
5. Keep hazard severity separate from UI priority.
6. Keep acknowledgment separate from mitigation.
7. Explicitly regression-test that clicking “acknowledge” cannot stop a fire, flood, storm, raid or outbreak from escalating.
8. Build deterministic priority from severity, urgency, affected scope and confidence.
9. Use stable tie-breaking.
10. Correlate parent/child hazards such as storm → local radiation without collapsing distinct actions.
11. Add hysteresis to threshold warnings so the UI does not chatter.
12. Keep Info-level operational noise out of the emergency stack unless it has real safety significance.
13. Reuse global audio/accessibility settings; do not persist volume controls inside campaign emergency state.
14. Ensure every critical cue has text, icon and non-color representation.
15. Highlight only canonical known zones/locations and never leak hidden threat information.
16. Make the emergency panel a summary/deep-link surface rather than another response-management engine.
17. Audit Plan 158 before adding any protocol, evacuation, assignment, supplies or success-state DTO.
18. If Plan 158 exists, activate/read it through typed adapters.
19. Keep DutyRoster as assignment authority, Inventory as supply authority, movement as evacuation authority and each response domain as success authority.
20. Let mental-health/autonomy systems consume alert broadcasts; do not directly assign panic.
21. Reconcile alert state from source incidents after save restore.
22. On old saves, reconstruct only currently active/forecast hazards; never fabricate past alert history.
23. Bound recent history and send major disasters to the canonical archive.
24. Measure alert fatigue in 30/120/180-day simulations.
25. Test one incident with many revisions, many simultaneous incidents, forecast cancellation, active escalation after acknowledgment, and source resolution while UI is closed.
26. Reject any alert type with no real producer and any response action with no real consumer.
27. Close only when the system gives the player earlier, clearer, more actionable information without becoming a second emergency simulator.

---

# 54. Final Outcome

When this plan is complete, ASHFALL's emergencies stop arriving as disconnected subsystem notifications and become one coherent crisis-management picture.

A radiation storm can first appear as a forecast because the WeatherSystem genuinely predicts it. The alert shows its estimated arrival time, affected region, confidence and recommended shelter preparations. As the storm gets closer, the same alert becomes more urgent. When radiation rises around the shelter, a local radiation child alert can identify the affected zone without duplicating the storm itself.

A fire behaves differently. There may be no forecast. The FireHazardSystem detects ignition, emits the incident, and the central alert immediately presents where the fire is, how severe it is, which zones are threatened and where the player can open fire suppression or evacuation controls.

A disease outbreak can move from suspected to confirmed because the DiseaseSystem changes its own state. A sump flood can escalate as water rises. A degraded ventilation component can begin as a warning and become a shelter-wide emergency only when the maintenance system says air safety is actually threatened.

The alert system does not simulate any of those hazards.

It translates them into a shared language.

That language tells the player:

- what happened;
- where;
- how severe;
- how soon;
- how certain;
- who or what is affected;
- what actions are relevant.

Acknowledging the warning means the player has seen it. Nothing more. Fire still burns. Radiation still rises. Raiders still approach. The game never confuses a UI click with a solved emergency.

Responses also remain real. If Plan 158 owns disaster protocols, the alert opens or activates those protocols. DutyRoster assigns people. Inventory supplies equipment. Medical systems handle quarantine and treatment. Maintenance handles repair. Shelter defense handles attackers. Movement handles evacuation.

This keeps one source of truth per action.

The player therefore receives the proactive gameplay promised by the source plan without paying the architectural cost of another parallel disaster engine.

The same design also makes simultaneous crises manageable. A failing generator, incoming storm and medical collapse can all exist at once. Priority tells the player which requires attention first. Causal grouping shows when one failure is creating others. The alert stack keeps every critical incident visible.

And because warning volume, hysteresis, deduplication, presentation cadence and accessibility are explicitly designed, the system can remain trustworthy. A filter replacement does not scream like structural collapse. A long-range storm forecast does not sound a siren every hour. Muted audio does not hide life-safety information. Colorblind players do not lose severity meaning.

The result is a shelter that can finally behave like a place with an emergency-management layer rather than a collection of independent hazard scripts.

Threats are still dangerous.

But now the player can see them coming, understand what matters, and act before reactive chaos becomes catastrophe.
