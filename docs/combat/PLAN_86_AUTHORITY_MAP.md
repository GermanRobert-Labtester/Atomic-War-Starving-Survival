# Plan B86 — Combat Breaching Authority Map

**Parent:** [`docs/PLANS_86_89_AUTHORITY_MAP.md`](../PLANS_86_89_AUTHORITY_MAP.md) · [`docs/plans/PLANS_86_89_INTEGRATION_PLAN.md`](../plans/PLANS_86_89_INTEGRATION_PLAN.md)
**Date:** 2026-09-06
**Status:** Recon complete

---

## 1. What exists

| Layer | Owner | Role |
|---|---|---|
| Unused tactical barriers | `BarrierState` in `CombatTypes.cs` / `CombatState.Barriers` | Integrity + material id per lane; saved; **not mutated by actions** |
| Barrier lookup | `TacticalCombatSystem.FindPlayerLaneBarrier` | Exists; fire path nulls material afterward |
| Ballistics | `BallisticsSystem` | `BarrierBlocked` / `BarrierPenetrated` when context supplied |
| Cover | Per-combatant `CoverRating` | Static float; no recalc seam |
| Route mines | `RouteInfrastructureSystem` + `MineClearingFlailEngine` | Persistent corridor clearance |
| Rail obstacles | `RailwaySystem.ClearTrackObstacle` | Scrap-cost track clear |
| Expedition loadout | `ExpeditionSystem` + garage | Bike / flashlight / vehicle profile only |
| Winch tags | `winch_kit`, `vmod_heavy_winch` | No tow behavior |
| Noise | `StealthSystem` + unused `StanceMods.Noise` | Prefer extend these |
| UI stub | `VaultDoorBreachingPanel` | Prototype — **not** B86 surface |

**Missing:** `TacticalCombatSystem.Obstacles.cs` (brief assumption false).

---

## 2. Extension seams (ordered)

1. Populate `CombatState.Barriers` at encounter begin; pass real `BarrierMaterial` + integrity into fire path.
2. Add breach/cut player actions with Evaluate* preflight beside existing combat actions.
3. `CombatBreachingEngine` owns clearance progress/method validation; mutates barriers via combat state.
4. Noise → stance / `StealthSystem.accumulatedNoise`.
5. Tool wear → `EquipmentConditionSystem.ApplyWear`.
6. Expedition prep gates for breach tools (extend `ExpeditionState` / inventory check).
7. Optional: winch assist on existing vehicle tags for debris class.
8. World persistence only if clearance must survive the encounter (`LocationEvolution` / route flag); else keep encounter-local.

---

## 3. Do not duplicate

- Mine flail / route infrastructure for corridor mines
- Railway track obstacle clearance
- Perimeter defense razorwire as expedition tactical props
- A second combat save section for barriers (already in `combat`)

---

## 4. Catalog plan

`breaching_equipment_catalog.json`:

- Obstacle profiles with `clearance_tags`, structural rating, cover rating, path-blocking, hazard tags
- Tool profiles with gameplay-only fields (no real explosive engineering)
- Dedupe against `combat_catalog.json` materials, perimeter defenses, and route mine types before authoring the “exact 12”
