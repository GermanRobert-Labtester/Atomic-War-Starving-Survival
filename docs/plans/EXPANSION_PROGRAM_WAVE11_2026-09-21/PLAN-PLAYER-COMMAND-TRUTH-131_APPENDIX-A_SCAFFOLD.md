# PLAN-PLAYER-COMMAND-TRUTH-131 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-SILENT-FAILURE-35`](../EXPANSION_PROGRAM_WAVE4_2026-09-21/PLAN-SILENT-FAILURE-35.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (5 files, Core + host)

| File | Lines |
|---|---:|
| `PlayerCommand/CampaignActionLog.cs` | 81 |
| `PlayerCommand/CommandContext.cs` | 29 |
| `PlayerCommand/CommandPreview.cs` | 101 |
| `PlayerCommand/CommandResult.cs` | 109 |
| `PlayerCommand/PlayerCommandCode.cs` | 57 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupPlayerCommandPMScaffold` / `SavePlayerCommandPMScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/PlayerCommand/PM131ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class PM131ScaffoldTests
{
    [Fact] public void PCT131A_TODO() { /* command code census + coverage table. */ }
    [Fact] public void PCT131B_TODO() { /* envelope enforcement note + scan for direct mutation paths (report, not rewrite) */ }
    [Fact] public void PCT131C_TODO() { /* preview↔result fidelity tests per code. */ }
    [Fact] public void PCT131D_TODO() { /* refusal code table + message wiring into Plan 138's catalog. */ }
    [Fact] public void PCT131E_TODO() { /* action-log independence test (log on/off → same checksum). */ }
    [Fact] public void PM131_AuthorityConformance_TODO() { /* pattern parity with PLAN-SILENT-FAILURE-35 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/PlayerCommand/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
