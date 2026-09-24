// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterArchiveSaveStore
// Core State : Ashfall.Core.Shelter.ShelterArchiveState
// Host Caller: Main.ShelterArchive
// Purpose    : Plan 162 — Shelter History & Archive System host session & persistence.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Memorial;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterArchiveSaveStore
    {
        public const string FileName = "shelter_archive_save.json";
        public const string SectionName = "shelter_archive";

        private static readonly SaveStore<ShelterArchiveState> s_store =
            SaveStoreHub.Checksummed<ShelterArchiveState>(FileName, nameof(ShelterArchiveSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ShelterArchiveState state) => s_store.CaptureBare(state);
        public static ShelterArchiveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ShelterArchiveState state) => s_store.TrySave(state);
        public static ShelterArchiveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 162 host session. Wraps <see cref="ShelterArchiveSystem"/>.
    /// Exposes search, chronological timeline filtering, milestone recording,
    /// governance decisions, and casualty remembrances.
    /// </summary>
    public sealed class ShelterArchiveHostSession : HostSessionBase
    {
        private readonly ShelterArchiveSystem _system;

        public ShelterArchiveSystem System => _system;
        public ShelterArchiveCensus Census => _system.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public ShelterArchiveHostSession(ShelterArchiveState? state = null, int foundingDay = 1)
        {
            _system = new ShelterArchiveSystem(state, foundingDay);
        }

        public static ShelterArchiveHostSession Create(ShelterArchiveState? state = null, int foundingDay = 1) =>
            new ShelterArchiveHostSession(state, foundingDay);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            LastEvent = $"Loaded {_system.AuthoredCategories.Count} archive categories.";
            RaiseStateChanged();
        }

        public ArchiveEntry RecordEntry(ArchiveEntry entry)
        {
            var recorded = _system.RecordEntry(entry);
            LastEvent = $"Recorded archive entry: [{recorded.Type}] '{recorded.Title}' (Day {recorded.Day}).";
            RaiseStateChanged();
            return recorded;
        }

        public ArchiveEntry RecordEvent(
            int day,
            string title,
            string description,
            ArchiveEntryType type = ArchiveEntryType.Event,
            ArchiveSignificance significance = ArchiveSignificance.Notable,
            IEnumerable<string>? tags = null,
            IEnumerable<string>? participantIds = null)
        {
            var recorded = _system.RecordEvent(day, title, description, type, significance, tags, participantIds);
            LastEvent = $"Recorded historical event '{title}' (Day {day}).";
            RaiseStateChanged();
            return recorded;
        }

        public ArchiveEntry RecordMemorialLoss(MemorialEntry memorial, string dwellerName = "")
        {
            var recorded = _system.RecordMemorialLoss(memorial, dwellerName);
            LastEvent = $"Archived memorial tribute for '{recorded.Title}' (Day {recorded.Day}).";
            RaiseStateChanged();
            return recorded;
        }

        public IReadOnlyList<ArchiveEntry> GetTimeline(
            ArchiveEntryType? typeFilter = null,
            ArchiveSignificance? minSignificance = null) =>
            _system.GetTimeline(typeFilter, minSignificance);

        public IReadOnlyList<ArchiveEntry> Search(
            string? keyword = null,
            string? tag = null,
            string? participantId = null,
            int? startDay = null,
            int? endDay = null) =>
            _system.Search(keyword, tag, participantId, startDay, endDay);
    }
}
