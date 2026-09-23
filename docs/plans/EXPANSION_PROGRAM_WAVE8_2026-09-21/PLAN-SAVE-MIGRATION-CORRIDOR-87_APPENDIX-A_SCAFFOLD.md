# PLAN-SAVE-MIGRATION-CORRIDOR-87 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-SAVE-GOVERNANCE-12`](../EXPANSION_PROGRAM_WAVE2_2026-09-21/PLAN-SAVE-GOVERNANCE-12.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (17 files, Core + host)

| File | Lines |
|---|---:|
| `DoseQuestMigration.cs` | 162 |
| `Economy/MigrationConsequenceEngine.cs` | 144 |
| `Economy/SeasonalHumanMigrationEngine.cs` | 162 |
| `Save/CampaignEnvelopeBuilder.cs` | 116 |
| `Save/CampaignSaveEnvelope.cs` | 180 |
| `Save/SaveEnvelopeHelper.cs` | 320 |
| `Save/SaveSectionRegistry.cs` | 600 |
| `Save/SaveSlotService.cs` | 1294 |
| `Save/SaveSlotTypes.cs` | 196 |
| `Save/SaveStore.cs` | 319 |
| `Save/SchemaVersionedEnvelope.cs` | 61 |
| `Save/SessionDurabilityManager.cs` | 365 |
| `Verdict/VerdictQuestMigration.cs` | 137 |
| `WildlifeMigrationSystem.Live.cs` | 313 |
| `WildlifeMigrationSystem.cs` | 127 |
| `host:Host/SaveSlotRoot.cs` | 83 |
| `host:UI/BasalRadonMigrationPanel.cs` | 101 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `mod_manifest_schema.json` | object(9 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupSaveSMScaffold` / `SaveSaveSMScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Save/SM87ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SM87ScaffoldTests
{
    [Fact] public void SMG87A_TODO() { /* ladder census: emit the 204-row table; classify implicit-v1 vs codec-versioned v */ }
    [Fact] public void SMG87B_TODO() { /* declared ladders: set implicit sections to 1 in `SchemaVersions` (or document th */ }
    [Fact] public void SMG87C_TODO() { /* fixtures: per-versioned-section minimal old→new fixture + round-trip test. */ }
    [Fact] public void SMG87D_TODO() { /* matrix: forward/backward scenario tests on the envelope; expected behavior table */ }
    [Fact] public void SMG87E_TODO() { /* ledger doc: `docs/save/SECTION_MIGRATION_LEDGER.md` generated from the registry  */ }
    [Fact] public void SM87_AuthorityConformance_TODO() { /* pattern parity with PLAN-SAVE-GOVERNANCE-12 */ }
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
