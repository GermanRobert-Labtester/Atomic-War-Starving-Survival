# ENGINEERING LOG CORPUS MATRIX

Overview of ASHFALL's ambient engineering and maintenance log corpora and their relationship to the canonical 20 glitches.

---

## 1. Corpus Census

| File Name | Collection ID | Schema Version | Record Count | Core Key Names | Primary Authors | Dominant Subsystems |
|---|---|---|---|---|---|---|
| `narrative/bunker_maintenance_glitches.json` | `the_20_subterranean_engineering_emergencies_and_pipe_glitch_logs` | 1 | 20 entries | `glitch_id`, `log_code`, `affected_subsystem`, `severity_tier`, `anomaly_description`, `diagnostic_telemetry`, `required_repair_kit`, `emergency_protocol`, `dmitri_shift_note`, `tags` | Chief Engineer Dmitri, Valery, Nadia, Boris, Mikhail, Elena, Stepan, Ilya, Taras | Steam/Heating, Artesian Pumps, Substation, Air Filtration, Boilers, Sump, Diesel Engine, CO2 Scrubber, Battery Bank, Blast Gate |
| `narrative/engineering_logs_expansion.json` | `engineering_logs_expansion` | 1 | 26 logs | `log_id`, `recorded_day`, `technician`, `system`, `severity`, `title`, `entry`, `parts_used`, `follow_up` | Tomas (engineer) | Meridian Model K 4-cyl Generator, pumps, filters, hatches, wiring |
| `narrative/bunker_maintenance_logs_batch_2.json` | `bunker_routine_maintenance_and_incident_logs` | 1 | 10 logs | `log_id`, `recorded_day`, `technician`, `system`, `severity`, `title`, `entry`, `parts_used`, `follow_up` | The Electrician, The Plumber, The Mechanic, The Architect | Air Filter, Water Recycler, Backup Generator Voltage Regulator, Blast Door Seals, Lathe |
| `narrative/bunker_maintenance_logs_batch_3.json` | N/A (root `items`) | 1 | 12 logs | `log_id`, `day`, `author`, `system`, `period`, `issue`, `action_taken`, `parts_used`, `follow_up`, `status`, `tags` | The Plumber, The Electrician, The Mechanic, The General | Cistern Feed Lines (Freeze), Voltage Brownouts & Load Shedding, Fuel Drum Crane |
| `narrative/engineering_mod_notes.json` | `engineering_mod_notes` | 1 | 8 mods | `mod_id`, `day`, `author`, `system`, `title`, `problem`, `solution`, `materials_used`, `risk_assessment`, `tags` | The Mechanic, The Electrician, The Plumber, The Architect | Water Recycler UV Lamp, Generator Variac Manual Bypass, Compost Toilet Urine Diversion |

---

## 2. Structural & Role Differences
1. **Canonical Glitches (`bunker_maintenance_glitches.json`)**:
   - Authored with engineering telemetry, precise physical parameters (Pa, bar, ppm, RMS mm/s, Hz, °C), emergency protocols, and repair kits.
   - Handled natively by `BunkerMaintenanceCatalog` and adapted into `NarrativeDiscoveryCatalog` (`BunkerGlitchSourceAdapter`).
   - Represents the canonical 20 baseline infrastructure emergencies spanning the full lifetime of the bunker (Day 1 to the Day 3650 finale).
2. **Ambient Logs & Field Notes (`engineering_logs_expansion.json`, Batches 2 & 3, Mod Notes)**:
   - Authored from the perspective of active shelter crew (Tomas, Electrician, Plumber, Mechanic) adapting to resource scarcity.
   - Rich in diegetic problem-solving: improvised boot-leather gaskets, tractor-axle cranes, salvaged satellite dish reflectors.
   - Categorized as `CODEX_ONLY` data assets discovered via time progression (`min_day` gating in Journal / discovery manifests) or room inspection.

---

## 3. Thematic Synergy with Core Gameplay
- **Power Grid**: Tomas's generator logs and Batch 3 load-shedding protocols directly mirror the load-shedding and battery-drain mechanics in `PowerGridSystem`.
- **Thermal Management**: Dmitri's steam header and anthracite slagging logs reinforce `ShelterThermalSystem` radiator health.
- **Atmospheric Scrubbers**: The mold-clogged pre-filter and channeled CO2 beds echo `VentilationSystem` filter degradation and radon ventilation.
- **Hydrology & Sumps**: Plumber logs for frozen cistern feeds and sewage sump float jams align with `SumpFloodingSystem`.
