# Plan 140 — Advanced Hydraulic Extrusion: Closeout

**Status:** COMPLETE · **Date:** 2026-09-12 · **Batch:** `PLANS-138-139-141-LATER-PHASES`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Foundry/HydraulicExtrusionEngine.cs` (`hydraulic_extrusion`) |
| Data | `Assets/StreamingAssets/Data/hydraulic_extrusion_catalog.json` (3 products, 3 machines, 3 billets) |
| Host | `src/Host/HydraulicExtrusionHostSession.cs`, `src/Host/HydraulicExtrusionSaveStore.cs`, `src/Main.HydraulicExtrusion.cs` |
| Save | `SaveSectionRegistry` row (`foundry`) + `hydraulic_extrusion_save.json` |
| UI | `src/UI/HydraulicExtrusionPanel.cs`, route `hydraulic_extrusion` (Expanded) |
| Tests | `Plan140HydraulicExtrusionTests` 14/14; `Plan140HydraulicExtrusionHostWiringTests` 3/3 |

## Contract guarantees

- **Foundry/industrial production remains canonical.** This is an advanced extrusion surface; inventory owns produced stock.
- **Real inputs.** Energy and cooling availability are read from the live power grid and water authority; below requirement the batch is refused (`power_unavailable` / `cooling_unavailable`).
- **Deterministic quality and defects.** Abstract phases (`billet_conditioning` → `qa`); output class `rejected` / `utility` / `high_pressure` / `premium`. A minimum defect floor means pristine tooling is never immune.
- **Tooling wear is real.** Each completion consumes tool/die condition; worn tooling lowers quality.
- **Bounded downstream benefit.** `ReliabilityBenefitBp` max 1500 bp and strictly below 10000 — utilities improve but never reach 100% reliability.
- **No heavy-industrial operating procedure** is encoded; catalog values are abstract.

## Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionTests.cs` (14/14)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionHostWiringTests.cs` (3/3)
- `dotnet build Ashfall.csproj` 0 warnings / 0 errors
- `bash scripts/ci/generate-architecture-map.sh --check` (180 subsystems, 100%)
- `godot --headless --path . -- --plans-139-141-selftest` PASS (18/18 consolidated)
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` PASS
- `godot --headless --path . -- --ui-accessibility-selftest` PASS (5/5)

## Non-goals preserved

No isolated manufacturing economy, no second inventory, no 100% utility reliability, no real force/temperature recipe.
