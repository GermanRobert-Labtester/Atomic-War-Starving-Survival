# Port Contract Gate — Quarantine Ticket

**Audit:** local post-PR #36 issue **#47** (PortContract half)
**Date:** 2026-09-05
**Status:** Remain Compile-Remove (`Tooling/PortContractGateTests.cs`)

## Evidence from dequarantine attempt

- Unclassified seams: `DiseaseSystem.RegisterStrain`, `PathogenStrainSystem.BindEngineHooks`.
- Stale `HOST_REQUIRED` without `src/` callers: `DoseLedgerSystem.ConfigureLadder`, `TradeSpecialtySystem.BindToCrafting`.

## Activation criteria (PC-01)

1. Refresh `docs/ci/port_contract_policy.json` against current Core public Bind/Wire/Register/Configure seams.
2. Reclassify missing callers as `DEFERRED` / `LIVE_VIA_CORE` / wire them in host.
3. `dotnet test --filter PortContractGate` green → remove Compile Remove.
4. Close this ticket.

**Owner lane:** Architecture / CI gates
**Expiry review:** 2026-Q4
