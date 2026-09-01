# Forecast Accuracy Matrix

> **Authority:** `Assets/Ashfall.Core/WeatherStationSystem.cs`

---

## 1. Station Tier & Confidence Calculation

Forecast confidence decays by distance into the future:
$$\text{Confidence}(i) = \text{Clamp}\left(\text{Accuracy} \times \text{TierMultiplier} \times (1 - 0.12 \times i), 0.10, 1.00\right)$$

| Station Tier | Base Accuracy | Tier Multiplier | Day +0 Confidence | Day +1 Confidence | Day +3 Confidence | Day +6 Confidence |
|---|---|---|---|---|---|---|
| **Offline** | 0.00 | 0.00 | 0.00 | 0.00 | 0.00 | 0.00 |
| **Damaged** | 0.70 | 0.40 | 0.28 | N/A | N/A | N/A |
| **Functional** | 0.70 | 0.75 | 0.53 | 0.46 | 0.34 | N/A |
| **Calibrated** | 0.85 | 0.95 | 0.81 | 0.71 | 0.52 | 0.23 |

---

## 2. Sensor Fault & Degradation Rules

1. **Daily Exposure Wear:** Severe weather (Black Rain, Acid Frost, Fallout Storm) applies degradation to station durability.
2. **Fault Trigger:** If durability falls below 25%, a sensor fault trips (`Anemometer bearing seized and barometer drift`), forcing tier to `Damaged`.
3. **Repair Protocol:** Spending mechanical scrap restores durability and clears faults when durability exceeds 40%.
