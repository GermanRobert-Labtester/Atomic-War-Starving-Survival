# Plan 208 — Leadership Succession & Challenge System

## Goal

Extend the existing `LeadershipSystem` (288 lines) with succession mechanics — what happens when the leader dies, is incapacitated, or faces a challenge from other survivors. Currently `LeadershipSystem` handles leader designation, stress accumulation, and crisis morale aura — but there is no succession planning, no leadership challenges, no elections, no leadership transfer on death, no deputy/second-in-command. When the leader dies, leadership simply vanishes. This plan adds continuity and political dynamics to shelter leadership.

## Why

**Repository evidence:** `LeadershipSystem.cs` (288 lines) tracks `current_leader_id`, `is_designated_leader`, `leader_stress_accumulation`, `step_down_cooldown`. Has `LeaderCrisisMoraleAura`, `LeaderStressPerDeath`, `LeaderStressDecayPerDay`. Full `CaptureState`/`RestoreState`. But no succession mechanics — no deputy, no election, no challenge, no transfer on death. When leader dies, `current_leader_id` becomes invalid with no replacement mechanism. Plan 159 (Shelter Governance) mentions "The Election" as a quest hook but doesn't implement succession mechanics.

**What is missing:** No succession planning. No deputy/second-in-command. No leadership challenges. No elections. No leadership transfer on death. No leadership contests. No term limits. No recall mechanics. Leadership is a static designation with no continuity.

**Why existing plans don't solve it:** Plan 159 (governance) covers political systems broadly but not leadership succession specifically. Plan 144 (survivor autonomy) adds autonomous behavior but not leadership mechanics. No plan addresses leadership succession as a system.

**Player value:** Creates political depth (leadership isn't permanent), adds strategic planning (designate successor), generates emergent stories (challenges, elections, power struggles), and ensures leadership continuity (shelter never leaderless).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` — existing leadership (288 lines, extend)
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships (challenge support)
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — leadership skill
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — morality (legitimacy)
- NEW: extend `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` with succession
- NEW: `Assets/StreamingAssets/Data/leadership_rules.json`

## Main Task 1 — Foundation / System Contract

1. Extend `LeadershipSystem.cs` with succession DTOs
2. Define `SuccessionPlan` DTO: `planId`, `leaderId`, `designatedSuccessor` (survivor_id), `backupSuccessor` (survivor_id), `createdDay`, `updatedDay`, `isActive` bool
3. Define `LeadershipChallenge` DTO: `challengeId`, `challengerId` (survivor_id), `challengedLeaderId`, `challengeType` (election/coup/contest/recall), `reason` (description), `supporters` (list of survivor_ids), `opponents` (list of survivor_ids), `challengeDay`, `resolutionDay` (-1 if unresolved), `outcome` (pending/challenger_wins/leader_wins/withdrawn/compromise), `voteResults` (dict of survivor_id → vote)
4. Define `LeadershipElection` DTO: `electionId`, `candidates` (list of survivor_ids), `voters` (list of eligible survivor_ids), `electionDay`, `voteDeadline` (day), `results` (dict of candidate_id → vote_count), `winner` (survivor_id), `turnout` (0-100), `legitimacy` (0-100)
5. Define `LeadershipTransfer` DTO: `transferId`, `fromLeaderId`, `toLeaderId`, `transferType` (succession_on_death/step_down/challenge_victory/election_victory/appointment), `transferDay`, `reason`, `isVoluntary` bool, `isContested` bool
6. Define `DeputyLeader` DTO: `deputyId`, `leaderId`, `deputySurvivorId`, `appointedDay`, `powers` (list of delegated authorities), `isActive` bool
7. Define `LeadershipLegitimacy` DTO: `legitimacyId`, `leaderId`, `legitimacyScore` (0-100), `factors` (list of legitimacy modifiers: elected/appointed/popular_support/competence/morality/crisis_performance), `lastUpdatedDay`
8. Extend `LeadershipState` DTO: add succession plan, active challenges, election history, transfer history, deputy leader, legitimacy score
9. Implement `CaptureState/RestoreState` extension with schema versioning
10. Define succession mechanics:
    - Leader can designate successor (and backup)
    - On leader death/incapacitation: successor automatically becomes leader
    - If no successor: emergency election triggered
    - Succession logged
11. Define leadership challenge mechanics:
    - Any survivor can challenge leader (if legitimacy low or crisis)
    - Challenge requires supporters (minimum 30% of shelter)
    - Challenge resolved by vote (all survivors vote)
    - Challenge winner becomes leader
    - Failed challenge: challenger penalized (morale, relationships)
    - Challenge logged
12. Define election mechanics:
    - Elections triggered by: leader death with no successor, successful challenge, voluntary step-down, scheduled election (term limits)
    - Candidates declared (any survivor with minimum support)
    - Campaign period (optional)
    - Vote by all eligible survivors
    - Winner becomes leader
    - Election logged
13. Define deputy mechanics:
    - Leader can appoint deputy
    - Deputy has delegated powers
    - Deputy acts as successor if no formal succession plan
    - Deputy can be removed by leader
    - Deputy logged
14. Define legitimacy mechanics:
    - Leader legitimacy (0-100) based on: how they gained power, performance, morality, popular support
    - Low legitimacy: challenges more likely, morale penalty
    - High legitimacy: challenges harder, morale bonus
    - Legitimacy changes over time based on performance
15. Define term limits (optional):
    - Leaders serve fixed terms (e.g., 90 days)
    - Term end: automatic election triggered
    - Leader can run for re-election
    - Term limits configurable
16. Add deterministic seeding: leadership events use `ISeededRng`
17. Wire into `GameBootstrap`: extend `SetupLeadership`, `TickLeadership`, `SaveLeadership`

## Main Task 2 — Implementation / Succession / Challenges / Elections / Deputy / Legitimacy / UI

1. Implement succession planning:
   - Leader designates successor
   - Succession plan stored
   - On leader death: successor becomes leader
   - If no successor: emergency election
   - Succession logged
2. Implement leadership challenges:
   - Survivor initiates challenge
   - Challenge requires supporters
   - Vote by all survivors
   - Challenge winner becomes leader
   - Failed challenge: penalties
   - Challenge logged
3. Implement elections:
   - Election triggered (death, challenge, term end, step-down)
   - Candidates declared
   - Campaign period
   - Vote counting
   - Winner becomes leader
   - Election logged
4. Implement deputy system:
   - Leader appoints deputy
   - Deputy has delegated powers
   - Deputy acts as automatic successor
   - Deputy can be removed
   - Deputy logged
5. Implement legitimacy tracking:
   - Legitimacy score calculated from factors
   - Legitimacy affects challenge difficulty
   - Legitimacy affects morale
   - Legitimacy logged
6. Implement term limits (optional):
   - Terms configured
   - Term end triggers election
   - Re-election allowed
   - Terms logged
7. Implement leadership UI:
   - Leadership panel: current leader, legitimacy, term remaining
   - Succession panel: designated successor, backup
   - Challenge panel: active challenges, vote
   - Election panel: candidates, results
   - Deputy panel: current deputy, powers
   - Leadership history: past leaders, transfers
8. Create leadership events:
    - "The Succession" — leader succeeded
    - "The Challenge" — leadership challenged
    - "The Election" — election held
    - "The Deputy" — deputy appointed
    - "The Step Down" — leader voluntarily steps down
    - "The Coup" — leadership seized by force
    - "The Mandate" — leader re-elected
    - "The Crisis" — leadership vacuum
9. Add leadership quest hooks:
    - "The Leader" — become shelter leader
    - "The Kingmaker" — elect 3 leaders
    - "The Challenger" — successfully challenge a leader
    - "The Successor" — succeed to leadership
    - "The Democrat" — hold 5 fair elections
    - "The Stabilizer" — maintain 90+ legitimacy for 100 days
    - "The Reformer" — implement term limits
10. Implement leadership tutorial: first leadership transition explains system
11. Add leadership tooltips: hover over leader shows legitimacy, term
12. Create leadership rules in data file
13. Implement leadership persistence: succession/challenges/elections saved
14. Integrate with `SurvivorRelationsSystem`: relationships affect challenge support

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `LeadershipSystem`: extend existing system
2. Connect to `SurvivorRelationsSystem`: relationships affect support
3. Integrate with `MoralChoiceSystem`: morality affects legitimacy
4. Connect to `SkillProgressionSystem`: leadership skill affects performance
5. Wire into `InterpersonalConflictSystem` (Plan 202): challenges can trigger conflicts
6. Connect to `DeathLegacySystem` (Plan 206): leader death triggers succession
7. Implement old-save compatibility: existing saves get no succession plan, current leader retains position
8. Add deterministic seeding: leadership events use `ISeededRng`
9. Create exploit prevention: legitimacy is performance-based, can't be gamed
10. Add tests: succession, challenges, elections, deputy, legitimacy, save round-trip
11. Verify all leadership transitions work correctly
12. Test edge cases: no leader (current behavior), frequent challenges (political instability)
13. Verify headless behavior: leadership processes correctly without UI
14. Add data-integrity-selftest: leadership validates against survivor catalogs
15. Create `--leadership-succession-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --leadership-succession-selftest
```

## Risk

**LOW** — Leadership succession is straightforward with clear inputs (death, challenges) and outputs (transfers, elections). Risk of politics feeling like overhead. Mitigation: make legitimacy meaningful, show clear consequences, and ensure leadership transitions feel natural not forced.

## Definition of Done

- `LeadershipSystem.cs` extended with succession DTOs
- Succession planning (designated successor, backup)
- Leadership challenges (initiation, supporters, vote, outcome)
- Elections (triggered, candidates, campaign, voting, results)
- Deputy system (appointment, powers, automatic succession)
- Legitimacy tracking (score, factors, effects)
- Optional term limits
- Leadership events and quest hooks
- Save/load round-trip tested
- Deterministic leadership events verified
- Old saves load with no succession plan, current leader retains
- Leadership rules in data authority
- UI leadership panel, succession panel, challenge panel, election panel, deputy panel, history
- Cross-system integration (leadership, relations, morality, skills, conflicts, death legacy)

## Follow-On Opportunities

- Leadership specialization (survivors develop leadership skills)
- Leadership legacy (famous leaders remembered)
- Leadership quests (specific leadership goals)
- Leadership events (legendary leadership, catastrophic failure)
- Leadership trading (trade political support with other settlements)
