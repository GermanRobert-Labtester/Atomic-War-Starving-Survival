using NUnit.Framework;
using AtomicWar._Game.Survivors;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class GriefKeepsakesTests
    {
        private GriefKeepsakeSystem _griefSystem;
        private Survivor _deceased;
        private Survivor _bondedFriend;
        private InterpersonalAffinity _affinity;

        [SetUp]
        public void SetUp()
        {
            _griefSystem = new GriefKeepsakeSystem();
            _deceased = new Survivor { Id = "deceased", DisplayName = "Fallen Comrade" };
            _bondedFriend = new Survivor { Id = "friend", DisplayName = "Grieving Friend" };
            _affinity = new InterpersonalAffinity();
            _affinity.Set("deceased", "friend", 80f);
        }

        [Test]
        public void OnSurvivorDied_ConvertsHighestValueItemToKeepsakeForBondedSurvivor()
        {
            _griefSystem.OnSurvivorDied(_deceased, new[] { _bondedFriend }, _affinity, "item_watch");

            Assert.IsTrue(_griefSystem.IsKeepsake(_bondedFriend, "item_watch"));
            Assert.Contains("item_watch", _bondedFriend.KeepsakeItemIds);
        }

        [Test]
        public void ForceScrapKeepsake_CausesMoraleLossAndRemovesItem()
        {
            _griefSystem.OnSurvivorDied(_deceased, new[] { _bondedFriend }, _affinity, "item_locket");
            _bondedFriend.Needs.Morale = 60f;

            var mentalBreak = new MentalBreakSystem();
            var rng = new System.Random(42);

            _griefSystem.ForceScrapKeepsake(_bondedFriend, "item_locket", mentalBreak, rng);

            Assert.IsFalse(_griefSystem.IsKeepsake(_bondedFriend, "item_locket"));
            Assert.AreEqual(20f, _bondedFriend.Needs.Morale, 0.01f);
        }
    }
}
