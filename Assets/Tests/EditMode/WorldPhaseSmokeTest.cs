using System;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;
using Object = UnityEngine.Object;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Acceptance test: a headless run transitioning from Day 29 to Day 30 disables
    /// unshielded electronics, swaps the active weather profile to Ashfall, and
    /// updates the trader economy values — the Day-30 Flashpoint cascade.
    /// </summary>
    [TestFixture]
    public class WorldPhaseSmokeTest
    {
        private const float HoursPerDay = 24f;

        private NeedsProfile _profile;
        private SeasonProfile _seasonProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private WeatherSystem _weatherSystem;
        private TemperatureSystem _tempSystem;
        private Shelter _shelter;
        private Inventory _inventory;
        private WorldPhaseSystem _worldPhaseSystem;
        private ItemDefinition _geiger;
        private ItemDefinition _cash;
        private RadioState _radio;
        private Random _rng;

        [SetUp]
        public void SetUp()
        {
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _profile.hungerPerHour = 2f;
            _profile.thirstPerHour = 3f;
            _profile.fatiguePerHour = 1.5f;
            _profile.warmthLossPerHourInCold = 4f;
            _profile.warmthRestorePerHourNearHeat = 6f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 10f;
            _profile.healthLossFromHunger = 3f;
            _profile.healthLossFromThirst = 4f;
            _profile.healthLossFromCold = 2f;
            _profile.moraleLossPerHourWhileCritical = 1f;

            // Season authored entirely toward post-war hazards, to prove the Phase 1
            // restriction (not designer authoring) is what keeps weather mundane.
            _seasonProfile = ScriptableObject.CreateInstance<SeasonProfile>();
            _seasonProfile.campaignLengthDays = 60;
            _seasonProfile.weatherCheckIntervalHours = 1f;
            _seasonProfile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "hazard_heavy", displayName = "Hazard Heavy", startDay = 0,
                    clearWeight = 0f, rainWeight = 0f, overcastWeight = 0f,
                    ashfallWeight = 10f, falloutStormWeight = 10f, blizzardWeight = 10f
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_profile);
            Object.DestroyImmediate(_seasonProfile);
            if (_geiger != null) Object.DestroyImmediate(_geiger);
            if (_cash != null) Object.DestroyImmediate(_cash);
        }

        private void InitSystems(int seed)
        {
            _rng = new Random(seed);

            _weatherSystem = new WeatherSystem(_seasonProfile, seed) { RestrictToNonHazardWeather = true };
            _tempSystem = new TemperatureSystem(_seasonProfile, _weatherSystem);
            _needsSystem = new NeedsSystem(_profile, sv => true);
            _radSystem = new RadiationSystem(_needsSystem) { IsPaused = true };

            _shelter = new Shelter();
            _shelter.AddModule(new ShelterModuleInstance("air_filtration", 2) { FilterHealth = 100f });

            _inventory = new Inventory { Capacity = 20, MaxWeight = 100f };

            _geiger = ScriptableObject.CreateInstance<ItemDefinition>();
            _geiger.id = "geiger_counter";
            _geiger.type = ItemType.Device;
            _geiger.empShielded = false;
            _geiger.stackMax = 1;
            _inventory.Add(_geiger, 1);

            _cash = ScriptableObject.CreateInstance<ItemDefinition>();
            _cash.id = "currency";
            _cash.type = ItemType.Trade;
            _cash.tradeValue = 20f;
            _cash.stackMax = 99;
            _inventory.Add(_cash, 1);

            _radio = new RadioState { AvailableFuel = 10f };

            var config = ScriptableObject.CreateInstance<WorldPhaseConfigSO>();
            config.flashpointDay = 30;
            config.exchangeMoraleHit = 25f;
            _worldPhaseSystem = new WorldPhaseSystem(config);
            Object.DestroyImmediate(config); // WorldPhaseSystem copies the values out at construction
        }

        private void ApplyNuclearExchangeCascade()
        {
            // Mirrors GameBootstrap.HandleNuclearExchange: EMP, weather snap, radiation unpause.
            EMPEvent.ApplyGlobal(_inventory, _shelter, _radio);
            _weatherSystem.RestrictToNonHazardWeather = false;
            _weatherSystem.ForceWeather(WeatherKind.Ashfall);
            _radSystem.IsPaused = false;
        }

        [Test]
        public void Day29To30Transition_DisablesElectronics_SwapsWeather_UpdatesEconomy()
        {
            InitSystems(seed: 7);
            _worldPhaseSystem.OnNuclearExchange += ApplyNuclearExchangeCascade;

            // --- Days 1..29: Civil War (Phase 1) ---
            for (int day = 1; day < 30; day++)
            {
                for (float hour = 0; hour < HoursPerDay; hour++)
                {
                    _weatherSystem.Tick(1f);
                    _tempSystem.Tick(1f);
                    _shelter.Tick(1f);
                    _needsSystem.Tick(1f);
                }
                _worldPhaseSystem.OnDayTick(day);

                Assert.That(_worldPhaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.CivilWar));
                Assert.IsTrue(_radSystem.IsPaused, "Radiation must stay paused pre-Flashpoint");
                bool isMundane = _weatherSystem.Current == WeatherKind.Clear
                    || _weatherSystem.Current == WeatherKind.Rain
                    || _weatherSystem.Current == WeatherKind.Overcast;
                Assert.IsTrue(isMundane, $"Day {day}: weather must stay mundane pre-Flashpoint");
            }

            var geigerSlot = _inventory.FindSlot("geiger_counter");
            Assert.IsFalse(geigerSlot.Device.Broken, "Geiger must still work through Day 29");
            Assert.That(TradeEconomy.GetEffectiveValue(_cash, _worldPhaseSystem.CurrentPhase), Is.GreaterThan(0f),
                "Currency must still hold value through Day 29");

            // --- Day 30: the atomic exchange ---
            _worldPhaseSystem.OnDayTick(30);

            Assert.That(_worldPhaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.Flashpoint));
            Assert.IsTrue(_worldPhaseSystem.HasTriggeredExchange);

            Assert.IsTrue(geigerSlot.Device.Broken, "Unshielded geiger must break on the Day-30 EMP");
            Assert.IsFalse(_shelter.GetModule("air_filtration").IsEnabled, "Unshielded filtration must be disabled by EMP");
            Assert.IsTrue(_radio.EmpDamage >= 100f, "Radio must be destroyed by EMP");

            Assert.AreEqual(WeatherKind.Ashfall, _weatherSystem.Current, "Weather must snap to Ashfall at the exchange");
            Assert.IsFalse(_radSystem.IsPaused, "Radiation must activate map-wide at the exchange");

            Assert.That(TradeEconomy.GetEffectiveValue(_cash, _worldPhaseSystem.CurrentPhase), Is.EqualTo(0f),
                "Currency must be worthless immediately after the exchange");

            // --- Day 31: Nuclear Winter label advance, no re-fire ---
            _worldPhaseSystem.OnDayTick(31);
            Assert.That(_worldPhaseSystem.CurrentPhase, Is.EqualTo(WorldPhase.NuclearWinter));
        }
    }
}
