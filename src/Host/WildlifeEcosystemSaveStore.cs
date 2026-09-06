// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : WildlifeEcosystemSaveStore
// Core State : Ashfall.Core.World.WildlifeEcosystemState
// Host Caller: Main.Plans162_165
// Purpose    : Ecology pressures, extinction flags, apex activity, tamed
//      animals, and bestiary observations. Pack populations themselves stay
//      in the `world` section (single population authority).
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class WildlifeEcosystemSaveStore
    {
        public const string FileName = "wildlife_ecosystem_save.json";
        public const string SectionName = "wildlife_ecosystem";

        private static readonly SaveStore<WildlifeEcosystemState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WildlifeEcosystemSaveStore),
            SchemaVersionedEnvelope<WildlifeEcosystemState>.Encode,
            SchemaVersionedEnvelope<WildlifeEcosystemState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(WildlifeEcosystemState state) => s_store.TrySave(state);
        public static WildlifeEcosystemState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(WildlifeEcosystemState state) => s_store.CapturePersisted(state);
    }
}
