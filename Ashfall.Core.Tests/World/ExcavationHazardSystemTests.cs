// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class ExcavationHazardSystemTests
    {
        private static string GetExcavationCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""mitigations"": [
    {
      ""id"": ""mitigation_ventilation_blower_install"",
      ""display_name"": ""Forced-Air Ventilation Blower"",
      ""hazard_tags"": [""methane""],
      ""required_items"": [
        { ""item_id"": ""iron_pipe"", ""amount"": 2 },
        { ""item_id"": ""mechanical_parts"", ""amount"": 2 }
      ],
      ""labor_ticks"": 100,
      ""effect"": { ""methane_vent_rate_permille"": 300 },
      ""requires_respiratory_protection"": false,
      ""tags"": [""ventilation"", ""installed""]
    },
    {
      ""id"": ""mitigation_chemical_spore_scrub"",
      ""display_name"": ""Biocide Spore Decontamination"",
      ""hazard_tags"": [""spores""],
      ""required_items"": [
        { ""item_id"": ""chemicals"", ""amount"": 2 },
        { ""item_id"": ""clean_water"", ""amount"": 1 }
      ],
      ""labor_ticks"": 90,
      ""effect"": { ""spore_reduction_permille"": 500 },
      ""requires_respiratory_protection"": true,
      ""tags"": [""biocide""]
    },
    {
      ""id"": ""mitigation_timber_shoring_reinforcement"",
      ""display_name"": ""Heavy Timber Strut Shoring"",
      ""hazard_tags"": [""shoring""],
      ""required_items"": [
        { ""item_id"": ""scrap_wood"", ""amount"": 6 },
        { ""item_id"": ""scrap_metal"", ""amount"": 2 }
      ],
      ""labor_ticks"": 120,
      ""effect"": { ""shoring_health_restore_permille"": 400 },
      ""requires_respiratory_protection"": false,
      ""tags"": [""shoring""]
    },
    {
      ""id"": ""mitigation_sky_armor_blast_matting"",
      ""display_name"": ""Blast Matting & Sandbag Curtain"",
      ""hazard_tags"": [""cave_in""],
      ""required_items"": [
        { ""item_id"": ""cloth"", ""amount"": 4 },
        { ""item_id"": ""scrap_metal"", ""amount"": 3 }
      ],
      ""labor_ticks"": 110,
      ""effect"": { ""collapse_risk_reduction_permille"": 450 },
      ""requires_respiratory_protection"": false,
      ""tags"": [""blast_matting"", ""installed""]
    }
  ]
}";
        }

        private static ExcavationHazardSystem CreateSystem(
            out Inventory.Inventory inv,
            int seed = 42)
        {
            var rng = new SeededRng(seed);
            inv = new Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
            var excavation = new ExcavationSystem(rng);
            var skyArmor = new SkyLayerArmorSystem();

            var system = new ExcavationHazardSystem(inv, rng, excavation, skyArmor);
            system.LoadCatalog(GetExcavationCatalogJson());
            return system;
        }

        [Fact]
        public void VentilationBlower_InstallsAndReducesMethane()
        {
            var system = CreateSystem(out var inv);
            var sector = system.GetOrCreateSector("sector_1");
            sector.MethanePpm = 2000;

            inv.Add(new ItemDefinition { id = "iron_pipe" }, 2);
            inv.Add(new ItemDefinition { id = "mechanical_parts" }, 2);

            var res = system.TryApplyMitigation("sector_1", "mitigation_ventilation_blower_install");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            Assert.True(sector.MethanePpm < 2000);
            Assert.Contains("mitigation_ventilation_blower_install", sector.InstalledMitigationIds);
        }

        [Fact]
        public void SporeScrub_RequiresGasMaskInInventory()
        {
            var system = CreateSystem(out var inv);
            var sector = system.GetOrCreateSector("sector_spores");
            sector.SporeConcentrationPermille = 600;

            inv.Add(new ItemDefinition { id = "chemicals" }, 5);
            inv.Add(new ItemDefinition { id = "clean_water" }, 5);
            // Missing gas_mask

            var failRes = system.TryApplyMitigation("sector_spores", "mitigation_chemical_spore_scrub");
            Assert.NotEqual(ActionResult.StatusKind.Success, failRes.Status);

            // Add gas mask
            inv.Add(new ItemDefinition { id = "gas_mask" }, 1);
            var okRes = system.TryApplyMitigation("sector_spores", "mitigation_chemical_spore_scrub");
            Assert.Equal(ActionResult.StatusKind.Success, okRes.Status);
            Assert.Equal(100, sector.SporeConcentrationPermille); // 600 - 500
        }

        [Fact]
        public void ShoringReinforcement_RestoresShoringHealth()
        {
            var system = CreateSystem(out var inv);
            var sector = system.GetOrCreateSector("sector_deep");
            sector.ShoringHealthPermille = 300;

            inv.Add(new ItemDefinition { id = "scrap_wood" }, 6);
            inv.Add(new ItemDefinition { id = "scrap_metal" }, 2);

            var res = system.TryApplyMitigation("sector_deep", "mitigation_timber_shoring_reinforcement");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            Assert.Equal(700, sector.ShoringHealthPermille); // 300 + 400
        }

        [Fact]
        public void BulkheadToggle_BlocksSealingWhenMinersTrapped()
        {
            var system = CreateSystem(out _);
            system.TriggerCaveInRescue("sector_cave", new[] { "survivor_1" }, 3, 200);

            var sealRes = system.TryToggleBulkhead("sector_cave", true, out var reason);
            Assert.NotEqual(ActionResult.StatusKind.Success, sealRes.Status);
            Assert.Equal("trapped_miners", sealRes.FailureCode);
        }

        [Fact]
        public void TrappedMinersRescue_CompletesSuccessfullyWithLabor()
        {
            var system = CreateSystem(out _);
            system.TriggerCaveInRescue("sector_mine", new[] { "survivor_1", "survivor_2" }, 3, 200);

            var sector = system.GetOrCreateSector("sector_mine");
            Assert.Equal(2, sector.ActiveTrappedMiners.Count);
            Assert.False(sector.RescueCompleted);

            system.ProgressRescueLabor("sector_mine", 200);

            Assert.True(sector.RescueCompleted);
            Assert.Empty(sector.ActiveTrappedMiners);
        }

        [Fact]
        public void TrappedMinersRescue_FailsWhenDeadlineExceeded()
        {
            var system = CreateSystem(out _);
            system.TriggerCaveInRescue("sector_mine", new[] { "survivor_1" }, 2, 200); // 2 days deadline (day 0 + 2 = 2)

            var sector = system.GetOrCreateSector("sector_mine");
            system.TickDay(1);
            Assert.False(sector.RescueFailed);

            system.TickDay(3); // Day 3 > Day 2
            Assert.True(sector.RescueFailed);
        }

        [Fact]
        public void BlastMatting_ReducesCollapseRisk()
        {
            var system = CreateSystem(out var inv);
            var sector = system.GetOrCreateSector("sector_mat");
            sector.ShoringHealthPermille = 200; // Low shoring

            var (riskBefore, _, _) = system.EvaluateOperationRisk("sector_mat");

            inv.Add(new ItemDefinition { id = "cloth" }, 4);
            inv.Add(new ItemDefinition { id = "scrap_metal" }, 3);
            system.TryApplyMitigation("sector_mat", "mitigation_sky_armor_blast_matting");

            var (riskAfter, _, _) = system.EvaluateOperationRisk("sector_mat");
            Assert.True(riskAfter < riskBefore);
        }

        [Fact]
        public void SaveRestore_PreservesSectorHazardsAndInstalledMitigations()
        {
            var system = CreateSystem(out var inv);
            var sector = system.GetOrCreateSector("sector_save");
            sector.MethanePpm = 4500;
            sector.ShoringHealthPermille = 800;

            inv.Add(new ItemDefinition { id = "iron_pipe" }, 2);
            inv.Add(new ItemDefinition { id = "mechanical_parts" }, 2);
            system.TryApplyMitigation("sector_save", "mitigation_ventilation_blower_install");

            var save = system.CaptureState();
            var system2 = CreateSystem(out _);
            system2.RestoreState(save);

            var restored = system2.GetOrCreateSector("sector_save");
            Assert.Equal(sector.MethanePpm, restored.MethanePpm);
            Assert.Contains("mitigation_ventilation_blower_install", restored.InstalledMitigationIds);
        }

        [Fact]
        public void DeterministicReplay_YieldsIdenticalHazardEvolution()
        {
            var sysA = CreateSystem(out _, seed: 777);
            var sysB = CreateSystem(out _, seed: 777);

            sysA.GetOrCreateSector("sec_A").MethanePpm = 1000;
            sysB.GetOrCreateSector("sec_A").MethanePpm = 1000;

            sysA.TickDay(1);
            sysB.TickDay(1);

            Assert.Equal(sysA.GetOrCreateSector("sec_A").MethanePpm,
                         sysB.GetOrCreateSector("sec_A").MethanePpm);
        }
    }
}
