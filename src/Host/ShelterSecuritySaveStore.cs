// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterSecuritySaveStore
// Core State : Ashfall.Core.Shelter.ShelterSecurityState
// Host Caller: Main.ShelterSecurity / ShelterSecurityHostSession
// Purpose    : Plan 138 — shelter security zones, clearances, locks, lockdowns, and breaches
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterSecuritySaveStore
    {
        public const string FileName = "shelter_security_save.json";
        public const string SectionName = "shelter_security";

        private static readonly SaveStore<ShelterSecurityState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterSecuritySaveStore),
            SchemaVersionedEnvelope<ShelterSecurityState>.Encode,
            SchemaVersionedEnvelope<ShelterSecurityState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ShelterSecurityState state) => s_store.TrySave(state);
        public static ShelterSecurityState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterSecurityState state) => s_store.CapturePersisted(state);
    }
}
