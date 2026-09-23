# PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-PORT-CONTRACT-TRUTH-157`](../EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-PORT-CONTRACT-TRUTH-157.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `PCT-157A` | contract inventory + obligations. |
| `PCT-157B` | realizer/consumer map with plan ids. |
| `PCT-157C` | test double + smoke call. |

## 2. Source inventory (1 files, Core + host)

| File | Lines |
|---|---:|
| `AdvancedMachinery/AdvancedMachineContracts.cs` | 101 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupAdvancedMachineryAM2Scaffold` / `SaveAdvancedMachineryAM2Scaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/AdvancedMachinery/AM2140ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class AM2140ScaffoldTests
{
    [Fact] public void AMC140A_TODO() { /* contract inventory + member table. */ }
    [Fact] public void AMC140B_TODO() { /* consumer map with plan ids (realizer + caller). */ }
    [Fact] public void AMC140C_TODO() { /* test double implementing the contract + a fixture call path. */ }
    [Fact] public void AMC140D_TODO() { /* overlap report against existing owners (no silent duplicates). */ }
    [Fact] public void AMC140E_TODO() { /* retirement/freeze decisions for unconsumed contracts. */ }
    [Fact] public void AM2140_AuthorityConformance_TODO() { /* pattern parity with PLAN-PORT-CONTRACT-TRUTH-157 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/AdvancedMachinery/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
