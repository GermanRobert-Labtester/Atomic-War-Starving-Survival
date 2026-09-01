# Radio Content Utilization Gate

> **Document Status:** Authoritative Content Utilization & Reachability Audit
> **Authority:** Plan 24 (Task 24BC)

---

## 1. Catalog Utilization Matrix

| Catalog JSON File | Scan Classification | Registered Core Consumers | Godot Presentation Surface | Reachability Status |
|---|---|---|---|---|
| `radio.json` | `GAMEPLAY_CONSUMED` | `RadioBroadcastCatalog`, `RadioTuner`, `CipherQuestChainEngine` | `RadioPanel`, `RadioHostSession` | 100% Reachable (Day 1–120 Windows & Cipher Chains) |
| `year_of_ash_radio.json` | `GAMEPLAY_CONSUMED` | `YearOfAshCatalogLoader`, `RadioScheduleCoordinator` | `RadioPanel`, `YearOfAshPanel` | 100% Reachable (Day 180–360 Triggers) |
| `verdict_radio.json` | `GAMEPLAY_CONSUMED` | `VerdictRadioSystem`, `VerdictCatalogLoader` | `RadioPanel`, `VerdictPanel` | 100% Reachable (Day 210+ Census / Reckoning) |
| `radio_distress_signals.json` | `GAMEPLAY_CONSUMED` | `RadioDistressSystem`, `SignalTriangulationSystem` | `RadioPanel` (Distress tab), `ExpeditionPanel` | 100% Reachable (26 Authoritative Distress Signals) |
| `faction_radio_corpus.json` | `GAMEPLAY_CONSUMED` | `FactionRadioEngine`, `RadioScheduleCoordinator` | `RadioPanel`, `FactionRadioHudPanel` | 100% Reachable (13 Faction Channels + Silence) |
| `faction_war_radio.json` | `GAMEPLAY_CONSUMED` | `FactionWarContentCatalogLoader`, `FactionWarSystem` | `RadioPanel`, `FactionWarPanel` | 100% Reachable (Faction War Escalation Days 480+) |
| `numbers_station_ciphers.json`| `GAMEPLAY_CONSUMED` | `SignalIntelligenceCatalog`, `CipherQuestChainEngine` | `RadioPanel`, `JournalDetailPanel` | 100% Reachable (8 Numbers Stations + Ciphers) |
| `bunker_wiretap_transcripts.json`| `GAMEPLAY_CONSUMED`| `SignalIntelligenceCatalog`, `RadioSignalLog` | `RadioPanel`, `CodexPanel` | 100% Reachable (4 Wiretaps + Expansion Wiretaps) |
| `cassette_sets.json` | `GAMEPLAY_CONSUMED` | `VinylMoraleSystem`, `RadioRecordingSystem` | `RadioPanel`, `VinylPanel` | 100% Reachable (4 Multi-part Cassette Tapes) |

---

## 2. Zero Orphan Guarantee

- Every single broadcast record maps to a valid station, frequency, and reachable day window.
- Zero orphan or dead-end distress signals exist; all 26 signals possess valid terminal outcome targets on canonical `locations.json` map nodes.
- All JSON catalogs conform strictly to `CatalogIntegrityValidator` rules with 0 errors.
