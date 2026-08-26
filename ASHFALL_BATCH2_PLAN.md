# ASHFALL Batch 2 — Quality Implementation Plan

> Roadmap items 17–32. Dependency-ordered Godot/Core vertical slices building on Batch 1.

## Summary

Deliver roadmap items 17–32 as dependency-ordered Godot/Core vertical slices building on Batch 1. Existing Core systems and UI scaffolds are reused and upgraded from demo/fixture paths into campaign-safe flows; missing authorities are added only where no authoritative system exists.

Batch 2 remains Godot-only. Core owns simulation, state, deterministic outcomes, and save DTOs. Godot hosts input, presentation, audio, and wiring. Unity remains read-only.

---

## W0 — Shared foundation (prerequisite)

### 1. Action result and transaction boundary

New player commands expose a typed result containing:
- `Success` flag
- `FailureCode` (stable enum for UI branching)
- `MessageKey` (player-facing message)
- `Deltas` (resource/state changes)
- `EventId` (for audit/journal)

Existing string-returning host methods remain compatibility wrappers while live UI moves to typed results.

Every action must validate before mutating. Inventory consumption, equipment reservation, research progress, resource production, and reward delivery must commit atomically. Repeated commands, reloads, and modal reopenings must not duplicate rewards or consume inputs twice.

### 2. Authoritative catalogs

- Move `ResearchSystem.RegisterDefaults()` knowledge definitions into a versioned research catalog
- Make `relic_recipes.json` the single workshop relic authority
- Extend existing catalogs rather than creating duplicates
- Add new versioned catalogs only where no authority exists
- Register all references with `CatalogIntegrityValidator`

### 3. Campaign-day order

`CampaignDayCoordinator` becomes the only owner of daily progression. Explicit order:

1. Weather, orbital, brine, power
2. Ventilation, foundry, water, greenhouse, excavation, trapping, production
3. Needs, medical, disease, ration conflict, caregiving, social, generational
4. Research, expedition/vehicle, maritime, caravans, treaties, faction
5. Death/memorial, audio context, journal, daily briefing

### 4. Persistence

Every new/extended stateful system implements `CaptureState/RestoreState`, defensive deep-copy, deterministic ordering, checksum-protected persistence. Use `IJsonSerializer`, `IFileIO`, `ISeededRng`.

---

## Phase 1 — Resource, research, health, shelter

| # | Feature | Core | Host | UI |
|---|---------|------|------|----|
| 17 | Workshop reverse-engineering | `WorkshopReverseEngineeringSystem` | `WorkshopHostSession` | `ResearchAtlasPanel`, `ResearchDetailPanel`, `CraftingPanel`, `CraftingDetailPanel` |
| 19 | Water treatment | `WaterTreatmentSystem` | `WaterTreatmentHostSession` | `WaterTreatmentPanel` |
| 30 | Ventilation/scrubbing | Extend `StartingLevelSystem` | `VentilationHostSession` | Shelter ops overlay |
| 22 | Pharma lab | `PharmaLabSystem` | `PharmaLabHostSession` | `PharmaLabPanel` |
| 31 | Weather station | `WeatherStationSystem` | `WeatherStationHostSession` | `WeatherForecastPanel` |
| 32 | Multi-bus audio | Extend `AudioManager` | `AudioEventBridge` | Context-driven buses |

## Phase 2 — Maritime, mobility, orbital, excavation, hunting

| # | Feature | Core | Host | UI |
|---|---------|------|------|----|
| 18 | Maritime stealth dive | Promote `StealthDiveInstance` to production | `MaritimeHostSession` | `MaritimeAtlasPanel` |
| 23 | Expedition vehicles | `ExpeditionVehicleSystem` + vehicle catalog | `ExpeditionVehicleHostSession` | `VehicleBayPanel` |
| 24 | Orbital Harrow telemetry | Extend `OrbitalHarrowSystem` | Orbital host adapter | `OrbitalHarrowPanel` |
| 27 | Excavation/rubble clearing | `ExcavationSystem` | `ExcavationHostSession` | Shelter ops overlay |
| 28 | Wildlife hunting/trapping | `WildlifeTrappingSystem` | `WildlifeHostSession` | `HuntingTrapPanel` |

## Phase 3 — Security, social, diplomacy

| # | Feature | Core | Host | UI |
|---|---------|------|------|----|
| 20 | Airlock sentry post | `AirlockSecuritySystem` | `AirlockSecurityHostSession` | `AirlockSecurityPanel` |
| 21 | Social dynamics | `SurvivorRelationsSystem` | Relations host session | Survivor dossier, mediation modal |
| 29 | Treaty desk | `RegionalTreatySystem` | Treaty host session | `TreatyDeskPanel` |

## Phase 4 — Long-horizon continuity

| # | Feature | Core | Host | UI |
|---|---------|------|------|----|
| 25 | Generational succession | Extend `GenerationalSuccessionEngine` + `CensusClaimSystem` | Lineage host session | `CenturySeedPanel` |
| 26 | Vinyl morale | `VinylMoraleSystem` | `VinylHostSession` | `CommonRoomPanel` |

---

## Verification gate (every slice)

1. `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
2. `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
3. `dotnet build Ashfall.csproj`
4. `godot --headless --path . -- --data-integrity-selftest`
5. `godot --headless --path . -- --bridge-selftest`
