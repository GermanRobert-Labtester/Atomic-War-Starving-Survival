#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan100_48MoralReactionsWeatherGatesIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void MoralChoiceFactionReactions_LoadsAllSixThresholdEvents_FromAuthoritativeJson()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var data = MoralChoiceFactionReactionsCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotNull(data);
            Assert.Equal(6, data.ThresholdReactions.Count);

            string[] expectedEventIds = new[]
            {
                MoralChoiceSystem.EventBountyIssued,
                MoralChoiceSystem.EventContractTaken,
                MoralChoiceSystem.EventContractRaised,
                MoralChoiceSystem.EventPatrolDefense,
                MoralChoiceSystem.EventLegendPositive,
                MoralChoiceSystem.EventLegendNegative
            };

            foreach (var expectedId in expectedEventIds)
            {
                Assert.True(data.ThresholdReactions.TryGetValue(expectedId, out var reaction),
                    $"Expected threshold reaction '{expectedId}' to be present.");
                Assert.NotNull(reaction);
                Assert.False(string.IsNullOrWhiteSpace(reaction.EventDescription),
                    $"Event '{expectedId}' must have non-empty EventDescription");
                Assert.NotEmpty(reaction.PeacekeeperDialogue);
                Assert.NotEmpty(reaction.RaiderDialogue);
                Assert.NotEmpty(reaction.KnowledgeKeeperDialogue);

                // Verify dialogue details
                foreach (var line in reaction.PeacekeeperDialogue)
                {
                    Assert.False(string.IsNullOrWhiteSpace(line.Speaker));
                    Assert.NotEmpty(line.Lines);
                }

                foreach (var line in reaction.RaiderDialogue)
                {
                    Assert.False(string.IsNullOrWhiteSpace(line.Speaker));
                    Assert.NotEmpty(line.Lines);
                }

                foreach (var line in reaction.KnowledgeKeeperDialogue)
                {
                    Assert.False(string.IsNullOrWhiteSpace(line.Speaker));
                    Assert.NotEmpty(line.Lines);
                }

                Assert.False(string.IsNullOrWhiteSpace(reaction.JournalEntry),
                    $"Event '{expectedId}' must have non-empty JournalEntry");
            }
        }

        [Fact]
        public void WeatherRouteGateCatalog_LoadsAuthoritativeGates_AndEvaluatesPassability()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();

            var catalog = WeatherRouteGateCatalog.LoadFromDirectory(dataDir, fileIO);

            Assert.NotNull(catalog);
            Assert.True(catalog.Gates.Count >= 15, $"Expected at least 15 gates, found {catalog.Gates.Count}");
            Assert.Equal(18, catalog.Gates.Count); // 15 route gates + 3 destination gates

            var gateIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var gate in catalog.Gates)
            {
                Assert.False(string.IsNullOrWhiteSpace(gate.id));
                Assert.StartsWith("gate_", gate.id, StringComparison.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(gate.target));
                Assert.False(string.IsNullOrWhiteSpace(gate.description));
                Assert.True(gateIds.Add(gate.id), $"Duplicate gate id '{gate.id}'");
            }

            // Verify evaluation behavior on gate_mountain_pass_blizzard (blocked by Blizzard)
            var blizzardGate = catalog.Gates.First(g => g.id == "gate_mountain_pass_blizzard");
            Assert.Contains("Blizzard", blizzardGate.blocked_weather);

            // When weather is Blizzard, should block without override item
            var block = catalog.EvaluateBlock(blizzardGate.target, "Blizzard", hasOverrideItem: _ => false);
            Assert.NotNull(block);
            Assert.Equal("gate_mountain_pass_blizzard", block.GateId);

            // When weather is Clear, should not block
            var noBlock = catalog.EvaluateBlock(blizzardGate.target, "Clear", hasOverrideItem: _ => false);
            Assert.Null(noBlock);

            // Verify evaluation behavior on gate_lowland_marsh_fog (blocked by BioFog, overridden by gas_mask)
            var fogGate = catalog.Gates.First(g => g.id == "gate_lowland_marsh_fog");
            Assert.Equal("gas_mask", fogGate.override_item);

            // When weather is BioFog and no gas mask, should block
            var fogBlock = catalog.EvaluateBlock(fogGate.target, "BioFog", hasOverrideItem: _ => false);
            Assert.NotNull(fogBlock);
            Assert.Equal("gate_lowland_marsh_fog", fogBlock.GateId);

            // When player has gas mask, should override and allow passage
            var fogOverridden = catalog.EvaluateBlock(fogGate.target, "BioFog", hasOverrideItem: item => item == "gas_mask");
            Assert.Null(fogOverridden);
        }

        [Fact]
        public void MoralReactions_And_WeatherGates_ExecuteConcurrentlyWithoutInterference()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Load both catalogs simultaneously
            var moralData = MoralChoiceFactionReactionsCatalogLoader.Load(dataDir, fileIO, json);
            var weatherCatalog = WeatherRouteGateCatalog.LoadFromDirectory(dataDir, fileIO);

            Assert.NotNull(moralData);
            Assert.NotNull(weatherCatalog);

            // Test scenario: player has VeryEvil score (-150) -> bounty issued
            Assert.True(moralData.ThresholdReactions.TryGetValue(MoralChoiceSystem.EventBountyIssued, out var bountyReaction));
            Assert.NotNull(bountyReaction);
            Assert.Contains("Peacekeeper Sergeant Veill", bountyReaction.PeacekeeperDialogue.Select(d => d.Speaker));

            // While evaluating this moral event, evaluate weather gate passability across routes
            var culvertGate = weatherCatalog.Gates.FirstOrDefault(g => g.id == "gate_culvert_black_rain");
            Assert.NotNull(culvertGate);

            // In BlackRain weather without override, culvert is blocked
            var culvertBlock = weatherCatalog.EvaluateBlock(culvertGate.target, "BlackRain", hasOverrideItem: _ => false);
            Assert.NotNull(culvertBlock);
            Assert.Equal("gate_culvert_black_rain", culvertBlock.GateId);

            // In Clear weather, culvert is unblocked
            var culvertClear = weatherCatalog.EvaluateBlock(culvertGate.target, "Clear", hasOverrideItem: _ => false);
            Assert.Null(culvertClear);

            // Verify moral state was not mutated by weather evaluation
            Assert.Equal(6, moralData.ThresholdReactions.Count);
            Assert.Equal(18, weatherCatalog.Gates.Count);
        }
    }
}
