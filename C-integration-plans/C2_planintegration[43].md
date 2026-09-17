# C2 — Flagship Integration Plan [43]: Time Capsules, Legacy Messages, Delayed Discovery, and In-Campaign Cross-Generational Communication

> **Deliverable:** `C2_planintegration[43].md`
> **Source scope:** Plan 212 — *Time Capsule & Legacy Messages*
> **Primary objective:** create a deterministic in-campaign system for survivors to package messages, records, keepsakes, and physical belongings for later discovery or delivery; support date-, survivor-, event-, and manual-opening conditions; preserve creator/recipient provenance across death and generation changes; and connect discoveries to memorial, fate, inheritance, archive, item ownership, morale, relationships, events, and UI without creating duplicate inventory, inheritance, archive, memorial, journal, survivor-memory, or cross-campaign legacy authorities.
> **Required execution order:** **212A Foundation / Temporal-Delivery Contract → 212B Capsule Creation, Discovery, Opening, Legacy Messages & UI → 212C Cross-System Integration, Save/CI, Exploit Control, Long-Horizon Retention, and Emotional-Narrative Closure**
> **Hard dependencies:** `MemorialSystem`; `SurvivorFateSystem`; `ISimClock`; semantic `EventSystem`; Plan 206 `DeathLegacySystem`; Plan 210 `PersonalBelongingsSystem`; Plan 162 Shelter Archive; canonical Inventory/Item authority; canonical survivor roster/identity; morale/relationships; Plan 31 semantic event vocabulary; Plan 36 port-contract discipline; Plan 39 save durability; Plan 55 retention; Plan 25 localization; Plan 37/184 accessibility.
> **Critical ownership correction:** a time capsule **does not own copies of physical items**. Physical contents remain canonical inventory/belonging item instances transferred into a sealed-container custody state. Opening transfers those same instances through canonical inventory ownership. Likewise, legacy messages are in-campaign communication records, not Plan 140 cross-campaign meta-progression.
> **Critical lifecycle correction:** **discovery and opening are separate facts**. A capsule may be found before its opening condition is satisfied. Discovery reveals that the capsule exists; opening exposes its sealed contents and message only when policy permits.
> **Scope discipline:** no duplicate inventory ledger, no duplicate inheritance system, no duplicate archive/history database, no second memorial system, no automatic “create capsule on death” that fabricates a dead survivor’s authorship, no open-condition polling over all historical events every frame, no arbitrary sentimental-value stat that directly grants rewards, no UI-open side effects, no save/load reroll of discovery or reactions, no infinite reward loops from create/open cycles, and no requirement that a survivor be dead for their future message to be meaningful.

---

# 0. Executive Intent

ASHFALL already contains strong systems for:

- survivor death and fate,
- memorialization,
- personal belongings,
- inheritance,
- journals and archives,
- long-term campaign chronology.

Those systems preserve **what happened** and **what survived**.

Plan 212 adds a different temporal layer:

```text
what someone intentionally leaves behind
→ for someone else
→ to encounter later
```

The desired player experience is:

```text
survivor chooses to leave something for the future
        │
        ├─ writes a message
        ├─ selects real belongings/records
        ├─ chooses a shelter location
        └─ defines when/for whom it may be opened
        │
        ▼
sealed time capsule
        │
        ├─ remains hidden or known
        ├─ survives creator death
        ├─ can be inherited/custodied
        └─ waits for condition
        │
        ▼
discovery
        │
        ├─ found intentionally
        ├─ found accidentally
        ├─ inherited
        └─ delivered to designated recipient
        │
        ▼
opening eligibility
        │
        ├─ date reached
        ├─ intended survivor eligible
        ├─ event occurred
        └─ manual/open-anytime policy
        │
        ▼
opening
        │
        ├─ message read
        ├─ physical contents transferred
        ├─ emotional response emitted
        └─ history/archive updated
```

The strongest product outcome is:

> **A survivor can deliberately place part of their present into the shelter’s future. Years later, another survivor can discover the sealed object, understand who made it and why, wait for or fulfill its opening condition, read the message, inherit the actual preserved belongings, and create a new emotional or historical consequence from a real piece of campaign history.**

---

# 1. Source Diagnosis

The source establishes that:

- no dedicated time capsule/legacy message system exists,
- `MemorialSystem` already covers remembrance,
- `SurvivorFateSystem` already covers death tracking,
- `ISimClock` already owns time,
- Plan 206 covers death/inheritance,
- Plan 210 covers personal belongings,
- Plan 162 covers shelter archive/history,
- Plan 140 covers **cross-campaign** legacy and therefore must remain distinct,
- four opening conditions are required:
  - date-based,
  - survivor-based,
  - event-based,
  - manual,
- capsule content types include:
  - item,
  - letter,
  - photo,
  - recording,
  - drawing,
  - artifact,
- legacy messages include:
  - specific survivor,
  - next generation,
  - anyone,
  - shelter leader,
- discovery types include:
  - accident,
  - searching,
  - inherited,
  - designated recipient,
- 10+ capsule templates are required,
- events, quest hooks, tutorial, UI, save/load, deterministic seeding, old-save behavior, headless processing, and `--time-capsule-selftest` are required.

The source also contains several design points that need normalization.

### 1.1 `isOpen` is not enough

A capsule needs at least:

```text
Hidden
Discovered
EligibleToOpen
Opened
Destroyed/Lost
```

because discovery and opening are different.

### 1.2 Physical contents cannot be copied into a capsule DTO

If an item is placed inside a capsule:

```text
canonical item instance
→ transferred into capsule custody
```

not:

```text
capsule.contents.Add(itemId)
while inventory still owns the same item
```

### 1.3 `auto-create on death` cannot create retroactive authored messages

A dead survivor cannot suddenly have written a capsule merely because the setting is enabled.

Safer interpretation:

```text
auto-seal prepared legacy package on death
```

only if that package/message was authored while alive.

### 1.4 Sentimental value is not a reward meter

The source suggests:

```text
sentimentalValue 0–100
```

Prefer an authored/derived narrative weight used for selection and emotional context.

Do not convert it directly into free morale/currency.

### 1.5 Event-based opening needs stable event predicates

Do not store only:

```text
openEvent = event_id
```

if the event is a historical instance that may never occur again.

Separate:

```text
event kind / predicate
event instance ref
```

depending intended semantics.

---

# 2. Program-Level Success Criteria

C2[43] closes only when all of the following are true.

1. `TimeCapsuleSystem.cs` exists.
2. The system has versioned capture/restore.
3. Capsules have stable IDs.
4. Legacy messages have stable IDs.
5. Discovery records have stable IDs.
6. Four source opening condition families are supported.
7. Capsule discovery and capsule opening are distinct transitions.
8. A capsule can be discovered while still sealed.
9. A capsule can be opened only once.
10. A legacy message can be delivered only once unless explicitly repeatable.
11. A message can be delivered and remain unread.
12. Read state is independent from delivery state.
13. Physical items placed in a capsule are real canonical item instances.
14. Sealed physical items are removed from normal available inventory.
15. Opening transfers physical items back through canonical inventory/belongings transactions.
16. Capsule item loss/destruction cannot duplicate items.
17. Creator death does not invalidate a valid pre-existing capsule.
18. Death-triggered messages deliver from a pre-authored record only.
19. Date conditions consume `ISimClock`.
20. Event conditions consume canonical semantic events.
21. Survivor conditions resolve against canonical survivor state.
22. Manual conditions require explicit player action.
23. Designated recipient rules are enforced.
24. `next_generation` resolves through canonical generation/lineage eligibility if available.
25. `shelter_leader` resolves against the current canonical leadership system at delivery/opening time.
26. Emotional responses route through canonical morale/relationships/psychology.
27. No capsule owns a second grief/relationship ledger.
28. Memorial integration uses the existing `MemorialSystem`.
29. Death/inheritance integration uses Plan 206.
30. Personal belongings integration uses Plan 210.
31. Archive integration uses Plan 162.
32. Old saves start with no capsules/messages.
33. No historical backfill occurs automatically.
34. Discovery/opening is deterministic.
35. Save/load cannot reroll accident discovery, reaction selection, or delivery.
36. UI opening does not trigger duplicate consequences.
37. `time_capsule_templates.json` contains 10+ valid templates.
38. Templates reference valid content types, condition types, and localization keys.
39. No-capsule campaigns remain valid.
40. Many-capsule campaigns remain bounded through notification and retention policy.
41. Same seed + same capsule state + same events produce the same discovery/opening history.
42. `--time-capsule-selftest` passes.

---

# 3. Architectural Invariants

## 3.1 Inventory remains physical-item authority

TimeCapsuleSystem may own:

```text
capsule custody association
```

but not a cloned inventory.

## 3.2 PersonalBelongingsSystem remains personal-ownership meaning

An heirloom in a capsule may still be a personal belonging.

TimeCapsuleSystem only controls delayed access.

## 3.3 DeathLegacySystem remains inheritance authority

A capsule can be inherited as a sealed object.

Plan 206 decides inheritance/estate transfer.

## 3.4 MemorialSystem remains remembrance authority

A capsule may be associated with a memorial.

It does not become a second grave/memorial record.

## 3.5 Shelter Archive remains historical record authority

A capsule opening may create archive-worthy structured history.

The capsule system does not become the canonical shelter chronicle.

## 3.6 Plan 140 remains cross-campaign authority

C2[43] is strictly in-campaign unless a future explicit bridge exports a summary.

## 3.7 Discovery is not opening

This is a hard invariant.

## 3.8 Opening is not reading

A capsule can be opened, while some messages/records may remain unread.

## 3.9 Delivery is not reading

Legacy messages can be delivered and later read.

## 3.10 Emotional response is a consequence request

Canonical morale/relationship/psychology systems apply durable effects.

---

# 4. Canonical Lifecycle Model

Use explicit capsule states:

```text
Draft
Sealed
Hidden
DiscoveredSealed
EligibleToOpen
Opened
Lost
Destroyed
Archived
```

A minimal lifecycle:

```text
Draft
→ Sealed
→ Hidden
→ DiscoveredSealed
→ EligibleToOpen
→ Opened
```

Alternative:

```text
Hidden
→ EligibleToOpen
→ DiscoveredSealed
→ Opened
```

if eligibility occurs before discovery.

Never derive everything from one `isOpen` bool.

---

# 5. Legacy Message Lifecycle

Use:

```text
Draft
Prepared
Waiting
Deliverable
DeliveredUnread
Read
Expired
Cancelled
```

Typical:

```text
Prepared
→ Waiting
→ Deliverable
→ DeliveredUnread
→ Read
```

Death message:

```text
Prepared while alive
→ author dies
→ Deliverable
→ DeliveredUnread
```

---

# 6. Workstream 212A — Foundation / Temporal Delivery Contract

## Goal

Create a normalized capsule/message state model, explicit condition evaluation, deterministic lifecycle transitions, physical item custody, and safe migration.

---

# 7. 212A Phase A — Create `TimeCapsuleSystem`

Path:

```text
Assets/Ashfall.Core/Communication/TimeCapsuleSystem.cs
```

Responsibilities:

- create/edit draft capsule,
- validate capsule content,
- seal capsule,
- bind shelter location,
- register opening condition,
- track discovery state,
- evaluate opening eligibility,
- open capsule,
- coordinate physical item transfer,
- manage legacy message scheduling/delivery,
- generate discovery/read consequence intents,
- expose UI read models,
- capture/restore owned state.

---

# 8. 212A Phase B — Definition vs Runtime Split

Static:

```text
TimeCapsuleTemplate
LegacyMessageTemplate
CapsuleReactionProfile
CapsuleConditionDefinition
CapsuleContentDefinition
```

Runtime:

```text
TimeCapsuleRecord
CapsuleContentRef
CapsuleDiscoveryRecord
LegacyMessageRecord
CapsuleCustodyRecord
CapsuleConditionState
TimeCapsuleState
```

---

# 9. 212A Phase C — `TimeCapsuleRecord`

Recommended fields:

```text
capsule_id
template_id optional
capsule_name
creator_id
created_day
sealed_day
location_id
status
open_condition
content_refs
primary_message_ref optional
designated_recipient_policy
discovery_ref optional
opened_day optional
opened_by optional
archive_ref optional
```

---

# 10. 212A Phase D — `CapsuleContentRef`

Recommended:

```text
content_id
content_type
author_id
canonical_item_instance_id optional
text_record_id optional
media_record_id optional
sentimental_weight
provenance_tags
```

Do not embed the whole item DTO.

---

# 11. 212A Phase E — Content Types

Preserve source:

```text
Item
Letter
Photo
Recording
Drawing
Artifact
```

Clarify:

- Item / Artifact may map to physical item instance.
- Letter / Drawing can be text/document records.
- Photo / Recording require an existing media/content representation or authored catalog record.

Do not create unsupported binary media infrastructure just to satisfy enum values.

---

# 12. 212A Phase F — Media Fallback

If actual photo/audio-recording assets are not supported:

- represent as authored textual record with media type metadata,
- show localized description/transcript.

Do not block capsule gameplay.

---

# 13. 212A Phase G — Sentimental Weight

Replace direct gameplay meaning with:

```text
sentimental_weight 0..100
```

used for:

- reaction template weighting,
- archive significance,
- UI ordering.

It does not directly award morale.

---

# 14. 212A Phase H — Creator Identity

Creator must:

- exist,
- be alive during creation/sealing unless importing authored historical capsule,
- have permission/context to create.

No posthumous auto-authorship.

---

# 15. 212A Phase I — Capsule Creation Transaction

Creation:

1. begin draft,
2. select message/content,
3. validate physical items,
4. choose location,
5. choose opening condition,
6. choose recipient policy,
7. preview,
8. seal.

Physical item custody transfers only at seal.

---

# 16. 212A Phase J — Draft Cancellation

Before seal:

- no item transfer committed,
- no capsule history event.

If temporary reservation is used:

- release atomically.

---

# 17. 212A Phase K — Seal Transaction

On seal:

```text
validate creator
validate contents
validate location
validate condition
reserve/transfer physical items
commit capsule ID
commit sealed state
emit capsule_created
```

All-or-nothing.

---

# 18. 212A Phase L — Capsule Custody

Define:

```text
ICapsuleItemCustodyPort
```

Operations:

```text
MoveIntoCapsule(itemInstanceId, capsuleId)
TransferOutOfCapsule(itemInstanceId, recipient/container)
DestroyWithCapsule(...)
ValidateCustody(...)
```

Implementation uses Inventory/PersonalBelongings owner.

---

# 19. 212A Phase M — No Item Duplication

After sealing:

- item absent from normal usable inventory,
- same instance appears as sealed custody,
- item cannot be traded/crafted/equipped.

---

# 20. 212A Phase N — Stackable Items

If stackable item instances support quantity:

- transfer exact quantity,
- canonical inventory creates/splits instance according to its rules.

Do not fake quantity in capsule DTO.

---

# 21. 212A Phase O — Perishable Contents

Audit item decay.

Policy options:

```text
normal decay continues
preservation modifier from capsule
disallow specific perishables
```

TimeCapsuleSystem does not become item-condition authority.

---

# 22. 212A Phase P — Physical Capsule Container

If the capsule itself is a craftable item/container:

- canonical item system owns container instance.

If not:

- capsule is shelter fixture/logical storage record.

Choose one architecture.

Avoid both.

---

# 23. 212A Phase Q — Shelter Location

Use canonical `room_id` / shelter topology.

Validate location exists and supports hiding/storage.

---

# 24. 212A Phase R — Location Categories

Possible:

```text
ArchiveRoom
Storage
WallCache
UnderFloor
MemorialArea
PrivateRoom
WorkshopCache
```

Only map to real shelter rooms/slots.

---

# 25. 212A Phase S — Location Destruction

If room destroyed/renovated:

- capsule may be exposed,
- moved,
- damaged,
- destroyed,

through shelter event adapter.

Do not ignore world change.

---

# 26. 212A Phase T — Opening Condition Union

Use typed union:

```text
DateOpenCondition
SurvivorOpenCondition
EventOpenCondition
ManualOpenCondition
```

Do not store all fields with `-1/null` sentinel combinations if avoidable.

---

# 27. 212A Phase U — Date Condition

Fields:

```text
target_day
allow_after_day
```

Default:

```text
open on or after target day
```

If exact-day-only exists, define explicitly.

---

# 28. 212A Phase V — Survivor Condition

Clarify semantics.

Possible:

```text
DesignatedSurvivorDiscovers
DesignatedSurvivorAliveAndPresent
DesignatedSurvivorReachesLifeStage
```

Baseline source:

```text
open when specific survivor finds it
```

Use discovery event by that survivor.

---

# 29. 212A Phase W — Event Condition

Use:

```text
semantic_event_kind
optional subject/context predicate
```

Examples:

- next winter begins,
- shelter reaches milestone,
- leader changes,
- child comes of age,
- war ends.

Do not bind to an event instance that already occurred unless immediate eligibility is intended.

---

# 30. 212A Phase X — Manual Condition

Means:

```text
once discovered/accessible, player may open at will
```

not:

```text
player can open a still-undiscovered hidden capsule from omniscient menu
```

---

# 31. 212A Phase Y — Compound Conditions

Source requires four categories, not compound logic.

Do not add AND/OR condition trees in baseline unless needed.

Future extension can support:

```text
date + recipient
```

through schema versioning.

---

# 32. 212A Phase Z — Condition Evaluation

Event-driven:

- date index checks on day advance,
- survivor discovery event,
- semantic event subscription,
- explicit manual request.

No per-frame polling.

---

# 33. 212A Phase AA — Eligibility State

Condition met:

```text
condition_satisfied = true
```

Capsule may still remain undiscovered.

---

# 34. 212A Phase AB — Discovery State

Discovery answers:

```text
who learned where the capsule is?
when?
how?
```

Opening answers separately.

---

# 35. 212A Phase AC — `CapsuleDiscoveryRecord`

Fields:

```text
discovery_id
capsule_id
discovered_by
discovered_day
discovery_type
location_ref
condition_was_met
reaction_profile_id
reaction_application_ids
```

---

# 36. 212A Phase AD — Discovery Types

Preserve source:

```text
FoundByAccident
Searching
Inherited
DesignatedRecipient
```

Potential:

```text
LocationExposed
```

only if shelter destruction integration needs it.

---

# 37. 212A Phase AE — Accidental Discovery

Must have a real trigger.

Examples:

- room maintenance,
- renovation,
- scavenging storage,
- survivor search activity.

No arbitrary random global discovery while nobody interacts with location.

---

# 38. 212A Phase AF — Searching Discovery

Player/survivor takes actual search action if such shelter activity exists.

If no search system exists:

- use a bounded explicit “Search hidden storage” action in capsule UI/location.

Consumes time.

---

# 39. 212A Phase AG — Inherited Discovery

Plan 206 transfers knowledge/custody of capsule.

A sealed capsule may pass to heir.

Opening condition still applies unless inheritance itself is the condition.

---

# 40. 212A Phase AH — Designated Discovery

If creator intentionally tells/assigns recipient:

- capsule can become known at intended trigger.

No random discovery needed.

---

# 41. 212A Phase AI — Discovery Does Not Reveal Contents

Discovered-sealed view can show:

- creator if labeled,
- capsule name if labeled,
- seal/open condition if known,
- exterior description.

Contents/message remain hidden until open.

---

# 42. 212A Phase AJ — Opening Request

`TryOpenCapsule` validates:

- capsule discovered/accessible,
- not already opened,
- condition met or manual policy,
- opener eligibility,
- location/custody valid.

---

# 43. 212A Phase AK — Opening Transaction

On successful open:

1. commit opening outcome,
2. mark capsule opened,
3. transfer physical items,
4. expose message/media,
5. create read/unread records,
6. emit emotional consequence intents,
7. journal/archive hooks,
8. persist idempotency.

---

# 44. 212A Phase AL — Failed Opening

Examples:

- too early,
- wrong recipient,
- inaccessible location,
- damaged lock/seal if modeled.

Do not apply opening consequences.

---

# 45. 212A Phase AM — Early Opening

Source does not require violating conditions.

Baseline:

```text
not allowed
```

Possible future moral choice:

```text
break seal early
```

only if explicitly authored with consequences.

---

# 46. 212A Phase AN — Legacy Message Record

Recommended:

```text
message_id
author_id
recipient_policy
recipient_id optional
content_template_key / authored_text_ref
created_day
delivery_condition
delivery_status
delivered_day optional
delivered_to optional
read_status
read_day optional
source_capsule_id optional
```

---

# 47. 212A Phase AO — Recipient Types

Preserve:

```text
SpecificSurvivor
NextGeneration
Anyone
ShelterLeader
```

---

# 48. 212A Phase AP — Specific Survivor

Recipient must resolve.

If recipient dies before delivery:

- policy determines:
  - undeliverable,
  - heir,
  - archive,
  - anyone fallback.

Do not silently redirect.

---

# 49. 212A Phase AQ — Next Generation

Needs canonical lineage/generation meaning.

Preferred:

```text
next eligible generation cohort after author's generation
```

If generational system unavailable:

- template disabled,
- not guessed from age alone.

---

# 50. 212A Phase AR — Anyone

Deliver to first eligible discoverer/reader according to channel.

---

# 51. 212A Phase AS — Shelter Leader

Resolve **current leader at delivery time** through canonical leadership authority.

Do not persist a stale leader ID at creation unless message intentionally addresses a named leader.

---

# 52. 212A Phase AT — Delivery Conditions

Preserve:

```text
OnDeath
OnDate
OnEvent
Immediate
```

---

# 53. 212A Phase AU — On Death

Message must be prepared while author alive.

On canonical death event:

```text
Prepared
→ Deliverable
```

No fabricated content.

---

# 54. 212A Phase AV — On Date

Use `ISimClock`.

---

# 55. 212A Phase AW — On Event

Use semantic event predicate.

---

# 56. 212A Phase AX — Immediate

Delivers through current communication/UI route.

It is still a legacy-message object only if design wants recorded intentional message.

Avoid using this system as generic messaging/chat.

---

# 57. 212A Phase AY — Read State

`DeliveredUnread` remains valid indefinitely subject to retention.

Effects tied to **reading** should not apply at delivery.

---

# 58. 212A Phase AZ — Unread Message After Recipient Death

Policy:

- may be inherited,
- archived,
- remain undelivered,
- become anyone-readable.

Data-driven.

---

# 59. 212A Phase BA — Reaction Model

Use:

```text
CapsuleReactionAssessment
```

Inputs:

- creator alive/dead,
- relationship,
- sentimental content,
- time elapsed,
- message tone,
- memorial context,
- recipient identity,
- surprise/discovery type.

Output:

- morale intent,
- grief/psychology intent,
- relationship-memory intent,
- archive significance.

---

# 60. 212A Phase BB — No Universal “deceased = grief + appreciation”

Different survivors may react differently.

Possible:

```text
grief
comfort
anger
nostalgia
curiosity
warmth
confusion
indifference
```

Use canonical context.

---

# 61. 212A Phase BC — Deterministic Reaction

If variety requires RNG:

- use `ISeededRng`,
- stable key by capsule/discovery/reader,
- persist resolved reaction.

No reload reroll.

---

# 62. 212A Phase BD — Reaction Effect Boundary

TimeCapsuleSystem does not mutate:

```text
morale
grief
relationship
psychology
```

directly.

Use consequence ports.

---

# 63. 212A Phase BE — Auto-Create-on-Death Setting Correction

Source proposes:

```text
auto-create on death bool
```

Replace with:

```text
auto_finalize_prepared_legacy_package_on_death
```

or disable baseline.

The system cannot author new content for a dead survivor.

---

# 64. 212A Phase BF — Max Capsules

Source proposes `max capsules`.

Use configurable:

```text
max_active_unopened_capsules
```

to bound state/UI.

Completed/opened history follows retention.

---

# 65. 212A Phase BG — Why Limit Exists

Limit is:

- storage/attention/technical bound,
- not arbitrary lore.

Can be increased by shelter archive/storage upgrades if desired later.

---

# 66. 212A Phase BH — `TimeCapsuleState`

Persist:

```text
active_capsules
legacy_messages
discovery_records/recent history
condition indexes
custody refs
idempotency keys
schema version
retention summaries
```

---

# 67. 212A Phase BI — Old Save Compatibility

Exact source behavior:

```text
existing saves get no capsules
```

Also:

```text
no legacy messages
no discoveries
```

---

# 68. 212A Phase BJ — No Historical Backfill

Do not generate:

- old survivors' death messages,
- old capsules,
- retroactive discoveries.

Only new authored content after feature activation.

---

# 69. 212A Phase BK — `GameBootstrap`

Source asks:

```text
SetupTimeCapsules
TickTimeCapsules
SaveTimeCapsules
```

Follow current Plan 28 composition architecture.

Avoid adding manual setup if descriptors/generator own registration.

---

# 70. 212A Phase BL — Tick Model

Daily tick only for:

- date conditions,
- retention,
- bounded accidental-discovery opportunities tied to actual location activity.

Event conditions are event-driven.

---

# 71. 212A Phase BM — Deterministic RNG Stream

Use:

```text
time_capsules
```

Stable keys:

```text
capsule ID
day
discovery context
reader ID
reaction phase
```

---

# 72. 212A Phase BN — Semantic Events

Candidate kinds:

```text
time_capsule_created
time_capsule_sealed
time_capsule_discovered
time_capsule_open_eligible
time_capsule_opened
legacy_message_prepared
legacy_message_delivered
legacy_message_read
capsule_item_transferred
```

---

# 73. 212A Phase BO — Port Contract

Mandatory:

- clock,
- survivor roster,
- inventory/custody,
- shelter locations,
- save.

Conditional:

- memorial,
- fate/death,
- death legacy,
- belongings,
- archive,
- morale,
- relations,
- psychology,
- leadership,
- lineage.

---

# 74. 212A Phase BP — Diagnostics

Expose:

```text
TIME_CAPSULES_ACTIVE
TIME_CAPSULES_DISCOVERED
TIME_CAPSULES_OPENED
LEGACY_MESSAGES_WAITING
LEGACY_MESSAGES_DELIVERED
LEGACY_MESSAGES_UNREAD
CAPSULE_ITEMS_IN_CUSTODY
CAPSULE_CUSTODY_ERRORS
TIME_CAPSULE_REQUIRED_PORTS_MISSING
```

---

# 75. 212A Tests

- capsule draft/seal,
- physical item custody,
- date condition,
- survivor condition,
- event condition,
- manual condition,
- discovery-before-open,
- open-before-discovery rejected,
- wrong recipient rejected,
- death delivery,
- message delivered unread,
- read idempotency,
- old-save empty state,
- no posthumous auto-authorship.

---

# 76. 212A Definition of Done

- [ ] `TimeCapsuleSystem.cs`,
- [ ] normalized capsule/message/discovery DTOs,
- [ ] explicit lifecycle states,
- [ ] 4 open-condition families,
- [ ] discovery/open separation,
- [ ] delivery/read separation,
- [ ] physical item custody port,
- [ ] no duplicate inventory,
- [ ] legacy recipient policies,
- [ ] reaction assessment,
- [ ] deterministic RNG,
- [ ] max-active policy,
- [ ] safe death-package behavior,
- [ ] old-save no-backfill,
- [ ] composition integration,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 77. Workstream 212B — Capsule Templates

## Goal

Author a diverse initial capsule corpus without hardcoding survivor biography or creating fake items/media.

---

# 78. 212B Phase A — Data File

Create:

```text
Assets/StreamingAssets/Data/time_capsule_templates.json
```

Recommended sections:

```text
capsule_templates
legacy_message_templates
reaction_profiles
condition_profiles
```

---

# 79. 212B Phase B — Minimum Template Count

Source requires:

```text
10+ capsule templates
```

Recommended initial target:

```text
12 capsule templates
```

for coverage of condition/content/recipient types.

---

# 80. 212B Phase C — Template Fields

```text
template_id
name_key
description_key
creator_requirements
allowed_content_types
default_condition_profile
recipient_policy_options
recommended_location_tags
reaction_profile_ids
archive_significance
repeat_policy
localization keys
```

---

# 81. 212B Phase D — Template 1: Letter to the Next Generation

Contents:

- letter.

Recipient:

- next generation.

Condition:

- date/event/life-stage.

Requires lineage integration.

---

# 82. 212B Phase E — Template 2: Open When Winter Ends

Contents:

- letter,
- small keepsake.

Condition:

- event/date corresponding to winter-end authority.

No hardcoded calendar guess.

---

# 83. 212B Phase F — Template 3: For the Next Leader

Recipient:

```text
ShelterLeader
```

Contents:

- leadership advice,
- record,
- optional personal item.

Uses canonical leadership at delivery/opening.

---

# 84. 212B Phase G — Template 4: If I Don't Come Back

Creator prepares before expedition.

Delivery:

- on death or declared missing/fate condition.

Do not auto-author after death.

---

# 85. 212B Phase H — Template 5: The Family Box

Contents:

- photo/drawing,
- keepsake,
- letter.

Requires canonical family/lineage refs.

---

# 86. 212B Phase I — Template 6: A Record of the Shelter

Contents:

- historical summary/record.

Integrates Plan 162 archive but does not duplicate it.

Can reference archive entry IDs.

---

# 87. 212B Phase J — Template 7: The Apprentice's Inheritance

Creator:

- mentor/high-skill survivor.

Recipient:

- named apprentice.

Contents:

- tool/artifact,
- letter.

Plan 206/210 handles belonging transfer.

---

# 88. 212B Phase K — Template 8: Open on Day N

Pure date capsule.

Useful for anniversaries.

---

# 89. 212B Phase L — Template 9: After the Crisis

Event condition.

Examples:

- war ends,
- reactor stabilized,
- famine resolved,
- shelter rebuilt.

Requires semantic event.

---

# 90. 212B Phase M — Template 10: Hidden in the Wall

Discovery-oriented.

Manual opening after discovery.

High accidental-search flavor.

---

# 91. 212B Phase N — Template 11: A Message to Whoever Finds This

Recipient:

```text
Anyone
```

Condition:

- manual/event.

Supports unknown future discoverer.

---

# 92. 212B Phase O — Template 12: Memorial Cache

Associated with memorial.

Contents:

- personal item,
- letter,
- drawing/photo.

MemorialSystem remains owner.

---

# 93. 212B Phase P — No Fake Biography

Templates may not assume:

- child,
- spouse,
- apprentice,
- profession,
- hometown,
- religion,
- military past

unless bound canonical data supports it.

---

# 94. 212B Phase Q — Context Binding

Templates can bind:

```text
creator
recipient
relationship
event
location
memorial
belonging
archive record
```

from canonical sources.

---

# 95. 212B Phase R — Missing Context

Template becomes ineligible.

No placeholder fabrication.

---

# 96. 212B Phase S — Template Repeat Policy

Examples:

```text
Repeatable
OncePerSurvivor
OncePerRelationship
OncePerMemorial
```

Avoid endless identical capsule creation farming.

---

# 97. 212B Phase T — Content Validation Matrix

| Template | Condition | Recipient | Physical content | Memorial | Death | Lineage | Runtime observed |
|---|---|---|---:|---:|---:|---:|---:|

---

# 98. Workstream 212B — Capsule Creation

## Goal

Make capsule creation a deliberate authored action with real content and temporal intent.

---

# 99. 212B Phase U — Creator Eligibility

Creator must:

- be alive,
- conscious/available,
- able to communicate,
- have access to selected items/location.

If autonomy system exists, some NPC-initiated capsules may occur later.

Baseline can be player-directed.

---

# 100. 212B Phase V — Capsule Name

Use:

- player-entered name if text input supported,
- template-generated localized title.

Do not require free text.

---

# 101. 212B Phase W — Message Authoring Modes

Support:

```text
template message
parameterized authored line
optional player free text
```

if safe/save/localization policy permits.

Baseline should work without free-form text.

---

# 102. 212B Phase X — Physical Contents Selection

UI lists only:

- available,
- transferable,
- non-reserved items.

Show:

- owner,
- sentimental significance,
- condition,
- stack quantity.

---

# 103. 212B Phase Y — Protected Items

Prevent sealing:

- quest-critical item,
- currently equipped essential item,
- nontransferable key object,

unless owning system permits.

---

# 104. 212B Phase Z — Item Ownership Consent

If item belongs to another survivor under Plan 210:

- creator cannot silently take it.

Use canonical transfer/consent policy.

---

# 105. 212B Phase AA — Location Selection

Show real shelter rooms/hidden-cache slots.

Location may affect:

- discovery chance,
- damage risk,
- accessibility.

Do not make location a purely cosmetic string if it drives discovery.

---

# 106. 212B Phase AB — Hiddenness

If needed, derive:

```text
concealment band
```

from location and storage.

Avoid mutable discovery chance stored on capsule.

---

# 107. 212B Phase AC — Condition Preview

UI says:

```text
May be opened on/after Day 120
Only Mara may open this
May be opened after the first thaw
May be opened manually once found
```

---

# 108. 212B Phase AD — Recipient Preview

For dynamic recipient:

```text
"The shelter leader at the time of delivery"
```

not current leader’s name unless binding is static.

---

# 109. 212B Phase AE — Seal Confirmation

Show:

- items becoming unavailable,
- opening condition,
- recipient,
- location,
- whether creator can retrieve before open.

---

# 110. 212B Phase AF — Unsealing Before Trigger

Source does not define edits after sealing.

Recommended:

```text
sealed capsules cannot be edited
```

Creator may deliberately cancel/recover only if policy allows, with explicit event.

No free edit without custody transaction.

---

# 111. 212B Phase AG — Creator Opens Own Capsule

Manual/date capsule may allow.

Recipient-restricted capsule may not.

Rules data.

---

# 112. Workstream 212B — Discovery

## Goal

Make finding an old capsule a meaningful event tied to real shelter activity and chronology.

---

# 113. 212B Phase AH — Discovery Trigger Sources

```text
SearchAction
LocationMaintenance
Renovation
Inheritance
DesignatedDelivery
LocationDamage
AuthorizedLookup
```

---

# 114. 212B Phase AI — Accidental Discovery Probability

Only evaluate when a relevant location interaction occurs.

Factors:

- location concealment,
- activity type,
- survivor observation/search skill,
- time elapsed,
- damage/renovation.

Use `ISeededRng`.

---

# 115. 212B Phase AJ — No Daily Global Lottery

Do not check every hidden capsule every day for random accident.

---

# 116. 212B Phase AK — Searching

If explicit search:

- consumes survivor work/time,
- only searches selected location,
- can find multiple hidden objects but attention budget applies.

---

# 117. 212B Phase AL — Discovery Result

Commit:

- capsule discovered,
- discoverer,
- day,
- method,
- condition status.

---

# 118. 212B Phase AM — Discovery Notification

Example:

```text
Mara found a sealed box behind the workshop wall.
```

Do not expose contents.

---

# 119. 212B Phase AN — Discovered Too Early

Player can:

- leave sealed,
- move to secure storage if rules allow,
- inspect exterior.

No automatic opening.

---

# 120. 212B Phase AO — Discovery After Condition

Capsule becomes:

```text
EligibleToOpen
```

and can prompt opening.

---

# 121. 212B Phase AP — Multiple Discoverers

Only first discovery record is primary.

Later viewers do not create repeated discovery rewards.

---

# 122. Workstream 212B — Opening

## Goal

Separate the emotional/narrative climax from mere discovery and make item/message delivery transactional.

---

# 123. 212B Phase AQ — Eligible Opener

Resolve:

- designated survivor,
- current leader,
- anyone,
- next generation,
- player/manual authority.

---

# 124. 212B Phase AR — Opener Missing

If intended specific survivor is dead/unavailable:

- policy can keep sealed,
- inheritance fallback,
- archive fallback.

No silent substitution.

---

# 125. 212B Phase AS — Open Result

Create:

```text
CapsuleOpenedRecord
```

or store opening fields in capsule.

Need stable:

- day,
- opener,
- delivered content IDs,
- reaction IDs.

---

# 126. 212B Phase AT — Physical Item Transfer

Target:

- opener inventory,
- shelter storage,
- inherited belonging owner,

according to content policy.

One transaction.

---

# 127. 212B Phase AU — Capacity Failure

If inventory/storage full:

- capsule can remain opened with contents in dedicated open-container custody,
- or transfer to shelter storage.

Do not destroy/duplicate.

---

# 128. 212B Phase AV — Reading Text Content

Opening may reveal letters.

Mark each record:

```text
unread/read
```

if UI allows later reading.

---

# 129. 212B Phase AW — Audio/Recording Content

If actual audio exists:

- playback is presentation.
- transcript remains available.

Plan 184 accessibility.

---

# 130. 212B Phase AX — Read Consequences

Apply when message actually read/heard.

Opening an item-only capsule does not fire “message read.”

---

# 131. 212B Phase AY — Historical Age

Derived:

```text
current day - created day
```

Used for display/reaction.

Do not persist mutable age.

---

# 132. 212B Phase AZ — Historical Classification

Potential:

```text
Recent
Old
Generational
Historic
```

thresholds data-driven.

---

# 133. Workstream 212B — Legacy Messages

## Goal

Provide direct delayed communication without requiring a physical capsule.

---

# 134. 212B Phase BA — Write Message

Creator selects:

- recipient,
- delivery condition,
- content/template,
- optional attached record.

No physical item attachment unless converted into a capsule/package.

---

# 135. 212B Phase BB — Message Delivery Channel

Possible:

- shelter archive inbox,
- personal belongings papers,
- leader office,
- memorial record.

Use canonical UI/storage concept.

Do not create magical wireless delivery.

---

# 136. 212B Phase BC — On-Death Delivery

Possible examples:

- sealed letter deposited with archive,
- named recipient receives prepared letter,
- current leader receives instructions.

Canonical death event triggers.

---

# 137. 212B Phase BD — Date Delivery

At target day:

- resolve recipient,
- deliver.

If no eligible recipient:

- apply fallback policy.

---

# 138. 212B Phase BE — Event Delivery

Semantic event triggers.

Example:

```text
when the shelter reaches Day 100
when the next leader takes office
when winter ends
```

---

# 139. 212B Phase BF — Immediate Message

Useful primarily for:

- testing/tutorial,
- current survivor letter.

Avoid making system redundant with dialogue.

---

# 140. 212B Phase BG — Recipient Death Before Delivery

Template policy:

```text
Cancel
Archive
DeliverToHeir
DeliverToNextGeneration
DeliverToAnyone
```

Explicit.

---

# 141. 212B Phase BH — Author Death After Delivery

Message provenance updates:

```text
author now deceased
```

Read reaction can differ.

No rewrite of original content.

---

# 142. 212B Phase BI — Message Read

Persist:

- reader,
- day,
- reaction.

If recipient is group/anyone:

- primary first read can be logged,
- avoid repeated rewards for every UI view.

---

# 143. 212B Phase BJ — Re-reading

Narrative allowed.

Mechanical consequences:

```text
once
```

unless authored periodic remembrance elsewhere.

---

# 144. Workstream 212B — Emotional Responses

## Goal

Make discoveries meaningful without turning them into generic stat dispensers.

---

# 145. 212B Phase BK — Reaction Factors

Use:

- creator alive/dead,
- creator-reader relationship,
- recipient match,
- item significance,
- message tone,
- age,
- memorial context,
- creator fate,
- unresolved grief,
- surprise.

---

# 146. 212B Phase BL — Reaction Profiles

Examples:

```text
WarmConnection
GriefAndComfort
BittersweetMemory
HistoricalCuriosity
AngerAtThePast
UnexpectedHope
FamilyContinuity
MentorLegacy
```

---

# 147. 212B Phase BM — Morale

Small/bounded intent.

No automatic huge bonus from old objects.

---

# 148. 212B Phase BN — Grief

Message from deceased may:

- intensify,
- process,
- comfort,

depending canonical psychology/grief.

No guaranteed healing.

---

# 149. 212B Phase BO — Relationship

Reading a living survivor’s message can affect connection only if recipient/author relationship exists.

---

# 150. 212B Phase BP — Historical Curiosity

Could generate:

- archive interest,
- discovery quest,
- location/history clue

only if canonical systems support.

Do not grant free knowledge.

---

# 151. 212B Phase BQ — Reaction Explainability

Reason codes:

```text
message_from_deceased_friend
designated_recipient
family_heirloom
unexpected_discovery
old_shelter_record
mentor_keepsake
```

---

# 152. Workstream 212B — Memorial Integration

## Goal

Use capsules to enrich remembrance without duplicating graves/memorials.

---

# 153. 212B Phase BR — Memorial Association

A capsule may reference:

```text
memorial_id
```

---

# 154. 212B Phase BS — Memorial Capsule

Possible:

- sealed into memorial,
- stored nearby,
- opened on anniversary/event.

MemorialSystem owns memorial condition/state.

---

# 155. 212B Phase BT — Death Memorial Package

If survivor prepared capsule:

- death event may associate it with memorial.

No new contents generated.

---

# 156. 212B Phase BU — Memorial Reading

Opening can emit:

```text
memorial_revisited
```

through canonical memorial API if appropriate.

---

# 157. Workstream 212B — Events

## Goal

Preserve source event set as semantic narrative moments.

---

# 158. 212B Phase BV — Source Events

```text
The Capsule
The Discovery
The Message
The Opening
The Past
The Future
The Inheritance
The Memory
```

---

# 159. 212B Phase BW — The Capsule

Trigger:

- meaningful capsule sealed.

---

# 160. 212B Phase BX — The Discovery

Trigger:

- first discovery.

---

# 161. 212B Phase BY — The Message

Trigger:

- legacy message delivered.

---

# 162. 212B Phase BZ — The Opening

Trigger:

- capsule opened.

---

# 163. 212B Phase CA — The Past

Trigger:

- historically old message read.

Threshold data-driven.

---

# 164. 212B Phase CB — The Future

Trigger:

- survivor creates first/significant future-directed message.

---

# 165. 212B Phase CC — The Inheritance

Trigger:

- capsule transfers through Plan 206 inheritance.

---

# 166. 212B Phase CD — The Memory

Trigger:

- significant emotional response.

Do not emit for every minor read.

---

# 167. Workstream 212B — Quest / Achievement Hooks

## Goal

Preserve source goals without creating a second achievement/quest system or incentivizing spam.

---

# 168. 212B Phase CE — Ownership

If Plan 149 owns achievements:

- register criteria there.

If shared quest runtime owns quests:

- use it.

TimeCapsuleSystem emits semantic facts only.

---

# 169. 212B Phase CF — The Time Capsule

Source:

```text
create 5 time capsules
```

Count distinct sealed valid capsules.

Anti-spam:

- opening/destroying and recreating same template/content cannot duplicate credit if achievement rules forbid.

---

# 170. 212B Phase CG — The Archaeologist

Discover 10 capsules.

Must be genuinely distinct capsule IDs.

---

# 171. 212B Phase CH — The Messenger

Write 20 legacy messages.

Potential spam risk.

Prefer:

- 20 valid delivered/prepared messages,
- per-day/per-recipient novelty rules.

---

# 172. 212B Phase CI — The Historian

Read 15 historical messages.

Historical age threshold must be meaningful.

---

# 173. 212B Phase CJ — The Keeper

Maintain 5 unopened capsules for 100 days.

Use sealed-duration state.

Opening early removes eligibility.

---

# 174. 212B Phase CK — The Legacy

Have 3 capsules discovered after creator death.

Requires capsules authored before death.

---

# 175. 212B Phase CL — The Connection

Receive 5 messages from deceased survivors.

One message ID counts once.

---

# 176. Workstream 212B — UI

## Goal

Make capsule creation, discovery, waiting, opening, message delivery, and history understandable without revealing sealed contents prematurely.

---

# 177. 212B Phase CM — Capsule Panel

Sections:

```text
Drafts
Sealed / Hidden
Discovered
Ready to Open
Opened History
Legacy Messages
Discovery Log
```

---

# 178. 212B Phase CN — Hidden Capsule Visibility

Important:

Player may know a capsule exists because they created it.

The in-world survivor discoverer may not know.

UI can show:

```text
Known to player / hidden in-world
```

if the game allows omniscient management.

Do not imply every survivor knows location.

---

# 179. 212B Phase CO — Capsule Detail Before Open

Show only allowed metadata:

- creator,
- creation day,
- exterior name/label,
- location if player knows,
- opening condition,
- designated recipient,
- seal status.

Contents can show:

```text
3 sealed contents
```

without names if secrecy intended.

---

# 180. 212B Phase CP — Creator View

If creator obviously knows contents:

- player may see list in management UI.

But discovery recipient should not automatically know.

Separate:

```text
player management knowledge
recipient knowledge
```

where game architecture supports.

---

# 181. 212B Phase CQ — Creation UI

Steps:

```text
Choose Template
Choose Contents
Write/Select Message
Choose Recipient
Choose Open Condition
Choose Location
Review & Seal
```

---

# 182. 212B Phase CR — Condition UI

Provide natural-language summary.

No raw enum IDs.

---

# 183. 212B Phase CS — Item Custody Warning

Before seal:

```text
These items will be unavailable until the capsule is opened.
```

---

# 184. 212B Phase CT — Legacy Message Panel

Sections:

```text
Draft
Waiting
Delivered
Unread
Read History
```

---

# 185. 212B Phase CU — Discovery Log

Show:

- capsule,
- discoverer,
- day,
- discovery method,
- opened later?

---

# 186. 212B Phase CV — Notifications

Priority:

- capsule discovered,
- capsule eligible to open,
- legacy message delivered,
- designated recipient unavailable,
- capsule lost/damaged.

Do not notify daily countdown.

---

# 187. 212B Phase CW — Tutorial

First capsule creation explains:

- real items are sealed,
- discovery != opening,
- conditions,
- messages can outlive creator,
- capsules persist within current campaign.

---

# 188. 212B Phase CX — Tooltips

Hover + focus/details.

Plan 37/184 accessible.

---

# 189. 212B Phase CY — Large Text

2× font.

Long message text scrolls/reflows.

---

# 190. 212B Phase CZ — Screen Reader Semantics

Capsule list announces:

- name,
- status,
- creator,
- condition,
- eligibility.

Do not expose sealed contents if hidden.

---

# 191. 212B Phase DA — Reduced Cognitive Load

Compact view:

```text
Capsule
Status
Who/when
Next action
```

Details collapsible.

---

# 192. 212B Phase DB — Media Accessibility

Photo/drawing:

- authored alt description.

Recording:

- transcript/captions.

No critical information audio-only.

---

# 193. 212B Phase DC — Localization

System-authored messages/templates use keys.

Optional player free-text remains player-entered.

---

# 194. 212B Definition of Done

- [ ] 10+ templates,
- [ ] recommended 12-template corpus,
- [ ] real content binding,
- [ ] creation transaction,
- [ ] item custody,
- [ ] location selection,
- [ ] condition preview,
- [ ] discovery triggers,
- [ ] discovered-sealed state,
- [ ] opening transaction,
- [ ] legacy message scheduling,
- [ ] recipient/fallback policy,
- [ ] read tracking,
- [ ] emotional response profiles,
- [ ] memorial association,
- [ ] source events,
- [ ] source hooks,
- [ ] capsule/message/discovery UI,
- [ ] tutorial/tooltips,
- [ ] accessibility,
- [ ] localization.

---

# 195. Workstream 212C — MemorialSystem Integration

## Goal

Link deliberate future messages with remembrance while preserving one memorial authority.

---

# 196. 212C Phase A — `MemorialSystem` Adapter

Define:

```text
ITimeCapsuleMemorialPort
```

Operations:

```text
AssociateCapsule(memorialId, capsuleId)
NotifyCapsuleOpened(...)
GetMemorialContext(...)
```

---

# 197. 212C Phase B — No Memorial Duplication

Do not store:

- burial details,
- memorial maintenance,
- mourning progression

inside capsule state.

---

# 198. 212C Phase C — Memorial Item

If a capsule itself becomes a memorial object:

- memorial records association,
- capsule retains delayed-access state.

---

# 199. Workstream 212C — Survivor Fate Integration

## Goal

Make death-triggered delivery reliable and idempotent.

---

# 200. 212C Phase D — Canonical Death Event

Use:

```text
SurvivorDied(author_id)
```

from `SurvivorFateSystem`/Plan 206 pipeline.

---

# 201. 212C Phase E — Prepared Death Messages

Index waiting messages by:

```text
author_id + OnDeath
```

On death:

- mark deliverable,
- resolve recipient,
- deliver once.

---

# 202. 212C Phase F — Prepared Capsules

Death can:

- transfer estate custody,
- associate memorial,
- enable condition if open condition is death-event-based.

No auto-created authored content.

---

# 203. 212C Phase G — Death Idempotency

Same death event cannot:

- deliver same message twice,
- inherit same capsule twice,
- trigger duplicate reactions.

---

# 204. Workstream 212C — Clock Integration

## Goal

Resolve temporal conditions through one simulation clock.

---

# 205. 212C Phase H — `ISimClock`

Use canonical day.

No wall clock.

---

# 206. 212C Phase I — Date Index

Index date-based:

```text
open conditions
message delivery conditions
```

by target day.

No scan of all history.

---

# 207. 212C Phase J — Save After Deadline

If a save loads after target day but condition was never processed due to older version/interruption:

- reconciliation processes exactly once.

---

# 208. 212C Phase K — Time Skip

Large time advances must process due entries deterministically.

---

# 209. Workstream 212C — EventSystem Integration

## Goal

Use semantic event conditions without replaying event history.

---

# 210. 212C Phase L — Event Predicate Registry

Define:

```text
ICapsuleEventConditionMatcher
```

Maps condition types to semantic events.

---

# 211. 212C Phase M — Stable Event Kinds

Use Plan 31 governed vocabulary.

No prose parsing.

---

# 212. 212C Phase N — Subject Predicates

Examples:

```text
leader_changed
winter_ended
specific_survivor_reached_adulthood
shelter_upgrade_completed
faction_war_ended
```

Use stable context IDs.

---

# 213. 212C Phase O — Event Replay Guard

Persist:

```text
condition satisfaction ID
```

Old event replay after load cannot reopen/deliver twice.

---

# 214. Workstream 212C — PersonalBelongingsSystem Integration

## Goal

Keep sentimental ownership and item provenance canonical.

---

# 215. 212C Phase P — Plan 210 Item Ownership

Before sealing:

- verify creator owns/is authorized to transfer item.

---

# 216. 212C Phase Q — Belonging Meaning

Capsule content can reference:

```text
belonging provenance
sentimental tags
owner history
```

without copying them.

---

# 217. 212C Phase R — Opening Belonging Transfer

On open:

- belonging may pass to intended heir/reader,
- Plan 210 decides new personal ownership/provenance.

---

# 218. 212C Phase S — Item History

Item provenance can record:

```text
sealed_in_capsule
opened_by
inherited
```

through canonical item history if supported.

---

# 219. Workstream 212C — DeathLegacySystem Integration

## Goal

Treat sealed capsules as estate objects without duplicating inheritance.

---

# 220. 212C Phase T — Estate Enumeration

Plan 206 can query:

```text
sealed capsules associated with deceased
```

---

# 221. 212C Phase U — Inheritance Outcomes

Possible:

- heir receives sealed capsule custody,
- shelter archive takes custody,
- memorial holds capsule,
- capsule remains hidden.

Policy-driven.

---

# 222. 212C Phase V — Inheritance Does Not Open

Hard invariant.

Transfer of custody ≠ opening.

---

# 223. 212C Phase W — Designated Recipient vs Estate Heir

If capsule explicitly designates recipient:

- inheritance policy should honor designation where valid,
- otherwise resolve conflict explicitly.

No silent override.

---

# 224. Workstream 212C — Shelter Archive Integration

## Goal

Keep historical meaning without turning the capsule system into a second archive.

---

# 225. 212C Phase X — Archive References

Capsule may include:

```text
archive entry reference
```

as content.

---

# 226. 212C Phase Y — Capsule Opening → Archive

Landmark discovery may emit:

```text
ArchiveLandmarkIntent
```

---

# 227. 212C Phase Z — Archive Significance

Derived from:

- age,
- creator importance,
- historical event link,
- content significance,
- rarity.

Not every capsule archived.

---

# 228. 212C Phase AA — Cross-Campaign Boundary

Plan 162/140 may later export a summary.

C2[43] does not persist capsule objects across playthroughs.

---

# 229. Workstream 212C — Leadership Integration

## Goal

Support `shelter_leader` recipient policy dynamically.

---

# 230. 212C Phase AB — Current Leader Resolution

At delivery/opening:

```text
LeadershipSystem.CurrentLeader
```

or canonical interface.

---

# 231. 212C Phase AC — Vacancy

If no current leader:

- message remains deliverable,
- deliver when leader installed,
- or route to acting leader if policy explicitly permits.

---

# 232. 212C Phase AD — Leader Changes Before Reading

If delivered to named office/inbox:

- define whether next leader inherits unread message.

Recommended:

```text
office-recipient message remains for office
```

if recipient type was ShelterLeader.

---

# 233. Workstream 212C — Lineage / Generation Integration

## Goal

Implement `next_generation` without guessing.

---

# 234. 212C Phase AE — Generation Authority

Use Cohort/GenerationalLineage if landed.

Query:

```text
GetGeneration(survivor)
GetNextEligibleGeneration(author)
```

---

# 235. 212C Phase AF — Recipient Selection

Within next generation:

- designated descendant if specified,
- otherwise first eligible adult/representative according to policy.

Use deterministic ordering.

---

# 236. 212C Phase AG — No Eligible Next Generation

Message/capsule remains waiting.

May fallback to archive/anyone only if authored.

---

# 237. Workstream 212C — Morale / Relations / Psychology

## Goal

Deliver emotional consequences through canonical owners and avoid generic “old thing = morale buff.”

---

# 238. 212C Phase AH — Morale Port

Use reason-coded:

```text
legacy_message_read
time_capsule_discovery
memorial_capsule_opened
```

---

# 239. 212C Phase AI — Relations Port

If creator alive:

- message can affect relation to reader.

If creator dead:

- relation history/memory can still contextualize but no live relationship mutation if system disallows it.

---

# 240. 212C Phase AJ — Psychology/Grief Port

Message from deceased may emit:

- grief reminder,
- comfort,
- guilt,
- closure opportunity.

Owner resolves.

---

# 241. 212C Phase AK — No Guaranteed Healing

Reading an old message does not automatically “resolve grief.”

---

# 242. Workstream 212C — Save / Load / Idempotency

## Goal

Prove all delayed triggers and custody transitions survive arbitrary save points exactly.

---

# 243. 212C Phase AL — Save Matrix

Test:

```text
draft capsule
seal transaction pending
sealed hidden
date condition pending
date condition satisfied but undiscovered
discovered sealed
eligible to open
opening transaction pending
opened
message waiting
message deliverable
message delivered unread
message read
creator died
inheritance pending
capsule inherited sealed
```

---

# 244. 212C Phase AM — Seal Idempotency

Physical item moves into custody once.

---

# 245. 212C Phase AN — Discovery Idempotency

First discovery commits once.

---

# 246. 212C Phase AO — Opening Idempotency

Opening items/effects once.

---

# 247. 212C Phase AP — Message Delivery Idempotency

One delivery per message.

---

# 248. 212C Phase AQ — Message Read Idempotency

One mechanical read consequence.

---

# 249. 212C Phase AR — Death Trigger Idempotency

One on-death transition.

---

# 250. 212C Phase AS — Event Condition Idempotency

One satisfaction record.

---

# 251. 212C Phase AT — Inheritance Idempotency

One custody transfer per estate outcome.

---

# 252. 212C Phase AU — RNG Outcome Commitment

Any accident/reaction roll committed before side effects.

---

# 253. Workstream 212C — Exploit Prevention

## Goal

Prevent capsule creation, item custody, discovery, emotional response, and quest-hook farming.

---

# 254. 212C Phase AV — Item Duplication Exploit

Critical gate:

```text
seal item
save
open/reload/cancel
```

must never duplicate canonical instance.

---

# 255. 212C Phase AW — Item Protection Exploit

Player cannot use capsule to:

- freeze decay if policy does not allow,
- evade theft/raid risk without cost,
- hide quest-critical items from systems incorrectly.

Document custody effects.

---

# 256. 212C Phase AX — Unlimited Secure Storage Exploit

If hidden capsules protect items from raids:

- this becomes gameplay storage and must have capacity/risk/cost.

Otherwise raids should be able to affect capsule location appropriately.

---

# 257. 212C Phase AY — Creation Spam

Maximum active unopened capsules + repeat policies.

No free reward merely for creating empty capsules.

---

# 258. 212C Phase AZ — Empty Capsule

Allowed for message-only design if template supports.

Achievement criteria may require meaningful content.

---

# 259. 212C Phase BA — Create/Delete Farming

Deleting/cancelling drafts yields no credit.

Sealed capsule achievements keyed by stable capsule ID.

---

# 260. 212C Phase BB — Open/Reseal Farming

Opened capsule cannot be reset to unopened.

New capsule = new object.

Repeated same content may be novelty-limited for rewards.

---

# 261. 212C Phase BC — Search Farming

Repeated search of unchanged location:

- no repeated random rolls every UI click.

Use search action ID/day/cooldown.

---

# 262. 212C Phase BD — Emotional Reward Farming

Same capsule/read reaction applies once.

Re-reading no repeated morale.

---

# 263. 212C Phase BE — Death Message Farming

One prepared message cannot deliver repeatedly after restore.

---

# 264. 212C Phase BF — Leader Recipient Farming

Repeated leader changes do not redeliver already delivered message.

---

# 265. 212C Phase BG — Event Trigger Farming

Repeated semantic event of same kind:

- first matching event satisfies condition once.

---

# 266. Workstream 212C — Edge Cases

## Goal

Make long-lived temporal objects robust to death, roster turnover, destruction, and changing shelter state.

---

# 267. 212C Phase BH — No Capsules

Current game behavior remains valid.

---

# 268. 212C Phase BI — Many Capsules

Stress:

```text
100 active
1000 historical
```

if config permits.

UI filtering/retention required.

---

# 269. 212C Phase BJ — Creator Dies Before Sealing

Draft becomes:

- cancelled,
- inherited draft if explicitly supported.

Do not auto-complete unfinished message.

---

# 270. 212C Phase BK — Recipient Dies Before Opening

Policy path.

No dangling UI.

---

# 271. 212C Phase BL — Creator and Recipient Die Same Event

Death processing order deterministic.

Message fallback policy resolves once.

---

# 272. 212C Phase BM — Location Destroyed

Capsule:

- exposed,
- damaged,
- destroyed,
- transferred to debris/storage,

through shelter owner.

---

# 273. 212C Phase BN — Location Removed During Renovation

Must migrate custody/location.

Do not orphan capsule.

---

# 274. 212C Phase BO — Capsule Destroyed

Physical contents:

- destroyed/damaged through canonical item system.

No recovery from capsule history.

---

# 275. 212C Phase BP — Event Never Occurs

Capsule can remain sealed indefinitely.

This is valid.

---

# 276. 212C Phase BQ — Date Beyond Campaign End

Allowed if creator intentionally chooses.

Campaign may end with unopened capsule.

Archive/epilogue may mention it.

---

# 277. 212C Phase BR — Manual Capsule Never Found

Cannot be opened through omniscient UI unless player-management knowledge allows explicit retrieval by location.

Define player knowledge policy.

---

# 278. 212C Phase BS — Specific Survivor Leaves Shelter

Message waits/redirects according to policy.

---

# 279. 212C Phase BT — Specific Survivor Returns

Delivery/open eligibility can resume.

---

# 280. 212C Phase BU — No Leader

Leader-recipient message waits.

---

# 281. 212C Phase BV — No Next Generation

Next-generation message remains waiting.

---

# 282. 212C Phase BW — Media Asset Missing

Fallback to text description/transcript.

No broken UI.

---

# 283. Workstream 212C — Determinism

## Goal

Make delayed temporal behavior exactly reproducible.

---

# 284. 212C Phase BX — Stable Ordering

Sort:

- due date conditions,
- event-matched capsules,
- candidate recipients,
- discovery candidates

by stable IDs.

---

# 285. 212C Phase BY — Same-Seed Digest

Build:

```text
time_capsule_history_digest
```

containing:

- capsule IDs/status,
- discovery IDs,
- opening IDs,
- message delivery/read IDs,
- custody refs,
- reaction IDs.

---

# 286. 212C Phase BZ — Same Inputs

Same:

```text
campaign seed
capsule creation choices
clock
events
deaths
location interactions
recipient state
```

→ same history.

---

# 287. Workstream 212C — Data Integrity

## Goal

Reject invalid temporal objects and content references before runtime corruption.

---

# 288. 212C Phase CA — Template Schema Validation

Validate:

- unique IDs,
- content types,
- condition types,
- recipient policies,
- reaction profiles,
- location tags,
- localization keys.

---

# 289. 212C Phase CB — Runtime Reference Validation

Validate:

- creator,
- recipient,
- item instance,
- location,
- memorial,
- event predicate,
- archive refs.

Missing runtime refs route to migration/error policy.

---

# 290. 212C Phase CC — Condition Union Validation

Exactly one condition variant.

No:

```text
date + survivor + event simultaneously
```

in baseline.

---

# 291. 212C Phase CD — Physical Custody Validation

Every sealed physical item:

```text
exactly one canonical custody owner
```

---

# 292. 212C Phase CE — Opened Item Validation

Every transferred item:

```text
not still in sealed custody
```

---

# 293. 212C Phase CF — Delivery Validation

Delivered message has:

- delivered day,
- recipient,
- status.

---

# 294. 212C Phase CG — Read Validation

Read message must already be delivered.

---

# 295. 212C Phase CH — Discovery Validation

Opened hidden capsule must have:

- discovery/access justification

unless creator retrieved a known capsule under explicit policy.

---

# 296. Workstream 212C — `--time-capsule-selftest`

Required scenarios:

1. create message-only capsule,
2. create physical-item capsule,
3. seal custody,
4. date condition,
5. survivor condition,
6. event condition,
7. manual condition,
8. discovered before eligible,
9. eligible before discovered,
10. accidental discovery,
11. active search discovery,
12. inherited discovery,
13. designated-recipient discovery,
14. open capsule,
15. transfer physical contents,
16. storage-capacity edge,
17. prepare legacy message,
18. immediate delivery,
19. date delivery,
20. event delivery,
21. on-death delivery,
22. delivered unread,
23. read message,
24. deceased-author reaction,
25. living-author reaction,
26. memorial association,
27. Plan 210 belonging custody,
28. Plan 206 inheritance,
29. leader recipient,
30. no leader,
31. next-generation recipient,
32. no next generation,
33. creator death before seal,
34. recipient death,
35. location destroyed,
36. save/load mid-seal,
37. save/load mid-opening,
38. duplicate-effect replay,
39. old save,
40. no-capsule campaign.

---

# 297. Workstream 212C — Deliberate Failure Proof

Break:

- nonexistent creator,
- invalid recipient,
- invalid item instance,
- duplicate item custody,
- invalid location,
- multiple opening-condition variants,
- invalid event kind,
- read-before-delivery,
- second opening,
- duplicate message delivery.

Assert the correct gate fails.

---

# 298. Workstream 212C — Long-Horizon Soak

## Goal

Prove the temporal layer remains meaningful over 100–400+ campaign days and through generation turnover.

---

# 299. 212C Phase CI — 200-Day Capsule Soak

Profiles:

```text
minimal_use
family_legacy
memorial_heavy
archive_heavy
many_messages
many_physical_capsules
```

Record:

```text
capsules created
discovered
opened
still sealed
messages prepared
delivered
read
item transfers
reactions
notifications
```

---

# 300. 212C Phase CJ — Multi-Generation Soak

If lineage exists:

- creator generation dies/ages,
- next generation receives message,
- inherited capsule opens,
- item provenance survives.

---

# 301. 212C Phase CK — 400-Day Storage Soak

Measure:

- active condition indexes,
- state size,
- historical records,
- custody records,
- archive rollups.

---

# 302. 212C Phase CL — Discovery Rate

Hidden capsules should not all be found immediately.

Compare:

- visible archive capsule,
- wall cache,
- memorial cache,
- actively searched cache.

---

# 303. 212C Phase CM — Emotional Frequency

Discoveries should feel significant.

Limit:

- repeated reaction notifications,
- repeated minor morale effects.

---

# 304. 212C Phase CN — Physical-Item Value

Player should have a real tradeoff:

```text
use valuable item now
vs
seal it for future
```

Do not make sentimental reward always exceed practical value.

---

# 305. 212C Phase CO — Novelty Risk

The source risk correctly notes novelty.

Mitigation:

- bind capsules to real survivor relationships/events/items,
- meaningful delay,
- real item opportunity cost,
- archive/memorial continuity,
- occasional discovery surprise.

---

# 306. 212C Phase CP — Template Distribution

Ensure usage includes:

- all four opening conditions,
- multiple recipient types,
- physical and nonphysical content,
- living/deceased creator outcomes.

---

# 307. Workstream 212C — Notification / Attention Budget

## Goal

Prevent long campaigns from becoming a stream of “capsule ready” alerts.

---

# 308. 212C Phase CQ — Notification Priorities

High:

- newly discovered historical capsule,
- message from recently deceased loved one,
- designated capsule ready.

Medium:

- date condition reached,
- inherited capsule.

Low:

- routine reminder.

---

# 309. 212C Phase CR — Grouped Notifications

If multiple date capsules become eligible same day:

```text
3 time capsules are now eligible to open.
```

---

# 310. 212C Phase CS — No Daily Reminders

Do not remind every day that a capsule remains unopened.

---

# 311. Workstream 212C — Performance

## Goal

Use indexes and event subscriptions rather than scanning every capsule every tick.

---

# 312. 212C Phase CT — Date Index

Map:

```text
target_day → capsule/message IDs
```

---

# 313. 212C Phase CU — Event Index

Map:

```text
semantic_event_kind → condition IDs
```

---

# 314. 212C Phase CV — Survivor Index

Map:

```text
survivor_id → designated capsule/message IDs
```

---

# 315. 212C Phase CW — Location Index

Map:

```text
location_id → hidden capsule IDs
```

used on search/maintenance.

---

# 316. 212C Phase CX — No Per-Frame Work

Hard gate.

---

# 317. 212C Phase CY — Save Size

Do not persist rendered localized message strings if template/key + parameters suffice.

Player-entered free text is exception.

---

# 318. Workstream 212C — Retention

## Goal

Preserve long-term meaning while controlling historical growth.

---

# 319. 212C Phase CZ — Plan 55 Retention

Keep fully:

- unopened capsules,
- undelivered/unread messages,
- physical custody,
- recent openings,
- landmark historical capsules.

Roll up:

- old routine discovery logs,
- repeated non-landmark reaction details.

---

# 320. 212C Phase DA — Opened Capsule Summary

Long-term summary:

```text
capsule ID
creator
opened day
opener
major content refs
archive significance
```

---

# 321. 212C Phase DB — Message Summary

Old read messages can retain:

- author,
- recipient,
- delivery/read day,
- template/content key,
- landmark flag.

---

# 322. 212C Phase DC — Free-Text Retention

Never silently discard player-authored text while the capsule/message remains user-visible.

If retention needs compaction:

- preserve it or explicitly archive/export.

---

# 323. Workstream 212C — Accessibility

## Goal

Make temporal communication readable and usable under Plan 184.

---

# 324. 212C Phase DD — Font Scaling

Long letters reflow at 2× text.

---

# 325. 212C Phase DE — Screen Reader

Letter/message content exposed semantically.

---

# 326. 212C Phase DF — Recording Transcript

Required for audio recording content.

---

# 327. 212C Phase DG — Photo/Drawing Description

Alt-description metadata.

---

# 328. 212C Phase DH — No Color-Only Seal State

Use:

```text
Sealed
Ready
Opened
```

labels/icons.

---

# 329. 212C Phase DI — Controller / Keyboard

All creation/open/read flows reachable without mouse.

---

# 330. Workstream 212C — Human Narrative Review

## Goal

Ensure capsules feel like communication across time rather than achievement containers.

---

# 331. 212C Phase DJ — Review Questions

For each template:

```text
Why would this survivor make this?
Why now?
Why this recipient?
Why this condition?
Why these contents?
What does waiting add?
Would discovery still matter if no mechanical reward existed?
```

---

# 332. 212C Phase DK — Swap Test

Swap creator with unrelated survivor.

If capsule still feels identical:

- add stronger contextual binding.

---

# 333. 212C Phase DL — Delay Test

If opening tomorrow feels identical to opening 100 days later:

- template does not exploit temporal mechanic enough.

---

# 334. 212C Phase DM — Death Test

A pre-authored message read after author death should feel changed by context.

Do not rewrite the message itself.

---

# 335. 212C Phase DN — Cross-Generation Test

A descendant/next-generation survivor should receive enough provenance to understand:

- who wrote it,
- when,
- why.

---

# 336. Documentation

Create:

```text
docs/systems/TIME_CAPSULES_AND_LEGACY_MESSAGES.md
```

Include:

- ownership boundaries,
- lifecycle,
- discovery/open separation,
- condition semantics,
- physical custody,
- legacy messages,
- death/inheritance,
- reactions,
- save/idempotency,
- retention,
- content authoring.

---

# 337. Content Authoring Guide

Create:

```text
docs/content/TIME_CAPSULE_TEMPLATE_AUTHORING.md
```

Checklist:

```text
1. identify creator context
2. identify recipient
3. choose temporal opening/delivery condition
4. choose real contents
5. define location/discovery context
6. define reaction profiles
7. define fallback on death/missing recipient
8. define memorial/archive significance
9. add localization/accessibility metadata
10. add deterministic test fixture
```

---

# 338. Integrated Time-Capsule Pipeline

```text
survivor + real items/message
           │
           ▼
       Draft Capsule
           │
           ▼
    validate + seal
           │
           ├─ canonical item custody
           ├─ location binding
           └─ opening condition
           │
           ▼
        Hidden/Sealed
           │
     ┌─────┴─────────┐
     ▼               ▼
 condition met     discovered
     │               │
     └──────┬────────┘
            ▼
     Discovered + Eligible
            │
            ▼
          Open
            │
     ┌──────┼───────────────┐
     ▼      ▼               ▼
 item transfer   message/read   emotional intent
     │              │               │
     ▼              ▼               ▼
 Inventory      UI/Journal    Morale/Relations/
 Belongings      Archive        Psychology
```

---

# 339. Capsule Authority Contract

TimeCapsuleSystem owns:

- capsule identity,
- seal/open condition,
- discovery state,
- opening state,
- temporal-delivery association,
- message delivery/read tracking.

---

# 340. Inventory Authority Contract

Inventory owns:

- physical item instances,
- quantities,
- condition,
- canonical custody.

Capsule system references/transfers.

---

# 341. Belongings Authority Contract

Plan 210 owns:

- sentimental/personal ownership,
- item provenance.

Capsule system delays access.

---

# 342. Death Authority Contract

SurvivorFate/Plan 206 owns:

- death,
- estate/inheritance.

Capsule system reacts.

---

# 343. Memorial Authority Contract

MemorialSystem owns:

- memorial identity,
- remembrance mechanics.

Capsules may attach.

---

# 344. Archive Authority Contract

Plan 162 owns shelter history/archive.

Capsules may include/reference archive records and emit archive landmarks.

---

# 345. Cross-Campaign Contract

Plan 140 owns cross-playthrough legacy.

Plan 212 is in-campaign only.

---

# 346. Clock Authority Contract

`ISimClock` owns time.

No wall clock.

---

# 347. Event Authority Contract

EventSystem/Plan 31 owns semantic events.

Capsules subscribe.

---

# 348. Discovery Contract

Discovery reveals existence/access.

It never implies opening.

---

# 349. Opening Contract

Opening reveals sealed content and transfers items once.

---

# 350. Message Delivery Contract

Delivery places a message with intended recipient/channel.

It does not imply reading.

---

# 351. Read Contract

Read status commits once for mechanical consequences.

Re-reading remains presentation-only.

---

# 352. Reaction Contract

Emotional effects are consequence intents.

No capsule-local morale/grief ledger.

---

# 353. Recipient Contract

Dynamic recipient types resolve at delivery/opening time.

---

# 354. Condition Contract

Exactly one baseline open/delivery condition variant.

---

# 355. Save Contract

Persist:

- capsule lifecycle,
- message lifecycle,
- discoveries,
- condition satisfaction,
- canonical custody refs,
- idempotency.

Do not copy inventory, memorial, fate, archive, or relationship truth.

---

# 356. Old-Save Contract

Old saves begin with no capsule/message history.

No retroactive legacy fabrication.

---

# 357. Determinism Contract

Same:

```text
seed
capsule choices
clock
events
survivor state
location interactions
```

→ same capsule history.

---

# 358. Retention Contract

Unopened/unread content remains fully preserved.

Historical routine logs may roll up.

---

# 359. Narrative Truth Contract

Messages/templates may assert only canonical facts or explicit player-authored text.

---

# 360. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| physical items duplicated in capsule | Medium | Critical | custody transaction |
| discovery automatically opens capsule | High | High | separate lifecycle states |
| death auto-creates fake authored messages | High | High | prepared-only death delivery |
| capsule duplicates inheritance | Medium | High | Plan 206 handoff |
| capsule duplicates archive | Medium | Medium | structured archive intents only |
| hidden capsules become infinite secure storage | Medium | High | location/capacity/risk policy |
| sentimental value becomes exploit reward | Medium | Medium | narrative weight only |
| repeated searches farm discovery RNG | High | High | action/day idempotency |
| re-reading farms morale | High | High | read consequence once |
| event replay redelivers messages | Medium | Critical | condition satisfaction IDs |
| specific recipient dies | High | Medium | explicit fallback policy |
| many capsules flood notifications | Medium | Medium | grouping/attention budget |
| next-generation recipient guessed incorrectly | Medium | High | lineage adapter required |
| old saves fabricate past capsules | Medium | High | no backfill |
| long campaigns bloat save | Medium | Medium | Plan 55 retention/indexing |

---

# 361. Commit Strategy

## 212A — Foundation

### C2[43].1 — baseline + ownership ADR

### C2[43].2 — capsule/message/discovery lifecycle DTOs

### C2[43].3 — opening-condition union / indexes

### C2[43].4 — inventory custody adapter

### C2[43].5 — seal/open atomic transactions

### C2[43].6 — legacy-message delivery/read state

### C2[43].7 — reaction assessment/consequence ports

### C2[43].8 — deterministic RNG/idempotency

### C2[43].9 — save migration / old-save no-backfill

### C2[43].10 — composition/events/diagnostics

### Gate: 212A complete

---

## 212B — Content / Runtime / UI

### C2[43].11 — 12 capsule templates

### C2[43].12 — creation UI / item selection

### C2[43].13 — location/discovery model

### C2[43].14 — date/survivor/event/manual conditions

### C2[43].15 — opening / content transfer

### C2[43].16 — legacy message flows

### C2[43].17 — emotional response profiles

### C2[43].18 — memorial capsule integration

### C2[43].19 — events / quest hooks

### C2[43].20 — capsule/message/history UI

### C2[43].21 — tutorial/tooltips/accessibility/localization

### C2[43].22 — content utilization/narrative review

### Gate: 212B complete

---

## 212C — Integration / Validation

### C2[43].23 — MemorialSystem

### C2[43].24 — SurvivorFate / DeathLegacy

### C2[43].25 — ISimClock / EventSystem

### C2[43].26 — PersonalBelongings / Inventory

### C2[43].27 — Shelter Archive / Leadership / Lineage

### C2[43].28 — morale/relations/psychology

### C2[43].29 — save-load/idempotency matrix

### C2[43].30 — exploit prevention

### C2[43].31 — edge cases

### C2[43].32 — data-integrity/custody gates

### C2[43].33 — `--time-capsule-selftest`

### C2[43].34 — deliberate failure proof

### C2[43].35 — 200-day + multi-generation soak

### C2[43].36 — 400-day retention/performance soak

### C2[43].37 — notification/value balance

### C2[43].38 — narrative playtest/docs/release closure

### Gate: 212C complete

---

# 362. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --time-capsule-selftest
```

Also run repository-canonical equivalents of:

```text
capsule item-custody integrity gate
discovery/open lifecycle gate
message delivery/read lifecycle gate
old-save no-backfill fixture
event-condition replay-idempotency test
death-delivery prepared-content test
inheritance-custody integration test
same-seed time-capsule digest replay
many-capsule attention/performance test
2× text / media accessibility test
```

---

# 363. `--time-capsule-selftest` Acceptance Matrix

| Scenario | Expected |
|---|---|
| Draft | no item duplication |
| Seal | physical custody committed |
| Date condition | clock-owned |
| Survivor condition | designated survivor required |
| Event condition | semantic event |
| Manual condition | explicit open action |
| Discovery before eligible | stays sealed |
| Eligible before discovery | waits hidden |
| Accident | deterministic + location activity |
| Search | bounded action |
| Inheritance | custody transfers, remains sealed |
| Opening | exactly once |
| Item transfer | canonical instance |
| Message delivery | exactly once |
| Message read | exactly once |
| On death | only pre-authored message |
| Memorial | association only |
| Next generation | canonical lineage |
| Shelter leader | current leader at delivery |
| Recipient death | fallback policy |
| Location destruction | no orphaned custody |
| Save/load | exact lifecycle |
| Old save | empty state |
| No capsules | valid |
| Many capsules | bounded |

---

# 364. Flagship Definition of Done — Foundation

- [ ] `TimeCapsuleSystem.cs`,
- [ ] capsule lifecycle,
- [ ] legacy-message lifecycle,
- [ ] discovery records,
- [ ] 4 opening conditions,
- [ ] recipient policies,
- [ ] discovery/open separation,
- [ ] delivery/read separation,
- [ ] physical item custody,
- [ ] deterministic reactions,
- [ ] save/versioning,
- [ ] old-save no-backfill,
- [ ] event-driven processing,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 365. Flagship Definition of Done — Runtime / Content / UI

- [ ] capsule creation,
- [ ] physical contents,
- [ ] letters/photos/recordings/drawings/artifacts representation,
- [ ] shelter location,
- [ ] date opening,
- [ ] survivor opening,
- [ ] event opening,
- [ ] manual opening,
- [ ] accidental discovery,
- [ ] searching discovery,
- [ ] inherited discovery,
- [ ] designated discovery,
- [ ] legacy messages,
- [ ] delivery conditions,
- [ ] read tracking,
- [ ] emotional response,
- [ ] 10+ templates,
- [ ] recommended 12 templates,
- [ ] source events,
- [ ] source hooks,
- [ ] capsule panel,
- [ ] capsule detail,
- [ ] message panel,
- [ ] discovery log,
- [ ] notifications,
- [ ] tutorial/tooltips,
- [ ] accessibility/localization.

---

# 366. Flagship Definition of Done — Integration / Validation

- [ ] MemorialSystem,
- [ ] SurvivorFateSystem,
- [ ] ISimClock,
- [ ] EventSystem,
- [ ] PersonalBelongingsSystem,
- [ ] DeathLegacySystem,
- [ ] Inventory,
- [ ] Shelter Archive,
- [ ] Leadership dynamic recipient,
- [ ] Lineage/next-generation recipient,
- [ ] morale/relations/psychology,
- [ ] save/load lifecycle matrix,
- [ ] item/discovery/open/message idempotency,
- [ ] anti-duplication,
- [ ] anti-storage exploit,
- [ ] anti-search/reaction/message farming,
- [ ] no/many capsule edge cases,
- [ ] creator/recipient death edges,
- [ ] location destruction,
- [ ] no leader/no next generation,
- [ ] same-seed replay,
- [ ] data-integrity/custody validation,
- [ ] deliberate failure fixtures,
- [ ] selftest,
- [ ] 200-day soak,
- [ ] multi-generation soak,
- [ ] 400-day retention test,
- [ ] attention/value balance,
- [ ] accessibility,
- [ ] performance,
- [ ] narrative playtest,
- [ ] docs.

---

# 367. Global Definition of Done

- [ ] no duplicate inventory state,
- [ ] no duplicate belonging state,
- [ ] no duplicate inheritance state,
- [ ] no duplicate memorial state,
- [ ] no duplicate shelter archive,
- [ ] no cross-campaign legacy leakage,
- [ ] no posthumous fabricated authorship,
- [ ] no discovery=open shortcut,
- [ ] no delivery=read shortcut,
- [ ] no duplicate item transfer,
- [ ] no rerolling discovery/reaction,
- [ ] no endless notification flood,
- [ ] no player-hidden data leaked through accessibility semantics,
- [ ] full verification green.

---

# 368. Closure Report Template

```markdown
## C2[43] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Memorial owner:
- Survivor fate/death owner:
- Clock:
- Event vocabulary:
- Inventory/custody owner:
- Personal belongings:
- Death inheritance:
- Archive:
- Lineage:
- Leadership:
- Existing capsule/message matches:

### 212A — Foundation
- TimeCapsuleSystem:
- Capsule states:
- Message states:
- Discovery/open separation:
- Conditions:
- Recipient policies:
- Custody port:
- Reaction model:
- RNG stream:
- Save schema:
- Old-save behavior:
- Missing ports:
- Result:

### 212B — Runtime / Content
- Template count:
- Physical capsules:
- Message-only capsules:
- Date conditions:
- Survivor conditions:
- Event conditions:
- Manual conditions:
- Accidental discoveries:
- Search discoveries:
- Inherited discoveries:
- Designated discoveries:
- Messages delivered:
- Messages read:
- UI:
- Unused templates:
- Result:

### 212C — Integration
- Memorial:
- Fate:
- Clock:
- EventSystem:
- Belongings:
- DeathLegacy:
- Inventory:
- Archive:
- Leadership:
- Lineage:
- Morale/relations/psychology:
- Duplicate custody:
- Duplicate opening:
- Duplicate delivery:
- Event replay duplicates:
- Result:

### Long-Horizon / Balance
- 200-day capsules:
- opened:
- unopened:
- average age at opening:
- notifications:
- item value sealed:
- reactions:
- multi-generation deliveries:
- 400-day state size:
- retention rollups:
- novelty findings:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Time capsule selftest:
- Custody integrity:
- Lifecycle gate:
- Old-save fixture:
- Event replay:
- Death delivery:
- Inheritance:
- Same-seed digest:
- Many-capsule stress:
- Accessibility:
- Result:

### Final Metrics
- TIME_CAPSULES_CREATED:
- TIME_CAPSULES_ACTIVE:
- TIME_CAPSULES_DISCOVERED:
- TIME_CAPSULES_OPENED:
- LEGACY_MESSAGES_PREPARED:
- LEGACY_MESSAGES_DELIVERED:
- LEGACY_MESSAGES_READ:
- CAPSULE_ITEMS_IN_CUSTODY:
- DUPLICATE_ITEM_CUSTODY_ERRORS:
- DUPLICATE_OPENING_ERRORS:
- DUPLICATE_MESSAGE_DELIVERY_ERRORS:
- EVENT_CONDITION_REPLAY_ERRORS:
- POSTHUMOUS_AUTHORSHIP_VIOLATIONS:
- ORPHANED_CAPSULE_ITEM_REFS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Capsule content:
- Media assets:
- Lineage integration:
- Shelter-location discovery:
- Memorial depth:
- UI:
```

---

# 369. Final Execution Directive

Execute Plan 212 as a **delayed-access communication and custody layer over the existing item, belonging, death, inheritance, memorial, archive, clock, event, relationship, and generation systems**.

The critical sequence is:

```text
audit canonical inventory/belonging/death/archive owners
→ define capsule/message lifecycle states
→ separate discovery from opening
→ define date/survivor/event/manual condition union
→ move physical items into canonical sealed custody
→ persist creator/recipient/location/provenance
→ resolve discoveries only from real location/inheritance/designated events
→ open exactly once when eligible
→ transfer the same physical item instances
→ deliver/read legacy messages independently
→ route emotional effects through canonical morale/relations/psychology
→ preserve landmark history through memorial/archive
→ validate long-horizon deterministic behavior and retention
```

Do not copy items into capsule DTOs.

Do not let inheritance automatically open a capsule.

Do not create a dead survivor’s message after death unless it was explicitly prepared while alive.

Do not turn sentimental value into a generic reward meter.

Do not equate discovery with opening.

Do not equate delivery with reading.

Do not export these in-campaign capsule objects into Plan 140 cross-campaign progression without a separate future bridge.

The strongest authority rule is:

> **TimeCapsuleSystem owns temporal access—who created the package, where it waits, when it becomes eligible, who discovered it, when it opened, and when a message was delivered/read—while every physical item, death, inheritance, memorial, archive entry, relationship, and emotional state remains owned by its canonical system.**

The strongest temporal rule is:

> **A capsule can exist, be hidden, become eligible, be discovered, and be opened at different times; those are separate facts and must survive save/load independently.**

The strongest physical rule is:

> **A physical belonging placed into a time capsule is the same canonical item instance before, during, and after its sealed custody. No copy is created merely because the item appears in capsule content.**

The flagship acceptance scenario is:

> **Have a living survivor prepare a capsule for a named younger survivor, include one real personal belonging and one letter, hide it in a valid shelter location, and set it to open on Day 120. Seal it and prove the physical item leaves normal inventory and enters canonical capsule custody. Save/load on Day 80. A maintenance action discovers the capsule on Day 90: record the discovery, but keep the capsule sealed because the date condition is not met. Kill the creator through the canonical fate pipeline on Day 100; the capsule remains valid, Plan 206 handles estate context, and no new posthumous message is fabricated. Advance to Day 120 through `ISimClock`, make the capsule eligible, let the designated survivor open it, transfer the exact same item instance through PersonalBelongings/Inventory, read the letter, and emit one grief/comfort reaction through canonical psychology/morale. Save/load immediately before opening and immediately after reading: the capsule must never reopen, the item must never duplicate, the message must never redeliver, and the reaction must never reapply. Finally run a second capsule addressed to the shelter leader on a future leadership-change event and prove the recipient resolves dynamically to the canonical leader at delivery time.**
