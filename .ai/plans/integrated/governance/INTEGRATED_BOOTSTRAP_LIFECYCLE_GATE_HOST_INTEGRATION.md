# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Bootstrap Lifecycle Gate Host Integration

**STATUS: APPROVED BY USER** (user authorized EN-06, 2026-09-26)
**Package:** `EN-06-ONE-BOOTSTRAP-PATH-GATE`
**Gate authority:** `Ashfall.Core.Orchestration.BootstrapLifecycleGate` (previously 0 `src/` references)

## Bounded outcome

Expose the EN-06 one-bootstrap-path lifecycle gate through the host CLI: every path mode
(FreshGame/SaveRestore/SessionReset) must reach `Ready` with zero deferred seams and no required
subsystem left in an unreached stage. Read-only; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.BootstrapLifecycle.cs` — 8-check probe `BootstrapLifecycleGateSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `BootstrapLifecycleGateSelfTest` + descriptor
  `--bootstrap-lifecycle-selftest`.
- `src/Host/HostCli.cs` — host enum + parse.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --bootstrap-lifecycle-selftest` → 8/8 (valid register; invalid stage/id refused;
non-sequential advance refused; sequential reaches Ready; parity valid; deferred seam flagged;
not-ready reports stage + unreached-required; reset clears). Build 0 errors; parity gate 4/4.

## Non-goals

No change to `BootstrapLifecycleGate` logic; no change to the live composition-root bootstrap path
(the gate is exercised as its own authority; CF-P28 host wiring remains its own record).
