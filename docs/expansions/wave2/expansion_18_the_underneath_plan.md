# ASHFALL — Expansion 18 Design Bible
# THE UNDERNEATH
### Wave 2 · Subterranean Networks, Mining, Deep Geology, Cave Ecology, and Subsidence

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Subterranean`, `Ashfall.Core.Underground`, `Ashfall.Core.Excavation`, `Ashfall.Core.Shelter` (SeismicDynamics), `Ashfall.Core.Narrative` (Geology)
**Proposed host owner:** `DeepEarthHostSession` (extends `SubterraneanHostSession` + `ExcavationHostSession`)
**Existing save sections:** `subterranean`, `excavation`, `excavation_hazards`, seismic state
**Existing CLI verbs:** `--subterranean-selftest`, `--excavation-selftest`, `--seismic-dynamics-selftest`, `--mine-clearing-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already owns the ground beneath the shelter with unusual completeness.
`SubterraneanSystem` (Plan 156) tracks per-node discovery, depth tier, structural
integrity, oxygen, flooding, shoring, and ventilation, with hazards rolled
deterministically from `(day, nodeId)`. `TunnelNetworkSystem` models segments,
junctions, and hazards. `ExcavationSystem` digs rooms and reinforces them with the
foundry's cast T-beam. `ExcavationHazardSystem`, `SeismicDynamicsSystem`,
`GeologicalStrataCatalog`, `HydroGeologyCatalog`, and `SaltMineExtractionSystem`
cover hazards, quakes, strata, water, and industrial salt. But the authored content
is thin: **one small zone catalog**, a short excavation site list, and small
geology tables. There is almost no deep world to find.

**The Underneath** fills the ground. It adds a full subterranean frontier: mining
and ore, deep strata and heritage layers, cave ecology, gas and flood hazards,
subsidence that reaches the surface, and the question of what the shelter was built
on top of.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has a floor. Below the floor is a foundation. Below the foundation is
everything the old world buried: service tunnels, a flooded metro, a salt seam, a
gas pocket, an aquifer, a crystalline cavern, and — at the bottom — something the
shelter's founders chose not to record.

**The Underneath** opens that vertical wilderness. It is the expansion about depth
as a resource, a hazard, and a memory. Going down produces ore, water, geothermal
heat, and knowledge. It also produces cave-ins, poison air, flooding, surface
subsidence, and the slow realization that the shelter's own stability depends on
what it leaves intact beneath itself.

The expansion is built on a hard rule from the live systems: **the underground is
deterministic, finite, and permanent.** A collapsed tunnel stays collapsed. A
flooded level stays flooded. A shored node stays shored. Every descent is an
investment in a specific map that will never regenerate.

### 1.2 The five loops it adds

```
    Survey ──► Dig/Bore ──► Shore & Ventilate ──► Extract ──► Surface use
      │            │               │                  │            │
      ▼            ▼               ▼                  ▼            ▼
   strata &     hazard         oxygen/flood       ore, water,   industry,
   cave map     events         management         heat, relics  power, trade
      │                                                        │
      ▼                                                        ▼
   Deep heritage ◄── archaeology layers ◄── subsidence risk ◄── surface damage
```

### 1.3 What the player manages

1. **Depth tiers.** `SubterraneanNodeState.depthTier` already exists. The expansion
   gives each tier its own geology, hazards, and rewards, and makes depth a real
   capability gate (shoring, ventilation, transit).
2. **Structural integrity.** `structuralIntegrity` decays and is restored by shoring
   (`shoringLevel` 0–3). The expansion adds subsidence: deep extraction can damage
   the surface.
3. **Air.** `oxygenLevel`, `ventilationInstalled` exist. The expansion adds gas
   pockets, airflow networks, and poisoned-air events.
4. **Water.** `waterLevel` and flooding exist. The expansion adds aquifers, drainage,
   and the choice between dewatering a level and leaving it sealed.
5. **Ore and extraction.** New: ore veins, grades, mining methods, and processing
   into existing foundry/metallurgy inputs.
6. **Cave ecology.** New: underground flora and fauna, bioluminescence, and a living
   ecosystem that extraction can destroy.
7. **Seismic risk.** `SeismicDynamicsSystem` forecasts quakes; the expansion ties
   extraction and injection to quake triggers and subsidence.
8. **Deep heritage.** New: strata as archaeology — permafrost layers, buried
   pre-war rooms, and the truth of the shelter's site.

### 1.4 What it is not

- Not a second map system. Subterranean nodes attach to the existing network topology
  and the existing cartography panel.
- Not a second hazard system. Gas, flood, collapse, and radiation route through
  `ExcavationHazardSystem` / `SubterraneanSystem` / `TunnelNetworkSystem`.
- Not a second inventory. Ore is an item.
- Not a second power source by default; geothermal taps existing systems.
- Not a second save authority. All state is additive in the existing envelopes.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Subterranean/SubterraneanSystem.cs` | Node state: discovery, integrity, oxygen, flood, shoring, ventilation, hazards | `LIVE` |
| `Assets/Ashfall.Core/Subterranean/SubterraneanZoneCatalog.cs` | Zone definitions | `LIVE` |
| `Assets/Ashfall.Core/Subterranean/SubterraneanSave.cs` | Network save codec | `LIVE` |
| `Assets/Ashfall.Core/Underground/TunnelNetworkSystem.cs` | Segments, junctions, hazard types, status | `LIVE` |
| `Assets/Ashfall.Core/ExcavationSystem.cs` | Sites, progress, workers, cave-in risk, T-beam reinforcement | `LIVE` |
| `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | Excavation hazards | `LIVE` |
| `Assets/Ashfall.Core/Excavation/ExcavationCatalogLoader.cs` | Hazard catalog | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs` (+ `.Monitoring`) | Quake dynamics and forecasting | `LIVE` |
| `Assets/Ashfall.Core/Narrative/GeologicalStrataCatalog.cs` | Strata data | `LIVE` |
| `Assets/Ashfall.Core/Narrative/HydroGeologyCatalog.cs` (+ Discovery, Projection) | Hydogeology and archives | `LIVE` |
| `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | Salt extraction | `LIVE` |
| `Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs` | Mine clearing | `LIVE` |
| `src/Host/SubterraneanHostSession.cs`, `src/Host/ExcavationHostSession.cs` | Host | `LIVE` |
| `src/Host/SeismicDynamicsSaveStore.cs`, `src/Host/ExcavationHazardSaveStore.cs` | Persistence | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries | Notes |
|---|---|---|
| `subterranean_zones.json` | small | node definitions |
| `excavation_sites.json` | ~10 KB | room-dig sites |
| `seismic_fault_catalog.json` | 2 KB | fault definitions |
| `geodetic_survey_catalog.json` | ~10 KB | survey stations |
| `geothermal_strata_catalog.json` | 1.5 KB | heat strata |
| `geothermal_drilling_depths.json` | 2.1 KB | drilling bands |
| `insar_geodesy_catalog.json` | 2.4 KB | deformation monitoring |
| `piezometer_network_catalog.json` | 2.6 KB | water pressure |
| `underground_flora.json` | 2.7 KB | cave flora |
| `excavation_hazard_mitigation.json` | hazard responses | |
| `mine_flail_catalog.json` | 2 KB | clearing equipment |

### 2.3 Confirmed gaps

- **GAP-18-1 — One small zone catalog.** The subterranean frontier has barely any
  authored nodes for a system that supports depth tiers, ventilation, and hazards.
- **GAP-18-2 — No ore or mining loop.** Salt is extracted; nothing else is mined,
  graded, or processed into the foundry chain.
- **GAP-18-3 — No subsidence.** Deep extraction and dewatering have no surface
  consequence, though `SeismicDynamicsSystem` and InSAR catalogs exist.
- **GAP-18-4 — No gas pockets.** `TunnelHazardType.ToxicGas` exists but no authored
  gas fields, detection, or ventilation strategy.
- **GAP-18-5 — No aquifer model.** Flooding exists as node state; the expansion has
  no aquifer, recharge, or drainage system.
- **GAP-18-6 — Cave ecology is thin.** `underground_flora.json` is 2.7 KB; no
  fauna, no ecosystem, no harvest ethics.
- **GAP-18-7 — No deep heritage.** Strata logs exist as narrative; no buried rooms,
  artifacts, or permafrost finds.
- **GAP-18-8 — No deep locations.** No mine, cavern, underground lake, or borehole
  field in `locations.json`.
- **GAP-18-9 — No deep rooms.** No habitable deep shelter, mine works, or pump room
  in `shelter_rooms.json`.

### 2.4 Non-duplication statement

This expansion will **not** add a second network, hazard, excavation, seismic,
water, inventory, power, or save system. It extends the five live underground owners
with data and additive subsystems. It also does not duplicate `HydrologySystem`/
water treatment: aquifer water feeds the existing water authority.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The underground is permanent.** Collapse, flood, and extraction results
persist. There is no regeneration, no reset, no undo.

**Pillar 2 — Depth costs capability.** You cannot simply walk deeper. Shoring,
ventilation, drainage, and transit are prerequisites, and each consumes surface
resources.

**Pillar 3 — The ground remembers.** Strata are layers of history. Digging is
archaeology as much as extraction, and some layers should not be disturbed.

**Pillar 4 — Extraction has a surface.** Every tonne pulled from below changes the
stability above. Subsidence is the bill for ore.

**Pillar 5 — It is alive down there.** Cave ecology is not decoration; it is an
ecosystem that extraction can wound or destroy, with real consequences for food,
medicine, and morale.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A descent | Headlamp, rope, breath, silence | Monster-movie dread |
| A cavern | Scale, water, mineral color, awe | Glowing fantasy crystals |
| A cave-in | Dust, weight, a missing voice | Action spectacle |
| Gas | A smell, a headache, a canary | Invisible instant death |
| Deep heritage | A boot, a button, a document | Treasure room |
| Extraction | Ore carts, dust, pumps, debt | Mining empire fantasy |

### 3.3 Content limits

- No real-world mines, geological formations, or designated aquifers.
- No fantasy monsters, magic crystals, or intelligent underground species.
- No glorified industrial exploitation; extraction is costly and consequential.
- Cave fauna is fictional and biologically plausible.

---

## 4. THE UNDERNEATH WORLD

### 4.1 Depth tiers

| Tier | Name | Geology | Primary hazard | Primary reward |
|---|---|---|---|---|
| 1 | Foundations | fill, rubble, old service tunnels | collapse | salvage, pipes |
| 2 | The Works | clay, sand, shallow rock | flooding | water, sand, clay |
| 3 | The Seam | coal, salt, iron | gas | ore, fuel |
| 4 | The Vault | limestone, karst | sinkholes | cave flora, water |
| 5 | The Deep | granite, quartz | heat, pressure | rare ore, heat |
| 6 | The Floor | unknown | unknown | the site's truth |

### 4.2 Interior rooms (deep)

- **`room_mine_head`** — shaft head, hoist, and ore sorting.
- **`room_ventilation_plant`** — fans, ducting, and gas monitoring.
- **`room_dewatering_station`** — pumps, sumps, and discharge.
- **`room_deep_barracks`** — a rest level for deep crews.
- **`room_strata_lab`** — sample analysis and survey interpretation.
- **`room_deep_archive`** — the secure store for heritage finds.
- **`room_cave_garden`** — a cultivated patch of cave flora.
- **`room_geothermal_tap`** — a heat exchanger tied to the live power system.

### 4.3 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_mine_adit` | The Old Adit | 5 | Historic mine entrance; timber, rails |
| `loc_borehole_field` | The Borehole Field | 6 | Deep drilling; strata samples |
| `loc_sinkhole_row` | The Sinkhole Row | 7 | Subsidence zone; collapsed streets |
| `loc_underground_lake` | The Black Lake | 8 | Aquifer access; drowning risk |
| `loc_crystal_cavern` | The Quiet Cavern | 7 | Mineral formations; ecology |
| `loc_gas_vein` | The Breathe | 8 | Gas field; fuel and danger |
| `loc_metro_descent` | The Metro Descent | 6 | Flooded transit; salvage |
| `loc_deep_bunker` | The Floor Bunker | 9 | The buried truth of the site |
| `loc_dewatering_outfall` | The Outfall | 5 | Where pumped water goes |
| `loc_karst_spring` | The Spring Mouth | 4 | Clean water and cave life |

All locations require valid item references and scanner registration.

### 4.4 The vertical map

The subterranean map is not a new map screen. It is a depth-layered graph on the
existing `SubterraneanCartographyPanel`, using the live node topology. Each node has
a `depthTier`, connections, and state. The expansion authors the graph; the systems
already know how to walk it.

---

## 5. MAIN STORYLINE — "WHAT THE FOUNDATION SITS ON"

### 5.1 Central conflict

The shelter's water table is dropping and its foundry is short on iron. Both
problems point down. A survey crew, led by a geologist named **Dr. Ivetka Sarne**,
confirms there is a seam of iron and a deep aquifer beneath the shelter — and that
the seam is directly under the main vault's load-bearing wall.

The shelter's engineer, **Bram Koole**, refuses to authorize extraction: pulling the
seam risks subsidence that could drop the vault. The quartermaster, **Nessa Orr**,
points out that without iron the foundry stops, and without water the shelter dies
anyway. The compromise is a smaller, slower, shored extraction — which costs more
and may still fail.

Then the crew finds the boot. Below the seam, in a permafrost lens, is a sealed
pre-war room with a document trail that explains why the shelter was built exactly
here — and what was buried first.

The expansion's question: **how deep are you willing to dig into your own
foundation, and what do you do with what you find?**

### 5.2 Theme (unspoken)

**Everything above rests on something below. You can ignore the foundation, but it
will not ignore you.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_geologist_ivetka_sarne` | Dr. Ivetka Sarne | Geologist | Wants to map everything; ethical about disturbance |
| `npc_engineer_bram_koole` | Bram Koole | Engineer | Guards the vault; opposes deep extraction |
| `npc_quartermaster_nessa_orr` | Nessa Orr | Quartermaster | Owns the shortage; pushes for ore |
| `npc_miner_haldo_crest` | Haldo Crest | Mine captain | Knows the ground by feel; superstitious |
| `npc_pump_chief_liss_arn` | Liss Arn | Dewatering chief | Flood water is her responsibility |
| `npc_cave_diver_noor` | Noor | Cave diver | The Black Lake's only guide |
| `npc_archivist_deep_tob` | Tob | Deep archivist | Records what is found and what is sealed |
| `npc_child_deep_pip` | Pip | Born underground | Represents the deep-born generation |

### 5.4 Story beats (15)

1. **The Dry Tap.** Water pressure drops; the foundry runs short on iron.
2. **The Survey.** Sarne confirms a seam and an aquifer under the vault.
3. **The Refusal.** Koole blocks extraction; the argument is costed.
4. **The First Cut.** A shored, slow extraction begins.
5. **The Breathing Rock.** A gas pocket is struck; ventilation is improvised.
6. **The Flood.** The Works level takes water; the pumps run day and night.
7. **The Cave.** A cavern ecosystem is found; extraction would destroy it.
8. **The Boot.** The permafrost lens yields a sealed room and a document.
9. **The Sinkhole.** A surface subsidence event damages a shelter room.
10. **The Second Seam.** A richer ore body requires getting past the cave.
11. **The Lake.** The aquifer is reached; the water is clean and the descent is fatal-risky.
12. **The Deep Bunker.** The Floor is opened; the site's history is read.
13. **The Commission.** The shelter debates sealing, publishing, or profiting.
14. **The Settlement.** Subsidence, ore, and truth reach a reckoning.
15. **What We Leave Below.** Final disposition of the deep.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Extraction | open / shored-slow / none | risk vs. need |
| Cave | preserve / mine through / relocate | ecology vs. output |
| Flood response | pump / seal / redirect | cost vs. control |
| Gas | vent / flare / cap and leave | safety vs. fuel |
| Aquifer | tap / test only / seal | water vs. ground |
| Deep heritage | publish / seal / sell | truth vs. stability |
| Subsidence response | shore surface / abandon room / evacuate | cost vs. safety |
| Final disposition | mine / preserve / close | legacy |

### 5.6 Endings (5 + fade)

1. **The Shored Deep** — extraction succeeds slowly; the shelter gains ore and keeps its vault.
2. **The Caved Vault** — subsidence wins; a room is lost and the shelter rebuilds shallower.
3. **The Living Cavern** — the cave is preserved; the shelter gives up ore for an ecosystem.
4. **The Published Floor** — the deep heritage is released; standing and knowledge shift.
5. **The Sealed Ground** — the deep is poured shut; the shelter chooses ignorance and stability.
6. **Fade** — the shaft is left to silence.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_deep_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_deep_dry_tap`, `quest_deep_survey`, `quest_deep_refusal`, `quest_deep_first_cut`,
`quest_deep_breathing_rock`, `quest_deep_the_flood`, `quest_deep_the_cave`,
`quest_deep_the_boot`, `quest_deep_sinkhole`, `quest_deep_second_seam`,
`quest_deep_the_lake`, `quest_deep_floor_bunker`, `quest_deep_the_commission`,
`quest_deep_settlement`, `quest_deep_what_we_leave_below`.

### 6.2 Side quests (28)

**Survey and science (5)**
- `quest_deep_strata_sample` — drill and classify a layer
- `quest_deep_fault_map` — trace a fault line
- `quest_deep_piezometer` — install pressure monitoring
- `quest_deep_insar` — read surface deformation
- `quest_deep_archive_compare` — match strata to old records

**Mining (5)**
- `quest_deep_ore_grade` — assay a seam
- `quest_deep_timber` — shore a tunnel
- `quest_deep_hoist` — repair the shaft hoist
- `quest_deep_ore_sort` — separate waste from ore
- `quest_deep_dust_lung` — protect miners from dust

**Hazards (5)**
- `quest_deep_gas_watch` — monitor the Breathe
- `quest_deep_canary` — a living gas detector
- `quest_deep_pump_shift` — dewatering duty
- `quest_deep_roof_fall` — a partial collapse rescue
- `quest_deep_bad_air` — evacuate a poisoned level

**Water (4)**
- `quest_deep_aquifer_test` — sample and test the deep water
- `quest_deep_seal_flood` — seal a flooded level
- `quest_deep_outfall` — manage pumped discharge
- `quest_deep_dry_well` — a surface well fails

**Cave ecology (4)**
- `quest_deep_cave_flora` — catalog cave plants
- `quest_deep_cave_fauna` — observe cave animals
- `quest_deep_cave_garden` — cultivate cave flora
- `quest_deep_ecology_choice` — mine or preserve

**Heritage (3)**
- `quest_deep_permafrost_find` — recover a frozen artifact
- `quest_deep_document_chain` — trace the site's history
- `quest_deep_seal_room` — decide whether to open the last door

**Surface consequences (2)**
- `quest_deep_crack_watch` — monitor the shelter's floor
- `quest_deep_repair_vault` — repair subsidence damage

### 6.3 Repeatable quests (8)

`quest_deep_repeat_dig`, `quest_deep_repeat_shore`, `quest_deep_repeat_vent`,
`quest_deep_repeat_pump`, `quest_deep_repeat_assay`, `quest_deep_repeat_survey`,
`quest_deep_repeat_cave_watch`, `quest_deep_repeat_repair`.

### 6.4 Dynamic hooks

`SubterraneanSystem` emits hazard events per node-day; `ExcavationSystem` emits
dig/cave-in events; `SeismicDynamicsSystem` emits quake events; `HydroGeology`
emits discoveries. The generator attaches authored follow-ups without a new bus.

### 6.5 Constraints

- No node may regenerate after collapse or flood.
- No ore may enter inventory without extraction and processing.
- No dewatering may create water from nothing; it moves water through the existing
  water authority.
- No heritage find may grant combat power; it grants knowledge, standing, or morale.
- No deep creature may be treated as a monster to farm.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `DeepSurveySystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** survey campaigns, strata sampling, fault mapping, and anomaly detection.
**Consumes:** `GeologicalStrataCatalog`, `GeodeticSurveyCatalog`, `InsarCatalog`,
`SeismicDynamicsSystem`. **Data:** `deep_strata.json`, `survey_campaigns.json`.
**Rules:** survey is slow and bounded; it reveals node properties before digging, so
the player can choose risk.

### 7.2 `MiningSystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** ore veins, grades, extraction rates, waste, and processing inputs.
**Consumes:** `SubterraneanSystem` nodes, `Inventory`, `SilentFoundry` metallurgy,
`PowerGridSystem`. **Data:** `ore_veins.json`, `mining_equipment.json`.
**Rules:** extraction reduces ore reserves permanently; it raises gas/collapse risk
and contributes to subsidence; ore grades feed the existing metallurgy chain.

### 7.3 `SubsidenceSystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** surface deformation caused by extraction and dewatering, crack
propagation, and structural damage to shelter rooms.
**Consumes:** `MiningSystem`, `SeismicDynamicsSystem`, `ExcavationSystem`,
`InsarCatalog`. **Data:** `subsidence_zones.json`.
**Rules:** subsidence is cumulative and visible; it can damage rooms and is repaired
with real materials.

### 7.4 `AquiferSystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** deep aquifer volume, recharge, drawdown, and water quality.
**Consumes:** `HydroGeologyCatalog`, water treatment, `PiezometerNetwork`.
**Data:** `aquifers.json`.
**Rules:** tapping an aquifer is a long-term commitment; over-draw lowers the surface
water table and can trigger subsidence.

### 7.5 `GasFieldSystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** gas pockets, pressure, migration, detection, and venting/flaring.
**Consumes:** `SubterraneanSystem` hazards, ventilation, `PowerGridSystem` (flare
energy is not free power). **Data:** `gas_pockets.json`.
**Rules:** gas is fuel and hazard; a flare may be tapped as a small energy source
through the existing power contribution path, never as a new generator.

### 7.6 `CaveEcologySystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** cave flora and fauna populations, habitats, cultivation, and harm.
**Consumes:** `underground_flora.json`, `Farming`/`NutritionDiversity`, `Inventory`.
**Data:** `cave_species.json`, `cave_habitats.json`.
**Rules:** extraction and flooding damage habitats; some species are food, medicine,
or light sources; no species is a monster.

### 7.7 `DeepHeritageSystem` (new, `Ashfall.Core.Subterranean`)

**Owns:** permafrost lenses, sealed rooms, artifact chains, and the site's historical
record. **Consumes:** `GeologicalStrataCatalog`, narrative documents, `ArchiveDesk`.
**Data:** `deep_heritage.json`.
**Rules:** finds are knowledge and memory; they may change faction standing and the
epilogue; they never grant combat power.

### 7.8 Systems explicitly not added

- No second network, hazard, excavation, seismic, or water authority.
- No fantasy monsters or magic materials.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `subterranean_zones.json` (extend)

Existing schema preserved (`nodeId`, depth tier, connections, hazard profile). The
expansion authors a full depth graph across six tiers.

### 8.2 `deep_strata.json` (new)

Stratum rows: id, depth band, material, hardness, water content, gas affinity,
heritage affinity, and survey difficulty.

### 8.3 `ore_veins.json` (new)

```json
{
  "schema_version": 1,
  "veins": [
    {
      "vein_id": "vein_iron_shallow",
      "display_name": "Shallow Iron Seam",
      "ore_item_id": "item_iron_ore",
      "grade_permille": 420,
      "reserve_units": 240,
      "depth_tier": 3,
      "hardness": 0.5,
      "gas_affinity": 0.2,
      "subsidence_affinity": 0.4,
      "tags": ["iron", "shallow", "foundation_risk"]
    }
  ]
}
```

### 8.4 `mining_equipment.json` (new)

Tools, supports, hoists, carts, drills, ventilation, and pumps.

### 8.5 `subsidence_zones.json` (new)

Zone rows: id, linked nodes, surface room impact, crack rate, repair cost, warning
window.

### 8.6 `aquifers.json` (new)

Aquifer rows: id, volume, recharge, quality band, drawdown effect, subsidence
coupling, contamination risk.

### 8.7 `gas_pockets.json` (new)

Gas rows: id, pressure, volume, migration rate, hazard severity, flare potential.

### 8.8 `cave_species.json` (new)

Flora and fauna rows: habitat, light need, harvest yield, cultivation difficulty,
extraction sensitivity, medicinal use.

### 8.9 `cave_habitats.json` (new)

Habitat rows: linked nodes, species pool, health, damage sources, recovery rate.

### 8.10 `deep_heritage.json` (new)

Find rows: layer, artifact chain, document node, knowledge value, standing effect,
seal options.

### 8.11 Items

New items appended to `items.json`: `item_iron_ore`, `item_copper_ore`,
`item_coal_chunk`, `item_rare_ore`, `item_mine_timber`, `item_ore_cart`,
`item_vent_fan`, `item_pump_impeller`, `item_gas_mask_canister`, `item_cave_moss`,
`item_cave_cap_fungus`, `item_permafrost_token`, `item_strata_core_sample`,
`item_deep_lamp`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`src/Host/SubterraneanHostSession.cs` and `src/Host/ExcavationHostSession.cs` own the
existing envelopes; seismic state has its own store. New sub-objects are additive.
No new save section.

### 9.2 State to persist

- Node state (live; extend with ore, gas, habitat, heritage flags).
- Ore reserves (permanent depletion).
- Subsidence and surface damage.
- Aquifer drawdown and quality.
- Gas pressure and venting.
- Cave habitat health and cultivated patches.
- Heritage finds and seal decisions.
- Survey data.

### 9.3 Determinism

- Hazard rolls already use `(day, nodeId)` seeding; the expansion preserves that.
- Mining yield, subsidence rate, and gas migration are pure arithmetic on authored
  rates.
- Paired replay hashes must match; no wall clock, no `System.Random`.

### 9.4 Migration

Legacy saves load with no ore reserves, no subsidence, no gas field, no habitats,
and no heritage finds. Existing node state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for grade, pressure, and destabilization.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `SubterraneanOperationsPanel` (extend) | Nodes, integrity, oxygen, flood | `DeepEarthHostSession` |
| `SubterraneanCartographyPanel` (extend) | Depth graph and discovered tiers | same |
| `MiningPanel` (new) | Veins, reserves, grades, equipment | same |
| `SubsidencePanel` (new) | Surface deformation and damage | same |
| `AquiferPanel` (new) | Deep water volume, drawdown, quality | same |
| `GasPanel` (new) | Pockets, pressure, venting | same |
| `CaveEcologyPanel` (new) | Habitats, species, cultivation | same |
| `DeepHeritagePanel` (new) | Finds, chains, seal decisions | same |
| `BoreholeSeismographPanel` (extend) | Quake monitoring with extraction coupling | seismic host |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Depth and hazard information is text plus icon, never color-alone.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Permanent consequences (collapse, flood, depletion) are stated before committing.
- Subsidence warnings are explicit and never surprise the player without lead time.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: distant rockfall, pump rhythm, gas hiss,
flood water, cave drip, fan hum, hoist chain. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `SubterraneanSystem` | Nodes extended additively; hazards unchanged |
| `TunnelNetworkSystem` | Segments gain ore/gas/habitat metadata |
| `ExcavationSystem` | Dig sites feed deep rooms; T-beam still the reinforcement |
| `ExcavationHazardSystem` | Gas/flood/collapse routed here |
| `SeismicDynamicsSystem` | Extraction and injection alter quake pressure |
| `GeologicalStrataCatalog` / `HydroGeology` | Strata and aquifer data extended |
| `PowerGridSystem` | Deep lamps, pumps, fans, hoist; flare tap via contributions |
| `WaterTreatmentSystem` | Aquifer water enters the live water authority |
| `Inventory` | Ore, tools, samples |
| `SilentFoundrySystem` | Ore grades feed metallurgy |
| `Farming` / `NutritionDiversity` | Cave flora as food/medicine |
| `MemorialSystem` | Mining deaths and heritage memorials |
| `FactionStanceEngine` | Heritage publication and ore trade |
| `CartographySystem` | Depth graph on the existing map authority |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `SubterraneanSystem`, `TunnelNetworkSystem`,
`ExcavationSystem`, `ExcavationHazardSystem`, `SeismicDynamicsSystem`,
`GeologicalStrataCatalog`, `HydroGeologyCatalog`, save stores, and panels. Record
file:line; change nothing.

**Phase 1 — Data + validators.** Extend zones, sites, strata, faults, geodesy,
geothermal, piezometers; author ore, equipment, subsidence, aquifers, gas, cave
species, habitats, heritage. Register validators and scanner.

**Phase 2 — Pure Core.** `DeepSurveySystem`, `MiningSystem`, `SubsidenceSystem`,
`AquiferSystem`, `GasFieldSystem`, `CaveEcologySystem`, `DeepHeritageSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `DeepEarthHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 90/180/360-day soak including extraction, subsidence, and quakes.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Subterranean nodes | 60 new |
| Strata | 18 |
| Ore veins | 20 |
| Mining equipment | 15 |
| Subsidence zones | 12 |
| Aquifers | 8 |
| Gas pockets | 10 |
| Cave species | 30 |
| Cave habitats | 12 |
| Heritage finds | 20 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 28 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Infinite ore | High | Finite reserves; permanent depletion |
| Ignored subsidence | High | Explicit warnings; real damage |
| Second hazard system | High | Route through live hazarding |
| Gas trivialization | Medium | Bounded flare tap; hazard on failure |
| Save bloat (per-node) | Medium | Compact node records; aggregate habitats |
| Determinism break | Low | Preserve `(day, nodeId)` seeding |
| Tone drift to dungeon crawl | Medium | No monsters; ecology and geology |
| Content overrun | Medium | Budget §22 |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Subterranean/DeepSurveyTests.cs`
- `Ashfall.Core.Tests/Subterranean/MiningSystemTests.cs`
- `Ashfall.Core.Tests/Subterranean/SubsidenceTests.cs`
- `Ashfall.Core.Tests/Subterranean/AquiferTests.cs`
- `Ashfall.Core.Tests/Subterranean/GasFieldTests.cs`
- `Ashfall.Core.Tests/Subterranean/CaveEcologyTests.cs`
- `Ashfall.Core.Tests/Subterranean/DeepHeritageTests.cs`
- `Ashfall.Core.Tests/Subterranean/DeepEarthSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Subterranean/DeepEarthDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/GeologyCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Node state never regenerates after collapse or flood.
- Ore reserves deplete permanently and stop at zero.
- Extraction raises hazard and subsidence pressure deterministically.
- Subsidence damages the correct surface rooms and repairs with real materials.
- Aquifer drawdown lowers the surface table and couples to subsidence.
- Gas hazard and flare tap are bounded; failure vents dangerously.
- Cave extraction damages habitats and can destroy species locally.
- Heritage finds never grant combat power.
- Round-trip restores nodes, reserves, subsidence, aquifer, gas, habitats.
- Legacy loads neutral; paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Subterranean/
godot --headless --path . -- --subterranean-selftest
godot --headless --path . -- --excavation-selftest
godot --headless --path . -- --seismic-dynamics-selftest
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

## 15. CROSS-EXPANSION HOOKS

| Expansion | Hook |
|---|---|
| 12 The Second Generation | Deep-born children; deep apprenticeship |
| 13 The Faithful | Cave shrines; earth-reverence movement |
| 14 Above the Ash | Borehole sampling; subsurface maps for flight routes |
| 15 The Deep Root | Cave flora cultivation; aquifer irrigation |
| 16 The Rebuilt Body | Mining prosthetics; robotic drills |
| 17 The Long Evening | Deep acoustics; cave performances |
| 19 The Bitter Air | Gas warfare legacy in sealed levels |
| 20 The Quiet Hand | Hidden deep bunkers; secret archives |
| 21 The Grid | Geothermal tap; pump and fan load |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- The live node state fields and permanent consequences.
- `(day, nodeId)` hazard determinism.
- The T-beam reinforcement contract from `ExcavationSystem`.
- `SeismicDynamicsSystem` as the quake authority.
- The fiction-only and no-monster rules.

### 16.2 New canon

- The six depth tiers and their names.
- The iron seam under the vault and the slow-extraction compromise.
- The Black Lake and the Quiet Cavern.
- The Floor Bunker and the site's buried history.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — DEPTH TIER TABLE

| Tier | Rock | Shoring need | Vent need | Water | Gas | Heritage | Ore |
|---|---|---|---|---|---|---|---|
| 1 Foundations | fill/rubble | low | low | seep | none | service | salvage |
| 2 The Works | clay/sand | med | low | high | low | records | sand/clay |
| 3 The Seam | coal/salt/iron | med | med | med | high | tools | iron/coal/salt |
| 4 The Vault | limestone/karst | high | med | high | med | rooms | cave flora |
| 5 The Deep | granite/quartz | high | high | low | low | lenses | rare ore |
| 6 The Floor | unknown | extreme | extreme | unknown | unknown | the truth | unknown |

Depth tiers are authored data. Capability gates (shoring level, ventilation,
drainage, lamp) are checked before a node can be entered or worked.

---

## 18. APPENDIX B — ORE VEIN TABLE (20 ROWS)

| # | Vein | Ore | Grade ‰ | Reserve | Tier | Subsidence | Notes |
|---|---|---|---|---|---|---|---|
| 1 | Shallow Iron | iron | 420 | 240 | 3 | 0.4 | foundation risk |
| 2 | Deep Iron | iron | 610 | 480 | 4 | 0.5 | better grade |
| 3 | Coal Rib | coal | 700 | 900 | 3 | 0.3 | fuel |
| 4 | Salt Seam | salt | 880 | 1200 | 3 | 0.2 | industrial |
| 5 | Copper Stain | copper | 310 | 160 | 3 | 0.3 | wiring |
| 6 | Copper Lode | copper | 520 | 220 | 4 | 0.4 | better |
| 7 | Grey Quartz | quartz | 400 | 300 | 5 | 0.2 | optics |
| 8 | Black Ore | rare | 210 | 90 | 5 | 0.5 | rare metal |
| 9 | Cobalt Trace | cobalt | 180 | 70 | 5 | 0.4 | batteries |
| 10 | Lead Pocket | lead | 600 | 200 | 4 | 0.3 | shielding |
| 11 | Zinc Bleed | zinc | 350 | 180 | 4 | 0.3 | alloys |
| 12 | Nickel Knot | nickel | 280 | 110 | 5 | 0.4 | steel |
| 13 | Tin Scrap | tin | 330 | 130 | 4 | 0.3 | bronze |
| 14 | Graphite Flake | graphite | 450 | 260 | 4 | 0.2 | lubricants |
| 15 | Sulphur Crust | sulphur | 640 | 300 | 3 | 0.2 | chemistry |
| 16 | Limestone Bed | limestone | 900 | 2000 | 2 | 0.1 | building |
| 17 | Clay Bank | clay | 950 | 2000 | 2 | 0.1 | ceramics |
| 18 | Sand Lens | sand | 980 | 2500 | 2 | 0.1 | glass |
| 19 | Peat Seam | peat | 720 | 700 | 2 | 0.2 | fuel |
| 20 | Deep Unknown | rare | 0 | 40 | 6 | 0.9 | the Floor |

Every reserve is finite. Extraction permanently reduces the number and cannot be
refilled. The shallow iron seam under the vault is deliberately the worst-risk,
worst-grade option, so the player must choose whether to take it.

---

## 19. APPENDIX C — CAVE SPECIES TABLE (30 ROWS)

| # | Species | Type | Light | Food | Medicine | Sensitivity |
|---|---|---|---|---|---|---|
| 1 | Pale Moss | flora | none | no | mild | low |
| 2 | Black Cap Fungus | flora | none | yes | no | med |
| 3 | Glow Lichen | flora | none | no | no | high |
| 4 | Blind Carp | fauna | none | yes | no | med |
| 5 | Cave Cricket | fauna | none | yes | no | low |
| 6 | Stone Snail | fauna | none | yes | no | low |
| 7 | Ribbon Worm | fauna | none | no | yes | med |
| 8 | Damp Bat | fauna | none | yes | no | high |
| 9 | Glow Midge | fauna | none | no | no | high |
| 10 | Pale Cray | fauna | none | yes | no | med |
| 11 | Root Thread | flora | none | no | no | med |
| 12 | Iron Moss | flora | none | no | yes | high |
| 13 | Crystal Beetle | fauna | none | no | no | very high |
| 14 | Cave Newt | fauna | none | yes | no | med |
| 15 | Blind Spider | fauna | none | no | yes | high |
| 16 | Ammonia Fungus | flora | none | no | yes | high |
| 17 | Sulphur Bloom | flora | none | no | no | very high |
| 18 | Chalk Grass | flora | none | no | no | low |
| 19 | Deep Truffle | flora | none | yes | no | very high |
| 20 | Black Eel | fauna | none | yes | no | high |
| 21 | Pale Slug | fauna | none | yes | no | low |
| 22 | Vault Moth | fauna | none | no | no | high |
| 23 | Stone Crab | fauna | none | yes | no | med |
| 24 | Glow Worm | fauna | none | no | no | high |
| 25 | Radiotrophic Mold | flora | none | no | yes | very high |
| 26 | Cold Kelp | flora | none | yes | no | high |
| 27 | Blind Fish | fauna | none | yes | no | high |
| 28 | Cave Bear | fauna | none | yes | yes | extreme |
| 29 | Stone Beast | fauna | none | no | no | extreme |
| 30 | The Quiet Thing | fauna | none | no | no | absolute |

Species 28–30 are apex cave fauna that must not be treated as farmable monsters.
They are hazards and neighbors; extraction and noise disturb them, and the correct
response is usually to leave them alone.

---

## 20. APPENDIX D — SUBSIDENCE MODEL

Subsidence is cumulative and visible. For each extraction event:

```
strain = ore_units * subsidence_affinity * (1 - shoring_mitigation)
zone.strain += strain
zone.crack_rate = f(zone.strain, depth, rock)
surface_damage = zone.strain > zone.threshold
```

| Stage | Visual | Effect |
|---|---|---|
| 0 | none | none |
| 1 | hairline cracks | cosmetic reports |
| 2 | floor tilt | minor room efficiency loss |
| 3 | wall cracks | room condition decay |
| 4 | roof sag | room may be unusable |
| 5 | collapse | room loss, injuries |

Warning comes at stage 2 with explicit lead time. The player can shore the surface,
abandon the room, or stop extraction. Nothing is hidden until stage 5.

---

## 21. APPENDIX E — HAZARD AND MITIGATION TABLE

| Hazard | Live owner | Trigger | Mitigation | Mitigation cost |
|---|---|---|---|---|
| Collapse | SubterraneanSystem | integrity low, quake | shoring 0–3 | timber, T-beam |
| Flooding | SubterraneanSystem | waterLevel rise | pumps, seals | power, parts |
| Toxic gas | TunnelNetworkSystem | gas pocket struck | vent, cap, flare | fans, power |
| Radiation | existing rad system | deep hot zones | shielding, time | lead, dose |
| Darkness | SubterraneanSystem | no lamp | lamps, glow flora | power, items |
| Bad air | SubterraneanSystem | oxygen low | ventilation | fans, duct |
| Sinkhole | SubsidenceSystem | strain threshold | surface shore | concrete, beams |
| Heat | SubterraneanSystem | deep tier | cooling, shift limits | power, water |

All hazards route through their live owners. The expansion adds rates, thresholds,
and content, never a parallel hazard model.

---

## 22. APPENDIX F — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `subterranean_zones.json` | 60 | 8,000 |
| `deep_strata.json` | 18 | 3,000 |
| `ore_veins.json` | 20 | 3,000 |
| `mining_equipment.json` | 15 | 2,000 |
| `subsidence_zones.json` | 12 | 2,000 |
| `aquifers.json` | 8 | 2,000 |
| `gas_pockets.json` | 10 | 2,000 |
| `cave_species.json` | 30 | 5,000 |
| `cave_habitats.json` | 12 | 2,000 |
| `deep_heritage.json` | 20 | 4,000 |
| Quest objectives | 51 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~63,500** |

---

## 23. APPENDIX G — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R18-1 | Infinite ore | Med | High | Finite reserves |
| R18-2 | Ignored subsidence | Med | High | Warnings + damage |
| R18-3 | Second hazard model | Low | High | Route through live owners |
| R18-4 | Gas trivialized | Med | Med | Bounded tap; danger |
| R18-5 | Save bloat | Med | Med | Compact nodes; aggregate habitats |
| R18-6 | Dungeon-crawl tone | Med | Med | No monsters; ecology |
| R18-7 | Determinism | Low | High | Preserve seeding |
| R18-8 | Content overrun | Med | Med | Budget §22 |
| R18-9 | Surface damage unfair | Med | Med | Lead time |
| R18-10 | Aquifer breaks water scarcity | High | High | Drawdown; recharge limits |

---

## 24. APPENDIX H — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Is the deep graph procedurally extended or fully authored?** Recommended:
   authored with seeded connection variation, so it is deterministic and mappable.
2. **Can a flooded node be reclaimed?** Recommended: yes, slowly, with real pumps
   and power; the water must go somewhere.
3. **Does gas flaring contribute power?** Recommended: a small contribution through
   the existing contribution path, not a new generator.
4. **Can cave species be domesticated?** Recommended: cultivation only for flora;
   fauna remains wild.
5. **Is the Floor Bunker's truth campaign-defining?** Recommended: it feeds the
   epilogue and standing, never combat.

---

## 26. APPENDIX I — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_deep_dry_tap` | 3 | Water pressure drops; foundry short on iron; both point down |
| `quest_deep_survey` | 5 | Drill cores; map the seam and aquifer; assess risk |
| `quest_deep_refusal` | 4 | Koole blocks extraction; cost the alternatives |
| `quest_deep_first_cut` | 5 | Shore, ventilate, extract a first ore load |
| `quest_deep_breathing_rock` | 4 | Strike gas; improvise ventilation; avoid ignition |
| `quest_deep_the_flood` | 5 | The Works floods; pump duty; decide seal or drain |
| `quest_deep_the_cave` | 4 | Find the cavern; catalog it; mine or preserve |
| `quest_deep_the_boot` | 5 | Permafrost lens; sealed room; document chain |
| `quest_deep_sinkhole` | 4 | Surface subsidence; repair or abandon a room |
| `quest_deep_second_seam` | 5 | Richer ore behind the cave; choose a path |
| `quest_deep_the_lake` | 5 | Reach the aquifer; test water; manage the descent |
| `quest_deep_floor_bunker` | 6 | Open the Floor; read the site's history |
| `quest_deep_the_commission` | 5 | Publish, seal, or sell the truth |
| `quest_deep_settlement` | 5 | Subsidence, ore, and truth reach a reckoning |
| `quest_deep_what_we_leave_below` | 3 | Final disposition; epilogue |

---

## 27. APPENDIX J — SIDE QUEST DETAIL

**Survey and science**
- `quest_deep_strata_sample` — drill a core; classify hardness, water, gas.
- `quest_deep_fault_map` — trace a fault; predict quake coupling.
- `quest_deep_piezometer` — install monitoring in a wet stratum.
- `quest_deep_insar` — read surface deformation from survey data.
- `quest_deep_archive_compare` — match a stratum to an old borehole record.

**Mining**
- `quest_deep_ore_grade` — assay a seam and decide if it pays.
- `quest_deep_timber` — shore a tunnel before the roof decides.
- `quest_deep_hoist` — repair the shaft hoist and its brake.
- `quest_deep_ore_sort` — separate waste, ore, and the interesting rock.
- `quest_deep_dust_lung` — dust control for the mining crew.

**Hazards**
- `quest_deep_gas_watch` — monitor pressure at the Breathe.
- `quest_deep_canary` — a bird, a cage, and a hard decision.
- `quest_deep_pump_shift` — dewatering duty during a storm above.
- `quest_deep_roof_fall` — a partial collapse; rescue the trapped.
- `quest_deep_bad_air` — evacuate a level before the air kills.

**Water**
- `quest_deep_aquifer_test` — sample and test; clean water has conditions.
- `quest_deep_seal_flood` — seal a level and accept the loss.
- `quest_deep_outfall` — the discharged water has to go somewhere.
- `quest_deep_dry_well` — the surface well fails; the deep tap is the answer.

**Cave ecology**
- `quest_deep_cave_flora` — catalog; some species heal, some glow.
- `quest_deep_cave_fauna` — observe without disturbing.
- `quest_deep_cave_garden` — cultivate against extraction damage.
- `quest_deep_ecology_choice` — the seam runs through the habitat.

**Heritage**
- `quest_deep_permafrost_find` — recover a frozen artifact intact.
- `quest_deep_document_chain` — trace the site's pre-war purpose.
- `quest_deep_seal_room` — the last door; open or pour it shut.

**Surface consequences**
- `quest_deep_crack_watch` — monitor the vault floor.
- `quest_deep_repair_vault` — repair subsidence with cast beams.

---

## 28. APPENDIX K — NPC DOSSIERS (BRIEF)

**Dr. Ivetka Sarne** — geologist. Wants a complete map of the ground and is
unusually careful about what should not be disturbed. She is the expansion's
conscience and its most curious voice: she would rather know than profit.

**Bram Koole** — engineer. Responsible for the vault's load-bearing wall and for
the lives under it. He is not anti-progress; he is pro-standing-structure. His risk
tolerance is low because he has seen what a floor does when it fails.

**Nessa Orr** — quartermaster. Owns the shortage and therefore owns the pressure to
dig. She would trade one risk for another and is not wrong about the arithmetic; she
is only wrong about how much the arithmetic is allowed to cost.

**Haldo Crest** — mine captain. Knows the ground by feel and by sound. Superstitious
in a practical way: he taps the roof before every shift and does not need anyone to
believe him.

**Liss Arn** — dewatering chief. The flood is hers. She keeps a written log of every
litre pumped and knows the outfall is poisoning a stream nobody has tested.

**Noor** — cave diver. The only person willing to swim the Black Lake. Quiet, exact,
and aware that the lake is more likely to keep her than return her.

**Tob** — deep archivist. Records what is found and what is sealed. Believes the
shelter has a right to its own origin and also that some doors are best left locked.

**Pip** — born underground. Has never seen the surface and does not miss it. The
reason the deep is a home and not just a resource.

---

## 29. APPENDIX L — LOCATION DETAIL

- **The Old Adit** — a timber-framed mine mouth; rails rusted into the floor.
- **The Borehole Field** — steel pipes in a grid; core trays and frozen mud.
- **The Sinkhole Row** — a street that fell in; a car on its nose.
- **The Black Lake** — still water, no wind, and a shoreline that keeps moving.
- **The Quiet Cavern** — formations older than the war; light that does not help.
- **The Breathe** — a gas vent that hisses like a slow animal.
- **The Metro Descent** — stairs into black water; handrails still cold.
- **The Floor Bunker** — a door set in bedrock; someone sealed it deliberately.
- **The Outfall** — where pumped water returns to the surface, changed.
- **The Spring Mouth** — clean water from a wall; cave life around it.

---

## 30. APPENDIX M — AQUIFER MODEL

| Field | Meaning |
|---|---|
| `volume_units` | available water, finite |
| `recharge_per_day` | natural refill from the live hydrology system |
| `quality_band` | clean / marginal / contaminated / unsafe |
| `drawdown_rate` | extraction vs. recharge |
| `subsidence_coupling` | drawdown contribution to surface strain |
| `contamination_risk` | salt, metals, or radionuclides |

The Black Lake is the expansion's central water decision. Tapping it can supply the
shelter for a long time, but over-draw lowers the surface table, stresses the
foundation, and can contaminate the water with deep salts. The correct play is
metered use and recharge discipline; the tempting play is maximum flow.

---

## 31. APPENDIX N — GAS FIELD MODEL

| Field | Meaning |
|---|---|
| `pressure` | force of the pocket |
| `volume` | finite gas available |
| `migration_rate` | how fast gas moves through rock |
| `hazard_severity` | density and toxicity in a work area |
| `flare_potential` | contribution to the power grid if flared safely |
| `ignition_risk` | danger during mining or flaring |

Gas is both a resource and a hazard. Venting is safe and wasteful; flaring is
productive and dangerous; capping is cheap and leaves a time bomb. The player
chooses per pocket, and pressure changes over time.

---

## 32. APPENDIX O — DEEP HERITAGE CHAIN TABLE

| # | Find | Layer | Chain | Knowledge | Seal option |
|---|---|---|---|---|---|
| 1 | Frozen Boot | permafrost | boots | origin of a name | leave |
| 2 | Tool Roll | seam | tools | pre-war mining practice | keep |
| 3 | Sample Jar | vault | samples | pre-war research | keep |
| 4 | Ledger Page | works | records | site purchase | seal |
| 5 | Identity Disc | floor | war history | hidden casualty | seal |
| 6 | Sealed Order | floor | orders | who ordered the site | seal |
| 7 | Photograph | vault | images | a family in sunlight | keep |
| 8 | Child's Toy | floor | personal | buried evacuation | keep |
| 9 | Medal | floor | war history | participation | publish |
| 10 | Map Overlay | works | plans | expanded bunker | publish |
| 11 | Skeleton Key | seam | access | unopened room | use |
| 12 | Clinical Notes | vault | medical | experiment traces | seal |
| 13 | Fuel Manifest | works | logistics | supply collapse | publish |
| 14 | Roster | floor | names | the missing | publish |
| 15 | Tape | floor | audio | a voice | listen |
| 16 | Coin Case | vault | valuables | a past economy | sell |
| 17 | Survey Mark | deep | geography | the true site plan | publish |
| 18 | Sealed Letter | floor | personal | a founder's secret | decide |
| 19 | Door Marking | floor | warning | what not to open | seal |
| 20 | The Floor | floor | unknown | unknown | final |

Heritage finds never grant combat power. They change knowledge, standing, morale,
and the epilogue, and they can be sealed forever.

---

## 33. APPENDIX P — DEEP ROOM TABLE

| Room | Function | Power | Requirement |
|---|---|---|---|
| `room_mine_head` | shaft access, sorting | low | adit discovered |
| `room_ventilation_plant` | air for deep tiers | high | duct to tier 3 |
| `room_dewatering_station` | pumps and sumps | high | flooded node |
| `room_deep_barracks` | deep crew rest | med | tier 3 reached |
| `room_strata_lab` | survey and assay | med | survey campaign |
| `room_deep_archive` | heritage storage | low | first find |
| `room_cave_garden` | cultivate cave flora | med | habitat found |
| `room_geothermal_tap` | heat exchange | high | deep heat strata |

Deep rooms compete with surface rooms for construction materials. Every room below
the floor is a bet on depth.

---

## 34. APPENDIX Q — MINING METHODS TABLE

| Method | Rate | Safety | Recovery | Best for |
|---|---|---|---|---|
| Hand Cut | low | med | 0.55 | shallow, soft |
| Drill and Blast | high | low | 0.75 | hard rock |
| Room and Pillar | med | high | 0.65 | broad seams |
| Longwall | very high | low | 0.85 | deep seams |
| Hydraulic | med | low | 0.70 | sand, clay |
| Shored Tunnel | low | very high | 0.50 | under structures |
| Strip the Floor | high | very low | 0.90 | last resort |

Method choice is the expansion's central risk dial. Shored Tunnel under the vault is
slow and preserves the shelter; Strip the Floor is fast and likely ends the campaign
with a sinkhole.

---

## 35. APPENDIX R — SURVEY CAMPAIGN TABLE

| Campaign | Depth | Days | Reveals | Cost |
|---|---|---|---|---|
| Core Sampling | 1–2 | 4 | strata, water | drill, power |
| Seismic Survey | 1–3 | 6 | faults, voids | geophones |
| Borehole Array | 1–4 | 10 | veins, aquifers | pipes, crew |
| Gas Mapping | 3–4 | 8 | pockets, pressure | sensors |
| Deep Sounding | 4–5 | 14 | deep strata, heat | power |
| Floor Probe | 6 | 21 | the site's truth | everything |

Survey cannot be skipped for deep work. Going down without a map is how crews die.

---

## 36. APPENDIX S — WORKED 180-DAY DEEP SCENARIO

**Days 1–20.** Water pressure drops; the foundry rations iron. Sarne surveys a
shallow seam and a deep aquifer. Koole refuses the seam; the player authorizes a
shored, slow trial.

**Days 21–50.** The first cut produces 60 units of iron at grade 420. The Works
level floods from a breached clay lens; pumps run 300 W continuously. Liss logs an
outfall that is warmer and saltier than the surface stream.

**Days 51–80.** A gas pocket is struck at the seam. The crew vents it; pressure
stays high. The cave is found beyond the seam; extraction would collapse its roof.

**Days 81–110.** The permafrost boot is recovered. Subsidence stage 2 appears on the
vault floor. The player shores the surface with cast beams and slows extraction.

**Days 111–140.** The Black Lake is reached. Tests show clean water with a deep salt
edge. Over-draw triggers stage 3 cracks. The cave garden begins as insurance.

**Days 141–170.** The Floor Bunker is opened. The document chain explains that the
site was chosen for its impermeable floor, and that the floor was already used once.
The commission debates publication.

**Days 171–180.** The final settlement: ore reserves, water volume, habitat health,
surface strain, and the truth. The epilogue records what the shelter chose to leave
below.

---

## 37. APPENDIX T — VIGNETTE (TONE SAMPLE)

> Haldo taps the roof twice with the flat of his hand before every shift, and
> nobody laughs, because the roof has answered him once and that was enough. The
> lamp makes a circle about three metres wide. Outside the circle, the seam goes on
> in both directions, black and patient, and the air tastes like a coin held too
> long on the tongue.
>
> Liss stands at the pump with her hand on the pipe, feeling the water come up warm.
> Above them, in the shelter, the floor is very slightly wrong, and only one person
> has noticed, and she has not yet decided to say so.

This sets the register: quiet, physical, and aware that the ground is listening.

---

## 38. APPENDIX U — BALANCE CONSTANTS (PROPOSED)

| Constant | Value | Rationale |
|---|---|---|
| Max ore per node per day | 20 units | prevents exponential extraction |
| Max reserves per vein | 2500 units | finite but meaningful |
| Subsidence warning stage | 2 | fair lead time |
| Subsidence repair cost | 2 cast beams per stage | ties to foundry |
| Aquifer safe draw | 60% recharge | prevents free water |
| Gas flare contribution | ≤ 5% grid | bounded |
| Cave habitat recovery | 0.5%/day undisturbed | slow healing |
| Deep crew fatigue | 1.5× surface | depth costs |
| Max concurrent deep rooms | 4 | prevents deep sprawl |
| Heritage combat power | 0 | hard rule |

---

## 39. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] No real-world mine, aquifer, or geological formation is named.
- [ ] No monsters or magic materials; cave fauna is plausible and respected.
- [ ] Node consequences are permanent and never silently reset.
- [ ] Extraction is finite and depletion is visible.
- [ ] Subsidence warnings precede damage.
- [ ] Aquifer draw has a real cost and cannot trivialize water scarcity.
- [ ] Gas flaring is bounded and dangerous.
- [ ] Cave extraction damages habitats with real consequences.
- [ ] Heritage finds never grant combat power.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism preserves the `(day, nodeId)` seeding contract.

---

## 40. APPENDIX W — GLOSSARY

- **Depth tier** — one of six authored geological bands.
- **Node** — a live `SubterraneanSystem` graph node with persistent state.
- **Shoring** — 0–3 structural reinforcement of a node.
- **Subsidence** — cumulative surface deformation caused by extraction.
- **Drawdown** — aquifer depletion relative to recharge.
- **Gas pocket** — a finite pressurized gas body.
- **Habitat** — a cave ecosystem unit linked to nodes.
- **Lens** — a sealed permafrost/till pocket holding heritage material.
- **The Floor** — the sixth tier and the site's buried truth.

---

## 41. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`ore_veins.json`** — `vein_id`, `display_name`, `ore_item_id`, `grade_permille`,
`reserve_units`, `depth_tier`, `hardness`, `gas_affinity`, `subsidence_affinity`,
`mining_methods[]`, `tags`.

**`aquifers.json`** — `aquifer_id`, `display_name`, `volume_units`,
`recharge_per_day`, `quality_band`, `subsidence_coupling`, `contamination_risk`,
`linked_nodes[]`, `tags`.

**`gas_pockets.json`** — `pocket_id`, `display_name`, `pressure`, `volume`,
`migration_rate`, `hazard_severity`, `flare_potential`, `ignition_risk`,
`linked_nodes[]`, `tags`.

**`cave_species.json`** — `species_id`, `display_name`, `kind` (`flora`|`fauna`),
`habitat_ids[]`, `light_need`, `food_value`, `medicine_value`,
`cultivation_difficulty`, `extraction_sensitivity`, `tags`.

**`cave_habitats.json`** — `habitat_id`, `display_name`, `linked_nodes[]`,
`species_pool[]`, `health`, `damage_sources[]`, `recovery_rate`, `tags`.

**`deep_heritage.json`** — `find_id`, `display_name`, `layer`, `artifact_chain`,
`document_node`, `knowledge_value`, `standing_effect`, `seal_options[]`, `tags`.

**`subsidence_zones.json`** — `zone_id`, `display_name`, `linked_nodes[]`,
`surface_rooms[]`, `crack_rate`, `strain_threshold`, `repair_cost[]`, `tags`.

**`deep_strata.json`** — `stratum_id`, `depth_band`, `material`, `hardness`,
`water_content`, `gas_affinity`, `heritage_affinity`, `survey_difficulty`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid node/room/item references, or out-of-range numbers.

---

## 43. APPENDIX Y — MINERAL PROCESSING TABLE

| Input | Process | Output | Byproduct | Facility |
|---|---|---|---|---|
| iron ore | crush, roast, smelt | iron ingot | slag | foundry |
| copper ore | crush, roast, smelt | copper ingot | sulphur gas | foundry |
| coal | sort, wash | coke fuel | coal dust | foundry |
| salt | crush, refine | salt | brine | salt works |
| limestone | calcine | lime | CO2 | kiln |
| clay | wedge, fire | brick/tile | grog | kiln |
| sand | wash, melt | glass | fines | glassworks |
| quartz | sort, cut | optic blanks | chips | workshop |
| rare ore | acid leach | rare salts | tailings | chemistry |
| cobalt trace | roast, leach | cobalt salts | tailings | chemistry |
| lead | smelt | lead sheet | dross | foundry |
| zinc | smelt | zinc | dross | foundry |
| graphite | mill | lubricant | dust | workshop |
| sulphur | melt, cast | sulphur blocks | fumes | chemistry |
| peat | dry | fuel | ash | cellar |

Processing ties the deep directly to the live foundry, chemistry, ceramics, and
glass systems. No new production system is created; ore is an input to the existing
chain.

---

## 44. APPENDIX Z — DEEP ECOLOGY FOOD WEB

```
   Deep energy sources
   ├─ chemical (sulphur, iron) ──► bacteria ──► snails, worms ──► fish ──► cave bear
   ├─ organic seep (peat, guano) ──► fungi ──► crickets, moths ──► spiders ──► newts
   └─ geothermal heat ──► lichen ──► midges ──► bats ──► apex
```

| Level | Examples | Extraction sensitivity |
|---|---|---|
| Producers | moss, lichen, sulphur bloom | high |
| Primary consumers | snails, crickets, worms | medium |
| Secondary consumers | spiders, newts, cray | high |
| Tertiary | fish, bats, eels | high |
| Apex | cave bear, stone beast | extreme |

Mining severs the food web at the producer level. Flooding drowns the lower levels.
Flaring poisons the air layer. The player can preserve a habitat by mining around it,
which costs time and ore access.

---

## 45. APPENDIX AA — SEISMIC COUPLING MODEL

`SeismicDynamicsSystem` already forecasts quakes. The expansion adds a two-way
coupling that does not duplicate it:

| Cause | Effect | Route |
|---|---|---|
| Extraction | raises local strain | SubsidenceSystem |
| Blasting | raises quake probability briefly | SeismicDynamicsSystem input |
| Dewatering | lowers pore pressure, can trigger slip | AquiferSystem → Seismic |
| Injection (disposal) | raises pore pressure | AquiferSystem → Seismic |
| Natural quake | damages nodes, raises gas migration | SubterraneanSystem |
| Quake | damages surface rooms | PowerGrid/shelter state |

The system never invents quakes outside the live seismic model; it adds pressure to
it, so the forecast remains meaningful.

---

## 46. APPENDIX AB — DEEP INCIDENT TABLE

| Incident | Cause | Severity | Response |
|---|---|---|---|
| Roof fall | integrity low | high | rescue, shore |
| Kick-through | karst void | high | rope, light |
| Gas ignition | flame near pocket | critical | evacuate, seal |
| Flash flood | breached lens | critical | pumps, gate |
| Bad air | ventilation failure | high | evacuate, fan |
| Hoist failure | worn cable | high | rescue |
| Gas poisoning | exposure | med | air, clinic |
| Drowning | Black Lake | critical | diver rescue |
| Burn | flare accident | high | medical |
| Cave-in on heritage | bad shoring | high | salvage, grief |
| Sinkhole opens | strain stage 5 | critical | rescue, abandon |
| Lost crew | navigation | critical | search |

Incidents route to existing medical, memorial, and shelter systems. No new incident
bus is created.

---

## 47. APPENDIX AC — DEEP-BORN GENERATION

A shelter that lives underground long enough produces people who have never seen
sky. The expansion makes this a real identity, not a label:

| Trait | Effect | Interaction |
|---|---|---|
| `deep_born` | surface discomfort, dark comfort | Expansion 12 childhood |
| `cave_sighted` | better low-light work | exploration |
| `stone_ear` | hears roof stress | mining |
| `surface_dread` | morale penalty above ground | expeditions |
| `deep_loyal` | bonds to the deep shelter | morale |
| `pale_frame` | low UV tolerance | health |

These traits are authored additions to the existing development trait catalog. They
connect The Underneath to The Second Generation and give the deep a culture, not
just a resource.

---

## 48. APPENDIX AD — WORKED BALANCE EXAMPLE

**Scenario.** The shelter chooses the shored-slow extraction under the vault.

| Day | Action | Ore | Subsidence | Water | Power |
|---|---|---|---|---|---|
| 20 | Shored excavation | 0 | 0 | −2% | +80 W |
| 40 | First ore | 60 | 0.1 | −4% | +120 W |
| 60 | Gas venting | 80 | 0.2 | −6% | +150 W |
| 80 | Cave preserved | 90 | 0.3 | −8% | +150 W |
| 100 | Aquifer tapped | 100 | 0.6 | +10% | +220 W |
| 120 | Stage 2 cracks | 110 | 1.0 | +6% | +220 W |
| 140 | Over-draw | 120 | 1.5 | −4% | +260 W |
| 160 | Surface shore | 120 | 1.1 | −2% | +240 W |
| 180 | Stable | 130 | 1.0 | 0% | +240 W |

The intent: extraction pays for itself in iron and heat, but the shelter never stops
paying for extraction. A player who strips the seam gets more ore faster and a
sinkhole; a player who shores gets less ore and keeps the vault.

---

## 49. APPENDIX AE — CONTENT CROSS-REFERENCE

The Underneath deliberately reuses existing content anchors rather than inventing
parallel ones:

| Existing anchor | Reused as |
|---|---|
| `geothermal_strata_catalog.json` | deep heat tap strata |
| `geothermal_drilling_depths.json` | borehole depth bands |
| `insar_geodesy_catalog.json` | deformation monitoring |
| `piezometer_network_catalog.json` | aquifer pressure monitoring |
| `seismic_fault_catalog.json` | extraction-quake coupling |
| `geodetic_survey_catalog.json` | surface control points |
| `underground_flora.json` | cave flora base |
| `excavation_sites.json` | deep room digs |
| `excavation_hazard_mitigation.json` | hazard responses |
| `mine_flail_catalog.json` | clearing equipment |
| `scavenging_tables.json` | deep salvage |
| `subterranean_zones.json` | the node graph |
| `sump_drainage_catalog.json` | dewatering |
| `fluid_infrastructure.json` | pumping and outfall |
| `alloys_and_ores.json` | ore vocabulary |
| `metallurgy_recipes.json` | processing |
| `glassworks_recipes.json` | sand and quartz |
| `salt_mine` content | salt seam |
| `gpr_exploration_catalog.json` | pre-dig scanning |
| `hydrogeology` archives | aquifer history |

This reuse is what keeps the expansion from creating parallel geology, parallel
water, or a parallel ore economy.

---

## 50. APPENDIX AF — STAFFING AND LABOR MODEL

Deep work is labor-intensive and dangerous. The expansion uses the existing
`LaborProductivity` and `DutyRoster` authorities:

| Role | Needed for | Fatigue | Risk |
|---|---|---|---|
| Miner | extraction | high | high |
| Shorer | stabilization | high | med |
| Ventilation tech | air | med | med |
| Pump operator | water | med | low |
| Surveyor | mapping | med | low |
| Assayer | ore grade | low | low |
| Cave guide | exploration | high | high |
| Diver | Black Lake | extreme | extreme |

Roles reuse existing skills where they exist (mining maps to labor/craft, surveying
to knowledge, diving to a new skill or a survival specialization). Displaced surface
labor is a real opportunity cost: every deep crew member is a surface worker.

---

## 51. APPENDIX AG — EPILOGUE HOOKS

| Choice | Epilogue line (proposed) |
|---|---|
| Shored extraction | "They took from the ground slowly, and the shelter stood." |
| Stripped floor | "They took everything, and the floor remembered." |
| Preserved cave | "They left one room of the deep alone, and it lived." |
| Sealed heritage | "The door was poured shut, and the question with it." |
| Published truth | "They told the shelter where it stood, and why." |
| Tapped lake | "The black water rose to their lamps." |
| Sealed lake | "The lake kept its water and its dead." |
| Deep-born home | "A generation learned the dark and called it home." |

Hooks append to `epilogue_chronicle.json` and the existing ending matrix. No new
ending authority is created.

---

## 52. APPENDIX AH — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `SubterraneanSystem` | node state | node state | surface topology |
| `TunnelNetworkSystem` | segments | segment state | loot |
| `ExcavationSystem` | sites | progress | health |
| `ExcavationHazardSystem` | hazard defs | hazards | — |
| `SeismicDynamicsSystem` | pressure | forecast | — |
| `GeologicalStrataCatalog` | strata | — | — |
| `HydroGeologyCatalog` | aquifer data | discoveries | water inventory |
| `PowerGridSystem` | draw | contributions | — |
| `WaterTreatmentSystem` | raw water | treated water | — |
| `Inventory` | items | transfers | — |
| `SilentFoundrySystem` | ore | products | — |
| `Farming` | cave flora | cultivation | — |
| `MemorialSystem` | deaths | memorials | — |
| `CartographySystem` | depth graph | map nodes | — |
| `FactionStanceEngine` | heritage | standing | — |

---

## 53. CLOSING STATEMENT

ASHFALL already owns the ground: nodes, integrity, air, water, shoring, quakes,
strata, and the dig itself. What it lacks is the world below — ore and mining,
gas and aquifers, subsidence and ecology, and a buried history under the shelter's
foundation. The Underneath adds that world without adding a second network, a
second hazard model, or a second map. It adds depth, and with it the oldest
survival bargain of all: you can take from the ground, and the ground will take
something back.

> Wave 2 note: this plan is one of five Wave 2 expansion bibles (17–21). Each is
> self-contained; none requires another to ship. The shared Wave 2 index lives at
> `docs/expansions/wave2/WAVE2_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. No claim on
> `SubterraneanSystem`, `ExcavationSystem`, `SeismicDynamicsSystem`, or any save
> store may be made until a foreman records the package in `INTEGRATION_PLANS.md`.
>
> Evidence anchors used throughout: `SubterraneanSystem` (Plan 156), `TunnelNetworkSystem`,
> `ExcavationSystem` and `ExcavationHazardSystem`, `SeismicDynamicsSystem`,
> `GeologicalStrataCatalog`, `HydroGeologyCatalog`, `SaltMineExtractionSystem`, and the
> live `subterranean`, `excavation`, and `excavation_hazards` save sections.