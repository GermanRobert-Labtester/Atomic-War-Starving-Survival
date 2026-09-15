# Plan 141 — Run-Flat Tire & Wheel Mobility: Closeout

**Status:** COMPLETE · **Date:** 2026-09-12 · **Batch:** `PLANS-138-139-141-LATER-PHASES`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Expeditions/RunFlatTireEngine.cs` (`runflat_tire`) |
| Data | `Assets/StreamingAssets/Data/runflat_tire_catalog.json` (3 profiles) + 5 install-kit items in `items.json` |
| Host | `src/Host/RunFlatTireHostSession.cs`, `src/Host/RunFlatTireSaveStore.cs`, `src/Main.RunFlatTire.cs` |
| Save | `SaveSectionRegistry` row (`expeditions`) + `runflat_tire_save.json` |
| UI | `src/UI/RunFlatTirePanel.cs`, route `runflat_tire` (Expanded) |
| Tests | `Plan141RunFlatTireTests` 18/18; `Plan141RunFlatTireHostWiringTests` 3/3 |

## Contract guarantees

- **Vehicle-component upgrade, not a new vehicle runtime.** Expedition movement stays with the expedition authority.
- **Puncture risk reduced, never eliminated.** Even the armored profile loses at least 1 integrity to every hazard; severe impacts can bend rim/bead and destroy a weakened wheel (`wheel_damaged`).
- **Real tradeoffs.** Rolling resistance costs fuel (`GetFuelPenaltyPct`), extra mass and load build heat; overheating degrades wear and lowers the safe-speed band.
- **Deterministic heat** with speed, load, ambient (from the live weather authority), and cooling when stopped.
- **Transactional install.** Workshop availability from the grid state; install kit (real `item_*` IDs) checked and consumed through the canonical `Inventory`; inventory remains the stock authority.
- **Repair is bounded** by profile repairability — never a factory reset.

## Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireTests.cs` (18/18)
- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireHostWiringTests.cs` (3/3)
- `dotnet build Ashfall.csproj` 0 warnings / 0 errors
- `bash scripts/ci/generate-architecture-map.sh --check` (180 subsystems, 100%)
- `godot --headless --path . -- --plans-139-141-selftest` PASS (18/18 consolidated)
- `godot --headless --path . -- --panel-bind-lifecycle-selftest` PASS
- `godot --headless --path . -- --ui-accessibility-selftest` PASS (5/5)

## Non-goals preserved

No tire state on steel-wheel rail vehicles, no invulnerable wheels, no real chemical/torque procedure, no second vehicle damage system.
