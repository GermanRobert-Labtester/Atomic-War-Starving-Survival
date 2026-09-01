# SFX Combat Batch 01 Implementation Log

Date: 2026-08-30

Status: generated candidates; awaiting human audition, take selection, trimming, and mastering.

## Scope

This batch contains 12 non-vocal firearm SFX concepts: one report and one
mechanical action for each of the six authoritative named guns. No dialogue,
intelligible speech, music, or singing was requested.

| Weapon | Construction | Report identity | Mechanical identity |
|---|---|---|---|
| CZ 75 Pistol | Factory | compact 9x19mm crack and steel-slide snap | precise magazine and slide action |
| Pipe Rifle | DIY | uneven .357 bark with loose pipe resonance | hand-loaded case and crude latch |
| Scrap Shotgun | DIY | broad double-barrel blast with sheet-metal ring | stiff break action and two-shell reload |
| Held-Bolt Rifle | Factory | dry .308 crack with long outdoor tail | heavy machined bolt and brass ejection |
| Assault Rifle | Factory | bright three-shot 5.56 burst | tight magazine and charging-handle action |
| Light Machine Gun | Factory | deep five-shot 7.62 burst | feed-cover, belt, and charging-handle sequence |

The DIY weapons deliberately use loose, irregular, hand-built textures rather
than the consistent machined action of the factory weapons.

## ElevenLabs generation

- Flow: [ASHFALL Combat SFX Batch 01 — Named Firearms](https://elevenlabs.io/app/flows/nEnfsTbX7fy0azjyS6Ot)
- Flow ID: `nEnfsTbX7fy0azjyS6Ot`
- Model: `eleven_text_to_sound_v2`
- Prompt influence: `0.7`
- Concepts: 12
- Takes per concept: 4
- Completed candidates: 48/48
- Generation failures: 0
- Preflight estimate: 1,364 credits / $0.248
- Generation-result total: 413.292 credits / $0.075144

The service automatically retried seven concurrency-limited jobs. No manual
generation retry was issued, so the batch was not charged twice by this work.

## Objective candidate QA

Candidates were downloaded only to
`/tmp/ashfall_elevenlabs_sfx_candidates/combat_01` for non-destructive QA.
That temporary location is not a game dependency.

- Decode: 48/48 pass
- Encoding: MP3, 44.1 kHz, stereo
- Duration: 48/48 within 0.04 seconds of target
- True peak above 0 dBTP: 38/48
- True peak at or above +1 dBTP: 21/48
- Abnormally quiet candidates: two CZ 75 report takes
  (`PbgJbE5pgkZWEsee4Fr7`, `ux22fMkdLZ7UAieC1Ffx`)
- Detected silence ranges from 0% to 78.8% of file duration; this includes
  expected gaps and tails but requires take-specific trimming judgment.

No candidate is release-ready as generated. The selected take for each concept
must be auditioned for semantic correctness and absence of accidental voices,
trimmed without cutting its transient or tail, mastered to the project's SFX
target with safe true peak, and exported as WAV or OGG before acceptance.

## Repository state

- The 12 specifications and weapon-identity validation live in
  `tools/generate_elevenlabs_sfx.py` under batch `combat_01`.
- Candidate provenance is recorded in
  `assets/audio/sfx/sfx_manifest.json`.
- Runtime promotions: 0
- Audio cue registrations: 0
- Gameplay trigger changes: 0
- Existing shipped audio overwritten: 0
- Godot launches: 0

## Next acceptance gate

1. Audition four takes per concept in the ElevenLabs flow.
2. Select exactly one take per concept or reject/regenerate only that concept.
3. Trim and master the selected source; do not normalize all candidates blindly.
4. Export a runtime WAV or OGG.
5. Accept it through the non-overwriting pipeline so hashes and provenance are recorded.
6. Register cue IDs and wire weapon-specific report/action triggers only after all
   accepted files resolve locally.

## Verification limitations

Godot was deliberately not launched because of the reported workstation crash.
The repository also has pre-existing, unrelated compile/test blockers in the
user-modified worktree; this candidate-only batch does not alter those files.

Verification on 2026-08-30:

- SFX specification/manifest validation: PASS (`40` specs, `6` weapon identities)
- Candidate decode: PASS (`48/48`)
- Candidate duration: PASS (`48/48`, maximum absolute error `0.04s`)
- Core test-project build: PASS (`0` warnings, `0` errors)
- Core tests: FAIL (`4,980/4,981` pass) because the pre-existing modified
  `CatalogIntegrityValidator.cs` has three catch-policy findings
- Godot host build: FAIL with two pre-existing `.Ok` tuple-member errors in
  `src/Host/HostCli.PanelTests.cs` at lines 829 and 831
- Data-integrity headless test: SKIPPED (Godot intentionally not launched)
- Bridge headless test: SKIPPED (Godot intentionally not launched)
- Diff whitespace validation: PASS
