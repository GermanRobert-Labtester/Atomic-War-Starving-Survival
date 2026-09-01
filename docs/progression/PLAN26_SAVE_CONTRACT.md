# Plan 26 Save Contract

> **Document Status:** Authoritative Save / Load Contract
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. Save Section Persistence

Plan 26 maintains complete backwards compatibility with existing campaign saves while extending progression state persistence.

### Stateful Progression Envelopes:
1. **`ResearchState` (`research` section):**
   - `systemId = "research_system"`
   - `unlockedIds` (List<string>)
   - `completedIds` (List<string>)
   - `activeResearchId` (string)
   - `activeResearchDays` (int)
   - `expansionUnlocked` (bool)
   - `currentDay` (int)
2. **`SkillProgressionState` (`skills` section):**
   - `actorId` (string)
   - `disciplineIds` (List<string>)
   - `disciplineXp` (List<float>)
   - `lastUsedDays` (List<int>)
   - `activeSkillIds` (List<string>)
   - `dormantSkillIds` (List<string>)
   - `expertSkillEarned` (bool)
3. **`TradeSpecialtySaveState` (`trade_specialties` section):**
   - `systemId = "trade_specialty_system"`
   - `survivorId` (string)
   - `professionId` (string)
   - `craftMilestonesCompleted` (List<string>)
   - `mastered` (bool)
4. **`LatentAwakeningSaveState` (`latent_awakening` section):**
   - `systemId = "latent_expert_awakening_system"`
   - `records` (List<LatentAwakeningRecord>)

---

## 2. Invariant & Checksum Guarantees

- **Invariant 3:** Saves written before Plan 26 load seamlessly without loss of completed research or active skills.
- **Invariant 4 (Determinism):** State restoration is 100% deterministic with invariant ordinal sorting on all serialized dictionary key collections.
