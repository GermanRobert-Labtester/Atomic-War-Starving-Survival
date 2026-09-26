// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 142 — Clothing & Warmth: host integration and wiring tests.
// Core contract: equipped layers, condition-scaled and wetness-scaled cold
// mitigation, drying, census projection, and save/restore round-trip.
// Production wiring gates: the host binds NeedsSystem.ClothingWarmthReductionProvider,
// registers the clothing_warmth save section, and classifies the day heartbeat.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core.Campaign;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan142ClothingWarmthHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "src", "Main.ClothingWarmth.cs")))
                        return directory.FullName;
                    directory = directory.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        [Fact]
        public void Census_ProjectsTrackedSurvivorsEquippedItemsAndSoak()
        {
            var system = new ClothingWarmthSystem();
            system.EquipClothing("s1", "item_winter_coat");
            system.EquipClothing("s1", "item_fur_boots");
            system.EquipClothing("s2", "item_ragged_coat");
            system.ApplyWetness("s2", 0.9f);

            var census = system.GetCensus();

            Assert.Equal(2, census.SurvivorsTracked);
            Assert.Equal(3, census.TotalEquippedItems);
            Assert.Equal(1, census.SoakedSurvivors);
            Assert.Equal(8, census.ProfilesRegistered);
        }

        [Fact]
        public void ColdReduction_ScalesWithConditionAndWetness_AndRespectsCap()
        {
            var system = new ClothingWarmthSystem();
            system.EquipClothing("s1", "item_arctic_survival_suit", condition: 1.0f);
            float pristine = system.CalculateColdLossReduction("s1");
            Assert.InRange(pristine, 0.55f, ClothingWarmthSystem.MaxColdMitigation);

            system.DegradeCondition("s1", 480f);
            float degraded = system.CalculateColdLossReduction("s1");
            Assert.True(degraded < pristine);

            system.ApplyWetness("s1", 1.0f);
            float soaked = system.CalculateColdLossReduction("s1");
            // Arctic suit is waterproof, so wetness must not change its mitigation.
            Assert.Equal(degraded, soaked, 3);

            // No clothing => no reduction.
            Assert.Equal(0f, system.CalculateColdLossReduction("nobody"));
        }

        [Fact]
        public void DryClothing_LowersWetness()
        {
            var system = new ClothingWarmthSystem();
            system.EquipClothing("s1", "item_ragged_coat");
            system.ApplyWetness("s1", 1.0f);
            float wet = system.State.survivors["s1"].wetness;
            Assert.True(wet > 0.9f);

            system.DryClothing("s1", 2f);
            Assert.True(system.State.survivors["s1"].wetness < wet);
        }

        [Fact]
        public void CaptureRestore_RoundTripsLayersConditionAndWetness()
        {
            var system = new ClothingWarmthSystem();
            system.EquipClothing("s1", "item_hazmat_cold_suit", condition: 0.6f);
            system.EquipClothing("s1", "item_thermal_underwear", condition: 0.8f);
            system.ApplyWetness("s1", 0.4f);
            float expectedWetness = system.State.survivors["s1"].wetness;
            var saved = system.CaptureState();

            var restored = new ClothingWarmthSystem();
            restored.RestoreState(saved);

            var rec = restored.State.survivors["s1"];
            Assert.Equal(2, rec.equipped.Count);
            Assert.Equal(0.6f, rec.equipped.Find(e => e.item_id == "item_hazmat_cold_suit")!.condition, 3);
            Assert.Equal(expectedWetness, rec.wetness, 3);
        }

        [Fact]
        public void SaveSectionRegistry_RegistersClothingWarmthSection()
        {
            Assert.Contains(SaveSectionRegistry.All, s => s.SectionKey == "clothing_warmth");
            Assert.Equal("clothing_warmth_save.json", SaveSectionRegistry.SectionFileNames["clothing_warmth"]);
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesClothingWarmthHeartbeat()
        {
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind("clothing_warmth_ticked"));
        }

        [Fact]
        public void HostSource_BindsClothingWarmthReductionProvider()
        {
            string main = Source(Path.Combine("src", "Main.ClothingWarmth.cs"));
            Assert.Contains("ClothingWarmthReductionProvider", main);
            Assert.Contains("SetupClothingWarmth", main);
            Assert.Contains("SaveClothingWarmth", main);

            string needs = Source(Path.Combine("Assets", "Ashfall.Core", "Survivors", "NeedsSystem.cs"));
            Assert.Contains("ClothingWarmthReductionProvider", needs);
        }
    }
}
