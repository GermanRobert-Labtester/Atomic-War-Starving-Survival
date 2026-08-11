using System.Collections.Generic;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using NUnit.Framework;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Water-terminal contract: the player can choose the next safe conversion
    /// queue, all cistern tiers remain saveable, and only clean cistern water
    /// augments the bunker ration source.
    /// </summary>
    [TestFixture]
    public class WaterPurificationTerminalTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();
        private WaterPurifierModuleSO _purifierDefinition;

        [SetUp]
        public void SetUp()
        {
            _purifierDefinition = ScriptableObject.CreateInstance<WaterPurifierModuleSO>();
            _purifierDefinition.ModuleId = "water_purifier";
            _purifierDefinition.DisplayName = "Water Purifier";
            _purifierDefinition.ConversionHoursPerUnit = 2f;
            _purifierDefinition.FilterDegradationPerUnitConverted = 5f;
            _toDestroy.Add(_purifierDefinition);
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

        [Test]
        public void DirtyFirstQueue_ProcessesDirtyReserveAndPersists()
        {
            var shelter = CreateShelter();
            var storage = new WaterStorage { DirtyWater = 2f, IrradiatedWater = 1f };
            var economy = new WaterEconomySystem();
            int stateChanges = 0;
            int queueChanges = 0;
            economy.OnWaterStateChanged += () => stateChanges++;
            economy.OnPurifierQueueChanged += _ => queueChanges++;

            Assert.That(economy.SetPurifierQueueMode(PurifierQueueMode.DirtyFirst), Is.True);
            var before = economy.GetSnapshot(shelter, storage);
            Assert.That(before.QueueMode, Is.EqualTo(PurifierQueueMode.DirtyFirst));
            Assert.That(before.NextSourceLabel, Is.EqualTo("DIRTY"));
            Assert.That(before.NextOutputLabel, Is.EqualTo("CLEAN"));
            Assert.That(before.UnitsQueued, Is.EqualTo(2));

            economy.Tick(2f, AtomicWar._Game.Environment.WeatherKind.Clear, 1, shelter, storage);
            Assert.That(storage.CleanWater, Is.EqualTo(1f).Within(0.001f));
            Assert.That(storage.DirtyWater, Is.EqualTo(1f).Within(0.001f));
            Assert.That(storage.IrradiatedWater, Is.EqualTo(1f).Within(0.001f));
            Assert.That(queueChanges, Is.EqualTo(1));
            Assert.That(stateChanges, Is.GreaterThanOrEqualTo(2));

            var saved = economy.CaptureState();
            var restored = new WaterEconomySystem();
            restored.RestoreState(saved);
            Assert.That(restored.CurrentPurifierQueue, Is.EqualTo(PurifierQueueMode.DirtyFirst));

            var savedStorage = storage.CaptureState();
            var restoredStorage = new WaterStorage();
            restoredStorage.RestoreState(savedStorage);
            Assert.That(restoredStorage.CleanWater, Is.EqualTo(1f).Within(0.001f));
            Assert.That(restoredStorage.DirtyWater, Is.EqualTo(1f).Within(0.001f));
            Assert.That(restoredStorage.IrradiatedWater, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void CleanCisternWater_ExtendsRationPreviewAndDailyIssue()
        {
            int food = 2;
            int carriedWater = 0;
            var storage = new WaterStorage { CleanWater = 2f, DirtyWater = 4f, IrradiatedWater = 3f };
            var rations = new BunkerRationingSystem(
                resource => resource == RationResource.Food ? food : carriedWater,
                (resource, amount) =>
                {
                    if (resource == RationResource.Food)
                    {
                        int issued = Mathf.Min(food, amount);
                        food -= issued;
                        return issued;
                    }
                    int issuedWater = Mathf.Min(carriedWater, amount);
                    carriedWater -= issuedWater;
                    return issuedWater;
                },
                () => Mathf.FloorToInt(storage.CleanWater),
                amount => Mathf.FloorToInt(storage.ConsumeClean(amount)));
            var needs = new NeedsSystem(CreateProfile());
            var survivors = CreateSurvivors(2, 80f, 80f);

            var preview = rations.GetSnapshot(survivors);
            Assert.That(preview.InventoryWaterOnHand, Is.Zero);
            Assert.That(preview.CleanCisternWaterOnHand, Is.EqualTo(2));
            Assert.That(preview.ProjectedWaterCoverage, Is.EqualTo(1f));

            Assert.That(rations.ApplyDailyRations(1, survivors, needs), Is.True);
            Assert.That(storage.CleanWater, Is.Zero);
            Assert.That(storage.DirtyWater, Is.EqualTo(4f), "Dirty water is never a ration source.");
            Assert.That(storage.IrradiatedWater, Is.EqualTo(3f), "Irradiated water is never a ration source.");
            Assert.That(survivors[0].Needs.Thirst, Is.EqualTo(30f).Within(0.001f));
            Assert.That(survivors[1].Needs.Thirst, Is.EqualTo(30f).Within(0.001f));
        }

        [Test]
        public void WaterTerminal_PresentsQueueAndRationLink()
        {
            var shelter = CreateShelter();
            var storage = new WaterStorage { CleanWater = 2f, DirtyWater = 4f, IrradiatedWater = 1f };
            var economy = new WaterEconomySystem();
            var ration = new BunkerRationingSnapshot
            {
                CleanCisternWaterOnHand = 2,
                WaterOnHand = 3,
                WaterRequired = 2,
                ProjectedWaterCoverage = 1f,
                ProjectedThirstReduction = 50f
            };
            var go = new GameObject("WaterPurificationHudTests");
            _toDestroy.Add(go);
            var terminal = go.AddComponent<WaterPurificationHUD>();
            terminal.Bind(() => economy.GetSnapshot(shelter, storage), () => ration);
            terminal.OnQueueCycleRequested += direction =>
            {
                bool changed = economy.CyclePurifierQueue(direction);
                terminal.ReportQueueResult(changed ? "Queue changed." : "Queue unchanged.");
            };

            terminal.Open();
            StringAssert.Contains("BUNKER WATER TERMINAL", terminal.PanelSummary);
            StringAssert.Contains("clean 2", terminal.PanelSummary);
            StringAssert.Contains("dirty 4", terminal.PanelSummary);
            StringAssert.Contains("IRRADIATED", terminal.PanelSummary);
            StringAssert.Contains("RATION LINK", terminal.PanelSummary);
            Assert.That(terminal.QueueNext(), Is.True);
            Assert.That(economy.CurrentPurifierQueue, Is.EqualTo(PurifierQueueMode.IrradiatedFirst));
            StringAssert.Contains("IRRADIATED FIRST", terminal.PanelSummary);

            var inputGo = new GameObject("WaterPurificationInputTests");
            _toDestroy.Add(inputGo);
            var input = inputGo.AddComponent<PlayerInputHandler>();
            Assert.That(input.WaterPurificationKey, Is.EqualTo(KeyCode.Y));
        }

        private Shelter CreateShelter()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_purifierDefinition, 1)
            {
                IsEnabled = true,
                FilterHealth = 100f
            });
            return shelter;
        }

        private NeedsProfile CreateProfile()
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(profile);
            return profile;
        }

        private static List<Survivor> CreateSurvivors(int count, float hunger, float thirst)
        {
            var survivors = new List<Survivor>(count);
            for (int i = 0; i < count; i++)
            {
                var survivor = new Survivor { Id = "survivor_" + i, DisplayName = "Survivor " + i };
                survivor.Needs.Hunger = hunger;
                survivor.Needs.Thirst = thirst;
                survivors.Add(survivor);
            }
            return survivors;
        }
    }
}
