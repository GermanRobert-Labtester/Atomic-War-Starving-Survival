using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class PotFurnaceGlassMeltEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("furnace_pot_identifier")]
        public string FurnacePotIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("batch_feedstock_formula")]
        public string BatchFeedstockFormula { get; set; } = string.Empty;

        [JsonPropertyName("melt_temperature_celsius")]
        public float MeltTemperatureCelsius { get; set; }

        [JsonPropertyName("optical_clarity_grade")]
        public string OpticalClarityGrade { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class LiebigCondenserFractureEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("distillation_rig_id")]
        public string DistillationRigId { get; set; } = string.Empty;

        [JsonPropertyName("glassware_component")]
        public string GlasswareComponent { get; set; } = string.Empty;

        [JsonPropertyName("vapor_temperature_celsius")]
        public float VaporTemperatureCelsius { get; set; }

        [JsonPropertyName("failure_phenomenon")]
        public string FailurePhenomenon { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GroundGlassJointGreaseEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("apparatus_station_id")]
        public string ApparatusStationId { get; set; } = string.Empty;

        [JsonPropertyName("lubricant_compound_used")]
        public string LubricantCompoundUsed { get; set; } = string.Empty;

        [JsonPropertyName("system_vacuum_torr")]
        public float SystemVacuumTorr { get; set; }

        [JsonPropertyName("failure_outcome")]
        public string FailureOutcome { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class AnnealingLehrBirefringenceEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("lehr_furnace_id")]
        public string LehrFurnaceId { get; set; } = string.Empty;

        [JsonPropertyName("annealed_glass_article")]
        public string AnnealedGlassArticle { get; set; } = string.Empty;

        [JsonPropertyName("soak_temperature_celsius")]
        public float SoakTemperatureCelsius { get; set; }

        [JsonPropertyName("residual_stress_optical_retardance_nm_cm")]
        public float ResidualStressOpticalRetardanceNmCm { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class GlassblowingDistillationCatalog
    {
        private readonly List<PotFurnaceGlassMeltEntry> _meltEntries = new List<PotFurnaceGlassMeltEntry>();
        private readonly List<LiebigCondenserFractureEntry> _condenserEntries = new List<LiebigCondenserFractureEntry>();
        private readonly List<GroundGlassJointGreaseEntry> _greaseEntries = new List<GroundGlassJointGreaseEntry>();
        private readonly List<AnnealingLehrBirefringenceEntry> _annealEntries = new List<AnnealingLehrBirefringenceEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<PotFurnaceGlassMeltEntry> MeltEntries => _meltEntries;
        public IReadOnlyList<LiebigCondenserFractureEntry> CondenserEntries => _condenserEntries;
        public IReadOnlyList<GroundGlassJointGreaseEntry> GreaseEntries => _greaseEntries;
        public IReadOnlyList<AnnealingLehrBirefringenceEntry> AnnealEntries => _annealEntries;

        public int TotalCount => _meltEntries.Count + _condenserEntries.Count + _greaseEntries.Count + _annealEntries.Count;

        public static GlassblowingDistillationCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new GlassblowingDistillationCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Pot-Furnace Soda-Lime Glass Melts
            string meltPath = Path.Combine(directoryPath, "pot_furnace_glass_melts.json");
            if (File.Exists(meltPath))
            {
                var list = JsonSerializer.Deserialize<List<PotFurnaceGlassMeltEntry>>(File.ReadAllText(meltPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._meltEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Liebig Condenser Thermal Gradient Fractures
            string condPath = Path.Combine(directoryPath, "liebig_condenser_fracture_logs.json");
            if (File.Exists(condPath))
            {
                var list = JsonSerializer.Deserialize<List<LiebigCondenserFractureEntry>>(File.ReadAllText(condPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._condenserEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Ground-Glass Joint Vacuum Greasing Audits
            string greasePath = Path.Combine(directoryPath, "ground_glass_joint_greasing_audits.json");
            if (File.Exists(greasePath))
            {
                var list = JsonSerializer.Deserialize<List<GroundGlassJointGreaseEntry>>(File.ReadAllText(greasePath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._greaseEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Annealing Lehr Stress Birefringence Checks
            string annealPath = Path.Combine(directoryPath, "annealing_lehr_birefringence_records.json");
            if (File.Exists(annealPath))
            {
                var list = JsonSerializer.Deserialize<List<AnnealingLehrBirefringenceEntry>>(File.ReadAllText(annealPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._annealEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public PotFurnaceGlassMeltEntry? GetMelt(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is PotFurnaceGlassMeltEntry e ? e : null;
        }

        public LiebigCondenserFractureEntry? GetCondenser(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is LiebigCondenserFractureEntry e ? e : null;
        }

        public GroundGlassJointGreaseEntry? GetGrease(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GroundGlassJointGreaseEntry e ? e : null;
        }

        public AnnealingLehrBirefringenceEntry? GetAnneal(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is AnnealingLehrBirefringenceEntry e ? e : null;
        }
    }
}
