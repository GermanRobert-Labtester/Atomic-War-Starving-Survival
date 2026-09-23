# PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-SAVE-MIGRATION-CORRIDOR-87`](PLAN-SAVE-MIGRATION-CORRIDOR-87.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `SMG-87A` | ladder census: emit the 204-row table; classify implicit-v1 vs codec-versioned vs explicit. |
| `SMG-87B` | declared ladders: set implicit sections to 1 in `SchemaVersions` (or document the codec-owned exception per ke |
| `SMG-87C` | fixtures: per-versioned-section minimal old→new fixture + round-trip test. |

## 2. Source inventory (18 files, Core + host)

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
| `SaveChecksum.cs` | 198 |
| `Verdict/VerdictQuestMigration.cs` | 137 |
| `WildlifeMigrationSystem.Live.cs` | 313 |
| `WildlifeMigrationSystem.cs` | 127 |
| `host:Host/SaveStoreChecksumSelfTest.cs` | 345 |
| `host:UI/BasalRadonMigrationPanel.cs` | 101 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupSaveSFScaffold` / `SaveSaveSFScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Save/SF98ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SF98ScaffoldTests
{
    [Fact] public void SIF98A_TODO() { /* coverage matrix generator over registry + codecs; `--check` mode. */ }
    [Fact] public void SIF98B_TODO() { /* corpus + recipes: bounded mutations for the highest-risk codecs (envelope, dose, */ }
    [Fact] public void SIF98C_TODO() { /* recovery behavior per store + focused tests; a failed load never yields partial  */ }
    [Fact] public void SIF98D_TODO() { /* rotation integration note (slice size, time budget) for Plan 74. */ }
    [Fact] public void SIF98E_TODO() { /* triage dump format + one worked example. */ }
    [Fact] public void SF98_AuthorityConformance_TODO() { /* pattern parity with PLAN-SAVE-MIGRATION-CORRIDOR-87 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
