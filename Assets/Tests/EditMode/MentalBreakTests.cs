using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Mental-break system tests (Prompt #29). Verifies the full chain:
    /// morale threshold tracking → break roll → BingeEater force-consumes
    /// 3x daily rations regardless of AI scoring → passive morale drain
    /// to other survivors → natural cure → interpersonal affinity
    /// matrix mutation from EventEffect.
    ///
    /// The BingeEater side effect is driven by a delegate (BingeEatHandler)
    /// injected by the host (Core/GameBootstrap). The Survivors assembly
    /// can't reference Inventory directly, so the actual consumption
    /// happens in the handler. In the test we provide a handler that
    /// does the consumption against the test's Inventory instance.
    /// </summary>
    [TestFixture]
    public class MentalBreakTests
    {
        private const float Eps = 1e-3f;

        private NeedsProfile _needsProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemDefinition _foodItem;
        private ItemDefinition _waterItem;
        private Shelter _shelter;
        private MedicalSystem _medicalSystem;
        private MentalBreakSystem _mentalBreakSystem;
        private List<Survivor> _allSurvivors;
        private System.Random _rng;

        private MentalBreakSO _bingeEater;
        private MentalBreakSO _violentParanoia;

        [SetUp]
        public void SetUp()
        {
            EventBus.Clear();

            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsProfile.hungerPerHour = 1f;
            _needsProfile.thirstPerHour = 1f;
            _needsProfile.fatiguePerHour = 0.5f;
            _needsProfile.hungerCritical = 100f;
            _needsProfile.thirstCritical = 100f;
            _needsProfile.warmthCritical = 10f;
            _needsProfile.moraleLossPerHourWhileCritical = 1f;

            _needsSystem = new NeedsSystem(_needsProfile);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            _foodItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _foodItem.id = "canned_food";
            _foodItem.displayName = "Canned Food";
            _foodItem.weight = 0.5f;
            _foodItem.type = ItemType.Food;
            _foodItem.hungerRestore = 30f;
            _foodItem.stackMax = 20;

            _waterItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _waterItem.id = "clean_water";
            _waterItem.displayName = "Clean Water";
            _waterItem.weight = 1.0f;
            _waterItem.type = ItemType.Water;
            _waterItem.thirstRestore = 40f;
            _waterItem.stackMax = 20;

            _inventory.Add(_foodItem, 10);
            _inventory.Add(_waterItem, 10);

            _shelter = new Shelter();
            _shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });
            _shelter.AddModule(new ShelterModuleInstance("radio", 1) { Fuel = 10f });

            _medicalSystem = new MedicalSystem(_needsSystem, _inventory, _shelter);

            _bingeEater = ScriptableObject.CreateInstance<MentalBreakSO>();
            _bingeEater.id = "binge_eater";
            _bingeEater.displayName = "Binge Eater";
            _bingeEater.consumptionMultiplier = 3f;
            _bingeEater.minFoodValueForBinge = 0f;
            _bingeEater.passiveMoraleDrainPerHour = 1f;
            _bingeEater.cureHours = 48f;
            _bingeEater.requiresMedicalBed = false; // comfort items + time
            _bingeEater.comfortItemCureAmount = 24f; // 1 comfort item = 50% cure
            _bingeEater.TraitWeights = new List<RiskBiasWeight>
            {
                new RiskBiasWeight { Trait = RiskBiasTrait.Realist, Weight = 1f }
            };

            _violentParanoia = ScriptableObject.CreateInstance<MentalBreakSO>();
            _violentParanoia.id = "violent_paranoia";
            _violentParanoia.displayName = "Violent Paranoia";
            _violentParanoia.sabotageChancePerTick = 0.05f;
            _violentParanoia.passiveMoraleDrainPerHour = 2f;
            _violentParanoia.cureHours = 72f;
            _violentParanoia.requiresMedicalBed = true; // only curable via medical bed
            _violentParanoia.comfortItemCureAmount = 12f; // comfort items help too
            _violentParanoia.TraitWeights = new List<RiskBiasWeight>
            {
                new RiskBiasWeight { Trait = RiskBiasTrait.Paranoid, Weight = 2f },
                new RiskBiasWeight { Trait = RiskBiasTrait.Realist, Weight = 1f }
            };

            _allSurvivors = new List<Survivor>();

            _mentalBreakSystem = new MentalBreakSystem();
            _mentalBreakSystem.RegisterBreak(_bingeEater);
            _mentalBreakSystem.RegisterBreak(_violentParanoia);

            // Inject a BingeEatHandler that performs the actual consumption
            // against the test's Inventory. This is the host-side concern
            // (Core/GameBootstrap in production) — the Survivors assembly
            // never touches Inventory directly.
            _mentalBreakSystem.BingeEatHandler = (sv, br) =>
            {
                // Find the highest-value food slot.
                InventorySlot best = null;
                float bestValue = float.NegativeInfinity;
                for (int i = 0; i < _inventory.Slots.Count; i++)
                {
                    var slot = _inventory.Slots[i];
                    if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                    if (slot.Item.type != ItemType.Food) continue;
                    if (slot.Item.hungerRestore < br.minFoodValueForBinge) continue;
                    if (slot.Item.hungerRestore > bestValue)
                    {
                        best = slot;
                        bestValue = slot.Item.hungerRestore;
                    }
                }
                if (best == null) return 0;
                int wanted = Mathf.Max(1, Mathf.CeilToInt(br.consumptionMultiplier));
                int consumed = Mathf.Min(wanted, best.Amount);
                if (consumed <= 0) return 0;
                _inventory.Remove(best.Item, consumed);
                sv.Needs.Hunger = Mathf.Max(0f, sv.Needs.Hunger - best.Item.hungerRestore * consumed);
                return consumed;
            };

            _rng = new System.Random(42);
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Clear();
        }

        // -------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------

        private Survivor MakeSurvivor(string id, RiskBiasTrait trait, float morale = 75f)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = id,
                RiskBias = trait
            };
            sv.Needs.Morale = morale;
            _needsSystem.Register(sv);
            _radSystem.Register(sv);
            _allSurvivors.Add(sv);
            return sv;
        }

        // -------------------------------------------------------------------
        // BingeEater (the spec's headline test)
        // -------------------------------------------------------------------

        [Test]
        public void BingeEater_ConsumesThreeTimesDailyRations_RegardlessOfAiScoring()
        {
            // Setup: 10 rations in inventory. Survivor has Hunger at 50
            // (would normally consume 1 ration). The BingeEater must consume
            // 3 in a single tick regardless of the AI score.
            var survivor = MakeSurvivor("sv_binger", RiskBiasTrait.Realist);
            Assert.AreEqual(10, _inventory.Count(_foodItem), "Setup: 10 rations in inventory.");

            // Force the survivor directly into BingeEater (we test the
            // CONSUMPTION behavior here; the ROLL is tested separately in
            // LowMorale_For48Hours_RollsForBreak_AndAssignsMatchingTraitBreak).
            // This avoids seed-dependence in the weight roll.
            survivor.currentMentalBreakId = "binge_eater";
            survivor.mentalBreakCureProgress = 0f;

            // The system itself force-consumes; this is what bypasses the AI.
            // The handler was injected in SetUp; we trigger one tick.
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);

            Assert.AreEqual(7, _inventory.Count(_foodItem),
                "BingeEater must consume 3 rations in a single tick (10 -> 7).");
            // Hunger is clamped to 0; with 50 starting and 3x30=90 of restore,
            // it should bottom out at 0. The key assertion is the INVENTORY
            // drain above — the 3x consumption is the spec's headline.
            Assert.AreEqual(0f, survivor.Needs.Hunger, Eps,
                "Hunger should bottom out at 0 after 3x30=90 restore.");
        }

        // -------------------------------------------------------------------
        // Low-morale threshold and break roll
        // -------------------------------------------------------------------

        [Test]
        public void LowMorale_For48Hours_RollsForBreak_AndAssignsMatchingTraitBreak()
        {
            var survivor = MakeSurvivor("sv_low", RiskBiasTrait.Paranoid);
            survivor.Needs.Morale = 5f; // below the 10 threshold

            // Tick 47h: no break yet.
            _mentalBreakSystem.Tick(47f, _allSurvivors, _rng);
            Assert.IsFalse(survivor.HasMentalBreak, "47h of low morale must NOT trigger a break.");
            Assert.AreEqual(47f, survivor.lowMoraleHours, Eps);

            // Tick one more hour: 48h threshold crossed, break rolls.
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);
            Assert.IsTrue(survivor.HasMentalBreak,
                "48h of continuous low morale MUST trigger a break.");
            // Paranoid is weighted 2x on _violentParanoia and 1x on _bingeEater.
            // Either is acceptable, but the most likely is ViolentParanoia.
            Assert.IsTrue(survivor.currentMentalBreakId == "binge_eater"
                       || survivor.currentMentalBreakId == "violent_paranoia",
                $"Break id was {survivor.currentMentalBreakId}, expected binge_eater or violent_paranoia.");
        }

        [Test]
        public void Morale_RecoversAboveThreshold_ResetsLowMoraleCounter()
        {
            var survivor = MakeSurvivor("sv_recover", RiskBiasTrait.Realist);
            survivor.Needs.Morale = 5f;
            _mentalBreakSystem.Tick(20f, _allSurvivors, _rng);
            Assert.AreEqual(20f, survivor.lowMoraleHours, Eps);

            // Climb back above threshold.
            survivor.Needs.Morale = 50f;
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);
            Assert.AreEqual(0f, survivor.lowMoraleHours, Eps,
                "Counter must reset when morale climbs back above the threshold.");
        }

        [Test]
        public void TryRollForBreak_NoBreaksRegistered_NoOp()
        {
            var bare = new MentalBreakSystem();
            var survivor = MakeSurvivor("sv_bare", RiskBiasTrait.Realist);
            survivor.lowMoraleHours = 100f;

            bool rolled = bare.TryRollForBreak(survivor, _rng);

            Assert.IsFalse(rolled, "With no registered breaks, the roll is a no-op.");
            Assert.IsFalse(survivor.HasMentalBreak);
        }

        [Test]
        public void TryRollForBreak_AlreadyBroken_Overwrites()
        {
            // The API doesn't guard against re-rolling; verify it doesn't
            // crash on an already-broken survivor.
            var survivor = MakeSurvivor("sv_already", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            bool rolled = _mentalBreakSystem.TryRollForBreak(survivor, _rng);
            Assert.IsTrue(rolled, "TryRollForBreak always runs the weighted roll.");
            Assert.IsTrue(survivor.HasMentalBreak);
        }

        // -------------------------------------------------------------------
        // Passive morale drain to other survivors
        // -------------------------------------------------------------------

        [Test]
        public void BrokenSurvivor_DrainsMoraleOfOthers()
        {
            var broken = MakeSurvivor("sv_broken", RiskBiasTrait.Realist, morale: 30f);
            var other1  = MakeSurvivor("sv_other1", RiskBiasTrait.Realist, morale: 60f);
            var other2  = MakeSurvivor("sv_other2", RiskBiasTrait.Realist, morale: 80f);
            broken.currentMentalBreakId = "binge_eater";

            // BingeEater has passiveMoraleDrainPerHour = 1; tick 1h.
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);

            // Other survivors drain 1 per hour; broken itself does NOT
            // (the drain is for OTHERS).
            Assert.AreEqual(59f, other1.Needs.Morale, Eps,
                "Other survivor #1 should lose 1 morale per hour near a BingeEater.");
            Assert.AreEqual(79f, other2.Needs.Morale, Eps,
                "Other survivor #2 should lose 1 morale per hour near a BingeEater.");
        }

        // -------------------------------------------------------------------
        // Natural cure
        // -------------------------------------------------------------------

        [Test]
        public void Break_NaturalCure_ResolvesAfterCureHours()
        {
            var survivor = MakeSurvivor("sv_cure", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            // BingeEater cureHours = 48. Skip to 47h: still broken.
            _mentalBreakSystem.Tick(47f, _allSurvivors, _rng);
            Assert.IsTrue(survivor.HasMentalBreak, "47h in: still broken.");

            // 48h: cured.
            _mentalBreakSystem.Tick(1f, _allSurvivors, _rng);
            Assert.IsFalse(survivor.HasMentalBreak, "48h in: cured.");
        }

        [Test]
        public void Cure_ManualCall_ResetsBreakAndCounter()
        {
            var survivor = MakeSurvivor("sv_force", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            survivor.lowMoraleHours = 30f;

            _mentalBreakSystem.Cure(survivor);

            Assert.IsFalse(survivor.HasMentalBreak);
            Assert.AreEqual(0f, survivor.lowMoraleHours, Eps,
                "Cure must reset the low-morale counter so a fresh break can accumulate.");
        }

        // -------------------------------------------------------------------
        // Interpersonal affinity
        // -------------------------------------------------------------------

        [Test]
        public void Affinity_Adjust_ClampedToRange()
        {
            _mentalBreakSystem.Affinity.Adjust("sv_a", "sv_b", 200f);
            Assert.AreEqual(100f, _mentalBreakSystem.Affinity.Get("sv_a", "sv_b"), Eps,
                "Adjust must clamp to +100.");

            _mentalBreakSystem.Affinity.Adjust("sv_a", "sv_b", -300f);
            Assert.AreEqual(-100f, _mentalBreakSystem.Affinity.Get("sv_a", "sv_b"), Eps,
                "Adjust must clamp to -100.");
        }

        [Test]
        public void Affinity_IsSymmetric()
        {
            _mentalBreakSystem.Affinity.Adjust("sv_a", "sv_b", 25f);
            Assert.AreEqual(25f, _mentalBreakSystem.Affinity.Get("sv_a", "sv_b"), Eps);
            Assert.AreEqual(25f, _mentalBreakSystem.Affinity.Get("sv_b", "sv_a"), Eps,
                "Affinity is undirected: a->b == b->a.");
        }

        [Test]
        public void EventEffect_AffinityDelta_MutatesMatrixViaEventRunner()
        {
            var a = MakeSurvivor("sv_a", RiskBiasTrait.Realist);
            var b = MakeSurvivor("sv_b", RiskBiasTrait.Realist);

            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "evt_affinity_test";
            ev.title = "Test";
            ev.weight = 1f;
            ev.conditions = new EventConditions { MinDay = 1 };
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "warm_feelings",
                    Text = "You grow closer.",
                    MoraleDelta = 0f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect
                        {
                            SurvivorAId = string.Empty,
                            SurvivorBId = "sv_b",
                            AffinityDelta = 30f
                        }
                    }
                }
            };

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { ev });
            var ctx = new EventContext(a, _shelter, _inventory, _rng)
            {
                AllSurvivors = _allSurvivors,
                MentalBreak = _mentalBreakSystem
            };
            runner.Run(ev, ctx);
            runner.ApplyChoice(ev, ev.choices[0], ctx);

            Assert.AreEqual(30f, _mentalBreakSystem.Affinity.Get("sv_a", "sv_b"), Eps,
                "EventEffect.AffinityDelta should have mutated the matrix.");
        }

        [Test]
        public void EventEffect_AffinityDelta_IgnoresEqualPair()
        {
            var a = MakeSurvivor("sv_only", RiskBiasTrait.Realist);
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "evt_self_affinity";
            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "x",
                    Text = "x",
                    Effects = new List<EventEffect>
                    {
                        new EventEffect { SurvivorAId = string.Empty, SurvivorBId = "sv_only", AffinityDelta = 50f }
                    }
                }
            };
            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { ev });
            var ctx = new EventContext(a, _shelter, _inventory, _rng)
            {
                AllSurvivors = _allSurvivors,
                MentalBreak = _mentalBreakSystem
            };
            runner.Run(ev, ctx);
            runner.ApplyChoice(ev, ev.choices[0], ctx);

            Assert.AreEqual(0f, _mentalBreakSystem.Affinity.Get("sv_only", "sv_only"), Eps,
                "Self-affinity must remain 0.");
        }

        // -------------------------------------------------------------------
        // Trait weight helper
        // -------------------------------------------------------------------

        [Test]
        public void WeightForTrait_EmptyTraitList_DefaultsToOne()
        {
            var bare = ScriptableObject.CreateInstance<MentalBreakSO>();
            bare.id = "bare";
            bare.TraitWeights = new List<RiskBiasWeight>();
            Assert.AreEqual(1f, MentalBreakSystem.WeightForTrait(bare, RiskBiasTrait.Paranoid));
            Assert.AreEqual(1f, MentalBreakSystem.WeightForTrait(bare, RiskBiasTrait.Fatalist));
        }

        [Test]
        public void WeightForTrait_ReturnsConfiguredWeight()
        {
            Assert.AreEqual(2f, MentalBreakSystem.WeightForTrait(_violentParanoia, RiskBiasTrait.Paranoid));
            Assert.AreEqual(1f, MentalBreakSystem.WeightForTrait(_violentParanoia, RiskBiasTrait.Realist));
            Assert.AreEqual(0f, MentalBreakSystem.WeightForTrait(_violentParanoia, RiskBiasTrait.Fatalist),
                "Unlisted trait has weight 0.");
        }

        // -------------------------------------------------------------------
        // Comfort-item cure
        // -------------------------------------------------------------------

        [Test]
        public void TryCureWithComfortItem_AdvancesCureProgress_ByConfiguredAmount()
        {
            var survivor = MakeSurvivor("sv_comfort", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            survivor.mentalBreakCureProgress = 0f;

            // The BingeEater test break has comfortItemCureAmount = 24h and
            // cureHours = 48h. One comfort item should advance by 24h.
            _mentalBreakSystem.ComfortCureHandler = (sv, br) => true;

            bool applied = _mentalBreakSystem.TryCureWithComfortItem(survivor);

            Assert.IsTrue(applied, "Cure should apply when handler returns true.");
            Assert.IsTrue(survivor.HasMentalBreak, "Not yet at cureHours; still broken.");
            Assert.AreEqual(24f, survivor.mentalBreakCureProgress, Eps,
                "Cure progress should advance by comfortItemCureAmount (24).");
        }

        [Test]
        public void TryCureWithComfortItem_CuresWhenProgressReachesCureHours()
        {
            var survivor = MakeSurvivor("sv_cure_now", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            // Start one cure-amount away from full cure.
            survivor.mentalBreakCureProgress = 24f; // cureHours = 48

            _mentalBreakSystem.ComfortCureHandler = (sv, br) => true;

            _mentalBreakSystem.TryCureWithComfortItem(survivor);

            Assert.IsFalse(survivor.HasMentalBreak,
                "Cure progress crossed cureHours: break should resolve.");
            Assert.AreEqual(0f, survivor.mentalBreakCureProgress, Eps,
                "Cure clears the progress counter.");
        }

        [Test]
        public void TryCureWithComfortItem_NoHandler_NoOp()
        {
            var survivor = MakeSurvivor("sv_nohandler", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            _mentalBreakSystem.ComfortCureHandler = null;

            bool applied = _mentalBreakSystem.TryCureWithComfortItem(survivor);

            Assert.IsFalse(applied, "Without a handler, the cure must be a no-op.");
            Assert.AreEqual(0f, survivor.mentalBreakCureProgress, Eps,
                "Progress must not change when the handler is null.");
        }

        [Test]
        public void TryCureWithComfortItem_HandlerReturnsFalse_NoOp()
        {
            var survivor = MakeSurvivor("sv_nocomfort", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            _mentalBreakSystem.ComfortCureHandler = (sv, br) => false; // no item consumed

            bool applied = _mentalBreakSystem.TryCureWithComfortItem(survivor);

            Assert.IsFalse(applied);
            Assert.AreEqual(0f, survivor.mentalBreakCureProgress, Eps);
        }

        [Test]
        public void TryCureWithComfortItem_NotBroken_NoOp()
        {
            var survivor = MakeSurvivor("sv_sane", RiskBiasTrait.Realist);
            _mentalBreakSystem.ComfortCureHandler = (sv, br) => true;

            bool applied = _mentalBreakSystem.TryCureWithComfortItem(survivor);

            Assert.IsFalse(applied, "No break: no cure attempt.");
        }

        [Test]
        public void TryCureWithComfortItem_ComfortItemCureAmountIsZero_NoOp()
        {
            // A break with comfortItemCureAmount = 0 ignores comfort cures.
            var noCureBreak = ScriptableObject.CreateInstance<MentalBreakSO>();
            noCureBreak.id = "no_comfort_cure";
            noCureBreak.comfortItemCureAmount = 0f;
            noCureBreak.cureHours = 48f;
            _mentalBreakSystem.RegisterBreak(noCureBreak);

            var survivor = MakeSurvivor("sv_break", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "no_comfort_cure";
            _mentalBreakSystem.ComfortCureHandler = (sv, br) => true;

            bool applied = _mentalBreakSystem.TryCureWithComfortItem(survivor);

            Assert.IsFalse(applied,
                "A break with comfortItemCureAmount = 0 must reject comfort cures.");
            Assert.AreEqual(0f, survivor.mentalBreakCureProgress, Eps);
        }

        // -------------------------------------------------------------------
        // Medical-bed cure
        // -------------------------------------------------------------------

        [Test]
        public void TryCureMentalBreak_RequiresMedicalBed_AndBedIsOperational_Cures()
        {
            var survivor = MakeSurvivor("sv_paranoid", RiskBiasTrait.Paranoid);
            survivor.currentMentalBreakId = "violent_paranoia";
            // ViolentParanoia has requiresMedicalBed = true.
            _shelter.AddModule(new ShelterModuleInstance("medical_bed", 1) { IsEnabled = true });

            bool cured = _medicalSystem.TryCureMentalBreak(survivor, _mentalBreakSystem, _shelter);

            Assert.IsTrue(cured, "Bed operational + break requires bed: cure should succeed.");
            Assert.IsFalse(survivor.HasMentalBreak, "Break should be resolved.");
        }

        [Test]
        public void TryCureMentalBreak_NoMedicalBedModule_NoOp()
        {
            var survivor = MakeSurvivor("sv_no_bed", RiskBiasTrait.Paranoid);
            survivor.currentMentalBreakId = "violent_paranoia";
            // No medical_bed module added.

            bool cured = _medicalSystem.TryCureMentalBreak(survivor, _mentalBreakSystem, _shelter);

            Assert.IsFalse(cured, "Without a medical_bed module, the cure must fail.");
            Assert.IsTrue(survivor.HasMentalBreak, "Break must remain.");
        }

        [Test]
        public void TryCureMentalBreak_BedDisabled_NoOp()
        {
            var survivor = MakeSurvivor("sv_disabled_bed", RiskBiasTrait.Paranoid);
            survivor.currentMentalBreakId = "violent_paranoia";
            _shelter.AddModule(new ShelterModuleInstance("medical_bed", 1) { IsEnabled = false });

            bool cured = _medicalSystem.TryCureMentalBreak(survivor, _mentalBreakSystem, _shelter);

            Assert.IsFalse(cured, "Bed disabled: cure must fail.");
        }

        [Test]
        public void TryCureMentalBreak_BreakDoesNotRequireBed_NoOp()
        {
            // BingeEater has requiresMedicalBed = false; medical bed doesn't
            // cure it (use comfort items or time instead).
            var survivor = MakeSurvivor("sv_binger", RiskBiasTrait.Realist);
            survivor.currentMentalBreakId = "binge_eater";
            _shelter.AddModule(new ShelterModuleInstance("medical_bed", 1) { IsEnabled = true });

            bool cured = _medicalSystem.TryCureMentalBreak(survivor, _mentalBreakSystem, _shelter);

            Assert.IsFalse(cured,
                "BingeEater doesn't require the bed; medical-bed cure must be a no-op.");
            Assert.IsTrue(survivor.HasMentalBreak);
        }

        [Test]
        public void TryCureMentalBreak_NotBroken_NoOp()
        {
            var survivor = MakeSurvivor("sv_sane_med", RiskBiasTrait.Realist);
            _shelter.AddModule(new ShelterModuleInstance("medical_bed", 1) { IsEnabled = true });

            bool cured = _medicalSystem.TryCureMentalBreak(survivor, _mentalBreakSystem, _shelter);

            Assert.IsFalse(cured, "No break: no cure attempt.");
        }

        [Test]
        public void CanCureMentalBreak_PureRead_DoesNotMutate()
        {
            var survivor = MakeSurvivor("sv_readonly", RiskBiasTrait.Paranoid);
            survivor.currentMentalBreakId = "violent_paranoia";
            _shelter.AddModule(new ShelterModuleInstance("medical_bed", 1) { IsEnabled = true });

            bool can = _medicalSystem.CanCureMentalBreak(survivor, _mentalBreakSystem, _shelter);

            Assert.IsTrue(can);
            Assert.IsTrue(survivor.HasMentalBreak, "CanCureMentalBreak must not mutate state.");
        }

        // -------------------------------------------------------------------
        // AI mental-break comfort action
        // -------------------------------------------------------------------

        [Test]
        public void MentalBreakComfortAction_AI_ConsumesComfortAndAdvancesProgress()
        {
            // End-to-end: a Paranoid survivor in ViolentParanoia (which
            // requires the bed) but the bed is unavailable. The AI
            // comfort action uses a Comfort item instead. Pre-advance cure
            // progress so the score is non-zero (the AI prioritizes
            // near-cured breaks over fresh ones).
            var survivor = MakeSurvivor("sv_ai_comfort", RiskBiasTrait.Paranoid);
            survivor.currentMentalBreakId = "violent_paranoia";
            survivor.mentalBreakCureProgress = _violentParanoia.cureHours - _violentParanoia.comfortItemCureAmount;
            // No medical_bed module.
            _mentalBreakSystem.ComfortCureHandler = (sv, br) => true;

            // Build the AI context the way GameBootstrap does.
            var action = ScriptableObject.CreateInstance<MentalBreakComfortActionSO>();
            var ctx = new AIContext(survivor, _shelter, _inventory, _rng)
            {
                MentalBreak = _mentalBreakSystem,
                GetSurvivors = () => _allSurvivors
            };

            float score = action.EvaluateRaw(ctx);
            Assert.Greater(score, 0f,
                "Score should be > 0 for a near-cured broken survivor with handler wired.");

            action.Execute(ctx);

            Assert.IsFalse(survivor.HasMentalBreak,
                "Cure threshold reached: break should be resolved by the AI action.");
            Assert.AreEqual(0f, survivor.mentalBreakCureProgress, Eps,
                "Cure resets the progress counter to 0.");
        }
    }
}
