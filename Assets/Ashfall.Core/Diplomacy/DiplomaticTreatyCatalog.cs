using System;
using System.Collections.Generic;
using Ashfall.Core.Catalogs;

namespace Ashfall.Core.Diplomacy
{
    /// <summary>
    /// Authored treaty framework (diplomatic_treaties.json). Frameworks are
    /// faction-agnostic — eligibility is by tag, never by hard-coded faction id.
    /// </summary>
    [Serializable]
    public sealed class DiplomaticTreatyDefinition
    {
        [Serializable]
        public sealed class ConcessionEntry
        {
            public string concession_kind = "goods"; // goods | service | guarantee
            public string item_id = string.Empty;
            public int amount = 1;
        }

        public string treaty_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<string>? eligible_faction_tags;
        public int minimum_signatories = 2;
        public List<ConcessionEntry>? required_concessions;
        public int duration_days;
        public int stability_rating;               // 0..100 starting stability
        public int violation_tolerance;            // violations tolerated before collapse (>=0)
        public bool guarantee_allowed;
        public List<string>? dmz_zone_ids;         // regions placed under DMZ while ratified
        public List<string>? agenda_clauses;
        public int violation_penalty_standing;     // standing delta routed via faction port on violation
        public List<string>? tags;

        public bool EligibleForTag(string factionTag) =>
            eligible_faction_tags != null && factionTag != null && eligible_faction_tags.Contains(factionTag);

        public bool IsDmzZone(string zoneId) =>
            dmz_zone_ids != null && zoneId != null && dmz_zone_ids.Contains(zoneId);
    }

    [Serializable]
    public sealed class DiplomaticTreatyCatalogContainer
    {
        public List<DiplomaticTreatyDefinition> treaties = new();
    }

    /// <summary>Loads and validates diplomatic_treaties.json (the authority).</summary>
    public static class DiplomaticTreatyCatalogLoader
    {
        public const string DefaultFileName = "diplomatic_treaties.json";
        public const int ExpectedTreatyCount = 8;

        /// <param name="validZoneIds">
        /// Zone ids owned by the world catalogs; DMZ references must resolve
        /// against this live list (empty list skips zone cross-validation,
        /// e.g. in early bootstrap).
        /// </param>
        public static List<DiplomaticTreatyDefinition> Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json,
            ISet<string>? validZoneIds = null)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                throw new InstitutionCatalogException(DefaultFileName, new[] { $"catalog file not found: {path}" });

            string rawText = fileIO.ReadAllText(path);
            var container = json.Deserialize<DiplomaticTreatyCatalogContainer>(rawText)
                ?? throw new InstitutionCatalogException(DefaultFileName, new[] { "catalog root failed to deserialize" });

            Validate(DefaultFileName, container.treaties, validZoneIds);
            return container.treaties;
        }

        public static void Validate(
            string catalogName,
            List<DiplomaticTreatyDefinition>? treaties,
            ISet<string>? validZoneIds = null)
        {
            var f = new InstitutionCatalogParse.Findings();
            if (treaties == null || treaties.Count == 0)
            {
                f.Add("*", "treaties", "catalog defines no treaty frameworks");
                f.ThrowIfAny(catalogName);
                return;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var t in treaties)
            {
                string id = t.treaty_id;
                f.RequireNonEmpty(id, "treaty_id", id);
                if (!InstitutionCatalogParse.IsCanonicalSnakeCase(id))
                    f.Add(id, "treaty_id", "must be canonical snake_case");
                if (!seen.Add(id))
                    f.Add(id, "treaty_id", "duplicate treaty_id within catalog");

                f.RequireNonEmpty(id, "display_name", t.display_name);
                f.RequirePositive(id, "minimum_signatories", t.minimum_signatories, "signatories");
                f.RequirePositive(id, "duration_days", t.duration_days, "days");
                f.RequireRange(id, "stability_rating", t.stability_rating, 0, 100);
                if (t.violation_tolerance < 0)
                    f.Add(id, "violation_tolerance", $"must be >= 0, got {t.violation_tolerance}");
                f.RequireRange(id, "violation_penalty_standing", t.violation_penalty_standing, -100, 0);

                if (t.required_concessions != null)
                {
                    for (int i = 0; i < t.required_concessions.Count; i++)
                    {
                        var c = t.required_concessions[i];
                        if (string.IsNullOrWhiteSpace(c.item_id))
                            f.Add(id, $"required_concessions[{i}].item_id", "must be a non-empty item id");
                        if (c.amount <= 0)
                            f.Add(id, $"required_concessions[{i}].amount", $"must be > 0, got {c.amount}");
                        if (c.concession_kind is not ("goods" or "service" or "guarantee"))
                            f.Add(id, $"required_concessions[{i}].concession_kind", $"unknown kind '{c.concession_kind}'");
                    }
                }

                if (t.dmz_zone_ids != null && validZoneIds != null && validZoneIds.Count > 0)
                {
                    foreach (string zone in t.dmz_zone_ids)
                    {
                        if (!validZoneIds.Contains(zone))
                            f.Add(id, "dmz_zone_ids", $"unknown zone id '{zone}'");
                    }
                }
            }

            f.ThrowIfAny(catalogName);
        }
    }
}
