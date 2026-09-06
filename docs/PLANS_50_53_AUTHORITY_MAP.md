# ASHFALL Plans 50–53 Master Authority Map
## Overland Vehicle Garage · Faction Espionage · Survivor Mental Health · Subterranean Acoustic Direction

**Status:** Implementation authority map
**Scope:** Plans 50, 51, 52, 53
**Target:** `Ashfall.Core` simulation, data catalogs, persistence, host projection, Godot audio & presentation

---

## 1. Architectural Invariants Matrix

| Subsystem | Authority Invariant | Data Catalog (Authority) | Core Simulation System | Save Store & Section | Presentation Node |
|---|---|---|---|---|---|
| **Plan 50: Vehicle Garage** | Invariant 1 (Zero Engine References) & Invariant 6 (JSON Authority) | `vehicle_modifications.json` | `VehicleGarageSystem` (decorates & extends `ExpeditionVehicleSystem`) | `VehicleGarageSaveStore` (`vehicle_garage`) | `GarageDetailPanel` |
| **Plan 51: Faction Espionage** | Invariant 4 (Deterministic PRNG) & Invariant 3 (Checksummed Saves) | `faction_intelligence.json` | `ShelterEspionageSystem` | `ShelterEspionageSaveStore` (`faction_espionage`) | `FactionDetailPanel` & `DailyBriefingModal` |
| **Plan 52: Mental Health** | Invariant 1 (Zero Engine References) & Invariant 4 (Seeded PRNG) | `psychological_trauma.json` | `SurvivorMentalHealthSystem` | `SurvivorMentalHealthSaveStore` (`survivor_mental_health`) | `SurvivorDetailPanel` & `AfflictionsPanel` |
| **Plan 53: Acoustic Director** | Invariant 5 (Thin Presentation Projection) & Invariant 1 (Headless Safe) | `shelter_audio_cues.json` | `ShelterAcousticDirector` | Reconstructed from live state | `AudioManager` & `ShelterAudioController` |

---

## 2. Cross-System Interaction Matrix

```text
[Vehicle Travel / Expeditions]
        │
        ├── Distance & Wear ──────────────► [VehicleGarageSystem]
        │                                         │
        ├── Breakdown & Stranded ◄────────────────┘
        │          │
        │          └── Recovery Mission ──► [ExpeditionSystem]
        │
[Faction Hostility / Diplomacy]
        │
        └── Hostility Threshold ──────────► [ShelterEspionageSystem]
                                                  │
                                                  ├── Leaks & Sabotage (Abstract)
                                                  ├── Sleeper Infiltration & Interrogation
                                                  └── Dead Drops ────────► [ExpeditionSystem]
                                                                                │
[Trauma & Crises] ◄──────── Critical Casualties / Betrayal ─────────────────────┘
        │
        ▼
[SurvivorMentalHealthSystem]
        │
        ├── Stress & Trauma Tokens (Insomnia / Nightmares)
        ├── Quiet-Room Decompression (room_reading_quiet_room)
        ├── Journal Catharsis & Resilience Unlocks ──► [JournalSystem]
        └── Non-Stigmatizing Crises (Hoarding, Refusal, Panic)
        │
        ▼
[Shelter Environmental & Survival State] (Power, Radiation, Excavation Risk, Radio, Bulkheads)
        │
        ▼
[ShelterAcousticDirector] (Core Semantic Director)
        │
        ▼
[AudioManager] (Godot Audio Stream Players, Bus EQ, Mixing & Ducking)
```

---

## 3. Determinism & Save Safety Strategy

- **RNG Substreams:** Dedicated forks from `ISeededRng`:
  - `garage_breakdown` (Plan 50)
  - `shelter_espionage` (Plan 51)
  - `survivor_mental_health` (Plan 52)
  - `shelter_acoustic` (Plan 53 - zero RNG, pure state evaluation)
- **Save Isolation:** Checksummed envelopes via `SaveStoreHub` and `SchemaVersionedEnvelope<T>`.
- **Legacy Compatibility:** Absent sections in legacy saves default to safe unmodded vehicles, zero sleeper agents, baseline calm mental states, and fresh acoustic reconstruction.
