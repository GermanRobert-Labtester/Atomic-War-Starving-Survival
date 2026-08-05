using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #214–#219 — Personal Quest Engine + latent expert traits.
    /// </summary>
    [TestFixture]
    public class PersonalQuestTests
    {
        private SkillProgressionSystem _progression;
        private PersonalQuestSystem _quests;
        private MedicalPerkSystem _medical;
        private SocialPerkSystem _social;
        private List<Survivor> _survivors;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _quests = new PersonalQuestSystem();
            _quests.Bind(_progression);
            _medical = new MedicalPerkSystem();
            _medical.Bind(_progression, () => _survivors);
            _medical.BindPersonalQuests(_quests);
            _social = new SocialPerkSystem();
            _social.Bind(_progression);
            _social.BindPersonalQuests(_quests);
            _survivors = new List<Survivor>();
        }

        private Survivor MakeArchetype(string archetypeId, string runtimeId = null)
        {
            var sv = PersonalQuestSystem.MakeArchetypeSurvivor(archetypeId, runtimeId);
            Assert.IsNotNull(sv, "archetype " + archetypeId);
            _quests.AssignProfile(sv, PersonalQuestSystem.ProfileForArchetype(archetypeId));
            _survivors.Add(sv);
            return sv;
        }

        // ── #214 Engine ──────────────────────────────────────────────────

        [Test]
        public void Engine_NoDay0Handout_LatentTraitNotGrantedOnAssign()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            Assert.AreEqual(PersonalQuestSystem.MiracleWorkerId, sv.LatentExpertTraitId);
            Assert.IsFalse(sv.LatentTraitUnlocked);
            Assert.IsFalse(sv.QuestlineActive);
            Assert.IsFalse(_quests.HasMiracleWorker(sv));
            Assert.IsFalse(_progression.HasActivePerk(sv.Id, PersonalQuestSystem.MiracleWorkerId));
        }

        [Test]
        public void Engine_StartsQuest_After30DaysAlive()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            string started = null;
            _quests.OnQuestlineStarted += (s, id) => started = id;

            for (int d = 0; d < PersonalQuestSystem.DaysAliveToStartQuest - 1; d++)
                _quests.TickDaily(_survivors, d + 1);

            Assert.IsFalse(sv.QuestlineActive);
            Assert.IsNull(started);

            _quests.TickDaily(_survivors, PersonalQuestSystem.DaysAliveToStartQuest);
            Assert.IsTrue(sv.QuestlineActive);
            Assert.AreEqual(QuestlineSO.Ids.ShakingHand, started);
            Assert.AreEqual(PersonalQuestSystem.DaysAliveToStartQuest, sv.DaysAlive);
        }

        [Test]
        public void Engine_StartsQuest_OnMoraleZeroThenFullRecovery()
        {
            var sv = MakeArchetype(PersonalQuestSystem.TherapistId);
            string started = null;
            _quests.OnQuestlineStarted += (s, id) => started = id;

            sv.Needs.Morale = 0f;
            _quests.WatchMorale(sv, 1);
            Assert.IsTrue(sv.MoraleHitZero);
            Assert.IsFalse(sv.QuestlineActive);

            sv.Needs.Morale = 100f;
            _quests.WatchMorale(sv, 2);
            Assert.IsTrue(sv.QuestlineActive);
            Assert.AreEqual(QuestlineSO.Ids.BrokenMind, started);
        }

        [Test]
        public void Engine_CompleteQuestline_UnlocksLatentTrait_AndFiresEvolution()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            _quests.TryStartQuestline(sv, "test", 1);

            string unlocked = null;
            string evoDisplay = null;
            _quests.OnLatentTraitUnlocked += (s, id) => unlocked = id;
            _quests.OnCharacterEvolution += (s, id, display) => evoDisplay = display;

            Assert.IsTrue(_quests.CompleteQuestline(sv, 5));
            Assert.IsTrue(sv.LatentTraitUnlocked);
            Assert.IsFalse(sv.QuestlineActive);
            Assert.AreEqual(PersonalQuestSystem.MiracleWorkerId, unlocked);
            Assert.IsTrue(_quests.HasMiracleWorker(sv));
            Assert.IsTrue(sv.HasTrait(PersonalQuestSystem.MiracleWorkerId));
            Assert.IsNotNull(evoDisplay);
            StringAssert.Contains("Miracle", evoDisplay);
        }

        [Test]
        public void Engine_SaveRestore_PreservesQuestProgress()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            _quests.TryStartQuestline(sv, "test", 1);
            sv.Needs.Morale = 10f;
            _quests.RecordStressPhase2Operation(sv, 2);
            _quests.RecordStressPhase2Operation(sv, 2);

            var save = _quests.CaptureState();
            var restored = new PersonalQuestSystem();
            restored.Bind(_progression);
            restored.RestoreState(save);

            var state = restored.GetState(sv.Id);
            Assert.IsTrue(state.QuestActive);
            Assert.AreEqual(2f, state.Progress, 0.01f);
            Assert.AreEqual(QuestlineSO.Ids.ShakingHand, state.QuestlineId);
        }

        [Test]
        public void Engine_MapNodeSpawn_OnPharmacistQuestStart()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PharmacistId);
            string node = null;
            _quests.OnMapNodeSpawnRequested += (n, owner) => node = n;
            _quests.TryStartQuestline(sv, "test", 1);
            Assert.AreEqual(PersonalQuestSystem.RuinedCvsNodeId, node);
        }

        // ── #215 Surgeon / Miracle Worker ────────────────────────────────

        [Test]
        public void Surgeon_ThreeStressOps_UnlocksMiracleWorker()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            _quests.TryStartQuestline(sv, "test", 1);
            sv.Needs.Morale = 20f;

            _medical.RecordPhase2Cure(sv, "sepsis", isPhase2: true, currentDay: 1);
            _medical.RecordPhase2Cure(sv, "sepsis", isPhase2: true, currentDay: 2);
            Assert.IsFalse(_quests.HasMiracleWorker(sv));

            _medical.RecordPhase2Cure(sv, "sepsis", isPhase2: true, currentDay: 3);
            Assert.IsTrue(_quests.HasMiracleWorker(sv));
            Assert.AreEqual(0.5f, _quests.GetSurgeryDurationMultiplier(sv), 0.001f);
            Assert.IsFalse(_quests.ConsumesSurgicalTools(sv));
            Assert.IsTrue(_quests.CanCureArsWithoutChelation(sv));
        }

        [Test]
        public void Surgeon_HighMoraleOps_DoNotCount()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            _quests.TryStartQuestline(sv, "test", 1);
            sv.Needs.Morale = 50f;
            for (int i = 0; i < 5; i++)
                _medical.RecordPhase2Cure(sv, "sepsis", isPhase2: true, currentDay: i);
            Assert.IsFalse(_quests.HasMiracleWorker(sv));
            Assert.AreEqual(0f, _quests.GetState(sv.Id).Progress, 0.01f);
        }

        [Test]
        public void MiracleWorker_StacksWithSteadyHands_Duration()
        {
            var sv = MakeArchetype(PersonalQuestSystem.SurgeonId);
            _quests.TryStartQuestline(sv, "test", 1);
            sv.Needs.Morale = 10f;
            for (int i = 0; i < 3; i++)
                _medical.RecordPhase2Cure(sv, "sepsis", isPhase2: true, currentDay: i);
            // Force Steady Hands as well
            _progression.TryGrantPerk(sv, MedicalPerkSystem.SteadyHandsId, 10);
            float m = _medical.GetSurgeryDurationMultiplier(sv);
            // 0.7 * 0.5 = 0.35
            Assert.AreEqual(0.35f, m, 0.001f);
        }

        // ── #216 Pharmacist / Alchemist ──────────────────────────────────

        [Test]
        public void Pharmacist_LogbookRetrieval_UnlocksAlchemist()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PharmacistId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordPharmacyLogbookRetrieved(sv, 2);
            Assert.IsTrue(_quests.HasAlchemist(sv));
            Assert.IsTrue(_quests.CanCraftAntibioticsFromMold(sv));
            Assert.AreEqual(0.30f, _quests.GetAlchemistDoubleYieldChance(sv), 0.001f);
        }

        [Test]
        public void Alchemist_DoubleYield_SometimesDoubles()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PharmacistId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.CompleteQuestline(sv, 1);

            int doubles = 0;
            var rng = new System.Random(7);
            for (int i = 0; i < 200; i++)
            {
                if (_quests.ApplyAlchemistYield(sv, 1, rng) == 2)
                    doubles++;
            }
            Assert.Greater(doubles, 20);
            Assert.Less(doubles, 120);
        }

        [Test]
        public void Alchemist_CraftAntibioticsFromMold()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PharmacistId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.CompleteQuestline(sv, 1);

            var inv = new InventoryClass { Capacity = 40 };
            var craft = new CraftingSystem(inv);
            craft.BindPersonalQuests(_quests, new System.Random(1));

            var mold = ScriptableObject.CreateInstance<ItemDefinition>();
            mold.id = PersonalQuestSystem.MoldItemId;
            var water = ScriptableObject.CreateInstance<ItemDefinition>();
            water.id = PersonalQuestSystem.DirtyWaterItemId;
            var abx = ScriptableObject.CreateInstance<ItemDefinition>();
            abx.id = PersonalQuestSystem.AntibioticsItemId;

            inv.Add(mold, 1);
            inv.Add(water, 1);
            Assert.IsTrue(craft.TryCraftAntibioticsFromMold(sv, mold, water, abx));
            Assert.GreaterOrEqual(inv.Count(abx), 1);
            Assert.AreEqual(0, inv.Count(mold));

            Object.DestroyImmediate(mold);
            Object.DestroyImmediate(water);
            Object.DestroyImmediate(abx);
        }

        // ── #217 Vet / Zoonotic Expert ───────────────────────────────────

        [Test]
        public void Vet_AlphaCure_UnlocksZoonoticExpert()
        {
            var sv = MakeArchetype(PersonalQuestSystem.VetId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordVetAlphaCure(sv, hoursSpent: 48f, medicalKitsSpent: 3, currentDay: 2);
            Assert.IsTrue(_quests.HasZoonoticExpert(sv));
            Assert.AreEqual(3, _quests.GetMaxTamedAnimals(sv));
            Assert.IsTrue(_quests.PetsEatSpoiledMeatOnly(sv));
        }

        [Test]
        public void Vet_PartialCure_DoesNotComplete()
        {
            var sv = MakeArchetype(PersonalQuestSystem.VetId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordVetAlphaCure(sv, hoursSpent: 24f, medicalKitsSpent: 3, currentDay: 2);
            Assert.IsFalse(_quests.HasZoonoticExpert(sv));
            _quests.RecordVetAlphaCure(sv, hoursSpent: 24f, medicalKitsSpent: 0, currentDay: 3);
            Assert.IsTrue(_quests.HasZoonoticExpert(sv));
        }

        [Test]
        public void Zoonotic_TameUpToThree_SpoiledMeatOnly()
        {
            var sv = MakeArchetype(PersonalQuestSystem.VetId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.CompleteQuestline(sv, 1);

            var needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
            var pets = new PetSystem(needs);
            pets.BindPersonalQuests(_quests, () => _survivors);

            for (int i = 0; i < 3; i++)
            {
                var animal = new PetState
                {
                    Id = "animal_" + i,
                    DisplayName = "Feral " + i,
                    IsAlive = true
                };
                Assert.IsTrue(pets.TryTameWastelandAnimal(sv, animal), "tame " + i);
                Assert.AreEqual(sv.Id, animal.OwnerSurvivorId);
                Assert.IsTrue(animal.EatsSpoiledMeatOnly);
            }

            var fourth = new PetState { Id = "animal_3", DisplayName = "Extra", IsAlive = true };
            Assert.IsFalse(pets.TryTameWastelandAnimal(sv, fourth));
            Assert.AreEqual(3, pets.CountPetsOwnedBy(sv.Id));
        }

        // ── #218 Therapist / Anchor ──────────────────────────────────────

        [Test]
        public void Therapist_ThreeDeEscalations_UnlocksAnchor()
        {
            var sv = MakeArchetype(PersonalQuestSystem.TherapistId);
            _quests.TryStartQuestline(sv, "test", 1);

            _social.RecordPeacefulDeEscalation(sv, 1);
            _social.RecordPeacefulDeEscalation(sv, 2);
            Assert.IsFalse(_quests.HasAnchor(sv));
            _social.RecordPeacefulDeEscalation(sv, 3);
            Assert.IsTrue(_quests.HasAnchor(sv));
        }

        [Test]
        public void Anchor_LocksOwnMorale_AndFloorsRoommates()
        {
            var therapist = MakeArchetype(PersonalQuestSystem.TherapistId, "therapist");
            _quests.TryStartQuestline(therapist, "test", 1);
            _quests.CompleteQuestline(therapist, 1);
            therapist.CurrentRoomId = "quarters";
            therapist.Needs.Morale = 10f;
            _quests.ApplyAnchorMoraleLock(therapist);
            Assert.AreEqual(100f, therapist.Needs.Morale, 0.01f);

            var roommate = new Survivor
            {
                Id = "roomie",
                DisplayName = "Roomie",
                State = SurvivorState.Idle,
                CurrentRoomId = "quarters"
            };
            roommate.Needs.Morale = 5f;
            roommate.Needs.Health = 100f;
            _survivors.Add(roommate);

            _quests.ApplyRoomMoraleFloor(roommate, _survivors);
            Assert.AreEqual(PersonalQuestSystem.AnchorRoomMoraleFloor, roommate.Needs.Morale, 0.01f);

            // Different room — no floor.
            roommate.CurrentRoomId = "plant";
            roommate.Needs.Morale = 5f;
            _quests.ApplyRoomMoraleFloor(roommate, _survivors);
            Assert.AreEqual(5f, roommate.Needs.Morale, 0.01f);
        }

        // ── #219 Undertaker / Death-Blind ────────────────────────────────

        [Test]
        public void Undertaker_MassGraveBurial_UnlocksDeathBlind_WithHits()
        {
            var sv = MakeArchetype(PersonalQuestSystem.UndertakerId);
            _quests.TryStartQuestline(sv, "test", 1);
            sv.Needs.Fatigue = 10f;
            sv.RadiationDose = 5f;

            _quests.RecordMassGraveBurial(sv, 12f, 1);
            Assert.IsFalse(_quests.HasDeathBlind(sv));

            _quests.RecordMassGraveBurial(sv, 12f, 2);
            Assert.IsTrue(_quests.HasDeathBlind(sv));
            Assert.AreEqual(10f + PersonalQuestSystem.MassGraveFatigueHit, sv.Needs.Fatigue, 0.01f);
            Assert.AreEqual(5f + PersonalQuestSystem.MassGraveRadHit, sv.RadiationDose, 0.01f);
            Assert.IsTrue(_quests.IsImmuneToDeathMorale(sv));
            Assert.AreEqual(
                PersonalQuestSystem.DeathBlindDebrisSleepMoralePerHour,
                _quests.GetDebrisSleepMoraleRegen(sv, nearDebris: true),
                0.001f);
        }

        [Test]
        public void DeathBlind_NoRegenWithoutDebris()
        {
            var sv = MakeArchetype(PersonalQuestSystem.UndertakerId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.CompleteQuestline(sv, 1);
            Assert.AreEqual(0f, _quests.GetDebrisSleepMoraleRegen(sv, nearDebris: false), 0.001f);
        }

        [Test]
        public void Profiles_AllFiveArchetypes_HaveDistinctTraits()
        {
            string[] ids =
            {
                PersonalQuestSystem.SurgeonId,
                PersonalQuestSystem.PharmacistId,
                PersonalQuestSystem.VetId,
                PersonalQuestSystem.TherapistId,
                PersonalQuestSystem.UndertakerId
            };
            var traits = new HashSet<string>();
            foreach (var id in ids)
            {
                var p = PersonalQuestSystem.ProfileForArchetype(id);
                Assert.IsNotNull(p);
                Assert.IsFalse(string.IsNullOrEmpty(p.LatentExpertTraitId));
                Assert.IsFalse(string.IsNullOrEmpty(p.ActiveQuestlineId));
                Assert.IsTrue(traits.Add(p.LatentExpertTraitId), "duplicate trait " + p.LatentExpertTraitId);
            }
        }
    }
}
