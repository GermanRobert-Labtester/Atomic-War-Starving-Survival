// ============================================================================
// Save Store : SurvivorSocialSaveStore
// Core State : Ashfall.Core.Survivors.SurvivorSocialSaveState
// Host Caller: Main.SurvivorSocial / SurvivorSocialCoordinator
// Purpose    : Leadership, friction, ration conflict, trauma bonds, skill atrophy
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Survivor-social save persistence — thin façade over the Core
    /// <see cref="SaveStore{T}"/> service (via <see cref="SaveStoreHub"/>,
    /// checksummed-envelope flavor). One section packs all five social-
    /// mechanics DTOs into a single <see cref="SurvivorSocialSaveState"/>.
    /// </summary>
    public static class SurvivorSocialSaveStore
    {
        public const string FileName = "survivor_social_save.json";
        public const string SectionName = "survivor_social";

        private static readonly SaveStore<SurvivorSocialSaveState> s_store =
            SaveStoreHub.Checksummed<SurvivorSocialSaveState>(
                FileName,
                nameof(SurvivorSocialSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(SurvivorSocialSaveState state) => s_store.CaptureBare(state);
        public static SurvivorSocialSaveState? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        public static string TryCapture(SurvivorSocialSaveState state) => s_store.CaptureBare(state);
        public static SurvivorSocialSaveState? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(SurvivorSocialSaveState state) => s_store.TrySave(state);
        public static SurvivorSocialSaveState? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(SurvivorSocialSaveState state) => s_store.CapturePersisted(state);
    }
}
