# Bunker Court Discovery Matrix & Manifest Registration

**Document ID:** ARCH-BUNKER-COURT-DISCOVERY-MATRIX
**Status:** Approved Architectural Specification
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-09

---

## 1. Discovery Architecture Overview

Court records in ASHFALL are not dumped into the player's journal immediately. They are discovered gradually as the player explores, interacts with bunker terminals, and uncovers historical data archives.

The narrative discovery pipeline is governed by:
- **Manifest:** `Assets/StreamingAssets/Data/narrative_discovery_manifest.json`
- **Adapter:** `BunkerCourtSourceAdapter` in `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs`
- **Knowledge State:** `JournalSystem.UnlockNarrativeDiscovered(discoveryId)` -> `narrative_disc_{discovery_id}` key in `KnowledgeBase`.
- **UI Surface:** `JournalCodex` -> `JournalPanel` (under category "Council Meeting Minutes & Verdicts").

---

## 2. Channel and Producer Specification

All 24 bunker tribunal cases use the following standardized provenance:
- **Source Catalog:** `narrative/bunker_court_verdicts_codex.json`
- **Channel:** `library_terminal` (accessing bunker historical data terminals / archives)
- **Producer ID:** `government_bunker` (the primary historical bunker complex registered in `locations.json`)
- **One Time:** `true` (once unlocked, record is permanently available in codex)
- **Weight:** `1`

---

## 3. Complete 24-Entry Discovery Matrix

| Case ID | Discovery ID (`discovery_id`) | Channel | Producer ID | Min Campaign Day (`min_day`) | One Time |
|---|---|---|---|---|---|
| `case_01_the_air_duct_moonshine_still` | `disc_court_moonshine_still` | `library_terminal` | `government_bunker` | 3 | true |
| `case_02_the_counterfeit_chore_chit_ring` | `disc_court_chore_chit_forgery` | `library_terminal` | `government_bunker` | 3 | true |
| `case_03_unauthorized_curfew_accordion_recital` | `disc_court_accordion_recital` | `library_terminal` | `government_bunker` | 3 | true |
| `case_04_the_smuggled_airlock_calico_cat` | `disc_court_smuggled_calico_cat` | `library_terminal` | `government_bunker` | 3 | true |
| `case_05_canteen_soup_ladle_favoritism` | `disc_court_ladle_favoritism` | `library_terminal` | `government_bunker` | 4 | true |
| `case_06_unauthorized_periscope_radio_tinkering` | `disc_court_periscope_radio_tinkering` | `library_terminal` | `government_bunker` | 5 | true |
| `case_07_the_great_potato_tallow_candle_heist` | `disc_court_potato_candle_heist` | `library_terminal` | `government_bunker` | 6 | true |
| `case_08_unauthorized_tunnel_wall_graffiti_manifesto` | `disc_court_tunnel_graffiti_manifesto` | `library_terminal` | `government_bunker` | 7 | true |
| `case_09_water_tap_padlock_tampering` | `disc_court_water_padlock_tampering` | `library_terminal` | `government_bunker` | 8 | true |
| `case_10_the_stolen_morphine_ampoule_mystery` | `disc_court_morphine_ampoule_mystery` | `library_terminal` | `government_bunker` | 10 | true |
| `case_11_unauthorized_seed_vault_curiosity` | `disc_court_seed_vault_curiosity` | `library_terminal` | `government_bunker` | 12 | true |
| `case_12_the_contraband_playing_card_gambling_den` | `disc_court_playing_card_gambling_den` | `library_terminal` | `government_bunker` | 14 | true |
| `case_13_the_secret_corridor_mushroom_farm` | `disc_court_secret_mushroom_farm` | `library_terminal` | `government_bunker` | 16 | true |
| `case_14_the_bunk_bed_curfew_squabble` | `disc_court_bunk_bed_curfew_squabble` | `library_terminal` | `government_bunker` | 18 | true |
| `case_15_the_tampered_geiger_counter_incident` | `disc_court_tampered_geiger_counter` | `library_terminal` | `government_bunker` | 20 | true |
| `case_16_the_stolen_sugar_cube_stash` | `disc_court_stolen_sugar_cube_stash` | `library_terminal` | `government_bunker` | 22 | true |
| `case_17_the_forbidden_surface_flower_scout` | `disc_court_surface_flower_scout` | `library_terminal` | `government_bunker` | 25 | true |
| `case_18_the_loudspeaker_intercom_prank` | `disc_court_loudspeaker_intercom_prank` | `library_terminal` | `government_bunker` | 28 | true |
| `case_19_the_black_market_antibiotic_trade` | `disc_court_black_market_antibiotics` | `library_terminal` | `government_bunker` | 30 | true |
| `case_20_the_stolen_battery_acid_experiment` | `disc_court_battery_acid_experiment` | `library_terminal` | `government_bunker` | 35 | true |
| `case_21_unauthorized_funeral_libation_consumption` | `disc_court_funeral_libation_consumption` | `library_terminal` | `government_bunker` | 40 | true |
| `case_22_the_great_steam_radiator_tap_conspiracy` | `disc_court_steam_radiator_tap` | `library_terminal` | `government_bunker` | 45 | true |
| `case_23_the_secret_ham_radio_broadcast_manifesto` | `disc_court_ham_radio_broadcast_manifesto` | `library_terminal` | `government_bunker` | 50 | true |
| `case_24_the_ratification_of_the_century_constitution` | `disc_court_century_constitution_ratification` | `library_terminal` | `government_bunker` | 60 | true |

---

## 4. Progressive Pacing Rationale

- **Cases 1–4 (Day 3):** Available during the initial shelter orientation. They establish the quirky, human baseline of bunker life (moonshine, chore token forgery, late-night accordion music, and a smuggled kitten).
- **Cases 5–9 (Days 4–8):** Early resource and equity challenges (soup ladling disputes, contraband candles, radio tinkering, bath padlock picking).
- **Cases 10–14 (Days 10–18):** Mid-early community developments (morphine audits, heirloom seed photography, boiler poker dens, illegal mushroom farming).
- **Cases 15–18 (Days 20–28):** Mid-game moral and physical dilemmas (Geiger counter tampering, stolen sugar cubes, surface dandelion retrieval).
- **Cases 19–23 (Days 30–50):** Late-game external relations (counterfeit antibiotic traders from the outside, radio contact with distant shelters).
- **Case 24 (Day 60):** Climactic historical epilogue unlocked as the player's long-surviving colony reaches stability.
