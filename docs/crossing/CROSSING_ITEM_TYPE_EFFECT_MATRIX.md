# Plan 126 Type and Effect Matrix

| ID | Type | Stack | Weight | Value | Hunger | Thirst | Health | Morale | Semantic status |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| `item_arbitration_token` | Quest | 1 | 0.05 | 12 | 0 | 0 | 0 | 0 | hearing-access document |
| `item_charter_stamp` | Quest | 1 | 0.05 | 18 | 0 | 0 | 0 | 0 | validation instrument |
| `item_weighbridge_chit` | Trade | 10 | 0.01 | 6 | 0 | 0 | 0 | 0 | transferable weight record |
| `item_smuggled_medicine` | Medical | 5 | 0.1 | 24 | 0 | 0 | 20 | 0 | H0: canonical global health effect |
| `item_crossing_bread` | Food | 15 | 0.2 | 8 | 22 | 0 | 0 | 1 | staple parity with flatbread |
| `item_lamp_oil_crossing` | Fuel | 5 | 1.0 | 6 | 0 | 0 | 0 | 0 | L2: no Crossing lamp consumer proven |
| `item_filtered_water_crossing` | Water | 5 | 1.0 | 12 | 0 | 40 | 0 | 0 | canonical water use |
| `item_quarantine_bands` | Quest | 10 | 0.01 | 4 | 0 | 0 | 0 | 0 | status marker |
| `item_granary_receipt` | Trade | 5 | 0.01 | 8 | 0 | 0 | 0 | 0 | grain claim |
| `item_smugglers_ledger` | Quest | 1 | 0.1 | 15 | 0 | 0 | 0 | 0 | evidence |
| `item_rejection_notice` | Quest | 1 | 0.01 | 3 | 0 | 0 | 0 | 0 | failed-claim evidence |
| `item_crossing_map` | Quest | 1 | 0.05 | 12 | 0 | 0 | 0 | 0 | information item; no map runtime |
| `item_black_market_pouch` | Trade | 1 | 0.2 | 8 | 0 | 0 | 0 | 0 | trade object; no container runtime |
| `item_charter_draft` | Quest | 1 | 0.05 | 20 | 0 | 0 | 0 | 0 | political document |

## Special gates

- Smuggled Medicine is H0: the global inventory DTO has `healthEffect`, and the generic inventory use path applies it. No medical procedure or new treatment system was added.
- Lamp Oil is L2: `Fuel` is a valid canonical type, but no live Crossing-specific lamp/fuel consumer was found. The item is therefore descriptive/trade content until an existing fuel authority references it.
- Crossing Map and Off-Ledger Pouch have no hidden travel, stealth, concealment, or capacity effects.
