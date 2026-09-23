# ASHFALL — Expansion 52 Design Bible
# THE WARM GROUND
### Wave 9 · Deep Drilling, Strata, Casing, Steam, Thermal Water, Hot Rooms, and the Heat Under the Shelter

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Shelter` (`GeothermalAquiferSystem`, `GeothermalOrcSystem`, `GeothermalCatalog`, `GeothermalAquiferState`), `Ashfall.Core` (`PowerGridSystem` generation seam), `Ashfall.Core.Shelter` (`ShelterThermalSystem` read seam), `Ashfall.Core.Inventory`
**Proposed host owner:** `WarmGroundHostSession` (extends `GeothermalAquiferHostSession` + `GeothermalAquiferSaveStore`)
**Existing save sections:** `geothermal_aquifer` (`geothermal_aquifer_save.json`, `GeothermalAquiferState`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no geothermal-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models deep heat. `GeothermalAquiferSystem` (`SystemId` =
"geothermal_aquifer") defines `GeothermalAquiferState` (`currentDepthMeters`,
`drillBitCondition` 100f, `steamPressurePsi`, `mineralScaling`,
`activeTurbineOutput`, `currentStrataId`, `installedCasingDepth`,
`turbineCommissioned`, `aquiferTapped`, `pressureReliefState` 0..1,
`generatorHealth` 100f, `projectActive`, `lastProcessedDay`,
`crossedStrataIds`) with the live constants `NominalTurbineOutputKw = 1500f`
and `DescalingChemicalCost = 2f`. Its API is real: `RegisterStrata`,
`LoadCatalog`, `GetEffectiveTurbineOutputKw`, `StartDrilling`, `CommissionTurbine`,
`Descale`, `TapAquifer`, `VentPressure`, `InstallCasing(depth)`, `TickDay`, and
`RestoreState`, returning results that report `AdvancedMeters`, `BitCondition`,
and `Depth`. `GeothermalStrataDef` describes strata with `StartDepthM`,
`EndDepthM`, `RockHardness`, `DrillWearPerMeter`, `SeismicHazard`,
`SteamTemperatureC`, `ThermalOutputKw`, `WaterYieldLitersPerDay`, and
`MinCasingTier`. A second live family, `GeothermalOrcSystem`, defines
`GeothermalStratumDefinition` (`BaseRockTemperatureC`,
`ThermalCapacityFactor`, `EnthalpyKjPerKg`, `BrineMineralIndex`,
`SilicaScalingFactor`, `CorrosionFactor`, `RecoveryRatePerDay`,
`MaxSafeExtractionKw`, `RequiredExcavationTier`, `RoomId`), `GeothermalStrataCatalog`,
and `GeothermalLoopState` (`BrineInletTempC`, `BrineOutletTempC`,
`BrineFlowLPerMin`, `WorkingFluidCharge`, `HeatExchangerFoulingPct`,
`TurbineConditionPct`, `BearingConditionPct`, `VibrationIndex`,
`BedrockThermalReserve`, `OperatingHours`, `Active`). The host session, save
store, and a headless demo already exist.

What does not exist: a campaign. The strata data is thin —
`geothermal_drilling_depths.json` holds **five rows** from caprock to magmatic
basement, and `geothermal_strata_catalog.json` holds **three** loop strata. There
are no drilling crews, no rig components, no casing tiers, no bit journals, no
descaling practice, no seismic risk content, no thermal water uses, no hot
rooms, no bathing, no greenhouse heat, and no story about why a shelter would
sink a hole a kilometer into the earth.

**The Warm Ground** is the expansion about the largest engineering project a
shelter can attempt: drilling for heat it will never see, in stages, with bits
that wear and rock that argues. It extends the live drilling and loop systems
and never touches the power, warmth, or water authorities.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `GeothermalAquiferSystem` | Depth, bit, casing, steam, tap | Extends with content and crews |
| `GeothermalOrcSystem` | Loops, brine, turbine, reserve | Extends with content and maintenance |
| `PowerGridSystem` (Wave 2) | Grid, allocation, priority | Sends generation into it; never allocates |
| 21 The Grid (Wave 2) | Power distribution | Reads; never rewrites |
| 42 The Core (Wave 7) | Nuclear generation | Parallel heat source; no reactor content |
| `ShelterThermalSystem` | Warmth and gear | Sends hot water/heat through it; never owns warmth |
| 27 The Thread (Wave 4) | Clothing and insulation | Reads warmth effect only |
| 22 The Clean Flow (Wave 3) | Water treatment | Sends thermal water to it; never treats water here |
| 40 The Wheel (Wave 6) | Mechanical power | Turbines and pumps are not wheels |
| 39 The Reagent (Wave 6) | Chemistry | Descaler is a reagent order; mixing stays there |
| 18 The Underneath (Wave 2) | Subterranean systems | The borehole is not a tunnel system |
| 15 The Deep Root (Wave 1) | Soil and greenhouses | Sends heat to them; never owns crops |
| 33 The Weather (Wave 5) | Seasons | Drilling respects winter; no weather authority |
| 46 The Long Change (Wave 7) | Ground change | Seismic observations file there |
| `StandingRecord` (Exp 03) | Records | Files drilling journals and tap records |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

There is a number the shelter has never tested: how deep the warm rock begins.
The driller's answer is five strata, four hundred meters of casing, a bit that
wears at a measurable rate, and a calculation that says the steam pocket under
the ridge could heat every room, every bath, and a greenhouse through the next
winter — if the shelter can afford to find it.

**The Warm Ground** is the expansion about the deep well: crews and rigs,
strata and casing, bits and breaks, steam and brine, descaling and pressure,
thermal water and hot rooms, and the slow conversion of a hole in the ground
into the shelter's quietest utility. It is the expansion about a project that
takes seasons, fails in inches, and pays for itself in warmth.

### 1.2 The five loops it adds

```
  Survey ──► Drill ──► Case ──► Tap ──► Heat
     │         │        │       │        │
     ▼         ▼        ▼       ▼        ▼
  Strata,   Bits,    Casing,  Steam,  Rooms,
  water     crews    cement   brine   baths
                                        │
                                        ▼
                          Loop ──► Maintain ──► Record
```

### 1.3 What the player manages

1. **Survey.** Where to drill and what the ground is made of.
2. **Crews.** Shifts, rig hands, drillers, and their fatigue.
3. **The rig.** Masts, motors, cable, bits, and spares.
4. **Strata.** Hardness, wear, seismic risk, and what each layer promises.
5. **Casing.** Tiers, depth, cement, and the safety it buys.
6. **The tap.** Steam, pressure, venting, and the moment the aquifer opens.
7. **Descaling and corrosion.** Chemistry ordered, not mixed.
8. **Loops and turbines.** ORC units, brine, bearings, vibration, and reserve.
9. **Thermal uses.** Hot water, hot rooms, baths, greenhouses, and process heat.
10. **Records.** Bit journals, depth logs, tap records, and the years of the well.

### 1.4 What it is not

- Not a second power system; generation flows through `PowerGridSystem`.
- Not a second warmth system; heat flows through `ShelterThermalSystem`.
- Not a second water authority; thermal water is delivered to treatment.
- Not a free infinite resource; reserve, scaling, and corrosion are real.
- Not a mining or tunnel expansion; the borehole is not a corridor.
- Not a reactor story; that is Expansion 42.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs` | Drilling, casing, tap | `LIVE` |
| `Assets/Ashfall.Core/Shelter/GeothermalAquiferState.cs` | Persisted well state | `LIVE` |
| `Assets/Ashfall.Core/Shelter/GeothermalOrcSystem.cs` | Loops, brine, turbines | `LIVE` |
| `Assets/Ashfall.Core/Shelter/GeothermalCatalog.cs` | Strata definition | `LIVE` |
| `Assets/Ashfall.Core/Shelter/GeothermalCatalogLoader.cs` | Catalog loading | `LIVE` |
| `src/Host/GeothermalAquiferHostSession.cs` | Host surface | `LIVE` |
| `src/Host/GeothermalAquiferSaveStore.cs` | `geothermal_aquifer` save | `LIVE` |
| `Assets/Ashfall.Core/PowerGridSystem.cs` | Generation sink | `LIVE` |
| `Assets/Ashfall.Core/ShelterThermalSystem.cs` | Heat and warmth sink | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `geothermal_drilling_depths.json` | 2,112 B | **5 strata** (caprock to magmatic basement) |
| `geothermal_strata_catalog.json` | 1,547 B | **3 loop strata** (granite, steam, basement) |
| Crew, rig, casing, bit journals | absent | confirmed none |
| Thermal use and bath content | absent | confirmed none |
| Steam turbine catalog | present | adjacent power content, not duplicated |

### 2.3 Confirmed gaps

- **GAP-52-1 — Five and three strata rows for a whole deep campaign.**
- **GAP-52-2 — No drilling crews, shifts, or rig content.**
- **GAP-52-3 — No bit, casing, or cement practice.**
- **GAP-52-4 — No seismic risk or geologist content.**
- **GAP-52-5 — No descaling, corrosion, or brine management content.**
- **GAP-52-6 — No loop maintenance or turbine care content.**
- **GAP-52-7 — No thermal water uses (rooms, baths, greenhouses, process).**
- **GAP-52-8 — No well records, journals, or depth logs.**
- **GAP-52-9 — No story or NPCs around the well.**
- **GAP-52-10 — The live project runs with no practice around it.**

### 2.4 Non-duplication statement

This expansion will **not** add a second power, warmth, water, chemistry,
mechanical, or subterranean system. It extends `GeothermalAquiferSystem` and
`GeothermalOrcSystem` with catalogs and practice, sends generation through
`PowerGridSystem`, heat through `ShelterThermalSystem`, thermal water through
the treatment owner, descaling through the reagent owner, and records through
`StandingRecord`. All new state is additive inside `GeothermalAquiferState` and
its loop save. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The ground is measured, never guessed.** A survey is honest about
what it does not know.

**Pillar 2 — Depth is earned in meters.** A campaign is seasons of small
progress, and the journal is the proof.

**Pillar 3 — Casing is safety.** Every metre of uncased hole is a risk the
shelter is choosing to carry.

**Pillar 4 — Heat is borrowed, not owned.** Scaling, corrosion, pressure, and
reserve make the well a relationship, not a tap.

**Pillar 5 — Warmth changes a house.** Hot water and a warm room are the
expansion's real product.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Drilling | Shifts, mud, noise, wear | Triumphal montage |
| Strata | Numbers and named rock | Mystical depths |
| Failure | Jammed bits, lost days | Catastrophe porn |
| Steam | Pressure gauges and care | Geyser spectacle |
| Hot water | Baths, kitchens, washes | Spa fantasy |
| Seismic | Monitoring and honesty | Earthquake doom |
| Maintenance | Descaling on schedule | Set-and-forget |
| The well | A utility with a personality | A living thing |

### 3.3 Content limits

- No real geological sites, wells, or companies copied.
- No dramatic blowouts as entertainment; failures are costly and survivable.
- No molten-rock fantasy; depths stay within the authored catalog bands.
- No spa luxury framing; warmth is domestic and modest.
- No drilling through graves, sacred sites, or occupied ground.
- No new save section.

---

## 4. THE WARM GROUND WORLD

### 4.1 Interior rooms

- **`room_rig_floor`** — the drill deck, mud, cable, and the brake.
- **`room_drill_shack`** — the driller's chair, gauges, and the depth board.
- **`room_bit_bench`** — bits, reamers, thread grease, and wear cards.
- **`room_cement_bay`** — casing joints, cement, and the tallies.
- **`room_steam_head`** — valves, gauges, and the vent line.
- **`room_loop_hall`** — ORC units, brine pumps, and vibration points.
- **`room_hot_room`** — the warm room where wet clothes dry.
- **`room_bath_house`** — hot water, stone, and the washing schedule.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_well_pad` | The Well Pad | 3 | The hole itself |
| `loc_survey_ridge` | The Survey Ridge | 2 | Where strata are read |
| `loc_mud_pits` | The Mud Pits | 3 | Returns and cuttings |
| `loc_steam_trench` | The Steam Trench | 3 | Buried pipe runs |
| `loc_cistern_warm` | The Warm Cistern | 2 | Thermal water storage |
| `loc_greenhouse_loop` | The Heated Rows | 2 | Greenhouse heat |
| `loc_vent_hill` | The Vent Hill | 3 | Pressure relief |
| `loc_seismic_post` | The Seismic Post | 3 | Ground watch |
| `loc_spare_pad` | The Spare Pad | 2 | Second site option |
| `loc_well_stone` | The Well Stone | 2 | Records and depth marks |

All locations require valid item or map-node references and scanner registration.

### 4.3 The rhythm

A drilling shift is a day; a casing run is a week; a stratum break is a season;
a tap is a year. The expansion's clock is the metre.

---

## 5. MAIN STORYLINE — "WHAT THE SHELTER SITS ON"

### 5.1 Central conflict

**Kova Rath** has drilled water wells for years and knows exactly how much she
does not know about four hundred meters. **Gilroy Fenwick** has read the old
survey and believes the steam pocket is real, which is a belief he can defend
with three numbers and a map. **Marl Skarn** makes bits and hates drilling past
the point where a bit should have been changed. **Anwen Gorse** wants the heat
in the rooms, because the shelter's winter is a hardship it can now afford to
answer. **Hurn Delft** wants every metre cased, because he has seen what a
pressured aquifer does to an open hole. **Tilda Bramble** wants the ORC units
maintained like aircraft, because a turbine that fails in January is a failure
the whole shelter feels.

The campaign is a war of inches. The caprock goes fast and the granite does not.
A bit seizes and costs eleven days. The steam pocket arrives with a roar that
nobody puts in the report, and the pressure test is a week of venting and
watching. Then the winter comes the same month as the tap, and the shelter has
to choose between heating the rooms and heating the greenhouse that feeds them.
The well does not solve the winter; it changes what winter means.

The expansion's question: **what does a shelter owe the ground it lives on?**

### 5.2 Theme (unspoken)

**Warmth is the slowest thing a shelter builds.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_driller_kova_rath` | Kova Rath | Driller | Depth, shifts, decisions |
| `npc_geologist_gilroy_fenwick` | Gilroy Fenwick | Geologist | Strata and survey |
| `npc_bit_marl_skarn` | Marl Skarn | Bit shop | Wear, steel, changes |
| `npc_heat_anwen_gorse` | Anwen Gorse | Thermal systems | Rooms, baths, loops |
| `npc_casing_hurn_delft` | Hurn Delft | Casing lead | Cement and safety |
| `npc_turbine_tilda_bramble` | Tilda Bramble | Turbine operator | ORC care |
| `npc_pressure_ib_fairweather` | Ib Fairweather | Pressure | Gauges and venting |
| `npc_apprentice_rennick_wold` | Rennick Wold | Apprentice | Journals and cards |

### 5.4 Story beats (15)

1. **The Survey.** The old numbers are re-read and doubted.
2. **The Pad.** The first site is leveled and marked.
3. **The Caprock.** Drilling is fast and teaches nothing.
4. **The Granite.** The bit wears and the crew learns meters per day.
5. **The Change.** Marl forces a bit change and saves a week.
6. **The Break.** A break costs eleven days and a lesson.
7. **The Casing.** Hurn cases to depth while the hole is quiet.
8. **The Pocket.** The steam pocket answers the gauge.
9. **The Vent.** Pressure is released slowly over a week.
10. **The Tap.** The aquifer opens and the well becomes a utility.
11. **The Loop.** The ORC units run and the grid feels them.
12. **The Hot Water.** Baths, kitchens, and a warm room.
13. **The Winter Choice.** Rooms or greenhouse, and the number that decides.
14. **The Scale.** Descaling on schedule and the well's second life.
15. **What the Shelter Sits On.** The well becomes ordinary.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Site | ridge / spare pad / both | ambition |
| Casing | full / minimum / to depth only | safety vs. cost |
| Crew pace | press on / measured / cautious | wear vs. time |
| Steam use | power / heat / both | priority |
| Winter | rooms first / greenhouse first / split | care |
| Descaling | scheduled / when weak / never | longevity |
| Records | full / tap only / none | accountability |
| Final | utility / public warmth / experiment | identity |

### 5.6 Endings (5 + fade)

1. **The Warm House** — the well heats rooms, baths, and rows, and winter
   becomes a season instead of an emergency.
2. **The Deep Utility** — the well runs the ORC units and carries the grid,
   and the shelter's power has a second heart.
3. **The Cased Hole** — the shelter spends its ambition on safety, stops at a
   shallower depth, and gets a smaller, utterly reliable well.
4. **The Measured Ground** — the campaign is recorded so carefully that the
   next shelter drills in half the time.
5. **The Second Site** — the ridge is abandoned honestly after a bad stratum,
   and the spare pad becomes the well with no shame in the move.
6. **Fade** — a warm room in February, wet clothes drying on a line, steam
   between two buildings, and a depth board with the last metre written in
   chalk.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_warm_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_warm_survey`, `quest_warm_pad`, `quest_warm_caprock`,
`quest_warm_granite`, `quest_warm_change`, `quest_warm_break`,
`quest_warm_casing`, `quest_warm_pocket`, `quest_warm_vent`, `quest_warm_tap`,
`quest_warm_loop`, `quest_warm_hot_water`, `quest_warm_winter_choice`,
`quest_warm_scale`, `quest_warm_what_we_sit_on`.

### 6.2 Side quests (30)

**Survey (5)**
- `quest_warm_read_strata` — strata read
- `quest_warm_water_test` — ground water test
- `quest_warm_seismic_net` — seismic posts set
- `quest_warm_site_mark` — site marked
- `quest_warm_old_map` — old survey compared

**Rig and crew (5)**
- `quest_warm_rig_up` — rig raised
- `quest_warm_shift_rota` — shifts set
- `quest_warm_hand_drill` — hand skills
- `quest_warm_spares` — spares stocked
- `quest_warm_rig_winter` — rig winterized

**Bits and steel (5)**
- `quest_warm_bit_log` — bit journal kept
- `quest_warm_bit_shop` — bit shop run
- `quest_warm_thread` — threads greased
- `quest_warm_ream` — hole reamed
- `quest_warm_measure_wear` — wear measured

**Casing and cement (5)**
- `quest_warm_casing_tally` — casing counted
- `quest_warm_cement_mix` — cement batch tested
- `quest_warm_case_run` — casing run made
- `quest_warm_pressure_seal` — seal tested
- `quest_warm_casing_card` — card filed

**Steam and loops (5)**
- `quest_warm_gauge_check` — gauges checked
- `quest_warm_vent_drill` — vent drilled
- `quest_warm_brine_sample` — brine sampled
- `quest_warm_fouling` — fouling cleaned
- `quest_warm_bearing` — bearings checked

**Thermal use (5)**
- `quest_warm_hot_room` — hot room opened
- `quest_warm_bath_wash` — bath schedule
- `quest_warm_kitchen_heat` — kitchen hooked
- `quest_warm_row_heat` — greenhouse heated
- `quest_warm_dry_line` — drying line run

### 6.3 Repeatable quests (8)

`quest_warm_repeat_depth`, `quest_warm_repeat_bit`, `quest_warm_repeat_scale`,
`quest_warm_repeat_gauge`, `quest_warm_repeat_brine`,
`quest_warm_repeat_bearing`, `quest_warm_repeat_journal`,
`quest_warm_repeat_heat`.

### 6.4 Dynamic hooks

Live events (`StartDrilling`, `InstallCasing`, `TapAquifer`, `VentPressure`,
`Descale`, `CommissionTurbine`, `TickDay`, grid state changes, winter weather,
seismic observations) attach authored follow-ups through existing seams. No new
event bus.

### 6.5 Constraints

- Depth, pressure, scaling, and turbine state stay with the live systems.
- Generation flows through `PowerGridSystem`; no direct allocation.
- Heat flows through `ShelterThermalSystem`; no second warmth model.
- Thermal water goes to the treatment owner; no second water authority.
- Descaling chemistry is ordered from the reagent owner.
- Seismic observations file with the world-change record owner.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `DrillingCampaignSystem` (extend `GeothermalAquiferSystem`)

**Owns:** siting, surveys, shift pacing, depth journals, and the campaign
calendar. **Consumes:** strata catalog, crews, weather. **Data:**
`well_strata_catalog.json` (extended), `well_surveys.json`. **Rules:** the
campaign advances only through real drilling results; shifts have fatigue;
winter slows or stops the rig; a site can be retired honestly.

### 7.2 `RigSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** rig components, masts, motors, cable, pumps, and rig condition.
**Consumes:** inventory, workshop repairs. **Data:** `well_rig_parts.json`.
**Rules:** rig condition multiplies progress and wear; parts are real; a
neglected rig costs days it never gets back.

### 7.3 `BitSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** bits, reamers, wear cards, change policy, and steel supply.
**Consumes:** workshop, bit shop, inventory. **Data:** `well_bits.json`.
**Rules:** wear per metre comes from the live strata values; a bit run past its
limit risks a break; the journal records every change and every break.

### 7.4 `CasingSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** casing tiers, joints, cement batches, depth tallies, and seal tests.
**Consumes:** inventory, kiln and workshop for fittings. **Data:**
`well_casing.json`. **Rules:** every live `MinCasingTier` is honored or
exceeded; an uncased interval is a recorded risk; seal tests are pass/fail with
retests.

### 7.5 `SteamHeadSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** pressure discipline, gauges, venting, relief, and tap safety.
**Consumes:** the live pressure fields, seismic posts. **Data:**
`well_steam.json`. **Rules:** venting is slow and scheduled; pressure above
authored bands forces action; nobody taps an aquifer on a bad day.

### 7.6 `LoopMaintenanceSystem` (extend `GeothermalOrcSystem`)

**Owns:** loop cleaning, fouling, corrosion, bearings, vibration, working fluid,
and reserve discipline. **Consumes:** workshop, metrology, reagent for
inhibitors. **Data:** `well_loops.json`. **Rules:** fouling and vibration are
measured weekly; cleaning restores output; draw stays under `MaxSafeExtractionKw`;
reserve is a real, slowly recovering number.

### 7.7 `ThermalUseSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** heat budgets for rooms, baths, kitchens, drying, and greenhouse rows.
**Consumes:** the grid, thermal owner, greenhouse owner. **Data:**
`thermal_uses.json`. **Rules:** heat is allocated by authored budgets, never by
ad-hoc scripting; domestic uses come before process uses in a cold snap unless
the shelter chooses otherwise and records it.

### 7.8 `WellRecordSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** depth boards, bit journals, casing cards, tap records, and the well
stone. **Data:** `well_records.json`. Records through `StandingRecord`.
**Rules:** every metre drilled is recorded with its day; failures are recorded
with their lesson; the stone records depth and date for whoever comes next.

### 7.9 Systems explicitly not added

- No second power, warmth, water, chemistry, or subterranean system.
- No infinite free energy; reserve, scaling, and corrosion apply.
- No blowout spectacle; failures are costly and contained.
- No mining, tunnels, or cave content.
- No reactor content.
- No new currency.
- No new RNG stream beyond the live day path.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `well_strata_catalog.json` (extend `geothermal_drilling_depths.json`)

```json
{
  "schema_version": 1,
  "strata": [
    {
      "strata_id": "strata_shale_150",
      "display_name": "Fractured Shale",
      "start_depth_m": 100,
      "end_depth_m": 150,
      "rock_hardness": 3.0,
      "drill_wear_per_meter": 0.8,
      "seismic_hazard": 0.1,
      "steam_temperature_c": 0,
      "thermal_output_kw": 0,
      "water_yield_liters_per_day": 120,
      "min_casing_tier": 1,
      "tags": ["dry", "shale"]
    }
  ]
}
```

### 8.2 `well_surveys.json` (new)

Surveys: site, depth target, strata expected, confidence, water findings.

### 8.3 `well_rig_parts.json` (new)

Rig parts: id, kind, condition band, wear rate, repair route, spares.

### 8.4 `well_bits.json` (new)

Bits: id, size, steel, hardness band, wear limit, change rule, cost.

### 8.5 `well_casing.json` (new)

Casing: tier, joint, depth band, cement mix, test pressure, card fields.

### 8.6 `well_steam.json` (new)

Steam: pressure band, relief rule, vent schedule, gauge calibration.

### 8.7 `well_loops.json` (new)

Loops: loop id, stratum, working fluid, cleaning interval, vibration limit.

### 8.8 `thermal_uses.json` (new)

Uses: use id, room, heat demand, priority band, winter rule, note.

### 8.9 `well_records.json` (new)

Records: day, depth, bit, break, casing, tap, lesson, keeper.

### 8.10 `well_crews.json` (new)

Crews: role, shift, skill, fatigue band, pair rule, training path.

### 8.11 Items

New items appended to `items.json`: `item_drill_bit`, `item_bit_reamer`,
`item_casing_joint`, `item_cement_sack`, `item_thread_grease`,
`item_pressure_gauge`, `item_drill_mud`, `item_descale_batch`,
`item_brine_flask`, `item_bearing_set_well`, `item_vibration_meter`,
`item_depth_chain`, `item_seismic_spike`, `item_steam_valve`,
`item_well_stone_chisel`, `item_warm_room_cloth`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`GeothermalAquiferState` remains the live save owner; loop state rides its
existing loop save. New sub-objects (surveys, rig, bits, casing, steam, uses,
records, crews) are additive inside it. No new save section.

### 9.2 State to persist

- Site, depth, bit condition, casing depth, and crossed strata.
- Rig components and condition.
- Bit journal and steel stock.
- Casing card and seal tests.
- Pressure state, venting, and gauge calibration.
- Loop fouling, corrosion, bearings, working fluid, and reserve.
- Thermal use budget and winter priority.
- Records: meters, failures, tap, and lessons.

### 9.3 Determinism

- Drilling progress derives from live hardness, wear, and rig condition.
- Seismic rolls come from the live seeded path, never wall-clock.
- Tap and pressure outcomes derive from live state and authored bands.
- Turbine output derives from the live ORC state and reserve.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with depth, casing, bit condition, and tap state intact; no
rig, bits, records, or thermal budgets exist until started. A part-drilled well
resumes at its recorded depth; a tapped well keeps its pressure and loop state.

### 9.5 Checksum

Invariant-culture floats; integer day, depth, and count fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `GeothermalPanel` (extend) | Depth and projects | `WarmGroundHostSession` |
| `RigPanel` (new) | Rig and crew state | same |
| `BitJournalPanel` (new) | Wear and changes | same |
| `CasingPanel` (new) | Tiers and tests | same |
| `SteamHeadPanel` (new) | Pressure and venting | same |
| `LoopPanel` (new) | Brine and turbines | same |
| `ThermalUsePanel` (new) | Heat budgets | same |
| `WellRecordPanel` (new) | Journals and stone | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Depth is shown in metres with days of progress, never as a vague bar.
- Pressure is shown as a number with its safe band, never as dread.
- Failures are explained in text: what jammed, what it costs, what changes.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- No reflex input; venting and tests are scheduled, not timed puzzles.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a drill turning in hard rock, a chain
under load, a gauge hiss, steam through a trench, a bath filling, a depth chalk
on a board. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `GeothermalAquiferSystem` | Depth, casing, tap, pressure |
| `GeothermalOrcSystem` | Loops, brine, turbine, reserve |
| `PowerGridSystem` (Wave 2) | Generation delivered |
| `ShelterThermalSystem` | Heat and warmth delivered |
| `KineticStorageSystem` (Wave 6) | Turbine smoothing |
| 22 The Clean Flow (Wave 3) | Thermal water to treatment |
| 39 The Reagent (Wave 6) | Descaler and inhibitor orders |
| 40 The Wheel (Wave 6) | Pumps and mechanical parts |
| 15 The Deep Root (Wave 1) | Heated greenhouse rows |
| 33 The Weather (Wave 5) | Winter pacing |
| 46 The Long Change (Wave 7) | Seismic observations |
| `ExcavationSystem` (Wave 2) | Ground works at the pad |
| `ShelterWorkshopSystem` (Wave 6) | Rig and bit repairs |
| `PrecisionMetrologySystem` (Wave 6) | Gauge and vibration calibration |
| `DutyRoster` (Exp 02) | Drilling shifts |
| `Inventory` | Steel, cement, spares |
| `StandingRecord` (Exp 03) | Journals and cards |
| `EpilogueChronicleBuilder` | Well history lines |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm drilling and loop systems, host, save
store, power, thermal, water, reagent, and records owners. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Extend the strata catalogs; author the nine
new catalogs; register validators and scanner.

**Phase 2 — Pure Core.** `DrillingCampaignSystem`, `RigSystem`, `BitSystem`,
`CasingSystem`, `SteamHeadSystem`, `LoopMaintenanceSystem`, `ThermalUseSystem`,
`WellRecordSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WarmGroundHostSession`, focused selftest coverage,
fresh journey from the survey to the warm room.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-winter soak: drilling pace, wear, reserve,
scaling, winter priority, and warm-room payoff.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Strata (total) | 14 |
| Surveys | 8 |
| Rig parts | 16 |
| Bits | 12 |
| Casing tiers | 6 |
| Steam rules | 10 |
| Loops | 8 |
| Thermal uses | 12 |
| Records | 24 |
| Crews | 8 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Power duplication | Critical | Grid sink only |
| Warmth duplication | Critical | Thermal owner only |
| Free energy | High | Reserve and scaling |
| Catastrophe spectacle | High | Costly, contained failure |
| Chemistry authority | Medium | Reagent orders |
| Water authority | Medium | Treatment owner |
| Determinism break | Low | Live day path |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `well_strata_catalog.json` | 9 new | 2,500 |
| `well_surveys.json` | 8 | 2,000 |
| `well_rig_parts.json` | 16 | 3,000 |
| `well_bits.json` | 12 | 2,500 |
| `well_casing.json` | 6 | 1,500 |
| `well_steam.json` | 10 | 2,000 |
| `well_loops.json` | 8 | 2,000 |
| `thermal_uses.json` | 12 | 2,500 |
| `well_records.json` | 24 | 4,000 |
| `well_crews.json` | 8 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~55,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R52-1 | Power overlap | Low | Critical | Grid sink |
| R52-2 | Warmth overlap | Low | Critical | Thermal owner |
| R52-3 | Free energy | Med | High | Reserve |
| R52-4 | Spectacle | Med | High | Contained failure |
| R52-5 | Chemistry | Low | Medium | Orders |
| R52-6 | Water | Low | Medium | Treatment |
| R52-7 | Determinism | Low | High | Live path |
| R52-8 | Content overrun | Med | Medium | Budget |
| R52-9 | Grind | Med | Medium | Journals and milestones |
| R52-10 | Ground ethics | Low | High | No graves or sacred sites |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How deep can the campaign go?** Recommended: one kilometer, with the
   magmatic basement as the authored floor and no deeper strata.
2. **Can the well fail permanently?** Recommended: no; a bad site is retired
   honestly and the spare pad exists, and a well once tapped stays serviceable
   with maintenance.
3. **Who gets heat first in a cold snap?** Recommended: domestic warmth before
   process heat by default, with a recorded override.
4. **Is the reserve finite?** Recommended: effectively finite per campaign;
   draw above the authored safe rate reduces recovery.
5. **Does the well ever power the whole shelter?** Recommended: no; it is a
   second heart, not a replacement, and the grid remains the authority.

---

## 17. APPENDIX D — STRATA TABLE (14 STRATA)

| # | Stratum | Depth m | Hardness | Wear/m | Seismic | Temp C | kW |
|---|---|---|---|---|---|---|---|
| 1 | Soil and fill | 0-20 | 1.0 | 0.2 | 0.00 | 0 | 0 |
| 2 | Caprock | 0-100 | 2.0 | 0.5 | 0.05 | 0 | 0 |
| 3 | Fractured shale | 100-150 | 3.0 | 0.8 | 0.10 | 0 | 0 |
| 4 | Wet sandstone | 150-220 | 2.5 | 0.6 | 0.05 | 20 | 0 |
| 5 | Hot granite | 100-300 | 4.0 | 1.2 | 0.15 | 60 | 120 |
| 6 | Hard basalt | 220-320 | 5.5 | 1.8 | 0.20 | 80 | 200 |
| 7 | Steam pocket | 300-500 | 5.0 | 1.5 | 0.25 | 180 | 1500 |
| 8 | Clay seal | 400-460 | 2.0 | 0.4 | 0.05 | 90 | 0 |
| 9 | Artesian aquifer | 500-750 | 6.0 | 2.0 | 0.20 | 110 | 400 |
| 10 | Sulphur band | 620-700 | 4.5 | 1.4 | 0.30 | 150 | 250 |
| 11 | Magmatic basement | 750-1000 | 8.0 | 3.0 | 0.45 | 350 | 3000 |
| 12 | Deep basement heat | 900-1000 | 7.0 | 2.4 | 0.35 | 280 | 2200 |
| 13 | Cold fault | any | 6.5 | 2.2 | 0.50 | 40 | 0 |
| 14 | Basement cap | 950-1000 | 9.0 | 3.5 | 0.40 | 320 | 1800 |

Fourteen strata from fill to basement cap, with wear, seismic hazard,
temperature, and thermal output side by side. The two rows worth designing
around are the steam pocket that arrives at three hundred metres with fifteen
hundred kilowatts and the cold fault that can appear at any depth and teach the
crew why surveys always carry an uncertainty column.

---

## 18. APPENDIX E — SURVEY TABLE

| # | Survey | Site | Target | Confidence | Finding |
|---|---|---|---|---|---|
| 1 | Old reading | ridge | 500 m | low | steam believed |
| 2 | Water tasting | pad | 200 m | med | clean sandstone |
| 3 | Seismic line | ridge | 800 m | med | fault suspected |
| 4 | Soil gas | pad | 100 m | high | normal |
| 5 | Spare pad check | spare | 400 m | med | dry shale |
| 6 | Depth probe | ridge | 1000 m | low | basement deep |
| 7 | Fault trace | ridge east | any | med | cold fault |
| 8 | Full site report | both | 1000 m | med | ridge preferred |

Eight surveys with confidence bands instead of promises, and the cold-fault
finding is deliberately discovered before drilling so that the campaign's
twist is a known risk the shelter chooses to accept rather than a surprise
designed to punish the player.

---

## 19. APPENDIX F — RIG PARTS TABLE

| # | Part | Kind | Condition band | Wear | Repair |
|---|---|---|---|---|---|
| 1 | Mast | frame | good / bent | low | workshop |
| 2 | Draw works | motor | good / worn | med | shop |
| 3 | Cable | consumable | service / frayed | high | replace |
| 4 | Kelly | steel | good / scored | med | shop |
| 5 | Swivel | seal | good / leak | med | seal kit |
| 6 | Mud pump | motor | good / weak | med | shop |
| 7 | Shale shaker | screen | good / torn | high | screen |
| 8 | Rotary table | gear | good / loose | low | shop |
| 9 | Brake | friction | good / thin | high | reline |
| 10 | Hoist | cable | good / jam | med | shop |
| 11 | Cathead | drum | good / slack | med | shop |
| 12 | Pipe rack | frame | good / bent | low | shop |
| 13 | Doghouse stove | heater | good / cold | low | stove |
| 14 | Spare motor | motor | spare | none | stored |
| 15 | Tool kit | tools | good / short | med | store |
| 16 | Mud hose | consumable | good / split | high | replace |

Sixteen rig parts with wear and repair routes, and the consumable rows are the
campaign's real texture: cable and hose and brake lining are what the rig eats,
and a shelter that forgets to kit them loses days it cannot invoice to anything.

---

## 20. APPENDIX G — BIT TABLE

| # | Bit | Size | Steel | Hardness band | Wear limit | Rule |
|---|---|---|---|---|---|---|
| 1 | Light steel | 150 mm | carbon | soft | 40 | change at 30 |
| 2 | Standard rock | 150 mm | alloy | medium | 60 | change at 50 |
| 3 | Hard formation | 150 mm | tungsten | hard | 80 | change at 65 |
| 4 | Diamond insert | 150 mm | insert | very hard | 120 | change at 100 |
| 5 | Reamer | 160 mm | alloy | medium | 70 | ream after break |
| 6 | Coring bit | 100 mm | tungsten | hard | 90 | core samples |
| 7 | Small bore | 80 mm | alloy | medium | 50 | probe holes |
| 8 | Heavy head | 180 mm | tungsten | hard | 140 | deep runs |
| 9 | Rescue bit | 150 mm | alloy | any | 30 | break recovery |
| 10 | Old bit | 150 mm | carbon | soft | 20 | training |
| 11 | Thread set | n/a | steel | n/a | n/a | grease each |
| 12 | Spare lot | 150 mm | mixed | mixed | varies | stored |

Twelve bits with explicit change rules, and the rules are the ethics: drilling
a hundred and twenty metres on a bit rated for a hundred is not bravery, it is
a break waiting to be scheduled, and Marl's whole job is to be the person who
says so before the hole decides.

---

## 21. APPENDIX H — CASING TABLE

| # | Tier | Depth band | Joint | Cement mix | Test |
|---|---|---|---|---|---|
| 1 | Tier one | 0-100 | screw | light | visual |
| 2 | Tier two | 100-300 | welded | medium | pressure |
| 3 | Tier three | 300-500 | welded | heavy | pressure |
| 4 | Tier four | 500-750 | welded | heavy | pressure |
| 5 | Tier five | 750-1000 | welded sleeve | special | two-stage |
| 6 | Liner | any interval | slotted | none | flow check |

Six casing tiers with tests that climb in seriousness with depth, and the liner
row exists because a shelter that cannot afford full tier-three pipe can still
protect an interval with a slotted liner — a smaller safety bought honestly
instead of a larger one only promised.

---

## 22. APPENDIX I — STEAM RULE TABLE

| # | Rule | Band | Action | Owner |
|---|---|---|---|---|
| 1 | Normal | 0-120 psi | monitor | pressure lead |
| 2 | Caution | 120-160 psi | check gauges | pressure lead |
| 3 | High | 160-200 psi | slow draw | driller |
| 4 | Vent required | 200-240 psi | vent on schedule | pressure lead |
| 5 | Emergency | over 240 psi | vent fully | driller |
| 6 | Cold start | below 20 psi | warm line | turbine op |
| 7 | Gauge drift | any | recalibrate | metrology |
| 8 | Valve stiff | any | service | workshop |
| 9 | Seep found | any | log and fix | casing lead |
| 10 | After quake | any | full check | all |

Ten pressure rules, written as bands with named owners instead of alarms, and
the last row is the one the expansion wants players to internalize: after the
ground moves, every gauge is read before anything else happens.

---

## 23. APPENDIX J — LOOP TABLE

| # | Loop | Stratum | Fluid | Clean | Vibration limit |
|---|---|---|---|---|---|
| 1 | Ridge primary | steam pocket | low-boil mix | 60 days | 4.0 mm/s |
| 2 | Ridge secondary | steam pocket | low-boil mix | 60 days | 4.0 mm/s |
| 3 | Deep loop | basement heat | high-temp mix | 90 days | 5.0 mm/s |
| 4 | Aquifer loop | artesian aquifer | brine-safe | 45 days | 3.5 mm/s |
| 5 | Winter loop | shared | low-boil mix | 30 days | 3.5 mm/s |
| 6 | Bath loop | any warm | water | 30 days | none |
| 7 | Row loop | any warm | water | 45 days | none |
| 8 | Reserve loop | spare | low-boil mix | 90 days | 4.5 mm/s |

Eight loops with cleaning intervals and vibration limits, and the maintenance
column is where the ORC owner earns their keep: a loop cleaned on schedule
returns output for years, and one cleaned when weak returns it once more, as a
repair instead of a service.

---

## 24. APPENDIX K — THERMAL USE TABLE

| # | Use | Demand kW | Priority | Winter rule | Note |
|---|---|---|---|---|---|
| 1 | Dormitory heat | 40 | first | always | rooms |
| 2 | Clinic heat | 25 | first | always | patients |
| 3 | Bath house | 30 | second | every other day | hygiene |
| 4 | Kitchen | 20 | first | always | food |
| 5 | Drying room | 15 | second | daily | wet clothes |
| 6 | Laundry | 18 | second | alternate | wash |
| 7 | Greenhouse rows | 60 | second | daytime | food |
| 8 | Seed store | 12 | first | always | farm |
| 9 | Workshops | 20 | third | working days | process |
| 10 | Archive dry heat | 8 | third | gentle | paper |
| 11 | Bath loop | 10 | fourth | spare | standby |
| 12 | Spare capacity | — | reserve | event | snowmelt |

Twelve thermal uses with priority bands, and the winter-rule column is the
policy the shelter argues over: the clinic and the seed store never lose heat,
the greenhouse loses it by day instead of by night, and the workshops take the
first cut — because a shelter's heat budget is a set of promises about who is
cold, and the plan writes them down before the first frost.

---

## 25. APPENDIX L — WELL RECORD TABLE

| # | Day | Depth m | Event | Actor | Lesson |
|---|---|---|---|---|---|
| 1 | 4 | 0 | pad marked | survey | site chosen |
| 2 | 12 | 100 | caprock done | crew | fast rock lies |
| 3 | 26 | 150 | shale crossed | crew | water found |
| 4 | 40 | 220 | granite entered | crew | wear doubled |
| 5 | 58 | 300 | bit changed | Marl | rule kept |
| 6 | 70 | 340 | bit seized | crew | 11 days lost |
| 7 | 96 | 420 | break freed | rescue bit | patience |
| 8 | 120 | 500 | pocket entered | crew | roar logged |
| 9 | 134 | 500 | vent week | Ib | slow is safe |
| 10 | 160 | 500 | casing run | Hurn | full tier |
| 11 | 175 | 500 | seal test | Hurn | passed |
| 12 | 180 | 500 | tap day | Kova | well born |
| 13 | 188 | 500 | loop start | Tilda | output |
| 14 | 190 | 500 | grid feed | grid | second heart |
| 15 | 200 | 500 | first bath | Anwen | warmth |
| 16 | 240 | 500 | scale check | loops | clean |
| 17 | 300 | 500 | winter first | all | rooms warm |
| 18 | 320 | 500 | winter choice | steward | rows first |
| 19 | 365 | 500 | year noted | record | one year |
| 20 | 380 | 500 | deep survey | Gilroy | next? |
| 21 | 400 | 500 | scale batch | reagent | routine |
| 22 | 430 | 500 | bearing swap | Tilda | care |
| 23 | 500 | 500 | second tap | none | stable |
| 24 | 730 | 500 | stone cut | Rennick | depth kept |

Twenty-four records across two years, and the nineteen-day run from tap to
first bath is the expansion's arc in miniature: a hole, a gauge, a week of
venting, a casing run, and then a person warm enough to wash in a bath in
February. The stone row at the end is the promise that a future reader knows
exactly how far the shelter went.

---

## 26. APPENDIX M — CREW TABLE

| # | Role | Shift | Skill | Fatigue band | Pair rule |
|---|---|---|---|---|---|
| 1 | Driller | day | drilling | high | never alone at brake |
| 2 | Assistant | day | rigging | med | with driller |
| 3 | Mud hand | day | pumps | med | no |
| 4 | Bit hand | day | steel | med | with bit shop |
| 5 | Casing hand | casing day | pipe | high | two on joint |
| 6 | Cement hand | casing day | mix | high | with lead |
| 7 | Night watch | night | gauges | low | no |
| 8 | Geologist | day | strata | low | no |

Eight crew roles with shifts, fatigue bands, and pair rules, and the first row
carries the rule the whole campaign hangs on: nobody runs the brake alone,
because the person who gets tired is the person at the controls, and the
shelter's way of saying so is a rota instead of a warning.

---

## 27. APPENDIX N — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_warm_survey` | 4 | Survey read |
| `quest_warm_pad` | 3 | Pad leveled |
| `quest_warm_caprock` | 3 | Caprock crossed |
| `quest_warm_granite` | 4 | Granite learned |
| `quest_warm_change` | 3 | Bit rule kept |
| `quest_warm_break` | 5 | Break recovered |
| `quest_warm_casing` | 4 | Casing run done |
| `quest_warm_pocket` | 4 | Pocket reached |
| `quest_warm_vent` | 3 | Vent completed |
| `quest_warm_tap` | 5 | Aquifer tapped |
| `quest_warm_loop` | 4 | Loops commissioned |
| `quest_warm_hot_water` | 3 | First bath |
| `quest_warm_winter_choice` | 4 | Heat priority set |
| `quest_warm_scale` | 3 | Descale routine |
| `quest_warm_what_we_sit_on` | 3 | Well ordinary |

---

## 28. APPENDIX O — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_warm_read_strata` | 3 | Strata read |
| `quest_warm_water_test` | 3 | Water tested |
| `quest_warm_seismic_net` | 4 | Posts set |
| `quest_warm_site_mark` | 3 | Site marked |
| `quest_warm_old_map` | 3 | Old survey compared |
| `quest_warm_rig_up` | 3 | Rig raised |
| `quest_warm_shift_rota` | 3 | Shifts set |
| `quest_warm_hand_drill` | 3 | Hand skills |
| `quest_warm_spares` | 3 | Spares stocked |
| `quest_warm_rig_winter` | 3 | Rig winterized |
| `quest_warm_bit_log` | 3 | Journal kept |
| `quest_warm_bit_shop` | 3 | Shop run |
| `quest_warm_thread` | 3 | Threads greased |
| `quest_warm_ream` | 3 | Hole reamed |
| `quest_warm_measure_wear` | 3 | Wear measured |
| `quest_warm_casing_tally` | 3 | Casing counted |
| `quest_warm_cement_mix` | 4 | Mix tested |
| `quest_warm_case_run` | 4 | Casing run |
| `quest_warm_pressure_seal` | 3 | Seal tested |
| `quest_warm_casing_card` | 3 | Card filed |
| `quest_warm_gauge_check` | 3 | Gauges checked |
| `quest_warm_vent_drill` | 3 | Vent drilled |
| `quest_warm_brine_sample` | 3 | Brine sampled |
| `quest_warm_fouling` | 3 | Fouling cleaned |
| `quest_warm_bearing` | 3 | Bearings checked |
| `quest_warm_hot_room` | 3 | Hot room opened |
| `quest_warm_bath_wash` | 3 | Bath scheduled |
| `quest_warm_kitchen_heat` | 3 | Kitchen hooked |
| `quest_warm_row_heat` | 4 | Rows heated |
| `quest_warm_dry_line` | 3 | Drying line run |

---

## 29. APPENDIX P — NPC DOSSIERS (BRIEF)

**Kova Rath** — driller. Has drilled water wells her whole life and knows that
four hundred metres is a different trade with the same words. Believes a hole
is only as good as its worst hour.

**Gilroy Fenwick** — geologist. Reads old surveys for the omissions and writes
uncertainty next to every number. Believes the ground is honest if you admit
you do not know it.

**Marl Skarn** — bit shop. Makes and dresses bits and will stop a run over a
chart line. Believes steel tells the truth.

**Anwen Gorse** — thermal systems. Wants the shelter warm enough that nobody
comes back from winter with cracked hands. Believes warmth is a form of care.

**Hurn Delft** — casing lead. Cases every metre he can and hates the ones he
cannot. Believes an open hole is a promise the ground will collect.

**Tilda Bramble** — turbine operator. Keeps the ORC units like aircraft and
carries a vibration meter like a stethoscope. Believes a quiet machine is a
maintained one.

**Ib Fairweather** — pressure. Reads gauges twice and vents slowly by
principle. Believes the pocket is patient, so he can be too.

**Rennick Wold** — apprentice. Eighteen, supervised, keeps the depth board and
the journals. Believes a chalk mark is a small ceremony.

---

## 30. APPENDIX Q — LOCATION DETAIL

- **The Well Pad** — mud, pipe, and a hole with a name.
- **The Survey Ridge** — stakes, a level, and old numbers.
- **The Mud Pits** — what the hole returns and what it hides.
- **The Steam Trench** — insulated pipe and one warm handrail.
- **The Warm Cistern** — water that arrives at body temperature.
- **The Heated Rows** — tomatoes in February and a hot pipe under soil.
- **The Vent Hill** — a cold day's steam plume and a valve that opens slowly.
- **The Seismic Post** — a spike, a card, and the ground's own log.
- **The Spare Pad** — a leveled site waiting for a reason.
- **The Well Stone** — a depth and a date, cut for whoever comes next. 

---

## 32. APPENDIX R — WORKED DRILLING CAMPAIGN

**Month one.** Gilroy re-reads the old survey and writes an honest confidence
band beside every number. Kova levels the pad and marks the site. Rennick opens
the depth board with a zero and a date, and the crew's first shift drills
through soil and fill at a rate that makes everyone optimistic, which is the
caprock's oldest trick.

**Month two.** The caprock goes fast. The shale goes slower and brings water
that Hurn warns is not the water they came for. The bit journal gets its first
change and Marl dismisses a suggestion to push thirty more metres, and the
depth board reads one hundred and fifty.

**Month three.** The granite enters and the progress rate halves in a week.
The crew learns the difference between drilling and grinding, and the rig's
cable and brake get a hard look because everything now takes longer than the
plan. The shelter's winter coal budget is recalculated twice.

**Month four.** A bit seizes at three hundred and forty metres. The crew
spends eleven days freeing it with the rescue bit, and the well record's
longest paragraph is about a failure. Kova reads it aloud at the end of the
shift, which becomes the crew's habit for every bad week.

**Month five.** The steam pocket answers the gauge: one hundred and eighty
degrees at the rock face, and a noise nobody puts in the official report.
Ib vents slowly for six days, and the shelter watches a steam plume from the
vent hill and starts to believe the campaign.

**Month six.** Hurn runs full tier-three casing to five hundred metres and
cements it in two stages, and the seal test passes on the second try after a
first try that teaches the cement crew a lesson about the mix. The hole is
safe, and safety costs ten days and everything the inventory had.

**Month seven.** Tap day. The aquifer opens, the pressure settles into its
band, and Tilda starts the ORC units with a hand on the emergency stop and
her eyes on the vibration meter. The grid feels the well for the first time:
fourteen hundred kilowatts at the second hour, drifting to eleven hundred by
evening as the loops warm.

**Month eight.** The warm room opens, the bath house schedules washing for
four teams a day, and the kitchen stops burning coal to heat water. The first
bath in February is a small event that the year note records without
embarrassment.

**Month nine.** The first scale batch is ordered, mixed by the reagent crew,
and pumped on schedule. The loops come back to full output and Tilda writes
one line in the loop journal: cleaned when due, not when weak.

**Month ten.** Winter arrives and the greenhouse rows are the question. The
shelter splits the heat: rooms at night, rows by day, workshops cut to working
days only. The split is written down and posted, and the corridors are cold
for exactly two hours a morning, and everybody knows why.

**Month eleven.** A small ground tremor trips the seismic post. Every gauge
is read before anything else happens, the well checks clean, and the drill
crew's discipline buys the shelter a calm afternoon instead of a scare.

**Month twelve.** Rennick cuts the depth and date into the well stone, and the
shelter has a utility instead of a project. The depth board's last chalk mark
stays where it is, and the crew starts asking about the spare pad.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Marl measures the bit and the chart says one hundred, and the reading says
> one hundred and three, and he stops the run and takes the shouting, and the
> hole is still there in the morning and so is he.

> Ib vents the pocket for six days and writes the same number each hour, and
> the sixth day the number finally moves, and he permits himself a small
> ceremony of one biscuit and one paragraph in the log.

> Anwen opens the bath house at six and the first team comes in off the night
> shift with hands that cannot straighten, and they wash in water that came
> from a kilometre of rock, and nobody says a word about luck.

> Rennick cuts the stone slowly because the letters have to last longer than
> the chisel, and when he finishes he reads it back to Kova, who nods and goes
> to check a gauge because that is what a ceremony looks like on a well pad.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Bad site | dry or tight hole | retire, move to spare pad |
| Worn bit | break and lost days | change rule enforced |
| Break stuck | days of fishing | rescue bit, patience |
| Uncased hole | collapse risk | case to depth |
| Cement fail | seal retry | retest, mix lesson |
| Overpressure | danger | vent slowly |
| Scaling | output falls | descale on schedule |
| Corrosion | leaks and wear | inhibitors, replace |
| Reserve draw | recovery slows | hold under safe rate |
| Winter overload | cold rooms | priority plan |

Every failure in the table has a written recovery, and none of them ends the
campaign. The plan's promise is that drilling is a long argument with the
ground in which the shelter is allowed to lose days, bits, and dignity, but
never the whole project in one afternoon.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No second power, warmth, water, or chemistry system.
- [ ] Generation flows through `PowerGridSystem` only.
- [ ] Heat flows through `ShelterThermalSystem` only.
- [ ] Thermal water goes to treatment; no second water authority.
- [ ] Descaler is ordered from the reagent owner; no mixing here.
- [ ] No blowout spectacle; failures are costly and contained.
- [ ] Depths stay within authored strata bands.
- [ ] No graves, sacred sites, or occupied ground are drilled.
- [ ] Save additions are additive inside the geothermal state.
- [ ] Determinism uses the live day path only.

---

## 36. APPENDIX V — GLOSSARY

- **Pad** — the leveled site where the rig stands.
- **Stratum** — a named rock layer with hardness, wear, and promise.
- **Bit** — the cutting head, with a wear rating per metre.
- **Run** — the metres drilled on one bit before a change.
- **Casing** — pipe that keeps an open hole from becoming a problem.
- **Cement** — what holds casing and seals the annulus.
- **Tap** — connecting the aquifer to the shelter's loop.
- **Vent** — controlled release of pressure.
- **Loop** — brine or water circuit that carries heat to a use.
- **Reserve** — the ground's heat, which recovers slowly and can be overspent.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `GeothermalAquiferSystem` | strata | depth, tap | power grid |
| `GeothermalOrcSystem` | loops | loop state | grid |
| `DrillingCampaignSystem` | surveys | campaign | depth |
| `RigSystem` | inventory | rig state | depth |
| `BitSystem` | wear | bit journal | depth |
| `CasingSystem` | tiers | casing card | depth |
| `SteamHeadSystem` | gauges | pressure state | power |
| `LoopMaintenanceSystem` | state | cleaning | loops |
| `ThermalUseSystem` | budgets | use state | thermal owner |
| `WellRecordSystem` | journals | records | nothing |
| `PowerGridSystem` | generation | nothing | nothing |
| `ShelterThermalSystem` | heat | nothing | nothing |
| `SanitationSystem` | water | nothing | nothing |
| `ReagentSystem` | orders | nothing | nothing |
| `ShelterWorkshopSystem` | repairs | nothing | nothing |
| `PrecisionMetrologySystem` | gauges | nothing | nothing |
| `DutyRoster` | shifts | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`well_strata_catalog.json`** (extend existing strata schema) — `strata_id`,
`display_name`, `start_depth_m`, `end_depth_m`, `rock_hardness`,
`drill_wear_per_meter`, `seismic_hazard`, `steam_temperature_c`,
`thermal_output_kw`, `water_yield_liters_per_day`, `min_casing_tier`,
`tags[]`.

**`well_surveys.json`** — `survey_id`, `site`, `target_depth_m`, `strata_expected[]`,
`confidence`, `finding`, `tags[]`.

**`well_rig_parts.json`** — `part_id`, `kind`, `condition_bands[]`, `wear`,
`repair_route`, `tags[]`.

**`well_bits.json`** — `bit_id`, `size_mm`, `steel`, `hardness_band`,
`wear_limit`, `change_rule`, `tags[]`.

**`well_casing.json`** — `tier_id`, `depth_band`, `joint`, `cement_mix`,
`test`, `tags[]`.

**`well_steam.json`** — `rule_id`, `band_psi`, `action`, `owner_role`,
`tags[]`.

**`well_loops.json`** — `loop_id`, `stratum_id`, `working_fluid`,
`clean_interval_days`, `vibration_limit`, `tags[]`.

**`thermal_uses.json`** — `use_id`, `room_id`, `demand_kw`, `priority_band`,
`winter_rule`, `tags[]`.

**`well_records.json`** — `record_id`, `day`, `depth_m`, `event`, `actor_id`,
`lesson`, `tags[]`.

**`well_crews.json`** — `role_id`, `shift`, `skill`, `fatigue_band`, `pair_rule`,
`training_path`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid stratum or item references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Meters drilled | progress | Depth |
| Meters per shift | pace | Shifts |
| Bit changes | steel discipline | Bits |
| Breaks and days lost | risk | Records |
| Casing coverage | safety | Casing |
| Vent hours | pressure care | Steam |
| Turbine output | generation | Grid |
| Loop output vs. rated | maintenance | Loops |
| Reservoir draw vs. safe | reserve | Loops |
| Warm rooms in winter | payoff | Uses |

Telemetry is diagnostic only; it never gates content and never ranks a driller
or a shift.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside the geothermal state.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows wear, a break, a tap, and a warm winter.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No spectacle, free-energy, or real-site content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can the shelter drill a second well, and does the first one keep running?
2. Who owns the decision to abandon a site, and how is it recorded?
3. Does the rig ever leave the pad, and can it be sold or traded?
4. How does the shelter store the brine it does not use?
5. Can the well be run cold in summer to bank heat for winter?
6. What happens to a loop whose stratum runs dry?
7. Does the seismic post ever delay drilling, and who decides?
8. How much heat does the shelter waste before it notices?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 15 The Deep Root | Heated greenhouse rows and soil warmth |
| 1 | 16 The Rebuilt Body | Warm rooms and joint pain |
| 2 | 21 The Grid | Second generation heart |
| 2 | 18 The Underneath | Ground works and borehole safety |
| 3 | 22 The Clean Flow | Thermal water to treatment |
| 3 | 23 The Alarm | Pressure and seismic alerts |
| 4 | 27 The Thread | Drying rooms and warm clothing |
| 4 | 31 The Kiln | Cement, clay, and firebrick |
| 5 | 33 The Weather | Winter pacing and storms |
| 5 | 34 The Long Road | Pipe and casing haulage |
| 6 | 39 The Reagent | Descaler and inhibitor orders |
| 6 | 40 The Wheel | Pumps, bearings, and drives |
| 6 | 41 The Quiet | Night drilling and quiet hours |
| 7 | 42 The Core | Parallel generation, no reactor content |
| 7 | 44 The Outpost | Well kits for remote sites |
| 7 | 46 The Long Change | Seismic and ground change |
| 8 | 47 The Brigade | Steam plant fire boundaries |
| 8 | 53 The Post | Well reports and letters |

Each hook is additive. The Warm Ground can ship alone, and every other
expansion can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Warm House.** The well heats rooms, baths, and rows, and winter becomes
a season the shelter plans for instead of an emergency it survives.

**The Deep Utility.** The ORC units carry their share of the grid, and the
shelter's power has a second heart under the ridge.

**The Cased Hole.** The shelter spends its ambition on depth it can protect,
stops at four hundred metres, and gets a smaller well that has never once
worried anyone.

**The Measured Ground.** The campaign is recorded metre by metre, and the next
crew drills the same ridge in half the time with a tenth of the surprises.

**The Second Site.** The ridge is abandoned honestly after a cold fault, and
the spare pad becomes the well, and nobody calls the move a failure.

**Fade.** A warm room in February, wet clothes drying on a line, steam
between two buildings, and a depth board with the last metre written in chalk.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Free energy | economy break | reserve and draw |
| Power duplication | authority break | grid sink |
| Blowout spectacle | tone break | contained failure |
| Set-and-forget | false | scaling and care |
| Instant well | unreal | seasons of drilling |
| Mystical depths | tone break | numbers and rock |
| Spa luxury | tone break | domestic warmth |
| Volcano doom | genre break | measured risk |
| Chemistry at the pad | authority break | reagent orders |
| Water authority | duplication | treatment owner |

The list exists because a deep well is easy to write as either a magic battery
or a disaster. The expansion's rule is that the well is a long engineering
project with a maintenance calendar, and its payoff is a bath in February.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Strata | 14 | 2,500 |
| Surveys | 8 | 2,000 |
| Rig parts | 16 | 3,000 |
| Bits | 12 | 2,500 |
| Casing | 6 | 1,500 |
| Steam rules | 10 | 2,000 |
| Loops | 8 | 2,000 |
| Thermal uses | 12 | 2,500 |
| Records | 24 | 4,000 |
| Crews | 8 | 2,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~55,000** |

---

## 46. APPENDIX AF — FIRST FOUR YEARS OF THE WELL

| Year | Focus | Milestone |
|---|---|---|
| 1 | Survey and caprock | site chosen |
| 2 | Granite and break | depth 420 |
| 3 | Pocket, casing, tap | well born |
| 4 | Warmth and scale | winter warm |

Four years is the honest arc for a deep well: a year of looking and learning
slow rock, a year that contains the campaign's worst month, a year of the tap
and the first bath, and a year when the well is simply part of the house.

---

## 47. APPENDIX AG — SEISMIC LOG TABLE

| # | Day | Reading | Action | Result |
|---|---|---|---|---|
| 1 | 40 | normal | none | logged |
| 2 | 120 | minor | check | clean |
| 3 | 200 | minor | check | clean |
| 4 | 260 | moderate | full stop | inspected |
| 5 | 260 | moderate | gauges read | clean |
| 6 | 261 | settling | resume | cautious |
| 7 | 330 | minor | check | clean |
| 8 | 500 | minor | check | clean |

Eight seismic log rows, and the full stop at day two hundred and sixty is the
entry that proves the post was worth its cost: the crew stops for a day, every
gauge is read, and nothing is found, which is exactly what a well-run response
looks like. The card is kept beside the depth board where every visitor can
see that caution is part of the record.

---

## 48. APPENDIX AH — WINTER HEAT PLAN TABLE

| # | Band | Time | Rooms | Rows | Shops |
|---|---|---|---|---|---|
| 1 | Mild | all day | full | full | full |
| 2 | Cold | day | full | full | full |
| 3 | Cold | night | full | night idle | off |
| 4 | Hard | day | full | half | working |
| 5 | Hard | night | full | off | off |
| 6 | Severe | day | full | off | off |
| 7 | Severe | night | full | off | off |
| 8 | Emergency | any | first | off | off |

Eight bands and a plan that puts rooms first in every hard night, and the
greenhouse's half-day row is the shelter's compromise with its own food: a
shelter that lets the rows freeze to keep the workshops warm has chosen
furniture over dinner, and the plan refuses to make that choice quietly.

---

## 49. APPENDIX AI — WELL COVENANT

| Clause | Promise |
|---|---|
| Survey | The ground is read before it is drilled |
| Pace | The bit is changed when the chart says so |
| Case | No uncased metre is forgotten |
| Vent | Pressure is released slowly and on schedule |
| Tap | The aquifer opens on a prepared day, never a rushed one |
| Clean | Loops are cleaned when due, not when weak |
| Draw | The reserve is spent under its safe rate |
| Heat | Rooms and clinic first, always |
| Record | Every metre, break, and lesson is written down |
| Hand | The stone states the depth for whoever comes next |

The covenant is the expansion's first-class design object, taped inside the
drill shack where the crew reads it during the bad weeks. It is also the
shelter's answer to the ground: a well is a promise made to something that
cannot hear, and the promise is kept by the records and the maintenance.

---

## 50. APPENDIX AJ — CREW SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Driller | Kova | assistant | one full shift |
| Geologist | Gilroy | apprentice | one survey |
| Bit shop | Marl | bit hand | one run |
| Thermal | Anwen | loop lead | one winter week |
| Casing | Hurn | cement hand | one casing run |
| Turbine | Tilda | night watch | one service |
| Pressure | Ib | driller | one vent |
| Apprentice | Rennick | next recruit | one journal month |

Eight roles with named successors, and the handover column is measured in the
unit of the job: a shift, a survey, a run, a winter week, a cleaning, a vent.
The turbine handover takes a service because the person taking over must have
seen a bearing replaced before they hear one fail.

---

## 51. APPENDIX AK — MUD AND RETURNS TABLE

| # | Return | Meaning | Action | Note |
|---|---|---|---|---|
| 1 | Grit | normal cutting | dump | pace good |
| 2 | Clay lumps | soft band | slow | watch |
| 3 | Warm mud | heat rising | prepare | good sign |
| 4 | Gas bubbles | pocket near | vent ready | caution |
| 5 | Sand slug | loose ground | case soon | risk |
| 6 | Water flow | aquifer near | switch mud | plan |
| 7 | Metal shavings | bit damage | pull now | stop |
| 8 | Nothing | lost returns | stop | serious |

Eight return readings and their meanings, painted on a board in the mud pit
shed so that even the newest hand can read the hole from what comes out of it.
The rows are a small education in how drilling actually talks, and the last one
is the only return that stops a shift instantly: a hole that stops giving back
is a hole telling the truth about something.

---

## 52. APPENDIX AL — TAP DAY PROCEDURE TABLE

| # | Step | Role | Check | Wait |
|---|---|---|---|---|
| 1 | Gauges read | pressure | band | do first |
| 2 | Crew paired | driller | pairs | do first |
| 3 | Vent open | pressure | flow | hourly |
| 4 | Casing verified | casing | passed | last week |
| 5 | Loop ready | turbine | charged | one day |
| 6 | Grid warned | grid | load shed | half day |
| 7 | Open valve | driller | slow turn | minutes |
| 8 | Read output | turbine | stable | one hour |
| 9 | Sign the card | all | names | end of day |
| 10 | First heat | thermal | rooms | that evening |

Ten tap-day steps with checks, and the sixth row is the one that keeps the
tap from becoming an emergency elsewhere: the grid is warned and sheds load
before the valve opens so that the well's first gift does not trip the lights
in the clinic. The last row is the point of the entire campaign.

---

## 53. CLOSING STATEMENT

ASHFALL already contains a drilling campaign in miniature: depth in meters, a
wearing bit, strata from caprock to magmatic basement, casing tiers, steam
pressure, mineral scaling, a turbine that reports its output, an ORC family
with brine loops and thermal reserve, and a save file that remembers all of it.
What it lacks is the campaign itself — crews, rigs, bits, cement, venting,
descaling, thermal uses, and a story about a shelter that decides to spend a
year finding out what it sits on. The Warm Ground adds that practice without
touching the power, warmth, water, or chemistry authorities. It adds a depth
board with a chalk mark for every metre, a warm room in February, and a stone
that records how far the shelter went for heat.

> Wave 9 note: this plan is one of five Wave 9 expansion bibles (52–56). Each is
> self-contained; none requires another to ship. The shared Wave 9 index lives
> at `docs/expansions/wave9/WAVE9_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `GeothermalAquiferSystem` (`GeothermalAquiferState` with
> `currentDepthMeters`, `drillBitCondition`, `steamPressurePsi`,
> `mineralScaling`, `activeTurbineOutput`, `currentStrataId`,
> `installedCasingDepth`, `turbineCommissioned`, `aquiferTapped`,
> `pressureReliefState`, `generatorHealth`, `crossedStrataIds`; constants
> `NominalTurbineOutputKw = 1500f`, `DescalingChemicalCost = 2f`; methods
> `StartDrilling`, `CommissionTurbine`, `Descale`, `TapAquifer`,
> `VentPressure`, `InstallCasing`, `TickDay`; `GeothermalStrataDef`),
> `GeothermalOrcSystem` (`GeothermalStratumDefinition`, `GeothermalStrataCatalog`,
> `GeothermalLoopState`), `src/Host/GeothermalAquiferHostSession.cs`,
> `GeothermalAquiferSaveStore` under `geothermal_aquifer`, and the thin data
> (`geothermal_drilling_depths.json` 2,112 B / 5 strata;
> `geothermal_strata_catalog.json` 1,547 B / 3 strata, including a 180 C steam
> pocket at 300–500 m and a 350 C magmatic basement at 750–1000 m).