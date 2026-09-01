# Food Spoilage & Storage Pressure Balance

This document balances crop perishability, pantry decay rates, root cellar efficiency, and preservation tradeoffs in `KitchenNutritionSystem` and `GreenhouseSystem`.

---

## 1. Baseline Perishability by Food Type

| Food Category | Examples | Baseline Shelf Life (Ambient) | Root Cellar (10°C) | Refrigerated (2°C) | Preserved (Canned/Dry) |
|---|---|---|---|---|---|
| **Leafy Greens** | Winter Cress, Scurvy-Grass | 3 Days | 7 Days | 14 Days | 60 Days (Fermented) |
| **Fresh Mushrooms** | Spore Caps, Phosphor Caps | 4 Days | 8 Days | 18 Days | 60 Days (Dried) |
| **Fresh Meats / Flesh** | Raw Game, Salvaged Fish | 3 Days | 5 Days | 12 Days | 30–40 Days (Smoked/Salted) |
| **Tubers & Roots** | Greenhouse Tuber, Frost Tuber | 10 Days | 30 Days | 60 Days | 45–50 Days (Pickled/Confit) |
| **Threshed Grains** | Mutated Grain, Ash-Barley | 30 Days | 60 Days | 120 Days | 90 Days (Canned Stew) |
| **Pre-War Wheat** | Clean Golden Wheat | 45 Days | 90 Days | 180 Days | 120 Days (Milled Flour) |
| **Honey / Propolis** | Raw Comb, Tincture | 365 Days | 365 Days | 365 Days | Indefinite (Sugar Matrix) |

---

## 2. Storage Pressure Invariants

1. **Harvest Glut Friction**: Harvesting 4+ plots simultaneously creates immediate pantry storage pressure; unpreserved perishables spoil within days, requiring preservation labor or meal consumption.
2. **Root Cellar Degradation**: Humidity spikes in sub-basement cellars without ventilation promote fungal rot (`RootCellarHumidityRotEntry`), reducing storage efficiency by 30% unless cured with preservation salt.
3. **No Free Infinite Rations**: Preserved items require ongoing storage slots, jars, tins, or salt inputs; players cannot hoard 1000 days of food without material and space investment.
