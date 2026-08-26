# ASHFALL Expansion System Skill: ashfall-expansion-save-evolve

## Overview
Evolves the save system for ASHFALL expansions by bumping ExpansionMasterSession codec versions, adding MigrationV{n} classes, and ensuring save wire contracts and checksummed envelopes. Runs SaveWireContract and SaveStoreChecksumSweepTests gates to verify save compatibility.

## Canonical Usage
```bash
# Evolve save system for expansion 05 after DTO changes
awf expansion-save-evolve --expansion 05 --dto Expansion05HoldfastSystemDto

# Bulk evolve multiple expansions
awf expansion-save-evolve --expansion 05,06,07 --dto "Expansion05Dto,Expansion06Dto,Expansion07Dto"

# Run in CI pipeline
awf expansion-save-evolve --all --validate
```

## What It Automates

### 1. Codec Version Bump
For each expansion system:
- Bumps `ExpansionMasterSession` codec version from V1 to V2, V3, etc.
- Updates `CurrentCodecVersion` constant
- Adds new `MigrationV{n}` class for backward compatibility
- Maintains all previous migration paths

#### Example Version Migration:
```csharp
// Before (V1)
public const int CurrentCodecVersion = 1;

// After (V2)
public const int CurrentCodecVersion = 2;

// New migration class
public class MigrationV1ToV2 : SaveMigration
{
    public override int FromVersion => 1;
    public override int ToVersion => 2;
    
    public override SystemState Migrate(SystemState state)
    {
        // Migration logic from V1 to V2
        return state;
    }
}
```

### 2. DTO Shape Evolution
For each expansion DTO:
- Validates DTO has `[Serializable]` attribute
- Validates DTO has correct properties for the system state
- Validates DTO follows save wire contract naming conventions
- Reports missing or incorrect properties

#### Example DTO Evolution:
```csharp
// Before (V1)
[Serializable]
public class Expansion05HoldfastSystemDto
{
    public string[] ActiveQuests { get; set; }
    public int ReputationScore { get; set; }
}

// After (V2) - evolved DTO
[Serializable]
public class Expansion05HoldfastSystemDto
{
    public string[] ActiveQuests { get; set; }
    public int ReputationScore { get; set; }
    public string[] CompletedQuests { get; set; } // New property
    public bool IsFactionHostile { get; set; } // New property
}
```

### 3. Save Wire Contract Validation
- Validates `SaveWireContract` tests exist for the expansion
- Validates tests cover all DTO properties
- Validates tests cover edge cases (null, empty, corrupted data)
- Reports missing or incomplete save wire contracts

### 4. Checksummed Envelope Migration
For each expansion save store:
- Migrates from bare-state saves to checksummed envelope format
- Adds `Checksum` field to save envelope
- Validates checksum calculation is deterministic
- Reports checksum validation failures

#### Example Envelope Migration:
```json
// Before (bare-state, V1)
{
  "system": "expansion_05_holdfast",
  "data": {
    "activeQuests": ["quest_holdfast_main"],
    "reputationScore": 100
  }
}

// After (checksummed, V2)
{
  "system": "expansion_05_holdfast",
  "schema_version": "2",
  "checksum": "a1b2c3d4e5f6...",
  "data": {
    "activeQuests": ["quest_holdfast_main"],
    "reputationScore": 100
  }
}
```

### 5. Legacy Fallback Support
- Maintains backward compatibility with pre-checksum saves
- Implements `TryLoad` fallback for legacy saves
- Validates legacy save loading works correctly
- Reports legacy save issues

### 6. Save Store Registration
- Validates expansion save store is registered in `SaveManager`
- Validates codec is registered with correct migration paths
- Validates save store implements `ICaptureState` and `IRestoreState`
- Reports missing or incorrect save store registration

### 7. SaveStoreChecksumSweepTests Integration
- Runs `SaveStoreChecksumSweepTests` for the expansion
- Validates all save operations produce correct checksums
- Validates checksum mutations are detected
- Validates null checksums are rejected

## Time Saved
- **75 minutes per DTO change** (manual codec evolution and testing)
- **90% reduction** in save system bugs
- **Automated validation** eliminates manual testing
- **Immediate feedback** on save compatibility issues

## Prerequisites
- Expansion system created via `ashfall-expansion-scaffold`
- DTO created via expansion system development
- `SaveWireContract` tests exist (or will be created)
- `dotnet` CLI available
- Godot project in workspace

## Verification After Use
```bash
# Run save wire contract tests
dotnet test Ashfall.Core.Tests/SaveWireContractTests.cs

# Run save store checksum sweep tests
dotnet test Ashfall.Core.Tests/SaveStoreChecksumSweepTests.cs

# Verify save system compiles
dotnet build Ashfall.Core/Ashfall.Core.csproj

# Test save/load round-trip
godot --headless --path . -- --save-test "expansion_05"
```

## Integration Points
- **Depends on:** `ashfall-expansion-scaffold` (creates expansion system)
- **Used by:** `ashfall-expansion-tick-wire` (ensures save system is ready)
- **Follow-up skills:** `ashfall-expansion-qa-playthrough` (tests save system)

## Error Detection
The skill detects and reports:

### 1. Codec Version Issues
```
❌ ERROR: Codec version issue in ExpansionMasterSession:
   - CurrentCodecVersion is 1 but should be 2 for expansion 05
   - Missing MigrationV1ToV2 class
   - Migration path from V1 to V2 is incomplete

⚠️  WARNING: Codec version mismatch:
   - Expansion05HoldfastSystemDto has version 2 but codec expects version 1
   - Suggested fix: Bump codec version and add migration
```

### 2. DTO Issues
```
❌ ERROR: DTO validation failed for Expansion05HoldfastSystemDto:
   - Missing [Serializable] attribute
   - Property 'ActiveQuests' should be 'activeQuests' (camelCase)
   - Property 'ReputationScore' is missing from save wire contract
   - Property 'NewProperty' is not in DTO but referenced in tests

⚠️  WARNING: DTO evolution detected:
   - New properties added: CompletedQuests, IsFactionHostile
   - Missing migration logic for new properties
   - Suggested fix: Add migration or update codec version
```

### 3. Save Wire Contract Issues
```
❌ ERROR: Save wire contract validation failed:
   - SaveWireContractTests for expansion_05 does not exist
   - Missing test for null DTO
   - Missing test for corrupted data
   - Missing test for round-trip serialization

⚠️  WARNING: Save wire contract incomplete:
   - Test 'Expansion05HoldfastSystem_SaveWireContract_RoundTrip' only covers 60% of properties
   - Missing edge case tests
   - Suggested fix: Add comprehensive test coverage
```

### 4. Checksum Issues
```
❌ ERROR: Checksum validation failed:
   - Save envelope for expansion_05 does not have checksum field
   - Checksum calculation is not deterministic (uses DateTime.Now)
   - Checksum validation fails for legacy saves

⚠️  WARNING: Checksum migration needed:
   - Bare-state saves detected for expansion 05
   - Should migrate to checksummed envelope format
   - Suggested fix: Run checksum migration tool
```

### 5. Save Store Issues
```
❌ ERROR: Save store registration failed:
   - Expansion05HoldfastSystem is not registered in SaveManager
   - Codec for expansion_05 is not registered
   - Save store does not implement ICaptureState

⚠️  WARNING: Save store incomplete:
   - SaveAll() method does not include expansion_05 system
   - FlushIfDirty() method missing for expansion_05
   - Suggested fix: Update GameBootstrap save wiring
```

### 6. Migration Issues
```
❌ ERROR: Migration validation failed:
   - MigrationV1ToV2.Migrate() does not handle null state
   - MigrationV1ToV2 does not preserve all required data
   - Migration path is not tested in SaveStoreChecksumSweepTests

⚠️  WARNING: Migration incomplete:
   - Migration from V1 to V2 only handles 70% of data
   - Missing migration for new properties in V2
   - Suggested fix: Complete migration logic
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Codec Version Bump
- Bumps `CurrentCodecVersion` to next version
- Creates `MigrationV{From}To{To}` class
- Updates migration path matrix
- Validates version bump is correct

### 2. DTO Updates
- Adds missing `[Serializable]` attribute
- Converts property names to camelCase
- Adds missing properties from save wire contract
- Removes unused properties

### 3. Save Wire Contract
- Creates missing `SaveWireContractTests`
- Adds comprehensive test coverage
- Includes edge case tests (null, empty, corrupted)
- Validates round-trip serialization

### 4. Checksum Migration
- Migrates bare-state saves to checksummed format
- Adds `Checksum` field to save envelopes
- Validates checksum calculation
- Updates save store to use checksum validation

### 5. Save Store Registration
- Registers expansion save store in `SaveManager`
- Registers codec with migration paths
- Implements `ICaptureState` and `IRestoreState`
- Adds `SaveAll()` and `FlushIfDirty()` methods

### 6. Migration Logic
- Implements complete migration from previous version
- Preserves all required data
- Handles edge cases (null, corrupted)
- Adds migration tests

## Configuration
- **Expansion number:** 01-99 (required)
- **DTO class:** Full class name with namespace (required)
- **Migration type:** full, partial, dry-run (default: full)
- **Strict mode:** Enable additional validation (default: true)
- **Auto-fix:** Apply safe fixes automatically (default: dry-run)
- **Validate:** Run save tests after evolution (default: true)
- **Backup:** Create backup of existing saves (default: true)

## Example Save System Evolution

### Before (V1 - bare state):
```csharp
// ExpansionMasterSession.cs (V1)
public class ExpansionMasterSession : ISaveWire<ExpansionMasterSessionDto>
{
    public const int CurrentCodecVersion = 1;
    
    public SystemState CaptureState()
    {
        return new SystemState
        {
            Version = CurrentCodecVersion,
            Data = new ExpansionMasterSessionDto
            {
                Holdfast = new Expansion05HoldfastSystemDto
                {
                    ActiveQuests = new[] { "quest_holdfast_main" },
                    ReputationScore = 100
                }
            }
        };
    }
    
    public void RestoreState(SystemState state) { }
}
```

### After (V2 - with migrations and checksums):
```csharp
// ExpansionMasterSession.cs (V2)
public class ExpansionMasterSession : ISaveWire<ExpansionMasterSessionDto>
{
    public const int CurrentCodecVersion = 2;
    
    private static readonly Dictionary<int, Type> Migrations = new()
    {
        { 1, typeof(MigrationV1ToV2) }
    };
    
    public SystemState CaptureState()
    {
        var state = new SystemState
        {
            Version = CurrentCodecVersion,
            Data = new ExpansionMasterSessionDto
            {
                Holdfast = new Expansion05HoldfastSystemDto
                {
                    ActiveQuests = new[] { "quest_holdfast_main" },
                    ReputationScore = 100,
                    CompletedQuests = new string[0],
                    IsFactionHostile = false
                }
            }
        };
        
        state.Checksum = SaveChecksum.Calculate(state);
        return state;
    }
    
    public void RestoreState(SystemState state)
    {
        // Apply migrations if needed
        if (state.Version < CurrentCodecVersion)
        {
            state = ApplyMigrations(state);
        }
        
        // Validate checksum
        if (!SaveChecksum.Validate(state))
        {
            throw new SaveCorruptionException("Checksum validation failed");
        }
    }
    
    private SystemState ApplyMigrations(SystemState state)
    {
        // Migration logic here
        return state;
    }
}

// MigrationV1ToV2.cs
public class MigrationV1ToV2 : SaveMigration
{
    public override int FromVersion => 1;
    public override int ToVersion => 2;
    
    public override SystemState Migrate(SystemState state)
    {
        var dto = state.Data as ExpansionMasterSessionDto;
        var oldHoldfast = dto.Holdfast as dynamic; // V1 DTO
        
        dto.Holdfast = new Expansion05HoldfastSystemDto
        {
            ActiveQuests = oldHoldfast.ActiveQuests,
            ReputationScore = oldHoldfast.ReputationScore,
            CompletedQuests = new string[0],
            IsFactionHostile = false
        };
        
        return state;
    }
}
```

## Related Skills
- `ashfall-expansion-scaffold` - Creates expansion system
- `ashfall-expansion-tick-wire` - Wires up the system
- `ashfall-save-migration` - General save migration patterns
- `ashfall-save-fuzz` - Save corruption testing
- `ashfall-expansion-qa-playthrough` - Tests save system in gameplay

## Notes
- Follows ASHFALL's strict save wire contract patterns
- Validates all save operations produce correct checksums
- Maintains backward compatibility with legacy saves
- Uses `SaveChecksum` for deterministic integrity verification
- Follows engine-agnostic design in Core

## Maintenance
- Update migration templates if save system evolves
- Add new migration patterns if codec versioning changes
- Update validation rules if SaveStoreChecksumSweepTests changes
- Add new save store types if expansion systems expand
