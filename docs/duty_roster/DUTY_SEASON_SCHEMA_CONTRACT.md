# Duty Season Schema Contract

> **Schema Authority:** `Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs` (`DutyRosterSeasonEntry`) and `Assets/StreamingAssets/Data/duty_roster_seasons.json`.

---

## 1. JSON Schema

```json
{
  "schema_version": 1,
  "items": [
    {
      "id": "season_example",
      "windowMinDays": 0,
      "windowMaxDays": 7,
      "encounterWeight": 1.5,
      "steamTripChanceBoost": 0.02
    }
  ]
}
```

---

## 2. Field Specifications

| Field Name | Type | Required | Default | Valid Range / Description |
|---|---|---|---|---|
| `id` | `string` | **Yes** | `"season_second_winter"` | Unique snake_case identifier with `season_` prefix |
| `windowMinDays` | `int` | **Yes** | `8` | Start day of the season window (inclusive), `windowMinDays >= 0` |
| `windowMaxDays` | `int` | **Yes** | `12` | End day of the season window (inclusive), `windowMaxDays >= windowMinDays` |
| `encounterWeight` | `float` | **Yes** | `1.6` | Encounter frequency multiplier `[0.5, 2.5]` |
| `steamTripChanceBoost` | `float` | **Yes** | `0.0` | Additive steam infrastructure malfunction risk boost `[0.0, 0.15]` |

---

## 3. Deserializer Rules & Invariants

- Deserialized via `CatalogLocator.LoadWrappedList<DutyRosterSeasonEntry>` under the `"items"` wrapper property.
- All 8 seasons must have strictly non-overlapping, ascending, continuous day intervals.
- For contiguous coverage: `season[i].windowMinDays == season[i-1].windowMaxDays + 1`.
