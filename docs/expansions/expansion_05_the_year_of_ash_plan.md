# ASHFALL — Expansion Design Bible & Godot-Native Integration Plan

**Title:** ASHFALL: THE YEAR OF ASH (THE LONG WINTER & THE FINAL RECKONING)  
**Internal id:** `expansion_05_the_year_of_ash`  
**Timeline Scope:** Day 180 to Day 360 (The Full Nuclear Year Cycle)  
**Target Engine:** Godot 4.7+ (.NET/C#) Host + `Ashfall.Core` Engine-Agnostic Simulation  
**Status:** Complete Design Bible & Master Architectural Specification  
**Tone Lock:** Cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.

---

# I. EXECUTIVE SUMMARY & EXPANSION VISION

The first 180 days of *ASHFALL* test baseline biological survival: sealing the blast hatch, rationing iodine, clearing radioactive fallout filters, establishing hydroponics, and negotiating with regional scavengers and early military patrols.

**`expansion_05_the_year_of_ash`** bridges Day 180 to Day 360 — the phase where physical survival collides with psychological exhaustion, societal decay, environmental transformation, and the final geopolitical resolution of the war. 

```
┌─────────────────────────────────────────────────────────────────────────────────────────────────────────┐
│                                ASHFALL: 360-DAY NUCLEAR WAR TIMELINE                                   │
├───────────────────────────────┬─────────────────────────────────┬───────────────────────────────────────┤
│ DAYS 1 – 60 (Holdfast)        │ DAYS 61 – 180 (Duty/Charter)    │ DAYS 181 – 360 (The Year of Ash)      │
│ • Initial Blast & Thermal Rad │ • Internal Roster & Duties      │ • Phase IV: Deep Freeze (-35°C) (180) │
│ • Allocation Schedule 12      │ • The Crossing & Regional Trade │ • Phase V: Total Faction Siege (240)  │
│ • Basic Bunker Infrastructure │ • Voucher & Backer System       │ • Phase VI: The Great Thaw (300-360)  │
└───────────────────────────────┴─────────────────────────────────┴───────────────────────────────────────┘
```

### Key Pillars
1. **The 180–360 Day Environmental Arc**: Transition from volcanic nuclear winter (Phase IV: Days 180–240, sub-zero -35°C cold, geothermal freeze, frozen pipes) to the total faction proxy war (Phase V: Days 240–300, artillery sieges, Continuity Reclamation decrees) to the radioactive toxic thaw (Phase VI: Days 300–360, black mud floods, radon gas geysers, final emergency broadcast frequencies).
2. **Survivor-Reactive Shelter Door Encounters**: 40 distinct encounters at the bunker hatch that evaluate the exact roster of living survivors inside the shelter — measuring their moral alignment, guilt levels, medical afflictions (respiratory degradation, chemical dependency, radiation stage), trauma bonds, and trade occupations to determine reaction branches, mutiny thresholds, and psychological fallout.
3. **Major Faction War Climax**: The four end-stage participants — Iron Garrison, The Rebuilders, Black Ops (D/9), and the Ash Sign Cult — enter end-stage ideological conflicts. Neutrality becomes untenable; bunker allegiance or defiance forces active territorial consequences. **No fifth territorial power is introduced.** `docs/lore/00_OVERVIEW.md` closes Sector 4's map at four Powers (Iron Garrison, Ash Militia, Cult of the Ash Sign, Warlords of Sector 4) plus two non-territorial code-only factions (Rebuilders, Black Ops). The extractive utility cartels (Coastal Hydro-Barons) operate as an unaligned commercial monopoly.
4. **Subterranean Environmental Hazards**: Dedicated simulation models for Radon-222 gas infiltration (`YearOfAshRadonSystem`) and Deep Freeze thermal balances (`YearOfAshDeepFreezeSystem`).
5. **Godot-Native Architecture**: Zero gameplay logic in engine nodes. Simulation systems live exclusively in `Ashfall.Core` (plain C#, zero `UnityEngine`/`Godot` namespaces). Godot 4.7+ hosts the presentation using lightweight `Control`, `Container`, and `RichTextLabel` nodes wired through an event bus.

---

# II. THE 180–360 DAY TIMELINE & ENVIRONMENTAL PHASES

```mermaid
timeline
    title The Nuclear Year Timeline (Days 180 - 360)
    section Phase IV: The Deep Freeze (Days 180-240)
        Day 180 : Stratospheric ash layer peaks (-38°C)
        Day 195 : Geothermal well freezing & pipe brittle fracture
        Day 210 : The Black Blizzard (Zero visibility, extreme fallout)
        Day 225 : Frostbite crisis & metabolic exhaustion
    section Phase V: Faction Siege & The Reckoning (Days 240-300)
        Day 240 : Continuity Reclamation Decree issued
        Day 255 : Iron Garrison heavy artillery sweeps the Verge
        Day 270 : Ash Sign Cult suicidal bunker sieges
        Day 285 : Black Ops terminal protocol activation
    section Phase VI: The Great Thaw & Broadcasts (Days 300-360)
        Day 300 : Black Mud runoff & radon gas infiltration
        Day 320 : Emergency Global Broadcasts on long-wave radio
        Day 340 : Final Continental Evacuation / Integration Gate
        Day 360 : The Year of Ash Conclusion & Epilogue Assessment
```

### Phase IV: The Deep Freeze (Days 180–240)
- **Atmospheric Conditions**: Solar radiation reduced by 85%. Ambient surface temperature plunges to -25°C to -45°C.
- **Bunker Hazard — Thermal Stress & Ice Loading**: Heavy frozen ash accumulates over the intake chimneys and blast door frame. If heating systems drop below tier 2, internal condensation freezes, cracking electrical conduits and causing water pipe burst events.
- **Survivor Impact**: Daily caloric demand increases by +40%. Survivors without thermal clothing (`item_insulated_parka`, `item_thermal_lining`) gain the `frostbite_stage_1` to `frostbite_stage_3` status, increasing medical dependency.
- **Simulation System**: `YearOfAshDeepFreezeSystem.cs` models heat loss equations:
  $$\Delta T_{indoor} = (\text{GeothermalFlow} \times 0.26) - ((20.0 - T_{surface}) \times (1.0 - \text{InsulationQuality} \times 0.70))$$
  Intake icing accumulates when surface temperature falls below -15°C, reaching critical blockage at 50mm thickness.

### Phase V: The Faction Siege & Total War (Days 240–300)
- **Geopolitical State**: Resource depletion pushes the surviving Powers to abandon diplomacy. The Iron Garrison declares Martial Allocation Authority; the Rebuilders fortify agricultural green-zones; the Black Ops Syndicate hunts bunker data cores; the Ash Sign Cult conducts mass suicidal assaults against sealed shelters.
- **Bunker Hazard — Surface Crossfire & Extortion**: Random daily events feature stray mortar concussions, sniper surveillance on exterior periscopes, and cut utility cables.
- **Survivor Impact**: High paranoia, ideological friction between faction-sympathetic survivors, and ration theft.
- **Simulation System**: `FactionWarSystem.cs` simulates daily territorial shifts and tension accretion. At tension >= 75%, civilian trade caravans are disrupted and artillery strikes increase.

### Phase VI: The Great Thaw & The Final Broadcasts (Days 300–360)
- **Atmospheric Conditions**: Seasonal shifts create high-altitude atmospheric inversions. Temperatures rise above 0°C, melting 10 months of radioactive snow into toxic "Black Mud" slurries.
- **Bunker Hazard — Drainage Inundation & Radon Gas**: Sump pumps run constantly. Radon gas seeps through foundation micro-cracks requiring active charcoal air scrubber replacements (`item_air_filter_heavy`).
- **Simulation System**: `YearOfAshRadonSystem.cs` tracks Radon-222 Bq/m³ accumulation:
  $$\text{Radon}_{inflow} = (120.0 + \text{Fissures} \times 280.0) \times (1.0 - \text{ScrubberHealth} \times 0.70)$$
  Concentrations exceeding 800 Bq/m³ trigger severe respiratory trauma and require filter replacement and foundation sealing.
- **Endgame Resolution**: Automated long-range continuity beacons activate across 142.850 MHz. Five distinct historical conclusion paths trigger based on player choices throughout the 360 days.

---

# III. SHELTER DOOR ENCOUNTERS & REACTIVE MORALE MATRIX

Unlike generic random events, **Door Encounters in Phase IV–VI evaluate the internal state and interpersonal relationships of all living survivors in the bunker**.

```
                           ┌────────────────────────────────────────┐
                           │      SURVIVOR ENCOUNTER EVALUATOR      │
                           └───────────────────┬────────────────────┘
                                               │
               ┌───────────────────────────────┼───────────────────────────────┐
               ▼                               ▼                               ▼
     [Medical & Needs State]         [Interpersonal Web]            [Moral Alignment & Traits]
     • Respiratory Sickness          • Active Trauma Bonds          • Ruthless Pragmatist vs
     • Radiation Phase (0-3)         • Grudges & Backstory Ties       Communal Humanist
     • Chemical Dependencies         • Guilt & Insomnia Levels      • Survivor Specializations
               │                               │                               │
               └───────────────────────────────┼───────────────────────────────┘
                                               ▼
                               ┌─────────────────────────────────┐
                               │  DYNAMIC CHOICE MODIFIERS &     │
                               │  SURVIVOR REACTION PREVIEWS     │
                               └─────────────────────────────────┘
```

## 1. Encounter Catalog (40 Authoritative Entries)
The master catalog [`door_encounters.json`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/StreamingAssets/Data/door_encounters.json) contains 40 distinct encounters spanning:
- **Phase IV Deep Freeze (#1–#15)**: Frozen couriers, frostbitten deserter families, wandering trauma surgeons, seized filter traders, and geothermal pipe repairmen.
- **Phase V Faction Siege (#16–#28)**: Garrison census auditors, Rebuilder seed couriers, D/9 booby-trap scouts, Cult penitent pyre disciples, and Hydro-Baron meter wardens.
- **Phase VI The Great Thaw (#29–#40)**: Black mud refugees, radon scrubber hawkers, salt chamber isotope physicists, crystal radio technicians, and Continental Maritime evacuation envoys.

## 2. Survivor Response Modifiers
Every decision calculates delta updates across four core emotional indices:
1. **Moral Alignment Delta**: Humanist choices increase commune bonding; Ruthless choices prevent resource depletion at the cost of long-term depression.
2. **Guilt Score**: Betraying or turning away vulnerable refugees increases survivor guilt, manifesting in insomnia, work inefficiency, and suicidal ideation.
3. **Trauma Bond Resonances**: If a survivor with a trauma bond to the shelter leader is present, harsh choices cause loyalty fractures.
4. **Faction Affinity**: Faction members sheltered inside advocate for their respective groups; defying their faction lowers internal security.

---

# IV. COMPLETE DATA CATALOG BLUEPRINTS

All game data is defined in authoritative JSON catalogs placed in `Assets/StreamingAssets/Data/` and mirrored in `res://Data/`:

```
Assets/StreamingAssets/Data/
├── door_encounters.json       (40 entries: Dynamic door visitors & choice matrices)
├── year_of_ash_items.json     (36 entries: High-tier gear, filters, isotopes, ordnance)
├── year_of_ash_events.json    (36 entries: Phase IV-VI environmental & faction crises)
├── year_of_ash_locations.json (30 entries: Sector 4/8 exploration & recovery nodes)
├── year_of_ash_radio.json     (18 entries: Emergency frequencies & teletype logs)
├── year_of_ash_survivors.json (24 entries: Late-game candidate rosters & RUR scores)
└── year_of_ash_quests.json    (12 entries: Multi-stage branching questline graphs)
```

### 1. Items Schema & Catalog Overview (`year_of_ash_items.json`)
The catalog provides 36 late-game items across 5 distinct tiers:
- **Survival Gear**: `item_military_filter_crate`, `item_air_filter_heavy`, `item_perennial_wheat_strain_7`, `item_continuity_ration_biscuit_tin`.
- **Thermal & Protective**: `item_insulated_parka`, `item_heavy_rubberized_pressure_suit`, `item_winter_cleats_crampons`, `item_hermetic_hatch_silicone_gasket`.
- **Engineering & Crafting**: `item_thermal_lining`, `item_brass_valve_fitting`, `item_boron_shielding_tile`, `item_glycol_antifreeze_canister`, `item_ceramic_heating_element`, `item_high_tensile_steel_culvert_brace`, `item_corrosion_inhibitor_drum`.
- **Tools & Scientific**: `item_quartz_crystal_resonator`, `item_radon_detector_electret`, `item_geiger_counter_pancake_probe`, `item_lead_shielded_sample_cask`, `item_sealed_lead_pig`, `item_liquid_nitrogen_dewar`, `item_denial_detonator_spool`, `item_plastic_explosive_block`, `item_strontium_90_thermoelectric_pellet`, `item_surgical_bone_chisel`, `item_insulated_snowmobile_battery`, `item_icebreaker_rendezvous_flare_rocket`.
- **Medical Reagents**: `item_sski_iodine_bulk_canister`, `item_prussian_blue_chelating_pellets`, `item_antibiotic_saline_infusion`.
- **Quest & Continuity**: `item_falsified_clearance`, `item_cryo_flask_rhizome`, `item_continental_maritime_transponder`, `item_calibrated_mass_spectrometer_tube`, `item_one_time_cipher_pad_d9`, `item_evacuation_manifest_scroll`.

### 2. Events Schema & Catalog Overview (`year_of_ash_events.json`)
Contains 36 daily crisis events covering:
- **Phase IV**: Stratospheric black blizzards, conduit brittle fractures, sentry frostbite collapses, allotment cold-frame failures, diesel fuel cloud gelling, exhaust flue rime chokes, periscope hoarfrost blindness, lead-acid electrolyte slush freezing, hydraulic blast seal freezing, sub-zero stasis frustration.
- **Phase V**: Garrison registration sweeps, howitzer shrapnel concussions, Ash Sign penitent sieges, Hydro-Baron pipeline throttling, D/9 culvert detonations, Continuity Decree promulgations, Warlord toll ambushes, Allotment brass embargoes, shrapnel intake perforations, garrison firing squads.
- **Phase VI**: Black mud thaw inundations, radon fissure ventings, 142.850 MHz carrier signals, sump pump overruns, humid spore outbreaks, thawing snowpack corpse discoveries, aquifer salt shocks, coastal pack ice calving booms, Aurora Borealis manifest calls, mass spectrometer warhead revelations, permafrost subsidence shifts, geothermal meltwater flashing, first green sprout sightings, D/9 carrier stand-downs, garrison headquarters abandonments, and the Day 360 final dawn.

### 3. Locations Schema & Catalog Overview (`year_of_ash_locations.json`)
Features 30 exploration nodes spanning Sector 4, Sector 8, and the Northern Coast:
- `loc_the_allotments` (The Works Allotment Commune)
- `loc_denial_cut_substation` (D/9 Denial Substation & Railway Cut)
- `loc_brine_pumping_sluice` (Sector 8 Industrial Brine Sluice)
- `loc_continental_radio_beacon` (High Granite Relay Array)
- `loc_low_background_lab` (Low-Background Salt Chamber Laboratory)
- `loc_geothermal_well_alpha` (Geothermal Steam Well Alpha)
- `loc_garrison_checkpoint_gamma` (Garrison Checkpoint Gamma)
- `loc_black_thaw_drainage_basin` (Black Thaw Radioactive Drainage Swale)
- `loc_maritime_icebreaker_dock` (Northern Sound Icebreaker Dock)
- `loc_rhizome_research_vault` (Agronomy Cryogenic Rhizome Vault)
- `loc_ash_sign_cathedral_crater` (Cathedral Vitrified Strike Crater)
- `loc_sector_4_rail_switchyard` (Sector 4 Freight Switchyard)
- `loc_hydro_baron_aqueduct_manifold` (Aqueduct Pressure Station 3)
- `loc_granite_pass_weather_observatory` (Granite Pass Meteorological Dome)
- `loc_d9_cache_bunker_delta` (D/9 Denial Armory Cache)
- `loc_flooded_quarry_cistern` (Limestone Quarry Cistern)
- `loc_sub_level_maintenance_shaft_9` (Sub-Level Maintenance Shaft 9)
- `loc_garrison_motor_pool` (Garrison Armored Motor Pool)
- `loc_rebuilder_brickworks_kiln` (The Works Brickworks Kiln)
- `loc_continental_convoy_staging_area` (Highway 12 Convoy Apron)
- `loc_salt_cavern_medical_depot` (Salt Cavern Emergency Reserve)
- `loc_collapsed_valley_viaduct` (The Shattered Viaduct)
- `loc_hydro_baron_desal_plant_4` (Coastal Desalination Unit 4)
- `loc_mountain_tunnel_refuge` (High Alpine Highway Tunnel)
- `loc_radioisotope_power_station` (Strontium RTG Lighthouse Tower)
- `loc_frozen_river_ferry_crossing` (The Ice Road Ferry Crossing)
- `loc_garrison_signal_bunker_echo` (Garrison Cryptographic Signal Bunker)
- `loc_d9_culvert_junction_bravo` (D/9 Culvert Junction Bravo)
- `loc_allotment_glasshouse_complex` (Allotment Polycarbonate Glasshouses)
- `loc_aurora_borealis_grounding_shoal` (Aurora Borealis Anchorage Shoal)

### 4. Emergency Radio & Carrier Waves (`year_of_ash_radio.json`)
18 long-wave broadcasts across 142.850 MHz, 88.400 MHz, 104.200 MHz, and 96.100 MHz.

### 5. Survivor Dossiers (`year_of_ash_survivors.json`)
24 deep-lore candidates with unique background stories, RUR scores, moral traits, and starting stats.

### 6. Dynamic Branching Questlines (`year_of_ash_quests.json`)
12 multi-stage directed graph questlines:
1. `quest_garrison_blood_debt` (The Garrison Blood Debt)
2. `quest_rebuilder_seed_vault` (The Living Seed Vault)
3. `quest_continental_convoy_gate` (The Northern Icebreaker Gate)
4. `quest_ash_sign_pyre_apostasy` (The Penitent's Shroud)
5. `quest_hydro_baron_aqueduct_sabotage` (The Brine Monopoly)
6. `quest_d9_null_stand_down` (Protocol Null Stand-Down)
7. `quest_deep_freeze_heating_crisis` (The Freezing Core)
8. `quest_low_background_provenance` (The Cold Count)
9. `quest_allotment_brass_treaty` (The Valve Seat Concord)
10. `quest_black_thaw_drainage_rescue` (Silt in the Sumps)
11. `quest_radio_142_carrier_lock` (The Day 340 Signal)
12. `quest_final_manifest_muster` (The Aurora Departure)

---

# V. FIVE ENDGAME EPILOGUES (DAY 360 RESOLUTION)

On Day 360, the simulation evaluates total historical decisions, faction standings, survivor survival counts, and technological milestones to render one of five definitive epilogues:

```
                                  [DAY 360 EVALUATION]
                                            │
        ┌───────────────────┬───────────────┼───────────────┬───────────────────┐
        ▼                   ▼               ▼               ▼                   ▼
 [Northern Redoubt]   [Agrarian Concord] [Open Ledger] [Deep Holdfast]  [Measured Truth]
  Convoy Evacuation   The Works Commune   Free Trade    Autonomous      Isotopic Proof
  & Arctic Crossing   & Seed Vault        Network       Isolation       & Demilitarization
```

### 1. The Northern Redoubt (Maritime Evacuation)
- **Requirements**: `quest_continental_convoy_gate` completed, `item_continental_maritime_transponder` tuned, >= 15 survivors alive, `item_evacuation_manifest_scroll` signed.
- **Ending Prose**: The heavy diesels of the *Aurora Borealis* shake the pack ice. Your survivors climb the gangway wrapped in blankets, carrying logbooks and seed flasks. As the vessel breaks north into the open lead, the grey coast of Sector 4 recedes into the mist. You leave behind an empty concrete bunker and take with you thirty human beings who survived the long winter.

### 2. The Agrarian Concord (The Works Dominion)
- **Requirements**: `faction_rebuilders` standing >= 60, `quest_rebuilder_seed_vault` completed, `quest_allotment_brass_treaty` signed, >= 10 brass valve fittings delivered.
- **Ending Prose**: The Allotments stretch across three hundred acres of reclaimed floodplain. Geothermal steam pipes wrap the seedling glasshouses in warmth. Ottilie Frayne marks the Day 360 entry in the municipal ledger: zero deaths from starvation in the spring quarter. The committee votes by show of hands, and your shelter holds permanent seat Number One.

### 3. The Open Ledger (Commercial Federation)
- **Requirements**: `faction_hydro_barons` standing >= 40, `faction_central_garrison` standing >= 20, debt ledger cleared, trade routes secured.
- **Ending Prose**: The trade road across the viaduct remains open. Convoys of tanker trucks and grain carts move under neutral arbitration flags. The scale at Stallrow never stops swinging, and every gallon of potable water is accounted for in ink. It is not paradise, but it is an economy where people trade with calibration weights instead of rifles.

### 4. The Deep Holdfast (Autonomous Isolation)
- **Requirements**: Neutral or hostile to all external factions, shelter autonomy level >= 90%, radon scrubbers and geothermal loops operational, zero survivors surrendered.
- **Ending Prose**: The blast hatch remains dog-bolted from the inside. Outside, the warlords exhausted their artillery and dissolved into the spring mud. Inside, the hydroponic trays grow green under LED banks, the air scrubber hums quietly, and thirty people wake in clean warmth. You answered to no master, paid no tithes, and survived on your own steel.

### 5. The Measured Truth (The Cold Count)
- **Requirements**: `quest_low_background_provenance` completed, `item_calibrated_mass_spectrometer_tube` recovered, global broadcast transmitted on 142.850 MHz.
- **Ending Prose**: The isotopic data from the salt chamber was broadcast in plaintext across twelve frequencies. When the regional commanders learned that the war was initiated by an uncorrected automated sensor error in an unmanned orbital silo, the ideological fire went out of the armies. Conscripts threw down their rifles and walked home. The war ended not with a treaty, but with a measurement.

---

# VI. GODOT 4.7+ PRESENTATION ARCHITECTURE

```
Godot Host (Presentation & UI)
├── Main.cs                            (Host window & master loop)
├── src/YearOfAsh/
│   ├── YearOfAshHostSession.cs        (Coordinator wiring all simulation systems)
│   ├── DoorEncounterModal.cs          (Modal dialog for 40 hatch visitors)
│   ├── FactionWarMapWidget.cs         (Territorial control & tension meter)
│   ├── RadonVentilationWidget.cs      (Radon-222 Bq/m³ bar & filter controls)
│   ├── GeothermalHeatingWidget.cs     (Thermal balance & intake de-icing)
│   ├── RadioBroadcastTerminal.cs      (Emergency frequency receiver)
│   └── YearOfAshSaveStore.cs          (JSON persistence store)
└── Core Simulation (Ashfall.Core - Plain C#)
    ├── YearOfAsh/
    │   ├── YearOfAshTimelineSystem.cs (Phase transitions & environment)
    │   ├── YearOfAshRadonSystem.cs    (Thaw radon infiltration)
    │   ├── YearOfAshDeepFreezeSystem.cs (Sub-zero heat loss & icing)
    │   ├── DoorEncounterSystem.cs     (Survivor evaluation & moral choices)
    │   ├── FactionWarSystem.cs        (Territory & tension simulation)
    │   ├── QuestlineSystem.cs         (12 branching quest graphs)
    │   └── YearOfAshCatalogLoader.cs  (Ports deserialization loader)
    └── YearOfAshSave.cs               (Cross-host serializable save state)
```

---

# VII. VERIFICATION PROTOCOL

Every deliverable must satisfy dual-engine determinism and compilation gates:
1. `dotnet test Ashfall.Core.Tests` — All 247+ tests must pass without engine namespaces.
2. `dotnet build Ashfall.csproj` — Godot host project must compile with 0 errors and 0 warnings.
3. Save/Load parity — Saves written by Godot must load identically in Unity batch test harnesses.
