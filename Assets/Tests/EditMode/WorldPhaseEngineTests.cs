using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// World Phase Engine: Day-30 Flashpoint transition, EMP electronics kill,
    /// phase-gated loot, and phase-restricted weather rolls.
    /// </summary>
    [TestFixture]
    public class WorldPhaseEngineTests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void Day29To30Transition_FiresNuclearExchangeOnce_AndAdvancesPhaseAcrossDay31()
        {
            var phaseSystem = new WorldPhaseSystem();
            int exchangeCount = 0;
            phaseSystem.OnNuclearExchange += () => exchangeCount++;

            phaseSystem.OnDayTick(29);
            Assert.That(phaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.CivilWar));
            Assert.That(exchangeCount, Is.EqualTo(0));

            phaseSystem.OnDayTick(30);
            Assert.That(phaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.Flashpoint));
            Assert.That(exchangeCount, Is.EqualTo(1));

            phaseSystem.OnDayTick(31);
            Assert.That(phaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.NuclearWinter));
            Assert.That(exchangeCount, Is.EqualTo(1), "Exchange must not refire on later days");
        }

        [Test]
        public void FlashpointDay_IsDesignerTunable_ViaConfig()
        {
            var config = ScriptableObject.CreateInstance<WorldPhaseConfigSO>();
            config.flashpointDay = 15;
            var phaseSystem = new WorldPhaseSystem(config);

            phaseSystem.OnDayTick(14);
            Assert.That(phaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.CivilWar));

            phaseSystem.OnDayTick(15);
            Assert.That(phaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.Flashpoint));

            Object.DestroyImmediate(config);
        }

        [Test]
        public void EMPEvent_BreaksUnshieldedElectronics_SparesShielded()
        {
            var inventory = new Inventory { Capacity = 10, MaxWeight = 50f };

            var geiger = ScriptableObject.CreateInstance<ItemDefinition>();
            geiger.id = "geiger_counter";
            geiger.type = ItemType.Device;
            geiger.empShielded = false;
            geiger.stackMax = 1;

            var hardenedDevice = ScriptableObject.CreateInstance<ItemDefinition>();
            hardenedDevice.id = "hardened_dosimeter";
            hardenedDevice.type = ItemType.Device;
            hardenedDevice.empShielded = true;
            hardenedDevice.stackMax = 1;

            inventory.Add(geiger, 1);
            inventory.Add(hardenedDevice, 1);

            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });

            var radio = new RadioState { AvailableFuel = 10f };

            var result = EMPEvent.ApplyGlobal(inventory, shelter, radio);

            var geigerSlot = inventory.FindSlot("geiger_counter");
            var hardenedSlot = inventory.FindSlot("hardened_dosimeter");
            Assert.IsTrue(geigerSlot.Device.Broken, "Unshielded geiger must break on EMP");
            Assert.IsFalse(hardenedSlot.Device.Broken, "EMP-shielded device must survive");
            Assert.IsFalse(shelter.GetModule("air_filtration").IsEnabled, "Unshielded module must be disabled");
            Assert.IsTrue(radio.EmpDamage >= 100f, "Radio must take full EMP damage");
            Assert.That(result.DevicesBroken, Is.EqualTo(1));
            Assert.That(result.ModulesDisabled, Is.EqualTo(1));
            Assert.IsTrue(result.RadioDestroyed);

            Object.DestroyImmediate(geiger);
            Object.DestroyImmediate(hardenedDevice);
        }

        [Test]
        public void EMPEvent_ShieldedShelterModule_StaysEnabled()
        {
            var moduleDef = ScriptableObject.CreateInstance<AirFiltrationModuleSO>();
            moduleDef.EmpShielded = true;
            var instance = new ShelterModuleInstance(moduleDef, 1);

            bool changed = EMPEvent.ApplyToShelterModule(instance, shielded: true);

            Assert.IsFalse(changed);
            Assert.IsTrue(instance.IsEnabled);

            Object.DestroyImmediate(moduleDef);
        }

        [Test]
        public void TradeEconomy_MoneyDropsToZero_WaterAndIodineSkyrocket_PostFlashpoint()
        {
            var cash = ScriptableObject.CreateInstance<ItemDefinition>();
            cash.type = ItemType.Trade;
            cash.tradeValue = 50f;

            var water = ScriptableObject.CreateInstance<ItemDefinition>();
            water.type = ItemType.Water;
            water.tradeValue = 15f;

            var iodine = ScriptableObject.CreateInstance<ItemDefinition>();
            iodine.type = ItemType.Iodine;
            iodine.tradeValue = 6f;

            float cashPreWar = TradeEconomy.GetEffectiveValue(cash, WorldPhase.CivilWar);
            float cashPostWar = TradeEconomy.GetEffectiveValue(cash, WorldPhase.NuclearWinter);
            Assert.That(cashPreWar, Is.GreaterThan(0f));
            Assert.That(cashPostWar, Is.EqualTo(0f).Within(Eps));

            float waterPreWar = TradeEconomy.GetEffectiveValue(water, WorldPhase.CivilWar);
            float waterPostWar = TradeEconomy.GetEffectiveValue(water, WorldPhase.NuclearWinter);
            Assert.That(waterPostWar, Is.GreaterThan(waterPreWar), "Water must skyrocket post-Flashpoint");

            float iodinePreWar = TradeEconomy.GetEffectiveValue(iodine, WorldPhase.CivilWar);
            float iodinePostWar = TradeEconomy.GetEffectiveValue(iodine, WorldPhase.NuclearWinter);
            Assert.That(iodinePostWar, Is.GreaterThan(iodinePreWar), "Iodine must skyrocket post-Flashpoint");

            Object.DestroyImmediate(cash);
            Object.DestroyImmediate(water);
            Object.DestroyImmediate(iodine);
        }

        [Test]
        public void LootTableSO_GetValidEntries_GatesByPhaseRequirement()
        {
            var jewelry = ScriptableObject.CreateInstance<ItemDefinition>();
            var cannedFood = ScriptableObject.CreateInstance<ItemDefinition>();

            var table = ScriptableObject.CreateInstance<LootTableSO>();
            table.entries.Add(new LootEntry { item = cannedFood, weight = 1f, phaseRequirement = WorldPhase.CivilWar });
            table.entries.Add(new LootEntry { item = jewelry, weight = 1f, phaseRequirement = WorldPhase.NuclearWinter });

            var civilWarEntries = table.GetValidEntries(WorldPhase.CivilWar);
            Assert.That(civilWarEntries.Count, Is.EqualTo(1));
            Assert.That(civilWarEntries[0].item, Is.EqualTo(cannedFood));

            var nuclearWinterEntries = table.GetValidEntries(WorldPhase.NuclearWinter);
            Assert.That(nuclearWinterEntries.Count, Is.EqualTo(2));

            Object.DestroyImmediate(jewelry);
            Object.DestroyImmediate(cannedFood);
            Object.DestroyImmediate(table);
        }

        [Test]
        public void WeatherSystem_RestrictToNonHazardWeather_ExcludesPostWarKinds()
        {
            var seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            seasonProfile.weatherCheckIntervalHours = 1f;
            seasonProfile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "hazard_heavy", displayName = "Hazard Heavy", startDay = 0,
                    clearWeight = 0f, rainWeight = 0f, overcastWeight = 0f,
                    ashfallWeight = 10f, falloutStormWeight = 10f, blizzardWeight = 10f
                }
            };

            var weather = new WeatherSystem(seasonProfile, seed: 7) { RestrictToNonHazardWeather = true };

            for (int i = 0; i < 200; i++)
            {
                weather.Tick(1f);
                bool isMundane = weather.Current == WeatherKind.Clear
                    || weather.Current == WeatherKind.Rain
                    || weather.Current == WeatherKind.Overcast;
                Assert.IsTrue(isMundane, "Restricted weather must never roll a post-war hazard");
            }

            weather.RestrictToNonHazardWeather = false;
            bool rolledHazard = false;
            for (int i = 0; i < 200; i++)
            {
                weather.Tick(1f);
                if (weather.Current == WeatherKind.Ashfall || weather.Current == WeatherKind.FalloutStorm
                    || weather.Current == WeatherKind.Blizzard)
                {
                    rolledHazard = true;
                    break;
                }
            }
            Assert.IsTrue(rolledHazard, "Once unrestricted, a hazard-heavy season should roll a hazard");

            Object.DestroyImmediate(seasonProfile);
        }
    }
}
