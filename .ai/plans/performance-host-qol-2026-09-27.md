# Performance, host ownership, and campaign recovery

STATUS: APPROVED BY USER

Authorization: user requested all three plans implemented without commits on
2026-09-27, then explicitly reassigned overlapping older claims to this integration.

## Acceptance

1. Record honest startup/frame and assembly measurements before/after; optimize
   only observed unnecessary UI work; preserve deterministic simulation.
2. Development selftests remain callable; release assemblies exclude test code.
3. Map Main partials to domains, replace plan filenames with truthful domain names,
   split CLI probes, and reuse the existing day coordinator in a host session.
4. Extend existing save authority with validated rolling recovery and a player
   recovery prompt; retain campaign day and restore a valid last panel.
5. Consolidate instruction authority and archive completed coordination documents
   using existing generators for generated outputs.
6. Scoped save, host, CLI, and UI checks pass. No full test run; no commit.

## Order and boundaries

Measure first; then build/UI performance, mechanical host consolidation, save and
navigation QoL. Read-only audits run concurrently. Builders own disjoint paths;
the root integrator owns shared wiring, measurements, ledgers and acceptance.
Existing dirty work is preserved. No simulation balancing, RNG changes, new save
authority, or speculative lazy catalog loading.

## Verification

Root also owns opt-in `tools/performance/frame_profile.gd` and measurements in
`artifacts/performance/three-plan-2026-09-27/`; runtime profiles use fresh isolated
XDG data roots so player campaigns are untouched.

Root also owns `tools/gotools/pkg/agentsync/agentsync.go`, existing Python agent
sync compatibility entry point, generated client instruction symlinks/report,
README.md, four root A1/WAVE9/session records and their dated archive paths,
and generated docs/INDEX.md. These are navigation/QoL implementation paths.

Use bin/run-scoped-tests with explicit test paths (at most 15 testing steps),
development and ExportRelease host builds, bounded Godot probes at 15 FPS, and
generator --check modes. Record all failures and unmeasured quantities honestly.
Mark fully integrated and archive only after all acceptance criteria are met.

## Root measurement claim extension

Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs
Assets/Ashfall.Core/Narrative/FringeCultsCatalog.cs
Assets/Ashfall.Core/Narrative/PaperMakingCatalog.cs
Assets/Ashfall.Core/Narrative/PolymerTextileCatalog.cs
Assets/Ashfall.Core/Narrative/OpticsGlassworksCatalog.cs
Assets/Ashfall.Core/Narrative/WaterTreatmentPotableCatalog.cs
Assets/Ashfall.Core/Narrative/MasonryBrickworksCatalog.cs
Assets/Ashfall.Core/Narrative/TextileSpinningWeavingCatalog.cs
Assets/Ashfall.Core/Narrative/FermentationYeastCatalog.cs
Assets/Ashfall.Core/Narrative/PneumaticTubeDispatchCatalog.cs
Assets/Ashfall.Core/Narrative/TanningLeatherworkCatalog.cs
Assets/Ashfall.Core/Narrative/IndustrialRuinsCatalog.cs
Assets/Ashfall.Core/Narrative/SignalIntelligenceCatalog.cs
Assets/Ashfall.Core/Narrative/WastelandCartographyCatalog.cs
Assets/Ashfall.Core/Narrative/MilitaryArmoryCatalog.cs
Assets/Ashfall.Core/Narrative/FaunaEntomologyCatalog.cs
Assets/Ashfall.Core/Narrative/CandleMakingWaxCatalog.cs
Assets/Ashfall.Core/Narrative/TimekeepingHorologyCatalog.cs
Assets/Ashfall.Core/Narrative/HydroGeologyCatalog.cs
Assets/Ashfall.Core/Narrative/BoneHornCarvingCatalog.cs
Assets/Ashfall.Core/Narrative/CordageCableCatalog.cs
Assets/Ashfall.Core/Narrative/CryoPreservationCatalog.cs
Assets/Ashfall.Core/Narrative/SeedBankPreservationCatalog.cs
Assets/Ashfall.Core/Narrative/TanningLeatherCatalog.cs
Assets/Ashfall.Core/Narrative/MetallurgyToolingCatalog.cs
Assets/Ashfall.Core/Narrative/BlackProjectsCatalog.cs
Assets/Ashfall.Core/Narrative/CrucibleFoundryCatalog.cs
Assets/Ashfall.Core/Narrative/RopeMakingCordageCatalog.cs
Assets/Ashfall.Core/Narrative/GrainMillingCatalog.cs
Assets/Ashfall.Core/Narrative/TimberCarpentryCatalog.cs
Assets/Ashfall.Core/Narrative/RefrigerationFermentationCatalog.cs
Assets/Ashfall.Core/Narrative/ApicultureBeeCatalog.cs
Assets/Ashfall.Core/Narrative/StructuralFortificationCatalog.cs
Assets/Ashfall.Core/Narrative/GlassblowingDistillationCatalog.cs
Assets/Ashfall.Core/Narrative/SoapSaponificationCatalog.cs
Assets/Ashfall.Core/Narrative/CharcoalPyrolysisCatalog.cs
Assets/Ashfall.Core/Narrative/MedicalPathologyCatalog.cs
Assets/Ashfall.Core/Narrative/BunkerContrabandCatalog.cs
Assets/Ashfall.Core/Narrative/SteamTurbinePowerCatalog.cs
Assets/Ashfall.Core/Narrative/AbyssalAnomaliesCatalog.cs
Assets/Ashfall.Core/Narrative/CeramicsKilnCatalog.cs

Assets/Ashfall.Core/Performance/CatalogReadProfiler.cs

## Completion evidence — 2026-09-27

- **Performance:** 300-second idle captures completed before and after at the enforced 15 FPS cap. Callback gates stopped hidden/empty work (Feedback 4,500→0, Expedition 4,500→0, hidden carousel 4,500→0); visible carousel animation remains active. Frame-tail and startup measurements are recorded in `docs/perf/FRAME_BASELINE.md`, `FRAME_AFTER.md`, and `STARTUP_BASELINE.md`. Catalog reads total 2.266 ms before and 2.069 ms after across the measured startup set, so no lazy loading was justified. The 10.11s/11.63s startup and 66.675ms/109.142ms mean frame intervals are noisy captures, not claimed gains.
- **Release weight:** ExportRelease excludes host selftest sources; Linux RID assembly fell from 16,151,552 to 15,301,120 bytes (850,432 bytes / 5.3%). Debug retains selftest entry points. Release symbol gate, parity (1,423 catalogs), 60-frame exported boot, packaged-data integrity (427/427), research-catalog, and bridge probes passed. Export PCK is 222,148,800 bytes; older PCK came from a different source/data snapshot and is not used as a before comparison. Exported startup also logged nonfatal MCP editor-plugin parse and missing localization/art warnings.
- **Host decomposition:** plan-numbered partials were renamed by actual domain, same-domain files consolidated, the Main ownership map produced, CLI panel selftests split, and `CampaignDayHostSession` now reuses the existing day coordinator. Current host build passed with 0 errors (18 warnings); equivalent member/token audit found 688 members unchanged. `BioFermentationHostSession` received its missing `System.Collections.Generic` import after the focused compile exposed it.
- **Save and navigation QoL:** canonical save slot rotation keeps three validated generations, exposes explicit backup recovery, and persists last panel/current campaign day through schema v3 while retaining v1/v2 compatibility. Scoped save tests passed 7/7; crash-kill probe verified an interrupted temp write leaves the committed primary and three backups valid, then explicit recovery restores the prior payload/day/panel; starting-cohort lifecycle, seven-day smoke (10/10), and runtime-scale (6/6) selftests passed. Six duplicate agent rulebooks now point to `AGENTS.md`; four completed coordination records are archived; README and docs index were updated. `generate-docs-index.py --check` verified 5,523 documents.
- **Limits:** no full suite was run. The five-minute profiles used llvmpipe and 15 FPS rather than a 60 FPS GPU-backed player target; the after profile had unexplained long stalls. No simulation, determinism contract, or eager-catalog behavior was changed. No commit was made.
