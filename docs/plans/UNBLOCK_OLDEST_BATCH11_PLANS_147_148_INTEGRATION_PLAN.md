# Batch 11 — Unblock Oldest Partial Plans: Plans 147 + 148

**Package:** `UNBLOCK-OLDEST-BATCH11-PLANS`
**Claim:** `claim-unblock-oldest-batch11-plans-2026-09-23`
**Date:** 2026-09-23
**Status:** ACTIVE
**Predecessor:** `UNBLOCK-OLDEST-BATCH10-PLANS` (Plans 141 + 145), which completed Plan 141 research unlock bridge and Plan 145 unified ending resolver full host integration.

---

## 1. Scope and Selection

Per `docs/plans/OLDEST_PARTIAL_PLANS_AUDIT_20_2026-09-23.md`, following the full integration of Plans 141 and 145, the next two oldest partial plans with 0 host references are:
1. **Plan 147:** `Per-NPC Memory & Relationship Depth` (`NpcMemorySystem` — `Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs`).
2. **Plan 148:** `Ideological Friction → Events & Quests` (`IdeologicalFrictionEvents` — `Assets/Ashfall.Core/Survivors/IdeologicalFrictionEvents.cs` and `IdeologicalFrictionSystem` — `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`).

---

## 2. Premise Audit — Current Evidence

| System / Asset | Current Evidence | Verdict |
|---|---|---|
| `NpcMemorySystem.cs` | Exists in `Assets/Ashfall.Core/Narrative/NpcMemorySystem.cs`. Pure domain logic for per-NPC relationship standing, trust (-100 to +100), grudge (0-100), favor owed (0-100), memory decay, restitution/forgiveness, dialogue tone, and trade price multipliers. Needs `NpcMemoryCensus` struct and `GetCensus()` for architecture scanner. | Core authority live (0 host refs) |
| `npc_memory_dialogue.json` | Exists in `Assets/StreamingAssets/Data/npc_memory_dialogue.json`. 12 authored dialogue templates across 5 emotional tones (`high_trust`, `high_grudge`, `favor_owed`, `betrayed`, `reconciled`). | Valid authored data authority |
| `IdeologicalFrictionEvents.cs` | Exists in `Assets/Ashfall.Core/Survivors/IdeologicalFrictionEvents.cs`. Pure domain authority transforming ideological differences into emergent confrontations, conversion attempts, bunker splits, and mediation quests. Needs `IdeologicalFrictionCensus` struct and `GetCensus()`. | Core authority live (0 host refs) |
| `IdeologicalFrictionSystem.cs` | Exists in `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs`. Tracks roommate compatibility, conflict groups, and pairwise affinities. | Core authority live |
| `ideological_events.json` | Exists in `Assets/StreamingAssets/Data/ideological_events.json`. 8 authored event templates across confrontation, conversion, split, and quest types. | Valid authored data authority |

---

## 3. Architecture & Boundary Rules

1. **One Authority per Concern (Rule 5):**
   - `NpcMemorySystem` is the single authority for per-NPC personal memory, grudge, and trust. Faction-level trust remains with `FactionStanceEngine`.
   - `IdeologicalFrictionEvents` and `IdeologicalFrictionSystem` are the single authority for worldview friction, bunker ideological factions, and mediation quests.
2. **Core Stays Engine-Free (Rule 2):** Neither Core authority shall reference Godot, UnityEngine, or engine serialization.
3. **Deterministic Persistence (Rule 4):** Both save states (`NpcMemorySaveState` and `IdeologicalFrictionEventSaveState`) implement schema versioning and round-trip through `SaveStoreHub.Checksummed<T>`.
4. **Fail-Closed Rollback:** Both host sessions implement phase 5 day owners with `IPreDaySnapshotRestore` pre-day snapshot rollback support.
5. **No Shadow Stores:** Dedicated save sections `npc_memory` and `ideological_friction` registered in `SaveSectionRegistry.cs`.

---

## 4. Phased Execution

- **Phase 1:** Core domain extensions: add `NpcMemoryCensus` and `GetCensus()` to `NpcMemorySystem.cs`; add `IdeologicalFrictionCensus` and `GetCensus()` to `IdeologicalFrictionEvents.cs`.
- **Phase 2:** Save section registration (`npc_memory` #229, `ideological_friction` #230) in `SaveSectionRegistry.cs`. Event vocabulary registration (`npc_memory_ticked`, `ideological_friction_ticked`) in `DayEventVocabulary.cs` and `EVENT_SEMANTIC_PARITY_MATRIX.md`. Update corruption test section pin (228 → 230).
- **Phase 3:** Host sessions: `NpcMemoryHostSession.cs` (with `NpcMemorySaveStore`) and `IdeologicalFrictionHostSession.cs` (with `IdeologicalFrictionSaveStore`).
- **Phase 4:** Campaign orchestration: `Main.NpcMemory.cs`, `Main.IdeologicalFriction.cs`, registration in `Main.CampaignOwners.cs` (phase 5, rollback), `Main.ExpandedShelterSystems.cs`, `Main.SaveOrchestrator.cs`, and `Main.Application.cs`.
- **Phase 5:** CLI self-test probes: `HostCli.NpcMemory.cs` (`--npc-memory-selftest`) and `HostCli.IdeologicalFriction.cs` (`--ideological-friction-selftest`), wired into `HostCliRegistry.cs` and `HostCli.cs`.
- **Phase 6:** Focused integration tests: `Plan147NpcMemoryHostIntegrationTests.cs` and `Plan148IdeologicalFrictionHostIntegrationTests.cs`.
- **Phase 7:** Ratify decisions `DEC-311` and `DEC-312` in `DECISION_REGISTER.md`, regenerate architecture map, and verify 10/10 fast gates.
