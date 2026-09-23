# PLAN-BOOTSTRAP-GATE-TRUTH-147 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-PROGRAMME-CLOSEOUT-100`](../EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-PROGRAMME-CLOSEOUT-100.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `PCL-100A` | index generator (script + `--check`); README tables replaced by generated output. |
| `PCL-100B` | promotion ledger template + first fill from a read-only claim snapshot. |
| `PCL-100C` | claim cross-check report. |

## 2. Source inventory (10 files, Core + host)

| File | Lines |
|---|---:|
| `Assets/AssetManifest.cs` | 461 |
| `Content/ContentUtilizationManifest.cs` | 227 |
| `Launch/StoreCapabilityManifest.cs` | 194 |
| `Orchestration/BootstrapLifecycleGate.cs` | 128 |
| `Orchestration/LedgerTruthIntegrityGate.cs` | 137 |
| `Orchestration/SubsystemManifest.cs` | 314 |
| `UI/PanelRegistryBootstrap.cs` | 274 |
| `UI/PlayerSurfaceManifest.cs` | 205 |
| `UI/UiAssetManifest.cs` | 63 |
| `host:Host/HostCli.SelfTestManifest.cs` | 36 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `narrative_discovery_manifest.json` | 243 |
| `mod_manifest_schema.json` | object(9 keys) |
| `armored_locomotive_manifests.json` | 7 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupOrchestrationBGScaffold` / `SaveOrchestrationBGScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Orchestration/BG147ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class BG147ScaffoldTests
{
    [Fact] public void BGT147A_TODO() { /* coverage report generator (manifest ↔ setups) with `--check`. */ }
    [Fact] public void BGT147B_TODO() { /* phase-truth table: entry phase vs actual invocation order. */ }
    [Fact] public void BGT147C_TODO() { /* ledger-gate semantics doc + a failure fixture per ledger class. */ }
    [Fact] public void BGT147D_TODO() { /* bootstrap precondition doc + partial-bootstrap failure test. */ }
    [Fact] public void BGT147E_TODO() { /* CI integration note for Plan 86's manifest verb. */ }
    [Fact] public void BG147_AuthorityConformance_TODO() { /* pattern parity with PLAN-PROGRAMME-CLOSEOUT-100 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
