# PLAN-PROGRAMME-CLOSEOUT-100 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-AGENT-WORKFLOW-GOVERNANCE-59`](../EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-AGENT-WORKFLOW-GOVERNANCE-59.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `AG-59A` | rulebook generation: section-level canonical sources + template rendering; `--check` fails on drift. |
| `AG-59B` | stale-reference gate: references to plan/debt/register ids must exist; AGENTS.md queue lists generated from `I |
| `AG-59C` | skills hygiene: validate each SKILL.md (frontmatter, description, location), retire dead ones, regenerate cata |

## 2. Source inventory (19 files, Core + host)

| File | Lines |
|---|---:|
| `DoseLedgerSave.cs` | 164 |
| `DoseLedgerSystem.cs` | 333 |
| `DutyRoster/DutyHourLedger.cs` | 126 |
| `Economy/FundsLedger.cs` | 227 |
| `FactionEmbargoLedger.cs` | 150 |
| `Flags/CampaignConsequenceLedger.cs` | 301 |
| `Flags/IFlagLedger.cs` | 64 |
| `Institutions/InstitutionAssignmentLedger.cs` | 57 |
| `LedgerDebtHeadlessDemo.cs` | 337 |
| `LedgerDebtSystem.cs` | 371 |
| `Medical/MedicalReservationLedger.cs` | 185 |
| `Orchestration/LedgerTruthIntegrityGate.cs` | 137 |
| `Radio/SignalTrustLedger.cs` | 179 |
| `Verdict/EvidenceLedger.cs` | 112 |
| `host:Host/DoseLedgerHostSession.cs` | 267 |
| `host:Host/DoseLedgerSaveStore.cs` | 59 |
| `host:UI/CaravanBarterLedgerPanel.cs` | 274 |
| `host:UI/DoseLedgerPanel.cs` | 452 |
| `host:UI/SubterraneanDebtLedgerPanel.cs` | 101 |

## 3b. Programme self-counts

| Metric | Value |
|---|---:|
| Programme directories | 19 |
| Plan files | 276 |
| Appendix files | 114 |
| Versioned generators | 16 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `ledger_debt_templates.json` | object(3 keys) |
| `bunker_trade_ledger_batch_2.json` | object(4 keys) |
| `trade_ledgers_expansion.json` | object(4 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupGovernancePCScaffold` / `SaveGovernancePCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Governance/PC100ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class PC100ScaffoldTests
{
    [Fact] public void PCL100A_TODO() { /* index generator (script + `--check`); README tables replaced by generated output */ }
    [Fact] public void PCL100B_TODO() { /* promotion ledger template + first fill from a read-only claim snapshot. */ }
    [Fact] public void PCL100C_TODO() { /* claim cross-check report. */ }
    [Fact] public void PCL100D_TODO() { /* verification-command audit (flags/scripts existence, no execution of broad suite */ }
    [Fact] public void PCL100E_TODO() { /* sunset criteria + final programme summary for the foreman. */ }
    [Fact] public void PC100_AuthorityConformance_TODO() { /* pattern parity with PLAN-AGENT-WORKFLOW-GOVERNANCE-59 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Governance/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
