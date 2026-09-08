# Point-of-No-Return Trigger Matrix (Plan 121)

| Branch ID | PoNR Flag ID | PoNR Trigger Sentence | Idempotent? | Runtime & Save Persistence |
|---|---|---|---|---|
| `branch_ind_9_hermit` | `flag_branch_ind_9_ponr` | They turn away the final delegation and commit to living beyond every settlement's protection. | Yes | `setFlags` list + `IFlagLedger` |
| `branch_ind_10_mediator` | `flag_branch_ind_10_ponr` | They sign their name to a negotiated partition that sacrifices one outpost to preserve the peace of three others. | Yes | `setFlags` list + `IFlagLedger` |
| `branch_ind_11_scavenger_king` | `flag_branch_ind_11_ponr` | They seal their primary supply cache against communal commandeering, declaring that access requires individual barter contracts. | Yes | `setFlags` list + `IFlagLedger` |
| `branch_ind_12_caretaker` | `flag_branch_ind_12_ponr` | They defy the exclusion order and bring three contaminated orphans into their own quarters under personal bond. | Yes | `setFlags` list + `IFlagLedger` |
| `branch_ind_13_witness` | `flag_branch_ind_13_ponr` | They post the unredacted ration ledger on the commons wall, exposing the rationing council's hidden diversion. | Yes | `setFlags` list + `IFlagLedger` |
| `branch_ind_14_engineer` | `flag_branch_ind_14_ponr` | They cut auxiliary steam to the lower barracks to ensure the main generator core survives the blizzard. | Yes | `setFlags` list + `IFlagLedger` |
| `branch_ind_15_prophet` | `flag_branch_ind_15_ponr` | They climb the water tower during the black rain and proclaim the storm a judgment that demands public penance. | Yes | `setFlags` list + `IFlagLedger` |

## Idempotence Guarantee
1. `IndependentBranchSystem.LockPointOfNoReturn()` sets `ponrLocked = true` and `ponrLockedDay = currentDay`.
2. Any subsequent calls when `ponrLocked == true` immediately return with zero side effects.
3. The flag is added once to `setFlags` list (which persists in `IndependentBranchSave`) and set in the runtime `IFlagLedger`.
4. Restore replays level state (`_flags.Set(flagId)`) without re-firing edge transition events (`OnPonrLocked`).
