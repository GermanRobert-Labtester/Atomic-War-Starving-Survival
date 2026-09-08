# Final Wish Save Contract & Backward Compatibility

**Document:** `docs/survivors/FINAL_WISH_SAVE_CONTRACT.md`

---

## 1. DTO Specification

```csharp
[Serializable]
public sealed class FinalWishSurvivorState
{
    public string survivorId = string.Empty;
    public string wishType = string.Empty;
    public float daysRemaining;
    public int stepsCompleted;
    public bool isActive;
    public bool hasTerminalPrognosis;
    public bool wishCompleted;
}

[Serializable]
public sealed class FinalWishSaveState
{
    public List<FinalWishSurvivorState> survivors = new List<FinalWishSurvivorState>();
    public Dictionary<string, string> archetypeWishes = new Dictionary<string, string>();
}
```

---

## 2. Backward Compatibility Guarantees

1. **Zero Save Schema Changes:** Expanding `final_wishes.json` from 8 to 30 templates requires zero changes to `FinalWishSaveState` or `FinalWishSurvivorState`.
2. **Old Save Migration:** Pre-Plan 65 saves continue to load cleanly because wish types and archetypes are represented as string keys.
3. **Mid-Progress Round-Trip:** `stepsCompleted` and `daysRemaining` preserve floating-point and integer state exactly without reset on reload.
4. **Idempotent Restoration:** Restoring a save clears existing dictionaries and repopulates deep copies, preventing memory reference leaks.
