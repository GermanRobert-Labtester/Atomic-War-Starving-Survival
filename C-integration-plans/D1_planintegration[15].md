# D1 Flagship Integration Plan [15]
## Plan 190 — Item Lore & Provenance Tracking

> **Purpose:** Give selected physical item instances a persistent, deterministic history so equipment can carry
> meaningful provenance—who made it, where it was found, who carried it, what significant events it survived,
> when it changed hands, and why it eventually became notable—without forcing every food tin, bullet, scrap
> part, or stackable consumable to become a bespoke narrative object.
>
> **Primary source:** Plan 190 — Item Lore & Provenance Tracking.
>
> **Core design problem:** `ProceduralItemInstance.cs` already tracks per-instance mechanical variation such as
> condition, contamination, purity, caloric value, and expiration, while `ItemCatalogLoader` and item definitions
> provide static item data. No authoritative item-history layer exists, however: crafting origin, discovery
> origin, ownership changes, significant combat use, gifts, trades, loss/recovery, event associations, and item
> significance are not persisted. Items therefore have mechanical individuality but no memory.
>
> **Implementation posture:** event-driven, instance-aware, deterministic, save-safe, provenance-first,
> aggressively bounded against history spam, and integrated through existing inventory, crafting, expedition,
> combat, trade, relationship, journal, archive, quest, and epilogue authorities.
>
> **Critical guardrail:** ItemLoreSystem must not promote every item instance into a permanent historical object.
> The system should track only lore-eligible instances or promote items into persistent-lore status when something
> genuinely significant happens. This avoids save bloat, inventory fragmentation, UI noise, and lore farming.

---

## 1. Source Problem Statement

The source plan identifies a narrow but high-impact narrative gap:

- `ProceduralItemInstance.cs` already gives physical item instances identity-like mechanical state;
- static item definitions know what an item is;
- crafting knows who made items;
- expeditions know where items were found;
- combat knows which equipment was used;
- relations/trade know when items are gifted or exchanged;
- major events know when important moments happen;

but none of these facts are assembled into a durable item-history contract.

The missing architecture is therefore:

```text
Item instance creation / acquisition
              ↓
        ItemLoreSystem
              ↓
        provenance anchor
   ┌──────────┼──────────────┐
   ↓          ↓              ↓
crafting   discovery     initial owner
   └──────────┼──────────────┘
              ↓
     authoritative item events
   ┌──────────┼──────────┬──────────────┐
   ↓          ↓          ↓              ↓
combat      gift       trade      loss/recovery
   └──────────┼──────────┴──────────────┘
              ↓
      significance evaluator
              ↓
   mundane / notable / important / legendary
              ↓
     presentation + narrative hooks
  ├─ item detail
  ├─ journal/archive
  ├─ quests/achievements
  ├─ trade appraisal
  └─ epilogue/legacy export
```

The item-history layer observes real events. It must never invent a combat, trade, owner, discovery, or crafting
event that did not occur.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `ItemLoreSystem.cs` exists with schema-versioned capture/restore.
2. Lore is keyed to stable **item instance IDs**, not static item-definition IDs.
3. The system distinguishes stackable commodity items from identity-bearing individual items.
4. Lore-eligible instances have one canonical provenance record.
5. Crafting origin is captured once from canonical crafting completion.
6. Discovery origin is captured once from canonical expedition/loot acquisition.
7. Ownership changes use explicit transfer events, not inventory snapshots.
8. Gift, trade, loss, recovery, combat, and major-event associations are idempotent.
9. A single underlying gameplay event cannot add duplicate lore because multiple systems summarize it.
10. Lore text is rendered from structured facts plus templates; narrative strings are not the sole authority.
11. Significance is based on weighted event types and milestone tags, not merely lore-entry count.
12. Mundane item spam is suppressed.
13. Legendary status cannot be farmed by repeated trivial trades, gifts, equip/unequip operations, or ordinary
    combat ticks.
14. Ownership history preserves prior owners but remains bounded.
15. Item condition/contamination changes do not erase lore.
16. Splitting/merging stackable items has an explicit provenance policy.
17. Crafting upgrades/repairs have explicit identity-retention semantics.
18. Item destruction/loss preserves historical facts where needed without leaving a usable ghost item.
19. Selling an item removes it from player inventory but does not clone it.
20. Reacquiring the same identifiable item preserves the same history.
21. Old saves load with empty lore unless authoritative historical facts already exist.
22. Old-save migration never fabricates crafter, discovery location, or event history.
23. Item detail UI reads a projection DTO only.
24. Headless simulation produces identical provenance/significance state for the same event sequence.
25. `--item-lore-selftest` proves provenance, event association, significance, anti-farming, save round-trip,
    stack policy, and old-save compatibility.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/item_lore/ITEM_LORE_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs`
- inventory container/stack implementation
- `ItemCatalogLoader.cs`
- `ItemDefinitions.cs`
- crafting completion/result APIs
- repair/upgrade systems
- expedition loot/discovery pipeline
- combat equipment/weapon usage events
- combat kill/encounter IDs
- survivor inventory/equipment ownership
- gift/relationship transfer APIs
- faction/trade transaction APIs
- item drop/loss/recovery paths
- shelter storage and shared inventory
- unique/quest item handling
- item destruction/consumption
- save schema
- stable item-instance ID generation
- journal/archive/achievement systems
- event bus / major-event IDs
- final battle/endgame event contracts
- modding contract from Plan 165 if implemented
- procedural item serialization

Create an authority map:

| Historical fact | Canonical source | Stable event ID? | Item instance ID? | Lore action |
|---|---|---:|---:|---|
| crafted | CraftingSystem | | | origin/event |
| discovered | Expedition/Loot | | | origin/event |
| owner transfer | Inventory/Trade | | | provenance |
| combat use | CombatSystem | | | selective event |
| gift | Relations/Inventory | | | event + owner |
| loss | Inventory/World | | | status event |
| recovery | Expedition/Inventory | | | event + owner |
| final battle | Endgame/Combat | | | significance |

Do not implement event polling until this map exists.

---

## 4. Scope Boundary

### In scope

- per-instance provenance;
- crafting/discovery origin;
- ownership chain;
- lore event records;
- significance level/tags;
- narrative templates;
- item detail projection;
- search/filter by significance;
- event-driven lore accumulation;
- save/load and migration;
- quest/achievement/archive/epilogue hooks;
- optional trade-value influence through economy;
- anti-spam and anti-farming rules;
- deterministic CI validation.

### Explicitly out of scope for first pass

- full physical-world item simulation;
- item consciousness/memory;
- arbitrary procedural prose;
- generative AI lore;
- cross-campaign heirloom persistence;
- unique art generation for legendary items;
- item theft/recovery quest framework;
- automatic item renaming from every event;
- per-round combat history;
- lore on every stackable consumable;
- blockchain-like immutable audit logs.

The goal is meaningful provenance, not exhaustive telemetry.

---

## 5. Stable Item Instance Identity

Plan 190 only works if an item can be referred to as the same physical object over time.

Audit current `ProceduralItemInstance` ID semantics.

Required invariant:

```text
one physical identity-bearing item instance
→ one stable itemInstanceId
→ same ID across save/load, transfers, repair, display, and reacquisition
```

Do not derive lore ID from:
- item definition;
- inventory slot;
- owner;
- current condition;
- generated display name.

---

## 6. Instance Identity Eligibility

Not all items should be individually tracked.

Recommended categories:

### Always identity-bearing
- weapons;
- armor;
- tools;
- crafted unique equipment;
- named/quest objects;
- gifts;
- trophies/artifacts;
- durable rare items.

### Conditional
- books;
- clothing;
- keepsakes;
- crafted utility items;
- containers.

### Usually commodity / no lore
- loose ammunition;
- generic food units;
- scrap;
- water portions;
- medicine doses;
- bulk crafting material.

A commodity can be promoted to identity-bearing only if the inventory model can safely isolate one instance.

---

## 7. Lore Eligibility Policy

Add item-definition metadata or policy resolver:

```text
lorePolicy:
  never
  significant_only
  always
```

`never`:
- no lore state.

`significant_only`:
- no persistent lore until a qualifying event promotes the item.

`always`:
- provenance begins immediately.

This is the primary save-bloat control.

---

## 8. Promotion to Lore-Bearing Item

A `significant_only` item may promote when:

- crafted by named survivor;
- gifted between named survivors;
- used in a major combat/event;
- becomes quest-relevant;
- survives loss/recovery;
- explicitly marked heirloom;
- player manually marks as keepsake, if design supports.

Promotion creates provenance using only currently authoritative facts.

Do not backfill guessed history.

---

## 9. Item Lore Runtime State

Recommended:

```csharp
public sealed record ItemLoreState
{
    public int SchemaVersion { get; init; }
    public IReadOnlyDictionary<string, ItemProvenanceState> ProvenanceByItemId { get; init; }
    public IReadOnlyDictionary<string, IReadOnlyList<ItemLoreEvent>> EventsByItemId { get; init; }
    public IReadOnlyDictionary<string, ItemSignificanceState> SignificanceByItemId { get; init; }
    public IReadOnlySet<string> ProcessedSourceEventIds { get; init; }
    public ItemLoreSettingsSnapshot Settings { get; init; }
}
```

If the processed-event ledger can grow too large, use per-item bounded event fingerprints or upstream idempotency.

---

## 10. Item Provenance Contract

Recommended:

```csharp
public sealed record ItemProvenanceState
{
    public string ItemInstanceId { get; init; }
    public ItemOrigin Origin { get; init; }
    public IReadOnlyList<ItemOwnershipRecord> OwnershipHistory { get; init; }
    public string? CurrentOwnerRef { get; init; }
    public string? CurrentContainerRef { get; init; }
}
```

`Origin` is one canonical origin, not a list of mutually contradictory origins.

---

## 11. Item Origin Types

Use explicit origin kind:

```text
crafted
found
looted
traded_in
gifted_in
scenario_spawn
inherited
unknown
```

Origin fields differ by kind.

Example:

```csharp
public sealed record ItemOrigin
{
    public string OriginKind { get; init; }
    public int? Day { get; init; }
    public string? SurvivorId { get; init; }
    public string? LocationId { get; init; }
    public string? FactionId { get; init; }
    public string? SourceEventId { get; init; }
    public string? ContextId { get; init; }
}
```

---

## 12. Crafting Origin

Crafting completion provides:

- item instance ID;
- recipe;
- crafter;
- day;
- crafting station/location;
- source transaction/event ID.

Lore event:

```text
origin.crafted
```

Do not generate it when:
- crafting begins;
- recipe is queued;
- crafting fails;
- UI preview opens.

---

## 13. Discovery Origin

Expedition/loot acquisition provides:

- item instance ID;
- location;
- day;
- discovery context;
- discoverer/party if known;
- source event ID.

Examples:
- salvaged;
- looted;
- recovered from corpse;
- found in cache;
- purchased;
- quest reward.

The source's free-text `discoveryContext` should become a stable context ID plus localization template.

---

## 14. Scenario-Spawned Items

Starting equipment may have no discoverable origin.

Use:

```text
originKind = scenario_spawn
```

Optionally link:
- original survivor;
- starting shelter;
- authored provenance data.

Do not fabricate "found before Day 1."

---

## 15. Ownership Record

Recommended:

```csharp
public sealed record ItemOwnershipRecord
{
    public string OwnershipRecordId { get; init; }
    public string OwnerKind { get; init; }
    public string OwnerId { get; init; }
    public int StartDay { get; init; }
    public int? EndDay { get; init; }
    public string AcquisitionKind { get; init; }
    public string SourceEventId { get; init; }
}
```

Owner kinds may include:
- survivor;
- shelter;
- faction;
- world/location;
- unknown.

---

## 16. Ownership Semantics

Ownership must not change on:
- equip/unequip if item remains survivor-owned;
- moving between backpack slots;
- sorting inventory;
- opening storage UI.

Ownership changes on:
- gift;
- trade;
- inheritance;
- abandonment/loss;
- shelter-to-survivor assignment only if design considers individual ownership meaningful.

Decide shelter inventory semantics explicitly.

---

## 17. Shelter-Owned Items

Many survival games treat equipment as communal.

If ASHFALL's inventory is shelter-owned:
- "holder" is distinct from "owner."

Recommended:

```text
CurrentOwner = shelter
CurrentBearer = survivor
```

Do not turn every equip action into ownership lore.

This distinction is essential.

---

## 18. Bearer History

Optional and bounded.

For significant items, record notable bearer associations only:
- carried through major battle;
- long-term personal assignment;
- gifted to survivor.

Do not record every temporary equip session.

---

## 19. Item Lore Event Contract

Recommended:

```csharp
public sealed record ItemLoreEvent
{
    public string LoreEventId { get; init; }
    public string ItemInstanceId { get; init; }
    public string EventTypeId { get; init; }
    public int Day { get; init; }

    public string SourceEventId { get; init; }
    public IReadOnlyList<string> AssociatedSurvivorIds { get; init; }
    public string? AssociatedLocationId { get; init; }
    public string? AssociatedFactionId { get; init; }
    public string? AssociatedWorldEventId { get; init; }

    public string TemplateId { get; init; }
    public IReadOnlyDictionary<string, string> StructuredFacts { get; init; }
    public int NarrativeWeight { get; init; }
}
```

Structured facts are authoritative; rendered text is presentation.

---

## 20. Source Trigger Families

The source identifies seven trigger families:

1. crafting;
2. discovery;
3. combat;
4. gift;
5. trade;
6. loss/recovery;
7. significant moment.

Retain these, but apply strict significance/filter rules.

Not every event in each family deserves a lore record.

---

## 21. Crafting Lore

Every lore-eligible crafted durable item can receive one crafting origin event.

Narrative examples:
- crafted by named survivor;
- built in specific workshop;
- made during shortage if that fact is canonical.

Do not add one lore event for every repair tick after that.

---

## 22. Repair Events

Ordinary repairs should not generate lore.

Potential lore-worthy repair:
- rebuilt from near-destruction;
- repaired by a notable prior owner;
- restored after recovery.

Use explicit threshold/event tag.

Otherwise condition changes remain mechanical only.

---

## 23. Upgrade Events

If item upgrades retain the same physical identity:
- record major upgrade as lore;
- preserve item ID.

If crafting system replaces old instance with new instance:
- define lineage:

```text
derivedFromItemId
```

Do not silently merge provenance of destroyed inputs into one item unless recipe semantics say so.

---

## 24. Composite Crafting Lineage

For a complex item made from several components:

- crafter/origin is enough for v1;
- do not recursively inherit every component's entire lore.

Optional:
- one significant component can be referenced if explicitly tagged.

Otherwise history explodes combinatorially.

---

## 25. Discovery Lore

A found durable item receives one origin event.

If later recovered after being lost:
- use recovery event, not a second discovery origin.

Origin is immutable once established.

---

## 26. Combat Lore Eligibility

The source proposes combat lore.

Do not record:
- every swing;
- every shot;
- every combat encounter.

Lore-worthy combat conditions may include:
- first kill with item;
- named/major battle;
- final blow in major encounter;
- survival while item was equipped;
- critical weapon break/recovery;
- designated story event.

Use canonical combat-event tags.

---

## 27. Combat Event Contract

Combat emits something like:

```csharp
SignificantItemCombatUse
{
    sourceEventId,
    itemInstanceId,
    combatEventId,
    day,
    locationId,
    bearerId,
    significanceTag,
    optionalTargetId
}
```

Bestiary/lore systems should not infer significance by scanning combat logs after the fact.

---

## 28. First Blood Guardrail

"First blood" is stylistic.

Do not promise that exact phrase unless combat authority emits a real first-kill/first-hit fact.

Lore templates should map only to facts actually known.

---

## 29. Gift Lore

A gift between named survivors is high-quality provenance.

Event contains:
- giver;
- receiver;
- day;
- relationship context if canonical;
- item instance;
- source transfer ID.

Ownership history updates transactionally.

---

## 30. Gift Reason

The source template includes `{reason}`.

Do not synthesize a reason unless:
- player selected one;
- relationship system emitted one;
- quest/event provided one.

Fallback:
"Given by Elena to Marcus on Day 120."

No invented emotional claims.

---

## 31. Trade Lore

Trade event contains:
- item ID;
- seller owner;
- buyer faction/settlement;
- day;
- transaction ID;
- location.

Record only if item is already notable/significant or the trade itself is notable.

Do not create lore for every commodity sale.

---

## 32. Trade Back Into Shelter

If the same item returns:
- same instance ID;
- add ownership transfer;
- preserve origin/events.

If trade system respawns a generic equivalent instead of same instance:
- it is not the same artifact.

Do not falsely preserve history across generic replacements.

---

## 33. Loss Event

Loss must be an actual lifecycle state:

```text
item leaves controlled inventory
and remains recoverable/known as lost
```

Examples:
- dropped during expedition;
- stolen;
- swept away;
- left behind.

Consumption/destruction is not "lost."

---

## 34. Recovery Event

Recovery must reference:
- same item instance ID;
- prior loss event ID where possible;
- recovery location/day;
- recoverer.

This creates meaningful narrative arcs.

---

## 35. Destroyed Item

If destroyed:
- mark provenance terminal status;
- remove usable item from inventory;
- retain historical lore if item was important/legendary.

UI can show it in archive/history, not active inventory.

---

## 36. Consumed Item

Consumables normally have no lore.

If a unique lore-bearing consumable exists:
- consuming ends physical identity;
- archive history may remain if important.

Do not keep it usable.

---

## 37. Significant Moment Association

Major-event systems can nominate items:

```text
ItemPresentAtSignificantEvent
```

Examples:
- shelter founding;
- final battle;
- major rescue;
- treaty signing;
- important expedition return.

Presence must have a real criterion:
- equipped/carried by participant;
- displayed at event;
- designated ceremonial object.

Do not associate every shelter inventory item with every global event.

---

## 38. Significant Event Participation Filter

For a final battle:
- weapons/armor actually used may qualify;
- random stored axe in locker does not.

For founding:
- explicit founding artifact/ceremonial item may qualify.

This prevents absurd lore inflation.

---

## 39. Item Significance State

Recommended:

```csharp
public sealed record ItemSignificanceState
{
    public string ItemInstanceId { get; init; }
    public string LevelId { get; init; }
    public int NarrativeWeight { get; init; }
    public IReadOnlySet<string> SignificanceTags { get; init; }
    public IReadOnlySet<string> AppliedRuleIds { get; init; }
}
```

Significance is derived from evidence but may be persisted for performance/audit.

---

## 40. Significance Levels

Retain source labels:

- mundane;
- notable;
- important;
- legendary.

But do **not** use simple lore-count thresholds as the only rule.

Lore quantity is not meaning.

---

## 41. Weighted Significance Model

Example:

```text
crafting origin                 +5
named survivor gift            +15
major battle                   +20
owner death while carried      +15
loss + recovery arc            +15
founding/final event            +30
long multi-owner heirloom      +20
ordinary trade                  +2
routine combat                  +0
```

Thresholds then map to levels.

Final weights belong in data.

---

## 42. Milestone Override Rules

Specific events may force minimum significance:

```text
present_at_final_battle -> at least important
heirloom_three_generations -> at least important
historic_founding_artifact -> legendary candidate
```

Avoid making every 11-entry item legendary.

---

## 43. Narrative Weight

Source proposes 0–100.

Use 0..100 or basis points as internal bounded score.

Narrative weight determines:
- UI prominence;
- archive eligibility;
- epilogue candidate priority;
- whether routine future events are worth recording.

It should not automatically buff combat stats.

---

## 44. Significance Tags

Candidate tags:

- heirloom;
- first_craft;
- gift_from_friend;
- battle_proven;
- recovered;
- founder_artifact;
- final_battle;
- memorial_item;
- faction_trade;
- survivor_keepsake;
- legendary_weapon.

Tags must correspond to real rule triggers.

---

## 45. Lore Density Setting

Source proposes lore-generation settings.

Recommended player-facing density:

```text
Minimal
Standard
Rich
```

Density changes **which low-weight events are retained**, not core provenance.

Never disable:
- origin;
- ownership;
- legendary/milestone facts.

This preserves save semantics.

---

## 46. Per-Item-Type Lore Toggle

The source proposes a per-item-type toggle.

Prefer policy metadata/config rather than hundreds of user toggles.

Player may optionally:
- mark one item "Track history";
- mark item "Favorite/Heirloom."

Do not expose a giant item-definition settings matrix unless needed.

---

## 47. Player-Favorited Items

Optional:
- player can "mark significant";
- this enables richer retention;
- it does not grant significance points by itself.

The mark says "I care about this," not "this is legendary."

---

## 48. Lore Retention Policy

Per item:

Always retain:
- origin;
- ownership transitions;
- high-weight events;
- first relevant event of type;
- legendary milestones.

Prune/compress:
- repeated ordinary trade;
- repeated routine combat;
- redundant repairs.

This is the second major save-bloat control.

---

## 49. Ownership History Compression

For very long chains:

Keep:
- first owner;
- named survivor owners;
- faction transfers;
- last/current owner;
- aggregate count of omitted trivial communal transfers.

Do not let one item generate thousands of entries.

---

## 50. Duplicate Owner Prevention

If ownership remains with same entity:
- no new ownership record.

If item returns to same owner after another owner:
- new record is valid.

Owner chain is chronological.

---

## 51. Item Stack Problem

The source plan assumes `itemId` means procedural item instance.

Audit whether commodities are stacks.

If a stack contains 10 identical canned foods:
- one stack ID may not represent ten physical histories.

Do not attach detailed provenance to stack container and pretend all units share it.

---

## 52. Stack Split Policy

For lore-bearing stackable item, options:

### Recommended
Promote one unit into unique instance before lore-bearing event.

Example:
- gift one bottle from stack;
- split unit;
- assign stable instance ID;
- lore follows that unit.

Remaining stack stays commodity.

---

## 53. Stack Merge Policy

Never merge two distinct lore-bearing item identities into one anonymous stack.

Options:
- prohibit merge;
- keep unique instances separate;
- allow UI grouping but preserve IDs internally.

This is mandatory for provenance integrity.

---

## 54. Ammunition Policy

Do not lore-track individual bullets.

Ammunition remains commodity unless a rare unique narrative item exists.

Weapon lore can record:
- major battle;
- ammunition type used if relevant;

without bullet identity.

---

## 55. Food/Medicine Policy

Do not generate normal lore for:
- meals;
- water;
- generic medicines.

Potential exception:
- unique keepsake ration;
- last surviving pre-war bottle;
- quest medicine item.

Use item lore policy.

---

## 56. Item Repair and Condition Independence

Lore state survives:
- wear;
- repair;
- contamination;
- decontamination;
- purity changes.

Mechanical condition does not erase identity.

If repair creates a replacement instance, use lineage contract.

---

## 57. Item Transformation

Recipes can transform one item into another.

Classify recipes:

### Identity-preserving
- sharpening;
- modification;
- repair;
- attachment installation.

Same instance/lore.

### Identity-replacing
- melt down axe into scrap;
- dismantle weapon;
- consume item as ingredient.

Original history terminates.

### Descendant creation
- reforging heirloom blade into new blade.

New item may reference `derivedFromItemId`.

Explicit recipe metadata required.

---

## 58. Descendant Lore

A descendant may get one provenance fact:

```text
forged_from:<oldItemId>
```

Do not copy every old event into child.

Archive/epilogue can traverse lineage if needed.

---

## 59. Item Lore Template Catalog

Create:

`Assets/StreamingAssets/Data/lore_templates.json`

Suggested root:

```json
{
  "schemaVersion": 1,
  "eventTypes": [],
  "templates": [],
  "significanceRules": [],
  "retentionRules": []
}
```

---

## 60. Lore Template Contract

```csharp
public sealed record ItemLoreTemplate
{
    public string TemplateId { get; init; }
    public string EventTypeId { get; init; }
    public string TextKey { get; init; }
    public IReadOnlyList<string> RequiredFactKeys { get; init; }
    public IReadOnlyList<string> OptionalFactKeys { get; init; }
    public int NarrativeWeight { get; init; }
}
```

No arbitrary expression evaluation.

---

## 61. Structured Narrative Rendering

Template example:

```text
item_lore.crafted.named_survivor
```

Facts:
- survivor ID;
- day;
- location.

Renderer resolves localized names.

Save stores:
- template ID;
- fact refs.

This survives localization changes.

---

## 62. Missing Historical References

A prior owner may die, leave, or be removed by mod changes.

Lore entry remains.

UI resolves:
- known historical name if survivor archive provides it;
- fallback `Unknown former owner`.

Never delete event due to missing live entity.

---

## 63. Location References

Store stable location ID.

If location name changes:
- lore renders current localized name or historical label depending architecture.

Do not store only free-text location.

---

## 64. Faction References

Trade lore stores stable faction ID.

If faction later destroyed:
- history still renders.

Use faction archive/static definition.

---

## 65. Event References

Major event association stores stable world/event ID.

If event has journal/archive record, link to it.

Do not duplicate the entire event narrative in item state.

---

## 66. Deterministic Lore Selection

If several templates are valid:
- filter compatible templates;
- sort stable IDs;
- deterministic seeded selection from item/event seed.

Seed:
```text
campaignSeed + itemInstanceId + sourceEventId + templateProfileVersion
```

Persist selected template ID.

No reload reroll.

---

## 67. Lore Does Not Need RNG for Every Event

Prefer deterministic direct templates for:
- crafting;
- discovery;
- gift;
- trade.

Use RNG only for narrative wording variation when needed.

Core history remains deterministic facts regardless.

---

## 68. Event Deduplication

Every lore event must reference a source event/transaction ID.

Examples:
- crafting transaction;
- combat event;
- gift transfer;
- trade transaction;
- recovery event;
- world milestone.

If same source event arrives twice:
- no duplicate lore.

---

## 69. Cross-System Summary Duplication

Likely bug:

```text
CombatSystem emits item battle event
ExpeditionSystem emits summary of same battle
Journal emits major battle event
```

Bestiary/lore should designate one canonical source.

Other summaries can carry the same root event ID.

---

## 70. Combat Lore Rate Limit

Per item:
- at most one routine combat lore event per day/expedition;
- major tagged events exempt.

This prevents spam even if combat emits many subevents.

---

## 71. Trade Lore Rate Limit

Routine trade:
- record ownership transfer structurally;
- narrative lore only if significant.

A player repeatedly trading one axe back and forth should not climb significance.

---

## 72. Gift Farming Prevention

Gift significance requires:
- real ownership transfer;
- relation context;
- cooldown or first-time pair significance.

Repeatedly gifting same item between two survivors should not generate unlimited weight.

---

## 73. Loss/Recovery Farming Prevention

A player cannot deliberately drop/pick up item repeatedly for legendary status.

Qualifying loss:
- involuntary/system-tagged;
- expedition loss;
- theft;
- disaster;
- abandonment with recovery gap.

Manual inventory drop/pickup does not count by default.

---

## 74. Combat Farming Prevention

Ordinary kills do not linearly add narrative weight forever.

Use:
- first combat;
- first kill;
- major battle;
- rare named event.

Repeated routine kills may update statistics but not lore/significance.

---

## 75. Item Statistics vs Lore

If weapon stats track:
- kill count;
- damage;
- uses;

keep them separate.

Lore can reference milestone:
- 100th kill;

without storing every kill event.

Do not make ItemLoreState a telemetry dump.

---

## 76. Significance Rule Evaluator

Data rule example:

```json
{
  "ruleId": "sig_heirloom_three_named_owners",
  "requires": {
    "namedOwnerCountAtLeast": 3
  },
  "addTags": ["heirloom"],
  "weight": 20
}
```

Rule evaluation is deterministic and idempotent.

---

## 77. Significance Recalculation

Recommended:
- evaluate incrementally when event/provenance changes;
- selftest can recompute from history and compare.

This guards against drift.

---

## 78. Significance Is Monotonic

Usually significance should not decrease merely because item is sold or condition worsens.

History remains.

Exception:
- no need to demote.

An old rusted rifle can remain legendary.

---

## 79. Legendary Eligibility

Legendary should require:
- one or more exceptional rule tags;
- or very high weight from diverse meaningful history.

Not:
- 11 trivial events.

This is a major correction to the source's simple count threshold.

---

## 80. Item Detail UI Projection

Recommended:

```csharp
public sealed record ItemLorePanelModel
{
    public string ItemInstanceId { get; init; }
    public string ItemDefinitionId { get; init; }
    public string SignificanceLevelId { get; init; }
    public IReadOnlyList<string> SignificanceTags { get; init; }

    public ItemOriginView Origin { get; init; }
    public IReadOnlyList<ItemOwnershipView> Ownership { get; init; }
    public IReadOnlyList<ItemLoreEventView> Timeline { get; init; }
}
```

UI never computes significance thresholds.

---

## 81. Item Detail Tabs

Enhance item detail with:

### Overview
Mechanical properties.

### History
Chronological lore events.

### Provenance
Origin and owner chain.

### Significance
Level/tags and why it matters.

Avoid four tabs if UI becomes cluttered; can combine history/provenance.

---

## 82. Timeline Ordering

Sort:
- day ascending;
- stable event sequence/source ID tie-break.

Do not rely on insertion/dictionary order.

---

## 83. Timeline Compression UI

If an item has many events:
- show highlights by default;
- expand all;
- group repeated low-weight events.

Do not dump 80 lines into item panel.

---

## 84. Provenance Chain UI

Show:

```text
Crafted by Elena — Day 47
Held by Shelter — Day 47–55
Gifted to Marcus — Day 55
Lost during River Flood — Day 88
Recovered by Jana — Day 91
```

Only if those facts actually exist.

---

## 85. Tooltip

Compact tooltip:

```text
Important
5 recorded events
Crafted by Elena
```

No full timeline on hover.

---

## 86. Search and Filter

Inventory/history views may filter:

- significance;
- tag;
- crafter;
- former owner;
- event type.

Search should operate on revealed/player-known lore only if secrecy matters.

Most item provenance is player-owned knowledge, so this is less restrictive than bestiary.

---

## 87. Significant Item View

Optional collection screen:

- all notable/important/legendary items;
- destroyed/lost historical artifacts;
- current owner/location.

Better than cluttering ordinary inventory.

---

## 88. Journal Integration

Journal only records:
- legendary promotion;
- important heirloom transfer;
- recovery of significant item;
- major item/event association.

Do not journal every crafting origin.

---

## 89. Shelter Archive Integration

Archive can retain:
- legendary item;
- founding artifact;
- final battle weapon;
- important lost/destroyed heirloom.

ItemLoreSystem exports candidate facts.

Archive owns historical curation.

---

## 90. Achievement Integration

Plan 149 may observe:

- notable-item count;
- heirloom owner count;
- legendary item;
- lore event count;
- major recovery.

ItemLoreSystem does not own rewards.

---

## 91. Quest Integration

Source hooks:

- The Collector;
- The Historian;
- The Heirloom;
- The Legend;
- The Crafter;
- The Trader;
- The Story.

Export read-only metrics/events.

QuestSystem owns quest progress/rewards.

---

## 92. Epilogue Integration

Plan 145 can consume:

- legendary item still owned;
- famous item lost;
- heirloom passed down;
- final battle artifact;
- creator/owner history.

Export stable artifact facts.

---

## 93. Generational Legacy Boundary

Source follow-on suggests cross-campaign item legacy.

Plan 190 is run-local.

It may export:
- item ID;
- significance;
- origin;
- major tags.

Plan 175/later meta system decides cross-campaign persistence.

---

## 94. Crafting Integration

CraftingSystem provides:
- completion event;
- resulting item IDs;
- crafter;
- recipe;
- station/location;
- day.

ItemLoreSystem:
- establishes origin;
- optionally marks `first_craft` significance if world/crafter milestone is real.

Do not make crafting own lore.

---

## 95. First Craft Tag

"first_craft" needs explicit scope:

- first item survivor ever crafted;
- first recipe type;
- first shelter craft.

Choose one.

Do not ambiguously award tag to many items.

---

## 96. Expedition Integration

Expedition acquisition event provides:
- item instance;
- location;
- context;
- discoverer/party.

Use for origin only on first acquisition.

Later expedition recovery uses recovery event.

---

## 97. Combat Integration

Combat can nominate:
- equipped weapon;
- armor;
- shield/tool if relevant.

Do not scan all inventory carried by combatant and mark each as "used in battle."

---

## 98. Armor Lore

Armor can gain:
- survived major battle;
- saved wearer from lethal hit only if combat emits such fact.

Avoid fake "saved Marcus's life" without authoritative event.

---

## 99. Tool Lore

Tools may gain:
- used in major repair/rescue;
- built critical shelter system.

Only if upstream event identifies tool instance.

No generic "present during construction."

---

## 100. Relationship Integration

Gift event may use relationship context.

Relations system owns:
- trust;
- affection;
- conflict.

Item lore merely records transfer/context.

---

## 101. Trade Integration

TradeSystem owns:
- price;
- transaction;
- faction standing effects.

Item lore may provide a descriptor for appraisal.

No direct currency mutation.

---

## 102. Lore-Based Trade Value

The source follow-on suggests lore increases value.

Recommended later/optional:

```text
base item value
+ bounded provenance premium
```

Only important/legendary items qualify.

Prevent recursive loop:
- trade creates lore;
- lore increases price;
- repeated trade increases lore/price.

Routine trade events should not add significance weight.

---

## 103. Faction Appraisal

Faction may care about:
- founder artifact;
- enemy weapon;
- cultural relic;
- famous survivor item.

Preference lives in faction/economy data.

Item lore provides tags/provenance.

---

## 104. Modding Integration

If Plan 165 is implemented, public mod contract may expose:

- lore template additions;
- significance rules;
- item lore policy metadata.

Mods must not inject arbitrary executable event handlers.

All rule kinds registered.

---

## 105. Modded Item Removal

If save has lore for modded item definition and mod disappears:
- Plan 165 reports compatibility;
- lore history should not be silently rerolled;
- historical placeholder may remain if safe.

Active usable item semantics belong to mod/save compatibility.

---

## 106. Old-Save Migration

Source says empty lore/all mundane.

Recommended default:

- no provenance record for existing commodity items;
- no lore events;
- no significance tags;
- existing procedural item IDs remain unchanged;
- no guessed crafter/location;
- no retroactive combat history.

When an old item later receives a qualifying event:
- create provenance with `originKind=unknown_legacy`;
- begin history from that point.

---

## 107. Legacy Origin

Use:

```text
originKind = legacy_unknown
```

UI:
"History before Day X is unknown."

This is more honest than fabricating "found at shelter."

---

## 108. Optional Old-Save Reconstruction

Only if authoritative data exists.

Examples:
- crafted-by metadata already persisted;
- quest reward provenance;
- unique item creation record.

Reconstruct those exact facts.

Do not infer from current owner.

---

## 109. No Migration Notification Flood

Legacy items do not produce lore notifications during migration.

They begin quietly.

Only future qualifying event may explain lore system via tutorial.

---

## 110. Save Schema

Persist:

- provenance for lore-bearing items;
- retained lore events;
- significance state/rules applied;
- event idempotency markers;
- optional settings.

Do not persist:
- rendered lore strings;
- static item definitions;
- duplicated survivor/location names.

---

## 111. Save/Load After Transfer

Test:

1. item owned by shelter;
2. gifted to survivor;
3. save;
4. reload.

Expected:
- one ownership transfer;
- one gift event;
- same significance;
- current owner correct.

---

## 112. Save/Load During Trade

If trade transaction is multi-step:
- lore updates only on committed trade.

A pending trade does not change ownership/history.

Reload cannot duplicate.

---

## 113. Save/Load During Loss/Recovery

If item is lost:
- provenance state says world/lost;
- save persists same item ID.

Recovery:
- restores usable item instance;
- appends recovery event once.

No cloning.

---

## 114. Stable IDs

Lore event:

```text
item_lore:<itemInstanceId>:<sourceEventId>:<eventTypeId>
```

Ownership:

```text
item_owner:<itemInstanceId>:<transferEventId>
```

Significance milestone:

```text
item_significance:<itemInstanceId>:<ruleId>
```

---

## 115. Headless Determinism

Same sequence:

```text
craft
gift
major battle
loss
recovery
trade
```

must generate identical:
- provenance;
- event IDs;
- selected templates;
- significance;
- tags;
- digest.

UI is irrelevant.

---

## 116. Item Lore Digest

Normalize:

- origin;
- ownership chain;
- retained events;
- significance level;
- tags;
- terminal status.

Hash for CI.

Save/load digest must match.

---

## 117. Data Integrity Validation

Validate:

- lore template IDs unique;
- event types registered;
- required fact keys supported;
- localization keys exist;
- significance rules valid;
- item lore policy values valid;
- referenced item categories/tags exist;
- no circular lineage aliases;
- thresholds bounded.

---

## 118. Cross-Catalog Validation

If significance rule references:
- item category;
- event tag;
- faction;
- location;
- quest/world event;

ensure reference resolves or uses validated dynamic tag registry.

No free-form magic strings.

---

## 119. Dedicated `--item-lore-selftest`

It should:

1. load item/lore catalogs;
2. create lore-eligible crafted weapon;
3. verify crafting origin;
4. replay craft event and verify no duplicate;
5. gift item;
6. verify owner chain;
7. record major combat event;
8. verify significance change;
9. record routine combat spam and verify retention/rate limit;
10. lose item;
11. save/reload;
12. recover same instance;
13. trade to faction;
14. reacquire same instance if fixture supports;
15. verify no clone;
16. test stack split promotion;
17. test forbidden stack merge;
18. test legacy item with unknown origin;
19. test legendary milestone rule;
20. verify UI projection;
21. exit non-zero on mismatch.

---

## 120. Unit Test Matrix

### Origin
- crafted;
- discovered;
- scenario;
- legacy unknown;
- origin immutable.

### Ownership
- shelter;
- survivor gift;
- faction trade;
- return;
- duplicate transfer event.

### Lore events
- combat significant;
- routine combat suppressed;
- loss/recovery;
- significant world event.

### Significance
- mundane;
- notable;
- important;
- legendary;
- count spam does not force legendary.

### Stacks
- commodity no lore;
- split unique;
- merge prohibited/preserved.

### Persistence
- no lore;
- notable item;
- lost item;
- destroyed item;
- old save.

---

## 121. Golden Item Fixtures

Use actual item IDs.

Create:

1. mundane scrap stack — no lore.
2. crafted axe — origin only.
3. gifted knife — origin + owner chain.
4. battle rifle — major combat event.
5. repaired tool — no spam.
6. heirloom item through three named owners.
7. lost/recovered item.
8. traded artifact.
9. final-battle weapon.
10. destroyed legendary item.
11. legacy old-save item with unknown origin.
12. unique stack split case.

---

## 122. Property / Fuzz Testing

Properties:

- one item has one origin;
- ownership history chronological;
- current owner equals final open ownership record;
- source event counted at most once;
- narrative weight bounded;
- significance level valid;
- destroyed item not active;
- same deterministic event sequence produces same digest;
- commodity stack does not suddenly gain per-unit lore without split.

---

## 123. Event-Sequence Fuzz

Generate random valid sequences:

```text
create -> assign -> gift -> combat -> trade -> reacquire -> loss -> recover -> destroy
```

Reject invalid transitions.

Assert:
- no duplication;
- no impossible current owner;
- no event after terminal destruction unless historical only.

---

## 124. Save-Bloat Stress Test

Generate:
- 10,000 commodity items;
- 1,000 procedural durable items;
- 100 lore-bearing items;
- 20 long-lived artifacts.

Measure:
- save size;
- capture time;
- restore time.

Goal:
commodity items add zero lore overhead beyond eligibility checks.

---

## 125. Lore Density Stress

Simulate 500 days with rich lore setting.

Ensure:
- per-item event retention bounded;
- ownership chains compressed if needed;
- significant artifacts remain fully meaningful;
- no runaway save growth.

---

## 126. Performance Budget

Item lore is event-driven.

Requirements:
- no per-frame scans;
- no daily scan of all items;
- significance evaluation only on lore/provenance change;
- per-item events indexed by item ID;
- UI projection computed on demand/cached.

Negligible overhead for mundane inventory.

---

## 127. Memory Budget

Avoid:
- dictionary entry for every commodity unit;
- duplicated rendered strings;
- huge processed-event global ledger.

Prefer:
- create state only when lore-bearing;
- stable facts/templates;
- bounded events.

---

## 128. Narrative Quality Audit

Export 100 rendered item histories.

Review:

- repetitive templates;
- trivial events appearing important;
- fake emotional reasons;
- contradictory ownership;
- excessive day/date repetition;
- implausible "legendary" promotion;
- grammar/localization issues.

Fix data/templates and trigger rules.

---

## 129. Significance Calibration

Generate:

`docs/item_lore/ITEM_SIGNIFICANCE_CALIBRATION.md`

Simulate representative histories.

Report:
- weight;
- tags;
- resulting level;
- number of ordinary vs important events.

Target distribution:
- most tracked items remain notable;
- important items uncommon;
- legendary items rare and event-rich.

---

## 130. Lore Retention Calibration

Generate:

`docs/item_lore/ITEM_LORE_RETENTION.md`

For each event type:
- always retain?
- retain first?
- retain only if tagged major?
- aggregate count?
- drop?

This must be explicit.

---

## 131. Item Lore Coverage Report

Generate:

`docs/item_lore/ITEM_LORE_COVERAGE.md`

| Trigger | Upstream source | Stable ID | Lore template | Significance rule | Fixture |
|---|---|---:|---:|---:|---:|
| crafting | | | | | |
| discovery | | | | | |
| combat | | | | | |
| gift | | | | | |
| trade | | | | | |
| loss/recovery | | | | | |
| significant event | | | | | |

Flag unwired triggers.

---

## 132. UI Accessibility

Support:

- keyboard/controller navigation;
- text scaling;
- significance not color-only;
- chronological history readable;
- owner chain readable without hover;
- long history collapsible;
- no forced animation;
- destroyed/lost status explicit.

---

## 133. Tutorial

First lore-eligible crafted/found item:

Explain:
- important items can accumulate history;
- ordinary commodities do not;
- major transfers/events are remembered;
- significance reflects history.

Do not teach player to farm lore.

---

## 134. Notification Policy

Notify:
- item becomes notable/important/legendary;
- important recovery;
- major heirloom transfer.

Do not notify:
- every crafting origin;
- ordinary ownership movement;
- every combat association.

---

## 135. Item Lore Search

If inventory collection is large:
- filter by significance;
- "has history";
- creator;
- owner;
- tag.

This makes feature useful without cluttering main list.

---

## 136. Main Inventory Presentation

Main inventory should show a small indicator:

```text
★ Important
```

or icon.

Full lore only on detail.

Avoid turning every row into a paragraph.

---

## 137. Lost Artifact View

Optional:
- notable/legendary items currently lost/destroyed/traded away;
- historical status.

This supports recovery quests later.

Not required for base inventory panel.

---

## 138. Item Theft Follow-On Contract

Future theft system can emit:

```text
ItemStolen
ItemRecovered
```

with same item ID.

Plan 190 already supports provenance.

Do not implement theft simulation now.

---

## 139. Item Quest Follow-On Contract

A quest can target stable item instance ID.

This enables:
- recover heirloom;
- deliver legendary weapon;
- return gift.

QuestSystem owns state.

ItemLoreSystem provides target/provenance.

---

## 140. Legacy Item UI

For old-save item promoted later:

```text
Earlier history unknown.
First recorded event: ...
```

This is transparent and lore-friendly.

---

## 141. Item Naming

If the game supports custom names:
- name change is presentation;
- may be recorded once if item becomes heirloom;
- does not change ID.

Do not create lore event for every rename.

---

## 142. Automatic Legendary Names

Do not auto-generate grandiose names by default.

Optional later:
- legendary artifact gains subtitle from tags.

Keep v1 straightforward.

---

## 143. Provenance Trustworthiness

Player-owned item history is generally objective system truth.

If future counterfeit/rumor mechanics exist:
- separate claimed provenance from verified provenance.

Do not mix now.

---

## 144. Player Knowledge vs World History

A found item may have pre-campaign history unknown to player.

V1 only tracks:
- known origin within campaign;
- authored static lore if item definition provides it.

Do not invent unseen previous owners.

Future discovered provenance can add claimed historic facts separately.

---

## 145. Static Item Lore vs Dynamic Provenance

Static item definition may already contain flavor/history.

Separate:

### Static lore
"Model manufactured before the war."

### Dynamic provenance
"Found by Jana in Sector 7 on Day 23."

UI can show both.

ItemLoreSystem owns dynamic run-local history.

---

## 146. Procedural Item Instance Integration

Preferred:
- keep item mechanical fields where they are;
- store `HasLore`/lore key only if needed;
- ItemLoreSystem state keyed by instance ID.

Do not embed large event lists directly into every item DTO unless serialization architecture clearly favors it.

---

## 147. Save Coupling Decision

Two options:

### Separate lore state
Pros:
- sparse;
- cleaner;
- commodity items unaffected.

### Lore embedded per item
Pros:
- lifecycle follows item automatically.

Recommendation:
sparse `ItemLoreState` + lifecycle event cleanup, unless item serialization already supports extensible components.

---

## 148. Item Deletion Cleanup

If mundane lore-free item consumed:
- no lore work.

If lore-bearing item terminally destroyed:
- mark terminal;
- archive if significant;
- optionally move history to historical record.

Do not orphan dictionary state forever without policy.

---

## 149. Historical Artifact Store

Optional sparse archive:

```csharp
DestroyedOrDepartedArtifactRecord
```

Only for important/legendary items.

This allows epilogue/history without keeping live item.

---

## 150. Common Item Destruction

Notable-but-low-value item destroyed:
- history may be discarded after journal/archive depending significance.

Do not keep infinite histories of broken common tools.

---

## 151. Faction-Owned Artifact Persistence

If item traded to faction:
- player no longer needs full live mechanics;
- provenance can persist compactly.

If reacquisition is impossible, archive only.

If reacquisition possible, external item registry must preserve instance ID.

---

## 152. External Ownership Registry

Audit trade implementation.

If sold item is destroyed into value and cannot return:
- lore can record departure;
- no promise of reacquisition.

Do not pretend the exact item remains simulated inside faction inventory.

---

## 153. Reacquisition Feasibility Gate

Only implement "same item comes back" if economy stores external unique instances.

Otherwise:
- trade is terminal provenance departure;
- future similar item is new instance.

Document clearly.

---

## 154. Significant Event Registration API

Recommended:

```csharp
public interface IItemSignificantEventSink
{
    void Record(ItemSignificantEvent evt);
}
```

World systems nominate eligible item IDs.

ItemLoreSystem validates:
- item exists/lore eligible;
- source event unique;
- event type supported.

---

## 155. Event Fan-Out

One major event may affect several items.

Example:
- survivor fights final battle with rifle + armor.

Allow multiple item-lore events sharing same world event ID but distinct item IDs.

Dedup key includes item.

---

## 156. Founding Artifact Rule

The source suggests item present at founding.

Do not automatically tag items simply stored in shelter on founding day.

Require:
- explicit ceremonial role;
- player-selected artifact;
- quest/event reference.

Otherwise too many false legends.

---

## 157. Final Battle Rule

A final battle item qualifies if:
- equipped/used;
- bearer participated;
- item survived or was destroyed in battle.

This is a legitimate high-weight event.

---

## 158. Heirloom Rule

"Heirloom" should require:
- at least N meaningful owner transitions;
- named survivors/family connection where possible;
- item remains same identity.

Three random short-term owners are not enough if transfers are trivial.

---

## 159. Family Integration

If Plan 150/183 family systems exist:
- parent→child transfer can strongly support heirloom tag.

Relations/family system confirms family link.

ItemLoreSystem does not infer surnames.

---

## 160. Item Lore + Art/Culture Integration

Plan 178 cultural artifacts may already have provenance.

Avoid two competing systems.

For artwork:
- ArtCreationSystem owns cultural-work state;
- ItemLoreSystem can provide generic item provenance only if artwork is represented as inventory item.

Prefer an adapter so creator/provenance is not duplicated.

---

## 161. Item Lore + Bestiary Integration

Plan 187 trophies/specimens later may become lore-bearing.

Bestiary provides creature source fact.

ItemLore records:
- specimen origin;
- hunter;
- event.

Do not duplicate creature knowledge.

---

## 162. Item Lore + Modding

Mod items can participate if:
- item instance IDs stable;
- lore policy valid;
- templates/rules validated.

Save fingerprint handles missing mod item.

No reroll or replacement of history.

---

## 163. CI Golden Scenario — The Axe

Scenario:

1. Elena crafts axe on Day 47.
2. Axe remains shelter-owned but Elena carries it.
3. Elena uses it in ordinary scavenger fight: no lore event beyond optional first significant use.
4. Elena gifts axe to Marcus on Day 70.
5. Marcus carries axe during major raid on Day 89.
6. Axe is lost during flood on Day 100.
7. Jana recovers same item on Day 103.
8. Axe passes to Elena's child later if family system exists.

Expected:
- one crafted origin;
- meaningful ownership chain;
- gift event;
- major battle event;
- loss/recovery arc;
- no equip spam;
- same instance ID;
- significance grows naturally to important/heirloom candidate.

---

## 164. CI Golden Scenario — Commodity Stack

Scenario:

- 100 water units in stack;
- split 1 unit for ordinary consumption;
- no lore.

Expected:
- zero `ItemLoreState` rows.

Then:
- one special bottle explicitly gifted as keepsake;
- inventory splits one unique instance;
- only that instance gets provenance.

This proves save-bloat boundary.

---

## 165. CI Golden Scenario — Trade Farming

Scenario:

- important knife sold to faction;
- bought back if exact-instance trade supported;
- sold again;
- repeated 10 times.

Expected:
- ownership history may record transfers subject to compression;
- ordinary repeated trade adds little/no narrative weight;
- no infinite legendary progression;
- no infinite standing/value loop.

---

## 166. CI Golden Scenario — Legacy Item

Scenario:

- old save contains procedural rifle with no lore metadata.
- after migration, rifle is used in major battle.

Expected:
- origin `legacy_unknown`;
- one battle event;
- item becomes notable if rule says so;
- no invented crafter/discovery day.

---

## 167. CI Golden Scenario — Upgrade

Scenario:

- important rifle gets scope/repair.

If identity-preserving:
- same item ID;
- optional major upgrade event only if configured.

If crafting replacement:
- old item terminal/consumed;
- new item references lineage if recipe says so.

No accidental double artifact.

---

## 168. Implementation Phase A — Identity & Audit

Tasks:

1. audit item instance IDs;
2. classify stack semantics;
3. classify durable vs commodity item families;
4. map craft/discovery/transfer/combat events;
5. define lore eligibility policy;
6. baseline save-size measurements.

Exit:
it is clear which items can safely carry persistent history.

---

## 169. Implementation Phase B — Provenance Foundation

Tasks:

1. `ItemLoreSystem`;
2. origin DTO;
3. ownership DTO;
4. state DTO;
5. stable IDs;
6. capture/restore;
7. origin immutability;
8. ownership transitions;
9. tests.

Exit:
crafted/found durable items can retain provenance.

---

## 170. Implementation Phase C — Event Ingestion

Tasks:

1. crafting event;
2. expedition discovery;
3. gift transfer;
4. trade;
5. loss/recovery;
6. major combat;
7. significant world event;
8. source-event dedup.

Exit:
all seven source trigger families are structurally supported.

---

## 171. Implementation Phase D — Significance

Tasks:

1. rule catalog;
2. narrative weight;
3. tags;
4. mundane/notable/important/legendary;
5. milestone overrides;
6. monotonicity;
7. anti-farming;
8. calibration.

Exit:
significance reflects meaning rather than raw event count.

---

## 172. Implementation Phase E — Stack/Transformation Safety

Tasks:

1. lore policy per item type;
2. split promotion;
3. merge restriction;
4. repair identity;
5. upgrade identity;
6. dismantle/destruction;
7. lineage;
8. stress tests.

Exit:
provenance cannot be corrupted by inventory mechanics.

---

## 173. Implementation Phase F — Narrative Templates

Tasks:

1. `lore_templates.json`;
2. structured fact requirements;
3. localization;
4. deterministic template selection;
5. narrative-quality audit;
6. no unsupported emotional/context claims.

Exit:
history renders naturally without becoming source of truth.

---

## 174. Implementation Phase G — UI

Tasks:

1. item detail history;
2. provenance chain;
3. significance indicator;
4. compact tooltip;
5. filters/search;
6. historical lost/destroyed item view if supported;
7. accessibility;
8. snapshots.

Exit:
UI contains no significance/business logic.

---

## 175. Implementation Phase H — Journal/Quest/Archive

Tasks:

1. legendary notification;
2. archive candidate;
3. quest facts;
4. achievement facts;
5. epilogue facts;
6. no routine spam.

Exit:
important item histories can influence broader narrative systems.

---

## 176. Implementation Phase I — Migration & CI

Tasks:

1. old-save empty/unknown origin migration;
2. optional exact reconstruction;
3. save/load;
4. event dedup;
5. stack tests;
6. save-bloat stress;
7. fuzz;
8. selftest;
9. docs;
10. full regression.

Exit:
item lore is deterministic, sparse, and supportable.

---

## 177. Exact File Plan

Expected new/modified files, adjusted to repository conventions:

### Core
- `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`
- `Assets/Ashfall.Core/Inventory/ItemLoreState.cs`
- `Assets/Ashfall.Core/Inventory/ItemProvenanceState.cs`
- `Assets/Ashfall.Core/Inventory/ItemLoreEvent.cs`
- `Assets/Ashfall.Core/Inventory/ItemSignificanceState.cs`
- `Assets/Ashfall.Core/Inventory/ItemLoreEvents.cs`
- `Assets/Ashfall.Core/Inventory/ItemLorePolicyResolver.cs`
- `Assets/Ashfall.Core/Inventory/ItemSignificanceEvaluator.cs`

### Data
- `Assets/StreamingAssets/Data/lore_templates.json`

### Tests
- `Ashfall.Core.Tests/Inventory/ItemLoreSystemTests.cs`
- `Ashfall.Core.Tests/Inventory/ItemLorePersistenceTests.cs`
- `Ashfall.Core.Tests/Inventory/ItemLoreStackTests.cs`
- `Ashfall.Core.Tests/Inventory/ItemLoreSignificanceTests.cs`
- `Ashfall.Core.Tests/Inventory/ItemLoreIntegrationTests.cs`

### Docs
- `docs/item_lore/ITEM_LORE_INTEGRATION_AUDIT.md`
- `docs/item_lore/ITEM_SIGNIFICANCE_CALIBRATION.md`
- `docs/item_lore/ITEM_LORE_RETENTION.md`
- `docs/item_lore/ITEM_LORE_COVERAGE.md`
- `docs/item_lore/ITEM_LORE_MIGRATION.md`

---

## 178. Bootstrap / Composition Root

Recommended order:

```text
load item catalog
load lore template/rule catalog
validate
construct inventory/item instance authority
construct ItemLoreSystem
restore inventory
restore ItemLoreState
wire crafting/expedition/combat/trade/relations/world-event adapters
bind item detail projection
```

Item lore restore should occur after item instances are available, or restore must tolerate deferred resolution.

---

## 179. Restore Validation

On restore:

- every live lore-bearing item ID should resolve to live item or valid historical/terminal record;
- dangling live references reported;
- missing survivor/location/faction references degrade presentation safely;
- significance recompute optionally verifies stored state.

Do not crash because a historical owner died.

---

## 180. No Generic `TickItemLore`

The source proposes `TickItemLore`.

Prefer event-driven design.

A tick is unnecessary unless:
- delayed event compaction;
- archive promotion;
- time-age significance.

Even then, use day/year events, not per-frame.

---

## 181. Item Age

Age itself may contribute modest significance only if item remains relevant for long periods.

Do not tick daily.

Evaluate on:
- year boundary;
- major event;
- UI projection.

Avoid "wait 500 days = legendary."

---

## 182. Significance Diversity Rule

A healthy legendary history should usually involve multiple event families.

Possible rule:
- require at least 2–3 distinct high-weight categories.

This prevents one repeated mechanic from farming legendary status.

---

## 183. Manual Lore Suppression

Player may opt to hide mundane/notable history in UI.

Do not delete provenance.

Presentation filter only.

This supports low-clutter play without damaging save history.

---

## 184. Manual Lore Preservation

Player may mark an item "Preserve history."

Effect:
- retention policy keeps lower-weight entries;
- does not add significance.

Useful for personal stories.

---

## 185. Cultural/Narrative Priority

This system's strongest value is not stats.

Priorities:

1. provenance clarity;
2. emergent story;
3. heirloom continuity;
4. archive/epilogue hooks;
5. optional trade/cultural value.

Avoid adding combat buffs just because item is legendary.

---

## 186. Legendary Mechanical Bonuses — Defer

A legendary rifle should not automatically gain damage.

That would turn history into power-grinding.

If future design wants sentimental/psychological effects:
- use separate explicit plan;
- bounded;
- context-specific.

V1 significance is primarily narrative/economic metadata.

---

## 187. Item Loss Quest Hook

Future recovery mission can query:

```text
lost important/legendary items
```

ItemLoreSystem provides:
- item ID;
- last owner;
- loss location/event.

QuestSystem creates mission.

---

## 188. Historical Owner Chain and Deceased Survivors

An heirloom passed from a deceased survivor is especially meaningful.

Use Memorial/Survivor archive to resolve:
- name;
- relationship.

Do not create duplicate death data.

---

## 189. Item Lore and Child/Generational Systems

Plan 183 can eventually support:
- parent→child heirloom.

The item transfer uses:
- family relation fact;
- ownership event.

No child-specific item-lore subsystem.

---

## 190. Data Privacy / Debug Boundary

Lore is in-game data.

Developer logs may include IDs.

Normal player UI should show localized names, not raw GUIDs/source event IDs.

Support export can include structural provenance for bug reports.

---

## 191. Structured Diagnostics

Logs:

```text
ItemLorePromoted item=<id> reason=<rule>
ItemOriginRecorded item=<id> origin=<kind>
ItemOwnershipTransferred item=<id> from=<owner> to=<owner>
ItemLoreEventRecorded item=<id> type=<type> source=<event>
ItemSignificanceChanged item=<id> from=<level> to=<level>
ItemLoreTerminal item=<id> status=<destroyed|departed>
```

No per-frame logging.

---

## 192. Release Gate

Release fails if:

- item IDs unstable across save/load;
- commodity stacks generate lore state by default;
- stack merge loses distinct identities;
- duplicate source event creates duplicate lore;
- trade/gift/drop farming increases significance indefinitely;
- old saves fabricate provenance;
- legendary rule can be reached through trivial repeated events;
- item UI calculates significance itself;
- selftest fails;
- save-bloat stress exceeds approved limits.

This plan's risk is not algorithmic complexity; it is systemic data explosion and false provenance.

---

## 193. Definition of Done — Flagship

### Identity
- [ ] stable item-instance IDs
- [ ] lore eligibility policy
- [ ] commodity vs identity-bearing classification
- [ ] stack split/merge policy

### Provenance
- [ ] one origin
- [ ] crafted origin
- [ ] found/looted origin
- [ ] legacy unknown origin
- [ ] ownership history
- [ ] current owner/bearer semantics

### Event ingestion
- [ ] crafting
- [ ] discovery
- [ ] combat
- [ ] gift
- [ ] trade
- [ ] loss/recovery
- [ ] significant moment
- [ ] stable source-event idempotency

### Significance
- [ ] mundane
- [ ] notable
- [ ] important
- [ ] legendary
- [ ] weighted rules
- [ ] significance tags
- [ ] anti-farming
- [ ] calibration report

### Lifecycle
- [ ] repair
- [ ] upgrade
- [ ] transform
- [ ] destruction
- [ ] departure/trade
- [ ] recovery
- [ ] lineage where needed

### UI
- [ ] item detail history
- [ ] provenance
- [ ] significance indicator
- [ ] compact tooltip
- [ ] filters
- [ ] accessibility
- [ ] no business logic

### Integrations
- [ ] ProceduralItemInstance
- [ ] Crafting
- [ ] Expedition
- [ ] Combat
- [ ] Relations
- [ ] Trade
- [ ] Journal
- [ ] Archive
- [ ] Quest/Achievement
- [ ] Epilogue facts

### Validation
- [ ] old-save migration
- [ ] save/load
- [ ] no duplicate events
- [ ] stack safety
- [ ] save-bloat stress
- [ ] selftest
- [ ] data-integrity
- [ ] headless

---

## 194. Follow-On Task 190-A — Legendary Item Illustration Overrides

Goal:
allow important/legendary items to use unique or upgraded presentation art.

Requires:
- asset registry;
- stable item ID;
- no runtime generation requirement.

The lore system only exposes significance.

---

## 195. Follow-On Task 190-B — Item Recovery Missions

Goal:
turn lost important artifacts into quest targets.

Inputs:
- lost item ID;
- loss location/context;
- significance.

QuestSystem owns mission lifecycle.

---

## 196. Follow-On Task 190-C — Artifact Appraisal & Cultural Trade

Goal:
let factions value provenance differently.

Requires:
- faction preference data;
- economy integration;
- anti-loop logic.

No duplicate currency/standing authority.

---

## 197. Follow-On Task 190-D — Item Theft

Goal:
support stealing and recovering unique artifacts.

Requires:
- transfer event;
- external ownership persistence;
- recovery quests.

ItemLoreSystem already preserves provenance.

---

## 198. Follow-On Task 190-E — Cross-Campaign Heirlooms

Goal:
let Plan 175/later meta systems carry selected legendary artifacts into legacy modes.

Plan 190 exports a read-only artifact package.

It must not own profile persistence.

---

## 199. Follow-On Task 190-F — Provenance Discovery

Goal:
support discovering pre-campaign or hidden history for rare artifacts.

Requires:
- verified/claimed provenance distinction;
- research/quest hooks;
- no retcon of established facts.

Separate from normal run-local provenance.

---

## 200. Final Guardrails

- No lore for every commodity unit.
- No provenance keyed only by static item definition.
- No unstable inventory-slot identity.
- No generic per-frame item-lore scan.
- No `System.Random`.
- No wall-clock lore generation.
- No duplicate event from summaries/replays.
- No combat log dump as lore.
- No equip/unequip ownership spam.
- No trade/gift/drop farming.
- No simple lore-count-only legendary rule.
- No fabricated emotional reasons.
- No fabricated old-save origins.
- No merging distinct lore-bearing items into anonymous stack.
- No duplicating an item on trade/recovery.
- No deleting lore when condition changes.
- No copying entire component histories into crafted descendant.
- No UI-owned significance calculation.
- No direct combat/stat buff merely for legendary status.
- No unbounded ownership/event lists.
- No profile/meta legacy ownership.
- No unresolved live-item provenance without diagnostics.

When complete, Plan 190 should allow a handful of objects in a campaign to become genuinely memorable:
the axe Elena built and Marcus later carried through a raid; the rifle lost in a flood and recovered downstream;
the knife passed from parent to child; the tool that survived the shelter's worst winter; the weapon carried
through the final battle.

The engineering proof is that these stories arise from real authoritative gameplay events, preserve one stable
physical identity, remain sparse enough for production saves, cannot be farmed by trivial repetition, survive
ownership and condition changes, and integrate with inventory rather than replacing it.
