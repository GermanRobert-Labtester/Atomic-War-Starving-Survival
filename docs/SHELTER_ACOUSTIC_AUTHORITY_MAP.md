# Plan 53 — Subterranean Diegetic Soundscape & Dynamic Acoustic Ambiance
## Authority & Domain Mapping

**Catalog:** `Assets/StreamingAssets/Data/shelter_audio_cues.json` (`schema_version: 1`)
**Core System:** `Assets/Ashfall.Core/Audio/ShelterAcousticDirector.cs`
**Host Projection:** `src/Audio/ShelterAcousticBridge.cs`
**Presentation Node:** `AudioManager.cs`

### 1. Separation of Responsibilities
- `ShelterAcousticDirector` (Core): Evaluates shelter simulation facts (generator wattage, ventilation status, radiation levels, excavation structural hazard permille, radio events, bulkhead openings) and outputs a deterministic set of active acoustic layers with normalized intensities (`0..1000`) and one-shot cue requests.
- `AudioManager` (Godot Host): Receives semantic layer intensities and one-shot cues, mapping them to audio buses (`generator`, `ventilation`, `machinery`, `alerts`, `subterranean`, `radio`, `sfx`), crossfading audio streams, applying bus ducking, and controlling low-pass filter environmental profiles.
- Headless Safety: The Core director executes with zero Godot or audio driver dependencies, enabling deterministic test verification without audio hardware.
