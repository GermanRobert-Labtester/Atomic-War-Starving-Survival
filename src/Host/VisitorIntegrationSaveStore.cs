// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : VisitorIntegrationSaveStore
// Core State : Ashfall.Core.Visitors.VisitorIntegrationSaveState
// Host Caller: Main.VisitorIntegration / VisitorIntegrationHostSession
// Purpose    : Plan 214 — admitted visitor stays, temporary housing,
//              integration requirements, monitoring references, and departures
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Visitors;

namespace AtomicWar.GodotApp
{
    public static class VisitorIntegrationSaveStore
    {
        public const string FileName = "visitor_integration_save.json";
        public const string SectionName = "visitor_integration";

        private static readonly SaveStore<VisitorIntegrationSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(VisitorIntegrationSaveStore),
            SchemaVersionedEnvelope<VisitorIntegrationSaveState>.Encode,
            SchemaVersionedEnvelope<VisitorIntegrationSaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(VisitorIntegrationSaveState state) => s_store.TrySave(state);
        public static VisitorIntegrationSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(VisitorIntegrationSaveState state) => s_store.CapturePersisted(state);
    }
}
