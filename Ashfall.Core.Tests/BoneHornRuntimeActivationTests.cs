// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BoneHornRuntimeActivationTests
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

        private static BoneHornCarvingCatalog LoadSourceCatalog()
        {
            return BoneHornCarvingCatalog.LoadFromDirectory(
                Path.Combine(FindDataDir(), "narrative"));
        }

        private static NarrativeDiscoveryCatalog LoadDiscoveryCatalog()
        {
            var catalog = new NarrativeDiscoveryCatalog();
            catalog.LoadFromFiles(FindDataDir(), new FileSystemIO());
            return catalog;
        }

        [Fact]
        public void SourceCatalog_RetainsExactEightEightSevenSevenCorpus()
        {
            var catalog = LoadSourceCatalog();

            Assert.Equal(8, catalog.DegreasingLogs.Count);
            Assert.Equal(8, catalog.SawingRecords.Count);
            Assert.Equal(7, catalog.PolishingReports.Count);
            Assert.Equal(7, catalog.ToolAssays.Count);
            Assert.Equal(30, catalog.TotalCount);

            var ids = catalog.DegreasingLogs.Select(e => e.Id)
                .Concat(catalog.SawingRecords.Select(e => e.Id))
                .Concat(catalog.PolishingReports.Select(e => e.Id))
                .Concat(catalog.ToolAssays.Select(e => e.Id))
                .ToList();
            Assert.Equal(30, ids.Distinct(StringComparer.Ordinal).Count());
            Assert.All(catalog.DegreasingLogs, e => Assert.True(e.PrepDurationDays > 0));
            Assert.All(catalog.ToolAssays, e => Assert.True(e.PointAngleDegrees > 0));
        }

        [Fact]
        public void Projection_PreservesFourFamiliesAndLabelsMeasurementsAsAuthored()
        {
            var catalog = LoadDiscoveryCatalog();
            var sourceCatalog = LoadSourceCatalog();
            var records = catalog.AllRecords
                .Where(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            Assert.Equal(30, records.Count);
            Assert.Equal(8, records.Count(r => r.SourceCatalog == BoneHornRuntimeContract.DegreasingCatalog));
            Assert.Equal(8, records.Count(r => r.SourceCatalog == BoneHornRuntimeContract.SawingCatalog));
            Assert.Equal(7, records.Count(r => r.SourceCatalog == BoneHornRuntimeContract.PolishingCatalog));
            Assert.Equal(7, records.Count(r => r.SourceCatalog == BoneHornRuntimeContract.ToolAssayCatalog));
            Assert.Equal(30, records.Select(r => r.DiscoveryId).Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(30, records.Select(r => $"{r.SourceCatalog}:{r.SourceRecordId}")
                .Distinct(StringComparer.Ordinal).Count());

            var sourceIds = sourceCatalog.DegreasingLogs.Select(e => e.Id)
                .Concat(sourceCatalog.SawingRecords.Select(e => e.Id))
                .Concat(sourceCatalog.PolishingReports.Select(e => e.Id))
                .Concat(sourceCatalog.ToolAssays.Select(e => e.Id))
                .ToHashSet(StringComparer.Ordinal);
            var projectedSourceIds = records.Select(r => r.SourceRecordId)
                .ToHashSet(StringComparer.Ordinal);
            Assert.True(sourceIds.SetEquals(projectedSourceIds), "every source record must have exactly one projection");

            Assert.All(records, record =>
            {
                Assert.False(string.IsNullOrWhiteSpace(record.RecordFamily));
                Assert.False(string.IsNullOrWhiteSpace(record.FacilityOrStationLabel));
                Assert.False(string.IsNullOrWhiteSpace(record.TechnicalSummary));
                Assert.Equal("Historical Observation", record.TruthClass);
                Assert.Contains("not a live", record.NumericClaimLabel, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("Authored", record.TechnicalSummary, StringComparison.Ordinal);
                Assert.NotEmpty(record.ProducerIds);
                Assert.Empty(record.RelatedDiscoveryIds);
            });

            var dogRecord = records.Single(r => r.SourceRecordId == "bone_degreasing_001");
            Assert.Contains("dog", dogRecord.TechnicalSummary, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("no live entity inferred", dogRecord.IdentityStatus, StringComparison.OrdinalIgnoreCase);

            var shedRecord = records.Single(r => r.SourceRecordId == "antler_horn_001");
            Assert.Contains("old shed", shedRecord.BodyText, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("no live entity inferred", shedRecord.IdentityStatus, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ProducerMap_UsesOnlyExistingShelterRoomsAndLocations()
        {
            var catalog = LoadDiscoveryCatalog();
            var knownProducers = new HashSet<string>(StringComparer.Ordinal)
            {
                "room_workshop",
                "room_workshop_heavy",
                "room_workshop_precision",
                "room_foundry",
                "loc_automated_abattoir",
                "loc_forestry_compound",
                "loc_st_brigids_almshouse",
                "government_bunker"
            };
            var records = catalog.AllRecords
                .Where(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            foreach (var producer in records.SelectMany(r => r.ProducerIds))
                Assert.Contains(producer, knownProducers);

            string[] requiredContexts =
            {
                "room_workshop",
                "room_workshop_heavy",
                "room_workshop_precision",
                "loc_automated_abattoir",
                "loc_forestry_compound",
                "loc_st_brigids_almshouse",
                "government_bunker"
            };
            foreach (var producer in requiredContexts)
            {
                var rows = catalog.GetByProducer(producer)
                    .Where(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                    .ToList();
                Assert.NotEmpty(rows);
                Assert.Equal(rows.Count, rows.Select(r => r.DiscoveryId).Distinct(StringComparer.Ordinal).Count());
            }
        }

        [Fact]
        public void WorkshopVerticalSlice_DiscoversOnceAndRestoresWithoutReplay()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            var workshopRecord = catalog.GetByProducer("room_workshop")
                .First(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog));

            Assert.True(catalog.TryDiscover(workshopRecord.DiscoveryId, journal, out var discovered));
            Assert.NotNull(discovered);
            int unlockCount = journal.CodexUnlockCount;
            var state = journal.CaptureState();

            Assert.False(catalog.TryDiscover(workshopRecord.DiscoveryId, journal, out _));
            Assert.Equal(unlockCount, journal.CodexUnlockCount);

            var restored = new JournalSystem();
            restored.RestoreState(state);
            Assert.True(restored.IsNarrativeDiscovered(workshopRecord.DiscoveryId));
            Assert.False(catalog.TryDiscover(workshopRecord.DiscoveryId, restored, out _));
            Assert.Equal(unlockCount, restored.CodexUnlockCount);
        }

        [Fact]
        public void AnimalAndItemLabelsRemainDisplayOnly()
        {
            var source = LoadSourceCatalog();
            var dataDir = FindDataDir();
            var canonicalItemIds = CollectIds(Path.Combine(dataDir, "items.json"));
            var canonicalSurvivorIds = CollectIds(Path.Combine(dataDir, "survivors.json"));

            Assert.DoesNotContain(source.SawingRecords.Select(e => e.SawToolId), id => canonicalItemIds.Contains(id));
            Assert.DoesNotContain(source.ToolAssays.Select(e => e.BoneBlankId), id => canonicalItemIds.Contains(id));
            Assert.DoesNotContain(new[] { "dog", "rabbit", "rat", "cat", "mixed_bird" },
                id => canonicalSurvivorIds.Contains(id));

            var projections = LoadDiscoveryCatalog().AllRecords
                .Where(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog));
            Assert.All(projections, record =>
            {
                Assert.Contains("not a live item", record.NumericClaimLabel, StringComparison.OrdinalIgnoreCase);
                Assert.Contains("no live entity inferred", record.IdentityStatus, StringComparison.OrdinalIgnoreCase);
            });
        }

        [Fact]
        public void ReloadingProjection_DoesNotDuplicateBoneHornRecords()
        {
            var catalog = LoadDiscoveryCatalog();
            Assert.Equal(243, catalog.Count);
            Assert.Equal(30, catalog.AllRecords.Count(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog)));

            catalog.LoadFromFiles(FindDataDir(), new FileSystemIO());

            Assert.Equal(243, catalog.Count);
            Assert.Equal(30, catalog.AllRecords.Count(r => BoneHornRuntimeContract.IsSourceCatalog(r.SourceCatalog)));
            Assert.Equal(243, catalog.AllRecords.Select(r => r.DiscoveryId)
                .Distinct(StringComparer.Ordinal).Count());
        }

        [Fact]
        public void ContentUtilization_RegistersAllFourSourcesWithReadOnlyConsumers()
        {
            var dataDirectory = new DirectoryInfo(FindDataDir());
            string repoRoot = dataDirectory.Parent!.Parent!.Parent!.FullName;

            var scanner = new ContentUtilizationScanner(
                repoRoot,
                Path.Combine(repoRoot, "Assets", "StreamingAssets", "Data"),
                Path.Combine(repoRoot, "Assets", "Ashfall.Core"),
                Path.Combine(repoRoot, "src"));
            var graph = scanner.Scan();

            foreach (var source in BoneHornRuntimeContract.SourceCatalogs)
            {
                var entry = graph.Catalogs.Single(c => c.Path == source);
                Assert.Equal("BoneHornSourceAdapter", entry.Loader);
                Assert.Contains("BoneHornCarvingCatalog", entry.ConsumerSystems);
                Assert.Contains("NarrativeDiscoveryCatalog", entry.ConsumerSystems);
                Assert.Contains("JournalCodex", entry.ConsumerSystems);
                Assert.Contains(graph.Edges, edge => edge.From == "file:" + source
                    && edge.To == "ui:JournalPanel");
            }
        }

        [Fact]
        public void DuplicateManifestDiscoveryId_IsRejectedWithoutSecondBoneProjection()
        {
            const string entry = """
                {
                  "discovery_id": "disc_bone_duplicate_fixture",
                  "source_catalog": "narrative/bone_degreasing_prep_logs.json",
                  "source_record_id": "bone_degreasing_001",
                  "channel": "location_inspection",
                  "producer_id": "room_workshop",
                  "min_day": 1,
                  "weight": 1,
                  "one_time": true
                }
                """;
            var catalog = new NarrativeDiscoveryCatalog();
            catalog.Load("{\"schema_version\":1,\"entries\":[" + entry + "," + entry + "]}",
                FindDataDir(), new FileSystemIO());

            Assert.Single(catalog.AllRecords);
            Assert.Equal("bone_degreasing_001", catalog.AllRecords[0].SourceRecordId);
        }

        private static HashSet<string> CollectIds(string path)
        {
            using var document = JsonDocument.Parse(File.ReadAllText(path));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            CollectIds(document.RootElement, ids);
            return ids;
        }

        private static void CollectIds(JsonElement element, HashSet<string> ids)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                if (element.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String)
                    ids.Add(id.GetString() ?? string.Empty);
                foreach (var property in element.EnumerateObject())
                    CollectIds(property.Value, ids);
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var child in element.EnumerateArray())
                    CollectIds(child, ids);
            }
        }
    }
}
