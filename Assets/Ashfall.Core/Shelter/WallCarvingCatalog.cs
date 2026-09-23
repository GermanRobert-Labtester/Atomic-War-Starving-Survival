// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    public sealed class WallCarvingBand
    {
        [JsonPropertyName("morale_band")]
        public string MoraleBand { get; set; } = string.Empty;

        [JsonPropertyName("morale_min")]
        public int MoraleMin { get; set; }

        [JsonPropertyName("morale_max")]
        public int MoraleMax { get; set; }

        [JsonPropertyName("templates")]
        public List<string> Templates { get; set; } = new List<string>();

        [JsonPropertyName("carving_chance")]
        public float CarvingChance { get; set; }
    }

    public sealed class WallCarvingCatalog
    {
        private sealed class CatalogRoot
        {
            [JsonPropertyName("schema_version")]
            public int SchemaVersion { get; set; }

            [JsonPropertyName("items")]
            public List<WallCarvingBand> Items { get; set; } = new List<WallCarvingBand>();
        }

        private readonly List<WallCarvingBand> _bands = new List<WallCarvingBand>();

        public IReadOnlyList<WallCarvingBand> Bands => _bands;
        public int TotalTemplateCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _bands.Count; i++)
                    count += _bands[i].Templates.Count;
                return count;
            }
        }

        public WallCarvingCatalog() { }

        public WallCarvingCatalog(IEnumerable<WallCarvingBand> bands)
        {
            if (bands != null)
                _bands.AddRange(bands);
        }

        public static WallCarvingCatalog FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new WallCarvingCatalog();

            var root = JsonSerializer.Deserialize<CatalogRoot>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new WallCarvingCatalog(root?.Items ?? (IEnumerable<WallCarvingBand>)Array.Empty<WallCarvingBand>());
        }

        public static WallCarvingCatalog LoadFromDirectory(string dataDirectory)
        {
            var filePath = Path.Combine(dataDirectory, "wall_carving_templates.json");
            if (!File.Exists(filePath))
                return new WallCarvingCatalog();

            var json = File.ReadAllText(filePath);
            return FromJson(json);
        }

        public WallCarvingBand? GetBandForMorale(float morale)
        {
            int rounded = (int)Math.Round(Math.Clamp(morale, 0f, 100f));
            for (int i = 0; i < _bands.Count; i++)
            {
                var b = _bands[i];
                if (rounded >= b.MoraleMin && rounded <= b.MoraleMax)
                    return b;
            }
            return _bands.Count > 0 ? _bands[0] : null;
        }

        public string GetRandomTemplate(float morale, Func<int, int> rngNext)
        {
            var band = GetBandForMorale(morale);
            if (band == null || band.Templates.Count == 0)
                return string.Empty;

            int idx = rngNext != null ? rngNext(band.Templates.Count) : 0;
            if (idx < 0 || idx >= band.Templates.Count) idx = 0;
            return band.Templates[idx];
        }
    }
}
