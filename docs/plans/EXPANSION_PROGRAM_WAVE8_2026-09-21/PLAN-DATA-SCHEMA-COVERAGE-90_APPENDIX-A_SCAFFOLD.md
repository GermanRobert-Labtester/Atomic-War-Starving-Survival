# PLAN-DATA-SCHEMA-COVERAGE-90 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-DATA-AUTHORITY-14`](../EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-DATA-AUTHORITY-14.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (5 files, Core + host)

| File | Lines |
|---|---:|
| `IO/CatalogBootValidator.cs` | 339 |
| `IO/CatalogDiagnostics.cs` | 71 |
| `IO/CatalogKeyNormalizer.cs` | 200 |
| `IO/CatalogLoadResult.cs` | 351 |
| `Save/SchemaVersionedEnvelope.cs` | 61 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `mod_manifest_schema.json` | object(9 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupIODS2Scaffold` / `SaveIODS2Scaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/IO/DS290ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class DS290ScaffoldTests
{
    [Fact] public void DSC90A_TODO() { /* triage table: consumed catalogs ranked; first-wave schema list (bounded, e.g. 15 */ }
    [Fact] public void DSC90B_TODO() { /* schemas: authored under `Data/schemas/` with the established naming; one PR per  */ }
    [Fact] public void DSC90C_TODO() { /* validator stage: extend the current integrity entry point; report file + path +  */ }
    [Fact] public void DSC90D_TODO() { /* drift gate: unknown/missing field diff per catalog; catalog edits that break sha */ }
    [Fact] public void DSC90E_TODO() { /* docs: version rule + authoring example; index of covered vs pending catalogs. */ }
    [Fact] public void DS290_AuthorityConformance_TODO() { /* pattern parity with PLAN-DATA-AUTHORITY-14 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/IO/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
