using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Host-integration tests for bond/burden wiring (#249–#256).
    /// Pure C# — exercises NeedsSystem, MentalBreakSystem, MedicalSystem,
    /// StructuralIntegritySystem, DynamicEconomySystem, and EatActionSO with
    /// PersonalQuestSystem bound, without GameBootstrap / scenes.
    /// </summary>
    [TestFixture]
    public class BondBurdenHostTests
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

        private void UnlockDragonsHoard(Survivor hoarder)
        {
            _quests.TryStartQuestline(hoarder, "test", 1);
            _quests.RecordSafeCarried(hoarder, safeWeightKg: 50f, fatigueLevel: 50f, currentDay: 2);
            _quests.RecordSafeCarried(hoarder, safeWeightKg: 50f, fatigueLevel: 70f, currentDay: 3);
            _quests.RecordSafeCarried(hoarder, safeWeightKg: 50f, fatigueLevel: 95f, currentDay: 4);
            Assert.IsTrue(_quests.HasDragonsHoard(hoarder));
        }

        // ── #249 NeedsSystem Selfless absorb ─────────────────────────────

        [Test]
        public void NeedsSystem_Selfless_AbsorbsTenPercentOfAllyMoraleDamage()
        {
            var mother = MakeArchetype(PersonalQuestSystem.FierceMotherId, "mother");
            Assert.IsTrue(_quests.HasSelfless(mother));

            var ally = new Survivor { Id = "ally", DisplayName = "Ally", State = SurvivorState.Idle };
            ally.Needs.Morale = 50f;
            mother.Needs.Morale = 50f;
            _survivors.Add(ally);
            _needs.Register(ally);

            _needs.Modify(ally, NeedKind.Morale, -20f);

            // Selfless absorbs 10% of the 20 → 2; ally takes 18.
            Assert.AreEqual(48f, mother.Needs.Morale, Eps);
            Assert.AreEqual(32f, ally.Needs.Morale, Eps);
        }

        // ── #252 NeedsSystem Traumatized cap ─────────────────────────────

        [Test]
        public void NeedsSystem_Traumatized_ClampsMoraleToFifty()
        {
            var dau = MakeArchetype(PersonalQuestSystem.HardenedDaughterId, "dau");
            Assert.IsTrue(_quests.HasTraumatized(dau));
            dau.Needs.Morale = 40f;

            _needs.Modify(dau, NeedKind.Morale, 80f);

            Assert.AreEqual(50f, dau.Needs.Morale, Eps);
            Assert.AreEqual(50f, _quests.GetMaxMoraleCap(dau), Eps);
        }

        // ── #249 MentalBreak Matriarch block ─────────────────────────────

        [Test]
        public void MentalBreakSystem_Matriarch_BlocksRollWhileOthersLive()
        {
            var mother = MakeArchetype(PersonalQuestSystem.FierceMotherId, "mat");
            _quests.TryStartQuestline(mother, "test", 1);
            Assert.IsTrue(_quests.CompleteQuestline(mother, 2));
            Assert.IsTrue(_quests.HasMatriarch(mother));

            var ally = new Survivor { Id = "ally_mb", DisplayName = "Ally", State = SurvivorState.Idle };
            _survivors.Add(ally);

            var despair = Track(ScriptableObject.CreateInstance<MentalBreakSO>());
            despair.id = "despair";
            despair.displayName = "Despair";
            despair.cureHours = 24f;
            despair.TraitWeights = new List<RiskBiasWeight>
            {
                new RiskBiasWeight { Trait = RiskBiasTrait.Realist, Weight = 1f }
            };

            var mbs = new MentalBreakSystem();
            mbs.RegisterBreak(despair);
            mbs.BindPersonalQuests(_quests, () => _survivors);

            mother.Needs.Morale = 0f;
            mother.lowMoraleHours = 100f;
            bool rolled = mbs.TryRollForBreak(mother, new System.Random(7));

            Assert.IsFalse(rolled, "Matriarch must not roll a break while others live.");
            Assert.IsFalse(mother.HasMentalBreak);
        }

        // ── #253 Medical Arrogant self-heal gate ─────────────────────────

        [Test]
        public void MedicalSystem_Arrogant_RefusesOtherMedic_AllowsSelf()
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

            var recipe = Track(MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
            med.RegisterTreatment(recipe);

            var psy = MakeArchetype(PersonalQuestSystem.PsychopathId, "psy");
            Assert.IsTrue(_quests.HasArrogant(psy));
            Assert.IsTrue(med.Inflict(psy, AfflictionSO.Ids.GunshotWound));

            var otherMedic = new Survivor
            {
                Id = "medic",
                DisplayName = "Medic",
                State = SurvivorState.Idle,
                MedicalSkill = 0.9f
            };

            Assert.IsFalse(med.TryStartTreatment(otherMedic, psy, recipe),
                "Arrogant patient must refuse treatment from another medic.");
            Assert.IsTrue(med.TryStartTreatment(psy, psy, recipe),
                "Arrogant patient must accept self-treatment.");
            Assert.IsTrue(med.GetActive(psy)[0].IsTreating);
        }

        // ── #250 StructuralIntegrity Pillar death debuff ─────────────────

        [Test]
        public void StructuralIntegrity_PillarDeath_SlowsRepairByTwentyPercent()
        {
            var father = MakeArchetype(PersonalQuestSystem.ExhaustedFatherId, "pillar");
            _quests.TryStartQuestline(father, "test", 1);
            for (int i = 0; i < 5; i++)
                _quests.RecordTier3ModuleBuilt(father, moduleLevel: 3, currentDay: 10 + i);
            Assert.IsTrue(_quests.HasPillarOfAtlas(father));

            var structure = new StructuralIntegritySystem(new System.Random(1));
            structure.BindPersonalQuests(_quests);

            // No struts → base rate = 2/hr. Damage then repair 1 hour.
            structure.ApplyDamage(20f);
            float integrityAfterDamage = structure.Integrity;
            Assert.Less(integrityAfterDamage, StructuralIntegritySystem.MaxIntegrity);

            float repairedHealthy = structure.Repair(1f);
            Assert.AreEqual(StructuralIntegritySystem.StrutRepairPerLevelPerHour, repairedHealthy, Eps);

            // Reset and apply Pillar death debuff.
            structure.ApplyDamage(repairedHealthy + 10f);
            float beforeDebuff = structure.Integrity;
            _quests.NotifySurvivorDied(father);
            Assert.IsTrue(_quests.PillarOfAtlasDeathDebuffActive);
            Assert.AreEqual(0.8f, _quests.GetShelterRepairSpeedMultiplier(), Eps);

            float repairedDebuffed = structure.Repair(1f);
            Assert.AreEqual(
                StructuralIntegritySystem.StrutRepairPerLevelPerHour * 0.8f,
                repairedDebuffed,
                Eps);
            Assert.AreEqual(beforeDebuff + repairedDebuffed, structure.Integrity, Eps);
            Assert.Less(repairedDebuffed, repairedHealthy);
        }

        // ── #255 Economy junk-as-medicine ────────────────────────────────

        [Test]
        public void DynamicEconomy_MasterManipulator_TradesJunkAsMedicineTier()
        {
            var liar = MakeArchetype(PersonalQuestSystem.LiarId, "liar");
            _quests.TryStartQuestline(liar, "test", 1);
            _quests.RecordLethalPhase2Cured(liar, wasHiddenFromPlayer: true, isPhase2Lethal: true, currentDay: 2);
            Assert.IsTrue(_quests.HasMasterManipulator(liar));

            var economy = new DynamicEconomySystem();
            economy.BindPersonalQuests(_quests, () => _survivors);

            var junk = Track(MakeItem("scrap_junk", ItemType.Material, tradeValue: 3f));
            float plain = economy.GetTradeValue(junk);
            float boosted = economy.GetTradeValue(junk, liar);

            Assert.Greater(plain, 0f);
            Assert.Greater(boosted, plain);
            // Medicine-tier reference is 40 × demand("antibiotics") default 1.
            Assert.AreEqual(40f, boosted, Eps);
        }

        // ── #256 EatAction Selfish 2× ration ─────────────────────────────

        [Test]
        public void EatAction_Selfish_ConsumesTwoRations()
        {
            var hoarder = MakeArchetype(PersonalQuestSystem.HoarderId, "hoarder");
            Assert.IsTrue(_quests.HasSelfish(hoarder));
            hoarder.Needs.Hunger = 80f;
            hoarder.Needs.Morale = 70f;

            var food = Track(MakeItem("canned_food", ItemType.Food, hungerRestore: 40f));
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            inv.Add(food, 5);

            var action = Track(ScriptableObject.CreateInstance<EatActionSO>());
            action.FoodItemId = "canned_food";

            var ctx = new AIContext(hoarder, null, inv, new System.Random(3))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            action.Execute(ctx);

            Assert.AreEqual(3, inv.Count(food), "Selfish must consume 2 of 5 rations.");
            Assert.AreEqual(40f, hoarder.Needs.Hunger, Eps,
                "Hunger restore applies once from Execute (second unit is inventory drain).");
        }

        [Test]
        public void EatAction_Selfish_MissedSecondRation_HitsMorale()
        {
            var hoarder = MakeArchetype(PersonalQuestSystem.HoarderId, "hoarder2");
            Assert.IsTrue(_quests.HasSelfish(hoarder));
            hoarder.Needs.Hunger = 80f;
            hoarder.Needs.Morale = 70f;

            var food = Track(MakeItem("canned_food", ItemType.Food, hungerRestore: 40f));
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            inv.Add(food, 1); // only one unit → second consume fails

            var action = Track(ScriptableObject.CreateInstance<EatActionSO>());
            action.FoodItemId = "canned_food";

            var ctx = new AIContext(hoarder, null, inv, new System.Random(3))
            {
                PersonalQuests = _quests,
                GetSurvivors = () => _survivors
            };

            action.Execute(ctx);

            Assert.AreEqual(0, inv.Count(food));
            Assert.AreEqual(
                70f - PersonalQuestSystem.SelfishMissRationMoraleHit,
                hoarder.Needs.Morale,
                Eps);
        }

        // ── #255 Deceptive UI need-masking ───────────────────────────────

        [Test]
        public void NeedsBar_Deceptive_MasksDistressNeedsWhenRolled()
        {
            var liar = MakeArchetype(PersonalQuestSystem.LiarId, "liar_ui");
            Assert.IsTrue(_quests.HasDeceptive(liar));
            liar.Needs.Hunger = 5f;
            liar.Needs.Thirst = 10f;
            liar.Needs.Health = 15f;
            liar.Needs.Fatigue = 40f;

            int maskSeed = -1;
            for (int s = 0; s < 4000; s++)
            {
                if (_quests.ShouldMaskNeedsInUi(liar, new System.Random(s)))
                {
                    maskSeed = s;
                    break;
                }
            }
            Assert.GreaterOrEqual(maskSeed, 0, "Expected a seed that triggers Deceptive mask.");

            var go = new GameObject("needs_bar_test");
            var bar = go.AddComponent<NeedsBar>();
            bar.SetNeeds(
                liar.Needs,
                liar.Needs.Health,
                radiation: 0f,
                liar,
                _quests,
                new System.Random(maskSeed));

            Assert.AreEqual(100f, bar.NeedBars["hunger"].CurrentValue, Eps);
            Assert.AreEqual(100f, bar.NeedBars["thirst"].CurrentValue, Eps);
            Assert.AreEqual(100f, bar.NeedBars["health"].CurrentValue, Eps);
            // Fatigue is not masked by the Deceptive UI lie.
            Assert.AreEqual(40f, bar.NeedBars["fatigue"].CurrentValue, Eps);

            Object.DestroyImmediate(go);
        }

        [Test]
        public void HUD_Bind_UsesPersonalQuestMask()
        {
            var liar = MakeArchetype(PersonalQuestSystem.LiarId, "liar_hud");
            liar.Needs.Hunger = 8f;
            liar.Needs.Thirst = 8f;
            liar.Needs.Health = 8f;

            int maskSeed = -1;
            for (int s = 0; s < 4000; s++)
            {
                if (_quests.ShouldMaskNeedsInUi(liar, new System.Random(s)))
                {
                    maskSeed = s;
                    break;
                }
            }
            Assert.GreaterOrEqual(maskSeed, 0);

            var go = new GameObject("hud_mask_test");
            var hud = go.AddComponent<HUD>();
            hud.BindPersonalQuests(_quests, new System.Random(maskSeed));
            hud.Bind(liar);

            Assert.AreEqual(100f, hud.NeedsBar.NeedBars["hunger"].CurrentValue, Eps);
            Object.DestroyImmediate(go);
        }

        // ── #256 Dragon's Hoard personal stash never spoils ───────────────

        [Test]
        public void Pantry_PersonalStash_SpoilsWithoutDragonsHoard()
        {
            var hoarder = MakeArchetype(PersonalQuestSystem.HoarderId, "stash_spoil");
            Assert.IsTrue(_quests.TryStealToPersonalInventory(hoarder, "canned_beans"));
            Assert.Contains("canned_beans", hoarder.HiddenItemIds);

            var inv = new InventoryClass { Capacity = 10, MaxWeight = 50f };
            // Deterministic rng that always rolls low enough to spoil.
            var pantry = new PantryContaminationSystem(inv, new System.Random(0));
            pantry.BindPersonalQuests(_quests, () => _survivors);

            int spoiled = pantry.TickPersonalStashes(_survivors, gameHours: 100f);
            Assert.Greater(spoiled, 0);
            Assert.Contains(PantryContaminationSystem.SpoiledMeatItemId, hoarder.HiddenItemIds);
        }

        [Test]
        public void Pantry_PersonalStash_NeverSpoilsWithDragonsHoard()
        {
            var hoarder = MakeArchetype(PersonalQuestSystem.HoarderId, "stash_safe");
            Assert.IsTrue(_quests.TryStealToPersonalInventory(hoarder, "canned_beans"));
            UnlockDragonsHoard(hoarder);
            Assert.IsTrue(_quests.ItemInPersonalStashNeverSpoils(hoarder));

            var inv = new InventoryClass { Capacity = 10, MaxWeight = 50f };
            var pantry = new PantryContaminationSystem(inv, new System.Random(0));
            pantry.BindPersonalQuests(_quests, () => _survivors);

            int spoiled = pantry.TickPersonalStashes(_survivors, gameHours: 100f);
            Assert.AreEqual(0, spoiled);
            Assert.Contains("canned_beans", hoarder.HiddenItemIds);
            Assert.IsFalse(hoarder.HiddenItemIds.Contains(PantryContaminationSystem.SpoiledMeatItemId));
        }

        [Test]
        public void Clothing_DragonsHoard_NeverDegrades()
        {
            var hoarder = MakeArchetype(PersonalQuestSystem.HoarderId, "clothes");
            UnlockDragonsHoard(hoarder);
            Assert.IsTrue(_quests.PersonalInventoryNeverDegrades(hoarder));

            hoarder.ClothingDurability = 50f;
            var clothing = new ClothingDegradationSystem();
            clothing.BindPersonalQuests(_quests, () => _survivors);
            clothing.Tick(hoarder, gameHours: 50f, roomHumidity: 0.9f);

            Assert.AreEqual(50f, hoarder.ClothingDurability, Eps);
            Assert.IsFalse(hoarder.IsRagged);
        }

        // ── #251 Wasteland Scout crawl debris instantly ───────────────────

        [Test]
        public void Excavation_WastelandScout_ClearsDebrisInstantly()
        {
            var son = MakeArchetype(PersonalQuestSystem.NaiveSonId, "scout");
            _quests.TryStartQuestline(son, "test", 1);
            _quests.RecordSoloRaidSurvived(son, adultsPresentInRoom: false, raidSurvived: true, currentDay: 2);
            Assert.IsTrue(_quests.HasWastelandScout(son));
            Assert.IsTrue(_quests.CanCrawlDebrisInstantly(son));

            son.Needs.Fatigue = 10f;
            var excavation = new ExcavationSystem(new System.Random(1));
            excavation.BindPersonalQuests(_quests);
            excavation.SealRoom("crawl_room", 25f);

            float cleared = excavation.ClearRubble(
                "crawl_room", son, hasShovel: false, hatchBlocked: false, workHours: 1f);

            Assert.AreEqual(25f, cleared, Eps);
            Assert.IsFalse(excavation.HasRubble("crawl_room"));
            Assert.AreEqual(10f, son.Needs.Fatigue, Eps, "Instant crawl costs no fatigue.");
        }
    }
}

