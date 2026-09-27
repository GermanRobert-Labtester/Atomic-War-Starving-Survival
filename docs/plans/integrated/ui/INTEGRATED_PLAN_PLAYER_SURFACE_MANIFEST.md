# PLAN-PLAYER-SURFACE-MANIFEST — Player Surface Coverage Manifest Host Integration
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — Core authority bound · host session bound · live route/CLI seam bound · focused tests green · no parallel authority created.**

## Integrated evidence

* `PlayerSurfaceManifest` gained a runtime consumer: `GeneratePlayerSurfaceManifest()` produces the coverage manifest from the live `PanelRegistry` (222 player-navigable surfaces: all routed, all dismissible, 60 interactive + 162 read-only, 22 snapshot-covered) and exposes it through `PlayerSurfaceCoverageSummary()`. Pure projection — generating it mutates no panel and no registry state. Probe: `--player-surface-manifest-selftest` 9/9. Test coverage stays with the pre-existing `PlayerSurfaceCoverageGateTests`; a duplicate suite was removed rather than added.

## Original plan body (preserved for the record)

> **Package:** `PLAYER-SURFACE-MANIFEST`
> **Category:** ui / governance
> **Plan type:** bind an unhosted coverage read model to the live panel registry.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/UI/PlayerSurfaceManifest.cs` and
`PlayerSurfaceContract.cs` — an authored coverage read model with **zero
consumers** — to the live `PanelRegistry`, so the project can state, from real
data, how many player-navigable surfaces are routed, bindable, keyboard-closable,
snapshot-covered, and interactive versus read-only.

**Bounded outcome:** the manifest is generated from the live registry and exposed
through a host command and a probe; the accessibility/UI rules that today are only
described in prose become a measured number.

**Non-goals:** no new panel, no new route, no behaviour change, no new save
section, no relaxed gate.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan read model | `grep -rl "PlayerSurfaceManifest\|PlayerSurfaceContract" src/` → no matches; no Core runtime consumer |
| Derives from live registry | `PlayerSurfaceManifest.Generate()` calls `PanelRegistryBootstrap.RegisterAll()` and walks `PanelRegistry.AllIds`, filtering `IsPlayerNavigable` |
| **Pre-flight: it works on real data** | measured run: **222** player-navigable surfaces · routed **222** · bound **222** · keyboard-closable **222** · snapshot-covered **22** · interactive **60** · read-only **162** |
| Existing prose rules it measures | AGENTS.md UI rule: "Preserve keyboard/controller close/back behavior, focus, visible feedback … and refresh/disposal lifecycle" |
| Related live gates | `PanelCatalogCompletenessTests`, `PanelRouteGateTests`, `Main.PanelLifecycle.cs`, `OverlayPanelCatalog` |

## 3. Files

### New — Host
- `src/Host/PlayerSurfaceManifestHostSession.cs`
- `src/Host/HostCli.PlayerSurfaceManifest.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--player-surface-manifest-selftest`

### Modified — Host
- `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `src/Main.PlayerSurfaces.cs` — expose the manifest as a diagnostics readout
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/UI/PlanPlayerSurfaceManifestTests.cs` (new)

## 4. Acceptance

1. `--player-surface-manifest-selftest` ≥ 9/9: manifest generates from the live
   registry; every surface is routed; every surface is keyboard-closable (the
   historical Esc softlock class is now asserted, not hoped for);
   routed+unrouted counts sum to the total; interactive + read-only = total;
   snapshot coverage is reported honestly (not 100%); regeneration is stable.
2. Focused Core-contract suite green.
3. Read-only: generating the manifest mutates no panel and no registry state.
4. Adjacent gates green (no new save section).

## 5. Deferred with named reasons

- No raise of the snapshot-coverage floor: 22/222 is the measured truth today;
  widening golden-snapshot coverage is separate content work.
- No new diagnostics panel: exposed through the existing diagnostics route instead.
