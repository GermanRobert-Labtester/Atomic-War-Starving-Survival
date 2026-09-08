# Independent Branch ID Authority Classification

## 1. Classification: Case A (Authoritative Code-Pinned Whitelist)

`Assets/Ashfall.Core/Factions/IndependentBranchIds.cs` was inspected to classify its architectural role:

- **Case A — Authoritative Whitelist & Lookup Contract [CONFIRMED]**
  - `IndependentBranchSystem.LockPointOfNoReturn()` executes:
    ```csharp
    string flagId = IndependentBranchIds.PonrFlagFor(_state.branch.branchId);
    ```
    Where `PonrFlagFor(branchId)` throws `ArgumentException($"Unknown Independent branch id '{branchId}'.")` if the branch is not matched in the switch statement.
  - `IndependentBranchIds.BranchCount` is defined as a constant and verified by `IndependentBranchCatalogTests` and `IndependentBranchSystemTests`.
  - `IndependentBranchIds.AllBranches` is an array of all registered branches, iterated by cross-catalog uniqueness checks and catalog completeness checks.

---

## 2. Evidence & Scope Boundary Decision

- Per §0 and §22 Outcome 2 of Plan 121, this required a **mechanical Core registry update** in `IndependentBranchIds.cs`:
  - `BranchCount` updated from `8` to `15`.
  - Constants added for branches 9 through 15 (`BranchHermit`, `BranchMediator`, `BranchScavengerKing`, `BranchCaretaker`, `BranchWitness`, `BranchEngineer`, `BranchProphet`).
  - Added to `AllBranches`.
  - Added PoNR flag constants (`FlagPonrHermit` through `FlagPonrProphet`).
  - Added switch cases in `PonrFlagFor(branchId)`.
  - Added ending ID constants (`EndingHermitA/B/C` through `EndingProphetA/B/C`).
- Zero new runtime gameplay mechanics or execution logic were introduced into Core; the edit was strictly structural typing and identity mapping.
