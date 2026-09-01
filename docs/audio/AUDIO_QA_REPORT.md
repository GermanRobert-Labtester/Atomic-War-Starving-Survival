# ASHFALL Audio QA Report — Plan 07B Radio / VO Batch

**Date:** 2026-08-31
**Scope:** catalog-backed radio interaction cues, ten authored broadcast bindings, seven new/replacement voice clips, follow-on shelter/dosimeter wiring, disease-crisis lifecycle feedback, and the five-asset optional SFX finish pass.

## Result

PASS, with four retained legacy files documented below. Runtime content has no missing
audio resources or Godot import-sidecar orphans.

| Check | Result |
|---|---|
| Cue catalog | 74 cues; all 74 resource paths resolve; 0 missing/fallback-only cues |
| Broadcast bindings | 10 `audio_cue` values across `year_of_ash_radio.json` and `verdict_radio.json`; tooling test verifies every value is a registered catalog ID |
| New/replacement VO | 7 WAV files, PCM s16le, mono, 44.1 kHz, each with a Godot-generated `.import` sidecar |
| Loudness | New/replacement source clips measure −16.9 to −17.7 LUFS. Catalog trims place the ten routed clips at about −22.9 to −23.7 LUFS effective playback. |
| Asset pairing | `scripts/ci/asset-orphan-sweep.sh`: 0 missing or dangling sidecars |
| Catalog drift | `generate-audio-catalog.py --check`: pass, 74 cues |
| Godot audio gate | `--audio-selftest`: 266 pass, 0 fail; 74 resolved, 0 silent |

## Routed broadcasts

| Catalog | Broadcasts with authored VO |
|---|---|
| Year of Ash | `radio_142_carrier_discovery`, `radio_garrison_martial_edict`, `radio_cult_ash_sign_liturgy`, `radio_allotment_seed_appeal`, `radio_bunker_19_distress_call` |
| Verdict | `radio_verdict_meter_reads_1142`, `radio_verdict_off_count_assessed`, `radio_verdict_eden_was_here`, `radio_verdict_geophone_taps`, `radio_verdict_reckoning_call` |

## Retained legacy files

No audio was deleted or overwritten.

| File | Classification | Resolution |
|---|---|---|
| `assets/audio/radio/vo_kind_hatch.wav` | Effectively silent (−70 LUFS) | Retained; superseded by `vo_kind_hatch_relay.wav` in the registered cue. |
| `assets/audio/radio/vo_kind_parley.wav` | Effectively silent (−70 LUFS) | Retained; superseded by `vo_kind_parley_beacon.wav` in the registered cue. |
| `assets/audio/sfx/radiation_alert.wav` | Legacy duplicate | Retained; active catalog uses `sfx_radiation_alarm.mp3`. |
| `assets/audio/sfx/weather_alert.wav` | Legacy duplicate | Retained; active catalog uses `sfx_alarm_klaxon.mp3`. |

The two legacy alert WAVs and two silent VO WAVs are deliberately excluded from the
runtime cue graph. Their import sidecars remain valid, so the asset gate remains clean.

## Production provenance

`tools/generate_radio_vo_batch.py --write-runtime` uses local eSpeak NG and ffmpeg only:
it produces original short diegetic transmissions, applies high/low-pass receiver
treatment, compression, and −16 LUFS normalization, and refuses to overwrite existing
runtime files. A manual listening pass remains appropriate before final mastering or
casting replacement VO.

## Shelter and dosimeter wiring addendum

This follow-up is wiring-only: it adds no binary assets, so the radio loudness
measurements above remain the current source-file distribution. It makes existing live
assets use the dedicated routing already exposed by `AudioSettings`.

| Cue / surface | Classification | Runtime path |
|---|---|---|
| `shelter_generator` | LIVE loop | `PowerGridSystem` binding starts while generation and fuel are available; it stops when either condition ends or the host session disposes. Routed to `Generator` at −16 dB. |
| `shelter_ventilation` | LIVE loop | `StartingLevelSystem` binding starts the sealed-Holdfast circulation bed and stops it when that session is removed. Routed to `Ventilation` at −12 dB. |
| `shelter_air_filter` | LIVE threshold alert | Plays only when the air-filter state crosses into `airHazardWarning`; a hazardous loaded save receives one alert on bind. |
| `danger_alarm_klaxon` | LIVE infrastructure alert | Power-grid breaker trips and brownout summaries request the existing alert cue; its cooldown handles repeated grid reports. |
| `rad_geiger_burst` | LIVE exposure feedback | `RadiationSystem.OnDoseChanged` plays only for a dose increase, covering every system exposure path rather than only the manual exposure control. |
| `rad_geiger_loop` | ORPHAN_CUE, deferred | Continuous dosimetry needs an explicit exposure-end signal to stop safely. It remains registered and validated; dynamic intensity/looping belongs to the 7C ambience state machine. |

The global `AshfallUiHelpers.MakeButton` factory and the Holdfast trade terminal already
emit UI/trade cues, so this batch deliberately did not add duplicate click playback.
The audio selftest now exercises radiation-dose mapping, shelter-loop startup/shutdown,
and filter threshold alerting. The survivor-death batch below supersedes its prior
224/224 result.

## Survivor-death cue addendum

`SurvivorFateSystem.OnSurvivorFate` was already the Core's authoritative,
idempotent all-cause survivor-death event; it was not an actual Core event gap. The
Godot audio bridge now binds that event and requests `med_survivor_death` exactly once
per fate record. This covers needs, radiation, disease, combat, expedition, medical,
and scripted deaths without subscribing separately to each source or duplicating the
survivor-fate cascade.

The new `sfx_survivor_death.wav` is an original, non-vocal somber low-tone impact,
routed to the dedicated **Medical** bus at -6 dB with a three-second cue cooldown. It
provides an audible loss without implying a specific survivor voice or cause of death.

QA measurement: 1.80 s, mono PCM s16le at 44.1 kHz, -20.8 LUFS integrated and
-14.0 dBTP. It is neither silent nor clipped and is intentionally quieter than the
radio-voice material above; no normalization was applied. Godot generated its `.import`
sidecar, the asset orphan sweep reports zero source/sidecar mismatches, and the catalog
drift gate reports 74 cues in sync.

## Disease-crisis audio addendum

The disease bridge now gives player-facing lifecycle transitions distinct feedback on the
dedicated **Medical** bus:

| Core event | Cue | Intent |
|---|---|---|
| `OnInfection` | `med_heartbeat` | Immediate personal-health warning |
| `OnOutbreakDeclared` | `med_coughing` | Outbreak escalation |
| `OnQuarantineStarted` | `med_quarantine_seal` | Low descending seal/lock gesture |
| `OnQuarantineEnded`, `OnOutbreakContained`, recovered `OnOutcomeResolved` | `med_quarantine_clear` | Soft rising all-clear gesture |

Fatal disease outcomes deliberately do not request a second disease cue: they flow into
the existing `SurvivorFateSystem.OnSurvivorFate` path and play `med_survivor_death` once.
This keeps every cause of death on the same idempotent, campaign-authoritative route.

`sfx_med_quarantine_seal.wav` and `sfx_med_quarantine_clear.wav` are original procedural
WAVs generated by `tools/generate_disease_cue_batch.py`; the generator refuses to
overwrite an existing asset. Both are mono PCM s16le at 44.1 kHz and have Godot `.import`
sidecars. Measurements: seal — 0.85 s, -18.3 LUFS, -13.0 dBTP; clear — 0.62 s,
-22.2 LUFS, -18.0 dBTP. Neither clip is silent, clipped, nor outside the SFX-duration
sanity range. Their different source loudness is intentional and is further balanced by
catalog trims (-7 dB seal, -8 dB clear); no normalization was applied.

The self-test now creates an isolated `DiseaseSystem`, verifies the infection →
quarantine → clearance sequence, and proves disposal removes every disease subscription.
The current headless result is **266 pass, 0 fail**, with **74 resolved** cue resources
and no silent paths. `asset-orphan-sweep.sh` reports zero source/sidecar mismatches, and
the audio-file Git attribute is unset as required for plain binary tracking.

## Weather / danger differentiation addendum

Three catalog paths now use original, distinct assets rather than borrowing a cue from a
different semantic domain:

| Cue | New asset | Measurement |
|---|---|---|
| `weather_black_rain` | `sfx_weather_black_rain.wav` | 2.80 s, −26.7 LUFS, −14.9 dBTP |
| `weather_blizzard` | `sfx_weather_blizzard.wav` | 5.60 s, −24.3 LUFS, −7.3 dBTP |
| `danger_alarm_klaxon` | `sfx_danger_alarm_klaxon.wav` | 4.35 s, −17.8 LUFS, −14.0 dBTP |

All are mono PCM s16le at 44.1 kHz, neither silent nor clipped, and each has a
Godot-generated `.import` sidecar. `tools/generate_weather_danger_cue_batch.py` uses
local SoX only and refuses to overwrite a named output. Its per-file invocation permits
QA-led replacement of a failed newly generated candidate without touching other assets.
No corpus-wide normalization was applied.

## Optional SFX completion addendum

The five remaining optional differentiation assets are complete. The four transition
assets are live through `AudioEventBridge`; the surface loop is registered and guarded
by an explicit surface-listening API. They are original, procedural WAVs generated
locally by `tools/generate_optional_sfx_finish_batch.py`; the generator refuses to
overwrite a named runtime output.

| Surface | Cue / weather mapping | Asset | Measurement |
|---|---|---|---|
| Chronic radiation | `rad_alert_chronic` | `sfx_radiation_chronic_alarm.wav` | 1.90 s, −14.0 LUFS, −8.0 dBTP |
| EMP conditions | `EMPStorm`, `AshLightning` → `weather_emp_storm` | `sfx_weather_emp_storm.wav` | 1.65 s, −18.4 LUFS, −11.1 dBTP |
| Glass hazards | `GlassStorm`, `RadHail` → `weather_glass_storm` | `sfx_weather_glass_storm.wav` | 2.50 s, −16.9 LUFS, −7.7 dBTP |
| Corrosive precipitation | `AcidSnow`, `BloodRain` → `weather_corrosive_precipitation` | `sfx_weather_corrosive_precipitation.wav` | 2.40 s, −18.0 LUFS, −3.9 dBTP |
| Surface storms (prepared) | `amb_surface_storm` loop | `amb_surface_storm.wav` | 12.00 s, −23.3 LUFS, −7.3 dBTP |

All five are mono PCM s16le at 44.1 kHz and have Godot-generated `.import` sidecars.
The distinct chronic source replaces the former acute/chronic shared-alarm ambiguity.
`SurfaceAmbienceController` switches normal/storm surface loops from `WeatherSystem`
only after the host explicitly starts surface ambience; it never infers player location
from an expedition. Its lifecycle self-test covers initial start, storm/clear switching,
both loop stops, and unsubscribe-on-dispose. The current game flow is bunker-only and
does not yet request surface mode, so this loop remains deliberately inactive until a
real surface presentation owns that transition.
