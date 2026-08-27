# ASHFALL — Master Expansion Design Bible & 10-Faction Strategic Integration Plan

**Title:** ASHFALL: THE YEAR OF ASH (THE LONG WINTER & THE FINAL RECKONING)
**Internal id:** `expansion_05_the_year_of_ash`
**Timeline Scope:** Day 180 to Day 360 (The Full Nuclear Year Cycle)
**Target Engine:** Godot 4.7+ (.NET/C#) Host + `Ashfall.Core` Engine-Agnostic Simulation
**Status:** Comprehensive Master Design Bible & Grand Geopolitical Architecture
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
│ • Initial Blast & Thermal Rad │ • Internal Roster & Duties      │ • Phase IV: Deep Freeze (-38°C) (180) │
│ • Allocation Schedule 12      │ • The Crossing & Regional Trade │ • Phase V: 10-Faction War (240-300)   │
│ • Basic Bunker Infrastructure │ • Voucher & Backer System       │ • Phase VI: The Great Thaw (300-360)  │
└───────────────────────────────┴─────────────────────────────────┴───────────────────────────────────────┘
```

---

# II. THE 10-FACTION GEOPOLITICAL SCHISM

As the stratospheric winter deepens and resources dwindle to starvation thresholds, Sector 4 splinters into two polarized 5-faction coalitions: the **Directorate & Military Bloc** (enforcing martial central allocation and strategic denial) and the **Rebel, Communal & Autonomy Bloc** (fighting for food sovereignty, free rail transit, and demilitarization).

```
                                  ┌─────────────────────────────────────────┐
                                  │      THE SECTOR 4 GEOPOLITICAL WAR      │
                                  └────────────────────┬────────────────────┘
                                                       │
                 ┌─────────────────────────────────────┴─────────────────────────────────────┐
                 ▼                                                                           ▼
   [DIRECTORATE & MILITARY BLOC]                                               [REBEL & COMMUNAL BLOC]
   1. The Iron Garrison (3rd Corps)                                            6. The Works Allotment Committee
   2. Detachment 9 (STD-9 / Protocol Null)                                     7. The Ash Militia (Upland League)
   3. Continental Logistics Convoy Corps                                       8. Penitent Cult of the Ash Sign
   4. 8th Penal Pioneer Sump Regiment                                          9. Shattered Rail Union & Switchmen
   5. High Granite Munitions Foundry                                           10. Deep Salt Cavern Freeholders
```

---

## 1. Directorate & Military Bloc (5 Factions)

### 1. `faction_central_garrison` — The Iron Garrison (3rd Corps Directorate)
- **Headquarters**: Checkpoint Gamma & Kilometre 12 Redoubts.
- **Doctrine**: Martial Law Schedule 14. All civilian shelters within five kilometres of the rail corridor are subject to immediate requisition of fuel, machine tools, and grain.
- **Strategic Asset**: 152mm heavy towed howitzer batteries dug into the granite bluffs.

### 2. `faction_black_ops` — Detachment 9 (Special Technical Directorate / Protocol Null)
- **Headquarters**: Kilometre 44 Railway Cut Substation.
- **Doctrine**: Total Infrastructure Denial. Standing pre-war orders signed by a dead ministry demand the demolition of every bridge, tunnel, and aqueduct in Sector 4 until a verified cryptographic stand-down code is received.
- **Strategic Asset**: Remote-wired linear Comp-B explosive charges and hardened copper telephone trunks.

### 3. `faction_supply_corps` — The Continental Logistics Escort (Highway 12 Convoy Corps)
- **Headquarters**: Highway 12 Staging Apron.
- **Doctrine**: The Northern Transit Line. Transports bulk medical serums, diesel fuel, and seed stocks between northern deep bunkers and southern military depots using armored halftracks.
- **Strategic Asset**: Multi-fuel armored tracked convoys and heated battery charging arrays.

### 4. `faction_penal_battalion` — The 8th Penal Pioneer Regiment
- **Headquarters**: Sump Mud Trench Sector (Ground Zero Perimeter).
- **Doctrine**: Hazardous Demolition & Trench Labor. Composed of mutinous conscripts, draft resisters, and civilian convicts forced to clear radioactive fallout debris with hand shovels.
- **Strategic Asset**: Explosive breaching sappers and deep earth trench systems.

### 5. `faction_ordnance_foundry` — High Granite Munitions & Arsenal Directorate
- **Headquarters**: High Granite Subterranean Foundry.
- **Doctrine**: Production Hegemony. Operates charcoal-fired furnaces and drop hammers forging brass cartridge casings and ammonium nitrate artillery charges.
- **Strategic Asset**: Tool-steel stamping dies and chemical powder cookers.

---

## 2. Rebel, Communal & Autonomy Bloc (5 Factions)

### 6. `faction_rebuilders` — The Works (Public Works Allotment Committee)
- **Headquarters**: The Allotments (River Floodplain).
- **Doctrine**: Agrarian Communitarianism. Reclaims contaminated alluvial floodplain soil using cold-hardened perennial rye (Strain-7) and maintains the sector's only operational steam autoclave.
- **Strategic Asset**: Polycarbonate glasshouses, seed rhizome cryo-dewars, and refractory brickworks.

### 7. `faction_ash_militia` — The Ash Militia (Central Upland Defense League)
- **Headquarters**: High Mountain Terraces & Switchback 4.
- **Doctrine**: Territorial Sovereignty & Defensive Deadfalls. Mountain farmsteads united under defensive mutual-aid pacts, repelling military foraging patrols with sniper ambushes and log barricades.
- **Strategic Asset**: Dry-stone mountain redoubts and high-angle optical spotting posts.

### 8. `faction_ash_sign` — The Penitent Cult of the Ash Sign (Vitrified Martyrs)
- **Headquarters**: Cathedral Vitrified Strike Crater.
- **Doctrine**: Eschatological Fatalism. Revering the nuclear flash as a divine cleansing of corrupt civilization; fiercely opposed to the military directorate's attempts to restore old-world authority.
- **Strategic Asset**: Fanatical suicide infiltrators and high-radiation tektite glass weapons.

### 9. `faction_railway_guild` — The Shattered Rail Union & Switchmen Guild
- **Headquarters**: Sector 4 Roundhouse & Repeater Hut 14.
- **Doctrine**: Free Transit & Communications. Maintains covert armored steam handcars and hardwired telegraph armature loops, sabotaging military troop trains and smuggling food to besieged shelters.
- **Strategic Asset**: Handcar rail network and loop telegraph wire relays.

### 10. `faction_salt_freeholders` — The Deep Salt Freeholders & Miner Cooperative
- **Headquarters**: 400m Dry Halite Caverns.
- **Doctrine**: Underground Autonomy & Medical Sanctuary. Operates a sterile subterranean forty-bed trauma infirmary and barters pure salt and dynamite under strict armed neutrality.
- **Strategic Asset**: 400m radiation-isolated halite vaults, dynamite magazines, and precision spectrometers.

---

## 3. Extractive & Commercial Cartels (Non-Aligned)
- `faction_hydro_barons` — **The Sluice Association**: Water meter monopoly controlling deep artesian wells in the limestone bluffs.
- `faction_warlords` — **Sector 4 Toll Warlords**: Switchback turnpike raiders extracting ammunition taxes at Kilometre 19.
- `faction_scavengers` — **Low-Background Radiation Runners**: Hazardous metal hunters trading lead pigs and RTG cores.

---

# III. THE 180–360 DAY TIMELINE & ENVIRONMENTAL CRISES

```mermaid
timeline
    title The Nuclear Year Timeline (Days 180 - 360)
    section Phase IV: Deep Freeze (Days 180-240)
        Day 180 : Stratospheric Ash Peak (-38°C)
        Day 195 : Electrical Conduit Thermal Shearing
        Day 210 : The Black Blizzard (938 hPa)
        Day 225 : Diesel Fuel Wax Crystallization
        Day 238 : Blast Door Hydraulic Fluid Lock
    section Phase V: 10-Faction Total War (Days 240-300)
        Day 240 : Martial Law Schedule 14 Promulgated
        Day 255 : High Granite Howitzers Shell Kilometre 19
        Day 268 : Rail Union Derails Munitions Flatcar
        Day 272 : 8th Penal Pioneer Sump Mutiny
        Day 281 : Protocol Null Blasts Railway Viaduct
    section Phase VI: The Great Thaw & Reckoning (Days 300-360)
        Day 300 : Black Mud Radioactive Inundation (+4°C)
        Day 312 : Foundation Radon-222 Bedrock Seepage
        Day 320 : Continental Maritime Transponder Lock (142.850 MHz)
        Day 343 : Mass Spectrometer Warhead Proof
        Day 360 : Day 360 Final Dawn & Evacuation Gate
```

---

# IV. MATHEMATICAL MODELS FOR SUBTERRANEAN SIMULATION

### 1. Radon-222 Infiltration & Scrubber Degradation (`YearOfAshRadonSystem.cs`)
Bedrock thaw fractures during Phase VI release volatile Radon-222 gas from uraniferous granite fissures:
$$\text{Inflow}_{Bq/m^3} = (120.0 + \text{Fissures} \times 280.0) \times (1.0 - \text{ScrubberHealth} \times 0.70)$$
$$\text{Degradation}_{daily} = \left(\frac{\text{IndoorRadon}}{1000.0}\right) \times 1.50$$
- **Safe Threshold**: $\le 200\text{ Bq/m}^3$.
- **Dangerous Threshold**: $\ge 800\text{ Bq/m}^3$ (triggers alpha lung dose accumulation and alarm siren).
- **Remediation**: Replace active charcoal canisters (`item_air_filter_heavy`) and brace bedrock cracks (`item_high_tensile_steel_culvert_brace`).

### 2. Deep Freeze Sub-Zero Thermodynamics (`YearOfAshDeepFreezeSystem.cs`)
Heat loss through concrete shell and intake chimneys at -38°C surface ambient:
$$\Delta T_{indoor} = (\text{GeothermalFlow} \times 0.26) - ((20.0 - T_{surface}) \times (1.0 - \text{InsulationQuality} \times 0.70))$$
$$\text{IcingRate}_{mm/day} = \max(0, |-15.0 - T_{surface}| \times 0.80)$$
- **Critical Blockage Alarm**: $\ge 50\text{ mm}$ hoarfrost ice collar on air intake louvers.
- **De-icing**: Actuate high-output ceramic heating elements (`item_ceramic_heating_element`) and glycol bypass loops.

### 3. Faction War Tension & Proxy Sway (`FactionWarSystem.cs`)
Daily friction calculations drive territorial shifts and civilian siege risk:
$$\Delta \text{Tension} = (\text{ArtilleryFrequency} \times 1.4) + (\text{ResourceScarcity} \times 0.8) - (\text{TradeVolume} \times 0.5)$$

---

# V. AUTHORITATIVE DATA ARCHITECTURE & PORT SCHEMAS

The authoritative data layer resides entirely in JSON files located in `Assets/StreamingAssets/Data/`:

```
Assets/StreamingAssets/Data/
├── door_encounters.json       (60 entries: Survivor-evaluating hatch visitor encounters)
├── year_of_ash_items.json     (48 entries: High-tier ordnance, isotopes, tools, reagents)
├── year_of_ash_events.json    (48 entries: Environmental, faction, and mechanical crises)
├── year_of_ash_locations.json (60 entries: Shelled depots, salt vaults, craters, redoubts)
├── year_of_ash_radio.json     (36 entries: Long-wave transmissions, ciphers, liturgies)
├── year_of_ash_survivors.json (36 entries: Late-game candidate dossiers & confession secrets)
└── year_of_ash_quests.json    (24 entries: Multi-stage branching questline directed graphs)
```

---

# VI. THE FIVE DEFINITIVE EPILOGUES (DAY 360 RESOLUTION)

On Day 360, the simulation aggregates historical player decisions, casualty rolls, faction standings, and technological discoveries to trigger one of five definitive historical epilogues:

1. **The Northern Redoubt (Maritime Evacuation)**: Boarding the *Aurora Borealis* with intact seed stocks and verified survivor manifests, escaping the irradiated valley.
2. **The Agrarian Concord (The Works Dominion)**: Partnering with Ottilie Frayne to establish a permanent agricultural commune on the reclaimed floodplain.
3. **The Open Ledger (Commercial Federation)**: Uniting the Salt Caverns, Rail Union, and Hydro-Barons into an unaligned free-trade network governed by calibration scales.
4. **The Deep Holdfast (Autonomous Isolation)**: Permanently dog-bolting the blast hatch, surviving independently sixty feet beneath the dying surface wars.
5. **The Measured Truth (The Cold Count)**: Broadcasting mass spectrometer proof of the automated silo malfunction, dissolving ideological hostilities across all military commands.

---

# VII. VERIFICATION & CI PROTOCOL

All code must pass the strict dual-engine isolation protocol:
1. `dotnet test Ashfall.Core.Tests` — All 252+ tests must execute cleanly in under 300ms without engine dependencies.
2. `dotnet build Ashfall.csproj` — Godot 4.7+ host presentation layer must build with 0 errors and 0 warnings.
