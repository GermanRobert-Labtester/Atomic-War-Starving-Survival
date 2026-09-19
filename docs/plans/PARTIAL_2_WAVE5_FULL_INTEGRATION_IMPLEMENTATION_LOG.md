# Partial Wave 5 — Plans 163 + 210 Integration Log

Date: 2026-09-19
Status: implemented and verified by the integrator

This log records the bounded integration of the next two ranked partial plans
from `PARTIAL_15_PRODUCTION_UNBLOCK_INTEGRATION_PLAN.md`.

## Plan 163 — Wasteland Cartography

`WastelandMapSystem` remains the sole owner of topology, fog state, discovery,
map provenance, and the existing `wasteland_map` save path. `CartographySystem`
now provides `ProjectCanonicalMap`, a deterministic read-only overlay that
converts canonical fog states into survey quality tiers and carries the
authoritative provenance source and traits without copying mutable map state.

`Main.GetCartographyProjection` exposes that overlay, while
`Main.RecordCartographySurvey` routes a field survey through
`WastelandMapSystem.DiscoverSurvey` and awards action XP through the shared
`SkillProgressionSystem` scavenging discipline. The campaign expedition RNG
fork is supplied for the existing stress/epiphany path. `MapPanel` renders
charted-node count, survey quality, and latest provenance from this projection;
it does not calculate map outcomes.

## Plan 210 — Survivor Personal Belongings

`PersonalBelongingsSystem` owns only survivor-to-item claim metadata, sentiment,
favorite flags, transfer history, and loss/inheritance facts. Physical stacks
remain in the canonical `Inventory`; the host rejects claims for missing items
and duplicate item-definition claims. The system is composed by
`SurvivorSocialCoordinator` and captured/restored inside the existing
`survivor_social` aggregate, avoiding a parallel save section.

Acquisition, gift, inheritance, and loss events apply attributed morale deltas
through `NeedsSystem`. `SurvivorFateSystem` is the single death trigger and
hands claims to the first deterministic living roster heir; no inventory stack
is moved or duplicated. `SurvivorDetailPanel` reads the claim projection and
shows a bounded personal-effects summary.

## Files changed

- `Assets/Ashfall.Core/Exploration/CartographySystem.cs`
- `Assets/Ashfall.Core/Survivors/PersonalBelongingsSystem.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs`
- `Ashfall.Core.Tests/Exploration/CartographySystemTests.cs`
- `Ashfall.Core.Tests/Survivors/PersonalBelongingsSystemTests.cs`
- `Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs`
- `src/Main.Plans163_210.cs`
- `src/Main.SurvivorFate.cs`
- `src/Main.SurvivorSocial.cs`
- `src/Main.PlayerSurfaces.cs`
- `src/Main.UiPanels.cs`
- `src/UI/MapPanel.cs`
- `src/UI/SurvivorDetailPanel.cs`
- `AGENTS.md`
- `docs/plans/PARTIAL_REMAINING_PLACEHOLDER_2026-09-19.md`

## Focused verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Exploration/CartographySystemTests.cs` — **8/8 passed**
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/PersonalBelongingsSystemTests.cs` — **7/7 passed**
- `bash scripts/run_test.sh Ashfall.Core.Tests/SurvivorSocialCoordinatorTests.cs` — **8/8 passed**
- `dotnet build Ashfall.csproj --no-restore` — **succeeded, 0 warnings, 0 errors**
- `git diff --check` on the wave paths — **passed**

No JSON catalog or new save section was required. Plans 219 (Documentation)
and 167 (Tunnel Network) remain as small placeholders in the linked ledger.
