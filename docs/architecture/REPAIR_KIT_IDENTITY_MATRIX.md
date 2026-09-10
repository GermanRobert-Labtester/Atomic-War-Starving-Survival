# REPAIR KIT IDENTITY MATRIX

## 1. Architectural Policy on Repair Kit Identifiers
Each of the 20 canonical glitches in `narrative/bunker_maintenance_glitches.json` specifies an array of `required_repair_kit` items (totaling 79 authored token strings).

**Core Rule**: These 79 tokens represent diegetic narrative apparatus and field-expedient engineer tools. They are **NOT** automatically deducted from player inventory when reading, unlocking, or resolving a glitch. They serve as rich atmospheric descriptions of what Chief Engineer Dmitri and his shift crew needed to fix the emergency.

If a future crafting or repair mechanic allows players to enact physical maintenance on these subsystems, the tokens map to the canonical functional crafting categories and existing inventory equivalents documented below.

---

## 2. Complete 79-Item Token Mapping

| Glitch ID | Authored Token String | Descriptive Name | In `items.json`? | Functional Component Fallback |
|---|---|---|---|---|
| `glitch_01` | `item_heavy_welding_rig` | Heavy Oxy-Acetylene Welding Rig | No (Narrative) | `tool_toolkit` / `scrap_metal` |
| `glitch_01` | `item_asbestos_gasket_ring` | High-Temp Asbestos Flange Gasket | No (Narrative) | `scrap_cloth` / `chemical_reagents` |
| `glitch_01` | `item_cast_iron_clamp_collar` | Cast-Iron Two-Piece Pipe Collar | No (Narrative) | `scrap_metal` |
| `glitch_01` | `item_molybdenum_solder_rod` | High-Strength Molybdenum Solder | No (Narrative) | `scrap_metal` / `solder` |
| `glitch_02` | `item_diaphragm_surge_accumulator` | Rubber Surge Dampener Bladder | No (Narrative) | `rubber` / `scrap_metal` |
| `glitch_02` | `item_brass_snifting_valve` | Brass Air-Bleed Snifting Valve | No (Narrative) | `scrap_metal` (brass) |
| `glitch_02` | `item_pipe_spanner_heavy` | Heavy Cast-Steel Pipe Spanner | No (Narrative) | `tool_toolkit` |
| `glitch_02` | `item_hydrometric_pressure_gauge` | High-Pressure Bourdon Gauge | No (Narrative) | `electronics` / `precision_tool` |
| `glitch_03` | `item_copper_grounding_rod` | Solid Copper Grounding Spike | No (Narrative) | `scrap_metal` (copper) |
| `glitch_03` | `item_selenium_rectifier_diode` | High-Voltage Selenium Rectifier Stack | No (Narrative) | `electronics` |
| `glitch_03` | `item_multimeter_tester` | Analog Galvanometer Multimeter | No (Narrative) | `electronics` / `tool_toolkit` |
| `glitch_03` | `item_heavy_insulated_pliers` | 10kV Insulated Lineman's Pliers | No (Narrative) | `tool_toolkit` |
| `glitch_04` | `item_hepa_filter_cartridge` | Pleated Sub-Micron HEPA Cartridge | No (Narrative) | `item_lead_lined_effluent_filter` / `filter_media` |
| `glitch_04` | `item_activated_charcoal_tray` | Granular Activated Charcoal Bed | No (Narrative) | `charcoal` |
| `glitch_04` | `item_uv_c_germicidal_lamp` | Quartz UV-C Sterilization Tube | No (Narrative) | `electronics` / `glass` |
| `glitch_04` | `item_rubber_hazmat_suit` | Vulcanized Rubber Hazmat Coverall | No (Narrative) | `equipment_hazmat` / `rubber` |
| `glitch_05` | `item_heavy_slice_bar` | 3-Meter Forged Steel Slice Bar | No (Narrative) | `scrap_metal` |
| `glitch_05` | `item_pneumatic_chipping_hammer` | Pneumatic Scaling Chisel | No (Narrative) | `tool_toolkit` |
| `glitch_05` | `item_refractory_fireclay_patch` | Alumina Refractory Fireclay Mix | No (Narrative) | `clay` / `mineral` |
| `glitch_05` | `item_welding_goggles` | Shade-5 Oxy-Fuel Cutting Goggles | No (Narrative) | `protective_eyewear` |
| `glitch_06` | `item_stainless_float_switch` | Stainless Steel Magnetic Float | No (Narrative) | `scrap_metal` / `electronics` |
| `glitch_06` | `item_centrifugal_macerator_blade` | Hardened Tool Steel Macerator Cutter | No (Narrative) | `scrap_metal` / `tool_blade` |
| `glitch_06` | `item_rubber_waders` | High-Chest Rubber Drainage Waders | No (Narrative) | `rubber` / `protective_gear` |
| `glitch_06` | `item_disinfectant_spray_pump` | Manual Bleach Sanitizing Sprayer | No (Narrative) | `chemical_reagents` |
| `glitch_07` | `item_diesel_injector_nozzle` | Micro-Orifice Diesel Pintle Nozzle | No (Narrative) | `precision_parts` / `scrap_metal` |
| `glitch_07` | `item_copper_crush_washer` | Annealed Copper Crush Gasket | No (Narrative) | `scrap_metal` (copper) |
| `glitch_07` | `item_torque_wrench_calibrated` | Micrometer Click-Type Torque Wrench | No (Narrative) | `tool_toolkit` |
| `glitch_07` | `item_carburetor_cleaning_solvent` | Aromatic Hydrocarbon Degreaser | No (Narrative) | `chemical_reagents` / `solvent` |
| `glitch_08` | `item_insulating_varnish_can` | Glyptal Electrical Baking Varnish | No (Narrative) | `chemical_reagents` |
| `glitch_08` | `item_solid_state_mosfet_driver` | Silicon Gate Switching Transistor | No (Narrative) | `electronics` |
| `glitch_08` | `item_soldering_iron_copper` | Heavy Copper Soldering Hatchet | No (Narrative) | `tool_toolkit` |
| `glitch_08` | `item_neoprene_isolation_pads` | Anti-Vibration Neoprene Mounts | No (Narrative) | `rubber` |
| `glitch_09` | `item_hydraulic_chevron_seal_kit` | Multi-Lip Polyurethane Chevron Rings | No (Narrative) | `rubber` / `seals` |
| `glitch_09` | `item_mineral_hydraulic_oil_drum` | ISO-46 Anti-Wear Hydraulic Fluid | No (Narrative) | `oil_fuel` |
| `glitch_09` | `item_spanner_wrench_set` | Metric Open-End Spanner Set | No (Narrative) | `tool_toolkit` |
| `glitch_09` | `item_degreaser_rag_pack` | Cotton Shop Cleaning Rags | No (Narrative) | `scrap_cloth` |
| `glitch_10` | `item_synthetic_ruby_pallet_stone` | Synthetic Corundum Pallet Jewel | No (Narrative) | `precision_parts` / `gem` |
| `glitch_10` | `item_watchmakers_loupe` | 10x Optical Jeweler's Loupe | No (Narrative) | `precision_tool` |
| `glitch_10` | `item_precision_tweezers` | Antimagnetic Fine Forceps | No (Narrative) | `tool_toolkit` |
| `glitch_10` | `item_synthetic_clock_oil` | Micro-Viscosity Horological Lubricant | No (Narrative) | `oil_fuel` / `chemical_reagents` |
| `glitch_11` | `item_soda_lime_granule_canister` | Indicating Soda Lime CO2 Absorbent | No (Narrative) | `chemical_reagents` |
| `glitch_11` | `item_baffle_compression_screen` | Perforated Brass Retention Grille | No (Narrative) | `scrap_metal` |
| `glitch_11` | `item_co2_draeger_tube_meter` | Colorimetric Gas Detector Bellows | No (Narrative) | `precision_tool` / `detector` |
| `glitch_11` | `item_vibratory_tamper_tool` | Pneumatic Granule Packing Tamper | No (Narrative) | `tool_toolkit` |
| `glitch_12` | `item_concentrated_glycol_carboy` | Pure Mono-Ethylene Glycol | No (Narrative) | `chemical_reagents` / `coolant` |
| `glitch_12` | `item_refrigeration_freon_tank` | R-12 Halocarbon Refrigerant Cylinder | No (Narrative) | `gas_canister` |
| `glitch_12` | `item_vacuum_dehydration_pump` | Rotary Vane Deep Vacuum Pump | No (Narrative) | `tool_toolkit` / `machinery` |
| `glitch_12` | `item_refractometer_tester` | Optical Brix/Coolant Refractometer | No (Narrative) | `precision_tool` |
| `glitch_13` | `item_flexible_plumbers_snake_rod` | Tempered Spring-Steel Drain Snake | No (Narrative) | `tool_toolkit` |
| `glitch_13` | `item_rubber_suction_puck` | Vacuum Pressure Driving Disc | No (Narrative) | `rubber` |
| `glitch_13` | `item_compressed_air_quick_tap` | Brass Quick-Disconnect Air Chuck | No (Narrative) | `scrap_metal` |
| `glitch_13` | `item_replacement_felt_gaskets` | High-Density Wool Sealing Skirt | No (Narrative) | `scrap_cloth` |
| `glitch_14` | `item_thermostatic_mixing_cartridge` | Copper Wax-Pellet Thermal Actuator | No (Narrative) | `precision_parts` / `scrap_metal` |
| `glitch_14` | `item_descaling_acid_solution` | Sulfamic Acid Rust/Lime Remover | No (Narrative) | `chemical_reagents` |
| `glitch_14` | `item_brass_check_valve` | One-Way Swing Check Flapper | No (Narrative) | `scrap_metal` (brass) |
| `glitch_14` | `item_pipe_thread_sealant` | Anaerobic PTFE Pipe Joint Paste | No (Narrative) | `chemical_reagents` / `glue` |
| `glitch_15` | `item_borosilicate_dielectric_tube` | Heavy-Wall Pyrex Ozone Reactor Tube | No (Narrative) | `glass` / `electronics` |
| `glitch_15` | `item_stainless_mesh_electrode` | Woven 316 Stainless Mesh Sleeve | No (Narrative) | `scrap_metal` |
| `glitch_15` | `item_high_voltage_silicone_insulator` | 15kV Molded Silicone Standoff | No (Narrative) | `electronics` / `rubber` |
| `glitch_15` | `item_activated_carbon_respirator` | Cartridge Respirator with Acid-Gas Core | No (Narrative) | `charcoal` / `mask` |
| `glitch_16` | `item_regenerated_silica_gel_cartridge` | Indicating Cobalt-Chloride Silica Desiccant | No (Narrative) | `chemical_reagents` |
| `glitch_16` | `item_dry_nitrogen_purge_bottle` | High-Purity Dry Nitrogen Cylinder | No (Narrative) | `gas_canister` |
| `glitch_16` | `item_optical_lens_paper` | Lint-Free Lens Cleaning Wipes | No (Narrative) | `scrap_cloth` / `paper` |
| `glitch_16` | `item_pure_ethanol_cleaner` | 99.5% Anhydrous Ethyl Alcohol | No (Narrative) | `chemical_reagents` / `alcohol` |
| `glitch_17` | `item_caustic_soda_lye_pellets` | Sodium Hydroxide Flakes | No (Narrative) | `chemical_reagents` |
| `glitch_17` | `item_grease_skimming_ladle` | Perforated Cast-Iron Fat Ladle | No (Narrative) | `tool_toolkit` / `scrap_metal` |
| `glitch_17` | `item_one_way_drain_check_valve` | Weighted Sewer Backwater Valve | No (Narrative) | `scrap_metal` |
| `glitch_17` | `item_heavy_rubber_gloves` | Gauntlet Nitrile Chemical Gloves | No (Narrative) | `rubber` / `protective_gear` |
| `glitch_18` | `item_heavy_copper_jumper_cable` | 50mm² Flexible Copper Bypass Strap | No (Narrative) | `scrap_metal` (copper) |
| `glitch_18` | `item_baking_soda_neutralizer_bucket` | Sodium Bicarbonate Slurry Powder | No (Narrative) | `chemical_reagents` |
| `glitch_18` | `item_explosion_proof_exhaust_fan` | Spark-Proof Centrifugal Vent Blower | No (Narrative) | `machinery` / `electronics` |
| `glitch_18` | `item_insulated_terminal_wrench` | 1000V Rated Box-End Spanner | No (Narrative) | `tool_toolkit` |
| `glitch_19` | `item_mercury_arc_rectifier_bulb` | Glass Bulb Mercury Arc Cathode | No (Narrative) | `electronics` / `glass` |
| `glitch_19` | `item_sand_filled_ceramic_fuse` | High-Rupture-Capacity 10A Fuse | No (Narrative) | `electronics` |
| `glitch_19` | `item_cooling_blower_fan` | Axial Tube-Cooled Blower | No (Narrative) | `machinery` / `electronics` |
| `glitch_19` | `item_high_voltage_spark_gap` | Tunable Tungsten Sphere Gap | No (Narrative) | `electronics` / `scrap_metal` |
| `glitch_20` | `item_graphite_tallow_grease_gun` | High-Pressure Lever Lubricator | No (Narrative) | `tool_toolkit` / `oil_fuel` |
| `glitch_20` | `item_ceremonial_brass_wrench` | Polished Cast-Brass Spanner | No (Narrative) | `tool_toolkit` / `scrap_metal` (brass) |
| `glitch_20` | `item_founders_day_flower_garland` | Dried Alpine Wildflowers & Ribbon | No (Narrative) | `organic_plants` / `herb` |
