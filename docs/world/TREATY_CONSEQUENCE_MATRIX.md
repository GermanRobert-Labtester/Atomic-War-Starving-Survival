# Treaty Consequence & Escalation Matrix

**Authority Catalog:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**System Coordinator:** `Assets/Ashfall.Core/RegionalTreatySystem.cs` / `Assets/Ashfall.Core/FactionWar/FactionWarSystem.cs`

---

## 1. Multi-Stage Breach Escalation Protocol

When treaty conditions fail or compliance drops below critical thresholds, diplomatic relations progress through defined escalation stages:

```
[ Active Compliance (Score 1.0) ]
              │
              ▼ (Missed delivery / minor trespass)
[ Formal Warning & Standing Drop (-5 to -8) ]
              │
              ▼ (Continued default / tariff withholding)
[ Commodity Embargo & Route Toll Surcharge (+50–100% prices) ]
              │
              ▼ (Armed incursion / sabotaged infrastructure)
[ Treaty Collapse, Border Closure & Faction War Escalation ]
```

---

## 2. Consequence & Market Impact Policies

| Treaty ID | Outcome | Standing Impact | Primary Economic / Market Consequence |
|---|---|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | `met` | +2.0 (Silent Foundry) | Mechanical scrap & coal demand ease (-0.4, -0.15) |
| `treaty_brine_pipe_and_iodine_exchange` | `missed` | -6.0 (Silent Foundry) | Pipe stock sits unsold; coal prices rise (+0.15) |
| `treaty_cluster_labour_schedule` | `met` | +2.0 (Silent Foundry) | Fuel allotments released normally (-0.25) |
| `treaty_cluster_labour_schedule` | `violated` | -8.0 (Silent Foundry) | Coal window closed; emergency heating fuel spikes (+0.25) |
| `treaty_road_iron_charter` | `met` | +3.0 (Silent Foundry) | Road anchors cleared; coal columns run smooth (-0.3) |
| `treaty_road_iron_charter` | `missed` | -6.0 (Silent Foundry) | Foundry lane closed; transport coal reprioritized (+0.2) |
| `treaty_garrison_grain_tithe_compact` | `met` | +4.0 (Central Garrison) | Stable grain flow; ration prices drop (-0.3) |
| `treaty_garrison_grain_tithe_compact` | `violated` | -12.0 (Central Garrison) | Eastern Road blockaded; ration prices soar (+0.6), fuel (+0.4) |
| `treaty_flotilla_saline_corridor_concordat` | `met` | +3.0 (The Fleet) | Free lock passage; diesel & clean water stabilize (-0.2, -0.25) |
| `treaty_flotilla_saline_corridor_concordat` | `missed` | -5.0 (The Fleet) | Lock Gate Four closed; idle vessel fuel consumption (+0.35) |
| `treaty_switchback_fuel_and_passage_accord` | `met` | +4.0 (Ash Sign) | High Scarp pass open; paraffin & fuel stable (-0.3) |
| `treaty_switchback_fuel_and_passage_accord` | `violated` | -10.0 (Ash Sign) | Switchback sealed with rockfall; mountain fuel spikes (+0.5) |
| `treaty_scale_suburban_fair_trade_convention` | `met` | +3.0 (The Scale) | Standard trading weights; transaction friction drops (-0.15) |
| `treaty_scale_suburban_fair_trade_convention` | `violated` | -8.0 (The Scale) | Caravanserai closed; medical supplies restricted (+0.45) |
| `treaty_foundry_scrap_salvage_demarcation` | `met` | +3.0 (Silent Foundry) | Steel scrap supply steady; agricultural iron cast (-0.25) |
| `treaty_foundry_scrap_salvage_demarcation` | `violated` | -7.0 (Silent Foundry) | Replacement casting suspended; mechanical scrap dear (+0.3) |
| `treaty_roster_border_demilitarization_pact` | `met` | +5.0 (Forward Roster) | Buffer observed; defensive ammo hoarding drops (-0.2) |
| `treaty_roster_border_demilitarization_pact` | `violated` | -15.0 (Forward Roster) | Border skirmishing erupts; combat ammo demand surges (+0.7) |
| `treaty_deep_coast_aquifer_protection_treaty` | `met` | +3.0 (The Fleet) | Freshwater intake safe; clean water price eases (-0.35) |
| `treaty_deep_coast_aquifer_protection_treaty` | `violated` | -10.0 (The Fleet) | Intake brine contamination; clean water crisis (+0.8), filters (+0.5) |
| `treaty_high_scarp_observatory_sanctuary` | `met` | +4.0 (Ash Sign) | Weather telemetry broadcast; scavenger losses drop (-0.2) |
| `treaty_high_scarp_observatory_sanctuary` | `violated` | -9.0 (Ash Sign) | Telemetry arrays vandalized; electronics demand rises (+0.4) |
