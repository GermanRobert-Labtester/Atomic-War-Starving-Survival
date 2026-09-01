using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core.Save;

namespace Ashfall.Core.Tests;

public class SaveLoadFailurePathTests
{
    private SaveSlotService CreateService(string basePath)
    {
        if (Directory.Exists(basePath))
            Directory.Delete(basePath, recursive: true);
        Directory.CreateDirectory(basePath);
        return new SaveSlotService(
            new FileSystemIO(),
            new SystemTextJsonSerializer(),
            new ConsoleLog(),
            basePath);
    }

    private string UniquePath(string suffix) => $"/tmp/ashfall_save_load_failure_tests_{suffix}_{DateTime.UtcNow.Ticks}";

    [Fact]
    public void TryLoadAggregate_MissingFile_ReturnsMissingStatusAndClearMessage()
    {
        string path = UniquePath("missing_file");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("missing_slot");

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.MissingFile, result.Status);
        Assert.Contains("not found", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Null(result.Envelope);
    }

    [Fact]
    public void TryLoadAggregate_EmptyFile_ReturnsCorruptStatusAndPreservesSession()
    {
        string path = UniquePath("empty_file");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("empty_slot");

        service.CreateSlot(profile, slot);
        string aggPath = service.GetAggregatePath(profile, slot);
        File.WriteAllText(aggPath, "   \n  \t  ");

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.CorruptData, result.Status);
        Assert.Contains("corrupt", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Live session preserved", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Null(result.Envelope);

        // Quarantine check
        string quarantinePath = aggPath + "." + slot.Value + SaveSlotService.QuarantineExtension;
        Assert.True(File.Exists(quarantinePath));
    }

    [Fact]
    public void TryLoadAggregate_MalformedJson_QuarantinesAndReturnsCorruptStatus()
    {
        string path = UniquePath("malformed_json");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("broken_slot");

        service.CreateSlot(profile, slot);
        string aggPath = service.GetAggregatePath(profile, slot);
        File.WriteAllText(aggPath, "{\"manifestVersion\": 1, \"manifest\": { \"slotId\": \"broken\", \"sections\": [INVALID_JSON_HERE");

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.CorruptData, result.Status);
        Assert.Contains("corrupted", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Live session preserved", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Null(result.Envelope);

        string quarantinePath = aggPath + "." + slot.Value + SaveSlotService.QuarantineExtension;
        Assert.True(File.Exists(quarantinePath));
    }

    [Fact]
    public void TryLoadAggregate_MismatchedAggregateChecksum_QuarantinesAndReturnsChecksumStatus()
    {
        string path = UniquePath("bad_agg_checksum");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("tampered_slot");

        service.CreateSlot(profile, slot);
        string aggPath = service.GetAggregatePath(profile, slot);

        var section = new SaveSectionEnvelope
        {
            sectionName = "inventory",
            schemaVersion = 1,
            payloadJson = "{\"water\": 10}"
        };
        section.checksum = SaveSlotService.ComputeSectionChecksum(section);

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "Tampered",
                currentDay = 10,
                seed = 42
            },
            sections = new List<SaveSectionEnvelope> { section },
            aggregateChecksum = "0000000000000000000000000000000000000000000000000000000000000000" // Corrupted hash
        };

        File.WriteAllText(aggPath, new SystemTextJsonSerializer().Serialize(envelope));

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.ChecksumMismatch, result.Status);
        Assert.Contains("checksum", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Live session preserved", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Null(result.Envelope);

        string quarantinePath = aggPath + "." + slot.Value + SaveSlotService.QuarantineExtension;
        Assert.True(File.Exists(quarantinePath));
    }

    [Fact]
    public void TryLoadAggregate_MismatchedSectionChecksum_QuarantinesAndReturnsChecksumStatus()
    {
        string path = UniquePath("bad_sec_checksum");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("tampered_sec_slot");

        service.CreateSlot(profile, slot);
        string aggPath = service.GetAggregatePath(profile, slot);

        var section = new SaveSectionEnvelope
        {
            sectionName = "inventory",
            schemaVersion = 1,
            payloadJson = "{\"water\": 50}",
            checksum = "bad_section_checksum_value"
        };

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "Bad Section Checksum",
                currentDay = 5,
                seed = 11
            },
            sections = new List<SaveSectionEnvelope> { section }
        };
        envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);

        File.WriteAllText(aggPath, new SystemTextJsonSerializer().Serialize(envelope));

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.ChecksumMismatch, result.Status);
        Assert.Contains("checksum", result.UserMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Null(result.Envelope);
    }

    [Fact]
    public void TryLoadAggregate_IronManTerminal_ReturnsBlockedStatus()
    {
        string path = UniquePath("ironman_blocked");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("iron_slot");

        service.CreateSlot(profile, slot);
        var manifest = new SaveManifest
        {
            profileId = profile,
            slotId = slot,
            campaignName = "IronMan Lost",
            currentDay = 3,
            seed = 99,
            mode = CampaignMode.IronMan,
            ironManTerminalState = IronManTerminalState.TerminalLoss
        };
        service.SaveManifest(profile, slot, manifest);

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.IronManBlocked, result.Status);
        Assert.Contains("Iron Man", result.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TryLoadAggregate_ValidEnvelope_ReturnsSuccessStatusAndEnvelope()
    {
        string path = UniquePath("valid_envelope");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("valid_slot");

        var section = new SaveSectionEnvelope
        {
            sectionName = "inventory",
            schemaVersion = 1,
            payloadJson = "{\"water\": 100}"
        };
        section.checksum = SaveSlotService.ComputeSectionChecksum(section);

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "Valid Campaign",
                currentDay = 15,
                seed = 500
            },
            sections = new List<SaveSectionEnvelope> { section }
        };
        envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));

        var result = service.TryLoadAggregate(profile, slot);

        Assert.True(result.IsSuccess);
        Assert.Equal(SaveLoadStatus.Success, result.Status);
        Assert.NotNull(result.Envelope);
        Assert.Equal("Valid Campaign", result.Envelope!.manifest.campaignName);
        Assert.Equal(15, result.Envelope.manifest.currentDay);
        Assert.Contains("successfully", result.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TryLoadAggregate_V1AllStraySections_FailsRatherThanSucceedingEmpty()
    {
        // Probe: does an all-unknown-section V1 envelope migrate to zero
        // sections and still report success? ValidateAggregate requires a
        // non-empty section list, and TryLoadAggregate re-validates after
        // MigrateToCurrent, so this should fail closed rather than return Ok.
        string path = UniquePath("v1_all_stray");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var slot = new SaveSlotId("stray_slot");
        service.CreateSlot(profile, slot);
        string aggPath = service.GetAggregatePath(profile, slot);

        var strayA = new SaveSectionEnvelope
        {
            sectionName = "totally_unknown_section_a",
            schemaVersion = 1,
            payloadJson = "{\"x\":1}"
        };
        strayA.checksum = SaveSlotService.ComputeSectionChecksum(strayA);
        var strayB = new SaveSectionEnvelope
        {
            sectionName = "totally_unknown_section_b",
            schemaVersion = 1,
            payloadJson = "{\"y\":2}"
        };
        strayB.checksum = SaveSlotService.ComputeSectionChecksum(strayB);

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "All Stray",
                currentDay = 3,
                seed = 7
            },
            sections = new List<SaveSectionEnvelope> { strayA, strayB }
        };
        envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);
        File.WriteAllText(aggPath, new SystemTextJsonSerializer().Serialize(envelope));

        var result = service.TryLoadAggregate(profile, slot);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Envelope);
    }

    [Fact]
    public void LoadManifest_V1EnvelopeWithForeignIdentity_DoesNotReturnForeignManifest()
    {
        // Probe: LoadManifest's aggregate-authority branch runs
        // ValidateAggregate + identity comparison unconditionally (not gated
        // on manifestVersion), so a V1 envelope carrying another slot's
        // identity should already be rejected rather than handed back as the
        // requested slot's manifest.
        string path = UniquePath("v1_foreign_manifest");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var requestedSlot = new SaveSlotId("slot_requested");
        var foreignSlot = new SaveSlotId("slot_foreign");
        service.CreateSlot(profile, requestedSlot);
        string aggPath = service.GetAggregatePath(profile, requestedSlot);

        var section = new SaveSectionEnvelope
        {
            sectionName = "inventory",
            schemaVersion = 1,
            payloadJson = "{\"water\": 5}"
        };
        section.checksum = SaveSlotService.ComputeSectionChecksum(section);

        var foreignEnvelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = foreignSlot, // Identity belongs to a different slot.
                campaignName = "Foreign",
                currentDay = 9,
                seed = 3
            },
            sections = new List<SaveSectionEnvelope> { section }
        };
        foreignEnvelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(foreignEnvelope);
        File.WriteAllText(aggPath, new SystemTextJsonSerializer().Serialize(foreignEnvelope));

        var manifest = service.LoadManifest(profile, requestedSlot);

        Assert.Null(manifest);
    }

    [Fact]
    public void WriteAggregateAtomically_V1EnvelopeWithForeignIdentity_IsRejected()
    {
        // Probe: WriteAggregateAtomically's identity check compares
        // envelope.manifest against the profileId/slotId parameters
        // unconditionally, so a V1 envelope targeting a different slot's
        // path but carrying foreign identity should be rejected rather than
        // written.
        string path = UniquePath("v1_write_foreign_identity");
        var service = CreateService(path);
        var profile = new SaveProfileId("default");
        var targetSlot = new SaveSlotId("slot_target");
        var foreignSlot = new SaveSlotId("slot_other");
        service.CreateSlot(profile, targetSlot);

        var section = new SaveSectionEnvelope
        {
            sectionName = "inventory",
            schemaVersion = 1,
            payloadJson = "{\"water\": 5}"
        };

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = foreignSlot, // Does not match targetSlot below.
                campaignName = "Mismatched Write",
                currentDay = 2,
                seed = 1
            },
            sections = new List<SaveSectionEnvelope> { section }
        };

        bool written = service.WriteAggregateAtomically(profile, targetSlot, envelope);

        Assert.False(written);
        string aggPath = service.GetAggregatePath(profile, targetSlot);
        Assert.False(File.Exists(aggPath));
    }
}
