# Plan 77 — Duty Roster Baseline Reconnaissance

> **Status:** Grounded baseline inspection completed 2026-09-03.
> **Authority:** `Assets/Ashfall.Core/DutyRoster/`, `Assets/StreamingAssets/Data/duty_roster_seasons.json`, `src/Host/DutyRosterHostSession.cs`.

---

## 1. Executive Summary

`Assets/StreamingAssets/Data/duty_roster_seasons.json` previously contained **1 season**:
```json
{
  "schema_version": 1,
  "items": [
    {
      "id": "season_second_winter",
      "windowMinDays": 8,
      "windowMaxDays": 12,
      "encounterWeight": 1.6,
      "steamTripChanceBoost": 0.08
    }
  ]
}
```

This single entry covered only days 8–12. Before day 8 and after day 12, no season was defined, leaving the shelter in an unvaried, static temporal state for the vast majority of the campaign.

---

## 2. Codebase Forensics & Field Consumers

1. **`DutyRosterSeasonEntry` (`Assets/Ashfall.Core/DutyRoster/DutyRosterCatalog.cs`):**
   - `id`: unique snake_case string (defaults to `DutyRosterIds.SeasonSecondWinter`).
   - `windowMinDays`: starting day bound (inclusive, defaults to 8).
   - `windowMaxDays`: ending day bound (inclusive, defaults to 12).
   - `encounterWeight`: float multiplier (defaults to 1.6f).
   - `steamTripChanceBoost`: float probability boost (defaults to 0.0f).

2. **`DutyRosterCatalog` (`DutyRosterCatalog.cs`):**
   - Holds `List<DutyRosterSeasonEntry> Seasons`.
   - Populated by `DutyRosterCatalogLoader.LoadList` from `duty_roster_seasons.json`.
   - Exposes `GetSeason(string id)` by unique ID.

3. **`encounterWeight` Consumer:**
   - In `ShelterEncounterSystem.cs`: `SetSecondWinter(float multiplier, int day)` sets `_state.encounterWeightMultiplier = multiplier <= 0f ? 1f : multiplier;`.
   - In `DutyRosterHostSession.cs`: `ActivateSecondWinter()` calls `Encounters.SetSecondWinter(DutyRosterIds.SecondWinterEncounterWeight, Clock.Day)`.
   - Represents a multiplicative scale factor for shelter-internal encounter occurrence during intense pressure phases.

4. **`steamTripChanceBoost` Consumer:**
   - Traced to steam system / brine infrastructure (`BrineWaterSystem.cs`).
   - In `BrineWaterSystem`: A "steam trip" is an emergency shutdown/trip of the shelter's steam generation/filtration plant when membrane integrity drops below `SteamTripIntegrity` (15%).
   - `steamTripChanceBoost` represents an additional probability of steam plant malfunction/trip during severe weather and duty cycles.

5. **Day Window Bounds:**
   - Both `windowMinDays` and `windowMaxDays` are evaluated as **INCLUSIVE** integer days: `day >= windowMinDays && day <= windowMaxDays`.
   - For example, `season_second_winter` with `windowMinDays: 8` and `windowMaxDays: 12` covers days 8, 9, 10, 11, and 12 (5 full days).
