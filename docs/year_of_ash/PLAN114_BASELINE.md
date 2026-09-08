# Plan 114 Baseline

Date: 2026-09-08

The Year of Ash catalog authority is `Assets/StreamingAssets/Data/year_of_ash_questlines.json`.
It uses a versioned root object with a `quests` array. The pre-change catalog contained eight
questlines; the implementation preserves those eight definitions and appends seven new definitions,
for a verified total of 15.

Runtime findings:

- `YearOfAshCatalogLoader` loads the JSON catalog and registers definitions with `QuestlineSystem`.
- `QuestlineSystem.GetAvailableQuestlines(day)` uses an inclusive absolute `minDay <= day <= maxDay`
  start window.
- `QuestlineSystem.TakeChoice` applies the authored numeric deltas, records choice history, and
  transitions by `nextStageId`. It does not currently evaluate `conditions` or enforce
  `unlockOnDay`.
- Terminal resolution occurs when a choice enters a terminal stage. Terminal stages in the new data
  have no choices.
- Year of Ash persistence already stores quest state inside `YearOfAshSave` version 5; no new save
  schema was added.
- `DoorEncounterSystem` is the door-encounter catalog/runtime, not a second quest graph runtime.

The existing eight questline IDs, stage chains, and authored values were treated as the parity
baseline. A prior parity test was also corrected so it no longer overwrites the JSON authority with
the eight-entry built-in fallback catalog; the test now verifies the existing eight against the
expanded authority.

Targeted Plan 114/parity verification: 9 passed, 0 failed. Full integrity, test, and build results
are recorded in `YEAR_OF_ASH_REGRESSION_MATRIX.md` after the final sweep.
