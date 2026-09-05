# Verdict Location Schema Specification

> **Authority:** `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`
> **Data Target:** `Assets/StreamingAssets/Data/verdict_locations.json`

---

## 1. Top-Level File Envelope

```json
{
  "schema_version": 1,
  "locations": [
    { ... }
  ]
}
```

- `schema_version` (integer, required): Evaluated and required by `CatalogIntegrityValidator`. Current canonical version is `1`.
- `locations` (array of objects, required): The collection of `VerdictLocationEntry` records.

---

## 2. Record Fields (`VerdictLocationEntry`)

| Field | Type | Required | Constraints / Semantics | Default |
|---|---|:---:|---|:---:|
| `id` | `string` | Yes | Canonical snake_case prefixed with `loc_`. Unique across all game catalogs. | `""` |
| `displayName` | `string` | Yes | Human-readable title displayed on investigation maps and panels. | `""` |
| `description` | `string` | Yes | 3–6 sentences of dense, grounded environmental prose with physical clues and contradictions. | `""` |
| `dangerLevel` | `int` | Yes | Combat, structural, and encounter hazard index (3 to 10 scale). | `5` |
| `travelHours` | `float` | Yes | Simulation hours required for one-way foot/transit journey (3.0 to 12.0h). | `5.0` |
| `baseRadsPerHour` | `float` | Yes | Chronic ambient radiological rate at the location (20.0 to 60.0 rad/h). | `30.0` |

---

## 3. Serialization Contract

- Deserialized via `CatalogLocator.LoadWrappedList<VerdictLocationEntry>(raw, SystemTextJsonSerializer.Options)`.
- Core loader gracefully skips null or empty-ID entries without throwing.
- No parallel runtime fields (`trail_id`, `next_location`) are present in data; sequence is emergent from prose, quest states, NPC dialogue, and radio cues.
