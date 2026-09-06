# ASHFALL Flagship Integration Plan — Plans 158–161
## Modular Vehicles, Dynamic Global Weather Disasters, Inter-Settlement Trade Routes & Endgame Legacy/Succession

**Scope:** Plans 158–161 — `ModularVehicleSystem`, `MacroWeatherSystem`, `TradeRouteSystem`, and `SuccessionSystem`.

**Primary domains:** expedition vehicles, world weather, shelter integrity, agriculture, economy, factions, caravans, pathing, narrative history, survivor death hooks, multi-generational persistence, deterministic RNG, content validation, Godot UI, and campaign-scale regression.

**Implementation objective:** Convert four large strategic systems into one coherent late-game progression layer. Vehicles become modular physical assets; global disasters alter the entire world over multi-week phases; trade becomes a persistent physical network with moving caravans and endogenous price pressure; and leadership succession turns campaign history into a durable generational layer. All four systems must integrate through existing authorities, preserve deterministic replay, remain backward-compatible with old saves, avoid duplicated state ownership, and remain stable over extremely long campaigns.

---

# 1. Flagship Mission

Plans 158–161 should collectively move ASHFALL from a shelter-and-expedition survival loop into a persistent regional simulation.

The intended strategic progression is:

```text
build and customize vehicles
→ establish reliable movement capacity
→ open and maintain trade routes
→ face world-scale weather disruptions
→ protect shelter, crops, caravans, and vehicles
→ accumulate leadership history and institutional knowledge
→ survive leader death / succession
→ enter a new political era without erasing campaign continuity
```

Player-facing outcomes:

1. **Vehicles become strategic platforms.** Armor, engines, cargo modules, wear, repair, fuel efficiency, and physical detached parts all matter.
2. **Weather becomes a macro-strategic force.** Nuclear winter, acid monsoon, ash storms, and EMP-like events unfold over phases that affect every zone and force long-term planning.
3. **Trade becomes physical and vulnerable.** Caravans move through the actual world, carry real inventory, face real threats, shift local prices, and affect settlement relations.
4. **Campaigns gain historical continuity.** Leaders die, heirs inherit institutions rather than magically replacing people, knowledge can be preserved imperfectly, and the settlement accumulates a lineage and eras.

The flagship standard is stronger than implementing four isolated classes. The systems must agree on:

```text
world topology
campaign time
survivor identity
vehicle identity
inventory
faction relations
route pathing
weather state
save/load
event ordering
RNG ownership
historical ledger
UI reconstruction
```

---

# 2. Non-Negotiable Architecture Rules

## 2.1 Existing authorities remain authoritative

New systems extend, but do not replace:

```text
ExpeditionVehicleSystem
ExpeditionSystem
WeatherSystem
FactionRelations
Inventory / ItemInventory
CaravanSystem if already present
CampaignEnvelopeBuilder
CombatTraumaSystem
MedicalSystem
```

The new systems own only their new state.

---

## 2.2 One canonical world topology

Plan 160 must reuse the same route/pathing graph used by `ExpeditionSystem`.

Plan 159 applies zone/world modifiers against the same canonical zone IDs.

Plan 158 vehicle mobility feeds that pathing/travel authority.

Do not create:
- a trade-only map,
- a weather-only zone registry,
- a modular-vehicle-only terrain system.

---

## 2.3 One canonical campaign clock

All:
- module attachment timers,
- disaster phases,
- caravan travel,
- route shortages,
- succession history,
- funeral windows,
- new-era transitions

use the same campaign day/tick authority.

No wall-clock time.

---

## 2.4 Deterministic RNG is concern-isolated

Use `ISeededRng` or current deterministic equivalent.

Preferred stream/namespace ownership:

```text
vehicle.module_install
vehicle.damage_degradation
weather.macro_pattern
weather.disaster_event
trade.ambush
trade.price_variation
trade.shortage
succession.inheritance
succession.power_struggle
succession.final_words
```

If named streams do not exist, use stable keyed RNG/counters.

Enabling trade must not shift future global-weather rolls.

---

## 2.5 Save/load correctness means continuation equivalence

For all four systems, require:

```text
uninterrupted run
==
save → fresh composition → restore → continue
```

Compare:
- state,
- future events,
- next stochastic outcome,
- canonical final hash.

---

## 2.6 Content definitions are data, runtime state is state

Definitions:

```text
vehicle_modules.json
macro_weather.json
trade_routes.json
legacy_traits.json
```

must never be copied wholesale into save state.

Persist:
- selected module IDs,
- current disaster ID/phase/progress,
- in-transit caravan state,
- active legacy IDs,
- leader history.

Definitions load from current validated catalogs.

---

## 2.7 Survivor identity is immutable

Leader succession must not replace survivor IDs or mutate historical records.

A leader who dies remains:
- a historical survivor,
- a memorial record,
- a leader-history entry.

The heir becomes a new leader through stable survivor identity.

---

## 2.8 “New era” never means arbitrary world reset

`OnNewEraBegun` is a semantic event.

It may intentionally reset only explicitly era-scoped state.

It must not silently reset:
- inventory,
- survivor relationships,
- world map discovery,
- faction wars,
- active disasters,
- caravans,
- infrastructure,
- vehicle condition,
- quests,
- treaty state,

unless a specific design rule says so.

Every reset must have a named, tested policy.

---

## 2.9 Prices are derived market state, not arbitrary randomness

Plan 160 dynamic pricing should primarily reflect:
- supply,
- demand,
- recent trade flow,
- shortages,
- route reliability,
- faction standing,
- settlement modifiers.

RNG may add bounded variation, but random noise must not be the economy.

---

## 2.10 UI is a projection of domain state

`GarageUI`, `WeatherUI`, `TradeRouteUI`, and `LegacyUI`:
- query authoritative systems,
- display previews,
- dispatch commands,
- render results.

They do not own:
- module durability,
- disaster progression,
- prices,
- succession state.

---

# 3. Mandatory Repository Reconnaissance Gate

Before production edits, inspect the live codebase and record findings.

## 3.1 Vehicle authority audit

Read:

```text
ExpeditionVehicleSystem
ExpeditionAggregateState
VehicleSaveCodec
EquipmentConditionSystem
ItemInventory / Inventory
existing vehicle definitions
vehicle repair/refuel APIs
```

Record:
1. vehicle stable ID,
2. vehicle base-stat definition,
3. current condition representation,
4. fuel model,
5. cargo model,
6. damage/breakdown handoff,
7. save ownership,
8. expedition ownership.

---

## 3.2 Weather authority audit

Read:

```text
WeatherSystem
world/zone weather state
season system
greenhouse/environment systems
shelter integrity/damage systems
ambient audio/theme event wiring
```

Record:
1. current daily weather source,
2. zone modifiers,
3. deterministic weather RNG,
4. season interaction,
5. greenhouse temperature/failure hooks,
6. shelter damage authority,
7. current forecast capability.

---

## 3.3 Trade/caravan authority audit

Read:

```text
CaravanSystem
ExpeditionSystem
FactionRelations
economy/trade systems
settlement catalogs
world topology/pathing
trade inventory transfer
```

Determine:
- whether caravans already exist,
- whether NPC settlement inventories exist,
- whether commodity prices already have an authority,
- how pathing is computed,
- how threats are represented,
- whether raids/escorts have quest integration.

---

## 3.4 Succession/history audit

Read:

```text
CampaignEnvelopeBuilder
MemorialSystem
CombatTraumaSystem
MedicalSystem
survivor death events
leader/faction governance state
campaign milestone/history systems
save schema/versioning
```

Record:
1. current leader concept,
2. leader death event ordering,
3. memorial creation,
4. survivor trait/skill storage,
5. long-term historical records,
6. campaign-day type and maximum range,
7. schema counters/version integer sizes.

---

## 3.5 Content validation

Read:

```text
CatalogIntegrityValidator.cs
ContentUtilizationScanner.cs
items.json
faction data
settlement data
world region/zone data
```

Determine:
- cross-reference registration pattern,
- duplicate-ID validation,
- orphan detection,
- deterministic diagnostic ordering.

---

## 3.6 UI conventions

Read existing passing Godot panels and:

```text
--scene-binding-selftest
scene-lint.py
input/accessibility audit
```

Record:
- panel lifecycle,
- host binding,
- focus navigation,
- dynamic card patterns,
- map visualization components.

---

## 3.7 Baseline regression

Record:

```bash
dotnet build Ashfall.csproj
dotnet test
```

and:

```text
--data-integrity-selftest
--content-utilization-selftest
--scene-binding-selftest
fast CI tier
```

**Exit gate:** no production edits until ownership and integration seams are documented.

---

# 4. Shared Data Integrity Standards

All four new catalogs must validate:

```text
unique IDs
valid references
finite numbers
legal ranges
non-empty required fields
stable ordering
backward-compatible defaults
```

Errors must report:

```text
catalog
definition ID
field path
bad value
expected rule/target catalog
```

Aggregate deterministically.

---

# 5. PLAN 158 — Vehicle Customization & Modular Armor

## 5.1 Goal

Expand existing expedition vehicles into modular chassis platforms with physically represented components, per-module durability, attach/detach workflows, fuel/cargo tradeoffs, and damage routing.

The player should be able to build a vehicle for:
- combat protection,
- long-range fuel efficiency,
- cargo capacity,
- scouting,
- rough terrain,
- convoy escort.

No configuration should dominate all dimensions.

---

## 5.2 Architecture

Create:

```text
ModularVehicleSystem.cs
```

as an extension/composition layer over `ExpeditionVehicleSystem`.

Recommended relationship:

```text
ExpeditionVehicleSystem
→ vehicle identity, expedition ownership, base state

ModularVehicleSystem
→ chassis slots, attached modules, module wear, derived modular stats
```

Avoid subclass inheritance if the project favors composition.

---

## 5.3 `VehicleChassis`

Definition fields may include:

```text
chassisId
displayName
baseMass
baseArmor
baseCargo
baseFuelCapacity
baseFuelEfficiency
baseSpeed
moduleSlots
compatibleModuleTags
baseDurability
vehicleTags
```

Use existing base vehicle definitions if possible.

If vehicles already exist, add a mapping:

```text
vehicleDefinitionId -> chassisDefinitionId
```

rather than duplicating all vehicle data.

---

## 5.4 `VehicleModule`

Module categories:

```text
Armor
Engine
Cargo
Utility
```

Task minimum is armor/engine/cargo.

Recommended definition:

```text
moduleId
displayName
moduleType
slotType
mass
durabilityMax
armorModifier
cargoModifier
speedModifier
fuelEfficiencyModifier
fuelCapacityModifier
terrainModifier
threatProfileModifier
attachTimeTicks
detachTimeTicks
requiredMechanicSkill
salvageProfile
itemId
tags
```

---

## 5.5 `vehicle_modules.json`

Register as canonical data authority.

Validation:
- unique module IDs,
- valid item IDs,
- known slot types,
- legal durability,
- finite modifiers,
- legal salvage profile,
- valid compatibility tags,
- attach/detach times > 0.

---

## 5.6 Runtime attachment state

Persist per installed component:

```text
slotId
moduleId
currentDurability
installedDay
installationQuality
damageState
```

Do not save definition copies.

---

## 5.7 Derived stat calculation

Implement:

```text
CalculateVehicleStats(vehicleId)
```

as a pure deterministic calculator over:
- chassis definition,
- installed modules,
- module condition,
- global vehicle condition if applicable.

Return:

```text
effectiveMass
effectiveArmor
effectiveCargoCapacity
effectiveFuelCapacity
effectiveFuelEfficiency
effectiveSpeed
terrain modifiers
threat profile
```

Stable module iteration order is mandatory.

---

## 5.8 Weight burden

Use a bounded game model.

Example concept:

```text
massBurden = effectiveMass / chassisRecommendedMass
```

Then apply authored curves.

Avoid hidden thresholds.

Expose breakdown in UI.

---

## 5.9 Fuel efficiency

Fuel efficiency should respond to:
- engine module,
- total mass burden,
- terrain,
- vehicle condition,
- weather if expedition authority already applies it.

Prevent double application:
- modular system returns base modular fuel modifier,
- expedition/travel system combines terrain/weather once.

---

## 5.10 Attach module

Command:

```text
TryAttachModule(vehicleId, slotId, moduleItemInstanceId, mechanicIds)
```

Validate:
- vehicle available/in garage,
- compatible slot,
- module item exists in inventory,
- slot free/replace policy,
- mechanic available,
- skill requirement,
- time requirement.

Do not instantly consume the module if installation is multi-tick unless job semantics reserve it.

---

## 5.11 Detach module

Command:

```text
TryDetachModule(vehicleId, slotId, mechanicIds)
```

On success:
- component leaves slot,
- same physical part returns to `ItemInventory`,
- remaining durability preserved if inventory item instances support condition.

If inventory only supports stackable IDs:
- introduce a durable item-instance representation or a vehicle-module inventory record.

Do not lose module wear by detaching/re-attaching.

---

## 5.12 Mechanic skill check

If installation quality is stochastic:
- use deterministic RNG,
- separate install stream,
- bound results.

Possible outcomes:
- perfect,
- standard,
- poor installation,
- blocked/failure without destruction.

Do not make a failed skill roll silently delete a module.

---

## 5.13 Armor degradation

Prefer a subsystem/component:

```text
VehicleArmorDamageResolver
```

rather than a second global “ArmorDegradationSystem” if vehicle damage already has an authority.

Damage order:

```text
incoming vehicle damage
→ eligible armor modules
→ module durability drain
→ residual chassis/vehicle damage
```

Define deterministic slot priority or coverage logic.

---

## 5.14 Combat/breakdown integration

Integrate through events/commands from:
- combat,
- raid,
- breakdown,
- collision/hazard systems.

Do not let narrative events mutate durability directly.

---

## 5.15 Broken modules

When durability reaches zero:
- modifier contribution changes according to definition,
- module marked broken,
- may remain attached until removed,
- may impose penalty if authored.

Do not silently delete.

---

## 5.16 Scrap module

Command:

```text
TryScrapModule(moduleInstanceId)
```

or vehicle-attached variant after detach.

Salvage:
- authored fixed/bounded returns,
- durability affects recovery,
- deterministic,
- atomic.

Prevent:
- scrapping attached module without detach flow,
- duplicate salvage,
- negative inventory.

---

## 5.17 Raider attention

“Heavily modified convoy” should use a derived visibility/threat score.

Inputs:
- armor silhouette,
- cargo mass/value,
- weapon/utility modules if later added,
- convoy size.

Narrative/encounter systems consume:

```text
VehicleThreatProfile
```

Do not hard-code encounter IDs in modular system.

---

## 5.18 Expedition aggregate

Update `ExpeditionAggregateState` only with state the expedition must carry.

Prefer vehicle state remains in canonical vehicle aggregate referenced by vehicle ID.

If expedition serialization snapshots vehicle state today, ensure:
- nested module IDs,
- durability,
- attachment slots

round-trip correctly.

Avoid two independent authoritative module copies.

---

## 5.19 `VehicleSaveCodec`

Support:
- old vehicles with no modules,
- nested module states,
- schema evolution,
- unknown module diagnostics,
- missing definition fallback/migration.

Old saves receive base/default slot state.

---

## 5.20 Garage UI

Create/extend `GarageUI`.

Display:

```text
vehicle chassis
slot grid
installed module
module durability
mass burden
armor
cargo
speed
fuel efficiency
terrain effects
threat profile
```

Actions:
- attach,
- detach,
- repair if available,
- scrap detached module.

Use drag/drop only as optional UX; keyboard/gamepad must have equivalent controls.

---

## 5.21 Plan 158 tests

Minimum:

1. module catalog validates.
2. all module item IDs resolve.
3. deterministic stat calculation.
4. module ordering cannot alter result.
5. incompatible slot rejected.
6. attach consumes/reserves physical module correctly.
7. failed attach is atomic.
8. detach returns same durable part.
9. module durability survives detach/re-attach.
10. mass burden reduces fuel efficiency correctly.
11. armor absorbs damage before chassis.
12. broken module contribution follows definition.
13. durability never underflows.
14. scrapping broken module returns authored partial resources.
15. double-scrap impossible.
16. same seed mechanic install outcome matches.
17. expedition state round-trip preserves modules.
18. old save loads vehicles without modules.
19. threat profile influences encounter hook once.
20. GarageUI rebind does not duplicate events.

---

# 6. PLAN 159 — Dynamic Global Weather Disasters

## 6.1 Goal

Introduce long-duration, world-scale weather/disaster states that force strategic adaptation across expeditions, agriculture, shelter integrity, trade, and ambient presentation.

A global disaster should feel like a campaign chapter, not a one-day weather modifier.

---

## 6.2 `MacroWeatherSystem`

Create:

```text
MacroWeatherSystem.cs
```

It owns:
- active global disaster,
- phase,
- phase progress,
- macro modifiers,
- forecast knowledge state.

It does not replace local weather.

---

## 6.3 `GlobalDisaster` definition

Phases:

```text
Onset
Peak
Dissipation
Ended
```

Definition fields:

```text
id
displayName
durationRangeOrFixedDuration
onsetTicks
peakTicks
dissipationTicks
temperatureShift
visibilityModifier
precipitationModifier
radiationModifier
electricalInterferenceModifier
cropStressModifier
shelterWearModifier
travelModifier
audioThemeKey
uiThemeKey
narrativeTriggers
tags
```

---

## 6.4 `macro_weather.json`

Author patterns such as:

```text
nuclear_winter
acid_monsoon
emp_storm
great_ash_storm
```

Keep IDs and phase modifiers data-driven.

---

## 6.5 Macro/local weather composition

Preferred model:

```text
LocalWeatherSnapshot =
WeatherSystem.BaseDailyPattern
+ MacroWeatherSystem.GlobalModifiers
+ zone-specific modifiers
```

Define precedence explicitly.

Macro weather should not destroy all local variation unless authored.

---

## 6.6 Temperature shift

`ApplyGlobalTemperatureShift` should return/provide a modifier.

Do not directly mutate every zone’s stored temperature if temperature is derived.

Use:

```text
GetGlobalClimateModifier(day)
```

and combine in local weather calculation.

---

## 6.7 Phase transitions

Transition deterministically:

```text
Onset -> Peak -> Dissipation -> Ended
```

Pin:
- start day,
- duration,
- phase boundaries.

No off-by-one ambiguity.

Add exact boundary tests.

---

## 6.8 Disaster scheduling

Determine whether disasters are:
- authored narrative milestones,
- stochastic campaign events,
- season-triggered,
- scenario-specific.

If stochastic:
- use macro-weather RNG stream,
- enforce cooldown/minimum interval,
- avoid overlapping incompatible disasters unless explicitly supported.

---

## 6.9 Shelter integrity

Create `ShelterIntegritySystem` only if no canonical shelter structural-health system exists.

Prefer extending existing shelter damage authority.

Track:
- structure condition,
- exposed modules/rooms,
- weather resistance,
- damage events.

Macro weather provides prolonged stress inputs.

---

## 6.10 Crop failure

Greenhouse integration should use canonical greenhouse state.

Inputs:
- greenhouse temperature,
- insulation,
- crop hardiness,
- disaster phase.

Outcomes:
- growth slowdown,
- yield loss,
- crop death only under authored thresholds.

Do not duplicate crop inventory.

---

## 6.11 Early Warning Radar

Treat as an infrastructure/forecast capability.

State:

```text
installed
condition
forecastHorizonDays
forecastConfidence
```

Macro system owns true future state; player knowledge is limited by radar.

UI must not reveal hidden exact start day beyond forecast capability.

---

## 6.12 Forecast timeline

`WeatherUI` gains macro timeline:

```text
Current
Onset
Peak
Dissipation
Expected end
confidence
```

Unknown forecast should display:
- uncertainty,
- broad warning window,
not exact hidden values.

---

## 6.13 Evacuation protocol

Command:

```text
TryTriggerEvacuationProtocol()
```

Uses canonical survivor room/assignment system.

Move:
- eligible surface survivors,
- to valid deeper shelter capacity.

Validate:
- capacity,
- medical/incapacitated constraints,
- critical workers,
- locked rooms.

Do not teleport survivors into nonexistent capacity.

---

## 6.14 Ambient/UI event

Fire:

```text
OnGlobalDisasterStarted
OnGlobalDisasterPhaseChanged
OnGlobalDisasterEnded
```

Presentation systems subscribe.

Simulation must not depend on audio/UI subscriber presence.

---

## 6.15 Narrative triggers

Examples:
- The First Snow,
- Great Ash Storm,
- first EMP blackout,
- monsoon onset.

Use one-shot campaign flags/event IDs.

Save/load must not replay narrative triggers.

---

## 6.16 Macro-weather persistence

Persist:

```text
activeDisasterId
startDay
phase
phaseProgress
scheduledDisaster if visible/owned
forecastKnowledgeState
triggeredNarrativeFlags if owned here
RNG continuation
```

Do not persist derived current temperature for every zone if recomputable.

---

## 6.17 Plan 159 tests

Minimum:

1. macro catalog validates.
2. phase durations legal.
3. exact onset boundary.
4. exact peak boundary.
5. exact dissipation boundary.
6. 30+ tick deterministic phase progression.
7. same seed/state schedules same disaster.
8. macro modifier composes with local weather once.
9. local weather variation remains where intended.
10. greenhouse receives temperature/crop stress.
11. insulated greenhouse avoids/reduces failure.
12. shelter integrity receives prolonged stress.
13. early warning respects forecast horizon.
14. UI does not reveal hidden exact dates.
15. evacuation obeys capacity.
16. evacuation failure is atomic.
17. disaster start event fires once.
18. phase event fires once per transition.
19. save/load preserves phase continuation.
20. old save with no macro state loads safely.

---

# 7. PLAN 160 — Inter-Settlement Trade Routes

## 7.1 Goal

Create persistent physical trade networks connecting known settlements.

Trade must involve:
- real goods,
- real vehicles,
- real guards,
- actual pathing,
- travel time,
- threats,
- settlement demand,
- price response,
- faction consequences.

---

## 7.2 `TradeRouteSystem`

Create:

```text
Ashfall.Core/Economy/TradeRouteSystem.cs
```

It owns:
- route definitions/instances,
- dispatched caravan commercial state,
- route reliability,
- settlement supply-flow history,
- shortage state,
- market history if no existing economy authority owns it.

It does not own world movement math.

---

## 7.3 DTOs

Required:

```text
CaravanRoute
Waypoint
ThreatLevel
```

Recommended:

### `CaravanRoute`

```text
routeId
originSettlementId
destinationSettlementId
waypointIds
baseDistance
allowedCommodityTags
baseRisk
status
```

### `Waypoint`

Use canonical world node ID where possible instead of duplicating coordinates.

### `ThreatLevel`

Prefer a value/enum derived from zone threat authority.

---

## 7.4 `trade_routes.json`

Definitions should reference:
- settlement IDs,
- world node IDs,
- commodity tags/items,
- base route constraints.

Do not store dynamic prices in authored route data.

---

## 7.5 Route discovery

A route should become usable only when:
- endpoints known,
- path known/reachable,
- diplomatic restrictions permit trade,
- required infrastructure/vehicles available.

Use world discovery authority.

---

## 7.6 Dispatch caravan

Command:

```text
TryDispatchCaravan(routeId, vehicleIds, guardIds, tradeManifest)
```

Validate:
- route active,
- vehicles available,
- cargo capacity,
- guards available,
- goods available,
- fuel/supplies,
- faction access.

All goods transfer into caravan cargo atomically.

---

## 7.7 Caravan state

Persist:

```text
caravanId
routeId
currentPathIndex
travelProgress
vehicleIds
guardIds
cargoManifest
origin
destination
departureDay
expectedArrivalRange
status
encounterState
```

Use stable IDs.

---

## 7.8 Shared pathing

Call the same pathing/travel math as `ExpeditionSystem`.

Preferred shared service:

```text
IWorldRoutePlanner
ITravelCostCalculator
```

Do not copy distance math into economy.

---

## 7.9 Threat/ambush chance

Calculate using:
- canonical zone threat,
- route exposure,
- convoy visibility,
- guards,
- vehicle modules/threat profile,
- faction hostility,
- macro weather if relevant.

Use deterministic RNG.

Do not add separate hidden “trade threat” if zone threat already exists.

---

## 7.10 Plan 158 integration

Modular vehicles affect trade:

```text
cargo modules -> manifest capacity
armor -> survivability
engine/fuel efficiency -> route cost
mass/threat profile -> ambush visibility
```

Use `CalculateVehicleStats`.

Do not reimplement module math in TradeRouteSystem.

---

## 7.11 Plan 159 integration

Macro disasters affect:
- route speed,
- closures,
- fuel use,
- threat,
- settlement supply demand,
- prices.

Trade uses current world/weather snapshots.

---

## 7.12 Dynamic pricing

If an existing economy system already owns pricing, extend it.

Otherwise create a focused market component rather than a global monolith.

Recommended model:

```text
price =
baseValue
× scarcityIndex
× recentDemandIndex
× recentSupplyIndex
× routeReliability
× factionModifier
× boundedVolatility
```

Clamp all values.

---

## 7.13 Recent trade history

Use rolling windows by campaign day:

```text
commodityId
settlementId
unitsImported
unitsExported
averagePrice
lastTradeDay
```

Do not let history grow without bounds.

Aggregate old data into summaries.

---

## 7.14 Deterministic price logic

Price fluctuations should be deterministic from state.

If volatility RNG is used:
- isolate stream,
- one documented draw budget,
- no unordered commodity iteration.

---

## 7.15 Faction relations

Maintained routes can improve relations via periodic authored increments.

Rules:
- only if actual trade completes,
- cap/decay as designed,
- no daily relationship farming from idle route existence.

---

## 7.16 Escort mission

Create a quest/expedition integration:
- player survivors attach to caravan,
- caravan uses shared travel encounters/pathing,
- guards become participants.

Trade system should not create a second combat engine.

---

## 7.17 Raid capability

If player may raid supply lines:
- use existing combat/encounter/faction-hostility systems,
- create consequences,
- transfer cargo through inventory authority,
- update route reliability.

Do not implement special loot bypass.

---

## 7.18 Supply shortage

A settlement becomes shortage-affected based on:
- consumption model,
- lack of incoming supply,
- route cut duration.

Penalties can affect:
- prices,
- faction stability,
- narrative events.

Use bounded state and prevent duplicate daily event spam.

---

## 7.19 Narrative logs

Structured events:
- caravan departed,
- arrived,
- delayed,
- ambushed,
- survived,
- lost,
- shortage began,
- shortage ended.

Journal/narrative system formats text.

---

## 7.20 TradeRouteUI

Display:

```text
map routes
origin/destination
route status
current caravan position/progress
commodity prices
recent trend
threat
weather disruption
vehicle/guard assignment
```

Do not expose hidden ambush RNG.

---

## 7.21 Trade persistence

Save:
- in-transit caravans,
- route reliability,
- rolling market state,
- shortages,
- pending escort mission refs,
- RNG continuation if owned here.

Restore must resume at same path progress.

---

## 7.22 Plan 160 tests

Minimum:

1. route catalog validates.
2. settlement/waypoint refs resolve.
3. dispatch requires real goods.
4. cargo capacity enforced.
5. dispatch is atomic.
6. vehicle unavailable rejected.
7. shared pathing matches expedition route cost.
8. modular vehicle cargo affects capacity.
9. macro weather affects route snapshot once.
10. ambush RNG deterministic.
11. guard rating affects authored risk.
12. price formula deterministic.
13. repeated same trade affects price in intended direction.
14. price bounded.
15. route completion improves relations once.
16. idle route does not farm relations.
17. in-transit save/load resumes correctly.
18. post-restore next encounter matches uninterrupted run.
19. shortage starts/ends deterministically.
20. rolling history remains bounded.

---

# 8. PLAN 161 — Endgame Legacy & Succession System

## 8.1 Goal

Create a durable leadership-history and succession framework for very long campaigns.

The system should preserve:
- historical leaders,
- achievements,
- designated heirs,
- institutional knowledge,
- succession crises,
- funeral consequences,
- era markers,
- legacy effects.

It must not imply biological “trait inheritance” unless the game explicitly models genealogy. Most “legacy traits” should be institutional, cultural, doctrinal, or learned inheritance.

---

## 8.2 `SuccessionSystem`

Create:

```text
Ashfall.Core/Narrative/SuccessionSystem.cs
```

Owns:
- current leadership office metadata,
- heir designation,
- leader legacy records,
- succession process,
- era markers,
- legacy buff activation,
- power-struggle state.

It does not own survivor death.

---

## 8.3 DTOs

Required:

```text
LeaderLegacy
KnowledgeTransfer
HeirApparent
```

Recommended:

### `LeaderLegacy`

```text
leaderId
eraId
leadershipStartDay
leadershipEndDay
causeOfExit
achievementIds
legacyTraitIds
finalWordsRecordId?
edictIds
```

### `KnowledgeTransfer`

```text
sourceLeaderId
targetHeirId
knowledgeDomain
transferStrength
archiveModifier
completed
```

### `HeirApparent`

```text
survivorId
designationDay
popularity
legitimacy
supportingFactionIds
status
```

---

## 8.4 `legacy_traits.json`

Prefer institutional effects.

Examples:

```text
legacy_field_medicine_doctrine
legacy_fortification_tradition
legacy_open_trade_charter
legacy_archive_culture
legacy_hard_winter_discipline
```

Avoid inheriting innate/personality traits as magic settlement buffs.

---

## 8.5 Legacy trait validation

Fields:

```text
id
displayName
triggerRequirements
effectType
effectMagnitude
stackingRule
durationOrPermanent
knowledgeDomain
tags
```

Validate:
- known effect type,
- legal magnitude,
- no duplicate stacking keys,
- references resolve.

---

## 8.6 Designate heir

Command:

```text
TryDesignateHeir(leaderId, heirId)
```

Validate:
- leader alive/active,
- heir alive/eligible,
- not same survivor,
- governance rules,
- no conflicting designation.

Designation may affect morale/popularity through canonical social/faction systems.

---

## 8.7 Heir popularity

Use derived inputs:
- survivor relations,
- faction standing,
- achievements,
- reputation,
- traits.

Do not store a permanently stale popularity number if it can change.

Persist:
- designation,
- optional snapshot for historical reasoning.

Calculate current popularity when needed.

---

## 8.8 Knowledge archive

The source proposes `KnowledgeArchiveSystem`.

First inspect whether Cultural Archive/education/skill systems already provide knowledge preservation.

Prefer extending an existing archive authority.

Concept:

```text
institutionalKnowledge[domain]
```

Examples:
- medical,
- engineering,
- agriculture,
- diplomacy.

This should preserve a portion of **institutional capability**, not copy a dead survivor’s exact skill points to another survivor automatically.

---

## 8.9 Knowledge transfer

When leader/expert dies:
- archive quality,
- apprentices,
- authored legacy,
- written records

determine retained knowledge.

Effect can:
- reduce training loss,
- unlock recipes,
- add settlement institutional bonus.

Do not duplicate survivor skill stats.

---

## 8.10 Funeral rites

Create a domain action/event:

```text
TryConductFuneralRites(leaderId, riteId)
```

Costs:
- authored resources,
- time,
- participant availability.

Results:
- morale/grief modifiers through canonical systems,
- memorial record,
- succession legitimacy effect.

Failure/no funeral can cause authored morale consequences.

---

## 8.11 Leader death integration

Subscribe to canonical death event after survivor death is finalized.

Recommended event ordering:

```text
Medical/Combat system resolves death
→ survivor aggregate marks deceased
→ memorial/death fact emitted
→ SuccessionSystem observes leader death
→ freezes historical leader record
→ checks designated heir
→ begins succession or power struggle
→ funeral window opens
```

Succession must never intercept/cancel death resolution.

---

## 8.12 Power struggle

If no valid heir:
- create deterministic event-chain state,
- identify eligible contenders,
- calculate support,
- schedule choices/events.

Use narrative event system.

Do not resolve leadership by arbitrary UI random choice.

---

## 8.13 Contender ordering

Stable selection:
- stable survivor ID,
- popularity,
- eligibility rules,
- deterministic tie-break.

RNG only where design calls for uncertainty.

---

## 8.14 Final Words

Treat “Final Words” as narrative content attached to the historical record.

Preferred:
- authored templates,
- structured achievements/relationships,
- deterministic content selection.

Do not require generative runtime text for save determinism.

If external generation is ever used, persist the resulting text/record once.

---

## 8.15 Lasting edicts

Edicts should be structured effects:

```text
edictId
leaderId
effectKey
startEra
duration
```

Validate stacking/expiry.

Do not encode arbitrary executable script strings in JSON.

---

## 8.16 Multi-generational campaign markers

Update `CampaignEnvelopeBuilder` with stable historical metadata only if appropriate.

Prefer a dedicated succession save section referenced by envelope.

Campaign envelope can include:
- current era ID,
- total eras,
- current leader ID.

Avoid bloating root envelope with every historical record.

---

## 8.17 Long-time numeric safety

Multi-century support requires auditing:

```text
campaign day type
tick counters
durations
history indexes
save version
RNG counters
Unix-like timestamps if any
```

Use sufficiently wide integer types.

Test at:
- 1 year,
- 10 years,
- 100 years,
- 500 years equivalent campaign days.

---

## 8.18 Historical log compaction

Do not let save files grow linearly forever with verbose event logs.

Use:
- leader records,
- era summaries,
- milestone compaction,
- bounded detailed history windows.

Keep durable key achievements, not every daily event.

---

## 8.19 `OnNewEraBegun`

Event payload:

```text
oldEraId
newEraId
oldLeaderId
newLeaderId
startDay
successionType
```

Subscribers may:
- update UI,
- generate journal entry,
- recalculate era-scoped modifiers.

---

## 8.20 Era reset policy

Create explicit registry:

```text
EraResetPolicy
```

Possible resettable state:
- era-specific popularity modifiers,
- temporary court/support scores,
- succession-crisis flags,
- leader-personal edicts if designed to expire.

Non-reset state:
- world map,
- inventory,
- survivors,
- structures,
- vehicles,
- active macro disaster,
- trade caravans,
- faction treaties/wars,
- archive knowledge,
- memorial history.

Test this policy directly.

---

## 8.21 LegacyUI

Display:

```text
current leader
designated heir
legitimacy/popularity
past leaders
era timeline
achievements
cause of death
legacy traits
final words
edicts
knowledge preservation
power-struggle status
```

Avoid rewriting history from current dynamic state.

Historical snapshots must remain stable.

---

## 8.22 Plan 161 tests

Minimum:

1. legacy catalog validates.
2. designate valid heir.
3. invalid/dead heir rejected.
4. popularity uses canonical relations.
5. leader death detected once.
6. historical record frozen once.
7. designated heir succession deterministic.
8. no-heir creates power struggle.
9. contender ordering deterministic.
10. funeral resources consumed atomically.
11. no-funeral consequence applies once.
12. institutional knowledge retention deterministic.
13. legacy buff applies once.
14. stacking rule enforced.
15. final words selection deterministic/persisted.
16. `OnNewEraBegun` fires once.
17. era reset policy preserves non-era world state.
18. century-scale day counters do not overflow.
19. history compaction preserves leader summaries.
20. save/load resumes mid-succession identically.

---

# 9. Cross-Plan Integration Architecture

## 9.1 Plan 158 ↔ Plan 160

Trade caravans use modular vehicle stats.

Canonical flow:

```text
TradeRouteSystem
→ asks ModularVehicleSystem for derived stats
→ asks Expedition pathing/travel for route costs
→ calculates commercial capacity/risk
```

No duplicated vehicle stat formulas.

---

## 9.2 Plan 159 ↔ Plan 160

Macro disaster modifiers affect:
- route travel time,
- closures,
- commodity scarcity,
- route threat,
- shortage.

Trade does not own weather.

---

## 9.3 Plan 159 ↔ Plan 158

Extreme weather may affect:
- module wear,
- fuel efficiency,
- travel penalties.

Apply weather once through travel/vehicle environmental context.

Avoid double-counting global + local penalties.

---

## 9.4 Plan 161 ↔ Plan 160

A leader may create trade-related legacy/edict modifiers.

Use structured modifier interfaces.

Do not let succession rewrite active caravan state.

---

## 9.5 Plan 161 ↔ Plan 159

A leader death during disaster must preserve disaster phase exactly.

New-era event cannot reset macro weather.

Add explicit regression.

---

## 9.6 Plan 161 ↔ Plan 158

Leader succession must not:
- reset vehicles,
- repair modules,
- detach modules,
- alter durability.

Legacy bonuses may influence future repair/training only through explicit modifiers.

---

# 10. Shared Event Model

Recommended immutable domain events.

## Vehicle

```text
VehicleModuleAttached
VehicleModuleDetached
VehicleModuleDamaged
VehicleModuleBroken
VehicleModuleScrapped
```

## Weather

```text
GlobalDisasterStarted
GlobalDisasterPhaseChanged
GlobalDisasterEnded
EvacuationProtocolTriggered
```

## Trade

```text
CaravanDispatched
CaravanArrived
CaravanAmbushed
CaravanLost
TradeRouteShortageStarted
TradeRouteShortageEnded
```

## Succession

```text
HeirDesignated
LeaderDied
SuccessionStarted
PowerStruggleStarted
FuneralRitesCompleted
NewLeaderInstalled
NewEraBegun
```

Stable IDs + campaign day.

---

# 11. Event Ordering Contracts

## 11.1 Vehicle damage

```text
combat/hazard damage fact
→ module armor resolution
→ residual vehicle damage
→ module break events
→ expedition result
```

## 11.2 Disaster day tick

```text
campaign day advances
→ macro phase transition
→ global modifier snapshot
→ local weather
→ greenhouse/shelter/trade/expedition consumers
→ presentation event
```

Document exact production order.

## 11.3 Caravan tick

```text
weather/path snapshot
→ travel progress
→ threat/encounter check
→ cargo/vehicle effects
→ arrival/shortage update
```

## 11.4 Leader death

As defined in Plan 161.

---

# 12. Shared Save Architecture

Recommended sections:

```text
modular_vehicles
macro_weather
trade_routes
succession_legacy
```

Follow current naming conventions.

Each section:
- explicit schema version,
- empty/default old-save state,
- deterministic ordering,
- no definition duplication.

---

# 13. Save Restore Order

Conceptually:

```text
catalogs
survivors
inventory
factions/world
vehicles
modular vehicle state
weather/local weather
macro weather
expedition/pathing
trade routes/caravans
medical/combat/memorial
succession
host event wiring
UI
```

Adapt to actual composition.

---

# 14. Old-Save Migration

## Modular vehicles

Existing vehicle:
- map to chassis,
- no optional modules,
- base durability preserved.

## Macro weather

No active disaster.

## Trade routes

No in-transit caravans unless existing caravan save can migrate.

## Succession

If a current leader exists:
- create era 1 baseline record,
- no heir,
- no historical predecessors.

Migration must be idempotent.

---

# 15. Long-Horizon Determinism Harness

Create:

```text
WorldScaleProgressionReplayTests
```

Fixture:
- seed 42,
- modular vehicle,
- macro disaster scheduled,
- active trade route,
- leader + designated heir,
- known inventory/factions/weather.

Run A:

```text
180 campaign days uninterrupted
```

Run B:

```text
90 days
save
fresh composition
restore
90 days
```

Compare:
- module wear,
- disaster phase/events,
- caravan positions/trade outcomes/prices,
- leader/succession state,
- inventory,
- faction relations,
- final hash,
- next day outcome.

---

# 16. Multi-Century Soak Test

Create a simulation-focused, low-content-overhead soak:

```text
100–500 simulated years
```

Goals:
- no integer overflow,
- no unbounded history explosion,
- no NaN prices,
- no negative durations,
- no stale references,
- era count stable,
- save/load still works.

This does not need full combat simulation each day.

Use accelerated deterministic ticks.

---

# 17. Content Utilization Graph — Plan 158

```text
vehicle module definition
→ item
→ acquisition/crafting
→ chassis slot compatibility
→ attached runtime use
→ expedition/trade consumer
```

Every module must have:
- at least one chassis,
- at least one acquisition route.

---

# 18. Content Utilization Graph — Plan 159

```text
global disaster definition
→ scheduler/narrative trigger
→ macro weather runtime
→ at least one live modifier consumer
```

Every modifier key must be consumed.

---

# 19. Content Utilization Graph — Plan 160

```text
trade route
→ origin/destination
→ path
→ commodity
→ dispatchable caravan
→ settlement market consumer
```

No orphan routes or commodities.

---

# 20. Content Utilization Graph — Plan 161

```text
legacy trait
→ trigger
→ succession/history system
→ live modifier consumer
```

No unused legacy buffs.

---

# 21. Catalog Integrity — Plan 158

Validate:
- module IDs,
- item refs,
- slot types,
- compatibility,
- modifier ranges,
- salvage refs,
- mass/durability.

---

# 22. Catalog Integrity — Plan 159

Validate:
- disaster IDs,
- legal phase durations,
- modifier keys,
- narrative trigger IDs,
- audio/UI theme refs if cataloged,
- incompatible overlap rules.

---

# 23. Catalog Integrity — Plan 160

Validate:
- route IDs,
- settlement refs,
- waypoint refs,
- commodity refs/tags,
- no impossible zero-length route,
- no identical origin/destination,
- legal base risk.

---

# 24. Catalog Integrity — Plan 161

Validate:
- legacy IDs,
- effect keys,
- stacking rules,
- knowledge domain refs,
- no contradictory duration/permanent flags.

---

# 25. UI Architecture

## GarageUI

Slot-based module view.

## WeatherUI

Macro forecast timeline + confidence.

## TradeRouteUI

Map/route/market/caravan view.

## LegacyUI

Historical timeline + succession state.

All:
- host-bound,
- idempotent subscriptions,
- mouse/keyboard/gamepad,
- no color-only critical state,
- no gameplay state mutation.

---

# 26. GarageUI Detail

Panels/cards:
- chassis summary,
- module slots,
- candidate modules,
- stat breakdown,
- durability,
- mass/fuel chart,
- attach/detach job state,
- scrap result preview.

Use production calculators.

---

# 27. WeatherUI Detail

Render:
- local daily weather,
- active macro disaster,
- phase,
- long-range warning,
- confidence,
- shelter/crop/trade impact summary.

Do not expose hidden future disaster data.

---

# 28. TradeRouteUI Detail

Render:
- route map,
- endpoints,
- commodity prices,
- recent trend,
- route reliability,
- current caravans,
- threat,
- guards/vehicles,
- macro-weather disruption.

Price chart uses bounded rolling history.

---

# 29. LegacyUI Detail

Render stable history:
- era,
- leader,
- achievements,
- legacy,
- cause of death,
- successor,
- final words,
- major events.

Historical values should not change because current relations/prices change.

---

# 30. Cross-System Resource Contention

Test scarce:
- fuel,
- repair materials,
- cargo modules,
- trade goods,
- funeral resources,
- shelter repair materials.

One successful command updates canonical inventory, all subsequent systems observe it.

---

# 31. Cross-System Survivor Contention

Potential assignments:
- mechanic,
- caravan guard,
- expedition crew,
- heir/leader role,
- evacuation roles.

Use canonical availability.

Leadership role alone should not necessarily block work unless game rules say so.

---

# 32. Macro Disaster × Trade Scenario

Simulate:
- active route,
- disaster onset,
- travel slowdown,
- delayed caravan,
- settlement shortage,
- price increase,
- disaster dissipation,
- route recovery.

Assert deterministic sequence.

---

# 33. Modular Convoy × Raider Attention Scenario

Compare:
- light cargo convoy,
- heavily armored/high-value convoy.

Use same route/seed.

Assert:
- derived threat profile differs,
- encounter eligibility/weight changes through existing narrative/encounter system,
- no direct forced ambush unless authored.

---

# 34. Succession During Crisis Scenario

Leader dies while:
- macro disaster active,
- caravan in transit,
- modular vehicle damaged.

Assert:
- succession begins,
- crisis state remains,
- caravan continues,
- vehicle remains damaged,
- new era changes only era-scoped state.

---

# 35. Trade Route Save/Restore Scenario

Save mid-segment.

Restore.

Assert:
- same position/progress,
- cargo,
- guards,
- vehicle IDs,
- next ambush outcome,
- ETA range.

---

# 36. Macro Weather Save/Restore Scenario

Save at:
- final onset day,
- first peak day,
- final peak day,
- first dissipation day.

Restore and advance one tick.

Assert exact phase.

---

# 37. Succession Save/Restore Scenario

Save:
- before leader death event processing,
- after death before heir installation,
- during power struggle,
- immediately before `OnNewEraBegun`.

Each restore must emit no duplicate historical records/events.

---

# 38. Modular Vehicle Save/Restore Scenario

Save:
- during attach job,
- with broken armor,
- with detached module in inventory,
- with vehicle in trade caravan.

Restore identity correctly.

---

# 39. Price Stability Protocol

Dynamic pricing must satisfy:

```text
finite
positive/non-negative per economy rule
bounded
mean-reverting or scarcity-driven
not exponentially divergent
```

Test 10,000 repeated trade updates.

No overflow/NaN.

---

# 40. Disaster Balance Protocol

For each disaster:
- phase duration,
- temperature impact,
- travel penalty,
- shelter wear,
- crop stress,
- forecast lead time

must be summarized in a balance table.

No disaster should create an unavoidable death spiral without counterplay unless explicitly narrative/endgame.

---

# 41. Vehicle Module Balance Protocol

For each module:
- benefit,
- mass cost,
- durability,
- fuel impact,
- threat impact,
- slot opportunity cost.

No armor module should be “free defense.”

No cargo module should increase capacity without burden unless intentionally rare.

---

# 42. Legacy Balance Protocol

Legacy buffs should be:
- meaningful,
- bounded,
- non-compounding where dangerous,
- historically earned.

If permanent, apply stacking caps.

Multi-century play must not create runaway +1000% bonuses.

---

# 43. Historical Compression Policy

Keep:
- one leader record per era,
- major milestones,
- final words,
- legacy traits,
- summarized trade/weather crises if linked.

Drop/aggregate:
- daily price rows beyond rolling window,
- low-value daily narrative logs.

---

# 44. Failure Diagnostics — Vehicle

Print:

```text
vehicleId
chassisId
slotId
moduleId
module durability
mass
fuel modifier
damage input
damage routing
inventory transaction
```

---

# 45. Failure Diagnostics — Weather

Print:

```text
day
disasterId
startDay
phase
phaseProgress
phase boundary
global modifier snapshot
local weather result
```

---

# 46. Failure Diagnostics — Trade

Print:

```text
caravanId
routeId
path index
day
cargo
vehicle IDs
guard IDs
zone threat
weather
ambush roll position
price state
```

---

# 47. Failure Diagnostics — Succession

Print:

```text
day
leaderId
heirId
eraId
succession status
contenders
legacy IDs
event IDs
history count
```

---

# 48. Risk Register

## Risk 1 — ModularVehicleSystem duplicates ExpeditionVehicleSystem

**Mitigation:** composition; base identity remains with existing vehicle authority.

## Risk 2 — module definitions copied into saves

**Mitigation:** save IDs + runtime durability only.

## Risk 3 — detach loses module durability

**Mitigation:** physical item instance/condition preservation.

## Risk 4 — armor absorbs damage in two systems

**Mitigation:** one vehicle damage routing pipeline.

## Risk 5 — weight penalty applied twice

**Mitigation:** modular calculator returns one stat snapshot; consumers do not rederive.

## Risk 6 — macro weather overwrites local weather completely

**Mitigation:** explicit composition model.

## Risk 7 — phase off-by-one errors

**Mitigation:** exact boundary test table.

## Risk 8 — forecast reveals hidden future state

**Mitigation:** player-knowledge forecast snapshot.

## Risk 9 — evacuation teleports beyond shelter capacity

**Mitigation:** canonical room/capacity validation.

## Risk 10 — trade system creates second pathfinder

**Mitigation:** shared route planner/travel calculator.

## Risk 11 — dynamic pricing becomes random noise

**Mitigation:** state-driven scarcity/supply/demand with bounded volatility.

## Risk 12 — price feedback explodes over long campaigns

**Mitigation:** clamps, decay, rolling windows, soak tests.

## Risk 13 — relation gain farms while route idle

**Mitigation:** reward completed trade, not route existence.

## Risk 14 — caravan cargo duplicated in settlement inventory

**Mitigation:** atomic transfer to in-transit cargo authority.

## Risk 15 — leader death event processed twice

**Mitigation:** stable death/succession event ID and idempotent state transition.

## Risk 16 — succession resets unrelated world systems

**Mitigation:** explicit EraResetPolicy allowlist.

## Risk 17 — “inherited traits” become runaway permanent stacking

**Mitigation:** institutional legacy model + stacking caps.

## Risk 18 — historical save grows without bound

**Mitigation:** era summaries and rolling economic logs.

## Risk 19 — century-scale counters overflow

**Mitigation:** wide integer audit + accelerated soak.

## Risk 20 — cross-system RNG drift

**Mitigation:** isolated streams/keyed RNG and replay harness.

---

# 49. Detailed Implementation Sequence

## Phase A — Reconnaissance

1. audit vehicles.
2. audit weather.
3. audit caravans/economy.
4. audit succession/death/history.
5. audit save.
6. audit catalogs/content scanner.
7. audit panels.
8. record baseline tests.

**Exit:** authority map complete.

---

## Phase B — Shared contracts

1. define RNG ownership.
2. define campaign-time contract.
3. define world route service.
4. define survivor availability interactions.
5. define modifier/event interfaces.
6. define save section order.

**Exit:** no cross-plan ambiguity.

---

## Phase C — Plan 158 core

1. module/chassis DTOs.
2. `vehicle_modules.json`.
3. loader/validation.
4. runtime attachment state.
5. pure stat calculator.
6. attach/detach jobs.
7. inventory integration.
8. damage degradation.
9. scrap.
10. expedition integration.
11. trade hook.
12. save codec.
13. GarageUI.
14. tests.

---

## Phase D — Plan 159 core

1. disaster DTO.
2. `macro_weather.json`.
3. phase state machine.
4. local weather composition.
5. shelter integrity hook.
6. greenhouse/crop hook.
7. radar/forecast.
8. evacuation.
9. narrative/audio/UI events.
10. save.
11. WeatherUI.
12. tests.

---

## Phase E — Plan 160 core

1. route DTOs.
2. `trade_routes.json`.
3. route validation.
4. shared pathing integration.
5. caravan dispatch.
6. cargo/vehicle/guard state.
7. threat/ambush.
8. market pricing.
9. faction relation.
10. escort mission.
11. shortage.
12. narrative events.
13. save.
14. TradeRouteUI.
15. tests.

---

## Phase F — Plan 161 core

1. legacy DTOs.
2. `legacy_traits.json`.
3. heir designation.
4. historical leader record.
5. knowledge preservation integration.
6. funeral rites.
7. death hook.
8. power struggle.
9. final words.
10. edicts.
11. era policy.
12. envelope/save integration.
13. LegacyUI.
14. century-scale tests.

---

## Phase G — Cross-plan integration

1. modular vehicle → trade.
2. macro weather → trade.
3. macro weather → vehicle travel.
4. succession → legacy modifiers.
5. succession crisis preservation.
6. shared events.
7. resource/survivor contention.

---

## Phase H — Content closure

1. register all catalogs.
2. add integrity rules.
3. add content utilization edges.
4. detect orphans.
5. run selftests.

---

## Phase I — Persistence/replay

1. old-save fixtures.
2. per-system roundtrip.
3. boundary save tests.
4. 180-day replay.
5. century soak.
6. final hash/trace.

---

## Phase J — CI closure

Run focused → integration → full.

---

# 50. Suggested Test Organization

```text
ModularVehicleSystemTests
ModularVehiclePersistenceTests
MacroWeatherSystemTests
MacroWeatherPersistenceTests
TradeRouteSystemTests
TradeRoutePersistenceTests
SuccessionSystemTests
SuccessionPersistenceTests
WorldScaleProgressionReplayTests
WorldScaleProgressionSoakTests
WorldScaleContentUtilizationTests
WorldScaleCrossSystemTests
```

---

# 51. Required Verification Gates

Focused suites for each system.

Then:

```text
--data-integrity-selftest
--content-utilization-selftest
--scene-binding-selftest
```

Build:

```bash
dotnet build Ashfall.csproj
```

Full:

```bash
dotnet test
```

Godot/CI:

```text
scene-lint.py
input/accessibility checks
bridge selftest
real campaign journey selftest
fast CI tier
```

where present.

No disabled/quarantined baseline tests.

---

# 52. Definition of Done — Plan 158

- [ ] ModularVehicleSystem extends existing vehicle authority without duplication.
- [ ] chassis/module definitions validated.
- [ ] module items resolve.
- [ ] deterministic stat calculation.
- [ ] module ordering irrelevant.
- [ ] attach/detach uses canonical skills/time.
- [ ] physical detached modules retain durability.
- [ ] armor degradation uses one damage pipeline.
- [ ] weight affects fuel efficiency exactly once.
- [ ] scrap is atomic and non-duplicable.
- [ ] vehicle aggregate/save handles nested module state.
- [ ] old vehicles load safely.
- [ ] raider attention consumes derived threat profile.
- [ ] GarageUI fully bound/accessibility-safe.
- [ ] tests/selftests pass.

---

# 53. Definition of Done — Plan 159

- [ ] MacroWeatherSystem owns global disaster state.
- [ ] macro catalog validates.
- [ ] onset/peak/dissipation deterministic.
- [ ] 30+ tick phase tests pass.
- [ ] global/local weather composition defined.
- [ ] shelter integrity hook live.
- [ ] greenhouse crop stress live.
- [ ] early warning radar limits player knowledge.
- [ ] long-term WeatherUI timeline live.
- [ ] evacuation respects capacity.
- [ ] narrative/audio/UI events fire once.
- [ ] save/load preserves phase continuation.
- [ ] old saves load with no active disaster.
- [ ] integrity/utilization tests pass.

---

# 54. Definition of Done — Plan 160

- [ ] TradeRouteSystem uses shared topology/pathing.
- [ ] trade route catalog validates.
- [ ] dispatch uses real vehicles/guards/goods.
- [ ] cargo transfer atomic.
- [ ] modular vehicle stats influence caravans.
- [ ] macro weather influences routes.
- [ ] ambush deterministic.
- [ ] dynamic pricing state-driven and bounded.
- [ ] completed trade improves faction relations once.
- [ ] escort uses existing expedition/combat authority.
- [ ] shortage events bounded/non-spamming.
- [ ] in-transit caravans save/restore.
- [ ] TradeRouteUI live.
- [ ] market soak remains finite.
- [ ] integrity/utilization tests pass.

---

# 55. Definition of Done — Plan 161

- [ ] SuccessionSystem observes canonical death events.
- [ ] legacy trait catalog validates.
- [ ] heir designation uses stable survivor IDs.
- [ ] popularity queries canonical relations/reputation.
- [ ] leader history freezes once.
- [ ] institutional knowledge preservation integrated.
- [ ] funeral rites atomic.
- [ ] no-heir power struggle deterministic.
- [ ] final words persisted/deterministic.
- [ ] legacy effects stack safely.
- [ ] `OnNewEraBegun` fires once.
- [ ] EraResetPolicy explicitly protects unrelated world state.
- [ ] CampaignEnvelopeBuilder tracks era markers without bloating root save.
- [ ] century-scale counters safe.
- [ ] history compaction prevents unbounded save growth.
- [ ] LegacyUI live.
- [ ] multi-century soak passes.

---

# 56. Global Definition of Done

- [ ] one canonical inventory.
- [ ] one canonical world topology.
- [ ] one campaign clock.
- [ ] deterministic isolated RNG.
- [ ] four save sections/versioned migrations.
- [ ] old-save compatibility.
- [ ] no duplicate state authority.
- [ ] no orphan content.
- [ ] UI reconstructs from state.
- [ ] cross-plan event ordering documented.
- [ ] 180-day replay identical.
- [ ] 100–500-year accelerated soak stable.
- [ ] full build/test/selftests green.
- [ ] no weakened pre-existing gates.

---

# 57. Final Deliverable Set

1. `ModularVehicleSystem.cs`
2. chassis/module DTOs
3. `vehicle_modules.json`
4. module item definitions/recipes
5. vehicle damage/module integration
6. `VehicleSaveCodec` migration
7. GarageUI updates
8. Plan 158 test suites

9. `MacroWeatherSystem.cs`
10. `GlobalDisaster` DTO
11. `macro_weather.json`
12. local weather integration
13. shelter/greenhouse hooks
14. early warning radar
15. WeatherUI timeline
16. macro weather save support
17. Plan 159 tests

18. `TradeRouteSystem.cs`
19. route/waypoint/threat DTOs
20. `trade_routes.json`
21. caravan dispatch/state
22. dynamic pricing component
23. shared pathing integration
24. faction/shortage/escort hooks
25. TradeRouteUI
26. Plan 160 tests

27. `SuccessionSystem.cs`
28. legacy/knowledge/heir DTOs
29. `legacy_traits.json`
30. death/succession integration
31. institutional knowledge bridge
32. funeral/power-struggle event chains
33. CampaignEnvelope/era markers
34. EraResetPolicy
35. LegacyUI
36. Plan 161 tests

37. four catalog integrity registrations
38. four content-utilization registrations
39. four save-section registrations
40. old-save migration fixtures
41. 180-day replay harness
42. century-scale soak harness
43. full CI evidence

---

# 58. Implementation Handoff Template

At completion record:

```text
Plan 158
- chassis authority:
- module count:
- module item authority:
- stat calculator:
- damage routing:
- fuel integration:
- save version:
- focused tests:

Plan 159
- disaster count:
- macro/local precedence:
- scheduler:
- shelter hook:
- greenhouse hook:
- forecast horizon model:
- save version:
- focused tests:

Plan 160
- route count:
- topology/pathing authority:
- caravan authority:
- pricing formula owner:
- rolling history window:
- faction relation hook:
- save version:
- focused tests:

Plan 161
- leader authority:
- death-event authority:
- knowledge archive authority:
- historical storage:
- EraResetPolicy:
- counter integer types:
- history compaction:
- save version:
- focused tests:

Shared
- campaign clock:
- RNG strategy:
- inventory authority:
- survivor availability authority:
- 180-day replay result:
- century soak result:
- data-integrity result:
- content-utilization result:
- scene-binding result:
- dotnet build:
- dotnet test:
```

---

# 59. Flagship Exit Standard

Plans 158–161 are ready when ASHFALL can sustain a regional, generational campaign without creating parallel authorities or deterministic drift.

A customized convoy must remain the same physical set of chassis/modules whether it is in the garage, on an expedition, or hauling trade cargo. A global disaster must modify the same weather/world state seen by crops, shelters, caravans, and expeditions. A trade route must move actual goods through actual world paths with prices responding to real supply conditions. A leader’s death must advance history without resetting the material world around them.

The final result should support a campaign in which the player can look back across eras and see continuity: vehicles rebuilt and scarred over decades, trade corridors opening and failing under disasters, institutions retaining partial knowledge, leaders leaving bounded legacies, and the settlement persisting as one deterministic simulation rather than a collection of disconnected feature states.
