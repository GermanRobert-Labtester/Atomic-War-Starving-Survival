# Plan 220 — Shelter Atmosphere & Ambiance System

## Goal

Create a shelter atmosphere and ambiance system that tracks the shelter's overall mood, personality, and environmental character as a composite of conditions — lighting, sound, cleanliness, activity level, decoration, and social energy — creating a dynamic "feel" that changes based on shelter state and affects survivor morale and behavior. Currently shelter conditions are tracked individually (temperature, air quality, power, etc.) but there is no unified atmosphere system, no composite mood tracking, no ambiance effects, no shelter personality. The shelter has conditions but no character. This plan adds atmospheric depth.

## Why

**Repository evidence:** Grep for `AtmosphereSystem`, `AmbianceSystem`, `ShelterMood`, `EnvironmentalMood`, `ShelterPersonality`, `ShelterCharacter`, `AmbientEffect`, `ShelterVibe`, `AtmosphericEffect`, `MoodLighting` in Core returns ZERO matches. Individual shelter systems track conditions (thermal, ventilation, power, fire, noise) but no composite atmosphere system exists. No unified mood tracking, no ambiance effects, no shelter personality.

**What is missing:** No atmosphere system. No ambiance tracking. No composite mood. No shelter personality. No environmental mood effects. No unified atmospheric character. The shelter has individual conditions but no overall "feel."

**Why existing plans don't solve it:** Plan 205 (Noise Discipline) covers acoustic atmosphere but not overall ambiance. Plan 186 (Shelter Maintenance) covers physical degradation but not atmosphere. Plan 158 (Disaster Response) covers crisis but not day-to-day ambiance. No plan addresses shelter atmosphere as a unified system.

**Player value:** Creates atmospheric depth (shelter has character), adds immersion (environmental mood affects survivors), generates emergent dynamics (atmosphere changes with shelter state), and makes the shelter feel like a living environment with personality.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs` — temperature (atmosphere component)
- `Assets/Ashfall.Core/VentilationSystem.cs` — air quality (atmosphere component)
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` — power/lighting (atmosphere component)
- `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` — fire/smoke (atmosphere component)
- NEW: `Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs`
- NEW: `Assets/StreamingAssets/Data/atmosphere_profiles.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterAtmosphereSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `AtmosphereState` DTO: `overallMood` (0-100, composite atmosphere score), `moodCategory` (bleak/tense/neutral/comfortable/welcoming/vibrant), `lightingQuality` (0-100), `soundEnvironment` (0-100, noise level inverse), `airQuality` (0-100), `temperatureComfort` (0-100), `cleanliness` (0-100), `activityLevel` (0-100), `socialEnergy` (0-100), `decorationLevel` (0-100), `lastUpdatedDay`, `moodTrend` (improving/stable/declining)
3. Define `AtmosphereProfile` DTO: `profileId`, `profileName` (industrial/sterile/lived-in/warm/cold/chaotic/serene), `characteristics` (list of atmosphere tags), `moraleModifier` (float), `productivityModifier` (float), `healthModifier` (float), `dominantMood` (mood category)
4. Define `AtmosphereEffect` DTO: `effectId`, `effectType` (morale_boost/morale_penalty/productivity_boost/productivity_penalty/health_boost/health_penalty/stress_reduction/stress_increase/sleep_quality_boost/sleep_quality_penalty), `magnitude` (float), `duration` (days, -1 if permanent), `source` (what caused this effect), `affectedSurvivors` (list of survivor_ids or "all")
5. Define `AtmosphereEvent` DTO: `eventId`, `eventType` (atmosphere_improved/atmosphere_degraded/mood_shift/profile_changed/lighting_changed/sound_changed/air_changed/temperature_changed/decoration_added/activity_shift), `day`, `description`, `magnitude` (float), `consequences` (list of effects)
6. Define `AtmosphereComponent` DTO: `componentId`, `componentName` (lighting/sound/air/temperature/cleanliness/activity/social/decoration), `currentValue` (0-100), `targetValue` (0-100), `changeRate` (per day), `source` (what system provides this value)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define atmosphere components (9 components):
   - **Lighting**: from `PowerGridSystem` — well-lit = positive, dim/dark = negative
   - **Sound**: from `ShelterNoiseSystem` (Plan 205) — quiet = positive, loud = negative
   - **Air Quality**: from `VentilationSystem` — clean = positive, polluted = negative
   - **Temperature**: from `ShelterThermalSystem` — comfortable = positive, extreme = negative
   - **Cleanliness**: from `SanitationSystem` (Plan 201) — clean = positive, dirty = negative
   - **Activity Level**: from `DutyRosterSystem` — active = positive, stagnant = negative
   - **Social Energy**: from `SurvivorRelationsSystem` — positive relationships = positive
   - **Decoration**: from shelter improvements — decorated = positive, bare = negative
   - **Overall Mood**: composite of all components
9. Define atmosphere profiles (7+ profiles):
   - **Industrial**: functional, efficient, cold — productivity bonus, morale penalty
   - **Sterile**: clean, organized, impersonal — health bonus, social penalty
   - **Lived-in**: comfortable, worn, homey — morale bonus, efficiency penalty
   - **Warm**: welcoming, decorated, social — morale bonus, productivity penalty
   - **Cold**: harsh, minimal, functional — productivity bonus, morale penalty
   - **Chaotic**: active, noisy, disorganized — social energy bonus, stress penalty
   - **Serene**: quiet, clean, peaceful — health bonus, activity penalty
10. Define atmosphere mechanics:
    - Components updated daily from source systems
    - Overall mood calculated from components
    - Mood category determined from overall score
    - Atmosphere profile matched based on component pattern
    - Profile effects applied to survivors
    - Atmosphere logged
11. Define atmosphere effects:
    - **Morale**: good atmosphere = morale boost, bad = penalty
    - **Productivity**: efficient atmosphere = work bonus, chaotic = penalty
    - **Health**: clean atmosphere = health bonus, polluted = penalty
    - **Stress**: serene atmosphere = stress reduction, chaotic = increase
    - **Sleep**: quiet/dark = sleep bonus, noisy/bright = penalty
    - Effects logged
12. Define atmosphere changes:
    - Atmosphere changes as components change
    - Mood shifts logged
    - Profile changes logged
    - Atmosphere events logged
13. Add deterministic seeding: atmosphere events use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupAtmosphere`, `TickAtmosphere`, `SaveAtmosphere`

## Main Task 2 — Implementation / Components / Profiles / Effects / Events / UI

1. Implement atmosphere components:
   - Components read from source systems
   - Components updated daily
   - Components logged
2. Implement overall mood:
   - Mood calculated from components
   - Mood category determined
   - Mood trend tracked
   - Mood logged
3. Implement atmosphere profiles:
   - Profile matched based on component pattern
   - Profile effects applied
   - Profile logged
4. Implement atmosphere effects:
   - Effects applied to survivors
   - Effects based on profile/mood
   - Effects logged
5. Implement atmosphere events:
   - Events triggered by atmosphere changes
   - Events logged
6. Implement atmosphere UI:
   - Atmosphere panel: overall mood, components, profile
   - Component detail: individual component values
   - Profile display: current profile, effects
   - Mood trend: graph of mood over time
   - Atmosphere log: history of events
   - Alerts: atmosphere shifts, profile changes
7. Create atmosphere events:
    - "The Shift" — atmosphere mood changed
    - "The Profile" — atmosphere profile changed
    - "The Improvement" — atmosphere improved
    - "The Decline" — atmosphere degraded
    - "The Comfort" — shelter became comfortable
    - "The Tension" — shelter became tense
    - "The Serenity" — shelter became peaceful
    - "The Chaos" — shelter became chaotic
8. Add atmosphere quest hooks:
    - "The Interior Designer" — achieve welcoming profile for 100 days
    - "The Engineer" — maintain industrial profile for 100 days
    - "The Peacemaker" — achieve serene profile
    - "The Host" — maintain comfortable atmosphere for 200 days
    - "The Optimizer" — maximize productivity through atmosphere
    - "The Caretaker" — keep all components above 70 for 50 days
    - "The Transformer" — change atmosphere profile 5 times
9. Implement atmosphere tutorial: first atmosphere shift explains system
10. Add atmosphere tooltips: hover shows component details
11. Create atmosphere profiles in data file (7+ profiles)
12. Implement atmosphere persistence: state/profiles saved
13. Integrate with source systems: components read from existing systems

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ShelterThermalSystem`: temperature component
2. Connect to `VentilationSystem`: air quality component
3. Integrate with `PowerGridSystem`: lighting component
4. Connect to `ShelterFireHazardSystem`: smoke/fire affects atmosphere
5. Wire into `ShelterNoiseSystem` (Plan 205): sound component
6. Connect to `SanitationSystem` (Plan 201): cleanliness component
7. Connect to `DutyRosterSystem`: activity component
8. Connect to `SurvivorRelationsSystem`: social energy component
9. Connect to `NeedsSystem`: atmosphere affects morale/health/stress
10. Implement old-save compatibility: existing saves get neutral atmosphere (50 all components)
11. Add deterministic seeding: atmosphere events use `ISeededRng`
12. Create exploit prevention: atmosphere is state-based, can't be gamed
13. Add tests: components, mood, profiles, effects, events, save round-trip
14. Verify all atmosphere profiles work correctly
15. Test edge cases: neutral atmosphere (current behavior), extreme atmosphere (very good/bad)
16. Verify headless behavior: atmosphere processes correctly without UI
17. Add data-integrity-selftest: atmosphere validates against shelter component systems
18. Create `--shelter-atmosphere-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-atmosphere-selftest
```

## Risk

**LOW** — Atmosphere is straightforward with clear inputs (component values) and outputs (mood, profile, effects). Risk of atmosphere feeling like an abstract meta-layer. Mitigation: make atmosphere tangible (visible effects on survivors), show clear cause-effect from shelter systems, and ensure atmosphere feels like environmental character not just numbers.

## Definition of Done

- `ShelterAtmosphereSystem.cs` exists with full `CaptureState/RestoreState`
- 9 atmosphere components (lighting, sound, air, temperature, cleanliness, activity, social, decoration, overall mood)
- 7+ atmosphere profiles (industrial, sterile, lived-in, warm, cold, chaotic, serene)
- Atmosphere effects (morale, productivity, health, stress, sleep)
- Atmosphere events and quest hooks
- Mood tracking (overall score, category, trend)
- Component integration with source systems
- Save/load round-trip tested
- Deterministic atmosphere events verified
- Old saves load with neutral atmosphere (50 all components)
- Atmosphere profiles in data authority (7+ profiles)
- UI atmosphere panel, component detail, profile display, mood trend, event log, alerts
- Cross-system integration (thermal, ventilation, power, fire, noise, sanitation, duty roster, relations, needs)

## Follow-On Opportunities

- Atmosphere specialization (survivors become expert interior designers/atmosphere managers)
- Atmosphere legacy (famous atmospheres remembered across campaigns)
- Atmosphere quests (specific atmosphere goals)
- Atmosphere events (atmosphere transformation, seasonal atmosphere)
- Atmosphere trading (trade atmosphere technology with other settlements)
