# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# EN-04 Rehabilitation Medicine Slate Host Integration

**STATUS: APPROVED BY USER** (user authorized all previously-gated integrations, 2026-09-26)
**Package:** `EN-04-REHABILITATION-SLATE-SURFACE`
**Gate authority:** `Ashfall.Core.Medical.RehabilitationSlateProjection` (previously 0 `src/` references)

## Bounded outcome

Give the signed EN-04 read model an operational host surface. It renders a survivor's prosthetic
count, rehab phase, quality ramp, next milestone, and phantom-pain state from `SurvivorBodyState` and
its `RehabRecord` without mutating state. Pure projection; no save, no duplicate authority.

## Delivered

- New `src/Host/HostCli.RehabilitationSlate.cs` — 8-check probe `RehabilitationSlateSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `RehabilitationSlateSelfTest` + descriptor
  `--rehabilitation-slate-selftest` (alias `--prosthetics-slate-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

`godot --headless -- --rehabilitation-slate-selftest` → 8/8 (null/intact none-phase; fitting 50% +
milestone + phantom pain; adaptation 75% + remaining days; mastery permanent; dual-condition count;
constructor clamps; unknown-phase Stable fallback). Build 0 errors; parity 4/4.

## Non-goals

No change to `RehabilitationSlateProjection`; no change to `SurvivorBodyState`/`RehabRecord`
ownership; no medical-ward panel edit (the projection is the read authority, exercised through CLI).
