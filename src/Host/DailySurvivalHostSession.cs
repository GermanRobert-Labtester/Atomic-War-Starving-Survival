using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp.Narrative
{
    /// <summary>
    /// Thin Godot-host session for DailySurvivalCatalog.
    /// Loads the four narrative JSON files (psychological journals, botanical logs,
    /// children folklore, ration fraud records) via IFileIO and exposes query methods.
    /// No rules here — hosts only wire.
    /// </summary>
    public sealed class DailySurvivalHostSession : HostSessionBase
    {
        public DailySurvivalCatalog Catalog { get; private set; }

        public DailySurvivalHostSession()
        {
            Catalog = new DailySurvivalCatalog();
        }

        /// <summary>
        /// Load all four daily survival catalogs from the narrative data directory.
        /// </summary>
        public void LoadCatalogs(string narrativeDataDir)
        {
            if (string.IsNullOrEmpty(narrativeDataDir)) return;
            var fileIO = new FileSystemIO();
            Catalog = DailySurvivalCatalog.LoadFromDirectory(narrativeDataDir, fileIO);
            RaiseStateChanged();
        }

        // ── Query methods ─────────────────────────────────────────────

        public IReadOnlyList<PsychologicalJournalEntry> GetJournalEntries()
            => Catalog.JournalEntries;

        public PsychologicalJournalEntry? GetJournalEntry(string id)
            => Catalog.GetJournal(id);

        public IReadOnlyList<MutatedBotanicalEntry> GetBotanicalEntries()
            => Catalog.BotanicalEntries;

        public MutatedBotanicalEntry? GetBotanicalEntry(string id)
            => Catalog.GetBotanical(id);

        public IReadOnlyList<ChildrenFolkloreEntry> GetFolkloreEntries()
            => Catalog.FolkloreEntries;

        public ChildrenFolkloreEntry? GetFolkloreEntry(string id)
            => Catalog.GetFolklore(id);

        public IReadOnlyList<RationFraudEntry> GetFraudEntries()
            => Catalog.FraudEntries;

        public RationFraudEntry? GetFraudEntry(string id)
            => Catalog.GetFraud(id);

        public string StatusLine()
        {
            return $"Daily survival catalog: {Catalog.TotalCount} entries " +
                   $"({Catalog.JournalEntries.Count} journals, " +
                   $"{Catalog.BotanicalEntries.Count} botanical, " +
                   $"{Catalog.FolkloreEntries.Count} folklore, " +
                   $"{Catalog.FraudEntries.Count} fraud).";
        }
    }
}
