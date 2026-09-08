# Feedback Message Schema Contract

## 1. Schema Definition

The canonical feedback message catalog is stored at:
`Assets/StreamingAssets/Data/feedback_messages.json`

Root envelope:
```json
{
  "schema_version": 1,
  "messages": [
    {
      "key": "string",
      "category": "string",
      "severity": "string",
      "template": "string",
      "parameter_count": 0,
      "display_duration_seconds": 3.0
    }
  ]
}
```

---

## 2. Field Specifications

| Field | Type | Constraints | Description |
|---|---|---|---|
| `schema_version` | integer | Must be >= 1 | Catalog schema revision |
| `key` | string | Non-empty snake_case | Template lookup key |
| `category` | string | Non-empty enum-like string | Scoping category (success, warning, error, etc.) |
| `severity` | string | `info`, `success`, `warning`, `error`, `critical` | Visual and audio severity classification |
| `template` | string | Non-empty formatting template | Composite string containing `{0}`, `{1}`, etc. |
| `parameter_count` | integer | Non-negative (0 to 10) | Exact count of required argument placeholders |
| `display_duration_seconds` | float | 1.0 to 15.0 seconds | Base on-screen duration before auto-dismissal |

---

## 3. Validation Rules

1. **Placeholder Match:** Every template containing placeholders up to `{N}` must have `parameter_count == N + 1`. Missing or excess parameter declarations fail validation.
2. **Severity Parsing:** If `severity` is unrecognized, it must safely fall back to `info` (`FeedbackSeverity.Info`).
3. **Duration Bounds:** Values below 1.0s or above 15.0s are clamped to the safe presentation range `[1.0f, 15.0f]`.
4. **Key Lookup & Scoping:** Keys must be indexable both flat (`_templatesByKey`) and by category composite (`$"{category}:{key}"`). When flat keys collide across different categories, category-scoped lookup disambiguates them.
5. **Safe Formatting:** Under no circumstances may argument substitution throw `FormatException` or `IndexOutOfRangeException` at runtime. Safe formatting substitutes available arguments and replaces missing placeholders with empty strings or default placeholders.
