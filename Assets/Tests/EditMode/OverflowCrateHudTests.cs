using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Receiving-crate contract: the field bag cannot consume a return unless it
    /// can safely accept it, and the UI remains a view/intent layer over that
    /// Core inventory truth.
    /// </summary>
    [TestFixture]
    public class OverflowCrateHudTests
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
        public void FullFieldBag_HoldsCrateItem_ThenMovesOneSafelyAndRoundTrips()
        {
            var water = MakeWater();
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            var fieldBag = new Inventory { Capacity = 1, MaxWeight = 10f };
            Assert.That(overflow.Add(water, 2), Is.True);
            Assert.That(fieldBag.Add(water, 1), Is.True, "fill the only field-bag stack");

            var system = new OverflowCrateSystem(overflow, fieldBag);
            OverflowCrateTransferResult lastResult = null;
            int stateChanges = 0;
            system.OnTransferResolved += result => lastResult = result;
            system.OnChanged += () => stateChanges++;

            Assert.That(system.TryTransferOne("clean_water", out var held), Is.False);
            Assert.That(held.Succeeded, Is.False);
            StringAssert.Contains("safe room", held.Reason);
            Assert.That(overflow.Count(water), Is.EqualTo(2), "a held transfer must not lose the crate item");
            Assert.That(fieldBag.Count(water), Is.EqualTo(1));
            Assert.That(lastResult, Is.SameAs(held));

            Assert.That(fieldBag.Remove(water, 1), Is.True);
            Assert.That(system.TryTransferOne("clean_water", out var moved), Is.True);
            Assert.That(moved.Succeeded, Is.True);
            Assert.That(overflow.Count(water), Is.EqualTo(1));
            Assert.That(fieldBag.Count(water), Is.EqualTo(1), "transfer moves one unit, not an opaque stack");
            Assert.That(stateChanges, Is.GreaterThanOrEqualTo(2), "bag change and completed transfer publish state");

            var savedCrate = overflow.CaptureState();
            var restoredCrate = new Inventory();
            restoredCrate.RestoreState(savedCrate, id => id == "clean_water" ? water : null);
            Assert.That(restoredCrate.Count(water), Is.EqualTo(1), "overflow contents persist by item id");
            system.Dispose();
        }

        [Test]
        public void CrateHud_PresentsCoreSnapshot_AndSendsSelectedItemIntent()
        {
            var water = MakeWater();
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            var fieldBag = new Inventory { Capacity = 4, MaxWeight = 10f };
            overflow.Add(water, 2);
            var system = new OverflowCrateSystem(overflow, fieldBag);

            var go = new GameObject("OverflowCrateHudTests");
            _toDestroy.Add(go);
            var hud = go.AddComponent<OverflowCrateHUD>();
            hud.Bind(system.GetSnapshot);
            hud.OnTransferRequested += itemId =>
            {
                system.TryTransferOne(itemId, out var result);
                hud.ReportTransferResult(result.Succeeded
                    ? "MOVED: 1 " + result.DisplayName + " to the field bag."
                    : "HELD: " + result.Reason);
            };

            hud.Open();
            StringAssert.Contains("BUNKER OVERFLOW CRATE", hud.PanelSummary);
            StringAssert.Contains("Clean Water  ×2", hud.PanelSummary);
            Assert.That(hud.TransferSelected(), Is.True);

            Assert.That(overflow.Count(water), Is.EqualTo(1));
            Assert.That(fieldBag.Count(water), Is.EqualTo(1));
            StringAssert.Contains("MOVED: 1 Clean Water", hud.LastOutcome);
            system.Dispose();
        }

        [Test]
        public void KeybindDocs_ExposeOverflowCrateKey()
        {
            var go = new GameObject("OverflowCrateInputTests");
            _toDestroy.Add(go);
            var input = go.AddComponent<PlayerInputHandler>();

            Assert.That(input.OverflowCrateKey, Is.EqualTo(KeyCode.O));
        }

        private ItemDefinition MakeWater()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = "clean_water";
            item.displayName = "Clean Water";
            item.type = ItemType.Water;
            item.stackMax = 1;
            item.weight = 1f;
            _toDestroy.Add(item);
            return item;
        }
    }
}
