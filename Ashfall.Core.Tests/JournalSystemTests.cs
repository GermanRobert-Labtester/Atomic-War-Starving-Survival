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
    }
}
