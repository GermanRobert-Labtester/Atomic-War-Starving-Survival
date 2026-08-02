using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class ShelterModuleTests
    {
        private const float Eps = 1e-4f;

        private RadiationShieldingModuleSO _shieldingSO;
        private AirFiltrationModuleSO _airFiltrationSO;
        private HeaterModuleSO _heaterSO;
        private WaterPurifierModuleSO _waterPurifierSO;

        [SetUp]
        public void SetUp()
        {
            _shieldingSO = ScriptableObject.CreateInstance<RadiationShieldingModuleSO>();
            _shieldingSO.ModuleId = "radiation_shielding";
            _shieldingSO.DisplayName = "Radiation Shielding";
            _shieldingSO.RadReductionPercentPerLevel = 0.20f; // 20% per level

            _airFiltrationSO = ScriptableObject.CreateInstance<AirFiltrationModuleSO>();
            _airFiltrationSO.ModuleId = "air_filtration";
            _airFiltrationSO.DisplayName = "Air Filtration";
            _airFiltrationSO.DegradationRatePerHour = 10f;
            _airFiltrationSO.LowHealthThreshold = 20f;
            _airFiltrationSO.RadLeakPerTickWhenDepleted = 8f;

            _heaterSO = ScriptableObject.CreateInstance<HeaterModuleSO>();
            _heaterSO.ModuleId = "heater";
            _heaterSO.DisplayName = "Bunker Heater";
            _heaterSO.HeatOutputPerLevel = 6f;
            _heaterSO.FuelConsumptionRatePerHour = 1f;

            _waterPurifierSO = ScriptableObject.CreateInstance<WaterPurifierModuleSO>();
            _waterPurifierSO.ModuleId = "water_purifier";
            _waterPurifierSO.DisplayName = "Water Purifier";
            _waterPurifierSO.ConversionHoursPerUnit = 2f;
        }

        [Test]
        public void AirFiltration_AtZero_AppliesRadLeak_AndDropsAirQuality()
        {
            var shelter = new Shelter();
            var airMod = new ShelterModuleInstance(_airFiltrationSO, level: 1)
            {
                FilterHealth = 0f
            };
            shelter.AddModule(airMod);

            Assert.That(shelter.AirQuality, Is.EqualTo(0f).Within(Eps));

            float exteriorRad = 50f;
            // Exterior 50, filter at 0 => leak of 8 applied => total 58 rads/hr
            float interiorRad = shelter.GetInteriorRadsPerHour(exteriorRad);
            Assert.That(interiorRad, Is.GreaterThan(exteriorRad));
            Assert.That(interiorRad, Is.EqualTo(58f).Within(Eps));
        }

        [Test]
        public void Heater_OffOrUnfueledInBlizzard_GivesNoTempBonus()
        {
            var shelter = new Shelter();
            var tempSystem = new TemperatureSystem();
            tempSystem.SetAmbient(-30f); // Blizzard outdoor temperature

            var heaterMod = new ShelterModuleInstance(_heaterSO, level: 2)
            {
                Fuel = 0f,
                IsEnabled = false
            };
            shelter.AddModule(heaterMod);

            Assert.That(shelter.IndoorTempBonus, Is.EqualTo(0f).Within(Eps));
            Assert.That(tempSystem.GetIndoorTemperature(shelter), Is.EqualTo(-30f).Within(Eps));

            // Enable heater and add fuel
            heaterMod.IsEnabled = true;
            heaterMod.AddFuel(5f);

            Assert.That(shelter.IndoorTempBonus, Is.EqualTo(12f).Within(Eps));
            Assert.That(tempSystem.GetIndoorTemperature(shelter), Is.EqualTo(-18f).Within(Eps));
        }

        [Test]
        public void Shelter_AddAndRemoveModulesRuntime_WithoutNullRefs()
        {
            var shelter = new Shelter();

            // Module queries on empty shelter return null / 0 defaults safely
            Assert.That(shelter.GetModule("non_existent"), Is.Null);
            Assert.That(shelter.AirQuality, Is.EqualTo(0f).Within(Eps));
            Assert.That(shelter.IndoorTempBonus, Is.EqualTo(0f).Within(Eps));
            Assert.DoesNotThrow(() => shelter.Tick(1f));
            Assert.DoesNotThrow(() => shelter.RemoveModule("non_existent"));

            // Add runtime modules
            var shieldingInst = new ShelterModuleInstance(_shieldingSO, level: 2);
            shelter.AddModule(shieldingInst);

            Assert.That(shelter.GetModule("radiation_shielding"), Is.EqualTo(shieldingInst));

            // Remove runtime module
            bool removed = shelter.RemoveModule("radiation_shielding");
            Assert.That(removed, Is.True);
            Assert.That(shelter.GetModule("radiation_shielding"), Is.Null);
            Assert.DoesNotThrow(() => shelter.GetInteriorRadsPerHour(100f));
        }

        [Test]
        public void RadiationShielding_ReducesAmbientIndoorRad_ByPercentPerLevel()
        {
            var shelter = new Shelter();
            var shieldingInst = new ShelterModuleInstance(_shieldingSO, level: 2); // 2 * 20% = 40% reduction
            shelter.AddModule(shieldingInst);

            // Add healthy air filtration so no leak is added
            var airMod = new ShelterModuleInstance(_airFiltrationSO, level: 1) { FilterHealth = 100f };
            shelter.AddModule(airMod);

            float exteriorRad = 100f;
            float interiorRad = shelter.GetInteriorRadsPerHour(exteriorRad);

            // 100 * (1 - 0.40) = 60
            Assert.That(interiorRad, Is.EqualTo(60f).Within(Eps));
        }

        [Test]
        public void RadiationSystem_ReadsShelterAggregate_WhenInExposureContext()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_shieldingSO, level: 2));
            shelter.AddModule(new ShelterModuleInstance(_airFiltrationSO, level: 1) { FilterHealth = 100f });

            var needsSystem = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
            var context = new ExposureContext
            {
                ZoneRadLevel = 100f,
                Shelter = shelter
            };

            var radSystem = new RadiationSystem(needsSystem, _ => context);
            var survivor = new Survivor { Id = "s1" };
            radSystem.Register(survivor);

            radSystem.Tick(1f);

            // Exterior 100 rads/hr attenuated by 40% = 60 rads accumulated
            Assert.That(survivor.RadiationDose, Is.EqualTo(60f).Within(Eps));
        }

        [Test]
        public void ShelterUpgrader_ConsumesMaterials_AndUpgradesModuleLevel()
        {
            var metalItem = ScriptableObject.CreateInstance<ItemDefinition>();
            metalItem.id = "scrap_metal";
            metalItem.displayName = "Scrap Metal";

            _shieldingSO.UpgradeRequirements.Clear();
            _shieldingSO.UpgradeRequirements.Add(new ModuleLevelRequirement
            {
                TargetLevel = 2,
                Materials = { new ModuleUpgradeCost { Item = metalItem, Amount = 3 } }
            });

            var shelter = new Shelter();
            var shieldMod = new ShelterModuleInstance(_shieldingSO, level: 1);
            shelter.AddModule(shieldMod);

            var inventory = new Inventory { Capacity = 10 };
            var upgrader = new ShelterUpgrader();

            // Insufficient materials
            Assert.That(upgrader.CanUpgrade(shieldMod, inventory), Is.False);
            Assert.That(upgrader.TryUpgrade(shieldMod, inventory, shelter), Is.False);
            Assert.That(shieldMod.Level, Is.EqualTo(1));

            // Add materials
            inventory.Add(metalItem, 5);
            Assert.That(upgrader.CanUpgrade(shieldMod, inventory), Is.True);

            bool success = upgrader.TryUpgrade(shieldMod, inventory, shelter);
            Assert.That(success, Is.True);
            Assert.That(shieldMod.Level, Is.EqualTo(2));
            Assert.That(inventory.Count(metalItem), Is.EqualTo(2)); // 5 - 3 = 2 remaining
        }
    }
}
