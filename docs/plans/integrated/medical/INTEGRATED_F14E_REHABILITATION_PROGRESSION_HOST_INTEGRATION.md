# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# F14-E Rehabilitation Arc Progression Host Integration

**STATUS: APPROVED BY USER** (user authorized XP-06/F14, 2026-09-26)
**Package:** `F14E-REHABILITATION-PROGRESSION-SURFACE`
**Gate authority:** `Ashfall.Core.Medical.RehabilitationProgressionEngine` (previously 0 `src/` references)

## Bounded outcome

Give the signed deterministic rehabilitation engine an operational host surface. It advances a
prosthetic arc through fitting (500 permille) -> adaptation (resilience-scaled ramp) -> mastery
(permanent 1000 permille) with integer permille math and zero RNG. Pure domain; no save section.

## Delivered

- New `src/Host/HostCli.RehabilitationProgression.cs` — 8-check probe `RehabilitationProgressionSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `RehabilitationProgressionSelfTest` + descriptor
  `--rehabilitation-progression-selftest` (alias `--prosthetic-progression-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --rehabilitation-progression-selftest` → 8/8 (start fitting 0.5;
day 3 held; day 4 -> adaptation; 7-day ramp; mastery at 1000; mastery permanent; quality factors;
zero-day no-op / null starts fresh). Parity 4/4.

## Non-goals

No change to `RehabilitationProgressionEngine`; no new save store (the engine returns a new immutable
`RehabRecord` for the existing `SurvivorBodyState.Rehab` owner to hold); no RNG introduced.

## Gameplay integration (2026-09-27) — the surface is now consumed

`AmputationSystem.FitProsthetic` starts the arc via
`RehabilitationProgressionEngine.StartRehabilitation` and stores it on the
additive `LimbState.rehabPhase` / `rehabDaysInPhase` / `rehabQualityPermille`
fields; `AmputationSystem.TickDay` advances it daily with the bound resilience
provider; `BuildBodyState` projects it back into `SurvivorBodyState.Rehab`. No
new save store — the arc rides the existing `amputation` envelope.

Verification: `PlanF14ProstheticCareIntegrationTests` 7/7
(fitting → adaptation → mastery, round-trip); host build 0 errors.