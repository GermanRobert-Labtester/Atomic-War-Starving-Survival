# Plan 202 — Survivor Interpersonal Conflict & Grievance System

## Goal

Create a survivor interpersonal conflict and grievance system where survivors can have arguments, personality clashes, grudges, and disputes over resources, shifts, fairness, and personal slights — with conflict escalation, resolution mechanics, and lasting relationship consequences. Currently `IdeologicalFrictionSystem.cs` (158 lines) handles belief-based friction, and `SurvivorRelationsSystem.cs` tracks affinity — but there is no general interpersonal conflict system, no arguments, no grudges, no personality clashes, no disputes over resources or fairness, no conflict resolution mechanics. Survivors don't fight (verbally or socially) over anything except ideology. This plan adds non-ideological interpersonal conflict to make survivor social dynamics richer.

## Why

**Repository evidence:** Grep for `InterpersonalConflict`, `SurvivorConflict`, `ArgumentSystem`, `DisputeSystem`, `GrievanceSystem`, `FightSystem`, `SurvivorDispute`, `PersonalityClash` in Core returns ZERO matches. `IdeologicalFrictionSystem.cs` (158 lines) handles belief-based friction only. `SurvivorRelationsSystem.cs` tracks affinity but no conflict mechanics. Plan 148 (Ideological Friction Events) adds belief-driven events but not general interpersonal conflicts. Plan 144 (Survivor Autonomy) adds autonomous behavior but not conflict resolution. No general conflict/grievance system exists.

**What is missing:** No interpersonal conflict system. No arguments between survivors. No grudges from unfair treatment. No personality clashes. No disputes over resources, shifts, or fairness. No conflict escalation. No conflict resolution mechanics. No mediation. Survivors only fight over ideology (Plan 148), not over anything else.

**Why existing plans don't solve it:** Plan 148 (ideological friction) covers belief-based conflicts only. Plan 144 (survivor autonomy) adds autonomous behavior but not conflict. Plan 24 (food authority) mentions ration conflicts but doesn't implement. Plan 179 (psychology) adds psychological profiles but not interpersonal conflict. No plan addresses general interpersonal conflict as a system.

**Player value:** Creates social realism (people argue over resources, fairness, personality), adds strategic depth (manage conflicts, mediate disputes), generates emergent stories (grudges, arguments, reconciliations), and makes survivor relationships more dynamic than just "affinity number."

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` — ideological friction (complementary)
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationship affinity
- `Assets/Ashfall.Core/Survivors/SurvivorTraits.cs` — personality traits
- `Assets/Ashfall.Core/Needs/NeedsSystem.cs` — needs (hunger, stress)
- NEW: `Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs`
- NEW: `Assets/StreamingAssets/Data/conflict_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `InterpersonalConflictSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `InterpersonalConflict` DTO: `conflictId`, `conflictType` (argument/grudge/personality_clash/resource_dispute/shift_conflict/fairness_grievance/personal_slight/betrayal), `initiatorId` (survivor_id), `targetId` (survivor_id), `triggerEvent` (description of what caused conflict), `severity` (mild/moderate/severe/critical), `escalationLevel` (0-100), `resolutionStatus` (active/mediated/resolved/escalated/abandoned), `startedDay`, `resolvedDay` (-1 if unresolved), `notes` (additional details)
3. Define `Grievance` DTO: `grievanceId`, `holderId` (survivor who holds grudge), `targetId` (survivor grievance is against), `grievanceType` (unfair_ration/stolen_item/broken_promise/humiliation/neglect/betrayal/overworked), `severity` (0-100), `accumulationRate` (how fast grudge grows), `decayRate` (how fast grudge fades), `triggeredDay`, `lastIntensifiedDay`
4. Define `ConflictEscalation` DTO: `escalationId`, `conflictId`, `escalationType` (verbal_argument/social_isolation/sabotage/physical_fight/alliance_formation), `day`, `participants` (list of survivor_ids), `outcome` (resolved/escalated/injured/mediated), `consequences` (list of effects)
5. Define `ConflictResolution` DTO: `resolutionId`, `conflictId`, `resolutionType` (apology/mediation/compensation/punishment/time_heals/escalation), `mediatorId` (survivor_id or null), `day`, `outcome` (success/partial/failure), `relationshipEffect` (affinity change), `notes`
6. Define `InterpersonalConflictState` DTO: list of active conflicts, list of grievances per survivor, list of conflict escalations, list of resolutions, conflict settings (conflict frequency modifier, mediation enabled bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define conflict types (8+ types):
   - **Argument**: verbal disagreement over specific issue, resolves quickly or escalates
   - **Grudge**: accumulated resentment from repeated slights, fades slowly
   - **Personality Clash**: ongoing friction from incompatible personalities, constant low-level tension
   - **Resource Dispute**: conflict over food, water, equipment, room assignment
   - **Shift Conflict**: conflict over work assignments, shift timing, workload fairness
   - **Fairness Grievance**: perception of unfair treatment (rations, duties, privileges)
   - **Personal Slight**: perceived insult, disrespect, humiliation
   - **Betrayal**: broken trust, broken promise, theft, sabotage
9. Define conflict triggers:
   - **Unfair rations**: survivor perceives ration distribution as unfair
   - **Stolen item**: survivor's item taken/damaged by another
   - **Broken promise**: promised reward/favor not delivered
   - **Humiliation**: public embarrassment, criticism, mockery
   - **Overworked**: survivor assigned too much work, sees others idle
   - **Roommate conflict**: incompatible roommates (snoring, hygiene, habits)
   - **Jealousy**: survivor jealous of another's status, relationship, possessions
   - **Misunderstanding**: communication breakdown, false accusation
10. Define conflict escalation ladder:
    - **Level 0-20 (Tension)**: passive-aggressive behavior, cold shoulder
    - **Level 21-40 (Disagreement)**: verbal argument, complaints
    - **Level 41-60 (Conflict)**: active hostility, social isolation, sabotage
    - **Level 61-80 (Feud)**: ongoing grudge, alliance formation against target
    - **Level 81-100 (Crisis)**: physical fight, serious sabotage, relationship destroyed
11. Define conflict resolution mechanics:
    - **Apology**: initiator apologizes, reduces escalation
    - **Mediation**: third survivor mediates, reduces escalation
    - **Compensation**: initiator offers item/favor, reduces grievance
    - **Punishment**: authority figure punishes initiator, reduces grievance
    - **Time Heals**: grievance slowly decays over time
    - **Escalation**: conflict escalates to fight/crisis, relationship damaged
12. Define conflict consequences:
    - **Mild (0-20)**: minor morale penalty, slight affinity loss
    - **Moderate (21-50)**: morale penalty, affinity loss, possible work penalty
    - **Severe (51-80)**: significant morale penalty, major affinity loss, work disruption
    - **Critical (81-100)**: morale collapse, relationship destroyed, possible violence, shelter-wide tension
13. Define personality clash mechanics:
    - Some trait combinations create constant friction (e.g., aggressive + sensitive)
    - Personality clashes don't resolve easily, require ongoing management
    - Roommates with personality clashes: constant low-level tension
    - Personality clashes can escalate if not addressed
14. Define mediation mechanics:
    - Survivors with high social skills can mediate
    - Mediation success based on: mediator skill + relationship to both parties + conflict severity
    - Successful mediation: reduces escalation, opens resolution path
    - Failed mediation: mediator becomes involved in conflict
15. Add deterministic seeding: conflict generation uses `ISeededRng`
16. Wire into `GameBootstrap`: `SetupInterpersonalConflict`, `TickInterpersonalConflict`, `SaveInterpersonalConflict`

## Main Task 2 — Implementation / Conflicts / Grievances / Escalation / Resolution / UI

1. Implement conflict generation:
   - Evaluate survivor state daily (needs, stress, relationships)
   - Check for conflict triggers (unfair rations, stolen items, etc.)
   - Generate conflict based on trigger + survivor personality
   - Conflict assigned initiator and target
   - Conflict logged
2. Implement grievance accumulation:
   - Each survivor tracks grievances against others
   - Grievances accumulate from conflicts, slights, unfairness
   - Grievances decay slowly over time
   - High grievance: conflict more likely
   - Grievances logged
3. Implement conflict escalation:
   - Conflicts have escalation level (0-100)
   - Escalation increases from: unresolved conflict, new slights, stress
   - Escalation decreases from: resolution, time, apology
   - Escalation triggers events (argument, fight, etc.)
   - Escalation logged
4. Implement conflict resolution:
   - Player can initiate resolution (apology, mediation, compensation)
   - Survivors can auto-resolve (time heals, apology)
   - Resolution success based on: severity, relationship, mediator skill
   - Resolution affects affinity and grievances
   - Resolution logged
5. Implement mediation:
   - Player assigns mediator survivor
   - Mediator attempts to resolve conflict
   - Success based on: mediator social skill + relationships + severity
   - Successful mediation: reduces escalation
   - Failed mediation: mediator drawn into conflict
6. Implement personality clashes:
   - Trait combinations create constant friction
   - Personality clashes don't resolve easily
   - Roommates with clashes: constant tension
   - Personality clashes require management (room reassignment, mediation)
7. Implement conflict consequences:
   - Low escalation: minor morale penalty
   - Moderate escalation: work disruption, affinity loss
   - High escalation: violence, relationship destruction
   - Shelter-wide tension from multiple conflicts
8. Implement conflict UI:
   - Conflict panel: active conflicts, grievances
   - Conflict detail: type, severity, escalation, participants
   - Resolution panel: mediation options, apology, compensation
   - Grievance display: per-survivor grievances
   - Alerts: conflict escalation, fight breaking out
   - Conflict log: history of conflicts/resolutions
9. Create conflict events:
    - "The Argument" — verbal disagreement
    - "The Grudge" — resentment accumulated
    - "The Fight" — physical altercation
    - "The Mediation" — third party intervenes
    - "The Apology" — initiator apologizes
    - "The Betrayal" — trust broken
    - "The Reconciliation" — conflict resolved
    - "The Feud" — ongoing hostility
10. Add conflict quest hooks:
    - "The Peacemaker" — mediate 10 conflicts
    - "The Diplomat" — resolve 5 feuds
    - "The Leader" — prevent 3 fights
    - "The Judge" — fairly resolve 10 disputes
    - "The Counselor" — help 5 survivors with grievances
    - "The Harmonizer" — maintain zero active conflicts for 30 days
    - "The Mediator" — successfully mediate 20 conflicts
11. Implement conflict tutorial: first argument explains system
12. Add conflict tooltips: hover over conflict shows details
13. Create conflict templates in data file (20+ templates)
14. Implement conflict persistence: conflicts/grievances saved with survivor state
15. Integrate with `SurvivorRelationsSystem`: conflicts affect affinity

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `IdeologicalFrictionSystem`: ideological friction can trigger interpersonal conflicts
2. Connect to `SurvivorRelationsSystem`: conflicts affect affinity
3. Integrate with `NeedsSystem`: stress/hunger increase conflict likelihood
4. Connect to `DutyRosterSystem`: shift conflicts, workload fairness
5. Wire into `MoralChoiceSystem`: conflict resolution affects morality
6. Connect to `TraitSystem`: personality clashes from trait combinations
7. Implement old-save compatibility: existing saves get no active conflicts
8. Add deterministic seeding: conflict generation uses `ISeededRng`
9. Create exploit prevention: conflicts are state-based, can't be gamed
10. Add tests: conflict generation, escalation, resolution, grievances, mediation, save round-trip
11. Verify all conflict types work correctly
12. Test edge cases: no conflicts (current behavior), many conflicts (shelter in chaos)
13. Verify headless behavior: conflicts process correctly without UI
14. Add data-integrity-selftest: conflicts validate against survivor/trait catalogs
15. Create `--interpersonal-conflict-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --interpersonal-conflict-selftest
```

## Risk

**LOW** — Interpersonal conflicts are straightforward with clear inputs (triggers, grievances) and outputs (escalation, resolution). Risk of conflicts feeling like random noise. Mitigation: base conflicts on survivor state/traits/relationships, show clear cause-effect, ensure resolution is meaningful, and make conflicts feel realistic not arbitrary.

## Definition of Done

- `InterpersonalConflictSystem.cs` exists with full `CaptureState/RestoreState`
- 8+ conflict types (argument, grudge, personality clash, resource dispute, shift conflict, fairness grievance, personal slight, betrayal)
- Conflict triggers (unfair rations, stolen items, broken promises, humiliation, overwork, roommate conflict, jealousy, misunderstanding)
- Conflict escalation ladder (0-100, 5 levels)
- Conflict resolution mechanics (apology, mediation, compensation, punishment, time heals)
- Grievance accumulation and decay
- Mediation system (third-party intervention)
- Personality clash mechanics (trait-based friction)
- Conflict consequences (morale, affinity, work disruption, violence)
- Conflict events and quest hooks
- Save/load round-trip tested
- Deterministic conflict generation verified
- Old saves load with no active conflicts
- Conflict templates in data authority (20+ templates)
- UI conflict panel, resolution panel, grievance display, alerts, log
- Cross-system integration (ideological friction, relations, needs, duty roster, morality, traits)

## Follow-On Opportunities

- Conflict specialization (survivors become expert mediators/counselors)
- Conflict legacy (famous feuds remembered)
- Conflict quests (specific conflict resolution goals)
- Conflict events (shelter-wide civil war, mass reconciliation)
- Conflict trading (trade conflict resolution services with other settlements)
