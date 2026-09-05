# Foundry Treaty Resource & Economy Handoff Contract

**Target System:** `Assets/Ashfall.Core/Economy/MarketSystem.cs`
**Host Dispatcher:** `src/Foundry/SilentFoundryHostSession.cs`

---

## 1. Resource Mutation Architecture

Consequence policies **never directly inject or confiscate items from the player's shelter inventory**. Doing so would bypass inventory capacity checks, event logs, and crafting reservation states.

Instead, policies alter regional market scarcity and price pressure via `market_modifiers`:
```csharp
if (_market != null && record.modifiers != null)
{
    for (int i = 0; i < record.modifiers.Count; i++)
    {
        var m = record.modifiers[i];
        if (m == null || string.IsNullOrEmpty(m.good_id)) continue;
        _market.AdjustDemand(m.good_id, m.demand_delta);
    }
}
```

---

## 2. Market Impact Distribution

| Good ID | Impacted by Treaties | Demand Deltas (Met / Missed / Violated) | Economic Consequence |
|---|---|---|---|
| `coal` | `brine_pipe`, `road_iron` | `-0.15` to `-0.20` (Met)<br>`+0.15` to `+0.20` (Missed) | Regulates fuel cost for running cupola heats. |
| `fuel` | `labour_schedule`, `saline_corridor`, `switchback_fuel`, `grain_tithe` | `-0.20` to `-0.30` (Met)<br>`+0.25` to `+0.35` (Missed)<br>`+0.25` to `+0.50` (Violated) | Controls generator run-time and transport costs across sectors. |
| `clean_water` | `saline_corridor`, `aquifer_protection` | `-0.25` to `-0.35` (Met)<br>`+0.80` (Violated) | Direct indicator of drinking water availability and desal throughput. |
| `canned_food` | `grain_tithe` | `-0.30` (Met)<br>`+0.60` (Violated) | Drives hunger mitigation prices during Verge road blockades. |
| `water_filter` | `aquifer_protection` | `+0.50` (Violated) | Spikes filter replacement prices when intake screens are fouled. |
| `scrap_metal` | `fair_trade_convention` | `-0.15` (Met) | Relieves basic construction and repair material costs. |
| `item_foundry_brine_pipe` | `brine_pipe` | `-0.40` (Met)<br>`+0.40` (Missed) | Clears pipe surplus or chokes pipe stock. |
| `item_foundry_ice_anchor` | `road_iron` | `-0.30` (Met)<br>`+0.30` (Missed) | Maintains ice road transit hardware clearance. |

All demand adjustments respect `MarketSystem.MinDemandMult` ($0.5$) and `MarketSystem.MaxDemandMult` ($3.0$), preventing runaway price infinity or zero-cost exploits.
