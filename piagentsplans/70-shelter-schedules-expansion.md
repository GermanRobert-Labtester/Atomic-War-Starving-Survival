# Plan 70 — Shelter Schedules Expansion (3 → 12 duty schedules)

## Goal (2 lines)
Expand `shelter_schedules.json` from 3 verified entries to 12 duty schedules. The shelter
schedule system defines day/night cycles, curfew hours, shift rotations, and rest periods
for survivors. 3 schedules (standard, emergency, siege) is too few — the shelter should
adapt its rhythm to the campaign phase, morale level, and external threats.

## Why (P2)
- Verified: `shelter_schedules.json` has 3 entries (schedule_id, display_name,
  day_start_hour, day_end_hour, curfew_start_hour, curfew_end_hour). The schedule system
  feeds the duty-roster system (existing 12B) and shelter-as-character pillar (existing
  29A).
- Creates the shelter-rhythm pillar: the schedule determines who works when, who sleeps
  when, and who is on watch. Different schedules create different shelter atmospheres —
  a standard schedule feels organized; an emergency schedule feels desperate; a siege
  schedule feels militarized.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/shelter_schedules.json` (expand 3 → 12 schedules)
- Read-only: confirm the schedule system consumer — `grep -rn "shelter_schedule\|ShelterSchedule"
  Assets/Ashfall.Core/` to find the loader and confirm the schema

## Content grammar (per schedule)
- snake_case `id` with prefix `schedule_` (confirmed prefix from existing 3).
- display_name: evocative (Standard Rotation, Emergency Shifts, Siege Watch, Winter
  Hibernation, etc.).
- day_start_hour / day_end_hour: the active hours (6.0 → 22.0 for standard; 8.0 → 18.0
  for winter; 24/7 for emergency).
- curfew_start_hour / curfew_end_hour: when survivors must be in their rooms.
- shift_pattern: single_shift / double_shift / triple_shift / all_hands / skeleton_crew.
- trigger_condition: when this schedule activates (morale band, threat level, season,
  faction war state, resource shortage).
- description: 1 sentence of grounded shelter flavor.

## Steps
1. Find the schedule system consumer to confirm the schema and how schedules activate.
2. Read the 3 existing schedules to understand the structure.
3. Author 9 new schedules:
   - Winter Hibernation (short days, long nights, conserve fuel — triggers in severe cold).
   - Emergency Shifts (24-hour rotation, all hands — triggers on shelter crisis).
   - Siege Watch (triple shifts, armed watch at all hours — triggers on faction war).
   - Mourning Schedule (reduced activity, no loud work — triggers after a death).
   - Festival Day (extended hours, no curfew — triggers on high morale + holiday).
   - Rationing Schedule (reduced activity to conserve food — triggers on food shortage).
   - Quarantine Schedule (isolated shifts, no common areas — triggers on disease outbreak).
   - Construction Push (extended hours, all hands on repair/build — triggers on shelter
     damage or upgrade).
   - Scout Rotation (staggered shifts to keep expeditions running — triggers when
     multiple expeditions are active).
4. Give each schedule: hours, curfew, shift pattern, trigger condition, description.
5. Wire 4 schedules to Plan 57 incidents (emergency, siege, quarantine, mourning —
   incidents trigger schedule changes).
6. Wire 2 schedules to existing 19C seasonal cadence (winter hibernation, festival day).
7. Wire 2 schedules to Plan 41 shelter rooms (quarantine isolates rooms; construction
   push assigns all hands to workshop).
8. Validate: `--data-integrity-selftest`; confirm schedules activate on the correct
   trigger in a headless boot.
9. xUnit: schedule catalog loads, all triggers valid, schedules activate on correct
   conditions, shift patterns apply to duty roster, save round-trip preserves active
   schedule.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `shelter_schedules.json` has 12 schedules (3 existing + 9 new), all triggers valid, 4
  wired to incidents, 2 wired to seasons, 2 wired to rooms, schedules activate on
  trigger, shift patterns apply, save round-trip green, integrity + tests green.

## Follow-on
- Plan 57 (incidents) — incidents trigger schedule changes.
- Existing 12B (duty roster) — schedules determine shift assignments.
- Plan 41 (shelter rooms) — schedules affect room usage.
- Existing 19C (seasonal cadence) — seasonal schedules.
- Existing 29A (shelter as character) — schedules are the shelter's rhythm.
