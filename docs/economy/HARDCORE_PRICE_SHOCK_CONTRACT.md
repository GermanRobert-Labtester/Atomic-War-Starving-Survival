# Hardcore Price Shock Contract

## 1. Event Model & Lifecycle

Price shocks are temporary market spikes triggered by world events. They are evaluated through `IPriceShockProvider.TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule)`:

```csharp
public bool TryGetPriceShock(PriceShockKind kind, int dayOffsetFromShockStart, out PriceShockRule rule)
{
    rule = default;
    if (dayOffsetFromShockStart < 0) return false;
    foreach (var r in _bundle.PriceShockRules)
    {
        if (r.Kind == kind && dayOffsetFromShockStart < r.DurationDays)
        {
            rule = r;
            return true;
        }
    }
    return false;
}
```

---

## 2. Catalog of Price Shocks

| Kind (`PriceShockKind`) | Multiplier | Duration (Days) | Affected Goods | Diegetic In-World Trigger |
|:---|:---:|:---:|:---|:---|
| `PlumePassing` | 1.8x | 3 | `*` (All trade goods) | Fallout storm crosses an active trade route, driving emergency panic buying. |
| `ConvoyAmbush` | 1.6x | 3 | `fuel`, `canned_food`, `medical_kit` | Major supply convoy intercepted and destroyed by raider warbands. |
| `FactionConflict` | 1.7x | 5 | `ammo_*`, `medical_kit`, `fuel` | Active armed border skirmish erupts between regional factions. |
| `SeasonalScarcity` | 1.5x | 7 | `canned_food`, `clean_water`, `seed_packets` | Sudden unseasonable blizzard disrupts foraging and greenhouse transport. |
| `DiseaseOutbreak` | 2.0x | 4 | `antibiotics`, `medical_kit`, `clean_water` | Pathogen contagion reported in crowded settlement tenements. |
| `FuelShortage` | 1.9x | 3 | `fuel`, `engine`, `scrap_mechanical` | Refinery pump failure or distribution pipeline sabotage stalls fuel tankers. |

---

## 3. Duration & Decay Behavior

- **Start of Event (`dayOffset = 0`):** Shock is fully active; multiplier is returned.
- **Mid-Event (`0 <= dayOffset < durationDays`):** Shock remains fully active.
- **Expiration (`dayOffset >= durationDays`):** `TryGetPriceShock` returns `false`, gracefully reverting prices to standard tier conditions.
