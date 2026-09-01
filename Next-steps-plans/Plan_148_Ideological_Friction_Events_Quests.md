# Plan 148 — Ideological Friction → Events & Quests

## Goal

Transform `IdeologicalFrictionSystem` from a passive numerical modifier (sleep penalties, affinity deltas) into an active narrative generator that produces ideological confrontation events, belief conversion attempts, bunker faction splits, and friction-triggered quest chains. Currently friction only affects sleep quality — this plan makes it a source of emergent storytelling and meaningful player decisions.

## Why

**Repository evidence:** `IdeologicalFrictionSystem.cs` (158 lines) defines 11 conflict pairs across two belief profile sets. Roommates on conflicting shifts get -2 affinity/day and 20% sleep penalty; matching beliefs get +1 affinity and 10% sleep bonus. The survivor social agent confirmed: "Friction is a passive numerical modifier, not a narrative generator. No belief-driven events, no conversion mechanics, no ideological faction splits, no quest triggers from extreme incompatibility." The `OnFrictionDetected` and `OnAffinityChanged` events are declared but have no subscribers — downstream consumers were never wired.

**What is missing:** Two survivors with diametrically opposed beliefs (e.g., `religious_faith` vs. `atheist_rationalist`) share a bunk and the only consequence is slightly worse sleep. There's no argument, no conversion attempt, no bunker-wide ideological split, no quest where the player must mediate or choose sides. The rich belief profile data exists but produces no gameplay beyond numbers.

**Why existing plans don't solve it:** Plan 12 (social/shelter life) mentions friction events but doesn't detail them. Plan 144 (survivor autonomy) adds autonomous behavior but not ideology-specific events. Plan 30 (ritual/faith/meaning) adds belief content but not friction mechanics. No plan connects ideological friction to events, quests, or belief conversion.

**Player value:** Makes belief choices meaningful (who you house together matters), creates emergent stories (a religious survivor tries to convert an atheist), adds depth to shelter management (ideological factions form), and generates moral dilemmas (do you suppress one belief to keep peace?).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/IdeologicalFrictionSystem.cs` — friction system
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationship data
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — shift assignment
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — player morality
- `Assets/StreamingAssets/Data/survivors.json` — survivor belief profiles
- NEW: `Assets/Ashfall.Core/Survivors/IdeologicalFrictionEvents.cs`
- NEW: `Assets/StreamingAssets/Data/ideological_events.json`

## Main Task 1 — Foundation / System Contract

1. Create `IdeologicalFrictionEvents.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `IdeologicalEvent` DTO: `eventId`, `eventType` (confrontation/conversion/split/quest), `participantIds` (list), `triggerCondition` (affinity threshold, days of friction, belief pair), `outcome` (list of possible outcomes with probabilities)
3. Define `IdeologicalEventOutcome` DTO: `outcomeId`, `description`, `affinityDelta`, `moraleDelta`, `beliefShift` (optional), `flagSet` (optional), `questUnlocked` (optional)
4. Define `IdeologicalFrictionEventState` DTO: list of fired events, list of active ideological factions, cooldown map
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define event trigger rules:
   - **Confrontation**: affinity < -50 between conflicting beliefs → argument event
   - **Conversion attempt**: affinity > 30 between conflicting beliefs → one tries to convert other
   - **Ideological split**: 3+ survivors with same belief vs. 3+ with opposing belief → bunker split event
   - **Quest trigger**: extreme friction (affinity < -80) → mediation quest unlocks
7. Create event probability: each event type has base probability modified by friction severity, belief strength, survivor personality traits
8. Implement event cooldown: same pair can't have event more than once per 14 days
9. Add deterministic seeding: event triggers use `ISeededRng`
10. Wire into `GameBootstrap`: `SetupFrictionEvents`, `TickFrictionEvents`, `SaveFrictionEvents`
11. Create `IdeologicalEventCatalogLoader` for event definitions
12. Implement event logging: all friction events recorded for UI/journal
13. Add UI hook: shelter panel shows active ideological tensions
14. Create friction journal: automatic log of ideological events

## Main Task 2 — Implementation / Events / Conversion / Splits / Quests

1. Implement confrontation events:
   - Two survivors with conflicting beliefs argue
   - Player mediates: choose side or stay neutral
   - Outcomes:
     - Side with A: A's affinity +20, B's affinity -30, morale -5
     - Side with B: B's affinity +20, A's affinity -30, morale -5
     - Stay neutral: both affinity -10, morale -10
     - Successful mediation (skill check): both affinity +10, morale +5
2. Implement conversion attempts:
   - Survivor with strong belief tries to convert roommate
   - Target resists or considers based on affinity, belief strength, personality
   - Outcomes:
     - Conversion succeeds: target's belief changes, original's affinity +30
     - Conversion fails: target's affinity -20, original's morale -5
     - Counter-conversion: target tries to convert original (reverse outcomes)
     - Agreement to disagree: both affinity +5, no belief change
3. Implement ideological split events:
   - Bunker divides into ideological factions
   - Player must manage the split:
     - Suppress one faction: morale penalty, faction resentment
     - Separate factions physically: housing shuffle, efficiency penalty
     - Mediate compromise: skill check, possible success
     - Let it play out: shelter efficiency drops, possible violence
   - Split creates lasting bunker state (factions persist until resolved)
4. Implement friction quest chains:
   - "The Debate" — mediate public debate between belief factions
   - "The Conversion" — help one survivor convert another (or prevent it)
   - "The Schism" — manage bunker split, prevent violence
   - "The Heretic" — survivor questions own belief, identity crisis quest
   - "The Crusade" — zealous survivor tries to purge opposing beliefs
5. Create belief-specific events:
   - `religious_faith` vs `atheist_rationalist`: theology debate, miracle claim
   - `military_discipline` vs `pacifist`: use of force argument
   - `collectivist_solidarity` vs `pragmatic_individualism`: resource sharing dispute
   - `superstitious_traditional` vs `atheist_rationalist`: ritual vs. reason
   - Each pair has 3-5 unique events with distinct dialogue
6. Implement belief shift mechanics:
   - Survivors can gradually shift beliefs through exposure
   - Belief shift requires sustained positive affinity with opposing believer
   - Shift is permanent and affects future friction calculations
   - Shifted survivors may face rejection from original belief group
7. Create ideological faction system:
   - 3+ survivors with same belief form informal faction
   - Faction has leader (highest skill/charisma)
   - Faction makes demands (housing, duty schedule, resources)
   - Player can recognize or suppress factions
   - Recognized factions provide morale bonus to members
   - Suppressed factions cause resentment and possible rebellion
8. Add friction quest hooks:
   - "The Preacher" — religious survivor wants to hold services
   - "The Scientist" — atheist wants to ban "superstitious" activities
   - "The Peacemaker" — neutral survivor asks player to mediate
   - "The Extremist" — zealous survivor plans action against opponents
   - "The Seeker" — uncertain survivor asks player for guidance
9. Add UI: ideological tension panel showing belief distribution and active frictions
10. Create friction journal: automatic log of ideological events and belief shifts
11. Implement friction tutorial: first friction event explains system
12. Add friction tooltips: hover over survivor shows belief and friction status
13. Create 25 ideological event templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `IdeologicalFrictionSystem`: friction ticks trigger event checks
2. Connect to `SurvivorRelationsSystem`: events modify affinity/relationships
3. Integrate with `MoralChoiceSystem`: mediation choices affect moral band
4. Connect to `DutyRosterSystem`: ideological factions affect shift assignment
5. Wire into `MentalHealthCrisisSystem`: extreme friction increases crisis risk
6. Connect to `ShelterThermalSystem`: split factions may refuse to share resources
7. Implement old-save compatibility: existing saves get empty event state
8. Add deterministic seeding: event triggers use `ISeededRng`
9. Create exploit prevention: event cooldowns prevent farming
10. Add tests: event triggering, conversion mechanics, split resolution, save round-trip
11. Verify catalog integrity: all survivor/belief IDs resolve
12. Test edge cases: no friction (no events), extreme friction (constant events)
13. Verify headless behavior: events process correctly without UI
14. Add data-integrity-selftest: event templates validate against survivor/belief catalogs
15. Create `--friction-events-selftest` verb for CI validation

## State / System Interaction Model

```text
Ideological friction tick
├─ Check event triggers for each conflicting pair
│  ├─ Confrontation: affinity < -50 → argument event
│  │  ├─ Player mediates: choose side or stay neutral
│  │  └─ Outcome: affinity/morale changes
│  ├─ Conversion: affinity > 30 → conversion attempt
│  │  ├─ Target resists/considers
│  │  └─ Outcome: belief shift or rejection
│  ├─ Split: 3+ vs 3+ beliefs → bunker split event
│  │  ├─ Player manages: suppress/separate/mediate/allow
│  │  └─ Outcome: faction formed, efficiency changes
│  └─ Quest: extreme friction → mediation quest unlocks
│     ├─ Player completes quest
│     └─ Outcome: friction resolved or worsened
├─ Event cooldowns applied
├─ Belief shifts tracked
├─ Ideological factions updated
└─ Events logged for UI/journal
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --friction-events-selftest
```

## Risk

**MEDIUM** — Ideological events can feel repetitive if same events trigger repeatedly. Risk of belief conversion feeling forced or unrealistic. Mitigation: large event variety (25+ templates), conversion requires sustained conditions, belief shifts are rare and gradual, and player mediation provides meaningful choices.

## Definition of Done

- `IdeologicalFrictionEvents.cs` exists with full `CaptureState/RestoreState`
- 4 event types implemented (confrontation, conversion, split, quest)
- Belief-specific events for major conflict pairs
- Belief shift mechanics functional
- Ideological faction system working
- Friction quest chains implemented
- Save/load round-trip tested
- Deterministic event triggers verified
- Old saves load without error
- 25 ideological event templates in data authority
- UI panel shows ideological tensions
- Cross-system integration (friction, relations, moral choice, duty roster, mental health)

## Follow-On Opportunities

- Belief evolution system (beliefs develop over time, not just static)
- Ideological alliances (beliefs that cooperate against common opponent)
- Religious ceremonies (belief-specific shelter events)
- Heresy mechanics (belief deviation punished by faction)
- Belief legacy (beliefs passed to next generation)
