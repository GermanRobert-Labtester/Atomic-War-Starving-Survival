# Plan 93 — Master Verdict NPC Roster (All 18 NPCs)

> **Authority:** `Assets/StreamingAssets/Data/verdict_npcs.json`
> **Total Definitions:** 18 (6 Baseline + 3 Plan 18 + 9 Plan 93 Additions)

---

## Complete 18-NPC Roster

| # | ID | Name | Role | Kind | Gating Flag | Location ID | Phase | Lines | Muster Witness? | Radio Link? | Plan 52 Living? |
|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | `npc_eden_vale` | Eden Vale | Amateur radio operator, comm-array bleed | `tape_echo` | `flag_verdict_eden_log_recovered` | `loc_comm_array` | 1 | 2 | No (archival) | Yes (88.5 MHz) | No |
| 2 | `npc_ferris_voss` | Ferris Voss | Fire-control acceptance engineer, last human in the fuse world | `paper_ghost` | `flag_verdict_fuse_world_read` | `loc_network_fuse_bunker` | 1 | 2 | No | No | No |
| 3 | `npc_iran_bell` | Iaran Bell | Tempest maintenance supervisor, the valve-touch hand | `paper_ghost` | `flag_verdict_shift_charter_restored` | `loc_network_fuse_bunker` | 2 | 2 | No | No | No |
| 4 | `npc_selya_saltmarsh` | Selya Saltmarsh | Census clerk, the only human with an opinion about the count | `living` | `flag_verdict_clerk_met` | `loc_twelve_gauge_array` | 2 | 2 | No | No | Yes |
| 5 | `npc_maro_veen` | Maro Veen | The machine's own voice — the census-window tape loop | `tape_echo` | `flag_verdict_call_resolved` | `loc_archive_tape_silo` | 3 | 2 | No | Yes (99.0 MHz) | No |
| 6 | `npc_whisper_cipher` | Whisper Cipher | The relay network's aggregate readings — univocal, procedural | `readings` | `flag_verdict_relay_read` | `loc_radio_relay_mast` | 1 | 2 | No | Yes (99.0 MHz) | No |
| 7 | `npc_tomas_reid` | Tomas Reid | Defense clerk, tribunal appeals and admissibility | `living` | `flag_verdict_reid_enrolled` | `loc_network_fuse_bunker` | 1 | 2 | No | No | Yes |
| 8 | `npc_elena_vane` | Elena Vane | Machine-cult deaconess, Voice of the Standard | `living` | `flag_verdict_vane_enrolled` | `loc_archive_tape_silo` | 2 | 2 | No | No | Yes |
| 9 | `npc_kasper_holt` | Kasper Holt | Chief Archival Custodian, chain of custody keeper | `paper_ghost` | `flag_verdict_holt_enrolled` | `loc_archive_tape_silo` | 1 | 2 | No | No | No |
| 10 | `npc_mara_elsen` | Mara Elsen | Tide-gauge keeper assigned to the coastal survey station after the civilian network stopped reporting | `paper_ghost` | `flag_verdict_tide_gauge_inspected` | `loc_abandoned_tide_gauge` | 1 | 3 | No | No | No |
| 11 | `npc_ilya_venn` | Ilya Venn | Weather-station observer who kept manual readings after the automated feed began contradicting the instruments outside | `tape_echo` | `flag_verdict_weather_chart_recovered` | `loc_coastal_meteorological_station` | 1 | 3 | Alternate | Yes (Weather burst) | No |
| 12 | `npc_garrick_daal` | Garrick Daal | Signalman assigned to the cliff bunker relay, responsible for routing coastal military and civil traffic through a failing repeater chain | `tape_echo` | `flag_verdict_cliff_signal_decoded` | `loc_clifftop_observation_bunker` | 2 | 3 | **Yes** (Comm anomaly) | Yes (99.0 MHz) | No |
| 13 | `npc_sena_korr` | Dr. Sena Korr | Marine-laboratory researcher tracking contamination in coastal organisms after sample results no longer matched the intake logs | `paper_ghost` | `flag_verdict_marine_samples_cataloged` | `loc_sealed_marine_laboratory` | 2 | 3 | **Yes** (Bio chronology) | No | No |
| 14 | `npc_torin_rask` | Torin Rask | Forestry surveyor mapping dead zones and windthrow after the official burn maps stopped matching what remained on the ground | `paper_ghost` | `flag_verdict_forestry_grid_surveyed` | `loc_forestry_survey_post` | 1 | 3 | No | No | No |
| 15 | `npc_oren_varek` | Oren Varek | Core-sample technician who catalogued subsurface layers beneath an interior monitoring cache | `paper_ghost` | `flag_verdict_core_strata_verified` | `loc_geological_core_vault` | 2 | 3 | Alternate | No | No |
| 16 | `npc_lena_rost` | Lena Rost | River-gauge attendant responsible for manual flood-stage readings after remote telemetry failed | `tape_echo` | `flag_verdict_river_stage_logged` | `loc_river_gauging_station` | 1 | 3 | No | No | No |
| 17 | `npc_tessa_mirn` | Tessa Mirn | Agricultural-station botanist who tracked germination failures across supposedly uncontaminated seed lots | `paper_ghost` | `flag_verdict_seed_trials_audited` | `loc_abandoned_agricultural_station` | 2 | 3 | No | No | No |
| 18 | `npc_karel_norn` | Karel Norn | Border-relay operator who handled the last handoffs between civil warning traffic and restricted military channels | `tape_echo` | `flag_verdict_border_handoff_recorded` | `loc_decommissioned_signal_relay` | 3 | 3 | **Yes** (Command order) | Yes (Demarcation carrier) | No |
