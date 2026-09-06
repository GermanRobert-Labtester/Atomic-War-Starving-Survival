// ============================================================================
// Save Store : ProceduralNarrativeSaveStore
// Core State : Ashfall.Core.Narrative.ProceduralNarrativeSaveState
// ============================================================================
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// One persisted section for generated narrative metadata and the shared
    /// quest runtime. The structured bindings survive localization changes.
    /// </summary>
    public static class ProceduralNarrativeSaveStore
    {
        public const string FileName = "procedural_narrative_save.json";
        public const string SectionName = "procedural_narrative";

        private static readonly SaveStore<ProceduralNarrativeSaveState> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(ProceduralNarrativeSaveStore),
            SchemaVersionedEnvelope<ProceduralNarrativeSaveState>.Encode,
            SchemaVersionedEnvelope<ProceduralNarrativeSaveState>.Decode);

        public static string TryCapturePersisted(ProceduralNarrativeSaveState state) => s_store.CapturePersisted(state);
        public static ProceduralNarrativeSaveState? TryLoad() => s_store.TryLoad();
    }
}
