# PLAN-PORT-CONTRACT-TRUTH-157 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140`](../EXPANSION_PROGRAM_WAVE11_2026-09-21/PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `AMC-140A` | contract inventory + member table. |
| `AMC-140B` | consumer map with plan ids (realizer + caller). |
| `AMC-140C` | test double implementing the contract + a fixture call path. |

## 2. Source inventory (10 files, Core + host)

| File | Lines |
|---|---:|
| `Campaign/DailyBriefingReportBuilder.cs` | 743 |
| `Inventory/IPlayerInventoryPort.cs` | 24 |
| `Ports.cs` | 121 |
| `Ports/PortContract.cs` | 135 |
| `ShelterMachineryReport.cs` | 140 |
| `VersionReport.cs` | 294 |
| `host:Host/AssetCoverageReport.cs` | 94 |
| `host:Host/HostCli.ExportParity.cs` | 236 |
| `host:Host/PortContractSelfTest.cs` | 181 |
| `host:UI/AutopsyReportPanel.cs` | 117 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `ammo_hoist_jam_reports.json` | 8 |
| `aramid_fiber_rot_reports.json` | 8 |
| `biochar_cation_exchange_reports.json` | 7 |
| `bolting_silk_mesh_reports.json` | 8 |
| `brain_tanning_hide_reports.json` | 8 |
| `bullet_alloy_assay_reports.json` | 7 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupPortsPCScaffold` / `SavePortsPCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Ports/PC157ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class PC157ScaffoldTests
{
    [Fact] public void PCT157A_TODO() { /* contract inventory + obligations. */ }
    [Fact] public void PCT157B_TODO() { /* realizer/consumer map with plan ids. */ }
    [Fact] public void PCT157C_TODO() { /* test double + smoke call. */ }
    [Fact] public void PCT157D_TODO() { /* overlap report against 27/95/153 concepts. */ }
    [Fact] public void PCT157E_TODO() { /* adopt/retire decisions with owners. */ }
    [Fact] public void PC157_AuthorityConformance_TODO() { /* pattern parity with PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Ports/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
