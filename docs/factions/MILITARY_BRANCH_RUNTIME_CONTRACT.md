# Military Faction Branch Runtime Contract

> **Catalog Authority:** `Assets/StreamingAssets/Data/military_faction_branch.json`
> **Linux Build Mirror:** `builds/linux/Assets/StreamingAssets/Data/military_faction_branch.json`
> **Core Runtime Service:** `Assets/Ashfall.Core/Factions/MilitaryBranchSystem.cs`
> **Catalog Loader & DTOs:** `Assets/Ashfall.Core/Factions/MilitaryBranchCatalog.cs`
> **Static ID Authority:** `Assets/Ashfall.Core/Factions/MilitaryBranchIds.cs`
> **Faction Coordinator:** `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`

---

## 1. Catalog & DTO Architecture

The Military branch system provides point-of-no-return (PoNR) character trajectories and morality-conditioned ending resolution for military-affiliated survivors during the Year of Ash campaign.

### 1.1 JSON Schema & Data Transfer Objects
```csharp
[Serializable]
public sealed class MilitaryBranchEndingEntry
{
    public string ending_id { get; set; } = string.Empty;
    public string band_min { get; set; } = string.Empty;
    public string band_max { get; set; } = string.Empty;
    public string display_name { get; set; } = string.Empty;
}

[Serializable]
public sealed class MilitaryBranchEntry
{
    public string id { get; set; } = string.Empty;
    public string display_name { get; set; } = string.Empty;
    public string ponr_flag { get; set; } = string.Empty;
    public string ponr_trigger { get; set; } = string.Empty;
    public string entry_band_min { get; set; } = string.Empty;
    public string entry_band_max { get; set; } = string.Empty;
    public List<MilitaryBranchEndingEntry> endings { get; set; } = new();
}

[Serializable]
public sealed class MilitaryBranchDataFile
{
    public int schema_version { get; set; } = 1;
    public string faction_id { get; set; } = string.Empty;
    public List<MilitaryBranchEntry> branches { get; set; } = new();
}
```

---

## 2. ID Authority & Completion Mode Classification

A critical preflight audit evaluated whether `MilitaryBranchIds.cs` acts as purely optional constants or as an authoritative whitelist:

1. **Point-of-No-Return Lock Mechanism:**
   In `MilitaryBranchSystem.cs` (line 130):
   ```csharp
   string flagId = MilitaryBranchIds.PonrFlagFor(_state.branch.branchId);
   ```
   `MilitaryBranchIds.PonrFlagFor` is an exhaustive C# switch expression:
   ```csharp
   public static string PonrFlagFor(string branchId) => branchId switch
   {
       BranchLoyalSoldier => FlagPonrLoyalSoldier,
       ...
       _ => throw new ArgumentException($"Unknown Military branch id '{branchId}'.", nameof(branchId))
   };
   ```
   Any branch ID absent from `MilitaryBranchIds.cs` causes an immediate `ArgumentException` when `LockPointOfNoReturn()` is executed.

2. **Test Assertions:**
   `MilitaryBranchCatalogTests` and `MilitaryBranchSystemTests` assert:
   ```csharp
   Assert.Equal(MilitaryBranchIds.BranchCount, catalog.Count);
   foreach (var branchId in MilitaryBranchIds.AllBranches)
   ```

3. **Classification Decision:**
   - **Case B — Authoritative Code-Pinned Whitelist [CONFIRMED]**
   - **Completion Mode:** **DATA + MINOR ID REGISTRATION**
   - In accordance with Section 2 and Section 4 of the implementation directive, `MilitaryBranchIds.cs` was updated with constants, `AllBranches` registration, and `PonrFlagFor` switch arms for branches 9 through 15.
   - Zero new gameplay logic or engine dependencies were introduced into Core; the edit was strictly typing and identity registration, exactly matching the precedent established in Plan 121 (`IndependentBranchIds.cs`).

---

## 3. Core Runtime Query & State Machine Rules

### 3.1 Branch Commitment (`CommitBranch`)
- **Signature:** `string CommitBranch(string branchId, MoralChoiceSystem moralChoice)`
- **Gate Check:** Verifies that the player's current moral band satisfies `band >= entry_band_min && band <= entry_band_max`. If out of range, throws `InvalidOperationException`.
- **Idempotency:** Once committed, subsequent calls return the existing committed branch ID without mutation (`first commit wins`).

### 3.2 Point-of-No-Return Lock (`LockPointOfNoReturn`)
- **Irreversibility:** Once locked, `_state.branch.ponrLocked = true` and `_state.branch.ponrLockedDay = currentDay`.
- **Durable Flag Emission:** Resolves the PoNR flag via `MilitaryBranchIds.PonrFlagFor(branchId)`. Sets the flag in `IFlagLedger` and appends it to `_state.setFlags`.
- **Idempotency:** Calling `LockPointOfNoReturn()` multiple times is a safe no-op.

### 3.3 Ending Resolution (`ResolveEnding`)
- **Prerequisite:** Cannot resolve an ending before PoNR is locked (`InvalidOperationException`).
- **Evaluation Order:** Iterates through `def.endings` in catalog declaration order. The first ending whose range satisfies `band >= min && band <= max` is selected.
- **Persistence:** The resolved ending ID is stored in `_state.branch.resolvedEndingId`. Future calls return this stored ID immediately (`PoNR locks the path; drift does not re-roll ending`).
- **Fallback Defense:** If an authored range gap occurs, falls back to the middle ending (`def.endings[count / 2]`). With Plan 122's exhaustive 7-band coverage, all branches match exactly one row with 0 gaps.

### 3.4 Faction Alignment vs Morality Score
- `MoralChoiceSystem.CurrentScore` (-200..+200) represents the player's personal ethical orientation.
- `MilitaryBranchSystemState.militaryAlignment` (-200..+200, initialized to -80) represents the Military faction's internal doctrinal stance, which the player can sway through faction decisions (`ShiftFactionAlignment(delta)`).

---

## 4. Save & Persistence Contract

- `MilitaryBranchSystemState` holds all mutable state (`timeline`, `branch`, `militaryAlignment`, `setFlags`).
- `CaptureState()` and `RestoreState()` perform deep cloning.
- On restore, `RestoreState` replays all entries from `setFlags` into `IFlagLedger`, ensuring runtime flags match save state without requiring `IFlagLedger` to be serialized directly.
- Fully backward compatible with existing saves.
