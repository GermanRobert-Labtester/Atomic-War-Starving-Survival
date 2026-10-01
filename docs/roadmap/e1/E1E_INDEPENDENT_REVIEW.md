# E1E independent review — capability overlap clusters

**Review date:** 2026-10-01
**Review mode:** independent read-only source and corpus audit
**Scope:** five seed clusters and their proposed live-authority mappings.

## Review result

The five reviewed candidates have live owners and concrete Core, host, and test
evidence. Cluster membership describes a search relationship, not an automatic
merge or a status decision. Preserve these distinctions:

- Wasteland rumor propagation is distinct from internal shelter communications.
- Per-NPC episodic memory is distinct from location, phantom, and campaign memory.
- Shelter political blocs are distinct from survivor leadership and repository governance.
- Needs state remains owned by `NeedsSystem`; `NeedsPerformanceBridge` is a read-only projection.
- Food acquisition, cooking, nutrition, and preservation form a connected pipeline with separate owners.

## Evidence corrections and maintained boundaries

The original review overstated three map discrepancies. The NPC-memory UI gap
is current: source, panel registry, and player-route wiring contain no dedicated
NPC-memory panel. Its host/save/test path proves a live domain system, not a
player-facing route, so the map's `None (GAP)` entry is retained. The cooking
UI entry is stale: `KitchenNutritionPanel` binds `CookingHostSession` and is
reachable through `kitchen_nutrition`; the map generator had left its UI and
route arrays empty. `NeedsPerformanceBridge` is a read-only projection over
survivor state, not an independently persisted subsystem; represent it under
the existing `survivors` row with its host/tests and keep save ownership there.
The E1E follow-up records these source-backed corrections without changing
runtime or plan status.

## Additional candidates reviewed

Five candidate-specific receipts now record the searched registries, current
authority, host/save evidence, tests, and recommendation: Plan 203 in
information flow (`RELATED_DISTINCT`), Plan 182 in NPC memory
(`RELATED_DISTINCT`), Plan 43 in shelter governance (`RELATED_DISTINCT`), Plan
24 in needs performance (`EXTENSION_CANDIDATE`), and Plan 215 in the food
pipeline (`RELATED_DISTINCT`). These relations describe semantic overlap only;
the candidates' source plan statuses and bodies are unchanged. Five linked
claims were added for existing rumor, memory persistence, governance, needs
projection, and the kitchen route. No claim asserts an NPC-memory player panel.

## Boundaries

Plan 147 has a numbering collision with an unrelated Plan 147 closeout; use the
stable plan IDs and paths in the register. No plan body, plan status, runtime
source, save schema, or gameplay behavior was changed by this review.
