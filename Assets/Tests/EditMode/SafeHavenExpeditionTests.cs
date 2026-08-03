using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #47 — radio intel reliability → expedition location outcomes.
    /// Unverified send injects sniper ambush (forceOnArrival at safe haven grid).
    /// Analyzed Trap injects empty cache. Location filter keeps other sites clean.
    /// </summary>
    [TestFixture]
    public class SafeHavenExpeditionTests
    {
        private NeedsProfile _needsProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemCatalogSO _itemCatalog;
        private ExpeditionSystem _expedition;
        private List<Object> _toDestroy;

        [SetUp]
        public void SetUp()
        {
            _toDestroy = new List<Object>();
            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(_needsProfile);
            _needsSystem = new NeedsSystem(_needsProfile, sv => true);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 40, MaxWeight = 200f };
            _itemCatalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            _itemCatalog.items = new List<ItemDefinition>();
            _toDestroy.Add(_itemCatalog);
            _expedition = new ExpeditionSystem(_radSystem, _inventory, _itemCatalog, seed: 7);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null) Object.DestroyImmediate(_toDestroy[i]);
            }
            // Pool encounters are SOs
            if (_expedition?.EncounterPool != null)
            {
                for (int i = 0; i < _expedition.EncounterPool.Count; i++)
                {
                    if (_expedition.EncounterPool[i] != null)
                        Object.DestroyImmediate(_expedition.EncounterPool[i]);
                }
            }
        }

        private Survivor MakeSurvivor(string id = "scout")
        {
            var s = new Survivor { Id = id, DisplayName = id };
            _needsSystem.Register(s);
            _radSystem.Register(s);
            return s;
        }

        private LocationDefinitionSO MakeLocation(string id, float travelHours = 1f)
        {
            var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            loc.id = id;
            loc.displayName = id;
            loc.travelHours = travelHours;
            loc.baseRadsPerHour = 5f;
            loc.dangerLevel = 1f;
            _toDestroy.Add(loc);
            return loc;
        }

        [Test]
        public void AmbushFactory_BindsToSafeHavenLocation_ForceOnArrival()
        {
            var ambush = SafeHavenEncounters.CreateAmbush();
            _toDestroy.Add(ambush);

            Assert.That(ambush.id, Is.EqualTo(EventRunner.SafeHavenAmbushEncounterId));
            Assert.That(ambush.requiredLocationId, Is.EqualTo(EventRunner.SafeHavenTargetLocationId));
            Assert.That(ambush.forceOnArrival, Is.True);
            Assert.That(ambush.GetEffectiveWeight(ExpeditionStance.Stealth, 1f, "ruined_subway"), Is.EqualTo(0f),
                "Ambush must not roll at unrelated locations.");
            Assert.That(ambush.GetEffectiveWeight(ExpeditionStance.Stealth, 1f, EventRunner.SafeHavenTargetLocationId),
                Is.GreaterThan(0f));
        }

        [Test]
        public void EmptyCacheFactory_BindsToSafeHaven_DiscoveryCategory()
        {
            var cache = SafeHavenEncounters.CreateEmptyCache();
            _toDestroy.Add(cache);

            Assert.That(cache.id, Is.EqualTo(SafeHavenEncounters.EmptyCacheEncounterId));
            Assert.That(cache.category, Is.EqualTo(EncounterCategory.Discovery));
            Assert.That(cache.requiredLocationId, Is.EqualTo(EventRunner.SafeHavenTargetLocationId));
            Assert.That(cache.forceOnArrival, Is.True);
        }

        [Test]
        public void AddEncounter_DoesNotWipeDefaultPool()
        {
            int before = _expedition.EncounterPool.Count;
            Assert.That(before, Is.GreaterThan(0), "Default encounters should exist.");

            var ambush = SafeHavenEncounters.CreateAmbush();
            _toDestroy.Add(ambush);
            _expedition.AddEncounter(ambush);

            Assert.That(_expedition.EncounterPool.Count, Is.EqualTo(before + 1));
            Assert.That(_expedition.HasEncounter(EventRunner.SafeHavenAmbushEncounterId), Is.True);
            Assert.That(_expedition.HasEncounter("enc_feral_dogs"), Is.True);
        }

        [Test]
        public void PickEncounter_UnrelatedLocation_NeverReturnsAmbush()
        {
            var ambush = SafeHavenEncounters.CreateAmbush();
            _toDestroy.Add(ambush);
            _expedition.AddEncounter(ambush);

            // Many rolls — ambush must never leak to other sites.
            for (int i = 0; i < 40; i++)
            {
                var pick = _expedition.PickEncounter("ruined_subway", ExpeditionStance.Speed, 5f);
                if (pick == null) continue;
                Assert.That(pick.id, Is.Not.EqualTo(EventRunner.SafeHavenAmbushEncounterId),
                    "Location-bound ambush must not pick at ruined_subway.");
            }
        }

        [Test]
        public void FindForcedLocationEncounter_ReturnsAmbushAtSafeHaven()
        {
            var ambush = SafeHavenEncounters.CreateAmbush();
            _toDestroy.Add(ambush);
            _expedition.AddEncounter(ambush);

            Assert.That(_expedition.FindForcedLocationEncounter("ruined_subway"), Is.Null);
            var forced = _expedition.FindForcedLocationEncounter(EventRunner.SafeHavenTargetLocationId);
            Assert.That(forced, Is.Not.Null);
            Assert.That(forced.id, Is.EqualTo(EventRunner.SafeHavenAmbushEncounterId));
        }

        [Test]
        public void ArrivalAtSafeHaven_Unverified_FiresAmbushOnce()
        {
            var ambush = SafeHavenEncounters.CreateAmbush();
            _toDestroy.Add(ambush);
            _expedition.AddEncounter(ambush);

            var survivor = MakeSurvivor();
            var loc = MakeLocation(EventRunner.SafeHavenTargetLocationId, travelHours: 1f);

            EncounterSO fired = null;
            int fireCount = 0;
            _expedition.OnEncounterTriggered += (exp, enc) =>
            {
                if (enc != null && enc.id == EventRunner.SafeHavenAmbushEncounterId)
                {
                    fired = enc;
                    fireCount++;
                }
            };

            Assert.That(_expedition.StartExpedition(survivor, loc, ExpeditionStance.Stealth), Is.True);

            // Tick until arrival (Outbound → Looting). travelHours=1 → few ticks.
            for (int i = 0; i < 30 && fired == null; i++)
                _expedition.Tick(1f);

            Assert.That(fired, Is.Not.Null, "Ambush must force-fire on first arrival at Safe Haven.");
            Assert.That(fireCount, Is.EqualTo(1));

            var state = _expedition.GetExpeditionBySurvivor(survivor.Id);
            Assert.That(state, Is.Not.Null);
            Assert.That(state.LocationEncounterFired, Is.True);
            Assert.That(state.Phase, Is.EqualTo(ExpeditionPhase.Looting).Or.EqualTo(ExpeditionPhase.Inbound));

            // Further ticks must not re-fire the forced ambush.
            for (int i = 0; i < 10; i++)
                _expedition.Tick(1f);
            Assert.That(fireCount, Is.EqualTo(1), "forceOnArrival must fire only once.");
        }

        [Test]
        public void ArrivalAtSafeHaven_VerifiedTrap_FiresEmptyCache_NotAmbush()
        {
            // Only empty cache injected (analyzed path).
            var cache = SafeHavenEncounters.CreateEmptyCache();
            _toDestroy.Add(cache);
            _expedition.AddEncounter(cache);

            var survivor = MakeSurvivor("analyst");
            var loc = MakeLocation(EventRunner.SafeHavenTargetLocationId, travelHours: 1f);

            EncounterSO fired = null;
            _expedition.OnEncounterTriggered += (exp, enc) =>
            {
                if (enc != null &&
                    (enc.id == SafeHavenEncounters.EmptyCacheEncounterId
                     || enc.id == EventRunner.SafeHavenAmbushEncounterId))
                    fired = enc;
            };

            Assert.That(_expedition.StartExpedition(survivor, loc), Is.True);
            for (int i = 0; i < 30 && fired == null; i++)
                _expedition.Tick(1f);

            Assert.That(fired, Is.Not.Null);
            Assert.That(fired.id, Is.EqualTo(SafeHavenEncounters.EmptyCacheEncounterId));
            Assert.That(fired.id, Is.Not.EqualTo(EventRunner.SafeHavenAmbushEncounterId));
        }

        [Test]
        public void ShouldInjectSafeHavenAmbush_DrivesWhichFactory()
        {
            // Mirrors GameBootstrap send_expedition branch.
            var unverified = new EventContext();
            Assert.That(EventRunner.ShouldInjectSafeHavenAmbush(unverified), Is.True);

            var trap = new EventContext { ActiveIntelReliability = IntelReliability.Trap };
            Assert.That(EventRunner.ShouldInjectSafeHavenAmbush(trap), Is.False);

            // Inject accordingly
            if (EventRunner.ShouldInjectSafeHavenAmbush(unverified))
                _expedition.AddEncounter(SafeHavenEncounters.CreateAmbush());
            Assert.That(_expedition.HasEncounter(EventRunner.SafeHavenAmbushEncounterId), Is.True);

            var other = new ExpeditionSystem(_radSystem, _inventory, _itemCatalog, seed: 9);
            if (!EventRunner.ShouldInjectSafeHavenAmbush(trap))
                other.AddEncounter(SafeHavenEncounters.CreateEmptyCache());
            Assert.That(other.HasEncounter(SafeHavenEncounters.EmptyCacheEncounterId), Is.True);
            Assert.That(other.HasEncounter(EventRunner.SafeHavenAmbushEncounterId), Is.False);

            // cleanup other pool
            for (int i = 0; i < other.EncounterPool.Count; i++)
            {
                if (other.EncounterPool[i] != null)
                    Object.DestroyImmediate(other.EncounterPool[i]);
            }
        }
    }
}
