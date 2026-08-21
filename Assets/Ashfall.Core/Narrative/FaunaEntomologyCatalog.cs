using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class ArmoredRoachHiveEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("nest_location")]
        public string NestLocation { get; set; } = string.Empty;

        [JsonPropertyName("specimen_morph")]
        public string SpecimenMorph { get; set; } = string.Empty;

        [JsonPropertyName("average_length_cm")]
        public float AverageLengthCm { get; set; }

        [JsonPropertyName("threat_rating")]
        public string ThreatRating { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BlindMoleratStudyEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("colony_id")]
        public string ColonyId { get; set; } = string.Empty;

        [JsonPropertyName("caste_classification")]
        public string CasteClassification { get; set; } = string.Empty;

        [JsonPropertyName("incisor_mohs_hardness")]
        public float IncisorMohsHardness { get; set; }

        [JsonPropertyName("burrowing_rate_cm_hr")]
        public float BurrowingRateCmHr { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CarrionVultureSightingEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("observation_post")]
        public string ObservationPost { get; set; } = string.Empty;

        [JsonPropertyName("avian_morphology")]
        public string AvianMorphology { get; set; } = string.Empty;

        [JsonPropertyName("estimated_wingspan_meters")]
        public float EstimatedWingspanMeters { get; set; }

        [JsonPropertyName("radiation_tracking_behavior")]
        public string RadiationTrackingBehavior { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SiloMosquitoVectorEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("silo_location_id")]
        public string SiloLocationId { get; set; } = string.Empty;

        [JsonPropertyName("vector_species")]
        public string VectorSpecies { get; set; } = string.Empty;

        [JsonPropertyName("larval_density_per_liter")]
        public int LarvalDensityPerLiter { get; set; }

        [JsonPropertyName("pathogen_transmitted")]
        public string PathogenTransmitted { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class FaunaEntomologyCatalog
    {
        private readonly List<ArmoredRoachHiveEntry> _roachEntries = new List<ArmoredRoachHiveEntry>();
        private readonly List<BlindMoleratStudyEntry> _moleratEntries = new List<BlindMoleratStudyEntry>();
        private readonly List<CarrionVultureSightingEntry> _vultureEntries = new List<CarrionVultureSightingEntry>();
        private readonly List<SiloMosquitoVectorEntry> _mosquitoEntries = new List<SiloMosquitoVectorEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<ArmoredRoachHiveEntry> RoachEntries => _roachEntries;
        public IReadOnlyList<BlindMoleratStudyEntry> MoleratEntries => _moleratEntries;
        public IReadOnlyList<CarrionVultureSightingEntry> VultureEntries => _vultureEntries;
        public IReadOnlyList<SiloMosquitoVectorEntry> MosquitoEntries => _mosquitoEntries;

        public int TotalCount => _roachEntries.Count + _moleratEntries.Count + _vultureEntries.Count + _mosquitoEntries.Count;

        public static FaunaEntomologyCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new FaunaEntomologyCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Armored Cockroach Hive Logs
            string roachPath = Path.Combine(directoryPath, "armored_cockroach_hive_logs.json");
            if (File.Exists(roachPath))
            {
                var list = JsonSerializer.Deserialize<List<ArmoredRoachHiveEntry>>(File.ReadAllText(roachPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._roachEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Blind Cave Mole-Rat Studies
            string moleratPath = Path.Combine(directoryPath, "blind_cave_molerat_studies.json");
            if (File.Exists(moleratPath))
            {
                var list = JsonSerializer.Deserialize<List<BlindMoleratStudyEntry>>(File.ReadAllText(moleratPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._moleratEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Surface Carrion Vulture Sightings
            string vulturePath = Path.Combine(directoryPath, "carrion_vulture_sighting_logs.json");
            if (File.Exists(vulturePath))
            {
                var list = JsonSerializer.Deserialize<List<CarrionVultureSightingEntry>>(File.ReadAllText(vulturePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._vultureEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Silo Mosquito Vector Records
            string mosquitoPath = Path.Combine(directoryPath, "silo_mosquito_vector_records.json");
            if (File.Exists(mosquitoPath))
            {
                var list = JsonSerializer.Deserialize<List<SiloMosquitoVectorEntry>>(File.ReadAllText(mosquitoPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mosquitoEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public ArmoredRoachHiveEntry? GetRoach(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ArmoredRoachHiveEntry e ? e : null;
        }

        public BlindMoleratStudyEntry? GetMolerat(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BlindMoleratStudyEntry e ? e : null;
        }

        public CarrionVultureSightingEntry? GetVulture(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CarrionVultureSightingEntry e ? e : null;
        }

        public SiloMosquitoVectorEntry? GetMosquito(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SiloMosquitoVectorEntry e ? e : null;
        }
    }
}
