# XP W1 Premise Evidence

**Package:** `XP-WAVE1-DIFFICULTY-AUTHORITY`
**Date:** 2026-09-18
**Status:** active

## Scope

| Item | Result | Evidence |
|---|---|---|
| XP-01 difficulty authority | Start | Repository search found no `DifficultyPreset`, `DifficultyDirector`, or `difficulty_presets.json`. |
| XP-01 completion chronicle | Deferred | `CampaignCompletionHistory` is an append-only completion ledger; it contains no difficulty ID or started-run event. Its paths are actively claimed by Wave 11. |
| XP-05 inventory fuel binding | Already live | `src/Main.Plans122to125.cs` binds `FuelConsumer = ConsumeSofcFuel`; the method consumes inventory fuel and then the grid reserve. |
| XP-05 quality catalog | Already live | `Assets/StreamingAssets/Data/sofc_power_catalog.json` contains `dirty`, `treated`, and `clean` fuel profiles with authoritative item IDs. |
| XP-05 persistence | Already live | `SofcPowerSaveStore` persists the existing `sofc_power` plant state. No fuel reserve is introduced. |

The source proposal names `item_canned_rations` and `item_iodine` as the
sparing preset's starter items. Neither ID exists in the shipped data. The
implemented catalog uses the existing `canned_food` and `iodine_pills` item
definitions; `starting_bonus_item_ids` is registered as a strict
cross-catalog reference key.

## Initial owned files

- `Assets/Ashfall.Core/Difficulty/` (new)
- `Assets/StreamingAssets/Data/difficulty_presets.json` (new)
- `Ashfall.Core.Tests/Difficulty/` (new)
- The governance and evidence files listed in `WORKTREE_OWNERSHIP.md`

## Invariants

- Core code remains engine-free.
- Catalog IDs use `snake_case`; catalog values are validated before host use.
- The director is a read-only scalar provider and does not own endings,
  completion history, UI state, or RNG.
- Existing behavior remains unchanged until a separately premise-checked
  consumer binds the provider.

## Focused verification

| Command / test | Result |
|---|---|
| `DifficultyPresetCatalogTests` + `DifficultyDirectorTests` | PASS — 8/8 |
| `CatalogIntegrityValidatorTests.AllCatalogIdsCrossReferenceCleanly` with the new catalog | PASS |
| `Plan122SofcElectrochemistryEngineTests` | PASS — 17/17 |

The broader `CatalogIntegrityValidatorTests` fixture class currently has four
unrelated failures because the concurrently edited `duty_roles.json` now
requires a `ward` role that the shared scratch fixture does not seed. The
shipped-data check above passed and showed no difficulty catalog error.
