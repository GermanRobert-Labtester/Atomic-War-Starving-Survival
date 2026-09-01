# Plan 215 — Shelter Resource Rationing & Crisis Management

## Goal

Create a shelter resource rationing and crisis management system where the shelter can implement rationing protocols during resource shortages, prioritize essential vs. non-essential consumption, manage crisis response, and make strategic decisions about resource allocation under pressure. Currently `DutyRosterSystem` has a single `mutationRationProtocol` boolean toggle, and resources are consumed from shared pools without prioritization — but there is no comprehensive rationing system, no resource priority levels, no crisis protocols, no rationing tiers, no emergency consumption management. When resources run low, the shelter has no strategic response beyond "run out." This plan adds resource crisis management.

## Why

**Repository evidence:** `DutyRosterSystem.cs` has `mutationRationProtocol` boolean (single toggle). No comprehensive rationing system exists. Resources consumed from shared pools without prioritization. No crisis protocols, no rationing tiers, no emergency consumption management, no resource priority levels.

**What is missing:** No comprehensive rationing system. No resource priority levels (essential/non-essential). No crisis protocols. No rationing tiers. No emergency consumption management. No strategic resource allocation under pressure. No "who gets what when resources are scarce" mechanics.

**Why existing plans don't solve it:** Plan 158 (Disaster & Emergency Response) covers acute crises (earthquakes, fires) but not resource shortages. Plan 201 (Sanitation) covers waste management. Plan 22 (One Food Authority) covers consumption tracking. No plan addresses resource rationing as a strategic system.

**Player value:** Creates strategic depth (make hard choices about who gets what), adds realism (resource shortages require management), generates emergent stories (rationing disputes, crisis decisions), and makes resource management more meaningful than just "don't run out."

## Files / Systems to Inspect

- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — existing ration protocol toggle
- `Assets/Ashfall.Core/Needs/NeedsSystem.cs` — survivor needs consumption
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — resource pools
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs` — food consumption
- NEW: `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs`
- NEW: `Assets/StreamingAssets/Data/rationing_protocols.json`

## Main Task 1 — Foundation / System Contract

1. Create `ResourceRationingSystem.cs` in `Assets/Ashfall.Core/Economy/`
2. Define `RationingProtocol` DTO: `protocolId`, `protocolName`, `protocolType` (standard/tight/emergency/crisis/triage), `activeResources` (list of resources being rationed), `rationLevels` (dict of resource → ration level), `priorityGroups` (list of priority groups with allocation percentages), `startDate`, `endDate` (-1 if indefinite), `status` (active/suspended/expired), `authorizedBy` (survivor_id)
3. Define `RationLevel` DTO: `resourceId`, `rationLevel` (full/three_quarter/half/third/quarter/minimal/none), `dailyAllocation` (units per survivor per day), `priorityModifier` (multiplier for priority groups), `effectiveDate`
4. Define `PriorityGroup` DTO: `groupId`, `groupName` (essential_personnel/children/elderly/medics/leadership/workers/general), `allocationPercentage` (0-100, share of available resources), `members` (list of survivor_ids), `priorityLevel` (1-5, 1 = highest)
5. Define `CrisisProtocol` DTO: `crisisId`, `crisisType` (food_shortage/water_shortage/medical_shortage/fuel_shortage/general_scarcity/multiple_shortage), `severity` (mild/moderate/severe/critical), `triggeredDay`, `activeProtocols` (list of rationing protocol_ids), `crisisResponse` (list of response actions), `resolvedDay` (-1 if unresolved)
6. Define `RationingEvent` DTO: `eventId`, `eventType` (rationing_started/rationing_escalated/rationing_lifted/resource_depleted/priority_dispute/crisis_declared/crisis_resolved/ration_violation), `day`, `resource` (resource_id), `description`, `severity`, `consequences` (list of effects)
7. Define `ResourceRationingState` DTO: list of active rationing protocols, list of priority groups, list of active crises, list of rationing events, rationing settings (auto-ration on shortage bool, crisis threshold, priority system enabled bool)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define rationing levels (7 levels):
   - **Full**: normal consumption (100%)
   - **Three-quarter**: 75% of normal
   - **Half**: 50% of normal
   - **Third**: 33% of normal
   - **Quarter**: 25% of normal
   - **Minimal**: survival minimum (10-15%)
   - **None**: no allocation (resource exhausted or reserved)
10. Define priority groups (7 groups):
    - **Essential Personnel**: critical workers (medics, engineers, security) — highest priority
    - **Children**: youngest survivors — high priority
    - **Elderly**: oldest survivors — moderate-high priority
    - **Medics**: medical staff — high priority
    - **Leadership**: shelter leaders — moderate priority
    - **Workers**: active workers — moderate priority
    - **General**: everyone else — lowest priority
11. Define crisis types (6 types):
    - **Food Shortage**: food supplies below threshold
    - **Water Shortage**: clean water below threshold
    - **Medical Shortage**: medical supplies below threshold
    - **Fuel Shortage**: fuel/power below threshold
    - **General Scarcity**: multiple resources low
    - **Multiple Shortage**: critical shortage of 3+ resources
12. Define crisis response:
    - Crisis declared when resource below threshold
    - Crisis triggers rationing protocols
    - Crisis response actions: rationing, priority allocation, emergency measures
    - Crisis resolved when resources above threshold
    - Crisis logged
13. Define rationing enforcement:
    - Rationing affects consumption rates
    - Priority groups get allocation percentage
    - Ration violations: taking more than allocated
    - Violations logged and penalized
    - Enforcement logged
14. Define rationing consequences:
    - Reduced rations: morale penalty, health effects
    - Priority allocation: some get more, some get less
    - Crisis: severe morale penalty, health risks, possible deaths
    - Consequences logged
15. Add deterministic seeding: rationing events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupResourceRationing`, `TickResourceRationing`, `SaveResourceRationing`

## Main Task 2 — Implementation / Protocols / Priority / Crisis / Enforcement / UI

1. Implement rationing protocols:
   - Protocol defines ration levels per resource
   - Protocol has priority groups with allocation
   - Protocol active/suspended/expired
   - Protocol logged
2. Implement priority groups:
   - Survivors assigned to priority groups
   - Groups have allocation percentage
   - Priority affects resource distribution
   - Groups logged
3. Implement crisis management:
   - Crisis declared when resource low
   - Crisis triggers protocols
   - Crisis response actions
   - Crisis resolved when resources recover
   - Crisis logged
4. Implement rationing enforcement:
   - Consumption adjusted by ration level
   - Priority groups get allocation
   - Violations detected and penalized
   - Enforcement logged
5. Implement rationing consequences:
   - Reduced rations: morale/health effects
   - Priority allocation: unequal distribution
   - Crisis: severe effects
   - Consequences logged
6. Implement rationing UI:
   - Rationing panel: active protocols, crisis status
   - Protocol detail: ration levels, priority groups
   - Priority panel: group assignments, allocation
   - Crisis panel: active crises, response actions
   - Resource status: current levels, rationing status
   - Violation log: ration violations
7. Create rationing events:
    - "The Rationing" — rationing protocol activated
    - "The Crisis" — resource crisis declared
    - "The Escalation" — rationing tightened
    - "The Relief" — rationing lifted
    - "The Depletion" — resource exhausted
    - "The Dispute" — priority allocation dispute
    - "The Violation" — ration violation detected
    - "The Resolution" — crisis resolved
8. Add rationing quest hooks:
    - "The Quartermaster" — manage 10 rationing protocols
    - "The Crisis Manager" — resolve 5 resource crises
    - "The Fair Dealer" — maintain zero ration violations for 100 days
    - "The Prioritizer" — successfully manage priority groups for 50 days
    - "The Survivor" — survive critical crisis (3+ resources low)
    - "The Strategist" — prevent 5 crises through proactive management
    - "The Leader" — make 10 difficult rationing decisions
9. Implement rationing tutorial: first rationing protocol explains system
10. Add rationing tooltips: hover over protocol shows details
11. Create rationing protocols in data file (10+ protocol templates)
12. Implement rationing persistence: protocols/crises saved
13. Integrate with `NeedsSystem`: rationing affects consumption

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `NeedsSystem`: rationing affects consumption rates
2. Connect to `Inventory`: resource levels trigger crises
3. Integrate with `KitchenNutritionSystem`: food rationing
4. Connect to `WaterTreatmentSystem`: water rationing
5. Wire into `PowerGridSystem`: fuel/power rationing
6. Connect to `MedicalPipelineCoordinator`: medical supply rationing
7. Wire into `LeadershipSystem`: leadership authorizes protocols
8. Connect to `InternalCommunicationSystem` (Plan 211): rationing announcements
9. Implement old-save compatibility: existing saves get no active rationing
10. Add deterministic seeding: rationing events use `ISeededRng`
11. Create exploit prevention: rationing is resource-based, can't be gamed
12. Add tests: protocols, priority groups, crises, enforcement, consequences, save round-trip
13. Verify all rationing levels work correctly
14. Test edge cases: no rationing (current behavior), full crisis (all resources low)
15. Verify headless behavior: rationing processes correctly without UI
16. Add data-integrity-selftest: rationing validates against resource/survivor catalogs
17. Create `--resource-rationing-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --resource-rationing-selftest
```

## Risk

**LOW** — Rationing is straightforward with clear inputs (resource levels) and outputs (protocols, crises). Risk of rationing feeling like spreadsheet management. Mitigation: make decisions meaningful (who gets what), show clear consequences, and ensure rationing feels like strategic leadership not just number-crunching.

## Definition of Done

- `ResourceRationingSystem.cs` exists with full `CaptureState/RestoreState`
- 7 rationing levels (full, three-quarter, half, third, quarter, minimal, none)
- 7 priority groups (essential personnel, children, elderly, medics, leadership, workers, general)
- 6 crisis types (food/water/medical/fuel/general/multiple shortage)
- Rationing protocols (per-resource levels, priority allocation)
- Crisis management (declaration, response, resolution)
- Rationing enforcement (consumption adjustment, violation detection)
- Rationing consequences (morale, health, unequal distribution)
- Rationing events and quest hooks
- Save/load round-trip tested
- Deterministic rationing events verified
- Old saves load with no active rationing
- Rationing protocols in data authority (10+ templates)
- UI rationing panel, protocol detail, priority panel, crisis panel, resource status, violation log
- Cross-system integration (needs, inventory, kitchen, water, power, medical, leadership, communication)

## Follow-On Opportunities

- Rationing specialization (survivors become expert quartermasters)
- Rationing legacy (famous crises remembered)
- Rationing quests (specific rationing goals)
- Rationing events (massive crisis, miraculous recovery)
- Rationing trading (trade resource management with other settlements)
