// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PsychologyArcSaveStore
// Core State : Ashfall.Core.Survivors.PsychologicalArcState
// Host Caller: Main.Plans162_165
// Purpose    : Breakdown-arc stages, exposure accumulators, behavior
//              cooldowns, private-stash ledgers, and catharsis resilience.
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class PsychologyArcSaveStore
    {
        public const string FileName = "psychological_arcs_save.json";
        public const string SectionName = "psychological_arcs";

        private static readonly SaveStore<PsychologicalArcState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(PsychologyArcSaveStore),
            SchemaVersionedEnvelope<PsychologicalArcState>.Encode,
            SchemaVersionedEnvelope<PsychologicalArcState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(PsychologicalArcState state) => s_store.TrySave(state);
        public static PsychologicalArcState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(PsychologicalArcState state) => s_store.CapturePersisted(state);
    }
}
