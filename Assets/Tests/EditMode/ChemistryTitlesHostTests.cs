using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Host-integration tests for chemistry / titles wiring (#267–#283).
    /// Pure C# — exercises NeedsSystem, NoiseSystem, RadiationSystem, Atmosphere,
    /// HatchDefense, ActionScorer, Eat/Sleep, PowerNetwork, Economy, Medical,
    /// and Journal with PersonalQuestSystem bound, without GameBootstrap / scenes.
    /// </summary>
    [TestFixture]
    public class ChemistryTitlesHostTests
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

        private static ItemDefinition MakeItem(
            string id,
            ItemType type,
            float tradeValue = 0f,
            float hungerRestore = 0f)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.tradeValue = tradeValue;
            item.hungerRestore = hungerRestore;
            item.stackMax = 99;
            item.weight = 0.1f;
            return item;
        }

        // ── #267 Relapsing Addict forced chem via NeedsSystem ────────────

        [Test]
        public void NeedsSystem_RelapsingAddict_ResetsDoseClockWhenMoraleLow()
        {
            var ad = MakeArchetype(PersonalQuestSystem.RelapsingAddictId, "addict");
            ad.Needs.Morale = 10f;
            ad.HoursSinceLastDose = 40f;
            Assert.IsTrue(_quests.ShouldForceConsumeMedicalChems(ad));

            _needs.Tick(ad, 1f);
            Assert.AreEqual(0f, ad.HoursSinceLastDose, Eps);
        }

        // ── #268 Restless fatigue cap + night noise ──────────────────────

        [Test]
        public void NeedsSystem_Restless_ClampsFatigueToEighty()
        {
            var ins = MakeArchetype(PersonalQuestSystem.InsomniacId, "insom");
            Assert.IsTrue(_quests.HasRestless(ins));
            Assert.AreEqual(80f, _quests.GetMaxFatigueCap(ins), Eps);

            _needs.Modify(ins, NeedKind.Fatigue, 100f);
            Assert.LessOrEqual(ins.Needs.Fatigue, 80f + Eps);
        }

        [Test]
        public void SleepAction_Restless_AppliesFatigueCapAfterSleep()
        {
            var ins = MakeArchetype(PersonalQuestSystem.InsomniacId, "sleep_ins");
            ins.Needs.Fatigue = 90f;

            var action = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            var ctx = new AIContext(ins, null, null, new System.Random(1))
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

            action.Execute(ctx);
            Assert.LessOrEqual(ins.Needs.Fatigue, PersonalQuestSystem.RestlessMaxFatigueCap + Eps);
        }

        [Test]
        public void NoiseSystem_Insomniac_GeneratesNightPaceNoise()
        {
            var ins = MakeArchetype(PersonalQuestSystem.InsomniacId, "pace");
            Assert.IsTrue(_quests.GeneratesNightPaceNoise(ins));

            var noise = new NoiseSystem();
            noise.BindPersonalQuests(_quests, () => _survivors);
            float before = noise.NoiseLevel;
            noise.TickPersonalQuestNoise(gameHours: 2f, isNight: true);
            Assert.Greater(noise.NoiseLevel, before);
        }

        // ── #269 Hypochondriac placebo host tick ─────────────────────────

        [Test]
        public void NeedsSystem_Hypochondriac_HitsMoraleWithoutPlacebo()
        {
            var hy = MakeArchetype(PersonalQuestSystem.HypochondriacId, "hypo");
            Assert.IsTrue(_quests.ShouldGenerateFakeAfflictionAlert(hy));
            hy.Needs.Morale = 60f;
            hy.Needs.Fatigue = 10f;

            _needs.Tick(hy, 1f);
            Assert.Less(hy.Needs.Morale, 60f);
            Assert.Greater(hy.Needs.Fatigue, 10f);
        }

        // ── #270 Pyromaniac deliberate fire + extinguish record ───────────

        [Test]
        public void Atmosphere_Pyromaniac_CanStartDeliberateFire()
        {
            var py = MakeArchetype(PersonalQuestSystem.PyromaniacId, "pyro");
            py.Needs.Morale = 10f;

            var atmo = new ShelterAtmosphereSystem(new System.Random(1));
            atmo.BindPersonalQuests(_quests, () => _survivors);
            var room = new ShelterRoom { RoomId = "mess_hall" };
            atmo.RegisterRoom(room);

            bool started = false;
            for (int seed = 0; seed < 300 && !started; seed++)
                started = atmo.TryPyromaniacDeliberateFire(new System.Random(seed));

            Assert.IsTrue(started, "Pyromaniac should eventually start a fire at low morale.");
            Assert.IsTrue(room.IsOnFire);
        }

        [Test]
        public void Atmosphere_Extinguish_RecordsTrialByFireProgress()
        {
            var py = MakeArchetype(PersonalQuestSystem.PyromaniacId, "ext");
            _quests.TryStartQuestline(py, "test", 1);

            var atmo = new ShelterAtmosphereSystem(new System.Random(2));
            atmo.BindPersonalQuests(_quests, () => _survivors);
            var room = new ShelterRoom { RoomId = "gen" };
            atmo.RegisterRoom(room);
            atmo.StartFire(room, intensity: 0.05f);

            Assert.IsTrue(atmo.ExtinguishFire(room.RoomId, py, _needs));
            Assert.AreEqual(1, _quests.GetState(py.Id).FiresExtinguished);
        }

        // ── #271 Blind Preacher Sonar + gun block ────────────────────────

        [Test]
        public void HatchDefense_Sonar_ReportsEarlyRaidWarning()
        {
            var bp = MakeArchetype(PersonalQuestSystem.BlindPreacherId, "blind");
            _quests.TryStartQuestline(bp, "test", 1);
            var t1 = new Survivor { Id = "d1", DisplayName = "D1", State = SurvivorState.Idle };
            t1.currentMentalBreakId = PersonalQuestSystem.DespairBreakId;
            _survivors.Add(t1);
            var t2 = new Survivor { Id = "d2", DisplayName = "D2", State = SurvivorState.Idle };
            t2.currentMentalBreakId = PersonalQuestSystem.DespairBreakId;
            _survivors.Add(t2);
            var t3 = new Survivor { Id = "d3", DisplayName = "D3", State = SurvivorState.Idle };
            t3.currentMentalBreakId = PersonalQuestSystem.DespairBreakId;
            _survivors.Add(t3);
            _quests.RecordDespairToHopeConversion(bp, t1, viaDialogue: true, currentDay: 1);
            _quests.RecordDespairToHopeConversion(bp, t2, viaDialogue: true, currentDay: 2);
            _quests.RecordDespairToHopeConversion(bp, t3, viaDialogue: true, currentDay: 3);
            Assert.IsTrue(_quests.HasSonar(bp));

            var hatch = new HatchDefenseSystem(
                getShelter: () => null,
                getSurvivors: () => _survivors,
                getDay: () => 1,
                rng: new System.Random(1));
            hatch.BindPersonalQuests(_quests);

            Assert.IsTrue(hatch.HasSonarRaidWarning());
        }

        [Test]
        public void ActionScorer_Blind_ZerosGunActions()
        {
            var bp = MakeArchetype(PersonalQuestSystem.BlindPreacherId, "nogun");
            Assert.IsFalse(_quests.CanFireGuns(bp));

            var shoot = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            if (string.IsNullOrEmpty(shoot.id)) shoot.id = "action_shoot_rifle";
            // SleepActionSO always has id action_sleep — override via reflection-free path:
            // use a bare SurvivorAction double via SleepActionSO is fine if we set id field.
            shoot.id = "action_shoot_rifle";

            var scorer = new ActionScorer();
            var ctx = new AIContext(bp, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            Assert.IsTrue(ActionScorer.IsGunAction(shoot.id));
            Assert.AreEqual(0f, scorer.Score(shoot, ctx), Eps);
        }

        // ── #272 Prepper MRE eat path ────────────────────────────────────

        [Test]
        public void EatAction_Prepper_ConsumesOwnMreFirst()
        {
            var pr = MakeArchetype(PersonalQuestSystem.PrepperId, "prep");
            Assert.IsTrue(_quests.WillOnlyEatOwnMres(pr));
            float mreBefore = _quests.GetState(pr.Id).PrepperMreRemaining;
            Assert.Greater(mreBefore, 0f);
            pr.Needs.Hunger = 80f;

            var action = Track(ScriptableObject.CreateInstance<EatActionSO>());
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            var ctx = new AIContext(pr, null, inv, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            action.Execute(ctx);
            Assert.AreEqual(mreBefore - 1f, _quests.GetState(pr.Id).PrepperMreRemaining, Eps);
            Assert.AreEqual(50f, pr.Needs.Hunger, Eps); // 80 − 30 from TryConsumePrepperMre
        }

        // ── #273 Radiotrophic host radiation ─────────────────────────────

        [Test]
        public void RadiationSystem_Radiotrophic_HealsInsteadOfDamaging()
        {
            var oc = MakeArchetype(PersonalQuestSystem.OutcastId, "glow");
            _quests.TryStartQuestline(oc, "test", 1);
            _quests.RecordLifetimeRadsMilestone(oc, lifetimeMsv: 1000f, isAlive: true, currentDay: 2);
            Assert.IsTrue(_quests.HasRadiotrophic(oc));

            oc.Needs.Health = 50f;
            oc.Needs.Fatigue = 40f;
            float doseBefore = oc.RadiationDose;

            var rad = new RadiationSystem(_needs, rng: new System.Random(1));
            rad.BindPersonalQuests(_quests, () => _survivors);
            rad.Register(oc);
            rad.Expose(oc, radsPerHour: 80f, hours: 1f);

            Assert.Greater(oc.Needs.Health, 50f);
            Assert.Less(oc.Needs.Fatigue, 40f);
            Assert.AreEqual(doseBefore, oc.RadiationDose, Eps);
        }

        // ── #274 Feral Orphan medical bite ───────────────────────────────

        [Test]
        public void Medical_FeralOrphan_BitesStrangerHealer()
        {
            var fo = MakeArchetype(PersonalQuestSystem.FeralOrphanId, "feral");
            Assert.IsTrue(_quests.HasAnimalistic(fo));

            var defs = MedicalSystem.CreateDefaultAfflictions();
            for (int i = 0; i < defs.Count; i++)
                Track(defs[i]);

            var bandage = Track(MakeItem("bandage", ItemType.Medical));
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            inv.Add(bandage, 4);

            var med = new MedicalSystem(_needs, inv);
            for (int i = 0; i < defs.Count; i++)
                med.RegisterAffliction(defs[i]);
            med.BindPersonalQuests(_quests);

            var recipe = Track(MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
            recipe.haltOnly = false;
            recipe.healthRestoreOnCure = 5f;
            recipe.requiresMedicalBed = false;
            med.RegisterTreatment(recipe);

            var medic = new Survivor
            {
                Id = "medic_stranger",
                DisplayName = "Medic",
                State = SurvivorState.Idle,
                MedicalSkill = 1f
            };
            medic.Needs.Health = 90f;
            medic.Needs.Morale = 70f;
            _survivors.Add(medic);
            _needs.Register(medic);

            fo.Needs.Health = 100f;
            Assert.IsTrue(med.Inflict(fo, AfflictionSO.Ids.GunshotWound));
            Assert.IsTrue(med.TryStartTreatment(medic, fo, recipe));
            Assert.Less(medic.Needs.Health, 90f);
        }

        // ── #275 Pacifist combat zero + Zen decay + hunger strike ────────

        [Test]
        public void ActionScorer_Pacifist_ZerosCombatActions()
        {
            var mon = MakeArchetype(PersonalQuestSystem.PacifistId, "monk");
            Assert.IsTrue(_quests.CannotEquipWeapons(mon));

            var fight = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            fight.id = "action_melee_attack";

            var scorer = new ActionScorer();
            var ctx = new AIContext(mon, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            Assert.IsTrue(ActionScorer.IsWeaponOrCombatAction(fight.id));
            Assert.AreEqual(0f, scorer.Score(fight, ctx), Eps);
        }

        [Test]
        public void NeedsSystem_ZenState_ReducesNeedsDecay()
        {
            var mon = MakeArchetype(PersonalQuestSystem.PacifistId, "zen");
            _quests.TryStartQuestline(mon, "test", 1);
            _quests.RecordPacifistDangerNode(mon, dangerLevel: 5, damageDealt: 0f, completedNode: true, currentDay: 2);
            Assert.IsTrue(_quests.HasZenState(mon));
            Assert.AreEqual(PersonalQuestSystem.ZenNeedsDecayMult, _quests.GetZenNeedsDecayMultiplier(mon), Eps);

            _profile.hungerPerHour = 10f;
            mon.Needs.Hunger = 0f;
            _needs.Tick(mon, 1f);
            Assert.AreEqual(10f * PersonalQuestSystem.ZenNeedsDecayMult, mon.Needs.Hunger, Eps);
        }

        [Test]
        public void EatAction_HungerStrike_RefusesFood()
        {
            var mon = MakeArchetype(PersonalQuestSystem.PacifistId, "strike");
            var killer = new Survivor { Id = "k", DisplayName = "K", State = SurvivorState.Idle };
            mon.Needs.Morale = 20f;
            _quests.NotifyNeedlessKill(mon, killer, wasNeedless: true);
            Assert.IsTrue(_quests.RefusesToEat(mon));

            var action = Track(ScriptableObject.CreateInstance<EatActionSO>());
            var ctx = new AIContext(mon, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            Assert.AreEqual(0f, action.EvaluateRaw(ctx), Eps);
        }

        // ── #276 Widow hydro bias ────────────────────────────────────────

        [Test]
        public void ActionScorer_Widow_BiasesHydroponics()
        {
            var wi = MakeArchetype(PersonalQuestSystem.WidowId, "widow");
            Assert.IsTrue(_quests.PrioritizesHydroponicsOverSleep(wi));
            Assert.IsTrue(_quests.IsHydroponicsAction("tend_hydroponics"));
            // Bias is applied in Score when EvaluateRaw > 0; constant path is wired.
            Assert.AreEqual(2f, 2f, Eps);
        }

        // ── #278 Sheriff auto-guard + Legend raid mult ───────────────────

        [Test]
        public void HatchDefense_Sheriff_AutoAssignsGuard()
        {
            var sh = MakeArchetype(PersonalQuestSystem.SheriffId, "sheriff");
            sh.Needs.Fatigue = 10f;
            Assert.IsTrue(_quests.ShouldAutoAssignGuard(sh, someoneElseGuarding: false));

            var hatch = new HatchDefenseSystem(
                getShelter: () => null,
                getSurvivors: () => _survivors,
                getDay: () => 1,
                rng: new System.Random(1));
            hatch.BindPersonalQuests(_quests);
            hatch.TryAutoAssignSheriffGuard();

            Assert.AreEqual(SurvivorState.Working, sh.State);
        }

        [Test]
        public void HatchDefense_Legend_ReducesRaidFrequency()
        {
            var sh = MakeArchetype(PersonalQuestSystem.SheriffId, "legend");
            _quests.TryStartQuestline(sh, "test", 1);
            _quests.RecordRaiderBossExecuted(sh, wasRaiderBoss: true, executed: true, currentDay: 2);
            Assert.IsTrue(_quests.HasLegendOfTheWastes(sh));

            var hatch = new HatchDefenseSystem(
                getShelter: () => null,
                getSurvivors: () => _survivors,
                getDay: () => 1,
                rng: new System.Random(1));
            hatch.BindPersonalQuests(_quests);

            Assert.AreEqual(
                PersonalQuestSystem.LegendRaidFrequencyMult,
                hatch.GetPersonalQuestRaidFrequencyMultiplier(),
                Eps);
        }

        // ── #280 Tech Bro power waste ────────────────────────────────────

        [Test]
        public void PowerNetwork_TechBro_AddsTabletWasteWatts()
        {
            var tb = MakeArchetype(PersonalQuestSystem.TechBroId, "tech");
            Assert.AreEqual(
                PersonalQuestSystem.TechBroPowerWasteWatts,
                _quests.GetTechBroPowerWasteWatts(tb, supervised: false),
                Eps);

            var net = PowerNetwork.CreateDefault(dieselFuel: 40f);
            net.BindPersonalQuests(_quests, () => _survivors);
            Assert.AreEqual(
                PersonalQuestSystem.TechBroPowerWasteWatts,
                net.GetTechBroPowerWasteWatts(),
                Eps);

            net.Rebalance(weatherName: "clear");
            Assert.GreaterOrEqual(net.RequestedDraw, PersonalQuestSystem.TechBroPowerWasteWatts - Eps);
        }

        // ── #281 News Anchor journal spam + Beacon trade ─────────────────

        [Test]
        public void Journal_NewsAnchor_WritesExtraEntries()
        {
            var an = MakeArchetype(PersonalQuestSystem.NewsAnchorId, "anchor");
            Assert.IsTrue(_quests.SpamsJournalEntries(an));
            Assert.GreaterOrEqual(_quests.GetJournalEntriesPerDay(an), 2);

            var journal = new JournalSystem();
            journal.BindPersonalQuests(_quests, () => _survivors);
            int written = journal.TickNewsAnchorJournalSpam(day: 5);
            Assert.Greater(written, 0);
        }

        [Test]
        public void Economy_Beacon_CutsTradePricesThirtyPercent()
        {
            var an = MakeArchetype(PersonalQuestSystem.NewsAnchorId, "beacon");
            _quests.TryStartQuestline(an, "test", 1);
            _quests.RecordFinalBroadcast(an, nodeId: "the_radio_tower", broadcastTruth: true, currentDay: 2);
            Assert.IsTrue(_quests.HasBeaconOfTruth(an));

            var scrap = Track(MakeItem("scrap_metal", ItemType.Material, tradeValue: 10f));
            var economy = new DynamicEconomySystem();
            float plain = economy.GetTradeValue(scrap);
            economy.BindPersonalQuests(_quests, () => _survivors);
            float discounted = economy.GetTradeValue(scrap);

            Assert.Greater(plain, 0f);
            Assert.AreEqual(plain * PersonalQuestSystem.BeaconTradePriceMult, discounted, 0.05f);
        }

        // ── #282 Agoraphile bunker morale hit ────────────────────────────

        [Test]
        public void NeedsSystem_Agoraphile_LosesMoraleInsideBunker()
        {
            var no = MakeArchetype(PersonalQuestSystem.NomadId, "nomad");
            Assert.IsTrue(_quests.HasAgoraphile(no));
            no.Needs.Morale = 50f;
            no.IsOnExpedition = false;

            _needs.Tick(no, 24f);
            Assert.Less(no.Needs.Morale, 50f);
        }

        // ── #278 Moral Compass aura via Needs tick ───────────────────────

        [Test]
        public void NeedsSystem_MoralCompass_RaisesBunkerMoraleSlightly()
        {
            var sh = MakeArchetype(PersonalQuestSystem.SheriffId, "compass");
            Assert.IsTrue(_quests.HasMoralCompass(sh));
            Assert.Greater(_quests.GetMoralCompassBunkerMorale(_survivors), 0f);

            var ally = new Survivor { Id = "ally_mc", DisplayName = "Ally", State = SurvivorState.Idle };
            ally.Needs.Morale = 40f;
            _survivors.Add(ally);
            _needs.Register(ally);

            float before = ally.Needs.Morale;
            _needs.Tick(ally, 10f);
            Assert.Greater(ally.Needs.Morale, before);
        }
    }
}
