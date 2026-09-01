# Regional Treaty Web & Accords

**Authority Catalog:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Consequence Catalog:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
**System Coordinator:** `Assets/Ashfall.Core/RegionalTreatySystem.cs`

---

## 1. Treaty Web Architecture

The diplomatic fabric of the ASHFALL wasteland is held together by 12 interrelated accords binding factions into mutual dependencies, trade tariffs, territorial demilitarization zones, and resource allocations.

```
       [ faction_silent_foundry ] ◄───(Road Iron Charter)───► [ faction_the_cutters ]
                   ▲                                                   ▲
                   │ (Brine Pipe & Iodine)                             │ (Saline Corridor)
                   ▼                                                   ▼
       [ faction_the_office ]                                [ faction_the_fleet ]
                   ▲                                                   ▲
                   │ (Fair Trade)                                      │ (Aquifer Protection)
                   ▼                                                   ▼
       [ faction_the_scale ] ◄───(Grain Tithe & Security)───► [ faction_rebuilders ]
                   ▲                                                   ▲
                   │                                                   │
                   ▼                                                   ▼
       [ faction_central_garrison ] ◄──(Demilitarization)──► [ faction_forward_roster ]
                   ▲                                                   ▲
                   │                                                   │ (Switchback Fuel)
                   └───────────► [ faction_ash_sign ] ◄────────────────┘
```

---

## 2. Comprehensive 12-Treaty Index

| Index | Treaty ID | Title | Signatory Factions | Territory Demarcation | Primary Resource / Obligation |
|---|---|---|---|---|---|
| 1 | `treaty_brine_pipe_and_iodine_exchange` | The Brine Pipe & Iodine Exchange | Silent Foundry, The Office | Smelter floor to saltworks hall | Lead pipes for medical iodine |
| 2 | `treaty_cluster_labour_schedule` | The Cluster Labour Schedule | Silent Foundry, The Office, Cutters | Charging floor & Cluster school | 8-hour shifts & boil orders |
| 3 | `treaty_road_iron_charter` | The Road Iron Charter | Silent Foundry, Cutters, The Fleet | Casting floor to The Cut | Ice anchors & coal haulage |
| 4 | `treaty_the_cluster_charter` | The Cluster Charter | Foundry, Office, Cutters, Fleet | Smelter bay & district schedule | Standing status & shift registry |
| 5 | `treaty_garrison_grain_tithe_compact` | The Garrison Grain Tithe Compact | Central Garrison, Rebuilders | The Verge & Checkpoint Gamma | Grain delivery for road security |
| 6 | `treaty_flotilla_saline_corridor_concordat` | The Flotilla Saline Corridor Concordat | The Fleet, The Cutters | Lock Gate Four to Shallows Market | Diesel tariffs for tidal sluice lockage |
| 7 | `treaty_switchback_fuel_and_passage_accord` | The Switchback Fuel & Passage Accord | Ash Sign, Forward Roster | Switchback Waystation to Snowline | Paraffin fuel for pilgrim guidance |
| 8 | `treaty_scale_suburban_fair_trade_convention` | The Scale Suburban Fair Trade Convention | The Scale, Rebuilders | Caravanserai to Grange Hall | Standard weights & debt arbitration |
| 9 | `treaty_foundry_scrap_salvage_demarcation` | The Foundry Scrap Salvage Demarcation | Silent Foundry, The Scale | Recovery Yard & Concrete Plant | Steel scrap for cast iron parts |
| 10 | `treaty_roster_border_demilitarization_pact` | The Roster Border Demilitarization Pact | Forward Roster, Central Garrison | Neutral Ground 5km buffer | Patrol caps & prisoner exchange |
| 11 | `treaty_deep_coast_aquifer_protection_treaty` | The Deep Coast Aquifer Protection Treaty | The Fleet, Rebuilders | Pump Station Nine intake marsh | Freshwater intake anti-pollution |
| 12 | `treaty_high_scarp_observatory_sanctuary` | The High Scarp Observatory Sanctuary | Ash Sign, Silent Foundry | Summit Relay & Low-Background Lab | Telemetry data for brass hardware |
