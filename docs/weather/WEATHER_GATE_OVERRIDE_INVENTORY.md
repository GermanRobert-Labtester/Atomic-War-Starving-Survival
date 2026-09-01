# Plan 48 — Weather Gate Override Inventory

## Override Items Used

| Item ID | Display Name | Gates Using | Type |
|---|---|---|---|
| gas_mask | Gas Mask | gate_lowland_marsh_fog, gate_industrial_valley_fog | Respiratory protection |
| hazmat_suit | Hazmat Suit | gate_open_wasteland_fallout, gate_exposed_highway_fallout | Radiation protection |

## Override Coverage

- 4 of 15 gates have overrides (27%)
- 11 gates have no override — weather forces waiting or rerouting
- All override items exist in `items.json` with valid canonical IDs

## Override Semantics

Overrides are **protective equipment** that makes hazardous-but-not-impassable routes traversable. They do not make physically impossible routes passable (a flooded underpass cannot be bypassed by a gas mask).

## No Skill Overrides

No Plan 33 skill IDs are referenced. All overrides use item possession/equipment checks.

## No Force Passage

Force passage is **not supported** by the runtime. The `consequence_on_force` field is descriptive text for future integration only.
