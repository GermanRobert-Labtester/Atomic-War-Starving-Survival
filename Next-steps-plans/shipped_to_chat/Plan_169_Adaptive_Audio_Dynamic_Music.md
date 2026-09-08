# Plan 169 — Audio Accessibility & Mix Legibility

## Goal

Make critical audio understandable and controllable for every player: clear bus controls,
radio-over-ambience ducking, visual equivalents for essential cues, and safe mix diagnostics.
This plan does not create an adaptive music state machine.

## Scope boundary

Plan 07 Task 7C owns reactive ambience/music states, crossfades, and the required music assets.
This plan consumes those host-side states only. It must not create `AdaptiveAudioSystem`,
`MusicDirector`, `music_layers.json`, a Core audio save section, or seeded audio variation.
Audio remains presentation-only and is never read by simulation.

## Task 1 — Audio accessibility contract

1. Inventory cues whose gameplay meaning must be available without hearing: alarms, low-power,
   geiger escalation, raid start, medical emergency, expedition return, and radio interruption.
2. Define a host-side accessibility mapping from cue category to concise visual notification.
   The mapping uses existing localized strings and never becomes a second event bus.
3. Add independent volume controls for music, ambience, UI, alerts, radio/voice, plus a dynamic
   range preset. Store preferences in the existing user-settings path, not campaign saves.

## Task 2 — Mix control and intelligibility

1. Add side-chain ducking rules so radio, emergency alerts, and essential dialogue remain audible
   over ambience/music.
2. Add cooldown/coalescing for repeated alerts and a debug readout of active buses, ducking, and
   the last emitted accessible notification.
3. Define room for quiet: mute/pause behavior, a reduced-stimulation preset, and a testable
   guarantee that repeated loops do not stack indefinitely.
4. Consume Plan 07's resolved state/cue identifiers; do not choose music moods or author tracks.

## Task 3 — Validation

1. Extend the audio self-test with cue-to-notification coverage, valid bus routing, and ducking
   priority assertions.
2. Exercise settings migration and default values without storing audio state in campaign saves.
3. Run a manual listen/readability pass and document only actionable mix findings.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

## Definition of Done

- Every critical cue has a visual equivalent and a valid bus route.
- Radio and alerts remain intelligible over Plan 07's ambience/music.
- Preferences use the existing settings store; no campaign audio state or new Core audio system is added.
