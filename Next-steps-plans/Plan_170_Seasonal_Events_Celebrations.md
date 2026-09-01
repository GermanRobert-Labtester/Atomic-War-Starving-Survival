# Plan 170 — Seasonal Events & Celebrations System

## Goal

Create a seasonal events and celebrations system where the shelter observes holidays, anniversaries, and seasonal festivals that boost morale, create traditions, and mark the passage of time. Currently `FeedbackMessageCatalogLoader.cs` contains two hint strings — "Small celebrations boost morale more than large ones" and "Morale low: Survivors need a celebration to boost spirits" — but no celebration system exists. There are no holidays, no anniversaries, no seasonal festivals, no traditions. Time passes but is unmarked. This plan adds temporal rhythm and communal joy to the shelter's existence.

## Why

**Repository evidence:** Grep for `seasonal_event`, `SeasonalEvent`, `HolidaySystem`, `FestivalSystem` returns only 2 feedback hint strings in `FeedbackMessageCatalogLoader.cs` (lines 193, 254). No holiday system, no celebration mechanic, no seasonal events, no anniversary tracking. `CampaignCalendar.cs` tracks days but doesn't mark special dates. `NeedsSystem.cs` tracks morale but has no celebration-based morale boost. The shelter has no traditions, no holidays, no communal celebrations.

**What is missing:** No seasonal events. No holidays. No celebrations. No anniversaries (shelter founding, survivor deaths). No festivals. No traditions. The feedback hints suggest celebrations should exist, but the system was never built. Time passes without rhythm or meaning.

**Why existing plans don't solve it:** Plan 19 (dynamic world) mentions seasonal cadence but not celebrations. Plan 83 (weather seasons) adds seasonal data but not events. Plan 135 (weather cascade) connects weather to gameplay but not seasonal festivities. Plan 164 (nuclear winter) adds seasonal progression but not celebrations. No plan addresses holidays, festivals, or communal events.

**Player value:** Creates temporal rhythm (holidays mark time passing), boosts morale (celebrations provide relief), adds traditions (shelter develops culture), generates emergent stories (annual anniversaries, seasonal festivals), and makes the shelter feel like a community rather than a survival machine.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — day tracking
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — morale tracking
- `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs` — celebration hints
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs` — death anniversaries
- `Assets/Ashfall.Core/Shelter/` — shelter-related systems
- NEW: `Assets/Ashfall.Core/Events/SeasonalEventSystem.cs`
- NEW: `Assets/StreamingAssets/Data/seasonal_events.json`

## Main Task 1 — Foundation / System Contract

1. Create `SeasonalEventSystem.cs` in `Assets/Ashfall.Core/Events/`
2. Define `SeasonalEvent` DTO: `eventId`, `eventName`, `eventType` (holiday/anniversary/festival/tradition/memorial), `triggerDay` (day of year or specific date), `triggerCondition` (optional: morale threshold, season, nuclear winter phase), `durationDays`, `moraleBoost` (0-20), `resourceCost` (list of items consumed), `activities` (list of activity options), `description`, `flavorText`
3. Define `Celebration` DTO: `celebrationId`, `eventId`, `celebrationDay`, `scale` (small/medium/large), `participants` (list of survivor IDs), `activitiesChosen` (list), `moraleGained`, `resourcesSpent`, `memorable` bool (recorded in archive)
4. Define `Anniversary` DTO: `anniversaryId`, `anniversaryType` (founding/memorial/achievement), `referenceDay` (day being commemorated), `referenceName` (what is remembered), `yearsSince` (how many years), `moraleEffect` (positive for founding/achievement, mixed for memorial)
5. Define `SeasonalEventState` DTO: list of seasonal events defined, list of celebrations held, list of anniversaries tracked, last celebration day, tradition streaks (consecutive years celebrating)
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define seasonal event types:
   - **Holidays**: fixed-date events (New Year, Midwinter, Harvest, Solstice)
   - **Anniversaries**: shelter founding, survivor death remembrances
   - **Festivals**: seasonal celebrations (Spring Festival, Summer Feast, Autumn Harvest, Winter Solstice)
   - **Traditions**: shelter-specific recurring events (established by player)
   - **Memorials**: annual remembrance of fallen survivors
8. Define seasonal event calendar:
   - **Day 1**: New Year (fresh start, hope)
   - **Day 90**: Spring Festival (renewal, planting)
   - **Day 180**: Midsummer Feast (abundance, community)
   - **Day 270**: Harvest Festival (gratitude, preparation)
   - **Day 360**: Winter Solstice (endurance, reflection)
   - **Shelter Founding Day**: anniversary of shelter establishment
   - **Memorial Days**: anniversary of each survivor death
9. Define celebration mechanics:
   - Player can choose to celebrate or ignore seasonal events
   - Celebrations have scale: small (minimal resources, small boost), medium (moderate cost, moderate boost), large (significant cost, large boost)
   - Celebrations consume resources (food, fuel, materials)
   - Celebrations provide morale boost to all participants
   - Large celebrations can become memorable (recorded in archive)
   - Ignoring events: no cost, no benefit, slight morale penalty ("they forgot again")
10. Define anniversary mechanics:
    - Shelter founding anniversary: positive morale, community pride
    - Survivor death anniversaries: mixed morale (sadness + remembrance)
    - Achievement anniversaries: positive morale, pride
    - Anniversaries auto-occur (no player choice)
    - First anniversary: small event, fifth: medium, tenth: large
11. Define tradition mechanics:
    - Player can establish shelter traditions (recurring celebrations)
    - Traditions recur annually on chosen date
    - Traditions build streak (consecutive years = stronger morale)
    - Breaking tradition streak: morale penalty
    - Traditions become part of shelter identity (Plan 166 integration)
12. Define activity options for celebrations:
    - **Feast**: consume food, morale boost
    - **Music**: consume fuel (for instruments/light), morale boost
    - **Stories**: survivors share stories, morale boost + bond strengthening
    - **Games**: survivors compete, morale boost + skill practice
    - **Memorial**: remember fallen, mixed morale (sadness + comfort)
    - **Dance**: morale boost + fatigue reduction
    - **Gifts**: consume items, morale boost + relationship strengthening
13. Add deterministic seeding: celebration outcomes use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupSeasonalEvents`, `TickSeasonalEvents`, `SaveSeasonalEvents`
15. Create `SeasonalEventCatalogLoader` for event definitions

## Main Task 2 — Implementation / Calendar / Celebrations / Anniversaries / Traditions

1. Implement seasonal event calendar:
   - Events defined in data file with trigger days
   - System checks each day if event triggers
   - Event notification shown to player
   - Player can choose to celebrate or ignore
   - Event description and activity options displayed
2. Implement celebration system:
   - Player selects celebration scale (small/medium/large)
   - Player chooses activities (feast, music, stories, etc.)
   - Resources consumed based on scale and activities
   - Morale boost calculated from scale + activities + participation
   - Celebration result shown (morale gained, resources spent)
   - Large celebrations marked as memorable (archive entry)
3. Implement anniversary system:
   - Shelter founding anniversary auto-triggers
   - Survivor death anniversaries auto-trigger
   - Anniversary notification shown
   - Anniversary provides morale effect (positive or mixed)
   - Anniversary recorded in journal
   - Major anniversaries (5th, 10th, 25th) have larger effects
4. Implement tradition system:
   - Player can establish tradition (choose date, name, activities)
   - Tradition recurs annually
   - Tradition streak tracked (consecutive years)
   - Celebrating tradition: morale boost + streak bonus
   - Breaking tradition: morale penalty, streak reset
   - Traditions displayed in shelter identity panel
5. Implement activity mechanics:
   - **Feast**: consumes food items, +morale based on food quality
   - **Music**: consumes fuel, +morale, survivors bond
   - **Stories**: no cost, +morale, +bond between participants
   - **Games**: no cost, +morale, skill practice (small XP)
   - **Memorial**: no cost, mixed morale (sadness + comfort)
   - **Dance**: +morale, -fatigue
   - **Gifts**: consumes items, +morale, +relationship between giver/receiver
6. Implement participation mechanics:
   - All survivors can participate (or opt out)
   - Participation affected by morale (low morale survivors may not participate)
   - Participation affected by relationships (feuding survivors may avoid)
   - Participation recorded (who attended, who skipped)
   - High participation: bonus morale
   - Low participation: reduced morale boost
7. Implement resource cost system:
   - Small celebration: minimal resources (1 food per participant)
   - Medium celebration: moderate resources (3 food + 1 fuel per participant)
   - Large celebration: significant resources (5 food + 2 fuel + 1 material per participant)
   - Resources must be available or celebration cannot proceed
   - Resource scarcity affects celebration quality
8. Create seasonal events:
   - "The New Year" — fresh start, hope, +morale
   - "The Spring Festival" — renewal, planting, +morale
   - "The Midsummer Feast" — abundance, community, +morale
   - "The Harvest Festival" — gratitude, preparation, +morale
   - "The Winter Solstice" — endurance, reflection, +morale
   - "The Founding" — shelter anniversary, +morale
   - "The Remembrance" — survivor death anniversary, mixed morale
   - "The Tradition" — shelter tradition celebration, +morale + streak
9. Add seasonal quest hooks:
   - "The First Celebration" — hold first seasonal event
   - "The Tradition" — establish shelter tradition
   - "The Streak" — celebrate tradition 3 years in a row
   - "The Grand Festival" — hold large celebration
   - "The Remembrance" — hold memorial for fallen survivor
   - "The Community" — all survivors participate in celebration
   - "The Legacy" — tradition continues for 10 years
10. Implement seasonal event UI:
    - Calendar panel: shows upcoming events, anniversaries
    - Celebration modal: choose scale, activities, participants
    - Event notification: popup when event triggers
    - Tradition panel: manage shelter traditions
    - Anniversary display: shows upcoming anniversaries
11. Add event journal: automatic log of celebrations and anniversaries
12. Implement event tutorial: first seasonal event explains system
13. Add event tooltips: hover over event shows details
14. Create 7 seasonal events + anniversary templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `NeedsSystem`: celebrations provide morale boost
2. Connect to `CampaignCalendar`: events triggered by day
3. Integrate with `MemorialSystem`: death anniversaries tracked
4. Connect to `ShelterIdentitySystem` (Plan 166): traditions part of identity
5. Wire into `ShelterArchiveSystem` (Plan 162): memorable celebrations recorded
6. Connect to `SurvivorRelationsSystem`: activities strengthen bonds
7. Implement old-save compatibility: existing saves get default event state (no traditions, founding day set)
8. Add deterministic seeding: celebration outcomes use `ISeededRng`
9. Create exploit prevention: celebrations cost resources, can't be spammed
10. Add tests: event triggering, celebration mechanics, anniversary tracking, tradition streaks, save round-trip
11. Verify catalog integrity: all event/activity IDs resolve
12. Test edge cases: no celebrations (no morale boost), many traditions (complex calendar)
13. Verify headless behavior: events process correctly without UI
14. Add data-integrity-selftest: seasonal events validate against item catalogs (resource costs)
15. Create `--seasonal-events-selftest` verb for CI validation

## State / System Interaction Model

```text
Seasonal events & celebrations
├─ Seasonal event calendar
│  ├─ 7 fixed events (New Year, Spring, Summer, Harvest, Winter, Founding, Remembrance)
│  ├─ Events triggered by day
│  ├─ Player notified, can celebrate or ignore
│  └─ Event description and activities shown
├─ Celebration system
│  ├─ Scale: small/medium/large
│  ├─ Activities: feast, music, stories, games, memorial, dance, gifts
│  ├─ Resources consumed
│  ├─ Morale boost calculated
│  └─ Large celebrations memorable (archive)
├─ Anniversary system
│  ├─ Shelter founding: positive morale
│  ├─ Survivor death: mixed morale
│  ├─ Achievement: positive morale
│  ├─ Auto-trigger (no player choice)
│  └─ Major anniversaries (5th, 10th) larger effects
├─ Tradition system
│  ├─ Player establishes traditions
│  ├─ Traditions recur annually
│  ├─ Streak tracking (consecutive years)
│  ├─ Celebrating: morale + streak bonus
│  └─ Breaking: morale penalty, streak reset
├─ Activity mechanics
│  ├─ Feast: food → morale
│  ├─ Music: fuel → morale + bond
│  ├─ Stories: free → morale + bond
│  ├─ Games: free → morale + skill
│  ├─ Memorial: free → mixed morale
│  ├─ Dance: morale + fatigue reduction
│  └─ Gifts: items → morale + relationship
└─ Integration
   ├─ Needs (morale boost)
   ├─ Calendar (day triggers)
   ├─ Memorial (death anniversaries)
   ├─ Identity (traditions)
   ├─ Archive (memorable celebrations)
   └─ Relations (bond strengthening)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --seasonal-events-selftest
```

## Risk

**LOW** — Seasonal events are straightforward with clear inputs (day, resources, activities) and outputs (morale, traditions). Risk of events feeling like chores rather than joyful occasions. Mitigation: make events optional (player can ignore), show clear benefits (morale boost), make celebrations feel meaningful (memorable events recorded), and let traditions develop organically.

## Definition of Done

- `SeasonalEventSystem.cs` exists with full `CaptureState/RestoreState`
- 7 seasonal events implemented (New Year through Remembrance)
- Celebration system with 3 scales and 7 activity types
- Anniversary system (founding, death, achievement)
- Tradition system with streak tracking
- Activity mechanics (feast, music, stories, games, memorial, dance, gifts)
- Participation mechanics (survivor attendance, relationship effects)
- Seasonal event calendar UI
- Celebration modal with scale/activity/participant selection
- Seasonal events and quest hooks
- Save/load round-trip tested
- Deterministic celebration outcomes verified
- Old saves load without error
- 7 events + anniversary templates in data authority
- Cross-system integration (needs, calendar, memorial, identity, archive, relations)

## Follow-On Opportunities

- Seasonal event special rewards (unique items from festivals)
- Seasonal event competitions (best celebration, longest streak)
- Seasonal event legacy (traditions remembered across campaigns)
- Seasonal event quests (organize specific celebrations)
- Seasonal event trading (trade festival goods with other settlements)
