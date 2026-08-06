using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using AtomicWar._Game.Economy;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Host-integration tests for civil-war / interpersonal wiring (#257–#266).
    /// </summary>
    [TestFixture]
    public class CivilWarHostTests : PersonalQuestHostTestBase
    {
        // ── #257 HatchDefense Art of War ─────────────────────────────────

        [Test]
        public void HatchDefense_ArtOfWar_BoostsShelterSecurityByTwentyFivePercent()
        {
            var gen = MakeArchetype(PersonalQuestSystem.GeneralId, "gen");
            _quests.TryStartQuestline(gen, "test", 1);
            _quests.RecordHitSquadWiped(gen, targetedAtGeneral: true, squadWiped: true, currentDay: 2);
            Assert.IsTrue(_quests.HasArtOfWar(gen));

            var hatch = new HatchDefenseSystem(
                getShelter: () => null,
                getSurvivors: () => _survivors,
                getDay: () => 1,
                rng: new System.Random(1));
            hatch.BindPersonalQuests(_quests);
            hatch.SecurityOverride = 100f;

            Assert.AreEqual(125f, hatch.GetShelterSecurity(), Eps);
        }

        // ── #257 Economy Hated military trust ────────────────────────────

        [Test]
        public void Economy_HatedGeneral_MilitaryTrustFloorsAtMinus100()
        {
            var gen = MakeArchetype(PersonalQuestSystem.GeneralId, "hated");
            Assert.IsTrue(_quests.HasHated(gen));
            Assert.IsTrue(_quests.IsShotOnSightByMilitary(gen));

            var economy = new DynamicEconomySystem();
            economy.BindPersonalQuests(_quests, () => _survivors);
            economy.SetTrust(FactionSO.Ids.MilitaryRemnants, 40f);

            Assert.AreEqual(
                PersonalQuestSystem.HatedMilitaryTrust,
                economy.GetTrust(FactionSO.Ids.MilitaryRemnants),
                Eps);
            Assert.AreEqual(
                PersonalQuestSystem.HatedMilitaryTrust,
                economy.GetEffectiveTrust(FactionSO.Ids.MilitaryRemnants),
                Eps);
        }

        // ── #257 Sleep bed-only ──────────────────────────────────────────

        [Test]
        public void SleepAction_Tactician_RefusesFloorSleep_AndPenalizesFatigue()
        {
            var gen = MakeArchetype(PersonalQuestSystem.GeneralId, "sleep_gen");
            Assert.IsTrue(_quests.RequiresBedModuleToSleep(gen));
            gen.Needs.Fatigue = 40f;

            var action = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            var ctx = new AIContext(gen, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors,
                SleepConditionsOverride = new SleepConditions
                {
                    IndoorTemperatureC = 18f,
                    AirQuality = 100f,
                    HasBed = false,
                    ComfortLevel = 0f
                }
            };

            Assert.AreEqual(0f, action.EvaluateRaw(ctx), Eps, "Floor sleep utility must be zero.");

            action.Execute(ctx);
            Assert.AreEqual(
                40f + PersonalQuestSystem.FloorSleepFatiguePenaltyPerHour,
                gen.Needs.Fatigue,
                Eps);
        }

        [Test]
        public void SleepAction_Tactician_AcceptsBed()
        {
            var gen = MakeArchetype(PersonalQuestSystem.GeneralId, "sleep_bed");
            gen.Needs.Fatigue = 60f;

            var action = Track(ScriptableObject.CreateInstance<SleepActionSO>());
            var ctx = new AIContext(gen, null, null, new System.Random(1))
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

            Assert.Greater(action.EvaluateRaw(ctx), 0f);
            action.Execute(ctx);
            Assert.Less(gen.Needs.Fatigue, 60f);
        }

        // ── #259 / #266 ActionScorer labor filters ───────────────────────

        [Test]
        public void ActionScorer_Coward_RefusesLoudLabor()
        {
            var des = MakeArchetype(PersonalQuestSystem.DeserterId, "coward");
            Assert.IsTrue(_quests.RefusesLoudLabor(des));

            var loud = Track(ScriptableObject.CreateInstance<BuildWindTurbineActionSO>());
            if (string.IsNullOrEmpty(loud.id)) loud.id = "action_build_wind_turbine";

            var scorer = new ActionScorer();
            var ctx = new AIContext(des, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            Assert.AreEqual(0f, scorer.Score(loud, ctx), Eps);
        }

        [Test]
        public void ActionScorer_GodComplex_RefusesMenialLabor()
        {
            var surg = MakeArchetype(PersonalQuestSystem.ArrogantSurgeonId, "god");
            Assert.IsTrue(_quests.HasGodComplex(surg));
            Assert.IsTrue(_quests.RefusesMenialLabor(surg));

            var dig = Track(ScriptableObject.CreateInstance<ExcavateEscapeHatchActionSO>());
            if (string.IsNullOrEmpty(dig.id)) dig.id = "action_excavate_escape_hatch";

            var scorer = new ActionScorer();
            var ctx = new AIContext(surg, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            Assert.AreEqual(0f, scorer.Score(dig, ctx), Eps);
        }

        [Test]
        public void ActionScorer_HyperEmpath_BiasesComfortTalk()
        {
            var emp = MakeArchetype(PersonalQuestSystem.EmpathId, "empath");
            Assert.IsTrue(_quests.PrioritizesComfortOverSurvival(emp));

            var comfort = Track(ScriptableObject.CreateInstance<MentalBreakComfortActionSO>());
            var talk = Track(ScriptableObject.CreateInstance<TalkDownActionSO>());
            if (string.IsNullOrEmpty(talk.id)) talk.id = "action_talk_down";

            var scorer = new ActionScorer();
            var ctx = new AIContext(emp, null, null, new System.Random(1))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            Assert.IsTrue(ActionScorer.IsComfortOrTalkAction(talk.id));
            Assert.AreEqual(
                PersonalQuestSystem.ComfortTalkUtilityBias,
                _quests.GetComfortTalkUtilityBias(emp),
                Eps);
            Assert.GreaterOrEqual(scorer.Score(talk, ctx), 0f);
            Assert.NotNull(comfort);
        }

        // ── #260 Crafting Supply Chain cost mult ─────────────────────────

        [Test]
        public void Crafting_SupplyChainMaster_ConsumesTwentyPercentFewerMaterials()
        {
            var qm = MakeArchetype(PersonalQuestSystem.QuartermasterId, "qm");
            _quests.TryStartQuestline(qm, "test", 1);
            _quests.RecordScrapStockpile(qm, 100, 100, 100, currentDay: 2);
            Assert.IsTrue(_quests.HasSupplyChainMaster(qm));
            Assert.AreEqual(0.8f, _quests.GetCraftMaterialCostMultiplier(qm), Eps);

            var cloth = Track(MakeItem("cloth", ItemType.Material));
            var bandage = Track(MakeItem("bandage", ItemType.Medical));
            var recipe = Track(ScriptableObject.CreateInstance<Recipe>());
            recipe.id = "craft_bandage";
            recipe.result = bandage;
            recipe.resultAmount = 1;
            recipe.craftingTimeHours = 0.5f;
            recipe.requiredStationId = "";
            recipe.ingredients.Add(new Ingredient { item = cloth, amount = 5 });

            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            inv.Add(cloth, 5);
            var craft = new CraftingSystem(inv);
            craft.BindPersonalQuests(_quests, new System.Random(1));

            Assert.IsTrue(craft.StartCraft(recipe, qm));
            Assert.AreEqual(1, inv.Count(cloth));
        }

        // ── #260 PowerNetwork fuel mult ──────────────────────────────────

        [Test]
        public void PowerNetwork_SupplyChainMaster_BurnsLessDiesel()
        {
            var qm = MakeArchetype(PersonalQuestSystem.QuartermasterId, "fuel_qm");
            _quests.TryStartQuestline(qm, "test", 1);
            _quests.RecordScrapStockpile(qm, 100, 100, 100, currentDay: 2);
            Assert.IsTrue(_quests.HasSupplyChainMaster(qm));
            Assert.AreEqual(0.7f, _quests.GetBunkerFuelBurnMultiplier(_survivors), Eps);

            var baseline = PowerNetwork.CreateDefault(dieselFuel: 40f);
            float fuelBeforeBase = baseline.GetDieselFuelTotal();
            baseline.Tick(1f, weatherName: "clear");
            float burnedBase = fuelBeforeBase - baseline.GetDieselFuelTotal();

            var efficient = PowerNetwork.CreateDefault(dieselFuel: 40f);
            efficient.BindPersonalQuests(_quests, () => _survivors);
            float fuelBeforeEff = efficient.GetDieselFuelTotal();
            efficient.Tick(1f, weatherName: "clear");
            float burnedEff = fuelBeforeEff - efficient.GetDieselFuelTotal();

            Assert.Greater(burnedBase, 0f);
            Assert.AreEqual(burnedBase * 0.7f, burnedEff, 0.05f);
        }

        // ── #260 Inventory auto-resort ───────────────────────────────────

        [Test]
        public void Inventory_ResortSlotsByType_OrdersByTypeThenId()
        {
            var food = Track(MakeItem("canned_food", ItemType.Food));
            var scrap = Track(MakeItem("scrap_metal", ItemType.Material));
            var med = Track(MakeItem("bandage", ItemType.Medical));

            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            inv.Add(food, 1);
            inv.Add(scrap, 1);
            inv.Add(med, 1);
            Assert.IsFalse(inv.IsSortedByType());

            inv.ResortSlotsByType();
            Assert.IsTrue(inv.IsSortedByType());
            Assert.AreEqual(3, inv.Slots.Count);
        }

        // ── #265 NeedsSystem Living Saint floor ──────────────────────────

        [Test]
        public void NeedsSystem_LivingSaint_FloorsMoraleAtFifty()
        {
            var mar = MakeArchetype(PersonalQuestSystem.MartyrId, "martyr");
            _quests.TryStartQuestline(mar, "test", 1);
            _quests.RecordTookLethalPhase2ForOther(mar, viaEventChoice: true, isPhase2Lethal: true, currentDay: 2);
            Assert.IsTrue(_quests.HasLivingSaint(mar));
            mar.Needs.Health = 0f;
            mar.State = SurvivorState.Dead;
            _quests.NotifySurvivorDied(mar);
            Assert.IsTrue(_quests.LivingSaintInspiredActive);

            var ally = new Survivor { Id = "ally_ls", DisplayName = "Ally", State = SurvivorState.Idle };
            ally.Needs.Morale = 30f;
            _survivors.Add(ally);
            _needs.Register(ally);

            _needs.Modify(ally, NeedKind.Morale, -5f);
            Assert.AreEqual(PersonalQuestSystem.LivingSaintMoraleFloor, ally.Needs.Morale, Eps);

            ally.Needs.Morale = 20f;
            _needs.Tick(1f);
            Assert.AreEqual(PersonalQuestSystem.LivingSaintMoraleFloor, ally.Needs.Morale, Eps);
        }

        // ── #262 NeedsSystem Hyper-Empath drift ──────────────────────────

        [Test]
        public void NeedsSystem_HyperEmpath_DriftsTowardBunkerAverage()
        {
            var emp = MakeArchetype(PersonalQuestSystem.EmpathId, "sponge");
            Assert.IsTrue(_quests.HasHyperEmpathetic(emp));
            emp.Needs.Morale = 20f;

            var happy = new Survivor { Id = "happy", DisplayName = "Happy", State = SurvivorState.Idle };
            happy.Needs.Morale = 80f;
            _survivors.Add(happy);
            _needs.Register(happy);

            float before = emp.Needs.Morale;
            _needs.Tick(1f);
            Assert.Greater(emp.Needs.Morale, before, "Empath morale should rise toward bunker avg.");
        }

        // ── #266 Medical God Complex + Humbled ───────────────────────────

        [Test]
        public void Medical_GodComplex_HitsPatientMoraleAfterHeal()
        {
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
            med.BindMedicalPerks(null, id =>
            {
                for (int i = 0; i < _survivors.Count; i++)
                    if (_survivors[i] != null && _survivors[i].Id == id) return _survivors[i];
                return null;
            });

            var recipe = Track(MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
            recipe.haltOnly = false;
            recipe.healthRestoreOnCure = 5f;
            recipe.requiresMedicalBed = false;
            med.RegisterTreatment(recipe);

            var surg = MakeArchetype(PersonalQuestSystem.ArrogantSurgeonId, "surgeon");
            Assert.IsTrue(_quests.HasGodComplex(surg));
            surg.MedicalSkill = 1f;

            var patient = new Survivor
            {
                Id = "patient",
                DisplayName = "Patient",
                State = SurvivorState.Idle
            };
            patient.Needs.Morale = 70f;
            patient.Needs.Health = 100f;
            _survivors.Add(patient);
            _needs.Register(patient);

            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsTrue(med.TryStartTreatment(surg, patient, recipe));
            Assert.IsTrue(med.GetActive(patient)[0].IsTreating);

            med.Tick(_survivors, gameHours: 2f);

            Assert.IsTrue(patient.IsAlive, "Patient must survive treatment for God Complex abuse.");
            Assert.AreEqual(
                70f - PersonalQuestSystem.GodComplexPatientMoraleHit,
                patient.Needs.Morale,
                Eps);
        }

        [Test]
        public void Medical_HumbledHealer_CanCureChronicDisability()
        {
            var surg = MakeArchetype(PersonalQuestSystem.ArrogantSurgeonId, "humbled");
            _quests.TryStartQuestline(surg, "test", 1);
            _quests.RecordCriticalSurgeryFailed(surg, wasCritical: true, currentDay: 2);
            for (int d = 0; d < PersonalQuestSystem.BotchedJobDepressionDays; d++)
                _quests.RecordDepressionDay(surg, currentDay: 3 + d);
            Assert.IsTrue(_quests.HasHumbledHealer(surg));
            Assert.IsTrue(_quests.CanCureChronicDisabilities(surg));

            var patient = new Survivor
            {
                Id = "chronic",
                DisplayName = "Chronic",
                State = SurvivorState.Idle
            };
            patient.DisabilityIds.Add(DisabilitySO.Ids.Limp);
            _survivors.Add(patient);

            var inv = new InventoryClass { Capacity = 10, MaxWeight = 50f };
            var med = new MedicalSystem(_needs, inv);
            med.BindPersonalQuests(_quests);

            Assert.IsTrue(med.TryCureChronicDisability(surg, patient, DisabilitySO.Ids.Limp));
            Assert.IsFalse(patient.HasDisability(DisabilitySO.Ids.Limp));
        }

        // ── #264 NeedsBar Denialist anxiety ──────────────────────────────

        [Test]
        public void NeedsBar_Denialist_DisplaysZeroRadiationAnxiety()
        {
            var pol = MakeArchetype(PersonalQuestSystem.ThePollyannaId, "polly");
            Assert.IsTrue(_quests.HasDenialist(pol));
            pol.RadiationAnxiety = 0.9f;

            var go = new GameObject("needs_bar_denialist");
            var bar = go.AddComponent<NeedsBar>();
            bar.SetNeeds(
                pol.Needs,
                pol.Needs.Health,
                radiation: 10f,
                pol,
                _quests,
                new System.Random(1));

            Assert.AreEqual(0f, bar.DisplayedRadiationAnxiety, Eps);
            Assert.AreEqual(0f, _quests.GetDisplayedRadiationAnxiety(pol, 0.9f), Eps);

            Object.DestroyImmediate(go);
        }

        // ── #259 Coward flee threshold ───────────────────────────────────

        [Test]
        public void Coward_AutoFlee_WhenBelowHalfHealth()
        {
            var des = MakeArchetype(PersonalQuestSystem.DeserterId, "flee");
            Assert.IsTrue(_quests.HasCoward(des));
            des.Needs.Health = 40f;
            Assert.IsTrue(_quests.ShouldAutoFleeCombat(des));
            des.Needs.Health = 80f;
            Assert.IsFalse(_quests.ShouldAutoFleeCombat(des));
        }

        // ── #265 Sacrificial intercept API ───────────────────────────────

        [Test]
        public void Sacrificial_InterceptsHatchBreachDamage()
        {
            var mar = MakeArchetype(PersonalQuestSystem.MartyrId, "sac");
            Assert.IsTrue(_quests.HasSacrificial(mar));
            mar.Needs.Health = 80f;

            var target = new Survivor { Id = "tgt", DisplayName = "T", State = SurvivorState.Idle };
            target.Needs.Health = 60f;
            _survivors.Add(target);

            float remaining = _quests.InterceptHatchBreachDamage(mar, target, 15f);
            Assert.AreEqual(0f, remaining, Eps);
            Assert.AreEqual(65f, mar.Needs.Health, Eps);
            Assert.AreEqual(60f, target.Needs.Health, Eps);
        }
    }
}
