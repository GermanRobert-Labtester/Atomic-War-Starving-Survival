// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PropagandaSaveStore
// Core State : Ashfall.Core.Propaganda.PropagandaState
// Host Caller: Main.Propaganda / PropagandaHostSession
// Purpose    : Plan 168 — propaganda messages, multi-day campaigns, detection, and morale warfare
// ============================================================================
using Ashfall.Core.Propaganda;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PropagandaSaveStore
    {
        public const string FileName = "propaganda_save.json";
        public const string SectionName = "propaganda_campaigns";

        private static readonly SaveStore<PropagandaState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PropagandaSaveStore),
            SchemaVersionedEnvelope<PropagandaState>.Encode,
            SchemaVersionedEnvelope<PropagandaState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PropagandaState state) => s_store.TrySave(state);
        public static PropagandaState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PropagandaState state) => s_store.CapturePersisted(state);
    }
}
