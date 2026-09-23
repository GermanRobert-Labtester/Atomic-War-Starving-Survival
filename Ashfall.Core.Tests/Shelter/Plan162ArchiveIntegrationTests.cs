// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan162Archive
{
    public sealed class Plan162ArchiveIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void CatalogIntegrity_ArchiveCategoriesJson_LoadsAllCanonicalCategories()
        {
            string path = ResolveDataPath("archive_categories.json");
            Assert.True(File.Exists(path), $"Catalog missing at: {path}");

            string json = File.ReadAllText(path);
            var system = new ShelterArchiveSystem();
            system.LoadCatalog(json);

            Assert.True(system.AuthoredCategories.Count >= 6, "Expected at least 6 archive categories.");

            var decisionCat = system.AuthoredCategories.FirstOrDefault(c => c.CategoryId == "cat_decisions");
            Assert.NotNull(decisionCat);
            Assert.Equal("Governance Decisions", decisionCat.CategoryName);
            Assert.Contains("Decision", decisionCat.EntryTypes);

            var memorialCat = system.AuthoredCategories.FirstOrDefault(c => c.CategoryId == "cat_memorials");
            Assert.NotNull(memorialCat);
            Assert.Contains("Memorial", memorialCat.EntryTypes);
        }

        [Fact]
        public void RecordEntry_DispatchesSeams_AndMaintainsTimelineChronology()
        {
            var system = new ShelterArchiveSystem(foundingDay: 1);
            ArchiveEntry? recordedFromSeam = null;
            system.OnEntryRecordedSeam = e => recordedFromSeam = e;

            // Record out of order: Day 10, Day 2, Day 5
            system.RecordEvent(day: 10, title: "Expedition Returns", description: "Scouts returned with relic.", type: ArchiveEntryType.Discovery);
            system.RecordEvent(day: 2, title: "Shelter Sealed", description: "Primary blast doors locked.", type: ArchiveEntryType.Milestone);
            system.RecordEvent(day: 5, title: "Refugees Admitted", description: "Council voted to accept travelers.", type: ArchiveEntryType.Decision);

            Assert.NotNull(recordedFromSeam);
            Assert.Equal(3, system.EntryCount);

            var timeline = system.GetTimeline();
            Assert.Equal(3, timeline.Count);
            Assert.Equal(2, timeline[0].Day);
            Assert.Equal("Shelter Sealed", timeline[0].Title);
            Assert.Equal(5, timeline[1].Day);
            Assert.Equal("Refugees Admitted", timeline[1].Title);
            Assert.Equal(10, timeline[2].Day);
            Assert.Equal("Expedition Returns", timeline[2].Title);
        }

        [Fact]
        public void MemorialIntegration_RecordsCasualties_AndDispatchesMemorialSeam()
        {
            var system = new ShelterArchiveSystem();
            ArchiveEntry? memorialRecorded = null;
            system.OnMemorialRecordedSeam = m => memorialRecorded = m;

            var memorial = new MemorialEntry
            {
                SurvivorId = "survivor_harlan",
                Cause = "ElectricalFire",
                Day = 18,
                SurvivedDays = 17,
                Epitaph = "Shielded the auxiliary power relays.",
                HeirloomRecipientId = "survivor_mara"
            };

            var archive = system.RecordMemorialLoss(memorial, dwellerName: "Harlan Vance");

            Assert.NotNull(memorialRecorded);
            Assert.Equal(archive, memorialRecorded);
            Assert.Equal(ArchiveEntryType.Memorial, archive.Type);
            Assert.Equal(ArchiveSignificance.Major, archive.Significance);
            Assert.Contains("Harlan Vance", archive.Title);
            Assert.Contains("electricalfire", archive.Tags);
            Assert.Contains("memorial", archive.Tags);
            Assert.Contains("casualty", archive.Tags);
            Assert.Contains("survivor_harlan", archive.ParticipantIds);
            Assert.Contains("survivor_mara", archive.ParticipantIds);
        }

        [Fact]
        public void FilteringAndSearch_QueryByKeywordTagsAndDayRange()
        {
            var system = new ShelterArchiveSystem();

            system.RecordEvent(day: 3, title: "Subterranean Seismic Shudder", description: "Bedrock settled 4cm without structural failure.", type: ArchiveEntryType.Event, tags: new[] { "seismic", "shelter" }, participantIds: new[] { "engineer_1" });
            system.RecordEvent(day: 7, title: "Hydroponics Garden Planted", description: "First batch of seed potatoes sown in bay 2.", type: ArchiveEntryType.Milestone, tags: new[] { "agriculture", "food" });
            system.RecordEvent(day: 12, title: "Raider Skirmish at Trench", description: "Perimeter sentries repelled hostile scout probe.", type: ArchiveEntryType.Event, tags: new[] { "combat", "defense" }, participantIds: new[] { "soldier_alpha" });

            // 1. Keyword search
            var kwResults = system.Search(keyword: "seismic");
            Assert.Single(kwResults);
            Assert.Equal(3, kwResults[0].Day);

            // 2. Tag filter
            var tagResults = system.Search(tag: "agriculture");
            Assert.Single(tagResults);
            Assert.Equal("Hydroponics Garden Planted", tagResults[0].Title);

            // 3. Participant search
            var partResults = system.Search(participantId: "soldier_alpha");
            Assert.Single(partResults);
            Assert.Equal(12, partResults[0].Day);

            // 4. Day range filter
            var rangeResults = system.Search(startDay: 5, endDay: 10);
            Assert.Single(rangeResults);
            Assert.Equal(7, rangeResults[0].Day);
        }

        [Fact]
        public void ProjectCanonicalSources_SynthesizesJournalAndMemorialWithoutStateDuplication()
        {
            var journal = new JournalSystem();
            journal.TryAddRawEntry("medical_storage", "Discovered intact medical storage.", null!, day: 4);

            var memorial = new MemorialSystem(new MemorialState());
            memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "dweller_jonah",
                Cause = "RadiationToxin",
                Day = 8,
                BirthDay = 1,
                Epitaph = "Found water when we had none."
            });

            var projected = ShelterArchiveSystem.ProjectCanonicalSources(journal, memorial);

            Assert.Equal(2, projected.Count);
            Assert.Contains(projected, p => p.EntryId.StartsWith("journal:"));
            Assert.Contains(projected, p => p.EntryId == "memorial:dweller_jonah");

            var memProjected = projected.First(p => p.EntryId == "memorial:dweller_jonah");
            Assert.Equal(8, memProjected.Day);
            Assert.Equal(ArchiveEntryType.Memorial, memProjected.Type);
            Assert.Contains("dweller_jonah", memProjected.ParticipantIds);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesFullArchiveContinuity()
        {
            string path = ResolveDataPath("archive_categories.json");
            var system1 = new ShelterArchiveSystem(foundingDay: 3);
            if (File.Exists(path)) system1.LoadCatalog(File.ReadAllText(path));

            system1.RecordEvent(day: 4, title: "Founding Decree", description: "Bunker governance charter ratified.", type: ArchiveEntryType.Decision, significance: ArchiveSignificance.Historic);
            system1.RecordEvent(day: 6, title: "Sump Overflow Contained", description: "Secondary pumps installed.", type: ArchiveEntryType.Event);

            var state = system1.CaptureState();
            Assert.Equal(1, state.SchemaVersion);
            Assert.Equal(3, state.FoundingDay);
            Assert.Equal(2, state.Entries.Count);
            Assert.True(state.AuthoredCategories.Count >= 6);

            var system2 = new ShelterArchiveSystem();
            system2.RestoreState(state);

            Assert.Equal(3, system2.FoundingDay);
            Assert.Equal(2, system2.EntryCount);
            Assert.True(system2.AuthoredCategories.Count >= 6);

            var timeline = system2.GetTimeline();
            Assert.Equal("Founding Decree", timeline[0].Title);
            Assert.Equal(ArchiveSignificance.Historic, timeline[0].Significance);
            Assert.Equal("Sump Overflow Contained", timeline[1].Title);
        }
    }
}
