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
