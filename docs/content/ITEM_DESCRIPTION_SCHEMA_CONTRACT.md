# Item Description Schema Contract

## 1. Top-Level Structure
`Assets/StreamingAssets/Data/item_description_texts.json` conforms to schema version 1.

```json
{
  "schema_version": 1,
  "collection_id": "item_description_texts",
  "descriptions": [
    {
      "item_id": "dosimeter",
      "category": "device",
      "base_description": "...",
      "current_state": "...",
      "visual_indicators": "...",
      "functional_description": "...",
      "sensory_details": "...",
      "emotional_weight": "...",
      "hazards": "...",
      "dependencies": "...",
      "contamination_status": "...",
      "preservation_state": "...",
      "makeshift_utility": "...",
      "alternatives": "...",
      "system_integration": "..."
    }
  ]
}
```

## 2. Field Specifications

| Field Name | Type | Required? | Description & Constraints |
|------------|------|-----------|---------------------------|
| `item_id` | `string` | **Yes** | Unique identifier in snake_case. Must not be null or whitespace. |
| `category` | `string` | **Yes** | Content category (e.g. `device`, `medical`, `protective`, `tool`, `material`). |
| `base_description` | `string` | **Yes** | Core narrative overview of the physical item. |
| `current_state` | `string` | No | Operational condition, wear, degradation, or structural damage. |
| `visual_indicators` | `string` | No | Visible hallmarks, colors, engravings, oxidation, warning labels. |
| `functional_description` | `string` | No | Practical utility and operation in survival conditions. |
| `sensory_details` | `string` | No | Weight, tactile feel, sound (clicks, hums), smell, thermal conduct. |
| `emotional_weight` | `string` | No | Psychological impact, memories, symbolic gravity in the ash wasteland. |
| `hazards` | `string` | No | Operational risks, toxic leaching, failure consequences. |
| `dependencies` | `string` | No | Consumables, power sources, calibration, maintenance needed. |
| `contamination_status` | `string` | No | Radiometric/biological contamination status and wash rules. |
| `preservation_state` | `string` | No | Chemical/physical preservation, oxidation resistance. |
| `makeshift_utility` | `string` | No | Alternative field uses and emergency adaptations. |
| `alternatives` | `string` | No | Waste substitute options and trade-offs. |
| `system_integration` | `string` | No | Gameplay system tie-in summary. |

## 3. Deserialization & Robustness Rules
1. **Case-Insensitive Property Matching**: In C# deserialization, JSON snake_case fields map cleanly to C# PascalCase or snake_case members.
2. **Missing/Null Safety**: Optional fields missing from JSON or null in payload are normalized to `string.Empty`, never null in public domain models.
3. **Graceful Degradation**: If an item in inventory lacks an entry in `ItemDescriptionCatalog`, the inspection model falls back to `ItemDefinition.description`.
