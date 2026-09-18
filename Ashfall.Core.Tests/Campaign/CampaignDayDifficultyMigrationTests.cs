// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class CampaignDayDifficultyMigrationTests
    {
        [Fact]
        public void Decode_V1ChecksummedHeader_MigratesToEmptyDifficulty()
        {
            var serializer = new SystemTextJsonSerializer();
            var legacy = new CampaignDaySaveV1
            {
                saveVersion = 1,
                lastAdvancedDay = 42,
                masterSeed = 707,
                derivationVersion = 3,
                streamPositions = new Dictionary<string, int>
                {
                    ["campaign"] = 11
                }
            };
            legacy.Checksum = SaveChecksum.Compute(legacy);

            CampaignDaySave decoded = CampaignDaySaveCodec.Decode(
                serializer.Serialize(legacy), serializer);

            Assert.Equal(CampaignDaySave.CurrentSaveVersion, decoded.saveVersion);
            Assert.Equal(string.Empty, decoded.difficulty_preset_id);
            Assert.Equal(42, decoded.lastAdvancedDay);
            Assert.Equal(707, decoded.masterSeed);

            string migratedJson = CampaignDaySaveCodec.EncodeToString(decoded, serializer);
            CampaignDaySave reloaded = CampaignDaySaveCodec.Decode(migratedJson, serializer);
            Assert.Equal(CampaignDaySave.CurrentSaveVersion, reloaded.saveVersion);
            Assert.Equal(string.Empty, reloaded.difficulty_preset_id);
        }

        [Fact]
        public void RoundTrip_PreservesSelectedDifficultyInChecksummedHeader()
        {
            var serializer = new SystemTextJsonSerializer();
            var state = new CampaignDaySave
            {
                lastAdvancedDay = 18,
                masterSeed = 991,
                difficulty_preset_id = "difficulty_austere"
            };

            string payload = CampaignDaySaveCodec.EncodeToString(state, serializer);
            CampaignDaySave restored = CampaignDaySaveCodec.Decode(payload, serializer);

            Assert.Equal("difficulty_austere", restored.difficulty_preset_id);
            Assert.Equal(CampaignDaySave.CurrentSaveVersion, restored.saveVersion);
            Assert.Equal(state.Checksum, restored.Checksum);
        }

        private sealed class CampaignDaySaveV1
        {
            public int saveVersion = 1;
            public int lastAdvancedDay = -1;
            public int masterSeed = 1986;
            public int derivationVersion = 1;
            public Dictionary<string, int> streamPositions =
                new Dictionary<string, int>();
            public string Checksum = string.Empty;
        }
    }
}
