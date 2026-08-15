using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class GhostTransmissionEntry
    {
        public string transmission_id;
        public float frequency_khz;
        public string broadcast_format;
        public string estimated_origin;
        public string signal_anomaly;
        public string transcript;
        public string cipher_clue;
        public string mystery_thread;
        public string[] tags;
    }

    [Serializable]
    public sealed class GhostTransmissionFile
    {
        public int schema_version;
        public string collection_id;
        public List<GhostTransmissionEntry> transmissions = new List<GhostTransmissionEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and frequency tuner for The Ghost Transmission Logs (12 mysterious anomalies).
    /// </summary>
    public sealed class GhostTransmissionCatalog
    {
        private readonly Dictionary<string, GhostTransmissionEntry> _byId =
            new Dictionary<string, GhostTransmissionEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<GhostTransmissionEntry> _all = new List<GhostTransmissionEntry>();

        public IReadOnlyList<GhostTransmissionEntry> AllTransmissions => _all;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<GhostTransmissionFile>(json);
            if (file?.transmissions == null) return;

            foreach (var t in file.transmissions)
            {
                if (t == null || string.IsNullOrEmpty(t.transmission_id)) continue;
                _byId[t.transmission_id] = t;
                _all.Add(t);
            }
        }

        public GhostTransmissionEntry GetById(string transmissionId)
        {
            if (string.IsNullOrEmpty(transmissionId)) return null;
            _byId.TryGetValue(transmissionId, out var t);
            return t;
        }

        public GhostTransmissionEntry FindNearestFrequency(float freqKhz, float maxDeltaKhz = 25.0f)
        {
            GhostTransmissionEntry best = null;
            float bestDelta = float.MaxValue;

            for (int i = 0; i < _all.Count; i++)
            {
                var entry = _all[i];
                float delta = Math.Abs(entry.frequency_khz - freqKhz);
                if (delta <= maxDeltaKhz && delta < bestDelta)
                {
                    bestDelta = delta;
                    best = entry;
                }
            }
            return best;
        }

        public List<GhostTransmissionEntry> GetByMysteryThread(string mysteryThread)
        {
            var results = new List<GhostTransmissionEntry>();
            if (string.IsNullOrEmpty(mysteryThread)) return results;

            for (int i = 0; i < _all.Count; i++)
            {
                var entry = _all[i];
                if (string.Equals(entry.mystery_thread, mysteryThread, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(entry);
                }
            }
            return results;
        }
    }
}
