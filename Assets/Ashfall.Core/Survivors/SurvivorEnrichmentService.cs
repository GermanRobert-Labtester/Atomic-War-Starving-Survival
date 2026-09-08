using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Pure C# read-only projection model for survivor identity.
    /// Composes canonical definition metadata with authored static enrichment
    /// (professions, beliefs, keepsakes, phantom backgrounds).
    /// Adheres to Plan 137: non-mechanical, descriptive, zero stat inflation.
    /// </summary>
    public sealed class SurvivorEnrichmentView
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfessionId { get; set; } = string.Empty;
        public string ProfessionLabel { get; set; } = string.Empty;
        public string BeliefProfileId { get; set; } = string.Empty;
        public string BeliefProfileLabel { get; set; } = string.Empty;
        public string PersonalKeepsakeItemId { get; set; } = string.Empty;
        public string KeepsakeItemLabel { get; set; } = string.Empty;
        public string PhantomBackgroundId { get; set; } = string.Empty;
        public string PhantomBackgroundLabel { get; set; } = string.Empty;
        public string PhilosophicalStance { get; set; } = string.Empty;
        public string ManifestoLawCode { get; set; } = string.Empty;
        public bool IsEnriched { get; set; }
    }

    /// <summary>
    /// Engine-agnostic read-only projection service.
    /// Composes static definition metadata, campaign identity, and authored
    /// enrichment into presentation-ready views without mutating gameplay authorities.
    /// </summary>
    public sealed class SurvivorEnrichmentService
    {
        private readonly ExpansionEnrichmentCatalog _enrichment;
        private readonly Dictionary<string, SurvivorEnrichmentView> _viewCache =
            new Dictionary<string, SurvivorEnrichmentView>(StringComparer.Ordinal);

        public ExpansionEnrichmentCatalog Catalog => _enrichment;

        public SurvivorEnrichmentService(ExpansionEnrichmentCatalog? enrichment = null)
        {
            _enrichment = enrichment ?? new ExpansionEnrichmentCatalog();
        }

        public SurvivorEnrichmentView GetView(string survivorId, SurvivorDefinition? def = null)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new SurvivorEnrichmentView { DisplayName = "Unknown" };

            if (_viewCache.TryGetValue(survivorId, out var cached))
                return cached;

            var fields = _enrichment.GetSurvivorFields(survivorId);
            var view = Project(survivorId, def, fields);
            _viewCache[survivorId] = view;
            return view;
        }

        private static SurvivorEnrichmentView Project(string survivorId, SurvivorDefinition? def, ExpansionSurvivorFields? fields)
        {
            string name = !string.IsNullOrEmpty(def?.displayName) ? def.displayName : FormatId(survivorId);
            bool isEnriched = fields != null;

            // Profession resolution: explicit pre-war profession ID, fallback to definition's profession string
            string profId = fields?.pre_war_profession_id ?? string.Empty;
            string profLabel;
            if (!string.IsNullOrEmpty(profId))
            {
                profLabel = FormatProfession(profId);
            }
            else if (!string.IsNullOrEmpty(def?.profession))
            {
                profLabel = def.profession;
            }
            else
            {
                profLabel = "Unspecified";
            }

            // Belief profile resolution
            string beliefId = fields?.belief_profile_id ?? string.Empty;
            string beliefLabel = !string.IsNullOrEmpty(beliefId)
                ? FormatBelief(beliefId)
                : "Undeclared";

            // Keepsake resolution
            string keepsakeId = fields?.personal_keepsake_item_id ?? string.Empty;
            string keepsakeLabel = !string.IsNullOrEmpty(keepsakeId)
                ? FormatId(keepsakeId)
                : "None";

            // Phantom background resolution
            string phantomId = fields?.phantom_background_id ?? string.Empty;
            string phantomLabel = !string.IsNullOrEmpty(phantomId)
                ? FormatPhantom(phantomId)
                : "Survivor";

            return new SurvivorEnrichmentView
            {
                SurvivorId = survivorId,
                DisplayName = name,
                ProfessionId = profId,
                ProfessionLabel = profLabel,
                BeliefProfileId = beliefId,
                BeliefProfileLabel = beliefLabel,
                PersonalKeepsakeItemId = keepsakeId,
                KeepsakeItemLabel = keepsakeLabel,
                PhantomBackgroundId = phantomId,
                PhantomBackgroundLabel = phantomLabel,
                PhilosophicalStance = fields?.philosophical_stance ?? string.Empty,
                ManifestoLawCode = fields?.manifesto_law_code ?? string.Empty,
                IsEnriched = isEnriched
            };
        }

        public static string FormatProfession(string profId) => profId switch
        {
            "nurse" => "Registered Nurse",
            "machinist" => "Industrial Machinist",
            "electrician" => "High-Voltage Electrician",
            "teacher" => "Secondary Educator",
            _ => FormatId(profId)
        };

        public static string FormatBelief(string beliefId) => beliefId switch
        {
            "atheist_rationalist" => "Material Rationalism",
            "collectivist_solidarity" => "Collectivist Solidarity",
            "military_discipline" => "Military Discipline",
            "pacifist" => "Nonviolent Pacifism",
            "pragmatic_individualism" => "Pragmatic Individualism",
            "religious_faith" => "Spiritual Faith",
            "superstitious_traditional" => "Traditional Observance",
            _ => FormatId(beliefId)
        };

        public static string FormatPhantom(string phantomId) => phantomId switch
        {
            "child_refugee" => "Displaced Youth",
            "driver" => "Transport Driver",
            "electrician" => "Grid Systems",
            "former_soldier" => "Defense Veteran",
            "generic" => "Civilian",
            "machinist" => "Tool & Die Crafts",
            "miner" => "Deep Ore Extraction",
            "nurse" => "Clinical Care",
            "teacher" => "Instruction & Lore",
            _ => FormatId(phantomId)
        };

        public static string FormatId(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            var parts = id.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1).ToLowerInvariant();
            }
            return string.Join(" ", parts);
        }
    }
}
