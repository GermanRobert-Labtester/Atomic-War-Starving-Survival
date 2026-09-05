# ASHFALL — SURVIVOR PERSONAL QUESTS V2 ARCHITECTURE SPECIFICATION (PLAN 83 / TASK B24)

**Classification:** Social Narrative & Personal Arc Authority
**Author:** AI Pair Programmer / Antigravity
**Status:** Implemented & CI-Gated
**Data Authority:** `Assets/StreamingAssets/Data/personal_quests.json`
**Core System:** `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`
**Host Session:** `src/Host/PersonalQuestHostSession.cs`
**Enforcement Tests:** `Ashfall.Core.Tests/Quests/PersonalQuestSystemTests.cs`
**Host Gate:** `--personal-quests-selftest`

---

## 1. Executive Mission & Background

In historical prototype builds, personal quests existed as a bloated 4,936-line monolithic Unity host system (`PersonalQuestSystem.cs`) that was rightly deleted during the Godot migration.

Personal Quests V2 rebuilds survivor narrative progression from first principles inside `Ashfall.Core` as a lean, data-driven, deterministic, engine-agnostic system. Each survivor who joins the bunker can unlock an intimate multi-stage personal arc rooted in their pre-war background, psychological trauma, or lingering obligations.

---

## 2. Core Architecture & Lifecycle

```
Survivor Joins Roster
       │
       ▼ (Condition: Trait / Day / Morale threshold)
Quest Triggered (`PersonalQuestSystem.TryTriggerQuest`)
       │
       ▼
Stage 1: Investigation / Preparation ──► (Player action / Expedition / Crafting / Dialogue)
       │
       ▼
Stage 2: Critical Moral Choice ────────► Choice A vs Choice B
       │                                     │
       ▼                                     ▼
Stage 3: Resolution / Legacy            Resolution / Consequence
       │                                     │
       ▼                                     ▼
Permanent Trait / Morale Shift          Permanent Trait / Morale Shift
```

### 2.1 Trigger Rules
1. **Trait & Role Affinity**: Quests trigger when a survivor possesses matching background traits (e.g. `medic`, `mechanic`, `scout`, `parent`, `veteran`, `scholar`).
2. **One Active Personal Quest Per Survivor**: A survivor can only progress one personal quest arc at a time.
3. **Survivor Death**: If a survivor dies while their personal quest is active, the quest immediately transitions to `Failed` with reason `"survivor_deceased"`, logging an entry to the memorial wall.

### 2.2 Branching Choices & Moral Dilemmas
Stages can present the player and survivor with mutually exclusive choices:
- **Pragmatic Choice**: Preserves shelter resources or provides immediate survival buffs, but may inflict psychological guilt or friction.
- **Compassionate / Closure Choice**: Sacrifices medicine, rations, or expedition hours to bring emotional closure, boosting long-term morale and granting unique traits (e.g. `peace_of_mind`, `hardened_resolve`).

---

## 3. Authored Quest Arcs (Data Authority)

The canonical catalog `Assets/StreamingAssets/Data/personal_quests.json` defines 10 rich quest arcs:

1. `pq_buried_cache`: **The Sibling's Cache** (Scout / Family) — Searching a collapsed pre-war suburban basement for an insulin stash left for a sibling.
2. `pq_last_confession`: **The Last Confession** (Medic / Guilt) — Unburdening a wartime triage secret regarding a contaminated vaccine batch.
3. `pq_radio_echoes`: **Signals from Sector 4** (Mechanic / Hope) — Tuning antenna arrays to triangulate a repeating Morse code transmission from an old colleague.
4. `pq_lost_journal`: **Pages of the Lost** (Scholar / Memory) — Recovering handwritten field notes scattered across a flooded archive sub-level.
5. `pq_oath_of_iron`: **The Smith's Promise** (Laborer / Duty) — Forging a durable prosthetic bracket using high-grade scrap for an injured comrade.
6. `pq_spores_of_mercy`: **Botanist's Remedy** (Botanist / Healing) — Cultivating an experimental fungal strain in the greenhouse to cure persistent spore-lung cough.
7. `pq_broken_locket`: **The Tarnished Locket** (Survivor / Loss) — Repairing a damaged family heirloom at the workbench before the anniversary of the Strike.
8. `pq_debt_of_blood`: **Blood Debt** (Veteran / Justice) — Deciding whether to exile or forgive a wasteland drifter recognized from a pre-war checkpoint massacre.
9. `pq_silent_choir`: **Hymns of the Concrete** (Teacher / Morale) — Transcribing folk songs from memory to lift the spirits of sheltered children during fallout storms.
10. `pq_watchers_remorse`: **The Watcher's Blindspot** (Guard / Vigilance) — Fortifying an overlooked vent shaft after nightmares of breach and collapse.

---

## 4. Save/Load & Envelope Integration

- **Save Section**: `"personal_quests"` registered in `SaveSectionRegistry.All`.
- **Façade**: `PersonalQuestSaveStore` delegating to `SaveStoreHub.Checksummed<PersonalQuestSaveState>`.
- **Atomic Campaign Envelope**: Persisted inside `campaign.json` during daily dawn commits.
- **Determinism**: State comparisons, choice resolutions, and outcome rolls use `ISeededRng` with strict culture-invariant formatting.
