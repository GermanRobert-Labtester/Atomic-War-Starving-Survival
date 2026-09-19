// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterReputationSaveStore
// Core State : Ashfall.Core.Reputation.ShelterReputationState
// Host Caller: Main.ShelterReputation / ShelterReputationHostSession
// Purpose    : Plan 207 — shelter reputation, notoriety, public tags, and external perception
// ============================================================================
using Ashfall.Core.Reputation;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ShelterReputationSaveStore
    {
        public const string FileName = "shelter_reputation_save.json";
        public const string SectionName = "shelter_reputation";

        private static readonly SaveStore<ShelterReputationState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterReputationSaveStore),
            SchemaVersionedEnvelope<ShelterReputationState>.Encode,
            SchemaVersionedEnvelope<ShelterReputationState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ShelterReputationState state) => s_store.TrySave(state);
        public static ShelterReputationState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterReputationState state) => s_store.CapturePersisted(state);
    }
}
