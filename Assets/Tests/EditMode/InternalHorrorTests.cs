using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Internal Horror: corpse rot, coma/caregive, fire/O2/CO, humidity→ContaminatedFood+botulism.
    /// </summary>
    [TestFixture]
    public class InternalHorrorTests
    {
        private const float Eps = 1e-3f;

        private NeedsProfile _profile;
        private NeedsSystem _needs;
        private List<AfflictionSO> _defs;
        private List<Object> _toDestroy;

        [SetUp]
        public void SetUp()
        {
            _toDestroy = new List<Object>();
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(_profile);
            _profile.hungerPerHour = 0f;
            _profile.thirstPerHour = 0f;
            _profile.fatiguePerHour = 0f;
            _profile.warmthLossPerHourInCold = 0f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 0f;
            _profile.moraleLossPerHourWhileCritical = 0f;
            _needs = new NeedsSystem(_profile);
            _defs = MedicalSystem.CreateDefaultAfflictions();
            for (int i = 0; i < _defs.Count; i++)
                _toDestroy.Add(_defs[i]);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
        }

        private MedicalSystem MakeMedical(Inventory inv = null)
        {
            var med = new MedicalSystem(_needs, inv);
            for (int i = 0; i < _defs.Count; i++)
                med.RegisterAffliction(_defs[i]);
            return med;
        }

        private static Survivor MakeSurvivor(string id = "sv_a")
        {
            return new Survivor { Id = id, DisplayName = id, MedicalSkill = 0.5f };
        }

        private ItemDefinition Track(ItemDefinition item)
        {
            _toDestroy.Add(item);
            return item;
        }

        // ─────────────────────────────────────────────────────────────────
        // Phase A — Atmosphere / fire
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void PowerNetwork_AddCarbonMonoxide_RaisesLevel()
        {
            var net = PowerNetwork.CreateDefault(dieselFuel: 10f);
            float before = net.CarbonMonoxidePpm;
            net.AddCarbonMonoxide(25f);
            Assert.That(net.CarbonMonoxidePpm, Is.EqualTo(before + 25f).Within(Eps));
        }

        [Test]
        public void Fire_ConsumesOxygen_AndSpikesLocalCo()
        {
            var atm = new ShelterAtmosphereSystem(new System.Random(1));
            var room = new ShelterRoom("plant", null);
            atm.RegisterRoom(room);
            atm.StartFire(room, intensity: 1f);

            float o2Before = room.OxygenFraction;
            float coBefore = room.LocalCoPpm;
            atm.Tick(1f, power: null, shelter: null);

            Assert.That(room.OxygenFraction, Is.LessThan(o2Before));
            Assert.That(room.LocalCoPpm, Is.GreaterThan(coBefore));
            Assert.IsTrue(room.IsOnFire);
        }

        [Test]
        public void Fire_AddsCoToPowerNetwork()
        {
            var atm = new ShelterAtmosphereSystem(new System.Random(2));
            var room = new ShelterRoom("plant", null);
            atm.RegisterRoom(room);
            atm.StartFire(room, intensity: 1f);

            var net = PowerNetwork.CreateDefault(0f);
            float before = net.CarbonMonoxidePpm;
            atm.Tick(1f, net, null);
            Assert.That(net.CarbonMonoxidePpm, Is.GreaterThan(before));
        }

        [Test]
        public void SealBulkhead_StarvesFire_WhenO2Depleted()
        {
            var atm = new ShelterAtmosphereSystem(new System.Random(3));
            var room = new ShelterRoom("plant", null);
            atm.RegisterRoom(room);
            atm.StartFire(room, intensity: 0.8f);

            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("grow_light", 1) { RoomId = "plant" });

            Assert.IsTrue(atm.SealBulkhead("plant", shelter));
            Assert.IsTrue(room.BulkheadSealed);
            Assert.IsTrue(room.ModulesSacrificed);
            Assert.IsFalse(shelter.GetModule("grow_light").IsEnabled);

            // Drive O2 below extinguish threshold
            room.OxygenFraction = ShelterAtmosphereSystem.FireExtinguishO2Threshold;
            atm.Tick(0.5f, null, shelter);
            Assert.IsFalse(room.IsOnFire, "Sealed room with low O2 must extinguish the fire");
        }

        [Test]
        public void FightFire_DamagesFighter_AndCanExtinguish()
        {
            var atm = new ShelterAtmosphereSystem(new System.Random(4));
            var room = new ShelterRoom("plant", null);
            atm.RegisterRoom(room);
            atm.StartFire(room, intensity: 0.4f);

            var fighter = MakeSurvivor("sv_fighter");
            fighter.Needs.Health = 100f;
            bool out_ = atm.FightFire("plant", fighter, _needs);
            Assert.That(fighter.Needs.Health, Is.LessThan(100f));
            Assert.IsTrue(out_ || room.FireIntensity < 0.4f);
        }

        [Test]
        public void LowDurabilityDiesel_CanIgniteRoom()
        {
            // Deterministic: force many hours with durability 0 and chance forced via seed
            var atm = new ShelterAtmosphereSystem(new System.Random(0));
            var plant = new ShelterRoom("plant", null);
            atm.RegisterRoom(plant);

            var diesel = PowerSourceSO.CreateDieselGenerator();
            _toDestroy.Add(diesel);
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(diesel);
            var src = new PowerSourceInstance(diesel, 50f)
            {
                Durability = 0f,
                RoomId = "plant",
                IsEnabled = true
            };
            net.AddSource(src);

            bool ignited = false;
            for (int i = 0; i < 80 && !ignited; i++)
            {
                atm.Tick(1f, net, null);
                ignited = plant.IsOnFire;
            }
            Assert.IsTrue(ignited, "Zero-durability diesel should eventually spark a room fire");
        }

        [Test]
        public void Atmosphere_RoundTripsThroughSave()
        {
            var atm = new ShelterAtmosphereSystem(new System.Random(5));
            var room = new ShelterRoom("stores", null)
            {
                Humidity = 0.8f,
                OxygenFraction = 0.15f,
                LocalCoPpm = 40f,
                IsOnFire = true,
                FireIntensity = 0.6f
            };
            atm.RegisterRoom(room);

            var save = atm.CaptureState();
            var atm2 = new ShelterAtmosphereSystem(new System.Random(5));
            atm2.RestoreState(save);
            var restored = atm2.GetRoom("stores");
            Assert.IsNotNull(restored);
            Assert.That(restored.Humidity, Is.EqualTo(0.8f).Within(Eps));
            Assert.That(restored.OxygenFraction, Is.EqualTo(0.15f).Within(Eps));
            Assert.IsTrue(restored.IsOnFire);
        }

        // ─────────────────────────────────────────────────────────────────
        // Phase B — Corpses
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Death_CreatesCorpseItem_InInventory()
        {
            var inv = new Inventory { Capacity = 30 };
            var med = MakeMedical(inv);
            var corpses = new CorpseManagementSystem(_needs, inv, med);
            var corpseDef = Track(CorpseManagementSystem.CreateCorpseDefinition());
            corpses.SetItemDefinitions(corpseDef, Track(CorpseManagementSystem.CreateFertilizerDefinition()));
            corpses.BindDeathHandler();

            var sv = MakeSurvivor("sv_dead");
            sv.Needs.Health = 1f;
            _needs.Modify(sv, NeedKind.Health, -10f); // triggers death

            Assert.That(sv.State, Is.EqualTo(SurvivorState.Dead));
            Assert.That(inv.CountByType(ItemType.Corpse), Is.EqualTo(1));
            Assert.That(corpses.CorpseCount, Is.EqualTo(1));
        }

        [Test]
        public void Corpse_InBunker_DrainsMorale_AndGrowsMold()
        {
            var inv = new Inventory { Capacity = 30 };
            var med = MakeMedical(inv);
            var room = new ShelterRoom("stores", null);
            var corpses = new CorpseManagementSystem(_needs, inv, med, rng: new System.Random(9));
            var corpseDef = Track(CorpseManagementSystem.CreateCorpseDefinition());
            corpses.SetItemDefinitions(corpseDef, Track(CorpseManagementSystem.CreateFertilizerDefinition()));
            corpses.SetStoresRoom(room);

            inv.Add(corpseDef, 1);
            var living = MakeSurvivor("sv_living");
            living.Needs.Morale = 80f;
            corpses.SetSurvivorProvider(() => new List<Survivor> { living });

            corpses.Tick(2f, new List<Survivor> { living });

            Assert.That(living.Needs.Morale, Is.LessThan(80f));
            Assert.IsTrue(room.HasMold);
            Assert.That(room.MoldLevel, Is.GreaterThan(0f));
        }

        [Test]
        public void BuryTheDead_RemovesCorpse_AndExposesDigger()
        {
            var inv = new Inventory { Capacity = 30 };
            var corpses = new CorpseManagementSystem(_needs, inv, rng: new System.Random(10));
            var corpseDef = Track(CorpseManagementSystem.CreateCorpseDefinition());
            corpses.SetItemDefinitions(corpseDef, Track(CorpseManagementSystem.CreateFertilizerDefinition()));
            inv.Add(corpseDef, 1);

            var digger = MakeSurvivor("sv_digger");
            digger.Needs.Fatigue = 10f;
            digger.Needs.Morale = 50f;
            corpses.SetSurvivorProvider(() => new List<Survivor> { digger });

            Assert.IsTrue(corpses.BuryTheDead(digger));
            Assert.That(corpses.CorpseCount, Is.EqualTo(0));
            Assert.That(digger.Needs.Fatigue, Is.GreaterThan(10f));
        }

        [Test]
        public void ProcessForFertilizer_AddsMaterial_AndShattersMorale()
        {
            var inv = new Inventory { Capacity = 30 };
            var corpses = new CorpseManagementSystem(_needs, inv, rng: new System.Random(11));
            var corpseDef = Track(CorpseManagementSystem.CreateCorpseDefinition());
            var fertDef = Track(CorpseManagementSystem.CreateFertilizerDefinition());
            corpses.SetItemDefinitions(corpseDef, fertDef);
            inv.Add(corpseDef, 1);

            var processor = MakeSurvivor("sv_proc");
            processor.Needs.Morale = 70f;
            corpses.SetSurvivorProvider(() => new List<Survivor> { processor });

            Assert.IsTrue(corpses.ProcessForFertilizer(processor));
            Assert.That(corpses.CorpseCount, Is.EqualTo(0));
            Assert.That(inv.CountById(CorpseManagementSystem.FertilizerItemId), Is.GreaterThan(0));
            Assert.That(processor.Needs.Morale, Is.LessThan(70f - 20f));
        }

        [Test]
        public void CorpseManagement_RoundTripsSourceIdsThroughSave()
        {
            var inv = new Inventory { Capacity = 30 };
            var med = MakeMedical(inv);
            var corpses = new CorpseManagementSystem(_needs, inv, med);
            var corpseDef = Track(CorpseManagementSystem.CreateCorpseDefinition());
            corpses.SetItemDefinitions(corpseDef, Track(CorpseManagementSystem.CreateFertilizerDefinition()));
            corpses.BindDeathHandler();

            var sv = MakeSurvivor("sv_save_body");
            sv.Needs.Health = 1f;
            _needs.Modify(sv, NeedKind.Health, -10f);
            Assert.That(corpses.CorpseCount, Is.EqualTo(1));

            var save = corpses.CaptureState();
            Assert.IsNotNull(save);
            Assert.IsNotNull(save.CorpseSourceIds);
            Assert.That(save.CorpseSourceIds.Length, Is.EqualTo(1));
            Assert.That(save.CorpseSourceIds[0], Is.EqualTo("sv_save_body"));

            // Fresh system + inventory that still holds the body stack (inventory
            // is saved separately; source-id metadata must round-trip alone).
            var inv2 = new Inventory { Capacity = 30 };
            inv2.Add(corpseDef, 1);
            var corpses2 = new CorpseManagementSystem(_needs, inv2, med);
            corpses2.SetItemDefinitions(corpseDef, Track(CorpseManagementSystem.CreateFertilizerDefinition()));
            corpses2.RestoreState(save);

            var save2 = corpses2.CaptureState();
            Assert.That(save2.CorpseSourceIds.Length, Is.EqualTo(1));
            Assert.That(save2.CorpseSourceIds[0], Is.EqualTo("sv_save_body"));
            Assert.That(corpses2.CorpseCount, Is.EqualTo(1));
        }

        // ─────────────────────────────────────────────────────────────────
        // Phase C — Coma / Caregive
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void Coma_SetsIncapacitated_AndBlocksSelfFeed()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor("sv_coma");
            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.Coma));
            Assert.IsTrue(med.IsComatose(patient));
            Assert.That(patient.State, Is.EqualTo(SurvivorState.Incapacitated));

            var eat = ScriptableObject.CreateInstance<EatActionSO>();
            _toDestroy.Add(eat);
            var ctx = new AIContext
            {
                Survivor = patient,
                MedicalSystem = med
            };
            Assert.That(eat.EvaluateRaw(ctx), Is.EqualTo(0f));
        }

        [Test]
        public void Caregive_ResetsNeglectClock_AndConsumesWater()
        {
            var inv = new Inventory { Capacity = 30 };
            var water = Track(ScriptableObject.CreateInstance<ItemDefinition>());
            water.id = "clean_water";
            water.type = ItemType.Water;
            water.stackMax = 20;
            inv.Add(water, 5);

            var med = MakeMedical(inv);
            var patient = MakeSurvivor("sv_coma");
            var carer = MakeSurvivor("sv_carer");
            med.Inflict(patient, AfflictionSO.Ids.Coma);

            // Age the neglect clock
            var active = med.GetActive(patient)[0];
            active.HoursSinceLastCare = 5f;

            Assert.IsTrue(med.TryCaregive(carer, patient));
            Assert.That(med.GetActive(patient)[0].HoursSinceLastCare, Is.EqualTo(0f).Within(Eps));
            Assert.That(inv.Count(water), Is.EqualTo(4));
        }

        [Test]
        public void Coma_WithoutCare_AcceleratesHealthDrain()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor("sv_coma");
            patient.Needs.Health = 80f;
            med.Inflict(patient, AfflictionSO.Ids.Coma);

            // Just under care interval: mild drain only
            med.Tick(new List<Survivor> { patient }, MedicalSystem.ComaCareIntervalHours - 0.5f);
            float mid = patient.Needs.Health;

            // Cross care interval with neglect
            med.Tick(new List<Survivor> { patient }, 2f);
            Assert.That(patient.Needs.Health, Is.LessThan(mid - 5f),
                "Neglect past care interval must accelerate health collapse");
        }

        [Test]
        public void CaregiveAction_ScoresHigh_WhenPatientNeedsCare()
        {
            var inv = new Inventory { Capacity = 30 };
            var water = Track(ScriptableObject.CreateInstance<ItemDefinition>());
            water.id = "clean_water";
            water.type = ItemType.Water;
            water.stackMax = 10;
            inv.Add(water, 3);

            var med = MakeMedical(inv);
            var patient = MakeSurvivor("sv_coma");
            var carer = MakeSurvivor("sv_carer");
            med.Inflict(patient, AfflictionSO.Ids.Coma);
            med.GetActive(patient)[0].HoursSinceLastCare = 5.5f;

            var action = ScriptableObject.CreateInstance<CaregiveActionSO>();
            _toDestroy.Add(action);
            var ctx = new AIContext
            {
                Survivor = carer,
                MedicalSystem = med,
                Inventory = inv,
                GetSurvivors = () => new List<Survivor> { carer, patient }
            };
            float score = action.EvaluateRaw(ctx);
            Assert.That(score, Is.GreaterThan(0.5f));

            action.Execute(ctx);
            Assert.That(med.GetActive(patient)[0].HoursSinceLastCare, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void Sepsis_ProgressesToComa()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();
            // Sepsis drain is lethal over the full 12h window; jump to the
            // progression edge so the cascade (not the kill) is under test.
            patient.Needs.Health = 100f;
            med.Inflict(patient, AfflictionSO.Ids.Sepsis);
            med.GetActive(patient)[0].HoursUntilProgression = 0.1f;
            med.Tick(new List<Survivor> { patient }, 0.2f);
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis));
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.Coma));
        }

        // ─────────────────────────────────────────────────────────────────
        // Phase E — Pantry rust / botulism
        // ─────────────────────────────────────────────────────────────────

        [Test]
        public void HighHumidity_RustsFood_IntoContaminatedFood()
        {
            var inv = new Inventory { Capacity = 40 };
            var food = Track(ScriptableObject.CreateInstance<ItemDefinition>());
            food.id = "canned_food";
            food.type = ItemType.Food;
            food.stackMax = 50;
            food.hungerRestore = 40f;
            inv.Add(food, 20);

            var contam = Track(PantryContaminationSystem.CreateContaminatedFoodDefinition());
            var pantry = new PantryContaminationSystem(inv, new System.Random(1));
            pantry.SetContaminatedFoodDefinition(contam);

            var room = new ShelterRoom("stores", null) { Humidity = 0.9f };
            // Force high conversion: many hours
            pantry.Tick(40f, room);

            Assert.That(inv.CountByType(ItemType.ContaminatedFood), Is.GreaterThan(0),
                "High humidity must rust some canned food");
            Assert.That(inv.CountByType(ItemType.Food), Is.LessThan(20));
        }

        [Test]
        public void LowHumidity_DoesNotRustFood()
        {
            var inv = new Inventory { Capacity = 40 };
            var food = Track(ScriptableObject.CreateInstance<ItemDefinition>());
            food.id = "canned_food";
            food.type = ItemType.Food;
            food.stackMax = 50;
            inv.Add(food, 10);

            var contam = Track(PantryContaminationSystem.CreateContaminatedFoodDefinition());
            var pantry = new PantryContaminationSystem(inv, new System.Random(2));
            pantry.SetContaminatedFoodDefinition(contam);

            var room = new ShelterRoom("stores", null) { Humidity = 0.3f };
            pantry.Tick(40f, room);

            Assert.That(inv.CountByType(ItemType.ContaminatedFood), Is.EqualTo(0));
            Assert.That(inv.CountByType(ItemType.Food), Is.EqualTo(10));
        }

        [Test]
        public void EatingContaminatedFood_CanInflictBotulism()
        {
            var inv = new Inventory { Capacity = 20 };
            var contam = Track(PantryContaminationSystem.CreateContaminatedFoodDefinition());
            inv.Add(contam, 5);

            var med = MakeMedical(inv);
            var eater = MakeSurvivor("sv_eater");
            eater.Needs.Hunger = 80f;

            // Force botulism via deterministic RNG seed that rolls low
            var pantry = new PantryContaminationSystem(inv, new System.Random(0));
            pantry.SetContaminatedFoodDefinition(contam);

            bool gotBotulism = false;
            for (int i = 0; i < 20 && !gotBotulism; i++)
            {
                if (inv.Count(contam) < 1)
                    inv.Add(contam, 5);
                pantry.ConsumeFood(contam, eater, _needs, med);
                gotBotulism = med.HasAffliction(eater, AfflictionSO.Ids.Botulism);
            }
            Assert.IsTrue(gotBotulism, "Repeated contaminated meals must eventually cause botulism");
        }

        [Test]
        public void Botulism_ProgressesToComa()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();
            patient.Needs.Health = 100f;
            med.Inflict(patient, AfflictionSO.Ids.Botulism);
            med.Tick(new List<Survivor> { patient }, 18.1f);
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Botulism));
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.Coma));
        }
    }
}
