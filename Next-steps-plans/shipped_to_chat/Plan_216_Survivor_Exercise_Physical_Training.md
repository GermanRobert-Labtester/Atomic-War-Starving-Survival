# Plan 216 — Survivor Exercise & Physical Training System

## Goal

Create a survivor exercise and physical training system where survivors can engage in structured physical fitness activities — running, strength training, calisthenics, combat drills — to improve physical attributes, maintain fitness, and counteract the deconditioning effects of shelter life. Currently survivor physical capability is static (determined by traits and needs) — there is no exercise system, no fitness tracking, no training regimens, no physical improvement through effort, no deconditioning from inactivity. Survivors don't get fitter or weaker based on activity. This plan adds physical development as a survivor management layer.

## Why

**Repository evidence:** Grep for `ExerciseSystem`, `PhysicalTraining`, `FitnessSystem`, `WorkoutSystem`, `TrainingRegimen`, `PhysicalConditioning`, `ExerciseRoutine`, `FitnessTraining`, `StrengthTraining`, `CardioSystem` in Core returns ZERO matches. Survivor physical capability is determined by traits (`TraitSystem`), needs (`NeedsSystem`), and age (`Plan_176` aging) — but no exercise or fitness system exists. No way to improve physical attributes through training. No deconditioning from inactivity.

**What is missing:** No exercise system. No fitness tracking. No training regimens. No physical improvement through effort. No deconditioning from inactivity. No physical attributes beyond traits. Survivors don't get fitter or weaker based on activity.

**Why existing plans don't solve it:** Plan 176 (Aging & Elderly) covers physical decline from aging but not fitness improvement from exercise. Plan 161 (Hobbies & Leisure) adds personal pastimes but not physical training. Plan 195 (Survivor Specialization Roles) adds role-based skills but not physical fitness. No plan addresses exercise/fitness as a system.

**Player value:** Creates survivor development (physical improvement through effort), adds strategic depth (manage fitness levels), generates emergent stories (training montages, fitness competitions), and makes physical capability dynamic rather than static.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships (training partners)
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skills (physical skills)
- `Assets/Ashfall.Core/Needs/NeedsSystem.cs` — needs (fatigue, hunger affect training)
- `Assets/Ashfall.Core/TraitSystem.cs` — traits (athletic traits)
- NEW: `Assets/Ashfall.Core/Survivors/ExerciseSystem.cs`
- NEW: `Assets/StreamingAssets/Data/exercise_routines.json`

## Main Task 1 — Foundation / System Contract

1. Create `ExerciseSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `FitnessProfile` DTO: `survivorId`, `cardioFitness` (0-100), `strength` (0-100), `flexibility` (0-100), `endurance` (0-100), `overallFitness` (0-100, composite), `fitnessTrend` (improving/stable/declining), `lastExerciseDay`, `trainingStreak` (consecutive days), `fitnessHistory` (list of fitness snapshots)
3. Define `ExerciseRoutine` DTO: `routineId`, `routineName`, `routineType` (cardio/strength/flexibility/endurance/combat_drill/mixed), `duration` (hours), `intensity` (light/moderate/heavy/extreme), `requirements` (equipment needed), `skillPrerequisite` (minimum fitness level), `benefits` (list of attribute improvements), `risks` (injury chance, fatigue)
4. Define `TrainingSession` DTO: `sessionId`, `survivorId`, `routineId`, `sessionDay`, `duration` (actual hours), `intensity` (actual), `completionRate` (0-100), `benefitsGained` (dict of attribute → improvement), `fatigueCost` (float), `injuryOccurred` bool, `injuryType` (string or null), `notes`
5. Define `PhysicalAttribute` DTO: `attributeId`, `attributeName` (cardio/strength/flexibility/endurance), `baseValue` (0-100, from traits), `currentValue` (0-100, modified by fitness), `modifier` (float, from fitness trend), `cap` (100, maximum)
6. Define `ExerciseState` DTO: dict of survivor_id → fitness profile, list of training sessions, list of available routines, exercise settings (auto-exercise bool, injury chance modifier, fitness decay rate)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define exercise types (6+ types):
   - **Cardio**: running, cycling, jumping — improves cardio fitness and endurance
   - **Strength**: weightlifting, push-ups, pull-ups — improves strength
   - **Flexibility**: stretching, yoga — improves flexibility, reduces injury risk
   - **Endurance**: long-duration activity — improves endurance and cardio
   - **Combat Drill**: fighting practice — improves strength, endurance, combat skill
   - **Mixed**: combination routine — balanced improvement
9. Define fitness mechanics:
   - Each survivor has fitness profile (cardio, strength, flexibility, endurance)
   - Fitness improves through exercise (specific routines improve specific attributes)
   - Fitness decays without exercise (deconditioning)
   - Fitness affects: work efficiency, combat performance, expedition capability, health
   - Fitness logged
10. Define training mechanics:
    - Survivor follows exercise routine
    - Routine has duration, intensity, requirements
    - Training session: survivor exercises, gains benefits, suffers fatigue
    - Training can cause injury (chance based on intensity + fitness level)
    - Training logged
11. Define fitness effects:
    - **High cardio**: faster movement, better expedition performance, lower fatigue
    - **High strength**: better combat, heavier carrying capacity, better manual labor
    - **High flexibility**: lower injury risk, better recovery, agile movement
    - **High endurance**: longer work shifts, better expedition stamina, disease resistance
    - **Low fitness**: slower movement, weaker combat, higher injury risk, faster fatigue
12. Define deconditioning:
    - Fitness decays without exercise (rate based on inactivity duration)
    - Long inactivity: significant fitness loss
    - Deconditioning logged
13. Define injury mechanics:
    - Exercise can cause injury (chance based on intensity + fitness)
    - Low fitness: higher injury chance
    - Injury types: strain, sprain, fracture, overexertion
    - Injury affects fitness and work capability
    - Injury logged
14. Add deterministic seeding: exercise events use `ISeededRng`
15. Wire into `GameBootstrap`: `SetupExercise`, `TickExercise`, `SaveExercise`

## Main Task 2 — Implementation / Fitness / Routines / Training / Effects / Injuries / UI

1. Implement fitness profiles:
   - Each survivor has fitness profile
   - Profile tracks cardio, strength, flexibility, endurance
   - Profile updates from training
   - Profile decays without exercise
   - Profile logged
2. Implement exercise routines:
   - Routines defined in data
   - Routines have type, duration, intensity, requirements
   - Routines improve specific attributes
   - Routines logged
3. Implement training sessions:
   - Survivor follows routine
   - Session: duration, intensity, completion
   - Benefits gained (attribute improvements)
   - Fatigue cost
   - Injury chance
   - Session logged
4. Implement fitness effects:
   - Fitness affects work efficiency
   - Fitness affects combat performance
   - Fitness affects expedition capability
   - Fitness affects health
   - Effects logged
5. Implement deconditioning:
   - Fitness decays without exercise
   - Decay rate based on inactivity
   - Deconditioning logged
6. Implement injury mechanics:
   - Exercise can cause injury
   - Injury chance based on intensity + fitness
   - Injury affects fitness and work
   - Injury logged
7. Implement exercise UI:
   - Fitness panel: per-survivor fitness profiles
   - Routine panel: available routines, requirements
   - Training panel: assign routines, view sessions
   - Fitness log: training history
   - Alerts: injury, deconditioning, fitness milestone
8. Create exercise events:
    - "The Workout" — training session completed
    - "The Improvement" — fitness attribute increased
    - "The Injury" — exercise injury occurred
    - "The Decline" — fitness decreased from inactivity
    - "The Milestone" — fitness level reached
    - "The Competition" — fitness competition held
    - "The Routine" — new routine established
    - "The Deconditioning" — significant fitness loss
9. Add exercise quest hooks:
    - "The Athlete" — reach 90+ overall fitness
    - "The Trainer" — complete 50 training sessions
    - "The Marathon" — complete 30-day training streak
    - "The Strong" — reach 90+ strength
    - "The Flexible" — reach 90+ flexibility
    - "The Endurer" — reach 90+ endurance
    - "The Coach" — train 10 survivors to 70+ fitness
10. Implement exercise tutorial: first training session explains system
11. Add exercise tooltips: hover over fitness shows details
12. Create exercise routines in data file (15+ routines)
13. Implement exercise persistence: fitness/sessions saved
14. Integrate with `NeedsSystem`: fatigue/hunger affect training

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `NeedsSystem`: fatigue/hunger affect training capability
2. Connect to `SkillProgressionSystem`: combat drills improve combat skill
3. Integrate with `TraitSystem`: athletic traits give fitness bonuses
4. Connect to `ExpeditionSystem`: fitness affects expedition performance
5. Wire into `CombatSystem`: fitness affects combat performance
6. Connect to `DutyRosterSystem`: fitness affects work efficiency
7. Implement old-save compatibility: existing saves get default fitness (50 all attributes)
8. Add deterministic seeding: exercise events use `ISeededRng`
9. Create exploit prevention: fitness requires actual training, can't be gamed
10. Add tests: fitness profiles, routines, training, effects, deconditioning, injuries, save round-trip
11. Verify all exercise types work correctly
12. Test edge cases: no exercise (current behavior), heavy training (peak fitness)
13. Verify headless behavior: exercise processes correctly without UI
14. Add data-integrity-selftest: exercise validates against survivor catalogs
15. Create `--exercise-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --exercise-selftest
```

## Risk

**LOW** — Exercise is straightforward with clear inputs (training sessions) and outputs (fitness improvements, injuries). Risk of exercise feeling like a chore. Mitigation: make fitness improvements meaningful (visible effects on work/combat/expedition), show clear progress, and ensure training feels rewarding not tedious.

## Definition of Done

- `ExerciseSystem.cs` exists with full `CaptureState/RestoreState`
- 6+ exercise types (cardio, strength, flexibility, endurance, combat drill, mixed)
- Fitness profiles per survivor (cardio, strength, flexibility, endurance)
- Exercise routines (type, duration, intensity, requirements, benefits)
- Training sessions (completion, benefits, fatigue, injury)
- Fitness effects on work, combat, expedition, health
- Deconditioning mechanics (fitness decay without exercise)
- Injury mechanics (chance based on intensity + fitness)
- Exercise events and quest hooks
- Save/load round-trip tested
- Deterministic exercise events verified
- Old saves load with default fitness (50 all attributes)
- Exercise routines in data authority (15+ routines)
- UI fitness panel, routine panel, training panel, fitness log, alerts
- Cross-system integration (needs, skills, traits, expedition, combat, duty roster)

## Follow-On Opportunities

- Exercise specialization (survivors become expert trainers/coaches)
- Exercise legacy (famous fitness achievements remembered)
- Exercise quests (specific fitness goals)
- Exercise events (fitness competition, training camp)
- Exercise trading (trade training services with other settlements)
