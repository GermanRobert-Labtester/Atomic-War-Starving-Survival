using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// H11 hardening: JournalSystem previously had zero tests. These cover the
    /// dedup contract, max-entry eviction, codex unlocks, unread/ping state,
    /// and a lossless CaptureState/RestoreState round-trip (Invariant 3).
    /// </summary>
    public class JournalSystemTests
    {
        private sealed class Author : ISurvivorAuthor
        {
            public Author(string id, string name = null, RiskBiasTrait bias = RiskBiasTrait.Realist)
            {
                Id = id;
                DisplayName = name ?? id;
                RiskBias = bias;
            }
            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }
        }

        private static readonly Author TestAuthor = new Author("sv_jane", "Jane", RiskBiasTrait.Reckless);

        [Fact]
        public void TryDiscover_DeduplicatesPerKnowledgeKey()
        {
            var j = new JournalSystem();
            var first = j.TryDiscover("k_found_radio", TestAuthor, 3);
            var second = j.TryDiscover("k_found_radio", TestAuthor, 4);

            Assert.NotNull(first);
            Assert.Null(second);                 // once per key
            Assert.Equal(1, j.EntryCount);
            Assert.True(j.HasUnread);
            Assert.True(j.NotificationPing);
            Assert.Equal(1, j.NotificationPingCount);
        }

        [Fact]
        public void TryDiscover_RejectsEmptyKey()
        {
            var j = new JournalSystem();
            Assert.Null(j.TryDiscover("", TestAuthor, 3));
            Assert.Null(j.TryDiscover(null, TestAuthor, 3));
            Assert.Equal(0, j.EntryCount);
        }

        [Fact]
        public void TryAddRawEntry_RecordsFreeformText_OncePerKey()
        {
            var j = new JournalSystem();
            var e1 = j.TryAddRawEntry("k_diary_ghost", "We found the station.", TestAuthor, 5, 7.5f);
            var e2 = j.TryAddRawEntry("k_diary_ghost", "Different text.", TestAuthor, 6);

            Assert.NotNull(e1);
            Assert.Null(e2);                     // deduped
            Assert.Equal(1, j.EntryCount);
            Assert.Contains("station", j.LatestText);
            Assert.Equal("Jane", j.Entries[0].AuthorName);
            Assert.Equal("sv_jane", j.Entries[0].AuthorId);
        }

        [Fact]
        public void MaxEntries_EvictsOldest()
        {
            var j = new JournalSystem();
            // 65 distinct keys beyond 64 cap.
            for (int i = 0; i < JournalSystem.MaxEntries + 1; i++)
                j.TryDiscover("k_log_" + i, TestAuthor, 1);

            Assert.Equal(JournalSystem.MaxEntries, j.EntryCount);
            // Newest inserted at index 0; oldest (k_log_0) evicted.
            Assert.Equal("k_log_" + JournalSystem.MaxEntries, j.Entries[0].KnowledgeKey);
            Assert.DoesNotContain(j.Entries, e => e.KnowledgeKey == "k_log_0");
        }

        [Fact]
        public void CodexUnlock_RecordsAndFlags()
        {
            var j = new JournalSystem();
            Assert.False(j.IsItemSeen("item_uplink"));
            Assert.True(j.UnlockItemSeen("item_uplink"));
            Assert.True(j.IsItemSeen("item_uplink"));
            Assert.False(j.UnlockItemSeen("item_uplink"));   // already unlocked
            Assert.Equal(1, j.CodexUnlockCount);
        }

        [Fact]
        public void MarkReadAndAcknowledgePing_ClearFlags()
        {
            var j = new JournalSystem();
            j.TryDiscover("k_ping", TestAuthor, 1);
            Assert.True(j.HasUnread);
            Assert.True(j.NotificationPing);

            j.AcknowledgePing();
            Assert.False(j.NotificationPing);
            Assert.True(j.HasUnread);             // acknowledge only clears ping

            j.MarkRead();
            Assert.False(j.HasUnread);
        }

        [Fact]
        public void CaptureRestore_RoundTrips_EntriesKnowledgeAndFlags()
        {
            var j = new JournalSystem();
            j.TryDiscover("k_vault", TestAuthor, 3, 4f);
            j.TryAddRawEntry("k_letter", "Signed under lantern light.", TestAuthor, 8);
            j.UnlockLocationVisited("loc_quartz_office");
            j.MarkRead();                          // persist has-unread=false
            Assert.Equal(2, j.EntryCount);

            var restored = new JournalSystem();
            restored.RestoreState(j.CaptureState());

            Assert.Equal(2, restored.EntryCount);
            Assert.Equal("k_letter", restored.Entries[0].KnowledgeKey);   // newest first
            Assert.Equal(8, restored.Entries[0].Day);
            Assert.Equal("k_vault", restored.Entries[1].KnowledgeKey);
            Assert.Equal(3, restored.Entries[1].Day);
            Assert.True(restored.IsLocationVisited("loc_quartz_office"));
            Assert.Equal(1, restored.CodexUnlockCount);
            Assert.False(restored.HasUnread);
            // Dedup must hold after restore: same key cannot be re-added.
            Assert.Null(restored.TryDiscover("k_vault", TestAuthor, 9));
            Assert.Equal(2, restored.EntryCount);
        }

        [Fact]
        public void Clear_ResetsEverything()
        {
            var j = new JournalSystem();
            j.TryDiscover("k_reset", TestAuthor, 2);
            j.UnlockItemSeen("item_cleaver");
            j.Clear();

            Assert.Equal(0, j.EntryCount);
            Assert.Equal(0, j.CodexUnlockCount);
            Assert.False(j.HasUnread);
            Assert.False(j.NotificationPing);
            Assert.Equal(0, j.NotificationPingCount);
            Assert.False(j.IsItemSeen("item_cleaver"));
        }

        [Fact]
        public void RestoreState_HandlesNull()
        {
            var j = new JournalSystem();
            j.RestoreState(null);                  // must not throw
            Assert.Equal(0, j.EntryCount);
        }

        [Fact]
        public void EntryLifecycle_NewestFirstOrdering_AndSequenceId()
        {
            var j = new JournalSystem();
            var e1 = j.TryDiscover("k_first", TestAuthor, 1, 8f);
            var e2 = j.TryDiscover("k_second", TestAuthor, 2, 14f);

            Assert.NotNull(e1);
            Assert.NotNull(e2);
            Assert.Equal(2, j.EntryCount);
            // Newest-first: index 0 is second entry
            Assert.Equal("k_second", j.Entries[0].KnowledgeKey);
            Assert.Equal("journal_2_k_second", j.Entries[0].Id);
            Assert.Equal("k_first", j.Entries[1].KnowledgeKey);
            Assert.Equal("journal_1_k_first", j.Entries[1].Id);
            Assert.Equal(2, j.Entries[0].Day);
            Assert.Equal(1, j.Entries[1].Day);
        }

        [Fact]
        public void TryDiscoverKnowledge_DualContract_EntryAndCodexUnlock()
        {
            var j = new JournalSystem();
            int codexEvents = 0;
            string lastCodexKey = null;
            j.OnCodexUnlocked += k =>
            {
                codexEvents++;
                lastCodexKey = k;
            };

            var entry = j.TryDiscoverKnowledge("k_dual_ruin", TestAuthor, 5, 12f);
            Assert.NotNull(entry);
            Assert.Equal(1, j.EntryCount);
            Assert.Equal(1, j.CodexUnlockCount);
            Assert.Equal(1, codexEvents);
            Assert.Equal("k_dual_ruin", lastCodexKey);

            // Repeat attempt must return null, add no entry, and not fire event
            var repeat = j.TryDiscoverKnowledge("k_dual_ruin", TestAuthor, 6);
            Assert.Null(repeat);
            Assert.Equal(1, j.EntryCount);
            Assert.Equal(1, j.CodexUnlockCount);
            Assert.Equal(1, codexEvents);
        }

        [Fact]
        public void AddKnowledgeEvidence_Vs_TryDiscoverKnowledge_Interaction()
        {
            var j = new JournalSystem();
            // AddKnowledgeEvidence unlocks codex only; no journal log entry is created
            bool unlocked = j.AddKnowledgeEvidence("sv_jane", "k_evidence_chem");
            Assert.True(unlocked);
            Assert.Equal(1, j.CodexUnlockCount);
            Assert.Equal(0, j.EntryCount);

            // Calling TryDiscoverKnowledge with the same key must return null because knowledge is already learned
            var entry = j.TryDiscoverKnowledge("k_evidence_chem", TestAuthor, 3);
            Assert.Null(entry);
            Assert.Equal(0, j.EntryCount);
            Assert.Equal(1, j.CodexUnlockCount);
        }

        [Fact]
        public void TryAddRawEntry_EdgeCases_NullAuthor_ClampedDay_FormattedHour()
        {
            var j = new JournalSystem();
            // Null author, negative day, hour specified
            var entry = j.TryAddRawEntry("k_anonymous", "A voice on the wire.", null!, day: -5, hour: 9.5f);

            Assert.NotNull(entry);
            Assert.Equal("Unknown", entry.AuthorName);
            Assert.Equal(string.Empty, entry.AuthorId);
            Assert.Equal(1, entry.Day); // Clamped to 1
            Assert.Equal("Day 1, 09h", entry.Timestamp);

            // Empty/whitespace text rejected
            Assert.Null(j.TryAddRawEntry("k_blank", "", TestAuthor, 1));
            Assert.Null(j.TryAddRawEntry("k_null", null!, TestAuthor, 1));
        }

        [Fact]
        public void MaxEntries_WithRecyclerAndFactory_RecyclesEvictedAndClears()
        {
            var j = new JournalSystem();
            var created = new System.Collections.Generic.List<JournalEntry>();
            var recycled = new System.Collections.Generic.List<JournalEntry>();

            j.SetEntryFactory(
                () =>
                {
                    var e = new JournalEntry();
                    created.Add(e);
                    return e;
                },
                e => recycled.Add(e));

            // Add 65 distinct entries
            for (int i = 0; i < JournalSystem.MaxEntries + 1; i++)
            {
                j.TryDiscover($"k_recycle_{i}", TestAuthor, i + 1);
            }

            Assert.Equal(JournalSystem.MaxEntries + 1, created.Count);
            Assert.Single(recycled); // 1 evicted entry passed to recycler
            Assert.Equal("k_recycle_0", recycled[0].KnowledgeKey); // Oldest was recycled

            // Clear should recycle all remaining 64 entries
            j.Clear();
            Assert.Equal(JournalSystem.MaxEntries + 1, recycled.Count);
            Assert.Equal(0, j.EntryCount);
        }

        [Fact]
        public void Tabs_Switching_Clamping_AndLastSeenTracking()
        {
            var j = new JournalSystem();
            int tabChanges = 0;
            int lastChangedTab = -1;
            j.OnTabChanged += t =>
            {
                tabChanges++;
                lastChangedTab = t;
            };

            // Initial state
            Assert.Equal(0, j.ActiveTab);
            Assert.Equal(-1, j.GetLastSeenIndex(0));
            Assert.Equal(-1, j.GetLastSeenIndex(99)); // Invalid returns -1

            // Switch tab with clamping
            j.SwitchTab(-5); // Already 0, no change
            Assert.Equal(0, tabChanges);

            j.SwitchTab(3);
            Assert.Equal(3, j.ActiveTab);
            Assert.Equal(1, tabChanges);
            Assert.Equal(3, lastChangedTab);

            j.SwitchTab(99); // Clamps to TabCount - 1 (4)
            Assert.Equal(JournalSystem.TabCount - 1, j.ActiveTab);
            Assert.Equal(2, tabChanges);

            // Test unread tracking across tabs
            j.MarkTabViewed(2); // Viewed tab 2 with 0 entries, 0 codex
            Assert.False(j.HasUnreadForTab(2));

            // Add entry and codex unlock
            j.TryDiscover("k_tab_test", TestAuthor, 1);
            j.UnlockItemSeen("item_battery");

            Assert.True(j.HasUnreadForTab(2));
            j.MarkTabViewed(2);
            Assert.False(j.HasUnreadForTab(2));

            // MarkTabViewed(0) clears global unread & ping
            j.MarkTabViewed(0);
            Assert.False(j.HasUnread);
            Assert.False(j.NotificationPing);
        }

        [Fact]
        public void RestoreState_SuppressesAllEvents()
        {
            var initial = new JournalSystem();
            initial.TryDiscover("k_event_1", TestAuthor, 1);
            initial.TryDiscoverKnowledge("k_event_2", TestAuthor, 2);
            initial.UnlockLocationVisited("loc_depot");
            initial.SwitchTab(3);

            var save = initial.CaptureState();

            var restored = new JournalSystem();
            int entriesAdded = 0;
            int pings = 0;
            int tabChanges = 0;
            int codexUnlocks = 0;

            restored.OnEntryAdded += _ => entriesAdded++;
            restored.OnNotificationPing += _ => pings++;
            restored.OnTabChanged += _ => tabChanges++;
            restored.OnCodexUnlocked += _ => codexUnlocks++;

            restored.RestoreState(save);

            // Invariant: RestoreState must NEVER emit mutation events
            Assert.Equal(0, entriesAdded);
            Assert.Equal(0, pings);
            Assert.Equal(0, tabChanges);
            Assert.Equal(0, codexUnlocks);

            // But state is correctly restored
            Assert.Equal(2, restored.EntryCount);
            Assert.Equal(2, restored.CodexUnlockCount);
            Assert.Equal(3, restored.ActiveTab);
        }

        [Fact]
        public void DeterministicOrdering_WithoutWallClock()
        {
            var j1 = new JournalSystem();
            var j2 = new JournalSystem();

            j1.TryAddRawEntry("k_clock_1", "Text 1", TestAuthor, day: 4, hour: 15.2f);
            j1.TryAddRawEntry("k_clock_2", "Text 2", TestAuthor, day: 5, hour: 2.0f);

            j2.TryAddRawEntry("k_clock_1", "Text 1", TestAuthor, day: 4, hour: 15.2f);
            j2.TryAddRawEntry("k_clock_2", "Text 2", TestAuthor, day: 5, hour: 2.0f);

            Assert.Equal(j1.Entries[0].Timestamp, j2.Entries[0].Timestamp);
            Assert.Equal("Day 5, 02h", j1.Entries[0].Timestamp);
            Assert.Equal(j1.Entries[1].Timestamp, j2.Entries[1].Timestamp);
            Assert.Equal("Day 4, 15h", j1.Entries[1].Timestamp);
            Assert.Equal(j1.Entries[0].Id, j2.Entries[0].Id);
            Assert.Equal(j1.Entries[1].Id, j2.Entries[1].Id);
        }
    }
}
