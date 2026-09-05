# Wasteland Grave Epitaph Schema Contract

**Target File:** `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`
**Schema Version:** `1`
**JSON Structure:** Root Object with `schema_version` and `epitaphs` array

---

## 1. Schema Specification

The catalog uses standard UTF-8 JSON conforming to the following shape:

```json
{
  "schema_version": 1,
  "epitaphs": [
    {
      "cause": "<cause_string>",
      "epitaph": "<memorial_text>"
    }
  ]
}
```

### 1.1 Field Definitions

| Field Name | Type | Required | Description | Constraints |
|---|---|---|---|---|
| `schema_version` | integer | Yes | Data authority schema version | Must be positive integer (currently `1`) |
| `epitaphs` | array | Yes | Collection of epitaph records | Exact count: 30 entries |
| `cause` | string | Yes | Cause of death classifier | Lower snake_case string (e.g., `radiation`, `combat`, `unspecified`) |
| `epitaph` | string | Yes | The textual inscription on the grave | Non-empty string; 1 sentence; 5–20 words |

---

## 2. Parsing and Deserialization

When consumed in C#, the catalog is modeled as:

```csharp
public sealed class WastelandGraveEpitaphsCatalog
{
    [JsonPropertyName("schema_version")]
    public int SchemaVersion { get; set; } = 1;

    [JsonPropertyName("epitaphs")]
    public List<WastelandGraveEpitaphRecord> Epitaphs { get; set; } = new();
}

public sealed class WastelandGraveEpitaphRecord
{
    [JsonPropertyName("cause")]
    public string Cause { get; set; } = string.Empty;

    [JsonPropertyName("epitaph")]
    public string Epitaph { get; set; } = string.Empty;
}
```

---

## 3. Invariants

1. **Root Schema Version:** Must have top-level `"schema_version": 1`.
2. **Key Exactness:** Keys are exactly `"cause"` and `"epitaph"`. No extraneous properties.
3. **No Empty Values:** Neither `cause` nor `epitaph` may be empty or whitespace.
4. **Encoding:** UTF-8 without BOM. Standard ASCII quotes, no unescaped control characters.
5. **No Duplicates:** Every `epitaph` string must be globally unique across the catalog.
