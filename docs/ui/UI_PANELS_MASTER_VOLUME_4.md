# ASHFALL: Atomic War - Starving Survival
# Master UI Panels Volume 4 (Panels 91–120) & The Complete 120-Panel System Encyclopedia

```
═══════════════════════════════════════════════════════════════════════════════════════════
  PROJECT: ASHFALL (2D Atomic-War Survival)
  DOCUMENT: Master UI Specification Volume 4 (Panels 91 to 120)
  THEME: Cold Survival / Scavenged Field Manual
  COLOR PALETTE: Dark Charcoal (#131313) | Ashen Grey (#D1D5DB) | Muted Teal (#2D5A5E) | Burnt Orange (#CC5500)
  EXPORT TARGET: assets/ui/Icons/ & Root Project Directory
═══════════════════════════════════════════════════════════════════════════════════════════
```

---

## 1. Panels 91–120 Detailed Architectural Specifications

```
───────────────────────────────────────────────────────────────────────────────────────────
PANEL 91: UI_PANEL_SULFURIC_ACID_CONTACT_PLANT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Chemical Synthesis & Heavy Refining
• Core Linkage: Ashfall.Core.Economy.CraftingSystem, PowerGridSystem
• Wireframe:
  - Left: Raw pyrite mineral feed hopper, roasting furnace temperature (850°C), sulfur dioxide (SO2) gas scrubber.
  - Center: Vanadium pentoxide catalytic conversion vessel (SO2 to SO3), exothermic heat recovery coil, sulfuric acid absorption tower.
  - Right: Concentrated sulfuric acid storage tank (98% H2SO4), acid pump pressure dial, corrosion resistance telemetry.
  - Bottom: [IGNITE PYRITE ROASTER], [REPLACE CATALYST BED], [TRANSFER ACID TO BATTERY REFINERY].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 92: UI_PANEL_METHANOL_WOOD_GAS_PRODUCER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Alternative Fuel & Energy
• Core Linkage: Ashfall.Core.PowerGridSystem, CraftingSystem
• Wireframe:
  - Left: Scrap timber / dry peat hopper feed rate, drying zone temperature, pyrolysis zone draft pressure.
  - Center: Downdraft gasifier reduction hearth cutaway, wood gas cyclone tar separator, condensable methanol distillate tap.
  - Right: Synthesis gas output flowmeter (CO + H2 + CH4), gas engine generator RPM, combustible energy BTU meter.
  - Bottom: [STOKE GASIFIER CHARGE], [PURGE TAR CONDENSER], [ENGAGE SYNGAS GENERATOR].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 93: UI_PANEL_PRUSSIAN_BLUE_CHEMICAL_SYNTHESIZER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Anti-Radiation Toxicological Chemistry
• Core Linkage: Ashfall.Core.RadiationSystem, Medical.MedicalSystem
• Wireframe:
  - Left: Chemical reagent stock (Ferric chloride, potassium ferrocyanide, distilled water, stirring motor RPM).
  - Center: Insoluble ferric ferrocyanide precipitation tank (Deep blue dye slurry), reaction pH indicator (pH 4.5).
  - Right: Vacuum Buchner funnel filter table, drying oven temperature, capsule filling machine output counter (500mg capsules).
  - Bottom: [PRECIPITATE PRUSSIAN BLUE], [WASH & FILTER CAKE], [PRESS ORAL ANTIDOTE CAPSULES].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 94: UI_PANEL_ACTIVATED_CARBON_STEAM_RETORT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Air & Water Filtration Manufacturing
• Core Linkage: Ashfall.Core.ShelterAirFiltrationSystem, WaterPurificationSystem
• Wireframe:
  - Left: Crushed nutshell / animal bone feedstock bin, carbonization retort kiln temperature (600°C).
  - Center: Superheated steam injection manifold (850°C), steam activation pressure, micropore surface area gauge (m²/g).
  - Right: Activated carbon granular sizing sieve, Iodine-adsorption number test meter, gas mask filter cartridge restock.
  - Bottom: [DISCHARGE ACTIVATED CHARCOAL], [PACK AIR FILTER CANISTERS], [RELOAD RETORT KILN].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 95: UI_PANEL_CHLORINE_BLEACH_ELECTROLYZER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Sanitation & Water Disinfection
• Core Linkage: Ashfall.Core.WaterPurificationSystem, MedicalSystem
• Wireframe:
  - Left: Saturated salt brine supply tank (NaCl solution), brine preheater temperature (+25°C), diaphragm cell voltage (4.5V DC).
  - Center: Dimensionally stable titanium anode/cathode cell, chlorine gas vent hood, sodium hydroxide (caustic soda) drain valve.
  - Right: Sodium hypochlorite bleach mixing tank (5.25% concentration), water disinfection dosing pump, pH neutralizer.
  - Bottom: [COMMENCE BRINE ELECTROLYSIS], [DRAW DISINFECTANT BLEACH], [NEUTRALIZE CAUSTIC RUNOFF].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 96: UI_PANEL_ANILINE_DYE_INK_PRINTING_PRESS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Culture, Propaganda & Faction Influence
• Core Linkage: Ashfall.Core.SocialSystem, RadioSystem
• Wireframe:
  - Left: Synthetic coal-tar aniline dye compounding bench (Lampblack, linseed oil binder, iron gall extract).
  - Center: Manual iron Gutenberg-style flatbed printing press, lead typography typesetting tray, ink roller brayer.
  - Right: Printed matter distribution queue (Shelter Survival Manuals, Faction News broadsheets, Scavenger Maps, Ration Vouchers).
  - Bottom: [PRINT 100 BROADSHEETS], [CAST LEAD TYPE REPLACEMENTS], [DISTRIBUTE TO CITIZENS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 97: UI_PANEL_BLIZZARD_CHILL_FACTOR_HEURISTIC
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Environmental Hazards & Hypothermia
• Core Linkage: Ashfall.Core.WeatherSystem, NeedsSystem
• Wireframe:
  - Left: Ambient outdoor weather metrics (Dry bulb temp: -38°C, Wind velocity: 65 km/h, Relative humidity: 85%).
  - Center: Effective wind-chill index calculator (-56°C), exposed skin frostbite onset countdown timer (04m 30s).
  - Right: Exterior sentry & expedition party thermal telemetry, body core temperature drops, shivering threshold alerts.
  - Bottom: [ORDER IMMEDIATE EXTERIOR RECALL], [BOOST RADIANT AIRLOCK HEAT], [DISPENSE THERMAL BALM].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 98: UI_PANEL_SNOW_MELTER_FLUE_HEAT_HARVESTER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Water Generation & Heat Recovery
• Core Linkage: Ashfall.Core.WaterPurificationSystem, WeatherSystem
• Wireframe:
  - Left: Exterior snow feed hopper, snow density meter (Powder / Wind-pack / Black Ice), conveyor feed rate.
  - Center: Flue gas heat-exchange melting grid, melting water runoff temperature (+8°C), sediment grit trap.
  - Right: Melted snowwater pre-filtration tank, gross radionuclide particulate screen, boiler feedwater makeup pump.
  - Bottom: [DUMP HOPPER SNOW CHARGE], [FLUSH SEDIMENT GRIT TRAP], [DIVERT TO POTABLE CISTERN].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 99: UI_PANEL_PERMAFROST_TUNNEL_CRYOGENIC_CREEP
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Subterranean Geotechnical Safety
• Core Linkage: Ashfall.Core.StructuralIntegritySystem, WeatherSystem
• Wireframe:
  - Left: Deep permafrost tunnel wall convergence calipers (mm/month), ground ice temperature (-8°C).
  - Center: 3D cross-section tunnel mesh showing plastic cryogenic ice creep deformation, rock bolt tension load cells.
  - Right: Acoustic ice fracture sensors, thermal insulation barrier integrity, sub-zero refrigeration loop telemetry.
  - Bottom: [ENGAGE PERMAFROST FREEZE PIPES], [TRIM CRYOGENIC ICE DEFORMATION], [RE-TORQUE TUNNEL RIBS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 100: UI_PANEL_SUB_GLACIAL_LAKE_HOT_WATER_DRILL
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Deep Exploration & Primitive Water Siphon
• Core Linkage: Ashfall.Core.Expeditions.ExpeditionSystem, WaterPurificationSystem
• Wireframe:
  - Left: High-pressure hot water boiler (90°C), triplex plunger pump pressure (120 Bar), insulated hose reel.
  - Center: Sub-glacial hot water jet drill head depth meter (220m), ice penetration speed (m/hr), borehole return water.
  - Right: Sub-glacial lake sonar cavity mapping, sterile borehole casing sleeve, pristine pre-war water sampling probe.
  - Bottom: [ACTIVATE THERMAL WATER JET], [INSERT STERILE SAMPLING TUBE], [EXTRACT SUB-GLACIAL WATER].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 101: UI_PANEL_AURORA_IONOSPHERIC_RAD_MONITOR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Atmospheric Physics & Space Weather
• Core Linkage: Ashfall.Core.WeatherSystem, RadioSystem
• Wireframe:
  - Left: Solar wind geomagnetic storm index (Kp-Index: 8 - Severe), magnetosphere compression depth.
  - Center: High-altitude CRT ionospheric absorption visualizer (Riometer trace), auroral secondary cosmic ray shower flux.
  - Right: Long-distance high-frequency (HF) radio blackout forecast, surface ground induced currents (GIC) telemetry.
  - Bottom: [DISCONNECT LONG-WIRE ANTENNAS], [LOG IONOSPHERIC EVENT], [PREDICT RADIO PROPAGATION WINDOW].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 102: UI_PANEL_INSULATION_AEROGEL_FABRICATION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Advanced Material Science & Survival Gear
• Core Linkage: Ashfall.Core.CraftingSystem, InventorySystem
• Wireframe:
  - Left: Silica alkoxide sol-gel chemical mixing vat, ethanol solvent exchange bath, gel aging timer.
  - Center: High-pressure supercritical CO2 extraction autoclave (1,100 PSI, 31°C), solvent depressurization curve.
  - Right: Ultra-light silica aerogel blanket inspection, thermal conductivity meter (k: 0.014 W/m·K), sub-zero parka lining queue.
  - Bottom: [VENT SUPERCRITICAL CO2], [SLICE AEROGEL BLANKET], [INSTALL THERMAL SUIT LINING].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 103: UI_PANEL_RAD_RESISTANT_TILAPIA_AQUACULTURE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Food Production & Protein Synthesis
• Core Linkage: Ashfall.Core.FoodProductionSystem, WaterPurificationSystem
• Wireframe:
  - Left: Recirculating aquaculture tanks (Tanks 1 to 4), water temperature (+26°C), dissolved oxygen (DO mg/L).
  - Center: Biological fluidized bed biofilter (Nitrosomonas / Nitrobacter bacteria conversion), ammonia/nitrite meters.
  - Right: Fish stock biomass weight calculation, insect larvae feeding dispenser, daily protein harvest projections.
  - Bottom: [HARVEST MATURE TILAPIA], [FEED PROTEIN INSECT MEAL], [BACKWASH BIOFILTER BED].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 104: UI_PANEL_BLACK_SOLDIER_FLY_LARVAE_VAT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Waste Recycling & Bioconversion
• Core Linkage: Ashfall.Core.FoodProductionSystem, NeedsSystem
• Wireframe:
  - Left: Organic cafeteria waste input bin, moisture balancing dry bran, bioconversion substrate bed.
  - Center: Vertical larval breeding shelves, temperature / humidity climatic controller (+30°C, 70% RH), fly mating cage.
  - Right: Self-harvesting prepupae migration ramp, larval drying / crushing mill, high-fat protein feed stock.
  - Bottom: [LOAD ORGANIC WASTE SLURRY], [COLLECT HARVESTED PUPAE], [CRUSH INTO LIVESTOCK MEAL].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 105: UI_PANEL_HYDROLOGICAL_SILT_SEDIMENTATION_BASIN
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Water Ingress & Physical Clarification
• Core Linkage: Ashfall.Core.WaterPurificationSystem, EconomySystem
• Wireframe:
  - Left: Inflow raw river water turbidimeter (NTU), coarse trash rack debris sensor, water inlet sluice gate.
  - Center: Clarification sedimentation basin with inclined lamella clarifier plates, aluminum sulfate flocculant dosing pump.
  - Right: Bottom cone sludge collection hopper, automated sludge dump valve timer, clarified water overflow weir.
  - Bottom: [PURGE BASIN SLUDGE CONE], [ADJUST FLOCCULANT DOSAGE], [DIVERT CLARIFIED WATER].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 106: UI_PANEL_APICULTURE_RAD_POLLEN_HONEY_HIVE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Greenhouse Ecology & Medical Honey
• Core Linkage: Ashfall.Core.FoodProductionSystem, MedicalSystem
• Wireframe:
  - Left: Sealed greenhouse beehives (Hives Alpha & Beta), queen bee pheromone detector, colony acoustic buzz frequency.
  - Center: Hive entrance radionuclide electrostatic pollen stripper screen, bee flight activity counter, ambient hive temp.
  - Right: Raw medical honey combs, antibacterial peroxide index, medicinal burn dressing ointment synthesis.
  - Bottom: [EXTRACT MEDICINAL HONEY], [CLEAN POLLEN STRIPPER SCREEN], [INSPECT BROOD COMB HEALTH].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 107: UI_PANEL_MUTATED_YEAST_GENOME_CULTIVATION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Biotechnology & Vitamin Synthesis
• Core Linkage: Ashfall.Core.FoodProductionSystem, Medical.AfflictionPipeline
• Wireframe:
  - Left: Yeast propagation bioreactor vat (1,000L), glucose/molasses feed rate, sterile air sparger DO level.
  - Center: Radiation-adapted yeast strain telemetry (DNA repair enzyme over-expression), cell density optical spectrometer.
  - Right: Yeast autolysis separation centrifuge, Vitamin B-complex / Thiamine paste recovery tank, debittered nutrient yeast.
  - Bottom: [INITIATE CELL AUTOLYSIS], [EXTRACT B-VITAMIN PASTE], [INOCULATE FRESH GLUCOSE BATCH].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 108: UI_PANEL_MUSHROOM_MYCOREMEDIATION_FILTER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Biological Decontamination & Rad-Extraction
• Core Linkage: Ashfall.Core.RadiationSystem, FoodProductionSystem
• Wireframe:
  - Left: Heavy radionuclide wastewater input reservoir (Strontium-90 / Cesium-137 contaminated liquor).
  - Center: Submerged mycelial fungal biomass filtration column (Pleurotus radiotolerans), biosorption uptake rate (Bq/kg).
  - Right: Saturated mycelium bio-cake disposal status, rad-shielded incineration kiln, decontaminated effluent clarity.
  - Bottom: [HARVEST SATURATED MYCELIUM], [INCINERATE SPENT FUNGAL CAKE], [RELOAD STERILE HYPHAE BED].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 109: UI_PANEL_ONE_TIME_PAD_CIPHER_CRYPTO_VAULT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Military Intelligence & Espionage
• Core Linkage: Ashfall.Core.RadioSystem, SocialSystem
• Wireframe:
  - Left: Mechanical pseudorandom letter drum generator, radioactive decay entropy source (Alpha pulse diode).
  - Center: One-Time Pad (OTP) page visualizer, 5-letter cryptographic groups, Vernam cipher modulo-26 calculation grid.
  - Right: Encrypted outbound diplomatic dispatches, destroyed used pad incinerator log, cryptographic security index (100%).
  - Bottom: [GENERATE RANDOM CIPHER PAD], [ENCRYPT EXPEDITION DISPATCH], [DESTROY USED KEY PAGE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 110: UI_PANEL_ACOUSTIC_TELEGRAPH_EARTH_RETURN
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Post-EMP Hardened Communications
• Core Linkage: Ashfall.Core.RadioSystem, PowerGridSystem
• Wireframe:
  - Left: Sub-surface bedrock audio galvanic electrodes, ground loop electrical resistance (Ohms), audio carrier frequency.
  - Center: Low-frequency acoustic ground transmission visualizer, audio hum waveform, Morse key telegraph transmitter.
  - Right: Received ground-wave audio filter, narrow-band DSP hum discriminator, regional bunker telegraph line status.
  - Bottom: [TRANSMIT EARTH-WAVE TELEGRAPH], [TUNE GROUND AUDIO DISCRIMINATOR], [BOOST GALVANIC ELECTRODE CURRENT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 111: UI_PANEL_ANALOG_COMPUTING_FIRE_CONTROL_GEAR
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Mechanical Ballistics & Hardware
• Core Linkage: Ashfall.Core.CombatSystem, ShelterDefenseSystem
• Wireframe:
  - Left: Mechanical differential gear train schematic, ball-and-disk integrator RPM, gear backlash tolerance.
  - Center: 3D mechanical ballistic cam visualization, range/wind deflection mechanical cams, hand-crank manual resolver.
  - Right: Turret electrical synchro transmitter signals, automated lead angle output, firing solution azimuth/elevation.
  - Bottom: [CRANK RANGE RESOLVER], [ENGAGE CAM LEAD TRACKING], [TRANSMIT TURRET SYNCHRO DRIVE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 112: UI_PANEL_SHORTWAVE_NUMBERS_STATION_INTERCEPT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: World Lore, Enigma & Secret Factions
• Core Linkage: Ashfall.Core.RadioSystem, Narrative
• Wireframe:
  - Left: Shortwave radio waterfall display (4.625 MHz / 7.120 MHz), synthetic synthesized phonetic voice audio visualizer.
  - Center: Received number sequence log (e.g., "38291 04918 84729 19482"), interval broadcast schedule timer.
  - Right: Decryption keybook library (Factions: Meridian Compact, Black Sky Cell, Vault Overseers), decrypted order preview.
  - Bottom: [RECORD NUMBER BROADCAST], [APPLY CIPHER BOOK], [UNCOVER HIDDEN WASTELAND CACHE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 113: UI_PANEL_LASER_OPTICAL_LINE_OF_SIGHT_COMMS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Tactical Silent Communications
• Core Linkage: Ashfall.Core.RadioSystem, ShelterDefenseSystem
• Wireframe:
  - Left: Optical transceiver alignment telescope, crosshair pointing azimuth/elevation to mountain relay outpost.
  - Center: Infrared semiconductor laser diode transmitter (850nm), pulse-position modulation (PPM) bit rate (100 kbps).
  - Right: Atmospheric blizzard scatter attenuation meter, photodiode receiver signal-to-noise ratio, secure line status.
  - Bottom: [TRANSMIT OPTICAL LASER BURST], [CO-ALIGN OPTICAL TELESCOPE], [SWITCH TO INFRARED FILTER].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 114: UI_PANEL_SECTOR_GRID_POWER_BALANCING_LOAD_DISPATCH
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Centralized Power Grid Management
• Core Linkage: Ashfall.Core.PowerGridSystem, GameBootstrap
• Wireframe:
  - Left: Generator supplies (Geothermal Turbine: 45 kW, RTG: 12 kW, Diesel: 80 kW, Battery: 20 kW), frequency synchroscope.
  - Center: Shelter electrical grid load balancing matrix: Priority Tier 1 (Life Support), Tier 2 (Defense), Tier 3 (Workshops).
  - Right: Line frequency meter (50.0 Hz target), transformer core temperatures, automated breaker trip status.
  - Bottom: [TRIM GENERATOR GOVERNOR], [SHED NON-ESSENTIAL WORKSHOPS], [COMMENCE EMERGENCY DIESEL COLD-START].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 115: UI_PANEL_NOBODYS_CHARTER_CROSSING_EXPEDITION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Expansion 04: The Great Crossing
• Core Linkage: Ashfall.Core.Expansions.NobodysCharter, ExpeditionSystem
• Wireframe:
  - Left: Arctic Crossing trek manifest (32 survivors, 4 cargo sledges, 12 draught animals), frostbite injury index.
  - Center: Topographical frozen mountain pass map, crevasse hazard markers, blizzard shelter waystations (Waypoints 1 to 7).
  - Right: Daily caloric / fuel depletion curve, sled mechanical breakdown probability, remaining distance to Nobody's Crossing.
  - Bottom: [ORDER FORCED MARCH (12H)], [MAKE TEMPORARY SNOW CAMP], [SACRIFICE HEAVY SLED CARGO].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 116: UI_PANEL_VERDICT_COUNCIL_EXECUTIVE_DECREE_LOG
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Expansion: Verdict / Governance Law
• Core Linkage: Ashfall.Core.Verdict, SocialSystem
• Wireframe:
  - Left: Active executive emergency decrees (Mandatory Rationing, 20:00 Curfew, Seizure of Private Weapons, Forced Labor).
  - Center: Shelter civil liberties vs stability balance scale, unrest index histogram, protest skirmish probability.
  - Right: Council member vote records, executive veto tokens remaining, citizen compliance rating percentage.
  - Bottom: [ENACT NEW EMERGENCY DECREE], [REPEAL MARTIAL LAW ORDER], [CALL SPECIAL COUNCIL SESSION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 117: UI_PANEL_SILENT_FOUNDRY_AUTOMATION_CORE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Expansion: Silent Foundry / Robotic Industry
• Core Linkage: Ashfall.Core.SilentFoundry, CraftingSystem
• Wireframe:
  - Left: Relic automated factory controller, punch-card program reader hopper, memory drum vacuum tube banks.
  - Center: 3D gantry robot arm status, tool changer turret (Milling Head, Plasma Torch, Gripper Jaw), axis position telemetry.
  - Right: Precision manufacturing queue (Engine blocks, hardened armor plates, artillery barrels), scrap metal feedstock level.
  - Bottom: [FEED AUTOMATION PUNCH-CARD], [RE-CALIBRATE GANTRY AXES], [OVERRIDE EMERGENCY E-STOP].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 118: UI_PANEL_DEEP_COAST_LIGHTHOUSE_FOG_BEACON
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Expansion: Deep Coast / Maritime Recon
• Core Linkage: Ashfall.Core.DeepCoast, MaritimeSystem
• Wireframe:
  - Left: Lighthouse tower elevation telemetry (45m bluff), wind gusts (95 km/h), acetylene gas generator pressure.
  - Center: Giant Fresnel lens rotating optic, motorized clockwork drive weight, high-intensity tungsten halogen searchlight beam.
  - Right: Compressed air foghorn acoustic transmitter, coastal reef hazard map, inbound salvage ship radar return.
  - Bottom: [ACTIVATE FOGHORN CHIRP], [WIND CLOCKWORK ROTATION DRIVE], [IGNITE ACETYLENE SEARCHLIGHT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 119: UI_PANEL_MEMORIAL_CENOTAPH_INSCRIPTION_ENGRAVER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Memorial, Social Morale & Final Wish
• Core Linkage: Ashfall.Core.FinalWishSystem, Journal
• Wireframe:
  - Left: Master Roll of Fallen Survivors (Names, Ages, Occupations, Dates of Death, Burial Catacomb Slots).
  - Center: Granite cenotaph slab stone carving layout, pneumatic chisel lettering font preview, commemorative epitaph editor.
  - Right: Community collective grief alleviation index, permanent memorial morale buffer (+15% Morale floor).
  - Bottom: [ENGRAVE MEMORIAL EPITAPH], [POLISH GRANITE CENOTAPH], [DEDICATE MEMORIAL TO VAULT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 120: UI_PANEL_ATMOSPHERIC_RESTORATION_GLOBAL_PROJECTION
────────────────────────────────────────────────═══════════
• Domain: Endgame Evaluation, Planetary Climatology & Victory
• Core Linkage: Ashfall.Core.EndgameSystem, YearOfAsh
• Wireframe:
  - Left: Global atmospheric metrics (Stratospheric soot loading Tg, Solar transmittance %, Ozone layer Dobson units).
  - Center: 100-Year planetary climate restoration projection globe, retreating permafrost boundary, returning rainfall belts.
  - Right: Human civilization rebirth milestones (Agricultural self-sufficiency, Surface settlement founding, Cosmic dawn).
  - Bottom: [RECORD GRAND FINALE CHRONICLE], [BROADCAST REBIRTH HYMN], [COMMENCE POST-WAR GENERATION].
```

---

## 2. The Complete 120-Panel Master Reference Matrix

```
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                          ASHFALL 120-PANEL UI COMPLETE MATRIX                           │
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
└────┴────────────────────────────────────────────┴───────────────────────────────────────┘
```
