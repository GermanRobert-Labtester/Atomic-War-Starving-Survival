# Plan 212 — Time Capsule & Legacy Messages

## Goal

Create a time capsule and legacy message system where survivors can create messages, records, and packages to be discovered by future survivors or opened at specific future dates — creating a cross-generational communication layer within the shelter. Currently `MemorialSystem` (262 lines) handles burial remembrance, and Plan 140 covers cross-campaign meta-progression — but there is no in-campaign time capsule system, no legacy messages, no "message in a bottle" mechanics, no way for survivors to leave something for the future. This plan adds temporal depth to survivor expression.

## Why

**Repository evidence:** Grep for `TimeCapsule`, `LegacyMessage`, `MessageToFuture`, `CapsuleSystem`, `LegacyLetter`, `FutureMessage`, `HeritageMessage`, `ChronoMessage` in Core returns ZERO matches. No time capsule or legacy message system exists.

**What is missing:** No time capsules. No legacy messages. No "message in a bottle" mechanics. No way for survivors to leave something for future discovery. No scheduled message opening. No cross-generational communication within a campaign.

**Why existing plans don't solve it:** Plan 140 (Generational Legacy) covers cross-campaign meta-progression (between playthroughs). Plan 206 (Death & Inheritance) covers what happens to possessions on death. Plan 162 (Shelter Archive) records history. No plan addresses time capsules or scheduled legacy messages within a campaign.

**Player value:** Creates emotional depth (survivors leave messages for the future), adds temporal gameplay (open capsules on specific dates), generates emergent stories (discovering old messages), and makes the shelter feel like a place with history and continuity.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/MemorialSystem.cs` — remembrance (complementary)
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` — death tracking
- `Assets/Ashfall.Core/Clock/ISimClock.cs` — time tracking
- NEW: `Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs`
- NEW: `Assets/StreamingAssets/Data/time_capsule_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `TimeCapsuleSystem.cs` in `Assets/Ashfall.Core/Communication/`
2. Define `TimeCapsule` DTO: `capsuleId`, `capsuleName`, `creatorId` (survivor_id), `createdDay`, `openCondition` (date-based/survivor-based/event-based/manual), `openDate` (day, -1 if not date-based), `openSurvivor` (survivor_id, null if not survivor-based), `openEvent` (event_id, null if not event-based), `location` (room_id where hidden), `contents` (list of `CapsuleContent`), `message` (text message to future), `isOpen` bool, `openedDay` (-1 if unopened), `openedBy` (survivor_id or null)
3. Define `CapsuleContent` DTO: `contentId`, `contentType` (item/letter/photo/recording/drawing/artifact), `itemId` (item_id if physical item), `text` (text content), `authorId` (survivor_id), `sentimentalValue` (0-100)
4. Define `LegacyMessage` DTO: `messageId`, `authorId` (survivor_id), `recipientType` (specific_survivor/next_generation/anyone/shelter_leader), `recipientId` (survivor_id or null), `content`, `createdDay`, `deliveryCondition` (on_death/on_date/on_event/immediate), `deliveryDate` (day, -1 if not date-based), `isDelivered` bool, `deliveredDay` (-1 if undelivered), `isRead` bool
5. Define `CapsuleDiscovery` DTO: `discoveryId`, `capsuleId`, `discoveredBy` (survivor_id), `discoveredDay`, `discoveryType` (found_by_accident/searching/inherited/designated_recipient), `reaction` (list of emotional responses)
6. Define `TimeCapsuleState` DTO: list of time capsules, list of legacy messages, list of discoveries, capsule settings (max capsules, auto-create on death bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define capsule creation:
   - Survivor creates capsule with contents and message
   - Capsule hidden in shelter location
   - Open condition set (date, survivor, event, manual)
   - Capsule logged
9. Define capsule discovery:
   - Capsules discovered when condition met
   - Discovery by designated recipient, accident, or search
   - Discovery triggers emotional response
   - Discovery logged
10. Define legacy messages:
    - Survivor writes message for future
    - Message has recipient type and delivery condition
    - Message delivered when condition met
    - Message read by recipient
    - Message logged
11. Define open conditions:
    - **Date-based**: open on specific day
    - **Survivor-based**: open when specific survivor finds it
    - **Event-based**: open when specific event occurs
    - **Manual**: open when player chooses
12. Define emotional responses:
    - Discovery triggers morale effects
    - Messages from deceased: grief + appreciation
    - Messages from living: connection + warmth
    - Historical messages: curiosity + wonder
13. Add deterministic seeding: capsule events use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupTimeCapsules`, `TickTimeCapsules`, `SaveTimeCapsules`

## Main Task 2 — Implementation / Capsules / Messages / Discovery / UI

1. Implement capsule creation:
   - Survivor creates capsule
   - Contents added (items, letters, photos)
   - Message written
   - Location chosen
   - Open condition set
   - Capsule logged
2. Implement capsule discovery:
   - Condition checked daily
   - Capsule discovered when condition met
   - Discovery reaction generated
   - Discovery logged
3. Implement legacy messages:
   - Message written by survivor
   - Recipient and delivery condition set
   - Message delivered when condition met
   - Message read by recipient
   - Message logged
4. Implement capsule UI:
   - Capsule panel: create, view, open capsules
   - Capsule detail: contents, message, open condition
   - Legacy message panel: write, view, deliver messages
   - Discovery log: history of discoveries
   - Notification when capsule discovered
5. Create capsule events:
    - "The Capsule" — time capsule created
    - "The Discovery" — capsule discovered
    - "The Message" — legacy message delivered
    - "The Opening" — capsule opened
    - "The Past" — historical message read
    - "The Future" — message to future written
    - "The Inheritance" — capsule inherited
    - "The Memory" — emotional response to discovery
6. Add capsule quest hooks:
    - "The Time Capsule" — create 5 time capsules
    - "The Archaeologist" — discover 10 capsules
    - "The Messenger" — write 20 legacy messages
    - "The Historian" — read 15 historical messages
    - "The Keeper" — maintain 5 unopened capsules for 100 days
    - "The Legacy" — have 3 capsules discovered after your death
    - "The Connection" — receive 5 messages from deceased survivors
7. Implement capsule tutorial: first capsule creation explains system
8. Add capsule tooltips: hover over capsule shows details
9. Create capsule templates in data file (10+ templates)
10. Implement capsule persistence: capsules/messages saved
11. Integrate with `MemorialSystem`: capsules can be memorial items

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MemorialSystem`: capsules integrate with remembrance
2. Connect to `SurvivorFateSystem`: death can trigger capsule delivery
3. Integrate with `ISimClock`: date-based opening conditions
4. Connect to `EventSystem`: event-based opening conditions
5. Wire into `PersonalBelongingsSystem` (Plan 210): capsule items are belongings
6. Connect to `DeathLegacySystem` (Plan 206): inheritance can include capsules
7. Implement old-save compatibility: existing saves get no capsules
8. Add deterministic seeding: capsule events use `ISeededRng`
9. Create exploit prevention: capsules are finite, can't be gamed
10. Add tests: capsule creation, discovery, messages, conditions, save round-trip
11. Verify all capsule types work correctly
12. Test edge cases: no capsules (current behavior), many capsules (discovery flood)
13. Verify headless behavior: capsules process correctly without UI
14. Add data-integrity-selftest: capsules validate against survivor/item catalogs
15. Create `--time-capsule-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --time-capsule-selftest
```

## Risk

**LOW** — Time capsules are straightforward with clear inputs (creation) and outputs (discovery, emotional response). Risk of capsules feeling like novelty. Mitigation: make discoveries meaningful (emotional responses, morale effects), show clear value, and ensure capsules feel like genuine cross-generational communication.

## Definition of Done

- `TimeCapsuleSystem.cs` exists with full `CaptureState/RestoreState`
- Time capsule creation (contents, message, location, open condition)
- 4 open conditions (date, survivor, event, manual)
- Capsule discovery (accident, search, inheritance, designated)
- Legacy messages (recipient, delivery condition, read tracking)
- Emotional responses to discovery
- Capsule events and quest hooks
- Save/load round-trip tested
- Deterministic capsule events verified
- Old saves load with no capsules
- Capsule templates in data authority (10+ templates)
- UI capsule panel, capsule detail, legacy message panel, discovery log, notifications
- Cross-system integration (memorial, fate, clock, events, belongings, death legacy)

## Follow-On Opportunities

- Capsule specialization (survivors become expert archivists)
- Capsule legacy (famous capsules remembered across campaigns)
- Capsule quests (specific capsule goals)
- Capsule events (mass capsule discovery, historical archive found)
- Capsule trading (trade capsules with other settlements)
