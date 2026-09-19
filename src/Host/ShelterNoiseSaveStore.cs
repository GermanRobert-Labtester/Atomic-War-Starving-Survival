// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterNoiseSaveStore
// Core State : Ashfall.Core.Shelter.ShelterNoiseState
// Host Caller: Main.ShelterAtmosphere / ShelterAtmosphereHostSession
// Purpose    : Plan 205 — acoustic output, room soundproofing, quiet hours
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterNoiseSaveStore
    {
        public const string FileName = "shelter_noise_save.json";
        public const string SectionName = "shelter_noise";

        private static readonly SaveStore<ShelterNoiseState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterNoiseSaveStore),
            SchemaVersionedEnvelope<ShelterNoiseState>.Encode,
            SchemaVersionedEnvelope<ShelterNoiseState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ShelterNoiseState state) => s_store.TrySave(state);
        public static ShelterNoiseState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ShelterNoiseState state) => s_store.CapturePersisted(state);
    }
}
