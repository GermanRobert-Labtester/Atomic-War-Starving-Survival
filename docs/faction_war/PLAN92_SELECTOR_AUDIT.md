# Plan 92 — Selector Semantics & Runtime Audit

> **Target Class:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
> **Method Under Audit:** `GetDialogueForLocation(string locationId, int day)`

---

## 1. Architectural Examination

### 1.1 Source Code Implementation
```csharp
public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day)
{
    var result = new List<FactionWarDialogueSnippet>();
    for (int i = 0; i < _dialogueSnippets.Count; i++)
    {
        var s = _dialogueSnippets[i];
        if (s != null && s.minDay <= day &&
            string.Equals(s.locationId, locationId, StringComparison.Ordinal))
            result.Add(s);
    }
    return result;
}
```

### 1.2 Evaluation Attributes
1. **Filtering Mechanism:**
   - Evaluates whether `s.locationId == locationId` using ordinal string comparison.
   - Evaluates whether `s.minDay <= day`.
   - Both predicates must hold for inclusion in the output list.
2. **State & Repetition Semantics:**
   - The selector is **completely stateless**.
   - No `seen` list is stored in Core or persisted to save files.
   - No cooldown timestamps are tracked.
   - All eligible snippets for a location remain eligible from their `minDay` onward.
3. **Determinism:**
   - Iteration follows the deserialized array index `i = 0` to `N - 1`.
   - Returns a new `List<FactionWarDialogueSnippet>` deterministically.
   - Zero non-deterministic random selection in Core. Host sessions or UI presenters that choose a line can apply seeded RNG over the returned list.
4. **Save State Impact:**
   - Because `FactionWarDialogueSnippet` has no persistent tracking fields in `YearOfAshSave` or `CampaignEnvelope`, expanding the catalog introduces **zero save migration overhead** and **zero save-load breaking risk**. Pre-Plan-92 saves remain 100% byte-compatible.
