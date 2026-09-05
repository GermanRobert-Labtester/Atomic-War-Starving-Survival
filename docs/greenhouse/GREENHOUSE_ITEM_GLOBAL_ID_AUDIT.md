# GREENHOUSE ITEM GLOBAL ID AUDIT (Plan 91)

Global item namespace at Plan 91 execution: **747 unique IDs** across 10 item
files (`items.json` 544, `year_of_ash_items` 62, `holdfast_items` 55,
`greenhouse_items` 30→30, `foundry_items` 30, `black_flotilla_items` 36,
`verdict_items` 15, `chemical_dependency_items` 13, `crossing_items` 11,
`dose_items` 9). 45 pre-existing cross-file duplicates (first-loaded-wins,
tolerated by the loader) were recorded before work began — none involve
Plan 91 IDs.

## Uniqueness of the 16 new IDs

Each ID was checked against all 747 existing IDs and all display names in the
10 item files before authoring:

| New ID | Exact-ID collision | Semantic near-duplicate check |
|---|---|---|
| `item_greenhouse_trowel` | none | no trowel/spade/hand-tool exists |
| `item_greenhouse_pruning_shears` | none | `medical_scissors` is clinical bandage work — distinct function |
| `item_greenhouse_watering_can` | none | no watering vessel exists; greenhouse has no piped irrigation consumer, so manual watering stays lore-valid |
| `item_greenhouse_hand_cultivator` | none | no cultivator/rake-head tool exists; distinct from trowel (role, weight, value) |
| `item_greenhouse_compost` | none | no compost exists; canon-adjacent via `location_rot_farmers_compost_yard` |
| `item_greenhouse_ash_fertilizer` | none | distinct from `item_grow_medium` (sterile substrate = bed, ash = nutrient amendment) |
| `item_greenhouse_fish_emulsion` | none | `trap_fish` (Fish Trap) proves a fish economy exists to feed it |
| `item_greenhouse_insecticidal_soap` | none | distinct from `item_blight_treatment` (fungal blight wash, live consumer) |
| `item_greenhouse_sticky_traps` | none | no trap-board item exists |
| `item_greenhouse_pest_mesh` | none | `item_faraday_mesh` is EMI shielding — distinct |
| `item_greenhouse_drip_kit` | none | no irrigation item exists |
| `item_greenhouse_line_filter` | none | `item_air_filter_hepa`/`air_filter`/`filter_pack` are air; `item_ro_membrane` is potable RO; `holdfast item_water_filter` is ceramic potable — none is an irrigation-line cartridge |
| `item_greenhouse_catchment_kit` | none | no rainwater item exists |
| `item_greenhouse_glass_pane` | none | `item_lead_glass_pane` is leaded *shielding* glazing; this is plain replacement glazing — distinct weight/stack/value/role |
| `item_greenhouse_uv_sheeting` | none | `plastic_material` / `item_bio_plastic` are generic crafting sheet — horticultural UV film is distinct in function and description |
| `item_greenhouse_shade_cloth` | none | no shade/windbreak textile exists |

## Prefix policy compliance (plan §7)

- All 16 use `item_greenhouse_*` — greenhouse supplies/tools/repair, per the
  non-seed convention.
- No `item_seed_` prefix was forced onto non-seed supplies.
- No generic tool was re-published under a greenhouse alias; generic gear
  (`rubber_hose`, `plastic_material`, `mechanical_parts`, `scrap_metal`,
  `wood_block`) is *referenced* by recipes, not duplicated.

## Reuse decisions (plan §27, §29, §36)

- **Water filter cartridge** — audited; no irrigation-line cartridge exists
  globally, so `item_greenhouse_line_filter` (Filter) is justified
  greenhouse-specific content. Potable filtration IDs untouched.
- **Glass panel** — audited; no plain glazing pane exists (only leaded), so a
  greenhouse-specific pane is justified.
- **Seed packets** — generic `seed_packets` in `items.json` remains the
  scavenge abstraction; greenhouse seeds stay distinct, named cultivars.

## Validation

- `CatalogIntegrityValidator` (data-integrity-selftest): **PASS, 0 findings**,
  including Tier-1/Tier-2 checks over the new recipes' `resultItemId` /
  `ingredients[].itemId` and the scavenging `item_id` additions.
- Pinned by `GreenhouseItemCatalogTests.GlobalCatalog_NoIdCollisionsAcrossItemFiles`
  and `.GlobalCatalog_RegistersAllThirtyGreenhouseEntries`.
