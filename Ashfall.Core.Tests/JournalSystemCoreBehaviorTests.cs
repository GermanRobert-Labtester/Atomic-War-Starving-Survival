using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Tests
{
    public class JournalSystemCoreBehaviorTests
    {
        private class TestAuthor : ISurvivorAuthor
        {
            public string Id { get; set; } = "survivor_dr_sarah_chen";
            public string DisplayName { get; set; } = "Dr. Sarah Chen";
            public RiskBiasTrait RiskBias { get; set; } = RiskBiasTrait.Realist;
        }

        [Fact]
        public void JournalSystem_RawEntryAndCodex_DiscoversAndUnlocks()
        {
            var journal = new JournalSystem();
            var author = new TestAuthor();

            var entry = journal.TryAddRawEntry("event_first_snow", "The first radioactive snow began falling.", author, day: 5, hour: 14f);
            Assert.NotNull(entry);
            Assert.Equal("event_first_snow", entry.KnowledgeKey);
            Assert.Equal("Dr. Sarah Chen", entry.AuthorName);
            Assert.Equal(5, entry.Day);
            Assert.True(journal.HasUnread);
            Assert.True(journal.NotificationPing);

            // Duplicate discovery rejected
            var dupe = journal.TryAddRawEntry("event_first_snow", "Another snow entry", author, day: 6);
            Assert.Null(dupe);
            Assert.Single(journal.Entries);

            // Codex unlocks
            bool unlockedItem = journal.UnlockItemSeen("item_gas_mask");
            Assert.True(unlockedItem);
            Assert.True(journal.IsItemSeen("item_gas_mask"));
            Assert.Equal(1, journal.CodexUnlockCount);

            // Duplicate codex unlock returns false
            Assert.False(journal.UnlockItemSeen("item_gas_mask"));
        }

        [Fact]
        public void JournalSystem_TabsAndReadState_TracksAccurately()
        {
            var journal = new JournalSystem();
            var author = new TestAuthor();

            journal.TryAddRawEntry("loc_bunker_hatch", "Found the old bunker hatch.", author, day: 1);
            Assert.True(journal.HasUnread);

            journal.MarkRead();
            Assert.False(journal.HasUnread);
            Assert.False(journal.NotificationPing);

            journal.SwitchTab(2);
            Assert.Equal(2, journal.ActiveTab);
        }

        [Fact]
        public void JournalSystem_SaveAndRestore_RoundTripsExactly()
        {
            var journal = new JournalSystem();
            var author = new TestAuthor();

            journal.TryAddRawEntry("entry_1", "Log entry 1", author, day: 2, hour: 8f);
            journal.TryAddRawEntry("entry_2", "Log entry 2", author, day: 3, hour: 12f);
            journal.UnlockItemSeen("item_radio_vacuum_tube");
            journal.UnlockLocationVisited("loc_substation_echo");

            var save = journal.CaptureState();
            Assert.NotNull(save);
            Assert.Equal(2, save.Entries.Length);
            Assert.Equal(2, save.CodexUnlockCount);

            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(save);
            Assert.False(string.IsNullOrWhiteSpace(json));

            var restoredSave = serializer.Deserialize<JournalSave>(json);
            Assert.NotNull(restoredSave);

            var newJournal = new JournalSystem();
            newJournal.RestoreState(restoredSave);

            Assert.Equal(2, newJournal.EntryCount);
            Assert.Equal(2, newJournal.CodexUnlockCount);
            Assert.True(newJournal.IsItemSeen("item_radio_vacuum_tube"));
            Assert.True(newJournal.IsLocationVisited("loc_substation_echo"));
            Assert.Equal("entry_2", newJournal.Entries[0].KnowledgeKey);
        }
    }
}
