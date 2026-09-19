// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : TimeCapsuleSaveStore
// Core State : Ashfall.Core.Communication.TimeCapsuleState
// Host Caller: Main.TimeCapsule / TimeCapsuleHostSession
// Purpose    : Plan 212 — Time capsules, legacy messages, delayed discovery, and cross-generational communication
// ============================================================================
using Ashfall.Core.Communication;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class TimeCapsuleSaveStore
    {
        public const string FileName = "time_capsules_save.json";
        public const string SectionName = "time_capsules";

        private static readonly SaveStore<TimeCapsuleState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(TimeCapsuleSaveStore),
            SchemaVersionedEnvelope<TimeCapsuleState>.Encode,
            SchemaVersionedEnvelope<TimeCapsuleState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(TimeCapsuleState state) => s_store.TrySave(state);
        public static TimeCapsuleState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(TimeCapsuleState state) => s_store.CapturePersisted(state);
    }
}
