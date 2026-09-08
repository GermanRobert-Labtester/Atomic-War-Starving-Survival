# Independent Branch Reachability Matrix

| Branch ID | Access Mode | Preconditions | PoNR Producer | Ending Resolver | Reachable in Live Campaign? |
|---|---|---|---|---|---|
| `branch_ind_9_hermit` | Generic Selection / UI Option | Moral band: `very_evil`..`very_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |
| `branch_ind_10_mediator` | Generic Selection / UI Option | Moral band: `slightly_evil`..`very_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |
| `branch_ind_11_scavenger_king` | Generic Selection / UI Option | Moral band: `very_evil`..`slightly_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |
| `branch_ind_12_caretaker` | Generic Selection / UI Option | Moral band: `neutral`..`very_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |
| `branch_ind_13_witness` | Generic Selection / UI Option | Moral band: `slightly_evil`..`very_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |
| `branch_ind_14_engineer` | Generic Selection / UI Option | Moral band: `very_evil`..`very_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |
| `branch_ind_15_prophet` | Generic Selection / UI Option | Moral band: `very_evil`..`very_positive` | `LockPointOfNoReturn` | `ResolveEnding` | **Yes (CATALOG_ELIGIBLE & FULLY_REACHABLE)** |

## Reachability Path
`FactionBranchCoordinator.GetBranchOptions(moralChoice)` enumerates all branches from `IndependentBranchCatalog`.
When player moral choice satisfies entry bounds:
1. `CanCommit` returns true.
2. Player commits via `CommitBranch`.
3. Event or threshold locks PoNR via `LockPointOfNoReturn`.
4. Climax / epilogue triggers `ResolveEnding`.
