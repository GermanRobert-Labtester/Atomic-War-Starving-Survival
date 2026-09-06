// SPDX-License-Identifier: MIT
// Deterministic offline balance harness for patrol recurrence tuning.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    public sealed class TravelEncounterBalanceSimulatorTests
    {
        private readonly TravelEncounterCatalog _catalog;

        public TravelEncounterBalanceSimulatorTests()
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(dataDir))
            {
                dataDir = Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _catalog = TravelEncounterCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
        }

        private sealed class BalanceResult
        {
            public int TotalSelections { get; init; }
            public int PatrolSelections { get; init; }
            public int CreatureSelections { get; init; }
            public int CheckpointSelections { get; init; }
            public int RaidSelections { get; init; }
            public Dictionary<string, int> Counts { get; init; } = new(StringComparer.OrdinalIgnoreCase);

            public string Summary =>
                $"total={TotalSelections}, patrol={PatrolSelections}, creature={CreatureSelections}, " +
                $"checkpoint={CheckpointSelections}, raid={RaidSelections}";
        }

        private BalanceResult Simulate(
            int seed,
            int days,
            string region,
            float danger,
            string stance)
        {
            var inventory = new Inventory.Inventory { Capacity = 1000, MaxWeight = 10000f };
            var allItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var encounter in _catalog.Encounters)
            {
                foreach (var choice in encounter.Choices ?? new List<TravelEncounterChoice>())
                {
                    if (!string.IsNullOrWhiteSpace(choice.RequiredItemId)) allItemIds.Add(choice.RequiredItemId);
                    foreach (var cost in choice.CostItems ?? new List<string>())
                    {
                        if (!string.IsNullOrWhiteSpace(cost)) allItemIds.Add(cost);
                    }
                }
            }
            foreach (string itemId in allItemIds) inventory.TryProduce(itemId, 1000);

            var system = new TravelEncounterSystem(_catalog, inventory);
            var rng = new SeededRng(seed);
            var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int patrol = 0;
            int creature = 0;
            int checkpoint = 0;
            int raid = 0;

            for (int day = 1; day <= days; day++)
            {
                var selected = system.SelectEncounter(region, danger, stance, "all", day, rng);
                if (selected == null) continue;

                counts[selected.Id] = counts.TryGetValue(selected.Id, out int count) ? count + 1 : 1;
                bool isPatrol = selected.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase);
                if (isPatrol) patrol++;
                if (string.Equals(selected.Category, "Creature", StringComparison.OrdinalIgnoreCase)) creature++;
                if (string.Equals(selected.PatrolArchetype, "checkpoint", StringComparison.OrdinalIgnoreCase)) checkpoint++;
                if (string.Equals(selected.PatrolArchetype, "raid_party", StringComparison.OrdinalIgnoreCase)) raid++;

                // Resolve one deterministic, currently available choice so
                // cooldowns are represented in the long-run simulation.
                var choice = (selected.Choices ?? new List<TravelEncounterChoice>())
                    .FirstOrDefault(c => system.EvaluateChoiceAvailability(selected, c, inventory).IsAvailable);
                if (choice != null)
                {
                    system.ResolveChoice(selected.Id, choice.ChoiceId, day, out _);
                }
            }

            return new BalanceResult
            {
                TotalSelections = counts.Values.Sum(),
                PatrolSelections = patrol,
                CreatureSelections = creature,
                CheckpointSelections = checkpoint,
                RaidSelections = raid,
                Counts = counts
            };
        }

        private (double Patrol, double Creature, double Checkpoint, double Raid) Aggregate(
            string region,
            float danger,
            string stance,
            int firstSeed = 1,
            int seedCount = 50)
        {
            int patrol = 0;
            int creature = 0;
            int checkpoint = 0;
            int raid = 0;
            for (int seed = firstSeed; seed < firstSeed + seedCount; seed++)
            {
                var result = Simulate(seed, 30, region, danger, stance);
                patrol += result.PatrolSelections;
                creature += result.CreatureSelections;
                checkpoint += result.CheckpointSelections;
                raid += result.RaidSelections;
            }
            return (
                patrol / (double)seedCount,
                creature / (double)seedCount,
                checkpoint / (double)seedCount,
                raid / (double)seedCount);
        }

        [Fact]
        public void SameSeedAndContext_ProducesIdenticalSelectionReport()
        {
            var first = Simulate(16605, 30, "high_scarp", 1.5f, "Balanced");
            var second = Simulate(16605, 30, "high_scarp", 1.5f, "Balanced");

            Assert.Equal(first.TotalSelections, second.TotalSelections);
            Assert.Equal(first.PatrolSelections, second.PatrolSelections);
            Assert.Equal(first.CreatureSelections, second.CreatureSelections);
            Assert.Equal(first.Counts.OrderBy(k => k.Key), second.Counts.OrderBy(k => k.Key));
        }

        [Fact]
        public void StanceTuning_RapidSuppressesPatrolsAndCautiousBoostsCheckpoints()
        {
            var rapid = Simulate(16606, 30, "high_scarp", 1.5f, "Rapid");
            var balanced = Simulate(16606, 30, "high_scarp", 1.5f, "Balanced");
            var cautious = Simulate(16606, 30, "high_scarp", 1.5f, "Cautious");

            Assert.True(rapid.PatrolSelections <= balanced.PatrolSelections,
                $"Rapid should not increase patrols: rapid={rapid.Summary}, balanced={balanced.Summary}");
            Assert.True(cautious.CheckpointSelections >= balanced.CheckpointSelections,
                $"Cautious should not reduce checkpoints: cautious={cautious.Summary}, balanced={balanced.Summary}");
        }

        [Fact]
        public void MultiSeedSweep_ProducesPatrolAndCreatureObservations()
        {
            int patrol = 0;
            int creature = 0;
            int total = 0;
            for (int seed = 1; seed <= 50; seed++)
            {
                var result = Simulate(seed, 30, "high_scarp", 1.5f, "Balanced");
                patrol += result.PatrolSelections;
                creature += result.CreatureSelections;
                total += result.TotalSelections;
            }

            Assert.True(total > 0, "Balance harness produced no selections.");
            Assert.True(patrol > 0, "Patrol corpus never won a weighted selection.");
            Assert.True(creature > 0, "Creature corpus never won a weighted selection.");
            Assert.True(patrol < total, $"Patrols dominate all selections: patrol={patrol}, total={total}");
            var contested = Simulate(16607, 30, "the_toll", 3f, "Balanced");
            var mixed = Simulate(16608, 30, "industrial_belt", 3f, "Balanced");
            var controlled = Simulate(16609, 30, "high_scarp", 1.5f, "Balanced");
            var global = Simulate(16610, 30, string.Empty, 3f, "Balanced");
            Assert.True(controlled.TotalSelections > 0);
            Assert.True(contested.TotalSelections > 0);
            Assert.True(mixed.TotalSelections > 0);
            Assert.True(global.TotalSelections > 0);
        }

        [Fact]
        public void ReleaseScenarioBands_AreStableAcrossSeeds()
        {
            var controlled = Aggregate("high_scarp", 1.5f, "Balanced");
            var contested = Aggregate("the_toll", 3f, "Balanced");
            var mixed = Aggregate(string.Empty, 3f, "Balanced");
            var rapid = Aggregate("high_scarp", 1.5f, "Rapid");
            var cautious = Aggregate("high_scarp", 1.5f, "Cautious");

            Assert.InRange(controlled.Checkpoint, 3.0, 5.0);
            Assert.InRange(contested.Raid, 2.0, 3.5);
            Assert.InRange(mixed.Patrol, 8.0, 15.0);
            Assert.True(rapid.Patrol <= controlled.Patrol,
                $"Rapid should suppress patrols: rapid={rapid.Patrol:F1}, balanced={controlled.Patrol:F1}");
            Assert.True(cautious.Checkpoint >= controlled.Checkpoint,
                $"Cautious should increase checkpoint recurrence: cautious={cautious.Checkpoint:F1}, balanced={controlled.Checkpoint:F1}");
        }
    }
}
