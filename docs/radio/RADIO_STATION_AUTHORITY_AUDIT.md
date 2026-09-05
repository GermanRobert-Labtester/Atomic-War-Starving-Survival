# ASHFALL Radio Station Authority Audit (AF-B1 / Plan 60)

**Document ID:** RADIO-AUTH-AUDIT-01
**Date:** 2026-09-05
**Scope:** Forensic inventory of all `RadioStationCatalog` callers, sources, data authorities, save models, and UI consumers in the ASHFALL codebase.

---

## 1. Executive Summary

Historically, `RadioStationCatalog` maintained a dual-authority risk: the class constructor invoked `RegisterDefaults()`, instantiating 6 hardcoded stations in memory. Although `RegisterDefaults()` was subsequently excised from Core in favor of `radio_stations.json`, the station catalog loader was incomplete, lacks structured schedule slots and typed signal degradation reasons, and had no dedicated CLI selftest.

This audit establishes the definitive mapping of station identities, data authorities, and consumers across Core and Host layers.

---

## 2. Station Inventory & Authoritative JSON Shape

All 6 canonical stations are defined exclusively in `Assets/StreamingAssets/Data/radio_stations.json`:

| Station ID | Frequency (MHz) | Owner Faction | Reliability | Default State | Signal Profile |
|---|---|---|---|---|---|
| `station_civil_defense` | 88.50 | `faction_civil_defense` | Official | Normal | `signal_profile_omni_high_power` |
| `station_garrison_overlord` | 88.40 | `military_remnants` | Partisan | Normal | `signal_profile_directional_vhf` |
| `station_vitrified_crater` | 104.20 | `children_of_the_crater` | Partisan | Normal | `signal_profile_fallout_flutter` |
| `station_open_classroom` | 91.30 | `faction_independent_survivors` | Anonymous | Normal | `signal_profile_pirate_fm` |
| `station_numbers_sigint` | 14.487 | `faction_unknown_intelligence` | Automated | Normal | `signal_profile_pulsed_hf` |
| `station_automated_relay` | 142.85 | `faction_automated_infrastructure` | Automated | Normal | `signal_profile_beacon_vhf` |

---

## 3. Caller & Consumer Inventory

### 3.1 Core Consumers
- `Assets/Ashfall.Core/Radio/RadioStationCatalog.cs`: The single runtime catalog for station queries, state overrides, schedule slot resolution, and signal strength calculations.
- `Assets/Ashfall.Core/Radio/RadioScheduleCoordinator.cs`: Uses `RadioStationCatalog` to check station state (e.g. `Silent` or `Jammed`) and filter broadcasts.
- `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`: Cross-references station IDs for broadcast records.
- `Assets/Ashfall.Core/Radio/RadioSignalLog.cs`: Tracks player discovery of station IDs.

### 3.2 Host & Presentation Consumers
- `src/Host/RadioHostSession.cs`:
  - Instantiates `RadioStationCatalog` via `RadioStationCatalogLoader.LoadAndRegister(...)`.
  - Manages player tuning, history, and active signals.
  - Queries `Stations.GetCurrentSlot` and `Stations.GetNextSlot`.
- `src/Host/RadioSaveStore.cs`:
  - Serializes and deserializes radio save envelopes including `RadioStationState` overrides.
  - Guarantees unknown station ID preservation on round-trip.
- `src/UI/RadioPanel.cs`:
  - Renders station rows displaying `DisplayName` (never raw IDs), frequency, state, current program, next program, signal quality, and lock reason.

### 3.3 Test Consumers
- `Ashfall.Core.Tests/Radio/RadioStationCatalogTests.cs`: Verifies exact JSON parity against `RadioLegacyCatalogFixture` and ensures no hardcoded station definitions remain in Core.
- `Ashfall.Core.Tests/Radio/RadioStationParityTests.cs`: Full B1-001 through B1-020 acceptance suite.
- `Ashfall.Core.Tests/Radio/RadioScheduleCoordinatorTests.cs`: Tests scheduling interactions with station states.

---

## 4. Migration & Architecture Mandates

1. **Zero Hardcoded Station Definitions**: No station IDs or text constants may be hardcoded as default catalog data in `Assets/Ashfall.Core/`.
2. **One Loader Authority**: `RadioStationCatalogLoader` is the sole loader for `radio_stations.json`, validating schema version, duplicate IDs, frequency bounds (0.1–1000 MHz), and schedule slots.
3. **No Silent Fallback**: If `radio_stations.json` is missing or corrupt, loading must fail with an explicit exception/diagnostic rather than falling back to dummy data.
4. **Unknown ID Preservation**: Station state overrides for unknown station IDs (e.g., from expansions or future mods) must be retained during export and import cycles.
5. **Equipment vs Schedule Decoupling**: Research items gate equipment capability (tuning band, noise floor, sensitivity), NEVER the station's broadcast schedule.
