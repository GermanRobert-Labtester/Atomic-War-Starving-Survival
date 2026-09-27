# Plan 253: Voluntary Register — Full Host Integration

**Status:** APPROVED BY USER
**Date:** 2026-09-26
**Claim:** `claim-quad-j-uvcorona-voluntaryregister-worldevolution-2026-09-26`

## Scope

Integrate `VoluntaryRegisterSystem` (148 lines) as a full host feature:
- Core authority exists (stateful system for high-dose volunteer work)
- Has state persistence (CaptureState/RestoreState)
- No catalog (pure state management)
- Needs: host session + save store + day owner + CLI probe + tests

## Implementation

1. Create `src/Host/VoluntaryRegisterHostSession.cs`
2. Create `src/Host/VoluntaryRegisterSaveStore.cs`
3. Create `src/Host/HostCli.VoluntaryRegister.cs`
4. Wire into new `src/Main.VoluntaryRegister.cs`
5. Register save section `voluntary_register`
6. Add day owner (phase 5)
7. Add CLI probe `--voluntary-register-selftest`
8. Create `Ashfall.Core.Tests/Survivors/PlanVoluntaryRegisterTests.cs`
9. Verify and archive

## Verification

- Host build: 0 errors
- Probe: 10+ checks
- Tests: 5+ focused cases
- Adjacent gates: green

## Archival

Move to `docs/plans/integrated/survivors/INTEGRATED_PLAN_VOLUNTARY_REGISTER.md`
