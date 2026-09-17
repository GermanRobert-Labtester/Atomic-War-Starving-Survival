# C1 — Flagship Integration Plan [39]: Shelter Reputation, External Perception, Notoriety & Wasteland Identity

> **Output:** `C1_planintegration[39].md`
>
> **Source baseline:** Plan 207 — Shelter Reputation & External Perception System
>
> **Primary mission:** make the shelter a known external actor whose conduct becomes remembered, interpreted and propagated across the wasteland—producing durable perceptions such as reliable, dangerous, generous, feared, sanctuary-like or treacherous—while preserving existing authorities for faction trust, morality, propaganda, trade pricing, visitors, raids, recruitment, diplomacy, information propagation, settlement identity and combat outcomes.
>
> **Primary architectural rule:** `ShelterReputationSystem` owns **publicly knowable shelter-level perception facts**: reputation evidence, global/public perception dimensions, notoriety/awareness, active public reputation tags, audience scope, source provenance, confidence, decay/forgetting policy, and reputation-history projection. It does **not** own faction trust, moral truth, market price, raid scheduling, visitor spawning, recruitment success, diplomatic state, propaganda production, shelter identity, combat results, or rumor propagation.
>
> **Primary correction to the source plan:** the source proposes `overallReputation`, `reputationByFaction`, `trustModifier`, direct trade/raid/recruitment modifiers, and a universal neutral-decay score. Those risk duplicating `FactionStanceEngine`, Plan 131 information flow, Plan 168 propaganda, MarketSystem, raid logic and MoralChoiceSystem. The flagship instead models reputation as **evidence-derived external perception**, split between public/global impressions and audience-specific beliefs. Faction-specific trust remains in `FactionStanceEngine`; reputation can influence faction reactions only through typed perception inputs.
>
> **Primary epistemic rule:** the shelter does not gain reputation merely because an event happened. The event must become **known externally**. A humanitarian act, defended raid, broken agreement or atrocity only changes external perception after information reaches an audience through witnesses, traders, radio, faction reports, propaganda, refugees, surviving attackers, settlement contacts or another canonical information path.
>
> **Primary notoriety rule:** notoriety means **how widely the shelter is known**, not whether it is liked. High notoriety can amplify both admiration and hostility. Low notoriety limits downstream effects because many actors simply do not know enough about the shelter.
>
> **Primary moral rule:** reputation is perception, not objective morality. A morally good action can be misunderstood. A morally bad action can remain secret. Propaganda can distort perception without changing moral truth. `MoralChoiceSystem` may produce reputation evidence, but the two systems must never share a single score.
>
> **Primary decay rule:** reputation does not blindly drift toward a universal neutral number. Different evidence types have different memory/salience. Major atrocities, treaty betrayals, legendary defenses or sanctuary behavior may persist; routine trades may fade quickly. Decay applies to **perception evidence and tag confidence/salience**, not historical truth.
>
> **Primary downstream rule:** visitor quality, trade terms, raid behavior, recruitment and diplomacy consume reputation as **one bounded input** alongside their own canonical factors. Reputation never directly mutates their final outcomes.
>
> **Mandatory execution order:** 207A authority/information audit → 207B reputation evidence contract → 207C public perception dimensions/tags → 207D notoriety and audience awareness → 207E information propagation/propaganda integration → 207F faction perception vs faction trust separation → 207G trade/visitor/raid/recruitment/diplomacy adapters → 207H decay, contradiction and reputation correction → 207I UI/map/history/explainability → 207J persistence/migration/determinism/anti-farm → 207K 30/120/180/400-day simulations and CI → 207L advanced myth, legends and cross-settlement reputation markets only after the core is proven.
>
> **Critical re-baseline rule:** before creating `ShelterReputationSystem.cs`, inspect `FactionStanceEngine`, `AirlockSecuritySystem`, `HoldfastTradeSession`, MarketSystem, raid/defense authority, RecruitmentSystem/Plan 204, FactionDiplomacySystem/Plan 197, MoralChoiceSystem, Plan 131 information/rumor network, Plan 166 shelter identity/naming, Plan 168 propaganda, Plan 138 visitor quality/defense references, Plan 199 migration/trader/refugee flows, expedition encounter logic, journal/archive/history, semantic event bus and save orchestration.
>
> **Guardrails:** no second faction trust dictionary; no `trustModifier` state owned by reputation; no duplicate morality score; no duplicate propaganda system; no reputation gain from unknown secret events; no global omniscient reaction; no direct `tradePrice *= reputation`; no direct `raidFrequency *= reputation`; no direct recruitment roll owned by reputation; no automatic defection/tribute without faction/diplomacy support; no "rich/poor" tag calculated from hidden inventory without external evidence; no "strong/weak" tag directly from internal defense stats unless outsiders know them; no "cruel" tag merely because a refugee was rejected without context/knowledge; no universal daily drift-to-50; no per-frame reputation decay; no event spam; no unseeded RNG; no `Guid.NewGuid`; no wall clock; no old-save retroactive reputation from all historical actions; no quest farming by repeating trivial trades or refugee accept/reject toggles.

---

# 0. Mission

ASHFALL already has many actions that *could* shape how the wasteland sees the shelter, but there is no canonical shelter-level public perception layer.

The source baseline identifies:

- `FactionStanceEngine.cs` tracking bilateral faction trust;
- `AirlockSecuritySystem.cs` handling visitors;
- `HoldfastTradeSession.cs` handling trade;
- `MoralChoiceSystem.cs` tracking moral choices;
- Plans 138, 166 and 168 mentioning shelter reputation conceptually;
- no actual `ShelterReputationSystem`;
- no notoriety;
- no fame/infamy;
- no global public tags;
- no reputation decay/growth;
- no unified external perception.

Current shape:

```text
ACTION
  │
  ├── faction trust may change
  ├── morality may change
  ├── trade may happen
  ├── visitor may arrive
  ├── raid may occur
  └── propaganda may happen

but the shelter itself has no persistent external identity.
```

Target shape:

```text
CANONICAL EVENT
      │
      ▼
PUBLICITY / INFORMATION PATH
      │
      ├── witness
      ├── traveler
      ├── radio
      ├── faction report
      ├── refugee testimony
      ├── propaganda
      ├── trader gossip
      └── surviving attacker
      │
      ▼
ReputationEvidence
      │
      ├── audience scope
      ├── confidence
      ├── salience
      ├── interpretation tags
      ├── source provenance
      └── notoriety contribution
      │
      ▼
ShelterReputationSystem
      │
      ├── public dimensions/tags
      ├── notoriety
      ├── audience-specific perception
      ├── contradiction/correction
      ├── decay/forgetting
      └── history/read models
      │
      ▼
CANONICAL CONSUMERS
      ├────────► FactionStanceEngine
      ├────────► visitors/airlock
      ├────────► Market / trade
      ├────────► raids/threat AI
      ├────────► recruitment
      ├────────► diplomacy
      ├────────► migration/traders/refugees
      └────────► Plan 171 narrative hooks
```

The reputation system should answer:

> What does the outside world currently believe about this shelter, how widely is that belief known, which evidence caused it, which audiences know it, and how reliable or salient is that perception?

It should not answer:

> Is the shelter objectively moral?
> Does Faction A trust the player?
> What is today's trade price?
> Will a raid happen?
> Which visitor spawns?
> Will a recruit accept?
> Did propaganda succeed?
> Is the shelter actually rich?

Those remain canonical systems.

---

# 1. Source-Evidence Interpretation

## 1.1 A shelter-level reputation system is genuinely absent

The source reports zero Core matches for:
- `ShelterReputation`;
- `ReputationSystem`;
- `ExternalPerception`;
- `BunkerReputation`;
- `WorldReputation`;
- `BunkerFame`;
- `InfamySystem`.

A new shelter-level perception authority is justified.

## 1.2 Faction trust is not global reputation

`FactionStanceEngine` is bilateral.

Example:

```text
Faction A trusts shelter = +70
Faction B distrusts shelter = -40
```

That is not the same as:

```text
publicly known as reliable
notoriety = high
```

The systems can interact, but must stay distinct.

## 1.3 Plan 131 is critical

External perception cannot be omniscient.

Plan 131 owns:
- news;
- rumor;
- intelligence propagation.

Reputation should consume:
- delivered information;
- publicized event evidence;
- audience reach.

## 1.4 Plan 168 propaganda can distort perception

Propaganda may:
- amplify;
- suppress;
- distort;
- frame.

It must not directly rewrite truth.

Reputation consumes the propagated propaganda claim.

## 1.5 “Overall reputation 0–100” is too lossy

A shelter can be:
- feared and respected;
- generous and dangerous;
- reliable but cruel;
- rich but treacherous.

A single scalar cannot capture this.

Recommended:
- no primary universal approval score.

Use:
- dimensions/tags;
- notoriety;
- optional derived summary only for UI.

## 1.6 “Rich/poor” and “strong/weak” require knowledge

External actors may not know exact:
- inventory;
- defenses;
- population.

These tags should arise from:
- observed trade volume;
- visible fortifications;
- raid outcomes;
- traveler reports;
- leaks.

## 1.7 “Refugee rejection = cruel” is context-sensitive

Rejecting:
- healthy travelers due to policy;
- desperate children during plague;
- armed infiltrators
are not equivalent.

MoralChoice may classify ethical context.

Reputation interpretation also depends on:
- what outsiders heard.

## 1.8 Reputation effects must be bounded inputs

The source proposes direct effects:
- fewer raids;
- better trade;
- recruitment;
- diplomacy;
- defection;
- tribute.

These should be adapters, not ownership transfer.

---

# 2. Non-Negotiable Reputation Invariants

## INV-207.1 — Reputation is perception, not truth

## INV-207.2 — No external perception without information reach

## INV-207.3 — Faction trust remains canonical in `FactionStanceEngine`

## INV-207.4 — Moral truth remains canonical in `MoralChoiceSystem`

## INV-207.5 — Propaganda remains canonical in Plan 168

## INV-207.6 — Information propagation remains canonical in Plan 131

## INV-207.7 — Trade price remains canonical in MarketSystem/trade session

## INV-207.8 — Raid scheduling remains canonical

## INV-207.9 — Visitor generation remains canonical

## INV-207.10 — Recruitment outcome remains canonical

## INV-207.11 — Diplomacy state remains canonical

## INV-207.12 — Notoriety is awareness, not approval

## INV-207.13 — Public tags may coexist

## INV-207.14 — Public tags require evidence provenance

## INV-207.15 — Audience-specific perceptions are not a duplicate trust score

## INV-207.16 — Reputation decay affects belief strength/salience, not historical truth

## INV-207.17 — Major evidence can be sticky

## INV-207.18 — Contradictory evidence is preserved and reconciled, not overwritten blindly

## INV-207.19 — One source event contributes once per audience/evidence path

## INV-207.20 — No per-frame decay/scan

## INV-207.21 — Old saves do not retroactively infer reputation

## INV-207.22 — Reputation downstream modifiers are bounded and explainable

## INV-207.23 — UI never presents derived perception as objective truth

## INV-207.24 — Headless reputation is UI-independent

---

# 3. Definition of Done

Plan 207 closes only when:

- faction trust authority is documented;
- information/rumor authority is documented;
- propaganda authority is documented;
- morality authority is documented;
- trade/visitor/raid/recruitment/diplomacy authorities are documented;
- shelter identity/naming is integrated;
- reputation evidence has stable source IDs;
- reputation evidence includes audience/reach;
- secret events do not change public reputation;
- a multi-dimensional/tag model replaces universal approval as primary truth;
- notoriety is separate from positive/negative perception;
- audience-specific perception is separate from faction trust;
- faction trust can consume public perception exactly once through an adapter;
- propaganda can alter public perception without changing moral truth;
- witnesses/traders/refugees/raiders can carry reputation evidence only through real information paths;
- contradictory reports can coexist;
- reputation dimensions/tags have bounded accumulation;
- decay is data-driven by evidence/tag type;
- no universal drift-to-neutral assumption is hardcoded;
- major events can persist long-term;
- trade uses reputation as one bounded input;
- visitor generation uses reputation as one bounded input;
- raid/threat systems use reputation as one bounded input;
- recruitment uses reputation as one bounded input;
- diplomacy uses reputation as one bounded input;
- defection/tribute effects are deferred unless real faction systems support them;
- external "strong/rich" perceptions require known evidence;
- old saves start with neutral/unknown external perception and no retroactive history;
- new events begin accumulating after feature activation;
- save/load preserves evidence, tags, awareness and history;
- no double application after reload;
- repeated trivial trades cannot farm infinite reputation;
- 30/120/180/400-day simulations prove reputation is persistent but not permanently frozen;
- `--shelter-reputation-selftest` exists or equivalent;
- all reputation-event definitions have real producers and consumers.

---

# 4. Phase P0 — Authority, Information & Consumer Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
FactionStanceEngine state/APIs
Plan 131 rumor/intelligence APIs
Plan 168 propaganda APIs
MoralChoiceSystem
AirlockSecuritySystem
visitor generation/quality
HoldfastTradeSession
MarketSystem
raid/threat system
shelter defense outcomes
RecruitmentSystem / Plan 204
FactionDiplomacySystem / Plan 197
Plan 166 shelter identity/name
Plan 138 visitor/defense reputation hooks
Plan 199 migration/trader/refugee flows
Expedition encounter system
journal/history/archive
semantic event bus
campaign clock
save order
UI faction/map/reputation surfaces
```

## P0.2 Build reputation authority matrix

Create:

`docs/reputation/SHELTER_REPUTATION_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
source API/event
reputation role
consumer API
persisted?
status
```

Rows:
- shelter name/identity;
- public awareness;
- notoriety;
- moral truth;
- faction trust;
- propaganda claim;
- rumor propagation;
- event history;
- raid outcome;
- trade outcome;
- refugee treatment;
- treaty fulfillment;
- combat reputation;
- visitor generation;
- trade price;
- recruitment;
- diplomacy;
- external perception tag;
- audience perception;
- reputation evidence.

## P0.3 Global-vs-faction ADR

Create:

`docs/architecture/ADR_PUBLIC_REPUTATION_VS_FACTION_TRUST.md`

## P0.4 Reputation-vs-morality ADR

Create:

`ADR_REPUTATION_VS_MORALITY.md`

## P0.5 Reputation-vs-information ADR

Create:

`ADR_REPUTATION_AND_INFORMATION_PROPAGATION.md`

## P0.6 Decay ADR

Create:

`ADR_REPUTATION_EVIDENCE_DECAY.md`

## P0.7 Baseline proof

Demonstrate:
- faction trust exists;
- no shelter notoriety;
- no public tags;
- no global external-perception read model.

---

# TASK 207A — Reputation Evidence Contract

# 207A.0 Goal

Represent what outside actors learned about the shelter, not simply what happened internally.

## 207A.1 Data file

`Assets/StreamingAssets/Data/reputation_events.json`

Use as:
- evidence interpretation rules.

## 207A.2 Event definition DTO

Suggested:

```text
event_type_id
source_event_types[]
required_publicity
default_tag_contributions[]
default_notoriety_weight
audience_policy_id
salience_profile_id
decay_profile_id
confidence_policy_id
```

## 207A.3 Runtime evidence DTO

Suggested:

```text
ReputationEvidence
  evidence_id
  source_event_id
  event_type_id
  observed_day
  propagated_day
  source_actor_ref optional
  audience_scope
  confidence
  salience
  publicity_level
  tag_contributions[]
  notoriety_contribution
  framing_refs[]
  contradiction_group_id optional
  processed_consumer_ids[]
```

## 207A.4 Evidence ID

Stable.

Example:

```text
rep-evidence:<source_event>:<audience>:<propagation>
```

## 207A.5 Source event

Canonical semantic event.

## 207A.6 Publicity

Candidate:

```text
private
witnessed
local
regional
widespread
```

## 207A.7 Private event

No public reputation effect.

Could affect:
- participating faction trust;
- participant memory.

## 207A.8 Witnessed event

Evidence enters:
- witness/community information pipeline.

## 207A.9 Regional/widespread

Requires actual propagation.

## 207A.10 Confidence

How credible the information is.

## 207A.11 Salience

How memorable/important.

## 207A.12 Tag contributions

Do not directly modify final scores.

Evidence contributes to derived perception.

## 207A.13 Framing

Plan 168/rumors can modify:
- interpretation.

Example:
- raid defense reported as heroic;
- same event framed as brutal.

## 207A.14 Contradiction group

Can link:
- competing reports about same event.

## 207A.15 No localized description persisted as truth

Use IDs + structured facts.

## 207A.16 Evidence history

Bounded/compacted.

## 207A.17 Source examples

- raid defended;
- raid lost;
- refugee accepted;
- refugee rejected;
- trade fulfilled;
- trade fraud;
- faction aided;
- faction member killed;
- propaganda exposed;
- humanitarian act;
- atrocity;
- major discovery;
- notable death.

Only if real source event exists.

## 207A.18 Unsupported source

Definition fails reachability.

### 207A DoD

Reputation begins with source-backed, audience-aware evidence rather than omniscient direct score changes.

---

# TASK 207B — Reputation Dimensions & Tags

# 207B.0 Goal

Represent the shelter's external identity as several coexisting perceptions.

## 207B.1 Recommended tags

Source candidates:
- feared;
- respected;
- generous;
- cruel;
- reliable;
- treacherous;
- strong;
- weak;
- rich;
- poor;
- sanctuary;
- dangerous.

## 207B.2 Tag semantics

Each tag must have:
- evidence producers;
- consumer relevance;
- contradiction/compatibility rules.

## 207B.3 Feared

Means:
- outsiders expect retaliation/cost/risk.

Can arise from:
- visible military success;
- punitive raids;
- surviving attacker reports.

## 207B.4 Respected

Means:
- outsiders perceive competence/honor/status.

Not universal approval.

## 207B.5 Generous

Requires externally known:
- aid/resource sharing.

## 207B.6 Cruel

Requires externally known conduct interpreted as cruelty.

Do not map all refugee rejection automatically.

## 207B.7 Reliable

Requires:
- fulfilled agreements;
- consistent trade/commitments.

## 207B.8 Treacherous

Requires:
- broken agreements;
- betrayal;
- proven fraud;
- exposed deception.

## 207B.9 Strong

Requires observed:
- defenses;
- combat outcomes;
- population/military presence.

## 207B.10 Weak

Requires observed:
- defeats;
- breaches;
- inability to defend;
- distress.

## 207B.11 Rich

Requires perceived abundance:
- visible trade;
- high-value goods;
- reports.

Not hidden stockpile truth.

## 207B.12 Poor

Requires perceived scarcity/desperation.

## 207B.13 Sanctuary

Requires:
- refugee/medical aid behavior;
- safety reputation.

## 207B.14 Dangerous

Means:
- aggressive/unpredictable threat.

Distinct from Feared.

## 207B.15 Compatible tags

Possible:
- feared + respected;
- strong + dangerous;
- generous + rich;
- sanctuary + respected.

## 207B.16 Contradictory tags

Strong and weak can both have recent evidence.

Do not simply make them mathematically impossible.

Use:
- recency/salience/confidence.

## 207B.17 Tag magnitude

0–100 internal acceptable.

Derived from evidence.

## 207B.18 Tag confidence

Separate from magnitude if needed.

Example:
- strong belief with low evidence credibility.

Prefer:
- salience-weighted evidence projection.

## 207B.19 Overall reputation

Do not persist as primary truth.

Optional UI summary:

```text
public standing band
```

derived from tags.

## 207B.20 Summary labels

Examples:
- obscure;
- respected outpost;
- feared stronghold;
- controversial sanctuary;
- notorious raider enclave.

Derived.

## 207B.21 No moral alignment label

### 207B DoD

External identity becomes multi-dimensional and capable of representing ambiguous reputations instead of collapsing the shelter into a single good/bad number.

---

# TASK 207C — Notoriety & Awareness

# 207C.0 Goal

Separate how widely the shelter is known from what people think about it.

## 207C.1 Notoriety definition

```text
public awareness / recognition
```

## 207C.2 Not approval

A notorious shelter may be:
- admired;
- hated;
- feared;
- controversial.

## 207C.3 Scale

0–100 internal.

## 207C.4 Inputs

- widely witnessed combat;
- high-volume trade;
- propaganda;
- radio activity;
- humanitarian events;
- famous discovery;
- major disaster;
- migration/refugee testimony.

## 207C.5 No internal-only input

A legendary discovery hidden in storage creates no notoriety.

## 207C.6 Audience awareness

Notoriety can be:
- global projection;
- region/faction audience distribution.

## 207C.7 MVP

Persist:
- global notoriety;
- audience awareness bands.

## 207C.8 Audience types

Possible:
- factions;
- traders;
- refugees/travelers;
- raiders;
- settlements.

Use real IDs/groups.

## 207C.9 High notoriety effect

Makes reputation evidence:
- more likely to be known;
- more actors aware of shelter.

Downstream systems decide interaction.

## 207C.10 Low notoriety

No hidden direct bonus.

Simply:
- fewer actors have perception data.

## 207C.11 Decay

Awareness can fade if:
- no new stories;
- time passes.

Major historical fame may have floor.

## 207C.12 Regionality

A shelter may be famous locally, obscure far away.

If Plan 131 supports spatial propagation:
- use it.

## 207C.13 No omniscient global 0–100 if regional model is available

## 207C.14 Notoriety source trace

UI can show:
- “Raid victory spread through trader reports.”

## 207C.15 No daily log

### 207C DoD

Notoriety measures how far the shelter's name has traveled, enabling fame and infamy without conflating visibility with approval.

---

# TASK 207D — Information Propagation & Plan 131 Integration

# 207D.0 Goal

Make reputation depend on actual information flow.

## 207D.1 Plan 131 owns propagation

Reputation does not move rumors itself.

## 207D.2 Integration flow

```text
source event
→ Plan 131 information item/report
→ delivered to audience
→ reputation evidence created/updated
```

## 207D.3 Direct witness path

If actor witnessed:
- immediate local evidence.

## 207D.4 Trader path

Traveler can carry:
- trade-related;
- visible shelter stories.

## 207D.5 Refugee path

Plan 199 refugee group may spread:
- sanctuary;
- cruelty;
- scarcity;
- safety reports.

## 207D.6 Raider survivor path

Can spread:
- strength;
- fear;
- weakness.

Only if attackers survive/escape/report.

## 207D.7 Radio path

Radio system can broadcast:
- propaganda;
- emergency;
- public statements.

## 207D.8 Faction report

Faction-specific knowledge.

## 207D.9 Secret actions

No propagation.

## 207D.10 Unknown perpetrator

If shelter not identified:
- no shelter reputation effect.

## 207D.11 Misattribution

If Plan 131 supports false attribution:
- reputation belief can be wrong.

Canonical source truth remains.

## 207D.12 Contradictory rumor

Audience can hold:
- mixed evidence.

## 207D.13 Reliability

Plan 131 source confidence can feed reputation evidence confidence.

## 207D.14 Correction

Later confirmed report can:
- weaken false perception;
- increase reliable tag.

## 207D.15 Information deletion/forgetting

Plan 131/Plan 185 may reduce propagation salience.

Reputation projection updates.

## 207D.16 No duplicate rumor archive

Store source refs.

### 207D DoD

The shelter becomes known only through witnesses, reports, travelers, radio and faction communication, preserving information asymmetry and allowing reputation to be incomplete or wrong.

---

# TASK 207E — Propaganda & Framing Integration

# 207E.0 Goal

Let Plan 168 manipulate perception without changing objective history.

## 207E.1 Propaganda authority

Plan 168.

## 207E.2 Reputation input

Propaganda can create:

```text
framed reputation evidence
```

## 207E.3 Positive campaign

May amplify:
- reliable;
- sanctuary;
- strong;
- respected.

## 207E.4 Negative enemy propaganda

May amplify:
- treacherous;
- cruel;
- weak;
- dangerous.

## 207E.5 Detected deception

Source plan suggests:
- detected propaganda → treacherous.

Only if:
- audience detects deception;
- shelter is identified as responsible.

## 207E.6 No automatic global penalty

Affected audience only.

## 207E.7 Truth vs claim

Evidence stores:
- source claim;
- confidence;
- framing.

## 207E.8 Propaganda cannot erase contradictory evidence instantly

## 207E.9 Repetition

Diminishing returns.

## 207E.10 Saturation

No infinite notoriety from broadcasting same message.

## 207E.11 Counter-propaganda

Plan 168.

Reputation consumes result.

## 207E.12 Exposure event

Can be high-salience.

## 207E.13 Propaganda reach

Plan 131/radio.

## 207E.14 No reputation-owned propaganda skill

### 207E DoD

Propaganda shapes external perception through audience-specific claims and framing while truth, propaganda production and information reach remain separate authorities.

---

# TASK 207F — Faction-Specific Perception vs `FactionStanceEngine`

# 207F.0 Goal

Allow factions to react to the shelter's public image without building a second faction reputation score.

## 207F.1 Do not persist `reputationByFaction` as duplicate trust

Instead persist:
- audience evidence/perception.

## 207F.2 Faction perception

Can include:
- public tags known by faction;
- faction-specific evidence;
- notoriety awareness.

## 207F.3 Faction trust

Remains:
- `FactionStanceEngine`.

## 207F.4 Adapter

Suggested:

```text
FactionPublicPerceptionInput
  faction_id
  known_tags[]
  notoriety_band
  confidence
  salient_evidence_refs[]
```

## 207F.5 Trust effect

FactionStanceEngine may use:
- public perception input
as one modifier.

## 207F.6 No persisted `trustModifier`

Compute on demand.

## 207F.7 Faction ideology/doctrine

Different factions interpret same tag differently.

Example:
- militarist faction may respect feared/strong;
- humanitarian faction may dislike cruel.

Faction authority owns interpretation policy or reputation adapter data.

## 207F.8 Same action, different audiences

Important.

## 207F.9 Direct interaction

Faction trust can change even if event is private.

That is not public reputation.

## 207F.10 Public reputation can affect first impression

For faction not yet met:
- if it has heard of shelter.

## 207F.11 No trust decay inside reputation

## 207F.12 No double application

If original event already changed faction trust directly:
- public reputation adapter must not reapply identical full consequence.

Use separate:
- direct relationship effect;
- public-perception effect.

## 207F.13 Audit examples

- killed faction member;
- aided faction;
- honored treaty;
- betrayed treaty.

### 207F DoD

Faction-specific knowledge of the shelter can influence faction reactions while bilateral trust remains one canonical state and direct-event effects are never double-counted.

---

# TASK 207G — Visitor & Airlock Integration

# 207G.0 Goal

Make the shelter's public image influence who considers visiting without letting reputation own visitor spawning.

## 207G.1 `AirlockSecuritySystem` / visitor generator owns arrivals

## 207G.2 Reputation supplies visitor-attraction profile

Suggested:

```text
VisitorReputationContext
  notoriety
  known_tags[]
  audience_confidence
```

## 207G.3 High notoriety

May increase:
- candidate pool;
- encounter frequency
if visitor system chooses.

## 207G.4 Sanctuary

May attract:
- refugees;
- patients;
- aid seekers.

Plan 199/refugee/visitor system owns actual arrival.

## 207G.5 Rich

May attract:
- traders;
- opportunists;
- thieves
if systems support.

## 207G.6 Strong/feared

Could deter:
- casual raiders;
- desperate thieves.

Not all visitors.

## 207G.7 Dangerous/cruel

May reduce:
- peaceful visitors;
- increase hostile/scavenger interest
depending on producer.

## 207G.8 Visitor quality

Avoid universal:
`high rep = skilled friendly`.

Different tags attract different people.

## 207G.9 Skilled recruit

RecruitmentSystem decides.

## 207G.10 No hidden visitor guarantee

Reputation changes weights only.

## 207G.11 Information requirement

Visitor must know reputation.

## 207G.12 No reputation use if visitor origin never received information

### 207G DoD

External perception changes the composition and likelihood of visitor candidates through bounded weights while visitor generation and screening remain canonical.

---

# TASK 207H — Trade & Market Integration

# 207H.0 Goal

Make reliability and notoriety matter in trade without duplicating market price authority.

## 207H.1 MarketSystem owns price

## 207H.2 HoldfastTradeSession owns transaction terms/session

## 207H.3 Reputation supplies trade context

Candidate:

```text
TradeReputationContext
  reliable
  treacherous
  feared
  rich
  poor
  notoriety
  audience_confidence
```

## 207H.4 Reliable

May improve:
- credit willingness;
- contract access;
- negotiation floor;
- quantity access.

Only if trade system supports.

## 207H.5 Treacherous

May:
- require upfront payment;
- reduce credit;
- restrict rare goods.

## 207H.6 Rich

May make seller:
- raise expectations;
- bring premium stock.

Do not guarantee lower prices.

## 207H.7 Poor/desperate

Could:
- worsen bargaining position;
- attract aid/debt offers.

## 207H.8 Feared

Could:
- deter cheating;
- increase risk premium.

No universal discount.

## 207H.9 No direct multiplier table inside reputation

## 207H.10 Contract reputation

Plan 192/economy agreement system may consume reliable tag.

## 207H.11 Fair trade event

A routine trade should not endlessly increase reliable.

Require:
- meaningful contract fulfillment;
- repeat diminishing return;
- trade volume threshold.

## 207H.12 Fraud

Only if cheating mechanic exists.

## 207H.13 Unknown trade

No reputation event if private and no witnesses/report.

### 207H DoD

Trade systems consume relevant external-perception context while actual prices, stock, contract terms and transaction state remain canonical.

---

# TASK 207I — Raid, Threat & Defense Integration

# 207I.0 Goal

Make perceived strength, fear and wealth influence threat behavior without making reputation the raid scheduler.

## 207I.1 Raid authority owns:
- target selection;
- frequency;
- strength;
- timing.

## 207I.2 Reputation context

Suggested:

```text
RaidReputationContext
  feared
  strong
  weak
  rich
  dangerous
  notoriety
  confidence
```

## 207I.3 Feared

Can increase expected cost.

Potential:
- deterrence.

## 207I.4 Strong

Same.

## 207I.5 Weak

Can increase perceived vulnerability.

## 207I.6 Rich

Can increase perceived reward.

## 207I.7 Notoriety

More raider groups may know shelter exists.

## 207I.8 Net effect

Not simple:
- feared = fewer raids.

A rich, famous, strong shelter might:
- attract powerful organized attackers;
- deter weak raiders.

Raid system decides.

## 207I.9 Raid defended

Creates evidence only if:
- attackers/witnesses/report survive;
- event becomes known.

## 207I.10 Raid lost

Same.

## 207I.11 Hidden defenses

Do not make shelter appear strong based on internal defense score alone.

## 207I.12 Failed raid attackers

Survivors can spread fear.

## 207I.13 No direct attack-frequency field in reputation

### 207I DoD

Raid AI can reason about publicly known strength, weakness, wealth and fear as bounded target-selection inputs without surrendering control of raid generation or difficulty.

---

# TASK 207J — Recruitment & Migration Integration

# 207J.0 Goal

Let external identity affect who wants to join or approach the shelter through real recruitment and migration systems.

## 207J.1 RecruitmentSystem / Plan 204 owns acceptance/success

## 207J.2 Reputation context

Possible:
- sanctuary;
- reliable;
- cruel;
- dangerous;
- strong;
- notoriety.

## 207J.3 Sanctuary

Can increase:
- applicants/refugees seeking safety.

## 207J.4 Strong

May attract:
- security-minded survivors;
- specialists.

## 207J.5 Cruel/dangerous

May repel:
- some candidates;
- attract hardliners
if personality/recruitment system supports.

## 207J.6 Notoriety

Expands candidate awareness.

## 207J.7 No direct success percentage in reputation

## 207J.8 Plan 199 refugees

Their testimony can:
- create reputation evidence.

Shelter reputation can:
- influence destination attractiveness.

Plan 199 owns migration destination decision.

## 207J.9 Circularity guard

Avoid feedback runaway:

```text
sanctuary → more refugees → sanctuary → more refugees...
```

Use:
- diminishing returns;
- capacity;
- source population;
- notoriety saturation.

## 207J.10 Rejected refugee

Reputation effect depends on:
- context;
- witnesses;
- later reports;
- moral interpretation.

## 207J.11 Accepted refugee

Same.

## 207J.12 Recruitment anti-farm

Repeated low-stakes acceptance cannot generate endless generosity score.

### 207J DoD

Reputation influences who hears about and considers the shelter while recruitment and migration systems retain candidate generation, capacity and final admission authority.

---

# TASK 207K — Diplomacy, Treaty & Defection Integration

# 207K.0 Goal

Make public reputation affect diplomatic opportunity without duplicating diplomacy state.

## 207K.1 Plan 197 owns:
- treaties;
- alliances;
- diplomatic options;
- negotiations.

## 207K.2 Reputation input

- respected;
- reliable;
- treacherous;
- feared;
- strong;
- dangerous;
- notoriety.

## 207K.3 Respected/reliable

May unlock:
- willingness to negotiate;
- treaty trust prerequisites.

## 207K.4 Treacherous

May:
- raise guarantees required;
- close some options.

## 207K.5 Feared

Can create:
- deterrence;
- coercive negotiation.

No universal positive.

## 207K.6 Diplomatic doctrine

Faction interprets reputation differently.

## 207K.7 Defection chance

Source proposes:
- respected → defections.

Defer unless:
- faction member defection system exists.

## 207K.8 Tribute demands/offers

Defer unless:
- diplomacy/economy supports tribute.

## 207K.9 Ally requests

Plan 171/197 can generate:
- requests.

Reputation supplies eligibility.

## 207K.10 Direct treaty event

Honoring/breaking treaty can:
- directly change faction trust;
- produce public reliable/treacherous evidence.

Prevent duplicate full penalties.

## 207K.11 Reputation cannot create treaty state directly

### 207K DoD

Diplomacy uses public perception as a reputation context for willingness and available strategies while all treaty, alliance, tribute and defection state remains owned by the diplomacy/faction systems.

---

# TASK 207L — Morality & Humanitarian Reputation

# 207L.0 Goal

Convert morally meaningful actions into possible public perception without equating morality and reputation.

## 207L.1 MoralChoiceSystem owns moral evaluation

## 207L.2 Reputation consumes:
- moral event descriptor;
- publicity;
- audience interpretation.

## 207L.3 Humanitarian act

Could support:
- generous;
- sanctuary;
- respected.

## 207L.4 Atrocity

Could support:
- cruel;
- dangerous;
- feared.

Only if:
- canonical atrocity event exists;
- outside world learns it.

## 207L.5 Refugee rejection

Moral evaluation depends on context.

Do not hardcode:
- reject = cruel.

## 207L.6 Prisoner execution

Only if prison/execution system exists.

## 207L.7 Aid

Needs actual transfer/treatment.

## 207L.8 Secret atrocity

Morality changes.
Public reputation does not until exposed.

## 207L.9 False accusation

Public reputation may suffer even if MoralChoice truth says shelter did not commit act.

## 207L.10 Redemption

Later public humanitarian evidence can change perception.

It does not erase moral history.

## 207L.11 Moral consistency

No need for direct mapping.

### 207L DoD

Reputation reflects what outsiders believe about morally charged actions while MoralChoice remains the sole authority for the ethical meaning of what actually happened.

---

# TASK 207M — Reputation Decay, Persistence of Legends & Contradictory Evidence

# 207M.0 Goal

Make public perception evolve over time without implausible universal amnesia.

## 207M.1 Evidence decay profiles

Data-driven.

Example categories:
- routine trade;
- minor aid;
- local dispute;
- major raid;
- treaty betrayal;
- humanitarian crisis;
- atrocity;
- legendary defense.

## 207M.2 Routine evidence

Can decay quickly.

## 207M.3 Major evidence

Slow decay.

## 207M.4 Legendary event

May have:
- permanent notoriety floor;
- long-lived tag salience.

## 207M.5 No universal overall score drift to 50

## 207M.6 Tag projection

Recompute from:
- active evidence;
- memory floors;
- contradiction.

## 207M.7 Audience decay

Different audiences may forget differently if information model supports.

## 207M.8 No-interaction faction perception

Public evidence can fade.

Faction trust decay remains FactionStanceEngine.

## 207M.9 Contradiction

Example:
- strong raid victory;
- later humiliating defeat.

Both evidence points matter.

## 207M.10 Recency

Recent event can dominate.

## 207M.11 Reliability

High-confidence evidence weighs more.

## 207M.12 Propaganda repetition

Diminishing.

## 207M.13 Correction

Proven false rumor reduces old evidence credibility.

## 207M.14 Notoriety decay

Slow.

High notoriety may maintain floor after iconic events.

## 207M.15 Negative/positive asymmetry

Do not assume mathematically symmetric memory.

Could be profile-specific.

## 207M.16 Decay processing

Daily boundary or lazy formula.

No per-frame.

## 207M.17 Threshold events

Only when reputation band/tag activation meaningfully changes.

## 207M.18 No daily “reputation faded” journal spam

### 207M DoD

Reputation evolves through evidence salience, contradiction and forgetting rather than a single number mechanically returning to neutral every day.

---

# TASK 207N — Derived Reputation Effects & Adapter Contract

# 207N.0 Goal

Provide one typed, bounded interface for downstream systems.

## 207N.1 Read model

Suggested:

```text
ShelterPublicPerception
  shelter_id
  notoriety_band
  active_tags[]
  public_summary
  audience_context optional
  confidence
  salient_evidence_refs[]
```

## 207N.2 Audience-specific query

```text
GetPerceptionForAudience(audienceId)
```

## 207N.3 Consumer-specific query

Prefer:

```text
GetTradeContext(...)
GetVisitorContext(...)
GetRaidContext(...)
GetDiplomacyContext(...)
GetRecruitmentContext(...)
```

or generic immutable context.

## 207N.4 No consumer writes reputation

Consumers receive read-only projections.

## 207N.5 Bounded effect

Each consumer has:
- max modifier budget.

## 207N.6 Effect stacking

Document.

Example:
- faction trust;
- trade reputation;
- market demand
must not all multiply uncontrolled.

## 207N.7 No hidden double count

One source event might influence:
- trust;
- public reputation.

Consumer must know which dimensions are already included.

## 207N.8 Effect provenance

Diagnostics:
- “Trader offered contract because Reliable 72 + known notoriety.”

## 207N.9 No universal effect table

Each consumer interprets tags.

## 207N.10 Effect data

Could live in:

`reputation_effect_profiles.json`

but belongs to:
- integration adapter policies,
not reputation truth.

## 207N.11 Reachability

Every effect profile has real consumer.

### 207N DoD

Downstream systems consume immutable, bounded reputation context with explicit provenance, preventing a single reputation score from silently controlling unrelated gameplay.

---

# TASK 207O — Reputation History, Archive & Shelter Identity

# 207O.0 Goal

Make the shelter's external story legible without duplicating the event archive.

## 207O.1 Plan 166 shelter identity

Use canonical:
- shelter name;
- emblem;
- identity.

## 207O.2 Reputation display

Example:

```text
Holdfast
Known as: Reliable, Feared, Sanctuary
Notoriety: Regional
```

## 207O.3 History

Use evidence summaries:
- source event;
- when it became known;
- audiences;
- resulting tag changes.

## 207O.4 Do not copy entire event description

Reference source event/archive.

## 207O.5 Plan 162/archive

Major reputation milestones can become:
- historical entries.

## 207O.6 “The Legend”

Narrative milestone only if:
- notoriety threshold + major evidence.

## 207O.7 “The Fear”

Same.

## 207O.8 “The Respect”

Same.

## 207O.9 “The Betrayal”

Use existing source event if possible.

## 207O.10 “The Sanctuary”

Could be shelter identity milestone.

## 207O.11 “The Atrocity”

Only canonical moral/event source.

## 207O.12 “The Fade”

Do not create every time score decays.

Could be one narrative event when a once-famous shelter becomes obscure.

## 207O.13 “The Notoriety”

Milestone.

## 207O.14 Bounded operational history

Archive major;
keep recent evidence compact.

### 207O DoD

The shelter's name and externally remembered deeds become a coherent historical identity while the canonical event/archive system remains the source of historical truth.

---

# TASK 207P — Reputation UI & External-Perception Map

# 207P.0 Goal

Show what the world believes, why, and how certain those beliefs are without pretending the player has omniscient access to every audience.

## 207P.1 Reputation panel

Show:
- shelter name;
- notoriety;
- active public tags;
- public summary;
- recent evidence.

## 207P.2 No primary giant “overall reputation 73”

If retained:
- derived summary only.

## 207P.3 Tag detail

Show:
- magnitude band;
- recent sources;
- persistence;
- known audiences.

## 207P.4 Exact 0–100

Optional advanced tooltip/debug.

## 207P.5 Notoriety band

Suggested:
- unknown;
- local;
- regional;
- widespread;
- legendary/notorious.

## 207P.6 Audience map

Source proposes:
- how factions view shelter.

Avoid exposing unknown internal faction beliefs.

Show:
- known perception/intel.

## 207P.7 Faction perception

Could display:
- “Faction X has heard you are Reliable and Strong”
only if player knows through diplomacy/intel.

## 207P.8 Unknown

Show:
- “Perception unknown.”

## 207P.9 Evidence log

Filter:
- combat;
- humanitarian;
- trade;
- diplomacy;
- propaganda.

## 207P.10 Effect panel

Show active downstream influence examples:

```text
Traders from Region A know your Reliable reputation.
Raiders in Region B have heard of your defenses.
```

Do not promise exact outcome.

## 207P.11 Cause-effect

Click tag:
- evidence list.

## 207P.12 Contradiction display

Example:
- strong 65;
- recent defeat challenges this perception.

## 207P.13 Propaganda marker

Distinguish:
- organic report;
- shelter propaganda;
- enemy propaganda;
- confirmed event.

## 207P.14 Tutorial

First externally propagated reputation event.

Not merely first private MoralChoice event.

## 207P.15 Tooltips

Not hover-only.

## 207P.16 Accessibility

- no color-only positive/negative;
- keyboard/controller;
- text scale;
- screen-reader order;
- map list alternative.

## 207P.17 No moralizing icons

Avoid:
- green good / red evil
as universal reputation judgment.

### 207P DoD

Players can understand how their shelter is perceived, where those beliefs came from, and which audiences know them without mistaking reputation for morality or omniscient truth.

---

# TASK 207Q — Persistence, Migration & Restore

# 207Q.0 Goal

Persist perception evidence and awareness without retroactively rewriting old campaigns.

## 207Q.1 Persist

Suggested:

```text
schema_version
feature_activation_day
reputation_evidence[]
audience_awareness[]
notoriety_anchor
processed_source_event_ids[]
processed_propagation_ids[]
active_tag_projection_cache optional
history_compaction_state
```

## 207Q.2 Do not persist duplicate

- faction trust;
- morality;
- market price;
- raid frequency;
- visitor quality;
- recruitment chance;
- diplomacy state;
- propaganda state;
- rumor item body;
- shelter identity.

## 207Q.3 Projection cache

Optional.

Must be rebuildable from evidence.

## 207Q.4 Old save

Initialize:
- no evidence;
- notoriety = 0/unknown;
- neutral/no active public tags.

## 207Q.5 Source plan says neutral reputation, zero notoriety

Accept as migration parity.

But do not create an artificial `overallReputation = 50` if overall score is removed.

## 207Q.6 No retroactive history mining

Do not scan:
- all old moral choices;
- all old raids;
- all old trades
to synthesize reputation.

## 207Q.7 Future event only

Feature activates now.

## 207Q.8 Existing faction trust

Preserved.
Not copied into public reputation.

## 207Q.9 Restore order

After:
- event IDs;
- shelter identity;
- factions;
- information/rumor state;
- propaganda
or use two-phase reconciliation.

## 207Q.10 Missing source event

Retain compact evidence facts if schema supports.

## 207Q.11 Missing faction/audience

Drop or archive audience-specific projection safely.

## 207Q.12 Definition changes

Stable tag/event IDs.

## 207Q.13 Replay protection

Restore cannot reapply:
- faction trust;
- trade modifier;
- visitor effects;
- quest events.

Consumers query state; no replay mutation.

## 207Q.14 History compaction

Evidence can compact when:
- old;
- low salience;
- not needed for contradiction.

Keep major milestone refs.

### 207Q DoD

Reputation saves only perception-specific evidence/awareness, starts cleanly on old campaigns, and restores without replaying downstream effects or copying faction/market/moral state.

---

# TASK 207R — Determinism & Anti-Farm

# 207R.0 Goal

Prevent reload rerolls, repetitive action farming and duplicate propagation effects.

## 207R.1 Reputation evidence deterministic

A known semantic event maps to:
- same contribution rules.

## 207R.2 Information propagation

If stochastic:
- Plan 131 owns seed/outcome.

## 207R.3 Reputation system uses delivered result

No second propagation roll.

## 207R.4 Stable evidence ID

## 207R.5 Same source report

Processed once per audience path.

## 207R.6 Repeated fair trades

Diminishing returns.

## 207R.7 Trade farming

A trivial buy/sell loop cannot produce infinite Reliable.

## 207R.8 Refugee farming

Cannot:
- accept;
- eject;
- reaccept
same group for repeated Generous/Sanctuary.

## 207R.9 Raid farming

Player cannot intentionally trigger weak raids for infinite Feared.

Use:
- event significance;
- diminishing repeated identical source.

## 207R.10 Propaganda spam

Saturation/cooldown.

## 207R.11 Promise/treaty

One agreement fulfillment:
- one evidence event.

## 207R.12 Secret/public toggle exploit

Publicity state committed by source propagation.

## 207R.13 Correction

Does not create full duplicate notoriety reward.

## 207R.14 Quest/achievement farming

Raw event counts reviewed.

## 207R.15 Stable iteration

Sort evidence IDs.

## 207R.16 No unseeded RNG

## 207R.17 No GUID

## 207R.18 No wall clock

### 207R DoD

Reputation evolves from unique, significant information events and cannot be farmed by repeating low-stakes transactions, reloads, propaganda spam or reversible policy toggles.

---

# TASK 207S — Performance & Evidence Compaction

# 207S.0 Goal

Keep reputation cheap over long campaigns.

## 207S.1 No per-frame recompute

## 207S.2 New evidence

Recompute affected:
- tags;
- audiences;
- notoriety.

## 207S.3 Daily decay

Use:
- lazy decay from last evaluation;
or
- active evidence index.

## 207S.4 O(active evidence) acceptable if bounded.

## 207S.5 Evidence compaction

Old low-impact evidence:
- aggregate by tag/audience/period.

## 207S.6 Preserve provenance

Major milestone refs retained.

## 207S.7 Contradiction evidence

Do not compact away required dispute.

## 207S.8 Audience index

By:
- faction;
- region;
- actor class.

## 207S.9 Consumer query

O(tags/evidence summary), not scan entire history.

## 207S.10 Large history benchmark

Synthetic:
- 1k;
- 10k;
- 100k raw event equivalents
with compaction.

## 207S.11 Map UI

Load audience detail on demand.

## 207S.12 State-size budget

Set explicit.

### 207S DoD

Reputation costs scale with active evidence and audience summaries rather than total campaign-event count.

---

# TASK 207T — Long-Horizon Balance Simulations

# 207T.0 Goal

Prove reputation becomes meaningful without dominating every external interaction.

## 207T.1 30-day unknown shelter

Low publicity.

Expected:
- low notoriety;
- few reputation effects.

## 207T.2 30-day active trader

Repeated fair trade.

Expected:
- some Reliable;
- diminishing return;
- not instant legendary status.

## 207T.3 120-day sanctuary campaign

Aid/refugees/medical help.

Track:
- Sanctuary;
- Generous;
- notoriety;
- visitor composition.

## 207T.4 120-day militant campaign

Raid defense/attacks.

Track:
- Feared;
- Strong;
- Dangerous;
- raid targeting.

## 207T.5 180-day contradictory campaign

Early:
- generous/reliable.
Later:
- betrayal/cruel act.

Verify:
- mixed reputation;
- no instant overwrite.

## 207T.6 180-day propaganda campaign

Organic vs propaganda evidence.

## 207T.7 400-day quiet-after-fame soak

Observe:
- routine evidence fades;
- major legend persists;
- notoriety declines gradually.

## 207T.8 Secret atrocity

Morality changes.
Reputation unchanged until exposure.

## 207T.9 False accusation

Reputation changes.
Moral truth unchanged.

## 207T.10 Correction

Perception recovers partially.

## 207T.11 Faction divergence

Same public evidence:
- faction A positive;
- faction B negative.

## 207T.12 Unknown faction

No reputation effect until awareness.

## 207T.13 Trader context

Reliable affects terms only within bounded budget.

## 207T.14 Raid context

Strong/feared does not eliminate raids.

## 207T.15 Recruitment context

Sanctuary/high notoriety does not guarantee recruit success.

## 207T.16 Diplomacy

Treacherous limits willingness but does not erase all options automatically.

## 207T.17 Old save

No sudden reputation.

## 207T.18 High notoriety

More known, not automatically more liked.

---

# TASK 207U — Testing & CI

# 207U.0 Goal

Make reputation causality, information reach and authority continuously verifiable.

## 207U.1 Data integrity

Validate:
- event type IDs;
- source event bindings;
- tag IDs;
- audience policies;
- decay profiles;
- consumer adapters;
- faction refs;
- localization.

## 207U.2 Selftest

Create:

```text
--shelter-reputation-selftest
```

## 207U.3 Selftest scenarios

At least:

1. private event → no reputation;
2. witnessed event → local evidence;
3. rumor propagated → remote audience evidence;
4. humanitarian event;
5. atrocity exposed;
6. raid defended;
7. raid lost;
8. fair trade diminishing return;
9. treaty honored;
10. treaty betrayed;
11. propaganda claim;
12. propaganda exposed;
13. false accusation;
14. correction;
15. notoriety growth;
16. notoriety decay;
17. contradictory tags;
18. faction trust remains separate;
19. trade adapter;
20. raid adapter;
21. visitor adapter;
22. recruitment adapter;
23. diplomacy adapter;
24. old save;
25. save/load;
26. headless.

## 207U.4 Source-scan authority gate

Detect:
- `reputationByFaction` duplicate trust;
- persisted `trustModifier`;
- direct Market price mutation;
- direct raid frequency mutation;
- direct recruitment success mutation;
- direct diplomacy state mutation;
- direct morality mutation;
- propaganda duplicate state;
- per-frame decay;
- unseeded RNG.

## 207U.5 Content acceptance

Reputation event ladder:

```text
DISCOVERED
LOADED
REGISTERED
SOURCE_PRODUCER_FOUND
PUBLICITY_PATH_FOUND
AUDIENCE_REACHED
EVIDENCE_CREATED
TAG/NOTORIETY_PROJECTED
DOWNSTREAM_CONSUMER_OBSERVED
```

## 207U.6 Dead-event gate

No event definition without source producer.

## 207U.7 Dead-tag gate

No tag without:
- evidence producer;
- presentation/consumer meaning.

## 207U.8 Dead-effect gate

No effect profile without consumer.

## 207U.9 Golden reputation fixtures

Fixed evidence set:
- exact tags/notoriety.

## 207U.10 Determinism fingerprint

Same evidence:
- same projection.

## 207U.11 No-double-faction-effect gate

## 207U.12 No-secret-event-reputation gate

## 207U.13 Anti-farm gate

## 207U.14 Long-horizon state-size gate

## 207U.15 Performance benchmark

## 207U.16 Generated docs

Create:
- `SHELTER_REPUTATION_ARCHITECTURE.md`;
- `SHELTER_REPUTATION_AUTHORITY_MATRIX.md`;
- `REPUTATION_EVENT_MATRIX.md`;
- `REPUTATION_TAG_MATRIX.md`;
- `REPUTATION_AUDIENCE_MATRIX.md`;
- `REPUTATION_DECAY_MATRIX.md`;
- `REPUTATION_CONSUMER_MATRIX.md`;
- `REPUTATION_MIGRATION_MATRIX.md`;
- `REPUTATION_BALANCE_REPORT.md`;
- `ADR_PUBLIC_REPUTATION_VS_FACTION_TRUST.md`;
- `ADR_REPUTATION_VS_MORALITY.md`;
- `ADR_REPUTATION_AND_INFORMATION_PROPAGATION.md`;
- `ADR_REPUTATION_EVIDENCE_DECAY.md`.

### 207U DoD

Every reputation change can be traced from a real source event through a real publicity path to a real audience and then into bounded downstream behavior.

---

# TASK 207V — Narrative Milestones & Quest Hooks

# 207V.0 Goal

Turn reputation into emergent identity without rewarding raw score manipulation.

## 207V.1 Semantic events

Candidate:

```text
shelter_reputation_evidence_received
shelter_reputation_tag_activated
shelter_reputation_tag_changed
shelter_notoriety_band_changed
shelter_reputation_milestone
shelter_reputation_corrected
```

## 207V.2 Avoid daily tag-decay events

## 207V.3 Source narrative event names

- The Legend;
- The Fear;
- The Respect;
- The Betrayal;
- The Sanctuary;
- The Atrocity;
- The Fade;
- The Notoriety.

Treat as authored narrative candidates.

## 207V.4 Quest hooks

Plan 171 owns dynamic quests.

Expose:
- notoriety band;
- tag band;
- faction-aware tag;
- major correction;
- sanctuary known by region;
- treachery scandal.

## 207V.5 Source quest ideas

- reach 90+ notoriety;
- feared by 5 factions;
- respected by 5 factions;
- accept 20 refugees;
- maintain reliable 100 days;
- reach 80 feared;
- low notoriety 200 days.

Treat as backlog.

## 207V.6 Avoid raw “accept 20 refugees”

Can incentivize humanitarian farming.

Prefer:
- establish recognized sanctuary;
- maintain reliable trade network;
- repair reputation after scandal;
- remain deliberately obscure while achieving objective.

## 207V.7 “Feared by 5 factions”

Faction trust/perception distinction must be precise.

## 207V.8 “Unknown”

Can be stealth/low-profile challenge if world systems support.

## 207V.9 Achievement vs quest

Some long-horizon metrics better as achievements/challenges.

### 207V DoD

Narrative content reacts to meaningful shelter identity milestones rather than encouraging repetitive reputation-point farming.

---

# TASK 207W — Advanced Reputation Markets, Legacy & Myth: Explicit Follow-On

# 207W.0 Goal

Prevent the base system from becoming a speculative meta-economy.

## 207W.1 Reputation specialization

Diplomats/propagandists use:
- canonical SkillProgression;
- Plan 168.

No reputation-owned skill.

## 207W.2 Reputation legacy across campaigns

Meta-progression/chronicle follow-on.

Not base save.

## 207W.3 Famous shelter memory

Plan 162/archive.

## 207W.4 Reputation trading

Source proposes:
- trade reputation services.

Reject as literal feature.

Could mean:
- PR/propaganda contract;
- diplomat;
- information broker
through Plan 168/155/131.

## 207W.5 Bounty on reputation

Raid/faction systems.

## 207W.6 Myth/legend distortion

Plan 131/162.

## 207W.7 Regional legends

Follow-on to audience model.

## 207W.8 Shelter succession

If identity persists after leadership change:
- future governance feature.

## 207W.9 Impersonation

Requires identity/propaganda/fraud.

## 207W.10 Reputation laundering

Could be propaganda/diplomacy, not direct score purchase.

## 207W.11 External awards/titles

Faction/diplomacy content.

### 207W DoD

Advanced legacy and reputation manipulation features remain layered over information, propaganda, diplomacy and archive systems instead of turning reputation into a purchasable universal currency.

---

# 5. Core Reputation Flow

```text
CANONICAL ACTION / EVENT
        │
        ▼
IS IT KNOWN OUTSIDE?
        │
        ├── no  ─────► no public reputation effect
        │
        └── yes
              │
              ▼
      INFORMATION PROPAGATION
              │
              ▼
      ReputationEvidence
              │
              ├── audience
              ├── confidence
              ├── salience
              ├── framing
              └── tag contributions
              │
              ▼
      PUBLIC PERCEPTION
              │
              ├── tags
              ├── notoriety
              └── audience context
              │
              ▼
      CANONICAL CONSUMERS
```

---

# 6. Reputation vs Truth Contract

Reputation is:
- belief.

Historical event is:
- truth.

They may differ.

---

# 7. Reputation vs Morality Contract

MoralChoice answers:
- was the action morally significant/how?

Reputation answers:
- what do outsiders think happened/what does it imply?

---

# 8. Reputation vs Faction Trust Contract

Faction trust:
- bilateral relationship.

Reputation:
- public perception evidence known by faction.

---

# 9. Reputation vs Propaganda Contract

Propaganda:
- produces/framing claims.

Reputation:
- receives audience belief evidence.

---

# 10. Reputation vs Information Contract

Plan 131:
- transports information.

Reputation:
- interprets delivered information as public perception.

---

# 11. Notoriety Contract

Notoriety:
- how widely known.

Not:
- positive/negative score.

---

# 12. Tag Contract

Tags represent:
- salient public interpretations.

They can coexist.

---

# 13. Strong/Weak Contract

Only externally visible evidence contributes.

Hidden defense stats do not.

---

# 14. Rich/Poor Contract

Only observed/perceived resources contribute.

Hidden inventory does not.

---

# 15. Reliable/Treacherous Contract

Requires:
- agreement/trade/promise evidence.

---

# 16. Generous/Sanctuary Contract

Requires:
- aid/admission/treatment known externally.

---

# 17. Cruel/Dangerous Contract

Requires:
- externally known conduct interpreted that way.

---

# 18. Feared/Respected Contract

Distinct dimensions.

Feared:
- expected cost/risk.

Respected:
- status/honor/competence.

---

# 19. Visitor Contract

Reputation provides:
- attraction/deterrence context.

Visitor system chooses.

---

# 20. Trade Contract

Reputation provides:
- trust/expectation context.

Market/trade chooses terms.

---

# 21. Raid Contract

Reputation provides:
- perceived cost/reward context.

Raid system decides targeting.

---

# 22. Recruitment Contract

Reputation provides:
- shelter appeal/awareness.

Recruitment decides outcome.

---

# 23. Diplomacy Contract

Reputation provides:
- first-impression/public-standing context.

Diplomacy owns options/state.

---

# 24. Migration Contract

Plan 199 can use:
- notoriety;
- sanctuary;
- dangerous;
- strong
as destination-context inputs.

No direct migration spawn.

---

# 25. Expedition Contract

Reputation can influence:
- encounter disposition
only via faction/NPC systems.

No direct encounter outcome mutation.

---

# 26. History Contract

Evidence history:
- perception provenance.

Archive:
- historical truth.

---

# 27. Decay Contract

Decay means:
- fewer actors remember/believe strongly.

Not:
- event unhappened.

---

# 28. Old-Save Contract

Old campaign starts:
- publicly unknown/neutral.

Existing faction trust remains as-is.

---

# 29. Persistence Matrix

| Fact | Owner |
|---|---|
| shelter identity/name | Plan 166 |
| moral truth | MoralChoice |
| faction trust | FactionStanceEngine |
| rumor propagation | Plan 131 |
| propaganda | Plan 168 |
| market price | MarketSystem |
| trade session | HoldfastTradeSession |
| visitor generation | visitor/Airlock |
| raid schedule | raid authority |
| recruitment | RecruitmentSystem |
| diplomacy | Plan 197 |
| reputation evidence | ShelterReputation |
| public tags | ShelterReputation |
| notoriety | ShelterReputation |
| audience perception | ShelterReputation |
| archive history | Plan 162/event archive |

---

# 30. Old-Save Migration Matrix

```text
feature_activation_day = current campaign day
reputation_evidence = []
public_tags = none
notoriety = 0 / unknown
audience_awareness = none
existing faction trust preserved
```

No retroactive synthesis.

---

# 31. Exactly-Once Identity

Stable:

```text
rep-evidence:<source>:<audience>:<propagation>
rep-tag-transition:<tag>:<audience>:<band>:<sequence>
notoriety-transition:<band>:<sequence>
consumer-context:<query-only>
```

Consumers do not persist side-effect events from reputation queries.

---

# 32. Failure Injection Matrix

## N207.1 Secret atrocity instantly makes shelter Cruel globally
Expected: information-reach gate fails.

## N207.2 Reputation state stores faction trust dictionary
Expected: faction-authority gate fails.

## N207.3 Trade price directly multiplied by reputation system
Expected: market-authority gate fails.

## N207.4 Feared tag directly edits raid frequency
Expected: raid-authority gate fails.

## N207.5 Hidden stockpile automatically creates Rich tag
Expected: external-knowledge gate fails.

## N207.6 Refugee rejection always creates Cruel regardless context
Expected: morality/perception semantics gate fails.

## N207.7 Same trade loop gives infinite Reliable
Expected: anti-farm gate fails.

## N207.8 Same treaty event changes faction trust directly and again through full reputation delta
Expected: double-count gate fails.

## N207.9 Old save gets legendary reputation from historical raids
Expected: migration parity fails.

## N207.10 Propaganda broadcast directly changes global reputation without reach
Expected: Plan-131 integration gate fails.

## N207.11 Notoriety presented as approval
Expected: UI semantic gate fails.

## N207.12 Reputation decays every frame
Expected: performance gate fails.

---

# 33. Determinism Contract

Same:

```text
reputation evidence
+ audience reach
+ confidence
+ salience
+ tag/decay policies
+ campaign time
```

must yield same:
- active tags;
- tag magnitudes;
- notoriety;
- audience perception;
- consumer context.

No RNG is required unless a source information system already uses seeded uncertainty.

---

# 34. Long-Horizon Metrics

Track:

```text
source reputation events
publicized events
private events ignored
evidence by audience
active tags
tag magnitudes
notoriety
awareness bands
contradictory evidence
corrections
consumer queries
trade influence
visitor influence
raid influence
recruitment influence
diplomacy influence
evidence compaction
state bytes
daily processing time
```

---

# 35. Balance Guardrails

Reputation should:
- make behavior legible;
- shape opportunities;
- create strategic identity.

It should not:
- replace faction trust;
- replace morality;
- dictate every external encounter.

---

# 36. Notoriety Guardrails

High notoriety should create:
- more awareness.

It can create both:
- opportunities;
- threats.

---

# 37. Tag Guardrails

No tag should be:
- permanent from one trivial event;
- globally omniscient.

---

# 38. Trade Guardrails

Reliable reputation should not become:
- permanent universal discount.

---

# 39. Raid Guardrails

Feared reputation should not become:
- raid immunity.

---

# 40. Recruitment Guardrails

Sanctuary reputation should not guarantee:
- skilled recruits.

---

# 41. Diplomacy Guardrails

Treacherous reputation should constrain trust-building.

It should not delete every diplomacy path.

---

# 42. Propaganda Guardrails

Propaganda can change perception.

It cannot make truth disappear.

---

# 43. UI Acceptance

## Summary
- shelter identity;
- notoriety;
- active tags.

## Evidence
- source;
- reach;
- confidence.

## Audience
- known perception only.

## Effects
- bounded hints, not guaranteed outcomes.

---

# 44. Accessibility

- no good/bad color-only encoding;
- text labels;
- keyboard/controller;
- map list alternative;
- text scaling;
- screen-reader evidence order.

---

# 45. Localization

Tag/event UI:
- localization keys.

No localized description as state.

---

# 46. Content Acceptance

Reputation event ladder:

```text
DISCOVERED
LOADED
REGISTERED
SOURCE_BOUND
PUBLICITY_BOUND
AUDIENCE_REACHED
EVIDENCE_CREATED
PERCEPTION_UPDATED
CONSUMER_REACHED
```

---

# 47. Reachability

Every shipped reputation-event definition:
- real source fixture;
- public/private fixture;
- audience fixture;
- consumer or presentation value.

---

# 48. Performance Guardrails

- event-driven evidence;
- lazy/day-boundary decay;
- audience indexes;
- compact history;
- no per-frame scan;
- no full event-log rescan.

---

# 49. CI / Gate Set

Recommended:

```text
shelter_reputation_authority_matrix
shelter_reputation_information_reach
shelter_reputation_no_duplicate_faction_trust
shelter_reputation_no_duplicate_morality
shelter_reputation_propaganda_authority
shelter_reputation_trade_authority
shelter_reputation_raid_authority
shelter_reputation_visitor_authority
shelter_reputation_recruitment_authority
shelter_reputation_diplomacy_authority
shelter_reputation_secret_event_guard
shelter_reputation_double_count_guard
shelter_reputation_antifarm
shelter_reputation_old_save
shelter_reputation_determinism
shelter_reputation_long_horizon
shelter_reputation_ui_access
```

---

# 50. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --shelter-reputation-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 51. Recommended Commit Breakdown

```text
207A-1 authority/information audit
207A-2 reputation-vs-faction-trust ADR
207A-3 reputation-vs-morality ADR
207A-4 reputation-vs-information ADR
207A-5 decay ADR
207A-6 evidence DTO
207A-7 event definition schema/loader
207A-8 data integrity/docs

207B-1 tag registry
207B-2 tag semantics
207B-3 compatible/contradictory tags
207B-4 evidence projection
207B-5 derived summary
207B-6 no-overall-score decision
207B-7 tests/docs

207C-1 notoriety state
207C-2 awareness bands
207C-3 audience awareness
207C-4 regionality
207C-5 notoriety decay
207C-6 source trace
207C-7 tests/docs

207D-1 Plan-131 adapter
207D-2 witness path
207D-3 trader/refugee/raider path
207D-4 radio/faction path
207D-5 secret-event suppression
207D-6 false attribution/correction
207D-7 no-duplicate-rumor storage
207D-8 tests/docs

207E-1 Plan-168 adapter
207E-2 framed evidence
207E-3 detected propaganda
207E-4 audience-specific exposure
207E-5 saturation/diminishing returns
207E-6 counter-propaganda
207E-7 tests/docs

207F-1 faction perception read model
207F-2 FactionStance adapter
207F-3 doctrine interpretation
207F-4 direct-vs-public effect separation
207F-5 first-impression integration
207F-6 no trustModifier persistence
207F-7 double-count tests/docs

207G-1 visitor context DTO
207G-2 Airlock/visitor adapter
207G-3 sanctuary/refugee attraction
207G-4 rich/strong/dangerous visitor weights
207G-5 information-reach gating
207G-6 tests/docs

207H-1 trade context DTO
207H-2 HoldfastTradeSession adapter
207H-3 MarketSystem boundaries
207H-4 reliability/treachery effects
207H-5 rich/poor/feared semantics
207H-6 diminishing trade evidence
207H-7 anti-farm tests/docs

207I-1 raid context DTO
207I-2 raid authority adapter
207I-3 cost/reward interpretation
207I-4 attacker-survivor report path
207I-5 no hidden-defense leak
207I-6 no raid-frequency ownership
207I-7 tests/docs

207J-1 recruitment context
207J-2 Plan-204 adapter
207J-3 Plan-199 migration destination context
207J-4 refugee testimony
207J-5 circularity caps
207J-6 anti-refugee-farm
207J-7 tests/docs

207K-1 Plan-197 diplomacy adapter
207K-2 treaty reliability/treachery
207K-3 faction doctrine interpretation
207K-4 defection/tribute gate
207K-5 ally-request hook
207K-6 double-count tests/docs

207L-1 MoralChoice adapter
207L-2 humanitarian/atrocity evidence
207L-3 secret-event behavior
207L-4 false accusation
207L-5 redemption/perception correction
207L-6 tests/docs

207M-1 decay profiles
207M-2 sticky major events
207M-3 contradiction projection
207M-4 correction
207M-5 notoriety decay
207M-6 lazy/day-boundary processing
207M-7 tests/docs

207N-1 public perception read model
207N-2 audience query
207N-3 consumer contexts
207N-4 effect budgets
207N-5 stacking rules
207N-6 effect provenance
207N-7 reachability/docs

207O-1 Plan-166 identity integration
207O-2 evidence history
207O-3 Plan-162 archive bridge
207O-4 reputation milestones
207O-5 history compaction
207O-6 docs/tests

207P-1 reputation panel
207P-2 tag detail
207P-3 notoriety bands
207P-4 known-audience map
207P-5 evidence/effect panels
207P-6 propaganda/correction UI
207P-7 accessibility/localization
207P-8 snapshots/tutorial

207Q-1 save schema
207Q-2 old-save clean activation
207Q-3 restore ordering
207Q-4 missing source/audience handling
207Q-5 definition migration
207Q-6 no-side-effect replay
207Q-7 compaction
207Q-8 migration docs/tests

207R-1 evidence idempotence
207R-2 Plan-131 RNG boundary
207R-3 trade/recruit/refugee anti-farm
207R-4 propaganda saturation
207R-5 raid-significance anti-farm
207R-6 stable ordering/IDs
207R-7 exploit tests

207S-1 lazy decay
207S-2 audience indexes
207S-3 evidence compaction
207S-4 1k/10k/100k event-equivalent benchmark
207S-5 query performance
207S-6 state-size gate

207T-1 30-day unknown
207T-2 30-day trader
207T-3 120-day sanctuary
207T-4 120-day militant
207T-5 180-day contradictory
207T-6 180-day propaganda
207T-7 400-day quiet-after-fame
207T-8 balance report

207U-1 selftest
207U-2 source-scan authority gates
207U-3 content acceptance
207U-4 failure fixtures
207U-5 deterministic goldens
207U-6 long-horizon/perf gates
207U-7 final ship/no-ship report

207V-1 semantic reputation events
207V-2 Plan-171 hook surface
207V-3 narrative milestone policy
207V-4 anti-grind quest review

207W-1 legacy/myth/reputation-service follow-on disposition
```

---

# 52. Risk Register

## R207.1 Reputation duplicates faction trust

Mitigation:
- public perception vs bilateral trust ADR;
- no per-faction trust score.

## R207.2 Reputation becomes morality score

Mitigation:
- perception-vs-truth separation.

## R207.3 Omniscient global reactions

Mitigation:
- Plan 131 audience reach requirement.

## R207.4 Reputation over-controls economy/raids

Mitigation:
- bounded consumer contexts.

## R207.5 Propaganda directly rewrites truth

Mitigation:
- framed evidence model.

## R207.6 Single scalar loses nuance

Mitigation:
- multidimensional tags + notoriety.

## R207.7 Reputation farming

Mitigation:
- significance;
- diminishing returns;
- evidence IDs.

## R207.8 Old saves suddenly become famous

Mitigation:
- clean activation baseline.

## R207.9 Evidence history grows forever

Mitigation:
- compaction;
- major milestone retention.

## R207.10 Downstream effects double count original events

Mitigation:
- direct-vs-public effect matrix and exactly-once gates.

---

# 53. Acceptance Checklist

## P0

- [ ] FactionStanceEngine audited
- [ ] Plan 131 audited
- [ ] Plan 168 audited
- [ ] MoralChoiceSystem audited
- [ ] AirlockSecuritySystem audited
- [ ] visitor generation audited
- [ ] HoldfastTradeSession audited
- [ ] MarketSystem audited
- [ ] raid/threat authority audited
- [ ] shelter defense outcome events audited
- [ ] RecruitmentSystem / Plan 204 audited
- [ ] FactionDiplomacy / Plan 197 audited
- [ ] Plan 166 shelter identity audited
- [ ] Plan 138 reputation references audited
- [ ] Plan 199 migration/trader/refugee audited
- [ ] Expedition encounter integration audited
- [ ] archive/history audited
- [ ] semantic event bus audited
- [ ] campaign clock audited
- [ ] save order audited
- [ ] UI surfaces audited
- [ ] reputation authority matrix published
- [ ] public reputation vs faction trust ADR
- [ ] reputation vs morality ADR
- [ ] reputation vs information ADR
- [ ] evidence decay ADR
- [ ] baseline captured

## 207A — Evidence

- [ ] versioned event definitions
- [ ] stable event type IDs
- [ ] source event bindings
- [ ] required publicity
- [ ] default tag contributions
- [ ] notoriety weight
- [ ] audience policy
- [ ] salience profile
- [ ] decay profile
- [ ] confidence policy
- [ ] runtime evidence DTO
- [ ] stable evidence ID
- [ ] canonical source event
- [ ] publicity bands
- [ ] private produces no public effect
- [ ] witnessed/local/regional semantics
- [ ] confidence
- [ ] salience
- [ ] tag contributions not direct final score
- [ ] framing refs
- [ ] contradiction group
- [ ] no localized truth strings
- [ ] history bounded
- [ ] unsupported sources fail

## 207B — Tags

- [ ] Feared semantics
- [ ] Respected semantics
- [ ] Generous semantics
- [ ] Cruel semantics
- [ ] Reliable semantics
- [ ] Treacherous semantics
- [ ] Strong semantics
- [ ] Weak semantics
- [ ] Rich semantics
- [ ] Poor semantics
- [ ] Sanctuary semantics
- [ ] Dangerous semantics
- [ ] each tag has producer
- [ ] each tag has consumer/presentation use
- [ ] compatible tags allowed
- [ ] contradictory evidence retained
- [ ] tag magnitude bounded
- [ ] confidence/salience semantics
- [ ] no primary universal approval score
- [ ] derived summary only
- [ ] no moral alignment label

## 207C — Notoriety

- [ ] notoriety = awareness
- [ ] notoriety != approval
- [ ] bounded scale
- [ ] source events require publicity
- [ ] hidden discovery gives no fame
- [ ] audience awareness
- [ ] global/regional design explicit
- [ ] high notoriety expands awareness
- [ ] low notoriety limits knowledge
- [ ] decay policy
- [ ] major fame floor if intended
- [ ] spatial propagation reused
- [ ] source trace
- [ ] no daily log

## 207D — Plan 131

- [ ] Plan 131 owns propagation
- [ ] source event → info → audience → evidence
- [ ] direct witness path
- [ ] trader path
- [ ] refugee path
- [ ] raider survivor path
- [ ] radio path
- [ ] faction-report path
- [ ] secret actions excluded
- [ ] unknown perpetrator excluded
- [ ] misattribution if supported
- [ ] contradictions supported
- [ ] reliability imported
- [ ] correction supported
- [ ] forgetting/reduced salience integration
- [ ] no rumor archive duplicate

## 207E — Propaganda

- [ ] Plan 168 remains authority
- [ ] framed evidence
- [ ] positive campaign
- [ ] enemy campaign
- [ ] detected deception requires attribution
- [ ] audience-scoped penalty
- [ ] truth vs claim
- [ ] contradictory evidence persists
- [ ] diminishing repetition
- [ ] saturation
- [ ] counter-propaganda
- [ ] exposure event
- [ ] propagation reach through Plan 131/radio
- [ ] no reputation-owned propaganda skill

## 207F — Factions

- [ ] no reputationByFaction trust duplicate
- [ ] faction audience perception only
- [ ] FactionStance remains trust owner
- [ ] typed public-perception input
- [ ] no persisted trustModifier
- [ ] faction doctrine interpretation
- [ ] same evidence different audiences
- [ ] private direct interaction stays trust-only
- [ ] public first impression
- [ ] no trust decay in reputation
- [ ] direct/public effect double-count prevented
- [ ] kill/aid/treaty examples tested

## 207G — Visitors

- [ ] visitor authority preserved
- [ ] visitor reputation context
- [ ] notoriety affects candidate awareness only
- [ ] Sanctuary attraction
- [ ] Rich attraction semantics
- [ ] Strong/Feared deterrence semantics
- [ ] Dangerous/Cruel contextual effects
- [ ] no universal high-rep skilled visitor rule
- [ ] recruitment remains separate
- [ ] reputation only if origin knows
- [ ] no information reach = no effect

## 207H — Trade

- [ ] MarketSystem owns price
- [ ] HoldfastTradeSession owns session
- [ ] trade context
- [ ] Reliable behavior
- [ ] Treacherous behavior
- [ ] Rich behavior
- [ ] Poor behavior
- [ ] Feared behavior
- [ ] no universal multiplier
- [ ] contract systems integrated if real
- [ ] routine trade diminishing return
- [ ] fraud only if mechanic exists
- [ ] private trade no public rep without witness/report

## 207I — Raids

- [ ] raid authority preserved
- [ ] raid reputation context
- [ ] Feared expected-cost semantics
- [ ] Strong expected-cost semantics
- [ ] Weak vulnerability semantics
- [ ] Rich reward semantics
- [ ] notoriety awareness
- [ ] no simple fewer-raids rule
- [ ] raid-defended evidence requires propagation
- [ ] raid-lost same
- [ ] hidden defenses excluded
- [ ] attacker-survivor report
- [ ] no direct attack-frequency field

## 207J — Recruitment/Migration

- [ ] RecruitmentSystem owns outcome
- [ ] reputation recruitment context
- [ ] Sanctuary attraction
- [ ] Strong contextual attraction
- [ ] Cruel/Dangerous contextual repulsion/attraction
- [ ] notoriety expands awareness
- [ ] no direct success percentage
- [ ] Plan 199 testimony
- [ ] destination attractiveness adapter
- [ ] circularity guard
- [ ] rejection context-sensitive
- [ ] acceptance context-sensitive
- [ ] anti-farm

## 207K — Diplomacy

- [ ] Plan 197 owns diplomacy
- [ ] reputation input
- [ ] Reliable/Respected
- [ ] Treacherous
- [ ] Feared
- [ ] faction doctrine interpretation
- [ ] defection deferred unless real
- [ ] tribute deferred unless real
- [ ] ally-request hook
- [ ] treaty public/direct double-count prevented
- [ ] no direct treaty state mutation

## 207L — Morality

- [ ] MoralChoice owns ethics
- [ ] reputation consumes publicized moral event
- [ ] humanitarian evidence
- [ ] atrocity evidence only if canonical
- [ ] refugee rejection context-sensitive
- [ ] prisoner execution only if system real
- [ ] aid requires actual aid
- [ ] secret atrocity public reputation unchanged
- [ ] false accusation perception-only
- [ ] later redemption changes perception not history
- [ ] no direct morality mapping

## 207M — Decay/Contradiction

- [ ] data-driven evidence decay
- [ ] routine evidence faster decay
- [ ] major evidence slow
- [ ] iconic event floor if intended
- [ ] no universal drift to 50
- [ ] tag projection from evidence
- [ ] audience-specific decay if supported
- [ ] faction trust decay remains separate
- [ ] contradictory evidence retained
- [ ] recency
- [ ] confidence
- [ ] propaganda diminishing
- [ ] correction
- [ ] notoriety decay
- [ ] asymmetry policy explicit
- [ ] no per-frame
- [ ] threshold events only
- [ ] no daily Fade spam

## 207N — Consumer Context

- [ ] immutable public perception read model
- [ ] audience-specific query
- [ ] trade context
- [ ] visitor context
- [ ] raid context
- [ ] diplomacy context
- [ ] recruitment context
- [ ] consumers read-only
- [ ] effect budgets
- [ ] stacking documented
- [ ] double-count diagnostics
- [ ] provenance
- [ ] no universal effect table
- [ ] effect profiles have consumers

## 207O — History/Identity

- [ ] Plan 166 shelter identity reused
- [ ] reputation display uses canonical name
- [ ] evidence history
- [ ] no copied entire event text
- [ ] Plan 162 archive bridge
- [ ] Legend milestone
- [ ] Fear milestone
- [ ] Respect milestone
- [ ] Betrayal source dedupe
- [ ] Sanctuary milestone
- [ ] Atrocity source gate
- [ ] Fade rare narrative only
- [ ] Notoriety milestone
- [ ] operational history bounded

## 207P — UI

- [ ] shelter name
- [ ] notoriety
- [ ] active tags
- [ ] public summary
- [ ] recent evidence
- [ ] no giant primary overall score
- [ ] tag detail
- [ ] exact scores optional
- [ ] notoriety bands
- [ ] known-audience map only
- [ ] unknown faction perceptions hidden
- [ ] evidence filters
- [ ] effect panel uses hints
- [ ] cause-effect drilldown
- [ ] contradiction display
- [ ] propaganda source markers
- [ ] tutorial on first propagated event
- [ ] no hover-only
- [ ] no color-only moral semantics
- [ ] keyboard/controller
- [ ] text scale
- [ ] screen reader
- [ ] map list alternative

## 207Q — Persistence

- [ ] schema version
- [ ] feature activation day
- [ ] evidence
- [ ] audience awareness
- [ ] notoriety anchor
- [ ] processed source IDs
- [ ] processed propagation IDs
- [ ] optional rebuildable projection cache
- [ ] no duplicate faction trust
- [ ] no duplicate morality
- [ ] no market state
- [ ] no raid state
- [ ] no visitor state
- [ ] no recruitment state
- [ ] no diplomacy state
- [ ] no propaganda state
- [ ] no rumor body duplicate
- [ ] no shelter identity duplicate
- [ ] old save zero notoriety
- [ ] old save no active tags
- [ ] no synthetic overall 50 requirement
- [ ] no retroactive history mining
- [ ] future event only
- [ ] existing faction trust preserved
- [ ] restore ordering
- [ ] missing source safe
- [ ] missing audience safe
- [ ] stable IDs across definition changes
- [ ] no side-effect replay
- [ ] history compaction

## 207R — Determinism/Anti-Farm

- [ ] deterministic evidence mapping
- [ ] Plan 131 owns propagation RNG
- [ ] no second propagation roll
- [ ] stable evidence ID
- [ ] same report once per audience path
- [ ] fair-trade diminishing returns
- [ ] no trade loop farm
- [ ] no refugee toggle farm
- [ ] no weak-raid fear farm
- [ ] propaganda saturation
- [ ] agreement evidence once
- [ ] publicity committed
- [ ] correction no duplicate notoriety
- [ ] quest farming reviewed
- [ ] stable iteration
- [ ] no unseeded RNG
- [ ] no GUID
- [ ] no wall clock

## 207S — Performance

- [ ] no per-frame recompute
- [ ] affected-tag recompute only
- [ ] lazy/day-boundary decay
- [ ] active evidence bounded
- [ ] compaction
- [ ] milestone provenance retained
- [ ] contradiction evidence preserved
- [ ] audience indexes
- [ ] consumer query summary-based
- [ ] 1k benchmark
- [ ] 10k benchmark
- [ ] 100k equivalent benchmark
- [ ] map details on demand
- [ ] state size budget

## 207T/U — Simulations/CI

- [ ] 30-day unknown
- [ ] 30-day trader
- [ ] 120-day sanctuary
- [ ] 120-day militant
- [ ] 180-day contradictory
- [ ] 180-day propaganda
- [ ] 400-day quiet-after-fame
- [ ] secret atrocity
- [ ] false accusation
- [ ] correction
- [ ] faction divergence
- [ ] unknown faction no effect
- [ ] trade bounded
- [ ] raid not immune
- [ ] recruitment not guaranteed
- [ ] diplomacy not binary
- [ ] old-save clean
- [ ] notoriety != approval
- [ ] data integrity
- [ ] shelter-reputation selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] dead-event gate
- [ ] dead-tag gate
- [ ] dead-effect gate
- [ ] golden reputation fixtures
- [ ] deterministic fingerprint
- [ ] no-double-faction-effect
- [ ] secret-event guard
- [ ] anti-farm
- [ ] state-size
- [ ] performance
- [ ] generated docs
- [ ] verify-fast

## 207V/W — Narrative/Follow-On

- [ ] bounded semantic reputation events
- [ ] source narrative names treated as content
- [ ] Plan 171 owns quests
- [ ] raw refugee-count quest reviewed
- [ ] raw faction-count reputation quest reviewed
- [ ] contextual identity goals preferred
- [ ] achievement-vs-quest choice explicit
- [ ] diplomats/propagandists use canonical skills
- [ ] legacy across campaigns deferred
- [ ] famous shelter memory uses archive
- [ ] literal reputation trading rejected
- [ ] PR/information services use Plan 168/131/155
- [ ] myth distortion uses Plan 131/162
- [ ] regional legends follow audience model
- [ ] impersonation requires fraud/identity
- [ ] reputation laundering not direct score purchase
- [ ] external titles use diplomacy

---

# 54. Ship / No-Ship Gate

**SHIP** only if:

```text
faction_trust_authorities == 1
AND morality_authorities == 1
AND information_propagation_authorities == 1
AND propaganda_authorities == 1
AND market_price_authorities == 1
AND raid_schedule_authorities == 1
AND visitor_generation_authorities == 1
AND recruitment_authorities == 1
AND diplomacy_authorities == 1
AND duplicate_faction_trust_state_in_reputation == 0
AND persisted_reputation_trust_modifier == false
AND duplicate_morality_state_in_reputation == 0
AND reputation_owned_propaganda_state == false
AND reputation_direct_market_price_mutation == false
AND reputation_direct_raid_frequency_mutation == false
AND reputation_direct_visitor_spawn_mutation == false
AND reputation_direct_recruitment_success_mutation == false
AND reputation_direct_diplomacy_state_mutation == false
AND secret_events_create_public_reputation == false
AND hidden_stockpile_creates_rich_tag == false
AND hidden_defense_stats_create_strong_tag == false
AND notoriety_is_treated_as_approval == false
AND universal_daily_overall_drift_to_neutral == false
AND duplicate_source_evidence_per_audience == 0
AND reputation_action_farm_paths == 0
AND retroactive_old_save_reputation_history == 0
AND per_frame_reputation_processing == 0
AND unseeded_reputation_rng == 0
AND dead_reputation_event_definitions == 0
AND dead_reputation_tags == 0
AND dead_reputation_effect_profiles == 0
AND shelter_reputation_old_save == pass
AND shelter_reputation_save_roundtrip == pass
AND shelter_reputation_information_reach == pass
AND shelter_reputation_faction_separation == pass
AND shelter_reputation_morality_separation == pass
AND shelter_reputation_propaganda_integration == pass
AND shelter_reputation_consumer_budgets == pass
AND shelter_reputation_antifarm == pass
AND shelter_reputation_determinism == pass
AND shelter_reputation_30_day_balance == pass
AND shelter_reputation_120_day_balance == pass
AND shelter_reputation_180_day_balance == pass
AND shelter_reputation_400_day_soak == pass
AND shelter_reputation_performance == pass
AND shelter_reputation_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 55. Implementer Handoff

1. Audit `FactionStanceEngine`, Plan 131, Plan 168, MoralChoice, Market/trade, visitor generation, raid logic, Recruitment, Plan 197 diplomacy and Plan 166 shelter identity before writing reputation code.
2. Treat reputation as **external perception**, not a master truth score.
3. Do not create a second per-faction trust dictionary.
4. Do not persist a `trustModifier`; derive faction public-perception input and let `FactionStanceEngine` interpret it.
5. Require a publicity/information path before any public reputation change.
6. Keep secret actions secret until exposed.
7. Let false accusations affect public perception without altering moral truth.
8. Let propaganda frame evidence without changing what actually happened.
9. Use multiple reputation tags/dimensions rather than one universal good/bad score.
10. Keep notoriety separate from approval.
11. Ensure Strong, Weak, Rich and Poor reflect what outsiders can actually observe or credibly hear.
12. Treat refugee, trade, combat and humanitarian events contextually.
13. Do not hardcode refugee rejection as Cruel.
14. Do not hardcode every raid defense as universally Respected.
15. Preserve contradictory evidence so a shelter can have an ambiguous reputation.
16. Make routine events decay faster than iconic events.
17. Do not use a universal drift-to-50 mechanic.
18. Let Plan 131 own rumor/news delivery, source reliability and propagation RNG.
19. Let Plan 168 own propaganda production and exposure.
20. Let MarketSystem own prices; reputation supplies trade context only.
21. Let raid systems own attack frequency/targeting; reputation supplies perceived cost/reward only.
22. Let visitor systems own arrivals; reputation supplies attraction/deterrence context only.
23. Let RecruitmentSystem own candidate success.
24. Let Plan 197 own treaties, ally requests, tribute and defection.
25. Prevent original events from applying once directly and then again as a full duplicate public-reputation penalty to the same faction.
26. Migrate old saves as externally unknown/neutral with zero notoriety and no fabricated historical evidence.
27. Preserve existing faction trust exactly.
28. Use stable evidence IDs and exactly-once audience processing.
29. Add diminishing returns for repeated fair trades, propaganda broadcasts, refugee decisions and raid outcomes.
30. Compact old low-salience evidence while preserving legendary milestones and contradiction provenance.
31. Build a UI that explains what is known, by whom, from which evidence and with what confidence.
32. Hide faction perceptions the player has no intelligence about.
33. Run unknown, sanctuary, militant, contradictory, propaganda, secret-atrocity, false-accusation and long-quiet simulations.
34. Close only when external actors react differently because they have actually heard believable things about the shelter—not because the engine exposes an omniscient reputation number.

---

# 56. Final Outcome

When this plan is complete, the shelter stops being an anonymous player base and becomes a place with an external identity.

But that identity will not be a simplistic morality bar.

A shelter can become known as reliable because traders have repeatedly seen it honor contracts. It can become feared because surviving raiders report devastating defenses. It can become a sanctuary because refugees and patients carry stories of being protected. It can become treacherous because a broken treaty becomes public. It can become dangerous because neighboring groups have evidence of aggressive raids.

Those reputations can coexist.

A feared shelter may also be respected.
A generous shelter may also be dangerous.
A rich shelter may be distrusted.
A sanctuary may be viewed as weak by one faction and admirable by another.

That ambiguity is the point.

Notoriety adds another dimension. A small shelter can be highly respected by the few people who know it and still be obscure across the wasteland. A notorious stronghold can be known everywhere while inspiring completely different reactions.

Most importantly, none of this is omniscient.

A secret atrocity changes moral truth but not public reputation until someone discovers it.
A heroic act known only to the player creates no fame.
A failed propaganda campaign may convince nobody.
A false accusation may damage reputation even though the shelter did nothing wrong.

The system therefore connects directly to ASHFALL's information ecosystem.

Witnesses matter.
Travelers matter.
Radio matters.
Faction reports matter.
Refugee testimony matters.
Surviving attackers matter.
Propaganda matters.

Reputation grows because information travels.

Downstream systems then consume that perception without surrendering their own authority.

Traders may treat a reliable shelter differently, but MarketSystem still owns the price.
Raiders may fear a strong shelter, but raid logic still decides whether the reward is worth the risk.
Refugees may seek a sanctuary, but migration and recruitment systems still decide who actually arrives and whether they can be admitted.
Diplomats may approach a respected stronghold differently, but FactionDiplomacy still owns treaties.
Faction trust remains bilateral and canonical.

This produces the strategic identity promised by the source plan without creating a giant global score that controls the whole game.

The player can deliberately cultivate a reputation—but can never guarantee exactly how every audience will interpret it.

That makes the shelter feel like a real actor in a world of incomplete information.

Its name travels.

Stories accumulate.

Enemies tell one version.
Refugees tell another.
Traders remember whether agreements were honored.
Factions decide whether the shelter is useful, threatening, honorable or dangerous.

Over time, the player is no longer simply managing a bunker.

They are managing what the wasteland believes that bunker represents.
