# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan 227: World Evolution — Full Host Integration

**Status:** APPROVED BY USER — **FULLY INTEGRATED**
**Date:** 2026-09-26
**Claim:** `claim-triple-j-voluntaryregister-worldevolution-2026-09-26`

## Scope

`WorldEvolutionEngine` (317 lines) was a stateful Core authority with
`CaptureState`/`RestoreState` and **no campaign wiring** — it was reachable only
from the `--world-exploration-selftest` CLI probe. It reads the authored
`world_evolution_events.json` catalog and applies world-state events (day
threshold + optional required flag + map target effects).

## What was integrated

- **Host session** — `src/Host/WorldEvolutionHostSession.cs`
  - `WorldEvolutionHostSession` (loads the authored catalog through the engine's
    own `dataDir` constructor; ticks through `TickDay`)
  - `WorldEvolutionSaveStore` (`SaveStoreHub.Checksummed<WorldEvolutionState>`)
- **Host wiring** — `src/Main.WorldEvolution.cs`
  - `SetupWorldEvolution` / `SaveWorldEvolution` / `TickWorldEvolution`
  - `FlushWorldEvolutionIfDirty` / `ResetWorldEvolution`
  - `ActiveWorldFlags()` derived read model (returns an empty set; a missing flag
    is a closed gate, so no event fires on an unowned fact)
- **Day owner** — `src/Main.CampaignOwners.cs`
  - `WorldEvolutionDayOwner` (ownerId `world_evolution`, phase 5) with
    `IPreDaySnapshotRestore` pre-day snapshot rollback
  - emits `world_evolution_ticked` heartbeat
- **Registry**
  - `SaveSectionRegistry`: section `world_evolution`, `world_evolution_save.json`
  - `DayEventVocabulary`: `world_evolution_ticked` → `SemanticKind.Heartbeat`
    (the key already existed from an earlier lane; the duplicate was removed)
  - `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`: heartbeat row
  - `scripts/ci/generate-architecture-map.py`: graph entry
- **Lifecycle** — `src/Main.SaveOrchestrator.cs` (setup + save),
  `src/Main.Lifecycle.cs` (reset)
- **CLI probe** — `--world-evolution-selftest` / `--evolution-events-selftest`
  - registered in `Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Host/HostCli.cs`,
    dispatched in `src/Main.Application.cs`, implemented in
    `src/Host/HostCli.WorldEvolution.cs`
- **Tests** — `Ashfall.Core.Tests/PlanTriplePackageJCoreTests.cs` (consolidated suite)

## Rule-5 authority boundary

The engine is the single authority for world-state evolution events. The host
passes the **authoritative** `_world.WastelandMap` in at tick time and keeps no
second map graph and no parallel event ledger. `LocationEvolutionSystem`,
`LandmarkDegradationSystem` and `WildlifeMigrationSystem` are passed `null`
because their own owners are not campaign-bound; the engine handles that
explicitly rather than the host inventing a collaborator.

## Contract findings pinned by tests/probe

1. **Flag gating is real.** The earliest authored event
   (`event_evolution_surge_harbor_overrun`, day 0) requires
   `dc8_surge_began`. An empty flag set must NOT fire it. The first *ungated*
   authored day is 12. (My first probe asserted the wrong thing and was fixed.)
2. **`RestoreState(null)` is a documented no-op**, not a reset. `Reset()` must
   restore an empty `WorldEvolutionState` to clear the triggered set. The Core
   semantics were left unchanged (Rule 10); the host was corrected.
3. **Later days may fire additional *distinct* authored events.** The real
   invariant is same-day idempotence plus "a triggered event is never removed and
   never re-runs", not "the count never grows".

## Verification

- Host build: **0 errors / 0 warnings in owned files**
- Core test build: **0 errors / 0 warnings**
- `--world-evolution-selftest`: **8/8 PASS**
- `PlanTriplePackageJCoreTests`: **9/9 PASS** (consolidated with voluntary register)
- Adjacent gates: **1929/1929 PASS**
- Generators: selftest manifest **312 tests** (310 headless), architecture map
  **314 subsystems**
