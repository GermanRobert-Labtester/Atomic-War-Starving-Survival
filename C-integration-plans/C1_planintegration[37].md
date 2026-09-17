# C1 — Flagship Integration Plan [37]: Seasonal Human Migration, Refugee Flows, Trader Circuits & Faction Relocation

> **Output:** `C1_planintegration[37].md`
>
> **Source baseline:** Plan 199 — Seasonal Migration (Human/Faction) System
>
> **Primary mission:** make ASHFALL's human world visibly and mechanically seasonal by allowing refugees, traders, nomads, and faction operations to shift through the world according to real calendar, weather, radiation, scarcity, conflict, route, settlement and faction conditions—without introducing a parallel population, trade, faction-territory, route, expedition, or settlement simulation.
>
> **Primary architectural rule:** `SeasonalMigrationSystem` owns **migration intent, seasonal activation, migration episodes, route-plan selection, group travel progression, arrival/departure timing, and migration-specific provenance**. It does not own canonical settlement population, survivor identity, caravan trading inventory, faction standing/territory, market prices, expedition hazards, weather, radiation, world topology, permanent settlement creation, refugee admission consequences, or long-term faction control.
>
> **Primary modeling correction:** the source proposes migrant groups with their own `morale`, `supplies`, leader state, permanent settlement transitions, faction territory mutation, and trader inventory behavior. Those fields can duplicate existing population, caravan, needs, faction, trade and colony systems. The flagship instead models a migrant group as a **temporary world movement entity** whose population manifest, cargo/readiness, faction affiliation and settlement outcomes are references/projections over existing authorities.
>
> **Primary route correction:** seasonal human migration must reuse canonical world topology and travel costs. `migration_routes.json` may define authored **route intent / seasonal corridors**, but it must not create a second graph disconnected from expedition/trade/world routes.
>
> **Primary consequence rule:** migration should affect the world only through existing consumer systems. Refugee arrivals create a pending settlement/hosting decision; trader arrival enables the canonical caravan/trade session; faction relocation updates operations through the faction/world-state authority; nomadic visits create temporary access to trade/intelligence/guide services where those systems already exist.
>
> **Primary persistence rule:** save **active migration episodes and migration-specific schedule/progress**, not duplicated settlement populations, caravan stock, faction territory, or market state. Old saves start with no already-in-flight migration episodes unless the migration layer can reconstruct a current seasonal episode deterministically without retroactive world changes.
>
> **Mandatory execution order:** 199A authority/topology audit → 199B migration route/pattern data contract → 199C migration episode/group identity → 199D trigger and seasonal activation logic → 199E route traversal and arrival/departure → 199F refugee integration → 199G trader/caravan integration → 199H faction relocation integration → 199I nomad/intelligence/guide integration → 199J world consequences/economy/expedition coupling → 199K UI, persistence, migration, determinism and performance → 199L 30/120/180/400-day simulations and CI → 199M advanced mass migration/settlement networks only after the core is proven.
>
> **Critical re-baseline rule:** before creating `SeasonalMigrationSystem.cs`, inspect `WildlifeMigrationSystem`, `TravelingCaravanSystem`, `WeatherSystem`, `ISimClock`, Plan-38 seasonal progression, Plan-164 nuclear-winter progression, world topology/route catalogs, settlement/population ownership, recruitment/admission, faction branch/territory authority, trade/market/caravan inventory, expedition route/hazard state, Plan-131 intelligence, Plan-160 expedition colonies, Plan-192 trade routes, Plan-133 persistent world consequences, Plan-170 seasonal events, and save orchestration.
>
> **Guardrails:** no second world graph; no second settlement population ledger; no duplicate survivor objects inside migrant groups; no migration-owned caravan stock or prices; no migration-owned faction standing; no direct arbitrary territory mutation; no migration-owned refugee food consumption once admitted; no generic `morale 0–100` if a real group/survivor morale authority exists; no generic `supplies 0–100` if inventory/cargo/logistics already exist; no daily per-frame movement; no unseeded RNG; no wall-clock month logic; no route teleportation; no fixed north/south/east/west semantics when world topology is graph-based; no "winter = everyone migrates south" hardcode; no guaranteed refugee spawning from calendar alone; no migration event/journal spam; no 10-route quota without reachability; no forced admission/rejection outcome bypassing settlement/governance/autonomy systems; no automatic faction relocation that invalidates authored mainline locations.

---

# 0. Mission

ASHFALL already has movement in the world, but not seasonal human movement.

The source baseline identifies:
- `WildlifeMigrationSystem.cs` and `.Live.cs` for wildlife;
- `TravelingCaravanSystem.cs` for NPC caravans on fixed routes;
- `WeatherSystem` and seasonal progression;
- no human seasonal migration;
- no refugee flows;
- no trader route seasonality;
- no faction relocation;
- no population shifts driven by winter, radiation, scarcity or conflict.

The current world therefore behaves approximately like:

```text
SEASONS CHANGE
     │
     ├── weather changes
     ├── wildlife may move
     └── human population/trade/faction presence stays static
```

The target is:

```text
CANONICAL WORLD CONDITIONS
      │
      ├── season
      ├── temperature
      ├── nuclear winter
      ├── radiation
      ├── resource pressure
      ├── faction conflict
      ├── route safety
      └── settlement capacity
      │
      ▼
SeasonalMigrationSystem
      │
      ├── route/pattern eligibility
      ├── migration episode creation
      ├── source/destination choice
      ├── group travel progress
      ├── arrival/return schedule
      └── migration provenance
      │
      ▼
MIGRATION TYPE ADAPTERS
      │
      ├────────► Refugee admission / settlement population
      ├────────► TravelingCaravanSystem / Market / trade session
      ├────────► Faction world-state / branch coordinator
      ├────────► Plan 131 intel/news
      ├────────► Expedition route hazard/presence
      └────────► Plan 160 colony / Plan 192 trade routes
```

The seasonal migration layer should answer:

> Which human group is moving, why now, from where to where, along which canonical route, how far has the episode progressed, and what canonical system should receive the arrival/departure consequence?

It should not answer:

> Who permanently lives in a settlement?
> What exact goods a trader owns?
> What final market price is charged?
> What territory a faction owns?
> Which survivor eats which ration?
> What expedition danger exists?
> What permanent colony is created?

Those remain canonical systems.

---

# 1. Source-Evidence Interpretation

## 1.1 Seasonal human migration is genuinely absent

The source reports zero Core matches for:
- `SeasonalMigration`;
- `MigrationRoute`;
- `SeasonalRoute`;
- `CaravanMigration`;
- `PopulationMigration`;
- `SeasonalMovement`.

A coordination system is justified.

## 1.2 Wildlife migration is inspiration, not a shared population model

`WildlifeMigrationSystem` already models non-human populations.

Human migration differs because human groups interact with:
- factions;
- trade;
- settlements;
- diplomacy;
- admission;
- quests;
- intelligence.

Reuse:
- seasonal trigger patterns;
- movement cadence;
- deterministic route selection
where appropriate.

Do not force humans into wildlife population DTOs.

## 1.3 TravelingCaravanSystem is the trader authority seam

The source explicitly identifies:
- `SpawnCaravan`;
- `DailyTick`;
- `TryBuyItem`.

Therefore seasonal trader circuits should preferably:
- schedule;
- reroute;
- activate;
- suppress
canonical caravans.

They should not build a second trader entity/trade inventory.

## 1.4 “Population size” needs an authority decision

A migrant group can carry a count.

But after arrival:
- accepted refugees must become real settlement population/survivors through the canonical population/recruitment/admission pipeline.

The group count cannot remain a second population truth.

## 1.5 Morale and supplies are likely duplicate abstractions

If migrant travel already has:
- caravan cargo;
- survival state;
- group readiness;
- route attrition,
reuse it.

If no group-level travel logistics exist:
- a narrow `migration_readiness` projection may be justified.

Do not automatically create generic `morale` and `supplies` bars.

## 1.6 Faction relocation is the highest-risk feature

Moving faction operations can affect:
- territory;
- branches;
- access;
- diplomacy;
- raids;
- trade;
- quests.

It must go through canonical faction/world-state APIs.

No direct:
`territory = destinationRegion`.

## 1.7 Seasonal route definitions must bind to world topology

`originRegion` and `destinationRegion` can be authored IDs.

Actual movement should resolve through:
- world graph;
- trade route graph;
- expedition routes.

No disconnected directional simulation.

## 1.8 Nuclear winter is not merely “winter pattern × larger intensity”

Plan 164 may radically change:
- hazard;
- accessibility;
- shelter demand;
- route viability.

Migration must react to the real nuclear-winter world state.

---

# 2. Non-Negotiable Migration Invariants

## INV-199.1 — One world-topology authority

Migration routes resolve through the canonical route graph.

## INV-199.2 — One settlement-population authority

Migration group counts do not become permanent population until admitted/settled through canonical APIs.

## INV-199.3 — One caravan/trade authority

Trader migration schedules existing caravan/trade systems.

## INV-199.4 — One faction-state authority

Faction relocation submits operations/world-state changes through faction APIs.

## INV-199.5 — One weather/season authority

Migration reads seasons; never calculates a second season.

## INV-199.6 — One radiation authority

## INV-199.7 — One scarcity/economy authority

## INV-199.8 — One expedition route/hazard authority

## INV-199.9 — Migration episode identity is stable

## INV-199.10 — Group movement is deterministic for a fixed seed/state

## INV-199.11 — Seasonal triggers use real conditions

## INV-199.12 — Return migration is not guaranteed if world conditions changed

## INV-199.13 — Refugee acceptance is a settlement/governance decision

## INV-199.14 — Rejection does not delete the group silently

The group may:
- continue;
- divert;
- disperse;
- settle elsewhere
through migration/world logic.

## INV-199.15 — Trader arrivals use canonical caravan inventories and trade sessions

## INV-199.16 — Faction relocation cannot strand mandatory content

## INV-199.17 — Migration information respects player knowledge

Do not show hidden groups/routes unless discovered.

## INV-199.18 — Route movement cannot teleport

## INV-199.19 — No per-frame movement updates

## INV-199.20 — Old saves do not receive retroactive migration history

## INV-199.21 — Migration events are bounded semantic milestones

## INV-199.22 — Wildlife migration is coordinated, not merged into human state

## INV-199.23 — Permanent settlement is an explicit handoff

## INV-199.24 — Human migration is not a free resource faucet

---

# 3. Definition of Done

Plan 199 closes only when:

- world topology/route authority is documented;
- season/calendar authority is documented;
- weather/radiation trigger inputs are documented;
- TravelingCaravanSystem ownership is documented;
- settlement population/recruitment authority is documented;
- faction world-state/territory authority is documented;
- expedition route/hazard ownership is documented;
- Plan-131 intel ownership is documented;
- migration route definitions resolve to canonical origin/destination/route references;
- no second graph exists;
- one stable migration episode model exists;
- group identity and group count semantics are explicit;
- route activation is deterministic from season/conditions plus seeded stochasticity only where intended;
- route deactivation does not erase active groups;
- in-flight groups continue/divert according to rules;
- arrival/return/settlement are semantic transitions;
- refugee groups can arrive and enter a pending admission decision;
- accepted refugees enter canonical settlement population;
- rejected refugees remain world entities until resolved;
- trader circuits activate canonical caravans;
- trader inventory/prices remain canonical;
- faction relocation goes through faction/world-state APIs;
- faction relocation cannot break mainline-critical content;
- nomadic groups use real trade/intel/guide services if available;
- migration can affect expedition information/presence without overwriting route truth;
- old saves load with no fabricated past flows;
- active seasonal state begins cleanly from migration feature activation;
- save/load round-trips active groups exactly;
- no group duplicates on reload;
- no route activation double-spawns groups;
- 30/120/180/400-day simulations show seasonality and bounded population/economic effects;
- `--seasonal-migration-selftest` exists or equivalent;
- data-integrity selftest validates route, location, faction and consumer references;
- every authored route is reachable or intentionally dormant/test-only;
- UI respects intelligence visibility.

---

# 4. Phase P0 — Authority, Topology & Population Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
ISimClock
calendar/season APIs
Plan-38 seasonal progression
Plan-164 nuclear winter
WeatherSystem
RadiationSystem
world topology graph
location catalogs
region catalogs
route catalogs
ExpeditionSystem
TravelingCaravanSystem
caravan identity
caravan route fields
caravan inventory
trade session APIs
MarketSystem
settlement population owner
survivor recruitment/admission
refugee/visitor systems if any
FactionBranchCoordinator
faction territory/control owner
Plan-131 intelligence
Plan-160 colony systems
Plan-192 trade routes
Plan-133 persistent world consequences
save order
world map UI
```

## P0.2 Build migration authority matrix

Create:

`docs/migration/SEASONAL_MIGRATION_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
read API
write API
migration role
persisted?
status
```

Rows:
- season;
- month/year;
- nuclear winter state;
- temperature;
- radiation;
- route graph;
- region;
- settlement capacity;
- population;
- visitor group;
- caravan;
- trader cargo;
- market price;
- faction presence;
- territory;
- expedition hazard;
- information visibility;
- migration episode;
- migration schedule;
- group travel progress;
- arrival handoff;
- permanent settlement.

## P0.3 Topology ADR

Create:

`docs/architecture/ADR_SEASONAL_MIGRATION_AND_WORLD_TOPOLOGY.md`

Define:
- migration corridor;
- canonical route path;
- route re-evaluation;
- blocked route;
- alternate path;
- no teleport.

## P0.4 Population ADR

Create:

`ADR_MIGRANT_GROUP_VS_SETTLEMENT_POPULATION.md`

Define:
- transient group count;
- member manifest if any;
- admission;
- survivor instantiation;
- permanent population handoff.

## P0.5 Caravan ADR

Create:

`ADR_SEASONAL_TRADER_CIRCUITS_VS_TRAVELING_CARAVANS.md`

## P0.6 Faction relocation ADR

Create:

`ADR_FACTION_RELOCATION_AUTHORITY.md`

## P0.7 Baseline proof

Demonstrate:
- fixed caravan routes;
- no seasonal activation;
- static human presence through season changes.

---

# TASK 199A — Route, Pattern & Corridor Data Contract

# 199A.0 Goal

Define authored seasonal movement intent without creating a second route graph.

## 199A.1 Data file

`Assets/StreamingAssets/Data/migration_routes.json`

Appropriate as data authority for:
- route policy;
- seasonality;
- origin/destination intent.

## 199A.2 `MigrationRouteDefinition`

Suggested:

```text
route_id
display_key
migration_type
origin_ref
destination_ref
canonical_route_selector
seasonal_pattern_id
activation_policy_id
return_policy_id optional
group_profile_id
max_concurrent_groups
cooldown_days
visibility_policy_id
tags[]
```

## 199A.3 Remove route-name logic dependency

`routeName` is localized presentation only.

## 199A.4 No direct directional field required

Source proposes:
- north;
- south;
- east;
- west.

Prefer:
- origin/destination/corridor.

Direction can be derived for UI if coordinates exist.

## 199A.5 Route selector

Can be:
- explicit canonical path ID;
- route tags;
- shortest safe path policy;
- trader route ID.

## 199A.6 Route activation

Not merely:
- departureMonth.

Use:
- season window;
- source condition;
- destination attractiveness;
- route viability.

## 199A.7 Calendar fields

If months actually exist:
- use canonical month.

If campaign uses season/day only:
- do not invent a 12-month clock.

## 199A.8 Return policy

Examples:

```text
none
scheduled
condition_based
season_reversal
home_when_safe
```

## 199A.9 One-way refugees

Common.

## 199A.10 Round-trip traders

Common.

## 199A.11 Nomadic circuits

Can have:
- waypoint loop.

Still bind every edge to canonical topology.

## 199A.12 Faction relocation

May switch between:
- authored operational bases.

Do not move every faction member individually.

## 199A.13 Group profiles

Separate data:

`migration_group_profiles.json`

## 199A.14 Group profile DTO

Suggested:

```text
id
migration_type
composition_profile
min_size
max_size
affiliation_ref optional
travel_policy_id
arrival_policy_id
information_profile_id
```

## 199A.15 No hardcoded group sizes

## 199A.16 Population size

At spawn:
- deterministic sample or authored value.

## 199A.17 Leader

Do not require a survivor ID unless a real named character exists.

Use:
- leader_entity_ref optional.

## 199A.18 Generic group

Can exist without instantiating 50 survivor entities.

## 199A.19 Route data integrity

Validate:
- origin;
- destination;
- route selector;
- season pattern;
- group profile;
- migration type.

## 199A.20 10+ routes

Source target is a content goal, not architectural success.

Ship only reachable, meaningful routes.

## 199A.21 Generated docs

Create:

`MIGRATION_ROUTE_MATRIX.md`
`MIGRATION_GROUP_PROFILE_MATRIX.md`

### 199A DoD

Migration data defines seasonal movement intent and group profiles while every actual journey remains grounded in the canonical world route graph.

---

# TASK 199B — Seasonal Pattern & Trigger Evaluation

# 199B.0 Goal

Determine when migration pressure exists using canonical world conditions.

## 199B.1 Pattern catalog

`migration_patterns.json`

## 199B.2 Pattern DTO

Suggested:

```text
pattern_id
season_windows[]
condition_predicates[]
migration_type_tags[]
intensity_curve_id
destination_policy_id
return_policy_id
```

## 199B.3 Source spring/summer/fall/winter patterns

Treat as authored tendencies.

Do not hardcode global directional rules.

## 199B.4 Spring

Possible:
- route reopening;
- return migration;
- trader activation.

Only where route definitions say so.

## 199B.5 Summer

Possible:
- trade peak;
- nomadic circuits;
- faction field operations.

## 199B.6 Fall

Possible:
- pre-winter stock movement;
- shelter-seeking.

## 199B.7 Winter

Possible:
- harsh-route suppression;
- shelter/refugee pressure;
- trader reduction.

## 199B.8 Nuclear winter

Read Plan-164 state.

Could:
- suppress some routes;
- intensify refugee pressure;
- create alternate safe corridors.

## 199B.9 Temperature trigger

WeatherSystem/current regional climate.

## 199B.10 Radiation trigger

Radiation/world hazard.

## 199B.11 Resource scarcity

Read canonical settlement/region economy if one exists.

Do not create region scarcity meter inside migration.

## 199B.12 Faction pressure

Read:
- conflict;
- raids;
- control changes
from faction/world authority.

## 199B.13 Seasonal trigger

Calendar.

## 199B.14 Trigger composition

Use:
- AND/OR predicate groups
with explicit data schema.

## 199B.15 Intensity

Represents:
- probability/rate/count of migration episode creation.

Not:
- direct resource consumption.

## 199B.16 Determinism

If stochastic spawning is desired:
- ISeededRng.

Seed by:
- campaign;
- route;
- season/year;
- activation sequence.

## 199B.17 No reroll on reload

## 199B.18 Hysteresis

Condition-triggered migration should not start/stop daily around threshold.

## 199B.19 Cooldown

Prevent repeated groups every day.

## 199B.20 Route capacity

Bound concurrent groups.

## 199B.21 Destination capacity

Can reduce attractiveness.

## 199B.22 No guaranteed “safe shelter magnet”

Player shelter only becomes destination if:
- known;
- accessible;
- acceptable;
- route policy allows.

## 199B.23 Information asymmetry

Migrants may not know all safe locations.

Use Plan 131/world knowledge if available.

### 199B DoD

Migration episodes arise from stable seasonal and world-condition policies instead of calendar spam or arbitrary direction rules.

---

# TASK 199C — Migration Episode & Group Identity

# 199C.0 Goal

Represent a moving human group as a temporary world entity with minimal migration-specific state.

## 199C.1 `MigrationEpisode`

Suggested:

```text
episode_id
route_id
group_id
migration_type
origin_ref
destination_ref
path_ref / path_nodes
departure_day
planned_arrival_day optional
return_episode_ref optional
status
current_path_index
segment_progress
spawn_reason
source_condition_refs[]
```

## 199C.2 Group record

Suggested:

```text
MigrantGroupRef
  group_id
  group_profile_id
  affiliation_ref optional
  population_count
  composition_summary
  leader_entity_ref optional
  external_group_state_ref optional
```

## 199C.3 Do not duplicate survivor list by default

Generic refugee group can be abstract until:
- admitted;
- interacted with;
- narratively instantiated.

## 199C.4 Named group

If named NPCs exist:
- reference them.

## 199C.5 Population count

Temporary migration fact.

Upon admission:
- hand off admitted count/members.

## 199C.6 Composition

Presentation/arrival policy.

Examples:
- families;
- traders;
- fighters;
- mixed.

Do not automatically infer combat strength.

## 199C.7 Supplies

Prefer external logistics/caravan state.

If no authority exists, store a narrow:
- travel_readiness;
or
- supply_days
with explicit ADR.

## 199C.8 Morale

Do not add unless group-level morale has a real consumer.

## 199C.9 Status

Suggested:

```text
scheduled
departing
en_route
waiting
arrived
pending_decision
returning
diverted
settled_handoff
dispersed
completed
cancelled
```

## 199C.10 Stable IDs

No GUID.

Suggested:

```text
migration:<route>:<season_cycle>:<sequence>
group:<profile>:<origin>:<episode_sequence>
```

## 199C.11 Episode creation

Exactly once.

## 199C.12 Return

New linked episode or phase.

Prefer:
- linked return episode
for clean provenance.

## 199C.13 Dispersal

Does not silently delete people if another world-population system tracks them.

## 199C.14 State compactness

No narrative text.

## 199C.15 Source facts

Reference cause IDs.

## 199C.16 Restore

No departure/arrival events replay.

### 199C DoD

Active migration is represented as a compact, stable world movement episode with no duplicate settlement, trade, faction, or survivor simulation.

---

# TASK 199D — Canonical Route Traversal

# 199D.0 Goal

Move groups through the same world geography used by expeditions/trade.

## 199D.1 Resolve path

At episode start:
- canonical route graph.

## 199D.2 Path snapshot vs dynamic reroute

ADR decision.

Recommended:
- snapshot initial path;
- allow reroute only on route-block event.

## 199D.3 Travel cost

Use:
- canonical segment distance/time;
- weather/hazard travel modifiers.

## 199D.4 No separate “daily progress = 1 location”

unless route graph naturally uses that.

## 199D.5 Movement cadence

Campaign day/tick boundary.

## 199D.6 No per-frame movement

## 199D.7 Segment progress

Persist.

## 199D.8 Weather

May:
- slow;
- block;
- divert.

WeatherSystem owns weather.

## 199D.9 Radiation

May:
- make route unsafe.

Radiation/hazard authority owns safety facts.

## 199D.10 Conflict

Faction/route hazard can block.

## 199D.11 Migration response

Group policy chooses:
- wait;
- reroute;
- turn back;
- disperse.

## 199D.12 No teleport on blocked path

## 199D.13 World route change

If a bridge collapses:
- route topology event invalidates path.

## 199D.14 Path recompute

Deterministic.

## 199D.15 Arrival

When canonical traversal completes.

## 199D.16 Travel attrition

Only if:
- a canonical group/caravan survival/logistics system exists.

Do not invent daily random deaths inside migration by default.

## 199D.17 Visibility

Map position shown only if player has intel.

## 199D.18 Encounter

Expedition may encounter group if:
- same route/region/time;
- encounter system supports.

Migration supplies presence predicate.

## 199D.19 No direct expedition safety multiplier

The expedition system can query:
- migrant/trader/faction presence
and decide consequences.

### 199D DoD

Migrant groups traverse real world routes under real weather/hazard constraints, can wait/reroute without teleportation, and expose presence to expedition systems without owning expedition outcomes.

---

# TASK 199E — Refugee Flow Integration

# 199E.0 Goal

Create meaningful seasonal/disaster refugee arrivals without a second settlement population system.

## 199E.1 Refugee trigger

Potential:
- nuclear winter;
- radiation displacement;
- faction conflict;
- settlement collapse;
- scarcity.

Source authority required.

## 199E.2 Destination selection

Candidate factors:
- known safety;
- route viability;
- settlement capacity;
- faction hostility;
- shelter reputation;
- existing contacts.

## 199E.3 Player shelter is not default universal destination

## 199E.4 Arrival state

At shelter/settlement:

```text
pending_admission
```

## 199E.5 Admission authority

Audit:
- survivor recruitment;
- shelter capacity;
- governance;
- population system.

## 199E.6 Accept

Calls canonical admission/recruitment.

## 199E.7 Abstract group to survivor instances

Decide:
- instantiate all;
- sample named survivors;
- population aggregate.

Use existing population architecture.

## 199E.8 No double count

Once handed off:
- migrant group count decrements/closes accordingly.

## 199E.9 Partial acceptance

Potential if system supports.

## 199E.10 Reject

Group remains external.

Possible:
- continues route;
- waits;
- diverts;
- leaves.

## 199E.11 No instant morale/standing penalty hardcode

Consequences through:
- faction;
- morality;
- relations;
- governance
if those systems define them.

## 199E.12 Resource demand

Accepted people consume through:
- Needs/Inventory/settlement economy.

Migration does not subtract abstract resources.

## 199E.13 Temporary sheltering

Only if visitor/guest system exists.

## 199E.14 Settlement elsewhere

If world settlement evolution supports:
- hand off.

Otherwise:
- group exits simulation at destination with world-event record.

Do not create full settlement system inside Plan 199.

## 199E.15 Return home

Only when:
- source region becomes safe;
- return policy triggers.

## 199E.16 Refugee crisis

Large group can create:
- pending decision;
- capacity pressure.

No automatic “conflict” event.

## 199E.17 Named refugee hooks

Dynamic quest/narrative system may instantiate.

## 199E.18 Anti-farm

Player cannot accept/reject same group repeatedly.

Decision stable.

## 199E.19 Save/load

Pending decision stable.

### 199E DoD

Refugee migration produces real, one-time admission/hosting decisions and world population handoffs without treating a moving group as a permanent duplicate population ledger.

---

# TASK 199F — Seasonal Trader Circuit Integration

# 199F.0 Goal

Make trader availability seasonal while preserving TravelingCaravanSystem and MarketSystem ownership.

## 199F.1 TravelingCaravanSystem audit

Determine:
- route;
- spawn;
- inventory;
- lifetime;
- visit;
- destination.

## 199F.2 Seasonal migration owns schedule eligibility

## 199F.3 Caravan system owns trader entity

## 199F.4 Spawn adapter

Migration episode may call:

```text
SpawnCaravan(...)
```

with:
- route;
- trader profile;
- source episode ID.

## 199F.5 If caravan already exists

Do not spawn duplicate.

## 199F.6 Fixed routes

Seasonal route definitions can:
- activate subsets;
- choose alternates
through caravan route API.

## 199F.7 Trader cargo

Canonical caravan/market authority.

## 199F.8 Market price

MarketSystem.

## 199F.9 Seasonal goods

Could be produced by:
- market supply;
- caravan inventory profile.

Migration only activates the relevant circuit.

## 199F.10 News/intel

Plan 131 can attach:
- traveler news packet.

## 199F.11 Arrival

Existing HoldfastTradeSession/trade session opens.

## 199F.12 Departure

Canonical caravan state.

## 199F.13 Return home

Caravan route.

## 199F.14 Predictability

Player can learn:
- approximate seasonal schedule.

Information visibility policy.

## 199F.15 Missed caravan

No forced popup.

## 199F.16 Nuclear winter

Some trader circuits:
- shut down;
- become rarer;
- reroute.

Data-driven.

## 199F.17 No free goods

## 199F.18 Anti-duplication

One migration episode ↔ one caravan spawn identity.

## 199F.19 Save/load

Do not respawn active caravan.

### 199F DoD

Seasonal trader circuits change when and where canonical caravans appear without duplicating trader inventory, trade sessions, market pricing, or caravan lifecycle.

---

# TASK 199G — Faction Relocation Integration

# 199G.0 Goal

Allow seasonal operational shifts without directly rewriting faction truth or breaking authored content.

## 199G.1 Define relocation scope

Prefer:
- operational presence;
- camp;
- patrol base;
- trading post;
- seasonal HQ.

Do not automatically migrate whole faction population.

## 199G.2 FactionBranchCoordinator audit

Identify:
- branch;
- region;
- activation;
- deactivation;
- territory control.

## 199G.3 Relocation episode

References:
- faction;
- origin operation;
- destination operation.

## 199G.4 Canonical faction API

Use:
- relocate operation;
- activate seasonal branch;
- set presence
if supported.

## 199G.5 No direct territory field mutation

## 199G.6 Territory consequences

Faction authority decides.

## 199G.7 Quest-critical location protection

Build reservation matrix.

If location/faction presence is required by active/mainline content:
- relocation blocked;
- alternative presence kept;
- quest migrates explicitly.

## 199G.8 Summer camp / winter bunker

Good model if authored.

## 199G.9 Seasonal patrols

Could alter:
- encounter availability
through faction system.

## 199G.10 Strategic opportunities

Result from:
- changed presence;
- route access;
- trade;
- diplomacy.

Do not add generic bonus.

## 199G.11 Negotiation

Use canonical faction/dialogue/quest system.

Migration exposes:
- relocation pending;
- relocation active.

## 199G.12 Player influence

Could alter route/destination only through faction diplomacy if supported.

## 199G.13 Nuclear winter

Factions may:
- retreat;
- consolidate.

## 199G.14 No faction teleport

If operation relocation is abstract:
- transition can still have departure/arrival schedule.
If individuals are simulated:
- use world route.

## 199G.15 Mainline continuity test

Mandatory.

## 199G.16 Save/load

Faction state and migration episode reconcile.

### 199G DoD

Faction seasonal relocation changes canonical operational presence through faction APIs while preserving mainline content, territory authority and faction-state consistency.

---

# TASK 199H — Nomadic Circuits, Guides & Information

# 199H.0 Goal

Make nomadic groups recurring mobile world actors using real service, trade and intelligence systems.

## 199H.1 Nomadic route

Waypoint loop through canonical graph.

## 199H.2 Resource following

Read:
- water;
- forage;
- trade;
- safety
only if those regional facts exist.

## 199H.3 Do not invent regional resources solely for this plan

## 199H.4 Visit

Temporary presence at settlement/location.

## 199H.5 Trade

Use caravan/trade adapter.

## 199H.6 Information

Plan 131.

## 199H.7 Cultural exchange

Narrative/relationship hook.

No generic “culture points.”

## 199H.8 Guide hiring

Only if expedition system supports:
- guide modifier;
- temporary companion;
- route knowledge.

## 199H.9 No bespoke guide bonus

## 199H.10 Guide contract

Use service/companion/hire system if real.

## 199H.11 Return circuit

Persistent route schedule.

## 199H.12 Familiarity

Player can learn recurring timing.

## 199H.13 Unknown groups

Hidden until:
- encountered;
- reported;
- radio/intel.

## 199H.14 Interaction

Trade/dialogue.

## 199H.15 Long-term settlement

Follow refugee/permanent settlement handoff.

### 199H DoD

Nomadic circuits become recurring mobile sources of trade, information and possible guide services without creating a parallel culture, service or expedition-modifier system.

---

# TASK 199I — Weather, Nuclear Winter & Environmental Coupling

# 199I.0 Goal

Make seasonal migration responsive to the actual environment rather than a scripted month table.

## 199I.1 WeatherSystem

Provides:
- current/forecast conditions.

## 199I.2 Season

Canonical calendar.

## 199I.3 Route weather exposure

World route/travel authority.

## 199I.4 Temperature thresholds

Route/pattern-specific.

## 199I.5 Radiation

Regional/source hazard.

## 199I.6 Nuclear winter

Plan 164 state.

## 199I.7 Extended winter

Pattern duration follows actual state.

## 199I.8 No infinite refugee spawn

Nuclear winter increases:
- eligible routes/rates,
bounded by:
- source population;
- cooldown;
- group caps.

## 199I.9 Source population budget

Important.

If world has regional populations:
- migration draws from them.

If not:
- authored route annual/seasonal population budget.

Do not spawn infinite humans from nowhere.

## 199I.10 Return suppression

If home remains unsafe:
- no return.

## 199I.11 Trader shutdown

Severe conditions can suspend route.

## 199I.12 Route thaw/reopen

Season can reopen.

## 199I.13 Forecast-driven departure

Could leave before harsh weather only if source society has forecast information.

## 199I.14 Information asymmetry

Not every group has perfect weather forecast.

## 199I.15 Wildlife coordination

Human and wildlife migrations can share:
- environmental trigger data.

Do not make one mutate the other directly unless ecological system consumes it.

### 199I DoD

Migration pressure follows real seasonal, radiation and nuclear-winter conditions with finite source populations and route viability instead of scripted infinite calendar spawning.

---

# TASK 199J — Economy, Trade, Expedition & World-Consequence Integration

# 199J.0 Goal

Make migration affect gameplay through existing economic and world systems.

## 199J.1 Refugee arrival

Possible downstream:
- population pressure;
- labor opportunity;
- food demand;
- housing demand.

Only after admission.

## 199J.2 No arrival-side inventory subtraction

## 199J.3 Trader arrival

Creates:
- market/trade opportunity.

## 199J.4 Trade boom

MarketSystem should derive:
- stock/supply shifts
if supported.

Migration does not directly multiply prices.

## 199J.5 Trader absence

May reduce availability.

## 199J.6 Faction relocation

May change:
- route security;
- trade access;
- encounter pools.

Faction/expedition systems consume.

## 199J.7 Nomad presence

May add:
- guide;
- intel;
- market offer.

## 199J.8 Expedition safety

Source proposes migration affects safety.

Preferred:
- expedition risk system queries mobile group/faction presence.

No direct `safety += migrantGroup`.

## 199J.9 Refugee road presence

Could:
- create encounter;
- information;
- escort request
via encounter/quest system.

## 199J.10 Plan 133 persistent world consequences

Use:
- arrivals;
- settlements;
- departures
as semantic inputs.

## 199J.11 Plan 192 trade routes

Player-established route may influence:
- trader corridor attractiveness;
- service access
if Plan 192 exposes API.

Migration does not own player route.

## 199J.12 Plan 160 colonies

Migrants may target colony if:
- colony accepts visitors/population.

Use colony API.

## 199J.13 Plan 131 information

Migration itself is news:
- exodus;
- trader arrival;
- faction move.

Plan 131 owns propagation.

## 199J.14 No universal cultural-friction event

Conflict requires:
- resource/political/social cause
from relevant systems.

## 199J.15 No automatic refugee conflict

## 199J.16 Scarcity

Migration can contribute to demand only after actual population presence/admission.

## 199J.17 World map

Shows known migration presence.

## 199J.18 Route discoveries

No map reveal unless player learns it.

### 199J DoD

Migration produces real population, trade, faction, expedition and information consequences by feeding canonical systems rather than directly changing their outputs.

---

# TASK 199K — Migration Events, Narrative Hooks & Player Decisions

# 199K.0 Goal

Expose meaningful movement milestones without turning daily travel into an event feed.

## 199K.1 Semantic engine events

Candidate:

```text
migration_episode_started
migrant_group_arrived
migrant_group_departed
migrant_group_diverted
refugee_admission_requested
refugee_admission_resolved
trader_circuit_arrived
faction_relocation_started
faction_relocation_completed
nomadic_group_arrived
```

## 199K.2 No daily movement event

## 199K.3 Source narrative names

- The Exodus;
- The Arrival;
- The Return;
- The Settlement;
- The Conflict;
- The Scarcity;
- The Trade;
- The Relocation.

Treat as authored content candidates.

## 199K.4 Large exodus

Can trigger narrative if:
- group size/region significance threshold.

## 199K.5 Arrival

Only notify player if:
- known;
- relevant;
- near controlled settlement.

## 199K.6 Return

May be history/news.

## 199K.7 Settlement

Only after canonical world settlement handoff.

## 199K.8 Conflict

Must be emitted by actual conflict/social/resource authority.

Migration can provide co-presence trigger.

## 199K.9 Scarcity

Canonical economy/logistics source.

## 199K.10 Trade

Trader arrival event.

## 199K.11 Relocation

Faction event.

## 199K.12 Quest hooks

Plan 171 owns dynamic quests.

Expose:
- refugee arrival;
- route blocked;
- trader delayed;
- faction relocation;
- settlement request.

## 199K.13 Source quests

Treat:
- Host;
- Trader;
- Guide;
- Diplomat;
- Route;
- Settlement;
- Network
as backlog.

## 199K.14 Avoid grind counters

“Accept 10 refugee groups” can incentivize gaming humanitarian choices.

Prefer authored contextual goals.

## 199K.15 No moral score inside migration

### 199K DoD

Migration surfaces meaningful departures, arrivals and decisions as semantic events while narrative consequences remain authored and non-grindy.

---

# TASK 199L — UI, Map, Visibility & Player Workflow

# 199L.0 Goal

Make migration strategically legible without giving omniscient access to every moving group.

## 199L.1 Migration map

Use canonical world map.

Overlay:
- known active migration corridors;
- known group positions;
- expected arrivals.

## 199L.2 No second map coordinate system

## 199L.3 Visibility

Possible states:

```text
unknown
rumored
reported
confirmed
tracked
```

Use Plan 131/intel if available.

## 199L.4 Rumored route

Show approximate corridor, not exact group position.

## 199L.5 Confirmed group

Show latest known location/time.

## 199L.6 Stale intel

Position may become stale.

## 199L.7 No perfect live tracking unless source allows

## 199L.8 Group detail

Show only known:
- type;
- approximate size;
- composition;
- affiliation;
- destination;
- ETA;
- current status.

## 199L.9 Morale/supplies

Do not expose if not modeled.

## 199L.10 Route panel

Show:
- seasonality;
- known schedule;
- route status;
- expected next activation.

## 199L.11 Refugee panel

Prefer existing settlement/admission UI.

Migration notification deep-links.

## 199L.12 Trader panel

Prefer existing caravan/trade UI.

## 199L.13 Faction relocation

Faction/map UI.

## 199L.14 Event log

Use semantic migration history.

## 199L.15 Filters

- refugees;
- traders;
- factions;
- nomads.

## 199L.16 Timeline

Upcoming known arrivals.

## 199L.17 Calendar integration

Seasonal schedule can appear on calendar if known.

## 199L.18 Tutorial

First meaningful migration encounter.

## 199L.19 Tooltips

Not hover-only.

## 199L.20 Accessibility

- route line + labels;
- no color-only type;
- keyboard/controller;
- list alternative;
- text scaling.

## 199L.21 Map clutter budget

Many groups should cluster/aggregate.

## 199L.22 No hidden-state spoilers

### 199L DoD

The player can plan around known seasonal movement through the existing map/calendar/trade/admission surfaces without receiving omniscient tracking or a duplicate management UI.

---

# TASK 199M — Persistence, Migration & Idempotence

# 199M.0 Goal

Save active migration cleanly without duplicating downstream world state.

## 199M.1 Persist

Suggested:

```text
schema_version
season_cycle_sequence
route_activation_state[]
active_migration_episodes[]
group_refs/minimal transient group state
processed_transition_ids[]
migration-specific cooldowns
```

## 199M.2 Do not persist duplicate

- settlement population;
- admitted survivors;
- caravan inventory;
- market prices;
- faction standing;
- territory;
- expedition hazards;
- weather;
- radiation.

## 199M.3 Active caravan linkage

Persist:
- caravan ID reference.

## 199M.4 Faction relocation linkage

Persist:
- faction operation transition ID.

## 199M.5 Refugee pending decision

Persist:
- group ID;
- pending handoff state;
- decision ID.

## 199M.6 Once admitted

Canonical population owns new state.

Migration keeps:
- completed episode/history ref only.

## 199M.7 Old save

Initialize:
- no historical migrations;
- no in-flight groups.

## 199M.8 Current-season activation

After migration feature becomes active:
- schedule next valid episode.
Do not retroactively spawn groups that “should have departed” months earlier.

## 199M.9 Optional reconstruction

Only if:
- deterministic;
- no world-state consequences;
- explicitly desired.

Default:
- no.

## 199M.10 Restore order

After:
- calendar;
- world topology;
- weather/hazards;
- factions;
- caravans;
- settlements
or use two-phase reconciliation.

## 199M.11 Missing linked caravan

Reconcile:
- respawn only if canonical caravan state says absent and episode requires.
Avoid duplicate.

## 199M.12 Missing route

Cancel/divert safely with migration note.

## 199M.13 Content version update

Route definitions can change.

Active episode keeps:
- resolved path snapshot
or migration rule.

## 199M.14 Processed transition IDs

Prevent:
- duplicate arrivals;
- duplicate admissions;
- duplicate trader spawns;
- duplicate faction relocation.

## 199M.15 History

Bounded semantic summaries.

### 199M DoD

Migration-specific schedules and in-flight episodes persist exactly once while all permanent downstream effects remain in their canonical save sections.

---

# TASK 199N — Determinism, Exploit Prevention & Population Conservation

# 199N.0 Goal

Prevent reload rerolls, infinite humans, duplicate caravans and migration farming.

## 199N.1 Seeded generation

Only where stochastic.

Seed:

```text
campaign_seed
+ route_id
+ season_cycle
+ activation_sequence
```

## 199N.2 Group size

Stable after spawn.

## 199N.3 Composition

Stable.

## 199N.4 Route path

Stable unless real route invalidation.

## 199N.5 Reload

No spawn reroll.

## 199N.6 Seasonal toggling

Player cannot manipulate season setting/debug to farm groups in normal play.

## 199N.7 Condition threshold

Hysteresis/cooldown.

## 199N.8 Population source budget

If regional population exists:
- decrement/transfer through authority.

If not:
- per-route seasonal cap.

## 199N.9 No infinite refugee creation

## 199N.10 Admission duplication

One group decision exactly once.

## 199N.11 Caravan duplication

One migration episode ↔ one caravan ID.

## 199N.12 Trader farm

Leaving/re-entering settlement cannot respawn same caravan inventory.

## 199N.13 Faction relocation farm

Repeated travel cannot replay relocation consequences.

## 199N.14 Nomad guide farm

Service/hire system owns cooldown/contract.

## 199N.15 Route establishment

Plan 192 owns player route creation.

No duplicate reward from toggling.

## 199N.16 Return migration

Population conservation.

## 199N.17 Settled group

Removed from migration pool.

## 199N.18 Dispersed group

Resolve count according to world-population policy.

## 199N.19 No GUID

## 199N.20 No wall clock

### 199N DoD

Migration populations, caravans, arrivals and returns are stable, bounded and non-farmable under save/load, repeated visits and long seasonal cycles.

---

# TASK 199O — Performance & Scheduling

# 199O.0 Goal

Support long campaigns and many route definitions without expensive daily world scans.

## 199O.1 Route activation scheduler

Index by:
- season window;
- next eligible day.

## 199O.2 Do not evaluate every predicate every frame

## 199O.3 Daily boundary

Acceptable for:
- active group travel;
- seasonal trigger check.

## 199O.4 Event-driven triggers

For:
- nuclear winter;
- radiation catastrophe;
- faction conflict;
- route closure.

## 199O.5 Active groups

O(active groups) daily.

## 199O.6 Route count

Authored route definitions can be many.

Activation indexes prevent unnecessary scans.

## 199O.7 Pathfinding

Do not recompute daily.

Snapshot/cache.

## 199O.8 Reroute

Only on invalidation/decision.

## 199O.9 Map rendering

Only visible/known groups.

## 199O.10 History retention

Bounded.

## 199O.11 Group abstraction

Do not instantiate every migrant as full survivor while en route unless needed.

## 199O.12 Large flow

One aggregate group, not 300 entities.

## 199O.13 Arrival materialization

Only canonical population system decides individual instantiation.

## 199O.14 Benchmark

Measure:
- 10 routes;
- 100 routes;
- 100 active groups synthetic.

### 199O DoD

Seasonal migration scales with active movement rather than total theoretical population and avoids repeated route-finding or per-frame scans.

---

# TASK 199P — Long-Horizon Balance & World Simulation

# 199P.0 Goal

Prove that migration creates seasonal texture without destabilizing population, economy, faction continuity or content reachability.

## 199P.1 30-day scenario

If one season shorter than 30 days:
- observe one seasonal transition.

Track:
- activations;
- arrivals;
- trader visits;
- refugee flows.

## 199P.2 120-day campaign

Track:
- full seasonal cycle(s);
- route use;
- return flows;
- trader seasonality;
- faction presence.

## 199P.3 180-day campaign

Track:
- multiple cycles;
- refugee settlement pressure;
- market availability;
- route saturation.

## 199P.4 400-day soak

Track:
- population conservation;
- no route drift;
- no ID collision;
- state size;
- history size;
- repeated seasonal returns.

## 199P.5 Stable climate

Low migration.

## 199P.6 Harsh winter

Higher refuge movement.

## 199P.7 Nuclear winter

Sustained pressure but finite source budget.

## 199P.8 Radiation crisis

Localized displacement.

## 199P.9 Faction war

Refugee/faction relocation if canonical conflict exists.

## 199P.10 Peaceful summer

Trade/nomadic circuits dominate.

## 199P.11 Blocked corridor

Reroute/wait.

## 199P.12 No alternate route

Turn back/divert/cancel.

## 199P.13 Player shelter full

Refugee group cannot be silently absorbed.

## 199P.14 Player rejects

Group persists/moves on.

## 199P.15 Player accepts

Population count transfers once.

## 199P.16 Trader route

No duplicate caravan stock.

## 199P.17 Faction relocation

No mainline break.

## 199P.18 Hidden migration

Player UI does not reveal it.

## 199P.19 Wildlife overlap

No accidental shared-state mutation.

## 199P.20 Economy

No runaway seasonal price oscillation caused by duplicate modifiers.

---

# TASK 199Q — Testing & CI

# 199Q.0 Goal

Make migration authority, reachability, persistence and world consequences continuously verifiable.

## 199Q.1 Data integrity

Validate:
- route IDs;
- origin/destination;
- route selectors;
- season patterns;
- group profiles;
- faction refs;
- location refs;
- consumer adapters;
- localization.

## 199Q.2 Selftest

Create:

```text
--seasonal-migration-selftest
```

## 199Q.3 Selftest scenarios

At least:

1. stable season no migration;
2. seasonal trader route;
3. refugee flow;
4. nuclear winter flow;
5. radiation displacement;
6. blocked route;
7. reroute;
8. no alternate route;
9. refugee arrival;
10. accept once;
11. reject/move-on;
12. trader caravan linkage;
13. trader save/load no duplicate;
14. faction relocation;
15. mainline reservation block;
16. nomad circuit;
17. hidden intel;
18. old save;
19. save/load active group;
20. 100-revision/day idempotence if applicable;
21. headless.

## 199Q.4 Source-scan authority gate

Detect:
- duplicate market price;
- duplicate caravan inventory;
- direct faction standing;
- direct territory mutation;
- duplicate permanent population;
- independent world graph;
- per-frame movement;
- unseeded RNG.

## 199Q.5 Content acceptance

Route ladder:

```text
DISCOVERED
LOADED
REGISTERED
ORIGIN_RESOLVED
DESTINATION_RESOLVED
CANONICAL_PATH_RESOLVED
TRIGGER_REACHABLE
EPISODE_CREATED
ARRIVAL_CONSUMER_REACHED
OUTCOME_OBSERVED
```

## 199Q.6 Dead-route gate

No route:
- with impossible trigger;
- unresolved destination;
- no consumer.

## 199Q.7 Dead-migration-type gate

No type without integration.

## 199Q.8 Consumer gate

Refugee:
- admission.

Trader:
- caravan/trade.

Faction:
- faction operation.

Nomad:
- trade/intel/guide.

## 199Q.9 Golden migration fixtures

Fixed season/world seed:
- exact episode IDs;
- routes;
- group sizes;
- arrival days.

## 199Q.10 Determinism fingerprint

Same state:
- same active groups/order.

## 199Q.11 Population-conservation test

## 199Q.12 Caravan-duplication test

## 199Q.13 Mainline-content test

## 199Q.14 Performance benchmark

## 199Q.15 Generated docs

Create:
- `SEASONAL_MIGRATION_ARCHITECTURE.md`;
- `SEASONAL_MIGRATION_AUTHORITY_MATRIX.md`;
- `MIGRATION_ROUTE_MATRIX.md`;
- `MIGRATION_GROUP_PROFILE_MATRIX.md`;
- `MIGRATION_TRIGGER_MATRIX.md`;
- `MIGRATION_CONSUMER_MATRIX.md`;
- `MIGRATION_VISIBILITY_MATRIX.md`;
- `MIGRATION_PERSISTENCE_MATRIX.md`;
- `MIGRATION_BALANCE_REPORT.md`;
- `ADR_SEASONAL_MIGRATION_AND_WORLD_TOPOLOGY.md`;
- `ADR_MIGRANT_GROUP_VS_SETTLEMENT_POPULATION.md`;
- `ADR_SEASONAL_TRADER_CIRCUITS_VS_TRAVELING_CARAVANS.md`;
- `ADR_FACTION_RELOCATION_AUTHORITY.md`.

### 199Q DoD

Every migration route and group can be traced from a real seasonal/world trigger through canonical travel to a real arrival consumer, with deterministic, bounded, save-safe outcomes.

---

# TASK 199R — Advanced Mass Migration, Settlement Networks & Regional Demography: Explicit Follow-On

# 199R.0 Goal

Prevent Plan 199 from becoming a full grand-strategy population simulator before the route/arrival layer is proven.

## 199R.1 Regional population pools

Potential follow-on if world population authority exists.

## 199R.2 Birth/death demography

Not migration responsibility.

## 199R.3 Full settlement population simulation

Separate plan.

## 199R.4 Refugee camps

Only if:
- temporary settlement/visitor system supports.

## 199R.5 Humanitarian aid routes

Plan 192/trade/quest integration.

## 199R.6 Migration treaties

Governance/faction diplomacy.

## 199R.7 Border controls

Faction/world policy.

## 199R.8 Smuggling migration routes

Plan 155/192.

## 199R.9 Mass evacuation

Plan 158 emergency response.

## 199R.10 Migration legacy

Plan 162 archive.

## 199R.11 Historic exodus

Narrative/archive.

## 199R.12 Resettlement programs

Settlement/governance.

## 199R.13 Labor migration

Economy/workforce follow-on.

## 199R.14 Disease spread via migration

DiseaseSystem can consume traveler contact.

Do not simulate disease inside migration.

## 199R.15 Cultural diffusion

Relationship/narrative/faction systems.

No generic culture meter by default.

## 199R.16 Migration route trade

Reframe:
- agreements/route access;
Plan 192.

### 199R DoD

Advanced demographic features remain explicit integrations into population, governance, diplomacy, trade, disease and archive authorities instead of expanding SeasonalMigration into a universal society simulator.

---

# 5. Core Migration Lifecycle

```text
ROUTE DEFINITION
      │
      ▼
TRIGGER ELIGIBLE
      │
      ▼
EPISODE SCHEDULED
      │
      ▼
GROUP DEPARTS
      │
      ▼
CANONICAL ROUTE TRAVEL
      │
      ├── proceed
      ├── wait
      ├── reroute
      ├── divert
      └── turn back
      │
      ▼
ARRIVAL
      │
      ├── refugee admission
      ├── trader caravan visit
      ├── faction presence handoff
      └── nomad visit
      │
      ▼
RETURN / SETTLEMENT HANDOFF / COMPLETION
```

---

# 6. World Topology Contract

Migration never owns:
- adjacency;
- route distance;
- hazard truth.

It resolves through the existing world graph.

---

# 7. Seasonal Contract

Migration reads:
- season;
- date;
- nuclear winter.

It does not advance them.

---

# 8. Route Contract

A migration route is:
- seasonal intent/corridor.

It is not:
- a second world path graph.

---

# 9. Group Contract

A migrant group is:
- temporary aggregate movement entity.

It is not:
- permanent settlement population.

---

# 10. Population Contract

At arrival:

```text
external migrant count
→ canonical admission/settlement handoff
→ population authority
```

Exactly once.

---

# 11. Refugee Contract

Migration owns:
- arrival episode.

Settlement/governance owns:
- accept/reject/host.

---

# 12. Trader Contract

Migration owns:
- seasonal route activation.

TravelingCaravanSystem owns:
- caravan entity;
- inventory;
- travel lifecycle if already sufficient.

MarketSystem owns:
- prices.

---

# 13. Faction Relocation Contract

Migration owns:
- seasonal relocation intent/episode.

Faction authority owns:
- actual operation/presence/territory consequence.

---

# 14. Nomad Contract

Nomad presence can expose:
- trade;
- intel;
- guides
through canonical systems.

---

# 15. Weather Contract

Weather modifies:
- eligibility;
- route viability;
- travel duration.

Weather truth remains WeatherSystem.

---

# 16. Radiation Contract

Radiation can trigger displacement or block routes.

RadiationSystem owns hazard state.

---

# 17. Scarcity Contract

Migration reads canonical scarcity.

After admission, population/economy systems experience demand.

No abstract migration-owned resource drain.

---

# 18. Conflict Contract

Faction pressure can trigger migration.

Migration does not calculate conflict outcomes.

---

# 19. Expedition Contract

Migration exposes:
- presence on route/location.

ExpeditionSystem decides:
- encounters;
- safety;
- opportunities.

---

# 20. Intelligence Contract

Player only knows migration that has been:
- observed;
- reported;
- inferred
through Plan 131/intel.

---

# 21. Wildlife Contract

Wildlife and human migration may share seasonal conditions.

They maintain separate population/state authorities.

---

# 22. Plan 192 Contract

Player trade routes may influence:
- corridor attractiveness;
- trader access.

Plan 192 owns route creation.

---

# 23. Plan 160 Contract

Colonies may be destinations only through colony APIs.

---

# 24. Plan 133 Contract

Permanent world changes from settlement/relocation flow through existing world consequence systems.

---

# 25. Plan 171 Contract

Migration emits triggers.

Dynamic quest runtime generates quests.

---

# 26. Plan 162 Contract

Major migrations can be archived.

Routine movements should not flood archive.

---

# 27. Persistence Matrix

| Fact | Owner |
|---|---|
| season | calendar |
| weather | WeatherSystem |
| radiation | RadiationSystem |
| topology | world route system |
| expedition hazards | ExpeditionSystem/world hazard |
| settlement population | settlement/population |
| survivor identity | survivor lifecycle |
| caravan identity | TravelingCaravanSystem |
| caravan cargo | caravan/inventory |
| market price | MarketSystem |
| faction standing | faction |
| faction territory | faction/world state |
| migration route definition | migration data |
| active migration episode | SeasonalMigrationSystem |
| migration group transient count/profile | SeasonalMigrationSystem |
| admission result | settlement/governance |
| permanent settlement | settlement/world evolution |
| intel visibility | Plan 131 |
| archive | Plan 162 |

---

# 28. Old-Save Migration

Default:

```text
migration schema absent
active episodes = []
route cooldowns = initialized from feature activation day
historical migration events = []
```

Do not:
- reconstruct prior seasons;
- create refugees on first load because winter already began;
- relocate factions retroactively.

Next normal trigger window starts from current state.

---

# 29. Stable Identity Contract

Examples:

```text
route:refugee_north_valley_to_holdfast
episode:route_id:year_or_cycle:sequence
group:episode_id:group_profile
arrival:episode_id:destination
admission:group_id:settlement
caravan_link:episode_id:caravan_id
```

---

# 30. Trigger Hysteresis Contract

Condition-based flow:

```text
inactive
→ threshold sustained
→ eligible
→ episode spawned
→ cooldown
→ rearm after exit/reentry
```

No daily repeated spawning around same threshold.

---

# 31. Source Population Conservation

Preferred:

```text
regional source population
- departing group
+ returning group
```

If no population authority:
- authored seasonal route quota.

Never infinite creation.

---

# 32. Return Contract

Return occurs only when:
- policy says;
- route viable;
- home condition acceptable;
- group has not settled/dispersed.

---

# 33. Settlement Contract

Permanent settlement is a separate world-state transition.

Migration episode ends after handoff.

---

# 34. Trader Inventory Contract

Seasonal migration never captures or restores trader stock.

Only caravan/trade authority.

---

# 35. Faction Territory Contract

Relocation does not automatically equal conquest.

Presence and territory are separate.

---

# 36. Visibility Contract

Unknown moving group can exist in simulation without map marker.

---

# 37. Failure Injection Matrix

## N199.1 Migration creates separate graph edge not in world topology
Expected: topology gate fails.

## N199.2 Accepted refugee count remains in migrant group and settlement population
Expected: population conservation fails.

## N199.3 Seasonal trader spawns duplicate caravan after reload
Expected: caravan idempotence fails.

## N199.4 Migration writes MarketSystem price directly
Expected: market authority gate fails.

## N199.5 Faction relocation directly overwrites territory
Expected: faction authority gate fails.

## N199.6 Winter threshold spawns new refugee group every day
Expected: hysteresis/cooldown gate fails.

## N199.7 Route blocked; group teleports to destination
Expected: traversal gate fails.

## N199.8 Old save begins with retroactive exodus and faction moves
Expected: migration parity fails.

## N199.9 Hidden faction relocation appears immediately on player map
Expected: intelligence visibility gate fails.

## N199.10 Nuclear winter creates unbounded refugee population
Expected: population budget gate fails.

## N199.11 Rejected refugee group disappears with no world resolution
Expected: group lifecycle gate fails.

## N199.12 Trader group owns second inventory state
Expected: caravan authority gate fails.

---

# 38. Determinism Contract

Same:

```text
campaign seed
+ calendar/season
+ route definitions
+ source world conditions
+ route viability
+ activation sequence
+ population budgets
```

must produce same:
- route eligibility;
- episode IDs;
- group size/composition;
- departure;
- path;
- arrival;
- return decision.

---

# 39. Long-Horizon Metrics

Track:

```text
route activations
migration episodes
refugee groups
trader circuits
faction relocations
nomadic visits
population moved
population admitted
population rejected
population returned
groups diverted
groups settled
route blocks
reroutes
caravan links
mainline relocation blocks
known vs hidden groups
trade-session opportunities
intel events
state bytes
daily processing time
```

---

# 40. Balance Guardrails

Migration should create:
- predictable seasonal texture;
- occasional crisis;
- economic opportunities;
- strategic planning.

It should not create:
- infinite population;
- constant refugee prompts;
- mandatory trader dependence;
- random faction map churn.

---

# 41. Refugee Frequency Guardrails

Refugee flow should correspond to:
- actual displacement pressure.

Do not create a mandatory group every fall.

---

# 42. Trader Frequency Guardrails

Seasonal circuits should improve planning:
- known approximate windows;
- variable enough to remain dynamic.

---

# 43. Faction Relocation Guardrails

Prefer a few meaningful seasonal operations.

Do not shuffle every faction every season.

---

# 44. Nomad Guardrails

Nomads should feel recurring and mobile.

Avoid turning them into permanent trader clones.

---

# 45. UI Acceptance

## Map
- known routes;
- known groups;
- stale intel.

## Arrival
- refugee decision deep-link;
- trader visit;
- faction change.

## Route panel
- seasonal windows;
- known status;
- next likely movement.

---

# 46. Accessibility

- route type text + icon;
- no color-only lines;
- map list alternative;
- keyboard/controller;
- text scale;
- stale/uncertain intel wording.

---

# 47. Localization

Route display names, group types, status:
- localization keys.

IDs remain stable logic keys.

---

# 48. Content Acceptance

Migration route ladder:

```text
DISCOVERED
LOADED
REGISTERED
TRIGGER_BOUND
ORIGIN_BOUND
DESTINATION_BOUND
PATH_RESOLVED
EPISODE_CREATED
GROUP_TRAVELED
ARRIVAL_CONSUMER_USED
WORLD_EFFECT_OBSERVED
```

---

# 49. Reachability

Every shipped route must prove:
- at least one reachable activation scenario;
- canonical path;
- consumer.

No dead route count padding.

---

# 50. Performance Guardrails

- O(active groups) daily;
- route triggers indexed;
- no per-frame;
- no daily pathfinding;
- aggregate groups;
- bounded history.

---

# 51. CI / Gate Set

Recommended:

```text
seasonal_migration_authority_matrix
seasonal_migration_world_topology_single
seasonal_migration_route_integrity
seasonal_migration_trigger_reachability
seasonal_migration_population_conservation
seasonal_migration_caravan_authority
seasonal_migration_market_authority
seasonal_migration_faction_authority
seasonal_migration_visibility
seasonal_migration_old_save
seasonal_migration_restore_idempotence
seasonal_migration_determinism
seasonal_migration_mainline_continuity
seasonal_migration_performance
seasonal_migration_long_horizon
seasonal_migration_ui_access
```

---

# 52. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --seasonal-migration-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 53. Recommended Commit Breakdown

```text
199A-1 authority/topology/population audit
199A-2 world-topology ADR
199A-3 migrant-vs-population ADR
199A-4 trader-vs-caravan ADR
199A-5 faction-relocation ADR
199A-6 route schema/loader
199A-7 group-profile schema
199A-8 route integrity/docs

199B-1 seasonal pattern schema
199B-2 weather/radiation predicates
199B-3 scarcity/faction-pressure predicates
199B-4 intensity/cooldown/hysteresis
199B-5 deterministic activation
199B-6 population budget
199B-7 trigger tests/docs

199C-1 migration episode DTO
199C-2 stable identities
199C-3 transient group state
199C-4 lifecycle statuses
199C-5 return linkage
199C-6 restore idempotence
199C-7 docs/tests

199D-1 canonical path resolver
199D-2 path snapshot/reroute policy
199D-3 travel progression
199D-4 route block/wait/reroute
199D-5 arrival
199D-6 visibility/presence
199D-7 expedition presence adapter
199D-8 traversal tests

199E-1 refugee trigger/destination
199E-2 pending admission
199E-3 canonical population handoff
199E-4 reject/move-on
199E-5 partial/temporary admission if supported
199E-6 anti-duplication
199E-7 save/load tests
199E-8 docs

199F-1 TravelingCaravan audit
199F-2 seasonal spawn adapter
199F-3 route selection
199F-4 caravan linkage identity
199F-5 market/trade session integration
199F-6 Plan-131 news packet
199F-7 no-duplicate-save tests
199F-8 docs

199G-1 faction operation/territory audit
199G-2 relocation adapter
199G-3 seasonal base definitions
199G-4 mainline reservation matrix
199G-5 relocation completion
199G-6 diplomacy hooks
199G-7 continuity tests
199G-8 docs

199H-1 nomadic circuit schema
199H-2 canonical waypoint route
199H-3 trade adapter
199H-4 intel adapter
199H-5 guide-service adapter if real
199H-6 visibility
199H-7 tests/docs

199I-1 nuclear-winter integration
199I-2 weather/temperature coupling
199I-3 radiation displacement
199I-4 route viability
199I-5 source-population caps
199I-6 return suppression
199I-7 wildlife coordination audit
199I-8 tests/docs

199J-1 economy/trade coupling
199J-2 expedition presence coupling
199J-3 Plan-133 world consequences
199J-4 Plan-192 route influence
199J-5 Plan-160 colony destination
199J-6 Plan-131 information
199J-7 conflict/scarcity authority regressions
199J-8 docs

199K-1 semantic migration events
199K-2 Plan-171 hook surface
199K-3 narrative-event budget
199K-4 anti-grind quest review

199L-1 world-map overlay
199L-2 intel visibility/staleness
199L-3 route panel
199L-4 arrival deep-links
199L-5 event log/calendar
199L-6 accessibility
199L-7 snapshots/tutorial

199M-1 persistence schema
199M-2 linked caravan/faction refs
199M-3 old-save clean start
199M-4 restore reconciliation
199M-5 route-definition migration
199M-6 transition idempotence
199M-7 history compaction
199M-8 migration docs

199N-1 seeded activation
199N-2 population conservation
199N-3 anti-refugee-spawn farm
199N-4 caravan anti-duplication
199N-5 faction replay protection
199N-6 return conservation
199N-7 exploit tests

199O-1 trigger indexes
199O-2 active-group scheduler
199O-3 path caching
199O-4 map aggregation
199O-5 10/100-route benchmark
199O-6 100-group benchmark

199P-1 30-day seasonal test
199P-2 120-day full cycle
199P-3 180-day stress
199P-4 400-day soak
199P-5 nuclear-winter scenario
199P-6 faction-war scenario
199P-7 market/trader scenario
199P-8 report

199Q-1 selftest
199Q-2 content acceptance/reachability
199Q-3 failure fixtures
199Q-4 deterministic goldens
199Q-5 mainline continuity gate
199Q-6 final ship/no-ship report

199R-1 advanced demographic/network follow-on disposition
```

---

# 54. Risk Register

## R199.1 Creates second population simulation

Mitigation:
- transient group + explicit admission handoff.

## R199.2 Trader circuits duplicate caravans

Mitigation:
- TravelingCaravan adapter;
- one episode↔caravan identity.

## R199.3 Faction relocation corrupts territory/mainline

Mitigation:
- faction API only;
- reservation matrix;
- continuity tests.

## R199.4 Seasonal spawning creates infinite humans

Mitigation:
- regional population or authored route budgets.

## R199.5 World topology diverges

Mitigation:
- route corridor binds to canonical graph.

## R199.6 Migration feels like background noise

Mitigation:
- fewer meaningful routes;
- player-visible opportunities/decisions.

## R199.7 Refugee prompts overwhelm player

Mitigation:
- bounded frequency;
- context-based triggers;
- batching/visitor handling if supported.

## R199.8 Map becomes omniscient

Mitigation:
- Plan-131 visibility/staleness.

## R199.9 Economy double counts trader effects

Mitigation:
- MarketSystem only.

## R199.10 Old saves get disruptive retroactive movement

Mitigation:
- clean feature-activation baseline.

---

# 55. Acceptance Checklist

## P0

- [ ] ISimClock audited
- [ ] calendar/season authority audited
- [ ] Plan 38 audited
- [ ] Plan 164 audited
- [ ] WeatherSystem audited
- [ ] RadiationSystem audited
- [ ] world topology audited
- [ ] location/region catalogs audited
- [ ] route catalogs audited
- [ ] ExpeditionSystem audited
- [ ] TravelingCaravanSystem audited
- [ ] caravan identity audited
- [ ] caravan inventory audited
- [ ] trade session audited
- [ ] MarketSystem audited
- [ ] settlement population audited
- [ ] recruitment/admission audited
- [ ] visitor/refugee systems audited
- [ ] FactionBranchCoordinator audited
- [ ] faction territory/presence audited
- [ ] Plan 131 audited
- [ ] Plan 160 audited
- [ ] Plan 192 audited
- [ ] Plan 133 audited
- [ ] save order audited
- [ ] map UI audited
- [ ] authority matrix published
- [ ] topology ADR
- [ ] population ADR
- [ ] caravan ADR
- [ ] faction relocation ADR
- [ ] static-human-world baseline captured

## 199A — Routes/Profiles

- [ ] migration route data versioned
- [ ] stable route IDs
- [ ] display names presentation-only
- [ ] no directional world logic dependency
- [ ] canonical route selector
- [ ] route activation policy
- [ ] no invented months if calendar lacks them
- [ ] return policy
- [ ] one-way refugee support
- [ ] round-trip trader support
- [ ] nomad waypoint loop
- [ ] faction operations rather than whole-population move by default
- [ ] group profiles
- [ ] data-driven group size
- [ ] optional real leader ref
- [ ] no full survivor materialization required
- [ ] route integrity
- [ ] 10+ not treated as quota
- [ ] route matrix
- [ ] group matrix

## 199B — Triggers

- [ ] pattern catalog
- [ ] season windows
- [ ] condition predicates
- [ ] spring tendency data-driven
- [ ] summer tendency data-driven
- [ ] fall tendency data-driven
- [ ] winter tendency data-driven
- [ ] nuclear-winter state canonical
- [ ] temperature trigger canonical
- [ ] radiation trigger canonical
- [ ] scarcity trigger canonical
- [ ] faction pressure canonical
- [ ] season trigger canonical
- [ ] AND/OR semantics
- [ ] intensity is spawn pressure only
- [ ] deterministic seed
- [ ] no reload reroll
- [ ] hysteresis
- [ ] cooldown
- [ ] route concurrency cap
- [ ] destination capacity
- [ ] shelter not universal magnet
- [ ] information asymmetry

## 199C — Episode/Group

- [ ] stable episode DTO
- [ ] route ref
- [ ] group ref
- [ ] origin/destination
- [ ] path state
- [ ] departure
- [ ] return linkage
- [ ] status lifecycle
- [ ] no duplicate survivor list
- [ ] temporary population count semantics
- [ ] composition summary only
- [ ] leader optional
- [ ] supplies external where possible
- [ ] morale not added without consumer
- [ ] stable IDs
- [ ] exactly-once creation
- [ ] return provenance
- [ ] dispersal semantics
- [ ] compact state
- [ ] no narrative text
- [ ] restore no event replay

## 199D — Traversal

- [ ] path resolved from canonical graph
- [ ] path snapshot/reroute ADR
- [ ] canonical travel cost
- [ ] no fake daily one-location progress
- [ ] daily/event boundary movement
- [ ] no per-frame
- [ ] segment progress persisted
- [ ] weather route effects canonical
- [ ] radiation route effects canonical
- [ ] faction block canonical
- [ ] wait/reroute/turn-back/divert
- [ ] no teleport
- [ ] topology invalidation event
- [ ] deterministic recompute
- [ ] arrival after true traversal
- [ ] no invented attrition
- [ ] visibility respects intel
- [ ] expedition encounter predicate
- [ ] no direct expedition safety mutation

## 199E — Refugees

- [ ] trigger uses real displacement
- [ ] destination selection grounded
- [ ] shelter not universal destination
- [ ] pending admission state
- [ ] admission authority identified
- [ ] accept through canonical API
- [ ] population materialization decision
- [ ] no double count
- [ ] partial acceptance only if supported
- [ ] rejected group persists
- [ ] no hardcoded moral/standing penalty
- [ ] accepted people consume canonical resources
- [ ] temporary hosting only if real
- [ ] settlement elsewhere only via world system
- [ ] return if home safe
- [ ] crisis is capacity pressure, not automatic conflict
- [ ] named hooks delegated
- [ ] anti-decision-farm
- [ ] pending decision save-safe

## 199F — Traders

- [ ] TravelingCaravan lifecycle audited
- [ ] migration owns schedule only
- [ ] caravan owns trader entity
- [ ] SpawnCaravan adapter
- [ ] duplicate active caravan prevented
- [ ] seasonal route subset/alternate support
- [ ] cargo canonical
- [ ] prices canonical
- [ ] seasonal goods through market/caravan
- [ ] Plan-131 news
- [ ] trade session canonical
- [ ] departure canonical
- [ ] return canonical
- [ ] approximate schedule knowledge
- [ ] missed caravan nonmodal
- [ ] nuclear winter suspension/reroute
- [ ] no free goods
- [ ] one episode ↔ one caravan
- [ ] reload no duplicate

## 199G — Factions

- [ ] relocation scope defined
- [ ] FactionBranchCoordinator audited
- [ ] operation refs
- [ ] canonical relocation API
- [ ] no direct territory write
- [ ] territory consequence faction-owned
- [ ] quest/mainline reservations
- [ ] summer/winter base data
- [ ] seasonal patrols through faction system
- [ ] opportunities emerge from presence
- [ ] negotiation canonical
- [ ] player influence only via diplomacy
- [ ] nuclear-winter behavior
- [ ] no instant teleport if movement modeled
- [ ] mainline continuity
- [ ] save reconciliation

## 199H — Nomads

- [ ] canonical waypoint loop
- [ ] resource following only if real
- [ ] no invented regional resources
- [ ] temporary visit
- [ ] trade adapter
- [ ] Plan-131 intel
- [ ] no generic culture currency
- [ ] guide service only if expedition supports
- [ ] no bespoke guide multiplier
- [ ] contract/hire system reused
- [ ] recurring circuit
- [ ] player learns timing
- [ ] unknown until discovered
- [ ] trade/dialogue interaction
- [ ] settlement handoff if permanent

## 199I — Environment

- [ ] WeatherSystem source
- [ ] canonical season
- [ ] route weather exposure
- [ ] route-specific temperature
- [ ] radiation source
- [ ] Plan-164 state
- [ ] extended winter
- [ ] no infinite nuclear-winter spawn
- [ ] source population budget
- [ ] unsafe-home return suppression
- [ ] trader shutdown
- [ ] thaw/reopen
- [ ] forecast departure only if group knows forecast
- [ ] information asymmetry
- [ ] wildlife trigger coordination only

## 199J — Consequences

- [ ] population pressure only after admission
- [ ] no arrival inventory subtraction
- [ ] trader arrival opens canonical trade
- [ ] trade boom Market-owned
- [ ] trader absence availability only
- [ ] faction presence affects consumers
- [ ] nomad services canonical
- [ ] expedition queries presence
- [ ] refugee road encounters delegated
- [ ] Plan 133 world consequence hook
- [ ] Plan 192 route influence
- [ ] Plan 160 colony destination
- [ ] Plan 131 information
- [ ] no universal cultural conflict
- [ ] no auto refugee conflict
- [ ] scarcity demand only after actual presence
- [ ] map shows known only
- [ ] no free route reveal

## 199K — Events/Quests

- [ ] semantic migration start
- [ ] arrival
- [ ] departure
- [ ] diversion
- [ ] admission request/resolution
- [ ] trader arrival
- [ ] faction relocation
- [ ] nomad arrival
- [ ] no daily movement event
- [ ] source narrative events treated as content
- [ ] large exodus threshold
- [ ] arrival visibility policy
- [ ] return/news policy
- [ ] settlement only after canonical handoff
- [ ] conflict source-owned
- [ ] scarcity source-owned
- [ ] trade event canonical
- [ ] relocation event canonical
- [ ] Plan 171 hooks
- [ ] source quest counters reviewed
- [ ] no humanitarian grind incentive
- [ ] no migration moral score

## 199L — UI

- [ ] canonical world map overlay
- [ ] no second coordinate system
- [ ] visibility states
- [ ] rumored corridor approximate
- [ ] confirmed position
- [ ] stale intel
- [ ] no perfect tracking
- [ ] detail only known fields
- [ ] no fake morale/supplies display
- [ ] route panel
- [ ] refugee deep-link
- [ ] trader deep-link
- [ ] faction map integration
- [ ] event log
- [ ] filters
- [ ] timeline
- [ ] calendar
- [ ] tutorial
- [ ] no hover-only
- [ ] accessible route encoding
- [ ] list alternative
- [ ] keyboard/controller
- [ ] text scale
- [ ] map clutter bound
- [ ] no hidden-state spoilers

## 199M — Persistence

- [ ] schema version
- [ ] season cycle sequence
- [ ] route activation state
- [ ] active episodes
- [ ] transient groups only
- [ ] processed transitions
- [ ] cooldowns
- [ ] no population duplicate
- [ ] no admitted survivor duplicate
- [ ] no caravan inventory duplicate
- [ ] no market duplicate
- [ ] no faction standing duplicate
- [ ] no territory duplicate
- [ ] no expedition hazard duplicate
- [ ] no weather/radiation duplicate
- [ ] caravan link
- [ ] faction relocation link
- [ ] pending refugee decision
- [ ] admitted group closes/transfers
- [ ] old save no history
- [ ] old save no in-flight groups
- [ ] next trigger from activation
- [ ] no retroactive spawn
- [ ] restore ordering
- [ ] missing caravan reconciliation
- [ ] missing route safe handling
- [ ] active path survives content update
- [ ] transition idempotence
- [ ] bounded history

## 199N — Determinism/Exploit

- [ ] seeded activation
- [ ] stable group size
- [ ] stable composition
- [ ] stable path
- [ ] reload no reroll
- [ ] season toggle not exploitable
- [ ] threshold hysteresis
- [ ] source population budget
- [ ] no infinite refugees
- [ ] admission exactly once
- [ ] caravan exactly once
- [ ] no trader reload farm
- [ ] no faction replay
- [ ] guide service canonical
- [ ] Plan 192 route creation canonical
- [ ] return conservation
- [ ] settled group removed
- [ ] dispersed count policy
- [ ] no GUID
- [ ] no wall clock

## 199O — Performance

- [ ] activation scheduler indexed
- [ ] no frame trigger scans
- [ ] daily active travel only
- [ ] event-driven disasters/conflicts
- [ ] O(active groups)
- [ ] route definitions indexed
- [ ] no daily pathfinding
- [ ] reroute only on invalidation
- [ ] map known-only
- [ ] history bounded
- [ ] aggregate groups
- [ ] no every-person materialization
- [ ] arrival materialization canonical
- [ ] 10-route benchmark
- [ ] 100-route benchmark
- [ ] 100-group benchmark

## 199P/Q — Simulations/CI

- [ ] 30-day scenario
- [ ] 120-day cycle
- [ ] 180-day campaign
- [ ] 400-day soak
- [ ] stable climate
- [ ] harsh winter
- [ ] nuclear winter
- [ ] radiation crisis
- [ ] faction war
- [ ] peaceful summer
- [ ] blocked corridor
- [ ] no alternate route
- [ ] shelter full
- [ ] reject
- [ ] accept
- [ ] trader no duplicate
- [ ] faction mainline continuity
- [ ] hidden migration
- [ ] wildlife coexistence
- [ ] economy no duplicate modifier
- [ ] data integrity
- [ ] seasonal-migration selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] dead-route gate
- [ ] dead-type gate
- [ ] consumer gate
- [ ] deterministic goldens
- [ ] population conservation
- [ ] caravan duplication
- [ ] mainline continuity
- [ ] performance
- [ ] generated docs
- [ ] verify-fast

## 199R — Follow-On

- [ ] regional population pools only with population authority
- [ ] birth/death demography out
- [ ] full settlement simulation out
- [ ] refugee camps only with visitor/settlement system
- [ ] humanitarian routes use Plan 192
- [ ] treaties use diplomacy/governance
- [ ] border controls faction-owned
- [ ] smuggling uses Plan 155/192
- [ ] mass evacuation uses Plan 158
- [ ] migration legacy uses Plan 162
- [ ] resettlement uses settlement/governance
- [ ] labor migration separate economy/workforce feature
- [ ] disease spread uses DiseaseSystem
- [ ] cultural diffusion uses narrative/relations
- [ ] route agreements use Plan 192

---

# 56. Ship / No-Ship Gate

**SHIP** only if:

```text
world_topology_authorities == 1
AND settlement_population_authorities == 1
AND trader_caravan_authorities == 1
AND faction_state_authorities == 1
AND migration_owned_market_price == false
AND migration_owned_caravan_inventory == false
AND migration_owned_faction_standing == false
AND migration_direct_territory_mutation == false
AND migration_owned_permanent_population == false
AND migration_owned_admitted_survivor_needs == false
AND migration_parallel_world_graph_edges == 0
AND duplicate_refugee_population_after_admission == 0
AND duplicate_caravans_from_migration == 0
AND daily_threshold_spawn_farms == 0
AND route_teleports == 0
AND infinite_unbudgeted_human_spawns == 0
AND hidden_migration_information_leaks == 0
AND mainline_breaking_faction_relocations == 0
AND per_frame_migration_processing == 0
AND unseeded_migration_rng == 0
AND old_save_retroactive_migration_side_effects == 0
AND dead_route_definitions == 0
AND dead_migration_type_integrations == 0
AND seasonal_migration_old_save == pass
AND seasonal_migration_save_roundtrip == pass
AND seasonal_migration_population_conservation == pass
AND seasonal_migration_caravan_idempotence == pass
AND seasonal_migration_faction_continuity == pass
AND seasonal_migration_visibility == pass
AND seasonal_migration_determinism == pass
AND seasonal_migration_30_day_balance == pass
AND seasonal_migration_120_day_balance == pass
AND seasonal_migration_180_day_balance == pass
AND seasonal_migration_400_day_soak == pass
AND seasonal_migration_performance == pass
AND seasonal_migration_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 57. Implementer Handoff

1. Audit the actual world graph, calendar, caravan, settlement population, faction, market, expedition and intelligence authorities before writing SeasonalMigration.
2. Treat migration routes as seasonal corridor **definitions**, not a second navigation graph.
3. Bind every route to canonical world topology and prove the path resolves.
4. Use canonical calendar/season/nuclear-winter state; do not invent months if ASHFALL does not actually model months.
5. Make migration episode IDs stable and deterministic.
6. Keep migrant groups abstract while en route unless a named/story actor actually requires survivor-level representation.
7. Do not create generic group morale/supplies bars unless a real group logistics authority consumes them.
8. Build source-population budgets or route quotas so harsh seasons cannot create infinite humans.
9. Use hysteresis and cooldowns so one threshold cannot spawn a new group every day.
10. Move groups through canonical travel costs and hazards.
11. Reroute only when real topology/hazard events invalidate the route.
12. Never teleport a blocked group.
13. For refugees, create a pending arrival/admission handoff.
14. Transfer accepted people to the canonical population/recruitment system exactly once.
15. Keep rejected groups in the world until they move on, divert, return or otherwise resolve.
16. Do not apply automatic moral, faction or resource penalties inside migration; let the owning systems react.
17. For trader circuits, use TravelingCaravanSystem as the actual trader entity and inventory owner.
18. Link one seasonal episode to one caravan identity to prevent reload duplication.
19. Let MarketSystem own prices and seasonal supply effects.
20. For faction relocation, move operations/presence through faction APIs and never write territory directly.
21. Build a reservation matrix protecting active/mainline-critical faction locations.
22. For nomads, reuse trade, intel and expedition-guide systems where real.
23. Respect Plan-131 information asymmetry: unknown migration exists without omniscient map markers.
24. Let migration produce expedition presence predicates rather than direct safety multipliers.
25. Persist only migration-specific routes/episodes/progress/cooldowns and external system references.
26. Migrate old saves with no fabricated migration history or retroactive group movements.
27. Run 30/120/180/400-day simulations for population conservation, seasonal trader availability, faction continuity and state growth.
28. Reject every route that cannot activate, resolve a path and reach a real arrival consumer.
29. Close only when seasonal movement changes what the player can encounter, trade with, shelter, negotiate with and plan around—without creating a second world simulation.

---

# 58. Final Outcome

When this plan is complete, ASHFALL's human world will finally move with the seasons.

Spring can reopen corridors that winter closed. Trader circuits can resume. Some displaced groups may attempt to return home if radiation and faction pressure have genuinely eased.

Summer can become the busiest period for long-distance trade and nomadic movement, not because a hardcoded script says “summer = more caravans,” but because the route definitions, weather, safety and trade systems make those circuits viable.

Fall can create visible pre-winter movement. Some traders may make final circuits before routes deteriorate. Groups threatened by worsening conditions may leave exposed regions. The player can see the shape of that movement if intelligence reaches them.

Winter can contract the world. Some trade corridors stop. Factions may consolidate into seasonal bases. Refugees may appear because real cold, radiation, conflict or scarcity has made their origin unsafe.

Nuclear winter can push this further—but still within finite population budgets and real route constraints. It cannot spawn endless anonymous refugees every day simply because the calendar says winter.

Most importantly, every moving group remains connected to the rest of ASHFALL.

A refugee group reaching the player's shelter does not become population automatically. It creates an admission decision. If accepted, those people enter the real survivor/population systems exactly once and begin consuming food, beds, medicine and labor capacity through those existing systems. If rejected, the group does not vanish; it continues its migration logic.

A trader circuit does not carry a second inventory inside SeasonalMigrationSystem. It activates or routes a real `TravelingCaravanSystem` caravan. The trade session, stock and prices remain canonical.

A faction relocation does not overwrite a territory field. It requests a seasonal operational change through the faction/world-state authority, with safeguards around mainline-critical locations.

A nomadic group can become a temporary source of trade, information or a guide—but only through the systems that already know how those services work.

The world map becomes more interesting without becoming omniscient. Some migrations are known because scouts, traders, radio traffic or faction contacts reported them. Others remain hidden until encountered. A reported route can go stale. A rumored refugee flow may be approximate rather than a perfect live tracker.

That creates strategic value.

The player can learn that a trader circuit usually appears after the spring thaw and plan stockpiles around it. They can anticipate that a winter faction relocation will make one road safer and another more dangerous. They can prepare shelter capacity when a reported refugee column is approaching. They can notice that a route has gone quiet because nuclear winter has cut it off.

None of this requires a second population/economy/faction simulation.

It requires one disciplined seasonal movement layer that connects the systems ASHFALL already has.

The result is a world where human geography changes with weather, danger, trade and time—and where the player can feel the year turning not only in the sky, but in who is moving across the wasteland.
