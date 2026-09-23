# PLAN-ARCHAEOLOGY-TRUTH-152 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ANCIENT-RUINS-VAULTS-84`](../EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-ANCIENT-RUINS-VAULTS-84.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `RV-84A` | site model: authored multi-room complexes with state (power, doors, hazards) and a map marker revealed by surv |
| `RV-84B` | entry puzzles: codes, power, breaching; tools and skills matter; failure is loud/costly. |
| `RV-84C` | interior hazards: authored per site, using canonical hazard owners. |

## 2. Source inventory (5 files, Core + host)

| File | Lines |
|---|---:|
| `Archaeology/ArchaeologySystem.cs` | 317 |
| `Medical/PalliativeCareDignityEngine.cs` | 254 |
| `host:Host/ArchaeologySaveStore.cs` | 32 |
| `host:UI/AnaerobicBiogasDigesterPanel.cs` | 131 |
| `host:UI/ArchaeologyExcavationPanel.cs` | 222 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupArchaeologyAXScaffold` / `SaveArchaeologyAXScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Archaeology/AX152ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class AX152ScaffoldTests
{
    [Fact] public void ARC152A_TODO() { /* lifecycle + stage clock table. */ }
    [Fact] public void ARC152B_TODO() { /* strata/hazard table + supply consumption per stage. */ }
    [Fact] public void ARC152C_TODO() { /* find provenance record + display-from-record test. */ }
    [Fact] public void ARC152D_TODO() { /* depletion model + no-re-loot test; conservation check. */ }
    [Fact] public void ARC152E_TODO() { /* abandonment state + revisit visibility test. */ }
    [Fact] public void AX152_AuthorityConformance_TODO() { /* pattern parity with PLAN-ANCIENT-RUINS-VAULTS-84 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
