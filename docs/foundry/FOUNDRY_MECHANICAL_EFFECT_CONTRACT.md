# Foundry Mechanical Effect Contract

**Authority File:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`
**Host Hook:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Mechanical Effect Grammar

Consequences in `foundry_treaty_consequences.json` are authored through two system-owned mechanical channels:

1. `standing_delta` (Numeric Float):
   - Applied to the signatory faction's trust level in `FactionStanceEngine` via `ModifyTrust(faction_id, standing_delta)`.
   - Modifies `SilentFoundryConsequenceState.guildStanding` clamped to `[-100.0, +100.0]`.
   - Modulates trade stance thresholds:
     - `ShareIntel`: Trust $\ge +40$
     - `Trade`: Trust $\ge -20$ (trade stall open)
     - `Rob`: Trust $< -20$ (trade stall blocked)
     - `HostileRaid`: Trust $\le -50$ (armed hostility)

2. `market_modifiers` (Array of Good Demand Modifiers):
   - Applied to the active `MarketSystem` via `market.AdjustDemand(good_id, demand_delta)`.
   - Bound to standard market multiplier clamps: `[MarketSystem.MinDemandMult, MarketSystem.MaxDemandMult]`.
   - Increases price/scarcity when `demand_delta > 0`; decreases price/eases availability when `demand_delta < 0`.

---

## 2. Supported Goods Vocabulary

Every `good_id` declared in `market_modifiers` MUST resolve in `Assets/StreamingAssets/Data/economy_goods.json`.

In the 15-policy catalog, the goods referenced are:

| Good ID | Category | Base Price | Catalog Defined? |
|---|---|---|---|
| `item_foundry_brine_pipe` | materials | 70.0 | Yes (`economy_goods.json` + `foundry_items.json`) |
| `item_foundry_ice_anchor` | materials | 2.0 | Yes (`economy_goods.json` + `foundry_items.json`) |
| `coal` | fuel | 12.0 | Yes (`economy_goods.json`) |
| `fuel` | fuel | 20.0 | Yes (`economy_goods.json`) |
| `clean_water` | water | 8.0 | Yes (`economy_goods.json` + `items.json`) |
| `canned_food` | food | 16.0 | Yes (`economy_goods.json` + `items.json`) |
| `water_filter` | tools | 35.0 | Yes (`economy_goods.json` + `items.json`) |
| `scrap_metal` | materials | 3.0 | Yes (`economy_goods.json` + `items.json`) |

---

## 3. Strict Prohibitions

- **No Invented Top-Level Fields:** The schema does not support fields like `water_delta`, `access_unlock`, `contamination_rise`, or `spawn_raid`.
- **No Direct Inventory Mutators in Policy:** Consequences modify market prices and demand pressures; direct shelter inventory mutation is governed by production and quest actions, not raw policy rows.
- **No Unregistered Goods:** Any good ID not present in `economy_goods.json` causes a test failure and warning in `SilentFoundryHostSession`.
