// SPDX-License-Identifier: MIT
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
    public class TravelEncounterPatrolVariantTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;

        public TravelEncounterPatrolVariantTests()
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
        public void CheckpointVariants_MechanicalEquivalence()
        {
            var v1 = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            var v2 = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v2")!;
            var v3 = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v3")!;

            Assert.NotNull(v1);
            Assert.NotNull(v2);
            Assert.NotNull(v3);

            var family = new[] { v1, v2, v3 };
            foreach (var member in family)
            {
                Assert.Equal("Human", member.Category);
                Assert.Equal("iron_garrison", member.FactionId);
                Assert.Equal("controlled", member.TerritoryState);
                Assert.Equal("patrol_garrison_checkpoint", member.CooldownGroup);
                Assert.Equal(0.5f, member.MinDangerLevel);
                Assert.Equal(2.0f, member.MaxDangerLevel);
                Assert.Equal(v1.RegionTags, member.RegionTags);
                Assert.Equal(v1.SeasonTags, member.SeasonTags);
                Assert.Equal(v1.Choices.Count, member.Choices.Count);

                for (int i = 0; i < v1.Choices.Count; i++)
                {
                    var c1 = v1.Choices[i];
                    var cM = member.Choices[i];
                    Assert.Equal(c1.ChoiceId, cM.ChoiceId);
                    Assert.Equal(c1.IsNonviolent, cM.IsNonviolent);
                    Assert.Equal(c1.IsAvoidance, cM.IsAvoidance);
                    Assert.Equal(c1.MoraleDelta, cM.MoraleDelta);
                    Assert.Equal(c1.GuiltDelta, cM.GuiltDelta);
                    Assert.Equal(c1.FactionId, cM.FactionId);
                    Assert.Equal(c1.FactionStandingDelta, cM.FactionStandingDelta);
                    Assert.Equal(c1.RequiredItemId, cM.RequiredItemId);
                    Assert.Equal(c1.RequiredItemQuantity, cM.RequiredItemQuantity);
                    Assert.Equal(c1.CostItems, cM.CostItems);
                }
            }

            // Titles and descriptions must differ (presentation variants)
            Assert.NotEqual(v1.Title, v2.Title);
            Assert.NotEqual(v1.Title, v3.Title);
            Assert.NotEqual(v1.Description, v2.Description);
            Assert.NotEqual(v1.Description, v3.Description);
        }

        [Fact]
        public void WarlordRaidVariants_MechanicalEquivalence()
        {
            var v1 = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var v2 = _catalog.GetEncounter("enc_patrol_warlord_raid_v2")!;
            var v3 = _catalog.GetEncounter("enc_patrol_warlord_raid_v3")!;

            Assert.NotNull(v1);
            Assert.NotNull(v2);
            Assert.NotNull(v3);

            var family = new[] { v1, v2, v3 };
            foreach (var member in family)
            {
                Assert.Equal("Human", member.Category);
                Assert.Equal("warlords_sector_4", member.FactionId);
                Assert.Equal("contested", member.TerritoryState);
                Assert.Equal("patrol_warlord_raid", member.CooldownGroup);
                Assert.Equal(2.0f, member.MinDangerLevel);
                Assert.Equal(5.0f, member.MaxDangerLevel);
                Assert.Equal(v1.RegionTags, member.RegionTags);
                Assert.Equal(v1.SeasonTags, member.SeasonTags);
                Assert.Equal(v1.Choices.Count, member.Choices.Count);

                for (int i = 0; i < v1.Choices.Count; i++)
                {
                    var c1 = v1.Choices[i];
                    var cM = member.Choices[i];
                    Assert.Equal(c1.ChoiceId, cM.ChoiceId);
                    Assert.Equal(c1.IsNonviolent, cM.IsNonviolent);
                    Assert.Equal(c1.IsAvoidance, cM.IsAvoidance);
                    Assert.Equal(c1.MoraleDelta, cM.MoraleDelta);
                    Assert.Equal(c1.GuiltDelta, cM.GuiltDelta);
                    Assert.Equal(c1.FactionId, cM.FactionId);
                    Assert.Equal(c1.FactionStandingDelta, cM.FactionStandingDelta);
                    Assert.Equal(c1.RequiredItemId, cM.RequiredItemId);
                    Assert.Equal(c1.RequiredItemQuantity, cM.RequiredItemQuantity);
                    Assert.Equal(c1.CostItems, cM.CostItems);
                }
            }

            Assert.NotEqual(v1.Title, v2.Title);
            Assert.NotEqual(v1.Title, v3.Title);
            Assert.NotEqual(v1.Description, v2.Description);
            Assert.NotEqual(v1.Description, v3.Description);
        }

        [Fact]
        public void PatrolVariants_NormalizedWeights_PreventWeightTripling()
        {
            var v1Check = _catalog.GetEncounter("enc_patrol_garrison_checkpoint")!;
            var v2Check = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v2")!;
            var v3Check = _catalog.GetEncounter("enc_patrol_garrison_checkpoint_v3")!;

            Assert.Equal(0.6f, v1Check.BaseWeight, 3);
            Assert.Equal(0.6f, v2Check.BaseWeight, 3);
            Assert.Equal(0.6f, v3Check.BaseWeight, 3);
            Assert.Equal(1.8f, v1Check.BaseWeight + v2Check.BaseWeight + v3Check.BaseWeight, 3);

            var v1Raid = _catalog.GetEncounter("enc_patrol_warlord_raid")!;
            var v2Raid = _catalog.GetEncounter("enc_patrol_warlord_raid_v2")!;
            var v3Raid = _catalog.GetEncounter("enc_patrol_warlord_raid_v3")!;

            Assert.Equal(0.5f, v1Raid.BaseWeight, 3);
            Assert.Equal(0.5f, v2Raid.BaseWeight, 3);
            Assert.Equal(0.5f, v3Raid.BaseWeight, 3);
            Assert.Equal(1.5f, v1Raid.BaseWeight + v2Raid.BaseWeight + v3Raid.BaseWeight, 3);
        }

        [Fact]
        public void RepetitionSimulation_50Days_NoCooldownViolation()
        {
            var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
            inv.TryProduce("canned_food", 100);
            var sys = new TravelEncounterSystem(_catalog, inv);
            var rng = new SeededRng(12345);

            int lastCheckpointDay = -100;
            int lastRaidDay = -100;

            for (int day = 1; day <= 50; day++)
            {
                // Select an eligible encounter in "high_scarp" or "the_toll"
                string region = (day % 2 == 0) ? "high_scarp" : "the_toll";
                var eligible = _catalog.Encounters
                    .Where(e => sys.IsEncounterEligible(e, region, 2.0f, "all", day))
                    .ToList();

                if (eligible.Count == 0) continue;

                int index = (int)(rng.NextDouble() * eligible.Count);
                if (index >= eligible.Count) index = eligible.Count - 1;
                var selected = eligible[index];

                if (selected.CooldownGroup == "patrol_garrison_checkpoint")
                {
                    Assert.True(day - lastCheckpointDay >= 5,
                        $"Checkpoint variant occurred on day {day}, but previous occurred on day {lastCheckpointDay} (< 5 days gap)!");
                    lastCheckpointDay = day;
                }
                else if (selected.CooldownGroup == "patrol_warlord_raid")
                {
                    Assert.True(day - lastRaidDay >= 5,
                        $"Warlord raid variant occurred on day {day}, but previous occurred on day {lastRaidDay} (< 5 days gap)!");
                    lastRaidDay = day;
                }

                // Resolve choice
                var choice = selected.Choices[0];
                sys.ResolveChoice(selected.Id, choice.ChoiceId, day, out _);
            }
        }

        [Fact]
        public void RepetitionSimulation_MultipleRuns_PresentationVariety()
        {
            var seenCheckpoints = new HashSet<string>();
            var seenRaids = new HashSet<string>();

            // Run 10 independent passes with different seeds
            for (int run = 1; run <= 10; run++)
            {
                var inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 1000f };
                inv.TryProduce("canned_food", 100);
                var sys = new TravelEncounterSystem(_catalog, inv);
                var rng = new SeededRng(run * 9999 + 42);

                for (int day = 1; day <= 50; day++)
                {
                    string region = (day % 2 == 0) ? "high_scarp" : "the_toll";
                    var eligible = _catalog.Encounters
                        .Where(e => sys.IsEncounterEligible(e, region, 2.0f, "all", day))
                        .ToList();

                    if (eligible.Count == 0) continue;

                    int index = (int)(rng.NextDouble() * eligible.Count);
                    if (index >= eligible.Count) index = eligible.Count - 1;
                    var selected = eligible[index];

                    if (selected.CooldownGroup == "patrol_garrison_checkpoint")
                        seenCheckpoints.Add(selected.Id);
                    else if (selected.CooldownGroup == "patrol_warlord_raid")
                        seenRaids.Add(selected.Id);

                    var choice = selected.Choices[0];
                    sys.ResolveChoice(selected.Id, choice.ChoiceId, day, out _);
                }
            }

            // Variety verification: all 3 presentation variants appear across seeds
            Assert.Contains("enc_patrol_garrison_checkpoint", seenCheckpoints);
            Assert.Contains("enc_patrol_garrison_checkpoint_v2", seenCheckpoints);
            Assert.Contains("enc_patrol_garrison_checkpoint_v3", seenCheckpoints);

            Assert.Contains("enc_patrol_warlord_raid", seenRaids);
            Assert.Contains("enc_patrol_warlord_raid_v2", seenRaids);
            Assert.Contains("enc_patrol_warlord_raid_v3", seenRaids);
        }
    }
}
