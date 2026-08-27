// ============================================================================
// Save Store : JournalSaveStore
// Core State : Ashfall.Core.JournalSave
// Host Caller: Main.Holdfast, Main.Narrative / JournalHostSession
// Purpose    : Player journal entries, discovered lore fragments, and quest logs
using Ashfall.Core.Journal;
using Ashfall.Core.Save;
using AtomicWar.GodotApp;

namespace AtomicWar.Journal
{
    /// <summary>
    /// Persists JournalSave as JSON under user://journal_save.json — thin
    /// façade over the Core SaveStore&lt;T&gt; service (via SaveStoreHub).
    /// Checksummed envelope, atomic write, and legacy bare-state loading live
    /// in the service; this class keeps the void Save/Load call surface with
    /// optional path overrides used by the host.
    /// </summary>
    public static class JournalSaveStore
    {
        public const string FileName = "journal_save.json";
        public const string SectionName = "journal";

        private static readonly SaveStore<JournalSave> s_store =
            SaveStoreHub.Checksummed<JournalSave>(FileName, nameof(JournalSaveStore));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static void Save(JournalSave save, string? pathOverride = null)
        {
            s_store.TrySave(save, pathOverride);
        }

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(JournalSave save) => s_store.CapturePersisted(save);

        public static JournalSave? Load(string? pathOverride = null)
        {
            return s_store.TryLoad(pathOverride);
        }
    }
}
