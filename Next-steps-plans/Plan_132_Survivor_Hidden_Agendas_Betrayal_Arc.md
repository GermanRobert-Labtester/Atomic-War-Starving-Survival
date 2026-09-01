# Plan 132 — Survivor Hidden Agendas & Betrayal Arc

## Goal

Create a persistent hidden-agenda system where survivors carry secret motivations, loyalties, and goals that unfold over time. Players discover these through behavioral clues, confrontation, and investigation. The system produces branching narrative arcs with delayed consequences, mutual exclusivity, and permanent campaign effects — including betrayal, reconciliation, or exploitation.

## Why

**Repository evidence:** `confession_secrets.json` (94 lines, 4 entries) provides one-shot forgiveness/grudge events. `SurvivorRelationsSystem.cs` (191 lines) tracks affinity/trust/resentment/grief but has no hidden-agenda layer. `MentalHealthCrisisSystem.cs` (207 lines) handles crises but not secret motivations. `IdeologicalFrictionSystem.cs` (158 lines) affects sleep quality but doesn't produce narrative arcs. No system tracks hidden loyalties, secret contacts, or delayed betrayal.

**What is missing:** Survivors are transparent — their states are fully visible to the player. There is no hidden layer of motivation, no discovery mechanic, no "this survivor has been stealing supplies" or "this survivor secretly contacts the PRPF" gameplay. The confession system is a one-shot event, not an evolving arc.

**Why existing plans don't solve it:** Plan 88 (confession secrets expansion) adds more one-shot confessions. Plan 52 (recurring NPC arcs) covers external NPCs, not internal shelter survivors. Plan 110 (moral choice gossip) is intra-shelter chatter, not hidden agendas. No plan addresses persistent secret motivations with discovery/confrontation mechanics.

**Player value:** Creates tension, replayability, and emotional stakes. Players must decide whether to investigate, confront, ignore, or exploit hidden agendas. Betrayals have lasting consequences. Trust becomes a meaningful resource.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` — ideological basis for agendas
- `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` — relationship foundation
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` — stress/crisis triggers
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — player morality interaction
- `Assets/Ashfall.Core/Factions/PrpfStandingSystem.cs` — faction loyalty example
- `Assets/StreamingAssets/Data/confession_secrets.json` — existing secret template
- `Assets/StreamingAssets/Data/survivors.json` — survivor archetypes
- NEW: `Assets/Ashfall.Core/Survivors/HiddenAgendaSystem.cs`
- NEW: `Assets/StreamingAssets/Data/hidden_agendas.json`

## Main Task 1 — Foundation / System Contract

1. Create `HiddenAgendaSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `HiddenAgenda` DTO: `survivorId`, `agendaType` (loyalty/theft/sabotage/escape/contact), `targetFactionId` or `targetSurvivorId`, `discoveryProgress` (0-100), `discovered` bool, `confronted` bool, `resolved` bool, `resolution` (betrayed/reconciled/exploited/ignored), `startDay`, `triggerConditions`
3. Define `HiddenAgendaState` DTO: list of active agendas, list of resolved agendas, discovery log
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define agenda types with distinct mechanics:
   - **Faction loyalty**: survivor secretly contacts faction, passes information
   - **Resource theft**: survivor steals supplies, hides stash
   - **Sabotage**: survivor damages shelter systems subtly
   - **Escape plan**: survivor planning to leave with resources
   - **Protection**: survivor hiding another survivor's secret
6. Create discovery mechanic: behavioral clues accumulate over time (suspicious absences, unexplained resource changes, system degradation patterns)
7. Define clue generation rules: each agenda type produces specific observable clues
8. Create investigation mechanic: player can assign survivors to investigate suspicions (cost: labor, risk: false accusation)
9. Implement confrontation system: player confronts survivor with evidence (discovery threshold must be met)
10. Define resolution branches: accusation succeeds/fails, survivor confessies/denies, player forgives/punishes/exploits
11. Wire into `SurvivorRelationsSystem`: agenda resolution permanently alters trust/resentment
12. Create `IHiddenAgendaSource` interface for systems that trigger agendas (ideological friction, faction standing, mental health crises)
13. Add deterministic seeding: agenda selection and clue timing use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupHiddenAgendas`, `TickAgendas`, `SaveAgendas`

## Main Task 2 — Implementation / Content / Branching

1. Implement faction loyalty agenda: survivor contacts PRPF/military/rebel faction, passes shelter information
   - Clues: unusual radio activity, missing during off-hours, faction standing shifts unexpectedly
   - Discovery: radio intercept, survivor observation, informant tip
   - Confrontation: evidence threshold 60%, survivor can deny if evidence < 80%
   - Branches:
     - **Expose and expel**: survivor leaves, faction standing penalty, shelter morale boost
     - **Turn double agent**: player feeds false information through survivor, faction standing gain
     - **Forgive and monitor**: survivor stays, trust permanently reduced, future agendas less likely
     - **Exploit for leverage**: player blackmails survivor, forced labor/loyalty, resentment maxed
2. Implement resource theft agenda: survivor stealing supplies
   - Clues: inventory discrepancies, survivor has unexplained items, stash discovered
   - Discovery: inventory audit, surveillance, accidental discovery
   - Confrontation: evidence threshold 50%, stash location revealed if > 70%
   - Branches:
     - **Recover and expel**: stolen goods recovered, survivor leaves
     - **Force repayment**: survivor works off debt, monitored
     - **Understand motive**: survivor had legitimate need, community discussion, no penalty
     - **Use stash as leverage**: player takes stash secretly, survivor confused
3. Implement sabotage agenda: survivor damaging shelter systems
   - Clues: system degradation patterns, maintenance logs show human error, survivor near incidents
   - Discovery: surveillance, pattern analysis, witness
   - Confrontation: evidence threshold 70% (sabotage hard to prove)
   - Branches:
     - **Immediate expulsion**: safety risk too high, no negotiation
     - **Forced repair**: survivor must fix damage under supervision
     - **Discover motivation**: survivor had ideological reason, debate/resolution
     - **Cover up**: player hides sabotage to avoid panic, system at risk
4. Implement escape plan agenda: survivor planning to leave with resources
   - Clues: survivor hoarding supplies, asking about routes, mapping exits
   - Discovery: caught packing, informant, diary found
   - Confrontation: evidence threshold 40% (escape is not betrayal per se)
   - Branches:
     - **Allow departure**: survivor leaves with personal items, neutral outcome
     - **Convince to stay**: player negotiates, survivor stays with conditions
     - **Confiscate resources**: survivor expelled, resources recovered
     - **Join escape**: player and survivor leave together (campaign-altering decision)
5. Implement protection agenda: survivor hiding another's secret
   - Creates nested secret layers
   - Discovery reveals both secrets
6. Create 15 agenda templates in `hidden_agendas.json` with trigger conditions
7. Implement delayed consequence: resolved agendas produce flags read by quest/faction systems
8. Create cross-agenda interaction: one survivor's agenda can trigger another's
9. Implement agenda immunity: some survivors never develop agendas (loyalty trait)
10. Add agenda prevention: high trust/affinity reduces agenda probability
11. Create agenda escalation: ignored clues lead to more severe actions
12. Implement informant mechanic: survivors can volunteer information about others
13. Add UI: "Suspicion Board" showing active investigations and clue progress

## Main Task 3 — Integration / Consequences / Validation

1. Wire resolved agendas into `MoralChoiceSystem`: betrayal/forgiveness affects morality band
2. Connect to faction systems: faction loyalty agendas affect faction standing
3. Integrate with quest system: agenda discoveries unlock confrontation quests
4. Connect to `SurvivorRelationsSystem`: resolution permanently alters relationship web
5. Wire into epilogue system: agenda resolutions affect ending evaluation
6. Implement old-save compatibility: existing saves get empty agenda state, survivors gain agendas over time
7. Add deterministic seeding: agenda selection uses `ISeededRng`, same seed → same agendas
8. Create exploit prevention: agendas have cooldowns, can't be reset by save/load
9. Add tests: agenda lifecycle (generate → discover → confront → resolve), save round-trip, determinism
10. Verify catalog integrity: all agenda target IDs resolve to real factions/survivors/locations
11. Test edge cases: all survivors expelled (no agenda targets), high-trust shelter (no agendas trigger)
12. Verify headless behavior: agendas tick correctly without UI
13. Add data-integrity-selftest: agenda templates validate against survivor/faction catalogs
14. Create `--hidden-agendas-selftest` verb for CI validation
15. Document agenda architecture for future expansion (new agenda types)

## Branching / Consequence Model

```text
Survivor develops hidden agenda (triggered by ideology/faction/need)
├─ Player unaware
│  ├─ Agenda progresses silently
│  │  ├─ Theft: resources gradually disappear
│  │  ├─ Sabotage: shelter systems degrade faster
│  │  ├─ Loyalty: faction learns shelter information
│  │  └─ Escape: survivor accumulates hidden stash
│  └─ Clues accumulate (observable but ambiguous)
├─ Player investigates
│  ├─ Assign investigator (labor cost)
│  │  ├─ Success: discovery progress +30%
│  │  └─ Failure: no progress, survivor alerted
│  ├─ Surveillance (direct observation)
│  │  ├─ Success: discovery progress +20%
│  │  └─ Failure: time wasted, survivor cautious
│  └─ Informant (survivor volunteers info)
│     ├─ Truthful: discovery progress +40%
│     └─ False accusation: innocent survivor targeted
├─ Player confronts (discovery ≥ threshold)
│  ├─ Evidence strong (>80%)
│  │  ├─ Survivor confesses
│  │  │  ├─ Expel: shelter morale +, trust in player +
│  │  │  ├─ Force repayment: resources recovered, survivor monitored
│  │  │  ├─ Forgive: trust permanently reduced, survivor grateful
│  │  │  └─ Exploit: survivor coerced, resentment maxed, future leverage
│  │  └─ Survivor denies (evidence insufficient)
│  │     ├─ Player backs down: survivor emboldened, agenda escalates
│  │     └─ Player insists: shelter divided, faction forms
│  └─ Evidence weak (<80%)
│     ├─ Survivor denies successfully: player looks paranoid
│     │  ├─ Shelter morale -
│     │  ├─ Accused survivor's allies turn against player
│     │  └─ Agenda continues with increased caution
│     └─ Player drops accusation: no immediate consequence
└─ Player ignores
   ├─ Agenda reaches completion
   │  ├─ Theft: major resource loss discovered
   │  ├─ Sabotage: critical system failure
   │  ├─ Loyalty: faction acts on information (raid, diplomatic pressure)
   │  └─ Escape: survivor gone with resources
   ├─ Shelter crisis triggered
   │  ├─ Morale penalty
   │  ├─ Trust in player reduced (should have caught it)
   │  └─ New quest: deal with consequences
   └─ Delayed discovery: clues revealed after the fact
      └─ Player can still investigate, but too late to prevent
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --hidden-agendas-selftest
```

## Risk

**HIGH** — Hidden agenda complexity can overwhelm players if too many agendas trigger simultaneously. Risk of false accusations creating frustration. Must balance clue visibility (too obvious = trivial, too hidden = unfair). Mitigation: cap active agendas at 2-3, make clues observable but ambiguous, allow investigation to reduce uncertainty.

## Definition of Done

- `HiddenAgendaSystem.cs` exists with full `CaptureState/RestoreState`
- 5 agenda types implemented (faction loyalty, theft, sabotage, escape, protection)
- Clue generation and investigation mechanics functional
- Confrontation system with evidence thresholds
- 4 resolution branches per agenda type
- Resolved agendas produce persistent flags read by other systems
- Save/load round-trip tested
- Deterministic agenda selection verified
- Old saves load without error
- 15 agenda templates in data authority
- UI panel shows suspicion board
- Cross-system integration (moral choice, factions, quests, relations)

## Follow-On Opportunities

- Counter-intelligence specialization (survivor skill)
- Interrogation mechanics (pressure vs. persuasion)
- Amnesty programs (formal forgiveness process)
- Secret alliances between survivors (positive hidden agendas)
- External informant network (hire spies in other settlements)
