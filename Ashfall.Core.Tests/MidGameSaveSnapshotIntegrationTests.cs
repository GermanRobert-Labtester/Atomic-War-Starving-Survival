using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Tests
{
    public class MidGameSaveSnapshotIntegrationTests
    {
        [Serializable]
        public class MidGameSaveSnapshot
        {
            public int Day { get; set; } = 50;
            public string CampaignSeed { get; set; } = "seed_midgame_50";
            public List<InventoryItemSave> Inventory { get; set; } = new List<InventoryItemSave>();
            public List<SurvivorSaveSlice> Survivors { get; set; } = new List<SurvivorSaveSlice>();
            public PowerGridSave Power { get; set; } = new PowerGridSave();
            public JournalSave Journal { get; set; } = new JournalSave();
            public string Checksum { get; set; } = string.Empty;
        }

        [Serializable]
        public class InventoryItemSave
        {
            public string Id { get; set; } = string.Empty;
            public int Quantity { get; set; }
        }

        [Serializable]
        public class SurvivorSaveSlice
        {
            public string Id { get; set; } = string.Empty;
            public float Health { get; set; }
            public float Hunger { get; set; }
            public float Thirst { get; set; }
            public float RadiationDose { get; set; }
        }

        private MidGameSaveSnapshot CreateSampleSnapshot(int day, int cannedFood, float chenHunger)
        {
            var snap = new MidGameSaveSnapshot
            {
                Day = day,
                CampaignSeed = $"seed_run_{day}",
                Inventory = new List<InventoryItemSave>
                {
                    new InventoryItemSave { Id = "item_canned_food", Quantity = cannedFood },
                    new InventoryItemSave { Id = "item_purified_water", Quantity = 14 },
                    new InventoryItemSave { Id = "item_fuel_canister", Quantity = 8 },
                    new InventoryItemSave { Id = "item_geiger_counter", Quantity = 1 }
                },
                Survivors = new List<SurvivorSaveSlice>
                {
                    new SurvivorSaveSlice { Id = "survivor_dr_sarah_chen", Health = 90f, Hunger = chenHunger, Thirst = 12f, RadiationDose = 5f },
                    new SurvivorSaveSlice { Id = "survivor_gunner_mikhail", Health = 75f, Hunger = 45f, Thirst = 30f, RadiationDose = 25f }
                },
                Power = new PowerGridSave
                {
                    State = new PowerGridState
                    {
                        GenerationWatts = 1200f,
                        FuelUnits = 45f,
                        BatteryCapacityWh = 8000f,
                        BatteryReserveWh = 6500f
                    }
                },
                Journal = new JournalSave
                {
                    NextSeq = 12,
                    HasUnread = false,
                    CodexUnlockCount = 5,
                    Entries = new[]
                    {
                        new JournalEntry { Id = "journal_1", Text = "Reached Day 50.", Day = 50, KnowledgeKey = "event_day_50" }
                    }
                }
            };
            snap.Checksum = SaveChecksum.Compute(snap);
            return snap;
        }

        [Fact]
        public void Day50_MidGameSnapshot_RoundTripsExactlyWithoutDataLoss()
        {
            var original = CreateSampleSnapshot(50, cannedFood: 25, chenHunger: 18.5f);
            var serializer = new SystemTextJsonSerializer();

            string json = serializer.Serialize(original);
            Assert.False(string.IsNullOrWhiteSpace(json));

            var restored = serializer.Deserialize<MidGameSaveSnapshot>(json);
            Assert.NotNull(restored);
            Assert.Equal(50, restored.Day);
            Assert.Equal(4, restored.Inventory.Count);
            Assert.Equal(25, restored.Inventory[0].Quantity);
            Assert.Equal("survivor_dr_sarah_chen", restored.Survivors[0].Id);
            Assert.Equal(18.5f, restored.Survivors[0].Hunger);
            Assert.Equal(1200f, restored.Power.State.GenerationWatts);
            Assert.Equal(original.Checksum, SaveChecksum.Compute(restored));
        }

        [Fact]
        public void SlotIsolation_SlotA_and_SlotB_DoNotContaminate()
        {
            var serializer = new SystemTextJsonSerializer();

            // Slot A: Day 10 save (10 canned food, 5 hunger)
            var slotA = CreateSampleSnapshot(10, cannedFood: 10, chenHunger: 5.0f);
            string jsonA = serializer.Serialize(slotA);

            // Slot B: Day 65 save (40 canned food, 75 hunger)
            var slotB = CreateSampleSnapshot(65, cannedFood: 40, chenHunger: 75.0f);
            string jsonB = serializer.Serialize(slotB);

            // Simulate loading Slot A, verifying, then loading Slot B, verifying, then reloading Slot A
            var loadedA1 = serializer.Deserialize<MidGameSaveSnapshot>(jsonA);
            Assert.Equal(10, loadedA1!.Day);
            Assert.Equal(10, loadedA1.Inventory[0].Quantity);
            Assert.Equal(5.0f, loadedA1.Survivors[0].Hunger);

            var loadedB = serializer.Deserialize<MidGameSaveSnapshot>(jsonB);
            Assert.Equal(65, loadedB!.Day);
            Assert.Equal(40, loadedB.Inventory[0].Quantity);
            Assert.Equal(75.0f, loadedB.Survivors[0].Hunger);

            var loadedA2 = serializer.Deserialize<MidGameSaveSnapshot>(jsonA);
            Assert.Equal(10, loadedA2!.Day);
            Assert.Equal(10, loadedA2.Inventory[0].Quantity);
            Assert.Equal(5.0f, loadedA2.Survivors[0].Hunger);
            Assert.Equal(loadedA1.Checksum, loadedA2.Checksum);
        }
    }
}
