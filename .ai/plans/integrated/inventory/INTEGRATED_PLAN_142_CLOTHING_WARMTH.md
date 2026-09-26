# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# Plan 142 — Clothing & Warmth — Full Host Integration

**STATUS: APPROVED BY USER**
**Authorized by:** user directive in session ("Yes integrate that fully!") — 2026-09-26.
**Claim:** `claim-plan142-clothing-warmth-integration-2026-09-26`
**Signed design authority:** `DEC-109` (Clothing & Warmth Layering & Cold Exposure Reduction, `SIGNED` 2026-09-20), anchor `C2[29]`.

## Bounded outcome

Convert the DEC-109 Core `ClothingWarmthSystem` from a host-unreachable
authority into a fully wired host feature: equipment layers and cold
mitigation are persisted, the existing but never-assigned
`NeedsSystem.ClothingWarmthReductionProvider` seam is bound to the live
clothing authority, and the system advances deterministically each campaign
day (wear + drying + weather wetness). No second needs/temperature authority,
no new inventory store, no invented item catalog.

## Premise audit (evidence-first)

- `ClothingWarmthSystem` has **0 references in `src/`**; `NeedsSystem.ClothingWarmthReductionProvider`
  (declared `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:97`) is assigned only
  by the Core test `ClothingWarmthSystemTests.cs:90` — never by the host. So
  equipped clothing currently has **zero gameplay effect** in the live game.
- No `clothing_warmth` save section, no CLI probe, no route/UI readout.
- The 8 profile item ids (`item_winter_coat`, …) do **not** exist in `items.json`.
  Authoring a `clothing_warmth_profiles.json` catalog would therefore declare
  foreign keys that resolve nowhere and trip the integrity pipeline. The
  DEC-109-signed internal profile table is kept as the system's data authority;
  a catalog is deliberately NOT invented here.
- Authority boundaries (Rule 5): `NeedsSystem` remains the sole mutable
  warmth/needs authority; `Inventory` remains physical item custody;
  `WeatherSystem` remains the weather authority. `ClothingWarmthSystem` owns
  only equipped-layer records, wetness, gear condition, and the cold-loss
  reduction fraction it feeds to NeedsSystem.

## Files changed

- Core: `Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs` (additive
  `ClothingWarmthCensus` + `GetCensus()`), `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
  (`clothing_warmth` section + filename), `Assets/Ashfall.Core/HostCliRegistry.cs`
  (`ClothingWarmthSelfTest` + descriptor), `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs`
  (`clothing_warmth_ticked` heartbeat).
- Host: `src/Host/ClothingWarmthHostSession.cs` (new; `ClothingWarmthSaveStore` +
  `SaveStoreHub.Checksummed<ClothingWarmthSaveState>`), `src/Main.ClothingWarmth.cs`
  (new; setup/save/flush/reset, provider binding, day tick, census, readout),
  `src/Host/HostCli.ClothingWarmth.cs` (new, 12 checks), `src/Host/HostCli.cs`
  (enum/parse/help), `src/Main.Application.cs` (dispatch), `src/Main.SaveOrchestrator.cs`
  (setup/save), `src/Main.Lifecycle.cs` (reset), `src/Main.CampaignOwners.cs`
  (phase-5 `ClothingWarmthDayOwner`), `src/Main.PlayerSurfaces.cs` +
  `src/UI/SurvivorDetailPanel.cs` (read-only clothing/warmth row).
- Tests: `Ashfall.Core.Tests/Inventory/Plan142ClothingWarmthHostIntegrationTests.cs`
  (new), `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`
  (section pin).
- Generated: `scripts/ci/generate-architecture-map.py` (+1 node) and the
  owning generators' outputs.
- Governance: this plan, `docs/plans/UNBLOCK_PLAN142_CLOTHING_WARMTH_INTEGRATION_PLAN.md`,
  `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `docs/governance/DECISION_REGISTER.md`.

## Acceptance

1. Core/tests/host builds 0 errors / 0 new warnings.
2. `Plan142ClothingWarmthHostIntegrationTests` + `ClothingWarmthSystemTests` green.
3. `--clothing-warmth-selftest` 12/12 headless.
4. `NeedsSystem.ClothingWarmthReductionProvider` bound in host: equipping a
   winter coat measurably reduces cold loss versus no clothing.
5. Save round-trip preserves equipped items, condition, and wetness; section pin
   recorded.
6. Generated `--check` gates pass (architecture map, save-store matrix, CLI
   catalog, selftest manifest).
7. Plan marked FULLY INTEGRATED and archived.
