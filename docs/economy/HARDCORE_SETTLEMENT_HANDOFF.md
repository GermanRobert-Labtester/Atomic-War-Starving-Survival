# Hardcore Settlement Handoff

## 1. Settlement Market Alignment

Settlement trading hubs map directly to the 8 faction profiles:

| Settlement ID | Controlling Faction | Applied Trade Profile | Trade Currency Used |
|:---|:---|:---|:---|
| `settlement_fort_iron` | `central_garrison_remnants` | `central_garrison_remnants` | Fuel vouchers, ammunition |
| `settlement_anchorage` | `faction_black_flotilla` | `faction_black_flotilla` | Dry cloth, stamped salvage |
| `settlement_pumping_station_four` | `faction_the_scale` | `faction_the_scale` | Volumetric water chits |
| `settlement_town_hall` | `faction_the_compact` | `faction_the_compact` | Archival deeds, arbitration warrants |
| `settlement_rail_exchange` | `faction_the_underwrite` | `faction_the_underwrite` | Fuel debentures, risk contracts |
| `settlement_cut_depot` | `faction_the_cutters` | `faction_the_cutters` | Black coal, sledge transit chits |
| `settlement_green_terrace` | `faction_the_rebuilders` | `faction_the_rebuilders` | Grain bushels, seed credits |
| `settlement_survey_trig` | `faction_the_overlay` | `faction_the_overlay` | Cadastral keys, survey warrants |

### Stock Refresh & Scarcity Coupling
- At daily roll (`TickDay`), settlements restock goods modulated by their governing scarcity tier.
- If an item is declared in `Refuses`, settlement vendors never seed it into initial stock pools.
