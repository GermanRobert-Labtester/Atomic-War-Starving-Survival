# Plan 193 — Chronic Conditions & Disabilities System

## Goal

Create a chronic conditions and disabilities system where survivors can develop permanent or long-term impairments from injuries, radiation exposure, disease, or age — affecting their capabilities, requiring accommodations, and creating meaningful character depth. Currently `RadiationSystem.cs` has `HasChronicIllness` (boolean flag) and `MedicalPathologyCatalog.cs` has `FunctionalImpairmentPct`, but there is no general chronic condition system — no permanent injuries, no disabilities, no long-term impairments, no accommodation mechanics, no condition management. Survivors either heal fully or die; there's no middle ground of living with lasting consequences. This plan adds realism and character depth through permanent consequences.

## Why

**Repository evidence:** Grep for `ChronicCondition`, `Disability`, `PermanentInjury`, `LongTermCondition`, `ChronicIllness` (as a system), `Impairment` (as a system) in Core returns only `RadiationSystem.cs:23` (`HasChronicIllness` boolean flag) and `MedicalPathologyCatalog.cs:104` (`FunctionalImpairmentPct` static data field). No dedicated chronic condition system, no disability tracking, no permanent injury mechanics, no accommodation system. `RadiationSystem` grants `SurvivorStatus.ChronicIllness` at high radiation but this is a status flag, not a condition with specific impairments.

**What is missing:** No general chronic condition system. No permanent injuries (limp, blindness, reduced strength). No disabilities (mobility impairment, sensory loss, cognitive decline). No long-term impairments from disease/injury. No accommodation mechanics (wheelchair access, sign language, prosthetics). No condition management (medication, therapy, assistive devices). Survivors heal or die — no lasting consequences.

**Why existing plans don't solve it:** Plan 179 (psychology) adds psychological conditions but not physical disabilities. Plan 185 (memory decay) adds cognitive decline but not physical impairments. Plan 176 (aging) adds aging but not age-related disabilities. Plan 172 (radiation mutation) adds mutations but not chronic conditions. No plan addresses chronic conditions/disabilities as a system.

**Player value:** Creates character depth (survivors with unique challenges), adds strategic complexity (accommodate disabled survivors), generates emergent stories (overcoming disability, adapting to limitations), and makes injuries feel consequential (permanent scars, not just HP loss).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation chronic illness
- `Assets/Ashfall.Core/Narrative/MedicalPathologyCatalog.cs` — impairment data
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — needs tracking
- `Assets/Ashfall.Core/MedicalPipeline/` — medical system
- NEW: `Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs`
- NEW: `Assets/StreamingAssets/Data/chronic_conditions.json`

## Main Task 1 — Foundation / System Contract

1. Create `ChronicConditionSystem.cs` in `Assets/Ashfall.Core/Medical/`
2. Define `ChronicCondition` DTO: `conditionId`, `conditionName` (limp/blindness/deafness/reduced_strength/cognitive_decline/chronic_pain/respiratory_damage/neurological_damage), `conditionType` (mobility/sensory/physical/cognitive), `severity` (mild/moderate/severe/total), `onsetDay`, `cause` (injury/radiation/disease/age/congenital), `isPermanent` bool, `progressionRate` (0 = stable, >0 = worsening per day), `affectedCapabilities` (list of capability modifiers)
3. Define `Disability` DTO: `disabilityId`, `disabilityName`, `disabilityType` (mobility/vision/hearing/speech/cognitive/physical), `severity` (mild/moderate/severe/profound), `accommodationRequirements` (list of needed accommodations), `capabilityModifiers` (dict of capability → modifier), `onsetDay`, `cause`
4. Define `Accommodation` DTO: `accommodationId`, `accommodationName` (wheelchair_ramp/sign_language_interpreter/prosthetic_limb/hearing_aide/cane/walker/medication/therapy), `accommodationType` (physical/communication/medical/therapeutic), `cost` (item_ids or currency), `effectiveness` (0-100, how well it addresses disability), `maintenanceRequired` bool, `maintenanceCost` (per day/week)
5. Define `CapabilityModifier` DTO: `capabilityName` (movement_speed/work_speed/learning_rate/social_interaction/combat_effectiveness/crafting_quality), `baseModifier` (0.0-1.0, multiplier), `currentModifier` (after accommodations), `affectedByConditions` (list of condition_ids)
6. Define `ChronicConditionState` DTO: list of survivor conditions, list of survivor disabilities, list of provided accommodations, list of capability modifiers per survivor, condition management settings
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define chronic condition types:
   - **Mobility**: limp (mild), cane-required (moderate), wheelchair-bound (severe), bedridden (total)
   - **Vision**: blurred-vision (mild), partial-blindness (moderate), total-blindness (severe)
   - **Hearing**: mild-hearing-loss (mild), moderate-hearing-loss (moderate), deaf (severe)
   - **Physical**: reduced-strength (mild), chronic-pain (moderate), organ-damage (severe)
   - **Cognitive**: memory-issues (mild), learning-difficulty (moderate), cognitive-decline (severe)
   - **Respiratory**: chronic-cough (mild), reduced-capacity (moderate), oxygen-dependent (severe)
   - **Neurological**: tremors (mild), seizures (moderate), paralysis (severe)
9. Define condition acquisition:
   - **Injury**: combat wounds, accidents, radiation burns → permanent injuries
   - **Radiation**: high radiation exposure → chronic illness, organ damage
   - **Disease**: severe illness → long-term complications
   - **Age**: aging → age-related conditions (Plan 176 integration)
   - **Congenital**: born with condition (rare, from parent conditions)
10. Define severity progression:
    - Some conditions are stable (permanent but not worsening)
    - Some conditions progress (worsen over time without treatment)
    - Some conditions can be managed (medication slows progression)
    - Some conditions can be cured (rare, requires advanced treatment)
    - Progression rate varies by condition and treatment
11. Define capability modifiers:
    - Each condition affects specific capabilities
    - Modifiers reduce capability effectiveness
    - Multiple conditions compound (multiplicative, not additive)
    - Accommodations restore some capability
    - Net modifier = base × condition × accommodation
12. Define accommodation mechanics:
    - Accommodations reduce disability impact
    - Physical accommodations: wheelchair ramps, prosthetics, canes
    - Communication accommodations: sign language interpreters, hearing aids
    - Medical accommodations: medication, therapy, assistive devices
    - Accommodations cost resources (items, currency, survivor time)
    - Accommodations require maintenance (refills, repairs, adjustments)
13. Define condition management:
    - Medication slows progression, reduces symptoms
    - Therapy improves capability, slows decline
    - Surgery can cure some conditions (rare, risky)
    - Prosthetics replace lost function (expensive, require maintenance)
    - Management requires ongoing resources
14. Add deterministic seeding: condition onset uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupChronicConditions`, `TickChronicConditions`, `SaveChronicConditions`

## Main Task 2 — Implementation / Conditions / Disabilities / Accommodations / Management

1. Implement condition acquisition:
   - Detect condition triggers (injury, radiation, disease, age)
   - Roll for condition onset (severity based on trigger intensity)
   - Condition added to survivor
   - Onset event logged
   - Player notified
2. Implement condition progression:
   - Each day, check condition progression
   - Stable conditions: no change
   - Progressive conditions: severity increases
   - Managed conditions: progression slowed/reversed
   - Progression logged
3. Implement capability modifiers:
   - Calculate net capability modifier per survivor
   - Base capability × condition modifiers × accommodation bonuses
   - Modifiers affect work speed, movement, learning, combat, crafting
   - Modifiers displayed in survivor detail
4. Implement accommodation provision:
   - Player assigns accommodation to disabled survivor
   - Accommodation requires resources (items, currency)
   - Accommodation reduces disability impact
   - Accommodation effectiveness calculated
   - Accommodation maintenance tracked
5. Implement condition management:
   - Medication: slows progression, reduces symptoms
   - Therapy: improves capability, slows decline
   - Surgery: can cure some conditions (risky)
   - Prosthetics: replace lost function
   - Management requires ongoing resources
6. Implement condition consequences:
   - Reduced work speed (affected tasks take longer)
   - Reduced movement speed (expedition slower)
   - Reduced combat effectiveness (lower accuracy/damage)
   - Reduced learning rate (skill progression slower)
   - Reduced social interaction (relationship penalties)
   - Increased medical needs (more treatment required)
7. Implement accommodation consequences:
   - Wheelchair ramp: enables mobility-impaired access
   - Sign language interpreter: enables deaf survivor communication
   - Prosthetic limb: restores lost function
   - Hearing aid: improves hearing
   - Medication: stabilizes condition
   - Therapy: improves capability
8. Implement condition UI:
   - Survivor detail: conditions list, disabilities, accommodations
   - Condition management panel: assign medication, therapy, surgery
   - Accommodation panel: provide accommodations
   - Capability modifier display: show how conditions affect capabilities
   - Condition log: history of condition onset/progression
9. Implement condition events:
    - "The Injury" — condition acquired from injury
    - "The Diagnosis" — condition identified
    - "The Progression" — condition worsened
    - "The Accommodation" — accommodation provided
    - "The Treatment" — condition managed/treated
    - "The Cure" — condition cured (rare)
    - "The Decline" — condition progressed despite treatment
    - "The Adaptation" — survivor adapted to condition
10. Add condition quest hooks:
    - "The Doctor" — treat 10 chronic conditions
    - "The Caregiver" — provide accommodations for 5 survivors
    - "The Survivor" — survivor with disability completes major task
    - "The Cure" — cure a chronic condition
    - "The Management" — manage 10 conditions effectively
    - "The Adaptation" — 5 survivors fully accommodated
    - "The Care" — maintain 20 accommodations
11. Implement condition tutorial: first condition onset explains system
12. Add condition tooltips: hover over condition shows severity, progression, accommodations
13. Create condition definitions in data file (20+ conditions)
14. Implement condition persistence: conditions saved with survivor state
15. Integrate with `RadiationSystem`: radiation chronic illness becomes full condition

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `RadiationSystem`: radiation chronic illness integrated
2. Connect to `MedicalPipeline`: condition treatment integrated
3. Integrate with `NeedsSystem`: conditions affect needs
4. Connect to `SkillProgressionSystem`: conditions affect learning
5. Wire into `CombatSystem`: conditions affect combat
6. Connect to `ExpeditionSystem`: conditions affect expedition speed
7. Implement old-save compatibility: existing saves get no conditions (or migrate `HasChronicIllness`)
8. Add deterministic seeding: condition onset uses `ISeededRng`
9. Create exploit prevention: conditions are consequence-based, can't be gamed
10. Add tests: condition acquisition, progression, accommodations, management, capability modifiers, save round-trip
11. Verify all condition types work correctly
12. Test edge cases: no conditions (healthy survivors), multiple conditions (compound effects)
13. Verify headless behavior: conditions process correctly without UI
14. Add data-integrity-selftest: conditions validate against item/skill catalogs
15. Create `--chronic-condition-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --chronic-condition-selftest
```

## Risk

**MEDIUM** — Chronic conditions touch many systems and can feel punishing if not handled sensitively. Risk of disabilities feeling like penalties rather than character depth. Mitigation: make accommodations effective, show clear management paths, ensure conditions add depth not frustration, and treat disabilities with respect (not as tragedies to overcome).

## Definition of Done

- `ChronicConditionSystem.cs` exists with full `CaptureState/RestoreState`
- 7 condition categories (mobility, vision, hearing, physical, cognitive, respiratory, neurological)
- 20+ specific conditions with severity levels
- Condition acquisition (injury, radiation, disease, age, congenital)
- Condition progression (stable, progressive, managed, curable)
- Capability modifiers (movement, work, learning, combat, crafting, social)
- Accommodation system (physical, communication, medical, therapeutic)
- Condition management (medication, therapy, surgery, prosthetics)
- Condition events and quest hooks
- Save/load round-trip tested
- Deterministic condition onset verified
- Old saves migrate `HasChronicIllness` flag
- Condition definitions in data authority
- UI condition panel, accommodation panel, capability modifier display
- Cross-system integration (radiation, medical, needs, skills, combat, expedition)

## Follow-On Opportunities

- Condition specialization (survivors adapt uniquely to conditions)
- Condition legacy (famous survivors with disabilities remembered)
- Condition quests (specific condition management goals)
- Condition events (breakthrough treatments, disability rights)
- Condition advocacy (improve accommodation access)
