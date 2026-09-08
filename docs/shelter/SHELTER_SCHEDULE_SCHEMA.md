# Shelter Schedule Schema Specification

**Document Version:** 1.0
**Authority:** `Assets/StreamingAssets/Data/shelter_schedules.json`
**Core Types:** `Ashfall.Core.ScheduleDefinition`, `Ashfall.Core.ShelterScheduleState`
**Host Binding:** `AtomicWar.GodotApp.ShelterScheduleHostSession`

---

## 1. Top-Level Container Structure

```json
{
  "schema_version": 1,
  "collection_id": "shelter_schedules",
  "schedules": [
    { ... }
  ]
}
```

| Property | Type | Description |
|---|---|---|
| `schema_version` | integer | Data contract schema revision (currently `1`). |
| `collection_id` | string | Constant identifier (`"shelter_schedules"`). |
| `schedules` | array | Array of `ScheduleDefinition` entries (exactly 12). |

---

## 2. Schedule Definition Item Grammar

Each entry in the `schedules` array adheres to the following specification:

| Field | JSON Key | Type | Constraints | Description |
|---|---|---|---|---|
| `schedule_id` | `schedule_id` | string | `schedule_[a-z0-9_]+` | Unique identifier used by systems and saves. |
| `display_name` | `display_name` | string | Non-empty | Localized UI display name. |
| `day_start_hour` | `day_start_hour` | float | `[0.0, 24.0]` | Clock hour when the active daytime phase begins. |
| `day_end_hour` | `day_end_hour` | float | `[0.0, 24.0]` | Clock hour when active daytime duty ends. |
| `curfew_start_hour` | `curfew_start_hour` | float | `[0.0, 24.0]` | Clock hour when curfew restrictions begin. |
| `curfew_end_hour` | `curfew_end_hour` | float | `[0.0, 24.0]` | Clock hour when curfew restrictions lift. |
| `fatigue_recovery_modifier` | `fatigue_recovery_modifier` | float | `[0.5, 1.5]` | Rest modifier applied to dweller fatigue recovery. |
| `lighting_demand_day` | `lighting_demand_day` | float | `[0.1, 1.0]` | Baseline grid lighting demand during day phase. |
| `lighting_demand_night` | `lighting_demand_night` | float | `[0.1, 1.0]` | Grid lighting demand during night / watch phase. |
| `lighting_demand_curfew` | `lighting_demand_curfew` | float | `[0.1, 1.0]` | Grid lighting demand during active curfew. |
| `allow_emergency_override` | `allow_emergency_override` | bool | `true / false` | Whether emergency manual override is permitted. |
| `shift_pattern` | `shift_pattern` | string | Enum (see below) | Duty assignment staffing pattern. |
| `trigger_condition` | `trigger_condition` | string | Snake_case tag | Condition or incident tag activating this schedule. |
| `description` | `description` | string | Non-empty prose | Grounded narrative flavor describing the operational rhythm. |

---

## 3. Shift Patterns

- `single_shift`: Standard day-duty roster with evening leisure and night curfew.
- `double_shift`: Two complementary rotations sharing facilities and beds.
- `triple_shift`: 24-hour continuous watch with 8-hour staggered rotations.
- `all_hands`: Emergency surge mobilizing all capable survivors regardless of fatigue.
- `skeleton_crew`: Minimum maintenance personnel active; remaining dwellers confined.

---

## 4. Trigger Categories

1. **Incidents (Plan 57):**
   - `incident_shelter_crisis` -> `schedule_emergency_shifts`
   - `incident_faction_siege` -> `schedule_siege_watch`
   - `incident_survivor_fatality` -> `schedule_mourning`
   - `incident_epidemic_outbreak` -> `schedule_quarantine`
2. **Seasonal Cadence (19C):**
   - `season_nuclear_winter` -> `schedule_winter_hibernation`
   - `season_solstice_commemoration` -> `schedule_festival_day`
3. **Shelter Rooms & Logistics (Plan 41):**
   - `room_workshop_expansion` -> `schedule_construction_push`
   - `shortage_food_reserves` -> `schedule_rationing`
   - `expedition_active_sorties` -> `schedule_scout_rotation`
4. **Baseline Postures:**
   - `default` -> `schedule_standard`
   - `surface_heat_or_radiation` -> `schedule_night_shift`
   - `civil_unrest_or_breach` -> `schedule_curfew_locked`
