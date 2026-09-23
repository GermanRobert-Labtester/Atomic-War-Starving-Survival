# ASHFALL — Expansion 22 Design Bible
# THE CLEAN FLOW
### Wave 3 · Water Quality, Sanitation, Public Health, Waste, Drainage, and Hygiene

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core` (WaterTreatment, BrineWater, SumpFlooding), `Ashfall.Core.Shelter` (Sanitation, ChlorAlkali), `Ashfall.Core.Disease` (exposure), `Ashfall.Core.World`
**Proposed host owner:** `CleanFlowHostSession` (extends `WaterTreatmentHostSession` + `SanitationHostSession` + `SumpFloodingHostSession`)
**Existing save sections:** `water_treatment`, `sanitation`, `sump`, `brine`, `chlor_alkali`, `fluid_logistics`
**Existing CLI verbs:** `--water-treatment-selftest`, `--sanitation-selftest`, `--sump-flooding-selftest`, `--brine-selftest`, `--chlor-alkali-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models water as a chain, not a bar. `WaterTreatmentSystem` owns the
bunker's water inventories and treatment jobs — raw, brackish, irradiated, and clean
water, filter integrity, charcoal, distillation fuel, treatment modes, contamination
profiles, and active jobs — and enforces mass balance: *water is never created from
nothing*. It emits disease, heavy-metal, and radiation exposure into the canonical
`DiseaseSystem`, needs, and medical/dose pipelines. `BrineWaterSystem` remains the
external source/membrane/treaty adapter. `SanitationSystem` owns waste types
(organic, chemical, radioactive), cleaning priority, and a derived hygiene band.
`WaterborneExposureRules` maps contaminated output to authored typhoid and dysentery
exposure through `DiseaseSystem.TryExpose` with **no RNG at all**. `SumpFloodingSystem`
owns groundwater nodes, incidents, sludge cake, hazardous tailings, unrouted
greywater, and a centrifuge. `ChlorAlkaliSynthesisEngine` and `FogHarvestingCatalog`
add production and harvest.

But the content is thin: `fluid_infrastructure.json` is 1.3 KB,
`sump_drainage_catalog.json` 1.8 KB, `fog_harvesting_catalog.json` 1.9 KB,
`sanitation_facilities.json` 3.9 KB, `chlor_alkali_synthesis_catalog.json` 1.6 KB.
A system that supports an entire public-health layer has almost no authored world.

**The Clean Flow** turns that machinery into the campaign's quiet everyday crisis:
water quality that must be tested, waste that must be routed, hygiene that must be
practiced, drainage that must not fail, and a public-health politics where the
cleanest water always has a queue.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter will survive a raid. It may not survive its own latrine.

**The Clean Flow** is the expansion about the unglamorous systems that kill people
slowly: contaminated water, unwashed hands, blocked drains, rotting organic waste,
a rising water table, and a treatment plant running one filter change past its life.
It is the expansion where the enemy is not a faction but a fecal-oral route.

The expansion's engine is the live water chain. Raw water comes from a source —
rain, fog, spring, brine, aquifer, sump, or melt. It is treated with a mode, a
filter, charcoal, and fuel. Its output has a quality and a contamination profile.
People drink it. What they excrete becomes waste, which becomes hygiene risk, which
becomes disease risk, which becomes a public-health response. Every step already
exists in code; the expansion gives every step a place, a person, and a cost.

### 1.2 The five loops it adds

```
   Source ──► Treatment ──► Quality ──► Consumption ──► Waste ──► Sanitation
      │           │            │             │             │            │
      ▼           ▼            ▼             ▼             ▼            ▼
   Fog, rain,  modes,       testing,     clean vs      organic,     hygiene,
   spring,     filters,     profiles,    unsafe        chemical,    laundering,
   brine,      fuel         backlog      ration        radioactive  bathing
   sump                                                                   │
      │                                                                   ▼
   Drainage ◄── greywater ◄── bathing/laundry ◄── public health measures ◄┘
      │
      ▼
   Groundwater ─► flooding ──► sludge ──► cake/tailings ──► disposal
```

### 1.3 What the player manages

1. **Sources.** Every water source has a yield curve, a quality band, a season, and
   an owner. Rain is free and unreliable; brine is steady and treaty-bound; fog is
   seasonal; the spring is contested; the sump is a last resort.
2. **Treatment.** `WaterTreatmentSystem` modes, filter integrity, charcoal supply,
   and distillation fuel. Running a filter too long is a slow poisoning; replacing
   one too early is waste.
3. **Quality.** Testing, profiles, and backlog. The player often does not know the
   exact contamination until the results come back, and the shelter drinks in the
   meantime.
4. **Sanitation.** Latrines, bathing, laundry, and cleaning priority. Hygiene is a
   derived band with real disease consequences.
5. **Waste.** Three streams (organic, chemical, radioactive) with different routing,
   storage, reuse, and disposal. Compost is a resource; tailings are a liability.
6. **Drainage.** Sump nodes, groundwater level, greywater, flooding, and the
   dewatering plant. A blocked drain is a slow disaster.
7. **Public health.** Measures, campaigns, and compliance; the difference between a
   bad week and an outbreak.

### 1.4 What it is not

- Not a second water system. `WaterTreatmentSystem` and `BrineWaterSystem` remain the
  owners; everything routes through them.
- Not a second disease system. Exposure routes through `DiseaseSystem.TryExpose`.
- Not a second sanitation system. `SanitationSystem` owns waste types and hygiene.
- Not a second drainage system. `SumpFloodingSystem` owns nodes and groundwater.
- Not a second save authority; all new state is additive in existing envelopes.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | Bunker water inventories, treatment jobs, modes, filter/charcoal/fuel, mass balance, exposure emission | `LIVE` |
| `Assets/Ashfall.Core/BrineWaterSystem.cs` | External brine source, membrane, treaty adapter | `LIVE` |
| `Assets/Ashfall.Core/Shelter/FluidWaterTreatmentBridge.cs` | Fluid logistics to treatment bridge | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SanitationSystem.cs` | Waste taxonomy, cleaning priority, derived hygiene band | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SanitationConsequenceRules.cs` | Hygiene consequences | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SanitationFacilityCatalog.cs` | Facility catalog | `LIVE` |
| `Assets/Ashfall.Core/WaterborneExposureRules.cs` | Authored waterborne exposure (typhoid, dysentery), no RNG | `LIVE` |
| `Assets/Ashfall.Core/SumpFloodingSystem.cs` | Groundwater nodes, incidents, sludge, greywater, centrifuge | `LIVE` |
| `Assets/Ashfall.Core/SumpDrainageCatalog.cs` | Sump catalog | `LIVE` |
| `Assets/Ashfall.Core/Shelter/FogHarvestingCatalog.cs` | Fog harvest | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs` | Chlor-alkali chemistry | `LIVE` |
| `Assets/Ashfall.Core/Narrative/WaterTreatmentPotableCatalog.cs` | Potable output catalog | `LIVE` |
| `src/Host/WaterTreatmentHostSession.cs`, `SanitationHostSession.cs`, `SumpFloodingHostSession.cs` | Host | `LIVE` |
| `src/UI/WaterTreatmentPanel.cs`, `SanitationPanel.cs`, `SumpFloodingPanel.cs`, `BrineExtractionPanel.cs` | UI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `fluid_infrastructure.json` | 1.3 KB | fluid routing |
| `sump_drainage_catalog.json` | 1.8 KB | sump nodes and cake profiles |
| `fog_harvesting_catalog.json` | 1.9 KB | fog fences |
| `sanitation_facilities.json` | 3.9 KB | facility definitions |
| `chlor_alkali_synthesis_catalog.json` | 1.6 KB | chemistry |
| `water_quality_test_reports_batch_2.json` | narrative | test reports |
| `artesian_well_contamination_logs.json` | narrative | contamination records |
| `slow_sand_schmutzdecke_logs.json` | narrative | slow sand filters |

### 2.3 Confirmed gaps

- **GAP-22-1 — Sources are nearly unlisted.** The treatment system accepts raw,
  brackish, irradiated, and clean water, but there is no authored source world with
  yields, seasons, quality bands, and owners.
- **GAP-22-2 — No water-quality profile catalog.** Output quality is mechanically
  tracked but not authored into testable profiles with consequences and thresholds.
- **GAP-22-3 — Sanitation facilities are a short table.** No latrine types, bath
  houses, laundries, or cleaning regimes with real trade-offs.
- **GAP-22-4 — Hygiene is a band, not a practice.** No authored practices, culture,
  compliance, or education.
- **GAP-22-5 — Waste routing is shallow.** Three streams exist; there is no routing
  network, reuse, composting, or disposal site content.
- **GAP-22-6 — Drainage has few nodes.** The sump system supports a network; only a
  small catalog exists, and the groundwater model lacks a full map.
- **GAP-22-7 — No public-health measures.** No campaigns, no inspections, no
  compliance, no outbreak-response drills.
- **GAP-22-8 — No water locations or NPCs.**
- **GAP-22-9 — No water commons.** Regional water sharing and treaties are absent
  though brine treaties and the Grid intertie provide the pattern.

### 2.4 Non-duplication statement

This expansion will **not** add a second water, sanitation, drainage, disease, or
save system. It extends `WaterTreatmentSystem`, `BrineWaterSystem`,
`SanitationSystem`, `SumpFloodingSystem`, and `ChlorAlkaliSynthesisEngine` with data
and additive subsystems. It routes all exposure through `DiseaseSystem.TryExpose`
and all hydration through `NeedsSystem`. It does **not** duplicate the Bitter Air
expansion's hazard, quarantine, or decontamination content: this expansion is about
water, waste, and hygiene, and cross-links to Bitter Air where the two meet.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Water is a chain, not a tap.** Every litre has a source, a treatment,
a quality, a consumer, and a waste product. The expansion makes the chain visible.

**Pillar 2 — The slow killer.** Cholera is not dramatic; it is a latrine uphill of a
well. The expansion's tension is quiet, cumulative, and preventable.

**Pillar 3 — Cleanliness is labor.** Hygiene costs water, fuel, soap, and hours.
A shelter that bathes is a shelter that does not drill. That trade is the point.

**Pillar 4 — Waste is a resource and a liability.** Compost feeds fields; tailings
poison wells. The player decides which stream goes where.

**Pillar 5 — Testing is uncertainty.** A test takes time. The shelter drinks while it
waits. The expansion never gives perfect knowledge for free.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A water test | A tray, a count, a delayed answer | Instant glowing readout |
| A latrine | Distance, slope, lime, fly netting | Gross-out jokes |
| A bath | A ration of warm water and a queue | Spa imagery |
| Waste sorting | Gloves, bins, a schedule | Gross spectacle |
| A drain | Standing water, a smell, a rod | Sewer adventure |
| Public health | A notice, a demonstration, a rule | Preaching |

### 3.3 Content limits

- No real-world disease, water system, or sanitation program is named.
- Fecal-oral transmission is depicted practically and without disgust as a device.
- No shaming of survivors for hygiene failure; systems fail, people cope.
- Child content respects dignity; no scatological humor.
- No content that trivializes the water scarcity loop.

---

## 4. THE CLEAN FLOW WORLD

### 4.1 Interior rooms

- **`room_water_works`** — treatment modes, filters, charcoal, fuel, and testing.
- **`room_latrine_block`** — latrines, wash points, and lime storage.
- **`room_bath_house`** — a ration of warm water and a queue.
- **`room_laundry`** — wash water, soap, drying, and greywater routing.
- **`room_waste_sorting`** — three streams, gloves, and a schedule.
- **`room_compost_works`** — organic waste to soil.
- **`room_public_health_office`** — testing, notices, inspection, and records.
- **`room_sump_plant`** — sump nodes, dewatering, cake, and tailings.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_spring_mouth` | The Spring Mouth | 4 | Clean spring; contested |
| `loc_fog_fence` | The Fog Fence | 4 | Fog harvest arrays |
| `loc_rain_cistern` | The Cistern Field | 3 | Rain capture and storage |
| `loc_deep_well` | The Deep Well | 5 | Artesian access and testing |
| `loc_latrine_field` | The Sunken Latrines | 3 | Sanitation and composting |
| `loc_waste_yard` | The Sorting Yard | 4 | Waste routing and reuse |
| `loc_drain_outfall` | The Outfall | 5 | Greywater discharge and testing |
| `loc_bath_ruins` | The Old Baths | 5 | Pre-war bathing facility salvage |
| `loc_slow_sand_bed` | The Slow Sand Beds | 5 | Filtration beds and media |
| `loc_water_tower` | The Tower | 6 | Distribution and pressure |

All locations require valid item references and scanner registration.

### 4.3 The water map

Water is a graph like the grid: sources feed treatment, treatment feeds storage,
storage feeds consumers, consumers produce waste, waste routes to disposal or reuse,
and drainage returns to groundwater. The expansion authors the graph, the live
systems walk it, and the panels show it honestly.

---

## 5. MAIN STORYLINE — "WHAT THE WELL REMEMBERS"

### 5.1 Central conflict

The shelter's clean water runs out three days into a heat wave. The treatment plant's
filter is past its life, the charcoal is low, and the distillation still is reserved
for the clinic. The water keeper, **Nessa Ord**, proposes drawing from the old
latrine field's downhill seep — it is close, abundant, and almost certainly foul.
The physician, **Dr. Asa Vell**, refuses. The greenhouse keeper points out that the
crops are dying anyway, and that greywater could save them.

Then the first cases appear. Two children, then a worker. It is either typhoid or
something the shelter has not seen, and the tests take four days. In those four days
the shelter must decide: ration the remaining clean water, drink the questionable
water, or close the bath house and let hygiene collapse.

At the same time, a neighboring settlement offers a water treaty: their spring
against the shelter's treatment capacity. The treaty is generous, and it would make
the shelter dependent. And in the old baths, there is a pre-war slow-sand filter
design that could double the plant's capacity — if the shelter can salvage, clean,
and operate it.

The expansion's question: **when clean water runs out, what does the shelter drink,
and who decides?**

### 5.2 Theme (unspoken)

**You cannot see the thing that will kill you. You can only build the habits that
keep it out.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_water_keeper_nessa_ord` | Nessa Ord | Water keeper | Owns the chain; tempted by shortcuts |
| `npc_physician_asa_vell` | Dr. Asa Vell | Physician | Refuses foul water; tracks cases |
| `npc_sanitation_chief_berek` | Berek Halm | Sanitation chief | Latrines, waste, and dignity |
| `npc_laundry_mistress_lir` | Lir Venn | Laundry mistress | Hygiene's unpaid general |
| `npc_public_health_tam` | Tam Orr | Public health clerk | Testing, notices, and compliance |
| `npc_drain_boss_koole` | Bram Koole | Drain boss | Sump, groundwater, and flooding |
| `npc_spring_envoy_vene` | Vene Hask | Neighbor envoy | Offers the water treaty |
| `npc_child_well_pim` | Pim | Child of the queue | The human face of water scarcity |

### 5.4 Story beats (15)

1. **The Heat.** Clean water runs out; the filter is past its life.
2. **The Shortcuts.** Nessa proposes the seep; Asa refuses; the argument is costed.
3. **The First Cases.** Two children, one worker; tests begin.
4. **The Queue.** Rationing clean water creates a daily line and a daily politics.
5. **The Four Days.** The shelter waits for results and decides what to drink.
6. **The Latrines.** Berek's inspection finds a slope directing flow toward the well.
7. **The Bath House.** Closing it saves water and costs hygiene.
8. **The Slow Sand.** The pre-war filter design is found in the old baths.
9. **The Greywater.** Lir proposes reusing wash water for the greenhouse.
10. **The Treaty.** Vene offers the spring; the terms are debated.
11. **The Outbreak.** The results return; the response begins or is botched.
12. **The Compost.** Organic waste becomes soil; the cycle closes or poisons.
13. **The Sump.** Groundwater rises; the dewatering plant runs continuously.
14. **The Reckoning.** Costs counted: cases, casualties, trust, and the chain.
15. **The Clean Flow.** Final disposition of the water system and the treaty.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Drinking water | ration clean / use questionable / close baths | safety vs. volume |
| Latrine siting | relocate / lime aggressively / cap | cost vs. risk |
| Greywater | reuse for crops / discharge / filter | conservation vs. safety |
| Treaty | sign / refuse / counter-offer | dependence vs. supply |
| Filter design | salvage slow sand / buy / improvise | capacity vs. risk |
| Public health | mandatory / voluntary / minimal | compliance vs. freedom |
| Waste | compost / bury / trade | use vs. liability |
| Final | own the chain / share / ration forever | legacy |

### 5.6 Endings (5 + fade)

1. **The Clean Spring** — the chain is secured; the shelter stops fearing its water.
2. **The Long Queue** — permanent rationing; the shelter survives, disciplined and tired.
3. **The Outbreak** — a preventable epidemic; the shelter loses people it did not have to.
4. **The Shared Well** — the treaty holds; two settlements drink from one chain.
5. **The Closed Baths** — hygiene is sacrificed for volume; disease risk becomes a season.
6. **Fade** — the heat passes; the filter is replaced; nothing is decided.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_flow_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_flow_the_heat`, `quest_flow_shortcuts`, `quest_flow_first_cases`,
`quest_flow_the_queue`, `quest_flow_four_days`, `quest_flow_latrines`,
`quest_flow_bath_house`, `quest_flow_slow_sand`, `quest_flow_greywater`,
`quest_flow_treaty`, `quest_flow_outbreak`, `quest_flow_compost`,
`quest_flow_the_sump`, `quest_flow_reckoning`, `quest_flow_clean_flow`.

### 6.2 Side quests (30)

**Sources (5)**
- `quest_flow_fog_fence` — repair fog harvest arrays
- `quest_flow_rain_capture` — expand cistern capture
- `quest_flow_deep_well` — test and restore the artesian well
- `quest_flow_spring_claim` — contest or share the spring
- `quest_flow_melt_yield` — seasonal melt capture

**Treatment (5)**
- `quest_flow_filter_change` — change a filter under load
- `quest_flow_charcoal_run` — make or salvage charcoal
- `quest_flow_distillation_fuel` — fuel for the still
- `quest_flow_mode_choice` — choose a treatment mode with trade-offs
- `quest_flow_slow_sand_build` — build and seed a slow-sand bed

**Quality and testing (5)**
- `quest_flow_test_kit` — build a testing kit
- `quest_flow_backlog` — clear a test backlog
- `quest_flow_quality_map` — map quality across sources
- `quest_flow_false_clean` — a result is wrong
- `quest_flow_public_notice` — publish the results honestly

**Sanitation and hygiene (5)**
- `quest_flow_latrine_build` — build latrines uphill-safe
- `quest_flow_bath_ration` — set the bathing ration
- `quest_flow_laundry_water` — laundry water budget
- `quest_flow_soap_run` — soap production or salvage
- `quest_flow_hand_practice` — establish handwashing practice

**Waste (5)**
- `quest_flow_sort_streams` — separate waste streams
- `quest_flow_compost_batch` — compost a batch
- `quest_flow_tailings_cask` — cask hazardous tailings
- `quest_flow_waste_reuse` — reuse a stream safely
- `quest_flow_disposal_site` — find a disposal site

**Drainage and public health (5)**
- `quest_flow_drain_rod` — clear a blocked drain
- `quest_flow_groundwater_watch` — monitor the water table
- `quest_flow_dewater_shift` — run the dewatering plant
- `quest_flow_inspection` — run a public-health inspection
- `quest_flow_outbreak_drill` — a response drill

### 6.3 Repeatable quests (8)

`quest_flow_repeat_test`, `quest_flow_repeat_filter`, `quest_flow_repeat_latrine`,
`quest_flow_repeat_laundry`, `quest_flow_repeat_sort`, `quest_flow_repeat_drain`,
`quest_flow_repeat_meter`, `quest_flow_repeat_inspect`.

### 6.4 Dynamic hooks

Live systems emit treatment, exposure, hygiene, sump, and flooding events. The
generator attaches authored follow-ups without a new event bus. All exposure flows
through the canonical disease contract; the expansion never applies disease directly.

### 6.5 Constraints

- No water may be created from nothing; mass balance is a hard live rule.
- No exposure may skip `DiseaseSystem.TryExpose`.
- No hygiene state may be stored directly; it is derived in the live system.
- No waste may be destroyed without a routing or disposal path.
- No treaty may bypass `FactionStanceEngine`.
- No content may trivialize water scarcity or make hygiene free.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `WaterSourceSystem` (new, `Ashfall.Core`)

**Owns:** authored sources, yield curves, seasonal availability, quality bands, and
ownership/claims. **Consumes:** `WaterTreatmentSystem` inputs, `WeatherSystem`,
`BrineWaterSystem`, `SumpFloodingSystem`. **Data:** `water_sources.json`.
**Rules:** sources are finite per day and vary by weather and season; the system
never invents water; pretreatment is required for most sources.

### 7.2 `WaterQualitySystem` (new, `Ashfall.Core`)

**Owns:** quality profiles, contamination classes, test kits, sample backlog, and
declared results. **Consumes:** `WaterTreatmentSystem` contamination profile,
`WaterborneExposureRules`, `DiseaseSystem`. **Data:** `water_quality_profiles.json`.
**Rules:** testing takes time; results can be right, delayed, or wrong; the shelter
may drink before results; declared results change policy and trust.

### 7.3 `HygienePracticeSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** authored practices (handwashing, bathing, laundry, cooking hygiene,
wound hygiene), compliance, education, and culture. **Consumes:**
`SanitationSystem` derived hygiene band, `NeedsSystem`, `MoraleContagionSystem`.
**Data:** `hygiene_practices.json`.
**Rules:** practices consume water, soap, fuel, and time; compliance is social, not
automatic; education raises compliance; order of magnitude matters more than
individual practice.

### 7.4 `WasteRoutingSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** routing of the three live waste streams to reuse, compost, storage,
treatment, or disposal. **Consumes:** `SanitationSystem`, `Inventory`,
`PowerGridSystem`, `Farming`. **Data:** `waste_streams.json`.
**Rules:** every route has a capacity, a cost, and a byproduct; misrouting causes
contamination or hygiene loss; nothing vanishes.

### 7.5 `DrainageNetworkSystem` (extend `SumpFloodingSystem`)

**Owns:** authored drainage nodes, greywater routing, groundwater response, and
dewatering plant operation. **Consumes:** `SumpFloodingSystem`, `FogHarvestingCatalog`,
`WeatherSystem`, `PowerGridSystem`. **Data:** `drainage_nodes.json`.
**Rules:** water flows downhill by authored topology; flooding follows the live
groundwater model; dewatering consumes power and produces cake and tailings.

### 7.6 `WaterCommonsSystem` (new, `Ashfall.Core.Factions`)

**Owns:** regional water sharing, claims, treaties, and embargo. **Consumes:**
`FactionStanceEngine`, `DiplomaticTreaties`, `WaterSourceSystem`. **Data:**
`water_treaties.json`.
**Rules:** sharing moves water through the live chain; a cut is a standing and needs
event; no second water network is created.

### 7.7 `PublicHealthSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** authored measures, inspections, compliance, notices, and readiness.
**Consumes:** `WaterQualitySystem`, `HygienePracticeSystem`, `DiseaseSystem`,
`SanitationSystem`. **Data:** `public_health_measures.json`.
**Rules:** measures reduce risk through the live disease contract; they cost time and
compliance; inspections find real conditions, never invented ones.

### 7.8 Systems explicitly not added

- No second water, sanitation, drainage, disease, or save system.
- No direct stat writes; all exposure and hydration route through canonical owners.
- No new RNG stream in the water path (the live exposure rules are RNG-free).
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `water_sources.json` (new)

```json
{
  "schema_version": 1,
  "sources": [
    {
      "source_id": "source_fog_fence_a",
      "display_name": "West Fog Fence",
      "source_class": "atmospheric",
      "yield_liters_per_day": 120,
      "season_modifier": { "spring": 0.5, "summer": 0.2, "autumn": 1.2, "winter": 0.8 },
      "raw_quality_band": "marginal",
      "pretreatment_required": true,
      "owner_faction_id": "self",
      "location_id": "loc_fog_fence",
      "tags": ["fog", "seasonal", "free"]
    }
  ]
}
```

### 8.2 `water_quality_profiles.json` (new)

Profile rows: profile id, contaminants, thresholds, test difficulty, treatment
response, and declared-result effects.

### 8.3 `sanitation_facilities.json` (extend)

Existing schema preserved; new facility rows across latrines, bathing, laundry,
wash points, and cleaning stations with water/soap/labor costs and hygiene effects.

### 8.4 `hygiene_practices.json` (new)

Practice rows: name, water cost, soap cost, fuel cost, time cost, compliance base,
education effect, disease-risk reduction, and culture tags.

### 8.5 `waste_streams.json` (new)

Stream rows: type, sources, routing options, capacity, byproducts, contamination
risk, and disposal requirements.

### 8.6 `drainage_nodes.json` (new)

Node rows: id, type, inlet/outlet, capacity, condition, blockage rate, overflow
destination, and repair recipe.

### 8.7 `public_health_measures.json` (new)

Measure rows: name, cost, coverage, compliance requirement, risk reduction, and
inspection rules.

### 8.8 `water_treaties.json` (new)

Treaty rows: partner, volume, quality, terms, toll, embargo conditions, and breach
consequences.

### 8.9 `fluid_infrastructure.json` (extend)

New routes, tanks, pumps, and valves tying sources to treatment to storage to
consumers to drainage.

### 8.10 `sump_drainage_catalog.json` (extend)

New sump nodes, groundwater profiles, and cake/tailings assay profiles.

### 8.11 Items

New items appended to `items.json`: `item_water_sample_kit`, `item_test_reagent`,
`item_filter_cartridge`, `item_charcoal_sack`, `item_lime_sack`,
`item_soap_bar`, `item_laundry_soda`, `item_latrine_screen`,
`item_compost_turner`, `item_tailings_cask`, `item_fog_mesh`,
`item_slow_sand_media`, `item_water_ration_tin`, `item_chlorine_bleach`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing stores: `WaterTreatmentSaveStore`, `SanitationSaveStore`,
`SumpFloodingSaveStore`, brine and chlor-alkali stores. New sub-objects are additive
inside these envelopes. No new save section.

### 9.2 State to persist

- Source yields, seasonal state, and claims.
- Quality test backlog and declared results.
- Hygiene practices and compliance.
- Waste routing and capacities.
- Drainage node condition and groundwater.
- Water treaties and embargo state.
- Public-health measures and inspection history.

### 9.3 Determinism

- The live waterborne exposure rules are RNG-free; the expansion preserves that.
- Test result errors, if any, are deterministic authored gaps, not random noise.
- Source yields are pure functions of weather and season.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no sources, no tests, no practices, no routing, no treaties,
and neutral measures. Existing water, sanitation, and sump state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for contamination and compliance.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `WaterTreatmentPanel` (extend) | Modes, inventories, filter, jobs, quality | `CleanFlowHostSession` |
| `WaterSourcePanel` (new) | Sources, yields, seasons, claims | same |
| `WaterQualityPanel` (new) | Tests, backlog, declared results | same |
| `SanitationPanel` (extend) | Facilities, waste, hygiene band | same |
| `HygienePanel` (new) | Practices, compliance, education | same |
| `WasteRoutingPanel` (new) | Streams, routes, reuse, disposal | same |
| `DrainagePanel` (new) | Nodes, groundwater, dewatering | same |
| `PublicHealthPanel` (new) | Measures, inspections, notices | same |
| `WaterTreatyPanel` (new) | Sharing, terms, embargo | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Quality is shown as what is known, not as hidden truth; uncertainty is explicit.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Hygiene and exposure risks are stated plainly; no silent deaths.
- Rationing decisions are logged and visible.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: running water, a pump cycling, a filter
seal, a latrine door, laundry, a drain rod, a test vial. No cue is required; text
carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `WaterTreatmentSystem` | Inventories, modes, jobs, quality extended |
| `BrineWaterSystem` | External source and treaty adapter |
| `SanitationSystem` | Waste and hygiene extended |
| `WaterborneExposureRules` | Exposure mapping consumed |
| `SumpFloodingSystem` | Nodes, groundwater, dewatering extended |
| `ChlorAlkaliSynthesisEngine` | Bleach and treatment chemistry |
| `FogHarvestingCatalog` | Harvest arrays extended |
| `DiseaseSystem` | Sole exposure and infection authority |
| `NeedsSystem` | Thirst and hydration |
| `MoraleContagionSystem` | Queue, rationing, and hygiene morale |
| `FactionStanceEngine` | Water treaties |
| `PowerGridSystem` | Pumps, stills, dewatering |
| `Farming` / `GreenhouseSystem` | Greywater and compost reuse |
| `Inventory` | Soap, filters, reagents, media |
| `MemorialSystem` | Preventable deaths |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `WaterTreatmentSystem`, `BrineWaterSystem`,
`SanitationSystem`, `WaterborneExposureRules`, `SumpFloodingSystem`,
`ChlorAlkaliSynthesisEngine`, save stores, and panels. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author sources, quality profiles, hygiene practices,
waste streams, drainage nodes, public-health measures, water treaties; extend
sanitation facilities, fluid infrastructure, and sump catalog. Register validators
and scanner.

**Phase 2 — Pure Core.** `WaterSourceSystem`, `WaterQualitySystem`,
`HygienePracticeSystem`, `WasteRoutingSystem`, `DrainageNetworkSystem`,
`WaterCommonsSystem`, `PublicHealthSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `CleanFlowHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 90/180-day soak including heat waves, outages, and outbreaks.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Water sources | 20 |
| Quality profiles | 15 |
| Sanitation facilities | 20 |
| Hygiene practices | 18 |
| Waste streams/routes | 15 |
| Drainage nodes | 24 |
| Public-health measures | 12 |
| Water treaties | 8 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second water system | Critical | Extend live owners only |
| Exposure bypasses disease | Critical | Route through `TryExpose` |
| Hygiene modeled as a stored stat | High | Derived only |
| Water created from nothing | Critical | Mass balance is live and hard |
| Gross-out tone | Medium | Practical, dignified prose |
| Outbreak unfair | Higher | Warnings, testing, response drills |
| Determinism break | Low | RNG-free water path preserved |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `water_sources.json` | 20 | 5,000 |
| `water_quality_profiles.json` | 15 | 3,000 |
| `sanitation_facilities.json` | 20 | 4,000 |
| `hygiene_practices.json` | 18 | 4,000 |
| `waste_streams.json` | 15 | 3,500 |
| `drainage_nodes.json` | 24 | 4,000 |
| `public_health_measures.json` | 12 | 2,500 |
| `water_treaties.json` | 8 | 2,000 |
| `fluid_infrastructure.json` | +20 | 3,000 |
| `sump_drainage_catalog.json` | +15 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~64,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R22-1 | Second water system | Low | Critical | Extend live owners |
| R22-2 | Exposure bypass | Low | Critical | `TryExpose` only |
| R22-3 | Stored hygiene stat | Med | High | Derived band only |
| R22-4 | Water from nothing | Low | Critical | Live mass balance |
| R22-5 | Gross-out tone | Med | Med | Dignified prose |
| R22-6 | Outbreak unfair | Med | High | Warnings and drills |
| R22-7 | Determinism | Low | High | RNG-free path |
| R22-8 | Content overrun | Med | Med | Budget §13 |
| R22-9 | Queue politics opaque | Med | Med | Explicit ration UI |
| R22-10 | Test uncertainty abused | Med | Med | Authored error bands |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can the shelter drink before test results?** Recommended: yes, with explicit
   risk and a real chance of exposure.
2. **Are test errors random or authored?** Recommended: authored gaps and delays,
   never random lies.
3. **Does greywater reuse need treatment?** Recommended: yes, by crop type and route.
4. **Can water treaties include quality guarantees?** Recommended: yes; a partner
   sending marginal water is a standing event.
5. **Is bathing compulsory?** Recommended: no; compliance is social and educational.

---

## 17. APPENDIX D — WATER SOURCE TABLE (20 SOURCES)

| # | Source | Class | Yield L/day | Quality | Pretreat | Owner | Season |
|---|---|---|---|---|---|---|---|
| 1 | Rain Cistern A | atmospheric | 90 | marginal | yes | self | wet |
| 2 | Rain Cistern B | atmospheric | 140 | marginal | yes | self | wet |
| 3 | West Fog Fence | atmospheric | 120 | marginal | yes | self | autumn |
| 4 | East Fog Fence | atmospheric | 80 | marginal | yes | self | autumn |
| 5 | Spring Mouth | groundwater | 200 | clean | no | contested | steady |
| 6 | Deep Well | groundwater | 160 | good | minimal | self | steady |
| 7 | Artesian Bore | groundwater | 240 | good | minimal | self | steady |
| 8 | Sump Seep | groundwater | 60 | unsafe | heavy | self | rising |
| 9 | Brine Intake | external | 300 | brackish | yes | treaty | steady |
| 10 | River Draw | surface | 350 | marginal | yes | contested | variable |
| 11 | Melt Ditch | surface | 180 | good | yes | self | spring |
| 12 | Snow Store | surface | 500 | good | yes | self | winter |
| 13 | Old Baths Tank | salvage | 80 | stale | yes | self | finite |
| 14 | Metro Sump | salvage | 120 | unsafe | heavy | self | steady |
| 15 | Condenser Array | atmospheric | 70 | good | no | self | dry |
| 16 | Trade Convoy | external | 100 | clean | no | paid | any |
| 17 | Hospital Reservoir | salvage | 90 | marginal | yes | contested | finite |
| 18 | Fog Ridge Camp | atmospheric | 60 | marginal | yes | neighbor | autumn |
| 19 | Glacial Seep | groundwater | 110 | good | minimal | contested | summer |
| 20 | Waste Reclaim | reuse | 40 | unsafe | heavy | self | steady |

No source is unlimited, and every source has a season and an owner. The shelter's
water security is the sum of a dozen fragile lines, which is exactly the point.

---

## 18. APPENDIX E — WATER QUALITY PROFILE TABLE (15 PROFILES)

| # | Profile | Contaminants | Threshold | Test days | Treatment response |
|---|---|---|---|---|---|
| 1 | Clean | none | 0 | 1 | none needed |
| 2 | Dusty | particles | low | 1 | filtration |
| 3 | Organic Load | bacteria | med | 2 | boil or chlorine |
| 4 | Fecal Load | coli class | high | 2 | chlorine or boil |
| 5 | Typhoid Class | pathogen | high | 4 | chlorine or boil |
| 6 | Dysentery Class | pathogen | high | 3 | chlorine or boil |
| 7 | Nitrate High | chemical | med | 3 | distillation |
| 8 | Heavy Metal | chemical | high | 4 | adsorption |
| 9 | Saline | brackish | med | 2 | distillation or RO |
| 10 | Irradiated | radionuclide | high | 2 | distillation |
| 11 | Acidic | pH | low | 1 | neutralization |
| 12 | Alkaline | pH | low | 1 | neutralization |
| 13 | Solvent | chemical | med | 3 | adsorption |
| 14 | Mixed Industrial | chemical | high | 5 | staged treatment |
| 15 | Unknown | unknown | unknown | 4 | cautious response |

Test time is the expansion's core uncertainty. Four days is a long time to be
thirsty, and the shelter must decide what it drinks while it waits.

---

## 19. APPENDIX F — SANITATION FACILITY TABLE (20 FACILITIES)

| # | Facility | Water/day | Soap/day | Labor | Hygiene effect | Waste |
|---|---|---|---|---|---|---|
| 1 | Simple Latrine | 0 | 0 | low | moderate | organic |
| 2 | Sealed Latrine | 5 | 0 | low | high | organic |
| 3 | Composting Toilet | 0 | 0 | med | high | compost |
| 4 | Wash Point | 20 | 0.5 | low | high | greywater |
| 5 | Hand Basin | 8 | 0.3 | low | very high | greywater |
| 6 | Bath House | 60 | 2 | med | very high | greywater |
| 7 | Shower Stall | 30 | 1 | low | high | greywater |
| 8 | Laundry Trough | 80 | 3 | med | high | greywater |
| 9 | Laundry Mangle | 60 | 2 | med | high | greywater |
| 10 | Cleaning Station | 15 | 1 | med | high | chemical |
| 11 | Sick Room Point | 25 | 1 | high | very high | chemical |
| 12 | Kitchen Wash | 40 | 2 | med | very high | organic |
| 13 | Wound Station | 10 | 0.5 | high | high | chemical |
| 14 | Nappy Point | 20 | 1 | med | high | organic |
| 15 | Waste Sink | 15 | 0.5 | low | moderate | chemical |
| 16 | Boot Wash | 10 | 0.2 | low | moderate | chemical |
| 17 | Instrument Soak | 20 | 0 | high | very high | chemical |
| 18 | Lice Bath | 40 | 1 | high | high | chemical |
| 19 | Quarantine Point | 30 | 1 | high | very high | chemical |
| 20 | Field Wash | 5 | 0.2 | low | low | greywater |

Every facility costs water, soap, and hours. A shelter that bathes is a shelter that
does not drill, and that trade is the expansion's daily arithmetic.

---

## 20. APPENDIX G — HYGIENE PRACTICE TABLE (18 PRACTICES)

| # | Practice | Water | Soap | Fuel | Compliance | Risk reduction |
|---|---|---|---|---|---|---|
| 1 | Handwash after latrine | 2 | 0.05 | 0 | high | very high |
| 2 | Handwash before cooking | 2 | 0.05 | 0 | high | high |
| 3 | Handwash before care | 2 | 0.05 | 0 | med | very high |
| 4 | Daily face wash | 3 | 0.02 | 0 | med | moderate |
| 5 | Weekly bath | 20 | 0.3 | 2 | med | high |
| 6 | Laundry weekly | 40 | 0.5 | 0 | med | high |
| 7 | Bedding airing | 0 | 0 | 0 | high | moderate |
| 8 | Cooking surface wash | 5 | 0.1 | 0 | high | high |
| 9 | Vessel boil | 10 | 0 | 4 | high | high |
| 10 | Wound care | 3 | 0.05 | 0 | high | very high |
| 11 | Nappy change | 5 | 0.1 | 0 | high | very high |
| 12 | Corpse handling | 8 | 0.2 | 0 | high | very high |
| 13 | Waste handling | 5 | 0.1 | 0 | high | high |
| 14 | Latrine cleaning | 10 | 0.3 | 0 | med | high |
| 15 | Fly control | 0 | 0 | 1 | med | high |
| 16 | Rodent control | 0 | 0 | 0 | med | moderate |
| 17 | Quarantine wash | 15 | 0.4 | 0 | high | very high |
| 18 | Water container clean | 8 | 0.1 | 0 | med | high |

Compliance is social, not automatic. Education, leadership, and morale raise it;
fatigue, grief, and scarcity lower it. A shelter can know all of this and still
fail at practice 1, which is how outbreaks actually start.

---

## 21. APPENDIX H — WASTE STREAM TABLE (15 ROUTES)

| # | Stream | Source | Route | Capacity | Byproduct | Risk |
|---|---|---|---|---|---|---|
| 1 | Organic | kitchen | compost | 50/day | humus | low |
| 2 | Organic | latrine | compost | 40/day | humus | med |
| 3 | Organic | garden | compost | 30/day | humus | low |
| 4 | Organic | butcher | rendering | 20/day | tallow | med |
| 5 | Organic | corpses | burial | 5/day | none | high |
| 6 | Chemical | clinic | incineration | 10/day | ash | med |
| 7 | Chemical | lab | neutralization | 8/day | salt | med |
| 8 | Chemical | workshop | storage | 12/day | none | high |
| 9 | Chemical | laundry | dilution | 20/day | greywater | med |
| 10 | Chemical | battery | casking | 5/day | acid | very high |
| 11 | Radioactive | clinic | casking | 3/day | none | very high |
| 12 | Radioactive | filters | casking | 4/day | none | very high |
| 13 | Radioactive | clothing | burial | 2/day | none | very high |
| 14 | Greywater | baths | reuse | 100/day | irrigation | med |
| 15 | Greywater | laundry | filtration | 80/day | sludge | med |

Nothing vanishes. Every route has a capacity, a byproduct, and a failure mode, and
the shelter's waste problem is really a routing problem.

---

## 22. APPENDIX I — DRAINAGE NODE TABLE (24 NODES)

| # | Node | Type | Capacity L/h | Condition | Overflow to | Repair |
|---|---|---|---|---|---|---|
| 1 | Sump A | sump | 400 | good | pump B | service |
| 2 | Sump B | sump | 600 | fair | pump C | service |
| 3 | Sump C | sump | 300 | poor | field | rebuild |
| 4 | Drain Main 1 | drain | 500 | good | sump A | rod |
| 5 | Drain Main 2 | drain | 450 | fair | sump A | rod |
| 6 | Drain East | drain | 250 | poor | field | replace |
| 7 | Drain West | drain | 250 | fair | sump B | rod |
| 8 | Kitchen Line | drain | 120 | good | main 1 | clean |
| 9 | Bath Line | drain | 200 | good | main 2 | clean |
| 10 | Laundry Line | drain | 180 | fair | main 2 | clean |
| 11 | Clinic Line | drain | 80 | good | main 1 | clean |
| 12 | Yard Gully | gully | 150 | poor | field | clear |
| 13 | Latrine Pit | soakaway | 60 | fair | ground | lime |
| 14 | Compost Drain | drain | 40 | good | gully | clean |
| 15 | Fuel Yard Drain | drain | 30 | poor | sump C | rebuild |
| 16 | Deep Level Sump | sump | 500 | fair | pump A | service |
| 17 | Metro Sump | sump | 700 | poor | pump B | rebuild |
| 18 | Pump A | pump | 800 | good | tank | service |
| 19 | Pump B | pump | 900 | fair | outfall | service |
| 20 | Pump C | pump | 400 | poor | field | rebuild |
| 21 | Storage Tank | tank | 2000 | good | treatment | clean |
| 22 | Outfall | outfall | 1200 | fair | river | dredge |
| 23 | Ground Pit | soakaway | 100 | fair | ground | lime |
| 24 | Emergency Sump | sump | 300 | good | pump A | service |

Drainage is the expansion's least glamorous and most load-bearing system. A blocked
main is a slow flood; a broken pump is a rising water table; a rising water table is
a poisoned well.

---

## 23. APPENDIX J — PUBLIC HEALTH MEASURE TABLE (12 MEASURES)

| # | Measure | Cost | Coverage | Compliance | Risk reduction |
|---|---|---|---|---|---|
| 1 | Handwash stations | build | kitchens, latrines | med | very high |
| 2 | Water boiling rule | fuel | all drinking | high | high |
| 3 | Chlorination program | bleach | all drinking | high | very high |
| 4 | Latrine inspection | labor | all facilities | med | high |
| 5 | Bath rotation | water | all residents | med | high |
| 6 | Laundry schedule | water, soap | all bedding | med | high |
| 7 | Food handling rules | labor | kitchen | high | very high |
| 8 | Waste separation | labor | all streams | high | high |
| 9 | Fly control | labor, lime | latrines, waste | med | high |
| 10 | Rodent control | traps | stores, drains | med | moderate |
| 11 | Sick-room isolation | room | symptomatic | high | very high |
| 12 | Outbreak drill | time | all staff | high | high |

Measures cost time and obedience, not just materials. A shelter can have every rule
and enforce none of them, which is exactly the failure the expansion dramatizes.

---

## 24. APPENDIX K — WATER TREATY TABLE (8 TREATIES)

| # | Partner | Volume L/day | Quality | Toll | Embargo risk | Breach |
|---|---|---|---|---|---|---|
| 1 | Fog Ridge Camp | 200 | marginal | 10% | med | warning |
| 2 | Spring Village | 300 | clean | 15% | low | standing |
| 3 | River Flotilla | 400 | marginal | 20% | high | standing |
| 4 | Foundry Enclave | 150 | good | trade | low | negotiation |
| 5 | Deep Bunker | 250 | good | reciprocal | low | review |
| 6 | Market Town | 120 | clean | paid | med | warning |
| 7 | Garrison | 500 | good | imposed | very high | crisis |
| 8 | Waste Reclaim Co-op | 80 | marginal | labor | med | warning |

Water treaties are quieter than grid interties and just as political. A partner who
sends marginal water is telling the shelter something, and the shelter has to decide
whether to test, treat, or trust.

---

## 25. APPENDIX L — TREATMENT MODE TABLE

The live system supports modes; the expansion gives them authored costs and outputs:

| Mode | Input | Filter use | Charcoal | Fuel | Output quality | Speed |
|---|---|---|---|---|---|---|
| Settle | any | none | none | none | marginal | fast |
| Filter | raw | high | low | none | good | med |
| Boil | any | none | none | high | good | med |
| Chlorinate | clear | low | none | none | good | fast |
| Distill | any | low | low | very high | clean | slow |
| Adsorb | chemical | med | high | low | good | slow |
| Neutralize | acidic | low | low | none | marginal | fast |
| Staged | mixed | high | high | high | clean | very slow |

Every mode trades speed, consumables, and output. The player does not choose the
"best" mode; they choose the mode that fits today's water, today's stock, and
today's thirst.

---

## 26. APPENDIX M — CONTAMINATION AND EXPOSURE CHAIN

```
   Source quality
      │
      ▼
   Treatment mode ──► output contamination profile
      │                       │
      ▼                       ▼
   Storage & handling ──► recontamination risk (containers, hands, flies)
      │                       │
      ▼                       ▼
   Consumption ─► WaterborneExposureRules ──► DiseaseSystem.TryExpose
      │                       │
      ▼                       ▼
   Symptoms ──► DiseaseTriage ──► care or comfort
      │
      ▼
   Waste ──► sanitation ──► flies/wells ──► more exposure
```

The chain is a loop. That is the expansion's whole lesson: an outbreak is not an
event, it is a cycle that the shelter either breaks or feeds.

---

## 27. APPENDIX N — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_flow_the_heat` | 3 | Water runs out; the filter is past its life |
| `quest_flow_shortcuts` | 4 | Debate the seep; cost the risk |
| `quest_flow_first_cases` | 4 | Triage the first cases; start tests |
| `quest_flow_the_queue` | 4 | Ration clean water; manage the line |
| `quest_flow_four_days` | 5 | Wait for results; decide what to drink |
| `quest_flow_latrines` | 4 | Inspect slopes; relocate or lime |
| `quest_flow_bath_house` | 3 | Close, ration, or keep the baths |
| `quest_flow_slow_sand` | 5 | Salvage, build, seed, and run a slow-sand bed |
| `quest_flow_greywater` | 4 | Route wash water to crops safely |
| `quest_flow_treaty` | 5 | Negotiate volume, quality, and terms |
| `quest_flow_outbreak` | 6 | Respond to results; treat, isolate, trace |
| `quest_flow_compost` | 4 | Close the organic loop without contamination |
| `quest_flow_the_sump` | 5 | Contain the rising groundwater |
| `quest_flow_reckoning` | 5 | Count cases, casualties, trust, and cost |
| `quest_flow_clean_flow` | 3 | Final disposition; epilogue |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Nessa Ord** — water keeper. Runs the chain with a bookkeeper's precision and a
survivor's willingness to cut corners. She is not careless; she is exhausted by a
job where every litre is a negotiation. Her arc is learning to say no to the seep.

**Dr. Asa Vell** — physician. Tracks the cases and refuses the shortcuts, because
she is the one who will sit with the children at night. Her authority is clinical,
not moral, and she resents being cast as the shelter's conscience.

**Berek Halm** — sanitation chief. Builds latrines, routes waste, and treats dignity
as an engineering requirement. Believes a shelter is judged by how it handles what
it would rather not discuss.

**Lir Venn** — laundry mistress. Runs the shelter's largest water consumer and
knows it. Fights for the wash because dirty bedding is a disease vector, and no one
thanks her for it.

**Tam Orr** — public health clerk. Tests water, writes notices, and tracks
compliance. Young, methodical, and quietly furious about how often warnings are
ignored until someone dies.

**Bram Koole** — drain boss. Owns the sump, the pumps, and the groundwater. Speaks in
capacities and overflow routes. The expansion's reminder that infrastructure is a
person's job.

**Vene Hask** — neighbor envoy. Offers water with honest terms and a hidden clause.
Represents the possibility of shared supply and the risk of dependence.

**Pim** — child of the queue. Carries the family's water ration twice a day and
knows exactly how heavy a litre is. The human measure of the entire expansion.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Spring Mouth** — clean water rising from rock; a queue and a claim marker.
- **The Fog Fence** — mesh panels on a ridge; water in the morning, nothing by noon.
- **The Cistern Field** — buried tanks with lids; half of them leak.
- **The Deep Well** — a hand pump, a bucket, and a testing kit.
- **The Sunken Latrines** — numbered pits, lime, and a slight downhill slope that
  matters more than anything else here.
- **The Sorting Yard** — three bins, gloves, and a schedule nobody likes.
- **The Outfall** — greywater leaving the shelter; the river tests downstream.
- **The Old Baths** — pre-war tiling, a slow-sand bed, and a design worth copying.
- **The Slow Sand Beds** — sand, gravel, and a biological layer that must be fed.
- **The Tower** — pressure and distribution; the shelter's water reaches every
  floor from here.

---

## 30. APPENDIX Q — WORKED 180-DAY WATER SCENARIO

**Days 1–15.** A heat wave halves the cisterns and the fog fences. Clean water runs
out on day 3. Nessa proposes the seep; Asa refuses. The queue begins.

**Days 16–30.** Two children and a worker fall ill. Tests take four days. The
shelter rations clean water and drinks boiled marginal water in the meantime. The
bath house closes; hygiene compliance drops; laundry backs up.

**Days 31–50.** Results confirm a fecal-oral class. Berek finds a latrine slope
feeding the well. Relocation and lime stop the source. The outbreak is contained at
nine cases and one death.

**Days 51–80.** The old baths yield a slow-sand design. The shelter builds two beds
and doubles treatment capacity. The treaty negotiation begins with Fog Ridge Camp.

**Days 81–110.** Greywater reuse saves the greenhouse. Compost closes the organic
loop. The deep well is tested and found clean, restoring some independence.

**Days 111–140.** The groundwater rises after heavy rain; the sump plant runs
continuously. A pump fails; the emergency sump holds. Tailings are casked.

**Days 141–180.** The water treaty is signed or refused. Public-health measures are
made standing. The charter is written: who drinks clean first, and why. The epilogue
records the queue.

---

## 31. APPENDIX R — VIGNETTE (TONE SAMPLE)

> The test tray sits on the bench for four days, and for four days the shelter
> drinks what it has and does not ask. Nessa counts the clean water in the morning
> and again at night, and the number only goes one direction.
>
> At the bath house door, Lir has chalked a schedule. Someone has rubbed out two
> names and written their own in. She does not fix it; she writes it down, because
> the list is a health record whether people like it or not.
>
> Down in the pit, Berek stands at the top of the slope with a level and a string,
> and the bubble shows a fall of one degree toward the well, and he says nothing for
> a while. One degree is all it takes.

This sets the register: quiet, procedural, and specific. All Clean Flow prose
should be written at this temperature.

---

## 32. APPENDIX S — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Filter exhaustion | quality drops | change, source clean |
| Charcoal shortage | treatment slows | make, salvage, trade |
| Source failure | volume drops | alternate source, ration |
| Latrine contamination | exposure risk | relocate, lime, cap |
| Hygiene collapse | disease risk | education, enforcement, water |
| Waste misroute | contamination | re-sort, clean, discipline |
| Drain blockage | local flooding | rod, pump, repair |
| Groundwater rise | flooding, well risk | dewater, seal, drain |
| Outbreak | illness, deaths | treat, isolate, trace |
| Treaty cut | supply loss | reserve, alternate, negotiate |
| Test error | wrong policy | retest, disclose, repair trust |

No failure is a game over. Every failure has a recovery path, and every recovery
costs water, materials, time, or trust. The deepest failure is a shelter that knows
what to do and stops doing it.

---

## 33. APPENDIX T — CAMPAIGN ARC TIMELINE

| Phase | Days | Theme | Decision |
|---|---|---|---|
| Heat | 1–30 | scarcity | what to drink |
| Cases | 31–60 | outbreak | response and testing |
| Latrines | 61–90 | source control | relocation or lime |
| Capacity | 91–120 | infrastructure | slow sand and reuse |
| Treaty | 121–150 | diplomacy | share or refuse |
| Standing | 151–180 | habits | codify public health |
| Reckoning | 181–240 | permanence | chain ownership |

Each phase changes the shelter's relationship to water, waste, and its own habits.

---

## 34. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] `WaterTreatmentSystem` remains the water authority.
- [ ] No water is created from nothing.
- [ ] All exposure routes through `DiseaseSystem.TryExpose`.
- [ ] Hygiene remains a derived band, never a stored stat.
- [ ] Waste never vanishes without a route.
- [ ] Drainage follows authored topology.
- [ ] Tests take authored time and can be wrong only in authored ways.
- [ ] Water treaties go through `FactionStanceEngine`.
- [ ] Prose is practical and dignified, never gross-out.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism preserves the RNG-free water path.

---

## 35. APPENDIX V — GLOSSARY

- **Source** — an authored water input with yield, quality, and owner.
- **Pretreatment** — required handling before treatment modes.
- **Quality profile** — authored contamination class with thresholds and test time.
- **Backlog** — untested samples; the shelter acts without knowledge.
- **Hygiene band** — the live derived cleanliness state.
- **Practice** — an authored behavior with cost and compliance.
- **Stream** — one of the three waste classes.
- **Route** — a disposal, reuse, or treatment path for a stream.
- **Node** — a drainage point in the authored network.
- **Treaty** — a regional water-sharing agreement.

---

## 36. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `WaterTreatmentSystem` | inventories | water state | disease |
| `BrineWaterSystem` | brine | brine state | water inventory |
| `SanitationSystem` | waste | waste state | hygiene stored |
| `WaterborneExposureRules` | output dose | — | disease state |
| `SumpFloodingSystem` | nodes | groundwater | water inventory |
| `ChlorAlkaliSynthesisEngine` | chemistry | bleach | — |
| `DiseaseSystem` | exposure | infection | water state |
| `NeedsSystem` | thirst | hydration | — |
| `MoraleContagionSystem` | queue | contagion | — |
| `FactionStanceEngine` | treaty | standing | — |
| `PowerGridSystem` | pumps | — | — |
| `Farming` | greywater | crop state | — |
| `Inventory` | supplies | transfers | — |
| `MemorialSystem` | deaths | memorials | — |

---

## 37. APPENDIX X — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Clean litres per person per day | hydration pressure | WaterTreatmentSystem |
| Treatment backlog | testing load | WaterQualitySystem |
| Filter life remaining | consumable pressure | WaterTreatmentSystem |
| Hygiene band distribution | practice success | SanitationSystem |
| Waste routed vs. stored | routing health | WasteRoutingSystem |
| Drain blockages | infrastructure stress | DrainageNetworkSystem |
| Groundwater level | flooding risk | SumpFloodingSystem |
| Exposure events | outbreak risk | WaterborneExposureRules |
| Treaty volume | dependence | WaterCommonsSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the water loop is tense or merely
bookkeeping.

---

## 38. APPENDIX Y — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Mass balance holds under all new paths.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §34.
- [ ] Phase 7 soak shows heat waves, outages, and outbreak recovery.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel water, sanitation, drainage, or disease system exists.

---

## 39. APPENDIX Z — CLOSING VIGNETTE

> On the morning the results come back, Tam pins the notice by the mess hall door:
> BOIL ALL WATER UNTIL THE FILTER IS REPLACED. Nine words. Three people read it
> before breakfast and one person reads it after, and the one who reads it after is
> the one who was already sick.
>
> Nessa replaces the filter that afternoon. It takes her two hours and uses the last
> clean water in the plant, and when she is done she stands with her hands on the
> housing and does not let go for a moment, because the housing is cold and the water
> behind it is the only reason anyone in this shelter is still alive.

---

## 41. APPENDIX AA — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_flow_fog_fence` | 4 | Repair mesh, re-tension, test yield |
| `quest_flow_rain_capture` | 4 | Clean gutters, seal tanks, connect |
| `quest_flow_deep_well` | 5 | Test, redevelop, pump-test, declare |
| `quest_flow_spring_claim` | 4 | Post a claim, negotiate, or share |
| `quest_flow_melt_yield` | 3 | Build a melt ditch before spring |
| `quest_flow_filter_change` | 3 | Isolate, swap media, flush, test |
| `quest_flow_charcoal_run` | 4 | Gather, char, quench, sack |
| `quest_flow_distillation_fuel` | 4 | Source fuel, budget still time |
| `quest_flow_mode_choice` | 3 | Match mode to water and stock |
| `quest_flow_slow_sand_build` | 5 | Build bed, add media, seed, commission |
| `quest_flow_test_kit` | 4 | Assemble reagents and trays |
| `quest_flow_backlog` | 4 | Work through untested samples |
| `quest_flow_quality_map` | 4 | Sample every source; declare bands |
| `quest_flow_false_clean` | 4 | Discover a bad result; disclose or hide |
| `quest_flow_public_notice` | 3 | Write and post the results honestly |
| `quest_flow_latrine_build` | 4 | Site, dig, seal, screen |
| `quest_flow_bath_ration` | 3 | Set the bathing water ration |
| `quest_flow_laundry_water` | 3 | Budget laundry water against bathing |
| `quest_flow_soap_run` | 4 | Make or salvage soap |
| `quest_flow_hand_practice` | 3 | Establish handwashing at points of use |
| `quest_flow_sort_streams` | 4 | Build bins; train the shelter |
| `quest_flow_compost_batch` | 4 | Mix, turn, monitor, harvest humus |
| `quest_flow_tailings_cask` | 3 | Cask, label, and store tailings |
| `quest_flow_waste_reuse` | 4 | Prove a reuse route is safe |
| `quest_flow_disposal_site` | 4 | Find or build a safe burial site |
| `quest_flow_drain_rod` | 3 | Locate the block; rod and flush |
| `quest_flow_groundwater_watch` | 3 | Install gauges; read the table |
| `quest_flow_dewater_shift` | 4 | Run the plant through a wet week |
| `quest_flow_inspection` | 4 | Inspect facilities; write findings |
| `quest_flow_outbreak_drill` | 3 | Run the response and time it |

---

## 42. APPENDIX AB — DATA SCHEMA DETAIL (NEW CATALOGS)

**`water_sources.json`** — `source_id`, `display_name`, `source_class`,
`yield_liters_per_day`, `season_modifier`, `raw_quality_band`,
`pretreatment_required`, `owner_faction_id`, `location_id`, `tags`.

**`water_quality_profiles.json`** — `profile_id`, `display_name`, `contaminants[]`,
`threshold`, `test_days`, `treatment_response`, `declared_effect`, `tags`.

**`hygiene_practices.json`** — `practice_id`, `display_name`, `water_cost`,
`soap_cost`, `fuel_cost`, `time_cost`, `compliance_base`,
`education_effect`, `risk_reduction`, `culture_tags[]`, `tags`.

**`waste_streams.json`** — `stream_id`, `display_name`, `waste_type`,
`source_tags[]`, `route_options[]`, `capacity_per_day`, `byproduct`, `risk`,
`disposal_requirement`, `tags`.

**`drainage_nodes.json`** — `node_id`, `display_name`, `node_type`,
`inlet_ids[]`, `outlet_id`, `capacity_lph`, `condition`, `blockage_rate`,
`overflow_destination`, `repair_recipe[]`, `tags`.

**`public_health_measures.json`** — `measure_id`, `display_name`, `cost[]`,
`coverage_tags[]`, `compliance_requirement`, `risk_reduction`,
`inspection_rules`, `tags`.

**`water_treaties.json`** — `treaty_id`, `partner_faction_id`, `volume_liters`,
`quality_band`, `toll`, `embargo_conditions[]`, `breach_consequence`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid location/faction references, or out-of-range numbers.

---

## 43. APPENDIX AC — REGIONAL WATER MAP

| Settlement | Source | Quality | Vulnerability |
|---|---|---|---|
| The shelter | mixed | treated | filter and fuel |
| Fog Ridge Camp | fog | marginal | calm seasons |
| Spring Village | spring | clean | single source |
| River Flotilla | river | marginal | upstream contamination |
| Foundry Enclave | well | good | drawdown |
| Deep Bunker | aquifer | good | pumping power |
| Market Town | rain | marginal | dry spells |
| Garrison | river, well | good | fuel for pumps |

Every settlement is water-insecure in a different way, which is what makes water
treaties worth considering and dangerous to depend on.

---

## 44. APPENDIX AD — HYGIENE EDUCATION MODEL

Compliance is not a switch. The expansion models it as a social curve:

| Input | Effect on compliance |
|---|---|
| Education sessions | +base gain |
| Visible illness | +temporary spike |
| Leadership example | +steady gain |
| Fatigue and grief | −decay |
| Scarcity | −decay |
| Facilities present | +reachable gain |
| Reminders and notices | +maintenance |
| Comfort and dignity | +retention |

A shelter can raise compliance with education and facilities, and lose it to
exhaustion. The expansion never shames survivors; it models the conditions under
which good habits survive.

---

## 45. APPENDIX AE — WASTE REUSE MODEL

| Reuse | Input | Requirement | Output | Risk |
|---|---|---|---|---|
| Compost | organic | turn, moisture | humus | low |
| Rendering | organic | heat, fuel | tallow | med |
| Greywater crops | greywater | treatment by crop | irrigation | med |
| Greywater flush | greywater | filtering | latrine water | low |
| Ash reuse | chemical ash | screening | grit, lye | low |
| Metal reclaim | chemical | sorting | scrap | med |
| Battery acid | chemical | casking | none | high |
| Clothing burial | radioactive | site | none | high |

Reuse closes loops, and every loop has a quality requirement. The expansion rewards
shelters that sort early and punish shelters that mix streams late.

---

## 46. APPENDIX AF — WORKED WATER BALANCE

| Day | Source L | Treated L | Consumed L | Waste L | Ground |
|---|---|---|---|---|---|
| 1 | 520 | 400 | 390 | 260 | stable |
| 2 | 480 | 380 | 395 | 270 | −10 |
| 3 | 460 | 300 | 400 | 280 | −90 |
| 4 | 900 (rain) | 700 | 410 | 290 | +200 |
| 5 | 610 | 520 | 405 | 280 | +110 |
| 6 | 500 | 420 | 410 | 285 | +0 |
| 7 | 470 | 250 (filter out) | 415 | 290 | −165 |

This is the shape of a normal week: two good days, one crisis day, and a slow drift
that the player is always managing. The numbers are authored so that a shelter which
ignores treatment and drainage will eventually be caught by them.

---

## 47. APPENDIX AG — LORE: THE MUNICIPAL WORKS

The pre-war world treated water as a municipal service, and the fiction explains the
ruins:

- **The Cistern Field** was a municipal rain-capture site; its tanks are still
  buried, and two of the lids still seal.
- **The Old Baths** was a public bath house with a slow-sand bed that served a
  neighborhood. Its design survives on a tile wall.
- **The Outfall** carried treated effluent to the river; the river still runs
  downstream of whoever is reading this.
- **The Fog Fence** was a meteorological research array adopted by scavengers as a
  water source.
- **The Deep Well** was a municipal backup supply, capped before the Exchange and
  reopened by the shelter.

No real utility, agency, or program is referenced. The Municipal Works is fictional
and exists to explain why the wasteland's water infrastructure is partially
repairable.

---

## 48. APPENDIX AH — OPEN QUESTIONS FOR REVIEW

1. Should test errors be possible at all, or only delays?
2. Should hygiene compliance be per-survivor or per-room?
3. Should greywater reuse require crop-specific treatment?
4. Should water treaties be breakable without a war?
5. Should the shelter be able to sell treated water?
6. Should latrine siting be player-directed or authored?
7. Should public-health measures be permanent or require upkeep?
8. Should the slow-sand bed require continuous operation to stay alive?

None of these may be decided unilaterally; each changes the expansion's tone and
balance.

---

## 49. APPENDIX AI — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do the live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are the systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is uncertainty honest? | lifecycle + a11y tests |
| Balance | Does the chain feel tight? | soak results |
| Audit | Is the prose dignified? | review checklist |

The audit gate matters here: this expansion deals with illness, waste, and death, and
it must never be written for shock value.

---

## 50. CLOSING STATEMENT

ASHFALL already models water honestly: inventories, treatment modes, filters,
charcoal, fuel, contamination, mass balance, hygiene as a derived band, waste
streams, groundwater, sludge, and an exposure rule with no dice in it. What it lacks
is the world those systems operate in: sources with seasons and owners, tests that
take time, practices that cost water and hours, routes for three kinds of waste,
drains that clog, and a public-health response that is either ready or not. The
Clean Flow adds that world without adding a second water system and without a single
drop created from nothing. It adds a queue, a filter, a latrine uphill of a well,
and the quiet, preventable stakes of an ordinary day.

> Wave 3 note: this plan is one of five Wave 3 expansion bibles (22–26). Each is
> self-contained; none requires another to ship. The shared Wave 3 index lives at
> `docs/expansions/wave3/WAVE3_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence anchors
> used throughout: `WaterTreatmentSystem`, `BrineWaterSystem`, `SanitationSystem`,
> `WaterborneExposureRules`, `SumpFloodingSystem`, `ChlorAlkaliSynthesisEngine`,
> and the live `water_treatment`, `sanitation`, and `sump` save sections.

> Wave 3 note: this plan is one of five Wave 3 expansion bibles (22–26). Each is
> self-contained; none requires another to ship. The shared Wave 3 index lives at
> `docs/expansions/wave3/WAVE3_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible.