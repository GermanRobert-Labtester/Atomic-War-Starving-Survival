# ENGINEERING TRIGGER AND PROJECTION MATRIX

## 1. Architectural Philosophy
The engineering emergencies in `narrative/bunker_maintenance_glitches.json` and ambient engineering logs represent the lived mechanical reality of the bunker across its multi-year operation.

This document defines how live simulation states, room inspections, and day-progression triggers project into matching glitch and maintenance profiles, without compromising simulation authority.

---

## 2. Trigger Sources & Projection Surface

```
┌────────────────────────────────────────────────────────────────────────┐
│                        TRIGGER SOURCES                                 │
├─────────────────────────┬────────────────────────┬─────────────────────┤
│   Day / Story Window    │   Room Inspection      │   Subsystem Stress  │
│   (min_day / timeline)  │   (player clicks room) │   (sensor threshold)│
└────────────┬────────────┴───────────┬────────────┴──────────┬──────────┘
             │                        │                       │
             ▼                        ▼                       ▼
┌────────────────────────────────────────────────────────────────────────┐
│                 BUNKER MAINTENANCE PROJECTION LAYER                    │
│   - Pure query function: (stateSnapshot, roomId, day) -> GlitchMatch  │
│   - Read-only: never modifies any system variables                     │
│   - Deterministic: same inputs yield same projection                  │
└────────────────────────────────────┬───────────────────────────────────┘
                                     │
                                     ▼
┌────────────────────────────────────────────────────────────────────────┐
│                       PRESENTATION SURFACES                            │
│   - JournalPanel / NarrativeDiscovery / MachineTell / RoomInspection  │
└────────────────────────────────────────────────────────────────────────┘
```

---

## 3. Subsystem Condition Trigger Mappings

| Subsystem Category | Trigger Metric / Key | Threshold Condition | Projected Glitch Candidate(s) | Narrative Effect |
|---|---|---|---|---|
| **Heating / Boiler** | `MachineConditionKeys.ThermalBoilerFuel` | Fuel < 20% or thermal strain | `glitch_01_radiator_header_steam_fracture`, `glitch_05_boiler_firebox_anthracite_slagging` | Radiator clatter, steam hiss, high-temp warnings |
| **Water / Sump** | `MachineConditionKeys.WaterFilterIntegrity` | Filter < 40% or sump level high | `glitch_02_artesian_intake_cavitation_hammer`, `glitch_06_sewage_sump_float_switch_jam` | Acoustic pipe hammer, drainage overflow warnings |
| **Power / Generator** | `MachineConditionKeys.PowerFuelUnits`, `MachineConditionKeys.PowerBatteryReserve` | Fuel low, or battery < 30% | `glitch_03_main_substation_neutral_ground_loop`, `glitch_07_diesel_genset_injector_carbon_knock`, `glitch_18_substation_battery_rack_thermal_runaway` | Generator knock, flickering fluorescents, battery heat |
| **Ventilation / Scrubber** | `MachineConditionKeys.HepaFilterHealth`, `MachineConditionKeys.VentilationFilterSaturation` | Filter health < 50% or saturation > 80% | `glitch_04_intake_air_filter_black_mold_clog`, `glitch_11_co2_chemical_scrubber_bed_channeling` | Fan thermal trip risk, stale air / CO2 lethargy |
| **Airlock / Blast Door** | `MachineConditionKeys.AirlockIncidentActive` | Incident active or outer seal strain | `glitch_09_blast_door_hydraulic_ram_seal_leak`, `glitch_14_decontamination_shower_thermostatic_valve_failure` | Hydraulic spray, blast door seal degradation |
| **Greenhouse / Flora** | Room inspection `room_greenhouse` | Day progression / Inspection | `glitch_08_hydroponic_led_ballast_resonance_buzz` | Ballast vibration hum, stressed livestock |
| **Airlock / Periscope** | Room inspection `room_airlock` | Severe weather / blizzard | `glitch_16_periscope_optical_prism_dewing_condensation` | Fogged optics, frosted prisms |
| **Radio / Comms** | Room inspection `room_radio_tuner` | High-power broadcast event | `glitch_19_radio_transmitter_mercury_arc_rectifier_flashover` | Blown ceramic fuse, mercury tube flashover |

---

## 4. Zero Mutation Contract
To preserve determinism and architectural integrity:
1. `BunkerMaintenanceProjection` is a static/pure query helper.
2. Querying `GetGlitchesForRoom()`, `GetGlitchesForSubsystem()`, or `MatchGlitchForCondition()` takes read-only state inputs and performs zero writes to `StartingLevelSystem`, `PowerGridSystem`, or any inventory collection.
3. No `System.Random` or `Guid.NewGuid()` is used. Replay simulations remain bit-identical.
