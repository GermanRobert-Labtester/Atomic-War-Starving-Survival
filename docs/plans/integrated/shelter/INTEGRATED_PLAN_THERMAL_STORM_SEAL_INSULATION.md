# PLAN-THERMAL-STORM-SEAL-INSULATION — Authored Insulation Catalog Reaches the Live Thermal Owner
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — authored data bound · existing owner seam used · live composition site wired · CLI probe green · focused tests green · no parallel authority · no new save section or day event.**

## Integrated evidence

Authored `shelter_insulation_catalog.json` (5 tiers) now reaches `ShelterThermalSystem` through its own `LoadInsulationCatalog` seam, called from the live composition site in `src/Main.ShelterInfrastructure.cs`.

Verified defect closed: `insul_storm_sealing` — the exact id `ShelterThermalHostSession` passes to `RetrofitInsulation` — was absent from the owner's four built-in tiers, so every storm-sealing retrofit returned `Failed("unknown_insulation")` and sealed zero rooms while reporting a blocked count. It now succeeds and records `roomInstalledInsulation`. The four pre-existing tiers also answer to authored values instead of C# literals.

Probe `--thermal-storm-seal-selftest`: 10/10. No new save section — thermal state already persists through `ShelterThermalSaveStore`.

## Original plan body (preserved for the record)

> **Package:** `shelter`
> **Category:** THERMAL-STORM-SEAL-INSULATION
> **Plan type:** bind authored game data to the existing, designed seam on its live owner.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`
> **Claim:** `claim-quad-i-shelter-2026-09-26`

## 1. Objective
Make one already-designed Core bind seam actually receive its authored data, so
JSON remains authoritative (AGENTS.md rule 3) without creating a second owner.

## 2. Current evidence (verified in source, this session)
- `ShelterThermalSystem.LoadInsulationCatalog(string)` (Assets/Ashfall.Core/ShelterThermalSystem.cs:241) has **zero callers** in `src/` and `Assets/`.
- The constructor instead registers **4 built-in** defs (`insul_scrap_panels`, `insul_mineral_wool`, `insul_fiberglass_batts`, `insul_aerogel_composite`, lines 262-309).
- `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` authors **5** defs — including `insul_storm_sealing` (costs: 6 cloth, 4 scrap_wood, 2 duct_tape).
- `src/Host/ShelterThermalHostSession.cs:99` hardcodes `const string stormSealingId = insul_storm_sealing` and calls `System.RetrofitInsulation(room.roomId, stormSealingId, inventory)` at :116.
- Therefore today: `RetrofitInsulation` hits `if (!_insulationCatalog.TryGetValue(...))` → `Failed(unknown_insulation)` for **every room**, so the live storm-sealing routine always applies 0 rooms while reporting a blocked count. This is a silent runtime failure, and the four reachable tiers use built-in numbers rather than authored numbers.

## 3. Design decision
Load the authored file at composition time and feed it through the owner's own `LoadInsulationCatalog` seam (`RegisterInsulation` is last-write-wins, so authored rows override the built-ins). The thermal owner keeps sole authority over insulation; the host only supplies the authored table.

## 4. Non-goals
* No new save section, no new day-event heartbeat, no new Core system, no new catalog.
* No gameplay-rule invention: only data that already exists in `Assets/StreamingAssets/Data/`.
* No UI surface is added unless the command already exists on the owner.

## 5. Implementation surface
- `src/Main.PackageIBindings.cs` — `BindAuthoredInsulationCatalog()`, called from the shelter-thermal composition site (`src/Main.ShelterInfrastructure.cs:446`).
- `src/Host/AuthoredDataBinder.cs` — one shared, read-only helper that returns authored JSON text by file name (no caching of gameplay state).

## 6. Verification (bounded)
* One headless host probe: `--thermal-storm-seal-selftest`
* One focused engine-free Core suite: PLAN_THERMAL_STORM_SEAL_INSULATION_HOST_INTEGRATION.md0
* Adjacent gates only where this package can move them.

## 7. Acceptance
Authored rows are reachable through the live owner; the probe and suite pass; the
existing owner remains the single authority; no duplicate ledger/cache is created.
