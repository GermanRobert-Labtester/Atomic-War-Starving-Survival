// ============================================================================
// Save Store : CampaignDaySaveStore
// Core State : Ashfall.Core.Campaign.CampaignDaySave
// Host Caller: Main.Campaign / CampaignDayHostSession
// Purpose    : Campaign day clock, day-transition lifecycle, and active day index
// ============================================================================
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="CampaignDaySave"/> as JSON under
    /// <c>user://campaign_day_save.json</c> — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// validation live in <see cref="CampaignDaySaveCodec"/>; path
    /// resolution, atomic write, and error handling live in the service. This
    /// section keeps its codec-based capture (not bare state).
    /// </summary>
    public static class CampaignDaySaveStore
    {
        public const string FileName = "campaign_day_save.json";
        public const string SectionName = "campaign_day";

        private static readonly SaveStore<CampaignDaySave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(CampaignDaySaveStore),
            (save, json) => CampaignDaySaveCodec.EncodeToString(save, json),
            (raw, json) => CampaignDaySaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Capture state to JSON through the codec without writing to disk.</summary>
        public static string TryCapture(CampaignDaySave state) => s_store.CaptureEncoded(state);

        /// <summary>Restore state from JSON through the codec without reading from disk.</summary>
        public static CampaignDaySave? TryRestore(string json) => s_store.RestoreEncoded(json);

        public static bool TrySave(CampaignDaySave save) => s_store.TrySave(save);

        public static CampaignDaySave? TryLoad() => s_store.TryLoad();
    }
}
