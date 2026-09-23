# ASHFALL — Expansion 15 Design Bible
# THE DEEP ROOT
### Wave 1 · Soil Reclamation, Orchards, Livestock, Veterinary Care, and Seed Heritage

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Farming`, `Ashfall.Core.Greenhouse`, `Ashfall.Core.Ecology`, `Ashfall.Core.World`
**Proposed host owner:** `DeepRootHostSession` (extends `AgricultureSaveStore` + `GreenhouseHostSession` + `CompanionSaveStore`)
**Existing save sections:** `agriculture`, `farming`, `greenhouse`, `aquaponics`, `aeroponics`, `fungi_cultivation`, `companion_animals`, `hunting`
**Existing CLI verbs:** `--agriculture-selftest`, `--greenhouse-selftest`, `--aquaponics-selftest`, `--companion-animals-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL's food layer is deep but narrow. `GreenhouseSystem` owns growth stages and
water; `AgricultureSystem` sits above it with plot strains, medium quality and
toxicity, pests, mutation outcomes, and compost; `ApicultureSystem` runs hives;
`FungiCultivationSystem` runs mycelium; `CompanionAnimalSystem` handles bonded
animals; `NutritionDiversitySystem` tracks dietary breadth. But `agriculture_items.json`
contains **two items**, and there is no open-ground farming, no orchard, no livestock
herd, no veterinary authority, no aquaculture pond, and no seed bank.

**The Deep Root** fills that gap. It takes the shelter out of the grow-room and back
onto the ground — poisoned, ash-covered, half-alive ground — and asks what it costs
to make land feed people again.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Indoor agriculture is a life-support system. It works because it is sealed, powered,
and controlled. It also cannot scale: every square metre of grow-bed costs lamps,
water, and filtration, and every kilogram of food it produces competes with the
shelter's power and water budgets.

The wasteland outside is a different proposition. It is free, vast, and poisonous.
Its topsoil is dead or radioactive or salted or gone. Its seasons are violent. Its
weeds are thriving. But a single reclaimed field can outproduce the entire grow-room
at a fraction of the input — if the shelter is willing to invest years and lives in
ground that may never come back.

**The Deep Root** is the expansion about that bet. It adds open-ground soil
reclamation, multi-year orchards, livestock herds, veterinary medicine, aquaculture
ponds, fiber crops, and the seed bank that makes all of it survivable across
generations. It is the slowest, most patient expansion in the game, and the one
that most changes what a long campaign can become.

### 1.2 What the player manages

1. **Soil.** `AgriPlotState.medium_quality` and `toxicity_permille` already exist for
   indoor plots. The expansion introduces **open-ground plots** with real chemistry:
   pH, salinity, organic matter, radionuclide load, and a fallow/rotation calendar.
2. **Water.** Open ground needs irrigation, and irrigation water carries contamination.
   The live `AgriWaterBand` (`Clean`/`Marginal`/`Contaminated`/`Unsafe`) already
   tags host-reported water quality; the expansion makes it consequential at scale.
3. **Time.** Orchards take campaign years. Livestock herds take generations. The
   player must plant for a harvest they may not survive to eat.
4. **Animals.** Feed, water, shelter, breeding, disease, and slaughter ethics.
5. **Seeds.** A seed bank that preserves heirloom lines against loss, blight, and
   radiation. `crop_strains` already carries `sterile_seed` as a mutation outcome —
   the bank is the answer to it.
6. **Fiber and fuel.** Hemp, flax, and oilseed are as important as food; textiles and
   lamp oil are survival inputs, not luxuries.

### 1.3 What it is not

- Not a farming minigame with hand-placed crops. Plots are managed, not tilled by cursor.
- Not a second growth system. `GreenhouseSystem` remains the single growth authority.
- Not a second needs or nutrition system. `NeedsSystem` and `NutritionDiversitySystem`
   stay owners.
- Not a pet simulator. Livestock are production animals with welfare and risk.
- Not a resource jackpot. The Deep Root is a long payoff, and it can fail.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` | Plot strain, medium quality/toxicity, pest, mutation, compost | `LIVE` |
| `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs` | Strain loading | `LIVE` |
| `Assets/Ashfall.Core/Farming/FungiCultivationSystem.cs` | Mycelium cultivation | `LIVE` |
| `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` | Dietary breadth | `LIVE` |
| `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | Single growth authority | `LIVE` |
| `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` | Bay definitions | `LIVE` |
| `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | Hives, honey, wax, swarming | `LIVE` |
| `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs` | Bonded animals | `LIVE` |
| `Assets/Ashfall.Core/Ecology/` | Wildlife/ecology | `LIVE` |
| `src/Host/AgricultureHostSession.cs`, `src/Host/GreenhouseHostSession.cs` | Host | `LIVE` |
| `src/Host/AgricultureSaveStore.cs`, `src/Host/CompanionSaveStore.cs` | Persistence | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries |
|---|---|
| `agriculture_items.json` | **2 items** (compost humus, pest treatment dust) |
| `crop_strains.json` | 12 strains |
| `hydroponic_crops.json` | ~10 crops |
| `greenhouse_items.json` | ~40 items (seeds, tools, treatments) |
| `aquaponics_system_catalog.json` | live system |
| `aeroponics_nutrient_catalog.json` | nutrients |
| `companion_animals.json` | **5 species** (ash hound, feral goat, cotton hare, rad-dog, iron crow) |
| `sump_drainage_catalog.json` | drainage |
| `underground_flora.json` | flora |

### 2.3 Confirmed gaps

- **GAP-15-1 — No open-ground agriculture.** All farming is indoor; `AgricultureSystem`
  plots are grow-beds, not fields. There is no soil chemistry, no land, no season on land.
- **GAP-15-2 — No orchard.** `crop_strains` are annuals; nothing models a tree that
  produces after three years.
- **GAP-15-3 — No livestock herd.** Companion animals are individual bonds; nothing
  models a breeding, feeding, producing herd.
- **GAP-15-4 — No veterinary authority.** Animal disease and injury have no owner.
- **GAP-15-5 — No aquaculture pond.** `aquaponics_system_catalog.json` exists but no
  fish species, pond, or harvest model.
- **GAP-15-6 — No seed bank.** `sterile_seed` exists as an outcome but no preservation,
  storage, or heirloom authority.
- **GAP-15-7 — Two agriculture items.** The material vocabulary is nearly absent.
- **GAP-15-8 — No open-ground locations.** No field, orchard, pasture, pond, nursery,
  or seed vault in `locations.json`.
- **GAP-15-9 — No fiber crops.** Textiles exist as items but not as a grown crop.

### 2.4 Non-duplication statement

This expansion will **not** add a second growth system (`GreenhouseSystem` owns
growth and water), a second nutrition system (`NutritionDiversitySystem`), a second
disease authority for humans (`DiseaseSystem`), a second companion bond model
(`CompanionAnimalSystem`), or a second harvest save authority. New systems own only
what is genuinely missing: land, trees, herds, ponds, and seeds.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Land is the long game.** Open ground pays off after months or years. It
must never compete with the greenhouse for immediate returns; it is for campaigns
that intend to last.

**Pillar 2 — Poison is patient.** Ash, salt, and radionuclides do not vanish. They
must be flushed, planted, bound, or buried, and they come back if neglected.

**Pillar 3 — Animals are partners, not props.** Livestock have needs, welfare, disease,
and death. A starving herd is a moral event, not a stock ticker.

**Pillar 4 — Seeds are inheritance.** The seed bank is the expansion's emotional core:
the shelter preserves lines it may never plant, for children it may never meet.

**Pillar 5 — Hunger is the clock.** Every gain must be weighed against the coming
winter. The player cannot farm their way out of the first year; they can farm their
way into the fifth.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| First sprout | Two leaves in grey soil, a hand over them | Sparkling growth VFX |
| Orchard | A row of sticks, a marker, patience | Lush fantasy orchard |
| Livestock | Feed, muck, births, butchering | Cute pet mascots |
| Vet work | Blood, splints, decisions to cull | Miracle healing |
| Seed bank | Cold, dry, labelled heirlooms | Treasure vault |
| Blight | A patch that must be burned | Monster spores |

### 3.3 Content limits

- No real-world livestock brand names or patented cultivars.
- No cartoon animal characters; animals are production and companionship, both real.
- Butchering is treated with gravity, not gore.
- No fantasy plants or impossible yields.

---

## 4. NEW WORLD REGIONS AND LOCATIONS

### 4.1 Interior rooms

- **`room_seed_bank`** — cold, dry, dark; labelled drawers of heirloom seed.
- **`room_vet_shed`** — animal surgery and isolation.
- **`room_feed_store`** — grain, silage, and fodder storage; pest risk.
- **`room_root_cellar_extended`** — long-term crop storage.
- **`room_compost_works`** — aerobic compost and soil mixing.
- **`room_nursery_trays`** — seed starting and grafting.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_ash_fields` | The Grey Fields | 3 | First open-ground reclamation |
| `loc_old_orchard` | The Dead Orchard | 4 | Ancient fruit trees; grafting stock |
| `loc_pasture_ruins` | The Ruined Pasture | 4 | Fencing, forage, herd release site |
| `loc_flooded_pond` | The Sunk Pond | 5 | Aquaculture and irrigation reservoir |
| `loc_seed_bank_annex` | The Seed Annex | 6 | Pre-war seed bank; heirloom recovery |
| `loc_greenhouse_farm` | The Glass Farm | 5 | Ruined commercial greenhouse; glazing salvage |
| `loc_salt_flat` | The Salt Flat | 4 | Salinity challenge; halophyte crops |
| `loc_fiber_fields` | The Retted Fields | 3 | Hemp/flax; fiber processing |
| `loc_grafting_stand` | The Grafting Stand | 3 | Trade in scion wood and rootstock |
| `loc_cattle_wreck` | The Stock Wreck | 5 | Frozen livestock transport; genetics |
| `loc_compost_yard` | The Steam Yard | 3 | Large-scale compost heat |
| `loc_herb_walk` | The Herb Walk | 3 | Medicinal and culinary herbs |

All locations require valid item references and scanner registration. Danger levels
are intentionally low relative to combat zones: the land's danger is time, not guns.

### 4.3 Land parcels

Open-ground agriculture is organized into **parcels** attached to a location. A
parcel is not a new map node; it is a plot cluster with soil state. Each exterior
location can host a small number of authored parcels.

---

## 5. MAIN STORYLINE — "THE GROUND THAT REMEMBERS"

### 5.1 Central conflict

The shelter's grow-room is at capacity. The administrator who kept it running, an
old agronomist named **Dessa Marr**, insists there is no future in the lamps — only
in the ground. She has a plan: reclaim the Grey Fields, weather two failed harvests,
and come out of the third with more food than the shelter has ever known.

Against her is the shelter's quartermaster, **Brann Ost**, who will not risk the
present for a future that may never arrive. He has watched too many expeditions die
for a promise.

Between them is the seed bank: a cache of heirloom lines that could feed the shelter
for generations, if the lines survive the blight that is already moving through the
greenhouse. The final decision is not "farm or not farm." It is **what the shelter is
willing to plant when it might not live to harvest.**

### 5.2 Theme (unspoken)

**The ground remembers what was done to it, and the only way to answer is to plant
again, knowing it may fail.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_agronomist_dessa_marr` | Dessa Marr | Agronomist | Believes in the ground; impatient with caution |
| `npc_quartermaster_brann_ost` | Brann Ost | Quartermaster | Guards the present; opponent of risk |
| `npc_herder_yla_brant` | Yla Brant | Herder | Knows animals better than people |
| `npc_vet_oscar_taal` | Oscar Taal | Veterinarian | Triage and cull decisions |
| `npc_seedkeeper_hana_vey` | Hana Vey | Seed keeper | Custodian of the heirloom drawers |
| `npc_breeder_ko_fen` | Ko Fen | Breeder | Genetics and herd resilience |
| `npc_forager_pell_ash` | Pell Ash | Forager | Bridges wild and cultivated |
| `npc_child_grower_tam` | Tam | Young grower | The next generation's stake in the land |

### 5.4 Story beats (14)

1. **At Capacity.** The grow-room cannot feed the shelter; Dessa proposes the fields.
2. **The Grey Fields.** First soil sampling; ash depth; a decision to try.
3. **The First Failure.** Weather or chemistry destroys the first planting.
4. **The Dead Orchard.** Grafting stock recovered; a three-year clock starts.
5. **The Seed Annex.** A pre-war seed bank; heirloom recovery; blight discovered.
6. **The Quarantine.** Blight in the greenhouse; burn or isolate.
7. **The Herd.** First livestock; feed pressure; a moral decision about slaughter.
8. **The Sunk Pond.** Aquaculture attempt; water quality crisis.
9. **The Vet's Choice.** A diseased animal, a cull, and a child who named it.
10. **The Second Failure.** Salinity or radionuclides return.
11. **The Fiber Harvest.** Textiles and oil; a non-food success.
12. **The Bank Opens.** Seed bank viability tested; lines preserved or lost.
13. **The Third Season.** The field's real first harvest.
14. **What We Leave.** The final disposition: fields, orchard, herd, bank.

### 5.5 Branching choices (7)

| Choice | Options | Axis |
|---|---|---|
| Risk the fields | yes / no / limited trial | present vs. future |
| Blight response | burn / isolate / treat | loss vs. risk |
| Livestock purpose | production / breeding / companionship | efficiency vs. welfare |
| Slaughter ethics | allow / limit / refuse | survival vs. sentiment |
| Seed bank | open / ration / seal | sharing vs. preservation |
| Pond vs. wells | aquaculture / irrigation | food vs. water |
| Final disposition | plant / fallow / abandon | legacy |

### 5.6 Endings (5 + fade)

1. **The Long Harvest** — the fields succeed; the shelter becomes a food power.
2. **The Orchard Kept** — trees matter more than the harvest; a generation's bet.
3. **The Grey Return** — the land cannot be reclaimed; the shelter returns indoors.
4. **The Empty Bank** — heirloom loss; generations of diversity gone.
5. **The Herd Walked** — animals released; the shelter chooses ecology over control.
6. **Fade** — the fields are left to the ash.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_root_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (14)

`quest_root_at_capacity`, `quest_root_grey_fields`, `quest_root_first_failure`,
`quest_root_dead_orchard`, `quest_root_seed_annex`, `quest_root_quarantine`,
`quest_root_the_herd`, `quest_root_sunk_pond`, `quest_root_vets_choice`,
`quest_root_second_failure`, `quest_root_fiber_harvest`, `quest_root_bank_opens`,
`quest_root_third_season`, `quest_root_what_we_leave`.

### 6.2 Side quests (26)

**Soil (5)**
- `quest_root_soil_sample` — sample and classify a parcel
- `quest_root_ash_removal` — cart ash off a field
- `quest_root_lime_sweetening` — correct pH
- `quest_root_salinity_flush` — irrigate salt out
- `quest_root_fallow_year` — choose to rest land

**Water and irrigation (4)**
- `quest_root_ditch_dig` — cut an irrigation channel
- `quest_root_drip_line` — lay drip irrigation
- `quest_root_water_band` — decide whether marginal water is worth the risk
- `quest_root_reservoir` — build a pond as a reservoir

**Orchard and tree crops (4)**
- `quest_root_graft` — join scion to rootstock
- `quest_root_orchard_guard` — protect saplings from weather and wildlife
- `quest_root_first_blossom` — a blossom year with no fruit
- `quest_root_fruit_thief` — someone is taking the first fruit

**Livestock (5)**
- `quest_root_first_herd` — acquire and settle animals
- `quest_root_feed_pressure` — winter feed math
- `quest_root_birth` — assist a birth
- `quest_root_cull_choice` — the vet's recommendation
- `quest_root_lost_herd` — animals bolt or are stolen

**Veterinary (3)**
- `quest_root_animal_fever` — herd disease
- `quest_root_splint_set` — a working animal is injured
- `quest_root_quarantine_shed` — isolate a sick animal

**Seed heritage (3)**
- `quest_root_line_loss` — a line fails to germinate
- `quest_root_heirloom_trade` — trade genetics with a neighbor
- `quest_root_cold_drawer` — restore the seed bank's cooling

**Fiber and fuel (2)**
- `quest_root_hemp_retting` — process fiber
- `quest_root_oil_press` — press oilseed for lamp oil and trade

### 6.3 Repeatable quests (7)

`quest_root_repeat_weeding`, `quest_root_repeat_feeding`,
`quest_root_repeat_harvest`, `quest_root_repeat_compost`,
`quest_root_repeat_seed_check`, `quest_root_repeat_herd_walk`,
`quest_root_repeat_soil_test`.

### 6.4 Dynamic hooks

The live `AgricultureSystem` emits pest, mutation, and compost events; `ApicultureSystem`
emits swarm and honey events; `CompanionAnimalSystem` emits bond events. The generator
can attach authored follow-ups without a new event bus.

### 6.5 Constraints

- No plot may produce food without the `GreenhouseSystem` growth tick or the new
  open-ground tick, whichever owns it. There is exactly one growth authority.
- No yield may bypass `NutritionDiversitySystem`.
- Animal death must route through a real consequence; no free respawn.
- Seed loss must be permanent unless a stored line exists.
- No quest may make the first year easy; the long bet is the whole point.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `OpenGroundSystem` (new, `Ashfall.Core.Farming`)

**Owns:** land parcels, soil chemistry (pH, salinity, organic matter, radionuclide
load), open-ground plot state, irrigation demand, and season windows.
**Delegates:** growth stages/water to `GreenhouseSystem` (single growth authority);
the open-ground system ticks the greenhouse with outdoor environment inputs, exactly
as `AgricultureSystem` already does indoors.
**Data:** `soil_types.json`, `parcels.json`.
**Determinism:** `agriculture.open_ground` host-forked RNG stream.

```csharp
public sealed class OpenGroundSystem
{
    public void AddParcel(string parcelId, string locationId, string soilTypeId);
    public SoilAssessment Assess(string parcelId);
    public OpenGroundTickResult Tick(int day, AgricultureEnvironmentSnapshot env);
    public bool Amend(string parcelId, string amendmentItemId, int quantity);
}
```

### 7.2 `OrchardSystem` (new, `Ashfall.Core.Farming`)

**Owns:** tree-level crops, grafting, blossom/fruit cycles, and multi-year timers.
**Consumes:** `GreenhouseSystem` growth tick for the annual phase; `WeatherSystem`
for frost/blossom damage. **Data:** `orchard_species.json`.

### 7.3 `LivestockHerdSystem` (new, `Ashfall.Core.Ecology`)

**Owns:** herd population, breeding, feed demand, grazing, products (milk, eggs,
wool, meat), and herd health aggregate. **Consumes:** `CompanionAnimalSystem` for
individual bonds; `NeedsSystem` for feed; `DiseaseSystem` models are not reused for
animals. **Data:** `livestock_breeds.json`, `feed_fodder.json`.
**Rules:** herds do not breed without feed and space; slaughter is a typed event with
a welfare and morale consequence.

### 7.4 `VeterinarySystem` (new, `Ashfall.Core.Ecology`)

**Owns:** animal ailments, diagnosis, treatment, recovery, and cull recommendation.
**Consumes:** `LivestockHerdSystem`, `Inventory` (medicine). **Data:**
`animal_ailments.json`.
**Rules:** animal disease is distinct from human `DiseaseSystem`; it never spreads to
survivors without an authored zoonosis flag, and then it routes through `DiseaseSystem`.

### 7.5 `AquacultureSystem` (new, `Ashfall.Core.Farming`)

**Owns:** pond state, fish stock, feed, water quality, and harvest.
**Consumes:** `aquaponics_system_catalog.json` and water treatment bands. **Data:**
`aquaculture_species.json`.
**Rules:** ponds compete with irrigation and drinking water directly.

### 7.6 `SeedBankSystem` (new, `Ashfall.Core.Farming`)

**Owns:** seed lots, viability, heirloom preservation, cold/dry storage, and line loss.
**Consumes:** `CropStrainCatalog`. **Data:** `seed_bank.json`.
**Rules:** a stored line can be lost through neglect, contamination, or a failed
germination roll; the bank is the shelter's insurance and its inheritance.

### 7.7 `FiberCropSystem` (new, thin, `Ashfall.Core.Farming`)

**Owns:** fiber and oilseed crops, retting, and processing outputs. Feeds `Inventory`
and crafting. **Data:** `fiber_crops.json`.

### 7.8 Systems explicitly not added

- No second growth authority (`GreenhouseSystem`).
- No second nutrition system (`NutritionDiversitySystem`).
- No second human disease system.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `agriculture_items.json` (extend 2 → 40)

Existing schema preserved (`id`, `displayName`, `description`, `type`, `stackMax`,
`weight`, `tradeValue`). Proposed items: soil amendments (lime, gypsum, ash,
biochar), seed lots, feed (fodder, silage, mash), veterinary supplies (salve,
splint, worming), orchard stock (rootstock, scion, grafting wax), aquaculture feed,
fiber bales, retting tools, oil press parts, and harvest tools.

### 8.2 `soil_types.json` (new)

```json
{
  "schema_version": 1,
  "soils": [
    {
      "soil_id": "soil_ash_over_loam",
      "display_name": "Ash Over Loam",
      "description": "Grey ash crust over a compacted loam that still holds water.",
      "starting_ph": 7.8,
      "starting_salinity_permille": 12,
      "starting_organic_matter": 25,
      "starting_rad_load_bq_kg": 400,
      "drainage": 0.5,
      "amendment_affinity": ["lime", "compost", "biochar"],
      "tags": ["ash", "reclaimable"]
    }
  ]
}
```

### 8.3 `parcels.json` (new)

Parcel IDs tied to `loc_` node, soil type, area, and starting condition.

### 8.4 `orchard_species.json` (new)

Tree species, graft compatibility, years-to-fruit, blossom frost sensitivity, yield,
and lifespan.

### 8.5 `livestock_breeds.json` (new)

Species/breed rows: feed demand, space, cold tolerance, disease resistance, product
type and rate, breed interval, and temperament.

### 8.6 `feed_fodder.json` (new)

Feed items and conversion values.

### 8.7 `animal_ailments.json` (new)

Ailment ID, symptoms, severity, treatment, contagion (herd only), and veterinary
skill requirement.

### 8.8 `aquaculture_species.json` (new)

Fish/shellfish rows: water temperature band, feed, growth days, pond load, harvest
yield, and die-off risk.

### 8.9 `seed_bank.json` (new)

Seed lots: strain ID, quantity, viability, storage band, last-tested day, heirloom
flag, and loss condition.

### 8.10 `fiber_crops.json` (new)

Crop rows: growth days, water, retting days, fibers per plot, oil yield, and tags.

### 8.11 `compost_recipes.json` (new)

Input mix, time, temperature, output quality, and heat reuse.

### 8.12 `crop_strains.json` (extend)

New open-ground, orchard, fiber, and halophyte strains, using the existing schema
(`id`, `display_name`, `seed_item_id`, `yield_modifier`, `soil_tolerance`,
`toxicity_tolerance_permille`, `radiation_tolerance`, `pest_susceptibility`,
`mutation_threshold`, `nutrition_profile`, `mutation_outcomes`, `tags`).

### 8.13 `companion_animals.json` (extend 5 → 20)

New bonded and working animals and livestock guard species, preserving schema.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing: `src/Host/AgricultureSaveStore.cs`, `src/Host/CompanionSaveStore.cs`,
greenhouse host state. New sub-objects are additive inside these envelopes.

### 9.2 State to persist

- Parcels and soil chemistry.
- Orchard trees and graft state.
- Herd population, health, feed, products.
- Pond state and stock.
- Seed bank lots and viability.
- Fiber/retting state.
- Compost batches (partly live already).

### 9.3 Determinism

- Pest, mutation, birth, disease, and germination rolls use host-forked streams.
- Soil chemistry is pure arithmetic on authored rates.
- Growth ticks remain in `GreenhouseSystem`; no second growth clock.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no parcels, no orchard, no herd, no pond, no bank. No crops
are invented.

### 9.5 Checksum

Invariant-culture floats; integer-permille for salinity, viability, and contamination.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `AgriculturePanel` (extend) | Indoor + open-ground plots, soil, irrigation | `DeepRootHostSession` |
| `SoilPanel` (new) | Parcel chemistry, amendments, fallow | same |
| `OrchardPanel` (new) | Trees, grafts, blossom, years-to-fruit | same |
| `HerdPanel` (new) | Population, feed, products, welfare | same |
| `VetPanel` (new) | Ailments, diagnosis, treatment, cull | same |
| `AquaculturePanel` (new) | Ponds, stock, water quality, harvest | same |
| `SeedBankPanel` (new) | Lots, viability, heirloom flags, storage | same |
| `CompanionPanel` (extend) | Bonded animals | `CompanionAnimalSystem` host |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands.
- Soil and welfare use text plus icon, never color alone.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Years-to-fruit and risk are stated plainly; no hidden probability.
- Butchering and culling require explicit confirmation.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: seed into soil, first rain on ash, animal
low, milk pail, shears, pond splash, frost crack. No cue is required; text always
carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `GreenhouseSystem` | Sole growth authority; open ground ticks it |
| `AgricultureSystem` | Extended with parcels; pest/mutation/compost reused |
| `ApicultureSystem` | Hives link to orchard and fiber pollination |
| `FungiCultivationSystem` | Compost and substrate interplay |
| `NutritionDiversitySystem` | New food sources count toward dietary breadth |
| `CompanionAnimalSystem` | Bonds with working animals and herd guards |
| `WaterTreatmentSystem` | Irrigation water band |
| `WeatherSystem` | Season windows, frost, storm damage |
| `RadiationSystem` | Soil rad load and crop uptake |
| `DiseaseSystem` | Only via authored zoonosis flag |
| `NeedsSystem` | Feed, water, and food pressure |
| `Inventory` | All inputs and outputs are items |
| `PowerGridSystem` | Grow lamps, seed bank cooling, feed mill |
| `Silage`/`food_preservation` | Crop storage |
| `TradingSystem` | Seed, fiber, and livestock trade |
| `MemorialSystem` / `GuiltInsomniaSystem` | Herd loss and cull consequences |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `AgricultureSystem`, `GreenhouseSystem`,
`ApicultureSystem`, `CompanionAnimalSystem`, `NutritionDiversitySystem`, save stores,
and host session APIs. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend agriculture items, strains, companion
animals; author soil, parcels, orchard, livestock, feed, ailments, aquaculture, seed
bank, fiber, compost. Register validators and scanner.

**Phase 2 — Pure Core.** `OpenGroundSystem`, `OrchardSystem`, `LivestockHerdSystem`,
`VeterinarySystem`, `AquacultureSystem`, `SeedBankSystem`, `FiberCropSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `DeepRootHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 180/360-day soak; the long bet must be provable.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Agriculture items | 38 new (2 → 40) |
| Crop strains | +20 |
| Companion animals | 15 new (5 → 20) |
| Soil types | 10 |
| Parcels | 24 |
| Orchard species | 10 |
| Livestock breeds | 10 |
| Feed/fodder | 12 |
| Animal ailments | 15 |
| Aquaculture species | 8 |
| Seed bank lines | 30 |
| Fiber crops | 8 |
| Compost recipes | 8 |
| Locations | 12 |
| Rooms | 6 |
| NPCs | 8 |
| Main quests | 14 |
| Side quests | 26 |
| Repeatable | 7 |
| Items (all types) | 38 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Open ground trivializes greenhouse | High | Years-to-payoff, weather, chemistry |
| Second growth authority | High | `GreenhouseSystem` ticks all growth |
| Animal welfare trivialized | High | Feed, space, disease, cull events |
| Seed bank becomes infinite food | High | Viability loss, storage cost, blight |
| Save bloat (per-tree/per-animal) | Medium | Aggregate herds; per-tree only for orchard |
| Zoonosis leak into human disease | Medium | Authored flag only |
| Determinism break | Low | Host-forked RNG only |
| Content overrun | Medium | Budget §22 |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Farming/OpenGroundSystemTests.cs`
- `Ashfall.Core.Tests/Farming/OrchardSystemTests.cs`
- `Ashfall.Core.Tests/Ecology/LivestockHerdTests.cs`
- `Ashfall.Core.Tests/Ecology/VeterinarySystemTests.cs`
- `Ashfall.Core.Tests/Farming/AquacultureTests.cs`
- `Ashfall.Core.Tests/Farming/SeedBankTests.cs`
- `Ashfall.Core.Tests/Farming/DeepRootSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Farming/DeepRootDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/AgricultureCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Soil chemistry responds deterministically to amendments and irrigation.
- Salt and rad load can return if neglected.
- Orchard trees require the authored years before fruiting.
- Herds do not breed without feed and space; products scale with welfare.
- Veterinary disease does not spread to humans without the authored flag.
- Ponds compete for water; die-off is possible.
- Seed lots lose viability; a lost line is not recoverable without storage.
- Growth stages come only from `GreenhouseSystem`.
- Round-trip restores parcels, trees, herds, ponds, lots.
- Legacy loads neutral; paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Farming/
bash scripts/run_test.sh Ashfall.Core.Tests/Ecology/
godot --headless --path . -- --agriculture-selftest
godot --headless --path . -- --greenhouse-selftest
godot --headless --path . -- --aquaponics-selftest
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
| 12 The Second Generation | Children as growers; school garden; seed inheritance |
| 13 The Faithful | Ash Gardener movement; harvest ceremonies |
| 14 Above the Ash | Aerial seeding; crop survey; seed airlift |
| 16 The Rebuilt Body | Veterinary prosthetics; robotic herders |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- `GreenhouseSystem` as the single growth authority.
- The live 12 crop strains and their mutation outcomes.
- The live 5 companion species.
- `NutritionDiversitySystem` as the dietary authority.
- The water scarcity hierarchy (drinking > crops > industry).

### 16.2 New canon

- The Grey Fields and the Dead Orchard.
- The seed bank and its heirloom lines.
- Livestock as production with welfare.
- The reclamation timeline as a campaign-long arc.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — SOIL TYPE TABLE

| Soil | Start pH | Salinity  | Organic | Rad Bq/kg | Drainage | Notes |
|---|---|---|---|---|---|---|
| Ash Over Loam | 7.8 | 12 | 25 | 400 | 0.5 | reclaimable |
| Deep Ash | 8.1 | 8 | 10 | 900 | 0.4 | ash cap |
| Salt Crust | 8.4 | 45 | 5 | 200 | 0.2 | halophyte only |
| Flood Silt | 6.9 | 18 | 40 | 600 | 0.6 | nutrient-rich, wet |
| Sandy Scour | 7.2 | 6 | 12 | 350 | 0.8 | drains fast |
| Clay Pan | 7.6 | 22 | 20 | 300 | 0.15 | waterlogging |
| Peat Remnant | 5.8 | 4 | 70 | 150 | 0.3 | acidic, rich |
| Rubble Fill | 8.0 | 15 | 8 | 500 | 0.7 | shallow, stony |
| Greenhouse Spoil | 6.7 | 10 | 45 | 250 | 0.6 | used media |
| Bone Dry Dust | 8.3 | 30 | 2 | 1100 | 0.9 | worst case |

Each soil type has an authored amendment affinity; no soil is unrecoverable, but
all are slow.

---

## 18. APPENDIX B — LIVESTOCK BREED TABLE

| Breed | Species | Feed/day | Space | Cold tol | Disease res | Product | Rate |
|---|---|---|---|---|---|---|---|
| Feral Goat *(live species)* | goat | 3 | low | high | high | milk | 1.5 L/day |
| Ash Hound *(live species)* | canine | 2 | low | med | med | guard | — |
| Silt Sheep | ovine | 4 | med | med | low | wool, milk | 1.2 kg/season |
| Mail-Pigeon | avian | 0.3 | tiny | high | med | messenger | — |
| Coppice Rabbit | lagomorph | 1 | low | high | high | meat, pelts | 0.8 kg/cycle |
| Bog Pig | porcine | 6 | high | med | low | meat | 40 kg/cycle |
| Draft Ox | bovine | 9 | high | high | med | labor, meat | — |
| Ruin Chicken | avian | 0.5 | low | med | high | eggs | 200/yr |
| Watch Crow *(live species)* | avian | 0.4 | tiny | high | med | alarm | — |
| Burrow Bee-Goat | caprine | 3 | med | high | high | milk, fiber | 1 L/day |

No breed maximizes feed efficiency, hardiness, product rate, and space at once.

---

## 19. APPENDIX C — ORCHARD SPECIES TABLE

| Species | Years to fruit | Frost sensitivity | Yield | Lifespan | Graft base |
|---|---|---|---|---|---|
| Ash Apple | 4 | high | high | 30 yr | crab rootstock |
| Grey Pear | 5 | med | high | 40 yr | quince |
| Cinder Plum | 3 | high | med | 20 yr | plum |
| Cold Cherry | 4 | high | med | 25 yr | cherry |
| Sour Quince | 3 | low | low | 35 yr | self |
| Nut Hazen | 5 | low | med | 60 yr | self |
| Walnut Grey | 8 | low | high | 80 yr | self |
| Fig Ruin | 3 | med | med | 20 yr | fig |
| Olive Ash | 6 | low | high | 70 yr | self |
| Service Berry | 3 | med | low | 25 yr | self |

Trees survive the player's term but not the player's patience. That tension is the
point.

---

## 20. APPENDIX D — SEED BANK VIABILITY MODEL

- **Storage bands:** cold-dry (best), cool-dry, ambient, damp (loss).
- **Viability** is integer-permille; it decays at an authored rate per band.
- **Testing** costs a seed and a day; failure reveals loss.
- **Heirloom flag** marks lines whose loss cannot be replaced from trade.
- **Blight cross-contamination** can ruin a drawer if quarantine fails.
- A lost line is permanent; the bank is the shelter's only insurance against
   `sterile_seed` and open-ground failure.

---

## 21. APPENDIX E — ECONOMY AND BALANCE MODEL

- **Indoor farming** is reliable but power- and water-hungry and capped by bays.
- **Open ground** is cheap per kilogram but slow, weather-exposed, and risky.
- **Orchards** pay in years; their real cost is the player's commitment.
- **Livestock** convert feed to protein, but feed competes with human food.
- **Aquaculture** is protein-dense but competes directly with drinking water.
- **Seed bank** is not food; it is the option to plant again after failure.
- **Fiber and oil** make the system solvent when food is stable.
- The intended arc: year 1 hunger, year 2 break-even, year 3+ surplus.

---

## 22. APPENDIX F — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `agriculture_items.json` | +38 | 6,000 |
| `crop_strains.json` | +20 | 4,000 |
| `companion_animals.json` | +15 | 3,000 |
| `soil_types.json` | 10 | 2,000 |
| `parcels.json` | 24 | 2,500 |
| `orchard_species.json` | 10 | 2,000 |
| `livestock_breeds.json` | 10 | 2,500 |
| `feed_fodder.json` | 12 | 1,500 |
| `animal_ailments.json` | 15 | 3,000 |
| `aquaculture_species.json` | 8 | 2,000 |
| `seed_bank.json` | 30 | 3,500 |
| `fiber_crops.json` | 8 | 1,800 |
| `compost_recipes.json` | 8 | 1,200 |
| Quest objectives | 47 quests | 14,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 12 | 4,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~63,000** |

---

## 23. APPENDIX G — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R15-1 | Open ground dominates indoor | Med | High | Slow payoff; weather; chemistry |
| R15-2 | Second growth authority | Low | High | Single greenhouse tick |
| R15-3 | Animal welfare trivialized | Med | High | Feed, space, disease, cull |
| R15-4 | Seed bank infinite food | Med | High | Viability loss; storage cost |
| R15-5 | Save bloat | Med | Med | Aggregate herds; per-tree orchard |
| R15-6 | Zoonosis leak | Low | Med | Authored flag |
| R15-7 | Water competition trivial | Med | Med | Aquaculture draws real water |
| R15-8 | Determinism | Low | High | Host-forked RNG |
| R15-9 | Content overrun | Med | Med | Budget §22 |
| R15-10 | First year too easy | High | High | Authored failures in main line |

---

## 24. APPENDIX H — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does open ground share the greenhouse growth tick or get its own clock?**
   Recommended: shares the greenhouse tick with outdoor environment inputs.
2. **Per-tree or aggregate orchard state?** Recommended: per-tree for the first N
   trees, aggregate beyond, to bound save size.
3. **Can livestock be bred to extinction?** Recommended: yes, with recovery only
   through trade, preserving permanence.
4. **Zoonosis** — should any animal disease cross to humans? Recommended: one
   authored case, routed through `DiseaseSystem`.
5. **Seed bank cold storage** — does it require continuous power? Recommended: yes,
   so the bank competes with the grid.

---

## 25. APPENDIX I — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_root_at_capacity` | 3 | Grow-room at limit; Dessa proposes ground; Brann objects |
| `quest_root_grey_fields` | 5 | Reach the fields; sample soil; stake a first parcel |
| `quest_root_first_failure` | 4 | Weather or chemistry kills the planting; assess and retry |
| `quest_root_dead_orchard` | 5 | Recover grafting stock; plant; begin the years-long clock |
| `quest_root_seed_annex` | 5 | Enter the annex; recover lines; discover blight |
| `quest_root_quarantine` | 4 | Blight in the greenhouse; burn, isolate, or treat |
| `quest_root_the_herd` | 5 | Acquire animals; feed; decide purpose |
| `quest_root_sunk_pond` | 5 | Restore the pond; stock it; manage water quality |
| `quest_root_vets_choice` | 4 | Diagnose; treat or cull; face the child who named it |
| `quest_root_second_failure` | 4 | Salt or rads return; amend, flush, or abandon |
| `quest_root_fiber_harvest` | 4 | Grow, ret, process; produce textiles and oil |
| `quest_root_bank_opens` | 5 | Test viability; open or seal the drawers |
| `quest_root_third_season` | 5 | The real first field harvest; count what it means |
| `quest_root_what_we_leave` | 3 | Final disposition; epilogue selection |

---

## 26. APPENDIX J — NPC DOSSIERS (BRIEF)

**Dessa Marr** — agronomist. Has kept the grow-room alive for years and knows it is
not enough. Impatient, exacting, and willing to spend the shelter's present for a
future she will likely not see. The expansion's engine.

**Brann Ost** — quartermaster. Opposes the fields because he has buried the people
who chased promises before. Not a villain; the voice of every player who has watched
an expedition die for a plan.

**Yla Brant** — herder. Speaks to animals and about them with more ease than about
people. Knows each animal individually and grieves each loss.

**Oscar Taal** — veterinarian. Calm with blood, blunt about odds. His cull
recommendations are the expansion's hardest conversations.

**Hana Vey** — seed keeper. Treats the drawers like a library of the dead. The one
person who understands that diversity is survival.

**Ko Fen** — breeder. Thinks in generations. Will trade genetics with a rival
shelter and accept the risk.

**Pell Ash** — forager. Bridges wild and cultivated; brings back what the fields
cannot yet grow.

**Tam** — young grower. Names the first lamb; plants the first seed; carries the
land's future in a way the adults can see but not articulate.

---

## 27. APPENDIX K — LOCATION DETAIL

- **The Grey Fields** — ash over loam; the first furrow is the whole expansion.
- **The Dead Orchard** — a row of black sticks; some still graft-viable.
- **The Ruined Pasture** — fencing to rebuild, forage to assess, a herd to place.
- **The Sunk Pond** — water that must be tested before it can be used twice.
- **The Seed Annex** — cold, dark, labelled; the shelter's inheritance.
- **The Glass Farm** — a ruined commercial greenhouse; glazing is the prize.
- **The Salt Flat** — halophytes only; a hard teacher.
- **The Retted Fields** — hemp and flax; the smell of processing.
- **The Grafting Stand** — a trade in scion and rootstock, and in rumor.
- **The Stock Wreck** — a frozen transport; genetics or disease, maybe both.
- **The Steam Yard** — compost heat you can feel through your boots.
- **The Herb Walk** — medicine that grows, if anyone remembers the names.

---

## 28. APPENDIX L — ANIMAL AILMENT TABLE

| Ailment | Herd risk | Severity | Treatment | Vet skill |
|---|---|---|---|---|
| Foot rot | high | med | dry, pare, salve | 2 |
| Mastitis | high | med | strip, salve, warmth | 2 |
| Worms | high | low | worming dose | 1 |
| Bloat | med | high | relieve, massage | 3 |
| Cold lung | med | high | warmth, rest | 3 |
| Fly strike | high | med | shear, treat | 2 |
| Milk fever | low | high | minerals | 3 |
| Retained afterbirth | low | high | assist, antibiotics | 4 |
| Splint/fracture | low | high | set, rest | 4 |
| Contagious swelling | low | high | isolate, cull | 4 |
| Rad burn | low | high | salve, isolate | 3 |
| Poisoned forage | med | high | charcoal, rest | 3 |
| Dehydration | high | med | water, shade | 1 |
| Starvation | high | high | feed, slow refeed | 2 |
| Zoonosis (authored) | low | critical | human quarantine | 4 |

Only the authored zoonosis row crosses into `DiseaseSystem`, and only through that
system's public path.

---

## 29. APPENDIX M — AQUACULTURE SPECIES TABLE

| Species | Temp band | Feed | Growth days | Pond load | Yield | Risk |
|---|---|---|---|---|---|---|
| Silt Carp | 12–24 | grain | 90 | med | high | low |
| Grey Tilapia | 18–30 | algae | 120 | high | high | med |
| Cold Trout | 6–16 | insects | 150 | low | med | high |
| Bog Catfish | 10–28 | scraps | 100 | high | high | low |
| Ruin Cray | 8–22 | detritus | 70 | low | med | med |
| Ash Mussel | 4–20 | filter | 200 | low | low | low |
| Reed Snail | 6–24 | waste | 60 | low | low | low |
| Mirror Koi | 10–26 | pellets | 180 | med | low (trade) | med |

Ponds compete with irrigation and drinking water; high pond load raises die-off.

---

## 30. APPENDIX N — FIBER AND OIL CROPS

| Crop | Days | Water | Fiber | Oil | Use |
|---|---|---|---|---|---|
| Cold Hemp | 100 | med | high | low | cordage, cloth |
| Grey Flax | 90 | med | high | med | linen, lamp oil |
| Ash Ramie | 120 | high | high | none | strong cloth |
| Oil Rape | 80 | low | none | high | lamp oil, trade |
| Nettle Cloth | 70 | low | med | none | cordage |
| Silt Reed | 60 | high | med | none | thatch, paper |
| Cinder Cotton | 140 | high | high | low | cloth (indoor) |
| Bog Kenaf | 110 | med | high | none | sacks, rope |

Fiber makes the shelter solvent when food is stable, and disappears when fuel or
water is short.

---

## 31. APPENDIX O — CONTENT REVIEW CHECKLIST

- [ ] No real cultivar, breed, or proprietary name is used.
- [ ] Growth stages come only from `GreenhouseSystem`.
- [ ] Open-ground payoff is measured in months, not days.
- [ ] Every soil type is reclaimable but slow.
- [ ] Animals have feed, space, welfare, disease, and a cull path.
- [ ] Butchering and culling are treated with gravity and require confirmation.
- [ ] The seed bank can lose lines permanently.
- [ ] Aquaculture competes with drinking water.
- [ ] Zoonosis stays behind the authored flag.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 32. APPENDIX P — GLOSSARY

- **Parcel** — an open-ground plot cluster with soil chemistry.
- **Amendment** — a material that changes soil chemistry (lime, compost, biochar).
- **Fallow** — deliberately resting land to recover.
- **Scion / rootstock** — the grafted parts that make an orchard tree.
- **Retting** — processing fiber crops to free the fiber.
- **Viability** — a seed lot's chance to germinate, in permille.
- **Zoonosis** — an animal disease that can cross to humans (authored).
- **Heirloom** — a seed line that cannot be replaced from trade.

---

## 34. APPENDIX Q — COMPOST RECIPE TABLE

| Recipe | Inputs | Days | Temp | Output | Heat reuse |
|---|---|---|---|---|---|
| Basic Aerobic | crop waste + manure | 30 | warm | humus | low |
| Hot Heap | straw + manure + water | 21 | hot | rich humus | med |
| Ash-Amended | humus + wood ash | 14 | warm | alkaline mix | low |
| Biochar Blend | char + humus | 10 | cool | stable carbon | none |
| Fungal Compost | waste + spawn | 45 | warm | disease-suppressive | low |
| Bokashi | kitchen waste + bran | 14 | cool | fermented matter | none |
| Leaf Mold | leaves + time | 120 | cool | soil conditioner | none |
| Steam Yard Batch | mixed manure + straw | 28 | hot | large humus + heat | high |

Compost supplies the organic matter that makes every other amendment work. It is
the least glamorous and most load-bearing part of the expansion.

---

## 35. APPENDIX R — FIELD ROTATION CALENDAR (EXAMPLE)

| Season | Parcel A | Parcel B | Parcel C | Parcel D |
|---|---|---|---|---|
| Spring | grain | legume | fallow/amended | fiber |
| Summer | grain | legume | orchard undercrop | fiber |
| Autumn | roots | green manure | orchard undercrop | roots |
| Winter | fallow | fallow | mulch | fallow |

Rotation is not enforced by the engine; it is a taught practice. A player who plants
the same crop every season sees medium quality and pest severity climb, exactly as
the live `AgriPlotState` fields already anticipate.

---

## 36. APPENDIX S — WORKED ECONOMY EXAMPLE (YEAR 1 → YEAR 3)

**Year 1**: The shelter plants one 200 m² parcel. Soil is ash over loam: pH 7.8,
salinity 12, rad load 400. Lime and compost cost labor and a week. The first
planting fails to frost. The second planting yields 40 kg of roots — less than the
seed and labor cost. The greenhouse still feeds the shelter. The player has banked
experience and organic matter.

**Year 2**: Medium quality is up; salinity is down after one flush. Two parcels are
planted. A herd of six goats is acquired, consuming 18 kg of feed per day. The herd
is a net loss until the first kids. The orchard saplings survive a winter. The seed
bank is cold and full.

**Year 3**: The first real field harvest yields 900 kg of mixed crops, exceeding the
greenhouse for the first time. The orchard blossoms but sets little fruit. The herd
provides milk daily and a first cull. The shelter is solvent for the first time.

This arc is the expansion's promise. It must be difficult, visible, and never
accelerated by a shortcut.

---

## 37. APPENDIX T — OPENING VIGNETTE (TONE SAMPLE)

> Dessa takes a handful of the grey soil and lets it run through her fingers, and
> when it is gone she does not wipe her hand. The grow-room hums behind her under
> the lamps, steady as a heart, and she does not turn around to look at it.
>
> "This is not dirt," she says. "Dirt is what they had before. This is a debt."
>
> Brann stands at the edge of the field with his arms crossed, which is how he
> stands everywhere. He does not say that they cannot afford a debt. He does not
> have to. Everybody listening knows how many winters the grow-room has left, and
> nobody has said it aloud yet.
>
> Tam kneels in the furrow and presses one seed into the ground with a thumb, and
> covers it, and pats it flat, the way a child does when they have been told exactly
> once how.

This sets the register. All Deep Root prose should be patient, physical, and free
of triumph.

---

## 38. CLOSING STATEMENT

ASHFALL can feed a shelter through a sealed grow-room and a hive of bees. What it
has never asked is whether the shelter can feed itself from the ground again — the
slow, poisoned, patient ground that the Exchange left behind. The Deep Root is that
question, answered in soils, orchards, herds, ponds, and seed drawers. It adds no
second growth authority, no instant harvest, and no fantasy of abundance. It adds a
field, a graft, a lamb, and a cold drawer of seeds that someone may plant long after
the player is gone.