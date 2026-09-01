using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp.Narrative
{
    /// <summary>
    /// Thin Godot-host session for OralLoreCatalog.
    /// Loads the two oral lore JSON files (oral_lore_codex.json, oral_lore_batch_2.json)
    /// via IFileIO and exposes query methods. No rules here — hosts only wire.
    /// </summary>
    public sealed class OralLoreHostSession : HostSessionBase
    {
        public OralLoreCatalog Catalog { get; private set; }

        public OralLoreHostSession()
        {
            Catalog = new OralLoreCatalog();
        }

        /// <summary>
        /// Load both oral lore catalogs from the narrative data directory.
        /// </summary>
        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            Catalog = new OralLoreCatalog();

            string codexPath = Path.Combine(dataDir, "narrative", "oral_lore_codex.json");
            if (fileIO.FileExists(codexPath))
            {
                string json = fileIO.ReadAllText(codexPath);
                Catalog.Load(json, serializer);
            }
            else
            {
                GD.PrintErr("[OralLore] missing: " + codexPath);
            }

            string batch2Path = Path.Combine(dataDir, "narrative", "oral_lore_batch_2.json");
            if (fileIO.FileExists(batch2Path))
            {
                string json = fileIO.ReadAllText(batch2Path);
                Catalog.Load(json, serializer);
            }
            else
            {
                GD.PrintErr("[OralLore] missing: " + batch2Path);
            }

            RaiseStateChanged();
        }

        // ── Query methods ─────────────────────────────────────────────

        /// <summary>All loaded songs/poems across both catalog files.</summary>
        public IReadOnlyList<OralLoreEntry> AllSongs => Catalog.AllSongs;

        /// <summary>Look up a single entry by its lore_id.</summary>
        public OralLoreEntry? GetSong(string loreId) => Catalog.GetById(loreId);

        /// <summary>Return every song that carries the given tag.</summary>
        public List<OralLoreEntry> GetSongsByTag(string tag) => Catalog.GetByTag(tag);

        /// <summary>Return every song whose genre contains the given string.</summary>
        public List<OralLoreEntry> GetSongsByGenre(string genre) => Catalog.GetByGenre(genre);
    }
}
