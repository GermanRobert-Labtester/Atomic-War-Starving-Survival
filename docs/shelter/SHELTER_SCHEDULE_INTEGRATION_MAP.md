# Shelter Schedule Integration Map

This document defines the cross-system wiring for the expanded 12 shelter schedules across incidents, seasons, room operations, and the power grid.

---

## 1. Incident System Integration (Plan 57)

Four schedules explicitly react to or are triggered by shelter incidents:

```
[Incident System]
       │
       ├─► incident_shelter_crisis     ──► schedule_emergency_shifts (All-hands repair, 24h active)
       ├─► incident_faction_siege      ──► schedule_siege_watch      (Armed sentinel rotations, override blocked)
       ├─► incident_survivor_fatality  ──► schedule_mourning         (Quiet hours, suspended metalwork)
       └─► incident_epidemic_outbreak  ──► schedule_quarantine       (Hermetic corridor zones, override blocked)
```

- **Incident Trigger Method:** Invoked via `ShelterScheduleSystem.TryActivateScheduleByTrigger(incidentTag)`.
- **Reversion:** When incident resolves, system can revert to `schedule_standard` or previous schedule captured in state.

---

## 2. Seasonal Cadence Integration (19C)

Two schedules represent macro-environmental and cultural phases:

```
[Seasonal Cadence Engine]
       │
       ├─► season_nuclear_winter          ──► schedule_winter_hibernation (08:00-17:00 active, 1.3 fatigue recovery, low lighting)
       └─► season_solstice_commemoration  ──► schedule_festival_day       (06:00-24:00 active, high lighting load, celebration)
```

- **Winter Hibernation:** Minimizes fuel consumption by suppressing daytime lighting (0.35) and curfew lighting (0.20), while maximizing dwell-time sleep efficiency (1.30 fatigue modifier).
- **Festival Day:** Spikes lighting demand (0.80 day / 1.00 night) to celebrate survival, shortening curfew to 01:00–06:00.

---

## 3. Shelter Room & Resource Allocation (Plan 41)

Schedules govern which facilities are unlocked or prioritized:

| Schedule ID | Affected Room | Operational Policy |
|---|---|---|
| `schedule_quarantine` | Medbay & Isolation Quarters | Quarantines medbay; restricts common corridors; disables cross-room dweller movement. |
| `schedule_construction_push` | Workshop & Fabrication Bay | Double-shifts all able dwellers to fabrication benches; increases power draw. |
| `schedule_scout_rotation` | Decon Airlock & Staging Room | Keeps staging bay manned 24/7; ensures immediate expedition turn-around. |
| `schedule_rationing` | Hydroponics & Pantry | Shifts cooking to single daily distribution; orders dwellers into bed rest to cut caloric burn. |

---

## 4. Power Grid System Integration

- **Baseline Demand:** Each schedule dictates `lightingDemandDay`, `lightingDemandNight`, and `lightingDemandCurfew`.
- **Curfew Drop:** During curfew hours, lighting demand drops to `lightingDemandCurfew` (e.g. 0.20–0.40).
- **Emergency Override:** If active, lighting demand drops to `lightingDemandCurfew * 0.5f`.
- **Brownout Multiplier:** If `PowerGridSystem.IsBrownout` is true during `TickDay()`, current lighting demand is halved (`*= 0.5f`).
- **Emergency Override Lockout:** Schedules `schedule_curfew_locked`, `schedule_siege_watch`, and `schedule_quarantine` enforce `allowEmergencyOverride = false`, preventing dwellers or operators from breaking the enforced curfew via override.

---

## 5. Host & UI Surface

- **Godot Host Session:** `ShelterScheduleHostSession` provides reactive event hooks:
  - `OnPhaseChanged` -> updates HUD banner and lighting node presets.
  - `OnScheduleChanged` -> updates `ShelterSchedulePanel` status cards and duty roster.
- **Save/Load:** `ShelterScheduleSaveStore` persists `ShelterScheduleState` with `activeScheduleId`, ensuring that reloading a game preserves the active schedule across sessions.
