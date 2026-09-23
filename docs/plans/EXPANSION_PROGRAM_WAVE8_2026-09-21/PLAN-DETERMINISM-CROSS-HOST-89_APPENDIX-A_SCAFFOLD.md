# PLAN-DETERMINISM-CROSS-HOST-89 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-DETERMINISM-REPLAY-13`](../EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-DETERMINISM-REPLAY-13.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (13 files, Core + host)

| File | Lines |
|---|---:|
| `Random/CampaignRngStream.cs` | 259 |
| `Save/CampaignEnvelopeBuilder.cs` | 116 |
| `Save/CampaignSaveEnvelope.cs` | 180 |
| `Save/SaveEnvelopeHelper.cs` | 320 |
| `Save/SaveSectionRegistry.cs` | 600 |
| `Save/SaveSlotService.cs` | 1294 |
| `Save/SaveSlotTypes.cs` | 196 |
| `Save/SaveStore.cs` | 319 |
| `Save/SchemaVersionedEnvelope.cs` | 61 |
| `Save/SessionDurabilityManager.cs` | 365 |
| `SaveChecksum.cs` | 198 |
| `host:Host/SaveStoreChecksumSelfTest.cs` | 345 |
| `host:Host/SevenDayDeterministicSmokeTest.cs` | 623 |

## 3. Data bindings

No catalog name-matched; verify loader paths before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupSaveDHScaffold` / `SaveSaveDHScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Save/DH89ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class DH89ScaffoldTests
{
    [Fact] public void DXH89A_TODO() { /* headless replay dump: one verb, one seed, N days, deterministic JSON (day, check */ }
    [Fact] public void DXH89B_TODO() { /* in-game capture: same tuple through the live day loop; saved beside the headless */ }
    [Fact] public void DXH89C_TODO() { /* comparator + bisect: first divergent day → first divergent section → named owner */ }
    [Fact] public void DXH89D_TODO() { /* fork-order test: reorder two setups in a test composition; expect failure with t */ }
    [Fact] public void DXH89E_TODO() { /* bounded recipe doc: exact commands, runtime budget, expected output; wired to Pl */ }
    [Fact] public void DH89_AuthorityConformance_TODO() { /* pattern parity with PLAN-DETERMINISM-REPLAY-13 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
