# ABYSSAL ANOMALIES RECORD AUTHORITY & PROVENANCE MATRIX (PLAN 151)

## 1. Executive Summary & Epistemic Directive
The Abyssal Anomalies corpus consists of 30 authored scientific observations, telemetry captures, and environmental testimonies divided across four distinct record families:
- **Hydrophone Acoustic Logs** (8 records in `narrative/hydrophone_acoustic_logs.json`)
- **Geothermal Borehole Logs** (7 records in `narrative/geothermal_borehole_logs.json`)
- **Cryopod Failure Logs** (8 records in `narrative/cryopod_failure_logs.json`)
- **Salt-Mine Inscriptions** (7 records in `narrative/salt_mine_inscriptions.json`)

### Core Authority Invariants
1. **Historical Telemetry, Not Live World State**: An archived temperature (e.g. 312.4°C), depth (e.g. 2420m), acoustic frequency (e.g. 24.5 Hz), or chamber pressure (e.g. 145 kPa) is a record of what an instrument measured at the relative timestamp indicated. It must never overwrite, modulate, or drive live shelter thermal, power, depth, or acoustic systems.
2. **Zero Cryogenic Revival**: Cryopod logs record historical failures, vitrification breakdowns, and terminal euthanasia. They do not introduce cryogenic mechanics, suspended animation gameplay, survivor resurrection, or NPC spawning.
3. **Environmental Ambiguity Preserved**: Where logs note anomalous acoustic clicks or deep vibrations, the prose remains ambiguous scientific testimony. It does not spawn supernatural entities or paranormal subsystems.
4. **Idempotent Discovery**: Discovery is recorded strictly via `JournalSystem` knowledge keys (`KnowledgeKeys.NarrativeDiscovered(discoveryId)`), requiring zero new save stores and introducing zero retroactive mutation.

---

## 2. Complete 30-Record Authority & Provenance Census

| Record ID | Family | Provenance Class | Source File | Authored Timestamp | Physical Asset / Location | Primary Producer | Channel | Min Day | Disposition |
|---|---|---|---|---|---|---|---|---|---|
| `hydrophone_shelf_ice_calving_echo` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_04_WINTER_DAY_088` | HYDRO-BUOY-NORTH-04 / Arctic Shelf | `loc_ice_core_store` | `location_inspection` | 20 | **ACTIVATED** |
| `hydrophone_submarine_cavitation_ghost` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_06_DAY_204` | HYDRO-BUOY-NORTH-02 / Arctic Trench | `loc_bathymetric_boat` | `location_inspection` | 30 | **ACTIVATED** |
| `hydrophone_deep_trench_thermal_vent` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_08_DAY_012` | HYDRO-BUOY-DEEP-09 / Rift Valley | `location_geo_thermal_plant_ruins` | `location_inspection` | 25 | **ACTIVATED** |
| `hydrophone_sunken_freighter_bulkhead_collapse` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_02_DAY_310` | HYDRO-BUOY-COASTAL-01 / Wreck 240m | `loc_cold_store_atlantic` | `location_inspection` | 15 | **ACTIVATED** |
| `hydrophone_biological_benthos_clicks` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_11_DAY_145` | HYDRO-BUOY-DEEP-07 / 620m Benthos | `loc_bathymetric_boat` | `location_inspection` | 45 | **ACTIVATED** |
| `hydrophone_coastal_minefield_chain_drag` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_09_DAY_033` | HYDRO-BUOY-COASTAL-05 / Granite Reef | `loc_lock_gate_four` | `location_inspection` | 35 | Deferred (Phase 2) |
| `hydrophone_active_sonar_orphan_ping` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_14_DAY_290` | HYDRO-BUOY-NORTH-01 / Autonomous Node | `room_radio_tuner` | `radio_archive` | 40 | Deferred (Phase 2) |
| `hydrophone_glacial_earthquake_harmonic` | Hydrophone | Historical Instrument Record | `hydrophone_acoustic_logs.json` | `YEAR_16_DAY_002` | HYDRO-BUOY-DEEP-03 / Basal Fault | `loc_snowline_station` | `location_inspection` | 50 | Deferred (Phase 2) |
| `borehole_magma_boundary_temperature_spike` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_05_DAY_114` | DEEP-WELL-SUMP-08 / Granodiorite 2420m | `location_geo_thermal_plant_ruins` | `location_inspection` | 10 | **ACTIVATED** |
| `borehole_sulfur_steam_vent_corrosion` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_07_DAY_302` | DEEP-WELL-SUMP-03 / Schist 1850m | `room_filtration` | `location_inspection` | 18 | **ACTIVATED** |
| `borehole_seismic_fault_hydraulic_pulse` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_09_DAY_056` | DEEP-WELL-SUMP-05 / Fault Breccia 2100m | `room_water_pump` | `location_inspection` | 22 | **ACTIVATED** |
| `borehole_heavy_metal_brine_precipitate` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_12_DAY_210` | DEEP-WELL-SUMP-02 / Quartz Vein 1600m | `location_the_sump_cathedral` | `location_inspection` | 28 | **ACTIVATED** |
| `borehole_downhole_drillstring_seizure` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_14_DAY_008` | DEEP-WELL-SUMP-09 / Peridotite 2750m | `room_workshop` | `location_inspection` | 32 | Deferred (Phase 2) |
| `borehole_radon_gas_outgassing_surge` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_15_DAY_180` | DEEP-WELL-SUMP-01 / Pegmatite 1200m | `room_filtration_stack` | `location_inspection` | 25 | Deferred (Phase 2) |
| `borehole_acoustic_resonator_whisper` | Geothermal | Historical Engineering Record | `geothermal_borehole_logs.json` | `YEAR_18_DAY_330` | DEEP-WELL-SUMP-06 / Columnar Basalt 2250m | `location_the_sump_cathedral` | `location_inspection` | 50 | Deferred (Phase 2) |
| `cryopod_coolant_circuit_boiloff` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_08_DAY_142` | CRYO-CHAMBER-VAULT-14-POD-04 / Dr. Chen | `government_bunker` | `library_terminal` | 15 | **ACTIVATED** |
| `cryopod_vitrification_crystallization_error` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_10_DAY_019` | CRYO-CHAMBER-VAULT-14-POD-08 / Gen. Kane | `government_bunker` | `library_terminal` | 25 | **ACTIVATED** |
| `cryopod_neural_eeg_spike_nightmare` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_12_DAY_300` | CRYO-CHAMBER-VAULT-14-POD-12 / Sara Vance | `loc_low_background_lab` | `location_inspection` | 35 | **ACTIVATED** |
| `cryopod_perfusion_pump_rotor_jam` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_14_DAY_050` | CRYO-CHAMBER-VAULT-14-POD-02 / Eng. Morris | `abandoned_hospital` | `location_inspection` | 20 | **ACTIVATED** |
| `cryopod_biometric_subject_identity_corruption` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_15_DAY_220` | CRYO-CHAMBER-VAULT-14-POD-11 / Unknown | `loc_records_annex` | `location_inspection` | 30 | Deferred (Phase 2) |
| `cryopod_desperate_manual_thaw_breach` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_17_DAY_088` | CRYO-CHAMBER-VAULT-14-POD-06 / Col. Reid | `government_bunker` | `library_terminal` | 40 | Deferred (Phase 2) |
| `cryopod_power_shedding_priority_cascade` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_19_DAY_110` | CRYO-CHAMBER-VAULT-14-ARRAY / Pods 13-24 | `government_bunker` | `library_terminal` | 45 | Deferred (Phase 2) |
| `cryopod_terminal_euthanasia_protocol` | Cryopod | Historical Incident Record | `cryopod_failure_logs.json` | `YEAR_21_DAY_360` | CRYO-CHAMBER-VAULT-14-ARRAY / Wing 30 Pods | `government_bunker` | `library_terminal` | 55 | Deferred (Phase 2) |
| `salt_mine_shaft_tally_340_days` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_01_DAY_340` | Level 04 Halite Drift / Tomas Heller | `location_the_sump_cathedral` | `location_inspection` | 8 | **ACTIVATED** |
| `salt_mine_brine_spring_warning` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_03_DAY_112` | Level 06 Deep Sump / Foreman Vance | `location_the_sump_cathedral` | `location_inspection` | 16 | **ACTIVATED** |
| `salt_mine_blind_mule_memorial` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_05_DAY_080` | Level 02 Haulage Stable / Stable Hand Eli | `location_the_sump_cathedral` | `location_inspection` | 12 | **ACTIVATED** |
| `salt_mine_airlock_collapse_last_words` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_06_DAY_290` | Level 05 Crawlway / Shift 3 Miners | `room_airlock` | `location_inspection` | 24 | **ACTIVATED** |
| `salt_mine_crystalline_shrine_vow` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_09_DAY_150` | Cavern of Swords / Selenite Geode | `loc_the_vessels_cell` | `location_inspection` | 35 | Deferred (Phase 2) |
| `salt_mine_methane_pocket_marker` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_11_DAY_005` | Level 03 North Heading / Inspector Ross | `loc_pump_station_nine` | `location_inspection` | 22 | Deferred (Phase 2) |
| `salt_mine_scrip_forgery_workshop_graffiti` | Salt Mine | Physical Inscription / Testimony | `salt_mine_inscriptions.json` | `YEAR_13_DAY_312` | Explosives Magazine 7 / Salt Bench | `stranger_cache` | `location_inspection` | 20 | Deferred (Phase 2) |

---

## 3. Allowed vs. Forbidden Downstream Interpretations

### Hydrophone Records
- **Allowed**: Reading acoustic signatures in the scientific codex; learning about pre-war and post-war naval activity; tracking ocean telemetry history.
- **Forbidden**: Modifying ocean simulation, sonar radar HUD, radiation exposure, or surface current speeds based on acoustic dB or frequency numbers.

### Geothermal Borehole Records
- **Allowed**: Discovering deep geological telemetry; uncovering historical wellhead pressures and temperatures; understanding the origins of shelter geothermal siphons.
- **Forbidden**: Modifying live shelter heat output, boiler pressure, power generation wattage, or causing instant boiler blowouts based on historical bar or °C values.

### Cryopod Incident Records
- **Allowed**: Reviewing sealed black-project logs; investigating historical failures of Vault 14 life support; discovering medical tragedies.
- **Forbidden**: Reviving frozen subjects; creating NPCs or companion characters; adding suspended animation or cryo-chamber rooms to the shelter.

### Salt-Mine Inscription Records
- **Allowed**: Inspecting historical miner tallies, emergency warnings, mule memorials, and tragic final letters.
- **Forbidden**: Spawning dead miners; treating historical warnings as immediate map lockouts; modifying current miner faction standing without a typed quest contract.
