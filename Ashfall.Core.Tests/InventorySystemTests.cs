using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    public class InventorySystemTests
    {
        private static ItemDefinition Def(string id, ItemType type = ItemType.Food,
            int stackMax = 6, float weight = 0.5f, bool equipable = false,
            EquipSlot slot = EquipSlot.None, float radProt = 0f, float durability = 0f,
            float hunger = 0f, float thirst = 0f, float health = 0f, float trade = 0f)
        {
            return new ItemDefinition
            {
                id = id,
                displayName = id,
                type = type,
                stackMax = stackMax,
                weight = weight,
                isEquipable = equipable,
                equipSlot = slot,
                radProtection = radProt,
                durability = durability,
                hungerRestore = hunger,
                thirstRestore = thirst,
                healthEffect = health,
                tradeValue = trade
            };
        }

        // Distinct item ids so each forces its own stack.
        private static ItemDefinition UniqueDef(string id, ItemType type = ItemType.Material,
            int stackMax = 6, float weight = 0.5f, float trade = 0f) =>
            Def(id, type, stackMax, weight, trade: trade);

        [Fact]
        public void Add_StacksIntoExistingSlot_UpToStackMax()
        {
            var inv = new InventoryContainer();
            var food = Def("canned_food", stackMax: 6);
            Assert.True(inv.Add(food, 4));
            Assert.True(inv.Add(food, 4)); // 4 + 4 = 8 → one stack of 6 + one of 2
            Assert.Equal(8, inv.Count(food));
            Assert.Equal(2, inv.Slots.Count);
        }

        [Fact]
        public void Add_RejectedWhenCapacityExceeded_AllOrNothing()
        {
            var inv = new InventoryContainer { Capacity = 2 };
            var food = UniqueDef("canned_food");
            var water = UniqueDef("clean_water");
            inv.Add(food, 1);
            inv.Add(water, 1); // capacity 2 now full
            int before = inv.Count(food);
            // A third distinct stack would exceed capacity: no partial add.
            int added = 0;
            inv.OnItemAdded += (_, a) => added += a;
            Assert.False(inv.Add(UniqueDef("iodine"), 1));
            Assert.Equal(2, inv.Slots.Count);
            Assert.Equal(before, inv.Count(food));
            Assert.Equal(0, added);
        }

        [Fact]
        public void Add_RejectedWhenWeightExceeded_AllOrNothing()
        {
            var inv = new InventoryContainer { MaxWeight = 1f };
            var heavy = Def("fuel_canister", ItemType.Fuel, stackMax: 3, weight: 4f);
            Assert.False(inv.Add(heavy, 1));
            Assert.Equal(0, inv.Count(heavy));
        }

        [Fact]
        public void RemoveById_RemovesAcrossStacks()
        {
            var inv = new InventoryContainer();
            var food = Def("canned_food", stackMax: 4);
            inv.Add(food, 8); // 4 + 4
            Assert.True(inv.RemoveById("canned_food", 5));
            Assert.Equal(3, inv.Count(food));
            Assert.Single(inv.Slots);
        }

        [Fact]
        public void Equip_And_Unequip_ReturnItemToStorage()
        {
            var inv = new InventoryContainer();
            var mask = Def("gas_mask", ItemType.Protective, 1, 1.2f, true, EquipSlot.Face, 0.35f, 100f);
            inv.Add(mask, 1);
            Assert.True(inv.Equip(mask));
            Assert.NotNull(inv.GetEquipped(EquipSlot.Face));
            Assert.Equal(0, inv.Count(mask)); // out of storage
            Assert.True(inv.GetEquippedProtection() > 0f);

            var returned = inv.Unequip(EquipSlot.Face);
            Assert.NotNull(returned);
            Assert.Equal(1, inv.Count(mask)); // back in storage
        }

        [Fact]
        public void Equip_ReplacesOccupant_ReturningItToStorage()
        {
            var inv = new InventoryContainer();
            var mask = Def("gas_mask", ItemType.Protective, 1, 1.2f, true, EquipSlot.Face, 0.35f, 100f);
            var respirator = Def("respirator", ItemType.Protective, 1, 1f, true, EquipSlot.Face, 0.25f, 80f);
            inv.Add(mask, 1);
            inv.Add(respirator, 1);
            Assert.True(inv.Equip(mask));
            Assert.NotNull(inv.GetEquipped(EquipSlot.Face));
            Assert.True(inv.Equip(respirator)); // swap
            Assert.Same(respirator, inv.GetEquipped(EquipSlot.Face)?.Item);
            Assert.Equal(1, inv.Count(mask)); // old one returned
        }

        [Fact]
        public void Consume_RemovesOneUnit()
        {
            var inv = new InventoryContainer();
            var food = Def("canned_food", hunger: 40f);
            inv.Add(food, 3);
            Assert.True(inv.Consume(food));
            Assert.Equal(2, inv.Count(food));
        }

        [Fact]
        public void Consume_AppliesNeedAndContaminationCallbacks()
        {
            var inv = new InventoryContainer();
            var water = Def("irradiated_water", ItemType.IrradiatedWater, 4, 0.8f, thirst: 40f);
            water.contamination = 0.6f;
            inv.Add(water, 1);

            float hungerDelta = 0f, contamDose = 0f;
            bool iodine = false;
            inv.Consume(water,
                applyNeed: (t, v) => { if (t == ItemType.Water) hungerDelta = v; return true; },
                applyContamination: d => contamDose = d,
                applyIodine: () => iodine = true);

            Assert.Equal(-40f, hungerDelta);
            Assert.Equal(0.6f * InventoryContainer.ContaminationDosePerUnit, contamDose);
            Assert.False(iodine);
        }

        [Fact]
        public void Devices_HavePerSlotState_AndBestGeigerSelected()
        {
            var inv = new InventoryContainer();
            var geiger = Def("geiger_counter", ItemType.Device, 1, 0.9f);
            inv.Add(geiger, 2);

            var slot0 = inv.FindSlot("geiger_counter");
            Assert.NotNull(slot0?.Device);
            Assert.True(inv.HasWorkingGeiger());

            // Break the first, best should fall to the second (still working).
            InstrumentDevice.Break(slot0.Device);
            var best = inv.FindBestWorkingDevice("geiger_counter");
            Assert.NotNull(best);
            Assert.NotSame(slot0, best);
        }

        [Fact]
        public void DriftAllDevices_LowersCalibration()
        {
            var inv = new InventoryContainer();
            var geiger = Def("geiger_counter", ItemType.Device, 1, 0.9f);
            inv.Add(geiger, 1);
            var device = inv.FindSlot("geiger_counter").Device;
            float before = device.Calibration;
            inv.DriftAllDevices(10f);
            Assert.True(device.Calibration < before);
            Assert.False(device.Broken); // drift is not hard failure
        }

        [Fact]
        public void SaveRoundtrip_PreservesSlotsDevicesAndEquipped()
        {
            var inv = new InventoryContainer { Capacity = 30, MaxWeight = 120f };
            var food = Def("canned_food");
            var mask = Def("gas_mask", ItemType.Protective, 1, 1.2f, true, EquipSlot.Face, 0.35f, 100f);
            var geiger = Def("geiger_counter", ItemType.Device, 1, 0.9f);
            inv.Add(food, 5);
            inv.Add(mask, 1);
            inv.Add(geiger, 1);
            inv.Equip(mask);
            InstrumentDevice.DrainBattery(inv.FindSlot("geiger_counter").Device, 0.3f);
            InventorySlot geigerSlot = inv.FindSlot("geiger_counter");

            var state = inv.CaptureState();
            var catalog = new ItemCatalog();
            catalog.RegisterRange(new[] { food, mask, geiger });
            var restored = new InventoryContainer();
            restored.RestoreState(state, id => catalog.Get(id));

            Assert.Equal(30, restored.Capacity);
            Assert.Equal(5, restored.Count(food));
            Assert.NotNull(restored.GetEquipped(EquipSlot.Face));
            var rGeiger = restored.FindSlot("geiger_counter");
            Assert.NotNull(rGeiger.Device);
            Assert.Equal(geigerSlot.Device.Battery, rGeiger.Device.Battery, 3);
        }

        [Fact]
        public void EquipSlots_ParsesCanonicalAndLegacyAliases()
        {
            Assert.True(EquipSlots.TryParse("body", out var body));
            Assert.Equal(EquipSlot.Body, body);
            Assert.True(EquipSlots.TryParse("Torso", out var torso));
            Assert.Equal(EquipSlot.Body, torso);
            Assert.True(EquipSlots.TryParse("", out var none));
            Assert.Equal(EquipSlot.None, none);
            Assert.False(EquipSlots.TryParse("eyeball", out _));
            Assert.False(EquipSlots.IsCanonicalName("Torso"));
            Assert.Equal("Body", EquipSlots.CanonicalNameForAlias("chest"));
        }

        [Fact]
        public void ItemCatalog_RegistersAndResolves()
        {
            var catalog = new ItemCatalog();
            var food = Def("canned_food");
            catalog.Register(food);
            Assert.True(catalog.Contains("canned_food"));
            Assert.Same(food, catalog.Get("canned_food"));
            Assert.Null(catalog.Get("unknown_id"));
        }

        [Fact]
        public void Clear_EmptiesSlotsAndEquipped_ThenReusable()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 50f };
            var food = Def("canned_food", stackMax: 6, weight: 0.5f);
            var mask = Def("gas_mask", equipable: true, slot: EquipSlot.Face, radProt: 0.35f);
            Assert.True(inv.Add(food, 3));
            Assert.True(inv.Add(mask, 1));
            Assert.True(inv.Equip(mask));
            Assert.Single(inv.Slots);
            Assert.Single(inv.Equipped);

            inv.Clear();

            Assert.Empty(inv.Slots);
            Assert.Empty(inv.Equipped);
            Assert.Equal(0, inv.GetCurrentWeight());

            // Reuse after clear.
            Assert.True(inv.Add(food, 2));
            Assert.Single(inv.Slots);
            Assert.Equal(2, inv.CountById("canned_food"));
        }
    }
}
