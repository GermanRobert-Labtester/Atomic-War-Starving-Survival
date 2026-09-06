# WASTELAND MAP PROVENANCE & DISCOVERY ARCHITECTURE
## Strategic Information Lineage, Fog Evolution, and Legacy Compatibility

**Document Version:** 1.0.0
**Scope:** Plan 73 (Wasteland Map & Strategic Cartography)
**Status:** Canonical Map Provenance Specification

---

## 1. Information Lineage Model

Every location known to the settlement carries an authoritative provenance chain:
- **How was it learned?** (`SourceKind`: `ExpeditionVisit`, `ExpeditionSurvey`, `RadioIntercept`, `TraderRumor`, `FieldGuide`)
- **What produced it?** (`ProducerSystemId`: `expedition_system`, `radio_system`, `field_guide`, etc.)
- **When was it learned?** (`DayObserved`)
- **How certain is the settlement?** (`Confidence`: `Low`, `Medium`, `High`, `Confirmed`)
- **When was it last corroborated?** (`LastConfirmedDay`)

---

## 2. Producer Integration Matrix

```text
┌─────────────────────────┐        ┌────────────────────────────┐
│   Radio Intercept       ├───────►│  DiscoverRumor(...)        │
│   (Distress / Broadcast)│        │  • Sets Fog: Rumored       │
└─────────────────────────┘        │  • Confidence: Low/Medium  │
                                   │  • Fuzzed coords, no loot  │
┌─────────────────────────┐        └─────────────┬──────────────┘
│   Cartographic Survey   ├───────►┌─────────────▼──────────────┐
│   (Radar / Seismic Tech)│        │  DiscoverSurvey(...)       │
└─────────────────────────┘        │  • Sets Fog: Surveyed      │
                                   │  • Confidence: High        │
┌─────────────────────────┐        │  • Unlocks Traits          │
│   Expedition Arrival    ├───────►└─────────────┬──────────────┘
│   (On-Site Exploration) │        ┌─────────────▼──────────────┐
└─────────────────────────┘        │  DiscoverVisited(...)      │
                                   │  • Sets Fog: Visited       │
                                   │  • Confidence: Confirmed   │
                                   │  • Full Ground Truth       │
                                   └────────────────────────────┘
```

---

## 3. Save Migration & Backward Compatibility

1. **Envelope Persistence:**
   - The map discovery state is saved in `wasteland_map_save.json` under the `wasteland_map` save section managed by `WastelandMapSaveStore`.
   - Node knowledge entries (`MapNodeKnowledgeState`) persist alongside the legacy `Discovered`, `Completed`, `Locked`, and `Unlocked` string lists.
2. **Legacy Save Invariant:**
   - When loading older saves lacking the new `Knowledge` array, `NormalizeAndValidate()` automatically initializes knowledge entries for all existing `Discovered` nodes:
     - `FogState = MapFogState.Visited`
     - `Confidence = InformationConfidence.Confirmed`
     - `SourceKind = KnowledgeSourceKind.ExpeditionVisit`
     - `DayObserved = 1`
   - Zero save corruption; zero data loss.
