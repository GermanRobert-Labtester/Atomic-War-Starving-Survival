# Plan 52 — The Sound of Scarcity: Ambience, Music, and Silence as State

> **Wave:** Continuity Wave 8 — *The Presented Game*
> **Depends on:** 17C (the six partial silence gaps), 20C (weather states), 23A/23B (power state),
> 31 (events as triggers), 42A (voice), 38B (seasons), 51C (where light ends and sound begins).
>
> **Theme:** 70+ cues on 12 buses, all routed, all resolving, selftest 245/245 — and the game is
> still quiet where it matters. **Surface ambience never starts in play** (only `StartBunkerAmbience`
> is called from `Main.GameFlow.cs:98`), **no ducking exists anywhere** in `src/Audio/` (so the
> documented 3–4-alert pile-up stands), only **14 of 22** weather kinds map to a cue, and
> `rad_geiger_loop` still can't stop because Core has no exposure-end signal. The one thing the game
> literally has as a weather state — `Silence`, `SilentSpring`, `FalseSpring` — is unplayed.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Catalog and buses are healthy | `src/Audio/AudioCueCatalog.cs` — 70+ cues, `Reg(...)` per cue; all 12 buses now carry cues (Generator, Ventilation, Medical, Surface wired in the 7B addendum); `AudioSelfTest` last recorded 245 pass / 70 cues resolved, 0 silent paths |
| 2 | **Surface ambience is never started by gameplay** | `PlayCue(AmbSurface)` exists only in `AudioManager.cs:305` (a convenience method); `grep -rn "AmbSurface" src/` outside `AudioManager`/`AudioCueCatalog`/`AudioSelfTest` → **nothing**. Only `_audio?.StartBunkerAmbience()` (`src/Main.GameFlow.cs:98`) and `StopAmbience()` (`:135`, `:514`) are called from game flow — so "outside" is silent |
| 3 | **No ducking or bus-volume control exists** | `grep -rn "Duck\|SetVolumeDb\|bus_volume" src/Audio/` → **0 hits** — `SILENCE_AUDIT.md` §8's pile-up scenarios (3–4 Alerts cues inside 5 s) remain exactly as written |
| 4 | Weather coverage is 14/22 | `grep -c "WeatherKind\." src/Audio/AudioEventBridge.cs` → 14 mappings; the audit's §4.1 gap list (Ashfall, AcidSnow, BioFog, BlackSnow, BloodRain, EMPStorm, GlassStorm, RadHail, **Silence**, FalseSpring, SilentSpring …) is the remaining set |
| 5 | Geiger loop is blocked on a Core gap | `AUDIO_QA_REPORT.md`: `rad_geiger_loop` = `ORPHAN_CUE, deferred … needs an explicit exposure-end signal`; Wave 1's 17C step 7 scheduled that event |
| 6 | Shelter loops are state-driven and correct | `src/Audio/ShelterAudioController.cs` binds generator/ventilation/air-filter to `PowerGridSystem`/atmosphere state — the **model** for weather/surface/season beds |
| 7 | Music identity is minimal | 3 music cues (`music_menu`, `music_gameplay`, plus the distinct `game_over` produced in 7B); no seasonal, tension, or loss variant; `SILENCE_AUDIT` §9 gap #4 remains partial |
| 8 | Voice exists for the world, not for people | 10+ radio VO references and ~15 VO assets produced (7B); survivor voice (Wave 6's 42A/42B) will need a delivery policy — spoken vs subtitled |
| 9 | Settings recovery is a manual doc | `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` — bus sliders and recovery verified by hand, not by gate |
| 10 | The fiction already names silence as a state | `WeatherKind` includes `Silence`, `SilentSpring`, `FalseSpring` — a nuclear-winter palette where *absence* of sound is authored content, currently unexpressed |

---

## Task 52A — Ambience as a state machine, not a call

**Goal:** one audio state machine driven by the same authorities the visuals use (place, weather,
season, power, occupancy), with defined transitions and no per-frame polling.

**Files:** `src/Audio/ShelterAudioController.cs` (extend into one controller),
`src/Audio/AudioManager.cs:300–310`, `src/Audio/AudioEventBridge.cs`, `src/Main.GameFlow.cs:98,135,514`,
`src/Host/WorldHostSession.cs` (weather), `src/Host/PowerGridHostSession.cs`, `SurvivorsHostSession`
(where survivors are), `RadiationSystem` (exposure-end), 20C's `WeatherEffects`, 38A's calendar,
`docs/audio/AMBIENCE_STATE_MACHINE.md`, `AudioSelfTest.cs`.

### Substeps

1. **Draw the state graph first** (one page): beds by *place* (bunker / surface / coast / transit /
   crowded shelter), modified by *weather*, *season*, *power state*, and *occupancy* — with allowed
   transitions and crossfade rules, because that is where games get audio mud.
2. **Extend `ShelterAudioController` into the single ambience owner** rather than adding a second
   controller — it already demonstrates correct lifecycle (start/stop on state, session replacement,
   shutdown), which is exactly the discipline `AmbSurface` lacks today.
3. **Wire surface ambience** from position (Wave 2's 20A position model): on-surface survivors get
   `amb_surface`; storm states crossfade to `amb_surface_storm`; returning indoors stops it. Verify
   start *and* stop with a gameplay-driven test, not just a selftest unit.
4. **Fill the 8 unmapped weather kinds** by family mapping (ash/acid/bio/blood/glass/hail/rad/thermal)
   onto existing beds with pitch/filter variation — no new production batch; and give `Silence` /
   `SilentSpring` / `FalseSpring` their authored treatment: **drop everything else**. Silence is
   content.
5. **Seasonal beds** once 38A exists: winter wind pressure, thaw drip, summer dust — a variant per
   season on the existing Surface/Ambience buses, not new buses.
6. **Add the missing Core signal** for `rad_geiger_loop`: an explicit exposure-end event
   (`RadiationSystem`), then rate the loop by dose-rate band (a loud meter is a readable meter) and
   stop it safely at the boundary Wave 5 flagged.
7. **Power-state audibility is already half done** — generator loop tracks fuel/generation; extend so
   *load shedding* is heard as the shelter changes (a room going quiet), and a brownout's klaxon obeys
   the ducking rules from 52B.
8. **Occupancy**: an empty shelter is quieter than a crowded one; there are schedules, sleep
   assignments, and duty rosters to read. Crossfade rather than stack.
9. **No polling**: uniforms/updates on state change (51C step 3), never `_Process`.
10. **Budget and device safety**: max concurrent voices per bus, and a hard cap; verify under the
    26C perf budget on the minimum-spec target.
11. **Tests**: state graph transitions (no double-start, no orphan loop), position-driven surface
    start/stop, weather family mapping for all 22, exposure-end stops the geiger loop, session-swap
    teardown (16C's rule), determinism-neutral (audio cannot change simulation — assert identical
    `SaveChecksum` muted).
12. **Docs**: `docs/audio/AMBIENCE_STATE_MACHINE.md` with the graph + the "silence is a cue" rule.
13. **Run the checklist** + `--audio-selftest`.

**DoD:** ambience follows the player's situation, and the quiet moments are authored.

---

## Task 52B — Mix discipline: ducking, priority, and the alert pile-up

**Goal:** make the mix legible under pressure — the documented 3–4-alert scenarios resolved by
policy, per-bus volume in settings, and a measured loudness floor across the whole catalog.

**Files:** `src/Audio/AudioManager.cs`, `src/Audio/AudioBusNames.cs`, `AudioCueCatalog.cs`,
`SILENCE_AUDIT.md` §5/§6/§8, `docs/audio/AUDIO_QA_REPORT.md`, `ashfall-audio-qa` skill,
`src/Settings/UserSettings.cs` (per-bus sliders), `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md`.

### Substeps

1. **Implement ducking** (absent today): an Alerts-class cue ducks Ambience/SFX by an authored amount
   for its duration, with a release ramp — the exact fix the silence audit prescribed and nobody
   wrote.
2. **Priority and concurrency caps per bus**: alerts deduplicate (same cue within a window plays
   once), and a storm + dose + klaxon stack collapses to the loudest single alert, as the audit's
   recommendation states.
3. **De-duplicate shared assets** (audit §5): `rad_contamination` vs `weather_black_rain`,
   `weather_alert` vs `danger_alarm_klaxon`, `shelter_pipe_clang` vs `day_transition` — different
   threats must not sound identical; produce/assign distinct assets for those four pairs.
4. **Loudness pass over the catalog**: normalise gain targets per bus so a -2 dB alert and a -14 dB
   ambience bed sit in one intentional mix; record the numbers in `AUDIO_QA_REPORT.md`.
5. **Per-bus volume in settings**: sliders for the 12 buses (or grouped), persisted via
   `UserSettingsStore`, with defaults that reproduce today's mix — then turn
   `AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` into an automated matrix (Wave 5's 39A philosophy).
6. **Alert semantics**: define what each alert means (dose, weather, structure, medical, danger) and
   enforce one sound family per semantic class so a player can react without reading text — and never
   two unrelated threats sharing one cue.
7. **Reduce-motion equivalent for sound**: a "reduce audio chaos" option (fewer simultaneous layers,
   gentler ducking) as an accessibility setting, not a difficulty change.
8. **Silence handling**: an explicit "all beds out" state for `Silence`-class weather, grief beats
   (41A), and the game-over sting — the loudest tool available and currently unused.
9. **Fail-safe audio**: device loss/disable mid-session must not spam or hard-mute gameplay-critical
   alerts; visual parity for every alert cue (37C captions/colour-independence).
10. **Measurement, not opinion**: log peak/duration of simultaneous cues during a seeded storm soak
    and assert the pile-up policy held — a mix rule that can't be observed can't be gated.
11. **Tests**: ducking engagement/release, dedup windows, per-bus cap, settings persistence and
    recovery, semantic-family uniqueness assertion (no two classes share a file), determinism-neutral
    check.
12. **Docs**: update `SILENCE_AUDIT.md` status column to reflect closure, so the next agent doesn't
    re-audit (Wave 3's 29B).
13. **Run the checklist** + `--audio-selftest` + `ashfall-audio-qa`.

**DoD:** under a storm, the mix tells the player what to act on; every bus is user-controllable.

---

## Task 52C — Score, sting, and the shape of a session

**Goal:** a small, authored musical identity that follows the game's own arcs — season, tension,
loss, and the ending — instead of a loop under everything.

**Files:** `assets/audio/music/`, `AudioCueCatalog` (music cues), `src/Audio/AudioManager.cs`,
`src/Main.GameFlow.cs` (state), `EpilogueMatrixRuntime`/ending path, 38A's calendar, 30A's war
tension, 41A's death pipeline, `docs/audio/MUSIC_PLAN.md`, `ashfall-audio-expansion-pack`.

### Substeps

1. **Set a ceiling**: 3–5 additional musical pieces total (menu, shelter, surface/pressure, loss,
   ending), each with a documented trigger. More is wallpaper.
2. **Music follows state, not screens**: tension input from 30A's `activeWarTension` and the
   player's own obligation deadlines (38C); pressure input from brownout/storm/dose; and *no* music
   during grief beats — let 52B step 8's silence do the work.
3. **Seasonal variation by transposition/instrumentation** of existing beds rather than new
   composition per season (a winter voice for the same theme reads as a year passing).
4. **Death and memorial get a cue, not a sting**: `game_over.ogg` is distinct now (7B); a single
   survivor's death should be handled by subtraction (beds drop, one held note) — the restrained
   option is the correct one for this game's tone.
5. **Ending music keyed to the permutation family** (19A/34A): three outcomes, not thirty-two
   bespoke tracks — group the matrix rows by tone and map each group to one piece.
6. **Voice policy**: decide which VO exists, and which lines stay subtitled (42B's captions + the
   existing radio VO set), so scope is bounded and accessibility is satisfied either way.
7. **Crossfade and re-entry rules** with 52A's state machine — one owner for what is playing, so a
   music change can't fight a weather bed.
8. **Loop integrity**: seamless-loop verification per asset (no clicks), documented in the QA report,
   since most produced beds are procedural.
9. **Loudness continuity** between music and beds (52B step 4) so raising ambience doesn't bury the
   score.
10. **Provenance/licensing** recorded per asset (Wave 8's 50A step 9), consistent with
    `docs/AI_DISCLOSURE.md` / `HUMAN_AUTHORSHIP.md`.
11. **Tests**: state→music selection determinism, no music during declared silence states,
    seamless-loop assertions (sample-boundary check), settings recovery.
12. **Docs**: `docs/audio/MUSIC_PLAN.md` — the pieces, their triggers, their cost, their owner.
13. **Run the checklist** + `--audio-selftest` + asset gate.

**DoD:** a session has a musical shape, and the moments without music are deliberate.

---

## Cross-Task Dependencies

```
51C (light/weather grade) ◄──► 52A step 9 (one weather expression, agreed once)
20C (WeatherEffects) ──► 52A step 4      38A (calendar) ──► 52A step 5, 52C step 3
23A/23B (power state) ──► 52A step 7      30A (war tension) ──► 52C step 2, 38C deadlines
20A (position, exposure-end) ──► 52A steps 3,6   41A (death/grief) ──► 52C step 4
31A (event kinds) ──► every trigger        37C (settings/reduce-motion, captions) ──► 52B
   52A (state machine) ──► 52B (mix policy) ──► 52C (score/sting)
```

**Execution order:** 52A → 52B → 52C, and inside Wave 8: 50A → 51A → 52A → 51B → 50C → 52B → 51C →
52C → 53 → 54 — audio needs the same state authorities the visuals do; doing it later means
re-wiring triggers twice.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --audio-selftest                 # cues, buses, loops
7. bash scripts/ci/generate-audio-catalog.py --check             # catalog in sync
8. bash scripts/ci/asset-orphan-sweep.sh && bash scripts/ci/lfs-health-check.sh
9. storm-soak mix log: pile-up policy held (52B step 10)
10. ashfall-audio-qa + docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md (automated)
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Audio assets | Settings/QA | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 52A | 2–3 + 1 Core event | 0 (mapping) | 0 | 10–14 | Medium | LOW (audio can't change sim — assert it) |
| 52B | 2 | 4 de-dup assets | 1 | 8–12 | Medium | LOW–MED |
| 52C | 1 | 3–5 pieces | 0 | 5–8 | Medium (content-dependent) | LOW |

**Guardrails:** no new buses, no new weather or mechanic to justify a sound, no music that talks over
informational cues, no alert without a visual equivalent, no audio path that can alter simulation
state (assert checksum identity muted), and no composition commissioned before 50A proves what art
already exists and is unmapped.
