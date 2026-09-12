// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;

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
        public IReadOnlyList<JournalCodexLink>? Links;
        public string? NavigationId;

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

    /// <summary>A bounded codex link. The target is an existing catalog ID.</summary>
    public sealed class JournalCodexLink
    {
        public string Id { get; }
        public string Label { get; }
        public string RoutePrefix { get; }

        public JournalCodexLink(string id, string label, string routePrefix = "bureaucratic_document")
        {
            Id = id ?? string.Empty;
            Label = label ?? string.Empty;
            RoutePrefix = routePrefix ?? string.Empty;
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

        public JournalCatalogs Catalogs => _catalogs;

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
            AppendRoomHistoryRows(rows);
            return rows;
        }

        /// <summary>
        /// Plan 29 Task 29A: shelter room-history vignettes render as Places rows
        /// gated by the room_history_seen_* knowledge key. Locked rows show the
        /// room, not the vignette title, so the discovery is not spoilt.
        /// </summary>
        private void AppendRoomHistoryRows(List<JournalCodexRow> rows)
        {
            if (_catalogs?.RoomHistories == null) return;
            for (int i = 0; i < _catalogs.RoomHistories.Count; i++)
            {
                var v = _catalogs.RoomHistories[i];
                if (v == null || string.IsNullOrEmpty(v.id)) continue;
                string roomName = v.roomDisplayName ?? v.roomId ?? "Shelter";
                rows.Add(_journal.IsRoomHistorySeen(v.id)
                    ? new JournalCodexRow
                    {
                        DisplayName = v.title ?? roomName,
                        Meta = $"Shelter · {roomName}" + (string.IsNullOrEmpty(v.timePeriod) ? "" : $" · {v.timePeriod}"),
                        Body = v.body ?? string.Empty,
                        IsLocked = false
                    }
                    : JournalCodexRow.Locked($"{roomName} — untold history"));
            }
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

            // Verdict world-history ladder (lore_verdict_*) — gated the same way:
            // a beat is only readable once the Verdict host has unlocked it.
            if (_catalogs.VerdictHistory != null)
            {
                for (int i = 0; i < _catalogs.VerdictHistory.Count; i++)
                {
                    var beat = _catalogs.VerdictHistory[i];
                    if (beat == null || string.IsNullOrEmpty(beat.id)) continue;
                    rows.Add(_journal.IsEventFired(beat.id)
                        ? new JournalCodexRow
                        {
                            DisplayName = beat.title ?? beat.id,
                            Meta = "The machine's register",
                            Body = beat.bodyText ?? string.Empty,
                            IsLocked = false
                        }
                        : JournalCodexRow.Locked(beat.title ?? beat.id));
                }
            }

            AppendNarrativeDiscoveryRows(rows);
            AppendBureaucraticDocumentRows(rows);

            return rows;
        }

        private void AppendNarrativeDiscoveryRows(List<JournalCodexRow> rows)
        {
            if (_catalogs?.NarrativeDiscoveries == null) return;
            var records = _catalogs.NarrativeDiscoveries.AllRecords;
            for (int i = 0; i < records.Count; i++)
            {
                var rec = records[i];
                if (rec == null || string.IsNullOrEmpty(rec.DiscoveryId)) continue;
                if (_journal.IsNarrativeDiscovered(rec.DiscoveryId))
                {
                    rows.Add(new JournalCodexRow
                    {
                        DisplayName = rec.Title,
                        Meta = BuildNarrativeMeta(rec),
                        Body = rec.BodyText,
                        IsLocked = false,
                        Links = BuildNarrativeLinks(_catalogs.NarrativeDiscoveries, rec),
                        NavigationId = rec.DiscoveryId
                    });
                }
                else
                {
                    rows.Add(JournalCodexRow.Locked($"{rec.Category} — undiscovered"));
                }
            }
        }

        private static string BuildNarrativeMeta(NarrativeDiscoveredRecord record)
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(record.TruthClass)) parts.Add(record.TruthClass);
            if (!string.IsNullOrEmpty(record.ProvenanceLabel)) parts.Add(record.ProvenanceLabel);
            if (!string.IsNullOrEmpty(record.NumericClaimLabel)) parts.Add(record.NumericClaimLabel);
            if (!string.IsNullOrEmpty(record.IdentityStatus)) parts.Add(record.IdentityStatus);
            if (!string.IsNullOrEmpty(record.RecordFamily)) parts.Add(record.RecordFamily);
            if (!string.IsNullOrEmpty(record.FacilityOrStationLabel)) parts.Add(record.FacilityOrStationLabel);
            if (!string.IsNullOrEmpty(record.TechnicalSummary)) parts.Add(record.TechnicalSummary);
            if (!string.IsNullOrEmpty(record.Subtitle)) parts.Add(record.Subtitle);
            if (parts.Count == 0) parts.Add(record.Category);
            return string.Join(" · ", parts);
        }

        private IReadOnlyList<JournalCodexLink> BuildNarrativeLinks(
            NarrativeDiscoveryCatalog catalog,
            NarrativeDiscoveredRecord record)
        {
            var links = new List<JournalCodexLink>();
            if (record.RelatedDiscoveryIds == null) return links;
            for (int i = 0; i < record.RelatedDiscoveryIds.Length; i++)
            {
                string relatedId = record.RelatedDiscoveryIds[i];
                if (catalog.TryGetRecord(relatedId, out var related)
                    && related != null
                    && _journal.IsNarrativeDiscovered(related.DiscoveryId))
                {
                    links.Add(new JournalCodexLink(
                        related.DiscoveryId,
                        related.Title,
                        "narrative_discovery"));
                }
            }
            return links;
        }

        private void AppendBureaucraticDocumentRows(List<JournalCodexRow> rows)
        {
            var catalog = _catalogs?.BureaucraticDocuments;
            if (catalog == null) return;

            for (int i = 0; i < catalog.Documents.Count; i++)
            {
                var document = catalog.Documents[i];
                if (document == null) continue;
                bool discovered = _journal.IsBureaucraticDocumentDiscovered(document.DocId);
                if (!discovered)
                {
                    rows.Add(JournalCodexRow.Locked($"{document.DocType.Replace('_', ' ')} — undiscovered"));
                    continue;
                }

                string meta = $"{document.TruthLabel} · Day {document.PostedDay} · " +
                    $"{document.PostedBy} · {document.Location} · {document.Material}";
                rows.Add(new JournalCodexRow
                {
                    DisplayName = document.Title,
                    Meta = meta,
                    Body = document.Transcript,
                    IsLocked = false,
                    Links = BuildDocumentLinks(catalog, document),
                    NavigationId = document.DocId
                });
            }
        }

        private static IReadOnlyList<JournalCodexLink> BuildDocumentLinks(
            BureaucraticDocumentCatalog catalog,
            BureaucraticDocumentDefinition document)
        {
            var links = new List<JournalCodexLink>();
            for (int i = 0; i < document.RelatedDocumentIds.Count; i++)
            {
                string relatedId = document.RelatedDocumentIds[i];
                if (catalog.TryGet(relatedId, out var related))
                    links.Add(new JournalCodexLink(related.DocId, related.Title));
            }
            return links;
        }
    }
}
