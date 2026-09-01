# Plan 153 — Faction Espionage & Sabotage

## Goal

Create a faction espionage and sabotage system where players can infiltrate rival factions, steal intelligence, sabotage operations, and conduct covert operations. Currently faction interactions are limited to open diplomacy, trade, and combat — there is no stealth, espionage, or covert action layer. This plan adds a shadow war dimension to faction relations, creating new strategic options and moral dilemmas.

## Why

**Repository evidence:** `FactionBranchCoordinator.cs` coordinates faction branches with standing, trust, and diplomatic interactions. `FactionStanceEngine.cs` tracks per-faction trust and trade stances. `HoldfastTradeSession.cs` handles faction-gated trade. But all faction interactions are overt — diplomacy, trade, combat. No system exists for covert operations, espionage, infiltration, or sabotage. The cross-system agent confirmed: faction systems have no stealth/espionage layer.

**What is missing:** Players cannot spy on factions, steal their secrets, sabotage their operations, or conduct covert actions. All faction interactions are above-board. There's no shadow war, no intelligence gathering, no covert ops. This limits strategic options and removes a major genre element of post-collapse survival.

**Why existing plans don't solve it:** Plan 134 (territory control) adds faction competition but not espionage. Plan 139 (combat→faction) connects combat to standing but not covert action. Plan 147 (per-NPC memory) adds NPC relationships but not espionage. No plan addresses faction espionage or sabotage mechanics.

**Player value:** Creates strategic depth (covert options alongside overt diplomacy), adds moral dilemmas (spy on allies? sabotage enemies?), generates emergent stories (infiltration discovered, double agents, intelligence coups), and makes faction interactions more varied and interesting.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction coordination
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` — faction trust/stance
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition mechanics
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — moral choices
- `Assets/StreamingAssets/Data/faction_lore.json` — faction definitions
- NEW: `Assets/Ashfall.Core/Factions/EspionageSystem.cs`
- NEW: `Assets/StreamingAssets/Data/espionage_operations.json`

## Main Task 1 — Foundation / System Contract

1. Create `EspionageSystem.cs` in `Assets/Ashfall.Core/Factions/`
2. Define `EspionageOperation` DTO: `operationId`, `operationType` (infiltrate/steal/sabotage/assassinate/propaganda), `targetFactionId`, `assignedAgentId` (survivor ID), `successChance` (0-100), `riskLevel` (low/medium/high/extreme), `duration` (days), `status` (planned/active/succeeded/failed/compromised)
3. Define `IntelligenceReport` DTO: `reportId`, `sourceFactionId`, `intelligenceType` (military/economic/political/technical), `value` (0-100), `accuracy` (0-100), `dayObtained`, `decoded` bool
4. Define `EspionageState` DTO: list of active operations, list of intelligence reports, list of compromised agents, faction suspicion levels
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define operation types:
   - **Infiltrate**: place agent in faction, long-term intelligence gathering
   - **Steal**: extract specific intelligence (tech, plans, codes)
   - **Sabotage**: damage faction operations (destroy supplies, disrupt trade)
   - **Assassinate**: eliminate key faction figure (extreme risk)
   - **Propaganda**: spread disinformation, reduce faction standing
7. Define espionage mechanics:
   - Operations require skilled agents (high stealth, intelligence skills)
   - Success chance based on agent skill, faction security, operation difficulty
   - Risk of discovery (agent compromised, faction hostility increases)
   - Failed operations have consequences (agent captured, faction standing penalty)
   - Successful operations provide intelligence, sabotage effects, or standing changes
8. Define intelligence system:
   - Intelligence reports have value and accuracy
   - Raw intelligence must be decoded (time, skill required)
   - Intelligence can be false (disinformation from faction)
   - Intelligence informs player decisions (faction weaknesses, plans)
9. Define suspicion mechanics:
   - Each faction has suspicion level toward player (0-100)
   - Espionage operations increase suspicion
   - High suspicion: faction increases security, harder operations
   - Extreme suspicion: faction becomes hostile, agents at risk
   - Suspicion decays over time if no operations
10. Add deterministic seeding: espionage outcomes use `ISeededRng`
11. Wire into `GameBootstrap`: `SetupEspionage`, `TickEspionage`, `SaveEspionage`
12. Create `EspionageOperationCatalogLoader` for operation definitions
13. Implement agent recruitment: survivors with stealth/intelligence skills
14. Create UI hook: espionage panel showing operations, intelligence, suspicion

## Main Task 2 — Implementation / Operations / Intelligence / Consequences

1. Implement infiltration operations:
   - Agent placed in faction for 30-90 days
   - Agent gathers intelligence passively
   - Risk of discovery increases over time
   - Successful infiltration: steady intelligence flow
   - Compromised agent: captured or killed, faction hostility
2. Implement theft operations:
   - Target specific intelligence (tech blueprints, trade routes, military plans)
   - Operation duration 7-30 days
   - Success: intelligence obtained
   - Failure: agent caught, suspicion increases
   - Stolen intelligence can be decoded and used
3. Implement sabotage operations:
   - Target faction operations (supply lines, production, trade)
   - Sabotage reduces faction effectiveness temporarily
   - Risk of discovery high
   - Successful sabotage: faction weakened, standing penalty
   - Failed sabotage: agent captured, major standing penalty
4. Implement assassination operations:
   - Target key faction figure (leader, specialist)
   - Extreme risk, extreme reward
   - Success: faction leader eliminated, chaos in faction
   - Failure: agent killed, faction becomes permanently hostile
   - Moral choice: assassination has moral band impact
5. Implement propaganda operations:
   - Spread disinformation about faction
   - Reduces faction standing with other factions
   - Low risk, low reward
   - Can turn factions against each other
   - Moral choice: deception has moral band impact
6. Implement intelligence decoding:
   - Raw intelligence must be decoded by skilled survivor
   - Decoding takes time (1-7 days)
   - Decoded intelligence reveals faction weaknesses, plans
   - Intelligence can be false (faction disinformation)
   - Intelligence informs strategic decisions
7. Implement agent management:
   - Agents need training (stealth, intelligence, disguise skills)
   - Agents can be compromised (captured, turned)
   - Compromised agents can be ransomed or left
   - Successful agents gain experience and reputation
   - Agent death affects shelter morale
8. Create espionage events:
   - "The Mole" — agent successfully infiltrates faction
   - "The Heist" — steal valuable intelligence
   - "The Sabotage" — disrupt faction operations
   - "The Assassination" — eliminate faction leader
   - "The Cover-Up" — hide evidence of espionage
   - "The Double Agent" — agent turns against player
   - "The Intelligence Coup" — major intelligence breakthrough
9. Add espionage quest hooks:
   - "The Spy Game" — build espionage network against rival faction
   - "The Mole Hunt" — discover which faction is spying on you
   - "The Defector" — faction member offers to switch sides
   - "The Intelligence War" — compete with faction for secrets
   - "The False Flag" — frame another faction for sabotage
10. Implement espionage consequences:
    - Successful espionage: faction weakened, player advantage
    - Failed espionage: faction hostility, agent lost
    - Discovered espionage: faction becomes enemy, possible war
    - Espionage against allies: moral penalty, standing loss
    - Espionage saves: intelligence can prevent disasters
11. Add UI: espionage panel showing operations, intelligence, suspicion levels
12. Create espionage journal: automatic log of espionage events
13. Implement espionage tutorial: first operation explains system
14. Add espionage tooltips: hover over operation shows success chance and risks
15. Create 15 espionage operation templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionBranchCoordinator`: espionage affects faction standing
2. Connect to `FactionStanceEngine`: suspicion modifies trust
3. Integrate with `ExpeditionSystem`: espionage operations during expeditions
4. Connect to `MoralChoiceSystem`: espionage choices affect moral band
5. Wire into `SurvivorRelationsSystem`: agent relationships affect missions
6. Connect to `TacticalCombatSystem`: discovered agents may fight
7. Implement old-save compatibility: existing saves get empty espionage state
8. Add deterministic seeding: espionage outcomes use `ISeededRng`
9. Create exploit prevention: operations have cooldowns, suspicion prevents spam
10. Add tests: operation resolution, intelligence decoding, suspicion, save round-trip
11. Verify catalog integrity: all faction/survivor IDs resolve
12. Test edge cases: no espionage (no operations), max suspicion (all factions hostile)
13. Verify headless behavior: espionage processes correctly without UI
14. Add data-integrity-selftest: operation templates validate against faction catalogs
15. Create `--espionage-selftest` verb for CI validation

## State / System Interaction Model

```text
Player initiates espionage operation
├─ Select operation type (infiltrate/steal/sabotage/assassinate/propaganda)
├─ Assign agent (survivor with stealth/intelligence skills)
├─ Operation begins
│  ├─ Duration: 7-90 days depending on type
│  ├─ Success chance: agent skill vs. faction security
│  ├─ Risk: discovery, agent compromise
│  └─ Faction suspicion increases
├─ Operation resolves
│  ├─ Success: intelligence/sabotage/assassination achieved
│  │  ├─ Intelligence: decoded, used for strategic advantage
│  │  ├─ Sabotage: faction weakened temporarily
│  │  ├─ Assassination: faction leader eliminated
│  │  └─ Propaganda: faction standing reduced
│  ├─ Failure: operation fails
│  │  ├─ Agent escapes: suspicion increases
│  │  ├─ Agent captured: can be ransomed or left
│  │  └─ Agent killed: morale penalty
│  └─ Compromised: operation discovered
│     ├─ Faction becomes hostile
│     ├─ Agent captured/killed
│     └─ Major standing penalty
├─ Intelligence gathered
│  ├─ Raw intelligence obtained
│  ├─ Decoded by skilled survivor
│  ├─ Reveals faction weaknesses/plans
│  └─ Informs player decisions
└─ Suspicion management
   ├─ Suspicion increases with operations
   ├─ High suspicion: harder operations
   ├─ Extreme suspicion: faction hostile
   └─ Suspicion decays over time
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --espionage-selftest
```

## Risk

**HIGH** — Espionage system complexity can overwhelm players if too many operation types and consequences exist. Risk of espionage making faction systems too easy (just spy on everyone). Mitigation: high failure rates, severe consequences for discovery, suspicion mechanics that limit spam, and moral costs for covert actions.

## Definition of Done

- `EspionageSystem.cs` exists with full `CaptureState/RestoreState`
- 5 operation types implemented (infiltrate, steal, sabotage, assassinate, propaganda)
- Espionage mechanics functional (success chance, risk, consequences)
- Intelligence gathering and decoding working
- Suspicion system tracking faction awareness
- Agent management (training, compromise, death)
- Espionage events and quest hooks
- Save/load round-trip tested
- Deterministic espionage outcomes verified
- Old saves load without error
- 15 espionage operation templates in data authority
- UI panel shows operations, intelligence, suspicion
- Cross-system integration (factions, expeditions, moral choice, combat, survivor relations)

## Follow-On Opportunities

- Counter-espionage (detect and stop enemy spies)
- Double agents (turn enemy agents to your side)
- Intelligence trading (sell intelligence to other factions)
- Espionage specialization (survivors become spymasters)
- Espionage legacy (famous operations remembered in epilogue)
