using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class FringeCultRuntimeActivationTests
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
        public void FringeCultsCatalog_LoadsThirtyRecordsAcrossFourFamilies()
        {
            var catalog = FringeCultsCatalog.LoadFromDirectory(
                Path.Combine(FindDataDir(), "narrative"));

            Assert.Equal(30, catalog.TotalCount);
            Assert.Equal(8, catalog.CobaltLiturgies.Count);
            Assert.Equal(8, catalog.IronSynodCanons.Count);
            Assert.Equal(7, catalog.GeophoneHymnals.Count);
            Assert.Equal(7, catalog.WastelandEpitaphs.Count);

            var ids = catalog.CobaltLiturgies.Select(e => e.Id)
                .Concat(catalog.IronSynodCanons.Select(e => e.Id))
                .Concat(catalog.GeophoneHymnals.Select(e => e.Id))
                .Concat(catalog.WastelandEpitaphs.Select(e => e.Id))
                .ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }

        [Fact]
        public void DiscoveryProjection_PreservesSafeTruthLabelsAndGroupIdentity()
        {
            var catalog = LoadDiscoveryCatalog();
            var records = catalog.AllRecords
                .Where(r => FringeCultRuntimeContract.IsSourceCatalog(r.SourceCatalog))
                .ToList();

            Assert.Equal(30, records.Count);
            Assert.Equal(8, records.Count(r => r.SourceCatalog == FringeCultRuntimeContract.CobaltCatalog));
            Assert.Equal(8, records.Count(r => r.SourceCatalog == FringeCultRuntimeContract.IronCatalog));
            Assert.Equal(7, records.Count(r => r.SourceCatalog == FringeCultRuntimeContract.HymnalCatalog));
            Assert.Equal(7, records.Count(r => r.SourceCatalog == FringeCultRuntimeContract.EpitaphCatalog));

            Assert.All(records, record =>
            {
                Assert.False(string.IsNullOrWhiteSpace(record.TruthClass));
                Assert.False(string.IsNullOrWhiteSpace(record.ProvenanceLabel));
                Assert.False(string.IsNullOrWhiteSpace(record.IdentityStatus));
                Assert.False(string.IsNullOrWhiteSpace(record.NumericClaimLabel));
                Assert.DoesNotContain("safe", record.NumericClaimLabel, StringComparison.OrdinalIgnoreCase);
            });

            var cobalt = records.Single(r => r.SourceRecordId == "liturgy_cobalt_psalm_of_blue_glow");
            Assert.Contains("Sacred count named in the liturgy: 45000 cpm", cobalt.NumericClaimLabel);
            Assert.Contains("ORDER_OF_THE_COBALT_FLAME", cobalt.Subtitle);
            Assert.Contains("Unresolved sect identity", cobalt.IdentityStatus);

            var iron = records.Single(r => r.SourceRecordId == "canon_synod_first_law_of_temper");
            Assert.Contains("Canon-prescribed furnace temperature: 1250 °C", iron.NumericClaimLabel);

            var hymn = records.Single(r => r.SourceRecordId == "hymnal_geophone_tuning_of_the_pickup_coil");
            Assert.Contains("Hymnal frequency notation: 4.5 Hz", hymn.NumericClaimLabel);

            var epitaph = records.Single(r => r.SourceRecordId == "epitaph_scav_rusted_license_plate");
            Assert.Contains("Cause of death recorded on marker", epitaph.NumericClaimLabel);
            Assert.Contains("Unresolved memorial identity", epitaph.IdentityStatus);
        }

        [Fact]
        public void ProducerMap_UsesCanonicalLocationsOrExplicitShelterRooms()
        {
            var catalog = LoadDiscoveryCatalog();
            var knownIds = CollectIds(Path.Combine(FindDataDir(), "locations.json"));
            knownIds.Add("room_foundry");
            knownIds.Add("room_radio_tuner");
            knownIds.Add("room_memorial_wall");

            foreach (var record in catalog.AllRecords.Where(r => FringeCultRuntimeContract.IsSourceCatalog(r.SourceCatalog)))
                Assert.Contains(record.ProducerId, knownIds);
        }

        [Fact]
        public void EpitaphDiscovery_DoesNotChangeMortalityOrCreateMemorials()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            var memorials = new MemorialSystem(new MemorialState());

            Assert.True(catalog.TryDiscover(
                "disc_fringe_epitaph_rusted_license_plate", journal, out var record));
            Assert.NotNull(record);
            Assert.Empty(memorials.Entries);
            Assert.False(journal.IsSurvivorMet("courier_jonas_vance"));
        }

        [Fact]
        public void Discovery_IsIdempotentAcrossSaveRestore()
        {
            var catalog = LoadDiscoveryCatalog();
            var journal = new JournalSystem();
            const string id = "disc_fringe_cobalt_psalm_of_blue_glow";

            Assert.True(catalog.TryDiscover(id, journal, out _));
            int unlockCount = journal.CodexUnlockCount;
            var save = journal.CaptureState();
            var restored = new JournalSystem();
            restored.RestoreState(save);

            Assert.False(catalog.TryDiscover(id, restored, out _));
            Assert.Equal(unlockCount, restored.CodexUnlockCount);
            Assert.True(restored.IsNarrativeDiscovered(id));
        }

        [Fact]
        public void ManifestDuplicateDiscoveryId_IsRejectedByProjection()
        {
            string dataDir = FindDataDir();
            string first = Entry("duplicate_fringe_id", FringeCultRuntimeContract.CobaltCatalog,
                "liturgy_cobalt_psalm_of_blue_glow", "loc_settlement_pilgrim_hearth");
            string second = Entry("duplicate_fringe_id", FringeCultRuntimeContract.IronCatalog,
                "canon_synod_first_law_of_temper", "room_foundry");
            var catalog = new NarrativeDiscoveryCatalog();
            catalog.Load($"{{\"schema_version\":1,\"entries\":[{first},{second}]}}", dataDir, new FileSystemIO());

            Assert.Equal(1, catalog.Count);
            Assert.True(catalog.TryGetRecord("duplicate_fringe_id", out var record));
            Assert.Equal("liturgy_cobalt_psalm_of_blue_glow", record!.SourceRecordId);
        }

        [Fact]
        public void ManifestOrder_DoesNotChangeProjectedOrder()
        {
            string dataDir = FindDataDir();
            string first = Entry("order_a", FringeCultRuntimeContract.CobaltCatalog,
                "liturgy_cobalt_psalm_of_blue_glow", "loc_settlement_pilgrim_hearth");
            string second = Entry("order_b", FringeCultRuntimeContract.IronCatalog,
                "canon_synod_first_law_of_temper", "room_foundry");
            var forward = new NarrativeDiscoveryCatalog();
            var reverse = new NarrativeDiscoveryCatalog();
            forward.Load($"{{\"schema_version\":1,\"entries\":[{first},{second}]}}", dataDir, new FileSystemIO());
            reverse.Load($"{{\"schema_version\":1,\"entries\":[{second},{first}]}}", dataDir, new FileSystemIO());

            Assert.Equal(
                forward.AllRecords.Select(r => r.DiscoveryId),
                reverse.AllRecords.Select(r => r.DiscoveryId));
        }

        private static string Entry(string discoveryId, string sourceCatalog, string sourceRecordId, string producerId)
        {
            return $"{{\"discovery_id\":\"{discoveryId}\",\"source_catalog\":\"{sourceCatalog}\",\"source_record_id\":\"{sourceRecordId}\",\"channel\":\"location_inspection\",\"producer_id\":\"{producerId}\",\"min_day\":1}}";
        }

        private static HashSet<string> CollectIds(string path)
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            CollectIds(doc.RootElement, ids);
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
