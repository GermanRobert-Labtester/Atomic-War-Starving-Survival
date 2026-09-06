# PLAN B66 CLOSEOUT — Subterranean Heavy Manufacturing & Metallurgical Smelting

**Date:** 2026-09-06 · **Branch:** `feat/asset-pipeline-flagship`
**Scope:** core vertical slice of the heavy metallurgy expansion. UI, host
session wiring and cross-plan (B68/B69) hooks are follow-ups.

## Architecture decision

**Option A honored — extension, no competing authority.** The heavy roster
rides the existing `SilentFoundrySystem` heat stage machine
(`ChargeLoaded→Preheat→AtHeat→Tapped→Casting→Cooling→Complete`), the
standard quality roll, the standard output transaction and the standard
incidents/labor surface. Heavy recipes are projected into
`FoundryProductEntry` shape (`category: "heavy_metallurgy"`) and merged into
the bound `SilentFoundryCatalog` — `CompleteCast` needed no branching.
No new save store, no new system class, no duplicated furnace state.

## Files changed

| File | Change |
|---|---|
| `Assets/Ashfall.Core/Foundry/MetallurgyHeavyCatalog.cs` | **new** — `MetallurgyRecipeEntry`/`MetallurgyHeavyCatalog`/loader (`metallurgy_recipes.json`), with `ToProductEntry()` projection |
| `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Metallurgy.cs` | **new partial** — `BindMetallurgyCatalog`, `BindVentilation`, `StartHeavyBatch` (atomic preflight), `SkimSlag`, slag accumulation, tier-scaled lining wear, ventilation source lifecycle |
| `Assets/Ashfall.Core/Foundry/SilentFoundryCatalog.cs` | `MergeHeavyRecipes()` (id-safe merge, never overwrites) |
| `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs` | minimal hooks: slag quality penalty (−slag/8), slag incident pressure (+slag/12 capped 60), `AdvanceMetallurgy` call in `TickDaily`, `ClearHeavyBatch` on dump paths, `OnHeavyCastResolved` after quality roll |
| `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs` | state fields `activeMetallurgyRecipeId`/`metallurgySlag`/`metallurgyBatchesCompleted` (legacy-safe defaults), B66 id constants |
| `Assets/StreamingAssets/Data/metallurgy_recipes.json` | **new data authority** — 12 recipes, `schema_version 1` |
| `Assets/StreamingAssets/Data/items.json` | +11 items (`item_metallurgy_*`); one recipe output reuses existing `item_foundry_shoring_bracket` |
| `Ashfall.Core.Tests/Foundry/MetallurgyB66Tests.cs` | **new** — 17 tests |

## Data authority — 12-recipe roster

Feedstocks: `metallurgy_iron_ingot`, `metallurgy_copper_ingot`,
`metallurgy_steel_billet`, `metallurgy_solder_stock` ·
Structural: `metallurgy_heavy_i_beam`, `metallurgy_shoring_plate`,
`metallurgy_reinforcement_bracket` (→ existing `item_foundry_shoring_bracket`) ·
Mechanical: `metallurgy_spring_steel_billet`, `metallurgy_gear_blank`,
`metallurgy_shaft_stock` · Specialist: `metallurgy_shielding_plate` (B69
cryo shielding hook), `metallurgy_tool_blank`.

All authored gameplay values (heat tiers 1–3, normalized slag yield) — no
real-world furnace/alloy data, per the safety boundary.

## Mechanics summary

- **Atomic start:** flux + charge + fuel + water all validated before any
  consumption; a refused start consumes nothing (state-machine verified).
- **Slag:** accumulates daily while a heavy batch cooks (`slag_yield /
  labor_days`), penalizes quality (−slag/8), raises tap incident chance,
  capped 0..100. `SkimSlag` removes 40.
- **Refractory wear:** heavy resolution applies `1.5 × process_heat_tier`
  extra lining wear on top of standard per-day wear (single pool).
- **Ventilation handoff:** heavy batches register a
  `VentilationSource` (smoke/CO scaled by heat tier, room
  `room_bp_11_the_silent_foundry_smelter_bay`); deactivated on completion,
  failure, incident or burnout. `VentilationSystem` owns air state.
- **Worker skill** applies exactly once (through the standard
  `StartProduction` path — no second bonus).

## Save fields (in `SilentFoundryState`, additive)

`activeMetallurgyRecipeId` (legacy default empty), `metallurgySlag`
(legacy 0), `metallurgyBatchesCompleted` (legacy 0). Old saves load with a
clean idle crucible — never an active batch. Resolved quality is persisted
(`pendingQuality`) and never rerolled; outputs commit exactly once.

## Verification (2026-09-06)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS 0 errors |
| `dotnet test` (full suite) | 8807/8809 — B66 tests 17/17 PASS; the 2 failures are `CampaignContinuityFlagshipB70_B73Tests` (concurrent B70–B73 stream, reproduced with and without B66 changes) |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `godot --headless -- --data-integrity-selftest` | PASS — 283 catalogs, 0 errors, 0 warnings (11 041 ids) |
| `godot --headless -- --content-utilization-selftest` | Orphaned: 0 |
| `godot --headless -- --bridge-selftest` | PASS |
| `godot --headless -- --scene-binding-selftest` | PASS — 25/25 (no scenes changed) |
| Paired determinism + mid-batch save round-trip | covered by tests |

## Known follow-ups

1. **Ventilation for standard (non-heavy) heats** — regular product heats do
   not yet register a ventilation source; extending the handoff to all heats
   may shift existing balance tests and is deliberately out of this slice.
2. **B68 hook** — `metallurgy_heavy_i_beam` / `metallurgy_shoring_plate`
   must be consumed by seismic dampener/shoring recipes when B68 lands.
3. **B69 hook** — `metallurgy_shielding_plate` reserved for cryo vault shielding.
4. **UI** — no dedicated metallurgy panel yet; existing foundry UI surfaces
   the shared heat machine. Follow the UI-panel standards (no fixture data,
   truthful state) when built.
5. Full-suite green requires the B70–B73 stream's continuity tests healed.
