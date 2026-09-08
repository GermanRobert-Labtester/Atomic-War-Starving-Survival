# Plan 188 — Individual Survivor Daily Routines

## Goal

Create an individual survivor daily routine system where each survivor has a personal schedule of activities throughout the day — wake time, work blocks, meal times, social time, sleep — rather than just being assigned to a single duty. Currently `ShelterScheduleSystem.cs` (240 lines) handles shelter-level schedules (Day/Night/Curfew/Emergency phases, bed assignments) and `DutyRosterSystem` assigns survivors to work tasks, but there is no personal routine system — no individual wake/sleep times, no meal scheduling, no personal time blocks, no routine preferences, no routine satisfaction. Survivors are either "on duty" or "off duty" with no granular daily rhythm. This plan adds personal depth to survivor management.

## Why

**Repository evidence:** Grep for `DailyRoutine`, `PersonalSchedule`, `IndividualSchedule`, `WakeTime`, `SleepTime`, `ActivitySchedule`, `TimeBlock`, `SurvivorRoutine` in Core returns ZERO system matches. `ShelterScheduleSystem.cs` (240 lines) has shelter-level phases (Day/Night/Curfew/Emergency) and bed assignments but no individual routines. `DutyRosterSystem` assigns work tasks but not daily structure. No personal schedules, no wake/sleep tracking, no meal times, no personal time blocks.

**What is missing:** No individual survivor routines. No personal wake/sleep times. No meal scheduling. No personal time blocks (work/rest/social/personal). No routine preferences (early riser vs night owl). No routine satisfaction. No routine conflicts (two survivors sharing workspace at different times). Survivors have duties but no daily rhythm.

**Why existing plans don't solve it:** Plan 159 (governance) adds political structure but not daily routines. Plan 154 (education) adds schooling schedules but not personal routines. Plan 183 (child development) adds child care needs but not adult routines. No plan addresses individual survivor daily routines.

**Player value:** Creates personality depth (early risers vs night owls), adds strategic complexity (schedule coordination), generates emergent stories (routine conflicts, missed meals, sleep deprivation), and makes survivors feel like individuals with preferences rather than interchangeable workers.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/ShelterScheduleSystem.cs` — shelter-level schedules
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — duty assignments
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — needs tracking
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — lifecycle
- NEW: `Assets/Ashfall.Core/Survivors/SurvivorRoutineSystem.cs`
- NEW: `Assets/StreamingAssets/Data/routine_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `SurvivorRoutineSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `TimeBlock` DTO: `blockId`, `startTime` (hour 0-23), `endTime` (hour 0-23), `activityType` (sleep/work/meal/social/personal/hygiene/exercise/leisure), `location` (where activity happens), `flexibility` (rigid/flexible)
3. Define `DailyRoutine` DTO: `routineId`, `survivorId`, `wakeTime` (hour), `sleepTime` (hour), `timeBlocks` (list of TimeBlock), `mealTimes` (breakfast/lunch/dinner hours), `workBlocks` (list of work time blocks), `personalBlocks` (list of personal time blocks), `routineTemplate` (early_riser/night_owl/standard/custom)
4. Define `RoutinePreference` DTO: `preferenceId`, `survivorId`, `chronotype` (early_riser/night_owl/intermediate), `mealPreference` (small_frequent/large_infrequent), `workPreference` (morning_shift/afternoon_shift/night_shift), `socialPreference` (introvert/extrovert), `exercisePreference` (morning/evening/none)
5. Define `RoutineSatisfaction` DTO: `satisfactionId`, `survivorId`, `day`, `sleepSatisfaction` (0-100, based on sleep duration/quality), `mealSatisfaction` (0-100, based on meal timing/quality), `workSatisfaction` (0-100, based on work-life balance), `socialSatisfaction` (0-100, based on social time), `overallSatisfaction` (0-100, weighted average)
6. Define `RoutineConflict` DTO: `conflictId`, `survivorA`, `survivorB`, `conflictType` (workspace_overlap/meal_time_conflict/sleep_disturbance/social_clash), `day`, `severity` (minor/moderate/major), `resolution` (resolved/ongoing)
7. Define `SurvivorRoutineState` DTO: list of survivor routines, list of routine preferences, list of daily satisfaction scores, list of routine conflicts, routine settings (global routine enforcement level)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define routine templates:
   - **Early Riser**: wake 5-6am, sleep 9-10pm, work morning shift, personal time evening
   - **Night Owl**: wake 9-10am, sleep 1-2am, work afternoon/evening shift, personal time morning
   - **Standard**: wake 7-8am, sleep 10-11pm, work day shift, balanced personal time
   - **Custom**: player-configured routine
10. Define time block activities:
    - **Sleep**: 6-8 hours required, affects fatigue/health
    - **Work**: assigned duty, affects productivity
    - **Meal**: breakfast/lunch/dinner, affects hunger/morale
    - **Social**: interaction with other survivors, affects relationships
    - **Personal**: hygiene, exercise, leisure, affects morale/health
    - **Hygiene**: shower, bathroom, grooming, affects hygiene/morale
    - **Exercise**: physical activity, affects health/morale
    - **Leisure**: reading, hobbies, relaxation, affects morale
11. Define routine satisfaction mechanics:
    - Sleep satisfaction: based on hours slept vs preferred, bed quality, roommates
    - Meal satisfaction: based on meal timing vs preferred, food quality, company
    - Work satisfaction: based on work hours vs preferred, task enjoyment, coworkers
    - Social satisfaction: based on social time vs preference (introvert needs less, extrovert needs more)
    - Overall satisfaction: weighted average affects morale
12. Define routine conflict mechanics:
    - Workspace overlap: two survivors assigned to same workspace at different times (conflict)
    - Meal time conflict: survivors want to eat at different times (minor conflict)
    - Sleep disturbance: roommates have incompatible sleep schedules (major conflict)
    - Social clash: introvert and extrovert share space (moderate conflict)
    - Conflicts reduce satisfaction, can be resolved through scheduling
13. Define routine enforcement:
    - **Strict**: survivors follow routine exactly, penalties for deviation
    - **Flexible**: survivors attempt routine, deviations allowed with reduced satisfaction
    - **None**: survivors have no routine (current behavior)
14. Add deterministic seeding: routine generation uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupSurvivorRoutines`, `TickSurvivorRoutines`, `SaveSurvivorRoutines`

## Main Task 2 — Implementation / Routines / Satisfaction / Conflicts / UI

1. Implement routine assignment:
   - Each survivor gets a routine (template or custom)
   - Routine defines wake/sleep times, work blocks, meal times, personal time
   - Routine based on survivor preferences (chronotype, work preference)
   - Player can override routine
   - Routine displayed in survivor detail panel
2. Implement daily routine execution:
   - Each day, survivors follow their routine
   - Wake time: survivor wakes, starts day
   - Work blocks: survivor performs assigned duty
   - Meal times: survivor eats (if food available)
   - Personal time: survivor does hygiene/exercise/leisure
   - Sleep time: survivor goes to bed
   - Routine execution logged
3. Implement satisfaction tracking:
   - Each day, calculate satisfaction for each category
   - Sleep: hours slept vs preferred, bed quality, roommate compatibility
   - Meal: meal timing vs preferred, food quality, company
   - Work: work hours vs preferred, task enjoyment
   - Social: social time vs preference
   - Overall satisfaction affects morale
4. Implement conflict detection:
   - Check for routine conflicts daily
   - Workspace overlap: two survivors, same workspace, different times
   - Sleep disturbance: incompatible roommate sleep schedules
   - Meal conflicts: different meal times in shared space
   - Social clashes: introvert/extrovert mismatches
   - Conflicts logged, satisfaction reduced
5. Implement conflict resolution:
   - Player can adjust routines to resolve conflicts
   - Survivors can compromise (adjust routine slightly)
   - Some conflicts unresolvable (fundamental incompatibility)
   - Resolution logged
6. Implement routine preferences:
   - Each survivor has chronotype (early/night/intermediate)
   - Each survivor has meal/work/social preferences
   - Preferences affect routine satisfaction
   - Preferences can be overridden by player
   - Preferences displayed in survivor detail
7. Implement routine templates:
   - Early riser template: 5am wake, 9pm sleep, morning work
   - Night owl template: 10am wake, 2am sleep, evening work
   - Standard template: 7am wake, 11pm sleep, day work
   - Custom template: player-configured
   - Templates auto-generate time blocks
8. Implement routine UI:
   - Survivor detail: routine display (wake/sleep/work/meal/personal times)
   - Routine editor: adjust times, change template
   - Conflict panel: show active conflicts, resolution options
   - Satisfaction panel: show satisfaction scores per category
   - Schedule view: daily timeline showing all survivor routines
9. Implement routine consequences:
   - High satisfaction: morale bonus, productivity bonus
   - Low satisfaction: morale penalty, productivity reduction
   - Sleep deprivation: fatigue, health decline
   - Missed meals: hunger, morale drop
   - Social isolation: relationship decay
   - Routine conflicts: stress, morale penalty
10. Create routine events:
    - "The Routine" — survivor routine established
    - "The Conflict" — routine conflict detected
    - "The Resolution" — conflict resolved
    - "The Adjustment" — routine adjusted
    - "The Deprivation" — sleep deprivation detected
    - "The Satisfaction" — high satisfaction achieved
    - "The Clash" — major routine incompatibility
    - "The Compromise" — survivors compromise on routine
11. Add routine quest hooks:
    - "The Manager" — establish routines for all survivors
    - "The Harmony" — resolve all routine conflicts
    - "The Satisfaction" — keep all survivors above 80% satisfaction
    - "The Early Bird" — assign 5 survivors to early riser routine
    - "The Night Shift" — assign 5 survivors to night owl routine
    - "The Balance" — maintain work-life balance for 30 days
    - "The Compromise" — resolve 10 routine conflicts
12. Implement routine tutorial: first survivor explains routine system
13. Add routine tooltips: hover over time block shows activity, satisfaction impact
14. Create routine template definitions in data file
15. Implement routine persistence: routines saved/loaded with game state

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ShelterScheduleSystem`: routines respect shelter phases
2. Connect to `DutyRosterSystem`: work blocks align with duty assignments
3. Integrate with `NeedsSystem`: routine affects needs (sleep → fatigue, meals → hunger)
4. Connect to `SurvivorRelationsSystem`: social time affects relationships
5. Wire into `NeedsSystem`: routine satisfaction affects morale
6. Connect to `MentalHealthCrisisSystem` (Plan 179): low satisfaction increases crisis risk
7. Implement old-save compatibility: existing saves get default routines (standard template)
8. Add deterministic seeding: routines use `ISeededRng`
9. Create exploit prevention: routines are preference-based, can't be gamed
10. Add tests: routine assignment, satisfaction calculation, conflict detection, resolution, save round-trip
11. Verify all routine templates work correctly
12. Test edge cases: no routine (current behavior), strict enforcement, flexible enforcement
13. Verify headless behavior: routines process correctly without UI
14. Add data-integrity-selftest: routines validate against survivor/duty catalogs
15. Create `--survivor-routine-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --survivor-routine-selftest
```

## Risk

**LOW** — Survivor routines are straightforward with clear inputs (time blocks, preferences) and outputs (satisfaction, conflicts). Risk of routines feeling micromanagement-heavy rather than personality-enhancing. Mitigation: make templates easy, show clear satisfaction impacts, allow flexible enforcement, and ensure routines add depth not complexity.

## Definition of Done

- `SurvivorRoutineSystem.cs` exists with full `CaptureState/RestoreState`
- 4 routine templates (early riser, night owl, standard, custom)
- Time block system (sleep/work/meal/social/personal/hygiene/exercise/leisure)
- Routine preference system (chronotype, meal/work/social preferences)
- Satisfaction tracking (sleep/meal/work/social/overall)
- Conflict detection (workspace overlap, sleep disturbance, meal/social clashes)
- Conflict resolution mechanics
- Routine enforcement levels (strict/flexible/none)
- Routine events and quest hooks
- Save/load round-trip tested
- Deterministic routines verified
- Old saves get default routines
- Routine templates in data authority
- UI routine editor, conflict panel, satisfaction panel
- Cross-system integration (shelter schedule, duty roster, needs, relations, psychology)

## Follow-On Opportunities

- Routine specialization (survivors develop preferred routines over time)
- Routine legacy (famous routines remembered)
- Routine quests (specific routine goals)
- Routine events (routine disruptions, schedule changes)
- Routine trading (survivors swap routines)
