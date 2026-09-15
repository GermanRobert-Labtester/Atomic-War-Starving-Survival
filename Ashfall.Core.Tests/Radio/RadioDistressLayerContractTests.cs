// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Plan 107 reconciliation coverage for the landed Plan 50 catalog and
    /// its later NPC-arc expansion layer. The primary 25-row authority wins
    /// duplicate IDs; unique expansion rows and compatibility built-ins remain.
    /// </summary>
    public sealed class RadioDistressLayerContractTests : CatalogTestBase
    {
        private static List<DistressSignalDefinition> LoadFile(string fileName)
        {
            string path = Path.Combine(DataDirectory, fileName);
            using var document = JsonDocument.Parse(File.ReadAllText(path));
            var list = new List<DistressSignalDefinition>();
            foreach (var element in document.RootElement.GetProperty("radio_broadcasts").EnumerateArray())
            {
                var signal = JsonSerializer.Deserialize<DistressSignalDefinition>(
                    element.GetRawText(), SystemTextJsonSerializer.Options);
                if (signal != null) list.Add(signal);
            }
            return list;
        }

        [Fact]
        public void LandedCatalogLayers_Are25PrimaryAnd19Expansion_WithFiveSharedIds()
        {
            var primary = LoadFile("radio_distress_signals.json");
            var expansion = LoadFile("radio_distress_signals_expansion.json");
            var primaryIds = primary.Select(signal => signal.FrequencyId).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var expansionIds = expansion.Select(signal => signal.FrequencyId).ToHashSet(StringComparer.OrdinalIgnoreCase);

            Assert.Equal(25, primary.Count);
            // 16 landed + §24 content waves (3 + 4 new scenarios).
            Assert.Equal(23, expansion.Count);
            Assert.Equal(primary.Count, primaryIds.Count);
            Assert.Equal(expansion.Count, expansionIds.Count);
            Assert.Equal(5, primaryIds.Intersect(expansionIds, StringComparer.OrdinalIgnoreCase).Count());
            Assert.Equal(43, primaryIds.Union(expansionIds, StringComparer.OrdinalIgnoreCase).Count());
        }

        [Fact]
        public void LayeredDefinitions_PreservePrimaryDuplicates_AndRetainUniqueNpcArcs()
        {
            var primary = LoadFile("radio_distress_signals.json");
            var expansion = LoadFile("radio_distress_signals_expansion.json");
            var system = new RadioDistressSystem();

            // Mirrors production composition: fallback built-ins, expansion,
            // then primary authority.
            system.LoadFromJson(JsonSerializer.Serialize(new
            {
                schema_version = 1,
                radio_broadcasts = expansion
            }, SystemTextJsonSerializer.Options));
            system.LoadFromJson(JsonSerializer.Serialize(new
            {
                schema_version = 1,
                radio_broadcasts = primary
            }, SystemTextJsonSerializer.Options));

            Assert.Equal("The Pianist's Last Broadcast",
                system.GetDefinition("freq_distress_55_1")!.SourceName);
            Assert.Equal("Checkpoint Kilo Automated Beacon",
                system.GetDefinition("freq_distress_217_4")!.SourceName);
            Assert.Equal("Almshouse Emergency Handset",
                system.GetDefinition("freq_distress_142_8")!.SourceName);
            Assert.Equal("npc_ilze_kaar",
                system.GetDefinition("freq_distress_142_8")!.NpcId);

            var npcArcIds = new[]
            {
                "freq_distress_104_7",
                "freq_distress_128_5",
                "freq_distress_142_8",
                "freq_distress_166_2"
            };
            foreach (string id in npcArcIds)
            {
                var definition = system.GetDefinition(id);
                Assert.NotNull(definition);
                Assert.NotNull(definition!.NpcId);
                Assert.NotEmpty(definition.NpcId);
                Assert.NotEmpty(definition.ResolveQuestId);
            }

            // 43 JSON identities (36 landed + 7 §24 expansion scenarios) plus
            // four built-in compatibility fallbacks that exist in neither layer.
            Assert.Equal(47, system.TotalRegisteredSignals);
        }

        [Fact]
        public void AllLandedLayers_HaveContiguousStrictlyImprovingFragments()
        {
            foreach (var signal in LoadFile("radio_distress_signals.json")
                .Concat(LoadFile("radio_distress_signals_expansion.json")))
            {
                Assert.Equal(signal.DaysToTrace, signal.MessageFragments.Count);
                Assert.Equal(
                    Enumerable.Range(1, signal.DaysToTrace),
                    signal.MessageFragments.Select(fragment => fragment.Day));

                for (int i = 1; i < signal.MessageFragments.Count; i++)
                {
                    Assert.True(
                        signal.MessageFragments[i].Clarity > signal.MessageFragments[i - 1].Clarity,
                        $"{signal.FrequencyId} clarity must strictly increase at day {signal.MessageFragments[i].Day}");
                }
            }
        }
    }
}
