using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #189–#194 — survival / wasteland-digestion milestone perks.
    /// </summary>
    [TestFixture]
    public class SurvivalPerkTests
    {
        private SkillProgressionSystem _progression;
        private SurvivalPerkSystem _perks;
        private Survivor _sv;
        private InventoryClass _inv;
        private WaterStorage _water;
        private NeedsSystem _needs;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _perks = new SurvivalPerkSystem();
            _perks.Bind(_progression);

            _sv = MakeSurvivor("sv_cook", "Cook");
            _inv = new InventoryClass { Capacity = 40 };
            _water = new WaterStorage();
            _water.AddClean(50f);
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            profile.hungerPerHour = 0f;
            profile.thirstPerHour = 0f;
            profile.fatiguePerHour = 0f;
            profile.warmthLossPerHourInCold = 0f;
            _needs = new NeedsSystem(profile);
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle
            };
            sv.Needs.Morale = 50f;
            sv.Needs.Health = 100f;
            sv.Needs.Hunger = 40f;
            sv.Needs.Thirst = 40f;
            sv.Needs.Fatigue = 20f;
            return sv;
        }

        private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Material)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = 50;
            item.weight = 0.5f;
            if (type == ItemType.Food) item.hungerRestore = 30f;
            return item;
        }

        // ── #189 Ration Stretcher ────────────────────────────────────────

        [Test]
        public void RationStretcher_EarnedAfter20Meals_SkipsWater25Percent()
        {
            Assert.IsFalse(_perks.Has(_sv, SurvivalPerkSystem.RationStretcherId));

            for (int i = 0; i < SurvivalPerkSystem.MealsForRationStretcher - 1; i++)
                _perks.RecordMealCooked(_sv, 1);
            Assert.IsFalse(_perks.Has(_sv, SurvivalPerkSystem.RationStretcherId));

            _perks.RecordMealCooked(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.RationStretcherId));
            Assert.AreEqual(20, _perks.GetCounters(_sv.Id).MealsCooked);

            // Deterministic free-water roll
            var always = new System.Random(0);
            // Force many rolls — with perk, ~25% free; without, never free
            int free = 0;
            for (int i = 0; i < 200; i++)
            {
                if (_perks.RollSkipCleanWater(_sv, always)) free++;
            }
            Assert.Greater(free, 20, "perk should free water sometimes");
            Assert.Less(free, 100, "should not free water always");

            var other = MakeSurvivor("sv_other", "Other");
            Assert.IsFalse(_perks.RollSkipCleanWater(other, new System.Random(0)));
        }

        [Test]
        public void CookingSystem_RecordsMeals_AndCanSkipWater()
        {
            var cooking = new CookingSystem(_inv, _water, new System.Random(1));
            cooking.BindSurvivalPerks(_perks, () => 3);
            cooking.SetMealDefinition(CookingSystem.CreateCookedMealDefinition());

            float waterBefore = _water.CleanWater;
            Assert.IsTrue(cooking.CookMeal(_sv));
            Assert.AreEqual(1, _perks.GetCounters(_sv.Id).MealsCooked);
            // Without perk, water is consumed
            Assert.Less(_water.CleanWater, waterBefore);

            for (int i = 0; i < 19; i++)
                Assert.IsTrue(cooking.CookMeal(_sv));
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.RationStretcherId));
        }

        // ── #190 Iron Stomach ────────────────────────────────────────────

        [Test]
        public void IronStomach_EarnedAfterTwoGastricRecoveries()
        {
            _perks.RecordIllnessRecovery(_sv, AfflictionSO.Ids.Botulism, 1);
            Assert.IsFalse(_perks.Has(_sv, SurvivalPerkSystem.IronStomachId));
            _perks.RecordIllnessRecovery(_sv, AfflictionSO.Ids.Dysentery, 2);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.IronStomachId));
            Assert.AreEqual(0.10f, _perks.GetContaminatedIllnessChanceMultiplier(_sv), 0.001f);
            Assert.AreEqual(0.035f, _perks.ScaleIllnessChance(_sv, 0.35f), 0.001f);
        }

        [Test]
        public void IronStomach_EatSpoiled_UsesReducedChance()
        {
            // Grant perk via recoveries
            _perks.RecordIllnessRecovery(_sv, "food_poisoning", 1);
            _perks.RecordIllnessRecovery(_sv, "botulism", 1);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.IronStomachId));

            var medical = new MedicalSystem(_needs, _inv);
            medical.RegisterAffliction(MakeAffliction(AfflictionSO.Ids.Botulism));

            var spoiled = MakeItem("spoiled_meat", ItemType.ContaminatedFood);
            spoiled.hungerRestore = 20f;
            _inv.Add(spoiled, 5);

            // RNG that always rolls 0.2 — base 0.35 would hit, scaled 0.035 would not
            var ctx = new AIContext
            {
                Survivor = _sv,
                Inventory = _inv,
                MedicalSystem = medical,
                SurvivalPerks = _perks,
                Random = new FixedRandom(0.2)
            };
            var eat = ScriptableObject.CreateInstance<EatActionSO>();
            eat.Execute(ctx);
            Assert.IsFalse(medical.HasAffliction(_sv, AfflictionSO.Ids.Botulism),
                "Iron Stomach should reduce 0.35 chance so roll 0.2 does not inflict");
        }

        [Test]
        public void DrinkDirtyWater_IronStomach_ReducesIllness()
        {
            _perks.RecordIllnessRecovery(_sv, "dysentery", 1);
            _perks.RecordIllnessRecovery(_sv, "dysentery", 1);

            _water.AddDirty(5f);
            _sv.Needs.Thirst = 80f;
            float healthBefore = _sv.Needs.Health;

            var drink = ScriptableObject.CreateInstance<DrinkContaminatedWaterActionSO>();
            drink.DirtyWaterIllnessChance = 1f; // would always hit without perk
            var ctx = new AIContext
            {
                Survivor = _sv,
                WaterStorage = _water,
                SurvivalPerks = _perks,
                Random = new FixedRandom(0.5) // 0.5 > 0.10 scaled chance
            };
            drink.Execute(ctx);
            Assert.AreEqual(healthBefore, _sv.Needs.Health, 0.01f,
                "scaled chance 0.10 should miss roll 0.5");
        }

        // ── #191 Wasteland Brewer ────────────────────────────────────────

        [Test]
        public void WastelandBrewer_EarnedAfter30Crops_UnlocksMoonshine()
        {
            for (int i = 0; i < 29; i++)
                _perks.RecordCropHarvested(_sv, 1, 1);
            Assert.IsFalse(_perks.CanCraftMoonshine(_sv));

            _perks.RecordCropHarvested(_sv, 1, 1);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.WastelandBrewerId));
            Assert.IsTrue(_perks.CanCraftMoonshine(_sv));
        }

        [Test]
        public void Workbench_CraftMoonshine_RequiresBrewer_AndConsumes()
        {
            var wb = new WorkbenchSystem(_inv, id => null);
            wb.BindSurvivalPerks(_perks, _needs);
            var fungi = WorkbenchSystem.CreateMutatedFungiDefinition();
            var dirty = MakeItem(SurvivalPerkSystem.DirtyWaterItemId, ItemType.Water);
            var moon = WorkbenchSystem.CreateMoonshineDefinition();
            wb.SetMoonshineItems(moon, fungi, dirty, MakeItem("fuel", ItemType.Fuel));
            _inv.Add(fungi, 10);
            _inv.Add(dirty, 5);

            Assert.IsFalse(wb.CanCraftMoonshine(_sv));
            for (int i = 0; i < 30; i++) _perks.RecordCropHarvested(_sv);
            Assert.IsTrue(wb.CraftMoonshine(_sv));
            Assert.AreEqual(1, _inv.Count(moon));
            Assert.AreEqual(8, _inv.Count(fungi));

            // Drink for morale / hangover
            float moraleBefore = _sv.Needs.Morale;
            float fatigueBefore = _sv.Needs.Fatigue;
            Assert.IsTrue(wb.ConsumeMoonshine(_sv, asFuel: false));
            Assert.Greater(_sv.Needs.Morale, moraleBefore);
            Assert.Greater(_sv.Needs.Fatigue, fatigueBefore);
        }

        // ── #192 The Butcher ─────────────────────────────────────────────

        [Test]
        public void Butcher_FirstProcess_GrantsPerk_AndBloodstained()
        {
            Assert.IsFalse(_sv.HasAestheticTag(SurvivalPerkSystem.BloodstainedTag));
            _perks.RecordButchery(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.ButcherId));
            Assert.IsTrue(_sv.HasAestheticTag(SurvivalPerkSystem.BloodstainedTag));
            Assert.AreEqual(0.5f, _perks.GetCorpseProcessTimeMultiplier(_sv), 0.001f);
            Assert.AreEqual(1, _perks.GetExtraBonesYield(_sv));
            Assert.AreEqual(1, _perks.GetExtraMeatYield(_sv));
        }

        [Test]
        public void CorpseProcessForParts_Butcher_DoublesYield_HalvesTime()
        {
            var corpseSys = new CorpseManagementSystem(_needs, _inv, rng: new System.Random(1));
            corpseSys.BindSurvivalPerks(_perks, () => 1);
            corpseSys.SetItemDefinitions(
                CorpseManagementSystem.CreateCorpseDefinition(),
                CorpseManagementSystem.CreateFertilizerDefinition());
            corpseSys.SetButcherYieldDefinitions(
                CorpseManagementSystem.CreateBonesDefinition(),
                CorpseManagementSystem.CreateMeatDefinition());

            var dead = MakeSurvivor("sv_dead", "Dead");
            dead.State = SurvivorState.Dead;
            Assert.IsTrue(corpseSys.CreateCorpseFrom(dead));

            // First process grants Butcher mid-action? Record happens after yield calc
            // without perk first: base yields
            Assert.IsTrue(corpseSys.ProcessForParts(_sv, out int bones1, out int meat1, out float hours1));
            // After first process, Butcher is granted
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.ButcherId));
            // Yields were base (perk granted after yield for first process)
            Assert.AreEqual(CorpseManagementSystem.BaseBonesYield, bones1);
            Assert.AreEqual(CorpseManagementSystem.BaseProcessHours, hours1, 0.01f);

            var dead2 = MakeSurvivor("sv_dead2", "Dead2");
            dead2.State = SurvivorState.Dead;
            corpseSys.CreateCorpseFrom(dead2);
            Assert.IsTrue(corpseSys.ProcessForParts(_sv, out int bones2, out int meat2, out float hours2));
            Assert.AreEqual(CorpseManagementSystem.BaseBonesYield + 1, bones2);
            Assert.AreEqual(CorpseManagementSystem.BaseMeatYield + 1, meat2);
            Assert.AreEqual(CorpseManagementSystem.BaseProcessHours * 0.5f, hours2, 0.01f);
            Assert.IsTrue(_sv.HasAestheticTag("bloodstained"));
        }

        // ── #193 Pharmacologist ──────────────────────────────────────────

        [Test]
        public void Pharmacologist_EarnedAfter10MedicalCrafts_HighYield()
        {
            for (int i = 0; i < 9; i++)
                _perks.RecordMedicalCraft(_sv, 1, 1);
            Assert.IsFalse(_perks.CanProduceHighYieldMeds(_sv));
            _perks.RecordMedicalCraft(_sv, 1, 1);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.PharmacologistId));

            Assert.AreEqual(
                SurvivalPerkSystem.HighYieldAntibioticId,
                _perks.ResolveMedicalCraftResultId(_sv, "antibiotics"));
            Assert.AreEqual(
                SurvivalPerkSystem.HighYieldIodineId,
                _perks.ResolveMedicalCraftResultId(_sv, "iodine_pills"));
            Assert.AreEqual("bandage", _perks.ResolveMedicalCraftResultId(_sv, "bandage"));
            Assert.IsTrue(SurvivalPerkSystem.IgnoresAntibioticResistance(
                SurvivalPerkSystem.HighYieldAntibioticId));
            Assert.AreEqual(0.5f, SurvivalPerkSystem.GetTreatmentDurationMultiplier(
                SurvivalPerkSystem.HighYieldIodineId), 0.001f);
        }

        [Test]
        public void CraftingSystem_MedicalCraft_RecordsAndUpgrades()
        {
            var crafting = new CraftingSystem(_inv);
            crafting.BindSurvivalPerks(_perks, () => 1);
            crafting.AddStation(new CraftingStation { id = "workbench", Condition = 100f });

            var chem = MakeItem("chemicals", ItemType.Material);
            var ab = MakeItem("antibiotics", ItemType.Medical);
            _inv.Add(chem, 20);

            var recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.id = "recipe_antibiotics";
            recipe.recipeName = "Antibiotics";
            recipe.result = ab;
            recipe.resultAmount = 1;
            recipe.craftingTimeHours = 1f;
            recipe.requiredStationId = "workbench";
            recipe.ingredients = new List<Ingredient>
            {
                new Ingredient { item = chem, amount = 1 }
            };

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(crafting.StartCraft(recipe, _sv));
                crafting.Tick(2f);
            }
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.PharmacologistId));
            // 11th craft should produce high-yield
            Assert.IsTrue(crafting.StartCraft(recipe, _sv));
            crafting.Tick(2f);
            Assert.GreaterOrEqual(
                _inv.CountById(SurvivalPerkSystem.HighYieldAntibioticId), 1);
        }

        [Test]
        public void AntibioticResistance_HighYield_IgnoresResistance()
        {
            var resist = new AntibioticResistanceSystem();
            // Build resistance
            var rng = new System.Random(1);
            for (int i = 0; i < 5; i++)
                resist.TryUseExpired("sv_cook", rng, "antibiotics");

            // High-yield always succeeds regardless of resistance
            Assert.IsTrue(resist.TryUseExpired(
                "sv_cook", new System.Random(99), SurvivalPerkSystem.HighYieldAntibioticId));
        }

        // ── #194 Mycology ────────────────────────────────────────────────

        [Test]
        public void Mycology_EarnedAfter50PlanterHours_PreventsToxicSpore()
        {
            var planter = new PlanterBox();
            var fungi = CropSO.CreateMutatedFungi();
            fungi.IsToxicStrain = true;
            planter.PlantSeed(fungi);
            // Grow to mature quickly
            planter.Water(true);
            planter.Tick(20f, roomTemperature: 20f, isLightPowered: true);

            planter.RecordDutyHours(_sv, 49f, _perks, 1);
            Assert.IsFalse(_perks.CanIdentifyToxicFungi(_sv));
            planter.RecordDutyHours(_sv, 1.5f, _perks, 1);
            Assert.IsTrue(_perks.Has(_sv, SurvivalPerkSystem.MycologyId));
            Assert.IsTrue(_perks.CanIdentifyToxicFungi(_sv));
            Assert.IsTrue(planter.IsActiveCropVisiblyToxic(_sv, _perks));

            var list = new List<Survivor> { _sv };
            Assert.IsTrue(_perks.PreventsToxicSporeEvent(list));
            // Even at 100% chance, Mycology blocks the event
            Assert.IsFalse(planter.TryToxicSporeEvent(list, _perks, new System.Random(1), chance: 1f));
            Assert.AreEqual(CropLifecycleStage.Mature, planter.Stage);
        }

        [Test]
        public void ToxicSpore_WithoutMycology_CanRuinBay()
        {
            var planter = new PlanterBox();
            planter.PlantSeed(CropSO.CreatePotatoes());
            planter.Water(true);
            planter.Tick(50f, 20f, true);
            Assert.AreEqual(CropLifecycleStage.Mature, planter.Stage);

            var list = new List<Survivor> { _sv };
            Assert.IsTrue(planter.TryToxicSporeEvent(list, _perks, new System.Random(1), chance: 1f));
            Assert.AreEqual(CropLifecycleStage.Dead, planter.Stage);
        }

        // ── Save / load ──────────────────────────────────────────────────

        [Test]
        public void SurvivalPerks_SaveRestore_RoundTrip()
        {
            for (int i = 0; i < 5; i++) _perks.RecordMealCooked(_sv, 1);
            _perks.RecordIllnessRecovery(_sv, "botulism", 1);
            _perks.RecordCropHarvested(_sv, 12, 1);
            _perks.RecordPlanterHours(_sv, 7.5f, 1);

            var save = _perks.CaptureState();
            var restored = new SurvivalPerkSystem();
            restored.Bind(_progression);
            restored.RestoreState(save);

            var c = restored.GetCounters(_sv.Id);
            Assert.AreEqual(5, c.MealsCooked);
            Assert.AreEqual(1, c.GastricIllnessRecoveries);
            Assert.AreEqual(12, c.CropsHarvested);
            Assert.AreEqual(7.5f, c.PlanterHours, 0.01f);
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private static AfflictionSO MakeAffliction(string id)
        {
            var a = ScriptableObject.CreateInstance<AfflictionSO>();
            a.id = id;
            a.displayName = id;
            return a;
        }

        private static int CountId(InventoryClass inv, string id)
        {
            int n = 0;
            if (inv?.Slots == null) return 0;
            for (int i = 0; i < inv.Slots.Count; i++)
            {
                var s = inv.Slots[i];
                if (s?.Item != null && s.Item.id == id) n += s.Amount;
            }
            return n;
        }

        /// <summary>Deterministic RNG that always returns a fixed double in NextDouble().</summary>
        private sealed class FixedRandom : System.Random
        {
            private readonly double _value;
            public FixedRandom(double value) { _value = value; }
            public override double NextDouble() => _value;
            public override int Next() => 0;
            public override int Next(int maxValue) => 0;
            public override int Next(int minValue, int maxValue) => minValue;
        }
    }
}
