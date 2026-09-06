# PLANS B66–B69 — HOST WIRING & CROSS-PLAN SCENARIOS CLOSEOUT

**Date:** 2026-09-06 · **Branch:** `feat/asset-pipeline-flagship`

## Delivered

### Host wiring (Main triad)
- **B66** — rides the pre-existing `SilentFoundryHostSession` (power draw,
  brownout suspension, thermal waste-heat already wired). Activated in-host:
  `BindMetallurgyCatalog` (12-recipe heavy roster merged into the production
  catalog) + `Engine.BindVentilation(_ventilation)` (heavy batches emit
  smoke/CO through the canonical ventilation authority).
- **B68** — `src/Main.PlansB68_B69.cs`: `SetupSeismicDynamics`/`SaveSeismicDynamics`
  (authored fault catalog + save restore), ticked in phase 1 via
  `SeismicGeologyDayOwner` (before production, per the plan's tick ordering).
- **B69** — `SetupCryoVault`/`SaveCryoVault` with canonical ports:
  power = grid room `room_cryo_vault` (new in `power_grid.json`, 280 W,
  critical priority; brownout/trip destabilizes storage; pre-B69 saves
  without the room fall back to brownout-only), radiation = normalized
  survivor dose (÷50 mSv clamp). Ticked in phase 2 via `CryoVaultDayOwner`
  (after the foundry — a brownout day degrades samples exactly once).
- **Scenario E handoff** — `OnQuakeOccurred` (magnitude ≥ 5.5, authored
  threshold) → `CryoVault.TriggerBreach`.

### Save registration (SaveStoreHub)
- `src/Host/SeismicDynamicsSaveStore.cs`, `src/Host/CryoVaultSaveStore.cs`
  — thin `SaveStore<T>` façades over `SchemaVersionedEnvelope`, capturing
  into the atomic campaign envelope via `CaptureSection`.
- `SaveSectionRegistry`: +2 metadata rows and +2 `SectionFileNames`
  whitelist entries; `SaveAll` enrolls both sections.
- Contract matrices updated: 156 sections / 150 checksum envelopes;
  `ARCHITECTURE_TEST_MAP.md` rows 155–156.

### Cross-plan scenarios A–G (integration tests)
`Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs` — 7 tests
over shared inventory, all **PASS**:
- **A** beam cast → `ExcavationSystem.TryApplyStructuralReinforcement`
  (cost 2), restore shows no duplicate output.
- **B** prepared shelter (dampener) weakens pulse deterministically.
- **C** quake during a heavy batch: heat machine not reset, batch completes
  exactly once.
- **D** cryo power crisis: save/load split == uninterrupted run; bounded loss.
- **E** severe quake → breach → triage preserves the protected line.
- **F** intercept decrypt + 3 bearings → authored location revealed exactly
  once → cultivar recovery releases the canonical seed.
- **G** full stress (beam batch + dampeners + quake + brownout + cryo):
  split at the warning boundary equals the straight run on all outputs.

## Verification (2026-09-06)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| Focused gate (`_verify_b6869.csproj`) | **68/68 PASS** (A–G + B66 17 + B68 14 + B69 14 + contract tests) |
| Save-contract tests (envelope fuzz, builder, corruption matrix, version report, triad drift, architecture map) | PASS after registry/count updates |
| `--data-integrity-selftest` | PASS — 284 catalogs |
| `--bridge-selftest` / `--scene-binding-selftest` | PASS / 25/25 |

**Full-suite verdict deferred:** the shared test project is intermittently
uncompilable from an in-flight concurrent stream (untracked
`MusterWarfareEngine` / `PowerGridSurge` / `WaterAndQuarantine` churn).
The canonical suite must be re-run once that stream lands; the last stable
full run (before the churn) was 8854/8855 with the single failure in that
stream's own code.

## Commit granularity note

Shared files whose diffs interleave with the muster stream's uncommitted
work (`Main.CampaignOwners.cs`, `Main.SaveOrchestrator.cs`,
`SaveSectionRegistry.cs`, `power_grid.json`, the two contract test files,
`ARCHITECTURE_TEST_MAP.md`) are **left uncommitted in the working tree** —
they reference that stream's untracked files and would not compile in
isolation. They ride the next integration commit. This commit contains only
the self-contained deliverables.

## Known follow-ups

1. Integration commit for the shared wiring files once the muster stream
   lands (working tree already verified green).
2. `CryoVaultPanel` + `SeismicMonitorPanel` UI (Stitch-first per policy).
3. Player-facing routes for geophone/dampener installation and vault actions.
