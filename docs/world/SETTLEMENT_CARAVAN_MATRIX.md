# Settlement Caravan Integration Matrix

## 1. Caravan Network Endpoints

| Caravan ID | Caravan Name | Faction | Key Settlement Endpoints |
|---|---|---|---|
| `caravan_flotilla_salt_run` | Salt & Saline Flotilla Convoy | `faction_the_fleet` | `loc_settlement_cape_beacon`, `loc_settlement_brine_pans` |
| `caravan_verge_grain_convoy` | Verge Agricultural Hauler | `faction_rebuilders` | `loc_settlement_silo_burrow` |
| `caravan_foundry_coal_iron` | Foundry Iron & Coal Column | `faction_silent_foundry` | `loc_settlement_iron_siding`, `loc_settlement_nine_rails` |
| `caravan_free_trader_circuit` | Scale Free-Trader Circuit | `faction_the_scale` | `loc_settlement_tinkers_notch`, `loc_settlement_pilgrim_hearth`, `loc_settlement_ferry_crossing` |

## 2. Validation
- All 4 active caravans include at least one canonical settlement in their `route_node_ids`.
- Across all caravans, 7 distinct settlement endpoints are actively serviced by trade convoys.
