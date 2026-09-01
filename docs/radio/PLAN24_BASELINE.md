# Plan 24 — Radio, Signals & the Airwaves World: Baseline Forensic Audit

> **Document Status:** Authoritative Baseline Audit
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026
> **Target:** Radio broadcast world unification, scheduling grid, station identities, distress mission lifecycle, signal intelligence, cassette recording contract, and cross-system integration.

---

## 1. Executive Summary

ASHFALL's radio landscape prior to Plan 24 contained a rich but fragmented corpus of broadcasts, distress logs, faction chatter, and cipher puzzles distributed across several separate JSON catalogs and host sessions. While individual components—such as `RadioTuner.cs`, `FactionRadioEngine.cs`, `VerdictRadioSystem.cs`, `SignalTriangulationSystem.cs`, and `CipherQuestChainEngine.cs`—functioned correctly in isolation, there was no unified station schedule, no authoritative distress mission lifecycle with terminal outcomes, and no formal contract preventing cassette replay from duplicating live world rewards.

Plan 24 establishes a single authoritative broadcast scheduling and station intelligence architecture that sits in `Assets/Ashfall.Core/Radio/`, projected into Godot via `RadioHostSession.cs`, and backed by verified data authorities.

---

## 2. Baseline Corpus Inventory

A comprehensive forensic audit of all primary broadcast catalogs reveals the following verified baseline:

| Catalog File | Authored Records | Primary Target / Content Description | Live Engine / Loader |
|---|---|---|---|
| `radio.json` | **53** | 50 civilian/military/emergency/survivor broadcasts + 3 cipher signals (`relay_count`, `winter_ledger`, `last_rotation`) | `RadioTuner`, `CipherQuestChainEngine` |
| `year_of_ash_radio.json` | **50** | Faction communiques, icebreaker alerts, martial edicts, Vitrified Crater sermons, Allotment trade calls | `YearOfAshCatalogLoader`, `YearOfAshSystem` |
| `verdict_radio.json` | **13** | Machine registers, census carriers, tribunal summons, witness accounts | `VerdictCatalogLoader`, `VerdictRadioSystem` |
| `radio_distress_signals.json` | **5** | Multi-fragment traceable distress signals (Checkpoint Kilo, Bunker 4-East, Sector 9, Relay 44, Flotilla) | `SignalTriangulationSystem` |
| `faction_war_radio.json` | **25** | Sector 4 escalation bulletins, continuity rebuttals, toll syndicate alerts | `FactionWarContentCatalogLoader` |
| `faction_radio_corpus.json` | **13 channels** | 13 faction channels with intercept chatter, parley resolutions, raid warnings, trade reactions + 12 silence events | `FactionRadioEngine` |
| `narrative/numbers_station_ciphers.json` | **8** | Grounded numbers stations, chime interval beacons, and tape loops | `SignalIntelligenceCatalog` |
| `narrative/radio_scriptbook.json` | **15** | Deep Vault Station 0, Tempest Directorate, Old World archives | `RadioScriptbookCatalog` |
| `narrative/radio_broadcast_rundowns.json` | **30 rundowns** | Scripted studio rundowns for bunker morning/evening bulletins | Narrative archive |
| `narrative/bunker_wiretap_transcripts.json` | **8** | Faction wiretap intercepts (Garrison, Hydro-Barons, Cult) | `SignalIntelligenceCatalog` |
| `cassette_sets.json` | **4 multi-part sets** | Checkpoint Kilo, Saint Maren, Deep Vault, Flotilla audio logs | `VinylMoraleSystem` |

**Total Primary Broadcast Records:** 118 base broadcasts across the 4 core sets + 25 Faction War + 8 Numbers Stations + 15 Scriptbook + 13 Faction corpus channels.

---

## 3. Architecture Placement & Live Systems

```text
                               +---------------------------------------+
                               |     Assets/Ashfall.Core/Radio/        |
                               |  - RadioTuner.cs                      |
                               |  - FactionRadioEngine.cs              |
                               |  - SignalTriangulationSystem.cs       |
                               |  - RadioScheduleCoordinator.cs (NEW)  |
                               |  - RadioDistressSystem.cs (NEW)       |
                               |  - RadioRecordingSystem.cs (NEW)      |
                               |  - RadioSignalLog.cs (NEW)            |
                               +-------------------+-------------------+
                                                   |
                   +-------------------------------+-------------------------------+
                   |                               |                               |
+------------------v-----------------+ +-----------v-----------+ +-----------------v-----------------+
|     Cross-System Feeds (Core)      | |   Data Authority      | |     Godot Presentation (src/)     |
| - WeatherSystem (Plan 19)          | | - radio.json          | | - RadioHostSession.cs             |
| - DiseaseSystem (Plan 09)          | | - year_of_ash_radio   | | - RadioSaveStore.cs               |
| - VerdictRadioSystem (Plan 15)     | | - verdict_radio.json  | | - RadioPanel.cs (Dashboard UI)    |
| - FactionWarSystem (Plan 21)       | | - radio_distress      | | - AudioManager (Audio hooks)      |
| - CipherQuestChainEngine (Plan 11) | | - faction_corpus      | |                                   |
+------------------------------------+ +-----------------------+ +-----------------------------------+
```

---

## 4. Key Gaps Identified in Baseline Audit

1. **Scheduling Fragmentation:** Faction broadcasts, Year of Ash broadcasts, and base civilian broadcasts used separate retrieval loops without a single coherent frequency/time tuning grid.
2. **Dead-End Distress Calls:** `radio_distress_signals.json` had 5 authored signals with multi-day clarity fragments, but lacked a terminal mission lifecycle integrating directly into the expedition/survivor/standing engine.
3. **Recording Safety Contract:** Cassette recordings could potentially re-fire one-shot live world flags or rewards if replayed unless strictly decoupled from the live simulation triggers.
4. **Frequency Collisions:** Several independent catalogs shared frequencies (e.g. 88.4 MHz, 88.5 MHz, 104.2 MHz, 142.85 MHz) without authored contextual layering (e.g. distinguishing scheduled time windows vs intentional hijacking/interference).
5. **Propaganda vs Ground Truth:** Unverified faction claims needed strict metadata classification to prevent broadcast text from mutating authoritative campaign flags without corroborating evidence.
6. **Station Continuity & Silence:** Broadcasters did not reflect major world milestones (e.g. faction collapse, strikes, weather disasters, orbital harrow events) with authentic state changes or meaningful carrier silence.

---

## 5. Non-Negotiable Integration Rules

1. **Engine Authority:** `RadioTuner.cs`, `FactionRadioEngine.cs`, `VerdictRadioSystem.cs`, `SignalTriangulationSystem.cs`, and `CipherQuestChainEngine.cs` remain authoritative in `Assets/Ashfall.Core/`.
2. **Determinism:** Tuning resolution, schedule calculation, and distress candidate derivation use `ISeededRng` and deterministic day/tick integer arithmetic—never `System.Random`, `Guid.NewGuid()`, or wall-clock timing.
3. **Save Compatibility:** All new radio state fields (presets, active distress lifecycle, recorded cassette index, signal log) serialize within checksummed envelopes and gracefully default on legacy saves.
4. **Data Authority:** All content is authored in snake_case JSON under `Assets/StreamingAssets/Data/` with valid `schema_version` and strict `CatalogIntegrityValidator` compliance.
5. **Audio Decoupling:** Spoken-word and SFX hooks are prepared as optional metadata references; complete textual transcripts and accessibility visual cues remain 100% playable with zero audio files attached.
