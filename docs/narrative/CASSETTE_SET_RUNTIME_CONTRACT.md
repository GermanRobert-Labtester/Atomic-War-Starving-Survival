# Cassette Set Runtime Contract & Schema Analysis

## 1. Executive Summary

This document establishes the verified runtime architecture, catalog schemas, item linkages, and completion hooks for multi-part cassette narratives in ASHFALL as audited for **Plan 67**.

---

## 2. Catalog Schema (`cassette_sets.json`)

The authoritative schema for `Assets/StreamingAssets/Data/cassette_sets.json` is a root container object:

```json
{
  "schema_version": 1,
  "items": [
    {
      "set_id": "string",
      "set_title": "string",
      "total_parts": 4,
      "parts": [
        {
          "part": 1,
          "item_id": "cassette_<set_id>_<part>",
          "title": "string",
          "description": "string"
        }
      ],
      "hidden_cache_location": "location_id",
      "hidden_cache_items": [
        "item_id_1",
        "item_id_2"
      ],
      "completion_narrative": "narrative_cassette_<set_id>_complete"
    }
  ]
}
```

### Key Structural Invariants
1. **Root Shape:** Object with `schema_version` (int) and `items` (array).
2. **Set Identification:** `set_id` (snake_case string, e.g. `checkpoint_kilo`).
3. **Set Display:** `set_title` (evocative display title in English).
4. **Part Count:** `total_parts` (int, strictly matching `parts.Length`).
5. **Part Item Key:** `item_id` using the established prefix `cassette_<set_id>_<part>`.
6. **Part Sequence:** `part` (1-indexed explicit integer: 1..N).
7. **Transcript Field:** `description` stores the audio transcript / diegetic monologue.
8. **Cache Location:** `hidden_cache_location` references a valid location in `locations.json` (enforced as a Tier-2 reference key in `CatalogIntegrityValidator`).
9. **Cache Rewards:** `hidden_cache_items` references valid item IDs in `items.json`.
10. **Completion Narrative:** `completion_narrative` references a valid narrative event ID in `events.json`.

---

## 3. Item Integration Architecture

In ASHFALL, magnetic media exists in two layers:
1. **General Media Item:** `item_cassette_tape` in `items.json` represents generic blank / unlabelled tapes used in crafting and standard trading.
2. **Authored Cassette Part Items:** Each specific part has an `item_id` defined in `cassette_sets.json` with prefix `cassette_`. When registered in `items.json`, each part functions as a physical, unique collectible artifact with weight 0.1 kg, stackMax 1, and `Media` type.
3. **Prefix Registry:** `cassette_` is a registered Tier-1 snake_case prefix in `CatalogIntegrityValidator.cs`, guaranteeing reference validation across all data catalogs.

---

## 4. Scavenging & Location Wiring

Scavenging tables in `Assets/StreamingAssets/Data/scavenging_tables.json` populate loot for locations. Each table defines entries:

```json
{
  "item_id": "cassette_<set_id>_<part>",
  "weight": 8,
  "min_quantity": 1,
  "max_quantity": 1,
  "rarity_tier": "rare"
}
```

Cassette parts are distributed across corresponding thematic location tables (e.g. medical tapes in `table_loot_hospital`, train logs in `table_loot_rail_yard`, school tapes in `table_loot_school`).

---

## 5. Narrative Completion & Save Model

- **Trigger:** Finding and listening to all parts of a set reveals the set's `hidden_cache_location` and triggers the associated `completion_narrative` event in `events.json`.
- **Idempotence:** Completion flags prevent duplicate narrative triggers and repeated cache rewards.
- **Persistence:** Save envelope preserves discovered cassettes, listened tape markers, and completed narrative IDs across sessions.
