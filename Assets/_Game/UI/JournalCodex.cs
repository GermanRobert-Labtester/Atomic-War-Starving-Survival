using System;
using System.Collections.Generic;
using System.Text;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Journal codex tabs (docs/ui/JOURNAL_UI_PLAN.md §5.2-5.5).
    /// Plain C# view-model builder: turns the JSON-authored catalogs + the
    /// journal's knowledge base into rows the JournalBookUI renders verbatim.
    /// Locked rows show a "[---]" silhouette until the matching unlock key.
    /// </summary>
    public enum JournalTab
    {
        Log = 0,
        Items = 1,
        People = 2,
        Places = 3,
        Events = 4
    }

    /// <summary>One renderable codex entry: name, meta line, verbatim body.</summary>
    public struct JournalCodexRow
    {
        public string DisplayName;
        public string Meta;
        public string Body;
        public bool IsLocked;

        public static JournalCodexRow Locked(string displayName)
        {
            return new JournalCodexRow
            {
                DisplayName = displayName,
                Meta = string.Empty,
                Body = "Not seen yet. The bunker has not logged this.",
                IsLocked = true
            };
        }
    }

    /// <summary>
    /// Builds codex rows from the injected catalogs, filtered by the journal's
    /// knowledge base. The catalogs are the JSON text sources; this class never
    /// paraphrases them (house rule: text is the asset).
    /// </summary>
    public class JournalCodex
    {
        private readonly JournalSystem _journal;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly LocationCatalogSO _locationCatalog;
        private readonly GameEventCatalogSO _eventCatalog;
        private readonly SurvivorCatalogSO _survivorCatalog;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;

        public JournalCodex(
            JournalSystem journal,
            ItemCatalogSO itemCatalog,
            LocationCatalogSO locationCatalog,
            GameEventCatalogSO eventCatalog,
            Func<IReadOnlyList<Survivor>> getSurvivors,
            SurvivorCatalogSO survivorCatalog = null)
        {
            _journal = journal;
            _itemCatalog = itemCatalog;
            _locationCatalog = locationCatalog;
            _eventCatalog = eventCatalog;
            _getSurvivors = getSurvivors;
            _survivorCatalog = survivorCatalog;
        }

        /// <summary>Rows for one tab; Log returns an empty list (handled by the book).</summary>
        public IReadOnlyList<JournalCodexRow> BuildRows(JournalTab tab)
        {
            switch (tab)
            {
                case JournalTab.Items: return BuildItemRows();
                case JournalTab.People: return BuildPeopleRows();
                case JournalTab.Places: return BuildPlaceRows();
                case JournalTab.Events: return BuildEventRows();
                default: return s_emptyRows;
            }
        }

        private static readonly List<JournalCodexRow> s_emptyRows = new List<JournalCodexRow>();

        // -----------------------------------------------------------------
        // Items — grouped by type, filtered by item_seen_*
        // -----------------------------------------------------------------

        private List<JournalCodexRow> BuildItemRows()
        {
            var rows = new List<JournalCodexRow>();
            if (_itemCatalog == null || _itemCatalog.items == null) return rows;
            for (int i = 0; i < _itemCatalog.items.Count; i++)
            {
                var item = _itemCatalog.items[i];
                if (item == null || string.IsNullOrEmpty(item.id)) continue;
                rows.Add(_journal.IsItemSeen(item.id)
                    ? new JournalCodexRow
                    {
                        DisplayName = item.displayName,
                        Meta = BuildItemMeta(item),
                        Body = item.description ?? string.Empty,
                        IsLocked = false
                    }
                    : JournalCodexRow.Locked(item.displayName));
            }
            return rows;
        }

        private string BuildItemMeta(ItemDefinition item)
        {
            var sb = new StringBuilder();
            if (item.weight > 0f) sb.Append($"{item.weight:0.#} kg");
            if (item.tradeValue > 0f)
            {
                if (sb.Length > 0) sb.Append(" · ");
                sb.Append($"trades ~{item.tradeValue:0}");
            }
            if (item.durability > 0f)
            {
                if (sb.Length > 0) sb.Append(" · ");
                sb.Append($"durability {item.durability:0}");
            }
            return sb.ToString();
        }

        // -----------------------------------------------------------------
        // People — archetype dossiers + living survivors, filtered by survivor_met_*
        // -----------------------------------------------------------------

        private List<JournalCodexRow> BuildPeopleRows()
        {
            var rows = new List<JournalCodexRow>();
            var shown = new HashSet<string>(StringComparer.Ordinal);

            // Archetype dossiers first (the bunker's pre-listed shelf). Locked
            // until a survivor with that archetype has actually joined.
            if (_survivorCatalog != null && _survivorCatalog.archetypes != null)
            {
                for (int i = 0; i < _survivorCatalog.archetypes.Count; i++)
                {
                    var archetype = _survivorCatalog.archetypes[i];
                    if (archetype == null || string.IsNullOrEmpty(archetype.id)) continue;
                    shown.Add(archetype.id);
                    bool met = _journal.IsSurvivorMet(archetype.id);
                    rows.Add(met
                        ? new JournalCodexRow
                        {
                            DisplayName = archetype.displayName,
                            Meta = archetype.profession ?? string.Empty,
                            Body = archetype.bio ?? string.Empty,
                            IsLocked = false
                        }
                        : JournalCodexRow.Locked(archetype.displayName));
                }
            }

            // Living survivors not represented by an archetype dossier (child,
            // refugee) get a plain file entry keyed by their own id.
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || string.IsNullOrEmpty(sv.Id)) continue;
                    string key = string.IsNullOrEmpty(sv.ArchetypeId) ? sv.Id : sv.ArchetypeId;
                    if (!shown.Add(key)) continue; // already covered by a dossier
                    rows.Add(_journal.IsSurvivorMet(key)
                        ? new JournalCodexRow
                        {
                            DisplayName = sv.DisplayName ?? sv.Id,
                            Meta = BuildSurvivorMeta(sv),
                            Body = sv.Id + (sv.IsAlive ? string.Empty : " (deceased)"),
                            IsLocked = false
                        }
                        : JournalCodexRow.Locked(sv.DisplayName ?? sv.Id));
                }
            }
            return rows;
        }

        private string BuildSurvivorMeta(Survivor sv)
        {
            if (!sv.IsAlive) return "deceased";
            if (sv.IsOnExpedition) return "out scavenging";
            if (sv.HasAcuteRadiationSickness || sv.HasAcuteRadiationSyndrome) return "rad-sick";
            if (sv.HasChronicIllness) return "chronic illness";
            switch (sv.State)
            {
                case SurvivorState.Working: return "working";
                case SurvivorState.Resting: return "resting";
                case SurvivorState.Sick: return "sick";
                case SurvivorState.Incapacitated: return "incapacitated";
                default: return "idle";
            }
        }

        // -----------------------------------------------------------------
        // Places — locations, filtered by location_visited_*
        // -----------------------------------------------------------------

        private List<JournalCodexRow> BuildPlaceRows()
        {
            var rows = new List<JournalCodexRow>();
            if (_locationCatalog == null || _locationCatalog.locations == null) return rows;
            for (int i = 0; i < _locationCatalog.locations.Count; i++)
            {
                var loc = _locationCatalog.locations[i];
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                rows.Add(_journal.IsLocationVisited(loc.id)
                    ? new JournalCodexRow
                    {
                        DisplayName = loc.displayName,
                        Meta = BuildLocationMeta(loc),
                        Body = loc.description ?? string.Empty,
                        IsLocked = false
                    }
                    : JournalCodexRow.Locked(loc.displayName));
            }
            return rows;
        }

        private string BuildLocationMeta(LocationDefinitionSO loc)
        {
            var sb = new StringBuilder();
            sb.Append($"peril {loc.dangerLevel:0.#}");
            if (loc.baseRadsPerHour > 0f) sb.Append($" · fallout {loc.baseRadsPerHour:0.#} rad/h");
            return sb.ToString();
        }

        // -----------------------------------------------------------------
        // Events — fired events, filtered by event_fired_*
        // -----------------------------------------------------------------

        private List<JournalCodexRow> BuildEventRows()
        {
            var rows = new List<JournalCodexRow>();
            if (_eventCatalog == null || _eventCatalog.events == null) return rows;
            for (int i = 0; i < _eventCatalog.events.Count; i++)
            {
                var evt = _eventCatalog.events[i];
                if (evt == null || string.IsNullOrEmpty(evt.id)) continue;
                rows.Add(_journal.IsEventFired(evt.id)
                    ? new JournalCodexRow
                    {
                        DisplayName = evt.title ?? evt.id,
                        Meta = string.Empty,
                        Body = evt.bodyText ?? string.Empty,
                        IsLocked = false
                    }
                    : JournalCodexRow.Locked(evt.title ?? evt.id));
            }
            return rows;
        }
    }
}
