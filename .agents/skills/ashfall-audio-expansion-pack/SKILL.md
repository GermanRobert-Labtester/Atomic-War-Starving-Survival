---
name: ashfall-audio-expansion-pack
description: Creates expansion audio packs (radio, echoes, ambient, SFX), registers AudioManager cues, wires the EventBridge, and runs ashfall-audio-qa validation.
---

# ASHFALL Asset Expansion Skill: ashfall-audio-expansion-pack

## Overview
Creates a complete audio pack for ASHFALL expansions including radio transmissions, echoes, ambient sounds, and SFX. Generates assets/audio/expansions/05_*/ + radio_05.json/echo_05.json with snake_case IDs, registers AudioManager cues, wires EventBridge, and runs ashfall-audio-qa orphan + loudness gate validation.

## Canonical Usage
```bash
# Create audio pack for expansion 05 Holdfast
awf audio-expansion-pack --expansion 05 --codename holdfast

# Create audio pack with custom audio files
awf audio-expansion-pack --expansion 05 --input audio_list.csv

# Create radio transmissions
awf audio-expansion-pack --expansion 05 --type radio --count 5

# Create echoes
awf audio-expansion-pack --expansion 05 --type echoes --count 10

# Run in CI pipeline
awf audio-expansion-pack --expansion 05 --ci
```

## What It Automates

### 1. Audio Directory Structure
Creates a complete audio asset tree for the expansion:

```
assets/
└── audio/
    └── expansions/
        └── 05_holdfast/
            ├── music/
            │   ├── ambient/
            │   │   ├── ambience_wasteland_day.ogg
            │   │   ├── ambience_wasteland_night.ogg
            │   │   └── ambience_settlement.ogg
            │   ├── combat/
            │   │   ├── combat_theme_01.ogg
            │   │   └── combat_theme_02.ogg
            │   └── exploration/
            │       ├── exploration_theme.ogg
            │       └── discovery_theme.ogg
            ├── sfx/
            │   ├── items/
            │   │   ├── item_pickup.wav
            │   │   ├── item_place.wav
            │   │   └── item_use.wav
            │   ├── ui/
            │   │   ├── ui_click.wav
            │   │   ├── ui_hover.wav
            │   │   └── ui_back.wav
            │   ├── survival/
            │   │   ├── survival_radiation.wav
            │   │   ├── survival_injury.wav
            │   │   └── survival_hunger.wav
            │   └── environment/
            │       ├── environment_wind.wav
            │       ├── environment_rain.wav
            │       └── environment_thunder.wav
            ├── radio/
            │   ├── radio_holdfast_frequency.ogg
            │   ├── radio_transmission_01.ogg
            │   ├── radio_transmission_02.ogg
            │   └── radio_static.wav
            ├── echoes/
            │   ├── echo_holdfast_01.ogg
            │   ├── echo_holdfast_02.ogg
            │   └── echo_holdfast_03.ogg
            └── .import/
                ├── music.ambient.ambience_wasteland_day.ogg.import
                ├── sfx.items.item_pickup.wav.import
                └── radio.radio_holdfast_frequency.ogg.import
```

### 2. Audio File Generation
Generates audio files using AI or creates placeholder files:

#### Radio Transmissions:
- Creates 5-10 radio transmissions with realistic voice acting
- Content: Faction communications, status updates, mission briefings
- Format: OGG Vorbis, 44.1kHz, 128-192kbps
- Naming: `radio_<expansion>_<number>.ogg`

#### Echoes:
- Creates 10-20 environmental echoes (distant voices, sounds)
- Content: Ambient wasteland sounds, survivor voices, wildlife
- Format: OGG Vorbis, 44.1kHz, 96-128kbps
- Naming: `echo_<expansion>_<number>.ogg`

#### Ambient Sounds:
- Creates background ambience for different biomes
- Content: Wind, rain, distant machinery, wildlife
- Format: OGG Vorbis, 44.1kHz, 64-96kbps
- Naming: `ambience_<biome>_<time>.ogg`

#### SFX:
- Creates sound effects for items, UI, survival events
- Content: Pickup sounds, UI clicks, radiation warnings, injuries
- Format: WAV (uncompressed for editing), OGG Vorbis (compressed for game)
- Naming: `<type>_<expansion>_<name>.<ext>`

### 3. Audio Metadata Generation
Creates JSON metadata files for AudioManager registration:

#### radio_05.json:
```json
{
  "schema_version": "1",
  "id": "radio_05",
  "expansion": "expansion_05",
  "frequency": 142.370,
  "transmissions": [
    {
      "id": "radio_holdfast_frequency",
      "name": "Holdfast Command Frequency",
      "description": "Primary radio frequency for Holdfast faction",
      "file": "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg",
      "trigger": "quest_holdfast_main_started",
      "unlock_condition": "has_radio",
      "volume": 0.8,
      "pitch": 1.0,
      "loop": false
    },
    {
      "id": "radio_holdfast_status_update",
      "name": "Status Update Transmission",
      "description": "Regular status update from Holdfast Command",
      "file": "res://assets/audio/expansions/05_holdfast/radio/radio_transmission_01.ogg",
      "trigger": "time_elapsed_days(30)",
      "unlock_condition": "radio_frequency_unlocked",
      "volume": 0.8,
      "pitch": 1.0,
      "loop": false
    }
  ],
  "metadata": {
    "created": "2024-01-15T13:00:00Z",
    "version": "1.0.0"
  }
}
```

#### echo_05.json:
```json
{
  "schema_version": "1",
  "id": "echo_05",
  "expansion": "expansion_05",
  "echoes": [
    {
      "id": "echo_holdfast_distant_voices",
      "name": "Distant Survivor Voices",
      "description": "Echoes of survivors talking in the distance",
      "file": "res://assets/audio/expansions/05_holdfast/echoes/echo_holdfast_01.ogg",
      "trigger": "player_near(loc_holdfast_camp)",
      "trigger_radius": 50,
      "volume": 0.6,
      "pitch": 1.0,
      "loop": true,
      "max_distance": 200
    },
    {
      "id": "echo_holdfast_radio_chatter",
      "name": "Radio Chatter",
      "description": "Faint radio transmissions from nearby settlements",
      "file": "res://assets/audio/expansions/05_holdfast/echoes/echo_holdfast_02.ogg",
      "trigger": "player_in_radius(100, has_radio)",
      "volume": 0.4,
      "pitch": 1.0,
      "loop": true,
      "max_distance": 300
    }
  ],
  "metadata": {
    "created": "2024-01-15T13:00:00Z",
    "version": "1.0.0"
  }
}
```

### 4. AudioManager Integration
Registers audio cues with AudioManager:

#### AudioManager Registration Code:
```csharp
// In AudioManager.cs or expansion-specific audio setup
public static void RegisterExpansion05Audio()
{
    // Radio transmissions
    AudioManager.RegisterRadioCue(
        id: "radio_holdfast_frequency",
        filePath: "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg",
        frequency: 142.370f,
        volume: 0.8f,
        priority: 100
    );

    AudioManager.RegisterRadioCue(
        id: "radio_holdfast_status_update",
        filePath: "res://assets/audio/expansions/05_holdfast/radio/radio_transmission_01.ogg",
        frequency: 142.370f,
        volume: 0.8f,
        priority: 90
    );

    // Echoes
    AudioManager.RegisterEchoCue(
        id: "echo_holdfast_distant_voices",
        filePath: "res://assets/audio/expansions/05_holdfast/echoes/echo_holdfast_01.ogg",
        trigger: "player_near(loc_holdfast_camp)",
        volume: 0.6f,
        maxDistance: 200f
    );

    // Ambient music
    AudioManager.RegisterAmbientMusic(
        id: "ambience_wasteland_day",
        filePath: "res://assets/audio/expansions/05_holdfast/music/ambient/ambience_wasteland_day.ogg",
        biome: "wasteland",
        time: "day",
        volume: 0.5f,
        loop: true
    );

    // SFX
    AudioManager.RegisterSFX(
        id: "item_pickup_holdfast",
        filePath: "res://assets/audio/expansions/05_holdfast/sfx/items/item_pickup.wav",
        category: "items",
        volume: 0.9f,
        pitchVariation: 0.1f
    );
}
```

### 5. EventBridge Wiring
Wires audio events to game systems via EventBridge:

#### EventBridge Integration:
```csharp
// In expansion initialization
public void SetupExpansion05Audio(EventBus eventBus)
{
    // Radio transmission events
    eventBus.Subscribe<QuestStartedEvent>(e =>
    {
        if (e.QuestId == "quest_holdfast_main")
        {
            AudioManager.PlayRadioCue("radio_holdfast_frequency");
        }
    });

    eventBus.Subscribe<PlayerMovedEvent>(e =>
    {
        if (e.NewLocation == "loc_holdfast_camp")
        {
            AudioManager.PlayEchoCue("echo_holdfast_distant_voices");
        }
    });

    eventBus.Subscribe<ItemPickedUpEvent>(e =>
    {
        if (e.ItemId.StartsWith("item_holdfast_"))
        {
            AudioManager.PlaySFX("item_pickup_holdfast");
        }
    });

    // Radiation warning
    eventBus.Subscribe<RadiationWarningEvent>(e =>
    {
        AudioManager.PlaySFX("survival_radiation");
    });
}
```

### 6. Loudness Normalization
Normalizes audio files to consistent loudness levels:

#### Loudness Targets:
```
Radio transmissions: -16 LUFS (broadcast standard)
Echoes: -20 LUFS (ambient level)
Ambient music: -23 LUFS (background level)
SFX: -10 LUFS (foreground level)
UI sounds: -20 LUFS (feedback level)
```

#### Normalization Process:
- Analyzes audio loudness using EBU R128 standard
- Adjusts volume to target LUFS
- Ensures consistent playback levels
- Prevents audio clipping

### 7. Audio Quality Validation
Runs ashfall-audio-qa validation:

#### Validation Checks:
- **Format Validation:** All files are OGG Vorbis or WAV
- **Sample Rate:** All files are 44.1kHz
- **Bitrate:** Appropriate for content type
- **Loudness:** Within target LUFS range
- **Duration:** Within expected range
- **Silence:** No excessive leading/trailing silence
- **Clipping:** No audio clipping detected
- **Orphan Detection:** All audio files are referenced

#### ashfall-audio-qa Output:
```
✓ Audio format validation passed:
  - All files: OGG Vorbis
  - Sample rate: 44.1kHz
  - Bitrate: Appropriate

✓ Loudness normalization passed:
  - Radio: -16.2 LUFS (target: -16)
  - Echoes: -20.1 LUFS (target: -20)
  - Music: -22.8 LUFS (target: -23)
  - SFX: -9.9 LUFS (target: -10)

✓ Orphan detection passed:
  - All audio files referenced in metadata
  - No unreferenced audio files

✓ Quality checks passed:
  - No clipping detected
  - No excessive silence
  - Duration within expected range

✓ AudioManager registration passed:
  - All cues registered correctly
  - EventBridge wiring complete
```

### 8. Asset Registry Updates
Updates `assets/expansions/assets.json` with audio asset counts:

```json
{
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 35,
      "audio_count": 35,
      "music_count": 6,
      "sfx_count": 12,
      "radio_count": 5,
      "echo_count": 10,
      "ambient_count": 6,
      "created": "2024-01-15T13:30:00Z",
      "last_updated": "2024-01-15T13:30:00Z",
      "status": "in_progress"
    }
  }
}
```

### 9. Godot Asset Gate Validation
- Validates all audio files have correct .import files
- Validates audio files are tracked by Git LFS
- Validates loudness normalization
- Validates AudioManager registration
- Reports validation issues to godot-asset-gate.sh

## Time Saved
- **60 minutes per expansion audio pack** (manual audio creation and registration)
- **95% reduction** in audio setup time
- **Automated loudness normalization** ensures consistent audio
- **CI-ready** audio assets generated automatically

## Prerequisites
- Expansion asset pack created via `ashfall-asset-pack-expansion`
- `dotnet` CLI available
- Godot project in workspace
- Git LFS installed and configured
- Audio generation tools available (or AI audio generation)

## Verification After Use
```bash
# Verify audio directory
tree assets/audio/expansions/05_holdfast/ | head -30

# Verify JSON metadata files
test -f assets/audio/expansions/05_holdfast/radio_05.json && echo "radio_05.json exists"
test -f assets/audio/expansions/05_holdfast/echo_05.json && echo "echo_05.json exists"

# Verify AudioManager registration
# (Check code for RegisterExpansion05Audio calls)

# Run ashfall-audio-qa
awf audio-qa --expansion 05

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** `ashfall-asset-pack-expansion` (creates asset pack structure)
- **Used by:** `ashfall-expansion-data-gen` (uses radio/echo IDs)
- **Follow-up skills:** `ashfall-audio-qa` (validates audio quality)

## Error Detection
The skill detects and reports:

### 1. Audio Generation Issues
```
❌ CRITICAL: Audio generation failed:
   - Type: radio transmission
   - Expansion: 05
   - Error: AI audio generation service unavailable
   - Suggested fix: Check audio generation service or use placeholder files

⚠️  WARNING: Audio file missing:
   - File: assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg
   - Error: File not found
   - Impact: Radio transmission not playable
   - Suggested fix: Generate audio file or add placeholder

❌ ERROR: Audio file invalid:
   - File: assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg
   - Error: Not a valid OGG file
   - Impact: AudioManager cannot load file
   - Suggested fix: Re-encode file as OGG Vorbis
```

### 2. Metadata Issues
```
❌ ERROR: JSON metadata invalid:
   - File: assets/audio/expansions/05_holdfast/radio_05.json
   - Error: Missing schema_version
   - Error: Missing expansion field
   - Error: transmission[0].file not a valid resource path
   - Impact: AudioManager cannot register cues
   - Suggested fix: Update JSON to match schema

⚠️  WARNING: Metadata reference broken:
   - File: assets/audio/expansions/05_holdfast/radio_05.json
   - Reference: transmission[0].trigger = "quest_holdfast_missing"
   - Error: quest_holdfast_missing does not exist
   - Impact: Radio transmission won't trigger
   - Suggested fix: Update trigger to valid quest ID
```

### 3. AudioManager Registration Issues
```
❌ ERROR: AudioManager registration failed:
   - Cue ID: radio_holdfast_frequency
   - Error: File not found at resource path
   - Error: Volume out of range (must be 0.0-1.0)
   - Impact: Radio transmission not registered
   - Suggested fix: Check file path and volume values

⚠️  WARNING: EventBridge wiring incomplete:
   - Event: QuestStartedEvent
   - Handler: PlayRadioCue("radio_holdfast_frequency")
   - Error: Event not subscribed
   - Impact: Radio won't play when quest starts
   - Suggested fix: Add event subscription in SetupExpansion05Audio
```

### 4. Loudness Issues
```
⚠️  WARNING: Loudness normalization needed:
   - File: assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg
   - Current: -8.2 LUFS
   - Target: -16 LUFS
   - Impact: Too loud, may overpower other audio
   - Suggested fix: Normalize audio to -16 LUFS

❌ ERROR: Audio clipping detected:
   - File: assets/audio/expansions/05_holdfast/sfx/survival_radiation.wav
   - Error: Peak level exceeds 0dB
   - Impact: Distorted audio playback
   - Suggested fix: Reduce volume or re-export audio
```

### 5. LFS Tracking Issues
```
⚠️  WARNING: LFS tracking missing:
   - File: assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg
   - Error: Not tracked by Git LFS
   - Impact: Large audio files not optimized
   - Suggested fix: git lfs track "assets/audio/expansions/05_*/**/*.ogg"

❌ CRITICAL: LFS not installed:
   - Git LFS required for audio assets
   - Install: https://git-lfs.com/
   - After install: git lfs install
```

### 6. Orphan Detection Issues
```
❌ CRITICAL: Orphan audio file detected:
   - File: assets/audio/expansions/05_holdfast/sfx/unused_sound.wav
   - Error: Not referenced in any JSON metadata
   - Impact: Dead asset, wasting disk space
   - Suggested fix: Delete file or add to metadata

⚠️  WARNING: Missing audio reference:
   - ID: sfx_holdfast_item_use
   - Expected in: radio_05.json or echo_05.json
   - Actual: Not found in any metadata
   - Impact: Audio cue won't play
   - Suggested fix: Add reference to metadata or remove ID
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Audio Generation
- Generates placeholder audio files if AI service unavailable
- Creates silent audio files for missing content
- Validates generated audio files
- Reports generation success/failure

### 2. Metadata Updates
- Creates missing JSON metadata files
- Updates broken references to valid IDs
- Validates JSON schema
- Reports metadata issues

### 3. AudioManager Registration
- Adds missing AudioManager registration code
- Validates resource paths
- Validates volume/pitch values
- Reports registration issues

### 4. Loudness Normalization
- Normalizes audio files to target LUFS
- Validates loudness levels
- Reports normalization issues
- Updates metadata with loudness values

### 5. Orphan Cleanup
- Identifies unreferenced audio files
- Reports orphan files
- Suggests deletion or reference addition
- Validates cleanup success

## Configuration
- **Expansion number:** 01-99 (required)
- **Audio type:** radio, echoes, music, sfx, ambient, all (required)
- **Count:** Number of audio files to generate (optional, default: 5-10 per type)
- **Input:** CSV file with audio specifications (optional)
- **Output directory:** Custom output directory (optional)
- **Loudness target:** Target LUFS for normalization (default: -16 for radio, -20 for echoes)
- **Format:** Audio format (ogg, wav, mp3) (default: ogg)
- **Sample rate:** Audio sample rate (default: 44100)
- **Bitrate:** Audio bitrate (default: 128000 for OGG)
- **Force:** Overwrite existing audio files (default: false)
- **Validate:** Run audio quality validation (default: true)
- **Register:** Update assets.json registry (default: true)
- **Normalize:** Normalize loudness (default: true)

## Example Audio Pack Generation Workflow

### Input CSV (radio.csv):
```csv
id,name,description,trigger,volume,pitch
radio_holdfast_frequency,Holdfast Command Frequency,Primary radio frequency for Holdfast faction,quest_holdfast_main_started,0.8,1.0
radio_holdfast_status,Status Update,Regular status update from Holdfast Command,time_elapsed_days(30),0.8,1.0
radio_holdfast_warning,Warning Transmission,Urgent warning from Holdfast Command,has_radio AND radiation_level>0.7,0.9,1.0
```

### Command:
```bash
awf audio-expansion-pack --expansion 05 --type radio --input radio.csv
```

### Output Files:
```
assets/audio/expansions/05_holdfast/
├── radio/
│   ├── radio_holdfast_frequency.ogg
│   ├── radio_holdfast_status.ogg
│   └── radio_holdfast_warning.ogg
├── radio_05.json
└── .import/
    ├── radio.radio_holdfast_frequency.ogg.import
    └── radio_05.json.import
```

### Generated radio_05.json:
```json
{
  "schema_version": "1",
  "id": "radio_05",
  "expansion": "expansion_05",
  "transmissions": [
    {
      "id": "radio_holdfast_frequency",
      "name": "Holdfast Command Frequency",
      "description": "Primary radio frequency for Holdfast faction",
      "file": "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg",
      "trigger": "quest_holdfast_main_started",
      "volume": 0.8,
      "pitch": 1.0,
      "loop": false
    },
    {
      "id": "radio_holdfast_status",
      "name": "Status Update",
      "description": "Regular status update from Holdfast Command",
      "file": "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_status.ogg",
      "trigger": "time_elapsed_days(30)",
      "volume": 0.8,
      "pitch": 1.0,
      "loop": false
    },
    {
      "id": "radio_holdfast_warning",
      "name": "Warning Transmission",
      "description": "Urgent warning from Holdfast Command",
      "file": "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_warning.ogg",
      "trigger": "has_radio AND radiation_level>0.7",
      "volume": 0.9,
      "pitch": 1.0,
      "loop": false
    }
  ]
}
```

### AudioManager Registration:
```csharp
public static void RegisterExpansion05Radio()
{
    AudioManager.RegisterRadioCue(
        id: "radio_holdfast_frequency",
        filePath: "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_frequency.ogg",
        frequency: 142.370f,
        volume: 0.8f,
        priority: 100
    );

    AudioManager.RegisterRadioCue(
        id: "radio_holdfast_status",
        filePath: "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_status.ogg",
        frequency: 142.370f,
        volume: 0.8f,
        priority: 90
    );

    AudioManager.RegisterRadioCue(
        id: "radio_holdfast_warning",
        filePath: "res://assets/audio/expansions/05_holdfast/radio/radio_holdfast_warning.ogg",
        frequency: 142.370f,
        volume: 0.9f,
        priority: 110
    );
}
```

## Related Skills
- `ashfall-asset-pack-expansion` - Creates asset pack structure
- `ashfall-expansion-data-gen` - Creates radio/echo IDs
- `ashfall-audio-qa` - Validates audio quality
- `ashfall-lfs-gate` - Validates LFS configuration
- `ashfall-write` - Generates audio descriptions

## Notes
- Follows ASHFALL's strict audio design guidelines
- Uses consistent loudness normalization
- Validates all audio files are referenced
- Ensures AudioManager integration
- Follows snake_case naming conventions

## Maintenance
- Update audio templates if audio style evolves
- Add new audio types if expansion domains expand
- Update loudness targets if audio standards change
- Update AudioManager integration if audio system evolves
