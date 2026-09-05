# Hardcore Effective Price Cap Audit

## 1. Audit Objective

Verify that compounding modifiers across Scarcity Tiers, Faction Preferences, and Price Shocks do not cause runaway valuation bugs or overflow price limits in `TradeScreenPresenter` or `MarketSystem`.

---

## 2. Compounding Scenarios & Ceiling Verification

| Scenario | Base Item | Base Trade Value | Scarcity Tier ($M_S$) | Faction Premium ($M_F$) | Price Shock ($M_P$) | Compound Multiplier | Resulting Price | Status |
|:---|:---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| Early Radiation Crisis | `clean_water` | 10 | Critical (2.5x) | The Scale (1.25x) | PlumePassing (1.8x) | 5.625x | 56 | PASS (< 10x) |
| Mid-Game Epidemic | `antibiotics` | 20 | Moderate (1.6x) | The Rebuilders (1.2x) | DiseaseOutbreak (2.0x) | 3.84x | 77 | PASS (< 10x) |
| Border Skirmish War | `ammo_762` | 15 | LateScarcity (1.8x) | The Garrison (1.3x) | FactionConflict (1.7x) | 3.978x | 60 | PASS (< 10x) |
| Deep Winter Diesel | `fuel` | 25 | DeepWinter (2.2x) | The Cutters (1.25x) | FuelShortage (1.9x) | 5.225x | 131 | PASS (< 10x) |
| Theoretical Maximum | `clean_water` | 10 | Critical (2.5x) | High Premium (1.5x) | DiseaseOutbreak (2.0x) | 7.50x | 75 | PASS (< 10x) |

---

## 3. Findings

1. **Upper Bound Safety:** The highest compound multiplier achievable under any legal game state is `7.50x`, safely below the `10.0x` systemic threshold.
2. **Integer Integrity:** All values round cleanly and fit within signed 32-bit integers without truncation or precision anomalies.
3. **No Zero Division / Infinite Cost:** Base values and multipliers are strictly positive ($> 0$).
