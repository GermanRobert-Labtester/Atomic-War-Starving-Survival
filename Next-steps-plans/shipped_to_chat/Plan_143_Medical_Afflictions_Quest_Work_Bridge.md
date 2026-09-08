# Plan 143 — Medical Afflictions → Quest & Work Bridge

## Goal

Connect medical afflictions to quest availability and work efficiency so that sick survivors unlock/lock specific quests and produce less work output. Currently afflictions are tracked in the medical pipeline but have no downstream effect on quests or duty performance. This makes illness a meaningful gameplay factor that shapes what the player can do and how efficiently the shelter operates.

## Why

**Repository evidence:** `MedicalPipelineCoordinator.cs` orchestrates the affliction pipeline. `SomaticFlashbackSystem.cs` has `workEfficiencyPenalty` (0.60 or 0.10) but it's the **only** affliction that affects work — and even that isn't consumed by `DutyRosterSystem`. No quest system queries `MedicalPipelineCoordinator`, `AfflictionId`, or any affliction state. The cross-system agent confirmed: "Afflictions do NOT affect work capacity, social interactions, or quest availability." The gameplay gaps agent confirmed: "Medical afflictions do not gate quests."

**What is missing:** A survivor with a broken leg works at full speed. A survivor with radiation sickness can still go on dangerous expeditions. A survivor with combat trauma has the same quest options as a healthy one. Afflictions are tracked but don't shape gameplay — they're a health bar, not a gameplay modifier.

**Why existing plans don't solve it:** Plan 137 (needs→performance cascade) connects needs to performance but doesn't address medical afflictions. Plan 112 (disease catalog expansion) adds more diseases but not quest/work integration. Plan 09 (medical disease depth) expands pathogens but doesn't bridge to quests. No plan connects afflictions to quest gating or work efficiency.

**Player value:** Makes medical treatment a strategic priority (heal survivors to unlock quests and restore productivity), creates meaningful medical decisions (which survivor to treat first), and generates emergent stories (a key quest-giver falls ill, forcing the player to adapt).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` — affliction tracking
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` — existing work efficiency precedent
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — work assignment
- `Assets/Ashfall.Core/Quests/` — quest systems
- `Assets/StreamingAssets/Data/afflictions.json` — affliction definitions (VERIFY)
- NEW: `Assets/Ashfall.Core/Medical/AfflictionQuestBridge.cs`
- NEW: `Assets/Ashfall.Core/Medical/AfflictionWorkBridge.cs`

## Main Task 1 — Foundation / System Contract

1. Create `AfflictionQuestBridge.cs` in `Assets/Ashfall.Core/Medical/`
2. Create `AfflictionWorkBridge.cs` in `Assets/Ashfall.Core/Medical/`
3. Define `AfflictionQuestGate` DTO: `afflictionId`, `questId`, `gateType` (blocks/unlocks/modifies), `severity` (mild/moderate/severe), `description`
4. Define `AfflictionWorkModifier` DTO: `afflictionId`, `workSpeedMultiplier` (0.3-1.0), `workQualityMultiplier` (0.5-1.0), `excludedDuties` (list of duty types)
5. Define `AfflictionBridgeState` DTO: list of active quest gates, list of active work modifiers
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define quest gate rules:
   - **Blocks**: severe afflictions block specific quests (e.g., broken leg blocks expedition quests)
   - **Unlocks**: having an affliction unlocks medical quests (e.g., radiation sickness unlocks "Find Anti-Rad" quest)
   - **Modifies**: afflictions change quest outcomes (e.g., doing quest with combat trauma has different dialogue)
8. Define work modifier rules:
   - Broken leg: -50% work speed, excluded from heavy labor/expedition
   - Radiation sickness: -30% work speed, -50% work quality, excluded from food handling
   - Combat trauma: -20% work speed, excluded from combat duty
   - Respiratory degeneration: -40% work speed, excluded from outdoor duty
   - Chemical dependency: -25% work speed, random work refusals
9. Create `IAfflictionQuestSink` interface for quest systems to query affliction gates
10. Create `IAfflictionWorkSink` interface for `DutyRosterSystem` to query work modifiers
11. Implement quest gate checking: quest systems check if survivor's afflictions block/unlock/modify the quest
12. Implement work modifier application: duty roster reads affliction modifiers and applies to work output
13. Add deterministic calculation: gates/modifiers are pure functions of affliction state (no RNG)
14. Wire into `GameBootstrap`: `SetupAfflictionBridges`, `SaveAfflictionBridges`

## Main Task 2 — Implementation / Quest Gating / Work Modification

1. Implement quest blocking:
   - Expedition quests blocked by: broken leg, severe radiation, respiratory failure
   - Combat quests blocked by: combat trauma, broken arm, severe illness
   - Social quests blocked by: contagious disease, severe mental crisis
   - Medical quests blocked by: unconsciousness, death
2. Implement quest unlocking:
   - Radiation sickness unlocks "Find Anti-Rad" quest
   - Combat trauma unlocks "PTSD Support Group" quest
   - Chemical dependency unlocks "Detox Program" quest
   - Respiratory degeneration unlocks "Air Filtration Upgrade" quest
   - Multiple afflictions unlock "Medical Crisis" shelter event
3. Implement quest modification:
   - Doing quest with affliction changes dialogue options
   - Afflicted survivors have unique quest outcomes
   - Some quests have "push through" option (affliction worsens)
   - Some quests have "seek help first" option (delays quest but treats affliction)
4. Implement work speed modification:
   - Duty roster reads affliction work modifiers
   - Work output multiplied by modifier (e.g., broken leg = 50% output)
   - Multiple afflictions stack multiplicatively
   - UI shows work modifier on duty roster panel
5. Implement work quality modification:
   - Crafting quality reduced by afflictions (defect chance increases)
   - Medical treatment quality reduced (side effect chance increases)
   - Combat effectiveness reduced (accuracy/damage penalty)
   - Teaching/mentoring quality reduced (XP gain reduced)
6. Implement duty exclusions:
   - Some duties automatically refuse afflicted survivors
   - Broken leg: refuses heavy labor, expedition, combat
   - Contagious disease: refuses food handling, medical, close contact
   - Combat trauma: refuses combat duty, guard duty
   - UI shows exclusion reason on duty roster
7. Create affliction-aware quest events:
   - "The Sick Leader" — leader falls ill, succession crisis
   - "Quarantine" — contagious disease spreads, shelter lockdown
   - "Medical Emergency" — multiple afflictions require triage
   - "Pushing Through" — afflicted survivor insists on working (risk vs. necessity)
8. Add affliction quest hooks:
   - "The Healer's Dilemma" — choose which survivor to treat (limited medicine)
   - "The Plague" — contagious disease threatens shelter
   - "The Wounded Warrior" — combat veteran needs treatment before next mission
   - "The Addiction" — survivor's chemical dependency worsens
9. Implement affliction recovery effects:
   - Treated affliction removes quest blocks
   - Recovered survivor gains "Survivor" trait (resilience bonus)
   - Near-death experience changes survivor's moral outlook
   - Long illness creates bonds with caregivers
10. Add UI: medical panel shows affliction effects on quests and work
11. Create medical journal: automatic log of affliction impacts
12. Implement medical tutorial: first affliction explains quest/work impacts
13. Add medical tooltips: hover over affliction shows quest/work effects
14. Create 15 affliction quest gates and 15 work modifiers in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MedicalPipelineCoordinator`: affliction changes trigger bridge updates
2. Connect to quest systems: quest availability checks affliction gates
3. Integrate with `DutyRosterSystem`: work output applies affliction modifiers
4. Connect to `SomaticFlashbackSystem`: existing work penalty integrated
5. Wire into `ExpeditionSystem`: expedition readiness checks afflictions
6. Connect to `TacticalCombatSystem`: combat effectiveness checks afflictions
7. Implement old-save compatibility: existing saves get empty bridge state
8. Add deterministic calculation: gates/modifiers are pure functions of affliction state
9. Create exploit prevention: afflictions have natural progression, can't be reset
10. Add tests: quest gating, work modification, stacking, save round-trip
11. Verify catalog integrity: all affliction/quest IDs resolve
12. Test edge cases: no afflictions (no gates/modifiers), multiple afflictions (stacking)
13. Verify headless behavior: bridges process correctly without UI
14. Add data-integrity-selftest: affliction gates/modifiers validate against catalogs
15. Create `--affliction-bridges-selftest` verb for CI validation

## State / System Interaction Model

```text
Survivor acquires affliction
├─ Quest bridge updates
│  ├─ Quests blocked (severe afflictions prevent specific quests)
│  ├─ Quests unlocked (affliction-specific medical quests)
│  └─ Quests modified (dialogue/outcome changes)
├─ Work bridge updates
│  ├─ Work speed modifier applied (e.g., -50%)
│  ├─ Work quality modifier applied (e.g., -30%)
│  └─ Duty exclusions applied (e.g., no heavy labor)
├─ UI updated
│  ├─ Medical panel shows affliction effects
│  ├─ Quest panel shows blocked/unlocked quests
│  └─ Duty roster shows work modifiers
└─ Downstream systems notified
   ├─ Quest systems: availability changed
   ├─ Duty roster: output modified
   ├─ Expedition: readiness changed
   └─ Combat: effectiveness changed
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --affliction-bridges-selftest
```

## Risk

**MEDIUM** — Affliction quest gating can frustrate players if key quests are blocked by unavoidable afflictions. Risk of death spirals (afflicted survivor can't work → less resources for treatment → affliction worsens). Mitigation: ensure alternative quest paths exist, provide treatment options, keep work penalties moderate (max 50%), and allow "push through" options with risks.

## Definition of Done

- `AfflictionQuestBridge.cs` and `AfflictionWorkBridge.cs` exist with full `CaptureState/RestoreState`
- Quest blocking/unlocking/modifying functional
- Work speed/quality modification functional
- Duty exclusion system working
- 15 affliction quest gates in data authority
- 15 affliction work modifiers in data authority
- Affliction-aware quest events and hooks
- Save/load round-trip tested
- Deterministic gate/modifier calculation verified
- Old saves load without error
- UI panels show affliction effects
- Cross-system integration (medical, quests, duty roster, expedition, combat)

## Follow-On Opportunities

- Affliction specialization (survivors become specialists in treating specific afflictions)
- Affliction research (research unlocks better treatments)
- Affliction legacy (survivors who recovered gain unique traits)
- Affliction social dynamics (caregiving bonds, stigma, isolation)
- Affliction mutation (long-term afflictions cause permanent changes)
