# PLAN-ENDGAME-EVALUATION-TRUTH-137 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-CAMPAIGN-EPILOGUE-TRUTH-259`](../EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-CAMPAIGN-EPILOGUE-TRUTH-259.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `CET-259A` | input contract + owner table. |
| `CET-259B` | deterministic assembly test (paired builds). |
| `CET-259C` | missing-input gap-line fixture. |

## 2. Source inventory (17 files, Core + host)

| File | Lines |
|---|---:|
| `Endgame/CampaignCompletionHistory.cs` | 511 |
| `Endgame/CampaignOutcomeEvaluator.cs` | 323 |
| `Endgame/CampaignOutcomeSnapshot.cs` | 81 |
| `Endgame/CrossRunProfileStore.cs` | 134 |
| `Endgame/EndgameHeadlessDemo.cs` | 102 |
| `Endgame/EndgameSystem.cs` | 323 |
| `Endgame/EpilogueChronicleBuilder.cs` | 126 |
| `Endgame/EpilogueChronicleCatalog.cs` | 90 |
| `Endgame/EpilogueContextFactory.cs` | 61 |
| `Endgame/EpilogueMatrixRuntime.cs` | 151 |
| `Endgame/UnifiedEndingResolver.cs` | 596 |
| `EndingsHeadlessDemo.cs` | 105 |
| `HoldfastEndings.cs` | 156 |
| `Verdict/VerdictEndingEvaluator.cs` | 72 |
| `host:Host/EndgameHostSession.cs` | 82 |
| `host:Host/EndgameSaveStore.cs` | 38 |
| `host:Main.Endgame.cs` | 235 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `endings.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: `Main.Endgame.cs`
- Proposed method names: `SetupEndgameEEScaffold` / `SaveEndgameEEScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Endgame/EE137ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class EE137ScaffoldTests
{
    [Fact] public void EET137A_TODO() { /* resolver authority decision + projection/retirement note for the loser path. */ }
    [Fact] public void EET137B_TODO() { /* input table (field → owner → fallback) + missing-input tests. */ }
    [Fact] public void EET137C_TODO() { /* completion history once-only tests (save/reload, replay). */ }
    [Fact] public void EET137D_TODO() { /* cross-run profile scope + local-only privacy check. */ }
    [Fact] public void EET137E_TODO() { /* chronicle determinism test (paired same-save builds). */ }
    [Fact] public void EE137_AuthorityConformance_TODO() { /* pattern parity with PLAN-CAMPAIGN-EPILOGUE-TRUTH-259 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
