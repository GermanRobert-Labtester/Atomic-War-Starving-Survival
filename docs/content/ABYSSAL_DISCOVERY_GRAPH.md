# ABYSSAL ANOMALIES DISCOVERY GRAPH & COVERAGE SPECIFICATION (PLAN 151)

## 1. Summary Statistics
- **Total Authoritative Records**: 30 (100% census coverage)
- **Activated in Pass 1**: 17 records (5 Hydrophone, 4 Geothermal, 4 Cryopod, 4 Salt Mine)
- **Deferred to Phase 2**: 13 records (3 Hydrophone, 3 Geothermal, 4 Cryopod, 3 Salt Mine)
- **Minimum Family Live Discoveries**: 4 (Exceeds requirement of >= 3 per family)
- **Total Unique Producers Used**: 10 distinct locations/nodes across shelter and wasteland
- **Maximum Records per Location**: <= 4 (Complies with Rule: no location dumps > 4 records)

---

## 2. Producer & Location Distribution

| Producer Node ID | Type / Domain | Activated Records | Record IDs |
|---|---|---|---|
| `location_the_sump_cathedral` | Underground / Mining | 4 | `borehole_heavy_metal_brine_precipitate`<br>`salt_mine_shaft_tally_340_days`<br>`salt_mine_brine_spring_warning`<br>`salt_mine_blind_mule_memorial` |
| `location_geo_thermal_plant_ruins` | Industrial Surface | 2 | `hydrophone_deep_trench_thermal_vent`<br>`borehole_magma_boundary_temperature_spike` |
| `loc_bathymetric_boat` | Maritime Expedition | 2 | `hydrophone_submarine_cavitation_ghost`<br>`hydrophone_biological_benthos_clicks` |
| `government_bunker` | High-Security Subsurface | 2 | `cryopod_coolant_circuit_boiloff`<br>`cryopod_vitrification_crystallization_error` |
| `loc_ice_core_store` | Glacial Research Outpost | 1 | `hydrophone_shelf_ice_calving_echo` |
| `loc_cold_store_atlantic` | Coastal Maritime Facility | 1 | `hydrophone_sunken_freighter_bulkhead_collapse` |
| `room_filtration` | Shelter Life Support | 1 | `borehole_sulfur_steam_vent_corrosion` |
| `room_water_pump` | Shelter Hydraulic Manifold | 1 | `borehole_seismic_fault_hydraulic_pulse` |
| `loc_low_background_lab` | Deep Physics Laboratory | 1 | `cryopod_neural_eeg_spike_nightmare` |
| `abandoned_hospital` | Medical Ruin | 1 | `cryopod_perfusion_pump_rotor_jam` |
| `room_airlock` | Shelter Perimeter Portal | 1 | `salt_mine_airlock_collapse_last_words` |

---

## 3. Earliest Possible Discovery Schedule (by Min Day)

```
Day 08: salt_mine_shaft_tally_340_days (location_the_sump_cathedral)
Day 10: borehole_magma_boundary_temperature_spike (location_geo_thermal_plant_ruins)
Day 12: salt_mine_blind_mule_memorial (location_the_sump_cathedral)
Day 15: hydrophone_sunken_freighter_bulkhead_collapse (loc_cold_store_atlantic)
Day 15: cryopod_coolant_circuit_boiloff (government_bunker)
Day 16: salt_mine_brine_spring_warning (location_the_sump_cathedral)
Day 18: borehole_sulfur_steam_vent_corrosion (room_filtration)
Day 20: hydrophone_shelf_ice_calving_echo (loc_ice_core_store)
Day 20: cryopod_perfusion_pump_rotor_jam (abandoned_hospital)
Day 22: borehole_seismic_fault_hydraulic_pulse (room_water_pump)
Day 24: salt_mine_airlock_collapse_last_words (room_airlock)
Day 25: hydrophone_deep_trench_thermal_vent (location_geo_thermal_plant_ruins)
Day 25: cryopod_vitrification_crystallization_error (government_bunker)
Day 28: borehole_heavy_metal_brine_precipitate (location_the_sump_cathedral)
Day 30: hydrophone_submarine_cavitation_ghost (loc_bathymetric_boat)
Day 35: cryopod_neural_eeg_spike_nightmare (loc_low_background_lab)
Day 45: hydrophone_biological_benthos_clicks (loc_bathymetric_boat)
```

---

## 4. Phase 2 Deferred Records Registry

The following 13 records are mapped to coherent narrative targets but explicitly deferred to Phase 2 to prevent early content flooding and preserve late-campaign discoveries:

1. `hydrophone_coastal_minefield_chain_drag` (`loc_lock_gate_four`, min_day 35) — Requires coastal mine clearing narrative arc.
2. `hydrophone_active_sonar_orphan_ping` (`room_radio_tuner`, min_day 40) — Requires signal intelligence sonar demodulator.
3. `hydrophone_glacial_earthquake_harmonic` (`loc_snowline_station`, min_day 50) — Requires alpine seismic station restoration.
4. `borehole_downhole_drillstring_seizure` (`room_workshop`, min_day 32) — Requires heavy machining lathe inspection.
5. `borehole_radon_gas_outgassing_surge` (`room_filtration_stack`, min_day 25) — Requires basal radon migration sensor.
6. `borehole_acoustic_resonator_whisper` (`location_the_sump_cathedral`, min_day 50) — Requires deep sump acoustic acoustic mapping.
7. `cryopod_biometric_subject_identity_corruption` (`loc_records_annex`, min_day 30) — Requires magnetic drum archive restoration.
8. `cryopod_desperate_manual_thaw_breach` (`government_bunker`, min_day 40) — Requires Vault 14 security airlock breaching.
9. `cryopod_power_shedding_priority_cascade` (`government_bunker`, min_day 45) — Requires automated mainframe terminal access.
10. `cryopod_terminal_euthanasia_protocol` (`government_bunker`, min_day 55) — Requires Directive Omega black box recovery.
11. `salt_mine_crystalline_shrine_vow` (`loc_the_vessels_cell`, min_day 35) — Requires fringe cult selenite cavern access.
12. `salt_mine_methane_pocket_marker` (`loc_pump_station_nine`, min_day 22) — Requires mine ventilation shaft exploration.
13. `salt_mine_scrip_forgery_workshop_graffiti` (`stranger_cache`, min_day 20) — Requires black market scrip exchange discovery.
