// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : NarcoticsSaveStore
// Core State : Ashfall.Core.Medical.NarcoticsState
// Host Caller: Main.Plans182_185
// Purpose    : Chemical medicines, toxicity, tolerance, addiction, and rehab beds
// ============================================================================
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class NarcoticsSaveStore
    {
        public const string FileName = "narcotics_save.json";
        public const string SectionName = "narcotics";

        private static readonly SaveStore<NarcoticsState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(NarcoticsSaveStore),
            SchemaVersionedEnvelope<NarcoticsState>.Encode,
            SchemaVersionedEnvelope<NarcoticsState>.Decode);

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static bool TrySave(NarcoticsState state) => s_store.TrySave(state);
        public static NarcoticsState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(NarcoticsState state) => s_store.CapturePersisted(state);
    }
}
