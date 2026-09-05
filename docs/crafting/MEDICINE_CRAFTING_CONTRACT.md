# Medicine Crafting Contract (Plan 55)

## Two medicine authorities — crafting must not become a third

1. **Pharma lab** (`pharma_recipes.json`, 26 recipes, station `pharma_bench`,
   `PharmaLabSystem` + chemist skill evaluator + dependency-risk wiring) owns
   pharmaceutical compounding: chelators, psychotropics, stimulants,
   anesthetics, antibiotics, antiseptics, taper kits.
2. **Medical treatments** (`medical_texts.json` conditions) own *consumption*:
   treatments require concrete item ids — `bandage`, `splint`, `antiseptic`,
   `antibiotics`, `clean_water`, `medical_kit`, `iodine_pills`, `anti_rad`,
   `fuel`, `cloth`, `canned_food`.

## Plan-55 medicine additions

| Recipe | Output | Consumer | Rationale |
|---|---|---|---|
| `craft_splint` | `splint` ×1 | `medical_fracture` requires `splint` + `bandage` | the item existed (Medical type, tradeValue 9) with a live treatment consumer but no production path |

That is the complete medicine addition, deliberately. Everything else the
source roster proposed is already owned elsewhere:

- Analgesics, antibiotics, chelation/decorporation, sterile sutures,
  rehydration, burn/frostbite preparations → **pharma lab** (e.g.
  `recipe_tramadol`, `recipe_cefazolin`, `recipe_edta_chelation`,
  `recipe_prussian_blue`, `recipe_suture_kit_sterile`, `recipe_burn_gel`,
  `recipe_silver_sulfadiazine`). Duplicating them as crafting recipes would
  create a second pharmaceutical authority (forbidden, §1.8).
- Herbal poultice, antiseptic solution, frostbite salve → **existing crafting
  recipes** (`craft_herbal_poultice`, `craft_antiseptic_solution`,
  `craft_frostbite_salve`).
- Iodine: `craft_anti_rad`, `recipe_iodine_kit` (crafting) and pharma
  `recipe_potassium_iodide`/`recipe_iodine_swab` exist. Iodine is **not**
  modeled as a universal radiation cure anywhere and Plan 55 does not change
  that (Risk 7 respected).

## Flagged finding (medical authority, not crafting)

`medical_texts.json` references item id `antiseptic` in 3 treatment
`required_items`, but no item with that id exists (`antiseptic_1l_of_1l`
does). Plan 55 did **not** fix this because the correct resolution — an item
addition or an alias — belongs to the medical/treatment authority. Flagged in
`RECIPE_REACHABILITY_MATRIX.md`.

## Scarcity preservation

Plan-55 medicine crafting adds exactly one low-tier trauma item from scrap
inputs. All advanced medicine remains pharma-lab-gated (station + chemist +
scarce precursors + dependency risk), keeping hospital scavenging, trade, and
medical triage decisions relevant.
