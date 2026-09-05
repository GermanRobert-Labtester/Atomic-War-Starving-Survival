// ============================================================================
// Save Store : BallisticShieldSaveStore
// Core State : Ashfall.Core.Combat.BallisticShieldState
// Host Caller: Main.Plans110_113 / BallisticShieldHostSession
// Purpose    : Plans 110-113 — defensive ballistic shields, stances,
//              directional block rating, and ground anchoring.
// ============================================================================
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class BallisticShieldSaveStore
    {
        public const string FileName = "ballistic_shield_save.json";
        public const string SectionName = "ballistic_shield";

        private static readonly SaveStore<BallisticShieldState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(BallisticShieldSaveStore),
            SchemaVersionedEnvelope<BallisticShieldState>.Encode,
            SchemaVersionedEnvelope<BallisticShieldState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(BallisticShieldState state) => s_store.CaptureBare(state);
        public static BallisticShieldState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static string TryCapture(BallisticShieldState state) => s_store.CaptureBare(state);
        public static BallisticShieldState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(BallisticShieldState state) => s_store.TrySave(state);
        public static BallisticShieldState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(BallisticShieldState state) => s_store.CapturePersisted(state);
    }
}
