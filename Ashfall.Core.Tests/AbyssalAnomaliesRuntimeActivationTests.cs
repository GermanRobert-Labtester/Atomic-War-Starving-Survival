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
    /// Plan 151 — abyssal anomalies runtime activation. Pins source allowlist,
    /// activated-vs-deferred filtering, and journal-only discovery.
    /// </summary>
    public sealed class AbyssalAnomaliesRuntimeActivationTests
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
        public void SourceAllowlist_MatchesAbyssalCatalogsOnly()
        {
            Assert.True(AbyssalAnomaliesRuntimeContract.IsSourceCatalog("narrative/hydrophone_acoustic_logs.json"));
            Assert.True(AbyssalAnomaliesRuntimeContract.IsSourceCatalog("narrative/geothermal_borehole_logs.json"));
            Assert.True(AbyssalAnomaliesRuntimeContract.IsSourceCatalog("narrative/cryopod_failure_logs.json"));
            Assert.True(AbyssalAnomaliesRuntimeContract.IsSourceCatalog("narrative/salt_mine_inscriptions.json"));
            Assert.False(AbyssalAnomaliesRuntimeContract.IsSourceCatalog("narrative/letters_expansion.json"));
            Assert.False(AbyssalAnomaliesRuntimeContract.IsSourceCatalog("narrative/cobalt_liturgies.json"));
        }

        [Fact]
        public void DiscoveryProjection_LoadsSeventeenAbyssalRecords()
        {
            var catalog = LoadDiscoveryCatalog();
            var records = catalog.AllRecords
                .Where(r => AbyssalAnomaliesRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            Assert.Equal(17, records.Count);
            Assert.Contains(records, r => r.ProducerId == "location_the_sump_cathedral");
            Assert.Contains(records, r => r.ProducerId == "location_geo_thermal_plant_ruins");
        }

        [Fact]
        public void Projection_KeepsDeferredPhaseTwoRowsLocked()
        {
            var metadata = AbyssalAnomaliesProjection.GetAllMetadata();
            int activated = metadata.Count(m => m.IsActivated);
            int deferred = metadata.Count(m => !m.IsActivated);

            Assert.Equal(17, activated);
            Assert.True(deferred >= 1);
            Assert.Equal(metadata.Count, activated + deferred);
            Assert.False(AbyssalAnomaliesRuntimeContract.IsActivatedSourceRecord(
                metadata.First(m => !m.IsActivated).RecordId));
        }

        [Fact]
        public void ManifestRows_AreActivatedOnly()
        {
            var catalog = LoadDiscoveryCatalog();
            var records = catalog.AllRecords
                .Where(r => AbyssalAnomaliesRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            Assert.All(records, r =>
                Assert.True(AbyssalAnomaliesRuntimeContract.IsActivatedSourceRecord(r.SourceRecordId)));
        }

        [Fact]
        public void ProducerDiscovery_UnlocksActivatedRows()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            const string producer = "location_geo_thermal_plant_ruins";
            var candidates = catalog.GetByProducer(producer)
                .Where(r => AbyssalAnomaliesRuntimeContract.IsSourceCatalog(r.SourceCatalog)
                    && AbyssalAnomaliesRuntimeContract.IsActivatedSourceRecord(r.SourceRecordId))
                .ToList();
            Assert.True(candidates.Count >= 1);

            Assert.True(catalog.TryDiscover(candidates[0].DiscoveryId, journal, out var unlocked));
            Assert.NotNull(unlocked);
            Assert.True(journal.IsNarrativeDiscovered(candidates[0].DiscoveryId));
        }

        [Fact]
        public void AbyssalDiscovery_IsIdempotentAcrossSaveRestore()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            var record = catalog.AllRecords.First(r =>
                AbyssalAnomaliesRuntimeContract.IsSourceCatalog(r.SourceCatalog)
                && AbyssalAnomaliesRuntimeContract.IsActivatedSourceRecord(r.SourceRecordId));

            Assert.True(catalog.TryDiscover(record.DiscoveryId, journal, out _));
            int unlockCount = journal.CodexUnlockCount;
            var save = journal.CaptureState();
            var restored = new JournalSystem();
            restored.RestoreState(save);

            Assert.False(catalog.TryDiscover(record.DiscoveryId, restored, out _));
            Assert.Equal(unlockCount, restored.CodexUnlockCount);
            Assert.True(restored.IsNarrativeDiscovered(record.DiscoveryId));
        }
    }
}
