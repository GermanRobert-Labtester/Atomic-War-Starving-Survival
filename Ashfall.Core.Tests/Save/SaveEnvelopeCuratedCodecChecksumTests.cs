// SPDX-License-Identifier: MIT
// ASHFALL save-envelope checksum: the six curated codecs.
//
// Extends the sample-DTO invariant suite to the real versioned codecs. Each
// codec's default state must survive a System.Text.Json round trip with an
// identical SaveChecksum, proving a save written by one host parses and re-hashes
// identically on the other.
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public sealed class SaveEnvelopeCuratedCodecChecksumTests
    {
        private static readonly (string Name, Type Type, object State)[] Codecs =
        {
            ("holdfast", typeof(HoldfastSave), new HoldfastSave()),
            ("year_of_ash", typeof(YearOfAshSave), new YearOfAshSave()),
            ("dose_ledger", typeof(DoseLedgerSave), new DoseLedgerSave()),
            ("expansion_hub", typeof(ExpansionHubSave), new ExpansionHubSave()),
            ("expansion_quest", typeof(ExpansionQuestSaveEnvelope), new ExpansionQuestSaveEnvelope()),
            ("weight_of_choices", typeof(WeightOfChoicesSave), new WeightOfChoicesSave()),
        };

        [Fact]
        public void CuratedCodecs_ChecksumIsStableAcrossSerializerRoundTrip()
        {
            var serializer = new SystemTextJsonSerializer();
            var failures = new List<string>();

            foreach (var (name, type, state) in Codecs)
            {
                string json = serializer.Serialize(state);
                object? restored = JsonSerializer.Deserialize(json, type, SystemTextJsonSerializer.Options);
                if (restored == null)
                {
                    failures.Add($"{name}: deserialized to null");
                    continue;
                }

                string before = SaveChecksum.Compute(state);
                string after = SaveChecksum.Compute(restored);
                if (before != after)
                    failures.Add($"{name}: {before} != {after}");
            }

            Assert.True(failures.Count == 0, string.Join(" | ", failures));
        }

        [Fact]
        public void CuratedCodec_ChecksumChangesWhenStateChanges()
        {
            var a = new HoldfastSave { simDay = 5 };
            var b = new HoldfastSave { simDay = 6 };
            Assert.NotEqual(SaveChecksum.Compute(a), SaveChecksum.Compute(b));
        }
    }
}
