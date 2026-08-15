namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Trust band the trader falls into at the table. Bands are data-defined
    /// (trade_tell_lines.json); these are the canonical ids.
    /// </summary>
    public static class TradeTrustBands
    {
        public const string Hostile = "hostile";
        public const string Wary = "wary";
        public const string Neutral = "neutral";
        public const string Warm = "warm";
    }

    /// <summary>One selected tell: the trader's readable posture at the table.</summary>
    public sealed class TradeTell
    {
        public string Id { get; }
        public TradeStance Stance { get; }
        public string Band { get; }
        public string Line { get; }

        public TradeTell(string id, TradeStance stance, string band, string line)
        {
            Id = id ?? string.Empty;
            Stance = stance;
            Band = band ?? string.Empty;
            Line = line ?? string.Empty;
        }
    }

    /// <summary>
    /// Tell selection surface: stance × trust band → tell id + line.
    /// Data-defined catalog, deterministic rotation via ISeededRng.
    /// </summary>
    public interface ITradeTellProvider
    {
        bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell);
        string BandForTrust(float trust);
    }

    /// <summary>
    /// The tell-line corpus engine. Same pattern as FactionRadioEngine: JSON
    /// in StreamingAssets is the authority, selection is seed-deterministic,
    /// and the engine is pure C# with zero host references.
    /// </summary>
    public sealed class TradeTellEngine : ITradeTellProvider
    {
        private readonly List<(string Id, float MinInclusive, float MaxInclusive)> _bands = new();
        private readonly Dictionary<string, List<string>> _pools = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, TradeStance> _stances = new(StringComparer.OrdinalIgnoreCase);

        public int BandCount => _bands.Count;
        public int PoolCount => _pools.Count;
        public int LineCount { get; private set; }

        public IReadOnlyList<string> Bands
        {
            get
            {
                var ids = new List<string>();
                foreach (var band in _bands) ids.Add(band.Id);
                return ids;
            }
        }

        public void RegisterBand(string id, float minInclusive, float maxInclusive)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            _bands.Add((id.Trim(), minInclusive, maxInclusive));
        }

        public void RegisterTellPool(TradeStance stance, string bandId, IEnumerable<string> lines)
        {
            if (string.IsNullOrWhiteSpace(bandId) || lines == null) return;
            string key = PoolKey(stance, bandId.Trim());
            var pool = new List<string>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    pool.Add(line.Trim());
                }
            }
            _pools[key] = pool;
            _stances[key] = stance;
            LineCount += pool.Count;
        }

        /// <summary>Ordered scan: first band whose [min, max] range contains the trust value.</summary>
        public string BandForTrust(float trust)
        {
            foreach (var band in _bands)
            {
                if (trust >= band.MinInclusive && trust <= band.MaxInclusive) return band.Id;
            }
            return _bands.Count > 0 ? _bands[_bands.Count - 1].Id : TradeTrustBands.Neutral;
        }

        public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell)
        {
            string band = BandForTrust(trust);
            string key = PoolKey(stance, band);
            if (!_pools.TryGetValue(key, out var pool) || pool.Count == 0)
            {
                tell = null;
                return false;
            }

            int index = pool.Count <= 1 ? 0 : (rng != null ? rng.Next(0, pool.Count) : 0);
            tell = new TradeTell(
                id: $"{StanceKey(stance)}_{band}_{index}",
                stance: stance,
                band: band,
                line: pool[index]);
            return true;
        }

        /// <summary>Raw pool access for corpus lint/tests. Pool is keyed by stance + band id.</summary>
        public bool TryGetPoolLines(TradeStance stance, string bandId, out IReadOnlyList<string> lines)
        {
            if (_pools.TryGetValue(PoolKey(stance, bandId ?? string.Empty), out var pool))
            {
                lines = pool;
                return true;
            }
            lines = Array.Empty<string>();
            return false;
        }

        private static string PoolKey(TradeStance stance, string bandId)
        {
            return StanceKey(stance) + "/" + bandId;
        }

        private static string StanceKey(TradeStance stance)
        {
            switch (stance)
            {
                case TradeStance.HostileRaid: return "hostile_raid";
                case TradeStance.Rob: return "rob";
                case TradeStance.Refuse: return "refuse";
                case TradeStance.ShareIntel: return "share_intel";
                case TradeStance.Trade:
                default: return "trade";
            }
        }

        /// <summary>
        /// Loads the tell corpus from raw JSON text:
        /// { "trust_bands": [{id,min,max}...], "tells": { "trade": { "warm": [lines...] } } }
        /// </summary>
        public static TradeTellEngine LoadFromJson(string json)
        {
            var engine = new TradeTellEngine();
            if (string.IsNullOrWhiteSpace(json)) return engine;

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("trust_bands", out var bandsProp) && bandsProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var b in bandsProp.EnumerateArray())
                {
                    string id = b.TryGetProperty("id", out var idEl) ? idEl.GetString() : null;
                    float min = b.TryGetProperty("min", out var minEl) ? (float)minEl.GetDouble() : -100f;
                    float max = b.TryGetProperty("max", out var maxEl) ? (float)maxEl.GetDouble() : 100f;
                    engine.RegisterBand(id, min, max);
                }
            }

            if (root.TryGetProperty("tells", out var tellsProp) && tellsProp.ValueKind == JsonValueKind.Object)
            {
                foreach (var stanceProp in tellsProp.EnumerateObject())
                {
                    if (!TryParseStance(stanceProp.Name, out var stance)) continue;
                    if (stanceProp.Value.ValueKind != JsonValueKind.Object) continue;

                    foreach (var bandProp in stanceProp.Value.EnumerateObject())
                    {
                        if (bandProp.Value.ValueKind != JsonValueKind.Array) continue;
                        var lines = new List<string>();
                        foreach (var line in bandProp.Value.EnumerateArray())
                        {
                            lines.Add(line.GetString() ?? string.Empty);
                        }
                        engine.RegisterTellPool(stance, bandProp.Name, lines);
                    }
                }
            }

            return engine;
        }

        private static bool TryParseStance(string key, out TradeStance stance)
        {
            switch ((key ?? string.Empty).Trim())
            {
                case "hostile_raid": stance = TradeStance.HostileRaid; return true;
                case "rob": stance = TradeStance.Rob; return true;
                case "refuse": stance = TradeStance.Refuse; return true;
                case "trade": stance = TradeStance.Trade; return true;
                case "share_intel": stance = TradeStance.ShareIntel; return true;
                default: stance = TradeStance.Trade; return false;
            }
        }
    }
}
