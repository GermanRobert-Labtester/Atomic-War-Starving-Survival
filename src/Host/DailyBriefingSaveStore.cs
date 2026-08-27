// ============================================================================
// Save Store : DailyBriefingSaveStore
// Core State : Ashfall.Core.Campaign.DailyBriefingSave
// Host Caller: Main.Campaign / DailyBriefingHostSession
// Purpose    : Daily morning briefings, priority bulletins, and broadcast log archives
// ============================================================================
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists <see cref="DailyBriefingSave"/> as JSON under
    /// <c>user://daily_briefing_save.json</c> — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Shape and
    /// validation live in <see cref="DailyBriefingSaveCodec"/>; path
    /// resolution, atomic write, and error handling live in the service.
    /// </summary>
    public static class DailyBriefingSaveStore
    {
        public const string FileName = "daily_briefing_save.json";
        public const string SectionName = "daily_briefing";

        private static readonly SaveStore<DailyBriefingSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(DailyBriefingSaveStore),
            (save, json) => DailyBriefingSaveCodec.EncodeToString(save, json),
            (raw, json) => DailyBriefingSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(DailyBriefingSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static DailyBriefingSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(DailyBriefingSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static DailyBriefingSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(DailyBriefingSave save) => s_store.TrySave(save);

        public static DailyBriefingSave? TryLoad() => s_store.TryLoad();
    }
}
