# ASHFALL Expansion System Skill: ashfall-expansion-scaffold

## Overview
Automates Phase 1–5 skeleton generation for new ASHFALL expansions (Holdfast, Duty Roster, Standing Record, Crossing, etc.). Creates a complete expansion foundation in minutes instead of hours.

## Canonical Usage
```bash
/expand 05 holdfast      # Creates expansion #5 "Holdfast" skeleton
/expand 03 duty_roster   # Creates expansion #3 "Duty Roster" skeleton
```

## What It Automates

### Phase 1: Core System Skeleton
- Creates `Ashfall.Core/Expansions/Expansion05Holdfast/` namespace
- Generates `Expansion05HoldfastSystem.cs` with:
  - `CaptureState()` / `RestoreState()` stubs
  - `Tick()` stub with "wired in Phase 11" comment
  - Constructor with dependency injection
  - Event wiring stubs
- Creates `Expansion05HoldfastDto.cs` for save wire contract

### Phase 2: Data Authority Stubs
- Creates `Assets/StreamingAssets/Data/expansions/expansion_05.json` with:
  - `schema_version: "1"`
  - `id: "expansion_05"`
  - `name: "Holdfast"`
  - Empty arrays for `item_ids`, `location_ids`, `quest_ids`, `npc_ids`
- Creates `expansion_05_ids.cs` static class with:
  - `public const string Id = "expansion_05";`
  - `public static readonly string[] AllItemIds = Array.Empty<string>();`
  - `public static readonly string[] AllLocationIds = Array.Empty<string>();`

### Phase 3: GameBootstrap Integration Stubs
- Adds to `GameBootstrap.Phase0Expansion.cs`:
  - `expansion05HoldfastSystem = new Expansion05HoldfastSystem(...);`
  - `eventBus.Subscribe(expansion05HoldfastSystem);`
  - `simLoop.RegisterSystem(expansion05HoldfastSystem);`
  - `saveManager.RegisterStore(expansion05HoldfastSystem);`
  - Comment: "// wired in Phase 11"

### Phase 4: Test Stubs
- Creates `Ashfall.Core.Tests/Expansions/Expansion05HoldfastSystemTests.cs` with:
  - `CanCaptureAndRestoreState()` stub
  - `Tick_WhenInitialized_DoesNothing()` stub
  - `Constructor_WithDependencies_Initializes()` stub

### Phase 5: Documentation
- Creates `Expansion05HoldfastSystem.docs.mdx` with usage examples
- Adds composition example for `bit start` preview

## Time Saved
- **4 hours per expansion** (manual Phase 1–5)
- **87% reduction** in setup time

## Prerequisites
- ASHFALL Core project loaded
- `dotnet` CLI available
- Godot project in workspace

## Verification After Use
```bash
# Verify compilation
dotnet build Ashfall.Core/Ashfall.Core.csproj

# Verify data integrity
godot --headless --path . -- --data-integrity-selftest

# Verify catalog integrity
# (CatalogIntegrityValidator should pass with new expansion_05.json)
```

## Integration Points
- **Depends on:** None (pure code generation)
- **Used by:** ashfall-expansion-tick-wire, ashfall-expansion-save-evolve
- **Follow-up skills:** ashfall-expansion-data-gen, ashfall-expansion-narrative-weave

## Error Handling
- Fails if expansion code already exists
- Validates expansion number is 01-99
- Validates codename follows snake_case
- Validates JSON schema_version is set
- Validates static Ids.cs constants are generated

## Example Output Structure
```
Assets/Ashfall.Core/
└── Expansions/
    └── Expansion05Holdfast/
        ├── Expansion05HoldfastSystem.cs
        ├── Expansion05HoldfastDto.cs
        └── Expansion05HoldfastSystem.docs.mdx

Assets/StreamingAssets/Data/expansions/
└── expansion_05.json

Assets/Ashfall.Core/
└── Ids/
    └── expansion_05_ids.cs

Ashfall.Core.Tests/
└── Expansions/
    └── Expansion05HoldfastSystemTests.cs
```

## Related Skills
- `ashfall-expansion-data-gen` - Bulk data generation for the expansion
- `ashfall-expansion-narrative-weave` - Weave quests/flags into base narrative
- `ashfall-expansion-tick-wire` - Verify GameBootstrap integration
- `ashfall-expansion-save-evolve` - Save system evolution for the expansion

## Notes
- Generates "wired in Phase 11" comments as placeholders for follow-up skills
- Creates empty arrays for IDs that will be populated by ashfall-expansion-data-gen
- Follows ASHFALL naming conventions: snake_case for IDs, PascalCase for classes
- Uses `ISeededRng` for any randomness (not `System.Random`)
- Uses `IJsonSerializer` for serialization (not `JsonUtility`)

## Maintenance
- Update template paths if project structure changes
- Add new expansion phases if expansion lifecycle evolves
- Update schema_version if data format changes
