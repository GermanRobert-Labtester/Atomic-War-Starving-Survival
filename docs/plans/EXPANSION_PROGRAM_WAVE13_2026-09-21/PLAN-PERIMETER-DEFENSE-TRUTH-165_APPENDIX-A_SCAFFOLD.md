# PLAN-PERIMETER-DEFENSE-TRUTH-165 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-BASE-DEFENSE-RAIDS-61`](../EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-BASE-DEFENSE-RAIDS-61.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `BD-61A` | warning net: sensors + watch produce warnings with time-to-contact and a false-alarm band; false alarms have a |
| `BD-61B` | watch/muster: readiness score drives response; overwork and low fitness reduce it (duty fitness owner). |
| `BD-61C` | raid resolution: attackers use doctrine/faction state; defense uses readiness + fortification + air support; o |

## 2. Source inventory (5 files, Core + host)

| File | Lines |
|---|---:|
| `Defense/DefenseSystem.cs` | 571 |
| `Defense/PerimeterDefenseCatalog.cs` | 152 |
| `Defense/PerimeterDefenseSystem.cs` | 796 |
| `Defense/PerimeterEarlyWarningEngine.cs` | 280 |
| `host:Host/PerimeterDefenseSaveStore.cs` | 38 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `perimeter_defenses.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupDefensePDScaffold` / `SaveDefensePDScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Defense/PD165ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class PD165ScaffoldTests
{
    [Fact] public void PDT165A_TODO() { /* segment model + condition source (built state). */ }
    [Fact] public void PDT165B_TODO() { /* gate policy states + Plan 155 read check. */ }
    [Fact] public void PDT165C_TODO() { /* approach coverage table + uncovered-approach visibility test. */ }
    [Fact] public void PDT165D_TODO() { /* breach threshold + list hand-off test to Plan 61. */ }
    [Fact] public void PDT165E_TODO() { /* save round-trip; no silent repair/damage on load. */ }
    [Fact] public void PD165_AuthorityConformance_TODO() { /* pattern parity with PLAN-BASE-DEFENSE-RAIDS-61 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Defense/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
