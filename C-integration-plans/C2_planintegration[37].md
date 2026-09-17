# C2 — Flagship Integration Plan [37]: Shelter Radio Program Production, Presenter Workflows, Audience Response, and Follow-Up Opportunities

> **Deliverable:** `C2_planintegration[37].md`
> **Source scope:** Plan 173 — *Shelter Radio Production & Audience Response*
> **Primary objective:** let the shelter produce player-authored radio programs, assign presenters, consume real preparation resources, occupy an existing Plan 24 broadcast slot, receive a real Plan 157 delivery result, consume Plan 168 propaganda outcomes where applicable, and turn those facts into deterministic audience response and follow-up opportunities—without creating a second schedule, second station/frequency catalog, second communications network, second faction broadcast corpus, or second propaganda effect engine.
> **Required execution order:** **173A Program Production Contract → 173B Presenter/Audience Loop & Program Content → 173C Cross-System Integration, Save/CI, Exploit Control, Balance, and Closure**
> **Hard dependencies:** Plan 24 station identities/frequencies/unified broadcast schedule; Plan 73 passive faction-broadcast content; Plan 157 range/interception/encryption/jamming/delivery; Plan 168 propaganda message truth/effect resolution; survivor skills/traits; inventory/equipment/condition; faction standing/reputation; morale/social systems; quest/follow-up runtime; Plan 31 semantic event vocabulary; Plan 36 port-contract discipline; Plan 39 save durability; Plan 55 retention.
> **Scope discipline:** no `BroadcastSchedule` duplicate, no station/frequency ownership, no local signal-range calculation, no local jamming/interception model, no propaganda-effect reimplementation, no passive faction-radio content duplication, no program outcome that assumes delivery without Plan 157 evidence, no audience reaction rolled from hidden network state, no schedule-slot farming through save/load, and no persisted derived delivery/propaganda facts that can be reconstructed from authoritative outcome references.

---

# 0. Executive Intent

ASHFALL already has the major radio infrastructure pieces split across plans:

```text
Plan 24
→ stations
→ frequencies
→ one shared broadcast schedule

Plan 73
→ passive faction broadcasts

Plan 157
→ range
→ delivery
→ interception
→ encryption
→ jamming

Plan 168
→ propaganda message truth
→ propaganda effect resolution
```

What is still missing is the **player-production layer**:

```text
choose a program
→ assign a presenter
→ spend prep time/resources
→ consume an actual broadcast slot
→ attempt transmission
→ learn who actually received it
→ resolve audience response
→ create follow-up opportunities
```

The target architecture is:

```text
radio_programs.json
      │
      ▼
RadioProgramProductionSystem
      │
      ├─ template
      ├─ presenter
      ├─ preparation
      ├─ equipment/resources
      ├─ schedule-slot reference
      └─ quality
      │
      ▼
Plan 24 broadcast slot
      │
      ▼
Plan 157 delivery result
      │
      ├────────────► no reach / jammed / partial / delivered
      │
      └────────────► known recipient/audience context
      │
      ▼
Plan 168 propaganda result if relevant
      │
      ▼
AudienceResponseResolver
      │
      ├─ morale/reputation signals
      ├─ faction reaction hooks
      ├─ listener contacts
      └─ follow-up requests
```

The strongest product outcome is:

> **The player can build a recurring shelter radio identity—news, education, emergency advisories, entertainment, storytelling, or persuasive messaging—and observe believable consequences driven by who actually heard the broadcast, who presented it, how well it was produced, and what authoritative propaganda/faction systems concluded.**

---

# 1. Source Diagnosis

The source defines a very strict ownership split:

- Plan 24 owns station identities, frequencies, and the unified broadcast schedule.
- Plan 73 owns passive faction-broadcast content.
- Plan 157 owns radio range, interception, encryption, and jamming.
- Plan 168 owns propaganda message truth/effect resolution.
- Plan 173 owns player-authored program templates and execution against an existing Plan 24 schedule slot.

The source further requires:

- a Core `RadioProgramProductionSystem`,
- program templates,
- required equipment,
- presenter assignment,
- preparation cost,
- schedule-slot reference,
- delivered/cancelled outcomes,
- quality band,
- audience response,
- reusable follow-up hooks,
- deterministic audience response,
- player-program types:
  - news,
  - education,
  - entertainment,
  - emergency,
  - storytelling,
- legible outcomes:
  - morale,
  - reputation signals,
  - faction reaction hooks,
  - listener contacts,
  - follow-up broadcasts,
- save only production history and unresolved follow-up hooks,
- `radio_programs.json` contains no schedule/frequency authority,
- tests for cancellation, missing equipment, eligibility, determinism, save/load, and no duplicate schedule ownership.

The architectural reading is:

```text
RadioProgramProductionSystem
= program production authority
```

but not:

```text
radio schedule authority
communications delivery authority
propaganda authority
faction passive-radio authority
```

---

# 2. Program-Level Success Criteria

C2[37] closes only when all of the following are true.

1. Exactly one broadcast schedule remains authoritative under Plan 24.
2. Exactly one communications/delivery authority remains under Plan 157.
3. Propaganda message/effect truth remains under Plan 168.
4. Passive faction broadcast content remains under Plan 73.
5. Player-created program templates load from `radio_programs.json`.
6. Every program execution references a real Plan 24 schedule slot.
7. Presenter eligibility is state-backed and validated.
8. Preparation consumes actual time/resources/equipment.
9. Program quality derives from real preparation/presenter/context.
10. A cancelled program does not emit a fake delivery result.
11. Audience response is only resolved after consuming Plan 157 delivery evidence.
12. Propaganda-tagged programs consume Plan 168 output rather than locally calculating persuasion.
13. Non-propaganda programs can still create morale/reputation/contact/follow-up consequences.
14. Audience response is deterministic for the same authoritative inputs.
15. No audience-response logic silently assumes a station/frequency/range.
16. No duplicate schedule-slot object is persisted locally as authority.
17. Save state stores only production history, unresolved follow-ups, and minimal in-progress production state if required.
18. Old saves initialize cleanly.
19. Schedule-slot occupancy is idempotent across save/load.
20. Equipment requirements are checked against actual inventory/equipment.
21. Presenter capability uses canonical survivor skill/trait data.
22. Follow-up opportunities are durable and expire/resolve deterministically.
23. Faction reaction hooks route through canonical faction APIs.
24. Morale effects route through canonical morale/social systems.
25. Reputation signals are not a second reputation ledger.
26. Headless CI can produce/cancel/deliver/resolve programs without UI.
27. Program history is retention-aware.
28. UI clearly distinguishes:
   - planned,
   - prepared,
   - scheduled,
   - cancelled,
   - aired but not received,
   - partially received,
   - successfully received.
29. Program production cannot be farmed by reopening UI or reloading before broadcast resolution.
30. Player program templates contain no frequency, station, signal range, or schedule authority.

---

# 3. Architectural Invariants

## 3.1 Plan 24 owns airtime

Program production stores:

```text
schedule_slot_id
```

or a stable external reference.

It does not clone:

```text
frequency
station
broadcast start/end schedule
```

unless cached for display only and marked non-authoritative.

## 3.2 Plan 157 owns delivery

The production system asks:

```text
what happened to this scheduled broadcast?
```

It does not ask:

```text
what is the range?
what factions are in range?
was it jammed?
```

and recalculate those locally.

## 3.3 Plan 168 owns propaganda effects

For propaganda/persuasive programs:

```text
production quality/context
→ Plan 168 input
→ Plan 168 result
```

The production system then records/uses that result.

## 3.4 Plan 73 owns passive faction content

Do not author faction broadcast corpus here.

## 3.5 Program production owns quality

Quality is a production fact based on:

- presenter capability,
- preparation effort,
- equipment,
- script/template demands,
- interruptions/cancellation.

## 3.6 Audience response is downstream

No response until delivery evidence exists.

## 3.7 Follow-ups are opportunities, not immediate duplicated quests

A follow-up hook can later create:

- contact,
- quest,
- requested broadcast,
- faction meeting,
- listener response.

The owning system decides final object creation.

## 3.8 Presenter assignment is real work

Presenter/preparation consumes survivor availability/time.

## 3.9 Program history is immutable outcome history

Once a broadcast result is committed, reload cannot change it.

## 3.10 UI is projection

Opening radio-production UI does not advance preparation or airtime.

---

# 4. Dependency Graph

```text
radio_programs.json
        │
        ▼
RadioProgramProductionSystem
        │
        ├─ presenter
        ├─ preparation
        ├─ resource/equipment check
        ├─ quality
        └─ slot reference
        │
        ▼
Plan 24 unified schedule
        │
        ▼
Plan 157 delivery result
        │
        ├─────────────► cancelled/not delivered
        │
        └─────────────► delivered audience context
                              │
                              ▼
                 Plan 168 if propaganda
                              │
                              ▼
                  AudienceResponseResolver
                              │
            ┌─────────────────┼─────────────────┐
            ▼                 ▼                 ▼
          morale          faction hooks     follow-ups
```

---

# 5. Baseline Capture

Before implementation, inspect and record:

- Plan 24 schedule DTO/API,
- schedule-slot reservation/commit/cancel semantics,
- station/frequency ownership,
- Plan 157 delivery-result DTO/API,
- jamming/interception/encryption outcome semantics,
- Plan 168 propaganda input/output types,
- passive faction broadcast APIs from Plan 73,
- survivor skill/trait/profession APIs,
- inventory/equipment/condition APIs,
- work/duty availability,
- faction standing/reputation APIs,
- morale/social consequence APIs,
- quest/follow-up opportunity APIs,
- current radio UI routes,
- save-section registration.

Run baseline verification:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Capture:

```text
existing schedule slot count
existing radio stations
existing frequency authority
delivery result fields
propaganda result fields
current player-broadcast capability if any
```

---

# 6. Workstream 173A — Program Production Contract

## Goal

Create one player-program production authority that prepares content and consumes an external schedule slot without duplicating the radio network.

---

# 7. 173A Phase A — Create `RadioProgramProductionSystem`

Recommended path:

```text
Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs
```

Responsibilities:

- load program templates,
- validate presenter eligibility,
- validate equipment/resources,
- begin preparation,
- track in-progress production,
- calculate production quality,
- request/consume a Plan 24 schedule slot,
- bind authoritative delivery result,
- bind Plan 168 propaganda result where applicable,
- resolve audience response,
- create follow-up hook records,
- capture/restore production-specific state/history.

---

# 8. 173A Phase B — Definition vs Runtime Types

Create:

```text
RadioProgramTemplate
RadioProgramProduction
RadioProgramPreparationState
RadioProgramQualityAssessment
RadioProgramExecutionOutcome
RadioAudienceResponse
RadioFollowUpHook
RadioProgramHistoryEntry
```

Do not overload one DTO.

---

# 9. 173A Phase C — `RadioProgramTemplate`

Fields:

```text
program_id
name_key
description_key
program_type
duration_class
presenter_requirements
equipment_requirements
resource_costs
preparation_time
quality_weights
audience_response_profile
propaganda_profile_id optional
follow_up_profile_ids
localization keys
```

Forbidden fields:

```text
frequency
station_id as authority
signal_range
broadcast_schedule
jamming rules
faction broadcast body
```

---

# 10. 173A Phase D — Program Type Vocabulary

Initial typed values:

```text
News
Education
Entertainment
Emergency
Storytelling
```

Optional future extension:

```text
Interview
Religious/Cultural
TechnicalInstruction
Propaganda
Debate
```

Only add if content and Plan 168 policy justify it.

---

# 11. 173A Phase E — Propaganda Tagging

A program may declare:

```text
uses_propaganda_resolution = true
propaganda_profile_id = ...
```

That means:

```text
Plan 168 owns persuasion/effect truth
```

not:

```text
ProgramType.Propaganda → local persuasion roll
```

---

# 12. 173A Phase F — Runtime Production Fields

Recommended:

```text
production_id
template_id
presenter_id
created_day
preparation_started_day
preparation_progress
resource_commitment_refs
equipment_refs
schedule_slot_id
status
quality_result
delivery_result_ref
propaganda_result_ref
audience_response_ref
follow_up_hook_ids
```

---

# 13. 173A Phase G — Production Status Vocabulary

```text
Draft
Preparing
Ready
Scheduled
Cancelled
Aired
DeliveryResolved
AudienceResolved
Closed
```

Use one lifecycle.

---

# 14. 173A Phase H — Schedule Reference

Store only:

```text
schedule_slot_id
```

plus optional immutable snapshot metadata for history such as:

```text
air_day
air_time label
```

if needed for archive/history.

Authority remains Plan 24.

---

# 15. 173A Phase I — Schedule Slot Reservation

Define explicit interface:

```text
IRadioScheduleSlotConsumer
```

Operations may include:

```text
CanReserve(slotId, programId)
Reserve(slotId, productionId)
Release(slotId, productionId)
CommitAired(slotId, productionId)
```

Names adapt to Plan 24.

---

# 16. 173A Phase J — No Local Slot Construction

Production system must not:

```text
new BroadcastSchedule(...)
new Frequency(...)
new Station(...)
```

except test fakes.

Add static/architecture test for this.

---

# 17. 173A Phase K — Presenter Eligibility

Template can require:

- literacy,
- communication/social skill,
- profession,
- technical skill,
- language,
- health/readiness,
- not incapacitated,
- available during preparation/broadcast.

Use canonical survivor APIs.

---

# 18. 173A Phase L — Program-Specific Presenter Profiles

Examples:

```text
News
→ literacy + social/leadership

Education
→ subject expertise + communication

Entertainment
→ performance/storytelling/social

Emergency
→ leadership/technical credibility

Storytelling
→ social/narrative/identity traits
```

Do not create new duplicate presenter stats if existing skills/traits suffice.

---

# 19. 173A Phase M — Presenter Availability

Presenter cannot simultaneously be:

- incapacitated,
- away on expedition,
- unavailable due to critical duty,
- assigned to conflicting broadcast/prep work.

Use canonical scheduler/availability.

---

# 20. 173A Phase N — Preparation Time

Preparation progresses at explicit cadence:

```text
work ticks / daily work allocation
```

not UI time.

---

# 21. 173A Phase O — Preparation Cost

Possible costs:

- paper,
- recording media,
- power,
- consumables,
- research/reference material,
- technician time.

Only use actual items/resources.

---

# 22. 173A Phase P — Resource Commitment

Prefer:

```text
reserve on production start
consume at defined milestone
refund rules on cancellation
```

Document transaction semantics.

---

# 23. 173A Phase Q — Equipment Requirements

Use real equipment tags:

```text
microphone
transmitter_access
recording_equipment
reference_material
emergency_encoder if required
```

Do not duplicate Plan 157 network hardware.

---

# 24. 173A Phase R — Equipment Ownership Boundary

Plan 157 may own communications hardware state.

Program production can require:

```text
hardware_available
```

through an interface.

It must not own/repair/range-calculate the transmitter.

---

# 25. 173A Phase S — Quality Model

Quality should be deterministic from:

```text
template base difficulty
presenter capability
preparation completeness
equipment quality/availability
script/research support
interruptions
broadcast-readiness state
```

No hidden network variables.

---

# 26. 173A Phase T — Quality Bands

Recommended:

```text
Poor
Functional
Good
Excellent
Exceptional
```

or source-aligned labels if repository already has quality vocabulary.

Use data thresholds.

---

# 27. 173A Phase U — Quality Numeric Score

If numeric 0–100 exists:

- keep bounded,
- derive band,
- persist final committed result for history.

Do not recalculate historical quality after presenter skill changes later.

---

# 28. 173A Phase V — Cancellation

Cancellation can happen due to:

- player action,
- missing slot,
- presenter unavailable,
- required equipment unavailable,
- network outage,
- emergency override,
- Plan 24 schedule conflict.

Outcome records explicit cancellation reason.

---

# 29. 173A Phase W — Cancellation Cost Policy

Define:

```text
preparation costs already spent remain spent
reserved unconsumed items release
airtime resource cost not consumed if never aired
```

unless content dictates otherwise.

---

# 30. 173A Phase X — Emergency Schedule Override

Emergency programs may have special Plan 24 slot priority if that system supports it.

Production system requests priority.

It does not implement its own emergency schedule.

---

# 31. 173A Phase Y — Delivery Binding Interface

Define:

```text
IRadioDeliveryResultSource
```

Input:

```text
schedule_slot_id / transmission_id
```

Output:

```text
authoritative delivery result
```

from Plan 157.

---

# 32. 173A Phase Z — Delivery Result Consumption

Production system may consume facts such as:

```text
transmitted?
received?
audience/recipient IDs?
partial reception?
intercepted?
jammed?
delivery confidence?
```

only if Plan 157 exposes them.

Do not infer unavailable fields.

---

# 33. 173A Phase AA — No Delivery Result, No Audience Resolution

If Plan 157 says:

```text
not transmitted
jammed
no recipient
```

then:

```text
audience response = none / delivery-failure response
```

No fake morale/faction impact for listeners who never received it.

---

# 34. 173A Phase AB — Propaganda Result Binding

Define:

```text
IRadioPropagandaResultSource
```

or consume Plan 168 result directly.

Store only reference/stable summary needed for history.

---

# 35. 173A Phase AC — Non-Propaganda Programs

News/education/entertainment/storytelling can resolve audience response without Plan 168 persuasion result.

But they still require Plan 157 delivery evidence.

---

# 36. 173A Phase AD — Program History

Create immutable outcome record:

```text
production_id
template_id
presenter_id
schedule_slot_id
status
quality_band
delivery_summary_ref
audience_response_summary
follow_up_ids
day
```

---

# 37. 173A Phase AE — History Is Not Schedule Authority

Storing:

```text
slot ID + historical air time
```

does not mean the history can reschedule/modify the slot later.

---

# 38. 173A Phase AF — Save State

Persist:

- in-progress productions if production spans save,
- final production history,
- unresolved follow-up hooks,
- stable external references,
- idempotency keys.

Do not persist:

- schedule catalog,
- station catalog,
- frequency catalog,
- delivery-network state,
- propaganda engine state.

---

# 39. 173A Phase AG — Old Save Compatibility

Missing section:

```text
valid
→ no production history
→ no unresolved follow-ups
```

Do not fabricate past shelter broadcasts.

---

# 40. 173A Phase AH — Idempotency Keys

Use:

```text
production_id
schedule_slot_id
delivery_result_id
audience_resolution_version
```

Audience response resolves once.

---

# 41. 173A Phase AI — Event Contracts

Candidate semantic events:

```text
radio_program_preparation_started
radio_program_ready
radio_program_scheduled
radio_program_cancelled
radio_program_aired
radio_program_delivery_resolved
radio_audience_response_resolved
radio_follow_up_created
radio_follow_up_resolved
```

---

# 42. 173A Phase AJ — Port Contract

Mandatory:

- Plan 24 schedule,
- Plan 157 delivery,
- survivor/presenter eligibility,
- inventory/equipment/work.

Conditional:

- Plan 168 for propaganda templates,
- faction reaction sink,
- morale sink,
- quest/follow-up sink.

Missing mandatory port fails validation.

---

# 43. 173A Phase AK — Diagnostics

Expose:

```text
RADIO_PROGRAM_TEMPLATES
RADIO_PRODUCTIONS_ACTIVE
RADIO_PRODUCTIONS_READY
RADIO_PROGRAMS_AIRED
RADIO_PROGRAMS_CANCELLED
RADIO_DELIVERY_RESULTS_BOUND
RADIO_AUDIENCE_RESULTS
RADIO_UNRESOLVED_FOLLOWUPS
RADIO_REQUIRED_PORTS_MISSING
```

---

# 44. 173A Tests

- template load,
- no forbidden schedule/frequency fields,
- eligible presenter,
- ineligible presenter,
- missing equipment,
- schedule reservation,
- schedule conflict,
- cancellation,
- quality determinism,
- save/load,
- no duplicate schedule ownership,
- old save,
- mandatory port failure.

---

# 45. 173A Definition of Done

- [ ] `RadioProgramProductionSystem`,
- [ ] typed template/runtime/outcome DTOs,
- [ ] 5 core program types,
- [ ] Plan 24 schedule-slot reference,
- [ ] no local station/frequency/schedule ownership,
- [ ] presenter eligibility,
- [ ] preparation work/cost,
- [ ] equipment validation,
- [ ] deterministic quality,
- [ ] cancellation semantics,
- [ ] Plan 157 delivery binding,
- [ ] Plan 168 propaganda binding,
- [ ] immutable production history,
- [ ] save/old-save,
- [ ] idempotency,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 46. Workstream 173B — Presenter/Audience Loop & Program Content

## Goal

Turn the production contract into a meaningful repeatable shelter activity with distinct program types, presenter roles, deterministic audience response, legible outcomes, and reusable follow-up opportunities.

---

# 47. 173B Phase A — Create `radio_programs.json`

Path:

```text
Assets/StreamingAssets/Data/radio_programs.json
```

Contains only player-program templates.

Forbidden content:

```text
station definitions
frequency definitions
schedule definitions
range values
jamming models
faction passive broadcasts
propaganda truth tables
```

---

# 48. 173B Phase B — Template Schema

Fields:

```text
program_id
program_type
name_key
description_key
duration_class
presenter_requirements
equipment_requirements
resource_costs
preparation_work
quality_weights
response_profile_id
propaganda_profile_id optional
follow_up_profiles
cooldown
tags
localization keys
```

---

# 49. 173B Phase C — Initial Content Budget

Recommended initial set:

```text
20–30 templates
```

balanced across the five source categories.

This is an integration plan recommendation, not a source requirement.

If content scope must remain smaller:

```text
minimum 3–4 meaningful templates per category
```

---

# 50. 173B Phase D — News Programs

Possible templates:

- daily shelter bulletin,
- verified wasteland news,
- casualty/recovery report,
- market/faction update,
- public-service notice.

News should use available canonical facts.

Do not fabricate offscreen world events.

---

# 51. 173B Phase E — News Truth Contract

News content generation may summarize:

- known events,
- archive entries,
- faction information,
- weather,
- trade notices.

Player must not broadcast information the shelter does not know unless program is fiction/propaganda and labeled accordingly.

---

# 52. 173B Phase F — Education Programs

Possible:

- radiation safety,
- sanitation,
- farming basics,
- repair tips,
- first aid,
- literacy/story-learning.

Mechanical effects should be bounded.

No instant survivor skill grants merely because one program aired.

---

# 53. 173B Phase G — Education Audience Response

Possible consequence:

- morale confidence,
- future contact,
- faction goodwill,
- tutorial/intel opportunity.

If skill/knowledge transfer to remote factions is ever mechanical, another explicit knowledge-transfer contract is needed.

---

# 54. 173B Phase H — Entertainment Programs

Possible:

- music hour,
- comedy/sketch,
- listener dedications,
- dramatized stories,
- survivor performances.

Use real presenter/performance traits where available.

---

# 55. 173B Phase I — Entertainment Morale

Morale effect:

- only for actual receiving audience if modeled,
- shelter-local morale can exist if shelter listens to its own broadcast and system supports it.

Avoid unconditional global morale gain.

---

# 56. 173B Phase J — Emergency Programs

Possible:

- radiation warning,
- evacuation alert,
- storm warning,
- disease notice,
- missing-person call.

Emergency program may receive Plan 24 schedule priority.

---

# 57. 173B Phase K — Emergency Accuracy

Emergency program content should reference real authoritative event state:

- Plan 135 weather,
- Plan 158 disaster,
- disease outbreak,
- shelter/faction emergency.

No false warning unless intentionally authored deception.

---

# 58. 173B Phase L — Storytelling Programs

Possible:

- survivor stories,
- shelter history,
- folklore,
- archive readings,
- memorial stories.

Can integrate with Plan 162 archive.

---

# 59. 173B Phase M — Storytelling Provenance

If based on real events:

- reference archive/journal/known facts.

If fictional:

- mark as fictional program.

---

# 60. 173B Phase N — Presenter Assignment Flow

UI flow:

```text
select program
→ show eligible presenters
→ show capability breakdown
→ show availability
→ assign
```

---

# 61. 173B Phase O — Presenter Capability Breakdown

Show:

- relevant skill,
- communication/leadership/performance,
- subject expertise,
- fatigue/health,
- prior radio experience if system supports it,
- equipment readiness.

---

# 62. 173B Phase P — Presenter Experience

If recurring presenter experience is desired:

Prefer:

```text
canonical skill XP / trait progression
```

not a second `radioExperience` meter.

---

# 63. 173B Phase Q — Multiple Presenters

Some templates may allow:

- host,
- guest,
- expert.

Only implement if schedule/production complexity remains manageable.

Each participant is a real survivor.

---

# 64. 173B Phase R — Guest Interviews

Potential:

- faction envoy,
- visitor,
- survivor specialist.

Requires actual presence/eligibility.

No offscreen guest teleportation.

---

# 65. 173B Phase S — Preparation Workflow

Preparation can include:

- research,
- script drafting,
- rehearsal,
- technical check.

Represent as one bounded production-work requirement.

Do not create four micro-management subsystems.

---

# 66. 173B Phase T — Production Quality Contributions

Suggested decomposition:

```text
presenter 35%
preparation 30%
equipment 20%
subject credibility 10%
context/interruptions 5%
```

Illustrative only.

Final weights data/config.

---

# 67. 173B Phase U — Quality Floor

A prepared program with valid equipment should not randomly become unusable.

Quality is deterministic.

---

# 68. 173B Phase V — Production Failure

Before airtime:

- presenter unavailable,
- equipment unavailable,
- slot cancelled,
- emergency override.

During airtime:

- delivery/network issues belong to Plan 157.

Keep distinction.

---

# 69. 173B Phase W — Audience Response Resolver

Create:

```text
RadioAudienceResponseResolver
```

Input:

```text
program template
quality result
presenter capability snapshot
Plan 157 delivery result
Plan 168 propaganda result optional
known audience/context
```

Output:

```text
response facts
```

---

# 70. 173B Phase X — No Independent Network

AudienceResponseResolver never asks:

- distance,
- frequency,
- antenna range,
- jammer power.

Those are already settled by Plan 157.

---

# 71. 173B Phase Y — Deterministic Audience Response

Use pure logic when possible.

If response diversity needs RNG:

- use seeded `ISeededRng`,
- key by production + recipient/audience + response profile,
- persist outcome.

Do not reroll on load.

---

# 72. 173B Phase Z — Audience Unit

Use whatever Plan 157 delivers:

- faction,
- settlement,
- listener group,
- contact,
- region.

Do not invent a finer-grained audience model if delivery result lacks it.

---

# 73. 173B Phase AA — Response Bands

Possible:

```text
NoResponse
Negative
Mixed
Positive
StrongPositive
TriggeredFollowUp
```

A program can produce multiple contextual responses if Plan 157 reports multiple recipients.

---

# 74. 173B Phase AB — Morale Signal

Audience response may emit:

```text
morale consequence intent
```

Canonical morale system applies final effect.

No local morale ledger.

---

# 75. 173B Phase AC — Reputation Signal

Response may emit:

```text
reputation/faction signal
```

Faction/reputation owner applies.

No local reputation number.

---

# 76. 173B Phase AD — Faction Reaction Hook

Examples:

- faction praises broadcast,
- faction objects,
- faction requests clarification,
- faction asks for airtime,
- faction issues rebuttal.

Plan 73 may later provide passive faction reply broadcast content.

Do not generate faction passive corpus here.

---

# 77. 173B Phase AE — Listener Contact

Follow-up hook example:

```text
listener_contact
```

Payload:

- source production,
- recipient context,
- contact type,
- expiry,
- opportunity target.

Quest/contact system owns final entity.

---

# 78. 173B Phase AF — Requested Follow-Up Broadcast

Hook:

```text
requested_follow_up_broadcast
```

Contains:

- requested program type/theme,
- requester ID,
- deadline,
- optional schedule constraints.

Plan 24 still owns slot reservation.

---

# 79. 173B Phase AG — Interview Request

Potential hook:

```text
guest_interview_request
```

Requires actual guest availability.

---

# 80. 173B Phase AH — Trade Opportunity

A successful practical/market broadcast may create:

```text
trade_contact
```

Market/trade system owns transaction.

---

# 81. 173B Phase AI — Rescue/Help Opportunity

Emergency call may create:

- rescue signal,
- contact,
- expedition opportunity.

Use canonical quest/expedition/signal systems.

---

# 82. 173B Phase AJ — Propaganda Response

If Plan 168 returns:

- morale persuasion,
- faction attitude effect,
- resistance,
- backlash,

RadioProgramProductionSystem records/hooks it.

It does not reinterpret propaganda math.

---

# 83. 173B Phase AK — Propaganda Quality Handoff

Production quality may be passed into Plan 168 as an input only if Plan 168 contract supports it.

Do not post-multiply Plan 168 results locally.

---

# 84. 173B Phase AL — Neutral Informational Programs

News/education should not be silently routed through propaganda because they influence reputation.

Use Plan 168 only where content is explicitly persuasive/propaganda according to its schema.

---

# 85. 173B Phase AM — Audience Explainability

For each response, expose reason codes:

```text
delivered_successfully
partial_reception
presenter_high_credibility
poor_production_quality
program_relevant_to_audience
propaganda_backlash
request_generated
```

---

# 86. 173B Phase AN — Response Timing

Some effects can be immediate:

- morale signal,
- acknowledgement.

Others become delayed follow-ups:

- contact,
- request,
- faction reply.

Do not force all outcomes in same tick.

---

# 87. 173B Phase AO — Follow-Up Lifecycle

Typed:

```text
Created
Available
Accepted
Expired
Resolved
Cancelled
```

---

# 88. 173B Phase AP — Follow-Up Expiry

Each follow-up declares:

- deadline,
- expiry policy,
- consequence of ignoring if any.

Persist.

---

# 89. 173B Phase AQ — Follow-Up Idempotency

Same audience response creates the same follow-up hook once.

---

# 90. 173B Phase AR — Follow-Up Ownership Transfer

When player accepts a follow-up:

```text
RadioFollowUpHook
→ Quest/Contact/Faction/Trade owner
```

Mark transferred/resolved.

Do not keep two active authorities.

---

# 91. 173B Phase AS — Radio Production UI

Panel sections:

```text
Program Library
In Preparation
Ready
Schedule Slot
Aired History
Audience Response
Follow-Ups
```

---

# 92. 173B Phase AT — Schedule UI Boundary

The panel may display Plan 24 schedule.

It must not maintain a separate editable local schedule.

All changes call Plan 24.

---

# 93. 173B Phase AU — Presenter UI

Show:

- eligibility,
- strengths,
- work cost,
- availability,
- estimated quality contribution.

---

# 94. 173B Phase AV — Equipment/Cost UI

Show:

- required equipment,
- missing items,
- prep resources,
- work time.

Use live canonical inventory.

---

# 95. 173B Phase AW — Quality Estimate

Before completion:

```text
estimated quality range/band
```

If deterministic and all inputs known, exact estimate may be shown.

Do not expose hidden audience response.

---

# 96. 173B Phase AX — Delivery Outcome UI

Use Plan 157 result labels.

Examples:

```text
Not Transmitted
Jammed
Partially Received
Delivered
Intercepted
```

only if those are actual Plan 157 semantics.

---

# 97. 173B Phase AY — Audience Outcome UI

Show:

- recipient/audience,
- response band,
- visible consequences,
- follow-up availability.

No hidden faction calculations.

---

# 98. 173B Phase AZ — Program History UI

History filter:

- program type,
- presenter,
- outcome,
- audience,
- day,
- follow-up generated.

---

# 99. 173B Phase BA — Journal / Archive

Journal may log notable broadcasts.

Plan 162 archive may record:

- first shelter broadcast,
- famous emergency transmission,
- landmark propaganda success/backlash,
- major listener contact.

No duplicate radio history authority.

---

# 100. 173B Phase BB — Audio/Presentation

If actual voice/audio generation/playback exists:

- presentation consumes the produced program result.

Do not require fully voiced audio to validate gameplay loop.

Text-only/subtitle representation remains valid.

---

# 101. 173B Phase BC — Accessibility

Program UI:

- keyboard/controller,
- no audio-only consequence,
- captions/transcripts,
- text delivery status,
- clear missing-equipment reasons.

---

# 102. 173B Phase BD — Localization

All automatic program/template/UI/response reason text uses localization keys.

Player-written/custom script text, if feature exists, remains user-authored.

---

# 103. 173B Phase BE — Custom Player Scripts Scope

The source says “player-authored program templates,” which can mean shelter-created programs from templates—not necessarily free-form text editing.

Do not add free-form script authoring unless explicitly desired.

Baseline:

```text
player chooses template/theme/presenter
```

---

# 104. 173B Phase BF — Template Content Validation

Validate:

- program ID,
- type,
- presenter requirements,
- equipment tags,
- resource IDs,
- response profile,
- propaganda profile if used,
- follow-up profile IDs,
- localization keys.

---

# 105. 173B Phase BG — Forbidden-Authority Data Gate

Fail if `radio_programs.json` contains fields named like:

```text
frequency
stationFrequency
broadcastSchedule
range
jamResistance
networkRadius
stationCatalog
```

unless clearly non-authoritative display metadata explicitly approved.

Prefer schema that simply disallows them.

---

# 106. 173B Phase BH — Utilization Report

Run representative broadcasts.

Report:

```text
templates loaded
templates prepared
templates aired
cancelled
delivered
not delivered
audience responses
follow-ups generated
propaganda programs
```

---

# 107. 173B Phase BI — Dead Template Policy

Never-used template:

- fix eligibility,
- mark intentionally rare,
- remove,
- exempt with reason.

---

# 108. 173B Definition of Done

- [ ] `radio_programs.json`,
- [ ] no schedule/frequency/network fields,
- [ ] news,
- [ ] education,
- [ ] entertainment,
- [ ] emergency,
- [ ] storytelling,
- [ ] presenter assignment,
- [ ] preparation workflow,
- [ ] resource/equipment costs,
- [ ] deterministic quality,
- [ ] Plan 157 audience delivery consumption,
- [ ] Plan 168 propaganda consumption,
- [ ] deterministic audience response,
- [ ] morale/reputation/faction hooks,
- [ ] listener contacts,
- [ ] requested follow-up broadcasts,
- [ ] follow-up lifecycle,
- [ ] radio production UI,
- [ ] schedule boundary preserved,
- [ ] delivery/audience outcome UI,
- [ ] history,
- [ ] journal/archive hooks,
- [ ] accessibility,
- [ ] localization,
- [ ] utilization report.

---

# 109. Workstream 173C — Cross-System Integration, Save/CI, Exploit Control, Balance, and Closure

## Goal

Prove radio production consumes the existing radio architecture correctly, preserves deterministic outcomes, does not duplicate schedule/network/propaganda authority, and creates meaningful but bounded social consequences.

---

# 110. 173C Phase A — Plan 24 Schedule Integration

All scheduling calls go through Plan 24.

Required tests:

- reserve free slot,
- reject occupied slot,
- release cancelled slot,
- mark aired,
- persist external reference.

---

# 111. 173C Phase B — No Duplicate Schedule Ownership Test

Static/architecture scan:

```text
RadioProgramProductionSystem
radio_programs.json
program UI
```

must not define:

```text
BroadcastSchedule
FrequencyCatalog
StationCatalog
```

or equivalents.

---

# 112. 173C Phase C — Plan 157 Delivery Integration

Every aired program must resolve against the authoritative delivery result.

No locally generated audience recipients.

---

# 113. 173C Phase D — Delivery Failure Cases

Test:

```text
jammed
hardware failure
no reachable audience
partial reception
intercepted
```

only to the extent Plan 157 exposes them.

Audience consequences must follow actual result.

---

# 114. 173C Phase E — Plan 168 Propaganda Integration

For propaganda program:

```text
production result
+ authoritative delivery
→ Plan 168
→ propaganda outcome
```

No duplicate influence math.

---

# 115. 173C Phase F — Plan 73 Passive Faction Broadcast Boundary

If faction sends reply:

- Plan 173 creates a reaction/follow-up hook,
- Plan 73 owns actual passive faction broadcast content.

---

# 116. 173C Phase G — Presenter Skill Integration

Relevant presenter skill comes from canonical skill system.

If presenter gains XP:

- route through SkillProgressionSystem.

No local radio skill ledger unless canonical skill registration is used.

---

# 117. 173C Phase H — Work/Duty Integration

Preparation and on-air work block/consume real survivor availability.

No survivor simultaneously on expedition and live broadcast.

---

# 118. 173C Phase I — Inventory/Equipment Integration

Preparation consumes/reserves actual resources.

Equipment availability/condition is live.

---

# 119. 173C Phase J — Equipment Failure Mid-Production

If required gear becomes unavailable before airtime:

- production may pause/cancel,
- slot may release or miss airtime.

Network transmission failure itself remains Plan 157.

---

# 120. 173C Phase K — Morale Integration

Audience response emits canonical morale consequence request.

No local morale state.

---

# 121. 173C Phase L — Reputation/Faction Integration

Faction reaction emits canonical standing/reputation reason.

No local radio reputation meter.

---

# 122. 173C Phase M — Quest/Opportunity Integration

Follow-up hook transfers once into:

- quest,
- contact,
- trade,
- expedition,
- faction request.

---

# 123. 173C Phase N — Save/Load Matrix

Test saves at:

```text
draft
preparing
ready
scheduled
cancelled
aired waiting delivery
delivery resolved waiting audience
audience resolved
follow-up pending
follow-up transferred
```

Exact lifecycle persists.

---

# 124. 173C Phase O — Schedule-Slot Idempotency

Reload cannot:

- reserve same slot twice,
- free another program’s slot,
- duplicate aired record.

Use stable slot/production IDs.

---

# 125. 173C Phase P — Delivery Idempotency

Same Plan 157 delivery result:

```text
consumed once
```

---

# 126. 173C Phase Q — Audience Idempotency

Same delivery/propaganda result:

```text
audience response resolved once
```

---

# 127. 173C Phase R — Follow-Up Idempotency

Same audience result cannot create duplicate listener contacts/requests.

---

# 128. 173C Phase S — Save-Scum Prevention

Reload cannot reroll:

- program quality,
- delivery result,
- propaganda result,
- audience response,
- follow-up generation.

---

# 129. 173C Phase T — UI Refresh Exploit

Opening/closing production panel never changes:

- prep progress,
- quality,
- slot,
- delivery,
- response,
- follow-up.

---

# 130. 173C Phase U — Cancellation Exploit

Prevent:

```text
prepare expensive program
learn likely bad timing/outcome
cancel
recover all spent resources
repeat
```

Costs already consumed stay consumed according to policy.

---

# 131. 173C Phase V — Slot-Hoarding Exploit

Player should not reserve unlimited future schedule slots if Plan 24 disallows it.

Respect schedule policy.

---

# 132. 173C Phase W — Presenter Farming Exploit

No repeated low-value broadcasts granting unlimited skill/reputation.

If presenter XP exists:

- per-day/novelty cap,
- quality threshold,
- diminishing returns.

---

# 133. 173C Phase X — Reputation Farming Exploit

Repeated identical broadcast to same faction:

- response novelty decays,
- standing reward limited,
- no infinite standing.

Faction system should own the final cap if available.

---

# 134. 173C Phase Y — Follow-Up Spam Guard

Cap:

- unresolved follow-ups,
- same-profile duplicate requests,
- repeated faction requests.

Oldest/low-value may expire according to policy.

---

# 135. 173C Phase Z — Program Cooldowns

Templates may declare cooldown.

Use for:

- emergency broadcasts,
- repeated entertainment,
- propaganda campaigns.

Do not hardcode globally unless justified.

---

# 136. 173C Phase AA — Emergency Broadcast Priority Test

If Plan 24 supports emergency priority:

- emergency program can preempt/replace according to its rules,
- displaced program receives deterministic cancellation/reschedule outcome.

Production system does not own priority queue.

---

# 137. 173C Phase AB — No Schedule Slot Edge Case

If no slot available:

- program remains Ready or expires/cancels by template policy,
- no fake broadcast.

---

# 138. 173C Phase AC — No Presenter Edge Case

Program cannot begin or cannot complete according to lifecycle.

No auto-generated anonymous host unless template explicitly allows automation.

---

# 139. 173C Phase AD — Missing Equipment Edge Case

Fail preparation/readiness clearly.

No silent quality penalty in place of mandatory gear unless template marks equipment optional.

---

# 140. 173C Phase AE — No Audience Edge Case

Aired successfully but no audience delivered:

- history records airtime,
- no social/faction response,
- no listener follow-up.

---

# 141. 173C Phase AF — Partial Delivery Edge Case

Only actual delivered audience subset can respond.

---

# 142. 173C Phase AG — Propaganda-Resistant Audience

Plan 168 may return resistance/backlash.

Program production records it as authoritative.

No second persuasion chance.

---

# 143. 173C Phase AH — High-Quality Non-Propaganda Program

High quality can increase:

- positive response likelihood/band,
- contact quality,
- reputation signal,

only through the audience-response profile.

No propaganda effect unless flagged.

---

# 144. 173C Phase AI — Poor-Quality Broadcast

Possible:

- mixed/negative response,
- no follow-up,
- reputation caution.

But avoid guaranteed catastrophic penalty from one poor entertainment show.

---

# 145. 173C Phase AJ — Emergency Accuracy Failure

If player schedules an emergency program after event state changed:

- content validity/relevance may reduce response,
- do not rewrite the source event.

---

# 146. 173C Phase AK — Same-Seed Determinism

Same:

```text
campaign seed
program template
presenter
preparation
slot
authoritative delivery
authoritative propaganda outcome
```

→ same audience response/follow-ups.

---

# 147. 173C Phase AL — Headless Selftest

Create:

```text
--radio-program-production-selftest
```

if project naming allows.

Source verification only requires `--bridge-selftest`, but a dedicated verb is useful.

If adding a new CLI verb is too much, fold into bridge selftest with explicit section.

---

# 148. 173C Phase AM — Required Selftest Scenarios

1. valid news program,
2. missing presenter,
3. ineligible presenter,
4. missing equipment,
5. slot reservation,
6. slot conflict,
7. cancellation,
8. delivered non-propaganda program,
9. undelivered program,
10. propaganda program via Plan 168,
11. deterministic audience response,
12. follow-up creation,
13. save/load mid-prep,
14. save/load after airing,
15. duplicate delivery/result replay,
16. old save.

---

# 149. 173C Phase AN — Data Integrity

Validate:

- template IDs,
- program types,
- presenter requirements,
- equipment tags,
- resource IDs,
- response profile IDs,
- propaganda profile IDs,
- follow-up profile IDs,
- localization keys,
- forbidden schedule/network fields absent.

---

# 150. 173C Phase AO — Deliberate Failure Proof

Break:

- nonexistent presenter requirement,
- invalid equipment tag,
- forbidden `frequency` field,
- missing Plan 24 port,
- duplicate audience resolution.

Assert relevant CI gate fails.

---

# 151. 173C Phase AP — 100-Day Production Soak

Run representative programming cadence.

Record:

```text
programs prepared
aired
cancelled
delivered
not delivered
audience responses
follow-ups
faction reactions
morale signals
presenter labor
resource cost
```

---

# 152. 173C Phase AQ — Program Mix Profiles

Run:

```text
news_heavy
education_heavy
entertainment_heavy
emergency_only
storytelling_heavy
balanced
propaganda_heavy
```

Compare:

- audience engagement,
- faction reactions,
- morale value,
- resource/labor burden,
- follow-up generation.

---

# 153. 173C Phase AR — Avoid Dominant Program Type

No one template/category should dominate every outcome.

Examples:

- entertainment → morale/contact,
- news → reputation/info trust,
- education → practical goodwill,
- emergency → situational high value,
- storytelling → identity/morale/legacy,
- propaganda → persuasion with backlash risk.

---

# 154. 173C Phase AS — Presenter Opportunity Cost

Measure:

- work-days consumed,
- missed duties,
- quality gain.

Radio should be a real strategic commitment.

---

# 155. 173C Phase AT — Resource Cost Balance

Production costs should not make basic radio use impossible.

High-impact/propaganda/emergency programs may cost more.

---

# 156. 173C Phase AU — Follow-Up Value Test

At least one program category should produce meaningful follow-up opportunities under plausible conditions.

No decorative hook system.

---

# 157. 173C Phase AV — Delivery vs Quality Separation Test

Test:

```text
excellent production + no delivery
→ no audience effect

poor production + successful delivery
→ audience effect based on poor quality

excellent production + successful delivery
→ stronger response
```

This proves correct authority split.

---

# 158. 173C Phase AW — Propaganda Separation Test

Test:

```text
same quality
same delivery
different Plan 168 result
→ different propaganda-specific audience outcome
```

Program system does not recompute it.

---

# 159. 173C Phase AX — Schedule Ownership Audit

Search repository for:

```text
new BroadcastSchedule
schedule ownership fields
frequency tables
station catalogs
```

inside Plan 173 scope.

Expected:

```text
zero unauthorized definitions
```

---

# 160. 173C Phase AY — Network Ownership Audit

Search Plan 173 code/data for:

```text
rangeKm
jamStrength
encryption
interceptionChance
frequency
antennaPower
```

Expected:

```text
no local network calculations
```

unless merely reading external result metadata.

---

# 161. 173C Phase AZ — Propaganda Ownership Audit

Search for:

```text
persuasionChance
propagandaEffect
beliefShift
```

Expected:

```text
no local effect-resolution logic
```

outside adapter/result consumption.

---

# 162. 173C Phase BA — Passive Faction Content Audit

No faction broadcast scripts/templates added to `radio_programs.json`.

Plan 73 remains owner.

---

# 163. 173C Phase BB — UI Runtime Parity

Displayed:

- prep progress,
- quality,
- cost,
- slot,
- delivery,
- response,
- follow-up status

must match runtime.

---

# 164. 173C Phase BC — Accessibility

Radio-production panel:

- keyboard/controller,
- text status,
- no audio-only outcomes,
- transcript/caption support,
- clear focus order.

---

# 165. 173C Phase BD — Headless Behavior

Preparation, airing state handoff, delivery consumption, audience resolution, and follow-up expiry work without UI.

---

# 166. 173C Phase BE — Retention

Use Plan 55:

Keep:

- recent production history,
- landmark broadcasts,
- major faction responses,
- famous emergency transmissions,
- unresolved follow-ups.

Roll up:

- repetitive low-value routine broadcasts.

---

# 167. 173C Phase BF — Archive / Legacy

Plan 162 may record:

- first shelter broadcast,
- landmark emergency message,
- influential program,
- famous presenter,
- major listener contact.

Archive remains consumer.

---

# 168. 173C Phase BG — Release Metrics

Report:

```text
templates total
programs prepared
programs aired
delivery success rate
cancel rate
mean quality
audience response distribution
follow-up conversion rate
faction reaction count
duplicate schedule violations
```

---

# 169. 173C Phase BH — Documentation

Create:

```text
docs/systems/RADIO_PROGRAM_PRODUCTION.md
```

Include:

- ownership boundaries,
- template schema,
- presenter rules,
- preparation lifecycle,
- schedule-slot interface,
- delivery interface,
- propaganda interface,
- audience-response rules,
- follow-up lifecycle,
- save/retention behavior.

---

# 170. 173C Definition of Done

- [ ] Plan 24 schedule integration,
- [ ] no duplicate schedule ownership,
- [ ] Plan 157 delivery integration,
- [ ] delivery failure/partial cases,
- [ ] Plan 168 propaganda integration,
- [ ] Plan 73 passive-content boundary,
- [ ] presenter skill/work integration,
- [ ] inventory/equipment integration,
- [ ] morale integration,
- [ ] faction/reputation integration,
- [ ] follow-up transfer integration,
- [ ] save/load lifecycle matrix,
- [ ] schedule/delivery/audience/follow-up idempotency,
- [ ] save-scum prevention,
- [ ] UI refresh no-op,
- [ ] cancellation/slot/presenter/reputation exploit guards,
- [ ] no-slot/no-presenter/no-equipment/no-audience edge cases,
- [ ] same-seed determinism,
- [ ] selftest/bridge-selftest coverage,
- [ ] data integrity,
- [ ] failure proof,
- [ ] 100-day soak,
- [ ] program mix profiles,
- [ ] quality/delivery/propaganda separation tests,
- [ ] ownership audits,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] headless,
- [ ] retention,
- [ ] archive/legacy,
- [ ] docs.

---

# 171. Integrated Radio Production Pipeline

```text
player selects program template
        │
        ▼
presenter + equipment + prep resources
        │
        ▼
production quality committed
        │
        ▼
request Plan 24 schedule slot
        │
        ▼
broadcast airs or cancels
        │
        ▼
consume Plan 157 delivery result
        │
        ├─ no delivery → close with no audience effect
        │
        └─ delivered
              │
              ├─ propaganda? → consume Plan 168 result
              │
              ▼
        audience-response resolver
              │
      ┌───────┼──────────┬───────────┐
      ▼       ▼          ▼           ▼
   morale  reputation  faction   follow-up hook
              │
              ▼
       canonical downstream owner
```

---

# 172. Schedule Authority Contract

Plan 24 owns:

- stations,
- frequencies,
- schedule,
- airtime slot availability,
- slot priority/override.

Plan 173 stores references only.

---

# 173. Delivery Authority Contract

Plan 157 owns:

- range,
- who can receive,
- jamming,
- interception,
- encryption,
- delivery result.

Plan 173 consumes result only.

---

# 174. Propaganda Authority Contract

Plan 168 owns:

- message truth,
- propaganda eligibility,
- persuasion/effect calculation,
- backlash/resistance if modeled.

Plan 173 consumes result only.

---

# 175. Passive Faction Broadcast Contract

Plan 73 owns passive faction broadcast corpus/content.

Plan 173 may trigger a response hook but not author the faction corpus.

---

# 176. Program Authority Contract

Plan 173 owns:

- player-program templates,
- production lifecycle,
- presenter assignment,
- prep work,
- production quality,
- production history,
- audience-response projection,
- unresolved follow-ups.

---

# 177. Presenter Contract

Presenter is a canonical survivor.

Eligibility and capability derive from:

- skill,
- trait,
- profession,
- health,
- availability.

---

# 178. Equipment Contract

Required production gear/resources are canonical items or external hardware-readiness facts.

No duplicate radio hardware state.

---

# 179. Quality Contract

Quality is a production fact.

Network delivery does not alter recorded production quality.

---

# 180. Delivery/Quality Separation Contract

```text
quality answers:
"how good was the program?"

delivery answers:
"who actually received it?"
```

Do not conflate.

---

# 181. Audience Contract

Audience response only considers recipients delivered by Plan 157.

---

# 182. Reputation Contract

Audience response can emit a reputation/faction signal.

Faction/reputation owner applies the durable state.

---

# 183. Morale Contract

Audience response can emit morale consequences.

Morale owner applies them.

---

# 184. Follow-Up Contract

A follow-up is:

```text
opportunity metadata
```

until transferred to the owning system.

---

# 185. Save Contract

Persist:

- in-progress production,
- production history,
- unresolved follow-ups,
- idempotency/external refs.

Do not persist:

- radio schedule,
- station/frequency catalog,
- communication-network state,
- propaganda engine state.

---

# 186. Old-Save Contract

Old saves start with:

```text
no shelter-produced program history
```

unless a safe historical import exists.

---

# 187. Determinism Contract

Same authoritative inputs:

```text
template
presenter
preparation
quality
slot
delivery
propaganda result
```

→ same audience response and follow-ups.

---

# 188. Idempotency Contract

Each stage commits once:

```text
slot reservation
quality finalization
airing
delivery binding
audience resolution
follow-up creation
```

---

# 189. UI Contract

Radio-production UI is a controller/read model over real systems.

It does not own schedule, delivery, or audience truth.

---

# 190. Retention Contract

Routine program history may roll up.

Preserve:

- landmark broadcasts,
- major responses,
- famous presenters,
- unresolved follow-ups.

---

# 191. Content Acceptance Contract

Program templates progress through:

```text
AUTHORED
→ LOADS
→ PRESENTER_ELIGIBLE
→ PREPARED
→ SCHEDULED
→ AIRED
→ DELIVERY_BOUND
→ AUDIENCE_RESPONSE_RESOLVED
→ FOLLOW_UP/CONSEQUENCE_PRODUCED
→ PLAYER_VISIBLE
```

---

# 192. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| Plan 173 creates a second schedule | Medium | Critical | slot-reference-only architecture |
| local range/jamming calculations drift from Plan 157 | Medium | Critical | delivery-result-only consumption |
| propaganda effects duplicated locally | Medium | Critical | Plan 168 result adapter only |
| faction passive content leaks into player-program catalog | Medium | High | schema/content gate |
| audience response fires without delivery | Medium | High | hard dependency on delivery result |
| presenter becomes a free no-cost role | Medium | Medium | real work/availability |
| repeated broadcasts farm standing | High | High | novelty/cooldown/faction caps |
| follow-up hooks spam campaign | Medium | Medium | unresolved cap/expiry |
| save/load duplicates slot or response | Medium | High | stage idempotency keys |
| high-quality program guarantees success despite jamming | Medium | High | delivery/quality separation |
| UI maintains shadow schedule | Medium | High | all schedule actions call Plan 24 |
| history growth unbounded | Medium | Medium | Plan 55 retention |

---

# 193. Commit Strategy

## 173A — Production Contract

### C2[37].1 — baseline + ownership ADR

### C2[37].2 — program template/runtime/outcome DTOs

### C2[37].3 — radio_programs.json schema

### C2[37].4 — presenter eligibility / work integration

### C2[37].5 — resource/equipment/preparation lifecycle

### C2[37].6 — deterministic quality

### C2[37].7 — Plan 24 schedule-slot adapter

### C2[37].8 — Plan 157 delivery adapter

### C2[37].9 — Plan 168 propaganda adapter

### C2[37].10 — save/old-save/idempotency

### C2[37].11 — events/ports/diagnostics

### Gate: 173A complete

---

## 173B — Presenter / Audience / Content

### C2[37].12 — news templates

### C2[37].13 — education templates

### C2[37].14 — entertainment templates

### C2[37].15 — emergency templates

### C2[37].16 — storytelling templates

### C2[37].17 — presenter UI / production prep

### C2[37].18 — audience-response resolver

### C2[37].19 — morale/reputation/faction hooks

### C2[37].20 — follow-up lifecycle

### C2[37].21 — radio production/history UI

### C2[37].22 — localization/accessibility/journal

### C2[37].23 — content-utilization report

### Gate: 173B complete

---

## 173C — Integration / Validation

### C2[37].24 — Plan 24 schedule integration tests

### C2[37].25 — Plan 157 delivery integration tests

### C2[37].26 — Plan 168 propaganda separation tests

### C2[37].27 — Plan 73 passive-content boundary audit

### C2[37].28 — skill/work/inventory integration

### C2[37].29 — morale/faction/follow-up transfer

### C2[37].30 — save-load/idempotency matrix

### C2[37].31 — exploit/edge-case suite

### C2[37].32 — data-integrity / forbidden-field gate

### C2[37].33 — bridge/dedicated selftest

### C2[37].34 — 100-day production soak

### C2[37].35 — program-mix / presenter-cost / response balance

### C2[37].36 — ownership audits / UI parity / retention

### C2[37].37 — docs/playtest/release closure

### Gate: 173C complete

---

# 194. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Recommended additional repository-canonical checks:

```text
radio-program template integrity
forbidden schedule/network-field scan
schedule ownership static audit
delivery/quality separation tests
propaganda ownership static audit
old-save fixture load
same-seed audience-response replay
100-day program-production soak
radio-program UI accessibility/runtime parity
```

If adding a dedicated verb is consistent with current CLI style:

```bash
godot --headless --path . -- --radio-program-production-selftest
```

Otherwise keep all scenarios under `--bridge-selftest`.

---

# 195. Flagship Definition of Done

## 173A — Production Contract

- [ ] `RadioProgramProductionSystem`,
- [ ] player-program template schema,
- [ ] one external schedule-slot reference,
- [ ] no frequency/station/schedule authority,
- [ ] presenter assignment,
- [ ] presenter eligibility,
- [ ] work/preparation cost,
- [ ] equipment/resource checks,
- [ ] deterministic quality,
- [ ] cancellation,
- [ ] Plan 157 delivery consumption,
- [ ] Plan 168 propaganda consumption,
- [ ] production history,
- [ ] unresolved follow-up state,
- [ ] save/old-save,
- [ ] idempotency,
- [ ] ports/events/diagnostics.

## 173B — Presenter / Audience Loop

- [ ] news programs,
- [ ] education programs,
- [ ] entertainment programs,
- [ ] emergency programs,
- [ ] storytelling programs,
- [ ] template-driven presenter requirements,
- [ ] real preparation workflow,
- [ ] audience-response resolver,
- [ ] response only after delivery evidence,
- [ ] morale signals,
- [ ] reputation signals,
- [ ] faction reaction hooks,
- [ ] listener contacts,
- [ ] requested follow-up broadcasts,
- [ ] follow-up expiry/transfer,
- [ ] production UI,
- [ ] history UI,
- [ ] schedule UI uses Plan 24,
- [ ] delivery UI uses Plan 157,
- [ ] propaganda UI uses Plan 168 result,
- [ ] accessibility/localization,
- [ ] utilization report.

## 173C — Integration / Validation

- [ ] one schedule authority remains,
- [ ] one communications authority remains,
- [ ] one propaganda authority remains,
- [ ] Plan 73 passive corpus remains separate,
- [ ] skill/work integration,
- [ ] inventory/equipment integration,
- [ ] morale integration,
- [ ] faction/reputation integration,
- [ ] quest/follow-up integration,
- [ ] lifecycle save/load matrix,
- [ ] slot/delivery/audience/follow-up idempotency,
- [ ] save-scum prevention,
- [ ] cancellation exploit guard,
- [ ] slot-hoarding guard,
- [ ] presenter/reputation farm guards,
- [ ] no-slot/no-presenter/no-equipment/no-audience cases,
- [ ] same-seed determinism,
- [ ] data integrity,
- [ ] forbidden-field gate,
- [ ] deliberate failure proof,
- [ ] selftest/bridge-selftest,
- [ ] 100-day soak,
- [ ] program-mix profiles,
- [ ] delivery-vs-quality separation,
- [ ] propaganda separation,
- [ ] ownership audits,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] headless,
- [ ] retention,
- [ ] archive/legacy,
- [ ] docs.

## Global

- [ ] no second `BroadcastSchedule`,
- [ ] no local station/frequency catalog,
- [ ] no local range/jamming/interception logic,
- [ ] no local propaganda effect resolver,
- [ ] no passive faction broadcast duplication,
- [ ] no audience effect without Plan 157 delivery,
- [ ] no duplicate social/reputation state,
- [ ] no save/load reroll or duplicate slot,
- [ ] full verification green.

---

# 196. Closure Report Template

```markdown
## C2[37] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Ownership Baseline
- Plan 24 schedule type:
- Plan 24 slot reservation API:
- Plan 157 delivery result:
- Plan 168 propaganda result:
- Plan 73 passive faction corpus:
- Presenter skill source:
- Inventory/equipment source:
- Faction/reputation sink:
- Morale sink:

### 173A — Production Contract
- RadioProgramProductionSystem:
- Program types:
- Template count:
- Presenter requirements:
- Equipment requirements:
- Preparation work:
- Quality model:
- Plan 24 adapter:
- Plan 157 adapter:
- Plan 168 adapter:
- Save schema:
- Old-save behavior:
- Missing ports:
- Result:

### 173B — Presenter / Audience
- News templates:
- Education templates:
- Entertainment templates:
- Emergency templates:
- Storytelling templates:
- Productions prepared:
- Productions aired:
- Deliveries bound:
- Audience responses:
- Morale signals:
- Reputation signals:
- Faction hooks:
- Listener contacts:
- Requested follow-ups:
- UI:
- Unused templates:
- Result:

### 173C — Integration / Validation
- Schedule ownership violations:
- Network ownership violations:
- Propaganda ownership violations:
- Passive-faction-content violations:
- Save-load duplicates:
- Slot duplicates:
- Audience duplicates:
- Follow-up duplicates:
- Cancellation exploit:
- Reputation farm findings:
- Same-seed replay:
- 100-day soak:
- Program mix balance:
- Presenter labor cost:
- UI parity:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge selftest:
- Radio-program selftest:
- Forbidden-field scan:
- Ownership audits:
- Old-save fixtures:
- Same-seed replay:
- Production soak:
- UI/accessibility:
- Result:

### Final Metrics
- RADIO_PROGRAM_TEMPLATES:
- RADIO_PRODUCTIONS_STARTED:
- RADIO_PROGRAMS_AIRED:
- RADIO_PROGRAMS_CANCELLED:
- RADIO_DELIVERY_SUCCESS:
- RADIO_DELIVERY_FAILURE:
- RADIO_AUDIENCE_RESPONSES:
- RADIO_FOLLOWUPS_CREATED:
- RADIO_FOLLOWUPS_RESOLVED:
- RADIO_FACTION_REACTIONS:
- RADIO_MORALE_SIGNALS:
- DUPLICATE_SLOT_RESERVATIONS:
- DUPLICATE_AUDIENCE_RESOLUTIONS:
- UNAUTHORIZED_SCHEDULE_TYPES:
- UNAUTHORIZED_NETWORK_LOGIC:
- UNAUTHORIZED_PROPAGANDA_LOGIC:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Program content:
- Presenter skill coverage:
- Follow-up variety:
- Audio presentation:
- Faction replies:
- UI:
```

---

# 197. Final Execution Directive

Execute Plan 173 as a **program-production and audience-response layer over the existing unified radio architecture**.

The critical sequence is:

```text
load player-program templates
→ select real presenter
→ consume real prep time/resources/equipment
→ reserve a real Plan 24 schedule slot
→ commit deterministic production quality
→ air or cancel
→ consume authoritative Plan 157 delivery result
→ if persuasive, consume authoritative Plan 168 propaganda result
→ resolve deterministic audience response
→ route morale/reputation/faction effects to canonical owners
→ create durable follow-up opportunities
→ retain production history without duplicating schedule/network state
```

Do not create a second broadcast schedule.

Do not own frequencies or stations.

Do not calculate range, interception, encryption, or jamming.

Do not reimplement propaganda.

Do not author passive faction-radio corpus.

Do not resolve audience response for recipients Plan 157 did not deliver to.

The strongest authority rule is:

> **Plan 173 owns what the shelter produces and how well it produces it; Plan 24 decides when it airs, Plan 157 decides who receives it, and Plan 168 decides propaganda truth/effects.**

The strongest audience rule is:

> **Audience response is a deterministic consequence of a real delivered broadcast, production quality, presenter capability, and authoritative propaganda/faction context—not an independent radio network simulation.**

The strongest save rule is:

> **Persist only production history, in-progress production state where necessary, unresolved follow-ups, and stable references; never persist a shadow schedule, network, or propaganda ledger.**

The flagship acceptance scenario is:

> **Prepare one education program and one propaganda-tagged emergency program using two different presenters. The education program reserves a real Plan 24 slot, consumes actual preparation resources, airs at committed quality, receives a Plan 157 partial-delivery result, and generates audience response only for the delivered recipients. The propaganda program reserves a later Plan 24 slot, receives a Plan 157 successful-delivery result, consumes the Plan 168 propaganda outcome, and produces a faction reaction plus one requested follow-up broadcast. Save/load once during preparation and once after airing but before audience resolution. Re-run the same state: quality, slot ownership, delivery binding, propaganda result reference, audience response, and follow-up IDs must remain identical, with zero duplicate schedule reservations and zero locally computed range/propaganda effects.**
