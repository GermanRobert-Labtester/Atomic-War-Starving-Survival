# XP W1 Implementation Log

**Package:** `XP-WAVE1-DIFFICULTY-AUTHORITY`
**Slice:** catalog, campaign binding, and checked header restore
**Date:** 2026-09-18

## Outcome

Implemented the first XP-01 authority slice:

- `difficulty_presets.json` is the sole authored source for four campaign
  presets.
- `DifficultyPresetCatalogLoader` fails closed on malformed JSON, unsupported
  schema, duplicate IDs, invalid scalar envelopes, and an unresolved default.
- `DifficultyDirector` resolves a persisted ID or the catalog default to an
  immutable scalar provider. An explicit unknown ID fails closed.
- `DifficultyScalarsProvider.Legacy` exposes all-one values for consumer
  parity before an owner opts into difficulty.
- The permanent data-integrity walk validates the typed catalog and strict
  starter-item references.

Implemented the campaign-binding sub-slice:

- The New Game panel exposes all authored presets with their data-authoritative
  descriptions and submits the stable preset ID with cohort and stores.
- The checked `campaign_day` header stores `difficulty_preset_id`; v1 headers
  validate against their historical checksum shape and migrate to v2 with the
  authored default on their next write.
- The inventory owner adds bonus items only for a fresh campaign. Restores
  never grant them again.
- `campaign_day` is now reset with the rest of the selected slot's sessions,
  allowing the header and difficulty selection to restore before dependents.

## Source corrections applied

| Proposal premise | Current source result | Action |
|---|---|---|
| SOFC fuel consumer is a `units => true` placeholder | Already inventory/grid bound by `Main.Plans122to125.cs` | No duplicate XP-05 fuel catalog, reserve, or save section; verified the existing SOFC tests. |
| sparing bonus IDs are `item_canned_rations` and `item_iodine` | Neither exists in shipped data | Used existing `canned_food` and `iodine_pills` IDs. |
| completion ledger can project started runs by difficulty | It stores completions only and has no difficulty ID | Deferred chronicle projection while Wave 11 owns the append-only history paths. |

## XP-01 full-binding premise audit — 2026-09-19

The follow-on package is authorized under the existing W1 claim. Before any
production edits, the current owner sites were re-read and recorded in
`docs/plans/CF_XP01_DIFFICULTY_FULL_BINDING_INTEGRATION_PLAN.md`:

- New-game selection is the existing cohort/supplies transaction in
  `Main.GameFlow.cs`; the panel forwards that selection through `Main.UiPanels.cs`.
- Needs hunger/thirst base drift is the one `ApplyBaseNeedDrift` site.
- Radiation dose uses the shared `ComputeEffectiveRate` chain before resistance
  and exposure; its clamps remain the owner’s responsibility.
- Disease onset probability is clamped in `TryExpose` after source/context
  modifiers.
- Market `ExplainPrice` owns the base/factor/floor/ceiling path.
- Equipment wear is applied once in `ApplyWear` before the condition floor and
  break/jam checks.
- Crisis deadlines are produced by `CrisisPredictor.Evaluate` from runway and
  horizon calculations.

No premise drift was found. The completion-history paths remain excluded from
this package and are not edited.

## Verification

| Gate | Result |
|---|---|
| `DifficultyPresetCatalogTests` + `DifficultyDirectorTests` | PASS — 8/8 |
| Shipped `CatalogIntegrityValidatorTests.AllCatalogIdsCrossReferenceCleanly` | PASS |
| `Plan122SofcElectrochemistryEngineTests` | PASS — 17/17 |
| `CampaignDayDifficultyMigrationTests` | PASS |
| `starting_cohort_lifecycle_selftest` | PASS — selected `difficulty_sparing` survives a slot restore and grants one `canned_food` plus one `iodine_pills` on fresh initialization |
| `git diff --check` | PASS |

## XP-01 full-binding completion — 2026-09-19

The follow-on package is complete. The selected preset now reaches all seven
bounded consumer seams through existing owners: hunger/thirst, radiation,
disease onset, market pricing, equipment wear, crisis deadlines, and the
fresh-campaign canonical inventory bonus path. Standard/legacy parity is
preserved, the panel exposes the authored display name and current numeric
scalars, and the checksummed manifest rejects unknown preset IDs before live
restore.

The first lifecycle run exposed a duplicate starter grant in the new-game
flow. It was removed so `SetupInventory` is the sole fresh-campaign bonus
owner; the end-to-end lifecycle gate then passed with exactly one `canned_food`
and one `iodine_pills` bonus for Sparing.

Verification: Core difficulty suite 15/15; Host CLI help 2/2; runtime
`--difficulty-selftest` 14/14; runtime `--starting-cohort-lifecycle-selftest`
PASS; host build 0 warnings/0 errors. The completion-history chronicle
projection remains separately deferred under `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY`;
the Wave 11 completion-history paths remain untouched.
