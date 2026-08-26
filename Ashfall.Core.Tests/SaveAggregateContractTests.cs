using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core.Save;

namespace Ashfall.Core.Tests;

public class SaveAggregateContractTests
{
    private const string TestBasePath = "/tmp/ashfall_aggregate_contract";

    private void EnsureCleanBase()
    {
        if (Directory.Exists(TestBasePath))
            Directory.Delete(TestBasePath, recursive: true);
        Directory.CreateDirectory(TestBasePath);
    }

    [Fact]
    public void ComputeSectionChecksum_IsDeterministic()
    {
        var section = new SaveSectionEnvelope
        {
            sectionName = "test",
            schemaVersion = 1,
            payloadJson = "{\"x\":1}"
        };
        string hash1 = SaveSlotService.ComputeSectionChecksum(section);
        string hash2 = SaveSlotService.ComputeSectionChecksum(section);
        Assert.Equal(hash1, hash2);
        Assert.False(string.IsNullOrEmpty(hash1));
    }

    [Fact]
    public void ComputeAggregateChecksum_ChangesWhenSectionChanges()
    {
        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = new SaveProfileId("p1"),
                slotId = new SaveSlotId("s1"),
                campaignName = "Test",
                currentDay = 1,
                seed = 1
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "a",
                    schemaVersion = 1,
                    payloadJson = "{\"v\":1}"
                }
            }
        };
        string hash1 = SaveSlotService.ComputeAggregateChecksum(envelope);

        envelope.sections[0] = new SaveSectionEnvelope
        {
            sectionName = "a",
            schemaVersion = 1,
            payloadJson = "{\"v\":2}"
        };
        string hash2 = SaveSlotService.ComputeAggregateChecksum(envelope);

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ValidateAggregate_AcceptsWellFormedEnvelope()
    {
        EnsureCleanBase();
        var service = new SaveSlotService(
            new FileSystemIO(),
            new SystemTextJsonSerializer(),
            new ConsoleLog(),
            TestBasePath);

        var section = new SaveSectionEnvelope
        {
            sectionName = "test",
            schemaVersion = 1,
            payloadJson = "{}"
        };
        string sectionChecksum = SaveSlotService.ComputeSectionChecksum(section);

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = new SaveProfileId("p1"),
                slotId = new SaveSlotId("s1"),
                campaignName = "Test",
                currentDay = 1,
                seed = 1
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "test",
                    schemaVersion = 1,
                    payloadJson = "{}",
                    checksum = sectionChecksum
                }
            }
        };
        envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);

        var result = service.ValidateAggregate(envelope);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateAggregate_RejectsNullManifest()
    {
        EnsureCleanBase();
        var service = new SaveSlotService(
            new FileSystemIO(),
            new SystemTextJsonSerializer(),
            new ConsoleLog(),
            TestBasePath);

        var envelope = new AggregateSaveEnvelope
        {
            manifest = null!,
            sections = new List<SaveSectionEnvelope>()
        };

        var result = service.ValidateAggregate(envelope);
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void ValidateAggregate_RejectsMissingChecksum()
    {
        EnsureCleanBase();
        var service = new SaveSlotService(
            new FileSystemIO(),
            new SystemTextJsonSerializer(),
            new ConsoleLog(),
            TestBasePath);

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = new SaveProfileId("p1"),
                slotId = new SaveSlotId("s1"),
                campaignName = "Test",
                currentDay = 1,
                seed = 1
            },
            sections = new List<SaveSectionEnvelope>(),
            aggregateChecksum = string.Empty
        };

        var result = service.ValidateAggregate(envelope);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void RoundTrip_WriteAndLoadAggregate_PreservesData()
    {
        EnsureCleanBase();
        var service = new SaveSlotService(
            new FileSystemIO(),
            new SystemTextJsonSerializer(),
            new ConsoleLog(),
            TestBasePath);

        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "RoundTrip",
                currentDay = 42,
                seed = 123
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "game",
                    schemaVersion = 1,
                    payloadJson = "{\"health\":100}"
                }
            }
        };

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));
        var loaded = service.LoadAggregate(profile, slot);

        Assert.NotNull(loaded);
        Assert.Equal("RoundTrip", loaded!.manifest.campaignName);
        Assert.Equal(42, loaded.manifest.currentDay);
        Assert.Single(loaded.sections);
        Assert.Equal("game", loaded.sections[0].sectionName);
    }

    [Fact]
    public void RoundTrip_MultiSectionAggregate_PreservesAllSections()
    {
        EnsureCleanBase();
        var service = new SaveSlotService(
            new FileSystemIO(),
            new SystemTextJsonSerializer(),
            new ConsoleLog(),
            TestBasePath);

        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "MultiSection",
                currentDay = 7,
                seed = 55
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "inventory",
                    schemaVersion = 1,
                    payloadJson = "{\"items\":[{\"id\":\"canned_food\",\"count\":5}]}"
                },
                new SaveSectionEnvelope
                {
                    sectionName = "world",
                    schemaVersion = 1,
                    payloadJson = "{\"weather\":\"clear\"}"
                },
                new SaveSectionEnvelope
                {
                    sectionName = "survivors",
                    schemaVersion = 1,
                    payloadJson = "[{\"id\":\"survivor_1\",\"name\":\"Alex\"}]"
                }
            }
        };

        // Compute checksums like PackAggregateEnvelope does.
        for (int i = 0; i < envelope.sections.Count; i++)
        {
            var s = envelope.sections[i];
            if (string.IsNullOrEmpty(s.checksum) && !string.IsNullOrEmpty(s.payloadJson))
            {
                s.checksum = SaveSlotService.ComputeSectionChecksum(s);
                envelope.sections[i] = s;
            }
        }
        envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));
        var loaded = service.LoadAggregate(profile, slot);

        Assert.NotNull(loaded);
        Assert.Equal(3, loaded!.sections.Count);
        Assert.Equal("inventory", loaded.sections[0].sectionName);
        Assert.Equal("world", loaded.sections[1].sectionName);
        Assert.Equal("survivors", loaded.sections[2].sectionName);
    }
}
