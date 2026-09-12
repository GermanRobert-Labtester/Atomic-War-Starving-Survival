// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PaperPrintingRuntimeActivationTests
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
            throw new DirectoryNotFoundException("Could not locate the data authority directory.");
        }

        private readonly string _dataDir = FindDataDir();

        [Fact]
        public void SourceCatalogs_RetainThirtyRecordsEachAcrossAllEightFamilies()
        {
            string narrativeDir = Path.Combine(_dataDir, "narrative");
            var making = PaperMakingCatalog.LoadFromDirectory(narrativeDir);
            var printing = PaperPrintingCatalog.LoadFromDirectory(narrativeDir);

            Assert.Equal(30, making.TotalCount);
            Assert.Equal(30, printing.TotalCount);
            Assert.Equal(8, making.BeaterEntries.Count);
            Assert.Equal(8, making.MouldEntries.Count);
            Assert.Equal(7, making.PressEntries.Count);
            Assert.Equal(7, making.SizingEntries.Count);
            Assert.Equal(8, printing.PulpEntries.Count);
            Assert.Equal(8, printing.InkEntries.Count);
            Assert.Equal(7, printing.TypeEntries.Count);
            Assert.Equal(7, printing.StencilEntries.Count);

            var ids = making.BeaterEntries.Select(e => e.Id)
                .Concat(making.MouldEntries.Select(e => e.Id))
                .Concat(making.PressEntries.Select(e => e.Id))
                .Concat(making.SizingEntries.Select(e => e.Id))
                .Concat(printing.PulpEntries.Select(e => e.Id))
                .Concat(printing.InkEntries.Select(e => e.Id))
                .Concat(printing.TypeEntries.Select(e => e.Id))
                .Concat(printing.StencilEntries.Select(e => e.Id))
                .ToList();
            Assert.Equal(60, ids.Count);
            Assert.Equal(60, ids.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void CombinedProjection_PreservesSourceProvenanceAndSafeMeasurements()
        {
            var catalog = LoadProjection();
            var records = catalog.AllRecords
                .Where(r => PaperPrintRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            Assert.Equal(60, records.Count);
            Assert.Equal(30, records.Count(r => PaperPrintRuntimeContract.IsPaperMakingCatalog(r.SourceCatalog)));
            Assert.Equal(60, records.Select(r => r.DiscoveryId).Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(60, records.Select(r => $"{r.SourceCatalog}:{r.SourceRecordId}").Distinct(StringComparer.Ordinal).Count());

            foreach (var record in records)
            {
                Assert.False(string.IsNullOrWhiteSpace(record.RecordFamily));
                Assert.False(string.IsNullOrWhiteSpace(record.FacilityOrStationLabel));
                Assert.False(string.IsNullOrWhiteSpace(record.TechnicalSummary));
                Assert.Equal("Historical Observation", record.TruthClass);
                Assert.Contains("not a live production value", record.NumericClaimLabel, StringComparison.Ordinal);
                Assert.Contains("Authored", record.TechnicalSummary, StringComparison.Ordinal);
                Assert.NotEmpty(record.ProducerIds);
            }

            Assert.Contains(
                "disc_paper_printing_rag_pulp_hollander_beater_linen_fibrillation",
                catalog.TryGetRecord("disc_paper_making_hollander_beater_flax_linen_rag_maceration", out var first)
                    ? first!.RelatedDiscoveryIds : Array.Empty<string>());
            Assert.Contains(catalog.GetByProducer("loc_printworks"), r => PaperPrintRuntimeContract.IsSourceCatalog(r.SourceCatalog));
            Assert.Contains(catalog.GetByProducer("room_workshop"), r => PaperPrintRuntimeContract.IsSourceCatalog(r.SourceCatalog));
        }

        [Fact]
        public void ExplicitProducerContexts_CoverFourteenPaperMakingRecordsWithoutDuplicateRows()
        {
            var catalog = LoadProjection();
            var paperMaking = catalog.AllRecords
                .Where(r => PaperPrintRuntimeContract.IsPaperMakingCatalog(r.SourceCatalog))
                .ToList();

            Assert.Equal(14, paperMaking.Count(r => r.ProducerIds.Length > 1));
            Assert.True(
                paperMaking.Count(r => r.ProducerIds.Any(p => p.StartsWith("room_", StringComparison.Ordinal))
                    && r.ProducerIds.Any(p => !p.StartsWith("room_", StringComparison.Ordinal))) >= 12,
                "at least twelve paper-making records must bridge room and location/archive contexts");
            Assert.Equal(243, catalog.AllRecords.Count);

            var knownProducers = new HashSet<string>(StringComparer.Ordinal)
            {
                "room_workshop", "room_workshop_heavy", "room_workshop_precision", "room_foundry",
                "loc_printworks", "loc_municipal_archive", "loc_excavation_archive_bunker", "government_bunker"
            };
            foreach (var producer in paperMaking.SelectMany(r => r.ProducerIds))
                Assert.Contains(producer, knownProducers);
        }

        [Fact]
        public void ReloadingProjection_DoesNotAggregatePreviousRecords()
        {
            var catalog = LoadProjection();
            Assert.Equal(243, catalog.Count);
            catalog.LoadFromFiles(_dataDir, new FileSystemIO());
            Assert.Equal(243, catalog.Count);
            Assert.Equal(60, catalog.AllRecords.Count(r => PaperPrintRuntimeContract.IsSourceCatalog(r.SourceCatalog)));
        }

        [Fact]
        public void Discovery_IsIdempotentAcrossSaveRestoreAndHasNoProductionContract()
        {
            var catalog = LoadProjection();
            var journal = new JournalSystem();
            const string id = "disc_paper_printing_ink_assay_paper_acid_burnthrough_corrosion";

            Assert.True(catalog.TryDiscover(id, journal, out var record));
            Assert.NotNull(record);
            int unlockCount = journal.CodexUnlockCount;
            var state = journal.CaptureState();

            var restored = new JournalSystem();
            restored.RestoreState(state);
            Assert.True(restored.IsNarrativeDiscovered(id));
            Assert.False(catalog.TryDiscover(id, restored, out _));
            Assert.Equal(unlockCount, restored.CodexUnlockCount);
            Assert.Contains("not a live production value", record!.NumericClaimLabel, StringComparison.Ordinal);
        }

        [Fact]
        public void DuplicateManifestDiscoveryId_IsRejectedWithoutSilentSecondProjection()
        {
            const string entry = """
                {
                  "discovery_id": "disc_paper_duplicate_fixture",
                  "source_catalog": "narrative/hollander_beater_pulping_logs.json",
                  "source_record_id": "hollander_beater_flax_linen_rag_maceration",
                  "channel": "location_inspection",
                  "producer_id": "loc_printworks",
                  "min_day": 1,
                  "weight": 1,
                  "one_time": true
                }
                """;
            string manifest = "{\"schema_version\":1,\"entries\":[" + entry + "," + entry + "]}";
            var catalog = new NarrativeDiscoveryCatalog();
            catalog.Load(manifest, _dataDir, new FileSystemIO());

            Assert.Single(catalog.AllRecords);
            Assert.Equal("hollander_beater_flax_linen_rag_maceration", catalog.AllRecords[0].SourceRecordId);
        }

        private NarrativeDiscoveryCatalog LoadProjection()
        {
            var catalog = new NarrativeDiscoveryCatalog();
            catalog.LoadFromFiles(_dataDir, new FileSystemIO());
            return catalog;
        }
    }
}
