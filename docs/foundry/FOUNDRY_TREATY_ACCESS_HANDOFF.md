# Foundry Treaty Route & Facility Access Handoff Contract

**Target Systems:** `Assets/Ashfall.Core/Expeditions/`, `Assets/Ashfall.Core/Legacy/FactionStanceEngine.cs`
**Host Hook:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Access Mechanics

In ASHFALL, route and facility access are mediated through:
1. **Faction Trade Stalls:** Directly gated by `FactionStanceEngine.GetStance(factionId)`. If trust drops below `-20.0` due to treaty penalties, the faction's merchant stops trading.
2. **Expedition Transit Corridors:** Regulated by destination waypoint flags and regional travel speed modifiers.
3. **Institutional Facilities:** Gated by faction alliance flags.

---

## 2. Treaty-Specific Route & Access Effects

| Treaty ID | Outcome | Route / Corridor Affected | Access Consequence in Lore & Host Sessions |
|---|---|---|---|
| `treaty_road_iron_charter` | `missed` | The Cut / Road Iron Haulage Lane | Haulage column prioritizes Cutters transport; foundry lane suspended until arrears clear. |
| `treaty_flotilla_saline_corridor_concordat` | `missed` | Lock Gate Four / Shallows Channel | Lock Gate Four closed during low high-tide; coastal vessels forced into holding anchorages. |
| `treaty_switchback_fuel_and_passage_accord` | `violated` | The Switchback Waystation & Snowline Pass | Pass sealed with rockfalls; pilgrim guide privileges revoked; mountain transit severed. |
| `treaty_deep_coast_aquifer_protection_treaty` | `violated` | Pump Station Nine & Deep Coast Docking | Maritime docking rights revoked at coastal pump wharf due to bilge discharge violation. |
| `treaty_garrison_grain_tithe_compact` | `violated` | Eastern Arterial Road & Checkpoint Gamma | Checkpoint Gamma sealed to civilian carts; armed escort suspended; contraband seized. |
| `treaty_scale_suburban_fair_trade_convention` | `met` | The Caravanserai & Grange Hall | Verified bronze balance weights recognized; two-percent standard fee applied across triangle. |

---

## 3. Host State Representation

Access states are rendered in `src/UI/FactionsPanel.cs` and `src/UI/RegionalTreatiesPanel.cs` via:
- Route Status Badges (`OPEN`, `RESTRICTED`, `SEALED`).
- Tooltips citing the exact treaty consequence `reason` string from `foundry_treaty_consequences.json`.
