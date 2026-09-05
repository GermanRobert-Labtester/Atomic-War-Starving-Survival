# Dose Location Weather Handoff

> **Integration:** Contract defining how dynamic weather systems (e.g. fallout storms, ash clouds, blizzards) interact with static location dose baselines.

---

## 1. Architectural Authority Separation

- **Static Authority:** `dose_locations.json` defines the **unmodified geographic baseline** of the world. It contains zero weather modifiers and must never be mutated at runtime.
- **Dynamic Authority:** `WeatherSystem` and `FalloutSystem` manage environmental states, precipitation, wind vectors, and fallout deposition.
- **Runtime Composition:** The effective environmental rate is calculated at runtime as:
  $$\text{EffectiveRate} = \text{BaselineRate} \times \text{SectorWeatherMultiplier} + \text{AcuteFalloutFlux}$$

---

## 2. Sector Weather Susceptibility

| Sector | Shielding Multiplier Against Fallout Storms | Notes |
|---|:---:|---|
| `bunker` | **0.0×** (Impervious) | Sealed subterranean concrete and lead baffles isolate internal living areas from atmospheric fallout storms. Baseline stays 0.02 µSv/h. |
| `surface` | **2.5× to 5.0×** | Direct exposure to airborne particles; heavy dust deposition dramatically increases exterior apron and water intake readings. |
| `external` | **3.0× to 6.0×** | High wind exposure across ridges and marshland carries active particulate, increasing exposure significantly. |
| `expedition` | **2.0× to 4.0×** | Elevated baseline increases further; outdoor scavenging during an active fallout storm rapidly pushes badges toward Amber thresholds. |
| `faction` | **1.5× to 3.0×** | Partial revetments and sandbags provide limited deflection, but dust churn remains hazardous. |

---

## 3. Post-Storm Recovery

Once weather clears:
- Dynamic multipliers return to 1.0×.
- Baseline catalog values remain completely intact and unaltered.
