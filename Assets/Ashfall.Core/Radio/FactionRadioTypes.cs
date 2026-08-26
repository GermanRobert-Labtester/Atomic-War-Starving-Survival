using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Event category for faction radio transmissions.
    /// </summary>
    public enum RadioEventKind
    {
        InterceptChatter = 0,
        ParleyResolution = 1,
        RaidWarning = 2,
        TradeReaction = 3,
        Silence = 4,
        CulturalBroadcast = 5
    }

    /// <summary>
    /// Immutable record representing a single radio transmission / intercept.
    /// </summary>
    public readonly struct RadioIntercept
    {
        public string FactionId { get; }
        public string Callsign { get; }
        public float FrequencyMhz { get; }
        public RadioEventKind Kind { get; }
        public string Message { get; }
        public int SignalStrength { get; } // 1..9 (S-units)
        public int Day { get; }

        public RadioIntercept(
            string factionId,
            string callsign,
            float frequencyMhz,
            RadioEventKind kind,
            string message,
            int signalStrength,
            int day)
        {
            FactionId = factionId ?? string.Empty;
            Callsign = callsign ?? string.Empty;
            FrequencyMhz = frequencyMhz;
            Kind = kind;
            Message = message ?? string.Empty;
            SignalStrength = Math.Clamp(signalStrength, 1, 9);
            Day = Math.Max(1, day);
        }
    }

    /// <summary>
    /// Faction radio channel configuration.
    /// </summary>
    public sealed class FactionRadioChannel
    {
        public string FactionId { get; set; } = string.Empty;
        public string Callsign { get; set; } = string.Empty;
        public float FrequencyMhz { get; set; } = 100.0f;
        public List<string> InterceptChatter { get; set; } = new();
        public List<string> ParleyResolutions { get; set; } = new();
        public List<string> RaidWarnings { get; set; } = new();
        public List<string> TradeReactions { get; set; } = new();
    }

    /// <summary>
    /// Engine-agnostic interface for listening to and polling faction radio chatter.
    /// </summary>
    public interface IFactionRadioProvider
    {
        RadioIntercept GetBroadcastAtFrequency(float frequencyMhz, int day, ISeededRng rng);
        RadioIntercept GetFactionEvent(string factionId, RadioEventKind kind, int day, ISeededRng rng);
        string? TryFindFactionAtFrequency(float frequencyMhz, float toleranceMhz = 1.5f);
        float GetFactionFrequency(string factionId);
        string GetFactionCallsign(string factionId);
        IReadOnlyList<string> GetAllFactions();
    }
}
