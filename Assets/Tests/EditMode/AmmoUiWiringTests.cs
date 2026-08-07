using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Ammo UI: stockpile breakdown by caliber/mod, military exclusive tooltips,
    /// hatch arms preview vs raid armor, expedition combat → encounter log.
    /// </summary>
    [TestFixture]
    public class AmmoUiWiringTests
    {
        private readonly List<Object> _toDestroy = new List<Object>();
        private Item_AmmoTypes _ammo;

        [SetUp]
        public void SetUp()
        {
            _ammo = new Item_AmmoTypes();
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

        [Test]
        public void FormatItemTooltip_MilitaryExclusive_HasBadge()
        {
            string tip = Item_AmmoTypes.FormatItemTooltip("ammo_556x45_ap");
            Assert.IsNotEmpty(tip);
            StringAssert.Contains("MILITARY EXCLUSIVE", tip);
            StringAssert.Contains("AP", tip);
            Assert.IsTrue(Item_AmmoTypes.IsMilitaryExclusiveTooltip("ammo_556x45_ap"));
        }

        [Test]
        public void FormatItemTooltip_Civilian_WorkbenchNote()
        {
            string tip = Item_AmmoTypes.FormatItemTooltip("ammo_9x19_fmj");
            Assert.IsNotEmpty(tip);
            StringAssert.Contains("Civilian", tip);
            Assert.IsFalse(tip.Contains("MILITARY EXCLUSIVE"));
            Assert.IsFalse(Item_AmmoTypes.IsMilitaryExclusiveTooltip("ammo_9x19_fmj"));
        }

        [Test]
        public void StockpileBreakdown_GroupsByCaliberAndMod()
        {
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(MakeItem("ammo_9x19_fmj"), 30);
            inv.Add(MakeItem("ammo_9x19_jhp"), 10);
            inv.Add(MakeItem("ammo_556x45_ap"), 5);

            var rows = _ammo.BuildStockpileRows(inv);
            Assert.AreEqual(3, rows.Count);

            string text = _ammo.FormatStockpileBreakdown(inv);
            StringAssert.Contains("AMMO STOCKPILE", text);
            StringAssert.Contains("FMJ", text);
            StringAssert.Contains("JHP", text);
            StringAssert.Contains("AP", text);
            StringAssert.Contains("[MIL EXCL]", text);
            StringAssert.Contains("×30", text);
        }

        [Test]
        public void HatchPowerPreview_MilitaryArmsLowerWithSoftAmmo()
        {
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(MakeItem("ammo_9x19_jhp"), 40);

            var proxy = new HatchDefenseSystemProxy(armor =>
                _ammo.GetAmmoStockpileDefensePower("ammo_9x19_jhp", 40, armor));

            string preview = _ammo.FormatHatchPowerPreview(inv, shelterSecurity: 20f, proxy);
            StringAssert.Contains("ARMS PREVIEW", preview);
            StringAssert.Contains("vs raiders", preview);
            StringAssert.Contains("vs military", preview);
            // JHP dumps into flesh, fails on plate — preview should flag soft loads.
            StringAssert.Contains("Soft loads", preview);

            float soft = _ammo.GetAmmoStockpileDefensePower(
                "ammo_9x19_jhp", 40, Item_AmmoTypes.ArmorUnarmored);
            float mil = _ammo.GetAmmoStockpileDefensePower(
                "ammo_9x19_jhp", 40, Item_AmmoTypes.ArmorMilitary);
            Assert.Greater(soft, mil);
        }

        [Test]
        public void HatchDefenseHUD_ShowsAmmoAndArmsPreview_WhenBound()
        {
            var hatch = new HatchDefenseSystem(
                getShelter: () => new Shelter(),
                getInventory: () => new Inventory { Capacity = 5, MaxWeight = 50f },
                getSurvivors: () => new List<Survivor>(),
                getDay: () => 40);

            var go = new GameObject("HatchAmmoUi");
            _toDestroy.Add(go);
            var hud = go.AddComponent<HatchDefenseHUD>();
            hud.Bind(hatch);
            hud.BindAmmoUi(
                () => "AMMO STOCKPILE (test)\n  9×19mm / FMJ  ×10",
                () => "ARMS PREVIEW\n  vs military arms 4.0");
            hud.Refresh();

            StringAssert.Contains("AMMO STOCKPILE", hud.AmmoStockpileLine);
            StringAssert.Contains("ARMS PREVIEW", hud.ArmsPreviewLine);
            StringAssert.Contains("AMMO STOCKPILE", hud.DetailSummary);
            StringAssert.Contains("ARMS PREVIEW", hud.DetailSummary);
        }

        [Test]
        public void InventoryStrip_MilitaryExclusive_TooltipAndFlag()
        {
            var go = new GameObject("StripAmmoUi");
            _toDestroy.Add(go);
            var strip = go.AddComponent<InventoryStripUI>();
            strip.TooltipResolver = Item_AmmoTypes.FormatItemTooltip;
            strip.MilitaryExclusiveChecker = Item_AmmoTypes.IsMilitaryExclusiveTooltip;

            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(MakeItem("ammo_556x45_ap"), 3);
            inv.Add(MakeItem("ammo_9x19_fmj"), 8);
            strip.Sync(inv);

            Assert.AreEqual(2, strip.IconCount);
            InventoryIcon apIcon = null;
            InventoryIcon civIcon = null;
            for (int i = 0; i < strip.Icons.Count; i++)
            {
                if (strip.Icons[i].ItemId == "ammo_556x45_ap") apIcon = strip.Icons[i];
                if (strip.Icons[i].ItemId == "ammo_9x19_fmj") civIcon = strip.Icons[i];
            }
            Assert.IsNotNull(apIcon);
            Assert.IsNotNull(civIcon);
            Assert.IsTrue(apIcon.IsMilitaryExclusive);
            Assert.IsFalse(civIcon.IsMilitaryExclusive);
            StringAssert.Contains("MILITARY EXCLUSIVE", apIcon.Tooltip);
            StringAssert.Contains("[mil]", strip.StripSummary);
        }

        [Test]
        public void CombatLogLine_IncludesMoraleAndHealth()
        {
            string line = Item_AmmoTypes.FormatCombatEncounterLog(
                "Mara",
                "bandit_checkpoint",
                "ammo_556x45_ap",
                finalDamage: 18.5f,
                moraleDelta: 2f,
                healthDelta: 0f,
                armorPenalty: false);
            StringAssert.Contains("Mara", line);
            StringAssert.Contains("morale +2", line);
            StringAssert.Contains("18.5", line);

            string soft = Item_AmmoTypes.FormatCombatEncounterLog(
                "Mara",
                "warlord_checkpoint",
                "ammo_9x19_jhp",
                finalDamage: 2f,
                moraleDelta: 0f,
                healthDelta: -6f,
                armorPenalty: true);
            StringAssert.Contains("soft on plate", soft);
            StringAssert.Contains("health -6", soft);
        }

        [Test]
        public void ExpeditionEncounterLog_PushesToHud()
        {
            var log = new ExpeditionEncounterLog(capacity: 8);
            var go = new GameObject("EncLogHud");
            _toDestroy.Add(go);
            var hud = go.AddComponent<ExpeditionEncounterLogHUD>();

            log.OnLineAdded += hud.Push;
            log.Add("Mara · 5.56 AP → bandit (12.0 dmg)  morale +2");
            log.Add("Mara · 9mm JHP → warlord (2.0 dmg, soft on plate)  health -6");

            Assert.AreEqual(2, log.Count);
            Assert.AreEqual(2, hud.LineCount);
            StringAssert.Contains("soft on plate", hud.LatestLine);
            StringAssert.Contains("ENCOUNTER LOG", hud.StatusLine);
            StringAssert.Contains("morale +2", hud.DetailSummary);
        }

        [Test]
        public void FormatCombatEncounterLog_FromResolveHit_ApClean()
        {
            Assert.IsTrue(Item_AmmoTypes.TryGetLoad("ammo_556x45_ap", out var load));
            var hit = _ammo.ResolveHit(load.Id, load.BaseDamage, Item_AmmoTypes.ArmorLightRaider);
            string line = Item_AmmoTypes.FormatCombatEncounterLog(
                "Scout", "raider_camp", load.Id, hit.FinalDamage, 2f, 0f, hit.ArmorPenaltyApplied);
            Assert.IsFalse(string.IsNullOrEmpty(line));
            StringAssert.Contains("Scout", line);
            StringAssert.Contains("morale +2", line);
        }
    }
}
