# BUG-01/03/11 Repair Plan: NeedsSystem/ResearchSystem/StartingLevelSystem State Authority

## 1. Bug

**BUG-01 (CRITICAL):** Three local `NeedsSystem` instances in `Main.ExpandedShelterSystems.cs` have empty `_survivors` lists. `Modify()` calls silently fail because `Get(survivorId)` returns null.

**BUG-03 (MEDIUM):** `AutopsyHostSession` creates local `ResearchSystem` and `StartingLevelSystem` instances that are not saved/restored.

**BUG-11 (MEDIUM):** 47 `new System()` calls in `Main.ExpandedShelterSystems.cs` create excessive disconnected instances instead of sharing authoritative ones.

## 2. Reproduction

```bash
# BUG-01: Local NeedsSystem instances have no survivors
grep -n "new NeedsSystem()" src/Main.ExpandedShelterSystems.cs
# Output: lines 245, 351, 429

# BUG-03: AutopsyHostSession creates local Research/StartingLevel
grep -n "new ResearchSystem\|new StartingLevelSystem" src/Host/AutopsyHostSession.cs
# Output: lines 30, 32

# BUG-11: Count of new System() calls
grep -c "new .*System(" src/Main.ExpandedShelterSystems.cs
# Output: 47
```

## 3. Root Cause

`Main.ExpandedShelterSystems.cs` creates local Core system instances in each `Setup*()` method instead of using shared authoritative instances. This was likely inherited from Unity architecture and reinforced by the P1-1 HostSession extraction.

## 4. Blast Radius

| Surface | How Affected | Risk |
|---------|--------------|------|
| ShelterThermalSystem | Uses local `stNeeds` - warmth mods lost | HIGH |
| KitchenNutritionSystem | Uses local `knNeeds` - health mods lost | HIGH |
| MentalHealthCrisisSystem | Uses local `mhNeeds` - morale mods lost | HIGH |
| AutopsyHostSession | Local `auRes`/`auStarting` not saved | MEDIUM |
| LibraryStudyHostSession | Local `lsResearch` not shared | MEDIUM |
| SurvivorsHostSession | Authoritative `Needs` never receives mods | HIGH |

## 5. Invariants

1. One authoritative `NeedsSystem` instance (`_survivors.Needs`)
2. One authoritative `ResearchSystem` instance
3. One authoritative `StartingLevelSystem` instance (`_startingLevel.System`)
4. All features use shared instances
5. Save/load captures complete state
6. Core remains engine-agnostic

## 6. Repair Options

### Option A: Constructor injection of shared instances (SELECTED)

**Approach:** Pass shared instances from Main to each HostSession/System constructor.

**Files:**
- `src/Main.ExpandedShelterSystems.cs`
- `src/Host/ShelterThermalHostSession.cs`
- `src/Host/KitchenNutritionHostSession.cs`
- `src/Host/MentalHealthCrisisHostSession.cs`
- `src/Host/AutopsyHostSession.cs`
- `src/Host/LibraryStudyHostSession.cs`

**Advantages:** Clean, single source of truth, minimal runtime cost, testable

**Risks:** Changes constructor signatures, requires careful wiring order

**Save impact:** None - state capture unchanged

**Determinism impact:** None - same state, just shared

### Option B: Shared registry

**Rejected:** Hidden coupling, harder to test

### Option C: Keep locals, add save/restore

**Rejected:** Still multiple instances, state divergence risk remains

## 7. Selected Repair

Option A: Constructor injection of shared instances.

## 8. Why Other Options Rejected

See section 6.

## 9. File Impact

6 files modified (see section 6)

## 10. Save/Data Implications

None. Save DTOs unchanged. Old saves load correctly.

## 11. Determinism Implications

None. Same state, just shared across features.

## 12. Test Plan

1. Add regression test: `NeedsSystemIntegrationTests.cs` - verify modifications from external systems propagate
2. Verify all existing selftests pass
3. Verify `dotnet test` passes
4. Verify `godot --headless` selftests pass

## 13. Implementation Phases

### Phase 1: Wire NeedsSystem
- Pass `_survivors.Needs` to ShelterThermal, KitchenNutrition, MentalHealthCrisis
- Remove local `new NeedsSystem()` calls

### Phase 2: Wire StartingLevelSystem
- Pass `_startingLevel.System` to AutopsyHostSession
- Remove local `new StartingLevelSystem()` calls

### Phase 3: Wire ResearchSystem
- Create shared `ResearchSystem` in Main
- Pass to AutopsyHostSession and LibraryStudyHostSession
- Remove local `new ResearchSystem()` calls

### Phase 4: Verify
- Run all tests
- Run selftests
- Inspect diff

## 14. Rollback Strategy

Each phase is independent. If Phase 1 causes issues, revert only Phase 1 changes.

## 15. Definition of Done

- [ ] No `new NeedsSystem()` in Main.ExpandedShelterSystems.cs
- [ ] No `new ResearchSystem()` in Main.ExpandedShelterSystems.cs
- [ ] No `new StartingLevelSystem()` in Main.ExpandedShelterSystems.cs
- [ ] All features use shared instances
- [ ] All tests pass
- [ ] All selftests pass
