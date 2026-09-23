# PLAN-JOURNEY-CONTEXT-TRUTH-156 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-TRAVEL-ENCOUNTER-TRUTH-177`](../EXPANSION_PROGRAM_WAVE14_2026-09-21/PLAN-TRAVEL-ENCOUNTER-TRUTH-177.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `TET-177A` | encounter table + data validation. |
| `TET-177B` | seeded selection determinism test (paired legs). |
| `TET-177C` | avoidance input table + skip/pre-empt tests. |

## 2. Source inventory (3 files, Core + host)

| File | Lines |
|---|---:|
| `Journeys/JourneyExecutionContext.cs` | 76 |
| `Onboarding/OnboardingJourney.cs` | 630 |
| `host:Main.UiTests.RealCampaignJourney.cs` | 530 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: `Main.UiTests.RealCampaignJourney.cs`
- Proposed method names: `SetupJourneysJCScaffold` / `SaveJourneysJCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Journeys/JC156ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class JC156ScaffoldTests
{
    [Fact] public void JCT156A_TODO() { /* field table with owners. */ }
    [Fact] public void JCT156B_TODO() { /* consumer audit (documented reads only). */ }
    [Fact] public void JCT156C_TODO() { /* snapshot equality test (headless vs host, same seed/day). */ }
    [Fact] public void JCT156D_TODO() { /* read-only guard: a write attempt from a consumer fails a scan/test. */ }
    [Fact] public void JCT156E_TODO() { /* mid-leg save/restore equivalence test. */ }
    [Fact] public void JC156_AuthorityConformance_TODO() { /* pattern parity with PLAN-TRAVEL-ENCOUNTER-TRUTH-177 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Journeys/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
