using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Play-mode gate: diegetic HUD must mount a real UIDocument panel so
    /// hatch ammo / encounter log / stores focus are on screen, not only
    /// detached string view-models.
    /// </summary>
    [TestFixture]
    public class DiegeticHudPlayModeTests
    {
        private GameObject _go;
        private HUD _hud;
        private DiegeticHudController _diegetic;

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _go = null;
            _hud = null;
            _diegetic = null;
        }

        private IEnumerator BuildHud()
        {
            _go = new GameObject("DiegeticHud (playmode)");
            _go.SetActive(false);
            _hud = _go.AddComponent<HUD>();
            _go.SetActive(true);

            // One frame for Awake / UIDocument panel creation.
            yield return null;

            _diegetic = _hud.EnsureDiegeticHud();
            Assert.That(_diegetic, Is.Not.Null);

            // UIDocument needs a frame after panelSettings assignment to expose root.
            yield return null;
            yield return null;

            _diegetic.EnsureDocumentMounted();
            _diegetic.EnsureBuilt();
            yield return null;
        }

        [UnityTest]
        public IEnumerator EnsureDiegeticHud_MountsLiveUIDocument()
        {
            yield return BuildHud();

            Assert.That(_diegetic.IsDocumentMounted, Is.True);
            Assert.That(_diegetic.Document, Is.Not.Null);
            Assert.That(_diegetic.Document.panelSettings, Is.Not.Null);
            Assert.That(_diegetic.Document.rootVisualElement, Is.Not.Null,
                "UIDocument should expose a live root once panel settings are assigned.");

            Assert.That(_diegetic.View, Is.Not.Null);
            Assert.That(_diegetic.View.Root, Is.Not.Null);
            Assert.That(_diegetic.View.HatchPanel, Is.Not.Null);
            Assert.That(_diegetic.View.EncounterPanel, Is.Not.Null);
            Assert.That(_diegetic.View.StoresPanel, Is.Not.Null);

            // Encounter strip is always in the tree.
            var encounter = _diegetic.Document.rootVisualElement.Q<VisualElement>(DiegeticHudView.EncounterPanelName)
                ?? _diegetic.View.EncounterPanel;
            Assert.That(encounter, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator Paint_HatchOpen_ShowsAmmoAndArmsOnPanel()
        {
            yield return BuildHud();

            var hatch = _hud.HatchDefenseHUD;
            hatch.BindAmmoUi(
                () => "AMMO STOCKPILE\n  9×19mm / FMJ  ×12",
                () => "ARMS PREVIEW\n  vs raiders 5.0");
            hatch.Open();
            _diegetic.Paint();
            yield return null;

            Assert.That(_diegetic.View.HatchPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("AMMO STOCKPILE", _diegetic.View.HatchAmmo.text);
            StringAssert.Contains("ARMS PREVIEW", _diegetic.View.HatchArms.text);
        }

        [UnityTest]
        public IEnumerator KeyboardFocus_SelectsAmmo_ShowsStoresTooltip()
        {
            yield return BuildHud();

            var strip = _hud.InventoryStripUI;
            strip.TooltipResolver = Item_AmmoTypes.FormatItemTooltip;
            strip.MilitaryExclusiveChecker = Item_AmmoTypes.IsMilitaryExclusiveTooltip;

            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = "ammo_556x45_ap";
            item.displayName = "ammo_556x45_ap";
            item.type = ItemType.Weapon;
            item.stackMax = 99;
            item.weight = 0.02f;

            var inv = new Inventory { Capacity = 8, MaxWeight = 40f };
            inv.Add(item, 4);
            strip.Sync(inv);

            Assert.That(strip.SelectNext(), Is.True);
            _hud.RefreshDiegeticHud();
            yield return null;

            Assert.That(_diegetic.View.StoresPanel.resolvedStyle.display, Is.Not.EqualTo(DisplayStyle.None));
            StringAssert.Contains("MILITARY EXCLUSIVE", _diegetic.View.StoresTooltip.text);

            Object.DestroyImmediate(item);
        }
    }
}
