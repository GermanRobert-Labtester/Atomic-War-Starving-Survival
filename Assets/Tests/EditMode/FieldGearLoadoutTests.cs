using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Field gear contract: face/body changes never lose worn protection when
    /// the bag is full, and the ordinary inventory save state remains the
    /// source of truth for both worn gear and bunker-crate contents.
    /// </summary>
    [TestFixture]
    public class FieldGearLoadoutTests
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
        public void FullBag_SwapsBodyGearThroughCrate_ThenPersistsAndStowsSafely()
        {
            var hazmatSuit = MakeGear("hazmat_suit", "Hazmat Suit", EquipSlot.Body, 80f);
            var water = MakeWater();
            var fieldBag = new Inventory { Capacity = 1, MaxWeight = 30f };
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            var system = new FieldGearLoadoutSystem(fieldBag, overflow);
            FieldGearLoadoutResult lastResult = null;
            int stateChanges = 0;
            system.OnActionResolved += result => lastResult = result;
            system.OnChanged += () => stateChanges++;

            Assert.That(fieldBag.Add(hazmatSuit, 1), Is.True);
            Assert.That(fieldBag.Equip(hazmatSuit), Is.True);
            Assert.That(fieldBag.Add(hazmatSuit, 1), Is.True,
                "The replacement fills the only field-bag stack while the first suit is worn.");
            Assert.That(fieldBag.CanAdd(hazmatSuit, 1), Is.False,
                "Precondition: the outgoing suit cannot re-enter the full bag.");

            Assert.That(system.TryEquip("hazmat_suit", out var equipped), Is.True);
            Assert.That(equipped.Succeeded, Is.True);
            Assert.That(equipped.SentToOverflow, Is.True);
            Assert.That(lastResult, Is.SameAs(equipped));
            Assert.That(fieldBag.GetEquipped(EquipSlot.Body)?.Item, Is.SameAs(hazmatSuit));
            Assert.That(fieldBag.Count(hazmatSuit), Is.Zero, "The replacement is now worn, not duplicated in the bag.");
            Assert.That(overflow.Count(hazmatSuit), Is.EqualTo(1), "Outgoing gear is retained in the receiving crate.");
            Assert.That(system.GetSnapshot().TotalProtection, Is.EqualTo(80f).Within(0.001f));

            fieldBag.GetEquipped(EquipSlot.Body).CurrentDurability = 63f;
            var savedFieldBag = fieldBag.CaptureState();
            var savedOverflow = overflow.CaptureState();
            var restoredFieldBag = new Inventory();
            var restoredOverflow = new Inventory();
            restoredFieldBag.RestoreState(savedFieldBag, id => id == "hazmat_suit" ? hazmatSuit : null);
            restoredOverflow.RestoreState(savedOverflow, id => id == "hazmat_suit" ? hazmatSuit : null);
            Assert.That(restoredFieldBag.GetEquipped(EquipSlot.Body)?.CurrentDurability, Is.EqualTo(63f).Within(0.001f));
            Assert.That(restoredOverflow.Count(hazmatSuit), Is.EqualTo(1), "Crate content saves by master item id.");

            Assert.That(fieldBag.Add(water, 1), Is.True, "Fill the only bag stack before stowing worn gear.");
            Assert.That(system.TryUnequip(EquipSlot.Body, out var stowed), Is.True);
            Assert.That(stowed.SentToOverflow, Is.True);
            Assert.That(fieldBag.GetEquipped(EquipSlot.Body), Is.Null);
            Assert.That(fieldBag.Count(water), Is.EqualTo(1), "A stow operation must not displace the bag item.");
            Assert.That(overflow.Count(hazmatSuit), Is.EqualTo(2), "Both safe crate moves are retained.");
            Assert.That(stateChanges, Is.GreaterThanOrEqualTo(2));
            system.Dispose();
        }

        [Test]
        public void LoadoutHud_PresentsProtectionAndDelegatesEquipIntent()
        {
            var gasMask = MakeGear("gas_mask", "Gas Mask", EquipSlot.Face, 30f);
            var hazmatSuit = MakeGear("hazmat_suit", "Hazmat Suit", EquipSlot.Body, 80f);
            var fieldBag = new Inventory { Capacity = 4, MaxWeight = 30f };
            var overflow = new Inventory { Capacity = 0, MaxWeight = 0f };
            var system = new FieldGearLoadoutSystem(fieldBag, overflow);
            Assert.That(fieldBag.Add(gasMask, 1), Is.True);
            Assert.That(fieldBag.Add(hazmatSuit, 1), Is.True);

            var go = new GameObject("FieldGearLoadoutHudTests");
            _toDestroy.Add(go);
            var hud = go.AddComponent<FieldGearLoadoutHUD>();
            hud.Bind(system.GetSnapshot);
            hud.OnEquipRequested += itemId =>
            {
                system.TryEquip(itemId, out var result);
                hud.ReportActionResult(result.Succeeded ? "EQUIPPED: " + result.DisplayName + "." : "HELD: " + result.Reason);
            };

            hud.Open();
            StringAssert.Contains("FIELD GEAR LOADOUT", hud.PanelSummary);
            StringAssert.Contains("Gas Mask", hud.PanelSummary);
            StringAssert.Contains("Hazmat Suit", hud.PanelSummary);
            Assert.That(hud.EquipSelected(), Is.True);
            Assert.That(fieldBag.GetEquipped(EquipSlot.Face)?.Item, Is.SameAs(gasMask));
            StringAssert.Contains("FACE: Gas Mask", hud.PanelSummary);
            StringAssert.Contains("SCAVENGE SHIELDING: 30", hud.PanelSummary);
            Assert.That(hud.ToggleSelectedSlot(), Is.True);
            StringAssert.Contains("SELECTED STOW SLOT: FACE", hud.PanelSummary);

            system.Dispose();
        }

        [Test]
        public void KeybindDocs_ExposeFieldGearLoadoutKey()
        {
            var go = new GameObject("FieldGearLoadoutInputTests");
            _toDestroy.Add(go);
            var input = go.AddComponent<PlayerInputHandler>();

            Assert.That(input.FieldGearLoadoutKey, Is.EqualTo(KeyCode.L));
        }

        private ItemDefinition MakeGear(string id, string displayName, EquipSlot slot, float protection)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = displayName;
            item.type = ItemType.Protective;
            item.stackMax = 1;
            item.weight = 2f;
            item.isEquipable = true;
            item.equipSlot = slot;
            item.radProtection = protection;
            item.durability = 100f;
            _toDestroy.Add(item);
            return item;
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
