# Plan 182 — Relationship drift authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_182_Relationship_Decay_Drift_System.md`. Time-only decay would invent social events.

---

## 1. Premise

`SurvivorRelationsSystem` already persists pair affinity/trust/resentment/grief (`survivor_relations`). `TickDay` only rolls conflict; **no neglect decay**. `RelationshipEntry` has **no `lastInteractionDay`**.

**Live producers that already call `ModifyAffinity`:** ideological friction (same duty-role pairs), trauma-bond bonuses, apprenticeship complete (+10), shelter social events, confession, psychology-arc −5, mediation/grief.

**Not producers today:** duty co-assignment as friendship, `ShelterAssignment` roommates, combat cooperation, gift/dialogue loop, time passing.

---

## 2. Ownership (proposed)

Sole affinity ledger: **`SurvivorRelationsSystem`**. Other systems emit deltas; they do not store a second pair graph. Trauma-bond strength decay stays on `TraumaBondSystem` and must not be mixed into pair affinity.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| `RelationshipDecaySystem` / time-only −affinity | **OUT** |
| New decay save section | **OUT** |
| Roommate decay that bypasses `ShelterAssignment` | **OUT** |
| Stamp `lastInteractionDay` on existing `ModifyAffinity` call sites | **IN** (implement later) |
| Neglect only if stamp is old **and** no other owner applied a daily delta | **IN** |

**Next implement:** `DEBT-182-LAST-INTERACTION-STAMP` — field + stamp on existing producers. No decay formula until that stamp exists.

---

## 4. Evidence paths

`SurvivorRelationsSystem.cs`, `SurvivorRelationsSaveStore.cs`, `SurvivorSocialCoordinator.cs`, `IdeologicalFrictionSystem.cs`, `TraumaBondSystem.cs`, `ShelterSocialDynamicsSystem.cs`, `ShelterAssignmentSystem.cs`, `src/UI/SurvivorRelationsPanel.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
