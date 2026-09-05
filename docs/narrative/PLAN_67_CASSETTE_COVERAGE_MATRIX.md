# Plan 67 — Cassette Sets Coverage & Differentiation Matrix

## 1. Complete Catalog Matrix (12 Sets / 48 Parts)

| # | Set ID | Title | Parts | Speaker & Profession | Location Family | Cache Location | Core Conflict | Ending Mode |
|---|---|---|---|---|---|---|---|---|
| 1 | `checkpoint_kilo` | The Last Days of Checkpoint Kilo | 4 | Corporal Maren, Border Guard | Military Checkpoint | `checkpoint_kilo_armory` | Air filtration failure during siege | Static / Fatal filtration truth |
| 2 | `hospital_saint_maren` | The Saint Maren Tapes | 3 | Dr. Alistair, Chief Surgeon | Hospital Clinic | `hospital_pharmacy` | Triage Protocol Omega and rationing | Pharmacy key bequest to survivors |
| 3 | `family_bunker` | The Martinez Family Recordings | 3 | Mateo & Ana Martinez | Residential Shelter | `family_bunker_backyard_shed` | Sickness and radiation in private bunker | Small child left alone |
| 4 | `resistance_broadcasts` | The Free Radio Tapes | 4 | Elena, Pirate Broadcaster | Pirate Radio Station | `old_library_cache` | Evasion of conscription patrols | Forced shutdown / Signal ends |
| 5 | `field_hospital_7` | Field Hospital 7 | 5 | Sister Judith, Military Nurse | Military Field Hospital | `prewar_medical_cache` | Progressive supply collapse & bed loss | Tagging unmovable patients |
| 6 | `evacuation_train` | The Evacuation Train | 4 | Janos, Line Dispatcher | Railway Corridor | `loc_cut_abandoned_depot` | Network breakdown & dead-end track | Train stopped at cold junction |
| 7 | `station_14` | Station 14 | 6 | Pavel, Broadcast Engineer | High-Power Relay Mast | `loc_radio_relay_mast` | Loss of central feed & community relay | Keeping the carrier wave alive |
| 8 | `greenhouse_tapes` | The Greenhouse Tapes | 3 | Dr. Vane, Crop Botanist | Agricultural Research | `loc_seed_library_annex` | Atmospheric opacity & crop blight | Sealing the heirloom seed stock |
| 9 | `fathers_tapes` | Father's Tapes | 4 | Thomas, Municipal Clerk | Municipal Tenement | `loc_municipal_archive` | Separation during quarantine & rationing | The blue cup / last contingency |
| 10 | `dam_keeper_log` | The Dam Keeper's Log | 5 | Chief Operator Ericson | Hydroelectric Dam & Pumps | `loc_pump_station_nine` | Grid islanding & hydraulic surge risk | Manual sluice lock & shutdown |
| 11 | `teachers_recordings` | The Teacher's Recordings | 3 | Clara, Primary Teacher | District Schoolhouse | `loc_school_gymnasium` | Diminishing attendance & quiet crisis | Reading final roll call to silence |
| 12 | `quarantine_tapes` | The Quarantine Tapes | 4 | Dr. Corvo, Epidemiologist | Isolation Ward | `location_hospital_psych_wing` | Unknown vector & test kit exhaustion | Protocol failure / unverified release |

---

## 2. Plan 06B (Echoes & Narrative Encounters) Overlap Audit

A forensic scan of `echoes.json` and narrative events confirms zero structural or narrative collision:
- **Echoes** are immediate physical discoveries with interactive moral/stat choices (`echo_answering_machine`, `echo_childs_coat`, `echo_frozen_scout`).
- **Cassette Sets** are multi-part sequential audio journals retrieved as tangible tape artifacts that unlock physical caches and long-form narrative understanding.
- **Differentiated Themes:** None of the 8 new sets replicate the existing 4 sets:
  - `field_hospital_7` is military front-line casualty triage (distinct from `hospital_saint_maren`'s civilian surgical ethics).
  - `quarantine_tapes` is epidemiological containment and diagnostic failure (distinct from personal illness in `family_bunker`).
  - `station_14` is technical carrier maintenance and community messages (distinct from `resistance_broadcasts`' political resistance).
  - `fathers_tapes` focuses on separation and missed appointments (distinct from the bunker confinement of `family_bunker`).

---

## 3. Voice & Texture Differentiation

| Set | Register & Cadence | Key Terminology & Motifs | Recording Context |
|---|---|---|---|
| `field_hospital_7` | Clinical, exhausted, precise | Bed triage, penicillin lot, transport manifest, patient tags | Shift debriefs into pocket recorder |
| `evacuation_train` | Terse, operational, route-oriented | Block signal, diversion switch, axle temperature, dead track | Train logbook dictation |
| `station_14` | Technical, patient, public-facing | Megawatts, carrier frequency, caller ledger, dummy load | Transmitter console open mic |
| `greenhouse_tapes` | Scientific, observational, pragmatic | Lux measurements, chlorophyll necrosis, germplasm envelopes | Laboratory bench logs |
| `fathers_tapes` | Intimate, restrained, practical | Saturday train, bicycle pump, blue porcelain cup, window latches | Domestic messages left for child |
| `dam_keeper_log` | Heavy industrial, mechanical, stubborn | Spillway head, islanding, turbine cavitation, manual governor | Hydro control room log |
| `teachers_recordings` | Gentle, orderly, formal | Arithmetic slate, geography chalk, attendance register, lunch tins | Empty classroom lessons |
| `quarantine_tapes` | Analytical, cautious, troubled | Incubation window, titer count, airlock gowning, exclusion band | Clinical isolation dictation |
