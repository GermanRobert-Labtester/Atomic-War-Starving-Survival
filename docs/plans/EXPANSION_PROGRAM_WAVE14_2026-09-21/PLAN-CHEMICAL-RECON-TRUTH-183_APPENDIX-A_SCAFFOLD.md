# PLAN-CHEMICAL-RECON-TRUTH-183 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-SIGNALS-REMOTE-SENSING-49`](../EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-SIGNALS-REMOTE-SENSING-49.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (70 files, Core + host)

| File | Lines |
|---|---:|
| `Combat/BallisticShieldEngine.cs` | 437 |
| `Combat/BallisticsSystem.cs` | 331 |
| `Combat/BallisticsWorkbenchSystem.cs` | 572 |
| `Combat/ChemWarfareSystem.cs` | 336 |
| `Combat/ChemicalPlumeDispersionEngine.cs` | 286 |
| `Combat/CombatAiMove.cs` | 65 |
| `Combat/CombatBreachingEngine.cs` | 650 |
| `Combat/CombatCatalog.cs` | 670 |
| `Combat/CombatDoctrineCapability.cs` | 45 |
| `Combat/CombatFactionStandingBridge.cs` | 256 |
| `Combat/CombatHeadlessDemo.cs` | 289 |
| `Combat/CombatPerks.cs` | 248 |
| `Combat/CombatTypes.cs` | 401 |
| `Combat/EnemyCompositionSelector.cs` | 211 |
| `Combat/SoundRangingCatalog.cs` | 270 |
| `Combat/SoundRangingThreatEngine.cs` | 335 |
| `Combat/StealthSystem.cs` | 340 |
| `Combat/TacticalCombatSystem.Actions.cs` | 563 |
| `Combat/TacticalCombatSystem.Breaching.cs` | 255 |
| `Combat/TacticalCombatSystem.Damage.cs` | 357 |
| `Combat/TacticalCombatSystem.Persistence.cs` | 425 |
| `Combat/TacticalCombatSystem.Targeting.cs` | 131 |
| `Combat/TacticalCombatSystem.cs` | 409 |
| `Combat/WeaponConditionSystem.cs` | 320 |
| `Combat/WeaponEquipmentBridge.cs` | 115 |
| `Expeditions/AerialReconWindowEngine.cs` | 200 |
| `Expeditions/AmphibiousDraisineCatalog.cs` | 237 |
| `Expeditions/AmphibiousDraisineEngine.cs` | 516 |
| `Expeditions/ArmoredCrawlerExpeditionSystem.cs` | 402 |
| `Expeditions/ArmoredCrawlerModuleCatalog.cs` | 133 |
| `Expeditions/AviationSystem.cs` | 462 |
| `Expeditions/ChemicalReconEngine.cs` | 593 |
| `Expeditions/ColonySystem.cs` | 517 |
| `Expeditions/DiscoveryConsequenceSystem.cs` | 390 |
| `Expeditions/DiveInstanceRunner.cs` | 117 |
| `Expeditions/DraisineRerailingSystem.cs` | 255 |
| `Expeditions/EncounterChoiceResolver.cs` | 133 |
| `Expeditions/ExpeditionAggregate.cs` | 116 |
| `Expeditions/ExpeditionCatalogLoader.cs` | 130 |
| `Expeditions/ExpeditionEncounterBridge.cs` | 355 |

… and 30 more.

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupExpeditionsCRScaffold` / `SaveExpeditionsCRScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Expeditions/CR183ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class CR183ScaffoldTests
{
    [Fact] public void CRT183A_TODO() { /* hazard model + dispersion input contract. */ }
    [Fact] public void CRT183B_TODO() { /* sampling quality/band tests. */ }
    [Fact] public void CRT183C_TODO() { /* protection posture table + exposure routing tests. */ }
    [Fact] public void CRT183D_TODO() { /* turnaround/delay path + visible-warning test. */ }
    [Fact] public void CRT183E_TODO() { /* save round-trip; no re-dispatch on load. */ }
    [Fact] public void CR183_AuthorityConformance_TODO() { /* pattern parity with PLAN-SIGNALS-REMOTE-SENSING-49 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Expeditions/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
