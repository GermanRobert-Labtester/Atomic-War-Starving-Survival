using System.Collections.Generic;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using NUnit.Framework;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Rationing contract: policy consumes only the allotted clean stores once
    /// per day, applies a fair shared result, and persists independently of
    /// inventory's existing save state.
    /// </summary>
    [TestFixture]
    public class BunkerRationingTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();

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
        public void DailyIssue_UsesPolicyOnceAndAppliesEqualRelief()
        {
            var stock = new Stockpile { Food = 2, Water = 1 };
            var system = CreateSystem(stock);
            var needs = new NeedsSystem(CreateProfile());
            var survivors = CreateSurvivors(2, 80f, 80f, 50f);
            int reports = 0;
            system.OnDailyRationsApplied += _ => reports++;

            Assert.That(system.AdjustLevel(RationResource.Water, -1), Is.True);
            Assert.That(system.ApplyDailyRations(1, survivors, needs), Is.True);

            Assert.That(stock.Food, Is.Zero);
            Assert.That(stock.Water, Is.Zero);
            Assert.That(survivors[0].Needs.Hunger, Is.EqualTo(40f).Within(0.001f));
            Assert.That(survivors[1].Needs.Hunger, Is.EqualTo(40f).Within(0.001f));
            Assert.That(survivors[0].Needs.Thirst, Is.EqualTo(55f).Within(0.001f));
            Assert.That(survivors[1].Needs.Thirst, Is.EqualTo(55f).Within(0.001f));
            Assert.That(survivors[0].Needs.Morale, Is.EqualTo(48f).Within(0.001f));
            Assert.That(survivors[1].Needs.Morale, Is.EqualTo(48f).Within(0.001f));
            Assert.That(system.LastReport.FoodCoverage, Is.EqualTo(1f));
            Assert.That(system.LastReport.WaterCoverage, Is.EqualTo(1f));
            Assert.That(reports, Is.EqualTo(1));

            Assert.That(system.ApplyDailyRations(1, survivors, needs), Is.False,
                "Loading or ticking the same day must not duplicate a ration issue.");
            Assert.That(reports, Is.EqualTo(1));
        }

        [Test]
        public void Shortage_IsSharedInProjectionAndSaveRestoresPolicy()
        {
            var stock = new Stockpile { Food = 1, Water = 0 };
            var system = CreateSystem(stock);
            var needs = new NeedsSystem(CreateProfile());
            var survivors = CreateSurvivors(2, 100f, 100f, 50f);
            system.SetLevel(RationResource.Food, RationLevel.Full);
            system.SetLevel(RationResource.Water, RationLevel.Full);

            var projected = system.GetSnapshot(survivors);
            Assert.That(projected.FoodRequired, Is.EqualTo(3));
            Assert.That(projected.WaterRequired, Is.EqualTo(3));
            Assert.That(projected.ProjectedFoodCoverage, Is.EqualTo(1f / 3f).Within(0.001f));
            Assert.That(projected.ProjectedWaterCoverage, Is.Zero);
            Assert.That(projected.ProjectedMoraleDelta, Is.LessThan(0f));

            Assert.That(system.ApplyDailyRations(4, survivors, needs), Is.True);
            Assert.That(survivors[0].Needs.Hunger, Is.EqualTo(survivors[1].Needs.Hunger).Within(0.001f));
            Assert.That(survivors[0].Needs.Morale, Is.EqualTo(survivors[1].Needs.Morale).Within(0.001f));
            Assert.That(survivors[0].Needs.Hunger, Is.EqualTo(100f - 55f / 3f).Within(0.001f));
            Assert.That(survivors[0].Needs.Thirst, Is.EqualTo(100f).Within(0.001f));
            Assert.That(stock.Food, Is.Zero);

            var save = system.CaptureState();
            var restored = CreateSystem(stock);
            restored.RestoreState(save);
            Assert.That(restored.FoodLevel, Is.EqualTo(RationLevel.Full));
            Assert.That(restored.WaterLevel, Is.EqualTo(RationLevel.Full));
            Assert.That(restored.LastAppliedDay, Is.EqualTo(4));
            Assert.That(restored.ApplyDailyRations(4, survivors, needs), Is.False);
        }

        [Test]
        public void RationBoard_DelegatesPolicyIntentAndExposesKeybind()
        {
            var stock = new Stockpile { Food = 3, Water = 3 };
            var system = CreateSystem(stock);
            var survivors = CreateSurvivors(2, 0f, 0f, 75f);
            var go = new GameObject("BunkerRationingHudTests");
            _toDestroy.Add(go);
            var board = go.AddComponent<BunkerRationingHUD>();
            board.Bind(() => system.GetSnapshot(survivors));
            board.OnLevelAdjustmentRequested += (resource, direction) =>
            {
                bool changed = system.AdjustLevel(resource, direction);
                board.ReportAdjustment(changed ? "Policy changed." : "Policy unchanged.");
            };

            board.Open();
            StringAssert.Contains("BUNKER RATION BOARD", board.PanelSummary);
            StringAssert.Contains("FOOD: STANDARD", board.PanelSummary);
            StringAssert.Contains("WATER: STANDARD", board.PanelSummary);
            StringAssert.Contains("Projected relief", board.PanelSummary);
            Assert.That(board.IncreaseSelected(), Is.True);
            Assert.That(system.FoodLevel, Is.EqualTo(RationLevel.Full));
            StringAssert.Contains("FOOD: FULL", board.PanelSummary);
            Assert.That(board.ToggleSelectedResource(), Is.True);
            Assert.That(board.DecreaseSelected(), Is.True);
            Assert.That(system.WaterLevel, Is.EqualTo(RationLevel.Restricted));

            var inputGo = new GameObject("BunkerRationingInputTests");
            _toDestroy.Add(inputGo);
            var input = inputGo.AddComponent<PlayerInputHandler>();
            Assert.That(input.BunkerRationingKey, Is.EqualTo(KeyCode.T));
        }

        private static BunkerRationingSystem CreateSystem(Stockpile stock)
        {
            return new BunkerRationingSystem(
                resource => resource == RationResource.Food ? stock.Food : stock.Water,
                (resource, amount) =>
                {
                    if (resource == RationResource.Food)
                    {
                        int issued = Mathf.Min(stock.Food, amount);
                        stock.Food -= issued;
                        return issued;
                    }

                    int waterIssued = Mathf.Min(stock.Water, amount);
                    stock.Water -= waterIssued;
                    return waterIssued;
                });
        }

        private sealed class Stockpile
        {
            public int Food;
            public int Water;
        }

        private NeedsProfile CreateProfile()
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(profile);
            return profile;
        }

        private static List<Survivor> CreateSurvivors(int count, float hunger, float thirst, float morale)
        {
            var survivors = new List<Survivor>(count);
            for (int i = 0; i < count; i++)
            {
                var survivor = new Survivor { Id = "survivor_" + i, DisplayName = "Survivor " + i };
                survivor.Needs.Hunger = hunger;
                survivor.Needs.Thirst = thirst;
                survivor.Needs.Morale = morale;
                survivors.Add(survivor);
            }
            return survivors;
        }
    }
}
