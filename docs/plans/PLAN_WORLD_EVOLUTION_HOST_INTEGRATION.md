# Plan 227: World Evolution — Full Host Integration

**Status:** APPROVED BY USER
**Date:** 2026-09-26
**Claim:** `claim-quad-j-uvcorona-voluntaryregister-worldevolution-2026-09-26`

## Scope

Integrate `WorldEvolutionEngine` (317 lines) as a full host feature:
- Core authority exists (stateful engine with event catalog)
- Has state persistence (CaptureState/RestoreState)
- Has catalog loader
- Needs: host session + save store + day owner + CLI probe + tests

## Implementation

1. Create `src/Host/WorldEvolutionHostSession.cs`
2. Create `src/Host/WorldEvolutionSaveStore.cs`
3. Create `src/Host/HostCli.WorldEvolution.cs`
4. Wire into new `src/Main.WorldEvolution.cs`
5. Register save section `world_evolution`
6. Add day owner (phase 5)
7. Add CLI probe `--world-evolution-selftest`
8. Create `Ashfall.Core.Tests/World/PlanWorldEvolutionTests.cs`
9. Verify and archive

## Verification

- Host build: 0 errors
- Probe: 10+ checks
- Tests: 5+ focused cases
- Adjacent gates: green

## Archival

Move to `docs/plans/integrated/world/INTEGRATED_PLAN_WORLD_EVOLUTION.md`
