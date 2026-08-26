# 5-Bug Core Repair Plan

**Source audit:** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Scope:** Surgical Core-only patches. No host wiring, no JSON data, no schema migration.
**Strategy:** Test-first per bug → minimal fix → adjacent tests → close.

## Bug Selection Rationale

Selected for: bounded blast radius, clear repro, no host coupling, no design ambiguity.

| Batch | Bug | Severity | File |
|---|---|---|---|
| 1 | BUG-06 Contractor expired-then-paid race | MEDIUM | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| 2 | BUG-09 MentalHealth caregiver eligibility gap | MEDIUM | `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` |
| 3 | BUG-10 Library XP pair-list IndexOutOfRange | MEDIUM | `Assets/Ashfall.Core/LibraryStudySystem.cs` |
| 4 | BUG-07 Schedule fatigueRecovery ignored in day | MEDIUM | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| 5 | BUG-08 Sump equipmentDisabled latch | MEDIUM | `Assets/Ashfall.Core/SumpFloodingSystem.cs` |

## Invariants (apply to all phases)

1. Core remains engine-agnostic — no `Godot.*`, no `UnityEngine.*`.
2. RNG is `ISeededRng` only — no `System.Random`, no `Guid.NewGuid`.
3. `CaptureState/RestoreState` continue to round-trip without aliasing live state.
4. Adjacent xUnit tests stay green.
5. No event subscription adds new lifecycle cost.
6. Determinism preserved (no new RNG draws, no new dictionary iteration).

## Test-first policy

For each bug, write one failing xUnit test BEFORE the production edit. Test must:
- exercise the actual production class (not a copy)
- fail for the documented defect reason
- pass after the fix
- not depend on incidental state

## Verification ladder

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj       → 0 errors
2. dotnet test  Ashfall.Core.Tests/<bug_regression> --filter ...   → pass
3. dotnet test  Ashfall.Core.Tests (full Battery3 filter)          → 102+ pass
```

No godot host re-run, no Unity commands.

## Execution order

Sequential, one bug at a time, with pre-integration checkpoint before each edit:

```
CHECKPOINT → TEST (red) → FIX (green) → REGRESSION (adjacent tests) → CLOSE
```

## Rollback strategy

Each fix is a single-file change. If a fix causes a regression, `git checkout <file>` restores. Tests are committed alongside the fix.

## Definition of Done

- 5 bugs closed
- 5 regression tests added (or updated existing test that now actually covers the case)
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → 107+/107+ pass (102 prior + ≥5 new)
- 0 build warnings introduced by the fixes
- No Core file references engine namespaces
