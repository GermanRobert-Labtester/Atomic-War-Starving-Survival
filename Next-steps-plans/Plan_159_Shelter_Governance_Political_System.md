# Plan 159 — Shelter Governance & Political System

## Goal

Create a shelter governance and political system where players make laws, set policies, manage resource allocation, administer justice, and navigate political factions within the shelter. Currently the shelter is managed by player fiat — there are no formal governance structures, no political factions, no justice system, no policy decisions. This plan adds a governance layer that makes the shelter a micro-society with political dynamics.

## Why

**Repository evidence:** `LeadershipSystem.cs` (referenced in late-game agent findings) supports leader designation with stress mechanics. `DutyRosterSystem.cs` assigns work. `MoralChoiceSystem.cs` tracks player morality. But there is no governance system — no laws, no policies, no political factions, no justice system, no resource allocation decisions. The shelter is a workplace, not a society.

**What is missing:** Players cannot establish laws or policies. There are no political factions within the shelter competing for influence. There is no justice system for resolving disputes. There are no governance decisions about resource allocation, immigration, defense policy, or social programs. The shelter has no political dimension.

**Why existing plans don't solve it:** Plan 12 (social/shelter life) covers friction and ration events but not governance. Plan 144 (survivor autonomy) adds autonomous behavior but not political organization. Plan 148 (ideological friction) creates belief-based tension but not political factions. Plan 150 (romance/family) adds social bonds but not governance. No plan addresses shelter governance or political systems.

**Player value:** Creates strategic depth (governance decisions have consequences), adds role-playing opportunities (what kind of society do you build?), generates emergent stories (political intrigue, justice dilemmas, policy debates), and makes the shelter feel like a community with shared values and conflicts.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/` — survivor systems
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — work assignment
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — moral framework
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — resource management
- NEW: `Assets/Ashfall.Core/Shelter/GovernanceSystem.cs`
- NEW: `Assets/StreamingAssets/Data/governance_policies.json`

## Main Task 1 — Foundation / System Contract

1. Create `GovernanceSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `Law` DTO: `lawId`, `lawType` (resource_allocation/immigration/justice/defense/social/economic), `description`, `effects` (list of modifiers), `enactedDay`, `repealedDay` (-1 if active), `supportLevel` (0-100), `oppositionLevel` (0-100)
3. Define `Policy` DTO: `policyId`, `policyType` (rationing/work_assignment/housing/medical/education/defense), `description`, `parameters` (map of settings), `active` bool, `affectedSurvivors` (list)
4. Define `PoliticalFaction` DTO: `factionId`, `factionName`, `ideology` (authoritarian/democratic/libertarian/collectivist/individualist), `leaderId`, `members` (list of survivor IDs), `influence` (0-100), `agenda` (list of preferred policies)
5. Define `JusticeCase` DTO: `caseId`, `caseType` (theft/violence/neglect/disobedience/corruption), `accusedId`, `victimId`, `evidence` (list), `verdict` (guilty/innocate/pending), `sentence` (fine/imprisonment/exile/community_service), `day`
6. Define `GovernanceState` DTO: list of laws, list of active policies, list of political factions, list of justice cases, governance type (autocracy/democracy/council/anarchy), stability rating (0-100)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define governance types:
   - **Autocracy**: player makes all decisions, no voting, fast but unpopular
   - **Democracy**: survivors vote on major decisions, slow but legitimate
   - **Council**: elected representatives make decisions, balanced
   - **Anarchy**: no formal governance, chaos, high freedom but low order
9. Define law categories:
   - **Resource allocation**: rationing, distribution priorities, luxury restrictions
   - **Immigration**: visitor admission, refugee policy, exclusion criteria
   - **Justice**: crime definitions, punishments, trial procedures
   - **Defense**: military service, weapon restrictions, external relations
   - **Social**: family policy, education requirements, religious freedom
   - **Economic**: trade policy, property rights, work requirements
10. Define political faction mechanics:
    - Factions form around ideological lines (Plan 148 integration)
    - Factions have leaders, members, influence levels
    - Factions support/oppose laws based on ideology
    - Factions compete for influence (propaganda, alliances, coercion)
    - High-influence factions can block laws or trigger revolts
11. Define justice system mechanics:
    - Crimes reported (theft, violence, neglect, disobedience)
    - Investigation (evidence gathering, witness testimony)
    - Trial (player judges or jury system)
    - Verdict and sentencing
    - Punishment enforcement (fines, imprisonment, exile, community service)
12. Add deterministic seeding: governance outcomes use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupGovernance`, `TickGovernance`, `SaveGovernance`
14. Create `GovernancePolicyCatalogLoader` for policy definitions
15. Create UI hook: governance panel showing laws, policies, factions, justice

## Main Task 2 — Implementation / Laws / Policies / Factions / Justice

1. Implement law enactment:
   - Player proposes law (or council votes)
   - Law debated (faction support/opposition)
   - Law voted on (if democracy/council) or enacted (if autocracy)
   - Law takes effect, modifies shelter operations
   - Law can be repealed (new vote or player decision)
2. Implement policy management:
   - Policies define how shelter operates day-to-day
   - Rationing policy: how food/water distributed
   - Work assignment: how duty roster determined
   - Housing: how rooms assigned
   - Medical: treatment priorities
   - Education: schooling requirements
   - Defense: military service requirements
3. Implement political faction system:
   - Factions form based on ideology (Plan 148 belief integration)
   - Authoritarian faction: order, discipline, hierarchy
   - Democratic faction: voting, rights, participation
   - Libertarian faction: freedom, minimal rules, individualism
   - Collectivist faction: community, shared resources, cooperation
   - Individualist faction: personal responsibility, merit, competition
   - Factions compete for influence through:
     - Propaganda (morale, speeches, posters)
     - Alliances (coalition building)
     - Coercion (threats, blackmail, intimidation)
     - Results (proving their ideology works)
4. Implement justice system:
   - Crimes occur (theft, violence, neglect, disobedience, corruption)
   - Crime reported to player (or council)
   - Investigation phase (gather evidence, interview witnesses)
   - Trial phase (player judges or jury of survivors)
   - Verdict: guilty or innocent
   - Sentencing: fine, imprisonment, exile, community service
   - Punishment enforced (fines deducted, prisoner assigned, exile executed)
5. Implement governance decisions:
   - Resource allocation: which needs prioritized (food vs. medicine vs. defense)
   - Immigration policy: who admitted (refugees, traders, enemies)
   - Defense policy: military service, weapon distribution
   - Social policy: family planning, education, religious freedom
   - Economic policy: trade, property, work requirements
6. Implement governance consequences:
   - Laws affect shelter operations (efficiency, morale, freedom)
   - Policies affect survivor behavior (compliance, resentment, cooperation)
   - Factions react to governance (support, opposition, revolt)
   - Justice affects order (deterrence, resentment, rehabilitation)
   - Governance type affects stability (autocracy fast but fragile, democracy slow but resilient)
7. Create governance events:
   - "The Debate" — factions argue over proposed law
   - "The Vote" — democracy in action, survivors vote
   - "The Trial" — justice system processes crime
   - "The Revolt" — faction opposes governance, unrest
   - "The Reform" — governance system changed (autocracy→democracy)
   - "The Crisis" — governance fails, anarchy threatens
   - "The Election" — new leaders chosen
8. Add governance quest hooks:
   - "The Constitution" — establish foundational governance document
   - "The Rebellion" — put down faction revolt
   - "The Trial" — high-profile justice case
   - "The Reform" — change governance system
   - "The Coalition" — build political alliance
   - "The Purge" — remove corrupt officials
   - "The Legacy" — establish lasting governance institutions
9. Implement governance UI:
   - Laws panel: active laws, proposed laws, voting
   - Policies panel: active policies, settings
   - Factions panel: faction info, influence, agendas
   - Justice panel: active cases, verdicts, punishments
   - Governance panel: governance type, stability, reforms
10. Create governance journal: automatic log of governance decisions
11. Implement governance tutorial: first governance decision explains system
12. Add governance tooltips: hover over law/policy shows effects
13. Create 20 laws, 15 policies, and 5 faction ideologies in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into shelter systems: laws/policies modify shelter operations
2. Connect to `DutyRosterSystem`: work assignment policies affect roster
3. Integrate with `MarketSystem`: economic policies affect trade
4. Connect to `MoralChoiceSystem`: governance decisions affect moral band
5. Wire into `IdeologicalFrictionSystem` (Plan 148): factions form from beliefs
6. Connect to `SurvivorRelationsSystem`: justice affects relationships
7. Implement old-save compatibility: existing saves get default governance state
8. Add deterministic seeding: governance uses `ISeededRng`
9. Create exploit prevention: governance decisions have cooldowns, factions have memory
10. Add tests: law enactment, policy effects, faction dynamics, justice, save round-trip
11. Verify catalog integrity: all law/policy/faction IDs resolve
12. Test edge cases: no governance (anarchy), max governance (total control)
13. Verify headless behavior: governance processes correctly without UI
14. Add data-integrity-selftest: governance definitions validate against catalogs
15. Create `--governance-selftest` verb for CI validation

## State / System Interaction Model

```text
Shelter governance
├─ Governance type established (autocracy/democracy/council/anarchy)
├─ Laws enacted
│  ├─ Proposed (by player or council)
│  ├─ Debated (faction support/opposition)
│  ├─ Voted (if democracy) or enacted (if autocracy)
│  ├─ Takes effect (modifies shelter operations)
│  └─ Can be repealed
├─ Policies set
│  ├─ Rationing, work assignment, housing, medical, education, defense
│  ├─ Policies define day-to-day operations
│  └─ Policies affect survivor behavior
├─ Political factions form
│  ├─ Based on ideology (authoritarian, democratic, libertarian, etc.)
│  ├─ Compete for influence (propaganda, alliances, coercion)
│  ├─ Support/oppose laws based on ideology
│  └─ High influence can block laws or trigger revolts
├─ Justice system operates
│  ├─ Crimes reported
│  ├─ Investigation (evidence, witnesses)
│  ├─ Trial (player judges or jury)
│  ├─ Verdict and sentencing
│  └─ Punishment enforced
└─ Governance consequences
   ├─ Laws affect efficiency, morale, freedom
   ├─ Policies affect behavior, compliance, resentment
   ├─ Factions react (support, oppose, revolt)
   ├─ Justice affects order, deterrence, rehabilitation
   └─ Governance type affects stability
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --governance-selftest
```

## Risk

**HIGH** — Governance complexity can overwhelm players if too many laws, policies, and factions exist. Risk of governance feeling like bureaucracy rather than meaningful decisions. Mitigation: start with simple governance (autocracy), unlock complexity gradually, make most decisions optional (can ignore governance and shelter runs itself), and provide clear UI showing governance status.

## Definition of Done

- `GovernanceSystem.cs` exists with full `CaptureState/RestoreState`
- 4 governance types implemented (autocracy, democracy, council, anarchy)
- 6 law categories functional (resource, immigration, justice, defense, social, economic)
- Policy management working (rationing, work, housing, medical, education, defense)
- Political faction system with 5 ideologies
- Justice system functional (crimes, investigation, trial, sentencing)
- Governance events and quest hooks
- Save/load round-trip tested
- Deterministic governance outcomes verified
- Old saves load without error
- 20 laws + 15 policies + 5 faction ideologies in data authority
- UI panel shows governance status
- Cross-system integration (duty roster, market, moral choice, ideological friction, survivor relations)

## Follow-On Opportunities

- Governance specialization (survivors become politicians, judges)
- Governance legacy (famous laws/factions remembered in epilogue)
- Governance quests (pass landmark legislation, reform system)
- Governance simulation (model governance outcomes before implementing)
- Governance diplomacy (inter-shelter governance agreements)
