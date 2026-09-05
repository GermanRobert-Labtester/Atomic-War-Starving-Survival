# Verdict Radio Site Handoff Contract (Plan 82 Integration)

> **Site Authority:** `Assets/StreamingAssets/Data/verdict_locations.json`
> **Rule:** Broadcasts provide ambient data, automated service logs, and telemetry corroborating physical site anomalies without overriding location state.

---

## Site-Linked Broadcast Matrix

| Broadcast ID | Trigger Day | Canonical Location ID | Site Name | Corroborating Physical Fact |
|---|---|---|---|---|
| `radio_verdict_barometric_spread` | 268 | `loc_coastal_meteorological_station` | Cape Wrath Meteorological Station | Paper barograph in station records 982 hPa while central automated feed issued false 1014 hPa packet. |
| `radio_verdict_service_cycle_greywater` | 272 | `loc_abandoned_tide_gauge` | Greywater Tide Gauge Station | Automated purge cycle proves station instrumentation remained fully powered and unmonitored. |
| `radio_verdict_stilling_well_delta` | 275 | `loc_abandoned_tide_gauge` | Greywater Tide Gauge Station | Corroborates red-ink +6 cm correction recorded in Mara Elsen's paper logbook. |
| `radio_verdict_strata_density_drift` | 288 | `loc_geological_core_vault` | Highland Core-Sample Repository | Subsurface acoustic velocity matches the altered density profile of tampered Core 7. |
| `radio_verdict_relay_switch_pass4` | 295 | `loc_decommissioned_signal_relay` | Pass 4 Signal Relay Mast | Proves automated emergency battery upkeep was maintained along border lines. |
| `radio_verdict_river_stage_deviation` | 304 | `loc_river_gauging_station` | Karsk River Gauging Station | High water mark on concrete piers aligns with the 4.12 m unpredicted stage telemetry. |
| `radio_verdict_core_vault_desiccant_purge`| 312 | `loc_geological_core_vault` | Highland Core-Sample Repository | Vault airlock purge confirms nitrogen climate preservation active over core racks. |
| `radio_verdict_repeater_origin_mismatch` | 326 | `loc_clifftop_observation_bunker` | North Cliff Observation Bunker | Confirms automated packet routing table deviations across North Cliff repeater chain. |
| `radio_verdict_spectrometry_drift_stjude` | 330 | `loc_sealed_marine_laboratory` | St. Jude Marine Laboratory | Sample rack 12 spectrometry drift corroborates Dr. Sena Korr's double-labeled benthic specimens. |
| `radio_verdict_substation_breaker_test` | 338 | `loc_network_fuse_bunker` | The Fuse World | 12 ms bus trip test confirms ongoing automated power routing in fuse corridors. |
