# PLAN-FEEDBACK-SURFACE-TRUTH-138 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-HOST-EVENT-ARCHIVE-91`](../EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-HOST-EVENT-ARCHIVE-91.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `HEA-91A` | taxonomy: enumerated fact ids with day/hour/subject fields; unknown facts are dropped and counted, not guessed |
| `HEA-91B` | writer: ring file per slot, rotation, deterministic ordering, flush on save. |
| `HEA-91C` | independence guard: focused test toggling archive on/off and comparing checksums. |

## 2. Source inventory (10 files, Core + host)

| File | Lines |
|---|---:|
| `Feedback/FeedbackDeduplicator.cs` | 102 |
| `Feedback/FeedbackEvent.cs` | 42 |
| `Feedback/FeedbackMessageCatalog.cs` | 182 |
| `Feedback/FeedbackMessageCatalogLoader.cs` | 343 |
| `Feedback/FeedbackMessageTypes.cs` | 41 |
| `Feedback/FeedbackService.cs` | 129 |
| `Feedback/IFeedbackService.cs` | 22 |
| `Feedback/ResolvedFeedbackMessage.cs` | 24 |
| `host:UI/FeedbackMessages.cs` | 560 |
| `host:UI/FeedbackPanel.cs` | 235 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `feedback_messages.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupFeedbackFBScaffold` / `SaveFeedbackFBScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Feedback/FB138ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class FB138ScaffoldTests
{
    [Fact] public void FST138A_TODO() { /* coverage gate: code set ↔ message set; fail with the missing code named. */ }
    [Fact] public void FST138B_TODO() { /* parameter substitution tests (missing parameter → typed failure, not "null"). */ }
    [Fact] public void FST138C_TODO() { /* dedup window tests in game time (OS clock change has no effect). */ }
    [Fact] public void FST138D_TODO() { /* interface scan: panels touching the catalog directly are reported. */ }
    [Fact] public void FST138E_TODO() { /* localization key check against Plan 52's inventory. */ }
    [Fact] public void FB138_AuthorityConformance_TODO() { /* pattern parity with PLAN-HOST-EVENT-ARCHIVE-91 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Feedback/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
