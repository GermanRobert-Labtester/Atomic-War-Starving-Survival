# Plan 213 — Advanced Crafting & Metallurgy Reconciliation: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-210-213-FLAGSHIP-ECONOMY`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Material.cs` (partial — extension, NOT a new system), `MaterialProfileCatalog.cs`, additive fields in `SilentFoundryTypes.cs`, provenance hook in `.Heat.cs`, restore fix in `.TreatyLabor.cs` |
| Data | `Assets/StreamingAssets/Data/alloys_and_ores.json` (6 material profiles — PROPERTIES ONLY; recipes stay solely in `metallurgy_recipes.json`) |
| UI | Purity + forging status cards on `SilentFoundryPanel`; `OnForgingCompleted`/semantic cast payload forwarders in the foundry host session |
| Save | Rides the existing `silent_foundry` section — additive fields, NO new store (registry-gated) |
| Tests | Reconciliation 11/11 · Foundry regression 73/73 · scenario D inside 6/6 |

## Reconciliation contract (§173.1 — enforced, not just documented)

- **Reflection gate:** no `MetallurgySystem`/`FoundryProductionSystem`/`MetallurgyQueue`/`MetallurgySaveStore`/`AlloyCatalog`/`ForgingSystem` types may exist; pre-existing B66 + Plans 130–133 powder authority sanctioned.
- **Registry gate:** no new metallurgy/forging/alloys save sections.
- **Purity** derives from the standard completion path (quality − bounded contamination 12% / slag 10% penalties) — never a chemical percentage.
- **Forging** is a zero-RNG deterministic command sequence (closed vocabulary heat/shape/finish/inspect vs the authored ideal order × purity factor, clamped [100,1300]) — headless-testable per §173.7.
- **Provenance** (`purity`/`materialProfileId`/`craftQualityPermille`) rides `FoundryProductionRecord` additively; old saves read Standard/unknown and are NEVER recalculated retroactively (migration-tested).
- **Vehicle armor (D6):** shipped as the read-only `TryGetLatestMaterialQuality(Any)` handoff query only — no vehicle authority exists, none invented.

## Real bug sealed

`SilentFoundrySystem.RestoreState` silently dropped the B66 metallurgy fields (active recipe, slag, batch count) on every restore — now restored additively with legacy-neutral defaults, and the forging session resumes mid-sequence exactly (save-mid-batch test).
