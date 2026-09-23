# PLAN-TEXT-PACK-LOCALIZATION-88 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-LOCALIZATION-READINESS-52`](../EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-LOCALIZATION-READINESS-52.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `LZ-52A` | inventory completion: extraction covers narrative catalogs, UI constants, CLI help, and item/medical text; rep |
| `LZ-52B` | freeze gate: `StringFreezePolicy` reads the freeze date; new hardcoded strings outside catalogs fail; exceptio |
| `LZ-52C` | pseudo-locale: generator + snapshot run; every overflow/truncation is a finding with panel + key. |

## 2. Source inventory (4 files, Core + host)

| File | Lines |
|---|---:|
| `Localization/LocalizationService.cs` | 595 |
| `Localization/StringFreezePolicy.cs` | 171 |
| `Localization/WildlifeTrappingLocalization.cs` | 34 |
| `host:Localization/AshfallLocalization.cs` | 88 |

## 3b. L10n inventory artifact

Records 535 · hardcoded 472 · lookups 63 · UI files 214 · pilot ResearchPanel, OnboardingHintPanel.

Top panels by literals: `ElectrostaticScrubberPanel` 18, `RailwayTerminalPanel` 15, `SlurryDewateringSumpPanel` 11, `GeigerCalibrationPanel` 10, `AquiferTreatyConcessionPanel` 9, `BasalRadonMigrationPanel` 9.

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupLocalizationTPScaffold` / `SaveLocalizationTPScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Localization/TP88ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class TP88ScaffoldTests
{
    [Fact] public void LTC88A_TODO() { /* content key census: emit per-catalog key tables from data + consumer scan. */ }
    [Fact] public void LTC88B_TODO() { /* stability rule + frozen-id list checked by a gate (id change without alias fails */ }
    [Fact] public void LTC88C_TODO() { /* pseudo-locale generator (script, not runtime) + one full panel pass under it. */ }
    [Fact] public void LTC88D_TODO() { /* orphan/missing key gate: consumer↔pack comparison with per-row failure output. */ }
    [Fact] public void LTC88E_TODO() { /* authoring guide `docs/l10n/CONTENT_KEYS.md` + one worked example per content typ */ }
    [Fact] public void TP88_AuthorityConformance_TODO() { /* pattern parity with PLAN-LOCALIZATION-READINESS-52 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Localization/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
