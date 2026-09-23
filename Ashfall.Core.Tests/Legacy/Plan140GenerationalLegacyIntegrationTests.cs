// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Xunit;

namespace Ashfall.Core.Tests.Legacy
{
    public sealed class Plan140GenerationalLegacyIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void LegacyTraitsCatalog_LoadsAuthoredTraits_Cleanly()
        {
            string dataDir = GetDataDir();
            var system = CampaignLegacySystem.LoadFromDirectory(dataDir, new FileSystemIO());

            Assert.NotNull(system.CatalogTraits);
            Assert.True(system.CatalogTraits.Count >= 20, $"Expected >= 20 legacy traits, found {system.CatalogTraits.Count}");

            Assert.True(system.TryGetTrait("trait_leaders_blood", out var leader));
            Assert.NotNull(leader);
            Assert.Equal("survivor", leader!.source);
            Assert.Equal("trait_dynasty_call", leader.evolution_target_id);

            Assert.True(system.TryGetTrait("trait_fortified_walls", out var walls));
            Assert.NotNull(walls);
            Assert.Equal("shelter", walls!.source);

            Assert.True(system.TryGetTrait("trait_old_alliance_vanguard", out var alliance));
            Assert.NotNull(alliance);
            Assert.Equal("faction", alliance!.source);
        }

        [Fact]
        public void ArchiveCampaign_InheritsShelterImprovements_AndFactionMemory()
        {
            string dataDir = GetDataDir();
            var system = CampaignLegacySystem.LoadFromDirectory(dataDir, new FileSystemIO(), new SeededRng(42));

            var campaign = new CampaignLegacy
            {
                campaignId = "run_01_alpha",
                endingId = "ending_stand_up",
                daysSurvived = 120,
                survivorCount = 14,
                deathsRecorded = 2,
                completionDay = 120,
                shelterImprovements = new List<string> { "reinforced_bulkhead_alpha", "deep_well_pump" },
                legacyTraits = new List<string> { "trait_fortified_walls", "trait_old_alliance_vanguard", "trait_stand_up_unity" }
            };

            system.ArchiveCampaign(campaign);

            Assert.Single(system.State.completedCampaigns);
            Assert.Equal(2, system.State.inheritedImprovements.Count);
            Assert.Contains("reinforced_bulkhead_alpha", system.State.inheritedImprovements);

            // Verify 100% inheritance of shelter, faction, and ending traits
            Assert.Contains(system.State.activeLegacyTraits, t => t.id == "trait_fortified_walls");
            Assert.Contains(system.State.activeLegacyTraits, t => t.id == "trait_old_alliance_vanguard");
            Assert.Contains(system.State.activeLegacyTraits, t => t.id == "trait_stand_up_unity");
        }

        [Fact]
        public void PrepareNewGameContext_AggregatesBonuses_ForNextRun()
        {
            string dataDir = GetDataDir();
            var system = CampaignLegacySystem.LoadFromDirectory(dataDir, new FileSystemIO());

            // Pre-seed active legacy traits
            system.State.activeLegacyTraits.Add(new LegacyTrait
            {
                id = "trait_hidden_cache",
                effect_type = "starting_supplies",
                magnitude = 25f
            });
            system.State.activeLegacyTraits.Add(new LegacyTrait
            {
                id = "trait_leaders_blood",
                effect_type = "morale_bonus",
                magnitude = 10f
            });
            system.State.activeLegacyTraits.Add(new LegacyTrait
            {
                id = "trait_fortified_walls",
                effect_type = "shelter_defense",
                magnitude = 20f
            });

            var newGameCtx = system.PrepareNewGameContext();

            Assert.NotNull(newGameCtx);
            Assert.Equal(25f, newGameCtx.startingResourcesBonus);
            Assert.Equal(10f, newGameCtx.startingMoraleBonus);
            Assert.Equal(20f, newGameCtx.startingDefenseBonus);
            Assert.Equal(3, newGameCtx.inheritedTraits.Count);
        }

        private sealed class ConstantRng : ISeededRng
        {
            private readonly double _val;
            public ConstantRng(double val) => _val = val;
            public int Seed => 42;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)_val;
            public double NextDouble() => _val;
        }

        [Fact]
        public void TraitEvolution_EvolvesTrait_AfterThreeGenerations()
        {
            string dataDir = GetDataDir();
            var system = CampaignLegacySystem.LoadFromDirectory(dataDir, new FileSystemIO(), new ConstantRng(0.1));

            string evolvedFrom = string.Empty;
            string evolvedTo = string.Empty;
            system.OnTraitEvolvedSeam = (from, to) =>
            {
                evolvedFrom = from;
                evolvedTo = to;
            };

            var rng = new ConstantRng(0.1);

            // Complete 3 campaigns in succession
            for (int i = 1; i <= 3; i++)
            {
                var run = new CampaignLegacy
                {
                    campaignId = $"run_{i}",
                    endingId = "ending_schedule_holds",
                    daysSurvived = 100,
                    survivorCount = 10,
                    completionDay = 100,
                    legacyTraits = new List<string> { "trait_leaders_blood" }
                };
                system.ArchiveCampaign(run, rng);
            }

            Assert.Equal(3, system.State.completedCampaigns.Count);
            Assert.Equal("trait_leaders_blood", evolvedFrom);
            Assert.Equal("trait_dynasty_call", evolvedTo);
            Assert.Contains(system.State.activeLegacyTraits, t => t.id == "trait_dynasty_call");
            Assert.DoesNotContain(system.State.activeLegacyTraits, t => t.id == "trait_leaders_blood");
        }

        [Fact]
        public void CampaignLegacySystem_CaptureRestore_PreservesFullHistory()
        {
            var system = new CampaignLegacySystem();
            system.ArchiveCampaign(new CampaignLegacy
            {
                campaignId = "run_test",
                endingId = "ending_test",
                daysSurvived = 50,
                completionDay = 50,
                shelterImprovements = new List<string> { "solar_test" }
            });

            var state = system.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.completedCampaigns);
            Assert.Single(state.inheritedImprovements);

            var restored = new CampaignLegacySystem();
            restored.RestoreState(state);

            Assert.Single(restored.State.completedCampaigns);
            Assert.Equal("run_test", restored.State.completedCampaigns[0].campaignId);
            Assert.Equal("solar_test", restored.State.inheritedImprovements[0]);
        }
    }
}
