using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// Workbench component economy: disassemble non-consumables into scrap,
    /// repair degraded gear / hard-broken devices, recalibrate instruments with
    /// ElectronicScrap, and install hatch defense upgrades. Reads ScrapValue /
    /// RepairRecipe from ItemDefinition; hatch costs from HatchDefenseSystem.
    /// </summary>
    public class WorkbenchSystem
    {
        public const string StationId = "workbench";
        public const float DefaultDisassembleYield = 0.5f;
        public const int ElectronicScrapForGeigerRecal = 1;
        public const int ElectronicScrapForPurifierRepair = 2;
        public const float PurifierCriticalFilterHealth = 25f;

        private readonly Inventory.Inventory _inventory;
        private readonly Func<string, ItemDefinition> _itemLookup;
        private readonly CraftingSystem _crafting;
        private readonly Func<Shelter.Shelter> _getShelter;
        private readonly Func<int> _getDay;
        private HatchDefenseSystem _hatchDefense;
        private SurvivalPerkSystem _survivalPerks;
        private ShelterPerkSystem _shelterPerks;
        private NeedsSystem _needs;
        public void SetNeedsSystem(NeedsSystem ns) => _needs = ns;
        private ItemDefinition _moonshineDef;
        private ItemDefinition _mutatedFungiDef;
        private ItemDefinition _dirtyWaterDef;
        private ItemDefinition _fuelDef;
        private ItemDefinition _batteryDef;
        private ItemDefinition _springDef;
        private System.Random _rng = new System.Random(198);

        public event Action OnWorkbenchChanged;
        public event Action<ItemDefinition, List<ScrapYield>> OnDisassembled;
        public event Action<ItemDefinition> OnRepaired;
        public event Action<ItemDefinition> OnRecalibrated;
        public event Action<string> OnHatchUpgradeInstalled;
        public event Action<Survivor> OnMoonshineCrafted;
        public event Action<Survivor, bool> OnMoonshineConsumed; // sv, asFuel

        public HatchDefenseSystem HatchDefense => _hatchDefense;

        public const string MoonshineRecipeId = "recipe_moonshine";
        public const int MoonshineFungiCost = 2;
        public const int MoonshineDirtyWaterCost = 1;
        public const float MoonshineCraftHours = 2f;
        public const float MoonshineAsFuelUnits = 1f;

        public WorkbenchSystem(
            Inventory.Inventory inventory,
            Func<string, ItemDefinition> itemLookup,
            CraftingSystem crafting = null,
            Func<Shelter.Shelter> getShelter = null,
            Func<int> getDay = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _itemLookup = itemLookup ?? throw new ArgumentNullException(nameof(itemLookup));
            _crafting = crafting;
            _getShelter = getShelter;
            _getDay = getDay ?? (() => 0);
        }

        /// <summary>Wire hatch defense so the workbench can list install upgrades.</summary>
        public void SetHatchDefense(HatchDefenseSystem hatchDefense)
        {
            _hatchDefense = hatchDefense;
            OnWorkbenchChanged?.Invoke();
        }

        /// <summary>Prompt #191 — Wasteland Brewer moonshine recipe gate.</summary>
        public void BindSurvivalPerks(SurvivalPerkSystem perks, NeedsSystem needs = null)
        {
            _survivalPerks = perks;
            _needs = needs;
        }

        /// <summary>Prompts #195/#198 — scrap substitution + rare component recovery.</summary>
        public void BindShelterPerks(ShelterPerkSystem perks, System.Random rng = null)
        {
            _shelterPerks = perks;
            if (rng != null) _rng = rng;
        }

        public void SetRareComponentItems(ItemDefinition battery, ItemDefinition spring)
        {
            _batteryDef = battery;
            _springDef = spring;
        }

        public static ItemDefinition CreateBatteryDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = ShelterPerkSystem.BatteryId;
            item.displayName = "Battery";
            item.description = "Intact cell. Uncraftable — only recovered from careful teardown.";
            item.type = ItemType.Material;
            item.stackMax = 20;
            item.weight = 0.4f;
            item.tradeValue = 12f;
            return item;
        }

        public static ItemDefinition CreateSpringDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = ShelterPerkSystem.SpringId;
            item.displayName = "Spring";
            item.description = "Tempered coil. Rare salvage — cannot be fabbed from scrap alone.";
            item.type = ItemType.Material;
            item.stackMax = 30;
            item.weight = 0.1f;
            item.tradeValue = 6f;
            return item;
        }

        public void SetMoonshineItems(
            ItemDefinition moonshine,
            ItemDefinition mutatedFungi = null,
            ItemDefinition dirtyWater = null,
            ItemDefinition fuel = null)
        {
            _moonshineDef = moonshine;
            _mutatedFungiDef = mutatedFungi;
            _dirtyWaterDef = dirtyWater;
            _fuelDef = fuel;
        }

        public static ItemDefinition CreateMoonshineDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = SurvivalPerkSystem.MoonshineId;
            item.displayName = "Moonshine";
            item.description =
                "Still-run from mutated fungi and dirty water. Burns as low-grade fuel, " +
                "or drowns the day in a glass — massive morale, massive hangover.";
            item.type = ItemType.Fuel;
            item.stackMax = 10;
            item.weight = 1f;
            item.tradeValue = 8f;
            item.moraleEffect = SurvivalPerkSystem.MoonshineMoraleBoost;
            return item;
        }

        public static ItemDefinition CreateMutatedFungiDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = SurvivalPerkSystem.MutatedFungiId;
            item.displayName = "Mutated Fungi";
            item.description = "Pale fruiting bodies from the planter. Some are dinner. Some are death.";
            item.type = ItemType.Material;
            item.stackMax = 30;
            item.weight = 0.3f;
            item.contamination = 0.15f;
            item.tradeValue = 2f;
            return item;
        }

        // -----------------------------------------------------------------
        // Prompt #191 — Moonshine distillation
        // -----------------------------------------------------------------

        public bool CanCraftMoonshine(Survivor brewer)
        {
            if (brewer == null || !brewer.IsAlive) return false;
            if (_survivalPerks == null || !_survivalPerks.CanCraftMoonshine(brewer)) return false;
            if (!HasOperationalWorkbench()) return false;
            EnsureMoonshineItems();
            if (_moonshineDef == null || !_inventory.CanAdd(_moonshineDef, 1)) return false;
            if (_mutatedFungiDef == null || _inventory.Count(_mutatedFungiDef) < MoonshineFungiCost)
                return false;
            if (_dirtyWaterDef != null)
            {
                if (_inventory.Count(_dirtyWaterDef) < MoonshineDirtyWaterCost) return false;
            }
            return true;
        }

        /// <summary>
        /// Convert MutatedFungi + DirtyWater → Moonshine (requires Wasteland Brewer).
        /// </summary>
        public bool CraftMoonshine(Survivor brewer)
        {
            if (!CanCraftMoonshine(brewer)) return false;
            EnsureMoonshineItems();

            if (!_inventory.Remove(_mutatedFungiDef, MoonshineFungiCost)) return false;
            if (_dirtyWaterDef != null)
                _inventory.Remove(_dirtyWaterDef, MoonshineDirtyWaterCost);

            if (!_inventory.Add(_moonshineDef, 1))
            {
                // Best-effort refund fungi
                _inventory.Add(_mutatedFungiDef, MoonshineFungiCost);
                return false;
            }

            WearStation();
            OnMoonshineCrafted?.Invoke(brewer);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Consume moonshine: asFuel → +low-grade fuel unit; otherwise morale boost + fatigue hangover.
        /// </summary>
        public bool ConsumeMoonshine(Survivor drinker, bool asFuel = false)
        {
            if (drinker == null || !drinker.IsAlive) return false;
            EnsureMoonshineItems();
            if (_moonshineDef == null || _inventory.Count(_moonshineDef) < 1) return false;
            if (!_inventory.Remove(_moonshineDef, 1)) return false;

            if (asFuel)
            {
                if (_fuelDef != null)
                    _inventory.Add(_fuelDef, 1);
                OnMoonshineConsumed?.Invoke(drinker, true);
                OnWorkbenchChanged?.Invoke();
                return true;
            }

            if (_needs != null)
            {
                _needs.Modify(drinker, NeedKind.Morale, SurvivalPerkSystem.MoonshineMoraleBoost);
                _needs.Modify(drinker, NeedKind.Fatigue, SurvivalPerkSystem.MoonshineFatigueHit);
            }
            else
            {
                drinker.Needs.Morale = Mathf.Clamp(
                    drinker.Needs.Morale + SurvivalPerkSystem.MoonshineMoraleBoost, 0f, 100f);
                drinker.Needs.Fatigue = Mathf.Clamp(
                    drinker.Needs.Fatigue + SurvivalPerkSystem.MoonshineFatigueHit, 0f, 100f);
            }

            OnMoonshineConsumed?.Invoke(drinker, false);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        private void EnsureMoonshineItems()
        {
            if (_moonshineDef == null)
            {
                var slot = _inventory.FindSlot(SurvivalPerkSystem.MoonshineId);
                _moonshineDef = slot?.Item ?? CreateMoonshineDefinition();
            }
            if (_mutatedFungiDef == null)
            {
                var slot = _inventory.FindSlot(SurvivalPerkSystem.MutatedFungiId);
                _mutatedFungiDef = slot?.Item ?? CreateMutatedFungiDefinition();
            }
            if (_dirtyWaterDef == null)
            {
                var slot = _inventory.FindSlot(SurvivalPerkSystem.DirtyWaterItemId);
                if (slot?.Item != null) _dirtyWaterDef = slot.Item;
            }
            if (_fuelDef == null)
            {
                var slot = _inventory.FindSlot("fuel");
                if (slot?.Item != null) _fuelDef = slot.Item;
            }
        }

        public bool HasOperationalWorkbench()
        {
            if (_crafting == null) return true; // tests without station gate
            var station = _crafting.GetStation(StationId);
            return station != null && station.IsOperational;
        }

        // -----------------------------------------------------------------
        // Scrap / recipe resolution (dynamic from ItemDefinition + defaults)
        // -----------------------------------------------------------------

        public List<ScrapYield> GetScrapValue(ItemDefinition item)
        {
            var list = new List<ScrapYield>();
            if (item == null) return list;

            if (item.scrapValue != null && item.scrapValue.Count > 0)
            {
                for (int i = 0; i < item.scrapValue.Count; i++)
                {
                    var y = item.scrapValue[i];
                    if (y != null && y.amount > 0 && !string.IsNullOrEmpty(y.materialId))
                        list.Add(y.Clone());
                }
                return list;
            }

            // Type defaults so items.json can omit scrapValue and UI still works
            switch (item.type)
            {
                case ItemType.Device:
                    list.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2));
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    break;
                case ItemType.Tool:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    list.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 1));
                    break;
                case ItemType.Protective:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    list.Add(new ScrapYield(ScrapMaterialIds.Chemicals, 1));
                    break;
                case ItemType.Filter:
                case ItemType.Fuel:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    break;
                case ItemType.Material:
                    if (item.id == "scrap_metal")
                        list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    else if (item.id == "battery")
                        list.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 1));
                    break;
                default:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    break;
            }
            return list;
        }

        public List<ScrapYield> GetDisassembleYield(ItemDefinition item)
        {
            float frac = item != null && item.disassembleYieldFraction > 0f
                ? Mathf.Clamp01(item.disassembleYieldFraction)
                : DefaultDisassembleYield;

            var baseScrap = GetScrapValue(item);
            var yield = new List<ScrapYield>();
            for (int i = 0; i < baseScrap.Count; i++)
            {
                int amount = Mathf.Max(1, Mathf.FloorToInt(baseScrap[i].amount * frac + 0.001f));
                // At least 1 of each material line if base had any
                if (baseScrap[i].amount > 0)
                    amount = Mathf.Max(1, amount);
                if (frac <= 0f) amount = 0;
                if (amount > 0)
                    yield.Add(new ScrapYield(baseScrap[i].materialId, amount));
            }
            return yield;
        }

        public RepairRecipe GetRepairRecipe(ItemDefinition item)
        {
            if (item?.repairRecipe != null && item.repairRecipe.costs != null && item.repairRecipe.costs.Count > 0)
                return item.repairRecipe.Clone();

            var recipe = new RepairRecipe { hours = 0.5f, requiresTools = true };
            // Defaults by type / id
            if (item != null && (item.id == "geiger_counter" || item.id == "dosimeter" || item.type == ItemType.Device))
            {
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2));
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
            }
            else if (item != null && item.type == ItemType.Protective)
            {
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.Chemicals, 1));
            }
            else
            {
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
            }
            return recipe;
        }

        // -----------------------------------------------------------------
        // Disassemble
        // -----------------------------------------------------------------

        public bool CanDisassemble(ItemDefinition item)
        {
            if (item == null || !item.CanDisassemble) return false;
            if (!HasOperationalWorkbench()) return false;
            if (_inventory.Count(item) < 1) return false;
            var yield = GetDisassembleYield(item);
            if (yield.Count == 0) return false;
            for (int i = 0; i < yield.Count; i++)
            {
                var mat = ResolveMaterial(yield[i].materialId);
                if (mat == null || !_inventory.CanAdd(mat, yield[i].amount)) return false;
            }
            return true;
        }

        /// <summary>
        /// Destroy one unit of the item and grant a percentage of its ScrapValue.
        /// </summary>
        public bool Disassemble(ItemDefinition item) => Disassemble(item, null);

        /// <summary>
        /// Disassemble with optional worker for Scrapper milestone (#198) and rare recovery.
        /// </summary>
        public bool Disassemble(ItemDefinition item, Survivor worker)
        {
            if (!CanDisassemble(item)) return false;

            var yield = GetDisassembleYield(item);
            // Pre-check capacity for all yields
            for (int i = 0; i < yield.Count; i++)
            {
                var mat = ResolveMaterial(yield[i].materialId);
                if (mat == null || !_inventory.CanAdd(mat, yield[i].amount)) return false;
            }

            if (!_inventory.Remove(item, 1)) return false;

            for (int i = 0; i < yield.Count; i++)
            {
                var mat = ResolveMaterial(yield[i].materialId);
                _inventory.Add(mat, yield[i].amount);
            }

            // Prompt #198 — Scrapper: high-tier teardown may yield battery/spring.
            if (worker != null && _shelterPerks != null)
            {
                _shelterPerks.RecordDisassemble(worker, 1, _getDay());
                bool highTier = ShelterPerkSystem.IsHighTierDisassembleTarget(item.id)
                    || item.type == ItemType.Weapon
                    || item.type == ItemType.Device;
                string rareId = _shelterPerks.RollRareComponent(worker, highTier, _rng);
                if (!string.IsNullOrEmpty(rareId))
                {
                    var rare = ResolveRareComponent(rareId);
                    if (rare != null && _inventory.CanAdd(rare, 1))
                        _inventory.Add(rare, 1);
                }
            }

            WearStation();
            OnDisassembled?.Invoke(item, yield);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        /// <summary>Disassemble a specific slot (preserves broken device state for flavor only — item is destroyed).</summary>
        public bool DisassembleSlot(InventorySlot slot) => DisassembleSlot(slot, null);

        public bool DisassembleSlot(InventorySlot slot, Survivor worker)
        {
            if (slot?.Item == null) return false;
            return Disassemble(slot.Item, worker);
        }

        // -----------------------------------------------------------------
        // Repair
        // -----------------------------------------------------------------

        public bool CanRepair(InventorySlot slot) => CanRepair(slot, null);

        public bool CanRepair(InventorySlot slot, Survivor worker)
        {
            if (slot?.Item == null) return false;
            if (!HasOperationalWorkbench()) return false;
            if (!slot.IsBrokenOrDegraded()) return false;

            var recipe = GetRepairRecipe(slot.Item);
            if (recipe.requiresTools && !HasOperationalWorkbench()) return false;
            return HasScrapCosts(recipe.costs, worker);
        }

        public bool Repair(InventorySlot slot) => Repair(slot, null);

        public bool Repair(InventorySlot slot, Survivor worker)
        {
            if (!CanRepair(slot, worker)) return false;

            var recipe = GetRepairRecipe(slot.Item);
            if (!ConsumeScrapCosts(recipe.costs, worker)) return false;

            if (slot.Item.type == ItemType.Device)
            {
                if (slot.Device == null) slot.Device = DeviceState.CreateDefault();
                InstrumentDevice.RepairHardFailure(slot.Device, _getDay());
            }
            else if (slot.Item.durability > 0f)
            {
                slot.CurrentDurability = slot.Item.durability;
            }

            WearStation();
            OnRepaired?.Invoke(slot.Item);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        /// <summary>Repair first matching item id in inventory that is broken/degraded.</summary>
        public bool RepairFirst(string itemId)
        {
            var slot = FindBrokenOrDegraded(itemId);
            return slot != null && Repair(slot);
        }

        // -----------------------------------------------------------------
        // Recalibrate (uncalibrated geiger → ElectronicScrap)
        // -----------------------------------------------------------------

        public bool CanRecalibrateGeiger()
        {
            if (!HasOperationalWorkbench()) return false;
            var slot = _inventory.FindSlot("geiger_counter");
            if (slot?.Device == null) return false;
            if (slot.Device.Broken) return false; // hard-broken needs Repair, not recal
            if (slot.Device.Calibration >= InstrumentDevice.ReliableCalibrationThreshold) return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            return scrap != null && _inventory.Count(scrap) >= ElectronicScrapForGeigerRecal;
        }

        public bool RecalibrateGeiger()
        {
            if (!CanRecalibrateGeiger()) return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            if (!_inventory.Remove(scrap, ElectronicScrapForGeigerRecal)) return false;

            var slot = _inventory.FindSlot("geiger_counter");
            InstrumentDevice.Recalibrate(slot.Device, _getDay());
            WearStation();
            OnRecalibrated?.Invoke(slot.Item);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        // -----------------------------------------------------------------
        // Critical module repair (water purifier)
        // -----------------------------------------------------------------

        public bool NeedsElectronicScrapForCriticalRepair()
        {
            int need = GetCriticalElectronicScrapDeficit();
            return need > 0;
        }

        /// <summary>
        /// How many ElectronicScrap units short for critical repairs (purifier + broken geiger).
        /// </summary>
        public int GetCriticalElectronicScrapDeficit()
        {
            int needed = 0;
            var shelter = _getShelter?.Invoke();
            var purifier = shelter?.GetModule("water_purifier");
            if (purifier != null && purifier.FilterHealth <= PurifierCriticalFilterHealth)
                needed += ElectronicScrapForPurifierRepair;

            var geiger = _inventory.FindSlot("geiger_counter");
            if (geiger?.Device != null && geiger.Device.Broken)
                needed += 2; // matches default repair recipe

            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            int have = scrap != null ? _inventory.Count(scrap) : 0;
            return Mathf.Max(0, needed - have);
        }

        public bool CanRepairWaterPurifier()
        {
            if (!HasOperationalWorkbench()) return false;
            var shelter = _getShelter?.Invoke();
            var purifier = shelter?.GetModule("water_purifier");
            if (purifier == null) return false;
            if (purifier.FilterHealth > PurifierCriticalFilterHealth && purifier.FilterHealth >= 99f)
                return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            return scrap != null && _inventory.Count(scrap) >= ElectronicScrapForPurifierRepair;
        }

        public bool RepairWaterPurifier()
        {
            if (!CanRepairWaterPurifier()) return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            if (!_inventory.Remove(scrap, ElectronicScrapForPurifierRepair)) return false;
            var purifier = _getShelter?.Invoke()?.GetModule("water_purifier");
            if (purifier == null) return false;
            purifier.FilterHealth = 100f;
            purifier.IsEnabled = true;
            WearStation();
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        // -----------------------------------------------------------------
        // UI helpers
        // -----------------------------------------------------------------

        public List<WorkbenchLine> BuildLines()
        {
            var lines = new List<WorkbenchLine>();
            var slots = _inventory.Slots;
            if (slots == null) return lines;

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot?.Item == null) continue;
                var item = slot.Item;

                if (item.CanDisassemble)
                {
                    var yield = GetDisassembleYield(item);
                    lines.Add(new WorkbenchLine
                    {
                        Kind = WorkbenchActionKind.Disassemble,
                        Item = item,
                        SlotIndex = i,
                        Label = $"Disassemble {item.displayName}",
                        CostSummary = FormatYield(yield),
                        CanExecute = CanDisassemble(item)
                    });
                }

                if (slot.IsBrokenOrDegraded())
                {
                    var recipe = GetRepairRecipe(item);
                    lines.Add(new WorkbenchLine
                    {
                        Kind = WorkbenchActionKind.Repair,
                        Item = item,
                        SlotIndex = i,
                        Label = $"Repair {item.displayName}",
                        CostSummary = FormatCosts(recipe.costs),
                        CanExecute = CanRepair(slot)
                    });
                }
            }

            var geiger = _inventory.FindSlot("geiger_counter");
            if (geiger?.Device != null
                && !geiger.Device.Broken
                && geiger.Device.Calibration < InstrumentDevice.ReliableCalibrationThreshold)
            {
                lines.Add(new WorkbenchLine
                {
                    Kind = WorkbenchActionKind.Recalibrate,
                    Item = geiger.Item,
                    SlotIndex = -1,
                    Label = "Recalibrate Geiger Counter",
                    CostSummary = $"{ElectronicScrapForGeigerRecal}x electronic_scrap",
                    CanExecute = CanRecalibrateGeiger()
                });
            }

            if (CanRepairWaterPurifier() || NeedsElectronicScrapForCriticalRepair())
            {
                var shelter = _getShelter?.Invoke();
                var purifier = shelter?.GetModule("water_purifier");
                if (purifier != null && purifier.FilterHealth < 100f)
                {
                    lines.Add(new WorkbenchLine
                    {
                        Kind = WorkbenchActionKind.RepairPurifier,
                        Item = null,
                        SlotIndex = -1,
                        Label = "Repair Water Purifier",
                        CostSummary = $"{ElectronicScrapForPurifierRepair}x electronic_scrap",
                        CanExecute = CanRepairWaterPurifier()
                    });
                }
            }

            AppendHatchInstallLines(lines);
            return lines;
        }

        /// <summary>Install / level a hatch module (locks, blast door, traps).</summary>
        public bool CanInstallHatchUpgrade(string moduleId)
        {
            if (_hatchDefense == null || !HasOperationalWorkbench()) return false;
            return _hatchDefense.CanInstallHatchUpgrade(moduleId, _itemLookup, _inventory);
        }

        public bool InstallHatchUpgrade(string moduleId)
        {
            if (!CanInstallHatchUpgrade(moduleId)) return false;
            if (!_hatchDefense.TryInstallHatchUpgrade(moduleId, _itemLookup, _inventory)) return false;
            WearStation();
            OnHatchUpgradeInstalled?.Invoke(moduleId);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        public bool ExecuteLine(WorkbenchLine line)
        {
            if (line == null) return false;
            switch (line.Kind)
            {
                case WorkbenchActionKind.Disassemble:
                    return Disassemble(line.Item);
                case WorkbenchActionKind.Repair:
                    if (line.SlotIndex >= 0 && line.SlotIndex < _inventory.Slots.Count)
                        return Repair(_inventory.Slots[line.SlotIndex]);
                    return RepairFirst(line.Item?.id);
                case WorkbenchActionKind.Recalibrate:
                    return RecalibrateGeiger();
                case WorkbenchActionKind.RepairPurifier:
                    return RepairWaterPurifier();
                case WorkbenchActionKind.InstallHatch:
                    return InstallHatchUpgrade(line.ModuleId);
                default:
                    return false;
            }
        }

        private void AppendHatchInstallLines(List<WorkbenchLine> lines)
        {
            if (_hatchDefense == null || lines == null) return;
            var shelter = _getShelter?.Invoke();

            for (int i = 0; i < HatchDefenseSystem.HatchModuleIds.Length; i++)
            {
                string moduleId = HatchDefenseSystem.HatchModuleIds[i];
                var existing = shelter?.GetModule(moduleId);
                int targetLevel = existing != null ? existing.Level + 1 : 1;
                if (targetLevel > 5) continue;
                if (existing?.Definition != null && targetLevel > existing.Definition.MaxLevel)
                    continue;

                HatchDefenseSystem.GetUpgradeMaterialCost(moduleId, targetLevel, out int scrap, out int mech);
                string label = FormatHatchInstallLabel(moduleId, targetLevel, existing != null);
                string cost = $"{scrap}x scrap_metal, {mech}x mechanical_parts";
                lines.Add(new WorkbenchLine
                {
                    Kind = WorkbenchActionKind.InstallHatch,
                    Item = null,
                    SlotIndex = -1,
                    ModuleId = moduleId,
                    Label = label,
                    CostSummary = cost,
                    CanExecute = CanInstallHatchUpgrade(moduleId)
                });
            }
        }

        private static string FormatHatchInstallLabel(string moduleId, int targetLevel, bool upgrade)
        {
            string name = moduleId == HatchDefenseModuleSO.BlastDoorId ? "Blast Door"
                : moduleId == HatchDefenseModuleSO.HatchTrapsId ? "Hatch Traps"
                : moduleId == HatchDefenseModuleSO.ReinforcedLocksId ? "Reinforced Locks"
                : moduleId;
            return upgrade
                ? $"Upgrade hatch: {name} → L{targetLevel}"
                : $"Install hatch: {name}";
        }

        // -----------------------------------------------------------------
        // Internals
        // -----------------------------------------------------------------

        private ItemDefinition ResolveMaterial(string materialId)
        {
            return _itemLookup?.Invoke(materialId);
        }

        private bool HasScrapCosts(List<ScrapYield> costs, Survivor worker = null)
        {
            if (costs == null) return true;
            bool canSub = worker != null && _shelterPerks != null
                && _shelterPerks.CanSubstituteScrap(worker);
            for (int i = 0; i < costs.Count; i++)
            {
                var c = costs[i];
                if (c == null || c.amount <= 0) continue;
                if (CountMaterialWithSubstitute(c.materialId, canSub) < c.amount)
                    return false;
            }
            return true;
        }

        private bool ConsumeScrapCosts(List<ScrapYield> costs, Survivor worker = null)
        {
            if (!HasScrapCosts(costs, worker)) return false;
            bool canSub = worker != null && _shelterPerks != null
                && _shelterPerks.CanSubstituteScrap(worker);
            for (int i = 0; i < costs.Count; i++)
            {
                var c = costs[i];
                if (c == null || c.amount <= 0) continue;
                if (!ConsumeMaterialWithSubstitute(c.materialId, c.amount, canSub))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Count available units of a material, optionally folding in the
        /// MechanicalParts ↔ ElectronicScrap twin (#195 Jury-Rigger).
        /// </summary>
        private int CountMaterialWithSubstitute(string materialId, bool canSubstitute)
        {
            var mat = ResolveMaterial(materialId);
            int count = mat != null ? _inventory.Count(mat) : 0;
            if (!canSubstitute) return count;
            string twinId = ShelterPerkSystem.GetScrapSubstituteId(materialId);
            if (string.IsNullOrEmpty(twinId)) return count;
            var twin = ResolveMaterial(twinId);
            if (twin != null) count += _inventory.Count(twin);
            return count;
        }

        private bool ConsumeMaterialWithSubstitute(string materialId, int amount, bool canSubstitute)
        {
            if (amount <= 0) return true;
            var mat = ResolveMaterial(materialId);
            int have = mat != null ? _inventory.Count(mat) : 0;
            int fromPrimary = Mathf.Min(have, amount);
            if (fromPrimary > 0 && mat != null)
            {
                if (!_inventory.Remove(mat, fromPrimary)) return false;
            }
            int remaining = amount - fromPrimary;
            if (remaining <= 0) return true;
            if (!canSubstitute) return false;
            string twinId = ShelterPerkSystem.GetScrapSubstituteId(materialId);
            if (string.IsNullOrEmpty(twinId)) return false;
            var twin = ResolveMaterial(twinId);
            if (twin == null || _inventory.Count(twin) < remaining) return false;
            return _inventory.Remove(twin, remaining);
        }

        private ItemDefinition ResolveRareComponent(string id)
        {
            if (string.Equals(id, ShelterPerkSystem.BatteryId, StringComparison.OrdinalIgnoreCase))
                return _batteryDef ?? ResolveMaterial(ShelterPerkSystem.BatteryId);
            if (string.Equals(id, ShelterPerkSystem.SpringId, StringComparison.OrdinalIgnoreCase))
                return _springDef ?? ResolveMaterial(ShelterPerkSystem.SpringId);
            return ResolveMaterial(id);
        }

        private InventorySlot FindBrokenOrDegraded(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _inventory.Slots == null) return null;
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                var s = _inventory.Slots[i];
                if (s?.Item != null && s.Item.id == itemId && s.IsBrokenOrDegraded())
                    return s;
            }
            return null;
        }

        private void WearStation()
        {
            var station = _crafting?.GetStation(StationId);
            station?.Degrade(CraftingSystem.StationWearPerCraft * 0.2f);
        }

        private static string FormatYield(List<ScrapYield> yield)
        {
            if (yield == null || yield.Count == 0) return "—";
            var parts = new List<string>();
            for (int i = 0; i < yield.Count; i++)
                parts.Add($"+{yield[i].amount} {yield[i].materialId}");
            return string.Join(", ", parts);
        }

        private static string FormatCosts(List<ScrapYield> costs)
        {
            if (costs == null || costs.Count == 0) return "—";
            var parts = new List<string>();
            for (int i = 0; i < costs.Count; i++)
                parts.Add($"{costs[i].amount}x {costs[i].materialId}");
            return string.Join(", ", parts);
        }
    }

    public enum WorkbenchActionKind
    {
        Disassemble,
        Repair,
        Recalibrate,
        RepairPurifier,
        /// <summary>Install or level a hatch defense module (locks / blast / traps).</summary>
        InstallHatch
    }

    [Serializable]
    public class WorkbenchLine
    {
        public WorkbenchActionKind Kind;
        public ItemDefinition Item;
        public int SlotIndex;
        /// <summary>Hatch module id when <see cref="Kind"/> is <see cref="WorkbenchActionKind.InstallHatch"/>.</summary>
        public string ModuleId;
        public string Label;
        public string CostSummary;
        public bool CanExecute;
    }

    /// <summary>Future-proof save DTO for WorkbenchSystem (currently stateless).</summary>
    [Serializable]
    public class WorkbenchSystemSave
    {
        // Reserved for future mutable state (e.g. station degradation, open/close).
    }
}
