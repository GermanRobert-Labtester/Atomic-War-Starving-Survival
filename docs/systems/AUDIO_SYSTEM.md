# ASHFALL Audio System Architecture

## Overview

The ASHFALL audio pipeline connects engine-agnostic Core domain simulation events to Godot-native audio playback through thin host bridges, preventing engine coupling while ensuring responsive, dynamic feedback.

## Architecture

```
[ Ashfall.Core Domain Systems ] (RadiationSystem, WeatherSystem, CombatSystem)
             │  C# Domain Events
             ▼
[ AtomicWar.GodotApp.Audio.AudioEventBridge ] (src/Audio/AudioEventBridge.cs)
             │  Maps domain events to Cue IDs
             ▼
[ AtomicWar.GodotApp.Audio.AudioManager ] (src/Audio/AudioManager.cs)
             │  Loads AudioStream resources & manages channels (Music, Ambient, SFX, Radio, UI)
             ▼
[ Godot AudioServer / AudioStreamPlayer2D ]
```

## Key Components

1. **`AudioManager` (`src/Audio/AudioManager.cs`)**:
   - Master volume, bus routing, and concurrent channel management.
   - Busses: `Master`, `Music`, `Ambient`, `SFX`, `Radio`, `UI`.
   - Dynamic ducking during radio broadcasts and dialogue.

2. **`AudioCueCatalog` (`src/Audio/AudioCueCatalog.cs`)**:
   - Constant identifiers for all sound effects, UI clicks, alerts, radiation Geiger ticks, weather ambience, and combat impact cues.

3. **`AudioEventBridge` (`src/Audio/AudioEventBridge.cs`)**:
   - Subscribes to domain events (e.g. `RadiationSystem.OnStatusGained`, `WeatherSystem.OnWeatherChanged`) and invokes `AudioManager.PlayCue()`.

4. **`AudioSettings` (`src/Audio/AudioSettings.cs`)**:
   - Persisted user volume preferences and audio device configuration.

## Verification & QA

- Headless audio cue tests: `godot --headless --path . -- --audio-selftest`
- Audio asset QA & loudness audit: `.agents/skills/ashfall-audio-qa/SKILL.md`
