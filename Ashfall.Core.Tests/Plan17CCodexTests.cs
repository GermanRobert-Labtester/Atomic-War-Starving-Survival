// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Plan 17C — Codex and journal knowledge tests.
// Validates JournalSystem tab count, KnowledgeBase discovery contract,
// ordinal-sorted keys for checksum stability, and JournalSystem dedup.

using System;
using System.Linq;
using Xunit;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Tests
{
    public class Plan17CCodexTests
    {
        private sealed class TestAuthor : ISurvivorAuthor
        {
            public TestAuthor(string id = "sv_test", string name = "Tester",
                RiskBiasTrait bias = RiskBiasTrait.Realist)
            {
                Id = id;
                DisplayName = name;
                RiskBias = bias;
            }
            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }
        }

        private static readonly TestAuthor Author = new TestAuthor();

        // -----------------------------------------------------------------
        // JournalSystem tab count
        // -----------------------------------------------------------------

        [Fact]
        public void JournalSystem_HasFiveTabs()
        {
            Assert.Equal(5, JournalSystem.TabCount);
        }

        [Fact]
        public void JournalSystem_ActiveTabDefaultsToZero()
        {
            var j = new JournalSystem();
            Assert.Equal(0, j.ActiveTab);
        }

        [Fact]
        public void JournalSystem_SwitchTab_ClampsToValidRange()
        {
            var j = new JournalSystem();
            j.SwitchTab(3);
            Assert.Equal(3, j.ActiveTab);
        }

        // -----------------------------------------------------------------
        // KnowledgeBase — Discover returns true only on first discovery
        // -----------------------------------------------------------------

        [Fact]
        public void KnowledgeBase_Discover_ReturnsTrueOnlyFirstTime()
        {
            var kb = new KnowledgeBase();
            Assert.True(kb.Discover("k_test_key"));
            Assert.False(kb.Discover("k_test_key"));   // second call → already known
            Assert.False(kb.Discover("k_test_key"));   // third call → still false
        }

        [Fact]
        public void KnowledgeBase_Discover_RejectsNullOrEmpty()
        {
            var kb = new KnowledgeBase();
            Assert.False(kb.Discover(null));
            Assert.False(kb.Discover(""));
            Assert.Equal(0, kb.Count);
        }

        // -----------------------------------------------------------------
        // KnowledgeBase — Has returns correct state
        // -----------------------------------------------------------------

        [Fact]
        public void KnowledgeBase_Has_ReflectsDiscoverState()
        {
            var kb = new KnowledgeBase();
            Assert.False(kb.Has("k_alpha"));

            kb.Discover("k_alpha");
            Assert.True(kb.Has("k_alpha"));
            Assert.False(kb.Has("k_beta"));

            kb.Discover("k_beta");
            Assert.True(kb.Has("k_alpha"));
            Assert.True(kb.Has("k_beta"));
        }

        [Fact]
        public void KnowledgeBase_Has_ReturnsFalseForNullOrEmpty()
        {
            var kb = new KnowledgeBase();
            kb.Discover("k_real");
            Assert.False(kb.Has(null));
            Assert.False(kb.Has(""));
        }

        // -----------------------------------------------------------------
        // KnowledgeBase — ordinal-sorted keys for checksum stability
        // -----------------------------------------------------------------

        [Fact]
        public void KnowledgeBase_CaptureState_KeysAreOrdinalSorted()
        {
            var kb = new KnowledgeBase();
            // Insert in non-alphabetical order
            kb.Discover("k_zulu");
            kb.Discover("k_alpha");
            kb.Discover("k_mike");

            var save = kb.CaptureState();
            Assert.NotNull(save);
            Assert.Equal(3, save.DiscoveredKeys.Length);

            // Verify ordinal sort order
            for (int i = 1; i < save.DiscoveredKeys.Length; i++)
            {
                Assert.True(
                    StringComparer.Ordinal.Compare(save.DiscoveredKeys[i - 1], save.DiscoveredKeys[i]) <= 0,
                    $"Keys not ordinal-sorted: '{save.DiscoveredKeys[i - 1]}' > '{save.DiscoveredKeys[i]}'");
            }
        }

        [Fact]
        public void KnowledgeBase_CaptureRestore_RoundTrips()
        {
            var kb = new KnowledgeBase();
            kb.Discover("k_first");
            kb.Discover("k_second");
            kb.Discover("k_third");

            var save = kb.CaptureState();
            var kb2 = new KnowledgeBase();
            kb2.RestoreState(save);

            Assert.True(kb2.Has("k_first"));
            Assert.True(kb2.Has("k_second"));
            Assert.True(kb2.Has("k_third"));
            Assert.Equal(3, kb2.Count);
        }

        // -----------------------------------------------------------------
        // JournalSystem.TryDiscover — creates entry with correct knowledge key
        // -----------------------------------------------------------------

        [Fact]
        public void JournalSystem_TryDiscover_CreatesEntryWithCorrectKey()
        {
            var j = new JournalSystem();
            var entry = j.TryDiscover("k_found_artifact", Author, 5);

            Assert.NotNull(entry);
            Assert.Equal("k_found_artifact", entry.KnowledgeKey);
            Assert.Equal(5, entry.Day);
            Assert.Equal(1, j.EntryCount);
        }

        // -----------------------------------------------------------------
        // JournalSystem dedup — same key doesn't create duplicate
        // -----------------------------------------------------------------

        [Fact]
        public void JournalSystem_TryDiscover_DeduplicatesSameKey()
        {
            var j = new JournalSystem();
            var first = j.TryDiscover("k_unique_find", Author, 1);
            var second = j.TryDiscover("k_unique_find", Author, 2);

            Assert.NotNull(first);
            Assert.Null(second);
            Assert.Equal(1, j.EntryCount);
        }

        // -----------------------------------------------------------------
        // JournalSystem save/load round-trip preserves entries
        // -----------------------------------------------------------------

        [Fact]
        public void JournalSystem_SaveLoad_RoundTrip_PreservesCodexEntries()
        {
            var j = new JournalSystem();
            j.TryDiscover("k_codex_alpha", Author, 3);
            j.TryDiscover("k_codex_beta", Author, 7);
            j.UnlockItemSeen("item_ancient_map");
            j.UnlockLocationVisited("loc_forgotten_archive");

            var save = j.CaptureState();
            var ser = new SystemTextJsonSerializer();
            string json = ser.Serialize(save);
            var restored = ser.Deserialize<JournalSave>(json);

            var j2 = new JournalSystem();
            j2.RestoreState(restored);

            Assert.Equal(2, j2.EntryCount);
            Assert.True(j2.IsItemSeen("item_ancient_map"));
            Assert.True(j2.IsLocationVisited("loc_forgotten_archive"));

            // Dedup must hold after restore
            Assert.Null(j2.TryDiscover("k_codex_alpha", Author, 99));
            Assert.Equal(2, j2.EntryCount);
        }
    }
}
