# Plan 46 ↔ Plan 85 Fragment Reconciliation

Plan 46's scavenging authority (`scavenging_tables.json`, 49 location-typed tables at baseline) is the fragment producer layer. No fragment entries existed before Plan 85 — this is the initial wiring, not a rebalance.

## Table semantics observed (§3.8)

- Entries are weighted (`weight`, 0 excluded from `TotalWeight`); `RollLoot` is a deterministic cumulative pick over `ISeededRng`.
- `depletion_model` (`finite` / `renewable` / `one_time` / `slow_regeneration`) exists in data; **no runtime tracks per-site depletion today** (pre-existing gap, recorded — not Plan 85 scope). Fragments therefore use repeatable sources.
- Non-item reward channel: `codex_unlock_id` exists (vocabulary key, unconsumed). Plan 85 added the parallel, consumed channel `map_fragment_id` instead of overloading it.

## Wiring decisions

- **Entry shape:** `{ item_id: "", weight: 2, min_quantity: 1, max_quantity: 1, rarity_tier: "rare", map_fragment_id: "<fragment_id>" }` — a fragment-only entry yields no physical loot line (§85C.4 anti-farm rule).
- **Validator extension (net stricter):** `map_fragment_id` added to the integrity `ReferenceKeys` — every fragment token must resolve to a registered `fragment_id` from `damaged_map_zones.json` (negative probe confirmed the gate fires: bogus ids fail `--data-integrity-selftest`). The existing table-integrity test was extended in the same spirit: an entry with empty `item_id` MUST carry a resolvable `map_fragment_id`.
- **Location-type affinity (§85C.2):** medical fragments in hospital/clinic/fire_station tables; court fragments in police/archive/printworks; farm fragment in the farm table; forestry in forestry_compound; metro in metro_station/power_substation; military in military_depot/checkpoint; crater in dead_hand_core/government_bunker; coast in tank_farm/relay_mast. **23 of 49 tables** now carry fragment entries.
- **Plan 46 tables are now live in the host:** `ExpeditionHostSession.Create` binds `Engine.ScavengingCatalog` (previously test-only). Destinations without a `scavenging_table_id` keep the legacy `lootCategories` fallback path unchanged.
- **Minimum-two requirement:** all 6 new zones (and all 6 original zones) are wired — far above the "at least two zones" floor.

## Full fragment → table mapping

See `FRAGMENT_ACQUISITION_MATRIX.md` (authoritative, test-pinned by `Catalog_EveryFragment_HasScavengingProducer`).
