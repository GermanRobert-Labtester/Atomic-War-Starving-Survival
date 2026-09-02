// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : RoboticsSaveStore
// Core State : Ashfall.Core.Crafting.RoboticsSaveState
// Host Caller: Main.Plans198_201
// Purpose    : Pre-war automaton units, labor directives, EMP timers & rogue states
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class RoboticsSaveStore
    {
        public const string FileName = "robotics_save.json";
        public const string SectionName = "robotics";

        private static readonly SaveStore<RoboticsSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(RoboticsSaveStore),
            SchemaVersionedEnvelope<RoboticsSaveState>.Encode,
            SchemaVersionedEnvelope<RoboticsSaveState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(RoboticsSaveState state) => s_store.TrySave(state);
        public static RoboticsSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(RoboticsSaveState state) => s_store.CapturePersisted(state);
    }
}
