# Faction War Override Precedence & Conflict Resolution

> **Core Algorithm:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` (`GetActiveLocationOverride`)
> **Test Suite:** `Ashfall.Core.Tests.FactionWarLocationOverridesExpansionTests.SequentialOverrides_Almshouse_TransitionsAtDay101`

---

## 1. Resolution Algorithm

When a system queries `GetActiveLocationOverride(locationId, day)`, the method iterates through all registered overrides in `_locationOverrides` and resolves conflicts deterministically:

```csharp
public FactionWarLocationOverride? GetActiveLocationOverride(string locationId, int day)
{
    FactionWarLocationOverride? best = null;
    for (int i = 0; i < _locationOverrides.Count; i++)
    {
        var o = _locationOverrides[i];
        if (o == null) continue;
        if (!string.Equals(o.locationId, locationId, StringComparison.Ordinal)) continue;
        if (day < o.activeFromDay) continue;
        if (o.activeUntilDay > 0 && day > o.activeUntilDay) continue;
        if (best == null || o.activeFromDay > best.activeFromDay)
            best = o;
    }
    return best;
}
```

### 1.1 Decision Rules
1. **Target Matching:** Must match `locationId` using `StringComparison.Ordinal`.
2. **Active Window Check:** Day must be within `[activeFromDay, activeUntilDay]` (or `[activeFromDay, ∞)` if `activeUntilDay == 0`).
3. **Highest Start Day Wins:** If multiple candidates qualify, the candidate with the strictly greater `activeFromDay` replaces `best`.
4. **Tie-Breaking Rule:** If two candidates match and have the identical `activeFromDay`, the condition `o.activeFromDay > best.activeFromDay` is false, so the earlier entry in the list retains precedence.
5. **No Candidates:** Returns `null`, indicating the location should render its baseline canonical data.

---

## 2. Practical Case Studies

### 2.1 Sequential Evolution: St. Jude's Almshouse (`loc_almshouse`)
The almshouse features two successive temporal states:
1. `loc_override_almshouse_pre_strike` (Days 1–100)
2. `loc_override_almshouse_post_strike` (Days 101–300)

- **Day 50:** Only `loc_override_almshouse_pre_strike` is active. Returns Pre-Strike.
- **Day 100:** Last day of Pre-Strike. Returns Pre-Strike.
- **Day 101:** `loc_override_almshouse_post_strike` becomes active (`101 >= 101`). Returns Shelling Ruin.
- **Day 300:** Last day of Post-Strike. Returns Shelling Ruin.
- **Day 301:** Neither override is active. Returns `null` (reverts to baseline St. Jude's Almshouse description).

### 2.2 Non-Overlapping Expansion Invariant
All 11 newly authored overrides in Plan 124 target distinct locations from each other and from the baseline entries. Consequently, within the new expansion set:
- Zero unexpected shadowing or unintended precedence conflicts occur.
- Each new location cleanly transitions: `Default -> Override State -> Default`.
