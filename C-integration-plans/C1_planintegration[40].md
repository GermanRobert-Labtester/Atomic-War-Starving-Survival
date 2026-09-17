# C1 — Flagship Integration Plan [40]: Survivor Personal Belongings, Keepsakes, Ownership, Gifts & Inheritance

> **Output:** `C1_planintegration[40].md`
>
> **Source baseline:** Plan 210 — Survivor Personal Belongings & Effects
>
> **Primary mission:** give each survivor material identity through personally meaningful possessions—keepsakes, clothing, tools, weapons, mementos, documents, jewelry and inherited effects—while preserving `Inventory` as the one physical item authority, `WornGear`/equipment as the equipped-item authority, equipment condition as the physical-condition authority, Plan 206 as death/inheritance orchestration, Plan 202 as interpersonal-conflict authority, Plan 147/190 as memory/lore authorities, and Needs/mental-health/relations as the owners of actual survivor state.
>
> **Primary architectural rule:** `PersonalBelongingsSystem` owns **personal-claim metadata and sentimental association**, not a second item container. A personal belonging is a canonical item instance (or an explicitly non-functional narrative artifact registered through the item authority) with an ownership/attachment record referencing the survivor. The system owns owner claim, sentimental salience, favorite designation, acquisition provenance, transfer provenance, inheritance provenance and belonging-specific lifecycle state. It does not own physical item existence, stack quantity, durability/condition, worn/equipped state, weapon function, market value, inventory transfer, survivor morale, relationship affinity, memory text, death state, or theft resolution.
>
> **Primary correction to the source plan:** the source defines `PersonalBelonging` with duplicated `itemId`, `itemName`, `condition`, description and a dictionary of survivor → item lists “separate from shared inventory.” That risks creating a shadow inventory and two physical truths. The flagship instead requires a **canonical item-instance reference** plus personal metadata. If an object is functional, it lives in `Inventory`/equipment exactly once. If it is purely narrative (photo, letter, diary page, medal), it must still be represented through a canonical artifact/item/content identity or an explicit lightweight non-stackable item instance owned by the inventory/item authority—not an invisible duplicate object stored only in the belongings system.
>
> **Primary sentiment rule:** sentimental value is not a passive additive morale buff applied every day. It is an attachment/salience input used by canonical morale/mental-health/relationship systems when meaningful events occur—acquisition, loss, damage, theft, gifting, recovery, inheritance, anniversary/memory trigger, or deliberate handling. This prevents “ten trinkets = permanent +100 morale” and avoids double-counting with Needs and psychology.
>
> **Primary condition rule:** `PersonalBelongingsSystem` never owns `condition`. Physical condition remains on the canonical item instance / `EquipmentConditionSystem`. Belongings may interpret condition changes as sentimental events (“favorite scarf damaged”) but must not maintain a duplicate 0–100 condition value.
>
> **Primary transfer rule:** gift, theft, confiscation, assignment and inheritance are **ownership transitions over a real item instance**. The physical transfer occurs through canonical inventory/equipment/property APIs first or transactionally alongside the personal-claim update. The belongings system never teleports items between survivors, creates copies, or changes quantity itself.
>
> **Primary death rule:** Plan 206 / death legacy owns death-time orchestration. On death, it queries personal-claim records and decides disposition. Inheritance is one possible transfer outcome; memorial retention, return to shared inventory, burial/cremation association, unresolved estate, or loss may also be valid depending on existing systems. PersonalBelongings records provenance and attachment consequences; it does not independently distribute the estate.
>
> **Primary memory rule:** sentimental association points to canonical memory/event/lore IDs where possible. The belongings system must not duplicate diary text, relationship history, memorial history or item-lore prose. Plan 147 owns personal memory; Plan 190 owns item lore/history; MemorialSystem owns remembrance surfaces.
>
> **Primary gameplay rule:** personal ownership may constrain automated consumption/sale/scrapping of an item, but these restrictions must be explicit policy checks. A “personal” item does not become inaccessible to emergency systems by magic; emergency override, consent, confiscation or last-resort use must have clear rules.
>
> **Mandatory execution order:** 210A inventory/item-instance/property authority audit → 210B personal-claim and sentimental-association contract → 210C keepsake/item-template content model → 210D acquisition and ownership assignment → 210E gifts/voluntary transfers → 210F loss/theft/confiscation/recovery → 210G Plan-206 death/inheritance handoff → 210H memory/lore/memorial integration → 210I morale/relations/conflict/autonomy effects → 210J UI/policies and anti-clutter design → 210K persistence/migration/idempotence → 210L determinism, exploit prevention, performance and long-horizon simulation → 210M advanced legacy/collecting/trading only after the base architecture proves itself.
>
> **Critical re-baseline rule:** before creating `PersonalBelongingsSystem.cs`, inspect `Inventory.cs`, item-instance IDs, stack semantics, WornGear/equipment ownership, `EquipmentConditionSystem`, inventory transfer APIs, item destruction/scrap/sale/consumption flows, survivor lifecycle, Plan 206 Death & Inheritance, `SurvivorRelationsSystem`, Plan 147 per-NPC memory, Plan 185 memory decay, Plan 190 Item Lore, `MemorialSystem`, Needs/mental-health/morale authority, Plan 202 interpersonal conflict, Plan 144 autonomy, room/storage systems, trade/market, crafting, expedition loot provenance, save orchestration and UI item-detail surfaces.
>
> **Guardrails:** no second physical inventory; no duplicate item condition; no duplicate item name/category truth; no personal item list that can diverge from Inventory; no invisible functional keepsakes outside canonical item instances; no daily flat morale bonus per belonging; no doubled favorite morale effect as an unconditional multiplier; no automatic sentimental growth every day; no decay of sentiment merely because time passes unless a real memory/salience policy says so; no theft if property/transfer semantics do not exist; no gift relationship delta applied both here and in Relations; no inheritance distribution outside Plan 206; no direct survivor morale mutation if Needs/psychology owns morale; no item duplication on transfer/death/save-load; no auto-sale/scrap of personal items without policy; no hard `max 10` unless a gameplay capacity rule has real meaning; no unseeded RNG; no `Guid.NewGuid`; no wall clock; no per-frame belonging scans; no sentimental-item grind loops; no quest designs that incentivize cycling gifts, deaths, thefts or favorite toggles.

---

# 0. Mission

ASHFALL currently treats almost every physical object as shelter property.

The source baseline reports:
- `Inventory.cs` as the shared shelter inventory authority;
- `WornGear` for equipped items;
- `EquipmentConditionSystem` for physical equipment condition;
- no personal inventory;
- no keepsakes;
- no survivor-level material ownership;
- no mementos;
- no sentimental items;
- no personal effects left behind on death.

Current shape:

```text
ITEM INSTANCE
    │
    ├── shared shelter inventory
    └── optionally equipped/worn

SURVIVOR
    │
    └── no persistent personal claim/attachment
```

Target shape:

```text
CANONICAL ITEM INSTANCE
      │
      ├── Inventory owns existence / quantity / location
      ├── Equipment owns worn/equipped state
      ├── Condition system owns physical condition
      └── Item lore owns item history
              │
              ▼
PersonalBelongingClaim
      │
      ├── owner survivor
      ├── sentimental salience
      ├── favorite status
      ├── acquisition provenance
      ├── personal meaning tags
      ├── memory/lore references
      └── transfer/inheritance provenance
              │
              ▼
MEANINGFUL EVENTS
      │
      ├── gift
      ├── loss
      ├── damage
      ├── theft
      ├── recovery
      ├── inheritance
      ├── memorial use
      └── voluntary relinquishment
              │
              ▼
CANONICAL CONSUMERS
      ├────────► SurvivorRelationsSystem
      ├────────► Needs / mental health
      ├────────► Plan 147 memory
      ├────────► Plan 202 conflict
      ├────────► Plan 206 death legacy
      ├────────► MemorialSystem
      ├────────► Plan 190 item lore
      └────────► Inventory policies / trade / scrap UI
```

The belongings layer should answer:

> Which canonical items does this survivor regard as personally theirs, how attached are they, why does the item matter, which memories or relationships does it represent, and how should a transfer/loss/inheritance event be interpreted?

It should not answer:

> Does the item physically exist?
> What is its current condition?
> Is it equipped?
> What is its market value?
> What is the survivor's current morale?
> Did theft succeed?
> Who inherits after death?
> What text is written in the diary?

Those remain existing authorities.

---

# 1. Source-Evidence Interpretation

## 1.1 Personal belongings are genuinely absent

The source reports zero Core matches for:
- `PersonalBelongings`;
- `PersonalInventory`;
- `SurvivorPossessions`;
- `PersonalEffects`;
- `Keepsake`;
- `SentimentalItem`;
- `PersonalClothing`;
- `IndividualInventory`.

A survivor-level personal-claim system is justified.

## 1.2 `Inventory` must remain physical authority

The source says belongings should be separate from shared inventory.

Architecturally, “separate” should mean:
- separate **ownership semantics**;
not:
- duplicate physical storage.

A personal knife still exists exactly once.

## 1.3 WornGear already creates a survivor-item association

Audit whether:
- equipped item instance already stores survivor ID;
- unequipping returns to shared inventory;
- personal claim can persist through equip/unequip.

Personal ownership and equipment state must be orthogonal.

## 1.4 `EquipmentConditionSystem` owns condition

Delete `condition` from persisted personal-claim DTO unless it is a cached read-only projection.

A beloved item can be damaged because the actual item was damaged.

## 1.5 Plan 206 assumes possessions exist

That makes PersonalBelongings a producer of:
- estate candidate references;
not the inheritance executor.

## 1.6 Plan 190 item lore is complementary

Item history might say:

```text
found at Old Hospital
used by Mara for 80 days
inherited by Ivo
```

Belongings should emit provenance events.

Plan 190 owns item-history presentation/persistence if already established.

## 1.7 Plan 202 theft integration needs a real property event

A stolen keepsake can create conflict only if:
- ownership was valid;
- transfer/theft occurred;
- perpetrator is known or suspected;
- information reaches owner.

No arbitrary theft roll inside PersonalBelongings.

## 1.8 “Sentimental value affects morale” requires anti-double-count design

A daily passive buff would:
- reward hoarding;
- duplicate psychology;
- make max-belongings optimal play.

Prefer event/context effects.

---

# 2. Non-Negotiable Belongings Invariants

## INV-210.1 — One physical item authority

`Inventory`/item instance is canonical.

## INV-210.2 — One condition authority

## INV-210.3 — One equipment authority

## INV-210.4 — Personal ownership is metadata over canonical item identity

## INV-210.5 — A functional belonging cannot exist only in belongings state

## INV-210.6 — One owner claim per exclusive personal item

Unless shared ownership is explicitly modeled later.

## INV-210.7 — Personal ownership and physical location are separate facts

A personal item can be:
- worn;
- stored;
- temporarily lent;
- held for safekeeping.

## INV-210.8 — Sentiment is not morale

## INV-210.9 — Favorite is not a free global stat multiplier

## INV-210.10 — Transfer requires canonical item movement/ownership transaction

## INV-210.11 — Gift acceptance respects autonomy

## INV-210.12 — Gift relationship effects go through `SurvivorRelationsSystem`

## INV-210.13 — Theft is source-owned by a real theft/transfer action

## INV-210.14 — Conflict is owned by Plan 202

## INV-210.15 — Inheritance orchestration is owned by Plan 206

## INV-210.16 — Memorial remembrance is owned by MemorialSystem

## INV-210.17 — Item lore/history is owned by Plan 190

## INV-210.18 — Memory content is owned by Plan 147/185

## INV-210.19 — No auto-sale/scrap/consume without personal-item policy check

## INV-210.20 — Old saves remain behaviorally equivalent with no personal claims

## INV-210.21 — Transfer/loss/inheritance events are exactly once

## INV-210.22 — No per-frame belonging processing

## INV-210.23 — No duplicated item after death/save-load

## INV-210.24 — Headless behavior is deterministic

---

# 3. Definition of Done

Plan 210 closes only when:

- canonical item-instance identity is documented;
- stack semantics are documented;
- WornGear/equipment ownership is documented;
- condition authority is documented;
- inventory transfer/destruction/sale/scrap/consume paths are documented;
- a personal-claim state references canonical item instances;
- no duplicate physical inventory exists;
- no duplicate condition exists;
- every claimed functional item resolves to a canonical item instance;
- pure narrative keepsakes use a canonical artifact/item path;
- ownership claim survives equip/unequip;
- acquisition provenance is stable;
- sentimental salience/attachment is distinct from morale;
- favorites are policy/salience state, not direct doubled stat effects;
- gift transfers are transactional and exactly once;
- gift acceptance/refusal uses autonomy/relationship context;
- relationship effects are emitted exactly once to `SurvivorRelationsSystem`;
- theft/loss only follows real item-state changes;
- Plan 202 receives theft grievance triggers through semantic events;
- Plan 206 queries estate candidates and owns inheritance distribution;
- inheritance transfers preserve canonical item identity;
- inherited sentimental associations can reference deceased survivor/memory without duplicating memory text;
- MemorialSystem and Plan 190 receive source events exactly once;
- auto-sell/scrap/consume respects personal-item protection policy;
- emergency override behavior is explicit;
- old saves receive no personal claims and preserve current behavior;
- save/load round-trips owner claims, sentiment, favorite state and provenance;
- no transfer duplicates item;
- no deleted/destroyed item remains claimed;
- no orphan claim survives missing survivor/item without reconciliation;
- UI is useful without becoming an inventory-within-inventory;
- 30/120/180/400-day simulations show meaningful identity without sentimental clutter;
- `--personal-belongings-selftest` exists or equivalent;
- data integrity validates item/survivor/template refs and ownership consistency.

---

# 4. Phase P0 — Inventory, Item Instance & Ownership Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
Inventory item-instance model
stack IDs / stack merge rules
item definition catalog
WornGear/equipment model
EquipmentConditionSystem
inventory add/remove/transfer APIs
consume APIs
scrap/disassemble APIs
sell/trade APIs
crafting output APIs
expedition loot provenance
survivor inventory/equipment APIs
room/storage APIs
survivor death lifecycle
Plan 206 DeathLegacySystem
SurvivorRelationsSystem
Plan 147 personal memory
Plan 185 memory decay
Plan 190 item lore
MemorialSystem
Needs / morale / mental health
Plan 202 conflict
Plan 144 autonomy
save order
inventory UI / survivor detail
```

## P0.2 Build belonging authority matrix

Create:

`docs/belongings/PERSONAL_BELONGINGS_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
read API
write API
belongings role
persisted?
status
```

Rows:
- item definition;
- item instance ID;
- stack;
- physical location;
- quantity;
- condition;
- equipped state;
- owner claim;
- sentimental salience;
- favorite;
- acquisition provenance;
- gift;
- theft;
- confiscation;
- inheritance;
- item lore;
- personal memory;
- relationship;
- morale/stress;
- death;
- memorial state.

## P0.3 Item-instance ADR

Create:

`docs/architecture/ADR_PERSONAL_BELONGINGS_OVER_CANONICAL_ITEMS.md`

Define:
- no shadow inventory;
- claim reference;
- stack rules;
- narrative-only artifacts.

## P0.4 Ownership/property ADR

Create:

`ADR_SURVIVOR_PERSONAL_PROPERTY_SEMANTICS.md`

Define:
- personal claim;
- shelter custody;
- possession;
- equipped;
- loaned;
- confiscated;
- estate.

## P0.5 Sentiment ADR

Create:

`ADR_SENTIMENTAL_VALUE_VS_MORALE.md`

## P0.6 Death/inheritance ADR

Create:

`ADR_PERSONAL_BELONGINGS_AND_DEATH_LEGACY.md`

## P0.7 Baseline proof

Demonstrate:
- all unequipped items return to shared inventory;
- no survivor personal claim persists;
- death leaves no personal-effect ownership state.

---

# TASK 210A — Personal Claim & Belonging Record

# 210A.0 Goal

Represent a survivor's personal relationship to a canonical item instance.

## 210A.1 Proposed owner

`Assets/Ashfall.Core/Survivors/PersonalBelongingsSystem.cs`

## 210A.2 Recommended DTO

```text
PersonalBelongingClaim
  belonging_id
  item_instance_id
  owner_survivor_id
  claim_state
  category_override optional
  sentimental_salience
  attachment_band
  acquired_day
  acquisition_source_ref
  personal_meaning_tags[]
  memory_refs[]
  favorite
  inherited_from_survivor_id optional
  inheritance_event_id optional
  last_meaningful_event_day
  processed_event_ids[]
```

## 210A.3 `belonging_id`

Can derive from item instance when one-to-one:

```text
belonging:<item_instance_id>
```

Avoid second random identity if unnecessary.

## 210A.4 `item_instance_id`

Mandatory for functional/physical objects.

## 210A.5 No `itemName`

Resolve through item definition/localization.

## 210A.6 No duplicate `itemId`

Item instance already references definition.

## 210A.7 No duplicate `condition`

Query canonical item.

## 210A.8 Category

Prefer item tags/catalog.

`category_override` only if:
- social meaning differs from technical item category.

Example:
- functional knife treated as keepsake.

## 210A.9 Claim state

Suggested:

```text
owned
loaned
held_in_custody
estate_pending
transferring
relinquished
lost
destroyed
```

Use only needed states.

## 210A.10 Physical location not claim state

Item can be owned by survivor while:
- in shared locker.

## 210A.11 Sentimental salience

0–100 internal acceptable.

Represents:
- emotional significance.

Not morale.

## 210A.12 Attachment band

Derived:

```text
minor
valued
cherished
irreplaceable
```

## 210A.13 Personal meaning tags

Examples:
- family;
- prewar;
- relationship;
- survival;
- achievement;
- memorial;
- identity;
- vocation.

Data-driven.

## 210A.14 Memory refs

Stable IDs only.

## 210A.15 Favorite

At most one active favorite per survivor for MVP.

But favorite means:
- most salient/personal;
not:
- 2× every gameplay effect.

## 210A.16 Inherited provenance

Reference deceased survivor.

## 210A.17 Processed events

Exactly-once transfer/loss effects.

## 210A.18 CaptureState

Versioned.

## 210A.19 RestoreState

No side effects.

## 210A.20 Reconciliation

Claim requires:
- live item instance or valid destroyed/lost terminal record.

### 210A DoD

A belonging is a personal claim over one canonical item instance, with only attachment/provenance metadata persisted.

---

# TASK 210B — Keepsake Templates & Canonical Item Content

# 210B.0 Goal

Create authored meaningful items without an invisible second content catalog.

## 210B.1 Source file

`Assets/StreamingAssets/Data/keepsake_templates.json`

Appropriate if templates describe:
- attachment meaning;
- acquisition contexts;
- flavor selection.

## 210B.2 Template DTO

Suggested:

```text
template_id
eligible_item_tags[]
specific_item_definition_id optional
display_role_key
default_meaning_tags[]
base_salience_range
acquisition_context_tags[]
memory_link_policy
rarity
narrative_hook_tags[]
```

## 210B.3 No `templateName` truth duplication

Use localization key.

## 210B.4 No full description if Plan 190/item catalog already owns lore

Use:
- flavor key;
- meaning key
if needed.

## 210B.5 Categories

Source:
- keepsake;
- clothing;
- tool;
- weapon;
- memento;
- document;
- jewelry.

Treat as social roles.

## 210B.6 Keepsake

May map to:
- photo;
- letter;
- trinket;
- toy;
- token.

These must still be canonical item/artifact instances.

## 210B.7 Clothing

Personal claim can attach to:
- scarf;
- hat;
- jacket.

Equipment/worn state stays canonical.

## 210B.8 Tool

Functional tool remains tool item.

Personal claim adds:
- “favorite wrench”.

## 210B.9 Weapon

Weapon remains canonical weapon/equipment.

No sentimental copy.

## 210B.10 Memento

Item linked to event.

## 210B.11 Document

If readable text exists:
- document/lore system owns text.

## 210B.12 Jewelry

Canonical item.

## 210B.13 30+ templates

Content target only.

Ship every template only if:
- eligible item exists;
- acquisition path exists;
- localization exists.

## 210B.14 Unique templates

Stable uniqueness policy.

## 210B.15 Template does not create item by itself

Acquisition system decides.

## 210B.16 Data integrity

Validate:
- item tags/IDs;
- memory hooks;
- localization;
- rarity enum.

## 210B.17 Generated docs

`KEEPSAKE_TEMPLATE_MATRIX.md`

### 210B DoD

Keepsake templates describe how canonical items can become personally meaningful without creating a second functional item catalog.

---

# TASK 210C — Acquisition & Personal Claim Creation

# 210C.0 Goal

Create personal ownership through explicit meaningful events.

## 210C.1 Acquisition sources

Source candidates:
- events;
- crafting;
- gifts;
- inheritance;
- discovery;
- assignment.

## 210C.2 Discovery

Expedition loot enters Inventory first.

Then:
- survivor may claim item if policy/event permits.

## 210C.3 Automatic claim

Only for authored cases:
- personal photograph found in survivor's origin event;
- pre-existing starting keepsake;
- named quest reward.

## 210C.4 Shared loot

Do not auto-claim valuable shared resources.

## 210C.5 Crafted item

Crafter does not automatically own every crafted item.

Need:
- personal commission;
- player assignment;
- event.

## 210C.6 Assignment

Player may designate canonical item as personal property.

Respect:
- shelter policy;
- survivor autonomy if relevant.

## 210C.7 Starting belongings

Potential.

But old-save migration remains none.

New-game survivor generation can:
- assign 0–N authored starting keepsakes.

## 210C.8 Acquisition event

Semantic:

```text
personal_belonging_claim_created
```

## 210C.9 Claim eligibility

Item cannot be:
- consumed;
- destroyed;
- quest-locked;
- already exclusively owned
unless transfer flow.

## 210C.10 Stackable items

Personal claim on one unit requires:
- item-instance/stack split.

Do not claim “one of 20” without canonical identity.

## 210C.11 Split transaction

Inventory owns.

## 210C.12 Capacity

Source default max 10.

Recommended:
- no hard simulation cap initially;
- UI soft target.

If hard cap retained:
- define physical/policy reason.

## 210C.13 Clutter budget

The design should naturally produce:
- 1–5 meaningful items,
not 10 mandatory.

## 210C.14 Acquisition salience

Depends on:
- source event;
- giver;
- rarity;
- survivor traits;
- memory.

## 210C.15 No random attachment reroll

## 210C.16 Acquisition provenance

Reference:
- location;
- event;
- giver;
- craft job
through stable ID.

### 210C DoD

Personal ownership begins through explicit claims over real item instances, with stack splitting, provenance and eligibility handled transactionally.

---

# TASK 210D — Sentimental Attachment & Meaning

# 210D.0 Goal

Make belongings emotionally important without turning them into passive buff slots.

## 210D.1 Sentimental salience

Represents:
- attachment strength.

## 210D.2 Inputs

Potential:
- family provenance;
- gift from close relation;
- inherited from deceased loved one;
- survival event;
- long use;
- authored memory;
- personal vocation/identity.

## 210D.3 No generic daily growth

Source proposes attachment increasing over time.

Correct approach:
- meaningful milestones;
- sustained use;
- anniversary/memory;
- survival events.

## 210D.4 Familiarity

Simply owning item for days may produce slow capped attachment only if designed.

No mandatory daily write.

Use lazy formula or milestone.

## 210D.5 Damage

Physical damage can reduce:
- utility;
but sentiment may:
- decrease;
- stay same;
- increase (“scarred keepsake”).

Do not hardcode `damage = sentiment loss`.

## 210D.6 Destruction

Can produce strong loss event.

## 210D.7 Favorite

Favorite selection can:
- mark which item's loss/recovery is most salient.

Do not simply double every number.

## 210D.8 Attachment vs rarity

Rare is not automatically sentimental.

## 210D.9 Attachment vs market value

Separate.

Cheap ring may be irreplaceable.

## 210D.10 Attachment vs condition

Separate.

Broken watch may remain cherished.

## 210D.11 Attachment vs memory

Memory refs explain meaning.

Plan 147 owns memory record.

## 210D.12 Decay

Do not implement generic sentimental decay unless:
- memory-decay policy says attachment fades.

Some attachments can strengthen.

## 210D.13 Inherited item

May gain:
- memorial meaning.

But not guaranteed higher value.

Depends on relationship.

## 210D.14 Negative sentiment

Potential future:
- hated reminder;
- trauma-linked object.

Do not force all personal belongings positive.

## 210D.15 MVP

Positive/neutral attachment is sufficient.

### 210D DoD

Sentimental value becomes event- and relationship-driven attachment, independent of item rarity, physical condition, market price and survivor morale.

---

# TASK 210E — Morale & Mental-Health Integration

# 210E.0 Goal

Make personal objects matter psychologically without stacking permanent buffs.

## 210E.1 Needs/mental-health authority

Audit exact morale/stress owner.

## 210E.2 PersonalBelongings emits semantic context

Examples:
- cherished_item_received;
- cherished_item_lost;
- cherished_item_destroyed;
- cherished_item_recovered;
- inherited_memory_item_received.

## 210E.3 No direct persistent `morale += sentimentalValue`

## 210E.4 Event effect

Canonical psychology system decides:
- magnitude;
- duration;
- resilience/traits.

## 210E.5 Possession comfort

If desired:
- small contextual modifier
through psychology.

Bound:
- capped across all belongings.

## 210E.6 No linear stacking

Ten keepsakes do not produce 10× comfort.

## 210E.7 Favorite

May raise:
- event salience.

Not automatic 2×.

## 210E.8 Loss

Impact depends:
- attachment;
- circumstances;
- trait;
- memory.

## 210E.9 Damage

Can produce:
- concern/grief
only if meaningful threshold crossed.

## 210E.10 Recovered item

Can produce:
- relief.

## 210E.11 Inheritance

Can produce:
- grief comfort;
- mixed response.

Plan 179/mental health.

## 210E.12 Memorialization

Could convert:
- grief into remembrance
through MemorialSystem.

## 210E.13 Double-count guard

Death itself already causes grief.

Inherited item must not:
- duplicate full death grief.

It may modify recovery/comfort.

### 210E DoD

Belongings influence psychology through meaningful events and bounded contextual comfort rather than additive permanent morale stacking.

---

# TASK 210F — Gift Giving & Voluntary Transfer

# 210F.0 Goal

Make gifts real property transfers with relationship meaning.

## 210F.1 Gift request

Suggested:

```text
BelongingTransferIntent
  transfer_id
  item_instance_id
  from_survivor_id
  to_survivor_id
  transfer_type
  reason_ref optional
  day
```

## 210F.2 Preconditions

- giver owns claim;
- item exists;
- item not consumed/destroyed;
- not equipped unless unequip transaction succeeds;
- receiver valid;
- item transferable.

## 210F.3 Autonomy

Receiver may:
- accept;
- refuse.

## 210F.4 Player-gift vs survivor-initiated gift

Both can use same transaction.

## 210F.5 Physical transfer

Inventory/equipment authority.

## 210F.6 Claim transfer

After/within transaction:
- old owner claim relinquished;
- new owner claim created.

## 210F.7 Sentimental inheritance of gift

Receiver attachment depends:
- relationship;
- item;
- reason;
- prior meaning.

Do not copy giver's sentiment automatically.

## 210F.8 Giver attachment

Giving a cherished item may be:
- sacrifice;
- generosity;
- painful.

Psychology decides.

## 210F.9 Relationship effect

Submit semantic gift event to `SurvivorRelationsSystem`.

## 210F.10 No direct affinity mutation here

## 210F.11 Refusal

No automatic penalty.

Depends:
- reason;
- relationship;
- expectations;
- item.

## 210F.12 Gift farming

Repeated item cycling:
- no repeated full relationship gain.

Track provenance and cooldown.

## 210F.13 Gift back

May be meaningful but not farmable.

## 210F.14 Gift while grieving

Could be narrative.

## 210F.15 Transaction rollback

If physical transfer fails:
- claim unchanged.

## 210F.16 Exactly once

Transfer event stable.

### 210F DoD

A gift changes both physical custody and personal ownership exactly once, respects autonomy, and routes all social/psychological effects through canonical systems.

---

# TASK 210G — Loss, Destruction, Theft, Confiscation & Recovery

# 210G.0 Goal

Handle adverse item events without inventing parallel theft or condition mechanics.

## 210G.1 Loss event

Source systems can emit:
- item destroyed;
- item consumed;
- item lost on expedition;
- item discarded;
- item stolen;
- item confiscated.

## 210G.2 PersonalBelongings subscribes

If affected item has claim:
- update claim lifecycle;
- emit sentimental event.

## 210G.3 Destroyed

Item authority destroys.

Belongings marks:
- destroyed terminal state or archives claim.

## 210G.4 Lost

If item may still exist in world:
- claim remains lost.

## 210G.5 Recovered

Same item instance if possible.

If representation rematerializes:
- provenance/link migration required.

## 210G.6 Theft

Requires real action/event.

## 210G.7 Known thief

Plan 202 receives:
- property_violation trigger.

## 210G.8 Unknown thief

Owner may have:
- loss grievance without target,
or
- suspicion event if another system supports.

Do not invent culprit.

## 210G.9 Confiscation

Requires:
- governance/security action.

May create fairness/conflict event.

## 210G.10 Emergency requisition

Potential:
- shelter can temporarily use personal tool/weapon.

Policy + autonomy/governance.

## 210G.11 Consumption

A personal consumable should normally be protected.

If consumed:
- explicit consent/emergency policy.

## 210G.12 Scrapping

Protected by UI/policy.

## 210G.13 Selling

Protected.

## 210G.14 Assignment

Loan vs transfer distinct.

## 210G.15 Loan

Owner remains same.
Physical holder/equipment changes.

## 210G.16 Theft relation effect

Plan 202 + Relations decide.

No direct penalty here.

## 210G.17 Recovery event

Can resolve:
- grievance;
- loss psychology
through consumers.

### 210G DoD

Loss, theft, confiscation and recovery react to real physical item events, preserve culprit uncertainty, and never fabricate item destruction or interpersonal consequences.

---

# TASK 210H — Favorite Designation

# 210H.0 Goal

Provide a clear personal focal object without creating an optimal-stat slot.

## 210H.1 One favorite per survivor

MVP.

## 210H.2 Favorite eligibility

Must be:
- currently personally owned;
- not destroyed/lost.

## 210H.3 Favorite meaning

- highest subjective attachment;
- UI prominence;
- event salience;
- narrative hook.

## 210H.4 No unconditional 2× morale

Reject source default.

## 210H.5 Loss of favorite

Psychology can apply stronger response based on:
- actual salience.

## 210H.6 Favorite switching

Should not be:
- free effect reset.

## 210H.7 Cooldown

Optional if switching has gameplay consequences.

## 210H.8 Survivor autonomy

Could choose favorite automatically/independently.

Player may:
- view;
- suggest
depending game philosophy.

## 210H.9 Player designation

If retained:
- treat as UI management, not character mind control.

Better:
- survivor selects based on attachment.

## 210H.10 Deterministic selection

Highest salience + stable tie-break.

## 210H.11 Favorite history

Only significant.

No event spam for every internal recalculation.

### 210H DoD

Favorite status highlights genuine attachment and narrative salience without becoming a manipulable doubled-buff slot.

---

# TASK 210I — Plan 206 Death & Inheritance Integration

# 210I.0 Goal

Ensure survivor death leaves coherent personal effects without duplicating estate logic.

## 210I.1 DeathLegacySystem owns death workflow

## 210I.2 Query

On death:

```text
GetPersonalEstate(survivorId)
```

returns canonical item refs + claim metadata.

## 210I.3 Estate state

Claims transition:
- estate_pending.

## 210I.4 No immediate auto-distribution inside PersonalBelongings

## 210I.5 Plan 206 decides

Potential:
- named heir;
- close relation;
- memorial;
- shared inventory;
- burial;
- unresolved estate.

## 210I.6 Named inheritance

If Plan 206 supports wills/heirs:
- use.

## 210I.7 Relationship-based heir

Plan 206/relations policy.

## 210I.8 Physical transfer

Inventory authority.

## 210I.9 Claim creation for heir

After Plan 206 commits inheritance.

## 210I.10 Sentiment

Receiver does not simply inherit same 0–100.

Compute from:
- relationship to deceased;
- item's prior meaning;
- event context;
- memorial association.

## 210I.11 `isInherited`

Can be derived from provenance.
No need standalone boolean if event exists.

## 210I.12 Deceased owner claim

Archive/close.

## 210I.13 Memorial item

If item placed at memorial:
- MemorialSystem owns placement/state.
Personal claim may become:
`memorialized`.

## 210I.14 No duplicate death grief

## 210I.15 Inheritance exactly once

Stable death/estate transfer ID.

## 210I.16 Simultaneous death

Deterministic estate order.

## 210I.17 No heir

Fallback canonical policy.

## 210I.18 Old save

No estate claims to distribute.

### 210I DoD

DeathLegacy orchestrates every personal-effect disposition while belongings supply the estate metadata and preserve inheritance provenance without duplicating items or grief.

---

# TASK 210J — Memorial, Memory & Item Lore Integration

# 210J.0 Goal

Let personal objects carry stories without duplicating narrative authorities.

## 210J.1 Plan 190 item lore

Can record:
- found;
- claimed;
- gifted;
- inherited;
- damaged;
- memorialized.

## 210J.2 PersonalBelongings emits item-history events

## 210J.3 Plan 147 memory

Survivor memory can link:
- “gift from X”;
- “belonged to deceased Y”;
- “carried during event Z”.

## 210J.4 No description duplication

## 210J.5 MemorialSystem

Can display:
- selected personal effect;
- inherited chain;
- deceased favorite.

## 210J.6 Memorial placement

Physical item must remain canonical.

## 210J.7 Memory decay

Plan 185 may reduce:
- conscious salience.

But inherited provenance remains historical.

## 210J.8 Anniversary events

Optional.

Use memory/calendar systems.

## 210J.9 Diary/document

Readable content remains document/lore authority.

## 210J.10 Famous keepsake

Plan 162/archive follow-on.

## 210J.11 Chain of ownership

Plan 190 can show:
- owner history.

Belongings stores transfer refs.

## 210J.12 No infinite history

Compact routine transfers.

Preserve:
- significant gift;
- inheritance;
- memorial.

### 210J DoD

Belongings become anchors for canonical memories, memorials and item histories while PersonalBelongings remains a metadata/provenance layer rather than a narrative database.

---

# TASK 210K — SurvivorRelations Integration

# 210K.0 Goal

Make personal-item exchanges socially meaningful without double-counting affinity.

## 210K.1 Relations authority

`SurvivorRelationsSystem`.

## 210K.2 Gift event

Submit semantic context:
- giver;
- receiver;
- item;
- receiver attachment;
- giver sacrifice;
- reason.

## 210K.3 Relations computes delta

## 210K.4 No direct relationship state in belonging transfer

## 210K.5 Inheritance

May affect:
- surviving relation memory;
not necessarily relationship to deceased because deceased relationship state may archive.

## 210K.6 Theft

Plan 202/Relations consume.

## 210K.7 Return borrowed item

Could strengthen reliability.

Only if relationship system supports.

## 210K.8 Refused gift

Context-specific.

## 210K.9 Favor/promise

If gift fulfills promise:
- promise system owns fulfillment.

## 210K.10 No relationship farming

One transfer provenance.

## 210K.11 Valuable vs sentimental

Relationship reaction can use:
- sacrifice;
- personal meaning;
not just market price.

### 210K DoD

Gift, theft, return and inheritance events provide relationship context exactly once while `SurvivorRelationsSystem` remains the only affinity/trust authority.

---

# TASK 210L — Plan 202 Conflict Integration

# 210L.0 Goal

Allow personal property violations to generate real grievances without making belongings a conflict engine.

## 210L.1 Theft trigger

If:
- canonical theft occurred;
- victim owns claim;
- perpetrator known/credible.

Emit:

```text
personal_property_violation
```

## 210L.2 Subject ref

Item instance ID.

## 210L.3 Salience

Attachment informs severity hint.

## 210L.4 Conflict system decides

- grievance;
- escalation;
- resolution.

## 210L.5 Confiscation dispute

Could emit:
- fairness/property trigger.

## 210L.6 Gift refusal

Not automatically conflict.

## 210L.7 Inheritance dispute

Only if Plan 206/governance emits:
- contested estate event.

## 210L.8 Lost item blamed on survivor

Requires causal/source event.

## 210L.9 Favorite damage

If another survivor caused:
- possible trigger.

## 210L.10 Recovery/restitution

Can resolve conflict through Plan 202.

## 210L.11 No direct conflict creation here

### 210L DoD

Personal belongings provide source-backed property-violation events to Plan 202 while all grievance, blame, escalation and reconciliation logic remains in the conflict system.

---

# TASK 210M — Autonomy, Consent & Personal Property Policy

# 210M.0 Goal

Prevent the system from turning survivor possessions into player-controlled decorative slots with no agency.

## 210M.1 Plan 144 autonomy

Audit:
- refusal;
- personal choice;
- consent.

## 210M.2 Claim assignment

Survivor may:
- accept;
- reject
player assignment if meaningful.

## 210M.3 Gift

Receiver can refuse.

## 210M.4 Giver

Survivor should not be forced to gift favorite without:
- player authority/policy;
- relationship consequence;
- coercion model.

## 210M.5 Confiscation

Governance action.

## 210M.6 Emergency requisition

Policy:
- personal weapon/tool may be borrowed in shelter emergency.

## 210M.7 Return obligation

Loan record.

## 210M.8 Sale/scrap protection

Default:
- protected.

## 210M.9 Player override

Requires:
- explicit confirmation.

Potential consequence:
- autonomy;
- grievance;
- morality
through canonical systems.

## 210M.10 Consumables

Do not designate common ration as cherished personal belonging by default.

## 210M.11 Resource scarcity

Personal property can create meaningful tension.

But shelter emergency policy may override.

## 210M.12 Policy settings

Potential:
- respect personal property;
- allow emergency requisition;
- communal ownership.

Governance authority if policies exist.

## 210M.13 No hidden hard block

UI explains:
- why item cannot be sold/scrapped.

### 210M DoD

Personal belongings create real property expectations and consent boundaries while governance/autonomy systems retain authority over coercion and emergency exceptions.

---

# TASK 210N — Inventory, Stack, Equipment & Storage Integration

# 210N.0 Goal

Ensure personal ownership works through every physical item lifecycle path.

## 210N.1 Shared inventory

Personal item may physically reside there.

UI marks:
- owner.

## 210N.2 Stack merge

Exclusive claimed item cannot merge into anonymous stack unless:
- identity is preserved.

## 210N.3 Stack split

Required for personal claim on one unit.

## 210N.4 Non-stackable

Simpler.

## 210N.5 Equipment

Claim persists when equipped.

## 210N.6 Unequip

Claim persists.

## 210N.7 Transfer equipment

If another survivor equips:
- loan;
- transfer;
- confiscation
must be explicit.

## 210N.8 Condition

UI queries canonical condition.

## 210N.9 Repair

Repair system can operate personal item.

Ownership unchanged.

## 210N.10 Crafting ingredient

Protected by default.

## 210N.11 Scrapping

Protected.

## 210N.12 Selling

Protected.

## 210N.13 Expedition

Owner may carry personal item.

Loss event if real item lost.

## 210N.14 Storage zone

Optional personal locker.

If storage system supports.

Do not create second container solely for semantics.

## 210N.15 Inventory filter

- personal;
- owner;
- favorite;
- inherited.

## 210N.16 Bulk actions

Exclude protected personal items by default.

## 210N.17 Item removal reconciliation

Any canonical item removal triggers claim update.

## 210N.18 No orphan claim

### 210N DoD

Every inventory/equipment/storage operation preserves or explicitly resolves personal ownership without stack identity loss or shadow copies.

---

# TASK 210O — Condition, Damage & Repair Semantics

# 210O.0 Goal

Use real physical condition to create emotional events without duplicating durability.

## 210O.1 Condition authority

`EquipmentConditionSystem` / item state.

## 210O.2 Personal claim reads current condition

## 210O.3 Threshold events

Possible:
- damaged;
- badly damaged;
- restored;
- destroyed.

## 210O.4 No daily “degraded” event

## 210O.5 Sentiment impact

Context-dependent.

## 210O.6 Repair

Can strengthen meaning:
- “I repaired my father's watch”
if authored.

Not automatic.

## 210O.7 Destroyed

Terminal physical item.

Claim archives.

## 210O.8 Replace item

New item instance is not same keepsake.

Do not transfer sentimental identity automatically.

## 210O.9 Rebuilt/restored

If repair system preserves item ID:
- same.

If destroys/recreates:
- need lineage/refurbishment semantics.

## 210O.10 Consumable personal item

If consumed:
- item gone.

Attachment event.

## 210O.11 No condition copy in save

### 210O DoD

Belongings react to canonical damage and repair thresholds while physical condition remains single-source and item identity is preserved across legitimate repair paths.

---

# TASK 210P — Acquisition from Events, Discovery & Crafting

# 210P.0 Goal

Create emergent personal items through existing content rails.

## 210P.1 Expedition discovery

A survivor may discover:
- photo;
- letter;
- tool;
- trinket.

Item enters canonical loot.

## 210P.2 Personal claim

Can be offered after:
- story event;
- survivor connection;
- personal quest.

## 210P.3 Plan 200 personal quests

Can reward:
- personally meaningful item.

Quest owns reward event.

## 210P.4 Crafting

Custom item can become personal if:
- crafted for survivor;
- survivor invests labor/material;
- relationship gift.

## 210P.5 Named weapon/tool

If naming system exists:
- integrate.

Do not create naming subsystem unless desired.

## 210P.6 Clothing

Personal clothing may be:
- worn gear;
- off-duty outfit
if clothing system supports.

## 210P.7 Starting keepsake

New survivor generation can draw:
- template + canonical item.

## 210P.8 Deterministic generation

Seed:
- campaign;
- survivor;
- template context
if procedural.

## 210P.9 No reroll on reload

## 210P.10 Rarity

Affects content distribution, not sentiment guarantee.

## 210P.11 Unique

Enforce canonical uniqueness.

## 210P.12 Item identification

Plan 191:
- unknown discovered item may become personal only after/while identified depending context.

Ownership can exist before full identification:
- survivor knows “mother's pendant” even if material unknown.

## 210P.13 Lore

Plan 190 receives provenance.

### 210P DoD

Personal effects emerge from expeditions, quests, crafting and survivor generation through canonical item creation paths and deterministic provenance.

---

# TASK 210Q — UI & Anti-Clutter Design

# 210Q.0 Goal

Make personal items emotionally visible without forcing players to manage a second inventory screen.

## 210Q.1 Survivor detail

Add:
- Personal Effects section.

## 210Q.2 Display

Each:
- item name;
- owner;
- personal meaning;
- attachment band;
- canonical condition;
- favorite;
- provenance.

## 210Q.3 No duplicate item stats

Use inventory/item detail.

## 210Q.4 Shared inventory

Owner badge.

## 210Q.5 Gift action

Contextual action from item/survivor detail.

## 210Q.6 Favorite

Display.
Prefer survivor-driven designation.

## 210Q.7 Inheritance

Plan 206 UI.

Belongings supplies estate details.

## 210Q.8 Belonging log

Do not show every condition tick.

Show:
- acquired;
- gifted;
- stolen;
- recovered;
- inherited;
- destroyed;
- memorialized.

## 210Q.9 Filters

- owner;
- inherited;
- favorite;
- protected.

## 210Q.10 Sentiment score

Prefer bands over raw 0–100.

## 210Q.11 Meaning explanation

Example:
- “Gift from Anya after the refinery rescue.”

## 210Q.12 Protection warning

Sale/scrap:
- “Personal effect of Mira — Cherished.”

## 210Q.13 Bulk sale

Excludes personal by default.

## 210Q.14 Bulk scrap

Same.

## 210Q.15 Emergency use

Explicit action/override.

## 210Q.16 Tutorial

First meaningful personal claim or gift.

## 210Q.17 Tooltips

Not hover-only.

## 210Q.18 Accessibility

- text labels;
- keyboard/controller;
- screen-reader;
- text scale;
- favorite not icon-only.

## 210Q.19 Clutter budget

UI should not encourage:
- filling 10 slots.

## 210Q.20 No separate “personal inventory weight” unless actual carry system exists

### 210Q DoD

Personal effects are visible through survivor and inventory views with strong sale/scrap protections, clear provenance and minimal additional micromanagement.

---

# TASK 210R — Persistence, Migration & Reconciliation

# 210R.0 Goal

Persist personal meaning without duplicating item state or breaking old saves.

## 210R.1 Persist

Suggested:

```text
schema_version
feature_activation_day
personal_claims[]
transfer_provenance[]
processed_event_ids[]
next_transfer_sequence
archived_significant_claim_refs[]
```

## 210R.2 Do not persist duplicate

- item definition;
- item name;
- condition;
- equipped state;
- physical location;
- quantity;
- morale;
- relationship;
- memory text;
- death state.

## 210R.3 Old save

Source says:
- no belongings.

Accept.

Initialize:
- no claims.

## 210R.4 Existing equipped items

Remain:
- equipped;
- unclaimed.

Do not auto-personalize old gear.

## 210R.5 Existing inventory

Unclaimed.

## 210R.6 Future acquisition

Normal rules.

## 210R.7 Restore order

After:
- item instances;
- survivors;
- equipment;
- condition
and before:
- dependent UI/Plan206 reconciliation
or two-phase restore.

## 210R.8 Missing item

If claim says owned but item absent:
- reconcile against removal history.

## 210R.9 Missing survivor

If survivor died:
- Plan 206 estate reconciliation.

If deleted/corrupt:
- safe fallback.

## 210R.10 Duplicate claim

Integrity failure.

## 210R.11 Multiple owner claim

Integrity failure.

## 210R.12 Stack changed

Claim must resolve to stable instance.

## 210R.13 Template removed

Claim remains with fallback display role.

## 210R.14 Significant archived claim

May persist after item destruction for memorial/history.

Separate:
- active claim;
- archived provenance.

## 210R.15 Restore idempotence

No repeated:
- morale;
- relation;
- memory;
- inheritance;
- conflict
effects.

### 210R DoD

Save data contains only personal-claim and provenance state, old saves remain unchanged, and restore reconciles against canonical item/survivor truth without replaying consequences.

---

# TASK 210S — Determinism & Exploit Prevention

# 210S.0 Goal

Prevent item duplication, gift farming, favorite toggling, inheritance farming and attachment rerolls.

## 210S.1 Acquisition generation

Seeded if procedural.

## 210S.2 Claim creation

Deterministic.

## 210S.3 Sentimental salience

Derived from:
- event;
- relationships;
- traits;
- history.

No reload reroll.

## 210S.4 Favorite selection

Deterministic if automatic.

## 210S.5 Gift acceptance

If stochastic:
- canonical social RNG.

No second roll here.

## 210S.6 Gift relationship farm

Same item cycling:
- diminishing/no repeated full reward.

## 210S.7 Favorite toggle farm

No direct morale effect from toggling.

## 210S.8 Inheritance farm

Death is canonical and irreversible.
One estate transfer.

## 210S.9 Theft recovery farm

Repeated steal/return:
- Plan 202/relations anti-farm.

Belonging effects use stable episode.

## 210S.10 Repair farm

Repeated small damage/repair:
- no repeated sentimental bonuses.

## 210S.11 Acquisition spam

Common crafted items should not automatically become personal.

## 210S.12 Collector farm

Quest counts unique meaningful claims, not transfer churn.

## 210S.13 Personal-protection exploit

Player cannot mark all valuable combat resources personal to bypass:
- raid loss;
- emergency consumption;
- economy rules
unless personal-property policy intentionally allows.

## 210S.14 Capacity exploit

No arbitrary “max 10” cycling rewards.

## 210S.15 Stable IDs

No GUID.

## 210S.16 No wall clock

### 210S DoD

Belongings cannot duplicate physical items or generate infinite morale, relationship, quest or inheritance value through reversible bookkeeping operations.

---

# TASK 210T — Performance & State Growth

# 210T.0 Goal

Support long campaigns with many survivors without per-frame personal-item overhead.

## 210T.1 Index claims by

- survivor;
- item instance.

## 210T.2 O(1) claim lookup

## 210T.3 Event-driven updates

On:
- claim;
- transfer;
- item removal;
- condition threshold;
- death;
- memory event.

## 210T.4 No per-frame condition polling

Subscribe to condition events or daily threshold checks.

## 210T.5 No daily sentiment growth writes

Use:
- lazy age;
- milestones.

## 210T.6 Active claims

Expected small.

## 210T.7 100 survivors × 10 claims synthetic

Benchmark.

## 210T.8 400-day history

Compact.

## 210T.9 Transfer history

Keep significant chain.

## 210T.10 UI

Load survivor subset on demand.

## 210T.11 Item filters

Index owner.

## 210T.12 State-size budget

Set explicit.

### 210T DoD

Belonging processing scales with actual claim/transfer events rather than survivor count × inventory count × frame count.

---

# TASK 210U — Long-Horizon Balance & Story Simulation

# 210U.0 Goal

Prove personal effects deepen identity without becoming inventory clutter or permanent-stat optimization.

## 210U.1 30-day no-belongings scenario

Behavioral parity.

## 210U.2 30-day new shelter

A few meaningful claims emerge.

Track:
- claims/survivor;
- UI actions;
- morale events.

## 210U.3 120-day normal campaign

Track:
- gifts;
- attachment;
- item damage;
- loss;
- inherited effects.

## 210U.4 180-day high-casualty campaign

Plan 206 integration.

Verify:
- estate transfers;
- no duplicates;
- grief double-count controls.

## 210U.5 400-day lineage campaign

Track:
- inherited chains;
- memorial items;
- owner history;
- save size.

## 210U.6 Zero personal items

No penalty.

Personal items are enrichment, not mandatory baseline need.

## 210U.7 One cherished item

Loss meaningful.

## 210U.8 Ten low-salience items

No 10× morale.

## 210U.9 Favorite

No free permanent multiplier.

## 210U.10 Gift chain

A→B→C.

Canonical item remains one instance.

## 210U.11 Loan

Ownership unchanged.

## 210U.12 Theft

Known thief triggers Plan 202 once.

## 210U.13 Unknown theft

No fabricated culprit.

## 210U.14 Recovery

Same item restored.

## 210U.15 Damage/repair

No sentiment farming.

## 210U.16 Death

Estate exactly once.

## 210U.17 No heir

Fallback safe.

## 210U.18 Memorial item

No duplicate physical copy.

## 210U.19 Stackable item

Split semantics.

## 210U.20 Bulk sale

Protected item excluded.

## 210U.21 Emergency requisition

Policy works.

## 210U.22 Old save

No claims suddenly appear.

---

# TASK 210V — Testing & CI

# 210V.0 Goal

Make personal-property authority, item identity and transfer integrity continuously verifiable.

## 210V.1 Data integrity

Validate:
- template IDs;
- item definition/tag refs;
- survivor refs;
- active item-instance refs;
- one-owner invariant;
- memory refs;
- localization.

## 210V.2 Selftest

Create:

```text
--personal-belongings-selftest
```

## 210V.3 Selftest cases

At least:

1. no belongings parity;
2. create claim;
3. claim stack split;
4. equip personal item;
5. unequip;
6. gift accepted;
7. gift refused;
8. loan;
9. theft known;
10. theft unknown;
11. loss;
12. destruction;
13. recovery;
14. favorite;
15. condition threshold;
16. repair;
17. inheritance;
18. no-heir estate;
19. memorialization;
20. old save;
21. save/load;
22. no duplicate item;
23. no duplicate relation/morale effect;
24. headless.

## 210V.4 Source-scan authority gate

Detect:
- duplicate item list/container;
- duplicate `condition`;
- duplicate item name/definition fields as truth;
- direct morale mutation;
- direct relation mutation;
- direct death distribution;
- direct conflict creation;
- per-frame belonging scan;
- unseeded RNG.

## 210V.5 Content acceptance

Keepsake/template ladder:

```text
DISCOVERED
LOADED
REGISTERED
ITEM_MATCH_FOUND
ACQUISITION_PATH_FOUND
CLAIM_CREATED
MEANINGFUL_EVENT_PRODUCED
DOWNSTREAM_CONSUMER_OBSERVED
```

## 210V.6 Dead-template gate

No template without:
- canonical item;
- acquisition path.

## 210V.7 Ownership integrity gate

Every active claim:
- one live item;
- one owner.

## 210V.8 Item uniqueness gate

Physical item count unchanged by ownership transfer.

## 210V.9 Condition authority gate

## 210V.10 Death/inheritance exactly-once gate

## 210V.11 Relations exactly-once gate

## 210V.12 Psychology exactly-once gate

## 210V.13 Conflict exactly-once gate

## 210V.14 Deterministic golden fixtures

Fixed survivor/item/event:
- exact salience/claim result.

## 210V.15 Performance benchmark

## 210V.16 Generated docs

Create:
- `PERSONAL_BELONGINGS_ARCHITECTURE.md`;
- `PERSONAL_BELONGINGS_AUTHORITY_MATRIX.md`;
- `PERSONAL_PROPERTY_STATE_CONTRACT.md`;
- `KEEPSAKE_TEMPLATE_MATRIX.md`;
- `BELONGING_TRANSFER_MATRIX.md`;
- `BELONGING_SENTIMENT_MATRIX.md`;
- `BELONGING_DEATH_INHERITANCE_MATRIX.md`;
- `BELONGING_INVENTORY_POLICY_MATRIX.md`;
- `BELONGING_MIGRATION_MATRIX.md`;
- `PERSONAL_BELONGINGS_BALANCE_REPORT.md`;
- `ADR_PERSONAL_BELONGINGS_OVER_CANONICAL_ITEMS.md`;
- `ADR_SURVIVOR_PERSONAL_PROPERTY_SEMANTICS.md`;
- `ADR_SENTIMENTAL_VALUE_VS_MORALE.md`;
- `ADR_PERSONAL_BELONGINGS_AND_DEATH_LEGACY.md`.

### 210V DoD

Every personal effect can be traced to one canonical physical item, one owner claim, one transfer/loss history and exactly-once downstream consequences.

---

# TASK 210W — Narrative Events & Quest Hooks

# 210W.0 Goal

Surface meaningful material stories without rewarding inventory churn.

## 210W.1 Semantic engine events

Candidate:

```text
personal_belonging_acquired
personal_belonging_gifted
personal_belonging_lost
personal_belonging_destroyed
personal_belonging_recovered
personal_belonging_inherited
personal_belonging_memorialized
personal_favorite_changed
```

## 210W.2 Source narrative names

- The Keepsake;
- The Gift;
- The Inheritance;
- The Loss;
- The Theft;
- The Favorite;
- The Memory;
- The Attachment.

Treat as authored narrative candidates.

## 210W.3 No event for every sentiment threshold tick

## 210W.4 “The Memory”

Prefer:
- Plan 147/190 memory event.

## 210W.5 “The Attachment”

Can be a rare milestone:
- long-term cherished item.

## 210W.6 Plan 171 quest hooks

Expose:
- meaningful gift;
- inherited item;
- recovered stolen keepsake;
- memorialized effect;
- cherished item destroyed.

## 210W.7 Source quest ideas

- Collector;
- Generous;
- Sentimental;
- Heir;
- Curator;
- Favorite;
- Memory Keeper.

Treat as backlog.

## 210W.8 Reject raw `acquire 20 personal belongings` by default

Encourages clutter.

## 210W.9 Reject `give 10 gifts` unless unique-context anti-farm.

## 210W.10 `inherit from 5 deceased survivors`

Can incentivize death.

Prefer not.

## 210W.11 `maintain 10 items 90+ condition`

Turns identity into maintenance grind.

Prefer achievement only if desired.

## 210W.12 Better quest shapes

- recover a survivor's stolen keepsake;
- return a deceased survivor's item to family;
- repair a cherished tool;
- decide whether to memorialize or reuse an inherited weapon;
- deliver a personal letter.

### 210W DoD

Belonging narrative content celebrates specific meaningful objects and decisions rather than raw collection, gifting or death counters.

---

# TASK 210X — Advanced Legacy, Personal Craftsmanship & Sentimental Trade: Explicit Follow-On

# 210X.0 Goal

Keep speculative meta-systems out of the ownership foundation.

## 210X.1 Personal craftsmanship

Use:
- CraftingSystem;
- SkillProgression.

Could allow:
- maker marks;
- commissioned gifts.

## 210X.2 Famous keepsake

Plan 190/162.

## 210X.3 Cross-campaign legacy

Meta progression/chronicle.

DEFER.

## 210X.4 Sentimental trading

Trading a personal object is just a real item transfer with attachment consequences.

No “sentiment market price” by default.

## 210X.5 Collector NPCs

Market/quest system.

## 210X.6 Heirloom lineage

Can be Plan 190 owner history + Plan 206 estate.

## 210X.7 Museum/archive

Plan 162/190.

## 210X.8 Personal locker furniture

Shelter/storage system if useful.

## 210X.9 Display cases

UI/decor/storage follow-on.

## 210X.10 Theft market

Plan 155 black market if stolen-goods provenance exists.

## 210X.11 Insurance

Out of scope.

## 210X.12 Personal ownership doctrine

Governance follow-on.

### 210X DoD

Advanced heirloom, craftsmanship, collector and legacy features build on canonical item identity and ownership provenance rather than expanding PersonalBelongings into a second economy or inventory.

---

# 5. Core Personal-Belonging Lifecycle

```text
CANONICAL ITEM CREATED / EXISTS
        │
        ▼
PERSONAL CLAIM CREATED
        │
        ├── stored
        ├── equipped
        ├── carried
        ├── loaned
        └── memorialized
        │
        ▼
MEANINGFUL EVENT
        │
        ├── gift
        ├── damage
        ├── theft
        ├── loss
        ├── recovery
        ├── death
        └── voluntary relinquishment
        │
        ▼
CLAIM TRANSITION
        │
        ├── new owner
        ├── estate
        ├── lost
        ├── destroyed
        └── archived
        │
        ▼
CANONICAL CONSEQUENCES
```

---

# 6. Physical Item vs Personal Claim Contract

Physical item:

```text
Inventory / item instance
```

Personal meaning:

```text
PersonalBelongings claim
```

Never duplicate.

---

# 7. Ownership vs Possession Contract

Owner:
- person with personal claim.

Holder/location:
- current physical custodian.

These may differ during:
- loan;
- equipment assignment;
- storage;
- confiscation.

---

# 8. Ownership vs Equipment Contract

Equipped does not imply:
- personal ownership.

Personal ownership does not require:
- currently equipped.

---

# 9. Condition Contract

Physical condition:
- canonical item state.

Sentimental system:
- observes condition thresholds.

---

# 10. Sentiment Contract

Sentiment:
- attachment/salience.

Not:
- morale;
- market value;
- durability.

---

# 11. Favorite Contract

Favorite:
- most personally salient.

Not:
- doubled global buff.

---

# 12. Keepsake Contract

Keepsake:
- social meaning classification.

Not:
- separate item type unless canonical item catalog uses it.

---

# 13. Clothing Contract

Personal clothing remains:
- clothing/equipment item.

Claim adds ownership only.

---

# 14. Tool Contract

Personal tool remains:
- tool item.

Claim adds attachment.

---

# 15. Weapon Contract

Personal weapon remains:
- weapon item.

Combat/equipment authority unchanged.

---

# 16. Document Contract

Document content:
- document/lore authority.

Claim:
- owner/meaning.

---

# 17. Jewelry Contract

Physical jewelry:
- item authority.

No hidden jewelry inventory.

---

# 18. Gift Contract

Gift transaction:

```text
validate claim
→ validate receiver
→ move/transfer canonical item
→ transfer claim
→ emit social event
```

Atomic or rollback.

---

# 19. Theft Contract

Theft transaction:
- theft/transfer system.

Belongings:
- detects property violation.

Plan 202:
- grievance.

---

# 20. Confiscation Contract

Governance/security owns:
- legality/action.

Belongings:
- property consequence.

---

# 21. Inheritance Contract

Plan 206:
- estate/distribution.

Belongings:
- estate contents + inherited provenance.

---

# 22. Memorial Contract

MemorialSystem:
- memorial placement/remembrance.

Belongings:
- identifies personal effect and history.

---

# 23. Item Lore Contract

Plan 190:
- item history.

Belongings:
- emits ownership-transfer events.

---

# 24. Personal Memory Contract

Plan 147:
- survivor memory.

Belongings:
- references memory IDs.

---

# 25. Memory Decay Contract

Plan 185:
- memory salience.

Belongings:
- may consume resulting attachment modifier.

No duplicate memory decay.

---

# 26. Morale Contract

Needs/psychology:
- owns morale/stress.

Belongings:
- emits contextual event.

---

# 27. Relationship Contract

Relations:
- owns affinity/trust.

Belongings:
- emits gift/property events.

---

# 28. Conflict Contract

Plan 202:
- owns grievance/escalation.

Belongings:
- emits property-violation trigger.

---

# 29. Autonomy Contract

Survivor may:
- accept/refuse gift;
- resist confiscation;
- object to sale
through Plan 144/governance.

---

# 30. Inventory Protection Contract

Personal items default:
- exclude from bulk consume/sell/scrap.

Override:
- explicit.

---

# 31. Stack Contract

Claimed units require stable identity.

No merge that destroys owner metadata.

---

# 32. Old-Save Contract

Old inventory:
- unclaimed.

Old worn gear:
- unclaimed.

No retroactive personal ownership.

---

# 33. Persistence Matrix

| Fact | Owner |
|---|---|
| item definition | item catalog |
| item instance | Inventory |
| stack/quantity | Inventory |
| physical location | Inventory/storage |
| condition | item/EquipmentCondition |
| equipped state | WornGear/equipment |
| personal owner claim | PersonalBelongings |
| sentimental salience | PersonalBelongings |
| favorite | PersonalBelongings |
| transfer provenance | PersonalBelongings / item history refs |
| relationship | SurvivorRelations |
| morale/stress | Needs/mental health |
| personal memory | Plan 147 |
| memory decay | Plan 185 |
| item lore/history | Plan 190 |
| conflict | Plan 202 |
| estate/inheritance orchestration | Plan 206 |
| memorial | MemorialSystem |

---

# 34. Old-Save Migration Matrix

```text
feature_activation_day = current_day
personal_claims = []
transfer_history = []
archived_personal_effects = []
```

Do not infer ownership from:
- equipped status;
- long possession;
- item rarity.

Future claims only.

---

# 35. Exactly-Once IDs

Examples:

```text
claim:<item_instance>
transfer:<item_instance>:<from>:<to>:<sequence>
loss:<item_instance>:<source_event>
inheritance:<death_event>:<item_instance>:<heir>
memory-link:<item_instance>:<memory_event>
```

---

# 36. Failure Injection Matrix

## N210.1 Personal belonging stores its own physical `condition`
Expected: condition authority gate fails.

## N210.2 Gift creates new item in receiver inventory without removing original
Expected: uniqueness gate fails.

## N210.3 Personal weapon disappears from Inventory and exists only in belonging list
Expected: shadow-inventory gate fails.

## N210.4 Favorite toggle gives repeated morale boost
Expected: anti-farm gate fails.

## N210.5 Ten keepsakes produce ten linear daily morale bonuses
Expected: sentiment-vs-morale gate fails.

## N210.6 DeathLegacy and PersonalBelongings both distribute same item
Expected: inheritance authority/idempotence fails.

## N210.7 Theft creates grievance with invented thief
Expected: source-event gate fails.

## N210.8 Bulk sell removes cherished item without warning/policy
Expected: personal-protection gate fails.

## N210.9 Old save auto-claims every equipped weapon
Expected: migration parity fails.

## N210.10 Stack merge erases personal ownership
Expected: stack integrity fails.

## N210.11 Item destroyed but claim remains active
Expected: reconciliation fails.

## N210.12 Gift directly writes relationship affinity
Expected: Relations authority gate fails.

---

# 37. Determinism Contract

Same:

```text
item instance
+ acquisition source
+ survivor
+ relationship/memory context
+ attachment policy
```

must produce same:
- claim;
- initial salience;
- favorite eligibility;
- transfer outcome context.

RNG only where source procedural content uses canonical seeded RNG.

---

# 38. Long-Horizon Metrics

Track:

```text
active personal claims
claims per survivor
favorites
gifts
refused gifts
loans
thefts
losses
recoveries
destroyed effects
inheritances
memorialized items
relationship events
psychology events
conflict triggers
stack splits
protected bulk-action skips
orphan claims
duplicate item detections
state bytes
processing time
```

---

# 39. Balance Guardrails

Personal belongings should make:
- individuals memorable;
- death tangible;
- gifts meaningful;
- property violations consequential.

They should not make:
- hoarding optimal;
- every item personal;
- the player manage 10 slots per survivor.

---

# 40. Sentiment Guardrails

One cherished item can matter more than:
- ten trivial items.

Attachment quality > quantity.

---

# 41. Gift Guardrails

Gift meaning depends on:
- relationship;
- sacrifice;
- context.

Not:
- repetitive transaction count.

---

# 42. Inheritance Guardrails

Inheritance should deepen grief/memory.

It should not:
- reward survivor death mechanically.

---

# 43. Theft Guardrails

Theft is severe because:
- property + sentiment.

Do not randomly fire it without action support.

---

# 44. Condition Guardrails

Physical deterioration should not create:
- constant notification noise.

Use thresholds.

---

# 45. UI Acceptance

## Survivor view
- personal effects;
- meaning;
- owner;
- favorite.

## Inventory
- owner badge;
- protection.

## Gift
- transfer intent/consent.

## Death
- Plan 206 estate surface.

## History
- significant provenance only.

---

# 46. Accessibility

- favorite has text label;
- owner not color-only;
- protection warning readable;
- keyboard/controller;
- no hover-only provenance;
- text scaling.

---

# 47. Localization

Template names/meaning:
- localization keys.

No localized strings persisted.

---

# 48. Content Acceptance

Keepsake pipeline:

```text
DISCOVERED
LOADED
REGISTERED
CANONICAL_ITEM_MATCHED
ACQUISITION_PATH_REACHED
CLAIM_CREATED
TRANSFER/LOSS_PATH_REACHED
CONSUMER_EFFECT_OBSERVED
```

---

# 49. Reachability

Every template:
- canonical item match;
- acquisition fixture.

Every transfer type:
- real API.

Every consequence:
- real consumer.

---

# 50. Performance Guardrails

- O(1) claim lookup;
- no per-frame;
- no full inventory scan daily;
- threshold event subscription;
- bounded history;
- small claim set.

---

# 51. CI / Gate Set

Recommended:

```text
personal_belongings_authority_matrix
personal_belongings_no_shadow_inventory
personal_belongings_item_uniqueness
personal_belongings_condition_authority
personal_belongings_stack_integrity
personal_belongings_equipment_integrity
personal_belongings_transfer_atomicity
personal_belongings_relations_authority
personal_belongings_morale_authority
personal_belongings_conflict_authority
personal_belongings_death_legacy_authority
personal_belongings_inheritance_idempotence
personal_belongings_old_save
personal_belongings_determinism
personal_belongings_antifarm
personal_belongings_long_horizon
personal_belongings_ui_access
```

---

# 52. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --personal-belongings-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 53. Recommended Commit Breakdown

```text
210A-1 inventory/item-instance audit
210A-2 canonical-item ADR
210A-3 personal-property ADR
210A-4 sentiment-vs-morale ADR
210A-5 death-legacy ADR
210A-6 claim DTO/state
210A-7 ownership indexes/reconciliation
210A-8 docs/tests

210B-1 keepsake template schema
210B-2 canonical item/tag binding
210B-3 social-role categories
210B-4 memory/lore hooks
210B-5 template integrity
210B-6 generated matrix

210C-1 acquisition intent API
210C-2 discovery claim path
210C-3 assignment claim path
210C-4 starting keepsakes
210C-5 stack split integration
210C-6 deterministic salience
210C-7 provenance
210C-8 tests/docs

210D-1 attachment model
210D-2 meaning tags
210D-3 event-based attachment growth
210D-4 condition-independent semantics
210D-5 favorite salience semantics
210D-6 inherited meaning
210D-7 no-daily-growth regression
210D-8 docs

210E-1 psychology adapter
210E-2 acquisition/loss/recovery events
210E-3 bounded possession comfort if approved
210E-4 no-linear-stacking
210E-5 grief/inheritance double-count guard
210E-6 tests/docs

210F-1 gift transfer transaction
210F-2 giver/receiver eligibility
210F-3 autonomy acceptance/refusal
210F-4 inventory transfer
210F-5 claim transfer
210F-6 Relations adapter
210F-7 anti-gift-farm
210F-8 rollback/tests/docs

210G-1 item-loss subscription
210G-2 destruction
210G-3 lost/recovery state
210G-4 theft adapter
210G-5 confiscation/requisition
210G-6 sale/scrap/consume protection
210G-7 loan semantics
210G-8 tests/docs

210H-1 favorite model
210H-2 survivor-driven selection
210H-3 no-2x-buff regression
210H-4 switching rules
210H-5 UI/history
210H-6 tests/docs

210I-1 Plan-206 estate query
210I-2 estate_pending state
210I-3 inheritance transaction
210I-4 no-heir fallback
210I-5 inherited salience
210I-6 memorial handoff
210I-7 exactly-once death transfer
210I-8 tests/docs

210J-1 Plan-190 item-history adapter
210J-2 Plan-147 memory refs
210J-3 Plan-185 salience coupling
210J-4 MemorialSystem adapter
210J-5 owner-history compaction
210J-6 docs/tests

210K-1 Relations event adapter
210K-2 gift context
210K-3 theft/return context
210K-4 promise/favor interaction
210K-5 no-affinity-direct-write gate
210K-6 anti-farm tests

210L-1 Plan-202 property-violation trigger
210L-2 known/unknown thief semantics
210L-3 confiscation dispute
210L-4 inheritance dispute hook
210L-5 recovery/restitution
210L-6 no-direct-conflict gate

210M-1 Plan-144 autonomy integration
210M-2 consent/refusal
210M-3 confiscation governance
210M-4 emergency requisition policy
210M-5 personal-property protection
210M-6 override consequences
210M-7 tests/docs

210N-1 inventory owner badge/query
210N-2 stack split/merge rules
210N-3 WornGear persistence
210N-4 loaned equipment
210N-5 repair/crafting/scrap/sell gates
210N-6 expedition carry/loss
210N-7 bulk action exclusions
210N-8 integrity tests

210O-1 condition threshold adapter
210O-2 damage events
210O-3 repair events
210O-4 destroy/archive
210O-5 replacement identity
210O-6 no-condition-duplicate gate
210O-7 tests/docs

210P-1 expedition discovery hook
210P-2 Plan-200 quest reward hook
210P-3 crafting commission hook
210P-4 new-survivor starting item generation
210P-5 Plan-191 identification interaction
210P-6 Plan-190 provenance
210P-7 tests/docs

210Q-1 survivor Personal Effects UI
210Q-2 inventory owner/protection badges
210Q-3 gift action
210Q-4 Plan-206 inheritance deep-link
210Q-5 filters/history
210Q-6 accessibility/localization
210Q-7 tutorial/snapshots
210Q-8 clutter-budget playtest

210R-1 persistence schema
210R-2 old-save empty claims
210R-3 restore ordering
210R-4 missing item/survivor reconciliation
210R-5 duplicate/multi-owner gates
210R-6 template migration
210R-7 no-side-effect replay
210R-8 docs/tests

210S-1 seeded procedural acquisition
210S-2 stable sentiment
210S-3 gift/favorite/inheritance anti-farm
210S-4 theft/recovery anti-farm
210S-5 repair anti-farm
210S-6 protection-policy exploit tests
210S-7 stable IDs

210T-1 claim indexes
210T-2 event-driven condition updates
210T-3 lazy attachment age
210T-4 100-survivor benchmark
210T-5 history compaction
210T-6 state-size report

210U-1 30-day parity
210U-2 30-day normal
210U-3 120-day social sim
210U-4 180-day mortality sim
210U-5 400-day lineage soak
210U-6 transfer/theft/repair scenarios
210U-7 report

210V-1 selftest
210V-2 source-scan authority gates
210V-3 content acceptance
210V-4 failure fixtures
210V-5 deterministic goldens
210V-6 performance/state-size gates
210V-7 final ship/no-ship report

210W-1 semantic event hooks
210W-2 Plan-171 quest surface
210W-3 narrative event budget
210W-4 anti-grind quest review

210X-1 advanced legacy/craftsmanship/trading disposition
```

---

# 54. Risk Register

## R210.1 Shadow inventory

Mitigation:
- canonical item-instance reference only.

## R210.2 Duplicate condition

Mitigation:
- read-only canonical condition.

## R210.3 Permanent morale stacking

Mitigation:
- event/context psychology adapter.

## R210.4 Gift farming

Mitigation:
- stable transfer provenance;
- diminishing repeated interactions.

## R210.5 Death duplicates items

Mitigation:
- Plan 206 authority;
- atomic transfer;
- exactly-once estate IDs.

## R210.6 Stack identity loss

Mitigation:
- mandatory split for claimed unit.

## R210.7 Personal items block essential resource use

Mitigation:
- explicit emergency requisition/governance policy.

## R210.8 Personal belongings become micromanagement

Mitigation:
- soft quantity target;
- survivor-driven ownership/favorites;
- integrated UI.

## R210.9 Theft invents social state

Mitigation:
- real property violation only.

## R210.10 Old saves change behavior

Mitigation:
- no retroactive claims.

---

# 55. Acceptance Checklist

## P0

- [ ] Inventory item-instance model audited
- [ ] stack semantics audited
- [ ] item definition catalog audited
- [ ] WornGear audited
- [ ] EquipmentConditionSystem audited
- [ ] inventory add/remove/transfer audited
- [ ] consume path audited
- [ ] scrap/disassemble audited
- [ ] sell/trade audited
- [ ] crafting outputs audited
- [ ] expedition provenance audited
- [ ] survivor equipment APIs audited
- [ ] storage/room systems audited
- [ ] survivor death lifecycle audited
- [ ] Plan 206 audited
- [ ] SurvivorRelationsSystem audited
- [ ] Plan 147 audited
- [ ] Plan 185 audited
- [ ] Plan 190 audited
- [ ] MemorialSystem audited
- [ ] Needs/mental health audited
- [ ] Plan 202 audited
- [ ] Plan 144 audited
- [ ] save order audited
- [ ] inventory/survivor UI audited
- [ ] authority matrix published
- [ ] canonical-item ADR
- [ ] property ADR
- [ ] sentiment-vs-morale ADR
- [ ] death-legacy ADR
- [ ] baseline captured

## 210A — Claim State

- [ ] PersonalBelongingClaim DTO
- [ ] item instance ref mandatory
- [ ] owner survivor ref
- [ ] no duplicate itemName
- [ ] no duplicate itemId truth
- [ ] no duplicate condition
- [ ] category from canonical tags where possible
- [ ] social category override only if necessary
- [ ] claim state
- [ ] physical location separate
- [ ] sentimental salience
- [ ] attachment band
- [ ] meaning tags
- [ ] memory refs
- [ ] one favorite semantics
- [ ] inherited provenance
- [ ] processed event IDs
- [ ] versioned capture
- [ ] side-effect-free restore
- [ ] reconciliation

## 210B — Templates

- [ ] versioned keepsake templates
- [ ] eligible item tags
- [ ] specific item override
- [ ] localization keys
- [ ] meaning tags
- [ ] salience range
- [ ] acquisition context
- [ ] memory-link policy
- [ ] rarity
- [ ] narrative hooks
- [ ] no duplicate lore truth
- [ ] keepsake canonical item
- [ ] clothing canonical item
- [ ] tool canonical item
- [ ] weapon canonical item
- [ ] memento canonical item
- [ ] document content external
- [ ] jewelry canonical item
- [ ] 30+ not treated as quota
- [ ] unique policy
- [ ] templates do not create item directly
- [ ] integrity
- [ ] matrix

## 210C — Acquisition

- [ ] expedition loot enters Inventory first
- [ ] personal claim after item creation
- [ ] authored auto-claim only
- [ ] shared valuable loot not auto-claimed
- [ ] crafter does not auto-own everything
- [ ] assignment path
- [ ] new-game starting keepsakes
- [ ] old save none
- [ ] acquisition event
- [ ] claim eligibility
- [ ] stack split
- [ ] Inventory owns split
- [ ] no arbitrary hard max unless justified
- [ ] clutter budget
- [ ] source-driven salience
- [ ] no reload reroll
- [ ] stable provenance

## 210D — Sentiment

- [ ] sentiment != morale
- [ ] family provenance
- [ ] gift provenance
- [ ] inheritance provenance
- [ ] survival-event provenance
- [ ] long-use milestone
- [ ] authored memory
- [ ] vocation meaning
- [ ] no generic daily growth
- [ ] lazy/milestone attachment if time-based
- [ ] damage does not force sentiment drop
- [ ] destruction event
- [ ] favorite not 2x buff
- [ ] rarity != sentiment
- [ ] market value != sentiment
- [ ] condition != sentiment
- [ ] memory refs
- [ ] no generic decay unless justified
- [ ] inheritance not guaranteed higher
- [ ] negative sentiment deferred/explicit

## 210E — Psychology

- [ ] psychology authority identified
- [ ] acquisition context event
- [ ] loss event
- [ ] destruction event
- [ ] recovery event
- [ ] inheritance event
- [ ] no direct persistent morale add
- [ ] bounded contextual comfort if used
- [ ] no linear stacking
- [ ] favorite salience only
- [ ] loss uses attachment/context
- [ ] damage threshold only
- [ ] recovery relief
- [ ] inheritance mixed grief/comfort
- [ ] memorial integration
- [ ] death grief double-count guard

## 210F — Gifts

- [ ] typed transfer intent
- [ ] giver owns item
- [ ] item exists
- [ ] equipped-item handling
- [ ] receiver valid
- [ ] transferability
- [ ] autonomy acceptance
- [ ] player/survivor gift same transaction
- [ ] canonical physical transfer
- [ ] claim transfer
- [ ] receiver sentiment recalculated
- [ ] giver sacrifice context
- [ ] Relations event
- [ ] no direct affinity
- [ ] refusal context-specific
- [ ] anti-cycle farming
- [ ] gift-back handling
- [ ] rollback
- [ ] exactly once

## 210G — Loss/Theft

- [ ] item removal sources identified
- [ ] claim subscriber
- [ ] destroy terminal
- [ ] lost state
- [ ] recovery same instance
- [ ] rematerialization lineage if needed
- [ ] theft requires action/event
- [ ] known thief Plan-202 trigger
- [ ] unknown thief no invented culprit
- [ ] confiscation governance
- [ ] emergency requisition policy
- [ ] personal consumables protected
- [ ] scrapping protected
- [ ] selling protected
- [ ] loan distinct
- [ ] relation/conflict consequence delegated
- [ ] recovery can resolve via consumers

## 210H — Favorite

- [ ] one favorite MVP
- [ ] only owned/living item
- [ ] UI/narrative prominence
- [ ] no unconditional 2x morale
- [ ] stronger loss via psychology context
- [ ] switching not effect reset
- [ ] cooldown if needed
- [ ] autonomy choice considered
- [ ] player mind-control avoided
- [ ] deterministic automatic selection
- [ ] no favorite event spam

## 210I — Death/Inheritance

- [ ] Plan 206 owns death workflow
- [ ] estate query
- [ ] estate_pending
- [ ] no local auto-distribution
- [ ] Plan-206 disposition
- [ ] named heir if supported
- [ ] relation-based heir if supported
- [ ] canonical item transfer
- [ ] heir claim
- [ ] sentiment recalculated
- [ ] inherited provenance derived
- [ ] deceased claim archived
- [ ] memorial handoff
- [ ] death grief double-count guarded
- [ ] exactly-once inheritance
- [ ] simultaneous death deterministic
- [ ] no-heir fallback
- [ ] old save none

## 210J — Memory/Lore/Memorial

- [ ] Plan 190 ownership history
- [ ] ownership event adapter
- [ ] Plan 147 memory refs
- [ ] no description duplication
- [ ] MemorialSystem display
- [ ] memorial physical item canonical
- [ ] Plan 185 memory decay
- [ ] historical provenance persists
- [ ] anniversary only via memory/calendar
- [ ] document text external
- [ ] famous keepsake follow-on
- [ ] owner chain
- [ ] routine history compacted

## 210K — Relations

- [ ] Relations authority preserved
- [ ] gift semantic context
- [ ] relationship delta computed there
- [ ] no relation state in transfer
- [ ] inheritance context
- [ ] theft delegated
- [ ] borrowed-item return if supported
- [ ] refused gift contextual
- [ ] favor/promise integration
- [ ] no relation farming
- [ ] sacrifice vs market value distinction

## 210L — Conflict

- [ ] theft source trigger
- [ ] item subject ref
- [ ] attachment severity hint
- [ ] Plan 202 owns grievance
- [ ] confiscation dispute
- [ ] gift refusal not automatic conflict
- [ ] inheritance dispute only if source event
- [ ] blame requires cause
- [ ] damage trigger only with responsible event
- [ ] recovery/restitution
- [ ] no direct conflict creation

## 210M — Autonomy/Policy

- [ ] Plan 144 audited
- [ ] assignment consent
- [ ] gift refusal
- [ ] forced favorite gifting guarded
- [ ] confiscation governance
- [ ] emergency requisition
- [ ] loan return
- [ ] sale protection
- [ ] scrap protection
- [ ] explicit override
- [ ] override consequences delegated
- [ ] common ration not default keepsake
- [ ] scarcity policy
- [ ] governance property doctrine if real
- [ ] UI reason trace

## 210N — Inventory/Equipment

- [ ] personal item may remain shared locker physically
- [ ] owner badge
- [ ] incompatible stack merge blocked
- [ ] claimed stack split
- [ ] non-stackable path
- [ ] claim persists while equipped
- [ ] claim persists unequipped
- [ ] other survivor equip requires loan/transfer
- [ ] condition read canonical
- [ ] repair keeps owner
- [ ] crafting ingredient protection
- [ ] scrapping protection
- [ ] selling protection
- [ ] expedition carry
- [ ] storage integration
- [ ] filters
- [ ] bulk action exclusion
- [ ] removal reconciliation
- [ ] no orphan claim

## 210O — Condition

- [ ] physical condition authority preserved
- [ ] threshold subscription
- [ ] damaged threshold
- [ ] badly-damaged threshold
- [ ] restored threshold
- [ ] destroyed threshold
- [ ] no daily condition event
- [ ] sentiment response contextual
- [ ] repair meaning optional
- [ ] destroyed claim archived
- [ ] replacement is new identity
- [ ] rebuild identity policy
- [ ] consumable disappearance
- [ ] no condition copied to save

## 210P — Discovery/Crafting

- [ ] expedition discovery hook
- [ ] canonical loot first
- [ ] claim from story context
- [ ] Plan 200 personal quest hook
- [ ] crafting commission
- [ ] naming only if real
- [ ] clothing integration
- [ ] starting keepsake
- [ ] seeded procedural generation
- [ ] no reroll
- [ ] rarity does not guarantee sentiment
- [ ] uniqueness
- [ ] Plan 191 identification interaction
- [ ] Plan 190 provenance

## 210Q — UI

- [ ] survivor Personal Effects section
- [ ] item name
- [ ] owner
- [ ] meaning
- [ ] attachment band
- [ ] canonical condition
- [ ] favorite
- [ ] provenance
- [ ] no duplicate item stats
- [ ] shared inventory owner badge
- [ ] gift action
- [ ] favorite display
- [ ] Plan 206 inheritance UI
- [ ] significant log only
- [ ] filters
- [ ] sentiment bands
- [ ] meaning explanation
- [ ] sale/scrap warning
- [ ] bulk sale exclusion
- [ ] bulk scrap exclusion
- [ ] emergency override
- [ ] tutorial
- [ ] no hover-only
- [ ] keyboard/controller
- [ ] screen-reader
- [ ] text scale
- [ ] favorite text label
- [ ] clutter budget
- [ ] no fake personal weight

## 210R — Persistence

- [ ] schema version
- [ ] feature activation day
- [ ] personal claims
- [ ] transfer provenance
- [ ] processed events
- [ ] transfer sequence
- [ ] archived significant refs
- [ ] no item-definition duplicate
- [ ] no item-name duplicate
- [ ] no condition duplicate
- [ ] no equipped-state duplicate
- [ ] no physical-location duplicate
- [ ] no quantity duplicate
- [ ] no morale duplicate
- [ ] no relation duplicate
- [ ] no memory text
- [ ] no death state
- [ ] old save no claims
- [ ] old equipped gear remains unclaimed
- [ ] old inventory unclaimed
- [ ] future claims normal
- [ ] restore ordering
- [ ] missing item reconciliation
- [ ] missing survivor reconciliation
- [ ] duplicate claim fails
- [ ] multi-owner fails
- [ ] stack ref stable
- [ ] template removal fallback
- [ ] active vs archived claim distinction
- [ ] restore no side effects

## 210S — Determinism/Exploit

- [ ] seeded procedural acquisition only
- [ ] deterministic claim
- [ ] deterministic salience
- [ ] no reload reroll
- [ ] favorite stable
- [ ] gift acceptance uses canonical RNG if any
- [ ] gift cycling blocked
- [ ] favorite toggle farm impossible
- [ ] inheritance exactly once
- [ ] theft recovery farm delegated/blocked
- [ ] repair farm blocked
- [ ] crafted-item claim spam blocked
- [ ] collector counts unique meaningful claims
- [ ] personal protection cannot bypass unrelated game rules
- [ ] no cap-cycling rewards
- [ ] no GUID
- [ ] no wall clock

## 210T — Performance

- [ ] index by survivor
- [ ] index by item
- [ ] O(1) lookup
- [ ] event-driven
- [ ] no per-frame condition poll
- [ ] no daily sentiment writes
- [ ] 100×10 synthetic benchmark
- [ ] 400-day history compact
- [ ] significant transfer chain
- [ ] UI on demand
- [ ] owner filter index
- [ ] state-size budget

## 210U/V — Simulations/CI

- [ ] 30-day parity
- [ ] 30-day new shelter
- [ ] 120-day normal
- [ ] 180-day mortality
- [ ] 400-day lineage
- [ ] zero belongings no penalty
- [ ] one cherished loss meaningful
- [ ] ten low-salience no 10x buff
- [ ] favorite no free multiplier
- [ ] A→B→C gift chain one item
- [ ] loan
- [ ] known theft one Plan-202 trigger
- [ ] unknown theft no culprit
- [ ] recovery same item
- [ ] damage/repair no farm
- [ ] death estate once
- [ ] no heir
- [ ] memorial no physical duplicate
- [ ] stack split
- [ ] bulk sale protection
- [ ] emergency requisition
- [ ] old save
- [ ] data integrity
- [ ] personal-belongings selftest
- [ ] source-scan authority gate
- [ ] content acceptance
- [ ] dead-template gate
- [ ] ownership integrity
- [ ] item uniqueness
- [ ] condition authority
- [ ] inheritance exactly once
- [ ] Relations exactly once
- [ ] psychology exactly once
- [ ] conflict exactly once
- [ ] deterministic goldens
- [ ] performance
- [ ] generated docs
- [ ] verify-fast

## 210W/X — Narrative/Follow-On

- [ ] bounded semantic belonging events
- [ ] source narrative names treated as content
- [ ] no sentiment-tick events
- [ ] Memory event uses Plan 147/190 where appropriate
- [ ] Attachment milestone rare
- [ ] Plan 171 owns quests
- [ ] raw collection quest reviewed/rejected
- [ ] gift-count grind reviewed/rejected
- [ ] inheritance-count goal reviewed/rejected
- [ ] condition-maintenance grind reviewed
- [ ] meaningful story quests preferred
- [ ] craftsmanship uses Crafting/SkillProgression
- [ ] famous keepsake uses Plan 190/162
- [ ] cross-campaign legacy deferred
- [ ] no sentimental-price subsystem
- [ ] collector NPC uses market/quest
- [ ] heirloom lineage uses Plan 190/206
- [ ] museum/archive uses Plan 162/190
- [ ] personal locker follows storage
- [ ] display cases follow UI/decor
- [ ] stolen-goods market uses Plan 155
- [ ] insurance out of scope
- [ ] ownership doctrine follows governance

---

# 56. Ship / No-Ship Gate

**SHIP** only if:

```text
physical_item_authorities == 1
AND item_condition_authorities == 1
AND equipment_authorities == 1
AND relationship_authorities == 1
AND morale_psychology_authorities == 1
AND death_inheritance_orchestration_authorities == 1
AND interpersonal_conflict_authorities == 1
AND item_lore_authorities == 1
AND personal_memory_authorities == 1
AND personal_belongings_shadow_inventory_containers == 0
AND personal_belongings_duplicate_item_definition_truth == 0
AND personal_belongings_duplicate_condition_state == 0
AND personal_belongings_duplicate_equipped_state == 0
AND personal_belongings_direct_morale_state_mutation == false
AND personal_belongings_direct_relationship_state_mutation == false
AND personal_belongings_direct_inheritance_distribution == false
AND personal_belongings_direct_conflict_creation == false
AND active_claims_without_canonical_items == 0
AND exclusive_items_with_multiple_personal_owners == 0
AND ownership_transfers_that_duplicate_items == 0
AND claimed_stack_merges_that_erase_identity == 0
AND bulk_sale_or_scrap_bypasses_personal_protection == 0
AND linear_daily_morale_stacking_from_belongings == false
AND favorite_toggle_farm_paths == 0
AND gift_cycle_farm_paths == 0
AND inheritance_replay_paths == 0
AND theft_recovery_farm_paths == 0
AND old_save_retroactive_personal_claims == 0
AND per_frame_belonging_processing == 0
AND unseeded_belonging_rng == 0
AND dead_keepsake_templates == 0
AND personal_belongings_old_save == pass
AND personal_belongings_save_roundtrip == pass
AND personal_belongings_item_uniqueness == pass
AND personal_belongings_stack_integrity == pass
AND personal_belongings_transfer_atomicity == pass
AND personal_belongings_relations_exactly_once == pass
AND personal_belongings_psychology_exactly_once == pass
AND personal_belongings_conflict_exactly_once == pass
AND personal_belongings_inheritance_exactly_once == pass
AND personal_belongings_determinism == pass
AND personal_belongings_30_day_balance == pass
AND personal_belongings_120_day_balance == pass
AND personal_belongings_180_day_balance == pass
AND personal_belongings_400_day_soak == pass
AND personal_belongings_performance == pass
AND personal_belongings_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 57. Implementer Handoff

1. Audit canonical item-instance identity, stack behavior, WornGear, EquipmentCondition, item removal, sale, scrap, crafting and death before writing PersonalBelongings.
2. Implement personal belongings as **claims over canonical item instances**, never as a second physical inventory.
3. Remove duplicated `itemName`, condition and other item truth from persisted belonging records.
4. Make every functional keepsake/tool/weapon/clothing/document/jewelry object exist exactly once in the item authority.
5. If narrative-only items are needed, register them through a canonical item/artifact representation rather than hiding them inside PersonalBelongings.
6. Separate personal ownership from physical location and current holder.
7. Keep personal ownership when an item is equipped, unequipped, stored or temporarily loaned.
8. Split stackable items before assigning an exclusive personal claim.
9. Use sentimental salience as attachment metadata, not survivor morale.
10. Route acquisition/loss/recovery/inheritance psychological effects into the canonical mental-health/Needs authority.
11. Never give linear daily morale bonuses per belonging.
12. Make favorite status narrative/salience-focused rather than a doubled-buff slot.
13. Create claims only through real acquisition/assignment events.
14. Do not automatically claim every crafted or found item.
15. Make gift transfers transactional: canonical item movement + claim transfer + social event.
16. Respect receiver autonomy and avoid hardcoded refusal penalties.
17. Route relationship consequences into `SurvivorRelationsSystem` exactly once.
18. Detect theft/loss from real physical item events; never roll theft internally.
19. Send known property violations to Plan 202 with the item as a source reference.
20. Do not invent a culprit when theft is unknown.
21. Protect personal items from bulk sale, scrap and consumption by default, with explicit emergency/governance override.
22. Let Plan 206 own estate/inheritance distribution after death.
23. Query PersonalBelongings for estate metadata and commit one canonical transfer per inherited item.
24. Recompute inherited attachment from heir↔deceased relationship and context; do not blindly copy the dead owner's sentimental score.
25. Use Plan 190 for item ownership history and Plan 147 for personal memories.
26. Use MemorialSystem for memorial placement and remembrance.
27. Migrate old saves with no personal claims, preserving existing inventory/equipment behavior.
28. Reconcile missing/destroyed items and deceased/missing owners during restore without replaying consequences.
29. Add one-owner, one-item, stack-integrity and duplicate-transfer CI gates.
30. Reject raw collection/gifting/inheritance quest counters that incentivize clutter or survivor death.
31. Run gift-chain, theft/recovery, damage/repair, stack-split, bulk-sale, emergency-requisition and multi-generation inheritance simulations.
32. Close only when personal effects make survivors feel materially distinct while every physical item still has exactly one canonical existence.

---

# 58. Final Outcome

When this plan is complete, ASHFALL's survivors will finally own things that matter to them.

Not a second inventory.

Not ten stat slots.

Actual objects.

A scarf can still be a canonical clothing item with real condition and a real physical location, but now the game can know that it belonged to Mara before the war and that she refuses to let it be casually scrapped.

A wrench can still function through the normal tool/crafting system, yet become Ivo's favorite because he repaired the shelter generator with it during a blackout.

A pistol remains one weapon instance in Inventory/WornGear, but can carry a personal claim, a chain of ownership and eventually an inheritance history.

A letter remains a real document/artifact, with its readable content owned by the document/lore system, while the survivor-level layer knows *why that letter matters*.

That distinction is essential.

The item stays physical.
The belonging layer adds meaning.

When a survivor gives a cherished item to someone else, the game performs a real item transfer. The receiver may accept or refuse. The relationship system interprets the gesture. The new owner's attachment is based on the relationship and context rather than copied mechanically from the giver.

When a personal item is stolen, PersonalBelongings does not invent the theft. It reacts to the theft that actually happened. If the perpetrator is known, Plan 202 receives a real property-violation trigger. If the culprit is unknown, the game does not magically know whom to blame.

When a favorite object is damaged, the canonical condition system is still the source of truth. The sentimental system simply recognizes that a meaningful threshold was crossed.

When a survivor dies, the object does not vanish and it does not duplicate.

Plan 206 receives the estate.

A ring may be inherited by a child or partner.
A weapon may be returned to the shelter armory.
A letter may be placed at a memorial.
A tool may pass to an apprentice.
An item with no heir may simply return to shared custody.

Whatever happens, the physical item moves exactly once.

The history can remain.

Plan 190 can remember who owned the object.
Plan 147 can remember why it mattered.
MemorialSystem can display it.
Plan 185 can later change the emotional salience without erasing its provenance.

The player also gains meaningful property decisions.

A personal item is protected from accidental bulk sale or scrapping. During a catastrophe, the shelter may have policies that permit emergency requisition of a personal weapon or tool. That action can have social consequences, but the item remains usable within the real survival simulation.

This is what prevents the feature from becoming decorative clutter.

Personal belongings become material identity embedded in the existing survival systems.

A cheap broken watch can matter more than a rare rifle.
A battered scarf can survive several owners.
A gifted tool can mark a repaired friendship.
A stolen keepsake can trigger a feud.
An inherited ring can turn a death into a story that persists for another hundred days.

The shelter inventory remains one inventory.

But now some of the objects inside it belong to people—not just to the player.
