# Plan 217 — Survivor Genealogy & Family Tree System

## Goal

Extend the existing `GenerationalLineageExtension` (119 lines, Core, unwired) and `GenerationalSuccessionEngine` (150 lines, Core.Legacy, wired to `ExpansionHostSession`) into a full genealogy and family tree system — wiring both into the Godot host, adding sibling/spouse detection, family units with naming, family events (birth/death/marriage/adoption/reunion/schism/milestone), multi-generational ancestor/descendant traces, family tree visualization, and lineage effects (kinship affinity bonuses, inherited trait tracking). Currently the engine tracks generation indices and mentor-apprentice transfer for Expansion 12 "The Century Seed," and the extension adds basic parent-child `LineageRecord` links — but neither is wired into the main game loop, neither provides family grouping, neither has a UI, and neither tracks the full genealogical picture. This plan bridges Core → host and extends both into a visible, interactive genealogy layer.

## Why

**Repository evidence:** `GenerationalLineageExtension.cs` (119 lines) exists in Core with `EstablishLineage`, `PerformSuccession`, `GetLineage`, `GetParent`, `LineageRecord` DTO (parentId, childId, relationshipType, establishedDay, isActive, inheritedTraitIds), and `CaptureState/RestoreState`. `GenerationalSuccessionEngine.cs` (150 lines) exists in `Ashfall.Core.Legacy` with `DwellerGenerationRecord` (generationIndex, inGameAgeYears, isRetired, isDeceased, mentorDwellerId, inheritedTraitIds), chapter time progression, and retirement at age 65. The engine IS wired: `ExpansionHostSession.cs:33` has a `Generational` property, `ExpansionHostSession.cs:61` constructs it, `CenturySeedPanel.cs` binds UI. The extension is NOT wired: grep for `GenerationalLineageExtension` in `src/` returns ZERO matches.

**What is missing:** (1) Extension not wired to host — never instantiated, never ticked, never saved. (2) No sibling detection (shared-parent computation). (3) No spouse tracking. (4) No family units (grouped family members with shared name). (5) No family naming mechanics. (6) No family events beyond lineage establishment (birth/death/marriage/divorce/adoption/reunion/schism/milestone). (7) No multi-generational ancestor/descendant traces. (8) No lineage depth calculation. (9) No family history recording. (10) No genealogy UI (family tree visualization). (11) No kinship affinity bonuses. (12) No integration with `CohortSystem` child birth or `SurvivorRelationsSystem` affinity.

**Why existing plans don't solve it:** Plan 150 (Romance & Family) covers romance/family formation but not genealogical record-keeping. Plan 154 (Education) mentions "parent-child bonus" for teaching but not lineage tracking. Plan 183 (Child Development) covers children growing up but not family tree recording. Plan 206 (Death & Inheritance) covers inheritance but not family lineage. No plan addresses wiring `GenerationalLineageExtension` or extending it into a full genealogy system.

**Player value:** Creates generational depth (families persist across time), adds emotional weight (knowing who is related to whom), generates emergent stories (family dramas, inherited traits, multi-generational sagas), and makes the Century Seed expansion's generational mechanics visible and interactive rather than hidden in an unwired extension.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/GenerationalLineageExtension.cs` — existing lineage extension (119 lines, unwired)
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` — existing succession engine (150 lines, wired to ExpansionHostSession)
- `src/Host/ExpansionHostSession.cs` — host wiring for engine (line 33 property, line 61 construction)
- `src/UI/CenturySeedPanel.cs` — existing UI for engine (presentation only)
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — affinity/bonds (191 lines)
- `Assets/Ashfall.Core/CohortSystem.cs` — children/maturation (174 lines)
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` — death tracking (438 lines)
- EXTEND: `Assets/Ashfall.Core/GenerationalLineageExtension.cs` — add sibling/spouse/family unit/event tracking
- NEW: `Assets/Ashfall.Core/Survivors/GenealogyBridge.cs` — bridge between extension and survivor systems
- NEW: `Assets/StreamingAssets/Data/family_name_templates.json`

## Main Task 1 — Wire Extension to Host + Extend Core Data Model

1. **Wire `GenerationalLineageExtension` into host:**
   - Add `GenerationalLineageExtension` property to `ExpansionHostSession.cs` (constructed with existing `Generational` engine)
   - Add `TickLineage(day)` call in expansion tick loop
   - Add `SaveLineage/LoadLineage` to expansion save flow (extension already has `CaptureState/RestoreState`)
   - Verify: existing `GenerationalSuccessionSaveState` captures engine; `LineageState` captures extension — both must persist
2. **Extend `LineageRecord` DTO** (add fields, backward-compatible):
   - Add `siblingIds` (computed on query, not stored — derived from shared parents)
   - Add `spouseId` (string, nullable)
   - Add `familyName` (string, inherited/assigned)
   - Add `generationDepth` (int, computed from ancestor chain length)
3. **Add `FamilyUnit` DTO** to extension file:
   - `unitId`, `familyName`, `foundingDay`, `memberIds` (list of survivor_ids), `patriarchId` (nullable), `matriarchId` (nullable), `status` (active/dissolved/extinct), `currentGeneration` (int)
4. **Add `FamilyEvent` DTO** to extension file:
   - `eventId`, `eventType` (birth/death/marriage/divorce/adoption/reunion/schism/milestone/succession), `day`, `participantIds` (list), `description`, `significance` (minor/moderate/major)
5. **Add `GenealogyQuery` helpers** to extension:
   - `GetSiblings(survivorId)` — find all records sharing a parent
   - `GetAncestors(survivorId)` — walk parent chain upward
   - `GetDescendants(survivorId)` — walk child chain downward
   - `GetLineageDepth(survivorId)` — count generations from root ancestor
   - `GetFamilyUnit(survivorId)` — find unit containing survivor
   - `GetSpouse(survivorId)` — return spouseId from record
6. **Add family event recording** to extension:
   - `RecordFamilyEvent(FamilyEvent)` — log event
   - `GetFamilyEvents(survivorId)` — events involving survivor
   - `GetAllFamilyEvents()` — full event history
7. **Add family unit management** to extension:
   - `FormFamilyUnit(familyName, foundingDay, memberIds)` — create unit
   - `AddToUnit(unitId, survivorId)` — add member
   - `RemoveFromUnit(unitId, survivorId)` — remove member
   - `DissolveUnit(unitId)` — mark dissolved
   - `GetAllUnits()` — list all family units
8. **Add family naming** to extension:
   - `AssignFamilyName(survivorId, name)` — set name
   - `InheritFamilyName(childId, parentId)` — pass parent's name to child
   - `GenerateFamilyName(ISeededRng)` — create name from templates
9. **Extend `LineageState`** (backward-compatible):
   - Add `familyUnits` (list of FamilyUnit)
   - Add `familyEvents` (list of FamilyEvent)
   - Old saves: new fields default to empty lists
10. **Extend events** on extension:
    - `OnFamilyUnitCreated`, `OnFamilyEvent`, `OnFamilyNameAssigned`
11. **Update `CaptureState/RestoreState`** — new fields serialize/deserialize; old saves get empty defaults
12. **Add deterministic seeding** — family naming uses `ISeededRng`

## Main Task 2 — Bridge to Survivor Systems + Family Tree UI

1. **Create `GenealogyBridge.cs`** in `Assets/Ashfall.Core/Survivors/`:
   - Thin adapter connecting `GenerationalLineageExtension` to survivor lifecycle
   - `OnChildBorn(parentId, childId)` — calls `EstablishLineage`, assigns/inherits family name, creates/updates family unit, records birth event
   - `OnSurvivorDied(survivorId)` — records death event, updates family unit status, checks if unit extinct
   - `OnMarriage(survivorIdA, survivorIdB)` — sets spouseId on both records, creates/merges family unit, records marriage event
   - `OnDivorce(survivorIdA, survivorIdB)` — clears spouseId, splits family unit, records divorce event
   - `OnAdoption(adoptiveParentId, childId)` — calls `EstablishLineage` with "adopted" type, records adoption event
2. **Wire bridge to existing systems:**
   - Connect to `CohortSystem` child birth events → `OnChildBorn`
   - Connect to `SurvivorFateSystem` death events → `OnSurvivorDied`
   - Connect to `SurvivorRelationsSystem` bond events → `OnMarriage`/`OnDivorce`
   - Connect to `GenerationalSuccessionEngine` retirement → succession event
3. **Add kinship affinity bonuses** to `SurvivorRelationsSystem`:
   - Query `GenerationalLineageExtension` for sibling/parent/child/spouse relationships
   - Apply affinity bonus modifiers: parent-child +20, sibling +15, spouse +25, grandparent +10
   - Apply affinity penalty: divorced ex-spouse -10, schism family member -15
4. **Create family tree UI** in `src/UI/GenealogyPanel.cs`:
   - Visual tree rendering: nodes for survivors, lines for relationships
   - Color coding: alive=green, deceased=gray, infant=light green
   - Relationship lines: parent-child=solid, marriage=dashed, adoption=dotted
   - Click node → survivor detail popup with lineage info
   - Zoom/scroll for large trees
   - Filter: by family unit, by generation, by alive/deceased
   - Generation display: current generation count, deepest lineage
5. **Create family detail panel:**
   - Family unit info: name, members, status, founding day
   - Family event log: chronological history
   - Lineage traces: ancestor chain, descendant tree
   - Lineage depth: number of generations
6. **Create `CenturySeedPanel` extension:**
   - Add lineage tab to existing panel (panel already binds to engine)
   - Show extension data: family units, events, tree
   - Keep existing engine display (chapter timeline, generation records)
7. **Create family name templates** in `Assets/StreamingAssets/Data/family_name_templates.json`:
   - 30+ family name templates (fictional, post-nuclear themed)
   - Schema: `{ "schema_version": 1, "family_names": [...] }`
   - Names follow ASHFALL naming rules (no real-world countries/people)
8. **Add genealogy quest hooks:**
   - "The Patriarch/Matriarch" — establish family with 10+ members across 3 generations
   - "The Genealogist" — document 5 complete family trees
   - "The Matchmaker" — arrange 5 marriages between survivors
   - "The Elder" — reach 3rd generation (great-grandchildren)
   - "The Historian" — record 20 family events
   - "The Dynasty" — maintain active family unit for 200 days
   - "The Reunion" — reunite 3 separated family members after schism
   - "The Legacy" — successfully perform 3 generational successions

## Main Task 3 — Integration / Consequences / Validation

1. **Wire into `GameBootstrap`/`Main.cs`:**
   - Extension already constructed in `ExpansionHostSession` — verify tick/save wiring
   - Add `GenealogyBridge` construction in survivor setup
   - Connect bridge to CohortSystem, SurvivorFateSystem, SurvivorRelationsSystem events
2. **Integrate with `CohortSystem`:**
   - Child birth triggers `OnChildBorn` → lineage established, family name inherited, birth event recorded
   - Maturation triggers family unit update
3. **Integrate with `SurvivorFateSystem`:**
   - Death triggers `OnSurvivorDied` → death event recorded, family unit updated
   - Last member death → unit marked extinct
4. **Integrate with `SurvivorRelationsSystem`:**
   - Marriage/divorce triggers family unit formation/dissolution
   - Kinship affinity bonuses applied to relationship calculations
5. **Integrate with `TraitSystem`:**
   - `inheritedTraitIds` already tracked in `LineageRecord` and `DwellerGenerationRecord`
   - Wire trait inheritance display in family tree UI
6. **Implement old-save compatibility:**
   - Existing saves: extension `LineageState` deserializes with empty `familyUnits`/`familyEvents`
   - Engine `GenerationalSuccessionSaveState` unchanged
   - No migration needed — new fields default to empty
7. **Add deterministic seeding:**
   - Family naming uses `ISeededRng` (not System.Random)
   - Family event ordering is deterministic
8. **Create exploit prevention:**
   - Genealogy is birth/relationship-based, can't be gamed
   - Family names are inherited or generated, not player-chosen
   - Kinship bonuses are automatic, not player-triggered
9. **Add tests:**
   - Extension wire: verify construction, tick, save/load in ExpansionHostSession
   - Bridge: verify OnChildBorn, OnSurvivorDied, OnMarriage, OnDivorce, OnAdoption
   - Sibling detection: verify shared-parent computation
   - Family units: verify formation, membership, dissolution, extinction
   - Family naming: verify inheritance, generation, template-based creation
   - Family events: verify recording, querying, chronological ordering
   - Multi-generational: verify ancestor/descendant traces, lineage depth
   - Kinship bonuses: verify affinity modifiers
   - Save round-trip: verify extension state captures/restores with new fields
   - Old-save compat: verify empty defaults for new fields
   - Determinism: verify same seed → same family names → same events
10. **Verify headless behavior:** genealogy processes correctly without UI
11. **Add data-integrity-selftest:** family names validate against template catalog
12. **Create `--genealogy-selftest` verb** for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --genealogy-selftest
```

## Risk

**LOW-MEDIUM** — The extension already exists and is tested (80 lines of tests), but it was never wired to the host. Risk 1: wiring may reveal missing integration points or conflicts with `ExpansionHostSession` save flow. Mitigation: wire incrementally, test each step. Risk 2: extending `LineageRecord` with new fields must remain backward-compatible. Mitigation: new fields default to empty/null, old saves deserialize cleanly. Risk 3: family tree UI may be complex for large shelters. Mitigation: filter/scroll, generation-limited display.

## Definition of Done

- `GenerationalLineageExtension` wired into `ExpansionHostSession` (constructed, ticked, saved)
- `LineageRecord` extended with spouseId, familyName, generationDepth
- `FamilyUnit` DTO added (name, members, status, generation)
- `FamilyEvent` DTO added (8 event types)
- `GenealogyBridge.cs` connects extension to CohortSystem, SurvivorFateSystem, SurvivorRelationsSystem
- Sibling detection (shared-parent computation)
- Multi-generational ancestor/descendant traces
- Lineage depth calculation
- Family naming (inheritance, generation, templates)
- Family event recording and querying
- Kinship affinity bonuses in SurvivorRelationsSystem
- Save/load round-trip tested (extension + new fields)
- Old saves load with empty family units/events
- Family name templates in data authority (30+ names)
- UI genealogy panel with family tree visualization
- Family detail panel with event log, lineage traces
- CenturySeedPanel extended with lineage tab
- Cross-system integration (cohort, fate, relations, traits, engine)
- Deterministic family naming verified
- `--genealogy-selftest` verb for CI

## Follow-On Opportunities

- Genealogy specialization (survivors become expert genealogists/historians)
- Genealogy legacy (famous families remembered across campaigns)
- Genealogy quests (specific family goals, dynasty challenges)
- Genealogy events (mass family reunion, dynasty celebration, inheritance dispute)
- Inter-settlement genealogy (trade family records, arrange marriages between settlements)
- Genetic trait inheritance modeling (traits passed through lineage with mutation chance)
