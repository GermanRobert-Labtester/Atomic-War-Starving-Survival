// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterWorkshopSaveStore
// Core State : Ashfall.Core.Shelter.ShelterWorkshopSave
// Host Caller: Main.ShelterWorkshop
// Purpose    : Precision workshop tooling, ammo press, and firearm refurbishment
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterWorkshopSaveStore
    {
        public const string FileName = "shelter_workshop_save.json";
        public const string SectionName = "shelter_workshop";

        private static readonly SaveStore<ShelterWorkshopSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterWorkshopSaveStore),
            SchemaVersionedEnvelope<ShelterWorkshopSave>.Encode,
            SchemaVersionedEnvelope<ShelterWorkshopSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ShelterWorkshopSave state) => s_store.TrySave(state);
        public static ShelterWorkshopSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterWorkshopSave state) => s_store.CapturePersisted(state);
    }
}
