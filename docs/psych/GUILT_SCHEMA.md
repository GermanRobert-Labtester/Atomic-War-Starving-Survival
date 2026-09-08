# Plan 66 — Guilt Sources Schema & Grammar Specification

**Authority:** `Assets/StreamingAssets/Data/guilt_sources.json`
**System Consumer:** `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`
**Schema Version:** `1`

---

## 1. Schema Structure

The guilt sources catalog defines the authoritative vocabulary of psychological consequence triggers in ASHFALL. It is serialized as snake_case JSON with a top-level `schema_version` and an `items` array:

```json
{
  "schema_version": 1,
  "items": [
    {
      "choice_pattern": "string",
      "severity": 0.0,
      "description": "string",
      "title": "string"
    }
  ]
}
```

### Field Definitions

| Field | Type | Constraint | Description |
|---|---|---|---|
| `choice_pattern` | `string` | Unique `snake_case` | Identifier for the triggering player choice or event. |
| `severity` | `float` | `0.10` to `1.00` | Magnitude of guilt burden contributed to survivor insomnia severity. |
| `description` | `string` | 1–2 sentences, contains `{name}` | Restrained sensory/procedural memory of the act. |
| `title` | `string` | 2–5 words | Human-readable title of the moral residue. |

---

## 2. Prose & Tone Standard

Guilt in ASHFALL is not an objective moral judge, alignment score, or karma meter. It reflects internal psychological residue.
1. **Restrained, Cold, Human:** No moralizing or preachy language ("you did an evil thing", "you feel terrible").
2. **Anchored in Concrete Artifacts:** Emphasizes sensory details, physical absences, altered routines, empty containers, grease pencil markings, and unspoken phrases.
3. **Survivor Placeholder:** Uses `{name}` to allow runtime interpolation of the specific survivor carrying the memory.
