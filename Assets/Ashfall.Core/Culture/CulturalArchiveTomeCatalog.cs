using System;
using System.Collections.Generic;
using System.Globalization;
using Ashfall.Core.Catalogs;

namespace Ashfall.Core.Culture
{
    /// <summary>
    /// Authored definition for one archival tome (cultural_archive_tomes.json).
    /// Static content only — runtime degradation/transcription state lives in
    /// CulturalArchiveVaultSystem.ArchiveDocumentState.
    /// </summary>
    [Serializable]
    public sealed class CulturalArchiveTomeDefinition
    {
        public string tome_id = string.Empty;
        public string display_name = string.Empty;
        public string category = string.Empty;
        public string description = string.Empty;
        public int transcription_days;
        public int paper_brittleness_tier = 1;      // 1 supple .. 3 brittle
        public int initial_degradation_permille;    // 0..1000
        public float microfiche_frame_density;      // > 0
        public int knowledge_bonus;                 // 0..10, consumed via codex unlock weight
        public float morale_effect;                 // -5..5, authored shelter morale contribution
        public List<InstitutionCatalogParse.CatalogCostEntry>? restoration_costs;
        public List<InstitutionCatalogParse.CatalogCostEntry>? microfiche_costs;
        public List<string>? tags;

        public bool HasTag(string tag) => tags != null && tags.Contains(tag);
    }

    [Serializable]
    public sealed class CulturalArchiveTomeCatalogContainer
    {
        public List<CulturalArchiveTomeDefinition> tomes = new();
    }

    /// <summary>Loads and validates cultural_archive_tomes.json (the authority).</summary>
    public static class CulturalArchiveTomeCatalogLoader
    {
        public const string DefaultFileName = "cultural_archive_tomes.json";
        public const int ExpectedTomeCount = 12;

        public static List<CulturalArchiveTomeDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                throw new InstitutionCatalogException(DefaultFileName, new[] { $"catalog file not found: {path}" });

            string rawText = fileIO.ReadAllText(path);
            var container = json.Deserialize<CulturalArchiveTomeCatalogContainer>(rawText)
                ?? throw new InstitutionCatalogException(DefaultFileName, new[] { "catalog root failed to deserialize" });

            Validate(DefaultFileName, container.tomes);
            return container.tomes;
        }

        public static void Validate(string catalogName, List<CulturalArchiveTomeDefinition>? tomes)
        {
            var f = new InstitutionCatalogParse.Findings();
            if (tomes == null || tomes.Count == 0)
            {
                f.Add("*", "tomes", "catalog defines no tomes");
                f.ThrowIfAny(catalogName);
                return;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var t in tomes)
            {
                string id = t.tome_id;
                f.RequireNonEmpty(id, "tome_id", id);
                if (!InstitutionCatalogParse.IsCanonicalSnakeCase(id))
                    f.Add(id, "tome_id", "must be canonical snake_case");
                if (!seen.Add(id))
                    f.Add(id, "tome_id", "duplicate tome_id within catalog");

                f.RequireNonEmpty(id, "display_name", t.display_name);
                f.RequireNonEmpty(id, "category", t.category);
                f.RequirePositive(id, "transcription_days", t.transcription_days, "days");
                f.RequireRange(id, "paper_brittleness_tier", t.paper_brittleness_tier, 1, 3);
                f.RequireRange(id, "initial_degradation_permille", t.initial_degradation_permille, 0, 1000);
                f.RequireAtLeastZero(id, "microfiche_frame_density", t.microfiche_frame_density);
                if (t.microfiche_frame_density <= 0f)
                    f.Add(id, "microfiche_frame_density", "must be > 0");
                f.RequireRange(id, "knowledge_bonus", t.knowledge_bonus, 0, 10);
                f.RequireRange(id, "morale_effect", t.morale_effect, -5f, 5f);
                f.RequireCostItems(id, "restoration_costs", t.restoration_costs);
                f.RequireCostItems(id, "microfiche_costs", t.microfiche_costs);
            }

            f.ThrowIfAny(catalogName);
        }

        /// <summary>Canonical degradation label for diagnostics (permille → tier name).</summary>
        public static string DescribeDegradation(int permille) =>
            $"{permille.ToString(CultureInfo.InvariantCulture)}\u2030";
    }
}
