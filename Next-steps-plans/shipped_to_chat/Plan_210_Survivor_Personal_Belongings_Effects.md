# Plan 210 — Survivor Personal Belongings & Effects

## Goal

Create a survivor personal belongings system where each survivor owns personal possessions (keepsakes, clothing, tools, mementos) separate from the shared shelter inventory — items that have sentimental value, affect morale, and can be inherited on death. Currently all items live in a shared shelter inventory (`Inventory.cs`) — survivors don't have personal possessions, no keepsakes, no personal clothing beyond equipped gear, no sentimental items, no personal effects. When a survivor dies, nothing is left behind. This plan gives each survivor material identity.

## Why

**Repository evidence:** Grep for `PersonalBelongings`, `PersonalInventory`, `SurvivorPossessions`, `PersonalEffects`, `Keepsake`, `SentimentalItem`, `PersonalClothing`, `IndividualInventory` in Core returns ZERO matches. `Inventory.cs` manages shared shelter inventory with `WornGear` for equipped items. `EquipmentConditionSystem` (189 lines) tracks equipment condition. But no personal possessions — no keepsakes, no sentimental items, no personal clothing beyond worn gear, no individual ownership.

**What is missing:** No personal belongings per survivor. No keepsakes or mementos. No sentimental items. No personal clothing (beyond equipped gear). No individual ownership of possessions. No personal effects that survive death. All items are shared or equipped — nothing is personally owned.

**Why existing plans don't solve it:** Plan 206 (Death & Inheritance) covers what happens to possessions on death but assumes possessions exist. Plan 190 (Item Lore) adds item history but not personal ownership. Plan 200 (Personal Quests) adds narrative arcs but not material possessions. No plan addresses personal belongings as a system.

**Player value:** Creates individual identity (each survivor has stuff), adds emotional attachment (sentimental items matter), generates emergent stories (stolen keepsakes, inherited mementos), and makes death more meaningful (possessions left behind).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Inventory/Inventory.cs` — shared inventory
- `Assets/Ashfall.Core/Inventory/EquipmentConditionSystem.cs` — equipment condition
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships (gift giving)
- `Assets/Ashfall.Core/MemorialSystem.cs` — remembrance (complementary)
- NEW: `Assets/Ashfall.Core/Survivors/PersonalBelongingsSystem.cs`
- NEW: `Assets/StreamingAssets/Data/keepsake_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `PersonalBelongingsSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `PersonalBelonging` DTO: `belongingId`, `ownerSurvivorId`, `itemId` (item definition id), `itemName`, `itemCategory` (keepsake/clothing/tool/weapon/memento/document/jewelry), `sentimentalValue` (0-100, emotional attachment), `condition` (0-100, physical state), `acquiredDay`, `acquiredFrom` (survivor_id or location or crafted), `description` (personal significance), `isFavorite` bool, `isInherited` bool (received from deceased survivor)
3. Define `KeepsakeTemplate` DTO: `templateId`, `templateName`, `category` (keepsake/clothing/tool/weapon/memento/document/jewelry), `baseSentimentalValue` (0-100), `rarity` (common/uncommon/rare/unique), `description` (flavor text), `associatedMemory` (memory/event that created this keepsake)
4. Define `BelongingTransfer` DTO: `transferId`, `belongingId`, `fromSurvivorId`, `toSurvivorId`, `transferType` (gift/inheritance/trade/theft/confiscation/assignment), `transferDay`, `reason`, `sentimentalEffect` (morale change for both parties)
5. Define `BelongingEvent` DTO: `eventId`, `eventType` (item_acquired/item_lost/item_damaged/item_destroyed/item_gifted/item_inherited/item_stolen/item_favorite_set), `survivorId`, `belongingId`, `day`, `description`, `moraleEffect` (float)
6. Define `PersonalBelongingsState` DTO: dict of survivor_id → list of personal belongings, list of belonging transfers, list of belonging events, settings (max belongings per survivor, sentimental decay rate, inheritance enabled bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define belonging categories (7 types):
   - **Keepsake**: personal memento (photo, letter, trinket), high sentimental value
   - **Clothing**: personal clothing beyond standard uniform (hat, scarf, jacket), moderate sentimental
   - **Tool**: personal tool (customized knife, favorite wrench), moderate sentimental + functional
   - **Weapon**: personal weapon (named gun, inherited blade), moderate sentimental + functional
   - **Memento**: reminder of event (medal, token, souvenir), high sentimental value
   - **Document**: personal papers (diary, letter, certificate), high sentimental/informational value
   - **Jewelry**: personal adornment (ring, necklace, bracelet), high sentimental value
9. Define acquisition mechanics:
   - Belongings acquired through: events, crafting, gifts, inheritance, discovery, assignment
   - Each survivor has max belongings (default: 10)
   - Belongings have sentimental value (0-100)
   - Belongings have condition (0-100, degrades over time)
   - Acquisition logged
10. Define sentimental value mechanics:
    - Sentimental value affects morale when possessed
    - Losing a beloved item: morale penalty
    - Receiving a sentimental gift: morale boost
    - High sentimental items: stronger morale effects
    - Sentimental value can increase over time (attachment grows)
11. Define gift-giving mechanics:
    - Survivors can give belongings to each other
    - Gift giving: morale boost for both (giver: generosity, receiver: appreciation)
    - Gift giving strengthens relationship
    - Refused gift: morale penalty, relationship damage
    - Gift logged
12. Define inheritance mechanics:
    - On owner death: belongings distributed (Plan 206 integration)
    - Inherited items carry sentimental value
    - Inherited items may have increased sentimental value (memory of deceased)
    - Inheritance logged
13. Define theft/loss mechanics:
    - Belongings can be stolen (by other survivors or external)
    - Stolen items: morale penalty for owner, possible relationship damage
    - Lost items (destroyed, discarded): morale penalty
    - Theft/loss logged
14. Define favorite mechanics:
    - Survivor can designate one belonging as "favorite"
    - Favorite item: doubled morale effects
    - Losing favorite: doubled morale penalty
    - Favorite logged
15. Add deterministic seeding: belonging events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupPersonalBelongings`, `TickPersonalBelongings`, `SavePersonalBelongings`

## Main Task 2 — Implementation / Belongings / Sentiment / Gifts / Inheritance / UI

1. Implement personal belongings:
   - Each survivor has list of belongings
   - Belongings have category, sentimental value, condition
   - Belongings can be acquired/lost
   - Belongings logged
2. Implement sentimental value:
   - Sentimental value affects morale
   - Value can increase over time (attachment)
   - Value decreases if item damaged
   - Sentiment logged
3. Implement gift-giving:
   - Survivor gives belonging to another
   - Both get morale effects
   - Relationship affected
   - Gift logged
4. Implement inheritance:
   - On death: belongings distributed
   - Inherited items carry sentimental value
   - Inheritance logged
5. Implement theft/loss:
   - Belongings can be stolen/lost
   - Owner morale penalty
   - Theft logged
6. Implement favorites:
   - Survivor designates favorite belonging
   - Favorite: doubled morale effects
   - Favorite logged
7. Implement condition degradation:
   - Belongings degrade over time
   - Degraded items: reduced sentimental value
   - Destroyed items: lost
   - Condition logged
8. Implement belonging UI:
   - Belongings panel: per-survivor list of possessions
   - Belonging detail: category, sentimental value, condition, description
   - Gift panel: give belonging to another survivor
   - Favorite designation: mark/unmark favorite
   - Belonging log: history of acquisitions/transfers
   - Inheritance panel: pending inheritances
9. Create belonging events:
    - "The Keepsake" — survivor acquires keepsake
    - "The Gift" — belonging given as gift
    - "The Inheritance" — belonging inherited
    - "The Loss" — belonging lost/destroyed
    - "The Theft" — belonging stolen
    - "The Favorite" — belonging marked as favorite
    - "The Memory" — sentimental value increased
    - "The Attachment" — long-term belonging bond
10. Add belonging quest hooks:
    - "The Collector" — acquire 20 personal belongings
    - "The Generous" — give 10 gifts
    - "The Sentimental" — have 5 belongings with 80+ sentimental value
    - "The Heir" — inherit belongings from 5 deceased survivors
    - "The Curator" — maintain 10 belongings in 90+ condition
    - "The Favorite" — designate favorites for all survivors
    - "The Memory Keeper" — have belongings with total sentimental value 500+
11. Implement belonging tutorial: first keepsake acquired explains system
12. Add belonging tooltips: hover over belonging shows details
13. Create keepsake templates in data file (30+ templates)
14. Implement belonging persistence: belongings saved with survivor state
15. Integrate with `DeathLegacySystem` (Plan 206): inheritance on death

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `Inventory`: belongings separate from shared inventory
2. Connect to `SurvivorRelationsSystem`: gifts affect relationships
3. Integrate with `MemorialSystem`: inherited items carry memory
4. Connect to `NeedsSystem`: sentimental items affect morale
5. Wire into `DeathLegacySystem` (Plan 206): inheritance distribution
6. Connect to `InterpersonalConflictSystem` (Plan 202): theft triggers conflict
7. Implement old-save compatibility: existing saves get no belongings
8. Add deterministic seeding: belonging events use `ISeededRng`
9. Create exploit prevention: belongings are finite, can't be gamed
10. Add tests: belongings, sentimental value, gifts, inheritance, theft, favorites, save round-trip
11. Verify all belonging categories work correctly
12. Test edge cases: no belongings (current behavior), many belongings (sentimental overload)
13. Verify headless behavior: belongings process correctly without UI
14. Add data-integrity-selftest: belongings validate against survivor/item catalogs
15. Create `--personal-belongings-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --personal-belongings-selftest
```

## Risk

**LOW** — Personal belongings are straightforward with clear inputs (acquisitions) and outputs (morale effects, inheritance). Risk of belongings feeling like inventory clutter. Mitigation: make sentimental items meaningful, show clear morale effects, and ensure belongings feel like personal identity not just items.

## Definition of Done

- `PersonalBelongingsSystem.cs` exists with full `CaptureState/RestoreState`
- 7 belonging categories (keepsake, clothing, tool, weapon, memento, document, jewelry)
- Personal belongings per survivor (max 10 default)
- Sentimental value mechanics (0-100, affects morale)
- Gift-giving mechanics (morale effects, relationship impact)
- Inheritance on death (Plan 206 integration)
- Theft/loss mechanics (morale penalties)
- Favorite designation (doubled morale effects)
- Condition degradation over time
- Belonging events and quest hooks
- Save/load round-trip tested
- Deterministic belonging events verified
- Old saves load with no belongings
- Keepsake templates in data authority (30+ templates)
- UI belongings panel, belonging detail, gift panel, favorite designation, log, inheritance panel
- Cross-system integration (inventory, relations, memorial, needs, death legacy, conflicts)

## Follow-On Opportunities

- Belonging specialization (survivors become expert craftspeople creating high-sentiment items)
- Belonging legacy (famous keepsakes remembered across campaigns)
- Belonging quests (specific belonging goals)
- Belonging events (legendary keepsake discovered, mass gift-giving ceremony)
- Belonging trading (trade sentimental items with other settlements)
