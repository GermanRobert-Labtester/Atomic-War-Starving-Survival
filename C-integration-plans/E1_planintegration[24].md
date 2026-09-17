---
PLAN_ID: E1-24
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 24
STATUS: READY_FOR_EXECUTION_WHEN_MESSAGE_EVENT_TOPOLOGY_AND_NOTIFICATION_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 211 — Internal Communication Network"
SEQUENCE_FILENAME: "E1_planintegration[24].md"
PREVIOUS_FILENAME: "E1_planintegration[23].md"
NEXT_FILENAMES:
  - "E1_planintegration[25].md"
  - "E1_planintegration[26].md"
CATEGORY: LINK+SHELTER+COMMUNICATION+MESSAGING+LEADERSHIP+SOCIAL
PRIMARY_INTENT: "Create shelter-local communication artifacts, channel infrastructure, delivery/receipt state, publication rules, and message-oriented UI while preserving canonical event, leadership, duty, schedule, relations, memorial, security, external-information, accessibility, topology, power, and maintenance authorities."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_EVENT_BUS_FORBIDDEN: true
SECOND_INFORMATION_TRUTH_SYSTEM_FORBIDDEN: true
SECOND_LEADERSHIP_COMMAND_SYSTEM_FORBIDDEN: true
SECOND_DUTY_SCHEDULER_FORBIDDEN: true
SECOND_NOTIFICATION_ACCESSIBILITY_SYSTEM_FORBIDDEN: true
SECOND_ROOM_TOPOLOGY_FORBIDDEN: true
SECOND_POWER_STATE_FORBIDDEN: true
SECOND_MAINTENANCE_STATE_FORBIDDEN: true
GLOBAL_NETWORK_COVERAGE_AS_AUTHORITY_FORBIDDEN: true
RNG_FOR_MESSAGE_DELIVERY_OR_READ_RECEIPTS_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: HIGH
NOISE_RISK: VERY_HIGH
SOCIAL_SIMULATION_RISK: HIGH
UI_SCALE_RISK: VERY_HIGH
---

# E1 Plan Integration [24] — Internal Shelter Communication, Bulletin Boards, Intercom, Mail, Notices, Requests, Receipts, and Community Information Flow

> **Sequence rule:** this file is `E1_planintegration[24].md`.
> The next files are `E1_planintegration[25].md`, `E1_planintegration[26].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 211 into an implementation-grade internal communication programme.

The source plan identifies a legitimate experiential gap: ASHFALL has many systems that create duties,
warnings, memorials, conflicts, schedules, requests, celebrations, and information, but the shelter itself
does not appear to have a durable internal layer through which survivors and leadership can publish, deliver,
read, acknowledge, and respond to those communications.

That can make the bunker feel informationally silent even when the simulation is busy.

The correct solution is not to build another general-purpose event bus and force every system to send its
events through `InternalCommunicationSystem`.

Instead, the communication layer should own a narrower set of facts:

- message identity and immutable content payload/reference;
- author and intended audience;
- publication channel;
- posting/delivery state;
- per-recipient receipt/read/acknowledgement state where gameplay needs it;
- bulletin-board occupancy/archive state;
- intercom broadcast instance and channel coverage result;
- internal-mail delivery envelope;
- expiry/archive policy;
- channel access/permission policy;
- UI/read models.

Canonical systems still own the meaning of what is being communicated.

A duty notice does not assign a duty. `DutyRosterSystem` assigns duties.
A security warning does not create a raid. `ShelterSecuritySystem` owns the threat.
A memorial notice does not create a death or burial. E1-23/Memorial own those facts.
A leadership announcement does not magically change morale, obedience, or faction standing.
A radio rumor does not become true because someone posts it on a board.

The architectural standard is:

**canonical systems create facts and actions; internal communication publishes, routes, and records messages
about those facts.**

## 1. Source Intent Preserved

Plan 211 asks for:

- bulletin boards;
- intercom announcements;
- internal mail;
- notices;
- leadership broadcasts;
- survivor-to-survivor messages;
- requests;
- memorial notices;
- celebration messages;
- message priority;
- channel access restrictions;
- read/acknowledgement state;
- timing/expiration;
- UI;
- templates;
- events and quests;
- persistence;
- old-save compatibility;
- deterministic behavior;
- integrations with Leadership, Relations, Schedule, Memorial, Conflict, and Security.

E1-24 preserves those goals while correcting ownership, recipient-state modeling, infrastructure boundaries,
message-noise scaling, and safety/accessibility behavior.

## 2. Core Architecture Thesis

```text
Canonical source fact / explicit authored message
       |
       +--> Duty / Schedule
       +--> Leadership / Governance
       +--> Memorial / E1-23
       +--> Security / Emergency
       +--> Relations / Social action
       +--> E1-3 information claim
       +--> Quest / Event / Celebration
       |
       v
Communication message artifact
       |
       +--> message_id
       +--> semantic type
       +--> source provenance
       +--> author
       +--> audience
       +--> localized content/template refs
       +--> priority
       +--> expiry/archive policy
       |
       v
Channel publication/delivery
       |
       +--> BulletinBoard
       +--> Intercom
       +--> InternalMail
       +--> Notice/Message UI
       |
       v
Recipient receipt/read/ack state
       |
       v
Optional canonical response/action
       |
       +--> Duty request
       +--> social reply
       +--> leadership response
       +--> memorial action
       +--> security action
```

Communication is an information-delivery layer, not an outcome engine.

## 3. Architectural Corrections to the Source Plan

### 3.1 `isRead` cannot live on a message with multiple recipients

A public board post or intercom broadcast can be read/heard by many survivors. Read state must be per
recipient or represented by a bounded receipt/acknowledgement model.

### 3.2 `networkCoverage` should be derived

Coverage depends on:

- installed boards/intercom/mail points;
- canonical room topology;
- channel capability;
- PowerGrid availability;
- E1-17 component condition;
- survivor location/presence;
- possibly access restrictions.

A global 0–100 field may be a UI summary but should not be authoritative.

### 3.3 Internal communication is not a second event bus

Canonical events may generate communication artifacts through adapters. Systems must remain able to function
without a message artifact unless the design explicitly says information delivery is a gameplay requirement.

### 3.4 Urgent warnings cannot rely solely on communication infrastructure

E1-12 accessibility and critical alert policy must still provide visual equivalents. If the intercom is
unpowered, a canonical fire/raid warning remains an actual threat; the system may report that communication
coverage failed, but simulation safety cannot disappear into an inbox.

### 3.5 Messages do not directly alter behavior

A posted request does not automatically transfer items or assign work.
An announcement does not directly modify morale.
A personal message does not directly change affinity.
A memorial notice does not directly apply grief.

Responses must route through the owning domain system.

### 3.6 Base delivery and receipt use no RNG

If a survivor is in a covered room and the intercom is working, the broadcast is deliverable.
If mail has a valid route/delivery policy, it reaches the recipient according to deterministic schedule.
Random read/response behavior, if desired, belongs to E1-6 Autonomy/social behavior and must be keyed and
bounded.

### 3.7 Boards are infrastructure/presentation channels, not duplicated rooms

Board location uses canonical E1-9 room IDs.
Physical installation belongs to E1-9/Construction.
Condition belongs to E1-17 if the asset is maintainable.
Powered intercom components depend on PowerGrid.

### 3.8 “Internal mail” needs scope

If survivors live inside one compact bunker, simulating letter-carrier pathfinding for every note may be
wasteful. Mail delivery can be an abstract deterministic channel unless physical delivery creates meaningful
gameplay. The first release should not create a postal-service simulator.

### 3.9 Templates are content, not autonomous event generation

`communication_templates.json` should provide localized structures/text patterns. It should not spawn random
requests, warnings, celebrations, or social events without canonical sources.

### 3.10 Message-count quests are risky

“Send 50 messages” and “archive 100 messages” incentivize spam. Prefer meaningful communication milestones
that require successful use of the system.

## 4. Non-Negotiable Rules

- InternalCommunication does not replace the global event bus.
- Canonical systems remain functional even if no communication artifact is published unless communication
  delivery itself is an intended gameplay gate.
- DutyRoster owns duties.
- ShelterSchedule owns schedule facts.
- Leadership/Governance owns leadership authority and decisions.
- SurvivorRelations owns relationships.
- E1-6 Autonomy owns survivor initiative/response decisions where enabled.
- MemorialSystem/E1-23 own death and memorial facts.
- ShelterSecurity owns threat/security state.
- E1-3 owns external information truth/claims/rumor propagation.
- E1-12 owns critical audio-accessibility mapping and accessible warning equivalence.
- E1-9 owns room/topology/infrastructure installation.
- E1-17 owns maintainable communication-hardware condition.
- PowerGrid owns powered channel availability.
- InternalCommunication owns message artifacts, channel publication, delivery envelopes, receipt/read/ack
  state, channel-specific archive/expiry, and communication UI state only.
- Message content must have provenance: explicit author input, canonical source fact, or authored template.
- A message cannot create the fact it describes.
- `isRead` is per recipient or per delivery receipt.
- Public broadcasts do not duplicate one full message object per recipient.
- Read/ack state is bounded and compact.
- Acknowledgement is distinct from compliance/action.
- “Received” is distinct from “read.”
- “Read” is distinct from “understood.”
- “Acknowledged” is distinct from “completed.”
- Priority affects presentation/notification policy, not canonical event severity unless severity source says so.
- An author cannot mark their own leadership warning “emergency” to bypass canonical emergency rules without
  permission policy.
- Board capacity is channel presentation/storage policy, not a reason to delete canonical warnings/events.
- Message expiry removes/archive presentation; it does not erase the source fact.
- Deleting mail deletes/archives the communication artifact according to policy, not the underlying quest,
  duty, warning, or memorial.
- Private mail access rules must be explicit.
- No implicit surveillance of all private messages by player/leadership unless governance/game design
  explicitly permits it.
- No relationship bonus merely for sending messages.
- No morale bonus merely for receiving announcements.
- No item transfer merely for posting a request.
- No RNG for delivery where channel capability deterministically permits delivery.
- Old saves do not fabricate message history.
- Old saves do not get a physical free board unless a pre-existing board/room asset maps to it.
- A compatibility virtual notice surface may exist if UI parity requires one, but must not pretend to be
  constructed infrastructure.
- Critical warnings remain accessible even when intercom power fails.
- Message floods are coalesced, summarized, archived, or rate-limited.
- First release should prove one board, one intercom broadcast, and one private mail flow before 20+ templates
  and broad social automation.

## 5. Acceptance Slices

### Slice A — Message artifact + bulletin board
Post one explicit/canonical message to one board; read/archive deterministically.

### Slice B — Intercom + coverage
Broadcast a leadership or safety announcement over real room/power/infrastructure capability.

### Slice C — Private mail + receipts
Send one private message and track delivered/read/responded as separate states.

### Slice D — Canonical integrations
Duty/Schedule/Memorial/Security adapters publish messages without becoming dependent on them.

### Slice E — Social automation/content scale
Only after spam/noise, privacy, save size, and response ownership are proven.

Do not start with 20 templates, 100-message archives, and autonomous survivor correspondence before Slices A–C.


---

## E1-24A — Premise verification and communication-authority audit

**Goal:** Verify message/event, leadership, duty, schedule, relations, autonomy, memorial, security, information, topology, power, maintenance, localization, notification, and save authorities before adding internal communication state.

### Required substeps

1. Inspect `DutyRosterSystem`, `SurvivorRelationsSystem`, `ShelterScheduleSystem`, `ShelterEncounterSystem` and `KindIntercomOffice`, Leadership/Governance, E1-6 Autonomy, E1-3 information network, E1-12 audio accessibility/alerts, E1-23/Memorial, ShelterSecurity, E1-9 topology/construction, E1-17 maintenance, PowerGrid, UI notifications, localization, event bus, QuestSystem, and save registry.
2. Search for message, mail, notice, announcement, alert, notification, board, intercom, office, broadcast, inbox, acknowledgement, receipt, communication, schedule notice, memorial notice, and request APIs.
3. Determine whether a generic notification/message DTO already exists in host/UI code.
4. Determine whether in-game events already have localized subject/body/priority fields.
5. Determine whether rooms expose occupancy/presence queries.
6. Determine whether player can see all survivor private state by current product convention.
7. Determine whether critical alerts already bypass/duplicate audio channels for accessibility.
8. Determine whether `intercom_office` is a real room capability or narrative-only string.
9. Create `docs/systems/INTERNAL_COMMUNICATION_AUTHORITY_MAP.md`.
10. Create intake duplicate-search evidence linking Plan 157 external radio, E1-3 information, E1-12 audio accessibility, E1-20 roles, E1-21 diplomacy, E1-23 death/memorial, security, duty, and schedule plans.
11. Set `PREMISE_VERIFIED_AT` to current HEAD.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24B — Internal communication ownership ADR

**Goal:** Define internal communication as message artifact + channel delivery/receipt state, not a second event or behavior system.

### Required substeps

1. Compare one `InternalCommunicationSystem`, a message registry plus channel adapters, and UI-only notification persistence.
2. Define owned facts: message artifact, audience, source provenance, publication channels, delivery envelopes, receipt/read/ack status, board/intercom/mail channel state, archive/expiry, privacy/access metadata.
3. Explicitly exclude source event truth, duty state, schedule state, relationship state, morale, security severity, external intel truth, room topology, power, and hardware condition.
4. Define event-to-message adapters as one-way projections.
5. Define direct player-authored message commands.
6. Define rollback/feature flags.
7. Require second-tool architecture review.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24C — Message identity and immutable content contract

**Goal:** Give every communication artifact stable identity and provenance.

### Required substeps

1. Define message ID, semantic type, author/source actor, source system/event ID, audience policy, subject/content localization/template refs, priority, created time, expiry/archive policy, privacy classification, response-thread ref, and attachment/action refs only if supported.
2. Do not store a single mutable `isRead` field.
3. Separate immutable body from delivery/receipt state.
4. Prefer localized template ID + parameter snapshot over raw English text for system-generated messages.
5. Allow player-entered/free text only if product supports it.
6. Add unique-ID and serialization tests.
7. Use source event ID for dedupe.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24D — Communication type taxonomy

**Goal:** Define message types by semantic purpose without coupling each type to a new subsystem.

### Required substeps

1. Support Announcement, Notice, Request, PersonalMail, Warning, Memorial, Celebration as content/presentation categories.
2. Allow DutyNotice, ScheduleNotice, SecurityWarning, SystemNotice, or InformationReport only if needed as subtypes/tags.
3. Do not make type itself execute behavior.
4. Define valid channels/audiences/priority bounds per type.
5. Define privacy defaults.
6. Add data-integrity tests.
7. Keep taxonomy extensible but small.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24E — Priority semantics

**Goal:** Make priority a communication-presentation property bounded by canonical source severity.

### Required substeps

1. Define Low, Normal, High, Urgent or align with existing notification severity.
2. System-generated warning priority comes from source severity mapping.
3. Leadership/player manual priority cannot masquerade as canonical fire/raid severity unless permission policy allows an emergency drill/manual emergency broadcast.
4. Priority affects notification prominence, sorting, interruption policy, expiry, and acknowledgement request.
5. Priority does not change morale/relationships/action success.
6. Add priority-escalation permission tests.
7. Reuse E1-12 severity semantics where possible.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24F — Audience policy contract

**Goal:** Represent message recipients without duplicating one message per person.

### Required substeps

1. Define audience kinds: PublicShelter, RoleGroup, DutyGroup, Room/Zone, ExplicitRecipients, Individual, Leadership, Site/Outpost if later extended.
2. Resolve membership through canonical survivor/role/duty/location state at publish or delivery time according to policy.
3. Document snapshot audience versus dynamic audience semantics.
4. Do not use recipient name strings as identity.
5. Add audience resolution tests.
6. Keep first release PublicShelter + Individual + ExplicitGroup.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24G — Per-recipient receipt state

**Goal:** Track delivery, reading, acknowledgement, and response separately for recipients who need persistent state.

### Required substeps

1. Define message ID + recipient ID receipt key.
2. States: Pending/Deliverable, Delivered, Read, Acknowledged, Returned/Failed where meaningful.
3. Do not treat Read as Acknowledged.
4. Do not treat Acknowledged as action completed.
5. Store timestamps/days only where useful.
6. Use sparse state: create receipt rows only for addressed messages or meaningful acknowledgement.
7. Add state-transition/idempotency tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24H — Broadcast receipt aggregation

**Goal:** Avoid one persistent receipt row per survivor for routine public announcements when gameplay does not require it.

### Required substeps

1. Define aggregate delivery/coverage metrics for ordinary broadcasts.
2. Persist explicit acknowledgement only for messages requiring it.
3. Use derived current audience count.
4. Keep delivered/read estimates presentation-only unless exact receipts are actually tracked.
5. Do not fabricate read status for unseen board posts.
6. Add large-population save-size tests.
7. Define when exact acknowledgement is mandatory.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24I — Response/thread contract

**Goal:** Model replies as linked messages rather than recursive mutable response arrays.

### Required substeps

1. Define thread ID and optional parent message ID.
2. Each response is its own message artifact with author/audience/provenance.
3. Do not embed unbounded nested response lists inside a message DTO.
4. Preserve ordering by created time + stable ID.
5. Define thread privacy inheritance.
6. Add reply/thread/save tests.
7. Bound archived thread history.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24J — Bulletin-board identity and E1-9 boundary

**Goal:** Represent boards as installed/defined communication channels at canonical room locations.

### Required substeps

1. Reuse E1-9 room/node IDs.
2. Define board ID, location ref, board policy/definition ID, capacity/archive policy, access/posting policy, and visible message refs.
3. E1-9/Construction owns physical installation if boards are buildable.
4. Do not store room coordinates.
5. Use pre-authored board capability when shelter already has one.
6. Add board/location/reference tests.
7. Do not create physical board on old-save migration without provenance.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24K — Bulletin-board posting transaction

**Goal:** Publish a message to a board without duplicating source content or bypassing access policy.

### Required substeps

1. Validate board exists/accessible.
2. Validate author posting permission.
3. Validate message type/channel compatibility.
4. Create/reuse immutable message artifact.
5. Add board publication record/message ref.
6. Apply capacity/archive policy.
7. Emit `MessagePublished` after commit.
8. Add double-post/reload tests.
9. Do not mutate source duty/event.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24L — Bulletin-board capacity and archive policy

**Goal:** Prevent full boards from deleting important facts or creating spam chores.

### Required substeps

1. Define visible-slot capacity separately from archive retention.
2. Expired/routine notices may auto-archive.
3. Urgent/mandatory notices cannot be silently evicted before safe expiry/ack policy.
4. Board capacity affects visible board surface, not global message registry truth.
5. Allow manual pin/unpin only if UI needs it.
6. Add full-board/urgent/pinned/expiry tests.
7. Keep archive bounded.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24M — Bulletin-board access policy

**Goal:** Use canonical role/governance context for who can post or read restricted boards.

### Required substeps

1. Policies may include All, Leadership, StaffRole, DutyGroup, Security, Medical, or explicit authorization.
2. E1-20 roles may satisfy posting authorization.
3. Do not duplicate leadership/role membership.
4. Reading restrictions should be rare and explicit.
5. Do not treat public board as private channel.
6. Add permission-change tests.
7. Show blocked reasons.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24N — Board message discovery/read semantics

**Goal:** Define when a survivor/player is considered to have seen a physical board post.

### Required substeps

1. Player UI viewing may mark player-facing read state, but survivor knowledge is separate.
2. If survivor-level reading matters, require canonical presence/free-time/autonomy interaction or abstract periodic access policy.
3. Do not automatically mark every survivor as read merely because a board exists.
4. Do not simulate individual board-reading unless it affects gameplay.
5. Use E1-3/knowledge boundary for facts that matter as survivor knowledge.
6. Add simple/advanced mode tests.
7. Default first release to player-visible board without individual NPC read simulation unless required.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24O — Intercom infrastructure identity

**Goal:** Represent intercom as a channel capability over real shelter infrastructure.

### Required substeps

1. Audit `intercom_office` and existing audio/speaker assets.
2. Define controller/source ID, speaker/coverage zone refs, channel capability ID, power requirement, and E1-17 maintainable refs.
3. E1-9 owns installed speakers/control rooms.
4. PowerGrid owns power.
5. E1-17 owns hardware condition.
6. InternalCommunication owns broadcast artifact and resolved delivery/coverage result.
7. Add infrastructure reference tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24P — Intercom coverage derivation

**Goal:** Compute coverage from topology and operational infrastructure rather than storing a global authoritative percentage.

### Required substeps

1. Resolve active speaker zones/room coverage.
2. Use E1-9 topology and room IDs.
3. Require PowerGrid availability where speakers/controller are powered.
4. Require E1-17 condition/capability.
5. Account for isolated/disabled zones.
6. Return covered rooms/survivors and derived percentage.
7. Do not persist global networkCoverage as simulation truth.
8. Add healthy/power-loss/hardware-failure/expanded-shelter tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24Q — Intercom broadcast transaction

**Goal:** Create one broadcast and deliver it to the resolvable covered audience.

### Required substeps

1. Validate author/broadcast permission.
2. Validate controller/channel availability unless emergency fallback policy exists.
3. Resolve content, priority, target audience/coverage.
4. Create one immutable broadcast message.
5. Create publication/broadcast record.
6. Record coverage snapshot/result.
7. Do not duplicate full message per room.
8. Add double-broadcast/reload tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24R — Emergency intercom boundary

**Goal:** Make intercom one delivery channel for emergencies without becoming the threat authority.

### Required substeps

1. ShelterSecurity/fire/disaster/medical/etc. source owns emergency fact/severity.
2. Communication adapter publishes semantic warning.
3. E1-12 ensures visual/accessibility equivalence.
4. If intercom is down, warning remains available through canonical accessible UI and other channels according to design.
5. Do not suppress threat because broadcast failed.
6. Do not let communication priority create a threat.
7. Add intercom-down emergency tests.
8. Record communication failure separately.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24S — Acknowledgement policy

**Goal:** Use acknowledgement only when it matters operationally.

### Required substeps

1. Routine notices require no acknowledgement.
2. Important leadership notices may request acknowledgement.
3. Emergency drills/orders may request acknowledgement if canonical leadership/security policy uses it.
4. Acknowledgement means 'message seen/confirmed', not obedience.
5. Do not auto-assign duty on acknowledgement.
6. Keep acknowledgement receipts sparse.
7. Add ack/no-ack/partial-coverage tests.
8. Expose missing acknowledgements without punitive hidden effects.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24T — Internal mail scope ADR

**Goal:** Decide whether internal mail is abstract messaging or physical delivery work.

### Required substeps

1. Assess shelter scale, survivor movement, mail slots, gameplay value, and Duty burden.
2. Recommended first release: deterministic abstract delivery to recipient mailbox/inbox when sender/recipient/channel are valid.
3. Physical hand-delivery may be an optional social action or special quest.
4. Do not create postal pathfinding/job scheduling for every note.
5. Define delivery latency if any.
6. Define privacy/access.
7. Require ADR before physical-mail simulation.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24U — Internal mail envelope

**Goal:** Separate private message content from delivery metadata.

### Required substeps

1. Define delivery ID, message ID, sender ID, recipient ID, created time, delivered time, channel/method, status, return reason, mailbox/location ref if physical, and read receipt ref.
2. Do not duplicate message content.
3. Use stable delivery ID.
4. One message can have multiple addressed deliveries if group mail is supported.
5. Add envelope serialization tests.
6. Keep recipient identity canonical.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24V — Mail delivery determinism

**Goal:** Deliver internal mail through explicit deterministic policy.

### Required substeps

1. Validate sender/recipient existence and communication access.
2. Determine immediate/next-period/mail-slot delivery policy.
3. Do not roll random delivery success.
4. Return mail if recipient dead/departed/unreachable according to policy.
5. Use campaign schedule/time.
6. Do not duplicate delivery after reload.
7. Add unavailable/dead/remote/save-boundary tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24W — Mail privacy and player-observability policy

**Goal:** Decide what the player/leadership can see without silently creating total surveillance.

### Required substeps

1. Define private, official, public, sealed/security, and system-generated privacy classes only as needed.
2. Player UI access to survivor private mail must match game governance/design.
3. Leadership role does not automatically read every personal message unless explicit policy allows it.
4. Metadata visibility may differ from content visibility.
5. Do not expose private relationship content through debug-like UI in normal play.
6. Add permission tests.
7. Document privacy model.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24X — Mail read/respond/ignore semantics

**Goal:** Route survivor response behavior through E1-6 Autonomy/social systems if NPC-level response matters.

### Required substeps

1. Delivery does not mean read.
2. Read can occur through explicit player UI or deterministic/autonomy interaction according to design.
3. Response is a new message/action.
4. Ignoring a message is not automatically a relationship penalty.
5. Do not roll random response inside InternalCommunication.
6. Use E1-6/social policy if autonomous responses are enabled.
7. Add response/no-response/save tests.
8. Keep first release simple.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24Y — Leadership broadcast authorization

**Goal:** Use canonical leadership/governance office/role to authorize shelter-wide announcements.

### Required substeps

1. Query Leadership/Governance/E1-20 role state.
2. Allow acting leader according to governance rules.
3. Do not create local leader ID list.
4. Define ordinary announcement versus emergency override permissions.
5. Do not make broadcast itself a governance decree unless linked to a canonical order/policy.
6. Add leader/acting/non-leader tests.
7. Show authority source in UI.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24Z — Leadership announcement versus command boundary

**Goal:** Separate speech from executable shelter policy.

### Required substeps

1. A broadcast may reference a canonical order/duty/policy ID.
2. Communication displays/explains the order.
3. Leadership/Governance/Duty owns execution/compliance.
4. Do not encode `announcement -> morale +10` or `announcement -> assign all guards` locally.
5. Acknowledgement remains informational.
6. Add linked-order/unlinked-announcement tests.
7. Preserve source provenance.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AA — DutyRoster notice integration

**Goal:** Publish duty information without making the board/intercom the assignment authority.

### Required substeps

1. DutyRoster emits assignment/shift-change facts.
2. Adapter may create personal or group duty notice.
3. Message links to canonical duty/shift ID.
4. Deleting/expiring notice does not cancel duty.
5. Reply/request-change routes to Duty/Leadership workflow.
6. Do not assign duty from message receipt.
7. Add assignment-change/cancel/reload tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AB — ShelterSchedule integration

**Goal:** Publish schedule reminders from canonical schedule state.

### Required substeps

1. ShelterSchedule owns event/shift/time.
2. Communication adapter posts/broadcasts selected schedule notices.
3. Use due-time scheduler rather than daily duplicate messages.
4. Expired schedule notice does not alter schedule.
5. Changes/closures publish corrected notice with supersedes/ref link.
6. Add reschedule/cancellation tests.
7. Keep message count bounded.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AC — Request-message contract

**Goal:** Represent requests as communication artifacts that can reference a canonical requested action/resource.

### Required substeps

1. Define request type/ref, requester, intended recipients, urgency, expiry, and optional canonical action link.
2. Posting does not reserve item/labor.
3. Accept/respond operation routes to Inventory/Duty/Quest/social owner.
4. Do not grant relationship bonus for fulfilling a request unless Relations/social event says so.
5. Mark request resolved from canonical completion receipt.
6. Add request/accept/expire/complete tests.
7. Prevent double fulfillment.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AD — Personal social-message boundary

**Goal:** Allow survivor-to-survivor correspondence without creating a parallel relationship simulator.

### Required substeps

1. Relations/autonomy may create message opportunities or consume response events.
2. InternalCommunication owns message/delivery/read thread only.
3. Do not directly change affinity, trust, romance, rivalry, or grief.
4. Content may reference existing relationship context.
5. Respect privacy.
6. Add friend/rival/partner/no-relationship tests.
7. Feature-gate autonomous social mail until noise budget passes.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AE — Memorial notice integration

**Goal:** Publish death/remembrance notices from E1-23/Memorial without duplicating death records.

### Required substeps

1. Use death record/memorial event ID.
2. Create one memorial notice template with survivor name/date/location as appropriate.
3. Board/intercom delivery is optional presentation.
4. Deleting notice does not erase memorial.
5. Psychology/grief remains external.
6. Add death-notice dedupe tests.
7. Use restrained wording.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AF — Celebration notice integration

**Goal:** Publish celebrations only from canonical event/calendar/achievement sources.

### Required substeps

1. Examples: birthday if age/calendar authority supports it, project completion, treaty signing, holiday, shelter milestone.
2. Do not randomly invent celebrations in communication tick.
3. Needs/Relations owns any social/morale consequence.
4. Use source event ID.
5. Add source/no-source tests.
6. Keep template content localized.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AG — Security warning integration

**Goal:** Publish security alerts from ShelterSecurity without becoming a threat resolver.

### Required substeps

1. Use canonical security incident ID/severity/location.
2. Map severity to communication priority through validated policy.
3. Publish via intercom/boards/UI as appropriate.
4. E1-12 provides accessible equivalent.
5. Do not mark incident resolved because warning was read.
6. Do not expose secret security intel to unauthorized audience unless policy says so.
7. Add raid/breach/false-alarm/source-retraction tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AH — Conflict-message integration

**Goal:** Allow InterpersonalConflict to create mediation notices/messages without duplicating conflict state.

### Required substeps

1. Conflict authority owns dispute/status/participants.
2. Communication may deliver invitation, complaint, mediation notice, apology message, or decision notice.
3. Replies route to conflict/social systems.
4. Do not change relationships merely because message exists.
5. Respect private/public context.
6. Add conflict-resolved-before-delivery test.
7. Feature-gate until Plan 202 authority exists.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AI — E1-3 external-information boundary

**Goal:** Allow external intelligence/rumors to be internally disseminated without becoming new world truth.

### Required substeps

1. E1-3 owns claim/source/confidence/knowledge.
2. InternalCommunication can publish a claim/report reference to a shelter audience.
3. Posting a rumor does not increase its truth confidence.
4. Track who received/read it only if survivor-level knowledge matters.
5. Do not duplicate rumor propagation graph.
6. Add confirmed/false/uncertain/stale intel notice tests.
7. Preserve source attribution.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AJ — Communication-to-knowledge gate

**Goal:** Decide whether reading a message changes survivor knowledge and use a single knowledge owner.

### Required substeps

1. Audit whether E1-3 supports shelter-local knowledge/claims.
2. If yes, message receipt/read may submit knowledge-acquisition evidence.
3. If not, keep communication UI/player-facing and avoid inventing a second survivor belief state.
4. Do not infer that every recipient believes a claim.
5. Read != believe.
6. Add boundary tests.
7. Require ADR before per-survivor knowledge simulation.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AK — Information correction/retraction

**Goal:** Support superseding stale or wrong notices without rewriting historical artifacts.

### Required substeps

1. Create corrected message with `supersedes`/`retracts` reference.
2. Mark prior message retracted/superseded in presentation.
3. Do not delete history silently if it mattered.
4. E1-3 owns truth/confidence changes.
5. Notify recipients appropriately for high-priority corrections.
6. Add retraction/save tests.
7. Keep thread/history bounded.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AL — Channel routing policy

**Goal:** Choose appropriate channels from message semantics without duplicating content recklessly.

### Required substeps

1. Routine public notice -> board/UI.
2. Important leadership message -> board + optional intercom.
3. Urgent canonical warning -> E1-12 accessible alert + intercom where available.
4. Private mail -> mail channel only unless escalation policy exists.
5. Memorial -> board/chronicle with optional announcement.
6. Do not create separate message IDs for the same semantic message per channel unless content differs.
7. Add routing tests.
8. Store publication refs.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AM — Multi-channel deduplication

**Goal:** Ensure one announcement shown on board and intercom is recognized as one semantic communication.

### Required substeps

1. Use one message ID with multiple publication/channel records.
2. Notifications coalesce by message ID.
3. Read/ack policy defines whether one channel satisfies receipt.
4. Do not create duplicate journal events.
5. Add board+intercom+UI notification tests.
6. Preserve per-channel delivery evidence.
7. Use stable correlation ID.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AN — Communication infrastructure power integration

**Goal:** Use PowerGrid for intercom/electronic board/mail terminal availability.

### Required substeps

1. Facility definitions declare power needs.
2. PowerGrid owns supply/brownout.
3. Manual physical boards require no power.
4. Electronic intercom/terminals may fail when unpowered.
5. Do not store independent power state.
6. Allow fallback channels.
7. Add healthy/unpowered/brownout tests.
8. Show current reason in UI.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AO — Communication hardware maintenance integration

**Goal:** Use E1-17 for speaker/controller/terminal/board hardware condition where maintainable.

### Required substeps

1. Define maintainable asset refs only for hardware worth maintaining.
2. E1-17 owns wear/repair/failure.
3. InternalCommunication queries channel capability.
4. Do not add local condition.
5. Physical board should probably not require elaborate maintenance unless gameplay value exists.
6. Add failed-speaker/partial-zone/restored tests.
7. Keep maintenance burden low.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AP — Shelter topology integration

**Goal:** Resolve communication zones from E1-9 room graph without creating a second shelter layout.

### Required substeps

1. Use canonical room IDs.
2. Intercom speakers declare covered room/zone refs.
3. Boards/mail points live at canonical room nodes.
4. Expanded rooms require explicit communication coverage provider or remain uncovered.
5. Do not store copied room coordinates.
6. Add E1-9 expansion/room-removal tests.
7. Recompute derived coverage on topology change.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AQ — Derived communication coverage read model

**Goal:** Expose coverage as a diagnostic summary only.

### Required substeps

1. Compute channel-specific coverage: board access, intercom powered coverage, private-mail reachability.
2. Optionally derive overall summary with labeled assumptions.
3. Do not persist 0–100 networkCoverage as authority.
4. Do not use the overall summary to decide whether a specific survivor received a message.
5. Show uncovered rooms/reasons.
6. Add consistency tests.
7. Keep channel-specific detail.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AR — Message expiration semantics

**Goal:** Expire presentation/delivery opportunities without erasing canonical source state.

### Required substeps

1. Define expiry by message type.
2. Permanent memorial/archival notices may not expire from archive.
3. Schedule notices expire after relevance window.
4. Requests expire/resolve according to request policy.
5. Expired private mail may remain archival rather than disappear.
6. Use campaign time.
7. Add expiry/time-skip tests.
8. Do not re-emit expired source events automatically.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AS — Archival/retention policy

**Goal:** Prevent message history from growing without bound while preserving important community memory.

### Required substeps

1. Keep important memorial, treaty, leadership, crisis, and authored personal-message summaries longer.
2. Aggregate/archive routine notices.
3. Compact per-recipient read receipts when no longer needed.
4. Retain source event/message IDs for dedupe where necessary.
5. Do not retain 100 duplicate duty reminders forever.
6. Define retention by type/importance.
7. Add long-campaign save-size tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AT — Notification coalescing and flood control

**Goal:** Prevent communication from overwhelming the player.

### Required substeps

1. Coalesce repeated duty/schedule notices.
2. Group multiple low-priority messages.
3. Rate-limit noncritical popups.
4. Do not rate-limit canonical critical accessibility alert itself.
5. Use inbox unread count rather than modal interruption for routine mail.
6. Summarize repeated requests.
7. Add heavy-message-load tests.
8. Track notification volume metrics.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AU — Priority queue and due processing

**Goal:** Process deliveries/expiry using due indexes instead of scanning every message each tick.

### Required substeps

1. Index pending deliveries by due time.
2. Index expirations by expiry time.
3. Process only due records.
4. Broadcast delivery resolves at commit/time window.
5. Do not tick every archived message.
6. Use campaign time.
7. Add time-skip tests.
8. Measure queue cost.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AV — No-RNG baseline

**Goal:** Make channel delivery, coverage, posting, and expiry deterministic.

### Required substeps

1. Board posting is deterministic.
2. Intercom coverage is deterministic from infrastructure/power/topology.
3. Mail delivery is deterministic from policy.
4. Read/ack changes occur from explicit interaction or canonical autonomy policy.
5. Do not call global `ISeededRng` for routine communication.
6. If autonomous response text choice uses variation, key it by message/thread and keep it presentation-only.
7. Add same-input/call-order tests.
8. Document zero-RNG baseline.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AW — Message template catalog

**Goal:** Use templates for localized content structure without turning them into event generators.

### Required substeps

1. Define template ID, semantic type, localization subject/body keys, required parameters, allowed channels, default priority, privacy, expiry policy, and source-system tags.
2. Do not define random spawn frequency in communication templates.
3. Validate parameter schemas.
4. Do not hard-code survivor names in template text.
5. Start with 8–12 useful templates before 20+.
6. Add localization/data-integrity tests.
7. Keep authored free text separate.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AX — Template source-provenance validation

**Goal:** Ensure every system-generated message can point back to an actual canonical source.

### Required substeps

1. Security warning requires security incident ref.
2. Memorial notice requires death/memorial ref.
3. Duty notice requires duty/shift ref.
4. Schedule notice requires schedule ref.
5. Request requires explicit request/action source.
6. Celebration requires canonical milestone/calendar event.
7. Reject orphan system-generated messages in integrity test.
8. Allow explicit leadership/personal authored messages without source event.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AY — Communication panel UI

**Goal:** Create one integrated inbox/boards/announcements surface without duplicating domain dashboards.

### Required substeps

1. Tabs/filters for Inbox, Boards, Announcements, Requests, Archive as appropriate.
2. Show subject, author, time, priority, channel, read/ack state, privacy, and source link.
3. Allow mark read/archive according to policy.
4. Link to canonical duty/security/memorial/quest detail rather than copying full state.
5. Support search/filter if message count warrants it.
6. Add empty/noisy/urgent/private snapshots.
7. Keep UI responsive.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24AZ — Board detail UI

**Goal:** Render visible board posts and posting permissions from canonical board state.

### Required substeps

1. Show board location/name.
2. Show pinned/active messages.
3. Show capacity/visible slots.
4. Show expired/archive navigation.
5. Show author/type/priority/expiry.
6. Post through authoritative command.
7. Do not expose restricted board content.
8. Add permission/full-board tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BA — Mail compose/inbox UI

**Goal:** Support private messaging with explicit recipient/privacy and no hidden social reward.

### Required substeps

1. Resolve recipient from canonical survivor roster.
2. Show unavailable/dead/departed status.
3. Compose subject/body/template where supported.
4. Show delivery/read/reply state.
5. Respect privacy policy.
6. Reply creates new message.
7. Delete/archive affects communication artifact only.
8. Add compose/send/reply/return tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BB — Leadership announcement UI

**Goal:** Provide authorized broadcast composition without conflating it with executable leadership orders.

### Required substeps

1. Show author authority.
2. Choose audience/channel/priority within permissions.
3. Allow link to canonical order/policy if available.
4. Show intercom coverage before send.
5. Warn about uncovered rooms/power failure.
6. Use E1-12 for emergency alert semantics.
7. Add unauthorized/emergency/coverage tests.
8. Do not apply effects directly.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BC — Message detail and provenance UI

**Goal:** Make messages auditable and clear about source versus communication.

### Required substeps

1. Show canonical source reference label when available: Duty assignment, Security incident, Memorial, Schedule event, Treaty, Request.
2. Show delivered/read/ack separately.
3. Show superseded/retracted state.
4. Show thread/replies.
5. Do not reveal hidden source data the player is not allowed to know.
6. Add source-link tests.
7. Keep raw IDs in debug only.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BD — Critical accessibility integration

**Goal:** Ensure all essential communicated cues remain usable without hearing or powered intercom.

### Required substeps

1. Map security/medical/fire/radiation/power emergencies to E1-12 accessible semantic notifications.
2. Intercom audio is supplementary presentation.
3. Show text/icon/location/severity.
4. Do not require opening mail to learn a life-critical immediate warning.
5. Respect reduced-stimulation settings.
6. Add audio-muted/intercom-down tests.
7. Keep accessibility outside campaign save where appropriate.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BE — Request fulfillment handoff

**Goal:** Close request messages from canonical action completion rather than from a reply alone.

### Required substeps

1. Request references action/resource need.
2. Reply may accept/decline.
3. Canonical system executes item transfer/duty/help action.
4. Completion receipt resolves request.
5. Do not mark complete because someone wrote 'done'.
6. Prevent duplicate fulfillment.
7. Add accepted-but-failed/completed/canceled tests.
8. Keep message state synchronized by receipt.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BF — Read/ack versus compliance boundary

**Goal:** Prove communication state cannot substitute for actual survivor behavior.

### Required substeps

1. Read duty notice does not mean duty performed.
2. Acknowledged evacuation order does not mean survivor moved.
3. Read medical request does not mean treatment occurred.
4. Read security warning does not mean defense deployed.
5. Response message does not mean relationship changed.
6. Add negative integration tests.
7. Use canonical action completion refs.
8. Document semantics prominently.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BG — Survivor autonomy and message response

**Goal:** If autonomous messaging is enabled, use E1-6 as the intent owner.

### Required substeps

1. Incoming message can become context/input to autonomy.
2. Autonomy may propose read/respond/request-help actions.
3. InternalCommunication executes message command only after intent/action approval.
4. Do not score autonomous responses locally.
5. Do not spam replies from every low-priority notice.
6. Use cooldown/importance thresholds.
7. Add autonomy-enabled/disabled tests.
8. Feature-gate first release.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BH — Social request and conflict safety

**Goal:** Prevent messaging from becoming an exploit for affinity, coercion, or harassment loops.

### Required substeps

1. Relations/social system decides consequences of messages.
2. Repeated sending may be throttled or ignored by autonomy/privacy policy.
3. Do not grant affinity per message.
4. Do not allow unrestricted urgent-message spam to every survivor.
5. Conflict escalation requires canonical social/conflict context.
6. Add spam/rival/blocked-user policy tests if blocking exists.
7. Keep content moderation within authored game scope.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BI — Communication blackout state

**Goal:** Derive channel failures from infrastructure/power/topology rather than storing one generic blackout flag.

### Required substeps

1. Intercom blackout from power/controller/speaker failure.
2. Electronic mail terminal blackout from power/hardware failure.
3. Physical boards remain usable if accessible.
4. UI can derive 'communications degraded' summary.
5. Do not block all internal UI messages unless gameplay explicitly models player terminal access.
6. Add partial/full-failure tests.
7. Show fallback channels.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BJ — Old-save migration

**Goal:** Introduce communication prospectively without fabricating message history or physical boards.

### Required substeps

1. Initialize message registry/delivery/archive empty.
2. Preserve all duty/schedule/memorial/security state without generating historical messages for past events.
3. Map a pre-existing `intercom_office`/board asset only if stable topology data proves it.
4. Otherwise expose a migration-safe virtual shelter notice UI or no physical board according to product parity decision.
5. Do not create fake read receipts.
6. Do not backfill old memorial notices.
7. Version migration.
8. Add early/late/expanded-shelter save fixtures.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BK — Save contract and restoration order

**Goal:** Persist communication-owned state without copying source systems.

### Required substeps

1. Persist messages, publication/channel refs, delivery envelopes, sparse receipts/acks, threads, board visible/archive refs, broadcast records, retention cursors, settings that are campaign communication policy, and schema version.
2. Do not persist copied duty, schedule, security, memorial, relationship, intel, topology, power, or hardware condition.
3. Restore survivor/topology/channel assets before validating recipients/channels.
4. Restore source systems before source links.
5. Rebuild derived coverage/unread summaries.
6. Do not re-publish source events on restore.
7. Handle missing recipient/room/template refs.
8. Add round-trip/corruption tests.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BL — User preference versus campaign policy split

**Goal:** Keep notification preferences separate from simulated communication state.

### Required substeps

1. Audio volume/reduced stimulation remain E1-12/user settings.
2. UI sorting/filter preferences remain user/profile settings if persistent.
3. Campaign communication policies include posting permissions, auto-delivery policy, archive rules only when they affect simulation.
4. Do not store personal UI notification preference in campaign save.
5. Add save-scope tests.
6. Document settings owner.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BM — Time-skip semantics

**Goal:** Advance mail delivery, scheduled notices, expirations, and archives exactly once.

### Required substeps

1. Use CampaignCalendar only.
2. Process due deliveries/events in deterministic order.
3. Do not generate one reminder per skipped day when a single current notice suffices.
4. Expired routine notices may archive during skip.
5. Critical canonical events still exist independently.
6. Add 1/7/30/100-day skip tests.
7. Compare stepped versus batch state.
8. Prevent duplicate notifications after load.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BN — Message-flood and archive stress test

**Goal:** Prove thousands of routine communications do not degrade save/UI or bury emergencies.

### Required substeps

1. Generate representative Duty/Schedule/mail/board traffic across a long campaign.
2. Measure active messages, archived messages, receipts, thread count, save size, unread count calculation, board render time, search/filter cost, and allocations.
3. Exercise one emergency amid heavy routine traffic.
4. Verify priority sort/coalescing keeps it visible.
5. Apply retention/compaction.
6. Record median/p95 UI/read-model cost.
7. Add regression thresholds.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BO — Communication player-burden audit

**Goal:** Ensure the feature adds community texture rather than inbox chores.

### Required substeps

1. Measure messages/player-week.
2. Measure required acknowledgement actions.
3. Measure manual archive/delete actions.
4. Measure redundant source-system notifications.
5. Prefer auto-archive/coalescing for routine notices.
6. Make private correspondence optional/meaningful.
7. Do not require reading every board message for optimal survival.
8. Set first-release noise budget.
9. Run playtest review.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BP — Privacy and information-leak audit

**Goal:** Prevent internal communication UI from exposing hidden knowledge or private data incorrectly.

### Required substeps

1. Test private mail visibility.
2. Test restricted board.
3. Test security-classified warning.
4. Test E1-3 rumor/source identity restrictions.
5. Test survivor hidden traits/relationship facts in templates.
6. Test dead/departed recipient mail.
7. Test leadership permissions.
8. Add negative info-leak assertions.
9. Document intentionally player-visible abstractions.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BQ — Exploit and idempotency audit

**Goal:** Prevent message spam, duplicate source projection, repeated request rewards, and acknowledgement farming.

### Required substeps

1. Test source event delivered twice.
2. Test post button double-click.
3. Test multi-channel same message.
4. Test reply double-submit.
5. Test request completion callback twice.
6. Test save before mail delivery.
7. Test expiry/retraction around save.
8. Test urgent-priority spam permission.
9. Test quest/achievement message-count farming.
10. Add stable operation/provenance assertions.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BR — Quest hook redesign

**Goal:** Prefer meaningful communication milestones instead of raw spam counts.

### Required substeps

1. Potential hooks: publish first shelter notice, successfully communicate a critical warning to all reachable zones, resolve a survivor request, establish intercom coverage after shelter expansion, preserve a memorial notice, coordinate a schedule change without missed duty.
2. Defer `send 50 messages`, `20 broadcasts`, `archive 100 messages` unless they have strong gameplay rationale.
3. QuestSystem owns lifecycle/rewards.
4. Use message/source/action IDs as provenance.
5. Do not reward sending low-value spam.
6. Add dedupe tests.
7. Keep hooks optional.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BS — Tutorial/onboarding

**Goal:** Teach channels and semantic boundaries using one useful message flow.

### Required substeps

1. First tutorial creates/receives one real notice.
2. Explain board versus private mail.
3. Explain intercom coverage/power if available.
4. Explain Read/Acknowledged versus actual action only when relevant.
5. Do not force all communication panels at once.
6. Use existing tutorial settings.
7. Add no-board/no-intercom fallback.
8. Keep copy localized.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BT — Headless internal-communication selftest

**Goal:** Build a deterministic CI scenario proving board publication, intercom coverage, mail receipts, canonical source links, and save/load.

### Required substeps

1. Load a shelter with canonical rooms and one board/intercom capability.
2. Publish one Duty/Schedule notice to board.
3. Broadcast one leadership/security message.
4. Verify Power/E1-17/topology determine intercom coverage.
5. Send one private mail.
6. Advance deterministic delivery.
7. Mark delivered/read/ack separately.
8. Reply with linked message.
9. Publish one memorial notice from E1-23 event.
10. Verify deletion/expiry does not mutate source systems.
11. Save/reload at publication/delivery/receipt boundaries.
12. Create `--internal-communication-selftest`.
13. Assert communication save contains no copied Duty, Schedule, Relations, Security, Intel, Power, topology, or condition state.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BU — Data-integrity selftest

**Goal:** Validate message templates, channels, board/intercom infrastructure refs, audiences, priorities, source adapters, and permissions.

### Required substeps

1. Validate unique template/channel/board IDs.
2. Validate room/topology refs.
3. Validate E1-17/Power capability refs.
4. Validate message type/channel compatibility.
5. Validate priority bounds.
6. Validate audience/role/duty refs.
7. Validate source-system template provenance requirements.
8. Validate localization parameters.
9. Validate no template embeds direct morale/relationship/duty/security mutations.
10. Fail with actionable diagnostics.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BV — Documentation and observability

**Goal:** Document message ownership, receipt semantics, channel infrastructure, privacy, source projection, and accessibility boundaries.

### Required substeps

1. Create `docs/systems/INTERNAL_COMMUNICATION.md`.
2. Create `docs/systems/COMMUNICATION_MESSAGE_CONTRACT.md`.
3. Create `docs/systems/COMMUNICATION_RECEIPTS.md`.
4. Create `docs/systems/COMMUNICATION_CHANNELS.md`.
5. Create `docs/systems/COMMUNICATION_PRIVACY.md`.
6. Document E1-3/E1-12/E1-9/E1-17 boundaries.
7. Document zero-RNG delivery baseline.
8. Add debug readout for message ID, source ref, audience, publications, deliveries, receipts, priority, expiry, retraction, channel capability, and blockers.
9. Keep debug mutations dev-only.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

## E1-24BW — Release gate and closure

**Goal:** Ship only when internal communication creates meaningful shelter information flow without becoming a second event bus, behavior controller, information-truth system, or notification spam engine.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run internal-communication selftest.
4. Run message/receipt/idempotency tests.
5. Run board/topology/access tests.
6. Run intercom Power/E1-17/coverage tests.
7. Run E1-12 emergency-accessibility tests.
8. Run mail/privacy/autonomy boundary tests.
9. Run Duty/Schedule/Memorial/Security/E1-3 integration tests.
10. Run old-save migration tests.
11. Run time-skip/retention tests.
12. Run flood/privacy/exploit audits.
13. Run long-campaign save/UI performance soak.
14. Verify board + intercom + one private-mail flow before broad template/social automation expansion.
15. Update ADR, authority map, docs, plan register, and handoff.
16. Mark DONE only when messages enrich shelter life while canonical source systems remain singular.

**Gate:** Preserve canonical ownership, deterministic/idempotent delivery, privacy, receipt semantics, and source provenance; add one negative boundary test.

---

# 6. Canonical Communication Authority Matrix

| Fact | Canonical owner | E1-24 role |
|---|---|---|
| Source event/fact | Duty/Schedule/Security/Memorial/etc. | Reference |
| External claim/truth | E1-3 | Reference/publish |
| Leadership authority | Leadership/Governance | Permission query |
| Relationship state | SurvivorRelations | Context only |
| Survivor autonomy | E1-6 | Optional response intent |
| Message artifact | E1-24 | Own |
| Channel publication | E1-24 | Own |
| Delivery envelope | E1-24 | Own |
| Read/ack receipt | E1-24 | Own |
| Board/intercom physical installation | E1-9 | Reference |
| Hardware condition | E1-17 | Query |
| Power availability | PowerGrid | Query |
| Critical accessibility alert | E1-12 | Semantic parallel |
| Room topology/occupancy | E1-9/Shelter | Query |
| Actual duty completion | DutyRoster | No duplicate |
| Security resolution | ShelterSecurity | No duplicate |
| Memorial/death | E1-23/Memorial | No duplicate |
| UI preference | User settings/profile | Separate from campaign |

---

# 7. Suggested Message Record

```yaml
schema_version: 1
message_id: "msg_000481"
message_type: "WARNING"

author:
  kind: "SYSTEM"
  id: "shelter_security"

source:
  system: "ShelterSecurity"
  event_id: "security_incident_0091"

content:
  template_id: "security_warning_breach"
  parameters:
    room_id: "room_east_airlock"

audience:
  kind: "PUBLIC_SHELTER"

priority: "URGENT"
created_day: 73
expires_day: 74
privacy: "PUBLIC"
thread_id: null
```

No `isRead`.

---

# 8. Suggested Delivery Receipt

```yaml
delivery_id: "delivery_msg_000512_survivor_003"
message_id: "msg_000512"
recipient_id: "survivor_003"

channel: "INTERNAL_MAIL"
status: "READ"

delivered_day: 73
read_day: 74
acknowledged_day: null
```

For ordinary public broadcasts, avoid creating one record per survivor unless exact acknowledgement matters.

---

# 9. Publication Model

One semantic message can have multiple channel publications:

```text
message_id: msg_0042
  |
  +--> board publication
  +--> intercom broadcast
  +--> UI notification
```

Do not create three unrelated copies.

---

# 10. Read / Acknowledge / Act

These states are distinct:

```text
Delivered
   !=
Read
   !=
Acknowledged
   !=
Acted Upon
   !=
Completed
```

The final two normally belong to canonical domain systems.

---

# 11. Bulletin Board Model

Board owns:

- visible message refs;
- posting/access policy;
- archive policy;
- location ref.

Board does not own:

- source event;
- survivor knowledge;
- room topology;
- leadership state.

---

# 12. Intercom Coverage

Derived:

```text
coverage =
installed speaker zones
∩ valid topology
∩ powered hardware
∩ serviceable hardware
```

The resulting room/survivor set determines delivery opportunity.

A 0–100 summary may be shown, never used instead of exact channel resolution.

---

# 13. Emergency Communication

Correct:

```text
security incident exists
-> E1-12 accessible warning
-> E1-24 intercom/board publication where available
-> canonical security response continues
```

Incorrect:

```text
intercom failed
-> no raid/fire warning exists
-> security threat effectively disappears
```

---

# 14. Duty Notice Boundary

```text
DutyRoster creates assignment
-> E1-24 publishes notice
-> survivor/player reads notice
-> DutyRoster still owns assignment and completion
```

Deleting the notice does not cancel the duty.

---

# 15. Request Boundary

```text
message request
-> recipient responds/accepts
-> canonical Inventory/Duty/Quest/social action
-> completion receipt
-> request marked resolved
```

No resource moves because a sentence was posted.

---

# 16. External Information Boundary

```text
E1-3 claim
-> internal report message
-> recipients may learn claim through knowledge owner if enabled
```

Posting a rumor does not increase confidence or convert it into truth.

---

# 17. Privacy Model

Minimum useful classes:

- Public
- Official/Restricted
- Private

Add more only if a real consumer requires them.

Player access to private mail must be a deliberate governance/UI abstraction, not an accidental consequence of
having a global inbox.

---

# 18. Message Retention

Suggested:

- urgent crisis notices: retain summary/history;
- memorial notices: long retention;
- personal mail: bounded thread archive;
- routine duty/schedule notices: short retention;
- repetitive automated notices: aggregate.

Retain dedupe/source IDs longer than visible content where needed.

---

# 19. Old-Save Migration

Default:

```text
message registry = empty
mail = empty
receipts = empty
archives = empty
historical source events = not backfilled
```

Physical boards/intercom are mapped only when real topology/infrastructure proves they existed.

No fabricated “100% coverage.”

---

# 20. Noise Budget

Track:

- routine messages/day;
- urgent messages/day;
- popups/day;
- unread mail count;
- board posts/day;
- duplicate source projections;
- messages per survivor/week;
- acknowledgement prompts/week;
- auto-archived messages;
- player time in communication UI.

The system fails UX acceptance if routine notifications bury critical ones.

---

# 21. Quest Hook Guidance

Prefer:

- establish communication in a newly expanded shelter wing;
- successfully broadcast a critical warning to all reachable zones;
- resolve a meaningful request;
- coordinate a schedule change;
- preserve a memorial notice;
- restore intercom after a breakdown.

Avoid raw spam achievements as primary mechanics.

---

# 22. Exploit Matrix

| Exploit/failure | Guard |
|---|---|
| Double event creates duplicate notice | Source event/message ID |
| Board + intercom count as two messages | Shared semantic message ID |
| Mark read grants action reward | Read/action separation |
| Request reply grants item twice | Canonical completion receipt |
| Reload re-delivers mail | Delivery operation ID |
| Urgent spam bypasses filters | Permission/severity policy |
| Delete warning erases threat | Source-system ownership |
| Private mail leaks through global UI | Privacy policy |
| Intercom outage hides critical safety alert | E1-12 fallback |
| Message-count quest farming | Meaningful milestone hooks |
| Coverage saved stale after expansion | Derived coverage |
| Archived message resurrects on load | Retention cursor/idempotency |

---

# 23. First Release Scope

Recommended:

1. one communication message registry;
2. one bulletin board;
3. one board posting flow;
4. one intercom controller + coverage query;
5. one leadership/security broadcast;
6. one private mail flow;
7. one delivery/read receipt;
8. one reply thread;
9. one Duty/Schedule source adapter;
10. one Memorial/Security source adapter;
11. save/load selftest;
12. notification flood budget.

Only then add autonomous survivor correspondence and 20+ templates.

---

# 24. Headless Selftest

1. load canonical shelter topology;
2. map one board;
3. map intercom speaker coverage;
4. publish one schedule notice;
5. publish same semantic message on board and UI without duplication;
6. send one leadership broadcast;
7. cut power and verify coverage changes;
8. send private mail;
9. advance delivery;
10. mark read and acknowledge separately;
11. reply in a thread;
12. publish memorial/security source message;
13. expire/archive routine message;
14. save/reload;
15. assert source systems were never mutated by message state.

---

# 25. Performance Strategy

Avoid:

- per-frame scans of all messages;
- per-recipient copies for every broadcast;
- unbounded response arrays;
- rebuilding all coverage every frame;
- retaining every routine notification forever.

Prefer:

- immutable shared message artifacts;
- sparse receipts;
- due-time queues;
- topology/power/maintenance invalidation;
- bounded archives;
- aggregate broadcast receipts;
- dirty read models.

---

# 26. Rollback Strategy

### Autonomous mail
Disable independently.

### Intercom
Disable while boards/mail remain.

### Board infrastructure
Fall back to read-only virtual communication UI if product requires compatibility.

### Source adapters
Disable individually.

### Exact NPC receipt tracking
Fall back to player-facing/read-model delivery.

Never roll back by copying Duty, Relations, Security, E1-3, Power, topology, or maintenance state into E1-24.

---

# 27. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --internal-communication-selftest
```

Suggested targeted suites:

- `InternalCommunicationAuthorityBoundaryTests`
- `CommunicationMessageTests`
- `CommunicationPriorityTests`
- `CommunicationAudienceTests`
- `CommunicationReceiptTests`
- `CommunicationThreadTests`
- `BulletinBoardTests`
- `BulletinBoardAccessTests`
- `IntercomInfrastructureTests`
- `IntercomCoverageTests`
- `IntercomEmergencyBoundaryTests`
- `InternalMailTests`
- `InternalMailPrivacyTests`
- `LeadershipBroadcastTests`
- `DutyNoticeIntegrationTests`
- `ScheduleNoticeIntegrationTests`
- `RequestMessageTests`
- `MemorialNoticeTests`
- `SecurityWarningTests`
- `ExternalInformationCommunicationTests`
- `CommunicationPowerBoundaryTests`
- `CommunicationMaintenanceBoundaryTests`
- `CommunicationMigrationTests`
- `CommunicationTimeSkipTests`
- `CommunicationFloodTests`
- `CommunicationPrivacyTests`
- `CommunicationExploitTests`
- `CommunicationPerformanceTests`

---

# 28. Completion Checklist

- [ ] Premise audit completed at current HEAD.
- [ ] Communication ownership ADR accepted.
- [ ] Message artifacts are immutable and provenance-linked.
- [ ] No global `isRead` field exists.
- [ ] Receipt/read/ack are distinct.
- [ ] Public broadcasts use sparse/aggregate receipt state.
- [ ] Thread replies are separate linked messages.
- [ ] Bulletin boards use E1-9 room identity.
- [ ] Board access policy is canonical-role/governance-aware.
- [ ] Board capacity cannot erase source truth.
- [ ] Intercom coverage is derived.
- [ ] PowerGrid controls powered availability.
- [ ] E1-17 owns hardware condition.
- [ ] E1-12 preserves critical accessible warnings.
- [ ] Internal mail delivery uses deterministic policy.
- [ ] Privacy model is explicit.
- [ ] Leadership authority remains canonical.
- [ ] Announcements do not directly execute orders.
- [ ] Duty/Schedule messages are projections only.
- [ ] Requests require canonical fulfillment.
- [ ] Personal mail does not directly change relationships.
- [ ] Memorial messages do not duplicate death/grief.
- [ ] Security messages do not resolve threats.
- [ ] E1-3 remains external-information truth owner.
- [ ] Survivor knowledge is not duplicated without ADR.
- [ ] Multi-channel publications dedupe by message ID.
- [ ] Coverage summary is derived, not persisted authority.
- [ ] Retention/expiry do not erase source facts.
- [ ] Templates do not generate random events.
- [ ] Old saves receive no fabricated history/infrastructure.
- [ ] Time-skip is exact.
- [ ] Spam/noise/privacy audits pass.
- [ ] Board + intercom + private mail vertical slices pass before broad automation.
- [ ] `E1_planintegration[25].md` is the next sequence filename.

---

# 29. Final Directive

Plan 211 should make the shelter feel like a community that actually communicates.

Survivors should see notices, receive letters, encounter requests, hear announcements, remember deaths, and
understand schedule or security changes through in-world channels. Leadership should be able to address the
shelter. New shelter wings should need communication coverage if that infrastructure matters. A failed
intercom should be noticeable.

But none of those channels should become the systems they describe.

The architectural standard is:

**events create facts; communication creates messages about those facts; recipients receive and acknowledge
messages; canonical systems own what happens next.**

If `InternalCommunicationSystem` starts assigning duties, changing morale or relationships, resolving
security incidents, deciding rumor truth, storing room/power/condition copies, or treating acknowledgement as
obedience, stop and restore the boundary.

---

# 30. Scenario Review Bank

For each scenario, identify the canonical source fact, semantic message, channel publication, audience,
delivery/read/acknowledgement state, privacy, infrastructure/power/maintenance dependencies, retention,
save/load idempotency, and one assertion proving communication does not become domain truth.

1. A duty assignment changes after its first notice was posted.
2. A raid starts while the intercom controller is unpowered.
3. One security warning is published on board, intercom, and UI simultaneously.
4. A survivor receives private mail, reads it, and never responds.
5. The player deletes a duty notice before the shift starts.
6. A leader sends an urgent broadcast when no canonical emergency exists.
7. A memorial notice references an existing E1-23 death record.
8. A false E1-3 rumor is published internally and later corrected.
9. An unauthorized survivor attempts to access a restricted board.
10. A newly constructed E1-9 shelter wing has no speaker coverage.
11. A speaker is powered but failed under E1-17.
12. Thirty skipped days contain repetitive schedule notices.
13. The same source event is delivered twice around autosave.
14. A private-mail recipient dies before delivery.
15. Hundreds of routine messages arrive around one life-critical warning.
16. A maximum-length campaign accumulates thousands of messages and receipts.
