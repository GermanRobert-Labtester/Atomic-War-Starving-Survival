using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// One authored morale-contagion event template (Flagship XI — Plan 154).
    /// Emotion channel pressure templates the contagion system instantiates; the
    /// system owns propagation, the catalog only parameterizes sources.
    /// </summary>
    public sealed class ContagionEventDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Hope | Despair | Panic — typed channel label (see MoraleContagionSystem).</summary>
        public string emotion_type { get; set; } = string.Empty;
        /// <summary>Source intensity at the origin survivor, 0..1.</summary>
        public float base_intensity { get; set; }
        public int duration_days { get; set; } = 1;
        /// <summary>Influence scaling per strength of social bond to the source.</summary>
        public float bond_multiplier { get; set; } = 1f;
        /// <summary>Influence scaling per physical/shift proximity to the source.</summary>
        public float proximity_multiplier { get; set; } = 1f;
        /// <summary>Fraction of remaining intensity each source decays per day.</summary>
        public float recovery_per_day { get; set; } = 0.2f;
        public List<string> tags { get; set; } = new List<string>();
    }

    /// <summary>Container shape for contagion_events.json (the authority).</summary>
    public sealed class ContagionEventsCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<ContagionEventDef> contagion_events { get; set; } = new List<ContagionEventDef>();
    }

    /// <summary>
    /// Loads morale-contagion event templates from JSON. Engine-agnostic: uses
    /// IFileIO and IJsonSerializer ports. The MoraleContagionSystem remains the
    /// sole propagation authority — the catalog only parameterizes sources.
    /// </summary>
    public static class ContagionEventCatalogLoader
    {
        public const string DefaultFileName = "contagion_events.json";

        public static ContagionEventsCatalogContainer Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new ContagionEventsCatalogContainer();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new ContagionEventsCatalogContainer();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new ContagionEventsCatalogContainer();

            var container = json.Deserialize<ContagionEventsCatalogContainer>(rawText);
            return container ?? new ContagionEventsCatalogContainer();
        }
    }
}
