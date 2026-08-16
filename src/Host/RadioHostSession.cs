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
    {
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
        public ISeededRng Rng { get; }
        public IReadOnlyList<RadioIntercept> History => _history;
        public int Day { get; private set; }
        public float CurrentFrequency { get; private set; }
        public RadioIntercept? LastIntercept { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public RadioHostSession(
            FactionRadioEngine engine,
            ISeededRng? rng = null,
            int day = 1)
        {
            Engine = engine ?? new FactionRadioEngine();
            Rng = rng ?? new SeededRng(DemoSeed);
            Day = Math.Max(1, day);
            CurrentFrequency = FirstFrequency();
        }

        public static RadioHostSession Create(string dataDir, int day = 1)
        {
            string path = Path.Combine(dataDir ?? string.Empty, CorpusFileName);
            if (!File.Exists(path))
                path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", CorpusFileName);

            string json = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
            var session = new RadioHostSession(FactionRadioEngine.LoadFromJson(json), new SeededRng(DemoSeed), day);
            session.Listen();
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
            StateChanged?.Invoke();
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

            LastEvent = $"Emergency beacon broadcast on {CurrentFrequency:0.00} MHz.";
            AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayVoiceOver("vo_kind_parley");
            BroadcastIntercepted?.Invoke(beacon, "vo_kind_parley");
            StateChanged?.Invoke();
            return LastEvent;
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
            StateChanged?.Invoke();
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
        /// </summary>
        private static string MakeBroadcastKey(RadioIntercept intercept)
        {
            return $"{intercept.Day}:{intercept.FrequencyMhz:F2}:{(intercept.Message?.GetHashCode() ?? 0)}";
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
