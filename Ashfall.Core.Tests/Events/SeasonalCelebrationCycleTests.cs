// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Events;
using Xunit;
using PlayerInventory = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Events
{
    public sealed class SeasonalCelebrationCycleTests
    {
        [Fact]
        public void Hold_IsAtomicOncePerCycleAndPersistsItsOccurrence()
        {
            var system = new SeasonalCelebrationSystem();
            string json = System.IO.File.ReadAllText(System.IO.Path.GetFullPath(
                System.IO.Path.Combine(AppContext.BaseDirectory,
                    "../../../../Assets/StreamingAssets/Data/shelter_celebrations.json")));
            system.LoadCatalog(json);
            var inventory = new PlayerInventory { MaxWeight = 1000f };
            inventory.TryProduce("dried_rations", 3);
            inventory.TryProduce("fuel", 2);

            Assert.False(system.TryHoldCelebration(
                "hol_new_year", "small", 4, 1, "dried_rations", "fuel", inventory,
                new SeededRng(11), out _));
            Assert.Empty(system.History);
            Assert.Equal(3, inventory.CountById("dried_rations"));
            Assert.Equal(2, inventory.CountById("fuel"));

            inventory.TryProduce("dried_rations", 1);
            Assert.True(system.TryHoldCelebration(
                "hol_new_year", "small", 4, 1, "dried_rations", "fuel", inventory,
                new SeededRng(11), out var record));
            Assert.NotNull(record);
            Assert.Equal(0, inventory.CountById("dried_rations"));
            Assert.Equal(0, inventory.CountById("fuel"));
            Assert.False(system.TryHoldCelebration(
                "hol_new_year", "small", 4, 1, "dried_rations", "fuel", inventory,
                new SeededRng(11), out _));

            var restored = new SeasonalCelebrationSystem();
            restored.LoadCatalog(json);
            restored.RestoreState(system.CaptureState());
            Assert.True(restored.WasHolidayOccurrenceHeld("hol_new_year", 1));
            Assert.False(restored.WasHolidayOccurrenceHeld("hol_new_year", 361));

            inventory.TryProduce("dried_rations", 4);
            inventory.TryProduce("fuel", 2);
            Assert.True(restored.TryHoldCelebration(
                "hol_new_year", "small", 4, 361, "dried_rations", "fuel", inventory,
                new SeededRng(12), out _));
        }

        [Fact]
        public void Skip_IsConsumptiveWithinCycleAndAllowsNextYearOccurrence()
        {
            var system = new SeasonalCelebrationSystem();

            Assert.True(system.TrySkipHoliday("hol_midsummer_day", 180, out float penalty));
            Assert.Equal(-2f, penalty);
            Assert.True(system.WasHolidayOccurrenceSkipped("hol_midsummer_day", 180));
            Assert.False(system.TrySkipHoliday("hol_midsummer_day", 180, out _));
            Assert.False(system.WasHolidayOccurrenceSkipped("hol_midsummer_day", 540));

            Assert.True(system.TrySkipHoliday("hol_midsummer_day", 540, out _));
            Assert.True(system.WasHolidayOccurrenceSkipped("hol_midsummer_day", 540));
            var restored = new SeasonalCelebrationSystem();
            restored.RestoreState(system.CaptureState());
            Assert.True(restored.WasHolidayOccurrenceSkipped("hol_midsummer_day", 540));
        }
    }
}
