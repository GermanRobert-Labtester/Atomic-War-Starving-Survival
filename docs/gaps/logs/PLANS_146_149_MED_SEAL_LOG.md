# Plans 146–149 MED Seal Log

**Claim:** `PLANS-146-149-MED-SEAL`  
**Date:** 2026-09-12  
**Status:** MED trio sealed; debt/portfolio intentionally blocked

## Scope sealed

1. **Live Start travel ticks** — `ExpeditionTravelStretch` projects `distanceTicks` for Estimate, StartExpedition, and DispatchSortie from the same `SetEstimateRouteModifiers` travel multiplier. Registry catalog entries are never mutated.
2. **Coated-part → PowerGrid** — install-required contract: mint does not auto-buff. `TryInstallCoatedPart` / uninstall / restore republish under source `ebpvd_installed` (cap 3 families / 80 W). Host action `install_coated_part` consumes inventory then installs; Eb-PVD completed tab exposes install buttons.
3. **Assay patient picker** — panel emits `assayId|patientId`; defaults to first living survivor via `SetPatientCandidates` / `ResolvePlans146OperatorId`.

## Explicitly out of scope (authority missing)

| Item | Ledger | Why blocked |
|---|---|---|
| DEBT-PLAN167 consequence routing | `KNOWN_DEBT.md` ACCEPTED | Needs written mapping of each consequence → canonical consumer, idempotency, save, cross-system test |
| DEBT-PLAN168 water delivery | `KNOWN_DEBT.md` ACCEPTED | Needs authority map + no-double-ledger transfer contract |
| DEBT-PLANS166-169 reload/replay | `KNOWN_DEBT.md` ACCEPTED | Needs bounded replay fixture package |
| Plans 150–169 / 170–199 portfolio | docs-only / ACCEPTED | Promotion requires foreman package with claimed paths |
| Wave_17 IDs 146–149 | name collision only | Not the industrial Plans 146–149 flagship set |

## Files

- `Assets/Ashfall.Core/Expeditions/ExpeditionTravelStretch.cs` (new)
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`
- `src/Host/ExpeditionHostSession.cs`
- `src/Host/PowerGridHostSession.cs`
- `src/Main.Plans146_149.cs`
- `src/UI/EbPvdCoatingPanel.cs`
- `src/UI/MicrofluidicDiagnosticPanel.cs`
- `Ashfall.Core.Tests/Expeditions/ExpeditionTravelStretchTests.cs` (new)
- `Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs`
- `Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs`
- `WORKTREE_OWNERSHIP.md`

## Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/ExpeditionTravelStretchTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Integration/Plans146_149IntegrationTests.cs
dotnet build Ashfall.csproj -v q
```

**Results (2026-09-12):**
- `ExpeditionTravelStretchTests` — 4/4 passed
- `PowerGridSystemTests` — 20/20 passed
- `Plans146_149IntegrationTests` — 8/8 passed
- `dotnet build Ashfall.csproj` — 0 warning(s), 0 error(s)
