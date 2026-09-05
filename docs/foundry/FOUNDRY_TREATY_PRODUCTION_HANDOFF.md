# Foundry Treaty Production Handoff Contract

**Target System:** `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`
**Catalog Authority:** `Assets/StreamingAssets/Data/foundry_production.json`

---

## 1. Treaty-to-Production Coupling

The Silent Foundry smelter bay (`room_bp_11_the_silent_foundry_smelter_bay`) casts products required by the accords:
- `item_foundry_brine_pipe` (4 units per cycle required by `treaty_brine_pipe_and_iodine_exchange`)
- `item_foundry_ice_anchor` (60 units per cycle required by `treaty_road_iron_charter`)
- `item_foundry_winch_drum` (3 units per cycle required by `treaty_road_iron_charter`)

---

## 2. Feedback Loops

When a treaty consequence fires:
1. **Demand & Value Adjustments:**
   - Quota Met: Demand delta decreases (`-0.4` for brine pipe, `-0.3` for ice anchor). Surplus clears at normal pricing without gluts.
   - Quota Missed: Demand delta increases (`+0.4` for brine pipe, `+0.3` for ice anchor). Stock sits unsold or sells at deep discount outside the accord quota window.

2. **Input Material Costs:**
   - Heats require `coal`, `fuel`, and `scrap_metal`.
   - Missed or violated treaties increase the demand delta of coal (e.g. `+0.15` to `+0.20`) and fuel (e.g. `+0.25` to `+0.50`).
   - Consequently, running the cupola becomes more expensive when external treaties are neglected or breached.

3. **Production State Invariant:**
   - Consequence policies never inject synthetic production halts or corrupt `FoundryState`.
   - The cupola remains fully operational, but raw material costs escalate and treaty fulfillment stalls until obligations are addressed.
