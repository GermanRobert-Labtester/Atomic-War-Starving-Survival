# Broadcast Corpus Inventory & Classification

> **Document Status:** Authoritative Corpus Map
> **Authority:** Plan 24 (Task 24A)
> **Total Inventoried Primary Broadcasts:** 118 Baseline (53 base + 50 Year of Ash + 13 Verdict + 5 Distress + 25 Faction War + 8 Numbers Stations + 15 Scriptbook)

---

## 1. Primary Broadcast Catalogs

### 1.1 Base Radio (`radio.json`) — 53 Broadcasts

| ID Range | Frequency | Day Range | Type | Source / Persona | Recurrence | Consequence / Hook |
|---|---|---|---|---|---|---|
| `radio_broadcast_01` – `10` | 88.5 MHz | 1–30 | Civilian / Public Service | Central Civil Defense Service | Repeatable / Window | Weather bulletins, iodine advisories, water plant notices |
| `radio_broadcast_11` – `20` | 102.1 MHz | 1–45 | Military / Emergency Alert | Civil Emergency Relay | Phase-bound | Fallout storm warnings, evacuation routes, looter warnings |
| `radio_broadcast_21` – `30` | 95.4 MHz | 5–60 | Emergency / Tactical | Forward Tactical Recon | Phase-bound | Artillery warnings, supply convoy ambush reports |
| `radio_broadcast_31` – `40` | 99.0 MHz | 10–120 | Automated Loop / Numbers | Automated Shelter Locator Array | Repeatable loop | Water purification, maintenance reminders, cold alerts |
| `radio_broadcast_41` – `50` | 101.3 MHz | 15–120 | Survivor / Pirate / Coded | Fragmented Survivor Transmissions | One-shot / Repeatable | Maria in subway, Westside Collective, coded number blocks |
| `radio_broadcast_relay_count` | 104.5 MHz | 5–365 | Numbers Station / Cipher | Coded Relay Carrier | One-shot puzzle | Plan 11B Cipher Quest (`relay_count` -> `loc_hidden_relay_bunker`) |
| `radio_broadcast_winter_ledger` | 94.2 MHz | 10–365 | Logistics Stream / Cipher | Logistics Reserve Frequency | One-shot puzzle | Plan 11B Cipher Quest (`winter_ledger` -> `loc_logistics_reserve_cache`) |
| `radio_broadcast_last_rotation` | 107.8 MHz | 20–365 | Military Dead Hand / Cipher | Command Cadre Rotation Terminal | One-shot puzzle | Plan 11B Cipher Quest (`last_rotation` -> `loc_deaddrop_command_shelter`) |

---

### 1.2 Year of Ash (`year_of_ash_radio.json`) — 50 Broadcasts

| Sample ID | Frequency | Day Trigger | Type | Source / Broadcaster | Consequence / Audio Ref |
|---|---|---|---|---|---|
| `radio_142_carrier_discovery` | 142.85 MHz | Day 180 | Maritime Rescue / Emergency | Aurora Borealis Icebreaker | Audio: `radio_vo_ch3_ash_road` |
| `radio_garrison_martial_edict` | 88.4 MHz | Day 195 | Military Edict / Faction | Iron Garrison Logistics | Audio: `radio_vo_ch7_milband` |
| `radio_cult_ash_sign_liturgy` | 104.2 MHz | Day 210 | Religious / Faction | Voice of the Vitrified Crater | Audio: `radio_vo_kind_hatch` |
| `radio_d9_protocol_null_carrier` | 96.1 MHz | Day 225 | Automated Defense Loop | Detachment 9 Signal Loop | Rail bridge charges armed |
| `radio_allotment_seed_appeal` | 101.5 MHz | Day 240 | Trade / Community | Works Public Council (Allotment) | Audio: `radio_vo_ch11_stockpile` |
| `radio_deep_thaw_radon_advisory`| 142.85 MHz | Day 300 | Meteorological / Hazard | Civil Meteorological Service | Radon infiltration alert |

---

### 1.3 The Verdict (`verdict_radio.json`) — 13 Broadcasts

| ID | Frequency | Day Trigger | Type | Source | Reckoning Gate |
|---|---|---|---|---|---|
| `radio_verdict_meter_reads_1142` | 99.0 MHz | Day 210 | Telemetry / Machine Register | Census Carrier, Machine Registers | Culpable |
| `radio_verdict_fuse_serviced` | 99.0 MHz | Day 211 | Maintenance Register | Fuse World Service Bay | Culpable |
| `radio_verdict_wing_sleeps` | 99.0 MHz | Day 242 | Telemetry / Drone Hive | Drone Hive Draw Readout | Culpable |
| `radio_verdict_off_count_assessed` | 99.0 MHz | Day 240 | Tribunal Summon / Census Call | The Office of Censuses | Culpable |
| `radio_verdict_eden_was_here` | 88.5 MHz | Day 245 | Witness Bleed | Eden Vale Tube Bleed | Culpable |
| `radio_verdict_count_is_open` | 88.5 MHz | Day 240 | Public Census Order | The Office of Censuses | Culpable |

---

### 1.4 Baseline Distress Signals (`radio_distress_signals.json`) — 5 Signals

| Signal ID | Frequency | Days to Trace | Outcome Category | Revealed Location |
|---|---|---|---|---|
| `freq_distress_217_4` | 217.4 MHz | 4 days | `survivor_community` | `checkpoint_kilo_armory` |
| `freq_distress_148_2` | 148.2 MHz | 3 days | `bait_trap` | `bunker_4_east_trap` |
| `freq_distress_108_9` | 108.9 MHz | 5 days | `abandoned_cache` | `sector_9_substation_cache` |
| `freq_distress_134_5` | 134.5 MHz | 2 days | `survivor_isolated` | `loc_relay_44_bunker` |
| `freq_distress_162_1` | 162.1 MHz | 4 days | `water_caravan_wreck` | `loc_marsh_caravan_wreck` |

---

### 1.5 Faction War Corpus (`faction_war_radio.json`) — 25 Broadcasts

| Sample ID | Frequency | Day Trigger | Type | Broadcaster / Organization |
|---|---|---|---|---|
| `radio_d480_span44_automated_loop` | 96.1 MHz | Day 480 | Automated Defense | Unattended Relay, Span 44 |
| `radio_d481_garrison_continuity_bulletin` | 88.4 MHz | Day 481 | Faction Propaganda | Central Garrison Continuity Office |
| `radio_d484_exchange_roster_wire_rebuttal` | 104.2 MHz | Day 484 | Partisan Rebuttal | Exchange Roster Wire |
| `radio_d490_ash_sign_shrine_transmission` | 142.85 MHz | Day 490 | Religious Polemic | The Ash Sign, Shrine Transmission |
| `radio_d493_toll_syndicate_rate_notice` | 61.9 MHz | Day 493 | Trade / Monopoly | Toll Syndicate Rate Office |

---

## 2. Classification Taxonomy

Each broadcast in the ASHFALL world is strictly classified across four dimensions:

1. **Content Genre:** `News`, `Propaganda`, `Weather`, `EmergencyAlert`, `Distress`, `NumbersStation`, `MusicCulture`, `SerialDrama`, `TradeMarket`, `ReligiousLiturgy`, `VerdictCensus`, `WarCommunique`, `AutomatedLoop`.
2. **Source Reliability:**
   - `Official` (High administrative accuracy, bureaucratic bias)
   - `Partisan` (Faction agenda, subjective claims, spin)
   - `Anonymous` (Unverified survivor reports, pirate transmissions)
   - `Automated` (Machine registers, sensor arrays, immutable loops)
   - `Unknown` (Clandestine numbers stations, mysterious carriers)
3. **Recurrence Model:**
   - `Scheduled` (Airs at fixed time windows / appointment days)
   - `PhaseBound` (Airs during specific campaign days / war stages)
   - `OneShot` (Fires exactly once upon condition trigger; archived to log)
   - `RepeatableLoop` (Plays continuously when tuned within frequency tolerance)
4. **Downstream Consequence:**
   - `PureAtmosphere` (Worldbuilding texture, moral depth, dread)
   - `IntelQuest` (Reveals map node, cipher clue, or expedition target)
   - `DistressMission` (Spawns active rescue opportunity with expiration)
   - `StrategicWarning` (Severe weather alert, orbital perigee, raid threat)
   - `VerdictEvidence` (Authentic testimony or registry log usable in trials)
