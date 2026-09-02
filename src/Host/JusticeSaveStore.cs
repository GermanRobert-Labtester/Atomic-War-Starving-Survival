// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : JusticeSaveStore
// Core State : Ashfall.Core.Narrative.JusticeState
// Host Caller: Main.Plans190_193
// Purpose    : Crime incidents, trials, punishments, banishments, and grudges
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class JusticeSaveStore
    {
        public const string FileName = "wasteland_justice_save.json";
        public const string SectionName = "wasteland_justice";

        private static readonly SaveStore<JusticeState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(JusticeSaveStore),
            SchemaVersionedEnvelope<JusticeState>.Encode,
            SchemaVersionedEnvelope<JusticeState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(JusticeState state) => s_store.TrySave(state);
        public static JusticeState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(JusticeState state) => s_store.CapturePersisted(state);
    }
}
