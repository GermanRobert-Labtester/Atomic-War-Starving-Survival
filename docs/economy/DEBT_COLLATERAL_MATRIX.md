# Plan 40 — Collateral Contract

## Collateral Model
The `forfeit` field on `DebtContract` is a **string** (named good), not a structured collateral object. This is intentional — the runtime models forfeit as a description, not an inventory reservation.

## Template Forfeit Descriptions
| Template | Forfeit |
|---|---|
| supply_corps_rations | eight tins of sealed rations from the shelter stores |
| supply_corps_fuel | six jerrycans of refined fuel from the shelter reserve |
| supply_corps_medical | three field medical kits from the Corps dispensary |
| hydro_barons_water | twelve litres of purified water from the Baron's cistern |
| hydro_barons_filter | two ceramic water filtration units from the Baron's workshop |
| hydro_barons_purification | two packs of water purification tablets from the Baron's pharmacy |
| railway_guild_fuel | ten litres of diesel from the Guild's depot |
| railway_guild_parts | fifteen salvaged mechanical components from the Guild's machine shop |
| railway_guild_transport | one salvaged engine from the Guild's reserve stock |
| ordnance_foundry_ammo | forty rounds of 7.62mm from the Foundry's production line |
| ordnance_foundry_tools | two soldering kits from the Foundry's tool crib |
| ordnance_foundry_armor | three gas masks from the Foundry's protective equipment stores |
| scavengers_food | fifteen packs of dried rations from the Scavenger cache |
| scavengers_medicine | five doses of antibiotics from the Scavenger's black-market stash |
| scavengers_equipment | two dosimeters from the Scavenger's salvage pile |

## Seizure Behavior
- `conseq_collateral_seizure` fires `OnCollateralSeizure` event
- Seizure is one-shot (keyed by `debtorId:consequenceId`)
- No duplicate seizure possible
- Collateral return on repayment is handled by the host layer
