# ASHFALL Audio Generation & Sound Design — Next 20 Tasks Roadmap

> **Authoritative Target**: Godot 4.7+ (.NET/C#) · Zero Unity Dependencies · Engine-Agnostic Core (`Assets/Ashfall.Core/`)
> **Standard Format**: 16-bit 44.1 kHz PCM WAV · Loudness Normalized (-14 to -18 LUFS ambience/loops, -3 to -6 dBFS SFX peaks) · Godot `.import` Sidecars
> **Enforcement**: `asset-orphan-sweep.sh` (0 orphans) · `generate-audio-catalog.py --check` · `godot --headless --path . -- --audio-selftest`

---

## Executive Summary

Following the successful implementation and verification of Phases 1 through 5 (145 registered cues, 91 verified audio assets, and 490 automated self-test checks passing), this document defines the **next 20 comprehensive audio generation and sound design tasks** for ASHFALL. Each task is systematically structured into **exactly 15 technical, sequential substeps** (300 substeps total), spanning procedural synthesis, DSP bus architecture, domain event wiring, UI ergonomics, and headless CI verification.

---

## Master Task Index

1. [Task 1: Dynamic Weather Audio Generation (Extreme Wasteland Meteorological Events)](#task-1-dynamic-weather-audio-generation-extreme-wasteland-meteorological-events)
2. [Task 2: Shelter Power Grid, Substation & Brownout Audio Suite](#task-2-shelter-power-grid-substation--brownout-audio-suite)
3. [Task 3: Subterranean Hydrology, Pipe Acoustics & Drainage Networks](#task-3-subterranean-hydrology-pipe-acoustics--drainage-networks)
4. [Task 4: Survivor Physiological State & Infirmary Foley](#task-4-survivor-physiological-state--infirmary-foley)
5. [Task 5: Mental Breakdown, Delirium & Cabin Fever Auditory Hallucinations](#task-5-mental-breakdown-delirium--cabin-fever-auditory-hallucinations)
6. [Task 6: Heavy Ballistics: Machine Guns, Rifles & Anti-Armor Foley](#task-6-heavy-ballistics-machine-guns-rifles--anti-armor-foley)
7. [Task 7: Improvised Post-War Weaponry & Trap Audio Synthesis](#task-7-improvised-post-war-weaponry--trap-audio-synthesis)
8. [Task 8: Melee Combat, Bludgeons & Blades Soundscape](#task-8-melee-combat-bludgeons--blades-soundscape)
9. [Task 9: Wasteland Wildlife, Fauna & Biome Audio Generation](#task-9-wasteland-wildlife-fauna--biome-audio-generation)
10. [Task 10: Wasteland Biome Ambient Soundscapes (Exterior Exploration)](#task-10-wasteland-biome-ambient-soundscapes-exterior-exploration)
11. [Task 11: Expanded Vehicle Fleet Acoustics (Specialized Transport & Locomotion)](#task-11-expanded-vehicle-fleet-acoustics-specialized-transport--locomotion)
12. [Task 12: Workshop Machining, Metalworking & Scavenger Crafting Foley](#task-12-workshop-machining-metalworking--scavenger-crafting-foley)
13. [Task 13: Nuclear Bunker Airlock & Decontamination Chamber Sequence](#task-13-nuclear-bunker-airlock--decontamination-chamber-sequence)
14. [Task 14: Diegetic Shortwave Radio Intercepts & Faction Frequencies](#task-14-diegetic-shortwave-radio-intercepts--faction-frequencies)
15. [Task 15: Cassette Tape Narrative Audiologs & Voice Synthesis Suite](#task-15-cassette-tape-narrative-audiologs--voice-synthesis-suite)
16. [Task 16: Deep Sub-Bunker Infrastructure & Hydroponics Greenhouse Acoustics](#task-16-deep-sub-bunker-infrastructure--hydroponics-greenhouse-acoustics)
17. [Task 17: Radiation Dosimetry & Geiger Multi-Stage Sonification](#task-17-radiation-dosimetry--geiger-multi-stage-sonification)
18. [Task 18: Barter, Trade & Economy Tactile Sound Effects](#task-18-barter-trade--economy-tactile-sound-effects)
19. [Task 19: Exploration Hazards, Structural Collapses & Scavenging Foley](#task-19-exploration-hazards-structural-collapses--scavenging-foley)
20. [Task 20: Cinematic Game Flow, Death Stings & Victory Fanfares](#task-20-cinematic-game-flow-death-stings--victory-fanfares)

---

### Task 1: Dynamic Weather Audio Generation (Extreme Wasteland Meteorological Events)

* **Goal**: Synthesize and integrate high-fidelity acoustic layers for ASHFALL's distinct post-nuclear weather conditions (Acid Rain, Glass Storms, EMP Lightning, and Black Rain).
* **Substeps**:
  1. Acoustically model Acid Rain / Corrosive Precipitation using high-frequency filtered Gaussian white noise (2.5 kHz to 7.0 kHz) modulated by a caustic chemical fizzing envelope with randomized droplet impact intervals (15–40 ms).
  2. Acoustically model Glass Storms by synthesizing supersonic shear winds combined with high-frequency crystalline micro-clattering (8.0 kHz to 13.5 kHz) using frequency-modulated sine bursts simulating airborne silica shards.
  3. Acoustically model EMP Lightning & Thunder by synthesizing a low-frequency concussive shockwave (30 Hz to 90 Hz exponential sine sweep) preceded by electromagnetic static crackle and ionized air snap.
  4. Acoustically model Black Rain by generating heavy, dense liquid impact transients hitting mud, rubble, and corrugated sheet metal, low-pass filtered at 1.8 kHz with an underlying rumbling thunderbed.
  5. Author the zero-dependency procedural synthesis script `tools/audio_gen/generate_weather_extremes.py` implementing these mathematical acoustics.
  6. Synthesize normalized 16-bit 44.1 kHz PCM mono WAV assets: `assets/audio/ambience/amb_weather_acid_rain_loop.wav`, `assets/audio/ambience/amb_weather_glass_storm_loop.wav`, `assets/audio/sfx/sfx_weather_emp_thunder.wav`, `assets/audio/ambience/amb_weather_black_rain_loop.wav`.
  7. Generate valid Godot `.import` sidecars with `loop=true` for environmental loops and `loop=false` for one-shot lightning strikes.
  8. Perform automated loudness inspection with `sox -n stat`, enforcing -16.0 LUFS for continuous precipitation loops and -3.0 dBFS true peak for EMP thunder.
  9. Add constant declarations to `src/Audio/AudioCueCatalog.cs`: `WeatherAcidRainLoop`, `WeatherGlassStormLoop`, `WeatherEmpThunder`, `WeatherBlackRainLoop`.
  10. Register cues with `AudioBusNames.Ambience` and `AudioBusNames.Surface`, configuring anti-fatigue micro-pitch jitter (±3%) and volume jitter (±1.2 dB).
  11. Implement dynamic bus send filtering in `SurfaceAmbienceController.cs` linking storm severity to real-time low-pass filter cutoff frequencies.
  12. Wire `WeatherSystem.OnWeatherChanged` in `AudioEventBridge.cs` to cross-fade between normal surface ambience and the active weather phenomenon.
  13. Add unit test assertions in `src/Audio/AudioSelfTest.cs` validating cue resolution, loop flags, and physical asset presence.
  14. Execute `bash scripts/ci/asset-orphan-sweep.sh` to confirm zero orphan audio sources and zero orphan `.import` sidecars.
  15. Run `python3 scripts/ci/generate-audio-catalog.py --check` and verify clean exit code 0.

---

### Task 2: Shelter Power Grid, Substation & Brownout Audio Suite

* **Goal**: Provide tangible acoustic presence to the bunker's electrical infrastructure, brownouts, load fluctuations, and generator strain.
* **Substeps**:
  1. Synthesize 50 Hz and 60 Hz electrical mains hum with strong odd-order harmonic overtones (150 Hz, 250 Hz, 350 Hz, 450 Hz) and subtle phase drift to model substation transformers.
  2. Synthesize heavy industrial circuit breaker mechanical relay drops (spring-loaded snap transient at 1.2 kHz followed by copper contact slap).
  3. Synthesize high-voltage capacitor bank charging whine (exponential upward pitch ramp starting at 120 Hz and cresting at 14.5 kHz over 4.2 seconds).
  4. Synthesize violent high-voltage electrical arcing and short-circuit sparks (bursts of high-passed white noise modulated by random pulse trains).
  5. Author the zero-dependency Python generator `tools/audio_gen/generate_shelter_power_grid.py`.
  6. Render WAV files to `assets/audio/sfx/`: `sfx_power_transformer_hum.wav`, `sfx_power_relay_trip.wav`, `sfx_power_capacitor_charge.wav`, `sfx_power_arc_spark.wav`.
  7. Auto-generate standard Godot `.import` sidecars with appropriate loop settings (`loop=true` for transformer hum).
  8. Enforce loudness normalization via `sox`: -18.0 LUFS for ambient transformer hum and -2.5 dBFS peak for circuit breaker snaps.
  9. Register cues in `AudioCueCatalog.cs` (`PowerTransformerHum`, `PowerRelayTrip`, `PowerCapacitorCharge`, `PowerArcSpark`) routed to `AudioBusNames.Generator` and `AudioBusNames.Alerts`.
  10. Enhance `ShelterAudioController.cs` to evaluate shelter electrical demand versus capacity, dynamically modulating transformer hum pitch and amplitude.
  11. Connect shelter brownout and blackout events in `AudioEventBridge.cs` to trigger sudden relay trips and electrical arcing.
  12. Add helper methods to `AudioManager.cs` (`StartTransformerHum()`, `StopTransformerHum()`, `PlayRelayTrip()`, `PlayArcSpark()`).
  13. Register new test cases in `src/Audio/AudioSelfTest.cs` verifying electrical lifecycle states and cue playback.
  14. Execute `asset-orphan-sweep.sh` to ensure strict sidecar compliance.
  15. Run `godot --headless --path . -- --audio-selftest` and verify 100% pass across all checks.

---

### Task 3: Subterranean Hydrology, Pipe Acoustics & Drainage Networks

* **Goal**: Acoustically ground the bunker's life-support fluid networks, plumbing stress, water hammer shocks, and sump pumps.
* **Substeps**:
  1. Synthesize water hammer shock impact: a resonant fluid-in-pipe cavitation thump (fundamental at 75 Hz) followed by high-frequency iron pipe ring (1.6 kHz) with exponential damping.
  2. Synthesize high-pressure steam safety valve purge: violent turbulent steam hiss (shaped bandpass noise centered at 2.2 kHz) with an instantaneous 10 ms attack and 3.5-second exhaust decay.
  3. Synthesize subterranean electric sump pump cycle: rhythmic 2-stroke mechanical intake chug (1.6 Hz tempo) with churning wet impeller fluid friction.
  4. Synthesize deep cistern cavern acoustics: isolated water droplet impacts exciting a long, diffuse 3.2-second low-frequency reverberation chamber.
  5. Author Python synthesis generator `tools/audio_gen/generate_hydrology_sfx.py`.
  6. Export 16-bit 44.1 kHz WAV assets: `assets/audio/sfx/sfx_pipe_water_hammer.wav`, `assets/audio/sfx/sfx_pipe_steam_vent.wav`, `assets/audio/sfx/sfx_sump_pump_loop.wav`, `assets/audio/ambience/amb_cistern_reverb_loop.wav`.
  7. Generate corresponding `.import` sidecars ensuring `loop=true` for pump and cistern ambience.
  8. Normalize audio levels: -17.0 LUFS for cistern ambience and -4.0 dBFS peak for water hammer shocks.
  9. Add cue constants to `AudioCueCatalog.cs`: `ShelterPipeWaterHammer`, `ShelterSteamVent`, `ShelterSumpPump`, `ShelterCisternAmbience`.
  10. Assign cues to `AudioBusNames.Ambience` and `AudioBusNames.Sfx` with appropriate volume offsets and distance falloff curves.
  11. Wire water filtration degradation thresholds (<30% filter life) in `AudioEventBridge.cs` to trigger water hammer and steam purge events.
  12. Add plumbing control methods to `AudioManager.cs` (`PlayWaterHammer()`, `PlaySteamVent()`, `StartSumpPump()`, `StopSumpPump()`).
  13. Add hydrology assertions to `AudioSelfTest.cs` ensuring clean lifecycle start, stop, and disposal behavior.
  14. Re-generate `docs/audio/AUDIO_CUE_CATALOG.md` and verify sync.
  15. Run `dotnet test Ashfall.Core.Tests` to verify no simulation regressions.

---

### Task 4: Survivor Physiological State & Infirmary Foley

* **Goal**: Deliver tactile, visceral audio cues for survivor trauma, critical dyspnea, bone-setting, suturing, and medical interventions.
* **Substeps**:
  1. Synthesize agonal respiration / dyspnea: rhythmic strained inhalation and wheezing exhalation (formant-filtered pink noise at 650 Hz, 1400 Hz, and 2800 Hz).
  2. Synthesize bone fracture manipulation and setting: multi-transient wet fibrous crunch (50–120 ms) followed by hollow calcium snapping impact at 450 Hz.
  3. Synthesize IV fluid drip chamber cadence: steady 1.0 Hz micro-transient droplet impact inside an acoustic plastic cylindrical resonator.
  4. Synthesize surgical suture thread friction: delicate, tactile fibrous pull and knot-cinching transients with high-frequency cloth/skin friction (3.0 kHz to 6.5 kHz).
  5. Synthesize manual defibrillator / cardiac stimulator charge and discharge: rising capacitor whine followed by heavy damped muscular chest thump.
  6. Author zero-dependency generator `tools/audio_gen/generate_infirmary_foley.py`.
  7. Render WAV files to `assets/audio/sfx/`: `sfx_med_agonal_breathing.wav`, `sfx_med_bone_fracture_snap.wav`, `sfx_med_iv_drip_loop.wav`, `sfx_med_suture_knot.wav`, `sfx_med_defib_thump.wav`.
  8. Auto-generate Godot `.import` sidecars for all 5 assets.
  9. Enforce loudness norms via `sox`: -16.0 LUFS for IV drip and -3.0 dBFS peak for bone setting.
  10. Register cues in `AudioCueCatalog.cs` routed to `AudioBusNames.Medical`.
  11. Wire `SurvivorInjurySystem` and medical triage interventions in `AudioEventBridge.cs`.
  12. Add medical convenience methods in `AudioManager.cs` (`StartAgonalBreathing()`, `StopAgonalBreathing()`, `PlayBoneSnap()`, `PlaySutureKnot()`).
  13. Connect `InfirmaryPanel.cs` treatment action buttons to fire matching medical intervention audio cues.
  14. Update `AudioSelfTest.cs` with infirmary cue verification test cases.
  15. Run full CI validation: `scripts/ci/asset-orphan-sweep.sh` and `godot --headless --path . -- --audio-selftest`.

---

### Task 5: Mental Breakdown, Delirium & Cabin Fever Auditory Hallucinations

* **Goal**: Sonify severe survivor psychological distress, delirium, and claustrophobic isolation through psychoacoustic auditory hallucinations.
* **Substeps**:
  1. Acoustically design phantom bunker door knocks: three muffled wooden/steel knocking transients positioned deep in stereo space with artificial bass decay.
  2. Synthesize auditory pareidolia in ventilation fans: harmonic whispered syllables ("who's there...", "open it...") phase-locked into the 400 Hz fan hum.
  3. Synthesize irregular clock ticking: mechanical escapement click sequence that subtly fluctuates in tempo (from 60 BPM to 140 BPM) before abruptly vanishing.
  4. Synthesize claustrophobic intracranial heartbeat: heavily low-passed (sub-85 Hz) visceral double-thump simulating vascular ear pressure during panic attacks.
  5. Author generator script `tools/audio_gen/generate_delirium_hallucinations.py`.
  6. Render WAV files to `assets/audio/sfx/`: `sfx_trauma_phantom_knock.wav`, `sfx_trauma_fan_whisper.wav`, `sfx_trauma_erratic_clock.wav`, `sfx_trauma_intracranial_pulse.wav`.
  7. Generate valid Godot `.import` sidecars with one-shot import settings.
  8. Calibrate subtle loudness levels (-18.0 LUFS to -14.0 LUFS) so hallucinations blend uncannily underneath environmental audio.
  9. Add cue constants to `AudioCueCatalog.cs`: `TraumaPhantomKnock`, `TraumaFanWhisper`, `TraumaErraticClock`, `TraumaIntracranialPulse`.
  10. Map cues to `AudioBusNames.Alerts` with wide randomized stereo panning (-0.8 to +0.8) to simulate spatial disorientation.
  11. Connect survivor stress threshold events (>80 stress) in `AudioEventBridge.cs` to randomly schedule auditory hallucination cues.
  12. Add cooldown and dedup logic in `AudioStateCoordinator.cs` to prevent fatigue and preserve eerie psychological impact.
  13. Add unit test assertions in `AudioSelfTest.cs` verifying cue resolution and stress event trigger bounds.
  14. Re-generate `docs/audio/AUDIO_CUE_CATALOG.md` and verify sync.
  15. Run headless audio self-test and verify zero ObjectDB leaks.

---

### Task 6: Heavy Ballistics: Machine Guns, Rifles & Anti-Armor Foley

* **Goal**: Expand tactical combat acoustics with authentic mechanical actions, heavy anti-materiel blasts, multi-round bursts, and magazine handling.
* **Substeps**:
  1. Synthesize .50 BMG heavy rifle blast: massive 42 Hz sub-bass shockwave, supersonic bullet crack (6.5 kHz), and wide outdoor valley reverberation tail.
  2. Synthesize Squad Automatic Weapon (SAW) 5-round burst: rapid cyclic rate (750 RPM) with overlapping muzzle blasts and mechanical operating rod chatter.
  3. Synthesize battle rifle 3-round burst: three tight muzzle cracks with casing ejections and bolt reciprocations.
  4. Synthesize stamped steel magazine ejection: magazine release latch click followed by spring expansion ping and hollow sheet-metal drop.
  5. Synthesize heavy charging handle rack: two-stage steel bolt retraction slide, spring compression, and front locking lug slam.
  6. Author synthesis script `tools/audio_gen/generate_heavy_ballistics.py`.
  7. Export WAV files to `assets/audio/sfx/`: `sfx_weapon_50bmg_fire.wav`, `sfx_weapon_saw_burst.wav`, `sfx_weapon_rifle_burst.wav`, `sfx_weapon_mag_eject.wav`, `sfx_weapon_bolt_rack.wav`.
  8. Generate Godot `.import` sidecars with unlooped one-shot presets.
  9. Apply dynamic range limiting at -1.0 dBFS true peak while preserving sharp initial transient impact.
  10. Register cues in `AudioCueCatalog.cs` (`CombatFire50Bmg`, `CombatSawBurst`, `CombatRifleBurst`, `CombatMagEject`, `CombatBoltRack`).
  11. Configure anti-fatigue micro-pitch jitter (0.96 to 1.04) and volume jitter (±1.5 dB) for all automatic burst cues.
  12. Enhance `AudioEventBridge.OnCombatEvent` to parse heavy firearm weapon IDs (`weapon_anti_materiel`, `weapon_lmg_545`, `weapon_battle_rifle`).
  13. Add unit tests in `Ashfall.Core.Tests` verifying weapon instance burst state propagation.
  14. Add self-test coverage in `src/Audio/AudioSelfTest.cs` exercising heavy ballistics playback.
  15. Run `dotnet test Ashfall.Core.Tests` and confirm zero failed tests.

---

### Task 7: Improvised Post-War Weaponry & Trap Audio Synthesis

* **Goal**: Deliver crunchy, unpredictable soundscapes for scrap-built wasteland weaponry, pipe zip-guns, nailers, snares, and shrapnel bombs.
* **Substeps**:
  1. Synthesize scrap pipe zip-gun shot: asymmetrical low-pressure black powder propellant boom, irregular pipe casing resonance, and scattered pellet spread.
  2. Synthesize pneumatic construction nailer: high-pressure air blast puff (1.5 kHz bandpass) followed by heavy steel piston impact and metal fastener penetration.
  3. Synthesize tripwire trigger snare: high-tensile wire twang (sine sweep 800 Hz to 240 Hz) and spring pin release click.
  4. Synthesize serrated bear trap jaw snap: heavy dual coiled springs releasing violently, ending in an explosive steel tooth collision at 1.8 kHz.
  5. Synthesize improvised shrapnel pipe bomb blast: deafening explosive concussion, immediate ear-ringing flare, and rain of rusted nails and scrap debris falling.
  6. Author Python procedural generator `tools/audio_gen/generate_improvised_weapons.py`.
  7. Render WAV assets to `assets/audio/sfx/`: `sfx_weapon_zip_gun.wav`, `sfx_weapon_nailer_shot.wav`, `sfx_trap_tripwire_snap.wav`, `sfx_trap_beartrap_snap.wav`, `sfx_weapon_pipe_bomb_detonate.wav`.
  8. Generate Godot `.import` sidecars for all 5 sound files.
  9. Calibrate dynamic levels using `sox -n stat`: -1.5 dBFS peak for detonations, -4.0 dBFS for trap snaps.
  10. Register cues in `AudioCueCatalog.cs` under `AudioBusNames.Sfx` and `AudioBusNames.Alerts`.
  11. Wire expedition hazard encounters and trap triggers in `AudioEventBridge.cs`.
  12. Add playback methods to `AudioManager.cs` (`PlayZipGun()`, `PlayNailer()`, `PlayTripwire()`, `PlayBearTrap()`, `PlayPipeBomb()`).
  13. Add cue validation tests in `AudioSelfTest.cs`.
  14. Verify zero orphan files via `bash scripts/ci/asset-orphan-sweep.sh`.
  15. Verify documentation parity with `python3 scripts/ci/generate-audio-catalog.py --check`.

---

### Task 8: Melee Combat, Bludgeons & Blades Soundscape

* **Goal**: Implement crushing, tactile foley for close-quarters wasteland combat, bludgeons, knives, entrenching tools, and parries.
* **Substeps**:
  1. Synthesize sledgehammer crushing strike: massive low-frequency kinetic impact (60 Hz thud) with masonry/bone pulverizing crumble.
  2. Synthesize serrated combat knife slash: sharp cutting transient with high-frequency cloth tear and swift metallic air whoosh.
  3. Synthesize trench shovel blunt strike: hollow stamped-steel ringing thwack upon heavy impact.
  4. Synthesize rusted crowbar leverage and strain: deep ductile iron groaning friction under heavy mechanical leverage.
  5. Synthesize blade parry / iron block: instantaneous high-pitch metallic deflection ping (3.2 kHz harmonic ring) with short decay.
  6. Author procedural synthesis script `tools/audio_gen/generate_melee_combat.py`.
  7. Render WAV files to `assets/audio/sfx/`: `sfx_melee_sledge_smash.wav`, `sfx_melee_knife_slash.wav`, `sfx_melee_shovel_strike.wav`, `sfx_melee_crowbar_groan.wav`, `sfx_melee_parry_ring.wav`.
  8. Generate Godot `.import` sidecars for each asset.
  9. Enforce loudness normalization: -3.0 dBFS peak for crushing impacts, -5.0 dBFS for slashes and parries.
  10. Add cue declarations to `AudioCueCatalog.cs` (`CombatMeleeSledge`, `CombatMeleeKnife`, `CombatMeleeShovel`, `CombatMeleeCrowbar`, `CombatMeleeParry`).
  11. Configure dynamic pitch jitter (0.92 to 1.08) in `AudioCueCatalog.cs` for organic variation during consecutive melee strikes.
  12. Wire melee weapon attacks and counter-attacks in `AudioEventBridge.OnCombatEvent`.
  13. Connect melee actions in `CombatDetailPanel.cs` to trigger corresponding tactile audio cues.
  14. Update `AudioSelfTest.cs` with melee cue coverage and playback checks.
  15. Run full test matrix (`dotnet test Ashfall.Core.Tests` and `--audio-selftest`).

---

### Task 9: Wasteland Wildlife, Fauna & Biome Audio Generation

* **Goal**: Populate exterior expeditions with eerie, mutated post-nuclear fauna calls, subterranean pests, and distant apex predators.
* **Substeps**:
  1. Synthesize subterranean mole rat chittering: rapid frequency-modulated clicks (1.8 kHz to 3.4 kHz) combined with gritty dirt-scratching transients.
  2. Synthesize two-headed irradiated raven caw: dual-pitch discordant guttural cry with rasping vocal fry and upper-harmonic distortion.
  3. Synthesize distant feral dog pack baying: mournful howling chorus with pitch-glides modulated by atmospheric wind filtering and long distance pre-delay.
  4. Synthesize radioactive insect swarm drone: granular high-density saw-wave swarm drone oscillating between 240 Hz and 520 Hz.
  5. Synthesize wasteland apex predator guttural snarl: deep sub-vocal chest rumble (80 Hz to 180 Hz) paired with wet saliva and nasal snarl.
  6. Author synthesis script `tools/audio_gen/generate_wasteland_fauna.py`.
  7. Export WAV files into `assets/audio/sfx/` and `assets/audio/ambience/`: `sfx_fauna_mole_rat.wav`, `sfx_fauna_two_headed_raven.wav`, `amb_fauna_dog_pack_distant.wav`, `amb_fauna_insect_swarm_loop.wav`, `sfx_fauna_predator_snarl.wav`.
  8. Generate Godot `.import` sidecars with `loop=true` for the insect swarm loop.
  9. Calibrate audio levels: -18.0 LUFS for ambient howls/swarms, -6.0 dBFS peak for close-proximity animal snarls.
  10. Register cues in `AudioCueCatalog.cs` under `AudioBusNames.Ambience` and `AudioBusNames.Sfx`.
  11. Wire `WildlifeMigrationSystem` and sector encounter updates in `AudioEventBridge.cs`.
  12. Add fauna triggering APIs in `AudioManager.cs` (`PlayRavenCaw()`, `PlayPredatorSnarl()`, `StartInsectSwarm()`, `StopInsectSwarm()`).
  13. Add distance-attenuated playback logic for exterior hostile entity proximity in Godot host sessions.
  14. Add test assertions to `AudioSelfTest.cs` verifying fauna cue resolution and loop transitions.
  15. Run `python3 scripts/ci/generate-audio-catalog.py --check` and verify clean sync.

---

### Task 10: Wasteland Biome Ambient Soundscapes (Exterior Exploration)

* **Goal**: Establish rich, distinct atmospheric audio beds for the primary wasteland biomes: Scorched Craters, Highway Girders, Irradiated Bogs, Metro Tunnels, and Dead Pine Forests.
* **Substeps**:
  1. Synthesize Scorched Crater thermal wind: sub-bass thermal updraft rumble (35 Hz to 90 Hz) with intermittent fine ash swirl gusts.
  2. Synthesize Abandoned Highway Girder whistler: eerie tonal whistling created by high-velocity wind passing through exposed rusted steel lattice beams (centered at 720 Hz and 1140 Hz).
  3. Synthesize Irradiated Marsh / Bog bubbling: viscous, toxic mud bubbles bursting intermittently with low-frequency methane gurgles.
  4. Synthesize Collapsed Metro Tunnel ambience: cavernous concrete acoustics featuring distant echoing water drops and intermittent distant subway rail creaks.
  5. Synthesize Petrified Dead Pine Forest ambience: dry timber trunk friction groans and brittle branch snaps under freezing winds.
  6. Author procedural synthesis script `tools/audio_gen/generate_biome_ambience.py`.
  7. Export seamless WAV loops into `assets/audio/ambience/`: `amb_biome_scorched_crater.wav`, `amb_biome_highway_girder.wav`, `amb_biome_irradiated_marsh.wav`, `amb_biome_metro_tunnel.wav`, `amb_biome_dead_forest.wav`.
  8. Generate `.import` sidecars with `loop=true` and lossless audio presets.
  9. Enforce strict loudness normalization across all biome loops at exactly -18.0 LUFS to prevent abrupt volume shifts during zone travel.
  10. Register cues in `AudioCueCatalog.cs` routed to `AudioBusNames.Surface`.
  11. Wire `ExpeditionHostSession` sector updates in `AudioEventBridge.cs` to cross-fade between biome ambient tracks upon entering new map sectors.
  12. Extend `SurfaceAmbienceController.cs` to blend biome-specific background layers with prevailing global weather phenomena.
  13. Add test coverage in `src/Audio/AudioSelfTest.cs` asserting all 5 biome loops resolve and start/stop cleanly.
  14. Execute `bash scripts/ci/asset-orphan-sweep.sh` to guarantee 0 orphan sources and 0 orphan sidecars.
  15. Run headless Godot self-tests to ensure zero memory or RID leaks during biome cross-fades.

---

### Task 11: Expanded Vehicle Fleet Acoustics (Specialized Transport & Locomotion)

* **Goal**: Implement authentic acoustic identities for ASHFALL's advanced vehicles (Steam Halftrack, Electric Scout Quad, Armored Draisine Railcar, and Armored Bus).
* **Substeps**:
  1. Synthesize Steam Halftrack locomotion: low-RPM steam boiler exhaust chuffs (3.2 Hz tempo), rhythmic caterpillar track steel clatter, and boiler safety valve hiss.
  2. Synthesize Electric Scout Quad: high-frequency brushless electric inverter motor whine (smooth pitch ramp 400 Hz to 2.8 kHz) with aggressive all-terrain tire gravel churn.
  3. Synthesize Armored Draisine Railcar: heavy steel conical wheels screeched against pitted steel rails, coupled with rhythmic joint clatter ("click-clack" at 2.4 Hz).
  4. Synthesize Manual Handcar: dual-stroke wooden pumping lever squeak, chain drive rattle, and cast-iron wheel rotation.
  5. Synthesize Heavy Armored Bus Air-Brakes: pneumatic high-pressure reservoir discharge blast (1.8-second pressurized exhaust burst).
  6. Author synthesis script `tools/audio_gen/generate_vehicle_fleet.py`.
  7. Render WAV files to `assets/audio/sfx/`: `sfx_vehicle_engine_halftrack.wav`, `sfx_vehicle_engine_electric_quad.wav`, `sfx_vehicle_railcar_loop.wav`, `sfx_vehicle_handcar_loop.wav`, `sfx_vehicle_bus_airbrake.wav`.
  8. Generate `.import` sidecars with `loop=true` for continuous engine/wheel loops and `loop=false` for airbrake releases.
  9. Normalize engine loop loudness to -12.0 LUFS and airbrakes to -3.5 dBFS peak.
  10. Register cues in `AudioCueCatalog.cs` under `AudioBusNames.Sfx` and `AudioBusNames.Alerts`.
  11. Expand `AudioEventBridge.ResolveVehicleEngineCue` to evaluate vehicle definitions from `vehicles.json` (`steam_halftrack`, `electric_quad`, `draisine_railcar`).
  12. Add vehicle lifecycle methods in `AudioManager.cs` ensuring mutual exclusivity when switching or stopping expedition vehicles.
  13. Add self-test assertions in `AudioSelfTest.cs` verifying clean start, stop, and breakdown transitions for each vehicle type.
  14. Re-generate `docs/audio/AUDIO_CUE_CATALOG.md` and verify sync.
  15. Run `dotnet test Ashfall.Core.Tests` and confirm zero regressions in expedition mechanics.

---

### Task 12: Workshop Machining, Metalworking & Scavenger Crafting Foley

* **Goal**: Provide immersive industrial foley for crafting, gunsmithing, salvaging, and metal fabrication tasks in the bunker workshop.
* **Substeps**:
  1. Synthesize oxyacetylene welding torch: high-pressure gas hiss with turbulent combustion roar and occasional pop transients.
  2. Synthesize high-RPM angle grinder: aggressive abrasive grinding disc whining (5.5 kHz to 9.0 kHz) against hardened steel plate with shower-of-sparks sizzle.
  3. Synthesize hydraulic scrap metal press: low-frequency electric hydraulic pump hum, hydraulic valve click, and crushing steel crumple.
  4. Synthesize blacksmith anvil forging hammer: clear high-harmonic steel bell ping (2.4 kHz) with deep iron anvil base resonance.
  5. Synthesize electric soldering iron: delicate resin flux boiling crackle and light wire tinning fizz.
  6. Author generator script `tools/audio_gen/generate_workshop_crafting.py`.
  7. Render WAV assets to `assets/audio/sfx/`: `sfx_craft_torch_loop.wav`, `sfx_craft_angle_grinder.wav`, `sfx_craft_hydraulic_press.wav`, `sfx_craft_anvil_strike.wav`, `sfx_craft_solder_sizzle.wav`.
  8. Generate `.import` sidecars with appropriate looping parameters.
  9. Normalize loudness levels: -14.0 LUFS for sustained tool loops, -4.0 dBFS peak for anvil strikes.
  10. Register cues in `AudioCueCatalog.cs` routed to `AudioBusNames.Sfx`.
  11. Connect `WorkshopPanel.cs` and `CraftingPanel.cs` item assembly progress bars to active tool sound loops.
  12. Add crafting convenience methods in `AudioManager.cs` (`StartTorchLoop()`, `StopTorchLoop()`, `PlayGrinder()`, `PlayAnvilStrike()`).
  13. Add test cases in `src/Audio/AudioSelfTest.cs` verifying crafting tool cue resolution and loop termination.
  14. Execute `bash scripts/ci/asset-orphan-sweep.sh` to confirm 0 orphan files.
  15. Run `godot --headless --path . -- --audio-selftest`.

---

### Task 13: Nuclear Bunker Airlock & Decontamination Chamber Sequence

* **Goal**: Deliver a dramatic, multi-stage acoustic sequence for expedition departures and returns through the bunker airlock.
* **Substeps**:
  1. Synthesize heavy vault door locking wheel: heavy brass dog mechanisms clicking sequentially as a central handwheel rotates.
  2. Synthesize pneumatic seal inflation: 3.5-second pressurized air hiss filling the circumferential silicone rubber gaskets.
  3. Synthesize high-velocity decontamination chemical deluge: high-pressure liquid jets cascading down onto metal floor grates.
  4. Synthesize chamber floor drainage suction: vortex air/fluid suction gurgle as chemical wash drains into waste containment.
  5. Synthesize pressure equalization blast: sudden concussive pneumatic air rush with audible pressure differential thump.
  6. Author procedural script `tools/audio_gen/generate_airlock_decon.py`.
  7. Render WAV assets to `assets/audio/sfx/`: `sfx_airlock_wheel_spin.wav`, `sfx_airlock_pneumatic_seal.wav`, `sfx_airlock_decon_deluge.wav`, `sfx_airlock_drain_suction.wav`, `sfx_airlock_equalize_blast.wav`.
  8. Auto-generate Godot `.import` sidecars with one-shot import presets.
  9. Enforce dynamic range: -2.5 dBFS peak for the equalization blast, -5.0 dBFS for the deluge spray.
  10. Register cues in `AudioCueCatalog.cs` (`AirlockWheelSpin`, `AirlockPneumaticSeal`, `AirlockDeconDeluge`, `AirlockDrainSuction`, `AirlockEqualizeBlast`).
  11. Wire expedition dispatch and return triggers in `Main.Expeditions.cs` and `AudioEventBridge.cs`.
  12. Implement sequence coordinator in `AudioManager.cs` (`PlayAirlockCycle()`) that plays the 5 stages with proper acoustic timing.
  13. Add unit tests in `AudioSelfTest.cs` exercising airlock sequence step triggers and cue resolution.
  14. Re-generate `docs/audio/AUDIO_CUE_CATALOG.md` and verify sync.
  15. Run headless Godot self-test to verify zero ObjectDB leaks during airlock sequencing.

---

### Task 14: Diegetic Shortwave Radio Intercepts & Faction Frequencies

* **Goal**: Expand the shortwave radio receiver with diegetic encrypted faction broadcasts, CB convoy chatter, propaganda loops, and radioteletype.
* **Substeps**:
  1. Synthesize Enclave High-Frequency Encrypted Burst: fast Frequency Shift Keying (FSK) digital audio packets at 1200 baud with ionospheric flutter.
  2. Synthesize Merchant Convoy CB Radio: analog squelch click, carrier hiss, and dynamic microphone proximity breath pops.
  3. Synthesize Doomsday Preacher Megaphone Loop: heavily saturated, band-limited (400 Hz to 2.2 kHz) distorted vocal sermon with natural tape slapback.
  4. Synthesize Radioteletype (RTTY) Baudot tones: continuous alternating mark/space tones (2125 Hz and 2295 Hz) running at 45.45 baud.
  5. Synthesize Extremely Low Frequency (ELF) Submarine Broadcast: deep sub-audible 76 Hz carrier pulse with ominous phase-shifted modulation.
  6. Author Python generator `tools/audio_gen/generate_faction_radio_signals.py`.
  7. Export WAV files into `assets/audio/radio/`: `radio_enclave_burst.wav`, `radio_cb_convoy_squelch.wav`, `radio_preacher_propaganda_loop.wav`, `radio_rtty_telemetry_loop.wav`, `radio_elf_submarine_pulse.wav`.
  8. Generate Godot `.import` sidecars with loop flags for telemetry and propaganda loops.
  9. Normalize all signals to match the Radio bus DSP transceiver filter profile.
  10. Register cues in `AudioCueCatalog.cs` routed to `AudioBusNames.Voice` and `AudioBusNames.Radio`.
  11. Connect signal classes from `Assets/StreamingAssets/Data/radio_intercepts.json` to matching cues in `RadioPanel.cs`.
  12. Add playback methods in `AudioManager.cs` (`PlayEnclaveBurst()`, `PlayCbSquelch()`, `StartPreacherLoop()`, `StartRttyLoop()`).
  13. Update `RadioPanel.cs` channel tuner buttons to play the new faction signals when dialed into corresponding frequencies.
  14. Add test assertions in `AudioSelfTest.cs` verifying radio cue resolution and loop termination on panel close.
  15. Run `python3 scripts/ci/generate-audio-catalog.py --check` and verify clean exit code 0.

---

### Task 15: Cassette Tape Narrative Audiologs & Voice Synthesis Suite

* **Goal**: Synthesize and integrate immersive pre-war and post-strike audiolog cassettes scattered throughout the wasteland.
* **Substeps**:
  1. Synthesize Pre-War Bunker Commander Farewell: weary baritone voice recording processed with magnetic tape saturation, 60 Hz head hum, and wow/flutter.
  2. Synthesize Missile Silo Technician Launch Countdown: panicked vocal urgency recorded through a tactical throat microphone with klaxons in the background.
  3. Synthesize Field Doctor Triage Journal: clinical spoken narrative punctuated by respirators and intermittent ECG telemetry beeps.
  4. Synthesize Lost Child's Lullaby Fragment: faint, fragile humming recording degraded with severe analog magnetic print-through and flutter.
  5. Synthesize Subterranean Mutiny Audio Artifact: violent shouting, physical scuffle foley, gunshots, and microphone hitting a concrete floor.
  6. Author Python narrative audio generator `tools/audio_gen/generate_narrative_tapes.py`.
  7. Render WAV files to `assets/audio/radio/`: `vo_log_commander_farewell.wav`, `vo_log_silo_countdown.wav`, `vo_log_doctor_triage.wav`, `vo_log_lullaby_fragment.wav`, `vo_log_mutiny_scuffle.wav`.
  8. Auto-generate Godot `.import` sidecars with one-shot voice presets.
  9. Calibrate speech intelligibility against tape hiss floor using `sox` spectral analysis.
  10. Register cues in `AudioCueCatalog.cs` (`AudioLogCommanderFarewell`, `AudioLogSiloCountdown`, `AudioLogDoctorTriage`, `AudioLogLullaby`, `AudioLogMutinyScuffle`).
  11. Wire audio log discovery events in `NarrativeHostSession` and `JournalDetailPanel.cs`.
  12. Add playback controls in `AudioManager.cs` ensuring narrative tapes duck background music and ambient loops appropriately.
  13. Connect tape deck UI buttons (Play, Stop, Rewind) in `JournalDetailPanel.cs` to trigger matching tape transport foley.
  14. Add test cases in `AudioSelfTest.cs` exercising narrative tape cue resolution and playback.
  15. Execute `dotnet test Ashfall.Core.Tests` and confirm zero test failures.

---

### Task 16: Deep Sub-Bunker Infrastructure & Hydroponics Greenhouse Acoustics

* **Goal**: Sonify the subterranean life-support agriculture, aeroponic misting systems, UV ballasts, and nutrient pumps.
* **Substeps**:
  1. Synthesize high-pressure aeroponic misting nozzles: periodic 4-second atomized water spray hiss through micro-orifices.
  2. Synthesize UV horticultural grow lamp ballasts: solid-state electronic buzz (120 Hz) with subtle thermal expansion ticking.
  3. Synthesize hydroponic nutrient reservoir circulation: gentle fluid trickling through plastic drainage channels into holding reservoirs.
  4. Synthesize greenhouse condensation drops: light, random water droplet impacts falling onto plant leaves and plastic tarp sheeting.
  5. Synthesize automated irrigation solenoid valve: sharp magnetic coil snap and immediate water pressure pulse.
  6. Author synthesis script `tools/audio_gen/generate_greenhouse_hydraulics.py`.
  7. Render WAV files into `assets/audio/sfx/` and `assets/audio/ambience/`: `sfx_greenhouse_aeroponic_mist.wav`, `amb_greenhouse_uv_ballast_loop.wav`, `amb_greenhouse_nutrient_pump_loop.wav`, `amb_greenhouse_condensation_loop.wav`, `sfx_greenhouse_solenoid_valve.wav`.
  8. Generate Godot `.import` sidecars with loop flags for pump, ballast, and condensation loops.
  9. Enforce loudness normalization: -17.0 LUFS for peaceful greenhouse ambience, -5.0 dBFS peak for solenoid valves.
  10. Register cues in `AudioCueCatalog.cs` under `AudioBusNames.Ambience` and `AudioBusNames.Sfx`.
  11. Connect greenhouse growth cycles and crop harvesting in `Main.CampaignOwners.cs` and `GreenhousePanel.cs`.
  12. Add greenhouse audio control methods in `AudioManager.cs` (`StartGreenhouseAmbience()`, `StopGreenhouseAmbience()`, `PlayIrrigationMist()`).
  13. Add greenhouse assertions in `AudioSelfTest.cs` ensuring clean loop start/stop lifecycle.
  14. Run `bash scripts/ci/asset-orphan-sweep.sh` to confirm zero orphan files.
  15. Run `godot --headless --path . -- --audio-selftest`.

---

### Task 17: Radiation Dosimetry & Geiger Multi-Stage Sonification

* **Goal**: Provide a continuous, granular, 5-stage radiation sonification system spanning background cosmic clicks to lethal detector saturation.
* **Substeps**:
  1. Synthesize low-background cosmic ray clicks: sparse, non-periodic discrete micro-clicks occurring at random 0.5 to 2.5 second intervals.
  2. Synthesize low-level beta particle flutter: steady, crisp clicking (15–40 Hz) indicating elevated background contamination.
  3. Synthesize severe gamma-ray avalanche: high-density continuous discharge (120–300 Hz) where individual pulses blur into an aggressive buzz.
  4. Synthesize pocket quartz fiber dosimeter charging squeal: high-frequency piezoelectric charging squeal (10.5 kHz) with decay.
  5. Synthesize ionization chamber detector saturation tone: continuous high-pitched alarm tone signaling sensor saturation and lethal flux.
  6. Author generator script `tools/audio_gen/generate_dosimetry_suite.py`.
  7. Export WAV assets to `assets/audio/sfx/`: `sfx_geiger_cosmic_click.wav`, `sfx_geiger_beta_flutter_loop.wav`, `sfx_geiger_gamma_avalanche_loop.wav`, `sfx_dosimeter_quartz_charge.wav`, `sfx_geiger_saturation_alarm.wav`.
  8. Generate Godot `.import` sidecars with appropriate looping parameters.
  9. Calibrate audio levels to ensure urgent psychological tension without causing acoustic ear fatigue.
  10. Add cue declarations in `AudioCueCatalog.cs` (`RadCosmicClick`, `RadBetaFlutter`, `RadGammaAvalanche`, `RadQuartzCharge`, `RadSaturationAlarm`).
  11. Enhance `AudioEventBridge.OnRadiationDoseChanged` to smoothly transition between the 5 dosimetry tiers based on current Sievert/hour exposure.
  12. Implement `SetDosimetryLevel(float doseRate)` in `AudioManager.cs` to manage seamless cross-fading between Geiger discharge loops.
  13. Add comprehensive dosimetry tests in `AudioSelfTest.cs` verifying threshold switching and mute behavior.
  14. Re-generate `docs/audio/AUDIO_CUE_CATALOG.md` and verify sync.
  15. Run full test matrix (`dotnet test Ashfall.Core.Tests` and `--audio-selftest`).

---

### Task 18: Barter, Trade & Economy Tactile Sound Effects

* **Goal**: Sonify post-nuclear commerce with tactile foley for bottle caps, silver bullion, lead ammunition counting, scrip currency, and brass balance scales.
* **Substeps**:
  1. Synthesize bottle cap barter transactions: light tinny metal bottle cap jingle and pouch shake (transients from 2.0 kHz to 6.0 kHz).
  2. Synthesize silver bullion ingot exchange: dense, pure metallic chime (3.4 kHz harmonic fundamental) dropped onto a wooden trade counter.
  3. Synthesize loose cartridge lead counting: heavy military rifle cartridges dropping onto wood one by one during trade negotiations.
  4. Synthesize worn paper scrip currency: crinkled, fibrous post-war paper banknotes sliding and counting between fingers.
  5. Synthesize antique brass balance scale settling: brass weight clink followed by balance pivot squeak as trade value reaches equilibrium.
  6. Author procedural synthesis tool `tools/audio_gen/generate_economy_foley.py`.
  7. Render WAV files to `assets/audio/sfx/`: `sfx_trade_bottle_caps.wav`, `sfx_trade_silver_bullion.wav`, `sfx_trade_cartridge_count.wav`, `sfx_trade_scrip_rustle.wav`, `sfx_trade_brass_scale.wav`.
  8. Auto-generate Godot `.import` sidecars for one-shot UI trade events.
  9. Normalize peak audio levels to -5.0 dBFS true peak to match standard UI feedback.
  10. Register cues in `AudioCueCatalog.cs` under `AudioBusNames.Ui`.
  11. Connect trade transaction execution in `TradePanel.cs` and `EconomyDetailPanel.cs` to play currency-specific foley based on trade goods.
  12. Add trade foley methods to `AudioManager.cs` (`PlayTradeCaps()`, `PlayTradeSilver()`, `PlayTradeAmmo()`, `PlayTradeScrip()`, `PlayTradeScale()`).
  13. Add trade audio assertions in `AudioSelfTest.cs` verifying cue resolution and UI bus assignment.
  14. Execute `bash scripts/ci/asset-orphan-sweep.sh` to confirm 0 orphan files.
  15. Run `python3 scripts/ci/generate-audio-catalog.py --check` and verify clean sync.

---

### Task 19: Exploration Hazards, Structural Collapses & Scavenging Foley

* **Goal**: Implement alarming structural stress foley, ceiling collapses, rusted locker forcing, glass crunching, and safe combination cracking.
* **Substeps**:
  1. Synthesize structural steel beam deflection groan: deep low-frequency iron tension groan (60 Hz to 180 Hz) ending in rivet failure pop.
  2. Synthesize masonry and ceiling drywall collapse: cascading debris fall with heavy concrete chunks pulverizing onto floorboards amidst billowing dust.
  3. Synthesize rusted locker door pry: high-friction rusted steel hinge squeal followed by mechanical latch shear and pop.
  4. Synthesize walking over shattered tempered glass: crisp, sharp multi-layered crunching transients beneath heavy rubber wasteland boots.
  5. Synthesize bank vault tumbler wheel lock dial clicks: delicate, heavy brass mechanical disc drop transients as combination numbers align.
  6. Author generator script `tools/audio_gen/generate_scavenging_hazards.py`.
  7. Export WAV assets to `assets/audio/sfx/`: `sfx_hazard_beam_groan.wav`, `sfx_hazard_ceiling_collapse.wav`, `sfx_scavenge_locker_pry.wav`, `sfx_scavenge_glass_crunch.wav`, `sfx_scavenge_safe_tumbler.wav`.
  8. Generate Godot `.import` sidecars for each sound file.
  9. Calibrate dynamic levels: -2.0 dBFS peak for collapses and -6.0 dBFS for safe dial tumblers.
  10. Register cues in `AudioCueCatalog.cs` (`HazardBeamGroan`, `HazardCeilingCollapse`, `ScavengeLockerPry`, `ScavengeGlassCrunch`, `ScavengeSafeTumbler`).
  11. Wire expedition ruin hazard encounters in `ExpeditionHostSession` and safe-cracking interactions in `SafeCrackModal.cs`.
  12. Add methods in `AudioManager.cs` (`PlayBeamGroan()`, `PlayCeilingCollapse()`, `PlayLockerPry()`, `PlayGlassCrunch()`, `PlaySafeTumbler()`).
  13. Add cue validation tests in `src/Audio/AudioSelfTest.cs`.
  14. Verify zero orphan sources and sidecars via `asset-orphan-sweep.sh`.
  15. Run `dotnet test Ashfall.Core.Tests` and confirm zero broken tests.

---

### Task 20: Cinematic Game Flow, Death Stings & Victory Fanfares

* **Goal**: Synthesize iconic musical and sound design stings for existential milestones: Thermonuclear Detonations, Survivor Deaths, Expedition Triumphs, Extinction, and New Dawn.
* **Substeps**:
  1. Synthesize distant thermonuclear detonation rumble: infrasonic rolling shockwave (20 Hz to 55 Hz) with 8-second atmospheric dispersion and air pressure drop.
  2. Synthesize tragic survivor death solo cello sting: somber, mournful acoustic bowed cello chord (D minor fundamental) with downward micro-tonal glissando.
  3. Synthesize expedition triumph low-brass swell: warm, resilient low-brass triad (French horn and trombone swell) evoking gritty survival against the odds.
  4. Synthesize shelter extinction desolate wind fall: sudden shutdown of bunker life-support hum transitioning into cold, dead surface wind whistle.
  5. Synthesize new dawn acoustic bell chime: resonant bronze bell strike with rich overtone spectrum and 4.5-second lingering acoustic decay.
  6. Author procedural sting synthesis script `tools/audio_gen/generate_cinematic_stings.py`.
  7. Render WAV files to `assets/audio/music/` and `assets/audio/sfx/`: `sfx_cinematic_nuke_rumble.wav`, `mus_sting_survivor_death.wav`, `mus_sting_expedition_triumph.wav`, `mus_sting_shelter_extinction.wav`, `sfx_sting_new_dawn_bell.wav`.
  8. Generate Godot `.import` sidecars with proper audio bus assignments.
  9. Normalize loudness to integrate seamlessly into the master music and UI hierarchy (-10.0 LUFS for music stings, -2.0 dBFS peak for nuke rumbles).
  10. Register cues in `AudioCueCatalog.cs` (`CinematicNukeRumble`, `StingSurvivorDeath`, `StingExpeditionTriumph`, `StingShelterExtinction`, `StingNewDawnBell`).
  11. Connect game flow milestones in `Main.CampaignOwners.cs` (game over, day transition, major quest triumphs) to fire cinematic stings.
  12. Add game flow methods in `AudioManager.cs` (`PlayNukeRumble()`, `PlaySurvivorDeathSting()`, `PlayExpeditionTriumphSting()`, `PlayExtinctionSting()`, `PlayNewDawnBell()`).
  13. Add end-to-end game flow tests in `AudioSelfTest.cs` asserting proper sting playback and cleanup.
  14. Execute `bash scripts/ci/asset-orphan-sweep.sh` to confirm zero asset orphan drift.
  15. Execute the full mandatory project verification matrix:
      - `dotnet test Ashfall.Core.Tests` (must pass 100%, 0 failed)
      - `godot --headless --path . -- --audio-selftest` (must pass 100%, 0 failed)
      - `godot --headless --path . -- --data-integrity-selftest` (0 errors across 208 catalogs)
      - `godot --headless --path . -- --content-utilization-selftest` (CI gate PASS)
      - `godot --headless --path . -- --scene-binding-selftest` (22/22 passed)
      - `python3 scripts/ci/scene-lint.py` (0 errors across production scenes)

---

## Verification & Guardrail Checklist

Every task in this roadmap must adhere to the following strict pipeline checks before completion:

```bash
# 1. Zero orphan sources & sidecars
bash scripts/ci/asset-orphan-sweep.sh

# 2. Audio cue documentation synchronization
python3 scripts/ci/generate-audio-catalog.py --check

# 3. Godot headless audio self-test
godot --headless --path . -- --audio-selftest

# 4. Core xUnit test suite
dotnet test Ashfall.Core.Tests

# 5. Scene linting
python3 scripts/ci/scene-lint.py
```
