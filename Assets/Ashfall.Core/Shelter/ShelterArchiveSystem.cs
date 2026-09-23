// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Shelter
{
    public enum ArchiveEntryType
    {
        Event = 0,
        Decision = 1,
        Memorial = 2,
        Milestone = 3,
        Discovery = 4,
        Achievement = 5
    }

    public enum ArchiveSignificance
    {
        Minor = 0,
        Notable = 1,
        Major = 2,
        Historic = 3
    }

    [Serializable]
    public class ArchiveEntry
    {
        public string EntryId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public ArchiveEntryType Type { get; set; } = ArchiveEntryType.Event;
        public ArchiveSignificance Significance { get; set; } = ArchiveSignificance.Notable;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> ParticipantIds { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class ArchiveCategoryDef
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> EntryTypes { get; set; } = new List<string>();
        public int DisplayOrder { get; set; } = 1;
    }

    [Serializable]
    public class ShelterArchiveState
    {
        public int SchemaVersion { get; set; } = 1;
        public int FoundingDay { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<ArchiveEntry> Entries { get; set; } = new List<ArchiveEntry>();
        public List<ArchiveCategoryDef> AuthoredCategories { get; set; } = new List<ArchiveCategoryDef>();
    }

    /// <summary>
    /// Plan 162 / C2[34] — Shelter History & Archive System.
    /// Provides an additive query model and searchable institutional memory over
    /// shelter milestones, governance decisions, memorial losses, and historical events.
    /// </summary>
    public sealed class ShelterArchiveSystem
    {
        private readonly ShelterArchiveState _state;

        public event Action<ArchiveEntry>? OnEntryRecorded;

        public Action<ArchiveEntry>? OnEntryRecordedSeam { get; set; }
        public Action<ArchiveEntry>? OnMemorialRecordedSeam { get; set; }

        public int FoundingDay => _state.FoundingDay;
        public int EntryCount => _state.Entries.Count;
        public IReadOnlyList<ArchiveCategoryDef> AuthoredCategories => _state.AuthoredCategories;

        public ShelterArchiveSystem(ShelterArchiveState? state = null, int foundingDay = 1)
        {
            _state = state ?? new ShelterArchiveState { FoundingDay = foundingDay };
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("categories", out var catEl) && catEl.ValueKind == JsonValueKind.Array)
                {
                    _state.AuthoredCategories.Clear();
                    foreach (var item in catEl.EnumerateArray())
                    {
                        var types = new List<string>();
                        if (item.TryGetProperty("entry_types", out var typesEl) && typesEl.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var t in typesEl.EnumerateArray())
                            {
                                string? str = t.GetString();
                                if (!string.IsNullOrEmpty(str)) types.Add(str);
                            }
                        }

                        var def = new ArchiveCategoryDef
                        {
                            CategoryId = item.TryGetProperty("category_id", out var idProp) ? idProp.GetString() ?? "" : "",
                            CategoryName = item.TryGetProperty("category_name", out var nameProp) ? nameProp.GetString() ?? "" : "",
                            Description = item.TryGetProperty("description", out var descProp) ? descProp.GetString() ?? "" : "",
                            EntryTypes = types,
                            DisplayOrder = item.TryGetProperty("display_order", out var orderProp) ? orderProp.GetInt32() : 1
                        };

                        if (!string.IsNullOrEmpty(def.CategoryId))
                        {
                            _state.AuthoredCategories.Add(def);
                        }
                    }
                }
            }
            catch
            {
                // Catalog parse fallback
            }
        }

        /// <summary>
        /// Projects the existing journal and memorial authorities into a
        /// searchable archive read index. No archive records are appended and
        /// no source state is copied into a second save section.
        /// </summary>
        public static IReadOnlyList<ArchiveEntry> ProjectCanonicalSources(
            JournalSystem? journal,
            MemorialSystem? memorial)
        {
            var byId = new Dictionary<string, ArchiveEntry>(StringComparer.Ordinal);

            if (journal != null)
            {
                foreach (var entry in journal.Entries)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.Id)) continue;
                    string sourceId = $"journal:{entry.Id}";
                    byId[sourceId] = new ArchiveEntry
                    {
                        EntryId = sourceId,
                        Day = Math.Max(1, entry.Day),
                        Type = ArchiveEntryType.Discovery,
                        Significance = ArchiveSignificance.Notable,
                        Title = string.IsNullOrEmpty(entry.KnowledgeKey) ? entry.Id : entry.KnowledgeKey,
                        Description = entry.Text ?? string.Empty,
                        Tags = new List<string> { "journal" },
                        ParticipantIds = string.IsNullOrEmpty(entry.AuthorId)
                            ? new List<string>()
                            : new List<string> { entry.AuthorId }
                    };
                }
            }

            if (memorial != null)
            {
                foreach (var entry in memorial.Entries)
                {
                    if (entry == null || string.IsNullOrEmpty(entry.SurvivorId)) continue;
                    string sourceId = $"memorial:{entry.SurvivorId}";
                    string name = entry.SurvivorId;
                    string description = $"Survivor fell on Day {entry.Day} due to {entry.Cause ?? "unspecified"}. Survived {entry.SurvivedDays} days.";
                    if (!string.IsNullOrEmpty(entry.Epitaph))
                        description += $" Epitaph: \"{entry.Epitaph}\"";

                    var participants = new List<string> { entry.SurvivorId };
                    if (!string.IsNullOrEmpty(entry.HeirloomRecipientId))
                        participants.Add(entry.HeirloomRecipientId);

                    var tags = new List<string> { "memorial", "casualty" };
                    if (!string.IsNullOrEmpty(entry.Cause)) tags.Add(entry.Cause.ToLowerInvariant());

                    byId[sourceId] = new ArchiveEntry
                    {
                        EntryId = sourceId,
                        Day = Math.Max(1, entry.Day),
                        Type = ArchiveEntryType.Memorial,
                        Significance = ArchiveSignificance.Major,
                        Title = $"In Memoriam: {name}",
                        Description = description,
                        Tags = tags,
                        ParticipantIds = participants
                    };
                }
            }

            return byId.Values
                .OrderBy(e => e.Day)
                .ThenBy(e => e.EntryId, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Records an authored or generated entry directly into the institutional archive.
        /// </summary>
        public ArchiveEntry RecordEntry(ArchiveEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            if (string.IsNullOrEmpty(entry.EntryId))
            {
                entry.EntryId = $"arch_{entry.Day}_{_state.NextSequence++}";
            }

            entry.Tags ??= new List<string>();
            entry.ParticipantIds ??= new List<string>();

            _state.Entries.Add(entry);
            OnEntryRecorded?.Invoke(entry);
            OnEntryRecordedSeam?.Invoke(entry);

            if (entry.Type == ArchiveEntryType.Memorial)
            {
                OnMemorialRecordedSeam?.Invoke(entry);
            }

            return entry;
        }

        /// <summary>
        /// Convenience method to format and record a historical shelter event.
        /// </summary>
        public ArchiveEntry RecordEvent(
            int day,
            string title,
            string description,
            ArchiveEntryType type = ArchiveEntryType.Event,
            ArchiveSignificance significance = ArchiveSignificance.Notable,
            IEnumerable<string>? tags = null,
            IEnumerable<string>? participantIds = null)
        {
            var entry = new ArchiveEntry
            {
                Day = Math.Max(1, day),
                Title = title ?? string.Empty,
                Description = description ?? string.Empty,
                Type = type,
                Significance = significance,
                Tags = tags != null ? new List<string>(tags) : new List<string>(),
                ParticipantIds = participantIds != null ? new List<string>(participantIds) : new List<string>()
            };

            return RecordEntry(entry);
        }

        /// <summary>
        /// Automatically registers a memorialized dweller loss into the historical archive.
        /// Bridges directly from MemorialSystem.OnMemorialized.
        /// </summary>
        public ArchiveEntry RecordMemorialLoss(MemorialEntry memorial, string dwellerName = "")
        {
            if (memorial == null) throw new ArgumentNullException(nameof(memorial));

            string name = string.IsNullOrEmpty(dwellerName) ? memorial.SurvivorId : dwellerName;
            string title = $"In Memoriam: {name}";
            string desc = $"Survivor fell on Day {memorial.Day} due to {memorial.Cause}. Survived {memorial.SurvivedDays} days.";
            if (!string.IsNullOrEmpty(memorial.Epitaph))
            {
                desc += $" Epitaph: \"{memorial.Epitaph}\"";
            }

            var tags = new List<string> { "memorial", "casualty", memorial.Cause.ToLowerInvariant() };
            var participants = new List<string> { memorial.SurvivorId };
            if (!string.IsNullOrEmpty(memorial.HeirloomRecipientId))
            {
                participants.Add(memorial.HeirloomRecipientId);
            }

            return RecordEvent(
                day: memorial.Day,
                title: title,
                description: desc,
                type: ArchiveEntryType.Memorial,
                significance: ArchiveSignificance.Major,
                tags: tags,
                participantIds: participants);
        }

        /// <summary>
        /// Connects this archive system to listen to deaths recorded by MemorialSystem.
        /// </summary>
        public void AttachMemorialSystem(MemorialSystem memorialSystem, Func<string, string>? nameLookup = null)
        {
            if (memorialSystem == null) return;
            memorialSystem.OnMemorialized += entry =>
            {
                string name = nameLookup != null ? nameLookup(entry.SurvivorId) : entry.SurvivorId;
                RecordMemorialLoss(entry, name);
            };
        }

        /// <summary>
        /// Retrieves the historical timeline sorted chronologically.
        /// </summary>
        public IReadOnlyList<ArchiveEntry> GetTimeline(
            ArchiveEntryType? typeFilter = null,
            ArchiveSignificance? minSignificance = null)
        {
            var query = _state.Entries.AsEnumerable();

            if (typeFilter.HasValue)
            {
                query = query.Where(e => e.Type == typeFilter.Value);
            }

            if (minSignificance.HasValue)
            {
                query = query.Where(e => e.Significance >= minSignificance.Value);
            }

            return query.OrderBy(e => e.Day).ThenBy(e => e.EntryId, StringComparer.Ordinal).ToList();
        }

        /// <summary>
        /// Searches the archive by query keyword, tag, participant, or day range.
        /// </summary>
        public IReadOnlyList<ArchiveEntry> Search(
            string? keyword = null,
            string? tag = null,
            string? participantId = null,
            int? startDay = null,
            int? endDay = null)
        {
            var query = _state.Entries.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                query = query.Where(e =>
                    e.Title.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Description.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                string t = tag.Trim();
                query = query.Where(e => e.Tags.Any(existingTag => string.Equals(existingTag, t, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(participantId))
            {
                string p = participantId.Trim();
                query = query.Where(e => e.ParticipantIds.Any(pid => string.Equals(pid, p, StringComparison.OrdinalIgnoreCase)));
            }

            if (startDay.HasValue)
            {
                query = query.Where(e => e.Day >= startDay.Value);
            }

            if (endDay.HasValue)
            {
                query = query.Where(e => e.Day <= endDay.Value);
            }

            return query.OrderBy(e => e.Day).ToList();
        }

        public ShelterArchiveState CaptureState()
        {
            var captured = new ShelterArchiveState
            {
                SchemaVersion = _state.SchemaVersion,
                FoundingDay = _state.FoundingDay,
                NextSequence = _state.NextSequence,
                Entries = new List<ArchiveEntry>(_state.Entries.Count),
                AuthoredCategories = new List<ArchiveCategoryDef>(_state.AuthoredCategories)
            };

            for (int i = 0; i < _state.Entries.Count; i++)
            {
                var src = _state.Entries[i];
                captured.Entries.Add(new ArchiveEntry
                {
                    EntryId = src.EntryId,
                    Day = src.Day,
                    Type = src.Type,
                    Significance = src.Significance,
                    Title = src.Title,
                    Description = src.Description,
                    Tags = new List<string>(src.Tags),
                    ParticipantIds = new List<string>(src.ParticipantIds)
                });
            }

            return captured;
        }

        public void RestoreState(ShelterArchiveState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.FoundingDay = state.FoundingDay;
            _state.NextSequence = state.NextSequence;
            _state.Entries.Clear();
            _state.AuthoredCategories.Clear();

            if (state.AuthoredCategories != null)
            {
                _state.AuthoredCategories.AddRange(state.AuthoredCategories);
            }

            if (state.Entries != null)
            {
                for (int i = 0; i < state.Entries.Count; i++)
                {
                    var src = state.Entries[i];
                    _state.Entries.Add(new ArchiveEntry
                    {
                        EntryId = src.EntryId,
                        Day = src.Day,
                        Type = src.Type,
                        Significance = src.Significance,
                        Title = src.Title,
                        Description = src.Description,
                        Tags = new List<string>(src.Tags ?? Enumerable.Empty<string>()),
                        ParticipantIds = new List<string>(src.ParticipantIds ?? Enumerable.Empty<string>())
                    });
                }
            }
        }
    }
}
