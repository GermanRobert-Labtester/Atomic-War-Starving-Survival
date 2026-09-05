# Archive Ink Schema Specification

> **Schema Authority:** `Assets/Ashfall.Core/ArchiveDeskSystem.cs` (`InkMaterialDefinition`) and `Assets/StreamingAssets/Data/archive_inks.json`.

---

## 1. JSON Schema

```json
{
  "schema_version": 1,
  "collection_id": "archive_inks",
  "inks": [
    {
      "ink_id": "ink_example",
      "display_name": "Example Ink",
      "legibility_score": 0.8,
      "archival_longevity_days": 365,
      "fade_rate_per_day": 0.001,
      "required_item_id": "charcoal",
      "required_amount": 1
    }
  ]
}
```

---

## 2. Field Specifications

| Field Name | Type | Required | Default | Description / Valid Range |
|---|---|---|---|---|
| `ink_id` | `string` | **Yes** | `""` | Unique identifier prefixed with `ink_` (snake_case) |
| `display_name` | `string` | **Yes** | `""` | Non-empty human-readable formulation name |
| `legibility_score` | `float` | **Yes** | `1.0` | Initial transcription clarity multiplier `[0.3, 1.0]` |
| `archival_longevity_days` | `float` | **Yes** | `365.0` | Document preservation duration in days `[50, 1000]` |
| `fade_rate_per_day` | `float` | **Yes** | `0.001` | Legibility decay per elapsed campaign day `[0.0005, 0.02]` |
| `required_item_id` | `string` | **Yes** | `""` | Foreign key matching an existing `id` in `items.json` |
| `required_amount` | `int` | **Yes** | `1` | Quantity of ingredient consumed per job `[1, 5]` |

---

## 3. Serialization Rules

- Deserialized by `ArchiveInkCatalogLoader.Load` via `SystemTextJsonSerializer`.
- `InkMaterialDefinition` carries `[JsonPropertyName]` mappings for all snake_case properties with fallback to camelCase.
- All 12 IDs must be unique across the catalog.
