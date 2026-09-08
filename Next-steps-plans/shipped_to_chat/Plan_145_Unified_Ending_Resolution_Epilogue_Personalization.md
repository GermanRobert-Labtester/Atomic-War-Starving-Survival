# Plan 145 — Unified Ending Resolution & Epilogue Personalization

## Goal

Merge the three separate ending systems (Holdfast endings, Muster epilogues, EpilogueMatrixRuntime) into a single coherent endgame resolution that evaluates the full campaign state and generates a personalized epilogue reflecting specific player choices, faction paths, survivor fates, and moral decisions. Currently the epilogue is 12 fixed paragraphs that ignore most evaluation fields and don't reflect the player's unique journey.

## Why

**Repository evidence:** Three separate ending systems exist with no unified resolution:
1. `HoldfastEndings.cs` — 5 endings (Schedule, Reserve, DarkRoad, Tender, White) armed by game logic
2. `muster_epilogues.json` — 12 epilogue variants with rich prose, reachable through Muster questline
3. `EpilogueMatrixRuntime.cs` — 32-permutation matrix evaluating 8 context fields, but `totalDaysSurvived` and `velSecretExposed` are **never read** by branching logic

The `EpilogueEvaluationContext` has 8 fields but only 6 are used. The epilogue narrative is 12 fixed paragraphs (5 regional fate + 4 demographic + 3 moral) that don't mention which faction branch was taken, which Muster approach was selected, which survivors lived/died, or what moral choices were made. The late-game agent confirmed: "Epilogue ignores faction branches, Muster approaches, moral choices, expeditions" and "Three separate ending systems with no unified resolution."

**What is missing:** A single ending resolution that considers all campaign state and produces a personalized epilogue. Players complete a 300+ day campaign and receive a generic ending that could have been written without knowing their specific choices.

**Why existing plans don't solve it:** Plan 15 (endgame meta) mentions epilogue depth but doesn't unify the three systems. Plan 96 (epilogue chronicle) expands slides but doesn't personalize prose. Plan 89 (muster epilogues) adds variants but they're separate from Holdfast/Matrix. Plan 140 (legacy) adds cross-campaign inheritance but doesn't fix the single-campaign epilogue. No plan addresses unified ending resolution.

**Player value:** Makes endings feel earned and personal (the epilogue reflects YOUR campaign), creates replayability (different choices → different epilogues), and provides closure (the story ends with your specific journey, not a generic template).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/HoldfastEndings.cs` — 5 Holdfast endings
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` — 32-permutation matrix
- `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs` — chronicle builder
- `Assets/StreamingAssets/Data/muster_epilogues.json` — 12 Muster variants
- `Assets/StreamingAssets/Data/epilogue_chronicle.json` — 5 slides (placeholder art)
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction branch resolution
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — moral choice band
- `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs` — Verdict endings
- NEW: `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs`
- NEW: `Assets/Ashfall.Core/Endgame/PersonalizedEpilogueGenerator.cs`

## Main Task 1 — Foundation / System Contract

1. Create `UnifiedEndingResolver.cs` in `Assets/Ashfall.Core/Endgame/`
2. Create `PersonalizedEpilogueGenerator.cs` in `Assets/Ashfall.Core/Endgame/`
3. Define `UnifiedEndingContext` DTO: extends `EpilogueEvaluationContext` with:
   - `factionBranchId` (Military/Rebel/Independent/PRPF/None)
   - `musterApproachId` (Open/Amnesty/Corridor/BloodPrice/RateCard/Administrator/MeasuredTruth/Unwritten)
   - `moralChoiceBand` (VeryEvil through VeryPositive)
   - `holdfastEndingId` (Schedule/Reserve/DarkRoad/Tender/White/None)
   - `verdictEndingId` (Recount/Held/Lease/None)
   - `keySurvivorFates` (list of survivor ID + alive/deceased/retired)
   - `majorQuestCompletions` (list of quest IDs)
   - `expeditionDiscoveries` (list of discovery IDs)
   - `shelterUpgrades` (list of upgrade IDs)
   - `factionStandings` (map of faction ID → final standing)
4. Define `UnifiedEndingResult` DTO: `endingCategory` (Political/Social/Personal/Moral), `endingId`, `endingTitle`, `endingProse`, `survivorEpilogues` (list), `worldStateSummary`, `legacyTraits` (list)
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define ending resolution priority:
   - Holdfast ending (if armed) takes priority for political category
   - Muster approach determines social category
   - Moral choice band determines moral category
   - Survivor fates determine personal category
   - Verdict ending determines judicial category
7. Create ending category system:
   - **Political**: who rules, what government forms
   - **Social**: community outcome, faction relationships
   - **Personal**: survivor fates, relationships, legacy
   - **Moral**: player's moral standing, ethical legacy
   - **Judicial**: Verdict outcome, justice served
8. Implement unified resolution: combine all category endings into single coherent result
9. Create `IPersonalizedEpilogueSink` interface for UI to receive personalized epilogue
10. Add deterministic resolution: ending is pure function of campaign state (no RNG)
11. Wire into `GameBootstrap`: `SetupUnifiedEnding`, `SaveUnifiedEnding`
12. Implement ending trigger: day threshold or player action triggers ending resolution
13. Create ending UI: ending screen shows unified result with personalized prose
14. Add ending journal: automatic log of ending resolution and epilogue

## Main Task 2 — Implementation / Personalized Prose / Survivor Epilogues

1. Implement personalized political prose:
   - Military branch + Schedule ending: "The Garrison's iron discipline became the bunker's law..."
   - Rebel branch + Dark Road ending: "The resistance's fire spread from the wastes into the shelter..."
   - Independent branch + Tender ending: "The Fleet's arrival transformed the bunker from prison to port..."
   - PRPF allied + Reserve ending: "The hidden third power emerged from shadow to claim what was owed..."
   - No faction + Fractured Warlords: "Without alliance, the bunker stood alone against the wastes..."
2. Implement personalized social prose:
   - High faction standing: "The wasteland remembers your name with respect..."
   - Low faction standing: "Your reputation precedes you, but not in ways that open doors..."
   - Specific faction alliances: "The Hydro Barons remember the water you shared..."
   - Refugee admission: "The refugees you sheltered built new lives within your walls..."
   - Visitor rejections: "Those you turned away spread stories of your closed door..."
3. Implement personalized moral prose:
   - VeryPositive band: "Your compassion became legend in the wastes..."
   - VeryEvil band: "Your name became a warning whispered around fires..."
   - Specific moral choices: "When you chose to share the last ration, the shelter remembered..."
   - Mercy vs. ruthlessness: "The prisoners you released spoke of your mercy..."
4. Implement personalized survivor epilogues:
   - For each notable survivor (high skill, moral choices, leadership):
     - If alive: "Name went on to..." (based on traits, skills, relationships)
     - If deceased: "Name's memory lived on through..." (based on final wish, memorial, legacy)
     - If retired: "Name passed the torch to..." (based on mentorship, succession)
   - Survivor epilogues reference specific campaign events (quests completed, relationships formed)
5. Implement personalized expedition prose:
   - Major discoveries mentioned: "The copper vein you discovered at The Works became the shelter's lifeline..."
   - Failed expeditions remembered: "Those lost at The Denial Cut were never forgotten..."
6. Implement personalized shelter prose:
   - Upgrades mentioned: "The greenhouse you built fed generations..."
   - Defense events: "The raid on Day 187 became a story told to new arrivals..."
7. Create ending variants based on campaign length:
   - Short campaign (<180 days): "The brief experiment..."
   - Medium campaign (180-365 days): "Through the first year..."
   - Long campaign (>365 days): "Years passed, and the bunker became..."
8. Implement ending music/audio cues:
   - Different music per ending category
   - Survivor-specific themes for notable survivor epilogues
9. Create ending art:
   - Unique ending illustrations per major ending
   - Survivor portraits for notable survivor epilogues
   - Location art for key expedition discoveries
10. Add UI: ending screen with scrolling personalized prose
11. Create ending journal: full epilogue text saved to journal
12. Implement ending replay: option to replay ending cinematics
13. Add ending sharing: export ending summary for sharing
14. Create 50 personalized prose templates covering major choice combinations

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `HoldfastEndings`: Holdfast ending feeds into unified resolution
2. Connect to `EpilogueMatrixRuntime`: Matrix evaluation feeds into unified resolution
3. Integrate with `FactionBranchCoordinator`: faction branch feeds into unified resolution
4. Connect to `MoralChoiceSystem`: moral band feeds into unified resolution
5. Wire into `SurvivorFateSystem`: survivor fates feed into unified resolution
6. Connect to `VerdictEndingEvaluator`: Verdict ending feeds into unified resolution
7. Implement old-save compatibility: existing saves get default unified ending
8. Add deterministic resolution: ending is pure function of campaign state
9. Create exploit prevention: ending is one-time resolution, can't be re-rolled
10. Add tests: ending resolution, prose generation, survivor epilogues, save round-trip
11. Verify catalog integrity: all ending/survivor/quest IDs resolve
12. Test edge cases: no faction (default ending), all survivors dead (extinction ending)
13. Verify headless behavior: resolution works without UI
14. Add data-integrity-selftest: prose templates validate against catalogs
15. Create `--unified-ending-selftest` verb for CI validation

## State / System Interaction Model

```text
Campaign reaches ending trigger (day threshold or player action)
├─ UnifiedEndingResolver collects campaign state
│  ├─ Faction branch taken
│  ├─ Muster approach selected
│  ├─ Moral choice band
│  ├─ Holdfast ending armed
│  ├─ Verdict ending evaluated
│  ├─ Survivor fates (alive/deceased/retired)
│  ├─ Major quest completions
│  ├─ Expedition discoveries
│  ├─ Shelter upgrades
│  └─ Faction standings
├─ Resolver determines ending per category
│  ├─ Political: faction branch + Holdfast ending
│  ├─ Social: faction standings + visitor/refugee outcomes
│  ├─ Personal: survivor fates + relationships
│  ├─ Moral: moral choice band + key moral decisions
│  └─ Judicial: Verdict ending
├─ PersonalizedEpilogueGenerator creates prose
│  ├─ Political prose (faction-specific)
│  ├─ Social prose (standing-specific)
│  ├─ Personal prose (survivor-specific)
│  ├─ Moral prose (band-specific)
│  └─ Judicial prose (Verdict-specific)
├─ Survivor epilogues generated
│  ├─ Notable survivors get individual prose
│  ├─ References to specific campaign events
│  └─ Legacy traits identified
├─ Ending screen displays
│  ├─ Scrolling personalized prose
│  ├─ Survivor portrait cards
│  ├─ Key moment illustrations
│  └─ Legacy trait summary
└─ Epilogue saved to journal
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --unified-ending-selftest
```

## Risk

**HIGH** — Personalized epilogue complexity can spiral with too many prose variants. Risk of prose feeling repetitive if templates don't cover enough combinations. Mitigation: start with 20 core templates covering major branches, expand based on player feedback, use procedural text generation for survivor-specific details.

## Definition of Done

- `UnifiedEndingResolver.cs` exists with full `CaptureState/RestoreState`
- `PersonalizedEpilogueGenerator.cs` exists with prose generation
- Three separate ending systems merged into unified resolution
- 5 ending categories (Political, Social, Personal, Moral, Judicial)
- Personalized prose for each category based on campaign state
- Survivor epilogues for notable survivors
- 50 personalized prose templates in data authority
- Ending screen with scrolling prose and survivor cards
- Save/load round-trip tested
- Deterministic ending resolution verified
- Old saves load without error
- Cross-system integration (Holdfast, Muster, Matrix, factions, moral choice, survivors, Verdict)

## Follow-On Opportunities

- Ending achievements (unlock special rewards for specific endings)
- Ending gallery (view all unlocked endings)
- Ending music album (soundtrack per ending)
- Ending art book (illustrations per ending)
- Ending legacy (endings affect New Game+ starting conditions)
