========================================================================================
DEPRECATED AND FOLDED — ALL AUDIT WORK VERIFIED & COMPLETED
Status: RESOLVED & CLOSED
Date: 2026-08-08
========================================================================================

# H-4 Remediation Plan — SaveSystem refactor to ISaveable

## Goal

Currently `SaveSystem` is a 1915-LOC god class that hard-codes 60+ saveable
systems. Each new system requires editing 4 places:
1. A `private _xSystem` field
2. A `public void SetX(xSystem)` setter
3. A `data.X = _xSystem.CaptureState()` block in `CaptureSnapshot`
4. An `_xSystem?.RestoreState(data.X)` block in `RestoreFromSnapshot`

The C-2 fix caught one instance (ClothingDurability) but the same bug
class can recur with any future system. The right fix is a generic
`ISaveable` interface that the SaveSystem iterates over.

## Design

```csharp
public interface ISaveable
{
    /// <summary>Stable id used to key the save data (e.g. "weather").</summary>
    string SaveId { get; }

    /// <summary>Snapshot the state as a JSON-serializable object.</summary>
    object CaptureState();

    /// <summary>Apply a previously-captured state. Idempotent.</summary>
    void RestoreState(object state);
}
```

Each system that wants to be saveable implements this interface for itself.
`SaveSystem` keeps a `List<ISaveable> _saveables` and iterates over it on
`Save`/`Load`. The per-system `Set*` setters in GameBootstrap are unchanged
(production code still needs to wire systems into the bootstrap); they
just additionally call `_saveSystem.Register(system)`.

## Why not make `SaveSystem` use a Dictionary<id, ISaveable>?

The `SaveData` class uses positional fields (`public WeatherState Weather;`,
`public InventorySaveState Inventory;`, etc.) because JsonUtility
serializes positional fields, not keyed entries. The refactor must
preserve this — each `ISaveable` registers itself with a key, and the
SaveData class gains a single `Dictionary<string, object>` field that
JsonUtility serializes as a list of (key, json) pairs.

This is a significant breaking change to the save format. We can either:
- Bump the save version (V2 → V3) and add a migration that converts the
  old positional fields to the new dict format.
- Keep the positional fields in SaveData and add the dict as a parallel
  structure. The dict is written but the positional fields are not.
  Future systems use the dict.

The second option is simpler and the audit goal is to PREVENT future bugs,
not to migrate existing data. But it means SaveData has a parallel
representation, which is ugly.

The cleanest path: bump the save version, write a migration that
converts V2 → V3 by reading the positional fields and writing them to
the dict. Existing V2 saves still load; new V3 saves use the dict.

## Implementation Plan

1. **Add `ISaveable` interface** in `Assets/_Game/Core/ISaveable.cs`.

2. **Add `_saveables` list to `SaveSystem`** with `Register(ISaveable)`
   method. Keep the existing `Set*` setters for backward compat but
   deprecate them in docs.

3. **Add `SaveData.Subsystems` field** (a `Dictionary<string, string>` —
   the key is the SaveId, the value is the JSON-serialized state).
   JsonUtility serializes dictionaries as key-value pairs. The migration
   reads each positional field, calls `CaptureState` on the matching
   system, serializes the result, and stores it in the dict.

4. **Add `CaptureSubsystemStates` and `RestoreSubsystemStates`** to
   `SaveSystem`. These iterate over `_saveables` and use the dict.

5. **Bump `CurrentSaveVersion` to 3** and add `MigrateV2toV3(data)`:
   - For each positional field, find the matching `ISaveable` (or
     fall back to the old field), serialize, store in the dict.
   - Clear the positional fields (so V3 saves don't double-write).

6. **For each of the 60+ systems**: declare `ISaveable` on the class
   and add `[Serializable] XxxSave` data classes if not present.

7. **Update GameBootstrap** to call `_saveSystem.Register(system)` after
   each `Set*` call. The `Set*` methods can stay (backward compat) but
   the `Register` call is the new canonical wiring.

8. **Tests** in `Assets/Tests/EditMode/SaveSystemRefactorTests.cs`:
   - `ISaveable_Register_RemembersSystems`
   - `ISaveable_Capture_IteratesAllRegistered`
   - `ISaveable_Restore_IteratesAllRegistered`
   - `ISaveable_MigrationV2toV3_PreservesAllState`
   - `ISaveable_UnknownSystem_DoesNotThrow`
   - `ISaveable_MultipleRegistrations_LastOneWins`

## Files

- **NEW** `Assets/_Game/Core/ISaveable.cs` (~30 LOC)
- **MODIFIED** `Assets/_Game/Core/SaveSystem.cs` (~50 LOC delta)
- **MODIFIED** `Assets/_Game/Survivors/Survivor.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Inventory/Inventory.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Environment/WeatherSystem.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Environment/TemperatureSystem.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Radiation/RadiationSystem.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Medical/MedicalSystem.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Shelter/Shelter.cs` (declare `ISaveable` for the aggregate)
- **MODIFIED** `Assets/_Game/Core/EventRunner.cs` (declare `ISaveable`)
- **MODIFIED** `Assets/_Game/Core/GameBootstrap.cs` (call `Register` after each Set*)
- **NEW** `Assets/Tests/EditMode/SaveSystemRefactorTests.cs` (~250 LOC)

## Risk

- **Save format breaking change.** Existing V2 saves still load (the
  migration converts them to the dict format on read), but a V3 save
  is structurally different from V2. Players who are mid-campaign
  with V2 saves will see a one-time migration that reads the old
  format. After the migration, the save file is V3.
- **Per-system refactor risk.** Each of the 60+ systems must declare
  `ISaveable` and add the methods. Miss one and the round-trip
  test fails (which is the point — C-2 is the safety net).
- **Migration risk.** `MigrateV2toV3` must handle all 60+ fields.
  This is the largest piece of new code; I'll cover it with the
  existing `SaveMigration_Tests` (which exercises V2 round-trip; the
  test extends to V3 with a synthetic V2 input).

## Out of Scope (deferred to a future audit)

- **Removing the per-system `Set*` setters.** They stay for now as a
  backward-compat shim. Future audits can remove them once the
  refactor is stable.
- **Refactoring `SaveData` to use the dict exclusively.** Same reason
  as above — we add the dict in parallel; a future audit removes the
  positional fields.
