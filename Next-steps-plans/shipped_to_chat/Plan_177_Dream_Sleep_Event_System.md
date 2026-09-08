# Plan 177 — Dream & Sleep Event System

## Goal

Create a dream and sleep event system where survivors experience dreams, nightmares, and sleep events that reveal their psychological state, process trauma, and sometimes provide insights or warnings. Currently `GuiltInsomniaSystem.cs` handles guilt-driven sleep disruption and `MoralChoiceIds.cs` references a `ComfortNightmare` quest, but there is no dream system — no dreams, no nightmares, no sleep events, no subconscious processing. Survivors sleep but nothing happens in their minds. This plan adds psychological depth through the sleeping mind.

## Why

**Repository evidence:** Grep for `DreamSystem`, `SleepEvent`, `Nightmare`, `DreamSequence` in Core returns only 2 matches: `MoralChoiceIds.cs:60` — `ComfortNightmare` quest ID, and `MoralChoiceIds.cs:182` — comfort category list. No dream system exists. `GuiltInsomniaSystem.cs` (286 lines) handles sleep disruption but not dream content. Survivors sleep but their minds are empty — no dreams, no nightmares, no subconscious processing of trauma.

**What is missing:** No dream system. No nightmares. No sleep events. No subconscious trauma processing. No dream-based insights or warnings. No dream interpretation. The sleeping mind is a blank void.

**Why existing plans don't solve it:** Plan 147 (per-NPC memory) adds memory but not dream processing. Plan 148 (friction→events) adds friction events but not sleep events. No plan addresses dreams, nightmares, or sleep-based psychological processing.

**Player value:** Creates psychological depth (survivors have inner lives), adds narrative variety (dreams tell stories), provides trauma processing (nightmares process trauma), generates emergent content (unique dreams per survivor), and makes sleep more than a stat reset.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` — sleep disruption
- `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` — combat trauma
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs` — flashbacks
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` — mental health
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` — nightmare quest reference
- NEW: `Assets/Ashfall.Core/Survivors/DreamSystem.cs`
- NEW: `Assets/StreamingAssets/Data/dream_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `DreamSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `DreamTemplate` DTO: `templateId`, `dreamType` (peaceful/nightmare/prophetic/memory/surreal), `triggerConditions` (list: trauma level, morale, recent events), `content` (narrative text), `effects` (list: morale modifier, insight gained, trauma processed), `rarity` (common/uncommon/rare)
3. Define `Dream` DTO: `dreamId`, `survivorId`, `templateId`, `dreamDay`, `dreamType`, `content` (resolved text), `effects` (applied), `remembered` bool, `interpreted` bool
4. Define `SleepEvent` DTO: `eventId`, `survivorId`, `eventType` (restful_sleep/insomnia/nightmare/dream/sleep_talking/sleepwalking), `duration` (hours), `quality` (0-100), `effects` (list)
5. Define `DreamState` DTO: list of dreams experienced, list of sleep events, dream frequency, last dream day, trauma processed through dreams
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define dream types:
   - **Peaceful**: pleasant dreams, morale boost, restful sleep
   - **Nightmare**: trauma processing, morale penalty, trauma reduction
   - **Prophetic**: rare insight dreams, warning of future events
   - **Memory**: replay of past events, processing grief/joy
   - **Surreal**: bizarre dreams, creativity boost, confusion
8. Define dream triggers:
   - High trauma → nightmare chance increases
   - Low morale → nightmare chance increases
   - Recent combat → combat nightmares
   - Recent death → grief dreams
   - High morale → peaceful dreams
   - Random events → surreal dreams
9. Define dream effects:
   - Peaceful: +morale, restful sleep bonus
   - Nightmare: -morale, trauma reduction (processing)
   - Prophetic: insight gained (warning or hint)
   - Memory: emotional processing, possible morale change
   - Surreal: +creativity, -clarity (confusion)
10. Define sleep quality:
    - Sleep quality affects rest bonus
    - Good sleep: full rest bonus
    - Poor sleep (insomnia): reduced rest bonus
    - Nightmare: rest penalty
    - Sleep quality affected by: shelter conditions, stress, trauma
11. Define dream interpretation:
    - Player can choose to interpret dream
    - Interpretation reveals meaning (trauma processing, insight)
    - Interpretation provides morale bonus (understanding)
    - Some dreams cannot be interpreted (too surreal)
12. Add deterministic seeding: dream selection uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupDreams`, `TickDreams`, `SaveDreams`
14. Create `DreamTemplateCatalogLoader` for dream definitions
15. Implement dream UI: dream journal showing experienced dreams

## Main Task 2 — Implementation / Dreams / Nightmares / Sleep / Interpretation

1. Implement dream generation:
   - Each sleep cycle, check for dream trigger conditions
   - Matching conditions → select dream template
   - Dream template resolved with survivor-specific content
   - Dream recorded in dream state
   - Dream effects applied
2. Implement nightmare system:
   - High trauma survivors experience nightmares
   - Nightmares process trauma (reduce trauma level)
   - Nightmares cause morale penalty
   - Nightmares logged in dream journal
   - Frequent nightmares indicate PTSD (Plan 179 integration)
3. Implement peaceful dreams:
   - High morale survivors experience peaceful dreams
   - Peaceful dreams boost morale
   - Peaceful dreams improve sleep quality
   - Peaceful dreams logged
4. Implement prophetic dreams:
   - Rare dreams provide insights
   - Insights warn of future events (raids, storms)
   - Insights provide hints (resource locations)
   - Prophetic dreams logged and interpreted
5. Implement memory dreams:
   - Dreams replay past events
   - Memory dreams process grief and joy
   - Memory dreams affect morale (positive or negative)
   - Memory dreams help process loss
6. Implement surreal dreams:
   - Random bizarre dreams
   - Surreal dreams boost creativity
   - Surreal dreams cause confusion (temporary)
   - Surreal dreams logged
7. Implement sleep quality:
   - Sleep quality calculated from conditions
   - Shelter conditions affect quality (warmth, safety)
   - Stress affects quality (trauma, morale)
   - Quality affects rest bonus
   - Poor quality: insomnia events
8. Implement dream interpretation:
   - Player reviews dream in journal
   - Player can choose to interpret
   - Interpretation reveals meaning
   - Interpretation provides morale bonus
   - Some dreams uninterpretable
9. Create dream events:
   - "The Dream" — survivor experiences dream
   - "The Nightmare" — survivor has nightmare
   - "The Prophecy" — prophetic dream received
   - "The Memory" — memory dream experienced
   - "The Surreal" — bizarre dream
   - "The Interpretation" — dream interpreted
   - "The Rest" — restful sleep achieved
10. Add dream quest hooks:
    - "The Dreamer" — experience 10 dreams
    - "The Nightmare" — survive nightmare trauma processing
    - "The Prophet" — receive prophetic dream
    - "The Interpreter" — interpret 5 dreams
    - "The Rest" — achieve perfect sleep quality
    - "The Processing" — process trauma through nightmares
    - "The Insight" — gain insight from dream
11. Implement dream UI:
    - Dream journal: list of experienced dreams
    - Dream detail: content, type, effects
    - Interpretation button: interpret dream
    - Sleep quality display: current sleep quality
    - Dream notification: new dream experienced
12. Add dream journal: automatic log of dream events
13. Implement dream tutorial: first dream explains system
14. Add dream tooltips: hover over dream shows type and effects
15. Create 30 dream templates in data file (6 per type)

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `GuiltInsomniaSystem`: sleep quality affects insomnia
2. Connect to `CombatTraumaSystem`: trauma triggers nightmares
3. Integrate with `MentalHealthCrisisSystem`: dreams affect mental health
4. Connect to `SomaticFlashbackSystem`: trauma processing through dreams
5. Wire into `NeedsSystem`: sleep quality affects fatigue
6. Connect to `MoralChoiceSystem`: nightmare quests integrate
7. Implement old-save compatibility: existing saves get empty dream state
8. Add deterministic seeding: dream selection uses `ISeededRng`
9. Create exploit prevention: dreams are automatic, can't be farmed
10. Add tests: dream generation, nightmare processing, sleep quality, interpretation, save round-trip
11. Verify catalog integrity: all dream template IDs resolve
12. Test edge cases: no dreams (no sleep), many dreams (active dreamer)
13. Verify headless behavior: dreams process correctly without UI
14. Add data-integrity-selftest: dream templates validate against trauma/morale catalogs
15. Create `--dream-selftest` verb for CI validation

## State / System Interaction Model

```text
Dream & sleep event system
├─ Dream generation
│  ├─ 5 types: peaceful/nightmare/prophetic/memory/surreal
│  ├─ Trigger conditions: trauma, morale, events
│  ├─ Template resolved with survivor content
│  └─ Effects applied
├─ Nightmare system
│  ├─ High trauma → nightmares
│  ├─ Nightmares process trauma
│  ├─ Nightmares cause morale penalty
│  └─ Frequent = PTSD indicator
├─ Peaceful dreams
│  ├─ High morale → peaceful
│  ├─ Boost morale
│  └─ Improve sleep quality
├─ Prophetic dreams
│  ├─ Rare insights
│  ├─ Warn of future events
│  └─ Provide hints
├─ Memory dreams
│  ├─ Replay past events
│  ├─ Process grief/joy
│  └─ Help process loss
├─ Sleep quality
│  ├─ Conditions affect quality
│  ├─ Quality affects rest bonus
│  └─ Poor quality: insomnia
└─ Integration
   ├─ Insomnia (sleep quality)
   ├─ Combat trauma (nightmare triggers)
   ├─ Mental health (dream effects)
   ├─ Flashbacks (trauma processing)
   ├─ Needs (fatigue)
   └─ Moral choice (nightmare quests)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --dream-selftest
```

## Risk

**LOW** — Dream system is additive with clear inputs (trauma, morale) and outputs (dreams, sleep quality). Risk of dreams feeling random rather than meaningful. Mitigation: tie dreams to survivor state, make interpretation meaningful, and ensure dreams provide value (trauma processing, insights).

## Definition of Done

- `DreamSystem.cs` exists with full `CaptureState/RestoreState`
- 5 dream types implemented (peaceful, nightmare, prophetic, memory, surreal)
- Dream generation from trigger conditions
- Nightmare trauma processing
- Sleep quality system
- Dream interpretation mechanic
- Dream events and quest hooks
- Save/load round-trip tested
- Deterministic dream selection verified
- Old saves load without error
- 30 dream templates in data authority
- UI dream journal panel
- Cross-system integration (insomnia, trauma, mental health, flashbacks, needs, moral choice)

## Follow-On Opportunities

- Dream sharing (survivors share dreams with each other)
- Dream therapy (guided dream interpretation)
- Dream legacy (famous dreams remembered)
- Dream quests (specific dream experiences)
- Dream artifacts (objects that influence dreams)
