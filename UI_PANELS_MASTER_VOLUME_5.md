# ASHFALL: Atomic War - Starving Survival
# Master UI Panels Volume 5 (Panels 121–150) & The Definitive 150-Panel Master System Suite

```
═══════════════════════════════════════════════════════════════════════════════════════════
  PROJECT: ASHFALL (2D Atomic-War Survival)
  DOCUMENT: Master UI Specification Volume 5 (Panels 121 to 150) - Total Completion (150/150)
  THEME: Cold Survival / Scavenged Field Manual
  COLOR PALETTE: Dark Charcoal (#131313) | Ashen Grey (#D1D5DB) | Muted Teal (#2D5A5E) | Burnt Orange (#CC5500)
  EXPORT TARGET: Game Root Directory & assets/ui/
═══════════════════════════════════════════════════════════════════════════════════════════
```

---

## 1. Executive Analysis: UI Architecture & Scope Completion

With Volumes 1 through 4 providing Panels 01–120 and the 62 Google Stitch visual implementations already downloaded into the repository, **30 more specialized panels** are required to reach **100% complete game coverage**. 

This 5th and final volume completes all remaining niches:
1. **Siege Warfare & Tactical Embrasures** (Panels 121–126)
2. **Nuclear Radiochemistry & Alpha Spectrometry** (Panels 127–132)
3. **Deep Subterranean Hydro-Logistics & Mining** (Panels 133–138)
4. **Specialized Surgery, Orthotics & Mental Wellness** (Panels 139–144)
5. **Accessibility, Audio Synthesis, Custom Difficulty & Grand Master Overseer Console** (Panels 145–150)

With this suite, **no further UI systems are required for the entire ASHFALL game lifecycle**.

---

## 2. Panels 121–150 Detailed Architectural Specifications

```
───────────────────────────────────────────────────────────────────────────────────────────
PANEL 121: UI_PANEL_MORTAR_INDIRECT_FIRE_BALLISTIC_PLOTTING
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Indirect Siege Defense & Heavy Ordnance
• Core Linkage: Ashfall.Core.CombatSystem, ShelterDefenseSystem
• Wireframe:
  - Left: 81mm improvised mortar tube elevation bubble level (45° to 85°), propellant cheese charge selector (Charge 0 to 4).
  - Center: Topographical parabolic trajectory plotting board, target grid coordinates, crosswind dispersion ellipse overlay.
  - Right: High-explosive vs white phosphorus smoke shell stockpile, shell flight time countdown (18.4s), blast fragmentation radius.
  - Bottom: [HANG MORTAR ROUND (FIRE)], [ADJUST ELEVATION BUBBLE], [SHIFT FIRING DEFLECTION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 122: UI_PANEL_CONCERTINA_WIRE_ELECTRIC_FENCE_ENERGIZER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Perimeter Defense & Entanglement
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, PowerGridSystem
• Wireframe:
  - Left: Pulse energizer capacitor bank voltage (10,000V Peak), earth grounding rod soil moisture resistance (Ohms).
  - Center: Perimeter fence line circuit map (Sectors Alpha to Foxtrot), razor wire entanglement health, continuous continuity loop.
  - Right: Short-circuit / wire-cut alarm annunciator, intruder electric shock deterrent counter, battery load drain (Amps).
  - Bottom: [PULSE HIGH-VOLTAGE DISCHARGE], [SWITCH TO SOLAR CHARGER], [LOCATE GROUND FAULT SECTOR].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 123: UI_PANEL_SUB_SURFACE_FOAM_FIRE_SUPPRESSION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Bunker Emergency Systems & Firefighting
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, GameBootstrap
• Wireframe:
  - Left: Aqueous Film-Forming Foam (AFFF) concentrate reservoir level (%), pressurized nitrogen propellant cylinders (150 Bar).
  - Center: Bunker sector heat / infrared flame detector matrix (Kitchen, Fuel Depot, Generator Bay, Smelter), flame spread status.
  - Right: High-expansion foam deluge valve actuators (Zone 1 to 6), automated ventilation damper interlocks.
  - Bottom: [DISCHARGE AFFF FOAM DELUGE], [ISOLATE FLAMMABLE FUEL LINES], [PURGE TOXIC FIRE EXHAUST].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 124: UI_PANEL_COUNTER_MINING_LISTENING_TUNNEL_POST
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Subterranean Counter-Siege Defense
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, CombatSystem
• Wireframe:
  - Left: Directional acoustic geophone stethoscope listening cups, subterranean sound amplification gain dial (0-100 dB).
  - Center: Subterranean acoustic radar polar plot showing enemy sapper pickaxe vibrations, tunnel excavation approach depth.
  - Right: Counter-mining defensive camouflet dynamite charge triggers (Shafts 1–4), estimated time until enemy breach.
  - Bottom: [DETONATE CAMOUFLET BLAST], [DRIVE COUNTER-MINE INTERCEPT SHAFT], [SILENCE DEFENSIVE TUNNEL].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 125: UI_PANEL_HEAVY_BARRICADE_SANDBAG_EMBRASURE_BENCH
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Fortification & Sentry Positions
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, InventorySystem
• Wireframe:
  - Left: Barricade construction materials (Burlap sandbags, crushed basalt ballast, armored AR500 steel embrasure plates).
  - Center: 2D embrasure cross-section visualizer displaying firing traverse angle (60° arc), ballistic bullet deflection slope.
  - Right: Barricade armor rating (Hit Points), defender cover bonus (+75% Incoming Fire Mitigation), weapon bipod mount status.
  - Bottom: [STACK REINFORCED SANDBAGS], [WELD SLOTTED ARMOR EMBRASURE], [MOUNT HEAVY MACHINE GUN].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 126: UI_PANEL_DECOY_RADAR_HEAT_EMITTER_BEACON
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Electronic Warfare & Missile Deception
• Core Linkage: Ashfall.Core.RadioSystem, ShelterDefenseSystem
• Wireframe:
  - Left: Surface decoy station power telemetry (Propane burner thermal flare output kW, radio frequency noise transmitter).
  - Center: Wasteland decoy placement map, radar cross-section (RCS) simulation signature matching true bunker coordinates.
  - Right: Inbound missile / scavenger raid decoy attraction probability (82%), fuel consumption rate (liters/hr).
  - Bottom: [IGNITE THERMAL DECOY FLARE], [BROADCAST RADAR ECHO SPOOF], [REPLACE DECOY PROPANE CYLINDER].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 127: UI_PANEL_ALPHA_SPECTROMETRY_SILICON_BARRIER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Radiochemistry & Actinide Analysis
• Core Linkage: Ashfall.Core.RadiationSystem, ResearchSystem
• Wireframe:
  - Left: Passivated Implanted Planar Silicon (PIPS) detector chamber, roughing vacuum pump pressure (<0.1 mbar).
  - Center: Alpha energy pulse-height multichannel analyzer (5.0 to 6.0 MeV spectrum), Plutonium-239 & Americium-241 peaks.
  - Right: Actinide chemical recovery yield (%), electroplated alpha disk count rate (cpm), sample isotope purity ratio.
  - Bottom: [EVACUATE ALPHA VACUUM CHAMBER], [COUNT 600S ALPHA SPECTRUM], [IDENTIFY WEAPONS-GRADE PLUTONIUM].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 128: UI_PANEL_TRITIUM_EXTRACTION_HEAVY_WATER_COLUMN
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Nuclear Physics & Radioisotope Harvesting
• Core Linkage: Ashfall.Core.RadiationSystem, PowerGridSystem
• Wireframe:
  - Left: Scavenged pre-war emergency exit sign stock, broken phosphor tube breaker hopper, tritiated gas collection manifold.
  - Center: Cryogenic charcoal adsorption bed (-196°C), palladium membrane diffuser cell, gas separation fractionator.
  - Right: Compressed tritium gas storage ampoule (T2 gas, 12.3-year halflife), radioluminescent gunsight vial filling bench.
  - Bottom: [EXTRACT TRITIUM GAS], [FILL RADIOLUMINESCENT SIGHT VIAL], [SEAL TRITIUM STORAGE CASING].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 129: UI_PANEL_MASS_SPECTROMETRY_GAS_CHROMATOGRAPH
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Chemical Warfare & Environmental Toxicology
• Core Linkage: Ashfall.Core.Medical.MedicalSystem, WaterPurificationSystem
• Wireframe:
  - Left: Capillary GC column oven temperature program (40°C to 300°C), carrier helium gas head pressure (PSI).
  - Center: Quadrupole electron-impact mass spectrum graph (m/z ratio), molecular fragmentation library match algorithm.
  - Right: Toxic agent identification report (Sarin degradation markers, sulfur mustard bis-2-chloroethyl, dioxin ppb).
  - Bottom: [INJECT GC-MS CHEMICAL SAMPLE], [MATCH SPECTRAL NIST LIBRARY], [ISSUE CHEMICAL THREAT ALARM].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 130: UI_PANEL_CRYO_TARGET_NEUTRON_ACTIVATION_ANALYSIS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Advanced Scientific Prospecting
• Core Linkage: Ashfall.Core.ResearchSystem, Economy.CraftingSystem
• Wireframe:
  - Left: Americium-Beryllium (Am-Be) isotopic neutron howitzer source, paraffin moderator block, thermal neutron flux.
  - Center: Target sample pneumatic rabbit transfer tube, short-lived isotopic activation decay curve (Minutes to hours).
  - Right: Trace elemental concentration report (Gold ppb, Rare earth neodymium ppm, Uranium ore enrichment grade %).
  - Bottom: [IRRADIATE TARGET WITH NEUTRONS], [PNEUMATIC TRANSFER TO DETECTOR], [CALCULATE TRACE ASSAY].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 131: UI_PANEL_RADIONUCLIDE_PRECIPITATION_SLUDGE_CLARIFIER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Liquid Rad-Waste Remediation
• Core Linkage: Ashfall.Core.RadiationSystem, WaterPurificationSystem
• Wireframe:
  - Left: High-level radioactive wastewater holding tank, chemical coagulant feeders (Ferric sulfate, sodium carbonate).
  - Center: Precipitation reaction flocculation tank (Strontium carbonate coprecipitation), sludge settling rake torque (Nm).
  - Right: Continuous solid-bowl decanter centrifuge, radioactive sludge cake moisture content (%), supernate rad level.
  - Bottom: [ADD STRONTIUM PRECIPITANT], [START CENTRIFUGE DECANTER], [TRANSFER SLUDGE TO VITRIFICATION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 132: UI_PANEL_HIGH_PURITY_GERMANIUM_DETECTOR_CRYOSTAT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Ultra-Precision Radiometry
• Core Linkage: Ashfall.Core.RadiationSystem, ResearchSystem
• Wireframe:
  - Left: High-Purity Germanium (HPGe) crystal temperature (77 Kelvin), liquid nitrogen Dewar level (liters remaining).
  - Center: Ultra-high resolution 16,384-channel gamma spectrum display (0.1 keV FWHM resolution), background lead castle.
  - Right: Automated multi-radionuclide deconvolution engine, minimal detectable activity (MDA) calculation, sample report.
  - Bottom: [REFILL LIQUID NITROGEN CRYOSTAT], [CALIBRATE ENERGY CHANNELS], [SAVE HIGH-RES GAMMA SPECTRUM].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 133: UI_PANEL_CAVERN_AIR_PRESSURE_AIRLOCK_REGULATOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Subterranean HVAC & Atmosphere Control
• Core Linkage: Ashfall.Core.ShelterAirFiltrationSystem, WeatherSystem
• Wireframe:
  - Left: Shelter interior air pressure vs surface atmospheric pressure differential (+250 Pascals overpressure target).
  - Center: Airlock vestibule rapid equalization valves, pneumatic seal bladder inflation pressure (3.5 Bar).
  - Right: Toxic dust backdraft blowout detector, HVAC overpressure relief damper blades, airlock cycle log.
  - Bottom: [CYCLE AIRLOCK EQUALIZATION], [BOOST SHELTER OVERPRESSURE], [INFLATE INFLATABLE DOOR GASKETS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 134: UI_PANEL_SUBSIDENCE_TILTMETER_CRUSTAL_MONITOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Geological Safety & Structural Health
• Core Linkage: Ashfall.Core.StructuralIntegritySystem, WeatherSystem
• Wireframe:
  - Left: Biaxial electrolytic tiltmeter sensors (Sensors 1 to 8), angular tilt deflection in microradians (μrad).
  - Center: Subterranean bedrock subsidence contour map showing cave-in sinkhole formation vectors, crustal strain rate.
  - Right: Bunker foundation differential settlement millimeters, structural pillar crack extensometer telemetry.
  - Bottom: [ZERO TILTMETER SENSORS], [ORDER SECTOR STRUCTURAL REINFORCEMENT], [TRIGGER BEDROCK CRUSH ALERT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 135: UI_PANEL_HYDRAULIC_TURBO_PUMP_AQUIFER_LIFT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Water Extraction & Deep Aquifer Hydrology
• Core Linkage: Ashfall.Core.WaterPurificationSystem, PowerGridSystem
• Wireframe:
  - Left: Submersible 12-stage centrifugal pump motor electrical parameters (480V 3-Phase, 18 Amps, Stator Temp: 65°C).
  - Center: Deep aquifer borehole water column (Depth: 350m), pump discharge head pressure (45 Bar), water delivery flow (L/min).
  - Right: Raw ground-water mineral salinity (TDS ppm), silica sand content abrasive wear sensor, reservoir buffer tank level.
  - Bottom: [START DEEP AQUIFER PUMP], [ADJUST VARIABLE FREQUENCY DRIVE], [FLUSH SAND SEPARATOR].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 136: UI_PANEL_BRINE_SOLAR_EVAPORATION_SALT_PAN
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Food Preservation & Mineral Harvesting
• Core Linkage: Ashfall.Core.FoodProductionSystem, WaterPurificationSystem
• Wireframe:
  - Left: Waste desalination concentrated brine inflow (12% Salinity), solar collector mirror concentration angles.
  - Center: Stepped evaporation pans (Concentrator Pan, Settling Pan, Crystallizer Pan), brine specific gravity hydrometer.
  - Right: Solid edible sodium chloride salt harvest yield (kg/day), trace iodine content, bitter magnesium bitterns tap.
  - Bottom: [RAKE CRYSTALLIZED SALT BATCH], [DRAIN MAGNESIUM BITTERNS], [TRANSFER SALT TO MEAT CURING].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 137: UI_PANEL_PNEUMATIC_JACKHAMMER_DRILL_LUBRICATOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Mining Tool Maintenance & Pneumatics
• Core Linkage: Ashfall.Core.Economy.CraftingSystem, InventorySystem
• Wireframe:
  - Left: Air compressor line receiver pressure (7.5 Bar), compressed air delivery CFM, pneumatic hose burst check.
  - Center: In-line automatic oiler mist reservoir, mineral oil viscosity grade, drill steel shank impact strike rate (BPM).
  - Right: Jackhammer tungsten carbide chisel bit wear indicator, piston ring air blow-by sensor, maintenance schedule.
  - Bottom: [REFILL PNEUMATIC OIL RESERVOIR], [SWAP CARBIDE CHISEL BIT], [TEST DRILL STRIKE FREQUENCY].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 138: UI_PANEL_SUBTERRANEAN_CARGO_TRAMWAY_WINCH
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Mine Haulage & Rail Logistics
• Core Linkage: Ashfall.Core.Economy.CraftingSystem, SurvivorWorkShiftSystem
• Wireframe:
  - Left: Heavy electric haulage winch drum (15mm steel wire rope), drum brake lining temperature (°C), motor torque.
  - Center: Inclined rail incline grade profile (30° slope, 400m track), ore cart payload weight (Tons), haulage velocity.
  - Right: Track runaway emergency derailer switch, rope tension load cell reading, automatic overspeed safety catch dog.
  - Bottom: [HOIST LOADED ORE TRAMCART], [ENGAGE INCLINE MECHANICAL BRAKE], [RESET ROPE TENSION ALARM].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 139: UI_PANEL_ARTERIAL_GRAFT_SURGICAL_CLAMP_TRAY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Advanced Vascular Trauma Surgery
• Core Linkage: Ashfall.Core.Medical.MedicalSystem, AfflictionPipeline
• Wireframe:
  - Left: Patient hemorrhagic shock hemodynamic telemetry (Blood pressure: 70/40, Hematocrit: 18%, Pulse: 145 BPM).
  - Center: Vascular surgical field schematic, Bulldog arterial clamps, Dacron woven synthetic vascular graft tube sizing.
  - Right: Heparinized saline flushing syringe, fine 6-0 polypropylene vascular suture needle counter, distal arterial flow Doppler.
  - Bottom: [CLAMP SEVERED FEMORAL ARTERY], [ANASTOMOSE DACRON GRAFT], [RELEASE CLAMP & VERIFY PULSE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 140: UI_PANEL_PEDIATRIC_MALNUTRITION_KWASHIORKOR_CHART
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Nutritional Medicine & Child Survival
• Core Linkage: Ashfall.Core.NeedsSystem, MedicalSystem
• Wireframe:
  - Left: Child survivor nutritional profile (Age, Height, Weight, Mid-Upper Arm Circumference MUAC: <115mm - Severe).
  - Center: Clinical malnutrition assessment (Bilateral pitting edema grade, skin peeling dermatitis, hypothermia risk).
  - Right: Therapeutic feeding formula schedule: Phase 1 (F-75 Starter Formula 75 kcal/100ml) -> Phase 2 (F-100 / Plumpy'Nut).
  - Bottom: [ADMINISTER F-75 RESCUE MILK], [DISPENSE MICRONUTRIENT VITAMINS], [LOG WEIGHT RECOVERY PROGRESS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 141: UI_PANEL_ELECTROENCEPHALOGRAPHY_SEIZURE_MONITOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Neurological Care & Trauma Sequelae
• Core Linkage: Ashfall.Core.CombatTraumaSystem, MedicalSystem
• Wireframe:
  - Left: 16-channel international 10-20 scalp EEG electrode impedance map, conductive gel contact check.
  - Center: Live brainwave electroencephalogram trace visualizer (Alpha / Beta / Theta / Delta rhythm waves), spike-and-wave discharges.
  - Right: Post-traumatic epileptiform seizure onset alert, anticonvulsant dosing titrator (Phenobarbital / Diazepam mg).
  - Bottom: [RECORD 30-MIN EEG TRACE], [INJECT DIAZEPAM ANTICONVULSANT], [CALIBRATE ELECTRODE IMPEDANCE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 142: UI_PANEL_ORTHOPEDIC_TRACTION_SPLINT_RIGGING
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Traumatic Bone Fracture Management
• Core Linkage: Ashfall.Core.Medical.AfflictionPipeline, MedicalSystem
• Wireframe:
  - Left: Patient skeletal x-ray/fluoroscopy review (Compound femur fracture, bone fragment displacement, soft tissue swelling).
  - Center: Thomas traction splint schematic, pulley counterbalance weight setup (10% Body Weight in lead weights), pin site tension.
  - Right: Distal neurovascular limb circulation check (Pedal pulse Doppler strength, capillary refill <2s, compartment pressure).
  - Bottom: [APPLY SKELETAL TRACTION TENSION], [DRESS TRANSSPINAL PIN SITES], [CHECK COMPARTMENT PRESSURE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 143: UI_PANEL_SURVIVOR_GRIEF_PEER_SUPPORT_CIRCLE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Community Mental Health & Morale Recovery
• Core Linkage: Ashfall.Core.SocialSystem, FinalWishSystem
• Wireframe:
  - Left: Support group circle participants (Survivors suffering from Guilt, Insomnia, Combat Trauma, Bereavement).
  - Center: Emotional catharsis dialogue flow visualizer, shared grief processing index, collective traumatic narrative cohesion.
  - Right: Morale recovery output (+20% Morale floor, -40% Nightmare frequency), interpersonal caregiver trust bonds formed.
  - Bottom: [FACILITATE SHARING CIRCLE], [OFFER MEMORIAL DEDICATION], [PRESCRIBE HERBAL SEDATIVE TEA].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 144: UI_PANEL_CHRONIC_RADIATION_CATARACT_ASPIRATION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Ophthalmic Surgery & Restoration of Sight
• Core Linkage: Ashfall.Core.Medical.MedicalSystem, RadiationSystem
• Wireframe:
  - Left: Patient ocular pathology assessment (Posterior subcapsular radiation cataract, blind eye status, corneal endothelium).
  - Center: Micro-incision phacoemulsification needle view, ultrasonic lens fragmentation energy, balanced salt infusion/aspiration.
  - Right: Rigid PMMA intraocular lens (IOL) implant insertion injector, post-op antibiotic eye drop regimen, visual recovery.
  - Bottom: [ASPIRATE CLOUDED LENS TISSUE], [INSERT INTRAOCULAR LENS IMPLANT], [SEAL CORNEAL INCISION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 145: UI_PANEL_ACCESSIBILITY_NEURODIVERGENT_PALETTE_CONFIG
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: User Settings, Accessibility & Inclusivity
• Core Linkage: Ashfall.Core.Settings, GameBootstrap
• Wireframe:
  - Left: Colorblindness shader simulation modes (Normal / Protanopia / Deuteranopia / Tritanopia / Monochromacy).
  - Center: UI typography font scaling slider (100% to 175%), high-contrast border toggles, dyslexic-friendly font option.
  - Right: Screen shake / flash / photo-sensitivity reducers, UI sound cue visual indicators, text-to-speech audio narration.
  - Bottom: [TEST COLOR PALETTE SHADER], [RESET TO DEFAULT ACCESSIBILITY], [SAVE ACCESSIBILITY PROFILE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 146: UI_PANEL_AUDIO_FREQUENCY_SYNTHESIZER_SETTINGS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Sound Engineering & Audio Mixer
• Core Linkage: Ashfall.Core.Audio, Settings
• Wireframe:
  - Left: Master audio mixer faders (Master, Music/Atmosphere, SFX/Machinery, Voice/Intercom, Geiger Counter Ticks, Sub-Bass).
  - Center: Real-time 10-band graphic equalizer display (32 Hz to 16 kHz), dynamic range compression (Night Mode vs Cinematic).
  - Right: Environmental audio reverberation acoustic presets (Concrete Bunker, Deep Mine, Frozen Wasteland, Metal Duct).
  - Bottom: [TEST GEIGER TICK FREQUENCY], [APPLY 10-BAND EQ], [MUTE NON-CRITICAL ALARMS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 147: UI_PANEL_KEYBINDING_TACTICAL_HOTKEY_REMAPPING
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Controls, Input & Hardware Mapping
• Core Linkage: Ashfall.Core.Input, Settings
• Wireframe:
  - Left: Input categories (Shelter Navigation, Tactical Combat, Fast-Action Rad-Away Injection, Map Pan, Sub-System Hotkeys).
  - Center: Action binding matrix table (Primary Key, Secondary Key, Controller Gamepad Button, Double-Tap vs Hold modifier).
  - Right: Real-time key conflict warning detector, controller stick deadzone calibration curves, mouse sensitivity slider.
  - Bottom: [RECORD NEW KEYBIND], [RESTORE DEFAULT CONTROLS], [APPLY KEYBIND PROFILE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 148: UI_PANEL_DIAGNOSTIC_FRAME_PACING_PERFORMANCE_METRICS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Developer Tools, Performance & Headless Telemetry
• Core Linkage: Ashfall.Core.Diagnostics, GameBootstrap
• Wireframe:
  - Left: Engine frame rate metrics (Current FPS, 99th percentile frame time ms, GPU draw calls, VRAM texture memory MB).
  - Center: Subsystem CPU execution time breakdown graph (Physics tick, Pathfinding, Needs simulation, UI draw, Bridge shim).
  - Right: Memory garbage collection GC pause timer, active entity count (Survivors, Items, Particles, Sounds), error console log.
  - Bottom: [DUMP PROFILER TELEMETRY], [FORCE MEMORY CLEANUP (GC)], [TOGGLE MINIMALIST DEBUG HUD].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 149: UI_PANEL_SURVIVAL_DIFFICULTY_CUSTOM_CHALLENGE_MODIFIERS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Game Balance, Difficulty & Replayability
• Core Linkage: Ashfall.Core.GameBootstrap, EndgameSystem
• Wireframe:
  - Left: Preset difficulty profiles (Scavenger [Normal], Nuclear Winter [Hard], Zero Hour [Nightmare], Custom Modifier Sandbox).
  - Center: Detailed challenge parameter sliders (Fallout radiation buildup rate %, Metabolic caloric drain, Cold onset speed, Loot scarcity).
  - Right: Iron-will permadeath toggle, single-save mode, final score multiplier calculation (e.g. 2.45x Score Multiplier).
  - Bottom: [LOCK CUSTOM DIFFICULTY CHALLENGE], [SHARE CHALLENGE SEED CODE], [COMMENCE SURVIVAL RUN].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 150: UI_PANEL_GRAND_MASTER_TERMINAL_OVERSEER_COMMAND
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Centralized Command & The Complete Universe
• Core Linkage: Ashfall.Core.MasterSession, GameBootstrap, All Subsystems
• Wireframe:
  - Left: 150-subsystem operational health matrix (All 150 tactical panels status: Green Nominal / Amber Degraded / Red Critical).
  - Center: Master panoramic bunker cutaway & wasteland sector tactical map with real-time survivor, power, air, and defense telemetry.
  - Right: Executive commander prompt terminal, grand apocalypse survival timeline, final victory conditions progression bar.
  - Bottom: [EXECUTE MASTER OVERRIDE], [SAVE COMPLETE UNIVERSE STATE], [DECLARE THE DAWN OF ASHFALL].
```

---

## 3. Comprehensive 150-Panel Master Reference Matrix

```
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                     ASHFALL DEFINITIVE 150-PANEL UI MASTER MATRIX                       │
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
│ 91 │ UI_PANEL_SULFURIC_ACID_CONTACT_PLANT       │ Ashfall.Core.Economy.CraftingSystem   │
│ 92 │ UI_PANEL_METHANOL_WOOD_GAS_PRODUCER        │ Ashfall.Core.PowerGridSystem          │
│ 93 │ UI_PANEL_PRUSSIAN_BLUE_CHEMICAL_SYNTHESIZE │ Ashfall.Core.RadiationSystem          │
│ 94 │ UI_PANEL_ACTIVATED_CARBON_STEAM_RETORT     │ Ashfall.Core.ShelterAirFiltration     │
│ 95 │ UI_PANEL_CHLORINE_BLEACH_ELECTROLYZER      │ Ashfall.Core.WaterPurificationSystem  │
│ 96 │ UI_PANEL_ANILINE_DYE_INK_PRINTING_PRESS    │ Ashfall.Core.SocialSystem             │
│ 97 │ UI_PANEL_BLIZZARD_CHILL_FACTOR_HEURISTIC   │ Ashfall.Core.WeatherSystem            │
│ 98 │ UI_PANEL_SNOW_MELTER_FLUE_HEAT_HARVESTER   │ Ashfall.Core.WaterPurificationSystem  │
│ 99 │ UI_PANEL_PERMAFROST_TUNNEL_CRYOGENIC_CREEP │ Ashfall.Core.StructuralIntegritySystem│
│100 │ UI_PANEL_SUB_GLACIAL_LAKE_HOT_WATER_DRILL  │ Ashfall.Core.Expeditions.Expedition   │
│101 │ UI_PANEL_AURORA_IONOSPHERIC_RAD_MONITOR    │ Ashfall.Core.WeatherSystem            │
│102 │ UI_PANEL_INSULATION_AEROGEL_FABRICATION    │ Ashfall.Core.CraftingSystem           │
│103 │ UI_PANEL_RAD_RESISTANT_TILAPIA_AQUACULTURE │ Ashfall.Core.FoodProductionSystem     │
│104 │ UI_PANEL_BLACK_SOLDIER_FLY_LARVAE_VAT      │ Ashfall.Core.FoodProductionSystem     │
│105 │ UI_PANEL_HYDROLOGICAL_SILT_SEDIMENTATION   │ Ashfall.Core.WaterPurificationSystem  │
│106 │ UI_PANEL_APICULTURE_RAD_POLLEN_HONEY_HIVE  │ Ashfall.Core.FoodProductionSystem     │
│107 │ UI_PANEL_MUTATED_YEAST_GENOME_CULTIVATION  │ Ashfall.Core.FoodProductionSystem     │
│108 │ UI_PANEL_MUSHROOM_MYCOREMEDIATION_FILTER   │ Ashfall.Core.RadiationSystem          │
│109 │ UI_PANEL_ONE_TIME_PAD_CIPHER_CRYPTO_VAULT  │ Ashfall.Core.RadioSystem              │
│110 │ UI_PANEL_ACOUSTIC_TELEGRAPH_EARTH_RETURN   │ Ashfall.Core.RadioSystem              │
│111 │ UI_PANEL_ANALOG_COMPUTING_FIRE_CONTROL     │ Ashfall.Core.CombatSystem             │
│112 │ UI_PANEL_SHORTWAVE_NUMBERS_STATION_INTER   │ Ashfall.Core.RadioSystem              │
│113 │ UI_PANEL_LASER_OPTICAL_LINE_OF_SIGHT_COMMS │ Ashfall.Core.RadioSystem              │
│114 │ UI_PANEL_SECTOR_GRID_POWER_LOAD_DISPATCH   │ Ashfall.Core.PowerGridSystem          │
│115 │ UI_PANEL_NOBODYS_CHARTER_CROSSING_EXPEDIT  │ Ashfall.Core.Expansions.NobodysCharter│
│116 │ UI_PANEL_VERDICT_COUNCIL_EXECUTIVE_DECREE  │ Ashfall.Core.Verdict                  │
│117 │ UI_PANEL_SILENT_FOUNDRY_AUTOMATION_CORE    │ Ashfall.Core.SilentFoundry            │
│118 │ UI_PANEL_DEEP_COAST_LIGHTHOUSE_FOG_BEACON  │ Ashfall.Core.DeepCoast                │
│119 │ UI_PANEL_MEMORIAL_CENOTAPH_INSCRIPTION     │ Ashfall.Core.FinalWishSystem          │
│120 │ UI_PANEL_ATMOSPHERIC_RESTORATION_GLOBAL    │ Ashfall.Core.EndgameSystem            │
│121 │ UI_PANEL_MORTAR_INDIRECT_FIRE_BALLISTIC    │ Ashfall.Core.CombatSystem             │
│122 │ UI_PANEL_CONCERTINA_ELECTRIC_FENCE         │ Ashfall.Core.ShelterDefenseSystem     │
│123 │ UI_PANEL_SUB_SURFACE_FOAM_FIRE_SUPPRESS    │ Ashfall.Core.ShelterDefenseSystem     │
│124 │ UI_PANEL_COUNTER_MINING_LISTENING_POST     │ Ashfall.Core.ShelterDefenseSystem     │
│125 │ UI_PANEL_HEAVY_BARRICADE_SANDBAG_EMBRASURE │ Ashfall.Core.ShelterDefenseSystem     │
│126 │ UI_PANEL_DECOY_RADAR_HEAT_EMITTER_BEACON   │ Ashfall.Core.RadioSystem              │
│127 │ UI_PANEL_ALPHA_SPECTROMETRY_SILICON_BARRIER│ Ashfall.Core.RadiationSystem          │
│128 │ UI_PANEL_TRITIUM_EXTRACTION_HEAVY_WATER    │ Ashfall.Core.RadiationSystem          │
│129 │ UI_PANEL_MASS_SPECTROMETRY_GAS_CHROMATOGR  │ Ashfall.Core.Medical.MedicalSystem    │
│130 │ UI_PANEL_CRYO_TARGET_NEUTRON_ACTIVATION    │ Ashfall.Core.ResearchSystem           │
│131 │ UI_PANEL_RADIONUCLIDE_PRECIPITATION_SLUDGE │ Ashfall.Core.RadiationSystem          │
│132 │ UI_PANEL_HPGE_DETECTOR_CRYOSTAT            │ Ashfall.Core.RadiationSystem          │
│133 │ UI_PANEL_CAVERN_AIR_PRESSURE_AIRLOCK       │ Ashfall.Core.ShelterAirFiltration     │
│134 │ UI_PANEL_SUBSIDENCE_TILTMETER_CRUSTAL_MON  │ Ashfall.Core.StructuralIntegritySystem│
│135 │ UI_PANEL_HYDRAULIC_TURBO_PUMP_AQUIFER_LIFT │ Ashfall.Core.WaterPurificationSystem  │
│136 │ UI_PANEL_BRINE_SOLAR_EVAPORATION_SALT_PAN  │ Ashfall.Core.FoodProductionSystem     │
│137 │ UI_PANEL_PNEUMATIC_JACKHAMMER_LUBRICATOR   │ Ashfall.Core.Economy.CraftingSystem   │
│138 │ UI_PANEL_SUBTERRANEAN_TRAMWAY_WINCH        │ Ashfall.Core.Economy.CraftingSystem   │
│139 │ UI_PANEL_ARTERIAL_GRAFT_SURGICAL_CLAMP     │ Ashfall.Core.Medical.MedicalSystem    │
│140 │ UI_PANEL_PEDIATRIC_MALNUTRITION_CHART      │ Ashfall.Core.NeedsSystem              │
│141 │ UI_PANEL_ELECTROENCEPHALOGRAPHY_SEIZURE    │ Ashfall.Core.CombatTraumaSystem       │
│142 │ UI_PANEL_ORTHOPEDIC_TRACTION_SPLINT_RIG    │ Ashfall.Core.Medical.AfflictionPipeline│
│143 │ UI_PANEL_SURVIVOR_GRIEF_SUPPORT_CIRCLE     │ Ashfall.Core.SocialSystem             │
│144 │ UI_PANEL_RADIATION_CATARACT_ASPIRATION     │ Ashfall.Core.Medical.MedicalSystem    │
│145 │ UI_PANEL_ACCESSIBILITY_NEURODIVERGENT      │ Ashfall.Core.Settings                 │
│146 │ UI_PANEL_AUDIO_SYNTHESIZER_SETTINGS        │ Ashfall.Core.Audio                    │
│147 │ UI_PANEL_KEYBINDING_TACTICAL_HOTKEY_REMAP  │ Ashfall.Core.Input                    │
│148 │ UI_PANEL_DIAGNOSTIC_FRAME_PACING_METRICS   │ Ashfall.Core.Diagnostics              │
│149 │ UI_PANEL_CUSTOM_CHALLENGE_MODIFIERS        │ Ashfall.Core.GameBootstrap            │
│150 │ UI_PANEL_GRAND_MASTER_OVERSEER_COMMAND     │ Ashfall.Core.MasterSession (Unified)  │
└────┴────────────────────────────────────────────┴───────────────────────────────────────┘
```
