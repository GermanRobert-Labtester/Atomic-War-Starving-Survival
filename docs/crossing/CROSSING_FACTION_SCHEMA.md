# Crossing Faction Schema Specification

## 1. DTO Structure

Defined in `Assets/Ashfall.Core/CrossingCatalog.cs`:

```csharp
public class CrossingFactionEntry
{
    public string id;
    public string display_name;
    public string alignment;
    public string home_region;
    public bool is_active;
    public float trust;
    public string[] wants;
    public string[] offers;
    public string signature_quote;
    public string access_rule;
    public string badge_asset_id;
}
```

---

## 2. Field Specifications

| Field | Type | Required | Description / Contract |
|---|---|---|---|
| `id` | `string` | Yes | Unique snake_case identifier starting with `faction_the_`. |
| `display_name` | `string` | Yes | Human-readable title in title case (e.g., "The Lamplighters"). |
| `alignment` | `string` | Yes | Starting institutional stance: `conditional`, `peaceful`, `neutral`, `allied`, `hostile`. |
| `home_region` | `string` | Yes | Authoritative region ID (`region_crossing` for Crossing charter factions). |
| `is_active` | `bool` | Yes | Whether the faction is active in the settlement economy (`true`). |
| `trust` | `float` | Yes | Initial base trust level (`0` default, valid range [-50, 50]). |
| `wants` | `string[]` | Yes | Array of 1–3 snake_case resource/commodity tags desired by the faction. |
| `offers` | `string[]` | Yes | Array of 2–3 snake_case service/boon tags provided by the faction. |
| `signature_quote` | `string` | Yes | Exactly one voice-defining sentence conveying ideology and institutional attitude. |
| `access_rule` | `string` | Yes | Concrete prose explaining how the player maintains or loses access to services. |
| `badge_asset_id` | `string` | Yes | Badge texture reference or empty string (falls back to `icon_unknown_faction.png`). |

---

## 3. Serialization Rules

- Serialized inside an object envelope with `"schema_version": 1`.
- The collection property name is `"actions"`, conforming to the initial authoring schema.
- Loaded through `CrossingCatalogLoader.LoadList<CrossingFactionEntry>()`.
