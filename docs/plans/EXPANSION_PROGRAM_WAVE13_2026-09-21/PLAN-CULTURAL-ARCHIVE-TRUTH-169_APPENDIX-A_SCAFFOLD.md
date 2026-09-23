# PLAN-CULTURAL-ARCHIVE-TRUTH-169 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-LORE-ARCHIVE-TRUTH-238`](../EXPANSION_PROGRAM_WAVE17_2026-09-21/PLAN-LORE-ARCHIVE-TRUTH-238.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `LAT-238A` | performance model + transmission rules. |
| `LAT-238B` | fidelity/degradation fixtures (seeded). |
| `LAT-238C` | loss-on-death rule + visibility test. |

## 2. Source inventory (8 files, Core + host)

| File | Lines |
|---|---:|
| `Culture/ArchiveChronicleMilestones.cs` | 24 |
| `Culture/CulturalArchiveTomeCatalog.cs` | 102 |
| `Culture/CulturalArchiveVaultSystem.cs` | 675 |
| `Culture/CultureCreationSystem.cs` | 287 |
| `Culture/DocumentationSystem.cs` | 698 |
| `Culture/ShelterFestivalEngine.cs` | 323 |
| `Culture/ShelterMuseumSystem.cs` | 539 |
| `host:Host/CulturalArchiveSaveStore.cs` | 26 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupCultureCAScaffold` / `SaveCultureCAScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Culture/CA169ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class CA169ScaffoldTests
{
    [Fact] public void CAT169A_TODO() { /* deposit model + copy/remove rule. */ }
    [Fact] public void CAT169B_TODO() { /* access tiers from Plan 141 + test per tier. */ }
    [Fact] public void CAT169C_TODO() { /* threat table + one loss fixture per threat (mitigation consumable). */ }
    [Fact] public void CAT169D_TODO() { /* loss marking + restoration path test. */ }
    [Fact] public void CAT169E_TODO() { /* save round-trip; no re-roll on load. */ }
    [Fact] public void CA169_AuthorityConformance_TODO() { /* pattern parity with PLAN-LORE-ARCHIVE-TRUTH-238 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
