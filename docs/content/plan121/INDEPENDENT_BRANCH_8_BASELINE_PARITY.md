# Independent Branch Eight-Baseline Parity Verification

## 1. Baseline Preservation
All 8 original branches (`branch_ind_1_survivor` through `branch_ind_8_wasteland_myth`) remain in positions 0 through 7:
- All 8 branch IDs unchanged.
- All 8 display names unchanged.
- All 8 PoNR flags unchanged.
- All 8 PoNR trigger text unchanged.
- All 8 entry band min/max ranges unchanged.
- All 24 baseline ending IDs, ranges, and display names unchanged.

## 2. Test Verification
`Ashfall.Core.Tests.IndependentBranchExpansionTests.BaselineEightBranches_PreservedVerbatim_WithExactOrderAndData` asserts:
- Position 0..7 match original IDs and display names.
- Each branch has exactly 3 endings.
- All 21 original `IndependentBranch` tests pass with zero modifications.
