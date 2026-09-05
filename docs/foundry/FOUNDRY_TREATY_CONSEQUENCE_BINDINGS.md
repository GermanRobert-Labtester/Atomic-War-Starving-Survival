# Foundry Treaty Consequence Bindings (Plan 102 ↔ Plan 103 Seam)

**Accord Authority:** `Assets/StreamingAssets/Data/foundry_accords.json` (Plan 102)
**Consequence Authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` (Plan 103)

---

## 1. Consequence Policy Seam

Plan 102 authored the contractual agreements. Plan 103 authored the mechanical consequence policies triggered by compliance (`met`), shortfall (`missed`), or active breach (`violated`).

### Current 15-Policy Coverage Across Plan 102 Treaties

| Accord ID | Title | Live Consequence Policies | Factions Impacted |
|---|---|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | The Brine Pipe & Iodine Exchange | `met`, `missed` | `faction_silent_foundry` |
| `treaty_cluster_labour_schedule` | The Cluster Labour Schedule | `met`, `violated` | `faction_silent_foundry` |
| `treaty_road_iron_charter` | The Road Iron Charter | `met`, `missed` | `faction_silent_foundry` |
| `treaty_the_cluster_charter` | The Cluster Charter | *None (Exempt finale marker)* | — |
| `treaty_garrison_grain_tithe_compact` | The Garrison Grain Tithe Compact | `met`, `violated` | `faction_central_garrison` |
| `treaty_flotilla_saline_corridor_concordat` | The Flotilla Saline Corridor Concordat | `met`, `missed` | `faction_the_fleet` |
| `treaty_switchback_fuel_and_passage_accord` | The Switchback Fuel & Passage Accord | `met`, `violated` | `faction_ash_sign` |
| `treaty_scale_suburban_fair_trade_convention` | The Scale Suburban Fair Trade Convention | `met` | `faction_the_scale` |
| `treaty_deep_coast_aquifer_protection_treaty` | The Deep Coast Aquifer Protection Treaty | `met`, `violated` | `faction_the_fleet` |
| `treaty_scrap_salvage_demarcation` | The Scrap Salvage Demarcation | *Staged follow-on* | `faction_the_cutters` |
| `treaty_roster_border_demilitarization_pact` | The Roster Border Demilitarization Pact | *Staged follow-on* | `faction_forward_roster` |
| `treaty_high_scarp_observatory_sanctuary` | The High Scarp Observatory Sanctuary | *Staged follow-on* | `faction_ash_sign` |

---

## 2. Dispatch Discipline

1. Treaties define rights, allocations, tariffs, and penalties.
2. The runtime looks up consequences via `SilentFoundryConsequencePolicyCatalog.Find(treaty_id, outcome)`.
3. If a treaty is not yet bound to a policy (e.g. `scrap_salvage`), `Find` returns `null` safely and no unintended mechanics fire.
4. Pinned by `FoundryAccordExpansionTests.ConsequenceSeam_Plan103PoliciesResolveAgainstTheseAccords`.
