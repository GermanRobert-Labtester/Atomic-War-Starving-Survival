// ============================================================================
// Save Store : AquaponicsSaveStore
// Core State : Ashfall.Core.Shelter.AquaponicsState
// Host Caller: Main.PlansB86_B89
// Purpose    : Plan B87 — closed-loop aquaponics ecology, biofilter health,
//              disease bands, and harvest/nutrient export state.
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Aquaponics save persistence — thin façade over Core SaveStore&lt;T&gt;
    /// via SaveStoreHub. Ships the legacy
    /// <c>{ SchemaVersion, State, Checksum }</c> envelope.
    /// </summary>
    public static class AquaponicsSaveStore
    {
        public const string FileName = "aquaponics_save.json";
        public const string SectionName = "aquaponics";

        private static readonly SaveStore<AquaponicsState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AquaponicsSaveStore),
            SchemaVersionedEnvelope<AquaponicsState>.Encode,
            SchemaVersionedEnvelope<AquaponicsState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCaptureDirect(AquaponicsState state) => s_store.CaptureBare(state);
        public static AquaponicsState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
        public static string TryCapture(AquaponicsState state) => s_store.CaptureBare(state);
        public static AquaponicsState? TryRestore(string json) => s_store.RestoreBare(json);
        public static bool TrySave(AquaponicsState state) => s_store.TrySave(state);
        public static AquaponicsState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AquaponicsState state) => s_store.CapturePersisted(state);
    }
}
