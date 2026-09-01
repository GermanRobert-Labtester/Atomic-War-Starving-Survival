// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class TradeSpecialtySystemTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        [Fact]
        public void Load_Loads16SpecialtiesFromCatalog()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var items = TradeSpecialtyCatalogLoader.Load(dataDir, fileIO, json);
            Assert.Equal(16, items.Count);
        }

        [Fact]
        public void LoadAndRegister_RegistersAll16ProfessionsInSystem()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new TradeSpecialtySystem();

            int count = TradeSpecialtyCatalogLoader.LoadAndRegister(system, dataDir, fileIO, json);
            Assert.Equal(16, count);

            // Verify a new profession from Plan 26 expansion works
            bool milestoneFired = false;
            system.OnSpecialtyMilestone += (surv, prof, tier) =>
            {
                if (prof == "water_technician") milestoneFired = true;
            };

            system.OnItemCrafted("survivor_elena", "water_technician", "item_filter_charcoal");
            Assert.True(milestoneFired);
            Assert.Equal(1, system.GetMasteryTier("survivor_elena"));
        }

        [Fact]
        public void SpecialtyProgression_Completing3MilestonesGrantsMastery()
        {
            var system = new TradeSpecialtySystem();
            bool mastered = false;
            system.OnSpecialtyMastered += (surv, prof) =>
            {
                if (surv == "survivor_1" && prof == "electrician") mastered = true;
            };

            system.OnItemCrafted("survivor_1", "electrician", "item_battery_cell");
            Assert.Equal(1, system.GetMasteryTier("survivor_1"));
            Assert.False(system.HasMasteredTrade("survivor_1"));

            system.OnItemCrafted("survivor_1", "electrician", "item_solar_cell");
            Assert.Equal(2, system.GetMasteryTier("survivor_1"));
            Assert.False(system.HasMasteredTrade("survivor_1"));

            system.OnItemCrafted("survivor_1", "electrician", "item_advanced_circuit");
            Assert.Equal(3, system.GetMasteryTier("survivor_1"));
            Assert.True(system.HasMasteredTrade("survivor_1"));
            Assert.True(mastered);
        }
    }
}
