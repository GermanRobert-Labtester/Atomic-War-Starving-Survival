using System;
using System.Collections.Generic;

namespace Ashfall.Core.YearOfAsh
{
    /// <summary>
    /// Content catalog for the faction war narrative layer (Days 240-360).
    /// Loads and indexes the five faction_war_* JSON files: event chains,
    /// journal entries, radio broadcasts, dialogue snippets, and communiques.
    ///
    /// This is the content side of <see cref="FactionWarSystem"/> (which handles
    /// simulation: standing, territory, tension). The catalog provides the
    /// narrative surface — what the player reads, hears, and experiences as the
    /// war escalates.
    /// </summary>
    public sealed class FactionWarContentCatalog
    {
        private readonly List<FactionWarEventChain> _eventChains = new List<FactionWarEventChain>();
        private readonly List<FactionWarJournalEntry> _journalEntries = new List<FactionWarJournalEntry>();
        private readonly List<FactionWarBroadcast> _broadcasts = new List<FactionWarBroadcast>();
        private readonly List<FactionWarDialogueSnippet> _dialogueSnippets = new List<FactionWarDialogueSnippet>();
        private readonly List<FactionWarCommunique> _communiques = new List<FactionWarCommunique>();
        private readonly List<FactionWarLocationOverride> _locationOverrides = new List<FactionWarLocationOverride>();

        public IReadOnlyList<FactionWarEventChain> EventChains => _eventChains;
        public IReadOnlyList<FactionWarJournalEntry> JournalEntries => _journalEntries;
        public IReadOnlyList<FactionWarBroadcast> Broadcasts => _broadcasts;
        public IReadOnlyList<FactionWarDialogueSnippet> DialogueSnippets => _dialogueSnippets;
        public IReadOnlyList<FactionWarCommunique> Communiques => _communiques;
        public IReadOnlyList<FactionWarLocationOverride> LocationOverrides => _locationOverrides;

        public int EventChainCount => _eventChains.Count;
        public int JournalEntryCount => _journalEntries.Count;
        public int BroadcastCount => _broadcasts.Count;
        public int DialogueSnippetCount => _dialogueSnippets.Count;
        public int CommuniqueCount => _communiques.Count;
        public int LocationOverrideCount => _locationOverrides.Count;

        internal void AddEventChain(FactionWarEventChain chain) => _eventChains.Add(chain);
        internal void AddJournalEntry(FactionWarJournalEntry entry) => _journalEntries.Add(entry);
        internal void AddBroadcast(FactionWarBroadcast broadcast) => _broadcasts.Add(broadcast);
        internal void AddDialogueSnippet(FactionWarDialogueSnippet snippet) => _dialogueSnippets.Add(snippet);
        internal void AddCommunique(FactionWarCommunique communique) => _communiques.Add(communique);
        internal void AddLocationOverride(FactionWarLocationOverride entry) => _locationOverrides.Add(entry);

        /// <summary>Returns event chains eligible on or before the given day.</summary>
        public List<FactionWarEventChain> GetEligibleChains(int day)
        {
            var eligible = new List<FactionWarEventChain>();
            for (int i = 0; i < _eventChains.Count; i++)
            {
                var chain = _eventChains[i];
                if (chain?.stages == null) continue;
                for (int j = 0; j < chain.stages.Count; j++)
                {
                    if (chain.stages[j] != null && chain.stages[j].minDay <= day)
                    {
                        eligible.Add(chain);
                        break;
                    }
                }
            }
            return eligible;
        }

        /// <summary>Returns journal entries for a specific day.</summary>
        public List<FactionWarJournalEntry> GetJournalForDay(int day)
        {
            var result = new List<FactionWarJournalEntry>();
            for (int i = 0; i < _journalEntries.Count; i++)
            {
                if (_journalEntries[i] != null && _journalEntries[i].day == day)
                    result.Add(_journalEntries[i]);
            }
            return result;
        }

        /// <summary>Returns broadcasts triggered on or before the given day.</summary>
        public List<FactionWarBroadcast> GetBroadcastsForDay(int day)
        {
            var result = new List<FactionWarBroadcast>();
            for (int i = 0; i < _broadcasts.Count; i++)
            {
                if (_broadcasts[i] != null && _broadcasts[i].dayTrigger <= day)
                    result.Add(_broadcasts[i]);
            }
            return result;
        }

        /// <summary>Returns dialogue snippets available at a location on or after minDay.</summary>
        public List<FactionWarDialogueSnippet> GetDialogueForLocation(string locationId, int day)
        {
            var result = new List<FactionWarDialogueSnippet>();
            for (int i = 0; i < _dialogueSnippets.Count; i++)
            {
                var s = _dialogueSnippets[i];
                if (s != null && s.minDay <= day &&
                    string.Equals(s.locationId, locationId, StringComparison.Ordinal))
                    result.Add(s);
            }
            return result;
        }

        /// <summary>Returns communiques issued by a faction on or before the given day.</summary>
        public List<FactionWarCommunique> GetCommuniquesForFaction(string factionId, int day)
        {
            var result = new List<FactionWarCommunique>();
            for (int i = 0; i < _communiques.Count; i++)
            {
                var c = _communiques[i];
                if (c != null && c.day <= day &&
                    string.Equals(c.factionId, factionId, StringComparison.Ordinal))
                    result.Add(c);
            }
            return result;
        }

        /// <summary>
        /// Returns the single active location override for a locationId on the
        /// given day, or null if none applies. If multiple overrides for the
        /// same location are simultaneously active (authoring error — should
        /// not occur in shipped data), the most recently-started one
        /// (highest activeFromDay) wins, matching "the latest thing that
        /// happened to this place is what's currently true."
        /// </summary>
        public FactionWarLocationOverride? GetActiveLocationOverride(string locationId, int day)
        {
            FactionWarLocationOverride? best = null;
            for (int i = 0; i < _locationOverrides.Count; i++)
            {
                var o = _locationOverrides[i];
                if (o == null) continue;
                if (!string.Equals(o.locationId, locationId, StringComparison.Ordinal)) continue;
                if (day < o.activeFromDay) continue;
                if (o.activeUntilDay > 0 && day > o.activeUntilDay) continue;
                if (best == null || o.activeFromDay > best.activeFromDay) best = o;
            }
            return best;
        }
    }

    // ── DTOs matching the JSON schema ────────────────────────────────────

    [Serializable]
    public sealed class FactionWarEventChain
    {
        public string chainId = string.Empty;
        public string band = string.Empty;
        public string title = string.Empty;
        public List<string> factionsInvolved = new List<string>();
        public string locationId = string.Empty;
        public List<FactionWarEventStage> stages = new List<FactionWarEventStage>();
    }

    [Serializable]
    public sealed class FactionWarEventStage
    {
        public string stageId = string.Empty;
        public int minDay;
        public string triggerCondition = string.Empty;
        public string title = string.Empty;
        public string bodyText = string.Empty;
        public List<FactionWarEventChoice> choices = new List<FactionWarEventChoice>();
    }

    [Serializable]
    public sealed class FactionWarEventChoice
    {
        public string choiceId = string.Empty;
        public string text = string.Empty;
        public int moraleDelta;
        public string leadsToStageId = string.Empty;
    }

    [Serializable]
    public sealed class FactionWarJournalEntry
    {
        public string id = string.Empty;
        public string authorName = string.Empty;
        public int day;
        public string locationId = string.Empty;
        public string voice = string.Empty;
        public string body = string.Empty;
    }

    [Serializable]
    public sealed class FactionWarBroadcast
    {
        public string id = string.Empty;
        public string frequency = string.Empty;
        public int dayTrigger;
        public string source = string.Empty;
        public string message = string.Empty;
        public string signalStrength = string.Empty;
        public bool isEmergency;
        public string audio_cue = string.Empty;
    }

    [Serializable]
    public sealed class FactionWarDialogueSnippet
    {
        public string id = string.Empty;
        public string locationId = string.Empty;
        public int minDay;
        public string speakerTag = string.Empty;
        public string body = string.Empty;
    }

    [Serializable]
    public sealed class FactionWarCommunique
    {
        public string id = string.Empty;
        public string eventChainId = string.Empty;
        public string factionId = string.Empty;
        public int day;
        public string title = string.Empty;
        public string body = string.Empty;

        /// <summary>Optional in-world-reliability annotation (true/false/partial), present on
        /// ~half of authored communiques. Empty when the source JSON omits it.</summary>
        public string authorNote = string.Empty;
    }

    /// <summary>
    /// A location description override active for a bounded or open-ended
    /// day window, layered over the base locations.json entry for display
    /// purposes only (see NARRATIVE_NEEDS.md §3 — base mechanical fields
    /// like dangerLevel/travelHours/baseRadsPerHour are never overridden
    /// here). Three overrideType values: pre_strike (bounded window,
    /// foreshadowing), post_strike (open-ended, permanent aftermath),
    /// ambient_addendum (open-ended, minor flavor — authored today as a full
    /// replacement string, per the doc's caveat, not an appended fragment).
    /// </summary>
    [Serializable]
    public sealed class FactionWarLocationOverride
    {
        public string id = string.Empty;
        public string locationId = string.Empty;
        public string overrideType = string.Empty;
        public int activeFromDay;

        /// <summary>0 (unset) means open-ended — present only on pre_strike
        /// entries in the shipped data, per NARRATIVE_NEEDS.md's documented
        /// convention.</summary>
        public int activeUntilDay;
        public string displayName = string.Empty;
        public string description = string.Empty;
    }

    // ── Root DTOs for deserialization ─────────────────────────────────────

    [Serializable]
    public sealed class FactionWarEventChainRoot
    {
        public int schema_version;
        public List<FactionWarEventChain> chains = new List<FactionWarEventChain>();
    }

    [Serializable]
    public sealed class FactionWarJournalRoot
    {
        public int schema_version;
        public List<FactionWarJournalEntry> entries = new List<FactionWarJournalEntry>();
    }

    [Serializable]
    public sealed class FactionWarBroadcastRoot
    {
        public int schema_version;
        public List<FactionWarBroadcast> broadcasts = new List<FactionWarBroadcast>();
    }

    [Serializable]
    public sealed class FactionWarDialogueRoot
    {
        public int schema_version;
        public List<FactionWarDialogueSnippet> snippets = new List<FactionWarDialogueSnippet>();
    }

    [Serializable]
    public sealed class FactionWarCommuniqueRoot
    {
        public int schema_version;
        public List<FactionWarCommunique> communiques = new List<FactionWarCommunique>();
    }

    [Serializable]
    public sealed class FactionWarLocationOverrideRoot
    {
        public int schema_version;
        public List<FactionWarLocationOverride> locationOverrides = new List<FactionWarLocationOverride>();
    }

    // ── Loader ───────────────────────────────────────────────────────────

    /// <summary>
    /// Loads all five faction_war_* JSON files into a <see cref="FactionWarContentCatalog"/>.
    /// Tolerant of missing files (logs warning, continues); parse failures in one file
    /// do not prevent loading the others.
    /// </summary>
    public sealed class FactionWarContentCatalogLoader
    {
        public const string EventsFile = "faction_war_events.json";
        public const string JournalFile = "faction_war_journal.json";
        public const string RadioFile = "faction_war_radio.json";
        public const string DialogueFile = "faction_war_dialogue.json";
        public const string CommuniquesFile = "faction_war_communiques.json";
        public const string LocationOverridesFile = "faction_war_location_overrides.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public FactionWarContentCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public FactionWarContentCatalog Load(string dataDirectory)
        {
            var catalog = new FactionWarContentCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Faction war content directory missing: " + dataDirectory);
                return catalog;
            }

            LoadEventChains(_files.Combine(dataDirectory, EventsFile), catalog);
            LoadJournalEntries(_files.Combine(dataDirectory, JournalFile), catalog);
            LoadBroadcasts(_files.Combine(dataDirectory, RadioFile), catalog);
            LoadDialogueSnippets(_files.Combine(dataDirectory, DialogueFile), catalog);
            LoadCommuniques(_files.Combine(dataDirectory, CommuniquesFile), catalog);
            LoadLocationOverrides(_files.Combine(dataDirectory, LocationOverridesFile), catalog);

            _log.Info($"Faction war content loaded: {catalog.EventChainCount} chains, " +
                      $"{catalog.JournalEntryCount} journal, {catalog.BroadcastCount} broadcasts, " +
                      $"{catalog.DialogueSnippetCount} dialogue, {catalog.CommuniqueCount} communiques, " +
                      $"{catalog.LocationOverrideCount} location overrides");

            return catalog;
        }

        private void LoadEventChains(string path, FactionWarContentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var root = _json.Deserialize<FactionWarEventChainRoot>(_files.ReadAllText(path));
                if (root?.chains == null) return;
                for (int i = 0; i < root.chains.Count; i++)
                    if (root.chains[i] != null && !string.IsNullOrEmpty(root.chains[i].chainId))
                        catalog.AddEventChain(root.chains[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }

        private void LoadJournalEntries(string path, FactionWarContentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var root = _json.Deserialize<FactionWarJournalRoot>(_files.ReadAllText(path));
                if (root?.entries == null) return;
                for (int i = 0; i < root.entries.Count; i++)
                    if (root.entries[i] != null && !string.IsNullOrEmpty(root.entries[i].id))
                        catalog.AddJournalEntry(root.entries[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }

        private void LoadBroadcasts(string path, FactionWarContentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var root = _json.Deserialize<FactionWarBroadcastRoot>(_files.ReadAllText(path));
                if (root?.broadcasts == null) return;
                for (int i = 0; i < root.broadcasts.Count; i++)
                    if (root.broadcasts[i] != null && !string.IsNullOrEmpty(root.broadcasts[i].id))
                        catalog.AddBroadcast(root.broadcasts[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }

        private void LoadDialogueSnippets(string path, FactionWarContentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var root = _json.Deserialize<FactionWarDialogueRoot>(_files.ReadAllText(path));
                if (root?.snippets == null) return;
                for (int i = 0; i < root.snippets.Count; i++)
                    if (root.snippets[i] != null && !string.IsNullOrEmpty(root.snippets[i].id))
                        catalog.AddDialogueSnippet(root.snippets[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }

        private void LoadCommuniques(string path, FactionWarContentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var root = _json.Deserialize<FactionWarCommuniqueRoot>(_files.ReadAllText(path));
                if (root?.communiques == null) return;
                for (int i = 0; i < root.communiques.Count; i++)
                    if (root.communiques[i] != null && !string.IsNullOrEmpty(root.communiques[i].id))
                        catalog.AddCommunique(root.communiques[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }

        private void LoadLocationOverrides(string path, FactionWarContentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var root = _json.Deserialize<FactionWarLocationOverrideRoot>(_files.ReadAllText(path));
                if (root?.locationOverrides == null) return;
                for (int i = 0; i < root.locationOverrides.Count; i++)
                    if (root.locationOverrides[i] != null && !string.IsNullOrEmpty(root.locationOverrides[i].id))
                        catalog.AddLocationOverride(root.locationOverrides[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }
    }
}
