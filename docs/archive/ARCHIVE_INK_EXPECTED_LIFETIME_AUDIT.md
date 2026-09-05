# Archive Ink Expected Lifetime Audit

> **Longitudinal Projection:** Legibility decay curves over standard campaign milestones (Day 0, 30, 90, 180, 365) and days until unreadable threshold (< 0.20).

---

## 1. Projected Legibility by Campaign Day

$$\text{Legibility}(t) = \max(0, L_0 - F \times t)$$

| Formulation | Day 0 | Day 30 | Day 90 | Day 180 | Day 365 | Longevity Limit | Days until Unreadable (< 0.20) |
|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| `ink_blood_emergency` | 0.40 | 0.10 | 0.00 | 0.00 | 0.00 | 100d | **20 days** |
| `ink_berry_juice` | 0.50 | 0.29 | 0.00 | 0.00 | 0.00 | 150d | **43 days** |
| `ink_improvised_pigment`| 0.55 | 0.37 | 0.01 | 0.00 | 0.00 | 180d | **58 days** |
| `ink_mineral_oxide` | 0.60 | 0.45 | 0.15 | 0.00 | 0.00 | 220d | **80 days** |
| `ink_lampblack` | 0.65 | 0.53 | 0.29 | 0.00 | 0.00 | 250d | **112 days** |
| `ink_sepia` | 0.70 | 0.58 | 0.34 | 0.00 | 0.00 | 280d | **125 days** |
| `ink_diluted_toner` | 0.75 | 0.66 | 0.48 | 0.21 | 0.00 | 350d | **183 days** |
| `ink_chemical_marker` | 0.80 | 0.71 | 0.53 | 0.26 | 0.00 | 400d | **200 days** |
| `ink_plant_dye` | 0.60 | 0.54 | 0.42 | 0.24 | 0.00 | 200d | **200 days** |
| `ink_soot_lamp` | 0.70 | 0.655| 0.565| 0.43 | 0.15 | 300d | **300 days** |
| `ink_archival_carbon` | 0.95 | 0.92 | 0.86 | 0.77 | 0.585| 600d | **600 days** |
| `ink_iron_gall` | 0.90 | 0.876| 0.828| 0.756| 0.608| 500d | **500 days** |

---

## 2. Strategic Insights

- Emergency formulations (`blood`, `berry`) are strictly for fast short-horizon records that must be acted upon within 1–2 game months.
- Standard formulations (`lampblack`, `sepia`, `diluted_toner`) comfortably span seasonal boundaries (3–6 months).
- Archival formulations (`iron_gall`, `archival_carbon`) easily endure across the full 365-day campaign arc, retaining > 0.58 legibility at Day 365.
