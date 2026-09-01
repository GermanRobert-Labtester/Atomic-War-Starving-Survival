# Plan 140 — Generational Legacy & Campaign Inheritance

## Goal

Deepen the `GenerationalSuccessionEngine` (164 lines) into a full legacy system where completed campaigns leave persistent marks on future runs. Survivors retire and pass traits to successors. Shelter improvements persist across campaigns. Faction relationships carry forward as historical memory. This creates long-term investment and makes each campaign part of a larger multi-generational story.

## Why

**Repository evidence:** `Legacy/GenerationalSuccessionEngine.cs` (164 lines) has aging, retirement, and trait inheritance skeleton. `DwellerGenerationRecord` tracks generation index, age, retirement, death, mentor, inherited traits. `CohortSystem.cs` (174 lines) handles children with `isMatured` flag and `maturationDay`. `HoldfastEndings.cs` defines 5 endings. `EpilogueMatrixRuntime.cs` (150 lines) evaluates 32 permutations from 8 context fields. But no system connects completed campaigns to future runs, no New Game+ inheritance, no persistent legacy traits.

**What is missing:** Campaigns are isolated. When a campaign ends (via ending or extinction), nothing carries forward to the next run. Survivors don't leave legacy traits. Shelter improvements don't persist. Faction relationships don't become historical memory. Players have no incentive to think beyond the current campaign.

**Why existing plans don't solve it:** Plan 15 (endgame meta) mentions "New Game+ legacy inheritance" but doesn't detail the system. Plan 96 (epilogue chronicle) expands epilogue content but not cross-campaign inheritance. Plan 89 (muster epilogues) adds epilogue variants but not persistence. No plan implements actual cross-campaign legacy mechanics.

**Player value:** Creates long-term investment (your choices matter beyond one campaign), adds replayability (legacy traits unlock new options), generates emergent multi-generational stories (your grandson inherits your traits), and makes endings feel meaningful (they're not just "the end" but "the beginning of the next chapter").

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` — succession skeleton
- `Assets/Ashfall.Core/CohortSystem.cs` — children/maturation
- `Assets/Ashfall.Core/HoldfastEndings.cs` — ending definitions
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` — epilogue evaluation
- `Assets/StreamingAssets/Data/epilogue_chronicle.json` — epilogue content
- NEW: `Assets/Ashfall.Core/Legacy/CampaignLegacySystem.cs`
- NEW: `Assets/Ashfall.Core/Legacy/LegacyTraitCatalog.cs`
- NEW: `Assets/StreamingAssets/Data/legacy_traits.json`
- NEW: `Assets/StreamingAssets/Data/campaign_history.json`

## Main Task 1 — Foundation / System Contract

1. Create `CampaignLegacySystem.cs` in `Assets/Ashfall.Core/Legacy/`
2. Define `CampaignLegacy` DTO: `campaignId`, `endingId`, `daysSurvived`, `survivorCount`, `deathsRecorded`, `factionStandings` (map), `shelterImprovements` (list), `legacyTraits` (list of trait IDs), `campaignFlags` (list of flag IDs), `completionDay`
3. Define `LegacyTrait` DTO: `id`, `name`, `description`, `source` (survivor/shelter/faction/ending), `effect` (stat bonus, unlock, modifier), `generation` (0 = founder, 1+ = descendant), `inherited` bool
4. Define `CampaignLegacyState` DTO: list of completed campaigns, list of active legacy traits, list of inherited shelter improvements, campaign history log
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define legacy trait categories:
   - Survivor traits: inherited from notable survivors (leadership, skill, moral)
   - Shelter traits: inherited from shelter improvements (fortification, efficiency)
   - Faction traits: inherited from faction relationships (allies, enemies)
   - Ending traits: inherited from campaign ending (political, social, economic)
7. Create trait inheritance rules:
   - Survivor traits: 50% chance per trait to pass to successor
   - Shelter traits: 100% inherited if shelter persists
   - Faction traits: 100% inherited as historical memory
   - Ending traits: 100% inherited as campaign legacy
8. Implement New Game+ integration:
   - On new campaign, load legacy state
   - Apply inherited traits to starting survivors
   - Apply inherited shelter improvements to starting shelter
   - Apply inherited faction memory to starting standings
   - Display legacy summary in campaign intro
9. Create `LegacyTraitCatalog` for trait definitions
10. Add deterministic seeding: trait inheritance uses `ISeededRng`
11. Wire into `GameBootstrap`: `SetupCampaignLegacy`, `LoadLegacy`, `SaveLegacy`
12. Create legacy UI: campaign selection shows legacy traits from previous runs
13. Implement legacy journal: automatic log of inherited traits and their effects
14. Create legacy tutorial: first New Game+ explains inheritance mechanics

## Main Task 2 — Implementation / Legacy Traits / Campaign Memory

1. Implement survivor legacy traits:
   - Notable survivors (high skill, moral choices, leadership) leave traits
   - Traits: "Leader's Blood" (+10% morale), "Survivor's Instinct" (+10% scavenging), "Medic's Knowledge" (unlock medical recipes), "Warrior's Training" (+10% combat)
   - Traits pass to children/successors with 50% chance
   - Multiple traits can stack (grandchild inherits from both grandparents)
2. Implement shelter legacy traits:
   - Shelter improvements persist across campaigns (if ending allows)
   - Traits: "Fortified Walls" (+20% defense), "Efficient Systems" (-10% resource consumption), "Expanded Quarters" (+5 survivor capacity), "Hidden Cache" (starting items)
   - Shelter traits require "shelter persists" ending condition
3. Implement faction legacy traits:
   - Faction relationships become historical memory
   - Allied factions: starting standing +20, trade discounts
   - Hostile factions: starting standing -20, trade penalties
   - Faction traits: "Old Alliance" (faction sends aid), "Ancient Grudge" (faction raids)
4. Implement ending legacy traits:
   - Each ending leaves distinct legacy
   - "The Schedule Holds": +10% duty efficiency, -10% morale (rigid structure)
   - "The Reserve": starting resources +20%, faction trust -10 (hoarding reputation)
   - "The Road Goes Dark": expedition speed +20%, shelter defense -10 (mobile focus)
   - "Stand-Up": morale +15%, faction standing +10 (community reputation)
   - "The White": research speed +20%, survivor count -2 (secrets kept)
5. Create legacy trait interactions:
   - Some traits synergize (Leader's Blood + Warrior's Training = "Warlord's Legacy")
   - Some traits conflict (Fortified Walls + Road Goes Dark = "Contradictory Heritage")
   - Conflicting traits cancel out or produce unique hybrid effects
6. Implement legacy trait evolution:
   - Traits can evolve over multiple campaigns
   - "Leader's Blood" → "Dynasty's Call" (after 3 generations)
   - "Survivor's Instinct" → "Wasteland Legend" (after 5 campaigns)
   - Evolution unlocks unique quests and endings
7. Create legacy quest hooks:
   - "Ancestral Duty" — fulfill ancestor's unfinished quest
   - "Family Heirloom" — recover item lost in previous campaign
   - "Old Enemy" — face descendant of ancient foe
   - "Legacy Location" — visit place significant to ancestor
8. Implement legacy ending conditions:
   - Some endings only available with specific legacy traits
   - "The Dynasty" ending requires 3+ generations of Leader's Blood
   - "The Wasteland Legend" ending requires 5+ campaign completions
   - Legacy endings have unique epilogues
9. Add UI: "Legacy" panel showing inherited traits, campaign history, available legacy endings
10. Create legacy journal: automatic log of trait inheritance and evolution
11. Implement legacy save/load: legacy state persists across campaign saves
12. Create legacy reset option: players can start fresh without legacy (for challenge)
13. Add legacy achievements: milestones for multi-campaign accomplishments
14. Create 20 legacy traits in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `GenerationalSuccessionEngine`: trait inheritance integrates with succession
2. Connect to `CohortSystem`: children inherit traits from parents
3. Integrate with `HoldfastEndings`: endings leave legacy traits
4. Connect to `EpilogueMatrixRuntime`: legacy traits affect epilogue evaluation
5. Wire into `FactionBranchCoordinator`: faction memory affects starting standings
6. Connect to `ShelterThermalSystem`: shelter improvements persist
7. Implement old-save compatibility: existing saves get empty legacy state
8. Add deterministic seeding: trait inheritance uses `ISeededRng`
9. Create exploit prevention: traits are one-time inheritance, can't be farmed
10. Add tests: trait inheritance, legacy loading, save round-trip, determinism
11. Verify catalog integrity: all trait IDs resolve
12. Test edge cases: no previous campaigns (no legacy), 10+ campaigns (trait evolution)
13. Verify headless behavior: legacy loads correctly without UI
14. Add data-integrity-selftest: legacy traits validate against catalogs
15. Create `--campaign-legacy-selftest` verb for CI validation

## State / System Interaction Model

```text
Campaign ends (via ending or extinction)
├─ Legacy evaluation
│  ├─ Survivor traits extracted (notable survivors)
│  ├─ Shelter traits extracted (improvements)
│  ├─ Faction traits extracted (standings)
│  └─ Ending traits extracted (ending type)
├─ Legacy state saved
│  ├─ Campaign record added to history
│  ├─ Legacy traits stored
│  └─ Shelter state persisted (if applicable)
├─ New campaign starts
│  ├─ Legacy state loaded
│  ├─ Survivor traits applied to starting survivors
│  ├─ Shelter traits applied to starting shelter
│  ├─ Faction traits applied to starting standings
│  └─ Legacy summary displayed in intro
├─ Campaign progresses
│  ├─ Legacy traits affect gameplay
│  │  ├─ Stat bonuses applied
│  │  ├─ Unlocks available
│  │  └─ Modifiers active
│  ├─ New traits earned (notable events)
│  └─ Traits evolve (multi-campaign)
└─ Campaign ends again
   ├─ New legacy traits added
   ├─ Old traits evolved
   └─ Cycle continues
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --campaign-legacy-selftest
```

## Risk

**HIGH** — Legacy system complexity can overwhelm players if too many traits accumulate. Risk of balance issues (legacy traits make game too easy). Mitigation: cap inherited traits (max 5 per campaign), include conflicting traits that cancel out, offer legacy reset option for challenge, and ensure base game is completable without legacy.

## Definition of Done

- `CampaignLegacySystem.cs` exists with full `CaptureState/RestoreState`
- Legacy trait categories implemented (survivor, shelter, faction, ending)
- Trait inheritance mechanics functional (50% survivor, 100% others)
- New Game+ integration loads legacy state
- 20 legacy traits in data authority
- Legacy trait evolution over multiple campaigns
- Legacy quest hooks and ending conditions
- Save/load round-trip tested
- Deterministic trait inheritance verified
- Old saves load without error
- UI panel shows legacy traits and campaign history
- Cross-system integration (succession, cohort, endings, epilogue, factions, shelter)

## Follow-On Opportunities

- Legacy achievements (multi-campaign milestones)
- Legacy challenges (restricted legacy runs)
- Legacy multiplayer (share legacy with friends)
- Legacy modding (custom legacy traits)
- Legacy epilogue generator (AI-written multi-generational saga)
