# Production Trade Flow & Regional Economy Integration

This document outlines regional price curves, opportunity costs, and export trade flows for cast foundry goods, agricultural crops, salt, and apiculture products.

---

## 1. Regional Export Commodities

| Commodity Item ID | Base Value | Top Regional Buyer | Regional Trade Dynamic | Opportunity Cost vs Internal Use |
|---|---|---|---|---|
| `item_foundry_plowshare` | 45 | Grain Exchange / Verge Allotments | Premium barter rate for rare cultivar seeds | Reduces shelter greenhouse tilling efficiency |
| `item_foundry_winch_drum` | 90 | The Fleet / Berth 9 | Trades for marine diesel and heavy salvage | Owed to road charter; defaulting causes haulage tariffs |
| `item_foundry_alloy_part` | 120 | Hydro-Barons / Power Stations | Trades for high-yield power cells and filters | Critical spare for shelter generator overhaul |
| `item_trade_salt_sack` | 35 | Inland Agrarian Settlements | Universal barter medium; immune to inflation | Consumes mined salt needed for food preservation |
| `item_honey_pot` | 25 | Dispensaries / Pilgrim Caravans | High luxury demand; traded for surgical tools | Sacrifices shelter survivor morale and wound salves |
| `item_beeswax_block` | 18 | Workshop Guilds / Navigational Scribes | Used in waterproof canvas and cartridge seals | Needed for shelter pipe sealant and candle casting |
| `food_canned_grain_stew` | 16 | Frontier Scavenger Outposts | Long shelf-life emergency rations | Depletes emergency winter famine reserves |

---

## 2. Market Anti-Arbitrage Rules

1. **Scrap vs Cast Spread**: Scrap costs + fuel + labor exceed the immediate sell price of raw ingots. Profit is earned only through specialized high-skill precision castings (`item_foundry_alloy_part`, `item_foundry_valve_body`).
2. **Transportation Weight**: Heavy structural castings (e.g. `item_foundry_t_beam` @ 40kg, `item_foundry_winch_drum` @ 75kg) require vehicle transport or caravan capacity, preventing lightweight infinite scavenging runs.
