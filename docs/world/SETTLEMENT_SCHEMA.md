# Settlement Schema Contract

## 1. JSON Schema Definition

```json
{
  "id": "settlement_example",
  "display_name": "Example Settlement",
  "archetype": "Trade Post | Faction Stronghold | Refugee Camp | Religious / Ideological Community",
  "region": "the_toll | industrial_belt | high_scarp | coastal_shelf | the_cluster | dead_suburbs | the_verge | the_drown",
  "location_id": "loc_settlement_example",
  "location_link": "loc_settlement_example",
  "population": 85,
  "allegiance": "faction_example",
  "threat_level": 3,
  "attitude": "friendly | neutral | wary | hostile",
  "description": "2-3 sentences of grounded environmental storytelling.",
  "trade_goods": [
    "item_export_1",
    "item_export_2"
  ],
  "trade_needs": [
    "item_import_1",
    "item_import_2"
  ],
  "economy": {
    "primary_export": "item_export_1",
    "primary_import": "item_import_1",
    "trade_specialty": "specialty_tag",
    "price_modifier_exports": 0.75,
    "price_modifier_imports": 1.30,
    "stock_item_ids": ["item_export_1", "item_export_2"]
  },
  "society": {
    "governance": "Governance Council",
    "population": 85,
    "core_value": "Core community value",
    "internal_tension": "Current internal struggle"
  },
  "faction_relation": {
    "primary_faction": "faction_example",
    "standing_gate_faction": "faction_example",
    "min_standing_to_enter": 0,
    "hostile_standing_threshold": -40
  }
}
```

## 2. Field Status Mapping

| Field | Status | Consumer |
|---|---|---|
| `id` | LIVE | `SettlementCatalog`, `CatalogIntegrityValidator` |
| `display_name` | LIVE | UI Panels, Tooltips, Map View |
| `archetype` | VALIDATED_METADATA | Filtering, Future Territory (Plan 44) |
| `region` | LIVE | Regional Weather, Travel Calculators |
| `location_link` / `location_id` | LIVE | `locations.json`, `caravans.json`, `expeditions.json` |
| `population` | VALIDATED_METADATA | Scale, Future Defense/Consumption |
| `allegiance` | LIVE | Faction Standing Gate, Future Territory |
| `threat_level` | VALIDATED_METADATA | Patrol Generation (Plan 45), Raid Risk |
| `attitude` | VALIDATED_METADATA | Initial Stance, Greeting Trees |
| `trade_goods` / `trade_needs` | LIVE | Caravan Inventory Generation, Barter Modifiers |
