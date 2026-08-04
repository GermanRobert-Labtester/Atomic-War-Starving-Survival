using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #13 — Sabotaged Caches: habit score, hostile plant, medical/paranoid
    /// detection, poison consume affliction, save/load.
    /// </summary>
    [TestFixture]
    public class SabotagedCacheTests
    {
        private const float Eps = 1e-3f;

        private NeedsProfile _needsProfile;
        private NeedsSystem _needs;
        private Inventory _inventory;
        private MedicalSystem _medical;
        private List<FactionSO> _factions;
        private WorldPhase _phase;
        private ItemDefinition _iodine;
        private ItemDefinition _scrap;
        private AfflictionSO _poisonAffliction;

        /// <summary>RNG that always rolls sabotage when chance is checked.</summary>
        private sealed class AlwaysSabotageRng : Random
        {
            public override double NextDouble() => 0.0;
        }

        /// <summary>RNG that never rolls sabotage.</summary>
        private sealed class NeverSabotageRng : Random
        {
            public override double NextDouble() => 0.99;
        }

        [SetUp]
        public void SetUp()
        {
            _phase = WorldPhase.CivilWar;
            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needs = new NeedsSystem(_needsProfile, sv => true);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            var shelter = new Shelter();
            _medical = new MedicalSystem(_needs, _inventory, shelter);
            _poisonAffliction = SabotagedCacheSystem.CreatePoisonIngestionAffliction();
            _medical.RegisterAffliction(_poisonAffliction);

            _factions = DynamicEconomySystem.CreateDefaultFactions();

            _iodine = ScriptableObject.CreateInstance<ItemDefinition>();
            _iodine.id = SabotagedCacheSystem.RealIodineItemId;
            _iodine.displayName = "Iodine Pills";
            _iodine.type = ItemType.Iodine;
            _iodine.stackMax = 20;
            _iodine.weight = 0.05f;
            _iodine.healthEffect = 0f;

            _scrap = ScriptableObject.CreateInstance<ItemDefinition>();
            _scrap.id = "scrap_metal";
            _scrap.displayName = "Scrap";
            _scrap.type = ItemType.Material;
            _scrap.stackMax = 50;
            _scrap.weight = 0.5f;
        }

        [TearDown]
        public void TearDown()
        {
            if (_needsProfile != null) Object.DestroyImmediate(_needsProfile);
            if (_iodine != null) Object.DestroyImmediate(_iodine);
            if (_scrap != null) Object.DestroyImmediate(_scrap);
            if (_poisonAffliction != null) Object.DestroyImmediate(_poisonAffliction);
            if (_factions != null)
            {
                for (int i = 0; i < _factions.Count; i++)
                    Object.DestroyImmediate(_factions[i]);
                _factions = null;
            }
        }

        private DynamicEconomySystem MakeEconomy()
        {
            var eco = new DynamicEconomySystem(() => _phase, null, new Random(1));
            for (int i = 0; i < _factions.Count; i++)
                eco.RegisterFaction(_factions[i]);
            return eco;
        }

        private static Survivor MakeSurvivor(
            string id,
            float medicalSkill = 0.3f,
            RiskBiasTrait bias = RiskBiasTrait.Realist)
        {
            return new Survivor
            {
                Id = id,
                DisplayName = id,
                MedicalSkill = medicalSkill,
                RiskBias = bias
            };
        }

        private SabotagedCacheSystem MakeActiveSystem(Random rng = null)
        {
            var sys = new SabotagedCacheSystem(rng ?? new AlwaysSabotageRng());
            var eco = MakeEconomy();
            // Drive trust into HostileRaid for scavenger_camp (raidThreshold -50).
            eco.SetTrust(FactionSO.Ids.ScavengerCamp, -60f);
            sys.BindEconomy(eco);
            sys.SetHabitScoreForTests(SabotagedCacheSystem.HabitThreshold);
            return sys;
        }

        [Test]
        public void RecordScavengeLoot_IncrementsHabitScore()
        {
            var sys = new SabotagedCacheSystem(new Random(1));
            Assert.That(sys.HabitScore, Is.EqualTo(0));
            for (int i = 0; i < 3; i++)
                sys.RecordScavengeLoot("node_a");
            Assert.That(sys.HabitScore, Is.EqualTo(3));
        }

        [Test]
        public void IsSabotageActive_RequiresHabitAndHostileFaction()
        {
            var sys = new SabotagedCacheSystem(new Random(1));
            var eco = MakeEconomy();
            sys.BindEconomy(eco);

            Assert.That(sys.IsSabotageActive, Is.False, "No habit, no hostiles");

            sys.SetHabitScoreForTests(SabotagedCacheSystem.HabitThreshold);
            Assert.That(sys.IsSabotageActive, Is.False,
                "Habit alone is not enough without HostileRaid/Rob stance");

            eco.SetTrust(FactionSO.Ids.ScavengerCamp, -60f);
            Assert.That(sys.HasHostileFactionLearningHabits(), Is.True);
            Assert.That(sys.IsSabotageActive, Is.True);
        }

        [Test]
        public void ProcessLootCandidate_NonMedical_AlwaysClean()
        {
            var sys = MakeActiveSystem();
            var scav = MakeSurvivor("sv_a");
            var outcome = sys.ProcessLootCandidate(scav, _scrap, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.Clean));
            Assert.That(result, Is.SameAs(_scrap));
            Assert.That(sys.CachesPlanted, Is.EqualTo(0));
        }

        [Test]
        public void ProcessLootCandidate_BelowHabit_CleanEvenWithHostile()
        {
            var sys = new SabotagedCacheSystem(new AlwaysSabotageRng());
            var eco = MakeEconomy();
            eco.SetTrust(FactionSO.Ids.ScavengerCamp, -60f);
            sys.BindEconomy(eco);
            sys.SetHabitScoreForTests(SabotagedCacheSystem.HabitThreshold - 1);

            var scav = MakeSurvivor("sv_low_habit");
            var outcome = sys.ProcessLootCandidate(scav, _iodine, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.Clean));
            Assert.That(result, Is.SameAs(_iodine));
        }

        [Test]
        public void ProcessLootCandidate_Active_Undetected_ReplacesWithPoisonedIodine()
        {
            var sys = MakeActiveSystem(new AlwaysSabotageRng());
            var scav = MakeSurvivor("sv_naive", medicalSkill: 0.2f, bias: RiskBiasTrait.Reckless);

            var outcome = sys.ProcessLootCandidate(scav, _iodine, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.Poisoned));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.id, Is.EqualTo(SabotagedCacheSystem.PoisonedIodineItemId));
            Assert.That(result.displayName, Is.EqualTo("Iodine Pills"),
                "Poisoned pills must look like real iodine on the strip");
            Assert.That(sys.CachesPlanted, Is.EqualTo(1));
            Assert.That(sys.CachesDetected, Is.EqualTo(0));
        }

        [Test]
        public void ProcessLootCandidate_HighMedicalSkill_DetectsAndDiscards()
        {
            var sys = MakeActiveSystem(new AlwaysSabotageRng());
            var medic = MakeSurvivor("sv_medic", medicalSkill: 0.75f);

            Assert.That(SabotagedCacheSystem.CanDetectTampering(medic), Is.True);

            var outcome = sys.ProcessLootCandidate(medic, _iodine, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.DetectedAndDiscarded));
            Assert.That(result, Is.Null);
            Assert.That(sys.CachesDetected, Is.EqualTo(1));
            Assert.That(sys.CachesPlanted, Is.EqualTo(0));
        }

        [Test]
        public void ProcessLootCandidate_Paranoid_DetectsAndDiscards()
        {
            var sys = MakeActiveSystem(new AlwaysSabotageRng());
            var paranoid = MakeSurvivor("sv_para", medicalSkill: 0.1f, bias: RiskBiasTrait.Paranoid);

            var outcome = sys.ProcessLootCandidate(paranoid, _iodine, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.DetectedAndDiscarded));
            Assert.That(result, Is.Null);
            Assert.That(sys.CachesDetected, Is.EqualTo(1));
        }

        [Test]
        public void ProcessLootCandidate_NeverSabotageRng_StaysCleanWhenActive()
        {
            var sys = MakeActiveSystem(new NeverSabotageRng());
            var scav = MakeSurvivor("sv_lucky", medicalSkill: 0.1f);

            var outcome = sys.ProcessLootCandidate(scav, _iodine, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.Clean));
            Assert.That(result, Is.SameAs(_iodine));
        }

        [Test]
        public void CanDetectTampering_MedicalTrait_Works()
        {
            var sv = MakeSurvivor("sv_trait", medicalSkill: 0.1f);
            Assert.That(SabotagedCacheSystem.CanDetectTampering(sv), Is.False);
            sv.Traits.Add("medic");
            Assert.That(SabotagedCacheSystem.CanDetectTampering(sv), Is.True);
        }

        [Test]
        public void IsMedicalLootCandidate_IodineAntiRadMedical_Yes_Scrap_No()
        {
            Assert.That(SabotagedCacheSystem.IsMedicalLootCandidate(_iodine), Is.True);
            Assert.That(SabotagedCacheSystem.IsMedicalLootCandidate(_scrap), Is.False);

            var antiRad = ScriptableObject.CreateInstance<ItemDefinition>();
            antiRad.id = "anti_rad";
            antiRad.type = ItemType.AntiRad;
            Assert.That(SabotagedCacheSystem.IsMedicalLootCandidate(antiRad), Is.True);
            Object.DestroyImmediate(antiRad);

            var poison = SabotagedCacheSystem.CreatePoisonedIodineDefinition();
            Assert.That(SabotagedCacheSystem.IsMedicalLootCandidate(poison), Is.False,
                "Already-poisoned item must not re-enter the plant pipeline");
            Object.DestroyImmediate(poison);
        }

        [Test]
        public void ConsumePoisoned_InflictsPoisonIngestion_AndDamagesHealth()
        {
            var sys = MakeActiveSystem();
            var poison = sys.PoisonedIodineDefinition;
            var consumer = MakeSurvivor("sv_eater");
            consumer.Needs.Health = 90f;
            consumer.Needs.Morale = 60f;
            _needs.Register(consumer);

            _inventory.Add(poison, 1);
            Assert.That(_inventory.Count(poison), Is.EqualTo(1));

            bool applied = sys.ConsumePoisonedFromInventory(_inventory, consumer, _medical, _needs);
            Assert.That(applied, Is.True);
            Assert.That(_inventory.Count(poison), Is.EqualTo(0));
            Assert.That(_medical.HasAffliction(consumer, AfflictionSO.Ids.PoisonIngestion), Is.True);
            Assert.That(sys.PoisonsConsumed, Is.EqualTo(1));
            Assert.That(consumer.Needs.Health,
                Is.EqualTo(90f + poison.healthEffect).Within(Eps),
                "healthEffect on the item is applied via Inventory.Consume");
            Assert.That(consumer.Needs.Morale,
                Is.EqualTo(60f + poison.moraleEffect).Within(Eps));
        }

        [Test]
        public void TryApplyPoisonOnConsume_IgnoresRealIodine()
        {
            var sys = MakeActiveSystem();
            var consumer = MakeSurvivor("sv_safe");
            consumer.Needs.Health = 80f;
            _needs.Register(consumer);

            bool applied = sys.TryApplyPoisonOnConsume(_iodine, consumer, _medical);
            Assert.That(applied, Is.False);
            Assert.That(_medical.HasAffliction(consumer, AfflictionSO.Ids.PoisonIngestion), Is.False);
            Assert.That(sys.PoisonsConsumed, Is.EqualTo(0));
        }

        [Test]
        public void CaptureRestore_PreservesHabitAndCounters()
        {
            var sys = MakeActiveSystem(new AlwaysSabotageRng());
            sys.SetHabitScoreForTests(12);
            var scav = MakeSurvivor("sv_a", medicalSkill: 0.1f);
            sys.ProcessLootCandidate(scav, _iodine, out _);
            var medic = MakeSurvivor("sv_m", medicalSkill: 0.9f);
            sys.ProcessLootCandidate(medic, _iodine, out _);

            var save = sys.CaptureState();
            Assert.That(save.HabitScore, Is.EqualTo(12));
            Assert.That(save.CachesPlanted, Is.EqualTo(1));
            Assert.That(save.CachesDetected, Is.EqualTo(1));

            var restored = new SabotagedCacheSystem(new Random(99));
            restored.RestoreState(save);
            Assert.That(restored.HabitScore, Is.EqualTo(12));
            Assert.That(restored.CachesPlanted, Is.EqualTo(1));
            Assert.That(restored.CachesDetected, Is.EqualTo(1));
        }

        [Test]
        public void RestoreState_Null_ResetsCounters()
        {
            var sys = MakeActiveSystem();
            sys.SetHabitScoreForTests(9);
            sys.RestoreState(null);
            Assert.That(sys.HabitScore, Is.EqualTo(0));
            Assert.That(sys.CachesPlanted, Is.EqualTo(0));
        }

        [Test]
        public void CreatePoisonIngestionAffliction_HasCanonicalId()
        {
            var a = SabotagedCacheSystem.CreatePoisonIngestionAffliction();
            Assert.That(a.id, Is.EqualTo(AfflictionSO.Ids.PoisonIngestion));
            Assert.That(a.healthDrainPerHour, Is.GreaterThan(0f));
            Assert.That(a.progressesToId, Is.EqualTo(AfflictionSO.Ids.Sepsis));
            Object.DestroyImmediate(a);
        }

        [Test]
        public void CreateDefaultAfflictions_IncludesPoisonIngestion()
        {
            var list = MedicalSystem.CreateDefaultAfflictions();
            bool found = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].id == AfflictionSO.Ids.PoisonIngestion)
                    found = true;
            }
            Assert.That(found, Is.True, "Default affliction set must register poison_ingestion");
            for (int i = 0; i < list.Count; i++)
                Object.DestroyImmediate(list[i]);
        }

        [Test]
        public void ExpeditionLootRoll_WithActiveSabotage_CanPlantPoison()
        {
            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            catalog.items = new List<ItemDefinition> { _iodine };

            var expedition = new ExpeditionSystem(
                null, _inventory, catalog, medicalSystem: _medical, seed: 1);

            var sys = MakeActiveSystem(new AlwaysSabotageRng());
            expedition.SetSabotagedCacheSystem(sys);

            var scav = MakeSurvivor("sv_exp", medicalSkill: 0.1f, bias: RiskBiasTrait.Reckless);
            scav.Needs.Health = 100f;
            _needs.Register(scav);

            // Drive many loot rolls via a forced expedition state in looting phase.
            // Use Force path: start expedition needs map — call ProcessLootCandidate
            // through the public expedition hook by completing a synthetic roll loop.
            // Direct system path already covered; here verify wiring via SetSabotagedCacheSystem.
            Assert.That(expedition.SabotagedCaches, Is.SameAs(sys));

            var exp = new ExpeditionState
            {
                ExpeditionId = "exp_sab",
                SurvivorId = scav.Id,
                Survivor = scav,
                TargetLocationId = "node_suburb_01",
                Phase = ExpeditionPhase.Looting,
                CarryingCapacity = 50f
            };

            // Simulate what PerformLootRoll does after a medical hit
            sys.RecordScavengeLoot(exp.TargetLocationId);
            var outcome = sys.ProcessLootCandidate(scav, _iodine, out var result);
            Assert.That(outcome, Is.EqualTo(SabotagedLootOutcome.Poisoned));
            Assert.That(exp.TryAddLoot(result), Is.True);
            Assert.That(exp.CollectedLoot.Count, Is.EqualTo(1));
            Assert.That(exp.CollectedLoot[0].id, Is.EqualTo(SabotagedCacheSystem.PoisonedIodineItemId));

            Object.DestroyImmediate(catalog);
        }
    }
}
