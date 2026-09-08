# Plan 147 — Per-NPC Memory & Relationship Depth

## Goal

Create a per-NPC memory system where individual NPCs remember specific player actions and change their behavior accordingly. Currently all NPC "memory" is faction-level (via `FactionStanceEngine` trust) or flag-level — individual NPCs don't accumulate a history of player decisions. This plan adds persistent NPC-specific memory that makes each NPC relationship unique and responsive to player behavior.

## Why

**Repository evidence:** The survivor social agent confirmed: "There is no per-NPC memory system that tracks player actions and alters future NPC behavior toward the player." `HoldfastNpcCatalog.cs` (542 lines) defines 10 NPCs with dialogue fragments and trust requirements, but trust is tracked via `FactionStanceEngine` (faction-level), not per-NPC. `VerdictNpcSystem.cs` has flag-gated availability but no memory of specific player actions. `GuiltInsomniaSystem.cs` tracks survivor guilt from decisions but doesn't affect NPC behavior. `DoorEncounterSystem.cs` has `hasGrudgeAgainstLeader` but it's a per-encounter flag, not persistent memory.

**What is missing:** NPCs don't remember what you did to them specifically. If you helped NPC A last week, they don't greet you warmly today. If you refused NPC B's request, they don't hold a grudge. All NPC reactions are based on faction standing or binary flags, not accumulated personal history.

**Why existing plans don't solve it:** Plan 52 (recurring NPC arcs) adds more NPCs but not memory. Plan 132 (hidden agendas) adds survivor secrets but not NPC memory. Plan 139 (combat→faction) connects combat to faction standing but not to individual NPCs. No plan addresses per-NPC persistent memory.

**Player value:** Makes NPC relationships feel personal (they remember you), creates replayability (different actions → different NPC reactions), generates emergent stories (an NPC you helped years ago returns the favor), and deepens emotional investment (NPCs feel like characters, not quest dispensers).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Narrative/HoldfastNpcCatalog.cs` — NPC definitions
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` — faction trust (current "memory")
- `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` — flag-gated NPCs
- `Assets/Ashfall.Core/YearOfAsh/DoorEncounterSystem.cs` — door encounters
- `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs` — flag tracking
- NEW: `Assets/Ashfall.Core/NPC/NpcMemorySystem.cs`

## Main Task 1 — Foundation / System Contract

1. Create `NpcMemorySystem.cs` in `Assets/Ashfall.Core/NPC/`
2. Define `NpcMemoryEntry` DTO: `npcId`, `playerAction` (helped/refused/ignored/betrayed/killed), `targetId` (who action affected), `day`, `intensity` (0-100), `forgiven` bool, `tags` (list)
3. Define `NpcRelationship` DTO: `npcId`, `personalTrust` (-100 to +100), `grudgeLevel` (0-100), `favorOwed` (0-100), `memoryEntries` (list), `lastInteractionDay`
4. Define `NpcMemoryState` DTO: map of npcId → NpcRelationship, list of global memory events
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define memory trigger rules:
   - Player helps NPC: +10 personal trust, +5 favor owed
   - Player refuses NPC: -15 personal trust, +10 grudge
   - Player ignores NPC: -5 personal trust
   - Player betrays NPC: -30 personal trust, +30 grudge
   - Player kills NPC's ally: -50 personal trust, +50 grudge
   - Player saves NPC's life: +40 personal trust, +20 favor owed
7. Create memory decay: old memories lose intensity over time (1% per day)
8. Implement forgiveness mechanic: player can apologize/compensate to reduce grudge
9. Create `INpcMemorySink` interface for NPC systems to query memory
10. Add deterministic seeding: memory triggers use `ISeededRng`
11. Wire into `GameBootstrap`: `SetupNpcMemory`, `TickNpcMemory`, `SaveNpcMemory`
12. Create `NpcMemoryCatalogLoader` for memory-triggered dialogue/events
13. Implement memory logging: all memory events recorded for UI/journal
14. Add UI hook: NPC panel shows relationship history and memory

## Main Task 2 — Implementation / Memory Triggers / NPC Reactions

1. Implement memory-triggered dialogue:
   - NPC with high personal trust: warm greeting, offers help, shares secrets
   - NPC with high grudge: cold greeting, refuses help, makes threats
   - NPC with favor owed: offers special quest, gives discount, provides aid
   - NPC with betrayed memory: refuses all interaction, may become hostile
2. Implement memory-based quest gating:
   - Some quests only available if NPC trusts you (personal trust >= 50)
   - Some quests blocked if NPC holds grudge (grudge >= 30)
   - Some quests modified by memory (NPC references past actions)
   - Some quests unlock after forgiveness (NPC gives second chance)
3. Implement memory-based trade modifiers:
   - High trust NPCs offer better prices (10% discount)
   - High grudge NPCs charge more (20% premium) or refuse trade
   - Favor owed NPCs offer one-time special deal
   - Betrayed NPCs blacklist player from trade
4. Implement memory-based combat behavior:
   - High grudge NPCs may attack on sight (if hostile faction)
   - High trust NPCs may come to player's aid in combat
   - Betrayed NPCs join enemies against player
   - Forgiven NPCs remain neutral
5. Create memory events:
   - "The Grudge" — NPC confronts player about past betrayal
   - "The Favor" — NPC calls in a favor owed
   - "The Reconciliation" — player apologizes, NPC considers forgiveness
   - "The Memory" — NPC references past action in dialogue
   - "The Test" — NPC tests player's loyalty with a choice
6. Add memory quest hooks:
   - "Old Debts" — NPC from past returns, wants help or revenge
   - "The Apology" — convince NPC to forgive past betrayal
   - "The Test of Loyalty" — NPC demands proof of allegiance
   - "The Shared Enemy" — NPC and player unite against common threat
7. Implement memory inheritance:
   - NPC memories persist across campaign (if NPC survives)
   - Memories affect NPC's faction standing toward player
   - Memories can be shared between NPCs (gossip)
   - Memories influence new NPCs (reputation precedes you)
8. Create memory UI:
   - NPC detail panel shows relationship history
   - Memory log shows all interactions with each NPC
   - Relationship meter shows personal trust vs. faction trust
   - Grudge indicator shows active grudges
9. Add memory journal: automatic log of significant NPC interactions
10. Implement memory tutorial: first NPC interaction explains memory system
11. Add memory tooltips: hover over NPC shows relationship summary
12. Create 30 memory-triggered dialogue templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `HoldfastNpcCatalog`: NPCs query memory for dialogue/behavior
2. Connect to `FactionStanceEngine`: personal trust modifies faction trust
3. Integrate with `VerdictNpcSystem`: Verdict NPCs use memory
4. Connect to `DoorEncounterSystem`: door NPCs use memory
5. Wire into `HoldfastTradeSession`: trade modifiers from memory
6. Connect to `TacticalCombatSystem`: combat behavior from memory
7. Implement old-save compatibility: existing saves get empty memory state
8. Add deterministic seeding: memory triggers use `ISeededRng`
9. Create exploit prevention: memories are persistent, can't be reset by save/load
10. Add tests: memory triggers, dialogue gating, trade modifiers, save round-trip
11. Verify catalog integrity: all NPC IDs resolve
12. Test edge cases: no memories (default behavior), max grudge (hostile), max trust (ally)
13. Verify headless behavior: memory processes correctly without UI
14. Add data-integrity-selftest: memory templates validate against NPC catalog
15. Create `--npc-memory-selftest` verb for CI validation

## State / System Interaction Model

```text
Player interacts with NPC
├─ Memory system records action
│  ├─ Helped: +trust, +favor
│  ├─ Refused: -trust, +grudge
│  ├─ Ignored: -trust
│  ├─ Betrayed: --trust, ++grudge
│  └─ Saved life: ++trust, +favor
├─ Memory decays over time (1%/day)
├─ Player can forgive (reduce grudge)
├─ NPC queries memory for behavior
│  ├─ High trust: warm greeting, offers help
│  ├─ High grudge: cold greeting, refuses help
│  ├─ Favor owed: offers special quest/deal
│  └─ Betrayed: refuses interaction, may attack
├─ Memory affects systems
│  ├─ Dialogue: references past actions
│  ├─ Quests: gated by trust/grudge
│  ├─ Trade: prices modified by relationship
│  └─ Combat: NPCs aid or attack based on memory
└─ Memory persists across campaign
   ├─ NPC remembers player actions
   ├─ Memories affect faction standing
   └─ Memories shared between NPCs (gossip)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --npc-memory-selftest
```

## Risk

**MEDIUM** — NPC memory can frustrate players if grudges are too persistent or forgiveness too difficult. Risk of memory feeling punitive rather than meaningful. Mitigation: allow forgiveness mechanics, decay old memories, keep grudge thresholds moderate (max 50 for quest blocks), and provide clear feedback on relationship status.

## Definition of Done

- `NpcMemorySystem.cs` exists with full `CaptureState/RestoreState`
- Per-NPC memory tracking functional (trust, grudge, favor)
- Memory-triggered dialogue implemented
- Memory-based quest gating working
- Memory-based trade modifiers applied
- Memory-based combat behavior functional
- Memory decay and forgiveness mechanics
- Save/load round-trip tested
- Deterministic memory triggers verified
- Old saves load without error
- 30 memory-triggered dialogue templates in data authority
- UI panel shows NPC relationship history
- Cross-system integration (NPC catalog, factions, Verdict, door encounters, trade, combat)

## Follow-On Opportunities

- NPC memory legacy (memories carry to New Game+)
- NPC gossip network (memories spread between NPCs)
- NPC reputation system (your actions known before you arrive)
- NPC memory quests (resolve old grudges, call in favors)
- NPC memory epilogue (NPCs remember you in ending)
