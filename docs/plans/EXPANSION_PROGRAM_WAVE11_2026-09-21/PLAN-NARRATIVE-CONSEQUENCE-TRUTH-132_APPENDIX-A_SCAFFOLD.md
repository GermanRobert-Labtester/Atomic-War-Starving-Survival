# PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-NARRATIVE-FAMILY-TRUTH-261`](../EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-NARRATIVE-FAMILY-TRUTH-261.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `NFT-261A` | census table (file → class → loader → consumer → test). |
| `NFT-261B` | unreferenced report (content-owner routed). |
| `NFT-261C` | fixture pass for catalog files lacking any test. |

## 2. Source inventory (14 files, Core + host)

| File | Lines |
|---|---:|
| `DebtConsequenceDispatcher.cs` | 317 |
| `DebtConsequenceHostBridge.cs` | 328 |
| `Difficulty/DifficultyConsequenceWeave.cs` | 81 |
| `Economy/MigrationConsequenceEngine.cs` | 144 |
| `Expeditions/DiscoveryConsequenceSystem.cs` | 390 |
| `Factions/EspionageConsequenceRouter.cs` | 120 |
| `Flags/CampaignConsequenceLedger.cs` | 301 |
| `Foundry/SilentFoundryConsequencePolicy.cs` | 227 |
| `NarrativeConsequence/NarrativeConsequenceGraph.cs` | 139 |
| `NarrativeConsequence/NarrativeSimulator.cs` | 350 |
| `NarrativeConsequence/NarrativeValidator.cs` | 280 |
| `Shelter/SanitationConsequenceRules.cs` | 51 |
| `Treaties/TreatyConsequences.cs` | 191 |
| `host:Host/NarrativeArcConsequenceAdapter.cs` | 68 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `foundry_treaty_consequences.json` | object(3 keys) |
| `discovery_consequences.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupNarrativeConsequenceNC2Scaffold` / `SaveNarrativeConsequenceNC2Scaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/NarrativeConsequence/NC2132ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class NC2132ScaffoldTests
{
    [Fact] public void NCT132A_TODO() { /* graph rule document + validator mapping. */ }
    [Fact] public void NCT132B_TODO() { /* reachability pass over authored data with a report. */ }
    [Fact] public void NCT132C_TODO() { /* dead-end policy tests (intended ending passes; accidental sink fails). */ }
    [Fact] public void NCT132D_TODO() { /* simulator determinism test (paired same-seed trace). */ }
    [Fact] public void NCT132E_TODO() { /* per-rule failure message check (one bad fixture per rule). */ }
    [Fact] public void NC2132_AuthorityConformance_TODO() { /* pattern parity with PLAN-NARRATIVE-FAMILY-TRUTH-261 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeConsequence/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
