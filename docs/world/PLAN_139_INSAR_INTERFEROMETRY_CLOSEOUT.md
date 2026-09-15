# Plan 139 — InSAR Ground-Deformation Intelligence: Closeout

**Status:** COMPLETE · **Date:** 2026-09-12 · **Batch:** `PLANS-138-139-141-LATER-PHASES`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/World/InSarDeformationEngine.cs` (`insar_deformation`) |
| Data | `Assets/StreamingAssets/Data/insar_geodesy_catalog.json` (4 sensor profiles) |
| Host | `src/Host/InSarMappingHostSession.cs`, `src/Host/InSarMappingSaveStore.cs`, `src/Main.InSarMapping.cs` |
| Save | `SaveSectionRegistry` row (`world`) + `insar_deformation_save.json` |
| UI | `src/UI/InSarMappingPanel.cs`, route `insar_mapping` (Expanded) |
| Tests | `Plan139InSarDeformationTests` 17/17; `Plan139InSarHostWiringTests` 3/3 |

## Contract guarantees

- **Intelligence, not terrain truth.** The engine never mutates `WastelandMapSystem` or geology; it projects trend and uncertainty for excavation/travel consumers.
- **Repeat-pass required.** A single pass is refused (`insufficient_survey_passes`); mixed reference geometry is refused (`pass_geometry_incompatible`).
- **No quake prediction.** Classification is `stable` / `slow_subsidence` / `accelerating_subsidence` / `abrupt_deformation` / `low_confidence`; warnings express risk, never hidden timing.
- **Honest uncertainty.** Below minimum coherence the result is explicitly `low_confidence`; travel risk is `unknown`, never "safe".
- **Weather + terrain decorrelation** feed coherence from the live `WeatherSystem` and high-danger map nodes; processing skill improves confidence, bounded 0.05–0.97.
- **Deterministic.** Forked `insar_deformation` campaign stream; same observations → same map with or without injected RNG.

## Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan139InSarDeformationTests.cs` (17/17)
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan139InSarHostWiringTests.cs` (3/3)
- `dotnet build Ashfall.csproj` 0 warnings / 0 errors
- `bash scripts/ci/generate-architecture-map.sh --check` (180 subsystems, 100%)
- `godot --headless --path . -- --plans-139-141-selftest` PASS (18/18 consolidated)
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` PASS
- `godot --headless --path . -- --ui-accessibility-selftest` PASS (5/5)
- `python3 scripts/ci/scene-lint.py` 30/0

## Non-goals preserved

No station/aircraft runtime, no second geological truth, no UI-side phase maths, no deterministic earthquake schedule.
