using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class EMPEventTests
    {
        [Test]
        public void ApplyToDevice_Unshielded_BreaksDevice_ReturnsTrue()
        {
            var device = DeviceState.CreateDefault();
            bool changed = EMPEvent.ApplyToDevice(device, shielded: false);

            Assert.IsTrue(changed);
            Assert.IsTrue(device.Broken);
        }

        [Test]
        public void ApplyToDevice_Shielded_LeavesDeviceIntact_ReturnsFalse()
        {
            var device = DeviceState.CreateDefault();
            bool changed = EMPEvent.ApplyToDevice(device, shielded: true);

            Assert.IsFalse(changed);
            Assert.IsFalse(device.Broken);
        }

        [Test]
        public void ApplyToDevice_AlreadyBroken_ReturnsFalse_NoDoubleCount()
        {
            var device = DeviceState.CreateDefault();
            device.Broken = true;

            bool changed = EMPEvent.ApplyToDevice(device, shielded: false);

            Assert.IsFalse(changed, "Already-broken devices must not be recounted as newly broken.");
            Assert.IsTrue(device.Broken);
        }

        [Test]
        public void ApplyToDevice_NullDevice_ReturnsFalse_NoThrow()
        {
            Assert.DoesNotThrow(() => Assert.IsFalse(EMPEvent.ApplyToDevice(null, false)));
        }

        [Test]
        public void ApplyToShelterModule_Unshielded_DisablesModule_ReturnsTrue()
        {
            var module = new ShelterModuleInstance("test_module") { IsEnabled = true };
            bool changed = EMPEvent.ApplyToShelterModule(module, shielded: false);

            Assert.IsTrue(changed);
            Assert.IsFalse(module.IsEnabled);
        }

        [Test]
        public void ApplyToShelterModule_Shielded_LeavesModuleEnabled_ReturnsFalse()
        {
            var module = new ShelterModuleInstance("test_module") { IsEnabled = true };
            bool changed = EMPEvent.ApplyToShelterModule(module, shielded: true);

            Assert.IsFalse(changed);
            Assert.IsTrue(module.IsEnabled);
        }

        [Test]
        public void ApplyToShelterModule_AlreadyDisabled_ReturnsFalse()
        {
            var module = new ShelterModuleInstance("test_module") { IsEnabled = false };
            bool changed = EMPEvent.ApplyToShelterModule(module, shielded: false);

            Assert.IsFalse(changed);
        }

        [Test]
        public void ApplyToRadio_Unshielded_DestroysRadio_ReturnsTrue()
        {
            var radio = new RadioState();
            bool destroyed = EMPEvent.ApplyToRadio(radio, shielded: false);

            Assert.IsTrue(destroyed);
            Assert.AreEqual(100f, radio.EmpDamage);
            Assert.IsFalse(radio.IsOperational);
        }

        [Test]
        public void ApplyToRadio_Shielded_LeavesRadioUndamaged_ReturnsFalse()
        {
            var radio = new RadioState();
            bool destroyed = EMPEvent.ApplyToRadio(radio, shielded: true);

            Assert.IsFalse(destroyed);
            Assert.AreEqual(0f, radio.EmpDamage);
        }

        [Test]
        public void ApplyGlobal_ShieldedDeviceAndModule_SurviveBlast_RadioStillDestroyed()
        {
            var inventory = new Inventory();
            var shieldedItem = ScriptableObject.CreateInstance<ItemDefinition>();
            shieldedItem.empShielded = true;
            var shieldedDevice = DeviceState.CreateDefault();
            inventory.Slots.Add(new InventorySlot { Item = shieldedItem, Device = shieldedDevice });

            var unshieldedItem = ScriptableObject.CreateInstance<ItemDefinition>();
            unshieldedItem.empShielded = false;
            var unshieldedDevice = DeviceState.CreateDefault();
            inventory.Slots.Add(new InventorySlot { Item = unshieldedItem, Device = unshieldedDevice });

            var shelter = new Shelter();
            var module = new ShelterModuleInstance("test_module") { IsEnabled = true };
            shelter.AddModule(module);

            var radio = new RadioState();

            var result = EMPEvent.ApplyGlobal(inventory, shelter, radio);

            Assert.IsFalse(shieldedDevice.Broken, "Shielded device must survive the blast.");
            Assert.IsTrue(unshieldedDevice.Broken, "Unshielded device must be broken by the blast.");
            Assert.AreEqual(1, result.DevicesBroken);
            Assert.AreEqual(1, result.ModulesDisabled);
            Assert.IsTrue(result.RadioDestroyed);
            Assert.IsFalse(module.IsEnabled);
        }

        [Test]
        public void ApplyGlobal_NullInventoryAndShelter_StillDamagesRadio_NoThrow()
        {
            var radio = new RadioState();
            EmpResult result = default;

            Assert.DoesNotThrow(() => result = EMPEvent.ApplyGlobal(null, null, radio));

            Assert.AreEqual(0, result.DevicesBroken);
            Assert.AreEqual(0, result.ModulesDisabled);
            Assert.IsTrue(result.RadioDestroyed);
        }

        [Test]
        public void ApplyGlobal_SlotWithoutDevice_IsSkipped()
        {
            var inventory = new Inventory();
            var nonDeviceItem = ScriptableObject.CreateInstance<ItemDefinition>();
            inventory.Slots.Add(new InventorySlot { Item = nonDeviceItem, Device = null });

            var result = EMPEvent.ApplyGlobal(inventory, null, new RadioState());

            Assert.AreEqual(0, result.DevicesBroken);
        }
    }
}
