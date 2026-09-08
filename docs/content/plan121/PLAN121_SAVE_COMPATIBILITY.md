# Plan 121: Save Compatibility & Persistence Contract

## 1. Save Contract Architecture
- Save model: `IndependentBranchSave` with schemaVersion = 1 and reflection-based `Checksum` via `SaveChecksum.Compute`.
- Codec: `IndependentBranchSaveCodec` (`Capture`, `Restore`, `Encode`, `Decode`).
- Mutable state stored:
  - `branch.branchId` (string ID of committed branch)
  - `branch.committed` (boolean)
  - `branch.ponrLocked` (boolean)
  - `branch.ponrLockedDay` (int)
  - `branch.resolvedEndingId` (string ID of resolved ending)
  - `setFlags` (`List<string>` of durable PoNR flags)
  - `militaryStanding` and `rebelStanding` records

## 2. Invariants Preserved
1. **No Ordinal / Index Serialization:** Saves persist string IDs (`branchId`, `resolvedEndingId`, `setFlags`), never raw integer array indices. Appending new branches cannot corrupt legacy index offsets.
2. **Old Save Compatibility:** Older campaign saves with branches 1–8 or no committed branch restore cleanly without errors or data mutation.
3. **New Branch Persistence:** Committing new branches (e.g., `branch_ind_9_hermit` or `branch_ind_13_witness`), advancing days, locking PoNR, and resolving endings round-trip with full checksum fidelity.
4. **Tamper Detection:** Mutating any serialized field causes `SaveChecksum` validation to throw `InvalidOperationException` on decode.

## 3. Verification Evidence
- `SaveRoundTrip_NewBranch_Hermit_PreservesState`: PASS
- `SaveRoundTrip_NewBranch_Witness_PreservesState`: PASS
- `LegacySave_SimulatedOldState_RestoresCleanlyWithoutCorruption`: PASS
- `Decode_TamperedChecksum_Throws`: PASS
- `RestoreState_WrongSystemId_Throws`: PASS
