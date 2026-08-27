// ============================================================================
// Save Store : YearOfAshSaveStore
// Core State : Ashfall.Core.YearOfAsh.YearOfAshSave
// Host Caller: Main.YearOfAsh / YearOfAshHostSession
// Purpose    : Year of Ash campaign progression, season timeline, and winter survival records
// ============================================================================
using Ashfall.Core.Save;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// File persistence adapter for YearOfAshSave in the Godot host
    /// environment — thin façade over the Core SaveStore&lt;T&gt; service
    /// (via SaveStoreHub, codec flavor). Shape, versioned migration, and
    /// validation live in <see cref="YearOfAshSaveCodec"/>; path resolution,
    /// atomic write, and error handling live in the service. Stores the save
    /// file in user://year_of_ash_save.json.
    /// </summary>
    public static class YearOfAshSaveStore
    {
        public const string FileName = "year_of_ash_save.json";
        public const string SectionName = "year_of_ash";

        private static readonly SaveStore<YearOfAshSave> s_store = SaveStoreHub.FromCodec(
            FileName,
            nameof(YearOfAshSaveStore),
            (save, json) => YearOfAshSaveCodec.Encode(save, json),
            (raw, json) => YearOfAshSaveCodec.Decode(raw, json));

        public static string SavePath => s_store.SavePath;

        public static bool Exists => s_store.Exists();

        public static bool TrySave(YearOfAshSave save, string pathOverride = null!) =>
            s_store.TrySave(save, pathOverride);

        public static YearOfAshSave? TryLoad(string pathOverride = null!) =>
            s_store.TryLoad(pathOverride);

        /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
        public static string TryCapturePersisted(YearOfAshSave save) => s_store.CapturePersisted(save);
    }
}
