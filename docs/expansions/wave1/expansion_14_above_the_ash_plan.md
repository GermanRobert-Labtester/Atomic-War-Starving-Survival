# ASHFALL — Expansion 14 Design Bible
# ABOVE THE ASH
### Wave 1 · Aviation, Sky Trade, Airdrops, Sky-Layer Armor, and Orbital Defense

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Expeditions` (Aviation), `Ashfall.Core.SkyDefense`, `Ashfall.Core.Shelter` (SkyLayerArmor), `Ashfall.Core` (OrbitalHarrowTelemetry)
**Proposed host owner:** `AviationHostSession` (extends `AviationSaveStore` + `SkyDefenseBatterySaveStore`)
**Existing save sections:** `aviation`, `sky_defense_battery` / battery state
**Existing CLI verbs:** `--aviation-selftest`, `--sky-defense-selftest`, `--orbital-harrow-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. It describes what should exist
and why current repository evidence supports it. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

The expansion sits on top of three live systems that already model the sky:
`AviationSystem` (flight phases, airworthiness, risk breakdown, aerial mapping),
`SkyDefenseBatterySystem` (counter-battery turrets, orbital tracks, interception
mitigation), and `SkyLayerArmorSystem` (roof armor against kinetic penetration and
weather). `OrbitalHarrowTelemetrySystem` feeds warnings and applies mitigation.

The expansion's job is to give that machinery a **world**: a sky economy, aerial
routes, airdrop competition, weather-driven flight, and the social consequences of
living under a sky that still drops the war. It adds no new physics and no new
save authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter can already put a balloon, a glider, and a jury-rigged ultralight into
the air. It can already shoot falling tungsten out of the sky. It can already armor
its roof against the next impact. But the sky is currently a menu: three aircraft,
six shells, six roof configurations, twelve orbital events, and no world around them.

**Above the Ash** turns the sky into a place. Weather fronts become flight windows.
A grounded balloon becomes a regional asset. A working ultralight makes the shelter
a trade node. Airdrops become contested windfalls. The orbital harrow becomes a
season, not a random event. And the roof over everyone's head becomes a decision the
player makes every storm.

The fantasy is not dogfighting. It is **small people with fragile machines, using
the only layer the enemy never fully claimed — the air above the ash.**

### 1.2 What the player manages

1. **Airworthiness.** `AircraftRuntimeState.airworthiness` decays with hours flown
   and hard landings. A grounded aircraft is scrap and a promise.
2. **Flight windows.** `WeatherSystem` provides wind, visibility, and temperature;
   `AviationSystem.CalculateFlightRisk` already turns those into wind-shear,
   visibility, icing, mechanical, and anti-air risk. A flight launched into a bad
   window is a rescue mission, not a mission.
3. **Fuel and payload.** Ultralight fuel is scarce; payload trades against range via
   `CalculateFlightRange`. Every kilogram is a decision.
4. **Mapping vs. trade vs. rescue.** Aerial recon reveals map cells
   (`OnAerialMappingPerformed`); air trade moves goods; rescue recovers grounded
   expeditions. One aircraft cannot do all three.
5. **Airdrops.** Cargo canisters descend on winds; the shelter can race rivals to
   recover them, or use its own aircraft to seed supply.
6. **The roof.** `SkyLayerArmorSystem` cells degrade under storm and impact; the
   player chooses material tiers and repair windows.
7. **The battery.** `SkyDefenseBatterySystem` turrets track, fire, overheat, and
   need calibration and service; interception reduces a pending strike's energy.

### 1.3 What it is not

- Not a flight simulator. Flight is abstract, deterministic, and text-forward.
- Not a combat dogfight system. Anti-air is ground-based exposure and risk, not
  aerial gunnery.
- Not a new weather authority. It consumes the existing weather and atmospheric
  sounding models.
- Not a new map authority. Aerial mapping reveals cells through the existing map.
- Not a real-world air force. Everything is salvage, improvisation, and loss.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Expeditions/AviationSystem.cs` | Flight phases, airworthiness, range/risk, mapping, history | `LIVE` |
| `Assets/Ashfall.Core/SkyDefense/SkyDefenseBatterySystem.cs` | Counter-battery turrets, tracks, interception | `LIVE` |
| `Assets/Ashfall.Core/SkyDefense/SkyDefenseOrdnanceCatalog.cs` | Ordnance definition loading | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` | Roof armor cells, degradation, mitigation | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SkyLayerArmorCatalog.cs` | Armor configuration loading | `LIVE` |
| `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` | Orbital warnings + interception mitigation | `LIVE` |
| `src/Host/AviationSaveStore.cs` | Aircraft/flight persistence | `LIVE` |
| `src/Host/SkyDefenseBatterySaveStore.cs` | Battery persistence | `LIVE` |
| `src/UI/AviationUI.cs` | Existing aviation panel | `LIVE` |
| `src/Host/HostCli.SkyDefense.cs` | Battery CLI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries | Notes |
|---|---|---|
| `aircraft_parts.json` | **3 aircraft** | balloon, glider, ultralight |
| `sky_defense_ordnance.json` | **6 ordnance** | flak, proximity, tungsten, chaff, beacon, shaped charge |
| `sky_layer_armor_catalog.json` | **6 configurations** | sandbag, scrap, concrete, steel hull, composite, blast canopy |
| `orbital_harrow_events.json` | **12 events** | kinetic tracks, clusters, EMP, dead-hand pings, false alarms |
| `atmospheric_sounding_catalog.json` | 4 altitude bands + 1 payload | wind, dust, radiation sampling |
| `cargo_airdrop_catalog.json` | profiles + pools | descent bands, interception, wind impact |

### 2.3 Confirmed gaps

- **GAP-14-1 — Three aircraft is a prototype, not a fleet.** No cargo airship, no
  gyrocopter, no kite-lift, no rocket-assisted launch, no wreck variants.
- **GAP-14-2 — No sky trade.** Nothing turns an aircraft into a trade route between
  settlements; no air caravan exists.
- **GAP-14-3 — No airdrop contest.** Cargo drop profiles exist but no rival
  competition, no interception-by-others, no recovery expedition.
- **GAP-14-4 — No aerial mapping content.** `OnAerialMappingPerformed` reveals cells
  from `locations.json`, but no authored aerial discoveries, anomalies, or
  flight-only locations exist.
- **GAP-14-5 — No flight locations.** No `loc_` node is an airfield, hangar, wreck
  field, or beacon tower.
- **GAP-14-6 — No weather-window contract.** Risk is computed but there is no
  authored "safe window" model, forecast tie-in, or ground-crew weather ritual.
- **GAP-14-7 — No orbital season.** Twelve events fire but there is no authored
  escalation, warning network, or shelter doctrine for living under a harrow season.
- **GAP-14-8 — No sky-defence society.** No crew roles, no radar picket, no battery
  lore, no civilian reaction to firing.
- **GAP-14-9 — Sky armor is mechanical only.** Degradation exists but no authored
  storm events, repair quests, or crew training.

### 2.4 Non-duplication statement

This expansion will **not** add a second weather system, a second map authority, a
second orbital telemetry system, a second battery, a second roof-armor model, or a
second expedition system. All new systems consume the live ones.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Fragile machines, real costs.** Every flight costs fuel, hours, and
airworthiness. Landing is the miracle. A crash is permanent unless salvaged.

**Pillar 2 — Weather is the mission planner.** The sky is not always open. The
player waits, gambles, or pays. This is the expansion's core tension.

**Pillar 3 — The sky is shared.** Rival shelters, traders, and raiders also fly.
Airdrops, routes, and map cells are contested. Cooperation is possible but rare.

**Pillar 4 — The harrow is a season.** Orbital strikes should feel like weather:
predictable in pattern, terrifying in execution, and survivable with preparation.

**Pillar 5 — Small people, big sky.** No hero pilots. A pilot is a survivor with a
skill and a history and a high chance of not coming back.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A launch | Two people, a rope, an engine that coughs | Top Gun fanfare |
| A flight | Cold, loud, short, beautiful | Aerial acrobatics |
| A crash | Inventory on the ground, a long walk home | Explosion spectacle |
| A harrow | A whistle, a shadow, a countdown | Apocalypse glow |
| A battery | Heat, oil, a turret that jams | Glowing sci-fi artillery |
| A roof repair | Tar, rebar, a storm coming | Fortress construction |

### 3.3 Content limits

- Fictional geography and factions only.
- No real aircraft names, real ordnance designations, or real military units.
- No glamorized air war; combat remains a failure of alternatives.
- The harrow is impersonal catastrophe, not a villain.

---

## 4. THE SKY WORLD

### 4.1 Aerial regions

The expansion adds an **air layer** above the existing map: an authored set of
**airways** (named flight corridors) linking existing `loc_` nodes. Airways are not
new map nodes; they are route metadata attached to flights, with weather exposure
and interception risk.

| Airway | Anchors | Character |
|---|---|---|
| `airway_ash_corridor` | shelter → `loc_ash_orphanage` → `loc_buried_school` | ash-choked, poor visibility |
| `airway_cold_ridge` | shelter → `loc_highland_relay` | icing risk, stable wind |
| `airway_river_road` | shelter → `loc_frozen_river_barge` | low, fast, easy to see |
| `airway_ruins_run` | shelter → `loc_substation_nine` | short, urban obstacles |
| `airway_blue_basin` | shelter → `loc_black_flotilla_flagship` | high radiation sampling |
| `airway_far_harrow` | shelter → `loc_orbital_crater` | exposed to interception |

### 4.2 New exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_hangar_boneyard` | The Hangar Boneyard | 6 | Salvage airframes, engines, fabric |
| `loc_kite_tower` | The Kite Tower | 4 | Static kite-lift mast; cheap lift platform |
| `loc_beacon_ridge` | Beacon Ridge | 5 | Navigational beacon; aerial navigation |
| `loc_wreck_marsh` | The Wreck Marsh | 6 | Crashed aircraft graveyard; parts and logbooks |
| `loc_radar_picket` | The Picket Line | 7 | Abandoned radar station; early warning |
| `loc_balloon_farm` | The Balloon Farm | 3 | Hydrogen generation and envelope repair |
| `loc_orbital_shield_ruins` | The Shield Works | 8 | Pre-war defense works; battery salvage |
| `loc_glider_takeoff` | The Cliff Takeoff | 5 | Glider ridge; silent covert recon |
| `loc_drop_marsh` | The Drop Marsh | 4 | Soft-landing zone for canisters |
| `loc_trust_airfield` | The Neutral Field | 5 | A shared airfield where rivals land |

### 4.3 New interior rooms

- **`room_hangar`** — aircraft storage and maintenance.
- **`room_air_traffic`** — a map table, a radio, and wind charts.
- **`room_battery_control`** — radar calibration, turret assignment.
- **`room_sky_workshop`** — envelope and airframe repair.
- **`room_pilot_ready`** — crew rest, briefings, personal effects.
- **`room_roof_access`** — sky-armor inspection and repair.

---

## 5. MAIN STORYLINE — "THE WIND THAT CARRIES"

### 5.1 Central conflict

A signal comes from above: a survivor shelter on the far side of the ash corridor
has a working airframe and no pilot, and the player's shelter has a pilot and no
airframe. The two can only meet from the sky.

Around this grows the expansion's conflict: the sky is the last commons left, and
everyone wants it. The Neutral Field is where rivals land for the same reason the
player does — it is the only ground that nobody can hold. When an orbital harrow
season begins and the warning network depends on a beacon nobody has maintained for
years, holding the sky becomes a matter of shared survival.

The central question: **does the player use the sky to connect the wasteland, or to
own it?**

### 5.2 Theme (unspoken)

**The sky is the one inheritance that cannot be fenced. What you build in it either
connects people or divides them.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_pilot_avano_reis` | Avano Reis | Grounded pilot | Wants to fly again; fears it more |
| `npc_rigger_mira_kolt` | Mira Kolt | Airframe rigger | Pragmatic, superstitious about weather |
| `npc_observer_seth_ild` | Seth Ild | Balloon observer | Sees everything; says little |
| `npc_battery_chief_orel` | Chief Orel | Battery commander | Burnt hands, steady voice |
| `npc_rival_pilot_kess` | Kess Havner | Rival shelter pilot | Competitive, not villainous |
| `npc_trader_airborne_venn` | Venn | Air trader | Sells routes and rare goods |
| `npc_beacon_keeper_rosa` | Rosa Lint | Beacon keeper | Custodian of the warning network |
| `npc_child_stowaway_pim` | Pim | Stowaway | The moral weight of the sky |

### 5.4 Story beats (13)

1. **The Signal.** A distant shelter calls for a pilot.
2. **The Boneyard.** First salvage airframe.
3. **The First Launch.** Risk, crash, or success; either is canon.
4. **The Neutral Field.** Meeting rivals; the shared airfield rules.
5. **The Airway Charts.** Mapping the ash corridor.
6. **The Drop.** A canister falls; a race begins.
7. **The Picket Line.** Restoring early warning.
8. **The Harrow Season.** Orbital escalation begins.
9. **The Battery Choice.** Spend ammunition or hold it.
10. **The Lost Flight.** A pilot does not return.
11. **The Beacon.** Rosa's warning network needs a keeper.
12. **The Wind That Carries.** The far shelter is reached — or not.
13. **The Commons.** The final disposition of the sky.

### 5.5 Branching choices (7)

| Choice | Options | Axis |
|---|---|---|
| Sky purpose | connect / own / withdraw | openness |
| Route sharing | share charts / keep / sell | trust vs. trade |
| Airdrop rivals | share / race / ambush | morality |
| Battery doctrine | fire early / hold / never | protection vs. scarcity |
| Lost pilot search | launch / wait / abandon | risk |
| Beacon custody | host / share / refuse | responsibility |
| Final disposition | commons / territory / silence | legacy |

### 5.6 Endings (5 + fade)

1. **The Open Sky** — a shared airway network; the wasteland connects.
2. **The Flagged Route** — the player's airway network is tolled and resented.
3. **The Ground Fleet** — the sky is abandoned; everything is hauled by road.
4. **The Long Fall** — a harrow season without warning; the battery is overwhelmed.
5. **The Keeper's Line** — the beacon passes to the next generation.
6. **Fade** — the airframe is left where it fell.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_sky_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (13)

`quest_sky_the_signal`, `quest_sky_boneyard_salvage`, `quest_sky_first_launch`,
`quest_sky_neutral_field`, `quest_sky_airway_charts`, `quest_sky_the_drop`,
`quest_sky_picket_restore`, `quest_sky_harrow_season`, `quest_sky_battery_choice`,
`quest_sky_lost_flight`, `quest_sky_the_beacon`, `quest_sky_wind_carries`,
`quest_sky_the_commons`.

### 6.2 Side quests (24)

**Hangar and maintenance (5)**
- `quest_sky_envelope_repair` — patch a hydrogen envelope
- `quest_sky_engine_teardown` — rebuild a scavenged engine
- `quest_sky_airworthiness_audit` — ground the fleet or fly
- `quest_sky_control_cable` — a critical cable needs rare wire
- `quest_sky_fabric_shortage` — envelope cloth is also shelter cloth

**Flight and weather (5)**
- `quest_sky_window_wait` — hold for a weather window
- `quest_sky_icing_advice` — the observer's forecast
- `quest_sky_night_landing` — a forced night return
- `quest_sky_headwind_choice` — turn back or press
- `quest_sky_balloon_ballast` — ballast discipline

**Trade and airdrop (5)**
- `quest_sky_air_trade` — establish an air route
- `quest_sky_drop_race` — beat rivals to a canister
- `quest_sky_drop_wreck` — a canister breaks open
- `quest_sky_trust_landing` — land on the Neutral Field
- `quest_sky_fuel_negotiation` — buy fuel at a bad price

**Recon and map (4)**
- `quest_sky_aerial_survey` — reveal cells
- `quest_sky_anomaly_pass` — observe an anomaly from the air
- `quest_sky_photo_run` — document a site
- `quest_sky_flight_only` — a location reachable only by air

**Battery and harrow (5)**
- `quest_sky_radar_calibration` — recalibrate the picket
- `quest_sky_barrel_service` — replace a worn barrel
- `quest_sky_ammo_argument` — which shell to keep
- `quest_sky_false_alarm` — do not fire at a friendly track
- `quest_sky_after_impact` — triage and roof repair

### 6.3 Repeatable quests (6)

`quest_sky_repeat_recon`, `quest_sky_repeat_maint`, `quest_sky_repeat_drop_watch`,
`quest_sky_repeat_wind_read`, `quest_sky_repeat_battery_drill`,
`quest_sky_repeat_roof_patch`.

### 6.4 Dynamic hooks

Flight events (`OnFlightLaunched`, `OnForcedLanding`, `OnFlightCrashed`,
`OnFlightReturned`, `OnAerialMappingPerformed`) and battery events already exist as
delegates. The generator can attach authored follow-ups without a new event bus.

### 6.5 Constraints

- No flight is guaranteed; risk is always real.
- No crash may be free of consequence; salvage is a quest.
- No aerial mapping may reveal a cell that bypasses the existing map progression.
- Battery interception may mitigate but never trivially negate a strike.
- No quest may present air combat against individuals as a victory fantasy.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `SkyTradeSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** air-route definitions, cargo manifests, route profitability, and rival
route competition. **Consumes:** `AviationSystem` flights, `TradingSystem`,
`RegionalPriceAtlas`, `Caravan` systems. **Data:** `air_routes.json`.
**Rules:** a route exists only while an airworthy aircraft and a pilot are assigned;
cargo mass reduces range via the existing `CalculateFlightRange`.

### 7.2 `AirdropContestSystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** active drop windows, rival claimants, recovery race resolution, and
recovery-site state. **Consumes:** `cargo_airdrop_catalog.json` profiles,
`WeatherSystem` wind, `ExpeditionSystem` ground recovery. **Data:** extend
`cargo_airdrop_catalog.json` with rival and recovery-site fields.
**Rules:** wind determines drift; interception is deterministic per authored risk;
the player never auto-wins a race.

### 7.3 `AerialSurveySystem` (new, `Ashfall.Core.Expeditions`)

**Owns:** aerial discovery definitions and which map cells reveal which authored
aerial findings. **Consumes:** `AviationSystem` mapping, `locations.json`,
`damaged_map_zones.json`. **Data:** `aerial_findings.json`.

### 7.4 `WeatherWindowSystem` (new, thin, `Ashfall.Core.Expeditions`)

**Owns:** authored flight window classification (`open`, `marginal`, `closed`) from
live weather, and forecast confidence. **Consumes:** `WeatherSystem`,
`atmospheric_sounding_catalog.json`. **Data:** `flight_windows.json`.
**Rules:** window classification is a pure function; no second weather model.

### 7.5 `OrbitalSeasonSystem` (new, `Ashfall.Core.SkyDefense`)

**Owns:** harrow-season state, escalation phase, and warning-network dependency.
**Consumes:** `OrbitalHarrowTelemetrySystem`, `SkyDefenseBatterySystem`.
**Data:** `harrow_seasons.json`.
**Rules:** seasons raise event frequency and severity within authored bounds; the
warning network (beacon/picket) improves lead time; no new physics.

### 7.6 `SkyCrewSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** pilot/rigger/observer/battery crew qualification and fatigue, expressed as
existing skills plus role tags. **Consumes:** `SkillProgressionSystem`,
`NeedsSystem`. It does **not** own skills or needs.

### 7.7 Systems explicitly not added

- No second weather, map, orbital, battery, roof-armor, or expedition system.
- No aerial dogfight combat system.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs are snake_case, integer `schema_version: 1`, validated by
`CatalogIntegrityValidator`, registered with `ContentUtilizationScanner`.

### 8.1 `aircraft_parts.json` (extend 3 → 12)

Existing schema preserved. New aircraft:

| ID | Name | Class | Character |
|---|---|---|---|
| `aircraft_cargo_airship` | Salvaged Cargo Airship | Airship | Slow, heavy lift, weather-fragile |
| `aircraft_gyrocopter` | Rotor Gyrocopter | Rotor | Short field, noisy, high fuel |
| `aircraft_kite_platform` | Kite-Lift Platform | Kite | Cheap, tethered, weather-bound |
| `aircraft_motor_glider` | Motor Glider | Glider | Efficient recon, low payload |
| `aircraft_rocket_sled` | Rocket-Assisted Sled | Assisted | One-shot extreme range |
| `aircraft_salvage_balloon` | Patchwork Balloon | Balloon | Cheapest, worst reliability |
| `aircraft_ambulance_plane` | Field Ambulance Plane | Powered | Medical evacuation |
| `aircraft_listener_plane` | Listening Plane | Powered | Signals intercept |
| `aircraft_wreck_static` | Static Wreck Airframe | Wreck | Not flyable; parts/shelter |

Each row uses `crew_requirement`, `payload_mass`, `fuel_type`, `base_fuel_burn`,
`cruise_range`, `speed_class`, `wind_tolerance`, `visibility_tolerance`,
`cold_tolerance`, `structural_reliability`, `discovery_radius`, `cargo_slots`,
`anti_air_exposure`, `tags`.

### 8.2 `sky_defense_ordnance.json` (extend 6 → 16)

Existing schema preserved. New types: airburst shrapnel, incendiary tracer, EMP
disruptor shell, illumination flare, smoke screen, radar-homing, decoy drone
cartridge, and training inert rounds. Every row keeps `ordnance_id`, `ammo_type`,
`item_id`, `magazine_units`, `tracking_modifier`, `interception_modifier`,
`heat_per_volley`, `recoil_load`, `burst_radius_units`,
`interception_ceiling_units`, `radar_lock_units`, `fragmentation_density`,
`propellant_grain_kg`, `residual_shrapnel_severity`, `tags`.

### 8.3 `sky_layer_armor_catalog.json` (extend 6 → 14)

New tiers: timber matting, rammed earth, geotextile layer, interlocked stone,
sacrificial ablative panel, water tank layer, arc-welded girder grid, and a
proposed repaired military composite. Fields preserved:
`id`, `name`, `description`, `tier`, `material_tier`, `default_thickness_meters`,
`blast_resistance_mj`, `attenuation_factor`, `degradation_rate`, `composition`,
`repair_cost`.

### 8.4 `orbital_harrow_events.json` (extend 12 → 30)

Existing schema preserved (`id`, `name`, `description`, `severity`, `signal_type`,
`is_false_positive`, `impact_energy_mj`, `lead_time_days`, `affected_cell_spread`,
`penetration_power_mj`, `salvage_yield_item_id`, `salvage_yield_quantity`,
`revealed_site_id`, `radio_hook_text`). New events include cluster swarms, decoy
tracks, lofted debris, EMP variants, and a season-capping "long fall."

### 8.5 `atmospheric_sounding_catalog.json` (extend)

Add bands (0–8k already live) with authored dust/radiation modifiers, and add
payloads: weather sonde, radiation sniffer, camera package, relay package.

### 8.6 `air_routes.json` (new)

Route ID, endpoint `loc_` IDs, airway, base risk, cargo tags, rival pressure, and
profit band.

### 8.7 `aerial_findings.json` (new)

Findings revealed only from the air: wreck fields, hidden settlements, crash sites,
signal fires, and an authored "flight-only" location.

### 8.8 `flight_windows.json` (new)

Window classification thresholds and forecast confidence by weather state.

### 8.9 `harrow_seasons.json` (new)

Season phases, event multipliers, warning-network lead-time effects, and end
conditions.

### 8.10 `airdrop_rivals.json` (new)

Rival claimants, their recovery strength, and race resolution weights.

### 8.11 Items

New items: `item_envelope_patch`, `item_hydrogen_flask`, `item_airframe_strut`,
`item_control_wire`, `item_aircraft_fuel_canister`, `item_flight_log`,
`item_wind_chart`, `item_beacon_bulb`, `item_radar_dish_component`,
`item_barrel_liner`, `item_parachute_silk`, `item_airdrop_beacon`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Aviation state is captured by `src/Host/AviationSaveStore.cs`; battery state by
`src/Host/SkyDefenseBatterySaveStore.cs`; roof armor by the shelter/grid store.
New sub-objects are additive inside these envelopes.

### 9.2 State to persist

- Aircraft runtime and airworthiness (already live).
- Active and historical flights (already live).
- New: air routes, active drop windows, recovery sites, aerial findings discovered,
  harrow season phase, warning-network condition, crew qualifications (derived from
  existing skills where possible).

### 9.3 Determinism

- Flight risk and incident rolls use the host-forked `ISeededRng`.
- Wind drift for airdrops is a pure function of live weather; no wall clock.
- Interception probability is authored and deterministic.
- Paired replay must match across continuous and interrupted runs.

### 9.4 Migration

Legacy saves load with no routes, no drops, no season, and neutral warning-network
state. No aircraft is invented.

### 9.5 Checksum

Invariant-culture floats; prefer integer-permille for exposure and risk weights.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `AviationUI` (extend) | Aircraft, airworthiness, active flights, risk | `AviationHostSession` |
| `SkyTradePanel` (new) | Air routes, manifests, rivals | same |
| `AirdropPanel` (new) | Active drops, drift, rival claimants | same |
| `AerialMapPanel` (new) | Aerial findings, survey coverage | same |
| `BatteryPanel` (extend) | Turrets, heat, calibration, tracks | `SkyDefenseHostSession` |
| `HarrowSeasonPanel` (new) | Season phase, warning lead time, doctrine | same |
| `RoofArmorPanel` (new) | Cells, tier, degradation, repair | shelter host |
| `PilotRosterPanel` (new) | Crew, qualification, fatigue | `AviationHostSession` |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Weather-window status uses text plus icon, never color alone.
- Keyboard/controller close/back preserved; focus maintained across refresh.
- No hidden probability; risk is expressed in-world (e.g. "the observer will not
  clear a launch in this wind").
- Live counts in the status rail update; stale values are never shown.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: engine cough, envelope creak, wind gust,
turret traverse, distant whistling descent, impact, relief silence. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `AviationSystem` | Extended via public methods only; events consumed |
| `SkyDefenseBatterySystem` | Consumes tracks and applies mitigation; no duplicate |
| `SkyLayerArmorSystem` | Consumes degradation/weather; adds repair commands |
| `OrbitalHarrowTelemetrySystem` | Feeds harrow seasons; no new telemetry |
| `WeatherSystem` / `WeatherIntelligenceCoordinator` | Flight windows read live weather |
| `ExpeditionSystem` | Ground recovery and destination travel reuse it |
| `TradingSystem` / `RegionalPriceAtlas` | Air-route economics |
| `Caravan` systems | Ground/air comparison |
| `MapNode` / `LocationLayoutSystem` | Aerial reveal uses existing map |
| `PowerGridSystem` | Hangar heat, battery radar, roof works |
| `Inventory` | Fuel, parts, ordnance, cargo are normal items |
| `SkillProgressionSystem` | Pilot/rigger/observer skill floors |
| `NeedsSystem` | Crew fatigue and exposure |
| `DiseaseSystem` / `RadiationSystem` | High-altitude and crash-site exposure |
| `MemorialSystem` | Pilot loss |
| `FactionStanceEngine` | Rival air powers, tolls, shared field |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `AviationSystem`, `SkyDefenseBatterySystem`,
`SkyLayerArmorSystem`, `OrbitalHarrowTelemetrySystem`, save stores, and `AviationUI`
APIs have not drifted. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend aircraft, ordnance, armor, orbital events,
sounding; author routes, findings, windows, seasons, rivals. Register validators
and scanner. No gameplay.

**Phase 2 — Pure Core.** `SkyTradeSystem`, `AirdropContestSystem`,
`AerialSurveySystem`, `WeatherWindowSystem`, `OrbitalSeasonSystem`, `SkyCrewSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `AviationHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 30/90/180-day soak including harrow season and crash streaks.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Aircraft | 9 new (3 → 12) |
| Ordnance | 10 new (6 → 16) |
| Armor configs | 8 new (6 → 14) |
| Orbital events | 18 new (12 → 30) |
| Altitude bands | +3 |
| Sounding payloads | +4 |
| Airways | 6 |
| Air routes | 12 |
| Aerial findings | 20 |
| Flight windows | 12 rules |
| Harrow seasons | 4 phases |
| Airdrop rivals | 5 |
| Locations | 10 |
| Rooms | 6 |
| NPCs | 8 |
| Main quests | 13 |
| Side quests | 24 |
| Repeatable | 6 |
| Items | 12 |
| Endings | 5 + fade |
| Prose estimate | 50,000–65,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Flight trivializes travel | High | Real fuel, weather, crash, repair |
| Aerial mapping breaks map progression | High | Reveal only authored findings; bounded radius |
| Battery trivializes harrow | High | Ammo scarcity, heat, calibration, interception caps |
| Duplicate weather model | High | Read live weather only |
| Save bloat (per-flight history) | Medium | Cap history; summarize older flights |
| Tone drift to air war | Medium | No dogfighting; combat is failure |
| Determinism break | Low | Host-forked RNG only |
| Content overrun | Medium | Budget §23 |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Expeditions/SkyTradeSystemTests.cs`
- `Ashfall.Core.Tests/Expeditions/AirdropContestTests.cs`
- `Ashfall.Core.Tests/Expeditions/AerialSurveyTests.cs`
- `Ashfall.Core.Tests/Expeditions/WeatherWindowTests.cs`
- `Ashfall.Core.Tests/SkyDefense/OrbitalSeasonTests.cs`
- `Ashfall.Core.Tests/Expeditions/AviationSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Expeditions/AviationDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/AviationCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Range/risk honor existing `CalculateFlightRange` and `CalculateFlightRisk`.
- Airworthiness decay is monotonic under flight hours and hard landings.
- Crash is never free; salvage produces real items.
- Airdrop drift is deterministic from weather; races are not auto-won.
- Aerial reveal does not exceed authored findings or bypass map progression.
- Battery interception reduces but never trivially negates a strike.
- Harrow season escalation stays within authored event bounds.
- Round-trip restores aircraft, routes, drops, findings, season.
- Legacy loads neutral.
- Paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/
bash scripts/run_test.sh Ashfall.Core.Tests/SkyDefense/
godot --headless --path . -- --aviation-selftest
godot --headless --path . -- --sky-defense-selftest
godot --headless --path . -- --orbital-harrow-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

---

## 14. ACCEPTANCE CRITERIA

Core authority documented and engine-free; data canonical with valid schema and
passing integrity; persistence round-trips with neutral legacy and Triad parity;
determinism proven; host reachable by a real route/event; player can observe the
outcome; focused tests green; docs updated. Compile-green is not acceptance.

---

## 15. CROSS-EXPANSION HOOKS (WAVE 1)

| Expansion | Hook |
|---|---|
| 12 The Second Generation | First flight milestone; stowaway child |
| 13 The Faithful | Aircraft as "signs"; sky-cult; Listener mast |
| 15 The Deep Root | Aerial seeding; crop survey flights |
| 16 The Rebuilt Body | Drone swarms; bionic pilot endurance |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- The existing 3 aircraft, 6 ordnance, 6 armor configs, and 12 orbital events.
- The interception mitigation contract (battery reduces, never erases).
- The map progression and concealment rules.
- Fictional geography and faction namespaces.

### 16.2 New canon

- Airways, the Neutral Field, the Picket Line, the Beacon.
- The harrow season as a recurring, forecastable phenomenon.
- The sky as a commons in wasteland politics.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — AIRCRAFT EXPANSION TABLE

| Aircraft | Crew | Payload kg | Range km | Wind tol | Reliability | Cargo | AA exposure |
|---|---|---|---|---|---|---|---|
| Observation Balloon *(LIVE)* | 1 | 80 | 30 | 18 | 0.85 | 2 | 0.45 |
| Scavenged Glider *(LIVE)* | 1 | 50 | 45 | 26 | 0.78 | 1 | 0.15 |
| Jury-Rigged Ultralight *(LIVE)* | 2 | 140 | 110 | 35 | 0.72 | 4 | 0.65 |
| Cargo Airship | 3 | 600 | 160 | 22 | 0.66 | 12 | 0.30 |
| Gyrocopter | 2 | 120 | 90 | 30 | 0.70 | 3 | 0.75 |
| Kite Platform | 1 | 40 | 12 | 12 | 0.80 | 1 | 0.10 |
| Motor Glider | 1 | 60 | 130 | 28 | 0.74 | 2 | 0.25 |
| Rocket Sled | 1 | 90 | 260 | 40 | 0.40 | 2 | 0.85 |
| Salvage Balloon | 1 | 100 | 35 | 16 | 0.62 | 3 | 0.40 |
| Ambulance Plane | 2 | 180 | 120 | 30 | 0.68 | 4 | 0.70 |
| Listening Plane | 2 | 70 | 140 | 32 | 0.73 | 2 | 0.35 |
| Static Wreck | 0 | 0 | 0 | 0 | 0.0 | 0 | 0 |

New rows are balanced so no aircraft dominates on payload, range, reliability, and
concealment simultaneously. The rocket sled is a one-shot gamble, not a solution.

---

## 18. APPENDIX B — ORDNANCE EXPANSION TABLE

| Ordnance | Type | Tracking | Interception | Heat | Recoil | Notes |
|---|---|---|---|---|---|---|
| 76mm HE Flak *(LIVE)* | flak | 0.5 | 0.0 | 12 | 6 | area |
| 76mm Proximity *(LIVE)* | proximity | 1.0 | 0.15 | 14 | 6 | scarce |
| 76mm Tungsten *(LIVE)* | kinetic | 1.5 | 0.25 | 18 | 9 | heavy |
| Chaff Burst *(LIVE)* | chaff | -0.5 | -0.1 | 4 | 2 | decoy |
| Beacon Smokey *(LIVE)* | marker | — | — | — | — | target marking |
| Shaped Charge *(LIVE)* | kinetic | — | — | — | — | penetration |
| Airburst Shrapnel | flak | 0.8 | 0.10 | 15 | 5 | high frag |
| Incendiary Tracer | tracer | 0.6 | 0.05 | 8 | 3 | visibility |
| EMP Disruptor | emp | 1.2 | 0.20 | 20 | 7 | electronics |
| Illumination Flare | marker | — | — | 2 | 1 | night |
| Smoke Screen | marker | — | — | 3 | 1 | conceal |
| Radar-Homing | homing | 1.8 | 0.30 | 22 | 8 | calibration-dependent |
| Decoy Cartridge | decoy | — | — | 5 | 2 | spoof track |
| Inert Training | inert | 0.2 | 0.0 | 4 | 2 | crew drill |

No round is a one-shot answer. Interception and heat are bounded by the live
system's caps.

---

## 19. APPENDIX C — SKY-LAYER ARMOR EXPANSION TABLE

| Config | Tier | Thickness m | Blast MJ | Attenuation | Degradation |
|---|---|---|---|---|---|
| Sandbag Layer *(LIVE)* | improvised | 0.8 | 5.0 | 0.60 | 0.35 |
| Scrap Overlay *(LIVE)* | improvised | 1.0 | 12.0 | 0.45 | 0.25 |
| Reinforced Concrete *(LIVE)* | reinforced | 1.5 | 37.5 | 0.20 | 0.15 |
| Steel Hull Plating *(LIVE)* | reinforced | 1.2 | 48.0 | 0.18 | 0.18 |
| Composite Military *(LIVE)* | hardened | 1.4 | 72.0 | 0.10 | 0.08 |
| Emergency Blast Canopy *(LIVE)* | improvised | 0.4 | 4.0 | 0.75 | 0.50 |
| Timber Matting | improvised | 0.6 | 3.0 | 0.70 | 0.40 |
| Rammed Earth | improvised | 1.0 | 9.0 | 0.52 | 0.30 |
| Geotextile Layer | reinforced | 0.5 | 7.0 | 0.58 | 0.28 |
| Interlocked Stone | reinforced | 1.1 | 20.0 | 0.34 | 0.22 |
| Ablative Panel | hardened | 0.9 | 44.0 | 0.16 | 0.55 |
| Water Tank Layer | hardened | 1.6 | 52.0 | 0.14 | 0.20 |
| Girder Grid | reinforced | 1.3 | 30.0 | 0.26 | 0.24 |
| Repaired Composite | hardened | 1.4 | 64.0 | 0.12 | 0.14 |

Degradation and repair costs keep higher tiers expensive; there is no free upgrade.

---

## 20. APPENDIX D — ORBITAL EVENT CLASSES

| Class | Live examples | Proposed additions | Player response |
|---|---|---|---|
| Kinetic single | early track, thermal descent, seismic precursor | high-angle drop, skipping track | battery or shelter |
| Cluster | multiple returns, split track | swarm descent, fragment cloud | timing allocation |
| EMP | radio blackout, signature mismatch | grid cascade, data-wipe | grounding, hardening |
| Dead-hand | repeating ping, broken checksum | countdown restart, false authority | refuse or obey |
| False alarm | radar ducting, debris misclassification | chaff echo, flock reflection | do not waste ammo |
| Season cap | — | "the long fall" | survive the night |

False alarms are essential: firing on every track empties the magazine and
destroys the battery's usefulness. The player must learn to read the sky.

---

## 21. APPENDIX E — ECONOMY AND BALANCE MODEL

- **Fuel** is the master sky bottleneck. An ultralight sortie consumes real fuel
  that the generator also needs, echoing the live diesel pressure.
- **Airworthiness** is the second bottleneck: hours accumulate and airframes wear
  out. Salvage parts are the only repair.
- **Air trade** should be profitable but not dominant over ground caravans; it
  offers speed and reach, not volume.
- **Airdrops** are a windfall with a race; rivals can claim cargo, and the player
  cannot always win.
- **Ordnance** is scarce: each shell fired is a shell not made. Interception must be
  worth the cost, or the battery becomes a liability.
- **Sky armor** competes with building materials. A fully hardened roof means a
  weaker foundry.
- Nothing in the sky is a net resource positive; the returns are reach, warning,
  and survival under the harrow.

---

## 22. APPENDIX F — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `aircraft_parts.json` | +9 | 4,500 |
| `sky_defense_ordnance.json` | +10 | 3,000 |
| `sky_layer_armor_catalog.json` | +8 | 2,500 |
| `orbital_harrow_events.json` | +18 | 6,000 |
| `atmospheric_sounding_catalog.json` | +7 | 1,500 |
| `air_routes.json` | 12 | 3,000 |
| `aerial_findings.json` | 20 | 4,000 |
| `flight_windows.json` | 12 | 2,000 |
| `harrow_seasons.json` | 4 | 1,500 |
| `airdrop_rivals.json` | 5 | 1,500 |
| Quest objectives | 43 quests | 12,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 12 | 1,800 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~56,800** |

---

## 23. APPENDIX G — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R14-1 | Flight trivializes travel | Med | High | Fuel, weather, wear, crash |
| R14-2 | Mapping bypass | Med | High | Authored findings only |
| R14-3 | Battery trivializes harrow | Med | High | Scarcity, heat, calibration |
| R14-4 | Duplicate weather | Low | High | Read live weather |
| R14-5 | Save bloat | Med | Med | Cap history |
| R14-6 | Air-war tone drift | Med | Med | Combat is failure |
| R14-7 | Determinism | Low | High | Host-forked RNG |
| R14-8 | Content overrun | Med | Med | Budget §22 |
| R14-9 | False alarms ignored | Med | Med | Authored teaching moments |
| R14-10 | Crash too punishing | Med | Med | Rescue and salvage paths |

---

## 24. APPENDIX H — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does the expansion add a powered fixed-wing aircraft?** Recommended yes
   (ambulance/listener), but it must be expensive and rare.
2. **Can the player intercept an airdrop rival?** Recommended race only, no ambush,
   to preserve tone.
3. **Harrow season frequency** — once per campaign, or recurring? Recommended
   recurring with escalating severity and recovery windows.
4. **Aerial reveal radius** — bounded to authored findings or free cell reveal?
   Recommended authored findings only.
5. **Crash fatality** — can a crash kill the pilot? Recommended yes, through the
   existing survivor death path, with memorial integration.

---

## 25. APPENDIX I — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_sky_the_signal` | 3 | A distant shelter calls; decode the frequency; answer or not |
| `quest_sky_boneyard_salvage` | 5 | Reach the boneyard; strip a frame; haul it home |
| `quest_sky_first_launch` | 5 | Fuel, crew, window, launch, land |
| `quest_sky_neutral_field` | 4 | Land among rivals; learn the field rules; keep the peace |
| `quest_sky_airway_charts` | 4 | Fly each airway; record hazards; chart the corridor |
| `quest_sky_the_drop` | 5 | Track the canister; race rivals; recover or lose |
| `quest_sky_picket_restore` | 5 | Reach the picket; repair radar; restore warning |
| `quest_sky_harrow_season` | 6 | Recognize the season; harden the roof; alert the network |
| `quest_sky_battery_choice` | 4 | Spend ammunition or hold; live with the choice |
| `quest_sky_lost_flight` | 5 | Search the airway; find wreck or survivor |
| `quest_sky_the_beacon` | 4 | Reach the beacon; relight it; choose a keeper |
| `quest_sky_wind_carries` | 5 | Fly the far corridor; make contact; return or stay |
| `quest_sky_the_commons` | 3 | Final disposition of the sky; epilogue selection |

---

## 26. APPENDIX J — NPC DOSSIERS (BRIEF)

**Avano Reis** — a pilot who survived a crash and no longer trusts the calm air.
Has the skill, avoids the cockpit. The expansion's central relationship: the player
has to earn his return, not command it.

**Mira Kolt** — airframe rigger. Superstitious about specific winds and specific
routes. Pragmatic, blunt, and the only person who can tell an envelope is about to
fail by looking at it.

**Seth Ild** — balloon observer. Sees the whole map and speaks in short facts. The
player's primary recon voice; his silences are information.

**Chief Orel** — battery commander. Burned hands from a breech failure. Believes
firing should always be the last option. Represents restraint under pressure.

**Kess Havner** — rival shelter's pilot. Competitive, not cruel; will share a
landing field and race for a canister in the same afternoon. A reflection of the
player.

**Venn** — air trader. Sells routes, charts, and rare goods, and honestly labels
what is unproven. The expansion's trade voice.

**Rosa Lint** — beacon keeper. The warning network's human anchor. Her arc is about
whether the next generation will keep the line.

**Pim** — a stowaway child. Rides along on a flight and forces the player to
confront why the sky matters to people who never asked for it.

---

## 27. APPENDIX K — ORBITAL EVENT EXPANSION TABLE (30 TOTAL)

| # | Event ID | Severity | Signal | Lead | Cap |
|---|---|---|---|---|---|
| 1–12 | *(LIVE)* | Mod–Severe | radar/thermal/seismic | 2–5 | single/cluster/EMP |
| 13 | `event_orbital_high_angle_drop` | Severe | radar_anomaly | 3 | high penetration |
| 14 | `event_orbital_skipping_track` | Severe | thermal_signature | 4 | long track |
| 15 | `event_orbital_swarm_descent` | Extreme | multiple_returns | 5 | many impacts |
| 16 | `event_orbital_fragment_cloud` | Severe | debris_track | 4 | wide spread |
| 17 | `event_orbital_grid_cascade` | Severe | emp_signature | 2 | power loss |
| 18 | `event_orbital_data_wipe` | Moderate | emp_signature | 2 | records loss |
| 19 | `event_orbital_countdown_restart` | Severe | dead_hand_ping | 6 | false authority |
| 20 | `event_orbital_chaff_echo` | Moderate | radar_anomaly | 2 | false positive |
| 21 | `event_orbital_flock_reflection` | Minor | radar_anomaly | 1 | false positive |
| 22 | `event_orbital_lofted_debris` | Moderate | debris_track | 3 | area damage |
| 23 | `event_orbital_ground_bounce` | Moderate | seismic_precursor | 4 | shallow |
| 24 | `event_orbital_double_tap` | Extreme | thermal_signature | 3 | two impacts |
| 25 | `event_orbital_decoy_swarm` | Severe | multiple_returns | 5 | ammo bait |
| 26 | `event_orbital_silent_entry` | Severe | none | 0 | no warning |
| 27 | `event_orbital_wet_impact` | Moderate | thermal_signature | 3 | flooding |
| 28 | `event_orbital_ash_plume` | Severe | seismic_precursor | 4 | fallout |
| 29 | `event_orbital_dead_hand_final` | Extreme | dead_hand_ping | 7 | season cap |
| 30 | `event_orbital_the_long_fall` | Extreme | all | 5 | capstone |

The false-positive and no-warning events are what make the battery doctrine matter.

---

## 28. APPENDIX L — FLIGHT WINDOW RULES

| Window | Wind | Visibility | Icing | Classification |
|---|---|---|---|---|
| Clear | < 0.4× tolerance | > 0.6 | none | `open` |
| Workable | < 0.7× tolerance | > 0.4 | mild | `open` |
| Marginal | < 1.0× tolerance | > 0.25 | moderate | `marginal` |
| Closed | ≥ tolerance | ≤ 0.25 | severe | `closed` |
| Grounded | storm | storm | severe | `closed` + forced |

Forecast confidence is a function of what is hardcoded in `flight_windows.json`.
No window is guaranteed; the forecast can be wrong, and that is the point.

---

## 29. APPENDIX M — LOCATION DETAIL

- **Hangar Boneyard** — half an airframe, a full engine, and someone else's tools.
- **Kite Tower** — a mast that only lifts when the wind is right; cheap but caps payload.
- **Beacon Ridge** — a light that has not turned in years; repairing it changes the map.
- **Wreck Marsh** — a graveyard of small aircraft and the logbooks that outlived them.
- **Picket Line** — a radar dish that still hums if you feed it power.
- **Balloon Farm** — a field of patched envelopes and a hydrogen generator that leaks.
- **Shield Works** — a pre-war defense factory, stripped but not empty.
- **Cliff Takeoff** — a silent launch, no engine, no noise, and no way back up.
- **Drop Marsh** — soft ground that saves canisters and swallows boots.
- **Neutral Field** — the only ground where rivals land together.

---

## 30. APPENDIX N — CONTENT REVIEW CHECKLIST

- [ ] No real aircraft, ordnance designation, or military unit is referenced.
- [ ] No air-war glorification; combat remains a failure of alternatives.
- [ ] Every aircraft has a real cost and no dominant stat line.
- [ ] Every ordnance has a trade-off; none is a one-shot answer.
- [ ] False alarms teach restraint.
- [ ] Harrow escalation stays bounded by authored events.
- [ ] Aerial reveal uses existing map progression and authored findings only.
- [ ] Crashes have consequence and a salvage path.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 31. APPENDIX O — GLOSSARY

- **Airway** — a named flight corridor with weather and interception metadata.
- **Window** — a weather classification (`open`/`marginal`/`closed`).
- **Airworthiness** — live aircraft condition; gates all flights.
- **Harrow season** — a recurring period of elevated orbital events.
- **Picket** — a radar station that extends warning lead time.
- **Neutral Field** — a shared airfield where rivals land together.
- **Commons** — the political question of who owns the sky.

---

## 33. APPENDIX P — SIDE QUEST DETAIL (24)

| Quest | Type | Objective kernel |
|---|---|---|
| `quest_sky_envelope_repair` | hangar | Patch envelope panels; leak test; ground or fly |
| `quest_sky_engine_teardown` | hangar | Salvage, clean, rebuild, test-run |
| `quest_sky_airworthiness_audit` | hangar | Inspect frame, cables, fabric; decide to ground |
| `quest_sky_control_cable` | hangar | Find rare wire; trace to a ruin; install |
| `quest_sky_fabric_shortage` | hangar | Envelope cloth vs. winter clothing |
| `quest_sky_window_wait` | weather | Hold for a window; manage crew temper |
| `quest_sky_icing_advice` | weather | Consult Seth; weigh his forecast |
| `quest_sky_night_landing` | weather | Return in dark; light the field; land |
| `quest_sky_headwind_choice` | weather | Turn back short or press with fuel margin |
| `quest_sky_balloon_ballast` | weather | Drop ballast to clear a ridge; lose cargo |
| `quest_sky_air_trade` | trade | Load, fly, land, sell, return |
| `quest_sky_drop_race` | airdrop | Race Kess's crew to a canister |
| `quest_sky_drop_wreck` | airdrop | Recover a broken canister without losing cargo |
| `quest_sky_trust_landing` | trade | Land at the Neutral Field under rules |
| `quest_sky_fuel_negotiation` | trade | Buy ultralight fuel at a painful price |
| `quest_sky_aerial_survey` | recon | Fly a grid; record findings |
| `quest_sky_anomaly_pass` | recon | Observe an anomaly; survive the exposure |
| `quest_sky_photo_run` | recon | Document a site from the air |
| `quest_sky_flight_only` | recon | Reach a location no road reaches |
| `quest_sky_radar_calibration` | battery | Align the dish; prove a track |
| `quest_sky_barrel_service` | battery | Replace a barrel; test-fire |
| `quest_sky_ammo_argument` | battery | Orel and the player disagree on a shell |
| `quest_sky_false_alarm` | battery | Identify a false track; do not fire |
| `quest_sky_after_impact` | battery | Triage, patch the roof, count the living |

---

## 34. APPENDIX Q — OPENING VIGNETTE (TONE SAMPLE)

> The engine turns over on the third pull and then dies. Mira swears at it without
> heat. Avano stands at the hangar door with his hands in his pockets and watches
> the ash move across the field in long grey lines, the way you watch water decide
> where to go. Nobody says the word wind. Everybody is thinking it.
>
> The balloon is up at the ridge, a dark thumbprint against a paler sky, and Seth
> is up there in the basket with the glasses he ground himself, counting the
> columns of smoke that only he can see. He will come down at dusk and say three
> sentences and one of them will matter.
>
> Somewhere past the corridor there is a shelter with an airframe and no pilot.
> Somewhere under the ash there is a canister with a beacon that will last four
> days. Somewhere above the weather there is a rod with the player's name written
> in a language nobody uses anymore.
>
> The engine turns over on the fourth pull.

This vignette sets the register: restraint, physical detail, no triumph. All
opening prose should match it.

---

## 35. CLOSING STATEMENT

The shelter already knows how to build a fragile aircraft and how to shoot a falling
rod out of the sky. What it does not yet have is a sky worth taking risks in:
weather windows that close, routes that connect settlements, airdrops that two
shelters race toward, a warning network that must be maintained, and a harrow
season that turns the whole map into a countdown. Above the Ash turns the existing
aviation and sky-defense machinery into a world of thin air and real fear, without
adding a single new authority or a single fantasy of air war.