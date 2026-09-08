# Verdict History Location Inventory

`VerdictCatalogLoader.LoadLocations` loads the 15 entries in
`verdict_locations.json`. The six original ladder references and six new
references all resolve against committed catalogs.

| Ladder use | Location ID | Catalog authority | Status |
|---|---|---|---|
| Layer 1 | `loc_geophone_pit_1` | `verdict_locations.json` | Resolved |
| Layers 2–3 | `loc_network_fuse_bunker` | `verdict_locations.json` | Resolved |
| Layer 4 | `location_the_dead_hand_core` | `locations.json` | Resolved committed world location |
| Layer 5 | `loc_comm_array` | `locations_expansion3.json` | Resolved committed Verdict location |
| Layer 6 | `loc_archive_tape_silo` | `verdict_locations.json` | Resolved |
| Layer 7 | `loc_twelve_gauge_array` | `verdict_locations.json` | Resolved |
| Layer 8 | `loc_decommissioned_signal_relay` | `verdict_locations.json` | Resolved |
| Layer 9 | `location_radar_site` | `deep_lore_locations.json` | Resolved Plan 116 location |
| Layer 10 | `location_weather_station` | `deep_lore_locations.json` | Resolved Plan 116 location |
| Layer 11 | `loc_clifftop_observation_bunker` | `verdict_locations.json` | Resolved |
| Layer 12 | `loc_border_checkpoint_ruins` | `verdict_locations.json` | Resolved |

The location field is currently documentary for the Verdict journal ladder.
It does not itself gate travel, unlock a journal event, or mutate the
EvidenceLedger.
