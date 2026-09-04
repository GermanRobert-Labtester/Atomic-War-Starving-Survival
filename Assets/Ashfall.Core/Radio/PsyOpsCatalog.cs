using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// One authored psyops broadcast campaign template (Flagship XI — Plan 157).
    /// Values are abstract faction-level game numbers (reach, receptiveness,
    /// pressure) — no real-world targeting or persuasion guidance.
    /// </summary>
    public sealed class PsyOpsCampaignDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        /// <summary>Strict reference to the faction_* id this campaign pressures.</summary>
        public string target_faction_id { get; set; } = string.Empty;
        /// <summary>Hope | DefectionAppeal | AidPromise | Fear | Unity | CounterRumor — theme label only.</summary>
        public string message_theme { get; set; } = string.Empty;
        /// <summary>Abstract reach class 0..100 before power/jamming modifiers.</summary>
        public float base_reach { get; set; } = 50f;
        /// <summary>Transmitter draw in watts while the campaign broadcasts (canonical power grid gates it).</summary>
        public float power_demand_watts { get; set; } = 120f;
        public int duration_days { get; set; } = 5;
        /// <summary>Target faction's abstract openness to this theme, 0..1.</summary>
        public float receptiveness { get; set; } = 0.5f;
        /// <summary>Loyalty-pressure units per broadcast day at full reach and zero jamming.</summary>
        public float loyalty_pressure_per_day { get; set; } = 1f;
        /// <summary>Theme label whose active counter-campaign suppresses this one.</summary>
        public string countered_by { get; set; } = string.Empty;
        public string broadcast_note { get; set; } = string.Empty;
    }

    /// <summary>Container shape for propaganda_campaigns.json (the authority).</summary>
    public sealed class PsyOpsCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public List<PsyOpsCampaignDef> propaganda_campaigns { get; set; } = new List<PsyOpsCampaignDef>();
    }

    /// <summary>
    /// Loads psyops campaign templates from JSON. Engine-agnostic: uses IFileIO
    /// and IJsonSerializer ports. The PsyOpsSystem remains the sole campaign
    /// authority — the catalog only parameterizes templates.
    /// </summary>
    public static class PsyOpsCatalogLoader
    {
        public const string DefaultFileName = "propaganda_campaigns.json";

        public static PsyOpsCatalogContainer Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new PsyOpsCatalogContainer();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new PsyOpsCatalogContainer();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new PsyOpsCatalogContainer();

            var container = json.Deserialize<PsyOpsCatalogContainer>(rawText);
            return container ?? new PsyOpsCatalogContainer();
        }
    }
}
