# Duplicate-search receipt — Plan 182

- **Plan ID / proposed capability:** `LEGACY-2D0AD23A05FF` — Relationship Decay & Drift System.
- **Queries used:** `relationship decay drift`; `survivor pair relationship decay`; `per-NPC memory intensity`; `trust grudge daily tick`.
- **Files and registries inspected:** `docs/roadmap/PLAN_REGISTER.json`; `docs/roadmap/e1/E1E_CAPABILITY_CLUSTERS.json`; `docs/architecture/CLAIMS.json`; `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`; `Next-steps-plans/Plan_182_Relationship_Decay_Drift_System.md`; `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs`; `Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs`; `src/Host/NpcMemoryHostSession.cs`; `Ashfall.Core.Tests/Narrative/Plan147NpcMemoryHostIntegrationTests.cs`.
- **Live Core authority found:** `SurvivorRelationsSystem` models pairwise survivor relationships. `NpcMemorySystem` separately owns per-NPC memories and their daily decay.
- **Host route and save owner found:** NPC memories persist under registered `npc_memory` / `npc_memory_save.json`; the relationship-decay candidate must be checked against the survivor relationship owner and its save seam before implementation. No dedicated player UI route is present for NPC memory.
- **Tests or runtime evidence inspected:** `Plan147NpcMemoryHostIntegrationTests` verifies memory decay and save registration; current source/test inventory for pairwise relations inspected.
- **Semantic overlap versus implementation duplicate:** The candidate's pairwise relationship drift is adjacent to trust and grudge memory fields but has different identity and state scope. Do not duplicate per-NPC memory decay or claim the candidate's relationship behavior is fully implemented.
- **Recommended action:** `LINK`.
- **Reviewer conclusion and date:** `RELATED_DISTINCT` candidate for the NPC-memory cluster. Preserve pairwise relations and per-NPC episodic memory as separate authorities. No plan status or body changes. Reviewed 2026-10-01.
