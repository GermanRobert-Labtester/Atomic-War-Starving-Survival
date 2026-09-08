using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class NarrativeDiscoverySystemTests
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
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from " + AppContext.BaseDirectory);
        }

        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly NarrativeDiscoveryCatalog _catalog;

        public NarrativeDiscoverySystemTests()
        {
            _dataDir = FindDataDir();
            _fileIO = new FileSystemIO();
            _catalog = new NarrativeDiscoveryCatalog();
            _catalog.LoadFromFiles(_dataDir, _fileIO);
        }

        [Fact]
        public void Manifest_LoadsAll60RecordsAcross18Catalogs()
        {
            Assert.Equal(60, _catalog.Count);
            Assert.Equal(60, _catalog.AllRecords.Count);

            var distinctCatalogs = _catalog.AllRecords.Select(r => r.SourceCatalog).Distinct().ToList();
            Assert.Equal(18, distinctCatalogs.Count);

            foreach (var r in _catalog.AllRecords)
            {
                Assert.False(string.IsNullOrWhiteSpace(r.DiscoveryId), "DiscoveryId must not be empty");
                Assert.StartsWith("disc_", r.DiscoveryId);
                Assert.Equal(KnowledgeKeys.NarrativeDiscovered(r.DiscoveryId), r.KnowledgeKey);
                Assert.False(string.IsNullOrWhiteSpace(r.SourceCatalog), "SourceCatalog must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(r.SourceRecordId), "SourceRecordId must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(r.Channel), "Channel must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(r.ProducerId), "ProducerId must not be empty");
                Assert.True(r.MinDay >= 1, "MinDay must be >= 1");
                Assert.False(string.IsNullOrWhiteSpace(r.Title), "Title must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(r.Category), "Category must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(r.BodyText), "BodyText must not be empty");
            }
        }

        [Fact]
        public void VerticalSlice_PilotCoversAllChannels()
        {
            // 1. location_inspection
            Assert.True(_catalog.TryGetRecord("disc_glitch_radiator_header", out var glitchRec));
            Assert.NotNull(glitchRec);
            Assert.Equal("location_inspection", glitchRec!.Channel);
            Assert.Equal("room_bunker_corridor", glitchRec.ProducerId);
            Assert.Contains("Emergency Protocol", glitchRec.BodyText);

            // 2. shelter_room_archive
            Assert.True(_catalog.TryGetRecord("disc_blueprint_surface_airlock", out var bpRec));
            Assert.NotNull(bpRec);
            Assert.Equal("shelter_room_archive", bpRec!.Channel);
            Assert.Equal("room_airlock", bpRec.ProducerId);
            Assert.Contains("Failure Hazard", bpRec.BodyText);

            // 3. library_terminal
            Assert.True(_catalog.TryGetRecord("disc_court_moonshine_still", out var courtRec));
            Assert.NotNull(courtRec);
            Assert.Equal("library_terminal", courtRec!.Channel);
            Assert.Equal("government_bunker", courtRec.ProducerId);
            Assert.Contains("Verdict", courtRec.BodyText);

            // 4. radio_archive
            Assert.True(_catalog.TryGetRecord("disc_wire_vel_iodine", out var wireRec));
            Assert.NotNull(wireRec);
            Assert.Equal("radio_archive", wireRec!.Channel);
            Assert.Equal("room_radio_tuner", wireRec.ProducerId);
            Assert.False(string.IsNullOrWhiteSpace(wireRec.BodyText));

            // 5. scavenging_document
            Assert.True(_catalog.TryGetRecord("disc_trade_salt_gauze", out var tradeRec));
            Assert.NotNull(tradeRec);
            Assert.Equal("scavenging_document", tradeRec!.Channel);
            Assert.Equal("room_storage_bay", tradeRec.ProducerId);
            Assert.Contains("Barter Exchange", tradeRec.BodyText);

            // 6. quest_aftermath
            Assert.True(_catalog.TryGetRecord("disc_case_radiation_syndrome", out var caseRec));
            Assert.NotNull(caseRec);
            Assert.Equal("quest_aftermath", caseRec!.Channel);
            Assert.Equal("abandoned_hospital", caseRec.ProducerId);
            Assert.Contains("Diagnosis", caseRec.BodyText);

            // 7. item_examination
            Assert.True(_catalog.TryGetRecord("disc_germination_red_fife_wheat", out var seedRec));
            Assert.NotNull(seedRec);
            Assert.Equal("item_examination", seedRec!.Channel);
            Assert.Equal("item_seed_ash_grain", seedRec.ProducerId);
            Assert.Contains("Cultivar", seedRec.Subtitle);
        }

        [Fact]
        public void QueryMethods_FilterByProducerAndChannel()
        {
            // By Producer
            var foundryRecords = _catalog.GetByProducer("room_foundry");
            Assert.Equal(4, foundryRecords.Count);

            var sumpRecords = _catalog.GetByProducer("location_the_sump_cathedral");
            Assert.Equal(3, sumpRecords.Count);

            var govBunkerRecords = _catalog.GetByProducer("government_bunker");
            Assert.Equal(10, govBunkerRecords.Count);

            // By Channel
            var locInspect = _catalog.GetByChannel("location_inspection");
            Assert.Equal(23, locInspect.Count);

            var scavDoc = _catalog.GetByChannel("scavenging_document");
            Assert.Equal(13, scavDoc.Count);

            var libTerm = _catalog.GetByChannel("library_terminal");
            Assert.Equal(10, libTerm.Count);

            var shelterArchive = _catalog.GetByChannel("shelter_room_archive");
            Assert.Equal(4, shelterArchive.Count);

            var questAftermath = _catalog.GetByChannel("quest_aftermath");
            Assert.Equal(4, questAftermath.Count);

            var radioArchive = _catalog.GetByChannel("radio_archive");
            Assert.Equal(3, radioArchive.Count);

            var itemExam = _catalog.GetByChannel("item_examination");
            Assert.Equal(3, itemExam.Count);

            int total = locInspect.Count + scavDoc.Count + libTerm.Count +
                        shelterArchive.Count + questAftermath.Count + radioArchive.Count + itemExam.Count;
            Assert.Equal(60, total);
        }

        [Fact]
        public void TryDiscover_Idempotent_SecondCallReturnsFalse()
        {
            var journal = new JournalSystem();
            string discId = "disc_folklore_clicking_beetle";

            Assert.False(journal.IsNarrativeDiscovered(discId));
            int initialCodexCount = journal.CodexUnlockCount;

            // First discovery
            bool firstResult = _catalog.TryDiscover(discId, journal, out var discovered);
            Assert.True(firstResult);
            Assert.NotNull(discovered);
            Assert.Equal(discId, discovered!.DiscoveryId);
            Assert.True(journal.IsNarrativeDiscovered(discId));
            Assert.True(journal.Knowledge.Has(KnowledgeKeys.NarrativeDiscovered(discId)));
            Assert.Equal(initialCodexCount + 1, journal.CodexUnlockCount);

            // Second discovery attempt (idempotence)
            bool secondResult = _catalog.TryDiscover(discId, journal, out var duplicate);
            Assert.False(secondResult);
            Assert.NotNull(duplicate);
            Assert.Equal(initialCodexCount + 1, journal.CodexUnlockCount);
        }

        [Fact]
        public void SaveRoundtrip_PreservesNarrativeDiscoveries()
        {
            var journal = new JournalSystem();
            string id1 = "disc_wire_sonya_buttons";
            string id2 = "disc_glass_melt_cobalt_blue_smalt";

            Assert.True(_catalog.TryDiscover(id1, journal, out _));
            Assert.True(_catalog.TryDiscover(id2, journal, out _));

            var save = journal.CaptureState();
            Assert.NotNull(save);
            Assert.NotNull(save.Knowledge);
            Assert.Contains(KnowledgeKeys.NarrativeDiscovered(id1), save.Knowledge.DiscoveredKeys);
            Assert.Contains(KnowledgeKeys.NarrativeDiscovered(id2), save.Knowledge.DiscoveredKeys);

            // Restore into a fresh JournalSystem
            var freshJournal = new JournalSystem();
            freshJournal.RestoreState(save);

            Assert.True(freshJournal.IsNarrativeDiscovered(id1));
            Assert.True(freshJournal.IsNarrativeDiscovered(id2));
            Assert.False(freshJournal.IsNarrativeDiscovered("disc_well_tritium_spike"));

            // Re-attempt discovery on restored journal -> must be false
            Assert.False(_catalog.TryDiscover(id1, freshJournal, out _));
        }

        [Fact]
        public void OldSaveCompatibility_LoadsSafelyWithZeroRetroactiveDiscoveries()
        {
            // Old save from earlier game versions with no narrative discoveries
            var oldSave = new JournalSave
            {
                ActiveTab = 0,
                Knowledge = new KnowledgeBaseSave
                {
                    DiscoveredKeys = new string[]
                    {
                        KnowledgeKeys.HighCo2,
                        KnowledgeKeys.ItemSeen("item_clean_water")
                    }
                }
            };

            var journal = new JournalSystem();
            journal.RestoreState(oldSave);

            foreach (var r in _catalog.AllRecords)
            {
                Assert.False(journal.IsNarrativeDiscovered(r.DiscoveryId));
            }

            // New discovery succeeds on restored old save
            Assert.True(_catalog.TryDiscover("disc_dispatch_salt_runner", journal, out var rec));
            Assert.NotNull(rec);
            Assert.True(journal.IsNarrativeDiscovered("disc_dispatch_salt_runner"));
        }

        [Fact]
        public void NegativeTests_InvalidOrMissingRecordsFailGracefully()
        {
            var journal = new JournalSystem();

            Assert.False(_catalog.TryGetRecord("disc_nonexistent_xyz", out var rec));
            Assert.Null(rec);

            Assert.False(_catalog.TryDiscover("disc_nonexistent_xyz", journal, out var rec2));
            Assert.Null(rec2);

            Assert.False(_catalog.TryDiscover("", journal, out _));
            Assert.False(_catalog.TryDiscover(null!, journal, out _));
            Assert.False(_catalog.TryDiscover("disc_wire_vel_iodine", null!, out _));

            var emptyProducer = _catalog.GetByProducer("nonexistent_producer_node");
            Assert.Empty(emptyProducer);

            var emptyChannel = _catalog.GetByChannel("nonexistent_channel_node");
            Assert.Empty(emptyChannel);
        }
    }
}
