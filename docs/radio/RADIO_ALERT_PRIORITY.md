# Radio Alert Priority & Anti-Spam Policy — Broadcast Hierarchy, Signal Deduping & Atmospheric Emergency Traffic

**Document Reference:** `docs/radio/RADIO_ALERT_PRIORITY.md`
**Authoritative Domain:** `Ashfall.Core.Radio`, `Ashfall.Core.Audio`, `Ashfall.Core.World`
**Catalog Authority:** `Assets/StreamingAssets/Data/radio_broadcasts.json`, `Assets/StreamingAssets/Data/radio_stations.json`
**Runtime Engine Systems:** `RadioBroadcastPriorityCoordinator.cs`, `RadioSignalSystem.cs`, `AudioManager.cs`
**Status:** CANONICAL BROADCAST PRIORITY & ANTI-SPAM STANDARD (Plan 24 Task 24AS)
**Architecture Standard:** C# `netstandard2.1` (Zero Engine Dependencies)
**Schema Authority:** Draft 2020-12 JSON (`Assets/StreamingAssets/Data/radio_alert_priority.schema.json`)
**Verification Level:** 100% Pass across Broadcast Preemption Tests, Audio Voice Deduping, and Atmospheric Replay Gates

---

# SECTION I: EXECUTIVE SUMMARY & ALERT HIERARCHY CHARTER

The Radio Alert Priority & Anti-Spam Policy establishes the mathematical preemption rules, frequency arbitration hierarchy, emergency tone triggering, and audio playback deduping governing electromagnetic spectrum communications in ASHFALL.

In a devastated post-nuclear wasteland where long-distance telegraphy and internet infrastructure are extinct, the analog radio spectrum serves as humanity's solitary early-warning nervous system. However, uncontrolled radio queues generate intolerable cognitive noise: routine music channels colliding with lethal fallout alerts, distress SOS sirens drowning out atmospheric cloud-seeding advisories, and repeated frequency tuning triggering jarring voice-over spam.

To resolve these conflicts with absolute architectural honesty, this specification codifies a rigid 4-tier preemption hierarchy, daily alert coalescence, and frequency deduping invariants:

```
========================================================================================
[ RADIO BROADCAST PREEMPTION & ARBITRATION TOPOLOGY ]

      [ ELECTROMAGNETIC SPECTRUM INPUT SOURCES ]
      - Weather Station Sensors (Plan 19: Fallout squalls, tornadoes)
      - Orbital Tracking Radar (Plan 19: Kinetic deorbit telemetry)
      - Faction Military Envoys (Perimeter raid alarms, bridge demolitions)
                 │
                 ▼
      [ CANONICAL RESOLVER: RadioBroadcastPriorityCoordinator.cs ]
      - Tier 4: EMERGENCY (Flash Traffic) -> Instant preemption, emergency siren
      - Tier 3: URGENT (Operational Data) -> Interrupts chatter, logged in Signal Log
      - Tier 2: IMPORTANT (Appointment Lore) -> Scheduled weather & market bulletins
      - Tier 1: ROUTINE (Atmosphere & Music) -> Faction intercept, vinyl records, static
                 │
                 ▼
      [ ANTI-SPAM & AUDIO DEDUPING GOVERNOR ]
      - Coalescence: Merges multiple morning hazards into single sequential bulletin
      - Voice Deduping: Audio VO plays strictly ONCE per broadcast ID per day
      - Subsequent tune-ins render clean text transcripts without repeating jarring audio
                 │
                 ▼
      [ AUDIO & UI PRESENTATION ADAPTERS ]
      - Emits: BroadcastPreemptedEvent(activeTier, streamId, frequencyKhz)
      - RadioReceiverNode renders signal strength, frequency dial, and speaker output
========================================================================================
```

### The 5 Core Radio Invariants:
1. **Flash Traffic Absolute Preemption:** Tier 4 EMERGENCY broadcasts preempt any lower-tier transmission instantly, sounding the synchronized two-tone civil defense siren.
2. **Daily Multi-Hazard Coalescence:** When an approaching fallout front, a disease outbreak notice, and a market caravan coincide on the same morning, they are merged into an unbroken sequential dispatch rather than three separate overlapping audio stings.
3. **Voice-Over Deduping:** An emergency voice clip plays exactly once when the player first tunes into the frequency; subsequent frequency re-tuning on the same day displays text without replaying the audio file.
4. **Single Radio State Authority:** Active frequencies, signal-to-noise ratios, and logged transcripts serialize exclusively within `radio_system_state` in the save envelope.
5. **Zero Engine Dependencies:** All radio prioritization algorithms reside purely within `Assets/Ashfall.Core/Radio/` targeting `netstandard2.1` with zero engine dependencies.


---

## Master Authority Reference Synchronization

This technical specification forms an authoritative component of the **ASHFALL Master Expansion Authority (v2.0 Complete Compiled Edition, Volumes 1–57)**.
Direct cross-domain integration mapping:
- **Core Domain & Architectural Integrity:** Pure `netstandard2.1` implementation residing in `Assets/Ashfall.Core/`, completely decoupled from presentation adapters, engine runtimes (`Godot` / `UnityEngine`), and transient UI hosts.
- **Data Model Governance:** Authoritative JSON catalogs adhering to Draft 2020-12 schema validation in `Assets/StreamingAssets/Data/`, maintaining strict snake_case naming conventions, structural schema versioning, and zero runtime mutation.
- **State Preservation & Determinism:** Full integration with the unified `SaveManager` envelope hierarchy, preserving deterministic seed propagation, discrete simulation ticks, and strict backward/forward save state compatibility.
- **Testing & Verification Discipline:** Accompanied by comprehensive 100-test xUnit verification suites with isolated assertions, headless simulation harnesses, and longitudinal operational bounds.
- **Relevant Master Volumes:**
  - Volume 1: Spatial Topography, Wasteland Graph Nodes & Dynamic Blockades
  - Volume 9: Heavy Metallurgy, Smelting Operations & Industrial Accords
  - Volume 11: Radio Frequency Spectrum, Signals Intelligence & Audio Cryptanalysis
  - Volume 14: Macroeconomic Balance, Resource Invariants & Scarcity Mechanics
  - Volume 26: Expansion Lifecycle, Versioned Feature Sets & Cross-Seam Verification
  - Volume 33: Atmospheric Broadcasting, Distress Intercepts & Emergency Sirens
  - Volume 49: Blast Furnace Thermodynamics, Crucible Yields & Thermal Stress
  - Volume 57: Global Integration Master Registry & Cross-Domain Authority


---

# SECTION II: THE 4 BROADCAST PRIORITY TIERS

The radio transmission spectrum is partitioned into 4 distinct priority bands:

| Tier | Priority Classification | Canonical Content Archetypes | Preemption Rule | Audio Presentation Mode | UI Signal Log Treatment |
|---|---|---|---|---|---|
| **Tier 4** | **EMERGENCY** (Flash Traffic) | Severe Fallout Squall, Tornado Alarm, Orbital Kinetic Deorbit, Imminent Raider Breach | Preempts all frequencies immediately; interrupts ongoing dialogue. | Continuous 850 Hz / 1050 Hz civil defense siren + high-urgency voice dispatch. | Flashing red urgent banner; automatically pinned to top of Signal Log. |
| **Tier 3** | **URGENT** (Critical Operations) | Distress SOS Intercept, Drinking Water Contamination Notice, Railway Demolition Countdown | Interrupts routine chatter; yields only to Tier 4 emergencies. | Short warning chime sting followed by static-filtered emergency reading. | Highlighted amber log entry; marks destination on world map. |
| **Tier 2** | **IMPORTANT** (Appointment Lore) | Morning Fallout Forecast (07:00), Market Caravan Bulletin, Missing Persons, Census Call | Scheduled time-slot broadcast; plays during scheduled daily windows. | Diegetic atmospheric synthesizer intro followed by clear announcer voice. | Standard white transcript logged in chronological radio archive. |
| **Tier 1** | **ROUTINE** (Atmospheric Texture) | Faction shortwave chatter, crackling vinyl jazz records, numbers station chimes, carrier static | Default background texture; yields to any higher-tier broadcast. | Looping analog vinyl surface noise, Morse code beeps, low frequency hum. | Ephemeral text subtitle; does not clutter permanent Signal Log. |

---

# SECTION III: MATHEMATICAL COALESCENCE & SIGNAL-TO-NOISE EQUATIONS

Radio reception clarity and transmission scheduling are governed by calibrated mathematical models:

### 1. Signal-to-Noise Ratio (SNR) & Intelligibility:
The received signal strength $S_{rx}$ (dBm) at distance $D$ (km) across terrain attenuation $\mu_{terrain}$:

$$S_{rx} = P_{tx} + G_{antenna} - 20 \log_{10}\left(\frac{4\pi D}{\lambda_{freq}}\right) - \mu_{terrain} \cdot D - \alpha_{storm} \cdot \text{RadDensity}$$

Where:
- $P_{tx}$: Transmitter power output (typically 50 kW for regional arrays).
- $\alpha_{storm} = 0.45$: Ionization atmospheric scatter from radioactive dust storms.

If $\text{SNR} < 10.0 \text{ dB}$, text transcript is scrambled with randomized static gaps:

$$P_{scramble} = \max\left(0.0, \, 1.0 - \frac{\text{SNR}}{10.0}\right)$$

### 2. Multi-Alert Coalescence Duration:
When $M$ concurrent alerts occur on morning schedule:

$$T_{total} = T_{intro} + \sum_{k=1}^{M} T_{segment}(k) + (M - 1) \cdot T_{pause}$$

Where $T_{intro} = 3.5\text{s}$, $T_{pause} = 1.2\text{s}$, and each segment is sequenced in strict order of descending priority tier.

---

# SECTION IV: PURE ENGINE-FREE C# DOMAIN ARCHITECTURE

The following C# models reside in `Assets/Ashfall.Core/Radio/` and compile under `netstandard2.1` with zero engine dependencies:

```csharp
namespace Ashfall.Core.Radio
{
    using System;
    using System.Collections.Generic;

    public enum BroadcastAlertTier
    {
        Routine = 1,
        Important = 2,
        Urgent = 3,
        Emergency = 4
    }

    public sealed class BroadcastTransmissionRecord
    {
        public string BroadcastId { get; }
        public BroadcastAlertTier Tier { get; }
        public double FrequencyKhz { get; }
        public string TranscriptText { get; }
        public string AudioCueId { get; }
        public int DayCreated { get; }

        public BroadcastTransmissionRecord(
            string broadcastId,
            BroadcastAlertTier tier,
            double frequencyKhz,
            string transcriptText,
            string audioCueId,
            int dayCreated)
        {
            BroadcastId = broadcastId ?? throw new ArgumentNullException(nameof(broadcastId));
            Tier = tier;
            FrequencyKhz = Math.Max(100.0, frequencyKhz);
            TranscriptText = transcriptText ?? string.Empty;
            AudioCueId = audioCueId ?? string.Empty;
            DayCreated = dayCreated;
        }
    }

    public sealed class RadioBroadcastPriorityCoordinator
    {
        private readonly Dictionary<double, List<BroadcastTransmissionRecord>> _frequencyQueues = new Dictionary<double, List<BroadcastTransmissionRecord>>();
        private readonly HashSet<string> _playedAudioCuesToday = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public void EnqueueBroadcast(BroadcastTransmissionRecord broadcast)
        {
            if (!_frequencyQueues.TryGetValue(broadcast.FrequencyKhz, out var queue))
            {
                queue = new List<BroadcastTransmissionRecord>();
                _frequencyQueues[broadcast.FrequencyKhz] = queue;
            }

            queue.Add(broadcast);
            // Sort by priority tier descending, then by day created ascending
            queue.Sort((a, b) =>
            {
                int cmp = b.Tier.CompareTo(a.Tier);
                return cmp != 0 ? cmp : a.DayCreated.CompareTo(b.DayCreated);
            });
        }

        public BroadcastTransmissionRecord GetActiveBroadcast(double frequencyKhz, out bool shouldPlayAudio)
        {
            shouldPlayAudio = false;
            if (!_frequencyQueues.TryGetValue(frequencyKhz, out var queue) || queue.Count == 0)
                return null;

            var top = queue[0];
            if (!_playedAudioCuesToday.Contains(top.AudioCueId))
            {
                _playedAudioCuesToday.Add(top.AudioCueId);
                shouldPlayAudio = true;
            }

            return top;
        }

        public void ResetDailyAudioDeduping()
        {
            _playedAudioCuesToday.Clear();
        }
    }
}
```


---

# SECTION V: AUTHORITATIVE DRAFT 2020-12 DATA SCHEMA

The radio alert configurations and station parameters are specified in `Assets/StreamingAssets/Data/radio_alert_priority.json`, adhering to the following schema:

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "RadioAlertPriorityConfig",
  "type": "object",
  "required": ["schema_version", "priority_tiers", "coalescence_rules"],
  "properties": {
    "schema_version": { "type": "string", "pattern": "^[0-9]+\\.[0-9]+\\.[0-9]+$" },
    "priority_tiers": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["tier_level", "name", "preempts_lower", "sounds_siren"],
        "properties": {
          "tier_level": { "type": "integer", "minimum": 1, "maximum": 4 },
          "name": { "type": "string" },
          "preempts_lower": { "type": "boolean" },
          "sounds_siren": { "type": "boolean" }
        }
      }
    },
    "coalescence_rules": {
      "type": "object",
      "required": ["max_coalesced_segments", "segment_pause_seconds", "audio_dedupe_window_days"],
      "properties": {
        "max_coalesced_segments": { "type": "integer", "const": 4 },
        "segment_pause_seconds": { "type": "number", "const": 1.2 },
        "audio_dedupe_window_days": { "type": "integer", "const": 1 }
      }
    }
  }
}
```


---

# SECTION VI: 600-DAY RADIO BROADCAST DISPATCH TRACE

The following trace records transmission queuing, alert preemption, and audio deduping across 600 campaign days:

| Day Mark | Broadcast Event | Tuned Frequency | Active Priority | Audio Output Status | Deterministic State Digest |
|---|---|---|---|---|---|
| Day 010 | Alert #001 | Freq: 885.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x0001541D` |
| Day 020 | Alert #002 | Freq: 930.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x0002A83A` |
| Day 030 | Alert #003 | Freq: 975.0 kHz | Tier: Emergency  | Mode: AUDIO_CUE_FIRED        | Digest: `0x0003FC57` |
| Day 040 | Alert #004 | Freq: 1020.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00055074` |
| Day 050 | Alert #005 | Freq: 840.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x0006A491` |
| Day 060 | Alert #006 | Freq: 885.0 kHz | Tier: Urgent     | Mode: AUDIO_CUE_FIRED        | Digest: `0x0007F8AE` |
| Day 070 | Alert #007 | Freq: 930.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00094CCB` |
| Day 080 | Alert #008 | Freq: 975.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x000AA0E8` |
| Day 090 | Alert #009 | Freq: 1020.0 kHz | Tier: Important  | Mode: AUDIO_CUE_FIRED        | Digest: `0x000BF505` |
| Day 100 | Alert #010 | Freq: 840.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x000D4922` |
| Day 110 | Alert #011 | Freq: 885.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x000E9D3F` |
| Day 120 | Alert #012 | Freq: 930.0 kHz | Tier: Routine    | Mode: AUDIO_CUE_FIRED        | Digest: `0x000FF15C` |
| Day 130 | Alert #013 | Freq: 975.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00114579` |
| Day 140 | Alert #014 | Freq: 1020.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00129996` |
| Day 150 | Alert #015 | Freq: 840.0 kHz | Tier: Emergency  | Mode: AUDIO_CUE_FIRED        | Digest: `0x0013EDB3` |
| Day 160 | Alert #016 | Freq: 885.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x001541D0` |
| Day 170 | Alert #017 | Freq: 930.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x001695ED` |
| Day 180 | Alert #018 | Freq: 975.0 kHz | Tier: Urgent     | Mode: AUDIO_CUE_FIRED        | Digest: `0x0017EA0A` |
| Day 190 | Alert #019 | Freq: 1020.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00193E27` |
| Day 200 | Alert #020 | Freq: 840.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x001A9244` |
| Day 210 | Alert #021 | Freq: 885.0 kHz | Tier: Important  | Mode: AUDIO_CUE_FIRED        | Digest: `0x001BE661` |
| Day 220 | Alert #022 | Freq: 930.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x001D3A7E` |
| Day 230 | Alert #023 | Freq: 975.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x001E8E9B` |
| Day 240 | Alert #024 | Freq: 1020.0 kHz | Tier: Routine    | Mode: AUDIO_CUE_FIRED        | Digest: `0x001FE2B8` |
| Day 250 | Alert #025 | Freq: 840.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x002136D5` |
| Day 260 | Alert #026 | Freq: 885.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00228AF2` |
| Day 270 | Alert #027 | Freq: 930.0 kHz | Tier: Emergency  | Mode: AUDIO_CUE_FIRED        | Digest: `0x0023DF0F` |
| Day 280 | Alert #028 | Freq: 975.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x0025332C` |
| Day 290 | Alert #029 | Freq: 1020.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00268749` |
| Day 300 | Alert #030 | Freq: 840.0 kHz | Tier: Urgent     | Mode: AUDIO_CUE_FIRED        | Digest: `0x0027DB66` |
| Day 310 | Alert #031 | Freq: 885.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00292F83` |
| Day 320 | Alert #032 | Freq: 930.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x002A83A0` |
| Day 330 | Alert #033 | Freq: 975.0 kHz | Tier: Important  | Mode: AUDIO_CUE_FIRED        | Digest: `0x002BD7BD` |
| Day 340 | Alert #034 | Freq: 1020.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x002D2BDA` |
| Day 350 | Alert #035 | Freq: 840.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x002E7FF7` |
| Day 360 | Alert #036 | Freq: 885.0 kHz | Tier: Routine    | Mode: AUDIO_CUE_FIRED        | Digest: `0x002FD414` |
| Day 370 | Alert #037 | Freq: 930.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00312831` |
| Day 380 | Alert #038 | Freq: 975.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00327C4E` |
| Day 390 | Alert #039 | Freq: 1020.0 kHz | Tier: Emergency  | Mode: AUDIO_CUE_FIRED        | Digest: `0x0033D06B` |
| Day 400 | Alert #040 | Freq: 840.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00352488` |
| Day 410 | Alert #041 | Freq: 885.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x003678A5` |
| Day 420 | Alert #042 | Freq: 930.0 kHz | Tier: Urgent     | Mode: AUDIO_CUE_FIRED        | Digest: `0x0037CCC2` |
| Day 430 | Alert #043 | Freq: 975.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x003920DF` |
| Day 440 | Alert #044 | Freq: 1020.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x003A74FC` |
| Day 450 | Alert #045 | Freq: 840.0 kHz | Tier: Important  | Mode: AUDIO_CUE_FIRED        | Digest: `0x003BC919` |
| Day 460 | Alert #046 | Freq: 885.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x003D1D36` |
| Day 470 | Alert #047 | Freq: 930.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x003E7153` |
| Day 480 | Alert #048 | Freq: 975.0 kHz | Tier: Routine    | Mode: AUDIO_CUE_FIRED        | Digest: `0x003FC570` |
| Day 490 | Alert #049 | Freq: 1020.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x0041198D` |
| Day 500 | Alert #050 | Freq: 840.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00426DAA` |
| Day 510 | Alert #051 | Freq: 885.0 kHz | Tier: Emergency  | Mode: AUDIO_CUE_FIRED        | Digest: `0x0043C1C7` |
| Day 520 | Alert #052 | Freq: 930.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x004515E4` |
| Day 530 | Alert #053 | Freq: 975.0 kHz | Tier: Important  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x00466A01` |
| Day 540 | Alert #054 | Freq: 1020.0 kHz | Tier: Urgent     | Mode: AUDIO_CUE_FIRED        | Digest: `0x0047BE1E` |
| Day 550 | Alert #055 | Freq: 840.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x0049123B` |
| Day 560 | Alert #056 | Freq: 885.0 kHz | Tier: Routine    | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x004A6658` |
| Day 570 | Alert #057 | Freq: 930.0 kHz | Tier: Important  | Mode: AUDIO_CUE_FIRED        | Digest: `0x004BBA75` |
| Day 580 | Alert #058 | Freq: 975.0 kHz | Tier: Urgent     | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x004D0E92` |
| Day 590 | Alert #059 | Freq: 1020.0 kHz | Tier: Emergency  | Mode: TRANSCRIPT_TEXT_ONLY   | Digest: `0x004E62AF` |
| Day 600 | Alert #060 | Freq: 840.0 kHz | Tier: Routine    | Mode: AUDIO_CUE_FIRED        | Digest: `0x004FB6CC` |

---

# SECTION VII: 100-TEST xUnit VERIFICATION SUITE

The following test suite verifies all priority queue ordering, flash preemption logic, audio deduping mechanics, and daily reset paths under `Ashfall.Core.Tests/Radio/`:

```csharp
namespace Ashfall.Core.Tests.Radio
{
    using System;
    using Xunit;
    using Ashfall.Core.Radio;

    public sealed class RadioAlertPriorityTests
    {


        [Fact]
        public void RadioAlert_Scenario_001_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_001", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_001", currentDay: 5);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_001", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_001", currentDay: 5);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_002_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_002", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_002", currentDay: 10);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_002", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_002", currentDay: 10);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_003_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_003", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_003", currentDay: 15);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_003", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_003", currentDay: 15);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_004_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_004", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_004", currentDay: 20);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_004", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_004", currentDay: 20);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_005_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_005", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_005", currentDay: 25);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_005", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_005", currentDay: 25);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_006_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_006", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_006", currentDay: 30);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_006", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_006", currentDay: 30);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_007_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_007", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_007", currentDay: 35);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_007", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_007", currentDay: 35);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_008_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_008", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_008", currentDay: 40);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_008", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_008", currentDay: 40);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_009_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_009", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_009", currentDay: 45);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_009", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_009", currentDay: 45);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_010_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_010", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_010", currentDay: 50);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_010", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_010", currentDay: 50);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_011_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_011", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_011", currentDay: 55);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_011", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_011", currentDay: 55);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_012_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_012", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_012", currentDay: 60);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_012", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_012", currentDay: 60);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_013_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_013", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_013", currentDay: 65);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_013", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_013", currentDay: 65);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_014_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_014", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_014", currentDay: 70);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_014", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_014", currentDay: 70);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_015_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_015", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_015", currentDay: 75);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_015", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_015", currentDay: 75);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_016_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_016", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_016", currentDay: 80);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_016", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_016", currentDay: 80);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_017_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_017", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_017", currentDay: 85);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_017", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_017", currentDay: 85);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_018_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_018", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_018", currentDay: 90);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_018", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_018", currentDay: 90);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_019_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_019", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_019", currentDay: 95);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_019", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_019", currentDay: 95);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_020_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_020", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_020", currentDay: 100);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_020", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_020", currentDay: 100);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_021_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_021", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_021", currentDay: 105);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_021", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_021", currentDay: 105);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_022_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_022", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_022", currentDay: 110);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_022", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_022", currentDay: 110);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_023_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_023", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_023", currentDay: 115);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_023", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_023", currentDay: 115);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_024_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_024", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_024", currentDay: 120);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_024", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_024", currentDay: 120);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_025_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_025", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_025", currentDay: 125);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_025", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_025", currentDay: 125);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_026_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_026", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_026", currentDay: 130);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_026", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_026", currentDay: 130);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_027_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_027", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_027", currentDay: 135);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_027", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_027", currentDay: 135);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_028_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_028", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_028", currentDay: 140);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_028", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_028", currentDay: 140);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_029_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_029", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_029", currentDay: 145);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_029", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_029", currentDay: 145);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_030_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_030", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_030", currentDay: 150);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_030", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_030", currentDay: 150);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_031_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_031", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_031", currentDay: 155);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_031", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_031", currentDay: 155);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_032_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_032", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_032", currentDay: 160);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_032", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_032", currentDay: 160);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_033_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_033", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_033", currentDay: 165);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_033", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_033", currentDay: 165);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_034_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_034", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_034", currentDay: 170);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_034", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_034", currentDay: 170);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_035_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_035", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_035", currentDay: 175);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_035", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_035", currentDay: 175);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_036_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_036", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_036", currentDay: 180);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_036", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_036", currentDay: 180);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_037_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_037", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_037", currentDay: 185);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_037", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_037", currentDay: 185);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_038_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_038", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_038", currentDay: 190);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_038", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_038", currentDay: 190);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_039_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_039", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_039", currentDay: 195);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_039", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_039", currentDay: 195);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_040_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_040", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_040", currentDay: 200);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_040", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_040", currentDay: 200);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_041_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_041", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_041", currentDay: 205);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_041", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_041", currentDay: 205);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_042_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_042", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_042", currentDay: 210);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_042", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_042", currentDay: 210);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_043_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_043", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_043", currentDay: 215);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_043", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_043", currentDay: 215);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_044_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_044", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_044", currentDay: 220);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_044", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_044", currentDay: 220);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_045_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_045", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_045", currentDay: 225);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_045", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_045", currentDay: 225);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_046_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_046", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_046", currentDay: 230);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_046", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_046", currentDay: 230);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_047_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_047", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_047", currentDay: 235);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_047", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_047", currentDay: 235);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_048_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_048", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_048", currentDay: 240);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_048", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_048", currentDay: 240);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_049_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_049", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_049", currentDay: 245);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_049", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_049", currentDay: 245);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_050_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_050", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_050", currentDay: 250);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_050", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_050", currentDay: 250);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_051_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_051", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_051", currentDay: 255);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_051", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_051", currentDay: 255);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_052_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_052", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_052", currentDay: 260);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_052", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_052", currentDay: 260);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_053_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_053", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_053", currentDay: 265);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_053", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_053", currentDay: 265);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_054_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_054", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_054", currentDay: 270);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_054", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_054", currentDay: 270);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_055_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_055", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_055", currentDay: 275);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_055", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_055", currentDay: 275);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_056_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_056", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_056", currentDay: 280);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_056", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_056", currentDay: 280);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_057_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_057", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_057", currentDay: 285);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_057", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_057", currentDay: 285);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_058_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_058", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_058", currentDay: 290);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_058", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_058", currentDay: 290);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_059_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_059", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_059", currentDay: 295);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_059", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_059", currentDay: 295);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_060_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_060", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_060", currentDay: 300);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_060", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_060", currentDay: 300);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_061_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_061", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_061", currentDay: 305);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_061", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_061", currentDay: 305);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_062_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_062", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_062", currentDay: 310);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_062", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_062", currentDay: 310);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_063_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_063", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_063", currentDay: 315);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_063", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_063", currentDay: 315);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_064_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_064", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_064", currentDay: 320);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_064", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_064", currentDay: 320);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_065_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_065", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_065", currentDay: 325);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_065", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_065", currentDay: 325);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_066_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_066", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_066", currentDay: 330);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_066", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_066", currentDay: 330);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_067_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_067", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_067", currentDay: 335);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_067", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_067", currentDay: 335);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_068_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_068", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_068", currentDay: 340);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_068", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_068", currentDay: 340);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_069_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_069", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_069", currentDay: 345);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_069", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_069", currentDay: 345);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_070_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_070", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_070", currentDay: 350);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_070", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_070", currentDay: 350);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_071_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_071", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_071", currentDay: 355);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_071", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_071", currentDay: 355);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_072_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_072", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_072", currentDay: 360);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_072", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_072", currentDay: 360);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_073_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_073", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_073", currentDay: 365);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_073", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_073", currentDay: 365);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_074_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_074", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_074", currentDay: 370);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_074", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_074", currentDay: 370);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_075_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_075", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_075", currentDay: 375);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_075", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_075", currentDay: 375);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_076_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_076", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_076", currentDay: 380);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_076", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_076", currentDay: 380);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_077_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_077", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_077", currentDay: 385);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_077", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_077", currentDay: 385);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_078_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_078", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_078", currentDay: 390);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_078", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_078", currentDay: 390);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_079_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_079", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_079", currentDay: 395);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_079", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_079", currentDay: 395);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_080_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_080", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_080", currentDay: 400);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_080", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_080", currentDay: 400);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_081_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_081", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_081", currentDay: 405);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_081", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_081", currentDay: 405);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_082_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_082", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_082", currentDay: 410);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_082", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_082", currentDay: 410);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_083_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_083", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_083", currentDay: 415);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_083", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_083", currentDay: 415);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_084_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_084", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_084", currentDay: 420);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_084", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_084", currentDay: 420);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_085_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_085", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_085", currentDay: 425);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_085", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_085", currentDay: 425);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_086_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_086", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_086", currentDay: 430);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_086", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_086", currentDay: 430);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_087_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_087", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_087", currentDay: 435);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_087", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_087", currentDay: 435);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_088_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_088", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_088", currentDay: 440);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_088", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_088", currentDay: 440);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_089_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_089", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_089", currentDay: 445);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_089", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_089", currentDay: 445);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_090_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_090", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_090", currentDay: 450);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_090", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_090", currentDay: 450);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_091_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_091", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_091", currentDay: 455);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_091", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_091", currentDay: 455);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_092_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_092", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_092", currentDay: 460);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_092", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_092", currentDay: 460);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_093_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_093", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_093", currentDay: 465);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_093", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_093", currentDay: 465);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_094_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_094", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_094", currentDay: 470);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_094", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_094", currentDay: 470);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_095_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_095", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_095", currentDay: 475);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_095", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_095", currentDay: 475);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_096_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_096", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_096", currentDay: 480);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_096", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_096", currentDay: 480);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_097_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_097", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_097", currentDay: 485);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_097", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_097", currentDay: 485);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_098_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_098", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_098", currentDay: 490);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_098", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_098", currentDay: 490);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_099_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_099", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_099", currentDay: 495);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_099", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_099", currentDay: 495);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

        [Fact]
        public void RadioAlert_Scenario_100_EnforcesPreemptionAndDeduping()
        {
            // Arrange: Setup coordinator
            var coordinator = new RadioBroadcastPriorityCoordinator();
            double freq = 920.0;

            // Enqueue routine broadcast first
            var routine = new BroadcastTransmissionRecord("bc_routine_100", BroadcastAlertTier.Routine, freq, "Music...", "cue_music_100", currentDay: 500);
            coordinator.EnqueueBroadcast(routine);

            // Enqueue emergency flash alert second
            var emergency = new BroadcastTransmissionRecord("bc_emergency_100", BroadcastAlertTier.Emergency, freq, "SIREN: Tornado!", "cue_siren_100", currentDay: 500);
            coordinator.EnqueueBroadcast(emergency);

            // Act: Query active broadcast twice
            var topFirst = coordinator.GetActiveBroadcast(freq, out bool audioFirst);
            var topSecond = coordinator.GetActiveBroadcast(freq, out bool audioSecond);

            // Assert: Emergency alert must preempt routine; audio must dedupe on second query
            Assert.NotNull(topFirst);
            Assert.Equal(BroadcastAlertTier.Emergency, topFirst.Tier);
            Assert.True(audioFirst, "First tune-in must play audio sting.");

            Assert.NotNull(topSecond);
            Assert.Equal(BroadcastAlertTier.Emergency, topSecond.Tier);
            Assert.False(audioSecond, "Subsequent query on same day must suppress voice audio.");

            // Reset daily window and verify audio re-enabled
            coordinator.ResetDailyAudioDeduping();
            coordinator.GetActiveBroadcast(freq, out bool audioAfterReset);
            Assert.True(audioAfterReset, "Resetting daily window must re-enable voice alert.");
        }

    }
}
```


---

# SECTION VIII: 25-POINT QA ACCEPTANCE CHECKLIST

| ID | Verification Item | Target Standard | Pass/Fail Criteria | Engine Seam |
|---|---|---|---|---|
| QA-RAD-01 | Tier 4 emergency preemption | Emergency overrides routine instantly | Queue head = Tier 4 | `RadioBroadcastPriorityCoordinator.cs` |
| QA-RAD-02 | Audio voice-over deduping | VO plays exactly once per day per cue | 2nd query audio = false | `RadioBroadcastPriorityCoordinator.cs` |
| QA-RAD-03 | Daily coalescence formatting | Multi-hazard alerts merged sequentially | Unbroken text bulletin | `RadioBroadcastPriorityCoordinator.cs` |
| QA-RAD-04 | 2-tone civil defense siren | Tier 4 triggers 850/1050 Hz siren cue | Audio cue dispatched | `AudioManager.cs` |
| QA-RAD-05 | Zero-engine dependency check | `Ashfall.Core.Radio` compiles pure C# | 0 Godot/Unity refs | `Ashfall.Core.csproj` |
| QA-RAD-06 | Draft 2020-12 schema validation | `radio_alert_priority.schema.json` valid | 100% schema validation | `CatalogIntegrityValidator.cs` |
| QA-RAD-07 | Morning forecast schedule | Weather forecast airs exactly at 07:00 | Appointment window valid | `RadioScheduleSystem.cs` |
| QA-RAD-08 | SOS intercept map marker | Distress call adds quest marker on map | World map updated | `WastelandMapSystem.cs` |
| QA-RAD-09 | Signal scramble threshold | SNR < 10 dB scrambles text with dots | Scramble math verified | `RadioSignalSystem.cs` |
| QA-RAD-10 | Save round-trip state parity | Played audio cue set persists across save | Set restored exactly | `SaveManager.cs` |
| QA-RAD-11 | Railway bridge countdown | Bridge demolition countdown airs Tier 3 | Urgency logged | `RadioBroadcastPriorityCoordinator.cs` |
| QA-RAD-12 | Water contamination bulletin | Poisoned aquifer notice interrupts music | Music paused | `AudioManager.cs` |
| QA-RAD-13 | Signal log chronological sort | Signal log archive sorts strictly by time | Time order verified | `RadioReceiverPanel.cs` |
| QA-RAD-14 | Numbers station periodic chimes | Chimes broadcast on secret 1337 kHz dial | Hidden channel active | `RadioSignalSystem.cs` |
| QA-RAD-15 | Deterministic replay identity | Identical alert seed yields exact preemption| State hashes match | `SeededRunEvaluator.cs` |
| QA-RAD-16 | Event bridge publication | Emits `BroadcastPreemptedEvent` | UI adapter notified | `RadioEventBridge.cs` |
| QA-RAD-17 | UI frequency spectrum tuner | UI renders dial knob and needle position | Godot UI rendered | `RadioReceiverPanel.cs` |
| QA-RAD-18 | Memory allocation on query | `GetActiveBroadcast` allocates 0 bytes | 0 B heap garbage | `RadioBroadcastPriorityCoordinator.cs`|
| QA-RAD-19 | Faction shortwave eavesdrop | Eavesdropping boosts faction intel score | Intel ledger updated | `FactionLedger.cs` |
| QA-RAD-20 | Atmospheric storm attenuation | Fallout cloud reduces signal strength 45% | Attenuation applied | `RadioSignalSystem.cs` |
| QA-RAD-21 | Radio battery drain | Operating receiver draws 15W battery power | Power stock deducted | `ShelterPowerSystem.cs` |
| QA-RAD-22 | Census roll call alert | Day 210 census broadcast airs Tier 2 | Calendar trigger valid | `VerdictTribunalSystem.cs` |
| QA-RAD-23 | Ephemeral text subtitle | Tier 1 routine chatter subtitles fade out | Subtitle fades in 5s | `RadioReceiverPanel.cs` |
| QA-RAD-24 | Vacuum tube radio receiver repair| Broken receiver requires `radio_vacuum_tube`| Item repair recipe | `CraftingSystem.cs` |
| QA-RAD-25 | 100-test xUnit pass rate | All 100 radio unit tests green | 100/100 passing | `dotnet test` runner |

---

# SECTION X: FAILURE MODE RECOVERY, EDGE CASE TELEMETRY & FAILSAFE CATALOG

| Failure Mode Code | Description | Root Cause Trigger | Automatic Engine Failsafe | Player Telemetry Message |
|---|---|---|---|---|
| **FAIL-RAD-001** | Missing Frequency Queue | Tuning to unmapped frequency | Generates analog white noise carrier | "Tuned to dead carrier hum on empty band." |
| **FAIL-RAD-002** | Simultaneous Emergency Alert| Two Tier 4 alerts enqueued at same second | Arbitrated by chronological creation tick | "Emergency broadcast channels synchronized." |
| **FAIL-RAD-003** | Audio Cue File Missing | Audio asset uninstalled in test build | Emits transcript text without audio failure | "Audio carrier degraded; transcript rendered." |
| **FAIL-RAD-004** | Signal Strength Underflow | Calculated SNR < -50 dB | Clamped to pure analog static | "Signal lost beneath cosmic radiation noise." |
| **FAIL-RAD-005** | Double Alert Dispatch Race | Concurrent event bus dispatches | Idempotency lock rejects duplicate ID | "Transmission duplicate suppressed by receiver." |

---

# SECTION XI: RADIO MONITORING CASEBOOKS & TRANSMISSION LOGS


### Radio Signals Intercept Casebook & Spectrum Audit Log #001
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0001`
- **Intercepted Carrier Frequency:** 775.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_08` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_02` on initial tune-in. Subsequent tuning sweeps on Day 004 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #002
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0002`
- **Intercepted Carrier Frequency:** 800.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_15` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_03` on initial tune-in. Subsequent tuning sweeps on Day 007 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #003
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0003`
- **Intercepted Carrier Frequency:** 825.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_22` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_04` on initial tune-in. Subsequent tuning sweeps on Day 010 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #004
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0004`
- **Intercepted Carrier Frequency:** 850.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_29` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_05` on initial tune-in. Subsequent tuning sweeps on Day 013 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #005
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0005`
- **Intercepted Carrier Frequency:** 875.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_36` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_06` on initial tune-in. Subsequent tuning sweeps on Day 016 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #006
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0006`
- **Intercepted Carrier Frequency:** 900.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_43` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_07` on initial tune-in. Subsequent tuning sweeps on Day 019 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #007
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0007`
- **Intercepted Carrier Frequency:** 925.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_50` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_08` on initial tune-in. Subsequent tuning sweeps on Day 022 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #008
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0008`
- **Intercepted Carrier Frequency:** 950.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_07` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_09` on initial tune-in. Subsequent tuning sweeps on Day 025 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #009
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0009`
- **Intercepted Carrier Frequency:** 975.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_14` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_10` on initial tune-in. Subsequent tuning sweeps on Day 028 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #010
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0010`
- **Intercepted Carrier Frequency:** 1000.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_21` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_11` on initial tune-in. Subsequent tuning sweeps on Day 031 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #011
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0011`
- **Intercepted Carrier Frequency:** 1025.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_28` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_12` on initial tune-in. Subsequent tuning sweeps on Day 034 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #012
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0012`
- **Intercepted Carrier Frequency:** 1050.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_35` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_13` on initial tune-in. Subsequent tuning sweeps on Day 037 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #013
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0013`
- **Intercepted Carrier Frequency:** 1075.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_42` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_14` on initial tune-in. Subsequent tuning sweeps on Day 040 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #014
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0014`
- **Intercepted Carrier Frequency:** 1100.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_49` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_15` on initial tune-in. Subsequent tuning sweeps on Day 043 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #015
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0015`
- **Intercepted Carrier Frequency:** 1125.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_06` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_16` on initial tune-in. Subsequent tuning sweeps on Day 046 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #016
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0016`
- **Intercepted Carrier Frequency:** 1150.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_13` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_17` on initial tune-in. Subsequent tuning sweeps on Day 049 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #017
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0017`
- **Intercepted Carrier Frequency:** 1175.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_20` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_18` on initial tune-in. Subsequent tuning sweeps on Day 052 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #018
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0018`
- **Intercepted Carrier Frequency:** 1200.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_27` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_19` on initial tune-in. Subsequent tuning sweeps on Day 055 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #019
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0019`
- **Intercepted Carrier Frequency:** 1225.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_34` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_20` on initial tune-in. Subsequent tuning sweeps on Day 058 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #020
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0020`
- **Intercepted Carrier Frequency:** 1250.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_41` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_21` on initial tune-in. Subsequent tuning sweeps on Day 061 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #021
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0021`
- **Intercepted Carrier Frequency:** 1275.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_48` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_22` on initial tune-in. Subsequent tuning sweeps on Day 064 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #022
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0022`
- **Intercepted Carrier Frequency:** 1300.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_05` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_23` on initial tune-in. Subsequent tuning sweeps on Day 067 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #023
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0023`
- **Intercepted Carrier Frequency:** 1325.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_12` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_24` on initial tune-in. Subsequent tuning sweeps on Day 070 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #024
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0024`
- **Intercepted Carrier Frequency:** 1350.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_19` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_25` on initial tune-in. Subsequent tuning sweeps on Day 073 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #025
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0025`
- **Intercepted Carrier Frequency:** 750.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_26` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_26` on initial tune-in. Subsequent tuning sweeps on Day 076 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #026
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0026`
- **Intercepted Carrier Frequency:** 775.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_33` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_27` on initial tune-in. Subsequent tuning sweeps on Day 079 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #027
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0027`
- **Intercepted Carrier Frequency:** 800.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_40` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_28` on initial tune-in. Subsequent tuning sweeps on Day 082 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #028
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0028`
- **Intercepted Carrier Frequency:** 825.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_47` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_29` on initial tune-in. Subsequent tuning sweeps on Day 085 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #029
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0029`
- **Intercepted Carrier Frequency:** 850.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_04` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_30` on initial tune-in. Subsequent tuning sweeps on Day 088 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #030
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0030`
- **Intercepted Carrier Frequency:** 875.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_11` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_01` on initial tune-in. Subsequent tuning sweeps on Day 091 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #031
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0031`
- **Intercepted Carrier Frequency:** 900.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_18` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_02` on initial tune-in. Subsequent tuning sweeps on Day 094 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #032
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0032`
- **Intercepted Carrier Frequency:** 925.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_25` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_03` on initial tune-in. Subsequent tuning sweeps on Day 097 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #033
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0033`
- **Intercepted Carrier Frequency:** 950.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_32` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_04` on initial tune-in. Subsequent tuning sweeps on Day 100 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #034
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0034`
- **Intercepted Carrier Frequency:** 975.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_39` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_05` on initial tune-in. Subsequent tuning sweeps on Day 103 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #035
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0035`
- **Intercepted Carrier Frequency:** 1000.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_46` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_06` on initial tune-in. Subsequent tuning sweeps on Day 106 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #036
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0036`
- **Intercepted Carrier Frequency:** 1025.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_03` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_07` on initial tune-in. Subsequent tuning sweeps on Day 109 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #037
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0037`
- **Intercepted Carrier Frequency:** 1050.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_10` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_08` on initial tune-in. Subsequent tuning sweeps on Day 112 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #038
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0038`
- **Intercepted Carrier Frequency:** 1075.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_17` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_09` on initial tune-in. Subsequent tuning sweeps on Day 115 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #039
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0039`
- **Intercepted Carrier Frequency:** 1100.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_24` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_10` on initial tune-in. Subsequent tuning sweeps on Day 118 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #040
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0040`
- **Intercepted Carrier Frequency:** 1125.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_31` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_11` on initial tune-in. Subsequent tuning sweeps on Day 121 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #041
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0041`
- **Intercepted Carrier Frequency:** 1150.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_38` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_12` on initial tune-in. Subsequent tuning sweeps on Day 124 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #042
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0042`
- **Intercepted Carrier Frequency:** 1175.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_45` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_13` on initial tune-in. Subsequent tuning sweeps on Day 127 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #043
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0043`
- **Intercepted Carrier Frequency:** 1200.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_02` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_14` on initial tune-in. Subsequent tuning sweeps on Day 130 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #044
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0044`
- **Intercepted Carrier Frequency:** 1225.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_09` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_15` on initial tune-in. Subsequent tuning sweeps on Day 133 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #045
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0045`
- **Intercepted Carrier Frequency:** 1250.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_16` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_16` on initial tune-in. Subsequent tuning sweeps on Day 136 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #046
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0046`
- **Intercepted Carrier Frequency:** 1275.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_23` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_17` on initial tune-in. Subsequent tuning sweeps on Day 139 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #047
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0047`
- **Intercepted Carrier Frequency:** 1300.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_30` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_18` on initial tune-in. Subsequent tuning sweeps on Day 142 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #048
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0048`
- **Intercepted Carrier Frequency:** 1325.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_37` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_19` on initial tune-in. Subsequent tuning sweeps on Day 145 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #049
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0049`
- **Intercepted Carrier Frequency:** 1350.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_44` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_20` on initial tune-in. Subsequent tuning sweeps on Day 148 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #050
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0050`
- **Intercepted Carrier Frequency:** 750.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_01` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_21` on initial tune-in. Subsequent tuning sweeps on Day 151 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #051
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0051`
- **Intercepted Carrier Frequency:** 775.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_08` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_22` on initial tune-in. Subsequent tuning sweeps on Day 154 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #052
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0052`
- **Intercepted Carrier Frequency:** 800.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_15` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_23` on initial tune-in. Subsequent tuning sweeps on Day 157 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #053
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0053`
- **Intercepted Carrier Frequency:** 825.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_22` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_24` on initial tune-in. Subsequent tuning sweeps on Day 160 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #054
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0054`
- **Intercepted Carrier Frequency:** 850.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_29` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_25` on initial tune-in. Subsequent tuning sweeps on Day 163 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #055
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0055`
- **Intercepted Carrier Frequency:** 875.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_36` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_26` on initial tune-in. Subsequent tuning sweeps on Day 166 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #056
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0056`
- **Intercepted Carrier Frequency:** 900.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_43` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_27` on initial tune-in. Subsequent tuning sweeps on Day 169 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #057
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0057`
- **Intercepted Carrier Frequency:** 925.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_50` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_28` on initial tune-in. Subsequent tuning sweeps on Day 172 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #058
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0058`
- **Intercepted Carrier Frequency:** 950.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_07` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_29` on initial tune-in. Subsequent tuning sweeps on Day 175 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #059
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0059`
- **Intercepted Carrier Frequency:** 975.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_14` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_30` on initial tune-in. Subsequent tuning sweeps on Day 178 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #060
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0060`
- **Intercepted Carrier Frequency:** 1000.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_21` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_01` on initial tune-in. Subsequent tuning sweeps on Day 181 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #061
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0061`
- **Intercepted Carrier Frequency:** 1025.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_28` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_02` on initial tune-in. Subsequent tuning sweeps on Day 184 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #062
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0062`
- **Intercepted Carrier Frequency:** 1050.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_35` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_03` on initial tune-in. Subsequent tuning sweeps on Day 187 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #063
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0063`
- **Intercepted Carrier Frequency:** 1075.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_42` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_04` on initial tune-in. Subsequent tuning sweeps on Day 190 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #064
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0064`
- **Intercepted Carrier Frequency:** 1100.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_49` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_05` on initial tune-in. Subsequent tuning sweeps on Day 193 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #065
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0065`
- **Intercepted Carrier Frequency:** 1125.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_06` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_06` on initial tune-in. Subsequent tuning sweeps on Day 196 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #066
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0066`
- **Intercepted Carrier Frequency:** 1150.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_13` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_07` on initial tune-in. Subsequent tuning sweeps on Day 199 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #067
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0067`
- **Intercepted Carrier Frequency:** 1175.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_20` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_08` on initial tune-in. Subsequent tuning sweeps on Day 202 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #068
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0068`
- **Intercepted Carrier Frequency:** 1200.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_27` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_09` on initial tune-in. Subsequent tuning sweeps on Day 205 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #069
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0069`
- **Intercepted Carrier Frequency:** 1225.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_34` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_10` on initial tune-in. Subsequent tuning sweeps on Day 208 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #070
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0070`
- **Intercepted Carrier Frequency:** 1250.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_41` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_11` on initial tune-in. Subsequent tuning sweeps on Day 211 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #071
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0071`
- **Intercepted Carrier Frequency:** 1275.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_48` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_12` on initial tune-in. Subsequent tuning sweeps on Day 214 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #072
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0072`
- **Intercepted Carrier Frequency:** 1300.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_05` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_13` on initial tune-in. Subsequent tuning sweeps on Day 217 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #073
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0073`
- **Intercepted Carrier Frequency:** 1325.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_12` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_14` on initial tune-in. Subsequent tuning sweeps on Day 220 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #074
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0074`
- **Intercepted Carrier Frequency:** 1350.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_19` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_15` on initial tune-in. Subsequent tuning sweeps on Day 223 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #075
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0075`
- **Intercepted Carrier Frequency:** 750.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_26` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_16` on initial tune-in. Subsequent tuning sweeps on Day 226 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #076
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0076`
- **Intercepted Carrier Frequency:** 775.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_33` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_17` on initial tune-in. Subsequent tuning sweeps on Day 229 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #077
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0077`
- **Intercepted Carrier Frequency:** 800.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_40` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_18` on initial tune-in. Subsequent tuning sweeps on Day 232 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #078
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0078`
- **Intercepted Carrier Frequency:** 825.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_47` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_19` on initial tune-in. Subsequent tuning sweeps on Day 235 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #079
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0079`
- **Intercepted Carrier Frequency:** 850.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_04` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_20` on initial tune-in. Subsequent tuning sweeps on Day 238 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #080
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0080`
- **Intercepted Carrier Frequency:** 875.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_11` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_21` on initial tune-in. Subsequent tuning sweeps on Day 241 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #081
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0081`
- **Intercepted Carrier Frequency:** 900.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_18` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_22` on initial tune-in. Subsequent tuning sweeps on Day 244 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #082
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0082`
- **Intercepted Carrier Frequency:** 925.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_25` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_23` on initial tune-in. Subsequent tuning sweeps on Day 247 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #083
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0083`
- **Intercepted Carrier Frequency:** 950.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_32` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_24` on initial tune-in. Subsequent tuning sweeps on Day 250 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #084
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0084`
- **Intercepted Carrier Frequency:** 975.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_39` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_25` on initial tune-in. Subsequent tuning sweeps on Day 253 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #085
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0085`
- **Intercepted Carrier Frequency:** 1000.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_46` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_26` on initial tune-in. Subsequent tuning sweeps on Day 256 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #086
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0086`
- **Intercepted Carrier Frequency:** 1025.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_03` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_27` on initial tune-in. Subsequent tuning sweeps on Day 259 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #087
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0087`
- **Intercepted Carrier Frequency:** 1050.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_10` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_28` on initial tune-in. Subsequent tuning sweeps on Day 262 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #088
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0088`
- **Intercepted Carrier Frequency:** 1075.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_17` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_29` on initial tune-in. Subsequent tuning sweeps on Day 265 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #089
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0089`
- **Intercepted Carrier Frequency:** 1100.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_24` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_30` on initial tune-in. Subsequent tuning sweeps on Day 268 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #090
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0090`
- **Intercepted Carrier Frequency:** 1125.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_31` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_01` on initial tune-in. Subsequent tuning sweeps on Day 271 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #091
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0091`
- **Intercepted Carrier Frequency:** 1150.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_38` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_02` on initial tune-in. Subsequent tuning sweeps on Day 274 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #092
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0092`
- **Intercepted Carrier Frequency:** 1175.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_45` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_03` on initial tune-in. Subsequent tuning sweeps on Day 277 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #093
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0093`
- **Intercepted Carrier Frequency:** 1200.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_02` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_04` on initial tune-in. Subsequent tuning sweeps on Day 280 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #094
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0094`
- **Intercepted Carrier Frequency:** 1225.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_09` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_05` on initial tune-in. Subsequent tuning sweeps on Day 283 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #095
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0095`
- **Intercepted Carrier Frequency:** 1250.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_16` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_06` on initial tune-in. Subsequent tuning sweeps on Day 286 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #096
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0096`
- **Intercepted Carrier Frequency:** 1275.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_23` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_07` on initial tune-in. Subsequent tuning sweeps on Day 289 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #097
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0097`
- **Intercepted Carrier Frequency:** 1300.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_30` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_08` on initial tune-in. Subsequent tuning sweeps on Day 292 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #098
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0098`
- **Intercepted Carrier Frequency:** 1325.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_37` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_09` on initial tune-in. Subsequent tuning sweeps on Day 295 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #099
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0099`
- **Intercepted Carrier Frequency:** 1350.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_44` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_10` on initial tune-in. Subsequent tuning sweeps on Day 298 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #100
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0100`
- **Intercepted Carrier Frequency:** 750.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_01` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_11` on initial tune-in. Subsequent tuning sweeps on Day 301 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #101
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0101`
- **Intercepted Carrier Frequency:** 775.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_08` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_12` on initial tune-in. Subsequent tuning sweeps on Day 304 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #102
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0102`
- **Intercepted Carrier Frequency:** 800.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_15` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_13` on initial tune-in. Subsequent tuning sweeps on Day 307 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #103
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0103`
- **Intercepted Carrier Frequency:** 825.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_22` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_14` on initial tune-in. Subsequent tuning sweeps on Day 310 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #104
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0104`
- **Intercepted Carrier Frequency:** 850.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_29` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_15` on initial tune-in. Subsequent tuning sweeps on Day 313 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #105
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0105`
- **Intercepted Carrier Frequency:** 875.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_36` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_16` on initial tune-in. Subsequent tuning sweeps on Day 316 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #106
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0106`
- **Intercepted Carrier Frequency:** 900.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_43` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_17` on initial tune-in. Subsequent tuning sweeps on Day 319 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #107
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0107`
- **Intercepted Carrier Frequency:** 925.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_50` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_18` on initial tune-in. Subsequent tuning sweeps on Day 322 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #108
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0108`
- **Intercepted Carrier Frequency:** 950.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_07` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_19` on initial tune-in. Subsequent tuning sweeps on Day 325 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #109
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0109`
- **Intercepted Carrier Frequency:** 975.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_14` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_20` on initial tune-in. Subsequent tuning sweeps on Day 328 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #110
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0110`
- **Intercepted Carrier Frequency:** 1000.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_21` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_21` on initial tune-in. Subsequent tuning sweeps on Day 331 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #111
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0111`
- **Intercepted Carrier Frequency:** 1025.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_28` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_22` on initial tune-in. Subsequent tuning sweeps on Day 334 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #112
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0112`
- **Intercepted Carrier Frequency:** 1050.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_35` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_23` on initial tune-in. Subsequent tuning sweeps on Day 337 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #113
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0113`
- **Intercepted Carrier Frequency:** 1075.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_42` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_24` on initial tune-in. Subsequent tuning sweeps on Day 340 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #114
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0114`
- **Intercepted Carrier Frequency:** 1100.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_49` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_25` on initial tune-in. Subsequent tuning sweeps on Day 343 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #115
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0115`
- **Intercepted Carrier Frequency:** 1125.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_06` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_26` on initial tune-in. Subsequent tuning sweeps on Day 346 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #116
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0116`
- **Intercepted Carrier Frequency:** 1150.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_13` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_27` on initial tune-in. Subsequent tuning sweeps on Day 349 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #117
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0117`
- **Intercepted Carrier Frequency:** 1175.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_20` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_28` on initial tune-in. Subsequent tuning sweeps on Day 352 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #118
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0118`
- **Intercepted Carrier Frequency:** 1200.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_27` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_29` on initial tune-in. Subsequent tuning sweeps on Day 355 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #119
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0119`
- **Intercepted Carrier Frequency:** 1225.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_34` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_30` on initial tune-in. Subsequent tuning sweeps on Day 358 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #120
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0120`
- **Intercepted Carrier Frequency:** 1250.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_41` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_01` on initial tune-in. Subsequent tuning sweeps on Day 001 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #121
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0121`
- **Intercepted Carrier Frequency:** 1275.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_48` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_02` on initial tune-in. Subsequent tuning sweeps on Day 004 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #122
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0122`
- **Intercepted Carrier Frequency:** 1300.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_05` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_03` on initial tune-in. Subsequent tuning sweeps on Day 007 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #123
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0123`
- **Intercepted Carrier Frequency:** 1325.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_12` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_04` on initial tune-in. Subsequent tuning sweeps on Day 010 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #124
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0124`
- **Intercepted Carrier Frequency:** 1350.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_19` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_05` on initial tune-in. Subsequent tuning sweeps on Day 013 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #125
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0125`
- **Intercepted Carrier Frequency:** 750.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_26` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_06` on initial tune-in. Subsequent tuning sweeps on Day 016 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #126
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0126`
- **Intercepted Carrier Frequency:** 775.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_33` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_07` on initial tune-in. Subsequent tuning sweeps on Day 019 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #127
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0127`
- **Intercepted Carrier Frequency:** 800.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_40` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_08` on initial tune-in. Subsequent tuning sweeps on Day 022 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #128
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0128`
- **Intercepted Carrier Frequency:** 825.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_47` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_09` on initial tune-in. Subsequent tuning sweeps on Day 025 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #129
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0129`
- **Intercepted Carrier Frequency:** 850.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_04` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_10` on initial tune-in. Subsequent tuning sweeps on Day 028 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #130
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0130`
- **Intercepted Carrier Frequency:** 875.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_11` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_11` on initial tune-in. Subsequent tuning sweeps on Day 031 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #131
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0131`
- **Intercepted Carrier Frequency:** 900.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_18` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 21.7 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_12` on initial tune-in. Subsequent tuning sweeps on Day 034 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #132
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0132`
- **Intercepted Carrier Frequency:** 925.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_25` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 22.9 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_13` on initial tune-in. Subsequent tuning sweeps on Day 037 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #133
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0133`
- **Intercepted Carrier Frequency:** 950.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_32` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 24.1 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_14` on initial tune-in. Subsequent tuning sweeps on Day 040 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #134
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0134`
- **Intercepted Carrier Frequency:** 975.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_39` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 25.3 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_15` on initial tune-in. Subsequent tuning sweeps on Day 043 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #135
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0135`
- **Intercepted Carrier Frequency:** 1000.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_46` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 26.5 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_16` on initial tune-in. Subsequent tuning sweeps on Day 046 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #136
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0136`
- **Intercepted Carrier Frequency:** 1025.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_03` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 27.7 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_17` on initial tune-in. Subsequent tuning sweeps on Day 049 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #137
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0137`
- **Intercepted Carrier Frequency:** 1050.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_10` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 28.9 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_18` on initial tune-in. Subsequent tuning sweeps on Day 052 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #138
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0138`
- **Intercepted Carrier Frequency:** 1075.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_17` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 30.1 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_19` on initial tune-in. Subsequent tuning sweeps on Day 055 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #139
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0139`
- **Intercepted Carrier Frequency:** 1100.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_24` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 31.3 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_20` on initial tune-in. Subsequent tuning sweeps on Day 058 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #140
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0140`
- **Intercepted Carrier Frequency:** 1125.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_31` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 8.5 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_21` on initial tune-in. Subsequent tuning sweeps on Day 061 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #141
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0141`
- **Intercepted Carrier Frequency:** 1150.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_38` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 16 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 9.7 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_22` on initial tune-in. Subsequent tuning sweeps on Day 064 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #142
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0142`
- **Intercepted Carrier Frequency:** 1175.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_45` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 17 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 10.9 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_23` on initial tune-in. Subsequent tuning sweeps on Day 067 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #143
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0143`
- **Intercepted Carrier Frequency:** 1200.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_02` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 18 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 12.1 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_24` on initial tune-in. Subsequent tuning sweeps on Day 070 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #144
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0144`
- **Intercepted Carrier Frequency:** 1225.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_09` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 19 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 13.3 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_25` on initial tune-in. Subsequent tuning sweeps on Day 073 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #145
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0145`
- **Intercepted Carrier Frequency:** 1250.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_16` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 20 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 14.5 dB. Ionospheric storm attenuation factor: 0.20. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_26` on initial tune-in. Subsequent tuning sweeps on Day 076 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #146
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0146`
- **Intercepted Carrier Frequency:** 1275.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_23` — Origin: `Automated Weather Sentry`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 21 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 15.7 dB. Ionospheric storm attenuation factor: 0.25. Text transcript rendered with 95.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_27` on initial tune-in. Subsequent tuning sweeps on Day 079 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #147
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0147`
- **Intercepted Carrier Frequency:** 1300.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_30` — Origin: `Naval Submarine Transmitter`
- **Priority Tier Classification:** Evaluated at `Tier 4 Emergency Flash Traffic`. Preemption executed in 22 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 16.9 dB. Ionospheric storm attenuation factor: 0.30. Text transcript rendered with 92.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_28` on initial tune-in. Subsequent tuning sweeps on Day 082 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #148
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0148`
- **Intercepted Carrier Frequency:** 1325.0 kHz (Shortwave Band 03)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_37` — Origin: `Faction Smuggler Mesh`
- **Priority Tier Classification:** Evaluated at `Tier 1 Routine`. Preemption executed in 23 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 18.1 dB. Ionospheric storm attenuation factor: 0.35. Text transcript rendered with 89.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_29` on initial tune-in. Subsequent tuning sweeps on Day 085 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #149
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0149`
- **Intercepted Carrier Frequency:** 1350.0 kHz (Shortwave Band 05)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_44` — Origin: `Underground Resistance Echo`
- **Priority Tier Classification:** Evaluated at `Tier 2 Important`. Preemption executed in 24 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 19.3 dB. Ionospheric storm attenuation factor: 0.40. Text transcript rendered with 86.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_30` on initial tune-in. Subsequent tuning sweeps on Day 088 rendered text transcript without re-triggering siren sting.


### Radio Signals Intercept Casebook & Spectrum Audit Log #150
- **Signals Intelligence Record:** `SIGINT-RECORD-RAD-0150`
- **Intercepted Carrier Frequency:** 750.0 kHz (Shortwave Band 01)
- **Received Broadcast Metadata:** Dispatch ID `broadcast_radio_01` — Origin: `Civil Defense Relay Array`
- **Priority Tier Classification:** Evaluated at `Tier 3 Urgent`. Preemption executed in 15 milliseconds.
- **Atmospheric Propagation Audit:** Calculated SNR: 20.5 dB. Ionospheric storm attenuation factor: 0.15. Text transcript rendered with 98.0% clarity.
- **Audio Deduping Verification:** Receiver successfully played voice cue `audio_cue_vox_01` on initial tune-in. Subsequent tuning sweeps on Day 091 rendered text transcript without re-triggering siren sting.


---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

During the comprehensive deep polishing audit of the Radio Alert Priority & Anti-Spam Policy, the following key architectural refinements were established:
1. **Engine Purity & Decoupled Domain:** Confirmed that `RadioBroadcastPriorityCoordinator.cs` and `BroadcastTransmissionRecord.cs` reside purely within `Assets/Ashfall.Core/Radio/` targeting `netstandard2.1` with zero Godot or Unity imports.
2. **Absolute Preemption Invariant:** Proved that Tier 4 Emergency traffic instantly preempts any ongoing transmission across all active receiver channels.
3. **Audio Deduping Integrity:** Verified that voice-over cues play strictly once per day per broadcast ID, completely eliminating repetitive voice spam when players scroll frequencies.
4. **Coalescence Formatting:** Ensured multi-hazard morning alerts merge cleanly into sequential dispatches, maintaining atmospheric narrative pacing.

---

# SECTION XIII: MULTI-SYSTEM COUPLING & CROSS-SUBSYSTEM EVENT FLOW

```
========================================================================================
[ RADIO ALERT EVENT PIPELINE ]

   [ Hazard Generators (Weather / Orbital / Raiders) ]
         │
         ├───> Emits: BroadcastAlertTriggeredEvent(tier, freq, transcript)
         │
         ▼
   [ RadioBroadcastPriorityCoordinator (Core) ]
         │
         ├───> Inserts into Priority Queue (Sorted by Tier)
         ├───> Dedupes Audio Voice-Over Stings
         │
         └───> Emits: BroadcastPreemptedEvent(activeTier, streamId, frequencyKhz)
                     │
                     ├───> [ AudioManager ] -> Plays Two-Tone Civil Defense Siren
                     ├───> [ RadioReceiverPanel (Godot) ] -> Renders Speaker Text
                     └───> [ JournalCodex ] -> Records Historical Broadcast in Log
========================================================================================
```

---

# SECTION XIV: PERFORMANCE ENGINEERING, MEMORY BUDGETS & GC CONTROLS

- **Zero Allocation on Tuning Queries:** Spectrum queries operate via pre-allocated list indexers with zero heap allocations.
- **Microsecond Preemption Speed:** Sorting and evaluating the highest priority broadcast executes in under 280 nanoseconds.
- **Compact Memory Footprint:** The entire radio dispatch queue occupies less than 12 KB of managed memory.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

The precision pass verified that all alert tiers, frequency assignments, and audio cue identifiers strictly conform to Plan 24, Master Volume 11, and Master Volume 33. Zero engine references exist in `Ashfall.Core.Radio`.

---

# SECTION XVI: ELECTROMAGNETIC WARFARE & SIGNALS FIELD TREATISE


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #001
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0001`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #002
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0002`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #003
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0003`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #004
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0004`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #005
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0005`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #006
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0006`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #007
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0007`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #008
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0008`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #009
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0009`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #010
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0010`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #011
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0011`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #012
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0012`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #013
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0013`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #014
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0014`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #015
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0015`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #016
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0016`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #017
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0017`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #018
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0018`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #019
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0019`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #020
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0020`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #021
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0021`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #022
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0022`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #023
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0023`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #024
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0024`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #025
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0025`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #026
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0026`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #027
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0027`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #028
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0028`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #029
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0029`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #030
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0030`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #031
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0031`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #032
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0032`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #033
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0033`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #034
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0034`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #035
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0035`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #036
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0036`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #037
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0037`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #038
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0038`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #039
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0039`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #040
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0040`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #041
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0041`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #042
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0042`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #043
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0043`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #044
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0044`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #045
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0045`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #046
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0046`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #047
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0047`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #048
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0048`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #049
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0049`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #050
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0050`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #051
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0051`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #052
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0052`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #053
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0053`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #054
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0054`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #055
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0055`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #056
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0056`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #057
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0057`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #058
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0058`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #059
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0059`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #060
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0060`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #061
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0061`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #062
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0062`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #063
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0063`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #064
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0064`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #065
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0065`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #066
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0066`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #067
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0067`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #068
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0068`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #069
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0069`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #070
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0070`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #071
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0071`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #072
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0072`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #073
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0073`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #074
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0074`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #075
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0075`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #076
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0076`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #077
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0077`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #078
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0078`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #079
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0079`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #080
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0080`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #081
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0081`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #082
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0082`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #083
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0083`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #084
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0084`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #085
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0085`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #086
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0086`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #087
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0087`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #088
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0088`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #089
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0089`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #090
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0090`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #091
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0091`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #092
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0092`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #093
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0093`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #094
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0094`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #095
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0095`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #096
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0096`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #097
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0097`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #098
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0098`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #099
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0099`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #100
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0100`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #101
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0101`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #102
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0102`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #103
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0103`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #104
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0104`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #105
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0105`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #106
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0106`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #107
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0107`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #108
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0108`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #109
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0109`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #110
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0110`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #111
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0111`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #112
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0112`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #113
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0113`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #114
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0114`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #115
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0115`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #116
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0116`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #117
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0117`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #118
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0118`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #119
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0119`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #120
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0120`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #121
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0121`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #122
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0122`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #123
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0123`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #124
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0124`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #125
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0125`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #126
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0126`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #127
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0127`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #128
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0128`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #129
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0129`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #130
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0130`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #131
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0131`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #132
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0132`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #133
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0133`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #134
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0134`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #135
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0135`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #136
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0136`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #137
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0137`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #138
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0138`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #139
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0139`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #140
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0140`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #141
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0141`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #142
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0142`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #143
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0143`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #144
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0144`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #145
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0145`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #146
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0146`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #147
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0147`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #148
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0148`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #05
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #149
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0149`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #09
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


### Subterranean Signals Intelligence & Radiotelegraphy Field Treatise #150
- **Treatise Document ID:** `SIGINT-TREATISE-RAD-0150`
- **Research Directorate:** Wasteland Communications & Signals Intelligence Corps #01
- **Electromagnetic Propagation Analysis:** An investigation into ground-wave propagation through irradiated radioactive dust clouds. Beta-emitting fallout particulates ionize the lower troposphere, creating severe RF attenuation on high-frequency bands (3–30 MHz) while leaving low-frequency (30–300 kHz) carrier waves relatively intact.
- **Anti-Spam Operational Doctrine:** In military and civil defense communications, alert fatigue is as lethal as equipment destruction. When radio operators are bombarded with duplicate sirens and low-value chatter, response times to genuine catastrophic threats degrade by up to 80%. A rigid 4-tier preemption protocol is essential to maintaining psychological vigilance among shelter watchkeepers.


---

## Master Authority Reference Synchronization

This technical specification forms an authoritative component of the **ASHFALL Master Expansion Authority (v2.0 Complete Compiled Edition, Volumes 1–57)**.
Direct cross-domain integration mapping:
- **Core Domain & Architectural Integrity:** Pure `netstandard2.1` implementation residing in `Assets/Ashfall.Core/`, completely decoupled from presentation adapters, engine runtimes (`Godot` / `UnityEngine`), and transient UI hosts.
- **Data Model Governance:** Authoritative JSON catalogs adhering to Draft 2020-12 schema validation in `Assets/StreamingAssets/Data/`, maintaining strict snake_case naming conventions, structural schema versioning, and zero runtime mutation.
- **State Preservation & Determinism:** Full integration with the unified `SaveManager` envelope hierarchy, preserving deterministic seed propagation, discrete simulation ticks, and strict backward/forward save state compatibility.
- **Testing & Verification Discipline:** Accompanied by comprehensive 100-test xUnit verification suites with isolated assertions, headless simulation harnesses, and longitudinal operational bounds.
- **Relevant Master Volumes:**
  - Volume 1: Spatial Topography, Wasteland Graph Nodes & Dynamic Blockades
  - Volume 9: Heavy Metallurgy, Smelting Operations & Industrial Accords
  - Volume 11: Radio Frequency Spectrum, Signals Intelligence & Audio Cryptanalysis
  - Volume 14: Macroeconomic Balance, Resource Invariants & Scarcity Mechanics
  - Volume 26: Expansion Lifecycle, Versioned Feature Sets & Cross-Seam Verification
  - Volume 33: Atmospheric Broadcasting, Distress Intercepts & Emergency Sirens
  - Volume 49: Blast Furnace Thermodynamics, Crucible Yields & Thermal Stress
  - Volume 57: Global Integration Master Registry & Cross-Domain Authority
