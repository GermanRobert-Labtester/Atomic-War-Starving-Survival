// SPDX-License-Identifier: MIT
// ============================================================================
// Authority : Assets/StreamingAssets/Data/narrative/*.json (authored content)
//             Assets/Ashfall.Core/Narrative/*Catalog.cs (typed entry DTOs)
// This file : host-side file -> DTO mapping only. No gameplay logic, no
//             schema duplication, no invented values.
// ============================================================================
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// The W2-06 · Decision Point 1 · Path B revival corpus, described as specs.
    /// Each entry pairs one authored JSON file with the Core entry DTO that
    /// models it. Reviving a catalog costs one row here — the loader is shared
    /// and generic.
    ///
    /// These are the process-log families that had a live subsystem owner but
    /// no runtime consumer for their authored prose.
    /// </summary>
    public static class NarrativeAssayLogCatalogs
    {
        private static List<AssayLogLine> Rows<T>(
            List<T>? entries, string kind, System.Func<T, string, AssayLogLine> project)
        {
            var rows = new List<AssayLogLine>();
            if (entries == null) return rows;
            foreach (var e in entries)
            {
                if (e == null) continue;
                var line = project(e, kind);
                if (!string.IsNullOrWhiteSpace(line.Prose)) rows.Add(line);
            }
            return rows;
        }

        /// <summary>
        /// Authored fermentation assays and reports. Owner:
        /// <c>BioFermentationHostSession</c>; surface: <c>BioFermentationPanel</c>.
        /// </summary>
        public static IReadOnlyList<AssayLogCatalogSpec> Fermentation() => new[]
        {
            new AssayLogCatalogSpec("sourdough_mother_acidity_logs.json", "sourdough_mother_acidity",
                json => Rows(CatalogLocator.LoadWrappedList<SourdoughMotherAcidityEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "sourdough_mother_acidity",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
            new AssayLogCatalogSpec("brewers_yeast_krausen_audits.json", "brewers_yeast_krausen",
                json => Rows(CatalogLocator.LoadWrappedList<BrewersYeastKrausenEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "brewers_yeast_krausen",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
            new AssayLogCatalogSpec("silage_lactic_pit_reports.json", "silage_lactic_pit",
                json => Rows(CatalogLocator.LoadWrappedList<SilageLacticPitEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "silage_lactic_pit",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
            new AssayLogCatalogSpec("fermentation_crock_airlock_assays.json", "crock_airlock_assay",
                json => Rows(CatalogLocator.LoadWrappedList<FermentationCrockAirlockEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "crock_airlock_assay",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
        };

        /// <summary>
        /// Authored water-treatment potency and disinfection logs. Owner:
        /// <c>WaterTreatmentHostSession</c>; surface: <c>WaterTreatmentPanel</c>.
        /// </summary>
        public static IReadOnlyList<AssayLogCatalogSpec> WaterTreatment() => new[]
        {
            new AssayLogCatalogSpec("slow_sand_schmutzdecke_logs.json", "slow_sand_schmutzdecke",
                json => Rows(CatalogLocator.LoadWrappedList<SlowSandSchmutzdeckeEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "slow_sand_schmutzdecke",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
            new AssayLogCatalogSpec("ozone_contact_tower_audits.json", "ozone_contact_tower",
                json => Rows(CatalogLocator.LoadWrappedList<OzoneContactTowerEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "ozone_contact_tower",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
            new AssayLogCatalogSpec("calcium_hypochlorite_titration_reports.json", "calcium_hypochlorite_titration",
                json => Rows(CatalogLocator.LoadWrappedList<CalciumHypochloriteTitrationEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "calcium_hypochlorite_titration",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
            new AssayLogCatalogSpec("activated_carbon_adsorption_records.json", "activated_carbon_adsorption",
                json => Rows(CatalogLocator.LoadWrappedList<ActivatedCarbonAdsorptionEntry>(
                    json, NarrativeAssayLogCatalogLoader.SerializerOptions),
                    "activated_carbon_adsorption",
                    (e, k) => new AssayLogLine(e.Id, k, e.TimestampRelative, e.Prose, e.Tags))),
        };
    }
}
