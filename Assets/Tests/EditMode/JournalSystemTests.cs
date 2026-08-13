using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;
using Ashfall.Core.Journal;

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

        // -----------------------------------------------------------------
        // Codex unlocks + tabs (docs/ui/JOURNAL_UI_PLAN.md §5, §7)
        // -----------------------------------------------------------------

        [Test]
        public void CodexUnlock_ItemSeen_FiresOnce_DeduplicatedByKey()
        {
            var journal = new JournalSystem();
            int unlocked = 0;
            string lastKey = null;
            journal.OnCodexUnlocked += key => { unlocked++; lastKey = key; };

            Assert.That(journal.UnlockItemSeen("dosimeter"), Is.True);
            Assert.That(journal.UnlockItemSeen("dosimeter"), Is.False, "Second unlock must be a no-op");
            Assert.That(unlocked, Is.EqualTo(1));
            Assert.That(lastKey, Is.EqualTo("item_seen_dosimeter"));
            Assert.That(journal.IsItemSeen("dosimeter"), Is.True);
            Assert.That(journal.IsItemSeen("geiger_counter"), Is.False);
        }

        [Test]
        public void CodexUnlock_KnowledgeKeyNamespaces_MatchMasterListConventions()
        {
            var journal = new JournalSystem();
            journal.UnlockLocationVisited("loc_grange_hall");
            journal.UnlockSurvivorMet("sv_elena");
            journal.UnlockEventFired("filter_failure");

            Assert.That(journal.Knowledge.Has(KnowledgeKeys.LocationVisited("loc_grange_hall")), Is.True);
            Assert.That(journal.Knowledge.Has(KnowledgeKeys.SurvivorMet("sv_elena")), Is.True);
            Assert.That(journal.Knowledge.Has(KnowledgeKeys.EventFired("filter_failure")), Is.True);
            Assert.That(journal.Knowledge.Has(KnowledgeKeys.ItemSeen("dosimeter")), Is.False);
        }

        [Test]
        public void TabState_SwitchTab_ClampsAndRaisesOnTabChanged()
        {
            var journal = new JournalSystem();
            int changed = 0;
            int lastTab = -1;
            journal.OnTabChanged += tab => { changed++; lastTab = tab; };

            Assert.That(journal.ActiveTab, Is.EqualTo(0));
            journal.SwitchTab(2);
            Assert.That(journal.ActiveTab, Is.EqualTo(2));
            Assert.That(changed, Is.EqualTo(1));
            Assert.That(lastTab, Is.EqualTo(2));

            // Same tab → no event
            journal.SwitchTab(2);
            Assert.That(changed, Is.EqualTo(1));

            // Out of range clamps
            journal.SwitchTab(99);
            Assert.That(journal.ActiveTab, Is.EqualTo(JournalSystem.TabCount - 1));
            journal.SwitchTab(-5);
            Assert.That(journal.ActiveTab, Is.EqualTo(0));
        }

        [Test]
        public void TabState_UnreadPerTab_TracksEntryCountAtLastView()
        {
            var journal = new JournalSystem();
            var author = new Survivor { Id = "sv1", DisplayName = "Ren", RiskBias = RiskBiasTrait.Realist };
            journal.TryDiscover(KnowledgeKeys.HighCo2, author, 1);
            journal.MarkTabViewed(2);
            Assert.That(journal.HasUnreadForTab(2), Is.False);

            journal.TryDiscover(KnowledgeKeys.FilterFailing, author, 2);
            Assert.That(journal.HasUnreadForTab(2), Is.True, "New entry after last view must mark tab unread");
            journal.MarkTabViewed(2);
            Assert.That(journal.HasUnreadForTab(2), Is.False);
        }

        [Test]
        public void JournalSaveRestore_RoundTripsTabsAndCodexUnlocks()
        {
            var journal = new JournalSystem();
            journal.UnlockItemSeen("dosimeter");
            journal.UnlockLocationVisited("loc_grange_hall");
            journal.SwitchTab(3);
            journal.MarkTabViewed(3);
            journal.TryDiscover(KnowledgeKeys.HighCo2,
                new Survivor { Id = "sv", DisplayName = "Kai", RiskBias = RiskBiasTrait.Cautious }, 32);

            var snap = journal.CaptureState();
            journal.Clear();
            journal.RestoreState(snap);

            Assert.That(journal.ActiveTab, Is.EqualTo(3));
            Assert.That(journal.IsItemSeen("dosimeter"), Is.True);
            Assert.That(journal.IsLocationVisited("loc_grange_hall"), Is.True);
            Assert.That(journal.EntryCount, Is.EqualTo(1));
            // MarkTabViewed ran before the entry was added → nothing seen in tab 3 yet.
            Assert.That(journal.GetLastSeenIndex(3), Is.EqualTo(0));
            // Unlock events must not re-fire on restore (knowledge is data, not discovery).
            int reFired = 0;
            journal.OnCodexUnlocked += _ => reFired++;
            journal.RestoreState(snap);
            Assert.That(reFired, Is.EqualTo(0));
        }

        [Test]
        public void JournalBook_TabSwitch_ReflectsSystemAndRendersCodexProvider()
        {
            var book = _hud.EnsureJournalBook();
            book.SetCodexProvider(tab =>
            {
                if (tab == JournalTab.Items)
                {
                    return new List<JournalCodexRow>
                    {
                        new JournalCodexRow { DisplayName = "Dosimeter", Meta = "0.5 kg", Body = "Counts rads.", IsLocked = false },
                        JournalCodexRow.Locked("Geiger Counter")
                    };
                }
                return new List<JournalCodexRow>();
            });

            book.Open();
            Assert.That(book.ActiveTab, Is.EqualTo(0));
            int tabChanged = 0;
            book.OnTabChanged += _ => tabChanged++;

            book.SwitchTab(1);
            Assert.That(book.ActiveTab, Is.EqualTo(1));
            Assert.That(tabChanged, Is.EqualTo(1));
            Assert.That(book.StatusLine, Does.Contain("Items"));
            Assert.That(book.DetailSummary, Does.Contain("Dosimeter"));
            Assert.That(book.DetailSummary, Does.Contain("Counts rads."));
            Assert.That(book.DetailSummary, Does.Contain("[---] Geiger Counter"), "Locked rows must show the silhouette");

            book.SwitchTab(0);
            Assert.That(book.ActiveTab, Is.EqualTo(0));
            Assert.That(book.DetailSummary, Does.Contain("No pages yet"), "Empty log shows the empty-state text");
        }

        // -----------------------------------------------------------------
        // Codex unread counter + dossier shelf (docs/ui/JOURNAL_UI_PLAN.md §8)
        // -----------------------------------------------------------------

        [Test]
        public void CodexUnlock_TracksUnreadPerTab_UntilTabViewed()
        {
            var journal = new JournalSystem();
            journal.SwitchTab(2); // People — viewed, count baseline captured
            Assert.That(journal.HasUnreadForTab(2), Is.False);

            journal.UnlockSurvivorMet("sv_elena");
            Assert.That(journal.CodexUnlockCount, Is.EqualTo(1));
            Assert.That(journal.HasUnreadForTab(2), Is.True, "Unlock after last view must mark the tab unread");

            journal.MarkTabViewed(2);
            Assert.That(journal.HasUnreadForTab(2), Is.False);

            // Log tab still mirrors global entry unread, not codex unlocks.
            journal.UnlockItemSeen("dosimeter");
            Assert.That(journal.HasUnreadForTab(0), Is.False);
        }

        [Test]
        public void JournalSaveRestore_RoundTripsCodexUnlockCount()
        {
            var journal = new JournalSystem();
            journal.UnlockItemSeen("dosimeter");
            journal.UnlockLocationVisited("loc_grange_hall");
            journal.SwitchTab(1);
            var snap = journal.CaptureState();

            journal.Clear();
            Assert.That(journal.CodexUnlockCount, Is.EqualTo(0));
            journal.RestoreState(snap);

            Assert.That(journal.CodexUnlockCount, Is.EqualTo(2));
            Assert.That(journal.IsItemSeen("dosimeter"), Is.True);
            Assert.That(journal.ActiveTab, Is.EqualTo(1));
            Assert.That(journal.HasUnreadForTab(1), Is.False, "Tab was viewed after the unlocks");
        }

        [Test]
        public void JournalCodex_DossierShelf_LockedUntilArchetypeMet()
        {
            var journal = new JournalSystem();
            var catalog = ScriptableObject.CreateInstance<SurvivorCatalogSO>();
            catalog.archetypes = new List<SurvivorArchetypeSO>
            {
                ScriptableObject.CreateInstance<SurvivorArchetypeSO>() // id set below
            };
            catalog.archetypes[0].id = "elena_vasquez";
            catalog.archetypes[0].displayName = "Elena Vasquez";
            catalog.archetypes[0].profession = "Paramedic";
            catalog.archetypes[0].bio = "Hands never shake.";

            var codex = new JournalCodex(
                journal, null, null, null, () => new List<Survivor>(),
                survivorCatalog: catalog);

            var rows = codex.BuildRows(JournalTab.People);
            Assert.That(rows.Count, Is.EqualTo(1));
            Assert.That(rows[0].IsLocked, Is.True, "Unmet archetype must show as locked");
            Assert.That(rows[0].DisplayName, Is.EqualTo("Elena Vasquez"));

            journal.UnlockSurvivorMet("elena_vasquez");
            rows = codex.BuildRows(JournalTab.People);
            Assert.That(rows[0].IsLocked, Is.False);
            Assert.That(rows[0].Body, Is.EqualTo("Hands never shake."));
            Assert.That(rows[0].Meta, Is.EqualTo("Paramedic"));
        }

        [Test]
        public void JournalCodex_ItemRows_LockedUntilSeen()
        {
            var journal = new JournalSystem();
            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = "dosimeter";
            item.displayName = "Dosimeter";
            item.type = ItemType.Device;
            item.weight = 0.5f;
            item.tradeValue = 30f;
            item.description = "Counts the rads you already took.";
            catalog.items = new List<ItemDefinition> { item };

            var codex = new JournalCodex(journal, catalog, null, null, () => new List<Survivor>());

            var rows = codex.BuildRows(JournalTab.Items);
            Assert.That(rows[0].IsLocked, Is.True);

            journal.UnlockItemSeen("dosimeter");
            rows = codex.BuildRows(JournalTab.Items);
            Assert.That(rows[0].IsLocked, Is.False);
            Assert.That(rows[0].Body, Is.EqualTo("Counts the rads you already took."));
            Assert.That(rows[0].Meta, Does.Contain("0.5 kg"));
            Assert.That(rows[0].Meta, Does.Contain("30"));
        }
    }
}
