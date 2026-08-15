using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Core;
using UnityEngine;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Phase 2 (Scale bloc) data-gate tests.
    /// Proves the crossing catalogs parse, are collision-free within the pack,
    /// and their ids line up with the master CrossingIds constants.
    /// Pure EditMode — mirrors HoldfastCatalogTests.
    /// </summary>
    [TestFixture]
    public class CrossingCatalogTests
    {
        [Test]
        public void ThreeBlocs_CurrentsShaped_NotFactionLore()
        {
            var blocs = CrossingFactionsCatalogLoader.Load();
            Assert.AreEqual(3, blocs.Count, "Scale / Underwrite / Compact");
            Assert.IsNotNull(CrossingFactionsCatalogLoader.GetById(CrossingIds.FactionScale));
            Assert.IsNotNull(CrossingFactionsCatalogLoader.GetById(CrossingIds.FactionUnderwrite));
            Assert.IsNotNull(CrossingFactionsCatalogLoader.GetById(CrossingIds.FactionCompact));
        }

        [Test]
        public void FactionIds_SnakeCase_Unique()
        {
            var all = CrossingFactionsCatalogLoader.Load();
            var set = new HashSet<string>();
            for (int i = 0; i < all.Count; i++)
            {
                var e = all[i];
                Assert.IsFalse(string.IsNullOrEmpty(e.id));
                Assert.IsTrue(set.Add(e.id), "duplicate " + e.id);
                Assert.AreEqual(e.id, e.id.ToLowerInvariant());
            }
        }

        [Test]
        public void SevenLocations_IncludeScaleRowAndBureaucraticSpine()
        {
            var locs = CrossingLocationsCatalogLoader.Load();
            Assert.GreaterOrEqual(locs.Count, 7, "gate + row + weighbridge + underwrite + records");
            var set = new HashSet<string>();
            for (int i = 0; i < locs.Count; i++)
            {
                var e = locs[i];
                Assert.IsFalse(string.IsNullOrEmpty(e.id));
                Assert.IsTrue(set.Add(e.id), "duplicate " + e.id);
                Assert.AreEqual(e.id, e.id.ToLowerInvariant());
            }
            Assert.IsNotNull(CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.ViaductGate));
            Assert.IsNotNull(CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.Scalehouse));
            Assert.IsNotNull(CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.Stallrow));
            Assert.IsNotNull(CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.Weighbridge));
            Assert.IsNotNull(CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.Underwrite));
            Assert.IsNotNull(CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.RecordsRoom));
        }

        [Test]
        public void ScaleItems_PresentAndMaterialise()
        {
            var items = CrossingItemsCatalogLoader.Load();
            var set = new HashSet<string>();
            for (int i = 0; i < items.Count; i++)
                set.Add(items[i].id);

            Assert.IsTrue(set.Contains(CrossingIds.Items.VouchToken), "vouch token present");
            Assert.IsTrue(set.Contains(CrossingIds.Items.CalibrationWeight), "calibration weight present");

            var vouchSo = CrossingItemsCatalogLoader.Materialise(
                CrossingItemsCatalogLoader.GetById(CrossingIds.Items.VouchToken));
            Assert.IsNotNull(vouchSo);
            Assert.AreEqual(CrossingIds.Items.VouchToken, vouchSo.id);
            Assert.AreEqual(ItemType.Quest, vouchSo.type, "vouch token is a quest key");

            var weightSo = CrossingItemsCatalogLoader.Materialise(
                CrossingItemsCatalogLoader.GetById(CrossingIds.Items.CalibrationWeight));
            Assert.IsNotNull(weightSo);
            Assert.AreEqual(CrossingIds.Items.CalibrationWeight, weightSo.id);
        }

        [Test]
        public void TwoScaleQuests_Registered()
        {
            var cards = CrossingQuestCatalogLoader.Load();
            Assert.GreaterOrEqual(cards.Count, 2, "first_weigh + scale_integrity");

            var first = CrossingQuestCatalogLoader.GetById(CrossingIds.Quests.FirstWeigh);
            Assert.IsNotNull(first);
            Assert.AreEqual("quest_crossing_the_vouch", first.prereq_quest_id);
            Assert.GreaterOrEqual(first.StageCount, 1);

            var integrity = CrossingQuestCatalogLoader.GetById(CrossingIds.Quests.ScaleIntegrity);
            Assert.IsNotNull(integrity);
            Assert.AreEqual(CrossingIds.Quests.FirstWeigh, integrity.prereq_quest_id);
        }

        [Test]
        public void QuestIds_MatchMasterConstants()
        {
            var cards = CrossingQuestCatalogLoader.Load();
            for (int i = 0; i < cards.Count; i++)
            {
                var q = cards[i];
                Assert.IsFalse(string.IsNullOrEmpty(q.id));
                Assert.AreEqual(q.id, q.id.ToLowerInvariant());
            }
            Assert.AreEqual("quest_crossing_first_weigh", CrossingIds.Quests.FirstWeigh);
            Assert.AreEqual("quest_crossing_scale_integrity", CrossingIds.Quests.ScaleIntegrity);
        }

        [Test]
        public void OpeningCompanions_AtExpectedLocations()
        {
            var osran = CharactersCatalogLoader.GetById("npc_osran_kell");
            Assert.IsNotNull(osran);
            Assert.AreEqual(CrossingIds.Locations.Scalehouse, osran.location_id);

            var mattis = CharactersCatalogLoader.GetById("npc_mattis_cray");
            Assert.IsNotNull(mattis);
            Assert.AreEqual(CrossingIds.Locations.ViaductGate, mattis.location_id);
        }

        [Test]
        public void LocationMerge_IntoLiveCatalog_Deduplicates()
        {
            var catalog = ScriptableObject.CreateInstance<LocationCatalogSO>();
            try
            {
                int first = CrossingLocationsCatalogLoader.ApplyToCatalog(catalog);
                Assert.AreEqual(CrossingLocationsCatalogLoader.Load().Count, first,
                    "all non-overlay crossing locations are new to a fresh catalog");
                int second = CrossingLocationsCatalogLoader.ApplyToCatalog(catalog);
                Assert.AreEqual(0, second, "re-merge must not duplicate entries");
                Assert.IsNotNull(catalog.GetById(CrossingIds.Locations.RecordsRoom));
            }
            finally
            {
                Object.DestroyImmediate(catalog);
            }
        }

        // ── Sprint 0 — the social gate made real ──────────────────────────

        [Test]
        public void VouchCard_ExistsAndMatchesBibleSpec()
        {
            var vouch = CrossingQuestCatalogLoader.GetById(CrossingIds.Quests.TheVouch);
            Assert.IsNotNull(vouch, "quest_crossing_the_vouch must ship before first_weigh is legal");
            Assert.AreEqual("lore_nc_the_vouch", vouch.knowledge_key);
            Assert.GreaterOrEqual(vouch.min_day, 70, "bible soft gate: Day 70+ (or grievance or Ostrowski)");
            Assert.AreEqual(CrossingIds.Locations.ViaductGate, vouch.target_location_id);
            Assert.GreaterOrEqual(vouch.StageCount, 4);
        }

        [Test]
        public void VouchCard_ChoiceFlags_UseMasterListIds()
        {
            var vouch = CrossingQuestCatalogLoader.GetById(CrossingIds.Quests.TheVouch);
            Assert.IsNotNull(vouch);
            var set = new HashSet<string>();
            if (vouch.choices != null)
            {
                for (int i = 0; i < vouch.choices.Length; i++)
                {
                    var c = vouch.choices[i];
                    Assert.IsFalse(string.IsNullOrEmpty(c.set_flag));
                    set.Add(c.set_flag);
                }
            }
            Assert.IsTrue(set.Contains(CrossingIds.Flags.VouchedClean),
                "vouch choices must land on the master-list clean-vouch flag");
            Assert.IsFalse(set.Contains("flag_crossing_vouch_granted"),
                "pre-bible invented id must not return");
        }

        [Test]
        public void Locations_FitLiveSchemaBands()
        {
            var locs = CrossingLocationsCatalogLoader.Load();
            for (int i = 0; i < locs.Count; i++)
            {
                var e = locs[i];
                Assert.GreaterOrEqual(e.dangerLevel, 1f, e.id + " danger below live floor");
                Assert.LessOrEqual(e.dangerLevel, 10f, e.id + " danger above live ceiling");
                Assert.GreaterOrEqual(e.baseRadsPerHour, 18f, e.id + " rads below the bible band");
                Assert.LessOrEqual(e.baseRadsPerHour, 52f, e.id + " rads above the bible band");
            }
        }

        [Test]
        public void RecordsRoom_DoesNotSpoilTheCharter()
        {
            var e = CrossingLocationsCatalogLoader.GetById(CrossingIds.Locations.RecordsRoom);
            Assert.IsNotNull(e);
            StringAssert.DoesNotContain("three pages", e.description,
                "the Charter reveal is a later quest; the room must not spoil it");
            StringAssert.DoesNotContain("original Charter", e.description);
        }
    }
}