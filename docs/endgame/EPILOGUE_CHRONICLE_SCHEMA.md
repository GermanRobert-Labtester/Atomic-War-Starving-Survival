# Epilogue Chronicle Catalog Schema Specification

**Document ID:** `docs/endgame/EPILOGUE_CHRONICLE_SCHEMA.md`
**Data Authority:** `Assets/StreamingAssets/Data/epilogue_chronicle.json`
**C# Bindings:** `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs` & `EpilogueChronicleBuilder.cs`

---

## 1. JSON Envelope

The catalog is encapsulated by a versioned root object:

```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "title": "EpilogueChronicleCatalog",
  "type": "object",
  "required": ["schema_version", "default_slides"],
  "properties": {
    "schema_version": {
      "type": "integer",
      "minimum": 1,
      "description": "Schema format revision, currently pinned to 1."
    },
    "default_slides": {
      "type": "array",
      "minItems": 20,
      "maxItems": 20,
      "items": {
        "$ref": "#/definitions/EpilogueSlideDefinition"
      }
    }
  },
  "definitions": {
    "EpilogueSlideDefinition": {
      "type": "object",
      "required": ["order", "title", "art_asset_id"],
      "properties": {
        "order": {
          "type": "integer",
          "minimum": 0,
          "maximum": 19,
          "description": "Deterministic presentation index in the sequence."
        },
        "title": {
          "type": "string",
          "minLength": 1,
          "maxLength": 30,
          "description": "Human-readable concise slide title (1 to 4 words)."
        },
        "art_asset_id": {
          "type": "string",
          "pattern": "^epilogue_[a-z0-9_]+_placeholder$",
          "description": "Asset registry token pointing to visual art or placeholder."
        }
      }
    }
  }
}
```

---

## 2. Field Semantics & Constraints

| Field | Type | Constraint | Runtime Role |
|---|---|---|---|
| `schema_version` | integer | `== 1` | Verified by `CatalogIntegrityValidator` on load. |
| `order` | integer | `[0..19]`, strictly unique | Controls sorting in `EpilogueChronicleBuilder.Build(...)`. |
| `title` | string | 1–4 words, non-empty | Rendered in presentation header cards and slide decks. |
| `art_asset_id` | string | `epilogue_*_placeholder` | Resolved by Godot UI theme/sprite loader or fallback card. |

---

## 3. Type Bindings in Ashfall.Core

The schema maps directly to pure C# POCOs in `Assets/Ashfall.Core/Endgame/EpilogueChronicleCatalog.cs`:

```csharp
public sealed class EpilogueSlideDefinition
{
    public int order { get; set; }
    public string title { get; set; } = string.Empty;
    public string art_asset_id { get; set; } = string.Empty;

    public EpilogueSlide ToSlide(string prose = "") =>
        new EpilogueSlide(order, title, prose, art_asset_id);
}

public sealed class EpilogueChronicleCatalogData
{
    public int schema_version { get; set; } = 1;
    public List<EpilogueSlideDefinition> default_slides { get; set; } = new();
}
```
