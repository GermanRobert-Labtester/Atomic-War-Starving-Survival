# ASHFALL Audio QA Report

Date: 2026-09-03
Audit anchor: c09c3e67a6e88920690767899b586ab85fecb84c
Scope: read-only census, cue closure, wiring, import/load behavior, loudness, lifecycle, and test adequacy

## Verdict

Audio is not release-ready in the observed working snapshot.

The independent media scan decoded all 150 source files, and the Godot audio self-test reports 490/490 with 145/145 cues resolved. Those green results conceal a critical listener-amplification defect, three live silent expansion events, an invalid WAV fallback implementation, and widespread source-level peak/DC problems. The canonical Godot import scan also crashes before completing.

The repository changed concurrently during this audit. No audio file, import setting, bus, or runtime implementation was modified by the audit.

## Inventory

| Metric | Result |
|---|---:|
| Source files | 150 |
| WAV / MP3 / OGG | 82 / 63 / 5 |
| PCM WAV format | 82/82 s16le |
| Total bytes | 16,499,025 |
| Total duration | 501.532 seconds |
| Mono / stereo | 94 / 56 |
| 44.1 / 24 / 22.05 kHz | 136 / 8 / 6 |
| Hardcoded registrations | 144 |
| JSON-loaded registrations | 1 |
| Runtime cues | 145 |
| Missing registered source paths | 0 |
| Uncataloged source files | 6 |
| Independent decode failures | 0 |
| Exact binary duplicates | 0 |

Uncataloged sources:

- assets/audio/radio/vo_kind_hatch.wav
- assets/audio/radio/vo_kind_parley.wav
- assets/audio/sfx/radiation_alert.wav
- assets/audio/sfx/weather_alert.wav
- assets/audio/sfx/sfx_hazard_toxic_sizzle.mp3
- assets/audio/sfx/sfx_interrogation_slam.mp3

The first four appear superseded. The final two correspond to live expansion events and should be registered or deliberately retired.

## Findings

### AQ-01 — CRITICAL: expansion event callbacks grow every frame

AudioManager calls RefreshDomainBindings during _Process and then calls ExpansionAudioBridge.SubscribeAll at src/Audio/AudioManager.cs:308.

SubscribeAll adds anonymous listeners for desperation, mutation, chemical warfare, and railway events at src/Audio/ExpansionAudioBridge.cs:29. It has no provider identity guard and Dispose removes no listeners. After N refreshes, a single event can invoke N playback callbacks. The subscribed Core objects can also retain a stale AudioManager after teardown.

No test directly covers ExpansionAudioBridge idempotence or disposal.

### AQ-02 — HIGH: WAV fallback does not parse WAV containers

AudioManager.LoadDirectStream at src/Audio/AudioManager.cs:603 constructs AudioStreamWav and assigns the entire RIFF file to AudioStreamWav.Data.

That property expects decoded PCM payload bytes and accompanying format metadata. A complete WAV buffer must be parsed through the engine loader. The current fallback can misinterpret RIFF headers as samples and use incorrect rate/channel settings.

### AQ-03 — HIGH: three live expansion events are silent

| Cue constant | Event source | Source asset | Runtime registration | Outcome |
|---|---|---:|---:|---|
| action_interrogation_slam | DesperationSystem.OnTabooBroken | Yes | No | Silent |
| bio_mutation_pulse | MutationSystem.OnMutationAcquired | Yes | JSON | Plays |
| hazard_toxic_sizzle | ChemWarfareSystem.OnHazardDeployed | Yes | No | Silent |
| train_screech_crash | RailwaySystem.OnDerailment | No | No | Silent |

### AQ-04 — HIGH: source-level clipping and inter-sample overs

Across all 150 files:

- 69 measure above 0 dBTP.
- 81 reach at least -0.1 dBFS sample peak.
- 21 have absolute DC offset at or above 0.01.
- 65 of 125 files with finite integrated readings fall outside the broad -27 to -15 LUFS range.

All 52 untracked WAV additions hit exactly 0.000 dBFS sample peak; 41 exceed 0 dBTP. Bus attenuation reduces playback level but does not repair clipped sources, inter-sample overs, or DC bias.

Short transients that reported negative-infinite integrated loudness were below the EBU gating window; they were not treated as silent.

### AQ-05 — HIGH: eight tracked alert/weather files have severe DC bias

| File | Integrated loudness | True peak | Mean/DC |
|---|---:|---:|---:|
| sfx_weather_surface_storm.wav | -7.33 LUFS | +5.00 dBTP | +0.2951 |
| sfx_weather_blizzard.wav | -8.06 LUFS | +4.90 dBTP | +0.2999 |
| sfx_radiation_chronic_alarm.wav | -6.49 LUFS | +4.59 dBTP | +0.2915 |
| sfx_weather_corrosive_precipitation.wav | -7.50 LUFS | +4.00 dBTP | +0.2963 |
| sfx_weather_black_rain.wav | -7.45 LUFS | +2.93 dBTP | +0.2648 |
| sfx_weather_glass_storm.wav | -7.57 LUFS | +2.93 dBTP | +0.2921 |
| sfx_danger_alarm_klaxon.wav | -8.05 LUFS | +2.60 dBTP | +0.3288 |
| sfx_weather_emp_storm.wav | -10.95 LUFS | +1.88 dBTP | +0.3532 |

These files were already tracked at the audit snapshot. The earlier report values did not match the current binaries and must not be used as mastering evidence.

Additional extremes include sfx_trauma_tinnitus_ring.wav at about -1.33 LUFS / 0 dBTP and radio_static_hiss.mp3 at about -39.74 LUFS.

### AQ-06 — MEDIUM: self-test proves presence more often than loadability

AudioSelfTest treats File.Exists as resolution at src/Audio/AudioSelfTest.cs:53. Its key-asset probes do the same at line 244. Loop tests fall back to LoadDirectStream and then check only whether the returned object is an accepted stream type.

The self-test passed 490/490 while emitting 22 unique ResourceLoader cache failures for loop assets. The fallback masked those failures. The import cache could not be regenerated because the full Godot import scan crashed.

### AQ-07 — MEDIUM: generated catalog check is date-volatile

python3 scripts/ci/generate-audio-catalog.py --check failed solely because the generated Last Verified date changed from 2026-09-02 to 2026-09-03. The generator also counts only hardcoded Reg calls, so its 144-cue view omits bio_mutation_pulse loaded from audio_cues.json.

### AQ-08 — LOW: twenty hardcoded cues lack static consumer evidence

No reference outside catalog/self-test code was found for twenty registered IDs, including ui_tab_change, rad_contamination, four distant-combat cues, structural collapse, airlock purge, three shelter facility loops, action_water_pour, med_infirmary, trauma_cabin_fever, and several danger cues.

This is a removal-candidate list, not proof of runtime orphaning: reflection, string dispatch, or future data can hide static reachability.

## Bus and routing

AudioSelfTest found all 12 expected buses. Runtime cue metadata maps sources to named buses and applies cue/bus gain trims. No missing bus was observed. The principal routing risk is duplicated event delivery, not absent bus configuration.

## Verification

| Check | Result |
|---|---|
| Independent ffmpeg decode of all 150 sources | PASS |
| Godot audio self-test | PASS — 490/490 |
| Cue source-path closure | PASS — 145/145 |
| Godot ResourceLoader loop loads | DEGRADED — 22 unique cache failures, fallback used |
| Audio catalog generator check | FAIL — volatile date only |
| godot --headless --path . --import | FAIL — exit 134 |

## Required remediation

1. Make ExpansionAudioBridge subscriptions idempotent and fully unsubscribe on provider change/disposal.
2. Replace manual WAV Data assignment with a container-aware WAV load path.
3. Register or intentionally remove the three silent expansion cues; create the missing train asset if the event remains audible.
4. Re-master the 52 new WAVs with headroom, then remove DC and overs from the tracked alert/weather group.
5. Make self-tests validate actual decode/load, duration, sample rate, channels, and non-empty samples.
6. Make catalog generation deterministic and merge hardcoded plus JSON-loaded cue sources.
7. Re-run the import and audio gates from a stable clean snapshot.

No normalization or automatic repair was performed.
