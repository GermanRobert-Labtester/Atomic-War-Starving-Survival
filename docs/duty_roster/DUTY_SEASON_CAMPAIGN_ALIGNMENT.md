# Duty Season Campaign Alignment

> **Authority Map:** Alignment between the authoritative campaign clock and derived duty-roster season state.

---

## 1. Authority Map

| Concept | Owning System / Authority | Plan 77 Relationship |
|---|---|---|
| **Campaign Day** | `SimClock` / `IClock` (Campaign calendar) | Read-only input to season selection |
| **Duty Season Definitions** | `duty_roster_seasons.json` / `DutyRosterCatalog` | Authored data authority |
| **Active Duty Season** | Derived via `GetSeasonForDay(day)` | Stateless projection derived from campaign day |
| **Encounter Multiplier** | `ShelterEncounterSystem` | Applies active season's `encounterWeight` |
| **Steam Infrastructure Risk** | `BrineWaterSystem` | Consumes active season's `steamTripChanceBoost` |
| **Save Persistence** | `DutyRosterSaveStore` | Persists `simDay`; active season is re-derived on restore |

---

## 2. Invariant Rules

1. **One Campaign Clock:** No parallel `dutyRosterDay` counter is created. `Clock.Day` is the sole source of temporal truth.
2. **Deterministic Selection:** For any valid integer day, `GetSeasonForDay(day)` returns the exact same season entry regardless of process restarts or frame rates.
3. **Stateless Selection:** Restoring a save instantly updates the active season by evaluating the restored campaign day against the catalog.
