# Plan 12 — Social State Authority Map

> **Purpose:** Map every piece of social state to its single authoritative owner, the exact DTO/class/method that holds it, what Plan 12 reads and writes, and the save/load contract. This document is the canonical reference for "who owns what" when Plan 12 content touches social mechanics.

---

## The One-Authority-Per-State Rule

**No shadow copies. No parallel counters. No duplicate truth.**

Every piece of social state in ASHFALL has exactly one authoritative owner. When Plan 12 content needs to read or write social state, it goes through that owner's public API. No system may maintain a private copy of another system's data. No host may cache social state outside the Core system's `CaptureState`/`RestoreState` contract.

This rule exists because:

1. **Save/load correctness** — `SaveChecksum` hashes one source of truth per section. Two copies means two hashes that can disagree.
2. **Determinism** — `ISeededRng` drives all stochastic transitions. A shadow copy with a different RNG stream produces divergent replays.
3. **Cross-host compatibility** — Godot reads what Core writes. A parallel counter in the host would not serialize into the campaign envelope.
4. **Auditability** — when a bug report says "affinity was wrong," there is exactly one place to look.

**Enforcement:** `SaveStoreCoverageGateTests` source-scans every `*SaveStore*.cs` and fails CI if any store has neither a checksum envelope nor Core-codec delegation. `CatalogIntegrityValidator` cross-checks ID references. The `SurvivorSocialCoordinator` aggregates five subsystems into ONE save section (`SurvivorSocialSaveState`) — not five loose files.

---

## State Authority Table

| # | State | Authoritative Owner | File | Key Methods | Plan 12 Use |
|---|-------|-------------------|------|-------------|-------------|
| 1 | Survivor age/cohort | `CohortSystem` | `Assets/Ashfall.Core/CohortSystem.cs` | `BookChild`, `TryMaturation`, `GetChild` | Coming-of-age eligibility, maturation |
| 2 | Parent/lineage | `GenerationalLineageExtension` + `GenerationalSuccessionEngine` | `Assets/Ashfall.Core/GenerationalLineageExtension.cs`, `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` | `EstablishLineage`, `GetLineage`, `GetParent`, `RegisterDweller`, `AdvanceTime` | Raised-by/guardian outcomes, generation tracking |
| 3 | Apprenticeship | `ApprenticeshipSystem` | `Assets/Ashfall.Core/ApprenticeshipSystem.cs` | `StartPair`, `TickDay`, `State.completedSkillIds` | Six authored arcs, skill grants |
| 4 | Skill progression | `SkillProgressionSystem` | `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` | `RecordAction`, `GetLevel`, `GetXp` | Schooling/apprentice payoff |
| 5 | Relationship | `SurvivorRelationsSystem` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` | `GetOrCreateRelationship`, `ModifyAffinity`, `ModifyTrust`, `ApplyGrief`, `TryTriggerConflict` | Mediation/mentor bonds, kinship |
| 6 | Ideological friction | `IdeologicalFrictionSystem` | `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` | `RegisterBelief`, `GetRoommateCompatibilityMultiplier`, `ConflictGroups` | 4 belief sets, social event eligibility |
| 7 | Ration grievance | `RationConflictSystem` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | `SetAllocation`, `Tick`, `OnResentmentEvent` | Ration disputes/escalation |
| 8 | Leadership | `LeadershipSystem` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` | `DesignateLeader`, `StepDown`, `GetLeaderStress` | Challenge consequences |
| 9 | Morale | `NeedsSystem` | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | `Modify(survivorId, NeedKind.Morale, delta)`, `Get(survivorId, NeedKind.Morale)` | Localized decor effect, social morale feedback |
| 10 | Memorial | `MemorialSystem` | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` | `Memorialize`, `Entries`, `CaptureState` | Plaque provenance, grief cascade |
| 11 | Room assignment | `ShelterAssignmentSystem` | `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | `GetAssignmentForSurvivor`, `GetAssignmentsForRoom`, `AreInSameRoom` | Occupant lookup for decor |
| 12 | Decor assignment | `ShelterDecorSystem` | `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | `Assign`, `Remove`, `GetSlot`, `GetRoomMoraleDelta` | Room expression, memorial plaques |
| 13 | Social coordination | `SurvivorSocialCoordinator` | `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` | `TickDay`, `CaptureState`, `RestoreState`, `RegisterBelief`, `SetAliveSurvivors` | Cross-system orchestration |

---

## Detailed State Reference

### 1. Survivor Age / Cohort — `CohortSystem`

**File:** `Assets/Ashfall.Core/CohortSystem.cs` (175 lines)

**DTO:**
```csharp
[Serializable]
public class CohortChild
{
    public string survivorId;
    public List<string> parentIds;
    public string guessBand;       // "low" | "medium" | "high"
    public string trueBand;        // corrected later, empty until known
    public int birthDay;
    public bool baselineCorrected;
    public string moralityMemory;  // the story told, not the dose
    public bool isMatured;         // Plan 12A — one-way maturation flag
    public int maturationDay;
}

[Serializable]
public class CohortSystemState
{
    public string systemId = CohortSystem.SystemId;  // "cohort_system"
    public List<CohortChild> children;
}
```

**CaptureState/RestoreState:**
- `CaptureState()` — deep-copies children in ordinal-sorted key order (deterministic cross-host serialization). Returns `CohortSystemState`.
- `RestoreState(CohortSystemState saved)` — clears internal dictionaries, re-populates from saved list. Null-safe per-child; skips entries with empty `survivorId`.

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnChildBooked` | `(childId, guessBand)` | `BookChild` succeeds |
| `OnBaselineCorrected` | `(childId, trueBand)` | `CorrectBaseline` succeeds |
| `OnMaturation` | `(childId, day)` | `TryMaturation` succeeds (one-way, idempotent) |
| `OnStateChanged` | `(CohortSystemState)` | Any mutation |

**Plan 12 reads:**
- `GetChild(childId)` → `CohortChild.isMatured` — gates coming-of-age event eligibility
- `Children` list — iterates for maturation eligibility checks
- `CohortChild.parentIds` — lineage/raised-by lookups for adoption arcs

**Plan 12 writes:**
- `TryMaturation(childId, day)` — sets `isMatured = true`, `maturationDay = day` (one-way, irreversible)

**Old-save compatibility:**
- `isMatured` defaults to `false` (C# default for `bool`)
- `maturationDay` defaults to `0`
- Pre-Plan-12 saves load with all children un-matured; maturation must be re-triggered by game logic

---

### 2. Parent / Lineage — `GenerationalLineageExtension` + `GenerationalSuccessionEngine`

**Files:**
- `Assets/Ashfall.Core/GenerationalLineageExtension.cs` (120 lines)
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` (165 lines)

**DTOs:**
```csharp
// GenerationalLineageExtension
[Serializable]
public sealed class LineageRecord
{
    public string parentId;
    public string childId;
    public string relationshipType;  // "parent", "adopted", "mentor"
    public int establishedDay;
    public bool isActive = true;
    public List<string> inheritedTraitIds;
}

[Serializable]
public sealed class LineageState
{
    public string systemId = "generational_lineage";
    public List<LineageRecord> lineages;
}

// GenerationalSuccessionEngine
[Serializable]
public sealed class DwellerGenerationRecord
{
    public string dwellerId;
    public int generationIndex;  // 0 = founder, 1 = firstborn, 2 = grandchild
    public int inGameAgeYears;
    public bool isRetired;
    public bool isDeceased;
    public string mentorDwellerId;
    public List<string> inheritedTraitIds;
}

[Serializable]
public sealed class GenerationalSuccessionSaveState
{
    public int currentChapterIndex;
    public int daysElapsedInChapter;
    public int totalYearsElapsed;
    public List<DwellerGenerationRecord> generationRecords;
}
```

**CaptureState/RestoreState:**
- `GenerationalLineageExtension.CaptureState()` — deep-clones via `SystemTextJsonSerializer` round-trip. Returns `LineageState`.
- `GenerationalLineageExtension.RestoreState(LineageState saved)` — replaces internal state via JSON clone.
- `GenerationalSuccessionEngine.CaptureState()` — copies all fields + per-record deep copy of `inheritedTraitIds`. Returns `GenerationalSuccessionSaveState`.
- `GenerationalSuccessionEngine.RestoreState(GenerationalSuccessionSaveState)` — clears `_records`, rebuilds from saved list. Defaults `currentChapterIndex` to 1 if ≤ 0.

**Events:**
| Event | Args | Source |
|-------|------|--------|
| `OnLineageEstablished` | `(parentId, childId)` | `GenerationalLineageExtension` |
| `OnSuccessionPerformed` | `(retireeId, successorId)` | `GenerationalLineageExtension` |
| `OnLineageChanged` | `()` | `GenerationalLineageExtension` |
| `OnDwellerRetired` | `(dwellerId, ageYears)` | `GenerationalSuccessionEngine` |
| `OnTraitInherited` | `(mentorId, apprenticeId, traitId)` | `GenerationalSuccessionEngine` |
| `OnChapterAdvanced` | `(chapterIndex)` | `GenerationalSuccessionEngine` |

**Plan 12 reads:**
- `GetParent(dwellerId)` → `LineageRecord` — raised-by/guardian lookup for adoption arcs
- `GetLineage(dwellerId)` → `List<LineageRecord>` — full lineage chain for generation tracking
- `GetRecord(dwellerId)` → `DwellerGenerationRecord` — generation index, age, retirement status

**Plan 12 writes:**
- `EstablishLineage(parentId, childId, relationshipType)` — creates `LineageRecord`, registers dweller in engine
- `RegisterDweller(dwellerId, age, generation)` — adds to generation tracking
- `AdvanceTime(days)` — ages all dwellers, triggers auto-retirement at age 65

**Old-save compatibility:**
- `LineageRecord.isActive` defaults to `true`
- `DwellerGenerationRecord.isRetired` / `isDeceased` default to `false`
- `generationIndex` defaults to `0` (founder)
- Pre-existing saves with no lineage records load as empty; generation tracking starts fresh

---

### 3. Apprenticeship — `ApprenticeshipSystem`

**File:** `Assets/Ashfall.Core/ApprenticeshipSystem.cs` (154 lines)

**DTOs:**
```csharp
[Serializable]
public sealed class Apprenticeship
{
    public string pairId;
    public string mentorId;
    public string apprenticeId;
    public string targetSkillId;
    public float progressXp;
    public float targetXp = 100f;
    public int dayStarted = -1;
    public bool isComplete;
    public bool isCancelled;
    public string milestonePerkId;
}

[Serializable]
public sealed class ApprenticeshipState
{
    public string systemId = "apprenticeship";
    public List<Apprenticeship> activePairs;
    public List<string> completedSkillIds;
}
```

**CaptureState/RestoreState:**
- `CaptureState()` — deep-clones via `SystemTextJsonSerializer` round-trip. Returns `ApprenticeshipState`.
- `RestoreState(ApprenticeshipState saved)` — replaces internal state via JSON clone.

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnApprenticeshipCompleted` | `(Apprenticeship)` | Pair reaches `targetXp` |
| `OnApprenticeshipChanged` | `()` | Any mutation (start, cancel, tick, complete) |

**Plan 12 reads:**
- `GetActivePairs()` → `List<Apprenticeship>` — UI display of in-progress mentorships
- `State.completedSkillIds` — gates "already learned" checks for authored arcs
- `Apprenticeship.progressXp / targetXp` — progress display

**Plan 12 writes:**
- `StartPair(mentorId, apprenticeId, targetSkillId, targetXp)` — creates new `Apprenticeship` entry
- `CancelPair(pairId)` — marks `isCancelled = true`
- `TickDay(day)` — advances `progressXp` by 10/day; on completion, calls `SkillProgressionSystem.RecordAction` and `SurvivorRelationsSystem.ModifyAffinity(+10)`

**Old-save compatibility:**
- `Apprenticeship.isComplete` / `isCancelled` default to `false`
- `progressXp` defaults to `0`
- `dayStarted` defaults to `-1`
- Pre-Plan-12 saves load with no active pairs; apprenticeship arcs must be re-started

---

### 4. Skill Progression — `SkillProgressionSystem`

**File:** `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` (729 lines)

**DTO:** Per-survivor `SkillProgressionState` (internal dictionary keyed by `survivorId`, ordinal comparison).

**Key Constants:**
- `DefaultXpPerAction = 5f`
- `DormantAfterUnusedDays = 14`
- `EpiphanyMoraleThreshold = 10f`
- `EpiphanyChance = 0.05f`
- Six disciplines: `medical`, `crafting`, `science`, `combat`, `scavenging`, `survival`

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnXpGained` | `(SkillActor, discipline, newXp)` | Action-driven XP gain |
| `OnSkillEarned` | `(SkillActor, skillId)` | Skill unlocked |
| `OnSkillDormant` | `(SkillActor, skillId)` | 14 days unused → dormant |
| `OnSkillReactivated` | `(SkillActor, skillId)` | Dormant skill re-used |
| `OnEpiphany` | `(SkillActor, highlightSkillId?)` | Stress-triggered instant mastery |

**Plan 12 reads:**
- `GetLevel(actor, skillId)` / `GetXp(actor, skillId)` — schooling/apprentice payoff checks
- `CatalogCount` — integrity verification

**Plan 12 writes:**
- `RecordAction(actor, skillId, xp, day)` — called by `ApprenticeshipSystem.TickDay` on completion
- Host-wired hooks: `ActionXpMultiplier`, `BunkerSkillDecayStopped`, `MaxMoraleCap`, `ApplyMorale`

**Old-save compatibility:**
- Skills default to 0 XP; dormant state resets on load (recomputed from last-use day)
- Epiphany is non-persistent (morale restore is a one-shot host-side effect)

---

### 5. Relationship — `SurvivorRelationsSystem`

**File:** `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` (263 lines)

**DTOs:**
```csharp
[Serializable]
public sealed class RelationshipEntry
{
    public string dwellerA;
    public string dwellerB;
    public float affinity;      // -100 to 100
    public float trust;         // 0 to 100
    public float resentment;    // 0 to 100
    public float grief;         // 0 to 100
    public string bondType;     // "friendship", "rivalry", "mentor", "caregiver"
    public List<string> recentCauses;
}

[Serializable]
public sealed class ConflictEntry
{
    public string conflictId;
    public string dwellerA;
    public string dwellerB;
    public string cause;
    public int dayStarted;
    public bool isResolved;
    public string resolution;
}

[Serializable]
public sealed class MediationEntry
{
    public string conflictId;
    public int day;
    public string mediatorId;
    public string outcome;
    public float affinityChange;
}

[Serializable]
public sealed class SurvivorRelationsState
{
    public string systemId = "survivor_relations";
    public List<RelationshipEntry> relationships;
    public List<ConflictEntry> activeConflicts;
    public List<MediationEntry> mediationHistory;
}
```

**CaptureState/RestoreState:**
- `CaptureState()` — deep-clones via `SystemTextJsonSerializer` round-trip. Returns `SurvivorRelationsState`.
- `RestoreState(SurvivorRelationsState saved)` — replaces internal state via JSON clone.

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnConflictStarted` | `(ConflictEntry)` | `TryTriggerConflict` fires (10% daily chance) |
| `OnConflictResolved` | `(MediationEntry)` | `TryMediate` succeeds |
| `OnRelationsChanged` | `()` | Any mutation |

**Plan 12 reads:**
- `GetOrCreateRelationship(a, b)` → `RelationshipEntry` — affinity/trust/resentment/grief for mediation events
- `activeConflicts` — conflict event eligibility
- `mediationHistory` — outcome tracking

**Plan 12 writes:**
- `ModifyAffinity(a, b, delta)` — called by `SurvivorSocialCoordinator` (friction, apprenticeship completion, trauma bonds)
- `ModifyTrust(a, b, delta)` — trust adjustments from social events
- `ApplyGrief(survivorId, amount)` — called by `MemorialSystem.GriefSink` on death

**Old-save compatibility:**
- All float fields default to `0`
- `bondType` defaults to empty string (no bond)
- `recentCauses` defaults to empty list
- Pre-existing saves load with neutral relationships; social history is lost but mechanically safe

---

### 6. Ideological Friction — `IdeologicalFrictionSystem`

**File:** `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` (159 lines)

**DTOs:**
```csharp
[Serializable]
public sealed class IdeologicalAffinityEntry
{
    public string pairKey;   // "survivorA|survivorB" (ordinal-sorted)
    public float affinity;
}

[Serializable]
public sealed class IdeologicalFrictionSaveState
{
    public List<IdeologicalAffinityEntry> affinities;
}
```

**Key Constants:**
- `ConflictSleepQualityPenalty = 0.20f`
- `SynergySleepQualityBonus = 0.10f`
- `ConflictAffinityDrainPerDay = 2f`
- `SynergyAffinityGainPerDay = 1f`

**Conflict Groups (7 base + 4 Plan 12B beliefs):**
- `military_discipline` ↔ `pragmatic_individualism`, `pacifist`, `religious_faith`
- `religious_faith` ↔ `atheist_rationalist`, `military_discipline`
- `atheist_rationalist` ↔ `religious_faith`, `superstitious_traditional`
- `pragmatic_individualism` ↔ `collectivist_solidarity`, `military_discipline`
- `collectivist_solidarity` ↔ `pragmatic_individualism`
- `superstitious_traditional` ↔ `atheist_rationalist`
- `pacifist` ↔ `military_discipline`
- **Plan 12B:** `belief_ration_collectivist`, `belief_every_soul_alone`, `belief_faith_in_rebuild`, `belief_ash_nihilist`

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnFrictionDetected` | `(survivorA, survivorB, penalty)` | Roommate compatibility check finds conflict |
| `OnRoommateSynergy` | `(survivorA, survivorB)` | Same belief profile |
| `OnAffinityChanged` | `(survivorA, survivorB, delta)` | Daily affinity drift |
| `OnStateChanged` | `()` | Any mutation |

**Plan 12 reads:**
- `GetRoommateCompatibilityMultiplier(a, b)` — sleep quality modifier for shared rooms
- `GetBelief(survivorId)` — event eligibility gating
- `ConflictGroups` — static dictionary, read by content authoring tools

**Plan 12 writes:**
- `RegisterBelief(survivorId, beliefProfileId)` — called by `SurvivorSocialCoordinator.RegisterBelief`
- Internal affinity drift via daily tick (consumed by `SurvivorSocialCoordinator` → `SurvivorRelationsSystem.ModifyAffinity`)

**Old-save compatibility:**
- Affinities default to `0` (neutral)
- Beliefs default to empty string (no belief registered → compatibility multiplier returns `1f`)
- Pre-Plan-12 saves load with no friction; beliefs must be re-registered by host

---

### 7. Ration Grievance — `RationConflictSystem`

**File:** `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` (184 lines)

**DTOs:**
```csharp
[Serializable]
public sealed class RationConflictSurvivorState
{
    public string survivorId;
    public float perceivedFairness;
    public string resentmentTargetId;
    public float resentmentLevel;
}

[Serializable]
public sealed class RationConflictSaveState
{
    public List<RationConflictSurvivorState> survivors;
}
```

**Key Constants:**
- `FairnessDeviationThreshold = 0.20f`
- `ResentmentGainPerDay = 0.10f`
- `ResentmentDecayPerDay = 0.03f`
- `ConfrontationThreshold = 0.70f`
- `TheftThreshold = 0.85f`
- `ConfrontationMoraleHit = -10f`
- `TheftMoraleHit = -15f`

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnResentmentBuilt` | `(survivorId, targetId, level)` | Daily tick finds allocation deviation |
| `OnRationConfrontation` | `(survivorId, targetId)` | Resentment ≥ 0.70 |
| `OnRationsStolen` | `(survivorId, targetId)` | Resentment ≥ 0.85 |
| `OnMoraleDelta` | `(survivorId, delta)` | Confrontation/theft morale hit |
| `OnStateChanged` | `()` | Any mutation |

**Plan 12 reads:**
- `GetState(survivorId)` → `RationConflictSurvivorState` — perceived fairness, resentment level/target
- `GetAllocation(survivorId)` / `GetAverageAllocation()` — ration display

**Plan 12 writes:**
- `SetAllocation(survivorId, allocation)` — called by host from `StartingLevelSystem` ration policy
- `Tick(survivorId, gameHours)` — daily resentment accumulation/decay
- `OnResentmentEvent` — external trigger for escalation

**Old-save compatibility:**
- `perceivedFairness` defaults to `0`
- `resentmentLevel` defaults to `0`
- `resentmentTargetId` defaults to empty string
- Default allocation (no registration) returns `0.5f`
- Pre-Plan-12 saves load with zero resentment; grievances must rebuild from allocation ticks

---

### 8. Leadership — `LeadershipSystem`

**File:** `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` (288 lines)

**DTOs:**
```csharp
[Serializable]
public class LeadershipSurvivorStateDTO
{
    public string survivor_id;
    public bool is_designated_leader;
    public float leader_stress_accumulation;
    public int leader_deaths_witnessed;
}

[Serializable]
public class LeadershipSaveState
{
    public string current_leader_id;
    public float step_down_cooldown;
    public List<LeadershipSurvivorStateDTO> survivor_states;
}
```

**Key Constants:**
- `LeaderCrisisMoraleAura = 10f`
- `LeaderStressPerDeath = 25f`
- `LeaderStressPerInjury = 10f`
- `LeaderStressDecayPerDay = 2f`
- `LeaderStressMax = 100f`
- `LeaderBreakRiskMultiplier = 3f`
- `StepDownCooldownDays = 14f`

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnLeaderDesignated` | `(survivorId)` | `DesignateLeader` succeeds |
| `OnLeaderSteppedDown` | `(survivorId)` | `StepDown` succeeds |
| `OnLeaderStressIncreased` | `(survivorId, stress)` | Death/injury stress accumulation |
| `OnLeaderBreakRisk` | `(survivorId)` | Stress reaches `LeaderStressMax` |
| `OnStateChanged` | `()` | Any mutation |

**Plan 12 reads:**
- `CurrentLeaderId` — challenge event gating
- `GetLeaderStress(survivorId)` — break-risk display
- `IsDesignatedLeader(survivorId)` — leadership event eligibility

**Plan 12 writes:**
- `DesignateLeader(survivorId)` / `StepDown(survivorId)` — leadership transitions
- Host-wired hooks: `ApplyMoraleDelta`, `ApplyShelterMoraleDelta`, `GetAliveSurvivorIds`

**Old-save compatibility:**
- `current_leader_id` defaults to empty (no leader)
- `step_down_cooldown` defaults to `0`
- `leader_stress_accumulation` defaults to `0`
- Pre-Plan-12 saves load with no designated leader; leadership must be re-established

---

### 9. Morale — `NeedsSystem`

**File:** `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` (346+ lines)

**DTO:**
```csharp
public class SurvivorNeedsState
{
    public string SurvivorId;
    public float Hunger;
    public float Thirst;
    public float Fatigue;
    public float Warmth;
    public float Morale = 50f;   // default starting morale
    public float Hygiene;
    public float Health;
    public float RadiationDose;
}
```

**Key Method:**
```csharp
public void Modify(string survivorId, NeedKind need, float delta)
public void Modify(SurvivorNeedsState survivor, NeedKind need, float delta)
```

**Plan 12 reads:**
- `Get(survivorId, NeedKind.Morale)` — morale display, event eligibility thresholds
- Morale affects `SkillProgressionSystem` epiphany chance (below `EpiphanyMoraleThreshold = 10f`)

**Plan 12 writes:**
- `Modify(survivorId, NeedKind.Morale, delta)` — called by:
  - `LeadershipSystem.ApplyMoraleDelta` (crisis morale aura)
  - `RationConflictSystem.OnMoraleDelta` (confrontation/theft hits)
  - `ShelterDecorSystem.GetRoomMoraleDelta` (localized decor effect)
  - `SkillProgressionSystem.ApplyMorale` (epiphany restore)

**Old-save compatibility:**
- `Morale` defaults to `50f` (neutral)
- Pre-existing saves load with default morale; no morale history is preserved

---

### 10. Memorial — `MemorialSystem`

**File:** `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` (292 lines)

**DTOs:**
```csharp
public enum DeathQuality { Unattended = 0, Rushed = 1, Peaceful = 2 }
public enum MemorialOutcome { Burial = 0, WallEntry = 1, AshScatter = 2 }

[Serializable]
public sealed class MemorialEntry
{
    public string SurvivorId;
    public string Cause;
    public int Day;
    public int SurvivedDays;
    public bool FinalWishResolved;
    public string Epitaph;
    public string HeirloomItemId;
    public string HeirloomRecipientId;
    public float MoraleDelta;
    public DeathQuality DeathQuality;     // Plan 09 9C
    public MemorialOutcome Outcome;       // Plan 09 9C
}

[Serializable]
public sealed class MemorialState
{
    public List<MemorialEntry> Entries;
    // Capture() / RestoreInto() for save round-trip
}
```

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnMemorialized` | `(MemorialEntry)` | `Memorialize` succeeds (idempotent) |

**Grief Sink:**
- `IGriefSink.ApplyDispersion(deceasedId, survivingRelationshipIds, baseGriefAmount, quality, day)`
- Quality scale: `Peaceful = 0.5f`, `Rushed = 1.0f`, `Unattended = 1.25f`
- Default `CapturingGriefSink` records dispersions for test assertion; host wires real `SurvivorRelationsSystem.ApplyGrief`

**Plan 12 reads:**
- `Entries` → `IReadOnlyList<MemorialEntry>` — plaque provenance, heirloom tracking
- `MemorialEntry.HeirloomItemId` — `ShelterDecorSystem.ResolvePlaqueSlot` cross-link

**Plan 12 writes:**
- `Memorialize(MemorialInput)` — idempotent; records death, fires grief cascade, transfers heirloom

**Old-save compatibility:**
- `DeathQuality` defaults to `Unattended` (enum 0) — but existing saves with `Peaceful` semantics load correctly because the field is additive
- `MemorialOutcome` defaults to `Burial` (enum 0) — matches pre-9C behaviour
- Pre-Plan-12 saves load with existing entries preserved; new fields default to safe values

---

### 11. Room Assignment — `ShelterAssignmentSystem`

**File:** `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` (395 lines)

**DTOs:**
```csharp
public sealed class ShelterRoom
{
    public string RoomId;
    public int Capacity;
    // ... room metadata
}

public sealed class ShelterAssignmentState
{
    public List<ShelterAssignment> Assignments;
    // Capture() / RestoreInto() for save round-trip
}
```

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnAssignmentChanged` | `(ShelterAssignmentEvent)` | Assignment mutation |

**Plan 12 reads:**
- `GetAssignmentForSurvivor(survivorId)` → `ShelterAssignment?` — which room a survivor occupies
- `GetAssignmentsForRoom(roomId)` → `IReadOnlyList<ShelterAssignment>` — who is in a room (for decor visibility)
- `AreInSameRoom(survivorA, survivorB)` → `bool` — roommate checks for friction/ration events
- `GetRoomOccupancy(roomId)` / `GetRoomCapacity(roomId)` — capacity checks

**Plan 12 writes:**
- Plan 12 does NOT write room assignments directly; that is the host's responsibility via `ShelterAssignmentSystem.Assign`
- Plan 12 reads occupancy to determine decor visibility and friction eligibility

**Old-save compatibility:**
- Assignments persist through `ShelterAssignmentState.Capture()`/`RestoreInto()`
- Pre-existing saves load with assignments preserved

---

### 12. Decor Assignment — `ShelterDecorSystem`

**File:** `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` (301 lines)

**DTOs:**
```csharp
[Serializable]
public class ShelterDecorPlacement
{
    public string RoomId;
    public string SlotId;
    public string ItemId;
    public int DayInstalled;
    public bool IsMemorialPlaque;
    public string MemorialSurvivorId;
    public string PlaqueSourceHeirloomId;
}

[Serializable]
public class ShelterDecorState
{
    public string systemId = "shelter_decor";
    public string Checksum;
    public List<ShelterDecorPlacement> Placements;
}

[Serializable]
public sealed class ShelterDecorStateCapture : ShelterDecorState { }
```

**Events:**
| Event | Args | Trigger |
|-------|------|---------|
| `OnDecorChanged` | `(ShelterDecorPlacement)` | `Assign` / `Remove` |
| `OnStateChanged` | `()` | Any mutation |

**Plan 12 reads:**
- `GetSlot(roomId, slotId)` → `ShelterDecorPlacement` — what is placed where
- `GetRoomMoraleDelta(roomId)` → `float` — localized morale modifier for `NeedsSystem.Modify`
- `ItemModifiers` → `IReadOnlyDictionary<string, ShelterDecorItemModifier>` — catalog of decor item effects
- `ResolvePlaqueSlot(heirloomItemId)` — memorial plaque auto-placement cross-link

**Plan 12 writes:**
- `Assign(roomId, slotId, itemId, dayInstalled)` — places a decor item
- `Remove(roomId, slotId)` — clears a slot
- Memorial plaque auto-publish: when `MemorialEntry.HeirloomItemId` matches a known plaque item, the host calls `Assign` with `IsMemorialPlaque = true`

**Old-save compatibility:**
- `IsMemorialPlaque` defaults to `false`
- `MemorialSurvivorId` / `PlaqueSourceHeirloomId` default to empty string
- `Checksum` defaults to empty string (envelope adds it on save)
- Pre-Plan-12 saves load with empty placement list; decor must be re-assigned

---

### 13. Social Coordination — `SurvivorSocialCoordinator`

**File:** `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` (263+ lines)

**DTO:**
```csharp
[Serializable]
public sealed class SurvivorSocialSaveState
{
    public LeadershipSaveState leadership;
    public IdeologicalFrictionSaveState friction;
    public RationConflictSaveState ration;
    public TraumaBondSaveState trauma;
    public SkillAtrophySaveState atrophy;
}
```

**Owned Subsystems:**
- `LeadershipSystem Leadership`
- `IdeologicalFrictionSystem Friction`
- `RationConflictSystem Ration`
- `TraumaBondSystem TraumaBond`
- `SkillAtrophySystem Atrophy`

**Hook Wiring (internal):**
- `Leadership.ApplyMoraleDelta` → `NeedsSystem.Modify(id, NeedKind.Morale, delta)`
- `Leadership.ApplyShelterMoraleDelta` → broadcasts to all alive survivors
- `TraumaBond.AdjustAffinity` → `SurvivorRelationsSystem.ModifyAffinity(a, b, delta)`
- `TraumaBond.AreOnSameShift` → `DutyRosterSystem.GetRoleOf` comparison
- `Friction.OnAffinityChanged` → `SurvivorRelationsSystem.ModifyAffinity(a, b, delta)`
- `Ration.OnMoraleDelta` → `NeedsSystem.Modify(id, NeedKind.Morale, delta)`

**CaptureState/RestoreState:**
- `CaptureState()` → `SurvivorSocialSaveState` — aggregates all five subsystem captures into ONE section
- `RestoreState(SurvivorSocialSaveState)` — dispatches to each subsystem's `RestoreState`

**Plan 12 reads:**
- `BuildReadModel()` → `SurvivorSocialReadModel` — aggregated view for UI panels
- Direct access to subsystem properties: `Leadership.CurrentLeaderId`, `Friction.GetBelief`, etc.

**Plan 12 writes:**
- `RegisterBelief(survivorId, beliefProfileId)` — forwards to `Friction.RegisterBelief`
- `SetAliveSurvivors(aliveIds)` — refreshes alive list, registers in `Ration`
- `TickDay(day, survivors)` — advances all five subsystems

**Old-save compatibility:**
- All five sub-states default to empty/zero on missing save data
- Pre-Plan-12 saves load with no social state; all systems start from defaults

---

## Cross-System Data Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│                    SurvivorSocialCoordinator                         │
│  ┌─────────────┐  ┌──────────────┐  ┌───────────────┐              │
│  │ Leadership   │  │ Friction     │  │ RationConflict│              │
│  └──────┬───────┘  └──────┬───────┘  └───────┬───────┘              │
│         │                 │                   │                      │
│         ▼                 ▼                   ▼                      │
│  ┌──────────────────────────────────────────────────────┐           │
│  │              NeedsSystem (Morale)                     │           │
│  └──────────────────────────────────────────────────────┘           │
│                                                                     │
│  ┌─────────────┐  ┌──────────────┐                                  │
│  │ TraumaBond   │  │ SkillAtrophy │                                  │
│  └──────┬───────┘  └──────────────┘                                  │
│         │                                                            │
│         ▼                                                            │
│  ┌──────────────────────────────────────────────────────┐           │
│  │         SurvivorRelationsSystem (Affinity)            │           │
│  └──────────────────────────────────────────────────────┘           │
└─────────────────────────────────────────────────────────────────────┘

┌───────────────────────┐     ┌───────────────────────┐
│   CohortSystem        │     │  GenerationalLineage   │
│   (age/maturation)    │     │  Extension + Engine    │
└───────────┬───────────┘     └───────────┬───────────┘
            │                              │
            ▼                              ▼
┌───────────────────────┐     ┌───────────────────────┐
│  ApprenticeshipSystem │     │  SkillProgression     │
│  (mentor/apprentice)  │────▶│  System (XP/levels)   │
└───────────────────────┘     └───────────────────────┘

┌───────────────────────┐     ┌───────────────────────┐
│  ShelterAssignment    │     │  ShelterDecorSystem    │
│  (room occupancy)     │◀────│  (placements, morale)  │
└───────────────────────┘     └───────────────────────┘
            │                              ▲
            │                              │
            ▼                              │
┌───────────────────────┐                  │
│  MemorialSystem       │──────────────────┘
│  (death → plaque)     │   ResolvePlaqueSlot
└───────────────────────┘
```

---

## Save Section Registry

All Plan 12 social state persists through the campaign envelope (`campaign.json`). The relevant sections:

| Section Key | System | Save DTO | Envelope |
|-------------|--------|----------|----------|
| `cohort` | `CohortSystem` | `CohortSystemState` | Checksummed |
| `generational_lineage` | `GenerationalLineageExtension` | `LineageState` | Checksummed |
| `generational_succession` | `GenerationalSuccessionEngine` | `GenerationalSuccessionSaveState` | Checksummed |
| `apprenticeship` | `ApprenticeshipSystem` | `ApprenticeshipState` | Checksummed |
| `survivor_relations` | `SurvivorRelationsSystem` | `SurvivorRelationsState` | Checksummed |
| `survivor_social` | `SurvivorSocialCoordinator` | `SurvivorSocialSaveState` | Checksummed (aggregates 5 subsystems) |
| `shelter_assignment` | `ShelterAssignmentSystem` | `ShelterAssignmentState` | Checksummed |
| `shelter_decor` | `ShelterDecorSystem` | `ShelterDecorStateCapture` | Checksummed |
| `memorial` | `MemorialSystem` | `MemorialState` | Checksummed |
| `needs` | `NeedsSystem` | (per-survivor needs) | Checksummed (via host session) |
| `skill_progression` | `SkillProgressionSystem` | (per-survivor skills) | Checksummed (via host session) |

---

## Verification

This document was compiled from the actual source files on 2026-09-01. All class names, method signatures, DTO fields, event names, and constant values were verified against:

- `Assets/Ashfall.Core/CohortSystem.cs` (175 lines)
- `Assets/Ashfall.Core/GenerationalLineageExtension.cs` (120 lines)
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` (165 lines)
- `Assets/Ashfall.Core/ApprenticeshipSystem.cs` (154 lines)
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` (729 lines)
- `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` (263 lines)
- `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` (159 lines)
- `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` (184 lines)
- `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` (288 lines)
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` (346+ lines)
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` (292 lines)
- `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` (395 lines)
- `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` (301 lines)
- `Assets/Ashfall.Core/Survivors/SurvivorSocialCoordinator.cs` (263+ lines)

---

## Rules for Plan 12 Content Authors

1. **Read from the authority.** Never cache social state in event scripts or quest logic. Call the system's public query method.
2. **Write through the API.** Never mutate DTO fields directly. Use the system's mutation methods so events fire and checksums stay valid.
3. **One writer per field.** If two systems need to modify the same value, they must go through one owner. Example: morale is only modified via `NeedsSystem.Modify(id, NeedKind.Morale, delta)`.
4. **Save/load is automatic.** If the system implements `CaptureState`/`RestoreState`, the campaign envelope handles persistence. Do not add manual save logic.
5. **Default-safe.** Every DTO field has a safe default (0, false, empty string). Pre-Plan-12 saves load without errors; content must handle the "no data yet" case gracefully.
6. **Deterministic.** All stochastic transitions use `ISeededRng`. Never use `System.Random` or `Guid.NewGuid()` in Plan 12 content.
