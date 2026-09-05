# Duty Season Schedule Integration

> **Schedule Seams:** Interaction between duty season phases and Plan 70 shelter schedules (`shelter_schedules.json`).

---

## 1. Context Provider, Not Schedule Owner

- **Authority:** `ShelterScheduleSystem` (Plan 70) owns schedule presets (e.g. `schedule_emergency_rationing`, `schedule_deep_winter_shift`, `schedule_normal_operations`).
- **Integration Seam:** The schedule system inspects `DutyRosterCatalog.GetSeasonForDay(day)` to recommend suitable operational profiles:
  - During `season_first_ashfall` and `season_second_winter`: Suggests emergency and short-shift routines.
  - During `season_first_siege`: Suggests defensive watch and curfew routines.
  - During `season_long_winter`: Recommends energy-saving and warmth-preserving schedules.
- **Strict Invariant:** `duty_roster_seasons.json` does not embed schedule IDs or override player-chosen schedules directly.
