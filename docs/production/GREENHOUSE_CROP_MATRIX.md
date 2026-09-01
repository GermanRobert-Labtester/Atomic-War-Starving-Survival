# Greenhouse Crop Matrix (12 Crops / Cultures)

This document establishes the canonical 12-crop agricultural roster for ASHFALL's greenhouse simulation (`GreenhouseSystem`), their 5-stage lifecycle parameters, water and light requirements, baseline yields, blight resistance, contamination tolerances, and nutritional/preservation roles.

---

## 1. Complete Crop Roster

| Crop ID | Display Name | Seed Item ID | Clean Yield ID | Tainted Yield ID | Growth (h) | Water/Day (L) | Light/Day (h) | Base Yield | Blight Res. | Contam. Tol. | Seasonal Best | Preservation / Culinary Role |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `crop_mushroom` | Spore Mushroom | `item_seed_mushroom` | `crop_mushroom` | `tainted_food` | 96 | 8 | 4 | 2 | 0.85 | 60 | Deep Winter / Any | Quick harvest, drying, soup thickener |
| `crop_tuber` | Greenhouse Tuber | `item_seed_tuber` | `crop_tuber` | `tainted_food` | 144 | 12 | 6 | 3 | 0.70 | 45 | Autumn / Winter | Calorie base, root cellar, pickling |
| `crop_grain` | Mutated Grain | `item_seed_grain` | `crop_grain` | `tainted_food` | 192 | 16 | 8 | 4 | 0.55 | 30 | Spring / Summer | Bread staple, brewing, grain porridge |
| `crop_wheat` | Pre-War Wheat | `item_seed_wheat` | `crop_wheat` | `tainted_food` | 240 | 20 | 10 | 6 | 0.40 | 20 | High Summer | Premium flour, high-morale baking |
| `crop_hardy_tuber` | Frost Tuber (Black Rutabaga) | `item_seed_hardy_tuber` | `crop_hardy_tuber` | `tainted_food` | 120 | 10 | 5 | 3 | 0.80 | 55 | Deep Winter | Cold-hardy staple, long-term root storage |
| `crop_ash_grain` | Ash-Barley (Soot Rye) | `item_seed_ash_grain` | `crop_ash_grain` | `tainted_food` | 168 | 14 | 6 | 4 | 0.65 | 40 | Autumn / Ash Storm | Soot-resistant flour, dense bread mash |
| `crop_biolum_mushroom` | Phosphor Cap Fungi | `item_seed_biolum_mushroom` | `crop_biolum_mushroom` | `tainted_food` | 84 | 6 | 2 | 2 | 0.90 | 70 | Any (Zero Light) | Sub-basement culture, sedative/pharma base |
| `crop_nutrient_algae` | Chlorella Slurry Basin | `item_seed_nutrient_algae` | `crop_nutrient_algae` | `tainted_food` | 72 | 18 | 8 | 5 | 0.75 | 50 | Spring / Summer | Protein supplement, emergency famine paste |
| `crop_medicinal_herb` | Yarrow & Fever-Bark | `item_seed_medicinal_herb` | `crop_medicinal_herb` | `tainted_food` | 160 | 12 | 7 | 3 | 0.60 | 35 | Spring | Dispensary antipyretic, dried tea packets |
| `crop_leafy_green` | Winter Cress & Scurvy-Grass | `item_seed_leafy_green` | `crop_leafy_green` | `tainted_food` | 60 | 10 | 6 | 3 | 0.50 | 25 | Spring / Early Summer | Rapid harvest, fresh vitamin, anti-scurvy |
| `crop_oilseed` | Sun-Flax & Rad-Mustard | `item_seed_oilseed` | `crop_oilseed` | `tainted_food` | 210 | 15 | 9 | 4 | 0.55 | 30 | Summer | Lamp oil, pressing, rendered fat confit |
| `crop_cold_legume` | Iron Pea & Dun Vetch | `item_seed_cold_legume` | `crop_cold_legume` | `tainted_food` | 150 | 11 | 6 | 4 | 0.75 | 40 | Late Autumn / Winter | Protein legume, nitrogen fixation, brining |

---

## 2. Five-Stage Growth Lifecycle

Every crop transitions deterministically through the 5 canonical greenhouse stages:

1. **Stage 0 (Fallow)**: Plot empty; requires seed planting, tilled soil, and initial hydration.
2. **Stage 1 (Sprouting)**: 0% to 33% of `GrowthHoursToMature`. Vulnerable to drought; requires minimum water.
3. **Stage 2 (Growing)**: 33% to 90% of `GrowthHoursToMature`. Rapid biomass accumulation; light/power draw active.
4. **Stage 3 (Mature)**: 100% growth reached. Harvestable. Holds for up to 48 hours before quality degradation.
5. **Stage 4 (Failed)**: Soil contamination exceeded tolerance OR blight reached critical threshold without treatment. Harvest yields `tainted_food` or 0 units.
