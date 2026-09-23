# PLAN-GENERATIONAL-MILESTONE-TRUTH-160 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-SUCCESSION-LEGACY-TRUTH-252`](../EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-SUCCESSION-LEGACY-TRUTH-252.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `SLT-252A` | manifest + owner table. |
| `SLT-252B` | ordering/race fixture. |
| `SLT-252C` | default-pool tests (nothing vanishes). |

## 2. Source inventory (8 files, Core + host)

| File | Lines |
|---|---:|
| `Culture/ArchiveChronicleMilestones.cs` | 24 |
| `GenerationalLineageExtension.cs` | 434 |
| `Generations/SecondGenerationMilestoneEngine.cs` | 229 |
| `Legacy/CampaignLegacySystem.cs` | 305 |
| `Legacy/GenerationalSuccessionEngine.cs` | 166 |
| `Medical/RespiratoryDegenerationSystem.cs` | 287 |
| `Survivors/GenerationalSystem.cs` | 456 |
| `host:Host/GenerationalSaveStore.cs` | 32 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupGenerationsGMScaffold` / `SaveGenerationsGMScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Generations/GM160ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class GM160ScaffoldTests
{
    [Fact] public void GMT160A_TODO() { /* milestone table with triggering facts. */ }
    [Fact] public void GMT160B_TODO() { /* read-only owner discipline + no-compute test. */ }
    [Fact] public void GMT160C_TODO() { /* once-only firing tests (reload, replay). */ }
    [Fact] public void GMT160D_TODO() { /* handover via Plan 141 ledger + no-rewrite test. */ }
    [Fact] public void GMT160E_TODO() { /* ending-input exposure check with a Plan 137 fixture. */ }
    [Fact] public void GM160_AuthorityConformance_TODO() { /* pattern parity with PLAN-SUCCESSION-LEGACY-TRUTH-252 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Generations/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
