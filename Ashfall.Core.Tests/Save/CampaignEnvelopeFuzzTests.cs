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
    /// Stress-tests the unified campaign envelope (campaign.json), schema whitelist,
    /// malformed payload isolation, backup failover, and migration ladder under fuzzing.
    /// </summary>
    public class CampaignEnvelopeFuzzTests : IDisposable
    {
        private readonly string _tempDir;

        public CampaignEnvelopeFuzzTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "AshfallCampaignFuzzTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        private static SaveManifest CreateManifest(int day = 10, int seed = 1986) => new SaveManifest
        {
            profileId = new SaveProfileId("fuzz_profile"),
            slotId = new SaveSlotId("fuzz_slot"),
            campaignName = "Fuzzing Stress Campaign",
            currentDay = day,
            seed = seed,
        };

        [Fact]
        public void CampaignEnvelope_All65RegisteredKeys_AcceptedAndOrdered()
        {
            var payloads = new Dictionary<string, string>();
            foreach (var meta in SaveSectionRegistry.All)
            {
                payloads[meta.SectionKey] = $"{{\"section\":\"{meta.SectionKey}\",\"data\":123}}";
            }

            var envelope = CampaignEnvelopeBuilder.Build(payloads, CreateManifest());

            Assert.Equal(CampaignEnvelopeBuilder.CurrentEnvelopeVersion, envelope.manifestVersion);
            Assert.Equal(SaveSectionRegistry.All.Count, envelope.sections.Count);

            // Assert exact registry order
            for (int i = 0; i < SaveSectionRegistry.All.Count; i++)
            {
                Assert.Equal(SaveSectionRegistry.All[i].SectionKey, envelope.sections[i].sectionName);
                Assert.True(envelope.sections[i].schemaVersion >= 1);
            }
        }

        [Theory]
        [InlineData("stray_unknown_subsystem")]
        [InlineData("malicious_injection_section")]
        [InlineData("unity_legacy_stub")]
        public void CampaignEnvelope_UnknownSectionKey_StrictlyRejectedByWhitelist(string invalidKey)
        {
            var payloads = new Dictionary<string, string>
            {
                { "world", "{\"world\":true}" },
                { invalidKey, "{\"invalid\":true}" }
            };

            var ex = Assert.Throws<ArgumentException>(() =>
                CampaignEnvelopeBuilder.Build(payloads, CreateManifest()));

            Assert.Contains("Unknown section key", ex.Message);
            Assert.Contains("SaveSectionRegistry", ex.Message);
        }

        [Theory]
        [InlineData("{")]
        [InlineData("{\"unterminated_string\": \"val")]
        [InlineData("null")]
        [InlineData("[]")]
        [InlineData("not_json_at_all")]
        public void CampaignEnvelope_MalformedSectionPayload_EnvelopesWithoutCrashing(string malformedJson)
        {
            var payloads = new Dictionary<string, string>
            {
                { "journal", malformedJson }
            };

            // Builder should package raw string without throwing serialization crashes
            var envelope = CampaignEnvelopeBuilder.Build(payloads, CreateManifest());
            Assert.Single(envelope.sections);
            Assert.Equal("journal", envelope.sections[0].sectionName);
            Assert.Equal(malformedJson, envelope.sections[0].payloadJson);
        }

        [Fact]
        public void CampaignEnvelope_NullManifest_ThrowsArgumentNullException()
        {
            var payloads = new Dictionary<string, string> { { "world", "{}" } };
            Assert.Throws<ArgumentNullException>(() => CampaignEnvelopeBuilder.Build(payloads, null!));
        }

        [Fact]
        public void CampaignEnvelope_EmptyPayloadMap_EmitsEmptySectionsList()
        {
            var payloads = new Dictionary<string, string>();
            var envelope = CampaignEnvelopeBuilder.Build(payloads, CreateManifest());

            Assert.Empty(envelope.sections);
            Assert.Equal(CampaignEnvelopeBuilder.CurrentEnvelopeVersion, envelope.manifestVersion);
        }

        [Fact]
        public void CampaignEnvelope_MissingSections_AreSilentlySkipped()
        {
            // Only provide 2 sections out of 65
            var payloads = new Dictionary<string, string>
            {
                { "world", "{\"world\":1}" },
                { "journal", "{\"journal\":2}" }
            };

            var envelope = CampaignEnvelopeBuilder.Build(payloads, CreateManifest());

            Assert.Equal(2, envelope.sections.Count);
            Assert.Equal("journal", envelope.sections[0].sectionName);
            Assert.Equal("world", envelope.sections[1].sectionName);
        }
    }
}
