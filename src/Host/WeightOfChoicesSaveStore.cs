// ============================================================================
// Save Store : WeightOfChoicesSaveStore
// Core State : Ashfall.Core.Factions.WeightOfChoicesSave
// Host Caller: Main.FactionBranch / FactionBranchHostSession
// Purpose    : Unified faction branching progression, alignment & PRPF standing
// ============================================================================
using Ashfall.Core.Factions;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Weight of Choices faction branch save persistence — thin façade over the Core
    /// SaveStore&lt;T&gt; service (via SaveStoreHub, codec flavor). Composes all four
    /// constituent faction branch subsystems (Military, Rebel, Independent, PRPF)
    /// into a single atomic versioned envelope.
    /// </summary>
    public static class WeightOfChoicesSaveStore
    {
        public const string FileName = "weight_of_choices_save.json";
        public const string SectionName = "weight_of_choices";

        private static readonly SaveStore<WeightOfChoicesSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(WeightOfChoicesSaveStore),
            WeightOfChoicesSaveCodec.Encode,
            WeightOfChoicesSaveCodec.Decode);

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
        public static string TryCaptureDirect(WeightOfChoicesSave state) => s_store.CaptureBare(state);

        /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
        public static WeightOfChoicesSave? TryRestoreDirect(string json) => s_store.RestoreBare(json);

        /// <summary>Capture state to JSON without writing to disk.</summary>
        public static string TryCapture(WeightOfChoicesSave state) => s_store.CaptureBare(state);

        /// <summary>Restore state from JSON without reading from disk.</summary>
        public static WeightOfChoicesSave? TryRestore(string json) => s_store.RestoreBare(json);

        public static bool TrySave(WeightOfChoicesSave state) => s_store.TrySave(state);

        public static WeightOfChoicesSave? TryLoad() => s_store.TryLoad();

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(WeightOfChoicesSave state) => s_store.CapturePersisted(state);
    }
}
