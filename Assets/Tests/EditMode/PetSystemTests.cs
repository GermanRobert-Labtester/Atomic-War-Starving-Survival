using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class PetSystemTests
    {
        private NeedsSystem _needsSystem;
        private NeedsProfile _profile;
        private Shelter _shelter;

        [SetUp]
        public void SetUp()
        {
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsSystem = new NeedsSystem(_profile);
            _shelter = new Shelter();
        }

        [Test]
        public void ContaminatedDogInRoom_RaisesAmbientRoomRadiation()
        {
            var petSystem = new PetSystem(_needsSystem, _shelter);

            var room = new ShelterRoom("quarters", null);
            petSystem.RegisterRoom(room);

            var dog = new PetState
            {
                Id = "dog_barnaby",
                DisplayName = "Barnaby",
                FurContamination = 50f, // Highly contaminated fur (50 rads/hr)
                CurrentRoomId = "quarters",
                IsAlive = true
            };
            petSystem.AddPet(dog);

            float initialContamination = room.AmbientContamination;
            float initialIndoorRad = room.GetIndoorRadContribution();

            // Advance system tick by 5 game hours
            petSystem.Tick(new List<Survivor>(), 5f);

            Assert.Greater(room.AmbientContamination, initialContamination, "Ambient room contamination should rise when a contaminated dog occupies the room.");
            Assert.Greater(room.GetIndoorRadContribution(), initialIndoorRad, "Indoor rad contribution should rise as room ambient contamination increases.");
        }

        [Test]
        public void PetStarvation_TriggersCatastrophicMoraleDrop()
        {
            var petSystem = new PetSystem(_needsSystem, _shelter);

            var survivor = new Survivor
            {
                Id = "survivor_1",
                DisplayName = "John",
                State = SurvivorState.Idle
            };
            survivor.Needs.Morale = 80f;

            var pet = new PetState
            {
                Id = "dog_buddy",
                DisplayName = "Buddy",
                Hunger = 95f,
                Thirst = 10f,
                IsAlive = true
            };
            petSystem.AddPet(pet);

            var survivors = new List<Survivor> { survivor };

            // Advance tick by 5 hours so Hunger reaches 100
            petSystem.Tick(survivors, 5f);

            Assert.IsFalse(pet.IsAlive, "Pet should be dead after starvation.");
            Assert.LessOrEqual(survivor.Needs.Morale, 40f, "Survivor morale should drop catastrophically when a pet dies.");
        }

        [Test]
        public void PetDecontamination_ClearsFurContamination()
        {
            var petSystem = new PetSystem(_needsSystem, _shelter);

            var pet = new PetState
            {
                Id = "dog_rex",
                DisplayName = "Rex",
                FurContamination = 30f,
                Radiation = 40f
            };

            petSystem.Decontaminate(pet);

            Assert.AreEqual(0f, pet.FurContamination, "Decontamination should clear fur contamination.");
            Assert.AreEqual(20f, pet.Radiation, "Decontamination should reduce radiation dose.");
        }
    }
}
