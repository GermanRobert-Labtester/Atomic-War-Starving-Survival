// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 150 — personal letter runtime activation. Pins source allowlist,
    /// producer-room binding, and journal-only discovery (no inventory side effects).
    /// </summary>
    public sealed class PersonalLetterRuntimeActivationTests
    {
        private static string FindDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL data directory.");
        }

        private static NarrativeDiscoveryCatalog LoadDiscoveryCatalog()
        {
            var catalog = new NarrativeDiscoveryCatalog();
            catalog.LoadFromFiles(FindDataDir(), new FileSystemIO());
            return catalog;
        }

        [Fact]
        public void SourceAllowlist_MatchesLetterCatalogsOnly()
        {
            Assert.True(PersonalLetterRuntimeContract.IsSourceCatalog("narrative/letters_expansion.json"));
            Assert.True(PersonalLetterRuntimeContract.IsSourceCatalog("narrative/unsent_letters_batch_2.json"));
            Assert.False(PersonalLetterRuntimeContract.IsSourceCatalog("narrative/cobalt_liturgies.json"));
            Assert.False(PersonalLetterRuntimeContract.IsSourceCatalog("narrative/hydrophone_acoustic_logs.json"));
        }

        [Fact]
        public void DiscoveryProjection_LoadsTwentySixLetterRecords()
        {
            var catalog = LoadDiscoveryCatalog();
            var records = catalog.AllRecords
                .Where(r => PersonalLetterRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            Assert.Equal(26, records.Count);
            Assert.All(records, r =>
            {
                Assert.False(string.IsNullOrWhiteSpace(r.ProducerId));
                Assert.StartsWith("room_", r.ProducerId);
                Assert.False(string.IsNullOrWhiteSpace(r.Title));
                Assert.Equal("Personal Letters & Unsent Correspondence", r.Category);
            });
        }

        [Fact]
        public void ProducerDiscovery_UnlocksMatchingRoomLetters()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            var bunks = catalog.GetByProducer("room_bunks")
                .Where(r => PersonalLetterRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();
            Assert.True(bunks.Count >= 5);

            Assert.True(catalog.TryDiscover(bunks[0].DiscoveryId, journal, out var unlocked));
            Assert.NotNull(unlocked);
            Assert.True(journal.IsNarrativeDiscovered(bunks[0].DiscoveryId));
        }

        [Fact]
        public void LetterDiscovery_IsIdempotentAcrossSaveRestore()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            var letter = catalog.AllRecords.First(r =>
                PersonalLetterRuntimeContract.IsSourceCatalog(r.SourceCatalog));

            Assert.True(catalog.TryDiscover(letter.DiscoveryId, journal, out _));
            int unlockCount = journal.CodexUnlockCount;
            var save = journal.CaptureState();
            var restored = new JournalSystem();
            restored.RestoreState(save);

            Assert.False(catalog.TryDiscover(letter.DiscoveryId, restored, out _));
            Assert.Equal(unlockCount, restored.CodexUnlockCount);
            Assert.True(restored.IsNarrativeDiscovered(letter.DiscoveryId));
        }
    }
}
