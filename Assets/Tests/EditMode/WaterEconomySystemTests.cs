using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class WaterEconomySystemTests
    {
        private const float Eps = 1e-4f;

        private CatchmentSurfaceModuleSO _catchmentSO;
        private WaterPurifierModuleSO _purifierSO;
        private WaterEconomySystem _system;

        [SetUp]
        public void SetUp()
        {
            _catchmentSO = ScriptableObject.CreateInstance<CatchmentSurfaceModuleSO>();
            _catchmentSO.ModuleId = "catchment_surface";
            _catchmentSO.DisplayName = "Roof Catchment";
            _catchmentSO.CollectionRatePerHour = 5f;

            _purifierSO = ScriptableObject.CreateInstance<WaterPurifierModuleSO>();
            _purifierSO.ModuleId = "water_purifier";
            _purifierSO.DisplayName = "Water Purifier";
            _purifierSO.ConversionHoursPerUnit = 2f;
            _purifierSO.FilterDegradationPerUnitConverted = 5f;

            _system = new WaterEconomySystem();
        }

        [Test]
        public void FalloutStorm_WithOpenCatchment_IncreasesIrradiatedWater()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = true });
            var storage = new WaterStorage();

            _system.Tick(1f, WeatherKind.FalloutStorm, currentDay: 10, shelter, storage);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(5f).Within(Eps));
            Assert.That(storage.CleanWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void FalloutStorm_WithClosedCatchment_CollectsNothing()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = false });
            var storage = new WaterStorage();

            _system.Tick(1f, WeatherKind.FalloutStorm, currentDay: 10, shelter, storage);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void Rain_PreDay30_CollectsCleanWater()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = true });
            var storage = new WaterStorage();

            _system.Tick(1f, WeatherKind.Rain, currentDay: 10, shelter, storage);

            Assert.That(storage.CleanWater, Is.EqualTo(5f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void Rain_PostDay30_CollectsDirtyWaterInstead()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = true });
            var storage = new WaterStorage();

            _system.Tick(1f, WeatherKind.Rain, currentDay: 35, shelter, storage);

            Assert.That(storage.DirtyWater, Is.EqualTo(5f).Within(Eps));
            Assert.That(storage.CleanWater, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void ClearWeather_CollectsNothing()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = true });
            var storage = new WaterStorage();

            _system.Tick(5f, WeatherKind.Clear, currentDay: 10, shelter, storage);

            Assert.That(storage.CleanWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.IrradiatedWater, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void Purifier_ConvertsIrradiatedToDirtyToClean_OverTime_ConsumingFilterDurability()
        {
            var shelter = new Shelter();
            var purifierInst = new ShelterModuleInstance(_purifierSO, level: 1)
            {
                IsEnabled = true,
                FilterHealth = 100f
            };
            shelter.AddModule(purifierInst);

            var storage = new WaterStorage();
            storage.AddIrradiated(3f);

            // ConversionHoursPerUnit = 2h: one 2-hour tick converts exactly one unit
            // one tier (irradiated -> dirty), consuming filter durability.
            _system.Tick(2f, WeatherKind.Clear, currentDay: 10, shelter, storage);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(2f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(1f).Within(Eps));
            Assert.That(purifierInst.FilterHealth, Is.EqualTo(95f).Within(Eps));

            // Run long enough to fully cascade every unit down to clean water.
            _system.Tick(20f, WeatherKind.Clear, currentDay: 10, shelter, storage);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.CleanWater, Is.EqualTo(3f).Within(Eps));
            Assert.That(purifierInst.FilterHealth, Is.LessThan(95f));
        }

        [Test]
        public void Purifier_EmitsOnWaterPurified_WhenDirtyConvertedToClean()
        {
            var shelter = new Shelter();
            var purifierInst = new ShelterModuleInstance(_purifierSO, level: 1)
            {
                IsEnabled = true,
                FilterHealth = 100f
            };
            shelter.AddModule(purifierInst);

            var storage = new WaterStorage();
            storage.AddDirty(3f);

            var survivors = new List<Survivor> { new Survivor { Id = "sv_1", DisplayName = "Test" } };
            _system.BindPersonalQuests(null, () => survivors);

            float capturedVolume = 0f;
            IReadOnlyList<Survivor> capturedSurvivors = null;
            _system.OnWaterPurified += (volume, sv) =>
            {
                capturedVolume = volume;
                capturedSurvivors = sv;
            };

            // ConversionHoursPerUnit = 2h: 3 dirty units need 6 hours.
            _system.Tick(7f, WeatherKind.Clear, currentDay: 10, shelter, storage);

            Assert.That(storage.DirtyWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.CleanWater, Is.EqualTo(3f).Within(Eps));
            Assert.That(capturedVolume, Is.EqualTo(3f).Within(Eps));
            Assert.That(capturedSurvivors, Is.SameAs(survivors));
        }

        [Test]
        public void Purifier_WithoutPower_DoesNotConvert()
        {
            var shelter = new Shelter();
            var purifierInst = new ShelterModuleInstance(_purifierSO, level: 1)
            {
                IsEnabled = false, // unpowered
                FilterHealth = 100f
            };
            shelter.AddModule(purifierInst);

            var storage = new WaterStorage();
            storage.AddIrradiated(3f);

            _system.Tick(10f, WeatherKind.Clear, currentDay: 10, shelter, storage);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(3f).Within(Eps));
        }

        [Test]
        public void Purifier_WithDepletedFilter_StopsConverting()
        {
            var shelter = new Shelter();
            var purifierInst = new ShelterModuleInstance(_purifierSO, level: 1)
            {
                IsEnabled = true,
                FilterHealth = 0f
            };
            shelter.AddModule(purifierInst);

            var storage = new WaterStorage();
            storage.AddIrradiated(3f);

            _system.Tick(10f, WeatherKind.Clear, currentDay: 10, shelter, storage);

            Assert.That(storage.IrradiatedWater, Is.EqualTo(3f).Within(Eps));
        }

        [Test]
        public void PurifierSnapshot_ReportsFilterServiceForecastAtCurrentBurn()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_purifierSO, level: 1)
            {
                IsEnabled = true,
                FilterHealth = 20f
            });
            var storage = new WaterStorage { IrradiatedWater = 3f };

            var active = _system.GetSnapshot(shelter, storage);
            Assert.That(active.FilterDegradationPerUnit, Is.EqualTo(5f));
            Assert.That(active.FilterBurnPerHour, Is.EqualTo(2.5f).Within(Eps));
            Assert.That(active.FilterRuntimeHours, Is.EqualTo(8f).Within(Eps));

            storage.IrradiatedWater = 0f;
            var idle = _system.GetSnapshot(shelter, storage);
            Assert.That(idle.FilterBurnPerHour, Is.EqualTo(0f));
            Assert.That(idle.FilterRuntimeHours, Is.EqualTo(-1f));
        }
    }
}
