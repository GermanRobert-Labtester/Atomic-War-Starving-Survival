# Expansion Content Matrix — Four Charter Expansions

## 1. Scope & Systemic Distribution

| Expansion | ID Prefix | Primary Systems | Quest Count | Memory/Encounter Count | Anchor Locations |
|---|---|---|---|---|---|
| **Holdfast** | `quest_holdfast_` | `IceRoadSystem`, `CensusClaimSystem`, `BrineWaterSystem`, `HoldfastTradeSession` | 24 | — | `loc_ice_road_gate`, `loc_weighbridge`, `location_abandoned_desalination`, `loc_cut_kilometre_19`, `player_shelter` |
| **Standing Record** | `quest_record_` | `LocationLayoutSystem`, `LocationMemorySystem`, `SiteEncounterSystem`, `MemorialSystem` | 22 | 52 strata memories | `loc_cut_kilometre_19`, `loc_transit_authority_hq`, `loc_excavation_command_vault`, `loc_excavation_metro_interchange`, `loc_excavation_mine_shaft`, `loc_excavation_archive_bunker`, `loc_lock_gate_four`, `loc_seed_library_annex`, `loc_cold_store_atlantic` |
| **Crossing** | `quest_crossing_` | `CrossingArbitrationSystem`, `CrossingSession`, `TradingSystem` | 20 | 14 encounters (4 major crises) | `loc_crossing_viaduct_gate`, `loc_crossing_weighbridge`, `loc_crossing_stallrow`, `loc_crossing_underwrite_hall` |
| **Verdict** | `quest_verdict_` | `ReckoningSystem`, `MachineLogSystem`, `EvidenceLedger` | 16 | 9 NPCs | `loc_geophone_pit_1`, `loc_twelve_gauge_array`, `loc_network_fuse_bunker`, `loc_archive_tape_silo` |

## 2. Integrity Verification

All 82 quests/questlines across the four charter expansions cross-reference valid location IDs, item IDs, and faction strings. Presence is checked by `CatalogIntegrityValidator` and `Plan18ExpansionDeepeningTests`.
