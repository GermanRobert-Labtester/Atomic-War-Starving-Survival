using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class RadioBroadcastEntry
    {
        public string broadcast_id;
        public float frequency_mhz;
        public string station_name;
        public string voice_profile;
        public int day_trigger;
        public string title;
        public string transcript;
        public string[] tags;
    }

    [Serializable]
    public sealed class RadioScriptbookFile
    {
        public int schema_version;
        public string collection_id;
        public List<RadioBroadcastEntry> broadcasts = new List<RadioBroadcastEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and frequency tuner interface for the Multi-Frequency Radio Scriptbook.
    /// </summary>
    public sealed class RadioScriptbookCatalog
    {
        private readonly Dictionary<string, RadioBroadcastEntry> _byBroadcastId =
            new Dictionary<string, RadioBroadcastEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<RadioBroadcastEntry> _allBroadcasts = new List<RadioBroadcastEntry>();

        public IReadOnlyList<RadioBroadcastEntry> AllBroadcasts => _allBroadcasts;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<RadioScriptbookFile>(json);
            if (file?.broadcasts == null) return;

            foreach (var b in file.broadcasts)
            {
                if (b == null || string.IsNullOrEmpty(b.broadcast_id)) continue;
                _byBroadcastId[b.broadcast_id] = b;
                _allBroadcasts.Add(b);
            }
        }

        public RadioBroadcastEntry GetById(string broadcastId)
        {
            if (string.IsNullOrEmpty(broadcastId)) return null;
            _byBroadcastId.TryGetValue(broadcastId, out var b);
            return b;
        }

        public List<RadioBroadcastEntry> GetByFrequency(float freqMhz, float tolerance = 0.15f)
        {
            var results = new List<RadioBroadcastEntry>();
            for (int i = 0; i < _allBroadcasts.Count; i++)
            {
                var entry = _allBroadcasts[i];
                if (Math.Abs(entry.frequency_mhz - freqMhz) <= tolerance)
                {
                    results.Add(entry);
                }
            }
            return results;
        }

        public RadioBroadcastEntry GetActiveBroadcast(float freqMhz, int currentDay, float tolerance = 0.15f)
        {
            var candidates = GetByFrequency(freqMhz, tolerance);
            RadioBroadcastEntry best = null;
            for (int i = 0; i < candidates.Count; i++)
            {
                var c = candidates[i];
                if (c.day_trigger <= currentDay)
                {
                    if (best == null || c.day_trigger > best.day_trigger)
                    {
                        best = c;
                    }
                }
            }
            return best;
        }
    }
}
