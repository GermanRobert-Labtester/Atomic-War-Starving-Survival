# Plan 188 — Daily routines authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_188_Individual_Survivor_Daily_Routines.md`. `src/Main.Plans188*.cs` is mercenary bounties (**number drift**).

---

## 1. Premise

| Concern | Owner | Save |
|---|---|---|
| Shelter phases / curfew / beds | `ShelterScheduleSystem` | `shelter_schedule` |
| Work occupancy | `DutyRosterSystem` | `duty_roster` |
| Needs decay | `NeedsSystem` | via `survivors` (duration, not hour blocks) |
| Rooms | `ShelterAssignmentSystem` | not a timetable |
| Hobbies | `SurvivorDowntimeSystem` | not daily rhythm |
| Sub-day clock | `ISimClock` (60 ticks/hour) | campaign clock; **not** a personal timetable |

`SchedulePhase.Night` has **zero** Core consumers. `routine_templates.json` / `SurvivorRoutineSystem`: **ABSENT**.

---

## 2. Ownership (proposed)

Routine = **schedule extension**. Duty roster is the work-block reader. Do not add a second survivor scheduler.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| `SurvivorRoutineSystem` / `routine_templates.json` | **OUT** |
| Second scheduler racing `shelter_schedule` + `duty_roster` | **OUT** |
| Treating downtime/assignment as the missing contract | **OUT** |
| Hour source = `ISimClock.HourOfDay` (or catalog hours) consumed by schedule + duty | **IN** |
| Satisfaction derived from `NeedsSystem`, not a new persist field | **IN** |

**Next implement:** `DEBT-188-SCHEDULE-HOUR-CONSUMER` — one schedule/duty read of hour-of-day. No per-survivor block ledger.

---

## 4. Evidence paths

`ShelterScheduleSystem.cs`, `Clock/ISimClock.cs`, `DutyRosterSystem.cs`, `NeedsSystem.cs`, `shelter_schedules.json`, `src/Host/DutyRosterHostSession.cs`, `docs/architecture/CLOCK_POLICY.md`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
