# PLAN-COMBAT-DEPTH-62 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (27 files)

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
| `Expeditions/TravelEncounterCombatBinder.cs` | 57 |
| `Survivors/CombatTraumaSystem.cs` | 255 |

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `chemical_weapons.json` | object(2 keys) |
| `ballistic_shield_catalog.json` | object(2 keys) |
| `ballistics_workbench_catalog.json` | object(2 keys) |
| `breaching_equipment_catalog.json` | object(4 keys) |
| `combat_catalog.json` | object(6 keys) |
| `faction_combat_thresholds.json` | object(3 keys) |

## 3. Host attachment

- Candidate host partials: none — new attachment or headless-only

- Proposed setup method: `SetupCombatScaffold` · proposed save method: `SaveCombatScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Combat/CD62ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class CD62ScaffoldTests
{
    [Fact] public void CD62A_TODO() { /* loadout surface: equip gating, weight, condition; no bypass of inventory authori */ }
    [Fact] public void CD62B_TODO() { /* ballistics consumer: range/cover modifiers visible; ammo and condition consumed. */ }
    [Fact] public void CD62C_TODO() { /* breaching tools: entry routes with noise/structural consequences routed to shelt */ }
    [Fact] public void CD62D_TODO() { /* chemical warfare: plume, masks, decontamination; contamination routed to radiati */ }
    [Fact] public void CD62E_TODO() { /* injury pipeline: wound → treatment → recovery/rehab → chronic/prosthetic; uses t */ }
    [Fact] public void CD62F_TODO() { /* doctrine/perks: training consumes time; options unlock (suppress, flank, cover)  */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
