// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterAtmosphereSaveStore
// Core State : Ashfall.Core.Shelter.AtmosphereState
// Host Caller: Main.ShelterAtmosphere / ShelterAtmosphereHostSession
// Purpose    : Plan 220 — composite mood, active profile, and environmental facets
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterAtmosphereSaveStore
    {
        public const string FileName = "shelter_atmosphere_save.json";
        public const string SectionName = "shelter_atmosphere";

        private static readonly SaveStore<AtmosphereState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ShelterAtmosphereSaveStore),
            SchemaVersionedEnvelope<AtmosphereState>.Encode,
            SchemaVersionedEnvelope<AtmosphereState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(AtmosphereState state) => s_store.TrySave(state);
        public static AtmosphereState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(AtmosphereState state) => s_store.CapturePersisted(state);
    }
}
