using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plans9497CatalogTests
    {
        private static string DataDirectory()
        {
            string? dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                dir = Path.GetDirectoryName(dir);
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found.");
        }

        [Fact]
        public void PlanCatalogs_LoadFromJsonAuthority()
        {
            string data = DataDirectory();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var grain = GrainProcessingCatalogLoader.Load(data, files, json);
            Assert.Equal(1, grain.schema_version);
            Assert.Contains(grain.recipes, r => r.output_item_id == "item_grain_flour");
            Assert.Contains(grain.silos, s => s.silo_id == "grain_silo_holdfast");

            var cryogenic = CryogenicAirSeparationCatalogLoader.Load(data, files, json);
            Assert.Equal(600f, cryogenic.required_power_watts);
            Assert.Contains(cryogenic.products, p => p.product_id == "item_oxygen_supply");
            Assert.Contains(cryogenic.products, p => p.product_id == "item_nitrogen_supply");

            var heliograph = HeliographCatalogLoader.Load(data, files, json);
            Assert.Contains(heliograph.stations, s => s.map_node_id == "loc_holdfast");
            Assert.Contains(heliograph.stations, s => s.map_node_id == "loc_hidden_relay_bunker");
        }

        [Fact]
        public void PlanCatalogs_ApplyToCoreAuthorities()
        {
            string data = DataDirectory();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var inventory = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            var grainSystem = new GrainProcessingSystem(inventory);
            grainSystem.LoadCatalog(GrainProcessingCatalogLoader.Load(data, files, json));
            inventory.AddById("crop_ash_grain", 2);
            Assert.True(grainSystem.StartMilling(
                "recipe_ash_grain_flour", "grain_silo_holdfast").IsSuccess);

            var cryogenicSystem = new CryogenicAirSeparationSystem(
                inventory, new SeededRng(4), () => 1000f);
            cryogenicSystem.LoadCatalog(
                CryogenicAirSeparationCatalogLoader.Load(data, files, json));
            Assert.True(cryogenicSystem.SetRunning(true));
            cryogenicSystem.TickDay(1);
            Assert.Equal(1, cryogenicSystem.State.cycles_completed);
            Assert.Equal(2, inventory.CountById("item_oxygen_supply"));
            Assert.Equal(3, inventory.CountById("item_nitrogen_supply"));

            var heliographSystem = new HeliographSystem(
                (_, _) => true, () => 1f, _ => true, _ => { }, _ => true);
            heliographSystem.LoadCatalog(
                HeliographCatalogLoader.Load(data, files, json));
            Assert.Equal(2, heliographSystem.State.stations.Count);
        }
    }
}
