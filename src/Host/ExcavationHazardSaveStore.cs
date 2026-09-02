// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ExcavationHazardSaveStore
// Core State : Ashfall.Core.Excavation.ExcavationHazardSave
// Host Caller: Main.ExcavationHazards
// Purpose    : Subterranean methane, flood, spore hazards, and cave-in rescue operations
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class ExcavationHazardSaveStore
    {
        public const string FileName = "excavation_hazards_save.json";
        public const string SectionName = "excavation_hazards";

        private static readonly SaveStore<ExcavationHazardSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ExcavationHazardSaveStore),
            SchemaVersionedEnvelope<ExcavationHazardSave>.Encode,
            SchemaVersionedEnvelope<ExcavationHazardSave>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(ExcavationHazardSave state) => s_store.TrySave(state);
        public static ExcavationHazardSave? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(ExcavationHazardSave state) => s_store.CapturePersisted(state);
    }
}
