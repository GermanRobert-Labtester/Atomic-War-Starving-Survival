# Plan 07 — Audio Production Wave: Voice, Cues & Silence Mapping

> **Theme:** Sound. The game has 118+ radio broadcasts, 23 echoes, 30 audio logs — and **zero
> of them reference an audio file**. 49 cues are registered in `AudioCueCatalog.cs` across 12
> buses. This plan turns a text-only soundscape into a produced one.
>
> **Key evidence:** `radio.json` (50) + `year_of_ash_radio.json` (50) + `verdict_radio.json`
> (13) + `radio_distress_signals.json` (5) = 118 broadcasts, **0 with audio refs**;
> `assets/audio/radio/` holds ~12 VO files; `docs/audio/AUDIO_CUE_CATALOG.md` is the
> drift-gated authority (`scripts/ci/generate-audio-catalog.py --check`).

---

## Task 7A — Audio cue gap map & silence audit

**Goal:** Before producing anything, build the authoritative map of *where sound should exist
but doesn't* — every panel action, event, weather state, and combat beat without a cue.

**Files:** `docs/audio/AUDIO_CUE_CATALOG.md` (extend), `src/Audio/AudioCueCatalog.cs`,
`src/Audio/AudioEventBridge.cs`; output report to `docs/audio/SILENCE_AUDIT.md`.

**Substeps:**
1. Extract all 49 registered cues + their call sites from `AudioEventBridge.cs` and panels.
2. Enumerate trigger surfaces: 22 `WeatherKind` states, combat events (jam, hit, lane breach), medical events (code, death), economy events (deal, default), expedition events (departure, breakdown, return).
3. Diff triggers vs cues → produce the silence matrix (event × has-cue?).
4. Identify reused cues (e.g. one klaxon covering 3 alert kinds) and flag where differentiation matters for player readability.
5. Check bus routing sanity: alerts that must cut through vs ambience that shouldn't.
6. Verify all cue resource paths resolve on disk (report broken/missing assets).
7. Run the existing audio selftest (`AudioSelfTest.cs`) to establish a green baseline.
8. Check volume trims/cooldowns for stacking bugs (alarm + geiger + storm simultaneously).
9. Produce `SILENCE_AUDIT.md`: ranked list of the top 20 silence gaps by player-impact.
10. Gate: catalog generator `--check` still passes after doc edits.

**Next steps:** this audit is the spec for 7B and 7C; it also feeds the `ashfall-audio-qa` skill.

---

## Task 7B — Priority cue & VO production batch

**Goal:** Produce the top-20 missing cues from the 7A audit and wire first-pass VO to the
10 highest-value radio broadcasts, using the audio-expansion-pack pipeline.

**Files:** `assets/audio/{sfx,ambience,radio,music}/` (new assets, LFS policy: `*.wav/*.mp3/*.ogg`
stay plain binary per `.gitattributes`), `src/Audio/AudioCueCatalog.cs`,
`src/Audio/AudioEventBridge.cs`, radio JSON files (add `audio_cue` fields).

**Substeps:**
1. From the 7A audit, lock the top-20 cue list (likely: distinct weather-state beds, combat jam/hit set, medical code, trade confirm/refuse, expedition depart/return).
2. Generate/source each asset per the `ashfall-audio-expansion-pack` skill conventions (naming `sfx_*`/`amb_*`/`vo_*`, loudness normalization target).
3. Register each cue in `AudioCueCatalog.cs` with bus, loop, trim, cooldown — matching the existing table format exactly.
4. Wire triggers in `AudioEventBridge.cs` (and panel code for UI cues).
5. Pick the 10 story-critical broadcasts (faction-war declarations, distress calls, verdict transmissions) and add an `audio_cue` field to their JSON entries.
6. Confirm the radio loader tolerates the new field (add binding test) before authoring VO.
7. Produce/ingest the 10 VO files under `assets/audio/radio/` (existing `vo_*` naming).
8. Normalize loudness across radio VO so the tuner doesn't jump in volume.
9. Run `ashfall-audio-qa`: orphan detection (files with no cue, cues with no file), format policy, loudness.
10. Run audio selftest + full `dotnet build` 0 warnings + catalog generator `--check` green.

**Next steps:** remaining ~98 broadcasts get VO in waves; dynamic music intensity (7C).

---

## Task 7C — Dynamic ambience & music state layer

**Goal:** Move from two ambient loops + two music tracks to a *reactive* soundscape driven by
live game state (weather, shelter power, population health, threat level).

**Files:** `src/Audio/AudioManager.cs` (host-side mixing logic only — all rules engine-agnostic
if any scoring is needed), `assets/audio/ambience/`, `assets/audio/music/`, `AudioCueCatalog.cs`.

**Substeps:**
1. Define 4–6 ambience states: bunker-calm, bunker-strained (low power/sick), surface-storm, surface-clear, combat-nearby — each a loop or layered stem.
2. Define crossfade rules (state → target loop + fade time); keep the state machine in a tiny Core-evaluable form if rules get complex, else pure host.
3. Hook state inputs: `WeatherSystem.Current`, `PowerGridSystem` load tier, `SickListSystem` census, combat-active flag.
4. Produce 3–4 new ambience beds and 2 music intensity stems (tension / mourning).
5. Wire the state evaluator into `AudioManager` with hysteresis (no flapping on borderline values).
6. Ensure determinism is untouched — audio state is presentation-only, never saved, never read by simulation.
7. Add cues for state transitions (storm rising sting).
8. Extend `AudioSelfTest` to assert each state resolves to an existing cue.
9. Run `ashfall-audio-qa` + catalog `--check` + full build.
10. Manual listen pass (the one step automation can't do) — document subjective notes in `docs/audio/`.

**Next steps:** sidechain ducking for radio-over-ambience; per-room reverb zones for the shelter interior view.
