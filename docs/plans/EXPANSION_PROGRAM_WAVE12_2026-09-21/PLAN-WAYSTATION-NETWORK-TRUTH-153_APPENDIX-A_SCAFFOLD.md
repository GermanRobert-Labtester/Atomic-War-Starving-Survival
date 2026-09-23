# PLAN-WAYSTATION-NETWORK-TRUTH-153 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-TRANSPORT-EXPEDITION-30`](../EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-TRANSPORT-EXPEDITION-30.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (6 files, Core + host)

| File | Lines |
|---|---:|
| `Waystation/WaystationCatalogLoader.cs` | 165 |
| `Waystation/WaystationNetworkSystem.cs` | 201 |
| `WaystationSystem.cs` | 208 |
| `host:Host/WaystationHostSession.cs` | 107 |
| `host:Host/WaystationSaveStore.cs` | 57 |
| `host:UI/WaystationNetworkPanel.cs` | 186 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `waystations.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupWaystationWNScaffold` / `SaveWaystationWNScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Waystation/WN153ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class WN153ScaffoldTests
{
    [Fact] public void WNT153A_TODO() { /* node model + service table (catalog-backed). */ }
    [Fact] public void WNT153B_TODO() { /* network attachment to Plan 95 route edges + no-orphan-node check. */ }
    [Fact] public void WNT153C_TODO() { /* status transition table + event/day tests. */ }
    [Fact] public void WNT153D_TODO() { /* service effect routing tests (needs/maintenance/economy owners). */ }
    [Fact] public void WNT153E_TODO() { /* persistence round-trip; no re-seed on load. */ }
    [Fact] public void WN153_AuthorityConformance_TODO() { /* pattern parity with PLAN-TRANSPORT-EXPEDITION-30 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Waystation/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
