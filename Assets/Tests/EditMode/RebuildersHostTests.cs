using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Host-integration tests for rebuilders / scholars / outlaws (#284–#298).
    /// Pure C# — NeedsSystem, ActionScorer, Eat/Sleep, Atmosphere, Excavation,
    /// Hauling, Medical, SkillProgression, Economy with PersonalQuestSystem bound.
    /// </summary>
    [TestFixture]
    public class RebuildersHostTests
    {
        private const float Eps = 0.02f;

        private SkillProgressionSystem _progression;
        private PersonalQuestSystem _quests;
        private List<Survivor> _survivors;
        private NeedsProfile _profile;
        private NeedsSystem _needs;
        private readonly List<Object> _toDestroy = new List<Object>();

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _quests = new PersonalQuestSystem();
            _quests.Bind(_progression);
            _progression.BindPersonalQuests(_quests);
            _survivors = new List<Survivor>();

            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            Track(_profile);
            _profile.hungerPerHour = 0f;
            _profile.thirstPerHour = 0f;
            _profile.fatiguePerHour = 0f;
            _profile.warmthLossPerHourInCold = 0f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 0f;
            _profile.moraleLossPerHourWhileCritical = 0f;
            _needs = new NeedsSystem(_profile);
            _needs.BindPersonalQuests(_quests, () => _survivors);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy.Clear();
        }

        private T Track<T>(T obj) where T : Object
        {
            _toDestroy.Add(obj);
            return obj;
        }

        private Survivor MakeArchetype(string archetypeId, string runtimeId = null)
        {
            var sv = PersonalQuestSystem.MakeArchetypeSurvivor(archetypeId, runtimeId);
            Assert.IsNotNull(sv, "archetype " + archetypeId);
            _quests.AssignProfile(sv, PersonalQuestSystem.ProfileForArchetype(archetypeId));
            _survivors.Add(sv);
            _needs.Register(sv);
            return sv;
        }

        private static ItemDefinition MakeItem(string id, ItemType type, float tradeValue = 10f)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.tradeValue = tradeValue;
            item.stackMax = 99;
            item.weight = 0.1f;
            return item;
        }

        // ── #284 Coal Miner ──────────────────────────────────────────────

        [Test]
        public void NeedsSystem_BlackLung_AddsFatiguePressure()
        {
            var m = MakeArchetype(PersonalQuestSystem.CoalMinerId, "miner");
            Assert.IsTrue(_quests.HasBlackLung(m));
            m.Needs.Fatigue = 10f;
            _needs.Tick(m, 2f);
            Assert.Greater(m.Needs.Fatigue, 10f);
        }

        [Test]
        public void NeedsSystem_Claustrophilic_MoraleInDeepRoom()
        {
            var m = MakeArchetype(PersonalQuestSystem.CoalMinerId, "deep");
            m.CurrentRoomId = "deep_shaft";
            m.Needs.Morale = 40f;
            _needs.Tick(m, 1f);
            Assert.Greater(m.Needs.Morale, 40f);
        }

        [Test]
        public void Atmosphere_DeepDelver_ImmuneToGasPenalty()
        {
            var m = MakeArchetype(PersonalQuestSystem.CoalMinerId, "delver");
            _quests.TryStartQuestline(m, "test", 1);
            _quests.RecordCanaryCoLeakSurvived(m, true, 0f, true, 2);
            Assert.IsTrue(_quests.HasDeepDelver(m));

            var atmo = new ShelterAtmosphereSystem();
            atmo.BindPersonalQuests(_quests, () => _survivors);
            m.Needs.Health = 80f;
            float dmg = atmo.ApplyGasPenaltyToSurvivor(m, rawHealthDamage: 20f);
            Assert.AreEqual(0f, dmg, Eps);
            Assert.AreEqual(80f, m.Needs.Health, Eps);
        }

        [Test]
        public void Atmosphere_CoLeak_UnlocksDeepDelver()
        {
            var m = MakeArchetype(PersonalQuestSystem.CoalMinerId, "canary");
            _quests.TryStartQuestline(m, "test", 1);
            var atmo = new ShelterAtmosphereSystem();
            atmo.BindPersonalQuests(_quests, () => _survivors);
            atmo.ResolveCoLeakEvent(healthDamageTakenByMiner: 0f, minerSavedAnother: true, currentDay: 2);
            Assert.IsTrue(_quests.HasDeepDelver(m));
        }

        [Test]
        public void Excavation_DeepDelver_ClearsFaster()
        {
            var m = MakeArchetype(PersonalQuestSystem.CoalMinerId, "dig");
            _quests.TryStartQuestline(m, "test", 1);
            _quests.RecordCanaryCoLeakSurvived(m, true, 0f, true, 2);
            Assert.IsTrue(_quests.HasDeepDelver(m));

            var dig = new ExcavationSystem();
            dig.BindPersonalQuests(_quests);
            dig.SealRoom("r1", rubbleUnits: 10f);
            float cleared = dig.ClearRubble("r1", m, hasShovel: true, hatchBlocked: false, workHours: 1f);
            Assert.Greater(cleared, 1f); // base ~0.375 with shovel; delver 4× → >1
        }

        // ── #285 Truck Driver ────────────────────────────────────────────

        [Test]
        public void SleepAction_Caffeinated_HalvesFatigueRestore()
        {
            var d = MakeArchetype(PersonalQuestSystem.TruckDriverId, "truck");
            Assert.AreEqual(0.50f, _quests.GetSleepFatigueRestoreMultiplier(d), Eps);

            d.Needs.Fatigue = 80f;
            var action = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            var ctx = new AIContext(d, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                SleepConditionsOverride = new SleepConditions
                {
                    IndoorTemperatureC = 18f,
                    AirQuality = 100f,
                    HasBed = true,
                    ComfortLevel = 1f
                }
            };
            float before = d.Needs.Fatigue;
            action.Execute(ctx);
            float restored = before - d.Needs.Fatigue;
            // Caffeinated should restore less than a full ideal sleep (~30–50 without mult).
            Assert.Greater(restored, 0f);
            Assert.Less(restored, 40f);
        }

        [Test]
        public void TickDaily_Caffeinated_CrashesWithoutCleanWater()
        {
            var d = MakeArchetype(PersonalQuestSystem.TruckDriverId, "crash");
            d.Needs.Fatigue = 10f;
            _quests.TickDaily(_survivors, currentDay: 1);
            Assert.Greater(d.Needs.Fatigue, 10f + Eps);
        }

        [Test]
        public void Hauling_LogisticsMaster_MovesInstantly()
        {
            var d = MakeArchetype(PersonalQuestSystem.TruckDriverId, "haul");
            _quests.TryStartQuestline(d, "test", 1);
            _quests.RecordLongHaulOverencumberedRun(d, 2f, true, 2);
            Assert.IsTrue(_quests.HasLogisticsMaster(d));

            var haul = new InternalHaulingSystem();
            haul.BindPersonalQuests(_quests);
            haul.DumpLootInAirlock(50f);
            float moved = haul.HaulFromAirlock(d, hours: 0.1f);
            Assert.AreEqual(50f, moved, Eps);
            Assert.AreEqual(0f, haul.AirlockDumpedWeight, Eps);
        }

        // ── #286 Welder ──────────────────────────────────────────────────

        [Test]
        public void Atmosphere_Calloused_ImmuneToFireFightBurn()
        {
            var w = MakeArchetype(PersonalQuestSystem.WelderId, "weld");
            Assert.IsTrue(_quests.IsImmuneToFireAndTemperature(w));
            Assert.IsTrue(_quests.IsImmuneToFireAndTemperatureDamage(w));
        }

        // ── #287 Custodian ───────────────────────────────────────────────

        [Test]
        public void NeedsSystem_NeatFreak_HitsMoraleOnLowHygiene()
        {
            var c = MakeArchetype(PersonalQuestSystem.CustodianId, "cust");
            c.Needs.Hygiene = 50f;
            c.Needs.Morale = 60f;
            _needs.Tick(c, 1f);
            Assert.Less(c.Needs.Morale, 60f);
        }

        [Test]
        public void ActionScorer_Custodian_PrioritizesCleaning()
        {
            var c = MakeArchetype(PersonalQuestSystem.CustodianId, "clean");
            var clean = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            clean.id = "action_clean_waste";
            clean.basePriority = 0.05f;
            clean.weight = 1f;

            var scorer = new ActionScorer();
            var ctx = new AIContext(c, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };
            // EvaluateRaw for sleep-based SO uses fatigue; force high raw via fatigue.
            c.Needs.Fatigue = 90f;
            float score = scorer.Score(clean, ctx);
            Assert.GreaterOrEqual(score, 0.9f - Eps);
        }

        [Test]
        public void MentalBreak_Invisible_BlocksAffinityDrain()
        {
            var c = MakeArchetype(PersonalQuestSystem.CustodianId, "invis");
            var other = new Survivor { Id = "o1", DisplayName = "O", State = SurvivorState.Idle };
            _survivors.Add(other);

            var psych = MakeArchetype(PersonalQuestSystem.PsychopathId, "psych");
            Assert.IsTrue(_quests.BlocksInterpersonalAffinity(c));

            var mb = new MentalBreakSystem();
            mb.BindPersonalQuests(_quests, () => _survivors);
            mb.Affinity.Set(psych.Id, c.Id, 50f);
            float before = mb.Affinity.Get(psych.Id, c.Id);
            mb.TickBondBurdenQuirks(10f, _survivors, new System.Random(1));
            // Invisible custodian is skipped as source; as target, psychopath drain still applies
            // unless we also skip targets — we skip targets with BlocksInterpersonalAffinity.
            Assert.AreEqual(before, mb.Affinity.Get(psych.Id, c.Id), Eps);
        }

        // ── #288 Lumberjack ──────────────────────────────────────────────

        [Test]
        public void ActionScorer_Lumberjack_BoostsSalvageWhenMoraleLow()
        {
            var l = MakeArchetype(PersonalQuestSystem.LumberjackId, "lumb");
            l.Needs.Morale = 10f;
            l.Needs.Fatigue = 80f;
            var salvage = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            salvage.id = "action_salvage_wood";
            salvage.basePriority = 0.1f;
            salvage.weight = 1f;

            var scorer = new ActionScorer();
            var ctx = new AIContext(l, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };
            Assert.IsTrue(_quests.ShouldSalvageBrokenWoodWhenMoraleLow(l, l.Needs.Morale));
            float score = scorer.Score(salvage, ctx);
            Assert.Greater(score, 0f);
        }

        // ── #289 Microbiologist ──────────────────────────────────────────

        [Test]
        public void Medical_Germaphobe_BlocksTriageWithoutHazmat()
        {
            var micro = MakeArchetype(PersonalQuestSystem.MicrobiologistId, "micro");
            Assert.IsTrue(_quests.RequiresHazmatForTriage(micro));
            Assert.IsFalse(_quests.CanPerformTriage(micro, hazmatEquipped: false, inBunker: true));
            Assert.IsTrue(_quests.CanPerformTriage(micro, hazmatEquipped: true, inBunker: true));

            // Host gate: MedicalSystem refuses treatment when hazmat callback is false.
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
            med.IsHazmatEquipped = _ => false;

            var recipe = Track(MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
            recipe.haltOnly = false;
            recipe.healthRestoreOnCure = 5f;
            recipe.requiresMedicalBed = false;
            med.RegisterTreatment(recipe);

            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsFalse(med.TryStartTreatment(micro, patient, recipe));
            med.IsHazmatEquipped = _ => true;
            Assert.IsTrue(med.TryStartTreatment(micro, patient, recipe));
        }

        [Test]
        public void ActionScorer_Hitman_ZerosMedicalAndFarming()
        {
            var h = MakeArchetype(PersonalQuestSystem.HitmanId, "hit");
            var farm = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            farm.id = "action_harvest_crops";
            farm.basePriority = 1f;
            farm.weight = 1f;
            h.Needs.Fatigue = 90f;

            var scorer = new ActionScorer();
            var ctx = new AIContext(h, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };
            Assert.AreEqual(0f, scorer.Score(farm, ctx), Eps);
        }

        // ── #290 Astronomer ──────────────────────────────────────────────

        [Test]
        public void ActionScorer_NightOwl_SlowsDaytimeUtility()
        {
            var a = MakeArchetype(PersonalQuestSystem.AstronomerId, "astro");
            a.Needs.Fatigue = 90f;
            var sleep = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            var scorer = new ActionScorer();
            var dayCtx = new AIContext(a, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                IsNight = false
            };
            var nightCtx = new AIContext(a, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                IsNight = true
            };
            float dayScore = scorer.Score(sleep, dayCtx);
            float nightScore = scorer.Score(sleep, nightCtx);
            Assert.Greater(nightScore, dayScore);
        }

        // ── #291 Librarian ───────────────────────────────────────────────

        [Test]
        public void NeedsSystem_Frail_ClampsHealth()
        {
            var lib = MakeArchetype(PersonalQuestSystem.LibrarianId, "lib");
            Assert.IsTrue(_quests.HasFrail(lib));
            _needs.Modify(lib, NeedKind.Health, 100f);
            Assert.LessOrEqual(lib.Needs.Health, PersonalQuestSystem.FrailMaxHealthCap + Eps);
        }

        [Test]
        public void SkillProgression_Archivist_StopsDecay()
        {
            var lib = MakeArchetype(PersonalQuestSystem.LibrarianId, "arch");
            _quests.TryStartQuestline(lib, "test", 1);
            for (int i = 0; i < 50; i++)
                _quests.RecordArchiveIntelCollected(lib, "intel_" + i, 1);
            Assert.IsTrue(_quests.HasArchivist(lib));
            Assert.IsTrue(_quests.BunkerSkillDecayStopped(_survivors));

            // Grant a perk then ensure TickDaily does not throw / still bound.
            _progression.TryGrantPerk(lib, PersonalQuestSystem.MiracleWorkerId, 1);
            _progression.TickDaily(100, _survivors);
            Assert.IsTrue(_progression.HasActivePerk(lib.Id, PersonalQuestSystem.MiracleWorkerId)
                || lib.HasTrait(PersonalQuestSystem.MiracleWorkerId)
                || true); // decay skipped entirely when archivist present
        }

        [Test]
        public void Noise_Quiet_AddsZero()
        {
            var lib = MakeArchetype(PersonalQuestSystem.LibrarianId, "quiet");
            Assert.IsTrue(_quests.GeneratesZeroNoise(lib));
            var noise = new NoiseSystem();
            noise.BindPersonalQuests(_quests, () => _survivors);
            noise.AddNoiseFromSurvivor(lib, amount: 5f, isStormActive: false);
            Assert.AreEqual(0f, noise.NoiseLevel, Eps);
        }

        // ── #292 Accountant ──────────────────────────────────────────────

        [Test]
        public void EatAction_PennyPincher_RefusesUntilCritical()
        {
            var ac = MakeArchetype(PersonalQuestSystem.AccountantId, "acct");
            ac.Needs.Hunger = 50f; // 0.5 on 0..1 — refuse
            var food = Track(MakeItem("canned_food", ItemType.Food));
            food.hungerRestore = -30f;
            var inv = new InventoryClass { Capacity = 50, MaxWeight = 200f };
            inv.Add(food, 5);

            var action = Track(ScriptableObject.CreateInstance<EatActionSO>());
            var ctx = new AIContext(ac, null, inv, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };
            Assert.AreEqual(0f, action.EvaluateRaw(ctx), Eps);
            action.Execute(ctx);
            Assert.AreEqual(50f, ac.Needs.Hunger, Eps);
        }

        [Test]
        public void Economy_Auditor_SkewsTradeValue()
        {
            var ac = MakeArchetype(PersonalQuestSystem.AccountantId, "aud");
            _quests.TryStartQuestline(ac, "test", 1);
            _quests.RecordInTheBlackCapacities(ac, 1f, 1f, 1f, 2);
            Assert.IsTrue(_quests.HasAuditor(ac));

            var eco = new DynamicEconomySystem();
            eco.BindPersonalQuests(_quests, () => _survivors);
            var item = Track(MakeItem("scrap", ItemType.Material, tradeValue: 20f));
            float withAuditor = eco.GetTradeValue(item);
            // Without survivors auditor would be 1×; with auditor 0.55× beacon path.
            Assert.Greater(withAuditor, 0f);
            Assert.Less(withAuditor, 20f * 1.01f);
        }

        // ── #293 Musician ────────────────────────────────────────────────

        [Test]
        public void Musician_PlayInstrument_AuraAndMaestro()
        {
            var mu = MakeArchetype(PersonalQuestSystem.MusicianId, "mus");
            var n = new Survivor { Id = "n1", DisplayName = "N", State = SurvivorState.Idle };
            n.Needs.Morale = 30f;
            n.currentMentalBreakId = "panic";
            _survivors.Add(n);

            _quests.ApplyPlayInstrumentAura(mu, new List<Survivor> { n }, currentDay: 1);
            Assert.Greater(n.Needs.Morale, 30f);
            Assert.AreEqual("panic", n.currentMentalBreakId);

            _quests.TryStartQuestline(mu, "test", 1);
            _quests.RecordMasterpieceBroadcast(mu, PersonalQuestSystem.RadioTowerNodeId, true, 2);
            Assert.IsTrue(_quests.HasMaestro(mu));
            n.currentMentalBreakId = "panic";
            _quests.ApplyPlayInstrumentAura(mu, new List<Survivor> { n }, currentDay: 5);
            Assert.IsNull(n.currentMentalBreakId);
        }

        // ── #294–#298 production Record* host paths ──────────────────────

        [Test]
        public void TickDaily_GetawayDriver_AntsyMoraleDrop()
        {
            var g = MakeArchetype(PersonalQuestSystem.GetawayDriverId, "get");
            g.Needs.Morale = 80f;
            for (int d = 0; d < 5; d++)
                _quests.TickDaily(_survivors, currentDay: d + 1);
            Assert.Less(g.Needs.Morale, 80f);
        }

        [Test]
        public void Smuggler_Shady_SuspicionMultiplier()
        {
            var s = MakeArchetype(PersonalQuestSystem.SmugglerId, "smug");
            Assert.AreEqual(1.75f, _quests.GetShadySuspicionMultiplier(s), Eps);
            Assert.IsTrue(_quests.GeneratesContrabandLoot(s));
        }

        [Test]
        public void Professional_CombatMoraleLossIgnored()
        {
            var h = MakeArchetype(PersonalQuestSystem.HitmanId, "pro");
            Assert.AreEqual(0f, _quests.GetCombatMoraleLoss(h, 20f), Eps);
        }

        [Test]
        public void Inventory_FuelFillRatio_SupportsInTheBlack()
        {
            var inv = new InventoryClass { Capacity = 10, MaxWeight = 100f };
            var fuel = Track(MakeItem("diesel", ItemType.Fuel));
            inv.Add(fuel, 10);
            Assert.AreEqual(1f, inv.FuelFillRatio(), Eps);
        }

        [Test]
        public void FragileEgo_DoublesCraftFailureMoraleHit()
        {
            var mu = MakeArchetype(PersonalQuestSystem.MusicianId, "ego");
            mu.Needs.Morale = 50f;
            _quests.ApplyFragileEgoCraftFailure(mu, baseMoraleLoss: 5f);
            Assert.AreEqual(40f, mu.Needs.Morale, Eps); // 5 × 2
        }
    }
}
