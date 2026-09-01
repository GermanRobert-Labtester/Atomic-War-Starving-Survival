# Plan 141 — Research → Downstream Unlocks Bridge

## Goal

Fix the dead end where `ResearchSystem` breakthrough items are never granted and research completions have no downstream effect. Connect research to crafting unlocks, expedition capabilities, shelter upgrades, combat doctrines, and medical procedures. Research becomes a meaningful progression system that opens new gameplay options rather than a terminal knowledge ledger.

## Why

**Repository evidence:** `ResearchSystem.cs` (lines 190-205) completes research nodes and logs `def.breakthroughItem` but **never adds the item to inventory, never unlocks recipes, never raises events**. The six defined breakthrough items (`item_water_filter_advanced`, `item_radiation_shielding_panel`, `item_gas_mask_improved`, `item_solar_inverter`, `item_radio_cipher_rotor`, `item_air_filter_hepa`) are strings with no downstream consumer. `WorkshopReverseEngineeringSystem` also completes research but just logs breakthroughs without granting anything. The gameplay gaps agent confirmed: "Research breakthrough items never granted."

**What is missing:** Research is a terminal activity — you complete a node, it's recorded, nothing changes. Players have no incentive to research because it doesn't unlock new capabilities. The research tree exists but is disconnected from crafting, expeditions, shelter, combat, and medical systems.

**Why existing plans don't solve it:** Plan 26 (knowledge/research/skills) and Plan 33 (skill catalog externalization) externalize research data but don't connect it to downstream systems. Plan 34 (research tree externalization) moves research to JSON but doesn't fix the unlock gap. No plan addresses research→crafting, research→expedition, research→shelter, research→combat, or research→medical bridges.

**Player value:** Makes research meaningful (completing nodes unlocks real capabilities), creates progression incentives (research to access new content), and connects knowledge to gameplay across multiple systems.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Research/ResearchSystem.cs` — research completion (dead end)
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` — recipe unlocks
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition capabilities
- `Assets/Ashfall.Core/Shelter/` — shelter upgrades
- `Assets/Ashfall.Core/Combat/` — combat doctrines
- `Assets/Ashfall.Core/Medical/` — medical procedures
- `Assets/StreamingAssets/Data/research_tree.json` — research definitions
- NEW: `Assets/Ashfall.Core/Research/ResearchUnlockBridge.cs`

## Main Task 1 — Foundation / System Contract

1. Create `ResearchUnlockBridge.cs` in `Assets/Ashfall.Core/Research/`
2. Define `ResearchUnlock` DTO: `researchNodeId`, `unlockType` (item/recipe/expedition/shelter/combat/medical), `unlockTargetId`, `unlockDescription`
3. Define `ResearchUnlockState` DTO: list of granted unlocks, list of pending unlocks
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define unlock mapping: each research node maps to specific unlocks
   - `research_water_purification_advanced` → unlocks `recipe_water_filter_advanced`
   - `research_radiation_shielding` → unlocks `item_radiation_shielding_panel` + shelter upgrade
   - `research_gas_mask_improvement` → unlocks `recipe_gas_mask_improved`
   - `research_solar_power` → unlocks `item_solar_inverter` + shelter power upgrade
   - `research_crypto_analysis` → unlocks `item_radio_cipher_rotor` + expedition capability
   - `research_air_filtration` → unlocks `recipe_air_filter_hepa` + shelter upgrade
6. Create `IResearchUnlockSink` interface for downstream systems to receive unlocks
7. Implement unlock granting: when research completes, bridge grants items/recipes/upgrades
8. Add deterministic unlocking: unlocks are pure functions of research completion (no RNG)
9. Wire into `GameBootstrap`: `SetupResearchUnlockBridge`, `SaveResearchUnlocks`
10. Create `ResearchUnlockCatalogLoader` for unlock mappings
11. Implement unlock logging: all granted unlocks recorded for UI/epilogue
12. Add UI hook: research panel shows unlocked capabilities
13. Create research journal: automatic log of research breakthroughs and their effects
14. Implement research tutorial: first research completion explains unlock system

## Main Task 2 — Implementation / Crafting / Expedition / Shelter / Combat / Medical Integration

1. Implement crafting integration:
   - Research completion unlocks recipes in `CraftingSystem`
   - Unlocked recipes appear in crafting UI
   - Recipes require researched items as ingredients
   - Example: `research_water_purification_advanced` unlocks `recipe_water_filter_advanced`
2. Implement expedition integration:
   - Research completion unlocks expedition capabilities
   - New expedition destinations become available
   - Expedition options expand (e.g., crypto analysis unlocks encrypted cache raids)
   - Example: `research_crypto_analysis` unlocks encrypted expedition encounters
3. Implement shelter integration:
   - Research completion unlocks shelter upgrades
   - New rooms/systems become available
   - Existing systems improve (efficiency, capacity)
   - Example: `research_radiation_shielding` unlocks radiation shielding room
4. Implement combat integration:
   - Research completion unlocks combat doctrines
   - New tactics/stances become available
   - Equipment upgrades unlocked
   - Example: `research_tactical_analysis` unlocks advanced combat tactics
5. Implement medical integration:
   - Research completion unlocks medical procedures
   - New treatments become available
   - Existing treatments improve (effectiveness, side effects)
   - Example: `research_radiation_therapy` unlocks advanced rad treatment
6. Create research tier system:
   - Tier 1 research: basic unlocks (simple recipes, minor upgrades)
   - Tier 2 research: intermediate unlocks (advanced recipes, major upgrades)
   - Tier 3 research: expert unlocks (unique items, game-changing capabilities)
7. Implement research prerequisites:
   - Some unlocks require multiple research nodes
   - Some unlocks require specific research paths
   - Research tree has branching with exclusive unlocks
8. Create research synergy bonuses:
   - Completing related research nodes grants bonus unlocks
   - Example: completing all medical research unlocks "Chief Medical Officer" trait
9. Add research failure mechanics:
   - Some research has chance of failure (wasted resources)
   - Failed research can be retried
   - Critical failures produce negative unlocks (hazards, afflictions)
10. Implement research trading:
    - Factions can share research (diplomatic unlock)
    - Captured enemy research can be reverse-engineered
    - Research data can be stolen (espionage unlock)
11. Add UI: research panel shows unlock tree with granted/pending/locked states
12. Create research journal: automatic log of all unlocks and their effects
13. Implement research interaction with other systems:
    - `CraftingSystem`: recipes unlocked
    - `ExpeditionSystem`: destinations/options unlocked
    - `ShelterThermalSystem`: upgrades applied
    - `TacticalCombatSystem`: doctrines unlocked
    - `MedicalPipelineCoordinator`: procedures unlocked
14. Create 30 research unlock mappings in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ResearchSystem`: research completion triggers unlock granting
2. Connect to `CraftingSystem`: recipes unlocked
3. Integrate with `ExpeditionSystem`: destinations/options unlocked
4. Connect to shelter systems: upgrades applied
5. Wire into `TacticalCombatSystem`: doctrines unlocked
6. Connect to `MedicalPipelineCoordinator`: procedures unlocked
7. Implement old-save compatibility: existing saves get empty unlock state, past research retroactively grants unlocks
8. Add deterministic unlocking: unlocks are pure functions of research completion
9. Create exploit prevention: unlocks are one-time, can't be re-granted
10. Add tests: unlock granting, crafting integration, expedition integration, shelter integration, save round-trip
11. Verify catalog integrity: all unlock target IDs resolve
12. Test edge cases: no research (no unlocks), all research (all unlocks)
13. Verify headless behavior: unlocks grant correctly without UI
14. Add data-integrity-selftest: unlock mappings validate against research/crafting/expedition catalogs
15. Create `--research-unlocks-selftest` verb for CI validation

## State / System Interaction Model

```text
Research node completed
├─ Bridge reads research definition
├─ Bridge determines unlocks (item/recipe/expedition/shelter/combat/medical)
├─ Bridge grants unlocks
│  ├─ Item: added to inventory (if physical) or unlocked (if capability)
│  ├─ Recipe: added to crafting system
│  ├─ Expedition: destination/option unlocked
│  ├─ Shelter: upgrade applied
│  ├─ Combat: doctrine unlocked
│  └─ Medical: procedure unlocked
├─ Bridge logs unlock
├─ UI updated (research panel, crafting panel, etc.)
└─ Downstream systems notified
   ├─ Crafting: new recipes available
   ├─ Expedition: new destinations/options
   ├─ Shelter: upgrades functional
   ├─ Combat: new tactics available
   └─ Medical: new treatments available
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --research-unlocks-selftest
```

## Risk

**MEDIUM** — Research unlock complexity can overwhelm players if too many unlocks trigger simultaneously. Risk of balance issues (research makes game too easy). Mitigation: gate unlocks behind research tiers, require multiple nodes for powerful unlocks, include research failure mechanics.

## Definition of Done

- `ResearchUnlockBridge.cs` exists with full `CaptureState/RestoreState`
- Research completion triggers unlock granting
- Crafting system receives unlocked recipes
- Expedition system receives unlocked destinations/options
- Shelter systems receive upgrades
- Combat system receives unlocked doctrines
- Medical system receives unlocked procedures
- 30 research unlock mappings in data authority
- Save/load round-trip tested
- Deterministic unlocking verified
- Old saves load without error (retroactive unlock granting)
- UI panel shows unlock tree
- Cross-system integration (research, crafting, expedition, shelter, combat, medical)

## Follow-On Opportunities

- Research specialization (survivors focus on specific research branches)
- Research competition (factions race to discover first)
- Research sabotage (rival factions destroy research)
- Research legacy (completed research carries to New Game+)
- Research collaboration (multiple survivors accelerate research)
