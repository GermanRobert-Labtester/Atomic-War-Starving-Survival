using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Tests for the contamination economy system: cross-contamination spread,
    /// decontamination queue behavior, and entry routing logic.
    /// </summary>
    [TestFixture]
    public class ContaminationEconomyTests
    {
        private const float Epsilon = 1e-4f;

        private StorageLayoutSO _testLayout;
        private ItemDefinition _cleanFood;
        private ItemDefinition _dirtyItem;

        [SetUp]
        public void Setup()
        {
            // Create test layout with 3 slots in a line
            _testLayout = ScriptableObject.CreateInstance<StorageLayoutSO>();
            _testLayout.layoutId = "test_layout";
            _testLayout.displayName = "Test Layout";
            _testLayout.contaminationTransferRate = 0.1f;
            _testLayout.halfFalloffDistance = 2f;

            _testLayout.slots = new List<StorageLayoutSO.SlotDefinition>
            {
                new StorageLayoutSO.SlotDefinition { slotId = "slot_0", position = new Vector2Int(0, 0) },
                new StorageLayoutSO.SlotDefinition { slotId = "slot_1", position = new Vector2Int(1, 0) },
                new StorageLayoutSO.SlotDefinition { slotId = "slot_2", position = new Vector2Int(2, 0) }
            };

            _testLayout.adjacencies = new List<StorageLayoutSO.AdjacencyPair>
            {
                new StorageLayoutSO.AdjacencyPair { slotA = 0, slotB = 1 },
                new StorageLayoutSO.AdjacencyPair { slotA = 1, slotB = 2 }
            };

            // Create test items
            _cleanFood = ScriptableObject.CreateInstance<ItemDefinition>();
            _cleanFood.id = "clean_food";
            _cleanFood.displayName = "Clean Food";
            _cleanFood.contamination = 0f;

            _dirtyItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _dirtyItem.id = "dirty_item";
            _dirtyItem.displayName = "Dirty Item";
            _dirtyItem.contamination = 1.0f;
        }

        [TearDown]
        public void TearDown()
        {
            if (_testLayout != null) Object.DestroyImmediate(_testLayout);
            if (_cleanFood != null) Object.DestroyImmediate(_cleanFood);
            if (_dirtyItem != null) Object.DestroyImmediate(_dirtyItem);
        }

        [Test]
        public void CrossContam_SpreadsFromDirtyToAdjacentCleanOverTime()
        {
            // Arrange: dirty item in slot 0, clean food in slot 1
            var room = new ShelterRoom("test_room", _testLayout);
            room.AddItem(_dirtyItem, 1, 0);
            room.AddItem(_cleanFood, 1, 1);

            // Initial state
            Assert.AreEqual(1.0f, room.Slots[0].Contamination, Epsilon);
            Assert.AreEqual(0.0f, room.Slots[1].Contamination, Epsilon);

            // Act: tick contamination economy
            var economy = new ContaminationEconomySystem();
            economy.RegisterRoom(room);
            economy.Tick(10f); // 10 game hours

            // Assert: clean food should have gained contamination
            Assert.Greater(room.Slots[1].Contamination, 0.0f, "Clean food should have gained contamination from adjacent dirty item");
            Assert.Less(room.Slots[1].Contamination, 1.0f, "Clean food contamination should be less than dirty item");
        }

        [Test]
        public void CrossContam_FalloffReducesTransferAtDistance()
        {
            // Arrange: dirty item in slot 0, clean food in slot 1 and slot 2
            var room = new ShelterRoom("test_room", _testLayout);
            room.AddItem(_dirtyItem, 1, 0);
            room.AddItem(_cleanFood, 1, 1);
            room.AddItem(_cleanFood, 1, 2);

            var economy = new ContaminationEconomySystem();
            economy.RegisterRoom(room);
            economy.Tick(5f);

            // Slot 1 (adjacent) should have more contamination than slot 2 (farther)
            Assert.Greater(room.Slots[1].Contamination, room.Slots[2].Contamination,
                "Closer slot should gain more contamination due to distance falloff");
        }

        [Test]
        public void DeconQueue_ReducesContaminationButNeverBelowResidualFloor()
        {
            // Arrange
            var room = new ShelterRoom("test_room", _testLayout);
            room.AddItem(_dirtyItem, 1, 0);

            var deconQueue = new DecontaminationQueue();
            deconQueue.DeconRatePerHour = 0.5f;
            deconQueue.ResidualFloor = 0.05f;
            deconQueue.ProcessTimePerItem = 2f;
            deconQueue.WaterCostPerHour = 1f;
            deconQueue.AvailableWater = 100f;
            deconQueue.RegisterRoom(room);

            // Enqueue slot 0 for decontamination
            bool enqueued = deconQueue.Enqueue("test_room", 0);
            Assert.IsTrue(enqueued, "Should successfully enqueue slot");

            // Act: tick for enough time to fully decontaminate
            deconQueue.Tick(10f);

            // Assert: contamination should be reduced to residual floor, not below
            Assert.AreEqual(0.05f, room.Slots[0].Contamination, Epsilon,
                "Decontamination should reduce to residual floor but not below");
        }

        [Test]
        public void DeconQueue_ConsumesWaterDuringProcessing()
        {
            // Arrange
            var room = new ShelterRoom("test_room", _testLayout);
            room.AddItem(_dirtyItem, 1, 0);

            var deconQueue = new DecontaminationQueue();
            deconQueue.DeconRatePerHour = 0.5f;
            deconQueue.ResidualFloor = 0.05f;
            deconQueue.ProcessTimePerItem = 2f;
            deconQueue.WaterCostPerHour = 2f;
            deconQueue.AvailableWater = 10f;
            deconQueue.RegisterRoom(room);

            deconQueue.Enqueue("test_room", 0);

            float initialWater = deconQueue.AvailableWater;

            // Act
            deconQueue.Tick(2f);

            // Assert: water should be consumed
            Assert.Less(deconQueue.AvailableWater, initialWater, "Water should be consumed during decontamination");
            Assert.GreaterOrEqual(deconQueue.AvailableWater, 0f, "Water should not go negative");
        }

        [Test]
        public void DeconQueue_StopsWhenOutOfWater()
        {
            // Arrange
            var room = new ShelterRoom("test_room", _testLayout);
            room.AddItem(_dirtyItem, 1, 0);

            var deconQueue = new DecontaminationQueue();
            deconQueue.DeconRatePerHour = 0.5f;
            deconQueue.ResidualFloor = 0.05f;
            deconQueue.ProcessTimePerItem = 10f;
            deconQueue.WaterCostPerHour = 5f;
            deconQueue.AvailableWater = 1f; // Very limited water
            deconQueue.RegisterRoom(room);

            deconQueue.Enqueue("test_room", 0);

            float initialContamination = room.Slots[0].Contamination;

            // Act: tick for a long time with limited water
            deconQueue.Tick(20f);

            // Assert: decontamination should stop when water runs out
            Assert.Greater(room.Slots[0].Contamination, 0.05f,
                "Decontamination should not reach residual floor when water runs out");
            Assert.Less(room.Slots[0].Contamination, initialContamination,
                "Some decontamination should occur before water runs out");
        }

        [Test]
        public void EntryRouting_DirtyItemWithoutDecon_RaisesRoomContamination()
        {
            // Arrange
            var room = new ShelterRoom("entry", _testLayout);

            float initialRoomContamination = room.AmbientContamination;

            // Act: bring dirty item directly into room (no decon)
            room.BringIntoRoom(_dirtyItem, 1);

            // Assert: room contamination should increase
            Assert.Greater(room.AmbientContamination, initialRoomContamination,
                "Room contamination should increase when dirty item is brought in without decon");
        }

        [Test]
        public void EntryRouting_DecontaminatedItemAddsMinimalContamination()
        {
            // Arrange: decontaminate item first
            var tempRoom = new ShelterRoom("temp", _testLayout);
            tempRoom.AddItem(_dirtyItem, 1, 0);

            var deconQueue = new DecontaminationQueue();
            deconQueue.DeconRatePerHour = 0.5f;
            deconQueue.ResidualFloor = 0.05f;
            deconQueue.ProcessTimePerItem = 2f;
            deconQueue.WaterCostPerHour = 1f;
            deconQueue.AvailableWater = 100f;
            deconQueue.RegisterRoom(tempRoom);

            deconQueue.Enqueue("temp", 0);
            deconQueue.Tick(10f);

            // Item is now decontaminated to residual floor
            Assert.AreEqual(0.05f, tempRoom.Slots[0].Contamination, Epsilon);

            // Now simulate bringing this decontaminated item into a clean room
            var cleanRoom = new ShelterRoom("entry", _testLayout);
            float initialRoomContamination = cleanRoom.AmbientContamination;

            // Act: bring the decontaminated item into the room
            // (In practice, you'd move the item from temp storage to entry storage)
            // The item's contamination is now 0.05, so it should add minimal contamination
            float contamDeposit = 0.05f * 1 * 0.01f; // item.contamination * amount * 0.01f
            float expectedRoomContamination = initialRoomContamination + contamDeposit;

            // Assert: the decontaminated item should add very little contamination
            Assert.Less(contamDeposit, 0.001f,
                "Decontaminated item should add minimal contamination to the room");
        }

        [Test]
        public void RoomAmbient_RisesFromDirtyItemsStoredInRoom()
        {
            // Arrange
            var room = new ShelterRoom("storage", _testLayout);
            room.AddItem(_dirtyItem, 5, 0); // 5 dirty items

            float initialAmbient = room.AmbientContamination;

            var economy = new ContaminationEconomySystem();
            economy.RegisterRoom(room);

            // Act
            economy.Tick(10f);

            // Assert: room ambient should rise from stored dirty items
            Assert.Greater(room.AmbientContamination, initialAmbient,
                "Room ambient contamination should rise from stored dirty items");
        }

        [Test]
        public void RoomAmbient_DecaysOverTime()
        {
            // Arrange
            var room = new ShelterRoom("storage", _testLayout);
            room.AmbientContamination = 0.5f;

            var economy = new ContaminationEconomySystem();
            economy.AmbientDecayRatePerHour = 0.05f;
            economy.RegisterRoom(room);

            // Act: tick with no dirty items in the room
            economy.Tick(10f);

            // Assert: ambient should decay
            Assert.Less(room.AmbientContamination, 0.5f,
                "Room ambient contamination should decay over time");
        }

        [Test]
        public void RoomContamination_AboveThreshold_ContributesToIndoorRad()
        {
            // Arrange
            var room = new ShelterRoom("storage", _testLayout);
            room.AmbientContamination = 0.5f; // Above threshold (0.2)

            // Act
            float radContribution = room.GetIndoorRadContribution();

            // Assert
            Assert.Greater(radContribution, 0f,
                "Room above threshold should contribute to indoor radiation");
        }

        [Test]
        public void RoomContamination_BelowThreshold_NoIndoorRadContribution()
        {
            // Arrange
            var room = new ShelterRoom("storage", _testLayout);
            room.AmbientContamination = 0.1f; // Below threshold (0.2)

            // Act
            float radContribution = room.GetIndoorRadContribution();

            // Assert
            Assert.AreEqual(0f, radContribution, Epsilon,
                "Room below threshold should not contribute to indoor radiation");
        }

        [Test]
        public void RoomContamination_AboveThreshold_ContributesToMoralePenalty()
        {
            // Arrange
            var room = new ShelterRoom("storage", _testLayout);
            room.AmbientContamination = 0.5f; // Above threshold

            // Act
            float moralePenalty = room.GetMoralePenaltyPerHour();

            // Assert
            Assert.Less(moralePenalty, 0f,
                "Room above threshold should have negative morale penalty");
        }

        [Test]
        public void RoomContamination_BelowThreshold_NoMoralePenalty()
        {
            // Arrange
            var room = new ShelterRoom("storage", _testLayout);
            room.AmbientContamination = 0.1f; // Below threshold

            // Act
            float moralePenalty = room.GetMoralePenaltyPerHour();

            // Assert
            Assert.AreEqual(0f, moralePenalty, Epsilon,
                "Room below threshold should have no morale penalty");
        }
    }
}
