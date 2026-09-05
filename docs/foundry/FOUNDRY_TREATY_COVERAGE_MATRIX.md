# Foundry Treaty Coverage Matrix (15 Policies / 12 Treaties)

**Treaty Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Consequence Catalog:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

---

## 1. Treaty-by-Treaty Policy Coverage

| Index | Treaty ID | Title | Signatories | Policies Authored | Covered Outcomes |
|---|---|---|---|---|---|
| 1 | `treaty_brine_pipe_and_iodine_exchange` | The Brine Pipe & Iodine Exchange | Silent Foundry, The Office | 2 | `met`, `missed` |
| 2 | `treaty_cluster_labour_schedule` | The Cluster Labour Schedule | Silent Foundry, The Office, Cutters | 2 | `met`, `violated` |
| 3 | `treaty_road_iron_charter` | The Road Iron Charter | Silent Foundry, Cutters, Fleet | 2 | `met`, `missed` |
| 4 | `treaty_the_cluster_charter` | The Cluster Charter | Foundry, Office, Cutters, Fleet | 0 | *(Exempt: Finale Marker)* |
| 5 | `treaty_garrison_grain_tithe_compact` | The Garrison Grain Tithe Compact | Central Garrison, Rebuilders | 2 | `met`, `violated` |
| 6 | `treaty_flotilla_saline_corridor_concordat` | The Flotilla Saline Corridor Concordat | The Fleet, The Cutters | 2 | `met`, `missed` |
| 7 | `treaty_switchback_fuel_and_passage_accord` | The Switchback Fuel & Passage Accord | Ash Sign, Forward Roster | 2 | `met`, `violated` |
| 8 | `treaty_scale_suburban_fair_trade_convention` | The Scale Suburban Fair Trade Convention | The Scale, Rebuilders | 1 | `met` |
| 9 | `treaty_scrap_salvage_demarcation` | The Scrap Salvage Demarcation | The Cutters, The Scale | 0 | *(Staged for Follow-On)* |
| 10 | `treaty_roster_border_demilitarization_pact` | The Roster Border Demilitarization Pact | Forward Roster, Garrison | 0 | *(Staged for Follow-On)* |
| 11 | `treaty_deep_coast_aquifer_protection_treaty` | The Deep Coast Aquifer Protection Treaty | The Fleet, Rebuilders | 2 | `met`, `violated` |
| 12 | `treaty_high_scarp_observatory_sanctuary` | The High Scarp Observatory Sanctuary | Ash Sign, The Scale | 0 | *(Staged for Follow-On)* |

---

## 2. Statistical Breakdown

- **Total Catalog Size:** 15 policies
- **Treaties with 2 Policies:** 7 (`brine_pipe`, `labour_schedule`, `road_iron`, `saline_corridor`, `switchback_fuel`, `deep_coast_aquifer`, `grain_tithe`)
- **Treaties with 1 Policy:** 1 (`fair_trade_convention`)
- **Exempt Treaties:** 1 (`the_cluster_charter`)
- **Staged Treaties:** 3 (`scrap_salvage`, `border_demilitarization`, `observatory_sanctuary`)
- **Total Actionable Wasteland Treaties Covered:** 8 of 11 (72.7% coverage)

---

## 3. Mathematical Consistency Note

As audited in Task 103H, covering 10+ treaties with 2 policies each would require at least 20–22 policies. Under the strict constraint of reaching exactly 15 total policies from 6 baseline entries (+9 policies):
- $14$ policies allocate symmetrically across the $7$ highest-impact logistical treaties.
- $1$ policy establishes fair-trade convention compliance.
- Zero treaties are hallucinated; all 15 policies resolve against real canonical treaties in `foundry_accords.json`.
