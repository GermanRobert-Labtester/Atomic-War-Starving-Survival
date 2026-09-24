// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 162 — Shelter History & Archive System host wiring.
// The pure domain ShelterArchiveSystem is the authority for historical records,
// shelter milestones, governance decisions, and casualty remembrances.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterArchiveHostSession? _shelterArchive;
        private bool _shelterArchiveDirty;

        public ShelterArchiveHostSession? ShelterArchive => _shelterArchive;

        public void SetupShelterArchive()
        {
            if (_shelterArchive != null) return;

            var saved = ShelterArchiveSaveStore.TryLoad();
            _shelterArchive = ShelterArchiveHostSession.Create(saved, foundingDay: _simDay > 0 ? _simDay : 1);
            _shelterArchive.StateChanged += () => _shelterArchiveDirty = true;

            // Load authored category catalog
            string catalogPath = CatalogPath.ResolveCatalog("archive_categories.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _shelterArchive.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }

            // Bridge memorial entries if memorial system is already setup
            if (_memorial != null)
            {
                _shelterArchive.System.AttachMemorialSystem(_memorial);
            }
        }

        public ArchiveEntry RecordArchiveEvent(
            int day,
            string title,
            string description,
            ArchiveEntryType type = ArchiveEntryType.Event,
            ArchiveSignificance significance = ArchiveSignificance.Notable,
            IEnumerable<string>? tags = null,
            IEnumerable<string>? participantIds = null)
        {
            SetupShelterArchive();
            return _shelterArchive!.RecordEvent(day, title, description, type, significance, tags, participantIds);
        }

        public ArchiveEntry RecordArchiveMemorialLoss(MemorialEntry memorial, string dwellerName = "")
        {
            SetupShelterArchive();
            return _shelterArchive!.RecordMemorialLoss(memorial, dwellerName);
        }

        public IReadOnlyList<ArchiveEntry> GetShelterArchiveTimeline(
            ArchiveEntryType? typeFilter = null,
            ArchiveSignificance? minSignificance = null)
        {
            SetupShelterArchive();
            return _shelterArchive!.GetTimeline(typeFilter, minSignificance);
        }

        public IReadOnlyList<ArchiveEntry> SearchShelterArchive(
            string? keyword = null,
            string? tag = null,
            string? participantId = null,
            int? startDay = null,
            int? endDay = null)
        {
            SetupShelterArchive();
            return _shelterArchive!.Search(keyword, tag, participantId, startDay, endDay);
        }

        public ShelterArchiveCensus GetShelterArchiveCensus() =>
            _shelterArchive?.Census ?? default;

        public void TickShelterArchive(int day)
        {
            SetupShelterArchive();
            // Periodic milestone logging or institutional heartbeat
            if (day % 10 == 0)
            {
                _shelterArchive!.RecordEvent(
                    day,
                    $"Day {day} Shelter Milestone",
                    $"The shelter community has endured to Day {day}.",
                    ArchiveEntryType.Milestone,
                    ArchiveSignificance.Notable,
                    new[] { "milestone", "endurance" });
            }
        }

        public void SaveShelterArchive()
        {
            if (_shelterArchive == null) return;
            var state = _shelterArchive.System.CaptureState();
            ShelterArchiveSaveStore.TrySave(state);
            if (CaptureSection(
                    ShelterArchiveSaveStore.SectionName,
                    ShelterArchiveSaveStore.TryCapturePersisted(state)))
            {
                _shelterArchiveDirty = false;
            }
        }

        public void FlushShelterArchiveIfDirty()
        {
            if (_shelterArchiveDirty)
            {
                SaveShelterArchive();
            }
        }

        public void ResetShelterArchive()
        {
            _shelterArchive = null;
            _shelterArchiveDirty = false;
        }
    }
}
