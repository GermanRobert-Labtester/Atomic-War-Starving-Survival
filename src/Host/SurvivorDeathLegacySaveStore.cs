// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SurvivorDeathLegacySaveStore
// Core State : Ashfall.Core.Survivors.SurvivorDeathLegacyState
// Host Caller: Main.SurvivorDeathLegacy / SurvivorDeathLegacyHostSession
// Purpose    : Plan 206 — Survivor death records, last wills, estate inheritance, and disputes
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class SurvivorDeathLegacySaveStore
    {
        public const string FileName = "death_legacy_save.json";
        public const string SectionName = "death_legacy";

        private static readonly SaveStore<SurvivorDeathLegacyState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(SurvivorDeathLegacySaveStore),
            SchemaVersionedEnvelope<SurvivorDeathLegacyState>.Encode,
            SchemaVersionedEnvelope<SurvivorDeathLegacyState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(SurvivorDeathLegacyState state) => s_store.TrySave(state);
        public static SurvivorDeathLegacyState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SurvivorDeathLegacyState state) => s_store.CapturePersisted(state);
    }
}
