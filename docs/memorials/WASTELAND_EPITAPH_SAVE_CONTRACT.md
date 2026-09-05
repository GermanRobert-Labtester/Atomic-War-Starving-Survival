# Wasteland Grave Epitaphs — Save & Persistence Contract

**Core Persistence Authority:** `Assets/Ashfall.Core/Memorial/MemorialSave.cs`
**Host Store Façade:** `src/Host/MemorialSaveStore.cs` (`user://memorial_save.json`)
**Campaign Single-Envelope Store:** `SaveStoreHub` (`campaign.json`, section `memorial`)

---

## 1. Saved Grave and Memorial State

In ASHFALL, memorialization persists the selected epitaph directly inside the memorial entry:

```csharp
[Serializable]
public sealed class MemorialEntry
{
    public string SurvivorId;
    public string Cause;
    public int Day;
    public int SurvivedDays;
    public bool FinalWishResolved;
    public string Epitaph;              // <-- Authoritative persisted string
    public string HeirloomItemId;
    public string HeirloomRecipientId;
    public float MoraleDelta;
    public DeathQuality DeathQuality;
    public MemorialOutcome Outcome;
}
```

---

## 2. Backward & Forward Compatibility

1. **Direct String Persistence:** Because `Epitaph` is stored as an explicit string on `MemorialEntry`, existing saves retain their exact historical text upon reload. Expanding `wasteland_grave_epitaphs.json` does not alter, recompute, or mutate already-created memorials.
2. **Checksum Integrity:** `MemorialSave` contains a `SaveChecksum` computed across all entries. Since `MemorialEntry` schema is completely unchanged, existing saves load with valid checksums.
3. **No Migration Needed:** Expanding the data catalog is 100% additive and requires zero save-file migration.
