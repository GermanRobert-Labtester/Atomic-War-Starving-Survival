# Dose Location Risk Matrix

> **Analysis:** Evaluation of the authored 0–8 risk scale, visual communication, and correlation with numerical dose rates.

---

## 1. Risk Tier Definitions

| Risk Level | Tier Designation | Typical Range (µSv/h) | Player Semantic | Sector Coverage |
|:---:|---|---|---|---|
| **0** | Shielded Living Area | 0.01 – 0.05 | Safe for continuous 24/7 occupancy. | `bunker` (5 rooms) |
| **1** | Controlled Transition | 0.50 – 1.00 | Immediate threshold; caution advised for prolonged stays. | `surface` (exterior apron) |
| **2** | Exposed Utility | 1.00 – 2.50 | Routine outdoor work area; dosimeter will show steady rise. | `surface` (observation post) |
| **3** | Contaminated Infrastructure | 2.50 – 5.00 | Significant hazard; work should be scheduled in shifts. | `surface` (water access), `faction` (checkpoint) |
| **4** | Hazardous Travel Corridor | 5.00 – 10.00 | Travel route with substantial cumulative burden over hours. | `external` (wetland, ridge) |
| **5** | Hot Perimeter | 10.00 – 20.00 | Severe fallout accumulation; requires PPE or rapid transit. | `expedition` (forest edge) |
| **6** | Severe Industrial / Urban Ruins | 20.00 – 35.00 | High radiological danger; rapid scavenger entry/exit. | `expedition` (hospital grounds) |
| **7** | Dangerous Hot Zone | 35.00 – 60.00 | Extreme radiation area; anti-rad medication mandatory. | `expedition` (military depot) |
| **8** | Extreme Ground Zero / Core | 60.00 – 80.00+ | Critical life hazard; reserved for ground zero anomalies. | *(Reserved for special events)* |

---

## 2. Correlation Analysis

1. **Non-Mechanical Independence:** In `DoseContentCatalog`, `riskLevel` is an int representing authoring/display severity, while `radiationUsv` is the physical exposure parameter consumed by accumulation math.
2. **Environmental Divergence Justified:**
   - `loc_garrison_checkpoint_gamma_exterior` has Risk 3 with 4.10 µSv/h: Moderate radiological elevation from churned vehicle dust, even though the faction patrol itself may be high combat danger.
   - `loc_frozen_wetland_crossing` has Risk 4 with 6.20 µSv/h: Moderate hourly dose, but because crossing takes many hours over ice, the operational risk tier is elevated to 4 to warn the player.
