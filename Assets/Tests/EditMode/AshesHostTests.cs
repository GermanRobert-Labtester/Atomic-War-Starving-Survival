using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Economy;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Host-integration tests for the ashes / improbable / final flawed cohort
    /// (#299–#318). Pure C# — Water, Structural, Radio, Sleep/Craft actions,
    /// Excavation, PowerNetwork, Medical, Economy with PersonalQuestSystem bound.
    /// </summary>
    [TestFixture]
    public class AshesHostTests : PersonalQuestHostTestBase
    {
        // ── #299 Prom Queen ──────────────────────────────────────────────

        [Test]
        public void WaterEconomy_PromQueen_BurnsCleanWaterDaily()
        {
            var q = MakeArchetype(PersonalQuestSystem.PromQueenId, "queen");
            Assert.Greater(_quests.GetMakeupCleanWaterBurn(q), 0f);

            var shelter = new Shelter();
            var storage = new WaterStorage();
            storage.AddClean(100f);

            var water = new WaterEconomySystem();
            water.BindPersonalQuests(_quests, () => _survivors);
            water.Tick(24f, WeatherKind.Clear, currentDay: 1, shelter: shelter, storage: storage);

            Assert.Less(storage.CleanWater, 100f);
        }

        // ── #300 Jock ────────────────────────────────────────────────────

        [Test]
        public void StructuralIntegrity_Jock_PunchesWallsWhenMoraleLow()
        {
            var j = MakeArchetype(PersonalQuestSystem.JockId, "jock");
            j.Needs.Morale = 5f;
            Assert.Greater(_quests.GetWallPunchStructuralDamage(j), 0f);

            var shelter = new Shelter();
            var structural = new StructuralIntegritySystem();
            structural.Bind(() => shelter, () => _survivors);
            structural.BindPersonalQuests(_quests);

            float before = structural.Integrity;
            structural.Tick(1f, shelter);
            Assert.Less(structural.Integrity, before);
        }

        // ── #301 Med Student ─────────────────────────────────────────────

        [Test]
        public void Medical_MedStudent_PukesOnFirstTraumaTreatment()
        {
            var student = MakeArchetype(PersonalQuestSystem.MedStudentId, "student");
            var patient = new Survivor { Id = "p1", DisplayName = "P", State = SurvivorState.Idle };
            patient.Needs.Health = 40f;
            _survivors.Add(patient);

            var bandage = Track(MakeItem("bandage", ItemType.Medical));
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            inv.Add(bandage, 4);

            var defs = MedicalSystem.CreateDefaultAfflictions();
            for (int i = 0; i < defs.Count; i++)
                Track(defs[i]);

            var med = new MedicalSystem(_needs, inv);
            for (int i = 0; i < defs.Count; i++)
                med.RegisterAffliction(defs[i]);
            med.BindPersonalQuests(_quests);

            var recipe = Track(MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
            recipe.haltOnly = false;
            recipe.healthRestoreOnCure = 5f;
            recipe.requiresMedicalBed = false;
            med.RegisterTreatment(recipe);

            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.GunshotWound));
            float hygieneBefore = student.Needs.Hygiene;
            Assert.IsTrue(med.TryStartTreatment(student, patient, recipe));
            Assert.Less(student.Needs.Hygiene, hygieneBefore);
        }

        // ── #302 Gamer ───────────────────────────────────────────────────

        [Test]
        public void ActionScorer_Gamer_FavorsCoopWorkAtNight()
        {
            var g = MakeArchetype(PersonalQuestSystem.GamerId, "gamer");
            g.Needs.Fatigue = 90f;
            var sleep = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            var scorer = new ActionScorer();
            var dayCtx = new AIContext(g, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                IsNight = false
            };
            var nightCtx = new AIContext(g, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                IsNight = true
            };
            float dayScore = scorer.Score(sleep, dayCtx);
            float nightScore = scorer.Score(sleep, nightCtx);
            Assert.Greater(nightScore, dayScore);
        }

        // ── #303 Choir Boy ───────────────────────────────────────────────

        [Test]
        public void CorpseManagement_ChoirBoy_SoftensAmbientMoralePenalty()
        {
            var choir = MakeArchetype(PersonalQuestSystem.ChoirBoyId, "choir");
            var other = new Survivor { Id = "o1", DisplayName = "O", State = SurvivorState.Idle };
            other.Needs.Morale = 80f;
            _survivors.Add(other);

            Assert.Less(_quests.GetCorpseMoralePenaltyMultiplier(_survivors), 1f);

            var inv = new InventoryClass { Capacity = 20, MaxWeight = 200f };
            var corpses = new CorpseManagementSystem(_needs, inv);
            corpses.BindPersonalQuests(_quests);
            Assert.IsTrue(corpses.CreateCorpseFrom(other));

            corpses.Tick(1f, _survivors);

            // Full un-mitigated drain would be exactly MoraleDrainPerCorpsePerHour (3);
            // with Choir Boy present it's softened, so morale stays above that floor.
            Assert.Greater(other.Needs.Morale, 80f - CorpseManagementSystem.MoraleDrainPerCorpsePerHour);
        }

        // ── #304/#305 Twins ──────────────────────────────────────────────

        [Test]
        public void NeedsSystem_TwinBeta_MirrorsAlphaNeeds()
        {
            var alpha = MakeArchetype(PersonalQuestSystem.TwinAlphaId, "alpha");
            var beta = MakeArchetype(PersonalQuestSystem.TwinBetaId, "beta");
            _quests.LinkTwins(alpha, beta);

            alpha.Needs.Hunger = 70f;
            beta.Needs.Hunger = 10f;
            _needs.Tick(beta, 1f);
            Assert.AreEqual(alpha.Needs.Hunger, beta.Needs.Hunger, Eps);
        }

        [Test]
        public void NeedsSystem_TwinDeath_TriggersSymbioticBondReaction()
        {
            var alpha = MakeArchetype(PersonalQuestSystem.TwinAlphaId, "alpha2");
            var beta = MakeArchetype(PersonalQuestSystem.TwinBetaId, "beta2");
            _quests.LinkTwins(alpha, beta);

            beta.Needs.Health = 10f;
            _needs.Modify(beta, NeedKind.Health, -100f);
            Assert.AreEqual(SurvivorState.Incapacitated, alpha.State);
        }

        // ── #306 Theorist ────────────────────────────────────────────────

        [Test]
        public void RadioTuner_Theorist_SabotagesRadio()
        {
            var t = MakeArchetype(PersonalQuestSystem.TheoristId, "theorist");

            System.Random sabotageRng = null;
            for (int seed = 0; seed < 5000; seed++)
            {
                var candidate = new System.Random(seed);
                if (_quests.ShouldSabotageRadio(t, candidate))
                {
                    sabotageRng = new System.Random(seed);
                    break;
                }
            }
            Assert.IsNotNull(sabotageRng, "expected to find a sabotage-triggering seed");

            var freq = Track(ScriptableObject.CreateInstance<RadioFrequencySO>());
            freq.id = "f1";
            freq.displayName = "Test Freq";
            freq.baseSignalStrength = 0.9f;

            var radio = new RadioTunerSystem(sabotageRng, () => 1);
            radio.BindPersonalQuests(_quests, () => _survivors);
            radio.SetFrequencies(new[] { freq });
            radio.TuneToFrequency("f1");
            Assert.IsFalse(string.IsNullOrEmpty(radio.State.CurrentFrequencyId));

            radio.Tick(1f, WeatherKind.Clear, 1);
            Assert.IsTrue(string.IsNullOrEmpty(radio.State.CurrentFrequencyId));
        }

        // ── #307 Hermit ──────────────────────────────────────────────────

        [Test]
        public void SleepAction_Hermit_RedirectsToAirlockWhenCrowded()
        {
            var h = MakeArchetype(PersonalQuestSystem.HermitId, "hermit");
            for (int i = 0; i < 3; i++)
            {
                var s = new Survivor { Id = "pop" + i, DisplayName = "P" + i, State = SurvivorState.Idle };
                _survivors.Add(s);
            }
            Assert.AreEqual(
                PersonalQuestSystem.AirlockRoomId,
                _quests.GetPreferredSleepRoomWhenCrowded(h, _survivors.Count));

            var ctx = new AIContext(h, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                SleepRoomId = "bunk_room"
            };
            var conditions = SleepActionSO.ResolveConditions(ctx);
            Assert.AreEqual(PersonalQuestSystem.AirlockRoomId, conditions.SleepRoomId);
        }

        // ── #308 Patient ─────────────────────────────────────────────────

        [Test]
        public void TickDaily_Patient_AwakensAfterHydratedStreak()
        {
            var p = MakeArchetype(PersonalQuestSystem.PatientId, "patient");
            Assert.IsTrue(_quests.IsComatoseBurden(p));
            p.Needs.Thirst = 10f;
            p.Needs.Hunger = 10f;
            for (int d = 0; d < PersonalQuestSystem.PatientAwakeningDaysRequired; d++)
                _quests.TickDaily(_survivors, currentDay: d + 1);
            Assert.IsFalse(_quests.IsComatoseBurden(p));
        }

        // ── #309 Escapee ─────────────────────────────────────────────────

        [Test]
        public void MentalBreak_Escapee_IronWillBlocksAllBreaks()
        {
            var e = MakeArchetype(PersonalQuestSystem.EscapeeId, "escapee");
            _quests.TryStartQuestline(e, "test", 1);
            _quests.RecordBreakingChainsEmissaryKill(e, assassinatedCultEmissaryAtHatch: true, currentDay: 2);
            Assert.IsTrue(_quests.IsImmuneToAllMentalBreaks(e));

            var mb = new MentalBreakSystem();
            mb.BindPersonalQuests(_quests, () => _survivors);
            var brk = Track(ScriptableObject.CreateInstance<MentalBreakSO>());
            brk.id = "test_break";
            mb.RegisterBreak(brk);

            e.lowMoraleHours = MentalBreakSystem.LowMoraleBreakWindowHours; // clear the morale gate
            bool triggered = mb.TryRollForBreak(e, new System.Random(1));
            Assert.IsFalse(triggered);
            Assert.IsTrue(string.IsNullOrEmpty(e.currentMentalBreakId));
        }

        // ── #310 Stowaway ────────────────────────────────────────────────

        [Test]
        public void HatchDefense_UnseenListener_WarnsOfRaid()
        {
            var s = MakeArchetype(PersonalQuestSystem.StowawayId, "stow");
            _quests.TryStartQuestline(s, "test", 1);
            _quests.RecordEarningKeepHiddenWater(
                s, discoveredHiddenWaterSource: true, savedGroupFromDehydration: true, currentDay: 2);
            Assert.IsTrue(_quests.HasUnseenListener(s));
            Assert.IsTrue(_quests.AnyUnseenListenerWarning(_survivors));

            var hatch = new HatchDefenseSystem(getSurvivors: () => _survivors);
            hatch.BindPersonalQuests(_quests);
            Assert.IsTrue(hatch.HasUnseenListenerRaidWarning());
        }

        // ── #311 Billionaire ─────────────────────────────────────────────

        [Test]
        public void ApplyEntitledLaborDay_Billionaire_HitsMoraleOnPhysicalLabor()
        {
            var b = MakeArchetype(PersonalQuestSystem.BillionaireId, "billionaire");
            b.Needs.Morale = 60f;
            _quests.ApplyEntitledLaborDay(b, didPhysicalLabor: true);
            Assert.Less(b.Needs.Morale, 60f);
        }

        // ── #312 Apprentice ──────────────────────────────────────────────

        [Test]
        public void CraftAction_Apprentice_ClumsyToolBreakSlowsCraft()
        {
            var a = MakeArchetype(PersonalQuestSystem.ApprenticeId, "apprentice");
            Assert.IsTrue(_quests.HasClumsy(a));

            System.Random breakRng = null;
            for (int seed = 0; seed < 5000; seed++)
            {
                if (_quests.RollClumsyToolBreak(a, new System.Random(seed)))
                {
                    breakRng = new System.Random(seed);
                    break;
                }
            }
            Assert.IsNotNull(breakRng, "expected to find a tool-break-triggering seed");

            var ctx = new AIContext(a, null, null, breakRng)
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };
            float mult = CraftActionSO.GetCraftSpeedMultiplier(ctx);
            Assert.Less(mult, 1f);
        }

        // ── #313 Fire Chief ──────────────────────────────────────────────

        [Test]
        public void NeedsSystem_FireChief_ImmuneToFireAndTemperature()
        {
            var f = MakeArchetype(PersonalQuestSystem.FireChiefId, "chief");
            Assert.IsTrue(_quests.IsImmuneToFireAndTemperature(f));
        }

        // ── #314 Amputee ─────────────────────────────────────────────────

        [Test]
        public void Excavation_Amputee_PunchesThroughBlockedHatch()
        {
            var amp = MakeArchetype(PersonalQuestSystem.AmputeeId, "amputee");
            _quests.TryStartQuestline(amp, "test", 1);
            _quests.RecordProstheticParts(
                amp,
                PersonalQuestSystem.ProstheticMechanicalPartsRequired,
                PersonalQuestSystem.ProstheticElectronicScrapRequired,
                2);
            Assert.IsTrue(_quests.HasCyberArm(amp));

            var dig = new ExcavationSystem();
            dig.BindPersonalQuests(_quests);
            dig.SealRoom("r1", rubbleUnits: 10f);
            float cleared = dig.ClearRubble("r1", amp, hasShovel: true, hatchBlocked: true, workHours: 1f);
            Assert.Greater(cleared, 0f);
        }

        // ── #315 Inmate ──────────────────────────────────────────────────

        [Test]
        public void Economy_Inmate_StartsUntrusted()
        {
            var inmate = MakeArchetype(PersonalQuestSystem.InmateId, "inmate");
            var eco = new DynamicEconomySystem();
            eco.BindPersonalQuests(_quests, () => _survivors);
            eco.SetTrust("faction_a", 40f);
            Assert.AreEqual(40f, eco.GetTrust("faction_a"), Eps);
            Assert.AreEqual(0f, eco.GetTrustForSurvivor("faction_a", inmate), Eps);
        }

        // ── #316 Synth ───────────────────────────────────────────────────

        [Test]
        public void NeedsSystem_Synth_DoesNotAccrueHungerOrThirst()
        {
            var synth = MakeArchetype(PersonalQuestSystem.SynthId, "synth");
            synth.Needs.Hunger = 20f;
            synth.Needs.Thirst = 20f;
            _profile.hungerPerHour = 5f;
            _profile.thirstPerHour = 5f;
            _needs.Tick(synth, 2f);
            Assert.AreEqual(20f, synth.Needs.Hunger, Eps);
            Assert.AreEqual(20f, synth.Needs.Thirst, Eps);
        }

        // ── #317 Dog ─────────────────────────────────────────────────────

        [Test]
        public void TickDaily_Dog_ProvidesRoommateMoraleAura()
        {
            var dog = MakeArchetype(PersonalQuestSystem.DogId, "dog");
            var roommate = new Survivor { Id = "rm1", DisplayName = "RM", State = SurvivorState.Idle };
            roommate.Needs.Morale = 40f;
            roommate.CurrentRoomId = "common";
            dog.CurrentRoomId = "common";
            _survivors.Add(roommate);

            _quests.TickDaily(_survivors, currentDay: 1);
            Assert.Greater(roommate.Needs.Morale, 40f);
        }

        // ── #318 Core ────────────────────────────────────────────────────

        [Test]
        public void PowerNetwork_Core_DiesOnBlackout()
        {
            var core = MakeArchetype(PersonalQuestSystem.CoreId, "core");
            Assert.IsTrue(_quests.DiesWhenPowerNetworkFails(core));

            var net = new PowerNetwork();
            net.BindPersonalQuests(_quests, () => _survivors);
            net.AddConsumer(new PowerConsumer { ModuleId = "m1", Watts = 10f, IsRequested = true, Priority = 1 });

            Assert.IsTrue(net.IsBlackout);
            Assert.IsFalse(core.IsAlive);
        }
    }
}
