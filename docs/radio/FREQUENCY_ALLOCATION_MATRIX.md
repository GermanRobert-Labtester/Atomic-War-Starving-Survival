# Frequency Allocation Matrix & Collision Audit

> **Document Status:** Authoritative Dial Layout
> **Authority:** Plan 24 (Task 24C)
> **Tuner Resolution Standard:** 0.05 MHz fine step / 0.5 MHz lock window

---

## 1. Frequency Band Allocation Overview

| Frequency (MHz) | Station ID / Channel | Canonical Broadcaster | Broadcast Types Airing | Collision / Bleed Status |
|---|---|---|---|---|
| **4.625** | `station_numbers_sigint` | The Dead Buzzer (UVB-76 Style Marker) | Monotone buzzer / intermittent cipher bursts | Dedicated HF Channel |
| **7.325** | `station_numbers_sigint` | Shortwave Numbers Array | Synthetic 5-digit phonetic sequences | Dedicated HF Channel |
| **14.487** | `station_numbers_sigint` | The Poacher Echo | Mechanical music box + cipher group | Dedicated HF Channel |
| **19.800** | `station_numbers_sigint` | Cherry Ripe Tape Vault | Corrupted magnetic tape loop | Dedicated HF Channel |
| **52.800** | `faction_ash_walkers` | Ash Walkers Lowband Net | Low-band nomadic chatter & route signs | Dedicated Low VHF |
| **61.900** | `faction_rust_cult` | Rust Cult & Toll Syndicate Net | Iron barter tariffs & scrap collection calls | Dedicated Low VHF |
| **68.200** | `faction_scavengers_guild` | Scavengers Guild Intercom | Salvage claims, wreck locations | Dedicated Low VHF |
| **74.300** | `faction_order_of_bunker` | Order of the Bunker | Liturgical prayers & airlock maintenance | Dedicated Low VHF |
| **77.300** | `distress_meridian_cold` | Meridian Cold Store Distress Beacon | Pavel isolated in sub-level 2 cold room | Authoritative Distress Frequency |
| **88.400** | `station_garrison_overlord` | Iron Garrison / Overlord Actual | Military edicts, raid alerts, convoy logs | Intentional Adjacent Bleed to 88.5 |
| **88.500** | `station_civil_defense` | Central Civil Defense & Public Service | Weather bulletins, public health, lost-and-found | Intentional Adjacent Bleed to 88.4 |
| **91.300** | `station_open_classroom` | The Open Classroom | Educational lessons, survivor network | Dedicated Mid VHF |
| **94.200** | `cipher_winter_ledger` / `warlords` | Logistics Cipher & Toll House Relay | Quartermaster cipher stream / Warlord warnings | Time-gated / intentional bleed |
| **95.400** | `station_tactical_recon` | Forward Tactical Recon | Artillery alerts, combat reports | Phase-bound Mid VHF |
| **96.100** | `station_automated_relay` | Detachment 9 Automated Defense Loop | Rail bridge primed warnings, null carrier | Dedicated Mid VHF |
| **98.500** | `station_scavenger_net` | Scavenger Net & Vinyl Shortwave | Cultural music broadcasts (VinylMorale bridge) | Preset 3 |
| **99.000** | `station_verdict_census` | The Tempest Directorate / Census Carrier | Automated shelter locator / Verdict reckoning | Authoritative Verdict Channel |
| **101.300** | `station_survivor_pirate` | Free Survivor Pirate Carrier | Maria SOS, Westside Collective, coded groups | Dedicated Upper VHF |
| **101.500** | `station_works_allotment` | The Works Public Council | Greenhouse boiler repairs & seed appeals | Dedicated Upper VHF |
| **102.100** | `station_emergency_relay` | Civil Emergency Broadcast System | Fallout alerts, contamination notices | Phase-bound Upper VHF |
| **104.200** | `station_vitrified_crater` | Voice of the Vitrified Crater / Hydro-Barons | Liturgy of the Black Water / Water barter | Authoritative Cult Channel |
| **104.500** | `cipher_relay_count` | Coded Relay Count Carrier | Encrypted coordinate handshake (Cipher Quest) | Authoritative Cipher Channel |
| **104.700** | `station_deep_vault_zero` | Station 0 (The Deep Vault) | Orbital telemetry, radiation advisories | Dedicated Upper VHF |
| **107.800** | `cipher_last_rotation` | Command Cadre Rotation Terminal | Dead-drop shelter protocol (Cipher Quest) | Authoritative Cipher Channel |
| **108.900** | `distress_sector_9` | Sector 9 Substation Distress | Substation technician beacon (5-day trace) | Authoritative Distress Frequency |
| **112.300** | `faction_vault_dwellers` | Vault Dwellers Perimeter Net | Internal security, door status | Dedicated Aircraft VHF |
| **118.500** | `faction_nomadic_clans` | Nomadic Caravan Channel | Seasonal migration routes, water holes | Dedicated Aircraft VHF |
| **120.400** | `distress_emergency_beacon` | Emergency Beacon Channel | Universal emergency beacon preset | Preset 4 |
| **128.100** | `faction_toll_collectors` | Toll Collectors Redoubt | Bridge transit fees, roadblock status | Dedicated Aircraft VHF |
| **134.500** | `distress_relay_44` | Relay 44 Bunker SOS | Isolated communications officer | Authoritative Distress Frequency |
| **136.700** | `faction_silo_keepers` | Silo Keepers Network | Grain reserves, silo ventilator checks | Dedicated Aircraft VHF |
| **142.500** | `station_linemans_loop` | The Lineman's Loop | Infrastructure damage, copper barter | Authoritative Lineman Channel |
| **142.850** | `station_automated_relay` | Icebreaker *Aurora Borealis* Evacuation | Maritime countdown & permafrost radon alert | Authoritative Cold Count Channel |
| **148.200** | `distress_bunker_4_east` | Civilian Bunker 4-East (Trap Signal) | Raider ambush disguised as distress | Authoritative Trap Frequency |
| **162.100** | `distress_marsh_caravan` | Marsh Caravan Distress | Stranded water tanker caravan in toxic mud | Authoritative Distress Frequency |
| **162.800** | `distress_barge_olenka` | Barge *Olenka* Drift Signal | Adrift river barge with seed potatoes | Authoritative Distress Frequency |
| **217.400** | `distress_checkpoint_kilo` | Checkpoint Kilo Automated Beacon | Final log & armory cache reveal | Authoritative Distress Frequency |

---

## 2. Intentional vs Accidental Overlap Rules

1. **88.40 MHz (Garrison) vs 88.50 MHz (Civil Defense):** Authored adjacent-channel bleed. Tuning to 88.45 MHz produces heterodyne static with both voices audible.
2. **99.00 MHz (Automated Shelter Locator vs Verdict Census):** Time-gated progression. Before Day 210, broadcasts standard civilian shelter advice. After Day 210 (Reckoning Culpable), overtaken by the cold machine-register Census Call.
3. **142.85 MHz (Maritime Evacuation vs Radon Warning):** Chronological progression. Days 180–300 broadcasts maritime evacuation updates; Day 300+ adds urgent geological radon advisory.
4. **All Distress Frequencies:** Completely isolated and non-overlapping with routine faction chatter to guarantee clear signal acquisition and clean direction-finding observations.
