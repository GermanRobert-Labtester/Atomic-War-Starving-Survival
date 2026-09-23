# PLAN-PROPAGANDA-TRUTH-150 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-RUMOR-PROPAGATION-TRUTH-120`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-RUMOR-PROPAGATION-TRUTH-120.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `RMT-120A` | origin ledger + creation hook at the event seam. |
| `RMT-120B` | propagation rules + per-route tests (visit, trade, radio). |
| `RMT-120C` | distortion ladder with seeded selection + same-seed equality test. |

## 2. Source inventory (6 files, Core + host)

| File | Lines |
|---|---:|
| `Propaganda/PropagandaSystem.cs` | 507 |
| `host:Host/PropagandaHostSession.cs` | 93 |
| `host:Host/PropagandaSaveStore.cs` | 31 |
| `host:Host/PropagandaSelfTest.cs` | 170 |
| `host:Main.Propaganda.cs` | 77 |
| `host:UI/PropagandaPanel.cs` | 227 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `propaganda_campaigns.json` | object(3 keys) |
| `propaganda_templates.json` | object(2 keys) |
| `stencil_propaganda_smear_logs.json` | 7 |

## 4. Host attachment

- Candidate host partials: `Main.Propaganda.cs`
- Proposed method names: `SetupPropagandaPRScaffold` / `SavePropagandaPRScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Propaganda/PR150ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class PR150ScaffoldTests
{
    [Fact] public void PRA150A_TODO() { /* action table (audience, medium, cost, cooldown). */ }
    [Fact] public void PRA150B_TODO() { /* reach model reading existing population holders. */ }
    [Fact] public void PRA150C_TODO() { /* effect routing tests into Plan 129/69 owners (no private score). */ }
    [Fact] public void PRA150D_TODO() { /* blowback rules + contradiction fixture (known fact blocks or backfires). */ }
    [Fact] public void PRA150E_TODO() { /* determinism + cooldown/expiry on the canonical clock. */ }
    [Fact] public void PR150_AuthorityConformance_TODO() { /* pattern parity with PLAN-RUMOR-PROPAGATION-TRUTH-120 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Propaganda/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
