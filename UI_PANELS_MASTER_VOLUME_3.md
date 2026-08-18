# ASHFALL: Atomic War - Starving Survival
# Master UI Panels Volume 3 (Panels 61–90) & Comprehensive 90-Panel System Matrix

```
═══════════════════════════════════════════════════════════════════════════════════════════
  PROJECT: ASHFALL (2D Atomic-War Survival)
  DOCUMENT: Master UI Specification Volume 3 (Panels 61 to 90)
  THEME: Cold Survival / Scavenged Field Manual
  COLOR PALETTE: Dark Charcoal (#131313) | Ashen Grey (#D1D5DB) | Muted Teal (#2D5A5E) | Burnt Orange (#CC5500)
  EXPORT TARGET: assets/ui/Icons/ & Root Project Directory
═══════════════════════════════════════════════════════════════════════════════════════════
```

---

## 1. Tactical UI Assets in Game Repository (`assets/ui/Icons/`)

| Asset Filename | System Domain | Description |
| :--- | :--- | :--- |
| `rtg_generator_ui_icon_1787040675971.jpg` | Nuclear Power & RTG | Heavy cylindrical lead-shielded RTG power canister with heat sink fins |
| `scintillation_spectrometer_ui_icon_1787040696039.jpg` | Nuclear Physics & Radiometry | Sodium iodide (NaI) scintillation detector probe & multichannel analyzer |
| `geothermal_turbine_ui_icon_1787040435148.jpg` | Geothermal Power | Heavy bronze steam turbine manifold with pressure gauge |
| `radio_triangulation_scope_ui_icon_1787040448216.jpg` | Radio & Signal Intercept | Cathode ray tube directional radio triangulation sweep scope |
| `reloading_press_ammo_ui_icon_1787040461364.jpg` | Armory & Ballistics | Bench-mounted reloading press, bullet casting mold, powder hopper |
| `cryo_seed_canister_ui_icon_1787040479871.jpg` | Botanical & Greenhouse | Liquid nitrogen Dewar flask with heirloom seed samples (-196°C) |
| `vitrification_cask_ui_icon_1787040493705.jpg` | Nuclear Waste Management | Hexagonal reinforced lead-shielded vitrified rad-waste canister |
| `geiger_counter_ui_icon_1787040121431.jpg` | Radiation System | Worn tactical analog dosimeter & Geiger-Müller counter |
| `chelation_ampoules_ui_icon_1787040140994.jpg` | Toxicological Medicine | EDTA & Prussian Blue anti-rad vials in lead-lined case |
| `blast_door_controller_ui_icon_1787040157036.jpg` | Shelter Defense & Armor | Cast-iron hydraulic bulkhead pressure control & locking wheel |
| `ration_conflict_tokens_ui_icon_1787040169401.jpg` | Ration & Social Governance | Caloric Emergency foil ration pouch with stamped zinc tokens |
| `somatic_trauma_ecg_ui_icon_1787040193733.jpg` | Psychiatric Rehabilitation | Analog biometrics ECG monitor with sedative autoinjector ampoule |
| `emp_vacuum_tube_bus_ui_icon_1787040204892.jpg` | Electronic Hardening | EMP hardened vacuum tube relay bank with Faraday copper mesh |

---

## 2. Panels 61–90 Detailed Architectural Specifications

```
───────────────────────────────────────────────────────────────────────────────────────────
PANEL 61: UI_PANEL_RTG_RADIOISOTOPE_GENERATOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Nuclear Physics & Emergency Power
• Core Linkage: Ashfall.Core.PowerGridSystem, Ashfall.Core.RadiationSystem
• Wireframe:
  - Left: Strontium-90 thermal core telemetry, isotope half-life decay curve (28.8-year halflife), thermal wattage.
  - Center: Thermoelectric Peltier thermocouple bridge, heat sink temperature differential (ΔT: 280°C), voltage output (24V DC).
  - Right: Critical shelter sub-grid distribution bus (Cryo Vault, Master Clock, Seismic Sensors), battery buffer recharge status.
  - Bottom: [SHUNT EXCESS THERMAL HEAT], [DIVERT TRICKLE LOAD TO CRYO VAULT], [CHECK CASING RAD SHIELDING].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 62: UI_PANEL_SCINTILLATION_DETECTOR_SPECTROMETRY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Scientific Research & Radiometry
• Core Linkage: Ashfall.Core.RadiationSystem, Ashfall.Core.ResearchSystem
• Wireframe:
  - Left: Multi-channel pulse height analyzer (MCA-7), sodium iodide probe calibration, high-voltage bias supply (900V).
  - Center: Gamma energy spectrum graph (keV) showing characteristic radionuclide peaks (Cs-137 @ 662 keV, Co-60 @ 1.17/1.33 MeV, I-131 @ 364 keV).
  - Right: Environmental isotope breakdown percentage, background subtraction filter, fallout age estimation algorithm.
  - Bottom: [EXECUTE 300s SPECTRAL COUNT], [ISOLATE ENERGY PEAK], [EXPORT RADIONUCLIDE PROFILE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 63: UI_PANEL_CRITICALITY_ALARM_DOSIMETRY_POST
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Nuclear Safety & Reactor Monitoring
• Core Linkage: Ashfall.Core.RadiationSystem, Ashfall.Core.ShelterDefenseSystem
• Wireframe:
  - Left: Prompt-gamma radiation tripwire sensors (Detectors 1–4), microsecond pulse detector, Cherenkov optical photodiode.
  - Center: Criticality alarm annunciator board, red flashing warning beacons, boron poison injection status, neutron flux rate.
  - Right: Emergency shelter evacuation zone status, automated blast gate scram triggers, survivor dosimeter rapid-read queue.
  - Bottom: [ACTIVATE BORON SCRAM INJECTION], [OVERRIDE FALSE CRITICALITY ALARM], [SEAL REACTOR PERIMETER].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 64: UI_PANEL_THERMOELECTRIC_COOLING_CHILLER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Medical Preservation & Laboratory
• Core Linkage: Ashfall.Core.Medical.MedicalSystem, PowerGridSystem
• Wireframe:
  - Left: Cold-storage biological inventory (Whole blood packs, antibiotics, live viral cultures, insulin vials).
  - Center: Multi-stage Peltier solid-state chiller schematic, cold-plate temperature (-4°C), thermal paste conductivity.
  - Right: DC cooling current controller (Amps), heat rejection radiator fan speed, thermal runaway safety cutoffs.
  - Bottom: [SET TARGET CHILL TEMPERATURE], [SWITCH TO EMERGENCY BATTERY BUS], [DEFROST EVAPORATOR COILS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 65: UI_PANEL_SEISMIC_FAULT_SONAR_ARRAY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Geotechnical Hazards & Structural Safety
• Core Linkage: Ashfall.Core.WeatherSystem, StructuralIntegritySystem
• Wireframe:
  - Left: Subterranean geophone sensor network (Sensors A-F), ground tremor frequency (Hz), shear wave velocity.
  - Center: 3D seismic fault-line acoustic visualizer, underground bedrock fracture depth, aftershock hazard probability.
  - Right: Bunker structural vibration index, rock bolt stress load cell readings, cave-in risk alert levels.
  - Bottom: [RE-TENSION ROCK BOLTS], [RECALIBRATE GEOPHONE SENSITIVITY], [TRIGGER STRUCTURAL ALARM].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 66: UI_PANEL_MAGNETIC_COMPASS_DECLINATION_STATION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Wasteland Cartography & Navigation
• Core Linkage: Ashfall.Core.Expeditions.ExpeditionSystem, WeatherSystem
• Wireframe:
  - Left: Post-EMP geomagnetic field disturbance index, true north vs magnetic north deviation angle (-18.4° W).
  - Center: Precision gyroscopic meridian compass turntable, brass azimuth ring, bubble level balance.
  - Right: Wasteland expedition route correction tables, dead reckoning navigation logs, cartographic magnetic declination map.
  - Bottom: [CALIBRATE EXPEDITION GYROSCOPES], [UPDATE SECTOR BEARING OFFSETS], [LOCK TRUE NORTH REFERENCE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 67: UI_PANEL_BOREHOLE_CORE_SAMPLE_STRATIGRAPHY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Geology, Mining & Mineral Prospecting
• Core Linkage: Ashfall.Core.Economy.CraftingSystem, WaterPurificationSystem
• Wireframe:
  - Left: Rotary drilling rig depth counter (0-500m), diamond core bit wear (%), drill mud circulation pressure.
  - Center: Core sample tray visualizer displaying geological rock strata cylinders (Basalt, Shale, Sandstone, Pyrite veins, Lead ore).
  - Right: Mineralogical assay report (Lead content: 14%, Copper: 4%, Potable Aquifer Permeability: Moderate).
  - Bottom: [EXTRACT 3-METER CORE BARREL], [LOG STRATIGRAPHIC LAYER], [COMMENCE DEEP EXPANSION SHAFT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 68: UI_PANEL_VENTILATION_FLUE_DAMPER_ACTUATOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Shelter Life Support & HVAC
• Core Linkage: Ashfall.Core.ShelterAirFiltrationSystem, WeatherSystem
• Wireframe:
  - Left: Flue exhaust air temperature, toxic combustion backdraft pressure sensor, carbon monoxide (CO ppm) monitor.
  - Center: Pneumatic damper blade position indicators (Flues 1–8: Closed / 25% / 50% / Full Open), actuator air pressure.
  - Right: Zone airflow balance meters (Kitchen Flue, Boiler Exhaust, Smelter Chimney, Generator Stack).
  - Bottom: [ACTIVATE EMERGENCY BACKDRAFT SEAL], [CALIBRATE DAMPER STEPPERS], [MANUAL HAND-CRANK OVERRIDE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 69: UI_PANEL_HYDRAULIC_MINE_SHORING_JACKS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Subterranean Expansion & Mining Safety
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, CraftingSystem
• Wireframe:
  - Left: Active excavation tunnel sectors (Sector Alpha Drift, South Ore Stope, West Aquifer Tunnel), ceiling span width.
  - Center: Hydraulic timber prop load gauges (Tons of overhead pressure), yield valve relief indicators, shoring jack pressure.
  - Right: Acoustic rock micro-fracture creak rate (Clicks/min), timber splintering stress sensors, timber replacement inventory.
  - Bottom: [PRESSURIZE SHORING HYDRAULICS], [INSTALL REINFORCED STEEL CROSS-BEAM], [EVACUATE UNSTABLE TUNNEL].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 70: UI_PANEL_ORE_PULVERIZER_JAW_CRUSHER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Mineral Processing & Smelting
• Core Linkage: Ashfall.Core.Economy.CraftingSystem, InventorySystem
• Wireframe:
  - Left: Raw mined rock hopper feed, coarse boulder feed rate (kg/hr), dust suppression water spray status.
  - Center: Heavy manganese steel jaw crusher cutaway, eccentric shaft RPM, jaw gap setting dial (5mm-50mm), flywheel inertia.
  - Right: Vibrating screen particle size distribution (Fine Sand, Concentrate Grain, Oversize Re-circulate), output bin weight.
  - Bottom: [START JAW CRUSHER MOTOR], [ADJUST CRUSH GAP DISCHARGE], [CLEAR ROCK FEED CHOKE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 71: UI_PANEL_PNEUMATIC_TUBE_MESSENGER_DISPATCH
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Internal Communications & Logistics
• Core Linkage: Ashfall.Core.SocialSystem, GameBootstrap
• Wireframe:
  - Left: Bunker station directory (Station 01: Command, Station 04: Medical, Station 07: Sentry Post, Station 12: Reactor).
  - Center: Vacuum tube manifold routing diverters, air compressor vacuum reservoir (inHg), capsule transit tracking sensors.
  - Right: Capsule dispatch carrier queue, urgent medical prescription canister, encrypted military order seals.
  - Bottom: [LAUNCH PNEUMATIC CAPSULE], [REVERSE TUBE AIR VACUUM], [CLEAR BLOCKED DISPATCH TUBE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 72: UI_PANEL_CAVE_IN_RESCUE_AIR_BORE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Emergency Response & Survivor Rescue
• Core Linkage: Ashfall.Core.SurvivorWorkShiftSystem, NeedsSystem
• Wireframe:
  - Left: Trapped miner sector telemetry (Sector 3 collapse), estimated oxygen reserve hours remaining, acoustic sound sensor.
  - Center: 6-inch micro-borehole drilling angle guide, pneumatic percussion hammer drill depth, borehole casing sleeve.
  - Right: Emergency supply line umbilical (Fresh oxygen pump, glucose broth feeding tube, two-way intercom microphone).
  - Bottom: [ACTIVATE OXYGEN INJECTION PUMP], [TRANSMIT RESCUE INTERCOM AUDIO], [DRILL STABILIZATION CASING].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 73: UI_PANEL_BLOOD_GAS_ACIDOSIS_ANALYZER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Advanced Critical Care Medicine
• Core Linkage: Ashfall.Core.Medical.AfflictionPipeline, MedicalSystem
• Wireframe:
  - Left: Patient arterial blood sample cartridge status, heparinized capillary tube input, electrode calibration slope.
  - Center: Blood gas diagnostic readout: Blood pH (7.18 - Severe Acidosis), pCO2 (58 mmHg), pO2 (62 mmHg), Base Excess (-8 mEq/L).
  - Right: Acid-base nomogram graph (Metabolic vs Respiratory Acidosis), projected cardiac arrest threshold, bicarbonate titrator.
  - Bottom: [INFUSE SODIUM BICARBONATE], [ADJUST MECHANICAL VENTILATOR PEEP], [PRINT ARTERIAL LAB REPORT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 74: UI_PANEL_DERMAL_BETA_BURN_DEBRIDEMENT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Radiation Medicine & Burn Surgery
• Core Linkage: Ashfall.Core.RadiationSystem, MedicalSystem
• Wireframe:
  - Left: Patient anatomical burn map (Beta burn erythema, radioactive fallout dust contact necrosis, blisters).
  - Center: Surgical debridement workstation, ultrasonic scalpel frequency, sterile irrigation saline jet pressure, local anesthetic.
  - Right: Biological dressing inventory (Silver sulfadiazine cream, amniotic membrane patch, sterile petroleum gauze).
  - Bottom: [EXCISE NECROTIC BETA TISSUE], [APPLY SILVER SULFADIAZINE], [APPLY STERILE PRESSURE WRAP].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 75: UI_PANEL_OPHTHALMIC_SLIT_LAMP_RADIATION_CATARACT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Clinical Examination & Ophthalmology
• Core Linkage: Ashfall.Core.Medical.AfflictionPipeline, SurvivorSystem
• Wireframe:
  - Left: Survivor visual acuity score (20/200 Snellen), radiation ocular dose history, corneal sensitivity reflex.
  - Center: Slit lamp biomicroscope ocular viewfinder (Posterior subcapsular cataract opacity visualization, lens granules).
  - Right: Intraocular pressure tonometer reading (mmHg), mydriatic pupil dilation drops, protective amber rad-glasses prescription.
  - Bottom: [EXAMINE LENS POSTERIOR CAPSULE], [PRESCRIBE LEAD-GLASS SHIELD], [SCHEDULE SURGICAL LENS REMOVAL].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 76: UI_PANEL_IMMUNOSUPPRESSION_BONE_MARROW_ISOLATOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Extreme Radiation Sickness Care
• Core Linkage: Ashfall.Core.RadiationSystem, MedicalSystem
• Wireframe:
  - Left: Aplastic anemia patient vitals, Absolute Neutrophil Count (ANC: <100/uL - Critical), bacterial infection risk index.
  - Center: Laminar flow sterile canopy schematic, positive pressure HEPA filtered air curtain, UV-C sterilizer air duct.
  - Right: Prophylactic antimicrobial regimen (Broad-spectrum antibacterial, antifungal fluconazole, sterile nutrition broth).
  - Bottom: [ENGAGE ULTRA-CLEAN LAMINAR FLOW], [ADMINISTER G-CSF MARROW STIMULANT], [RESTRICT ROOM ACCESS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 77: UI_PANEL_HEAVY_METAL_TOXICOLOGY_URINE_ASSAY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Toxicology & Environmental Screening
• Core Linkage: Ashfall.Core.Medical.MedicalSystem, WaterPurificationSystem
• Wireframe:
  - Left: Survivor urine sample batch queue, specific gravity hydrometer, protein/glucose reagent dipstick readings.
  - Center: Colorimetric dithizone spectrophotometer assay (Uranium toxicity peak, Lead intoxication ppm, Cadmium bio-burden).
  - Right: Renal clearance filtration efficiency, tubular damage markers (Beta-2 Microglobulin), chelation candidate flag.
  - Bottom: [RUN COLORIMETRIC TOXICOLOGY ASSAY], [CERTIFY CHELATION CANDIDATE], [LOG WATER SOURCE CONTAMINATION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 78: UI_PANEL_SOMATIC_PHANTOM_LIMB_MIRROR_BOX
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Neurological Therapy & Rehabilitation
• Core Linkage: Ashfall.Core.CombatTraumaSystem, SocialSystem
• Wireframe:
  - Left: Amputee survivor phantom pain severity score, phantom muscle spasm duration, neuropathic pain medication tolerance.
  - Center: Optical mirror box visualizer, dual-hand reflection alignment grid, biofeedback electromyography (EMG) electrode trace.
  - Right: Guided relaxation audio frequency, neuroplastic retraining progress bar, caregiver facilitation bonus.
  - Bottom: [COMMENCE MIRROR VISUAL THERAPY], [RECORD EMG SPASM REDUCTION], [DISPENSE MUSCLE RELAXANT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 79: UI_PANEL_THERMAL_IMAGING_NIGHT_BINOCULARS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Reconnaissance, Security & Sentry
• Core Linkage: Ashfall.Core.CombatSystem, ShelterDefenseSystem
• Wireframe:
  - Left: Sensor operating parameters (Uncooled vanadium oxide microbolometer, 640x480 resolution, frame rate 50Hz, battery %).
  - Center: False-color thermal viewfinder (White-Hot / Black-Hot / Ironbow / Red-Alert), human thermal signatures against snow.
  - Right: Distance stadiametric rangefinder calculation, target heat differential (ΔT: +34°C vs -20°C ambient), movement vector.
  - Bottom: [CYCLE THERMAL PALETTE], [CALIBRATE NON-UNIFORMITY SHUTTER], [TRANSMIT SENTRY FIRING COORDINATES].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 80: UI_PANEL_VEHICLE_ENGINE_TURBO_DIESEL_BENCH
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Vehicle Maintenance & Logistics
• Core Linkage: Ashfall.Core.Expeditions.ExpeditionSystem, CraftingSystem
• Wireframe:
  - Left: Scavenger truck 6-cylinder diesel engine block, glow plug resistance (Ohms), starter motor crank torque.
  - Center: Turbocharger boost pressure gauge (PSI), exhaust gas temperature pyrometer (EGT °C), fuel injector spray pattern.
  - Right: Dual-fuel selector valve (Petroleum Diesel / Filtered Waste Vegetable Oil), preheater fuel loop temperature.
  - Bottom: [CRANK DIESEL ENGINE TEST], [SWITCH TO VEGETABLE BIOFUEL], [PURGE FUEL LINE AIR BUBBLES].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 81: UI_PANEL_SNIPER_BALLISTIC_WIND_DRIFT_CALCULATOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Precision Combat & Perimeter Defense
• Core Linkage: Ashfall.Core.CombatSystem, WeatherSystem
• Wireframe:
  - Left: Atmospheric ballistics inputs (Barometric pressure hPa, Air temperature -25°C, Density altitude, Crosswind knot/bearing).
  - Center: Reticle mil-dot elevation/windage adjustment turret dials, bullet trajectory arc graph (7.62x54mmR / .338 Lapua).
  - Right: Terminal ballistics target calculation: Range (850m), Flight Time (1.24s), Spin Drift (+4 clicks), Coriolis drift.
  - Bottom: [ZERO SNIPER OPTIC (800M)], [COMPUTE LEAD FOR MOVING TARGET], [ENGAGE FIRING SOLUTION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 82: UI_PANEL_SCAVENGER_PNEUMATIC_CUTTER_JAWS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Scavenging & Heavy Salvage
• Core Linkage: Ashfall.Core.InventorySystem, ExpeditionSystem
• Wireframe:
  - Left: Hydraulic power pack pressure (700 Bar), reservoir hydraulic fluid level, 2-stroke gas motor RPM.
  - Center: Heavy bypass cutter jaw schematic, cutting force rating (65 Tons), hardened tool-steel blade edge condition.
  - Right: Target salvage material thickness (Reinforced rebar, armored vault plate, vehicle door hinge), estimated cut time.
  - Bottom: [ENGAGE HYDRAULIC CUTTING JAWS], [REVERSE VALVE SPREADER MODE], [SHARPEN CUTTER TOOL BLADES].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 83: UI_PANEL_TRIPWIRE_PERIMETER_ALARM_CENTRAL
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Perimeter Defense & Security
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, CombatSystem
• Wireframe:
  - Left: Perimeter defense circuit status (Sectors 1 to 12), continuous electrical continuity loop, break-wire resistance.
  - Center: Bunker exterior map showing tripwire lines, acoustic vibration sensor locations, trip flare release pods.
  - Right: Active intrusion alarm annunciator (Flashing red sector warning, audible alarm bell toggle, automated searchlight slew).
  - Bottom: [IGNITE SECTOR DEFENSE TRIP-FLARE], [ARM PERIMETER CLAYMORE MINES], [RESET BREAK-WIRE CONTINUITY].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 84: UI_PANEL_SATELLITE_EPHEMERIS_ORBITAL_DECAY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Astronomy, Scavenging & World Lore
• Core Linkage: Ashfall.Core.RadioSystem, ResearchSystem
• Wireframe:
  - Left: Tracked military recon satellite catalog (SAT-09, KH-11 Relay, Meteor-3M), orbital altitude (180km - Decaying).
  - Center: World orbital ground-track map showing satellite passage footprints, Doppler shift radio receiver frequency graph.
  - Right: Satellite downlink reception window timer (Pass duration: 04m 12s), telemetry signal-to-noise ratio (SNR), data dump decode.
  - Bottom: [POINT TRACKING PARABOLIC DISH], [RECORD SATELLITE DOWNLINK DATA], [PREDICT CRASH DEBRIS IMPACT ZONE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 85: UI_PANEL_CENTURY_SEED_GENETIC_PROPAGATION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Long-Term Victory & Genetic Preservation
• Core Linkage: Ashfall.Core.Expansions.CenturySeed, FoodProductionSystem
• Wireframe:
  - Left: Master heirloom seed gene-pool (Generation 1 to 5), radiation-induced chromosomal aberration rate (<0.02%).
  - Center: Germination growth chamber environment (CO2 enrichment ppm, photosynthetic LED spectrum, nutrient film thickness).
  - Right: Multi-decade genetic viability projections, drought/cold tolerance traits, Century Seed maturation milestone countdown.
  - Bottom: [CROSS-POLLINATE HARDY STRAIN], [EXTRACT CRYOGENIC EMBRYO], [PLANT NEXT GENERATION SEEDBED].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 86: UI_PANEL_REFUGEE_INTAKE_QUARANTINE_STAGING
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Social Governance, Immigration & Disease Control
• Core Linkage: Ashfall.Core.SocialSystem, Medical.AfflictionPipeline
• Wireframe:
  - Left: Inbound refugee party dossier (Family headcount, originating wasteland sector, declared trade assets).
  - Center: Triage intake classification board (RED: Critical Rad / YELLOW: Contagious Sick / GREEN: Cleared / BLACK: Deceased).
  - Right: Bunker housing availability (Bunks remaining: 4), shelter food surplus days, security risk background check.
  - Bottom: [AUTHORIZE QUARANTINE INTAKE], [OFFER REFUGEE REPAIR CONTRACT], [REFUSE SHELTER ENTRY (EXILE)].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 87: UI_PANEL_CHILD_EDUCATION_PREWAR_CURRICULUM
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Social Morale, Culture & Survivor Skills
• Core Linkage: Ashfall.Core.SocialSystem, GameBootstrap
• Wireframe:
  - Left: Shelter school classroom roster (Child survivors ages 6–16), attendance rate, nutritional focus score.
  - Center: Slate chalkboard lesson planner (Pre-War Science, Agricultural Chemistry, Radio Mechanics, Wasteland Safety Rules).
  - Right: Survivor instructor assignment (Elder survivor teaching bonus, mentorship bonding), future workforce skill aptitude.
  - Bottom: [ASSIGN APPRENTICESHIP WORKSHOP], [AUTHORIZE PAPER & PENCIL RATION], [COMMENCE LITERACY LESSON].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 88: UI_PANEL_SHELTER_CURRENCY_MINT_SCATTER_ZINC
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Economy & Fiscal Governance
• Core Linkage: Ashfall.Core.Economy.DynamicEconomySystem, CraftingSystem
• Wireframe:
  - Left: Minting raw materials (Melted zinc battery casings, scrap copper wire, stamping steel dies).
  - Center: Manual screw drop-hammer coin press diagram, die face alignment (Face: Bunker Trefoil / Obverse: Calorie Value).
  - Right: Shelter currency token supply in circulation (Zinc tokens, denomination breakdown: 10, 50, 100 Calorie Value), inflation rate.
  - Bottom: [MINT 100 CALORIE TOKENS], [REPLACE WORN COIN DIE], [RECALL DEBASED CURRENCY BATCH].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 89: UI_PANEL_STANDING_RECORD_FACTION_PACT_LEDGER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Factions, Standing Record Expansion & Diplomacy
• Core Linkage: Ashfall.Core.Expansions.StandingRecord, FactionSystem
• Wireframe:
  - Left: Regional faction treaty ledger (Meridian Compact, Iron Nomads, Salt Barons, Undertow Coalition).
  - Center: Historical pact stipulations (Mutual military assistance, shared ice-road patrol, toll-free trade status).
  - Right: Faction compliance score, mutual grievance claims, joint defensive bunker stockpile reserve balance.
  - Bottom: [EXTEND STANDING TREATY (30 DAYS)], [LODGE FORMAL TREATY GRIEVANCE], [SIGN JOINT MILITARY COMMUNIQUE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 90: UI_PANEL_VAULT_CHRONICLER_ORAL_HISTORY_RECORD
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Narrative, Journal & Cultural Continuity
• Core Linkage: Ashfall.Core.Journal, FinalWishSystem
• Wireframe:
  - Left: Chronicler's audio archive library (Magnetic wire recordings 1 to 48), speaker identity, topic (The Exchange, Old World, Scavenger Legends).
  - Center: Reel-to-reel magnetic wire sound recorder interface, recording head bias voltage, audio playback speaker VU meter.
  - Right: Folklore transcription script window, community cultural continuity index, wasteland oral history preservation score.
  - Bottom: [RECORD SURVIVOR ORAL TESTIMONY], [TRANSCRIBE AUDIO TO ARCHIVE SCROLL], [BROADCAST CHRONICLE TO BEDSIDES].
```

---

## 3. Comprehensive 90-Panel System Reference Matrix

```
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                           ASHFALL 90-PANEL UI MASTER MATRIX                             │
├────┬────────────────────────────────────────────┬───────────────────────────────────────┤
│ ID │ Screen / Panel Identifier                  │ Core System Mapping                   │
├────┼────────────────────────────────────────────┼───────────────────────────────────────┤
│ 01 │ UI_PANEL_DOSE_LEDGER_CHELATION             │ Ashfall.Core.RadiationSystem          │
│ 02 │ UI_PANEL_COMBAT_TRAUMA_SOMATIC_FLASHBACK   │ Ashfall.Core.CombatTraumaSystem       │
│ 03 │ UI_PANEL_CHEMICAL_DEPENDENCY_TAPERING      │ Ashfall.Core.Medical.MedicalSystem    │
│ 04 │ UI_PANEL_RESPIRATORY_DEGENERATION_OXYGEN   │ Ashfall.Core.Medical.AfflictionPipeline│
│ 05 │ UI_PANEL_QUARANTINE_ISOLATION_BAY          │ Ashfall.Core.Medical.AfflictionPipeline│
│ 06 │ UI_PANEL_SURGICAL_SUITE_AUTOCLAVE          │ Ashfall.Core.Medical.MedicalSystem    │
│ 07 │ UI_PANEL_AIR_INTAKE_CYCLONE_HEPA_BANK      │ Ashfall.Core.ShelterAirFiltration     │
│ 08 │ UI_PANEL_BLAST_DOOR_SKY_LAYER_ARMOR        │ Ashfall.Core.ShelterDefenseSystem     │
│ 09 │ UI_PANEL_EMP_VACUUM_TUBE_SWITCHBOARD       │ Ashfall.Core.PowerGridSystem          │
│ 10 │ UI_PANEL_AQUIFER_SIPHON_BRINE_DISTILLATION │ Ashfall.Core.WaterPurificationSystem  │
│ 11 │ UI_PANEL_THERMAL_JACKET_HEAT_EXCHANGER     │ Ashfall.Core.WeatherSystem            │
│ 12 │ UI_PANEL_COAL_BRIQUETTING_SOLID_FUEL_PRESS │ Ashfall.Core.Economy.CraftingSystem   │
│ 13 │ UI_PANEL_HOLDFAST_BARTER_LEDGER_DEBT       │ Ashfall.Core.Economy.DynamicEconomy   │
│ 14 │ UI_PANEL_ICE_ROAD_CONVOY_HALFTRACK_DISPATCH│ Ashfall.Core.Expeditions.Expedition   │
│ 15 │ UI_PANEL_SCAVENGER_ARMORY_WEAPON_BENCH     │ Ashfall.Core.InventorySystem          │
│ 16 │ UI_PANEL_HAZMAT_SUIT_VULCANIZER_SEAM_WELDER│ Ashfall.Core.RadiationSystem          │
│ 17 │ UI_PANEL_LEAD_SMELTING_SLAG_FOUNDRY        │ Ashfall.Core.Economy.CraftingSystem   │
│ 18 │ UI_PANEL_WAYSTATION_TOLL_CARAVAN_MANIFEST  │ Ashfall.Core.Economy.DynamicEconomy   │
│ 19 │ UI_PANEL_RATION_CONFLICT_CALORIC_DISTRIB   │ Ashfall.Core.NeedsSystem              │
│ 20 │ UI_PANEL_CENSUS_CLAIMS_VOLUNTARY_REGISTER  │ Ashfall.Core.SocialSystem             │
│ 21 │ UI_PANEL_IDEOLOGICAL_FRICTION_FACTION_BAL  │ Ashfall.Core.SocialSystem             │
│ 22 │ UI_PANEL_FINAL_WISH_TESTAMENT_DEPOSITORY   │ Ashfall.Core.FinalWishSystem          │
│ 23 │ UI_PANEL_NOBODYS_CHARTER_MORAL_TRIBUNAL    │ Ashfall.Core.Expansions.NobodysCharter│
│ 24 │ UI_PANEL_SURVIVOR_WORK_SHIFT_ROSTER        │ Ashfall.Core.Survivors.SurvivorWork   │
│ 25 │ UI_PANEL_GEIGER_RAD_PLUME_ATMOSPHERIC_MAP  │ Ashfall.Core.WeatherSystem            │
│ 26 │ UI_PANEL_WASTELAND_CARTOGRAPHY_SECTOR_ATLAS│ Ashfall.Core.Expeditions.Expedition   │
│ 27 │ UI_PANEL_HATCH_DEFENSE_AUTOMATED_SENTRY    │ Ashfall.Core.ShelterDefenseSystem     │
│ 28 │ UI_PANEL_GREENHOUSE_SOIL_DEIRRADIATION     │ Ashfall.Core.FoodProductionSystem     │
│ 29 │ UI_PANEL_MUTATED_FAUNA_PATHOGEN_ANALYZER   │ Ashfall.Core.Medical.AfflictionPipeline│
│ 30 │ UI_PANEL_YEAR_OF_ASH_SOLAR_IRRADIANCE_ARRAY│ Ashfall.Core.Expansions.YearOfAsh     │
│ 31 │ UI_PANEL_DEEP_STRATA_GEOTHERMAL_BOREHOLE   │ Ashfall.Core.PowerGridSystem          │
│ 32 │ UI_PANEL_RADIO_ECHO_TRIANGULATION_SCANNER  │ Ashfall.Core.RadioSystem              │
│ 33 │ UI_PANEL_PROSTHETICS_FIELD_ORTHOPEDICS     │ Ashfall.Core.Medical.MedicalSystem    │
│ 34 │ UI_PANEL_FERMENTATION_ETHANOL_BIOFUEL      │ Ashfall.Core.Economy.CraftingSystem   │
│ 35 │ UI_PANEL_AIRLOCK_DECONTAMINATION_CASCADE   │ Ashfall.Core.RadiationSystem          │
│ 36 │ UI_PANEL_CARAVAN_LIVESTOCK_MULE_STABLE     │ Ashfall.Core.ExpeditionSystem         │
│ 37 │ UI_PANEL_AMMO_RELOADING_PRESS_BALLISTICS   │ Ashfall.Core.CraftingSystem           │
│ 38 │ UI_PANEL_PERMAFROST_SEED_BANK_CRYOGENICS   │ Ashfall.Core.FoodProductionSystem     │
│ 39 │ UI_PANEL_SHELTER_BATTERY_FLYWHEEL_KINETICS │ Ashfall.Core.PowerGridSystem          │
│ 40 │ UI_PANEL_COALITION_PACT_DIPLOMATIC_ENVOY   │ Ashfall.Core.Factions.FactionSystem   │
│ 41 │ UI_PANEL_CHRONIC_RADIATION_LEUKEMIA_LEDGER │ Ashfall.Core.RadiationSystem          │
│ 42 │ UI_PANEL_SUNKEN_HARBOR_DIVING_BELL_SALVAGE │ Ashfall.Core.MaritimeSystem           │
│ 43 │ UI_PANEL_MARITIME_ICEBREAKER_BOILER_TELEM  │ Ashfall.Core.MaritimeSystem           │
│ 44 │ UI_PANEL_SMUGGLER_DEAD_DROP_NETWORK        │ Ashfall.Core.Economy.DynamicEconomy   │
│ 45 │ UI_PANEL_MUTATED_FLORA_BOTANICAL_HERBARIUM │ Ashfall.Core.FoodProductionSystem     │
│ 46 │ UI_PANEL_SURVIVOR_FUNERAL_CATALYZER_PYRE   │ Ashfall.Core.FinalWishSystem          │
│ 47 │ UI_PANEL_RAD_SHIELD_SKY_LAYER_BALLAST      │ Ashfall.Core.ShelterDefenseSystem     │
│ 48 │ UI_PANEL_MORAL_BRANCHING_EXILE_BALLOT      │ Ashfall.Core.MoralBranchingSystem     │
│ 49 │ UI_PANEL_EXPEDITION_AMBUSH_TACTICAL_GRID   │ Ashfall.Core.CombatSystem             │
│ 50 │ UI_PANEL_HYDROLOGICAL_DESALINATION_COLUMN  │ Ashfall.Core.WaterPurificationSystem  │
│ 51 │ UI_PANEL_RADIOACTIVE_WASTE_VITRIFICATION   │ Ashfall.Core.RadiationSystem          │
│ 52 │ UI_PANEL_PRE_WAR_TECHNICAL_BLUEPRINT_ARCH  │ Ashfall.Core.ResearchSystem           │
│ 53 │ UI_PANEL_RAD_STORM_SHELTER_CURFEW_PROTOCOL │ Ashfall.Core.WeatherSystem            │
│ 54 │ UI_PANEL_OPTICAL_RANGEFINDER_SNIPER_POST   │ Ashfall.Core.CombatSystem             │
│ 55 │ UI_PANEL_SURVIVOR_PSYCHOSOCIAL_JOURNAL     │ Ashfall.Core.Journal                  │
│ 56 │ UI_PANEL_IMPROVISED_BATTERY_ELECTROLYTE    │ Ashfall.Core.CraftingSystem           │
│ 57 │ UI_PANEL_LONG_WAVE_BROADCAST_PROPAGANDA    │ Ashfall.Core.RadioSystem              │
│ 58 │ UI_PANEL_SCAVENGER_HAZARD_SUIT_DECON_SHOWER│ Ashfall.Core.RadiationSystem          │
│ 59 │ UI_PANEL_SOIL_MICROBIOME_BIOCHAR_RETORT    │ Ashfall.Core.FoodProductionSystem     │
│ 60 │ UI_PANEL_GRAND_VICTORY_EPILOGUE_COSMIC_DAWN│ Ashfall.Core.EndgameSystem            │
│ 61 │ UI_PANEL_RTG_RADIOISOTOPE_GENERATOR        │ Ashfall.Core.PowerGridSystem          │
│ 62 │ UI_PANEL_SCINTILLATION_DETECTOR_SPECTROMET │ Ashfall.Core.RadiationSystem          │
│ 63 │ UI_PANEL_CRITICALITY_ALARM_DOSIMETRY_POST  │ Ashfall.Core.RadiationSystem          │
│ 64 │ UI_PANEL_THERMOELECTRIC_COOLING_CHILLER    │ Ashfall.Core.Medical.MedicalSystem    │
│ 65 │ UI_PANEL_SEISMIC_FAULT_SONAR_ARRAY         │ Ashfall.Core.WeatherSystem            │
│ 66 │ UI_PANEL_MAGNETIC_COMPASS_DECLINATION      │ Ashfall.Core.Expeditions.Expedition   │
│ 67 │ UI_PANEL_BOREHOLE_CORE_SAMPLE_STRATIGRAPHY │ Ashfall.Core.Economy.CraftingSystem   │
│ 68 │ UI_PANEL_VENTILATION_FLUE_DAMPER_ACTUATOR  │ Ashfall.Core.ShelterAirFiltration     │
│ 69 │ UI_PANEL_HYDRAULIC_MINE_SHORING_JACKS      │ Ashfall.Core.ShelterDefenseSystem     │
│ 70 │ UI_PANEL_ORE_PULVERIZER_JAW_CRUSHER        │ Ashfall.Core.Economy.CraftingSystem   │
│ 71 │ UI_PANEL_PNEUMATIC_TUBE_MESSENGER_DISPATCH │ Ashfall.Core.SocialSystem             │
│ 72 │ UI_PANEL_CAVE_IN_RESCUE_AIR_BORE           │ Ashfall.Core.SurvivorWorkShiftSystem  │
│ 73 │ UI_PANEL_BLOOD_GAS_ACIDOSIS_ANALYZER       │ Ashfall.Core.Medical.AfflictionPipeline│
│ 74 │ UI_PANEL_DERMAL_BETA_BURN_DEBRIDEMENT      │ Ashfall.Core.RadiationSystem          │
│ 75 │ UI_PANEL_OPHTHALMIC_SLIT_LAMP_CATARACT     │ Ashfall.Core.Medical.AfflictionPipeline│
│ 76 │ UI_PANEL_IMMUNOSUPPRESSION_MARROW_ISOLATOR │ Ashfall.Core.RadiationSystem          │
│ 77 │ UI_PANEL_HEAVY_METAL_TOXICOLOGY_URINE_ASSAY│ Ashfall.Core.Medical.MedicalSystem    │
│ 78 │ UI_PANEL_SOMATIC_PHANTOM_LIMB_MIRROR_BOX   │ Ashfall.Core.CombatTraumaSystem       │
│ 79 │ UI_PANEL_THERMAL_IMAGING_NIGHT_BINOCULARS  │ Ashfall.Core.CombatSystem             │
│ 80 │ UI_PANEL_VEHICLE_ENGINE_TURBO_DIESEL_BENCH │ Ashfall.Core.Expeditions.Expedition   │
│ 81 │ UI_PANEL_SNIPER_BALLISTIC_WIND_DRIFT_CALC  │ Ashfall.Core.CombatSystem             │
│ 82 │ UI_PANEL_SCAVENGER_PNEUMATIC_CUTTER_JAWS   │ Ashfall.Core.InventorySystem          │
│ 83 │ UI_PANEL_TRIPWIRE_PERIMETER_ALARM_CENTRAL  │ Ashfall.Core.ShelterDefenseSystem     │
│ 84 │ UI_PANEL_SATELLITE_EPHEMERIS_ORBITAL_DECAY │ Ashfall.Core.RadioSystem              │
│ 85 │ UI_PANEL_CENTURY_SEED_GENETIC_PROPAGATION  │ Ashfall.Core.Expansions.CenturySeed   │
│ 86 │ UI_PANEL_REFUGEE_INTAKE_QUARANTINE_STAGING │ Ashfall.Core.SocialSystem             │
│ 87 │ UI_PANEL_CHILD_EDUCATION_PREWAR_CURRICULUM │ Ashfall.Core.SocialSystem             │
│ 88 │ UI_PANEL_SHELTER_CURRENCY_MINT_ZINC        │ Ashfall.Core.Economy.CraftingSystem   │
│ 89 │ UI_PANEL_STANDING_RECORD_FACTION_PACT      │ Ashfall.Core.Expansions.StandingRecord│
│ 90 │ UI_PANEL_VAULT_CHRONICLER_ORAL_HISTORY_REC │ Ashfall.Core.Journal                  │
└────┴────────────────────────────────────────────┴───────────────────────────────────────┘
```
