# PLAN-COMBAT-DOCTRINE-CAPABILITY — Researched Doctrine → Combat Capability Host Binding
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **ARCHIVAL DIRECTIVE (MANDATORY): This plan is FULLY INTEGRATED. Move/keep this file in the integrated plans folder `docs/plans/integrated/<category>/`. It must never remain in `docs/plans/` as open work, and it must not be re-executed or reopened without a new foreman signature.**
> **INTEGRATION STATE: FULLY INTEGRATED — Core authority bound · host session bound · live route/CLI seam bound · focused tests green · no parallel authority created.**

## Integrated evidence

* Researched knowledge now drives the live combat engine: `RecomputeCombatDoctrine()` (called from `SetupExpeditions`) projects `ResearchSystem.HasCapability` through `CombatDoctrineCapability.FromResearch` onto `TacticalCombatSystem.DoctrineCapability`, the property the shot and mobility add-sites already read but which nothing ever assigned. Authored source: `library_manuals.json` (`knowledge_combat_training`, `knowledge_fortified_chokepoints`). Probe: `--combat-doctrine-selftest` 10/10. Focused tests: `PlanCombatDoctrineCapabilityTests` 5/5 (projection values remain pinned by the pre-existing `TacticalCombatDeterminismTests.B3_009`; no duplicate suite created). Zero new save sections: ResearchSystem stays the only knowledge authority.

## Original plan body (preserved for the record)

> **Package:** `COMBAT-DOCTRINE-CAPABILITY`
> **Category:** combat / research
> **Plan type:** bind a designed-but-unassigned Core seam to the live research owner.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Assign `CombatDoctrineCapability` to `TacticalCombatSystem.DoctrineCapability` from
the LIVE `ResearchSystem`, so researching a doctrine actually changes combat.

**Bounded outcome:** the capability is projected from the campaign's own knowledge
truth via the factory the Core author already provided
(`CombatDoctrineCapability.FromResearch(Func<string,bool>)`) and recomputed when
knowledge changes, so accuracy and tactical-mobility bonuses stop being dead code.

**Non-goals:** no new research authority, no new combat stat, no invented bonuses
(all four numbers are authored in the Core factory), no UI panel.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| **Designed seam, never assigned** | `TacticalCombatSystem.cs:52` `public CombatDoctrineCapability DoctrineCapability { get; set; } = CombatDoctrineCapability.None;` — mechanical sweep: `grep -rq DoctrineCapability src/` → **no assignment anywhere** |
| **The bonuses are already consumed** | `TacticalCombatSystem.Actions.cs:148` `WeaponAccuracy = def.accuracy + (DoctrineCapability?.AccuracyBonus ?? 0f)`; `:508` `float success = mods.Mobility + (DoctrineCapability?.TacticalMobilityBonus ?? 0f)` |
| Factory exists, uncalled | `CombatDoctrineCapability.FromResearch(hasKnowledge)` → combat training `Accuracy +0.05`, `Recoil 0.10`, `Mobility +0.05`; fortified chokepoints `BarrierIntegrity +0.20` |
| **Content is authored** | `library_manuals.json:393-394` unlocks `knowledge_combat_training`; `:431-432` unlocks `knowledge_fortified_chokepoints` |
| **Other systems already honour it** | `perimeter_defenses.json:146,170` `"required_knowledge": "knowledge_fortified_chokepoints"`; `autopsy_procedures.json:257`, `prewar_archives.json:90` reference the same id |
| Live knowledge query | `ResearchSystem.HasCapability(knowledgeId)` (`Research/ResearchSystem.cs:82`) whose own doc states consumers "ask this single question instead of … caching unlock truth locally" |

**Player-visible defect being closed:** a survivor can research *Combat Training*
and *Fortified Chokepoints* and receive **no** combat benefit at all, while the
perimeter-defense build menu already gates on the same knowledge.

## 3. Files

### New — Host
- `src/Host/CombatDoctrineCapabilityHostSession.cs`
- `src/Host/HostCli.CombatDoctrine.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--combat-doctrine-selftest`

### Modified — Host
- `src/Host/CombatHostSession.cs` — bind + recompute from the live research owner
- `src/Host/HostCli.cs`, `src/Main.Application.cs`, `src/Main.Combat.cs`
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/Combat/PlanCombatDoctrineCapabilityTests.cs` (new)

## 4. Acceptance

1. `--combat-doctrine-selftest` ≥ 10/10: no-knowledge ⇒ all bonuses 0; combat
   training ⇒ the three authored bonuses; fortified chokepoints ⇒ the barrier
   bonus; both together ⇒ both sets; capability recomputes after an unlock; the
   live `TacticalCombatSystem` instance carries the assigned capability; no second
   knowledge cache.
2. Focused Core-contract suite green, asserting the exact authored numbers.
3. `ResearchSystem` stays the only knowledge authority; combat only projects it.
4. Adjacent gates green (no new save section — this is a derived capability).

## 5. Deferred with named reasons

- No doctrine UI panel: the combat surfaces are shared.
- `RecoilMitigation` / `BarrierIntegrityBonus` consumers: `Actions.cs` consumes
  accuracy and mobility today; extending recoil/barrier reads is a separate
  combat-authority decision, not a missing binding.
