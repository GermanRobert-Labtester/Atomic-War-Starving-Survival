using System;
using System.Collections.Generic;

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// One fictional pathogen strain (Flagship XI — Plan 155). A strain is a
    /// gameplay variant of an authored disease: it overrides the parent's
    /// abstract outcome/spread classes and may mutate (a pure state transition)
    /// into sibling strains. No real-world laboratory property is modeled.
    /// </summary>
    public sealed class PathogenStrainDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Strict reference to the authored parent disease_* id whose vector/tell the strain inherits.</summary>
        public string strain_of { get; set; } = string.Empty;
        public int incubation_days { get; set; } = 1;
        public int illness_days { get; set; } = 5;
        /// <summary>Abstract outcome class, 0..1.</summary>
        public float lethality { get; set; }
        /// <summary>Abstract spread class, 0..1.</summary>
        public float infectivity { get; set; }
        /// <summary>How strongly the canonical radiation dose raises this strain's severity, 0..1. Read-only coupling.</summary>
        public float radiation_severity_gain { get; set; }
        /// <summary>Per-active-infection chance per day that the strain mutates into one of mutation_targets. 0..1.</summary>
        public float mutation_chance_per_day { get; set; }
        /// <summary>Fictional mutation graph: sibling strain ids this strain may transition into.</summary>
        public List<string> mutation_targets { get; set; } = new List<string>();
        /// <summary>Treatment-affinity labels (supportive/suppressant/...), vocabulary only.</summary>
        public List<string> treatment_tags { get; set; } = new List<string>();
        public string fictional_note { get; set; } = string.Empty;
    }

    /// <summary>Container shape for pathogens.json (the authority).</summary>
    public sealed class PathogenStrainCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<PathogenStrainDef> pathogen_strains { get; set; } = new List<PathogenStrainDef>();
    }

    /// <summary>
    /// Loads fictional pathogen strains from JSON. Engine-agnostic: uses IFileIO
    /// and IJsonSerializer ports. Strains are merged into the canonical
    /// DiseaseCatalog by the strain system (no parallel disease engine).
    /// </summary>
    public static class PathogenStrainCatalogLoader
    {
        public const string DefaultFileName = "pathogens.json";

        public static PathogenStrainCatalogContainer Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new PathogenStrainCatalogContainer();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new PathogenStrainCatalogContainer();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new PathogenStrainCatalogContainer();

            var container = json.Deserialize<PathogenStrainCatalogContainer>(rawText);
            return container ?? new PathogenStrainCatalogContainer();
        }
    }
}
