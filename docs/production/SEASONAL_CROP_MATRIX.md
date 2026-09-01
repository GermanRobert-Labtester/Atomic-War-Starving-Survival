# Seasonal Crop Matrix (Plan 19 Integration)

Agricultural productivity in ASHFALL connects directly to the authoritative seasonal state (`Plan 19` Seasonal Dynamics / `ISeasonalStateProvider`). Unfavorable seasons impose growth penalties or water demand surges, while optimal seasons yield bonus harvest and faster cycles.

---

## 1. Season Phase Performance

| Crop ID | Display Name | Spring (Thaw) | Summer (Radiant) | Autumn (Ash Fall) | Winter (Deep Freeze) | Best Strategy |
|---|---|---|---|---|---|---|
| `crop_mushroom` | Spore Mushroom | Neutral (+0%) | Neutral (+0%) | Neutral (+0%) | **Favored (+15%)** | Constant sub-basement production |
| `crop_tuber` | Greenhouse Tuber | Neutral (+0%) | Good (+10%) | **Favored (+20%)** | Good (+10%) | Late season bulk calorie bank |
| `crop_grain` | Mutated Grain | Good (+10%) | **Favored (+25%)** | Neutral (+0%) | Penalty (-25% growth) | High-sun summer staple |
| `crop_wheat` | Pre-War Wheat | Neutral (+0%) | **Favored (+30%)** | Penalty (-20%) | Hard Lock (Needs Heat) | Summer luxury morale crop |
| `crop_hardy_tuber` | Frost Tuber | Neutral (+0%) | Neutral (+0%) | Good (+10%) | **Favored (+25%)** | Deep freeze winter emergency buffer |
| `crop_ash_grain` | Ash-Barley | Good (+10%) | Good (+10%) | **Favored (+25%)** | Neutral (+0%) | Heavy fallout / ash storm resilience |
| `crop_biolum_mushroom` | Phosphor Cap | Neutral (+0%) | Neutral (+0%) | Neutral (+0%) | **Favored (+20%)** | Lightless bunker cultivation |
| `crop_nutrient_algae` | Chlorella Slurry | **Favored (+25%)** | **Favored (+25%)** | Penalty (-15%) | Penalty (-35% growth) | Warm-weather fast turnaround protein |
| `crop_medicinal_herb` | Yarrow / Fever-Bark| **Favored (+30%)** | Good (+15%) | Neutral (+0%) | Penalty (-20%) | Spring medical stock building |
| `crop_leafy_green` | Winter Cress | **Favored (+35%)** | Good (+10%) | Neutral (+0%) | Good (+15%) | Post-winter scurvy remediation |
| `crop_oilseed` | Sun-Flax | Good (+15%) | **Favored (+30%)** | Penalty (-15%) | Penalty (-40%) | Peak summer oil harvest |
| `crop_cold_legume` | Iron Pea | Good (+10%) | Neutral (+0%) | **Favored (+20%)** | **Favored (+20%)** | Late-season protein & soil fixing |

---

## 2. Mitigation Strategies

1. **Winter Heating Allocation**: Heating greenhouse bays with bunker thermal radiators cancels the Winter growth penalty on `crop_grain` and `crop_wheat`.
2. **Grow-Lamp Boost**: Operating high-draw grow lamps during Autumn/Winter offsets low natural irradiance.
3. **Crop Rotation**: Rotating legumes (`crop_cold_legume`) after heavy feeders (`crop_wheat`, `crop_grain`) reduces soil contamination buildup by 15%.
