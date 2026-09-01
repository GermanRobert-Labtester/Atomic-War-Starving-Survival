# SFX Batch 0 — Safety and Runtime Foundation

Date: 2026-08-30
Status: implementation complete; canonical verification partially blocked by a pre-existing host compile error

## Scope

- No ElevenLabs generation and no Creator-account credit spend.
- Preserve all 44 shipped audio files.
- Replace the all-or-nothing legacy generator with explicit plan/generate/accept gates.
- Record provenance only when a reviewed, mastered WAV/OGG candidate is accepted.
- Keep simultaneous looped cues independent by stable cue ID.
- Bind radiation and weather events without duplicate or stale subscriptions.
- Establish a unique acoustic identity for every authoritative named gun.

## Implemented

- `tools/generate_elevenlabs_sfx.py`
  - requires explicit `--id` or `--all` selection;
  - stays dry unless direct fallback receives `--execute`;
  - writes direct-API candidates only under `/tmp`;
  - rejects automatic runtime overwrite and unmastered MP3 acceptance;
  - stores no API key and never writes it to the manifest;
  - validates the weapon identity list against `combat_catalog.json` and `items.json`.
- `assets/audio/sfx/sfx_manifest.json`
  - schema version 2 policy and empty accepted-generation ledger;
  - OAuth MCP recorded as the primary transport;
  - no-voice, review-required, non-overwrite, and weapon-distinction policies.
- `src/Audio/AudioManager.cs`
  - one keyed `AudioStreamPlayer` per loop;
  - WAV, OGG, and MP3 loop flags;
  - independent stop-by-cue and stop-by-bus;
  - settings and event subscriptions released on exit.
- `src/Audio/AudioEventBridge.cs` and `src/Main.Audio.cs`
  - live Core radiation/weather discovery;
  - reference-safe rebinding and deterministic unsubscribe/dispose.
- `src/Audio/AudioSelfTest.cs`
  - supported loop-format checks;
  - specific weather/radiation mapping, duplicate-binding, replacement-session, and dispose checks.

## Named-weapon acoustic contract

| Weapon | Build | Required signature |
|---|---|---|
| CZ 75 Pistol | Factory | Compact sharp crack, steel slide cycle |
| Pipe Rifle | DIY | Uneven bark, loose tube resonance, crude latch after-rattle |
| Scrap Shotgun | DIY | Broad dirty boom, flexing sheet metal, loose chamber clatter |
| Held-Bolt Rifle | Factory | Powerful dry crack, long tail, heavy machined bolt |
| Assault Rifle | Factory | Bright three-round cadence, compact gas action |
| Light Machine Gun | Factory | Deep five-round burst, heavy reciprocating action |

New authoritative guns now make pipeline validation fail until they receive a unique identity. DIY and factory construction classes cannot be omitted.

## Verification record

- Python compile and pipeline validation: PASS (`28` SFX specs, `6` named-weapon identities).
- Missing selection rejection: PASS.
- MP3 runtime-accept rejection: PASS.
- Dry-run generation: PASS; no network request and no candidate/runtime write.
- Isolated bridge behavior harness against the real Core systems: PASS (bind, no duplicate bind, replacement detach, radiation mapping, dispose).
- Shipped-audio aggregate SHA-256 after implementation: `6b928a146311cb1a8f1ea42289a7b126ca27472f5b5d6a05c4f0aa8c1c19416f`.
- Core test-project build: PASS, 0 warnings/errors.
- Core tests: `4,980/4,981` PASS; blocked only by three pre-existing catch-policy findings in the user-modified `CatalogIntegrityValidator.cs:470/475/480`.
- Godot host compile: BLOCKED by the pre-existing `HostCli.PanelTests.cs:829/831` tuple member `Ok` errors; the Batch 0 sources introduce no additional compiler diagnostics.

Godot headless checks are intentionally deferred while the host cannot compile; no editor process is launched.
