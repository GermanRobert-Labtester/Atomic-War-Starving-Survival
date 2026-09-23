# PLAN-DEPRECATED-TREE-RETIREMENT-94 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ORPHAN-SEAL-01`](../EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (0 files, Core + host)

| File | Lines |
|---|---:|

## 3b. Deprecation surface

| Asset | Detail |
|---|---|
| `Assets/_Game/` files | 4: `NoiseDisciplineSystem.cs`, `NoiseDisciplineSystem.cs.uid`, `BunkerSocialSystems.cs`, `BunkerSocialSystems.cs.uid` |
| Non-comment Unity refs (repo) | 3 |
| Game csproj globs | 2 |
| Test csproj `_Game` include | yes |
| `src/Bridge/` | absent |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupPerformanceDRScaffold` / `SavePerformanceDRScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Performance/DR94ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class DR94ScaffoldTests
{
    [Fact] public void DTR94A_TODO() { /* reference map: who compiles/uses each `_Game` file (this table, verified in the  */ }
    [Fact] public void DTR94B_TODO() { /* port-or-retire: smallest engine-free port of the noise benchmark, or retire with */ }
    [Fact] public void DTR94C_TODO() { /* csproj cleanup: remove the link in the same change that lands the port. */ }
    [Fact] public void DTR94D_TODO() { /* gate: extend the invariant source test; fail on new `UNITY_*` symbols or new `_G */ }
    [Fact] public void DTR94E_TODO() { /* marker policy note in the architecture doc (`docs/CURRENT_AUTHORITY.md` or its s */ }
    [Fact] public void DR94_AuthorityConformance_TODO() { /* pattern parity with PLAN-ORPHAN-SEAL-01 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Performance/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
