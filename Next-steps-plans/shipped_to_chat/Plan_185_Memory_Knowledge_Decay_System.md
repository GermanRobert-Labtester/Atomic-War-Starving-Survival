# Plan 185 — Memory & Knowledge Decay System

## Goal

Create a memory and knowledge decay system where unused skills fade, knowledge is forgotten over time, and memories lose clarity — requiring practice, review, and reinforcement to maintain. Currently `SkillProgressionSystem.cs` has skill dormancy (skills go dormant after 14 unused days) and Plan 180 (certifications) protects certified skills from decay, but there is no broader memory/knowledge decay system — no forgetting of events, no fading of knowledge, no loss of unpracticed capabilities beyond dormancy. This plan adds cognitive realism to survivor capabilities.

## Why

**Repository evidence:** Grep for `MemoryDecay`, `SkillDecay`, `TraitDecay`, `Forgetting`, `Deterioration` in Core returns only `SkillProgressionSystem.cs:56` — `BunkerSkillDecayStopped` callback for halting decay. Skill dormancy exists (14 unused days → dormant) but no broader memory/knowledge decay. `PhantomMemoryEngine.cs` tracks memories but they never fade. No forgetting mechanic, no knowledge degradation, no memory clarity loss.

**What is missing:** No memory decay. No knowledge forgetting. No skill degradation beyond dormancy. No memory clarity loss. Once learned, everything stays perfect forever (or goes dormant with easy reactivation). No cognitive realism — survivors never forget anything.

**Why existing plans don't solve it:** Plan 180 (certifications) protects certified skills from decay but doesn't add decay for uncertified knowledge. Plan 147 (per-NPC memory) adds memory but not memory decay. Plan 154 (education) adds knowledge acquisition but not knowledge loss. No plan addresses memory/knowledge decay as a system.

**Player value:** Creates strategic depth (maintain skills through practice), adds realism (people forget things), generates emergent stories (forgotten knowledge, rediscovered skills), and makes skill maintenance meaningful.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — current skill system
- `Assets/Ashfall.Core/PhantomMemoryEngine.cs` — memory system
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationship memory
- `Assets/Ashfall.Core/Journal/` — journal system
- NEW: `Assets/Ashfall.Core/Cognition/MemoryDecaySystem.cs`
- NEW: `Assets/StreamingAssets/Data/decay_rates.json`

## Main Task 1 — Foundation / System Contract

1. Create `MemoryDecaySystem.cs` in `Assets/Ashfall.Core/Cognition/`
2. Define `MemoryType` DTO: `typeId`, `typeName` (skill/knowledge/event/relationship/procedural), `baseDecayRate` (per day), `reinforcementBonus` (per use/review), `decayFloor` (minimum level before complete loss), `category` (cognitive/physical/emotional)
3. Define `Memory` DTO: `memoryId`, `survivorId`, `memoryType`, `content` (what is remembered), `strength` (0-100), `lastReinforced` (day), `decayRate` (current rate), `clarity` (0-100, how clearly remembered)
4. Define `ForgettingEvent` DTO: `eventId`, `survivorId`, `memoryId`, `eventType` (skill_faded/knowledge_forgotten/memory_blurred/relationship_faded), `day`, `strengthLost`, `description`
5. Define `Reinforcement` DTO: `reinforcementId`, `survivorId`, `memoryId`, `reinforcementType` (practice/review/reminder/experience), `strengthRestored`, `day`
6. Define `MemoryDecayState` DTO: list of memories per survivor, list of forgetting events, list of reinforcements, decay settings (global rate modifier)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define memory types and decay rates:
   - **Skills**: decay slowly (physical skills decay slower than mental)
   - **Knowledge**: decay moderately (facts forgotten without review)
   - **Events**: decay faster (details blur over time)
   - **Relationships**: decay slowly (emotional memories persist)
   - **Procedural**: decay slowest (muscle memory persists)
9. Define decay mechanics:
   - Each day without reinforcement: strength decreases
   - Decay rate varies by memory type
   - Below 50% strength: memory becomes "vague"
   - Below 25% strength: memory becomes "fragmentary"
   - At 0% strength: memory is forgotten (lost)
   - Certified skills (Plan 180) decay at 0.1x rate
   - Dormant skills (SkillProgressionSystem) decay at 0.5x rate
10. Define reinforcement mechanics:
    - **Practice**: using skill restores strength
    - **Review**: studying knowledge restores strength
    - **Reminder**: encountering related event restores strength
    - **Experience**: living through similar event restores strength
    - Each reinforcement type has different effectiveness
11. Define memory clarity:
    - Clarity decreases with strength
    - High clarity: detailed, accurate memory
    - Medium clarity: general sense, missing details
    - Low clarity: vague impression, unreliable
    - Clarity affects skill check bonuses
12. Define forgetting consequences:
    - Forgotten skills: cannot use until relearned
    - Forgotten knowledge: cannot reference
    - Blurred events: unreliable journal entries
    - Faded relationships: reduced relationship bonus
    - Forgetting is gradual, not sudden
13. Define memory preservation:
    - Writing in journal preserves event memories
    - Teaching knowledge reinforces it
    - Regular practice maintains skills
    - Strong relationships resist decay
    - Certified skills nearly immune (Plan 180)
14. Add deterministic seeding: decay uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupMemoryDecay`, `TickMemoryDecay`, `SaveMemoryDecay`

## Main Task 2 — Implementation / Decay / Reinforcement / Forgetting / Preservation

1. Implement daily decay:
   - Each day, check all memories
   - For unreinforced memories: apply decay
   - Decay rate based on memory type
   - Strength and clarity decrease
   - Decay logged
2. Implement reinforcement tracking:
   - Track last reinforcement day per memory
   - Practice: detect skill use
   - Review: detect knowledge study
   - Reminder: detect related events
   - Experience: detect similar situations
3. Implement reinforcement effects:
   - Reinforcement restores strength
   - Amount depends on reinforcement type
   - Practice most effective for skills
   - Review most effective for knowledge
   - Reminder most effective for events
4. Implement forgetting events:
   - Strength reaches 50%: "fading" warning
   - Strength reaches 25%: "fragmentary" warning
   - Strength reaches 0%: "forgotten" event
   - Forgetting events logged
   - Forgotten memories removed
5. Implement memory clarity:
   - Clarity tracks with strength
   - High clarity: full bonuses
   - Medium clarity: reduced bonuses
   - Low clarity: minimal bonuses
   - Clarity displayed in UI
6. Implement memory preservation:
   - Journal writing preserves events
   - Teaching reinforces knowledge
   - Practice maintains skills
   - Relationships maintained through interaction
   - Certification nearly prevents decay
7. Implement relearning:
   - Forgotten skills can be relearned
   - Relearning faster than initial learning
   - Partial memories accelerate relearning
   - Relearning logged
8. Implement decay modifiers:
   - Age affects decay (elderly forget faster — Plan 176)
   - Stress accelerates decay
   - Good health slows decay
   - Nutrition affects memory
   - Sleep quality affects consolidation
9. Create decay events:
   - "The Fade" — memory beginning to fade
   - "The Blur" — memory becoming vague
   - "The Forget" — memory lost
   - "The Reinforcement" — memory strengthened
   - "The Review" — knowledge reviewed
   - "The Practice" — skill practiced
   - "The Rediscovery" — forgotten skill relearned
10. Add decay quest hooks:
    - "The Scholar" — maintain knowledge through review
    - "The Practitioner" — maintain skills through practice
    - "The Journal" — preserve events in journal
    - "The Teacher" — reinforce knowledge by teaching
    - "The Rediscovery" — relearn forgotten skill
    - "The Memory" — maintain 20 active memories
    - "The Forgetful" — deal with important memory lost
11. Implement decay UI:
    - Memory panel: all memories with strength/clarity
    - Decay warning: memories at risk highlighted
    - Reinforcement log: recent reinforcements
    - Forgetting history: log of forgotten memories
    - Preservation tips: how to maintain memories
12. Add decay journal: automatic log of decay events
13. Implement decay tutorial: first fade warning explains system
14. Add decay tooltips: hover over memory shows strength/clarity
15. Create decay rate definitions per memory type

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SkillProgressionSystem`: skill decay integrated
2. Connect to `PhantomMemoryEngine`: memory decay applied
3. Integrate with `JournalSystem`: journal preserves memories
4. Connect to `SurvivorRelationsSystem`: relationship decay
5. Wire into `AgingSystem` (Plan 176): age affects decay
6. Connect to `SkillCertificationSystem` (Plan 180): certification slows decay
7. Implement old-save compatibility: existing saves get default memory state
8. Add deterministic seeding: decay uses `ISeededRng`
9. Create exploit prevention: decay is time-based, can't be gamed
10. Add tests: decay rates, reinforcement, forgetting, relearning, save round-trip
11. Verify all memory types decay correctly
12. Test edge cases: no decay (constant reinforcement), rapid decay (total neglect)
13. Verify headless behavior: decay processes correctly without UI
14. Add data-integrity-selftest: decay rates validate against memory catalogs
15. Create `--memory-decay-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --memory-decay-selftest
```

## Risk

**LOW** — Memory decay is straightforward with clear inputs (time, reinforcement) and outputs (strength/clarity changes). Risk of decay feeling punishing rather than realistic. Mitigation: make reinforcement easy (practice, review), show clear warnings, allow relearning, and ensure decay is gradual not sudden.

## Definition of Done

- `MemoryDecaySystem.cs` exists with full `CaptureState/RestoreState`
- 5 memory types (skill, knowledge, event, relationship, procedural)
- Daily decay applied to unreinforced memories
- Reinforcement tracking (practice, review, reminder, experience)
- Forgetting events (fade, blur, forget)
- Memory clarity system
- Memory preservation (journal, teaching, practice)
- Relearning mechanic
- Decay modifiers (age, stress, health, nutrition, sleep)
- Decay events and quest hooks
- Save/load round-trip tested
- Deterministic decay verified
- Old saves load without error
- Decay rates in data authority
- UI memory panel with decay warnings
- Cross-system integration (skills, phantom memory, journal, relations, aging, certification)

## Follow-On Opportunities

- Memory specialization (photographic memory trait)
- Memory legacy (important memories preserved permanently)
- Memory quests (specific memory maintenance goals)
- Memory events (memory flashes, deja vu)
- Memory trading (sharing memories between survivors)
