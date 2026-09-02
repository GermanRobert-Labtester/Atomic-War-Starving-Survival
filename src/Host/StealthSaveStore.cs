// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : StealthSaveStore
// Core State : Ashfall.Core.Combat.StealthState
// Host Caller: Main.Plans178_181
// Purpose    : Expedition stealth, detection risk, camouflage, and night ops
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class StealthSaveStore
    {
        public const string FileName = "stealth_save.json";
        public const string SectionName = "expedition_stealth";

        private static readonly SaveStore<StealthState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(StealthSaveStore),
            SchemaVersionedEnvelope<StealthState>.Encode,
            SchemaVersionedEnvelope<StealthState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(StealthState state) => s_store.TrySave(state);
        public static StealthState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(StealthState state) => s_store.CapturePersisted(state);
    }
}
