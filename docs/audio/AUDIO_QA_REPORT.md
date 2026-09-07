# ASHFALL audio QA report

## Current result

The unified output path is `AudioManager` plus Core-event bridges. Gameplay
systems emit typed events; the Godot host maps them to catalog cue IDs; only
`AudioManager` touches `AudioStreamPlayer`. No audio callback mutates Core
state.

The vinyl-to-radio bridge now emits `radio_vinyl_broadcast` after the Core
morale/power decision has completed. It uses an existing radio static stream,
so the presentation path does not invent a new asset or gameplay consequence.

## Wiring and asset census

| Check | Result |
|---|---:|
| Catalog cues | 196 |
| Resolved cues | 190 |
| Fallback-only cues | 6 |
| Silent cues | 0 |
| Audio files under `assets/audio/` | 286 |
| Unique catalog resource references | 273 |
| Referenced resources present | 267 |
| Unreferenced assets | 19 |
| Formats | 194 WAV, 83 MP3, 9 OGG |

The six missing direct resources are intentionally fallback-backed generated
SFX (`ammo_press_stamp`, `weapon_clean_click`, `machine_overhaul_clank`,
`radio_decrypted_beep`, `dispute_argument_shout`, and
`mediation_accord_chime`). The existing catalog fallback contract resolves all
of them.

The 19 unreferenced files are retained legacy/alternate source assets. They
are not deleted by QA; future audio asset cleanup must classify them before
removal.

## Settings contract

The five user-facing mix controls are authoritative in
`UserSettingsData`/`UserSettingsStore`. `AudioSettings` remains the persisted
advanced per-domain bus store for compatibility. `ApplyUnifiedMix` projects
the unified mix into the playback layer before applying buses, preventing
SettingsPanel changes from being overwritten by stale advanced settings.

## Verification

```text
dotnet build Ashfall.csproj                         PASS
godot --headless --path . -- --audio-selftest      PASS — 619/619
```

The audio selftest also checks cue streams, fallback resolution, bridge
rebinding/disposal, anti-fatigue variations, machine tells, radio, vinyl
output cue registration, and settings persistence/recovery. Non-tree playback
guards remove the prior headless `AudioStreamPlayer.Play()` diagnostic.