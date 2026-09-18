// SPDX-License-Identifier: MIT
// XP-01 — campaign manifest difficulty contract pins.
// TEST-AGGREGATION: none (independent save/checksum contract).
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Difficulty;

public class DifficultyManifestContractTests
{
    private static AggregateSaveEnvelope BuildEnvelope(int manifestVersion, string presetId)
    {
        return new AggregateSaveEnvelope
        {
            manifestVersion = CampaignEnvelopeBuilder.CurrentEnvelopeVersion,
            manifest = new SaveManifest
            {
                manifestVersion = manifestVersion,
                profileId = new SaveProfileId("p1"),
                slotId = new SaveSlotId("s1"),
                campaignName = "Difficulty contract",
                currentDay = 1,
                seed = 1,
                difficultyPresetId = presetId
            },
            sections = new List<SaveSectionEnvelope>
            {
                new SaveSectionEnvelope
                {
                    sectionName = "campaign_day",
                    schemaVersion = 1,
                    payloadJson = "{\"day\":1}"
                }
            }
        };
    }

    [Fact]
    public void V1Manifest_ChecksumIgnoresDifficultyField()
    {
        // Legacy envelopes were checksummed before the field existed. A v1
        // manifest must produce the identical checksum whether the
        // deserialized preset id is empty (real v1 saves) or populated, so
        // every existing save keeps validating byte-for-byte.
        string legacy = SaveSlotService.ComputeAggregateChecksum(BuildEnvelope(1, string.Empty));
        string populated = SaveSlotService.ComputeAggregateChecksum(BuildEnvelope(1, "difficulty_austere"));
        Assert.Equal(legacy, populated);
    }

    [Fact]
    public void V2Manifest_ChecksumBindsDifficultyPreset()
    {
        string standard = SaveSlotService.ComputeAggregateChecksum(BuildEnvelope(2, "difficulty_standard"));
        string austere = SaveSlotService.ComputeAggregateChecksum(BuildEnvelope(2, "difficulty_austere"));
        string empty = SaveSlotService.ComputeAggregateChecksum(BuildEnvelope(2, string.Empty));

        Assert.NotEqual(standard, austere);
        Assert.NotEqual(standard, empty);

        // Deterministic recompute.
        Assert.Equal(standard, SaveSlotService.ComputeAggregateChecksum(BuildEnvelope(2, "difficulty_standard")));
    }

    [Fact]
    public void ManifestJson_RoundTripsPresetIdAndLegacyAbsence()
    {
        var options = new JsonSerializerOptions { IncludeFields = true };

        string json = JsonSerializer.Serialize(BuildEnvelope(2, "difficulty_sparing").manifest, options);
        var restored = JsonSerializer.Deserialize<SaveManifest>(json, options);
        Assert.NotNull(restored);
        Assert.Equal(2, restored!.manifestVersion);
        Assert.Equal("difficulty_sparing", restored.difficultyPresetId);

        // A v1 manifest's JSON carries no difficulty field; deserialization
        // yields the empty default, which resolves to the catalog default.
        string legacyJson = "{\"manifestVersion\":1,\"currentDay\":7}";
        var legacy = JsonSerializer.Deserialize<SaveManifest>(legacyJson, options);
        Assert.NotNull(legacy);
        Assert.Equal(1, legacy!.manifestVersion);
        Assert.Equal(string.Empty, legacy.difficultyPresetId);
    }

    [Fact]
    public void NewManifests_DefaultToVersionTwo()
    {
        var manifest = new SaveManifest();
        Assert.Equal(SaveManifest.CurrentManifestVersion, manifest.manifestVersion);
        Assert.Equal(2, manifest.manifestVersion);
        Assert.Equal(string.Empty, manifest.difficultyPresetId);
    }
}
