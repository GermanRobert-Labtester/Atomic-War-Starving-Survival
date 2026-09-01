# Plan 190 — Item Lore & Provenance Tracking

## Goal

Create an item lore and provenance tracking system where significant items carry history — who crafted them, where they were found, who owned them before, what events they witnessed. Currently `ProceduralItemInstance.cs` tracks condition, contamination, and caloric variance for procedural items, and `ItemCatalogLoader`/`ItemDefinitions` define static item data, but there is no item history, no provenance tracking, no ownership chain, no event association, no lore accumulation. Items are interchangeable units with no memory. This plan adds narrative depth to the inventory system, making significant items feel like artifacts with stories.

## Why

**Repository evidence:** Grep for `ItemLore`, `ItemHistory`, `ItemProvenance`, `CraftedHistory`, `ItemOrigin`, `ArtifactHistory`, `OwnershipChain`, `ItemMemory` in Core returns ZERO matches. `ProceduralItemInstance.cs` tracks condition, contamination, purity, caloric value, and expiration — but no history, no provenance, no lore. `ItemCatalogLoader`/`ItemDefinitions` define static item properties — but no dynamic history. Items are created, used, and destroyed with no memory of their journey.

**What is missing:** No item lore system. No provenance tracking. No ownership history. No crafting history (who made this). No discovery history (where was this found). No event association (this item was present during X). No item memory. No narrative accumulation. Every item of the same type is identical — no unique stories.

**Why existing plans don't solve it:** Plan 185 (memory decay) adds memory for survivors but not for items. Plan 147 (per-NPC memory) adds NPC memory but not item memory. Plan 161 (hobbies) adds crafted items but not crafted history. No plan addresses item lore/provenance as a system.

**Player value:** Creates emotional attachment (this is MY axe, crafted by Elena, used in the raid on Day 47), adds narrative depth (items tell stories), generates emergent stories (heirloom weapons passed between survivors), and makes significant items feel unique rather than interchangeable.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs` — procedural item tracking
- `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` — item catalog
- `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs` — item definitions
- `Assets/Ashfall.Core/Crafting/` — crafting system
- `Assets/Ashfall.Core/Expedition/` — expedition system (item discovery)
- NEW: `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs`
- NEW: `Assets/StreamingAssets/Data/lore_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `ItemLoreSystem.cs` in `Assets/Ashfall.Core/Inventory/`
2. Define `ItemLore` DTO: `loreId`, `itemId` (procedural item instance id), `loreType` (origin/crafting/ownership/event/significance), `loreText` (narrative description), `day` (when lore was added), `associatedSurvivorId` (optional, survivor involved in lore), `associatedLocationId` (optional, location involved in lore), `associatedEventId` (optional, event involved in lore)
3. Define `ItemProvenance` DTO: `provenanceId`, `itemId`, `ownershipChain` (list of previous owner survivor_ids), `craftingSurvivorId` (who crafted it, if crafted), `craftingDay` (day crafted), `discoveryLocationId` (where found), `discoveryDay` (day found), `discoveryContext` (how found: "salvaged from ruin", "crafted by survivor", "looted from enemy", "traded from faction", etc.)
4. Define `ItemEvent` DTO: `eventItemId`, `itemId`, `eventId`, `eventType` (combat/crafting/trade/gift/loss/recovery/significant_moment), `day`, `description`, `associatedSurvivorIds` (list), `associatedLocationId`
5. Define `ItemSignificance` DTO: `significanceId`, `itemId`, `significanceLevel` (mundane/notable/important/legendary), `significanceTags` (list of tags: "heirloom", "weapon_of_battle", "first_craft", "gift_from_friend", "looted_from_enemy"), `narrativeWeight` (0-100, how much this item matters to the story)
6. Define `ItemLoreState` DTO: list of item lores, list of item provenances, list of item events, list of item significances, lore generation settings (lore density, significance thresholds)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define lore generation triggers:
   - **Crafting**: when survivor crafts item, lore added ("Crafted by Elena on Day 47 in the workshop")
   - **Discovery**: when item found, lore added ("Salvaged from the ruins of Sector 7 on Day 23")
   - **Combat**: when item used in significant combat, lore added ("Used in the raid on Day 89, drew first blood")
   - **Gift**: when item given between survivors, lore added ("Gifted from Elena to Marcus on Day 120, a token of trust")
   - **Trade**: when item traded with factions, lore added ("Traded with the Iron Raiders on Day 156 for medical supplies")
   - **Loss/Recovery**: when item lost and recovered, lore added ("Lost during the flood on Day 200, recovered three days downstream")
   - **Significant Moment**: when item present at significant event, lore added ("Present at the founding of the Commonwealth, Day 365")
9. Define provenance tracking:
   - Each item has ownership chain (list of previous owners)
   - Each item has crafting origin (who made it, when)
   - Each item has discovery origin (where found, when, how)
   - Provenance persists through ownership changes
   - Provenance displayed in item detail
10. Define significance levels:
    - **Mundane**: common items, no lore generated (rocks, sticks, basic supplies)
    - **Notable**: items with some history (crafted by named survivor, used in combat)
    - **Important**: items with significant history (heirloom, weapon of battle, gift between friends)
    - **Legendary**: items with epic history (present at founding, used in final battle, passed through generations)
11. Define lore text templates:
    - Crafting: "Crafted by {survivor} on Day {day} in {location}"
    - Discovery: "Found in {location} on Day {day}, {context}"
    - Combat: "Used in combat on Day {day}, {combat_description}"
    - Gift: "Gifted from {giver} to {receiver} on Day {day}, {reason}"
    - Trade: "Traded with {faction} on Day {day}, {trade_description}"
    - Loss/Recovery: "Lost on Day {day}, {loss_description}. Recovered on Day {recovery_day}, {recovery_description}"
    - Significant: "Present at {event} on Day {day}, {event_description}"
12. Define lore accumulation:
    - Items accumulate lore over time
    - Each significant event adds lore entry
    - Lore list viewable in item detail
    - Lore affects item significance level
    - High-significance items have narrative weight
13. Define lore display:
    - Item detail panel shows lore list
    - Provenance shown (ownership chain, crafting origin, discovery origin)
    - Significance level shown (mundane/notable/important/legendary)
    - Lore entries in chronological order
    - Lore text in narrative tone
14. Add deterministic seeding: lore generation uses `ISeededRng`
15. Wire into `GameBootstrap`: `SetupItemLore`, `TickItemLore` (process lore generation), `SaveItemLore`

## Main Task 2 — Implementation / Lore / Provenance / Events / Significance / UI

1. Implement lore generation:
   - Detect lore trigger events (crafting, discovery, combat, gift, trade, loss/recovery, significant moment)
   - Generate lore entry with template
   - Add lore to item
   - Update item significance
   - Lore generation logged
2. Implement provenance tracking:
   - Track ownership chain (each owner added to chain)
   - Track crafting origin (crafter, day, location)
   - Track discovery origin (location, day, context)
   - Provenance persists through ownership changes
   - Provenance displayed in item detail
3. Implement event association:
   - When item present at significant event, event logged
   - Event types: combat, crafting, trade, gift, loss/recovery, significant moment
   - Event description generated
   - Event added to item history
   - Event affects significance
4. Implement significance calculation:
   - Calculate significance based on lore count and types
   - Mundane: 0-2 lore entries
   - Notable: 3-5 lore entries
   - Important: 6-10 lore entries
   - Legendary: 11+ lore entries or specific legendary events
   - Significance displayed in item detail
5. Implement lore templates:
   - Crafting template: "Crafted by {survivor} on Day {day}..."
   - Discovery template: "Found in {location} on Day {day}..."
   - Combat template: "Used in combat on Day {day}..."
   - Gift template: "Gifted from {giver} to {receiver}..."
   - Trade template: "Traded with {faction} on Day {day}..."
   - Loss/Recovery template: "Lost on Day {day}... Recovered on Day {recovery_day}..."
   - Significant template: "Present at {event} on Day {day}..."
6. Implement lore UI:
   - Item detail panel: lore list, provenance, significance
   - Lore timeline: chronological lore entries
   - Provenance chain: ownership history
   - Significance indicator: mundane/notable/important/legendary
   - Lore search: find items by lore type/significance
7. Implement lore persistence:
   - Lore saved with item instance
   - Lore persists through save/load
   - Lore persists through ownership changes
   - Lore persists through item condition changes
8. Implement lore inheritance:
   - When item passed to new owner, lore continues
   - New owner added to ownership chain
   - Lore visible to new owner
   - Item carries history forward
9. Implement lore discovery:
   - When item found, discovery lore added
   - Discovery context recorded (salvaged, looted, traded, gifted)
   - Discovery location recorded
   - Discovery day recorded
10. Create lore events:
    - "The Crafting" — item crafted with lore
    - "The Discovery" — item found with lore
    - "The Battle" — item used in significant combat
    - "The Gift" — item given between survivors
    - "The Trade" — item traded with faction
    - "The Loss" — item lost
    - "The Recovery" — item recovered
    - "The Legend" — item becomes legendary
11. Add lore quest hooks:
    - "The Collector" — accumulate 10 notable items
    - "The Historian" — generate 50 lore entries
    - "The Heirloom" — pass item through 3 owners
    - "The Legend" — create a legendary item
    - "The Crafter" — craft 20 items with lore
    - "The Trader" — trade 10 items with lore
    - "The Story" — item present at 5 significant events
12. Implement item detail UI enhancement:
    - Lore tab: list of lore entries
    - Provenance tab: ownership chain, crafting/discovery origin
    - Significance indicator: level and tags
    - Lore generation toggle: enable/disable lore for item type
13. Add lore journal: automatic log of significant lore events
14. Implement lore tutorial: first crafted item explains lore system
15. Add lore tooltips: hover over item shows significance, lore count

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ProceduralItemInstance`: lore attached to procedural items
2. Connect to `CraftingSystem`: crafting generates lore
3. Integrate with `ExpeditionSystem`: discovery generates lore
4. Connect to `CombatSystem`: combat generates lore
5. Wire into `SurvivorRelationsSystem`: gift/trade generates lore
6. Connect to `TradeSystem`: trade generates lore
7. Implement old-save compatibility: existing saves get empty lore (all items mundane)
8. Add deterministic seeding: lore uses `ISeededRng`
9. Create exploit prevention: lore is event-based, can't be farmed
10. Add tests: lore generation, provenance tracking, event association, significance calculation, save round-trip
11. Verify all lore types generate correctly
12. Test edge cases: no lore (mundane items), extensive lore (legendary items)
13. Verify headless behavior: lore processes correctly without UI
14. Add data-integrity-selftest: lore validates against item/survivor/location catalogs
15. Create `--item-lore-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --item-lore-selftest
```

## Risk

**LOW** — Item lore is straightforward with clear inputs (events) and outputs (lore entries, significance). Risk of lore feeling like clutter rather than narrative enrichment. Mitigation: make lore optional (toggle per item type), show clear significance levels, ensure lore adds meaning not noise, and allow players to focus on items that matter to them.

## Definition of Done

- `ItemLoreSystem.cs` exists with full `CaptureState/RestoreState`
- Lore generation for 7 trigger types (crafting, discovery, combat, gift, trade, loss/recovery, significant moment)
- Provenance tracking (ownership chain, crafting origin, discovery origin)
- Event association (items present at events)
- Significance levels (mundane/notable/important/legendary)
- Lore text templates (narrative descriptions)
- Lore accumulation (items gain lore over time)
- Lore persistence (saved with items, persists through ownership changes)
- Lore inheritance (passed between owners)
- Lore events and quest hooks
- Save/load round-trip tested
- Deterministic lore generation verified
- Old saves load with empty lore
- Lore templates in data authority
- UI item detail enhancement (lore tab, provenance tab, significance indicator)
- Cross-system integration (procedural items, crafting, expedition, combat, relations, trade)

## Follow-On Opportunities

- Item illustration unlocks (legendary items get unique art)
- Item legacy (legendary items remembered across campaigns)
- Item quests (specific item goals)
- Item events (item theft, item recovery missions)
- Item trading (lore increases item value)
