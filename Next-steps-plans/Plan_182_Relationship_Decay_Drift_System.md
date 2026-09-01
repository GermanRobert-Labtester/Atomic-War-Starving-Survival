# Plan 182 — Relationship Decay & Drift System

## Goal

Create a relationship decay and drift system where survivor bonds weaken over time without interaction, friends drift apart, and relationships require maintenance. Currently `SurvivorRelationsSystem.cs` (191 lines) tracks affinity, trust, resentment, grief, and bond types, but relationships only change through explicit events — they never decay from neglect. Two survivors who became close friends on Day 10 remain close friends on Day 365 even if they never interact again. This plan adds temporal realism to social bonds.

## Why

**Repository evidence:** Grep for `RelationshipDecay`, `FriendshipDecay`, `TrustDecay`, `MemoryFade`, `bond_decay` in Core returns ZERO matches. `SurvivorRelationsSystem.cs` (191 lines) has rich data model (affinity, trust, resentment, grief, bondType) but no decay mechanic. Relationships change only through explicit events (shared experiences, conflicts, gifts). Once formed, bonds are permanent unless broken by a negative event. No drift, no fading, no "grew apart" mechanic.

**What is missing:** No relationship decay. No friendship drift. No trust erosion from absence. No "grew apart" events. Relationships are permanent once formed. Survivors who never interact still maintain their bond forever. No social maintenance required.

**Why existing plans don't solve it:** Plan 147 (per-NPC memory) adds memory but not relationship decay. Plan 150 (romance/family) adds relationship formation but not decay. Plan 179 (psychology) adds psychological profiles but not social decay. No plan addresses relationship maintenance or drift.

**Player value:** Creates social urgency (relationships need attention), adds strategic depth (manage social bonds), generates emergent stories (friends drifting apart, reconciliations), and makes relationships feel real (they require effort).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationship tracking
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — morale tracking
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — work assignments
- `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` — room assignments
- NEW: `Assets/Ashfall.Core/Survivors/RelationshipDecaySystem.cs`

## Main Task 1 — Foundation / System Contract

1. Create `RelationshipDecaySystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `DecayRate` DTO: `bondType` (friend/collover/rival/family/mentor), `baseDecayPerDay` (affinity lost per day without interaction), `interactionBonus` (affinity restored per interaction), `neglectThreshold` (affinity level where decay accelerates)
3. Define `DriftEvent` DTO: `eventId`, `survivorA`, `survivorB`, `driftType` (grew_apart/trust_eroded/friendship_faded/resentment_built), `affinityLost`, `day`, `cause` (no_interaction/conflicting_duties/room_separation/ideological_friction)
4. Define `Interaction` DTO: `interactionId`, `survivorA`, `survivorB`, `interactionType` (shared_work/roommate/conversation/gift/conflict/shared_event), `affinityChange`, `day`
5. Define `RelationshipDecayState` DTO: list of decay rates per bond type, list of drift events, list of interactions, last interaction day per pair, neglected relationships list
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define decay mechanics:
   - Each day without interaction: affinity decreases by decay rate
   - Decay rate varies by bond type (friends decay faster than family)
   - Below neglect threshold: decay accelerates
   - At 0 affinity: bond breaks ("grew apart" event)
   - Trust decays slower than affinity (trust is harder to lose)
   - Resentment increases slowly from neglect
8. Define interaction types that restore affinity:
   - **Shared work**: assigned to same task, +affinity
   - **Roommates**: share room, +affinity per day
   - **Conversation**: social interaction event, +affinity
   - **Gift**: giving items, +affinity (scaled by gift value)
   - **Shared event**: experience same event together, +affinity
   - **Conflict**: negative interaction, -affinity but resolves tension
9. Define drift events:
   - **Grew Apart**: affinity slowly reached 0, bond dissolves
   - **Trust Eroded**: trust decayed from lack of interaction
   - **Friendship Faded**: close friendship became acquaintance
   - **Resentment Built**: neglect turned to active resentment
10. Define drift causes:
    - **No interaction**: most common, general drift
    - **Conflicting duties**: assigned to opposing tasks
    - **Room separation**: housed in different parts of shelter
    - **Ideological friction**: ideological differences (Plan 148)
    - **Value conflict**: moral disagreement
11. Define drift prevention:
    - Regular interaction prevents decay
    - Roommates have slower decay (proximity bonus)
    - Shared work slows decay (common purpose)
    - Strong bonds (high affinity) decay slower
    - Family bonds decay slowest
12. Add deterministic seeding: decay uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupRelationshipDecay`, `TickRelationshipDecay`, `SaveRelationshipDecay`
14. Implement relationship UI: relationship panel showing bond status, decay warnings
15. Create drift event journal: automatic log of drift events

## Main Task 2 — Implementation / Decay / Interactions / Drift / Reconciliation

1. Implement daily decay:
   - Each day, check all active relationships
   - For pairs with no interaction: apply decay
   - Decay rate based on bond type
   - Below threshold: accelerated decay
   - Decay logged
2. Implement interaction tracking:
   - Track last interaction day per pair
   - Shared work: detect from duty roster
   - Roommates: detect from room assignment
   - Conversation: triggered by social events
   - Gifts: tracked when given
   - Shared events: tracked from event system
3. Implement interaction effects:
   - Interaction resets decay timer
   - Interaction restores affinity
   - Amount restored depends on interaction type
   - Positive interactions build affinity
   - Negative interactions (conflict) reduce affinity but can resolve resentment
4. Implement drift events:
   - Affinity reaches threshold: "growing apart" warning
   - Affinity reaches 0: "grew apart" event, bond breaks
   - Trust decays: "trust eroded" event
   - Resentment builds: "resentment" event
   - Drift events logged in journal
5. Implement reconciliation:
   - Drifted survivors can reconcile
   - Reconciliation requires positive interaction
   - Reconciliation restores some affinity
   - Reconciliation event: "reconnected"
   - Some bonds cannot be fully restored
6. Implement drift prevention:
   - Player can manage work assignments to keep friends together
   - Room assignments keep survivors in proximity
   - Social events bring survivors together
   - Gifts maintain bonds
   - Prevention is strategic management
7. Implement drift consequences:
   - Drifted friends: reduced cooperation, morale penalty
   - Broken bonds: possible resentment, conflict
   - Eroded trust: reduced willingness to help
   - Drift affects shelter cohesion
8. Create drift events:
   - "The Drift" — relationship beginning to fade
   - "The Gap" — relationship broken (grew apart)
   - "The Reconnection" — survivors reconcile
   - "The Resentment" — neglect turns to resentment
   - "The Memory" — survivors reminisce (slows decay)
   - "The Effort" — player actively maintains relationship
   - "The Loss" — important relationship lost
9. Add drift quest hooks:
    - "The Friend" — maintain friendship for 100 days
    - "The Drift" — notice and prevent relationship decay
    - "The Reconciliation" — reconcile drifted survivors
    - "The Community" — keep all relationships above threshold
    - "The Effort" — actively maintain 5 relationships
    - "The Loss" — deal with important relationship ending
    - "The Bridge" — reconnect former friends
10. Implement relationship UI:
    - Relationship panel: all bonds with status
    - Decay warning: relationships at risk highlighted
    - Interaction log: recent interactions per pair
    - Drift history: log of drift events
    - Prevention tips: how to maintain bonds
11. Add drift journal: automatic log of drift events
12. Implement drift tutorial: first decay warning explains system
13. Add drift tooltips: hover over bond shows decay status
14. Create decay rate definitions per bond type

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SurvivorRelationsSystem`: decay applied to existing relationships
2. Connect to `DutyRosterSystem`: shared work detected
3. Integrate with `ShelterAssignmentSystem`: roommate proximity detected
4. Connect to `IdeologicalFrictionSystem` (Plan 148): friction accelerates decay
5. Wire into `NeedsSystem`: drift affects morale
6. Connect to `MentalHealthCrisisSystem` (Plan 179): drift affects psychology
7. Implement old-save compatibility: existing saves get default decay state
8. Add deterministic seeding: decay uses `ISeededRng`
9. Create exploit prevention: decay is time-based, can't be gamed
10. Add tests: decay rates, interaction effects, drift events, reconciliation, save round-trip
11. Verify all bond types decay correctly
12. Test edge cases: no decay (constant interaction), rapid decay (total neglect)
13. Verify headless behavior: decay processes correctly without UI
14. Add data-integrity-selftest: decay rates validate against relationship catalogs
15. Create `--relationship-decay-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --relationship-decay-selftest
```

## Risk

**LOW** — Relationship decay is straightforward with clear inputs (time, interactions) and outputs (affinity changes, drift events). Risk of decay feeling punishing rather than realistic. Mitigation: make prevention easy (roommates, shared work), show clear warnings, allow reconciliation, and ensure decay is gradual not sudden.

## Definition of Done

- `RelationshipDecaySystem.cs` exists with full `CaptureState/RestoreState`
- Decay rates defined per bond type
- Daily decay applied to neglected relationships
- Interaction tracking (shared work, roommates, conversation, gifts, events)
- Drift events (grew apart, trust eroded, friendship faded, resentment)
- Reconciliation mechanic
- Drift prevention through management
- Drift consequences (morale, cooperation)
- Drift events and quest hooks
- Save/load round-trip tested
- Deterministic decay verified
- Old saves load without error
- Decay rates in data authority
- UI relationship panel with decay warnings
- Cross-system integration (relations, duty roster, room assignment, friction, needs, psychology)

## Follow-On Opportunities

- Relationship milestones (friendship anniversaries)
- Relationship legacy (famous friendships remembered)
- Relationship quests (specific bond goals)
- Relationship events (reunion, falling out, reconciliation)
- Relationship trading (social introductions between survivors)
