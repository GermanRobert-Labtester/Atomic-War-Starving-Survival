// ============================================================================
// Save Store : CaravanSaveStore
// Core State : Ashfall.Core.TravelingCaravanState
// Host Caller: Main.Economy / TravelingCaravanHostSession
// Purpose    : Traveling caravan arrivals, trade schedules, inventory, and barter states
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Traveling Caravan save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub). Checksummed envelope,
    /// atomic write, and legacy bare-state loading live in the service.
    /// </summary>
    public static class CaravanSaveStore
    {
        public const string FileName = "caravan_save.json";
        public const string SectionName = "caravan";

        private static readonly SaveStore<TravelingCaravanState> s_store =
            SaveStoreHub.Checksummed<TravelingCaravanState>(FileName, nameof(CaravanSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(TravelingCaravanState state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static TravelingCaravanState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(TravelingCaravanState state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static TravelingCaravanState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(TravelingCaravanState state) => s_store.TrySave(state);

        public static TravelingCaravanState? TryLoad() => s_store.TryLoad();
    }
}
