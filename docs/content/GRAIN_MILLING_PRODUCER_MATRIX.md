# Grain Milling Producer Matrix & Discovery Paths

## 1. Overview

This document specifies the discovery trigger architecture for ASHFALL's 30 `GrainMillingCatalog` records. In compliance with **Plan 157 §9 (Workstream D)**, every catalog family is connected to at least one active, playable producer path without fabricating new map locations or duplicating existing survival loops.

A total of **20 records are actively wired** into primary gameplay discovery paths across five canonical producer types. The remaining **10 records are safely deferred** to future wasteland expansions, deep-archive recovery, or advanced faction barter.

---

## 2. Five Canonical Producer Archetypes

1. **Silo Inspection (`silo_inspection`)**:
   - Triggered upon discovering or surveying major bulk agricultural infrastructure (`loc_grain_silo`, `loc_settlement_silo_burrow`, or shelter silo `grain_silo_holdfast`).
   - Primary Family: **Grain Silo Weevil Audits**.
2. **Mill / Workshop Inspection (`mill_workshop_inspection`)**:
   - Triggered through shelter machine maintenance in `room_workshop` or exploring industrial/quarry sites (`loc_quarry`).
   - Primary Family: **Burr Millstone Dressing Logs**.
3. **Agricultural Settlement Records (`agricultural_settlement_records`)**:
   - Triggered when establishing contact or visiting agrarian settlements (`loc_settlement_silo_burrow`, `loc_agricultural_outpost`, or shelter `room_greenhouse`).
   - Primary Family: **Mill Dampener Tempering Assays**.
4. **Sifter & Bolting Maintenance (`bolting_maintenance`)**:
   - Triggered by inspecting food-processing machinery in shelter kitchens (`room_common_mess_hall`) or central milling bolters (`loc_grain_silo`).
   - Primary Family: **Bolting Silk Mesh Reports**.
5. **Trade & Scavenging Discovery (`trade_scavenging_inspection`)**:
   - Triggered through grain/flour trade inspection at the Grain Silo Exchange (`loc_grain_silo`) or industrial salvage operations (`loc_salvage_yard`).
   - Cross-Family Discovery: Technical manuals, recovery duster reports, and high-tech conditioning skids.

---

## 3. Comprehensive Record Routing Matrix

| Record ID | Family | Producer Archetype | Canonical Producer ID | Min Day | Status | Discovery Trigger & Narrative Context |
|---|---|---|---|:---:|:---:|---|
| `burr_millstone_french_chert_chisel_cracking` | Millstone | Mill / Workshop | `room_workshop` | 1 | **Active** | Dressing manual found in shelter tool rack. |
| `burr_millstone_runner_stone_rynd_balance_lead` | Millstone | Mill / Workshop | `loc_quarry` | 5 | **Active** | Slate quarry workers' notes on runner stone balancing. |
| `burr_millstone_furrow_lands_feather_edge_wear` | Millstone | Mill / Workshop | `loc_grain_silo` | 8 | **Active** | Grist mill maintenance log recovered behind the scale. |
| `burr_millstone_spindle_footstep_bearing_lignum_vitae` | Millstone | Mill / Workshop | `loc_quarry` | 12 | **Active** | Heavy water-mill bearing archive recovered in slate caves. |
| `burr_millstone_bridge_tree_tentering_screw_chatter` | Millstone | Mill / Workshop | `room_workshop` | 15 | **Active** | Workshop mechanical vibration troubleshooting log. |
| `burr_millstone_swallow_eye_centrifugal_feed_shoe` | Millstone | Mill / Workshop | `loc_settlement_silo_burrow` | 18 | **Active** | New Ceres millwright log detailing hopper feed shoe. |
| `burr_millstone_flour_hoop_leather_sweeper_brush` | Millstone | Mill / Workshop | `loc_grain_silo` | — | *Deferred* | Commissary mill casing inspection notes. |
| `burr_millstone_iron_banding_thermal_shrink_fit` | Millstone | Mill / Workshop | `room_foundry` | — | *Deferred* | Foundry fabrication archive for 1.4m quartz hoops. |
| `bolting_silk_gauze_number_mesh_selection` | Bolting Silk | Sifter Maintenance | `room_common_mess_hall` | 2 | **Active** | Mess hall recipe binder with Swiss silk sieve tables. |
| `bolting_silk_centrifugal_reel_beater_tear` | Bolting Silk | Sifter Maintenance | `loc_grain_silo` | 6 | **Active** | Exchange emergency repair report following beater tear. |
| `bolting_silk_flour_mite_infestation_pore_clog` | Bolting Silk | Sifter Maintenance | `loc_grain_silo` | 10 | **Active** | Sanitation report detailing Acarus siro mite outbreak. |
| `bolting_silk_static_electricity_brush_grounding` | Bolting Silk | Sifter Maintenance | `room_workshop` | 14 | **Active** | Winter electrostatic discharge and grounding notes. |
| `bolting_silk_plansifter_gyratory_counterweight_wobble` | Bolting Silk | Sifter Maintenance | `room_workshop` | 20 | **Active** | Dynamic balancing log for twelve-frame plansifter. |
| `bolting_silk_middlings_purifier_air_current_aspiration` | Bolting Silk | Sifter Maintenance | `loc_settlement_silo_burrow` | 22 | **Active** | New Ceres semolina aspiration calibration record. |
| `bolting_silk_nylon_synthetic_monofilament_upgrade` | Bolting Silk | Sifter Maintenance | `room_workshop` | — | *Deferred* | High-tech synthetic monofilament retrofit dossier. |
| `bolting_silk_bran_duster_wire_gauze_scour` | Bolting Silk | Trade / Scavenging | `loc_grain_silo` | — | *Deferred* | Exchange terminal yield recovery audit. |
| `grain_silo_granary_weevil_larva_hollow_berry` | Silo Weevil | Silo Inspection | `loc_grain_silo` | 3 | **Active** | Grain Exchange bin audit identifying Sitophilus damage. |
| `grain_silo_thermal_convection_moisture_migration` | Silo Weevil | Silo Inspection | `loc_settlement_silo_burrow` | 7 | **Active** | New Ceres silo report on cold bedrock condensation. |
| `grain_silo_carbon_dioxide_inert_gas_asphyxiation` | Silo Weevil | Silo Inspection | `loc_agricultural_outpost` | 11 | **Active** | Hermetic silo sealed atmosphere log. |
| `grain_silo_mycotoxin_aflatoxin_hotspot_probe` | Silo Weevil | Silo Inspection | `loc_grain_silo` | 16 | **Active** | Alarming thermocouple thermal spike hazard log. |
| `grain_silo_diatomaceous_earth_desiccant_dusting` | Silo Weevil | Silo Inspection | `loc_settlement_silo_burrow` | 19 | **Active** | Natural silica desiccant treatment record. |
| `grain_silo_concrete_hopper_funnel_flow_rat_hole` | Silo Weevil | Silo Inspection | `loc_grain_silo` | — | *Deferred* | Hopper bridging and rat-holing structural report. |
| `grain_silo_pneumatic_grain_turnover_aeration` | Silo Weevil | Silo Inspection | `loc_settlement_silo_burrow` | — | *Deferred* | Deep winter pneumatic grain turnover log. |
| `mill_tempering_hard_wheat_bran_toughening` | Tempering | Agrarian Records | `room_greenhouse` | 2 | **Active** | Greenhouse post-harvest grain hydration guide. |
| `mill_tempering_conditioning_rest_bin_dwell_time` | Tempering | Agrarian Records | `loc_settlement_silo_burrow` | 6 | **Active** | New Ceres timber rest bin diffusion study. |
| `mill_tempering_hydrothermal_hot_water_scour` | Tempering | Agrarian Records | `room_workshop` | 9 | **Active** | Hydrothermal scourer skid maintenance log. |
| `mill_tempering_endosperm_mellowing_starch_damage` | Tempering | Agrarian Records | `loc_agricultural_outpost` | 13 | **Active** | Energy conservation and starch mellowing assay. |
| `mill_tempering_over_wetting_paste_choke_fluting` | Tempering | Agrarian Records | `room_workshop` | 17 | **Active** | Incident report on roller mill spiral fluting choke. |
| `mill_tempering_pin_mill_entoleter_insect_egg_shatter` | Tempering | Agrarian Records | `loc_settlement_silo_burrow` | — | *Deferred* | Entoleter impact mechanical sanitization blueprint. |
| `mill_tempering_automatic_rotor_dampener_spray_jet` | Tempering | Agrarian Records | `loc_grain_silo` | — | *Deferred* | Intensive rotor dampener spray manifold diagram. |

---

## 4. Activation Summary

- **Total Catalog Entries**: 30 records
- **Active Discovery Records (Phase 1)**: 20 records (5 Millstone, 6 Bolting Silk, 5 Silo Weevil, 5 Tempering)
- **Deferred Records (Future Expansions)**: 10 records (3 Millstone, 2 Bolting Silk, 2 Silo Weevil, 2 Tempering)
- **Producer Diversity**:
  - Shelter Rooms: `room_workshop` (5), `room_common_mess_hall` (1), `room_greenhouse` (1)
  - Wasteland Locations: `loc_grain_silo` (5), `loc_settlement_silo_burrow` (5), `loc_quarry` (2), `loc_agricultural_outpost` (2)
