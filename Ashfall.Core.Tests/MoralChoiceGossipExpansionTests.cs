using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class MoralChoiceGossipExpansionTests : CatalogTestBase
    {
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        private static MoralChoiceGossipData Load() =>
            MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);

        [Fact]
        public void Catalog_HasExactlyTwentyOnePoolsAndFourHundredTwentyLines()
        {
            var data = Load();
            var pools = GetPools(data);

            Assert.Equal(21, pools.Count);
            Assert.Equal(420, pools.Sum(p => p.Lines.Count));

            foreach (var pool in pools)
            {
                Assert.Equal(20, pool.Lines.Count);
                Assert.All(pool.Lines, line => Assert.False(string.IsNullOrWhiteSpace(line), pool.Name));

                var normalized = pool.Lines.Select(Normalize).ToList();
                Assert.Equal(20, normalized.Distinct(StringComparer.Ordinal).Count());
            }
        }

        [Fact]
        public void SlightlyPositiveWhispers_AreLoadedAndRuntimeReachable()
        {
            var data = Load();
            var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(110));

            Assert.Equal(20, data.WhisperLines.SlightlyPositive.Count);
            Assert.Equal(20, runtime.GetWhisperLines(MoralPathBand.SlightlyPositive).Count);
            Assert.All(Enumerable.Range(0, 20), _ =>
                Assert.False(string.IsNullOrWhiteSpace(runtime.PickWhisper(MoralPathBand.SlightlyPositive))));
        }

        [Fact]
        public void GossipSelection_RemainsSeedDeterministicAcrossAllSectionsAndBands()
        {
            var data = Load();
            var left = new MoralChoiceGossipRuntime(data, new SeededRng(110));
            var right = new MoralChoiceGossipRuntime(data, new SeededRng(110));

            foreach (var band in Enum.GetValues<MoralPathBand>())
            {
                for (int i = 0; i < 5; i++)
                {
                    Assert.Equal(left.PickCampChatter(band), right.PickCampChatter(band));
                    Assert.Equal(left.PickNpcGreeting(band), right.PickNpcGreeting(band));
                    Assert.Equal(left.PickWhisper(band), right.PickWhisper(band));
                }
            }
        }

        [Fact]
        public void GossipLines_DoNotContainInternalContextIdentifiers()
        {
            var data = Load();
            foreach (var pool in GetPools(data))
            {
                foreach (var line in pool.Lines)
                {
                    Assert.DoesNotContain("quest_moral_", line, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain("moral_event_", line, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain("very_positive", line, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain("slightly_positive", line, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain("slightly_evil", line, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        private static List<GossipPool> GetPools(MoralChoiceGossipData data) => new List<GossipPool>
        {
            new GossipPool("camp_chatter.very_positive", data.CampChatter.VeryPositive),
            new GossipPool("camp_chatter.positive", data.CampChatter.Positive),
            new GossipPool("camp_chatter.slightly_positive", data.CampChatter.SlightlyPositive),
            new GossipPool("camp_chatter.neutral", data.CampChatter.Neutral),
            new GossipPool("camp_chatter.slightly_evil", data.CampChatter.SlightlyEvil),
            new GossipPool("camp_chatter.evil", data.CampChatter.Evil),
            new GossipPool("camp_chatter.very_evil", data.CampChatter.VeryEvil),
            new GossipPool("npc_greeting_shifts.very_positive", data.NpcGreetingShifts.VeryPositive),
            new GossipPool("npc_greeting_shifts.positive", data.NpcGreetingShifts.Positive),
            new GossipPool("npc_greeting_shifts.slightly_positive", data.NpcGreetingShifts.SlightlyPositive),
            new GossipPool("npc_greeting_shifts.neutral", data.NpcGreetingShifts.Neutral),
            new GossipPool("npc_greeting_shifts.slightly_evil", data.NpcGreetingShifts.SlightlyEvil),
            new GossipPool("npc_greeting_shifts.evil", data.NpcGreetingShifts.Evil),
            new GossipPool("npc_greeting_shifts.very_evil", data.NpcGreetingShifts.VeryEvil),
            new GossipPool("whisper_lines.very_positive", data.WhisperLines.VeryPositive),
            new GossipPool("whisper_lines.positive", data.WhisperLines.Positive),
            new GossipPool("whisper_lines.slightly_positive", data.WhisperLines.SlightlyPositive),
            new GossipPool("whisper_lines.neutral", data.WhisperLines.Neutral),
            new GossipPool("whisper_lines.slightly_evil", data.WhisperLines.SlightlyEvil),
            new GossipPool("whisper_lines.evil", data.WhisperLines.Evil),
            new GossipPool("whisper_lines.very_evil", data.WhisperLines.VeryEvil)
        };

        private static string Normalize(string line)
        {
            var characters = line.ToLowerInvariant()
                .Select(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) ? c : ' ')
                .ToArray();
            return string.Join(" ", new string(characters)
                .Split((char[])null!, StringSplitOptions.RemoveEmptyEntries));
        }

        private sealed class GossipPool
        {
            public GossipPool(string name, List<string> lines)
            {
                Name = name;
                Lines = lines;
            }

            public string Name { get; }
            public List<string> Lines { get; }
        }
    }
}
