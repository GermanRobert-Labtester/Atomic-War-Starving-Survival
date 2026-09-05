// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests
{
    public class PatrolEncounterFullRegressionTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public PatrolEncounterFullRegressionTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);
        }

        [Fact]
        public void CatalogBaseline_ReconcilesToExact55Encounters()
        {
            var encounters = _catalog.Encounters.ToList();
            Assert.Equal(55, encounters.Count);

            int patrolCount = encounters.Count(e => e.Id.StartsWith("enc_patrol_"));
            int nonPatrolCount = encounters.Count - patrolCount;

            Assert.Equal(19, patrolCount);
            Assert.Equal(36, nonPatrolCount);
        }

        [Fact]
        public void RegionalScenarios_SurfaceExpectedArchetypes()
        {
            var sys = new TravelEncounterSystem(_catalog);

            // 1. high_scarp: Checkpoint variants eligible
            var highScarpEncounters = _catalog.Encounters
                .Where(e => sys.IsEncounterEligible(e, "high_scarp", 1.0f, "all", 1))
                .Select(e => e.Id)
                .ToList();

            Assert.Contains("enc_patrol_garrison_checkpoint", highScarpEncounters);
            Assert.Contains("enc_patrol_garrison_checkpoint_v2", highScarpEncounters);
            Assert.Contains("enc_patrol_garrison_checkpoint_v3", highScarpEncounters);

            // 2. the_toll: Warlord raids and warlord press gang eligible
            var tollEncounters = _catalog.Encounters
                .Where(e => sys.IsEncounterEligible(e, "the_toll", 3.0f, "all", 1))
                .Select(e => e.Id)
                .ToList();

            Assert.Contains("enc_patrol_warlord_raid", tollEncounters);
            Assert.Contains("enc_patrol_warlord_raid_v2", tollEncounters);
            Assert.Contains("enc_patrol_warlord_raid_v3", tollEncounters);
            Assert.Contains("enc_patrol_warlord_press_gang", tollEncounters);

            // 3. industrial_belt: Railway convoy and warlord raids eligible
            var industrialEncounters = _catalog.Encounters
                .Where(e => sys.IsEncounterEligible(e, "industrial_belt", 2.5f, "all", 1))
                .Select(e => e.Id)
                .ToList();

            Assert.Contains("enc_patrol_foundry_supply", industrialEncounters);
            Assert.Contains("enc_patrol_warlord_raid", industrialEncounters);
        }

        [Fact]
        public void ChoiceResolution_Atomicity_SufficientVsInsufficientInventory()
        {
            var war = new FactionWarSystem();
            var inv = new Inventory.Inventory { Capacity = 50, MaxWeight = 500f };
            // Checkpoint toll costs 2 canned_food
            inv.TryProduce("canned_food", 1); // Only 1 available (insufficient)

            var sys = new TravelEncounterSystem(_catalog, inv, war);
            int initialStanding = war.GetStanding("iron_garrison");

            // Attempt resolution with insufficient food
            bool failedOk = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var failRes);
            Assert.False(failedOk);
            Assert.Null(failRes);

            // Inventory unchanged (atomicity: 0 items deducted)
            Assert.Equal(1, inv.CountById("canned_food"));
            // Standing unchanged
            Assert.Equal(initialStanding, war.GetStanding("iron_garrison"));
            // No cooldown set
            Assert.Equal(0, sys.GetCooldownExpiry("patrol_garrison_checkpoint"));

            // Now provide the second canned food (total 2: sufficient)
            inv.TryProduce("canned_food", 1);
            Assert.Equal(2, inv.CountById("canned_food"));

            bool successOk = sys.ResolveChoice("enc_patrol_garrison_checkpoint", "choice_pay_garrison_toll", 1, out var succRes);
            Assert.True(successOk);
            Assert.NotNull(succRes);

            // 2 items deducted, 0 remain
            Assert.Equal(0, inv.CountById("canned_food"));
            // Standing updated (+1)
            Assert.Equal(initialStanding + 1, war.GetStanding("iron_garrison"));
            // Cooldown set to day 6
            Assert.Equal(6, sys.GetCooldownExpiry("patrol_garrison_checkpoint"));
        }

        [Fact]
        public void LegacyEncounters_NonRegression()
        {
            string[] legacyCreatureIds = new[]
            {
                "enc_travel_wolf_pack_crossing",
                "enc_travel_slag_beetle_slag_heap",
                "enc_travel_timber_tick_canopy",
                "enc_travel_bristleback_charge"
            };

            foreach (var id in legacyCreatureIds)
            {
                var enc = _catalog.GetEncounter(id);
                Assert.NotNull(enc);
                Assert.NotEqual("Human", enc!.Category);
                Assert.True(enc.Choices.Count >= 2);
                Assert.True(string.IsNullOrEmpty(enc.CooldownGroup));
            }
        }

        [Fact]
        public void RouteTrace_10Pass_DeterministicReplay()
        {
            List<string> RunRouteTrace(int seed)
            {
                var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
                inv.TryProduce("canned_food", 50);
                var sys = new TravelEncounterSystem(_catalog, inv);
                var rng = new SeededRng(seed);
                var trace = new List<string>();

                string[] routeRegions = new[] { "high_scarp", "the_toll", "industrial_belt" };

                for (int day = 1; day <= 10; day++)
                {
                    string region = routeRegions[(day - 1) % routeRegions.Length];
                    var eligible = _catalog.Encounters
                        .Where(e => sys.IsEncounterEligible(e, region, 2.0f, "all", day))
                        .OrderBy(e => e.Id)
                        .ToList();

                    if (eligible.Count == 0) continue;

                    int pick = (int)(rng.NextDouble() * eligible.Count);
                    if (pick >= eligible.Count) pick = eligible.Count - 1;
                    var enc = eligible[pick];
                    var choice = enc.Choices[0];

                    sys.ResolveChoice(enc.Id, choice.ChoiceId, day, out _);
                    trace.Add($"{day}:{enc.Id}:{choice.ChoiceId}");
                }

                return trace;
            }

            var trace1 = RunRouteTrace(424242);
            var trace2 = RunRouteTrace(424242);

            Assert.Equal(trace1.Count, trace2.Count);
            for (int i = 0; i < trace1.Count; i++)
            {
                Assert.Equal(trace1[i], trace2[i]);
            }
        }
    }
}
