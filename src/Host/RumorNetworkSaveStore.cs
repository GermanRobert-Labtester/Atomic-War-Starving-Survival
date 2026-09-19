// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RumorNetworkSaveStore
// Core State : Ashfall.Core.InformationFlow.RumorNetworkState
// Host Caller: Main.RumorNetwork / RumorNetworkHostSession
// Purpose    : Plan 203 / 131 — wasteland rumors, information hubs, propagation, and intercepts
// ============================================================================
using Ashfall.Core.InformationFlow;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RumorNetworkSaveStore
    {
        public const string FileName = "rumor_network_save.json";
        public const string SectionName = "wasteland_rumors";

        private static readonly SaveStore<RumorNetworkState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RumorNetworkSaveStore),
            SchemaVersionedEnvelope<RumorNetworkState>.Encode,
            SchemaVersionedEnvelope<RumorNetworkState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RumorNetworkState state) => s_store.TrySave(state);
        public static RumorNetworkState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RumorNetworkState state) => s_store.CapturePersisted(state);
    }
}
