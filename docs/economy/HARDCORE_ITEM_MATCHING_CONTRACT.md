# Hardcore Item Matching Contract

## 1. Matching Rules

`HardcoreEconomyTuning.MatchesItem(IReadOnlyList<string> affectedIds, string itemId)` evaluates item eligibility against three distinct token formats:

```csharp
private static bool MatchesItem(IReadOnlyList<string> affectedIds, string itemId)
{
    if (affectedIds.Count == 0) return true; // empty list = universal match
    foreach (var token in affectedIds)
    {
        var trimmed = token.Trim();
        if (trimmed == "*"
            || string.Equals(trimmed, itemId, StringComparison.OrdinalIgnoreCase)
            || (trimmed.EndsWith("*", StringComparison.Ordinal)
                && itemId.StartsWith(trimmed.Substring(0, trimmed.Length - 1), StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }
    }
    return false;
}
```

### Supported Token Syntaxes:
1. **Universal Wildcard (`*`):**
   - Matches any query `itemId`.
   - Used in broad price shocks such as `PlumePassing` where all traded goods spike across the route.
2. **Exact Item Identifier:**
   - Case-insensitive ordinal equality (`string.Equals(..., StringComparison.OrdinalIgnoreCase)`).
   - Examples: `clean_water`, `antibiotics`, `fuel`, `scrap_mechanical`, `engine`.
3. **Prefix Wildcard (`<prefix>_*`):**
   - Matches any item whose ID begins with the specified prefix.
   - Example: `ammo_*` matches `ammo_9x19`, `ammo_762`, `ammo_308`, `ammo_12g`, etc.
   - Example: `chart_*` matches `chart_coastal`, `chart_soundings`, etc.

---

## 2. Validation Constraints

1. **Case-Insensitive Normalization:** All comparisons use `OrdinalIgnoreCase` to prevent cross-platform file or identifier casing discrepancies.
2. **Whitespace Trimming:** Leading and trailing whitespace is stripped before evaluation.
3. **No Substring Collisions:** Non-wildcard tokens require full string equality. An affected item token `fuel` will never accidentally match an item named `refuel_hose` or `fuel_filter`.
