# Expedition Destination Schema

From `ExpeditionJsonDto` (`Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs`).

## Fields

| Field | Type | Required | Valid range / semantics |
|---|---|---|---|
| `id` | string | yes | unique `loc_*` (legacy exceptions exist); globally registered |
| `displayName` | string | yes | grounded place name; never a raw ID |
| `distanceTicks` | int | yes | ≥ 1; fallback `travelHours × 2` else 8 |
| `dangerLevel` | float→int | yes | 1–10 (validated by Plan 32 wiring tests) |
| `encounterChancePerTick` | float | yes | (0, 1), runtime clamp ≤ 0.50; default `clamp(0.10 + danger×0.02, 0.05, 0.50)` |
| `baseStaminaDrainPerHour` | float | yes | > 0, clamp ≤ 5.0; default `clamp(1.5 + danger×0.25, 1.0, 5.0)` |
| `scavenging_table_id` | string | yes (post-Plan 46) | must resolve in `scavenging_tables.json` |
| `lootCategories` | string[] | yes | tokens resolve via `ExpeditionLootReferenceResolver` (item ID or semantic category) |
| `requiresDiscovery` | bool | no | discovery-gated destination (currently 2 + 1 new) |
| `travelHours` | float | no | legacy distance input |

## Catalog wrapper

`{"schema_version": 1, "expeditions": [...]}` via `CatalogLocator.LoadWrappedList`.

## Destination identity model

Expedition destinations are **self-owned physical identities** (`loc_*` in `expeditions.json`). `locations.json` and expansion location catalogs can also register expedition-capable sites through the same loader (first-seen wins — no duplicate identities). All 8 new IDs were checked against existing catalogs for collisions: none.

## Encounter-pressure model

Cumulative P(≥1 encounter) ≈ `1 − (1 − p)^ticks` under the simple independent model. Existing data already tolerates high cumulative pressure (Dead Hand Core: 18 ticks × 0.30 ≈ 0.996). New entries were tuned so pressure differentiates roles rather than stacking maximally (see roster doc).
