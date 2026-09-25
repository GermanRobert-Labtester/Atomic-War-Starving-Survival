import os

plan_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/piagentsplans/04-relic-blueprint-expansion.md"

header = """# Plan 04 — Workshop Relic Blueprint Expansion, Reverse-Engineering Physics & Pre-War Technological Archeology

**Package:** `PLAN-04-RELIC-BLUEPRINT-EXPANSION`
**Document Class:** Master System Architecture, Blueprint Catalog & Production Integration Blueprint
**Authority Level:** Canonical Production Plan
**Target Runtimes:** Ashfall.Core (`netstandard2.1`, Engine-Free) · Godot Host (`net8.0`) · Ashfall.Core.Tests (`net9.0`)
**Data Authority:** `Assets/StreamingAssets/Data/` (relic_recipes.json, items_relics.json, schema_version: 1, snake_case)
**Historical Anchor:** piagentsplans Wave 1 (2026-08-30) · Workshop & Crafting Progression Suite · Master Authority Volumes 4, 15, 22, 36, 44
**Save Authority:** Checksummed Section `workshop_relic_blueprints` via `SaveStoreHub` (Section 154)
**Determinism Mandate:** Pure Domain Invariants under `ISeededRng` / `SeededRng.Fork("workshop_relics")`; Zero Wall-Clock reads; Zero `System.Random`

---

# SECTION I: COMPREHENSIVE ARCHITECTURAL OBJECTIVES & SYSTEM TOPOLOGY

Plan 04 transforms pre-war relics from simple vendor trash into the central technological progression spine of *ASHFALL*. Surviving in the fallout-wracked wasteland requires rediscovering, dismantling, and reverse-engineering complex pre-war machinery—from micro-dosimeters and water-condenser coils to automated perimeter defense turrets and geothermal heat exchanger loops.

This architecture formalizes the full technical lifecycle: `Expedition Scavenge` -> `Forensic Inspection` -> `Workshop Teardown & Tool Wear` -> `Blueprint Decoding` -> `Shelter Manufacturing`:

```
+===================================================================================================+
|                                     DISCOVERED PRE-WAR RELIC                                      |
|   Tier 1 (Civilian Salvage) · Tier 2 (Industrial / Clinical) · Tier 3 (Classified Military Stratum)|
+===================================================================================================+
                                                  │
                                                  ▼
+===================================================================================================+
|                        ASHFALL CORE WORKSHOP REVERSE-ENGINEERING ENGINE                           |
|  Assets/Ashfall.Core/Crafting/ & Assets/Ashfall.Core/Technology/                                  |
|  - RelicTeardownEngine (Tool Wear, Power Draw, Explosive Catastrophe Risk)                        |
|  - BlueprintRegistry (Permille Decoding Progress, Latent Skill Interactions)                      |
|  - MaterialDisassemblyYields (Exotic Capacitors, Optical Crystals, Rare-Earth Alloys)              |
|  - Pure Domain Logic - 100% Engine-Free (Zero Godot/UnityEngine References)                       |
+===================================================================================================+
        │                                         │                                      │
        ▼                                         ▼                                      ▼
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
| SALVAGED RAW COMPONENTS   |   | PERMANENT UNLOCKED BLUEPRINTS     |   | RESEARCH CODEX ADVANCES   |
| - High-Grade Copper Wire  |   | - 60 Authored Tech Schematics     |   | - Tech Tree Branch Unlocks|
| - Vacuum Tubes & Relays   |   | - Micro-Dosimeter Mk3             |   | - Latent Expertise XP     |
| - Lead Shielding Plates   |   | - Hydroponic Nutrient Synthesizer |   | - Engineering Mentorship  |
+───────────────────────────+   +───────────────────────────────────+   +───────────────────────────+
        │                                         │                                      │
        └─────────────────────────────────────────┼──────────────────────────────────────┘
                                                  ▼
+===================================================================================================+
|                          GODOT WORKSHOP PRESENTATION & BENCH UI SEAM                              |
|  src/UI/WorkshopDisassemblyBenchView.cs & src/Host/WorkshopHostSession.cs                         |
|  - Interactive Exploded-View Diagrammatic Blueprint Viewer                                        |
|  - Precision Tool Wear & Power Status Needles; Haptic Vibration Feedback                          |
+===================================================================================================+
```

### 1.1 Non-Negotiable Invariants
1. **Engine Separation**: Zero references to `Godot`, `UnityEngine`, or engine hardware APIs inside `Assets/Ashfall.Core/Crafting/`.
2. **Deterministic Teardown Probabilities**: Teardown success, critical material extraction yield, and tool wear degradation are governed exclusively via `ISeededRng` keyed to survivor skill and tool condition.
3. **Data Authority**: Master recipes, material inputs, and unlocked output blueprints reside exclusively in `Assets/StreamingAssets/Data/relic_recipes.json` with `schema_version: 1`.
4. **Conservation of Mass and Materials**: Dismantling a relic consumes the relic item from shelter inventory and deposits discrete component items into inventory ledgers. No phantom assets are created.

---

# SECTION II: THE THREE TECHNOLOGICAL TIERS & TEARDOWN MECHANICS

### 2.1 Technological Relic Stratification
1. **Tier 1: Commercial & Civilian Salvage (Difficulty: 15 - 35)**:
   - *Archetypes*: Vacuum tube radio receivers, mechanical water condensers, lead-acid battery reconditioners, kerosene incubator heaters, manual sewing and loom assemblies.
   - *Tooling Required*: Basic Hand Tools (`tool_bench_vise`, `tool_soldering_iron_basic`). Power Draw: 50 - 150 Watts.
   - *Failure Consequence*: Stripped screws, cracked bakelite casing, -25% component yield.

2. **Tier 2: Heavy Industrial & Clinical Relics (Difficulty: 40 - 70)**:
   - *Archetypes*: Micro-dosimeter scintillators, automated autoclave sterilizers, centrifuge blood separators, geothermal coolant recirculation pumps, hydraulic presses.
   - *Tooling Required*: Precision Instrumentation (`tool_multimeter_calibrated`, `tool_lathe_metalworking`, `tool_optics_bench`). Power Draw: 250 - 800 Watts.
   - *Failure Consequence*: Blown capacitors, shattered quartz crystals, toxic chemical aerosol release (local medical hazard).

3. **Tier 3: Classified Pre-War Military Stratum Tech (Difficulty: 75 - 100)**:
   - *Archetypes*: Active-phased array radar heads, laser rangefinder guidance optics, automated sentry turret fire-control modules, subterranean seismic geophone sensors, thermal imaging sights.
   - *Tooling Required*: Master Cleanroom Tech (`tool_oscilloscope_crt`, `tool_inert_gas_welder`, `tool_diamond_cutter`). Power Draw: 1,200 - 3,500 Watts.
   - *Failure Consequence*: Anti-tamper incendiary self-destruct trigger, lethal capacitor discharge, total destruction of relic with zero yield and severe survivor trauma.

### 2.2 Mathematical Formulas for Reverse-Engineering
The probability of successfully recovering a permanent blueprint $P_{\\text{blueprint}}$ during teardown is calculated as:

$$P_{\\text{blueprint}} = \\min\\left(0.95, \\max\\left(0.05, \\frac{S_{\\text{engineering}} \\cdot 1.5 + S_{\\text{science}} \\cdot 0.8 - D_{\\text{relic}}}{100.0} + C_{\\text{tool}} \\cdot 0.20\\right)\\right)$$

Where:
- $S_{\\text{engineering}}, S_{\\text{science}}$: Survivor operative skills (0 to 100).
- $D_{\\text{relic}}$: Relic complexity difficulty rating (15 to 100).
- $C_{\\text{tool}}$: Workstation tool condition ratio (0.0 to 1.0).

---

# SECTION III: AUTHORITATIVE DATA SCHEMAS (`Assets/StreamingAssets/Data/`)

### 3.1 `relic_recipes.json`
```json
{
  "schema_version": 1,
  "catalog_id": "relic_recipes_master_v1",
  "comment": "Master Catalog of 60 Pre-War Relic Teardown & Reverse-Engineering Schematics",
  "recipes": [
    {
      "recipe_id": "recipe_relic_micro_dosimeter_mk1",
      "source_relic_item_id": "item_relic_broken_dosimeter",
      "tier": 1,
      "difficulty_rating": 25,
      "base_teardown_time_minutes": 120,
      "power_consumption_watts": 80,
      "required_tools": ["tool_soldering_iron_basic", "tool_wire_stripper"],
      "unlocked_blueprint_id": "bp_crafted_micro_dosimeter_mk1",
      "yield_components": [
        { "item_id": "item_scrap_copper_wire", "count_min": 2, "count_max": 4, "probability": 1.0 },
        { "item_id": "item_geiger_muller_tube_used", "count_min": 1, "count_max": 1, "probability": 0.85 },
        { "item_id": "item_zinc_casing", "count_min": 1, "count_max": 1, "probability": 1.0 }
      ],
      "catastrophic_failure_hazard": "none"
    }
  ]
}
```

---

# SECTION IV: MASTER CATALOG OF 60 PRE-WAR RELIC BLUEPRINTS & RECIPES

The following catalog defines 60 pre-war technical relics across all three tiers, establishing a deep mid-to-late game technological rediscovery progression:

"""

# Generate 60 authored relic definitions
relic_entries = []
relic_names = [
    # Tier 1: Civilian / Commercial (1 - 20)
    ("Rotary Dial Field Telephone", "item_relic_field_phone", 1, 20, 90, 60, "bp_field_intercom_net", "tool_soldering_iron_basic"),
    ("Kerosene Radiant Brooder", "item_relic_brooder_heater", 1, 22, 110, 0, "bp_greenhouse_heater", "tool_pipe_wrench"),
    ("Mechanical Water Condenser Coil", "item_relic_condenser_coil", 1, 25, 120, 80, "bp_atmospheric_moisture_trap", "tool_tube_cutter"),
    ("Vacuum Tube Audio Pre-Amp", "item_relic_tube_preamp", 1, 28, 140, 100, "bp_radio_signal_booster_mk1", "tool_soldering_iron_basic"),
    ("Lead-Acid Battery Desulfator", "item_relic_battery_desulfator", 1, 30, 150, 120, "bp_battery_reconditioner", "tool_multimeter_calibrated"),
    ("High-Pressure Hand Boiler Valve", "item_relic_boiler_valve", 1, 32, 130, 0, "bp_steam_distillation_unit", "tool_bench_vise"),
    ("Crank-Operated Centrifugal Blower", "item_relic_hand_blower", 1, 18, 80, 0, "bp_air_filtration_crank_pump", "tool_grease_gun"),
    ("Selenium Cell Exposure Meter", "item_relic_exposure_meter", 1, 35, 160, 50, "bp_optical_radiation_indicator", "tool_precision_tweezers"),
    ("Bimetallic Thermal Regulator Switch", "item_relic_thermostat_switch", 1, 24, 100, 40, "bp_automated_vent_damper", "tool_soldering_iron_basic"),
    ("Zinc-Air Emergency Lantern Core", "item_relic_lantern_core", 1, 20, 90, 30, "bp_chemical_glow_lantern", "tool_bench_vise"),
    ("Cast-Iron Hand Grain Grinder", "item_relic_grain_grinder", 1, 15, 60, 0, "bp_industrial_food_mill", "tool_pipe_wrench"),
    ("Manual Blood Transfusion Pump", "item_relic_transfusion_pump", 1, 34, 130, 0, "bp_sterile_peristaltic_pump", "tool_precision_tweezers"),
    ("Bakelite Variable Air Capacitor", "item_relic_air_capacitor", 1, 26, 120, 70, "bp_radio_shortwave_tuner", "tool_soldering_iron_basic"),
    ("Quartz Crystal Clock Movement", "item_relic_clock_movement", 1, 30, 140, 40, "bp_master_chronometer_timer", "tool_watchmaker_loupe"),
    ("Hand-Cranked DC Magneto Dynamo", "item_relic_crank_dynamo", 1, 22, 100, 0, "bp_emergency_dynamo_charger", "tool_bench_vise"),
    ("Spring-Loaded Anemometer Head", "item_relic_anemometer_head", 1, 25, 110, 0, "bp_weather_station_vane", "tool_grease_gun"),
    ("Ceramic Porous Water Filter Candle", "item_relic_filter_candle", 1, 28, 120, 0, "bp_micro_porous_water_filter", "tool_bench_vise"),
    ("Pneumatic Piston Door Closer", "item_relic_pneumatic_closer", 1, 20, 90, 0, "bp_airlock_automatic_seal", "tool_pipe_wrench"),
    ("Reflective Parabolic Solar Cooker", "item_relic_solar_concentrator", 1, 18, 80, 0, "bp_concentrated_solar_furnace", "tool_tin_snips"),
    ("Mercury Tilt Safety Switch", "item_relic_mercury_switch", 1, 32, 130, 20, "bp_seismic_tremor_alarm", "tool_soldering_iron_basic"),

    # Tier 2: Heavy Industrial / Medical (21 - 40)
    ("Scintillation Photomultiplier Tube", "item_relic_photomultiplier", 2, 45, 180, 250, "bp_spectrometric_dosimeter_mk2", "tool_optics_bench"),
    ("Autoclave Chamber Pressure Sensor", "item_relic_pressure_transducer", 2, 48, 190, 300, "bp_high_pressure_autoclave", "tool_multimeter_calibrated"),
    ("Centrifugal Blood Hematocrit Rotor", "item_relic_centrifuge_rotor", 2, 52, 210, 400, "bp_clinical_lab_centrifuge", "tool_lathe_metalworking"),
    ("Geothermal Heat Exchanger Braid", "item_relic_heat_exchanger", 2, 55, 240, 500, "bp_deep_well_geothermal_tap", "tool_inert_gas_welder"),
    ("Industrial Peristaltic Dosing Pump", "item_relic_dosing_pump", 2, 50, 200, 350, "bp_hydroponic_nutrient_dispenser", "tool_multimeter_calibrated"),
    ("Hydraulic Proportional Flow Valve", "item_relic_hydraulic_valve", 2, 58, 220, 450, "bp_heavy_hydraulic_crane", "tool_lathe_metalworking"),
    ("High-Voltage Spark Gap Igniter", "item_relic_spark_igniter", 2, 42, 170, 280, "bp_foundry_electric_arc_furnace", "tool_soldering_iron_basic"),
    ("Germanium Diode Microwave Mixer", "item_relic_microwave_mixer", 2, 60, 250, 400, "bp_radar_motion_perimeter_alarm", "tool_optics_bench"),
    ("Pneumatic Impact Rock Drill Head", "item_relic_rock_drill_head", 2, 46, 180, 600, "bp_automated_mining_bore", "tool_lathe_metalworking"),
    ("Ultrasonic Liquid De-Gas Transducer", "item_relic_ultrasonic_transducer", 2, 54, 210, 380, "bp_fuel_refining_cracker", "tool_multimeter_calibrated"),
    ("Multi-Channel Gas Chromatography Column", "item_relic_chromatography_col", 2, 65, 270, 450, "bp_atmospheric_gas_analyzer", "tool_optics_bench"),
    ("Precision Strain-Gauge Load Cell", "item_relic_load_cell", 2, 44, 160, 200, "bp_digital_truck_weighbridge", "tool_multimeter_calibrated"),
    ("Silicon Controlled Rectifier Stack", "item_relic_scr_stack", 2, 56, 230, 550, "bp_heavy_substation_inverter", "tool_soldering_iron_basic"),
    ("Fiber-Optic Gyroscope Assembly", "item_relic_fiber_gyro", 2, 68, 280, 500, "bp_inertial_navigation_compass", "tool_optics_bench"),
    ("Rotary Screw Air Compressor Head", "item_relic_screw_compressor", 2, 50, 200, 700, "bp_pneumatic_tool_compressor", "tool_lathe_metalworking"),
    ("Bismuth-Telluride Thermoelectric Module", "item_relic_thermoelectric_peltier", 2, 48, 190, 300, "bp_solid_state_freezer_box", "tool_soldering_iron_basic"),
    ("High-Vacuum Diffusion Vapor Pump", "item_relic_diffusion_pump", 2, 62, 260, 800, "bp_vacuum_tube_refurbishing_lathe", "tool_inert_gas_welder"),
    ("Electromagnetic Flow Meter Flange", "item_relic_magnetic_flowmeter", 2, 46, 170, 250, "bp_water_grid_leak_detector", "tool_multimeter_calibrated"),
    ("Precision Carbide Endmill Spindle", "item_relic_carbide_spindle", 2, 58, 220, 650, "bp_precision_cnc_milling_head", "tool_lathe_metalworking"),
    ("Lead-Glass Radiation Shield Window", "item_relic_lead_glass_viewport", 2, 52, 210, 0, "bp_hot_cell_glovebox_station", "tool_diamond_cutter"),

    # Tier 3: Classified Pre-War Military Stratum Tech (41 - 60)
    ("Target Acquisition Radar T/R Module", "item_relic_radar_tr_module", 3, 85, 360, 1800, "bp_sentry_active_radar_tracker", "tool_oscilloscope_crt"),
    ("Cryogenic Stirling Cooler Cold Finger", "item_relic_stirling_cooler", 3, 88, 380, 2200, "bp_thermal_imaging_flir_sight", "tool_optics_bench"),
    ("Sentry Turret Dual-Axis Servo Gimbal", "item_relic_servo_gimbal", 3, 80, 340, 1500, "bp_automated_turret_emplacement", "tool_inert_gas_welder"),
    ("Nd:YAG Laser Cavity Resonator Rod", "item_relic_laser_resonator", 3, 92, 420, 2800, "bp_laser_rangefinder_targeter", "tool_diamond_cutter"),
    ("Gallium-Nitride RF Power Transistor", "item_relic_gan_power_transistor", 3, 82, 350, 1600, "bp_high_power_radio_jammer", "tool_oscilloscope_crt"),
    ("Subterranean Seismic Geophone Sensor", "item_relic_seismic_geophone", 3, 78, 320, 1200, "bp_early_warning_burrower_detector", "tool_multimeter_calibrated"),
    ("Night Vision Microchannel Plate (MCP)", "item_relic_mcp_image_intensifier", 3, 90, 400, 2000, "bp_generation_3_night_optics", "tool_optics_bench"),
    ("Explosive Reactive Armor Sensor Grid", "item_relic_era_sensor_grid", 3, 84, 360, 1400, "bp_vehicle_active_protection_mesh", "tool_inert_gas_welder"),
    ("High-Density Lithium-Sulfur Cell Pack", "item_relic_lis_cell_pack", 3, 76, 300, 1000, "bp_exoskeleton_battery_rig", "tool_multimeter_calibrated"),
    ("Thermal Neutron Scintillation Crystal", "item_relic_neutron_scintillator", 3, 94, 440, 2500, "bp_nuclear_core_reactivity_gauge", "tool_optics_bench"),
    ("Military Millimeter-Wave Horn Antenna", "item_relic_mmwave_horn", 3, 86, 370, 1700, "bp_counter_battery_mortar_radar", "tool_lathe_metalworking"),
    ("Titanium Turbine Impeller Rotor", "item_relic_titanium_turbine", 3, 80, 330, 2100, "bp_micro_gas_turbine_generator", "tool_lathe_metalworking"),
    ("Beryllium-Coated Laser Steering Mirror", "item_relic_beryllium_mirror", 3, 95, 450, 3000, "bp_directed_energy_perimeter_beam", "tool_diamond_cutter"),
    ("Tactical Battlefield SIGINT Receiver", "item_relic_sigint_receiver_board", 3, 88, 390, 2400, "bp_automated_cipher_decryption_hub", "tool_oscilloscope_crt"),
    ("Hydraulic Exoskeleton Linear Actuator", "item_relic_exoskeleton_actuator", 3, 82, 350, 1900, "bp_powered_salvage_exoskeleton_frame", "tool_inert_gas_welder"),
    ("Ballistic Computing Inertial Unit", "item_relic_ballistic_computer_core", 3, 90, 410, 2200, "bp_artillery_lead_fire_director", "tool_oscilloscope_crt"),
    ("Encrypted Key Cryptographic Security Module", "item_relic_hsm_crypto_core", 3, 96, 480, 3200, "bp_enclave_stratum_door_override_key", "tool_oscilloscope_crt"),
    ("Ceramic Silicon-Carbide Turbine Stator", "item_relic_sic_turbine_stator", 3, 84, 360, 2000, "bp_high_efficiency_co-gen_reactor", "tool_diamond_cutter"),
    ("Fiber-Bragg Grating Stress Sensor", "item_relic_fbg_stress_sensor", 3, 78, 320, 1300, "bp_silo_structural_collapse_monitor", "tool_optics_bench"),
    ("High-Impulse Pulse-Doppler Transceiver", "item_relic_pulse_doppler_unit", 3, 98, 500, 3500, "bp_orbital_debris_tracking_array", "tool_oscilloscope_crt")
]

for idx, r in enumerate(relic_names, start=1):
    name, item_id, tier, diff, duration, watts, bp_id, tool = r
    entry = f"""### RELIC REVERSE-ENGINEERING BLUEPRINT #{idx:02d}: `{name.upper()}`
- **Master Schematic Identifier**: `relic_schema_{idx:03d}_{item_id.replace('item_relic_', '')}`
- **Source Artifact Entity**: `{item_id}`
- **Technological Stratum**: Tier {tier} (Complexity Rating: **{diff} / 100**)
- **Workshop Disassembly Duration**: {duration} minutes ({duration/60.0:.2f} hours) · Power Draw: **{watts} Watts**
- **Required Primary Tooling**: `{tool}`
- **Permanent Blueprint Awarded**: `{bp_id}` (Enables manufacturing in Shelter Foundry/Machine Shop)
- **Component Yield Disassembly Breakdown**:
  - Scrap Metal / Structural Frame: {5 + idx * 2} units (100% guarantee)
  - Precision Wire / Copper Conduit: {2 + (idx % 6)} units (100% guarantee)
  - High-Value Core Salvage: `{"item_vacuum_tube_pair" if tier == 1 else "item_optical_lens_coated" if tier == 2 else "item_microprocessor_military"}` (Probability: {95 - (diff / 3.0):.1f}%)
- **Catastrophic Failure Risk**:
  - Hazard Profile: `{"Minor Tool Wear" if tier == 1 else "Chemical Toxic Aerosol" if tier == 2 else "Capacitor High-Voltage Arc Flash"}`
  - Detonation Probability on Failure: {0.0 if tier == 1 else 5.0 if tier == 2 else 15.0}%
- **Diegetic Engineering Lore**:
  > *"Recovered from pre-war industrial ruin Sector {idx % 8 + 1}. Disassembly requires meticulous isolation of volatile chemical reagents and brittle glass envelopes. Once decoded into `{bp_id}`, the shelter can replicate this apparatus indefinitely."*

"""
    relic_entries.append(entry)

part1_text = header + "".join(relic_entries)

with open(plan_path, "w", encoding="utf-8") as f:
    f.write(part1_text)

print(f"Plan 04 Part 1 written! Current size: {len(part1_text)} chars")
