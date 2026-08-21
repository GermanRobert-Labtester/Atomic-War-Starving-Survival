using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class CobaltLiturgyEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("cult_faction")]
        public string CultFaction { get; set; } = string.Empty;

        [JsonPropertyName("liturgy_type")]
        public string LiturgyType { get; set; } = string.Empty;

        [JsonPropertyName("ritual_sacrament")]
        public string RitualSacrament { get; set; } = string.Empty;

        [JsonPropertyName("sacred_rad_threshold_cpm")]
        public int SacredRadThresholdCpm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class IronSynodCanonEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("synod_chapter")]
        public string SynodChapter { get; set; } = string.Empty;

        [JsonPropertyName("canon_number")]
        public string CanonNumber { get; set; } = string.Empty;

        [JsonPropertyName("metallurgical_rule")]
        public string MetallurgicalRule { get; set; } = string.Empty;

        [JsonPropertyName("sacred_temperature_celsius")]
        public int SacredTemperatureCelsius { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GeophoneHymnalEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("monastery_circle")]
        public string MonasteryCircle { get; set; } = string.Empty;

        [JsonPropertyName("hymn_number")]
        public string HymnNumber { get; set; } = string.Empty;

        [JsonPropertyName("resonant_frequency_hz")]
        public float ResonantFrequencyHz { get; set; }

        [JsonPropertyName("liturgical_acoustic_mode")]
        public string LiturgicalAcousticMode { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class WastelandEpitaphEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("grave_site")]
        public string GraveSite { get; set; } = string.Empty;

        [JsonPropertyName("marker_material")]
        public string MarkerMaterial { get; set; } = string.Empty;

        [JsonPropertyName("deceased_identity")]
        public string DeceasedIdentity { get; set; } = string.Empty;

        [JsonPropertyName("cause_of_death")]
        public string CauseOfDeath { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class FringeCultsCatalog
    {
        private readonly List<CobaltLiturgyEntry> _cobaltLiturgies = new List<CobaltLiturgyEntry>();
        private readonly List<IronSynodCanonEntry> _ironSynodCanons = new List<IronSynodCanonEntry>();
        private readonly List<GeophoneHymnalEntry> _geophoneHymnals = new List<GeophoneHymnalEntry>();
        private readonly List<WastelandEpitaphEntry> _wastelandEpitaphs = new List<WastelandEpitaphEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<CobaltLiturgyEntry> CobaltLiturgies => _cobaltLiturgies;
        public IReadOnlyList<IronSynodCanonEntry> IronSynodCanons => _ironSynodCanons;
        public IReadOnlyList<GeophoneHymnalEntry> GeophoneHymnals => _geophoneHymnals;
        public IReadOnlyList<WastelandEpitaphEntry> WastelandEpitaphs => _wastelandEpitaphs;

        public int TotalCount => _cobaltLiturgies.Count + _ironSynodCanons.Count + _geophoneHymnals.Count + _wastelandEpitaphs.Count;

        public static FringeCultsCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new FringeCultsCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Cobalt Liturgies
            string cobaltPath = Path.Combine(directoryPath, "cobalt_liturgies.json");
            if (File.Exists(cobaltPath))
            {
                var list = JsonSerializer.Deserialize<List<CobaltLiturgyEntry>>(File.ReadAllText(cobaltPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._cobaltLiturgies.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Iron Synod Canons
            string ironPath = Path.Combine(directoryPath, "iron_synod_canons.json");
            if (File.Exists(ironPath))
            {
                var list = JsonSerializer.Deserialize<List<IronSynodCanonEntry>>(File.ReadAllText(ironPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._ironSynodCanons.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Geophone Hymnals
            string geophonePath = Path.Combine(directoryPath, "geophone_hymnals.json");
            if (File.Exists(geophonePath))
            {
                var list = JsonSerializer.Deserialize<List<GeophoneHymnalEntry>>(File.ReadAllText(geophonePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._geophoneHymnals.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Wasteland Epitaphs
            string epitaphPath = Path.Combine(directoryPath, "wasteland_grave_epitaphs.json");
            if (File.Exists(epitaphPath))
            {
                var list = JsonSerializer.Deserialize<List<WastelandEpitaphEntry>>(File.ReadAllText(epitaphPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._wastelandEpitaphs.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public CobaltLiturgyEntry? GetCobaltLiturgy(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CobaltLiturgyEntry e ? e : null;
        }

        public IronSynodCanonEntry? GetIronSynodCanon(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is IronSynodCanonEntry e ? e : null;
        }

        public GeophoneHymnalEntry? GetGeophoneHymnal(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GeophoneHymnalEntry e ? e : null;
        }

        public WastelandEpitaphEntry? GetWastelandEpitaph(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is WastelandEpitaphEntry e ? e : null;
        }
    }
}
