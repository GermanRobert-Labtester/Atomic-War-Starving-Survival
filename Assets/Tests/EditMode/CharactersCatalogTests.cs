using NUnit.Framework;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    // Lore bible 04_ENCOUNTERS Part I — recurring characters + the Lasko vote.
    [TestFixture]
    public class CharactersCatalogTests
    {
        [Test]
        public void AtLeastFourteenCharacters()
        {
            // 8 Part I + 6 Holdfast faces. Later expansions may add more (do not freeze the count).
            Assert.GreaterOrEqual(CharactersCatalogLoader.Load().Count, 14,
                "characters.json should hold the 8 Part I characters plus 6 Holdfast faces");
            Assert.IsNotNull(CharactersCatalogLoader.GetById("npc_edor_vale"));
            Assert.IsNotNull(CharactersCatalogLoader.GetById("npc_yara_holm"));
        }

        [Test]
        public void AllIdsUniqueAndSnakeCase()
        {
            var set = new HashSet<string>();
            foreach (var c in CharactersCatalogLoader.Load())
            {
                Assert.IsFalse(string.IsNullOrEmpty(c.id));
                Assert.IsTrue(set.Add(c.id), $"duplicate id {c.id}");
                Assert.AreEqual(c.id, c.id.ToLowerInvariant(), $"id {c.id} not lowercase");
            }
        }

        [Test]
        public void PellAndIanovAreLocationBound()
        {
            var pell = CharactersCatalogLoader.GetById("npc_sergeant_pell");
            var ianov = CharactersCatalogLoader.GetById("npc_doctor_ianov");
            Assert.IsNotNull(pell);
            Assert.IsNotNull(ianov);
            Assert.AreEqual("loc_conscription_office", pell.location_id);
            Assert.AreEqual("loc_veterinary_surgery", ianov.location_id);
        }

        [Test]
        public void FactionsUseEconomyNamespace()
        {
            var allowed = new HashSet<string> {
                "none", "military_remnants", "upland_militia", "cult_of_the_glow", "scavenger_camp", "hydro_barons"
            };
            foreach (var c in CharactersCatalogLoader.Load())
                Assert.IsTrue(allowed.Contains(c.faction),
                    $"character {c.id} has non-economy faction '{c.faction}'");
        }

        [Test]
        public void OstrowskiWillNotNameBuyers()
        {
            var o = CharactersCatalogLoader.GetById("npc_bram_ostrowski");
            Assert.IsNotNull(o);
            bool found = false;
            foreach (var w in o.will_not)
                if (w == "name_his_buyers") found = true;
            Assert.IsTrue(found, "the mapmaker must refuse to say who else bought the same sheet");
        }

        [Test]
        public void KestrelWantsNothing()
        {
            var k = CharactersCatalogLoader.GetById("npc_kestrel");
            Assert.IsNotNull(k);
            Assert.AreEqual(0, k.wants.Length, "Kestrel wants nothing");
        }
    }

    [TestFixture]
    public class LaskoVoteEventTests
    {
        private static GameEvent Vote() => EventRunner.CreateLaskoVoteEvent();

        [Test]
        public void ThreeBranchesMatchBibleTable()
        {
            var ev = Vote();
            Assert.AreEqual(3, ev.choices.Count);
            Assert.AreEqual(40, ev.conditions.MinDay);
            Assert.AreEqual("lasko_vote_cast", ev.conditions.BlockedFlagId);
        }

        [Test]
        public void ReturnedBranchTrustDeltasAndAftermathSchedule()
        {
            var c = Vote().choices.Find(x => x.ChoiceId == "returned");
            Assert.IsNotNull(c);
            Assert.AreEqual("military_remnants", c.FactionId);
            Assert.AreEqual(15f, c.TrustDelta, 0.001f);

            bool militiaHit = false, scheduled = false;
            foreach (var fx in c.Effects)
            {
                if (fx.FactionId == "upland_militia" && fx.TrustDelta == -15f) militiaHit = true;
                if (fx.ScheduleEventId == "event_lasko_aftermath" && fx.ScheduleDelayDays == 11) scheduled = true;
            }
            Assert.IsTrue(militiaHit, "returned must cost Militia trust");
            Assert.IsTrue(scheduled, "returned must schedule the eleven-day aftermath");
        }

        [Test]
        public void ShelteredBranchIsTheInverse()
        {
            var c = Vote().choices.Find(x => x.ChoiceId == "sheltered");
            Assert.IsNotNull(c);
            Assert.AreEqual(-15f, c.TrustDelta, 0.001f);
            bool militiaGain = false;
            foreach (var fx in c.Effects)
                if (fx.FactionId == "upland_militia" && fx.TrustDelta == 15f) militiaGain = true;
            Assert.IsTrue(militiaGain, "sheltered must raise Militia trust");
        }

        [Test]
        public void AbstainedBranchNotesBothFactions()
        {
            var c = Vote().choices.Find(x => x.ChoiceId == "abstained");
            Assert.IsNotNull(c);
            Assert.AreEqual(-8f, c.MoraleDelta, 0.001f);
            var deltas = new Dictionary<string, float>();
            deltas[c.FactionId] = c.TrustDelta;
            foreach (var fx in c.Effects)
                if (!string.IsNullOrEmpty(fx.FactionId)) deltas[fx.FactionId] = fx.TrustDelta;
            Assert.AreEqual(-6f, deltas["military_remnants"], 0.001f);
            Assert.AreEqual(-6f, deltas["upland_militia"], 0.001f);
        }

        [Test]
        public void AftermathEventIsShortAndGatedOnReturned()
        {
            var ev = EventRunner.CreateLaskoAftermathEvent();
            Assert.AreEqual("event_lasko_aftermath", ev.id);
            Assert.AreEqual("lasko_returned", ev.conditions.RequiredFlagId);
            Assert.AreEqual(1, ev.choices.Count);
        }
    }
}
