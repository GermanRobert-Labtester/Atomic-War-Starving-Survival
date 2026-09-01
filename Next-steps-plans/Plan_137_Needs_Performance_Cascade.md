# Plan 137 — Needs → Performance Cascade

## Goal

Connect `NeedsSystem` (hunger, thirst, fatigue, warmth) to combat effectiveness, work efficiency, and expedition performance. Starving, exhausted, or freezing survivors fight worse, work slower, and explore less effectively. This creates meaningful survival pressure and makes needs management a strategic priority rather than just a death-prevention meter.

## Why

**Repository evidence:** `NeedsSystem.cs` handles hunger, thirst, fatigue, warmth, morale, hygiene with death thresholds. `SomaticFlashbackSystem.cs` has `workEfficiencyPenalty` (0-1) but it's isolated — not consumed by `DutyRosterSystem` or combat. `TraumaBondSystem.cs` mentions "boosting work efficiency when assigned to identical shifts" but the bonus is not wired to actual output. `TacticalCombatSystem` has zero references to `NeedsSystem`. `DutyRosterSystem` has zero references to `NeedsSystem`. `ExpeditionSystem` has zero references to `NeedsSystem`. The cross-system agent confirmed: "Needs do NOT cascade into combat performance, work efficiency, or expedition performance."

**What is missing:** Survivors fight, work, and explore at full capacity regardless of needs. A starving survivor with 10% hunger deals the same combat damage as a well-fed one. A fatigued survivor works at the same rate as a rested one. This removes survival pressure from gameplay loops and makes needs a passive death-clock rather than an active performance driver.

**Why existing plans don't solve it:** Plan 1 (needs save round-trip) tests save/load. Plan 14 (economy/weather/shelter loop) mentions daily texture but not performance cascades. Plan 13 (economy survival loop) mentions trapping/hunting but not needs→performance. No plan connects needs to combat/work/expedition effectiveness.

**Player value:** Creates strategic depth (feed your best fighters before combat, rest key workers before important tasks), makes survival pressure meaningful throughout gameplay (not just at death threshold), and generates emergent stories (a starving survivor's desperate last stand).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — needs tracking
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` — combat mechanics
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — work assignment
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition mechanics
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` — existing work efficiency precedent
- `Assets/Ashfall.Core/Survivors/TraumaBondSystem.cs` — existing work efficiency precedent
- NEW: `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs`

## Main Task 1 — Foundation / System Contract

1. Create `NeedsPerformanceBridge.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `NeedsPerformanceModifiers` DTO: `combatAccuracyMultiplier` (0.5-1.0), `combatDamageMultiplier` (0.5-1.0), `workSpeedMultiplier` (0.3-1.0), `expeditionStaminaMultiplier` (0.5-1.0), `expeditionSpeedMultiplier` (0.5-1.0)
3. Define modifier calculation rules:
   - Hunger < 30%: no penalty. 30-60%: -10% combat, -15% work. 60-90%: -25% combat, -30% work. >90%: -50% combat, -70% work
   - Thirst < 20%: no penalty. 20-50%: -15% combat, -20% work. 50-80%: -30% combat, -40% work. >80%: -50% combat, -60% work
   - Fatigue < 40%: no penalty. 40-70%: -10% combat, -20% work. 70-90%: -25% combat, -40% work. >90%: -50% combat, -60% work
   - Warmth < 20%: no penalty. 20-50%: -10% combat, -15% work. 50-80%: -20% combat, -30% work. >80%: -40% combat, -50% work
   - Modifiers stack multiplicatively (not additively) to prevent extreme penalties
4. Create `GetPerformanceModifiers(string survivorId)` method that reads current needs and returns modifiers
5. Implement `ICombatPerformanceModifier` interface for `TacticalCombatSystem` to consume
6. Implement `IWorkEfficiencyModifier` interface for `DutyRosterSystem` to consume
7. Implement `IExpeditionPerformanceModifier` interface for `ExpeditionSystem` to consume
8. Add deterministic calculation: modifiers are pure functions of needs state (no RNG)
9. Wire into `GameBootstrap`: `SetupNeedsPerformanceBridge` (no tick needed — pure query)
10. Create UI hook: survivor panel shows performance modifier percentage
11. Add warning system: survivors with >60% needs penalty show warning icon
12. Implement recovery: as needs are restored, modifiers return to 1.0
13. Create critical threshold: any need at death threshold → modifier drops to 0.3 (last effort)
14. Add morale interaction: low morale amplifies need penalties by 10%

## Main Task 2 — Implementation / Combat / Work / Expedition Integration

1. Implement combat integration:
   - `TacticalCombatSystem` reads `GetPerformanceModifiers(survivorId)` before calculating accuracy/damage
   - Hungry survivors miss more shots, deal less damage
   - Fatigued survivors have reduced reaction time (initiative penalty)
   - Cold survivors have reduced stamina (fewer actions per turn)
   - UI shows modifier during combat ("Hunger: -25% accuracy")
2. Implement work efficiency integration:
   - `DutyRosterSystem` reads `GetPerformanceModifiers(survivorId)` for work output
   - Hungry/thirsty/fatigued survivors produce less per shift
   - Work quality may decrease (crafting defects, medical errors)
   - UI shows modifier on duty roster ("Fatigue: -40% output")
3. Implement expedition integration:
   - `ExpeditionSystem` reads `GetPerformanceModifiers(survivorId)` for travel speed/stamina
   - Hungry survivors drain stamina faster
   - Fatigued survivors move slower
   - Cold survivors have reduced carry capacity
   - UI shows modifier on expedition briefing ("Thirst: -30% stamina")
4. Create needs-based events:
   - "Pushing Through" — survivor with high needs completes critical task anyway (morale boost)
   - "Breaking Point" — survivor with extreme needs refuses to work/fight (morale penalty)
   - "Rally" — shelter-wide morale event temporarily reduces need penalties
5. Implement needs-based quest hooks:
   - "The Hungry Guard" — assign hungry survivors to guard duty (risk vs. necessity)
   - "Exhausted Expedition" — push fatigued team to complete expedition before deadline
   - "Frozen Watch" — cold survivors maintain shelter defense during blizzard
6. Add needs-based combat tactics:
   - "Desperate Strike" — hungry survivor deals bonus damage once per combat (then collapses)
   - "Adrenaline Rush" — fatigued survivor gains temporary speed boost (then crashes)
   - "Cold Fury" — cold survivor gains damage resistance (then hypothermia risk)
7. Create needs-based work specializations:
   - Light duty (scavenging, cooking) has reduced need penalties
   - Heavy duty (construction, combat) has amplified need penalties
   - Skill can partially offset need penalties (experienced workers cope better)
8. Implement needs-based expedition roles:
   - Scout (fast but fragile): need penalties amplified
   - Carrier (slow but sturdy): need penalties reduced
   - Navigator (mental): fatigue penalty amplified, hunger reduced
9. Add needs recovery acceleration:
   - Well-fed survivors recover fatigue faster
   - Rested survivors recover hunger slower
   - Warm survivors recover warmth faster
10. Create needs-based social effects:
    - Hungry survivors are irritable (friction chance +)
    - Fatigued survivors are withdrawn (morale penalty)
    - Cold survivors are desperate (theft chance +)
11. Add UI: survivor detail panel shows needs performance modifiers
12. Create needs performance journal: automatic log of performance-impacting need events
13. Implement needs performance tutorial: first-hour guidance on managing survivor needs
14. Add needs performance tooltips: hover over modifier shows breakdown

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `TacticalCombatSystem`: combat calculations read performance modifiers
2. Connect to `DutyRosterSystem`: work output reads performance modifiers
3. Integrate with `ExpeditionSystem`: travel speed/stamina reads performance modifiers
4. Connect to `SurvivorRelationsSystem`: need-induced friction affects relationships
5. Wire into `MentalHealthCrisisSystem`: need stress increases crisis probability
6. Connect to `ShelterDecorSystem`: need penalties affect morale from decor
7. Implement old-save compatibility: existing saves get default modifiers (1.0)
8. Add deterministic calculation: modifiers are pure functions of needs state
9. Create exploit prevention: modifiers are read-only, can't be manipulated
10. Add tests: modifier calculation, combat integration, work integration, expedition integration, save round-trip
11. Verify catalog integrity: all survivor IDs resolve
12. Test edge cases: all needs at 0% (no penalty), all needs at 100% (maximum penalty), death threshold (0.3 modifier)
13. Verify headless behavior: modifiers calculate correctly without UI
14. Add data-integrity-selftest: modifier thresholds validate against needs ranges
15. Create `--needs-performance-selftest` verb for CI validation

## State / System Interaction Model

```text
Survivor needs state (hunger, thirst, fatigue, warmth)
├─ Calculate performance modifiers
│  ├─ Hunger penalty: 0% to -50% combat/work
│  ├─ Thirst penalty: 0% to -50% combat/work
│  ├─ Fatigue penalty: 0% to -50% combat/work
│  ├─ Warmth penalty: 0% to -40% combat/work
│  └─ Morale amplifier: low morale adds 10% to all penalties
├─ Combat integration
│  ├─ Accuracy multiplied by modifier
│  ├─ Damage multiplied by modifier
│  ├─ Initiative reduced by fatigue
│  └─ Stamina reduced by cold
├─ Work integration
│  ├─ Output multiplied by modifier
│  ├─ Quality reduced by extreme needs
│  └─ Shift duration reduced by fatigue
├─ Expedition integration
│  ├─ Travel speed multiplied by modifier
│  ├─ Stamina drain amplified by hunger
│  └─ Carry capacity reduced by cold
└─ Social integration
   ├─ Friction chance increased by hunger
   ├─ Morale penalty from fatigue
   └─ Theft chance from cold desperation
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --needs-performance-selftest
```

## Risk

**MEDIUM** — Performance penalties can frustrate players if too severe or too opaque. Risk of death spirals (hungry survivors work poorly → less food → more hungry). Mitigation: keep penalties moderate (max 50%), provide clear UI feedback, offer recovery options (rest, eat, warm up), and allow players to prioritize which survivors to keep fed/rested.

## Definition of Done

- `NeedsPerformanceBridge.cs` exists with modifier calculation
- Combat system reads and applies performance modifiers
- Work/duty system reads and applies performance modifiers
- Expedition system reads and applies performance modifiers
- Modifiers stack multiplicatively with morale amplification
- UI shows performance modifiers on survivor panels
- Needs-based events and quest hooks implemented
- Save/load round-trip tested (modifiers are pure functions, no state to save)
- Deterministic modifier calculation verified
- Old saves load without error
- Cross-system integration (combat, duty, expedition, relations, mental health)

## Follow-On Opportunities

- Needs-based skill specialization (survivors cope better with specific needs)
- Needs-based training (survivors can train to resist need penalties)
- Needs-based mutations (long-term starvation causes permanent penalties)
- Needs-based social roles (community caregivers reduce need penalties for others)
- Needs-based legacy (survivors who endured extreme needs gain unique traits)
