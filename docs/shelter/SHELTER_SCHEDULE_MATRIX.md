# Shelter Schedule Matrix (12 Duty Rhythms)

This document catalogs the complete suite of 12 authoritative schedules defined in `Assets/StreamingAssets/Data/shelter_schedules.json`.

---

## Complete Schedule Roster

| ID | Display Name | Active Hours | Curfew Hours | Fatigue Mod | Light (D/N/C) | Override | Pattern | Trigger Condition |
|---|---|---|---|---|---|---|---|---|
| `schedule_standard` | Standard Rotation | 06:00–22:00 | 22:00–06:00 | 1.00 | 0.50 / 0.80 / 0.30 | Yes | `single_shift` | `default` |
| `schedule_night_shift` | Night Rotation | 18:00–06:00 | 06:00–18:00 | 0.90 | 0.60 / 0.70 / 0.40 | Yes | `double_shift` | `surface_heat_or_radiation` |
| `schedule_curfew_locked` | Locked-Down Curfew | 08:00–20:00 | 20:00–08:00 | 1.20 | 0.40 / 0.90 / 0.20 | No | `skeleton_crew` | `civil_unrest_or_breach` |
| `schedule_emergency_shifts` | Emergency Shifts | 00:00–24:00 | 22:00–06:00 | 0.60 | 0.80 / 0.90 / 0.60 | Yes | `all_hands` | `incident_shelter_crisis` |
| `schedule_siege_watch` | Siege Watch | 00:00–24:00 | 20:00–06:00 | 0.70 | 0.70 / 0.90 / 0.50 | No | `triple_shift` | `incident_faction_siege` |
| `schedule_winter_hibernation` | Winter Hibernation | 08:00–17:00 | 18:00–08:00 | 1.30 | 0.35 / 0.65 / 0.20 | Yes | `skeleton_crew` | `season_nuclear_winter` |
| `schedule_mourning` | Mourning Schedule | 07:00–19:00 | 19:00–07:00 | 1.15 | 0.30 / 0.60 / 0.20 | Yes | `skeleton_crew` | `incident_survivor_fatality` |
| `schedule_festival_day` | Festival Day | 06:00–24:00 | 01:00–06:00 | 0.85 | 0.80 / 1.00 / 0.40 | Yes | `all_hands` | `season_solstice_commemoration` |
| `schedule_rationing` | Rationing Schedule | 09:00–17:00 | 18:00–08:00 | 1.25 | 0.30 / 0.50 / 0.20 | Yes | `single_shift` | `shortage_food_reserves` |
| `schedule_quarantine` | Quarantine Schedule | 07:00–21:00 | 21:00–07:00 | 1.00 | 0.50 / 0.75 / 0.30 | No | `double_shift` | `incident_epidemic_outbreak` |
| `schedule_construction_push` | Construction Push | 05:00–23:00 | 23:00–05:00 | 0.75 | 0.75 / 0.90 / 0.40 | Yes | `double_shift` | `room_workshop_expansion` |
| `schedule_scout_rotation` | Scout Rotation | 04:00–22:00 | 22:00–04:00 | 0.95 | 0.60 / 0.80 / 0.35 | Yes | `triple_shift` | `expedition_active_sorties` |

---

## Detailed Narrative Profiles

### 1. `schedule_standard` — Standard Rotation
- **Baseline Preserved:** Yes
- **Operational Intent:** Predictable, sustainable baseline balancing workshops, mess hall dining, and 8 hours of quiet sleep.
- **Atmosphere:** Orderly hum of generators; dwellers assemble for breakfast and work call.

### 2. `schedule_night_shift` — Night Rotation
- **Baseline Preserved:** Yes
- **Operational Intent:** Inverts diurnal rhythm to operate equipment when exterior ambient temperatures and surface radiation fall.
- **Atmosphere:** Blacked-out observation slits; night crews move quietly under red auxiliary strips while others sleep.

### 3. `schedule_curfew_locked` — Locked-Down Curfew
- **Baseline Preserved:** Yes
- **Operational Intent:** Strictly isolates corridors to quench riots, infiltration rumors, or minor security breaches. Emergency override disabled.
- **Atmosphere:** Heavy bulkheads sealed; dwellers whisper in bunk cubicles while security personnel inspect doors.

### 4. `schedule_emergency_shifts` — Emergency Shifts
- **Operational Intent:** Unrestricted 24/7 surge during catastrophic failures (sump flood, fire, scrubber breach).
- **Atmosphere:** Continuous klaxons, soot-covered repair parties running tools in relays, brief catnaps on ductwork.

### 5. `schedule_siege_watch` — Siege Watch
- **Operational Intent:** Military readiness against hostile warlords or surface raiders. Armed watches rotated every 8 hours. Emergency override locked.
- **Atmosphere:** Tense silence; weapon racks open, sentries standing at periscopes and airlock interlocks.

### 6. `schedule_winter_hibernation` — Winter Hibernation
- **Operational Intent:** Extreme fuel conservation during nuclear winter blizzards. Non-essential sectors unheated and unlit.
- **Atmosphere:** Dwellers huddled beneath double blankets; frost clinging to bulkhead seams; kitchen serving single warm gruel pots.

### 7. `schedule_mourning` — Mourning Schedule
- **Operational Intent:** Cultural pause following survivor death. Forbids loud fabrication; encourages communal memorial.
- **Atmosphere:** Soft murmurs near the memorial wall; candles burning in ration tins; metalwork silent for a full day.

### 8. `schedule_festival_day` — Festival Day
- **Operational Intent:** Controlled morale celebration during holidays, solstices, or major milestones. High power consumption allowed.
- **Atmosphere:** Music playing on vinyl phonographs; mess hall crowded with communal singing and preserved delicacy rations.

### 9. `schedule_rationing` — Rationing Schedule
- **Operational Intent:** Metabolic minimization during catastrophic food scarcity. Short working shifts to reduce calorie burn.
- **Atmosphere:** Lethargic atmosphere; lights dimmed to resting levels; dwellers encouraged to remain supine in bunks.

### 10. `schedule_quarantine` — Quarantine Schedule
- **Operational Intent:** Infectious disease containment. Corridors zoned; medbay sealed; meal delivery conducted via pneumatic dispatch or airlocks. Emergency override disabled.
- **Atmosphere:** Masked sentries spraying chlorine wash; yellow chalk markings at threshold lines; dweller separation strictly maintained.

### 11. `schedule_construction_push` — Construction Push
- **Operational Intent:** Rapid expansion or emergency fortification push. High lighting demand and intensive shifts.
- **Atmosphere:** Clatter of hydraulic presses, welding arc flashes reflecting across bare rock, double-shift mess lines.

### 12. `schedule_scout_rotation` — Scout Rotation
- **Operational Intent:** Staggered staging for surface expeditions. Pre-dawn departures and late decontamination intake.
- **Atmosphere:** Radios crackling in staging airlocks; gear checks conducted under low blue lamps; scouts resting between sorties.
