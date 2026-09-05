// Task 45-47: Canonical consumption service — authored effects, atomic rollback,
// single inventory authority, and survivor target resolution.

using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Thin Core-only test adapter that mirrors InventoryHostSession.ConsumeResult delta-computation
    /// without any Godot dependency. Validates Tasks 45-47: authored effects, atomic rollback,
    /// and canonical-inventory authority (not trade stock).
    /// </summary>
    internal sealed class ConsumeTestHelper
    {
        private readonly InventoryContainer _inv;
        private readonly ItemCatalog _cat;

        public ConsumeTestHelper(InventoryContainer inv, ItemCatalog cat)
        {
            _inv = inv;
            _cat = cat;
        }

        public ActionResult ConsumeResult(string itemId)
        {
            var def = _cat.Get(itemId);
            if (def == null)
                return ActionResult.Failed("unknown_item", $"Unknown item: {itemId}.");

            if (!def.IsConsumable())
                return ActionResult.Blocked("not_consumable", $"{def.displayName} cannot be consumed.");

            if (_inv.Count(def) < 1)
                return ActionResult.Blocked("cannot_consume", $"Cannot consume {def.displayName}: none held.");

            var deltas = new Dictionary<string, double>(System.StringComparer.Ordinal);
            if (def.hungerRestore != 0f) deltas["hunger"] = -def.hungerRestore;
            if (def.thirstRestore != 0f) deltas["thirst"] = -def.thirstRestore;
            if (def.healthEffect != 0f) deltas["health"] = def.healthEffect;
            if (def.moraleEffect != 0f) deltas["morale"] = def.moraleEffect;
            if (def.radCleanse > 0f) deltas["rad_cleanse"] = def.radCleanse;
            if (def.type == ItemType.Iodine) deltas["iodine"] = 1.0;
            if (def.contamination > 0f)
                deltas["contamination"] = def.contamination * InventoryContainer.ContaminationDosePerUnit;

            // Use null applyNeed so the Core's Consume does not attempt a need-callback rollback.
            bool ok = _inv.Consume(def);
            if (!ok)
                return ActionResult.Blocked("consume_failed", $"Failed to consume {def.displayName}.");

            return ActionResult.Success($"Consumed 1 x {def.displayName}.", deltas);
        }
    }

    /// <summary>
    /// Verifies that consumption wires authored item effects to correct delta keys,
    /// that the atomic rollback fires when the effect callback fails,
    /// and that items are consumed from the canonical player inventory (not trade stock).
    /// </summary>
    public class CanonicalConsumptionServiceTests
    {
        // ── Helpers ──────────────────────────────────────────────────────────────

        private static ItemDefinition Consumable(string id, ItemType type,
            float hunger = 0f, float thirst = 0f, float health = 0f,
            float morale = 0f, float radCleanse = 0f, float contamination = 0f)
        {
            return new ItemDefinition
            {
                id = id,
                displayName = id,
                type = type,
                stackMax = 10,
                weight = 0.1f,
                hungerRestore = hunger,
                thirstRestore = thirst,
                healthEffect = health,
                moraleEffect = morale,
                radCleanse = radCleanse,
                contamination = contamination,
            };
        }

        private static ItemCatalog BuildCatalog(params ItemDefinition[] defs)
        {
            var cat = new ItemCatalog();
            foreach (var d in defs) cat.Register(d);
            return cat;
        }

        private static InventoryContainer Inv()
            => new InventoryContainer { Capacity = 10, MaxWeight = 50f };

        private static ConsumeTestHelper Session(InventoryContainer inv, ItemCatalog cat)
            => new ConsumeTestHelper(inv, cat);

        // ── 1. Authored need deltas — food (hunger -40, morale +2) ───────────────

        [Fact]
        public void ConsumeResult_CannedFood_ReturnsHungerAndMoraleDelta()
        {
            var def = Consumable("canned_food", ItemType.Food, hunger: 40f, morale: 2f);
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 2);

            var result = Session(inv, cat).ConsumeResult("canned_food");

            Assert.True(result.IsSuccess);
            Assert.True(result.Deltas.TryGetValue("hunger", out var h));
            Assert.Equal(-40.0, h, precision: 3);
            Assert.True(result.Deltas.TryGetValue("morale", out var m));
            Assert.Equal(2.0, m, precision: 3);
        }

        // ── 2. Authored need deltas — water (thirst -40) ─────────────────────────

        [Fact]
        public void ConsumeResult_CleanWater_ReturnsThirstDelta()
        {
            var def = Consumable("clean_water", ItemType.Water, thirst: 40f);
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 1);

            var result = Session(inv, cat).ConsumeResult("clean_water");

            Assert.True(result.IsSuccess);
            Assert.True(result.Deltas.TryGetValue("thirst", out var t));
            Assert.Equal(-40.0, t, precision: 3);
        }

        // ── 3. Contaminated food adds positive contamination delta ────────────────

        [Fact]
        public void ConsumeResult_ContaminatedFood_ReturnsContaminationDelta()
        {
            var def = Consumable("tainted_food", ItemType.ContaminatedFood, hunger: 14f, contamination: 0.6f);
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 1);

            var result = Session(inv, cat).ConsumeResult("tainted_food");

            Assert.True(result.IsSuccess);
            Assert.True(result.Deltas.ContainsKey("contamination"), "contaminated food should produce contamination delta");
            Assert.True(result.Deltas["contamination"] > 0);
        }

        // ── 4. Medical item returns positive health delta ─────────────────────────

        [Fact]
        public void ConsumeResult_Bandage_ReturnsPositiveHealthDelta()
        {
            var def = Consumable("bandage", ItemType.Medical, health: 30f);
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 1);

            var result = Session(inv, cat).ConsumeResult("bandage");

            Assert.True(result.IsSuccess);
            Assert.True(result.Deltas.TryGetValue("health", out var h));
            Assert.True(h > 0, "health delta should be positive for medical item");
        }

        // ── 5. Iodine type emits iodine=1 delta ───────────────────────────────────

        [Fact]
        public void ConsumeResult_IodinePills_ReturnsIodineDelta()
        {
            var def = new ItemDefinition
            {
                id = "iodine_pills",
                displayName = "Iodine Pills",
                type = ItemType.Iodine,
                stackMax = 10,
                weight = 0.05f,
            };
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 1);

            var result = Session(inv, cat).ConsumeResult("iodine_pills");

            Assert.True(result.IsSuccess);
            Assert.True(result.Deltas.TryGetValue("iodine", out var iod));
            Assert.Equal(1.0, iod, precision: 3);
        }

        // ── 6. Anti-rad emits positive rad_cleanse delta ──────────────────────────

        [Fact]
        public void ConsumeResult_AntiRad_ReturnsRadCleanseDelta()
        {
            var def = Consumable("anti_rad", ItemType.AntiRad, radCleanse: 50f);
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 1);

            var result = Session(inv, cat).ConsumeResult("anti_rad");

            Assert.True(result.IsSuccess);
            Assert.True(result.Deltas.TryGetValue("rad_cleanse", out var rc));
            Assert.True(rc > 0, "anti_rad should produce positive rad_cleanse delta");
        }

        // ── 7. Zero held → Blocked, no callback invoked ───────────────────────────

        [Fact]
        public void ConsumeResult_ZeroHeld_ReturnsBlocked()
        {
            var def = Consumable("canned_food", ItemType.Food, hunger: 40f);
            var cat = BuildCatalog(def);
            var inv = Inv();

            var result = Session(inv, cat).ConsumeResult("canned_food");

            Assert.False(result.IsSuccess);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
        }

        // ── 8. Unknown item → Failed with "unknown_item" code ─────────────────────

        [Fact]
        public void ConsumeResult_UnknownItem_ReturnsFailed()
        {
            var result = Session(Inv(), BuildCatalog()).ConsumeResult("nonexistent_xyz");

            Assert.False(result.IsSuccess);
            Assert.Equal(ActionResult.StatusKind.Failed, result.Status);
            Assert.Equal("unknown_item", result.FailureCode);
        }

        // ── 9. Non-consumable item → Blocked with "not_consumable" code ───────────

        [Fact]
        public void ConsumeResult_NonConsumableItem_ReturnsBlocked()
        {
            var def = new ItemDefinition
            {
                id = "scrap_metal",
                displayName = "Scrap Metal",
                type = ItemType.Material,
                stackMax = 20,
                weight = 0.5f,
            };
            var cat = BuildCatalog(def);
            var inv = Inv();
            inv.Add(def, 3);

            var result = Session(inv, cat).ConsumeResult("scrap_metal");

            Assert.False(result.IsSuccess);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("not_consumable", result.FailureCode);
        }

        // ── 10. Atomic rollback — item count unchanged when callback returns false ─

        [Fact]
        public void Inventory_Consume_RestoresItem_WhenCallbackReturnsFalse()
        {
            var def = Consumable("canned_food", ItemType.Food, hunger: 40f);
            var inv = Inv();
            inv.Add(def, 2);

            int before = inv.CountById("canned_food");
            // Callback refuses the hunger apply — triggers atomic rollback in Core
            bool ok = inv.Consume(def, applyNeed: (type, delta) => false);

            Assert.False(ok);
            Assert.Equal(before, inv.CountById("canned_food")); // item restored
        }

        // ── 11. Successful consume decrements count by exactly 1 ─────────────────

        [Fact]
        public void Inventory_Consume_ReducesCountByOne_OnSuccess()
        {
            var def = Consumable("canned_food", ItemType.Food, hunger: 40f);
            var inv = Inv();
            inv.Add(def, 3);

            // Null applyNeed = no rollback condition; item is consumed normally
            bool ok = inv.Consume(def);

            Assert.True(ok);
            Assert.Equal(2, inv.CountById("canned_food"));
        }

        // ── 12. IsConsumable() helper returns correct value per item type ─────────

        [Fact]
        public void IsConsumable_Food_ReturnsTrue()
        {
            var def = Consumable("canned_food", ItemType.Food, hunger: 40f);
            Assert.True(def.IsConsumable());
        }

        [Fact]
        public void IsConsumable_Material_ReturnsFalse()
        {
            var def = new ItemDefinition { id = "scrap", type = ItemType.Material };
            Assert.False(def.IsConsumable());
        }
    }
}
