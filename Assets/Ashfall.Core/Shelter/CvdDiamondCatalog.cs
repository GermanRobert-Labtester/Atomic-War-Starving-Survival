// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Authored CVD diamond profile (Plan 124 Phase 2). Abstract
    /// materials-production chain — growth grades, defects, reactors, feeds,
    /// substrates, and registered high-wear consumers. No real deposition
    /// recipes or reactor procedures.
    /// </summary>
    public sealed class DiamondGrowthGrade
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int rank { get; set; }
        public int wear_factor_bp { get; set; }
        public bool requires_certification { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (rank < 0 || rank > 100) { error = "rank out of [0,100]"; return false; }
            if (wear_factor_bp < 0 || wear_factor_bp > 10000) { error = "wear_factor_bp out of [0,10000]"; return false; }
            return true;
        }
    }

    public sealed class DiamondDefectProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int grade_penalty_bp { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (grade_penalty_bp < 0 || grade_penalty_bp > 10000) { error = "grade_penalty_bp out of [0,10000]"; return false; }
            return true;
        }
    }

    public sealed class CvdReactorProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int growth_rate_per_tick_bp { get; set; }
        public int plasma_stability_baseline_bp { get; set; }
        public int magnetron_wear_per_tick_bp { get; set; }
        public int chamber_wear_per_tick_bp { get; set; }
        public float power_demand_kw { get; set; }
        public int cooling_requirement { get; set; }
        public List<string> install_item_ids { get; set; } = new List<string>();
        public List<string> repair_item_ids { get; set; } = new List<string>();
        public string maintenance_profile_id { get; set; } = string.Empty;

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (growth_rate_per_tick_bp <= 0 || growth_rate_per_tick_bp > 10000) { error = "growth_rate_per_tick_bp out of (0,10000]"; return false; }
            if (plasma_stability_baseline_bp <= 0 || plasma_stability_baseline_bp > 10000) { error = "plasma_stability_baseline_bp out of (0,10000]"; return false; }
            if (magnetron_wear_per_tick_bp < 0 || magnetron_wear_per_tick_bp > 10000) { error = "magnetron_wear_per_tick_bp out of [0,10000]"; return false; }
            if (chamber_wear_per_tick_bp < 0 || chamber_wear_per_tick_bp > 10000) { error = "chamber_wear_per_tick_bp out of [0,10000]"; return false; }
            if (power_demand_kw <= 0f || power_demand_kw > 500f) { error = "power_demand_kw out of (0,500]"; return false; }
            if (cooling_requirement < 0) { error = "cooling_requirement must be >= 0"; return false; }
            return true;
        }
    }

    public sealed class DiamondFeedProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int purity_bp { get; set; }
        public List<string> feedstock_item_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (purity_bp <= 0 || purity_bp > 10000) { error = "purity_bp out of (0,10000]"; return false; }
            return true;
        }
    }

    public sealed class DiamondSubstrateProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int quality_bp { get; set; }
        public string input_item_id { get; set; } = string.Empty;

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (quality_bp <= 0 || quality_bp > 10000) { error = "quality_bp out of (0,10000]"; return false; }
            if (string.IsNullOrWhiteSpace(input_item_id)) { error = "input_item_id is required"; return false; }
            return true;
        }
    }

    public sealed class DiamondToolComponent
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string min_grade_id { get; set; } = string.Empty;
        public bool requires_certification { get; set; }
        public List<string> accepted_consumer_tags { get; set; } = new List<string>();
        public List<string> install_item_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (string.IsNullOrWhiteSpace(min_grade_id)) { error = "min_grade_id is required"; return false; }
            return true;
        }
    }

    public sealed class DiamondConsumerMapping
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<string> accepted_component_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            return true;
        }
    }

    public sealed class DiamondMaintenanceProfile
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int inspection_interval_days { get; set; }
        public int repair_skill_minimum { get; set; }

        public bool Validate(out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(id)) { error = "id is required"; return false; }
            if (inspection_interval_days <= 0) { error = "inspection_interval_days must be > 0"; return false; }
            if (repair_skill_minimum < 0 || repair_skill_minimum > 100) { error = "repair_skill_minimum out of [0,100]"; return false; }
            return true;
        }
    }

    /// <summary>
    /// CVD diamond catalog root (Plan 124 Phase 2). Loaders/validators only;
    /// the synthesis engine arrives in Phase 4.
    /// </summary>
    public sealed class CvdDiamondCatalog
    {
        public int schema_version { get; set; } = 1;
        public string description { get; set; } = string.Empty;
        public List<DiamondGrowthGrade> growth_grades { get; set; } = new List<DiamondGrowthGrade>();
        public List<DiamondDefectProfile> defect_profiles { get; set; } = new List<DiamondDefectProfile>();
        public List<CvdReactorProfile> reactor_profiles { get; set; } = new List<CvdReactorProfile>();
        public List<DiamondFeedProfile> feed_profiles { get; set; } = new List<DiamondFeedProfile>();
        public List<DiamondSubstrateProfile> substrate_profiles { get; set; } = new List<DiamondSubstrateProfile>();
        public List<DiamondToolComponent> tool_components { get; set; } = new List<DiamondToolComponent>();
        public List<DiamondConsumerMapping> consumer_mappings { get; set; } = new List<DiamondConsumerMapping>();
        public List<DiamondMaintenanceProfile> maintenance_profiles { get; set; } = new List<DiamondMaintenanceProfile>();

        private readonly Dictionary<string, DiamondGrowthGrade> _grades = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondDefectProfile> _defects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, CvdReactorProfile> _reactors = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondFeedProfile> _feeds = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondSubstrateProfile> _substrates = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondToolComponent> _components = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondConsumerMapping> _consumers = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DiamondMaintenanceProfile> _maintenance = new(StringComparer.Ordinal);

        public void Index()
        {
            _grades.Clear();
            foreach (var d in growth_grades) if (d != null && !string.IsNullOrEmpty(d.id)) _grades[d.id] = d;
            _defects.Clear();
            foreach (var d in defect_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _defects[d.id] = d;
            _reactors.Clear();
            foreach (var d in reactor_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _reactors[d.id] = d;
            _feeds.Clear();
            foreach (var d in feed_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _feeds[d.id] = d;
            _substrates.Clear();
            foreach (var d in substrate_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _substrates[d.id] = d;
            _components.Clear();
            foreach (var d in tool_components) if (d != null && !string.IsNullOrEmpty(d.id)) _components[d.id] = d;
            _consumers.Clear();
            foreach (var d in consumer_mappings) if (d != null && !string.IsNullOrEmpty(d.id)) _consumers[d.id] = d;
            _maintenance.Clear();
            foreach (var d in maintenance_profiles) if (d != null && !string.IsNullOrEmpty(d.id)) _maintenance[d.id] = d;
        }

        public DiamondGrowthGrade? GetGrade(string id) => string.IsNullOrEmpty(id) ? null : _grades.TryGetValue(id, out var v) ? v : null;
        public DiamondDefectProfile? GetDefect(string id) => string.IsNullOrEmpty(id) ? null : _defects.TryGetValue(id, out var v) ? v : null;
        public CvdReactorProfile? GetReactor(string id) => string.IsNullOrEmpty(id) ? null : _reactors.TryGetValue(id, out var v) ? v : null;
        public DiamondFeedProfile? GetFeed(string id) => string.IsNullOrEmpty(id) ? null : _feeds.TryGetValue(id, out var v) ? v : null;
        public DiamondSubstrateProfile? GetSubstrate(string id) => string.IsNullOrEmpty(id) ? null : _substrates.TryGetValue(id, out var v) ? v : null;
        public DiamondToolComponent? GetComponent(string componentId) =>
            string.IsNullOrEmpty(componentId) ? null : _components.TryGetValue(componentId, out var v) ? v : null;
        public DiamondConsumerMapping? GetConsumer(string id) => string.IsNullOrEmpty(id) ? null : _consumers.TryGetValue(id, out var v) ? v : null;
        public DiamondMaintenanceProfile? GetMaintenance(string id) => string.IsNullOrEmpty(id) ? null : _maintenance.TryGetValue(id, out var v) ? v : null;
        public IReadOnlyCollection<DiamondGrowthGrade> Grades => _grades.Values;
        public IReadOnlyCollection<DiamondConsumerMapping> Consumers => _consumers.Values;

        /// <summary>
        /// Structural validation: duplicate-ID rejection, grade ordering
        /// (non-circular, strictly ascending ranks), per-row ranges, and
        /// internal foreign keys (component→grade, consumer→component).
        /// </summary>
        public bool ValidateCatalog(out string error)
        {
            error = string.Empty;
            if (schema_version != 1) { error = "unsupported schema_version"; return false; }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var d in growth_grades)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("grade:" + d.id)) { error = $"duplicate grade id '{d.id}'"; return false; }
            }
            foreach (var d in defect_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("defect:" + d.id)) { error = $"duplicate defect id '{d.id}'"; return false; }
            }
            foreach (var d in reactor_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("reactor:" + d.id)) { error = $"duplicate reactor id '{d.id}'"; return false; }
            }
            foreach (var d in feed_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("feed:" + d.id)) { error = $"duplicate feed id '{d.id}'"; return false; }
            }
            foreach (var d in substrate_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("substrate:" + d.id)) { error = $"duplicate substrate id '{d.id}'"; return false; }
            }
            foreach (var d in tool_components)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("component:" + d.id)) { error = $"duplicate tool_component id '{d.id}'"; return false; }
            }
            foreach (var d in consumer_mappings)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("consumer:" + d.id)) { error = $"duplicate consumer id '{d.id}'"; return false; }
            }
            foreach (var d in maintenance_profiles)
            {
                if (d == null || !d.Validate(out error)) return false;
                if (!seen.Add("maint:" + d.id)) { error = $"duplicate maintenance id '{d.id}'"; return false; }
            }

            // Grade ordering: ranks must be unique. The rank-0 sentinel is
            // the rejected batch (wear_factor_bp must be 0 — no usable
            // output). Among production grades (rank >= 1), higher rank must
            // never have a worse wear factor.
            var byRank = new SortedDictionary<int, DiamondGrowthGrade>();
            foreach (var g in growth_grades)
            {
                if (byRank.TryGetValue(g.rank, out var other))
                { error = $"duplicate grade rank {g.rank} ('{other.id}' vs '{g.id}')"; return false; }
                byRank[g.rank] = g;
                if (g.rank == 0 && g.wear_factor_bp != 0)
                { error = $"grade '{g.id}' at rank 0 (rejected sentinel) must have wear_factor_bp 0"; return false; }
            }
            for (int i = 1; i < growth_grades.Count; i++)
            {
                var lower = growth_grades[i - 1];
                var higher = growth_grades[i];
                if (lower.rank == 0) continue; // sentinel is exempt
                if (higher.rank > 0 && higher.wear_factor_bp > lower.wear_factor_bp)
                { error = $"grade ordering violated: '{higher.id}' (rank {higher.rank}) must not be worse than '{lower.id}' (rank {lower.rank})"; return false; }
            }

            foreach (var comp in tool_components)
            {
                if (GetGrade(comp.min_grade_id) == null)
                { error = $"tool_component '{comp.id}': unresolved min_grade_id '{comp.min_grade_id}'"; return false; }
                if (comp.accepted_consumer_tags.Count == 0)
                { error = $"tool_component '{comp.id}': no registered consumers (outputs need consumers)"; return false; }
            }
            foreach (var consumer in consumer_mappings)
            {
                if (consumer.accepted_component_ids.Count == 0)
                { error = $"consumer '{consumer.id}': accepts no components"; return false; }
                foreach (var componentId in consumer.accepted_component_ids)
                {
                    if (GetComponent(componentId) == null)
                    { error = $"consumer '{consumer.id}': unresolved accepted_component_id '{componentId}'"; return false; }
                }
            }
            return true;
        }
    }

    public static class CvdDiamondCatalogLoader
    {
        public const string DefaultFileName = "cvd_diamond_catalog.json";

        public static CvdDiamondCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir))
                return Empty();

            string path = fileIo.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path))
                return Empty();

            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json))
                return Empty();

            var catalog = JsonSerializer.Deserialize<CvdDiamondCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();

            catalog.Index();
            return catalog;
        }

        private static CvdDiamondCatalog Empty()
        {
            var catalog = new CvdDiamondCatalog();
            catalog.Index();
            return catalog;
        }
    }
}
