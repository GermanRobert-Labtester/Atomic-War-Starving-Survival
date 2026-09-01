# Plan 181 — Difficulty Settings System

## Goal

Create a difficulty settings system where players can choose from preset difficulties (Easy/Normal/Hard/Nightmare) or customize individual gameplay sliders (radiation rate, raid frequency, resource scarcity, survivor death penalty, etc.). Currently the game has a single fixed difficulty — no presets, no sliders, no customization. Plan 175 (meta-progression) adds difficulty *modifiers* for New Game+ challenges, but there is no general difficulty settings system for the base game. This plan makes ASHFALL accessible to players of all skill levels.

## Why

**Repository evidence:** Grep for `DifficultySetting`, `DifficultyLevel`, `GameDifficulty`, `DifficultyModifier` in Core returns ZERO matches. No difficulty system exists. The game runs at a single fixed difficulty level. Plan 175 adds NG+ difficulty modifiers (ironman, scarce, hostile) but those are meta-progression challenges, not base-game difficulty settings.

**What is missing:** No difficulty presets. No difficulty sliders. No way to adjust game challenge. One size fits all. Players who find the game too hard have no options. Players who want more challenge beyond NG+ modifiers have no granular control.

**Why existing plans don't solve it:** Plan 175 (meta-progression) adds NG+ difficulty modifiers but those are persistent challenge bonuses for meta currency, not a general difficulty system. Plan 34 (long arc) mentions "accessibility parity: difficulty presets must not be the only way to soften the game" but doesn't implement difficulty settings. No plan addresses a standalone difficulty settings system.

**Player value:** Makes game accessible to all skill levels, allows personalized challenge, reduces frustration for casual players, increases challenge for hardcore players, and provides granular control over gameplay experience.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — day tracking
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — needs decay rates
- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation rates
- `Assets/Ashfall.Core/Economy/MarketSystem.cs` — economy
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition difficulty
- NEW: `Assets/Ashfall.Core/Difficulty/DifficultySettingsSystem.cs`
- NEW: `Assets/StreamingAssets/Data/difficulty_presets.json`

## Main Task 1 — Foundation / System Contract

1. Create `DifficultySettingsSystem.cs` in `Assets/Ashfall.Core/Difficulty/`
2. Define `DifficultyPreset` DTO: `presetId`, `presetName` (easy/normal/hard/nightmare/custom), `modifiers` (dict of setting → value), `description`
3. Define `DifficultySettings` DTO: `activePreset`, `radiationRate` (0.5-2.0 multiplier), `raidFrequency` (0.5-2.0), `resourceScarcity` (0.5-2.0), `survivorDeathPenalty` (0.5-2.0), `needDecayRate` (0.5-2.0), `expeditionDanger` (0.5-2.0), `economicPressure` (0.5-2.0), `weatherSeverity` (0.5-2.0), `customMode` bool
4. Define `DifficultyState` DTO: active settings, preset chosen, custom slider values, settings locked bool
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define 4 presets:
   - **Easy**: radiation 0.5x, raids 0.5x, resources 1.5x, needs 0.7x, expedition danger 0.5x
   - **Normal**: all 1.0x (baseline)
   - **Hard**: radiation 1.5x, raids 1.5x, resources 0.7x, needs 1.3x, expedition danger 1.5x
   - **Nightmare**: radiation 2.0x, raids 2.0x, resources 0.5x, needs 1.5x, expedition danger 2.0x
7. Define 9 customizable sliders (custom mode):
   - Radiation rate, raid frequency, resource scarcity, survivor death penalty, need decay rate, expedition danger, economic pressure, weather severity, crafting difficulty
   - Each slider: 0.5x to 2.0x multiplier
   - Custom mode unlocks when player selects "Custom" preset
8. Define difficulty effects on systems:
   - `RadiationSystem`: radiation rate modifier
   - `NeedsSystem`: need decay rate modifier
   - `MarketSystem`: economic pressure modifier (prices)
   - `ExpeditionSystem`: expedition danger modifier
   - `WeatherSystem`: weather severity modifier
   - `CraftingSystem`: crafting difficulty modifier
   - Raid frequency: affects raid spawning
   - Resource scarcity: affects starting resources and loot
   - Death penalty: affects consequences of survivor death
9. Define settings lock:
   - Settings can be locked at campaign start (ironman-style)
   - Locked settings cannot be changed mid-campaign
   - Unlocked settings can be adjusted anytime
   - Lock is optional (player choice)
10. Add deterministic seeding: difficulty modifiers are deterministic multipliers
11. Wire into `GameBootstrap`: `SetupDifficulty`, apply modifiers to all systems
12. Create `DifficultyPresetCatalogLoader` for preset definitions
13. Implement difficulty UI: settings panel with presets and sliders
14. Create difficulty selection screen at campaign start
15. Add difficulty indicator to HUD (current difficulty name)

## Main Task 2 — Implementation / Presets / Sliders / Effects / UI

1. Implement preset selection:
   - Campaign start: player selects difficulty preset
   - Preset applies all modifiers automatically
   - Preset description shown
   - Preset can be changed if not locked
2. Implement custom mode:
   - Custom preset unlocks individual sliders
   - Each slider adjustable 0.5x-2.0x
   - Custom configuration saved
   - Custom config can be named and shared
3. Implement difficulty effects:
   - Radiation rate: multiplies radiation accumulation
   - Need decay: multiplies hunger/thirst/fatigue decay
   - Raid frequency: multiplies raid spawn chance
   - Resource scarcity: divides starting resources and loot
   - Death penalty: multiplies morale loss from death
   - Expedition danger: multiplies expedition risk
   - Economic pressure: multiplies price volatility
   - Weather severity: multiplies weather damage
   - Crafting difficulty: multiplies craft time and cost
4. Implement settings lock:
   - Player can lock settings at campaign start
   - Locked settings persist for entire campaign
   - Lock prevents mid-campaign adjustment
   - Lock is optional (player choice)
5. Implement difficulty display:
   - Current difficulty shown in HUD
   - Settings panel shows all modifiers
   - Slider tooltips show effect descriptions
   - Preset comparison view
6. Implement difficulty recommendations:
   - First-time players: Normal recommended
   - Experienced players: Hard recommended
   - Mass-effect challenge: Nightmare
   - Custom: for specific preferences
7. Create difficulty events:
   - "The Choice" — difficulty selected at campaign start
   - "The Adjustment" — difficulty changed mid-campaign
   - "The Lock" — settings locked
   - "The Challenge" — surviving on Hard/Nightmare
   - "The Mercy" — difficulty reduced after struggle
8. Add difficulty quest hooks:
   - "The Survivor" — complete campaign on Hard
   - "The Masochist" — complete campaign on Nightmare
   - "The Custom" — complete campaign with custom settings
   - "The Lock" — complete campaign with locked settings
   - "The Purist" — complete campaign on Normal (no modifiers)
9. Implement difficulty UI:
   - Settings panel: preset selection, slider adjustment
   - Campaign start: difficulty selection screen
   - HUD: current difficulty indicator
   - Preset comparison: side-by-side preset details
   - Custom config: name, save, load configurations
10. Add difficulty journal: automatic log of difficulty changes
11. Implement difficulty tutorial: difficulty selection explains system
12. Add difficulty tooltips: hover over slider shows effect
13. Create 4 preset definitions in data file
14. Implement difficulty integration with Plan 175 (meta-progression): NG+ modifiers stack with difficulty settings

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `RadiationSystem`: radiation rate modifier
2. Connect to `NeedsSystem`: need decay modifier
3. Integrate with `MarketSystem`: economic pressure modifier
4. Connect to `ExpeditionSystem`: expedition danger modifier
5. Wire into `WeatherSystem`: weather severity modifier
6. Connect to `CraftingSystem`: crafting difficulty modifier
7. Implement old-save compatibility: existing saves get Normal difficulty
8. Add deterministic modifiers: all multipliers are deterministic
9. Create exploit prevention: locked settings prevent mid-campaign changes
10. Add tests: preset application, slider adjustment, system integration, save round-trip
11. Verify all modifiers apply correctly
12. Test edge cases: minimum difficulty (all 0.5x), maximum difficulty (all 2.0x)
13. Verify headless behavior: difficulty applies correctly without UI
14. Add data-integrity-selftest: presets validate against system catalogs
15. Create `--difficulty-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --difficulty-selftest
```

## Risk

**LOW** — Difficulty settings are straightforward multipliers with clear inputs (preset/sliders) and outputs (modified game values). Risk of difficulty feeling meaningless if modifiers are too subtle. Mitigation: make modifier ranges meaningful (0.5x-2.0x), show clear effect descriptions, and provide preset recommendations.

## Definition of Done

- `DifficultySettingsSystem.cs` exists with full `CaptureState/RestoreState`
- 4 difficulty presets (Easy, Normal, Hard, Nightmare)
- 9 customizable sliders (custom mode)
- Difficulty effects on 6+ systems (radiation, needs, market, expedition, weather, crafting)
- Settings lock option
- Difficulty selection at campaign start
- HUD difficulty indicator
- Difficulty events and quest hooks
- Save/load round-trip tested
- Old saves get Normal difficulty
- 4 presets in data authority
- UI settings panel with presets and sliders
- Cross-system integration (radiation, needs, market, expedition, weather, crafting)
- Integration with Plan 175 (NG+ modifiers stack)

## Follow-On Opportunities

- Difficulty achievements (complete on each difficulty)
- Difficulty leaderboards (fastest completion per difficulty)
- Difficulty sharing (share custom configurations)
- Difficulty rotation (weekly challenge difficulties)
- Difficulty legacy (difficulties mastered remembered)
