# PLAN-UTILITY-AI-TRUTH-133 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-DETERMINISM-CROSS-HOST-89`](../EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-DETERMINISM-CROSS-HOST-89.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `DXH-89A` | headless replay dump: one verb, one seed, N days, deterministic JSON (day, checksum, stream counts). |
| `DXH-89B` | in-game capture: same tuple through the live day loop; saved beside the headless dump. |
| `DXH-89C` | comparator + bisect: first divergent day → first divergent section → named owner file. |

## 2. Source inventory (7 files, Core + host)

| File | Lines |
|---|---:|
| `UtilityAI/UtilityAction.cs` | 161 |
| `UtilityAI/UtilityActionScorer.cs` | 93 |
| `UtilityAI/UtilityAiHeadlessDemo.cs` | 96 |
| `UtilityAI/UtilityAiSystem.cs` | 124 |
| `host:Host/UtilityAiHostSession.cs` | 74 |
| `host:Main.UiTests.UtilityAi.cs` | 73 |
| `host:UtilityAI/UtilityAiPanel.cs` | 105 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `utility_actions.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: `Main.UiTests.UtilityAi.cs`
- Proposed method names: `SetupUtilityAIUAScaffold` / `SaveUtilityAIUAScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/UtilityAI/UA133ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class UA133ScaffoldTests
{
    [Fact] public void UAT133A_TODO() { /* score contract doc + range table. */ }
    [Fact] public void UAT133B_TODO() { /* tie-break rule + equal-score fixture test. */ }
    [Fact] public void UAT133C_TODO() { /* selection trace buffer + dev-tool read (no runtime authority). */ }
    [Fact] public void UAT133D_TODO() { /* headless↔host parity test on a scripted state. */ }
    [Fact] public void UAT133E_TODO() { /* forbidden-input scan over `UtilityAI/`. */ }
    [Fact] public void UA133_AuthorityConformance_TODO() { /* pattern parity with PLAN-DETERMINISM-CROSS-HOST-89 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAI/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
