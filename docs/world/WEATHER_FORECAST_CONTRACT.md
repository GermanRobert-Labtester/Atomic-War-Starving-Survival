# Weather Forecast Contract

> **Authority:** `Assets/Ashfall.Core/World/WeatherSystem.cs`, `Assets/Ashfall.Core/WeatherStationSystem.cs`

---

## 1. Architectural Contract

1. **Non-Mutating Lookahead:** Forecasting queries `WeatherSystem.PeekForecast(horizon)` which derives future seeded rolls using `unchecked(_seed * 397 + (_state.rollCount + i))` without advancing `_state.rollCount` or changing the realized RNG sequence.
2. **Single Realization Path:** When `WeatherSystem.Tick()` advances time, it rolls using the identical sequence formula. A forecasted weather state with 100% confidence will always match realized weather.
3. **Horizon Bounds:** The maximum forecast horizon is bounded between 0 and 7 days based on station tier.

---

## 2. Forecast DTO Schema

```csharp
[Serializable]
public sealed class ForecastEntry
{
    public int day;
    public WeatherKind weather;
    public float confidence;
    public bool isRouteSafe;
    public float temperature;
    public string warning;
    public string preparationPayoff;
    public string atmosphericFlavor;
}
```

---

## 3. Station Quality Tiers

| Tier | Required State | Forecast Horizon | Max Confidence | Precision |
|---|---|---|---|---|
| **Offline** | Not installed or durability 0 | 0 days | 0.00 | None |
| **Damaged** | Durability < 40 or sensor fault | 1 day | 0.40 | Coarse |
| **Functional** | Installed, uncalibrated, durability >= 40 | 3 days | 0.75 | Standard |
| **Calibrated** | Installed, calibrated, durability >= 40 | 7 days | 0.95 | High |
