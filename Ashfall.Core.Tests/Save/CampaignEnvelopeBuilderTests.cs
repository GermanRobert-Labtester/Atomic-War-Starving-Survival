using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    /// <summary>
    /// Pins the V2 campaign envelope contract: registry-keyed sections in
    /// registry order, real schema versions, whitelist rejection, the V1→V2
    /// migration ladder, and CapturePersisted byte-identity with the file
    /// format.
    /// </summary>
    public class CampaignEnvelopeBuilderTests : IDisposable
    {
        private readonly string _tempDir;

        public CampaignEnvelopeBuilderTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "AshfallEnvelopeBuilderTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        private SaveSlotService NewService() =>
            new SaveSlotService(new FileSystemIO(), new SystemTextJsonSerializer(), new ConsoleLog(), _tempDir);

        private static SaveManifest Manifest(int day = 7) => new SaveManifest
        {
            profileId = new SaveProfileId("p1"),
            slotId = new SaveSlotId("s1"),
            campaignName = "Builder Campaign",
            currentDay = day,
            seed = 42,
        };

        [Fact]
        public void Build_EmitsSectionsInRegistryOrderWithRealSchemaVersions()
        {
            // Deliberately out-of-registry-order input map.
            var payloads = new Dictionary<string, string>
            {
                { "world", "{\"world\":true}" },
                { "holdfast", "{\"saveVersion\":5}" },
                { "journal", "{\"entries\":[]}" },
            };

            AggregateSaveEnvelope envelope = CampaignEnvelopeBuilder.Build(payloads, Manifest());
            Assert.Equal(CampaignEnvelopeBuilder.CurrentEnvelopeVersion, envelope.manifestVersion);
            Assert.Equal(3, envelope.sections.Count);

            // Registry order: journal, holdfast, ... world.
            Assert.Equal("journal", envelope.sections[0].sectionName);
            Assert.Equal("holdfast", envelope.sections[1].sectionName);
            Assert.Equal("world", envelope.sections[2].sectionName);

            Assert.Equal(1, envelope.sections[0].schemaVersion);
            Assert.Equal(5, envelope.sections[1].schemaVersion); // codec ladder version
            Assert.Equal(1, envelope.sections[2].schemaVersion);

            var validation = NewService().ValidateAggregate(envelope);
            Assert.True(validation.IsValid, string.Join("; ", validation.Errors));
        }

        [Fact]
        public void Build_RejectsUnknownSectionKeys()
        {
            var payloads = new Dictionary<string, string> { { "stray_section", "{}" } };
            var ex = Assert.Throws<ArgumentException>(() =>
                CampaignEnvelopeBuilder.Build(payloads, Manifest()));
            Assert.Contains("stray_section", ex.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void Build_SkipsEmptyPayloads()
        {
            var payloads = new Dictionary<string, string>
            {
                { "journal", "{\"entries\":[]}" },
                { "inventory", "  " },
            };
            AggregateSaveEnvelope envelope = CampaignEnvelopeBuilder.Build(payloads, Manifest());
            Assert.Single(envelope.sections);
            Assert.Equal("journal", envelope.sections[0].sectionName);
        }

        [Fact]
        public void MigrateToCurrent_RenamesV1FileSectionsAndDropsStrays()
        {
            string Payload(string v) => "{\"v\":\"" + v + "\"}";
            SaveSectionEnvelope V1(string name, string payload) => new SaveSectionEnvelope
            {
                sectionName = name,
                schemaVersion = 1,
                payloadJson = payload,
            };

            var sections = new List<SaveSectionEnvelope>
            {
                V1("inventory_save", Payload("inv")),
                V1("holdfast_s1_save", Payload("hf")),
                V1("thirdonary_quest_save", Payload("th")),
                V1("weather_save", Payload("stray-weather")),
                V1("holdfast_flavor", Payload("stray-flavor")),
            };
            foreach (var s in sections) s.checksum = SaveSlotService.ComputeSectionChecksum(s);

            var v1 = new AggregateSaveEnvelope { manifestVersion = 1, manifest = Manifest(), sections = sections };
            v1.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(v1);

            AggregateSaveEnvelope v2 = SaveSlotService.MigrateToCurrent(v1);

            Assert.Equal(CampaignEnvelopeBuilder.CurrentEnvelopeVersion, v2.manifestVersion);
            Assert.Equal(3, v2.sections.Count);
            Assert.Equal("inventory", v2.sections[0].sectionName);
            Assert.Equal("holdfast", v2.sections[1].sectionName);
            Assert.Equal(5, v2.sections[1].schemaVersion);
            Assert.Equal("thirdonary", v2.sections[2].sectionName);
            // Payloads survive verbatim.
            Assert.Equal(Payload("inv"), v2.sections[0].payloadJson);
            Assert.Equal(Payload("hf"), v2.sections[1].payloadJson);

            var validation = NewService().ValidateAggregate(v2);
            Assert.True(validation.IsValid, string.Join("; ", validation.Errors));
        }

        [Fact]
        public void MigrateToCurrent_PreservesReservedLegacyImportSection()
        {
            var section = new SaveSectionEnvelope
            {
                sectionName = SaveSlotService.LegacyImportSectionName,
                schemaVersion = 1,
                payloadJson = "{\"raw\":\"imported\"}",
            };
            section.checksum = SaveSlotService.ComputeSectionChecksum(section);
            var v1 = new AggregateSaveEnvelope
            {
                manifestVersion = 1,
                manifest = Manifest(),
                sections = new List<SaveSectionEnvelope> { section },
            };
            v1.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(v1);

            AggregateSaveEnvelope v2 = SaveSlotService.MigrateToCurrent(v1);
            Assert.Single(v2.sections);
            Assert.Equal("legacy", v2.sections[0].sectionName);
            Assert.Equal("{\"raw\":\"imported\"}", v2.sections[0].payloadJson);
        }

        [Fact]
        public void MigrateToCurrent_IsNoOpForCurrentVersion_AndRejectsFuture()
        {
            var envelope = CampaignEnvelopeBuilder.Build(
                new Dictionary<string, string> { { "journal", "{}" } }, Manifest());
            Assert.Same(envelope, SaveSlotService.MigrateToCurrent(envelope));

            var future = new AggregateSaveEnvelope { manifestVersion = 99, manifest = Manifest() };
            Assert.Throws<InvalidOperationException>(() => SaveSlotService.MigrateToCurrent(future));
        }

        [Fact]
        public void ValidateAggregate_AcceptsV1AndCurrent_RejectsFuture()
        {
            var service = NewService();

            var current = CampaignEnvelopeBuilder.Build(
                new Dictionary<string, string> { { "journal", "{}" } }, Manifest());
            Assert.True(service.ValidateAggregate(current).IsValid);

            var section = new SaveSectionEnvelope
            {
                sectionName = "inventory",
                schemaVersion = 1,
                payloadJson = "{}",
            };
            section.checksum = SaveSlotService.ComputeSectionChecksum(section);
            var v1 = new AggregateSaveEnvelope
            {
                manifestVersion = 1,
                manifest = Manifest(),
                sections = new List<SaveSectionEnvelope> { section },
            };
            v1.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(v1);
            Assert.True(service.ValidateAggregate(v1).IsValid);

            var future = new AggregateSaveEnvelope
            {
                manifestVersion = CampaignEnvelopeBuilder.CurrentEnvelopeVersion + 1,
                manifest = Manifest(),
                sections = new List<SaveSectionEnvelope> { section },
            };
            future.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(future);
            Assert.False(service.ValidateAggregate(future).IsValid);
        }

        [Fact]
        public void TryLoadAggregate_MigratesV1InMemoryAndLeavesDiskUntouched()
        {
            var service = NewService();
            var profile = new SaveProfileId("p1");
            var slot = new SaveSlotId("s1");

            var section = new SaveSectionEnvelope
            {
                sectionName = "inventory_save",
                schemaVersion = 1,
                payloadJson = "{\"water\":100}",
            };
            section.checksum = SaveSlotService.ComputeSectionChecksum(section);
            var v1 = new AggregateSaveEnvelope { manifestVersion = 1, manifest = Manifest(), sections = new List<SaveSectionEnvelope> { section } };
            v1.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(v1);

            Assert.True(service.WriteAggregateAtomically(profile, slot, v1));

            var result = service.TryLoadAggregate(profile, slot);
            Assert.True(result.IsSuccess, result.UserMessage);
            Assert.Equal(CampaignEnvelopeBuilder.CurrentEnvelopeVersion, result.Envelope!.manifestVersion);
            Assert.Equal("inventory", result.Envelope.sections[0].sectionName);
            Assert.Equal("{\"water\":100}", result.Envelope.sections[0].payloadJson);

            // The on-disk file stays V1 until the next save rewrites it.
            string disk = File.ReadAllText(service.GetAggregatePath(profile, slot));
            Assert.Contains("\"manifestVersion\":1", disk, StringComparison.Ordinal);
        }

        [Fact]
        public void CapturePersisted_IsByteIdenticalToTrySaveFileContent()
        {
            string baseDir = _tempDir;

            var checksummed = new SaveStore<BuilderState>(
                "checksummed_save.json", new FileSystemIO(), new SystemTextJsonSerializer(), new ConsoleLog(),
                () => baseDir, "ChecksumStore");
            var state = new BuilderState { Day = 3, Name = "identical" };
            Assert.True(checksummed.TrySave(state));
            Assert.Equal(
                File.ReadAllText(checksummed.SavePath),
                checksummed.CapturePersisted(state));

            var codec = SaveStore<BuilderState>.FromCodec(
                "codec_save.json", new FileSystemIO(), new SystemTextJsonSerializer(), new ConsoleLog(),
                () => baseDir, "CodecStore",
                (s, json) => json.Serialize(s),
                (raw, json) => json.Deserialize<BuilderState>(raw));
            Assert.True(codec.TrySave(state));
            Assert.Equal(
                File.ReadAllText(codec.SavePath),
                codec.CapturePersisted(state));
        }

        [Fact]
        public void RegistryFileNames_CoverEverySectionKeyAndMapV1Names()
        {
            Assert.Equal(SaveSectionRegistry.All.Count, SaveSectionRegistry.SectionFileNames.Count);
            foreach (var meta in SaveSectionRegistry.All)
            {
                Assert.True(SaveSectionRegistry.SectionFileNames.ContainsKey(meta.SectionKey),
                    $"missing file name for section '{meta.SectionKey}'");
            }

            // V1 section names (file name sans extension) resolve back to keys.
            Assert.True(SaveSectionRegistry.TryGetKeyForSectionName("inventory_save", out var k1));
            Assert.Equal("inventory", k1);
            Assert.True(SaveSectionRegistry.TryGetKeyForSectionName("holdfast_s1_save.json", out var k2));
            Assert.Equal("holdfast", k2);
            Assert.True(SaveSectionRegistry.TryGetKeyForSectionName("journal", out var k3));
            Assert.Equal("journal", k3);
            Assert.False(SaveSectionRegistry.TryGetKeyForSectionName("weather_save", out _));
            Assert.False(SaveSectionRegistry.TryGetKeyForSectionName("holdfast_flavor", out _));
        }

        private sealed class BuilderState
        {
            public int Day;
            public string Name = string.Empty;
        }
    }
}
