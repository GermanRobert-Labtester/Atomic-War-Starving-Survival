# Faction War Location Override Schema Specification

> **Target File:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Mirror File:** `builds/linux/Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Version:** Schema Version 1

---

## 1. Schema Overview

`faction_war_location_overrides.json` defines temporal modifications applied to canonical wasteland locations as the faction war evolves over time.

### 1.1 JSON Root Object
```json
{
  "schema_version": 1,
  "locationOverrides": [
    { ... }
  ]
}
```

| Property | Type | Description |
|---|---|---|
| `schema_version` | integer | Data schema version. Must be `1`. |
| `locationOverrides` | array of objects | Ordered array of temporal location overrides. |

---

## 2. Override Entry Object Definition

Each entry in the `locationOverrides` array contains the following fields:

```json
{
  "id": "loc_override_checkpoint_occupied",
  "locationId": "loc_garrison_checkpoint_gamma",
  "overrideType": "occupied",
  "activeFromDay": 200,
  "activeUntilDay": 250,
  "displayNameOverride": "Garrison Checkpoint Gamma (Reinforced Redoubt)",
  "descriptionOverride": "Reinforced log revetments and wire coils encircle the checkpoint gatehouse...",
  "atmosphereOverride": "A smell of burning green wood, harsh coal soot, and oiled rifle mechanisms."
}
```

### 2.1 Field Constraints

| Field | Type | Required | Rules & Validation |
|---|---|---|---|
| `id` | string | Yes | Unique string identifier. Must start with the prefix `loc_override_`. Snake_case naming convention. |
| `locationId` | string | Yes | Must match an existing canonical location ID in the game catalogs (`locations.json`, `deep_lore_locations.json`, `crossing_locations.json`, `year_of_ash_locations.json`, etc.). |
| `overrideType` | string | Yes | Must be one of the 9 canonical types: `pre_strike`, `post_strike`, `ambient_addendum`, `occupied`, `abandoned`, `fortified`, `liberated`, `reclaimed`, `contaminated`. |
| `activeFromDay` | integer | Yes | Campaign day when this override becomes active. Must be `>= 1`. |
| `activeUntilDay` | integer | Yes | Campaign day when this override expires. Must be either `0` (unbounded / permanent) or `>= activeFromDay`. |
| `displayNameOverride` | string | Yes | Player-facing modified location title. Non-empty string. |
| `descriptionOverride` | string | Yes | Player-facing evocative narrative description of the altered site. Non-empty string. |
| `atmosphereOverride` | string | Yes | Player-facing sensory / atmospheric descriptor. Non-empty string. |

---

## 3. C# Class Definition & Serialization Contract

Located in `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`:

```csharp
namespace Ashfall.Core.YearOfAsh
{
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
}
```

The C# data transfer objects strictly avoid engine-specific dependencies (`Godot`, `UnityEngine`) adhering to Core Invariant 1. Serialization round-trips cleanly via `IJsonSerializer`.
