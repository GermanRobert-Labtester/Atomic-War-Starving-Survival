# C1 — Flagship Integration Plan [29]: Propaganda, Information Warfare & Credibility

> **Output:** `C1_planintegration[29].md`
>
> **Source baseline:** Plan 168 — Propaganda & Morale Warfare System
>
> **Primary mission:** give the player a deliberate information-warfare capability—print, broadcast, seed, and coordinate persuasive messages—while preserving the existing authorities for printing, radio infrastructure, rumors/intelligence, faction standing, survivor morale, moral choice, expeditions, trade, quests, and narrative events.
>
> **Primary architectural rule:** propaganda owns **authored message intent, campaign coordination, delivery requests, attribution state, credibility history, and outcome orchestration**. It does not own faction morale, faction stance, recruitment, defections, refugee flow, market prices, survivor morale, rumor propagation, or radio hardware state.
>
> **Primary information rule:** exposure, attribution, credibility, and audience knowledge are first-class. A message can be delivered without being attributed, attributed without being believed, believed without changing faction standing, or backfire because existing information/faction systems react to it.
>
> **Mandatory execution order:** 168A authority audit + message/campaign contract → 168B production and distribution adapters → 168C credibility, attribution and effectiveness projection → 168D faction/moral/rumor consequences through existing authorities → 168E UI, persistence, deterministic hardening, anti-spam and balance → 168F counter-propaganda only as an adapter onto existing faction/radio/rumor systems → 168G advanced psyops/defection/intercept mechanics only where real consumers exist.
>
> **Critical re-baseline rule:** before creating `PropagandaSystem`, inspect `PaperPrintingCatalog`, `VerdictRadioSystem`, current radio/transmitter infrastructure, Plan-131 rumor/intelligence, Plan-139 combat→faction politics, Plan-147 NPC memory, Plan-155 underground information channels, Plan-159 governance/public-policy history, `FactionBranchCoordinator`, `FactionStanceEngine`, `MoralChoiceSystem`, survivor morale/mental-health state, quest prerequisites, expedition delivery hooks, and journal/narrative event buses. New propaganda code coordinates those systems rather than duplicating them.
>
> **Critical scope correction:** the source proposes direct “faction morale” mutation with secondary effects such as reduced recruitment, lower trade prices, defectors, aggression, allies, and refugee flow. Those are separate authorities and may not exist as a single faction-morale system. The flagship therefore treats propaganda as a **cause/evidence layer** and only writes to a real downstream consumer if that consumer is proven in the repository.
>
> **Guardrails:** no second faction-standing system; no generic faction-morale number unless an existing canonical faction morale/cohesion authority already exists; no direct trade-price mutation from propaganda code; no direct refugee spawn modifier; no automatic defector spawning; no direct survivor-morale mutation unless targeting the shelter through an existing morale adapter; no second rumor network; no player-written free-text propaganda required for MVP; no LLM/runtime content generation requirement; no `effectiveness += X each day` detached from actual delivery; no unseeded RNG; no wall-clock/GUID; no global omniscience; no daily detection rolls for inactive campaigns; no giant five-channel feature set unless each channel has a real infrastructure/delivery seam; no “broadcast intercept” if the radio system cannot represent frequency access/control; no counter-propaganda simulator if existing faction AI cannot author/send information events; no generic propaganda-as-mind-control mechanic.

---

# 0. Mission

ASHFALL already has fragments of information warfare.

The source baseline identifies:
- `StencilPropagandaSmearEntry` in `PaperPrintingCatalog.cs`;
- scripted faction broadcasts in `VerdictRadioSystem.cs`;
- radio census/broadcast narrative content;
- psychological contamination that affects survivors from environments;
- faction standing/trust;
- rumor and intelligence systems from later plans.

What is missing is a player-facing, systemic bridge:

```text
PLAYER INTENT
   │
   ▼
AUTHOR MESSAGE
   │
   ├── medium
   ├── truthfulness
   ├── theme
   ├── target
   ├── audience
   ├── author
   └── objective
   │
   ▼
PRODUCE / DISTRIBUTE
   │
   ├── print
   ├── radio
   ├── physical posting
   ├── rumor seeding
   └── frequency hijack if real
   │
   ▼
INFORMATION OUTCOME
   │
   ├── reached audience?
   ├── attributed?
   ├── believed?
   ├── contradicted?
   ├── exposed as deceptive?
   └── amplified/countered?
   │
   ▼
CANONICAL CONSEQUENCE AUTHORITIES
   │
   ├────────► rumor/intelligence
   ├────────► faction standing/stance
   ├────────► moral choice
   ├────────► NPC memory/dialogue
   ├────────► quest conditions
   ├────────► trade/economy if explicit adapter exists
   ├────────► refugee/admission if explicit adapter exists
   ├────────► recruitment/cohesion if explicit authority exists
   └────────► journal/archive/epilogue
```

That is the right abstraction.

The propaganda layer should answer:

> What message did we create, how did we try to deliver it, who received it, who thinks we authored it, how credible was it, and which existing systems now have enough evidence to react?

It should not answer:

> What does faction morale equal?
> How many defectors spawn?
> What trade price is set?
> Which refugees arrive?
> Who becomes hostile?
> What exact radio waveform was hijacked?

Those belong elsewhere.

---

# 1. Re-Baseline Against Existing Information Systems

## 1.1 Paper printing is already a production/content precedent

`StencilPropagandaSmearEntry` proves the repository already has propaganda-shaped narrative data.

Plan 168 should first determine:
- whether that type can be generalized;
- whether it is narrative-only;
- whether it should remain as a content record consumed by a new system.

Do not create duplicate “smear” definitions with a second schema if one existing schema can be safely extended.

## 1.2 Radio infrastructure already exists

`VerdictRadioSystem` is scripted, but it is still valuable because it may already provide:
- frequency;
- transmitter;
- reception;
- broadcast event;
- player radio context.

Player propaganda should route through that infrastructure where possible.

## 1.3 Rumor/intelligence likely owns slow social spread

The source’s “rumor campaign” should almost certainly be:
- a specialized input into Plan 131,
not a new propagation model.

## 1.4 Faction standing is not propaganda effectiveness

A successful hostile broadcast may:
- lower trust;
- increase hostility;
- increase suspicion;
- produce no immediate stance change;
depending on faction rules.

Do not conflate reach/credibility with standing delta.

## 1.5 Faction morale may not exist as a single authority

The source assumes a `faction morale` state.

Audit for:
- cohesion;
- recruitment;
- war support;
- willingness to defect;
- civilian allegiance;
- leadership legitimacy.

If no canonical state exists:
- do not add a generic faction-morale meter solely for propaganda.

## 1.6 Psychological contamination is not psyops

`PsychologicalContaminationSystem` deals with trauma from environments.

Do not wire propaganda into it merely because both are “psychological.”

## 1.7 Counter-propaganda is a separate actor capability

If factions can already:
- broadcast;
- seed rumors;
- create events,
they can produce counter-messages using the same information rails.

If not:
- defer full counter-propaganda campaigns.

---

# 2. Non-Negotiable Information-Warfare Invariants

## INV-168.1 — One information-propagation authority per channel

Radio uses radio.
Rumor uses rumor/intelligence.
Physical distribution uses expedition/encounter.
Printing uses printing/inventory.

## INV-168.2 — Message creation does not equal delivery

A drafted leaflet sitting in the shelter has no external effect.

## INV-168.3 — Delivery does not equal belief

Reach, attribution, credibility and persuasion are separate.

## INV-168.4 — Belief does not equal faction standing

Downstream faction effects use explicit rules.

## INV-168.5 — No generic mind-control meter

Propaganda cannot directly “set faction morale -20”.

## INV-168.6 — Truthfulness is factual metadata

Truth/half-truth/lie/exaggeration must have:
- evidence/source basis;
- authored claim semantics.

No arbitrary toggle with magic bonuses detached from the claim.

## INV-168.7 — Lies can be exposed only through information/evidence

No random “lie discovered” without a plausible source.

## INV-168.8 — Direct detection is source-attributed

A faction can learn:
- a message exists;
- who authored it;
- whether it is false
through explicit channels.

## INV-168.9 — Credibility is distinct from faction standing

Credibility measures:
- how believable the shelter/source is on information claims.

It is not:
- political friendship.

## INV-168.10 — Credibility is not a duplicate global reputation system

Prefer derived credibility from:
- confirmed truthful claims;
- exposed lies;
- known source history.

## INV-168.11 — Campaign effects are bounded

No unlimited stacking from 100 repeated leaflets.

## INV-168.12 — Resource costs are real

Paper, ink, radio energy, survivor labor, expedition time use existing systems.

## INV-168.13 — Distribution consumes actual infrastructure/time

No free “select target faction → propaganda lands.”

## INV-168.14 — Player-written text has no mechanical semantic authority by default

MVP uses authored templates/claims.

Custom text, if allowed, is cosmetic.

## INV-168.15 — Moral consequences use MoralChoiceSystem

No hidden moral score in propaganda.

## INV-168.16 — Faction consequences use faction authority

No propaganda-owned hostility state.

## INV-168.17 — Refugee/recruitment/defection effects require real consumers

Otherwise deferred.

## INV-168.18 — Counter-propaganda uses same information semantics

No special AI cheat path.

## INV-168.19 — Save/load never re-distributes a message

## INV-168.20 — No per-frame campaign simulation

Campaign progression is event/day driven.

---

# 3. Definition of Done

Plan 168 closes only when:

- printing/radio/rumor/expedition/faction/moral/narrative authorities are audited;
- existing `StencilPropagandaSmearEntry` is dispositioned;
- one propaganda message/catalog contract exists;
- message identity is deterministic;
- campaign identity is deterministic;
- authored claim/truthfulness semantics are data-driven;
- creation cost uses real resources/labor;
- each shipped distribution method has a real delivery adapter;
- unsupported methods are deferred rather than mocked;
- message creation and delivery are separate state transitions;
- reach, attribution, credibility, and persuasion are separate calculations;
- any stochastic outcome is seeded and traceable;
- no second rumor engine exists;
- no second radio infrastructure exists;
- no generic faction-morale meter exists unless audit proves an existing canonical consumer model;
- faction standing shifts only through explicit consequence rules;
- moral consequences route through `MoralChoiceSystem`;
- public credibility is derived or owned narrowly, not a second reputation universe;
- detected deception can produce consequences only if attribution/exposure exists;
- truthful propaganda is not automatically “safe” from political retaliation;
- repetitive spam is capped/diminished;
- campaigns have finite duration/termination;
- old saves receive empty/default propaganda state;
- restore never replays distribution;
- campaign progression is deterministic;
- headless behavior passes;
- template/faction/channel/asset/localization references validate;
- `--propaganda-selftest` exists or equivalent;
- 30/120/180-day campaign simulations demonstrate bounded effects and no spam exploit;
- 15 templates are authored only after all referenced themes/claims/consumers are real;
- counter-propaganda is shipped only to the depth supported by current faction AI/radio/rumor systems.

---

# 4. Phase P0 — Forensic Authority & Capability Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
PaperPrintingCatalog types
StencilPropagandaSmearEntry fields/consumers
VerdictRadioSystem APIs
VerdictCensusBroadcast APIs
radio transmitter ownership
radio frequency/reception state
Plan 131 rumor/intelligence APIs
FactionBranchCoordinator
FactionStanceEngine
MoralChoiceSystem
survivor skill/writing/communication stats
inventory/paper/ink item IDs
power/radio equipment requirements
expedition delivery APIs
trade/caravan contact APIs
NPC memory/dialogue predicates
quest predicates/effects
journal/narrative event buses
save sections
```

## P0.2 Build propaganda authority matrix

Create:

`docs/information/PROPAGANDA_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
propaganda role
status
```

Rows:
- message definition;
- authored claim;
- print output;
- radio transmission;
- rumor propagation;
- physical delivery;
- reach;
- audience knowledge;
- attribution;
- credibility;
- faction standing;
- faction cohesion/morale if real;
- survivor morale;
- recruitment;
- defection;
- refugee flow;
- moral consequence;
- journal/history;
- counter-message.

## P0.3 Audit “faction morale”

Search for actual canonical state.

Disposition:

```text
EXISTS_CANONICALLY
DERIVED_FROM_EXISTING_FACTS
ABSENT_DEFER
```

No assumption.

## P0.4 Audit source “writing skill”

If no writing skill:
- use an existing communication/education/intelligence/survivor capability;
- or template base quality.

Do not invent a skill merely for one plan.

## P0.5 Audit media resources

Verify:
- paper;
- ink;
- radio equipment;
- electricity;
- transmitter access.

## P0.6 Audit channels

For each source channel:

```text
leaflet
radio
wall_posting
rumor_campaign
broadcast_intercept
```

classify:

```text
REAL
PARTIAL
MISSING
```

## P0.7 Baseline no-system reproduction

Capture:
- printed smear content cannot be player-deployed;
- radio broadcasts are scripted;
- no player propaganda campaign state;
- no attribution/credibility model.

---

# TASK 168A — Message, Claim, Campaign & Credibility Contract

# 168A.0 Goal

Create one compact, deterministic information-warfare state model that owns messages/campaign coordination but not downstream social truth.

## 168A.1 Proposed namespace

If genuinely new:

`Assets/Ashfall.Core/InformationWarfare/`

Prefer broader name over `Propaganda/` if later communications/deception systems can share typed claims.

## 168A.2 Message definition vs message instance

Separate:

```text
PropagandaTemplate
PropagandaMessageInstance
```

Template:
- authored data.

Instance:
- this campaign's produced message.

## 168A.3 Template DTO

Suggested:

```text
id
medium_tags[]
theme
claim_id
target_audience_tags[]
base_persuasion
base_attribution_risk
resource_profile
localization_keys
moral_context_tags[]
```

## 168A.4 Claim DTO

A claim should be structured:

```text
claim_id
truth_class
subject
predicate
object/value
evidence_refs[]
```

Do not require natural-language semantic parsing.

## 168A.5 Truth classes

Source proposes:

```text
truth
half_truth
lie
exaggeration
```

Keep only if authored claims can distinguish them consistently.

## 168A.6 Half-truth semantics

Must have:
- factual component;
- omitted/distorted component.

Not just “medium-risk lie.”

## 168A.7 Exaggeration semantics

Claim based on real fact with amplified magnitude/implication.

## 168A.8 Free-text content

MVP:
- localization/template text chosen by player.

Optional player custom text:
- cosmetic;
- not mechanically interpreted.

## 168A.9 Theme enum

Candidate:

```text
hope
fear
unity
division
triumph
sacrifice
```

Only retain themes with distinct rule consumers.

## 168A.10 Objective enum

Candidate:

```text
build_support
undermine_confidence
encourage_cooperation
discourage_hostility
encourage_defection
protect_identity
```

Remove objectives with no consumer.

## 168A.11 Target faction

Canonical faction ID.

## 168A.12 Target audience

Use audience tags only if factions/world data models:
- civilian;
- military;
- leadership;
- traders;
etc.

If not:
- faction-level target only.

## 168A.13 Author ID

Optional survivor ID.

## 168A.14 Quality

Derive from:
- template;
- author capability;
- production quality.

Do not store redundant computed quality if it can be reproduced; storing instance quality is acceptable if it represents finished artifact state.

## 168A.15 Message ID

Stable deterministic ID:

```text
campaign_id + template_id + message_sequence
```

No GUID.

## 168A.16 Campaign DTO

Suggested:

```text
campaign_id
name optional
objective
target_faction
message_ids[]
status
start_day
end_day
delivery_refs[]
compromised_state
```

Do not store one generic accumulated `effectiveness` if outcomes are event-based.

## 168A.17 Campaign status

Minimal:

```text
planned
active
completed
compromised
cancelled
```

## 168A.18 Delivery record

Suggested:

```text
delivery_id
message_id
channel
day
source_location
target
reach_result
attribution_result
belief_result
source_event_id
```

## 168A.19 Credibility

Before adding a stored score, create ADR:

`ADR_PROPAGANDA_CREDIBILITY.md`

Preferred:
- derive from confirmed truthful/exposed-deceptive public events;
- maintain bounded aggregate only if raw-history retention makes derivation impractical.

## 168A.20 Credibility scope

Potential:
- global shelter source credibility;
- per faction.

Per-faction may be more coherent.

Do not create both automatically.

## 168A.21 Credibility != trust

Same faction can:
- hate the shelter;
- still believe its broadcasts.

## 168A.22 Attribution confidence

Possible:

```text
unknown
suspected
attributed
confirmed
```

Use existing information vocabulary if present.

## 168A.23 Belief/reception result

Possible:

```text
ignored
noticed
considered
believed
rejected
backfired
```

Only if a real consumer needs discrete state.

## 168A.24 No universal numeric mindshare meter

## 168A.25 Campaign cancellation

Stops future scheduled distribution.

Does not erase delivered messages.

## 168A.26 CaptureState

Persist:
- message instances;
- campaign state;
- delivery records;
- pending distributions;
- minimal credibility aggregate if approved.

## 168A.27 RestoreState

No distribution side effects.

## 168A.28 Old save

Empty state.

## 168A.29 Event provenance

Every message/delivery links to:
- source decision;
- item transaction;
- expedition/radio/rumor event.

## 168A.30 Generated schema docs

Create:
- `PROPAGANDA_MESSAGE_CONTRACT.md`;
- `PROPAGANDA_TEMPLATE_MATRIX.md`.

### 168A DoD

Propaganda has a stable authored-message/campaign contract with explicit claim, delivery, attribution and credibility semantics, without storing duplicate faction/world outcomes.

---

# TASK 168B — Production & Distribution Channels

# 168B.0 Goal

Make each propaganda medium a real logistical action using existing systems.

---

# 168B-P — Printing / Leaflets

## 168B.P1 Reuse PaperPrintingCatalog

Assess whether `StencilPropagandaSmearEntry` becomes:
- template subtype;
- print recipe;
- narrative record.

## 168B.P2 Production cost

Use:
- paper;
- ink;
- printing labor;
- power if printer requires.

Only real items/resources.

## 168B.P3 Printed quantity

Finite item/batch.

## 168B.P4 No hidden leaflet points

## 168B.P5 Storage

If printed leaflets are physical:
- canonical inventory or dedicated print-batch entity.

Choose one.

## 168B.P6 Distribution

Requires:
- expedition;
- caravan/trader handoff;
- encounter;
depending on real system.

## 168B.P7 Faction territory

Use world map/control authority.

## 168B.P8 Reach

Depends on:
- batch quantity;
- destination;
- distribution event.

Not “leaflet = low reach” magic alone.

## 168B.P9 Risk

Distribution exposure belongs to:
- expedition/encounter;
- witness/intel;
not a detached global roll.

---

# 168B-R — Radio Broadcasts

## 168B.R1 Reuse radio infrastructure

Player must have:
- transmitter;
- power;
- usable frequency/broadcast capability
if modeled.

## 168B.R2 Broadcast action

Use canonical radio command/event.

## 168B.R3 Duration

Campaign-time minutes/hours if relevant.

No per-frame simulation.

## 168B.R4 Reach

Derived from:
- transmitter;
- frequency;
- distance/coverage;
- jamming/weather if current radio system supports.

## 168B.R5 Target faction awareness

Only if target can receive/listen.

## 168B.R6 Attribution

May be:
- explicit sender ID;
- voice recognized;
- signal source triangulated
only if systems exist.

## 168B.R7 Power cost

Real power authority.

## 168B.R8 Broad audience

Radio may reach unintended listeners through current signal model.

This is valuable emergent behavior.

---

# 168B-W — Wall Postings

## 168B.W1 Physical deployment

Use expedition/location interaction.

## 168B.W2 Requires real location

No abstract “post in faction territory” without trip/encounter.

## 168B.W3 Material cost

Printed poster/stencil materials.

## 168B.W4 Persistence

Posting can become:
- location memory/scar;
- discoverable information object
if world/location system supports.

## 168B.W5 Removal

Target faction may remove via world evolution if such autonomy exists.

Otherwise bounded duration.

---

# 168B-U — Rumor Campaigns

## 168B.U1 Reuse Plan 131

Propaganda message becomes rumor seed.

## 168B.U2 No duplicate propagation tick

## 168B.U3 Seeding method

Via:
- trader;
- traveler;
- NPC;
- radio,
using existing contacts.

## 168B.U4 Source masking

If rumor system supports provenance/credibility.

## 168B.U5 Truth class

Carries claim/evidence metadata.

## 168B.U6 Spread result

Rumor system owns:
- recipients;
- timing;
- credibility loss/amplification.

## 168B.U7 Campaign observes outcome

Does not reproduce it.

---

# 168B-I — Broadcast Intercepts / Frequency Hijack

## 168B.I1 Hard gate

Implement only if radio authority exposes:
- enemy frequency;
- access/control;
- transmission collision or hijack concept.

## 168B.I2 Captured equipment

Must be canonical item/system requirement.

## 168B.I3 No hacking minigame

Unless existing system.

## 168B.I4 High risk

Comes from:
- attribution;
- frequency detection;
- expedition/intel source.

## 168B.I5 If unsupported

DEFER.

Do not simulate with arbitrary “high cost + 30% risk” button.

---

# 168B-C — Channel Registry

## 168B.C1 Typed channel adapters

Each channel implements:

```text
CanProduce
CreateDeliveryPlan
ExecuteDelivery
ProjectReach
ProjectAttributionRisk
```

## 168B.C2 No generic reflection

## 168B.C3 Channel data

Costs/ranges in data where appropriate.

## 168B.C4 Unsupported channels

Fail closed with clear reason.

### 168B DoD

Every shipped propaganda method corresponds to a real logistical/information channel, with real costs and no duplicate propagation or radio simulation.

---

# TASK 168C — Reach, Attribution, Credibility & Persuasion Projection

# 168C.0 Goal

Make propaganda outcomes explainable and bounded instead of one opaque effectiveness score.

## 168C.1 Outcome pipeline

Recommended:

```text
delivery succeeded?
   │
   ▼
reach
   │
   ▼
audience eligibility
   │
   ▼
attribution
   │
   ▼
credibility
   │
   ▼
message receptiveness
   │
   ▼
persuasion consequence eligibility
```

## 168C.2 Reach

Use real channel result.

## 168C.3 Audience eligibility

If audience tags are modeled.

Otherwise:
- faction-level.

## 168C.4 Attribution

Separate from reach.

## 168C.5 Claim verification

A lie becomes exposed when:
- audience has contradictory evidence;
- faction intelligence verifies;
- campaign contradicts later public facts;
- authored investigation/event occurs.

No arbitrary daily lie exposure.

## 168C.6 Credibility contribution

Truthful confirmed message:
- positive source credibility.

Exposed lie:
- negative.

Half-truth/exaggeration:
- context-dependent.

## 168C.7 Receptiveness

Potential inputs:
- current faction relationship;
- war loss/history;
- scarcity;
- leadership conflict;
- known world events;
- current policy.

Only real inputs.

## 168C.8 No invented “war weary” meter

Unless one exists.

## 168C.9 Author skill

If current capability exists:
- contributes to clarity/quality.

## 168C.10 Production quality

Printing/radio conditions can affect reach/clarity.

## 168C.11 Repetition

Repeated identical message suffers diminishing returns.

## 168C.12 Message diversity

Campaign may benefit from multiple relevant claims.

No arbitrary diversity bonus unless needed.

## 168C.13 Saturation cap

Per:
- faction;
- objective;
- time window.

Prevents spam.

## 168C.14 Backfire

Can occur if:
- exposed lie;
- grossly contradicted claim;
- offensive audience mismatch;
- enemy counter-evidence.

Do not use random catastrophic failure just for drama.

## 168C.15 Effectiveness read model

Instead of one number, show:

```text
reach
credibility
receptiveness
attribution
saturation
projected impact band
```

## 168C.16 Final impact band

Example:

```text
negligible
low
moderate
high
```

## 168C.17 Exact numeric value

Can exist internally for tuning.

Do not imply certainty in UI.

## 168C.18 Deterministic core

All deterministic except explicitly seeded uncertain delivery/detection.

## 168C.19 Seed source

Stable:
- delivery ID;
- campaign seed;
- world seed.

## 168C.20 No `Random.Shared`

## 168C.21 No wall clock

## 168C.22 Diagnostics

Trace every factor.

## 168C.23 Unit tests

- unreachable;
- reached;
- unattributed;
- low credibility;
- high credibility;
- repeated spam;
- exposed lie;
- factual claim;
- hostile audience;
- allied audience.

### 168C DoD

Propaganda outcomes are traceable through reach, attribution, credibility, receptiveness and saturation rather than a single magic effectiveness meter.

---

# TASK 168D — Faction, Moral, Rumor, NPC & Quest Consequences

# 168D.0 Goal

Translate information outcomes into real downstream state exactly once.

---

# 168D-F — Faction Consequences

## 168D.F1 One faction mutation authority

Reuse Plan-139 result.

## 168D.F2 Hostile propaganda

May reduce:
- standing/trust
if faction attributes campaign to shelter.

## 168D.F3 Friendly propaganda

May improve:
- standing
only if faction values/supports it.

## 168D.F4 Truthful hostile speech

Can still anger a faction.

Truth ≠ politically safe.

## 168D.F5 Lie exposure

Can cause stronger political penalty if attributed.

## 168D.F6 No direct stance shift from “effectiveness”

Use explicit thresholds/events.

## 168D.F7 No automatic neutral→friendly/hostile after one campaign

Bounded.

## 168D.F8 Political consequence ID

Stable:
- campaign/delivery + faction + rule.

## 168D.F9 No duplicate faction effect through rumor and direct adapter

Information channel only affects **knowledge**; one consequence resolver owns mutation.

---

# 168D-M — Moral Choice

## 168D.M1 Truthfulness is not itself sufficient

Moral weight also depends on:
- target;
- intended harm;
- deception;
- exploitation;
- civilian targeting.

## 168D.M2 MoralChoiceSystem owns state

## 168D.M3 Pre-commit preview

If authored moral weight is knowable:
- show it.

## 168D.M4 No hidden moral arithmetic

## 168D.M5 Truth campaign

May have low/no deception cost.

## 168D.M6 Fear/division campaign

Could carry moral weight even if facts are technically true.

## 168D.M7 Self-defense morale messaging

Separate from hostile deception.

---

# 168D-N — NPC Memory

## 168D.N1 Named NPC exposure

Plan 147 may record:
- persuaded;
- offended;
- recognized voice/source;
- exposed lie.

## 168D.N2 No clone to every NPC

Only actual recipients/known sources.

## 168D.N3 Dialogue

Predicates can reference:
- known campaign;
- credibility;
- faction view.

Memory/dialogue own response.

---

# 168D-Q — Quest Integration

## 168D.Q1 Quest runtime owns quests

## 168D.Q2 Predicates

Possible:

```text
propaganda_campaign_completed
message_delivered
campaign_attributed
lie_exposed
credibility_band
target_faction_received_claim
```

## 168D.Q3 Source quest hooks

Treat as content backlog:
- The Propagandist
- The Campaign
- The Voice
- The Underground
- War of Words
- The Truth
- The Defector

## 168D.Q4 Defector quest

Only if a real defection/character-transfer mechanic exists.

Otherwise defer.

## 168D.Q5 No quest invented by runtime

---

# 168D-E — Economy / Refugees / Recruitment / Defection

## 168D.E1 Hard gate

Each requires a real downstream authority.

## 168D.E2 Trade prices

Do not mutate directly.

Potential path:

```text
propaganda changes known faction relation
→ faction stance changes
→ trade system naturally changes
```

## 168D.E3 Refugees

Only visitor/admission authority can change arrival pressure.

## 168D.E4 Recruitment

Only faction recruitment/manpower authority if real.

## 168D.E5 Defection

Requires:
- actual faction members/entities;
- transfer/defection lifecycle.

If absent:
- narrative-only event or defer.

## 168D.E6 Civilian allegiance

Do not invent without settlement/civilian authority.

---

# 168D-H — Shelter-Targeted Propaganda

## 168D.H1 Enemy propaganda

If factions can target the player shelter:
- input arrives through radio/rumor/posting.

## 168D.H2 Survivor morale

Existing morale/mental-health system may consume:
- credible threatening message;
- humiliating defeat narrative;
- hopeful allied message.

## 168D.H3 No direct broad `morale -= 10`

Use semantic event/modifier adapter.

## 168D.H4 Counter-message

Player can respond through same systems.

### 168D DoD

Political, moral, personal, quest and shelter consequences occur through their canonical authorities and only when information has actually reached/been attributed to the relevant actors.

---

# TASK 168E — Campaign Management, UI, Persistence, Anti-Spam & Balance

# 168E.0 Goal

Make propaganda usable as strategy rather than a spreadsheet or spam exploit.

## 168E.1 UI surface decision

Source proposes a campaign manager panel.

Audit:
- radio;
- printing;
- faction operations;
- intel panels.

A dedicated information-warfare panel is justified only if:
- multiple channels/campaigns need coordination.

## 168E.2 Campaign overview

Show:
- objective;
- target;
- messages;
- delivery status;
- reach;
- attribution;
- credibility;
- saturation;
- known consequences.

## 168E.3 Message creation flow

Prefer authored choices:

```text
template
theme
truth class/claim
target
audience
author
channel
```

## 168E.4 No mandatory free-text composition

## 168E.5 If custom text allowed

- cosmetic;
- escaped;
- short;
- not interpreted mechanically.

## 168E.6 Distribution planner

Shows actual:
- resource cost;
- survivor/time cost;
- infrastructure;
- route;
- risk.

## 168E.7 Effectiveness tracker

Do not show fake precision.

Use factor breakdown.

## 168E.8 Faction “morale” display

Only if canonical faction morale/cohesion exists.

Otherwise show:
- known reception;
- standing;
- credibility;
- current campaign effects.

## 168E.9 Detection display

Player knows only what own intelligence reveals.

## 168E.10 Compromised campaign

Status set when:
- attribution/exposure meets rule.

## 168E.11 Cancellation

Stops future planned deliveries.

## 168E.12 Campaign duration

Finite.

No indefinite passive pressure.

## 168E.13 Journal

Log:
- first campaign;
- major broadcast;
- compromise;
- exposed lie;
- milestone success/failure.

No every-leaflet spam.

## 168E.14 Tutorial

Only existing tutorial framework.

## 168E.15 Tooltips

Explain:
- reach;
- credibility;
- attribution;
- saturation.

## 168E.16 Accessibility

No color-only truth/risk/impact.

## 168E.17 Localization

All authored messages/templates use keys.

---

# 168E-P — Persistence

## 168E.P1 New state only

Persist:

```text
messages
campaigns
delivery records
pending distribution
processed consequence IDs
approved credibility aggregate if needed
```

## 168E.P2 Do not persist duplicates

No:
- faction standing;
- rumor network state;
- radio hardware state;
- inventory;
- moral band;
- survivor morale
inside propaganda state.

## 168E.P3 Old save

Empty campaign state.

## 168E.P4 Mid-production save

Print/job owner persists production if applicable.

Propaganda stores message intent/ref.

## 168E.P5 Mid-expedition distribution

Expedition owns expedition state.

## 168E.P6 Mid-radio broadcast

Prefer atomic delivery event at broadcast execution.

## 168E.P7 Pending rumor

Rumor system owns propagation.

## 168E.P8 Reload idempotence

No repeated:
- resource consumption;
- delivery;
- standing;
- moral effect;
- credibility change.

---

# 168E-A — Anti-Spam / Exploit Prevention

## 168E.A1 Diminishing returns

Repeated same claim/channel/target within window:
- saturation.

## 168E.A2 Resource cost

Printing/broadcasting consumes real resources.

## 168E.A3 Labor cost

Author/distributor time.

## 168E.A4 Access cost

Physical delivery may risk expedition.

## 168E.A5 Channel capacity

Radio/printing may have throughput limits if current infrastructure does.

## 168E.A6 No 100-message burst

Campaign action budget.

## 168E.A7 Same-message duplicate

Stable delivery ID prevents duplicate consequence.

## 168E.A8 Save-scum detection reroll

Committed delivery uses stable seed.

## 168E.A9 Credibility farming

Repeating trivial truths cannot max credibility.

Require:
- meaningful claims;
- distinct events;
- capped gain.

## 168E.A10 Lie spam

Multiple low-value lies cannot create infinite short-term gain.

Saturation and credibility damage.

## 168E.A11 Cross-channel spam

Same claim over leaflet+radio+rumor shares saturation pool unless design says media reinforcement.

## 168E.A12 Friendly-faction farming

No endless positive standing from praise.

---

# 168E-B — Balance

## 168E.B1 Propaganda cannot replace combat/trade/diplomacy

It is:
- indirect;
- slower;
- uncertain.

## 168E.B2 Truth vs lie

Avoid simplistic:
- truth = weak;
- lie = strong.

Truth can be powerful when evidence aligns.
Lie may produce faster short-term effect but credibility risk.

## 168E.B3 Theme matching

Themes should interact with real conditions.

No generic Rock-Paper-Scissors.

## 168E.B4 Campaign objectives

At least 3 useful objectives for MVP.

Do not ship 5 if consumers missing.

## 168E.B5 Costs

Scale with:
- medium;
- reach;
- target.

## 168E.B6 Political retaliation

Strong operations can provoke response.

## 168E.B7 No automatic war

Routine propaganda alone should not instantly cause total faction war.

## 168E.B8 Credibility recovery

After exposed lies:
- future truthful evidence and time can recover if design allows.

## 168E.B9 No instant forgiveness

## 168E.B10 Attribution ambiguity

Can create strategic value:
- influence without certain blame.

Only if information system supports.

---

# 168E-S — Long-Horizon Simulation

## 168E.S1 30-day light-use scenario

Track:
- messages;
- costs;
- deliveries;
- credibility;
- faction effects.

## 168E.S2 120-day mixed-truth campaign

Track:
- saturation;
- exposure;
- standing;
- resource burden.

## 168E.S3 180-day aggressive information-war scenario

Assert:
- no infinite standing/morale collapse;
- no unbounded campaign queue;
- credibility can meaningfully diverge.

## 168E.S4 No-propaganda case

Zero side effects/overhead.

## 168E.S5 Truth-only case

Viable.

## 168E.S6 Deception-heavy case

Short-term benefit with long-term credibility risk.

## 168E.S7 Multi-faction campaign

Effects isolated by target/knowledge.

## 168E.S8 Counter-message case

If counter-propaganda supported.

---

# 168E-T — Testing & CI

## 168E.T1 Data integrity

Validate:
- template IDs;
- claim IDs;
- faction IDs;
- channels;
- themes;
- audience tags;
- resource profiles;
- localization.

## 168E.T2 Selftest

Create:

```text
--propaganda-selftest
```

## 168E.T3 Selftest scenarios

At least:
1. create message;
2. no delivery/no effect;
3. printed leaflet production;
4. physical delivery;
5. radio broadcast;
6. rumor seed;
7. unsupported intercept fails closed;
8. truthful claim;
9. exposed lie;
10. unattributed delivery;
11. faction consequence;
12. moral consequence;
13. saturation;
14. old save;
15. save/load idempotence;
16. headless.

## 168E.T4 Source-scan authority gate

Detect:
- propaganda-owned faction morale;
- propaganda-owned faction standing;
- direct market price changes;
- direct refugee count changes;
- second rumor propagation loop;
- second radio transmission model.

## 168E.T5 Failure fixtures

Create deliberate invalid:
- faction ID;
- channel;
- claim;
- resource profile;
- unsupported consumer.

## 168E.T6 Generated docs

Create:
- `PROPAGANDA_ARCHITECTURE.md`;
- `PROPAGANDA_AUTHORITY_MATRIX.md`;
- `PROPAGANDA_TEMPLATE_MATRIX.md`;
- `PROPAGANDA_CHANNEL_MATRIX.md`;
- `PROPAGANDA_CONSEQUENCE_DISPOSITION.md`;
- `ADR_PROPAGANDA_CREDIBILITY.md`;
- `PROPAGANDA_BALANCE_REPORT.md`.

### 168E DoD

Campaigns remain bounded, understandable, deterministic, save-safe, and strategically useful without becoming a click-spam influence currency.

---

# TASK 168F — Counter-Propaganda & Faction Response

# 168F.0 Goal

Allow other factions to respond through the same information infrastructure without inventing a second AI propaganda simulator.

## 168F.1 Audit faction autonomous actions

Can factions currently:
- broadcast;
- send rumors;
- create events;
- alter radio content?

## 168F.2 Scripted broadcasts

`VerdictRadioSystem` may already provide faction messaging.

Use it as a counter-message source if semantically appropriate.

## 168F.3 Counter-message model

A faction broadcast/rumor can:
- contradict claim;
- provide evidence;
- attack shelter credibility;
- rally supporters.

## 168F.4 No hidden counter-propaganda points

## 168F.5 Evidence-based rebuttal

Countering false claims should be easier when evidence exists.

## 168F.6 Shelter-side reception

If counter-message reaches shelter:
- survivors/NPCs may react via existing morale/dialogue/rumor systems.

## 168F.7 External audience reception

Rumor/intel determines.

## 168F.8 Faction resource cost

Only model if faction autonomy already tracks operational resources.

Otherwise action budget/AI rule.

## 168F.9 Campaign selection

Faction AI chooses counter-message based on:
- attributed hostile propaganda;
- policy/stance;
- capability.

## 168F.10 No omniscient response

Faction must know campaign exists.

## 168F.11 Compromise

Counter-intelligence can expose authorship only through real evidence/intel.

## 168F.12 Counter-counter loop

Hard cap.

No infinite ping-pong.

## 168F.13 Response cooldown

Campaign/faction bounded.

## 168F.14 If autonomous messaging absent

MVP:
- scripted/reactive counter-events only.

## 168F.15 Acceptance

Counter-propaganda is optional for core ship if underlying AI rails are not ready.

### 168F DoD

Faction responses, where supported, use the same reach/credibility/attribution semantics and cannot cheat through hidden omniscient counters.

---

# TASK 168G — Advanced PsyOps, Intercepts, Defection & Public Allegiance: Explicit Follow-On Gate

# 168G.0 Goal

Prevent propaganda from becoming a generic covert-operations / social-control simulator before its base information model is stable.

## 168G.1 Broadcast hijack

Implement only with real:
- frequency control;
- transmitter;
- enemy signal;
- access requirement.

## 168G.2 Recruitment of defectors

Requires:
- faction member identity;
- allegiance lifecycle;
- transfer;
- downstream survivor/NPC representation.

Otherwise defer.

## 168G.3 Civilian allegiance

Requires:
- settlement population;
- allegiance/cohesion authority.

Otherwise defer.

## 168G.4 Psychological operations against military units

Requires:
- unit morale/surrender behavior.

Otherwise defer.

## 168G.5 Fake orders

Requires:
- command/identity/authentication system.

Defer.

## 168G.6 Forged broadcasts

Requires:
- source authentication mechanics.

Defer.

## 168G.7 Deep deception campaigns

Can be content follow-on using claims/rumors, not new engine.

## 168G.8 Propaganda archive

Plan 162/archive can store notable campaign artifacts.

No second archive.

## 168G.9 Propaganda art generator

Presentation follow-on.

Not gameplay requirement.

## 168G.10 Famous campaigns in epilogue

Derive from:
- campaign history;
- public impact;
- credibility consequences.

### 168G DoD

Advanced psyops remain evidence-gated extensions instead of half-implemented promises inside the MVP.

---

# 5. Information-Warfare State Model

```text
DRAFT
  │
  ▼
PRODUCED
  │
  ▼
PLANNED FOR DELIVERY
  │
  ▼
DELIVERED
  │
  ├── unreachable / ignored
  └── reached
       │
       ├── unattributed
       ├── suspected
       └── attributed
              │
              ▼
         accepted / rejected / contradicted
              │
              ▼
         downstream consequence eligibility
```

Campaign state coordinates many message deliveries but does not replace their provenance.

---

# 6. Message Truth Contract

Truthfulness describes the claim.

It is not a universal effectiveness multiplier.

Examples:

```text
truth
→ easier verification
→ lower exposure risk to credibility

lie
→ may exploit information gap
→ severe credibility loss if disproven
```

No automatic “lie = +30 effectiveness.”

---

# 7. Theme Contract

Theme influences:
- message framing;
- audience compatibility.

Themes do not directly apply:
- morale points;
- standing points.

---

# 8. Objective Contract

Campaign objective is:
- planner intent.

Actual system effects remain in downstream authorities.

---

# 9. Reach Contract

Reach is channel-specific.

Leaflets:
- physical region/location.

Radio:
- signal coverage.

Rumor:
- network propagation.

Wall:
- location visitors.

Intercept:
- frequency listeners.

---

# 10. Attribution Contract

Attribution answers:

```text
Do recipients know who authored/distributed this?
```

It does not answer:
- whether they believe it.

---

# 11. Credibility Contract

Credibility answers:

```text
How reliable is this source believed to be?
```

It does not answer:
- whether recipient likes the source.

---

# 12. Belief Contract

Belief/reception is claim-specific.

A faction may:
- believe a fact;
- reject its framing;
- still hate the sender.

---

# 13. Saturation Contract

Repeated exposure to same:
- claim;
- theme;
- source;
- objective
within a time window has bounded marginal effect.

---

# 14. Detection Contract

The term “detection” must be split into:

```text
delivery discovered
authorship attributed
deception exposed
```

These are distinct.

---

# 15. Printing Contract

Propaganda uses:
- real print resources;
- existing production/content path.

No abstract leaflet currency.

---

# 16. Radio Contract

Player broadcast uses:
- real radio equipment/power/frequency.

No second transmitter simulation.

---

# 17. Rumor Contract

Rumor campaign seeds:
- Plan 131.

Plan 131 owns:
- spread;
- recipient;
- delay;
- distortion if supported.

---

# 18. Expedition Contract

Physical distribution requires:
- actual expedition/location action where possible.

No remote magic deployment.

---

# 19. Faction Contract

Faction authority owns:
- trust;
- standing;
- hostility.

Propaganda can request a consequence only after information conditions are met.

---

# 20. Moral Contract

MoralChoice owns:
- moral weight/outcome.

Truthfulness alone does not determine morality.

---

# 21. NPC Memory Contract

Named NPC may remember:
- receiving;
- believing;
- exposing;
- being targeted.

Plan 147 owns personal relationship state.

---

# 22. Quest Contract

Propaganda exposes predicates/events.

Quest runtime owns:
- availability;
- progress;
- completion.

---

# 23. Economy Contract

Propaganda cannot directly set price.

If faction relation changes:
- market/trade may react.

If a real confidence/cohesion economy input exists:
- use typed adapter.

---

# 24. Refugee Contract

No direct refugee flow effect unless visitor/admission authority exposes a public-reputation/information input.

---

# 25. Faction Morale / Cohesion ADR

Create:

`ADR_FACTION_MORALE_AND_COHESION.md`

Questions:

```text
Does canonical faction morale exist?
What facts does it own?
Which systems consume it?
Is it distinct from standing?
Is it distinct from military readiness?
Is it distinct from civilian support?
```

If no:
- Plan 168 does not create one.

---

# 26. Persistence Matrix

| Fact | Owner |
|---|---|
| message template | propaganda data |
| message instance | propaganda |
| campaign | propaganda |
| delivery record | propaganda/history |
| print inventory | inventory/printing |
| radio hardware | radio |
| rumor propagation | rumor/intel |
| faction standing | faction |
| faction morale/cohesion | canonical faction system if real |
| survivor morale | survivor needs/morale |
| credibility | derived/propaganda aggregate |
| NPC memory | NPC memory |
| moral band | MoralChoice |
| quest state | quest |
| archive history | archive |

---

# 27. Old-Save Migration

Default:

```text
propaganda:
  schema_version: 1
  messages: []
  campaigns: []
  deliveries: []
  processed_consequences: []
```

No retroactive propaganda.

---

# 28. Idempotence Contract

Stable IDs:

```text
message_id
campaign_id
delivery_id
consequence_id
```

Reload cannot repeat:
- printing cost;
- expedition delivery;
- broadcast;
- rumor seed;
- standing;
- moral consequence;
- credibility change.

---

# 29. Failure Injection Matrix

## N168.1 Draft message changes faction standing
Expected: delivery gate fails.

## N168.2 Leaflet message delivered without printing/resources
Expected: channel authority gate fails.

## N168.3 Rumor campaign implements its own propagation tick
Expected: duplicate-information gate fails.

## N168.4 Radio propaganda broadcasts without transmitter/power despite radio system requiring them
Expected: infrastructure gate fails.

## N168.5 Unattributed message creates direct hostility toward shelter
Expected: attribution gate fails unless faction rule explicitly supports suspicion.

## N168.6 Lie becomes “exposed” via random daily roll without evidence
Expected: verification gate fails.

## N168.7 Propaganda directly sets trade price
Expected: economy authority gate fails.

## N168.8 Propaganda directly spawns defectors
Expected: defection authority gate fails.

## N168.9 Same delivery replayed after save/load
Expected: idempotence fails.

## N168.10 100 repeated identical leaflets stack unlimited effect
Expected: saturation gate fails.

## N168.11 Faction counter-propaganda reacts to secret campaign it does not know about
Expected: information gate fails.

## N168.12 “Faction morale” state added despite no consumer/authority ADR
Expected: architecture gate fails.

---

# 30. Determinism Contract

Same:

```text
campaign state
+ message
+ channel
+ target state
+ information state
+ world day
+ stable seed
```

must yield same:
- delivery;
- reach;
- attribution;
- belief/reception result;
- credibility update;
- consequence eligibility.

---

# 31. Long-Horizon Metrics

Track:

```text
messages created
messages produced
deliveries attempted
deliveries reached
attributions
exposed lies
truth confirmations
campaigns completed
campaigns compromised
credibility by faction/source
saturation by claim
faction consequence count
moral consequence count
radio airtime
print resources consumed
expedition delivery cost
counter-messages
state bytes
```

---

# 32. Balance Guardrails

Information warfare should be:

```text
slower than violence
cheaper than war in some contexts
riskier than diplomacy in others
dependent on infrastructure
dependent on credibility
```

It should not be:

```text
free standing manipulation
instant faction morale control
infinite spam
unavoidable AI debuff
```

---

# 33. Truth vs Deception Balance

Truth campaign advantages:
- credibility;
- resilience to verification;
- moral safety depending on target.

Deception advantages:
- exploit information gaps;
- potentially larger short-term persuasion.

Deception costs:
- exposure;
- long-term credibility;
- political/moral backlash.

---

# 34. Channel Balance

## Leaflets
Low-medium cost.
Localized reach.
Requires physical access.

## Radio
Higher infrastructure.
Broad reach.
Potential unintended listeners.

## Wall posting
Very local.
High physical exposure.
Persistent place evidence.

## Rumor
Slow.
Network-dependent.
Potential distortion.

## Intercept
High-risk.
Only if real frequency-control mechanics exist.

---

# 35. Campaign Complexity Guardrails

MVP target:
- 3 channels fully real;
- 3 objectives;
- 8–15 templates.

Do not force five channels if two are fake.

---

# 36. UI Acceptance

## Campaign overview
- target;
- objective;
- status;
- messages;
- deliveries.

## Message detail
- claim;
- truth class;
- theme;
- author;
- production cost.

## Delivery
- channel;
- infrastructure;
- target;
- projected reach;
- attribution risk.

## Outcome
- reached?
- attributed?
- credibility impact?
- downstream consequence?

---

# 37. Accessibility

- no color-only truth class;
- no color-only attribution risk;
- keyboard campaign creation;
- text scaling;
- message text readable;
- reduced-motion safe.

---

# 38. Localization

Templates:
- localization keys.

Player text:
- cosmetic only;
- escaped.

Claims:
- structured IDs, not localized strings as logic keys.

---

# 39. Content Acceptance

Templates/channels:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

A template that cannot be produced/delivered is not gameplay-complete.

---

# 40. Reachability

For each template:

```text
can player unlock?
can produce?
can choose valid target?
can deliver through at least one real channel?
can consequence be observed?
```

For each channel:
- infrastructure reachable.

---

# 41. Performance Guardrails

- no per-frame campaign scan;
- process scheduled deliveries only;
- index campaigns by target/status;
- reuse rumor/radio indexes;
- no repeated full-history credibility scan if aggregate can be maintained.

---

# 42. CI / Gate Set

Recommended:

```text
propaganda_authority_single
propaganda_template_integrity
propaganda_channel_integrity
propaganda_no_duplicate_rumor
propaganda_no_duplicate_radio
propaganda_delivery_required
propaganda_attribution_gate
propaganda_credibility_integrity
propaganda_saturation
propaganda_no_direct_market_mutation
propaganda_no_direct_refugee_mutation
propaganda_no_unowned_faction_morale
propaganda_save_matrix
propaganda_determinism
propaganda_reachability
propaganda_long_horizon
propaganda_ui_access
```

---

# 43. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --propaganda-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 44. Recommended Commit Breakdown

```text
168A-1 authority/capability audit
168A-2 StencilPropagandaSmear disposition
168A-3 message/template/claim schema
168A-4 campaign/delivery state
168A-5 credibility ADR
168A-6 stable IDs/idempotence
168A-7 old-save/save-roundtrip
168A-8 generated contracts/docs

168B-1 print/leaflet adapter
168B-2 expedition physical distribution
168B-3 radio broadcast adapter
168B-4 rumor-seed adapter
168B-5 wall-posting adapter if world supports
168B-6 intercept capability audit/defer
168B-7 channel registry
168B-8 channel tests/docs

168C-1 reach projection
168C-2 attribution projection
168C-3 verification/exposed-lie rules
168C-4 credibility projection
168C-5 receptiveness
168C-6 saturation
168C-7 diagnostics
168C-8 deterministic unit tests

168D-1 faction consequence adapter
168D-2 MoralChoice adapter
168D-3 NPC-memory/dialogue adapter
168D-4 quest predicates
168D-5 faction-morale/cohesion ADR
168D-6 economy/refugee/recruitment disposition
168D-7 shelter-targeted message adapter
168D-8 cross-system authority tests

168E-1 campaign UI
168E-2 message/distribution planner
168E-3 accessibility/localization
168E-4 anti-spam/idempotence
168E-5 30-day balance
168E-6 120/180-day info-war soaks
168E-7 CI gates/failure fixtures
168E-8 final ship/no-ship report

168F-1 faction counter-message capability audit
168F-2 scripted counter-message adapter
168F-3 counter-evidence/credibility
168F-4 bounded response loops

168G-1 intercept/defection/civilian-allegiance disposition
168G-2 propaganda archive/epilogue follow-on
```

---

# 45. Risk Register

## R168.1 Creates generic faction morale just for propaganda

Mitigation:
- mandatory ADR;
- no consumer = no state.

## R168.2 Duplicates rumor network

Mitigation:
- Plan 131 adapter only.

## R168.3 Duplicates radio

Mitigation:
- player broadcasts use existing radio authority.

## R168.4 Propaganda feels like spreadsheet percentage stacking

Mitigation:
- reach/attribution/credibility factor model;
- finite campaigns;
- authored outcomes.

## R168.5 Lying is always optimal

Mitigation:
- evidence/exposure;
- long-term credibility;
- moral/political cost.

## R168.6 Spam dominates factions

Mitigation:
- saturation;
- resource/labor/channel capacity.

## R168.7 Free-text content creates impossible semantic evaluation

Mitigation:
- authored claims/templates;
- cosmetic custom text only.

## R168.8 Counter-propaganda becomes AI cheat system

Mitigation:
- faction knowledge gate;
- same channel semantics.

---

# 46. Acceptance Checklist

## P0

- [ ] `PaperPrintingCatalog` audited
- [ ] `StencilPropagandaSmearEntry` consumers audited
- [ ] `VerdictRadioSystem` audited
- [ ] radio infrastructure audited
- [ ] Plan 131 rumor/intelligence audited
- [ ] FactionBranchCoordinator audited
- [ ] FactionStanceEngine audited
- [ ] MoralChoiceSystem audited
- [ ] survivor writing/communication capability audited
- [ ] paper/ink/resource IDs audited
- [ ] power/transmitter requirements audited
- [ ] expedition delivery APIs audited
- [ ] trade/caravan information channels audited
- [ ] Plan 147 NPC memory audited
- [ ] quest predicates audited
- [ ] journal/narrative event buses audited
- [ ] save sections audited
- [ ] faction morale/cohesion existence audited
- [ ] five source channels classified REAL/PARTIAL/MISSING
- [ ] propaganda authority matrix published
- [ ] baseline no-system behavior captured

## 168A

- [ ] appropriate namespace selected
- [ ] template vs message instance separated
- [ ] typed template DTO
- [ ] structured claim DTO
- [ ] truth classes semantically defined
- [ ] half-truth not just multiplier
- [ ] exaggeration grounded in fact
- [ ] custom text cosmetic only
- [ ] theme enum trimmed to real consumers
- [ ] objective enum trimmed to real consumers
- [ ] canonical faction IDs
- [ ] audience tags only if modeled
- [ ] author optional
- [ ] quality derived appropriately
- [ ] stable message ID
- [ ] compact campaign DTO
- [ ] no generic accumulated-effectiveness truth
- [ ] finite statuses
- [ ] delivery record
- [ ] credibility ADR
- [ ] credibility scope decided
- [ ] credibility separate from trust
- [ ] attribution confidence
- [ ] no universal mindshare meter
- [ ] cancellation semantics
- [ ] CaptureState
- [ ] RestoreState no replay
- [ ] old-save empty state
- [ ] source provenance
- [ ] generated contract docs

## 168B — Printing

- [ ] existing smear type dispositioned
- [ ] real paper/ink/labor
- [ ] finite print batch
- [ ] no leaflet points
- [ ] storage authority chosen
- [ ] physical distribution requires real path
- [ ] faction territory uses world authority
- [ ] reach uses actual delivery
- [ ] risk uses expedition/witness/info

## 168B — Radio

- [ ] existing radio infrastructure reused
- [ ] transmitter/power requirements
- [ ] canonical broadcast event
- [ ] campaign-time duration
- [ ] real coverage inputs
- [ ] receiver requirement
- [ ] attribution grounded
- [ ] real power cost
- [ ] unintended listeners possible if radio supports

## 168B — Wall/Rumor/Intercept

- [ ] wall posting requires location interaction
- [ ] wall materials real
- [ ] posting persistence only if world supports
- [ ] Plan 131 rumor reused
- [ ] no duplicate rumor tick
- [ ] rumor seed uses real contact
- [ ] provenance supported
- [ ] intercept hard-gated on radio capability
- [ ] captured equipment requirement if real
- [ ] no hacking minigame invented
- [ ] unsupported intercept deferred
- [ ] typed channel registry
- [ ] unsupported channels fail closed

## 168C

- [ ] outcome pipeline explicit
- [ ] reach separated
- [ ] audience eligibility
- [ ] attribution separated
- [ ] lie verification evidence-based
- [ ] truthful confirmations affect credibility
- [ ] exposed lies affect credibility
- [ ] receptiveness uses real state
- [ ] no invented war-weariness meter
- [ ] author capability only if existing
- [ ] production quality
- [ ] repeated-message diminishing returns
- [ ] saturation cap
- [ ] backfire evidence-based
- [ ] factorized effectiveness read model
- [ ] impact band
- [ ] UI uncertainty honest
- [ ] deterministic core
- [ ] stable seeded uncertainty
- [ ] no Random.Shared
- [ ] diagnostics
- [ ] unit tests

## 168D — Faction

- [ ] one faction mutation authority
- [ ] hostile attributed consequences
- [ ] friendly support only if valued
- [ ] truthful hostile messaging may still anger
- [ ] exposed lie stronger penalty if attributed
- [ ] no direct stance shift from effectiveness
- [ ] no one-campaign instant total war
- [ ] stable consequence IDs
- [ ] no rumor/direct duplicate faction effect

## 168D — Moral/NPC/Quest

- [ ] morality considers intent/target/deception
- [ ] MoralChoice owns moral state
- [ ] pre-commit preview where known
- [ ] no hidden moral arithmetic
- [ ] fear/division can carry cost even if factual
- [ ] NPC memory only for actual recipients
- [ ] no all-NPC clone
- [ ] dialogue predicates use known campaign state
- [ ] quest runtime remains owner
- [ ] propaganda quest hooks treated as content
- [ ] defector quest gated on real defection system

## 168D — Economy/Refugees/Recruitment

- [ ] trade prices not directly mutated
- [ ] refugee flow not directly mutated
- [ ] recruitment only if authority exists
- [ ] defection only if lifecycle exists
- [ ] civilian allegiance not invented
- [ ] shelter-targeted propaganda uses existing morale/info rails
- [ ] no direct broad morale arithmetic

## 168E — UI

- [ ] panel route decision justified
- [ ] campaign overview
- [ ] authored message creation flow
- [ ] no mandatory free-text semantic input
- [ ] custom text escaped/cosmetic if allowed
- [ ] distribution planner
- [ ] factorized effectiveness
- [ ] faction morale only displayed if canonical
- [ ] detection knowledge-gated
- [ ] compromised status grounded
- [ ] cancellation
- [ ] finite campaign duration
- [ ] journal bounded
- [ ] tutorial only if framework exists
- [ ] tooltips
- [ ] accessibility
- [ ] localization

## 168E — Persistence

- [ ] only message/campaign/delivery new state
- [ ] no duplicate faction/rumor/radio/inventory/moral/morale state
- [ ] old-save empty state
- [ ] print production owner persists job
- [ ] expedition owner persists distribution trip
- [ ] radio delivery atomic
- [ ] rumor owner persists propagation
- [ ] reload idempotence

## 168E — Anti-Spam

- [ ] saturation
- [ ] real resource cost
- [ ] real labor cost
- [ ] physical access cost
- [ ] channel capacity if available
- [ ] action budget
- [ ] duplicate delivery no-op
- [ ] save-scum detection reroll prevented
- [ ] credibility farming prevented
- [ ] lie spam prevented
- [ ] cross-channel saturation
- [ ] friendly-standing farm prevented

## 168E — Balance/Simulation

- [ ] propaganda not replacement for combat/trade/diplomacy
- [ ] truth not artificially weak
- [ ] lie short-term/long-term tradeoff
- [ ] themes use real conditions
- [ ] only supported objectives ship
- [ ] costs scale with channel
- [ ] political retaliation bounded
- [ ] no automatic war
- [ ] credibility recovery policy
- [ ] attribution ambiguity if supported
- [ ] 30-day light-use
- [ ] 120-day mixed campaign
- [ ] 180-day aggressive campaign
- [ ] no-propaganda case
- [ ] truth-only viable
- [ ] deception-heavy risk
- [ ] multi-faction isolation
- [ ] counter-message scenario if supported

## 168E — CI

- [ ] template integrity
- [ ] claim integrity
- [ ] faction integrity
- [ ] channel integrity
- [ ] resource integrity
- [ ] propaganda selftest
- [ ] source-scan authority gate
- [ ] deliberate failure fixtures
- [ ] generated docs
- [ ] content acceptance
- [ ] reachability
- [ ] headless
- [ ] verify-fast

## 168F

- [ ] faction autonomous messaging audited
- [ ] scripted Verdict broadcasts reused where possible
- [ ] counter-messages use same information semantics
- [ ] evidence-based rebuttal
- [ ] shelter reception uses existing morale/info
- [ ] no hidden counter points
- [ ] no omniscient response
- [ ] counter-intel requires evidence
- [ ] counter-loop capped
- [ ] response cooldown
- [ ] defer if faction AI rails absent

## 168G

- [ ] broadcast hijack gated on real frequency mechanics
- [ ] defectors gated on real allegiance lifecycle
- [ ] civilian allegiance gated on real population authority
- [ ] unit psyops gated on real unit morale
- [ ] fake orders deferred without command/authentication
- [ ] forged broadcasts deferred without source authentication
- [ ] deep deception remains content layer
- [ ] archive uses Plan 162
- [ ] propaganda art is presentation follow-on
- [ ] epilogue derives from campaign history

---

# 47. Ship / No-Ship Gate

**SHIP** only if:

```text
propaganda_authorities == 1
AND duplicate_rumor_propagation_systems == 0
AND duplicate_radio_transmission_systems == 0
AND messages_without_delivery_causing_effects == 0
AND faction_effect_without_information_or_attribution_rule == 0
AND generic_unowned_faction_morale_state == 0
AND propaganda_owned_faction_standing == false
AND propaganda_owned_market_prices == false
AND propaganda_owned_refugee_flow == false
AND propaganda_owned_defector_lifecycle == false
AND propaganda_owned_survivor_morale_state == false
AND unseeded_propaganda_rng == 0
AND daily_inactive_campaign_detection_rolls == 0
AND repeated_message_unbounded_stacking == false
AND free_text_controls_mechanical_effects == false
AND exposed_lies_without_evidence_path == 0
AND unsupported_channels_shipped_as_fake_buttons == 0
AND old_save_propaganda == pass
AND propaganda_save_roundtrip == pass
AND propaganda_reload_idempotence == pass
AND propaganda_delivery_required == pass
AND propaganda_attribution_gate == pass
AND propaganda_credibility_integrity == pass
AND propaganda_saturation == pass
AND propaganda_determinism == pass
AND propaganda_reachability == pass
AND propaganda_30_day_balance == pass
AND propaganda_120_day_balance == pass
AND propaganda_180_day_balance == pass
AND propaganda_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 48. Implementer Handoff

1. Audit printing, radio, rumor, faction and MoralChoice seams before creating `PropagandaSystem`.
2. Decide whether `StencilPropagandaSmearEntry` is reusable, migratable, or narrative-only.
3. Do not create faction morale unless the repository already has a clear authority/consumer contract for it.
4. Separate authored template from produced message instance.
5. Represent truth/half-truth/lie/exaggeration as structured claim semantics, not magic multipliers.
6. Keep custom player-written text cosmetic unless a future system can safely interpret it.
7. Make message creation, production, delivery, reach, attribution, belief and consequence separate transitions.
8. Use real paper/ink/labor for print.
9. Use real transmitter/power/frequency for radio.
10. Seed rumors into Plan 131 rather than reimplementing spread.
11. Require real expedition/location interaction for leaflets/wall postings where possible.
12. Defer broadcast hijacking unless frequency-control mechanics actually exist.
13. Track credibility separately from faction friendship.
14. Expose lies only through evidence/information paths.
15. Use saturation and finite channel capacity to stop spam.
16. Route faction effects through the Plan-139 faction mutation authority.
17. Route moral consequences through `MoralChoiceSystem`.
18. Route personal reactions through Plan 147.
19. Route trade/refugee/recruitment/defection only if their authorities have explicit adapters.
20. Persist messages/campaigns/deliveries, not duplicated downstream state.
21. Make counter-propaganda use the same information rules and knowledge gates.
22. Author 15 templates only after every template can be produced, delivered and observed.
23. Run 30/120/180-day truth/deception/spam scenarios.
24. Close only when propaganda feels like information warfare—logistics, credibility, audience, and consequences—not a menu that subtracts points from an enemy faction.

---

# 49. Final Outcome

When this plan is complete, ASHFALL gains information warfare without creating a second invisible war simulator.

The player can create a message because a real survivor and real shelter infrastructure can produce it. Leaflets consume printing resources. Physical distribution requires access to the target area. Radio messages use the existing transmitter and power system. Rumor campaigns enter the existing rumor network instead of bypassing it.

A delivered message then follows a coherent information path.

Someone must actually receive it.
They may or may not know who sent it.
They may or may not believe it.
They may later discover that a claim was true, misleading, exaggerated, or false.

That distinction gives credibility real strategic value.

A shelter that repeatedly broadcasts verifiable truths can become a source that even enemies grudgingly believe. A shelter that relies on deception can exploit information gaps for short-term gains but risks a durable credibility collapse when its claims are exposed. None of that is the same thing as faction friendship.

Political consequences remain in the faction system. Moral consequences remain in MoralChoice. Personal resentment or admiration remains in NPC memory. Radio coverage remains in radio. Rumor spread remains in the information network. Expeditions remain the authority for physical distribution. Trade prices, refugee flow, recruitment, and defections change only if their own systems expose a real consequence seam.

Counter-propaganda follows the same rules. Enemy factions cannot magically negate a secret campaign they do not know exists. If they respond, they must do so through broadcasts, rumors, evidence, or other information channels the game already understands.

Most importantly, the player cannot win an information war by pressing the same button every day.

Resources are finite.
Channels have limits.
Audiences saturate.
Credibility persists.
Evidence can surface.
Attribution matters.

The result is a propaganda system that behaves like the rest of ASHFALL:

grounded in real state, constrained by logistics, and dangerous because the world remembers what the player chose to say.
