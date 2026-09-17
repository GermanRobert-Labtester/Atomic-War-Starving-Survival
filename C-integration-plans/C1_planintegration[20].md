# C1 — Flagship Integration Plan [20]: Expedition Discovery → Persistent World Consequences

> **Output:** `C1_planintegration[20].md`
>
> **Source baseline:** Plan 133 — Expedition Discovery → Persistent World Consequences
>
> **Primary mission:** make expedition discoveries persist as world facts that alter location state, route safety, faction awareness, economy, quest availability, caravan behavior, and later expedition decisions.
>
> **Primary architectural rule:** do not create a second world-state simulation. Discoveries are facts and orchestration. Existing authorities remain owners of location mutations, routes, factions, economy, quests, caravans, and expedition state.
>
> **Primary gameplay rule:** exploration should create strategic consequences the player can understand, defer, reveal, exploit, or lose.
>
> **Mandatory execution order:** 133A authority contract → 133B five consequence families + authored templates → 133C integration, persistence, deterministic hardening, reachability and CI.
>
> **Critical re-baseline rule:** before creating `DiscoveryConsequenceSystem`, verify whether `LocationEvolutionSystem`, expedition result DTOs, world-event history, Plan-31 semantic events, Plan-30 faction autonomy, Plan-32 route mutation, Plan-38 commitment/deadline, and Plan-41 place-memory state already cover pieces of the proposed responsibility. New code must coordinate those authorities, not duplicate them.
>
> **Guardrails:** no second faction-awareness model if Plan 131 rumor/intelligence already owns awareness; no second route-risk table; no passive-income subsystem invented only for resource discoveries; no new excavation minigame; no fake settlement simulation; no new quest runtime; no new diplomacy runtime; no hidden consequences with no warning; no save/load retrigger; no consequence based on wall-clock or unordered iteration; no discovery panel if existing expedition report + map detail + journal can provide the required surface.

---

# 0. Mission

ASHFALL expeditions already move the player out into the world.

They can:
- select a destination;
- travel;
- consume resources;
- encounter risk;
- gather loot;
- return.

The source plan identifies the missing continuity layer:

```text
EXPEDITION
   │
   ▼
DISCOVERY
   │
   ▼
LOOT / RETURN
   │
   └── world largely unchanged
```

This means the most strategically interesting expedition result currently has too little persistence:

```text
found copper vein
cleared bandit camp
uncovered ruins
met faction patrol
found strategic chokepoint
```

These should not behave like disposable encounter text.

They should become durable world facts:

```text
expedition discovers something
        │
        ▼
Discovery Record
        │
        ├── location
        ├── discovery type/subtype
        ├── day
        ├── provenance
        ├── player knowledge
        ├── reveal/conceal state
        └── lifecycle status
        │
        ▼
Consequence Coordinator
        │
        ├────────► LocationEvolutionSystem
        ├────────► Route / world graph authority
        ├────────► Faction awareness / rumor authority
        ├────────► Economy / market authority
        ├────────► Quest authority
        ├────────► TravelingCaravanSystem
        ├────────► Commitments / deadlines
        ├────────► Place memory / journal / briefing
        └────────► Expedition report
```

A discovery should create a chain that the player can reason about:

```text
discover
→ decide whether to reveal
→ immediate benefit/cost
→ other actors learn
→ world authorities mutate
→ secondary options appear
→ chain resolves, decays, or is lost
```

The plan is successful when the world can answer:

> What did the player discover here, who knows about it, what changed because of it, and what consequences are still pending?

---

# 1. Source-Evidence Interpretation

## 1.1 Expedition content is currently sparse

The source reports `expeditions.json` with only two entries.

This does not block persistent-consequence architecture.

It does mean 133 should avoid overfitting to 20 templates that all require destinations that do not yet exist.

Templates must reference real locations or be staged behind future destination plans.

## 1.2 `LocationEvolutionSystem` is the existing mutation foundation

It already tracks facts such as:
- owner;
- contamination;
- loot depletion.

Therefore discoveries should **write through** this authority.

Do not create a parallel `DiscoveryLocationState`.

## 1.3 Caravan and economy systems already exist

The task is:
- route/risk input;
- scarcity/supply input;
not new caravan or economy engines.

## 1.4 Faction reaction hooks already exist

Use current faction coordinators and Plan-30/131 awareness/rumor seams.

If faction awareness is not actually present in current code:
- add the smallest adapter/state needed;
- keep it explicitly within existing faction/world-intelligence ownership.

## 1.5 Quest availability already has an authority

A discovery can unlock a quest.

It must not own quest state.

## 1.6 Plan 41 place-memory is a natural historical sink

Discovery is also a world-history fact.

The map should later show:
- known discovery;
- scar;
- changed ownership/resource/threat state.

## 1.7 Plan 31 event vocabulary is the visibility rail

Discovery chains should emit semantic transitions such as:
- discovery found;
- discovery revealed;
- faction learned;
- route secured;
- resource contested;
- opportunity lost.

No heartbeat events.

---

# 2. Non-Negotiable Discovery Invariants

## INV-133.1 — One discovery identity

Each discovered fact has one stable ID.

No duplicate records from:
- repeated expedition result processing;
- reload;
- UI reopening;
- consequence retries.

## INV-133.2 — Discovery is not world authority

Discovery records facts and orchestration state.

World effects are written through existing domain authorities.

## INV-133.3 — One consequence transition per effect

If a route becomes safer:
- one route mutation;
- one event;
- one history entry.

No duplicate effect through both expedition and discovery code.

## INV-133.4 — Conceal/reveal affects information, not physical truth

If the player physically cleared a threat:
- the threat is cleared whether revealed or concealed.

Concealment controls:
- information spread;
- faction interest;
- trade/credit opportunities.

It cannot undo world facts.

## INV-133.5 — Factions know only through an information channel

Faction awareness comes from:
- reveal;
- rumor;
- patrol;
- caravan;
- autonomous discovery;
according to existing systems.

No global omniscience.

## INV-133.6 — Consequence timing is deterministic

Same:
- seed;
- day;
- discovery;
- awareness state;
- player choices
=> same consequence schedule.

## INV-133.7 — Consequence escalation is bounded

A player finding many things cannot generate unbounded simultaneous demands/modals/events.

## INV-133.8 — Consequences are player-readable

Before a major consequence lands:
- its cause and pending state are visible where reasonable.

## INV-133.9 — One-time discovery cannot be farmed

The same world fact cannot be rediscovered for repeated standing/resources.

## INV-133.10 — Resource extraction uses existing production/economy rails

No new passive-income wallet.

## INV-133.11 — Route safety uses the route authority

No discovery-owned risk multiplier.

## INV-133.12 — Quests unlock through canonical quest runtime

No discovery-owned quest list.

## INV-133.13 — Settlement/NPC arrival uses existing world autonomy only

If no existing settlement-attractor mechanic exists:
- defer that consequence;
- do not create it inside 133.

## INV-133.14 — Strategic fortification uses existing shelter/world expansion rails

No new garrison/fortification game invented here.

## INV-133.15 — Opportunity loss is real state

If another actor exploits/depletes a discovery:
- that state persists and is visible.

## INV-133.16 — Save/load is idempotent

Reload cannot:
- reveal;
- escalate;
- re-award;
- re-trigger faction demand;
- duplicate route changes.

---

# 3. Definition of Done

Plan 133 is complete only when:

- current expedition result seams are audited;
- current location mutation authority is identified;
- current faction awareness/rumor authority is identified or explicitly missing;
- current route safety mutation seam is identified;
- current economy/market mutation seam is identified;
- current quest unlock seam is identified;
- current caravan risk/routing seam is identified;
- one discovery coordinator/registry exists only if existing systems cannot directly own the orchestration;
- stable discovery IDs exist;
- discovery DTO/state is versioned;
- capture/restore is implemented;
- each discovery has provenance and location;
- conceal/reveal state is explicit;
- physical world truth is separated from information state;
- five discovery families are represented;
- resource discovery uses existing resource/production/economy rails;
- threat-cleared discovery changes actual route/world risk;
- ruins discovery unlocks lore/quest/scavenger consequences through existing authorities;
- faction contact uses canonical faction/diplomacy state;
- strategic-location discovery uses only existing fortification/territory mechanics;
- at least 20 templates exist **only if** all referenced systems/locations are real and reachable; otherwise templates are staged with explicit deferral status rather than fake references;
- faction awareness spreads through existing rumor/intelligence channels;
- consequence escalation is data-driven and bounded;
- conceal/reveal is reachable from expedition report/map detail/journal;
- discovery state is visible without requiring a brand-new panel unless existing surfaces cannot carry it;
- multiple discoveries at one location compose deterministically;
- decaying/depleting discoveries persist correctly;
- discovery information can be traded only through real trade/faction mechanisms;
- old saves receive safe empty/default discovery state;
- save/load round trip passes;
- no discovery effect retriggers on reload;
- deterministic schedule/replay passes;
- headless day advancement processes consequences;
- catalog integrity validates location/faction/quest/effect references;
- `--discovery-consequences-selftest` exists or equivalent named gate;
- 30/180-day stress simulations show bounded active consequence count;
- every consequence chain is attributable in briefing/journal/day diagnostics;
- no parallel world/faction/economy/quest authority is introduced.

---

# 4. Phase P0 — Forensic Authority Audit

## P0.1 Capture repository baseline

Record:

```text
commit SHA
branch
dirty paths
expedition definitions
expedition result DTOs
expedition outcome events
LocationEvolutionSystem state
world graph/route mutation APIs
faction awareness/intelligence/rumor APIs
faction standing APIs
economy mutation APIs
quest unlock APIs
caravan route/risk APIs
place-memory APIs
day-event kinds
save sections
```

## P0.2 Build discovery integration matrix

Create:

`docs/expeditions/DISCOVERY_AUTHORITY_MATRIX.md`

Columns:

```text
consequence fact
current authority
write API
read API
persisted?
event
UI surface
discovery role
status
```

Rows:
- discovered resource;
- resource availability;
- resource ownership;
- threat cleared;
- route danger;
- route closure/open state;
- ruins known;
- lore unlocked;
- faction awareness;
- faction standing;
- diplomatic channel;
- strategic control;
- caravan risk;
- quest availability;
- place memory;
- opportunity loss.

## P0.3 Reject duplicate ownership early

If proposed `DiscoveryConsequenceSystem` would own any existing fact:
- redesign before coding.

## P0.4 Decide coordinator responsibility

Allowed responsibilities:

```text
record discovery fact
track reveal/conceal
track consequence stage IDs
track awareness metadata if no canonical owner exists
schedule deterministic pending consequences
dispatch effect declarations to existing authorities
provide read model
capture/restore orchestration state
```

Forbidden responsibilities:

```text
own route risk
own market prices
own quest state
own faction standing
own caravan simulation
own world location mutation
```

## P0.5 Decide UI surface

Audit:
- expedition post-run report;
- map detail;
- journal;
- briefing;
- existing discoveries/records surface if any.

Only add `Discovery Log` panel if:
- route budget allows;
- no current live surface can show persistent discovery status.

Default preference:
- expedition report for immediate decision;
- map detail for location state;
- journal/records for history;
- briefing for deadlines/escalation.

---

# TASK 133A — Discovery Contract, Identity, State & Orchestration

# 133A.0 Goal

Create one stable record of what was found and one deterministic orchestration layer that writes to existing world authorities.

## 133A.1 Discovery record

Define Core DTO/value:

```text
id
template_id
location_id
type
subtype
discovered_day
source_expedition_id
source_encounter_id
revealed
reveal_day
exploited
lifecycle_state
tags
```

Add only fields actually used.

## 133A.2 Stable discovery ID

Prefer deterministic composition:

```text
location + template + source occurrence identity
```

or canonical world-event ID.

Do not use `Guid.NewGuid()`.

## 133A.3 Discovery type enum/value

Initial:

```text
resource
threat
ruins
faction_contact
strategic
```

Use stable snake_case in data.

## 133A.4 Discovery lifecycle

Suggested:

```text
found
concealed
revealed
active
contested
resolved
depleted
lost
expired
```

Not every type uses every state.

## 133A.5 Physical truth vs information state

Separate:

```text
world_state_applied
information_state
```

Example:
- threat is physically cleared immediately;
- faction awareness may remain zero.

## 133A.6 Consequence progress state

Track stable IDs for consequences already triggered.

Example:

```text
triggered_consequence_ids[]
```

Used for idempotence.

## 133A.7 Faction awareness map

Before adding:

- inspect Plan 131/current rumor/intelligence system.

If one exists:
- use it.

If missing:
- minimal awareness adapter may track:

```text
faction_id
knowledge_level
learned_day
source
```

but must be documented as faction-intelligence state, not discovery-owned diplomacy.

## 133A.8 Awareness levels

If existing system has levels:
- reuse.

Otherwise minimal:

```text
unknown
rumored
confirmed
```

No arbitrary 0–100 meter unless current faction system already uses one.

## 133A.9 Discovery source interface

Source plan proposes `IDiscoverySource`.

Before creating:
- check expedition result/event seam.

Preferred:
- consume existing expedition result DTO/event directly.

Create interface only if multiple producers genuinely need a shared port.

## 133A.10 Valid discovery producers

Potential:
- expedition encounter;
- scavenging resolution;
- radio/map fragment;
- quest outcome.

Plan 133 initially focuses expedition.

Do not broaden until needed.

## 133A.11 Consequence template catalog

Create/extend:

`discovery_consequences.json`

Versioned schema.

## 133A.12 Template fields

Recommended:

```text
id
discovery_type
subtype
eligible_location_tags
required_world_state
immediate_effects
reveal_effects
awareness_rules
escalation_stages
decay_rule
localization_keys
```

No embedded scripting.

## 133A.13 Effect declarations

Use existing effect vocabulary/ports:

```text
location_mutation
route_modifier
standing_delta
quest_unlock
market_modifier
caravan_modifier
world_flag
journal_unlock
commitment_create
```

Only if each effect type maps to a real authority.

## 133A.14 No generic reflection effect engine

Use typed applier registry.

## 133A.15 Template reference integrity

Validate:
- location tags/IDs;
- faction IDs;
- quest IDs;
- route IDs if explicit;
- resource IDs;
- localization keys;
- effect types.

## 133A.16 CaptureState

Persist:
- discoveries;
- lifecycle;
- reveal/conceal;
- triggered consequence IDs;
- pending schedule;
- awareness state only if owned here.

## 133A.17 RestoreState

Restore exactly.

No consequence dispatch during restore.

## 133A.18 Schema version

Version discovery state.

Old save:
- defaults to empty list.

## 133A.19 Day owner

Before adding `TickConsequences`, inspect day coordinator.

Preferred:
- existing world evolution/faction day owner invokes discovery orchestration.

No new day owner if avoidable.

## 133A.20 Deterministic tick

Process pending consequences:
- stable sorted discovery ID;
- stable stage order;
- seeded RNG only for authored probability.

## 133A.21 No wall-clock

Forbidden:
- `DateTime.Now`;
- `UtcNow`;
- `Guid`;
- platform enumeration order.

## 133A.22 Transition event

Emit semantic:
- discovery_found;
- discovery_revealed;
- discovery_concealed;
- discovery_consequence_triggered;
- discovery_resolved;
- discovery_lost.

Use Plan-31 registry or add canonical entries.

## 133A.23 Journal/place-memory

Finding discovery:
- writes place memory/history.

No duplicate full prose in both systems.

## 133A.24 Immediate expedition report

At return or discovery moment:
- show discovery;
- show immediate physical effect;
- show reveal/conceal choice if eligible.

## 133A.25 Conceal action

Conceal means:
- no voluntary information spread;
- no reveal-only benefit;
- world physical effect still persists.

## 133A.26 Reveal action

Reveal:
- creates information event;
- triggers configured benefits;
- initiates faction awareness.

## 133A.27 Reveal later

Map detail/journal route can reveal a concealed discovery later.

## 133A.28 Repeated reveal

Idempotent refusal:
- already revealed;
- no duplicate benefit.

## 133A.29 Exploitation state

Only set when actual production/quest/resource authority confirms use.

No UI checkbox pretending exploitation happened.

## 133A.30 Decay

Discovery lifecycle may decay only when:
- underlying authority state changes;
- authored deterministic timer.

## 133A.31 Opportunity-loss transition

If other actors exploit/deplete:
- mark discovery lost/depleted;
- record why.

## 133A.32 Composition root wiring

Use existing campaign services/composition root.

No `GameBootstrap` sidecar if project has migrated beyond it.

## 133A.33 Reference identity test

One discovery coordinator instance:
- expedition adapter;
- save;
- UI;
- day tick.

## 133A.34 Headless support

No UI required to process state.

## 133A.35 Docs

Create:

`docs/expeditions/DISCOVERY_CONSEQUENCE_ARCHITECTURE.md`

### 133A DoD

A discovery is a stable, saveable, deterministic world fact with explicit information state and a typed path into existing authorities.

---

# TASK 133B — Five Discovery Families & Their Real Consequence Chains

# 133B.0 Goal

Implement five distinct discovery families without inventing replacement systems.

---

# 133B-R — Resource Deposit

## 133B.R1 Discovery fact

Examples:
- copper vein;
- clean water source;
- fuel cache;
- salvage field.

Use real resource IDs.

## 133B.R2 Immediate physical state

Location gains:
- resource availability/mutation
through `LocationEvolutionSystem` or canonical resource-location authority.

## 133B.R3 Extraction option

If existing production/expedition system supports:
- repeat extraction job;
- scavenging table;
- expedition action.

Do not add passive income if no production rail exists.

## 133B.R4 Shelter benefit

Real output:
- delivered through Plan 35 inventory/storage.

## 133B.R5 Reveal benefit

Possible:
- trade offer;
- faction standing;
- contract/commitment;
only via existing systems.

## 133B.R6 Faction interest

Awareness may create:
- request to share;
- buy information;
- access demand.

## 133B.R7 Demand as commitment

If Plan 38C supports:
- create commitment.

Do not create discovery-specific deadline tracker.

## 133B.R8 Agreement

Use:
- resource transfer;
- standing;
- contract.

## 133B.R9 Refusal

Use:
- standing;
- faction hostility;
- later raid **only if existing raid authority supports trigger**.

## 133B.R10 Secret keeping

Conceal:
- suppress voluntary awareness;
- no trade benefit.

## 133B.R11 Independent discovery

Faction may learn later via rumor/patrol.

This must be authored and bounded.

## 133B.R12 Resource depletion

Underlying location resource authority determines depletion.

Discovery observes state.

## 133B.R13 Market effect

Economy can react to supply availability.

No direct hardcoded price override in discovery code.

## 133B.R14 Resource tests

- find;
- conceal;
- reveal;
- extract;
- faction learns;
- share/refuse;
- deplete;
- save/load.

---

# 133B-T — Threat Cleared

## 133B.T1 Threat classes

Only real:
- bandit;
- radiation hazard;
- wildlife;
- structural block
where systems exist.

## 133B.T2 Clearance producer

Discovery fires from actual threat resolution.

No "threat cleared" declaration without system proof.

## 133B.T3 Route safety effect

Use route authority:
- danger tier;
- hazard modifier;
- closure state.

## 133B.T4 Caravan effect

Caravan risk uses updated route.

Do not add separate caravan bonus if route resolver already feeds caravan.

## 133B.T5 Trade availability

If location becomes reachable:
- market/trade authority may unlock.

## 133B.T6 Settler attraction

Only implement if existing world autonomy has settlement migration.

Otherwise mark deferred.

## 133B.T7 Credit claim

If factions know event:
- player may claim credit through existing standing/dialogue.

## 133B.T8 Rival credit

Faction taking credit requires:
- existing world narrative/standing mechanism.

Do not create an abstract fame system.

## 133B.T9 Threat return

If underlying system supports respawn/recontamination:
- route safety reverses.

Discovery lifecycle becomes stale/expired.

## 133B.T10 Threat tests

- danger before/after;
- caravan route result before/after;
- hidden/revealed credit;
- threat return;
- idempotence.

---

# 133B-U — Ruins Uncovered

## 133B.U1 Ruin discovery

Use real location/ruin catalog.

## 133B.U2 Lore unlock

Journal/codex via existing content rails.

## 133B.U3 Archaeological quest

Only unlock existing quest type.

No excavation minigame.

## 133B.U4 Excavation as work

If excavation exists:
- labor/expedition/production action.

If not:
- use existing scavenging/expedition action.

## 133B.U5 Scavenger interest

Use:
- world encounter pressure;
- faction/caravan attention
only if existing systems support.

## 133B.U6 Information sale

Trade/diplomacy action.

## 133B.U7 Relic reward

Delivered through inventory.

## 133B.U8 Lore value

No stat currency unless existing.

## 133B.U9 Ruin depletion

Location evolution tracks excavation/depletion.

## 133B.U10 Ruin tests

- discover;
- journal unlock;
- quest unlock;
- reveal/sell;
- excavation/depletion;
- save.

---

# 133B-F — Faction Contact

## 133B.F1 Contact producer

From actual patrol/scout encounter.

## 133B.F2 Diplomatic channel

Use faction system state:
- known/contacted.

## 133B.F3 Standing outcome

Use canonical standing API.

## 133B.F4 Avoid contact

Stealth/avoid only if expedition encounter already supports.

No new stealth system.

## 133B.F5 Future encounter weighting

Faction interaction chance can use:
- territory;
- awareness;
- standing.

No discovery-specific encounter RNG table if existing world encounter authority can consume flags.

## 133B.F6 Loyalty quest

Unlock canonical quest.

## 133B.F7 Contact concealment

A player can choose not to report to shelter/factions only if information ownership matters.

Do not "conceal" the fact from the contacted faction itself.

## 133B.F8 Contact tests

- contact;
- standing;
- channel;
- future encounter eligibility;
- quest unlock;
- save.

---

# 133B-S — Strategic Location

## 133B.S1 Strategic fact

Examples:
- chokepoint;
- defensible ridge;
- bridgehead;
- observation point.

Use real map nodes.

## 133B.S2 World graph relevance

Strategic status may:
- affect route;
- control;
- defense
only via existing systems.

## 133B.S3 Fortification option

If current shelter/world construction supports remote fortification:
- unlock.

Otherwise:
- use existing location upgrade;
- or defer.

## 133B.S4 Faction access demand

Use diplomacy/commitments.

## 133B.S5 Competing faction interest

Plan 30/131 awareness.

No new faction competition simulator.

## 133B.S6 Garrison

Only if a real garrison/assignment system exists.

Otherwise mark follow-on.

## 133B.S7 Sabotage

Only via existing sabotage/raid/event authority.

## 133B.S8 Strategic tests

- discovery;
- reveal;
- competing awareness;
- option unlock;
- territory/control mutation;
- deferred unsupported effects remain explicit.

---

# 133B-C — Catalog of 20 Templates

## 133B.C1 Authoring rule

Do not write 20 templates before:
- all effect fields exist;
- all referenced locations/systems exist.

## 133B.C2 Template distribution

Target example:

```text
resource          5
threat            4
ruins             4
faction_contact   3
strategic         4
```

Adjust to real content.

## 133B.C3 Location reachability

Every template references:
- real location;
- or tag matched by real reachable location.

## 133B.C4 No duplicate semantic templates

Two templates should differ in:
- source;
- consequences;
- context.

Not just prose.

## 133B.C5 Localization

All names/descriptions/choice text keyed.

## 133B.C6 Acceptance gate

Gameplay templates must reach:
- EFFECT_PRODUCED.

Narrative-only info fragments can terminate at SELECTED if honestly classified.

## 133B.C7 Reachability

Plan 46B-style synthetic runs:
- exploration-heavy;
- normal;
- conservative.

## 133B.C8 Rare template policy

Intentionally rare:
- explicit reason;
- valid path.

## 133B.C9 Unreachable templates

Fix/archive.

### 133B DoD

Five discovery families produce distinct, real world consequences through existing systems, with authored templates that are reachable and non-duplicative.

---

# TASK 133C — Integration, Escalation, Persistence & Hardening

# 133C.0 Goal

Make consequence chains compose safely across factions, economy, routes, quests, caravans and long campaigns.

---

## 133C.1 Location evolution integration

Discovery writes:
- mutation record;
- source discovery ID;
- day;
- reason.

## 133C.2 No duplicate location mutation

If expedition system already changed location:
- discovery records that result;
- does not change twice.

## 133C.3 Route integration

Threat/resource/strategic effects use route resolver.

## 133C.4 Route safety composability

Discovery route modifier composes with:
- weather;
- season;
- territory;
- hazards.

## 133C.5 Economy integration

Resource supply modifies economy through existing scarcity/supply APIs.

## 133C.6 Economy attribution

Diagnostics can identify:
- discovery ID;
- resource;
- location.

## 133C.7 Quest integration

Discovery unlocks:
- quest ID;
- prerequisite state.

## 133C.8 Quest idempotence

Repeated discovery processing:
- no duplicate quest.

## 133C.9 Caravan integration

Caravan behavior reacts to:
- route safety;
- market;
- known locations.

Prefer indirect authority-driven effect.

## 133C.10 Faction awareness propagation

Use Plan 131/current rumor system.

## 133C.11 Awareness sources

Potential:
- player reveal;
- caravan;
- patrol;
- rumor;
- competing expedition.

## 133C.12 Awareness schedule

Data-driven:
- delay;
- probability if authored;
- channel.

## 133C.13 Awareness determinism

Seeded.

## 133C.14 Awareness visibility

Player may know:
- known faction aware;
- suspected interest;
- unknown.

Do not reveal hidden AI knowledge unless information channel.

## 133C.15 Consequence escalation

Per template:

```text
stage
trigger
minimum_day/delay
condition
effect
warning
off_ramp
```

## 133C.16 No generic "attention meter"

Use discrete typed stages.

## 133C.17 Active consequence cap

Set bounded number.

If cap reached:
- queue by priority;
- no drop.

## 133C.18 Priority

Suggested:
1. immediate safety;
2. deadline/commitment;
3. faction demand;
4. market opportunity;
5. ambient interest.

## 133C.19 Modal budget

Consequences mostly surface through:
- briefing;
- journal;
- map detail.

Only actionable decision uses modal/current decision surface.

## 133C.20 Escalation warning

Before:
- raid;
- loss;
- depletion;
- hostile claim
where knowable.

## 133C.21 Concealment cost

Source says "forego benefits."

Implement as:
- no reveal-only trade/standing/contract;
- not arbitrary penalty.

## 133C.22 Reveal after conceal

Delayed consequence schedule begins on reveal unless independent awareness already occurred.

## 133C.23 Independent awareness while concealed

Possible if:
- physical world discoverable by others.

Concealment is not magic.

## 133C.24 Multiple discoveries same location

Composition rules:
- stable sorted order;
- one location mutation owner;
- no overwrite loss.

## 133C.25 Conflict example

Resource + threat:
- clearing threat may improve extraction access.

Use both existing state facts.

## 133C.26 Strategic + faction contact

May increase interest.

No combinatorial hardcoded switch explosion.

## 133C.27 Rule composition

Prefer tags/effect declarations.

## 133C.28 Discovery decay

Types:
- depleted;
- threat returned;
- information stale;
- opportunity taken.

## 133C.29 Decay cause

Always real:
- underlying state;
- authored timer + autonomous actor.

## 133C.30 No arbitrary disappearance

Journal/history retains record.

## 133C.31 Discovery trade

Selling information:
- one-time or rights-limited according to data.

## 133C.32 No infinite resale exploit

Track:
- faction sold-to;
- exclusivity.

## 133C.33 Standing reward bounded

No farming via repeated reveal/sell.

## 133C.34 Commitments

Faction demands can become Plan-38 commitments.

## 133C.35 Deadlines

Use commitment due windows.

No discovery-specific due-day loop.

## 133C.36 Raids/hostility

Only call existing raid/faction system.

## 133C.37 Place memory

Record:
- found;
- revealed;
- contested;
- depleted;
- resolved.

## 133C.38 Map detail

Show:
- discovery state;
- known faction interest;
- current location consequence.

## 133C.39 Expedition report

Show immediate:
- discoveries;
- physical effects;
- reveal/conceal choices.

## 133C.40 Journal

Historical chain.

## 133C.41 Dedicated Discovery Log decision

Only create if:
- map+journal cannot provide current actionable list.

Default: no new panel.

## 133C.42 Accessibility

Discovery state:
- text;
- icons;
- no color-only urgency.

## 133C.43 Persistence schema

Register one discovery save section if genuinely new state is not owned elsewhere.

Do not persist duplicate domain effects.

## 133C.44 Save contents

Persist:
- discovery identity;
- reveal state;
- stage;
- pending triggers;
- sold-to/credit state;
- triggered effect IDs.

## 133C.45 Restore ordering

Restore discovery orchestration after required domain authorities are available.

## 133C.46 No restore side effects

No event emission while reconstructing.

## 133C.47 Old-save compatibility

No discovery section:
- empty state;
- current world state unaffected.

## 133C.48 Mid-chain save

Cases:
- concealed;
- revealed pre-awareness;
- faction aware pre-demand;
- demand pending;
- resource partially exploited;
- opportunity lost.

## 133C.49 Reload idempotence

No duplicate:
- standing;
- quest;
- route safety;
- inventory;
- faction demand.

## 133C.50 Determinism fingerprint

Replay:
- discover;
- conceal;
- reveal;
- awareness;
- escalation;
- resolution.

## 133C.51 Headless tick

No UI needed.

## 133C.52 No-expedition edge case

Zero discoveries:
- zero processing overhead beyond minimal.

## 133C.53 All-concealed case

No voluntary reveal consequences.

Independent physical/world effects still correct.

## 133C.54 Rapid-discovery stress

Discover 10+ within short period.

Assert:
- active cap;
- queue;
- no lost transitions.

## 133C.55 30-day simulation

Track:
- discoveries;
- pending;
- faction awareness;
- demands;
- route changes;
- quests;
- caravan effects.

## 133C.56 180-day simulation

Track:
- depletion;
- opportunity loss;
- repeated world evolution;
- save growth.

## 133C.57 Bounded save growth

Discovery history may grow.

Use retention/history policy for resolved old records if necessary.

Never delete facts needed for epilogue/history.

## 133C.58 Diagnostics

Per day record:
- discovery ID;
- stage;
- triggered consequence;
- target authority;
- effect result.

## 133C.59 Selftest

Add:

```text
--discovery-consequences-selftest
```

or current gate naming.

## 133C.60 Selftest scenarios

At least:
- resource reveal/share;
- threat clear route;
- ruins quest;
- faction contact;
- strategic interest;
- conceal/reveal;
- save/load;
- old save;
- duplicate input;
- stress cap.

## 133C.61 Data-integrity selftest

Validate templates.

## 133C.62 Source-scan gate

Detect:
- duplicate world mutation arithmetic;
- `Guid.NewGuid`;
- wall-clock;
- new economy/quest state in discovery namespace.

## 133C.63 Docs

Create:
- `DISCOVERY_CONSEQUENCE_ARCHITECTURE.md`;
- `DISCOVERY_TEMPLATE_MATRIX.md`;
- `DISCOVERY_INTEGRATION_CONTRACT.md`.

### 133C DoD

Discovery chains remain deterministic, bounded, save-safe and causally integrated with every affected world authority.

---

# 5. Discovery State Model

Recommended conceptual state:

```text
FOUND
 ├── CONCEALED
 │     ├── REVEALED_LATER
 │     └── DISCOVERED_BY_OTHERS
 └── REVEALED
       │
       ▼
ACTIVE
 ├── CONTESTED
 ├── EXPLOITED
 ├── RESOLVED
 ├── DEPLETED
 └── LOST
```

Do not implement every branch if family does not need it.

---

# 6. Information vs Physical-State Matrix

| Discovery | Physical truth | Information truth |
|---|---|---|
| resource | resource exists | who knows |
| threat cleared | threat removed | who credits action |
| ruins | ruins discovered by player | who knows location/value |
| faction contact | contacted faction knows | shelter/other factions may not |
| strategic | location is strategic | who knows/acts on it |

Concealment only controls the second column.

---

# 7. Effect Authority Matrix

| Effect | Authority |
|---|---|
| location mutation | LocationEvolutionSystem |
| route safety | WastelandMap/route authority |
| inventory/resource delivery | Plan 35 inventory/storage |
| faction awareness | rumor/intelligence authority |
| standing | faction standing |
| diplomacy channel | faction system |
| prices/supply | economy |
| quest unlock | quest runtime |
| caravan risk | caravan + route |
| commitment | Plan 38 CommitmentSystem |
| place history | LocationMemory |
| briefing/journal | Plan 31 / journal |

---

# 8. Discovery Template Contract

A template must answer:

```text
what can be found?
where?
under what condition?
what physical effect occurs immediately?
what changes only if revealed?
how can others learn?
what escalation stages exist?
what ends/depletes it?
what systems receive effects?
```

---

# 9. Concealment Contract

Concealment can:

- hide voluntary disclosure;
- delay trade/standing benefit;
- delay faction demand;
- preserve exclusivity temporarily.

Concealment cannot:

- resurrect cleared threat;
- un-find ruins for player;
- make other actors physically incapable of independent discovery.

---

# 10. Awareness Contract

Faction awareness should be discrete and attributable.

Example:

```text
unknown
→ rumored
→ confirmed
```

Each transition has:
- source;
- day;
- faction;
- discovery.

---

# 11. Escalation Contract

Every escalation stage has:

```text
id
preconditions
delay
warning
effect
off_ramps
next stages
```

No opaque timer.

---

# 12. Consequence Budget

Track:

```text
active actionable consequences
pending background consequences
critical warnings
```

Set a cap for player-attention events, not world simulation facts.

---

# 13. Route-Safety Contract

Discovery does not set:

```text
caravan_success += 20%
```

Instead:

```text
threat cleared
→ route authority danger state changes
→ caravan reads route
```

One truth.

---

# 14. Economy Contract

Resource discovery affects:
- supply;
- availability;
- local scarcity
through economy APIs.

No discovery-owned price formula.

---

# 15. Quest Contract

Discovery unlock:

```text
discovery state
→ quest prerequisite true
→ canonical quest runtime exposes quest
```

No duplicate quest lifecycle.

---

# 16. Place-Memory Contract

History stores:
- what was found;
- when;
- what changed.

Player visibility obeys knowledge.

---

# 17. UI Surface Contract

## Expedition report
Immediate decision.

## Map detail
Current location consequence.

## Journal
Historical record.

## Briefing
Upcoming consequence/deadline.

This four-surface model should usually eliminate need for a new Discovery Log panel.

---

# 18. Event Vocabulary

Potential canonical kinds:

```text
discovery_found
discovery_concealed
discovery_revealed
discovery_leaked
faction_discovery_awareness
resource_claimed
resource_depleted
threat_cleared
route_secured
ruins_uncovered
strategic_site_identified
discovery_contested
discovery_opportunity_lost
```

Use only registry-compliant values.

---

# 19. Persistence Matrix

| Fact | Persist owner |
|---|---|
| discovery identity | discovery state |
| revealed/concealed | discovery state |
| consequence stage | discovery state |
| location owner/resource/threat | LocationEvolution |
| route safety | route/world |
| faction standing | faction |
| faction awareness | rumor/faction if available |
| market state | economy |
| quest state | quest |
| caravan state | caravan |
| history | journal/place memory |

Do not duplicate.

---

# 20. Old-Save Migration

Schema v0/no section:

```text
discoveries = []
pending = []
```

No retroactive discoveries generated merely from loading.

Future expedition actions populate state.

---

# 21. Idempotence Keys

Each effect dispatch should use:

```text
discovery_id + consequence_id
```

Domain action may also carry batch/event ID if supported.

---

# 22. Failure Injection Matrix

## N133.1 Expedition result delivered twice
Expected: one discovery.

## N133.2 Reveal clicked twice
Expected: one reveal benefit.

## N133.3 Save reload during pending faction demand
Expected: one demand.

## N133.4 Threat-cleared discovery mutates route twice
Expected: authority/idempotence test fails.

## N133.5 Concealed resource still gains reveal-only trade benefit
Expected: concealment test fails.

## N133.6 Faction instantly knows concealed discovery
Expected: information-channel test fails unless faction was direct participant.

## N133.7 Route safety hardcoded inside discovery system
Expected: source-scan/authority test fails.

## N133.8 Resource discovery creates passive-income wallet
Expected: architecture gate fails.

## N133.9 Ruins trigger nonexistent excavation minigame
Expected: integration contract fails.

## N133.10 Old save gains retroactive discoveries
Expected: migration test fails.

## N133.11 20 rapid discoveries create 20 simultaneous modals
Expected: attention budget fails.

## N133.12 Template references nonexistent location
Expected: data-integrity fail.

---

# 23. Determinism Contract

Same:

```text
seed
+ expedition result
+ discovery choices
+ faction/world state
+ day
```

must yield same:

```text
discovery ID
awareness timing
escalation order
effect dispatch
resolution
diagnostic digest
```

---

# 24. Long-Horizon Metrics

Track:

```text
discoveries found
concealed
revealed
independently leaked
resource deposits active/depleted
threats cleared/returned
route safety changes
faction awareness transitions
demands/contracts generated
quests unlocked
caravan route changes
opportunities lost
average active consequence count
max active consequence count
save-state size
```

---

# 25. Performance Guardrails

## Daily tick

Process:
- active/pending discoveries only.

Do not scan all catalog templates every day.

## Catalog

Load once.

## Runtime

Index:
- by discovery type;
- location tag;
- consequence stage.

## Save

Avoid unbounded duplicated effect history.

---

# 26. Accessibility & UX

Every discovery choice shows:
- what is known now;
- what reveal/conceal means;
- immediate consequences;
- uncertainty honestly.

No red/green-only consequence labels.

---

# 27. Localization

All:
- discovery names;
- descriptions;
- choice labels;
- faction demands;
- warnings
keyed through Plan 25.

---

# 28. Content Acceptance

Plan-45 stages:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

All gameplay discovery templates must reach `EFFECT_PRODUCED`.

---

# 29. Reachability

Per template report:
- eligible;
- discovered;
- revealed;
- consequence triggered.

Unreachable:
- fix;
- stage;
- archive.

---

# 30. CI / Gate Set

Recommended:

```text
discovery_template_integrity
discovery_authority_single
discovery_idempotence
discovery_determinism
discovery_save_matrix
discovery_reachability
discovery_attention_budget
discovery_headless
```

---

# 31. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --discovery-consequences-selftest
godot --headless --path . -- --real-campaign-journey-selftest
godot --headless --path . -- --expansions-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current commands if renamed.

---

# 32. Recommended Commit Breakdown

```text
133A-1 authority audit + integration matrix
133A-2 discovery DTO/state/schema
133A-3 deterministic ID/idempotence
133A-4 template loader/effect declarations
133A-5 conceal/reveal
133A-6 save/restore
133A-7 day/event/journal integration
133A-8 docs/selftests

133B-1 resource family
133B-2 threat-cleared family
133B-3 ruins family
133B-4 faction-contact family
133B-5 strategic family
133B-6 20-template authoring
133B-7 reachability/content acceptance
133B-8 snapshots/reporting/docs

133C-1 LocationEvolution/route integration
133C-2 faction awareness/rumor
133C-3 economy/quest/caravan integration
133C-4 escalation/attention budget
133C-5 multi-discovery composition/decay
133C-6 old-save + mid-chain save
133C-7 30/180-day deterministic soak
133C-8 CI gates/failure fixtures/final docs
```

---

# 33. Risk Register

## R133.1 Consequence coordinator becomes second world engine

Mitigation:
- strict authority matrix;
- typed effect adapters.

## R133.2 Consequence overload

Mitigation:
- active actionable cap;
- briefing/journal queue.

## R133.3 Concealment behaves like magic invisibility

Mitigation:
- separate physical truth and information state.

## R133.4 Faction awareness duplicates rumor system

Mitigation:
- forensic audit first;
- reuse Plan 131.

## R133.5 Resource deposits create economy exploit

Mitigation:
- existing production/economy;
- depletion;
- balance tests.

## R133.6 Route safety double-applies

Mitigation:
- one route mutation authority;
- before/after route test.

## R133.7 Template content outruns available systems

Mitigation:
- no authoring before effect seam exists.

## R133.8 Long history bloats save

Mitigation:
- resolved-history retention/rollup;
- no duplicate effect payloads.

---

# 34. Acceptance Checklist

## P0

- [ ] expedition result seam audited
- [ ] location mutation authority audited
- [ ] route mutation authority audited
- [ ] faction awareness/rumor audited
- [ ] economy seam audited
- [ ] quest seam audited
- [ ] caravan seam audited
- [ ] place-memory seam audited
- [ ] event vocabulary audited
- [ ] save sections audited
- [ ] discovery authority matrix published
- [ ] UI-surface decision documented

## 133A

- [ ] discovery DTO/value
- [ ] stable deterministic ID
- [ ] five discovery types
- [ ] lifecycle states minimal
- [ ] physical vs information state separated
- [ ] triggered-consequence IDs
- [ ] awareness authority reused or minimal
- [ ] producer seam reused before interface addition
- [ ] catalog schema versioned
- [ ] typed effect declarations
- [ ] no reflection effect engine
- [ ] reference integrity
- [ ] CaptureState
- [ ] RestoreState
- [ ] old-save default
- [ ] existing day owner reused if possible
- [ ] deterministic processing order
- [ ] no wall-clock/GUID
- [ ] semantic events
- [ ] journal/place-memory
- [ ] expedition report
- [ ] conceal
- [ ] reveal
- [ ] reveal later
- [ ] repeated reveal idempotent
- [ ] exploitation tied to real authority
- [ ] decay tied to real state
- [ ] opportunity-loss transition
- [ ] composition root one instance
- [ ] headless support
- [ ] architecture docs

## 133B — Resource

- [ ] real resource IDs
- [ ] location state mutation
- [ ] extraction uses existing system
- [ ] delivery uses inventory/storage
- [ ] reveal benefit uses faction/trade
- [ ] faction interest
- [ ] demands use commitments where possible
- [ ] agreement uses resource/standing
- [ ] refusal uses existing hostility/raid
- [ ] concealment suppresses voluntary info
- [ ] independent awareness possible
- [ ] resource depletion authority
- [ ] economy reacts indirectly
- [ ] full resource tests

## 133B — Threat

- [ ] real threat producer
- [ ] clearance proved
- [ ] route danger mutation
- [ ] caravan consumes route state
- [ ] trade availability if real
- [ ] settler attraction only if existing
- [ ] credit claim
- [ ] no fame subsystem
- [ ] threat return supported if real
- [ ] full threat tests

## 133B — Ruins

- [ ] real ruin location
- [ ] lore unlock
- [ ] canonical quest unlock
- [ ] no excavation minigame
- [ ] scavenger interest only through existing system
- [ ] information sale
- [ ] relic delivery
- [ ] no lore currency
- [ ] depletion
- [ ] full ruin tests

## 133B — Faction Contact

- [ ] real contact encounter
- [ ] diplomatic channel
- [ ] standing
- [ ] avoid only if stealth exists
- [ ] future encounter weighting through existing authority
- [ ] loyalty quest canonical
- [ ] conceal semantics sane
- [ ] full contact tests

## 133B — Strategic

- [ ] real strategic node
- [ ] graph/control effect
- [ ] fortification only if existing
- [ ] faction access demand
- [ ] awareness competition
- [ ] garrison only if existing
- [ ] sabotage only if existing
- [ ] full strategic tests

## 133B — Templates

- [ ] author only after seams
- [ ] ~20 templates if support exists
- [ ] real reachable locations
- [ ] no semantic duplicates
- [ ] localized
- [ ] gameplay templates EFFECT_PRODUCED
- [ ] reachability report
- [ ] rare policy
- [ ] impossible templates fixed/archived

## 133C

- [ ] location mutation integration
- [ ] no duplicate mutation
- [ ] route integration
- [ ] route modifiers compose with weather/season/territory
- [ ] economy integration
- [ ] economy attribution
- [ ] quest integration
- [ ] quest idempotence
- [ ] caravan integration
- [ ] rumor/awareness integration
- [ ] awareness sources
- [ ] deterministic awareness
- [ ] hidden awareness not leaked
- [ ] escalation stages
- [ ] no attention meter
- [ ] active consequence cap
- [ ] deterministic priority
- [ ] modal budget
- [ ] escalation warnings
- [ ] concealment cost is opportunity cost
- [ ] delayed reveal
- [ ] independent discovery while concealed
- [ ] multi-discovery composition
- [ ] no hardcoded combination explosion
- [ ] decay types
- [ ] real decay cause
- [ ] history persists
- [ ] discovery trade
- [ ] no repeated sale exploit
- [ ] bounded standing reward
- [ ] commitments reused
- [ ] deadline system reused
- [ ] raid system reused
- [ ] place memory
- [ ] map detail
- [ ] expedition report
- [ ] journal
- [ ] no new panel unless proven
- [ ] accessible state
- [ ] one new save section only if necessary
- [ ] no duplicate domain persistence
- [ ] restore ordering
- [ ] no restore side effects
- [ ] old save
- [ ] mid-chain saves
- [ ] reload idempotence
- [ ] deterministic fingerprint
- [ ] headless
- [ ] no-expedition case
- [ ] all-concealed case
- [ ] rapid-discovery stress
- [ ] 30-day simulation
- [ ] 180-day simulation
- [ ] save growth bounded
- [ ] day diagnostics
- [ ] discovery selftest
- [ ] data integrity
- [ ] source-scan authority gate
- [ ] docs complete

---

# 35. Ship / No-Ship Gate

**SHIP** only if:

```text
discovery_orchestration_authorities == 1
AND duplicate_location_state_owners == 0
AND duplicate_route_risk_owners == 0
AND duplicate_faction_standing_owners == 0
AND duplicate_quest_runtimes == 0
AND discovery_ids_deterministic == true
AND discovery_retrigger_on_reload == 0
AND reveal_duplicate_benefits == 0
AND concealed_discoveries_global_faction_omniscience == false
AND physical_truth_undo_by_concealment == false
AND discovery_effects_without_existing_authority == 0
AND gameplay_templates_without_effect_proof == 0
AND unreachable_templates_without_disposition == 0
AND active_actionable_consequence_budget_exceeded == false
AND old_save_migration == pass
AND discovery_save_roundtrip == pass
AND discovery_determinism == pass
AND discovery_headless == pass
AND discovery_reachability == pass
AND discovery_30_day_soak == pass
AND discovery_180_day_soak == pass
AND data_integrity_selftest == pass
AND discovery_consequences_selftest == pass
AND content_acceptance_gate == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 36. Implementer Handoff

1. Audit every authority before creating `DiscoveryConsequenceSystem`.
2. Let discovery own orchestration facts, never route/economy/quest/faction truth.
3. Build stable deterministic discovery IDs first.
4. Separate physical world change from information/reveal state.
5. Use one typed effect-adapter registry into existing systems.
6. Wire expedition result → discovery exactly once.
7. Persist orchestration state without duplicating domain effects.
8. Implement conceal/reveal as information strategy, not magical invisibility.
9. Reuse rumor/intelligence for faction awareness.
10. Use Plan 38 commitments for faction demands/deadlines.
11. Use route authority for threat-cleared safety.
12. Use Plan 35 inventory/production for resource exploitation.
13. Use canonical quest runtime for ruins/contact follow-up.
14. Use caravan system indirectly through route/economy where possible.
15. Do not create settlement, fortification, garrison, excavation or sabotage systems unless already present.
16. Author templates only after every referenced effect seam exists.
17. Cap player-attention consequences while allowing background world state to continue.
18. Make multi-discovery composition deterministic.
19. Treat opportunity loss/depletion as persistent state.
20. Add old-save and mid-chain save fixtures before content expansion.
21. Run 30- and 180-day stress simulations.
22. Close with named selftest, content acceptance, reachability and authority gates.

---

# 37. Final Outcome

When this plan is complete, an expedition no longer ends when the party comes home.

Finding a copper vein changes the location because a resource really exists there. The player can keep the information quiet, exploit it through the existing production/inventory economy, reveal it for trade or standing, and eventually face other actors who learn about it through real information channels.

Clearing a threat makes the route safer because the route authority changed—not because the caravan system received a hidden "+20%" bonus. Caravans then react to the safer route naturally.

Uncovering ruins becomes a persistent place-memory fact. Lore and quests unlock through the existing journal and quest systems. Scavenger or faction interest can grow through world autonomy if those systems support it.

Meeting a faction patrol can establish a real diplomatic channel and standing history. Finding a strategic chokepoint can become relevant to territory, commitments or fortification only where those mechanics already exist.

The world can also move without the player. A concealed discovery may leak. Another faction may exploit a resource. A site may be depleted. An opportunity can be lost. Those outcomes remain attributable and deterministic, rather than feeling like arbitrary event rolls.

Most importantly, the discovery layer never becomes a second world simulation.

It records what was found, what the player revealed, which consequences have already fired, and what remains pending. Everything else is delegated to the authorities that already own the world.

The result is exploration with memory.

The player does not merely visit the wasteland.

They change it.
