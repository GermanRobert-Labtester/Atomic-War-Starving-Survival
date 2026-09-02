// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterSocialSaveStore
// Core State : Ashfall.Core.Shelter.ShelterSocialSave
// Host Caller: Main.ShelterSocialDynamics
// Purpose    : Living quarters privacy pressure, communal mess hall, and disputes
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterSocialSaveStore
    {
        public const string FileName = "shelter_social_dynamics_save.json";
        public const string SectionName = "shelter_social_dynamics";

        private static readonly SaveStore<ShelterSocialSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterSocialSaveStore),
            SchemaVersionedEnvelope<ShelterSocialSave>.Encode,
            SchemaVersionedEnvelope<ShelterSocialSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ShelterSocialSave state) => s_store.TrySave(state);
        public static ShelterSocialSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterSocialSave state) => s_store.CapturePersisted(state);
    }
}
