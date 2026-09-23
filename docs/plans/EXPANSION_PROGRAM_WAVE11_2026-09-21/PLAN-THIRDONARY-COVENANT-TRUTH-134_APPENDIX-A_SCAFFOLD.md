# PLAN-THIRDONARY-COVENANT-TRUTH-134 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-JUSTICE-LAW-37`](../EXPANSION_PROGRAM_WAVE4_2026-09-21/PLAN-JUSTICE-LAW-37.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (7 files, Core + host)

| File | Lines |
|---|---:|
| `Crossing/CrossingThirdonaryIntegration.cs` | 170 |
| `Thirdonary/ThirdonaryCatalogLoader.cs` | 48 |
| `Thirdonary/ThirdonaryQuestSystem.cs` | 296 |
| `Thirdonary/ThirdonarySave.cs` | 57 |
| `Thirdonary/ThirdonaryTypes.cs` | 105 |
| `host:Host/ThirdonaryHostSession.cs` | 105 |
| `host:Host/ThirdonarySaveStore.cs` | 58 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `thirdonary_quests.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupThirdonaryTCScaffold` / `SaveThirdonaryTCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Thirdonary/TC134ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class TC134ScaffoldTests
{
    [Fact] public void TCT134A_TODO() { /* lifecycle document + transition table. */ }
    [Fact] public void TCT134B_TODO() { /* dispute model + resolution path tests (legal vs covenant terms). */ }
    [Fact] public void TCT134C_TODO() { /* catalog binding tests (unknown id fails; all terms resolve). */ }
    [Fact] public void TCT134D_TODO() { /* save round-trip incl. mid-dispute state. */ }
    [Fact] public void TCT134E_TODO() { /* standing-consequence wiring to its existing owner. */ }
    [Fact] public void TC134_AuthorityConformance_TODO() { /* pattern parity with PLAN-JUSTICE-LAW-37 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Thirdonary/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
