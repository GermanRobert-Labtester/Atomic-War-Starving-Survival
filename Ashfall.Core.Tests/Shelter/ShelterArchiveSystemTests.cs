// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterArchiveSystemTests
    {
        [Fact]
        public void RecordEntry_GeneratesDefaultEntryId_AndFiresEvent()
        {
            var system = new ShelterArchiveSystem(foundingDay: 1);
            ArchiveEntry? observed = null;
            system.OnEntryRecorded += e => observed = e;

            var entry = new ArchiveEntry
            {
                Day = 5,
                Title = "Power Restored",
                Description = "Generator restored to full capacity.",
                Type = ArchiveEntryType.Milestone,
                Significance = ArchiveSignificance.Major
            };

            var recorded = system.RecordEntry(entry);

            Assert.NotNull(recorded);
            Assert.Equal(recorded, observed);
            Assert.StartsWith("arch_5_", recorded.EntryId);
            Assert.Equal(1, system.EntryCount);
        }

        [Fact]
        public void RecordEvent_StoresTagsAndParticipants_PreservesData()
        {
            var system = new ShelterArchiveSystem();

            var recorded = system.RecordEvent(
                day: 12,
                title: "Raid Defended",
                description: "Bandits repelled at the outer blast door.",
                type: ArchiveEntryType.Event,
                significance: ArchiveSignificance.Notable,
                tags: new[] { "defense", "combat", "security" },
                participantIds: new[] { "dweller_alice", "dweller_bob" });

            Assert.Equal(12, recorded.Day);
            Assert.Equal(ArchiveEntryType.Event, recorded.Type);
            Assert.Equal(ArchiveSignificance.Notable, recorded.Significance);
            Assert.Contains("defense", recorded.Tags);
            Assert.Equal(2, recorded.ParticipantIds.Count);
        }

        [Fact]
        public void RecordMemorialLoss_CreatesMemorialArchiveEntryWithCorrectTags()
        {
            var system = new ShelterArchiveSystem();
            var memorial = new MemorialEntry
            {
                SurvivorId = "dweller_charlie",
                Cause = "RadiationSickness",
                Day = 24,
                SurvivedDays = 23,
                Epitaph = "Always watched the perimeter.",
                HeirloomRecipientId = "dweller_dana"
            };

            var archiveEntry = system.RecordMemorialLoss(memorial, dwellerName: "Charlie Vance");

            Assert.Equal(24, archiveEntry.Day);
            Assert.Equal(ArchiveEntryType.Memorial, archiveEntry.Type);
            Assert.Equal(ArchiveSignificance.Major, archiveEntry.Significance);
            Assert.Contains("In Memoriam: Charlie Vance", archiveEntry.Title);
            Assert.Contains("RadiationSickness", archiveEntry.Description);
            Assert.Contains("radiationsickness", archiveEntry.Tags);
            Assert.Contains("memorial", archiveEntry.Tags);
            Assert.Contains("dweller_charlie", archiveEntry.ParticipantIds);
            Assert.Contains("dweller_dana", archiveEntry.ParticipantIds);
        }

        [Fact]
        public void AttachMemorialSystem_BridgesOnMemorializedEvent()
        {
            var memorialState = new MemorialState();
            var memorialSystem = new MemorialSystem(memorialState);
            var archiveSystem = new ShelterArchiveSystem();

            archiveSystem.AttachMemorialSystem(memorialSystem, id => id == "survivor_1" ? "Evelyn Cross" : id);

            memorialSystem.Memorialize(new MemorialInput
            {
                SurvivorId = "survivor_1",
                Cause = "Hypothermia",
                Day = 15,
                BirthDay = 1,
                FinalWishResolved = true,
                Epitaph = "Cold winds never broke her spirit."
            });

            Assert.Equal(1, archiveSystem.EntryCount);
            var timeline = archiveSystem.GetTimeline();
            Assert.Single(timeline);
            Assert.Contains("Evelyn Cross", timeline[0].Title);
            Assert.Equal(ArchiveEntryType.Memorial, timeline[0].Type);
        }

        [Fact]
        public void GetTimeline_FiltersByTypeAndSignificance_OrdersChronologically()
        {
            var system = new ShelterArchiveSystem();

            system.RecordEvent(day: 10, title: "Day 10 Event", description: "Minor checkup", type: ArchiveEntryType.Event, significance: ArchiveSignificance.Minor);
            system.RecordEvent(day: 2, title: "Day 2 Milestone", description: "Well drilled", type: ArchiveEntryType.Milestone, significance: ArchiveSignificance.Historic);
            system.RecordEvent(day: 5, title: "Day 5 Decision", description: "Rationing initiated", type: ArchiveEntryType.Decision, significance: ArchiveSignificance.Major);
            system.RecordEvent(day: 5, title: "Day 5 Discovery", description: "Geothermal vent", type: ArchiveEntryType.Discovery, significance: ArchiveSignificance.Notable);

            var fullTimeline = system.GetTimeline();
            Assert.Equal(4, fullTimeline.Count);
            Assert.Equal(2, fullTimeline[0].Day);
            Assert.Equal(5, fullTimeline[1].Day);
            Assert.Equal(5, fullTimeline[2].Day);
            Assert.Equal(10, fullTimeline[3].Day);

            var milestonesOnly = system.GetTimeline(typeFilter: ArchiveEntryType.Milestone);
            Assert.Single(milestonesOnly);
            Assert.Equal("Day 2 Milestone", milestonesOnly[0].Title);

            var majorOrHigher = system.GetTimeline(minSignificance: ArchiveSignificance.Major);
            Assert.Equal(2, majorOrHigher.Count);
            Assert.Contains(majorOrHigher, e => e.Title == "Day 2 Milestone");
            Assert.Contains(majorOrHigher, e => e.Title == "Day 5 Decision");
        }

        [Fact]
        public void Search_ByKeywordTagParticipantAndDayRange_ReturnsExpectedEntries()
        {
            var system = new ShelterArchiveSystem();

            system.RecordEvent(day: 3, title: "Radio Broadcast", description: "Heard distress signal from Valley", tags: new[] { "radio", "signal" }, participantIds: new[] { "survivor_a" });
            system.RecordEvent(day: 8, title: "Expedition Returns", description: "Brought back medical supplies", tags: new[] { "expedition", "medical" }, participantIds: new[] { "survivor_b" });
            system.RecordEvent(day: 15, title: "Medical Outbreak", description: "Flu spread through residential sector", tags: new[] { "medical", "crisis" }, participantIds: new[] { "survivor_a", "survivor_b" });

            // Keyword
            var signalMatches = system.Search(keyword: "distress");
            Assert.Single(signalMatches);
            Assert.Equal("Radio Broadcast", signalMatches[0].Title);

            // Tag
            var medicalMatches = system.Search(tag: "medical");
            Assert.Equal(2, medicalMatches.Count);

            // Participant
            var participantMatches = system.Search(participantId: "survivor_a");
            Assert.Equal(2, participantMatches.Count);

            // Day range
            var rangeMatches = system.Search(startDay: 5, endDay: 12);
            Assert.Single(rangeMatches);
            Assert.Equal("Expedition Returns", rangeMatches[0].Title);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ShelterArchiveSystem(foundingDay: 3);
            system1.RecordEvent(day: 7, title: "Found Water", description: "Clean aquifer tapped", tags: new[] { "water" }, participantIds: new[] { "dw_1" });
            system1.RecordEvent(day: 14, title: "Hydroponics Online", description: "First harvest underway", tags: new[] { "food" }, participantIds: new[] { "dw_2" });

            var state = system1.CaptureState();
            Assert.Equal(3, state.FoundingDay);
            Assert.Equal(2, state.Entries.Count);

            var system2 = new ShelterArchiveSystem();
            system2.RestoreState(state);

            Assert.Equal(3, system2.FoundingDay);
            Assert.Equal(2, system2.EntryCount);

            var timeline = system2.GetTimeline();
            Assert.Equal("Found Water", timeline[0].Title);
            Assert.Equal("Hydroponics Online", timeline[1].Title);
            Assert.Contains("water", timeline[0].Tags);
            Assert.Contains("dw_2", timeline[1].ParticipantIds);
        }

        [Fact]
        public void ProjectCanonicalSources_BuildsDetachedJournalAndMemorialIndex()
        {
            var journal = new JournalSystem();
            journal.TryAddRawEntry("power_restored", "The generator is stable again.", null!, day: 3);
            journal.TryAddRawEntry("water_found", "A clean seam opened beneath the east wall.", null!, day: 7);

            var memorial = new MemorialSystem(new MemorialState());
            memorial.Memorialize(new MemorialInput
            {
                SurvivorId = "dweller_charlie",
                Cause = "RadiationSickness",
                Day = 5,
                BirthDay = 1,
                Epitaph = "Always watched the perimeter."
            });

            var archive = new ShelterArchiveSystem();
            var projected = ShelterArchiveSystem.ProjectCanonicalSources(journal, memorial);
            var repeated = ShelterArchiveSystem.ProjectCanonicalSources(journal, memorial);

            Assert.Equal(3, projected.Count);
            Assert.StartsWith("journal:", projected[0].EntryId);
            Assert.Equal("memorial:dweller_charlie", projected[1].EntryId);
            Assert.Equal(0, archive.EntryCount);
            Assert.Equal(projected.Select(e => e.EntryId), repeated.Select(e => e.EntryId));
            Assert.Contains("radiationsickness", projected[1].Tags);
            Assert.Contains("dweller_charlie", projected[1].ParticipantIds);
        }
    }
}
