using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Inventory (ported from Unity's
    /// AtomicWar._Game.Inventory). Wraps an Inventory container + ItemCatalog,
    /// loads item definitions (catalog or seed), and persists to user:// via
    /// InventorySaveStore. No gameplay rules — hosts only present.
    /// </summary>
    public sealed class InventoryHostSession
    {
        public InventoryContainer Inventory { get; }
        public ItemCatalog Catalog { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public InventoryHostSession(InventoryContainer inventory = null, ItemCatalog catalog = null)
        {
            Inventory = inventory ?? new InventoryContainer();
            Catalog = catalog ?? new ItemCatalog();
            // A bare session (tests / direct construction) needs the catalog
            // seeded too, or RestoreSave cannot resolve item ids.
            if (Catalog.Count == 0)
                SeedCatalog(Catalog);
            Inventory.OnInventoryChanged += () => StateChanged?.Invoke();
        }

        public static InventoryHostSession Create(string dataDir)
        {
            var session = new InventoryHostSession();

            var save = InventorySaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Inventory state restored from save.";
            }
            return session;
        }

        /// <summary>
        /// Seed a playable demo catalog when no items.json is bound. Ids follow the
        /// canonical snake_case master list (AGENTS.md id rule).
        /// </summary>
        private static void SeedCatalog(ItemCatalog catalog)
        {
            var defs = new[]
            {
                MakeDef("canned_food", "Canned Food", ItemType.Food, stackMax: 6, weight: 0.5f, hunger: 40f, trade: 6f),
                MakeDef("clean_water", "Clean Water", ItemType.Water, stackMax: 4, weight: 0.8f, thirst: 50f, trade: 8f),
                MakeDef("irradiated_water", "Irradiated Water", ItemType.IrradiatedWater, stackMax: 4, weight: 0.8f, thirst: 40f, trade: 1f, contamination: 0.6f),
                MakeDef("bandage", "Bandage", ItemType.Medical, stackMax: 8, weight: 0.1f, health: 10f, trade: 5f),
                MakeDef("iodine_pills", "Iodine Pills", ItemType.Iodine, stackMax: 5, weight: 0.05f, trade: 12f),
                MakeDef("rad_away", "Rad-Away", ItemType.AntiRad, stackMax: 3, weight: 0.2f, radCleanse: 30f, trade: 20f),
                MakeDef("fuel_canister", "Fuel Canister", ItemType.Fuel, stackMax: 3, weight: 4f, trade: 15f),
                MakeDef("battery", "Battery", ItemType.Tool, stackMax: 10, weight: 0.1f, trade: 4f),
                MakeDef("calibration_kit", "Calibration Kit", ItemType.Tool, stackMax: 4, weight: 0.3f, trade: 18f),
                MakeDef("gas_mask", "Gas Mask", ItemType.Protective, stackMax: 1, weight: 1.2f, equip: true, equipSlot: EquipSlot.Face, radProt: 0.35f, durability: 100f, trade: 30f),
                MakeDef("hazmat_suit", "Hazmat Suit", ItemType.Protective, stackMax: 1, weight: 3f, equip: true, equipSlot: EquipSlot.Body, radProt: 0.55f, durability: 100f, trade: 45f),
                MakeDef("geiger_counter", "Geiger Counter", ItemType.Device, stackMax: 1, weight: 0.9f, trade: 25f),
                MakeDef("scrap_mechanical", "Mechanical Parts", ItemType.Material, stackMax: 50, weight: 0.2f, trade: 2f),
                MakeDef("scrap_electronic", "Electronic Scrap", ItemType.Material, stackMax: 50, weight: 0.1f, trade: 3f),
                MakeDef("scrap_chemical", "Chemicals", ItemType.Material, stackMax: 50, weight: 0.3f, trade: 4f)
            };
            foreach (var d in defs) catalog.Register(d);
        }

        private static ItemDefinition MakeDef(
            string id, string name, ItemType type,
            int stackMax = 1, float weight = 1f,
            float hunger = 0f, float thirst = 0f, float health = 0f,
            float morale = 0f, float radCleanse = 0f, float contamination = 0f,
            bool equip = false, EquipSlot equipSlot = EquipSlot.None,
            float radProt = 0f, float durability = 0f, float trade = 0f)
        {
            return new ItemDefinition
            {
                id = id,
                displayName = name,
                type = type,
                stackMax = stackMax,
                weight = weight,
                hungerRestore = hunger,
                thirstRestore = thirst,
                healthEffect = health,
                moraleEffect = morale,
                radCleanse = radCleanse,
                contamination = contamination,
                isEquipable = equip,
                equipSlot = equipSlot,
                radProtection = radProt,
                durability = durability,
                tradeValue = trade
            };
        }

        // ── Item operations ────────────────────────────────────────────

        public string Add(string itemId, int amount)
        {
            var def = Catalog.Get(itemId);
            if (def == null) return $"Unknown item: {itemId}.";
            bool ok = Inventory.Add(def, amount);
            LastEvent = ok
                ? $"Added {amount} × {def.displayName}."
                : $"Cannot add {amount} × {def.displayName} (weight/capacity/stack limit).";
            return LastEvent;
        }

        public string Remove(string itemId, int amount)
        {
            var def = Catalog.Get(itemId);
            if (def == null) return $"Unknown item: {itemId}.";
            bool ok = Inventory.Remove(def, amount);
            LastEvent = ok
                ? $"Removed {amount} × {def.displayName}."
                : $"Not enough {def.displayName} held.";
            return LastEvent;
        }

        public string Equip(string itemId)
        {
            var def = Catalog.Get(itemId);
            if (def == null) return $"Unknown item: {itemId}.";
            bool ok = Inventory.Equip(def);
            LastEvent = ok
                ? $"Equipped {def.displayName} ({def.equipSlot})."
                : $"Cannot equip {def.displayName}: not equipable, not held, or slot occupied.";
            return LastEvent;
        }

        public string Unequip(string slotName)
        {
            if (!EquipSlots.TryParse(slotName, out EquipSlot slot) || slot == EquipSlot.None)
                return $"Unknown slot: {slotName}.";
            var item = Inventory.Unequip(slot);
            LastEvent = item != null
                ? $"Unequipped {item.displayName}."
                : $"Nothing equipped in {slot} (or bag is full).";
            return LastEvent;
        }

        public string Consume(string itemId, float therapeuticScale = 1f)
        {
            var def = Catalog.Get(itemId);
            if (def == null) return $"Unknown item: {itemId}.";
            bool ok = Inventory.Consume(def, therapeuticScale: therapeuticScale);
            LastEvent = ok
                ? $"Consumed 1 × {def.displayName}."
                : $"Cannot consume {def.displayName}: none held.";
            return LastEvent;
        }

        // ── Status ─────────────────────────────────────────────────────

        public string InventoryLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Inventory: {Inventory.CountById("canned_food")} canned_food · ")
                .Append($"{Inventory.CountById("clean_water")} clean_water · ")
                .Append($"{Inventory.CountById("battery")} battery · ")
                .Append($"weight {Inventory.GetCurrentWeight():F1}/{Inventory.MaxWeight:F0} kg");
            for (int i = 0; i < Inventory.Slots.Count; i++)
            {
                var s = Inventory.Slots[i];
                if (s == null || s.Item == null) continue;
                sb.Append($"\n  {s.Item.id} ×{s.Amount}");
                if (s.Item.type == ItemType.Device && s.Device != null)
                    sb.Append($" [bat {s.Device.Battery:F0}% cal {s.Device.Calibration:F0}%{(s.Device.Broken ? " BROKEN" : "")}]");
            }
            return sb.ToString();
        }

        public string EquipLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("Equipped:");
            if (Inventory.Equipped.Count == 0) sb.Append(" none");
            for (int i = 0; i < Inventory.Equipped.Count; i++)
            {
                var e = Inventory.Equipped[i];
                if (e?.Item == null) continue;
                sb.Append($"\n  [{e.Item.equipSlot}] {e.Item.id} (dur {e.CurrentDurability:F0})");
            }
            sb.Append($"\nProtection: {Inventory.GetEquippedProtection():F2}");
            return sb.ToString();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public InventorySaveState CaptureSave() => Inventory.CaptureState();

        public void RestoreSave(InventorySaveState state) =>
            Inventory.RestoreState(state, id => Catalog.Get(id));
    }
}
