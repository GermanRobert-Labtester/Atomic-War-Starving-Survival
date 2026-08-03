using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Diegetic journal: first-time discoveries write trait-voiced entries,
    /// ping the journal book UI (no modal popups), and never double-fire.
    /// </summary>
    [TestFixture]
    public class JournalSystemTests
    {
        private GameObject _hudObject;
        private HUD _hud;

        [SetUp]
        public void SetUp()
        {
            _hudObject = new GameObject("JournalTestHUD");
            _hud = _hudObject.AddComponent<HUD>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_hudObject != null)
                Object.DestroyImmediate(_hudObject);
        }

        [Test]
        public void HighCo2_FirstTime_AddsJournalEntry_AndPingsBookUI()
        {
            var author = new Survivor
            {
                Id = "sv_cautious",
                DisplayName = "Mara",
                RiskBias = RiskBiasTrait.Cautious
            };
            var shelter = new Shelter();
            var inventory = new Inventory { Capacity = 10 };
            var context = new EventContext(author, shelter, inventory)
            {
                CurrentDay = 32,
                CurrentHour = 14f,
                // Trigger high_co2 via diesel CO (Atmosphere #20) without needing modules.
                CarbonMonoxidePpm = SleepQualitySystem.HighCo2PpmThreshold + 5f,
                IndoorTemperatureC = 15f
            };

            var journal = new JournalSystem();
            var book = _hud.EnsureJournalBook();
            Assert.That(book, Is.Not.Null);
            Assert.That(book.EntryCount, Is.EqualTo(0));
            Assert.That(book.NotificationPing, Is.False);

            // Mirror GameBootstrap: live discoveries push to the book + ping.
            journal.OnEntryAdded += entry => book.Push(entry);

            var runner = new EventRunner();
            int added = runner.ObserveDiscoveries(journal, context);

            Assert.That(added, Is.EqualTo(1), "First high_co2 must write one entry");
            Assert.That(journal.EntryCount, Is.EqualTo(1));
            Assert.That(journal.Knowledge.Has(KnowledgeKeys.HighCo2), Is.True);
            Assert.That(journal.HasUnread, Is.True);
            Assert.That(journal.NotificationPing, Is.True);
            Assert.That(journal.NotificationPingCount, Is.EqualTo(1));

            var entry = journal.Entries[0];
            Assert.That(entry.KnowledgeKey, Is.EqualTo(KnowledgeKeys.HighCo2));
            Assert.That(entry.AuthorName, Is.EqualTo("Mara"));
            Assert.That(entry.Day, Is.EqualTo(32));
            Assert.That(entry.Text, Does.Contain("Day 32."));
            Assert.That(entry.Text, Does.Contain("My head is pounding"));
            Assert.That(entry.Text, Does.Contain("open the vents"));

            // UI book received the entry and notification ping (acceptance).
            Assert.That(book.EntryCount, Is.EqualTo(1));
            Assert.That(book.NotificationPing, Is.True);
            Assert.That(book.NotificationPingCount, Is.EqualTo(1));
            Assert.That(book.HasUnread, Is.True);
            Assert.That(book.LatestText, Is.EqualTo(entry.Text));
            Assert.That(book.LatestAuthor, Is.EqualTo("Mara"));
            Assert.That(book.StatusLine, Does.Contain("PING"));
        }

        [Test]
        public void HighCo2_SecondObserve_DoesNotDuplicateEntry()
        {
            var author = new Survivor
            {
                Id = "sv1",
                DisplayName = "Ren",
                RiskBias = RiskBiasTrait.Realist
            };
            var context = new EventContext(author, new Shelter(), new Inventory { Capacity = 4 })
            {
                CurrentDay = 10,
                CarbonMonoxidePpm = 40f
            };

            var journal = new JournalSystem();
            var runner = new EventRunner();

            Assert.That(runner.ObserveDiscoveries(journal, context), Is.EqualTo(1));
            Assert.That(journal.EntryCount, Is.EqualTo(1));
            int pings = journal.NotificationPingCount;

            // Still foul air — knowledge already learned → no new page.
            Assert.That(runner.ObserveDiscoveries(journal, context), Is.EqualTo(0));
            Assert.That(journal.EntryCount, Is.EqualTo(1));
            Assert.That(journal.NotificationPingCount, Is.EqualTo(pings));
        }

        [Test]
        public void HighCo2_TraitVoice_ParanoidDiffersFromCautious()
        {
            string cautious = JournalVoice.ComposeFullText(
                KnowledgeKeys.HighCo2, RiskBiasTrait.Cautious, 32);
            string paranoid = JournalVoice.ComposeFullText(
                KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid, 32);

            Assert.That(cautious, Is.EqualTo(
                "Day 32. My head is pounding. The air feels thick. We need to open the vents, even if the ash gets in."));
            Assert.That(paranoid, Does.Contain("Day 32."));
            Assert.That(paranoid, Does.Contain("poison").Or.Contain("choke").Or.Contain("vice"));
            Assert.That(paranoid, Is.Not.EqualTo(cautious));
        }

        [Test]
        public void JournalSystem_TryDiscover_UnknownKeyStillRecords_WhenKnowledgeAdds()
        {
            // KnowledgeBase accepts any non-empty key; voice falls back for unknown.
            var journal = new JournalSystem();
            var author = new Survivor { Id = "a", DisplayName = "Ash", RiskBias = RiskBiasTrait.Fatalist };
            var entry = journal.TryDiscover("high_co2", author, 1, 8f);
            Assert.That(entry, Is.Not.Null);
            Assert.That(journal.TryDiscover("high_co2", author, 2), Is.Null);
        }

        [Test]
        public void JournalBook_Open_ClearsUnreadAndPing()
        {
            var book = _hud.EnsureJournalBook();
            var entry = new JournalEntry
            {
                Id = "j1",
                Text = "Day 1. Test page.",
                Timestamp = "Day 1",
                AuthorName = "Test",
                KnowledgeKey = KnowledgeKeys.HighCo2,
                Day = 1
            };
            book.Push(entry);
            Assert.That(book.NotificationPing, Is.True);
            Assert.That(book.HasUnread, Is.True);

            book.Open();
            Assert.That(book.IsOpen, Is.True);
            Assert.That(book.NotificationPing, Is.False);
            Assert.That(book.HasUnread, Is.False);
            Assert.That(book.EntryCount, Is.EqualTo(1));
        }

        [Test]
        public void Journal_SaveRestore_PreservesEntriesWithoutRefiringEvents()
        {
            var journal = new JournalSystem();
            var author = new Survivor
            {
                Id = "sv",
                DisplayName = "Kai",
                RiskBias = RiskBiasTrait.Cautious
            };
            journal.TryDiscover(KnowledgeKeys.HighCo2, author, 32, 12f);
            Assert.That(journal.EntryCount, Is.EqualTo(1));

            int liveAdds = 0;
            journal.OnEntryAdded += _ => liveAdds++;

            var snap = journal.CaptureState();
            journal.Clear();
            Assert.That(journal.EntryCount, Is.EqualTo(0));

            journal.RestoreState(snap);
            Assert.That(journal.EntryCount, Is.EqualTo(1));
            Assert.That(journal.Knowledge.Has(KnowledgeKeys.HighCo2), Is.True);
            Assert.That(journal.Entries[0].Text, Does.Contain("My head is pounding"));
            Assert.That(liveAdds, Is.EqualTo(0), "Restore must not re-fire OnEntryAdded");
        }

        [Test]
        public void ObserveDiscoveries_AirQualityThreshold_AlsoTriggersHighCo2()
        {
            var author = new Survivor
            {
                Id = "sv_air",
                DisplayName = "Len",
                RiskBias = RiskBiasTrait.Cautious
            };
            var shelter = new Shelter();
            // No air_filtration module → AirQuality returns 0 → high_co2.
            var context = new EventContext(author, shelter, new Inventory { Capacity = 4 })
            {
                CurrentDay = 5,
                CarbonMonoxidePpm = 0f
            };

            var journal = new JournalSystem();
            var runner = new EventRunner();
            Assert.That(runner.ObserveDiscoveries(journal, context), Is.EqualTo(1));
            Assert.That(journal.Knowledge.Has(KnowledgeKeys.HighCo2), Is.True);
        }
    }
}
