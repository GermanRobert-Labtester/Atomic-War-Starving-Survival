# Plan 121: Regression Matrix & Risk Mitigation

## 1. Risk Resolution Matrix

| Risk ID | Description | Severity | Mitigation & Verification | Result |
|---|---|---|---|---|
| **R1** | Static ID registry rejects new branches | Medium | Classify `IndependentBranchIds.cs` as Case A; update constants and switch. | **RESOLVED** (0 errors) |
| **R2** | Candidate IDs collide with existing 8 | High | Sequenced new branches as 9–15 (`branch_ind_9`..`15`). | **RESOLVED** (0 collisions) |
| **R3** | Existing saves persist array indexes | High | Verified saves persist string IDs, not ordinals. | **RESOLVED** (Safe) |
| **R4** | PoNR trigger treated as executable | High | Kept PoNR trigger text as descriptive narrative prose. | **RESOLVED** (No mini-DSL) |
| **R5** | PoNR flag unregistered / invalid | Medium | Used standard `flag_branch_ind_*` prefix registered in `CatalogIntegrityValidator`. | **RESOLVED** (0 findings) |
| **R6** | Ending ranges overlap ambiguously | High | Structured exact non-overlapping 3-piece partitions across all 7 bands. | **RESOLVED** (0 overlaps) |
| **R7** | Ending ranges leave gaps | High | Covered all 7 bands in every new branch (`very_evil`..`very_positive`). | **RESOLVED** (0 gaps) |
| **R8** | Moral-band ordering guessed incorrectly | High | Aligned with `MoralChoiceSystem.BandForScore` and `MoralPathBand` enum ordinals. | **RESOLVED** (Verified) |
| **R9** | Pool balance distortion | Medium | 11–13 eligible branches per band across all 7 bands. | **RESOLVED** (Balanced) |
| **R10** | New branches unreachable | High | Enumerate automatically in `FactionBranchCoordinator.GetBranchOptions`. | **RESOLVED** (Reachable) |
| **R24** | Ending ID collision | High | Global grep confirms 0 duplicate ending IDs across all catalogs. | **RESOLVED** (Unique) |
| **R25** | Baseline 8 rebalanced | High | Baseline branches 1–8 kept byte-for-byte in positions 0..7. | **RESOLVED** (Parity kept) |
| **R26** | Hardcoded catalog count | Medium | Updated `FactionBranchCoordinatorTests` to use `BranchCount` constants. | **RESOLVED** (17/17 passed) |

---

## 2. Full Regression Suite Execution

| Test Suite | Tests Run | Passed | Failed |
|---|---|---|---|
| `IndependentBranchExpansionTests` | 24 | 24 | 0 |
| `IndependentBranchSystemTests` | 18 | 18 | 0 |
| `IndependentBranchCatalogTests` | 3 | 3 | 0 |
| `FactionBranchCoordinatorTests` | 17 | 17 | 0 |
| Full Workspace Suite (`Ashfall.Core.Tests`) | 9,949 | 9,949 | 0 |
