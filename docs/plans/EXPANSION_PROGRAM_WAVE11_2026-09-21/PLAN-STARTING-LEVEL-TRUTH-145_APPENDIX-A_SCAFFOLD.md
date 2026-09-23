# PLAN-STARTING-LEVEL-TRUTH-145 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-BALANCE-DIFFICULTY-INTEGRATION-73`](../EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-BALANCE-DIFFICULTY-INTEGRATION-73.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `BD-73A` | preset selection + persistence: creation UI/CLI; save field; legacy default. |
| `BD-73B` | scalar census: author → consumer table; hardcoded values replaced by scalars where authored. |
| `BD-73C` | baseline registry: each sweep documented with seed, commit, config, result bands. |

## 2. Source inventory (9 files, Core + host)

| File | Lines |
|---|---:|
| `Inventory/StartingSuppliesCatalog.cs` | 135 |
| `StartingLevel/StartingLevelState.cs` | 79 |
| `StartingLevel/StartingLevelSystem.cs` | 478 |
| `Survivors/StartingCohortCatalog.cs` | 366 |
| `Survivors/SurvivorStartingStateLoader.cs` | 161 |
| `host:Host/HostCli.StartingSupplies.cs` | 155 |
| `host:Host/StartingLevelHostSession.cs` | 194 |
| `host:Main.UiTests.StartingCohortLifecycle.cs` | 267 |
| `host:UI/StartingCohortSetupPanel.cs` | 325 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `starting_survivors.json` | object(2 keys) |
| `starting_supplies.json` | object(3 keys) |
| `starting_survivor_cohorts.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: `Main.UiTests.StartingCohortLifecycle.cs`
- Proposed method names: `SetupStartingLevelSLScaffold` / `SaveStartingLevelSLScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/StartingLevel/SL145ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SL145ScaffoldTests
{
    [Fact] public void SLT145A_TODO() { /* precedence rule doc + combination tests (preset×scenario matrix sample). */ }
    [Fact] public void SLT145B_TODO() { /* grant path through inventory + no-double-grant test. */ }
    [Fact] public void SLT145C_TODO() { /* scenario override field list + schema check. */ }
    [Fact] public void SLT145D_TODO() { /* first-day invariant validator + fixtures (one per preset). */ }
    [Fact] public void SLT145E_TODO() { /* day-one save/reload equivalence test. */ }
    [Fact] public void SL145_AuthorityConformance_TODO() { /* pattern parity with PLAN-BALANCE-DIFFICULTY-INTEGRATION-73 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/StartingLevel/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
