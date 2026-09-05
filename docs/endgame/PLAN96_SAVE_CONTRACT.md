# Epilogue Chronicle Save State & Migration Contract

**Document ID:** `docs/endgame/PLAN96_SAVE_CONTRACT.md`
**System ID:** `EndgameSystem.SystemId` ("endgame")
**Save Section:** `endgame` (Section 97 in `SaveSectionRegistry`)

---

## 1. Save Envelope Boundary

In strict compliance with Invariant 1 and Invariant 3:
- The `epilogue_chronicle.json` catalog is **read-only static authority**.
- Slide definitions, orders, and placeholder asset tokens are **never serialized into save envelopes**.
- The save store (`EndgameSaveStore.cs` / `EndgameSaveState`) persists only transient campaign state:

```csharp
[Serializable]
public sealed class EndgameSaveState
{
    public int schema_version { get; set; } = 1;
    public string systemId { get; set; } = EndgameSystem.SystemId;
    public EndgamePhase phase { get; set; } = EndgamePhase.Active;
    public int triggeredDay { get; set; } = -1;
    public int sealedDay { get; set; } = -1;
    public string activeEndingId { get; set; } = string.Empty;
    public CampaignEpilogueReport? epilogueReport { get; set; }
}
```

---

## 2. Backward & Forward Compatibility

1. **Pre-Plan 96 Saves**: Saves made before Plan 96 contain only `activeEndingId` and basic report fields. When loaded into a game with the 20-slide chronicle, the builder generates the 20-slide sequence without any schema migration error.
2. **Deterministic Checksums**: Because `epilogue_chronicle.json` is outside the campaign envelope, expanding the catalog from 5 to 20 slides does not alter `SaveChecksum` for existing campaign saves.
3. **Sealed Campaigns**: Once a campaign is sealed (`EndgamePhase.Sealed`), re-opening the chronicle in `ChroniclePanel` reads the live 20-slide presentation sequence while preserving the frozen report data.
