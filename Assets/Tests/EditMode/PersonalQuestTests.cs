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
    /// Prompts #214–#283 — Personal Quest Engine + latent expert traits.
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
        public void Profiles_AllTwentyArchetypes_HaveDistinctTraits()
        {
            string[] ids =
            {
                PersonalQuestSystem.SurgeonId,
                PersonalQuestSystem.PharmacistId,
                PersonalQuestSystem.VetId,
                PersonalQuestSystem.TherapistId,
                PersonalQuestSystem.UndertakerId,
                PersonalQuestSystem.VeteranId,
                PersonalQuestSystem.CopId,
                PersonalQuestSystem.BouncerId,
                PersonalQuestSystem.HunterId,
                PersonalQuestSystem.PrisonerId,
                PersonalQuestSystem.PlumberId,
                PersonalQuestSystem.ElectricianId,
                PersonalQuestSystem.ArchitectId,
                PersonalQuestSystem.MechanicId,
                PersonalQuestSystem.ChemistId,
                PersonalQuestSystem.BotanistId,
                PersonalQuestSystem.CourierId,
                PersonalQuestSystem.BurglarId,
                PersonalQuestSystem.MeteorologistId,
                PersonalQuestSystem.HazmatTechId
            };
            var traits = new HashSet<string>();
            foreach (var id in ids)
            {
                var p = PersonalQuestSystem.ProfileForArchetype(id);
                Assert.IsNotNull(p, id);
                Assert.IsFalse(string.IsNullOrEmpty(p.LatentExpertTraitId));
                Assert.IsFalse(string.IsNullOrEmpty(p.ActiveQuestlineId));
                Assert.IsTrue(traits.Add(p.LatentExpertTraitId), "duplicate trait " + p.LatentExpertTraitId);
            }
        }

        // ── #220 Veteran / Warlord ───────────────────────────────────────

        [Test]
        public void Veteran_FeralSquadExecuted_UnlocksWarlord()
        {
            var sv = MakeArchetype(PersonalQuestSystem.VeteranId);
            string node = null;
            _quests.OnMapNodeSpawnRequested += (n, o) => node = n;
            _quests.TryStartQuestline(sv, "test", 1);
            Assert.AreEqual(PersonalQuestSystem.FortifiedSquadNodeId, node);

            _quests.RecordFeralSquadExecuted(sv, 2);
            Assert.IsTrue(_quests.HasWarlord(sv));
            Assert.AreEqual(
                PersonalQuestSystem.AssaultRifleWeaponPower,
                _quests.GetWeaponPowerOverride(sv, PersonalQuestSystem.PipeWeaponId, 12f),
                0.01f);
            Assert.IsTrue(_quests.CanDefendLevel3Unarmed(sv));
            float bonus = _quests.GetWarlordUnarmedDefenseBonus(
                new List<Survivor> { sv }, weaponsPresent: false);
            Assert.AreEqual(PersonalQuestSystem.WarlordUnarmedDefensePower, bonus, 0.01f);
        }

        // ── #221 Cop / Peacekeeper ───────────────────────────────────────

        [Test]
        public void Cop_LockboxCracked_UnlocksPeacekeeper()
        {
            var sv = MakeArchetype(PersonalQuestSystem.CopId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordEvidenceLockboxCracked(sv, 2);
            Assert.IsTrue(_quests.HasPeacekeeper(sv));
            Assert.IsTrue(_quests.CanUseWarningShot(sv));
            Assert.IsTrue(_quests.BlocksInternalCrimeEvent(
                PersonalQuestSystem.InternalSaboteurEventId, _survivors));
            Assert.IsTrue(_quests.BlocksInternalCrimeEvent(
                PersonalQuestSystem.RationThiefEventId, _survivors));
        }

        [Test]
        public void Peacekeeper_WarningShot_DropsNoLoot()
        {
            var sv = MakeArchetype(PersonalQuestSystem.CopId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.CompleteQuestline(sv, 1);
            var combat = new CombatPerkSystem();
            combat.Bind(_progression);
            combat.BindPersonalQuests(_quests);
            var drops = combat.ComputeFleeDropIndices(
                sv, lootCount: 5, tradeValueAt: i => 1f, weightAt: i => 1f);
            Assert.AreEqual(0, drops.Count);
        }

        // ── #222 Bouncer / Juggernaut ────────────────────────────────────

        [Test]
        public void Bouncer_SoloHatchDefense_UnlocksJuggernaut()
        {
            var sv = MakeArchetype(PersonalQuestSystem.BouncerId);
            _quests.TryStartQuestline(sv, "test", 1);
            float before = sv.BaseMaxHealth;
            _quests.RecordSoloHatchDefense(sv, activeGuardCount: 1, survived: true, currentDay: 2);
            Assert.IsTrue(_quests.HasJuggernaut(sv));
            Assert.AreEqual(before * 2f, sv.BaseMaxHealth, 0.01f);
            Assert.IsTrue(_quests.IsImmuneToTraumaAffliction(sv, "broken_bone"));
            Assert.IsTrue(_quests.IsImmuneToTraumaAffliction(sv, "laceration"));
            Assert.IsTrue(_quests.IgnoresEncumbrance(sv));
            Assert.Greater(_quests.GetExpeditionCarryCapacity(sv, 20f), 1000f);
        }

        [Test]
        public void Bouncer_NotAlone_DoesNotComplete()
        {
            var sv = MakeArchetype(PersonalQuestSystem.BouncerId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordSoloHatchDefense(sv, activeGuardCount: 2, survived: true, currentDay: 2);
            Assert.IsFalse(_quests.HasJuggernaut(sv));
        }

        // ── #223 Hunter / Apex Predator ──────────────────────────────────

        [Test]
        public void Hunter_WhiteElkTrackAndKill_UnlocksApexPredator()
        {
            var sv = MakeArchetype(PersonalQuestSystem.HunterId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordWhiteElkNodeVisit(sv, "node_a");
            _quests.RecordWhiteElkNodeVisit(sv, "node_b");
            _quests.RecordWhiteElkKill(sv, usedScrapBow: true, usedFirearm: false, currentDay: 1);
            Assert.IsFalse(_quests.HasApexPredator(sv)); // need 3 nodes

            _quests.RecordWhiteElkNodeVisit(sv, "node_c");
            _quests.RecordWhiteElkKill(sv, usedScrapBow: false, usedFirearm: false, currentDay: 2);
            Assert.IsFalse(_quests.HasApexPredator(sv)); // need scrap bow

            _quests.RecordWhiteElkKill(sv, usedScrapBow: true, usedFirearm: true, currentDay: 2);
            Assert.IsFalse(_quests.HasApexPredator(sv)); // no firearms

            _quests.RecordWhiteElkKill(sv, usedScrapBow: true, usedFirearm: false, currentDay: 3);
            Assert.IsTrue(_quests.HasApexPredator(sv));
            Assert.AreEqual(1f, _quests.GetStealthFactor(sv), 0.001f);
            Assert.AreEqual(50, _quests.GetApexPredatorMeatYield(sv, isForestOrSwamp: true));
            Assert.AreEqual(0, _quests.GetApexPredatorMeatYield(sv, isForestOrSwamp: false));
        }

        // ── #224 Prisoner / Survivalist ──────────────────────────────────

        [Test]
        public void Prisoner_WardenKeys_UnlocksSurvivalist()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PrisonerId);
            _quests.TryStartQuestline(sv, "test", 1);
            string node = null;
            _quests.OnMapNodeSpawnRequested += (n, o) => node = n;
            // quest already started; verify questline has penitentiary node
            var ql = _quests.GetQuestline(QuestlineSO.Ids.TheWardensKey);
            Assert.AreEqual(PersonalQuestSystem.PenitentiaryNodeId, ql.spawnMapNodeId);

            _quests.RecordWardenKeysRetrieved(sv, 2);
            Assert.IsTrue(_quests.HasSurvivalist(sv));
            Assert.IsTrue(_quests.CanEatContaminatedWithoutSickness(sv));
            Assert.AreEqual(
                PersonalQuestSystem.SurvivalistAloneStaminaMult,
                _quests.GetAloneStaminaDrainMultiplier(sv, isAloneOnMap: true),
                0.001f);
            Assert.AreEqual(1f, _quests.GetAloneStaminaDrainMultiplier(sv, isAloneOnMap: false), 0.001f);
        }

        // ── #225 Plumber / Hydraulic Master ──────────────────────────────

        [Test]
        public void Plumber_PipeBurst_UnlocksHydraulicMaster()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PlumberId);
            _quests.TryStartQuestline(sv, "test", 1);
            float radBefore = sv.RadiationDose;
            _quests.RecordPipeBurstFixed(sv, submergedInIrradiatedWater: false, currentDay: 2);
            Assert.IsFalse(_quests.HasHydraulicMaster(sv));
            _quests.RecordPipeBurstFixed(sv, submergedInIrradiatedWater: true, currentDay: 2);
            Assert.IsTrue(_quests.HasHydraulicMaster(sv));
            Assert.Greater(sv.RadiationDose, radBefore);
            Assert.AreEqual(3f, _quests.GetPurifierSpeedMultiplier(_survivors), 0.01f);
            Assert.IsTrue(_quests.CanExtractWaterFromHumidity(sv));
        }

        // ── #226 Electrician / Grid Walker ───────────────────────────────

        [Test]
        public void Electrician_Substation_UnlocksGridWalker()
        {
            var sv = MakeArchetype(PersonalQuestSystem.ElectricianId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordSubstationRepaired(sv, duringFalloutStorm: false, currentDay: 2);
            Assert.IsFalse(_quests.HasGridWalker(sv));
            _quests.RecordSubstationRepaired(sv, duringFalloutStorm: true, currentDay: 2);
            Assert.IsTrue(_quests.HasGridWalker(sv));
            Assert.IsTrue(_quests.GeneratorsImmuneToBreakdown(_survivors));
            Assert.AreEqual(1.5f, _quests.GetPowerCapacityMultiplier(_survivors), 0.01f);
        }

        // ── #227 Architect / Vault Builder ───────────────────────────────

        [Test]
        public void Architect_Blueprints_UnlocksVaultBuilder()
        {
            var sv = MakeArchetype(PersonalQuestSystem.ArchitectId);
            _quests.TryStartQuestline(sv, "test", 1);
            string node = null;
            _quests.OnMapNodeSpawnRequested += (n, o) => node = n;
            // already started — check questline node
            Assert.AreEqual(PersonalQuestSystem.TheFirmNodeId,
                _quests.GetQuestline(QuestlineSO.Ids.TheBlueprints).spawnMapNodeId);
            _quests.RecordBlueprintsRecovered(sv, 2);
            Assert.IsTrue(_quests.HasVaultBuilder(sv));
            Assert.AreEqual(0.5f, _quests.GetRoomBuildCostMultiplier(sv), 0.01f);
            Assert.IsTrue(_quests.LocksStructuralIntegrityAtMax(_survivors));
        }

        // ── #228 Mechanic / Grease Monkey ────────────────────────────────

        [Test]
        public void Mechanic_EngineBlock_UnlocksGreaseMonkey()
        {
            var sv = MakeArchetype(PersonalQuestSystem.MechanicId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordEngineBlockRetrieved(sv, 2);
            Assert.IsTrue(_quests.HasGreaseMonkey(sv));
            Assert.AreEqual(0.5f, _quests.GetVehicleEscapeCostMultiplier(_survivors), 0.01f);
            Assert.IsTrue(_quests.UnlocksVehicleEscape(_survivors));
            Assert.IsTrue(_quests.BicyclesNeverDegrade(sv));
        }

        // ── #229 Chemist / Synthesizer ───────────────────────────────────

        [Test]
        public void Chemist_ChlorineTank_UnlocksSynthesizer_AndScarredLungs()
        {
            var sv = MakeArchetype(PersonalQuestSystem.ChemistId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordChlorineTankCapped(sv, 2);
            Assert.IsTrue(_quests.HasSynthesizer(sv));
            Assert.IsTrue(sv.HasDisability(PersonalQuestSystem.ScarredLungsId));
            Assert.IsTrue(_quests.CanCraftAntiRadFromChemicalScrap(sv));
            Assert.AreEqual(2f, _quests.GetRadAwayEfficiencyMultiplier(_survivors), 0.01f);
        }

        // ── #230 Botanist / Gaia ─────────────────────────────────────────

        [Test]
        public void Botanist_FourteenPerfectDays_UnlocksGaia()
        {
            var sv = MakeArchetype(PersonalQuestSystem.BotanistId);
            _quests.TryStartQuestline(sv, "test", 1);
            for (int d = 0; d < 10; d++)
                _quests.RecordPlanterPerfectDay(sv, planterAtFullHealth: true, currentDay: d + 1);
            Assert.IsFalse(_quests.HasGaia(sv));
            _quests.RecordPlanterPerfectDay(sv, planterAtFullHealth: false, currentDay: 11);
            Assert.AreEqual(0, _quests.GetState(sv.Id).PerfectPlanterDays);
            for (int d = 0; d < PersonalQuestSystem.SeedVaultPerfectDaysRequired; d++)
                _quests.RecordPlanterPerfectDay(sv, planterAtFullHealth: true, currentDay: 20 + d);
            Assert.IsTrue(_quests.HasGaia(sv));
            Assert.AreEqual(3, _quests.GetCropYieldMultiplier(sv));
            Assert.IsTrue(_quests.CropsImmuneToMold(_survivors));
            Assert.IsTrue(_quests.CanGrowMedicinalHerbs(sv));
        }

        // ── #231 Courier / Wasteland Runner ──────────────────────────────

        [Test]
        public void Courier_FiveDeadDrops_UnlocksWastelandRunner()
        {
            var sv = MakeArchetype(PersonalQuestSystem.CourierId);
            _quests.TryStartQuestline(sv, "test", 1);
            for (int i = 0; i < 3; i++)
                _quests.RecordDeadDropSuccess(sv, i + 1);
            _quests.RecordDeadDropFailure(sv);
            Assert.AreEqual(0, _quests.GetState(sv.Id).DeadDropSuccesses);
            for (int i = 0; i < PersonalQuestSystem.LostRouteDeadDropsRequired; i++)
                _quests.RecordDeadDropSuccess(sv, 10 + i);
            Assert.IsTrue(_quests.HasWastelandRunner(sv));
            Assert.AreEqual(0.5f, _quests.GetExpeditionTravelTimeMultiplier(sv), 0.01f);
            Assert.IsTrue(_quests.IgnoresWeatherMovementPenalty(sv));
        }

        // ── #232 Burglar / Ghost ─────────────────────────────────────────

        [Test]
        public void Burglar_VaultNoAlarm_UnlocksGhost()
        {
            var sv = MakeArchetype(PersonalQuestSystem.BurglarId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordVaultCracked(sv, alarmTriggered: true, currentDay: 2);
            Assert.IsFalse(_quests.HasGhost(sv));
            _quests.RecordVaultCracked(sv, alarmTriggered: false, currentDay: 2);
            Assert.IsTrue(_quests.HasGhost(sv));
            Assert.IsTrue(_quests.BypassesLocksAndSafes(sv));
            Assert.IsTrue(_quests.ForcesZeroHatchVisibilityWhenOutside(sv));
        }

        // ── #233 Meteorologist / Stormcaller ─────────────────────────────

        [Test]
        public void Meteorologist_RadarInStorm_UnlocksStormcaller()
        {
            var sv = MakeArchetype(PersonalQuestSystem.MeteorologistId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordRadarDishAligned(sv, duringFalloutStorm: false, currentDay: 2);
            Assert.IsFalse(_quests.HasStormcaller(sv));
            _quests.RecordRadarDishAligned(sv, duringFalloutStorm: true, currentDay: 2);
            Assert.IsTrue(_quests.HasStormcaller(sv));
            Assert.IsTrue(_quests.HasPerfectTenDayForecast(_survivors));
            Assert.AreEqual(15f, _quests.GetStormMoraleBuff(sv, outsideDuringStorm: true), 0.01f);
            Assert.AreEqual(0f, _quests.GetStormMoraleBuff(sv, outsideDuringStorm: false), 0.01f);
        }

        // ── #234 Hazmat Tech / Rad-Walker ────────────────────────────────

        [Test]
        public void Hazmat_BlackBox_UnlocksRadWalker()
        {
            var sv = MakeArchetype(PersonalQuestSystem.HazmatTechId);
            _quests.TryStartQuestline(sv, "test", 1);
            Assert.AreEqual(PersonalQuestSystem.GroundZeroCraterNodeId,
                _quests.GetQuestline(QuestlineSO.Ids.GroundZero).spawnMapNodeId);
            _quests.RecordBlackBoxRetrieved(sv, 2);
            Assert.IsTrue(_quests.HasRadWalker(sv));
            Assert.AreEqual(0.5f, _quests.GetRadiationAbsorbFactor(sv), 0.01f);
            Assert.IsTrue(_quests.SkipsDeconOnReturn(sv));
        }

        // ── #235 Teacher / Polymath ──────────────────────────────────────

        [Test]
        public void Teacher_ManifestAndMourning_UnlocksPolymath()
        {
            var sv = MakeArchetype(PersonalQuestSystem.TeacherId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordRationManifestFound(sv, 2);
            Assert.IsFalse(_quests.HasPolymath(sv));
            for (int d = 0; d < PersonalQuestSystem.TeacherMourningDaysRequired - 1; d++)
                _quests.RecordTeacherMourningDay(sv, 3 + d);
            Assert.IsFalse(_quests.HasPolymath(sv));
            _quests.RecordTeacherMourningDay(sv, 20);
            Assert.IsTrue(_quests.HasPolymath(sv));
            Assert.IsTrue(_quests.UnlocksSkillMentorshipForAllSkills(sv));
            Assert.AreEqual(3f, _quests.GetActionPerkXpMultiplier(sv), 0.01f);
            _progression.BindPersonalQuests(_quests);
            _progression.RecordAction(sv, "medical", 10f, 21);
            Assert.AreEqual(30f, _progression.GetXp(sv.Id, "medical"), 0.01f);
        }

        // ── #236 Politician / Demagogue ──────────────────────────────────

        [Test]
        public void Politician_ThreePropaganda_UnlocksDemagogue()
        {
            var sv = MakeArchetype(PersonalQuestSystem.PoliticianId);
            _quests.TryStartQuestline(sv, "test", 1);
            for (int i = 0; i < 2; i++)
                _quests.RecordPropagandaHostileResolution(sv, i + 1);
            Assert.IsFalse(_quests.HasDemagogue(sv));
            _quests.RecordPropagandaHostileResolution(sv, 3);
            Assert.IsTrue(_quests.HasDemagogue(sv));
            Assert.AreEqual(0f, _quests.GetFactionTrustFloor(_survivors), 0.01f);
            Assert.AreEqual(0f, _quests.ClampFactionTrust(-40f, _survivors), 0.01f);
            Assert.IsTrue(_quests.FactionsDropTribute(_survivors));
        }

        // ── #237 Priest / Shepherd ───────────────────────────────────────

        [Test]
        public void Priest_CrisisTalkDown_UnlocksShepherd_Sermon()
        {
            var priest = MakeArchetype(PersonalQuestSystem.PriestId);
            var other = new Survivor { Id = "ally", DisplayName = "Ally", State = SurvivorState.Idle };
            other.Needs.Morale = 40f;
            _survivors.Add(other);
            _quests.TryStartQuestline(priest, "test", 1);
            _quests.RecordCrisisOfFaith(priest, 2);
            Assert.IsTrue(priest.HasMentalBreak);
            Assert.IsFalse(_quests.HasShepherd(priest));
            _quests.RecordTalkDownSavedPriest(priest, other, 3);
            Assert.IsTrue(_quests.HasShepherd(priest));
            Assert.IsFalse(priest.HasMentalBreak);
            other.Needs.Morale = 40f;
            Assert.IsTrue(_quests.TryPerformSermon(priest, _survivors));
            Assert.AreEqual(60f, other.Needs.Morale, 0.01f);
        }

        // ── #238 Reporter / Muckraker ────────────────────────────────────

        [Test]
        public void Reporter_FiveIntel_UnlocksMuckraker()
        {
            var sv = MakeArchetype(PersonalQuestSystem.ReporterId);
            _quests.TryStartQuestline(sv, "test", 1);
            for (int i = 0; i < 4; i++)
                _quests.RecordFirstStrikeIntel(sv, "first_strike_intel_" + i, i + 1);
            Assert.IsFalse(_quests.HasMuckraker(sv));
            _quests.RecordFirstStrikeIntel(sv, "first_strike_intel_4", 5);
            Assert.IsTrue(_quests.HasMuckraker(sv));
            Assert.IsTrue(_quests.RevealsAllMapFog(sv));
        }

        // ── #239 Radio Host / Voice of the Wastes ────────────────────────

        [Test]
        public void RadioHost_48hBroadcast_UnlocksVoice()
        {
            var sv = MakeArchetype(PersonalQuestSystem.RadioHostId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordContinuousBroadcastHours(sv, 24f, duringBlizzard: true, maxedFatigueAndThirst: true, currentDay: 2);
            Assert.IsFalse(_quests.HasVoiceOfTheWastes(sv));
            _quests.RecordContinuousBroadcastHours(sv, 24f, duringBlizzard: true, maxedFatigueAndThirst: true, currentDay: 3);
            Assert.IsTrue(_quests.HasVoiceOfTheWastes(sv));
            Assert.AreEqual(100f, sv.Needs.Fatigue, 0.01f);
            Assert.IsTrue(_quests.RadioPowerIsFree(_survivors));
            Assert.IsTrue(_quests.RadioIntelIsInstant(sv));
            Assert.IsTrue(_quests.BlocksTrapIntel(sv));
        }

        // ── #240 Chef / Iron Chef ────────────────────────────────────────

        [Test]
        public void Chef_LastSupper_UnlocksIronChef()
        {
            var sv = MakeArchetype(PersonalQuestSystem.ChefId);
            _quests.TryStartQuestline(sv, "test", 1);
            var foods = new List<string> { "canned_food", "pre_war_spice", "meat", "dirty_water_soup" };
            foreach (var f in foods)
                _quests.RecordFoodItemHoarded(sv, f, 1);
            _quests.RecordLastSupperCooked(sv, foods, cookHours: 12f, currentDay: 2);
            Assert.IsFalse(_quests.HasIronChef(sv));
            _quests.RecordLastSupperCooked(sv, foods, cookHours: 24f, currentDay: 2);
            Assert.IsTrue(_quests.HasIronChef(sv));
            sv.Needs.Hunger = 80f;
            sv.Needs.Thirst = 70f;
            sv.Needs.Fatigue = 90f;
            _quests.ApplyIronChefMeal(sv);
            Assert.AreEqual(0f, sv.Needs.Hunger, 0.01f);
            Assert.AreEqual(0f, sv.Needs.Thirst, 0.01f);
            Assert.AreEqual(0f, sv.Needs.Fatigue, 0.01f);
        }

        // ── #241 Athlete / Tireless ──────────────────────────────────────

        [Test]
        public void Athlete_Marathon_UnlocksTireless()
        {
            var sv = MakeArchetype(PersonalQuestSystem.AthleteId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordMarathonExpedition(sv, nodesAway: 10, hoursElapsed: 20f, onFoot: true, returnedHome: true, currentDay: 2);
            Assert.IsFalse(_quests.HasTireless(sv));
            _quests.RecordMarathonExpedition(sv, nodesAway: 15, hoursElapsed: 48f, onFoot: true, returnedHome: true, currentDay: 2);
            Assert.IsTrue(_quests.HasTireless(sv));
            Assert.AreEqual(3f, _quests.GetStaminaPoolMultiplier(sv), 0.01f);
            Assert.AreEqual(1f, _quests.GetDailySleepHoursRequired(sv), 0.01f);
            Assert.AreEqual(300f, sv.BaseMaxStamina, 0.01f);
        }

        // ── #242 Firefighter / Asbestos ──────────────────────────────────

        [Test]
        public void Firefighter_InfernoNoSuit_UnlocksAsbestos()
        {
            var sv = MakeArchetype(PersonalQuestSystem.FirefighterId);
            _quests.TryStartQuestline(sv, "test", 1);
            float hp = sv.Needs.Health;
            _quests.RecordInfernoExtinguished(sv, "plant", woreHazmatSuit: true, currentDay: 2);
            Assert.IsFalse(_quests.HasAsbestos(sv));
            _quests.RecordInfernoExtinguished(sv, "plant", woreHazmatSuit: false, currentDay: 2);
            Assert.IsTrue(_quests.HasAsbestos(sv));
            Assert.Less(sv.Needs.Health, hp);
            Assert.IsTrue(_quests.IsImmuneToFireAndTemperature(sv));
            Assert.IsTrue(_quests.IgnoresColdSleepQuality(sv));
        }

        // ── #243 Tailor / Armorer ────────────────────────────────────────

        [Test]
        public void Tailor_TenScraps_UnlocksArmorer()
        {
            var sv = MakeArchetype(PersonalQuestSystem.TailorId);
            _quests.TryStartQuestline(sv, "test", 1);
            for (int i = 0; i < 9; i++)
                _quests.RecordClothingDisassembled(sv, i + 1);
            Assert.IsFalse(_quests.HasArmorer(sv));
            _quests.RecordClothingDisassembled(sv, 10);
            Assert.IsTrue(_quests.HasArmorer(sv));
            Assert.IsTrue(_quests.CanCraftReinforcedHazmatSuits(sv));
            Assert.AreEqual(0.25f, _quests.GetClothingDegradeMultiplier(_survivors), 0.01f);
        }

        // ── #244 Watchmaker / Tinkerer ───────────────────────────────────

        [Test]
        public void Watchmaker_50Scrap_UnlocksTinkerer()
        {
            var sv = MakeArchetype(PersonalQuestSystem.WatchmakerId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordWatchRepaired(sv, electronicScrapSpent: 20, currentDay: 2);
            Assert.IsFalse(_quests.HasTinkerer(sv));
            _quests.RecordWatchRepaired(sv, electronicScrapSpent: 50, currentDay: 2);
            Assert.IsTrue(_quests.HasTinkerer(sv));
            Assert.IsTrue(_quests.DevicesNeverLoseCalibration(_survivors));
            Assert.IsTrue(_quests.ShowsTrueRadiation(_survivors));
        }

        // ── #245 Historian / Lorekeeper ──────────────────────────────────

        [Test]
        public void Historian_ConstitutionInFire_UnlocksLorekeeper()
        {
            var sv = MakeArchetype(PersonalQuestSystem.HistorianId);
            _quests.TryStartQuestline(sv, "test", 1);
            Assert.AreEqual(PersonalQuestSystem.RuinedMuseumNodeId,
                _quests.GetQuestline(QuestlineSO.Ids.MuseumArchive).spawnMapNodeId);
            _quests.RecordConstitutionRetrieved(sv, museumBurning: false, currentDay: 2);
            Assert.IsFalse(_quests.HasLorekeeper(sv));
            _quests.RecordConstitutionRetrieved(sv, museumBurning: true, currentDay: 2);
            Assert.IsTrue(_quests.HasLorekeeper(sv));
            Assert.AreEqual(15f, _quests.GetJournalMoraleBoost(_survivors), 0.01f);
            Assert.AreEqual(2f, _quests.GetArtifactTradeValueMultiplier(_survivors), 0.01f);
        }

        // ── #246 Defector / Zealot's Bane ────────────────────────────────

        [Test]
        public void Defector_KillsCultLeader_UnlocksZealotsBane()
        {
            var sv = MakeArchetype(PersonalQuestSystem.DefectorId);
            _quests.TryStartQuestline(sv, "test", 1);
            _quests.RecordCultLeaderKilled(sv, "random_cultist", 2);
            Assert.IsFalse(_quests.HasZealotsBane(sv));
            _quests.RecordCultLeaderKilled(sv, PersonalQuestSystem.CultLeaderId, 2);
            Assert.IsTrue(_quests.HasZealotsBane(sv));
            Assert.IsTrue(_quests.CultistsFleeFrom(sv));
            Assert.AreEqual(1.5f, _quests.GetFactionCombatDamageMultiplier(sv), 0.01f);
        }

        // ── #247 Addict / Chem-Resistant ─────────────────────────────────

        [Test]
        public void Addict_FourteenCleanDays_UnlocksChemResistant()
        {
            var sv = MakeArchetype(PersonalQuestSystem.AddictId);
            _quests.TryStartQuestline(sv, "test", 1);
            for (int d = 0; d < 5; d++)
                _quests.RecordWithdrawalCleanDay(sv, relapsed: false, currentDay: d + 1);
            _quests.RecordWithdrawalCleanDay(sv, relapsed: true, currentDay: 6);
            Assert.AreEqual(0, _quests.GetState(sv.Id).WithdrawalCleanDays);
            for (int d = 0; d < PersonalQuestSystem.WithdrawalCleanDaysRequired; d++)
                _quests.RecordWithdrawalCleanDay(sv, relapsed: false, currentDay: 10 + d);
            Assert.IsTrue(_quests.HasChemResistant(sv));
            Assert.IsTrue(_quests.ImmuneToAddiction(sv));
            Assert.AreEqual(2f, _quests.GetMedicalHealMultiplier(sv), 0.01f);
        }

        // ── #248 Parent / Protector ──────────────────────────────────────

        [Test]
        public void Parent_LocketMourning_UnlocksProtector()
        {
            var parent = MakeArchetype(PersonalQuestSystem.ParentId);
            var ally = new Survivor { Id = "child_sub", DisplayName = "Ally", State = SurvivorState.Idle };
            ally.Needs.Health = 5f;
            ally.BaseMaxHealth = 100f;
            _survivors.Add(ally);
            _quests.TryStartQuestline(parent, "test", 1);
            _quests.RecordChildDeathIntel(parent, 2);
            Assert.IsTrue(parent.HasMentalBreak);
            Assert.IsFalse(_quests.HasProtector(parent));
            _quests.RecordParentMourningSurvived(parent, mourningDays: 3f, currentDay: 3);
            Assert.IsFalse(_quests.HasProtector(parent));
            _quests.RecordParentMourningSurvived(parent, mourningDays: 7f, currentDay: 10);
            Assert.IsTrue(_quests.HasProtector(parent));
            Assert.IsFalse(parent.HasMentalBreak);
            Assert.IsTrue(_quests.IsProtectorEnraged(parent, _survivors));
            Assert.AreEqual(3f, _quests.GetProtectorActionSpeedMultiplier(parent, _survivors), 0.01f);
        }

        // ── #249 Fierce Mother / Matriarch ───────────────────────────────

        [Test]
        public void FierceMother_SelflessAndEmptyCrib_UnlocksMatriarch()
        {
            var mother = MakeArchetype(PersonalQuestSystem.FierceMotherId);
            Assert.IsTrue(_quests.HasSelfless(mother));
            Assert.IsFalse(_quests.HasMatriarch(mother));

            var child = new Survivor { Id = "kid_a", DisplayName = "Kid", State = SurvivorState.Idle, IsChild = true };
            child.Needs.Hunger = 10f;
            _survivors.Add(child);
            Assert.IsTrue(_quests.ShouldCancelEatOrSleepForChild(mother, _survivors));

            var ally = new Survivor { Id = "ally_m", DisplayName = "Ally", State = SurvivorState.Idle };
            ally.Needs.Morale = 50f;
            mother.Needs.Morale = 50f;
            _survivors.Add(ally);
            float absorbed = _quests.GetSelflessMoraleAbsorb(mother, 20f);
            Assert.AreEqual(2f, absorbed, 0.01f);
            _quests.ApplyMoraleDamageWithSelfless(ally, 20f, _survivors);
            Assert.Less(mother.Needs.Morale, 50f);

            _quests.TryStartQuestline(mother, "test", 1);
            Assert.AreEqual(PersonalQuestSystem.DaycareNodeId,
                _quests.GetQuestline(QuestlineSO.Ids.TheEmptyCrib).spawnMapNodeId);
            _quests.RecordDaycareToyRetrieved(mother, radiationLevel: 50f, currentDay: 2);
            Assert.IsFalse(_quests.HasMatriarch(mother));
            _quests.RecordDaycareToyRetrieved(mother, radiationLevel: 80f,
                nodeId: PersonalQuestSystem.DaycareNodeId, currentDay: 2);
            Assert.IsTrue(_quests.HasMatriarch(mother));

            child.CurrentRoomId = "quarters";
            mother.CurrentRoomId = "quarters";
            Assert.AreEqual(20f, _quests.GetMatriarchRoomHealthBonus(child, _survivors), 0.01f);
            Assert.IsTrue(_quests.BlocksMentalBreak(mother, "despair", _survivors));
        }

        // ── #250 Exhausted Father / Pillar of Atlas ──────────────────────

        [Test]
        public void ExhaustedFather_WorkaholicAndFiveTier3_UnlocksPillar()
        {
            var father = MakeArchetype(PersonalQuestSystem.ExhaustedFatherId);
            Assert.IsTrue(_quests.HasWorkaholic(father));
            Assert.AreEqual(0.5f, _quests.GetCraftRepairFatigueDrainMultiplier(father), 0.01f);
            Assert.AreEqual(0.5f, _quests.GetSleepFatigueRestoreMultiplier(father), 0.01f);
            father.Needs.Fatigue = 50f;
            Assert.IsTrue(_quests.ShouldIgnoreRestAction(father));
            father.Needs.Fatigue = 96f;
            Assert.IsFalse(_quests.ShouldIgnoreRestAction(father));

            _quests.TryStartQuestline(father, "test", 1);
            for (int i = 0; i < 4; i++)
                _quests.RecordTier3ModuleBuilt(father, moduleLevel: 3, currentDay: 20 + i);
            Assert.IsFalse(_quests.HasPillarOfAtlas(father));
            _quests.RecordTier3ModuleBuilt(father, moduleLevel: 2, currentDay: 30);
            Assert.IsFalse(_quests.HasPillarOfAtlas(father));
            _quests.RecordTier3ModuleBuilt(father, moduleLevel: 3, currentDay: 40);
            Assert.IsTrue(_quests.HasPillarOfAtlas(father));
            Assert.IsTrue(_quests.IgnoresFatigueActionSpeedPenalty(father));
            Assert.AreEqual(1f, _quests.GetFatigueActionSpeedMultiplier(father, 0.5f), 0.01f);

            _quests.NotifySurvivorDied(father);
            Assert.IsTrue(_quests.PillarOfAtlasDeathDebuffActive);
            Assert.AreEqual(0.8f, _quests.GetShelterRepairSpeedMultiplier(), 0.01f);
        }

        // ── #251 Naive Son / Wasteland Scout ─────────────────────────────

        [Test]
        public void NaiveSon_DependentPollyanna_SoloRaid_UnlocksScout()
        {
            var son = MakeArchetype(PersonalQuestSystem.NaiveSonId);
            Assert.IsTrue(son.IsChild);
            Assert.IsTrue(son.CannotFight);
            Assert.IsTrue(_quests.HasDependent(son));
            Assert.IsTrue(_quests.HasPollyanna(son));
            Assert.IsFalse(_quests.CanEquipFirearms(son));
            Assert.AreEqual(10f, _quests.GetExpeditionCarryCapacity(son, 40f), 0.01f);
            Assert.IsTrue(_quests.IsImmuneToDespairBreak(son));

            var adult = new Survivor { Id = "adult_h", DisplayName = "Adult", State = SurvivorState.Idle };
            adult.Needs.Morale = 40f;
            _survivors.Add(adult);
            _quests.ApplyChildInteractionHope(son, adult);
            Assert.AreEqual(65f, adult.Needs.Morale, 0.01f);

            _quests.TryStartQuestline(son, "test", 1);
            _quests.RecordSoloRaidSurvived(son, adultsPresentInRoom: true, raidSurvived: true, currentDay: 2);
            Assert.IsFalse(_quests.HasWastelandScout(son));
            _quests.RecordSoloRaidSurvived(son, adultsPresentInRoom: false, raidSurvived: true, currentDay: 2);
            Assert.IsTrue(_quests.HasWastelandScout(son));
            Assert.IsTrue(_quests.IsImmuneToSniperEncounters(son));
            Assert.IsTrue(_quests.CanCrawlDebrisInstantly(son));
        }

        // ── #252 Hardened Daughter / Child of the Ash ────────────────────

        [Test]
        public void HardenedDaughter_TraumaCap_FirstBlood_UnlocksAsh()
        {
            var dau = MakeArchetype(PersonalQuestSystem.HardenedDaughterId);
            Assert.IsTrue(dau.IsChild);
            Assert.IsTrue(_quests.HasTraumatized(dau));
            Assert.AreEqual(50f, _quests.GetMaxMoraleCap(dau), 0.01f);
            dau.Needs.Morale = 80f;
            _quests.ClampMoraleToCap(dau);
            Assert.AreEqual(50f, dau.Needs.Morale, 0.01f);
            Assert.IsTrue(_quests.RefusesPlayOrComfort(dau));
            Assert.AreEqual(2f, _quests.GetTrainGuardUtilityBias(dau), 0.01f);

            _quests.TryStartQuestline(dau, "test", 1);
            _quests.RecordRaiderKillingBlow(dau, duringHatchBreach: false, isFactionRaider: true, currentDay: 2);
            Assert.IsFalse(_quests.HasChildOfTheAsh(dau));
            _quests.RecordRaiderKillingBlow(dau, duringHatchBreach: true, isFactionRaider: true, currentDay: 2);
            Assert.IsTrue(_quests.HasChildOfTheAsh(dau));
            Assert.IsTrue(_quests.IsImmuneToRadiationAnxiety(dau));
            Assert.IsTrue(_quests.CanEquipAdultWeapons(dau));
            Assert.AreEqual(1f, _quests.GetChildWeaponAccuracyMultiplier(dau), 0.01f);
            Assert.IsTrue(_quests.HasSociopath(dau));
            Assert.IsTrue(_quests.IsImmuneToDeathMorale(dau));
        }

        // ── #253 Psychopath / Cold Calculus ──────────────────────────────

        [Test]
        public void Psychopath_SociopathArrogant_PerfectEquation_UnlocksColdCalculus()
        {
            var psy = MakeArchetype(PersonalQuestSystem.PsychopathId);
            Assert.IsTrue(_quests.HasSociopath(psy));
            Assert.IsTrue(_quests.HasArrogant(psy));
            Assert.IsTrue(_quests.IsImmuneToDeathMorale(psy));
            Assert.IsTrue(_quests.MustSelfHeal(psy));
            Assert.IsFalse(_quests.CanBeHealedBy(psy, new Survivor { Id = "medic" }));
            Assert.IsTrue(_quests.CanBeHealedBy(psy, psy));
            Assert.AreEqual(8f, _quests.GetInterpersonalAffinityDrainPerHour(psy), 0.01f);

            _quests.TryStartQuestline(psy, "test", 1);
            _quests.RecordDeliberateNeedDeath(psy, "radiation", wasDeliberate: true, currentDay: 2);
            Assert.IsFalse(_quests.HasColdCalculus(psy));
            _quests.RecordDeliberateNeedDeath(psy, "starvation", wasDeliberate: true, currentDay: 2);
            Assert.IsTrue(_quests.HasColdCalculus(psy));
            Assert.AreEqual(1.5f, _quests.GetUtilityExecutionSpeedMultiplier(psy, livingPopulation: 2), 0.01f);
            Assert.AreEqual(1f, _quests.GetUtilityExecutionSpeedMultiplier(psy, livingPopulation: 5), 0.01f);
        }

        // ── #254 Serial Killer / Butcher of Day 30 ───────────────────────

        [Test]
        public void SerialKiller_UrgeAndEmbrace_UnlocksButcher()
        {
            var killer = MakeArchetype(PersonalQuestSystem.SerialKillerId);
            Assert.IsTrue(killer.HasTrait(PersonalQuestSystem.KindId));
            Assert.IsTrue(killer.HasTrait(PersonalQuestSystem.CharismaticId));
            Assert.AreEqual(0f, _quests.GetUrgeNeed(killer), 0.01f);

            var victim = new Survivor { Id = "coma_v", DisplayName = "Victim", State = SurvivorState.Incapacitated };
            victim.Needs.Health = 3f;
            _survivors.Add(victim);

            string murderTarget = null;
            _quests.OnSecretMurderAttempted += (k, tid, kind) => murderTarget = tid;
            _quests.TickUrge(killer, 100f, _survivors);
            Assert.AreEqual(100f, _quests.GetUrgeNeed(killer), 0.01f);
            Assert.AreEqual("coma_v", murderTarget);

            _quests.TryStartQuestline(killer, "test", 1);
            _quests.RecordMaskSlipsChoice(killer, embrace: true, currentDay: 5);
            Assert.IsTrue(_quests.HasButcherOfDay30(killer));
            Assert.IsTrue(_quests.AutoClearsHumanEncounters(killer));
            Assert.AreEqual(1f, _quests.GetExpeditionStealthFactor(killer), 0.01f);
        }

        [Test]
        public void SerialKiller_ExecutePath_NoLatentTrait()
        {
            var killer = MakeArchetype(PersonalQuestSystem.SerialKillerId);
            _quests.TryStartQuestline(killer, "test", 1);
            _quests.RecordMaskSlipsChoice(killer, embrace: false, currentDay: 5);
            Assert.IsFalse(_quests.HasButcherOfDay30(killer));
            Assert.IsFalse(killer.IsAlive);
            Assert.IsFalse(killer.QuestlineActive);
        }

        // ── #255 Pathological Liar / Master Manipulator ──────────────────

        [Test]
        public void Liar_DeceptiveMask_Phase2Cure_UnlocksManipulator()
        {
            var liar = MakeArchetype(PersonalQuestSystem.LiarId);
            Assert.IsTrue(_quests.HasDeceptive(liar));
            liar.Needs.Hunger = 5f;
            // Deterministic mask: force via many rolls that at least one masks,
            // or test GetDisplayedNeed with seeded rng that always masks.
            var always = new System.Random(0);
            // With chance 0.35 some seeds mask — use direct ShouldMask when distressed
            // by testing GenerateFalseIntelNode instead for AI quirk.
            string fake = _quests.GenerateFalseIntelNode(liar, new System.Random(1));
            Assert.IsNotNull(fake);
            Assert.IsTrue(fake.StartsWith("fake_stash_"));

            _quests.TryStartQuestline(liar, "test", 1);
            _quests.RecordLethalPhase2Cured(liar, wasHiddenFromPlayer: false, isPhase2Lethal: true, currentDay: 2);
            Assert.IsFalse(_quests.HasMasterManipulator(liar));
            _quests.RecordLethalPhase2Cured(liar, wasHiddenFromPlayer: true, isPhase2Lethal: true, currentDay: 2);
            Assert.IsTrue(_quests.HasMasterManipulator(liar));
            Assert.IsTrue(_quests.TradesJunkAsMedicine(liar));
            Assert.AreEqual(50f, _quests.GetJunkTradeValueAsMedicine(liar, 5f, 50f), 0.01f);
        }

        // ── #256 Selfish Hoarder / Dragon's Hoard ────────────────────────

        [Test]
        public void Hoarder_SelfishTheftAndSafe_UnlocksDragonsHoard()
        {
            var hoarder = MakeArchetype(PersonalQuestSystem.HoarderId);
            Assert.IsTrue(_quests.HasSelfish(hoarder));
            Assert.AreEqual(2f, _quests.GetRationConsumptionMultiplier(hoarder), 0.01f);
            Assert.AreEqual(15f, _quests.GetSelfishMissedRationMoraleHit(hoarder), 0.01f);

            Assert.IsTrue(_quests.TryStealToPersonalInventory(hoarder, "canned_beans"));
            Assert.IsTrue(hoarder.HasHiddenStash);
            Assert.Contains("canned_beans", hoarder.HiddenItemIds);

            _quests.TryStartQuestline(hoarder, "test", 1);
            _quests.RecordSafeCarried(hoarder, safeWeightKg: 50f, fatigueLevel: 50f, currentDay: 2);
            _quests.RecordSafeCarried(hoarder, safeWeightKg: 50f, fatigueLevel: 70f, currentDay: 3);
            Assert.IsFalse(_quests.HasDragonsHoard(hoarder));
            _quests.RecordSafeCarried(hoarder, safeWeightKg: 50f, fatigueLevel: 95f, currentDay: 4);
            Assert.IsTrue(_quests.HasDragonsHoard(hoarder));
            Assert.IsTrue(_quests.PersonalInventoryNeverDegrades(hoarder));
            Assert.IsTrue(_quests.GetState(hoarder.Id).SafeWasEmpty);
        }

        // ── #257 Disgraced General / Art of War ──────────────────────────

        [Test]
        public void General_HatedTactician_HitSquad_UnlocksArtOfWar()
        {
            var gen = MakeArchetype(PersonalQuestSystem.GeneralId);
            Assert.IsTrue(_quests.HasTactician(gen));
            Assert.IsTrue(_quests.HasHated(gen));
            Assert.AreEqual(-100f, _quests.GetMilitaryFactionTrustOffset(gen), 0.01f);
            Assert.IsTrue(_quests.IsShotOnSightByMilitary(gen));
            Assert.IsTrue(_quests.RequiresBedModuleToSleep(gen));
            Assert.AreEqual(12f, _quests.GetFloorSleepFatiguePenaltyPerHour(gen, hasBedModule: false), 0.01f);
            Assert.AreEqual(0f, _quests.GetFloorSleepFatiguePenaltyPerHour(gen, hasBedModule: true), 0.01f);

            _quests.TryStartQuestline(gen, "test", 1);
            _quests.RecordHitSquadWiped(gen, targetedAtGeneral: false, squadWiped: true, currentDay: 2);
            Assert.IsFalse(_quests.HasArtOfWar(gen));
            _quests.RecordHitSquadWiped(gen, targetedAtGeneral: true, squadWiped: true, currentDay: 2);
            Assert.IsTrue(_quests.HasArtOfWar(gen));
            Assert.AreEqual(1.25f, _quests.GetArtOfWarShelterSecurityMultiplier(_survivors), 0.01f);
            Assert.AreEqual(125f, _quests.ApplyArtOfWarShelterSecurity(100f, _survivors), 0.01f);
        }

        // ── #258 Rebel Saboteur / Demolitions Expert ─────────────────────

        [Test]
        public void Saboteur_AntiAuthority_Checkpoint_UnlocksDemolitions()
        {
            var sab = MakeArchetype(PersonalQuestSystem.SaboteurId);
            var qm = MakeArchetype(PersonalQuestSystem.QuartermasterId, "qm_ord");
            Assert.IsTrue(_quests.HasAntiAuthority(sab));
            Assert.IsTrue(_quests.AutoDisarmsTraps(sab));
            Assert.IsTrue(_quests.LosesMoraleFromAuthorityOrder(sab, qm));
            sab.Needs.Morale = 50f;
            _quests.ApplyAntiAuthorityOrderMorale(sab, qm);
            Assert.AreEqual(38f, sab.Needs.Morale, 0.01f);

            _quests.TryStartQuestline(sab, "test", 1);
            _quests.RecordMilitaryCheckpointDestroyed(sab, "wrong_node", usedIed: true, currentDay: 2);
            Assert.IsFalse(_quests.HasDemolitionsExpert(sab));
            _quests.RecordMilitaryCheckpointDestroyed(
                sab, PersonalQuestSystem.MilitaryCheckpointNodeId, usedIed: true, currentDay: 2);
            Assert.IsTrue(_quests.HasDemolitionsExpert(sab));
            Assert.IsTrue(_quests.CanBreachVaultsInstantly(sab));
            Assert.AreEqual(3f, _quests.GetExplosiveDamageMultiplier(sab), 0.01f);
        }

        // ── #259 Deserter Sniper / Ghost Shooter ─────────────────────────

        [Test]
        public void Deserter_CowardFlee_HoldLine_UnlocksGhostShooter()
        {
            var des = MakeArchetype(PersonalQuestSystem.DeserterId);
            Assert.IsTrue(_quests.HasCoward(des));
            Assert.IsTrue(_quests.RefusesLoudLabor(des));
            Assert.IsTrue(_quests.IsLoudLaborAction("build_wall"));
            des.Needs.Health = 40f;
            Assert.IsTrue(_quests.ShouldAutoFleeCombat(des));
            des.Needs.Health = 80f;
            Assert.IsFalse(_quests.ShouldAutoFleeCombat(des));

            _quests.TryStartQuestline(des, "test", 1);
            _quests.RecordRaidDefenseWithoutFleeing(des, raidSurvived: true, fled: true, defendedHatch: true, currentDay: 2);
            Assert.IsFalse(_quests.HasGhostShooter(des));
            _quests.RecordRaidDefenseWithoutFleeing(des, raidSurvived: true, fled: false, defendedHatch: true, currentDay: 2);
            Assert.IsTrue(_quests.HasGhostShooter(des));
            Assert.IsTrue(_quests.SuppressesHostileEncounterUi(des));
            Assert.IsTrue(_quests.CanMapLayerRangedEngage(des));
        }

        // ── #260 Quartermaster / Supply Chain Master ─────────────────────

        [Test]
        public void Quartermaster_Strict_Audit_UnlocksSupplyChain()
        {
            var qm = MakeArchetype(PersonalQuestSystem.QuartermasterId);
            Assert.IsTrue(_quests.HasStrict(qm));
            Assert.IsTrue(_quests.ShouldAutoResortInventory(qm));
            Assert.AreEqual(2f, _quests.GetStrictInventoryMoraleDelta(qm, true, true, 0.5f), 0.01f);
            Assert.AreEqual(-4f, _quests.GetStrictInventoryMoraleDelta(qm, true, true, 0.1f), 0.01f);

            _quests.TryStartQuestline(qm, "test", 1);
            _quests.RecordScrapStockpile(qm, 100, 50, 100, currentDay: 2);
            Assert.IsFalse(_quests.HasSupplyChainMaster(qm));
            _quests.RecordScrapStockpile(qm, 100, 100, 100, currentDay: 3);
            Assert.IsTrue(_quests.HasSupplyChainMaster(qm));
            Assert.AreEqual(0.8f, _quests.GetCraftMaterialCostMultiplier(qm), 0.01f);
            Assert.AreEqual(0.7f, _quests.GetBunkerFuelBurnMultiplier(_survivors), 0.01f);
        }

        // ── #261 Child Soldier / Reclaimed Youth ─────────────────────────

        [Test]
        public void ChildSoldier_Stunted_DropRifle_UnlocksReclaimedYouth()
        {
            var kid = MakeArchetype(PersonalQuestSystem.ChildSoldierId);
            Assert.IsTrue(kid.IsChild);
            Assert.IsTrue(_quests.HasStunted(kid));
            Assert.IsFalse(_quests.CanLearnScienceSkill(kid));
            Assert.IsFalse(_quests.CanLearnMedicalSkill(kid));
            Assert.IsTrue(_quests.CausesNightTerrors(kid));
            var roomie = new Survivor { Id = "roomie", DisplayName = "R", State = SurvivorState.Idle, CurrentRoomId = "bunk" };
            kid.CurrentRoomId = "bunk";
            _survivors.Add(roomie);
            Assert.IsTrue(_quests.DisruptsRoomSleep(kid, roomie));

            _quests.TryStartQuestline(kid, "test", 1);
            for (int d = 0; d < 29; d++)
                _quests.RecordUnequippedWeaponDay(kid, weaponUnequipped: true, currentDay: d + 1);
            Assert.IsFalse(_quests.HasReclaimedYouth(kid));
            // Re-equip resets streak
            _quests.RecordUnequippedWeaponDay(kid, weaponUnequipped: false, currentDay: 30);
            Assert.AreEqual(0, _quests.GetState(kid.Id).UnequippedWeaponDays);
            for (int d = 0; d < 30; d++)
                _quests.RecordUnequippedWeaponDay(kid, weaponUnequipped: true, currentDay: 40 + d);
            Assert.IsTrue(_quests.HasReclaimedYouth(kid));
            Assert.IsFalse(_quests.HasStunted(kid));
            Assert.IsTrue(_quests.CanLearnScienceSkill(kid));
            Assert.IsTrue(_quests.HasHopeAura(kid));
            Assert.IsFalse(_quests.CausesNightTerrors(kid));
        }

        // ── #262 Pure Empath / Soul Weaver ───────────────────────────────

        [Test]
        public void Empath_HyperEmpathy_Sponge_UnlocksSoulWeaver()
        {
            var emp = MakeArchetype(PersonalQuestSystem.EmpathId);
            Assert.IsTrue(_quests.HasHyperEmpathetic(emp));
            Assert.IsTrue(_quests.PrioritizesComfortOverSurvival(emp));
            emp.Needs.Morale = 40f;
            _quests.ApplyHyperEmpatheticMorale(emp, bunkerAverageMorale: 80f, gameHours: 1f);
            Assert.Greater(emp.Needs.Morale, 40f);

            var patient = new Survivor { Id = "p1", DisplayName = "P", State = SurvivorState.Idle };
            _survivors.Add(patient);
            _quests.TryStartQuestline(emp, "test", 1);
            emp.Needs.Health = 80f;
            _quests.RecordMentalBreakCured(emp, patient, curedSuccessfully: true, currentDay: 1);
            Assert.AreEqual(1f, emp.Needs.Health, 0.01f);
            _quests.RecordMentalBreakCured(emp, patient, curedSuccessfully: true, currentDay: 2);
            Assert.IsFalse(_quests.HasSoulWeaver(emp));
            _quests.RecordMentalBreakCured(emp, patient, curedSuccessfully: true, currentDay: 3);
            Assert.IsTrue(_quests.HasSoulWeaver(emp));

            emp.Needs.Health = 50f;
            emp.Needs.Morale = 60f;
            patient.Needs.Health = 5f;
            patient.Needs.Morale = 10f;
            Assert.IsTrue(_quests.TrySoulWeaverTransfer(emp, patient, 20f, 15f));
            Assert.AreEqual(30f, emp.Needs.Health, 0.01f);
            Assert.AreEqual(45f, emp.Needs.Morale, 0.01f);
            Assert.AreEqual(25f, patient.Needs.Health, 0.01f);
            Assert.AreEqual(25f, patient.Needs.Morale, 0.01f);
        }

        // ── #263 Bitter Misanthrope / Lone Wolf ──────────────────────────

        [Test]
        public void Misanthrope_Rude_SoloExpedition_UnlocksLoneWolf()
        {
            var mis = MakeArchetype(PersonalQuestSystem.MisanthropeId);
            Assert.IsTrue(_quests.HasRude(mis));
            Assert.AreEqual(6f, _quests.GetRudeAffinityDrainPerHour(mis), 0.01f);
            Assert.AreEqual(1.25f, _quests.GetSoloRoomActionSpeedMultiplier(mis, othersInRoom: 0), 0.01f);
            Assert.AreEqual(1f, _quests.GetSoloRoomActionSpeedMultiplier(mis, othersInRoom: 1), 0.01f);

            _quests.TryStartQuestline(mis, "test", 1);
            for (int d = 0; d < 14; d++)
                _quests.RecordSoloExpeditionDay(mis, entirelyAlone: true, returnedToBunker: false, currentDay: d + 1);
            Assert.IsFalse(_quests.HasLoneWolf(mis));
            _quests.RecordSoloExpeditionDay(mis, entirelyAlone: true, returnedToBunker: true, currentDay: 15);
            Assert.AreEqual(0, _quests.GetState(mis.Id).SoloExpeditionDays);
            for (int d = 0; d < 15; d++)
                _quests.RecordSoloExpeditionDay(mis, entirelyAlone: true, returnedToBunker: false, currentDay: 20 + d);
            Assert.IsTrue(_quests.HasLoneWolf(mis));
            Assert.AreEqual(0.5f, _quests.GetLoneWolfNeedsDecayMultiplier(mis, outsideBunker: true), 0.01f);
            Assert.AreEqual(1.75f, _quests.GetLoneWolfCombatMultiplier(mis, outsideBunker: true), 0.01f);
        }

        // ── #264 Pollyanna Denialist / Grounded Optimist ─────────────────

        [Test]
        public void Pollyanna_Denialist_ArsSurvive_UnlocksGroundedOptimist()
        {
            var pol = MakeArchetype(PersonalQuestSystem.ThePollyannaId);
            Assert.IsTrue(_quests.HasDenialist(pol));
            Assert.IsFalse(_quests.HasPollyanna(pol)); // distinct from Naive Son trait_pollyanna
            Assert.AreEqual(0f, _quests.GetDisplayedRadiationAnxiety(pol, 90f), 0.01f);
            Assert.IsTrue(_quests.WantsToWalkOutsideInFalloutStorm(pol));

            _quests.TryStartQuestline(pol, "test", 1);
            _quests.RecordSurvivedAcuteRadiationSyndrome(pol, contractedArs: true, survived: false, currentDay: 2);
            Assert.IsFalse(_quests.HasGroundedOptimist(pol));
            _quests.RecordSurvivedAcuteRadiationSyndrome(pol, contractedArs: true, survived: true, currentDay: 3);
            Assert.IsTrue(_quests.HasGroundedOptimist(pol));
            Assert.IsFalse(_quests.HasDenialist(pol));
            Assert.Greater(_quests.GetGroundedOptimistMoraleBuff(pol, hardship01: 0.8f),
                _quests.GetGroundedOptimistMoraleBuff(pol, hardship01: 0.1f));
        }

        // ── #265 Selfless Martyr / Living Saint ──────────────────────────

        [Test]
        public void Martyr_Sacrificial_UltimatePrice_UnlocksLivingSaint()
        {
            var mar = MakeArchetype(PersonalQuestSystem.MartyrId);
            var ally = new Survivor { Id = "ally_m", DisplayName = "Ally", State = SurvivorState.Idle };
            ally.Needs.Health = 80f;
            ally.Needs.Hunger = 10f;
            _survivors.Add(ally);
            Assert.IsTrue(_quests.HasSacrificial(mar));
            Assert.IsTrue(_quests.ShouldInterceptHatchBreachDamage(mar, ally));
            mar.Needs.Health = 100f;
            float left = _quests.InterceptHatchBreachDamage(mar, ally, 30f);
            Assert.AreEqual(0f, left, 0.01f);
            Assert.AreEqual(70f, mar.Needs.Health, 0.01f);
            Assert.AreEqual(80f, ally.Needs.Health, 0.01f);
            mar.Needs.Hunger = 20f;
            Assert.IsTrue(_quests.TrySecretlyGiveFoodRation(mar, ally));
            Assert.Greater(mar.Needs.Hunger, 20f);

            _quests.TryStartQuestline(mar, "test", 1);
            _quests.RecordTookLethalPhase2ForOther(mar, viaEventChoice: true, isPhase2Lethal: true, currentDay: 2);
            Assert.IsTrue(_quests.HasLivingSaint(mar));
            _quests.NotifySurvivorDied(mar);
            Assert.IsTrue(_quests.LivingSaintInspiredActive);
            Assert.AreEqual(50f, _quests.GetLivingSaintMoraleFloor(), 0.01f);
            ally.Needs.Morale = 20f;
            _quests.ApplyLivingSaintMoraleFloor(ally);
            Assert.AreEqual(50f, ally.Needs.Morale, 0.01f);
        }

        // ── #266 Arrogant Surgeon / Humbled Healer ───────────────────────

        [Test]
        public void ArrogantSurgeon_GodComplex_BotchedJob_UnlocksHumbled()
        {
            var surg = MakeArchetype(PersonalQuestSystem.ArrogantSurgeonId);
            Assert.IsTrue(_quests.HasGodComplex(surg));
            Assert.IsTrue(_quests.RefusesMenialLabor(surg));
            Assert.IsTrue(_quests.IsMenialLaborAction("clean_floor"));
            Assert.AreEqual(100f, _quests.GetStartingMedicalSkill(surg), 0.01f);
            Assert.AreEqual(-10f, _quests.GetPatientMoraleAfterHealDelta(surg), 0.01f);

            var patient = new Survivor { Id = "pat_s", DisplayName = "Pat", State = SurvivorState.Idle };
            patient.Needs.Morale = 50f;
            _survivors.Add(patient);
            _quests.ApplyPatientMoraleAfterHeal(surg, patient);
            Assert.AreEqual(40f, patient.Needs.Morale, 0.01f);

            _quests.TryStartQuestline(surg, "test", 1);
            _quests.RecordDepressionDay(surg, currentDay: 1); // no fail yet
            Assert.IsFalse(_quests.HasHumbledHealer(surg));
            _quests.RecordCriticalSurgeryFailed(surg, wasCritical: true, currentDay: 2);
            Assert.AreEqual(PersonalQuestSystem.DepressionBreakId, surg.currentMentalBreakId);
            for (int d = 0; d < 9; d++)
                _quests.RecordDepressionDay(surg, currentDay: 3 + d);
            Assert.IsFalse(_quests.HasHumbledHealer(surg));
            _quests.RecordDepressionDay(surg, currentDay: 20);
            Assert.IsTrue(_quests.HasHumbledHealer(surg));
            Assert.IsFalse(_quests.HasGodComplex(surg));
            Assert.AreEqual(0f, _quests.GetPatientMoraleAfterHealDelta(surg), 0.01f);
            Assert.IsTrue(_quests.CanCureChronicDisabilities(surg));
        }

        // ── #267 Relapsing Addict / Clean & Sober ────────────────────────

        [Test]
        public void RelapsingAddict_ForcedConsume_ColdTurkey_UnlocksCleanAndSober()
        {
            var ad = MakeArchetype(PersonalQuestSystem.RelapsingAddictId);
            Assert.IsTrue(ad.HasTrait("addicted"));
            ad.Needs.Morale = 30f;
            Assert.IsTrue(_quests.ShouldForceConsumeMedicalChems(ad));
            ad.Needs.Morale = 50f;
            Assert.IsFalse(_quests.ShouldForceConsumeMedicalChems(ad));

            _quests.TryStartQuestline(ad, "test", 1);
            for (int d = 0; d < 20; d++)
                _quests.RecordColdTurkeyCleanDay(ad, usedAnyChem: false, currentDay: d + 1);
            Assert.IsFalse(_quests.HasCleanAndSober(ad));
            _quests.RecordColdTurkeyCleanDay(ad, usedAnyChem: true, currentDay: 21);
            Assert.AreEqual(0, _quests.GetState(ad.Id).ColdTurkeyCleanDays);
            for (int d = 0; d < 21; d++)
                _quests.RecordColdTurkeyCleanDay(ad, usedAnyChem: false, currentDay: 30 + d);
            Assert.IsTrue(_quests.HasCleanAndSober(ad));
            Assert.AreEqual(2f, _quests.GetCleanAndSoberStaminaMultiplier(ad), 0.01f);
            Assert.IsTrue(_quests.IsImmuneToChemicalAddiction(ad));
            ad.Needs.Morale = 10f;
            Assert.IsFalse(_quests.ShouldForceConsumeMedicalChems(ad));
        }

        // ── #268 Insomniac / The Watcher ─────────────────────────────────

        [Test]
        public void Insomniac_Restless_LongNight_UnlocksWatcher()
        {
            var ins = MakeArchetype(PersonalQuestSystem.InsomniacId);
            Assert.IsTrue(_quests.HasRestless(ins));
            Assert.AreEqual(0.20f, _quests.GetSleepFatigueRestoreMultiplier(ins), 0.01f);
            Assert.AreEqual(80f, _quests.GetMaxFatigueCap(ins), 0.01f);
            Assert.IsTrue(_quests.GeneratesNightPaceNoise(ins));
            Assert.AreEqual(8f, _quests.GetNightPaceNoisePerHour(ins), 0.01f);

            _quests.TryStartQuestline(ins, "test", 1);
            for (int n = 0; n < 4; n++)
                _quests.RecordLongNightGuardNight(ins, guardedAlone: true, slept: false, currentDay: n + 1);
            Assert.IsFalse(_quests.HasTheWatcher(ins));
            _quests.RecordLongNightGuardNight(ins, guardedAlone: true, slept: true, currentDay: 5);
            Assert.AreEqual(0, _quests.GetState(ins.Id).LongNightGuardNights);
            for (int n = 0; n < 5; n++)
                _quests.RecordLongNightGuardNight(ins, guardedAlone: true, slept: false, currentDay: 10 + n);
            Assert.IsTrue(_quests.HasTheWatcher(ins));
            Assert.IsFalse(_quests.GeneratesNightPaceNoise(ins));
            Assert.IsTrue(_quests.IgnoresFatigueCombatPenalties(ins));
        }

        // ── #269 Hypochondriac / Hyper-Aware ─────────────────────────────

        [Test]
        public void Hypochondriac_FakeAlerts_Sepsis_UnlocksHyperAware()
        {
            var hy = MakeArchetype(PersonalQuestSystem.HypochondriacId);
            Assert.IsTrue(_quests.HasParanoidHealth(hy));
            Assert.IsTrue(_quests.ShouldGenerateFakeAfflictionAlert(hy));
            hy.Needs.Morale = 50f;
            hy.Needs.Fatigue = 10f;
            _quests.ApplyHypochondriacPlaceboTick(hy, givenPlacebo: false);
            Assert.Less(hy.Needs.Morale, 50f);
            Assert.Greater(hy.Needs.Fatigue, 10f);
            float m = hy.Needs.Morale;
            _quests.ApplyHypochondriacPlaceboTick(hy, givenPlacebo: true);
            Assert.Greater(hy.Needs.Morale, m);

            _quests.TryStartQuestline(hy, "test", 1);
            _quests.RecordSepsisSurvived(hy, contractedSepsis: true, survived: false, currentDay: 1);
            Assert.IsFalse(_quests.HasHyperAware(hy));
            _quests.RecordSepsisSurvived(hy, contractedSepsis: true, survived: true, currentDay: 2);
            Assert.IsTrue(_quests.HasHyperAware(hy));
            Assert.IsFalse(_quests.ShouldGenerateFakeAfflictionAlert(hy));
            Assert.IsTrue(_quests.IsImmuneToContaminationSpread(hy));
        }

        // ── #270 Pyromaniac / Fire-Breather ──────────────────────────────

        [Test]
        public void Pyromaniac_Fascination_TrialByFire_UnlocksFireBreather()
        {
            var py = MakeArchetype(PersonalQuestSystem.PyromaniacId);
            Assert.IsTrue(_quests.HasFascination(py));
            py.Needs.Morale = 40f;
            _quests.ApplyFascinationHeaterMorale(py, nearRunningHeatOrPower: true, gameHours: 1f);
            Assert.Greater(py.Needs.Morale, 40f);
            py.Needs.Morale = 20f;
            int starts = 0;
            var rng = new System.Random(1);
            for (int i = 0; i < 200; i++)
                if (_quests.ShouldDeliberatelyStartFire(py, rng)) starts++;
            Assert.Greater(starts, 0);

            _quests.TryStartQuestline(py, "test", 1);
            for (int i = 0; i < 4; i++)
                _quests.RecordBunkerFireExtinguished(py, currentDay: i + 1);
            Assert.IsFalse(_quests.HasFireBreather(py));
            _quests.RecordBunkerFireExtinguished(py, currentDay: 5);
            Assert.IsTrue(_quests.HasFireBreather(py));
            Assert.IsTrue(_quests.CanCraftIncendiaryWeapons(py));
            Assert.IsFalse(_quests.ShouldDeliberatelyStartFire(py, new System.Random(1)));
        }

        // ── #271 Blind Preacher / Sonar ──────────────────────────────────

        [Test]
        public void BlindPreacher_Converts_UnlocksSonar()
        {
            var bp = MakeArchetype(PersonalQuestSystem.BlindPreacherId);
            Assert.IsTrue(_quests.HasBlind(bp));
            Assert.IsTrue(bp.CannotFight);
            Assert.IsFalse(_quests.CanFireGuns(bp));
            Assert.IsTrue(_quests.NavigatesBySoundOnly(bp));

            var t1 = new Survivor { Id = "d1", DisplayName = "D1", State = SurvivorState.Idle };
            t1.currentMentalBreakId = PersonalQuestSystem.DespairBreakId;
            t1.Needs.Morale = 5f;
            _survivors.Add(t1);
            var t2 = new Survivor { Id = "d2", DisplayName = "D2", State = SurvivorState.Idle };
            t2.currentMentalBreakId = PersonalQuestSystem.DespairBreakId;
            _survivors.Add(t2);
            var t3 = new Survivor { Id = "d3", DisplayName = "D3", State = SurvivorState.Idle };
            t3.currentMentalBreakId = PersonalQuestSystem.DespairBreakId;
            _survivors.Add(t3);

            _quests.TryStartQuestline(bp, "test", 1);
            _quests.RecordDespairToHopeConversion(bp, t1, viaDialogue: true, currentDay: 1);
            _quests.RecordDespairToHopeConversion(bp, t2, viaDialogue: true, currentDay: 2);
            Assert.IsFalse(_quests.HasSonar(bp));
            _quests.RecordDespairToHopeConversion(bp, t3, viaDialogue: true, currentDay: 3);
            Assert.IsTrue(_quests.HasSonar(bp));
            Assert.AreEqual(12f, _quests.GetSonarWarningHours(bp), 0.01f);
            Assert.IsTrue(_quests.AnySonarEarlyWarning(_survivors));
            Assert.IsNull(t1.currentMentalBreakId);
        }

        // ── #272 Prepper / Improvised Engineering ────────────────────────

        [Test]
        public void Prepper_MreOnly_HatchDestroyed_UnlocksImprovised()
        {
            var pr = MakeArchetype(PersonalQuestSystem.PrepperId);
            Assert.IsTrue(_quests.HasParanoid(pr));
            Assert.GreaterOrEqual(pr.RadiationAnxiety, 0.75f);
            Assert.IsTrue(pr.HiddenItemIds.Contains("mre_prewar"));
            Assert.IsTrue(_quests.WillOnlyEatOwnMres(pr));
            Assert.IsTrue(_quests.TryConsumePrepperMre(pr));

            _quests.TryStartQuestline(pr, "test", 1);
            _quests.RecordHatchDestroyedRaidSurvived(pr, hatchDestroyed: true, survived: false, currentDay: 1);
            Assert.IsFalse(_quests.HasImprovisedEngineering(pr));
            _quests.RecordHatchDestroyedRaidSurvived(pr, hatchDestroyed: true, survived: true, currentDay: 2);
            Assert.IsTrue(_quests.HasImprovisedEngineering(pr));
            Assert.IsTrue(_quests.CanBuildModulesFromJunkOnly(pr));
        }

        // ── #273 Mutated Outcast / Radiotrophic ──────────────────────────

        [Test]
        public void Outcast_RoomMeal_1000mSv_UnlocksRadiotrophic()
        {
            var oc = MakeArchetype(PersonalQuestSystem.OutcastId);
            Assert.AreEqual(800f, oc.LifetimeRadiationExposure, 0.01f);
            var diner = new Survivor { Id = "din", DisplayName = "D", State = SurvivorState.Idle, CurrentRoomId = "mess" };
            oc.CurrentRoomId = "mess";
            diner.Needs.Morale = 50f;
            _survivors.Add(diner);
            _quests.ApplyOutcastRoomMealMorale(oc, diner);
            Assert.AreEqual(47f, diner.Needs.Morale, 0.01f);

            _quests.TryStartQuestline(oc, "test", 1);
            _quests.RecordLifetimeRadsMilestone(oc, lifetimeMsv: 999f, isAlive: true, currentDay: 1);
            Assert.IsFalse(_quests.HasRadiotrophic(oc));
            _quests.RecordLifetimeRadsMilestone(oc, lifetimeMsv: 1000f, isAlive: true, currentDay: 2);
            Assert.IsTrue(_quests.HasRadiotrophic(oc));
            oc.Needs.Health = 50f;
            oc.Needs.Fatigue = 40f;
            _quests.ApplyRadiotrophicTick(oc, zoneRadPerHour: 80f, gameHours: 1f);
            Assert.Greater(oc.Needs.Health, 50f);
            Assert.Less(oc.Needs.Fatigue, 40f);
        }

        // ── #274 Feral Orphan / Apex Scavenger ───────────────────────────

        [Test]
        public void FeralOrphan_PackTraining_UnlocksApexScavenger()
        {
            var fo = MakeArchetype(PersonalQuestSystem.FeralOrphanId);
            Assert.IsTrue(fo.IsChild);
            Assert.IsTrue(_quests.HasAnimalistic(fo));
            Assert.IsTrue(_quests.EatsOnlyRawMeat(fo));
            Assert.IsTrue(_quests.PrefersFloorSleep(fo));
            var stranger = new Survivor { Id = "str", DisplayName = "S", State = SurvivorState.Idle };
            Assert.IsTrue(_quests.BitesWhenHealedByStranger(fo, stranger));
            var vet = MakeArchetype(PersonalQuestSystem.VetId, "vet_t");
            Assert.IsFalse(_quests.BitesWhenHealedByStranger(fo, vet));

            _quests.TryStartQuestline(fo, "test", 1);
            for (int d = 0; d < 29; d++)
                _quests.RecordPackTrainingDay(fo, vet, trainedToday: true, currentDay: d + 1);
            Assert.IsFalse(_quests.HasApexScavenger(fo));
            _quests.RecordPackTrainingDay(fo, vet, trainedToday: true, currentDay: 30);
            Assert.IsTrue(_quests.HasApexScavenger(fo));
            Assert.IsTrue(_quests.CanUseTools(fo));
            Assert.IsTrue(_quests.HasZoonoticExpertInherited(fo));
        }

        // ── #275 Pacifist / Zen State ────────────────────────────────────

        [Test]
        public void Pacifist_HungerStrike_ZeroDamageNode_UnlocksZen()
        {
            var mon = MakeArchetype(PersonalQuestSystem.PacifistId);
            Assert.IsTrue(_quests.HasVowOfNonviolence(mon));
            Assert.IsTrue(mon.CannotFight);
            Assert.IsTrue(_quests.AutoFleesAllEncounters(mon));
            var killer = new Survivor { Id = "k", DisplayName = "K", State = SurvivorState.Idle };
            _quests.NotifyNeedlessKill(mon, killer, wasNeedless: true);
            Assert.IsTrue(_quests.IsOnHungerStrike(mon));
            Assert.IsTrue(_quests.RefusesToEat(mon));
            mon.Needs.Morale = 60f;
            _quests.TickHungerStrike(mon);
            Assert.IsFalse(_quests.IsOnHungerStrike(mon));

            _quests.TryStartQuestline(mon, "test", 1);
            _quests.RecordPacifistDangerNode(mon, dangerLevel: 5, damageDealt: 1f, completedNode: true, currentDay: 1);
            Assert.IsFalse(_quests.HasZenState(mon));
            _quests.RecordPacifistDangerNode(mon, dangerLevel: 5, damageDealt: 0f, completedNode: true, currentDay: 2);
            Assert.IsTrue(_quests.HasZenState(mon));
            Assert.AreEqual(0.20f, _quests.GetZenNeedsDecayMultiplier(mon), 0.01f);
        }

        // ── #276 Widow / Master Geneticist ───────────────────────────────

        [Test]
        public void Widow_Grieving_PreWarRose_UnlocksGeneticist()
        {
            var wi = MakeArchetype(PersonalQuestSystem.WidowId);
            Assert.IsTrue(_quests.HasGrieving(wi));
            Assert.AreEqual(0.55f, _quests.GetGrievingActionEfficiencyMultiplier(wi), 0.01f);
            Assert.IsTrue(_quests.PrioritizesHydroponicsOverSleep(wi));
            Assert.IsTrue(_quests.IsHydroponicsAction("tend_hydroponics"));

            _quests.TryStartQuestline(wi, "test", 1);
            _quests.RecordPreWarRoseGrown(wi, cropId: "tomato", harvested: true, currentDay: 1);
            Assert.IsFalse(_quests.HasMasterGeneticist(wi));
            _quests.RecordPreWarRoseGrown(wi, cropId: PersonalQuestSystem.PreWarRoseItemId, harvested: true, currentDay: 2);
            Assert.IsTrue(_quests.HasMasterGeneticist(wi));
            Assert.IsFalse(_quests.HasGrieving(wi));
            Assert.IsTrue(_quests.CanCrossBreedMedicinalFood(wi));
        }

        // ── #277 Ex-Con / The Enforcer ───────────────────────────────────

        [Test]
        public void ExCon_Distrusted_DragWounded_UnlocksEnforcer()
        {
            var ex = MakeArchetype(PersonalQuestSystem.ExConId);
            Assert.IsTrue(_quests.HasDistrusted(ex));
            var other = new Survivor { Id = "o1", DisplayName = "O", State = SurvivorState.Idle, CurrentRoomId = "bunk" };
            ex.CurrentRoomId = "bunk";
            _survivors.Add(other);
            Assert.IsTrue(_quests.CausesPersonalStashLock(ex, other));
            var cop = MakeArchetype(PersonalQuestSystem.CopId, "cop1");
            Assert.IsTrue(_quests.RefusesOrdersFrom(ex, cop));
            Assert.AreEqual(1.25f, _quests.GetExConPhysicalLaborMultiplier(ex), 0.01f);

            var wounded = new Survivor { Id = "w", DisplayName = "W", State = SurvivorState.Idle };
            wounded.Needs.Health = 5f;
            _survivors.Add(wounded);
            _quests.TryStartQuestline(ex, "test", 1);
            _quests.RecordDraggedWoundedHome(ex, wounded, fromExpedition: true, woundedWasDying: true, madeItHome: true, currentDay: 2);
            Assert.IsTrue(_quests.HasTheEnforcer(ex));
            wounded.currentMentalBreakId = "fight";
            Assert.IsTrue(_quests.TryIntimidateEndMentalBreak(ex, wounded));
            Assert.IsNull(wounded.currentMentalBreakId);
        }

        // ── #278 Sheriff / Legend of the Wastes ──────────────────────────

        [Test]
        public void Sheriff_MoralCompass_RaiderBoss_UnlocksLegend()
        {
            var sh = MakeArchetype(PersonalQuestSystem.SheriffId);
            Assert.IsTrue(_quests.HasMoralCompass(sh));
            Assert.IsTrue(_quests.HasFailingHeart(sh));
            Assert.AreEqual(3f, _quests.GetMoralCompassBunkerMorale(_survivors), 0.01f);
            sh.Needs.Morale = 80f;
            _quests.ApplyMoralCompassEvilChoice(sh, evilChoice: true);
            Assert.Less(sh.Needs.Morale, 80f);
            float stam = _quests.GetFailingHeartStaminaMax(sh, daysProgressed: 40);
            Assert.Less(stam, 100f);
            Assert.IsTrue(_quests.ShouldAutoAssignGuard(sh, someoneElseGuarding: false));

            _quests.TryStartQuestline(sh, "test", 1);
            _quests.RecordRaiderBossExecuted(sh, wasRaiderBoss: true, executed: true, currentDay: 2);
            Assert.IsTrue(_quests.HasLegendOfTheWastes(sh));
            Assert.AreEqual(0.25f, _quests.GetLegendRaidFrequencyMultiplier(_survivors), 0.01f);
        }

        // ── #279 Former Politician / The Statesman ───────────────────────

        [Test]
        public void FormerPolitician_DirtyDays_UnlocksStatesman()
        {
            var pol = MakeArchetype(PersonalQuestSystem.FormerPoliticianId);
            Assert.IsTrue(_quests.HasSilverTongue(pol));
            Assert.AreEqual(0f, _quests.GetManualLaborSkillCap(pol), 0.01f);
            Assert.AreEqual(100f, _quests.GetSilverTongueCharisma(pol), 0.01f);
            Assert.IsTrue(_quests.TriesToDelegateTasks(pol));
            pol.Needs.Morale = 80f;
            _quests.ApplyDirtyLaborMorale(pol, "clean_waste");
            Assert.Less(pol.Needs.Morale, 80f);

            _quests.TryStartQuestline(pol, "test", 1);
            for (int d = 0; d < 13; d++)
                _quests.RecordDirtyLaborDay(pol, didDirtyJob: true, currentDay: d + 1);
            Assert.IsFalse(_quests.HasTheStatesman(pol));
            _quests.RecordDirtyLaborDay(pol, didDirtyJob: false, currentDay: 14);
            Assert.AreEqual(0, _quests.GetState(pol.Id).RealLeaderDirtyDays);
            for (int d = 0; d < 14; d++)
                _quests.RecordDirtyLaborDay(pol, didDirtyJob: true, currentDay: 20 + d);
            Assert.IsTrue(_quests.HasTheStatesman(pol));
            Assert.IsTrue(_quests.CanMergeFactionsViaRadio(pol));
        }

        // ── #280 Tech Bro / Cybernetics ──────────────────────────────────

        [Test]
        public void TechBro_PowerWaste_ManualPurifier_UnlocksCybernetics()
        {
            var tb = MakeArchetype(PersonalQuestSystem.TechBroId);
            Assert.IsTrue(_quests.HasDelusional(tb));
            Assert.IsTrue(_quests.WastesPowerOnTablet(tb, supervised: false));
            Assert.AreEqual(15f, _quests.GetTechBroPowerWasteWatts(tb, supervised: false), 0.01f);
            Assert.AreEqual(0f, _quests.GetTechBroPowerWasteWatts(tb, supervised: true), 0.01f);

            _quests.TryStartQuestline(tb, "test", 1);
            _quests.RecordManualWaterPurifierBuilt(tb, builtFromScrap: true, isManual: true, currentDay: 1);
            Assert.IsFalse(_quests.HasCybernetics(tb)); // tablet still alive
            _quests.RecordTabletDestroyedByEmp(tb, empHit: true, currentDay: 2);
            Assert.IsTrue(_quests.GetState(tb.Id).TechTabletDead);
            Assert.IsFalse(_quests.WastesPowerOnTablet(tb, supervised: false));
            _quests.RecordManualWaterPurifierBuilt(tb, builtFromScrap: true, isManual: true, currentDay: 3);
            Assert.IsTrue(_quests.HasCybernetics(tb));
            Assert.IsTrue(_quests.CanCraftAutoTurrets(tb));
        }

        // ── #281 News Anchor / Beacon of Truth ───────────────────────────

        [Test]
        public void NewsAnchor_JournalSpam_Broadcast_UnlocksBeacon()
        {
            var an = MakeArchetype(PersonalQuestSystem.NewsAnchorId);
            Assert.IsTrue(_quests.HasPhotogenic(an));
            Assert.AreEqual(5f, _quests.GetPhotogenicHygieneMoraleHit(an, hygiene01: 0.2f), 0.01f);
            Assert.IsTrue(_quests.SpamsJournalEntries(an));
            Assert.AreEqual(3, _quests.GetJournalEntriesPerDay(an));

            _quests.TryStartQuestline(an, "test", 1);
            _quests.RecordFinalBroadcast(an, nodeId: "the_radio_tower", broadcastTruth: true, currentDay: 2);
            Assert.IsTrue(_quests.HasBeaconOfTruth(an));
            Assert.AreEqual(0.70f, _quests.GetBeaconTradePriceMultiplier(_survivors), 0.01f);
        }

        // ── #282 Nomad / Master Pathologist ──────────────────────────────

        [Test]
        public void Nomad_Agoraphile_BedModule_UnlocksPathologist()
        {
            var no = MakeArchetype(PersonalQuestSystem.NomadId);
            Assert.IsTrue(_quests.HasAgoraphile(no));
            Assert.IsTrue(_quests.PacesAtHatch(no));
            no.Needs.Morale = 50f;
            _quests.ApplyAgoraphileBunkerDay(no, spentDayInside: true);
            Assert.Less(no.Needs.Morale, 50f);
            for (int d = 0; d < 4; d++)
                _quests.ApplyAgoraphileBunkerDay(no, spentDayInside: true);
            Assert.IsTrue(_quests.ShouldLeaveBunkerOnOwn(no));

            _quests.TryStartQuestline(no, "test", 1);
            _quests.RecordPersonalBedModuleFullyUpgraded(no, isPersonalRoom: true, fullyUpgraded: true, currentDay: 2);
            Assert.IsTrue(_quests.HasMasterPathologist(no));
            Assert.IsTrue(_quests.IsImmuneToWeatherEffects(no));
            Assert.IsTrue(_quests.CanScavengeInLethalWeather(no));
            Assert.IsFalse(_quests.ShouldLeaveBunkerOnOwn(no));
        }

        // ── #283 Exec / Monopolist ───────────────────────────────────────

        [Test]
        public void Exec_Ruthless_TradeValue_UnlocksMonopolist()
        {
            var ex = MakeArchetype(PersonalQuestSystem.ExecId);
            Assert.IsTrue(_quests.HasRuthless(ex));
            Assert.AreEqual(1.20f, _quests.GetRuthlessModuleEfficiencyMultiplier(ex), 0.01f);
            Assert.AreEqual(1.35f, _quests.GetRuthlessModuleWearMultiplier(ex), 0.01f);
            Assert.IsTrue(_quests.PrioritizesLootOverLivesInFire(ex));

            _quests.TryStartQuestline(ex, "test", 1);
            _quests.RecordBunkerTradeValue(ex, totalTradeValue: 9999f, currentDay: 1);
            Assert.IsFalse(_quests.HasMonopolist(ex));
            _quests.RecordBunkerTradeValue(ex, totalTradeValue: 10000f, currentDay: 2);
            Assert.IsTrue(_quests.HasMonopolist(ex));
            Assert.IsTrue(_quests.CanBuyOutFactionInventories(ex));
        }
    }
}
