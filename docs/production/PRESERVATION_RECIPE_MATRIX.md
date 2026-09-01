# Preservation Recipe Matrix (10 Preservation Recipes)

This document establishes the 10 core preservation recipes that convert perishable harvests and scavenged meats into stable, long-shelf-life provisions within `KitchenNutritionSystem` and `recipes.json`.

---

## 1. Ten Core Preservation Recipes

| Recipe ID | Recipe Name | Method | Inputs | Time (h) | Station | Output Item ID | Portion Count | Shelf Life (Days) | Spoilage Reduction |
|---|---|---|---|---|---|---|---|---|---|
| `craft_pickled_tubers` | Pickled Greenhouse Tubers | Brining / Crock | Tuber ×4, Salt ×2, Clean Water ×1 | 1.5 | stove | `food_pickled_tubers` | 4 | 45 | +38 days |
| `craft_dried_mushrooms` | Sun/Hearth Dried Mushrooms | Desiccant Drying | Mushroom ×6, Cloth ×1 | 2.0 | stove | `food_dried_mushrooms` | 6 | 60 | +55 days |
| `craft_smoked_meat_rations` | Smoked Game Strips | Hardwood Smoking | Raw Meat ×4, Fuel ×2, Salt ×1 | 3.0 | stove | `food_smoked_meat` | 4 | 30 | +26 days |
| `craft_canned_grain_stew` | Sealed Ash-Grain Potage | Canning / Boiling | Ash-Grain ×3, Tuber ×2, Metal Scrap ×1, Fuel ×2 | 2.5 | stove | `food_canned_grain_stew` | 3 | 90 | +85 days |
| `craft_salted_fish_meat` | Dry-Salted Flesh Strips | Dry Salting | Raw Meat ×5, Preservation Salt ×3 | 1.0 | workbench | `food_salted_meat` | 5 | 40 | +36 days |
| `craft_rendered_fat_confit` | Fat-Sealed Root Confit | Fat Immersion | Tuber ×3, Rendered Fat ×2, Salt ×1 | 2.0 | stove | `food_fat_confit` | 3 | 50 | +43 days |
| `craft_fermented_sauerkraut` | Crock-Lacto Cabbage/Cress | Crock Fermentation | Leafy Green ×5, Salt ×2, Clean Water ×1 | 1.5 | workbench | `food_fermented_sauerkraut` | 5 | 60 | +56 days |
| `craft_honey_preserved_pulp` | Honey-Sealed Fruit/Pulp | Sugar Enclosure | Crop/Fruit ×4, Honey Pot ×1 | 1.0 | stove | `food_honey_preserved_pulp` | 4 | 75 | +70 days |
| `craft_dried_herb_packets` | Dried Herbal Infusion Packets | Desiccant Pack | Medicinal Herb ×4, Paper/Cloth ×1 | 1.0 | workbench | `food_dried_herb_packets` | 4 | 90 | +80 days |
| `craft_brined_legume_mash` | Brined Iron-Pea Paste | Salted Crock Puree | Cold Legume ×4, Salt ×2, Clean Water ×1 | 1.5 | stove | `food_brined_legume_mash` | 4 | 45 | +38 days |

---

## 2. Strategic Tradeoffs

1. **Fuel & Salt Sinks**: Long preservation requires either fuel/heat (smoking, canning, stewing) or chemical inputs (coarse rock salt, rendered fat, honey).
2. **Morale Quality vs Shelf Life**:
   - Fresh Food: High morale bonus (+2 to +8), fast spoilage (3–7 days).
   - Preserved Food: Neutral/modest morale (+0 to +3), long shelf life (30–90 days).
   - Tainted / Spoiled Food: Severe morale penalty (-3 to -6), radiation/illness risk.
