# Plan 191 — Item Identification & Appraisal System

## Goal

Create an item identification and appraisal system where salvaged items return from expeditions in an unidentified state, requiring analysis by skilled survivors to reveal their properties, value, and potential uses. Currently `ProceduralScavengeSystem.cs` (213 lines) generates loot with full property disclosure — all items are immediately known. There is no identification step, no appraisal skill, no unknown item state, no analysis mechanic. This plan adds discovery depth to scavenging and creates a role for skilled appraisers.

## Why

**Repository evidence:** Grep for `UnidentifiedItem`, `ItemAnalysis`, `Appraise`, `AnalyzeItem`, `ItemAppraisal`, `IdentifyItem`, `ItemIdentification` in Core returns ZERO matches. `ProceduralScavengeSystem.cs` (213 lines) generates loot with weighted Poisson distribution, world-phase degradation, per-location visit counts, container state, contamination, and decontamination — but all items are fully identified upon acquisition. `ResearchSystem.cs` references `knowledge_scavenge_efficiency` and `SkillProgressionSystem.cs` references `skill_apex_scavenger` but these don't gate an identification mechanic.

**What is missing:** No item identification step. No appraisal skill. No unknown item state. No analysis mechanic. Items are never unidentified. There is no post-expedition sorting or discovery phase. Every item is immediately known with full properties.

**Why existing plans don't solve it:** Plan 190 (item lore) adds history to known items but doesn't add an identification step. Plan 133 (expedition consequences) adds persistent discovery but not item identification. Plan 155 (black market) adds trade but not appraisal. No plan addresses item identification as a system.

**Player value:** Creates discovery excitement (what did we find?), adds strategic depth (assign skilled appraisers), generates emergent stories (rare finds, misidentified items), and makes scavenging feel like treasure hunting rather than inventory management.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` — loot generation
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition system
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — inventory management
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- NEW: `Assets/Ashfall.Core/Inventory/ItemIdentificationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/identification_difficulty.json`

## Main Task 1 — Foundation / System Contract

1. Create `ItemIdentificationSystem.cs` in `Assets/Ashfall.Core/Inventory/`
2. Define `UnidentifiedItem` DTO: `instanceId`, `baseItemId` (hidden until identified), `discoveredDay`, `discoveredLocationId`, `discoveredBySurvivorId`, `identificationProgress` (0-100), `identificationDifficulty` (easy/medium/hard/expert), `requiredSkillLevel` (minimum skill to identify), `analysisMethod` (visual_inspection/basic_test/advanced_analysis/expert_appraisal), `isIdentified` bool
3. Define `IdentificationResult` DTO: `resultId`, `instanceId`, `identifiedDay`, `identifiedBySurvivorId`, `identificationMethod`, `accuracy` (0-100, quality of identification), `revealedProperties` (list of property names revealed), `hiddenProperties` (list still hidden if accuracy < 100)
4. Define `AppraisalSkill` DTO: `skillId`, `survivorId`, `skillLevel` (novice/apprentice/journeyman/expert/master), `specialization` (general/weapons/medical/tech/artifact), `accuracyBonus` (0-100), `speedBonus` (0-100), `identificationMethods` (list of methods unlocked)
5. Define `ItemIdentificationState` DTO: list of unidentified items, list of identification results, list of appraisal skills, identification settings (default difficulty, auto-identify threshold)
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define identification difficulty tiers:
   - **Easy**: common items (scrap, food, basic tools) — instant identification
   - **Medium**: uncommon items (weapons, medicine, electronics) — requires basic test
   - **Hard**: rare items (advanced tech, artifacts, encrypted data) — requires advanced analysis
   - **Expert**: legendary items (pre-war military, experimental tech, alien artifacts) — requires expert appraisal
8. Define analysis methods:
   - **Visual Inspection**: free, instant, reveals basic properties (name, category, condition), low accuracy (50-70%)
   - **Basic Test**: requires tools (magnifying glass, multimeter, test kit), 1 hour, reveals most properties, medium accuracy (70-90%)
   - **Advanced Analysis**: requires lab equipment (spectrometer, diagnostic terminal), 1 day, reveals all properties, high accuracy (90-100%)
   - **Expert Appraisal**: requires expert survivor + specialized tools, 1-3 days, reveals all properties + hidden lore, 100% accuracy
9. Define identification consequences:
   - Unidentified items cannot be used (except as trade goods)
   - Unidentified items show as "Unknown [category]" in inventory
   - Misidentified items (low accuracy) may have incorrect properties
   - Fully identified items unlock full use + lore (Plan 190 integration)
   - Some items have hidden properties only revealed at 100% accuracy
10. Define appraisal skill progression:
    - Novice: visual inspection only, 50% accuracy
    - Apprentice: basic test unlocked, 70% accuracy
    - Journeyman: advanced analysis unlocked, 85% accuracy
    - Expert: expert appraisal unlocked, 95% accuracy
    - Master: instant identification for easy/medium items, 100% accuracy
11. Define specialization bonuses:
    - **Weapons**: +20% accuracy for weapons, faster identification
    - **Medical**: +20% accuracy for medicine/biological, reveals hidden medical properties
    - **Tech**: +20% accuracy for electronics/tech, reveals hidden functions
    - **Artifact**: +20% accuracy for pre-war/artifact items, unlocks lore
    - **General**: no specialization bonus, balanced across all types
12. Add deterministic seeding: identification uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupItemIdentification`, `TickItemIdentification`, `SaveItemIdentification`
14. Create identification UI: unidentified item indicator, analysis panel, appraisal skill viewer
15. Integrate with `ProceduralScavengeSystem`: loot generation marks items as unidentified based on difficulty

## Main Task 2 — Implementation / Identification / Analysis / Appraisal / Discovery

1. Implement unidentified item generation:
   - When loot is generated, items marked unidentified based on difficulty
   - Easy items: auto-identified (common scrap, food)
   - Medium items: 80% start unidentified
   - Hard items: 100% start unidentified
   - Expert items: 100% start unidentified + hidden properties
   - Unidentified items show as "Unknown [category]" in inventory
2. Implement visual inspection:
   - Any survivor can visually inspect an item
   - Inspection is instant (no time cost)
   - Reveals basic properties (name guess, category, condition)
   - Accuracy 50-70% (some properties may be wrong/missing)
   - Inspection logged
3. Implement basic test:
   - Requires basic tools (magnifying glass, multimeter, test kit)
   - Takes 1 hour
   - Survivor must have Apprentice+ appraisal skill
   - Reveals most properties
   - Accuracy 70-90%
   - Test logged
4. Implement advanced analysis:
   - Requires lab equipment (spectrometer, diagnostic terminal)
   - Takes 1 day
   - Survivor must have Journeyman+ appraisal skill
   - Reveals all properties
   - Accuracy 90-100%
   - Analysis logged
5. Implement expert appraisal:
   - Requires Expert+ survivor + specialized tools
   - Takes 1-3 days
   - Reveals all properties + hidden lore
   - 100% accuracy
   - Appraisal logged
   - May unlock special quests/lore
6. Implement identification progress:
   - Each analysis method increases identification progress
   - Visual inspection: +20% progress
   - Basic test: +40% progress
   - Advanced analysis: +30% progress
   - Expert appraisal: +10% progress (but guarantees 100% accuracy)
   - At 100% progress: item fully identified
7. Implement accuracy calculation:
   - Base accuracy from analysis method
   - +Survivor skill level bonus
   - +Specialization bonus (if applicable)
   - +Tool quality bonus
   - -Item difficulty penalty
   - Final accuracy determines property reveal quality
8. Implement property reveal:
   - Accuracy 50-69%: basic properties only (name, category, condition)
   - Accuracy 70-89%: most properties (name, category, condition, value, use)
   - Accuracy 90-99%: all properties + some hidden
   - Accuracy 100%: all properties + all hidden + lore
9. Implement misidentification:
   - Low accuracy may reveal incorrect properties
   - Misidentified items have wrong value/use
   - Misidentification discovered when item used (wrong effect)
   - Misidentification can be corrected with better analysis
10. Implement appraisal skill progression:
    - Each identification grants XP to appraiser
    - XP scaled by item difficulty
    - Skill levels unlock better analysis methods
    - Specializations unlocked at Journeyman+
    - Master level grants instant identification for easy/medium
11. Create identification events:
    - "The Discovery" — unidentified item found
    - "The Inspection" — visual inspection completed
    - "The Analysis" — advanced analysis completed
    - "The Appraisal" — expert appraisal completed
    - "The Revelation" — item fully identified
    - "The Misidentification" — item incorrectly identified
    - "The Expert" — survivor reaches Master appraisal skill
    - "The Treasure" — legendary item identified
12. Add identification quest hooks:
    - "The Appraiser" — identify 20 items
    - "The Expert" — reach Master appraisal skill
    - "The Treasure" — identify 5 legendary items
    - "The Collector" — identify items from all categories
    - "The Specialist" — reach specialization in 3 types
    - "The Discovery" — find and identify rare artifact
    - "The Mentor" — train 3 apprentices to Journeyman
13. Implement identification UI:
    - Inventory: unidentified items marked with "?" icon
    - Item detail: identification progress bar, analysis options
    - Analysis panel: select method, assign survivor, start analysis
    - Appraisal skill panel: survivor skills, specializations, progression
    - Discovery log: history of identifications
14. Add identification journal: automatic log of significant identifications
15. Implement identification tutorial: first unidentified item explains system

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ProceduralScavengeSystem`: loot generation marks items unidentified
2. Connect to `ExpeditionSystem`: expedition loot includes unidentified items
3. Integrate with `InventorySystem`: unidentified items tracked separately
4. Connect to `SkillProgressionSystem`: appraisal skill progression
5. Wire into `CraftingSystem`: identified items required for recipes
6. Connect to `MarketSystem`: unidentified items have reduced trade value
7. Implement old-save compatibility: existing saves get all items identified
8. Add deterministic seeding: identification uses `ISeededRng`
9. Create exploit prevention: identification is time/skill-based, can't be gamed
10. Add tests: identification flow, accuracy calculation, skill progression, misidentification, save round-trip
11. Verify all difficulty tiers work correctly
12. Test edge cases: no identification (all unknown), instant identification (Master skill)
13. Verify headless behavior: identification processes correctly without UI
14. Add data-integrity-selftest: identification validates against item/skill catalogs
15. Create `--item-identification-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --item-identification-selftest
```

## Risk

**LOW** — Item identification is straightforward with clear inputs (analysis methods, skill) and outputs (identification progress, accuracy). Risk of identification feeling like a chore rather than discovery. Mitigation: make visual inspection instant, show clear progress, allow auto-identification for common items, and ensure rare finds feel special.

## Definition of Done

- `ItemIdentificationSystem.cs` exists with full `CaptureState/RestoreState`
- 4 identification difficulty tiers (easy/medium/hard/expert)
- 4 analysis methods (visual inspection, basic test, advanced analysis, expert appraisal)
- Appraisal skill progression (novice → apprentice → journeyman → expert → master)
- 5 specialization types (general, weapons, medical, tech, artifact)
- Identification progress tracking (0-100%)
- Accuracy calculation with skill/tool/difficulty modifiers
- Property reveal based on accuracy
- Misidentification mechanic
- Identification events and quest hooks
- Save/load round-trip tested
- Deterministic identification verified
- Old saves load with all items identified
- Identification difficulty in data authority
- UI identification panel, analysis panel, appraisal skill viewer
- Cross-system integration (scavenging, expedition, inventory, skills, crafting, market)

## Follow-On Opportunities

- Identification specialization (unique appraisal techniques)
- Identification legacy (famous appraisals remembered)
- Identification quests (specific identification goals)
- Identification events (cursed items, trapped items)
- Identification trading (sell appraisal services)
