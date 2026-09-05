# Verdict Radio Save Contract

> **Save Envelope Authority:** `Assets/Ashfall.Core/Verdict/VerdictSave.cs`
> **Radio State Model:** `VerdictRadioSystem.VerdictRadioState`

---

## 1. Persisted State Model

```csharp
[Serializable]
public class VerdictRadioState
{
    public string systemId = "verdict_radio_system";
    public List<string> firedIds = new List<string>();
}
```

## 2. Serialization & Persistence Invariants

1. **State Isolation:**
   Only the list of already-fired broadcast IDs (`firedIds`) is stored on disk. The corpus definitions, frequencies, messages, and schedules remain in `verdict_radio.json`.
2. **Determinism:**
   `CaptureState()` sorts `firedIds` ordinally via `StringComparer.Ordinal` before serializing, guaranteeing bit-exact deterministic checksum generation across runs.
3. **No Migration Penalty:**
   Because radio state is a dynamic hash set of fired strings, appending 17 new broadcasts to `verdict_radio.json` requires zero schema version bumps or migration logic. Existing saves with 0–13 fired IDs load flawlessly.
4. **Idempotency:**
   Restoring a save populates `_firedIds`. Subsequent calls to `Poll()` immediately skip all restored IDs, preventing duplicate broadcast events or re-triggered audio cues.
