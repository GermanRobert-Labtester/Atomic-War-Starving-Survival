using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin host read model for the faction radio view. The corpus and
    /// deterministic intercept selection stay in Ashfall.Core; this class only
    /// owns the current day, receiver history, and host notifications.
    /// </summary>
    public sealed class RadioHostSession
    : HostSessionBase{
        public const int DemoSeed = 2026;
        private const string CorpusFileName = "faction_radio_corpus.json";
        private readonly List<RadioIntercept> _history = new();
        private readonly HashSet<string> _playedBroadcastKeys = new();

        /// <summary>
        /// Static mapping from faction event categories to voice-over clip resource names.
        /// Only factions/categories with actual WAV assets in assets/audio/radio/ are mapped.
        /// Missing mappings fall back to radio static only (safe no-op for audio).
        /// </summary>
        private static readonly Dictionary<string, string> s_voiceOverMap = new()
        {
            { "vo_kind_parley", "vo_kind_parley" },
            { "vo_kind_hatch", "vo_kind_hatch" },
            { "vo_ch3_ash_road", "vo_ch3_ash_road" },
            { "vo_ch7_milband", "vo_ch7_milband" },
            { "vo_ch11_stockpile", "vo_ch11_stockpile" },
        };

        /// <summary>
        /// Fired when a new (non-duplicate) broadcast is intercepted.
        /// Carries the intercept and the resolved voice-over clip name (null if none).
        /// </summary>
        public event Action<RadioIntercept, string?>? BroadcastIntercepted;

        public FactionRadioEngine Engine { get; }
        public SignalTriangulationSystem Triangulation { get; }
        public ISeededRng Rng { get; }
        public IReadOnlyList<RadioIntercept> History => _history;
        public int Day { get; private set; }
        public float CurrentFrequency { get; private set; }
        public RadioIntercept? LastIntercept { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;
        public RadioHostSession(
            FactionRadioEngine engine,
            ISeededRng? rng = null,
            int day = 1,
            SignalTriangulationSystem? triangulation = null)
        {
            Engine = engine ?? new FactionRadioEngine();
            Triangulation = triangulation ?? new SignalTriangulationSystem();
            Rng = rng ?? new SeededRng(DemoSeed);
            Day = Math.Max(1, day);
            CurrentFrequency = FirstFrequency();
            Triangulation.OnStateChanged += _ => RaiseStateChanged();
            Triangulation.OnLocationRevealed += id => { LastEvent = $"Location discovered: {id}"; RaiseStateChanged(); };
        }

        public static RadioHostSession Create(string dataDir, int day = 1)
        {
            string path = Path.Combine(dataDir ?? string.Empty, CorpusFileName);
            if (!File.Exists(path))
                path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", CorpusFileName);

            string json = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
            var session = new RadioHostSession(FactionRadioEngine.LoadFromJson(json), new SeededRng(DemoSeed), day);
            session.Listen();
            // Persistence: a radio save (checksummed, user://) wins over fresh
            // state — history, played-broadcast dedup keys, and tuned frequency
            // all survive a reload. No save = fresh receiver (legacy fallback).
            var save = RadioSaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Radio state restored from save.";
            }
            return session;
        }

        public void SetDay(int day)
        {
            Day = Math.Max(1, day);
        }

        public string Listen(float? frequencyMhz = null)
        {
            if (frequencyMhz.HasValue)
                CurrentFrequency = frequencyMhz.Value;

            var intercept = Engine.GetBroadcastAtFrequency(CurrentFrequency, Day, Rng);
            LastIntercept = intercept;
            _history.Add(intercept);
            if (_history.Count > 32)
                _history.RemoveAt(0);

            LastEvent = string.IsNullOrWhiteSpace(intercept.FactionId)
                ? $"Dead air at {CurrentFrequency:0.00} MHz."
                : $"Intercepted {intercept.Callsign} at {CurrentFrequency:0.00} MHz.";

            // Audio: radio static on every tune
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayRadioStatic();

            // Audio: voice-over only for new (non-duplicate) broadcasts with a mapped clip
            string? voiceOverClip = ResolveVoiceOver(intercept);
            string broadcastKey = MakeBroadcastKey(intercept);
            if (voiceOverClip != null && _playedBroadcastKeys.Add(broadcastKey))
            {
                AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayVoiceOver(voiceOverClip);
            }

            BroadcastIntercepted?.Invoke(intercept, voiceOverClip);
            RaiseStateChanged();
            return LastEvent;
        }

        public string BroadcastBeacon(string customMessage = "Holdfast shelter holding. Awaiting courier contact.")
        {
            var beacon = new RadioIntercept(
                "faction_holdfast",
                "HOLDFAST BASE",
                CurrentFrequency,
                RadioEventKind.ParleyResolution,
                customMessage,
                5,
                Day);
            _history.Add(beacon);
            if (_history.Count > 32)
                _history.RemoveAt(0);

            // The broadcast is the latest intercept: mirror Listen() so the
            // receiver's LastIntercept reflects the beacon just sent. This keeps
            // the shelter-operations gate green and makes the UI's "latest
            // intercept" readout show the outgoing beacon, not stale dead air.
            LastIntercept = beacon;

            LastEvent = $"Emergency beacon broadcast on {CurrentFrequency:0.00} MHz.";
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayVoiceOver("vo_kind_parley");
            BroadcastIntercepted?.Invoke(beacon, "vo_kind_parley");
            RaiseStateChanged();
            return LastEvent;
        }

        public void TuneDelta(float deltaMhz)
        {
            float target = (float)Math.Round(Math.Clamp(CurrentFrequency + deltaMhz, 88.0f, 150.0f), 2);
            Listen(target);
        }

        /// <summary>
        /// Record a direction-finding observation for the currently intercepted signal.
        /// </summary>
        public bool RecordBearingObservation(float bearingDegrees)
        {
            string sigId = LastIntercept.HasValue && !string.IsNullOrEmpty(LastIntercept.Value.FactionId)
                ? LastIntercept.Value.FactionId
                : $"freq_{CurrentFrequency:000.0}";

            var obs = new RadioObservation
            {
                signalId = sigId,
                stationId = "station_holdfast",
                day = Day,
                hour = 12f,
                bearingDegrees = (float)Math.Round(Math.Clamp(bearingDegrees, 0f, 359f), 1),
                errorDegrees = 1.5f,
                signalStrength = (LastIntercept.HasValue ? LastIntercept.Value.SignalStrength : 2) / 5.0f,
                noiseLevel = 0.05f,
                frequencyMhz = CurrentFrequency,
                weatherCondition = "Clear",
                operatorSkill = 0.9f
            };

            bool ok = Triangulation.RecordObservation(obs);
            if (ok)
            {
                LastEvent = $"Recorded DF bearing {obs.bearingDegrees:000}° on {CurrentFrequency:0.00} MHz ({Triangulation.GetObservationCount(sigId)} obs).";
                RaiseStateChanged();
            }
            return ok;
        }

        /// <summary>
        /// Process collected observations to triangulate signal emitter and reveal wasteland coordinates.
        /// </summary>
        public TriangulationCandidate? TriangulateCurrentSignal()
        {
            string sigId = LastIntercept.HasValue && !string.IsNullOrEmpty(LastIntercept.Value.FactionId)
                ? LastIntercept.Value.FactionId
                : $"freq_{CurrentFrequency:000.0}";

            var candidate = Triangulation.Triangulate(sigId, Rng);
            if (candidate != null)
            {
                bool discovered = Triangulation.IsLocationDiscovered(candidate.locationId);
                LastEvent = discovered
                    ? $"Triangulation confirmed: {candidate.displayName} at ({candidate.estimatedX:F1}, {candidate.estimatedY:F1}) [Conf: {candidate.confidence:P0}]."
                    : $"Triangulation progress on {sigId}: {candidate.confidence:P0} confidence ({Triangulation.GetObservationCount(sigId)} obs).";
                RaiseStateChanged();
            }
            else
            {
                LastEvent = $"Insufficient bearing observations to triangulate {sigId} (need ≥2).";
                RaiseStateChanged();
            }
            return candidate;
        }

        /// <summary>
        /// Player-visible warlord radio warning (doctrine shift / hostile action).
        /// Adds a RaidWarning intercept under the canonical warlords_sector_4
        /// identity so the radio history carries the consequence. Presentation
        /// only — the warlord AI emits the intent through Core events; this is
        /// the thin adapter that surfaces it.
        /// </summary>
        public string InterceptWarlordWarning(string message, int day)
        {
            if (string.IsNullOrWhiteSpace(message)) return "";
            var warning = new RadioIntercept(
                "warlords_sector_4",
                "TOLL HOUSE RELAY",
                94.2f,
                RadioEventKind.RaidWarning,
                message,
                3,
                day > 0 ? day : Day);
            _history.Add(warning);
            if (_history.Count > 32)
                _history.RemoveAt(0);
            LastEvent = "Warlord radio warning intercepted on 94.2 MHz.";
            BroadcastIntercepted?.Invoke(warning, null);
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Cultural broadcast bridge: rare vinyl → shortwave. Deterministic, host-wired.
        /// VinylMoraleSystem fires OnCulturalBroadcast for rare records; Main wires it here.
        /// Power load (150W) is checked by the host before calling — this method only records the signal.
        /// </summary>
        public string RecordCulturalBroadcast(string recordId, string genre, string displayName, int day, float signalStrength)
        {
            if (string.IsNullOrWhiteSpace(recordId)) return string.Empty;
            string msg = $"Cultural broadcast: '{displayName}' ({genre}) — pre-war vinyl on shortwave. Wanderers may hear this.";
            var broadcast = new RadioIntercept(
                "faction_holdfast",
                "HOLDFAST CULTURAL RELAY",
                98.6f,
                RadioEventKind.CulturalBroadcast,
                msg,
                Math.Clamp((int)(signalStrength * 9f), 1, 9),
                day > 0 ? day : Day);
            _history.Add(broadcast);
            if (_history.Count > 32)
                _history.RemoveAt(0);
            LastEvent = $"Cultural broadcast on 98.6 MHz: {recordId} ({genre})";
            BroadcastIntercepted?.Invoke(broadcast, null);
            RaiseStateChanged();
            return LastEvent;
        }

        /// <summary>
        /// Resolve a voice-over clip name for this intercept.
        /// Returns null if no clip is mapped for this faction/event combination.
        /// </summary>
        private static string? ResolveVoiceOver(RadioIntercept intercept)
        {
            if (string.IsNullOrWhiteSpace(intercept.FactionId))
                return null;

            // Try faction-specific mapping first, then generic event kind
            foreach (var kvp in s_voiceOverMap)
            {
                if (intercept.Message != null &&
                    intercept.Message.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }
            return null;
        }

        /// <summary>
        /// Stable dedup key: day + frequency + message hash.
        /// Prevents replay on UI refresh/reopen while allowing same-frequency different-day broadcasts.
        /// Uses the deterministic Core StableHash — string.GetHashCode() is randomized
        /// per process and would make the key unstable across runs.
        /// </summary>
        private static string MakeBroadcastKey(RadioIntercept intercept)
        {
            return $"{intercept.Day}:{intercept.FrequencyMhz:F2}:{StableHash.Of(intercept.Message)}";
        }

        /// <summary>True when this broadcast's dedup key was already played (voice-over already fired).</summary>
        public bool HasPlayed(RadioIntercept intercept)
        {
            return _playedBroadcastKeys.Contains(MakeBroadcastKey(intercept));
        }

        // ── Persistence ─────────────────────────────────────────────────────

        /// <summary>
        /// Snapshot every authoritative mutable value: ordered intercept history,
        /// played-broadcast dedup keys (ordinal-sorted for a stable checksum), the
        /// tuned frequency, and the sim day. The FactionRadioEngine corpus itself
        /// is static data — never serialized.
        /// </summary>
        public RadioSaveState CaptureSave()
        {
            var state = new RadioSaveState
            {
                day = Day,
                currentFrequency = CurrentFrequency,
                history = new List<RadioInterceptEntry>(_history.Count)
            };
            for (int i = 0; i < _history.Count; i++)
            {
                var h = _history[i];
                state.history.Add(new RadioInterceptEntry
                {
                    factionId = h.FactionId,
                    callsign = h.Callsign,
                    frequencyMhz = h.FrequencyMhz,
                    kind = (int)h.Kind,
                    message = h.Message,
                    signalStrength = h.SignalStrength,
                    day = h.Day
                });
            }
            state.playedBroadcastKeys = new List<string>(_playedBroadcastKeys);
            state.playedBroadcastKeys.Sort(StringComparer.Ordinal);
            return state;
        }

        /// <summary>
        /// Rebuild receiver state from a snapshot. Overwrites history, dedup keys,
        /// frequency, and day; the engine corpus is unchanged.
        /// </summary>
        public void RestoreSave(RadioSaveState state)
        {
            _history.Clear();
            _playedBroadcastKeys.Clear();
            if (state == null) return;

            Day = Math.Max(1, state.day);
            CurrentFrequency = state.currentFrequency > 0f ? state.currentFrequency : FirstFrequency();

            if (state.history != null)
                for (int i = 0; i < state.history.Count; i++)
                {
                    var e = state.history[i];
                    if (e == null) continue;
                    _history.Add(new RadioIntercept(
                        e.factionId, e.callsign, e.frequencyMhz,
                        (RadioEventKind)e.kind, e.message, e.signalStrength, Math.Max(1, e.day)));
                }

            if (state.playedBroadcastKeys != null)
                for (int i = 0; i < state.playedBroadcastKeys.Count; i++)
                    if (!string.IsNullOrEmpty(state.playedBroadcastKeys[i]))
                        _playedBroadcastKeys.Add(state.playedBroadcastKeys[i]);

            LastEvent = "Radio state restored.";
        }

        // ── Triangulation demo actions ────────────────────────────────

        /// <summary>Record a directional observation of a signal.</summary>
        public string RecordObservationDemo(string signalId, float bearing, float signalStrength = 0.7f, float noise = 0.2f)
        {
            var obs = new RadioObservation
            {
                signalId = signalId,
                stationId = "station_alpha",
                day = Day,
                hour = 12f,
                bearingDegrees = bearing,
                errorDegrees = 5f + noise * 10f,
                signalStrength = signalStrength,
                noiseLevel = noise,
                frequencyMhz = CurrentFrequency,
                weatherCondition = "Clear",
                operatorSkill = 0.6f
            };
            bool ok = Triangulation.RecordObservation(obs);
            return ok
                ? $"Observation recorded: {signalId} at bearing {bearing:F0}° (strength {signalStrength:F2}, noise {noise:F2})."
                : "Invalid observation.";
        }

        /// <summary>Attempt to triangulate a signal.</summary>
        public string TriangulateDemo(string signalId)
        {
            var candidate = Triangulation.Triangulate(signalId, Rng);
            if (candidate == null)
                return $"Not enough observations for {signalId}. Need at least {SignalTriangulationSystem.MinObservationsForHypothesis}.";
            bool discovered = Triangulation.IsLocationDiscovered(candidate.locationId);
            return discovered
                ? $"Triangulation complete! Location {candidate.locationId} discovered (confidence {candidate.confidence:F2}, uncertainty ±{candidate.uncertaintyRadiusKm:F0} km)."
                : $"Hypothesis: {candidate.locationId} (confidence {candidate.confidence:F2}, uncertainty ±{candidate.uncertaintyRadiusKm:F0} km, {candidate.observationCount} observations).";
        }

        /// <summary>Get triangulation status for a signal.</summary>
        public string TriangulationStatusLine(string signalId)
        {
            int obsCount = Triangulation.GetObservationCount(signalId);
            var candidate = Triangulation.GetCandidate(signalId);
            if (candidate == null)
                return $"Signal {signalId}: {obsCount} observation(s). No hypothesis yet.";
            bool discovered = Triangulation.IsLocationDiscovered(candidate.locationId);
            return $"Signal {signalId}: {obsCount} obs, confidence {candidate.confidence:F2}, " +
                   $"uncertainty ±{candidate.uncertaintyRadiusKm:F0} km" +
                   (discovered ? " [DISCOVERED]" : " [pending]");
        }

        public string StatusLine()
        {
            string carrier = LastIntercept.HasValue
                ? (string.IsNullOrWhiteSpace(LastIntercept.Value.FactionId)
                    ? "dead air"
                    : LastIntercept.Value.Callsign)
                : "no carrier sampled";
            return $"Radio: {Engine.FactionCount} channels · {CurrentFrequency:0.00} MHz · {carrier} · " +
                   $"{_history.Count} intercepts logged (day {Day}).";
        }

        private float FirstFrequency()
        {
            var factions = Engine.GetAllFactions();
            return factions.Count > 0 ? Engine.GetFactionFrequency(factions[0]) : 88.4f;
        }
    }
}
