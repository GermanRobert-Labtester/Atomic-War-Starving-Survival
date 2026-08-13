using NUnit.Framework;
using AtomicWar._Game.Data;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    // Lore bible 05_FACTIONS — the Currents catalog (non-territorial factions).
    [TestFixture]
    public class CurrentsCatalogTests
    {
        [Test]
        public void FourteenCurrents()
        {
            var currents = CurrentsCatalogLoader.Load();
            Assert.AreEqual(14, currents.Count, "currents.json should hold the 8 bible Currents + the 6 second-wave Currents");
        }

        [Test]
        public void AllIdsUniqueAndSnakeCase()
        {
            var currents = CurrentsCatalogLoader.Load();
            var set = new HashSet<string>();
            for (int i = 0; i < currents.Count; i++)
            {
                var c = currents[i];
                Assert.IsFalse(string.IsNullOrEmpty(c.id), $"entry {i} missing id");
                Assert.IsTrue(set.Add(c.id), $"duplicate id {c.id}");
                Assert.AreEqual(c.id, c.id.ToLowerInvariant(), $"id {c.id} not lowercase");
            }
        }

        [Test]
        public void AlignmentsRestricted()
        {
            var currents = CurrentsCatalogLoader.Load();
            var allowed = new HashSet<string> { "peaceful", "conditional", "dangerous" };
            for (int i = 0; i < currents.Count; i++)
                Assert.IsTrue(allowed.Contains(currents[i].alignment),
                    $"bad alignment '{currents[i].alignment}' on {currents[i].id}");
        }

        [Test]
        public void ThreeAlreadyCodedAreActive()
        {
            var coded = new HashSet<string> { "faction_archivists", "faction_sun_seekers", "faction_osteophages" };
            var currents = CurrentsCatalogLoader.Load();
            int active = 0;
            for (int i = 0; i < currents.Count; i++)
            {
                var c = currents[i];
                if (c.is_active)
                {
                    active++;
                    Assert.IsTrue(coded.Contains(c.id), $"{c.id} marked active but has no NPC_* class yet");
                }
            }
            Assert.AreEqual(3, active, "exactly the three coded NPC classes should be active");
        }

        [Test]
        public void OsteophagesHaveNoSignatureQuote()
        {
            var osteo = CurrentsCatalogLoader.GetById("faction_osteophages");
            Assert.IsNotNull(osteo);
            Assert.AreEqual("", osteo.signature_quote,
                "the bible gives the Osteophages no quote — they do not negotiate verbally");
        }

        [Test]
        public void SevenBadgesAdoptedOnceEach()
        {
            // Lore bible 05_FACTIONS "Second wave — the orphaned badges":
            // seven finished badge assets, adopted exactly once onto a Current.
            var seen = new Dictionary<string, string>();
            foreach (var c in CurrentsCatalogLoader.Load())
            {
                if (string.IsNullOrEmpty(c.badge_asset_id)) continue;
                Assert.IsTrue(c.badge_asset_id.StartsWith("faction_badge_") ||
                    c.badge_asset_id.StartsWith("faction_leader_"),
                    $"badge id '{c.badge_asset_id}' on {c.id} is not a Factions art id");
                string existing = seen.ContainsKey(c.badge_asset_id) ? seen[c.badge_asset_id] : "";
                Assert.IsFalse(seen.ContainsKey(c.badge_asset_id),
                    $"badge {c.badge_asset_id} adopted by both {existing} and {c.id}");
                seen[c.badge_asset_id] = c.id;
            }
            string[] expected = {
                "faction_badge_free_traders", "faction_badge_scientific_remnant",
                "faction_badge_deserter_coalition", "faction_badge_doomsday_preppers",
                "faction_badge_outcast_nomads", "faction_badge_scavenger_guild",
                "faction_badge_iron_raiders", "faction_leader_1", "faction_leader_2"
            };
            foreach (var b in expected)
                Assert.IsTrue(seen.ContainsKey(b), $"orphaned art {b} was not adopted by any Current");
        }

        [Test]
        public void BadgeAssetsExistInResources()
        {
            foreach (var c in CurrentsCatalogLoader.Load())
            {
                if (string.IsNullOrEmpty(c.badge_asset_id)) continue;
                var sprite = UnityEngine.Resources.Load<UnityEngine.Sprite>(
                    "Art/Factions/" + c.badge_asset_id);
                Assert.IsNotNull(sprite,
                    $"badge art for {c.id} ('{c.badge_asset_id}') missing from Resources/Art/Factions");
            }
        }
    }
}
