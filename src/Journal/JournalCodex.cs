using System;
using System.Collections.Generic;
using System.Text;
using Ashfall.Core.Journal;

namespace AtomicWar.Journal
{
    /// <summary>Journal codex tabs (docs/ui/JOURNAL_UI_PLAN.md §5.2-5.5).</summary>
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
        public string? DisplayName;
        public string? Meta;
        public string? Body;
        public bool IsLocked;

        public static JournalCodexRow Locked(string? displayName)
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
    /// paraphrases them.
    /// </summary>
    public class JournalCodex
    {
        private readonly JournalSystem _journal;
        private readonly JournalCatalogs _catalogs;
        private readonly Func<IReadOnlyList<ISurvivorAuthor>>? _getSurvivors;

        public JournalCodex(
            JournalSystem journal,
            JournalCatalogs catalogs,
            Func<IReadOnlyList<ISurvivorAuthor>>? getSurvivors = null)
        {
            _journal = journal;
            _catalogs = catalogs;
            _getSurvivors = getSurvivors;
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
            if (_catalogs?.Items == null) return rows;
            for (int i = 0; i < _catalogs.Items.Count; i++)
            {
                var item = _catalogs.Items[i];
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

        private static string BuildItemMeta(ItemDefinitionData item)
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

            if (_catalogs?.Survivors != null)
            {
                for (int i = 0; i < _catalogs.Survivors.Count; i++)
                {
                    var archetype = _catalogs.Survivors[i];
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

            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || string.IsNullOrEmpty(sv.Id)) continue;
                    if (!shown.Add(sv.Id)) continue;
                    rows.Add(_journal.IsSurvivorMet(sv.Id)
                        ? new JournalCodexRow
                        {
                            DisplayName = sv.DisplayName ?? sv.Id,
                            Meta = "survivor",
                            Body = sv.Id,
                            IsLocked = false
                        }
                        : JournalCodexRow.Locked(sv.DisplayName ?? sv.Id));
                }
            }
            return rows;
        }

        // -----------------------------------------------------------------
        // Places — locations, filtered by location_visited_*
        // -----------------------------------------------------------------

        private List<JournalCodexRow> BuildPlaceRows()
        {
            var rows = new List<JournalCodexRow>();
            if (_catalogs?.Locations == null) return rows;
            for (int i = 0; i < _catalogs.Locations.Count; i++)
            {
                var loc = _catalogs.Locations[i];
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

        private static string BuildLocationMeta(LocationDefinitionData loc)
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
            if (_catalogs?.Events == null) return rows;
            for (int i = 0; i < _catalogs.Events.Count; i++)
            {
                var evt = _catalogs.Events[i];
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
