# ASHFALL: THE YEAR OF ASH (DAYS 180–360) — Master Creative Pack & Narrative Bible

**Internal id:** `expansion_05_the_year_of_ash`  
**Kind:** Shippable prose & definitive narrative resolution. Companion to `docs/expansions/expansion_05_the_year_of_ash_plan.md` and `docs/lore/06_REBUILDERS_AND_BLACK_OPS.md`.  
**Voice lock:** Cold, exhausted, human, restrained. Specificity over adjectives. The game never tells the player how to feel.  
**VO:** Lines marked `[VO]` are text-first; record only if the radio/tannoy pipeline exists. Everything else is UI/Codex/inspect.  

---

# I. THE CONTINUITY FORK: POWERS, WORKS, DENIAL, AND TITHES

### The Core Architectural & Narrative Axiom

In *ASHFALL*, adding new faction-shaped entities to a world with a tightly drawn geopolitical map risks diluting the weight of every faction. The resolution to the continuity fork is straightforward: **the new entities are not the same *kind* of thing as the original four Powers.**

```
┌─────────────────────────────────────────────────────────────────────────────────────────────────────────┐
│                                 THE TAXONOMY OF FORCE IN SECTOR 4                                       │
├────────────────────────────────┬────────────────────────────────┬──────────────────────────────────────┤
│ THE FOUR POWERS                │ THE CURRENTS & TRADES          │ THE DENIED & THE WORKS               │
│ • Sovereign territorial claims │ • Trans-regional practices     │ • Unrescinded military taskings (D/9)│
│ • Pre-war map borders          │ • Access granted or lost       │ • Municipal public works (The Works) │
│ • Standing armies & conscripts │ • No sovereign territory       │ • Industrial water monopolies        │
│ • Demand political allegiance  │ • No diplomatic standing       │ • Market & utility hegemony          │
└────────────────────────────────┴────────────────────────────────┴──────────────────────────────────────┘
```

---

## 1. The Original Four Powers (Sovereign Belligerents)

The four entities defined in `01_GAZETTEER.md` and `05_FACTIONS.md` are **Powers**:
1. **The Iron Garrison** (`faction_central_garrison` / `iron_garrison`): Holds the pre-war military redoubts, rail yards, and armories. Governs through martial decrees, logistics schedules, and armed conscription.
2. **The Ash Militia** (`faction_ash_militia` / `ash_militia`): Controls the agricultural terraces of the Central Uplands. Governs through communal defense pacts, lineage registries, and defensive fortifications.
3. **The Cult of the Ash Sign** (`faction_ash_sign` / `cult_of_ash_sign`): Holds the irradiated crater zones and deep ash fissures. Governs through theological fatalism, radium cleansing rites, and penitent suicide columns.
4. **The Warlords of Sector 4** (`faction_warlords` / `warlords_sector_4`): Controls the high-pass toll gates and switchback causeways. Governs through transit taxation, posted ledgers, and armed checkpoints.

### Why They Are Powers:
- **Territory**: They hold contiguous sub-regions on the master map.
- **Belligerence**: They maintain organized fighting formations and can wage formal, sustained territorial war.
- **Diplomatic Query**: A Power asks the player: *"Whose side are you on?"*
- **Player Mechanics**: Siding with one Power shifts broad reputation vectors, unlocks faction-specific military technology, and locks out opposing sovereign territories.

---

## 2. Why The Rebuilders Never Counted as a Power

**Canonical Designation:** `faction_rebuilders` · **The Works** (Public Works Allotment Committee)

The Rebuilders do not call themselves "The Rebuilders." They call themselves **the Works**, short for the municipal public works department that employed three of their founders before the Exchange. "The Rebuilders" was a derisive nickname coined by the Toll guards at Kilometre 19, and it stuck.

```
┌──────────────────────────────────────────────────────────────────────────────────┐
│                                THE WORKS PARADOX                                 │
├──────────────────────────────────────────────────────────────────────────────────┤
│ • Settled the floodplain: only viable agricultural loam in Sector 4              │
│ • The ground is rich because it floods with contaminated river runoff            │
│ • Result: They produce food for 200 people, but are 70 days from dying of thirst │
│ • They run the sector's ONLY working autoclave and still in a caretaker's shed   │
└──────────────────────────────────────────────────────────────────────────────────┘
```

The Works cannot wage a war. If the Garrison deployed a single motorized infantry company with mortar support, the Allotments would fall in ninety minutes. The Garrison does not do this because the Garrison's quartermaster has calculated that three tons of rye flour and forty bushels of dried turnips per quarter are worth more than fifty acres of radioactive mud and seventy dead agronomists.

The Works holds **soil**, not **sovereignty**. They hold **technology**, not **territory**.

---

## 3. Why Black Ops (D/9) Never Counted as a Power

**Canonical Designation:** `faction_black_ops` · **Detachment 9 / Protocol Null**

Black Ops is not an army, a faction, or a political movement. It is **eighteen surviving men and women** operating out of a reinforced telecommunications bunker beneath the railway cut at Kilometre 44.

They were part of the Ministry of Supply's Special Technical Directorate (STD-9), tasked before the Exchange with a single contingency protocol: **deny the industrial infrastructure of Sector 4 to enemy forces in the event of territorial collapse.**

```
┌──────────────────────────────────────────────────────────────────────────────────┐
│                           THE PROTOCOL NULL DILEMMA                              │
├──────────────────────────────────────────────────────────────────────────────────┤
│ • D/9 knows the bridge is already broken.                                        │
│ • D/9 knows the country they were defending has ceased to exist.                 │
│ • D/9's standing orders are on perforated paper signed by a dead minister.       │
│ • The orders state: 'Maintain denial posture until relief arrives.'              │
│ • Relief has not arrived in 240 days.                                            │
└──────────────────────────────────────────────────────────────────────────────────┘
```

D/9 does not want land. They do not want tax revenue. They do not want converts. They want **the six remaining unblown culverts on the rail line to stay unblown until their radio gives them a verified stand-down code** that will never come, because the transmitter in the capital was destroyed on Day 1.

---

## 4. Why The Hydro-Barons Never Counted as a Power

**Canonical Designation:** `faction_hydro_barons` · **The Sluice Association**

The Hydro-Barons are four families who control the three pre-war deep-bore artesian wells in the limestone bluff north of the salt flats. They have no uniform, no flag, no ideology, and no soldiers beyond eight men with shotguns and a pack of starved lurchers.

They do not claim sovereignty. They claim **the meter**.

Every litre of potable water that moves through the northern pipeline network is metered through a brass positive-displacement flow counter manufactured in 1968. The Hydro-Barons sit on folding chairs in the pump house and record the counter reading in a cloth-bound ledger twice a day.

---

# II. MASTER CROSS-WALK: 40 SHELTER DOOR ENCOUNTERS

Every door encounter in Phase IV–VI evaluates who is standing behind the hatch before presenting player options.

```
┌──────────────────────────────────────────────────────────────────────────────────────────────────┐
│                               DOOR ENCOUNTER EVALUATION PIPELINE                                 │
├──────────────────────────────────────────────────────────────────────────────────────────────────┤
│ 1. Scan living bunker roster (Medical state, radiation tier, guilt score, trauma bonds).         │
│ 2. Match encounter condition tags (e.g. `needs_doctor`, `has_ex_garrison`, `fissure_open`).      │
│ 3. Generate dynamic survivor reaction previews in modal UI.                                      │
│ 4. Resolve choice consequences across morale, inventory, contagion, and faction tension.        │
└──────────────────────────────────────────────────────────────────────────────────────────────────┘
```

### Encounters #1 to #10 (Deep Freeze Early Phase, Days 180–205)

#### 1. `enc_the_frozen_courier` — The Messenger from Kilometre 19
- **Visitor**: A young dispatch runner from the Toll Road, frost on eyelashes, carrying an oilcloth pouch sealed with lead wire.
- **Trigger**: Day >= 180, ambient temp <= -25°C.
- **Roster Reactions**:
  - *If Corporal Vane is present*: "That’s a 4th Battalion courier pouch. If he’s carrying it, the switchboard at the Pass is dead."
  - *If Dr. Chen is present*: "Look at his hands. If we don't warm those fingers in tepid saline within thirty minutes, he loses both index fingers."
- **Choices**:
  1. *Admit him, thaw his hands, trade for the dispatch pouch* (-10% Heating Reserve, +1 `item_one_time_cipher_pad_d9`, +5 Morale).
  2. *Take the pouch through the mail slot, leave him outside* (+1 `item_one_time_cipher_pad_d9`, -12 Morale, +8 Guilt).
  3. *Refuse to unbolt the hatch* (+0 Resources, -5 Morale).

#### 2. `enc_deserter_family_in_rags` — The Salt Flats Evacuees
- **Visitor**: A father, mother, and eight-year-old child wrapped in fiber insulation and packing tape.
- **Trigger**: Day >= 185, ambient temp <= -30°C.
- **Roster Reactions**:
  - *If Sister Martha is present*: "The child is coughing blood. It is the grey lung from the ash pits. We have blankets in the annex."
  - *If Valeria Koss is present*: "Three more mouths. That’s 2,100 calories a day we don't have. Check their boots first."
- **Choices**:
  1. *Grant sanctuary in the quarantine vestibule* (-3 Rations/day, +10 Morale, +15 Guilt if food runs out).
  2. *Provide 2 tins of rations and a wool blanket, turn them away* (-2 `item_continuity_ration_biscuit_tin`, -1 `item_thermal_lining`, +0 Morale).
  3. *Drive them off with the intercom klaxon* (-15 Morale, +20 Guilt to all Humanist survivors).

#### 3. `enc_wandering_trauma_surgeon` — Dr. Sarah Chen's Arrival
- **Visitor**: An elderly surgeon pushing a bicycle with solid rubber tires loaded with lead-shielded instrument cases.
- **Trigger**: Day >= 190, medical supplies > 0.
- **Roster Reactions**:
  - *If any survivor has `frostbite_stage_2`*: "She’s carrying bone saws and dry suture reels. Let her in before the frost takes her feet."
- **Choices**:
  1. *Recruit Dr. Chen into the permanent roster* (+1 Survivor `survivor_dr_sarah_chen`, unlocks Advanced Surgery).
  2. *Trade surgical blades for iodine* (-20 Iodine, +1 `item_surgical_bone_chisel`).
  3. *Turn away* (-10 Morale).

#### 4. `enc_garrison_requisition_detail` — The Schedule 14 Sweep
- **Visitor**: Four Garrison conscripts in sheepskin coats with a tracked sledge, demanding 40 kg of grain or 20 litres of diesel.
- **Trigger**: Day >= 195, faction tension >= 30%.
- **Roster Reactions**:
  - *If Gunner Mikhail is present*: "Sergeant's coat is patched at the shoulder. They're foraging without supply orders. We can refuse."
- **Choices**:
  1. *Hand over 20L diesel to satisfy the requisition* (-1 Fuel, +5 Garrison Rep).
  2. *Bribe the sergeant with a bottle of pure alcohol* (-1 Spirits, +0 Garrison Rep, keeps diesel).
  3. *Refuse through the gunport* (+15 Faction Tension, potential mortar retaliation).

#### 5. `enc_works_seed_custodian` — Tomas Lind with Cryo Rhizomes
- **Visitor**: Tomas Lind, breathless, carrying a vacuum dewar of frost-resistant rye roots.
- **Trigger**: Day >= 200.
- **Choices**:
  1. *Admit Tomas and store the rhizomes in the cold locker* (+1 Survivor, +1 `item_perennial_wheat_strain_7`, +15 Rebuilder Rep).
  2. *Refuse entry* (-10 Rebuilder Rep).

#### 6. `enc_penitent_ash_flagellant` — Brother Paul's Liturgy
- **Visitor**: A barefoot man carrying a glowing chunk of vitrified blast glass in a wire cage.
- **Choices**:
  1. *Confiscate the radioactive glass and isolate him* (+1 `item_sealed_lead_pig`, +1 Rad Dose).
  2. *Refuse* (No effect).

#### 7. `enc_black_ops_wiretap_scout` — Agent Ross at the Periscope
- **Visitor**: A figure in a white sniper smock inspecting the bunker's exterior coaxial cable.
- **Choices**:
  1. *Challenge over the external loudspeaker* (Scout retreats, leaves note).
  2. *Fire a warning shot* (+20 D/9 Hostility).

#### 8. `enc_hydro_baron_meter_clerk` — The Water Ledger Audit
- **Visitor**: A clerk with brass calipers and a receipt book demanding maintenance salt.
- **Choices**:
  1. *Pay 10 units of salt* (+10 Hydro-Baron Rep, preserves water pressure).
  2. *Refuse payment* (Water pressure halved next 5 days).

#### 9. `enc_frostbitten_militia_sentry` — Corporal Vane's Companion
- **Visitor**: A half-frozen militia lookout with blackened cheekbones.
- **Choices**:
  1. *Admit and amputate two toes* (-2 Medical, +1 Survivor `survivor_felix_vane`).
  2. *Refuse* (Sentry freezes on the stoop, -15 Morale).

#### 10. `enc_frozen_allotment_scavenger` — Naomi Strand's Barter
- **Visitor**: Naomi Strand carrying a bundle of salvaged brass fittings.
- **Choices**:
  1. *Trade 5 rations for 3 brass fittings* (-5 Rations, +3 `item_brass_valve_fitting`).
  2. *Recruit Naomi Strand* (+1 Survivor `survivor_naomi_strand`).

---

### Encounters #11 to #25 (Deep Freeze to Faction Siege, Days 206–280)

#### 11. `enc_geothermal_well_mechanic` — Pavel Volkov
- **Visitor**: A boilermaker from the steam plants with a manifold wrench.
- **Benefit**: Can repair damaged geothermal heating loops instantly if admitted.

#### 12. `enc_cult_ash_sign_converts` — The Three Martyrs
- **Visitor**: Three young novices requesting permission to sit inside the warm exhaust plume.
- **Risk**: Radiation contamination (+15 mSv) if allowed near air intakes.

#### 13. `enc_d9_null_detonator_courier` — Cipher Pouch Delivery
- **Visitor**: An ununiformed courier delivering an encrypted envelope addressed to "Bunker Station 04".
- **Benefit**: Contains `item_one_time_cipher_pad_d9` required for Protocol Null stand-down.

#### 14. `enc_starving_allotment_child` — The Bread Scraps
- **Visitor**: A nine-year-old girl with a wooden bowl.
- **Moral Choice**: High guilt delta if turned away; minimal resource drain if fed.

#### 15. `enc_black_blizzard_stragglers` — Four Lost Miners
- **Visitor**: Four quarrymen blinded by blowing ash in -40°C storm.
- **Choice**: High shelter capacity test; adds 4 mouths or severe guilt penalty.

#### 16. `enc_garrison_provost_marshall` — Major Kroll's Inquiry
- **Visitor**: Garrison officer investigating the disappearance of a patrol.
- **Danger**: Requires high Trust or `item_falsified_clearance` to avoid bunker search.

#### 17. `enc_works_autoclave_technician` — Lydia Hart
- **Visitor**: Agronomy engineer carrying an autoclave seal.
- **Benefit**: Improves hydroponic yield by +30% if recruited.

#### 18. `enc_ash_sign_pyre_apostate` — Sister Martha's Brother
- **Visitor**: A fleeing cultist who refused to walk into the vitrified crater.
- **Narrative**: Triggers deep confession dialog if Sister Martha is in shelter.

#### 19. `enc_hydro_baron_enforcer_squad` — The Sluice Guards
- **Visitor**: Armed guards demanding return of an escaped pipe-welder.
- **Choice**: Surrender survivor or risk valve cut-off.

#### 20. `enc_continental_convoy_outrider` — Captain Alder's Scout
- **Visitor**: Snowmobile scout marking waypoints for the Day 340 maritime convoy.
- **Benefit**: Unlocks `loc_continental_convoy_staging_area` on regional map.

#### 21. `enc_frozen_lead_pig_carrier` — The Isotope Smuggler
- **Visitor**: Smuggler with a heavy lead container containing `item_strontium_90_thermoelectric_pellet`.

#### 22. `enc_garrison_deserter_sniper` — Igor Morozov
- **Visitor**: Sentry carrying a scoped rifle, offering perimeter protection for shelter.

#### 23. `enc_works_greenhouse_botanist` — Clara Sloan
- **Visitor**: Hydroponics specialist with winter-hardened seed trays.

#### 24. `enc_d9_demolition_sapper` — Sapper Vance
- **Visitor**: Engineer offering `item_plastic_explosive_block` in exchange for dry sleeping quarters.

#### 25. `enc_fleeing_switchyard_telegrapher` — Vera Sokolov
- **Visitor**: Railway telegrapher with logs of final military troop movements.

---

### Encounters #26 to #40 (The Great Thaw & The Final Reckoning, Days 281–360)

#### 26. `enc_black_thaw_mud_refugees` — The Flooded Basement Families
- **Visitor**: 6 survivors covered in radioactive black silt after permafrost collapse.

#### 27. `enc_radon_scrubber_merchant` — The Filter Peddler
- **Visitor**: Scavenger selling heavy charcoal radon filter cartridges (`item_air_filter_heavy`).

#### 28. `enc_salt_chamber_physicist` — Dr. Erik Dahl
- **Visitor**: Senior researcher from `loc_low_background_lab` carrying spectrometer tubes.

#### 29. `enc_crystal_radio_enthusiast` — Marcus Vane
- **Visitor**: Radio operator with frequency logs for the 142.850 MHz emergency beacon.

#### 30. `enc_continental_maritime_envoy` — First Officer Lindqvist
- **Visitor**: Envoy from icebreaker *Aurora Borealis* verifying survivor manifest names.

#### 31. `enc_ash_sign_last_penitent` — The Dying Prophet
- **Visitor**: Cult elder seeking a dry place to expire without radiation burns.

#### 32. `enc_garrison_mutineer_platoon` — The Broken Ranks
- **Visitor**: 8 soldiers who abandoned Checkpoint Gamma after their commander died.

#### 33. `enc_works_final_grain_caravan` — The Bumper Rye Harvest
- **Visitor**: Two wagons of fresh spring grain requesting salt and tool trades.

#### 34. `enc_hydro_baron_pipeline_refugee` — The Ruined Overseer
- **Visitor**: Former well-owner whose pumps silted up during the mud thaw.

#### 35. `enc_d9_protocol_null_commander` — Colonel Brand
- **Visitor**: Final commander of D/9 seeking authorization to seal the railway cut.

#### 36. `enc_strontium_rtg_salvager` — The Glowing Sled
- **Visitor**: Scavenger with terminal acute radiation sickness offering power core.

#### 37. `enc_permafrost_subsidence_surveyor` — Geotech Engineer
- **Visitor**: Engineer warning of foundation shear cracks in nearby bunkers.

#### 38. `enc_radio_142_carrier_operator` — The Beacon Technician
- **Visitor**: Technician needing quartz crystal resonator to synchronize global signal.

#### 39. `enc_icebreaker_rendezvous_runner` — The Final Call
- **Visitor**: Runner announcing the 48-hour boarding window for the northern ship.

#### 40. `enc_day_360_final_dawn_witness` — The First Crow
- **Visitor**: A child pointing at a living bird on the periscope rim on the 360th morning.

---

# III. REGIONAL EXPLORATION NODES (30 LOCATIONS)

```
Sector 4 Map Topology (30 Authoritative Exploration Nodes)
├── The Works Agrarian Basin (Locs 01, 10, 19, 29)
├── D/9 Railway Cut & Military Caches (Locs 02, 15, 18, 27, 28)
├── Hydro-Baron Sluices & Coastal Pumping (Locs 03, 13, 16, 23)
├── High Granite Ridge & Radio Relays (Locs 04, 14, 24, 25)
├── Deep Geological & Salt Vaults (Locs 05, 06, 17, 21)
├── Garrison Redoubts & Switchyards (Locs 07, 12, 20, 26)
└── Northern Maritime Sound & Estuary (Locs 08, 09, 22, 30)
```

### Detailed Node Specifications (Sample 5 of 30)

#### `loc_low_background_lab` — Low-Background Salt Chamber Laboratory
- **Elevation**: -420m (Beneath the limestone formation).
- **Environment**: Completely isolated from surface cosmic radiation. Ambient background: 0.02 µSv/h. Steel walls forged in 1938 before atmospheric nuclear testing.
- **Inspect Text**: "The lead brick shielding is stacked three layers deep. Inside the inner counting chamber sits a mass spectrometer tube with German vacuum seals intact. On the chalkboard, an uncompleted half-life calculation dated October 14th."
- **Scavenge**: `item_calibrated_mass_spectrometer_tube`, `item_lead_shielded_sample_cask`, `item_sealed_lead_pig`.

#### `loc_aurora_borealis_grounding_shoal` — Anchorage of the *Aurora Borealis*
- **Elevation**: Sea Level (High Arctic Estuary).
- **Environment**: Pack ice grinding against a 6,000-ton icebreaker hull. Grey sea smoke rising from leads in the ice. Heavy bunker-C diesel exhaust in the air.
- **Inspect Text**: "The *Aurora Borealis* sits locked in four feet of shore-fast ice. Her auxiliary generator chugs steadily, throwing a single yellow beam across the snow. Armed lookouts in seal-skin coats watch the causeway from the bridge wing."
- **Scavenge**: `item_continental_maritime_transponder`, `item_icebreaker_rendezvous_flare_rocket`, `item_evacuation_manifest_scroll`.

#### `loc_geothermal_well_alpha` — Geothermal Steam Well Alpha
- **Elevation**: +180m (Volcanic Fracture).
- **Environment**: Superheated steam screaming through mineral-crusted pressure valves at 180°C. Heavy sulfur smell. Mud pots boiling beside the catwalk.
- **Inspect Text**: "The primary steam manifold is shaking against its anchor bolts. Ice has formed on the exhaust louvers while the pipe itself glows dull red through scale. If the bypass valve sticks, the entire manifold will rupture."
- **Scavenge**: `item_brass_valve_fitting`, `item_glycol_antifreeze_canister`, `item_ceramic_heating_element`.

#### `loc_denial_cut_substation` — D/9 Denial Substation
- **Elevation**: +95m (Railway Cut).
- **Environment**: Blast-damaged concrete switch house surrounded by triple-apron concertina wire and claymore tripwires.
- **Inspect Text**: "A dead teletype machine sits on a green steel desk. Perforated paper tape spills across the floor into frozen puddles. On the wall, a wooden map board shows every bridge in Sector 4 marked with a red china marker 'X'."
- **Scavenge**: `item_one_time_cipher_pad_d9`, `item_plastic_explosive_block`, `item_denial_detonator_spool`.

#### `loc_the_allotments` — The Works Allotment Commune
- **Elevation**: +45m (River Floodplain).
- **Environment**: Five acres of double-glazed polycarbonate cold-frames covered in burlap mats. Composting manure heaps steaming in the sub-zero wind.
- **Inspect Text**: "Dozens of people in padded boiler suits move between the rows with wooden scrapers, clearing frost from the glass. In the center shed, a wood-fired copper still bubbles quietly, filling five-gallon glass carboys with pure water."
- **Scavenge**: `item_perennial_wheat_strain_7`, `item_hermetic_hatch_silicone_gasket`, `item_corrosion_inhibitor_drum`.

---

# IV. VERBATIM EMERGENCY RADIO TRANSMISSIONS (18 LOGS)

### Broadcast 01: `radio_142_carrier_discovery` (142.850 MHz)
```
[142.850 MHz — SIGNAL STRENGTH: S7 — MODULATION: AM TELETYPE]
...CZC ZCZC 014 340 0014
FROM: CONTINENTAL MARITIME RESCUE COORDINATION (ICEBREAKER AURORA BOREALIS)
TO: ANY REMAINING CIV OR MIL SHELTERS SECTOR 4 AND 8
BT
MARITIME EVACUATION CORRIDOR REMAINS OPEN AT NORTHERN SOUND ANCHORAGE
COORDINATES 68-14-N 014-22-E. 
VESSEL MAINTAINS STEAM. DEPARTURE LOCKED DAY 360 AT FIRST LIGHT.
MANIFEST RESTRICTED TO CERTIFIED LOW-RAD RESIDENTS AND REGISTERED SEED CUSTODIANS.
BRING LOGBOOKS, CALIBRATION DATA, AND ZERO UNCHECKED ORDNANCE.
IF YOU HEAR THIS, ACKNOWLEDGE ON 142.850 WITH SHELTER SERIAL AND HEADCOUNT.
BT
NNNN...
```

### Broadcast 02: `radio_garrison_martial_edict` (88.400 MHz)
```
[88.400 MHz — SIGNAL STRENGTH: S9 — VOICE TRANSMISSION — RECORDED LOOP]
"This is the Iron Garrison Logistics Directorate, Sector 4 Command.
By order of the Military Governor, all subterranean shelters within five kilometres of
the rail line are placed under Martial Allocation Schedule 14.
Turn out all excess fuel reserves, stored grain exceeding twelve kilograms per occupant,
and any functioning machine-tool components at Checkpoint Gamma by Tuesday morning.
Shelters failing to comply will be classified as non-compliant supply caches.
We are not negotiating. We are feeding the men who hold the perimeter.
End of transmission."
```

### Broadcast 03: `radio_cult_ash_sign_liturgy` (104.200 MHz)
```
[104.200 MHz — SIGNAL STRENGTH: S4 — CARRIER FLUTTER — LIVE VOICE]
"Do not scrape the soot from your windowpanes, brothers. The ash is the veil of His mercy.
The ice came because the world was unclean and required a cold sheet.
On the three-hundredth day, the water will run black from the hills.
Drink of the black water and be sealed.
Those who hide in concrete with brass locks are already dead; their tombs are only warm.
The fire cleared the slate; the frost will freeze it; the thaw will wash the slate clean.
Praise the flash. Praise the silent crater."
```

### Broadcast 04: `radio_d9_protocol_null_carrier` (96.100 MHz)
```
[96.100 MHz — SIGNAL STRENGTH: S8 — AUDIO FREQUENCY SHIFT KEYING]
"STATION NULL. STATION NULL.
MESSAGE 088 / AUTH CODE: LIMA-NOVEMBER-SEVEN-FOUR.
TARGET GRID 44-ALPHA RAILWAY BRIDGE REMAINS PRIMED.
STAND-DOWN TIMERS WILL NOT RESET WITHOUT SECTOR GENERAL ORDER 1.
ANY ATTEMPT BY CIVILIAN OR MILITIA UNITS TO TRANSIT THE CUT WILL TRIGGER CHARGES.
THIS IS AN AUTOMATED SYSTEM. NO OPERATOR IS ON DUTY.
REMAIN IN SHELTER."
```

---

# V. THE 24 LATE-GAME SURVIVOR DOSSIERS

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
└────────────────────┴───────────────────────┴────────────┴────────────────────────────────────────┘
```

---

# VI. THE FIVE DEFINITIVE ENDGAME EPILOGUES (DAY 360 PROSE)

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
