# C1 — Flagship Integration Plan [26]: Black Market & Underground Economy

> **Output:** `C1_planintegration[26].md`
>
> **Source baseline:** Plan 155 — Black Market & Underground Economy
>
> **Primary mission:** add an underground trade layer that makes restricted goods, illicit deals, hidden dealers, detection, trust, heat, faction consequences, and moral tradeoffs meaningful without creating a second economy, second reputation system, second law-enforcement simulation, or second quest runtime.
>
> **Primary architectural rule:** the underground market owns **access, concealment, dealer relationships, transaction-risk orchestration, and illicit-market state**. `MarketSystem`, `HoldfastTradeSession`, `InventorySystem`, `FactionStanceEngine`, `MoralChoiceSystem`, quest runtime, bounty/enforcement systems, needs/medical systems, and journal/history remain authoritative for their own facts.
>
> **Primary economic rule:** black-market profitability must arise from scarcity, access, risk, information asymmetry, and transaction friction. It must not become a second price table that permanently beats legal trade.
>
> **Mandatory execution order:** 155A authority audit + contraband/dealer/access contract → 155B transaction, detection, heat and concealment → 155C market/faction/moral/inventory integration → 155D UI, persistence, balance, deterministic hardening and CI → 155E advanced criminal-services/organization features only if existing rails support them.
>
> **Critical re-baseline rule:** before creating `BlackMarketSystem`, inspect the current legal trade pipeline, faction stance/trust, Plan-131 rumor/intelligence, Plan-139 combat→faction consequences, Plan-147 NPC personal memory, current bounty/enforcement mechanics, item legality/ownership metadata if any, market price formation, quest prerequisites, MoralChoice effect channels, dependency/drug consumption, and save orchestration. Reuse those seams instead of duplicating them.
>
> **Guardrails:** no second inventory; no separate illegal currency unless current economy already supports it; no direct hardcoded black-market price engine if `MarketSystem` can consume illicit supply/risk inputs; no new global “criminal reputation” meter without proof current faction/public-awareness systems cannot express the needed state; no assassination or violence services unless existing quest/contract/combat systems already own those actions; no abstract law-enforcement simulator; no heat-driven omniscience; no contraband detection without an information/search/checkpoint source; no unseeded RNG; no wall-clock/GUID dependence; no hidden confiscation; no duplicate moral penalty and faction penalty from one event unless each has its own canonical owner and reason; no direct addiction mutation from transaction code; no new black-market panel if existing trade surfaces can host a hidden/dealer mode cleanly.

---

# 0. Mission

ASHFALL already has a legal economy.

The source baseline confirms:
- `MarketSystem` handles deterministic demand walks;
- `HoldfastTradeSession` provides faction-gated trade;
- `FactionStanceEngine` controls trade posture;
- economy goods are catalog-driven.

What does not exist is a second **access channel** through the same world economy:

```text
LEGAL ECONOMY
  │
  ├── known sellers
  ├── overt prices
  ├── faction trade rules
  └── legal inventory
```

There is no durable representation of:

```text
UNDERGROUND ECONOMY
  │
  ├── hidden dealers
  ├── restricted goods
  ├── illicit services
  ├── risk of detection
  ├── trust/access
  ├── faction-specific bans
  ├── heat / attention
  └── moral/political consequence
```

The implementation should **not** create a disconnected shadow wallet, shadow inventory, or shadow faction model.

The target architecture is:

```text
WORLD / FACTION / MARKET STATE
        │
        ├── scarcity
        ├── bans/restrictions
        ├── faction stance
        ├── settlement security
        ├── rumor/intelligence
        └── item catalogs
        │
        ▼
BlackMarketSystem
        │
        ├── dealer access
        ├── dealer trust
        ├── contraband classification
        ├── illicit offer construction
        ├── transaction-risk projection
        ├── heat / scrutiny state
        ├── transaction provenance
        └── exactly-once detection resolution
        │
        ▼
EXISTING AUTHORITIES
        │
        ├────────► MarketSystem
        ├────────► InventorySystem
        ├────────► HoldfastTradeSession
        ├────────► FactionStanceEngine
        ├────────► MoralChoiceSystem
        ├────────► dependency / medical
        ├────────► bounty / enforcement
        ├────────► quest runtime
        ├────────► rumor/intelligence
        └────────► journal / epilogue
```

The underground market should answer:

> Which illicit goods/services can the player access here, through whom, at what effective risk, and what will happen if the transaction becomes known?

It should **not** answer:
- how faction standing works;
- how confiscation is stored;
- how addiction works;
- how bounties are simulated;
- how quests run;
- how legal prices evolve.

---

# 1. Source-Evidence Interpretation

## 1.1 The legal market is already deterministic

`MarketSystem` is not missing.

Therefore black market should be:
- another venue/access layer;
- another set of risk/availability inputs;
not a separate economy.

## 1.2 Faction stance already controls legal trade

Illegal trade must still care about:
- settlement owner;
- faction bans;
- hostility;
- enforcement pressure.

A faction may refuse legal trade while a hidden dealer still operates, but that is a **dealer access fact**, not a bypass of all faction/world state.

## 1.3 “Contraband” should be a property/classification, not a duplicate item catalog by default

The source proposes `black_market_goods.json`.

Before creating duplicate goods:
- audit whether existing `economy_goods.json` / `items.json` can add restriction metadata.

Preferred:
- one item ID;
- multiple legality contexts.

## 1.4 Detection should come from real exposure contexts

Source examples:
- settlement entry;
- transaction;
- informant;
- undercover agent.

The plan must distinguish:
- carrying;
- selling/buying;
- being searched;
- being reported.

No arbitrary daily detection roll.

## 1.5 Heat should represent attention, not guilt

Heat is useful if it means:
- scrutiny;
- dealer caution;
- enforcement attention.

It should not duplicate:
- moral band;
- faction standing;
- NPC personal trust;
- public rumor.

## 1.6 Criminal reputation is likely derivable

Plan 131 + faction standing + NPC memory may already represent:
- known illicit trader;
- rumor;
- personal dealer trust.

Therefore “criminal reputation” should not be a new global meter unless an audit proves a separate public-underworld identity is necessary.

## 1.7 Services are much higher scope than goods

Forged papers, smuggling, assassination, laundering, information sales all require different owning systems.

The MVP should ship **goods + dealers + detection + heat + faction/moral consequences** first.

---

# 2. Non-Negotiable Underground-Economy Invariants

## INV-155.1 — One item identity

A contraband item remains the same canonical item ID across legal/illegal contexts.

## INV-155.2 — One inventory authority

Illicit goods live in `InventorySystem` or current item storage.

No shadow inventory.

## INV-155.3 — One market/economy authority

Legal and illegal venues may quote different offers, but underlying scarcity/value data comes from canonical economy state.

## INV-155.4 — Dealer state is personal/venue state

Dealer trust/access belongs to underground-market or NPC relationship state.

It does not replace faction trust.

## INV-155.5 — Contraband legality is contextual

A good may be banned by:
- faction;
- location;
- current policy.

Not globally “illegal” unless data says so.

## INV-155.6 — Detection requires exposure

No detection roll if no:
- search;
- checkpoint;
- transaction;
- informant/report;
- inspection.

## INV-155.7 — Heat is not omniscience

High heat increases the chance/severity of scrutiny where scrutiny can actually occur.

## INV-155.8 — Consequences use existing systems

Confiscation → inventory.
Fine/resource loss → economy/inventory.
Standing → faction.
Moral → MoralChoice.
Bounty → bounty/enforcement.
Addiction → medical/dependency.

## INV-155.9 — Exactly-once transaction consequence

A detected transaction cannot penalize twice on reload/re-entry.

## INV-155.10 — Black market cannot make legal trade obsolete

Legal trade remains:
- safer;
- more stable;
- broader;
- sustainable.

## INV-155.11 — Dealer trust is not a free discount ladder

Trust may unlock:
- better access;
- small bounded pricing improvement;
- special stock.

It does not guarantee permanent arbitrage.

## INV-155.12 — No infinite heat-free laundering

If laundering exists, it has:
- fee;
- capacity;
- source;
- exactly-once conversion.

## INV-155.13 — No generic criminal-career system in MVP

Kingpin/empire/network ownership is follow-on scope.

## INV-155.14 — No generic law-enforcement simulator in MVP

Use existing faction/enforcement/bounty reactions.

## INV-155.15 — No transaction-side health mutation

Drug use effects occur when item is consumed through current medical/needs/dependency systems.

## INV-155.16 — No transaction-side combat mutation

Weapon performance occurs through equipment/combat systems.

## INV-155.17 — Journal/history records meaningful events, not every trivial price quote

## INV-155.18 — All stochastic outcomes are seeded and source-attributed

---

# 3. Definition of Done

Plan 155 closes only when:

- legal market/trade authority is audited;
- existing item catalog is audited for legality metadata;
- current faction bans/policies are audited;
- bounty/enforcement availability is audited;
- rumor/intelligence availability is audited;
- dealer identity model is explicit;
- contraband classification is contextual and data-driven;
- a dealer-access model exists;
- dealer trust is bounded and persistent;
- underground offers derive from canonical goods/scarcity;
- a transaction uses the same inventory/value transfer authority as legal trade;
- detection is tied to real exposure events;
- heat reflects scrutiny and decays deterministically;
- high heat changes dealer availability and enforcement pressure without global omniscience;
- confiscation/fines/standing/moral/bounty consequences route through canonical systems;
- addiction/boost/crash occurs only through item-use systems;
- legal market can react to illicit supply through existing market inputs;
- no shadow inventory or shadow currency exists;
- no duplicate public reputation meter exists unless an ADR proves need;
- old saves load with empty/default underground state;
- save/load round trip is idempotent;
- no transaction or detection retriggers after restore;
- catalog integrity validates dealers, item IDs, faction bans, settlement IDs, localization keys;
- `--black-market-selftest` exists or equivalent;
- 30/120/180-day balance simulations prove legal trade remains relevant;
- max-heat state remains recoverable where design intends;
- anti-arbitrage tests pass;
- no hidden confiscation or unexplained hostility occurs;
- advanced services/organization features are explicitly dispositioned.

---

# 4. Phase P0 — Forensic Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
MarketSystem pricing APIs
HoldfastTradeSession buy/sell transaction path
FactionStanceEngine trade posture
inventory transaction APIs
economy goods IDs
items IDs
current banned/restricted metadata
MoralChoice mutation APIs
dependency/drug-use APIs
bounty/enforcement APIs
rumor/intelligence APIs
quest prerequisite/effect APIs
settlement security/search/checkpoint APIs
journal/history APIs
save sections
```

## P0.2 Build authority matrix

Create:

`docs/economy/BLACK_MARKET_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
black-market role
status
```

Rows:
- item identity;
- item legality;
- legal price;
- illicit offer price;
- inventory;
- dealer identity;
- dealer trust;
- settlement security;
- contraband search;
- transaction detection;
- heat;
- faction standing;
- bounty;
- fine;
- confiscation;
- moral consequence;
- drug use;
- weapon use;
- rumor/public knowledge;
- quest state.

## P0.3 Audit `black_market_goods.json` duplication risk

Preferred designs:

### Option A — extend canonical goods
Add:

```text
restriction_tags[]
faction_bans[]
underground_categories[]
```

### Option B — separate black-market listing catalog
References canonical `good_id`.

Do **not** duplicate base item data.

## P0.4 Audit dealer identity

Can dealers reuse:
- named NPC catalog;
- traveling caravan/trader entities?

If yes:
- use canonical NPC ID.

If not:
- create stable dealer definition IDs.

## P0.5 Audit enforcement seams

Determine whether current systems support:
- fines;
- confiscation;
- bounty;
- search;
- patrol investigation.

Anything absent is deferred.

## P0.6 Baseline proof

Reproduce current:
- all trade legal;
- no dealer access;
- no contraband classification;
- no heat/detection.

---

# TASK 155A — Contraband, Dealers, Access & Offer Construction

# 155A.0 Goal

Create an underground venue layer on top of canonical items and market state.

---

# 155A-G — Contraband Classification

## 155A.G1 Contraband definition

Suggested:

```text
good_id
category
restriction_tags[]
faction_bans[]
base_risk
moral_context
required_access_level
```

No duplicate item name/value if canonical item already has it.

## 155A.G2 Categories

MVP categories:

```text
restricted_medicine
restricted_weapons
stolen_goods
sensitive_intel
restricted_technology
```

Avoid real-world detailed weapon/drug trafficking mechanics.

## 155A.G3 “Services” separated

Services are not goods.

Do not mix:
- assassination;
- forgery;
- smuggling
into inventory catalog.

## 155A.G4 Illegality is faction-relative

A faction ban list references canonical faction IDs.

## 155A.G5 Location restriction

Optional:
- settlement/policy tags.

## 155A.G6 Moral context

Do not give every illegal item an automatic moral penalty.

MoralChoice rules decide whether the **transaction/action** is morally relevant.

## 155A.G7 Detection risk

Base risk is only one input.

Actual exposure risk also depends on:
- security;
- heat;
- concealment;
- transaction context.

## 155A.G8 Data integrity

Validate all IDs.

---

# 155A-D — Dealer Identity & Access

## 155A.D1 Dealer DTO

Suggested:

```text
dealer_id
npc_id optional
display_key
location_rule
specialties[]
access_requirements[]
trust
heat_tolerance
inventory_profile
refresh_policy
```

## 155A.D2 Stable dealer ID

Data-defined.

No runtime GUID.

## 155A.D3 Named vs itinerant

Named dealer:
- stable location/NPC.

Itinerant:
- current caravan/travel system if possible.

No new moving-dealer simulator.

## 155A.D4 Dealer specialty

Controls:
- eligible offers;
- not separate economy.

## 155A.D5 Dealer trust

Range:
- 0..100 or current relationship standard.

## 155A.D6 Dealer trust vs NPC personal trust

If Plan 147 dealer NPC exists:
- decide whether underground trust derives from personal memory/trust.

Avoid two unrelated relationship meters.

ADR:

`ADR_DEALER_TRUST_VS_NPC_TRUST.md`

## 155A.D7 Access requirements

Possible:
- introduction/rumor;
- faction standing;
- personal NPC trust;
- quest flag;
- previous small deal.

## 155A.D8 No automatic access from “criminal reputation”

Unless current public reputation exists.

## 155A.D9 Dealer refusal

Reasons:
- insufficient trust;
- high heat;
- hostile faction;
- unavailable inventory.

Typed, visible.

## 155A.D10 Dealer inventory

Construct from:
- profile;
- canonical goods;
- scarcity;
- refresh seed/day.

## 155A.D11 Refresh

Campaign-time deterministic.

No per-frame.

## 155A.D12 Inventory quantity

Real stock.

No infinite buy.

## 155A.D13 Dealer funds/barter

Use existing trade model.

No shadow currency.

---

# 155A-O — Offer Construction

## 155A.O1 Price inputs

Potential:

```text
canonical market value
× scarcity
× underground risk premium
× dealer trust modifier
× heat modifier
× faction/location modifier
```

## 155A.O2 One pricing authority

Prefer a `MarketQuote`/pricing service extension.

Do not hardcode parallel price arithmetic throughout UI/system.

## 155A.O3 Risk premium

Data-driven.

## 155A.O4 Trust discount

Bounded.

## 155A.O5 Heat premium

High heat may increase:
- dealer risk premium.

## 155A.O6 Bulk pricing

Only if legal trade already supports bulk pricing or a reusable quote mechanism exists.

## 155A.O7 No guaranteed arbitrage

Buy/sell spread + scarcity + risk prevent loops.

## 155A.O8 Quote determinism

Same:
- day;
- dealer;
- state
=> same quote.

## 155A.O9 Quote expiry

If market day changes:
- quote refresh.

## 155A.O10 No quote persistence required

Unless current trade session persists offers.

### 155A DoD

The game can classify restricted canonical goods, resolve hidden dealers, enforce access rules, and build deterministic illicit offers without creating a separate economy or inventory.

---

# TASK 155B — Transactions, Detection, Heat & Concealment

# 155B.0 Goal

Make illicit trade risky because exposure can occur through real transaction/search/report channels.

---

# 155B-T — Transaction

## 155B.T1 Transaction DTO

Suggested:

```text
transaction_id
dealer_id
good_id
quantity
unit_price
total_value
day
location_id
risk_context
detection_state
source_quote_id optional
```

## 155B.T2 Stable transaction ID

Use canonical trade transaction identity if available.

Otherwise deterministic sequence from campaign transaction owner.

## 155B.T3 Buy/sell path

Reuse inventory + economic transfer transaction.

## 155B.T4 Atomicity

Transaction commits:
- goods;
- payment;
- underground ledger
together.

## 155B.T5 Failure rollback

No:
- paid but no goods;
- goods but no payment.

## 155B.T6 Transaction logging

Record compact history.

## 155B.T7 No illegal-currency wallet

Use current currency/barter.

---

# 155B-X — Detection Context

## 155B.X1 Exposure sources

Supported only where real:

```text
dealer transaction
settlement search/checkpoint
informant/report
inventory inspection
quest/event
```

## 155B.X2 Carrying contraband

Possession alone does not roll detection daily.

Detection occurs when inspected.

## 155B.X3 Settlement entry

Only if settlement security/checkpoint event exists.

If absent:
- defer entry-search detection.

## 155B.X4 Dealer transaction

Can have:
- informant risk;
- observation risk
through seeded transaction resolution.

## 155B.X5 Use detection

Drug side effects are health, not law detection.

Public use/report only if event/witness system exists.

## 155B.X6 Source-attributed detection

Record:

```text
source
day
location
faction
transaction_id/item refs
```

## 155B.X7 Detection result

Possible:

```text
undetected
suspicious
reported
confirmed
```

Use actual information model.

## 155B.X8 Witness/informant

Reuse Plan 131 rumor/intelligence/NPC memory where possible.

## 155B.X9 No omniscient faction reaction

Faction consequence occurs when the relevant authority learns/confirms.

---

# 155B-H — Heat

## 155B.H1 Heat meaning

“Attention/scrutiny generated by illicit behavior.”

## 155B.H2 Scope

Audit whether heat should be:
- global;
- per-faction;
- per-settlement.

Global 0–100 may be too coarse.

Recommended:
- per enforcement jurisdiction/faction if affordable.

## 155B.H3 MVP heat

If faction system is small:
- `heatByFaction`.

If not:
- one global heat with explicit limitation ADR.

## 155B.H4 Heat sources

Data-driven:
- illicit transaction;
- confirmed detection;
- large value;
- repeated use of same dealer;
- known violence related to deal.

## 155B.H5 No heat for invisible acts by default

Secret transaction may still create **dealer-network risk**, but not faction enforcement heat unless exposure occurred.

Consider two concepts if needed:
- underground activity pressure;
- enforcement heat.

Avoid one misleading meter.

## 155B.H6 Simpler preferred model

MVP:
- transaction risk rises with recent underground activity;
- faction heat rises only on known/suspicious events.

## 155B.H7 Heat decay

Campaign-day deterministic.

## 155B.H8 Decay is jurisdiction-specific

No instant clean slate.

## 155B.H9 Laying low

No activity:
- natural decay.

## 155B.H10 Bribe

Only if current faction/trade/choice system supports.

## 155B.H11 Legal quests reduce heat

Only if authored faction consequence exists.

No generic wash-away crime button.

## 155B.H12 Dealer caution

High relevant heat can:
- reduce stock;
- increase premium;
- refuse.

## 155B.H13 Enforcement response

Use existing systems:
- search;
- bounty;
- faction suspicion.

## 155B.H14 Heat cap/floor

Bounded.

---

# 155B-C — Concealment / Smuggling

## 155B.C1 MVP scope

Do not create a full smuggling minigame.

## 155B.C2 Concealment input

Only if current expedition/inventory has:
- container;
- concealment;
- cargo tags.

Otherwise detection risk uses current context only.

## 155B.C3 Smuggling route

Only if Plan 32/133 route system supports risk-tagged route choices.

## 155B.C4 Forged documents

Only if item/quest/checkpoint systems support credentials.

Otherwise defer.

## 155B.C5 Search resolution

Seeded.

## 155B.C6 Confiscation

Canonical inventory removal.

## 155B.C7 Partial confiscation

Only if authored.

## 155B.C8 Fine

Use current currency/resource transaction.

## 155B.C9 Refusal/fight

If event offers resistance:
- use choice/combat systems.

No enforcement combat engine.

### 155B DoD

Illegal trade produces deterministic, context-bound exposure risk and bounded scrutiny without daily random punishment or faction omniscience.

---

# TASK 155C — Faction, Moral, Inventory, Market & Medical Integration

# 155C.0 Goal

Push underground consequences into existing domain authorities exactly once.

---

# 155C-F — Faction Reaction

## 155C.F1 Faction standing

Confirmed illicit action may produce standing/trust delta if:
- faction bans good/action;
- faction knows.

## 155C.F2 One faction mutation path

Reuse faction authority from Plan 139 ADR.

## 155C.F3 No double trust/standing

Same invariant as Plan 139.

## 155C.F4 Faction-specific tolerance

One faction may ignore a good another bans.

Data-driven.

## 155C.F5 Dealer faction

Dealer may belong to:
- faction;
- neutral;
- independent.

Personal dealer relationship stays separate.

## 155C.F6 Embargo

Use existing embargo/trade stance.

## 155C.F7 Bounty

Only if bounty/enforcement authority exists.

## 155C.F8 Investigation

If no investigation system:
- represent as increased known heat / event / bounty threshold,
not new simulator.

## 155C.F9 Confiscation authority

Faction/enforcement event tells inventory what to remove.

## 155C.F10 Fines

Use real payment.

---

# 155C-M — Moral Choice

## 155C.M1 No universal “illegal = immoral”

Moral consequence depends on:
- good/service;
- context;
- harm.

## 155C.M2 Use MoralChoiceSystem

Transaction can emit an authored moral-choice consequence.

## 155C.M3 Restricted medicine

Buying medicine illegally to save a life may have:
- no negative moral effect;
- or a tradeoff.

## 155C.M4 Exploitative goods/services

May carry stronger moral consequence.

## 155C.M5 No hidden moral arithmetic in BlackMarketSystem

Use typed effect/adaptor.

## 155C.M6 Player readability

If moral consequence is known:
- show before confirm.

---

# 155C-I — Inventory / Item Use

## 155C.I1 Contraband stored normally

Item metadata/legality context separate.

## 155C.I2 Detection scans canonical inventory

## 155C.I3 Drug use

Uses current consume/medical/dependency path.

## 155C.I4 Weapons

Uses existing equipment/combat.

## 155C.I5 Intel

Uses journal/quest/intelligence content rails.

## 155C.I6 Stolen goods

Use provenance/tag only if item system supports.

## 155C.I7 No duplicate illegal-item stack type

---

# 155C-E — Legal Market Interaction

## 155C.E1 Illicit supply effect

Only if MarketSystem accepts:
- supply shock;
- scarcity modifier;
- transaction volume.

## 155C.E2 No direct black-market price override

## 155C.E3 Market flooding

If supported:
- large illicit sale affects scarcity/availability.

## 155C.E4 Market feedback bounded

No one transaction permanently destroys price.

## 155C.E5 Legal sellers

May refuse contraband via trade policy.

## 155C.E6 Fence/laundering

Only if current goods provenance/value systems support.

## 155C.E7 Money laundering

If game has one fungible currency with no clean/dirty distinction:
- do not invent dirty money.

Defer.

## 155C.E8 Goods laundering

Possible:
- fence converts stolen/restricted provenance into ordinary sale eligibility
only if provenance exists.

---

# 155C-R — Rumor / Public Knowledge

## 155C.R1 Plan 131 reuse

Detection/report can publish:
- "player trafficked restricted goods".

## 155C.R2 No new criminal reputation system by default

Public awareness derives from:
- rumors;
- faction standing;
- dealer/NPC memory.

## 155C.R3 Dealer memory

Plan 147 can store:
- reliable customer;
- betrayed dealer;
- unpaid debt
if dealer is named NPC.

## 155C.R4 No automatic emotional cloning

Same Plan 147 rule.

---

# 155C-Q — Quest Integration

## 155C.Q1 Quest runtime owns quest lifecycle

## 155C.Q2 Possible predicates

```text
has_black_market_access
dealer_trust_at_least
heat_at_least
has_contraband
transaction_detected
```

## 155C.Q3 Source quest ideas

- Undercover;
- Smuggle;
- Informant;
- Redemption;
- Heist.

Treat as content backlog.

## 155C.Q4 No “criminal empire” runtime

`Kingpin` / `Empire` are follow-on scope.

## 155C.Q5 Existing quests first

Audit current quest corpus.

---

# 155C-V — Violence / Combat

## 155C.V1 Deal goes violent

Only through existing encounter/combat.

## 155C.V2 Black-market system never resolves attacks

## 155C.V3 Plan 139

Faction-tagged combat consequence still applies.

## 155C.V4 Dealer betrayal

If event:
- authored encounter;
- not random kill switch.

### 155C DoD

Underground transactions alter faction, moral, inventory, market, medical, quest, rumor, and combat state only through canonical domain owners.

---

# TASK 155D — UI, Persistence, Balance, Determinism & CI

# 155D.0 Goal

Make underground trade legible, save-safe, deterministic, strategically viable, and unable to dominate the legal economy.

## 155D.1 UI route decision

Audit existing:
- market panel;
- trade panel;
- dealer NPC dialogue.

Preferred:
- dealer-specific underground trade mode inside existing trade surface.

New panel only if needed.

## 155D.2 Dealer surface

Show:
- dealer name;
- specialty;
- trust/access;
- current offers;
- relevant heat;
- risk band.

## 155D.3 Contraband tooltip

Show:
- item;
- category;
- banned factions;
- known detection risk;
- likely consequences.

## 155D.4 Uncertain risk

Do not show exact percentage if player lacks security/intel information.

## 155D.5 Transaction confirm

Show:
- quantity;
- price;
- known risk;
- known moral/political consequence.

## 155D.6 Post-transaction

Show:
- completed;
- detected/suspicious if player knows;
- heat change if visible.

## 155D.7 Heat UI

If faction-scoped:
- show relevant jurisdiction.

## 155D.8 Dealer trust

Keep separate from faction standing.

## 155D.9 Journal

Meaningful:
- first dealer;
- major deal;
- detected;
- bounty;
- betrayal;
- redemption.

No every-sale spam.

## 155D.10 Tutorial

Only current tutorial framework.

## 155D.11 Accessibility

No color-only risk.

## 155D.12 Localization

All labels/reasons keyed.

---

# 155D-P — Persistence

## 155D.P1 New state only for genuinely new facts

Potential:

```text
schema_version
dealer_state[]
transaction_refs/history
heat_by_faction
processed_detection_ids
access_unlocks
```

## 155D.P2 Do not persist

- inventory duplicate;
- faction standing;
- market prices;
- quest state;
- bounty;
- moral band.

## 155D.P3 Old save

Empty dealer/heat/history.

## 155D.P4 Dealer refresh

Restores deterministic state.

## 155D.P5 Mid-transaction save

Prefer transactions atomic and not saveable mid-commit.

## 155D.P6 Post-transaction pre-detection

If detection resolves same transaction:
- commit together.

## 155D.P7 Pending report

If rumor/report delayed:
- owning rumor system persists.

## 155D.P8 Reload idempotence

No duplicate:
- item transfer;
- fine;
- confiscation;
- standing;
- moral consequence;
- heat;
- bounty.

---

# 155D-D — Determinism

## 155D.D1 Dealer inventory

Same day/seed/state:
- same offers.

## 155D.D2 Quote

Same inputs:
- same price.

## 155D.D3 Detection

Same seed/context:
- same outcome.

## 155D.D4 Heat decay

Pure campaign-time.

## 155D.D5 No wall clock

## 155D.D6 No unseeded `Random.Shared`

## 155D.D7 Stable iteration

Sort dealer/good IDs.

---

# 155D-B — Balance

## 155D.B1 Legal vs underground comparison

Track:
- profit margin;
- risk;
- availability;
- volatility;
- access cost.

## 155D.B2 Legal trade must remain useful

Reference scenario:
- legal lower upside;
- safer;
- more reliable.

## 155D.B3 Underground premium

High margin on:
- rare/restricted goods;
not every commodity.

## 155D.B4 Risk budget

Detection cannot be:
- trivial;
- constant guaranteed punishment.

## 155D.B5 Heat recovery

Player can reduce attention through time/behavior where design allows.

## 155D.B6 Max heat

Not permanent campaign lockout unless intentionally authored.

## 155D.B7 Dealer refusal

At extreme heat:
- some/all dealers may refuse.

Recovery path explicit.

## 155D.B8 Anti-arbitrage

Test:
- legal buy → black sell;
- black buy → legal sell;
- dealer A → dealer B.

No risk-free infinite loop.

## 155D.B9 Bulk exploit

Trust/bulk discounts cannot invert buy/sell spread.

## 155D.B10 Detection exploit

Save/reload cannot reroll outcome if transaction already committed.

## 155D.B11 Contraband hoarding

Carrying risk only applies at real inspection.

No hidden daily punishment.

## 155D.B12 Faction hostility runaway

One small illicit deal should not universally trigger war.

## 155D.B13 Moral spam

Repeated tiny deals cannot crater moral band faster than intended without diminishing/event policy.

## 155D.B14 Medical exploit

Restricted medicine black-market access must not trivialize scarcity.

## 155D.B15 Weapon exploit

Restricted equipment acquisition balanced by:
- price;
- rarity;
- access;
- risk.

No tactical details beyond game abstraction.

---

# 155D-S — Simulation

## 155D.S1 30-day scenario

Small dealer use.

Track:
- profit;
- heat;
- legal trade share.

## 155D.S2 120-day scenario

Regular underground trader.

Track:
- detection frequency;
- faction outcomes;
- market interaction;
- dealer trust.

## 155D.S3 180-day stress

Aggressive underground activity.

Assert:
- no economy collapse;
- no infinite profit;
- consequences remain bounded/legible.

## 155D.S4 No-black-market case

Zero use:
- no overhead/penalties.

## 155D.S5 Max-heat case

Dealer refusal/recovery.

## 155D.S6 Multi-faction bans

Same good:
- legal in one jurisdiction;
- banned in another.

## 155D.S7 Dealer scarcity

Stock refresh stays finite.

---

# 155D-T — Testing & CI

## 155D.T1 Data integrity

Validate:
- dealer IDs;
- good IDs;
- faction IDs;
- location IDs;
- categories;
- risk bounds;
- trust bounds;
- localization.

## 155D.T2 Selftest

Create:

```text
--black-market-selftest
```

## 155D.T3 Selftest cases

At least:
1. no underground access;
2. dealer unlock;
3. quote;
4. buy contraband;
5. sell contraband;
6. deterministic detection;
7. confiscation;
8. faction consequence;
9. moral consequence;
10. heat increase/decay;
11. dealer refusal;
12. old save;
13. save/load idempotence;
14. anti-arbitrage.

## 155D.T4 Headless

No UI dependency.

## 155D.T5 Source-scan authority gate

Detect:
- duplicate inventory list;
- duplicate faction trust;
- direct drug-effect mutation;
- direct combat stats;
- bounty state stored in black market;
- quest state stored in black market.

## 155D.T6 Failure fixtures

Each important gate proves failure.

## 155D.T7 Generated docs

Create:
- `BLACK_MARKET_ARCHITECTURE.md`;
- `BLACK_MARKET_GOODS_MATRIX.md`;
- `BLACK_MARKET_DEALER_MATRIX.md`;
- `BLACK_MARKET_REACTION_DISPOSITION.md`;
- `BLACK_MARKET_BALANCE_REPORT.md`.

### 155D DoD

The underground economy is understandable, deterministic, risky, bounded, persistent, and strategically distinct from legal trade without replacing it.

---

# TASK 155E — Advanced Underground Services & Criminal Organization: Explicitly Gated

# 155E.0 Goal

Prevent Plan 155 from quietly expanding into a crime-management game.

## 155E.1 Assassination contracts

Default:
- DEFER.

Only if:
- quest/contract system;
- combat target system;
- faction consequence
already support it.

## 155E.2 Forged documents

Only if:
- checkpoint/credential system exists.

Otherwise defer.

## 155E.3 Smuggling routes

Only if:
- route risk;
- cargo inspection;
- checkpoint systems
exist.

## 155E.4 Heists

Content/quest follow-on.

## 155E.5 Undercover operations

Requires:
- faction quest;
- cover/identity or dialogue mechanics.

Defer unless current systems suffice.

## 155E.6 Criminal organization

Default:
- DEFER.

Would require:
- network;
- employees;
- territory;
- income;
- rival dealers.

## 155E.7 Dealer wars

Default:
- DEFER.

## 155E.8 Laundering money

Reject if game has no dirty-money distinction.

## 155E.9 Laundering stolen goods

Possible only if provenance exists.

## 155E.10 Criminal legacy

Epilogue can derive from:
- transaction history;
- faction outcomes;
- rumors.

No new permanent reputation stat required.

### 155E DoD

Advanced crime features remain explicit follow-ons unless their owning systems already exist.

---

# 5. Underground Market State Model

```text
NO ACCESS
   │
   └── introduction / discovery
        ▼
DEALER ACCESS
   │
   ├── trusted enough
   ├── offers available
   └── heat acceptable
        │
        ▼
TRANSACTION
   │
   ├── completes
   └── exposure resolution
        │
        ├── undetected
        ├── suspicious
        └── confirmed
              │
              ▼
        CANONICAL CONSEQUENCES
```

---

# 6. Contraband Classification Contract

A canonical item may have:

```text
restriction tags
faction bans
underground categories
```

The item itself remains one item.

---

# 7. Dealer Contract

Dealer owns:
- access;
- trust;
- stock profile;
- willingness.

Dealer does **not** own:
- item definitions;
- faction standing;
- inventory;
- market scarcity.

---

# 8. Dealer Trust Contract

Prefer:

```text
dealer access trust
```

only if it cannot cleanly reuse Plan-147 personal trust.

Do not create a second relationship meter for a named dealer without ADR.

---

# 9. Quote Contract

Illicit quote should be traceable:

```text
canonical base
+ scarcity
+ risk premium
+ dealer relationship
+ heat/jurisdiction
```

One quote service if possible.

---

# 10. Detection Contract

Detection is **event-bound**.

Forbidden:

```text
every day:
  roll detection for every contraband item
```

Allowed:

```text
checkpoint search
transaction observation
informant report
inspection event
```

---

# 11. Heat Contract

Heat measures:
- attention.

It is not:
- morality;
- faction standing;
- personal dealer trust;
- public reputation.

---

# 12. Information Contract

A transaction can be:

```text
secret
suspected
reported
confirmed
```

Faction response waits for appropriate information.

---

# 13. Faction-Ban Contract

Same item can be:

```text
Faction A: legal
Faction B: restricted
Faction C: prohibited
```

No global hardcoded legal flag unless universal.

---

# 14. Moral Contract

Illegality and morality are separate.

Examples:
- illegal medicine for survival;
- stolen humanitarian supplies;
- restricted weapons;
- exploitative service.

MoralChoice authority decides consequences.

---

# 15. Inventory Contract

Contraband is ordinary canonical inventory plus legality/provenance context.

No shadow stash unless a real storage/container system models it.

---

# 16. Market Contract

Black market cannot own independent scarcity truth.

Legal and underground trade draw from/shared with:
- market;
- dealer stock;
- world supply.

---

# 17. Medical Contract

Buying restricted medicine:
- item acquisition.

Using it:
- medical/dependency system.

No transaction-side buff/crash.

---

# 18. Combat Contract

Buying weapon:
- item acquisition.

Equipping/using:
- equipment/combat system.

No direct combat stat bonus in black-market code.

---

# 19. Bounty / Enforcement Contract

Confirmed illicit activity may call:

```text
enforcement/bounty authority
```

If absent:
- defer bounty.

Do not create it as a black-market field.

---

# 20. Quest Contract

Underground state may expose predicates:

```text
dealer_access
heat_band
transaction_detected
has_contraband
```

Quest runtime owns everything else.

---

# 21. Rumor Contract

Known criminal activity is an information fact.

Plan 131 owns:
- propagation;
- recipients;
- credibility.

---

# 22. Persistence Matrix

| Fact | Owner |
|---|---|
| item definition | item/economy catalog |
| contraband restriction metadata | item/black-market data |
| inventory | inventory |
| dealer trust/access | black-market or NPC memory |
| dealer stock state | black-market |
| transaction history | black-market/history |
| heat | black-market |
| faction standing | faction |
| moral band | MoralChoice |
| bounty | enforcement |
| quest state | quest |
| drug effects | medical/dependency |
| legal market prices | MarketSystem |
| rumor knowledge | rumor/intel |

---

# 23. Old-Save Migration

Default:

```text
black_market:
  schema_version: 1
  dealers: []
  heat: {}
  transaction_history: []
  processed_detection_ids: []
```

Dealer definitions may load from catalog while relationship state starts neutral/locked.

---

# 24. Idempotence Contract

Key identities:

```text
transaction_id
detection_event_id
consequence_source_id
```

Reload cannot repeat:
- transfer;
- confiscation;
- fine;
- standing;
- moral effect;
- bounty;
- heat.

---

# 25. Failure Injection Matrix

## N155.1 Same item duplicated in legal and black-market catalogs
Expected: item-identity gate fails.

## N155.2 Transaction uses separate shadow inventory
Expected: authority gate fails.

## N155.3 Carrying contraband triggers random daily detection
Expected: detection-context gate fails.

## N155.4 Faction reacts to secret deal without information source
Expected: information gate fails.

## N155.5 Dealer trust and NPC trust diverge for same named dealer without ADR
Expected: relationship authority gate fails.

## N155.6 Black market directly changes drug needs/health
Expected: medical-authority gate fails.

## N155.7 Black market directly changes weapon combat stats
Expected: combat-authority gate fails.

## N155.8 Save/reload rerolls transaction detection
Expected: idempotence/determinism fail.

## N155.9 Legal buy / black sell yields infinite risk-free profit
Expected: arbitrage gate fails.

## N155.10 High heat permanently locks all underground content with no intended recovery
Expected: balance/recovery gate fails.

## N155.11 Black-market system stores bounty state
Expected: architecture gate fails.

## N155.12 One small transaction craters every faction standing
Expected: faction-scope balance fails.

---

# 26. Determinism Contract

Same:

```text
day
+ dealer
+ market state
+ player inventory/resources
+ faction restrictions
+ heat
+ transaction choice
+ seeded detection context
```

must yield same:
- offers;
- prices;
- transaction result;
- detection;
- heat;
- consequence IDs.

---

# 27. Long-Horizon Metrics

Track:

```text
underground transactions
legal transactions
underground profit
legal profit
dealer trust distribution
heat distribution
detections
confiscated value
fines
standing changes
moral consequences
bounties if real
dealer refusals
arbitrage opportunities
market price impact
```

---

# 28. Balance Guardrails

The black market should be:

```text
high-value
narrow
volatile
risky
situational
```

Legal trade should be:

```text
broad
stable
lower-risk
sustainable
```

Both remain strategically valid.

---

# 29. Heat Tuning Guardrails

Avoid source's fixed values as immutable truth.

Treat:
- 50+ cautious;
- 80+ refusal
as candidate tuning bands.

Use telemetry.

---

# 30. Detection Tuning Guardrails

Source proposes:
- 5% entry;
- 10% transaction.

Treat as candidate values.

Actual formula must respect:
- context;
- security;
- heat;
- information.

---

# 31. Anti-Arbitrage Matrix

Test:

```text
legal → underground
underground → legal
dealer A → dealer B
same dealer buy → sell
bulk trust discount
heat premium changes
```

All include transaction cost/spread/risk.

---

# 32. Legal-Economy Preservation

Reference scenarios should show:
- ordinary necessities cheaper/safer legally;
- restricted/rare goods more accessible underground;
- no universal best venue.

---

# 33. UI Acceptance

## Dealer access
- hidden/locked reason.

## Dealer view
- specialty;
- trust;
- heat;
- stock.

## Offer
- canonical item;
- price;
- known risk;
- restriction context.

## Confirm
- exact transaction;
- risk band;
- known political/moral consequence.

---

# 34. Accessibility

- no color-only heat;
- no red/green-only legality;
- tooltips keyboard accessible;
- text scaling;
- risk expressed in words/icons.

---

# 35. Localization

All:
- dealer names;
- specialties;
- risk reasons;
- consequence reasons;
- refusal strings
keyed.

---

# 36. Content Acceptance

Dealer/goods/rules:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Every shipped contraband definition must appear in a reachable offer or explicit staged/deferred set.

---

# 37. Reachability

For each dealer:

```text
can be discovered?
can access condition be met?
can inventory populate?
can player complete transaction?
```

For each good:

```text
exists?
dealer can offer?
restriction applies somewhere?
transaction possible?
effect/use owned elsewhere?
```

---

# 38. Performance Guardrails

Black-market daily work:
- dealer refresh only when due;
- heat decay indexed;
- no full item catalog scan if indexed by category.

No per-frame work.

---

# 39. CI / Gate Set

Recommended:

```text
black_market_item_identity
black_market_dealer_integrity
black_market_quote_determinism
black_market_transaction_atomicity
black_market_detection_context
black_market_information_gate
black_market_heat_bounds
black_market_no_shadow_inventory
black_market_no_duplicate_faction_authority
black_market_anti_arbitrage
black_market_save_matrix
black_market_long_horizon
black_market_ui_access
```

---

# 40. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --black-market-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 41. Recommended Commit Breakdown

```text
155A-1 market/inventory/faction/enforcement audit
155A-2 contraband metadata schema
155A-3 dealer catalog/identity
155A-4 dealer access/trust ADR
155A-5 deterministic stock refresh
155A-6 illicit quote integration
155A-7 item/faction/location integrity
155A-8 docs/selftests

155B-1 atomic underground transaction
155B-2 detection context model
155B-3 transaction observation/informant path
155B-4 faction/jurisdiction heat
155B-5 heat decay/dealer caution
155B-6 confiscation/fine adapters
155B-7 concealment/smuggling seam audit
155B-8 save/idempotence tests

155C-1 faction consequence adapter
155C-2 moral-choice adapter
155C-3 inventory/item-use integration
155C-4 market supply feedback
155C-5 rumor/NPC-memory integration
155C-6 quest predicates
155C-7 combat/violence adapter if real
155C-8 cross-system authority tests

155D-1 dealer/trade UI
155D-2 risk/heat/localization/accessibility
155D-3 old-save/roundtrip
155D-4 deterministic replay
155D-5 30-day balance
155D-6 120/180-day anti-arbitrage/economy stress
155D-7 CI gates/failure fixtures
155D-8 final ship/no-ship report

155E-1 advanced service disposition
155E-2 laundering/credential/smuggling audit
155E-3 heist/undercover follow-on
155E-4 criminal organization defer
```

---

# 42. Risk Register

## R155.1 Black market replaces legal economy

Mitigation:
- narrow restricted stock;
- risk premium;
- stock limits;
- legal-trade balance tests.

## R155.2 Heat becomes magic omniscience

Mitigation:
- information/exposure gating;
- jurisdiction scope.

## R155.3 Duplicate item catalogs drift

Mitigation:
- reference canonical item IDs.

## R155.4 Dealer trust duplicates NPC memory

Mitigation:
- ADR;
- reuse Plan 147 where possible.

## R155.5 Detection feels random/punitive

Mitigation:
- event-bound searches;
- visible risk bands;
- seeded determinism.

## R155.6 Moral system becomes “illegal = evil”

Mitigation:
- contextual MoralChoice integration.

## R155.7 Criminal services explode scope

Mitigation:
- 155E explicit defer gate.

## R155.8 Arbitrage destroys economy

Mitigation:
- quote integration;
- spreads;
- stock;
- anti-arbitrage simulation.

---

# 43. Acceptance Checklist

## P0

- [ ] MarketSystem audited
- [ ] HoldfastTradeSession audited
- [ ] FactionStanceEngine audited
- [ ] inventory transaction path audited
- [ ] economy goods/items audited
- [ ] restriction metadata audited
- [ ] MoralChoice APIs audited
- [ ] medical/dependency item-use audited
- [ ] bounty/enforcement audited
- [ ] rumor/intelligence audited
- [ ] quest predicates audited
- [ ] checkpoint/search systems audited
- [ ] journal/history audited
- [ ] save sections audited
- [ ] black-market authority matrix published
- [ ] baseline no-underground behavior reproduced

## 155A — Goods

- [ ] one canonical item identity
- [ ] no duplicate names/base values
- [ ] restriction tags
- [ ] faction bans
- [ ] categories
- [ ] services excluded from goods
- [ ] contextual legality
- [ ] moral effect not automatic
- [ ] detection risk only base input
- [ ] reference integrity

## 155A — Dealers

- [ ] stable dealer IDs
- [ ] named NPC reuse assessed
- [ ] itinerant uses existing travel if possible
- [ ] specialties
- [ ] dealer trust bounded
- [ ] dealer vs NPC trust ADR
- [ ] access requirements
- [ ] no global criminal-reputation dependency
- [ ] typed refusal reasons
- [ ] deterministic inventory refresh
- [ ] finite quantities
- [ ] normal currency/barter

## 155A — Quotes

- [ ] one quote service/path
- [ ] canonical market value
- [ ] scarcity
- [ ] risk premium
- [ ] bounded trust modifier
- [ ] heat modifier
- [ ] no guaranteed arbitrage
- [ ] deterministic quote
- [ ] quote expiry

## 155B — Transactions

- [ ] stable transaction ID
- [ ] canonical inventory transfer
- [ ] atomic payment/goods
- [ ] rollback
- [ ] compact history
- [ ] no shadow currency
- [ ] no duplicate transaction on reload

## 155B — Detection

- [ ] real exposure sources
- [ ] no daily possession roll
- [ ] settlement entry only if checkpoint exists
- [ ] transaction detection seeded
- [ ] public use only if witness system exists
- [ ] detection source attribution
- [ ] information states
- [ ] Plan 131 reuse
- [ ] no omniscient faction reaction

## 155B — Heat

- [ ] heat meaning documented
- [ ] global vs faction scope ADR
- [ ] known/suspicious event sources
- [ ] no invisible-action enforcement heat by default
- [ ] campaign-time decay
- [ ] bounded heat
- [ ] laying-low semantics
- [ ] bribe only if real
- [ ] legal quest reduction only if authored
- [ ] dealer caution
- [ ] enforcement adapter

## 155B — Smuggling

- [ ] no minigame
- [ ] concealment only if inventory/cargo supports
- [ ] route smuggling only if route risk supports
- [ ] forged documents only if credentials exist
- [ ] seeded search
- [ ] canonical confiscation
- [ ] canonical fine
- [ ] resistance through existing choice/combat

## 155C — Faction

- [ ] one faction mutation path
- [ ] no standing/trust double count
- [ ] faction-specific tolerance
- [ ] dealer faction status separate
- [ ] embargo authority reused
- [ ] bounty authority reused
- [ ] no investigation simulator
- [ ] confiscation canonical
- [ ] fine canonical

## 155C — Moral

- [ ] illegal != automatically immoral
- [ ] MoralChoice authority reused
- [ ] contextual medicine case
- [ ] exploitative cases authored
- [ ] no hidden moral arithmetic
- [ ] consequence shown before confirm where known

## 155C — Inventory/Use

- [ ] contraband stored normally
- [ ] scans canonical inventory
- [ ] drug effects only on consumption
- [ ] weapon effects only on equip/use
- [ ] intel through journal/quest/intelligence
- [ ] stolen provenance only if supported
- [ ] no shadow stack type

## 155C — Market

- [ ] illicit supply feedback only if MarketSystem supports
- [ ] no direct price override
- [ ] bounded market flooding
- [ ] legal sellers use trade policy
- [ ] dirty money not invented
- [ ] goods laundering only if provenance exists

## 155C — Rumor/Quest/Combat

- [ ] Plan 131 reused
- [ ] no new criminal reputation by default
- [ ] Plan 147 dealer memory if named NPC
- [ ] quest predicates only
- [ ] criminal empire deferred
- [ ] violence through existing encounter/combat
- [ ] Plan 139 combat consequences still apply
- [ ] no dealer-betrayal kill switch

## 155D — UI

- [ ] existing trade surface reused if possible
- [ ] dealer identity/specialty
- [ ] trust/access
- [ ] heat/risk band
- [ ] contraband restrictions
- [ ] uncertain risk hidden appropriately
- [ ] transaction confirm
- [ ] post-transaction result
- [ ] faction-scoped heat where relevant
- [ ] journal bounded
- [ ] tutorial only if framework exists
- [ ] accessibility
- [ ] localization

## 155D — Persistence/Determinism

- [ ] only new state persisted
- [ ] no duplicate inventory/faction/market/quest/bounty state
- [ ] old-save default
- [ ] deterministic dealer refresh
- [ ] atomic transaction save
- [ ] pending reports owned by rumor
- [ ] reload idempotence
- [ ] deterministic quotes
- [ ] deterministic detection
- [ ] deterministic heat
- [ ] no wall-clock/GUID
- [ ] stable iteration

## 155D — Balance

- [ ] legal vs underground benchmark
- [ ] legal remains viable
- [ ] underground premium narrow
- [ ] risk not trivial
- [ ] heat recoverable where intended
- [ ] max-heat behavior
- [ ] anti-arbitrage matrix
- [ ] bulk pricing exploit
- [ ] detection reroll exploit
- [ ] no daily possession punishment
- [ ] faction hostility bounded
- [ ] moral spam bounded
- [ ] medical scarcity not trivialized
- [ ] equipment rarity preserved
- [ ] 30-day sim
- [ ] 120-day sim
- [ ] 180-day sim
- [ ] no-black-market case
- [ ] max-heat case
- [ ] multi-faction legality
- [ ] stock scarcity

## 155D — CI

- [ ] dealer/good/faction/location integrity
- [ ] black-market selftest
- [ ] headless
- [ ] source-scan authority gate
- [ ] anti-arbitrage gate
- [ ] information gate
- [ ] failure fixtures
- [ ] generated docs
- [ ] verify-fast

## 155E

- [ ] assassination service deferred unless quest/combat rails support
- [ ] forged documents deferred unless credential rail exists
- [ ] smuggling routes gated on inspection/route mechanics
- [ ] heist follow-on
- [ ] undercover follow-on
- [ ] criminal organization deferred
- [ ] dealer wars deferred
- [ ] dirty money rejected if no clean/dirty currency
- [ ] goods laundering only if provenance exists
- [ ] epilogue derives from existing history

---

# 44. Ship / No-Ship Gate

**SHIP** only if:

```text
canonical_item_identities == 1_per_good
AND shadow_inventories == 0
AND duplicate_market_authorities == 0
AND duplicate_faction_trust_authorities == 0
AND black_market_owned_bounty_state == 0
AND black_market_owned_quest_state == 0
AND black_market_owned_medical_effect_state == 0
AND black_market_owned_combat_effect_state == 0
AND transaction_detection_without_exposure_source == 0
AND faction_reaction_without_information_source == 0
AND unseeded_detection_rng == 0
AND duplicate_transaction_consequences == 0
AND dealer_trust_relationship_conflicts_without_adr == 0
AND legal_trade_dominated_in_reference_balance == false
AND risk_free_arbitrage_paths == 0
AND permanent_max_heat_lockout_unintended == false
AND old_save_black_market == pass
AND black_market_save_roundtrip == pass
AND black_market_quote_determinism == pass
AND black_market_detection_determinism == pass
AND black_market_transaction_atomicity == pass
AND black_market_information_gate == pass
AND black_market_anti_arbitrage == pass
AND black_market_30_day_balance == pass
AND black_market_120_day_balance == pass
AND black_market_180_day_balance == pass
AND black_market_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 45. Implementer Handoff

1. Audit the legal trade and item path before creating any black-market catalog.
2. Keep canonical item IDs; treat contraband as contextual restriction metadata.
3. Decide dealer identity and Plan-147 relationship reuse before inventing a dealer-trust meter.
4. Build underground access and stock on top of real market scarcity.
5. Use one pricing/quote path wherever possible.
6. Keep transaction transfer atomic through existing inventory/economy APIs.
7. Bind detection to actual transaction/search/report events, never random daily possession checks.
8. Scope heat to real enforcement jurisdictions if possible.
9. Reuse Plan 131 for rumor/public knowledge.
10. Apply faction consequences only when the faction actually learns/confirms the activity.
11. Route moral consequences through `MoralChoiceSystem`; illegal is not automatically immoral.
12. Route drug/medicine effects through consumption/medical systems.
13. Route weapon effects through equipment/combat systems.
14. Route bounty/embargo/confiscation/fine through their existing owners.
15. Do not invent dirty money unless the economy already distinguishes provenance.
16. Keep services such as assassination, forgery, smuggling routes, heists, and criminal organizations out of MVP unless supporting systems already exist.
17. Prefer hidden/dealer mode in the existing trade UI over a duplicate commerce panel.
18. Make every risk and known consequence legible before confirmation.
19. Add old-save, deterministic quote, deterministic detection, idempotence, and anti-arbitrage fixtures early.
20. Run 30/120/180-day legal-vs-underground balance simulations.
21. Close only when underground trade is attractive because it offers **access under risk**, not because it is a numerically superior replacement for the legal economy.

---

# 46. Final Outcome

When this plan is complete, ASHFALL has an underground economy without having two economies.

The same canonical goods can be legal in one settlement, restricted in another, and available through a hidden dealer elsewhere. The player reaches those dealers through real introductions, personal trust, rumors, quests, or faction context. Dealer stock is finite and derives from the same scarcity pressures that shape the legal market.

The transaction itself still uses the same inventory and economic transfer machinery. The difference is that the venue is hidden, the goods may be restricted, and the transaction carries exposure risk.

That risk is not a random punishment ticking in the background. A transaction can be observed. A checkpoint can search cargo if the game actually has one. An informant can report a deal through the existing information layer. A faction reacts only when the activity becomes known.

Heat therefore means scrutiny, not magic criminality.

Faction standing, embargoes, fines, confiscation, bounties, quests, addiction, moral consequence, and combat remain owned by the systems that already handle them. The underground market only provides the cause and the source identity needed to apply those consequences exactly once.

The legal economy remains strategically important because it is safer, broader, and more stable. The underground market earns its place by giving access to things the player cannot otherwise easily obtain, at higher risk, narrower stock, and less predictable terms.

The result is the right kind of post-collapse economy:

not a second shop with better prices,

but a hidden network where access itself has value and every profitable deal can create consequences elsewhere in the world.
