# Duty Season Weather Alignment

> **Meteorological Distinction:** Separation of duty roster operational phases from weather simulation seasons.

---

## 1. Duty Season vs. Weather Season

- **Duty Roster Seasons:** Workload, encounter density, and internal shelter operational rhythm phases.
- **Weather Seasons (`WeatherSystem` / `weather_seasons.json`):** Atmospheric temperature, blizzard frequency, precipitation, and radiation fallout.
- **No Duplicate Weather State:** `duty_roster_seasons.json` does not author temperature, snow, or radiation levels.

---

## 2. Alignment Matrix

| Duty Season Window | Days | Weather System Alignment | Coherence Notes |
|---|---|---|---|
| `season_first_ashfall` | 0–7 | Immediate Post-Detonation Ashfall | Heavy particulate fallout, high confusion |
| `season_second_winter` | 8–12 | Early Nuclear Winter Cold Snap | Sharp temperature drop, freezing pipes |
| `season_settling` | 13–30 | Stable Nuclear Winter | Sub-zero baseline, regular indoor shifts |
| `season_spring_thaw` | 31–60 | Seasonal Runoff & Atmospheric Shift | Relative warming, meltwater influx |
| `season_faction_pressure` | 61–120 | Temperate / Ash-Summer | Clearer surface routes, contested roads |
| `season_first_siege` | 121–180 | Dust Storms & Dry Winds | Reduced surface visibility, shelter lockdown |
| `season_consolidation` | 181–240 | Early Autumn Chills | Dropping temperatures, preparation for freeze |
| `season_long_winter` | 241–365 | Deep Second-Year Nuclear Winter | Severe sustained blizzards, maximum heating draw |
