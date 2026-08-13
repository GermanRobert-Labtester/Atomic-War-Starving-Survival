using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Data;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class HoldfastCatalogTests
    {
        [Test]
        public void LocationIdsUniqueSnakeCase()
        {
            var all = HoldfastLocationsCatalogLoader.Load();
            Assert.GreaterOrEqual(all.Count, 11, "Cut spine plus Salt/Cluster/Shelf cards");
            var set = new HashSet<string>();
            for (int i = 0; i < all.Count; i++)
            {
                var e = all[i];
                Assert.IsFalse(string.IsNullOrEmpty(e.id));
                Assert.IsTrue(set.Add(e.id), "duplicate " + e.id);
                Assert.AreEqual(e.id, e.id.ToLowerInvariant());
            }
            Assert.IsNotNull(HoldfastLocationsCatalogLoader.GetById("loc_ice_road_gate"));
            Assert.IsNotNull(HoldfastLocationsCatalogLoader.GetById("loc_cut_kilometre_19"));
            Assert.IsNotNull(HoldfastLocationsCatalogLoader.GetById("loc_cut_waystation_a"));
        }

        [Test]
        public void ThreeHoldfastFactionsNotInLoreDto()
        {
            var factions = HoldfastFactionsCatalogLoader.Load();
            Assert.AreEqual(3, factions.Count);
            Assert.IsNotNull(HoldfastFactionsCatalogLoader.GetById("faction_the_office"));
            Assert.IsNotNull(HoldfastFactionsCatalogLoader.GetById("faction_the_cutters"));
            Assert.IsNotNull(HoldfastFactionsCatalogLoader.GetById("faction_the_fleet"));
        }

        [Test]
        public void TenMainQuestsRegistered()
        {
            var quests = HoldfastQuestCatalogLoader.Load();
            Assert.AreEqual(10, quests.Count);
            Assert.AreEqual(QuestlineSO.Ids.HoldfastTheSheet, HoldfastQuestSystem.Sheet);
            Assert.AreEqual(QuestlineSO.Ids.HoldfastTheHatch, HoldfastQuestSystem.Hatch);
            for (int i = 0; i < HoldfastQuestSystem.MainQuestIds.Length; i++)
                Assert.IsNotNull(HoldfastQuestCatalogLoader.GetById(HoldfastQuestSystem.MainQuestIds[i]),
                    HoldfastQuestSystem.MainQuestIds[i]);
        }

        [Test]
        public void SheetAndCensusItemsPresent()
        {
            var items = HoldfastItemsCatalogLoader.Load();
            var set = new HashSet<string>();
            for (int i = 0; i < items.Count; i++)
                set.Add(items[i].id);
            Assert.IsTrue(set.Contains("item_map_sheet_ice_road"));
            Assert.IsTrue(set.Contains("item_census_return_blank"));
            Assert.IsTrue(set.Contains("item_order_12c"));
        }

        [Test]
        public void EdorAndYaraInCharacters()
        {
            Assert.IsNotNull(CharactersCatalogLoader.GetById("npc_edor_vale"));
            Assert.IsNotNull(CharactersCatalogLoader.GetById("npc_yara_holm"));
            Assert.AreEqual("loc_weighbridge", CharactersCatalogLoader.GetById("npc_edor_vale").location_id);
            Assert.AreEqual("loc_cut_waystation_a", CharactersCatalogLoader.GetById("npc_yara_holm").location_id);
        }

        [Test]
        public void RecastsAreAlwaysOn()
        {
            var plant = HoldfastLocationsCatalogLoader.GetById("location_abandoned_desalination");
            Assert.IsNotNull(plant);
            Assert.IsTrue(plant.recast_always);
            Assert.IsTrue(plant.inspect.Contains("Occupied"));
        }
    }
}
