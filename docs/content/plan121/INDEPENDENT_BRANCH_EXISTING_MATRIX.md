# Existing Eight-Branch Inventory Matrix

| Index | Branch ID | Display Name | PoNR Flag | Entry Min | Entry Max | Endings (3 per branch) |
|---|---|---|---|---|---|---|
| 0 | `branch_ind_1_survivor` | The Survivor | `flag_branch_ind_1_ponr` | `very_evil` | `very_positive` | 1a: `lone_survivor` (neutral..slightly_positive)<br>1b: `traitor` (very_evil..evil)<br>1c: `legend` (positive..very_positive) |
| 1 | `branch_ind_2_mercenary` | The Mercenary | `flag_branch_ind_2_ponr` | `very_evil` | `very_positive` | 2a: `warlord` (very_evil..evil)<br>2b: `survivor` (slightly_evil..slightly_positive)<br>2c: `legend` (positive..very_positive) |
| 2 | `branch_ind_3_peacekeeper_diplomat` | The Peacekeeper / Diplomat | `flag_branch_ind_3_ponr` | `positive` | `very_positive` | 3a: `peacekeeper_unifier` (very_positive..very_positive)<br>3b: `diplomat` (positive..positive)<br>3c: `survivor` (slightly_positive..slightly_positive) |
| 3 | `branch_ind_4_exile` | The Exile | `flag_branch_ind_4_ponr` | `very_evil` | `evil` | 4a: `tyrant` (very_evil..very_evil)<br>4b: `ghost` (slightly_evil..evil)<br>4c: `traitor` (neutral..neutral) |
| 4 | `branch_ind_5_kingmaker` | The Kingmaker | `flag_branch_ind_5_ponr` | `very_evil` | `very_positive` | 5a: `puppet_master` (very_evil..evil)<br>5b: `survivor` (slightly_evil..slightly_positive)<br>5c: `unifier` (positive..very_positive) |
| 5 | `branch_ind_6_legend` | The Legend | `flag_branch_ind_6_ponr` | `very_evil` | `very_positive` | 6a: `savior` (positive..very_positive)<br>6b: `monster` (very_evil..evil)<br>6c: `myth` (slightly_evil..slightly_positive) |
| 6 | `branch_ind_7_ghost` | The Ghost | `flag_branch_ind_7_ponr` | `very_evil` | `very_positive` | 7a: `unseen` (neutral..slightly_positive)<br>7b: `forgotten` (very_evil..slightly_evil)<br>7c: `watcher` (positive..very_positive) |
| 7 | `branch_ind_8_wasteland_myth` | The Wasteland Myth | `flag_branch_ind_8_ponr` | `very_evil` | `very_positive` | 8a: `feared_legend` (very_evil..evil)<br>8b: `revered_legend` (positive..very_positive)<br>8c: `forgotten_legend` (slightly_evil..slightly_positive) |

## Invariant Check
- All 8 branches preserved byte-for-byte in position 0 through 7.
- Zero renumbering or modification of legacy branch IDs, PoNR flags, or ending IDs.
