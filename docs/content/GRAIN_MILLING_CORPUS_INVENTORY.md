# Grain Milling Corpus Inventory & Forensic Census

## 1. Executive Summary

This document establishes the forensic census for the 30 authored records comprising ASHFALL's `GrainMillingCatalog` across four distinct industrial food-processing and storage families. The catalog models four critical post-harvest phases:
1. **Burr Millstone Dressing Logs** (8 records) — Mechanical grinding face dressing, furrow cutting, runner stone balancing, and thrust bearing maintenance.
2. **Bolting Silk Mesh Reports** (8 records) — Sifter reel gauze grades, micron aperture screening, flour extraction yields, and mechanical sifter dynamics.
3. **Grain Silo Weevil Audits** (7 records) — Long-term bulk grain storage, pest infestations (*Sitophilus granarius*, mites), moisture migration, hot spots, and hermetic gas treatment.
4. **Mill Dampener Tempering Assays** (7 records) — Controlled water addition, hydrothermal conditioning, bran toughening, and kernel mellowing.

In accordance with **Plan 157 §4 (Non-Negotiable Authority Boundaries)**, all 30 entries represent historical, technical, or maintenance observations recorded at sample time. They do not own or mutate live shelter grain quantities, recipe outputs, food safety status, spoilage rates, or market prices.

---

## 2. Complete 30-Record Census

### 2.1 Burr Millstone Dressing Logs (`burr_millstone_dressing_logs.json`)

| Record ID | Equipment Pair ID | Stone Material Type | Cracks / In | Runner RPM | Relative Timestamp | Truth Class | Candidate Location | Measurement Summary |
|---|---|---|:---:|:---:|---|---|---|---|
| `burr_millstone_french_chert_chisel_cracking` | `PRIMARY_BURR_RUNNER_PAIR_01` | French Cellular Quartz Chert | 16.0 | 115.0 | `YEAR_02_MILL_LOG_01` | TechnicalMaintenanceLog | `room_workshop` / `loc_grain_silo` | 16 cracks/in, 115 RPM |
| `burr_millstone_runner_stone_rynd_balance_lead` | `RESERVE_GRIST_MILL_RUNNER_02` | Sectional Quartz Segments | 12.0 | 125.0 | `YEAR_04_MILL_LOG_03` | MaintenanceObservation | `room_workshop` / `loc_quarry` | 12 cracks/in, 125 RPM, 1.8 kg lead |
| `burr_millstone_furrow_lands_feather_edge_wear` | `COMMISSARY_FLOUR_MILL_01` | Derbyshire Peak Gritstone | 8.0 | 110.0 | `YEAR_07_MILL_LOG_02` | OperationalIncident | `loc_grain_silo` | 8 cracks/in, 110 RPM, 65°C spike |
| `burr_millstone_spindle_footstep_bearing_lignum_vitae` | `WATER_DRIVEN_GRIST_MILL_03` | French Burr Chert Blocks | 14.0 | 105.0 | `YEAR_09_MILL_LOG_04` | TechnicalEngineeringLog | `loc_quarry` / `room_workshop` | 14 cracks/in, 105 RPM, 0.2mm wear |
| `burr_millstone_bridge_tree_tentering_screw_chatter` | `PRECISION_SEMOLINA_STONE_04` | Fine Grain Quartz Burr | 18.0 | 130.0 | `YEAR_12_MILL_LOG_01` | MaintenanceObservation | `room_workshop` | 18 cracks/in, 130 RPM, 0.15mm gap flutter |
| `burr_millstone_swallow_eye_centrifugal_feed_shoe` | `AUTOMATED_HOPPER_FEED_STONE` | Composite Emery Cement Stone | 10.0 | 120.0 | `YEAR_15_MILL_LOG_03` | OperationalDiagnostic | `loc_settlement_silo_burrow` | 10 cracks/in, 120 RPM, 180 kg/hr feed |
| `burr_millstone_flour_hoop_leather_sweeper_brush` | `DUST_FREE_ENCLOSED_MILL_02` | French Burr Heavy Blocks | 14.0 | 112.0 | `YEAR_17_MILL_LOG_02` | MaintenanceObservation | `room_workshop` / `loc_grain_silo` | 14 cracks/in, 112 RPM |
| `burr_millstone_iron_banding_thermal_shrink_fit` | `HEAVY_FOUNDRY_MILL_RUNNER_01` | Segmented Burr Quartz Ring | 16.0 | 118.0 | `YEAR_20_MILL_LOG_01` | EngineeringFabricationLog | `room_foundry` / `loc_quarry` | 16 cracks/in, 118 RPM, 1.4m hoop fit |

### 2.2 Bolting Silk Mesh Reports (`bolting_silk_mesh_reports.json`)

| Record ID | Sifter Reel ID | Silk Gauze Grade | Aperture (µm) | Extraction Yield (%) | Relative Timestamp | Truth Class | Candidate Location | Measurement Summary |
|---|---|---|:---:|:---:|---|---|---|---|
| `bolting_silk_gauze_number_mesh_selection` | `PRIMARY_PATENT_FLOUR_BOLTER_01` | No. 10 XX Swiss Bolting Silk | 129.0 | 72.5% | `YEAR_02_BOLTING_01` | TechnicalCalibrationLog | `room_common_mess_hall` / `loc_grain_silo` | 129 µm, 72.5% yield |
| `bolting_silk_centrifugal_reel_beater_tear` | `CENTRIFUGAL_FORCE_REEL_02` | Heavy Duty Dutch Silk 12XX | 105.0 | 68.0% | `YEAR_04_BOLTING_03` | OperationalIncident | `loc_grain_silo` | 105 µm, 68.0% yield, 220 RPM tear |
| `bolting_silk_flour_mite_infestation_pore_clog` | `STORED_FLOUR_SIFTER_UNIT_03` | Standard Bolting Cloth 8X | 180.0 | 55.0% | `YEAR_07_BOLTING_02` | BiologicalStorageAudit | `loc_grain_silo` / `loc_settlement_silo_burrow` | 180 µm, 55.0% yield, 75% RH mites |
| `bolting_silk_static_electricity_brush_grounding` | `DRY_CLIMATE_PLANSIFTER_01` | Triple Extra Silk 11XXX | 118.0 | 74.0% | `YEAR_09_BOLTING_04` | EngineeringDiagnostic | `room_workshop` | 118 µm, 74.0% yield, 18% RH charge |
| `bolting_silk_plansifter_gyratory_counterweight_wobble` | `FREE_SWINGING_PLANSIFTER_BAY` | High Tension Silk No. 9 | 150.0 | 78.0% | `YEAR_12_BOLTING_01` | MaintenanceObservation | `room_workshop` | 150 µm, 78.0% yield, elliptical wobble |
| `bolting_silk_middlings_purifier_air_current_aspiration` | `MIDDLINGS_PURIFIER_DECK_04` | Graduated Aperture Purifier Silk | 220.0 | 82.0% | `YEAR_15_BOLTING_03` | TechnicalCalibrationLog | `loc_settlement_silo_burrow` | 220 µm, 82.0% yield, aspiration lift |
| `bolting_silk_nylon_synthetic_monofilament_upgrade` | `MODERNIZED_SIFTING_CHEST_01` | Polyamide Monofilament 120T | 120.0 | 76.5% | `YEAR_17_BOLTING_02` | EngineeringUpgradeLog | `room_workshop` | 120 µm, 76.5% yield, 3x lifespan |
| `bolting_silk_bran_duster_wire_gauze_scour` | `TERMINAL_BRAN_DUSTER_UNIT` | Phosphor Bronze Wire Gauze 40M | 380.0 | 86.0% | `YEAR_20_BOLTING_01` | TechnicalRecoveryLog | `loc_grain_silo` | 380 µm, 86.0% yield, +3.5% recovery |

### 2.3 Grain Silo Weevil Audits (`grain_silo_weevil_audits.json`)

| Record ID | Grain Silo Bin ID | Crop Species | Moisture (%) | Temp (°C) | Relative Timestamp | Truth Class | Candidate Location | Measurement Summary |
|---|---|---|:---:|:---:|---|---|---|---|
| `grain_silo_granary_weevil_larva_hollow_berry` | `DEEP_STORAGE_SILO_ALPHA_01` | Hard Red Winter Wheat | 13.8% | 24.5°C | `YEAR_03_SILO_LOG_01` | BiologicalInspectionReport | `loc_grain_silo` | 13.8% moisture, 24.5°C, -14 kg/hL |
| `grain_silo_thermal_convection_moisture_migration` | `REINFORCED_CONCRETE_SILO_03` | Spring Durum Wheat | 17.2% | 12.0°C | `YEAR_05_SILO_LOG_03` | PhysicalStorageAudit | `loc_settlement_silo_burrow` | 17.2% moisture, 12.0°C, 20cm crust |
| `grain_silo_carbon_dioxide_inert_gas_asphyxiation` | `HERMETIC_STEEL_SILO_CELL_04` | Hulled Malting Barley | 11.5% | 16.0°C | `YEAR_08_SILO_LOG_02` | TechnicalSanitationLog | `loc_agricultural_outpost` | 11.5% moisture, 16.0°C, 65% CO2 |
| `grain_silo_mycotoxin_aflatoxin_hotspot_probe` | `EMERGENCY_RESERVE_SILO_02` | Yellow Dent Maize | 19.5% | 48.0°C | `YEAR_11_SILO_LOG_04` | BiologicalHazardReport | `loc_grain_silo` | 19.5% moisture, 48.0°C spike, Aspergillus |
| `grain_silo_diatomaceous_earth_desiccant_dusting` | `CENTRAL_COMMISSARY_GRAIN_ELEVATOR` | Soft White Wheat | 12.2% | 18.0°C | `YEAR_14_SILO_LOG_01` | ChemicalPestControlLog | `loc_settlement_silo_burrow` | 12.2% moisture, 18.0°C, 0.1% desiccant |
| `grain_silo_concrete_hopper_funnel_flow_rat_hole` | `CONICAL_DISCHARGE_BIN_05` | Raw Field Rye | 16.0% | 15.0°C | `YEAR_17_SILO_LOG_03` | EngineeringDiagnostic | `loc_grain_silo` | 16.0% moisture, 15.0°C, 120t stagnant |
| `grain_silo_pneumatic_grain_turnover_aeration` | `MAIN_STORAGE_BATTERY_SILO_06` | Hard Red Spring Wheat | 13.0% | 14.0°C | `YEAR_20_SILO_LOG_01` | TechnicalMaintenanceLog | `loc_settlement_silo_burrow` | 13.0% moisture, 14.0°C, -8°C chill |

### 2.4 Mill Dampener Tempering Assays (`mill_dampener_tempering_assays.json`)

| Record ID | Conditioning Bin ID | Water Addition (%) | Target Moisture (%) | Dwell (hrs) | Relative Timestamp | Truth Class | Candidate Location | Measurement Summary |
|---|---|:---:|:---:|:---:|---|---|---|---|
| `mill_tempering_hard_wheat_bran_toughening` | `PRIMARY_TEMPERING_SILO_01` | 3.5% | 15.5% | 24.0h | `YEAR_02_TEMPER_LOG_01` | TechnicalConditioningLog | `loc_grain_silo` / `room_greenhouse` | +3.5% water, 15.5% target, 24h dwell |
| `mill_tempering_conditioning_rest_bin_dwell_time` | `SECONDARY_REST_BIN_BAY_02` | 2.8% | 14.8% | 18.0h | `YEAR_05_TEMPER_LOG_03` | TechnicalConditioningLog | `loc_settlement_silo_burrow` | +2.8% water, 14.8% target, 18h dwell |
| `mill_tempering_hydrothermal_hot_water_scour` | `HYDROTHERMAL_SCOURER_SKID` | 4.0% | 16.0% | 8.0h | `YEAR_08_TEMPER_LOG_02` | TechnicalSanitationLog | `room_workshop` / `loc_grain_silo` | +4.0% water, 16.0% target, 52°C, 8h |
| `mill_tempering_endosperm_mellowing_starch_damage` | `PRECISION_MILLING_BIN_03` | 3.0% | 15.2% | 20.0h | `YEAR_11_TEMPER_LOG_04` | TechnicalConditioningLog | `loc_agricultural_outpost` | +3.0% water, 15.2% target, 20h dwell, -18% kWh |
| `mill_tempering_over_wetting_paste_choke_fluting` | `EXPERIMENTAL_WET_MILL_CELL` | 6.5% | 18.5% | 12.0h | `YEAR_14_TEMPER_LOG_01` | OperationalIncident | `room_workshop` | +6.5% water, 18.5% target, fluting choke |
| `mill_tempering_pin_mill_entoleter_insect_egg_shatter` | `ENTOLETER_IMPACT_STATION` | 0.0% | 14.5% | 0.0h | `YEAR_17_TEMPER_LOG_03` | EngineeringPestControlLog | `loc_settlement_silo_burrow` | 0.0% water, 90 m/s impact pin shatter |
| `mill_tempering_automatic_rotor_dampener_spray_jet` | `CONTINUOUS_INTENSIVE_DAMPENER` | 3.2% | 15.0% | 16.0h | `YEAR_20_TEMPER_LOG_01` | EngineeringUpgradeLog | `loc_grain_silo` | +3.2% water, 15.0% target, 4-bar atomization |

---

## 3. Repeated Facilities Analysis (Workstream A1)

Analysis of the 30 entries reveals consistent chronological tracking of core wasteland food-processing infrastructure across a 20-year post-exchange timeline (`YEAR_02` to `YEAR_20`):

1. **Central Grain Exchange & Elevator Complex (`loc_grain_silo`)**:
   - Houses `PRIMARY_BURR_RUNNER_PAIR_01` (Year 02), `PRIMARY_PATENT_FLOUR_BOLTER_01` (Year 02), `DEEP_STORAGE_SILO_ALPHA_01` (Year 03), `COMMISSARY_FLOUR_MILL_01` (Year 07), `EMERGENCY_RESERVE_SILO_02` (Year 11), `TERMINAL_BRAN_DUSTER_UNIT` (Year 20), and `CONTINUOUS_INTENSIVE_DAMPENER` (Year 20).
   - This records the emergence, degradation, pest crisis, and eventual technological retrofitting of the district's primary grain hub.
2. **New Ceres Silo Collective (`loc_settlement_silo_burrow`)**:
   - Houses `REINFORCED_CONCRETE_SILO_03` (Year 05), `SECONDARY_REST_BIN_BAY_02` (Year 05), `CENTRAL_COMMISSARY_GRAIN_ELEVATOR` (Year 14), `MIDDLINGS_PURIFIER_DECK_04` (Year 15), `ENTOLETER_IMPACT_STATION` (Year 17), and `MAIN_STORAGE_BATTERY_SILO_06` (Year 20).
   - Reflects an organized agricultural commune advancing from passive concrete storage and mold issues toward mechanized diatomaceous dusting, mechanical egg-shattering entoleters, and pneumatic turnover.
3. **Shelter Workshop & Foundry Facilities (`room_workshop`, `room_foundry`)**:
   - Houses `RESERVE_GRIST_MILL_RUNNER_02` (Year 04), `DRY_CLIMATE_PLANSIFTER_01` (Year 09), `PRECISION_SEMOLINA_STONE_04` (Year 12), `EXPERIMENTAL_WET_MILL_CELL` (Year 14), `MODERNIZED_SIFTING_CHEST_01` (Year 17), and `HEAVY_FOUNDRY_MILL_RUNNER_01` (Year 20).
   - Highlights the shelter's mechanical fabrication capacity: spindle balancing with molten lead, grounding electrostatic sieves, repairing bridge-trees, and thermal shrink-fitting massive iron rings onto segmented burr stones.

---

## 4. Contradiction & Plausibility Audit (Workstream A2)

- **Moisture vs. Biological Activity**:
  - Grain stored at 11.5% (`HERMETIC_STEEL_SILO_CELL_04`) and 12.2% (`CENTRAL_COMMISSARY_GRAIN_ELEVATOR`) is below the 13.5% critical threshold for mold growth, which aligns with standard cereal science.
  - High moisture readings of 17.2% (`REINFORCED_CONCRETE_SILO_03`) and 19.5% (`EMERGENCY_RESERVE_SILO_02`) accurately report top crusting, fungal proliferation, and self-heating hotspots (48°C), validating biological realism.
- **Tempering Hydration**:
  - Tempering additions of 2.8%–4.0% aiming for 14.5%–16.0% moisture match standard industrial wheat tempering practice (hydrating the pericarp without softening the vitreous endosperm).
  - The outlier entry `mill_tempering_over_wetting_paste_choke_fluting` (6.5% addition, 18.5% moisture) is explicitly authored as a malfunction that clogged the roller mill fluting, confirming internal narrative consistency.
- **Flour Extraction Rates**:
  - Extraction rates range from 55% (mite-clogged reels) to 72.5% (standard patent flour), 78% (straight grade), 82% (semolina middlings), and 86% (bran duster scouring). These numbers reflect authentic milling yields without fantasy inflation.
