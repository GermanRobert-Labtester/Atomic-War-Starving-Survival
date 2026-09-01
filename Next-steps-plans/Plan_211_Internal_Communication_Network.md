# Plan 211 — Internal Communication Network

## Goal

Create an internal communication network system where the shelter has bulletin boards, intercom announcements, internal mail, and notice systems that allow information to flow between survivors and from leadership to the shelter population. Currently information flows through narrative events and direct UI — there is no internal communication infrastructure, no bulletin boards, no intercom system, no internal mail, no way for leadership to broadcast announcements, no way for survivors to leave messages for each other. The shelter is informationally silent. This plan adds internal communication as a shelter management and social layer.

## Why

**Repository evidence:** Grep for `CommunicationNetwork`, `BulletinBoard`, `InternalMail`, `ShelterAnnouncement`, `MessageBoard`, `Intercom`, `ShelterRadio`, `NoticeBoard` in Core returns only 1 match: `ShelterEncounterSystem.cs:61` has `KindIntercomOffice = "intercom_office"` (a string constant, not a system). Plan 157 (Communications Radio Network) covers external radio infrastructure, not internal shelter communication. No internal communication system exists.

**What is missing:** No bulletin boards. No intercom announcements. No internal mail. No notice boards. No leadership broadcast system. No survivor-to-survivor messaging. No way to post notices, requests, or announcements. Information flows only through direct UI and narrative events.

**Why existing plans don't solve it:** Plan 157 (radio network) covers external communication with other settlements. Plan 131 (information network) covers intelligence/rumor gathering. Plan 203 (intelligence) covers spy networks. No plan addresses internal shelter communication.

**Player value:** Creates social depth (survivors communicate), adds leadership tools (broadcast announcements), generates emergent stories (messages left, notes found), and makes the shelter feel like a community with information flow.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — duty assignments (communication about duties)
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships (message recipients)
- `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs` — schedule (announcement timing)
- NEW: `Assets/Ashfall.Core/Communication/InternalCommunicationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/communication_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `InternalCommunicationSystem.cs` in `Assets/Ashfall.Core/Communication/`
2. Define `CommunicationNetwork` DTO: `networkId`, `bulletinBoards` (list of board locations), `intercomSystem` (intercom coverage), `mailSlots` (list of mail delivery points), `announcementSystem` (leadership broadcast capability), `networkCoverage` (0-100, how much of shelter has communication access), `lastCommunication` (day), `settings` (auto-deliver bool, priority system bool)
3. Define `BulletinBoard` DTO: `boardId`, `boardName`, `location` (room_id), `messages` (list of posted messages), `capacity` (max messages), `accessLevel` (who can post: all/leadership/staff), `lastUpdatedDay`
4. Define `CommunicationMessage` DTO: `messageId`, `messageType` (announcement/notice/request/personal_mail/warning/memorial/celebration), `authorId` (survivor_id or "leadership"), `recipientId` (survivor_id or null for public), `subject`, `content`, `priority` (low/normal/high/urgent), `postedDay`, `expiresDay` (-1 if permanent), `isRead` bool, `responses` (list of response messages)
5. Define `IntercomAnnouncement` DTO: `announcementId`, `authorId` (survivor_id), `content`, `broadcastDay`, `broadcastTime` (hour), `coverageArea` (list of room_ids), `priority` (routine/important/emergency), `acknowledged` (list of survivor_ids who acknowledged)
6. Define `MailDelivery` DTO: `deliveryId`, `messageId`, `senderId`, `recipientId`, `deliveryDay`, `deliveryLocation` (room_id), `status` (pending/delivered/read/returned), `deliveryMethod` (hand_delivery/mail_slot/intercom)
7. Define `InternalCommunicationState` DTO: list of bulletin boards, list of messages, list of intercom announcements, list of mail deliveries, network coverage, communication settings
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define communication types (7+ types):
   - **Announcement**: leadership broadcast to all shelter (intercom + bulletin boards)
   - **Notice**: informational post on bulletin board (events, schedules, reminders)
   - **Request**: survivor asks for something (help, items, information)
   - **Personal Mail**: private message between survivors
   - **Warning**: urgent safety/security notice
   - **Memorial**: death remembrance notice
   - **Celebration**: event announcement (birthday, achievement, holiday)
10. Define bulletin board mechanics:
    - Boards placed in shelter rooms (mess hall, workshop, medical, etc.)
    - Anyone can post on public boards; leadership-only boards restricted
    - Boards have capacity (max messages)
    - Messages expire after set duration
    - Boards logged
11. Define intercom mechanics:
    - Intercom system covers shelter rooms
    - Leadership can broadcast announcements
    - Announcements have priority (routine/important/emergency)
    - Emergency announcements alert all survivors
    - Intercom logged
12. Define internal mail mechanics:
    - Survivors can send messages to each other
    - Mail delivered to recipient's location
    - Mail can be read, responded to, or ignored
    - Unread mail accumulates
    - Mail logged
13. Define message priority:
    - **Low**: informational, no urgency
    - **Normal**: standard priority
    - **High**: important, time-sensitive
    - **Urgent**: immediate attention required
    - Priority affects notification and response time
14. Add deterministic seeding: communication events use `ISeededRng`
15. Wire into `GameBootstrap`: `SetupInternalCommunication`, `TickInternalCommunication`, `SaveInternalCommunication`

## Main Task 2 — Implementation / Boards / Intercom / Mail / Announcements / UI

1. Implement bulletin boards:
   - Boards placed in rooms
   - Messages posted by survivors/leadership
   - Messages expire over time
   - Boards logged
2. Implement intercom system:
   - Leadership broadcasts announcements
   - Announcements have priority and coverage
   - Survivors acknowledge receipt
   - Intercom logged
3. Implement internal mail:
   - Survivors send messages to each other
   - Mail delivered to location
   - Mail read/responded/ignored
   - Mail logged
4. Implement announcements:
   - Leadership broadcasts to shelter
   - Announcements posted on boards
   - Emergency announcements alert all
   - Announcements logged
5. Implement message priority:
   - Priority affects notification
   - Urgent messages trigger alerts
   - Priority logged
6. Implement communication UI:
   - Communication panel: boards, mail, announcements
   - Board detail: messages, post new message
   - Mail panel: inbox, sent, compose
   - Announcement panel: compose broadcast
   - Message detail: read, respond, delete
   - Notification alerts for new messages
7. Create communication events:
    - "The Notice" — new notice posted
    - "The Announcement" — leadership broadcast
    - "The Letter" — personal mail received
    - "The Request" — help requested
    - "The Warning" — urgent warning issued
    - "The Memorial" — death notice posted
    - "The Celebration" — event announced
    - "The Response" — message answered
8. Add communication quest hooks:
    - "The Communicator" — send 50 messages
    - "The Announcer" — make 20 broadcasts
    - "The Correspondent" — receive 30 letters
    - "The Helper" — respond to 20 requests
    - "The Board Manager" — maintain 5 bulletin boards
    - "The Network" — achieve 100% communication coverage
    - "The Historian" — archive 100 messages
9. Implement communication tutorial: first message explains system
10. Add communication tooltips: hover over message shows details
11. Create communication templates in data file (20+ message templates)
12. Implement communication persistence: messages/boards saved
13. Integrate with `LeadershipSystem`: leadership has broadcast priority

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `LeadershipSystem`: leadership broadcasts
2. Connect to `SurvivorRelationsSystem`: personal mail between friends
3. Integrate with `ShelterScheduleSystem`: schedule announcements
4. Connect to `MemorialSystem`: death notices
5. Wire into `InterpersonalConflictSystem` (Plan 202): conflict-related messages
6. Connect to `ShelterSecuritySystem` (Plan 209): security warnings
7. Implement old-save compatibility: existing saves get basic bulletin board, no messages
8. Add deterministic seeding: communication events use `ISeededRng`
9. Create exploit prevention: messages are finite, can't be gamed
10. Add tests: boards, intercom, mail, announcements, priority, save round-trip
11. Verify all communication types work correctly
12. Test edge cases: no communication (current behavior), heavy communication (message flood)
13. Verify headless behavior: communication processes correctly without UI
14. Add data-integrity-selftest: communication validates against room/survivor catalogs
15. Create `--internal-communication-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --internal-communication-selftest
```

## Risk

**LOW** — Communication is straightforward with clear inputs (messages) and outputs (delivery, responses). Risk of communication feeling like noise. Mitigation: make messages meaningful (requests get responses, announcements matter), show clear value, and ensure communication feels like community building not clutter.

## Definition of Done

- `InternalCommunicationSystem.cs` exists with full `CaptureState/RestoreState`
- 7+ communication types (announcement, notice, request, mail, warning, memorial, celebration)
- Bulletin boards (per-room, capacity, access levels)
- Intercom system (broadcast, priority, coverage)
- Internal mail (send, deliver, read, respond)
- Message priority (low/normal/high/urgent)
- Communication events and quest hooks
- Save/load round-trip tested
- Deterministic communication events verified
- Old saves load with basic bulletin board, no messages
- Communication templates in data authority (20+ templates)
- UI communication panel, board detail, mail panel, announcement panel, message detail, notifications
- Cross-system integration (leadership, relations, schedule, memorial, conflicts, security)

## Follow-On Opportunities

- Communication specialization (survivors become expert communicators/secretaries)
- Communication legacy (famous messages remembered)
- Communication quests (specific communication goals)
- Communication events (mass communication, communication blackout)
- Communication trading (trade communication technology with other settlements)
