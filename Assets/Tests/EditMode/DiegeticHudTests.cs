using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// UI Toolkit diegetic HUD: hatch ammo/arms paint, encounter-log strip,
    /// keyboard focus path for selected ammo tooltip on the inventory strip.
    /// </summary>
    [TestFixture]
    public class DiegeticHudTests
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

        private ItemDefinition MakeItem(string id, int stackMax = 99)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = ItemType.Weapon;
            item.stackMax = stackMax;
            item.weight = 0.02f;
            _toDestroy.Add(item);
            return item;
        }

        private static bool IsPanelVisible(VisualElement panel)
        {
            if (panel == null) return false;
            if (panel.ClassListContains("hidden")) return false;
            return panel.style.display != DisplayStyle.None;
        }

        [Test]
        public void View_Build_CreatesNamedPanels()
        {
            var view = new DiegeticHudView();
            var root = view.Build();

            Assert.IsNotNull(root);
            Assert.AreEqual(DiegeticHudView.RootName, root.name);
            Assert.IsNotNull(view.HatchPanel);
            Assert.IsNotNull(view.HatchAmmo);
            Assert.IsNotNull(view.HatchArms);
            Assert.IsNotNull(view.EncounterPanel);
            Assert.IsNotNull(view.EncounterList);
            Assert.IsNotNull(view.StoresPanel);
            Assert.IsNotNull(view.StoresTooltip);

            // Hatch / stores start hidden; encounter strip is always in layout.
            Assert.IsFalse(IsPanelVisible(view.HatchPanel));
            Assert.IsFalse(IsPanelVisible(view.StoresPanel));
            Assert.IsTrue(IsPanelVisible(view.EncounterPanel));
        }

        [Test]
        public void View_PaintHatch_ShowsAmmoAndArmsWhenOpen()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintHatch(
                open: true,
                status: "HATCH [H]  DEF 12",
                ammoBreakdown: "AMMO STOCKPILE\n  9×19mm / FMJ  ×30",
                armsPreview: "ARMS PREVIEW\n  vs raiders 6.0");

            Assert.IsTrue(IsPanelVisible(view.HatchPanel));
            StringAssert.Contains("HATCH", view.HatchStatus.text);
            StringAssert.Contains("AMMO STOCKPILE", view.HatchAmmo.text);
            StringAssert.Contains("ARMS PREVIEW", view.HatchArms.text);
            Assert.IsTrue(view.HatchArms.ClassListContains("emphasis"));

            view.PaintHatch(false, null, null, null);
            Assert.IsFalse(IsPanelVisible(view.HatchPanel));
        }

        [Test]
        public void View_PaintEncounter_FillsLineList()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEncounter(
                "ENCOUNTER LOG  ·  Mara · 5.56 AP",
                new[]
                {
                    "Mara · 5.56 AP → bandit (12.0 dmg)  morale +2",
                    "Mara · 9mm JHP → warlord (2.0 dmg, soft on plate)  health -6"
                });

            StringAssert.Contains("ENCOUNTER LOG", view.EncounterStatus.text);
            Assert.AreEqual(2, view.EncounterList.childCount);
            var first = view.EncounterList[0] as Label;
            Assert.IsNotNull(first);
            StringAssert.Contains("morale +2", first.text);
        }

        [Test]
        public void View_PaintStoresFocus_MilitaryExclusiveClass()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintStoresFocus(
                show: true,
                summary: "STORES: ammo_556x45_ap ×3 [mil]",
                tooltip: "5.56×45 AP\n[MILITARY EXCLUSIVE]",
                militaryExclusive: true);

            Assert.IsTrue(IsPanelVisible(view.StoresPanel));
            StringAssert.Contains("MILITARY EXCLUSIVE", view.StoresTooltip.text);
            Assert.IsTrue(view.StoresTooltip.ClassListContains("exclusive"));
            Assert.IsTrue(view.StoresPanel.ClassListContains("exclusive-panel"));

            view.PaintStoresFocus(false, null, null, false);
            Assert.IsFalse(IsPanelVisible(view.StoresPanel));
            Assert.IsFalse(view.StoresPanel.ClassListContains("exclusive-panel"));
        }

        [Test]
        public void Controller_EnsureDocumentMounted_CreatesUIDocumentWithPanelSettings()
        {
            var go = new GameObject("DiegeticHudMount");
            _toDestroy.Add(go);
            var diegetic = go.AddComponent<DiegeticHudController>();

            // Do NOT call BuildDetachedForTests — we want the live document path.
            Assert.IsTrue(diegetic.EnsureDocumentMounted());
            diegetic.EnsureBuilt();

            Assert.IsTrue(diegetic.IsDocumentMounted);
            Assert.IsNotNull(diegetic.Document);
            Assert.IsNotNull(diegetic.Document.panelSettings);
            Assert.IsTrue(diegetic.IsBuilt);
            Assert.IsNotNull(diegetic.View);
            Assert.IsNotNull(diegetic.View.Root);
            Assert.IsNotNull(diegetic.View.HatchPanel);
            Assert.IsNotNull(diegetic.View.EncounterPanel);
            Assert.IsNotNull(diegetic.View.StoresPanel);
        }

        [Test]
        public void HUD_EnsureDiegeticHud_MountsDocument()
        {
            var go = new GameObject("HUDDiegeticMount");
            _toDestroy.Add(go);
            var hud = go.AddComponent<HUD>();

            var diegetic = hud.EnsureDiegeticHud();
            Assert.IsNotNull(diegetic);
            Assert.IsTrue(diegetic.IsDocumentMounted, "Play-mode path should mount UIDocument on HUD.");
            Assert.IsNotNull(diegetic.Document);
            Assert.IsNotNull(diegetic.Document.panelSettings);
        }

        [Test]
        public void Controller_Paint_HatchAndEncounterFromViewModels()
        {
            var go = new GameObject("DiegeticHudPaint");
            _toDestroy.Add(go);

            var hatch = go.AddComponent<HatchDefenseHUD>();
            hatch.BindAmmoUi(
                () => "AMMO STOCKPILE (test)\n  9×19mm / FMJ  ×10",
                () => "ARMS PREVIEW\n  vs military arms 4.0");
            hatch.Refresh();

            var log = go.AddComponent<ExpeditionEncounterLogHUD>();
            log.Push("Scout · 5.56 AP → raider (14.0 dmg)  morale +2");

            var strip = go.AddComponent<InventoryStripUI>();
            var diegetic = go.AddComponent<DiegeticHudController>();
            diegetic.BuildDetachedForTests();
            diegetic.BindSources(hatch, strip, log);

            // Hatch closed → panel hidden; ammo lines still live on view-model.
            Assert.IsFalse(hatch.IsOpen);
            Assert.IsFalse(IsPanelVisible(diegetic.View.HatchPanel));

            hatch.Open();
            diegetic.Paint();
            Assert.IsTrue(IsPanelVisible(diegetic.View.HatchPanel));
            StringAssert.Contains("AMMO STOCKPILE", diegetic.View.HatchAmmo.text);
            StringAssert.Contains("ARMS PREVIEW", diegetic.View.HatchArms.text);

            StringAssert.Contains("ENCOUNTER LOG", diegetic.View.EncounterStatus.text);
            Assert.GreaterOrEqual(diegetic.View.EncounterList.childCount, 1);
            var line = diegetic.View.EncounterList[0] as Label;
            Assert.IsNotNull(line);
            StringAssert.Contains("Scout", line.text);
        }

        [Test]
        public void Controller_KeyboardFocusPath_SelectNextShowsAmmoTooltip()
        {
            var go = new GameObject("DiegeticHudFocus");
            _toDestroy.Add(go);

            var hatch = go.AddComponent<HatchDefenseHUD>();
            var log = go.AddComponent<ExpeditionEncounterLogHUD>();
            var strip = go.AddComponent<InventoryStripUI>();
            strip.TooltipResolver = Item_AmmoTypes.FormatItemTooltip;
            strip.MilitaryExclusiveChecker = Item_AmmoTypes.IsMilitaryExclusiveTooltip;

            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(MakeItem("ammo_556x45_ap"), 3);
            inv.Add(MakeItem("ammo_9x19_fmj"), 8);
            strip.Sync(inv);

            var diegetic = go.AddComponent<DiegeticHudController>();
            diegetic.BuildDetachedForTests();
            diegetic.BindSources(hatch, strip, log);

            Assert.IsFalse(IsPanelVisible(diegetic.View.StoresPanel));
            Assert.AreEqual(-1, strip.SelectedIndex);

            // [I] next — cycle to first stocked icon (keyboard focus path).
            Assert.IsTrue(strip.SelectNext());
            diegetic.Paint();

            Assert.AreEqual(0, strip.SelectedIndex);
            Assert.IsTrue(IsPanelVisible(diegetic.View.StoresPanel));
            Assert.IsTrue(diegetic.TooltipPinned);
            Assert.IsNotEmpty(diegetic.View.StoresTooltip.text);

            // Military AP first in strip order when that stack is selected.
            var selected = strip.SelectedIcon;
            Assert.IsNotNull(selected);
            if (selected.ItemId == "ammo_556x45_ap")
            {
                StringAssert.Contains("MILITARY EXCLUSIVE", diegetic.View.StoresTooltip.text);
                Assert.IsTrue(diegetic.View.StoresTooltip.ClassListContains("exclusive"));
            }

            // [Shift+I] / left arrow — prev wraps.
            Assert.IsTrue(strip.SelectPrev());
            diegetic.Paint();
            Assert.AreEqual(1, strip.SelectedIndex);
            Assert.IsTrue(IsPanelVisible(diegetic.View.StoresPanel));

            // Cycle until military exclusive is focused and assert exclusive paint.
            bool foundMil = false;
            for (int i = 0; i < strip.IconCount; i++)
            {
                strip.SelectIndex(i);
                diegetic.Paint();
                if (strip.SelectedIcon != null && strip.SelectedIcon.IsMilitaryExclusive)
                {
                    foundMil = true;
                    StringAssert.Contains("MILITARY EXCLUSIVE", diegetic.View.StoresTooltip.text);
                    Assert.IsTrue(diegetic.View.StoresTooltip.ClassListContains("exclusive"));
                    break;
                }
            }
            Assert.IsTrue(foundMil, "Expected a military-exclusive ammo icon on the strip.");

            // [Esc] clear selection + stores focus.
            Assert.IsTrue(strip.ClearSelection());
            diegetic.ClearStoresFocus();
            Assert.AreEqual(-1, strip.SelectedIndex);
            Assert.IsFalse(diegetic.TooltipPinned);
            Assert.IsFalse(IsPanelVisible(diegetic.View.StoresPanel));
        }

        [Test]
        public void HUD_EnsureDiegeticHud_BindsSourcesAndPaints()
        {
            var go = new GameObject("HUDDiegetic");
            _toDestroy.Add(go);
            var hud = go.AddComponent<HUD>();

            var diegetic = hud.EnsureDiegeticHud();
            Assert.IsNotNull(diegetic);
            Assert.IsTrue(diegetic.IsBuilt);
            Assert.IsTrue(diegetic.IsDocumentMounted);
            Assert.IsNotNull(diegetic.View);
            Assert.IsNotNull(diegetic.View.Root);

            // Encounter panel always present after ensure.
            Assert.IsNotNull(diegetic.View.EncounterPanel);

            var strip = hud.InventoryStripUI;
            strip.TooltipResolver = Item_AmmoTypes.FormatItemTooltip;
            strip.MilitaryExclusiveChecker = Item_AmmoTypes.IsMilitaryExclusiveTooltip;
            var inv = new Inventory { Capacity = 5, MaxWeight = 50f };
            inv.Add(MakeItem("ammo_9x19_fmj"), 5);
            strip.Sync(inv);
            strip.SelectNext();
            hud.RefreshDiegeticHud();

            Assert.IsTrue(IsPanelVisible(diegetic.View.StoresPanel));
            StringAssert.Contains("Civilian", diegetic.View.StoresTooltip.text);
        }
    }
}
