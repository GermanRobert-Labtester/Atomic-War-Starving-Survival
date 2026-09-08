# Fresh Campaign Initialization Contract

## Lifecycle modes

New campaign composition runs under `FreshInitialize`. Slot restore runs
under `Restore`.

`FreshInitialize` is the only mode allowed to load a starting cohort.
`Restore` first loads the selected campaign envelope and then restores the
survivor aggregate, including a valid empty roster. Restore never calls the
starting-cohort loader.

## Fresh transaction

1. Reset all in-memory sessions only.
2. Allocate the next deterministic free slot (`slot_1`, then `slot_2`, ...).
3. Create/select that empty slot before composing sessions.
4. Clear only legacy global projection files, never another slot root.
5. Compose and apply the selected cohort once.
6. Save state through the selected slot's normal campaign envelope.

If slot creation fails, New Game aborts with a visible error. It does not
restore a prior slot and does not silently fall back to a new cohort.

## Restore rules

- The selected slot's `campaign.json` is authoritative.
- Existing survivor slices, roster membership, death state, needs, and dose
  always beat current catalog defaults.
- A saved zero-survivor list remains zero survivors.
- Failed/corrupt restore leaves the live campaign intact and never enters
  `FreshInitialize`.
- `ResetAllSessionsInMemory()` never deletes persisted campaign data.
