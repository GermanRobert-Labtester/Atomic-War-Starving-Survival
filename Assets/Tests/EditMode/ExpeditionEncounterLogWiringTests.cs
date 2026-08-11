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
    /// Audit H-6h — ExpeditionSystem.OnEncounterTriggered had zero subscribers in
    /// production; only OnEncounterResolved was wired to ExpeditionEncounterLog, so
    /// the player only ever saw the outcome, never the "you've run into X" beat.
    /// GameBootstrap.OnExpeditionEncounterTriggered_LogCombat is private, so — same
    /// as GameBootstrap.OnExpeditionEncounterResolved_LogCombat's precedent — this
    /// mirrors its exact logic against a real ExpeditionSystem + ExpeditionEncounterLog
    /// pair, driven through the real forced-encounter production path.
    /// </summary>
    [TestFixture]
    public class ExpeditionEncounterLogWiringTests
    {
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
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(needsProfile);
            _needsSystem = new NeedsSystem(needsProfile, sv => true);
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
                if (_toDestroy[i] != null) Object.DestroyImmediate(_toDestroy[i]);

            if (_expedition?.EncounterPool != null)
            {
                for (int i = 0; i < _expedition.EncounterPool.Count; i++)
                    if (_expedition.EncounterPool[i] != null)
                        Object.DestroyImmediate(_expedition.EncounterPool[i]);
            }
        }

        /// <summary>Mirrors GameBootstrap.OnExpeditionEncounterTriggered_LogCombat exactly.</summary>
        private static void LogTriggered(ExpeditionEncounterLog log, ExpeditionState exp, EncounterSO selected)
        {
            if (log == null || selected == null) return;
            string who = exp?.Survivor != null
                ? (exp.Survivor.DisplayName ?? exp.Survivor.Id)
                : "Scavenger";
            string enc = !string.IsNullOrEmpty(selected.title)
                ? selected.title
                : (selected.id ?? "contact").Replace('_', ' ');
            log.Add($"{who} encounters {enc}...");
        }

        [Test]
        public void OnEncounterTriggered_PushesPreResolutionBeat_ToLog()
        {
            var ambush = SafeHavenEncounters.CreateAmbush();
            _toDestroy.Add(ambush);
            ambush.title = "Raider Ambush";
            _expedition.AddEncounter(ambush);

            var survivor = new Survivor { Id = "scout", DisplayName = "Scout" };
            _needsSystem.Register(survivor);
            _radSystem.Register(survivor);

            var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            loc.id = EventRunner.SafeHavenTargetLocationId;
            loc.displayName = loc.id;
            loc.travelHours = 1f;
            loc.baseRadsPerHour = 5f;
            loc.dangerLevel = 1f;
            _toDestroy.Add(loc);

            var log = new ExpeditionEncounterLog();
            _expedition.OnEncounterTriggered += (exp, enc) => LogTriggered(log, exp, enc);

            Assert.That(log.Count, Is.EqualTo(0), "Log must be empty before any encounter fires.");
            Assert.That(_expedition.StartExpedition(survivor, loc, ExpeditionStance.Stealth), Is.True);

            for (int i = 0; i < 30 && log.Count == 0; i++)
                _expedition.Tick(1f);

            Assert.That(log.Count, Is.EqualTo(1), "The forced ambush must push exactly one triggered beat.");
            Assert.That(log.Latest, Does.Contain("Scout"));
            Assert.That(log.Latest, Does.Contain("Raider Ambush"));
        }

        [Test]
        public void LogTriggered_FallsBackToId_WhenTitleMissing()
        {
            var log = new ExpeditionEncounterLog();
            var encounter = ScriptableObject.CreateInstance<EncounterSO>();
            encounter.id = "feral_dog_pack";
            encounter.title = string.Empty;
            _toDestroy.Add(encounter);

            LogTriggered(log, exp: null, selected: encounter);

            Assert.That(log.Latest, Does.Contain("feral dog pack"));
            Assert.That(log.Latest, Does.Contain("Scavenger"), "Null expedition survivor must fall back to a generic label.");
        }
    }
}
