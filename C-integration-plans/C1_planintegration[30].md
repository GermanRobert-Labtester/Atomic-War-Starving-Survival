# C1 — Flagship Integration Plan [30]: Dynamic Quest Generation, State-Driven Opportunities & Canonical Quest Runtime

> **Output:** `C1_planintegration[30].md`
>
> **Source baseline:** Plan 171 — Dynamic Quest Generation System
>
> **Primary mission:** add deterministic, state-driven quest generation that turns real shelter shortages, survivor situations, faction conditions, discovered locations, hazards, seasonal pressures, and world events into authored quest instances without creating a second quest runtime or allowing procedural content to invent world facts.
>
> **Primary architectural rule:** the generator owns **candidate discovery, template matching, parameter binding, uniqueness, generation provenance, and opportunity scheduling**. The existing canonical quest runtime owns **availability, acceptance, assignment, objective progress, completion, failure, rewards, consequences, persistence of active quest lifecycle, and UI-facing quest status** wherever that runtime already provides those concepts.
>
> **Primary narrative rule:** procedural generation is not free-form story generation. Every generated quest must be assembled from authored templates, typed predicates, validated world entities, real objective/effect primitives, and deterministic parameter resolvers. No generated quest may ask for an impossible item, nonexistent location, dead survivor, invalid faction, unavailable action, or consequence with no canonical owner.
>
> **Primary replayability rule:** the target is **high recombination and state responsiveness**, not literally infinite unique authored content. Replayability comes from the same finite, quality-controlled templates resolving against different people, locations, shortages, relationships, hazards, faction states, route conditions, and campaign histories.
>
> **Mandatory execution order:** 171A quest-runtime/authority audit → 171B template schema + trigger facts + parameter domains → 171C deterministic candidate generation and feasibility filtering → 171D canonical quest instantiation/lifecycle adapters → 171E chaining/follow-up/history without procedural story drift → 171F UI, persistence, balance, anti-spam, reachability and CI → 171G advanced difficulty/rarity/customization only after the base generator proves quality.
>
> **Critical re-baseline rule:** before creating `DynamicQuestGenerator`, inspect the current shared quest runtime, `questline_master.json`, `moral_choice_quests.json`, `moral_choice_quest_stubs.json`, `CampaignConsequenceLedger`, expedition/discovery systems, faction standing, survivor relations, survivor availability, item/catalog authorities, location evolution, weather/radiation authorities, journal/history, save orchestration, and content-acceptance/reachability gates. The generator must output into those rails rather than duplicating them.
>
> **Critical source correction:** the source suggests checking triggers “each tick,” persisting a full second `DynamicQuestState`, using a universal 5-day cooldown/30-day expiry, generating rewards directly, and creating separate quest acceptance/completion/failure logic. Those are high-risk duplication points. The flagship replaces them with event/day-driven candidate evaluation, canonical quest-runtime instantiation, template-specific opportunity windows, source-attributed deterministic IDs, and reward/effect primitives resolved through existing systems.
>
> **Guardrails:** no second quest state machine; no duplicate active/completed quest list if canonical quest runtime already owns it; no generator-owned item rewards granted directly; no generator-owned faction standing mutation; no generator-owned morale mutation; no generator-owned expedition progress; no generated location IDs invented from text; no generated survivor refs to dead/missing/unavailable actors unless template explicitly permits them; no generated escort target without a real escort lifecycle; no defend quest without a real defense/encounter seam; no negotiate quest without a real faction/diplomacy action; no “resolve conflict” quest without a real conflict/choice authority; no free-form placeholder substitution as gameplay logic; no per-frame trigger scan; no `Guid.NewGuid`; no wall clock; no unseeded randomness; no reward inflation; no quest spam from one persistent shortage; no recursive chain explosion; no dynamic quest allowed to override authored mainline/story quest truth.

---

# 0. Mission

ASHFALL already has quests, but they are static.

The source baseline identifies:
- `questline_master.json`;
- `moral_choice_quests.json`;
- `moral_choice_quest_stubs.json`;
- fixed objectives;
- fixed rewards;
- fixed outcomes;
- no procedural generation code.

That means the campaign can simulate a changing world while the quest layer remains comparatively predetermined.

Current shape:

```text
AUTHORED QUEST FILE
      │
      ▼
STATIC QUEST
      │
      ├── fixed objective
      ├── fixed target
      ├── fixed reward
      └── fixed outcome
```

Target shape:

```text
REAL CAMPAIGN STATE
      │
      ├── shortage
      ├── injured survivor
      ├── faction relation
      ├── location discovery
      ├── weather/hazard
      ├── world consequence
      ├── survivor relationship
      └── route/world change
      │
      ▼
DynamicQuestGenerator
      │
      ├── collect typed trigger facts
      ├── query authored templates
      ├── enumerate valid parameter candidates
      ├── reject impossible combinations
      ├── rank novelty/relevance
      ├── choose deterministically
      └── emit canonical quest-definition instance
      │
      ▼
CANONICAL QUEST RUNTIME
      │
      ├── available
      ├── accepted
      ├── active
      ├── objective progress
      ├── complete / fail / expire
      ├── reward/effect resolution
      └── quest history
      │
      ▼
EXISTING DOMAIN AUTHORITIES
      │
      ├────────► ExpeditionSystem
      ├────────► Inventory/economy
      ├────────► Faction standing
      ├────────► SurvivorRelations
      ├────────► MoralChoice
      ├────────► LocationEvolution
      ├────────► Weather/radiation
      ├────────► CampaignConsequenceLedger
      └────────► Journal/archive
```

The generator should answer:

> Given the current world state, which authored quest templates are relevant, which real entities can fill their roles, and which valid quest opportunity should be offered now?

It should not answer:

> Has the expedition actually reached the destination?
> Did the player acquire the item?
> Should faction standing increase?
> How much morale changed?
> What exact story outcome occurred?

Those remain canonical quest/effect/domain responsibilities.

---

# 1. Source-Evidence Interpretation

## 1.1 Dynamic generation is genuinely absent

The source reports zero Core matches for:
- `DynamicQuest`;
- `ProceduralQuest`;
- `QuestGenerat`;
- `quest_generat`.

A generator is therefore a legitimate new orchestration component.

## 1.2 The quest runtime already exists

The generator must complement static quests, not become a parallel quest system.

The first implementation question is not:

> How do we build quest acceptance/completion/failure?

It is:

> What canonical quest-definition contract can generated instances feed?

## 1.3 Static catalogs remain valuable

`questline_master.json` and `moral_choice_quests.json` should remain:
- authored narrative;
- campaign spine;
- high-specificity content.

Dynamic quests should fill:
- state-responsive opportunities;
- side stories;
- logistical needs;
- local follow-ups;
- personal/faction situations.

Do not procedurally replace authored story arcs.

## 1.4 Empty `location_id` fields are a separate data-quality issue

The source notes 65 moral-choice quests with empty `location_id`.

Do not use dynamic generation to hide or “fix” unresolved static quest references.

Static content integrity remains its own concern.

## 1.5 Template generation needs feasibility, not just conditions

A condition can match while the quest remains impossible.

Example:

```text
supply shortage = medicine
```

does not mean:
- a valid medicine item exists;
- a reachable location stocks it;
- the player can travel there;
- the objective primitive can track retrieval.

A generated candidate needs a feasibility proof.

## 1.6 Quest types are capability contracts

Source types:
- fetch;
- escort;
- investigate;
- defend;
- negotiate;
- explore;
- resolve.

Each type can ship only if a real objective resolver exists.

## 1.7 Rewards must not be invented by the generator

The generator can select a reward profile.

The canonical reward/effect system resolves:
- item grants;
- standing;
- XP;
- morale;
- unlocks.

## 1.8 Quest chaining needs explicit authored continuation rules

A chain must not recursively synthesize arbitrary plot.

Use:
- follow-up template IDs/tags;
- state-triggered next opportunities;
- stable chain IDs.

---

# 2. Non-Negotiable Dynamic-Quest Invariants

## INV-171.1 — One quest lifecycle authority

Generated and static quests use the same runtime status model.

## INV-171.2 — Generator does not own active/completed truth

If canonical quest runtime persists active/completed quests, generator stores only generation-specific provenance/suppression state.

## INV-171.3 — Every generated parameter resolves to a real entity

No invented item/location/survivor/faction/threat/anomaly identifiers.

## INV-171.4 — Every generated objective maps to a canonical objective primitive

No text-only objective pretending to be trackable.

## INV-171.5 — Every generated reward maps to a canonical effect primitive

No direct reward mutation inside generator.

## INV-171.6 — Generation is deterministic under the campaign seed/state

## INV-171.7 — Candidate evaluation is event/day driven

No per-frame scan.

## INV-171.8 — Template conditions are typed

No arbitrary executable condition strings.

## INV-171.9 — Parameter bindings are typed

No gameplay logic in free-form placeholder text.

## INV-171.10 — Feasibility precedes selection

Impossible candidates never reach the quest board.

## INV-171.11 — Availability is not acceptance

Generated quest opportunity can exist without becoming active.

## INV-171.12 — Expiry is template-specific

No universal 30-day expiry unless intentionally chosen as a global default.

## INV-171.13 — Generation cadence is state-aware

No universal 5-day cadence as the only control.

## INV-171.14 — Persistent trigger conditions do not spam duplicate quests

A long shortage cannot emit the same fetch quest every generation cycle.

## INV-171.15 — Static/mainline quests have precedence

Dynamic content cannot duplicate or contradict authored campaign-critical objectives.

## INV-171.16 — Survivor state is snapshotted appropriately

Generated text can reference the survivor chosen at generation, but objective validity must define what happens if that survivor dies/leaves.

## INV-171.17 — Location knowledge is respected

Unrevealed locations are not exposed unless the quest explicitly performs discovery.

## INV-171.18 — Faction knowledge/standing is respected

Generated negotiation cannot reveal unknown factions or hidden standings.

## INV-171.19 — Rewards cannot exceed the value budget

## INV-171.20 — Chains are finite and cycle-free

## INV-171.21 — Old saves need no retroactive generated backlog

## INV-171.22 — UI displays why a quest exists only when that trigger information is player-visible

## INV-171.23 — No generated quest without a completion/failure test path

## INV-171.24 — Content quality beats volume

Twenty good templates are acceptable; one hundred combinatorially broken templates are not.

---

# 3. Definition of Done

Plan 171 closes only when:

- canonical quest runtime ownership is documented;
- generated quest instances reuse the same acceptance/progress/completion/failure/reward machinery;
- template schema is versioned and typed;
- trigger facts map to real authorities;
- every shipped quest type maps to real objective primitives;
- parameter resolvers return canonical IDs only;
- feasibility filters reject impossible combinations;
- location resolution respects discovery/reachability;
- survivor resolution respects alive/available/fitness/assignment constraints where relevant;
- faction resolution respects active/known faction state;
- item resolution respects canonical catalogs and objective meaning;
- threat/anomaly resolution references real active world state;
- generation is event/day-driven;
- stable candidate and quest IDs exist;
- duplicate/similar quest suppression exists;
- candidate ranking accounts for relevance, novelty, recent history, and saturation;
- generation uses seeded deterministic selection only after deterministic candidate construction;
- dynamic opportunities enter canonical quest runtime;
- generator does not duplicate active/completed quest lists;
- expiry/cooldown are template/profile driven;
- reward profiles are bounded and resolved by canonical effect authority;
- failure consequences are authored and canonical;
- chains use explicit follow-up rules and bounded depth;
- dynamic quests complement static quests;
- no mainline conflict/duplication occurs;
- old saves get empty generation history/suppression state;
- save/load does not regenerate already-issued quests;
- headless behavior passes;
- 20 templates are accepted only when every parameter binding is reachable;
- 30/120/180-day simulations show useful variety without quest-board flooding;
- `--dynamic-quest-selftest` or equivalent exists;
- template/catalog/objective/effect references validate;
- generated quest corpus snapshots/golden manifests are deterministic;
- unreachable combinations are surfaced by CI rather than silently skipped forever.

---

# 4. Phase P0 — Quest Runtime & Content Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
quest runtime types
quest definition DTO
quest instance/runtime DTO
quest status enum
accept API
assignment API
objective tracker
reward/effect resolver
failure/expiry support
quest history/log
quest UI model
quest save section
questline_master count
moral_choice_quests count
moral_choice_quest_stubs count
CampaignConsequenceLedger APIs
ExpeditionSystem APIs
LocationEvolutionSystem APIs
world graph/discovery APIs
FactionBranchCoordinator / stance APIs
SurvivorRelationsSystem APIs
survivor lifecycle/availability APIs
item catalog/inventory APIs
weather/radiation APIs
journal/event APIs
```

## P0.2 Build quest authority matrix

Create:

`docs/quests/DYNAMIC_QUEST_AUTHORITY_MATRIX.md`

Columns:

```text
fact
current authority
read API
write API
persisted?
generator role
status
```

Rows:
- quest definition;
- quest availability;
- acceptance;
- active status;
- assigned survivor;
- objective state;
- item acquisition;
- expedition arrival;
- faction outcome;
- relationship outcome;
- reward;
- failure;
- expiry;
- history;
- generation provenance;
- suppression/cooldown;
- chain state.

## P0.3 Audit generated-definition insertion seam

Determine whether runtime can accept:
- in-memory definition;
- generated catalog overlay;
- runtime quest factory.

Preferred:
- canonical factory/registry entry with generated definition ID.

## P0.4 Audit objective primitive vocabulary

Inventory actual objective types:

```text
acquire_item
deliver_item
visit_location
discover_location
survivor_present
protect_entity
resolve_encounter
talk_to_npc
reach_faction_state
set_world_flag
etc.
```

Do not assume source quest types are supported.

## P0.5 Audit reward/effect primitive vocabulary

Inventory:
- item grant;
- standing;
- relationship;
- XP;
- morale;
- flag;
- journal;
- location discovery.

## P0.6 Audit static quest conflict rules

Find:
- unique quest IDs;
- prerequisites;
- mutually exclusive arcs;
- reserved locations/NPCs.

## P0.7 Baseline proof

Demonstrate:
- no procedural quest generated from a changed world state;
- static quest behavior remains canonical.

---

# TASK 171A — Template Schema, Trigger Facts & Parameter Domains

# 171A.0 Goal

Define an authored template language that can express dynamic opportunities safely without becoming a scripting language.

---

# 171A-T — Template Contract

## 171A.T1 Proposed file

`Assets/StreamingAssets/Data/quest_templates.json`

## 171A.T2 Template DTO

Suggested:

```text
template_id
quest_family
title_key
description_key
trigger_predicates[]
parameter_slots[]
objective_specs[]
reward_profile_id
failure_profile_id optional
availability_window
cooldown_profile
uniqueness_key
chain_hooks[]
difficulty_profile
tags[]
```

## 171A.T3 Separate text from logic

Text uses localization keys/tokens.

Logic uses typed fields.

## 171A.T4 Quest family

Candidate:

```text
resource_recovery
escort
investigation
defense
diplomacy
exploration
social_resolution
```

Use source labels as UI tags if useful.

## 171A.T5 Family does not define runtime objective

Template objective specs do.

## 171A.T6 Trigger predicates

Typed.

Example:

```text
resource_shortage
survivor_state
faction_standing_band
location_state
world_event_present
day_range
weather_state
radiation_state
relationship_state
```

Only real authorities.

## 171A.T7 Generic comparison

Allow:

```text
lt
lte
eq
gte
gt
contains
not_contains
```

but parameter schema remains typed.

## 171A.T8 No arbitrary property path

Do not allow:
`"Assets.Player.foo.bar"`.

## 171A.T9 Trigger semantics

Trigger means:
- candidate may be relevant.

It does not guarantee feasibility.

## 171A.T10 Multiple predicates

Support:
- all;
- any
in a bounded explicit structure.

Avoid a full expression language if unnecessary.

## 171A.T11 Trigger source attribution

Record which real fact matched.

## 171A.T12 Event-trigger optimization

Templates should register interest in:
- inventory changed;
- survivor state changed;
- faction stance changed;
- location discovered;
- weather changed;
- world event;
- day boundary.

## 171A.T13 No full trigger scan every frame

---

# 171A-P — Parameter Slot Contract

## 171A.P1 Typed parameter slot

Suggested:

```text
slot_id
domain
selection_constraints
required
bind_mode
```

## 171A.P2 Domains

Candidate:

```text
item
location
survivor
faction
npc
threat
world_event
affliction
route
resource_category
```

Only implemented domains.

## 171A.P3 Binding result

Canonical ID plus display token.

## 171A.P4 No free text as entity identity

## 171A.P5 Item resolver

Constraints can include:
- category;
- shortage relevance;
- obtainable;
- not already excessive;
- not quest-reserved.

## 171A.P6 Location resolver

Constraints:
- discovered;
- reachable;
- has relevant content;
- not depleted/closed unless objective handles that.

## 171A.P7 Discovery quest location

Can bind:
- unrevealed candidate region/node
only if exploration system can expose a **coarse** objective without leaking hidden identity.

## 171A.P8 Survivor resolver

Constraints:
- alive;
- present/available;
- trait/tag;
- fitness;
- relationship;
- not already hard-reserved.

## 171A.P9 Faction resolver

Constraints:
- known;
- active;
- standing band;
- relationship;
- not destroyed/removed.

## 171A.P10 Threat resolver

Must reference:
- active known threat/event.

No invented “raiders” string.

## 171A.P11 Anomaly resolver

Use LocationEvolution/world event data.

If no anomaly authority:
- defer anomaly slot.

## 171A.P12 Resolver diagnostics

Every rejected candidate reports:
- reason code.

---

# 171A-O — Objective Specification

## 171A.O1 Objective specs reference canonical objective primitives

Example:

```text
type: acquire_item
item_slot: medicine
quantity_profile: moderate
```

## 171A.O2 Parameterized text is presentation

Gameplay objective fields are already bound.

## 171A.O3 Fetch

Ship only if:
- acquire/deliver item primitive exists.

## 171A.O4 Escort

Ship only if:
- escort entity lifecycle/path objective exists.

Otherwise DEFER.

## 171A.O5 Investigate

Can use:
- visit location;
- resolve encounter;
- inspect flag
if supported.

## 171A.O6 Defend

Requires:
- defend encounter/location objective.

No fake timer quest.

## 171A.O7 Negotiate

Requires:
- dialogue/diplomacy/choice resolution primitive.

## 171A.O8 Explore

Can use:
- discover location;
- visit route/node.

## 171A.O9 Resolve

Requires:
- current survivor conflict/event ID;
- canonical resolution effect.

## 171A.O10 Objective cardinality

MVP:
- 1–3 objectives.

Avoid arbitrary complex DAG generation.

## 171A.O11 Optional objective

Only if runtime supports.

---

# 171A-R — Reward / Failure Profiles

## 171A.R1 Reward profile catalog

Use canonical effect specs.

## 171A.R2 Generator selects profile

It does not grant.

## 171A.R3 Item rewards

Bound to:
- difficulty;
- scarcity;
- quest cost.

## 171A.R4 Faction reward

Only if relevant faction and authored effect.

## 171A.R5 Morale reward

Only if canonical morale effect exists and semantically justified.

## 171A.R6 XP

Use canonical skill/XP system.

## 171A.R7 Unique reward

Requires real unique item/definition.

Do not procedurally invent.

## 171A.R8 Failure profile

May include:
- no consequence;
- standing loss;
- relationship loss;
- lost opportunity;
- world flag
through canonical effect primitives.

## 171A.R9 No automatic failure punishment

Failure must be authored per template.

## 171A.R10 Reward-value budget

Compute estimated value for integrity/balance.

### 171A DoD

Dynamic quest templates express relevance, parameter domains, canonical objectives, and canonical reward/failure effects without embedding a second quest scripting/runtime language.

---

# TASK 171B — Trigger Evaluation, Candidate Construction & Feasibility

# 171B.0 Goal

Generate only relevant and executable quest candidates.

---

# 171B-E — Event/Day-Driven Evaluation

## 171B.E1 Trigger subscriptions

On:
- day boundary;
- inventory threshold crossing;
- survivor state event;
- faction state event;
- location discovery/evolution;
- weather/radiation transition;
- campaign consequence event.

## 171B.E2 Coalesce events

Many state changes in one transaction/day:
- one generation evaluation pass.

## 171B.E3 Debounce

No duplicate generation from repeated equivalent events.

## 171B.E4 Daily fallback

One day-boundary evaluation can catch missed event seams.

## 171B.E5 No per-frame tick

---

# 171B-C — Candidate Construction

## 171B.C1 Match templates

Use indexed trigger categories.

## 171B.C2 Enumerate parameter domains

Deterministically sorted IDs.

## 171B.C3 Constraint filtering

Apply slot constraints.

## 171B.C4 Candidate cartesian explosion

Do not enumerate every combination if domains large.

Use staged bounded selection.

## 171B.C5 Constraint-first ordering

Resolve most restrictive slots first.

## 171B.C6 Candidate cap per template

Configurable.

## 171B.C7 Deterministic sampling

If too many:
- stable seeded sampling.

## 171B.C8 Build candidate signature

Suggested:

```text
template_id
bound_parameter_ids
trigger_source_ids
```

## 171B.C9 Stable candidate ID

Hash canonical ordered signature.

## 171B.C10 Generated quest ID

Derived from:
- candidate ID;
- generation epoch/sequence
if repeatable after cooldown.

## 171B.C11 No GUID

---

# 171B-F — Feasibility Filter

## 171B.F1 Objective support

Every objective primitive registered.

## 171B.F2 Entity existence

All refs valid.

## 171B.F3 Entity lifecycle

Survivor/NPC alive and eligible.

## 171B.F4 Location reachability

Route exists.

## 171B.F5 Location knowledge

No forbidden leak.

## 171B.F6 Item attainability

At least one valid source/path if the quest expects acquisition.

Do not generate:
- retrieve impossible item.

## 171B.F7 Resource paradox

If shortage quest asks player to spend more of the same scarce resource than reward/goal makes reasonable:
- reject.

## 171B.F8 Faction viability

Faction active and interaction path exists.

## 171B.F9 Diplomacy feasibility

Negotiate only if:
- two real factions/parties;
- dialogue/quest effect can resolve.

## 171B.F10 Escort feasibility

Target can travel.

## 171B.F11 Defend feasibility

Threat/location defense event can be instantiated.

## 171B.F12 Social-resolution feasibility

Conflict ID exists and unresolved.

## 171B.F13 Static quest reservations

Do not bind:
- NPC;
- location;
- unique item
reserved by conflicting mainline quest unless coexistence allowed.

## 171B.F14 Active dynamic conflict

Do not generate two quests requiring incompatible exclusive state.

## 171B.F15 Deadline feasibility

Travel/action can realistically complete before expiry.

## 171B.F16 Survivor assignment feasibility

If required:
- at least one valid survivor.

## 171B.F17 Reward validity

All effects valid.

## 171B.F18 Failure validity

All failure effects valid.

## 171B.F19 Content text tokens

All required tokens resolved.

## 171B.F20 Feasibility reason log

For testing/diagnostics.

---

# 171B-N — Novelty & Saturation

## 171B.N1 Recent template history

Track generation history compactly.

## 171B.N2 Similarity key

Template can define:

```text
template
target
location
item category
survivor
```

## 171B.N3 Duplicate active

Hard reject.

## 171B.N4 Recently completed

Penalty/cooldown.

## 171B.N5 Persistent shortage

One quest may remain available/active.

Do not regenerate until:
- state meaningfully changes;
- prior quest resolves/expires;
- cooldown permits.

## 171B.N6 Repeated location

Avoid same place too often.

## 171B.N7 Survivor spotlight fairness

Avoid always selecting same survivor when alternatives exist.

## 171B.N8 Faction variety

Bound repeated same-faction opportunities.

## 171B.N9 Novelty is a ranking factor, not truth

Do not pick nonsense just because it is different.

---

# 171B-R — Candidate Ranking

## 171B.R1 Score factors

Potential:

```text
state urgency
template relevance
feasibility confidence
novelty
world significance
player knowledge
recent saturation
difficulty fit
```

## 171B.R2 No opaque magic if avoidable

Expose diagnostics.

## 171B.R3 Urgency

Supply shortage can rank higher than flavor opportunity.

## 171B.R4 Campaign load

If quest board already full:
- suppress low-value candidates.

## 171B.R5 Mainline precedence

Static critical quest pressure can reduce dynamic generation.

## 171B.R6 Deterministic weighted selection

Use `ISeededRng` only among valid similarly ranked candidates.

## 171B.R7 Seed source

Campaign seed + day + generation sequence.

## 171B.R8 Stable ordering before RNG

Sort candidate IDs.

### 171B DoD

The generator evaluates on meaningful state changes, constructs bounded candidates, proves feasibility, suppresses duplicates, ranks relevance, and only then selects deterministically.

---

# TASK 171C — Canonical Quest Instantiation & Lifecycle Integration

# 171C.0 Goal

Turn a selected candidate into a normal quest that the existing runtime can own.

## 171C.1 Generated definition factory

Create:

```text
DynamicQuestDefinitionFactory
```

or equivalent.

Input:
- template;
- bindings;
- generation context.

Output:
- canonical quest definition.

## 171C.2 Canonical quest ID

Generated definition uses stable generated quest ID.

## 171C.3 Provenance metadata

Attach:

```text
source = dynamic
template_id
candidate_id
generation_day
trigger_source_ids
bindings
```

## 171C.4 No separate `DynamicQuest.status`

If canonical quest instance already stores status.

## 171C.5 No separate assigned survivor field

If canonical quest runtime owns assignment.

## 171C.6 No separate objective tracker

## 171C.7 No separate completion checker

## 171C.8 No separate reward grant path

## 171C.9 Available state

Register as available quest.

## 171C.10 Accept

Use same API as static quests.

## 171C.11 Assignment

Use same API.

## 171C.12 Active

Same runtime.

## 171C.13 Completion

Canonical objective resolver.

## 171C.14 Failure

Canonical runtime.

## 171C.15 Expiry

Use canonical expiry if available.

If runtime lacks it:
- add generic availability expiry support usable by static quests too.

Do not hide expiry in generator.

## 171C.16 Generated quest removal

Expired/unaccepted quest removed from active availability but retained in compact history/suppression.

## 171C.17 Reward resolution

Canonical effect system.

## 171C.18 Faction standing

Canonical effect.

## 171C.19 Survivor relation

Canonical effect.

## 171C.20 Location evolution

Canonical effect.

## 171C.21 Expedition objective

Canonical expedition state/event.

## 171C.22 Journal

Quest runtime/journal adapter.

## 171C.23 Notification

Quest availability event.

## 171C.24 No duplicate event family

Generated quest uses semantic quest events with source metadata.

## 171C.25 Static/dynamic UI parity

Same quest list/detail surface, with:
- dynamic opportunity badge/filter
if useful.

## 171C.26 No mandatory separate quest board

Source proposes a quest board.

Audit existing quest UI first.

Preferred:
- same quest journal/board;
- filter “Opportunities”.

## 171C.27 Active quest cap

Use existing quest capacity if any.

If new dynamic-only cap:
- cap **available dynamic opportunities**, not total active quest runtime unless design says so.

## 171C.28 Source “max 3 active”

Treat as tuning candidate.

## 171C.29 Availability cap

Suggested:
- 2–4 dynamic opportunities.

## 171C.30 Acceptance cap

Could remain unrestricted or use global quest capacity.

## 171C.31 Expiry messaging

Show exact campaign day/deadline.

## 171C.32 Assignment death

Canonical quest failure/retarget policy.

Generator does not silently rebind after acceptance unless template says so.

## 171C.33 Pre-accept invalidation

If world state changes before acceptance:
- revalidate.

## 171C.34 Post-accept world change

Quest runtime handles objective/failure conditions.

## 171C.35 No generated quest mutation after acceptance

Bindings freeze except explicitly dynamic objective rules.

### 171C DoD

A generated opportunity becomes an ordinary canonical quest instance and thereafter follows the exact same lifecycle, effect, save, journal, and UI rules as authored quests.

---

# TASK 171D — Quest Families & Real-System Adapters

# 171D.0 Goal

Ship only quest families whose objectives can be resolved by real systems.

---

# 171D-F — Fetch / Resource Recovery

## 171D.F1 Preconditions

Canonical:
- item IDs;
- inventory;
- item acquisition/delivery objective.

## 171D.F2 Trigger

Real shortage.

## 171D.F3 Target item

Relevant shortage category.

## 171D.F4 Source location

Reachable, known or appropriately discoverable.

## 171D.F5 Quantity

Difficulty/resource budget.

## 171D.F6 Completion

Canonical inventory/delivery event.

## 171D.F7 Avoid trivial self-completion

If player already owns enough:
- candidate rejected or objective requires delivery/reserve.

## 171D.F8 Reward

Not just same item at huge profit.

---

# 171D-E — Escort

## 171D.E1 Hard gate

Only if escort/travel lifecycle exists.

## 171D.E2 Target

Canonical survivor/NPC.

## 171D.E3 Destination

Reachable.

## 171D.E4 Availability

Target cannot be:
- dead;
- already away;
- quest-reserved.

## 171D.E5 Failure

Target death/lost/timeout through real events.

## 171D.E6 If no escort authority

DEFER family.

---

# 171D-I — Investigate

## 171D.I1 Good MVP family

Can map to:
- visit;
- inspect;
- resolve encounter;
- report.

## 171D.I2 Trigger

Location evolution/anomaly/world event.

## 171D.I3 No invented anomaly

## 171D.I4 Outcome

Canonical location/event state.

---

# 171D-D — Defend

## 171D.D1 Hard gate

Requires:
- attack/threat;
- defense encounter;
- location ownership.

## 171D.D2 No fake “wait N days” defend quest

unless defense authority genuinely resolves that window.

## 171D.D3 Threat ID

Canonical.

## 171D.D4 Failure

Canonical defense loss/outcome.

---

# 171D-N — Negotiate

## 171D.N1 Hard gate

Requires:
- faction/dialogue/diplomacy choice.

## 171D.N2 Two-faction disputes

Only if world state actually represents a dispute/conflict.

## 171D.N3 No invented dispute

## 171D.N4 Resolution

Canonical dialogue/choice/effect.

## 171D.N5 If no diplomacy action

DEFER.

---

# 171D-X — Explore

## 171D.X1 Strong MVP family

Use:
- route;
- map node;
- discovery.

## 171D.X2 Hidden location

Quest can identify:
- region/clue
without leaking exact hidden target if game uses knowledge rungs.

## 171D.X3 Completion

Discovery event.

## 171D.X4 Expedition integration

Canonical.

---

# 171D-S — Social Resolve

## 171D.S1 Trigger

Existing survivor conflict/friction/event.

## 171D.S2 No invented conflict

## 171D.S3 Objective

Use:
- dialogue;
- mediation choice;
- commitment
if supported.

## 171D.S4 Relationship effects

Canonical SurvivorRelations.

## 171D.S5 If no resolution seam

DEFER family.

### 171D DoD

Every shipped quest family has a real trigger, real parameter domain, real objective completion signal, and real failure/reward path.

---

# TASK 171E — Chaining, Follow-Ups & Story Coherence

# 171E.0 Goal

Allow dynamic quests to create bounded story continuity without generating incoherent procedural plot.

## 171E.1 Chain ID

Stable:

```text
chain_id
root_quest_id
```

## 171E.2 Follow-up specification

Template can declare:

```text
on_complete_followup_tags[]
on_fail_followup_tags[]
```

or explicit template IDs.

## 171E.3 No arbitrary recursive generation

Follow-up enters normal candidate pipeline.

## 171E.4 Follow-up conditions

Must still be true.

## 171E.5 Bind continuity

Follow-up may inherit:
- faction;
- survivor;
- location;
- item;
- source event
through explicit binding rules.

## 171E.6 Frozen continuity

Inherited bindings remain canonical IDs.

## 171E.7 Chain max depth

Hard cap.

Suggested:
- 3–5.

## 171E.8 Cycle detection

Graph validation.

## 171E.9 No A→B→A loops

CI fail.

## 171E.10 Completion branch

Can enable:
- escalation;
- aftermath;
- reward collection;
- relationship follow-up.

## 171E.11 Failure branch

Can enable:
- repair;
- rescue;
- consequence follow-up.

## 171E.12 Failure is not always content generator

Avoid rewarding failure with endless new quests.

## 171E.13 Rare templates

Use:
- specific high-salience state;
- not random rarity alone.

## 171E.14 Flavor variation

Template may have several localization variants.

Selection seeded.

## 171E.15 No runtime prose synthesis requirement

## 171E.16 Survivor personal stories

Use real:
- traits;
- memory;
- relationships;
- afflictions
as predicates/bindings.

## 171E.17 Faction story continuity

Use real:
- standing;
- incidents;
- commitments;
- disputes.

## 171E.18 Location story continuity

Use:
- location evolution/scars/history.

## 171E.19 Consequence ledger

Generated quest outcome writes canonical semantic consequence/event.

## 171E.20 Archive/epilogue

Only significant dynamic quests become historical milestones.

## 171E.21 “Quest completion triggers generation”

Do not directly spawn inside completion handler.

Emit state/event and let candidate pipeline evaluate.

This preserves one generation seam.

## 171E.22 Chain observability

Diagnostics show:
- parent;
- trigger;
- inherited bindings;
- chosen follow-up.

### 171E DoD

Dynamic quest chains remain finite, source-attributed, state-valid, and narratively coherent because follow-ups are authored templates that re-enter the same feasibility pipeline.

---

# TASK 171F — UI, Persistence, Anti-Spam, Balance, Reachability & CI

# 171F.0 Goal

Make dynamic opportunities legible, save-safe, deterministic, varied, and impossible to farm or flood.

---

# 171F-U — UI

## 171F.U1 Reuse canonical quest surface

Preferred:
- one quest list;
- source badge/filter.

## 171F.U2 Generated opportunity card

Show:
- title;
- type/family;
- why it matters;
- location;
- faction;
- deadline;
- difficulty;
- rewards;
- assignment requirement.

## 171F.U3 Trigger explanation

Player-facing only if information is known.

Examples:
- “Medicine reserves are critically low.”
- “A newly discovered site may contain supplies.”

Do not expose hidden world state.

## 171F.U4 Dynamic badge

Optional:
- “Opportunity”.

Avoid technical “Procedurally generated.”

## 171F.U5 Quest detail

Same canonical objective/reward UI.

## 171F.U6 Difficulty

Use estimated domain difficulty, not arbitrary 1–5 only.

1–5 can be presentation band.

## 171F.U7 Reward preview

Actual canonical effect data.

## 171F.U8 Expiry

Absolute day/time.

## 171F.U9 Assignment preview

Show eligible survivors.

## 171F.U10 Impossible survivor

Disabled with reason.

## 171F.U11 Filters

Use existing filter system.

## 171F.U12 Notification

Bounded.

No notification every day for same unmet trigger.

## 171F.U13 Journal

Availability not necessarily journaled.

Accepted/completed/failed significant quests are.

## 171F.U14 Tutorial

Only current tutorial framework.

## 171F.U15 Accessibility

No color-only rarity/difficulty/status.

---

# 171F-P — Persistence

## 171F.P1 Generator-specific state

Persist only if not canonical elsewhere:

```text
schema_version
generation_sequence
candidate suppression/history
last_generation_by_template/signature
chain provenance
generated definition snapshots if runtime needs
```

## 171F.P2 Do not duplicate canonical lifecycle

No second:
- active;
- complete;
- failed
lists.

## 171F.P3 Generated definition snapshot

If templates may change between versions, persist the resolved canonical definition/bindings required to finish an accepted quest.

## 171F.P4 Available unaccepted quest

Persist enough to avoid reroll on reload.

## 171F.P5 Expired history

Compact signature/date.

## 171F.P6 Old save

Empty generation history.

## 171F.P7 No retroactive backlog

Old campaign does not suddenly generate dozens of quests immediately.

Use warm-up/cap.

## 171F.P8 Save/reload

Same available opportunities.

## 171F.P9 No reroll exploit

Reload cannot produce a different quest from same generation epoch.

## 171F.P10 Template deletion

Accepted generated quest remains finishable through snapshot/version migration.

---

# 171F-A — Anti-Spam & Exploit Prevention

## 171F.A1 Availability cap

Configurable.

## 171F.A2 Event coalescing

One generation pass per change batch/day.

## 171F.A3 Trigger hysteresis

Threshold triggers can require:
- crossing;
- recovery before re-trigger.

## 171F.A4 Signature cooldown

Per:
- template + bound parameters.

## 171F.A5 Family cooldown

Avoid endless fetch quests.

## 171F.A6 Global cadence

May be a soft budget rather than fixed 5-day timer.

## 171F.A7 Urgent exception

Major world event can bypass soft cadence.

## 171F.A8 Static quest pressure

Dynamic budget shrinks when many authored quests are active.

## 171F.A9 Reward farming

Same state cannot repeatedly generate profitable quest.

## 171F.A10 Self-completion

Reject quests already complete at generation.

## 171F.A11 Expiry abuse

Declining/letting expire does not reset signature instantly.

## 171F.A12 Chain farming

One chain source cannot loop.

## 171F.A13 Survivor death farming

Death event does not generate repeated memorial/recovery quests without uniqueness.

## 171F.A14 Location discovery farming

One location discovery triggers follow-up at most according to template uniqueness.

---

# 171F-B — Difficulty & Reward Balance

## 171F.B1 Difficulty inputs

Use real:
- travel distance;
- hazard;
- item rarity;
- faction hostility;
- objective count;
- survivor constraint;
- deadline.

## 171F.B2 No generic day-based difficulty growth by default

Campaign state already becomes harder/different.

## 171F.B3 Difficulty bands

Presentation:
- 1–5.

## 171F.B4 Reward budget

Correlates with:
- cost;
- risk;
- opportunity.

## 171F.B5 Scarcity feedback

Do not reward scarce resource so generously that shortage quest trivializes shortage.

## 171F.B6 Faction reward bounds

No easy standing farm.

## 171F.B7 Morale reward bounds

No quest-board morale farm.

## 171F.B8 XP bounds

No skill farm.

## 171F.B9 Rare rewards

Only real authored unique rewards.

## 171F.B10 Failure penalty

Proportionate.

---

# 171F-V — Variety & Quality

## 171F.V1 Variety metric

Track:
- family;
- template;
- target;
- location;
- survivor;
- faction.

## 171F.V2 Formulaic-text risk

Use authored variant text.

## 171F.V3 Same template repetition

Bound.

## 171F.V4 Invalid pair combinations

Template constraints.

## 171F.V5 Tonal consistency

Dynamic text must match ASHFALL.

## 171F.V6 No omniscient narration

Use known facts.

## 171F.V7 Concrete nouns

Resolved real:
- people;
- places;
- items.

## 171F.V8 No impossible urgency

Deadline based on travel/action feasibility.

---

# 171F-S — Long-Horizon Simulations

## 171F.S1 30-day baseline

Track:
- opportunities generated;
- accepted;
- expired;
- duplicate suppression;
- family distribution.

## 171F.S2 120-day normal campaign

Track:
- unique templates;
- repeated signatures;
- reward value;
- static vs dynamic quest load.

## 171F.S3 180-day stress

High:
- shortages;
- faction changes;
- hazards.

Assert:
- board not flooded;
- candidates remain feasible;
- no reward inflation.

## 171F.S4 No-trigger scenario

No dynamic quests.

## 171F.S5 Many-trigger scenario

Ranking/cap selects bounded set.

## 171F.S6 Persistent-shortage scenario

One/small bounded relevant opportunity, not repeated spam.

## 171F.S7 All-survivors-unavailable scenario

Assignment-required templates rejected.

## 171F.S8 No-reachable-location scenario

Travel templates rejected.

## 171F.S9 Mainline-heavy scenario

Dynamic content yields priority.

## 171F.S10 Long-run 400-day content variety

Measure:
- template repetition;
- chain frequency;
- state bytes;
- generated count.

---

# 171F-T — Testing & CI

## 171F.T1 Template integrity

Validate:
- IDs;
- trigger types;
- parameter domains;
- objective primitives;
- effect profiles;
- localization.

## 171F.T2 Resolver integrity

Every domain has:
- deterministic ordering;
- null/empty behavior.

## 171F.T3 Feasibility tests

Deliberately impossible candidates rejected.

## 171F.T4 Golden generation corpus

For fixed world-state fixtures, snapshot:
- candidate IDs;
- selected quest IDs;
- bindings;
- difficulty;
- reward profile.

## 171F.T5 Determinism fingerprint

Same fixture/seed:
- byte-equivalent canonical generation manifest.

## 171F.T6 Selftest

Create:

```text
--dynamic-quest-selftest
```

## 171F.T7 Selftest scenarios

At least:
1. no trigger;
2. supply shortage;
3. faction standing change;
4. survivor trait/state;
5. location discovery;
6. weather/radiation event;
7. impossible candidate rejection;
8. duplicate suppression;
9. canonical acceptance;
10. objective completion;
11. failure;
12. expiry;
13. chain;
14. old save;
15. reload no reroll;
16. headless.

## 171F.T8 Source-scan authority gate

Detect:
- duplicate quest status state machine;
- direct inventory grant;
- direct faction standing write;
- direct morale write;
- direct expedition progress mutation;
- per-frame trigger scan.

## 171F.T9 Content acceptance

Every shipped template:
- GENERATED in at least one fixture;
- instantiated;
- reaches objective/effect path.

## 171F.T10 Reachability

Produce generated matrix:

```text
template
fixture
bindings
result
```

## 171F.T11 Dead-template gate

No template that can never generate.

## 171F.T12 Impossible-combination rate

Track candidate rejection reasons.

High rejection rate:
- schema/template quality issue.

## 171F.T13 Performance benchmark

Generation pass:
- bounded candidate count;
- no combinatorial explosion.

## 171F.T14 Allocation benchmark

No excessive allocations in daily generation.

## 171F.T15 Generated docs

Create:
- `DYNAMIC_QUEST_ARCHITECTURE.md`;
- `DYNAMIC_QUEST_AUTHORITY_MATRIX.md`;
- `DYNAMIC_QUEST_TEMPLATE_MATRIX.md`;
- `DYNAMIC_QUEST_OBJECTIVE_COMPATIBILITY.md`;
- `DYNAMIC_QUEST_GENERATION_GOLDENS.md`;
- `DYNAMIC_QUEST_BALANCE_REPORT.md`.

### 171F DoD

Dynamic opportunities are deterministic, bounded, varied, feasible, save-stable, and fully exercised through the canonical quest runtime.

---

# TASK 171G — Advanced Difficulty, Rarity, Player Influence & Legacy: Explicit Follow-On

# 171G.0 Goal

Prevent the generator from expanding before basic quality and feasibility are proven.

## 171G.1 Difficulty scaling

Can be added after:
- difficulty model;
- reward budget;
- simulation telemetry
are stable.

## 171G.2 Quest rarity

Rarity may be:
- template availability weight;
- special-state threshold.

Do not make rarity a hidden quality label.

## 171G.3 Legendary dynamic quests

Prefer authored rare templates.

Not random inflated rewards.

## 171G.4 Player customization

Could allow:
- choosing among offered targets;
- choosing reward category
only if quest design supports.

Do not let player rewrite objectives arbitrarily.

## 171G.5 Quest trading/sharing

Default:
- DEFER.

Requires:
- settlement/quest ownership;
- information transfer;
- reward ownership.

## 171G.6 Dynamic quest legacy

Archive/epilogue can remember:
- high-impact generated quests.

No separate legacy system.

## 171G.7 Procedural prose generation

Default:
- DEFER/REJECT for core determinism.

Authored variants are sufficient.

## 171G.8 LLM-generated quest content

Not a runtime dependency.

## 171G.9 User-authored templates/modding

Follow Plan-47 mod/content-pack contract if later supported.

### 171G DoD

Advanced variety features remain optional extensions to a deterministic authored-template generator rather than destabilizing its core contracts.

---

# 5. Dynamic Quest Generation State Model

```text
WORLD FACT CHANGES
      │
      ▼
TRIGGER EVALUATION
      │
      ▼
TEMPLATE MATCH
      │
      ▼
PARAMETER BINDING
      │
      ▼
FEASIBILITY FILTER
      │
      ├── rejected
      └── valid candidate
             │
             ▼
       NOVELTY / PRIORITY RANK
             │
             ▼
       DETERMINISTIC SELECT
             │
             ▼
       CANONICAL QUEST INSTANCE
             │
             ▼
       NORMAL QUEST RUNTIME
```

Generation stops owning the quest once the canonical runtime accepts the instance.

---

# 6. Static vs Dynamic Quest Contract

Static quests are best for:

```text
campaign spine
named authored arcs
high-specificity moral choices
major endings
deep bespoke narrative
```

Dynamic quests are best for:

```text
shortages
local opportunities
state follow-ups
personal situations
faction conditions
world changes
repeatable campaign texture
```

Dynamic content complements rather than replaces authored quests.

---

# 7. Trigger Contract

A trigger is a typed observation:

```text
resource_shortage(medicine)
faction_standing_changed(garrison)
location_discovered(node_12)
survivor_affliction_changed(survivor_4)
weather_changed(storm)
```

No free-form eval string.

---

# 8. Parameter Contract

Bindings are canonical identities:

```text
item_id
location_id
survivor_id
faction_id
event_id
```

Text is rendered from those IDs.

---

# 9. Feasibility Contract

A candidate is valid only if:

```text
all entities exist
all objectives supported
all effects supported
path/action exists
deadline feasible
knowledge constraints valid
no reserved conflict
```

---

# 10. Objective Contract

Dynamic generator defines:
- bound objective specs.

Quest runtime tracks:
- progress.

---

# 11. Reward Contract

Generator selects:
- reward profile.

Effect resolver applies:
- reward.

No direct item/standing/morale writes.

---

# 12. Difficulty Contract

Difficulty is **derived** from real task cost/risk.

Presentation band:
- 1–5.

Do not hardcode by template alone.

---

# 13. Availability Contract

Generated opportunity:
- available;
- not active.

Acceptance occurs through canonical quest API.

---

# 14. Expiry Contract

Expiry is:
- availability-window behavior.

After acceptance, active quest deadlines/failure rules are canonical quest behavior.

---

# 15. Assignment Contract

If survivor assignment exists:
- canonical quest/expedition/duty rules own it.

Generator only confirms eligible candidates before offer.

---

# 16. Location-Knowledge Contract

Generated text cannot reveal:
- precise hidden location;
- hazard;
- faction control
beyond current player knowledge.

Exploration quests may use:
- clue region;
- unknown node objective
only through real discovery mechanics.

---

# 17. Survivor-Lifecycle Contract

Before acceptance:
- unavailable survivor can invalidate offer.

After acceptance:
- death/absence follows quest failure/retarget rules.

No silent substitution.

---

# 18. Faction Contract

Faction quest:
- uses canonical faction ID;
- standing read;
- consequence effect.

Generator never owns faction state.

---

# 19. Expedition Contract

Travel/explore/investigate/fetch:
- may create expedition objective;
- ExpeditionSystem owns journey.

No “quest progress += distance.”

---

# 20. Location Evolution Contract

Dynamic quest can reference current:
- scar;
- anomaly;
- closure;
- discovery.

Location system owns changes.

---

# 21. Survivor Relations Contract

Personal quest can query:
- conflict;
- bond;
- grudge.

SurvivorRelations/NPC memory owns those facts.

---

# 22. Campaign Consequence Contract

Ledger supplies:
- trigger facts;
- source history.

Generated quest outcome emits:
- canonical semantic event.

No duplicate consequence store.

---

# 23. Weather / Radiation Contract

Weather/radiation can make candidate relevant.

They do not become copied quest state.

Quest snapshot records:
- source event ID;
- bound context
if needed.

---

# 24. Chaining Contract

Follow-up:

```text
parent outcome
→ state event
→ authored follow-up candidates
→ normal generation
```

No recursive direct spawn.

---

# 25. Uniqueness Contract

Template defines a uniqueness/similarity signature.

Examples:

```text
one_per_location
one_per_survivor_per_condition
one_per_shortage_episode
repeatable_after_recovery
```

---

# 26. Cooldown Contract

Use:
- signature;
- family;
- template
cooldowns.

A single global 5-day timer is insufficient by itself.

---

# 27. Generation Budget Contract

At each evaluation:
- bounded candidate count;
- bounded issued opportunities.

Urgency can influence ranking.

---

# 28. Persistence Matrix

| Fact | Owner |
|---|---|
| static quest definition | static quest data |
| dynamic template | dynamic quest data |
| generated definition | canonical quest runtime / generator snapshot |
| quest status | canonical quest runtime |
| objective progress | canonical quest runtime |
| rewards/effects | effect authority |
| assignment | quest/expedition |
| generation provenance | generator |
| candidate suppression | generator |
| chain provenance | generator |
| faction standing | faction |
| inventory | inventory |
| relationships | relations |
| location state | location evolution |
| weather/radiation | environment |

---

# 29. Old-Save Migration

Default:

```text
dynamic_generation:
  schema_version: 1
  generation_sequence: 0
  recent_signatures: []
  chain_history: []
```

No generated backlog.

Existing static quest state unchanged.

---

# 30. Idempotence Contract

Stable identities:

```text
trigger_event_id
candidate_id
generated_quest_id
chain_id
```

Reload cannot:
- reroll;
- reissue;
- duplicate reward;
- duplicate consequence.

---

# 31. Failure Injection Matrix

## N171.1 Generator stores independent active quest status
Expected: quest-authority gate fails.

## N171.2 Fetch quest targets nonexistent item
Expected: resolver/integrity fail.

## N171.3 Escort quest ships without escort lifecycle
Expected: objective-compatibility gate fails.

## N171.4 Unrevealed location name leaks through generated quest
Expected: knowledge gate fails.

## N171.5 Persistent shortage emits same medicine quest every five days
Expected: signature/hysteresis gate fails.

## N171.6 Generator grants faction standing directly
Expected: effect-authority gate fails.

## N171.7 Reload changes available generated quest
Expected: deterministic persistence fail.

## N171.8 Accepted quest changes target survivor after original dies
Expected: frozen-binding test fails unless explicitly authored.

## N171.9 Chain A→B→A loops indefinitely
Expected: cycle/depth gate fails.

## N171.10 Generated reward exceeds configured value budget
Expected: reward-budget gate fails.

## N171.11 Dynamic quest reserves mainline unique item/NPC
Expected: reservation conflict gate fails.

## N171.12 Trigger evaluation runs every frame
Expected: performance/source-scan fail.

---

# 32. Determinism Contract

Same:

```text
campaign seed
+ world state snapshot
+ generation epoch
+ template catalog
+ static quest state
+ recent generation history
```

must yield same:
- trigger set;
- candidate IDs;
- rejection reasons;
- ranking;
- selected quest;
- bindings;
- reward profile;
- expiry.

---

# 33. Long-Horizon Metrics

Track:

```text
generation passes
templates matched
candidates built
candidates rejected
rejection reasons
quests issued
quests accepted
quests completed
quests failed
quests expired
duplicate suppressions
family distribution
template repetition
survivor spotlight distribution
location reuse
faction reuse
reward value
chain depth
state bytes
```

---

# 34. Quality Metrics

Track:

```text
% generated quests feasible
% accepted
% completed
% expired
% same-template repetition
% same-location repetition
% self-complete rejection
% mainline conflict rejection
% impossible-deadline rejection
```

A generator with high rejection rate is not “robust”; its templates/resolvers are poorly constrained.

---

# 35. Balance Guardrails

Dynamic quests should:
- respond to real problems;
- create optional opportunity;
- diversify campaigns.

They should not:
- become mandatory chores;
- shower rewards;
- drown authored quests;
- force constant notifications.

---

# 36. Quest Board / Surface Guardrails

Prefer:
- one canonical quest surface.

Dynamic “board” can be:
- a filter/tab/source category.

Only create separate panel if navigation/ownership proves necessary.

---

# 37. Notification Guardrails

New opportunity notifications:
- batch;
- suppress duplicates;
- respect UI density.

No daily alert for same shortage.

---

# 38. Text & Narrative Guardrails

Generated text uses authored templates.

Use:
- concrete resolved names;
- current known state.

Avoid:
- omniscient exposition;
- generic “something happened” text;
- impossible promises.

---

# 39. Localization Contract

Templates:
- title key;
- description key;
- objective key.

Tokens:
- typed entity renderers.

No raw gameplay logic in localized strings.

---

# 40. Accessibility

- quest source/status not color-only;
- filters keyboard accessible;
- deadlines readable;
- reward/objective text wraps;
- text-scale safe.

---

# 41. Content Acceptance

Dynamic template ladder:

```text
DISCOVERED
LOADED
REGISTERED
TRIGGER_MATCHED
PARAMETERS_BOUND
FEASIBLE
GENERATED
ACCEPTED
EFFECT_PRODUCED
```

This is stricter than static content acceptance because generated content can fail during binding.

---

# 42. Reachability Gate

Every shipped template needs at least one deterministic fixture that reaches:

```text
GENERATED
```

Every objective/effect needs at least one fixture that reaches:

```text
EFFECT_PRODUCED
```

No “20 templates” count without proof.

---

# 43. Performance Guardrails

Generation pass:
- event/day-driven;
- indexed templates;
- bounded candidates;
- no large Cartesian product;
- stable resolver caches.

Target budget should be measured, not guessed.

---

# 44. CI / Gate Set

Recommended:

```text
dynamic_quest_authority_single
dynamic_quest_template_integrity
dynamic_quest_objective_compatibility
dynamic_quest_parameter_integrity
dynamic_quest_feasibility
dynamic_quest_knowledge_gate
dynamic_quest_reservation_conflict
dynamic_quest_duplicate_suppression
dynamic_quest_reward_budget
dynamic_quest_chain_acyclic
dynamic_quest_save_no_reroll
dynamic_quest_determinism
dynamic_quest_content_acceptance
dynamic_quest_reachability
dynamic_quest_long_horizon
dynamic_quest_ui_access
```

---

# 45. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --dynamic-quest-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 46. Recommended Commit Breakdown

```text
171A-1 quest-runtime/content authority audit
171A-2 dynamic quest authority matrix
171A-3 template schema
171A-4 trigger predicate registry
171A-5 parameter domain registry
171A-6 objective/reward compatibility matrix
171A-7 catalog loader/integrity
171A-8 docs/tests

171B-1 event/day trigger scheduler
171B-2 deterministic parameter resolvers
171B-3 candidate signature/IDs
171B-4 feasibility filter
171B-5 static-quest reservation guard
171B-6 novelty/saturation
171B-7 ranking/seeded selection
171B-8 candidate diagnostics/goldens

171C-1 generated definition factory
171C-2 canonical availability registration
171C-3 acceptance/assignment parity
171C-4 objective completion parity
171C-5 failure/expiry parity
171C-6 canonical reward/effect path
171C-7 static/dynamic UI parity
171C-8 save/idempotence tests

171D-1 fetch/resource family
171D-2 investigate family
171D-3 explore family
171D-4 escort capability audit/implementation if real
171D-5 defend capability audit/implementation if real
171D-6 negotiate capability audit/implementation if real
171D-7 social-resolve capability audit/implementation if real
171D-8 family compatibility tests

171E-1 chain provenance
171E-2 follow-up template links
171E-3 inherited bindings
171E-4 chain depth/cycle gate
171E-5 completion/failure follow-up
171E-6 narrative variants
171E-7 consequence ledger/archive hooks
171E-8 chain tests

171F-1 quest surface/source filter
171F-2 trigger/relevance explanation
171F-3 generator-specific persistence
171F-4 anti-spam/hysteresis
171F-5 30-day sim
171F-6 120/180/400-day variety/reward sim
171F-7 CI/failure fixtures/perf
171F-8 final ship/no-ship report

171G-1 rarity/difficulty/customization disposition
171G-2 archive/legacy/modding follow-ons
```

---

# 47. Risk Register

## R171.1 Creates a second quest runtime

Mitigation:
- generated definition factory only;
- canonical lifecycle authority.

## R171.2 Produces nonsensical combinations

Mitigation:
- typed parameter domains;
- feasibility filtering;
- fixtures.

## R171.3 Procedural quests feel repetitive

Mitigation:
- novelty/saturation;
- authored variants;
- state-specific bindings.

## R171.4 Quest spam overwhelms authored content

Mitigation:
- opportunity cap;
- mainline precedence;
- event coalescing.

## R171.5 Rewards inflate economy/standing

Mitigation:
- canonical reward profiles;
- value budget;
- long-horizon simulations.

## R171.6 Dynamic quests leak hidden information

Mitigation:
- knowledge-aware resolvers;
- exploration clue semantics.

## R171.7 Chains explode recursively

Mitigation:
- re-enter candidate pipeline;
- max depth;
- cycle validation.

## R171.8 Template changes break active saves

Mitigation:
- persist resolved canonical definition snapshot/version.

---

# 48. Acceptance Checklist

## P0

- [ ] canonical quest runtime identified
- [ ] quest definition DTO audited
- [ ] quest instance/status audited
- [ ] acceptance API audited
- [ ] assignment API audited
- [ ] objective tracker audited
- [ ] reward/effect resolver audited
- [ ] failure/expiry support audited
- [ ] quest history audited
- [ ] quest UI audited
- [ ] quest save section audited
- [ ] questline_master audited
- [ ] moral_choice_quests audited
- [ ] moral_choice_quest_stubs audited
- [ ] CampaignConsequenceLedger audited
- [ ] ExpeditionSystem audited
- [ ] LocationEvolutionSystem audited
- [ ] world graph/discovery audited
- [ ] faction state audited
- [ ] SurvivorRelations audited
- [ ] survivor lifecycle audited
- [ ] item/inventory authority audited
- [ ] weather/radiation audited
- [ ] journal/events audited
- [ ] generated-definition insertion seam selected
- [ ] objective primitive vocabulary documented
- [ ] reward/effect vocabulary documented
- [ ] static quest reservations/conflicts documented
- [ ] baseline no-generation captured
- [ ] dynamic quest authority matrix published

## 171A — Templates

- [ ] versioned `quest_templates.json`
- [ ] logic separated from localized text
- [ ] quest families are descriptive, not runtime engines
- [ ] typed trigger predicates
- [ ] no arbitrary property paths
- [ ] bounded AND/OR structure
- [ ] trigger source provenance
- [ ] event subscriptions registered
- [ ] typed parameter slots
- [ ] canonical domains only
- [ ] binding returns canonical ID
- [ ] item resolver constraints
- [ ] location resolver constraints
- [ ] hidden-location knowledge safe
- [ ] survivor resolver lifecycle safe
- [ ] faction resolver knowledge safe
- [ ] threat resolver uses real state
- [ ] anomaly resolver gated on real authority
- [ ] rejection diagnostics
- [ ] objective specs canonical
- [ ] gameplay logic not in text placeholders
- [ ] fetch compatibility verified
- [ ] escort gated
- [ ] investigate compatibility verified
- [ ] defend gated
- [ ] negotiate gated
- [ ] explore compatibility verified
- [ ] social resolve gated
- [ ] objective count bounded
- [ ] reward profiles canonical
- [ ] no direct reward grant
- [ ] item reward budget
- [ ] faction effects canonical
- [ ] morale effects canonical
- [ ] XP canonical
- [ ] unique rewards authored
- [ ] failure profiles canonical
- [ ] no universal failure penalty
- [ ] reward-value budget

## 171B — Scheduling/Candidates

- [ ] event-driven trigger evaluation
- [ ] day-boundary fallback
- [ ] event coalescing
- [ ] debounce
- [ ] no per-frame scan
- [ ] indexed template matching
- [ ] deterministic domain ordering
- [ ] bounded candidate construction
- [ ] restrictive slots first
- [ ] per-template candidate cap
- [ ] seeded bounded sampling
- [ ] candidate signature
- [ ] stable candidate ID
- [ ] stable generated quest ID
- [ ] no GUID
- [ ] objective support feasibility
- [ ] entity existence
- [ ] lifecycle feasibility
- [ ] route reachability
- [ ] knowledge safety
- [ ] item attainability
- [ ] resource paradox check
- [ ] faction viability
- [ ] diplomacy feasibility
- [ ] escort feasibility
- [ ] defense feasibility
- [ ] social-conflict feasibility
- [ ] mainline reservation check
- [ ] dynamic conflict check
- [ ] deadline feasibility
- [ ] survivor assignment feasibility
- [ ] reward validity
- [ ] failure validity
- [ ] text-token validity
- [ ] feasibility reason log
- [ ] recent-template history
- [ ] similarity signature
- [ ] duplicate active hard reject
- [ ] recently completed penalty
- [ ] persistent-shortage suppression
- [ ] location repetition control
- [ ] survivor spotlight fairness
- [ ] faction variety
- [ ] relevance over novelty
- [ ] ranking factors
- [ ] urgency
- [ ] board-load pressure
- [ ] mainline precedence
- [ ] seeded selection only after ranking
- [ ] stable order before RNG

## 171C — Runtime

- [ ] generated definition factory
- [ ] canonical generated quest IDs
- [ ] generation provenance
- [ ] no separate dynamic status
- [ ] no duplicate assignment
- [ ] no duplicate objective tracker
- [ ] no duplicate completion checker
- [ ] no direct reward path
- [ ] available registered canonically
- [ ] accept uses canonical API
- [ ] assignment uses canonical API
- [ ] active uses canonical runtime
- [ ] completion canonical
- [ ] failure canonical
- [ ] expiry canonical/generalized
- [ ] expired history compact
- [ ] reward effect canonical
- [ ] faction effect canonical
- [ ] relation effect canonical
- [ ] location effect canonical
- [ ] expedition event canonical
- [ ] journal canonical
- [ ] notification semantic
- [ ] no duplicate event family
- [ ] static/dynamic UI parity
- [ ] separate quest board not created unless justified
- [ ] available-opportunity cap
- [ ] active cap decision documented
- [ ] expiry messaging
- [ ] assignment-death policy
- [ ] pre-accept revalidation
- [ ] post-accept world change canonical
- [ ] bindings freeze after acceptance

## 171D — Families

- [ ] fetch uses canonical acquire/deliver primitive
- [ ] fetch trigger is real shortage
- [ ] item relevant
- [ ] source location real/reachable
- [ ] quantity bounded
- [ ] self-completion prevented
- [ ] reward does not trivialize shortage
- [ ] escort only if real lifecycle
- [ ] escort target valid
- [ ] escort destination valid
- [ ] escort failure canonical
- [ ] investigate uses real location/event
- [ ] anomaly not invented
- [ ] defend only if real defense event
- [ ] no fake defend timer
- [ ] negotiate only with real dispute/action
- [ ] explore uses real discovery
- [ ] hidden-location clue safe
- [ ] social resolve only from real conflict
- [ ] relationship effects canonical
- [ ] unsupported families deferred

## 171E — Chains

- [ ] stable chain ID
- [ ] authored follow-up rules
- [ ] no direct recursive spawn
- [ ] follow-up conditions revalidated
- [ ] inherited binding rules
- [ ] canonical inherited IDs
- [ ] max chain depth
- [ ] cycle detection
- [ ] no A→B→A
- [ ] completion branch
- [ ] failure branch
- [ ] no failure content farming
- [ ] rare templates state-driven
- [ ] authored text variants
- [ ] no runtime prose-generation dependency
- [ ] survivor story hooks grounded
- [ ] faction story hooks grounded
- [ ] location story hooks grounded
- [ ] consequence ledger event
- [ ] archive/epilogue only significant quests
- [ ] completion emits trigger event, not direct spawn
- [ ] chain diagnostics

## 171F — UI

- [ ] canonical quest surface reused
- [ ] opportunity card
- [ ] player-visible trigger reason
- [ ] no hidden information leak
- [ ] dynamic source badge/filter
- [ ] canonical detail view
- [ ] real difficulty estimate
- [ ] canonical reward preview
- [ ] absolute expiry
- [ ] eligible survivor preview
- [ ] disabled invalid assignments
- [ ] filter integration
- [ ] bounded notifications
- [ ] journal semantics
- [ ] tutorial only if framework exists
- [ ] accessibility

## 171F — Persistence

- [ ] generator stores only generation-specific state
- [ ] no duplicate lifecycle lists
- [ ] generated definition snapshot strategy
- [ ] unaccepted quest persists
- [ ] compact expired history
- [ ] old-save empty generation state
- [ ] old save no backlog flood
- [ ] reload same opportunities
- [ ] reload cannot reroll
- [ ] accepted quest survives template deletion/version change

## 171F — Anti-Spam

- [ ] availability cap
- [ ] event coalescing
- [ ] trigger hysteresis
- [ ] signature cooldown
- [ ] family cooldown
- [ ] soft global cadence
- [ ] urgent exception
- [ ] static quest pressure
- [ ] reward farming prevented
- [ ] self-complete candidate rejected
- [ ] expiry abuse prevented
- [ ] chain farming prevented
- [ ] survivor-death spam prevented
- [ ] location-discovery spam prevented

## 171F — Balance/Quality

- [ ] difficulty real-state based
- [ ] no generic day-only scaling
- [ ] presentation bands
- [ ] reward budget
- [ ] shortage reward does not erase shortage
- [ ] faction reward bounded
- [ ] morale reward bounded
- [ ] XP reward bounded
- [ ] unique rewards authored
- [ ] failure penalties proportionate
- [ ] variety metrics
- [ ] text variants
- [ ] template repetition bounded
- [ ] invalid combination constraints
- [ ] tonal consistency
- [ ] no omniscient narration
- [ ] real concrete entity names
- [ ] deadline realistic

## 171F — Simulations

- [ ] 30-day baseline
- [ ] 120-day normal campaign
- [ ] 180-day stress
- [ ] no-trigger
- [ ] many-trigger
- [ ] persistent-shortage
- [ ] all-survivors-unavailable
- [ ] no-reachable-location
- [ ] mainline-heavy
- [ ] 400-day variety
- [ ] reward inflation absent
- [ ] quest-board flood absent

## 171F — CI

- [ ] template integrity
- [ ] resolver integrity
- [ ] feasibility failure fixtures
- [ ] golden generation corpus
- [ ] determinism fingerprint
- [ ] dynamic-quest selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] reachability matrix
- [ ] dead-template gate
- [ ] rejection-rate telemetry
- [ ] performance benchmark
- [ ] allocation benchmark
- [ ] generated docs
- [ ] verify-fast

## 171G

- [ ] difficulty follow-on gated
- [ ] rarity follow-on grounded
- [ ] no random legendary reward inflation
- [ ] player customization bounded
- [ ] quest trading deferred
- [ ] legacy derives from archive
- [ ] procedural prose not required
- [ ] LLM content not runtime dependency
- [ ] mod templates follow Plan-47 contract

---

# 49. Ship / No-Ship Gate

**SHIP** only if:

```text
quest_lifecycle_authorities == 1
AND duplicate_dynamic_quest_status_machine == 0
AND generator_owned_direct_reward_mutations == 0
AND generator_owned_faction_standing_mutations == 0
AND generator_owned_morale_mutations == 0
AND generator_owned_expedition_progress_mutations == 0
AND generated_invalid_entity_refs == 0
AND generated_hidden_information_leaks == 0
AND shipped_quest_families_without_objective_consumer == 0
AND impossible_generated_quests == 0
AND persistent_trigger_duplicate_spam == 0
AND mainline_reservation_conflicts == 0
AND dynamic_chain_cycles == 0
AND dynamic_chain_depth_exceeds_budget == 0
AND reward_value_budget_violations == 0
AND per_frame_generation_scans == 0
AND unseeded_generation_rng == 0
AND reload_rerolls_available_quests == false
AND old_save_backlog_flood == false
AND dead_unreachable_templates == 0
AND dynamic_quest_old_save == pass
AND dynamic_quest_save_roundtrip == pass
AND dynamic_quest_determinism == pass
AND dynamic_quest_feasibility == pass
AND dynamic_quest_knowledge_gate == pass
AND dynamic_quest_duplicate_suppression == pass
AND dynamic_quest_content_acceptance == pass
AND dynamic_quest_reachability == pass
AND dynamic_quest_30_day_balance == pass
AND dynamic_quest_120_day_balance == pass
AND dynamic_quest_180_day_balance == pass
AND dynamic_quest_400_day_variety == pass
AND dynamic_quest_selftest == pass
AND data_integrity_selftest == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 50. Implementer Handoff

1. Audit the canonical quest runtime before writing a generator.
2. Make generated quests enter that runtime instead of owning their own lifecycle.
3. Keep static questlines as the authored campaign spine.
4. Build a typed template language, not a free-form scripting engine.
5. Treat source quest families as capability labels; ship only families with real objective primitives.
6. Make every parameter binding return a canonical item/location/survivor/faction/event ID.
7. Evaluate triggers on events/day boundaries, never every frame.
8. Separate relevance from feasibility.
9. Reject impossible candidates before ranking.
10. Respect player knowledge when selecting locations/factions/threats.
11. Respect static quest reservations and active dynamic conflicts.
12. Build stable candidate signatures and generated quest IDs.
13. Suppress persistent-trigger duplicates with hysteresis and signature cooldowns.
14. Use deterministic seeded selection only after constructing a stable valid candidate set.
15. Freeze bindings after acceptance unless the template explicitly supports dynamic objectives.
16. Resolve objectives, rewards, failures, standing, relations, inventory, expeditions, and location effects through canonical systems.
17. Make follow-ups re-enter the same generator instead of recursively spawning direct children.
18. Cap chain depth and validate cycles statically.
19. Persist unaccepted generated opportunities so reload cannot reroll them.
20. Snapshot accepted generated definitions sufficiently to survive template version changes.
21. Prefer one canonical quest UI with an “Opportunity” source/filter over a second quest board.
22. Author 20 templates only when each has deterministic reachability and effect proof.
23. Track rejection reasons and repetition metrics; high rejection means the template system needs repair.
24. Run 30/120/180/400-day simulations for spam, reward inflation, mainline crowding, variety, and persistence.
25. Close only when dynamic quests feel like the world asking the player to respond to what is actually happening—not a slot machine generating generic chores.

---

# 51. Final Outcome

When this plan is complete, ASHFALL's quest layer finally listens to the world it is simulating.

A medicine shortage can create a resource-recovery opportunity because medicine is genuinely scarce, a relevant item exists, a reachable source exists, and the canonical quest runtime knows how to track acquisition or delivery.

A newly discovered location can create an investigation because that place has a real unresolved state.

A survivor conflict can create a personal opportunity because the relationship system says the conflict exists.

A faction crisis can create diplomacy content only if the faction and diplomacy systems actually provide something the player can resolve.

Nothing is invented merely to fill a template.

The generator first asks:

> Is this situation relevant?

Then:

> Can every role be filled with a real entity?

Then:

> Can the player actually complete the resulting objective?

Only then does it create a quest.

Once created, the quest stops being special. It uses the same acceptance, assignment, objective tracking, completion, failure, rewards, consequences, save/load, journal, and UI machinery as authored quests. Dynamic generation therefore expands content without splitting quest truth into two systems.

Replayability comes from recombination.

The same authored template may involve:
- a different survivor;
- a different faction;
- a different route;
- a different shortage;
- a different evolved location;
- a different hazard;
- a different campaign history.

But because those parameters are real and validated, the quest remains grounded.

Chains can emerge, but only through authored follow-up relationships that re-enter the same feasibility pipeline. A completed investigation may expose a rescue opportunity. A failed delivery may create a repair/reconciliation route. Nothing recursively invents infinite plot.

The generator is also deliberately restrained.

It cannot flood the player with the same shortage quest.
It cannot generate a quest already complete.
It cannot leak hidden locations.
It cannot hijack mainline NPCs or unique items.
It cannot grant inflated rewards.
It cannot reroll on reload.
It cannot ship a quest family whose objective system does not exist.

The result is not “infinite quests.”

It is something better:

a finite authored vocabulary capable of producing a large number of coherent, campaign-specific opportunities because ASHFALL's actual state determines what stories are possible.
