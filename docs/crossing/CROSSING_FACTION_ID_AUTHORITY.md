# Crossing Faction ID Authority & Global Collision Audit

## 1. Crossing Faction Identifiers

All Crossing factions strictly use the `faction_the_<name>` snake_case convention:
- `faction_the_scale`
- `faction_the_underwrite`
- `faction_the_compact`
- `faction_the_lamplighters`
- `faction_the_granary_wardens`
- `faction_the_water_committee`
- `faction_the_quarantine_post`
- `faction_the_smugglers_court`

---

## 2. Code Constants in `CrossingIds.cs`

In `Assets/Ashfall.Core/CrossingCatalog.cs`:

```csharp
public static class CrossingIds
{
    public const string FactionScale = "faction_the_scale";
    public const string FactionUnderwrite = "faction_the_underwrite";
    public const string FactionCompact = "faction_the_compact";
    public const string FactionLamplighters = "faction_the_lamplighters";
    public const string FactionGranaryWardens = "faction_the_granary_wardens";
    public const string FactionWaterCommittee = "faction_the_water_committee";
    public const string FactionQuarantinePost = "faction_the_quarantine_post";
    public const string FactionSmugglersCourt = "faction_the_smugglers_court";
}
```

---

## 3. Global Collision Audit Results

A full recursive search across `Assets/StreamingAssets/Data/`, `Assets/Ashfall.Core/`, `src/`, and `Ashfall.Core.Tests/` confirms:
1. `faction_the_scale`: Shared intentionally with Standing Record (`standing_record_factions.json`) as the universal weights and measures authority.
2. `faction_the_underwrite`: Shared intentionally with Standing Record (`standing_record_factions.json`) as the commercial credit and underwriting authority.
3. `faction_the_compact`: Shared intentionally with Standing Record (`standing_record_factions.json`) as the charter and boundary documentation authority.
4. `faction_the_lamplighters`: Zero collisions. (Distinct from global `faction_lamplighters` in `currents.json`, representing the local Crossing municipal lighting guild).
5. `faction_the_granary_wardens`: Zero collisions.
6. `faction_the_water_committee`: Zero collisions.
7. `faction_the_quarantine_post`: Zero collisions.
8. `faction_the_smugglers_court`: Zero collisions.
