# Plan 144 — Survivor Autonomy & Initiative

## Goal

Give survivors the ability to make independent decisions, express preferences, and take initiative rather than being purely reactive instruments. Currently survivors only respond to player orders and system-generated stimuli — they never help each other unprompted, refuse tasks based on emotional state, form their own goals, or act on relationships. This plan adds a lightweight autonomy layer that makes survivors feel like characters with agency.

## Why

**Repository evidence:** The survivor social agent confirmed: "Survivors are reactive instruments. They respond to player orders and system-generated stimuli, but never initiate actions, express preferences, or make independent decisions." The only autonomous behaviors are: `PhantomMemoryEngine` (15% trigger on scavenging), `RationConflictSystem` (conflict at resentment thresholds), `SurvivorRelationsSystem.TryTriggerConflict()` (10% daily conflict chance), and `MentalHealthCrisisSystem` (crisis triggers). No system lets survivors help each other, refuse orders based on emotion, form goals, or act on relationships.

**What is missing:** Survivors never spontaneously comfort a grieving companion, offer to help an overloaded coworker, refuse a dangerous task out of fear, pursue a personal project, or seek out a friend. They are tools, not characters. The rich relationship data in `SurvivorRelationsSystem` (affinity, trust, resentment, grief, bondType) is never queried for autonomous behavior.

**Why existing plans don't solve it:** Plan 132 (hidden agendas) adds secret motivations but those are hidden, not autonomous behavior. Plan 12 (social/shelter life) mentions friction events but not survivor-initiated actions. Plan 52 (recurring NPC arcs) covers external NPCs, not internal shelter survivor autonomy. No plan addresses survivor-initiated behavior.

**Player value:** Makes survivors feel alive (they act on their own), creates emergent stories (a survivor spontaneously helps another, creating a bond), reduces micromanagement (survivors handle small decisions themselves), and deepens emotional investment (survivors have personalities, not just stats).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationship data (unused for autonomy)
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — survivor state
- `Assets/Ashfall.Core/Survivors/MentalHealthCrisisSystem.cs` — crisis triggers
- `Assets/Ashfall.Core/PhantomMemoryEngine.cs` — existing autonomous trigger precedent
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — work assignment
- NEW: `Assets/Ashfall.Core/Survivors/SurvivorAutonomySystem.cs`

## Main Task 1 — Foundation / System Contract

1. Create `SurvivorAutonomySystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `AutonomyAction` DTO: `actorId`, `actionType` (help/refuse/initiate/express/pursue), `targetId` (optional), `trigger` (relationship/need/emotion/opportunity), `outcome` (accepted/rejected/ignored), `day`
3. Define `AutonomyState` DTO: list of recent autonomous actions, cooldowns per survivor, preference profiles
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define autonomy action types:
   - **Help**: survivor spontaneously helps another (comfort, assist with work, share resources)
   - **Refuse**: survivor refuses task based on emotional state (fear, resentment, exhaustion)
   - **Initiate**: survivor starts a project or activity without being told
   - **Express**: survivor expresses preference or emotion (complains, praises, suggests)
   - **Pursue**: survivor works toward a personal goal (skill practice, relationship building)
6. Define trigger conditions:
   - Relationship triggers: high affinity → help, high resentment → refuse, grief → comfort/withdraw
   - Need triggers: low hunger → share food, high fatigue → refuse work, low warmth → seek heat
   - Emotion triggers: high morale → initiate projects, low morale → complain/withdraw
   - Opportunity triggers: free time → pursue goals, witnessed event → express reaction
7. Create `IAutonomySink` interface for other systems to receive autonomous actions
8. Implement action probability: each action type has base probability modified by survivor traits, relationship strength, need severity
9. Add deterministic seeding: autonomy triggers use `ISeededRng`
10. Wire into `GameBootstrap`: `SetupSurvivorAutonomy`, `TickAutonomy`, `SaveAutonomy`
11. Create autonomy cooldown: survivors can't take autonomous actions more than once per N days
12. Implement autonomy logging: all autonomous actions recorded for UI/journal
13. Add UI hook: survivor panel shows recent autonomous actions
14. Create autonomy journal: automatic log of survivor-initiated events

## Main Task 2 — Implementation / Help / Refuse / Initiate / Express / Pursue

1. Implement help actions:
   - Survivor with high affinity to stressed survivor offers comfort (+5 morale)
   - Survivor with high skill offers to mentor low-skill survivor (skill XP transfer)
   - Survivor with spare food shares with hungry survivor (-5 hunger for both)
   - Survivor helps overloaded coworker (work speed bonus for pair)
   - Help has 20% base chance per day if trigger conditions met
2. Implement refuse actions:
   - Survivor with high resentment refuses to work with resented target
   - Survivor with high fatigue refuses dangerous tasks (expedition, combat)
   - Survivor with low morale refuses non-essential tasks
   - Survivor with fear (recent trauma) refuses task related to trauma source
   - Refusal has 10% base chance per day if trigger conditions met
   - Player can override refusal (morale penalty) or accept (no penalty)
3. Implement initiate actions:
   - Survivor with high skill starts practice session (skill XP gain)
   - Survivor with hobby starts project (morale bonus, shelter decoration)
   - Survivor with high affinity organizes social event (shelter morale boost)
   - Survivor with leadership trait proposes plan (shelter efficiency bonus)
   - Initiation has 5% base chance per day if survivor has free time
4. Implement express actions:
   - Survivor with high morale praises shelter (morale boost for others)
   - Survivor with low morale complains (morale penalty for others)
   - Survivor expresses preference for duty (work efficiency bonus if assigned to preferred)
   - Survivor expresses opinion on faction (affects player's faction standing)
   - Expression has 15% base chance per day if trigger conditions met
5. Implement pursue actions:
   - Survivor works toward personal goal (learn skill, build relationship, recover from trauma)
   - Goal progress tracked per survivor
   - Goal completion grants trait or bonus
   - Pursuit has 10% base chance per day if survivor has free time and goal defined
6. Create autonomy personality modifiers:
   - Independent trait: +20% initiate, -10% help
   - Social trait: +20% help, +10% express
   - Stubborn trait: +20% refuse, -10% accept override
   - Ambitious trait: +20% pursue, +10% initiate
   - Anxious trait: +20% refuse (fear), +10% express (complain)
7. Implement autonomy relationship effects:
   - Successful help increases affinity (+5)
   - Refused override decreases affinity (-10)
   - Accepted expression increases trust (+3)
   - Completed pursuit grants relationship bonus to mentor/partner
8. Create autonomy events:
   - "A Kind Word" — survivor comforts another, bond forms
   - "The Strike" — survivor refuses work, shelter disrupted
   - "The Surprise" — survivor initiates project, shelter benefits
   - "The Confession" — survivor expresses hidden feeling, relationship changes
   - "The Achievement" — survivor completes personal goal, trait gained
9. Add autonomy quest hooks:
   - "The Mediator" — resolve dispute between two autonomous survivors
   - "The Project" — support survivor's initiated project
   - "The Refusal" — convince refusing survivor to reconsider
   - "The Goal" — help survivor achieve personal goal
10. Add UI: autonomy panel showing recent survivor-initiated actions
11. Create autonomy journal: automatic log of autonomous events
12. Implement autonomy tutorial: first autonomous action explains system
13. Add autonomy tooltips: hover over action shows trigger and outcome
14. Create 20 autonomy action templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SurvivorRelationsSystem`: autonomy actions modify relationships
2. Connect to `DutyRosterSystem`: refusals affect duty assignment
3. Integrate with `NeedsSystem`: need triggers feed into autonomy
4. Connect to `MentalHealthCrisisSystem`: emotion triggers feed into autonomy
5. Wire into `SkillProgressionSystem`: help/initiate/pursue grant skill XP
6. Connect to `MoralChoiceSystem`: some autonomy actions are moral choices
7. Implement old-save compatibility: existing saves get empty autonomy state
8. Add deterministic seeding: autonomy triggers use `ISeededRng`
9. Create exploit prevention: cooldowns prevent autonomy spam
10. Add tests: action triggering, relationship effects, work effects, save round-trip
11. Verify catalog integrity: all action templates validate
12. Test edge cases: no relationships (no help), all refuse (shelter paralyzed)
13. Verify headless behavior: autonomy ticks correctly without UI
14. Add data-integrity-selftest: autonomy templates validate against survivor/trait catalogs
15. Create `--survivor-autonomy-selftest` verb for CI validation

## State / System Interaction Model

```text
Daily autonomy tick
├─ For each survivor
│  ├─ Check triggers (relationship/need/emotion/opportunity)
│  ├─ Roll for action (probability based on triggers + personality)
│  ├─ If action triggered
│  │  ├─ Help: comfort/assist/share with target
│  │  │  ├─ Target accepts: affinity +, morale +
│  │  │  └─ Target rejects: no effect
│  │  ├─ Refuse: decline task
│  │  │  ├─ Player overrides: morale -, affinity -
│  │  │  └─ Player accepts: no penalty
│  │  ├─ Initiate: start project/activity
│  │  │  ├─ Success: shelter bonus, morale +
│  │  │  └─ Failure: no effect
│  │  ├─ Express: share feeling/opinion
│  │  │  ├─ Positive: morale + for listeners
│  │  │  └─ Negative: morale - for listeners
│  │  └─ Pursue: work toward goal
│  │     ├─ Progress: goal closer
│  │     └─ Completion: trait/bonus gained
│  └─ Cooldown applied
└─ Actions logged for UI/journal
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --survivor-autonomy-selftest
```

## Risk

**MEDIUM** — Survivor autonomy can frustrate players if survivors refuse critical tasks or make poor decisions. Risk of autonomy feeling random rather than motivated. Mitigation: keep refusal probability low (10%), make refusals overrideable, ensure autonomy actions are mostly positive (help, initiate), and provide clear feedback on triggers.

## Definition of Done

- `SurvivorAutonomySystem.cs` exists with full `CaptureState/RestoreState`
- 5 autonomy action types implemented (help, refuse, initiate, express, pursue)
- Trigger conditions functional (relationship, need, emotion, opportunity)
- Personality modifiers affect action probability
- Autonomy actions modify relationships and work
- Autonomy events and quest hooks
- Save/load round-trip tested
- Deterministic autonomy triggers verified
- Old saves load without error
- 20 autonomy action templates in data authority
- UI panel shows recent autonomous actions
- Cross-system integration (relations, duty, needs, mental health, skills, moral choice)

## Follow-On Opportunities

- Survivor goals system (long-term personal objectives)
- Survivor hobbies (leisure activities that affect morale/skills)
- Survivor leadership (autonomous survivors can lead others)
- Survivor rebellion (mass refusal if morale too low)
- Survivor legacy (autonomous actions remembered in epilogue)
