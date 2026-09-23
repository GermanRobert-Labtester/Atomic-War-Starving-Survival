# PLAN-MORAL-CHOICE-TRUTH-136 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276`](../EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `MLF-276A` | loader fixture pairs (valid/invalid per loader). |
| `MLF-276B` | id resolution tests. |
| `MLF-276C` | faction reaction mapping tests. |

## 2. Source inventory (20 files, Core + host)

| File | Lines |
|---|---:|
| `MoralChoice/MoralChoiceBranchQuestCatalogLoader.cs` | 72 |
| `MoralChoice/MoralChoiceCatalogLoader.cs` | 125 |
| `MoralChoice/MoralChoiceChainCatalogLoader.cs` | 189 |
| `MoralChoice/MoralChoiceChainData.cs` | 78 |
| `MoralChoice/MoralChoiceExpansionQuestCatalogLoader.cs` | 71 |
| `MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs` | 96 |
| `MoralChoice/MoralChoiceFactionReactionsData.cs` | 34 |
| `MoralChoice/MoralChoiceFlagCatalogLoader.cs` | 60 |
| `MoralChoice/MoralChoiceFlagDefinitions.cs` | 21 |
| `MoralChoice/MoralChoiceGossipCatalogLoader.cs` | 145 |
| `MoralChoice/MoralChoiceGossipData.cs` | 59 |
| `MoralChoice/MoralChoiceGossipRuntime.cs` | 185 |
| `MoralChoice/MoralChoiceIds.cs` | 260 |
| `MoralChoice/MoralChoiceState.cs` | 77 |
| `MoralChoice/MoralChoiceSystem.cs` | 688 |
| `MoralChoice/MoralResolveResult.cs` | 51 |
| `host:Host/HostCli.MoralChoice.cs` | 200 |
| `host:Host/MoralChoiceSaveStore.cs` | 46 |
| `host:Main.MoralChoice.cs` | 288 |
| `host:UI/MoralChoiceModal.cs` | 318 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: `Main.MoralChoice.cs`
- Proposed method names: `SetupMoralChoiceMCScaffold` / `SaveMoralChoiceMCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/MoralChoice/MC136ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class MC136ScaffoldTests
{
    [Fact] public void MCT136A_TODO() { /* resolve-semantics doc + per-field derivation table. */ }
    [Fact] public void MCT136B_TODO() { /* flag precedence/exclusion rules as validator rules + fixtures. */ }
    [Fact] public void MCT136C_TODO() { /* chain stage gating tests (including the documented bypass). */ }
    [Fact] public void MCT136D_TODO() { /* faction reaction wiring tests against Plan 29's owner. */ }
    [Fact] public void MCT136E_TODO() { /* gossip runtime determinism + day-based expiry + save round-trip. */ }
    [Fact] public void MC136_AuthorityConformance_TODO() { /* pattern parity with PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/MoralChoice/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
