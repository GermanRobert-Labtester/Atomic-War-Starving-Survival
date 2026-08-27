using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Core simulation engine for the Ashfall Faction Radio &amp; Intercept system.
    /// Pure C#, deterministic selection via ISeededRng, zero engine dependencies.
    /// </summary>
    public sealed class FactionRadioEngine : IFactionRadioProvider
    {
        private readonly Dictionary<string, FactionRadioChannel> _channels = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _silenceEvents = new();
        private readonly List<string> _factionOrder = new();

        public int FactionCount => _channels.Count;
        public int SilenceEventCount => _silenceEvents.Count;

        public void RegisterChannel(FactionRadioChannel channel)
        {
            if (channel == null || string.IsNullOrEmpty(channel.FactionId)) return;
            string normId = channel.FactionId.ToLowerInvariant();
            if (!_channels.ContainsKey(normId))
            {
                _factionOrder.Add(normId);
            }
            _channels[normId] = channel;
        }

        public void AddSilenceEvent(string silenceText)
        {
            if (!string.IsNullOrWhiteSpace(silenceText))
            {
                _silenceEvents.Add(silenceText.Trim());
            }
        }

        public IReadOnlyList<string> GetAllFactions() => _factionOrder;

        public float GetFactionFrequency(string factionId)
        {
            return _channels.TryGetValue(factionId, out var ch) ? ch.FrequencyMhz : 100.0f;
        }

        public string GetFactionCallsign(string factionId)
        {
            return _channels.TryGetValue(factionId, out var ch) ? ch.Callsign : "UNKNOWN TRANSMITTER";
        }

        public string? TryFindFactionAtFrequency(float frequencyMhz, float toleranceMhz = 1.5f)
        {
            string bestFaction = null!;
            float minDiff = float.MaxValue;

            foreach (var (fId, ch) in _channels)
            {
                float diff = Math.Abs(ch.FrequencyMhz - frequencyMhz);
                if (diff <= toleranceMhz && diff < minDiff)
                {
                    minDiff = diff;
                    bestFaction = fId;
                }
            }

            return bestFaction;
        }

        public RadioIntercept GetBroadcastAtFrequency(float frequencyMhz, int day, ISeededRng rng)
        {
            var matchedFaction = TryFindFactionAtFrequency(frequencyMhz, 1.5f);
            if (string.IsNullOrEmpty(matchedFaction) || !_channels.TryGetValue(matchedFaction, out var channel))
            {
                // Return Silence / Dead air
                string silenceMsg = _silenceEvents.Count > 0
                    ? _silenceEvents[PickIndex(_silenceEvents.Count, day, frequencyMhz, rng)]
                    : "STATIC... [ No carrier detected on frequency. ] ...STATIC";

                return new RadioIntercept(
                    factionId: string.Empty,
                    callsign: "UNATTENDED RELAY / DEAD AIR",
                    frequencyMhz: frequencyMhz,
                    kind: RadioEventKind.Silence,
                    message: silenceMsg,
                    signalStrength: 1,
                    day: day);
            }

            // Calculate Signal Strength based on tuning precision
            float offset = Math.Abs(channel.FrequencyMhz - frequencyMhz);
            int signalStrength = 9;
            if (offset > 1.0f) signalStrength = 2;
            else if (offset > 0.5f) signalStrength = 4;
            else if (offset > 0.2f) signalStrength = 7;

            // Pick a message from intercept chatter
            var pool = channel.InterceptChatter;
            string msg = (pool != null && pool.Count > 0)
                ? pool[PickIndex(pool.Count, day, channel.FrequencyMhz, rng)]
                : "STATIC... [ Carrier active. Voice modulation degraded. ]";

            return new RadioIntercept(
                factionId: channel.FactionId,
                callsign: channel.Callsign,
                frequencyMhz: frequencyMhz,
                kind: RadioEventKind.InterceptChatter,
                message: msg,
                signalStrength: signalStrength,
                day: day);
        }

        public RadioIntercept GetFactionEvent(string factionId, RadioEventKind kind, int day, ISeededRng rng)
        {
            if (!_channels.TryGetValue(factionId, out var channel))
            {
                return GetBroadcastAtFrequency(100.0f, day, rng);
            }

            List<string> pool;
            switch (kind)
            {
                case RadioEventKind.ParleyResolution: pool = channel.ParleyResolutions; break;
                case RadioEventKind.RaidWarning: pool = channel.RaidWarnings; break;
                case RadioEventKind.TradeReaction: pool = channel.TradeReactions; break;
                case RadioEventKind.InterceptChatter:
                default: pool = channel.InterceptChatter; break;
            }

            string msg = (pool != null && pool.Count > 0)
                ? pool[PickIndex(pool.Count, day, (int)kind, rng)]
                : $"RADIO: [{channel.Callsign}] Transmission logged.";

            return new RadioIntercept(
                factionId: channel.FactionId,
                callsign: channel.Callsign,
                frequencyMhz: channel.FrequencyMhz,
                kind: kind,
                message: msg,
                signalStrength: 8,
                day: day);
        }

        private static int PickIndex(int count, int day, float seedModifier, ISeededRng rng)
        {
            if (count <= 1) return 0;
            if (rng != null)
            {
                return rng.Next(0, count);
            }
            // Deterministic hash fallback using StableHash (djb2/x33).
            // HashCode.Combine is runtime-randomized in modern .NET and would
            // break cross-host determinism. StableHash.Of is deterministic.
            int hash = StableHash.Of(day.ToString() + ":" + ((int)(seedModifier * 100)).ToString());
            return Math.Abs(hash) % count;
        }

        /// <summary>
        /// Loads corpus from raw JSON text.
        /// </summary>
        public static FactionRadioEngine LoadFromJson(string json)
        {
            var engine = new FactionRadioEngine();
            if (string.IsNullOrWhiteSpace(json)) return engine;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("silence_events", out var silenceProp) && silenceProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in silenceProp.EnumerateArray())
                {
                    engine.AddSilenceEvent(item.GetString()!);
                }
            }

            if (root.TryGetProperty("factions", out var factionsProp) && factionsProp.ValueKind == JsonValueKind.Object)
            {
                foreach (var facProp in factionsProp.EnumerateObject())
                {
                    string fId = facProp.Name;
                    var facObj = facProp.Value;

                    var channel = new FactionRadioChannel
                    {
                        FactionId = fId,
                        FrequencyMhz = facObj.TryGetProperty("frequency_mhz", out var fq) ? (float)fq.GetDouble() : 100.0f,
                        Callsign = facObj.TryGetProperty("callsign", out var cs) ? cs.GetString() ?? fId : fId
                    };

                    if (facObj.TryGetProperty("intercept_chatter", out var ic) && ic.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var line in ic.EnumerateArray())
                            channel.InterceptChatter.Add(line.GetString() ?? string.Empty);
                    }

                    if (facObj.TryGetProperty("parley_resolution", out var pr) && pr.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var line in pr.EnumerateArray())
                            channel.ParleyResolutions.Add(line.GetString() ?? string.Empty);
                    }

                    if (facObj.TryGetProperty("raid_warning", out var rw) && rw.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var line in rw.EnumerateArray())
                            channel.RaidWarnings.Add(line.GetString() ?? string.Empty);
                    }

                    if (facObj.TryGetProperty("trade_reaction", out var tr) && tr.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var line in tr.EnumerateArray())
                            channel.TradeReactions.Add(line.GetString() ?? string.Empty);
                    }

                    engine.RegisterChannel(channel);
                }
            }

            return engine;
        }
    }
}
