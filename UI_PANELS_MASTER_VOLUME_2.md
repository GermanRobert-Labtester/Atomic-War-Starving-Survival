# ASHFALL: Atomic War - Starving Survival
# Master UI Panels Volume 2 (Panels 31–60) & Complete 60-Panel Matrix

```
═══════════════════════════════════════════════════════════════════════════════════════════
  PROJECT: ASHFALL (2D Atomic-War Survival)
  DOCUMENT: Master UI Specification Volume 2 (Panels 31 to 60)
  THEME: Cold Survival / Scavenged Field Manual
  COLOR TOKENS: Dark Charcoal (#131313) | Ashen Grey (#D1D5DB) | Muted Teal (#2D5A5E) | Burnt Orange (#CC5500)
  ASSETS LOCATION: assets/ui/Icons/
═══════════════════════════════════════════════════════════════════════════════════════════
```

---

## 1. Tactical UI Assets in Game Repository (`assets/ui/Icons/`)

| Asset Filename | System Domain | Description |
| :--- | :--- | :--- |
| `geiger_counter_ui_icon_1787040121431.jpg` | Radiation System | Worn tactical analog dosimeter & Geiger-Müller counter |
| `chelation_ampoules_ui_icon_1787040140994.jpg` | Toxicological Medicine | EDTA & Prussian Blue anti-rad vials in lead-lined case |
| `blast_door_controller_ui_icon_1787040157036.jpg` | Shelter Defense & Armor | Cast-iron hydraulic bulkhead pressure control & locking wheel |
| `ration_conflict_tokens_ui_icon_1787040169401.jpg` | Ration & Social Governance | Caloric Emergency foil ration pouch with stamped zinc tokens |
| `somatic_trauma_ecg_ui_icon_1787040193733.jpg` | Psychiatric Rehabilitation | Analog biometrics ECG monitor with sedative autoinjector ampoule |
| `emp_vacuum_tube_bus_ui_icon_1787040204892.jpg` | Electronic Hardening | EMP hardened vacuum tube relay bank with Faraday copper mesh |
| `geothermal_turbine_ui_icon_1787040435148.jpg` | Geothermal Power | Heavy bronze steam turbine manifold with pressure gauge |
| `radio_triangulation_scope_ui_icon_1787040448216.jpg` | Radio & Signal Intercept | Cathode ray tube directional radio triangulation sweep scope |
| `reloading_press_ammo_ui_icon_1787040461364.jpg` | Armory & Ballistics | Bench-mounted reloading press, bullet casting mold, powder hopper |
| `cryo_seed_canister_ui_icon_1787040479871.jpg` | Botanical & Greenhouse | Liquid nitrogen Dewar flask with heirloom seed samples (-196°C) |
| `vitrification_cask_ui_icon_1787040493705.jpg` | Nuclear Waste Management | Hexagonal reinforced lead-shielded vitrified rad-waste canister |

---

## 2. Panels 31–60 Detailed Architectural Specifications

```
───────────────────────────────────────────────────────────────────────────────────────────
PANEL 31: UI_PANEL_DEEP_STRATA_GEOTHERMAL_BOREHOLE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Shelter Infrastructure & Power Generation
• Core Linkage: Ashfall.Core.PowerGridSystem, Ashfall.Core.WeatherSystem
• Wireframe:
  - Left: Subterranean strata depth log (350m borehole), rock temperature gradient (+118°C), thermistor array.
  - Center: Geothermal steam turbine manifold, throttle valve handwheel, steam loop pressure PSI dial, turbine RPM.
  - Right: Electrical alternator output (kW), parasitic pump load, condenser cooling loop temperature.
  - Bottom: [THROTTLE STEAM INTAKE], [FLUSH MINERAL SCALE], [DIVERT STEAM TO HYDROPONICS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 32: UI_PANEL_RADIO_ECHO_TRIANGULATION_SCANNER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Narrative, Radio Intercept & Scavenging
• Core Linkage: Ashfall.Core.RadioSystem, Ashfall.Core.Journal
• Wireframe:
  - Left: Scanned frequency band (VHF/Shortwave/Longwave), receiver sensitivity, background static noise level.
  - Center: Circular CRT radar sweep screen showing directional signal bearing, triangulation angle arcs, carrier waveform.
  - Right: Signal demodulation log, automated Morse decoder text ribbon, cipher decryption progress bar.
  - Bottom: [CALIBRATE LOOP ANTENNA], [TRANSMIT HOMING CHIRP], [LOG TRANSMISSION TO CODEX].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 33: UI_PANEL_PROSTHETICS_FIELD_ORTHOPEDICS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Medical & Surgical Recovery
• Core Linkage: Ashfall.Core.Medical.MedicalSystem, Ashfall.Core.AfflictionPipeline
• Wireframe:
  - Left: Amputee survivor dossier, stump skin integrity, nerve pain rating, mobility deficit score.
  - Center: Mechanical prosthetic assembly bench (Peg-leg, Scavenged Steel Arm, Pneumatic Knee Joint), fitment tolerances.
  - Right: Cushioning liner materials (Neoprene, Treated Wool), strap tensioners, anti-microbial zinc powder level.
  - Bottom: [REFIT SOCKET LINER], [LUBRICATE MECHANICAL PIVOT], [EQUIP REINFORCED PROSTHESIS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 34: UI_PANEL_FERMENTATION_ETHANOL_BIOFUEL
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Economy, Crafting & Shelter Power
• Core Linkage: Ashfall.Core.Economy.CraftingSystem, InventorySystem
• Wireframe:
  - Left: Fermentation vat ingredients (Sugar Beet mash, Moldy grain, Potato peelings), yeast culture viability, mash SG.
  - Center: Copper pot still reflux column, column head vapor temperature dial (78.4°C ethanol cutoff), cooling condenser.
  - Right: Output receiver barrels (High-Proof Medical Spirit 96%, Vehicle Biofuel 85%, Low-Grade Fuel Spirits).
  - Bottom: [COLLECT HEADS/TAILS FRACTION], [TRANSFER BIOFUEL TO FUEL VAULT], [STOKE STILL BURNER].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 35: UI_PANEL_AIRLOCK_DECONTAMINATION_CASCADE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Radiation Protection & Survivor Ingress
• Core Linkage: Ashfall.Core.RadiationSystem, Ashfall.Core.SurvivorWorkShiftSystem
• Wireframe:
  - Left: Ingress team dosimeter readings, surface particulate dust load (CPM), biological contamination flag.
  - Center: 3-Stage airlock cascade visual: Stage 1 (Dry pneumatic boot scrape), Stage 2 (Chemical foaming wash), Stage 3 (Air shower).
  - Right: Effluent drainage sump tank level, rad-water filter canister lifetime, positive pressure seal indicator.
  - Bottom: [INITIATE CASCADE CYCLE], [PURGE SUMP RAD-EFFLUENT], [UNLOCK INNER BUNKER HATCH].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 36: UI_PANEL_CARAVAN_LIVESTOCK_MULE_STABLE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Logistics, Travel & Expedition Support
• Core Linkage: Ashfall.Core.ExpeditionSystem, NeedsSystem
• Wireframe:
  - Left: Pack animal roster (Irradiated draught mules, sled dogs, woolly goats), stamina bars, rad sickness indicators.
  - Center: Stable ventilation, straw bedding cleanliness, hay/lichen fodder reserve days, water trough salinity.
  - Right: Pack saddle harness condition, load capacity allocation (kg per animal), hoof iron shoeing wear.
  - Bottom: [TREAT RADIATION BURNS], [ALLOCATE HIGH-CALORIE FODDER], [HARNESS FOR EXPEDITION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 37: UI_PANEL_AMMO_RELOADING_PRESS_BALLISTICS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Armory & Munitions Production
• Core Linkage: Ashfall.Core.CraftingSystem, CombatSystem
• Wireframe:
  - Left: Component inventory (Spent Brass Casings, Box of Primers, Smokeless Powder Grains, Cast Lead Bullets).
  - Center: Reloading station press stage (Decap/Resize, Prime, Charge Powder, Seat Bullet, Crimp Case Mouth).
  - Right: Ballistics spec sheet (Caliber: 7.62x39mm / 9x18mm / 12ga Slug), projected muzzle velocity, misfire risk percentage.
  - Bottom: [BATCH PRESS 50 ROUNDS], [ADJUST POWDER CHARGE (GRAINS)], [MELT SCRAP LEAD INGOTS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 38: UI_PANEL_PERMAFROST_SEED_BANK_CRYOGENICS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Science, Long-Term Survival & Greenhouse
• Core Linkage: Ashfall.Core.FoodProductionSystem, YearOfAsh
• Wireframe:
  - Left: Frozen seed vault inventory (Ancient rye, Rad-resistant brassica, Nitrogen-fixing clover), germination percentage.
  - Center: Liquid nitrogen Dewar cooling array, digital temperature display (-196°C), vacuum jacket seal integrity.
  - Right: Thawing protocol chamber, slow-acclimation moisture gradient, germination test petri dishes.
  - Bottom: [INITIATE STAGED THAW], [REFILL LIQUID NITROGEN], [TRANSFER TO HYDROPONICS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 39: UI_PANEL_SHELTER_BATTERY_FLYWHEEL_KINETICS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Power Storage & Emergency Reserve
• Core Linkage: Ashfall.Core.PowerGridSystem
• Wireframe:
  - Left: Substation battery bank status (Lead-acid cells 1-24), individual cell specific gravity, sulfation level.
  - Center: Vacuum-enclosed kinetic flywheel schematic, magnetic bearing levitation status, rotational speed (18,000 RPM).
  - Right: Emergency power reserve discharge rate, automated load shedding priority tiers (Life Support > Lights > Defense).
  - Bottom: [DISENGAGE FLYWHEEL CLUTCH], [RUN EQUALIZATION CHARGE], [ACTIVATE EMERGENCY BLACKOUT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 40: UI_PANEL_COALITION_PACT_DIPLOMATIC_ENVOY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Geopolitics & Faction Diplomacy
• Core Linkage: Ashfall.Core.Factions.FactionSystem, MoralBranchingSystem
• Wireframe:
  - Left: Faction envoy profile (Meridian Compact, Iron Nomads, Undertow), diplomatic credentials, bodyguard armaments.
  - Center: Diplomatic treaty draft table (Mutual Defense, Shared Radio Frequencies, Grain Corridor Access, Non-Aggression).
  - Right: Faction territory border map, disputed resource waystations, trust reputation meters.
  - Bottom: [RATIFY COALITION ACCORD], [DEMAND RESOURCE CONCESSION], [EXPEL DIPLOMATIC ENVOY].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 41: UI_PANEL_CHRONIC_RADIATION_LEUKEMIA_LEDGER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Medical & Long-Term Rad Afflictions
• Core Linkage: Ashfall.Core.RadiationSystem, Ashfall.Core.Medical.MedicalSystem
• Wireframe:
  - Left: Chronic patient list, lifetime cumulative sievert exposure (>2.5 Sv), White Blood Cell count (x10^9/L).
  - Center: Bone marrow micro-cellular health scan, platelet deficiency bars, internal hemorrhage warning indicators.
  - Right: Palliative care regimens (Whole blood transfusion, pain relief morphine, antimicrobial prophylaxis).
  - Bottom: [SCHEDULE WHOLE BLOOD TRANSFUSION], [INCREASE PALLIATIVE COMFORT], [RECORD PATIENT PROGNOSIS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 42: UI_PANEL_SUNKEN_HARBOR_DIVING_BELL_SALVAGE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Deep Scavenging & Coastal Exploration
• Core Linkage: Ashfall.Core.Expeditions.ExpeditionSystem, MaritimeSystem
• Wireframe:
  - Left: Flooded coastal industrial sector bathymetry map, water depth (45m), underwater current velocity.
  - Center: Diving bell descent winch control, air umbilical pressure dial, diver suit comms audio visualizer.
  - Right: Recovered wet salvage cargo list (Waterproof ammunition canisters, sealed pre-war electronics, brass fittings).
  - Bottom: [HOIST SALVAGE CRANE], [BOOST DIVING BELL AIR OXYGEN], [INITIATE EMERGENCY SURFACE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 43: UI_PANEL_MARITIME_ICEBREAKER_BOILER_TELEMETRY
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Coastal Transport & Maritime Expeditions
• Core Linkage: Ashfall.Core.MaritimeSystem, WeatherSystem
• Wireframe:
  - Left: Coastal trawler hull condition, reinforced ice-ram bow stress strain-gauge, sea ice thickness (1.2m).
  - Center: Heavy Scotch marine steam boiler diagram, firebox temperature (850°C), steam pressure PSI, propeller shaft RPM.
  - Right: Bunker fuel oil tanks, feedwater heater level, bilge water pump telemetry.
  - Bottom: [FULL SPEED AHEAD (RAM ICE)], [BLOW DOWN BOILER TUBES], [DROP EMERGENCY HARBOR ANCHOR].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 44: UI_PANEL_SMUGGLER_DEAD_DROP_NETWORK
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Black Market & Undercover Trade
• Core Linkage: Ashfall.Core.Economy.DynamicEconomySystem, QuestSystem
• Wireframe:
  - Left: Secret dead-drop GPS coordinate list (Caches 1 to 8), last access timestamp, proximity security threat.
  - Center: Concealed cache container breakdown (Hollowed lead battery, buried mortar tube, sunken buoy).
  - Right: Cipher-locked barter escrow items, smuggler token currency balance, encrypted frequency drop codes.
  - Bottom: [DEPOSIT CONTRABAND GOODS], [RETRIEVE ESCROW PAYOUT], [REKEY CIPHER ENCRYPTION].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 45: UI_PANEL_MUTATED_FLORA_BOTANICAL_HERBARIUM
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Science, Botany & Herbal Remedies
• Core Linkage: Ashfall.Core.FoodProductionSystem, MedicalSystem
• Wireframe:
  - Left: Herbarium specimen binder (Ash-Rose, Glowing Mycelium, Iron-Bark Lichen, Toxic Night-Berry).
  - Center: Botanical drawing overlay, active alkaloid profile, radiation tolerance index, medicinal extraction properties.
  - Right: Drying rack status, tincture solvent alcohol levels, herbal poultice apothecary stock.
  - Bottom: [EXTRACT MEDICINAL ALKALOID], [PREPARE BURN POULTICE], [LOG SPECIMEN DISCOVERY].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 46: UI_PANEL_SURVIVOR_FUNERAL_CATALYZER_PYRE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Social Morale & Memorial Systems
• Core Linkage: Ashfall.Core.FinalWishSystem, SocialSystem
• Wireframe:
  - Left: Deceased survivor dossier, cause of death, shelter service record, grieving family members / close companions.
  - Center: Memorial crematorium chamber / vault catacomb burial slot diagram, memorial inscription text editor.
  - Right: Heirloom keepsake distribution log, memorial token registry, shelter grief & morale recovery timer.
  - Bottom: [CONDUCT SOLEMN FUNERAL RITE], [INSCRIBE ROLL OF HONOR], [DISPERSE ASHES TO WASTELAND].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 47: UI_PANEL_RAD_SHIELD_SKY_LAYER_BALLAST
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Shelter Defense & Structural Engineering
• Core Linkage: Ashfall.Core.ShelterDefenseSystem, RadiationSystem
• Wireframe:
  - Left: Shelter roof load-bearing strain gauges, ceiling deflection sensors (mm), overhead structural concrete columns.
  - Center: Multi-layered sky-armor schematic (Crushed Basalt, Lead Ingot Deck, Earth Berm, Blast Deflector Plates).
  - Right: Overhead gamma radiation attenuation factor (Tenth-Value Layer calculation), roof ballast hopper fill levels.
  - Bottom: [DISCHARGE BALLAST OVERHEAD], [INSPECT STRUCTURAL PILLARS], [DEPLOY OVERHEAD LEAD SHIELDS].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 48: UI_PANEL_MORAL_BRANCHING_EXILE_BALLOT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Moral Governance & Faction Laws
• Core Linkage: Ashfall.Core.MoralBranchingSystem, NobodysCharter
• Wireframe:
  - Left: Accused survivor info, offenses against community survival, defense plea statement.
  - Center: Secret wooden ballot box voting tally (Mercy / Hard Labor / Immediate Wasteland Exile), voter turnout.
  - Right: Exile survival kit provisioner (3 days water, broken respirator, no firearm), projected faction unrest delta.
  - Bottom: [SEAL VOTING BALLOT BOX], [CERTIFY EXILE DECREE], [COMMUTE SENTENCE BY EXECUTIVE PARDON].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 49: UI_PANEL_EXPEDITION_AMBUSH_TACTICAL_GRID
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Combat, Tactics & Survival Skirmishes
• Core Linkage: Ashfall.Core.CombatSystem, ExpeditionSystem
• Wireframe:
  - Left: Squad turn order, individual action points (AP), cover status (Full / Half / Exposed), health & suppression bars.
  - Center: Top-down isometric wasteland skirmish grid, cover elevations, line-of-sight cones, grenade blast radiuses.
  - Right: Targeted enemy intel (Armor plating, equipped weapon, flanking vulnerability), hit probability calculation (%).
  - Bottom: [TAKE AIM & FIRE], [SEEK REINFORCED COVER], [ORDER TACTICAL SMOKE RETREAT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 50: UI_PANEL_HYDROLOGICAL_DESALINATION_COLUMN
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Water Purification & Hydro-Geology
• Core Linkage: Ashfall.Core.WaterPurificationSystem, EconomySystem
• Wireframe:
  - Left: Raw contaminated intake water telemetry (Salinity ppm, Radionuclide Bq/L, Heavy metals turbidity).
  - Center: Multi-effect evaporation column diagram, steam heating coils, condensing baffles, vacuum pump pressure.
  - Right: Purified distillate mineral balancing injector (Calcium carbonate, trace salts), discharge conductivity monitor.
  - Bottom: [ACTIVATE EVAPORATOR STAGE 2], [INJECT MINERAL SALTS], [DIVERT CONTAMINATED SLUDGE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 51: UI_PANEL_RADIOACTIVE_WASTE_VITRIFICATION
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Nuclear Waste Management & Shelter Safety
• Core Linkage: Ashfall.Core.RadiationSystem, CraftingSystem
• Wireframe:
  - Left: Concentrated radioactive residue sump level (Filter cakes, chelation waste, de-con sludge), Curie radioactivity.
  - Center: High-temperature vitrification furnace (1,150°C), borosilicate glass frit mixing hopper, pouring nozzle.
  - Right: Hexagonal lead-shielded dry cask storage slots (Casks 1-12), external cask radiation leakage dosimeter readings.
  - Bottom: [POUR VITRIFIED GLASS CASK], [SEAL LEAD STORAGE LID], [LOWER CASK INTO DEEP VAULT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 52: UI_PANEL_PRE_WAR_TECHNICAL_BLUEPRINT_ARCHIVE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Research, Science & Engineering
• Core Linkage: Ashfall.Core.ResearchSystem, GameBootstrap
• Wireframe:
  - Left: Microfilm aperture card catalog (Industrial, Medical, Agricultural, Defensive Schematics), damage percentage.
  - Center: Optical micro-fiche reader lightbox view, high-resolution technical cutaway blueprints with annotations.
  - Right: Reverse-engineering research tree progress bar, required workshop tools, technological unlock benefits.
  - Bottom: [BEGIN BLUEPRINT DECODING], [DIGITIZE TECHNICAL SCHEMATIC], [FABRICATE PROTOTYPE COMPONENT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 53: UI_PANEL_RAD_STORM_SHELTER_CURFEW_PROTOCOL
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Emergency Response & Shelter Governance
• Core Linkage: Ashfall.Core.WeatherSystem, ShelterDefenseSystem
• Wireframe:
  - Left: Approaching seasonal rad-storm intensity (Category 4 Ash-Front), estimated time of arrival (ETA: 02h 15m).
  - Center: Shelter internal sector lockdown board (Bulkheads 1 to 6 status), automated magnetic door releases.
  - Right: Survivor muster roster (Accounted for: 34 / In Danger Zone: 2), emergency iodine distribution status.
  - Bottom: [SOUND RAD-STORM CURFEW SIREN], [SEAL ALL INTERNAL BULKHEADS], [DISPENSE EMERGENCY IODINE].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 54: UI_PANEL_OPTICAL_RANGEFINDER_SNIPER_POST
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Perimeter Defense & Reconnaissance
• Core Linkage: Ashfall.Core.CombatSystem, ShelterDefenseSystem
• Wireframe:
  - Left: Observation cupola environmental readings (Crosswind m/s, thermal shimmer distortion, range to target).
  - Center: Stereoscopic optical rangefinder viewfinder with dual split-image coincidence dial and mil-dot reticle.
  - Right: Sector perimeter threat logging (Scavenger scouts, radioactive beasts, approaching convoys), designated fire zones.
  - Bottom: [LOCK DISTANCE TO TARGET], [MARK SECTOR THREAT], [ORDER WARNING SHOT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 55: UI_PANEL_SURVIVOR_PSYCHOSOCIAL_JOURNAL_CHRONICLE
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Narrative, Journal & Morale
• Core Linkage: Ashfall.Core.Journal, SocialSystem
• Wireframe:
  - Left: Date chronicle index (Day 1 to Current Day), major landmark historical events, community milestones.
  - Center: Worn handwritten diary journal notebook with scanned survivor handwriting, sketches, and emotional reflections.
  - Right: Sentiment analysis breakdown (Hope vs Despair score, community cohesion index, collective trauma resilience).
  - Bottom: [PEN DAILY CHRONICLE ENTRY], [ATTACH ARTIFACT SKETCH], [ARCHIVE TO VAULT HISTORICAL RECORD].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 56: UI_PANEL_IMPROVISED_BATTERY_ELECTROLYTE_SYNTHESIS
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Crafting & Chemistry
• Core Linkage: Ashfall.Core.CraftingSystem, PowerGridSystem
• Wireframe:
  - Left: Chemical feedstocks (Scrap lead plates, distilled water, concentrated sulfur, car battery acid residues).
  - Center: Acid dilution & balancing tank, floating hydrometer specific gravity gauge (1.280 target), cooling coils.
  - Right: Battery cell rejuvenation bay (Desulfation pulse charger, cell voltage meters, lead-plate replacement queue).
  - Bottom: [PULSE DESULFATION CYCLE], [POUR BALANCED ELECTROLYTE], [TEST UNDER 100A LOAD].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 57: UI_PANEL_LONG_WAVE_BROADCAST_PROPAGANDA_TRANSMITTER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Faction Influence, Morale & Radio
• Core Linkage: Ashfall.Core.RadioSystem, SocialSystem
• Wireframe:
  - Left: Transmitter status (500W Long-Wave Final Amplifier), antenna SWR meter, tube anode voltage dial.
  - Center: Broadcast playlist scheduler (Folk songs, shelter news, anti-raider warnings, refugee rally signals).
  - Right: Broadcast transmission coverage radius map, wasteland listener reception reports, regional morale boost delta.
  - Bottom: [COMMENCE LIVE BROADCAST], [TUNE ANTENNA MATCHING NETWORK], [SWITCH TO ENCRYPTED MILITIA CHANNEL].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 58: UI_PANEL_SCAVENGER_HAZARD_SUIT_DECON_SHOWER
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Radiation Safety & Maintenance
• Core Linkage: Ashfall.Core.RadiationSystem, InventorySystem
• Wireframe:
  - Left: Returning scavenger gear rack (Heavy Boots, Rubberized Hazmat Overalls, Respirator Cartridges).
  - Center: Washdown stall schematics with high-pressure chelate detergent nozzles, ultrasonic scrubbers, hot water heater.
  - Right: Geiger wand surface scan readout (Target: <5 uSv/hr clean threshold), contaminated wash runoff filter.
  - Bottom: [RUN HIGH-PRESSURE DETERGENT WASH], [EXECUTE GEIGER WAND SWEEP], [RELEASE DECONTAMINATED SUIT].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 59: UI_PANEL_SOIL_MICROBIOME_BIOCHAR_RETORT
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Agriculture, Soil Restoration & Greenhouse
• Core Linkage: Ashfall.Core.FoodProductionSystem, YearOfAsh
• Wireframe:
  - Left: Organic waste & biomass input feed (Dead crop stalks, wood scrap, dry bones), anaerobic moisture sensor.
  - Center: Pyrolysis retort kiln chamber (450°C oxygen-starved burn), biochar porosity inspection microscope, gas flare.
  - Right: Soil bacterial inoculant mixing vat (Mycorrhizal fungi spores, compost tea brew, activated charcoal porous bed).
  - Bottom: [DISCHARGE ACTIVATED BIOCHAR], [INOCULATE WITH BENEFICIAL MICROBIOME], [AMEND GREENHOUSE TOPSOIL].

───────────────────────────────────────────────────────────────────────────────────────────
PANEL 60: UI_PANEL_GRAND_VICTORY_EPILOGUE_COSMIC_DAWN
───────────────────────────────────────────────────────────────────────────────────────────
• Domain: Endgame Evaluation, Narrative & Epilogue
• Core Linkage: Ashfall.Core.EndgameSystem, SaveSystem
• Wireframe:
  - Left: Grand Survival Ledger summary (Total Days Survived, Fallen Survivors Remembered, Clean Water Generated, Century Seed Mature).
  - Center: Panoramic cinematic viewport of the first blue sky breaking through the nuclear winter clouds above the bunker hatch.
  - Right: Faction legacies & world historical verdict (The fate of humanity, survivor descendants' chronicles, lasting monuments).
  - Bottom: [BEGIN NEW GENERATION], [EXPORT VICTORY RECORD], [RETURN TO MAIN MENU].
```

---

## 3. Complete 60-Panel System Reference Matrix

```
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                           ASHFALL 60-PANEL UI MASTER MATRIX                             │
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
└────┴────────────────────────────────────────────┴───────────────────────────────────────┘
```
