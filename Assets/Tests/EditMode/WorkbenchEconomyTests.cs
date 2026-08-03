using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Workbench scrap economy: disassemble non-consumables into component scrap,
    /// repair broken devices with scrap, recalibrate geiger with ElectronicScrap.
    /// Acceptance: disassemble broken radio → destroyed + ElectronicScrap;
    /// use scrap to repair broken Geiger.
    /// </summary>
    [TestFixture]
    public class WorkbenchEconomyTests
    {
        private Inventory _inv;
        private Dictionary<string, ItemDefinition> _items;
        private CraftingSystem _crafting;
        private WorkbenchSystem _bench;
        private readonly List<Object> _toDestroy = new List<Object>();

        private ItemDefinition MakeItem(
            string id,
            ItemType type = ItemType.Material,
            System.Action<ItemDefinition> configure = null)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = type == ItemType.Device ? 1 : 50;
            item.weight = 0.2f;
            item.disassembleYieldFraction = 0.5f;
            configure?.Invoke(item);
            _items[id] = item;
            _toDestroy.Add(item);
            return item;
        }

        [SetUp]
        public void SetUp()
        {
            _items = new Dictionary<string, ItemDefinition>();
            _inv = new Inventory { Capacity = 40, MaxWeight = 500f };
            _crafting = new CraftingSystem(_inv);
            _crafting.AddStation(new CraftingStation
            {
                id = WorkbenchSystem.StationId,
                displayName = "Workbench",
                Condition = 100f
            });

            MakeItem(ScrapMaterialIds.ElectronicScrap, ItemType.Material);
            MakeItem(ScrapMaterialIds.MechanicalParts, ItemType.Material);
            MakeItem(ScrapMaterialIds.Chemicals, ItemType.Material);

            MakeItem("handheld_radio", ItemType.Device, item =>
            {
                item.displayName = "Handheld Radio";
                item.scrapValue = new List<ScrapYield>
                {
                    new ScrapYield(ScrapMaterialIds.ElectronicScrap, 3),
                    new ScrapYield(ScrapMaterialIds.MechanicalParts, 1)
                };
                item.disassembleYieldFraction = 0.5f;
                item.repairRecipe = new RepairRecipe
                {
                    costs = new List<ScrapYield>
                    {
                        new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2)
                    }
                };
            });

            MakeItem("geiger_counter", ItemType.Device, item =>
            {
                item.displayName = "Geiger Counter";
                item.scrapValue = new List<ScrapYield>
                {
                    new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2),
                    new ScrapYield(ScrapMaterialIds.MechanicalParts, 1)
                };
                item.repairRecipe = new RepairRecipe
                {
                    costs = new List<ScrapYield>
                    {
                        new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2),
                        new ScrapYield(ScrapMaterialIds.MechanicalParts, 1)
                    }
                };
            });

            _bench = new WorkbenchSystem(
                _inv,
                id => _items.TryGetValue(id, out var def) ? def : null,
                _crafting);
        }

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
        public void Disassemble_BrokenRadio_DestroysItem_AndYieldsElectronicScrap()
        {
            var radio = _items["handheld_radio"];
            Assert.That(_inv.Add(radio, 1), Is.True);
            var slot = _inv.FindSlot("handheld_radio");
            Assert.That(slot, Is.Not.Null);
            slot.Device.Broken = true;

            Assert.That(_bench.CanDisassemble(radio), Is.True);
            Assert.That(_bench.Disassemble(radio), Is.True);

            // Destroyed
            Assert.That(_inv.Count(radio), Is.EqualTo(0), "Radio must be destroyed on disassembly");

            // 50% of scrapValue: electronic 3 → 1 (floor max 1), mechanical 1 → 1
            var eScrap = _items[ScrapMaterialIds.ElectronicScrap];
            var mParts = _items[ScrapMaterialIds.MechanicalParts];
            Assert.That(_inv.Count(eScrap), Is.GreaterThanOrEqualTo(1),
                "Disassembly must yield ElectronicScrap");
            Assert.That(_inv.Count(mParts), Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Repair_BrokenGeiger_WithScrap_ClearsBroken()
        {
            var geiger = _items["geiger_counter"];
            var eScrap = _items[ScrapMaterialIds.ElectronicScrap];
            var mParts = _items[ScrapMaterialIds.MechanicalParts];

            Assert.That(_inv.Add(geiger, 1), Is.True);
            var slot = _inv.FindSlot("geiger_counter");
            InstrumentDevice.Break(slot.Device);
            Assert.That(slot.Device.Broken, Is.True);
            Assert.That(InstrumentDevice.CanMeasure(slot.Device), Is.False);

            // Need scrap from disassembly (or stock)
            _inv.Add(eScrap, 2);
            _inv.Add(mParts, 1);

            Assert.That(_bench.CanRepair(slot), Is.True);
            Assert.That(_bench.Repair(slot), Is.True);

            Assert.That(slot.Device.Broken, Is.False, "Repair must clear hard failure");
            Assert.That(slot.Device.Calibration, Is.EqualTo(1f).Within(1e-4f));
            Assert.That(InstrumentDevice.CanMeasure(slot.Device), Is.True);
            Assert.That(_inv.Count(eScrap), Is.EqualTo(0), "ElectronicScrap consumed");
            Assert.That(_inv.Count(mParts), Is.EqualTo(0), "MechanicalParts consumed");
        }

        [Test]
        public void Acceptance_DisassembleRadio_ThenRepairGeiger_WithScrap()
        {
            var radio = _items["handheld_radio"];
            var geiger = _items["geiger_counter"];
            var eScrap = _items[ScrapMaterialIds.ElectronicScrap];
            var mParts = _items[ScrapMaterialIds.MechanicalParts];

            _inv.Add(radio, 1);
            _inv.FindSlot("handheld_radio").Device.Broken = true;
            _inv.Add(geiger, 1);
            InstrumentDevice.Break(_inv.FindSlot("geiger_counter").Device);

            // Radio scrap alone may not cover full repair (2 electronic + 1 mechanical);
            // disassemble yield is 50% → at least 1 electronic + 1 mechanical.
            Assert.That(_bench.Disassemble(radio), Is.True);
            Assert.That(_inv.Count(radio), Is.EqualTo(0));
            Assert.That(_inv.Count(eScrap), Is.GreaterThanOrEqualTo(1));

            // Top up if yield is short of full repair cost (realistic: need more junk)
            int needE = 2 - _inv.Count(eScrap);
            int needM = 1 - _inv.Count(mParts);
            if (needE > 0) _inv.Add(eScrap, needE);
            if (needM > 0) _inv.Add(mParts, needM);

            Assert.That(_bench.RepairFirst("geiger_counter"), Is.True);
            Assert.That(_inv.FindSlot("geiger_counter").Device.Broken, Is.False);
        }

        [Test]
        public void RecalibrateGeiger_RequiresElectronicScrap()
        {
            var geiger = _items["geiger_counter"];
            var eScrap = _items[ScrapMaterialIds.ElectronicScrap];
            _inv.Add(geiger, 1);
            var slot = _inv.FindSlot("geiger_counter");
            slot.Device.Calibration = 0.4f;
            slot.Device.Broken = false;

            Assert.That(_bench.CanRecalibrateGeiger(), Is.False);
            _inv.Add(eScrap, 1);
            Assert.That(_bench.CanRecalibrateGeiger(), Is.True);
            Assert.That(_bench.RecalibrateGeiger(), Is.True);
            Assert.That(slot.Device.Calibration, Is.EqualTo(1f).Within(1e-4f));
            Assert.That(_inv.Count(eScrap), Is.EqualTo(0));
        }

        [Test]
        public void Consumables_CannotDisassemble()
        {
            var food = MakeItem("canned_food", ItemType.Food);
            _inv.Add(food, 1);
            Assert.That(food.CanDisassemble, Is.False);
            Assert.That(_bench.CanDisassemble(food), Is.False);
        }

        [Test]
        public void ScrapMaterials_CannotDisassemble()
        {
            var scrap = _items[ScrapMaterialIds.ElectronicScrap];
            _inv.Add(scrap, 5);
            Assert.That(scrap.CanDisassemble, Is.False);
            Assert.That(_bench.CanDisassemble(scrap), Is.False);
        }

        [Test]
        public void WorkbenchUI_ListsDisassembleAndRepair_FromItemDefinitions()
        {
            var radio = _items["handheld_radio"];
            var geiger = _items["geiger_counter"];
            _inv.Add(radio, 1);
            _inv.Add(geiger, 1);
            InstrumentDevice.Break(_inv.FindSlot("geiger_counter").Device);
            _inv.Add(_items[ScrapMaterialIds.ElectronicScrap], 2);
            _inv.Add(_items[ScrapMaterialIds.MechanicalParts], 1);

            var go = new GameObject("WorkbenchUITest");
            try
            {
                var ui = go.AddComponent<WorkbenchUI>();
                ui.Bind(_bench);
                ui.Open();
                Assert.That(ui.IsOpen, Is.True);
                Assert.That(ui.Lines.Count, Is.GreaterThanOrEqualTo(2));

                bool hasDisassemble = false, hasRepair = false;
                for (int i = 0; i < ui.Lines.Count; i++)
                {
                    if (ui.Lines[i].Kind == WorkbenchActionKind.Disassemble
                        && ui.Lines[i].Item != null && ui.Lines[i].Item.id == "handheld_radio")
                        hasDisassemble = true;
                    if (ui.Lines[i].Kind == WorkbenchActionKind.Repair
                        && ui.Lines[i].Item != null && ui.Lines[i].Item.id == "geiger_counter")
                        hasRepair = true;
                }
                Assert.That(hasDisassemble, Is.True, "UI must list radio disassemble from ScrapValue");
                Assert.That(hasRepair, Is.True, "UI must list geiger repair from RepairRecipe");
                Assert.That(ui.PanelSummary, Does.Contain("WORKBENCH"));
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void Scavenge_PrioritizesJunk_WhenElectronicScrapNeededForCriticalRepair()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("water_purifier", 1) { FilterHealth = 10f });

            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            needsProfile.hungerPerHour = 1f;
            needsProfile.thirstPerHour = 1f;
            needsProfile.fatiguePerHour = 1f;
            _toDestroy.Add(needsProfile);

            var survivor = new Survivor { Id = "sv_scav", DisplayName = "Scav" };
            survivor.Needs.Hunger = 40f;
            survivor.Needs.Thirst = 40f;

            var action = ScriptableObject.CreateInstance<ScavengeActionSO>();
            _toDestroy.Add(action);

            var ctxNormal = new AIContext(survivor, shelter, _inv)
            {
                NeedsElectronicScrapForCriticalRepair = false,
                JunkScavengeUrgency = 0f
            };
            float scoreNormal = action.EvaluateRaw(ctxNormal);

            var ctxCritical = new AIContext(survivor, shelter, _inv)
            {
                NeedsElectronicScrapForCriticalRepair = true,
                JunkScavengeUrgency = 1f
            };
            float scoreCritical = action.EvaluateRaw(ctxCritical);

            Assert.That(scoreCritical, Is.GreaterThan(scoreNormal),
                "Scavenge must score higher when ElectronicScrap is needed for critical repair");
            Assert.That(ScavengeActionSO.PreferJunkLoot(ctxCritical), Is.True);
            Assert.That(ScavengeActionSO.PreferJunkLoot(ctxNormal), Is.False);

            // Workbench deficit detection with purifier
            var benchWithShelter = new WorkbenchSystem(
                _inv,
                id => _items.TryGetValue(id, out var def) ? def : null,
                _crafting,
                () => shelter);
            Assert.That(benchWithShelter.NeedsElectronicScrapForCriticalRepair(), Is.True);
        }

        [Test]
        public void StationOffline_BlocksDisassembleAndRepair()
        {
            var radio = _items["handheld_radio"];
            _inv.Add(radio, 1);
            var station = _crafting.GetStation(WorkbenchSystem.StationId);
            station.Condition = 0f;

            Assert.That(_bench.HasOperationalWorkbench(), Is.False);
            Assert.That(_bench.CanDisassemble(radio), Is.False);
        }
    }
}
