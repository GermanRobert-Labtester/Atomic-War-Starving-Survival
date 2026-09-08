# Plan 127 World History Baseline Matrix

The ladder is authored in `verdict_data.json` and presented through the
journal/codex overlay. The table records the complete post-expansion order.

| Layer | Knowledge key | Title | Discovery location | Runtime state |
|---:|---|---|---|---|
| 1 | `lore_verdict_geophone_one` | The First Geophone Pit | `loc_geophone_pit_1` | Existing hardcoded unlock |
| 2 | `lore_verdict_shift_charters` | The Linen Codes | `loc_network_fuse_bunker` | Existing hardcoded unlock |
| 3 | `lore_verdict_standard` | The Standard for the Continuance of Service | `loc_network_fuse_bunker` | Existing hardcoded unlock |
| 4 | `lore_verdict_the_hold` | Hold Pending Count | `location_the_dead_hand_core` | Existing hardcoded unlock |
| 5 | `lore_verdict_the_call` | The Reckoning Call | `loc_comm_array` | Existing hardcoded unlock |
| 6 | `lore_verdict_the_count` | The Count | `loc_archive_tape_silo` | Existing hardcoded unlock |
| 7 | `lore_verdict_second_geophone` | The Second Geophone Pit | `loc_twelve_gauge_array` | Catalog/codex row; no dynamic unlock |
| 8 | `lore_verdict_cable_run_east` | The Cable Run East | `loc_decommissioned_signal_relay` | Plan 113 site; catalog/codex row |
| 9 | `lore_verdict_counting_house_origin` | The Counting House Before the Count | `location_radar_site` | Plan 116 site; catalog/codex row |
| 10 | `lore_verdict_first_halt` | The First Halt | `location_weather_station` | Plan 116 site; catalog/codex row |
| 11 | `lore_verdict_last_hand` | The Last Hand | `loc_clifftop_observation_bunker` | Plan 113 site; catalog/codex row |
| 12 | `lore_verdict_open_count` | The Open Count | `loc_border_checkpoint_ruins` | Plan 113 site; catalog/codex row |

## Cross-plan alignment

- **Plan 113:** layers 8, 11, and 12 reuse committed quest investigation
  sites: Pass 4 Signal Relay Mast, North Cliff Observation Bunker, and Gate
  Seven Border Checkpoint. The quest catalog describes these sites in
  narrative prompts, but does not expose a structured location-reference
  field for the ladder to link against.
- **Plan 116:** layers 9 and 10 reuse the committed deep-lore locations
  `location_radar_site` and `location_weather_station`.
- **Plan 82:** layer 7 reuses the committed Verdict location
  `loc_twelve_gauge_array`.

These are authored content alignments, not new runtime wiring. No new quest,
deep-lore, discovery, or save field was added.
