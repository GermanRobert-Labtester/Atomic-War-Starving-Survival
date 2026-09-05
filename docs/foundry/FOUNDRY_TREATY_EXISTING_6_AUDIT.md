# Audit of the Existing Six Baseline Consequence Policies

**File:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**Baseline Policies Count:** 6
**Status:** Audited and preserved byte-for-byte in Plan 103 expansion.

---

## 1. Inventory of Baseline Entries

| # | Treaty ID | Outcome | Faction | Standing Delta | Good Modifiers |
|---|---|---|---|---|---|
| 1 | `treaty_brine_pipe_and_iodine_exchange` | `met` | `faction_silent_foundry` | `+2.0` | `item_foundry_brine_pipe` (-0.4), `coal` (-0.15) |
| 2 | `treaty_brine_pipe_and_iodine_exchange` | `missed` | `faction_silent_foundry` | `-6.0` | `item_foundry_brine_pipe` (+0.4), `coal` (+0.15) |
| 3 | `treaty_cluster_labour_schedule` | `met` | `faction_silent_foundry` | `+2.0` | `fuel` (-0.25) |
| 4 | `treaty_cluster_labour_schedule` | `violated` | `faction_silent_foundry` | `-8.0` | `fuel` (+0.25) |
| 5 | `treaty_road_iron_charter` | `met` | `faction_silent_foundry` | `+3.0` | `coal` (-0.2), `item_foundry_ice_anchor` (-0.3) |
| 6 | `treaty_road_iron_charter` | `missed` | `faction_silent_foundry` | `-6.0` | `coal` (+0.2), `item_foundry_ice_anchor` (+0.3) |

---

## 2. Design Patterns Observed

1. **Symmetric Market Relief vs. Penalty:**
   - For quota-based treaties (`brine_pipe`, `road_iron`, `cluster_labour`), the `met` outcome relieves market demand by the exact magnitude that the `missed` or `violated` outcome penalizes.
   - Pinned by regression test `SilentFoundryConsequenceTests.Policy_MetReliefRowsMirrorTheMissPenalties`.
   - Prevents permanent economic drift over repeated multi-cycle campaigns.

2. **Proportional Standing Magnitude:**
   - Met: `+2.0` to `+3.0`.
   - Missed: `-6.0`.
   - Violated: `-8.0`.
   - Clear distinction: active violation carries greater penalty than passive shortfall.

3. **Institutional Tone:**
   - Reasons explain tangible operational results ("the Office releases iodine", "re-opens the accident book", "scrambles for replacement heat").
   - Free of melodrama; grounded in wasteland industrial logistics.

4. **Integration Surface:**
   - Modifiers interface cleanly with `MarketSystem.AdjustDemand(good_id, delta)` and `FactionStanceEngine.ModifyTrust(faction_id, delta)`.
