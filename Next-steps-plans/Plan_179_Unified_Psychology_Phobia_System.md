# Plan 179 — Unified Psychology & Phobia System

## Goal

Create a unified psychology and phobia system that integrates the 6 existing trauma systems (CombatTrauma, SomaticFlashback, GuiltInsomnia, MentalHealthCrisis, PsychologicalContamination, PhantomMemory) into a coherent psychological profile per survivor, with phobia development, personality evolution, and long-term psychological arcs. Currently trauma systems operate in parallel without shared state — a survivor can have combat hypervigilance, guilt insomnia, and location contamination simultaneously with no unified psychological picture. This plan creates the unified layer that connects them.

## Why

**Repository evidence:** Grep for `PhobiaSystem`, `PsychologicalProfile`, `PersonalityChange`, `TraumaProfile` in Core returns ZERO matches. The recon confirmed 6 functional trauma systems: `CombatTraumaSystem.cs` (239 lines, hypervigilance/false alarms), `SomaticFlashbackSystem.cs` (286 lines, noise-triggered flashbacks), `GuiltInsomniaSystem.cs` (guilt-driven sleep disruption), `MentalHealthCrisisSystem.cs` (207 lines, crisis pipeline with acuity), `PsychologicalContaminationSystem.cs` (233 lines, location-triggered contamination), `PhantomMemoryEngine.cs` (328 lines, background-specific item triggers). All functional but isolated — no unified profile, no phobia development, no personality change.

**What is missing:** No unified psychological profile. No phobia development from specific experiences. No long-term personality evolution. No cross-system trauma integration. No therapy/recovery arc beyond the ward mechanic. The systems are reactive (trigger → effect → decay) rather than developmental.

**Why existing plans don't solve it:** Plan 147 (per-NPC memory) adds memory but not psychological integration. Plan 148 (friction→events) adds friction events but not psychological arcs. Plan 177 (dreams) adds dream processing but not unified psychology. No plan addresses psychological unification or phobia development.

**Player value:** Creates psychological depth (survivors develop distinct psychological profiles), adds long-term consequences (trauma shapes personality over time), generates emergent stories (phobia development, recovery arcs), and makes mental health more than a stat to manage.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` — combat trauma
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` — flashbacks
- `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` — guilt insomnia
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` — crisis pipeline
- `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` — contamination
- `Assets/Ashfall.Core/PhantomMemoryEngine.cs` — phantom memories
- NEW: `Assets/Ashfall.Core/Psychology/PsychologicalProfileSystem.cs`
- NEW: `Assets/StreamingAssets/Data/phobia_definitions.json`

## Main Task 1 — Foundation / System Contract

1. Create `PsychologicalProfileSystem.cs` in `Assets/Ashfall.Core/Psychology/`
2. Define `PsychologicalProfile` DTO: `survivorId`, `personalityTraits` (list of trait modifiers from experiences), `phobias` (list of developed phobias), `copingMechanisms` (list of learned coping strategies), `resilience` (0-100, overall psychological resilience), `vulnerabilities` (list of trigger sensitivities), `recoveryArc` (current stage of recovery if in therapy)
3. Define `Phobia` DTO: `phobiaId`, `phobiaName` (claustrophobia/acrophobia/nyctophobia/thalassophobia/social_phobia/blood_phobia/radiation_phobia), `triggerCondition` (what triggers it), `severity` (0-100), `effects` (list: work penalty, avoidance behavior, morale penalty), `developedFrom` (trauma event that caused it)
4. Define `CopingMechanism` DTO: `mechanismId`, `mechanismName` (meditation/exercise/socializing/creative_work/substance_use/denial), `effectiveness` (0-100), `sideEffects` (list of negative effects), `learnedFrom` (therapy/experience/peer)
5. Define `PsychologyState` DTO: list of survivor profiles, phobia development log, therapy sessions held, personality evolution history
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define phobia types:
   - **Claustrophobia**: fear of enclosed spaces (from tunnel/bunker trauma)
   - **Acrophobia**: fear of heights (from fall/expedition accident)
   - **Nyctophobia**: fear of darkness (from nighttime trauma)
   - **Thalassophobia**: fear of deep water (from drowning/near-drowning)
   - **Social Phobia**: fear of social situations (from betrayal/loss)
   - **Blood Phobia**: fear of blood/injury (from combat trauma)
   - **Radiation Phobia**: fear of radiation (from contamination event)
8. Define phobia development:
   - Specific trauma events can trigger phobia development
   - Combat trauma → blood phobia or nyctophobia
   - Location contamination → claustrophobia or radiation phobia
   - Expedition accident → acrophobia or thalassophobia
   - Social betrayal → social phobia
   - Phobia probability based on trauma severity and resilience
9. Define phobia effects:
   - Triggered phobia: work penalty, avoidance behavior, morale penalty
   - Phobia severity increases with repeated exposure
   - Phobia can be managed through therapy
   - Severe phobia: survivor refuses specific tasks
10. Define coping mechanisms:
    - **Meditation**: reduces stress, requires quiet time
    - **Exercise**: reduces anxiety, requires physical energy
    - **Socializing**: reduces isolation, requires peer interaction
    - **Creative Work**: processes trauma, requires art materials
    - **Substance Use**: temporary relief, risk of addiction
    - **Denial**: blocks awareness, prevents healing
11. Define personality evolution:
    - Trauma experiences shape personality over time
    - Survivors develop coping styles
    - Personality affects work preferences and social behavior
    - Personality changes are gradual and permanent
    - Personality visible in survivor detail
12. Define therapy system:
    - Therapy sessions address specific traumas/phobias
    - Therapist survivor with psychology skill
    - Therapy reduces phobia severity
    - Therapy teaches coping mechanisms
    - Therapy requires time and trust
13. Add deterministic seeding: psychology uses `ISeededRng`
14. Wire into `GameBootstrap`: `SetupPsychology`, `TickPsychology`, `SavePsychology`
15. Implement psychology UI: psychological profile panel per survivor

## Main Task 2 — Implementation / Profiles / Phobias / Coping / Therapy

1. Implement psychological profiles:
   - Each survivor has psychological profile
   - Profile integrates data from all 6 trauma systems
   - Profile shows personality traits, phobias, coping mechanisms
   - Profile updates as trauma systems report events
2. Implement phobia development:
   - Trauma events checked for phobia triggers
   - Phobia probability roll with `ISeededRng`
   - Successful roll: phobia develops
   - Phobia recorded in profile
   - Phobia effects applied
3. Implement phobia management:
   - Phobia triggered by specific conditions
   - Triggered: work penalty, avoidance, morale penalty
   - Repeated exposure increases severity
   - Therapy reduces severity
   - Severe phobia: task refusal
4. Implement coping mechanisms:
   - Survivors develop coping mechanisms over time
   - Coping reduces trauma impact
   - Some coping mechanisms have side effects
   - Coping learned through therapy or experience
   - Coping visible in profile
5. Implement personality evolution:
   - Personality traits shift based on experiences
   - Combat experience → more cautious or more aggressive
   - Loss experience → more withdrawn or more protective
   - Recovery experience → more resilient
   - Personality affects behavior and preferences
6. Implement therapy:
   - Therapist assigned to survivor
   - Therapy sessions scheduled
   - Sessions address specific traumas/phobias
   - Therapy reduces phobia severity
   - Therapy teaches coping mechanisms
   - Therapy requires trust between therapist and patient
7. Implement cross-system integration:
   - CombatTraumaSystem reports to profile
   - SomaticFlashbackSystem reports to profile
   - GuiltInsomniaSystem reports to profile
   - MentalHealthCrisisSystem reports to profile
   - PsychologicalContaminationSystem reports to profile
   - PhantomMemoryEngine reports to profile
   - Profile aggregates all reports
8. Create psychology events:
   - "The Phobia" — phobia develops
   - "The Trigger" — phobia triggered
   - "The Coping" — coping mechanism learned
   - "The Therapy" — therapy session held
   - "The Recovery" — phobia severity reduced
   - "The Personality" — personality trait shifts
   - "The Resilience" — resilience increases
9. Add psychology quest hooks:
    - "The Therapist" — become qualified therapist
    - "The Recovery" — help survivor overcome phobia
    - "The Coping" — teach coping mechanism
    - "The Profile" — complete psychological assessment
    - "The Resilience" — reach high resilience
    - "The Healing" — complete therapy arc
    - "The Understanding" — understand survivor's psychology
10. Implement psychology UI:
    - Profile panel: traits, phobias, coping, resilience
    - Phobia detail: trigger, severity, effects
    - Therapy panel: schedule sessions, track progress
    - Coping display: learned mechanisms
    - Personality display: trait evolution
11. Add psychology journal: automatic log of psychology events
12. Implement psychology tutorial: first phobia explains system
13. Add psychology tooltips: hover over trait shows history
14. Create 7 phobia definitions in data file
15. Create 6 coping mechanism definitions

## Main Task 3 — Integration / Consequences / Validation

1. Wire into all 6 trauma systems: report events to profile
2. Connect to `NeedsSystem`: psychology affects morale
3. Integrate with `SurvivorRelationsSystem`: psychology affects relationships
4. Connect to `SkillProgressionSystem`: therapy skill
5. Wire into `DreamSystem` (Plan 177): dreams process trauma
6. Connect to `AgingSystem` (Plan 176): resilience changes with age
7. Implement old-save compatibility: existing survivors get default profiles
8. Add deterministic seeding: psychology uses `ISeededRng`
9. Create exploit prevention: therapy requires time and trust
10. Add tests: profile integration, phobia development, coping, therapy, save round-trip
11. Verify catalog integrity: all phobia/coping IDs resolve
12. Test edge cases: no trauma (healthy profile), extensive trauma (complex profile)
13. Verify headless behavior: psychology processes correctly without UI
14. Add data-integrity-selftest: phobias validate against trauma event catalogs
15. Create `--psychology-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --psychology-selftest
```

## Risk

**MEDIUM** — Psychology complexity can overwhelm if too many interacting systems exist. Risk of psychology feeling like a spreadsheet rather than human experience. Mitigation: keep profile view simple, show clear cause-effect, make therapy meaningful, and ensure psychology enhances rather than complicates existing trauma systems.

## Definition of Done

- `PsychologicalProfileSystem.cs` exists with full `CaptureState/RestoreState`
- Integration with all 6 existing trauma systems
- 7 phobia types implemented
- Phobia development from specific trauma events
- 6 coping mechanisms implemented
- Personality evolution system
- Therapy system with sessions and progress
- Psychology events and quest hooks
- Save/load round-trip tested
- Deterministic psychology verified
- Old saves get default profiles
- 7 phobias + 6 coping mechanisms in data authority
- UI psychological profile panel
- Cross-system integration (all 6 trauma systems, needs, relations, skills, dreams, aging)

## Follow-On Opportunities

- Psychology specialization (therapist certification)
- Psychology legacy (therapist remembered)
- Psychology quests (specific therapy arcs)
- Psychology group therapy (shared sessions)
- Psychology research (study trauma patterns)
