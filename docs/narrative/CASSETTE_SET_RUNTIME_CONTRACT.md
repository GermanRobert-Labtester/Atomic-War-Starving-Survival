# Cassette Set Runtime Contract — Plan 67 Audit (Task 67A)

Verified 2026-09-06 against active source. Authority: `Assets/StreamingAssets/Data/cassette_sets.json`, `CatalogIntegrityValidator.cs`, `CatalogIntegrityRules.cs`, `ContentUtilizationScanner.cs`, `ScavengingTableCatalog.cs`, `ExpeditionSystem.cs`.

## File schema — `cassette_sets.json`

Top level: `schema_version: 1`, `items[]` (one record per set).

### Set record

| Field | Type | Semantics |
|---|---|---|
| `set_id` | string | snake_case identity (definition position). |
| `set_title` | string | Raw display text (not a localization key). |
| `total_parts` | int | Declared part count. |
| `parts` | array | Ordered part records. |
| `hidden_cache_location` | string | **Reference key** — must resolve to a location id in `locations.json`. |
| `hidden_cache_items` | string[] | **Reference key** — every element must resolve to an item id (`items.json` or another definition file). |
| `completion_narrative` | string | **TIER-1 reference** (`narrative_` prefix) — must resolve to an event id in `events.json`. |

### Part record

| Field | Type | Semantics |
|---|---|---|
| `part` | int | Explicit 1-based part number. Order authority. |
| `item_id` | string | **Definition key** — the part's cassette item identity is *defined here*, in-place. Existing convention: `cassette_<set>_<n>`, no zero-padding, no `item_` prefix. Not required to exist in `items.json`. |
| `title` | string | Raw display text. |
| `description` | string | Raw prose transcript (narration frame + quoted voice). |

## Non-existent fields (do not add)

- `location_hint` — does not exist. Placement is expressed through Plan 46 scavenging tables and `hidden_cache_location`.
- `journal_unlock` — does not exist. Completion payoff is the `completion_narrative` event reference.
- No morale field at set or part level. `VinylMoraleSystem` consumes vinyl records only; the generic `item_cassette_tape` carries `moraleEffect: 3` at item level.

## Runtime consumers

- **No runtime loader exists for set cassettes.** `RadioRecordingSystem` is a separate broadcast-recording feature with its own save state (`RecordedCassetteEntry`) and does not read `cassette_sets.json`.
- `CatalogIntegrityValidator` scans the file generically: definition keys register ids; reference keys and TIER-1 prefixes must resolve.
- `ContentUtilizationScanner` classifies the file as Audio and records its expected consumers (`VinylMoraleSystem` registry, `VinylPanel` UI).
- `ScavengingTableCatalog` + `ExpeditionSystem.AddLoot` accept any string `item_id` — cassette part ids placed in loot tables flow through without an `items.json` lookup.

## Placement architecture

- Plan 46 scavenging tables **are merged** (49 tables in `scavenging_tables.json`).
- Loot entry shape: `{ item_id, weight, min_quantity, max_quantity, rarity_tier }`.
- For parts to behave as first-class physical items (weight, trade value, unique stacking), an `items.json` record with matching `id` is added — consistent with existing loose-prefix item ids (`bandage`, `antibiotics`).

## Completion semantics

- There is no runtime completion state to fire; the `completion_narrative` event id follows the existing data pattern (`events.json`, `weight: 1`, `minDay: 18`) used by all four original sets. Adding a completion narrative is a data operation only.

## Localization model

Raw text throughout. No localization keys.
