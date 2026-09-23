# PLAN-LEADERSHIP-TRUTH-173 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-INSTITUTIONS-TRUTH-141`](../EXPANSION_PROGRAM_WAVE11_2026-09-21/PLAN-INSTITUTIONS-TRUTH-141.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `INT-141A` | availability predicate table per institution. |
| `INT-141B` | lifecycle + one-holder-per-berth enforcement tests. |
| `INT-141C` | vacating path tests (death/injury/removal/resign/reassign). |

## 2. Source inventory (72 files, Core + host)

| File | Lines |
|---|---:|
| `Survivors/AgingSystem.cs` | 304 |
| `Survivors/AntenatalMaternalHealthEngine.cs` | 321 |
| `Survivors/BackstorySystem.cs` | 364 |
| `Survivors/CaregivingSystem.cs` | 393 |
| `Survivors/ChildDevelopmentSystem.cs` | 354 |
| `Survivors/CombatTraumaSystem.cs` | 255 |
| `Survivors/CrewConsentVerdict.cs` | 28 |
| `Survivors/DesperationSystem.cs` | 289 |
| `Survivors/DreamSystem.cs` | 300 |
| `Survivors/ExerciseSystem.cs` | 416 |
| `Survivors/FinalWishCatalog.cs` | 115 |
| `Survivors/FinalWishCatalogLoader.cs` | 67 |
| `Survivors/FinalWishSystem.cs` | 399 |
| `Survivors/FitnessForDutyModel.cs` | 910 |
| `Survivors/GenealogyBridge.cs` | 183 |
| `Survivors/GenerationalSystem.cs` | 456 |
| `Survivors/GuiltInsomniaSystem.cs` | 225 |
| `Survivors/GuiltSourceCatalog.cs` | 107 |
| `Survivors/HiddenAgendaSystem.cs` | 441 |
| `Survivors/HobbySystem.cs` | 344 |
| `Survivors/ISurvivorComponentStore.cs` | 93 |
| `Survivors/IdeologicalFrictionEvents.cs` | 653 |
| `Survivors/IdeologicalFrictionSystem.cs` | 164 |
| `Survivors/InterpersonalConflictSystem.cs` | 657 |
| `Survivors/LaborProductivity.cs` | 206 |
| `Survivors/LatentExpertAwakeningSystem.cs` | 241 |
| `Survivors/LeadershipSystem.cs` | 663 |
| `Survivors/MemorialComponentAdapter.cs` | 139 |
| `Survivors/MemorialComponentParity.cs` | 388 |
| `Survivors/MemorialComponentStore.cs` | 301 |
| `Survivors/MoralBranchingSystem.cs` | 295 |
| `Survivors/MoraleContagionCatalog.cs` | 65 |
| `Survivors/MoraleContagionSave.cs` | 199 |
| `Survivors/MoraleContagionSystem.cs` | 770 |
| `Survivors/NeedsComponentParity.cs` | 340 |
| `Survivors/NeedsComponentStore.cs` | 322 |
| `Survivors/NeedsModifierStack.cs` | 239 |
| `Survivors/NeedsPerformanceBridge.cs` | 476 |
| `Survivors/NeedsSystem.cs` | 441 |
| `Survivors/PersonalBelongingsSystem.cs` | 584 |

… and 32 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `leadership_policies.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupSurvivorsLDScaffold` / `SaveSurvivorsLDScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Survivors/LD173ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class LD173ScaffoldTests
{
    [Fact] public void LST173A_TODO() { /* authority model + legitimacy input table. */ }
    [Fact] public void LST173B_TODO() { /* order-compliance rules + low-legitimacy fixture per effect. */ }
    [Fact] public void LST173C_TODO() { /* succession paths + ledger write tests (Plan 141). */ }
    [Fact] public void LST173D_TODO() { /* legitimacy source audit (no parallel score proof). */ }
    [Fact] public void LST173E_TODO() { /* save round-trip; no re-run on load. */ }
    [Fact] public void LD173_AuthorityConformance_TODO() { /* pattern parity with PLAN-INSTITUTIONS-TRUTH-141 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
