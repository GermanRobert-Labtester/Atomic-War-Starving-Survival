using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
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
            var room = new ShelterRoom("quarters", null);

            var petSystem = new PetSystem(_needsSystem, (roomId, amount) =>
            {
                if (roomId == room.RoomId)
                {
                    room.AmbientContamination = Mathf.Clamp01(room.AmbientContamination + amount);
                }
            });

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
            var petSystem = new PetSystem(_needsSystem);

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
            var petSystem = new PetSystem(_needsSystem);

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

        // ── H-6e: PetSystem.TryTameWastelandAnimal existed but nothing ever
        // called it — a Zoonotic Expert could never actually tame an animal ──

        [Test]
        public void TryTameWastelandAnimalDaily_TamesAnimal_ForQualifyingZoonoticExpert()
        {
            var petSystem = new PetSystem(_needsSystem);
            var quests = new PersonalQuestSystem();

            var vet = new Survivor { Id = "sv_vet", DisplayName = "Vet", State = SurvivorState.Idle };
            vet.Traits.Add(PersonalQuestSystem.ZoonoticExpertId);
            var survivors = new List<Survivor> { vet };
            petSystem.BindPersonalQuests(quests, () => survivors);

            PetState tamed = null;
            for (int seed = 0; seed < 300 && tamed == null; seed++)
                tamed = petSystem.TryTameWastelandAnimalDaily(survivors, new System.Random(seed));

            Assert.IsNotNull(tamed, "A qualifying Zoonotic Expert should eventually tame an animal.");
            Assert.AreEqual(vet.Id, tamed.OwnerSurvivorId);
            Assert.AreSame(tamed, petSystem.GetPet(tamed.Id));
            Assert.AreEqual(1, petSystem.CountPetsOwnedBy(vet.Id));
        }

        [Test]
        public void TryTameWastelandAnimalDaily_ReturnsNull_WhenNoSurvivorQualifies()
        {
            var petSystem = new PetSystem(_needsSystem);
            var quests = new PersonalQuestSystem();

            var civilian = new Survivor { Id = "sv_civilian", DisplayName = "Civilian", State = SurvivorState.Idle };
            var survivors = new List<Survivor> { civilian };
            petSystem.BindPersonalQuests(quests, () => survivors);

            var tamed = petSystem.TryTameWastelandAnimalDaily(survivors, new System.Random(1));

            Assert.IsNull(tamed, "A survivor without the Zoonotic Expert trait should never tame an animal.");
            Assert.AreEqual(0, petSystem.Pets.Count);
        }

        // ── H-6e: PetSystem.Decontaminate existed but no AI action could ever
        // call it — a tamed animal's fur contamination could only ever rise ──

        [Test]
        public void DecontaminatePetAction_ScoresZero_WhenNoPetsAreDirty()
        {
            var petSystem = new PetSystem(_needsSystem);
            var clean = new PetState { Id = "dog_clean", DisplayName = "Clean Dog", FurContamination = 1f, IsAlive = true };
            petSystem.AddPet(clean);

            var survivor = new Survivor { Id = "sv_1", DisplayName = "Ann", State = SurvivorState.Idle };
            var action = ScriptableObject.CreateInstance<DecontaminatePetActionSO>();
            var ctx = new AIContext(survivor) { PetSystem = petSystem };

            Assert.AreEqual(0f, action.EvaluateRaw(ctx));
        }

        [Test]
        public void DecontaminatePetAction_WashesDownTheDirtiestPet()
        {
            var petSystem = new PetSystem(_needsSystem);
            var mild = new PetState { Id = "dog_mild", DisplayName = "Mild", FurContamination = 10f, IsAlive = true };
            var filthy = new PetState { Id = "dog_filthy", DisplayName = "Filthy", FurContamination = 35f, IsAlive = true };
            petSystem.AddPet(mild);
            petSystem.AddPet(filthy);

            var survivor = new Survivor { Id = "sv_1", DisplayName = "Ann", State = SurvivorState.Idle };
            var action = ScriptableObject.CreateInstance<DecontaminatePetActionSO>();
            var ctx = new AIContext(survivor) { PetSystem = petSystem };

            Assert.Greater(action.EvaluateRaw(ctx), 0f);

            action.Execute(ctx);

            Assert.AreEqual(0f, filthy.FurContamination, "The dirtiest pet should be washed down first.");
            Assert.AreEqual(10f, mild.FurContamination, "A cleaner pet should be left alone while a dirtier one exists.");
        }
    }
}
