# Plan 55 Recipe Category Matrix — New Roster & Reconciliation

Baseline: **73 recipes** (Case B — other plans already landed content).
Final: **81 recipes**. The 41-concept source roster was reconciled against the
73 existing recipes; concepts already covered were **not duplicated**, and
concepts without a live consumer were **substituted or deferred** (documented
below). Category totals therefore differ from the source's 10/5/8/6/5/4/3
targets, which assumed the 39 baseline.

## New recipes (8)

| id | name | category | tier | ingredients | result | station | hours | input source | output consumer |
|---|---|---|---|---|---|---|---|---|---|
| `craft_flatbread` | Bake Ash-Grain Flatbread | food | basic | crop_ash_grain×2, clean_water×1, fuel×1 | `item_flatbread`×2 | stove | 0.8 | greenhouse harvest, water/fuel infra | needs/food system |
| `craft_boiled_roots` | Boil Foraged Roots | food | basic | roots×3, clean_water×1, fuel×1 | `item_boiled_roots`×3 | stove | 0.5 | surface foraging (scavenging) | needs/food system |
| `craft_vegetable_soup` | Cook Greenhouse Vegetable Soup | food | basic | crop_leafy_green×3, crop_tuber×2, clean_water×1, fuel×1 | `item_vegetable_soup`×3 | stove | 1.2 | greenhouse harvest | needs/food system (+morale) |
| `craft_pemmican` | Press Pemmican Blocks | food | intermediate | item_smoked_meat×2, crop_ash_grain×2, item_preservation_salt×1 | `item_pemmican`×3 | workbench | 2.0 | preserved meat chain + greenhouse | needs/food system; expedition logistics |
| `craft_travel_ration` | Bundle Travel Rations | food | intermediate | item_smoked_meat×1, item_pickled_tubers×2, item_flatbread×2 | `item_travel_ration`×2 | workbench | 1.0 | other Plan-55 + legacy recipes | needs/food system; expedition logistics |
| `craft_splint` | Splint Fracture Kit | medicine | basic | wooden_plank×1, cloth×2, duct_tape×1 | `splint`×1 | workbench | 0.5 | loot/scrap | `medical_fracture` treatment (`medical_texts.json` required_items) |
| `reload_556` | Reload 5.56mm Batches | ammunition | intermediate | empty_brass_shell×10, reloading_primer×10, smokeless_powder×1 | `ammo_556`×10 | workbench | 1.5 | scavenging (depot/police tables) | `combat_catalog.json` 5.56mm weapons |
| `reload_762` | Reload 7.62mm Batches | ammunition | intermediate | empty_brass_shell×10, reloading_primer×10, smokeless_powder×1 | `ammo_762`×10 | workbench | 1.6 | scavenging (depot/police tables) | `combat_catalog.json` 7.62mm weapons |

## Reconciliation of the 41 source concepts

| Source concept | Disposition | Reason |
|---|---|---|
| Dried meat | **already exists** — `craft_smoked_meat_rations` | duplicate avoided |
| Smoked fish | substituted — salted/smoked meat chain exists; no fish item chain with distinct consumer | no live differentiation |
| Preserved vegetables | **already exists** — `craft_pickled_tubers` | duplicate avoided |
| Flatbread | **added** — `craft_flatbread` | staple gap; grain economy honored (crop_ash_grain) |
| Bone broth | **deferred** | no `bone` item; one-purpose ingredient clutter (§6) |
| Fermented cabbage | **already exists** — `craft_fermented_sauerkraut` | duplicate avoided |
| Pemmican | **added** — `craft_pemmican` | expedition logistics; ≈calorie-neutral, portability is the value |
| Boiled roots | **added** — `craft_boiled_roots` | scarcity fallback food |
| Canned stew | **already exists** — `craft_canned_grain_stew`, `craft_canned_rations` | duplicate avoided |
| Ration bars | **added** (as `craft_travel_ration`) | compact logistics; consumes downstream crafted goods |
| Charcoal filter cartridge | **already exists** — `craft_charcoal_filter`, `craft_water_filter` | duplicate avoided |
| Rainwater collector | **deferred** | no rain-collection consumer system |
| Solar still kit | **deferred** | no portable water-equipment consumer |
| Iodine water-treatment dose | **deferred** | no chemical water-treatment consumer; iodine_pills are medical authority |
| Boiling/purification component kit | **already exists** — `boil_water`, `craft_filter_reconditioning`, component sinks | duplicate avoided |
| Herbal poultice | **already exists** — `craft_herbal_poultice` | duplicate avoided |
| Iodine antiseptic solution | **already exists** — `craft_antiseptic_solution`; item authority noted | duplicate avoided |
| Contamination-management course | **deferred to pharma authority** | `pharma_recipes.json` owns chelation/decorporation (`recipe_edta_chelation`, `recipe_prussian_blue`) |
| Analgesic preparation | **deferred to pharma authority** | `recipe_tramadol`, `recipe_palliative_morphine` exist |
| Antibiotic course | **deferred to pharma authority** | 5 antibiotic-producing pharma recipes exist |
| Antiseptic item | **flagged, deferred** | `medical_texts.json` requires item id `antiseptic` which does not exist (`antiseptic_1l_of_1l` does) — a dangling reference owned by the medical authority, not crafting |
| Splint | **added** — `craft_splint` | item existed with a live treatment consumer but no recipe |
| Field suture kit | **deferred** | `surgical_suture` has no live consumer; pharma owns sterile suture kits |
| Tool repair kit | **deferred** | `ShelterWorkshopSystem` overhaul consumes `machine_oil` + `mechanical_parts` directly (repair-in-place authority) — no kit item consumer |
| Utility knife / hand axe | **deferred** | no distinct consumer (crafting/woodcutting systems absent); `military_grade_hatchet` is loot with no .cs consumer |
| Sewing kit | **deferred** | no clothing-condition consumer; `craft_textile_repair` already converts cloth directly |
| Lockpick set | **deferred** | `SafeCrackingSystem` tools are defined per-safe in data, not as inventory items |
| Radiation detector assembly | **already exists** — `craft_dosimeter`, `craft_geiger_counter`, `craft_calibration_kit` | duplicate avoided |
| Air-filter cartridge | **already exists** — `craft_air_filter`, `recipe_gas_mask_filter` | duplicate avoided |
| Generator service kit | **deferred** | no generator-condition consumer of service items |
| Insulation panel | **deferred** | shelter insulation consumes raw materials directly via build/upgrade costs |
| Door bracing kit | **deferred** | sky-layer armor / room builds consume raw items directly |
| Water-pipe section | **deferred** | `thaw_frozen_pipe` consumes existing items directly (zero-result sink authority) |
| Reload 9×19 / 12g batches | **already exist** — `reload_9x19`, `reload_12g_buck` | duplicate avoided |
| Reload 5.56 batch | **added** — `reload_556` | live caliber (2 weapons), provenance repaired |
| Reload 7.62 batch | **added** — `reload_762` | live caliber (1 weapon), provenance repaired |
| Improvised slug | **already exists** — `ammo_improvised_rod`/`ammo_improvised_burn` in caliber authority | duplicate avoided |
| Engine gasket | **substituted** — see vehicle row | no component-consumption path |
| Tire patch kit | **substituted** | vehicle repair consumes no items |
| Fuel-filter element | **substituted** | vehicle repair consumes no items |
| Vehicle repair components ×3 | **deferred with evidence** | `ExpeditionVehicleSystem.Repair(vehicleId, amount)` takes a float and consumes nothing; per §55D.12 no Plan-55-only vehicle repair framework was created. The mechanical niche is served by existing `craft_engine` + shelter mechanical sinks (room builds, sky-layer armor, workshop overhauls consuming `mechanical_parts`) |

## Tier classification (documentation-only; no runtime field)

- **Basic:** `craft_flatbread`, `craft_boiled_roots`, `craft_vegetable_soup`,
  `craft_splint` — common inputs, short time, starting infrastructure.
- **Intermediate:** `craft_pemmican`, `craft_travel_ration`, `reload_556`,
  `reload_762` — refined inputs (preserved meat chain; scarce reloading
  components), workbench, moderate time.
- **Advanced:** covered by the pre-existing pharma lab (26 recipes at
  `pharma_bench`), research breakthrough items, and zero-result infrastructure
  sinks. Plan 55 added none — existing advanced content already occupies this
  tier without duplication.
