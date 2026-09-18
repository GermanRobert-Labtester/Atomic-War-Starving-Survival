# Port Contract Gate — Quarantine Ticket

**Audit:** local post-PR #36 issue **#47** (PortContract half)
**Date:** 2026-09-05
**Status:** CLOSED / RESOLVED (2026-09-18)
**Resolution:** Plan 36A/36B executed. `Tooling/PortContractGateTests.cs` dequarantined and 8/8 passing.

## Evidence from dequarantine attempt (Resolved)

- Unclassified seams: `DiseaseSystem.RegisterStrain` classified as `TEST_ONLY`, `PathogenStrainSystem.BindEngineHooks` classified as `HOST_REQUIRED`. All 248 active Core public Bind/Wire/Register/Configure seams inventoried and classified.
- Stale `HOST_REQUIRED` without `src/` callers: `DoseLedgerSystem.ConfigureLadder` and `TradeSpecialtySystem.BindToCrafting` removed from policy (methods no longer exist in Core).

## Activation criteria (PC-01) — All Complete

1. Refresh `docs/ci/port_contract_policy.json` against current Core public Bind/Wire/Register/Configure seams. (COMPLETE: 248 seams tracked: 176 `HOST_REQUIRED`, 34 `LIVE_VIA_CORE`, 21 `TEST_ONLY`, 17 `DEFERRED`)
2. Reclassify missing callers as `DEFERRED` / `LIVE_VIA_CORE` / wire them in host. (COMPLETE: All 176 `HOST_REQUIRED` verified with active callers in `src/`; `DEFERRED` tracked with shrink-only baseline)
3. `dotnet test --filter PortContractGate` green → remove Compile Remove. (COMPLETE: 8/8 pass; `<Compile Remove="Tooling/PortContractGateTests.cs" />` removed from `Ashfall.Core.Tests.csproj`)
4. Close this ticket. (CLOSED 2026-09-18)

**Owner lane:** Architecture / CI gates
**Resolution commit / wave:** Wave 11 Part 2 Plan 36A/36B execution (2026-09-18)
