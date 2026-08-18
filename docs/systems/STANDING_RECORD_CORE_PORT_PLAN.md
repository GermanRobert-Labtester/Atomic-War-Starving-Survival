# Standing Record Core Port Plan

**Target:** Promote the Standing Record (Expansion 03) from read-only
catalog systems to a tick-engineered Core system with
`CaptureState` / `RestoreState`, a host adapter, and a Tier-3 HYBRID
dashboard sub-card.

**Pattern:** Mirror the Phase-18 Skill Progression port
(`SKILL_PROGRESSION_CORE_PORT_PLAN.md`). Engine-agnostic, no
`UnityEngine.*` / `Godot.*` / `JsonUtility`, pure C# / `IFileIO` /
`IJsonSerializer` / `ISeededRng`.

---

## Why this port now

- `SURFACE_GAP_REPORT.md` (Phase 26 close) flagged `StandingRecordPanel`
  as `MISSING (awaiting Core)`.
- The four sidecars already exist on disk and the three catalog readers
  already live in Core:
  - `Assets/StreamingAssets/Data/standing_record_factions.json` (1 record)
  - `Assets/StreamingAssets/Data/standing_record_layouts.json` (14 layouts)
  - `Assets/StreamingAssets/Data/standing_record_memory.json` (38 strata)
  - `Assets/StreamingAssets/Data/standing_record_quests.json` (10 quests)
  - `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs`
  - `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs`
  - `Assets/Ashfall.Core/StandingRecord/SiteEncounterSystem.cs`
- All three catalog systems support `State` + `CaptureState` /
  `RestoreState` already — but each carries its own envelope. A unified
  engine + state + tick is missing.

---

## Five-phase plan

### Phase 1 — Engine + State (Core)

`Assets/Ashfall.Core/StandingRecord/StandingRecordState.cs` — unified
state. `[Serializable]` for `IJsonSerializer`.

```csharp
public sealed class StandingRecordState
{
    public string systemId = StandingRecordEngine.SystemId;
    public bool expansionUnlocked;
    public int currentDay;
    public bool overlayAccess = true;
    public LocationLayoutState layout;
    public LocationMemoryState memory;
    public SiteEncounterState encounters;
}
```

`Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` —
coordinates the three existing catalog systems.

```csharp
public sealed class StandingRecordEngine
{
    public const string SystemId = "standing_record_system";
    public const string FlagExpUnlocked = "exp_standing_record_unlocked";

    public StandingRecordState State { get; }

    private readonly IFileIO _files;
    private readonly IJsonSerializer _json;
    private readonly ISeededRng _rng;
    private readonly ILog _log;

    public LocationLayoutSystem Layouts { get; }
    public LocationMemorySystem Memory { get; }
    public SiteEncounterSystem Encounters { get; }

    public StandingRecordEngine(
        IFileIO files, IJsonSerializer json,
        ISeededRng rng, ILog log = null,
        StandingRecordState state = null)
    {
        _files = files; _json = json; _rng = rng;
        _log = log ?? NullLog.Instance;
        State = state ?? new StandingRecordState();
        Layouts = new LocationLayoutSystem(_files, _json, _log);
        Memory  = new LocationMemorySystem(_files, _json, _log);
        Encounters = new SiteEncounterSystem(_files, _json, _log);
        Memory.RestoreState(State.memory);
        Encounters.RestoreState(State.encounters);
    }

    public void Load(string dataDir)
    {
        Layouts.Load(dataDir);
        Memory.Load(dataDir);
        Encounters.Load(dataDir);
    }

    public void Unlock(int currentDay)
    {
        if (State.expansionUnlocked) return;
        State.expansionUnlocked = true;
        State.currentDay = currentDay;
        Memory.SetFlag(FlagExpUnlocked);
        _log.Info("[StandingRecord] unlocked @ day " + currentDay);
    }

    public void Tick(int newDay)
    {
        if (!State.expansionUnlocked) return;
        State.currentDay = newDay;
        Encounters.DailyLockstep(newDay);
    }

    public SiteEncounterRecord BeginExpeditionAt(
        string parentLocationId, string roomId, int day)
    {
        var encounter = Encounters.RegisterEncounter(
            parentLocationId, roomId, day, payload: "expedition-visited");
        Layouts.Unlock(parentLocationId, roomId);
        return encounter;
    }

    public StandingRecordState CaptureState() => State;
    public void RestoreState(StandingRecordState saved) { State = saved; ... }
}
```

### Phase 2 — Data sidecar audit (StreamingAssets)

All four sidecars are present and on-disk. The plan is **no new
data**, only to register them with `CatalogIntegrityValidator` if not
already.

| Sidecar | Status |
|---|---|
| `standing_record_factions.json` | present, 1 record |
| `standing_record_layouts.json` | present, 14 layouts |
| `standing_record_memory.json` | present, 38 strata |
| `standing_record_quests.json` | present, 10 quests |

### Phase 3 — Engine ID constants

`StandingRecordEngine.SystemId = "standing_record_system"` — already
matches the existing two sub-system ids. Flag id
`FlagExpUnlocked = "exp_standing_record_unlocked"` — consistent with
`LocationMemorySystem.FlagExpUnlocked`.

### Phase 4 — Host adapter (Godot-only)

`src/Host/StandingRecordHostSession.cs` — owns `StandingRecordEngine`,
wires `SurvivorsHostSession.AdvanceDay`, exposes
`CaptureSave()` / `RestoreSave()` for the Godot save
codec.

```csharp
public sealed class StandingRecordHostSession
{
    public StandingRecordEngine Engine { get; }

    public static StandingRecordHostSession Create(...)
    {
        return new StandingRecordHostSession(...);
    }

    public void AdvanceDay(int day) => Engine.Tick(day);
    public void Unlock() => Engine.Unlock(day: 0);

    public StandingRecordSave CaptureSave()
        => new StandingRecordSave { state = Engine.CaptureState() };
    public void RestoreSave(StandingRecordSave save)
        => Engine.RestoreState(save.state);
}
```

`src/Host/StandingRecordSaveStore.cs` — wraps the host save codec
keyed by `StandingRecordEngine.SystemId`.

### Phase 5 — UI dashboard (Tier-3 HYBRID)

`src/UI/StandingRecordAtlasPanel.cs` — Tier-3 HYBRID sub-card sibling
of the Phase 9 modal `StandingRecordPanel.cs`. 6-card status rail +
3 DataGrid tiles (Locations / Memory strata / Site Encounters) +
right-side detail inspector. `Bind(StandingRecordHostSession)`.

Reuses 5 primitives:
- `AshfallDashboardShell`
- `AshfallSidebar` (4 location scopes)
- `AshfallStatusRail` (6 cards)
- `AshfallDataGrid` (3 tiles)
- `AshfallUiHelpers` (MakeSectionHeader / MakeSeparator / MakeDataRow / etc.)

Snapshot target `standing_record_default`.

### Tests

`Ashfall.Core.Tests/StandingRecordEngineTests.cs` — 8 tests:

1. Load → 14 layouts + 38 strata + 10 quests
2. Unlock sets flag + `expansionUnlocked=true`
3. Tick increments day + locks overlay access for raid tracking
4. BeginExpeditionAt registers a SiteEncounter + unlocks a room
5. CaptureState round-trips through `JsonSerializer`
6. RestoreState restores day + flag + overlay access + history
7. Encounter resolution ties `mutation` to `Memory.SetFlag`
8. Lockstep determinism: same seed → same day progression

---

## Files

| Path | Phase | New |
|---|---|---|
| `Assets/Ashfall.Core/StandingRecord/StandingRecordState.cs` | 1 | NEW (~50 lines) |
| `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs` | 1 | NEW (~150 lines) |
| `Ashfall.Core.Tests/StandingRecordEngineTests.cs` | 5 | NEW (~200 lines) |
| `src/Host/StandingRecordHostSession.cs` | 4 | NEW (~120 lines) |
| `src/Host/StandingRecordSaveStore.cs` | 4 | NEW (~80 lines) |
| `src/UI/StandingRecordAtlasPanel.cs` | 5 | NEW (~470 lines) |

Six files total — same scope as the Phase 18 Skill Progression port.

---

## Verification checklist (matches `AGENTS.md` §5)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    # Must compile
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All tests pass; +8 from this phase
3. dotnet build Ashfall.csproj                                   # Godot host: 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest       # Catalog integrity: 0 errors
5. godot --path . -- --bridge-selftest                           # Shim honesty
6. godot --path . -- --ui-snapshot-uitest                        # 28/28 (was 27/27)
```

---

## Closing criteria

The phase closes when:

- 28/28 snapshot targets render with **distinct MD5 fingerprints** (on-disk
  byte inspection — the 4062B-duplicate trap must not return).
- All 28 catalog sidecars + the new engine + the unified state envelope
  carry over a save round-trip with checksum integrity.
- `SURFACE_GAP_REPORT.md` and `SNAPSHOT_COVERAGE.md` show `StandingRecord` as
  `COVERED` (Phase 27).
- Documentation updated; `VISUAL_QA_REPORT.md` may pick up the §24 entry.

