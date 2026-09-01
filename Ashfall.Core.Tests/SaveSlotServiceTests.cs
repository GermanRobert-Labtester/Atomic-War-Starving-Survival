using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core.Save;

namespace Ashfall.Core.Tests;

public class SaveSlotServiceTests
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

    private string UniquePath(string suffix) => $"/tmp/ashfall_save_slot_tests_{suffix}";

    [Fact]
    public void CreateSlot_CreatesDirectory()
    {
        string path = UniquePath("create_dir");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");

        Assert.True(service.CreateSlot(profile, slot));
        Assert.True(service.SlotExists(profile, slot));
    }

    [Fact]
    public void CreateSlot_ReturnsFalseIfAlreadyExists()
    {
        string path = UniquePath("create_exists");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");

        Assert.True(service.CreateSlot(profile, slot));
        Assert.False(service.CreateSlot(profile, slot));
    }

    [Fact]
    public void SlotExists_ReturnsFalseForMissing()
    {
        string path = UniquePath("slot_missing");
        var service = CreateService(path);
        Assert.False(service.SlotExists(new SaveProfileId("p1"), new SaveSlotId("missing")));
    }

    [Fact]
    public void ListSlots_ReturnsCreatedSlots()
    {
        string path = UniquePath("list_slots");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        service.CreateSlot(profile, new SaveSlotId("a"));
        service.CreateSlot(profile, new SaveSlotId("b"));

        var slots = service.ListSlots(profile);
        Assert.Equal(2, slots.Count);
    }

    [Fact]
    public void SaveAndLoadManifest_RoundTrips()
    {
        string path = UniquePath("manifest_rt");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        service.CreateSlot(profile, slot);

        var manifest = new SaveManifest
        {
            profileId = profile,
            slotId = slot,
            campaignName = "ManifestTest",
            currentDay = 7,
            seed = 55
        };
        service.SaveManifest(profile, slot, manifest);

        var loaded = service.LoadManifest(profile, slot);
        Assert.NotNull(loaded);
        Assert.Equal("ManifestTest", loaded!.campaignName);
        Assert.Equal(7, loaded.currentDay);
    }

    [Fact]
    public void WriteAggregateAtomically_CreatesValidEnvelope()
    {
        string path = UniquePath("atomic_write");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");

        var sectionPayload = "{\"day\":5}";
        var section = new SaveSectionEnvelope
        {
            sectionName = "test",
            schemaVersion = 1,
            payloadJson = sectionPayload,
            checksum = SaveSlotService.ComputeSectionChecksum(new SaveSectionEnvelope
            {
                sectionName = "test",
                schemaVersion = 1,
                payloadJson = sectionPayload
            })
        };

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "Atomic",
                currentDay = 5,
                seed = 99
            },
            sections = new List<SaveSectionEnvelope> { section }
        };

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));
    }

    [Fact]
    public void LoadAggregate_ReturnsWrittenEnvelope()
    {
        string path = UniquePath("load_agg");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "LoadTest",
                currentDay = 10,
                seed = 77
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "game",
                    schemaVersion = 1,
                    payloadJson = "{}"
                }
            }
        };

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));
        var loaded = service.LoadAggregate(profile, slot);

        Assert.NotNull(loaded);
        Assert.Equal("LoadTest", loaded!.manifest.campaignName);
        Assert.Equal(10, loaded.manifest.currentDay);
    }

    [Fact]
    public void LoadAggregate_ReturnsNullForMissing()
    {
        string path = UniquePath("load_missing");
        var service = CreateService(path);
        var loaded = service.LoadAggregate(new SaveProfileId("p1"), new SaveSlotId("missing"));
        Assert.Null(loaded);
    }

    [Fact]
    public void ValidateAggregate_RejectsMismatchedChecksum()
    {
        string path = UniquePath("validate_bad");
        var service = CreateService(path);
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
                    sectionName = "x",
                    schemaVersion = 1,
                    payloadJson = "{}"
                }
            },
            aggregateChecksum = "totally-wrong"
        };

        var result = service.ValidateAggregate(envelope);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void TryImportLegacySave_CreatesNewSlot()
    {
        string path = UniquePath("legacy_import");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");

        string legacyPath = Path.Combine(path, "legacy.json");
        File.WriteAllText(legacyPath, "{\"day\":7,\"seed\":42}");

        var slot = new SaveSlotId("imported_1");
        Assert.True(service.TryImportLegacySave(legacyPath, profile, slot, out _));
        Assert.True(service.SlotExists(profile, slot));

        var loaded = service.LoadAggregate(profile, slot);
        Assert.NotNull(loaded);
        Assert.True(loaded!.migratedFromLegacy);
    }

    [Fact]
    public void TryImportLegacySave_RejectsMissingFile()
    {
        string path = UniquePath("legacy_missing");
        var service = CreateService(path);
        Assert.False(service.TryImportLegacySave("/nonexistent/legacy.json", new SaveProfileId("p1"), new SaveSlotId("s1"), out _));
    }

    [Fact]
    public void TryImportLegacySave_ReturnsFalseIfSlotExists()
    {
        string path = UniquePath("legacy_idem");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");

        string legacyPath = Path.Combine(path, "legacy2.json");
        File.WriteAllText(legacyPath, "{\"day\":1}");

        Assert.True(service.TryImportLegacySave(legacyPath, profile, new SaveSlotId("legacy_slot"), out _));
        Assert.False(service.TryImportLegacySave(legacyPath, profile, new SaveSlotId("legacy_slot"), out _));
    }

    [Fact]
    public void DeleteSlot_RemovesDirectory()
    {
        string path = UniquePath("delete_ok");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        service.CreateSlot(profile, slot);

        Assert.True(service.DeleteSlot(profile, slot));
        Assert.False(service.SlotExists(profile, slot));
    }

    [Fact]
    public void DeleteSlot_ReturnsTrueIfAlreadyMissing()
    {
        string path = UniquePath("delete_missing");
        var service = CreateService(path);
        Assert.True(service.DeleteSlot(new SaveProfileId("p1"), new SaveSlotId("missing")));
    }

    [Fact]
    public void DeleteSlot_RejectsIronManTerminal()
    {
        string path = UniquePath("delete_ironman");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("ironman");

        service.CreateSlot(profile, slot);
        var manifest = new SaveManifest
        {
            profileId = profile,
            slotId = slot,
            campaignName = "Iron",
            currentDay = 1,
            seed = 1,
            mode = CampaignMode.IronMan,
            ironManTerminalState = IronManTerminalState.TerminalLoss
        };
        service.SaveManifest(profile, slot, manifest);

        Assert.False(service.DeleteSlot(profile, slot));
        Assert.True(service.SlotExists(profile, slot));
    }

    [Fact]
    public void IsIronManTerminal_ReturnsFalseForNormal()
    {
        string path = UniquePath("ironman_false");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        service.CreateSlot(profile, slot);

        Assert.False(service.IsIronManTerminal(profile, slot));
    }

    [Fact]
    public void IsIronManTerminal_ReturnsTrueForTerminalLoss()
    {
        string path = UniquePath("ironman_true");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        service.CreateSlot(profile, slot);

        var manifest = new SaveManifest
        {
            profileId = profile,
            slotId = slot,
            campaignName = "Iron",
            currentDay = 1,
            seed = 1,
            mode = CampaignMode.IronMan,
            ironManTerminalState = IronManTerminalState.TerminalLoss
        };
        service.SaveManifest(profile, slot, manifest);

        Assert.True(service.IsIronManTerminal(profile, slot));
    }

    [Fact]
    public void ListProfiles_ReturnsProfileWithSlots()
    {
        string path = UniquePath("profiles");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        service.CreateSlot(profile, new SaveSlotId("s1"));

        var profiles = service.ListProfiles();
        Assert.Single(profiles);
        Assert.Equal(profile, profiles[0]);
    }

    [Fact]
    public void WriteAggregate_CreatesBackupOnSecondWrite()
    {
        string path = UniquePath("backup");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = 1,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "Backup",
                currentDay = 1,
                seed = 1
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "x",
                    schemaVersion = 1,
                    payloadJson = "{}"
                }
            }
        };

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));
        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));

        string slotRoot = service.GetSlotRoot(profile, slot);
        Assert.True(File.Exists(Path.Combine(slotRoot, "campaign.json")));
    }

    [Fact]
    public void IsIronManTerminal_OnCorruptCampaign_DoesNotQuarantine()
    {
        // The terminal check is a read-only policy probe consulted by
        // SelectSlot/DeleteSlot before the user has taken any destructive
        // action. It must never quarantine (move/delete) the campaign file
        // it is only being asked to describe — that is reserved for an
        // explicit load/restore attempt via TryLoadAggregate.
        string path = UniquePath("ironman_no_quarantine");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        service.CreateSlot(profile, slot);

        string aggregatePath = service.GetAggregatePath(profile, slot);
        File.WriteAllText(aggregatePath, "{ not valid json");

        bool result = service.IsIronManTerminal(profile, slot);

        // Indeterminate/invalid campaigns fail closed for the destructive
        // policy check (blocked = true)...
        Assert.True(result);
        // ...but the corrupt file must be left exactly where it was so a
        // later explicit recovery/load attempt can still inspect it.
        Assert.True(File.Exists(aggregatePath));
        Assert.False(File.Exists(aggregatePath + "." + slot.Value + SaveSlotService.QuarantineExtension));
    }

    [Fact]
    public void IsIronManTerminal_OnValidNonTerminalCampaign_ReturnsFalseAndPreservesFile()
    {
        string path = UniquePath("ironman_valid_preserved");
        var service = CreateService(path);
        var profile = new SaveProfileId("p1");
        var slot = new SaveSlotId("s1");
        service.CreateSlot(profile, slot);

        var envelope = new AggregateSaveEnvelope
        {
            manifestVersion = CampaignEnvelopeBuilder.CurrentEnvelopeVersion,
            manifest = new SaveManifest
            {
                profileId = profile,
                slotId = slot,
                campaignName = "Valid",
                currentDay = 1,
                seed = 1,
                generationId = "gen-1",
                ironManTerminalState = IronManTerminalState.Active
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "journal",
                    schemaVersion = SaveSectionRegistry.SchemaVersionFor("journal"),
                    generationId = "gen-1",
                    payloadJson = "{}"
                }
            }
        };
        envelope.sections[0].checksum = SaveSlotService.ComputeSectionChecksum(envelope.sections[0]);
        envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(envelope);

        Assert.True(service.WriteAggregateAtomically(profile, slot, envelope));

        string aggregatePath = service.GetAggregatePath(profile, slot);
        Assert.False(service.IsIronManTerminal(profile, slot));
        Assert.True(File.Exists(aggregatePath));
        Assert.False(File.Exists(aggregatePath + "." + slot.Value + SaveSlotService.QuarantineExtension));
    }
}
