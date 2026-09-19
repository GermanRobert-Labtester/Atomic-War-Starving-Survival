// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RelationshipDecaySaveStore
// Core State : Ashfall.Core.Survivors.RelationshipDecayState
// Host Caller: Main.RelationshipDecay / RelationshipDecayHostSession
// Purpose    : Plan 182 — Survivor pair bond decay, interaction tracking, and social drift
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class RelationshipDecaySaveStore
    {
        public const string FileName = "relationship_decay_save.json";
        public const string SectionName = "relationship_decay";

        private static readonly SaveStore<RelationshipDecayState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RelationshipDecaySaveStore),
            SchemaVersionedEnvelope<RelationshipDecayState>.Encode,
            SchemaVersionedEnvelope<RelationshipDecayState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RelationshipDecayState state) => s_store.TrySave(state);
        public static RelationshipDecayState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RelationshipDecayState state) => s_store.CapturePersisted(state);
    }
}
