# Plan 124 — Faction War Location Overrides Expansion Baseline & Forensics

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Build Parity Mirror:** `builds/linux/Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Core Loader & Indexer:** `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
> **Verification Test Suite:** `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`

---

## 1. Verified Baseline Reconnaissance

### 1.1 Catalog State Prior to Plan 124
Prior to Plan 124, `faction_war_location_overrides.json` contained **9 verified location overrides** (`schema_version: 1`).
Each override record defines a temporal state modification applied to an existing canonical location during a specific day window:
- `id`: string (`loc_override_<semantic_descriptor>`)
- `locationId`: string (references canonical location ID in `locations.json`, `deep_lore_locations.json`, etc.)
- `overrideType`: string (semantic descriptor of change: `pre_strike`, `post_strike`, `ambient_addendum`)
- `activeFromDay`: integer (inclusive start day)
- `activeUntilDay`: integer (inclusive end day, or `0` for permanent/unbounded)
- `displayNameOverride`: string (modified name for the location)
- `descriptionOverride`: string (modified descriptive text)
- `atmosphereOverride`: string (environmental and sensory mood)

### 1.2 Core Loader & Query Contract
`FactionWarContentCatalogLoader` loads `faction_war_location_overrides.json` into `FactionWarLocationOverridesRoot`:
```csharp
[Serializable]
public sealed class FactionWarLocationOverride
{
    public string id = string.Empty;
    public string locationId = string.Empty;
    public string overrideType = string.Empty;
    public int activeFromDay;
    public int activeUntilDay;
    public string displayNameOverride = string.Empty;
    public string descriptionOverride = string.Empty;
    public string atmosphereOverride = string.Empty;
}

[Serializable]
public sealed class FactionWarLocationOverridesRoot
{
    public int schema_version = 1;
    public List<FactionWarLocationOverride> locationOverrides = new();
}
```

The selector function in `FactionWarContentCatalog` evaluates overrides dynamically:
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

### 1.3 Selector Semantics & Operational Rules
1. **Window Filtering:** An override matches if `day >= o.activeFromDay` AND `(o.activeUntilDay <= 0 || day <= o.activeUntilDay)`. Both day boundaries are strictly inclusive.
2. **Precedence:** When multiple overrides match the same location for the current day, the one with the highest `activeFromDay` wins (`o.activeFromDay > best.activeFromDay`). If two overrides share the exact same `activeFromDay`, the earlier one declared in the catalog list wins.
3. **Restoration / Default:** When no override is active (e.g. before `activeFromDay` or after `activeUntilDay`), `GetActiveLocationOverride` returns `null`, causing callers to render the canonical base location identity and description.
4. **Stateless Evaluation:** The system does not mutate save state or keep track of "seen" overrides; evaluation is deterministic, pure, and functionally computed from `(locationId, day)`.

---

## 2. Original Baseline Roster (Entries 0..8)

| Index | Override ID | Target Location ID | Type | Days | Display Name Override |
|---|---|---|---|---|---|
| 0 | `loc_override_almshouse_pre_strike` | `loc_almshouse` | `pre_strike` | 1–100 | St. Jude's Almshouse (Pre-Strike) |
| 1 | `loc_override_almshouse_post_strike` | `loc_almshouse` | `post_strike` | 101–300 | St. Jude's Almshouse (Shelling Ruin) |
| 2 | `loc_override_plaza_cleared` | `loc_ration_queue_plaza` | `pre_strike` | 1–50 | Ration Distribution Plaza (Clear) |
| 3 | `loc_override_silo_fortified` | `loc_grain_silo` | `post_strike` | 200–350 | Grain Silo (Hardened Outpost) |
| 4 | `loc_override_conscription_burned` | `loc_conscription_office` | `post_strike` | 220–340 | Conscription Office (Gutted Shell) |
| 5 | `loc_override_cache_looted` | `loc_d9_cache_bunker_delta` | `post_strike` | 300–450 | D/9 Cache Delta (Breached Vault) |
| 6 | `loc_override_weighbridge_barricaded` | `loc_weighbridge` | `post_strike` | 250–380 | North Weighbridge (Double Barricade) |
| 7 | `loc_override_waystation_refugee` | `loc_shrine_switchback_waystation` | `ambient_addendum` | 150–280 | Switchback Waystation (Overcrowded) |
| 8 | `loc_override_understory_transmitter_ambient` | `loc_understory_transmitter` | `ambient_addendum` | 480–600 | Understory Mast Site (Listening Post) |

All 9 baseline overrides remain unmodified in indices 0..8 of `faction_war_location_overrides.json`, preserving byte-for-byte fidelity and existing runtime contracts.

---

## 3. Plan 124 Expansion Scope

Plan 124 expands the catalog from **9 to 20 total overrides** (+11 newly authored entries) covering days 200 to 400 of the mid-to-late campaign.
The expansion incorporates locations from:
- **Deep Lore Locations (Plan 116):** 4 locations (`location_municipal_water_reservoir`, `location_chemical_plant`, `location_metro_station`, `location_burned_woodland`).
- **Year of Ash Crisis Outcomes (Plan 114):** 3 locations (`loc_garrison_checkpoint_gamma`, `loc_sector_4_rail_switchyard`, `loc_ash_militia_deadfall_barrier`).
- **Crossing Expansion (Plan 115):** 2 locations (`loc_crossing_granary_pledge`, `loc_crossing_petition_tent`).
- **Core Settlements & Infrastructure:** 2 locations (`loc_settlement_iron_siding`, `loc_bridge_seven`).
