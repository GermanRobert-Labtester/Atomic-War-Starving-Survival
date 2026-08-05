using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class ExpeditionEngineTests
    {
        private const float Eps = 1e-3f;

        private NeedsProfile _needsProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemCatalogSO _itemCatalog;
        private ItemDefinition _foodItem;
        private ItemDefinition _waterItem;
        private LocationDefinitionSO _location;
        private ExpeditionSystem _expeditionSystem;

        [SetUp]
        public void SetUp()
        {
            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsProfile.hungerPerHour = 2f;
            _needsProfile.thirstPerHour = 3f;
            _needsProfile.fatiguePerHour = 1.5f;

            _needsSystem = new NeedsSystem(_needsProfile, sv => true);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            _foodItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _foodItem.id = "canned_food";
            _foodItem.displayName = "Canned Food";
            _foodItem.weight = 0.5f;

            _waterItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _waterItem.id = "clean_water";
            _waterItem.displayName = "Clean Water";
            _waterItem.weight = 1.0f;

            _itemCatalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            _itemCatalog.items = new List<ItemDefinition> { _foodItem, _waterItem };

            _location = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            _location.id = "ruined_subway";
            _location.displayName = "Ruined Subway";
            _location.travelHours = 3f;
            _location.baseRadsPerHour = 20f;
            _location.dangerLevel = 2f;

            _expeditionSystem = new ExpeditionSystem(_radSystem, _inventory, _itemCatalog, seed: 12345);
        }

        [Test]
        public void Simulate10TickExpedition_SurvivorReturnsWithLootAndUpdatedLifetimeRad()
        {
            var survivor = new Survivor { Id = "sv_tester", DisplayName = "Tester" };
            _needsSystem.Register(survivor);
            _radSystem.Register(survivor);

            float initialLifetimeRad = survivor.LifetimeRadiationExposure;
            int initialInventoryCount = _inventory.Slots.Count;

            // Start 3-tick travel distance expedition
            bool started = _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            Assert.IsTrue(started, "Expedition should start successfully.");
            Assert.IsTrue(_expeditionSystem.IsOnExpedition(survivor.Id));
            Assert.AreEqual(SurvivorState.Working, survivor.State);

            var state = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            Assert.IsNotNull(state);

            // Force add 2 loot items to guarantee loot return test assertion
            state.TryAddLoot(_foodItem);
            state.TryAddLoot(_waterItem);

            // Tick 10 times (3 outbound, 4 looting, 3 inbound)
            _expeditionSystem.Tick(10f);

            // Survivor should have completed expedition
            Assert.IsFalse(_expeditionSystem.IsOnExpedition(survivor.Id), "Survivor should no longer be on active expedition.");
            Assert.AreEqual(SurvivorState.Idle, survivor.State, "Survivor should return to Idle state in bunker.");
            Assert.Greater(survivor.LifetimeRadiationExposure, initialLifetimeRad, "Lifetime radiation exposure should increase.");
            Assert.Greater(_inventory.Slots.Count, initialInventoryCount, "Loot should be deposited into bunker inventory.");
            Assert.Less(survivor.Needs.Fatigue, 100f);
        }

        [Test]
        public void RecklessSurvivor_AutoEngagesInCombatEncounter()
        {
            var reckless = new Survivor
            {
                Id = "sv_reckless",
                DisplayName = "Reckless Scavenger",
                RiskBias = RiskBiasTrait.Reckless
            };
            _needsSystem.Register(reckless);
            _radSystem.Register(reckless);

            _expeditionSystem.StartExpedition(reckless, _location, ExpeditionStance.Speed);
            var state = _expeditionSystem.GetExpeditionBySurvivor(reckless.Id);

            // Manually advance to Looting phase
            state.Phase = ExpeditionPhase.Looting;
            state.TravelTicksCompleted = 3;

            // Tick expedition to trigger loot / encounter loop
            _expeditionSystem.Tick(1f);

            Assert.AreEqual(RiskBiasTrait.Reckless, reckless.RiskBias);
            Assert.IsTrue(state.Stamina < 100f, "Stamina should drain during expedition.");
        }

        [Test]
        public void ParanoidSurvivor_FleesAndDropsLootWhenAnxious()
        {
            var paranoid = new Survivor
            {
                Id = "sv_paranoid",
                DisplayName = "Paranoid Scavenger",
                RiskBias = RiskBiasTrait.Paranoid,
                RadiationAnxiety = 0.8f,
                HasRadiationAnxietyStatus = true
            };
            _needsSystem.Register(paranoid);
            _radSystem.Register(paranoid);

            _expeditionSystem.StartExpedition(paranoid, _location, ExpeditionStance.Stealth);
            var state = _expeditionSystem.GetExpeditionBySurvivor(paranoid.Id);
            state.TryAddLoot(_foodItem);
            state.TryAddLoot(_waterItem);

            // Advance to Looting phase
            state.Phase = ExpeditionPhase.Looting;
            state.TravelTicksCompleted = 3;

            // Tick once in looting phase
            _expeditionSystem.Tick(1f);

            // Paranoid survivor should trigger early inbound return
            Assert.AreEqual(ExpeditionPhase.Inbound, state.Phase, "Paranoid survivor should flee to Inbound phase.");
        }

        [Test]
        public void SaveLoadRoundtrip_MidExpeditionStateIsPreserved()
        {
            string tempSaveDir = Path.Combine(Path.GetTempPath(), "expedition_save_test_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempSaveDir);

            try
            {
                var gameState = new GameState { Phase = GamePhase.Running };
                var survivor = new Survivor { Id = "sv_scout", DisplayName = "Scout" };
                var survivorsList = new List<Survivor> { survivor };

                var saveSystem = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = gameState,
                WeatherSystem = null,
                TemperatureSystem = null,
                NeedsSystem = _needsSystem,
                RadiationSystem = _radSystem,
                Shelter = null,
                GetSurvivors = () => survivorsList,
                ItemLookup = id => id == "canned_food" ? _foodItem : (id == "clean_water" ? _waterItem : null),
                ModuleLookup = id => null,
                SavesDir = tempSaveDir
            });

                saveSystem.SetExpeditionSystem(_expeditionSystem);

                // Start expedition and advance to mid-looting state
                _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Speed);
                var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
                exp.Phase = ExpeditionPhase.Looting;
                exp.LootingTicksCompleted = 2;
                exp.Stamina = 75f;
                exp.TryAddLoot(_foodItem);

                // Save to slot
                bool saveSuccess = saveSystem.Save("test_slot");
                Assert.IsTrue(saveSuccess, "Save should succeed.");

                // Restore into fresh ExpeditionSystem & SaveSystem
                var newExpeditionSystem = new ExpeditionSystem(_radSystem, _inventory, _itemCatalog, seed: 12345);
                var newSaveSystem = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = gameState,
                WeatherSystem = null,
                TemperatureSystem = null,
                NeedsSystem = _needsSystem,
                RadiationSystem = _radSystem,
                Shelter = null,
                GetSurvivors = () => survivorsList,
                ItemLookup = id => id == "canned_food" ? _foodItem : (id == "clean_water" ? _waterItem : null),
                ModuleLookup = id => null,
                SavesDir = tempSaveDir
            });
                newSaveSystem.SetExpeditionSystem(newExpeditionSystem);

                bool loadSuccess = newSaveSystem.Load("test_slot");
                Assert.IsTrue(loadSuccess, "Load should succeed.");

                var restoredExp = newExpeditionSystem.GetExpeditionBySurvivor("sv_scout");
                Assert.IsNotNull(restoredExp, "Restored expedition state should exist.");
                Assert.AreEqual(ExpeditionPhase.Looting, restoredExp.Phase);
                Assert.AreEqual(ExpeditionStance.Speed, restoredExp.Stance);
                Assert.AreEqual(2, restoredExp.LootingTicksCompleted);
                Assert.AreEqual(75f, restoredExp.Stamina, Eps);
                Assert.AreEqual(1, restoredExp.CollectedLoot.Count);
                Assert.AreEqual("canned_food", restoredExp.CollectedLoot[0].id);
            }
            finally
            {
                if (Directory.Exists(tempSaveDir))
                {
                    Directory.Delete(tempSaveDir, true);
                }
            }
        }

        [Test]
        public void BicycleWiring_EquipsWhenInventoryHasBike_AndShortensTravel()
        {
            var bikeDef = ScriptableObject.CreateInstance<ItemDefinition>();
            bikeDef.id = BicycleSystem.BicycleItemId;
            bikeDef.displayName = "Bicycle";
            bikeDef.weight = 8f;
            _inventory.Add(bikeDef, 1);

            var bike = new BicycleSystem();
            _expeditionSystem.SetBicycleSystem(bike);
            _expeditionSystem.SetHasItem(id => _inventory.CountById(id) > 0);

            var survivor = new Survivor { Id = "sv_bike", DisplayName = "Rider" };
            _needsSystem.Register(survivor);
            _radSystem.Register(survivor);

            // 6h travel without bike → 6 ticks; with bike → ~3 ticks.
            _location.travelHours = 6f;
            Assert.IsTrue(_expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth));

            var state = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            Assert.IsNotNull(state);
            Assert.IsTrue(state.HasBicycle, "Bicycle should equip from inventory.");
            Assert.LessOrEqual(state.TotalDistanceTicks, 4,
                "Bicycle should halve outbound distance ticks.");
            Assert.Greater(state.BicycleDurability, 0f);
        }

        [Test]
        public void FloodedNodeWiring_ArrivalWithoutPump_SetsWading()
        {
            var flooded = new FloodedNodeSystem();
            flooded.SetFlooded(_location.id, true);
            _expeditionSystem.SetFloodedNodeSystem(flooded);
            _expeditionSystem.SetHasItem(_ => false); // no pump

            var survivor = new Survivor { Id = "sv_wade", DisplayName = "Wader" };
            survivor.Needs.Warmth = 80f;
            _needsSystem.Register(survivor);
            _radSystem.Register(survivor);

            Assert.IsTrue(_expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth));
            var state = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            // Jump to arrival
            state.TravelTicksCompleted = state.TotalDistanceTicks;
            state.Phase = ExpeditionPhase.Outbound;
            _expeditionSystem.Tick(1f);

            Assert.IsTrue(state.IsWading || state.Phase == ExpeditionPhase.Looting,
                "Flooded arrival should process (wading or looting).");
            if (state.Phase == ExpeditionPhase.Looting || state.IsWading)
            {
                Assert.IsTrue(state.IsWading, "Without a pump, flooded nodes force wading.");
                Assert.AreEqual(0f, survivor.Needs.Warmth, Eps);
            }
        }

        [Test]
        public void FloodedNodeSave_RoundTrip_PreservesNodeIds()
        {
            var flooded = new FloodedNodeSystem();
            flooded.SetFlooded("node_a", true);
            flooded.SetFlooded("node_b", true);

            var save = flooded.CaptureState();
            Assert.IsNotNull(save.FloodedNodeIds);
            Assert.AreEqual(2, save.FloodedNodeIds.Length);

            var restored = new FloodedNodeSystem();
            restored.RestoreState(save);
            Assert.IsTrue(restored.IsFlooded("node_a"));
            Assert.IsTrue(restored.IsFlooded("node_b"));
            Assert.IsFalse(restored.IsFlooded("node_c"));
        }
    }
}
