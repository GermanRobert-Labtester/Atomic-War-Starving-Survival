# PLAN-INSTITUTIONS-TRUTH-141 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-SURVIVOR-ROSTER-TRUTH-244`](../EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-SURVIVOR-ROSTER-TRUTH-244.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `SRT-244A` | status model + setter table. |
| `SRT-244B` | availability derivation tests (no double storage). |
| `SRT-244C` | transfer hook + move records. |

## 2. Source inventory (8 files, Core + host)

| File | Lines |
|---|---:|
| `Catalogs/InstitutionCatalogParse.cs` | 140 |
| `DutyRoster/DutyRosterAssignmentEngine.cs` | 320 |
| `Institutions/IInstitutionAvailability.cs` | 40 |
| `Institutions/InstitutionAssignmentLedger.cs` | 57 |
| `Shelter/ShelterAssignmentSave.cs` | 80 |
| `Shelter/ShelterAssignmentSystem.cs` | 293 |
| `host:Host/ShelterAssignmentHostSession.cs` | 183 |
| `host:Main.FlagshipInstitutions.cs` | 580 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: `Main.FlagshipInstitutions.cs`
- Proposed method names: `SetupInstitutionsITScaffold` / `SaveInstitutionsITScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Institutions/IT141ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class IT141ScaffoldTests
{
    [Fact] public void INT141A_TODO() { /* availability predicate table per institution. */ }
    [Fact] public void INT141B_TODO() { /* lifecycle + one-holder-per-berth enforcement tests. */ }
    [Fact] public void INT141C_TODO() { /* vacating path tests (death/injury/removal/resign/reassign). */ }
    [Fact] public void INT141D_TODO() { /* ledger-only rule + panel read check. */ }
    [Fact] public void INT141E_TODO() { /* persistence round-trip + no re-nomination on load. */ }
    [Fact] public void IT141_AuthorityConformance_TODO() { /* pattern parity with PLAN-SURVIVOR-ROSTER-TRUTH-244 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Institutions/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
