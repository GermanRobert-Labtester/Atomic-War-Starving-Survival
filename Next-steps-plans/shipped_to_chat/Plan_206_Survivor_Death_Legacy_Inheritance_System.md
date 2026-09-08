# Plan 206 — Survivor Death, Legacy & Inheritance System

## Goal

Create a survivor death, legacy, and inheritance system where each survivor's death is tracked with cause, creates a legacy record, and triggers distribution of personal possessions to designated heirs or the shelter commons. Currently survivors die (health reaches zero, radiation, combat) but death is a simple removal — no cause-of-death record, no last will, no inheritance of possessions, no memorial beyond `MemorialSystem`'s burial outcomes. A survivor who accumulated skills, items, and relationships simply vanishes. This plan makes death meaningful by ensuring each survivor's life leaves a trace.

## Why

**Repository evidence:** Grep for `DeathSystem`, `InheritanceSystem`, `LegacySystem`, `WillSystem`, `SuccessionSystem`, `MortalitySystem`, `DeathCause`, `CauseOfDeath`, `LastWill` in Core returns ZERO matches. `MemorialSystem.cs` (262 lines) handles burial outcomes and remembrance. `SurvivorFateSystem` tracks survivor alive/dead status. But no cause-of-death tracking, no last will/testament, no inheritance of possessions, no death record. When a survivor dies, their equipped items, personal effects, and uncompleted goals simply vanish.

**What is missing:** No cause-of-death tracking. No last will/testament system. No inheritance of possessions. No death record/certificate. No beneficiary designation. No legacy effects on remaining survivors (grief inheritance, motivation from death). Death is a boolean state change, not a meaningful event with consequences.

**Why existing plans don't solve it:** Plan 140 (Generational Legacy) covers cross-campaign inheritance (meta-progression between playthroughs). Plan 176 (Aging & Elderly) covers age-related death. Plan 198 (Health History) covers medical records for living survivors. Plan 200 (Personal Quests) covers character arcs. None address in-campaign inheritance when a survivor dies — what happens to their possessions, their unfinished quests, their designated heir.

**Player value:** Makes death meaningful (possessions don't just vanish), adds emotional weight (survivors name heirs), creates strategic decisions (who inherits what), and generates emergent stories (disputed wills, unexpected legacies).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/MemorialSystem.cs` — burial/remembrance (complementary)
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` — alive/dead tracking
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — item management
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships (heir selection)
- NEW: `Assets/Ashfall.Core/Survivors/SurvivorDeathLegacySystem.cs`
- NEW: `Assets/StreamingAssets/Data/inheritance_rules.json`

## Main Task 1 — Foundation / System Contract

1. Create `SurvivorDeathLegacySystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `DeathRecord` DTO: `recordId`, `survivorId`, `survivorName`, `deathDay`, `causeOfDeath` (starvation/dehydration/radiation/combat/disease/old_age/accident/murder/suicide/execution/unknown), `locationAtDeath` (location_id), `lastWords` (optional string), `witnesses` (list of survivor_ids present), `deathContext` (description of circumstances)
3. Define `LastWill` DTO: `willId`, `survivorId`, `createdDay`, `beneficiaries` (list of `BeneficiaryEntry`: survivor_id + item_category + percentage), `executor` (survivor_id who manages distribution), `specialBequests` (list of specific item → survivor assignments), `residuaryBeneficiary` (survivor_id who gets remainder), `witnesses` (list of survivor_ids), `isValid` bool
4. Define `BeneficiaryEntry` DTO: `beneficiaryId` (survivor_id), `category` (weapons/clothing/medical/food/tools/valuables/all), `percentage` (0-100, share of category), `notes` (optional)
5. Define `InheritanceDistribution` DTO: `distributionId`, `deathRecordId`, `willId`, `distributionDay`, `items` (list of `InheritedItem`), `disputes` (list of `InheritanceDispute`), `status` (pending/distributed/disputed/escrowed)
6. Define `InheritedItem` DTO: `itemId`, `originalOwnerId`, `recipientId`, `itemName`, `itemCategory`, `condition` (0-100), `sentimentalValue` (0-100, affects morale)
7. Define `InheritanceDispute` DTO: `disputeId`, `distributionId`, `disputantId` (survivor_id), `disputeType` (contested_bequest/excluded_heir/invalid_will/value_disagreement), `resolution` (mediated/upheld/overturned/dropped), `mediatorId` (survivor_id or null)
8. Define `LegacyEffect` DTO: `effectId`, `deceasedSurvivorId`, `effectType` (grief_motivation/inheritance_bonus/skill_memory/relationship_bond/trauma), `affectedSurvivorId`, `magnitude` (0-100), `duration` (days, -1 for permanent), `description`
9. Define `SurvivorDeathLegacyState` DTO: list of death records, list of wills, list of distributions, list of disputes, list of legacy effects, settings (auto-create wills bool, inheritance enabled bool, disputes enabled bool)
10. Implement `CaptureState/RestoreState` with schema versioning
11. Define cause-of-death categories (11 types):
    - **Starvation**: hunger reached zero
    - **Dehydration**: thirst reached zero
    - **Radiation**: cumulative radiation lethal
    - **Combat**: killed in tactical combat
    - **Disease**: fatal illness
    - **Old Age**: elderly age-related (Plan 176 integration)
    - **Accident**: shelter accident (fire, flood, structural)
    - **Murder**: killed by another survivor
    - **Suicide**: self-inflicted (mental health crisis)
    - **Execution**: judicial execution (Plan 159 integration)
    - **Unknown**: cause undetermined
12. Define will mechanics:
    - Survivors can create/update wills at any time
    - Wills designate beneficiaries by category (weapons, clothing, medical, etc.)
    - Wills can include special bequests (specific items to specific survivors)
    - Wills require witnesses (2+ survivors)
    - Wills can be contested if survivor was mentally incompetent
    - Intestate (no will): possessions distributed by relationship priority
13. Define inheritance distribution:
    - On death: will read, items distributed
    - Items go to designated beneficiaries
    - Sentimental items: morale bonus for recipient
    - Disputed items: held in escrow until resolution
    - Undistributed items: go to shelter commons
14. Define dispute mechanics:
    - Excluded heirs can contest will
    - Beneficiaries can dispute value分配
    - Disputes resolved through mediation or authority decision
    - Disputes affect relationships between survivors
15. Define legacy effects:
    - Death of close friend: grief motivation (skill bonus for period)
    - Inheritance receipt: morale boost
    - Witnessing death: trauma risk
    - Receiving sentimental item: relationship bond with deceased's memory
16. Add deterministic seeding: death events use `ISeededRng`
17. Wire into `GameBootstrap`: `SetupDeathLegacy`, `TickDeathLegacy`, `SaveDeathLegacy`

## Main Task 2 — Implementation / Records / Wills / Distribution / Disputes / Legacy / UI

1. Implement death record creation:
   - Auto-create death record when survivor dies
   - Record cause, location, witnesses, circumstances
   - Death record stored permanently
   - Death record accessible in memorial panel
2. Implement will creation:
   - Survivor can create will (player-initiated or auto)
   - Will designates beneficiaries by category
   - Will includes special bequests
   - Will requires witnesses
   - Will stored and can be updated
3. Implement inheritance distribution:
   - On death: will read automatically
   - Items distributed to beneficiaries
   - Sentimental items flagged
   - Distribution logged
   - If no will: intestate distribution (relationship priority)
4. Implement dispute system:
   - Excluded heirs can contest
   - Disputes filed against distribution
   - Mediation or authority resolution
   - Disputes affect relationships
5. Implement legacy effects:
   - Death triggers legacy effects on survivors
   - Grief motivation, trauma, morale changes
   - Effects logged
6. Implement death UI:
   - Death record panel: cause, location, witnesses
   - Will panel: create/edit will, designate beneficiaries
   - Inheritance panel: pending distributions, disputes
   - Legacy panel: active legacy effects
   - Memorial integration: death records feed into memorial
7. Create death events:
    - "The Death" — survivor died
    - "The Will" — will read and executed
    - "The Inheritance" — items distributed
    - "The Dispute" — inheritance contested
    - "The Legacy" — legacy effect triggered
    - "The Memorial" — death commemorated
    - "The Grief" — survivor affected by death
    - "The Testament" — survivor creates will
8. Add death quest hooks:
    - "The Executor" — execute 10 wills
    - "The Mediator" — resolve 5 inheritance disputes
    - "The Heir" — receive inheritance from 3 survivors
    - "The Witness" — witness 10 deaths
    - "The Planner" — create wills for all survivors
    - "The Peacemaker" — resolve 10 disputes without escalation
    - "The Legacy" — have 5 survivors leave lasting legacy effects
9. Implement death tutorial: first death explains system
10. Add death tooltips: hover over death record shows details
11. Create inheritance rules in data file
12. Implement death persistence: records/wills/distributions saved
13. Integrate with `MemorialSystem`: death records feed memorial

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MemorialSystem`: death records integrate with burial/remembrance
2. Connect to `SurvivorFateSystem`: death triggers record creation
3. Integrate with `Inventory`: items transferred via inheritance
4. Connect to `SurvivorRelationsSystem`: beneficiaries based on relationships
5. Wire into `InterpersonalConflictSystem` (Plan 202): disputes can trigger conflicts
6. Connect to `LeadershipSystem`: leader death triggers succession
7. Implement old-save compatibility: existing saves get no death records, no wills
8. Add deterministic seeding: death events use `ISeededRng`
9. Create exploit prevention: death is permanent, can't be gamed
10. Add tests: death records, wills, distribution, disputes, legacy effects, save round-trip
11. Verify all death causes work correctly
12. Test edge cases: no will (intestate), complex will (many beneficiaries), disputed will
13. Verify headless behavior: death legacy processes correctly without UI
14. Add data-integrity-selftest: death records validate against survivor/item catalogs
15. Create `--death-legacy-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --death-legacy-selftest
```

## Risk

**LOW** — Death/inheritance is straightforward with clear inputs (death events) and outputs (records, distributions). Risk of inheritance feeling like bookkeeping. Mitigation: make sentimental items meaningful, show emotional consequences, and ensure wills feel like personal choices not mechanical assignments.

## Definition of Done

- `SurvivorDeathLegacySystem.cs` exists with full `CaptureState/RestoreState`
- 11 cause-of-death categories
- Last will/testament system (beneficiaries, bequests, witnesses)
- Inheritance distribution (testate and intestate)
- Dispute mechanics (contested wills, mediation)
- Legacy effects (grief, trauma, morale, motivation)
- Death records (permanent, accessible)
- Death events and quest hooks
- Save/load round-trip tested
- Deterministic death events verified
- Old saves load with no death records, no wills
- Inheritance rules in data authority
- UI death record panel, will panel, inheritance panel, legacy panel
- Cross-system integration (memorial, fate, inventory, relations, conflicts, leadership)

## Follow-On Opportunities

- Death specialization (survivors become expert executors/mediators)
- Death legacy (famous deaths remembered across campaigns)
- Death quests (specific death-related goals)
- Death events (mass casualty event, heroic sacrifice)
- Death trading (trade inheritance services with other settlements)
