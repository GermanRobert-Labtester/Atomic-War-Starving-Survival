# ASHFALL Radio Frequency Plan & Multi-Band Receiver Architecture

> **Document Status:** Authoritative Frequency Allocation & Receiver Band Plan
> **Subsystem:** Radio Host Session / Core Tuner / Direction Finding
> **Plan Authority:** Flagship Distress Radio Rescue Missions (Task 7)
> **Core Model:** `Ashfall.Core.Radio.RadioReceiverPlan`

---

## 1. Receiver Architecture

The legacy single-band receiver (which clamped tuning between 88.0 MHz and 150.0 MHz) has been replaced by an authoritative **4-Band Receiver Architecture** covering all 46 authored frequencies across the post-war spectrum (55.1 MHz to 901.2 MHz).

### Band Allocations

| Band ID | Display Name | Frequency Range (MHz) | Frequency Range (kHz) | Step (MHz) | Primary Services & Signals |
|---|---|---|---|---|---|
| `vhf_low` | **VHF-Low** | 50.0 – 150.0 MHz | 50,000 – 150,000 kHz | 0.1 MHz | FM broadcast, civilian emergency, `freq_distress_88_3` (Trapped Mechanic) |
| `vhf_high` | **VHF-High** | 150.0 – 300.0 MHz | 150,000 – 300,000 kHz | 0.1 MHz | Maritime VHF, tactical net, `freq_distress_156_8` (Injured Trader), `freq_distress_192_4` (Raider Trap) |
| `uhf_low` | **UHF-Low** | 300.0 – 550.0 MHz | 300,000 – 550,000 kHz | 0.1 MHz | Civil defense, telemetry, weather, `freq_distress_445_2` (Family Shelter) |
| `uhf_high` | **UHF-High** | 550.0 – 950.0 MHz | 550,000 – 950,000 kHz | 0.1 MHz | Military command links, beacons, `freq_distress_901_2` (Military Patrol) |

---

## 2. Unit Normalization & Precision

To eliminate floating-point drift and discrepancies between Core and Host layers:

- **Core Authority:** `RadioTuner` evaluates continuous tuning in **kHz** integer steps where possible, while `RadioReceiverPlan` provides deterministic conversions:
  ```csharp
  MhzToKHz(float mhz) => (float)Math.Round(mhz * 1000f, 1);
  KHzToMhz(float khz) => (float)Math.Round(khz / 1000f, 4);
  ```
- **Tuning Resolution:**
  - Fine tuning: ±0.1 MHz / ±0.5 MHz
  - Coarse tuning: ±5.0 MHz
  - Band switching: Cyclical navigation (`NextBand()` / `PreviousBand()`) preserves relative position or clamps within target band bounds.

---

## 3. Spectrum Inventory & Channel Allocation (46 Distinct Frequencies)

### Band 1: VHF-Low (50.0 – 150.0 MHz) — 22 Frequencies

| Frequency (MHz) | Allocated Services / Call / Signal | Type | Band |
|---|---|---|---|
| 55.1 | The Pianist's Last Broadcast / Primary School 14 | Distress / Narrative | `vhf_low` |
| 55.6 | Patrol Echo-4 Field Radio | Distress / Tactical | `vhf_low` |
| 67.8 | Deep Geological Beacon Omega | Mystery / Science | `vhf_low` |
| 77.3 | Meridian Cold Store (Pavel) | Distress / Rescue | `vhf_low` |
| 82.1 | Old Quarry Excavator Cab | Distress / Grim | `vhf_low` |
| 88.3 | Trapped Mechanic at Rail Depot | Distress / Rescue (`quest_distress_trapped_mechanic`) | `vhf_low` |
| 88.4 | Faction Baseline Net (Default Receiver Lock) | Faction Carrier | `vhf_low` |
| 88.5 | Caravan 'Greybell' Outriders | Distress / Trade | `vhf_low` |
| 88.9 | Allotment 7 Greenhouse Bench | Distress / Civilian | `vhf_low` |
| 89.6 | Displaced Family — Rail Tunnel | Distress / Rescue | `vhf_low` |
| 91.8 | "Injured Courier" Toll Ambush | Distress / Bait Trap | `vhf_low` |
| 93.4 | Collapsed Cellar — Grange 6 | Distress / Rescue | `vhf_low` |
| 94.2 | Toll House Relay (Warlords Sector 4) | Hostile Warning | `vhf_low` |
| 97.8 | Grain Elevator Silo 3 | Distress / Grim | `vhf_low` |
| 98.5 | Scavenger Net | Civilian Preset | `vhf_low` |
| 98.6 | Holdfast Cultural Relay (Vinyl) | Shelter Broadcast | `vhf_low` |
| 101.3 | Unknown Origin — Band 6 Loop | Distress / Mystery | `vhf_low` |
| 103.4 | "Free Antibiotics" Bait Depot | Distress / Bait Trap | `vhf_low` |
| 104.2 | Hydro-Barons | Faction Net | `vhf_low` |
| 104.7 | Relay Mast Automated Watch | Automated Beacon | `vhf_low` |
| 105.6 | Sub-Basement Pharmacy Clinic | Distress / Grim | `vhf_low` |
| 108.9 | Sector 9 Electrical Substation | Distress / Grim | `vhf_low` |
| 115.2 | Besieged Waystation Echo | Distress / Rescue | `vhf_low` |
| 119.3 | Collapsed Highway Culvert | Distress / Grim | `vhf_low` |
| 120.4 | Distress Beacon | Emergency Preset | `vhf_low` |
| 124.7 | Field Medic Post Omicron (Dr. Araujo) | Distress / Rescue | `vhf_low` |
| 127.6 | Rigged Military Beacon | Distress / False Trap | `vhf_low` |
| 128.5 | Field Rig, Unsigned (Garrison) | Distress / Military | `vhf_low` |
| 129.6 | Repeating Emergency Beacon | Distress / Civilian | `vhf_low` |
| 131.0 | Coded Morse Distress (Bunker X) | Distress / Mystery | `vhf_low` |
| 134.5 | Relay 44 Bunker SOS (Elena Vasquez) | Distress / Rescue | `vhf_low` |
| 138.9 | Maintenance Vault Sump Team | Distress / Rescue | `vhf_low` |
| 141.2 | Decoy S.O.S. Transponder | Distress / Bait Trap | `vhf_low` |
| 142.8 | Almshouse Emergency Handset (Ilze Kaar) | Distress / Medical | `vhf_low` |
| 142.85 | Cold Count Network | Faction Net | `vhf_low` |
| 144.1 | Burned Waystation Redoubt | Distress / Grim | `vhf_low` |
| 148.2 | Civilian Bunker 4-East / Cape Verity Lighthouse | Distress / Trap & Narrative | `vhf_low` |

### Band 2: VHF-High (150.0 – 300.0 MHz) — 12 Frequencies

| Frequency (MHz) | Allocated Services / Call / Signal | Type | Band |
|---|---|---|---|
| 152.4 | Scavenger Pair — Broken Axle | Distress / Rescue | `vhf_high` |
| 156.5 | Abandoned Transmitter Mast | Distress / Grim | `vhf_high` |
| 156.8 | Injured Trader on Route 6 | Distress / Rescue (`quest_distress_injured_trader`) | `vhf_high` |
| 162.1 | Marsh Water Caravan Distress | Distress / Rescue | `vhf_high` |
| 162.8 | Barge 'Olenka' — VHF Channel 16 | Distress / Rescue | `vhf_high` |
| 166.2 | Substation Omega Maintenance Band (Anete Sarn) | Distress / Arc | `vhf_high` |
| 174.5 | Intermittent Ionospheric Echo | Distress / Mystery | `vhf_high` |
| 192.4 | Raider Lure: Fuel Cache | Distress / Trap (`quest_distress_raider_trap`) | `vhf_high` |
| 203.1 | Isolated Water Treatment Worker | Distress / Rescue | `vhf_high` |
| 217.4 | Checkpoint Kilo / District Hospital 3 | Distress / Memorial | `vhf_high` |
| 278.3 | Old Woman's Garden Broadcast | Distress / Civilian | `vhf_high` |
| 288.1 | Faction Tactical Bait | Distress / Bait Trap | `vhf_high` |

### Band 3: UHF-Low (300.0 – 550.0 MHz) — 6 Frequencies

| Frequency (MHz) | Allocated Services / Call / Signal | Type | Band |
|---|---|---|---|
| 311.0 | Salt Mine Survey Team | Distress / Industry | `uhf_low` |
| 311.5 | Stranded Expedition Group | Distress / Rescue | `uhf_low` |
| 333.6 | Impersonated Settlement Call | Distress / Deceptive | `uhf_low` |
| 367.9 | Dead Man's Loop | Distress / Grim | `uhf_low` |
| 392.7 | Automated Weather Station Gamma / North Dam | Distress / Weather & Tech | `uhf_low` |
| 401.9 | Military Convoy Echo-7 / Relay Kestrel-9 | Distress / Military Cache | `uhf_low` |
| 410.7 | Scavenger Kidnap Setup | Distress / Bait Trap | `uhf_low` |
| 445.2 | Family Shelter Distress Call | Distress / Rescue (`quest_distress_family_shelter`) | `uhf_low` |
| 478.2 | Impersonated Medical Evacuation | Distress / Deceptive | `uhf_low` |
| 512.4 | Civil-Defense Emergency Transmitter | Distress / Civil Defense | `uhf_low` |

### Band 4: UHF-High (550.0 – 950.0 MHz) — 6 Frequencies

| Frequency (MHz) | Allocated Services / Call / Signal | Type | Band |
|---|---|---|---|
| 623.8 | Scientific Emergency Beacon | Distress / Science | `uhf_high` |
| 701.3 | Encrypted Military Burst | Distress / Military | `uhf_high` |
| 756.1 | Encrypted Civilian Cipher | Distress / Cipher | `uhf_high` |
| 812.5 | Child's Call for Help | Distress / Moral Choice | `uhf_high` |
| 867.9 | Siblings Hiding from Threat | Distress / Rescue | `uhf_high` |
| 901.2 | Stranded Military Patrol | Distress / Rescue (`quest_distress_military_patrol`) | `uhf_high` |

---

## 4. Collision & Overlap Analysis

1. **Shared Nominal Frequencies:**
   - `55.1 MHz`: Shared between `concert_hall_ruins` (The Pianist) and `Primary School 14`. Primary School 14 is in expansion data; `RadioDistressSystem` uses day/schedule filtering and distinct frequency IDs (`freq_distress_55_1`).
   - `148.2 MHz`: Shared between Civilian Bunker 4-East (Primary) and Cape Verity Lighthouse (Expansion). Primary is authenticated as a raider trap; Cape Verity is an expansion narrative. Both resolve cleanly.
   - `217.4 MHz`: Shared between Checkpoint Kilo Automated Beacon (Primary) and District Hospital 3 (Expansion).
   - `401.9 MHz`: Shared between Military Convoy Echo-7 (Primary) and Relay Station Kestrel-9 (Expansion).
2. **Resolution Guarantee:**
   When multiple transmissions share a nominal frequency, `RadioDistressSystem.FindSignalAtFrequency()` selects the best active signal based on sim day and status, ensuring deterministic and non-conflicting reception.
