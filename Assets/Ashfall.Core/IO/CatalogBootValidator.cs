// SPDX-License-Identifier: MIT
// ASHFALL Core: central catalog boot validation.
//
// Validates that all required catalogs load successfully at game startup.
// Required catalogs that fail to load will throw, preventing the game from
// starting with missing critical data.

using System;
using System.Collections.Generic;

namespace Ashfall.Core.IO
{
    /// <summary>
    /// Central validator for catalog loading at game boot.
    /// Tracks which catalogs are required and validates they load without errors.
    /// </summary>
    public static class CatalogBootValidator
    {
        /// <summary>Catalog load delegate for validation.</summary>
        public delegate object CatalogLoader(string dataDir, IFileIO fileIO, IJsonSerializer json);

        /// <summary>Catalog entry for validation registry.</summary>
        public class CatalogEntry
        {
            public string FileName { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public CatalogClassification Classification { get; set; } = CatalogClassification.Optional;
            public CatalogLoader? Loader { get; set; }
        }

        private static readonly List<CatalogEntry> s_registeredCatalogs = new List<CatalogEntry>();
        private static bool s_initialized = false;

        /// <summary>Register a catalog for boot validation.</summary>
        public static void RegisterCatalog(
            string fileName,
            string displayName,
            CatalogClassification classification,
            CatalogLoader? loader = null)
        {
            s_registeredCatalogs.Add(new CatalogEntry
            {
                FileName = fileName,
                DisplayName = displayName,
                Classification = classification,
                Loader = loader
            });
        }

        /// <summary>
        /// Validate all registered catalogs. For required catalogs, throws if load fails.
        /// Returns a report of all validation results.
        /// </summary>
        public static CatalogBootReport Validate(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var report = new CatalogBootReport();

            if (!s_initialized)
            {
                InitializeDefaultCatalogs();
                s_initialized = true;
            }

            int requiredCount = 0;
            int optionalCount = 0;
            int devOnlyCount = 0;

            foreach (var entry in s_registeredCatalogs)
            {
                switch (entry.Classification)
                {
                    case CatalogClassification.Required:
                        requiredCount++;
                        break;
                    case CatalogClassification.Optional:
                        optionalCount++;
                        break;
                    case CatalogClassification.DeveloperOnly:
                        devOnlyCount++;
                        break;
                }

                string path = fileIO.Combine(dataDir, entry.FileName);

                // Check if file exists
                if (!fileIO.FileExists(path))
                {
                    if (entry.Classification == CatalogClassification.Required)
                    {
                        report.AddError(entry.DisplayName, entry.FileName, "File not found");
                    }
                    else
                    {
                        report.AddWarning(entry.DisplayName, entry.FileName, "File not found (optional)");
                    }
                    continue;
                }

                // Try to load if a loader is registered
                if (entry.Loader != null)
                {
                    try
                    {
                        object? data = entry.Loader(dataDir, fileIO, json);

                        // Check if data is null or empty
                        if (data == null)
                        {
                            if (entry.Classification == CatalogClassification.Required)
                            {
                                report.AddError(entry.DisplayName, entry.FileName, "Loaded as null");
                            }
                            else
                            {
                                report.AddWarning(entry.DisplayName, entry.FileName, "Loaded as null (optional)");
                            }
                        }
                        else if (data is System.Collections.IList list && list.Count == 0)
                        {
                            if (entry.Classification == CatalogClassification.Required)
                            {
                                report.AddError(entry.DisplayName, entry.FileName, "Loaded as empty list");
                            }
                            else
                            {
                                report.AddWarning(entry.DisplayName, entry.FileName, "Loaded as empty list (optional)");
                            }
                        }
                        else
                        {
                            report.AddSuccess(entry.DisplayName, entry.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        CatalogDiagnostics.Warn(entry.DisplayName, entry.FileName, ex);
                        if (entry.Classification == CatalogClassification.Required)
                        {
                            report.AddError(entry.DisplayName, entry.FileName, "Load failed: " + ex.Message);
                        }
                        else
                        {
                            report.AddWarning(entry.DisplayName, entry.FileName, "Load failed (optional): " + ex.Message);
                        }
                    }
                }
                else
                {
                    // No loader registered, just check file exists and is valid JSON
                    try
                    {
                        string raw = fileIO.ReadAllText(path);
                        if (string.IsNullOrWhiteSpace(raw))
                        {
                            if (entry.Classification == CatalogClassification.Required)
                            {
                                report.AddError(entry.DisplayName, entry.FileName, "File is empty");
                            }
                            else
                            {
                                report.AddWarning(entry.DisplayName, entry.FileName, "File is empty (optional)");
                            }
                        }
                        else
                        {
                            // Validate JSON
                            System.Text.Json.JsonDocument.Parse(raw);
                            report.AddSuccess(entry.DisplayName, entry.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        CatalogDiagnostics.Warn(entry.DisplayName, entry.FileName, ex);
                        if (entry.Classification == CatalogClassification.Required)
                        {
                            report.AddError(entry.DisplayName, entry.FileName, "Invalid JSON: " + ex.Message);
                        }
                        else
                        {
                            report.AddWarning(entry.DisplayName, entry.FileName, "Invalid JSON (optional): " + ex.Message);
                        }
                    }
                }
            }

            report.TotalCount = requiredCount + optionalCount + devOnlyCount;
            report.RequiredCount = requiredCount;
            report.OptionalCount = optionalCount;
            report.DevOnlyCount = devOnlyCount;

            return report;
        }

        /// <summary>
        /// Initialize the default set of catalogs for validation.
        /// Called automatically on first validation.
        /// </summary>
        private static void InitializeDefaultCatalogs()
        {
            // Required catalogs - game cannot start without these
            RegisterCatalog("items.json", "Items", CatalogClassification.Required);
            RegisterCatalog("recipes.json", "Recipes", CatalogClassification.Required);
            RegisterCatalog("locations.json", "Locations", CatalogClassification.Required);
            RegisterCatalog("survivors.json", "Survivors", CatalogClassification.Required);
            RegisterCatalog("faction_lore.json", "Factions", CatalogClassification.Required);
            RegisterCatalog("economy_goods.json", "Economy Goods", CatalogClassification.Required);
            RegisterCatalog("events.json", "World Events", CatalogClassification.Required);
            RegisterCatalog("weather_seasons.json", "Weather Seasons", CatalogClassification.Required);
            RegisterCatalog("radio.json", "Radio Broadcasts", CatalogClassification.Required);
            RegisterCatalog("narrative_encounters.json", "Narrative Encounters", CatalogClassification.Required);
            RegisterCatalog("questline_master.json", "Quest Lines", CatalogClassification.Required);
            RegisterCatalog("world_history.json", "World History", CatalogClassification.Required);
            RegisterCatalog("wasteland_map_v1.json", "World Map", CatalogClassification.Required);

            // Optional catalogs - expansions and non-critical content
            RegisterCatalog("dive_sites.json", "Maritime Dive Sites", CatalogClassification.Optional);
            RegisterCatalog("foundry_accords.json", "Foundry Accords", CatalogClassification.Optional);
            RegisterCatalog("foundry_production.json", "Foundry Production", CatalogClassification.Optional);
            RegisterCatalog("foundry_treaty_consequences.json", "Foundry Treaty Consequences", CatalogClassification.Optional);
            RegisterCatalog("warlord_doctrines.json", "Warlord Doctrines", CatalogClassification.Optional);
            RegisterCatalog("combat_catalog.json", "Combat Catalog", CatalogClassification.Optional);
            RegisterCatalog("verdict_data.json", "Verdict Data", CatalogClassification.Optional);
            RegisterCatalog("verdict_items.json", "Verdict Items", CatalogClassification.Optional);
            RegisterCatalog("verdict_locations.json", "Verdict Locations", CatalogClassification.Optional);
            RegisterCatalog("verdict_radio.json", "Verdict Radio", CatalogClassification.Optional);
            RegisterCatalog("black_flotilla_items.json", "Black Flotilla Items", CatalogClassification.Optional);
            RegisterCatalog("deep_lore_locations.json", "Deep Lore Locations", CatalogClassification.Optional);
            RegisterCatalog("dose_items.json", "Dose Items", CatalogClassification.Optional);
            RegisterCatalog("dose_locations.json", "Dose Locations", CatalogClassification.Optional);
            RegisterCatalog("dose_quests.json", "Dose Quests", CatalogClassification.Optional);
            RegisterCatalog("dose_registers.json", "Dose Registers", CatalogClassification.Optional);
        }

        /// <summary>
        /// Throw if any required catalogs failed validation.
        /// Used at startup to prevent game from starting with missing required data.
        /// </summary>
        public static void ThrowIfRequiredFailed(CatalogBootReport report)
        {
            if (report.HasRequiredErrors)
            {
                var errors = new System.Text.StringBuilder();
                errors.AppendLine("Required catalog validation failed:");
                foreach (var entry in report.Entries)
                {
                    if (entry.Severity == CatalogLoadSeverity.Error || entry.Severity == CatalogLoadSeverity.Fatal)
                    {
                        errors.AppendLine("  [" + entry.Severity + "] " + entry.DisplayName + " (" + entry.FileName + "): " + entry.Message);
                    }
                }
                throw new InvalidOperationException(errors.ToString());
            }
        }
    }

    /// <summary>
    /// Report from catalog boot validation.
    /// </summary>
    public class CatalogBootReport
    {
        private readonly List<CatalogBootEntry> _entries = new List<CatalogBootEntry>();

        public int TotalCount { get; set; }
        public int RequiredCount { get; set; }
        public int OptionalCount { get; set; }
        public int DevOnlyCount { get; set; }

        public IReadOnlyList<CatalogBootEntry> Entries => _entries.AsReadOnly();

        public bool HasErrors => _entries.Exists(e => e.Severity == CatalogLoadSeverity.Error || e.Severity == CatalogLoadSeverity.Fatal);
        public bool HasRequiredErrors => _entries.Exists(e =>
            (e.Severity == CatalogLoadSeverity.Error || e.Severity == CatalogLoadSeverity.Fatal) &&
            e.IsRequired);

        public void AddSuccess(string displayName, string fileName)
        {
            _entries.Add(new CatalogBootEntry(displayName, fileName, CatalogLoadSeverity.Info, false, "OK"));
        }

        public void AddWarning(string displayName, string fileName, string message)
        {
            _entries.Add(new CatalogBootEntry(displayName, fileName, CatalogLoadSeverity.Warning, false, message));
        }

        public void AddError(string displayName, string fileName, string message)
        {
            _entries.Add(new CatalogBootEntry(displayName, fileName, CatalogLoadSeverity.Error, true, message));
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Catalog Boot Report:");
            sb.AppendLine("  Total: " + TotalCount + " (Required: " + RequiredCount + ", Optional: " + OptionalCount + ", DevOnly: " + DevOnlyCount + ")");
            sb.AppendLine("  Success: " + System.Linq.Enumerable.Count(_entries, e => e.Severity == CatalogLoadSeverity.Info));
            sb.AppendLine("  Warnings: " + System.Linq.Enumerable.Count(_entries, e => e.Severity == CatalogLoadSeverity.Warning));
            sb.AppendLine("  Errors: " + System.Linq.Enumerable.Count(_entries, e => e.Severity >= CatalogLoadSeverity.Error));
            foreach (var entry in _entries)
            {
                if (entry.Severity >= CatalogLoadSeverity.Warning)
                {
                    sb.AppendLine("    [" + entry.Severity + "] " + entry.DisplayName + " (" + entry.FileName + "): " + entry.Message);
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Single entry in a catalog boot report.
    /// </summary>
    public readonly struct CatalogBootEntry
    {
        public string DisplayName { get; }
        public string FileName { get; }
        public CatalogLoadSeverity Severity { get; }
        public bool IsRequired { get; }
        public string Message { get; }

        public CatalogBootEntry(string displayName, string fileName, CatalogLoadSeverity severity, bool isRequired, string message)
        {
            DisplayName = displayName;
            FileName = fileName;
            Severity = severity;
            IsRequired = isRequired;
            Message = message;
        }
    }
}
