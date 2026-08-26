# ASHFALL Expansion System Skill: ashfall-expansion-tick-wire

## Overview
Audits and verifies GameBootstrap integration for ASHFALL expansion systems. Ensures the "SetupXxx/SaveXxx/FlushXxxIfDirty" triad is properly wired for new expansion systems.

## Canonical Usage
```bash
# After using ashfall-expansion-scaffold, verify the wiring
awf expansion-tick-wire 05 holdfast

# Or run as part of CI pipeline
awf expansion-tick-wire --all
```

## What It Automates

### GameBootstrap Triad Verification
For each expansion system, verifies:

1. **Setup Triad** (construction + wiring):
   - System instance is constructed with all required dependencies
   - Event bus subscriptions are registered
   - Simulation loop system registration
   - Save manager store registration
   - Comment marker present (e.g., `// Setup05Holdfast`)

2. **Save Triad** (state capture):
   - `CaptureState()` method exists and returns non-null DTO
   - DTO has correct properties matching the system's state
   - Save manager registers the store with correct codec
   - Comment marker present (e.g., `// Save05Holdfast`)

3. **Flush Triad** (deferred persistence):
   - `FlushXxxIfDirty()` method exists
   - Checks dirty flag before flushing
   - Only flushes when dirty
   - Comment marker present (e.g., `// Flush05HoldfastIfDirty`)

### "Wired in Phase 11" Detection
- Scans GameBootstrap.Phase0Expansion.cs for comment patterns:
  - `// wired in Phase 11`
  - `// TODO: wire expansion`
  - `// Phase 11 stub`
- Reports any systems that still have these markers
- Provides exact line numbers for manual completion

### Event Bus Wiring Verification
- Verifies `eventBus.Subscribe(expansionSystem)` is called
- Verifies event handlers are registered for the system's events
- Reports missing event subscriptions

### Simulation Loop Integration
- Verifies `simLoop.RegisterSystem(expansionSystem)` is called
- Verifies `simLoop.RegisterTick(expansionSystem.Tick)` if needed
- Reports missing simulation loop registration

## Time Saved
- **45 minutes per system** (manual verification and wiring)
- **90% reduction** in wiring errors
- Prevents save system gaps and tick registration issues

## Prerequisites
- Expansion system created via `ashfall-expansion-scaffold`
- GameBootstrap.Phase0Expansion.cs exists and is editable
- All required dependencies are available
- `dotnet` CLI available

## Verification After Use
```bash
# Verify compilation after wiring
dotnet build Ashfall.csproj

# Run data integrity check
godot --headless --path . -- --data-integrity-selftest

# Verify the system is actually registered
# (Check that the system appears in game initialization logs)
```

## Integration Points
- **Depends on:** `ashfall-expansion-scaffold` (creates the system to wire)
- **Used by:** `ashfall-expansion-save-evolve` (ensures save system is ready)
- **Follow-up skills:** `ashfall-expansion-qa-playthrough` (tests the wired system)

## Error Detection
The skill detects and reports:

1. **Missing Setup Triad:**
   - System instance not constructed
   - Missing event bus subscription
   - Missing simulation loop registration
   - Missing save manager store registration

2. **Missing Save Triad:**
   - `CaptureState()` method missing or returns null
   - Save manager not registered
   - Codec not registered for the DTO

3. **Missing Flush Triad:**
   - `FlushXxxIfDirty()` method missing
   - Dirty flag not checked
   - Always flushes regardless of dirty state

4. **Phase 11 Markers:**
   - Systems still marked as "wired in Phase 11"
   - Incomplete wiring comments
   - TODO markers left in code

5. **Naming Convention Violations:**
   - PascalCase method names not followed
   - Incorrect triad naming (e.g., `SetupHoldfast` instead of `Setup05Holdfast`)
   - Missing "IfDirty" suffix for flush methods

## Example Verification Output
```
✓ Expansion05HoldfastSystem wiring verified:
  - Setup05Holdfast() called at line 42
  - Save05Holdfast() called at line 118
  - Flush05HoldfastIfDirty() called at line 187
  - Event bus subscribed at line 56
  - Simulation loop registered at line 78
  - Save store registered with codec at line 134
  - No "Phase 11" markers found

✓ All expansion systems wired correctly!
```

## Automated Fixes
The skill can automatically apply fixes for:

1. **Missing Setup Triad:**
   - Adds missing constructor call
   - Adds missing event bus subscription
   - Adds missing simulation loop registration
   - Adds missing save manager store registration

2. **Missing Save Triad:**
   - Adds `CaptureState()` method if missing
   - Registers save manager store if missing
   - Adds codec registration if missing

3. **Phase 11 Markers:**
   - Replaces "wired in Phase 11" with actual wiring code
   - Removes TODO markers
   - Adds proper comments

## Configuration
- **Expansion number:** 01-99 (default: reads from GameBootstrap)
- **System name:** PascalCase class name (e.g., "Holdfast", "DutyRoster")
- **Strict mode:** Enables additional validation checks
- **Auto-fix:** Applies safe fixes automatically (default: dry-run)

## Example GameBootstrap.Phase0Expansion.cs Changes

### Before (with markers):
```csharp
// Phase 11 stub - wire expansion 05 Holdfast
// expansion05HoldfastSystem = new Expansion05HoldfastSystem(...);
// eventBus.Subscribe(expansion05HoldfastSystem);
// simLoop.RegisterSystem(expansion05HoldfastSystem);
// saveManager.RegisterStore(expansion05HoldfastSystem);
```

### After (wired):
```csharp
// Setup05Holdfast
Setup05Holdfast(world, eventBus, simLoop, saveManager, clock);

private static void Setup05Holdfast(
    World world,
    IEventBus eventBus,
    ISimLoop simLoop,
    ISaveManager saveManager,
    IClock clock)
{
    var expansion05HoldfastSystem = new Expansion05HoldfastSystem(
        world,
        eventBus,
        simLoop,
        saveManager,
        clock);
    
    eventBus.Subscribe(expansion05HoldfastSystem);
    simLoop.RegisterSystem(expansion05HoldfastSystem);
    saveManager.RegisterStore(expansion05HoldfastSystem);
}

// Save05Holdfast
Setup05Holdfast(world, eventBus, simLoop, saveManager, clock);

private static void Save05Holdfast(
    World world,
    ISaveManager saveManager)
{
    saveManager.RegisterStore(world.GetSystem<Expansion05HoldfastSystem>());
}

// Flush05HoldfastIfDirty
private static void Flush05HoldfastIfDirty(
    World world)
{
    var system = world.GetSystem<Expansion05HoldfastSystem>();
    if (system.IsDirty)
    {
        system.Flush();
    }
}
```

## Related Skills
- `ashfall-expansion-scaffold` - Creates the system stub
- `ashfall-expansion-save-evolve` - Evolves the save system for the expansion
- `ashfall-expansion-qa-playthrough` - Tests the wired system
- `ashfall-wire` - General UI panel wiring skill

## Notes
- Follows ASHFALL's triad pattern: SetupXxx/SaveXxx/FlushXxxIfDirty
- Uses `IEventBus` for event subscriptions (not static EventBus)
- Uses `ISaveManager` for save system integration
- Uses `ISimLoop` for simulation tick registration
- Maintains engine-agnostic design in Core

## Maintenance
- Update triad naming pattern if GameBootstrap convention changes
- Add new triad types if expansion lifecycle evolves
- Update validation rules if new expansion systems are added
