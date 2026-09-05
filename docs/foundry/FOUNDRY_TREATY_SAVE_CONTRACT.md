# Foundry Treaty Consequence Save Contract & Invariant Discipline

**Runtime Source:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
**State DTO:** `Ashfall.Core.Foundry.SilentFoundryConsequenceState`
**Checksum Provider:** `Assets/Ashfall.Core/SaveChecksum.cs`

---

## 1. Save State DTO Shape

```csharp
[Serializable]
public sealed class SilentFoundryConsequenceState
{
    public float guildStanding = 0f;
    public List<FoundryConsequenceRecord> applied = new List<FoundryConsequenceRecord>();

    public bool IsApplied(string treatyId, int cycleMarker) { ... }
}

[Serializable]
public sealed class FoundryConsequenceRecord
{
    public string treatyId = string.Empty;
    public FoundryTreatyOutcome outcome = FoundryTreatyOutcome.NotRatified;
    public int appliedDay = 0;
    public int cycleMarker = 0;
    public float standingDelta = 0f;
    public string reason = string.Empty;
}
```

---

## 2. Six Invariants Verification

1. **Invariant 1 (Zero Engine Coupling):**
   - `SilentFoundryConsequencePolicy.cs` contains zero references to `UnityEngine`, `UnityEditor`, `Godot`, or `GodotSharp`.
   - Compiles strictly against `netstandard2.1` in `Ashfall.Core.csproj`.

2. **Invariant 2 (Ports and Adapters):**
   - File access via `IFileIO`.
   - JSON serialization via `IJsonSerializer` (`SystemTextJsonSerializer`).
   - Logging via `ILog` / `GodotLog`.

3. **Invariant 3 (Cross-Host Save Compatibility & Determinism):**
   - State uses plain `[Serializable]` C# POCOs.
   - `SaveChecksum.Compute(state)` guarantees culture-invariant hash stability across round-trips.
   - Pinned by `SilentFoundryConsequenceTests.SaveRoundTrip_ConsequenceLedgerPreservesIntegrityAndChecksum`.

4. **Invariant 4 (Determinism):**
   - Assessment days are deterministic arithmetic: `ratified_day + n * 30`.
   - Consequences use deterministic lookups via `(treaty_id, outcome)`. Zero unseeded RNG.

5. **Invariant 5 (No Gameplay Logic in Hosts):**
   - Presentation logic lives in `src/UI/` and `src/Foundry/SilentFoundryHostSession.cs`.
   - Simulation state lives strictly in `SilentFoundrySystem` and `SilentFoundryConsequenceState`.

6. **Invariant 6 (Data Authority is JSON):**
   - Authority is `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`.
   - Validated at boot by `SilentFoundryConsequenceCatalogLoader` and CI gates.
