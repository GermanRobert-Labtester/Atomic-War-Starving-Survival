# Plan 45 — Trigger Matrix

## How Patrols Are Selected

Patrol encounters use the existing `TravelEncounterSystem.SelectEncounter()` API:

```csharp
SelectEncounter(string region, float dangerLevel, string stance, string currentSeason, int currentDay, ISeededRng rng)
```

## Eligibility Filters
1. **Cooldown**: 5-day cooldown after resolution
2. **Danger range**: `dangerLevel` must be within `[min_danger_level, max_danger_level]`
3. **Region filter**: `region_tags` must contain current region or "all"
4. **Season filter**: `season_tags` must contain current season or "all"
5. **Weight**: `base_weight × stance_weights[stance]`

## Territory Context
Territory state is encoded in `region_tags` and `territory_state` fields:
- Controlled territory patrols use low-moderate danger levels (0.5-2.5)
- Contested territory patrols use moderate-high danger levels (1.5-5.0)
- Border patrols use moderate danger levels (1.0-3.5)

The `WarlordDoctrineSystem.TravelDangerModifier()` delegate increases encounter chance in controlled/contested territory, making patrols more likely where factions operate.
