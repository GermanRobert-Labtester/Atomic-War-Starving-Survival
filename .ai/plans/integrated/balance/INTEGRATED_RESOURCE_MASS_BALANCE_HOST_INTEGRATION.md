# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Resource Mass Balance Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `RELEASE-BALANCE-MASS-MATRIX`
**Gate authority:** `Ashfall.Core.Balance.ResourceMassBalanceSimulator` (previously 0 `src/` references)

## Bounded outcome

Expose the deterministic 30-day survival-loop mass-balance simulator through the host CLI so the
release-craft balance gate is runnable. No gameplay state, no save section, no duplicate authority.

## Delivered

- New `src/Host/HostCli.MassBalance.cs` — 8-check probe `ResourceMassBalanceSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `ResourceMassBalanceSelfTest` + descriptor
  `--mass-balance-selftest` (alias `--resource-mass-balance-selftest`).
- `src/Host/HostCli.cs` — host enum + parse.
- `src/Main.Application.cs` — dispatch.

## Verification

`dotnet build Ashfall.csproj` → 0 errors; `godot --headless -- --mass-balance-selftest` → 8/8.
`HostCliActionParityGateTests` 4/4; selftest manifest + CLI catalog regenerated.

## Non-goals

No change to `ResourceMassBalanceSimulator` math, no save section, no panel, no RNG seed change.
