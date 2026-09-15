// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AmphibiousDraisineSaveStore
// Core State : System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>
// Host Caller: Main.Plans122to125
// Purpose    : Plan 125 — per-vehicle kit condition, pontoons, ingress, crossing state
// ============================================================================
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class AmphibiousDraisineSaveStore
    {
        public const string FileName = "amphibious_draisine_save.json";
        public const string SectionName = "amphibious_draisine";

        private static readonly SaveStore<System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(AmphibiousDraisineSaveStore),
            SchemaVersionedEnvelope<System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>>.Encode,
            SchemaVersionedEnvelope<System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(System.Collections.Generic.Dictionary<string, AmphibiousDraisineState> state) => s_store.TrySave(state);
        public static System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(System.Collections.Generic.Dictionary<string, AmphibiousDraisineState> state) => s_store.CapturePersisted(state);
    }
}
