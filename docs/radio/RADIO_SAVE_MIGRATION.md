# Radio Save State & Migration Hardening

> **Document Status:** Authoritative Persistence Contract
> **Authority:** Plan 24 (Task 24AY)
> **Target Schema:** `RadioSaveState` Version 2 (Seamless V1 -> V2 Migration)

---

## 1. Persistent State Scope

The radio save state captures only true mutable player and world progression data, never static catalog definitions or derived runtime caches:

```csharp
[Serializable]
public sealed class RadioSaveState
{
    // Schema versioning
    public int schemaVersion = 2;
    public int day = 1;
    public float currentFrequency = 88.5f;

    // Ordered intercept history (last 32 records)
    public List<RadioInterceptEntry> history = new List<RadioInterceptEntry>();

    // Dedup keys to prevent voice-over replay on reload
    public List<string> playedBroadcastKeys = new List<string>();

    // Discovered station presets (memory)
    public List<string> discoveredStationIds = new List<string>();
    public List<float> customPresets = new List<float>();

    // Active & resolved distress signals
    public List<DistressSignalSaveEntry> distressSignals = new List<DistressSignalSaveEntry>();

    // Intercepted signal log (analytic codex records)
    public List<SignalLogEntry> signalLog = new List<SignalLogEntry>();

    // Recorded cassettes index
    public List<RecordedCassetteEntry> recordedCassettes = new List<RecordedCassetteEntry>();

    // Station override states (e.g. silenced, jammed)
    public List<StationStateOverrideEntry> stationOverrides = new List<StationStateOverrideEntry>();
}
```

---

## 2. Backward Compatibility Guarantee

1. **Pre-Plan-24 (V1) Save Compatibility:** When a legacy V1 save is restored (which contains only `day`, `currentFrequency`, `history`, and `playedBroadcastKeys`), all new fields initialize to clean, valid defaults (`discoveredStationIds` populates from starting faction, `distressSignals` defaults to inactive/untriggered).
2. **Deterministic Checksums:** All collections are sorted by ordinal string IDs before calculating `SaveChecksum` hashes, ensuring cross-host determinism and tamper detection.
3. **No Reload Replay:** Reloading a save during an active distress call preserves the exact days remaining without resetting the timer; resolved calls are permanently marked resolved.
