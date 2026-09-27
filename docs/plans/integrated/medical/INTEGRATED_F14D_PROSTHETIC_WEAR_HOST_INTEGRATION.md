# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# F14-D Prosthetic Condition & Wear Host Integration

**STATUS: APPROVED BY USER** (user authorized XP-06/F14, 2026-09-26)
**Package:** `F14D-PROSTHETIC-WEAR-SURFACE`
**Gate authority:** `Ashfall.Core.Medical.ProstheticConditionWearEngine` (previously 0 `src/` references)

## Bounded outcome

Give the signed F14-D engine an operational host surface. It evaluates daily prosthetic condition
wear, complexity-tier biomechanical efficiency caps, and failure risk in permille integer math, with
labor raising wear and maintenance lowering it. Pure static; no save section, no duplicate condition store.

## Delivered

- New `src/Host/HostCli.ProstheticWear.cs` — 8-check probe `ProstheticConditionWearSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `ProstheticConditionWearSelfTest` + descriptor
  `--prosthetic-wear-selftest` (alias `--prosthetic-condition-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --prosthetic-wear-selftest` → 8/8 (simple slow wear / 600 cap;
heavy advanced labor; critical failure risk; zero-condition 1000 risk; input clamping; tier caps
800/1000; labor/maintenance ordering; determinism). Parity 4/4.

## Non-goals

No change to `ProstheticConditionWearEngine`; the canonical item-condition store remains the owner —
the engine returns an immutable evaluation for it to apply; no save section.

## Gameplay integration (2026-09-27) — the surface is now consumed

The live limb authority `AmputationSystem` now consumes the engine on its daily
`TickDay`: every fitted `Prosthetic`/`Bionic` limb evaluates
`ProstheticConditionWearEngine.EvaluateDailyWear` with canonical/overridable
labour-intensity and maintenance-quality providers and writes the resulting
condition permille onto the additive `LimbState.prostheticConditionPermille`
field (persisted in the existing `amputation` save envelope). A player
`ServiceProsthetic` action restores condition, and
`OnProstheticMaintenanceNeeded` fires at the maintenance threshold. The earlier
"engine only returns an evaluation" framing is superseded: the same limb
authority applies it, so no duplicate condition store exists.

Verification: `PlanF14ProstheticCareIntegrationTests` 7/7 (wear, service,
round-trip); pre-existing `AmputationSystemTests` 7/7; host build 0 errors.