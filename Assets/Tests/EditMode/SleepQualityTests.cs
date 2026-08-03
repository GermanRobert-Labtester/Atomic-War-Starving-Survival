using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Environmental sleep quality (Prompt #32): bed comfort, temperature, diesel
    /// noise, foul air. Acceptance: freezing room + running generator → 30% fatigue
    /// restored and a Morale debuff.
    /// </summary>
    [TestFixture]
    public class SleepQualityTests
    {
        private const float Eps = 1e-3f;
        private readonly List<Object> _toDestroy = new List<Object>();
        private PowerSourceSO _diesel;
        private BedModuleSO _bedDef;
        private SleepActionSO _sleepAction;

        [SetUp]
        public void SetUp()
        {
            _diesel = PowerSourceSO.CreateDieselGenerator(50f);
            _toDestroy.Add(_diesel);
            _bedDef = BedModuleSO.CreateDefault(comfort: 1f, capacity: 1);
            _toDestroy.Add(_bedDef);
            _sleepAction = ScriptableObject.CreateInstance<SleepActionSO>();
            _toDestroy.Add(_sleepAction);
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

        private static Survivor MakeTiredSurvivor(float fatigue = 100f, float morale = 70f, float health = 100f)
        {
            var sv = new Survivor { Id = "s_sleep", DisplayName = "Sleeper" };
            sv.Needs.Fatigue = fatigue;
            sv.Needs.Morale = morale;
            sv.Needs.Health = health;
            return sv;
        }

        private PowerNetwork MakeRunningDiesel(string roomId)
        {
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(_diesel);
            var src = new PowerSourceInstance(_diesel, initialFuel: 50f)
            {
                RoomId = roomId,
                IsEnabled = true
            };
            net.AddSource(src);
            net.Rebalance();
            return net;
        }

        [Test]
        public void Acceptance_FreezingRoom_RunningGenerator_Restores30PercentFatigue_AndMoraleDebuff()
        {
            // Bed in quarters, diesel running in adjacent plant, indoor temp freezing.
            var shelter = new Shelter();
            var bed = new ShelterModuleInstance(_bedDef, 1)
            {
                RoomId = SleepQualitySystem.DefaultSleepRoomId
            };
            shelter.AddModule(bed);
            shelter.SetRoomsAdjacent(
                SleepQualitySystem.DefaultSleepRoomId,
                SleepQualitySystem.DefaultGeneratorRoomId);

            // Air filtration healthy so atmosphere is not the variable under test.
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });

            var power = MakeRunningDiesel(SleepQualitySystem.DefaultGeneratorRoomId);
            Assert.That(SleepQualitySystem.IsDieselGeneratorRunning(power), Is.True);

            var survivor = MakeTiredSurvivor(fatigue: 100f, morale: 70f);
            float moraleBefore = survivor.Needs.Morale;

            SleepQualitySystem.ResetBedOccupancy(shelter);
            var conditions = SleepQualitySystem.BuildConditions(
                shelter,
                power,
                indoorTemperatureC: SleepQualitySystem.FreezingTempC,
                preferredSleepRoomId: SleepQualitySystem.DefaultSleepRoomId);

            Assert.That(conditions.HasBed, Is.True, "Survivor should claim the bed");
            Assert.That(conditions.DieselNoiseAdjacent, Is.True, "Generator in adjacent plant must make noise");
            Assert.That(conditions.IndoorTemperatureC, Is.EqualTo(SleepQualitySystem.FreezingTempC).Within(Eps));

            var result = SleepQualitySystem.Evaluate(conditions);

            Assert.That(result.Quality, Is.EqualTo(0.3f).Within(Eps),
                "Freezing temp factor 0.3; diesel cap 0.5 does not raise it");
            Assert.That(result.FatigueRestored,
                Is.EqualTo(SleepQualitySystem.BaseFatigueRecovery * 0.3f).Within(Eps),
                "Only 30% of base fatigue recovery");
            Assert.That(result.Freezing, Is.True);
            Assert.That(result.NoiseCapped, Is.True);
            Assert.That(result.MoraleDelta, Is.LessThan(0f), "Poor sleep applies a Morale debuff");

            SleepActionSO.ApplySleepResult(survivor, result);

            Assert.That(survivor.Needs.Fatigue,
                Is.EqualTo(100f - SleepQualitySystem.BaseFatigueRecovery * 0.3f).Within(Eps));
            Assert.That(survivor.Needs.Morale, Is.LessThan(moraleBefore));
            Assert.That(survivor.State, Is.EqualTo(SurvivorState.Resting));
        }

        [Test]
        public void IdealConditions_FullBed_QuietWarm_FullRecovery()
        {
            var conditions = new SleepConditions
            {
                IndoorTemperatureC = 18f,
                AirQuality = 100f,
                CarbonMonoxidePpm = 0f,
                DieselNoiseAdjacent = false,
                HasBed = true,
                ComfortLevel = 1f
            };

            var result = SleepQualitySystem.Evaluate(conditions);
            Assert.That(result.Quality, Is.EqualTo(1f).Within(Eps));
            Assert.That(result.FatigueRestored,
                Is.EqualTo(SleepQualitySystem.BaseFatigueRecovery).Within(Eps));
            Assert.That(result.MoraleDelta, Is.GreaterThanOrEqualTo(0f));
            Assert.That(result.HealthDelta, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void FloorSleep_HalvesFatigueRecovery_AndMoraleHit()
        {
            var conditions = new SleepConditions
            {
                IndoorTemperatureC = 18f,
                AirQuality = 100f,
                DieselNoiseAdjacent = false,
                HasBed = false,
                ComfortLevel = 0f
            };

            var result = SleepQualitySystem.Evaluate(conditions);
            Assert.That(result.SleptOnFloor, Is.True);
            Assert.That(result.FatigueRestored,
                Is.EqualTo(SleepQualitySystem.BaseFatigueRecovery * SleepQualitySystem.FloorRecoveryMultiplier)
                    .Within(Eps));
            Assert.That(result.MoraleDelta, Is.LessThan(0f));
        }

        [Test]
        public void DieselNoise_CapsQualityAt50Percent()
        {
            var conditions = new SleepConditions
            {
                IndoorTemperatureC = 18f,
                AirQuality = 100f,
                DieselNoiseAdjacent = true,
                HasBed = true,
                ComfortLevel = 1f
            };

            var result = SleepQualitySystem.Evaluate(conditions);
            Assert.That(result.Quality, Is.EqualTo(SleepQualitySystem.NoiseQualityCap).Within(Eps));
            Assert.That(result.NoiseCapped, Is.True);
            Assert.That(result.FatigueRestored,
                Is.EqualTo(SleepQualitySystem.BaseFatigueRecovery * 0.5f).Within(Eps));
        }

        [Test]
        public void HighCo2_AppliesHealthHeadache()
        {
            var conditions = new SleepConditions
            {
                IndoorTemperatureC = 18f,
                AirQuality = 10f, // foul
                CarbonMonoxidePpm = 0f,
                DieselNoiseAdjacent = false,
                HasBed = true,
                ComfortLevel = 1f
            };

            var result = SleepQualitySystem.Evaluate(conditions);
            Assert.That(result.AtmosphereHeadache, Is.True);
            Assert.That(result.HealthDelta, Is.EqualTo(SleepQualitySystem.AtmosphereHealthPenalty).Within(Eps));

            var survivor = MakeTiredSurvivor(health: 90f);
            SleepActionSO.ApplySleepResult(survivor, result);
            Assert.That(survivor.Needs.Health,
                Is.EqualTo(90f + SleepQualitySystem.AtmosphereHealthPenalty).Within(Eps));
        }

        [Test]
        public void HighCarbonMonoxide_AlsoTriggersHeadache()
        {
            var conditions = new SleepConditions
            {
                IndoorTemperatureC = 18f,
                AirQuality = 100f,
                CarbonMonoxidePpm = SleepQualitySystem.HighCo2PpmThreshold,
                DieselNoiseAdjacent = false,
                HasBed = true,
                ComfortLevel = 1f
            };

            var result = SleepQualitySystem.Evaluate(conditions);
            Assert.That(result.AtmosphereHeadache, Is.True);
            Assert.That(result.HealthDelta, Is.LessThan(0f));
        }

        [Test]
        public void BedCapacity_SecondSleeperGoesToFloor()
        {
            var shelter = new Shelter();
            var bedSO = BedModuleSO.CreateDefault(comfort: 1f, capacity: 1);
            _toDestroy.Add(bedSO);
            shelter.AddModule(new ShelterModuleInstance(bedSO, 1)
            {
                RoomId = SleepQualitySystem.DefaultSleepRoomId
            });

            SleepQualitySystem.ResetBedOccupancy(shelter);
            var first = SleepQualitySystem.BuildConditions(shelter, null, 18f);
            var second = SleepQualitySystem.BuildConditions(shelter, null, 18f);

            Assert.That(first.HasBed, Is.True);
            Assert.That(second.HasBed, Is.False, "Capacity 1: second sleeper on floor");
        }

        [Test]
        public void SleepAction_Execute_UsesEnvironmentalQuality()
        {
            var survivor = MakeTiredSurvivor(100f, 70f);
            var action = _sleepAction;

            var context = new AIContext(survivor)
            {
                SleepConditionsOverride = new SleepConditions
                {
                    IndoorTemperatureC = SleepQualitySystem.FreezingTempC,
                    AirQuality = 100f,
                    DieselNoiseAdjacent = true,
                    HasBed = true,
                    ComfortLevel = 1f
                }
            };

            action.Execute(context);

            float expectedFatigue = 100f - SleepQualitySystem.BaseFatigueRecovery * 0.3f;
            Assert.That(survivor.Needs.Fatigue, Is.EqualTo(expectedFatigue).Within(Eps));
            Assert.That(survivor.Needs.Morale, Is.LessThan(70f));
        }

        [Test]
        public void TemperatureMultiplier_RampsFromFreezingToIdeal()
        {
            Assert.That(SleepQualitySystem.TemperatureMultiplier(SleepQualitySystem.FreezingTempC),
                Is.EqualTo(SleepQualitySystem.FreezingTempMultiplier).Within(Eps));
            Assert.That(SleepQualitySystem.TemperatureMultiplier(SleepQualitySystem.IdealTempMinC),
                Is.EqualTo(1f).Within(Eps));
            Assert.That(SleepQualitySystem.TemperatureMultiplier(18f), Is.EqualTo(1f).Within(Eps));
            float mid = SleepQualitySystem.TemperatureMultiplier(6f);
            Assert.That(mid, Is.GreaterThan(SleepQualitySystem.FreezingTempMultiplier));
            Assert.That(mid, Is.LessThan(1f));
        }

        [Test]
        public void AdjacentRooms_OnlyNoiseWhenGeneratorNextDoor()
        {
            var shelter = new Shelter();
            shelter.SetRoomsAdjacent("quarters", "plant");
            shelter.AddModule(new ShelterModuleInstance(_bedDef, 1) { RoomId = "quarters" });
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });

            var powerNear = MakeRunningDiesel("plant");
            var powerFar = MakeRunningDiesel("roof");

            SleepQualitySystem.ResetBedOccupancy(shelter);
            var near = SleepQualitySystem.BuildConditions(shelter, powerNear, 18f, "quarters");
            SleepQualitySystem.ResetBedOccupancy(shelter);
            var far = SleepQualitySystem.BuildConditions(shelter, powerFar, 18f, "quarters");

            Assert.That(near.DieselNoiseAdjacent, Is.True);
            Assert.That(far.DieselNoiseAdjacent, Is.False,
                "Generator on roof (not adjacent) should not cap sleep");
        }
    }
}
