# Hardcore Tier Selection Contract

## 1. Selection Semantics

The scarcity tier selection contract is implemented in `HardcoreEconomyTuning.GetScarcityMultiplier(int currentDay, string itemId)`:

```csharp
foreach (var entry in _bundle.ScarcityTiers)
{
    if (MatchesDay(entry, currentDay) && MatchesItem(entry.AffectedItemIds, itemId))
        return entry.Multiplier;
}
return 1.0f;
```

### Determinism & Precedence Rules:
1. **Linear Evaluation:** Tiers are evaluated in array declaration order. The first tier matching both `currentDay` and `itemId` returns its multiplier immediately.
2. **Boundary Overlap Behavior:** If two tiers declare overlapping day boundaries (e.g. `Days 1-15` and `Days 15-40`), the earlier declared tier takes precedence for any item it affects.
3. **Item Specificity:** If Day 15 is evaluated for an item declared in Tier 0 (`Critical`), Tier 0 applies (multiplier 2.5). If Day 15 is evaluated for an item only declared in Tier 1 (`High`), Tier 1 applies (multiplier 2.0).
4. **Fallback:** If no tier matches the requested day and item combination, the system defaults to baseline parity (`1.0f`).

---

## 2. Day Range Grammar

`MatchesDay` supports three standardized label formats:
- **Bounded Interval with Prefix:** `"Days X-Y"` (e.g. `Days 1-15`, `Days 41-100`). Matches `currentDay >= X && currentDay <= Y`.
- **Bounded Interval without Prefix:** `"X-Y"` (e.g. `1-10`, `20-30`). Matches `currentDay >= X && currentDay <= Y`.
- **Open-Ended Horizon:** `"Days X+"` or `"X+"` (e.g. `Days 341+`). Matches `currentDay >= X`.
