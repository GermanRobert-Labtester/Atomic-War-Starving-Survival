# Dive Site Master Matrix (Plan 23 / Task 23B)

Final catalog: **14 live, differentiated dive sites** in `dive_sites.json` (schema_version 2).
Stable IDs: the 4 committed Plan 09 sites, the 8 in-flight Plan 10 sites (IDs preserved
verbatim, tuned profiles untouched), and 2 Plan 23 additions. Duplicate-resolution per
`PLAN10_PLAN23_DIVE_RECONCILIATION.md`. Count contract pinned by
`Plan10CatalogCoverageTests.DiveSites_Has14Entries` + `Plan23DiveMechanicCoverageTests`.

| # | site_id | Name | Anchor (loc) | Class | Air | Noise | Gear gate | Safes | Contamination | Loot identity | Discovery | Fleet hook |
|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | `site_exp09_ss_sovereign` | S.S. Sovereign Wreck | loc_settlement_cape_beacon | Cargo steamer wreck | 120 | 0.50 | — | ✅ purser safe (d4) | — | Market iodine cargo / log cylinder | Commune logbook | iodine-market story (keeper thread) |
| 2 | `site_exp09_ferry_terminal` | The Drowned Ferry Terminal | `loc_frozen_river_ferry_crossing` | Structure | 90 | 0.60 | — | — | — | Civilian/utility loot | Visible from the crossing | — |
| 3 | `site_exp09_barge_flotilla` | The Barge Flotilla (Upstream Mooring) | `loc_black_flotilla_outpost` | Flotilla mooring | 100 | 0.40 | — | — | — | Flotilla cargo | Home water of the Salvage Fleet | Salvage |
| 4 | `site_exp09_naval_patrol` | The Patrol Craft | `loc_maritime_icebreaker_dock` | Warship wreck | 80 | 0.70 | — | — | thousand-yard stare (deep key) | Munitions/bounded military | Deep-coast survey | Escort (war grave candidate) |
| 5 | `site_exp09_sunken_submarine` | The Half-Submerged Barrik | `loc_shelf_service_channel` | Deep submarine | 70 | 0.80 | sealed dive lamp | — | thousand-yard stare | Military/technical | Verrill's line notes | Deep Fleet prestige |
| 6 | `site_exp09_flooded_metro` | The Floored Metro Concourse | `loc_the_overflow` | Flooded transit | 110 | 0.45 | — | — | — | Civilian/utility | The Overflow grid maps | — |
| 7 | `site_exp09_submerged_convoy` | The Submerged Convoy (Three Trucks) | `location_crashed_icebreaker_convoy` | Cargo hulk | 95 | 0.55 | — | — | — | Convoy cargo | Convoy war aftermath | Escort |
| 8 | `site_exp09_drowned_fuel_depot` | The Drowned Fuel Depot (Lower Dock) | `loc_hydro_baron_desal_plant_4` | Industrial | 85 | 0.65 | cutting tool | — | — | Bounded fuel/parts | Hydro-Baron dock gossip | Salvage |
| 9 | `site_exp09_offshore_relay` | The Offshore Radio Relay Station | `loc_coastal_fog_signal_station` | Relay structure | 75 | 0.70 | — | — | — | Radio/electronics | Foghorn service log | Codekeeper |
| 10 | `site_exp09_flooded_field_hospital` | The Flooded Quarantine Barge | `loc_settlement_cape_beacon` | Quarantine barge | 90 | 0.50 | — | — | stare + phantom smell | Medical goods | Commune's oldest charts | — |
| 11 | `site_exp09_wrecked_patrol_craft` | The Wrecked Flotilla Picket Craft | `loc_shelf_roadstead_crane` | War grave | 85 | 0.65 | — | — | thousand-yard stare | Memorial/bounded military | Convoy-war aftermath | War-grave memorial (23D) |
| 12 | `site_exp09_submerged_siphon` | The Submerged Siphon & Pump Station | `loc_hydro_baron_desal_plant_4` | Pumping station | 105 | 0.40 | — | — | — | Pump parts/water | Baron work orders | Hydro-Barons |
| 13 | `site_exp23_payroll_strongroom` | The Harbor-Master's Payroll Strongroom | `loc_maritime_icebreaker_dock` | Safe-heavy | 80 | 0.75 | salvage cutting tool | ✅✅ payroll + ledger box | — | Claim tags, ledgers, bounded coin-ammo | Codekeeper sells the bearing | Salvage/office |
| 14 | `site_exp23_brine_cistern` | The Brine Cistern Gallery | `loc_settlement_cape_beacon` | Deep/contamination | 95 | 0.50 | rebreather canister | — | disgust cascade + phantom smell | Chemicals, iodine, sealant | Verrill's caution, cistern charts | Deep Fleet (contaminated) |

## Uniqueness rule

No two sites share the same practical combination of **access anchor + hazard ladder +
mechanic set + reward identity**. Differentiators: anchor location, room hazard ladder,
noise floor, air budget, gear gate, safe count/difficulty, contamination keys, loot
category mix, discovery path, and fleet affiliation. `Sites_AllHaveLocationAnchor_AndDiscovery`
and the uniqueness gate pin this in tests.

## Repeatability

- All sites: repeatable **procedural scavenge** salvage (visit-count decay, day-phase
  degradation) — the coast is a region, not a one-shot dungeon.
- One-time consumables: site safes (open once, persisted), the Sovereign keeper thread,
  the picket bell (unique Relic node), the fleet log cylinder (quest object, single
  authoritative source: the purser safe).
