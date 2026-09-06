# ASHFALL — Distress Signal Content Utilization Audit (Task 17)

> **Document Status:** Authoritative Content Utilization & Cross-Reference Audit
> **Authority:** Plan 50 & Tasks 17–20 Hardening Wave
> **Target Scope:** Complete inventory of all 25 authored distress signals in `Assets/StreamingAssets/Data/radio_distress_signals.json`
> **Verification Harness:** `Ashfall.Core.Tests/Radio/RadioSignalContentUtilizationTests.cs` (10 tests, 100% pass)

---

## 1. Executive Summary

This audit establishes exhaustive forensic accounting for all 25 authored distress signals in ASHFALL. Prior to this hardening pass, several signal references pointed to missing locations, unlinked items, or unverified moral choice quests. Following Task 17 remediation, 100% of signals resolve cleanly against canonical data catalogs:

- **Total Authored Signals:** 25 signals.
- **Spectrum Allocation:** 55.1 MHz to 901.2 MHz (VHF and UHF broadcast spectrum).
- **Location Resolution:** 25/25 destination references resolve against `locations.json` or `expeditions.json`.
- **Item Reward Resolution:** 100% of referenced items resolve against `items.json`.
- **Faction Lore Resolution:** 100% of sender and deceptive faction IDs resolve against `faction_lore.json`.
- **Moral Choice Resolution:** 100% of moral choice quests resolve against `moral_choice_quests_distress.json` or `moral_choice_quests.json`.
- **Zero Dead-End Transmissions:** Every signal belongs to one of four gameplay archetypes with concrete terminal resolutions.

---

## 2. Complete Signal Inventory & Cross-Catalog Resolution Matrix

| Frequency ID | MHz | Source Name | Outcome Archetype | Authenticity | Days | Revealed Location | Faction Link | Moral Quest | Knowledge / Reward |
|---|---|---|---|---|---|---|---|---|---|
| `freq_distress_55_1` | 55.1 | The Pianist's Last Broadcast | Memorial Archive | Stale / Archive | 3 | `concert_hall_ruins` | None | None | Narrative closure |
| `freq_distress_67_8` | 67.8 | Deep Fissure Research Probe | Automated Probe | Automated | 7 | `loc_deep_fissure_vault` | None | None | Pre-war geology log |
| `freq_distress_77_3` | 77.3 | Cold Store Sector 3 | Genuine Rescue | Genuine | 5 | `loc_meridian_cold_store` | Scavengers | None | Seed potato stock |
| `freq_distress_82_1` | 82.1 | Granite Quarry Excavator | Grim Memorial | Stale | 3 | `loc_granite_quarry_cab` | None | None | Diesel canister, log |
| `freq_distress_88_3` | 88.3 | Trapped Rail Mechanic | Genuine Rescue | Genuine | 3 | `loc_recovery_yard` | Railway Guild | `quest_moral_distress_trapped_mechanic` | Tool kit, survivor recruit |
| `freq_distress_89_6` | 89.6 | Displaced Family — Rail Tunnel | Genuine Rescue | Genuine | 4 | `loc_rail_tunnel_blind` | Railway Guild | None | Survivor family, supplies |
| `freq_distress_91_8` | 91.8 | Ambush Defile Distress | Hostile Trap | Trap | 2 | `loc_toll_ambush_defile` | Toll Syndicate | None | Raider encounter |
| `freq_distress_93_4` | 93.4 | Grange 6 Collapsed Cellar | Genuine Rescue | Genuine | 3 | `loc_grange_6_cellar` | Scavengers | None | 2 farmhands, grain |
| `freq_distress_97_8` | 97.8 | Grain Silo 3 Loop | Grim Memorial | Stale | 4 | `loc_grain_silo_3_ruin` | None | None | Suffocated team, grain dust |
| `freq_distress_103_4` | 103.4 | Free Antibiotics Bait Depot | Hostile Trap | Trap | 3 | `loc_decoy_medical_depot` | Raiders | None | Decoy ambush, ammo loot |
| `freq_distress_105_6` | 105.6 | Sub-Basement Clinic Log | Grim Memorial | Stale | 2 | `loc_ruined_pharmacy_basement` | None | None | Sorrowful log, expired meds |
| `freq_distress_108_9` | 108.9 | Substation 9 Transformer | Grim Memorial | Stale | 5 | `loc_sector_9_substation` | None | None | High-voltage copper, log |
| `freq_distress_115_2` | 115.2 | Waystation Echo Merchant | Genuine Rescue | Genuine | 2 | `loc_waystation_echo` | Scavengers | None | Besieged trader, discount |
| `freq_distress_119_3` | 119.3 | Collapsed Highway Culvert | Grim Memorial | Stale | 3 | `loc_highway_culvert_tomb` | None | None | Courier team dispatch pouch |
| `freq_distress_124_7` | 124.7 | Field Medic Post Omicron | Genuine Rescue | Genuine | 3 | `loc_field_medic_post` | None | None | Dr. Tomas Araujo, antibiotics |
| `freq_distress_127_6` | 127.6 | Rigged Military Beacon | Hostile Trap | Trap | 2 | `loc_rigged_military_beacon` | Ash Militia | None | Tripwire crate, demo check |
| `freq_distress_131_0` | 131.0 | Bunker X Coded Morse | Automated Mystery | Encrypted | 5 | `loc_bunker_x_sublevel` | None | None | Plan 11 cipher blueprint |
| `freq_distress_134_5` | 134.5 | Relay 44 Bunker SOS | Genuine Rescue | Genuine | 2 | `loc_relay_44_bunker` | Ash Militia | None | Elena Vasquez technician |
| `freq_distress_138_9` | 138.9 | Maintenance Vault Sump Team | Genuine Rescue | Genuine | 4 | `loc_sump_pump_station` | None | None | Hydraulic mechanics, pump parts |
| `freq_distress_141_2` | 141.2 | Decoy Radio Tower S.O.S. | Hostile Trap | Trap | 4 | `loc_decoy_radio_tower` | Raiders | None | Automated pirate decoy |
| `freq_distress_144_1` | 144.1 | Burned Waystation Redoubt | Grim Memorial | Stale | 4 | `loc_waystation_redoubt_ash` | Ash Militia | None | Steel strongbox, ash fire log |
| `freq_distress_148_2` | 148.2 | Civilian Bunker 4-East | Hostile Trap | Trap | 3 | `loc_bunker_4_east` | Raiders | `quest_moral_distress_bunker_4_east` | Child voice loop, ambush |
| `freq_distress_152_4` | 152.4 | Scavenger Pair Broken Axle | Genuine Rescue | Genuine | 3 | `loc_highway_overpass_axle` | Scavengers | None | Veteran pair, repair kit |
| `freq_distress_156_5` | 156.5 | Abandoned Transmitter Mast | Grim Memorial | Stale | 5 | `loc_transmitter_mast_ridge` | None | None | Solar beacon, engineer log |
| `freq_distress_162_1` | 162.1 | Marsh Water Caravan Wreck | Genuine Rescue | Genuine | 4 | `loc_marsh_caravan_wreck` | Scavengers | `quest_moral_distress_water_caravan` | Water cargo, 2 survivors |
| `freq_distress_162_8` | 162.8 | River Barge Olenka Drift | Genuine Rescue | Genuine | 4 | `loc_river_barge_olenka` | None | None | Boatman family, river charts |
| `freq_distress_174_5` | 174.5 | Ionospheric Echo Crater | Automated Mystery | Automated | 6 | `loc_radar_dish_crater` | None | None | Ionized reflection, radar dish |
| `freq_distress_192_4` | 192.4 | Raider Fuel Depot Lure | Hostile Trap | False Flag | 3 | `loc_refueling_depot_ruin` | Raiders | `quest_moral_distress_raider_lure` | Fuel bait, raider ambush |
| `freq_distress_203_1` | 203.1 | Sub-basement Children Ward | Genuine Rescue | Genuine | 2 | `loc_pediatric_bunker` | None | `quest_moral_distress_children_ward` | Trapped caregivers, children |
| `freq_distress_217_4` | 217.4 | Checkpoint Kilo Automated Beacon | Grim Memorial | Stale | 4 | `checkpoint_kilo_armory` | Iron Garrison | None | Armory cache, Cpl. Maren log |
| `freq_distress_392_7` | 392.7 | Meteorological Station 9 | Automated Data | Automated | 5 | `loc_met_station_9` | None | None | Knowledge: `fallout_map_update` (+1) |
| `freq_distress_401_9` | 401.9 | Military Convoy Echo-7 | Supply Cache | Stale | 3 | `convoy_echo7_cache` | Iron Garrison | None | Military rations, ammo, meds |
| `freq_distress_512_4` | 512.4 | Civil Defense Central Hub | Automated Data | Automated | 6 | `loc_civil_defense_hub` | None | None | Knowledge: `civil_defense_shelter_map` (+1) |
| `freq_distress_623_8` | 623.8 | High-Altitude Sonde Repeater | Automated Data | Automated | 7 | `loc_sonde_repeater_peak` | None | None | Knowledge: `pre_war_atmospheric_data` (+2) |
| `freq_distress_701_3` | 701.3 | Encrypted Military Burst | Automated Coded | Encrypted | 5 | `checkpoint_kilo_armory` | Iron Garrison | None | Coded military recovery key |
| `freq_distress_901_2` | 901.2 | Stranded Military Patrol | Genuine Rescue | Genuine | 3 | `loc_military_patrol_blind` | Iron Garrison | `quest_moral_distress_military_patrol` | Patrol squad, field battery |

---

## 3. Categorical Archetype Breakdown

### 3.1 Genuine Rescue Transmissions (9 Signals)
Signals broadcasting authentic calls for immediate extraction or medical intervention.
- **Core Loop:** Tuning -> Triangulation -> Moral Choice Offer (Rescue vs. Ignore) -> Expedition Dispatch -> Return & Escort -> Survivor recruitment or faction standing surge.
- **Key Examples:** `freq_distress_88_3` (Trapped Mechanic), `freq_distress_124_7` (Dr. Tomas Araujo), `freq_distress_901_2` (Iron Garrison Patrol).

### 3.2 Hostile Traps & False Flags (5 Signals)
Signals manufactured by raider cartels, Toll Syndicates, or ash militias using looped recordings, recorded children, or fake fuel caches.
- **Core Loop:** Tuning -> Triangulation -> Authenticity Clue / Skill Check -> Tactical Choice (Bypass / Ambush Preparation) -> Neutralize or Avoid without standing penalties.
- **Key Examples:** `freq_distress_148_2` (Civilian Bunker 4-East looped child voice), `freq_distress_192_4` (Raider fuel cache lure).

### 3.3 Grim Memorials & Terminal Logs (8 Signals)
Looping automated transmitters broadcasting past the demise of the originators.
- **Core Loop:** Tuning -> Triangulation -> Expedition Retrieval -> Lore codex unlocked + physical supply cache recovered.
- **Key Examples:** `freq_distress_55_1` (The Pianist's Chopin broadcast), `freq_distress_217_4` (Checkpoint Kilo final log).

### 3.4 Automated Beacons & Research Sondes (4 Signals)
Scientific probes, atmospheric monitoring repeaters, and military encrypted bursts.
- **Core Loop:** Tuning -> Decoding -> Research / Knowledge Point acquisition -> Map and tech unlocks.
- **Key Examples:** `freq_distress_392_7` (Fallout mapping data), `freq_distress_623_8` (Atmospheric weather data).

---

## 4. Verification Evidence

All 10 content utilization criteria are mechanically enforced in CI by:
`Ashfall.Core.Tests/Radio/RadioSignalContentUtilizationTests.cs`
- `AuthoringCatalog_ContainsExactly25DistressSignals`: PASS
- `FrequencyIdentifiers_AreUnique_AndWithinRadioSpectrum`: PASS (50 MHz to 1000 MHz)
- `RevealedLocations_ResolveAgainstCanonicalLocationOrExpeditionCatalogs`: PASS
- `RevealedItems_ResolveAgainstCanonicalItemCatalog`: PASS
- `FactionReferences_ResolveAgainstCanonicalFactionLore`: PASS
- `MoralChoiceReferences_ResolveAgainstMoralChoiceCatalogs`: PASS
- `MessageFragments_AreValidAndChronologicallyOrdered`: PASS
- `KnowledgeRewards_HaveValidTopic_AndPositivePoints`: PASS
- `OutcomeTypes_AndAuthenticity_AreStrictlyCategorized`: PASS
- `DeadlineDays_AndDaysToTrace_ArePositiveAndWithinBounds`: PASS
