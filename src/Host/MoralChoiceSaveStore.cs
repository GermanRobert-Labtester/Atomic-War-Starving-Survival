// ============================================================================
// Save Store : MoralChoiceSaveStore
// Core State : Ashfall.Core.MoralChoice.MoralChoiceState
// Host Caller: Main.MoralChoice / MoralChoiceHostSession
// Purpose    : Moral choice branches, ethical dilemmas, community trust, and faction reactions
// ============================================================================
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Persists MoralChoiceState as JSON under user://moral_choice_save.json —
    /// thin façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub).
    /// Checksummed envelope, atomic write, and legacy bare-state loading live
    /// in the service; this class keeps the void Save call surface with an
    /// optional path override used by the host.
    /// </summary>
    public static class MoralChoiceSaveStore
    {
        public const string FileName = "moral_choice_save.json";
        public const string SectionName = "host_event";

        private static readonly SaveStore<MoralChoiceState> s_store =
            SaveStoreHub.Checksummed<MoralChoiceState>(FileName, nameof(MoralChoiceSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static void Save(MoralChoiceState state, string? pathOverride = null)
        {
            s_store.TrySave(state, pathOverride);
        }

        public static MoralChoiceState? TryLoad(string? pathOverride = null)
        {
            return s_store.TryLoad(pathOverride);
        }
    }
}
