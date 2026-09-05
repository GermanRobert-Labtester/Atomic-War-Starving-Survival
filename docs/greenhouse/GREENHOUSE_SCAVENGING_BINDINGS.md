# GREENHOUSE SCAVENGING BINDINGS (plan §39-41)

Three additions to `table_loot_greenhouse` ("Botanical Cultivar Nursery &
Hydroponic Trays", `location_type: greenhouse`) in
`Assets/StreamingAssets/Data/scavenging_tables.json` — the Plan 46 weighted-
table authority. No legacy destination loot lists were touched; Plan 76
agricultural destinations inherit these through the table runtime.

| Added entry | weight | qty | rarity | Rationale (plan §40) |
|---|---:|---|---|---|
| `item_greenhouse_glass_pane` | 10 | 1 | uncommon | bulky, breakable — uncommon but findable |
| `item_greenhouse_uv_sheeting` | 10 | 1–2 | uncommon | the classic salvage yield from a derelict tunnel |
| `item_greenhouse_drip_kit` | 6 | 1 | rare | the highest-value prize of the nursery |

Table context after the change (13 entries): seed_packets 45, chemicals 35,
clean_water 25, wood_block 15, fungicide_fogger 12, air_filter_hepa 12,
**glass_pane 10, uv_sheeting 10**, ro_membrane 10, **drip_kit 6**,
cassette tapes 8×3, map fragments. Weighted probability of a Plan 91 item
per hit ≈ 8.5% — rare enough to feel found, common enough to matter.

## Schema compliance

- Entries use the current weighted schema: `{item_id, weight, min_quantity,
  max_quantity, rarity_tier}` — no new fields invented.
- Rarity tiers restricted to the existing `common`/`uncommon`/`rare` set.
- All three `item_id`s resolve in the global item registry (pinned by
  `Scavenging_BoundItemIdsResolveInGlobalRegistry`, which also documents the
  deliberate empty-`item_id` + `map_fragment_id` map-reward pattern).
- `table_loot_farm` and `table_loot_warehouse` were left untouched: the
  nursery table is the correct single home for greenhouse-specific
  equipment (plan §39 suggests exactly 3 bindings).

## Verification hook

Deterministic resolution is covered by
`GreenhouseItemCatalogTests.Scavenging_GreenhouseTableBindsThreePlan91Items`
and `Scavenging_BoundEntriesUseSaneWeightsAndRarity` (data-level); runtime
rolling behavior is owned by the existing Plan 46 scavenging runtime and was
not modified.
