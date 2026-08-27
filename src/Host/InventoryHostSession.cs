using System;
#pragma warning disable CS8618
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Inventory (ported from the legacy
    /// engine host). Engine-agnostic inventory surface. Wraps an Inventory container + ItemCatalog,
    /// loads item definitions (catalog or seed), and persists to user:// via
    /// InventorySaveStore. No gameplay rules — hosts only present.
    /// </summary>
    public sealed class InventoryHostSession
    : HostSessionBase{
        public InventoryContainer Inventory { get; }
        public ItemCatalog Catalog { get; }

        public string LastEvent { get; private set; } = string.Empty;
        public InventoryHostSession(InventoryContainer inventory = null!, ItemCatalog catalog = null!)
        {
            Inventory = inventory ?? new InventoryContainer();
            Catalog = catalog ?? new ItemCatalog();
            // A bare session (tests / direct construction) needs the catalog
            // seeded too, or RestoreSave cannot resolve item ids.
            if (Catalog.Count == 0)
                SeedCatalog(Catalog);
            Inventory.OnInventoryChanged += () => RaiseStateChanged();
        }

        public static InventoryHostSession Create(string dataDir)
        {
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            var session = new InventoryHostSession(null!, catalog);

            var save = InventorySaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Inventory state restored from save.";
            }
            else
            {
                session.LoadOrSeedStartingSupplies(dataDir, fileIO, serializer);
            }
            return session;
        }

        public void LoadOrSeedStartingSupplies(string dataDir, IFileIO fileIO = null!, IJsonSerializer serializer = null!)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            var supplies = ItemCatalogLoader.LoadStartingSupplies(dataDir, fileIO, serializer);
            if (supplies != null && supplies.Count > 0)
            {
                for (int i = 0; i < supplies.Count; i++)
                {
                    Add(supplies[i].itemId, supplies[i].amount);
                }
                LastEvent = "Starting supplies loaded into Holdfast storage from JSON authority.";
            }
            else
            {
                SeedStartingSupplies();
            }
        }

        public void SeedStartingSupplies()
        {
            Add("clean_water", 12);
            Add("canned_food", 16);
            Add("irradiated_water", 4);
            Add("item_air_filter_hepa", 2);
            Add("item_desal_membrane", 1);
            Add("iodine_pills", 4);
            Add("bandage", 2);
            Add("rad_away", 1);
            Add("item_dosimeter_pen", 1);
            Add("item_geiger_m3", 1);
            Add("gas_mask", 1);
            Add("hazmat_suit", 1);
            Add("battery", 4);
            Add("scrap_mechanical", 6);
            Add("scrap_electronic", 3);
            LastEvent = "Starting supplies loaded into Holdfast storage.";
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
                MakeDef("gas_mask", "Gas Mask", ItemType.Protective, stackMax: 1, weight: 1.5f, equip: true, equipSlot: EquipSlot.Face, radProt: 30f, durability: 100f, trade: 40f),
                MakeDef("hazmat_suit", "Hazmat Suit", ItemType.Protective, stackMax: 1, weight: 5f, equip: true, equipSlot: EquipSlot.Body, radProt: 80f, durability: 100f, trade: 40f),
                MakeDef("geiger_counter", "Geiger Counter", ItemType.Device, stackMax: 1, weight: 0.9f, trade: 25f),
                MakeDef("item_geiger_m3", "Geiger Counter (M3)", ItemType.Device, stackMax: 1, weight: 0.9f, trade: 35f),
                MakeDef("item_dosimeter_pen", "Quartz Pen Dosimeter", ItemType.Device, stackMax: 2, weight: 0.1f, trade: 15f),
                MakeDef("item_air_filter_hepa", "HEPA Filtration Core", ItemType.Material, stackMax: 4, weight: 1.5f, trade: 25f),
                MakeDef("item_desal_membrane", "Desalination Membrane", ItemType.Material, stackMax: 2, weight: 0.8f, trade: 30f),
                MakeDef("item_seed_mushroom", "Mushroom Spores", ItemType.Material, stackMax: 20, weight: 0.05f, trade: 5f),
                MakeDef("item_seed_tuber", "Tuber Eyes", ItemType.Material, stackMax: 20, weight: 0.1f, trade: 6f),
                MakeDef("item_seed_grain", "Hardy Grain Seeds", ItemType.Material, stackMax: 20, weight: 0.05f, trade: 8f),
                MakeDef("item_seed_wheat", "Pre-War Wheat Seeds", ItemType.Material, stackMax: 20, weight: 0.05f, trade: 15f),
                MakeDef("crop_mushroom", "Harvested Fungal Caps", ItemType.Food, stackMax: 10, weight: 0.2f, hunger: 25f, trade: 6f),
                MakeDef("crop_tuber", "Harvested Tubers", ItemType.Food, stackMax: 10, weight: 0.4f, hunger: 35f, trade: 8f),
                MakeDef("crop_grain", "Harvested Grain", ItemType.Food, stackMax: 10, weight: 0.3f, hunger: 30f, trade: 10f),
                MakeDef("crop_wheat", "Harvested Pre-War Wheat", ItemType.Food, stackMax: 10, weight: 0.3f, hunger: 50f, trade: 20f),
                MakeDef("item_blight_treatment", "Antifungal Chemical Wash", ItemType.Material, stackMax: 5, weight: 0.5f, trade: 15f),
                MakeDef("scrap_mechanical", "Mechanical Parts", ItemType.Material, stackMax: 50, weight: 0.2f, trade: 2f),
                MakeDef("scrap_electronic", "Electronic Scrap", ItemType.Material, stackMax: 50, weight: 0.1f, trade: 3f),
                MakeDef("scrap_chemical", "Chemicals", ItemType.Material, stackMax: 50, weight: 0.3f, trade: 4f),

                // ── ASHFALL: THE VERDICT (Expansion 08) — narrative evidence & quest items ──
                // Evidence is authoritative in the Verdict EvidenceLedger; these ItemDefinitions
                // make the fragments physically present so the inventory surface and quest
                // grantItemId references resolve without forking a parallel item system.
                MakeDef("evidence_geophone_hymn", "The Farm's Seismic Signature", ItemType.Relic, weight: 1.2f, trade: 12f),
                MakeDef("evidence_twelve_gauge_steel", "The Fired-Plate Ordnance Log", ItemType.Relic, weight: 4f, trade: 18f),
                MakeDef("evidence_fuse_linen", "The Standard's Linen", ItemType.Relic, weight: 0.4f, trade: 40f),
                MakeDef("evidence_census_draft", "The Partial County Ledger", ItemType.Relic, weight: 0.8f, trade: 25f),
                MakeDef("evidence_mailroom_tape", "Carbon-Copy Censusing Rota", ItemType.Relic, weight: 0.3f, trade: 30f),
                MakeDef("evidence_uxo_register", "The Hold Register", ItemType.Relic, weight: 1.6f, trade: 55f),
                MakeDef("evidence_call_calibration", "The Calibration Burst", ItemType.Relic, weight: 0.2f, trade: 45f),
                MakeDef("evidence_call_plain", "The Plain Burst", ItemType.Relic, weight: 0.2f, trade: 50f),
                MakeDef("evidence_reels_matter", "The Archive's Own Accounting", ItemType.Relic, weight: 2f, trade: 35f),
                MakeDef("evidence_valve_s36", "The Valve, Read Per 36", ItemType.Relic, weight: 3f, trade: 60f),
                MakeDef("evidence_eden_log", "Eleven Months of Tube-Bleed", ItemType.Relic, weight: 1.1f, trade: 70f),
                MakeDef("evidence_veen_your_people", "The Count, Presented", ItemType.Relic, weight: 0.5f, trade: 0f),
                MakeDef("item_archive_tape_silo_key", "The Tape-Silo Key", ItemType.Quest, weight: 0.9f, trade: 90f),
                MakeDef("item_fuse_world_shift_charter", "Shift 36's Charter", ItemType.Quest, weight: 1.4f, trade: 75f),
                MakeDef("item_verdict_salt_flat_sample", "Salt Flat Sample", ItemType.Comfort, weight: 1f, morale: 2f, trade: 8f),

                // ── ASHFALL: THE BLACK FLOTILLA (Expansion 09) — salvage & deep-lore items ──
                MakeDef("paper_scrap", "Paper Scrap", ItemType.Material, stackMax: 100, weight: 0.01f, trade: 0.1f),
                MakeDef("item_suitcase_locked", "Locked Suitcase", ItemType.Tool, weight: 4f, trade: 5f),
                MakeDef("industrial_bleach", "Industrial Bleach (5L)", ItemType.Material, stackMax: 2, weight: 5.5f, trade: 18f),
                MakeDef("ammonia_tank", "Pressurized Ammonia Tank", ItemType.Material, weight: 12f, trade: 25f),
                MakeDef("item_anchor_notes", "The Anchor's Final Script", ItemType.Quest, weight: 0.1f, trade: 0f),
                MakeDef("halon_tank", "Halon Fire Suppressant", ItemType.Material, weight: 15f, trade: 40f),
                MakeDef("pipe_wrench", "Heavy Pipe Wrench", ItemType.Tool, weight: 3.5f, trade: 14f),
                MakeDef("crayon", "Crayon", ItemType.Comfort, stackMax: 30, weight: 0.02f, trade: 0.5f),
                MakeDef("sawdust_block", "Compressed Sawdust Block", ItemType.Fuel, stackMax: 50, weight: 1f, trade: 2f),
                MakeDef("item_ash_ghillie", "Ash Ghillie Suit", ItemType.Protective, weight: 3f, trade: 20f),
                MakeDef("item_teddy_bear", "Teddy Bear", ItemType.Comfort, stackMax: 4, weight: 0.3f, morale: 1f, trade: 2f),
                MakeDef("item_car_keys", "Car Keys", ItemType.Material, stackMax: 5, weight: 0.05f, trade: 1f),
                MakeDef("item_ice_pick", "Ice Pick", ItemType.Tool, stackMax: 2, weight: 0.8f, trade: 10f),
                MakeDef("brass_fittings", "Brass Fittings", ItemType.Material, stackMax: 30, weight: 0.3f, trade: 8f),

                // ── ASHFALL: THE BLACK FLOTILLA (Expansion 09) — phantom loot-table items ──
                // These were referenced in DeepLoreLocationCatalog loot tables but never registered.
                MakeDef("acoustic_foam_panel", "Acoustic Foam Panel", ItemType.Material, stackMax: 10, weight: 0.5f, trade: 3f),
                MakeDef("ammo_9x19", "9x19mm Ammunition", ItemType.Material, stackMax: 20, weight: 0.01f, trade: 2f),
                MakeDef("blood_bag", "Blood Bag (Compatible)", ItemType.Medical, weight: 0.6f, health: 15f, trade: 30f),
                MakeDef("bone_saw", "Bone Saw", ItemType.Tool, weight: 2.5f, trade: 12f),
                MakeDef("cardboard_box", "Cardboard Box", ItemType.Material, stackMax: 20, weight: 0.3f, trade: 1f),
                MakeDef("cigarette_pack_sealed", "Sealed Cigarette Pack", ItemType.Comfort, stackMax: 5, weight: 0.05f, morale: 1f, trade: 8f),
                MakeDef("fat_rendered", "Rendered Fat", ItemType.Food, stackMax: 10, weight: 0.5f, hunger: 8f, trade: 3f),
                MakeDef("spoiled_blood_bag", "Spoiled Blood Bag", ItemType.ContaminatedFood, weight: 0.6f, trade: 0f),
                MakeDef("spoiled_canned_food", "Spoiled Canned Food", ItemType.ContaminatedFood, weight: 0.4f, trade: 0f),
                MakeDef("spoiled_meat", "Spoiled Meat", ItemType.ContaminatedFood, stackMax: 20, weight: 0.3f, trade: 0f),

                // ── ASHFALL: THE GLASS ORCHARD (Expansion 11) — greenhouse items ──
                MakeDef("item_seed_mushroom", "Spore Capsule", ItemType.Material, stackMax: 20, weight: 0.1f, trade: 4f),
                MakeDef("item_seed_tuber", "Tuber Cutting", ItemType.Material, stackMax: 10, weight: 0.3f, trade: 6f),
                MakeDef("item_seed_grain", "Mutated Grain Spike", ItemType.Material, stackMax: 30, weight: 0.05f, trade: 5f),
                MakeDef("item_seed_wheat", "Pre-War Wheat Seed", ItemType.Quest, stackMax: 5, weight: 0.05f, trade: 80f),
                MakeDef("item_planter_box", "Planter Box", ItemType.Material, stackMax: 6, weight: 8f, trade: 25f),
                MakeDef("item_grow_lamp", "Grow Lamp", ItemType.Device, stackMax: 2, weight: 3f, trade: 40f),
                MakeDef("item_lead_glass_pane", "Lead-Glass Pane", ItemType.Material, stackMax: 4, weight: 5f, trade: 30f),
                MakeDef("item_blight_treatment", "Blight Treatment", ItemType.Medical, stackMax: 6, weight: 0.2f, trade: 15f),
                MakeDef("item_grow_medium", "Sterile Grow Medium", ItemType.Material, stackMax: 8, weight: 2f, trade: 8f),
                MakeDef("crop_mushroom", "Greenhouse Mushroom", ItemType.Food, stackMax: 20, weight: 0.1f, hunger: 6f, morale: 1f, trade: 3f),
                MakeDef("crop_tuber", "Greenhouse Tuber", ItemType.Food, stackMax: 12, weight: 0.3f, hunger: 12f, thirst: 1f, trade: 5f),
                MakeDef("crop_grain", "Clean Grain", ItemType.Food, stackMax: 30, weight: 0.1f, hunger: 18f, morale: 2f, trade: 6f),
                MakeDef("crop_wheat", "Pre-War Wheat", ItemType.Food, stackMax: 20, weight: 0.2f, hunger: 30f, morale: 8f, health: 2f, trade: 30f),
                MakeDef("tainted_food", "Tainted Rations", ItemType.ContaminatedFood, stackMax: 20, weight: 0.2f, hunger: 14f, morale: -3f, contamination: 0.6f, trade: 1f)
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
