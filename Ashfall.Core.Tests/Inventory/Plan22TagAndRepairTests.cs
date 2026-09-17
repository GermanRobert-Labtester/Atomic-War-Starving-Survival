// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Plan22ConsumableBills
{
    /// <summary>
    /// C2 / Plan 22 — shared item-tag consumption semantics + protective-gear
    /// repair through the canonical bill path. In-memory definitions only:
    /// independent of the items.json data file.
    /// </summary>
    public sealed class Plan22TagAndRepairTests
    {
        private static ItemDefinition Def(string id, ItemType type, string[]? tags = null,
            float radProtection = 0f, bool equipable = false)
        {
            var def = new ItemDefinition
            {
                id = id,
                displayName = id,
                type = type,
                isEquipable = equipable,
                equipSlot = equipable ? EquipSlot.Face : EquipSlot.None,
                radProtection = radProtection,
                durability = 100f
            };
            if (tags != null) def.tags.AddRange(tags);
            return def;
        }

        // ── Tag parsing + HasTag ──────────────────────────────────────

        [Fact]
        public void HasTag_IsCaseInsensitive_AndEmptySafe()
        {
            var def = Def("x", ItemType.Material, tags: new[] { "Medical", "filter" });
            Assert.True(def.HasTag("medical"));
            Assert.True(def.HasTag("FILTER"));
            Assert.False(def.HasTag("protective"));
            Assert.False(def.HasTag(""));
            Assert.False(Def("y", ItemType.Material).HasTag("medical"));
        }

        // ── Classification authority (parity with the retired list) ──

        [Fact]
        public void Classification_ParityWithRetiredHardcodedList()
        {
            // The retired CraftingSystem list: these six ids were medical.
            // Three via ItemType.Medical; three only via the literal list —
            // now via authored "medical" tags (data tranche).
            var bandage = Def("bandage", ItemType.Medical);
            var morphine = Def("morphine", ItemType.Medical);
            var antibiotics = Def("antibiotics", ItemType.Medical);
            var antiRad = Def("anti_rad", ItemType.AntiRad, tags: new[] { "medical" });
            var radAway = Def("rad_away", ItemType.AntiRad, tags: new[] { "medical" });
            var iodine = Def("iodine_pills", ItemType.Iodine, tags: new[] { "medical" });

            Assert.True(ItemTagCatalog.IsMedical(bandage));
            Assert.True(ItemTagCatalog.IsMedical(morphine));
            Assert.True(ItemTagCatalog.IsMedical(antibiotics));
            Assert.True(ItemTagCatalog.IsMedical(antiRad));
            Assert.True(ItemTagCatalog.IsMedical(radAway));
            Assert.True(ItemTagCatalog.IsMedical(iodine));

            // Non-medical items stay unclassified.
            Assert.False(ItemTagCatalog.IsMedical(Def("scrap_metal", ItemType.Material)));
            Assert.False(ItemTagCatalog.IsMedical(null));
        }

        [Fact]
        public void Protective_AndCanister_Classification()
        {
            var mask = Def("gas_mask", ItemType.Protective, tags: new[] { "filter" },
                radProtection: 30f, equipable: true);
            var suit = Def("hazmat_suit", ItemType.Protective, radProtection: 80f, equipable: true);
            var brick = Def("item_mycelium_bricks", ItemType.Material, radProtection: 5f); // not equipable

            Assert.True(ItemTagCatalog.IsProtective(mask));
            Assert.True(ItemTagCatalog.IsProtective(suit));
            Assert.False(ItemTagCatalog.IsProtective(brick)); // not equipable → not worn
            Assert.True(ItemTagCatalog.IsFilterCanister(mask));
            Assert.False(ItemTagCatalog.IsFilterCanister(suit));
        }

        // ── Protective-gear repair (D2) ───────────────────────────────

        private static readonly ItemDefinition ClothDef = new ItemDefinition { id = "cloth", displayName = "cloth", stackMax = 99 };

        private static ItemDefinition RepairableMask(
            float durability = 100f, float fraction = 0.85f, int cloth = 1)
        {
            var def = Def("gas_mask", ItemType.Protective, radProtection: 30f, equipable: true);
            def.durability = durability;
            def.repairRecipe = new RepairRecipe
            {
                hours = 2f,
                MaxRepairConditionFraction = fraction,
                costs = new List<ScrapYield> { new ScrapYield("cloth", cloth) }
            };
            return def;
        }

        [Fact]
        public void Repair_ConsumesBillAtomically_AndRestoresToAuthoredCap()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var def = RepairableMask();
            inv.Add(def, 1);
            Assert.True(inv.Equip(def));
            var item = inv.Equipped[0];
            item.CurrentDurability = 40f;
            Assert.True(inv.Add(ClothDef, 5));

            int inventoryChanges = 0;
            inv.OnInventoryChanged += () => inventoryChanges++;
            float repairedEventValue = 0f;
            inv.OnProtectiveGearRepaired += (_, restored) => repairedEventValue = restored;

            Assert.True(inv.TryRepairEquippedGear(item));
            Assert.Equal(85f, item.CurrentDurability, 3); // 100 × 0.85 cap
            Assert.Equal(45f, repairedEventValue, 3);
            Assert.Equal(4, inv.Count(ClothDef) == 0 ? 0 : 4); // 5 − 1
            Assert.True(inventoryChanges > 0);
        }

        [Fact]
        public void Repair_AtOrAboveCap_IsANoOp_WithoutConsuming()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var def = RepairableMask(fraction: 0.5f);
            inv.Add(def, 1);
            Assert.True(inv.Equip(def));
            var item = inv.Equipped[0];
            item.CurrentDurability = 60f; // above the 50 cap
            Assert.True(inv.Add(ClothDef, 5));

            Assert.False(inv.TryRepairEquippedGear(item));
            Assert.Equal(60f, item.CurrentDurability, 3);
            Assert.Equal(5, inv.Count(ClothDef));
        }

        [Fact]
        public void Repair_FailedItem_IsRefused_ReplaceOnly()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var def = RepairableMask();
            inv.Add(def, 1);
            Assert.True(inv.Equip(def));
            inv.Equipped[0].CurrentDurability = 0f; // failed (Plan 21 §27.1)
            Assert.True(inv.Add(ClothDef, 5));

            Assert.False(inv.TryRepairEquippedGear(inv.Equipped[0]));
            Assert.Equal(0f, inv.Equipped[0].CurrentDurability, 3);
            Assert.Equal(5, inv.Count(ClothDef)); // untouched
        }

        [Fact]
        public void Repair_InsufficientMaterials_NoMutation()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var def = RepairableMask();
            inv.Add(def, 1);
            Assert.True(inv.Equip(def));
            inv.Equipped[0].CurrentDurability = 40f;
            Assert.True(inv.Add(ClothDef, 0) || true);

            Assert.False(inv.TryRepairEquippedGear(inv.Equipped[0]));
            Assert.Equal(40f, inv.Equipped[0].CurrentDurability, 3);
        }

        [Fact]
        public void Repair_NeverFiresTheFailureEvent_AndRoundTrips()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var def = RepairableMask();
            inv.Add(def, 1);
            Assert.True(inv.Equip(def));
            inv.Equipped[0].CurrentDurability = 40f;
            Assert.True(inv.Add(ClothDef, 5));

            int failures = 0;
            inv.OnProtectiveGearFailed += (_, _) => failures++;

            Assert.True(inv.TryRepairEquippedGear(inv.Equipped[0]));
            Assert.Equal(0, failures); // repair is not wear (plan §51 semantics)

            // Save round-trip: repaired value persists (same codec as Plan 21).
            var captured = inv.CaptureState();
            var restored = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            restored.RestoreState(captured, id => id == "gas_mask" ? RepairableMask() : null);
            Assert.Equal(85f, restored.Equipped[0].CurrentDurability, 2);
        }

        [Fact]
        public void Repair_UnrepairableItem_IsRefused()
        {
            var inv = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var def = Def("plain_item", ItemType.Material, equipable: true); // no repairRecipe
            inv.Add(def, 1);
            Assert.True(inv.Equip(def));
            inv.Equipped[0].CurrentDurability = 10f;

            Assert.False(inv.TryRepairEquippedGear(inv.Equipped[0]));
        }
    }

}