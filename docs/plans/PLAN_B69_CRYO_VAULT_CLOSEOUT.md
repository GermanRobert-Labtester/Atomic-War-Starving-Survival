# PLAN B69 CLOSEOUT — Cryogenic Sample Preservation & Genetic Cultivar Seed Vault

**Date:** 2026-09-06 · **Branch:** `feat/asset-pipeline-flagship`
**Scope:** core vault slice — 18-cultivar data authority, the canister/
viability/breach state machine, and canonical greenhouse/pharma handoffs.
Host session wiring and the CryoVaultPanel UI are follow-ups.

## Architecture decision

**Expansion, no duplication of any authority:**

| Concern | Owner (untouched) | B69 relationship |
|---|---|---|
| Coolant production | `CryogenicAirSeparationSystem` | vault *consumes* `item_nitrogen_supply` (the plant's product) |
| Insulation | B66 metallurgy | upgrades consume `item_metallurgy_shielding_plate` |
| Cultivation | `GreenhouseExpansionCatalog` | recovery releases **existing canonical seed items**; greenhouse consumes them via its standard planting path |
| Medicine | `PharmaLabSystem` | culture lines release `item_hermetic_sample_ampoule` — a generic pharma `input_ids` item |
| Radiation | provider port (`Func<float>`) | vault never computes dose; it scales decay by authored sensitivity |
| Power | provider port (`Func<bool>`) | brownout → instability rise; warning window before loss |
| Narrative lore | `SeedBankPreservationCatalog` / `CryoPreservationCatalog` | untouched (prose documents, not runtime loops) |

## Files changed

| File | Change |
|---|---|
| `Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs` | **new** — `CryoCultivarDef`/catalog/loader (`cryo_cultivars.json`), `CryoCanisterPhase` state machine (`Loaded→Stable→Warning→Critical→RecoveryQueued→Thawing→Released/Failed`), `CryoVaultSaveState`, actions (`RegisterSample`, `ReplenishCoolant`, `UpgradeInsulation`, `QueueRecovery`, `SetTriageProtection`, `TriggerBreach`, `ResolveBreach`), daily tick (coolant burn → thermal stage → decay profile → radiation scaling → recovery pipeline), full save round-trip |
| `Assets/StreamingAssets/Data/cryo_cultivars.json` | **new data authority** — 18 specimen lines (`cryo_seed_*`/`cryo_culture_*` ids), `schema_version 1` |
| `Ashfall.Core.Tests/Shelter/CryoVaultB69Tests.cs` | **new** — 14 tests |

## Mechanics summary

- **No duplication invariant:** `RegisterSample` consumes the source item
  atomically and the line exists in exactly one canister; `CompleteRecovery`
  releases the canonical item and clears the canister in the same
  transaction (host inventory-full stalls the release instead of duplicating).
- **Viability:** 0..1000 persisted. Stable decay ~0.15–0.4/day; unstable
  (Warning/Critical/no-power) 4–9/day; breach 10–16/day; all scaled by
  authored `radiation_sensitivity` × provider exposure. Clamped, never
  rerolled after restore.
- **Coolant economy:** populated vault burns ≥ 1/day (4 − 0.8·insulation);
  breach boils 12/day. `ReplenishCoolant` trades one nitrogen unit for +35.
- **Recovery (69.8):** queued → thawing (`recovery_days`, power-gated) →
  viability-gated deterministic outcome (≥ 200 permille releases
  `recovery_amount`; below → failed, sample lost — resolved once, persisted).
- **Breach triage (69.13):** `TriggerBreach` → bounded drain with
  `SetTriageProtection` halving the drain (×0.4) — strategic choice, never
  an instant wipe. `ResolveBreach` after repair.
- **Insulation:** 3 levels, one B66 shielding plate each; slows coolant burn.

## Data authority — 18 cultivars

Recovery items are exclusively canonical: `item_seed_wheat`,
`item_seed_cold_legume`, `item_seed_hardy_tuber`, `item_seed_ash_grain`,
`item_seed_biolum_mushroom`, `item_seed_mushroom`, `item_seed_nutrient_algae`,
`item_seed_medicinal_herb`, `item_seed_leafy_green`, `item_seed_oilseed`,
`item_hermetic_sample_ampoule` (pinned by
`Catalog_AllRecoveryItemsResolveToCanonicalSeeds`). Sample types: seed /
culture. Traits: radiation_tolerant, rapid_growth, low_light, protein_rich,
pharmaceutical_yield, frost_hardy, heirloom.

## Verification (2026-09-06)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `dotnet test` (full suite) | 8854/8855 — B69 **14/14 PASS**; the 1 failure is `CatchPolicyLintGateTests` on the **untracked concurrent-stream file** `ShelterPowerGridCatalog.cs` (not B69 code; B69's loader passes the same gate via `CatalogDiagnostics.Warn`) |
| `--data-integrity-selftest` | PASS — **284 catalogs** (incl. `cryo_cultivars.json`), 0 errors, 11 066 ids |
| `--bridge-selftest` / `--scene-binding-selftest` | PASS / 25/25 (no scenes changed) |
| Paired determinism + no-reroll round-trip | covered (`PairedRuns_SameSeed_IdenticalViabilityCurve`, `SaveRoundTrip_ViabilityPersists_NoReroll`) |

## Known follow-ups

1. **Host wiring** — construct/tick the vault, register save section
   (follows the `SaveStoreHub`/envelope pattern), bind radiation + power
   providers to canonical authorities.
2. **UI** — `CryoVaultPanel` (slots, viability, coolant, thermal, triage);
   must follow the panel standard; `CryogenicPermafrostCorePanel` is a UI-05
   stub and must not be promoted as-is.
3. **Seismic handoff (B68)** — severe quake events call `TriggerBreach`
   (Scenario E cross-plan test).
4. **Power budget** — register the vault's draw in the shelter grid
   scenario (Plan 66/69 shared power economy).
