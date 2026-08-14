# ASHFALL: THE YEAR OF ASH (DAYS 180–360) — Grand Lore Bible & Master Creative Pack

**Internal id:** `expansion_05_the_year_of_ash`  
**Kind:** Shippable prose & definitive narrative resolution. Companion to `docs/expansions/expansion_05_the_year_of_ash_plan.md` and `docs/lore/06_REBUILDERS_AND_BLACK_OPS.md`.  
**Voice lock:** Cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.  
**VO:** Lines marked `[VO]` are text-first; record only if the radio/tannoy pipeline exists. Everything else is UI/Codex/inspect.  

---

# I. THE GREAT SCHISM OF SECTOR 4: 10 FACTIONS AT WAR

When the stratospheric ash layer reached peak opacity on Day 180, ambient temperatures plunged to -38°C. With agriculture impossible and fuel reserves freezing into cloudy wax, the pre-war social compact evaporated. Sector 4 shattered along ideological lines into two warring coalitions: the **Directorate & Military Bloc** and the **Rebel, Communal & Autonomy Bloc**.

```
┌──────────────────────────────────────────────────────────────────────────────────────────────────────────┐
│                                    THE 10 FACTIONS OF THE YEAR OF ASH                                    │
├─────────────────────────────────────┬────────────────────────────────────────────────────────────────────┤
│ DIRECTORATE & MILITARY BLOC         │ REBEL, COMMUNAL & AUTONOMY BLOC                                    │
├─────────────────────────────────────┼────────────────────────────────────────────────────────────────────┤
│ 1. The Iron Garrison (3rd Corps)    │ 6. The Works (Public Works Allotment Committee)                    │
│ 2. Detachment 9 (STD-9 / Protocol)  │ 7. The Ash Militia (Central Upland Defense League)                 │
│ 3. Continental Logistics Escort     │ 8. Penitent Cult of the Ash Sign (Vitrified Martyrs)               │
│ 4. 8th Penal Pioneer Sump Regiment  │ 9. Shattered Rail Union & Switchmen Guild                          │
│ 5. High Granite Munitions Foundry   │ 10. Deep Salt Cavern Freeholders & Miner Cooperative               │
└─────────────────────────────────────┴────────────────────────────────────────────────────────────────────┘
```

---

## 1. The Directorate & Military Bloc (5 Factions)

### 1. The Iron Garrison (3rd Corps Provisional Directorate)
- **Designation**: `faction_central_garrison`
- **Commander**: Major Kroll (Provost Marshal) / General Vance (Deceased)
- **Headquarters**: Checkpoint Gamma & Kilometre 12 Redoubt
- **History & Doctrine**: The remnants of the regional army corps that held the strategic rail corridor. Governs through martial decrees, logistics schedules, and armed conscription. Under **Martial Law Schedule 14**, the Garrison claims absolute ownership of all stored calories, diesel fuel, and machine tools within five kilometres of the rail corridor. They view civilian shelters as non-compliant supply depots whose reserves must be nationalized to support the defensive perimeter.
- **Weaponry**: 152mm heavy towed howitzers, armored half-tracks, 7.62mm service rifles.
- **Atmospheric Voice**: Cold, bureaucratic, unyielding. Rubber-stamped requisition forms, squealing half-track sprockets, steam whistles, and distant howitzer concussions.

### 2. Detachment 9 (Special Technical Directorate / Protocol Null)
- **Designation**: `faction_black_ops`
- **Commander**: Colonel Brand
- **Headquarters**: Kilometre 44 Railway Cut Telecom Bunker
- **History & Doctrine**: Eighteen surviving specialists of the Ministry of Supply's Special Technical Directorate (STD-9). Their standing pre-war tasking was simple: **deny all strategic infrastructure to enemy forces in the event of territorial collapse**. Operating under automated telecommunication orders signed by a dead minister, D/9 has wired every bridge, viaduct, and tunnel in Sector 4 with linear Comp-B demolition blocks. They refuse to stand down without a cryptographic override code (`Lima-November-74`) that will never arrive from the vaporized capital.
- **Weaponry**: Electric bridge-wire blasting caps, Comp-B demolition blocks, sniper rifles, one-time cipher pads.
- **Atmospheric Voice**: Paranoic, methodical, exhausted. Clicking telegraph sounders, five-digit cipher books, and perforated paper tape curling across frozen concrete floors.

### 3. Continental Logistics Escort (Highway 12 Convoy Corps)
- **Designation**: `faction_supply_corps`
- **Commander**: Captain Alder
- **Headquarters**: Highway 12 Staging Apron & Weigh Station
- **History & Doctrine**: The armored freight detachment tasked with moving heavy supplies along the northern arterial highway. Operating tracked halftracks with heated battery packs, they escort bulk medical supplies, seed stocks, and diesel fuel between northern deep vaults and frontline garrisons. They maintain military neutrality toward compliant civilian shelters, trading security for heated garage berths.
- **Weaponry**: Armored troop carriers with pintle-mounted heavy machine guns, 24V starter carts, tow cables.
- **Atmospheric Voice**: Grinding diesel transmissions, tire chains rattling on hard-packed ice, and hot engine oil fumes.

### 4. The 8th Penal Pioneer Regiment (Iron Sump Engineers)
- **Designation**: `faction_penal_battalion`
- **Commander**: Sapper Vance (Mutineer Representative) / Provost Guards
- **Headquarters**: Sump Mud Trenches (Ground Zero Perimeter)
- **History & Doctrine**: Formed from military convicts, draft resisters, and civilian detainees sentenced to hazardous labor. Forced to dig defensive trenches and clear radioactive unexploded ordnance with hand shovels. When radiation sickness claimed 60% of their ranks, the sappers mutinied, killed their provost officers, and fortified the sump mud drifts.
- **Weaponry**: Entrenching shovels, improvised unexploded ordnance booby traps, burlap respirators.
- **Atmospheric Voice**: Splashing black mud, wet coughing, rusted iron shackles, and muffled explosions in the fog.

### 5. High Granite Munitions & Arsenal Directorate
- **Designation**: `faction_ordnance_foundry`
- **Commander**: Chief Assayer Markov
- **Headquarters**: Subterranean Granite Foundry Dome
- **History & Doctrine**: The industrial foundry carved inside a solid granite mountain. Holds the sector's only working drop hammers and brass stamping dies. Forges artillery shell casings and cooks ammonium nitrate charges for the Garrison in exchange for food rations. Holds an iron monopoly on ammunition reloading.
- **Weaponry**: Hardened tool-steel dies, hydraulic cartridge presses, charcoal crucibles.
- **Atmospheric Voice**: Rhythmic drop-hammer thuds shaking the bedrock, searing sulfur fumes, and glowing orange bronze slag.

---

## 2. The Rebel, Communal & Autonomy Bloc (5 Factions)

### 6. The Works (Public Works Allotment Committee)
- **Designation**: `faction_rebuilders`
- **Leader**: Ottilie Frayne
- **Headquarters**: The Allotments (River Floodplain)
- **History & Doctrine**: Founded by municipal public works employees who settled the river floodplain—the only viable agricultural loam in Sector 4. They operate five acres of double-glazed polycarbonate cold-frames heated by composting manure and steam pipes. Using **Strain-7 cold-hardened rye rhizomes**, they produce bread for four hundred people while fighting off military grain requisitions.
- **Weaponry**: 12-gauge hunting shotguns, iron pry-bars, steam autoclaves, wood-fired brick kilns.
- **Atmospheric Voice**: Steaming compost heaps, dripping glass panes, boiling copper stills, and calm communal committee debates.

### 7. The Ash Militia (Central Upland Defense League)
- **Designation**: `faction_ash_militia`
- **Leader**: Commander Talia
- **Headquarters**: Mountain Terrace Redoubts & Switchback 4
- **History & Doctrine**: A mutual-defense league of upland farmsteads and switchback lookouts. They defend their terraced potato fields and root cellars against Garrison foraging sweeps using deadfall timber barricades and high-angle sniper ambushes. They advocate for total regional autonomy and the abolishment of martial requisitions.
- **Weaponry**: Scoped hunting rifles, dynamite deadfall traps, ski scout patrols.
- **Atmospheric Voice**: Mountain wind howling through pine deadfalls, sharp rifle cracks echoing off granite bluffs, and quiet lookouts chewing dried turnips.

### 8. The Penitent Cult of the Ash Sign (Vitrified Martyrs)
- **Designation**: `faction_ash_sign`
- **Leader**: Hierophant Malachi / Sister Martha (Apostate)
- **Headquarters**: Cathedral Vitrified Strike Crater (Ground Zero)
- **History & Doctrine**: A theological movement born among radiation burn survivors in the eighty-meter blast crater. They view the nuclear exchange not as a tragedy, but as a divine purification of human corruption. They burn their dead on sulfur pyres and conduct mass suicidal assaults against sealed military bunkers, seeking to 'liberate the dwellers of the false concrete tombs.'
- **Weaponry**: Vitrified radioactive glass daggers, sulfur smoke pots, suicide demolition vests.
- **Atmospheric Voice**: Rhythmic chanting in dense ash fog, chiming brass prayer bells, crackling sulfur bonfires, and Geiger counters screaming at 20 µSv/h.

### 9. The Shattered Rail Union & Switchmen Guild
- **Designation**: `faction_railway_guild`
- **Leader**: Elena Vasquez
- **Headquarters**: Sector 4 Locomotive Roundhouse & Repeater Hut 14
- **History & Doctrine**: The union of civilian railway workers, track linesmen, and telegraphers. They control the armored steam handcars that navigate unsevered track segments. Fiercely opposed to the military occupation of the railway, they conduct covert track sabotage, cut telegraph lines, and smuggle flour from The Works to isolated upland communities.
- **Weaponry**: Hydraulic spike pullers, armored steam handcars, telegraph armature keys.
- **Atmospheric Voice**: Steel wheels squealing on frosted rails, clicking Morse sounders, kerosene lanterns, and greasy track ballast.

### 10. The Deep Salt Cavern Freeholders & Miner Cooperative
- **Designation**: `faction_salt_freeholders`
- **Leader**: Dr. Erik Dahl / Gregor the Miner
- **Headquarters**: 400m Dry Halite Caverns & Low-Background Lab
- **History & Doctrine**: Four hundred miners, orderlies, and physicists living 400 meters underground in dry, sterile salt caverns. Completely shielded from surface cosmic radiation and fallout (0.02 µSv/h), they operate a forty-bed emergency trauma hospital and barter pure mineral salt and mining dynamite under strict, armed neutrality.
- **Weaponry**: Mining dynamite, rock-bolting guns, heavy pneumatic drills.
- **Atmospheric Voice**: Absolute silence broken by the hum of battery inverters, crisp dry air smelling of halite mineral dust, and the rhythmic drip of condensation collectors.

---

# II. REGIONAL EXPLORATION DOSSIERS (60 LOCATIONS)

```
Sector 4 & 8 Strategic Map Overview (60 Authoritative Exploration Nodes)
├── Floodplain Agriculture & Kilns (Locs 01, 10, 19, 29, 36, 43, 44)
├── Industrial Rail & Switchyards (Locs 02, 12, 28, 32, 37, 42, 55)
├── Deep Halite Caverns & Labs (Locs 05, 21, 34, 46, 49, 57)
├── Garrison Redoubts & Batterys (Locs 07, 18, 27, 31, 35, 40, 53, 59)
├── High Granite Ridge & Observatories (Locs 04, 14, 24, 51, 54, 58, 60)
├── Ground Zero Crater & Trenches (Locs 11, 22, 33, 39, 41, 45, 52)
└── Northern Estuary & Anchorage (Locs 03, 08, 09, 13, 16, 20, 23, 25, 26, 30, 38, 47, 48, 50, 56)
```

### Detailed Room Cards & Sensory Inspection Logs (Sample 10 of 60)

#### 1. `loc_the_allotments` — The Works Allotment Commune
- **Coordinates**: Grid 14-Floodplain · **Risk Level**: 1 · **Radiation**: 0.45 µSv/h
- **Sensory Atmosphere**: The humid air smells of steaming horse manure, damp potting loam, and boiling cabbage. Polycarbonate panes rattle in the freezing wind, heavy with frost on the outside and dripping condensation on the inside.
- **Inspect**: *"Three hundred wooden seed flats line the tiered benches, green with the hair-thin shoots of Strain-7 perennial rye. In the center aisle, an iron wood-stove glows cherry red, heating a copper coil that circulates warm water through buried radiator pipes beneath the beds."*
- **Scavenge**: `item_perennial_wheat_strain_7`, `item_hermetic_hatch_silicone_gasket`, `item_corrosion_inhibitor_drum`.

#### 2. `loc_low_background_lab` — Low-Background Salt Chamber Laboratory
- **Coordinates**: Sub-Level -420m (Halite Formation) · **Risk Level**: 1 · **Radiation**: 0.02 µSv/h
- **Sensory Atmosphere**: Perfect, unnatural silence. The air is bone-dry and cool (+12°C), tasting faintly of mineral salt. Zero cosmic ray flutter on the detectors.
- **Inspect**: *"A five-ton counting chamber forged from the armor plate of a pre-1945 battleship sits in the center of the salt vault. Inside, a quartz-window mass spectrometer tube hums quietly on battery power, analyzing Cesium isotopic ratios with micro-volt precision."*
- **Scavenge**: `item_calibrated_mass_spectrometer_tube`, `item_lead_shielded_sample_cask`, `item_sealed_lead_pig`.

#### 3. `loc_ash_sign_cathedral_crater` — Cathedral Vitrified Strike Crater
- **Coordinates**: Ground Zero (Sector 4 Center) · **Risk Level**: 5 · **Radiation**: 18.50 µSv/h
- **Sensory Atmosphere**: Searing ozone, sulfur dioxide smoke, and the deafening howl of wind across a vitreous glass bowl. Your dosimeter screams with continuous high-pitch alarm chirps.
- **Inspect**: *"The eighty-meter impact depression is lined with slick, bottle-green tektite glass formed when the warhead's fireball fused sixty thousand tons of granite and sand. Iron crosses draped with radioactive wool rags ring the rim, smoldering in the sulfur mist."*
- **Scavenge**: `item_tungsten_carbide_drill_bit`, `item_lead_shielded_sample_cask`.

#### 4. `loc_granite_arsenal_foundry` — High Granite Munitions & Arsenal Directorate
- **Coordinates**: Subterranean Granite Vault · **Risk Level**: 4 · **Radiation**: 1.30 µSv/h
- **Sensory Atmosphere**: Searing sulfur fumes, deafening mechanical drop-hammer concussions that rattle the fillings in your teeth, and the blinding orange glare of charcoal-fired bronze crucibles.
- **Inspect**: *"A three-ton pneumatic drop hammer stamps glowing brass slugs into 152mm artillery casing blanks. Workers in split-leather aprons and soot-blackened goggles shovel crushed charcoal into the furnace while an armed Garrison sergeant tallies finished shell cases in an ink ledger."*
- **Scavenge**: `item_brass_stamping_die`, `item_artillery_fuze_wrench`, `item_ammonium_nitrate_sack`.

#### 5. `loc_penal_pioneer_trench_sector` — 8th Penal Pioneer Sump Trenches
- **Coordinates**: Crater Perimeter Swale · **Risk Level**: 5 · **Radiation**: 14.20 µSv/h
- **Sensory Atmosphere**: Stagnant, oily water rising to the knees, the sickly stench of rotting foot-cloths, and wet, gasping coughs echoing through mud-revetted firing bays.
- **Inspect**: *"A labyrinth of zig-zag trenches shored with rusted corrugated iron and shattered railway ties. Conscripts wearing burlap sack respirators chip frozen radioactive silt from drainage sumps with iron mattocks while unexploded 152mm shells lie half-buried in the clay parapets."*
- **Scavenge**: `item_plastic_explosive_block`, `item_denial_detonator_spool`, `item_prussian_blue_chelating_pellets`.

#### 6. `loc_railway_guild_roundhouse` — Shattered Rail Union Roundhouse
- **Coordinates**: Sector 4 Freight Switchyard · **Risk Level**: 3 · **Radiation**: 0.85 µSv/h
- **Sensory Atmosphere**: The heavy smell of cylinder oil, coal smoke, and damp wood ash. Steam hisses softly from the boiler of a modified armored handcar parked on the turntable pit.
- **Inspect**: *"An iron locomotive inspection shed draped with canvas curtains to hide light. On the central turntable sits a handcar fitted with quarter-inch boiler plate armor and an electric searchlight powered by a 24V aircraft generator."*
- **Scavenge**: `item_railroad_hydraulic_spike_puller`, `item_telegraph_sounder_relay`, `item_insulated_snowmobile_battery`.

#### 7. `loc_deep_salt_hospital_sanctuary` — Deep Salt Cavern Surgical Sanctuary
- **Coordinates**: Sub-Level -400m · **Risk Level**: 1 · **Radiation**: 0.01 µSv/h
- **Sensory Atmosphere**: Crisp, antiseptic smell of carbolic acid mixed with dry mineral salt. Rows of iron hospital cots with clean linen sheets stretch into the softly lit cavern.
- **Inspect**: *"Forty cots carved directly into white halite walls. Dr. Chen operates under battery-powered shadowless lamps, amputating frostbitten toes with clean osteotomes while orderly nurses change sterile dressings. The air is completely free of infection and fallout."*
- **Scavenge**: `item_surgical_bone_chisel`, `item_antibiotic_saline_infusion`, `item_cyanide_antidote_kit`.

#### 8. `loc_aurora_borealis_grounding_shoal` — Aurora Borealis Anchorage Shoal
- **Coordinates**: Northern Sound Estuary · **Risk Level**: 2 · **Radiation**: 0.55 µSv/h
- **Sensory Atmosphere**: Dense white sea smoke rising from leads in the pack ice, the deep rhythmic throb of auxiliary diesel generators shaking the shore ice, and the clean, salted air of the polar sea.
- **Inspect**: *"The 6,000-ton steel hull of the icebreaker Aurora Borealis sits locked in five feet of shore-fast pack ice. Her deck searchlights cast twin beams of yellow light across the snow causeway. Armed lookouts in seal-skin coats watch the boarding gangway."*
- **Scavenge**: `item_continental_maritime_transponder`, `item_icebreaker_rendezvous_flare_rocket`, `item_evacuation_manifest_scroll`.

#### 9. `loc_shelled_grain_elevator_ruin` — Bombarded Concrete Grain Silo 4
- **Coordinates**: Floodplain Siding · **Risk Level**: 4 · **Radiation**: 3.10 µSv/h
- **Sensory Atmosphere**: Smoldering, burnt grain smell mixed with pulverized concrete dust and freezing soot. Wind whistles through jagged artillery perforations in the sixty-foot silo walls.
- **Inspect**: *"A shattered concrete tower split open like a cracked egg by a 152mm howitzer shell. Forty tons of charred, moldering wheat spill across the frozen rail tracks, smoking without flame in the snow. Scavengers sift the ash with wire screens for edible grains."*
- **Scavenge**: `item_hermetic_hatch_silicone_gasket`, `item_ammonium_nitrate_sack`.

#### 10. `loc_the_final_dawn_outlook` — The Day 360 Permafrost Vista
- **Coordinates**: High Granite Pinnacle · **Risk Level**: 1 · **Radiation**: 0.40 µSv/h
- **Sensory Atmosphere**: Clean, cold alpine wind carrying the scent of melting snow and wet granite. The sky above is clear, pale blue for the first time in 360 days.
- **Inspect**: *"A flat granite promontory overlooking the entire thirty-mile breadth of Sector 4. Below, the dark ribbon of the thawed river cuts through black mud plains. To the north, the grey waters of the Arctic sound glint in the first true sunrise of spring."*
- **Scavenge**: `item_evacuation_manifest_scroll`.

---

# III. VERBATIM EMERGENCY RADIO TRANSMISSIONS (36 LOGS)

```
Radio Broadcast Spectrum (36 Authoritative Frequency Intercepts)
├── 142.850 MHz Continental Maritime Emergency Channel (Logs 01, 06, 07, 11, 12, 18, 21, 25, 26, 31, 36)
├── 88.400 MHz Iron Garrison Military Command (Logs 02, 08, 09, 17, 19, 23, 29, 33)
├── 96.100 MHz Detachment 9 Protocol Null Telemetry (Logs 04, 15, 27, 35)
├── 101.500 MHz The Works Communitarian Net (Logs 05, 13, 16, 20, 24, 28, 32)
└── 104.200 MHz Vitrified Crater Penitent Broadcast (Logs 03, 10, 14, 22, 30, 34)
```

### Complete Transcripts for Sample 6 Intercepts

#### Intercept 01: `radio_142_carrier_discovery` (142.850 MHz)
```
[142.850 MHz — SIGNAL: S7 — AM TELETYPE DECODER]
CZC ZCZC 014 340 0014
FROM: CONTINENTAL MARITIME RESCUE COORDINATION (ICEBREAKER AURORA BOREALIS)
TO: ALL SUBTERRANEAN SHELTERS SECTOR 4 AND 8
BT
MARITIME EVACUATION CORRIDOR REMAINS OPEN AT NORTHERN SOUND ANCHORAGE
COORDINATES 68-14-N 014-22-E. 
VESSEL MAINTAINS AUXILIARY STEAM. DEPARTURE LOCKED DAY 360 AT FIRST LIGHT.
MANIFEST RESTRICTED TO CERTIFIED LOW-RAD RESIDENTS AND REGISTERED SEED CUSTODIANS.
BRING LOGBOOKS, CALIBRATION DATA, AND ZERO UNCHECKED ORDNANCE.
IF YOU HEAR THIS, ACKNOWLEDGE ON 142.850 WITH SHELTER SERIAL AND HEADCOUNT.
BT
NNNN
```

#### Intercept 02: `radio_garrison_martial_edict` (88.400 MHz)
```
[88.400 MHz — SIGNAL: S9 — LIVE VOICE RECORDED LOOP]
"This is the Iron Garrison Logistics Directorate, Sector 4 Command.
By order of the Military Governor, all subterranean shelters within five kilometres of
the rail line are placed under Martial Allocation Schedule 14.
Turn out all excess fuel reserves, stored grain exceeding twelve kilograms per occupant,
and any functioning machine-tool components at Checkpoint Gamma by Tuesday morning.
Shelters failing to comply will be classified as non-compliant supply caches.
We are not negotiating. We are feeding the men who hold the perimeter.
End of transmission."
```

#### Intercept 03: `radio_cult_ash_sign_liturgy` (104.200 MHz)
```
[104.200 MHz — SIGNAL: S4 — CARRIER FLUTTER — LIVE HYMN]
"Do not scrape the soot from your windowpanes, brothers. The ash is the veil of His mercy.
The ice came because the world was unclean and required a cold sheet.
On the three-hundredth day, the water will run black from the hills.
Drink of the black water and be sealed.
Those who hide in concrete with brass locks are already dead; their tombs are only warm.
The fire cleared the slate; the frost will freeze it; the thaw will wash the slate clean.
Praise the flash. Praise the silent crater."
```

#### Intercept 04: `radio_d9_protocol_null_carrier` (96.100 MHz)
```
[96.100 MHz — SIGNAL: S8 — AUDIO FREQUENCY SHIFT KEYING TELEMETRY]
"STATION NULL. STATION NULL.
MESSAGE 088 / AUTH CODE: LIMA-NOVEMBER-SEVEN-FOUR.
TARGET GRID 44-ALPHA RAILWAY BRIDGE REMAINS PRIMED.
STAND-DOWN TIMERS WILL NOT RESET WITHOUT SECTOR GENERAL ORDER 1.
ANY ATTEMPT BY CIVILIAN OR MILITIA UNITS TO TRANSIT THE CUT WILL TRIGGER CHARGES.
THIS IS AN AUTOMATED SYSTEM. NO OPERATOR IS ON DUTY.
REMAIN IN SHELTER."
```

#### Intercept 05: `radio_salt_lab_spectrometry_data` (104.200 MHz)
```
[104.200 MHz — SIGNAL: S5 — RESEARCH LOG BROADCAST]
"TRANSMISSION: Research Log 44. Dr. Erik Dahl broadcasting from Low-Background Salt Lab.
Mass spectrometry of fallout dust collected at 400m depth confirms Cesium-137 / Strontium-90 
isotopic ratio 1.042. This signature is unique to automated Arctic silo ordnance. 
Repeat: the warheads were launched by unmanned automatic fail-safe systems. 
There was no foreign human command. Broadcast this data to all units. End the war."
```

#### Intercept 06: `radio_day_360_beacon_silence` (142.850 MHz)
```
[142.850 MHz — SIGNAL: S9 — FINAL MARITIME DECOMMISSION]
CZC ZCZC 360 360 0360
DAY 360 OF THE EXCHANGE
CONTINENTAL METEOROLOGICAL SERVICE REPORTS ASH CLEARANCE OVER SECTOR 4.
SURFACE TEMPERATURE PLUS SIX CELSIUS.
ALL EVACUATION GATES ARE NOW CLOSED.
THE LONG WINTER IS OVER.
SHUTTING DOWN TRANSMITTER.
GOD SPEED.
NNNN
```

---

# IV. COMPLETE 36 SURVIVOR CANDIDATE DOSSIERS

```
┌──────────────────────────────────────────────────────────────────────────────────────────────────┐
│                                SURVIVOR CANDIDATE ROSTER MATRIX                                  │
├────────────────────┬───────────────────────┬────────────┬────────────────────────────────────────┤
│ ID                 │ NAME                  │ OCCUPATION │ PSYCHOLOGICAL TRAIT & CONFLICT         │
├────────────────────┼───────────────────────┼────────────┼────────────────────────────────────────┤
│ `survivor_ottilie` │ Ottilie Frayne        │ Agronomist │ Ruthless Communal (Allotments Pioneer) │
│ `survivor_anneke`  │ Anneke Ruhl           │ Sluice Eng │ Guilt-Ridden Hydro-Baron Deserter      │
│ `survivor_vane`    │ Corporal Felix Vane   │ Sentry     │ Shell-Shocked Garrison Deserter        │
│ `survivor_martha`  │ Sister Martha         │ Penitent   │ Apostate Cultist (Suffering Trauma)    │
│ `survivor_chen`    │ Dr. Sarah Chen        │ Surgeon    │ Clinical Humanist (Morphine Dependent) │
│ `survivor_tomas`   │ Tomas Lind            │ Seed Vault │ Fanatical Preservationist              │
│ `survivor_valeria` │ Valeria Koss          │ Auditor    │ Cold Pragmatist (Calculates Rations)   │
│ `survivor_mikhail` │ Gunner Mikhail        │ Artillery  │ Hearing-Damaged Garrison Veteran       │
│ `survivor_naomi`   │ Naomi Strand          │ Scavenger  │ Claustrophobic Metal Hunter            │
│ `survivor_pavel`   │ Pavel Volkov          │ Boilermaker│ Chronic Smoker, Geothermal Expert      │
│ `survivor_zoya`    │ Zoya Reid             │ Chemist    │ Radon & Chelating Specialist           │
│ `survivor_alder`   │ Captain Alder         │ Convoy Cdr │ Northern Icebreaker Scout              │
│ `survivor_lydia`   │ Lydia Hart            │ Machinist  │ Precision Toolmaker                    │
│ `survivor_erik`    │ Dr. Erik Dahl         │ Physicist  │ Low-Background Lab Principal           │
│ `survivor_igor`    │ Igor Morozov          │ Sniper     │ Paranoiac Sentry                       │
│ `survivor_clara`   │ Clara Sloan           │ Botanist   │ Greenhouse Designer                    │
│ `survivor_vera`    │ Vera Sokolov          │ Telegrapher│ Cipher & Radio Tech                    │
│ `survivor_marcus`  │ Marcus Vane           │ Radio Op   │ Long-Wave Antenna Rig                  │
│ `survivor_lindqv`  │ 1st Officer Lindqvist │ Mariner    │ Arctic Navigator                       │
│ `survivor_brand`   │ Colonel Brand         │ D/9 Officer│ Unforgiving Denial Commander           │
│ `survivor_duth`    │ Ansel Duth            │ Toll Clerk │ Bookkeeper & Scrip Assayer             │
│ `survivor_hadi`    │ Hadi Morrow           │ Peat Cutter│ Heavy Laborer                          │
│ `survivor_kess`    │ Kess Adler            │ Trapper    │ Radiation-Scarred Hunter               │
│ `survivor_len`     │ Len Quill             │ Archivist  │ Historical Document Conservator        │
│ `survivor_markov`  │ Chief Assayer Markov  │ Metallurg  │ High Granite Arsenal Founder           │
│ `survivor_talia`   │ Commander Talia       │ Militia Ldr│ Central Upland Defense Strategist      │
│ `survivor_kroll`   │ Major Kroll           │ Provost Msh│ 3rd Corps Iron Disciplinarian          │
│ `survivor_malachi` │ Hierophant Malachi    │ Cult Elder │ Vitrified Crater Prophet               │
│ `survivor_sapper`  │ Sapper Vance          │ Pioneer    │ Mutinous Penal Demolitionist           │
│ `survivor_elena`   │ Elena Vasquez         │ Switchwoman│ Rail Union Scout Leader                │
│ `survivor_gregor`  │ Gregor the Miner      │ Dynamite   │ Salt Cavern Blaster                    │
│ `survivor_marina`  │ Marina Drake          │ Driver     │ Armored Convoy Specialist              │
│ `survivor_yuri`    │ Yuri Belov            │ Caster     │ Munitions Crucible Tender              │
│ `survivor_nadia`   │ Nadia Brant           │ Scout      │ Mountain Ski Courier                   │
│ `survivor_boris`   │ Boris Kogan           │ Medic      │ Penal Trench Triage Orderly            │
│ `survivor_anton`   │ Anton Vane            │ Broker     │ Underground Salt Exchange Assayer      │
└────────────────────┴───────────────────────┴────────────┴────────────────────────────────────────┘
```

---

# V. THE FIVE DEFINITIVE ENDGAME EPILOGUES (DAY 360 PROSE)

### 1. Epilogue Path A: The Northern Redoubt (Maritime Evacuation)
> *The heavy diesels of the Aurora Borealis shake the ice under your boots. A sailor in an oilskin apron reaches down from the cargo net and hauls up the wooden crate containing your shelter's seed trays and logbooks. Behind you, the gray shoreline of Sector 4 is disappearing into sea smoke. Thirty-two people from your bunker are wrapped in dry wool on the starboard mess deck, drinking hot tea boiled with ship's steam. You left the hatch open on the hillside above Kilometre 19; the wind will fill the entrance vestibule with snow by evening. The war did not stop, but for the people on this ship, the long winter has ended.*

### 2. Epilogue Path B: The Agrarian Concord (The Works Dominion)
> *The glasshouses of the Allotments steam in the cold spring rain. Three hundred acres of floodplain mud have been turned with spade and mattock, planted with the cold-hardened rye strain rescued from the vault. In the caretaker’s office, Ottilie Frayne dips an iron nib into black soot-ink and draws a double rule under the winter casualties. Zero deaths from starvation in the final quarter. The Garrison dismounted their artillery at the crossroads yesterday and traded three gun tractors for thirty sacks of seed potatoes. You hold the first seat on the Works Council, and when the dinner bell rings at noon, four hundred people stand in line with clean tin plates.*

### 3. Epilogue Path C: The Open Ledger (Commercial Federation)
> *The scale at Stallrow never stops swinging. Convoys of high-wheel carts loaded with salt cakes, dried turnip chips, and clean five-gallon glass carboys creak across the shattered viaduct under white flags of truce. Every transaction is entered in the cloth-bound ledger with carbon paper copies for buyer, seller, and road warden. There is no flag, no anthem, and no forgiveness for debts, but a litre of potable water costs exactly two brass cartridges from here to the coast. You sit at the arbitration table with the master key, and no man moves an iron drum without your stamp.*

### 4. Epilogue Path D: The Deep Holdfast (Autonomous Isolation)
> *The dog-bolts on the blast door have rusted solid into their bronze bushings, and you have no intention of oiling them. Outside, the warlords shot their last belts of ammunition into the spring mud and scattered into the hills. Inside, the hydroponic lettuce trays glow emerald green under the ultraviolet lamps, the charcoal air scrubber purrs like a contented cat, and forty-two people live in clean air sixty feet beneath the ash. You answer to no governor, paid no grain tithes, and surrendered no children to the conscription details. The world above burned and froze, but this concrete room remained unbroken.*

### 5. Epilogue Path E: The Measured Truth (The Cold Count)
> *The printer ribbon on the teletype hammered for six straight hours, transmitting the isotopic mass-spectrometer ratios across twelve long-wave frequencies. When the regional commanders read the numbers—the exact Cesium-137 signature proving that the initial launch was an uncorrected automated malfunction from an unmanned silo in the polar desert—the war evaporated. There was no enemy to punish, no nation to avenge, and no glory to salvage. In the garrison barracks at Kilometre 12, soldiers unbolted the armory doors, threw their rifles into the sump pit, and began walking home. The war ended not with a peace treaty, but with a measurement.*
