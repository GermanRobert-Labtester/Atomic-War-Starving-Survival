# XP W1 Implementation Log

**Package:** `XP-WAVE1-DIFFICULTY-AUTHORITY`
**Slice:** catalog and Core resolver
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
| `git diff --check` | PASS |

## Follow-on boundary

The catalog is deliberately not yet bound to campaign creation, persistence,
or the seven proposed consumer seams. Those edits need individual source
premise checks and save-owner integration. `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY`
remains open until those paths and the read-only chronicle are complete.
