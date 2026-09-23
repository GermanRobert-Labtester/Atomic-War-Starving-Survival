# PLAN-CATALOG-BOOT-TRUTH-148 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-DATA-SCHEMA-COVERAGE-90`](../EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-DATA-SCHEMA-COVERAGE-90.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `DSC-90A` | triage table: consumed catalogs ranked; first-wave schema list (bounded, e.g. 15). |
| `DSC-90B` | schemas: authored under `Data/schemas/` with the established naming; one PR per catalog family. |
| `DSC-90C` | validator stage: extend the current integrity entry point; report file + path + expected type. |

## 2. Source inventory (10 files, Core + host)

| File | Lines |
|---|---:|
| `IO/CatalogBootValidator.cs` | 339 |
| `IO/CatalogDiagnostics.cs` | 71 |
| `IO/CatalogKeyNormalizer.cs` | 200 |
| `IO/CatalogLoadResult.cs` | 351 |
| `Medical/MicrofluidicDiagnosticCatalogLoader.cs` | 107 |
| `Medical/MicrofluidicDiagnosticEngine.cs` | 609 |
| `host:Host/MicrofluidicDiagnosticHostSession.cs` | 98 |
| `host:Host/MicrofluidicDiagnosticSaveStore.cs` | 32 |
| `host:UI/MicrofluidicDiagnosticPanel.cs` | 401 |
| `host:UI/UiNodeDiagnostics.cs` | 145 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `microfluidic_diagnostic_catalog.json` | object(4 keys) |
| `geothermal_steam_vent_diagnostics.json` | 7 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupIOCBScaffold` / `SaveIOCBScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/IO/CB148ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class CB148ScaffoldTests
{
    [Fact] public void CBT148A_TODO() { /* normalizer rule table + fixtures (case/separator/alias/unicode). */ }
    [Fact] public void CBT148B_TODO() { /* load-result class contract + one test per class. */ }
    [Fact] public void CBT148C_TODO() { /* diagnostics field contract + a boot-failure fixture carrying all fields. */ }
    [Fact] public void CBT148D_TODO() { /* boot policy table (required/optional per catalog class) + degradation tests. */ }
    [Fact] public void CBT148E_TODO() { /* integration note: schema failure is a result class, not an exception path. */ }
    [Fact] public void CB148_AuthorityConformance_TODO() { /* pattern parity with PLAN-DATA-SCHEMA-COVERAGE-90 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/IO/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
