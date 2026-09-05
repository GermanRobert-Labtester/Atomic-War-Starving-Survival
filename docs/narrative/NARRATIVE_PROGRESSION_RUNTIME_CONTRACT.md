# Narrative Progression — Runtime Contract

_ASHFALL · docs/narrative · Plan 74_

---

## Authority

`Assets/StreamingAssets/Data/narrative_progression.json` is the **sole authority** for the ordered chapter list.

---

## Schema

```json
{
  "schema_version": 1,
  "entries": [
    {
      "description": "Chapter N <Status>: <Title> — <one-sentence summary>",
      "order": N
    }
  ]
}
```

### Fields

| Field | Type | Constraint |
|---|---|---|
| `schema_version` | int | Must be `1`; required by CatalogIntegrityValidator |
| `entries` | array | Ordered list of chapter entries |
| `description` | string | Human-readable; display only — no engine parsing |
| `order` | int | 1-based; must be unique within the file |

### Fields that do NOT exist in the runtime DTO

Do **not** add any of these — they are silently dropped by the deserializer:

- `trigger_day` — belongs to the incident/event system
- `phase` — belongs to the expansion phase system
- `world_state_changes` — belongs to IEventBus / flag system
- `winter_window` — belongs to WeatherSystem
- `faction_unlock` — belongs to faction catalogs

---

## Consumer

`src/Host/EventsHostSession.cs` → `LoadNarrativeProgression()` → `NarrativeRoot` → `List<NarrativeEntryData>`

```csharp
public class NarrativeEntryData
{
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
}
```

Exposed via `GetNarrativeProgression()` for `NarrativePanel` (UI display) and `NarrativeEncounterSystem` (encounter gating by chapter order).

---

## Chapter Status Tokens

Display convention in the `description` field. The runtime does not parse these:

| Token | Meaning (display) |
|---|---|
| `Complete:` | Chapter finished |
| `Active:` | Current chapter |
| `Pending:` | Future chapter |
| _(none)_ | Mid/late-game chapters without pre-authored status |

---

## Ordering Contract

- `order` values 1–5 are legacy and must not be renumbered.
- `order` values 6–15 are Plan 74 additions and are now stable.
- New campaigns may extend beyond 15 with the next sequential integer.
- The runtime loads all entries; no filtering by `order` range is applied.

---

## Modification Rules

1. Read `EventsHostSession.cs` (the DTO) before adding any field.
2. Never add fields not in `NarrativeEntryData`.
3. Never renumber existing chapters 1–15.
4. Run `--data-integrity-selftest` after every edit.

---

_Last updated: Plan 74 — 2026-09-03_
