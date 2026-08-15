using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using Ashfall.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #48 — weather-driven hatch entrapment.
    /// Force Blizzard 72h → Buried; expeditions hard-locked; DigOut spikes entry CO2.
    /// </summary>
    [TestFixture]
    public class HatchEntrapmentTests
    {
        private WeatherSystem _weather;
        private HatchEntrapmentSystem _hatch;
        private ExpeditionSystem _expedition;
        private NeedsProfile _needsProfile;
        private NeedsSystem _needs;
        private RadiationSystem _rad;
        private Inventory _inventory;
        private ItemCatalogSO _catalog;
        private LocationDefinitionSO _location;
        private List<Object> _toDestroy;
        private ShelterRoom _entryRoom;
        private AtomicWar._Game.Shelter.Shelter _shelter;

        [SetUp]
        public void SetUp()
        {
            _toDestroy = new List<Object>();
            _weather = new WeatherSystem(); // manual mode — ForceWeather only
            _hatch = new HatchEntrapmentSystem();
            _entryRoom = new ShelterRoom(HatchEntrapmentSystem.EntryRoomId, null);
            _shelter = new AtomicWar._Game.Shelter.Shelter();

            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(_needsProfile);
            _needs = new NeedsSystem(_needsProfile, sv => true);
            _rad = new RadiationSystem(_needs);
            _inventory = new Inventory { Capacity = 40, MaxWeight = 200f };
            _catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            _catalog.items = new List<ItemDefinition>();
            _toDestroy.Add(_catalog);

            _location = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            _location.id = "ruined_subway";
            _location.displayName = "Ruined Subway";
            _location.travelHours = 2f;
            _location.baseRadsPerHour = 10f;
            _location.dangerLevel = 1f;
            _toDestroy.Add(_location);

            _expedition = new ExpeditionSystem(_rad, _inventory, _catalog, seed: 11);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null) Object.DestroyImmediate(_toDestroy[i]);
            }
            if (_expedition?.EncounterPool != null)
            {
                for (int i = 0; i < _expedition.EncounterPool.Count; i++)
                {
                    if (_expedition.EncounterPool[i] != null)
                        Object.DestroyImmediate(_expedition.EncounterPool[i]);
                }
            }
        }

        private void SyncLock()
        {
            _expedition.HatchBlocksExpeditions = _hatch.AreExpeditionsLocked;
        }

        private void TickBlizzardHours(float hours)
        {
            _weather.ForceWeather(WeatherKind.Blizzard);
            // Tick in 6h chunks (mirrors weather check cadence) for realism.
            float remaining = hours;
            while (remaining > 0f)
            {
                float step = Mathf.Min(6f, remaining);
                _hatch.Tick(step, _weather.Current, _shelter);
                remaining -= step;
            }
            SyncLock();
        }

        [Test]
        public void ForceBlizzard_72Hours_HatchBecomesBuried()
        {
            Assert.That(_hatch.State, Is.EqualTo(HatchState.Clear));

            TickBlizzardHours(HatchEntrapmentSystem.HazardHoursToSeal);

            Assert.That(_hatch.State, Is.EqualTo(HatchState.Buried),
                "Blizzard lasting longer than 3 days must bury the hatch.");
            Assert.That(_hatch.ContinuousHazardHours, Is.GreaterThanOrEqualTo(72f));
            Assert.That(_hatch.BuriedEventFired, Is.True);
            Assert.That(_hatch.AreExpeditionsLocked, Is.True);
        }

        [Test]
        public void BuriedHatch_HardLocksExpeditions_AndDisablesUi()
        {
            var clearScout = new Survivor { Id = "clear_scout", DisplayName = "Clear Scout" };
            var buriedScout = new Survivor { Id = "buried_scout", DisplayName = "Buried Scout" };
            _needs.Register(clearScout);
            _needs.Register(buriedScout);
            _rad.Register(clearScout);
            _rad.Register(buriedScout);

            Assert.That(_expedition.StartExpedition(clearScout, _location), Is.True,
                "Expeditions should start while hatch is Clear.");

            TickBlizzardHours(72f);

            Assert.That(_hatch.State, Is.EqualTo(HatchState.Buried));
            Assert.That(_expedition.HatchBlocksExpeditions, Is.True);
            Assert.That(_expedition.IsExpeditionUiEnabled, Is.False,
                "Expedition UI must be disabled while hatch is buried.");
            Assert.That(_expedition.StartExpedition(buriedScout, _location), Is.False,
                "No one can leave while the hatch is buried.");
        }

        [Test]
        public void DigOut_SpikesEntryRoomCo2_AndClearsHatch()
        {
            TickBlizzardHours(72f);
            Assert.That(_hatch.State, Is.EqualTo(HatchState.Buried));

            float co2Before = _entryRoom.Co2Ppm;
            Assert.That(_hatch.DigOut(_entryRoom), Is.True);

            Assert.That(_hatch.State, Is.EqualTo(HatchState.Clear));
            Assert.That(_entryRoom.Co2Ppm, Is.EqualTo(co2Before + HatchEntrapmentSystem.DigOutCo2SpikePpm),
                "DigOut exertion in a sealed space must spike entry-room CO2.");
            Assert.That(_hatch.AreExpeditionsLocked, Is.False);

            SyncLock();
            var survivor = new Survivor { Id = "digger", DisplayName = "Digger" };
            _needs.Register(survivor);
            _rad.Register(survivor);
            Assert.That(_expedition.StartExpedition(survivor, _location), Is.True,
                "After DigOut, expeditions may leave again.");
        }

        [Test]
        public void BrokenAirFilter_WhileBuried_StartsSuffocationCountdown()
        {
            // Bury first with a working filter, then break the filter.
            var air = new ShelterModuleInstance("air_filtration", 1)
            {
                FilterHealth = 100f,
                IsEnabled = true
            };
            _shelter.AddModule(air);

            TickBlizzardHours(72f);
            Assert.That(_hatch.State, Is.EqualTo(HatchState.Buried));
            Assert.That(_hatch.IsSuffocationActive, Is.False,
                "Working filter while buried must not start suffocation.");

            air.FilterHealth = 0f;
            Assert.That(HatchEntrapmentSystem.IsAirFilterBroken(_shelter), Is.True);

            // First sealed+broken tick arms the countdown at full duration.
            _hatch.Tick(1f, WeatherKind.Blizzard, _shelter);
            Assert.That(_hatch.IsSuffocationActive, Is.True);
            Assert.That(_hatch.SuffocationHoursRemaining,
                Is.EqualTo(HatchEntrapmentSystem.SuffocationDurationHours).Within(0.01f));

            _hatch.Tick(5f, WeatherKind.Blizzard, _shelter);
            Assert.That(_hatch.SuffocationHoursRemaining,
                Is.EqualTo(HatchEntrapmentSystem.SuffocationDurationHours - 5f).Within(0.01f));
        }

        [Test]
        public void HighFactionTrust_SchedulesOutsideDigOut()
        {
            string scheduledId = null;
            int scheduledDay = -1;
            var trust = new Dictionary<string, float>
            {
                { FactionSO.Ids.ScavengerCamp, 85f }
            };

            TickBlizzardHours(72f);
            _hatch.Tick(
                1f,
                WeatherKind.Blizzard,
                _shelter,
                id => trust.TryGetValue(id, out float t) ? t : 0f,
                (eventId, day, origin) =>
                {
                    scheduledId = eventId;
                    scheduledDay = day;
                },
                currentDay: 10);

            Assert.That(_hatch.FactionRescueScheduled, Is.True);
            Assert.That(_hatch.FactionRescueFactionId, Is.EqualTo(FactionSO.Ids.ScavengerCamp));
            Assert.That(scheduledId, Is.EqualTo(EventRunner.FactionDigOutEventId));
            Assert.That(scheduledDay, Is.EqualTo(11));
        }

        [Test]
        public void CultOfTheGlow_HighEffectiveTrust_SchedulesOutsideDigOut()
        {
            // Concept 16: cult is eligible for hatch dig-out when the trust
            // callback reports effective trust > 80 (highly irradiated party).
            string scheduledId = null;
            var trust = new Dictionary<string, float>
            {
                { FactionSO.Ids.CultOfTheGlow, 100f }
            };

            TickBlizzardHours(72f);
            _hatch.Tick(
                1f,
                WeatherKind.Blizzard,
                _shelter,
                id => trust.TryGetValue(id, out float t) ? t : 0f,
                (eventId, day, origin) => { scheduledId = eventId; },
                currentDay: 10);

            Assert.That(_hatch.FactionRescueScheduled, Is.True);
            Assert.That(_hatch.FactionRescueFactionId, Is.EqualTo(FactionSO.Ids.CultOfTheGlow));
            Assert.That(scheduledId, Is.EqualTo(EventRunner.FactionDigOutEventId));

            // Default candidate list must include cult_of_the_glow.
            Assert.That(
                HatchEntrapmentSystem.FindHighTrustFaction(
                    id => id == FactionSO.Ids.CultOfTheGlow ? 90f : 0f),
                Is.EqualTo(FactionSO.Ids.CultOfTheGlow));
        }

        [Test]
        public void BuriedAliveEvent_BodyText_AndExtremeWeatherGate()
        {
            var ev = EventRunner.CreateBuriedAliveEvent();
            _toDestroy.Add(ev);

            Assert.That(ev.id, Is.EqualTo(EventRunner.BuriedAliveEventId));
            Assert.That(ev.bodyText, Does.Contain("The hatch will not open. We are snowed in."));
            Assert.That(ev.conditions.RequireExtremeWeather, Is.True);

            var ctx = new EventContext
            {
                CurrentDay = 5,
                CurrentWeather = WeatherKind.Clear
            };
            ctx.SetEventFlag(HatchEntrapmentSystem.FlagBuriedAliveOffered, true);
            Assert.That(ev.CanTrigger(ctx), Is.False,
                "Buried Alive must not fire in clear weather.");

            ctx.CurrentWeather = WeatherKind.Blizzard;
            Assert.That(ev.CanTrigger(ctx), Is.True);

            ctx.CurrentWeather = WeatherKind.FalloutStorm;
            Assert.That(ev.CanTrigger(ctx), Is.True);
        }

        [Test]
        public void FalloutStorm_72Hours_FreezesHatch()
        {
            _weather.ForceWeather(WeatherKind.FalloutStorm);
            float remaining = 72f;
            while (remaining > 0f)
            {
                float step = Mathf.Min(6f, remaining);
                _hatch.Tick(step, WeatherKind.FalloutStorm, _shelter);
                remaining -= step;
            }

            Assert.That(_hatch.State, Is.EqualTo(HatchState.Frozen));
            Assert.That(_hatch.AreExpeditionsLocked, Is.True);
        }

        [Test]
        public void FactionRescue_ClearsHatch_AndSetsDebtFlag()
        {
            TickBlizzardHours(72f);
            var ctx = new EventContext();
            Assert.That(_hatch.ApplyFactionRescue(ctx), Is.True);
            Assert.That(_hatch.State, Is.EqualTo(HatchState.Clear));
            Assert.That(ctx.HasEventFlag(HatchEntrapmentSystem.FlagFactionDigOutDebt), Is.True);
        }

        [Test]
        public void CaptureRestore_PreservesBuriedState()
        {
            TickBlizzardHours(72f);
            var save = _hatch.CaptureState();

            var restored = new HatchEntrapmentSystem();
            restored.RestoreState(save);

            Assert.That(restored.State, Is.EqualTo(HatchState.Buried));
            Assert.That(restored.AreExpeditionsLocked, Is.True);
            Assert.That(restored.ContinuousHazardHours, Is.GreaterThanOrEqualTo(72f));
        }

        [Test]
        public void ClearWeather_BeforeSeal_ResetsHazardClock()
        {
            _hatch.Tick(48f, WeatherKind.Blizzard, _shelter);
            Assert.That(_hatch.ContinuousHazardHours, Is.EqualTo(48f).Within(0.01f));

            _hatch.Tick(6f, WeatherKind.Clear, _shelter);
            Assert.That(_hatch.ContinuousHazardHours, Is.EqualTo(0f),
                "Clear weather before seal must reset the continuous hazard clock.");
            Assert.That(_hatch.State, Is.EqualTo(HatchState.Clear));
        }
    }
}
