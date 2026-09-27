# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# F14-G Survivor Body Presentation Slate Host Integration

**STATUS: APPROVED BY USER** (user authorized XP-06/F14, 2026-09-26)
**Package:** `F14G-BODY-PRESENTATION-SURFACE`
**Gate authority:** `Ashfall.Core.Medical.SurvivorBodyPresentationSlate` (previously 0 `src/` references)

## Bounded outcome

Give the signed F14-G read model an operational host surface. It projects `SurvivorBodyState` into an
accessible, words-not-color limb slate: four canonical limb rows, grip capability, mobility permille,
maintenance notices, and phantom-pain alert. Pure projection; no save, no mutation, no duplicate body authority.

## Delivered

- New `src/Host/HostCli.BodyPresentation.cs` — 8-check probe `SurvivorBodyPresentationSelfTest`.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum `SurvivorBodyPresentationSelfTest` + descriptor
  `--body-presentation-selftest` (alias `--limb-presentation-selftest`).
- `src/Host/HostCli.cs` — host enum + parse + `PrintHelp` entry.
- `src/Main.Application.cs` — dispatch.

## Verification

Build 0 errors; `godot --headless -- --body-presentation-selftest` → 8/8 (intact combat-ready; single
hook Simple Only; degraded peg leg flagged; phantom pain; bilateral critical; null-body fallback;
canonical limb order; deterministic + non-mutating). Parity 4/4.

## Non-goals

No change to `SurvivorBodyPresentationSlate` or `SurvivorBodyState`; no UI panel edit (the slate is
the read authority reached through the CLI surface); no save section.

## Gameplay integration (2026-09-27) — the surface is now consumed

`AmputationSystem.BuildBodySlate` projects the live limb authority into
`SurvivorBodyPresentationSlate`, and `SurvivorDetailPanel.BodySlateProvider` is
bound in `Main.PlayerSurfaces.cs`, rendering the accessible body summary, each
non-intact limb row, and the phantom-pain alert. The earlier "no UI panel edit"
non-goal is superseded: the Survivor Detail panel is the intended consumer per
DEC-64.

Verification: `PlanF14ProstheticCareIntegrationTests` 7/7 (slate projection);
host build 0 errors.
